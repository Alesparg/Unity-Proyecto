using UnityEngine;

public class PatrullaEnemigo : MonoBehaviour
{
    // Variables a configurar desde el editor
    [Header("Configuración")]
    [SerializeField] private float velocidad = 2f;
    [SerializeField] private float recorrido = 3f;   // distancia desde el punto inicial hasta cada extremo
    [SerializeField] private bool ajustarColliderAlSprite = true;

    [Header("Persecución")]
    [Tooltip("Distancia a la que el enemigo detecta al jugador y empieza a ir hacia él")]
    [SerializeField] private float radioDeteccion = 6f;
    [Tooltip("Distancia extra antes de dejar de perseguir, para que no titubee en el borde del radio")]
    [SerializeField] private float margenAbandono = 2f;
    [Tooltip("Velocidad horizontal mientras persigue al jugador")]
    [SerializeField] private float velocidadPersecucion = 3.2f;

    // Variables de uso interno en el script
    private Rigidbody2D rb;
    private float limiteIzquierdo;
    private float limiteDerecho;
    private int sentido = -1; // -1 = izquierda, 1 = derecha
    private bool colliderAjustado;
    private Transform jugador;
    private bool persiguiendo;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            // Sin esto el círculo del collider hace que el enemigo ruede al chocar
            rb.constraints |= RigidbodyConstraints2D.FreezeRotation;
        }
    }

    private void Start()
    {
        // Los extremos se calculan sobre la posición que el enemigo tiene al empezar la partida
        RecentrarPatrulla(transform.position.x);

        sentido = -1; // arranca hacia la izquierda

        GameObject jugadorObj = GameObject.FindGameObjectWithTag("Player");
        if (jugadorObj != null) { jugador = jugadorObj.transform; }
    }

    private void LateUpdate()
    {
        // Se espera al primer LateUpdate para que el Animator ya haya puesto el sprite del
        // primer frame y la medida del collider sea la correcta
        if (!ajustarColliderAlSprite || colliderAjustado) { return; }

        colliderAjustado = true;
        AjustarColliderAlSprite();
    }

    private void FixedUpdate()
    {
        float x = rb != null ? rb.position.x : transform.position.x;

        ActualizarPersecucion(x);

        // Persiguiendo: va derecho hacia el jugador, sin respetar los extremos de la patrulla
        if (persiguiendo)
        {
            float haciaJugador = jugador.position.x - x;
            float horizontal = Mathf.Abs(haciaJugador) < 0.05f ? 0f : Mathf.Sign(haciaJugador) * Mathf.Abs(velocidadPersecucion);

            AplicarVelocidadHorizontal(horizontal);
            return;
        }

        // Al llegar a un extremo se invierte el sentido
        if (sentido < 0 && x <= limiteIzquierdo) { sentido = 1; }
        else if (sentido > 0 && x >= limiteDerecho) { sentido = -1; }

        AplicarVelocidadHorizontal(sentido * Mathf.Abs(velocidad));
    }

    // Sólo persigue mientras el jugador esté dentro del radio; si se aleja, vuelve a patrullar.
    private void ActualizarPersecucion(float x)
    {
        if (jugador == null) { persiguiendo = false; return; }

        float distancia = Vector2.Distance(new Vector2(x, transform.position.y), jugador.position);
        float umbral = persiguiendo
            ? Mathf.Max(0f, radioDeteccion) + Mathf.Max(0f, margenAbandono)
            : Mathf.Max(0f, radioDeteccion);

        bool deberiaPerseguir = distancia <= umbral;

        if (deberiaPerseguir == persiguiendo) { return; }

        persiguiendo = deberiaPerseguir;

        // Al dejar de perseguir retoma el vaivén donde quedó, no en el punto original
        if (!persiguiendo) { RecentrarPatrulla(x); }
    }

    private void RecentrarPatrulla(float x)
    {
        limiteIzquierdo = x - Mathf.Abs(recorrido);
        limiteDerecho = x + Mathf.Abs(recorrido);
    }

    private void AplicarVelocidadHorizontal(float horizontal)
    {
        if (rb != null)
        {
            // Sólo se fija la velocidad horizontal para que la gravedad siga actuando en vertical
            rb.linearVelocity = new Vector2(horizontal, rb.linearVelocity.y);
        }
        else
        {
            transform.position += Vector3.right * (horizontal * Time.fixedDeltaTime);
        }
    }

    // El CircleCollider2D se escala con el eje mayor del transform. Como el enemigo tiene una
    // escala muy grande y no uniforme, el círculo quedaba mucho más alto que el dibujo y el
    // enemigo parecía flotar (y hería desde más abajo de lo que se ve).
    private void AjustarColliderAlSprite()
    {
        CircleCollider2D colision = GetComponent<CircleCollider2D>();
        SpriteRenderer dibujo = GetComponent<SpriteRenderer>();

        if (colision == null || dibujo == null || dibujo.sprite == null) { return; }

        Vector3 escala = transform.lossyScale;
        float ejeMayor = Mathf.Max(Mathf.Abs(escala.x), Mathf.Abs(escala.y));

        if (ejeMayor < 0.0001f) { return; }

        // Radio en unidades locales para que el círculo en el mundo mida lo alto del dibujo
        float radioEnMundo = dibujo.sprite.bounds.size.y * Mathf.Abs(escala.y) * 0.5f;
        colision.radius = radioEnMundo / ejeMayor;
    }
}
