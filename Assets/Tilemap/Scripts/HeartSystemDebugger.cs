using UnityEngine;

/// <summary>
/// Script auxiliar para diagnosticar y arreglar problemas de visibilidad del sistema de corazones.
/// Añádelo a cualquier GameObject en tu escena y usa los métodos desde el Inspector o código.
/// </summary>
public class HeartSystemDebugger : MonoBehaviour
{
    [Header("Opciones de Debug")]
    [SerializeField] private bool autoFixOnStart = false;
    [SerializeField] private bool showHeartCount = true;

    void Start()
    {
        if (autoFixOnStart)
        {
            FixHeartVisibility();
        }
    }

    void Update()
    {
        if (showHeartCount && HeartSystemManager.Instance != null)
        {
            // Opcional: Mostrar info en pantalla
        }
    }

    /// <summary>
    /// Arregla la visibilidad de los corazones
    /// </summary>
    [ContextMenu("Fix Heart Visibility")]
    public void FixHeartVisibility()
    {
        if (HeartSystemManager.Instance != null)
        {
            HeartSystemManager.Instance.EnsureVisibility();
            Debug.Log("✅ Visibilidad de corazones arreglada");
        }
        else
        {
            Debug.LogError("❌ HeartSystemManager.Instance es null");
        }
    }

    /// <summary>
    /// Reinicializa completamente el sistema de corazones
    /// </summary>
    [ContextMenu("Reinitialize Heart System")]
    public void ReinitializeHeartSystem()
    {
        if (HeartSystemManager.Instance != null)
        {
            HeartSystemManager.Instance.ReinitializeSystem();
        }
        else
        {
            Debug.LogError("❌ HeartSystemManager.Instance es null");
        }
    }

    /// <summary>
    /// Muestra información de diagnóstico del sistema de corazones
    /// </summary>
    [ContextMenu("Show Heart System Info")]
    public void ShowHeartSystemInfo()
    {
        if (HeartSystemManager.Instance == null)
        {
            Debug.LogError("❌ HeartSystemManager.Instance es null");
            return;
        }

        Debug.Log("=== INFORMACIÓN DEL SISTEMA DE CORAZONES ===");
        Debug.Log($"Corazones actuales: {HeartSystemManager.Instance.GetCurrentHearts()}");
        Debug.Log($"¿Tiene instancia? {HeartSystemManager.Instance != null}");
    }

    /// <summary>
    /// Simula perder un corazón
    /// </summary>
    [ContextMenu("Lose Heart")]
    public void LoseHeart()
    {
        if (HeartSystemManager.Instance != null)
        {
            HeartSystemManager.Instance.LoseHeart();
        }
    }

    /// <summary>
    /// Simula ganar un corazón
    /// </summary>
    [ContextMenu("Gain Heart")]
    public void GainHeart()
    {
        if (HeartSystemManager.Instance != null)
        {
            HeartSystemManager.Instance.GainHeart();
        }
    }

    /// <summary>
    /// Resetea todos los corazones
    /// </summary>
    [ContextMenu("Reset Hearts")]
    public void ResetHearts()
    {
        if (HeartSystemManager.Instance != null)
        {
            HeartSystemManager.Instance.ResetHearts();
        }
    }
}
