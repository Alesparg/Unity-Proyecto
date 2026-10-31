using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class VerificarGameManager
{
    public static string Execute()
    {
        var sb = new StringBuilder();

        bool guardado = EditorSceneManager.SaveOpenScenes();
        sb.AppendLine("SaveOpenScenes=" + guardado + " escenaActiva=" + SceneManager.GetActiveScene().path);

        var managers = Object.FindObjectsByType<GameManager>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        sb.AppendLine("Componentes GameManager en escena: " + managers.Length);
        foreach (var m in managers)
            sb.AppendLine("  en '" + m.gameObject.name + "' estrellasMaximas=" + m.EstrellasMaximas + " tiempoMaximo=" + m.TiempoMaximo + " HayLimiteDeTiempo=" + m.HayLimiteDeTiempo);

        var jugador = GameObject.Find("jugador");
        sb.AppendLine("jugador encontrado=" + (jugador != null));
        if (jugador != null)
        {
            sb.AppendLine("  CartelVictoria=" + (jugador.GetComponent<CartelVictoria>() != null));
            sb.AppendLine("  CartelDerrota=" + (jugador.GetComponent<CartelDerrota>() != null));
            sb.AppendLine("  Vidas=" + (jugador.GetComponent<Vidas>() != null));
        }

        var recogibles = Object.FindObjectsByType<RecogerColeccionable>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        sb.AppendLine("Objetos con RecogerColeccionable: " + recogibles.Length);
        foreach (var r in recogibles)
            sb.AppendLine("  " + r.gameObject.name + " tag=" + r.gameObject.tag);

        var corazones = Object.FindObjectsByType<HeartSystemManager>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        sb.AppendLine("HeartSystemManager en escena: " + corazones.Length);

        return sb.ToString();
    }
}
