# 🍄 MarioBrosNES

Proyecto de videojuego 2D desarrollado en **Unity** inspirado en Super Mario Bros. (NES). Incluye mecánicas clásicas de plataformas, sistema de power-ups, enemigos con IA, bloques interactivos, checkpoints, sistema de combos, internacionalización (i18n) y gestión completa de estadísticas de partida.

---

## 📋 Tabla de Contenidos

1. [Introducción](#introducción)
2. [Objetivos Cumplidos](#objetivos-cumplidos)
3. [Arquitectura del Sistema](#arquitectura-del-sistema)
4. [Descripción Técnica de Componentes](#descripción-técnica-de-componentes)
5. [Flujo de Juego](#flujo-de-juego)
6. [Controles](#controles)
7. [Decisiones de Diseño](#decisiones-de-diseño)
8. [Guía para Desarrolladores](#guía-para-desarrolladores)
9. [Requisitos Técnicos](#requisitos-técnicos)

---

## 1. Introducción

**MarioBrosNES** recrea las mecánicas del clásico Super Mario Bros. en Unity con un enfoque modular y extensible. El jugador controla a Mario a través de niveles con plataformas, enemigos, bloques y power-ups. A medida que progresa, puede conseguir nuevas habilidades, acumular puntos mediante combos y explorar sub-niveles mediante tuberías.

El proyecto está estructurado para facilitar la incorporación de nuevos niveles, enemigos y power-ups sin necesidad de modificar los sistemas existentes.

---

## 2. Objetivos Cumplidos

### 2.1 Requisitos Funcionales

**Control del Jugador**
- [x] Movimiento horizontal con aceleración y desaceleración configurables
- [x] Carrera activada manteniendo la tecla Run
- [x] Salto con recorte (soltar tecla reduce la altura del salto)
- [x] Caída mejorada (`fallMultiplier`) para física más responsiva
- [x] Disparo de bolas de fuego en estado Fire (máx. 2 simultáneas)
- [x] Pausa del juego desde el controlador

**Enemigos**
- [x] Movimiento horizontal con detección de paredes
- [x] Activación/desactivación por proximidad al jugador
- [x] Goomba: muerte por pisoteo con animación de aplastado
- [x] Koopa: sistema de caparazón con múltiples estados
- [x] Daño al jugador por contacto lateral
- [x] Compatibilidad con estrella (Star) para muerte inmediata

**Power-Ups**
- [x] Mushroom: Small → Super
- [x] Fire Flower: → Estado Fire
- [x] Star: invencibilidad temporal (10 segundos)
- [x] 1-Up: vida extra
- [x] Emergencia animada desde bloques

**Bloques**
- [x] Bloque `?` de un solo uso con cambio de sprite
- [x] Bloque rompible (solo si el jugador es Super o Fire)
- [x] Bloques con moneda, power-up, estrella o vida extra
- [x] Bloque multi-moneda con duración temporal
- [x] Animación de rebote al ser golpeado

**Sistema de Juego**
- [x] Puntuación, monedas, vidas y tiempo en el HUD
- [x] Sistema de checkpoints por nivel
- [x] Transiciones entre escenas con pantalla de carga
- [x] Pantalla de Game Over con reinicio completo
- [x] Recolección de 100 monedas → vida extra automática

### 2.2 Requisitos No Funcionales

| Requisito | Estado | Detalle |
|---|---|---|
| Componentes persistente entre escenas | ✅ | `GameManager`, `HUDManager`,`PlayerManager` |
| Sistema de combos | ✅ | Stomp Combo y Shell Combo con tablas de puntos |
| Internacionalización | ✅ | Sistema i18n con archivos JSON por idioma |
| Activación de enemigos por proximidad | ✅ | `ActivationEnemies` con triggers |
| Física de bola de fuego | ✅ | Rebote en suelo, destrucción en pared |
| Cámara sin retroceso | ✅ | `CameraFollow` con límite horizontal |

---

## 3. Arquitectura del Sistema

### 3.1 Estructura de Escenas

El proyecto consta de 3 escenas principales:

| Índice | Nombre | Descripción |
|---|---|---|
| 0 | `Inicio` | Menú principal y selección de dificultad |
| 1 | `1-1` | Nivel principal del mundo 1 |
| 2 | `Secret1-1` | Sub-nivel secreto accesible mediante `PipeEntry` |

### 3.2 Estructura de Carpetas

```
Assets/
├── Scripts/
│   ├── Core/
│   │   ├── Main.cs
│   │   ├── GameManager.cs
│   │   ├── GameStats.cs
│   │   ├── InputManager.cs
│   │   └── HUDManager.cs
│   ├── Player/
│   │   ├── PlayerController.cs
│   │   └── PlayerManager.cs
│   ├── Enemies/
│   │   ├── EnemigoBase.cs
│   │   ├── Goomba.cs
│   │   ├── Koopa.cs
│   │   └── ActivationEnemies.cs
│   ├── PowerUps/
│   │   ├── PowerUpBase.cs
│   │   ├── Mushroom.cs
│   │   ├── Flower.cs
│   │   ├── Star.cs
│   │   └── 1_Up.cs
│   ├── Blocks/
│   │   ├── BlockBase.cs
│   │   ├── QuestionBlock.cs
│   │   └── BreakBlock.cs
│   ├── World/
│   │   ├── CameraFollow.cs
│   │   ├── DeadZone.cs
│   │   ├── PipeEntry.cs
│   │   ├── Checkpoint.cs
│   │   ├── Checkpoints.cs
│   │   ├── FireBall.cs
│   │   └── Moneda.cs
│   ├── Scene/
│   │   ├── InitializerScene1.cs
│   │   └── Initializer2.cs
│   └── i18n/
│       ├── i18n.cs
│       ├── Translations.cs
│       └── TranslateItem.cs
└── Resources/
    └── i18n/
        ├── _es.json
        └── _en.json
```

### 3.3 Diagrama de Dependencias

```
Main (RuntimeInitialize)
 └── i18n (translateManager)

GameManager (Singleton)
 ├── GameStats (datos de partida)
 ├── HUDManager (UI)
 ├── PlayerManager → PlayerController
 └── CameraFollow

PlayerController
 ├── InputManager (acciones de entrada)
 └── PlayerManager (estado y power-ups)

EnemigoBase
 ├── Goomba (override onStomp)
 └── Koopa (override onStomp + estados de caparazón)

BlockBase
 ├── QuestionBlock (un solo uso)
 └── BreakBlock (rompible o usado)

PowerUpBase
 ├── Mushroom
 ├── Flower
 ├── Star
 └── Up_1
```

---

## 4. Descripción Técnica de Componentes

### 4.1 Sistema de Gestión Global

#### `Main.cs`
Punto de entrada estático inicializado **antes de cargar cualquier escena** mediante `[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]`. Instancia el sistema de traducción `i18n` y lo expone globalmente en `Main.translateManager`.

#### `GameManager.cs`
Singleton persistente (`DontDestroyOnLoad`) que actúa como núcleo del juego.

Responsabilidades:
- Spawn y respawn del jugador en el último checkpoint alcanzado
- Cambio de niveles con pantalla de inicio animada
- Detección de Game Over y reinicio completo de la partida
- Gestión del timer y detención al perder
- Pausa del juego a través del `PauseMenu`
- Exposición de métodos de estadísticas globales

Métodos principales:

```csharp
GameManager.Instance.AddScore(int amount);       // Suma puntos y actualiza HUD
GameManager.Instance.AddCoin(int amount = 1);    // Suma monedas, da 1UP a las 100
GameManager.Instance.AddLife(int amount = 1);    // Suma vidas
GameManager.Instance.LoseLife();                 // Resta vida o dispara Game Over
GameManager.Instance.SetCheckpoint(Checkpoints id); // Guarda checkpoint actual
GameManager.Instance.ResetPlayerPosition();      // Mueve jugador al checkpoint
GameManager.Instance.TogglePause();              // Alterna pausa
```

#### `GameStats.cs`
Clase serializable que almacena el estado completo de la partida.

| Campo | Tipo | Valor por defecto |
|---|---|---|
| `score` | int | 0 |
| `coins` | int | 0 |
| `lives` | int | 3 (Normal) |
| `time` | float | 400 segundos |
| `difficulty` | Dificulty | Normal |

Vidas según dificultad al llamar `ResetGameStats()`:

| Dificultad | Vidas |
|---|---|
| Easy | 10 |
| Normal | 3 |
| Hard | 1 |

#### `InputManager.cs`
Singleton estático que centraliza el acceso al sistema de entrada (`PlayerInputActions`). Permite activar y desactivar mapas de acciones desde cualquier script.

```csharp
InputManager.SwitchMap(InputManager.Actions.Player);      // Activa mapa del jugador
InputManager.DisableMap(InputManager.Actions.Player);     // Desactiva un mapa
InputManager.Actions.Player.Jump.performed += callback;   // Suscribirse a acciones
```

#### `HUDManager.cs`
Singleton persistente que gestiona toda la interfaz gráfica del juego.

- Puntuación en formato de 6 dígitos (`D6`)
- Monedas en formato de 2 dígitos (`D2`)
- Timer decremental con corrutina (`TimerRoutine`)
- Pantalla de inicio de nivel con vidas restantes (5 segundos en tiempo real)
- Pantalla de Game Over (5 segundos en tiempo real)

---

### 4.2 Sistema del Jugador

#### `PlayerController.cs`
Controla toda la física de movimiento del jugador.

**Movimiento horizontal:**
- Aceleración y desaceleración suaves con `Mathf.MoveTowards`
- Velocidad normal: `3 u/s` | Corriendo: `8 u/s`
- Flip de sprite automático según dirección

**Salto:**
- Fuerza de impulso vertical (`ForceMode2D.Impulse`)
- Recorte de salto al soltar la tecla (`_jumpCutMultiplier`)
- Caída mejorada con `fallMultiplier` y `lowJumpMultiplier`
- Detección de suelo con 3 raycasts (centro, izquierda, derecha)

**Bolas de fuego:**
- Máximo 2 activas simultáneamente
- Se disparan en la dirección que mira el sprite
- Limpian las referencias nulas de la lista automáticamente

**Muerte:**
- Desactiva controles e input
- Aplana el collider
- Anima con salto hacia arriba y gravedad personalizada
- Recarga la escena y llama a `LoseLife()` tras el delay

Parámetros configurables en el Inspector:

| Parámetro | Descripción |
|---|---|
| `_jumpForce` | Fuerza de salto |
| `_maxVelocity` | Velocidad máxima horizontal |
| `_acceleration` | Velocidad de aceleración |
| `_deceleration` | Velocidad de frenado |
| `_jumpCutMultiplier` | Multiplicador de recorte de salto |
| `fallMultiplier` | Intensidad de caída rápida |
| `lowJumpMultiplier` | Intensidad de salto bajo |
| `deathJumpForce` | Fuerza del salto de muerte |
| `deathGravity` | Gravedad durante animación de muerte |
| `_fireBallPrefab` | Prefab de la bola de fuego |

#### `PlayerManager.cs`
Gestiona el **estado del jugador**. El GameObject del jugador persiste entre escenas al usar `DontDestroyOnLoad` en su `Awake`, conservando el estado (Small, Super, Fire) al cambiar de nivel. Cualquier script que necesite acceder a él obtiene la referencia mediante `GetComponent<PlayerManager>()`.

**Estados del jugador:**

| Estado | Collider | Animación |
|---|---|---|
| `Small` | 0.7 × 1.0 | MarioState = 0 |
| `Super` | 0.7 × 2.0 | MarioState = 1 |
| `Fire` | 0.7 × 2.0 | MarioState = 2 |

**Lógica de daño:**

```
Fire  → daño → Super
Super → daño → Small
Small → daño → Die()
```

Al recibir daño se activa **invulnerabilidad temporal** (3 segundos) con efecto de parpadeo y desactivación de colisiones con la capa `Enemigos`.

**Power-ups:**

```csharp
ApplyPowerUp(PowerUpType.Mushroom);    // Small → Super
ApplyPowerUp(PowerUpType.FireFlower);  // → Fire
ApplyPowerUp(PowerUpType.Star);        // Invencibilidad 10s
ApplyPowerUp(PowerUpType.Up_1);        // +1 vida
```

La transición de estado congela el tiempo (`Time.timeScale = 0`) y reproduce una animación en `UnscaledTime` durante 0.6 segundos.

---

### 4.3 Sistema de Enemigos

#### `EnemigoBase.cs`
Clase base para todos los enemigos. Proporciona movimiento horizontal, detección de paredes y gestión de colisiones.

- Solo procesa física cuando `isActive = true`
- Al recibir colisión desde arriba (`contact.normal.y < -0.5f`) → llama a `onStomp()`
- Al recibir colisión lateral → llama a `OnPlayerCollision()`
- Si el jugador tiene estrella activa → `Die()` inmediato

Métodos sobreescribibles:

```csharp
protected virtual void onStomp() { }           // Comportamiento al ser pisado
protected virtual void OnPlayerCollision() { }  // Comportamiento al tocar al jugador
protected virtual void OnCollisionEnter2D() { } // Colisiones generales
```

#### `Goomba.cs`
Extiende `EnemigoBase`. Al ser pisado:
1. Desactiva Rigidbody2D y Animator
2. Muestra sprite de aplastado
3. Espera 0.5 segundos y se destruye
4. Llama a `AddStompComboPoints()` en el GameManager

#### `Koopa.cs`
Extiende `EnemigoBase`. Sistema de estados:

| Estado | `isShell` | `isMovingShell` | Comportamiento |
|---|---|---|---|
| Normal | false | false | Camina horizontalmente |
| Caparazón detenido | true | false | Quieto, timer de `wakeUpTime` segundos |
| Caparazón en movimiento | true | true | Se lanza a `shellSpeed`, mata enemigos |

Al lanzarse el caparazón, hay un delay de 5 segundos antes de que pueda dañar al jugador (`canKill`). Si ningún jugador interactúa con el caparazón detenido, este sale del caparazón tras `wakeUpTime` segundos y retoma el movimiento normal.

#### `ActivationEnemies.cs`
Trigger que activa y desactiva enemigos por proximidad. Al entrar en el trigger, `isActive = true`; al salir, `isActive = false`. Optimiza el rendimiento evitando que los enemigos procesen física fuera del rango visible.

---

### 4.4 Sistema de Power-Ups

#### `PowerUpBase.cs`
Clase base para todos los power-ups. Gestiona la emergencia desde bloques y la recolección.

```csharp
EmergeFromBlock();             // Desactiva collider y Rigidbody mientras emerge
FinishEmerging();              // Reactiva física al terminar la animación de emergencia
Collect(player, PowerUpType);  // Aplica el power-up, suma 1000 puntos y destruye el objeto
```

#### Tipos de Power-Up

| Power-Up | Script | Comportamiento especial |
|---|---|---|
| Mushroom | `Mushroom.cs` | Se mueve horizontalmente, cambia de dirección al chocar |
| Fire Flower | `Flower.cs` | Estático |
| Star | `Star.cs` | Se desplaza y rebota automáticamente sobre el suelo |
| 1-Up | `1_Up.cs` | Se mueve horizontalmente como el Mushroom |

---

### 4.5 Sistema de Bloques

#### `BlockBase.cs`
Clase base con toda la lógica de interacción. Se activa cuando el jugador golpea el bloque desde abajo (`contact.normal.y > 0.5f`).

Tipos de bloque configurables en el Inspector:

| Propiedad | Descripción |
|---|---|
| `isCoinBlock` | Da moneda(s) al golpearlo |
| `isPowerUpBlock` | Da power-up según estado del jugador |
| `is1UpBlock` | Da vida extra |
| `isStarBlock` | Da una estrella |
| `isMultiCoinBlock` | Da múltiples monedas durante `multiCoinDuration` segundos |

El power-up de `isPowerUpBlock` depende del estado actual del jugador:
- `Small` → Mushroom
- `Super` / `Fire` → Fire Flower

**Animación de golpe:** el bloque sube `offsetY` unidades y vuelve a su posición original mediante `Vector3.MoveTowards` en una corrutina.

**Animación de moneda:** la moneda emerge del bloque hasta 3 unidades de altura y regresa antes de destruirse.

**Animación de power-up:** el power-up emerge 1 unidad por encima del bloque con física desactivada, y la reactiva al llegar a la posición objetivo.

#### `QuestionBlock.cs`
Bloque `?` de un solo uso. Al golpearlo cambia al sprite de usado (`usedSprite`) y desactiva su animador. Soporta multi-moneda.

#### `BreakBlock.cs`
Bloque rompible. Lógica al ser golpeado:
- Si `isBreakable = true` y el jugador es `Super` o `Fire` → se destruye tras 0.5 segundos
- Si `isBreakable = true` y el jugador es `Small` → rebota sin romperse
- Si `isUsed = true` → no hace nada
- Si es bloque de monedas → delega a `BlockBase.OnHit()`

---

### 4.6 Sistema de Checkpoints

#### `Checkpoints.cs` (enum)

```csharp
public enum Checkpoints
{
    L1_1_A,   // Inicio del nivel 1-1
    L1_1_B,   // Punto medio del nivel 1-1
    L1_1_C,   // Final del nivel 1-1
    LS1_1_A,  // Sub-nivel accesible por tubería
}
```

#### `Checkpoint.cs`
Trigger invisible en la escena. Al ser tocado por el jugador, llama a `GameManager.Instance.SetCheckpoint(checkpointID)`. El GameManager almacena el último checkpoint y lo usa para el respawn.

#### `PipeEntry.cs`
Permite al jugador entrar en tuberías al presionar la dirección correcta.

| Dirección configurada | Input requerido |
|---|---|
| `Down` | Stick/tecla abajo (y < -0.5) |
| `Left` | Stick/tecla izquierda (x < -0.5) |
| `Right` | Stick/tecla derecha (x > 0.5) |

Al entrar: guarda el checkpoint de la tubería con `SetCheckpoint()` y carga la escena configurada en `sceneName`.

---

### 4.7 Sistema de Combos

#### Shell Combo (Caparazón)
Se incrementa cada vez que el caparazón en movimiento elimina un enemigo. Se resetea al detener el caparazón.

| Combo | Puntos |
|---|---|
| ×1 | 500 |
| ×2 | 1.000 |
| ×3 | 2.000 |
| ×4 | 4.000 |
| ×5 | 5.000 |
| ×6 | 8.000 |
| ×7+ | 1UP |

#### Stomp Combo (Pisoteo)
Se incrementa cada vez que el jugador pisa un enemigo sin tocar el suelo entre pisotones. Se resetea al aterrizar.

| Combo | Puntos |
|---|---|
| ×1 | 100 |
| ×2 | 200 |
| ×3 | 400 |
| ×4 | 800 |
| ×5 | 1.000 |
| ×6 | 2.000 |
| ×7 | 4.000 |
| ×8 | 5.000 |
| ×9 | 8.000 |
| ×10+ | 1UP |

---

### 4.8 Sistema de Internacionalización (i18n)

#### `i18n.cs`
Carga archivos JSON de idioma desde `Resources/i18n/` y expone `GetTranslation(key)`. Por defecto carga español (`es`). Al cambiar de idioma, invoca `OnLanguageChanged` para notificar a todos los componentes suscritos.

```csharp
Main.translateManager.LoadLanguage("en");            // Cambiar idioma en tiempo de ejecución
Main.translateManager.GetTranslation("ui_score");    // Obtener traducción por clave
```

#### Formato del JSON

Los archivos deben colocarse en `Assets/Resources/i18n/` con nombre `_CODIGO.json` (ej. `_es.json`, `_en.json`).

**Claves disponibles actualmente:**

| Clave | Español (`_es`) | Inglés (`_en`) | Uso |
|---|---|---|---|
| `_Play` | JUGAR | PLAY | Botón de inicio en menú |
| `_Config` | Configuración | Settings | Botón de ajustes |
| `_Exit` | Salir | Exit | Botón de salida |
| `_Lives` | VIDAS | LIVES | Etiqueta de vidas en HUD |
| `_Time` | Tiempo | Time | Etiqueta de tiempo en HUD |
| `_Level` | Mundo | World | Etiqueta de nivel en HUD |
| `_Dificulty` | Dificultad | Difficulty | Selector de dificultad |
| `_Easy` | Facil | Easy | Opción de dificultad |
| `_Normal` | Normal | Normal | Opción de dificultad |
| `_Hard` | Dificil | Hard | Opción de dificultad |
| `_Language` | Idioma | Language | Selector de idioma |

**Ejemplo de archivo `_es.json`:**

```json
{
  "traducciones": [
    { "key": "_Play",      "value": "JUGAR" },
    { "key": "_Config",    "value": "Configuración" },
    { "key": "_Exit",      "value": "Salir" },
    { "key": "_Lives",     "value": "VIDAS" },
    { "key": "_Time",      "value": "Tiempo" },
    { "key": "_Level",     "value": "Mundo" },
    { "key": "_Dificulty", "value": "Dificultad" },
    { "key": "_Easy",      "value": "Facil" },
    { "key": "_Normal",    "value": "Normal" },
    { "key": "_Hard",      "value": "Dificil" },
    { "key": "_Language",  "value": "Idioma" }
  ]
}
```

#### `TranslateItem.cs`
Componente que se adjunta a cualquier `TextMeshProUGUI`. Requiere que el GameObject tenga el componente `TextMeshProUGUI`. Se suscribe automáticamente a `OnLanguageChanged` para actualizarse al cambiar de idioma. Solo requiere configurar la `key` desde el Inspector.

---

### 4.9 Sistema de Cámara y Mundo

#### `CameraFollow.cs`
Sigue al jugador **solo en el eje X** (horizontal). Implementa un límite `maxCameraX` que impide que la cámara retroceda aunque el jugador vuelva hacia atrás. Al instanciar un nuevo jugador, se llama a `SetTarget()` para recalcular el límite y reposicionar la cámara.

El campo `offsetX` (configurable desde el Inspector) permite ajustar el desplazamiento horizontal entre la cámara y el jugador, anticipando la visión hacia adelante o manteniéndola centrada según se prefiera.

#### `DeadZone.cs`
Trigger en la parte inferior del nivel. Al detectar al jugador, espera 2 segundos, recarga la escena y llama a `LoseLife()`. Cualquier otro objeto que caiga en la zona muerta se destruye inmediatamente.

#### `FireBall.cs`
Proyectil disparado por Mario en estado Fire.

- Se desplaza horizontalmente a `speed u/s`
- Caída acelerada con `gravityMultiplier`
- Rebota en suelos (contacto con `normal.y > 0.5`)
- Se destruye al chocar con paredes (raycast lateral)
- Al golpear un enemigo: llama a `Die()`, suma 200 puntos y se destruye
- Se destruye automáticamente tras `lifeTime` segundos (por defecto 4s)

#### `Moneda.cs`
Moneda de mundo (no procedente de bloque). Al ser tocada por el jugador: +1 moneda, +200 puntos y se destruye.

---

## 5. Flujo de Juego

### 5.1 Ciclo de Inicio

```
Main.Init() [RuntimeInitializeOnLoadMethod]
 └── i18n instanciado (idioma por defecto: "es")

MainScene cargada (menú principal)
 └── Jugador selecciona dificultad e inicia partida

GameManager.Awake()
 └── SpawnPlayer() en posición inicial
 └── HUDManager inicializado

Level 1-1 cargado
 └── InitializerScene1.Awake()
     ├── InputManager.SwitchMap(Player)
     └── GameManager.ResetPlayerPosition()
 └── HUDManager.ShowLevelStartScreen() (5 segundos)
 └── TimerRoutine iniciado (cuenta regresiva desde 400s)
```

### 5.2 Ciclo de Nivel

```
Loop de juego
 ├── Jugador se mueve, salta y corre
 ├── Colisión con bloque desde abajo
 │    └── BlockBase.OnHit()
 │         ├── Animación de rebote
 │         ├── GiveCoins() → +moneda, +200 puntos
 │         └── GivePowerUp() → emerge power-up
 ├── Jugador toca power-up
 │    └── PowerUpBase.Collect()
 │         ├── PlayerManager.ApplyPowerUp()
 │         └── +1000 puntos
 ├── Jugador pisa enemigo
 │    └── EnemigoBase.onStomp()
 │         └── GameManager.AddStompComboPoints()
 ├── Jugador cae a DeadZone
 │    └── DeathRoutine (2s delay)
 │         ├── SceneManager.LoadScene (escena actual)
 │         └── GameManager.LoseLife()
 │              ├── lives > 0 → Respawn + ShowLevelStartScreen
 │              └── lives ≤ 0 → GameOver()
 └── Timer llega a 0 → LoseLife() automático
```

### 5.3 Ciclo de Game Over

```
GameManager.GameOver()
 ├── StopCoroutine (timer)
 ├── SceneManager.LoadScene(0) [MainScene]
 ├── HUDManager.ShowGameOverScreen() (5 segundos)
 ├── stats.ResetGameStats()
 └── SpawnPlayer() en posición inicial
```

---

## 6. Controles

### Mapa: Player (durante el juego)

| Acción | Tecla |
|---|---|
| Mover izquierda | `A` |
| Mover derecha | `D` |
| Agacharse / Entrar tubería abajo | `S` |
| Saltar | `Space` |
| Correr / Disparar bola de fuego | `Left Shift` |
| Pausar | `Escape` |

### Mapa: UIcontrolls (en menús)

| Acción | Tecla |
|---|---|
| Confirmar / Submit | `Enter` |
| Cancelar / Volver | `Escape` |
| Navegar arriba | `W` |
| Navegar abajo | `S` |

> Los controles se gestionan mediante el **New Input System** de Unity a través de `InputManager` y el asset `PlayerInputActions`. El sistema tiene dos mapas de acción separados (`Player` y `UIcontrolls`) que se activan y desactivan según el contexto usando `InputManager.SwitchMap()` y `InputManager.DisableMap()`.

---

## 7. Decisiones de Diseño

### 7.1 Persistencia entre Escenas con DontDestroyOnLoad
**Problema:** Al cambiar de escena, Unity destruye todos los objetos de la escena anterior.

**Solución:** `GameManager`, `HUDManager` y `PlayerManager` usan `DontDestroyOnLoad`. Se implementa protección singleton para evitar duplicados al recargar escenas:

```csharp
private void Awake()
{
    if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
    else Destroy(gameObject);
}
```

### 7.2 Sistema de Combos Progresivos
**Problema:** El pisoteo y el uso del caparazón necesitaban recompensa creciente para incentivar el encadenamiento.

**Solución:** Tablas de puntos fijas indexadas por contador. Al superar el máximo de la tabla se otorga un 1UP. El combo de pisoteo se resetea al tocar el suelo; el de caparazón, al detenerlo. Esto crea una decisión táctica: ¿detengo el caparazón y pierdo el combo, o lo dejo moverse arriesgando perder el control?

### 7.3 Invulnerabilidad con Parpadeo
**Problema:** Al recibir daño, el jugador podía ser golpeado repetidamente antes de reaccionar.

**Solución:** `TemporaryInvulnerability` desactiva las colisiones entre las capas `Player` y `Enemigos` con `Physics2D.IgnoreLayerCollision` durante 3 segundos, combinado con parpadeo visual cada 0.1 segundos para feedback claro al jugador.

### 7.4 Transición de Estado con Congelado de Tiempo
**Problema:** El cambio de tamaño de Mario (Small ↔ Super) necesitaba feedback visual claro sin interrumpir el juego bruscamente.

**Solución:** `IsChangingState` congela `Time.timeScale = 0` y usa `AnimatorUpdateMode.UnscaledTime` para reproducir la animación de transformación durante 0.6 segundos en tiempo real, luego restaura el tiempo normal.

### 7.5 Cámara con Límite Horizontal
**Problema:** En los juegos de plataformas es indeseable que la cámara retroceda, ya que el jugador podría ver partes del nivel ya pasadas.

**Solución:** `CameraFollow` mantiene un valor `maxCameraX` que solo se incrementa, nunca se reduce. La cámara solo avanza con el jugador, nunca retrocede.

### 7.6 Activación de Enemigos por Proximidad
**Problema:** Procesar la física de todos los enemigos del nivel desde el inicio es costoso y puede causar comportamientos inesperados fuera de pantalla.

**Solución:** `ActivationEnemies` usa triggers para activar (`isActive = true`) y desactivar (`isActive = false`) cada `EnemigoBase`. El `FixedUpdate` de los enemigos comprueba `isActive` antes de ejecutar cualquier lógica.

---

## 8. Guía para Desarrolladores

### 8.1 Cómo Leer y Navegar el Código

Flujo de lectura recomendado:

1. `Main.cs` → Inicialización global antes de cargar escenas
2. `GameManager.cs` + `GameStats.cs` → Sistema de datos y ciclo de vida de la partida
3. `InputManager.cs` → Cómo se gestiona la entrada
4. `PlayerController.cs` + `PlayerManager.cs` → Mecánicas del jugador
5. `EnemigoBase.cs` → Lógica base de enemigos
6. `BlockBase.cs` → Sistema de bloques interactivos
7. `PowerUpBase.cs` → Sistema extensible de power-ups

Patrón de acceso global:

```csharp
GameManager.Instance.AddScore(500);
GameManager.Instance.stats.lives;
HUDManager.Instance.UpdateHUD(stats);
Main.translateManager.GetTranslation("ui_score");
```

### 8.2 Cómo Añadir un Nuevo Enemigo

1. Crear el script heredando de `EnemigoBase`:

```csharp
public class NuevoEnemigo : EnemigoBase
{
    protected override void onStomp()
    {
        // Lógica al ser pisado
        GameManager.Instance.AddStompComboPoints();
        base.Die();
    }

    protected override void OnPlayerCollision(Collision2D collision)
    {
        PlayerManager player = collision.collider.GetComponent<PlayerManager>();
        if (player == null) return;

        if (player.IsStarActive()) { Die(); return; }
        player.TakeDamage();
    }
}
```

2. Crear un prefab con `Rigidbody2D`, `Collider2D` y el nuevo script.
3. Asignar el tag `Enemy` al prefab.
4. Colocar un `ActivationEnemies` trigger en la escena para activarlo por proximidad.

### 8.3 Cómo Añadir un Nuevo Power-Up

1. Crear el script heredando de `PowerUpBase`:

```csharp
public class NuevoPowerUp : PowerUpBase
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Collect(collision.gameObject, PowerUpType.NuevoTipo); // Añadir al enum
        }
    }
}
```

2. Añadir el nuevo tipo al enum `PowerUpType` en `PowerUpBase.cs`.
3. Añadir el caso en `PlayerManager.ApplyPowerUp()`:

```csharp
case PowerUpType.NuevoTipo:
    // Lógica del efecto
    break;
```

4. Crear el prefab y configurarlo en los bloques correspondientes.

### 8.4 Cómo Añadir un Nuevo Idioma

1. Crear el archivo JSON en `Assets/Resources/i18n/` con nombre `_CODIGO.json` (ej. `_fr.json`):

```json
{
  "traducciones": [
    { "key": "ui_score", "value": "SCORE" },
    { "key": "ui_lives", "value": "VIES" }
  ]
}
```

2. Llamar a `LoadLanguage` con el código del nuevo idioma:

```csharp
Main.translateManager.LoadLanguage("fr");
```

3. Todos los `TranslateItem` de la escena se actualizarán automáticamente vía el evento `OnLanguageChanged`.

### 8.5 Cómo Añadir un Nuevo Checkpoint

1. Añadir el nuevo ID al enum en `Checkpoints.cs`:

```csharp
public enum Checkpoints
{
    L1_1_A,
    L1_1_B,
    L1_1_C,
    LS1_1_A,
    L1_2_A,  // Nuevo checkpoint
}
```

2. Colocar un GameObject con el componente `Checkpoint` en la escena y asignar el nuevo ID desde el Inspector.

### 8.6 Buenas Prácticas

**Cachear referencias costosas:**
```csharp
// MAL: llamar en Update es costoso
void Update() { FindObjectOfType<PlayerManager>().TakeDamage(); }

// BIEN: cachear en Awake o Start
private PlayerManager _player;
void Awake() { _player = FindObjectOfType<PlayerManager>(); }
void Update() { _player.TakeDamage(); }
```

**Desuscribirse de eventos al destruir:**
```csharp
void OnEnable()  { InputManager.Actions.Player.Jump.performed += HandleJump; }
void OnDisable() { InputManager.Actions.Player.Jump.performed -= HandleJump; }
```

**Corrutinas vs Update:**
Usar corrutinas para efectos temporales con duración definida (invulnerabilidad, transformaciones, delays). Reservar `Update` y `FixedUpdate` para lógica continua por frame.

---

## 9. Requisitos Técnicos

- **Motor:** Unity 2022.x o superior
- **Paquetes requeridos:**
  - TextMeshPro
  - Input System (New Input System)
  - Physics 2D
- **Plataforma:** Windows / macOS / WebGL
- **Resolución recomendada:** 1920 × 1080

### Configuración de Build Settings

Las escenas deben estar registradas en Build Settings en el orden correcto:

| Índice | Escena |
|---|---|
| 0 | `Inicio` (Menú Principal) |
| 1 | `1-1` (Nivel principal) |
| 2 | `Secret1-1` (Sub-nivel tubería) |
