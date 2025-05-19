using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [Header("Elements")]
    [SerializeField] private GameObject towerPrefab;
    public TowerType towerType;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Button thisButton;
    [SerializeField] private Image towerImage;
    [SerializeField] private Sprite towerIcon;
    [SerializeField] private GameObject towerPreviewPrefab;
    [SerializeField] private GameObject LockImage;
    [SerializeField] private GameObject Selector;
    private GameObject tempTowerObj;
    public bool unlocked = false;
    [HideInInspector] public bool isSelected = false;

    public void Unlock(TowerInfoUnLocked towerInfo)
    {
        LockImage.SetActive(false);
        towerImage.color = Color.white;
        towerIcon = towerInfo.towerSprite;
        unlocked = true;
    }

    public void Lock()
    {
        LockImage.SetActive(true);
    }

    public void Select()
    {
        Selector.SetActive(true);
    }

    public void Deselect()
    {
        Selector.SetActive(false);
    }

    public Button GetButton()
    {
        return this.thisButton;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!ShopManager.Instance.canBuy)
        {
            UIManager.instance.ShowLackOfResourcesBuild();
            return;
        }
        if (!unlocked)
        {
            UIManager.instance.ShowLockedDialog();
            return;
        }

        if (isSelected)
        {
            tempTowerObj = Instantiate(towerPreviewPrefab, canvas.transform);
            tempTowerObj.GetComponent<Image>().sprite = towerIcon;
            tempTowerObj.transform.SetAsLastSibling();
            tempTowerObj.GetComponent<RectTransform>().sizeDelta = new Vector2(76, 153);

            eventData.pointerDrag = this.gameObject;

            UpdatePreviewPosition(eventData);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!ShopManager.Instance.canBuy)
        {
            return;
        }

        if (unlocked && tempTowerObj != null && isSelected)
        {
            UpdatePreviewPosition(eventData);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!ShopManager.Instance.canBuy)
        {
            return;
        }

        if (unlocked && tempTowerObj != null && isSelected)
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("Node") && hit.collider.GetComponent<Node>().towerPlace.childCount == 0)
            {
                GameObject newTower = Instantiate(towerPrefab, hit.collider.GetComponent<Node>().towerPlace.position, Quaternion.identity);
                newTower.name = towerPrefab.name;
                newTower.transform.SetParent(hit.collider.GetComponent<Node>().towerPlace);
                Tower tower = newTower.GetComponent<Tower>();
                tower.towerValue += tower.TowerSo.towerCrytalCost;
                CurrencyManager.Instance.RemoveCrystal(tower.TowerSo.towerCrytalCost);
                ShopManager.Instance.UpdateInfoUI();
            }

            Destroy(tempTowerObj);
        }
    }

    private void UpdatePreviewPosition(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out Vector2 localPoint
        );

        tempTowerObj.GetComponent<RectTransform>().anchoredPosition = localPoint;
    }

    public GameObject GetTowerPrefab()
    {
        return towerPrefab;
    }
}
