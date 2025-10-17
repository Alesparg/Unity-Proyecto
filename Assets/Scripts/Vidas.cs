using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vidas : MonoBehaviour
{
    [SerializeField] private float vida = 5f;

    // Método para modificar la vida
    public void ModificarVida(float puntos)
    {
        vida += puntos;
        Debug.Log("Vida actual: " + vida);

        // Si pierde vida, quitar un corazón
        if (puntos < 0)
        {
            HeartSystemManager heartSystem = Object.FindFirstObjectByType<HeartSystemManager>();
            if (heartSystem != null)
            {
                heartSystem.LoseHeart();
                Debug.Log("¡Daño recibido! Corazón perdido.");
            }
            else
            {
                Debug.LogError("❌ No se encontró HeartSystemManager!");
            }
        }
    }

    // Método para verificar si el personaje sigue vivo
    private bool EstasVivo()
    {
        return vida > 0;
    }

    // Detectar colisiones con objetos
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("meta"))
        {
            Debug.Log("¡GANASTE!");
            return;
        }
    }
}

