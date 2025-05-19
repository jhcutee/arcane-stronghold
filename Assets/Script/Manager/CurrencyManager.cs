using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;
    [Header("Elements")]
    [SerializeField] private TextMeshProUGUI crystalText;
    [SerializeField] private TextMeshProUGUI mageBookText;
    [SerializeField] private TextMeshProUGUI heartText;

    private int crytstalAmount = 5000;
    private int mageBookAmount = 10;
    private int heartAmount = 3;

    public int CrytstalAmount { get => crytstalAmount; }
    public int MageBookAmount { get => mageBookAmount; }
    public int HeartAmount { get => heartAmount;}

    private void Awake()
    {
        if(Instance != null) Destroy(Instance);
        else Instance = this;
        crystalText.text = CrytstalAmount.ToString();
        mageBookText.text = MageBookAmount.ToString();
        heartText.text = HeartAmount.ToString();
        Enemy.onEndPointPositionReached += RemoveHeart;
    }

    public void AddCrystal(int amount)
    {
        crytstalAmount += amount;
        crystalText.text = CrytstalAmount.ToString();
    }
    public void AddMagebook(int amount)
    {
        mageBookAmount += amount;
        mageBookText.text = mageBookAmount.ToString();
    }   
    public void RemoveCrystal(int amount)
    {
        crytstalAmount -= amount;
        crystalText.text = CrytstalAmount.ToString();
    }
    public void RemoveMagebook(int amount)
    {
        mageBookAmount -= amount;
        mageBookText.text = mageBookAmount.ToString();
    }
    private void RemoveHeart()
    {
        heartAmount--;
        heartText.text = HeartAmount.ToString();
        if(heartAmount <= 0)
        {
            heartAmount = 0;
            GameManager.instance.SetGameState(GameState.GameOver);
        }
    }
}
