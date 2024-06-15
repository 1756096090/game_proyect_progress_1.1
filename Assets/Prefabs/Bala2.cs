using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Stats;
using UnityEngine;

public class Bala2 : MonoBehaviour
{
    public float damage = 10;
    public GameObject bala;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            float escalaX = bala.transform.localScale.x; // Suponiendo que la escala es uniforme
            playerStats.Health -= damage * escalaX / 10;
            Destroy(gameObject); // Destruir la bala cuando colisiona con un jugador
        }
    }
}
