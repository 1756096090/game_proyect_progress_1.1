using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Stats;
using UnityEngine;

public class pillsController : MonoBehaviour
{  
    public GameObject player1;
    public GameObject player2;
    public float aumentoMaximo = 0.2f; // Porcentaje de aumento de la vida máxima

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("jugador1"))
        {

            player1.GetComponent<FirstPlayerController>().vidaActual *= 0.2f;
            PlayerStats playerStats = player1.GetComponent<PlayerStats>();

            // FirstPlayerController playerController = player1.GetComponent<FirstPlayerController>();

            // if (playerController.vidaActual >= playerStats.maxHealth)
            // {
            //     float aumentoVida = playerStats.maxHealth * aumentoMaximo;
            //     playerStats.maxHealth += aumentoVida;
            //     playerController.AumentarVidaActual(playerController.vidaActual + aumentoVida);
            // }
            // else
            // {
            //     playerStats.AumentarVidaMaxima(playerStats.maxHealth * aumentoMaximo);
            // }
        }
        else if (other.CompareTag("jugador2"))
        {
            
        //     PlayerStats playerStats = player2.GetComponent<PlayerStats>();
        //     SecondPlayerController playerController = player2.GetComponent<SecondPlayerController>();

        //     if (playerController.vidaActual >= playerStats.maxHealth)
        //     {
        //         float aumentoVida = playerStats.maxHealth * aumentoMaximo;
        //         playerStats.maxHealth += aumentoVida;
        //         playerController.AumentarVidaActual(playerController.vidaActual + aumentoVida);
        //     }
        //     else
        //     {
        //         playerStats.AumentarVidaMaxima( playerStats.maxHealth * aumentoMaximo);
        //     }
        }

        Destroy(gameObject); 
    }
}
