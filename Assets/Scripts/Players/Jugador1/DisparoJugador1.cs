using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisparoJugador : MonoBehaviour
{
    public Transform controladorDisparo;
    public GameObject bala;
    public float velocidadCrecimiento = 1f;
    public float escalaMaxima = 2f;

    private bool disparando = false;
    private float tiempoDisparo;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            disparando = true;
            tiempoDisparo = Time.time;
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            disparando = false;
            Disparar();
        }
    }

    private void FixedUpdate()
    {
        if (disparando)
        {
            float tiempoPasado = Time.time - tiempoDisparo;
            float escala = Mathf.Lerp(1f, escalaMaxima, tiempoPasado * velocidadCrecimiento);
            Vector3 nuevaEscala = new Vector3(escala, escala, 1f);
            bala.transform.localScale = nuevaEscala;
        }
    }

    private void Disparar()
    {
        Vector3 posicion = controladorDisparo.position;
        posicion.z = -10; // Establecer la posición z a -10
        GameObject nuevaBala = Instantiate(bala, posicion, controladorDisparo.rotation);
        Destroy(nuevaBala, 5f); // Destruir la bala después de 5 segundos si no colisiona
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject); // Destruir la bala cuando colisiona con otro collider
    }
}
