using UnityEngine;

public class PisotonEnemigo : MonoBehaviour
{
    // Variables a configurar desde el editor
    [Header("Configuración")]
    [SerializeField] private float fuerzaRebote = 7f;

    // Variables de uso interno en el script
    private Collider2D miCollider2D;

    private void OnEnable()
    {
        miCollider2D = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) { return; }
        if (!EsPisoton(collision, miCollider2D)) { return; }

        // Impulso al jugador para que rebote tras el pisotón
        Rigidbody2D jugador = collision.rigidbody;

        if (jugador != null)
        {
            Vector2 velocidad = jugador.linearVelocity;
            jugador.linearVelocity = new Vector2(velocidad.x, fuerzaRebote);
        }

        // Cada enemigo aplastado cuenta como una muerte para el HUD y el cartel de victoria
        if (GameManager.Existe) { GameManager.Instance.RegistrarEnemigoEliminado(); }

        Destroy(gameObject);
    }

    // Devuelve true cuando el jugador cae encima del enemigo desde arriba.
    // Es una prueba sólo geométrica para que Herir pueda consultarla sin depender del orden de ejecución.
    public static bool EsPisoton(Collision2D collision, Collider2D enemigo)
    {
        if (collision == null || enemigo == null) { return false; }
        if (!collision.gameObject.CompareTag("Player")) { return false; }

        // Los pies del jugador deben estar por encima del centro del enemigo
        if (collision.collider.bounds.min.y < enemigo.bounds.center.y) { return false; }

        // Y debe venir cayendo, no subiendo
        Rigidbody2D jugador = collision.rigidbody;
        return jugador == null || jugador.linearVelocity.y <= 0.5f;
    }
}
