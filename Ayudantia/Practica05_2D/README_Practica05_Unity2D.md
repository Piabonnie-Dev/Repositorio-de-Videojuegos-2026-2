# Práctica 05 - Unity 2D
## Enemigos, patrullaje por colisión e inteligencia artificial básica con herencia

Este proyecto corresponde a la **Práctica 05** de Ayudantía en **Unity 2D**. La práctica es acumulativa respecto a la Práctica 04 y tiene como propósito implementar un enemigo básico con comportamiento tipo **Goomba**, usando **herencia** mediante una superclase llamada `Personaje`, compartida por el jugador y el enemigo.

## Objetivo general
Implementar un enemigo básico que patrulle automáticamente, detecte colisiones con el entorno y con el jugador, y aproveche un diseño orientado a objetos por medio de una superclase compartida.

## Objetivos específicos de la práctica
- Crear una superclase `Personaje` con atributos y métodos comunes.
- Hacer que `Jugador` y `EnemigoIA` hereden de `Personaje`.
- Implementar patrullaje automático basado en colisiones.
- Detectar al jugador mediante colisiones.
- Manejar daño básico al jugador.
- Integrar animaciones simples del enemigo.

## Estructura del proyecto
Los scripts principales del proyecto son los siguientes:

### `Personaje.cs`
Clase base abstracta que concentra la lógica compartida entre jugador y enemigo.

Incluye:
- `velocidad`
- `vida`
- referencia a `Rigidbody2D`
- referencia a `SpriteRenderer`
- método `RecibirDaño(int cantidad)`
- método `Morir()`

### `Jugador.cs`
Hereda de `Personaje` y se encarga de registrar si el personaje está o no en el suelo mediante colisiones con objetos etiquetados como `Suelo`.

### `ControldeJugador.cs`
Controla el movimiento del jugador.

Incluye:
- movimiento horizontal con aceleración y desaceleración
- salto con buffer y coyote time
- gravedad mejorada
- control de estados para animación
- volteo del sprite
- reproducción de sonidos de salto, pasos e impacto

### `EnemigoIA.cs`
Hereda de `Personaje` y controla el comportamiento básico del enemigo.

Incluye:
- patrullaje horizontal automático
- cambio de dirección al chocar con pared u obstáculo
- volteo del sprite al cambiar de dirección
- actualización de animación simple con el parámetro `Caminando`
- daño al jugador mediante colisión

## Controles
- **Moverse:** `A / D` o flechas izquierda y derecha
- **Saltar:** barra espaciadora o tecla configurada en `Jump`

## Configuración necesaria en la escena
Para que el proyecto funcione correctamente, los objetos deben estar configurados con sus componentes y etiquetas adecuadas.

### Jugador
Debe tener:
- `SpriteRenderer`
- `Animator`
- `Rigidbody2D`
- `Collider2D`
- `AudioSource`
- script `Jugador`
- script `ControldeJugador`
- tag `Player`

### Suelo / plataformas
Debe tener:
- `Collider2D`
- tag `Suelo`

### Enemigo
Debe tener:
- `SpriteRenderer`
- `Rigidbody2D`
- `Collider2D`
- script `EnemigoIA`
- `Animator` si se usan animaciones

### Paredes laterales / límites
Debe tener:
- `Collider2D`
- tag `Pared`

### Obstáculos
Si se usan obstáculos para cambiar la dirección del enemigo, deben tener:
- `Collider2D`
- tag `Obstaculo`

## Parámetros de Animator esperados
### Jugador
El `Animator` del jugador utiliza los parámetros:
- `Caminando`
- `Saltando`
- `Cayendo`

### Enemigo
El `Animator` del enemigo utiliza:
- `Caminando`

## Funcionamiento general
1. El jugador puede desplazarse horizontalmente y saltar.
2. El sistema de salto incluye coyote time, jump buffer y control de gravedad.
3. El enemigo se mueve automáticamente de un lado a otro.
4. Cuando el enemigo choca con una pared u obstáculo, invierte su dirección.
5. Cuando el jugador colisiona con el enemigo, se activa la lógica de daño.
6. El proyecto usa herencia para reutilizar atributos y comportamiento base entre jugador y enemigo.

## Recomendaciones para pruebas
- Verificar que todas las tags existan antes de ejecutar la escena.
- Confirmar que el jugador tenga la tag `Player`.
- Confirmar que el suelo tenga la tag `Suelo`.
- Confirmar que las paredes tengan la tag `Pared`.
- Revisar que los colliders no estén mal dimensionados.
- En caso de fallos en la mecánica de “pisotón”, ajustar colliders y la lógica de colisión del enemigo.

## Carpeta sugerida
La práctica debe almacenarse en la ruta:

`/Ayudantia/Practica05/`

## Entregable
- Carpeta completa de la práctica.
- Commits claros y progresivos.
- Archivo `README.md` con instrucciones.
- Demostración en clase del enemigo funcionando.

## Créditos
Práctica desarrollada en Unity 2D como parte del curso de ayudantía, enfocada en herencia, colisiones, patrullaje automático y lógica básica de enemigos.

## Autor

## Elliot Kenneth Villafaña Pasten 