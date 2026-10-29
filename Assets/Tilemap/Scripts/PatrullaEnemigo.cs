using UnityEngine;

public class PatrullaEnemigo : MonoBehaviour
{
    // Variables a configurar desde el editor
    [Header("Configuración")]
    [SerializeField] private float velocidad = 2f;
    [SerializeField] private float recorrido = 3f;   // distancia desde el punto inicial hasta cada extremo
    [SerializeField] private bool ajustarColliderAlSprite = true;

    // Variables de uso interno en el script
    private Rigidbody2D rb;
    private float limiteIzquierdo;
    private float limiteDerecho;
    private int sentido = -1; // -1 = izquierda, 1 = derecha
    private bool colliderAjustado;

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
        float x = transform.position.x;

        limiteIzquierdo = x - Mathf.Abs(recorrido);
        limiteDerecho = x + Mathf.Abs(recorrido);

        sentido = -1; // arranca hacia la izquierda
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

        // Al llegar a un extremo se invierte el sentido
        if (sentido < 0 && x <= limiteIzquierdo) { sentido = 1; }
        else if (sentido > 0 && x >= limiteDerecho) { sentido = -1; }

        float velocidadHorizontal = sentido * Mathf.Abs(velocidad);

        if (rb != null)
        {
            // Sólo se fija la velocidad horizontal para que la gravedad siga actuando en vertical
            rb.linearVelocity = new Vector2(velocidadHorizontal, rb.linearVelocity.y);
        }
        else
        {
            transform.position += Vector3.right * (velocidadHorizontal * Time.fixedDeltaTime);
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
