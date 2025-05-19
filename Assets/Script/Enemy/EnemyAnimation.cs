using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : EnemyComponent
{
    [SerializeField] private Animator animator;

    public Animator Animator { get => animator; }

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        Enemy.onEnemyStateChanged += EnemyStateChanged;
    }

    private void OnDisable()
    {
        Enemy.onEnemyStateChanged -= EnemyStateChanged;
    }
    private void PlayStunAnimation()
    {
        Animator.SetTrigger("Stun");
    }
    private void PlayHurtAnimation()
    {
        Animator.SetTrigger("Hurt");
    }
    private void PlayDeadAnimation()
    {
        Animator.SetTrigger("Dead");
    }
    private void PlayAttackAnimation()
    {
        Animator.SetTrigger("Attack");
    }
    private IEnumerator PlayStun()
    {
        PlayStunAnimation();
        yield return new WaitForSeconds(2f);

        enemy.SetEnemyState(EnemyState.Walk, this.enemy);
        enemy.SetMoveSpeed(1f);
    }
    private IEnumerator PlayHurt()
    {
        PlayHurtAnimation();
        yield return new WaitForSeconds(AnimatorUtils.GetCurrentAnimationLength(this.animator) - 0.1f);
        
        enemy.SetEnemyState(EnemyState.Walk, this.enemy);
        enemy.SetMoveSpeed(1f);
    }
    private IEnumerator PlayDead()
    {
        PlayDeadAnimation();
        EnemyCanvas.instance.HideEnemyInfo();
        yield return new WaitForSeconds(AnimatorUtils.GetCurrentAnimationLength(this.animator) - 0.1f);
        enemy.EnemyHP.ResetHp();
        enemy.enemyEffectController.activeEffects.Clear();
        Spawner.Instance.ObjectPooler.ReturnInstanceToPool(this.gameObject);
    }
    private void EnemyStateChanged(EnemyState enemyState, Enemy enemy)
    {
        if (enemyState == EnemyState.Hurt && enemy == this.enemy)
        {
            StartCoroutine(PlayHurt());
        }
        else if (enemyState == EnemyState.Die && enemy == this.enemy)
        {
            StartCoroutine(PlayDead());
        }
        else if (enemyState == EnemyState.Attack && enemy == this.enemy)
        {

        }
        else if(enemyState == EnemyState.Stun && enemy == this.enemy)
        {
            StartCoroutine(PlayStun());
        }
    }
}
