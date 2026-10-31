using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPresentacion : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private string escenaJuego = "SampleScene";
    [SerializeField] private string escenaControles = "ObjetivosControles";
    [SerializeField] private string escenaMenu = "Presentacion";

    public void Jugar()
    {
        SceneManager.LoadScene(escenaJuego);
    }

    public void AbrirControles()
    {
        SceneManager.LoadScene(escenaControles);
    }

    public void VolverAlMenu()
    {
        SceneManager.LoadScene(escenaMenu);
    }
}
