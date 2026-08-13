# Sistema de Animación del Jugador

Este sistema permite animar al jugador usando los sprites de la carpeta `Prisets/Jugador`.

## Archivos creados

1. **JugadorAnimation.cs** - Sistema principal de animación
2. **JugadorAnimationSetup.cs** - Script de ayuda para configuración
3. **Resources/Prisets/Jugador/** - Sprites copiados para carga automática

## Sprites disponibles

- `character_green_idle.png` - Estado quieto (idle)
- `character_green_walk_a.png` - Caminata frame A
- `character_green_walk_b.png` - Caminata frame B
- `character_green_jump.png` - Salto
- `character_green_front.png` - Vista frontal

## Cómo configurar

### Opción 1: Automática (Recomendada)

1. Añade el componente `JugadorAnimation` al GameObject del jugador
2. Añade el componente `JugadorAnimationSetup` al mismo GameObject
3. En el Inspector, haz clic derecho en `JugadorAnimationSetup` y selecciona "Setup Completo del Jugador"
4. El sistema configurará todo automáticamente

### Opción 2: Manual

1. Asegúrate de que el jugador tenga un `SpriteRenderer`
2. Añade el componente `JugadorAnimation`
3. En el Inspector del componente `JugadorAnimation`:
   - Arrastra los scripts `movimiento` y `Saltar` si no se detectan automáticamente
   - Los sprites se cargarán automáticamente de `Resources/Prisets/Jugador/`
   - Ajusta `Walk Animation Speed` para cambiar la velocidad de la animación de caminata
   - Activa `Flip On Direction` para voltear el sprite según la dirección

## Estados de animación

El sistema detecta automáticamente:

- **Idle**: Cuando el jugador no se mueve
- **Walk**: Cuando el jugador se mueve horizontalmente
- **Jump**: Cuando el jugador está en el aire
- **Front**: Vista frontal (uso manual)

## Cómo usar en código

```csharp
// Obtener referencia
JugadorAnimation animacion = GetComponent<JugadorAnimation>();

// Forzar un estado específico
animacion.SetSprite(JugadorAnimationState.Idle);
animacion.SetSprite(JugadorAnimationState.Walk);
animacion.SetSprite(JugadorAnimationState.Jump);
animacion.SetSprite(JugadorAnimationState.Front);
```

## Scripts necesarios

El sistema de animación funciona mejor con estos scripts existentes:

- **movimiento.cs** - Para detectar movimiento horizontal
- **Saltar.cs** - Para detectar cuando el jugador está en el aire

## Solución de problemas

### Los sprites no cambian

1. Verifica que el `SpriteRenderer` esté presente
2. Usa `JugadorAnimationSetup` → "Verificar Configuración"
3. Usa `JugadorAnimationSetup` → "Forzar Carga de Sprites"

### Las animaciones no son fluidas

1. Ajusta `Walk Animation Speed` en el Inspector
2. Valores más bajos = animación más rápida
3. Valores más altos = animación más lenta

### El sprite no se voltea

1. Activa `Flip On Direction` en el Inspector
2. Verifica que el script `movimiento` esté funcionando correctamente

### Los sprites no se cargan automáticamente

1. Asegúrate de que los sprites estén en `Resources/Prisets/Jugador/`
2. Verifica que los nombres de los sprites contengan: "idle", "walk_a", "walk_b", "jump"
3. O arrastra los sprites manualmente en el Inspector

## Personalización

### Cambiar velocidad de animación

```csharp
JugadorAnimation anim = GetComponent<JugadorAnimation>();
// Usar reflexión para cambiar la velocidad
var speedField = typeof(JugadorAnimation).GetField("walkAnimationSpeed",
    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
if (speedField != null)
{
    speedField.SetValue(anim, 0.1f); // Nueva velocidad
}
```

### Añadir nuevos sprites

1. Coloca los nuevos sprites en `Resources/Prisets/Jugador/`
2. Nómbralos con las palabras clave correspondientes
3. El sistema los detectará automáticamente

## Características

- ✅ Detección automática de movimiento y salto
- ✅ Carga automática de sprites desde Resources
- ✅ Volteo automático del sprite según dirección
- ✅ Animación de caminata con 2 frames
- ✅ Transiciones suaves entre estados
- ✅ Compatible con scripts existentes (movimiento, Saltar)
- ✅ Debug y herramientas de configuración
