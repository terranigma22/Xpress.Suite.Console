# Xpress.Suite.ConsoleTools

Librería profesional para .NET (C#) diseñada para mejorar y enriquecer la experiencia de desarrollo en aplicaciones de consola. Proporciona una API completa y fácil de usar para crear interfaces de consola más amigables, informativas y visualmente atractivas.

## Propósito

Resolver las limitaciones de la consola tradicional de .NET, ofreciendo funcionalidades avanzadas que normalmente requieren mucho código boilerplate o librerías externas complejas.

## Características Principales

### 1. Sistema de Logging Avanzado
- Múltiples niveles de log: `Trace`, `Debug`, `Info`, `Warn`, `Error`, `Fatal`, `Success`, `Fail`
- Timestamps automáticos
- Colores personalizables por nivel
- Formato consistente y profesional
- Iconos visuales para rápida identificación

### 2. Tablas Formateadas
- Visualización de datos en formato tabla
- Alineación automática de columnas
- Bordes personalizables (`TableOptions`)
- Soporte para cualquier tipo `IEnumerable<T>`

### 3. Medición de Tiempo (Timers)
- Cronómetros para medir tiempos de ejecución
- Temporizadores nombrados múltiples
- Medición síncrona (`Time`) y asíncrona (`TimeAsync`)
- Reporte automático de resultados
- Cronómetro global con `Elapsed()` y `ResetElapsed()`

### 4. Contadores
- Contadores incrementales/decrementales
- Contadores nombrados
- Seguimiento de estadísticas en tiempo real

### 5. Agrupación Visual (Groups)
- Organización jerárquica de logs
- Indentación automática
- Mejora la legibilidad de logs complejos

### 6. Elementos Visuales Avanzados
- **Barras de progreso**: Personalizables en color y tamaño
- **Spinners de carga**: Indicadores animados para operaciones largas
- **Separadores y líneas**: Mejoran la organización visual
- **Header/Footer**: Títulos y pies con diseño de cuadro

### 7. Interacción con Usuario
- Input con validación (`Ask<T>`)
- Confirmaciones (`Confirm`)
- Password masking (`AskPassword`)
- Preguntas tipadas

### 8. Personalización
- Esquemas de color configurables (`ConsoleColorScheme`)
- Temas predefinidos (`Default`, `Dark`, `HighContrast`)
- Configuración global o por instancia
- Nivel mínimo de log configurable

## Arquitectura

### Patrón de Diseño
- **Singleton**: Acceso global único a la instancia principal
- **Fluent Interface**: Métodos encadenables para configuración
- **Thread-Safe**: Todas las operaciones son seguras para hilos

### Estructura

```
Xpress.Suite.ConsoleTools/
├── ConsoleManager.cs           # Clase principal (Singleton)
├── Enums/
│   ├── LogLevel.cs            # Niveles de logging
│   └── ConsoleTheme.cs        # Temas predefinidos
├── Models/
│   ├── ConsoleColorScheme.cs  # Esquemas de color
│   └── TableOptions.cs        # Opciones de tablas
└── Extensions/
    └── (disponible para extensiones futuras)
```

## Tecnologías

- .NET 10.0
- C# 12.0
- Sin dependencias externas

## Beneficios

### Para Desarrolladores
- **Ahorro de tiempo**: Funcionalidades listas para usar
- **Código más limpio**: Menos boilerplate
- **Debug más fácil**: Logs claros y estructurados
- **Profesional**: Aplicaciones con aspecto profesional

### Para Usuarios Finales
- **Experiencia mejorada**: Interfaz clara y amigable
- **Feedback visual**: Barras de progreso, spinners, colores
- **Información clara**: Logs organizados y legibles

## Casos de Uso

- **Aplicaciones de consola empresariales**: Logging estructurado y profesional
- **Herramientas CLI**: Interfaz amigable para usuarios
- **Scripts de automatización**: Feedback visual claro
- **Procesamiento por lotes**: Monitoreo de progreso
- **Aplicaciones de depuración**: Logs detallados y coloreados
- **CI/CD pipelines**: Salida clara y organizada

## Ejemplo Rápido

```csharp
using Xpress.Suite.ConsoleTools;
using Xpress.Suite.ConsoleTools.Enums;

var console = ConsoleManager.Instance;

// Configuración
console.SetTheme(ConsoleTheme.Dark);
console.SetMinLogLevel(LogLevel.Debug);

// Header
console.Header("Mi Aplicación");

// Logging
console.Info("Iniciando proceso...");
console.Debug("Cargando configuración");
console.Warn("Archivo no encontrado, usando defaults");

// Timer
console.Time(() =>
{
    Thread.Sleep(1000);
}, "proceso");

// Barra de progreso
for (int i = 0; i <= 100; i += 10)
{
    console.ProgressBar(i, 100, 30, ConsoleColor.Cyan);
    Thread.Sleep(100);
}

// Tabla
var datos = new[]
{
    new { Id = 1, Nombre = "Item A", Estado = "OK" },
    new { Id = 2, Nombre = "Item B", Estado = "OK" },
};
console.Table(datos);

// Contador
console.Count("registros");
console.Success("¡Proceso completado!");
console.Elapsed("Tiempo total");
```

## Próximas Mejoras Potenciales

- Soporte para logging a archivos
- Exportación de logs a JSON/CSV
- Gráficos ASCII en consola
- Menús interactivos
- Autocompletado para inputs
- Temas personalizables adicionales
- Integración con Serilog/NLog

## Conclusión

ConsoleTools es una librería esencial para cualquier desarrollador .NET que trabaje con aplicaciones de consola, transformando la experiencia básica de línea de comandos en una interfaz rica, profesional y fácil de usar.
