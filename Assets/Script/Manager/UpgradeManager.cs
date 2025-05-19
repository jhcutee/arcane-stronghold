using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;
    [Header("Elements")]
    [SerializeField] private GameObject upgradeCanvas;
    [Header("Upgrades")]
    [SerializeField] private GameObject upgradeButton;
    [SerializeField] private Image upgradeTowerImage;
    [SerializeField] private TextMeshProUGUI crystalPrice;
    [SerializeField] private TextMeshProUGUI magebookPrice;
    [SerializeField] private Button sellButton;
    private Node nodeSelected;
    private bool isSelected;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SetNode(Node node)
    {
        nodeSelected = node;
    }
    public void HideUpgradeCanvas()
    {
        upgradeCanvas.SetActive(false);
        isSelected = false;
    }
    public void ToogleUpgradeCanvas()
    {
        isSelected = !isSelected;
        if (isSelected)
        {
            upgradeCanvas.SetActive(true);
            Vector2 nodePositon = nodeSelected.transform.position;
            upgradeCanvas.transform.position = nodePositon;
            Tower tower = nodeSelected.towerPlace.GetComponentInChildren<Tower>();
            if (tower.CurrentTowerLevel >= tower.TowerSo.maxUpgradeLevel)
            {
                upgradeButton.SetActive(false);
            }
            else
            {
                upgradeTowerImage.sprite = tower.TowerSo.towerIcon[tower.CurrentTowerLevel];
                crystalPrice.text = tower.TowerSo.upgradeCrystalCost[tower.CurrentTowerLevel - 1].ToString();
                magebookPrice.text = tower.TowerSo.upgradeMageBookCost[tower.CurrentTowerLevel - 1].ToString();
                upgradeButton.SetActive(true);
                upgradeButton.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
                if(CheckUpgradeTower(tower))
                    upgradeButton.GetComponentInChildren<Button>().onClick.AddListener(() => tower.UpgradeTower(tower));
                else
                {
                    upgradeButton.GetComponentInChildren<Button>().onClick.AddListener(() => UIManager.instance.ShowLackOfResourcesUpdate());
                }
                sellButton.onClick.RemoveAllListeners();
                sellButton.onClick.AddListener(() => HandleSellButton(tower));
            }
        }
        else
        {
            upgradeCanvas.SetActive(false);
        }
    }

    public bool CheckUpgradeTower(Tower tower)
    {
        bool hasEnoughCrystals = false;
        bool hasEnoughMageBooks = false;

        int currentLevelIndex = tower.CurrentTowerLevel - 1;

        if (CurrencyManager.Instance.CrytstalAmount >= tower.TowerSo.upgradeCrystalCost[currentLevelIndex])
        {
            hasEnoughCrystals = true;
            crystalPrice.color = Color.white;
        }
        else
        {
            crystalPrice.color = Color.red;
        }

        if (CurrencyManager.Instance.MageBookAmount >= tower.TowerSo.upgradeMageBookCost[currentLevelIndex])
        {
            hasEnoughMageBooks = true;
            magebookPrice.color = Color.white;
        }
        else
        {
            magebookPrice.color = Color.red;
        }

        return hasEnoughCrystals && hasEnoughMageBooks;
    }
    private void HandleSellButton(Tower tower)
    {
        int towerValue = Mathf.CeilToInt( tower.towerValue * 70/100);
        CurrencyManager.Instance.AddCrystal(towerValue);
        Destroy(tower.gameObject);
        HideUpgradeCanvas();    
    }
}
