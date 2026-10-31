using System.Collections.Generic;
using UnityEngine;

public class GeneradorObjetoAleatorio : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private GameObject[] objetosPrefabs;

    [SerializeField]
    [Range(0.5f, 5f)]
    private float tiempoEspera;

    [SerializeField]
    [Range(0.5f, 5f)]
    private float tiempoIntervalo;

    [Tooltip("Cantidad máxima de enemigos vivos al mismo tiempo")]
    [SerializeField]
    [Min(1)]
    private int maximoEnemigos = 3;

    [Tooltip("Multiplicador de tamaño que se aplica a cada enemigo generado")]
    [SerializeField]
    [Min(0.1f)]
    private float multiplicadorEscala = 1.5f;

    // Instancias generadas que todavía siguen vivas en la escena
    private readonly List<GameObject> enemigosActivos = new List<GameObject>();

    void Start()
    {
        //InvokeRepeating(nameof(GenerarObjetoAleatorio), tiempoEspera, tiempoIntervalo);
    }

    void GenerarObjetoAleatorio()
    {
        QuitarEnemigosMuertos();

        // Con el cupo lleno no se genera nada hasta que el jugador elimine uno
        if (enemigosActivos.Count >= maximoEnemigos) { return; }

        int indexAleatorio = Random.Range(0, objetosPrefabs.Length);
        GameObject prefabAleatorio = objetosPrefabs[indexAleatorio];

        GameObject enemigo = Instantiate(prefabAleatorio, transform.position, Quaternion.identity);

        // El prefab ya trae su propia escala, así que el tamaño extra se multiplica sobre ella
        enemigo.transform.localScale = Vector3.Scale(enemigo.transform.localScale, Vector3.one * multiplicadorEscala);

        enemigosActivos.Add(enemigo);
    }

    // Un enemigo eliminado con Destroy queda como referencia nula, así que se descarta de la lista
    private void QuitarEnemigosMuertos()
    {
        for (int i = enemigosActivos.Count - 1; i >= 0; i--)
        {
            if (enemigosActivos[i] == null) { enemigosActivos.RemoveAt(i); }
        }
    }

    private void OnBecameInvisible()
    {
        Debug.Log("El SpriteRenderer deja de ser visible por las cámaras en la escena");
        CancelInvoke(nameof(GenerarObjetoAleatorio));
    }

    private void OnBecameVisible()
    {
        Debug.Log("El SpriteRenderer es visible por las cámaras en la escena");
        InvokeRepeating(nameof(GenerarObjetoAleatorio), tiempoEspera, tiempoIntervalo);
    }

}
