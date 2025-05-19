using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEffectController : EnemyComponent
{
    [SerializeField] public List<EnemyEffectData> activeEffects = new List<EnemyEffectData>();

    private void Update()
    {
        if (activeEffects.Count > 0)
        {
            UpdateEnemyColor(activeEffects[0].effectType);
            for (int i = activeEffects.Count - 1; i >= 0; i--)
            {
                EnemyEffectData effect = activeEffects[i];
                
                effect.duration -= Time.deltaTime;

                ApplyEffect(effect);

                if (effect.duration <= 0f)
                {
                    RemoveEffect(effect);
                    activeEffects.RemoveAt(i);
                }
            }
        }
        else
        {
            ResetEnemyColor();
        }
    }

    public void AddEffect(EnemyEffectData newEffect)
    {
        activeEffects.Add(new EnemyEffectData(newEffect.effectType, newEffect.value, newEffect.duration));
        
    }

    private void ApplyEffect(EnemyEffectData effect)
    {
        switch (effect.effectType)
        {
            case EffectType.Burn:
                // Apply burn damage only once per second
                if (effect.duration % 1f <= Time.deltaTime)
                {
                    enemy.EnemyHP.ApplyDmg(effect.value); // Apply fixed damage (e.g., 5 damage)
                }
                break;

            case EffectType.Slow:
                StartCoroutine(SetMoveSpeedMultiplier(effect.value));
                break;

            case EffectType.Poison:
                // Apply poison damage only once per second
                if (effect.duration % 1f <= Time.deltaTime)
                {
                    enemy.EnemyHP.ApplyDmg(effect.value); // Apply fixed damage (e.g., 5 damage)
                }
                break;
        }
    }

    private void RemoveEffect(EnemyEffectData effect)
    {
        switch (effect.effectType)
        {
            case EffectType.Slow:
                StartCoroutine(SetMoveSpeedMultiplier(0f));
                break;
        }
    }

    public void ApplyPushback(Vector2 fromPosition, int currentTowerLevel, float puskBackAmount)
    {
        if(this.enemy.HasSpecialEffect(EnemyEffectType.ResistKnockback)) return;

        Vector3 currentPoint = enemy.CurrentPointPosition;
        Vector3 previousPoint = enemy.lastPointPosition;

        float diffX = Mathf.Abs(currentPoint.x - previousPoint.x);
        float diffY = Mathf.Abs(currentPoint.y - previousPoint.y);

        Vector3 newPosition = enemy.transform.position;
        
        if (diffY > diffX)
        {
            if (currentPoint.y > previousPoint.y)
            {
                newPosition.y -= puskBackAmount;
            }
            else
            {
                newPosition.y += puskBackAmount;
            }
        }
        else
        {
            if (currentPoint.x > previousPoint.x)
            {
                newPosition.x -= puskBackAmount;
            }
            else
            {
                newPosition.x += puskBackAmount;
            }
        }

        enemy.transform.position = newPosition;
    }
    private void UpdateEnemyColor(EffectType effectType)
    {
        switch (effectType)
        {
            case EffectType.Slow:
                enemy.spriteRenderer.color = ColorUtils.HexToColor("#00FBFF"); // Change color to blue for Slow
                break;

            case EffectType.Burn:
                enemy.spriteRenderer.color = ColorUtils.HexToColor("#FF8700"); // Change color to red for Burn
                break;

            case EffectType.Poison:
                enemy.spriteRenderer.color = ColorUtils.HexToColor("#B9FF00"); // Change color to green for Poison
                break;

            default:
                ResetEnemyColor(); // Reset to default color for other effects
                break;
        }
    }

    private void ResetEnemyColor()
    {
        enemy.spriteRenderer.color = Color.white; // Reset to the default color
    }
    public IEnumerator SetMoveSpeedMultiplier(float slowPercent)
    {
        float animtionLength = AnimatorUtils.GetCurrentAnimationLength(enemy.EnemyAnimation.Animator);
        yield return new WaitForSeconds(animtionLength + 0.3f);

        float speedMutiplier = 1f - slowPercent;
        enemy.SetMoveSpeed(speedMutiplier);
    }
    public bool HasEffect(EffectType effectType)
    {
        foreach (var effect in activeEffects)
        {
            if (effect.effectType == effectType)
            {
                return true; // Slow effect is active
            }
        }
        return false; // No Slow effect found
    }

}

[System.Serializable]
public class EnemyEffectData
{
    public EffectType effectType;
    public float value;
    public float duration;

    public EnemyEffectData(EffectType effectType, float value, float duration)
    {
        this.effectType = effectType;
        this.value = value;
        this.duration = duration;
    }
}


