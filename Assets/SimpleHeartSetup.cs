using UnityEngine;

public class SimpleHeartSetup : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private Sprite heartSprite; // Arrastra tu imagen corazon-removebg-preview.png aquí
    
    void Start()
    {
        // Crear SimpleHeartSystem si no existe
        SimpleHeartSystem heartSystem = FindObjectOfType<SimpleHeartSystem>();
        if (heartSystem == null)
        {
            GameObject heartSystemGO = new GameObject("SimpleHeartSystem");
            heartSystem = heartSystemGO.AddComponent<SimpleHeartSystem>();
        }
        
        // Configurar el sprite
        if (heartSprite != null)
        {
            heartSystem.heartSprite = heartSprite;
            Debug.Log("Sprite asignado: " + heartSprite.name);
        }
        else
        {
            Debug.LogError("¡NO HAS ARRASTRADO LA IMAGEN DEL CORAZÓN!");
            Debug.LogError("Arrastra tu imagen 'corazon-removebg-preview.png' al campo 'Heart Sprite'");
        }
    }
}
