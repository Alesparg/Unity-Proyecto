using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EstadoJuego
{
    Jugando,
    Ganado,
    Perdido
}

// Controlador principal del juego: única instancia activa y estado general de la partida.
// Se ejecuta antes que el resto (DefaultExecutionOrder) para que cualquier Awake/Start
// que lo consulte ya lo encuentre disponible.
[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Estrellas que se muestran en el cartel de victoria")]
    [SerializeField] private int estrellasMaximas = 3;

    [Tooltip("Tiempo límite en segundos. 0 = sin límite (no hay cuenta atrás)")]
    [SerializeField] private float tiempoMaximo = 0f;

    public static GameManager Instance { get; private set; }

    // Permite consultarlo sin riesgo de NullReference: if (GameManager.Existe) ...
    public static bool Existe => Instance != null;

    public EstadoJuego Estado { get; private set; } = EstadoJuego.Jugando;
    public int EstrellasRecogidas { get; private set; }
    public int EstrellasMaximas => estrellasMaximas;
    public float TiempoRestante { get; private set; }
    public float TiempoMaximo => tiempoMaximo;
    public bool HayLimiteDeTiempo => tiempoMaximo > 0f;

    // Cronómetro de la partida: cuenta hacia arriba mientras se está jugando y se detiene al ganar o perder.
    public float TiempoTranscurrido { get; private set; }
    public int EnemigosEliminados { get; private set; }

    public event Action<int> EstrellasCambiadas;
    public event Action<EstadoJuego> EstadoCambiado;
    public event Action<float> TiempoCambiado;
    public event Action<int> EnemigosCambiados;

    private void Awake()
    {
        // Si ya hay una instancia activa, este duplicado se destruye y no toca el estado
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("GameManager duplicado (" + name + "): se destruye el sobrante.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        TiempoRestante = tiempoMaximo;
    }

    private void OnDestroy()
    {
        // Se libera la referencia para que una partida nueva pueda registrarse
        if (Instance == this) { Instance = null; }
    }

    private void Update()
    {
        if (Estado != EstadoJuego.Jugando) { return; }

        // El cronómetro corre siempre, haya o no límite de tiempo
        TiempoTranscurrido += Time.deltaTime;

        if (!HayLimiteDeTiempo) { return; }

        TiempoRestante = Mathf.Max(0f, TiempoRestante - Time.deltaTime);
        TiempoCambiado?.Invoke(TiempoRestante);

        if (TiempoRestante <= 0f) { Perder(); }
    }

    public void RegistrarEstrella() { RegistrarEstrellas(1); }

    public void RegistrarEstrellas(int cantidad)
    {
        if (cantidad <= 0 || Estado != EstadoJuego.Jugando) { return; }

        EstrellasRecogidas = Mathf.Clamp(EstrellasRecogidas + cantidad, 0, estrellasMaximas);
        EstrellasCambiadas?.Invoke(EstrellasRecogidas);
    }

    public void RegistrarEnemigoEliminado()
    {
        if (Estado != EstadoJuego.Jugando) { return; }

        EnemigosEliminados++;
        EnemigosCambiados?.Invoke(EnemigosEliminados);
    }

    public void Ganar()
    {
        if (Estado != EstadoJuego.Jugando) { return; }

        Estado = EstadoJuego.Ganado;
        EstadoCambiado?.Invoke(Estado);
        Debug.Log("GameManager: partida ganada con " + EstrellasRecogidas + " estrella(s), " +
                  EnemigosEliminados + " enemigo(s) eliminado(s) en " + TiempoTranscurridoFormateado() + ".");
    }

    public void Perder()
    {
        if (Estado != EstadoJuego.Jugando) { return; }

        Estado = EstadoJuego.Perdido;
        EstadoCambiado?.Invoke(Estado);
        Debug.Log("GameManager: partida perdida.");
    }

    public void ReiniciarNivel()
    {
        Scene escenaActual = SceneManager.GetActiveScene();

        if (escenaActual.buildIndex >= 0) { SceneManager.LoadScene(escenaActual.buildIndex); }
        else { SceneManager.LoadScene(escenaActual.name); }
    }

    // "mm:ss" listo para mostrar en el HUD
    public string TiempoFormateado()
    {
        if (!HayLimiteDeTiempo) { return "--:--"; }

        int totalSegundos = Mathf.CeilToInt(TiempoRestante);
        return (totalSegundos / 60).ToString("00") + ":" + (totalSegundos % 60).ToString("00");
    }

    // "mm:ss" del cronómetro (tiempo jugado, no cuenta atrás)
    public string TiempoTranscurridoFormateado()
    {
        int totalSegundos = Mathf.Max(0, Mathf.FloorToInt(TiempoTranscurrido));
        return (totalSegundos / 60).ToString("00") + ":" + (totalSegundos % 60).ToString("00");
    }
}
