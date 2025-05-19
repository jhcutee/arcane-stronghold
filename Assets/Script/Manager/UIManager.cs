using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [Header("Elements")]
    [Header("Dialogs")]
    [SerializeField] private GameObject waveFinished;
    [SerializeField] private GameObject waveCounter;
    [SerializeField] private GameObject theLastWave;
    [SerializeField] private GameObject lackOfResourcesUpgrade;
    [SerializeField] private GameObject lackOfResourcesBuild;
    [SerializeField] private GameObject locked;

    [Header("Popups")]
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject levelCompleted;
    [SerializeField] private GameObject gameWin;
    [SerializeField] private GameObject pause;
    
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
        waveFinished.SetActive(false);
        waveCounter.SetActive(false);
        theLastWave.SetActive(false);
        lackOfResourcesUpgrade.SetActive(false);
        lackOfResourcesBuild.SetActive(false);
        gameOver.SetActive(false);
        levelCompleted.SetActive(false);
        gameWin.SetActive(false);
        pause.SetActive(false);
    }
    private void OnEnable()
    {
        GameManager.onGameStateChanged += OnGameStateChanged;
    }
    private void OnDisable()
    {
        GameManager.onGameStateChanged -= OnGameStateChanged;
    }
    //Handle enemy wave
    public void ShowWaveFinishedLabel() => StartCoroutine(PlayWaveFinished());
    public void ShowWaveCounterLabel(int index) => StartCoroutine(PlayWaveCounter(index));
    public void ShowTheLastWaveLabel() => StartCoroutine(PlayTheLastWave());
    public void ShowLackOfResourcesUpdate() => StartCoroutine(PlayLackOfResources(lackOfResourcesUpgrade));
    public void ShowLackOfResourcesBuild() => StartCoroutine(PlayLackOfResources(lackOfResourcesBuild));
    public void ShowLockedDialog() => StartCoroutine(PlayLackOfResources(locked));
    private IEnumerator PlayWaveFinished()
    {
        waveFinished.SetActive(true);
        yield return new WaitForSeconds(2f);
        waveFinished.SetActive(false);
    }
    private IEnumerator PlayWaveCounter(int index)
    {
        TextMeshProUGUI waveCounterText = waveCounter.GetComponentInChildren<TextMeshProUGUI>();
        waveCounterText.text = "Wave " + $"{ index + 1}";
        if(index != 0)
        {
            yield return new WaitForSeconds(3f);
        }
        waveCounter.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        waveCounter.SetActive(false);
    }
    private IEnumerator PlayTheLastWave()
    {
        yield return new WaitForSeconds(4f);
        theLastWave.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        theLastWave.SetActive(false);
    }

    private IEnumerator PlayLackOfResources(GameObject dialog)
    {
        if(dialog.activeSelf == true)
        {
            yield break;
        }
        UpgradeManager.instance.HideUpgradeCanvas();
        dialog.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        dialog.SetActive(false);
    }
    public void HandlePauseButton()
    {
        GameManager.instance.SetGameState(GameState.Pause);
        pause.SetActive(true);
        Time.timeScale = 0f;
    }
    public void HandleResumeButton()
    {
        GameManager.instance.SetGameState(GameState.Playing); 
        ClosePausePopup();
    }
    public void ClosePausePopup()
    {
        pause.SetActive(false);
        ContinueGame();
    }
    public void ContinueGame()
    {
        if(CurrencyManager.Instance.HeartAmount <= 0)
        {
            return;
        }
        Time.timeScale = 1f;
    }
    private void HandleGameOver()
    {
        gameOver.SetActive(true);
        Time.timeScale = 0f;
    }
    private void HandleLevelCompleted()
    {
        levelCompleted.SetActive(true);
    }
    private void HandleGameWin()
    {
        gameWin.SetActive(true);
    }
    public void CloseGameOverPopup()
    {
        gameOver.SetActive(false);
    }
    public void CloseLevelCompletedPopup()
    {
        levelCompleted.SetActive(false);
    }
    public void CloseGameWinPopup()
    {
        gameWin.SetActive(false);
    }
    public void HandleHomeButton()
    {
        SceneManager.LoadScene("Home");
    }
    public void HandleNextButton()
    {
        PlayerPrefs.SetInt("CurrentLevel", LevelManager.instance.CurrentLevel + 1);
        SceneManager.LoadScene("GamePlay");
    }
    public void HandleRetryButton()
    {
        SceneManager.LoadScene("GamePlay");
    }
    private void OnGameStateChanged(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.GameOver:
                HandleGameOver();
                break;
            case GameState.LevelCompleted:
                HandleLevelCompleted();
                break;
            case GameState.GameWin:
                HandleGameWin();
                break;
            default:
                break;
        }
    }
}