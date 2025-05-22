using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type
{
    None,
    Circle = 1,
    Triangle = 2,
    Square = 3
}
public enum TypeColor
{
    None,
    Blue = 1,
    Green = 2,
    Yellow = 3,
    Red = 4
}
public enum TypeImage
{
    None,
    Car = 1,
    Flower = 2,
    Animal = 3,
    Food = 4
}
public class Element
{
    private Type elementType;
    private TypeColor elementColor;
    private TypeImage elementImage;

    public Element(Type elementType)
    {
        this.elementType = elementType;
    }

    public void SetElementTypes(int indexType)
    {
        elementColor = (TypeColor)indexType;
        elementImage = (TypeImage)indexType;
    }
    public TypeImage GetTypeImage() { return elementImage; }
    public bool ÑheckingForMatch(Element otherElement)
    {
        return (elementType == otherElement.elementType) && (elementColor == otherElement.elementColor) && (elementImage == otherElement.elementImage);
    }

    public bool IsExplosive() { if(elementImage == TypeImage.Food) return true; 
        return false; }
}
