using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object Pool genérico y optimizado para Unity.
/// Soporta expansión automática, pre-warming y callbacks.
/// </summary>
public class ObjectPool : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private int initialPoolSize = 5;
    [SerializeField] private int maxPoolSize = 20;
    [SerializeField] private bool autoExpand = true;
    [SerializeField] private bool collectionCheck = true;

    [Header("Debug")]
    [SerializeField] private bool showDebugInfo = false;

    private Queue<GameObject> pooledObjects;
    private Transform poolContainer;

    /// <summary>
    /// Obtiene un objeto del pool. Si no hay disponibles y autoExpand está activado, crea uno nuevo.
    /// </summary>
    public GameObject GetPooledObject()
    {
        GameObject obj = null;

        // Buscar objeto inactivo en el pool
        while (pooledObjects.Count > 0)
        {
            obj = pooledObjects.Dequeue();

            if (obj != null && !obj.activeInHierarchy)
            {
                if (showDebugInfo)
                    Debug.Log($"[ObjectPool] Reusing object from pool. Active objects: {CountActiveObjects()}");

                return obj;
            }
            else if (obj == null && collectionCheck)
            {
                Debug.LogWarning("[ObjectPool] Found null object in pool (object was destroyed externally).");
            }
        }

        // Si no hay objetos disponibles y el autoExpand está activado
        if (autoExpand && (maxPoolSize <= 0 || CountActiveObjects() < maxPoolSize))
        {
            obj = CreateNewObject();
            if (showDebugInfo)
                Debug.Log($"[ObjectPool] Created new object. Total objects: {CountAllObjects()}");
            return obj;
        }

        if (showDebugInfo)
            Debug.LogWarning("[ObjectPool] Pool exhausted and autoExpand is disabled or maxPoolSize reached.");

        return null;
    }

    /// <summary>
    /// Retorna un objeto al pool para su reutilización.
    /// </summary>
    public void ReturnObject(GameObject obj)
    {
        if (obj == null)
        {
            Debug.LogWarning("[ObjectPool] Attempted to return null object to pool.");
            return;
        }

        obj.SetActive(false);
        obj.transform.SetParent(poolContainer, false);

        if (showDebugInfo)
            Debug.Log($"[ObjectPool] Object returned to pool. Available: {pooledObjects.Count}");

        pooledObjects.Enqueue(obj);
    }

    /// <summary>
    /// Crea y pre-calienta el pool con objetos iniciales.
    /// </summary>
    private void InitializePool()
    {
        pooledObjects = new Queue<GameObject>();
        poolContainer = new GameObject($"Pool_{objectPrefab.name}").transform;
        poolContainer.SetParent(transform, false);

        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject obj = CreateNewObject();
            obj.SetActive(false);
            pooledObjects.Enqueue(obj);
        }

        if (showDebugInfo)
            Debug.Log($"[ObjectPool] Pool initialized with {initialPoolSize} objects.");
    }

    /// <summary>
    /// Crea un nuevo objeto del prefab.
    /// </summary>
    private GameObject CreateNewObject()
    {
        GameObject obj = Instantiate(objectPrefab, poolContainer);
        obj.SetActive(false);
        return obj;
    }

    /// <summary>
    /// Cuenta cuántos objetos están activos en el pool.
    /// </summary>
    private int CountActiveObjects()
    {
        int count = 0;
        foreach (Transform child in poolContainer)
        {
            if (child.gameObject.activeSelf)
                count++;
        }
        return count;
    }

    /// <summary>
    /// Cuenta el total de objetos en el pool (activos e inactivos).
    /// </summary>
    private int CountAllObjects()
    {
        return poolContainer.childCount;
    }

    /// <summary>
    /// Limpia el pool destruyendo todos los objetos.
    /// </summary>
    public void ClearPool()
    {
        if (poolContainer != null)
        {
            Destroy(poolContainer.gameObject);
        }

        pooledObjects?.Clear();
        InitializePool();

        if (showDebugInfo)
            Debug.Log("[ObjectPool] Pool cleared and reinitialized.");
    }

    private void Awake()
    {
        if (objectPrefab == null)
        {
            Debug.LogError("[ObjectPool] No prefab assigned to ObjectPool!");
            return;
        }

        InitializePool();
    }

    private void OnDestroy()
    {
        if (poolContainer != null)
        {
            Destroy(poolContainer.gameObject);
        }
    }
}
