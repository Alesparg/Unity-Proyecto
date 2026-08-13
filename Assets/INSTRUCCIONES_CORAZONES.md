# 🖤 INSTRUCCIONES PARA EL SISTEMA DE CORAZONES

## ❗ PROBLEMA SOLUCIONADO
El problema era que no había ningún objeto en la escena con el sistema de corazones configurado. Los corazones se perdían en la consola pero no se mostraban visualmente.

## ✅ SOLUCIÓN IMPLEMENTADA

### 1. Nuevo Script Principal: `HeartSystemManager.cs`
- **Funcionalidad completa**: Crea automáticamente el Canvas, contenedor y corazones
- **Configuración automática**: Busca el sprite del corazón automáticamente
- **Fallback**: Si no encuentra sprite, usa corazones rojos básicos
- **Debug mejorado**: Información detallada en consola

### 2. Script de Configuración: `HeartAutoSetup.cs`
- **Configuración automática**: Se ejecuta al inicio de la escena
- **Búsqueda inteligente**: Encuentra el sprite del corazón por nombre
- **Sin configuración manual**: Todo se hace automáticamente

### 3. Script de Prueba: `HeartTest.cs`
- **Controles de teclado**: L (perder), G (ganar), R (reset), D (debug)
- **Verificación**: Comprueba que el sistema funciona correctamente

### 4. Script Actualizado: `Vidas.cs`
- **Compatible**: Ahora usa `HeartSystemManager` en lugar de `WorkingHeartSystem`

## 🚀 CÓMO USAR

### Opción 1: Configuración Automática (RECOMENDADA)
1. **Arrastra** el script `HeartAutoSetup` a cualquier GameObject en tu escena
2. **Ejecuta** la escena
3. **¡Listo!** El sistema se configurará automáticamente

### Opción 2: Configuración Manual
1. **Crea** un GameObject vacío en la escena
2. **Añade** el script `HeartSystemManager`
3. **Arrastra** tu imagen `corazon-removebg-preview.png` al campo "Heart Sprite"
4. **Ejecuta** la escena

### Opción 3: Configuración Manual Avanzada
1. **Crea** un GameObject vacío en la escena
2. **Añade** el script `HeartSystemManager`
3. **Configura** todos los parámetros según tus necesidades
4. **¡Listo!**

## 🎮 CONTROLES DE PRUEBA
- **L**: Perder un corazón
- **G**: Ganar un corazón  
- **R**: Resetear todos los corazones
- **D**: Mostrar información de debug
- **F1**: Información detallada del sistema

## 🔧 CONFIGURACIÓN AVANZADA

### En el Inspector de `HeartSystemManager`:
- **Heart Sprite**: Imagen del corazón (opcional, se busca automáticamente)
- **Max Hearts**: Número máximo de corazones (por defecto: 5)
- **Heart Size**: Tamaño de cada corazón (por defecto: 60x60)
- **Heart Position**: Posición en pantalla (por defecto: esquina superior izquierda)
- **Heart Spacing**: Espacio entre corazones (por defecto: 10)
- **Show Debug Info**: Mostrar información de debug (recomendado: activado)

## 🐛 SOLUCIÓN DE PROBLEMAS

### Si los corazones no se muestran:
1. **Verifica** que hay un objeto con `HeartSystemManager` en la escena
2. **Revisa** la consola para mensajes de error
3. **Presiona F1** para ver información de debug
4. **Usa** el script `HeartTest` para probar manualmente

### Si el sprite no se carga:
1. **Verifica** que tu imagen está en la carpeta `Assets`
2. **Asegúrate** de que el nombre contiene "corazon", "heart" o "vida"
3. **Arrastra** manualmente el sprite al campo "Heart Sprite"

### Si los corazones no se pierden:
1. **Verifica** que el script `Vidas.cs` está en el jugador
2. **Revisa** que se llama a `ModificarVida()` con valores negativos
3. **Usa** los controles de prueba (tecla L) para verificar

## 📝 NOTAS IMPORTANTES

- **El sistema es compatible** con tu código existente
- **No necesitas** cambiar nada en tus scripts de daño
- **Los corazones se crean automáticamente** al inicio
- **El Canvas se crea automáticamente** si no existe
- **El sistema es robusto** y maneja errores automáticamente

## 🎯 RESULTADO FINAL
- ✅ Los corazones se muestran correctamente en pantalla
- ✅ Se pierden visualmente cuando recibes daño
- ✅ Se pueden ganar corazones
- ✅ Sistema completamente funcional
- ✅ Configuración automática sin complicaciones

¡Tu sistema de corazones ahora funciona perfectamente! 🖤
