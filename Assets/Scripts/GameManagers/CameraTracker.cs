using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Esta clase se encarga de contener todos los métodos relacionados con el seguimiento de la cámara y su movimiento.
/// </summary>
public class CameraTracker 
{

    /// <summary>
    /// Retorna el centro de la posición actual de la cámara.
    /// </summary>
    /// <returns>Un Vector2 con la posición central de la cámara.</returns>
    static public Vector2 GetCameraCenter()
    {
        Camera camera = Camera.main;
        return new Vector2(camera.transform.position.x / 2, camera.transform.position.y / 2);
    }
}
