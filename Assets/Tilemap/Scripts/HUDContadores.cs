using TMPro;
using UnityEngine;

// Cronómetro y contador de enemigos eliminados en la esquina superior derecha.
// Se construye por código sobre el Canvas existente para no depender de prefabs.
public class HUDContadores : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float margen = 18f;
    [SerializeField] private float tamanoFuente = 28f;
    [SerializeField] private Color colorTiempo = new Color(1f, 0.96f, 0.82f, 1f);
    [SerializeField] private Color colorEnemigos = new Color(1f, 0.70f, 0.62f, 1f);

    private TextMeshProUGUI textoTiempo;
    private TextMeshProUGUI textoEnemigos;

    private void Awake()
    {
        textoTiempo = CrearTexto("HUDTiempo", 0, colorTiempo);
        textoEnemigos = CrearTexto("HUDEnemigos", 1, colorEnemigos);
    }

    private void Update()
    {
        if (textoTiempo == null || textoEnemigos == null) { return; }

        if (!GameManager.Existe)
        {
            textoTiempo.text = "TIEMPO --:--";
            textoEnemigos.text = "ENEMIGOS 0";
            return;
        }

        GameManager juego = GameManager.Instance;

        textoTiempo.text = "TIEMPO " + juego.TiempoTranscurridoFormateado();
        textoEnemigos.text = "ENEMIGOS " + juego.EnemigosEliminados;

        // Los carteles de victoria/derrota ya muestran estos datos: el HUD se oculta
        bool visible = juego.Estado == EstadoJuego.Jugando;

        if (textoTiempo.enabled != visible)
        {
            textoTiempo.enabled = visible;
            textoEnemigos.enabled = visible;
        }
    }

    private TextMeshProUGUI CrearTexto(string nombre, int indice, Color color)
    {
        Transform anterior = transform.Find(nombre);
        if (anterior != null) { Destroy(anterior.gameObject); }

        GameObject obj = new GameObject(nombre, typeof(RectTransform), typeof(TextMeshProUGUI));
        obj.layer = LayerMask.NameToLayer("UI");
        obj.transform.SetParent(transform, false);

        RectTransform rt = obj.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(1f, 1f);
        rt.anchorMax = new Vector2(1f, 1f);
        rt.pivot = new Vector2(1f, 1f);
        rt.sizeDelta = new Vector2(320f, tamanoFuente + 14f);
        rt.anchoredPosition = new Vector2(-margen, -margen - indice * (tamanoFuente + 10f));

        TextMeshProUGUI texto = obj.GetComponent<TextMeshProUGUI>();

        if (TMP_Settings.defaultFontAsset != null)
        {
            texto.font = TMP_Settings.defaultFontAsset;
        }

        texto.text = string.Empty;
        texto.fontSize = tamanoFuente;
        texto.enableAutoSizing = false;
        texto.color = color;
        texto.alignment = TextAlignmentOptions.TopRight;
        texto.raycastTarget = false;

        // Contorno negro para que los números se lean sobre cualquier fondo
        if (texto.fontMaterial != null)
        {
            texto.fontMaterial.SetFloat("_OutlineWidth", 0.2f);
            texto.fontMaterial.SetColor("_OutlineColor", new Color(0f, 0f, 0f, 0.9f));
        }

        return texto;
    }
}
