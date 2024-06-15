using Assets.Scripts.Effects;
using Assets.Scripts.Stats;
using Sirenix.OdinInspector;
using UnityEngine;
 
public class DamageTargetStats : MonoBehaviour
{
    // Constants
    private readonly double                 critChance = 0.1;
    private PlayerStats                     playerStats;
    private float                           timeSinceAttack = 0f;
    private float                           timeSinceBlock = 0f;
    private Animator                        animator;
    private SecondPlayerAnimator            animate;
    private int                             facingDirection;
    private int                             blockUses = 2;
    private float                           blockCooldown = 2.0f;
 
    // Input data for damage logic
    [LabelText("Attack sensor")]
    [SerializeField] private Transform attackTransformAtack; // Object or controller that recognizes the horizontalAttack
    [LabelText("Attack Range")]
    [SerializeField] private float attackRangeAttack;
    [LabelText("Attack Cooldown")]
    [Range(0.0f, 2.0f)]
    [SerializeField] private float attackCooldown;
    [LabelText("Attack Key")]
    [SerializeField] private AttackKeys attackKey;
    [LabelText("Attack Damage Multiplier")]
    [Range(0.0f, 1.0f)]
    [SerializeField] private float attackDamageMultiplier;
    [SerializeField] private LayerMask attackableLayer;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip blockSound;
 
    // private RaycastHit2D[] hits;
    private Collider2D[] hits;
 
    public bool IsBlocking { private set; get; }
    private bool IsAttacking => Input.GetKeyDown(KeyCode.N) || Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.L);
    private bool IsStoppingBlock => Input.GetKeyUp(KeyCode.J) || IsAttacking;
 
    private void Awake()
    {
        animator = GetComponent<Animator>();
        animate = new SecondPlayerAnimator(animator);
    }
 
    private void Start()
    {
        playerStats = GetComponent<PlayerStats>();
    }
 
    private void Update()
    {
        timeSinceAttack += Time.deltaTime;
        if(blockUses == 0 && timeSinceBlock < blockCooldown) timeSinceBlock += Time.deltaTime;
        if(timeSinceBlock > blockCooldown) blockUses = 2;
        FacingDirection(Input.GetAxisRaw("Horizontal2"));
 
        if (Input.GetKeyDown(AttackKeysExtensions.ToKey(attackKey)) && timeSinceAttack > attackCooldown /*&& !m_rolling*/)
        {
            animator.SetTrigger(SecondPlayerAnimations.attack + AttackKeysExtensions.Index(attackKey));
            Attack();
 
            // Reset timer
            timeSinceAttack = 0.0f;
        } 
        if (Input.GetKeyDown(KeyCode.J) && blockUses > 0)
        {
            HandleBlocking(Input.GetKeyDown(KeyCode.J));
        } else if (Input.GetKeyUp(KeyCode.J))
        {
            IsBlocking = false;
        }
        if (blockUses == 0)
        {
            timeSinceBlock = 0;
        }
    }
 
    public void Attack()
    {
        hits = Physics2D.OverlapCircleAll(transform.position, 2);
 
        foreach (Collider2D c in hits)
        {
            if (c.CompareTag("jugador1"))
            {
                float damage = Random.Range(0f, 1f) < critChance ? playerStats.Attack * 1.5f : playerStats.Attack;
                Vector2 hitDirection = (c.transform.position - transform.position).normalized/10;
                if(c.GetComponent<Attack>().IsBlocking)
                {
                    blockUses--;
                } else
                {
                    c.GetComponent<PlayerStats>().Health -= damage;
                    c.GetComponent<Attack>().WasHitted();
 
                }
                c.GetComponent<KnockbackEffect>().CallKnockback(
                    -hitDirection * facingDirection, new Vector2(-facingDirection * 0.1f, 0.06f), Input.GetAxisRaw("Horizontal")
                );
            }
        }
    }
 
 
    private void HandleBlocking(bool isBlocking)
    {
        if (isBlocking)
        {
            animate.Block();
            IsBlocking = true;
        }
    }
 
    public void HitBlocked()
    {
        Routines.WaitAndExecute(0.15f, () => animate.IdleBlock(true));
        animate.IdleBlock(false);
    }
 
    public void WasHitted()
    {
        animate.Hurt();
    }
 
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackTransformAtack.position, attackRangeAttack);
    }
 
    private void FacingDirection(float inputX)
    {
        facingDirection = (inputX > 0) ? 1 : -1;
    }
}