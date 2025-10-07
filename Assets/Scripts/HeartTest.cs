using UnityEngine;

public class HeartTest : MonoBehaviour
{
    [Header("Controles de Prueba")]
    public KeyCode loseHeartKey = KeyCode.L;
    public KeyCode gainHeartKey = KeyCode.G;
    public KeyCode resetHeartsKey = KeyCode.R;
    public KeyCode debugInfoKey = KeyCode.D;
    
    private HeartSystemManager heartSystem;
    
    void Start()
    {
        // Buscar el sistema de corazones
        heartSystem = Object.FindFirstObjectByType<HeartSystemManager>();
        
        if (heartSystem == null)
        {
            Debug.LogWarning("⚠️ No se encontró HeartSystemManager. Creando uno automáticamente...");
            
            // Crear automáticamente
            GameObject heartSystemGO = new GameObject("HeartSystemManager");
            heartSystem = heartSystemGO.AddComponent<HeartSystemManager>();
        }
        
        Debug.Log("✅ HeartTest configurado correctamente");
        ShowControls();
    }
    
    void Update()
    {
        if (heartSystem == null) return;
        
        // Presiona L para perder un corazón
        if (Input.GetKeyDown(loseHeartKey))
        {
            heartSystem.LoseHeart();
            Debug.Log("🧪 Prueba: Corazón perdido manualmente");
        }
        
        // Presiona G para ganar un corazón
        if (Input.GetKeyDown(gainHeartKey))
        {
            heartSystem.GainHeart();
            Debug.Log("🧪 Prueba: Corazón ganado manualmente");
        }
        
        // Presiona R para resetear
        if (Input.GetKeyDown(resetHeartsKey))
        {
            heartSystem.ResetHearts();
            Debug.Log("🧪 Prueba: Corazones reseteados");
        }
        
        // Presiona D para mostrar información de debug
        if (Input.GetKeyDown(debugInfoKey))
        {
            ShowDebugInfo();
        }
    }
    
    void ShowControls()
    {
        Debug.Log("🎮 CONTROLES DE PRUEBA:");
        Debug.Log($"   {loseHeartKey} - Perder corazón");
        Debug.Log($"   {gainHeartKey} - Ganar corazón");
        Debug.Log($"   {resetHeartsKey} - Resetear corazones");
        Debug.Log($"   {debugInfoKey} - Mostrar información de debug");
    }
    
    void ShowDebugInfo()
    {
        if (heartSystem != null)
        {
            Debug.Log("🔍 INFORMACIÓN DE DEBUG:");
            Debug.Log($"   Corazones actuales: {heartSystem.GetCurrentHearts()}/{heartSystem.GetMaxHearts()}");
            Debug.Log($"   Sistema encontrado: ✅");
        }
        else
        {
            Debug.Log("🔍 INFORMACIÓN DE DEBUG:");
            Debug.Log("   Sistema encontrado: ❌");
        }
    }
}