using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class EnemyCanvas : MonoBehaviour
{
    public static EnemyCanvas instance;
    [Header("Elements")]
    [SerializeField] private Image enemyAvatar;
    [SerializeField] private TextMeshProUGUI enemyName;
    [SerializeField] private TextMeshProUGUI enemyAbility;
    [SerializeField] private GameObject enemyCanvas;
    [SerializeField] private Image enemyHPBar;
    [SerializeField] private TextMeshProUGUI enemyHPText;
    private bool isActive = false;
    private Enemy lastSelectedEnemy = null;

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
        enemyCanvas.gameObject.SetActive(false);
    }
    private void Update()
    {
        if(isActive && lastSelectedEnemy != null)
        {
            UpdateHealthBar();
        }
    }
    public void SetEnemyInfo(Enemy enemy)
    {
        if (isActive && lastSelectedEnemy == enemy)
        {
            HideEnemyInfo();
            lastSelectedEnemy = null;
            return;
        }

        enemyAvatar.sprite = enemy.EnemySO.enemyAvatar;
        enemyName.text = enemy.gameObject.name;
        enemyAbility.text = "Ability: " + enemy.EnemySO.specialEffects[0].ToString();
        ShowEnemyCanvas();
        lastSelectedEnemy = enemy;
    }

    public void ShowEnemyCanvas()
    {
        isActive = true;
        enemyCanvas.SetActive(true);
    }

    public void HideEnemyInfo()
    {
        enemyCanvas.SetActive(false);
        isActive = false;
    }
    private void UpdateHealthBar()
    {
        if (lastSelectedEnemy != null)
        {
            float healthPercentage = (float)lastSelectedEnemy.EnemyHP.CurrentHp / lastSelectedEnemy.MaxHp;
            enemyHPBar.fillAmount = healthPercentage;
            enemyHPText.text = lastSelectedEnemy.EnemyHP.CurrentHp + "/" + lastSelectedEnemy.MaxHp;
        }
    }
}
