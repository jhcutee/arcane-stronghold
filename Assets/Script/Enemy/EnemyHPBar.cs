using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image fillAmountHp;
    public Image FillAmountHp => fillAmountHp;
}
