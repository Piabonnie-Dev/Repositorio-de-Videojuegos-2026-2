# Práctica 1 (Teoría): Juego Pong en OpenGL

## Descripción del proyecto
Este proyecto consiste en la implementación de una versión básica del juego clásico **Pong** utilizando **OpenGL** y **GLUT** en C++. El programa permite controlar dos paletas, mover una pelota dentro del escenario, detectar colisiones con las paletas y con los bordes superior e inferior, además de llevar el marcador de ambos jugadores.

La práctica está orientada a reforzar conceptos fundamentales de gráficos por computadora, entrada por teclado, animación básica y detección de colisiones, tal como se solicita en la guía de la práctica. 

## Objetivo de la práctica
Desarrollar una versión funcional de Pong en OpenGL que permita:

- mover dos paletas con teclado,
- hacer rebotar la pelota dentro del escenario,
- detectar colisiones entre pelota y paletas,
- sumar puntos cuando la pelota toca el borde izquierdo o derecho,
- reiniciar la pelota al centro después de cada punto. 

## Características implementadas
A partir del archivo fuente proporcionado, el juego incluye las siguientes funciones:

- Pelota dibujada como círculo usando polígonos en OpenGL.
- Movimiento continuo de la pelota en el eje X y Y.
- Rebote de la pelota en el techo y en el suelo.
- Dos paletas controlables por dos jugadores.
- Restricción de movimiento para que las paletas no salgan de la pantalla.
- Detección de colisión entre la pelota y ambas paletas.
- Cambio de dirección de la pelota dependiendo del punto de impacto en la paleta.
- Sistema de puntuación para Jugador 1 y Jugador 2.
- Reinicio de pelota y paletas después de anotar un punto.
- Inicio del juego con la barra espaciadora.
- Marcador e instrucciones dibujadas dentro de la ventana del juego. 

## Archivos del proyecto
Este README está basado en el archivo fuente en C++ del juego Pong y en las instrucciones del PDF de la práctica. 

Ejemplo de estructura recomendada por la práctica:

```text
/Teoria
  /Practica01
    Pong.cpp
    README.md
    README_instrucciones.txt
```

## Dependencias necesarias
Para compilar y ejecutar el proyecto se requiere:

- Compilador de C++.
- OpenGL.
- GLUT o FreeGLUT.
- GLU.

La guía de la práctica indica que en Linux/Ubuntu puede instalarse `freeglut3-dev` y compilarse con las bibliotecas de OpenGL correspondientes.

## Nota importante sobre este código
El archivo fuente actual incluye:

```cpp
#include <Windows.h>
#include <GL/glut.h>
```

Por lo tanto, **esta versión está preparada directamente para Windows**. Si deseas compilar en Linux, normalmente tendrás que **eliminar o adaptar** la línea `#include <Windows.h>`, porque esa cabecera es específica de Windows. Esto se deduce del código fuente actual. 

## Cómo compilar

### Opción 1: Windows (recomendado para este código)
Compila el proyecto en un entorno con **Visual Studio + OpenGL + FreeGLUT** correctamente configurados.

Pasos generales:

1. Crear un proyecto de C++ vacío.
2. Agregar el archivo fuente del juego.
3. Configurar las bibliotecas de OpenGL y FreeGLUT.
4. Compilar y ejecutar.

La práctica indica seguir una guía de configuración de OpenGL para Visual Studio en Windows. fileciteturn0file1L13-L21

### Opción 2: Linux / Ubuntu
Si adaptas el archivo para Linux, una compilación típica sería:

```bash
sudo apt install freeglut3-dev
g++ -o Pong Pong.cpp -lGL -lglut -lGLU
```

Este comando está basado en el formato indicado en el PDF de la práctica, aunque puede requerir ajustes por el uso de `Windows.h` en tu código actual. 

## Cómo ejecutar
Después de compilar, ejecuta el programa generado.

### En Windows
Ejecuta el `.exe` desde Visual Studio o desde la carpeta del proyecto.

### En Linux
```bash
./Pong
```

## Controles del juego
Los controles identificados en tu código son los siguientes:

### Jugador 1
- `W` = mover paleta hacia arriba
- `S` = mover paleta hacia abajo

### Jugador 2
- `Flecha arriba` = mover paleta hacia arriba
- `Flecha abajo` = mover paleta hacia abajo

### Control general
- `Espacio` = iniciar el movimiento de la pelota
- `ESC` = salir del juego 

## Funcionamiento del juego
1. Al iniciar, la pelota aparece en el centro de la ventana.
2. El juego comienza al presionar la tecla `Espacio`.
3. Cada jugador mueve su paleta verticalmente para evitar que la pelota pase.
4. La pelota rebota en el borde superior e inferior.
5. Si la pelota toca el borde derecho, se suma un punto al Jugador 1.
6. Si la pelota toca el borde izquierdo, se suma un punto al Jugador 2.
7. Después de cada punto, la pelota vuelve al centro y el juego queda en pausa hasta volver a presionar `Espacio`. 

## Lógica implementada en el código
El programa utiliza:

- `Display()` para dibujar y actualizar el estado del juego.
- `keyboard()` para las teclas del Jugador 1 y la barra espaciadora.
- `specialKeys()` para las teclas especiales del Jugador 2.
- `resetBall()` para reiniciar la pelota y las paletas al centro después de anotar.
- `drawHUD()` para mostrar el marcador y el mensaje de inicio.
- `Timer()` para forzar la actualización de la pantalla aproximadamente cada 16 ms. 

## Observaciones
- El marcador actualmente se muestra **dentro de la ventana del juego**, no en la terminal.
- El tamaño lógico del escenario se define con `gluOrtho2D(0.0, 160.0, 0.0, 120.0)`.
- La ventana se crea con el título **"Juego Pong Elliot Kenneth"**. 

## Relación con los requerimientos de la práctica
Este proyecto cumple con los elementos principales solicitados:

- Implementación de un Pong básico.
- Movimiento de dos jugadores.
- Rebotes y colisiones.
- Reinicio de la pelota tras anotar.
- Instrucciones de compilación y ejecución.
- Controles y dependencias documentados en el README. 

## Autor
**Elliot Kenneth Villafaña Pasten**


