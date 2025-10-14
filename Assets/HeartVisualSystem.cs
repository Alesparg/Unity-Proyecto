using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartVisualSystem : MonoBehaviour
{
    [Header("Configuración Visual")]
    [SerializeField] private Sprite heartSprite; 
    [SerializeField] private int maxHearts = 5;
    [SerializeField] private Vector2 heartSize = new Vector2(80, 80); 

    private List<Image> heartImages = new List<Image>();
    private int currentHearts;
    private Transform heartContainer;

    void Start()
    {
        CreateVisualHearts();
    }

    void CreateVisualHearts()
    {
        // Crear Canvas si no existe
        Canvas canvas = Object.FindFirstObjectByType<Canvas>();
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
        layout.spacing = 15;
        layout.childControlWidth = false;
        layout.childControlHeight = false;
        layout.childForceExpandWidth = false;
        layout.childForceExpandHeight = false;

        // Posicionar en esquina superior izquierda
        RectTransform rect = containerGO.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 1);
        rect.anchorMax = new Vector2(0, 1);
        rect.pivot = new Vector2(0, 1);
        rect.anchoredPosition = new Vector2(30, -30);

        // Crear corazones
        currentHearts = maxHearts;
        for (int i = 0; i < maxHearts; i++)
        {
            CreateHeartImage();
        }

        Debug.Log("Sistema visual de corazones creado con " + maxHearts + " corazones");
        Debug.Log("Sprite asignado: " + (heartSprite != null ? heartSprite.name : "NINGUNO"));
    }

    void CreateHeartImage()
    {
        GameObject heartGO = new GameObject("Heart_" + (heartImages.Count + 1));
        heartGO.transform.SetParent(heartContainer);

        
        Image heartImage = heartGO.AddComponent<Image>();

        if (heartSprite != null)
        {
            heartImage.sprite = heartSprite;
            Debug.Log("Corazón " + (heartImages.Count + 1) + " creado con sprite: " + heartSprite.name);
        }
        else
        {
            // Crear un corazón rojo visible si no hay sprite
            heartImage.color = Color.red;
            Debug.LogWarning("¡NO HAY SPRITE! Creando corazón rojo básico");
        }

        // Configurar tamaño
        RectTransform rect = heartGO.GetComponent<RectTransform>();
        rect.sizeDelta = heartSize;

        heartImages.Add(heartImage);
        Debug.Log("Corazón " + heartImages.Count + " añadido. Total: " + heartImages.Count);
    }

    public void LoseHeart()
    {
        if (currentHearts > 0)
        {
            currentHearts--;

            if (heartImages.Count > currentHearts && heartImages[currentHearts] != null)
            {
                heartImages[currentHearts].enabled = false;
                Debug.Log("¡CORAZÓN OCULTO! Restantes: " + currentHearts);
            }
            else
            {
                Debug.LogError("No se pudo ocultar el corazón. Índice: " + currentHearts + ", Total corazones: " + heartImages.Count);
            }

            if (currentHearts <= 0)
            {
                Debug.Log("¡GAME OVER!");
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

            if (heartImages.Count >= currentHearts && heartImages[currentHearts - 1] != null)
            {
                heartImages[currentHearts - 1].enabled = true;
                Debug.Log("¡CORAZÓN MOSTRADO! Actuales: " + currentHearts);
            }
        }
        else
        {
            Debug.Log("Ya tienes todos los corazones!");
        }
    }

    public void ResetHearts()
    {
        currentHearts = maxHearts;
        foreach (Image heart in heartImages)
        {
            if (heart != null)
            {
                heart.enabled = true;
            }
        }
        Debug.Log("Corazones reseteados a " + maxHearts);
    }

    public int GetCurrentHearts()
    {
        return currentHearts;
    }
}

