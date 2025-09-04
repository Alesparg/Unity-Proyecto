using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Herir : MonoBehaviour
{
    // Variables a configurar desde el editor
    [Header("Configuracion")]
    [SerializeField] int puntos = 5;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Buscar el script de Vidas en el jugador
            Vidas jugador = collision.gameObject.GetComponent<Vidas>();

            if (jugador != null)
            {
                jugador.ModificarVida(-puntos);
                Debug.Log("PUNTOS DE DAÑO REALIZADOS AL JUGADOR: " + puntos);
            }
            else
            {
                Debug.LogError("No se encontró el script Vidas en el jugador!");
            }
        }
    }
}

