###### v0.1
# Duo Dinamita 🧨

### Estándar de código para desarrollo de videojuegos

### Indice
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
7. [Sitios Útiles](#7-sitios-útiles)



## 1. Objetivo
Definir un estandar nos facilita una comprecion y define clamaramnte las intenicones que tiene el programador al redactar su código. Dando una mejor capacidad de mantenimiento, optimización de tiempos y posibilidad de escalar los programas.


## 2. Control de version del documento
Ver|Editor|Descripción de las modificaciones|Fecha
:---|:---|:---|:---
0.1|Darío Ciarlantini|alfa|29/01/2018


## 3. Estándar de Código

### 3.1 Indentación

Todo el código debe estar correctamente indentado. Los tabs deben ser usados como unidad de indentación y deben estar configurados en nuestros editores como 4 espacios. A su vez todo comentario debe estar indentado en línea con los bloques de código a los que se refiere.

Esto nos permitira identificar rápidamente bloques de código.


### 3.2 Espacios

Se recomienda utilizar un espacio para separar todos los operadores (excepto los de incremento/decrecimiento ej: i++ / i--), variables, literales, palabras claves, comas y punto y coma.

No se debe utilizar espacios luego de abrir un paréntesis o cerrarlo.

- Correcto
```C#
for (int i = 0; i < 10; i++) {
	System.debug(i);
}
```

- Incorrecto
```C#
for(int i=0;i<10;i++){
	System.debug( i );
}
```

### 3.3 Llaves y corchetes
La apertura y cierre de llave para una declaración tiene que permanecer en la misma linea. Solo en determinados casos donde la declaración se vuelva demaciado grande para el tamaño de la linea, este debera pasar a la siguiente. 

En el caso de los corchetes, este debe abrir siempre debajo de la linea de declaración y deber quedar solo el corchete. En el caso de cierre, despues despues del bloque de código, este cierra un espacio por debajo de la última linea y tiene que estar solo.

Los corchetes en la declaraciones siempre deben aparecer para mejorar la claridad del bloque.

- Correcto
```C#
for (int i = 0; i < 10; i++)
{
	for (int j = 0; j < 10; j++)
	{
		System.debug(i + j);
	}
}
```


- Incorrecto
```C#
for (int i = 0; i < 10; i++) {
	for (int j = 0; j < 10; j++) {
		System.debug(i + j);
	}
}
```

- Incorrecto
```C#
for (int i = 0; i < 10; i++)
	for (int j = 0; j < 10; j++)
		System.debug(i + j);
```

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

#### 3.5.2 Genéricos
(rev)

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
El repositorio debe incluir:
* Solución
* README explicando uso basico de la solución.
* Diagramas UML (Editable + PDF).
* Manual de Uso (Instructivo).
* Instalación / Requerimentos.
* Notas de Uso.


### 5.2 Repositorio
Para repositorios Git el proceos de versionado y guardado de proyecto se divide en tres brachs principales.

**Mastes:** Siempre debe conservar la última versión estable del software. Etiquetado con su correspondiente código de versión.

**Develop:** Branch donde se van a centrar los cambios entre uan versión a otra de la solución. Debe encontrarse en un estado donde se pueda utlilizar el software aunque no se encuentre testeado.

**Brach_de_nueva_caracteristica:** Este branch tiene que contener todos los cambios apliacdos para la nueva caracteristica. El nombre de este tiene que ser igual a la implementación buscada. 

Una vez terminada la nueva implemebtación esta se tiene que mergiar a Develop como un nuevo commit, asi se distingue la ubicación de esta en el gráfico del Git.

Al terminar todas las implementación, factorización y testing. Debe mergearse en Master para entrar en producción. 


### 5.3 Versionado

El código de versión a Mostar debera contaner los siguientes componentes en el orden pactado separador por puntos:

1. Número de versión principal: Esta dependerá de las factoizaciones generales del programa. Principalmente definirá si un proyecto se encuentra en BETA con un “0” (cero) asignado o un “1” (uno) si el proyecto se encuentra con todas sus características.

2. Número de implementación o cambio: Muestra el número progresivo de las modificaciones planteadas al juego. Por cada modificación que luego vaya a ser testeada se recomienda el cambio de número. En el caso de realizar una build en un estado de prueba, tiene que estar acompañada de “b” informando el estado beta de la implementación.

3. Cinco números generados por la GUID: Estos números generados automáticamente determina si una build realizada es idéntica a otra realizada.


Ejemplo|Descripción
:------|:--------
0.10.rn0by|Versión de Beta de la aplicación.
1.5b.cyB1b|Versión en producción con una prueba de una nueva implementación.
1.40.hIHBh|Versión en producción.



El código de versión tiene que mostrarse en pantalla,. Esta no siempre tiene que ser visible por el usuario, pero tiene que poder ser localizada fácilmente por el equipo de desarrollo.


### 5.4 Documentación

## 6. Manejo de Assets

### 6.1 Estándar de Nombres
(rev)


### 6.2 Tamaños y requerimientos
(rev)

## 7. Sitios Útiles
(rev)

[Microsoft Referencesource](https://referencesource.microsoft.com): Sitio de referencia de Microsoft.
