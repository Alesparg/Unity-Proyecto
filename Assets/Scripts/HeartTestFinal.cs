using UnityEngine;

public class HeartTestFinal : MonoBehaviour
{
    private WorkingHeartSystem heartSystem;

    void Start()
    {
        // Buscar una sola vez al inicio
        heartSystem = Object.FindFirstObjectByType<WorkingHeartSystem>();

        if (heartSystem == null)
        {
            Debug.LogError("❌ No se encontró WorkingHeartSystem en la escena!");
        }
    }

    void Update()
    {
        if (heartSystem == null) return;

        // Presiona L para perder un corazón
        if (Input.GetKeyDown(KeyCode.L))
        {
            heartSystem.LoseHeart();
            Debug.Log("Prueba: Corazón perdido manualmente");
        }

        // Presiona G para ganar un corazón
        if (Input.GetKeyDown(KeyCode.G))
        {
            heartSystem.GainHeart();
            Debug.Log("Prueba: Corazón ganado manualmente");
        }

        // Presiona R para resetear
        if (Input.GetKeyDown(KeyCode.R))
        {
            heartSystem.ResetHearts();
            Debug.Log("Prueba: Corazones reseteados");
        }

        // Presiona D para mostrar información
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("✅ Sistema de corazones encontrado y funcionando");
        }
    }
}

