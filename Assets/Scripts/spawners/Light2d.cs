using UnityEngine;
using UnityEngine.Rendering.Universal; // Necesario para Light2D

public class ControlLuz2D : MonoBehaviour
{
    [Header("Configuración de Luz 2D")]
    [SerializeField] private Color colorLuz = Color.white;

    private Light2D luz2D;

    void Awake()
    {
        // Busca el componente Light2D en este GameObject
        luz2D = GetComponent<Light2D>();

        if (luz2D == null)
        {
            Debug.LogError("No se encontró un Light2D en este objeto. Agregá uno desde el Inspector.");
        }
    }

    void Start()
    {
        // Aplica el color inicial
        if (luz2D != null)
            luz2D.color = colorLuz;
    }

    // Método público para cambiar el color en tiempo de ejecución
    public void CambiarColor(Color nuevoColor)
    {
        colorLuz = nuevoColor;
        if (luz2D != null)
            luz2D.color = colorLuz;
    }
}

