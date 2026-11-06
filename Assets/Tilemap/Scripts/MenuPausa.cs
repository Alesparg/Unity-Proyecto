using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Menú de pausa. Con la tecla configurada (H por defecto) se congela el juego
// (Time.timeScale = 0) y se muestra el panel con las opciones de reanudar o
// volver al menú principal. El panel se construye por código, igual que el
// resto de carteles del proyecto.
public class MenuPausa : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private KeyCode teclaPausa = KeyCode.H;
    [SerializeField] private string titulo = "PAUSA";
    [SerializeField] private string textoReanudar = "Reanudar";
    [SerializeField] private string textoMenu = "Menú principal";
    [SerializeField] private string escenaMenu = "Presentacion";
    [SerializeField] private Color colorFondo = new Color(0f, 0f, 0f, 0.72f);

    private GameObject panel;
    private bool pausado;

    public bool Pausado => pausado;

    private void Awake()
    {
        CrearPanel();
        panel.SetActive(false);
    }

    private void Update()
    {
        if (!Input.GetKeyDown(teclaPausa)) { return; }

        AlternarPausa();
    }

    private void OnDestroy()
    {
        // Si se cambia de escena con el juego pausado, el tiempo debe volver a correr
        if (pausado) { Time.timeScale = 1f; }
    }

    public void AlternarPausa()
    {
        if (pausado) { Reanudar(); return; }

        // Sobre los carteles de victoria o derrota el juego ya está detenido
        if (GameManager.Existe && GameManager.Instance.Estado != EstadoJuego.Jugando) { return; }

        Pausar();
    }

    public void Pausar()
    {
        pausado = true;
        Time.timeScale = 0f;

        if (panel != null) { panel.SetActive(true); }
    }

    public void Reanudar()
    {
        pausado = false;
        Time.timeScale = 1f;

        if (panel != null) { panel.SetActive(false); }
    }

    public void IrAlMenuPrincipal()
    {
        // Time.timeScale no se reinicia solo al cargar una escena
        pausado = false;
        Time.timeScale = 1f;

        SceneManager.LoadScene(escenaMenu);
    }

    private void CrearPanel()
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();

        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("CanvasPausa", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvas = canvasObj.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }

        panel = new GameObject("MenuPausa", typeof(RectTransform), typeof(Image));
        panel.transform.SetParent(canvas.transform, false);

        Estirar(panel.GetComponent<RectTransform>());
        panel.GetComponent<Image>().color = colorFondo;

        TextMeshProUGUI textoTitulo = CrearTexto(panel.transform, "TextoTitulo", titulo, TamanoTituloNitido());
        Colocar(textoTitulo.rectTransform, new Vector2(0f, 150f), new Vector2(1200f, 200f));

        Button botonReanudar = CrearBoton(panel.transform, "BotonReanudar", textoReanudar, new Color(0.24f, 0.55f, 0.32f, 1f));
        Colocar(botonReanudar.GetComponent<RectTransform>(), new Vector2(0f, -20f), new Vector2(360f, 100f));
        botonReanudar.onClick.AddListener(Reanudar);

        Button botonMenu = CrearBoton(panel.transform, "BotonMenu", textoMenu, new Color(0.25f, 0.36f, 0.58f, 1f));
        Colocar(botonMenu.GetComponent<RectTransform>(), new Vector2(0f, -150f), new Vector2(360f, 100f));
        botonMenu.onClick.AddListener(IrAlMenuPrincipal);
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

    private Button CrearBoton(Transform padre, string nombre, string etiqueta, Color color)
    {
        GameObject obj = new GameObject(nombre, typeof(RectTransform), typeof(Image), typeof(Button));
        obj.transform.SetParent(padre, false);

        Image imagen = obj.GetComponent<Image>();
        imagen.color = color;

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
