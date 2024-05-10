using Assets.Scripts.Effects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Scripts.Stats
{
    public class DamageTargetStats : MonoBehaviour
    {
        //Constants
        private readonly double                 critChance = 0.1;
        private readonly KnockbackEffect        knockback;
        private PlayerStats                     playerStats;
        private float                           timeSinceAttack = 0.25f;
        private int                             currentAttack = 0;
        private Animator                        animator;


        //Input data for damage logic
        [LabelText("1st Attack sensor")]
        [SerializeField] private Transform      attackTransformAtack1; //Object or controller that recognizes the horizontalAttack
        [LabelText("2nd Attack sensor")]
        [SerializeField] private Transform      attackTransformAtack2;
        [LabelText("3rd Attack sensor")]
        [SerializeField] private Transform      attackTransformAtack3;
        [LabelText("1st Attack Range")]
        [SerializeField] private float          attackRangeAttack1;
        [LabelText("2nd Attack Range")]
        [SerializeField] private float          attackRangeAttack2;
        [LabelText("3rd Attack Range")] 
        [SerializeField] private float          attackRangeAttack3;
        [SerializeField] private LayerMask      attackableLayer;

        //private RaycastHit2D[] hits;
        private Collider2D[] hits;

        public bool HasTakenDamage { get; set; } = false;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            playerStats = GetComponent<PlayerStats>();
        }

        private void Update()
        {

            timeSinceAttack += Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.N) && timeSinceAttack > 0.25f /*&& !m_rolling*/)
            {
                currentAttack++;

                // Loop back to one after third horizontalAttack
                if (currentAttack > 3)
                    currentAttack = 1;

                // Reset Attack combo if time since last horizontalAttack is too large
                if (timeSinceAttack > 1.0f)
                    currentAttack = 1;

                // Call one of three horizontalAttack animations "Attack1", "Attack2", "Attack3"
                animator.SetTrigger(SecondPlayerAnimations.attack + currentAttack);

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
                if (c.CompareTag("Jugador1"))
                {
                    float damage = Random.Range(0f, 1f) < critChance ? playerStats.Attack * 1.5f : playerStats.Attack;
                    c.GetComponent<PlayerStats>().Health -= damage;
                }
            }
            //int direction = Input.GetAxisRaw("Horizontal") > 0 ? 1 : -1;
            //hits = Physics2D.CircleCastAll(attackTransformAtack1.position, attackRangeAttack1, transform.right, 0, attackableLayer);
            //
            //for(int i = 0; i < hits.Length; i++)
            //{
            //    IDamageable damageable = hits[i].collider.gameObject.GetComponent<IDamageable>();
            //
            //    if(damageable != null)
            //    {
            //        //Vector2 hitDirection = new Vector2(transform.right.x, transform.right.y) * direction;
            //        damageable.Damage();
            //    }
            //}
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(attackTransformAtack1.position, attackRangeAttack1);
            Gizmos.DrawWireSphere(attackTransformAtack2.position, attackRangeAttack2);
            Gizmos.DrawWireSphere(attackTransformAtack3.position, attackRangeAttack3);

        }
    }
}