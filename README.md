# Manual de Usuario - Pac-Man 2D con RTDESK en Unity
**Basado en** https://github.com/zigurous/unity-pacman-tutorial

Por José Daniel Galván & Joshua Diaz

**Asignatura**: Motores de Videojuegos

## Requerimientos
Por favor emplear versión 2022.3.45f1 de Unity Editor para poder trabajar correctamente con este proyecto.

## Introducción
Bienvenido a **Pac-Man 2D**, un juego clásico de arcade desarrollado en Unity. En este juego, controlarás a Pac-Man  en un modo colaborativo de dos jugadores mientras recorren un laberinto, comen puntos y evitan a los fantasmas. ¡Obtén el mayor puntaje!

## Instalación
1. Abre el proyecto en el editor de Unity.
2. Verifica que Unity realice la carga correcta de todos los componentes de estas fuentes
3. Abre la escena llamada **Inicio**.
4. Ejecuta "Play" a la escena.

## Controles, multijugador:

### jugador 1
- **Tecla Flecha Arriba**: Mover hacia arriba
- **Tecla Flecha Abajo**: Mover hacia abajo
- **Tecla Flecha Izquierda**: Mover hacia la izquierda
- **Tecla Flecha Derecha**: Mover hacia la derecha

### jugador 2
- **Tecla W**: Mover hacia arriba
- **Tecla S**: Mover hacia abajo
- **Tecla A**: Mover hacia la izquierda
- **Tecla D**: Mover hacia la derecha

## Objetivo del Juego
El objetivo del juego es simple:
- Recoge todos los puntos en el laberinto para ganar.
- Acumula puntos y obten el mayor posible
- Evita a los fantasmas que patrullan el laberinto.
- Come las "píldoras de poder" para volver vulnerables a los fantasmas temporalmente y poder devorarlos.

## Elementos del Juego
- **Pac-Man**: El personaje principal, controlado por los jugadores.
- **Fantasmas**: Enemigos que intentan atrapar a Pac-Man. Existen diferentes tipos con comportamientos distintos.
- **Puntos**: Pequeñas bolas distribuidas en el laberinto que Pac-Man debe comer para ganar puntos.
- **Píldoras de Poder**: Permiten a Pac-Man comerse a los fantasmas durante un tiempo limitado.

## Puntuación
- Punto normal: **10 puntos**
- Píldora de poder: **50 puntos**
- Fantasma devorado: **200, 400, 800, 1600 puntos (dependiendo del orden en que sean comidos)**


## RTDESK
En el presente proyecto se ha implementado el paquete RTDESK para la ejecución de simulación discreta desacoplada. 

Se implementaron 3 tipos de eventos:
- Pacman utiliza la clase InputManager para la captura de eventos del teclado, con lo cual se evita que esa validación de lectura se repita por cada frame en el método Update.
- Los scripts Movement y AnimatedSprite se envían mensajes a sí mismos para la ejecución continua del movimiento y animación del sprite respectivamente.
- El script Passage, al detectar una colisión, envía un mensage a Pacman o al fantasma que colisionó con el tunel lateral para que el GameObject que colisionó, cambie de posición y se simule la "teletransportación" hacia el tunel lateral del lado contrario.


*¡Esperamos que disfrutes jugando Pac-Man 2D!*