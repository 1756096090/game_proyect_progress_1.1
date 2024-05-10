using Assets.Scripts.Stats;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombateAtaque2 : MonoBehaviour
{
    private Animator                          animator;
    private readonly float                    critChance = 0.1f;
    
    [SerializeField] private Transform        controladorGolpe;
    [SerializeField] private float            radioGolpe;
    [LabelText("Daño")]
    [SerializeField] private float            attackPower;
    [SerializeField] private float            tiempoEntreAtaque;
    [SerializeField] private float            tiempoSiguienteAtaque;


    public void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if ( tiempoSiguienteAtaque > 0) 
        {
            tiempoSiguienteAtaque -= Time.deltaTime;
            
        }
        if (Input.GetKeyDown(KeyCode.X) && tiempoSiguienteAtaque <= 0)
        {
            Golpe();
            tiempoSiguienteAtaque = tiempoEntreAtaque;
        }
    }
    private void Golpe()
    {
        animator.SetTrigger("ataque2");
        Collider2D[] objetos = Physics2D.OverlapCircleAll(controladorGolpe.position, radioGolpe);
        float damage = Random.Range(0f, 1f) < critChance ? attackPower * 1.5f : attackPower;

        foreach (Collider2D c in objetos)
        {
            if (c.CompareTag("jugador2"))
            {
                c.GetComponent<PlayerStats>().Health -= attackPower;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(controladorGolpe.position, radioGolpe);
    }
}
