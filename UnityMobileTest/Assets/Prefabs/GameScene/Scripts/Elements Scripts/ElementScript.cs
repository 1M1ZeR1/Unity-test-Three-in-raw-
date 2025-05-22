using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementScript : MonoBehaviour
{
    private enum SkillType
    {
        None,
        Sticky,
        Heavy,
        Explosive,

    }

    protected bool _blockedClick = false;

    [SerializeField] private GameObject actionboardControllerObject;
    private ActionBoardControllerScript actionboardControllerScript;

    private TypeImage typeImage = TypeImage.None;

    private void Awake()
    {
        actionboardControllerObject.TryGetComponent(out actionboardControllerScript);
    }
    private void OnMouseDown()
    {
        if (_blockedClick) return;
        actionboardControllerScript.AddElementToActionBoard(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(typeImage == TypeImage.Flower && collision.gameObject.CompareTag("Element")) 
        {
            if (collision.gameObject.GetComponent<ElementScript>().GetTypeImage() != TypeImage.Flower)
            {
                FixedJoint2D fixedJoint2D = collision.gameObject.AddComponent<FixedJoint2D>();

                fixedJoint2D.connectedBody = GetComponent<Rigidbody2D>();
                fixedJoint2D.enableCollision = false;
            }
        }
    }

    public void SetTypeImage(TypeImage typeImage) { this.typeImage = typeImage; UseElementSkill(); }
    public TypeImage GetTypeImage (){ return typeImage; }
    private void UseElementSkill()
    {
        switch(typeImage)
        {
            case TypeImage.Car:
                GetComponent<Rigidbody2D>().gravityScale = 5;
                break;
            case TypeImage.Animal:
                _blockedClick = true;
                actionboardControllerScript.unlockElements += () => { _blockedClick = false; };
                break;
        }
    }
}
