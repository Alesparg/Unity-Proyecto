using System.Collections;
using UnityEngine;

/// <summary>
/// Sistema de animación del jugador usando sprites.
/// Controla las animaciones de movimiento, salto e idle.
/// </summary>
public class JugadorAnimation : MonoBehaviour
{
    [Header("Sprites del Jugador")]
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite walkSpriteA;
    [SerializeField] private Sprite walkSpriteB;
    [SerializeField] private Sprite jumpSprite;
    [SerializeField] private Sprite frontSprite;

    [Header("Configuración de Animación")]
    [SerializeField] private float walkAnimationSpeed = 0.15f;
    [SerializeField] private bool flipOnDirection = true;

    [Header("Referencias")]
    [SerializeField] private movimiento movimientoScript;
    [SerializeField] private Saltar saltoScript;

    private SpriteRenderer spriteRenderer;
    private bool isWalking;
    private bool isJumping;
    private bool useWalkSpriteA = true;
    private Coroutine walkAnimationCoroutine;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Intentar encontrar los scripts automáticamente si no están asignados
        if (movimientoScript == null)
            movimientoScript = GetComponent<movimiento>();

        if (saltoScript == null)
            saltoScript = GetComponent<Saltar>();

        // Cargar sprites automáticamente si no están asignados
        LoadSpritesAutomatically();
    }

    private void Start()
    {
        // Empezar con el sprite idle
        if (idleSprite != null)
            spriteRenderer.sprite = idleSprite;
    }

    private void Update()
    {
        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        // Detectar si está saltando
        isJumping = !IsGrounded();

        // Detectar si está caminando
        isWalking = IsMoving() && !isJumping;

        // Cambiar sprite según el estado
        if (isJumping)
        {
            SetJumpSprite();
        }
        else if (isWalking)
        {
            if (walkAnimationCoroutine == null)
            {
                walkAnimationCoroutine = StartCoroutine(WalkAnimation());
            }
        }
        else
        {
            SetIdleSprite();
        }

        // Voltear sprite según dirección
        if (flipOnDirection)
        {
            FlipSprite();
        }
    }

    private bool IsMoving()
    {
        if (movimientoScript == null) return false;

        // Verificar si hay input horizontal
        float horizontalInput = Input.GetAxis("Horizontal");
        return Mathf.Abs(horizontalInput) > 0.1f;
    }

    private bool IsGrounded()
    {
        if (saltoScript == null) return true;

        // Acceder a la variable privada usando reflexión
        var field = typeof(Saltar).GetField("puedoSaltar",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        if (field != null)
        {
            bool puedoSaltar = (bool)field.GetValue(saltoScript);
            return puedoSaltar;
        }

        return true;
    }

    private void SetIdleSprite()
    {
        if (walkAnimationCoroutine != null)
        {
            StopCoroutine(walkAnimationCoroutine);
            walkAnimationCoroutine = null;
        }

        if (idleSprite != null && spriteRenderer.sprite != idleSprite)
        {
            spriteRenderer.sprite = idleSprite;
        }
    }

    private void SetJumpSprite()
    {
        if (walkAnimationCoroutine != null)
        {
            StopCoroutine(walkAnimationCoroutine);
            walkAnimationCoroutine = null;
        }

        if (jumpSprite != null && spriteRenderer.sprite != jumpSprite)
        {
            spriteRenderer.sprite = jumpSprite;
        }
    }

    private IEnumerator WalkAnimation()
    {
        while (isWalking)
        {
            // Alternar entre sprites de caminata
            useWalkSpriteA = !useWalkSpriteA;

            if (useWalkSpriteA && walkSpriteA != null)
            {
                spriteRenderer.sprite = walkSpriteA;
            }
            else if (!useWalkSpriteA && walkSpriteB != null)
            {
                spriteRenderer.sprite = walkSpriteB;
            }

            yield return new WaitForSeconds(walkAnimationSpeed);
        }

        walkAnimationCoroutine = null;
    }

    private void FlipSprite()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput > 0.1f)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (horizontalInput < -0.1f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void LoadSpritesAutomatically()
    {
        if (idleSprite == null || walkSpriteA == null || walkSpriteB == null || jumpSprite == null)
        {
            Debug.Log("🔍 Cargando sprites del jugador automáticamente...");

            // Buscar sprites en la carpeta Resources/Prisets/Jugador
            Sprite[] sprites = Resources.LoadAll<Sprite>("Prisets/Jugador");

            foreach (Sprite sprite in sprites)
            {
                string spriteName = sprite.name.ToLower();

                if (spriteName.Contains("idle") && idleSprite == null)
                {
                    idleSprite = sprite;
                    Debug.Log($"✅ Sprite idle cargado: {sprite.name}");
                }
                else if (spriteName.Contains("walk_a") && walkSpriteA == null)
                {
                    walkSpriteA = sprite;
                    Debug.Log($"✅ Sprite walk_a cargado: {sprite.name}");
                }
                else if (spriteName.Contains("walk_b") && walkSpriteB == null)
                {
                    walkSpriteB = sprite;
                    Debug.Log($"✅ Sprite walk_b cargado: {sprite.name}");
                }
                else if (spriteName.Contains("jump") && jumpSprite == null)
                {
                    jumpSprite = sprite;
                    Debug.Log($"✅ Sprite jump cargado: {sprite.name}");
                }
                else if (spriteName.Contains("front") && frontSprite == null)
                {
                    frontSprite = sprite;
                    Debug.Log($"✅ Sprite front cargado: {sprite.name}");
                }
            }

            // Si no se encontraron en Resources, intentar usar el nombre del archivo
            if (idleSprite == null)
            {
                Debug.LogWarning("⚠️ No se encontraron sprites en Resources/Prisets/Jugador");
                Debug.LogWarning("👉 Por favor, arrastra los sprites manualmente en el Inspector");
            }
        }
    }

    /// <summary>
    /// Método público para forzar un sprite específico
    /// </summary>
    public void SetSprite(JugadorAnimationState state)
    {
        switch (state)
        {
            case JugadorAnimationState.Idle:
                SetIdleSprite();
                break;
            case JugadorAnimationState.Walk:
                if (walkAnimationCoroutine == null)
                    walkAnimationCoroutine = StartCoroutine(WalkAnimation());
                break;
            case JugadorAnimationState.Jump:
                SetJumpSprite();
                break;
            case JugadorAnimationState.Front:
                if (frontSprite != null)
                    spriteRenderer.sprite = frontSprite;
                break;
        }
    }
}

public enum JugadorAnimationState
{
    Idle,
    Walk,
    Jump,
    Front
}
