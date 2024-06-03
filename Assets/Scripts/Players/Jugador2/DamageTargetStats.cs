using Assets.Scripts.Effects;
using Assets.Scripts.Stats;
using Sirenix.OdinInspector;
using UnityEngine;

    public class DamageTargetStats : MonoBehaviour
    {
        //Constants
        private readonly double                 critChance = 0.1;
        private KnockbackEffect                 knockback;
        private PlayerStats                     playerStats;
        private float                           timeSinceAttack = 0.25f;
        private Animator                        animator;
        private SecondPlayerAnimator            animate;
        private Vector2                         direction;



    //Input data for damage logic
    [LabelText("Attack sensor")]
        [SerializeField] private Transform      attackTransformAtack; //Object or controller that recognizes the horizontalAttack
        [LabelText("Attack Range")]
        [SerializeField] private float          attackRangeAttack;
        [LabelText("Attack Cooldown")]
        [Range(0.0f, 2.0f)]
        [SerializeField] private float          attackCooldown;
        [LabelText("Attack Key")]
        [SerializeField] private AttackKeys     attackKey;
        [LabelText("Attack Damage Multiplier")]
        [Range(0.0f, 1.0f)]
        [SerializeField] private float          attackDamageMultiplier;
        [SerializeField] private LayerMask      attackableLayer;

        //private RaycastHit2D[] hits;
        private Collider2D[] hits;

        public bool HasTakenDamage { get; set; } = false;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            animate = new SecondPlayerAnimator(animator);
            knockback = GetComponent<KnockbackEffect>();
        }

        private void Start()
        {
            playerStats = GetComponent<PlayerStats>();
        }

        private void Update()
        {

            timeSinceAttack += Time.deltaTime;
            direction = new Vector2(transform.right.x, transform.right.y);


        if (Input.GetKeyDown(AttackKeysExtensions.ToKey(attackKey)) && timeSinceAttack > attackCooldown /*&& !m_rolling*/)
            {

                animator.SetTrigger(SecondPlayerAnimations.attack + AttackKeysExtensions.Index(attackKey));
                Attack();

                // Reset timer
                timeSinceAttack = 0.0f;
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
                    c.GetComponent<PlayerStats>().Health -= damage;
                    c.GetComponent<Attack>().WasHitted();
                    c.GetComponent<KnockbackEffect>().CallKnockback(direction, Vector2.up, Input.GetAxisRaw("Horizontal"));
            }
        }
        }

        public void WasHitted()
        {
            animate.Hurt();
        }


        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(attackTransformAtack.position, attackRangeAttack);

        }
    }
