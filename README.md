# Historial de Versiones

![master](https://img.shields.io/badge/master-v0.0.0-green?style=for-the-badge&logo=appveyor)
![develop](https://img.shields.io/badge/develop-v0.3.0-blue?style=for-the-badge&logo=appveyor)

| Versión | Descripción |
| ------------- | ------------- |
|![develop](https://img.shields.io/badge/develop-v0.3.0-blue?style=for-the-badge&logo=appveyor) | - Movimiento entre todas las caras del cubo <br> - Colisiones, recoger objeto <br>- Disparo de bola de nieve <br> - Diseño del skybox <br> - Ampliación de los modos de juego <br> - Diseño esquemático de las interfaces ampliado para modo multijugador <br> - Cambios en Photon para mejorar la interpolación que realiza|
|![develop](https://img.shields.io/badge/develop-v0.2.0-blue?style=for-the-badge&logo=appveyor) | - Movimiento en 3 de las caras del cubo <br> - Diseño de la interfaz básica (concept) y botones <br> - Modelos 3D de 6 personajes con texturizado <br> - Sala para dos jugadores <br> - Arreglos de colisiones en el cubo <br> - Arreglos de bugs varios| 
![develop](https://img.shields.io/badge/develop-v0.1.0-blue?style=for-the-badge&logo=appveyor)  | - Interacción con el cubo <br> - Movimientos de cámara<br>- Primeras aproximaciones respecto al movimiento del personaje <br> - GDD semi-completo<br> - Bocetos y diseños iniciales  
|


# Java is Creep

Somos una empresa dedicada al desarrollo de videojuegos con un potencial de diversion explosivo.

[![Twitter](https://img.shields.io/twitter/follow/java_is_creep?label=Follow&style=social)](https://twitter.com/intent/follow?screen_name=java_is_creep)

[![Instagram](https://img.shields.io/badge/Follow--green?style=social&logo=instagram)](https://www.instagram.com/java_is_creep/)


## Roles de los integrantes
- Game Designer: Axel
- Artista 2D: Leo
- Artista 3D: Leo - Fonti - Axel
- Programadores:
    - Multijugador: Dani
    - Físicas: Sergio
-	Música: Axel – Fonti
-	CM: Axel – Fonti
-   Técnico: Fonti



# ICE<sup>3</sup>
ICE<sup>3</sup> es la mezcla definitiva entre arcade, estrategia, hielo y peleas, en el que deberás hacer uso de tu habilidad para encontrar a otros personajes y mandarlos fuera del mapa. Pero no todo es tan sencillo, todo esto ocurre en un cubo, y, por si fuera poco, ¡todo resbala! Para conseguir sobrevivir y ser el último en pie, deberás patinar, golpear y sacar del cubo a todo aquel que se cruce en tu camino.

Para ello, dispondrás de un abanico de objetos que pueden darle la vuelta <strike>a la tortilla</strike> al cubo y proclamarte así ganador.

___
## Descripción Workflow en Git

La metodología que hemos seguido para nuestro proyecto es [Gitflow Workflow](https://www.atlassian.com/git/tutorials/comparing-workflows/gitflow-workflow) diseñada por Atlassian.

Tenemos una rama principal **master** en la cual únicamente publicamos versiones estables y numeradas.

La rama de desarrollo general se denomina **develop** en ella se publican versiones de las cuales se puede llegar a realizar un release a **master**. Desde esta rama nacen ramas individuales para cada desarrollador denominadas **features** y que reciben un nombre descriptivo de la funcionalidad que desarrollan.

Estas **features** a su vez pueden actuar como un segundo develop para una porción del equipo de desarrollo si fuera necesario. Por ejemplo la rama _desarrolloJuegoBuena_ es un develop secundario de la que nacen features en relación al servidor o al cliente, evitando así posibles conflictos en el merge. 

En principio los commits de las **features** también deben ser descriptivos de lo que han desarrollado, si no se llega a la meta intentada en el commit se debe aplicar la etiqueta *WIP* al comienzo del mismo si es posible.

Los **features** se cierran en **develop** y aunque se pueden reutilizar, es mejor práctica crear una rama nueva continuando la numeración del feature que se está desarrollando. Si un **feature** no va a continuar y por lo cual no se cerrará con **develop** se debe realizar un commit de *DISCONTINUED* para dar por terminada a la rama.
___



