using Assets.Scripts.Effects;
using Assets.Scripts.Stats;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;


public class Attack : MonoBehaviour
{
    private Animator                                animator;
    private readonly float                          critChance = 0.1f;
    private float                                   timeSinceAttack = 0.25f;
    private KnockbackEffect                         knockback;  
    private Vector2                                 direction;


    [SerializeField] private Transform              controladorGolpe;
    [SerializeField] private float                  radioGolpe;
    [LabelText("Daño")]
    [Description("Daño que se le hará al jugador enemigo")]
    [SerializeField] private float                  damageConstant;
    [SerializeField] private float                  attackCooldown;
    [SerializeField] private AttackKeys             attackKey; 


    public void Start()
    {
        animator = GetComponent<Animator>();
        knockback = GetComponent<KnockbackEffect>();
    }

    private void Update()
    {
        timeSinceAttack += Time.deltaTime;
        direction = new Vector2(transform.right.x, transform.right.y);

        if (Input.GetKeyDown(AttackKeysExtensions.ToKey(attackKey)) && timeSinceAttack > attackCooldown)
        {
            Golpe();
            timeSinceAttack = 0f;
        }
    }
    private void Golpe()
    {
        animator.SetTrigger("ataque" + AttackKeysExtensions.Index(attackKey));
        Collider2D[] objetos = Physics2D.OverlapCircleAll(controladorGolpe.position, radioGolpe);

        foreach (Collider2D c in objetos)
        {
            if (c.CompareTag("jugador2"))
            {
                float damage = Random.Range(0f, 1f) < critChance ? damageConstant * 1.5f : damageConstant;
                c.GetComponent<PlayerStats>().Health -= damage;
                StartCoroutine(PlayerStateManagement.WaitAndExecute(0.15f, c.GetComponent<DamageTargetStats>().WasHitted));
                c.GetComponent<KnockbackEffect>().CallKnockback(direction, Vector2.up, Input.GetAxisRaw("Horizontal2"));

            }
        }
    }

    public void WasHitted()
    {
        animator.SetTrigger("takeDamage");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(controladorGolpe.position, radioGolpe);
    }
}
