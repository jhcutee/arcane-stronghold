using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    [Header("Elemets")]
    public Transform towerPlace;
    private void OnMouseDown()
    {
        if(towerPlace.childCount > 0)
        {
            UpgradeManager.instance.SetNode(this);
            UpgradeManager.instance.ToogleUpgradeCanvas();
        }
    }
}
