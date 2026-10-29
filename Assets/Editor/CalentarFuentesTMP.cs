using System;
using System.Reflection;
using TMPro;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Fuerza la lectura de las tablas de caracteres de todas las fuentes TMP desde el hilo
/// principal en cada recarga de dominio.
///
/// Motivo: el sistema de texto de UI Toolkit (UITKTextJobSystem) genera el texto en hilos
/// de trabajo y llama de forma perezosa a FontAsset.ReadFontAssetDefinition(). Esa funcion
/// NO es segura entre hilos: dos trabajadores a la vez reconstruyen el diccionario de
/// caracteres y lo corrompen (ArgumentException "same key" / InvalidOperationException
/// "concurrent update"). Si la tabla ya existe antes de que arranquen los trabajos, la ruta
/// perezosa nunca se ejecuta y el fallo desaparece.
/// </summary>
[InitializeOnLoad]
internal static class CalentarFuentesTMP
{
    private const string Filtro = "t:TMP_FontAsset";
    private const string ClaveSesion = "Aura.TMPFuentesCalentadas";
    private static readonly PropertyInfo TablaCaracteres =
        typeof(TMP_FontAsset).GetProperty("characterLookupTable", BindingFlags.Public | BindingFlags.Instance);

    static CalentarFuentesTMP()
    {
        EditorApplication.delayCall += CalentarTodas;
    }

    private static void CalentarTodas()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode) { return; }
        if (TablaCaracteres == null) { return; }

        string[] guids = AssetDatabase.FindAssets(Filtro);
        int calentadas = 0;

        for (int i = 0; i < guids.Length; i++)
        {
            string ruta = AssetDatabase.GUIDToAssetPath(guids[i]);
            TMP_FontAsset fuente = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(ruta);

            if (fuente == null) { continue; }

            try
            {
                TablaCaracteres.GetValue(fuente);
                calentadas++;
            }
            catch (Exception e)
            {
                Debug.LogWarning("[TMP] No se pudo preparar la fuente " + ruta + ": " + e.Message);
            }
        }

        if (!SessionState.GetBool(ClaveSesion, false))
        {
            SessionState.SetBool(ClaveSesion, true);
            Debug.Log("[TMP] Tablas de caracteres preparadas en el hilo principal: " + calentadas + " fuente(s).");
        }
    }
}
