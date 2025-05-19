using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpellItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [Header("Elements")]
    [SerializeField] private GameObject spellPrefab;
    [SerializeField] public SpellInfo spellInfo;
    [SerializeField] private GameObject spellPreviewPrefab;
    [SerializeField] private Canvas canvas;

    private GameObject tempSpellObj;
    public bool isCooldown = false;
    public int spellAmount;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(!CanUseSpell()) return;
        tempSpellObj = Instantiate(spellPreviewPrefab, canvas.transform);
        tempSpellObj.GetComponent<Image>().sprite = spellInfo.spellSprite;
        tempSpellObj.transform.SetAsLastSibling();
        tempSpellObj.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);

        UpdatePreviewPosition(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (tempSpellObj != null && CanUseSpell())
        {
            UpdatePreviewPosition(eventData);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (tempSpellObj != null && CanUseSpell())
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);

            RaycastHit2D[] hits = Physics2D.RaycastAll(worldPos, Vector2.zero);
            bool isValidPosition = false;
            foreach (var hit in hits)
            {
                if (hit.collider != null && hit.collider.CompareTag("EnemyLane"))
                {
                    isValidPosition = true;
                    break;
                }
            }

            if (isValidPosition)
            {
                GameObject newSpell = Instantiate(spellPrefab, worldPos, Quaternion.identity);
                newSpell.name = spellPrefab.name;
                newSpell.SetActive(true);
                SpellManager.Instance.UseSpell(this);
            }
            Destroy(tempSpellObj);
        }
    }
    public bool CanUseSpell()
    {
        return spellAmount > 0 && !isCooldown;
    }
    private void UpdatePreviewPosition(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out Vector2 localPoint
        );

        tempSpellObj.GetComponent<RectTransform>().anchoredPosition = localPoint;
    }
}
