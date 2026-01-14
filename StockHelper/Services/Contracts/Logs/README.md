# Logger Implementation Guide

## Descripción General

El sistema de logging ha sido completamente refactorizado para proporcionar:
- ? Thread-safety completo
- ? Rotación automática de archivos de log
- ? Configuración flexible por ambiente
- ? Manejo robusto de errores
- ? Logging de excepciones con stack traces
- ? Múltiples appenders (Console y File)

## Arquitectura

### Componentes Principales

1. **Logger** (Singleton)
   - Clase central del sistema de logging
   - Thread-safe
   - Soporta múltiples appenders

2. **LoggerConfiguration**
   - Configuración centralizada
   - Perfiles predefinidos para Test y Production
   - Inicialización simplificada

3. **Appenders**
   - **ConsoleAppender**: Log a consola con colores por nivel
   - **FileAppender**: Log a archivo con rotación automática

4. **LogLevels**
   - Debug
   - Info
   - Warning
   - Error
   - Fatal

## Uso Básico

### Inicialización (En Program.cs)

```csharp
// Automática según ambiente
InitializeLogger(); // Ya implementado en Program.cs

// O manual
var config = LoggerConfiguration.CreateTestConfiguration();
config.InitializeLogger();
```

### Logging Simple

```csharp
Logger.Current.Debug("Mensaje de debug");
Logger.Current.Info("Información general");
Logger.Current.Warning("Advertencia");
Logger.Current.Error("Error");
Logger.Current.Fatal("Error fatal");
```

### Logging de Excepciones

```csharp
try
{
    // Código que puede fallar
}
catch (Exception ex)
{
    Logger.Current.LogException(LogLevels.Error, "Descripción del contexto", ex);
}
```

### Uso en Exception Handlers

```csharp
// Automático al llamar Handler()
try
{
    // código
}
catch (Exception ex)
{
    new MySystemException(ex.Message, "DAL", ex).Handler();
}
```

## Configuración

### App.config

```xml
<appSettings>
    <add key="logFileDirectory" value="Logs"/>
</appSettings>
```

### Ambientes

**Test/Development**:
- Console logging: Habilitado
- File logging: Habilitado (test-system.log)
- Nivel mínimo: Debug

**Production**:
- Console logging: Deshabilitado
- File logging: Habilitado (system.log)
- Nivel mínimo: Info

## Características Avanzadas

### Rotación de Logs
- Tamaño máximo por archivo: 10 MB
- Archivos de backup: 5 (system.log.1 a system.log.5)
- Rotación automática al exceder el tamaño

### Thread-Safety
- Todos los métodos son thread-safe
- Los appenders se ejecutan de forma segura en entornos multi-hilo

### Gestión de Appenders

```csharp
// Agregar appender
var fileAppender = new FileAppender("custom.log");
Logger.Current.AddAppender(fileAppender);

// Remover appender
Logger.Current.RemoveAppender(fileAppender);

// Limpiar todos
Logger.Current.ClearAppenders();

// Contar appenders
int count = Logger.Current.AppenderCount;
```

## Buenas Prácticas

1. **Usar niveles apropiados**:
   - Debug: Información detallada para desarrollo
   - Info: Eventos importantes del flujo normal
   - Warning: Situaciones inusuales pero no críticas
   - Error: Errores que requieren atención
   - Fatal: Errores críticos que impiden continuar

2. **Contextualizar mensajes**:
   ```csharp
   // ? Malo
   Logger.Current.Error("Error");
   
   // ? Bueno
   Logger.Current.Error($"Error al cargar usuario {userId}: {ex.Message}");
   ```

3. **Usar LogException para excepciones**:
   ```csharp
   // ? Malo
   Logger.Current.Error(ex.Message);
   
   // ? Bueno
   Logger.Current.LogException(LogLevels.Error, "Error en operación X", ex);
   ```

4. **Evitar logging excesivo en bucles**:
   ```csharp
   // ? Malo
   foreach (var item in items)
   {
       Logger.Current.Debug($"Processing {item}"); // Puede generar miles de logs
   }
   
   // ? Bueno
   Logger.Current.Debug($"Processing {items.Count} items");
   ```

## Solución de Problemas

### El log no se escribe

1. Verificar que el directorio de logs existe y tiene permisos
2. Verificar que el Logger está inicializado
3. Verificar el nivel mínimo de logging del appender

### Archivo de log crece mucho

- El sistema rota automáticamente archivos >10MB
- Se mantienen máximo 5 archivos de backup
- Los archivos antiguos se eliminan automáticamente

### Performance issues

- El logging es asíncrono en cuanto a los appenders
- Cada appender maneja sus propios errores sin bloquear otros
- La rotación de archivos es eficiente

## Ejemplos Completos

### Ejemplo 1: Logging en un Servicio

```csharp
public class UserService
{
    public User GetUser(Guid id)
    {
        Logger.Current.Debug($"GetUser called with id: {id}");
        
        try
        {
            var user = _repository.GetById(id);
            
            if (user == null)
            {
                Logger.Current.Warning($"User not found: {id}");
                return null;
            }
            
            Logger.Current.Info($"User retrieved successfully: {user.Name}");
            return user;
        }
        catch (Exception ex)
        {
            Logger.Current.LogException(LogLevels.Error, 
                $"Error retrieving user {id}", ex);
            throw;
        }
    }
}
```

### Ejemplo 2: Logging en UI

```csharp
private void btnLogin_Click(object sender, EventArgs e)
{
    try
    {
        Logger.Current.Info($"Login attempt for user: {txtUsername.Text}");
        
        bool success = _loginService.Authenticate(
            txtUsername.Text, 
            txtPassword.Text);
        
        if (success)
        {
            Logger.Current.Info($"Login successful for user: {txtUsername.Text}");
            // Continuar...
        }
    }
    catch (Exception ex)
    {
        Logger.Current.LogException(LogLevels.Error, 
            "Login error", ex);
        MessageBox.Show("Error al iniciar sesión");
    }
}
```

## Migración desde el Sistema Anterior

El código existente que usa `Logger.Current.Info()`, `.Error()`, etc. sigue funcionando sin cambios.

Cambios recomendados:

```csharp
// Antes
Logger.Current.Error(ex.Message);

// Ahora (recomendado)
Logger.Current.LogException(LogLevels.Error, "Contexto del error", ex);
```

## Próximas Mejoras Sugeridas

1. Implementar logging asíncrono completo (async/await)
2. Agregar appender para base de datos
3. Agregar appender para servicios externos (Sentry, Application Insights)
4. Implementar filtros y formateadores personalizados
5. Agregar métricas y estadísticas de logs
