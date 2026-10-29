using UnityEditor;
using UnityEngine;
using TMPro;

[InitializeOnLoad]
public static class ActualizarRecursosTMP
{
    private const string RutaSettings = "Assets/TextMesh Pro/Resources/TMP Settings.asset";
    private const string VersionObjetivo = "2";
    private const string ClaveSesion = "Aura.TMPRecursosActualizados";

    static ActualizarRecursosTMP()
    {
        EditorApplication.delayCall += IntentarActualizar;
        EditorApplication.playModeStateChanged += AlCambiarModoDeJuego;
        AssetDatabase.importPackageFailed += AlFallarImportacion;
    }

    private static void AlCambiarModoDeJuego(PlayModeStateChange cambio)
    {
        if (cambio == PlayModeStateChange.EnteredEditMode)
        {
            EditorApplication.delayCall += IntentarActualizar;
        }
    }

    private static void IntentarActualizar()
    {
        // ImportPackage no funciona en Play mode: esperamos a estar en el editor.
        if (EditorApplication.isPlaying || EditorApplication.isPlayingOrWillChangePlaymode) { return; }
        if (EditorApplication.isCompiling || EditorApplication.isUpdating) { return; }

        if (SessionState.GetBool(ClaveSesion, false)) { return; }
        if (VersionActual() == VersionObjetivo) { return; }

        SessionState.SetBool(ClaveSesion, true);

        Debug.Log("[TMP] Recursos esenciales desactualizados. Importando los del paquete com.unity.ugui...");

        AssetDatabase.importPackageCompleted -= AlTerminarImportacion;
        AssetDatabase.importPackageCompleted += AlTerminarImportacion;

        TMP_PackageResourceImporter.ImportResources(true, false, false);
    }

    private static void AlTerminarImportacion(string nombrePaquete)
    {
        if (nombrePaquete != "TMP Essential Resources") { return; }

        AssetDatabase.importPackageCompleted -= AlTerminarImportacion;

        MarcarVersion();

        Debug.Log("[TMP] Recursos esenciales actualizados.");
    }

    private static void AlFallarImportacion(string nombrePaquete, string error)
    {
        if (nombrePaquete != "TMP Essential Resources") { return; }

        // Permite reintentar en el siguiente reload.
        SessionState.SetBool(ClaveSesion, false);

        Debug.LogError("[TMP] No se pudieron importar los recursos esenciales: " + error);
    }

    private static void MarcarVersion()
    {
        TMP_Settings settings = AssetDatabase.LoadAssetAtPath<TMP_Settings>(RutaSettings);

        if (settings == null)
        {
            Debug.LogError("[TMP] No se encontro " + RutaSettings);
            return;
        }

        SerializedObject serializado = new SerializedObject(settings);
        SerializedProperty version = serializado.FindProperty("assetVersion");

        if (version == null) { return; }

        version.stringValue = VersionObjetivo;
        serializado.ApplyModifiedProperties();

        EditorUtility.SetDirty(settings);
        AssetDatabase.SaveAssetIfDirty(settings);
    }

    private static string VersionActual()
    {
        TMP_Settings settings = AssetDatabase.LoadAssetAtPath<TMP_Settings>(RutaSettings);

        if (settings == null) { return null; }

        SerializedObject serializado = new SerializedObject(settings);
        SerializedProperty version = serializado.FindProperty("assetVersion");

        return version != null ? version.stringValue : null;
    }
}
