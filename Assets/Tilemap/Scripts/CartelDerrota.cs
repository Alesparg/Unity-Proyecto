using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Cartel de derrota. Sigue el mismo patrón que CartelVictoria: el panel se
// construye por código y sólo se muestra cuando el GameManager marca "Perdido".
public class CartelDerrota : MonoBehaviour
{
    [SerializeField] private string mensaje = "¡Moriste!";
    [SerializeField] private string textoBoton = "Reintentar";
    [SerializeField] private Color colorFondo = new Color(0f, 0f, 0f, 0.8f);

    private GameObject cartel;
    private bool mostrado;

    private void Awake()
    {
        CrearCartel();
        cartel.SetActive(false);
    }

    private void Start()
    {
        // El GameManager se ejecuta antes (DefaultExecutionOrder -100), pero se
        // comprueba de todos modos por si no existe en la escena.
        if (!GameManager.Existe)
        {
            Debug.LogWarning("CartelDerrota: no hay GameManager en la escena, el cartel no se mostrará.");
            return;
        }

        GameManager.Instance.EstadoCambiado += CuandoCambiaElEstado;
        CuandoCambiaElEstado(GameManager.Instance.Estado);
    }

    private void OnDisable()
    {
        if (GameManager.Existe) { GameManager.Instance.EstadoCambiado -= CuandoCambiaElEstado; }
    }

    private void CuandoCambiaElEstado(EstadoJuego estado)
    {
        if (estado != EstadoJuego.Perdido || mostrado) { return; }

        mostrado = true;
        cartel.SetActive(true);
    }

    public void Reintentar()
    {
        if (GameManager.Existe) { GameManager.Instance.ReiniciarNivel(); return; }

        Scene escenaActual = SceneManager.GetActiveScene();

        if (escenaActual.buildIndex >= 0) { SceneManager.LoadScene(escenaActual.buildIndex); }
        else { SceneManager.LoadScene(escenaActual.name); }
    }

    private void CrearCartel()
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();

        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("CanvasDerrota", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvas = canvasObj.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }

        cartel = new GameObject("CartelDerrota", typeof(RectTransform), typeof(Image));
        cartel.transform.SetParent(canvas.transform, false);

        Estirar(cartel.GetComponent<RectTransform>());
        cartel.GetComponent<Image>().color = colorFondo;

        TextMeshProUGUI texto = CrearTexto(cartel.transform, "TextoDerrota", mensaje, TamanoTituloNitido());
        Colocar(texto.rectTransform, new Vector2(0f, 90f), new Vector2(1400f, 240f));

        Button boton = CrearBoton(cartel.transform, "BotonReintentar", textoBoton);
        Colocar(boton.GetComponent<RectTransform>(), new Vector2(0f, -110f), new Vector2(360f, 100f));

        boton.onClick.AddListener(Reintentar);
    }

    private TextMeshProUGUI CrearTexto(Transform padre, string nombre, string contenido, float tamano)
    {
        GameObject obj = new GameObject(nombre, typeof(RectTransform), typeof(TextMeshProUGUI));
        obj.transform.SetParent(padre, false);

        TextMeshProUGUI texto = obj.GetComponent<TextMeshProUGUI>();

        if (TMP_Settings.defaultFontAsset != null)
        {
            texto.font = TMP_Settings.defaultFontAsset;
        }

        texto.text = contenido;
        texto.fontSize = tamano;
        texto.enableAutoSizing = false;
        texto.color = Color.white;
        texto.alignment = TextAlignmentOptions.Center;
        texto.raycastTarget = false;

        if (texto.fontMaterial != null)
        {
            texto.fontMaterial.SetFloat("_Sharpness", 0.25f);
        }

        return texto;
    }

    private Button CrearBoton(Transform padre, string nombre, string etiqueta)
    {
        GameObject obj = new GameObject(nombre, typeof(RectTransform), typeof(Image), typeof(Button));
        obj.transform.SetParent(padre, false);

        Image imagen = obj.GetComponent<Image>();
        imagen.color = new Color(0.85f, 0.25f, 0.2f, 1f);

        Button boton = obj.GetComponent<Button>();
        boton.targetGraphic = imagen;

        ColorBlock colores = boton.colors;
        colores.normalColor = Color.white;
        colores.highlightedColor = new Color(0.85f, 0.85f, 0.85f, 1f);
        colores.pressedColor = new Color(0.7f, 0.7f, 0.7f, 1f);
        boton.colors = colores;

        TextMeshProUGUI etiquetaTexto = CrearTexto(obj.transform, "Texto", etiqueta, 44f);
        Estirar(etiquetaTexto.rectTransform);

        return boton;
    }

    private float TamanoTituloNitido()
    {
        if (TMP_Settings.defaultFontAsset == null) { return 86f; }

        return Mathf.Round(TMP_Settings.defaultFontAsset.faceInfo.pointSize);
    }

    private static void Estirar(RectTransform rt)
    {
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }

    private static void Colocar(RectTransform rt, Vector2 posicion, Vector2 tamano)
    {
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = tamano;
        rt.anchoredPosition = new Vector2(Mathf.Round(posicion.x), Mathf.Round(posicion.y));
    }
}
