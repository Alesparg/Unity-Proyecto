using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CartelVictoria : MonoBehaviour
{
    [SerializeField] private string mensaje = "¡Has ganado! ¡Felicidades!";
    [SerializeField] private string textoBoton = "Salir";
    [SerializeField] private Color colorFondo = new Color(0f, 0f, 0f, 0.8f);
    [SerializeField] private Color colorEstrella = new Color(1f, 0.82f, 0.15f, 1f);
    [SerializeField] private int estrellasMaximas = 3;

    private readonly List<Image> estrellas = new List<Image>();
    private GameObject cartel;
    private bool mostrado;

    private void Awake()
    {
        CrearCartel();
        cartel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (mostrado) { return; }
        if (!collision.gameObject.CompareTag("meta")) { return; }

        mostrado = true;
        ActualizarEstrellas(RecogerColeccionable.Recogidos);
        cartel.SetActive(true);
    }

    public void SalirDelJuego()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void ActualizarEstrellas(int recogidos)
    {
        int cantidad = Mathf.Clamp(recogidos, 0, estrellasMaximas);

        for (int i = 0; i < estrellas.Count; i++)
        {
            estrellas[i].gameObject.SetActive(i < cantidad);
        }
    }

    private void CrearCartel()
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();

        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("CanvasVictoria", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvas = canvasObj.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }

        cartel = new GameObject("CartelVictoria", typeof(RectTransform), typeof(Image));
        cartel.transform.SetParent(canvas.transform, false);

        Estirar(cartel.GetComponent<RectTransform>());
        cartel.GetComponent<Image>().color = colorFondo;

        TextMeshProUGUI texto = CrearTexto(cartel.transform, "TextoVictoria", mensaje, TamanoTituloNitido());
        Colocar(texto.rectTransform, new Vector2(0f, 150f), new Vector2(1400f, 240f));

        CrearEstrellas(cartel.transform);

        Button boton = CrearBoton(cartel.transform, "BotonSalir", textoBoton);
        Colocar(boton.GetComponent<RectTransform>(), new Vector2(0f, -230f), new Vector2(320f, 100f));

        boton.onClick.AddListener(SalirDelJuego);
    }

    private void CrearEstrellas(Transform padre)
    {
        GameObject contenedor = new GameObject("Estrellas", typeof(RectTransform));
        contenedor.transform.SetParent(padre, false);
        Colocar(contenedor.GetComponent<RectTransform>(), new Vector2(0f, -20f), new Vector2(700f, 170f));

        Sprite sprite = CrearSpriteEstrella();
        float separacion = 180f;

        for (int i = 0; i < estrellasMaximas; i++)
        {
            GameObject obj = new GameObject("Estrella" + (i + 1), typeof(RectTransform), typeof(Image));
            obj.transform.SetParent(contenedor.transform, false);

            RectTransform rt = obj.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = new Vector2(150f, 150f);
            rt.anchoredPosition = new Vector2((i - (estrellasMaximas - 1) * 0.5f) * separacion, 0f);

            Image imagen = obj.GetComponent<Image>();
            imagen.sprite = sprite;
            imagen.color = colorEstrella;
            imagen.raycastTarget = false;

            estrellas.Add(imagen);
        }
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

        // Aprieta el borde del SDF para que el texto no se vea difuminado.
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
        imagen.color = new Color(0.95f, 0.65f, 0.1f, 1f);

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
        // El atlas del font asset esta rasterizado a este tamano: usarlo tal cual
        // evita reescalar los glifos y es lo que mantiene el texto nitido.
        if (TMP_Settings.defaultFontAsset == null) { return 86f; }

        return Mathf.Round(TMP_Settings.defaultFontAsset.faceInfo.pointSize);
    }

    private Sprite CrearSpriteEstrella()
    {
        const int resolucion = 256;
        const float radioExterior = 0.5f;
        const float radioInterior = 0.225f;

        Vector2[] contorno = new Vector2[10];

        for (int i = 0; i < contorno.Length; i++)
        {
            float radio = (i % 2 == 0) ? radioExterior : radioInterior;
            float angulo = Mathf.Deg2Rad * (90f + i * 36f);
            contorno[i] = new Vector2(Mathf.Cos(angulo) * radio, Mathf.Sin(angulo) * radio);
        }

        Texture2D textura = new Texture2D(resolucion, resolucion, TextureFormat.RGBA32, false);
        textura.filterMode = FilterMode.Bilinear;
        textura.wrapMode = TextureWrapMode.Clamp;

        Color32[] pixeles = new Color32[resolucion * resolucion];

        for (int y = 0; y < resolucion; y++)
        {
            for (int x = 0; x < resolucion; x++)
            {
                float cobertura = 0f;

                for (int sy = 0; sy < 2; sy++)
                {
                    for (int sx = 0; sx < 2; sx++)
                    {
                        float px = (x + (sx + 0.5f) * 0.5f) / resolucion - 0.5f;
                        float py = (y + (sy + 0.5f) * 0.5f) / resolucion - 0.5f;

                        if (DentroDelPoligono(new Vector2(px, py), contorno)) { cobertura += 0.25f; }
                    }
                }

                pixeles[y * resolucion + x] = new Color(1f, 1f, 1f, cobertura);
            }
        }

        textura.SetPixels32(pixeles);
        textura.Apply();

        return Sprite.Create(textura, new Rect(0f, 0f, resolucion, resolucion), new Vector2(0.5f, 0.5f), 100f);
    }

    private static bool DentroDelPoligono(Vector2 punto, Vector2[] poligono)
    {
        bool dentro = false;
        int j = poligono.Length - 1;

        for (int i = 0; i < poligono.Length; i++)
        {
            if ((poligono[i].y > punto.y) != (poligono[j].y > punto.y))
            {
                float cruce = (poligono[j].x - poligono[i].x) * (punto.y - poligono[i].y) / (poligono[j].y - poligono[i].y) + poligono[i].x;

                if (punto.x < cruce) { dentro = !dentro; }
            }

            j = i;
        }

        return dentro;
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
