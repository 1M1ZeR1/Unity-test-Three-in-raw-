using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ElementsSpawnerScript : MonoBehaviour
{
    [Header("Падающие элементы")]
    [SerializeField] private GameObject[] elementsObject;

    [SerializeField] private float timeToSpawn;

    [Header("Основной контроллер")]
    [SerializeField] private GameObject actionboardControllerObject;
    private ActionBoardControllerScript actionBoardControllerScript;

    [Header("Спрайты для элементов")]
    [SerializeField] private Sprite[] sprites;

    private List<GameObject> listToRandom = new List<GameObject>();

    protected bool _inRebuilding = true;

    void Start()
    {
        actionboardControllerObject.TryGetComponent(out actionBoardControllerScript);

        StartCoroutine(SpawnFunction());
    }
    private IEnumerator SpawnFunction()
    {
        Dictionary<GameObject,Element> abstractElements = new Dictionary<GameObject, Element>();

        for (int k = 0; k < 3; k++)
        {
            for (int i = 0; i < 4; i++)
            {
                Element element;

                for (int j = 0; j < elementsObject.Length; j++)
                {
                    GameObject newObject = Instantiate(elementsObject[j], transform.parent);
                    listToRandom.Add(newObject);

                    element = new Element((Type)j + 1);

                    element.SetElementTypes(i + 1);

                    abstractElements.Add(newObject,element);

                    actionBoardControllerScript.AddToObjectClassDictionary(newObject, element);
                }
            }
        }

        listToRandom = listToRandom.OrderBy(x => Random.value).ToList();

        foreach(var newObject in listToRandom)
        {
            newObject.SetActive(true);
            SpritesController(newObject, abstractElements[newObject].GetTypeImage());

            yield return new WaitForSeconds(timeToSpawn);
        }

        listToRandom.Clear();
        abstractElements.Clear();

        actionBoardControllerScript.UnBlockClicks();
        _inRebuilding = false;
    }
    public void RebuildField()
    {
        if (_inRebuilding) return;

        _inRebuilding = true;

        listToRandom = actionBoardControllerScript.GetAllElementsOnField();

        foreach (var element in listToRandom) { element.SetActive(false); }

        listToRandom = listToRandom.OrderBy(x => Random.value).ToList();

        StartCoroutine(RebuildingField());
    }
    private IEnumerator RebuildingField()
    {
        foreach (var element in listToRandom)
        {
            TieBreakerScript.BreakAllTiesForEveryone(element);

            element.transform.position = new Vector3(0, 2, element.transform.position.z);
            element.SetActive(true);

            yield return new WaitForSeconds(timeToSpawn);
        }

        _inRebuilding = false;
    }
    private void SpritesController(GameObject element, TypeImage typeImage)
    {
        SpriteRenderer elementSpriteRenderer = element.GetComponent<SpriteRenderer>();
        SpriteRenderer imageSpriteRenderer = element.transform.GetChild(0).GetComponent<SpriteRenderer>();

        ElementScript elementScript = element.GetComponent<ElementScript>();

        switch(typeImage)
        {
            case TypeImage.Car:elementSpriteRenderer.color = Color.blue;
                imageSpriteRenderer.sprite = sprites[0];
                elementScript.SetTypeImage(TypeImage.Car);
                break;
            case TypeImage.Flower:
                elementSpriteRenderer.color = Color.green;
                imageSpriteRenderer.sprite = sprites[1];
                elementScript.SetTypeImage(TypeImage.Flower);
                break;
            case TypeImage.Animal:
                elementSpriteRenderer.color = Color.yellow;
                imageSpriteRenderer.sprite = sprites[2];
                elementScript.SetTypeImage(TypeImage.Animal);
                break;
            case TypeImage.Food:
                elementSpriteRenderer.color = Color.red;
                imageSpriteRenderer.sprite = sprites[3];
                elementScript.SetTypeImage(TypeImage.Food);
                break;

        }
    }
}
