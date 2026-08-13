using UnityEngine;

/// <summary>
/// Script de diagnóstico para el sistema de partículas que cura corazones.
/// Añádelo al GameObject que tiene el Particle System y el script Curar.
/// </summary>
public class ParticleHeartDebugger : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private Curar curarScript;

    [Header("Configuración de Debug")]
    [SerializeField] private bool mostrarInfoEnStart = true;
    [SerializeField] private bool detectarColisiones = true;

    private void Start()
    {
        if (mostrarInfoEnStart)
        {
            MostrarInfoDiagnostico();
        }
    }

    private void Update()
    {
        if (detectarColisiones && Input.GetKeyDown(KeyCode.F2))
        {
            SimularColision();
        }
    }

    [ContextMenu("Mostrar Información de Diagnóstico")]
    public void MostrarInfoDiagnostico()
    {
        Debug.Log("=== DIAGNÓSTICO DEL SISTEMA DE PARTÍCULAS CURADORAS ===");

        // Verificar Particle System
        if (particleSystem == null)
            particleSystem = GetComponent<ParticleSystem>();

        if (particleSystem != null)
        {
            Debug.Log($"✅ Particle System: {particleSystem.name}");
            Debug.Log($"   - Está activo: {particleSystem.gameObject.activeSelf}");
            Debug.Log($"   - Está jugando: {particleSystem.isPlaying}");
            Debug.Log($"   - Emisión habilitada: {particleSystem.emission.enabled}");
            Debug.Log($"   - Tiempo de vida: {particleSystem.main.startLifetime.constant}");
        }
        else
        {
            Debug.LogError("❌ No se encontró Particle System");
        }

        // Verificar Script Curar
        if (curarScript == null)
            curarScript = GetComponent<Curar>();

        if (curarScript != null)
        {
            Debug.Log($"✅ Script Curar: encontrado");
        }
        else
        {
            Debug.LogError("❌ No se encontró script Curar");
        }

        // Verificar HeartSystemManager
        if (HeartSystemManager.Instance != null)
        {
            Debug.Log($"✅ HeartSystemManager: encontrado");
            Debug.Log($"   - Corazones actuales: {HeartSystemManager.Instance.GetCurrentHearts()}");
        }
        else
        {
            Debug.LogError("❌ HeartSystemManager.Instance es null");
        }

        // Verificar Collider
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            Debug.Log($"✅ Collider2D: {collider.GetType().Name}");
            Debug.Log($"   - Es trigger: {collider.isTrigger}");
        }
        else
        {
            Debug.LogWarning("⚠️ No se encontró Collider2D (necesario para detección de colisiones)");
        }

        Debug.Log("===================================================");
    }

    [ContextMenu("Simular Colisión con Jugador")]
    public void SimularColision()
    {
        Debug.Log("🎯 Simulando colisión con jugador...");

        if (HeartSystemManager.Instance != null)
        {
            HeartSystemManager.Instance.GainHeart();
            Debug.Log("💚 Corazón añadido (simulación)");
        }
        else
        {
            Debug.LogError("❌ No se puede simular: HeartSystemManager.Instance es null");
        }
    }

    [ContextMenu("Forzar Visibilidad de Corazones")]
    public void ForzarVisibilidadCorazones()
    {
        if (HeartSystemManager.Instance != null)
        {
            HeartSystemManager.Instance.EnsureVisibility();
            Debug.Log("👁️ Visibilidad de corazones forzada");
        }
        else
        {
            Debug.LogError("❌ HeartSystemManager.Instance es null");
        }
    }

    [ContextMenu("Reinicializar Sistema de Corazones")]
    public void ReinicializarSistemaCorazones()
    {
        if (HeartSystemManager.Instance != null)
        {
            HeartSystemManager.Instance.ReinitializeSystem();
            Debug.Log("🔄 Sistema de corazones reinicializado");
        }
        else
        {
            Debug.LogError("❌ HeartSystemManager.Instance es null");
        }
    }

    [ContextMenu("Iniciar Particle System")]
    public void IniciarParticleSystem()
    {
        if (particleSystem == null)
            particleSystem = GetComponent<ParticleSystem>();

        if (particleSystem != null)
        {
            particleSystem.Play();
            Debug.Log("▶️ Particle System iniciado");
        }
    }

    [ContextMenu("Detener Particle System")]
    public void DetenerParticleSystem()
    {
        if (particleSystem == null)
            particleSystem = GetComponent<ParticleSystem>();

        if (particleSystem != null)
        {
            particleSystem.Stop();
            Debug.Log("⏸️ Particle System detenido");
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log($"💥 COLISIÓN DE PARTÍCULA DETECTADA");
        Debug.Log($"   - Objeto colisionado: {other.name}");
        Debug.Log($"   - Tag: {other.tag}");
        Debug.Log($"   - Tiene componente Jugador: {other.GetComponent<Jugador>() != null}");
    }
}
