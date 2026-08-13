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

    private ObjectPool objectPool;

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
