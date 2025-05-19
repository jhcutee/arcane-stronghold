using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
public enum EnemyState
{
    Walk,
    Hurt,
    Die,
    Attack,
    Slow,
    Stun,
}
[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [Header("Events")]
    public static Action onEndPointPositionReached;
    public static Action<EnemyState, Enemy> onEnemyStateChanged;
    public static Action onBlackHoleTrigger;

    [Header("Elements")]
    [SerializeField] private EnemySO enemySO;
    [SerializeField] private EnemyHP enemyHP;
    [SerializeField] public SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] public EnemyEffectController enemyEffectController;
    [SerializeField] private EnemyAnimation enemyAnimation;
    public WayPoint WayPoint { get; set; }

    [Header("Settings")]
    protected EnemyType enemyType;
    public int maxHp;
    protected float speed;
    protected EnemyEffectType[] specialEffectTypes;
    protected EnemyState enemyState;
    public EnemyType EnemyType => enemyType;
    public int MaxHp => maxHp;
    public float Speed => speed;
    public EnemyEffectType[] SpecialEffectTypes => specialEffectTypes;

    public int currentPositionIndex;
    public Vector3 CurrentPointPosition => WayPoint.GetPointPosition(currentPositionIndex);
    public Vector3 lastPointPosition;

    public EnemyState EnemyState { get => enemyState; }
    public EnemyHP EnemyHP => enemyHP;

    public EnemyEffectController EnemyEffectController { get => enemyEffectController; }
    public EnemyAnimation EnemyAnimation { get => enemyAnimation; }
    public EnemySO EnemySO { get => enemySO;}

    private void Awake()
    {
        enemyAnimation = GetComponent<EnemyAnimation>();
        enemyHP = GetComponent<EnemyHP>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        lastPointPosition = this.transform.position;
        enemyEffectController = GetComponent<EnemyEffectController>();
        GetEnemyInfo();
    }
    void Start()
    {
        rb.gravityScale = 0;
        currentPositionIndex = 0;
    }
    private void Update()
    {
        Move();
        RotateEnemy();
        if (IsCurrentPointPositionReached())
        {
            UpdatePointPosition();
        }
    }
    protected virtual void LateUpdate()
    {
        if (spriteRenderer != null)
            spriteRenderer.sortingOrder = -(int)(transform.position.y * 100);
    }
    private void GetEnemyInfo()
    {
        enemyType = EnemySO.enemyType;
        maxHp = EnemySO.maxHp;
        speed = EnemySO.moveSpeed;
        specialEffectTypes = EnemySO.specialEffects;
    }
    public void SetEnemyState(EnemyState enemyState, Enemy _enemy)
    {
        this.enemyState = enemyState;
        onEnemyStateChanged?.Invoke(enemyState, this);
    }
    private void Move()
    {
        if (enemyState == EnemyState.Walk)
            this.transform.position = Vector3.MoveTowards(this.transform.position, CurrentPointPosition, Speed * Time.deltaTime);
    }
    private void RotateEnemy()
    {
        spriteRenderer.flipX = lastPointPosition.x > CurrentPointPosition.x;
    }
    public void SetMoveSpeed(float multiplier)
    {
        speed = EnemySO.moveSpeed * multiplier;
        
    }
    private bool IsCurrentPointPositionReached()
    {
        if (Vector3.Distance(this.transform.position, CurrentPointPosition) < 0.1f)
        {
            lastPointPosition = this.transform.position;
            return true;
        }
        return false;
    }
    private void UpdatePointPosition()
    {
        int maxPointIndex = WayPoint.Points.Length - 1;
        if(currentPositionIndex < maxPointIndex)
        {
            currentPositionIndex++;
        }
        else
        {
            EndPointPosReached();
        }
    }
    private void EndPointPosReached()
    {
        onEndPointPositionReached?.Invoke();
        EnemyHP.ResetHp();
        Spawner.Instance.ObjectPooler.ReturnInstanceToPool(this.gameObject);
        enemyEffectController.activeEffects.Clear();
    }
    public void ResetWayPoint()
    {
        currentPositionIndex = 0;
    }
    public bool HasSpecialEffect(EnemyEffectType effectType)
    {
        foreach (var effect in specialEffectTypes)
        {
            if (effect == effectType)
            {
                return true;
            }
        }
        return false;
    }
    
    private void OnMouseDown()
    {
        EnemyCanvas.instance.SetEnemyInfo(this);
    }
}
