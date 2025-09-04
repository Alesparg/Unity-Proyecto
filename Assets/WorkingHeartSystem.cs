using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkingHeartSystem : MonoBehaviour
{
    [Header("Configuración")]
    public Sprite heartSprite; // Arrastra tu imagen corazon-removebg-preview.png aquí
    public int maxHearts = 5;
    
    private List<GameObject> heartObjects = new List<GameObject>();
    private int currentHearts;
    
    void Start()
    {
        CreateHearts();
    }
    
    void CreateHearts()
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
        
        // Crear contenedor
        GameObject container = new GameObject("HeartContainer");
        container.transform.SetParent(canvas.transform);
        
        // Configurar layout
        HorizontalLayoutGroup layout = container.AddComponent<HorizontalLayoutGroup>();
        layout.spacing = 10;
        
        // Posicionar
        RectTransform rect = container.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 1);
        rect.anchorMax = new Vector2(0, 1);
        rect.pivot = new Vector2(0, 1);
        rect.anchoredPosition = new Vector2(20, -20);
        
        // Crear corazones
        currentHearts = maxHearts;
        for (int i = 0; i < maxHearts; i++)
        {
            CreateHeart(container.transform);
        }
        
        Debug.Log("Sistema de corazones creado con " + maxHearts + " corazones");
        Debug.Log("Sprite: " + (heartSprite != null ? heartSprite.name : "NINGUNO"));
    }
    
    void CreateHeart(Transform parent)
    {
        GameObject heart = new GameObject("Heart");
        heart.transform.SetParent(parent);
        
        Image heartImage = heart.AddComponent<Image>();
        
        if (heartSprite != null)
        {
            heartImage.sprite = heartSprite;
            Debug.Log("Corazón creado con sprite: " + heartSprite.name);
        }
        else
        {
            heartImage.color = Color.red;
            Debug.LogWarning("Sin sprite - usando corazón rojo");
        }
        
        RectTransform rect = heart.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(60, 60);
        
        heartObjects.Add(heart);
    }
    
    public void LoseHeart()
    {
        if (currentHearts > 0)
        {
            currentHearts--;
            
            if (heartObjects.Count > currentHearts && heartObjects[currentHearts] != null)
            {
                heartObjects[currentHearts].SetActive(false);
                Debug.Log("¡Corazón oculto! Restantes: " + currentHearts);
            }
            
            if (currentHearts <= 0)
            {
                Debug.Log("¡GAME OVER!");
            }
        }
    }
    
    public void GainHeart()
    {
        if (currentHearts < maxHearts)
        {
            currentHearts++;
            
            if (heartObjects.Count >= currentHearts && heartObjects[currentHearts - 1] != null)
            {
                heartObjects[currentHearts - 1].SetActive(true);
                Debug.Log("¡Corazón mostrado! Actuales: " + currentHearts);
            }
        }
    }
    
    public void ResetHearts()
    {
        currentHearts = maxHearts;
        foreach (GameObject heart in heartObjects)
        {
            if (heart != null)
            {
                heart.SetActive(true);
            }
        }
        Debug.Log("Corazones reseteados");
    }
}
