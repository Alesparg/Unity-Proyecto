using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorObjetoLoopWithPool : MonoBehaviour
{
    [SerializeField]
    [Range(0.5f, 5f)]
    private float tiempoEspera;

    [SerializeField]
    [Range(0.5f, 5f)]
    private float tiempoIntervalo;

    [SerializeField]
    [Min(1)]
    private int maximoProyectiles = 2;

    private ObjectPool objectPool;
    private readonly List<GameObject> proyectilesActivos = new List<GameObject>();

    private void Awake()
    {
        objectPool = GetComponent<ObjectPool>();
    }

    void Start()
    {
        InvokeRepeating(nameof(GenerarObjetoLoop), tiempoEspera, tiempoIntervalo);
    }

    void GenerarObjetoLoop()
    {
        GameObject pooledObject = objectPool.GetPooledObject();

        if (pooledObject != null)
        {
            pooledObject.transform.position = transform.position;
            pooledObject.transform.rotation = Quaternion.identity;
            pooledObject.SetActive(true);

            // Añadir un componente para retornar el objeto al pool cuando se desactive
            PooledObject pooledObjComponent = pooledObject.GetComponent<PooledObject>();
            if (pooledObjComponent == null)
            {
                pooledObjComponent = pooledObject.AddComponent<PooledObject>();
            }
            pooledObjComponent.Initialize(objectPool);

            proyectilesActivos.Add(pooledObject);
        }

        LimitarProyectiles();
    }

    /// <summary>
    /// Mantiene vivos como maximo 'maximoProyectiles'. Al salir uno nuevo, el mas antiguo
    /// se desactiva, y PooledObject.OnDisable lo devuelve al pool automaticamente.
    /// </summary>
    private void LimitarProyectiles()
    {
        for (int i = proyectilesActivos.Count - 1; i >= 0; i--)
        {
            GameObject proyectil = proyectilesActivos[i];

            if (proyectil == null || !proyectil.activeSelf)
            {
                proyectilesActivos.RemoveAt(i);
            }
        }

        while (proyectilesActivos.Count > maximoProyectiles)
        {
            GameObject masAntiguo = proyectilesActivos[0];
            proyectilesActivos.RemoveAt(0);

            if (masAntiguo != null)
            {
                masAntiguo.SetActive(false);
            }
        }
    }
}

/// <summary>
/// Componente que automáticamente retorna el objeto al pool cuando se desactiva.
/// </summary>
public class PooledObject : MonoBehaviour
{
    private ObjectPool pool;

    public void Initialize(ObjectPool objectPool)
    {
        pool = objectPool;
    }

    private void OnDisable()
    {
        if (pool != null)
        {
            pool.ReturnObject(gameObject);
        }
    }
}
