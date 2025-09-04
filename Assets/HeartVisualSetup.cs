using UnityEngine;

public class HeartVisualSetup : MonoBehaviour
{
    [Header("Configuración Visual")]
    [SerializeField] private Sprite heartSprite; // Arrastra tu imagen corazon-removebg-preview.png aquí
    
    void Start()
    {
        // Crear HeartVisualSystem si no existe
        HeartVisualSystem heartSystem = FindObjectOfType<HeartVisualSystem>();
        if (heartSystem == null)
        {
            GameObject heartSystemGO = new GameObject("HeartVisualSystem");
            heartSystem = heartSystemGO.AddComponent<HeartVisualSystem>();
        }
        
        // Configurar el sprite
        if (heartSprite != null)
        {
            // Usar reflexión para asignar el sprite privado
            var spriteField = typeof(HeartVisualSystem).GetField("heartSprite", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (spriteField != null)
            {
                spriteField.SetValue(heartSystem, heartSprite);
                Debug.Log("Sprite asignado: " + heartSprite.name);
            }
        }
        else
        {
            Debug.LogError("¡NO HAS ARRASTRADO LA IMAGEN DEL CORAZÓN!");
            Debug.LogError("Arrastra tu imagen 'corazon-removebg-preview.png' al campo 'Heart Sprite'");
        }
    }
}
