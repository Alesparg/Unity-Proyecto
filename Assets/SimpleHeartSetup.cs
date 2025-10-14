
using UnityEngine;

public class SimpleHeartSetup : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private Sprite heartSprite;

    void Start()
    {
        // Crear SimpleHeartSystem si no existe
        SimpleHeartSystem heartSystem = Object.FindFirstObjectByType<SimpleHeartSystem>();
        if (heartSystem == null)
        {
            GameObject heartSystemGO = new GameObject("SimpleHeartSystem");
            heartSystem = heartSystemGO.AddComponent<SimpleHeartSystem>();
        }

        
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
