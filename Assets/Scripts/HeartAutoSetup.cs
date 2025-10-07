using UnityEngine;

public class HeartAutoSetup : MonoBehaviour
{
    [Header("Configuración Automática")]
    public bool setupOnStart = true;
    public bool findHeartSpriteAutomatically = true;
    
    void Start()
    {
        if (setupOnStart)
        {
            SetupHeartSystem();
        }
    }
    
    public void SetupHeartSystem()
    {
        Debug.Log("🔧 Configurando sistema de corazones automáticamente...");
        
        // Buscar si ya existe un HeartSystemManager
        HeartSystemManager existingManager = Object.FindFirstObjectByType<HeartSystemManager>();
        if (existingManager != null)
        {
            Debug.Log("✅ Ya existe un HeartSystemManager");
            return;
        }
        
        // Crear GameObject para el sistema de corazones
        GameObject heartSystemGO = new GameObject("HeartSystemManager");
        HeartSystemManager heartManager = heartSystemGO.AddComponent<HeartSystemManager>();
        
        // Configurar el sprite automáticamente si está habilitado
        if (findHeartSpriteAutomatically)
        {
            SetupHeartSprite(heartManager);
        }
        
        Debug.Log("✅ Sistema de corazones configurado automáticamente");
    }
    
    void SetupHeartSprite(HeartSystemManager heartManager)
    {
        Debug.Log("🔍 Buscando sprite del corazón...");
        
        // Buscar sprite por nombre
        Sprite[] allSprites = Resources.FindObjectsOfTypeAll<Sprite>();
        
        foreach (Sprite sprite in allSprites)
        {
            string spriteName = sprite.name.ToLower();
            
            if (spriteName.Contains("corazon") || 
                spriteName.Contains("heart") ||
                spriteName.Contains("vida") ||
                spriteName.Contains("life"))
            {
                heartManager.heartSprite = sprite;
                Debug.Log($"✅ Sprite encontrado y asignado: {sprite.name}");
                return;
            }
        }
        
        // Si no se encuentra, buscar por archivo específico
        Sprite heartSprite = Resources.Load<Sprite>("corazon-removebg-preview");
        if (heartSprite != null)
        {
            heartManager.heartSprite = heartSprite;
            Debug.Log($"✅ Sprite cargado desde Resources: {heartSprite.name}");
        }
        else
        {
            Debug.LogWarning("⚠️ No se encontró sprite del corazón. Se usará un corazón rojo básico.");
        }
    }
    
    // Método público para configurar manualmente desde el Inspector
    public void ManualSetup()
    {
        SetupHeartSystem();
    }
}