using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impact : TowerComponent
{
    public void SelfReturnToPool()
    {
        this.gameObject.SetActive(false);
    }
}
