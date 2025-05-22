using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class TieBreakerScript : MonoBehaviour
{
    public static void BreakAllTiesForOther(GameObject element)
    {
        var allFixedJoints = element.GetComponents<FixedJoint2D>();
        if (allFixedJoints.Length > 0)
        {
            foreach (var joint in allFixedJoints) { Destroy(joint); }
        }
    }
    public static void BreakAllTiesForFlower(Rigidbody2D rg)
    {
        FixedJoint2D[] allFixedObjects = FindObjectsByType<FixedJoint2D>(FindObjectsSortMode.None);
        var componentsToDelete = new List<FixedJoint2D>();

        foreach (FixedJoint2D fix in allFixedObjects)
        {
            if (fix.connectedBody == rg)
            {
                fix.connectedBody = null;
                componentsToDelete.Add(fix);
            }
        }

        foreach (var componentToDelete in componentsToDelete) { Destroy(componentToDelete); }
    }
    public static void BreakAllTiesForEveryone(GameObject element)
    {
        var rg = element.GetComponent<Rigidbody2D>();
        var allFixedJoints = element.GetComponents<FixedJoint2D>();
        if (allFixedJoints.Length > 0)
        {
            foreach (var joint in allFixedJoints) { Destroy(joint); }
        }
        else
        {
            FixedJoint2D[] allFixedObjects = FindObjectsByType<FixedJoint2D>(FindObjectsSortMode.None);
            var componentsToDelete = new List<FixedJoint2D>();

            foreach (FixedJoint2D fix in allFixedObjects)
            {
                if (fix.connectedBody == rg)
                {
                    fix.connectedBody = null;
                    componentsToDelete.Add(fix);
                }
            }

            foreach (var componentToDelete in componentsToDelete) { Destroy(componentToDelete); }
        }
    }
}
