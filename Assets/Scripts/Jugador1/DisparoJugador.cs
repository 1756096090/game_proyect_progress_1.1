using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisparoJugador : MonoBehaviour
{
    public Transform controladorDisparo;
    public GameObject bala;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            Disparar();
        }
    }

    private void Disparar()
    {
        Instantiate(bala, controladorDisparo.position, controladorDisparo.rotation );

    }
}
