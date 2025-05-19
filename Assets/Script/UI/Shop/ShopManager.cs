using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;
    [Header("Elements")]
    [SerializeField] private TowerItem[] towerItems;
    [SerializeField] private TowerUnlocked[] towersUnlocked;

    [Header("TowerInfo")]
    [SerializeField] private GameObject info;
    [SerializeField] private TextMeshProUGUI towerName;
    [SerializeField] private TextMeshProUGUI dmgPerLevel;
    [SerializeField] private TextMeshProUGUI specialAbility;
    [SerializeField] private TextMeshProUGUI crystalCost;
    [SerializeField] private GameObject comingSoon;
    private TowerType towerSelected;
    private bool isTowerSelectedUnlocked = false;
    public bool canBuy = false;
    int currentLevel;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
        
        info.SetActive(false);
        comingSoon.SetActive(false);
    }
    private void Start()
    {
        currentLevel = LevelManager.instance.CurrentLevel;
        UnlockTowers();
    }
    private void UnlockTowers()
    {
        for (int i = 0; i < towerItems.Length; i++)
        {
            if (i < towersUnlocked[currentLevel-1].towersUnlocked.Length &&
                towersUnlocked[currentLevel - 1].towersUnlocked[i].towerType == towerItems[i].towerType)
            {
                towerItems[i].Unlock(towersUnlocked[currentLevel - 1].towersUnlocked[i]);
            }
            else
            {
                towerItems[i].Lock();
            }
        }
    }

    public void SelectTower(int index)
    {
        for (int i = 0; i < towerItems.Length; i++)
        {
            if (i == index)
            {
                towerItems[i].Select();
                towerItems[i].isSelected = true;
                towerSelected = towerItems[i].towerType;
                isTowerSelectedUnlocked = towerItems[i].unlocked;
            }
            else
            {
                towerItems[i].Deselect();
                towerItems[i].isSelected = false;
            }
        }
        ShowInfo(towerSelected, isTowerSelectedUnlocked);
    }

    private void ShowInfo(TowerType towerType, bool unlocked)
    {
        if (unlocked)
        {
            info.SetActive(true);
            comingSoon.SetActive(false);
            for (int i = 0; i < towersUnlocked[currentLevel - 1].towersUnlocked.Length; i++)
            {
                if (towersUnlocked[currentLevel - 1].towersUnlocked[i].towerType == towerType)
                {
                    towerName.text = FormatTowerName(towersUnlocked[currentLevel - 1].towersUnlocked[i].towerType.ToString());
                    dmgPerLevel.text = $"Damage: {string.Join("/", towersUnlocked[currentLevel - 1].towersUnlocked[i].dmgPerLevel)}";
                    specialAbility.text = "Ability: " + towersUnlocked[currentLevel - 1].towersUnlocked[i].specialAbillity;
                    canBuy = CurrencyManager.Instance.CrytstalAmount >= towersUnlocked[currentLevel - 1].towersUnlocked[i].crystalCost;
                    crystalCost.color = canBuy ? Color.white : Color.red;
                    crystalCost.text = "Price: " + towersUnlocked[currentLevel - 1].towersUnlocked[i].crystalCost;
                }
            }
        }
        else
        {
            info.SetActive(false);
            comingSoon.SetActive(true);
        }
    }

    private string FormatTowerName(string towerName)
    {
        return System.Text.RegularExpressions.Regex.Replace(towerName, "(?<!^)([A-Z])", " $1");
    }
    public void UpdateInfoUI()
    {
        CheckCanBuy();
        crystalCost.color = canBuy ? Color.white : Color.red;
    }
    public void CheckCanBuy()
    {
        for (int i = 0; i < towersUnlocked[currentLevel - 1].towersUnlocked.Length; i++)
        {
            if (towersUnlocked[currentLevel - 1].towersUnlocked[i].towerType == towerSelected)
            {
                canBuy = CurrencyManager.Instance.CrytstalAmount >= towersUnlocked[currentLevel - 1].towersUnlocked[i].crystalCost;
                
            }
        }
    }
}

