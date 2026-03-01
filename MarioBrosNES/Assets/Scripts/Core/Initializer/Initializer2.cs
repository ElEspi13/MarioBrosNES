using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer2 : MonoBehaviour
{
    /// <summary>
    /// Inicializa la posición del jugador al inicio del nivel utilizando el GameManager para resetear la posición del jugador.
    /// </summary>
    void Start()
    {
        GameManager.Instance.ResetPlayerPosition();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
