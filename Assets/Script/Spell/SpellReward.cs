using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpellReward : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject rewardPanel;
    [SerializeField] private Button rewardButton;
    [SerializeField] private Image rewardSpell1;
    [SerializeField] private Image rewardSpell2;

    [Header("Spells")]
    [SerializeField] private SpellItem[] spells;
    private void Awake()
    {
        rewardPanel.SetActive(false);
    }
    public void GetSpellReward()
    {
        AdsManager.Instance.ShowRewardedAd(
        onRewardEarned: null,
        onAdClosed: ShowRewardPanel
    );

    }
    public void ShowRewardPanel()
    {
        rewardPanel.SetActive(true);
        int randomSpellIndex1 = Random.Range(0, spells.Length);
        int randomSpellIndex2 = Random.Range(0, spells.Length);
        while (randomSpellIndex1 == randomSpellIndex2)
        {
            randomSpellIndex2 = Random.Range(0, spells.Length);
        }
        rewardSpell1.sprite = spells[randomSpellIndex1].spellInfo.spellIcon;
        SpellManager.Instance.AddSpellAmount(spells[randomSpellIndex1]);
        rewardSpell2.sprite = spells[randomSpellIndex2].spellInfo.spellIcon;
        SpellManager.Instance.AddSpellAmount(spells[randomSpellIndex2]);

        Time.timeScale = 0;
    }
    public void CloseRewardPanel()
    {
        rewardPanel.SetActive(false);
        UIManager.instance.ContinueGame();
        rewardButton.interactable = AdsManager.Instance.isRewardedAdReady;
    }
}
