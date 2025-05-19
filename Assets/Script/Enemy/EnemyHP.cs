using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : EnemyComponent
{
    [Header("Elements")]
    [SerializeField] private GameObject healthBarPrefab;
    [SerializeField] protected Transform healthBarPos;
    [Header("Settings")]
    [SerializeField] private int maxHp;
    public int CurrentHp { get; set; }
    private Image hpBar;
    private bool isDead = false;
    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        maxHp = enemy.MaxHp;
        CurrentHp = maxHp;
        CreathHPBar();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            TakeHit(100);
        }
        UpdateHpBar();
    }
    private void CreathHPBar()
    {
        GameObject newBar = Instantiate(healthBarPrefab, healthBarPos.position, Quaternion.identity);
        newBar.transform.SetParent(transform);

        EnemyHPBar hpBar = newBar.GetComponent<EnemyHPBar>();
        this.hpBar = hpBar.FillAmountHp;
    }
    private void UpdateHpBar()
    {
        hpBar.fillAmount = Mathf.Lerp(hpBar.fillAmount, (float)CurrentHp / maxHp, Time.deltaTime * 10);
    }
    public void TakeHit(int finalDmg)
    {
        if (isDead) return;
        ApllyDmg(finalDmg);
        if (enemy.EnemyState != EnemyState.Stun)
            enemy.SetEnemyState(EnemyState.Hurt, this.enemy);
    }
    public void ApllyDmg(int damage)
    {
        CurrentHp -= damage;
        if (CurrentHp <= 0)
        {
            CurrentHp = 0;
            Die();
        }
    }
    public void ResetHp()
    {
        CurrentHp = maxHp;
        isDead = false;
        hpBar.fillAmount = 1;
    }
    private void Die()
    {
        if(isDead) return;
        isDead = true;
        enemy.SetEnemyState(EnemyState.Die, this.enemy);
        CurrencyManager.Instance.AddCrystal(enemy.EnemySO.crystalReward);
        CurrencyManager.Instance.AddMagebook(enemy.EnemySO.mageBookReward);
        ShopManager.Instance.UpdateInfoUI();
    }

    public void ApplyDmg(float damage)
    {
        CurrentHp -= Mathf.CeilToInt(damage);
        if (CurrentHp <= 0)
        {

            CurrentHp = 0;
            Die();
        }
    }
}
