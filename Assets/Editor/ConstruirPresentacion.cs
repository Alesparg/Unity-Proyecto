using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class ConstruirPresentacion
{
    private const string RutaEscena = "Assets/Scenes/Presentacion.unity";
    private const string RutaJuego = "Assets/Scenes/SampleScene.unity";

    public static void Execute()
    {
        EditorSceneManager.SaveOpenScenes();

        var escena = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

        ConfigurarCamara();

        var canvas = CrearCanvas();
        var fuente = ObtenerFuente();

        var fondo = CrearRect("Fondo", canvas.transform);
        var fondoImg = fondo.gameObject.AddComponent<Image>();
        fondoImg.color = new Color(0.05f, 0.08f, 0.15f, 1f);
        fondoImg.raycastTarget = false;
        Estirar(fondo);

        CrearTexto("Titulo", canvas.transform, "El Mundo Salvaje", 130f,
            new Color(1f, 0.96f, 0.82f, 1f), fuente, new Vector2(0f, 190f), new Vector2(1700f, 240f));

        CrearTexto("Autor", canvas.transform, "Creado por Alejandro Spengler", 46f,
            new Color(0.72f, 0.80f, 0.94f, 1f), fuente, new Vector2(0f, 45f), new Vector2(1400f, 90f));

        CrearBoton(canvas.transform, fuente);
        CrearEventSystem();

        EditorSceneManager.MarkSceneDirty(escena);
        EditorSceneManager.SaveScene(escena, RutaEscena);

        EditorBuildSettings.scenes = new List<EditorBuildSettingsScene>
        {
            new EditorBuildSettingsScene(RutaEscena, true),
            new EditorBuildSettingsScene(RutaJuego, true)
        }.ToArray();

        AssetDatabase.Refresh();
        Debug.Log("[Presentacion] Escena creada en " + RutaEscena + " y registrada como escena inicial.");
    }

    private static void ConfigurarCamara()
    {
        var camara = UnityEngine.Object.FindFirstObjectByType<Camera>();
        if (camara == null) return;

        camara.clearFlags = CameraClearFlags.SolidColor;
        camara.backgroundColor = new Color(0.05f, 0.08f, 0.15f, 1f);
        camara.orthographic = true;
        camara.orthographicSize = 5f;
    }

    private static Canvas CrearCanvas()
    {
        var go = new GameObject("Canvas", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        go.layer = LayerMask.NameToLayer("UI");

        var canvas = go.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        var escalador = go.GetComponent<CanvasScaler>();
        escalador.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        escalador.referenceResolution = new Vector2(1920f, 1080f);
        escalador.matchWidthOrHeight = 0.5f;

        return canvas;
    }

    private static void CrearEventSystem()
    {
        var go = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        go.layer = LayerMask.NameToLayer("UI");
    }

    private static TMP_FontAsset ObtenerFuente()
    {
        var fuente = TMP_Settings.defaultFontAsset;
        if (fuente == null)
            fuente = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
        return fuente;
    }

    private static RectTransform CrearRect(string nombre, Transform padre)
    {
        var go = new GameObject(nombre, typeof(RectTransform));
        go.layer = LayerMask.NameToLayer("UI");
        go.transform.SetParent(padre, false);
        return go.GetComponent<RectTransform>();
    }

    private static void Estirar(RectTransform rt)
    {
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }

    private static TextMeshProUGUI CrearTexto(string nombre, Transform padre, string contenido, float tamano,
        Color color, TMP_FontAsset fuente, Vector2 posicion, Vector2 tamanoRect)
    {
        var rt = CrearRect(nombre, padre);
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = tamanoRect;
        rt.anchoredPosition = posicion;

        var texto = rt.gameObject.AddComponent<TextMeshProUGUI>();
        if (fuente != null) texto.font = fuente;
        texto.text = contenido;
        texto.fontSize = tamano;
        texto.color = color;
        texto.alignment = TextAlignmentOptions.Center;
        texto.raycastTarget = false;
        return texto;
    }

    private static void CrearBoton(Transform padre, TMP_FontAsset fuente)
    {
        var rt = CrearRect("BotonJugar", padre);
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.sizeDelta = new Vector2(440f, 130f);
        rt.anchoredPosition = new Vector2(0f, -230f);

        var imagen = rt.gameObject.AddComponent<Image>();
        imagen.color = new Color(0.17f, 0.56f, 0.36f, 1f);

        var boton = rt.gameObject.AddComponent<Button>();
        boton.targetGraphic = imagen;

        var menu = rt.gameObject.AddComponent<MenuPresentacion>();
        UnityEventTools.AddPersistentListener(boton.onClick, menu.Jugar);

        var etiqueta = CrearTexto("Texto", rt, "Jugar", 62f, Color.white, fuente, Vector2.zero, Vector2.zero);
        Estirar((RectTransform)etiqueta.transform);
    }
}
