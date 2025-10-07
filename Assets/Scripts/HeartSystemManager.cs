using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartSystemManager : MonoBehaviour
{
    [Header("Configuración")]
    public Sprite heartSprite; // Arrastra tu imagen corazon-removebg-preview.png aquí
    public int maxHearts = 5;
    public Vector2 heartSize = new Vector2(60, 60);
    public Vector2 heartPosition = new Vector2(30, -30);
    public float heartSpacing = 10f;

    [Header("Debug")]
    public bool showDebugInfo = true;

    private List<GameObject> heartObjects = new List<GameObject>();
    private int currentHearts;
    private Canvas heartCanvas;
    private GameObject heartContainer;

    void Start()
    {
        // Buscar si ya existe un sistema de corazones
        HeartSystemManager existing = Object.FindFirstObjectByType<HeartSystemManager>();
        if (existing != null && existing != this)
        {
            Debug.LogWarning("Ya existe un HeartSystemManager. Destruyendo duplicado.");
            Destroy(gameObject);
            return;
        }

        SetupHeartSystem();
    }

    void SetupHeartSystem()
    {
        Debug.Log("🔧 Configurando sistema de corazones...");
        
        // Crear o encontrar Canvas
        SetupCanvas();
        
        // Crear contenedor de corazones
        SetupHeartContainer();
        
        // Configurar sprite del corazón
        SetupHeartSprite();
        
        // Crear corazones iniciales
        CreateInitialHearts();
        
        Debug.Log($"✅ Sistema de corazones configurado correctamente con {maxHearts} corazones");
    }

    void SetupCanvas()
    {
        // Buscar Canvas existente
        heartCanvas = Object.FindFirstObjectByType<Canvas>();
        
        if (heartCanvas == null)
        {
            Debug.Log("📱 Creando Canvas para corazones...");
            GameObject canvasGO = new GameObject("HeartCanvas");
            heartCanvas = canvasGO.AddComponent<Canvas>();
            heartCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            heartCanvas.sortingOrder = 100; // Asegurar que esté encima
            
            // Añadir componentes necesarios
            CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            
            canvasGO.AddComponent<GraphicRaycaster>();
        }
        else
        {
            Debug.Log("📱 Usando Canvas existente");
        }
    }

    void SetupHeartContainer()
    {
        // Buscar contenedor existente
        heartContainer = GameObject.Find("HeartContainer");
        
        if (heartContainer == null)
        {
            Debug.Log("📦 Creando contenedor de corazones...");
            heartContainer = new GameObject("HeartContainer");
            heartContainer.transform.SetParent(heartCanvas.transform, false);
            
            // Configurar RectTransform
            RectTransform rect = heartContainer.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0, 1);
            rect.anchorMax = new Vector2(0, 1);
            rect.pivot = new Vector2(0, 1);
            rect.anchoredPosition = heartPosition;
            
            // Añadir Layout Group
            HorizontalLayoutGroup layout = heartContainer.AddComponent<HorizontalLayoutGroup>();
            layout.spacing = heartSpacing;
            layout.childAlignment = TextAnchor.UpperLeft;
            layout.childControlWidth = false;
            layout.childControlHeight = false;
            layout.childForceExpandWidth = false;
            layout.childForceExpandHeight = false;
        }
        else
        {
            Debug.Log("📦 Usando contenedor existente");
        }
    }

    void SetupHeartSprite()
    {
        // Si no hay sprite asignado, intentar encontrarlo automáticamente
        if (heartSprite == null)
        {
            Debug.LogWarning("⚠️ No hay sprite asignado. Buscando automáticamente...");
            
            // Buscar el sprite del corazón por nombre
            Sprite[] allSprites = Resources.FindObjectsOfTypeAll<Sprite>();
            foreach (Sprite sprite in allSprites)
            {
                if (sprite.name.ToLower().Contains("corazon") || 
                    sprite.name.ToLower().Contains("heart") ||
                    sprite.name.ToLower().Contains("vida"))
                {
                    heartSprite = sprite;
                    Debug.Log($"✅ Sprite encontrado automáticamente: {sprite.name}");
                    break;
                }
            }
            
            if (heartSprite == null)
            {
                Debug.LogError("❌ No se encontró sprite del corazón. Los corazones se mostrarán como cuadros rojos.");
            }
        }
        else
        {
            Debug.Log($"✅ Sprite del corazón asignado: {heartSprite.name}");
        }
    }

    void CreateInitialHearts()
    {
        // Limpiar corazones existentes
        ClearAllHearts();
        
        // Crear corazones iniciales
        currentHearts = maxHearts;
        for (int i = 0; i < maxHearts; i++)
        {
            CreateHeart();
        }
        
        if (showDebugInfo)
        {
            Debug.Log($"❤️ Creados {maxHearts} corazones iniciales");
        }
    }

    void CreateHeart()
    {
        GameObject heart = new GameObject($"Heart_{heartObjects.Count}");
        heart.transform.SetParent(heartContainer.transform, false);
        
        // Añadir componente Image
        Image heartImage = heart.AddComponent<Image>();
        
        if (heartSprite != null)
        {
            heartImage.sprite = heartSprite;
            heartImage.color = Color.white;
        }
        else
        {
            // Crear corazón rojo básico si no hay sprite
            heartImage.color = Color.red;
            Debug.LogWarning("⚠️ Usando corazón rojo básico (sin sprite)");
        }
        
        // Configurar tamaño
        RectTransform rect = heart.GetComponent<RectTransform>();
        rect.sizeDelta = heartSize;
        
        heartObjects.Add(heart);
        
        if (showDebugInfo)
        {
            Debug.Log($"❤️ Corazón {heartObjects.Count} creado");
        }
    }

    public void LoseHeart()
    {
        if (currentHearts > 0)
        {
            currentHearts--;
            
            // Ocultar el corazón correspondiente
            if (currentHearts >= 0 && currentHearts < heartObjects.Count && heartObjects[currentHearts] != null)
            {
                heartObjects[currentHearts].SetActive(false);
                Debug.Log($"💔 Corazón perdido! Restantes: {currentHearts}");
            }
            else
            {
                Debug.LogError($"❌ Error al ocultar corazón. Índice: {currentHearts}, Total: {heartObjects.Count}");
            }
            
            // Verificar game over
            if (currentHearts <= 0)
            {
                Debug.Log("💀 ¡GAME OVER!");
                // Aquí puedes llamar a tu sistema de Game Over
            }
        }
        else
        {
            Debug.Log("⚠️ Ya no tienes corazones para perder!");
        }
    }

    public void GainHeart()
    {
        if (currentHearts < maxHearts)
        {
            currentHearts++;
            
            // Mostrar el corazón correspondiente
            if (currentHearts > 0 && currentHearts <= heartObjects.Count && heartObjects[currentHearts - 1] != null)
            {
                heartObjects[currentHearts - 1].SetActive(true);
                Debug.Log($"❤️ Corazón ganado! Actuales: {currentHearts}");
            }
            else
            {
                Debug.LogError($"❌ Error al mostrar corazón. Índice: {currentHearts - 1}, Total: {heartObjects.Count}");
            }
        }
        else
        {
            Debug.Log("❤️ Ya tienes el máximo de corazones!");
        }
    }

    public void ResetHearts()
    {
        currentHearts = maxHearts;
        
        // Mostrar todos los corazones
        for (int i = 0; i < heartObjects.Count; i++)
        {
            if (heartObjects[i] != null)
            {
                heartObjects[i].SetActive(true);
            }
        }
        
        Debug.Log($"🔄 Corazones reseteados a {maxHearts}");
    }

    void ClearAllHearts()
    {
        foreach (GameObject heart in heartObjects)
        {
            if (heart != null)
            {
                DestroyImmediate(heart);
            }
        }
        heartObjects.Clear();
    }

    public int GetCurrentHearts()
    {
        return currentHearts;
    }

    public int GetMaxHearts()
    {
        return maxHearts;
    }

    // Método para debug
    void Update()
    {
        if (showDebugInfo && Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log($"💓 Estado actual: {currentHearts}/{maxHearts} corazones");
            Debug.Log($"💓 Objetos de corazón: {heartObjects.Count}");
            Debug.Log($"💓 Canvas: {(heartCanvas != null ? "✅" : "❌")}");
            Debug.Log($"💓 Contenedor: {(heartContainer != null ? "✅" : "❌")}");
            Debug.Log($"💓 Sprite: {(heartSprite != null ? heartSprite.name : "❌ Ninguno")}");
        }
    }

    void OnDestroy()
    {
        // Limpiar al destruir
        ClearAllHearts();
    }
}
