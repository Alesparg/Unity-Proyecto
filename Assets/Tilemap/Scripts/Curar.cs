using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curar : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private int puntos = 1;
    [SerializeField] private bool usarHeartSystemDirectamente = true;

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"🎯 Partícula colisionó con: {other.name}");

            if (usarHeartSystemDirectamente)
            {
                // Usar HeartSystemManager directamente
                if (HeartSystemManager.Instance != null)
                {
                    for (int i = 0; i < puntos; i++)
                    {
                        HeartSystemManager.Instance.GainHeart();
                    }
                    Debug.Log($"💚 {puntos} corazón(es) añadido(s) vía HeartSystemManager");
                }
                else
                {
                    Debug.LogError("❌ HeartSystemManager.Instance es null. No se puede curar.");
                }
            }
            else
            {
                // Usar el sistema del Jugador (método anterior)
                Jugador jugador = other.GetComponent<Jugador>();

                if (jugador != null)
                {
                    jugador.ModificarVida(puntos);
                    Debug.Log($"💚 Puntos de curación aplicados al jugador: {puntos}");
                }
                else
                {
                    Debug.LogWarning("⚠️ No se encontró el componente 'Jugador' en el objeto con tag Player.");
                }
            }
        }
    }
}

