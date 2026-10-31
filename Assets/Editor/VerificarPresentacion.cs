using System.Text;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class VerificarPresentacion
{
    public static string Execute()
    {
        var sb = new StringBuilder();

        sb.AppendLine("BuildSettings:");
        for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            sb.AppendLine("  [" + i + "] " + EditorBuildSettings.scenes[i].path + " enabled=" + EditorBuildSettings.scenes[i].enabled);

        var botonGO = GameObject.Find("Canvas/BotonJugar");
        if (botonGO == null)
        {
            sb.AppendLine("BotonJugar NO encontrado");
            return sb.ToString();
        }

        var boton = botonGO.GetComponent<Button>();
        sb.AppendLine("BotonJugar encontrado. Button=" + (boton != null));
        if (boton != null)
        {
            int n = boton.onClick.GetPersistentEventCount();
            sb.AppendLine("  onClick persistentes=" + n);
            for (int i = 0; i < n; i++)
                sb.AppendLine("    [" + i + "] target=" + boton.onClick.GetPersistentTarget(i) +
                              " metodo=" + boton.onClick.GetPersistentMethodName(i));
        }

        var menu = botonGO.GetComponent<MenuPresentacion>();
        sb.AppendLine("  MenuPresentacion=" + (menu != null));

        foreach (var texto in Object.FindObjectsByType<TextMeshProUGUI>(FindObjectsSortMode.None))
        {
            sb.AppendLine("TMP '" + texto.gameObject.name + "' = \"" + texto.text + "\" size=" + texto.fontSize +
                          " font=" + (texto.font != null ? texto.font.name : "NULL") +
                          " mat=" + (texto.fontSharedMaterial != null ? texto.fontSharedMaterial.name : "NULL"));
        }

        EditorApplication.ExecuteMenuItem("File/Save Project");
        AssetDatabase.SaveAssets();
        sb.AppendLine("Proyecto guardado.");
        return sb.ToString();
    }
}
