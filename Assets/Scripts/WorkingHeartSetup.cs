using UnityEngine;

public class WorkingHeartSetup : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private Sprite heartSprite; // Arrastra tu imagen corazon-removebg-preview.png aquí
    
    void Start()
    {
        // Crear WorkingHeartSystem si no existe
        WorkingHeartSystem heartSystem = FindObjectOfType<WorkingHeartSystem>();
        if (heartSystem == null)
        {
            GameObject heartSystemGO = new GameObject("WorkingHeartSystem");
            heartSystem = heartSystemGO.AddComponent<WorkingHeartSystem>();
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
