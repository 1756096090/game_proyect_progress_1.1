using Assets.Scripts.Effects;
using Assets.Scripts.Stats;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
 
public class Attack : MonoBehaviour
{
    private Animator animator;
    private readonly float                  critChance = 0.1f;
    private float                           timeSinceAttack = 0f;
    private float                           timeSinceBlock = 0f;
    private int                             facingDirection;
    private int                             blockUses = 2;
    private readonly float                  blockCooldown = 2f;
 
    [SerializeField] private Transform      controladorGolpe;
    [SerializeField] private float          radioGolpe;
    [LabelText("Daño")]
    [Description("Daño que se le hará al jugador enemigo")]
    [SerializeField] private float          damageConstant;
    [SerializeField] private float          attackCooldown;
    [SerializeField] private                AttackKeys attackKey;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip blockSound;
 
    private bool IsAttacking => Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.C);
    public bool IsBlocking { private set; get; }
    private bool IsStoppingBlock => Input.GetKeyUp(KeyCode.F) || IsAttacking;
 
    public void Start()
    {
        animator = GetComponent<Animator>();
    }
 
    private void Update()
    {
        timeSinceAttack += Time.deltaTime;
        if(blockUses == 0 && timeSinceBlock < blockCooldown) timeSinceBlock += Time.deltaTime;
        if(timeSinceBlock > blockCooldown) blockUses = 2;
 
        FacingDirection(Input.GetAxisRaw("Horizontal"));
 
        if (Input.GetKeyDown(AttackKeysExtensions.ToKey(attackKey)) && timeSinceAttack > attackCooldown)
        {
            Golpe();
            timeSinceAttack = 0f;
        }
 
        if (Input.GetKeyDown(KeyCode.E) && blockUses > 0)
        {
            HandleBlocking(Input.GetKeyDown(KeyCode.E));
        } else if(Input.GetKeyUp(KeyCode.E)) 
        {
            IsBlocking = false;
        }
        {
        if (blockUses == 0)
            timeSinceBlock = 0;
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
                Vector2 hitDirection = (c.transform.position - transform.position).normalized;
                if(c.GetComponent<DamageTargetStats>().IsBlocking)
                {
                    c.GetComponent<DamageTargetStats>().HitBlocked();
                    blockUses--;
                } else
                {
                    c.GetComponent<PlayerStats>().Health -= damage;
                    StartCoroutine(Routines.WaitAndExecute(0.15f, c.GetComponent<DamageTargetStats>().WasHitted));
                }
                c.GetComponent<KnockbackEffect>().CallKnockback(
                    -hitDirection * facingDirection, new Vector2(-facingDirection * 0.12f, 0.5f), Input.GetAxisRaw("Horizontal2")
                    );
 
            }
        }
    }
 
    private void HandleBlocking(bool isBlocking)
    {
        if (isBlocking)
        {
            //animator.SetTrigger("block");
            IsBlocking = true;
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
 
    private void FacingDirection(float inputX)
    {
        facingDirection = (inputX > 0) ? 1 : -1;
    }
}
