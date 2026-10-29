using UnityEngine;

public class IslaFlotante : MonoBehaviour
{
    public enum Eje
    {
        Vertical,
        Horizontal
    }

    [SerializeField] Eje eje = Eje.Vertical;
    [SerializeField] float recorrido = 2f;   // distancia total del vaiven, en unidades
    [SerializeField] float duracion = 3f;    // segundos que tarda un ciclo completo
    [SerializeField] float desfase = 0f;     // adelanta o atrasa el vaiven para desincronizar islas

    Vector3 posicionInicial;
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        posicionInicial = transform.position;
    }

    void FixedUpdate()
    {
        float ciclo = Mathf.Max(0.01f, duracion);
        float onda = Mathf.Sin(((Time.time + desfase) * Mathf.PI * 2f) / ciclo);
        Vector3 direccion = eje == Eje.Vertical ? Vector3.up : Vector3.right;
        Vector3 destino = posicionInicial + direccion * (onda * recorrido * 0.5f);

        if (rb != null)
        {
            rb.MovePosition(destino);
        }
        else
        {
            transform.position = destino;
        }
    }
}
