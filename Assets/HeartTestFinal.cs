using UnityEngine;

public class HeartTestFinal : MonoBehaviour
{
    void Update()
    {
        // Presiona L para perder un corazón
        if (Input.GetKeyDown(KeyCode.L))
        {
            WorkingHeartSystem heartSystem = Object.FindFirstObjectByType<WorkingHeartSystem>();
            if (heartSystem != null)
            {
                heartSystem.LoseHeart();
                Debug.Log("Prueba: Corazón perdido manualmente");
            }
            else
            {
                Debug.LogError("No se encontró WorkingHeartSystem!");
            }
        }

        
        if (Input.GetKeyDown(KeyCode.G))
        {
            WorkingHeartSystem heartSystem = Object.FindFirstObjectByType<WorkingHeartSystem>();
            if (heartSystem != null)
            {
                heartSystem.GainHeart();
                Debug.Log("Prueba: Corazón ganado manualmente");
            }
        }

        
        if (Input.GetKeyDown(KeyCode.R))
        {
            WorkingHeartSystem heartSystem = Object.FindFirstObjectByType<WorkingHeartSystem>();
            if (heartSystem != null)
            {
                heartSystem.ResetHearts();
                Debug.Log("Prueba: Corazones reseteados");
            }
        }

        
        if (Input.GetKeyDown(KeyCode.D))
        {
            WorkingHeartSystem heartSystem = Object.FindFirstObjectByType<WorkingHeartSystem>();
            if (heartSystem != null)
            {
                Debug.Log("Sistema de corazones encontrado y funcionando");
            }
            else
            {
                Debug.LogError("No se encontró WorkingHeartSystem!");
            }
        }
    }
}

