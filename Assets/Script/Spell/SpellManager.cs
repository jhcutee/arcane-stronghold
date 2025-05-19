using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellManager : MonoBehaviour
{
    private static SpellManager instance;
    [Header("Elements")]
    [Header("Spells")]
    public List<Spell> spells = new List<Spell>();
    public Color spellUnavailableColor;
    public static SpellManager Instance { get => instance; }
    private void Awake()
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
    private void Start()
    {
        LoadSpellAmounts();

        foreach (var spell in spells)
        {
            UpdateSpellUI(spell);
        }
    }

    private void OnApplicationQuit()
    {
        SaveSpellAmounts();
    }

    public void UseSpell(SpellItem spellitem)
    {
        foreach(var spell in spells)
        {
            if (spell.spellItem == spellitem)
            {
                spell.spellItem.spellAmount--;

                StartCoroutine(HandleCooldown(spell));

                UpdateSpellUI(spell);
            }
        }
    }

    private IEnumerator HandleCooldown(Spell spell)
    {
        spell.spellItem.isCooldown = true;
        float cooldownTime = spell.spellItem.spellInfo.spellCooldownTime;

        float elapsedTime = 0f;
        while (elapsedTime < cooldownTime)
        {
            elapsedTime += Time.deltaTime;

            float fillAmount = 1f - (elapsedTime / cooldownTime);
            spell.imageCooldown.fillAmount = fillAmount;
            spell.cooldownText.text = Mathf.CeilToInt(cooldownTime - elapsedTime).ToString();

            yield return null;
        }

        spell.spellItem.isCooldown = false;
        spell.imageCooldown.fillAmount = 0f;
        spell.cooldownText.text = "";
    }

    private void UpdateSpellUI(Spell spell)
    {
        spell.spellAmountText.text = $"x{spell.spellItem.spellAmount}";
        if(spell.spellItem.spellAmount <= 0)
        {
            spell.spellItem.GetComponent<Image>().color = spellUnavailableColor;
        }
        if (!spell.spellItem.isCooldown)
        {
            spell.imageCooldown.fillAmount = 0f;
            spell.cooldownText.text = "";
        }
    }

    public void SaveSpellAmounts()
    {
        for (int i = 0; i < spells.Count; i++)
        {
            PlayerPrefs.SetInt($"SpellAmount_{i}", spells[i].spellItem.spellAmount);
        }
        PlayerPrefs.Save();
    }

    public void LoadSpellAmounts()
    {
        for (int i = 0; i < spells.Count; i++)
        {
            if (PlayerPrefs.HasKey($"SpellAmount_{i}"))
            {
                spells[i].spellItem.spellAmount = PlayerPrefs.GetInt($"SpellAmount_{i}");
            }
        }
    }
    public void AddSpellAmount(SpellItem spellItem)
    {
        foreach (var spell in spells)
        {
            if (spell.spellItem == spellItem)
            {
                spell.spellItem.spellAmount++;
                UpdateSpellUI(spell);
                SaveSpellAmounts();
                break;
            }
        }
    }
}
[System.Serializable]
public class Spell
{
    public SpellItem spellItem;
    public Image imageCooldown;
    public TextMeshProUGUI cooldownText;
    public TextMeshProUGUI spellAmountText;
}
