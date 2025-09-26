using UnityEngine;

public class HeartAutoSetup : MonoBehaviour
{
    void Start()
    {
        // Buscar la imagen del corazón automáticamente
        Sprite heartSprite = FindHeartSprite();
        
        if (heartSprite != null)
        {
            // Crear HeartSystem si no existe
            HeartSystem heartSystem = FindObjectOfType<HeartSystem>();
            if (heartSystem == null)
            {
                GameObject heartSystemGO = new GameObject("HeartSystem");
                heartSystem = heartSystemGO.AddComponent<HeartSystem>();
            }
            
            // Configurar automáticamente
            heartSystem.heartSprite = heartSprite;
            heartSystem.maxHearts = 5;
            heartSystem.heartSize = new Vector2(50, 50);
            heartSystem.heartColor = Color.red;
            
            Debug.Log("Sistema de corazones configurado automáticamente!");
        }
        else
        {
            Debug.LogWarning("No se encontró la imagen del corazón. Crea el HeartSystem manualmente.");
        }
    }
    
    Sprite FindHeartSprite()
    {
        // Buscar la imagen del corazón por nombre
        Sprite[] allSprites = Resources.FindObjectsOfTypeAll<Sprite>();
        
        foreach (Sprite sprite in allSprites)
        {
            if (sprite.name.Contains("corazon") || sprite.name.Contains("heart"))
            {
                return sprite;
            }
        }
        
        return null;
    }
}
