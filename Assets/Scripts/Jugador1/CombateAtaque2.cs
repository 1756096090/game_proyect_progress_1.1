using Assets.Scripts.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombateAtaque2 : MonoBehaviour
{
    private Animator        animator;
    
    [SerializeField] private Transform        controladorGolpe;
    [SerializeField] private float            radioGolpe;
    [SerializeField] private float            dañoGolpe;
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

        foreach (Collider2D c in objetos)
        {
            if (c.CompareTag("Jugador2"))
            {
                c.GetComponent<PlayerStats>().Health -= dañoGolpe;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(controladorGolpe.position, radioGolpe);
    }
}
