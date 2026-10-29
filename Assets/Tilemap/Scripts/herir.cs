using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Herir : MonoBehaviour
{
    // Variables a configurar desde el editor
    [Header("Configuracion")]
    [SerializeField] int puntos = 5;

    // Variables de uso interno en el script
    private PisotonEnemigo pisoton;
    private Collider2D miCollider2D;

    private void OnEnable()
    {
        pisoton = GetComponent<PisotonEnemigo>();
        miCollider2D = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Si el jugador cae encima de un enemigo pisoteable, no recibe daño: lo elimina
            if (pisoton != null && PisotonEnemigo.EsPisoton(collision, miCollider2D)) { return; }

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

