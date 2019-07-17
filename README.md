###### v0.1
#Duo Dinamita 🧨


##Estándar de código para desarrollo de videojuegos

###Indice
1. [Objetivo]()
2. [Control de versión del documento]()
3. [Estándar de código](#3-estándar-de-código)
	1. [Indentación]()
	2. [Espacios]()
	3. [Llaves y corchetes]()
	4. [Largo y salto de linea]()
		1. [Condiciones IF]()
		2. [Loops]()
	5. [Convención de Nombres]()
		1. [Variables, Clases y Métodos]()
		2. [Genéricos]()
		3. [Clases y Métodos de Test]()
	6. [Namespace]()
	7. [Dependencias]()
	8. [Comentarios]()
4. [Buenas practicas]()
	1. [Patrones de Diseño]()
	2. [Lectura del Código]()
	3. [Reutilización de Código]()
	4. [Optimización]()
	5. [Helpers]()
	6. [Testing]()
		1. [Métodos de Prueba]()
		2. [Revisión de nueva implementación]()
5. [Gestion de proyectos]()
	1. [Almacenamientos de proyectos]()
	2. [Repositorio]()
	3. [Versionado]()
	4. [Documentación]()
6. [Manejo de Assets]()
	1. [Estándar de Nombres]()
	2. [Tamaños y requerimientos]()
7. [Sitios Útiles]()



## 1. Objetivo
Definir un estandar nos facilita una comprecion y define clamaramnte las intenicones que tiene el programador al redactar su código. Dando una mejor capacidad de mantenimiento, optimización de tiempos y posibilidad de escalar los programas.


## 2. Control de version del documento
Ver|Editor|Descripción de las modificaciones|Fecha
:---|:---|:---|:---|:---
0.1|Darío Ciarlantini|alfa|29/01/2018


## 3. Estandar de Código

### 3.1 Indentación

Todo el código debe estar correctamente indentado. Los tabs deben ser usados como unidad de indentación y deben estar configurados en nuestros editores como 4 espacios. A su vez todo comentario debe estar indentado en línea con los bloques de código a los que se refiere.

Esto nos permite identificar rápidamente bloques de código.


### 3.2 Espacios

Se recomienda utilizar un espacio para separar todos los operadores (excepto los de incremento/decrecimiento ej: i++; i--;), variables, literales, palabras claves, comas y punto y coma.

No se debe utilizar espacios luego de abrir un paréntesis o cerrarlo.

(agregar ejemplo)

### 3.3 Llaves y corchetes
(rev)

### 3.4 Largo y salto de linea
(rev)

#### 3.4.1 Condiciones IF
(rev)

#### 3.4.2 Loops
(rev)


### 3.5 Convención de Nombres
(rev)

#### 3.5.1 Variables, Clases y Métodos
(rev)

#### 3.5.2 Genéricos (rev)

Forma de nombrarlo:
El nombre de la clase o método tiene que ser auto explicable.

Implementación:

Sí tiene más de doce caracteres o más de un parámetro tiene que crear una instancia a travez de una variable VAR para mejorar la lectura y comprensión del código.


### 3.6 Namespace

Cada proyecto debe tener un Namspace asignado.


### 3.7 Dependencias

Las dependencias recomendadas por defecto son las utilizadas por Unity en su pagina oficial. Estas dependerán de la versión del editor que se este utilizando para seguir las escritas por el manual. 

En el caso de temer una dependencia a una DLLo una implementación temporal o de prueba. Se requerirá una dependencia personalizada para la plataforma a la que se le quiera incorporar.


### 3.8 Comentarios
(rev)


## 4. Buenas Practicas

### 4.1 Patrones de Diseño
(rev)


### 4.2 Lectura del Código
(rev)


### 4.3 Reutilización de Código
(rev)


### 4.4 Optimización
(rev)


### 4.5 Helpers
(rev)


### 4.6 Testing
(rev)

#### 4.6.1 Métodos de Prueba
(rev)

#### 4.6.2 Revisión de nueva implementación
(rev)


## 5. Gestión de Proyecto

### 5.1 Almacenamientos de proyectos
(rev)


### 5.2 Repositorio
(rev)


### 5.3 Versionado

El código de versión a Mostar debera contaner los siguientes componentes en el orden pactado separador por puntos:

Número de versión principal: Esta dependerá de las factoizaciones generales del programa. Principalmente definirá si un proyecto se encuentra en BETA con un “0” (cero) asignado o un “1” (uno) si el proyecto se encuentra con todas sus características.
Número de implementación o cambio: Muestra el número progresivo de las modificaciones planteadas al juego. Por cada modificación que luego vaya a ser testeada se recomienda el cambio de número. En el caso de realizar una build en un estado de prueba, tiene que estar acompañada de “b” informando el estado beta de la implementación.
Cinco números generados por la GUID: Estos números generados automáticamente determina si una build realizada es idéntica a otra realizada.

Ejemplos:
0.10.aaaaa
1.5b.bbbbb
1.40.ccccc

El código de versión tiene que mostrarse en pantalla,. Esta no siempre tiene que ser visible por el usuario, pero tiene que poder ser localizada fácilmente por el equipo de desarrollo.


### 5.4 Documentación

## 6. Manejo de Assets

### 6.1 Estándar de Nombres
(rev)


### 6.2 Tamaños y requerimientos
(rev)

## 7. Sitios Útiles
(rev)