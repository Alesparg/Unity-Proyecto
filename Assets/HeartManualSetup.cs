using UnityEngine;

public class HeartManualSetup : MonoBehaviour
{
    [Header("Configuración Manual")]
    [SerializeField] private Sprite heartSprite; // Arrastra tu imagen corazon-removebg-preview.png aquí
    
    void Start()
    {
        // Crear HeartSystem si no existe
        HeartSystem heartSystem = FindObjectOfType<HeartSystem>();
        if (heartSystem == null)
        {
            GameObject heartSystemGO = new GameObject("HeartSystem");
            heartSystem = heartSystemGO.AddComponent<HeartSystem>();
        }
        
        // Configurar manualmente
        if (heartSprite != null)
        {
            heartSystem.heartSprite = heartSprite;
            heartSystem.maxHearts = 5;
            heartSystem.heartSize = new Vector2(50, 50);
            heartSystem.heartColor = Color.red;
            
            Debug.Log("Sistema de corazones configurado manualmente con: " + heartSprite.name);
        }
        else
        {
            Debug.LogError("¡NO HAS ARRASTRADO LA IMAGEN DEL CORAZÓN!");
            Debug.LogError("Arrastra tu imagen 'corazon-removebg-preview.png' al campo 'Heart Sprite'");
        }
    }
}
