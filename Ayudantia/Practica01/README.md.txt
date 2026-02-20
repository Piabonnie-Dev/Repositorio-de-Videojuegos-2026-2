Instrucciones para ejecutar los inputs:

Al iniciar el proyecto, hay dos jugadores, el jugador 1 que es el que esta acostado y tiene los dos scripts:

-"Jugador.cs"  // 
- y "ControldeJugador.cs"

Cada jugador esta metido en un objeto vacio y esta metido el prefab de ambos jugadores.

-El jugador 2 solo tiene el script "Jugador.cs" que detecta la colisión con el otro jugador 1 y viceversa con el jugador 1.


-Jugador.cs: Es el script que detecta las colisiones llamando un debug.log() a la consola con que objeto esta chocando y su nombre, tiene 3  entradas una de enter que llama a la consola "entre en contacto con", en STAY que llama a la consola "sigo en contacto con:" y en EXIT que llama a la consola "Deje de colisionar con:"

-Controldejugador.cs: Este script detecta los inputs de mi jugador 1, llama con un debug.log() cuando presiono la tecla SpaceBar y en la consola sale "Salto" y otra variabe float llamada h que le metemos un metodo booleano en el que llama el input.GetAxis("Horizontal"), y solo agregando en el Debug.log() agrego Debug.Log("Movimiento Horizontal" + h);. 

Con esto en la consola cada vez que apreto A y D me indica la consola que me estoy moviendo ya sea a la izquierda o a la derecha, A: -1, B: 1, y se queda h= 0 cuando no me muevo. 

Saludos. 

Villafaña Pasten Elliot Kenneth
 