###### v0.1
# Duo Dinamita 🧨

### Estándar de código para desarrollo de videojuegos

### Indice
1. [Objetivo](#1-objetivo)
2. [Control de versión del documento](#2-control-de-versión-del-documento)
3. [Estándar de código](#3-estándar-de-código)
	1. [Indentación](#31-indentación)
	2. [Espacios](#32-espacios)
	3. [Llaves y corchetes](#33-llaves-y-corchetes)
	4. [Largo y salto de linea](#34-largo-y-salto-de-linea)
		1. [Condiciones IF](#341-condiciones-if)
		2. [Loops](#342-loops)
		3. [Genéricos](#343-genéricos)
	5. [Convención de Nombres](#35-convención-de-nombres)
		1. [Variables, Clases y Métodos](#351-Variables,-clases-y-métodos)
		2. [Clases y Métodos de Test](#352-clases-y-métodos-de-test)
	6. [Namespace](#36-namespace)
	7. [Dependencias](#37-dependencias)
	8. [Comentarios](#38-comentarios)
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
```JAVA
for (int i = 0; i < 10; i++)
{
	for (int j = 0; j < 10; j++)
	{
		System.debug(i + j);
	}
}
```


- Incorrecto
```JAVA
for (int i = 0; i < 10; i++) {
	for (int j = 0; j < 10; j++) {
		System.debug(i + j);
	}
}
```

- Incorrecto
```JAVA
for (int i = 0; i < 10; i++)
	for (int j = 0; j < 10; j++)
		System.debug(i + j);
```

### 3.4 Largo y salto de linea
Se tiene que evitar las líneas que se extiendan a más de 100 caracteres, y bajo ninguna sircustancia deben pasar los 120 caracteres. En caso de suceder esto se deberá insertar saltos de líneas. Esto permitirá una mejor lectura y comprención del código.

Los saltos de líneas se deben agregar en determinados contextos:
- Despues de una coma.
- Ddespues de un operador.
- Se prefiere insertar los saltos de línea en niveles superiores a nivel bloque de código.


**Ejemplos**
-  Luego de una coma
	- VALIDO
	```JAVA
	someMethod(longExpression1, longExpression2, longExpression3,
		longExpression4, longExpression5);
	```
	- PREFERIDO
	```JAVA
	someMethod(
		longExpression1,
		longExpression2,
		longExpression3,
		longExpression4,
		longExpression5
	);
	```
- Luego de un operador
	- SI
	```JAVA
	longName1 = longName2 * longName3 + longName4 – longName5 +
		4 * longname6;
	```

- Alto nivel antes que bajo nivel
<br/><i>(\*) nótese que el salto de línea no se produce dentro de los paréntesis</i>
	- SI
	```JAVA
	longName1 = longName2 * (longName3 + longName4 – longName5) +
		4 * longname6;
	```
	- NO
	```JAVA
	longName1 = longName2 * (longName3 + longName4 -
		longName5) + 4 * longname6;
	```
#### 3.4.1 Condiciones IF
Para el caso de condiciones IF extensas, se deben insertar los saltos de línea bajo los mismos criterios, teniendo cuidado en indentar correctamente cada porcion de codigo.
- NO
```JAVA
if ((condition1 && condition2)
|| (condition3 && condition4)
||!(condition5 && condition6)) {
	doSomethingAboutIt();
}
```
- PREFERIDO
```JAVA
if ((condition1 && condition2)
	|| (condition3 && condition4)
	|| !(condition5 && condition6)
) {
	doSomethingAboutIt ();
}
```
- VALIDO
```JAVA
public void usingCondition() {
	if (myLongCondition()) {
		doSomethingAboutIt ();
	}
}
[...]
public Boolean myLongCondition() {
	return (condition1 && condition2)
		|| (condition3 && condition4)
		|| !(condition5 && condition6);
}
```

#### 3.4.2 Loops
(rev)

#### 3.4.3 Genéricos
Sí tiene más de doce caracteres o más de un parámetro tiene que crear una instancia a travez de una variable VAR para mejorar la lectura y comprensión del código.


### 3.5 Convención de Nombres
Una de las claves principales para ubna lectura clara del código es elegir correctamente el nombre de cada elemento y que pueda auto-describirse. Puede que debore eligiendo el nombre correcto, pero al pasar el tiempo y volver al proyecto se dara un apalmada en el hobro por ese trabajo.

#### 3.5.1 Variables, Clases y Métodos
Variables
Las variables locales, de instancia y de clases deben escribirse en formato CamelCase (iniciando en minuscula). No se recomienda el uso de guión bajo para marcarlas. Tampoco deben tener algun otro tipo de signo. Solo pueden ser escritar usando texto.
Los nombres tiene que ser cortos y precisos. Tiene que descirbir lo que contenga. Bajo ninguna sircustancia se deberan usar nombres de una sola letra o poco descriptivos. Evitar usar diminutivos.

Constantes
Los nombres de las constantes se deben escribir en mayúsculas separadas por guiones bajos. Los nombres de constantes pueden contener dígitos, pero no como el primer carácter.

Clases
Los nombres de clase deben ser sustantivos en CamelCase iniciando con mayuscula. El nombre tiene que ser auto-descriptivo y no debe usar signos. Evitar usar diminutivos.

Métodos
Los métodos deben ser verbos en CamelCase Iniciando con mayuscula. Esta tiene que describir la funcion que tiene que cumplir el método.


### 3.6 Namespace

Cada proyecto debe tener un Namspace asignado.


### 3.7 Dependencias

Las dependencias recomendadas por defecto son las utilizadas por Unity en su pagina oficial. Estas dependerán de la versión del editor que se este utilizando para seguir las escritas por el manual. 

En el caso de temer una dependencia a una DLLo una implementación temporal o de prueba. Se requerirá una dependencia personalizada para la plataforma a la que se le quiera incorporar.


### 3.8 Comentarios
No se recomienda bajo ninguna circustancia el uso de comentarios. Estos entorpesen y ensucian el código, dificulta el mantenimiento y puede que incluso no sean correctos. Solo se deberian utilizar en el caso de agregar claridad al funcionamiento o detalles pocos claros del código. Esto quiere decir, que expliquen alguna decisión en la construcción del código o alguna operación compleja.

No se deben utilizar para explicar cada línea de código, esto se debe producir por un correcto nombramiento de variables, métodos.

(rev)

- ACEPTABLE
```JAVA
public AccountEditExtension(ApexPages.StandardController stdController) {
	/* Ask if we’re running a test method,
	* we can’t call .addFields() in a test context. */
	if (!Test.isRunningTest()) {
		stdController.addFields(new String[] { ‘Custom_field__c’ });
	}
	account = (Account) stdController.getRecord();
}
```
- NO
```JAVA
public AccountEditExtension(ApexPages.StandardController stdController) {
	if (!Test.isRunningTest()) { // Ask if we’re running a test method, we can’t call .addFields() in a  test context.
		stdController.addFields(new String[] { ‘Custom_field__c’ });
	}
	account = (Account) stdController.getRecord();
}
```


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
* Solución.
* README explicando uso basico de la solución.
* Diagramas UML (Editable + PDF).
* Manual de Uso (Instructivo).
* Instalación / Requerimentos.
* Notas de Uso.


### 5.2 Repositorio
Para repositorios Git el proceos de versionado y guardado de proyecto se divide en tres brachs principales.

**Master:** Siempre debe conservar la última versión estable del software. Etiquetado con su correspondiente código de versión.

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
(rev)

## 6. Manejo de Assets

### 6.1 Estándar de Nombres
(rev)


### 6.2 Tamaños y requerimientos
(rev)

## 7. Sitios Útiles
(rev)

[Microsoft Referencesource](https://referencesource.microsoft.com): Sitio de referencia de Microsoft.
