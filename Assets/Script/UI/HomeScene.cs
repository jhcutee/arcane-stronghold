using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HomeScene : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject levelPopup;
    private void Awake()
    {
        levelPopup.SetActive(false);
    }
    public void HandlePlayBtn()
    {
        LoadingScene.sceneToLoad = "Gameplay";
        UnityEngine.SceneManagement.SceneManager.LoadScene("Loading");
    }
    public void HandleSettingBtn()
    {
        //do something
    }
    public void HandleLevelBtn()
    {
        levelPopup.SetActive(true);
    }
    public void HandleCloseLevelPopup()
    {
        levelPopup.SetActive(false);
    }
    public void HandleLevelSelectBtn(int id)
    {
        PlayerPrefs.SetInt("CurrentLevel", id); 
        LoadingScene.sceneToLoad = "Gameplay";
        UnityEngine.SceneManagement.SceneManager.LoadScene("Loading");
    }
}
