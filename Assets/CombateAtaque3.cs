using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombateAtaque3 : MonoBehaviour
{
    public Transform controladorGolpe;
    public float radioGolpe;
    public float da�oGolpe;
    private Animator animator;
    public float tiempoEntreAtaque;
    public float tiempoSiguienteAtaque;


    public void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (tiempoSiguienteAtaque > 0)
        {
            tiempoSiguienteAtaque -= Time.deltaTime;

        }
        if (Input.GetKeyDown(KeyCode.C) && tiempoSiguienteAtaque <= 0)
        {
            Golpe();
            tiempoSiguienteAtaque = tiempoEntreAtaque;
        }
    }
    private void Golpe()
    {
        animator.SetTrigger("ataque3");
        Collider2D[] objetos = Physics2D.OverlapCircleAll(controladorGolpe.position, radioGolpe);

        foreach (Collider2D c in objetos)
        {
            if (c.CompareTag("Jugador2"))
            {

            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(controladorGolpe.position, radioGolpe);
    }
}
