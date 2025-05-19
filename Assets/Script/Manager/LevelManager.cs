using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [Header("Settings")]
    [SerializeField] private List<GameObject> levels;
    private int currentLevel;

    public int CurrentLevel => currentLevel;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }

        currentLevel = PlayerPrefs.GetInt("CurrentLevel",1);
    }
    private void Start()
    {
        GenarateLevel();
    }
    private void GenarateLevel()
    {
        GameObject levelGO = Instantiate(levels[currentLevel - 1]);
        AdsManager.Instance.ShowInterstitialAd();
    }
}