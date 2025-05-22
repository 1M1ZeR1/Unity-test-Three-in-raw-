using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ActionBoardControllerScript : MonoBehaviour
{
    [Header("ß÷åéêè")]
    [SerializeField] private GameObject[] cells;

    [SerializeField] private int deletingToUnlock;

    protected Dictionary<GameObject, bool> _isPanelFiled = new Dictionary<GameObject, bool>();
    protected Dictionary<GameObject, Element> _objectToClass = new Dictionary<GameObject, Element>();
    protected Dictionary<GameObject, GameObject> _elementToCell = new Dictionary<GameObject, GameObject>();

    public delegate void UnlockLockedElements();
    public event UnlockLockedElements unlockElements;

    private bool inBlockedClickMode = true;

    protected int _score = 0;

    private void Start()
    {
        foreach(var cell in cells)
        {
            _isPanelFiled.Add(cell.gameObject, false);
        }   
    }
    public void AddToObjectClassDictionary(GameObject newObject, Element elementInstance)
    {
        _objectToClass.Add(newObject, elementInstance);
    }
    public void UnBlockClicks() { inBlockedClickMode = false; }
    public void AddElementToActionBoard(GameObject element)
    {
        if(inBlockedClickMode) return;
        foreach (var panel in _isPanelFiled)
        {
            if (!panel.Value) 
            {
                if (element.TryGetComponent(out Rigidbody2D rg))
                {
                    if (_objectToClass[element].GetTypeImage() == TypeImage.Flower) { TieBreakerScript.BreakAllTiesForFlower(rg); }
                    else
                    {
                        TieBreakerScript.BreakAllTiesForOther(rg.gameObject);
                    }

                    rg.isKinematic = true;
                    rg.angularVelocity = 0f;
                    rg.velocity = Vector2.zero;
                }

                element.transform.position = new Vector3(panel.Key.transform.position.x, panel.Key.transform.position.y,element.transform.position.z);
                element.transform.rotation = Quaternion.identity;

                GameObject selectedPanel = panel.Key;

                _elementToCell.Add(element, selectedPanel);
                _isPanelFiled[selectedPanel] = true;

                CheckingCoincidence();
                
                break; 
            }
        }
    }

    private void CheckingCoincidence()
    {
        if (_elementToCell.Count < 3) return;

        var elementsObject = _elementToCell.Keys.ToArray();

        for (int i = 0; i < elementsObject.Length - 1; i++)
        {
            List<GameObject> matchedElements = new List<GameObject>()
            {
                elementsObject[i]
            };

            GameObject currentWorkingElement = elementsObject[i];
            int coincidences = 1;

            for (int j = i + 1; j < elementsObject.Length; j++)
            {
                if (_objectToClass[currentWorkingElement].ÑheckingForMatch(_objectToClass[elementsObject[j]]))
                {
                    coincidences++;
                    matchedElements.Add(elementsObject[j]);
                }

                if (coincidences == 3)
                {
                    deletingToUnlock--;
                    if(deletingToUnlock == 0) { if (unlockElements != null) { unlockElements.Invoke(); } }

                    List<GameObject> toRemove = new List<GameObject>(matchedElements);

                    if (_objectToClass[toRemove[0]].IsExplosive()) { DropElements(toRemove); }

                    foreach (var element in toRemove)
                    {
                        _objectToClass.Remove(element);
                        _isPanelFiled[_elementToCell[element]] = false;
                        _elementToCell.Remove(element);
                        Destroy(element);
                    }

                    _score++;

                    if(_objectToClass.Count <= 0) { InvokeEnd(true); }

                    return;
                }
            }
        }
        if (_elementToCell.Count == 7) { InvokeEnd(false); }
    }
    private void InvokeEnd(bool win)
    {
        PlayerPrefs.SetInt("Score", _score);

        if (win){PlayerPrefs.SetString("GameResult", "Win");}
        else { PlayerPrefs.SetString("GameResult", "Lose"); }

        SceneManager.LoadScene("MenuScene");
    }
    public List<GameObject> GetAllElementsOnField()
    {
        List<GameObject> result = new List<GameObject>();

        foreach(var element in _objectToClass.Keys) 
        {
            if (!_elementToCell.ContainsKey(element))
            {
                result.Add(element);
            }
        }

        return result;
    }
    private void DropElements(List<GameObject> explosiveElements)
    {
        var elemetsToDrop = new List<GameObject>();

        foreach(var elementInCell in _elementToCell)
        {
            if (!explosiveElements.Contains(elementInCell.Key)){
                elemetsToDrop.Add(elementInCell.Key);
            }
        }

        foreach(var element in elemetsToDrop)
        {
            _isPanelFiled[_elementToCell[element]] = false;
            _elementToCell.Remove(element);

            Rigidbody2D rg = element.GetComponent<Rigidbody2D>();

            rg.isKinematic = false;
            rg.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        }
    }
}
