using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartSystem : MonoBehaviour
{
    [Header("Configuración")]
    public Sprite heartSprite; // Arrastra tu imagen corazon-removebg-preview.png aquí
    public int maxHearts = 5;
    public Vector2 heartSize = new Vector2(50, 50);
    public Color heartColor = Color.red;
    
    private List<GameObject> heartImages = new List<GameObject>();
    private int currentHearts;
    private Transform heartContainer;
    
    void Start()
    {
        CreateHeartSystem();
    }
    
    void CreateHeartSystem()
    {
        // Crear Canvas si no existe
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasGO = new GameObject("Canvas");
            canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();
        }
        
        // Crear contenedor de corazones
        GameObject containerGO = new GameObject("HeartContainer");
        containerGO.transform.SetParent(canvas.transform);
        heartContainer = containerGO.transform;
        
        // Configurar layout
        HorizontalLayoutGroup layout = containerGO.AddComponent<HorizontalLayoutGroup>();
        layout.spacing = 10;
        layout.childControlWidth = false;
        layout.childControlHeight = false;
        layout.childForceExpandWidth = false;
        layout.childForceExpandHeight = false;
        
        // Posicionar en esquina superior izquierda
        RectTransform rect = containerGO.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 1);
        rect.anchorMax = new Vector2(0, 1);
        rect.pivot = new Vector2(0, 1);
        rect.anchoredPosition = new Vector2(20, -20);
        
        // Crear corazones
        currentHearts = maxHearts;
        for (int i = 0; i < maxHearts; i++)
        {
            CreateHeartImage();
        }
        
        Debug.Log("Sistema de corazones creado con " + maxHearts + " corazones");
        Debug.Log("Imagen del corazón asignada: " + (heartSprite != null ? heartSprite.name : "NINGUNA"));
    }
    
    void CreateHeartImage()
    {
        GameObject heartGO = new GameObject("Heart");
        heartGO.transform.SetParent(heartContainer);
        
        // Añadir Image component
        Image heartImage = heartGO.AddComponent<Image>();
        if (heartSprite != null)
        {
            heartImage.sprite = heartSprite;
            Debug.Log("Corazón creado con sprite: " + heartSprite.name);
        }
        else
        {
            Debug.LogWarning("¡NO HAY SPRITE ASIGNADO AL CORAZÓN!");
            // Crear un corazón rojo básico si no hay sprite
            heartImage.color = Color.red;
        }
        
        // Configurar tamaño
        RectTransform rect = heartGO.GetComponent<RectTransform>();
        rect.sizeDelta = heartSize;
        
        heartImages.Add(heartGO);
        Debug.Log("Corazón " + heartImages.Count + " creado. Total: " + heartImages.Count);
    }
    
    public void LoseHeart()
    {
        if (currentHearts > 0)
        {
            currentHearts--;
            
            // Destruir el último corazón visible
            if (heartImages.Count > currentHearts && heartImages[currentHearts] != null)
            {
                GameObject heartToDestroy = heartImages[currentHearts];
                heartImages.RemoveAt(currentHearts);
                Destroy(heartToDestroy);
                Debug.Log("¡CORAZÓN ELIMINADO! Restantes: " + currentHearts);
            }
            else
            {
                Debug.LogError("No se pudo eliminar el corazón. Índice: " + currentHearts + ", Total corazones: " + heartImages.Count);
            }
            
            // Verificar game over
            if (currentHearts <= 0)
            {
                Debug.Log("¡GAME OVER!");
                // Aquí puedes llamar a tu sistema de Game Over
            }
        }
        else
        {
            Debug.Log("Ya no tienes corazones para perder!");
        }
    }
    
    public void GainHeart()
    {
        if (currentHearts < maxHearts)
        {
            currentHearts++;
            CreateHeartImage();
            Debug.Log("Corazón ganado. Actuales: " + currentHearts);
        }
    }
    
    public int GetCurrentHearts()
    {
        return currentHearts;
    }
    
    public void ResetHearts()
    {
        // Destruir todos los corazones existentes
        foreach (GameObject heart in heartImages)
        {
            if (heart != null)
            {
                DestroyImmediate(heart);
            }
        }
        heartImages.Clear();
        
        // Recrear todos los corazones
        currentHearts = maxHearts;
        for (int i = 0; i < maxHearts; i++)
        {
            CreateHeartImage();
        }
        Debug.Log("Corazones reseteados a " + maxHearts);
    }
}
