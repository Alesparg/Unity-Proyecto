using UnityEngine;

/// <summary>
/// Script de ayuda para configurar el sistema de animación del jugador.
/// Añade este componente al GameObject del jugador y usa los métodos del menú contextual.
/// </summary>
public class JugadorAnimationSetup : MonoBehaviour
{
    [Header("Referencias Automáticas")]
    [SerializeField] private JugadorAnimation jugadorAnimation;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private movimiento movimientoScript;
    [SerializeField] private Saltar saltoScript;

    private void Awake()
    {
        // Buscar componentes automáticamente
        if (jugadorAnimation == null)
            jugadorAnimation = GetComponent<JugadorAnimation>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (movimientoScript == null)
            movimientoScript = GetComponent<movimiento>();

        if (saltoScript == null)
            saltoScript = GetComponent<Saltar>();
    }

    [ContextMenu("Setup Completo del Jugador")]
    public void SetupCompleto()
    {
        Debug.Log("🔧 Iniciando setup completo del jugador...");

        // Verificar SpriteRenderer
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
                Debug.Log("✅ SpriteRenderer añadido");
            }
        }

        // Verificar JugadorAnimation
        if (jugadorAnimation == null)
        {
            jugadorAnimation = gameObject.GetComponent<JugadorAnimation>();
            if (jugadorAnimation == null)
            {
                jugadorAnimation = gameObject.AddComponent<JugadorAnimation>();
                Debug.Log("✅ JugadorAnimation añadido");
            }
        }

        // Configurar referencias en JugadorAnimation
        if (jugadorAnimation != null)
        {
            var animationField = typeof(JugadorAnimation).GetField("movimientoScript",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (animationField != null && movimientoScript != null)
            {
                animationField.SetValue(jugadorAnimation, movimientoScript);
            }

            var saltoField = typeof(JugadorAnimation).GetField("saltoScript",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (saltoField != null && saltoScript != null)
            {
                saltoField.SetValue(jugadorAnimation, saltoScript);
            }

            Debug.Log("✅ Referencias configuradas en JugadorAnimation");
        }

        // Cargar sprites automáticamente
        if (jugadorAnimation != null)
        {
            var loadMethod = typeof(JugadorAnimation).GetMethod("LoadSpritesAutomatically",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (loadMethod != null)
            {
                loadMethod.Invoke(jugadorAnimation, null);
                Debug.Log("✅ Sprites cargados automáticamente");
            }
        }

        Debug.Log("✅ Setup completado!");
    }

    [ContextMenu("Verificar Configuración")]
    public void VerificarConfiguracion()
    {
        Debug.Log("=== VERIFICACIÓN DE CONFIGURACIÓN ===");

        if (spriteRenderer != null)
            Debug.Log($"✅ SpriteRenderer: OK");
        else
            Debug.LogError("❌ SpriteRenderer: FALTA");

        if (jugadorAnimation != null)
            Debug.Log($"✅ JugadorAnimation: OK");
        else
            Debug.LogError("❌ JugadorAnimation: FALTA");

        if (movimientoScript != null)
            Debug.Log($"✅ movimiento: OK");
        else
            Debug.LogWarning("⚠️ movimiento: No encontrado (opcional)");

        if (saltoScript != null)
            Debug.Log($"✅ Saltar: OK");
        else
            Debug.LogWarning("⚠️ Saltar: No encontrado (opcional)");

        Debug.Log("================================");
    }

    [ContextMenu("Forzar Carga de Sprites")]
    public void ForzarCargaSprites()
    {
        if (jugadorAnimation != null)
        {
            var loadMethod = typeof(JugadorAnimation).GetMethod("LoadSpritesAutomatically",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (loadMethod != null)
            {
                loadMethod.Invoke(jugadorAnimation, null);
                Debug.Log("✅ Sprites cargados manualmente");
            }
        }
        else
        {
            Debug.LogError("❌ JugadorAnimation no encontrado");
        }
    }

    [ContextMenu("Prueba de Animación - Idle")]
    public void PruebaIdle()
    {
        if (jugadorAnimation != null)
        {
            jugadorAnimation.SetSprite(JugadorAnimationState.Idle);
            Debug.Log("🎨 Animación Idle activada");
        }
    }

    [ContextMenu("Prueba de Animación - Walk")]
    public void PruebaWalk()
    {
        if (jugadorAnimation != null)
        {
            jugadorAnimation.SetSprite(JugadorAnimationState.Walk);
            Debug.Log("🎨 Animación Walk activada");
        }
    }

    [ContextMenu("Prueba de Animación - Jump")]
    public void PruebaJump()
    {
        if (jugadorAnimation != null)
        {
            jugadorAnimation.SetSprite(JugadorAnimationState.Jump);
            Debug.Log("🎨 Animación Jump activada");
        }
    }
}
