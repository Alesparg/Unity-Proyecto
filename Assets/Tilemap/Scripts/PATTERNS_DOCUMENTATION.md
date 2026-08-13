# Patrones de Diseño - Singleton y Object Pooling

Este proyecto contiene implementaciones robustas y optimizadas de los patrones Singleton y Object Pooling para Unity.

## Singleton

### Clase: `Singleton<T>`

Implementación genérica del patrón Singleton que es thread-safe y persistente entre escenas.

### Características
- **Thread-safe**: Usa locks para evitar condiciones de carrera
- **Persistente**: Los Singletons usan `DontDestroyOnLoad` por defecto
- **Auto-creación**: Crea automáticamente la instancia si no existe
- **Protección contra duplicados**: Destruye instancias duplicadas automáticamente
- **Safe on quit**: Previene errores al cerrar la aplicación

### Cómo usarlo

```csharp
// Tu clase debe heredar de Singleton<T>
public class GameManager : Singleton<GameManager>
{
    protected override void Awake()
    {
        base.Awake(); // Importante: llamar a base.Awake()

        if (Instance == this)
        {
            // Tu código de inicialización aquí
            Debug.Log("GameManager inicializado");
        }
    }

    public void DoSomething()
    {
        Debug.Log("Haciendo algo");
    }
}

// Acceder a la instancia desde cualquier lugar
GameManager.Instance.DoSomething();

// Verificar si existe la instancia
if (GameManager.HasInstance)
{
    GameManager.Instance.DoSomething();
}

// Destruir la instancia manualmente
GameManager.DestroyInstance();
```

### Ejemplo en el proyecto

**HeartSystemManager** ahora usa el patrón Singleton:

```csharp
public class HeartSystemManager : Singleton<HeartSystemManager>
{
    protected override void Awake()
    {
        base.Awake();

        if (Instance == this)
        {
            SetupHeartSystem();
        }
    }

    public void LoseHeart()
    {
        // Lógica para perder corazón
    }
}

// Usar desde cualquier lugar
HeartSystemManager.Instance.LoseHeart();
```

---

## Object Pooling

### Clase: `ObjectPool`

Implementación optimizada de Object Pooling con expansión automática y gestión de memoria.

### Características
- **Expansión automática**: Crea nuevos objetos cuando el pool se agota
- **Límite máximo**: Configurable para evitar uso excesivo de memoria
- **Return automático**: Los objetos se retornan al pool automáticamente cuando se desactivan
- **Debug info**: Logs opcionales para debugging
- **Contenedor organizado**: Los objetos del pool se organizan en un contenedor
- **Collection check**: Detecta objetos destruidos externamente

### Cómo usarlo

#### 1. Configuración en el Inspector

```
ObjectPool Component:
- Object Prefab: El prefab a poolinear
- Initial Pool Size: Tamaño inicial del pool (default: 5)
- Max Pool Size: Máximo de objetos (0 = sin límite)
- Auto Expand: Si el pool debe expandirse automáticamente
- Collection Check: Detectar objetos destruidos externamente
- Show Debug Info: Mostrar logs de debugging
```

#### 2. Obtener objetos del pool

```csharp
public class Spawner : MonoBehaviour
{
    private ObjectPool objectPool;

    private void Awake()
    {
        objectPool = GetComponent<ObjectPool>();
    }

    void SpawnObject()
    {
        GameObject obj = objectPool.GetPooledObject();

        if (obj != null)
        {
            obj.transform.position = transform.position;
            obj.transform.rotation = Quaternion.identity;
            obj.SetActive(true);

            // Opcional: Añadir componente para return automático
            PooledObject pooled = obj.GetComponent<PooledObject>();
            if (pooled == null)
            {
                pooled = obj.AddComponent<PooledObject>();
            }
            pooled.Initialize(objectPool);
        }
    }
}
```

#### 3. Retornar objetos al pool

**Opción A: Automático (recomendado)**

El componente `PooledObject` retorna automáticamente el objeto cuando se desactiva:

```csharp
// El objeto se desactiva (por ejemplo, al colisionar)
private void OnCollisionEnter(Collision collision)
{
    gameObject.SetActive(false); // El objeto se retorna automáticamente
}
```

**Opción B: Manual**

```csharp
objectPool.ReturnObject(gameObject);
```

### Ejemplo en el proyecto

**GeneradorObjetoLoopWithPool** muestra el uso completo:

```csharp
public class GeneradorObjetoLoopWithPool : MonoBehaviour
{
    private ObjectPool objectPool;

    private void Awake()
    {
        objectPool = GetComponent<ObjectPool>();
    }

    void GenerarObjetoLoop()
    {
        GameObject pooledObject = objectPool.GetPooledObject();

        if (pooledObject != null)
        {
            pooledObject.transform.position = transform.position;
            pooledObject.transform.rotation = Quaternion.identity;
            pooledObject.SetActive(true);

            // Configurar return automático
            PooledObject pooledObjComponent = pooledObject.GetComponent<PooledObject>();
            if (pooledObjComponent == null)
            {
                pooledObjComponent = pooledObject.AddComponent<PooledObject>();
            }
            pooledObjComponent.Initialize(objectPool);
        }
    }
}
```

---

## Mejoras implementadas

### Singleton (anterior vs nuevo)

**Anterior:**
- Solo verificaba duplicados en Start
- No era thread-safe
- No persistía entre escenas
- No proporcionaba acceso global estándar

**Nuevo:**
- Thread-safe con locks
- Persistente entre escenas (DontDestroyOnLoad)
- Acceso global via `Instance`
- Protección en OnApplicationQuit
- Verificación de existencia con `HasInstance`

### Object Pooling (anterior vs nuevo)

**Anterior:**
- Retornaba null cuando se agotaba
- No tenía expansión automática
- Usaba List (menos eficiente)
- No tenía método ReturnObject
- Sin límite máximo
- Sin debugging

**Nuevo:**
- Expansión automática configurable
- Límite máximo configurable
- Usa Queue (más eficiente)
- Método ReturnObject para reutilización
- Return automático con PooledObject
- Debug info opcional
- Contenedor organizado
- Collection check para objetos destruidos externamente

---

## Buenas prácticas

### Singleton
- Usar solo para managers y sistemas globales
- No almacenar estado de escena específica
- Llamar siempre a `base.Awake()` en override
- Usar `HasInstance` antes de acceder si no estás seguro

### Object Pooling
- Usar para objetos que se crean/destruyen frecuentemente
- Configurar tamaño inicial según uso típico
- Usar return automático cuando sea posible
- Desactivar objetos en lugar de destruirlos
- Limpiar el pool al cambiar de escena si es necesario

---

## Migración desde código antiguo

### Singleton
1. Hacer que tu clase herede de `Singleton<TuClase>`
2. Reemplazar el código de duplicación en Start con `base.Awake()`
3. Reemplazar `FindFirstObjectByType<TuClase>()` con `TuClase.Instance`

### Object Pooling
1. Actualizar referencias al nuevo componente ObjectPool
2. Reemplazar `SetActive(false)` con `objectPool.ReturnObject(obj)` si necesitas control manual
3. Añadir componente `PooledObject` para return automático
4. Configurar parámetros en el Inspector según tus necesidades
