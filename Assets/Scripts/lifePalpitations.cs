using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Stats;
using UnityEngine;
using UnityEngine.UI;

public class LifePalpitations : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public Image image1;
    public Image image2;
    public Image image3;
    public Image image4;
    public Color pulsatingColor = Color.red; // Color al estar al 20%

    private Color originalColor; // Color original de las imágenes
    private float pulseSpeed = 5f; // Velocidad de pulso

    private void Start()
    {
        if (image1 != null)
        {
            originalColor = image1.color; // Guardar el color original
        }
    }

    private void Update()
    {
        // Verificar la salud de los jugadores
        float player1Health = player1.GetComponent<FirstPlayerController>().vidaActual;
        float player2Health = player2.GetComponent<SecondPlayerController>().vidaActual;

        // Determinar cuáles imágenes deben pulsar
        bool player1Pulsate = player1Health < player1.GetComponent<PlayerStats>().maxHealth * 0.2f;
        bool player2Pulsate = player2Health < player2.GetComponent<PlayerStats>().maxHealth * 0.2f;

        // Pulsar las imágenes según corresponda
        PulsateImage(image1, player1Pulsate, player1Health);
        PulsateImage(image2, player1Pulsate, player1Health);
        PulsateImage(image3, player2Pulsate, player2Health);
        PulsateImage(image4, player2Pulsate, player2Health);
    }

    // Cambiar la escala de la imagen para simular un pulso
    private void PulsateImage(Image image, bool pulsate, float health)
    {
        if (image == null) return;

        if (pulsate)
        {
            // Cambiar el color gradualmente a pulsatingColor
            image.color = Color.Lerp(originalColor, pulsatingColor, Mathf.PingPong(Time.time * pulseSpeed, 1));
            

        }
        else
        {
            // Restaurar el color y la escala originales
            image.color = health <= 0 ? Color.clear : originalColor;
        }
    }
}
