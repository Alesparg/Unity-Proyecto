using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curar : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private int puntos = 1; // Ahora usa int en lugar de float

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            Jugador jugador = other.GetComponent<Jugador>();

            if (jugador != null)
            {
                jugador.ModificarVida(puntos);
                Debug.Log("💚 Puntos de curación aplicados al jugador: " + puntos);
            }
            else
            {
                Debug.LogWarning("⚠️ No se encontró el componente 'Jugador' en el objeto con tag Player.");
            }
        }
    }
}

