using TMPro;
using UnityEngine;
 
public class ËndGameControlText : MonoBehaviour

{

    public GameObject jugador1;
    public GameObject jugador2;
    public TextMeshProUGUI texto;

    void Update()

    {

        bool ambosJugadoresPresentes = jugador1 != null && jugador2 != null;

        // Ocultar el texto si ambos jugadores están presentes, mostrarlo si uno de ellos falta

        texto.enabled = !ambosJugadoresPresentes;

        if (!ambosJugadoresPresentes)

        {

            texto.text = jugador1 == null ? "¡Jugador 1 gana!" : "¡Jugador 2 gana!";

        }

    }

}
