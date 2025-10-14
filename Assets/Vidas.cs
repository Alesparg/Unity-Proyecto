using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vidas : MonoBehaviour
{
    [SerializeField] private float vida = 5f;

    // Guardar referencia al sistema de corazones
    private WorkingHeartSystem heartSystem;

    void Awake()
    {
        // Buscar el sistema de corazones solo una vez
        heartSystem = Object.FindFirstObjectByType<WorkingHeartSystem>();

        // Si no existe, crearlo automáticamente
        if (heartSystem == null)
        {
            GameObject go = new GameObject("WorkingHeartSystem");
            heartSystem = go.AddComponent<WorkingHeartSystem>();
        }
    }

    // Método para modificar la vida
    public void ModificarVida(float puntos)
    {
        vida += puntos;
        Debug.Log("Vida actual: " + vida);

        // Si pierde vida, quitar un corazón
        if (puntos < 0)
        {
            if (heartSystem != null)
            {
                heartSystem.LoseHeart();
                Debug.Log("¡Daño recibido! Corazón perdido.");
            }
            else
            {
                Debug.LogError("No se encontró WorkingHeartSystem!");
            }
        }
    }

    // Método para verificar si el personaje sigue vivo
    public bool EstasVivo()
    {
        return vida > 0;
    }

    // Detectar colisiones con objetos
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("meta"))
        {
            Debug.Log("¡GANASTE!");
        }
    }
}



