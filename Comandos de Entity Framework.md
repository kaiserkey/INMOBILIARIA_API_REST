# Comandos de Entity Framework
## Depuracion y ejecucion de aplicaciones
- dotnet build -> compila el proyecto para revisar errores
-dotnet run -> ejecuta la aplicacion actual

## Crear aplicaciones desde la terminal 
- dotnet new console -n Name -> crea una nueva aplicacion de consola 
- dotnet new classlib -n Name -> crea una nueva libreria de clases
- dotnet new sln -n Name -> crea runa nueva solucion 
- dotnet sln add Project_Folder -> agregar carpetas a la solucion creada

## Agregar paquetes de nuget por la terminal
- dotnet list package -> lista de paquetes instalados
- dotnet add package Name -> agrega el paquete seleccionado al proyecto actual

## Paquetes necesarios para trabajar con la base de datos
- dotnet add package Pomelo.EntityFrameworkCore.MySql
- dotnet add package Microsoft.EntityFrameworkCore.Design

## Migraciones
- dotnet tool install dotnet-ef --global -> instalar la herramienta de migraciones de .net
- dotnet ef migrations list -> lista de migraciones pendientes
- dotnet ef migrations add Name_Migration -> crea una nueva migracion para el proyecto actual
- dotnet ef database update -> agrega las migraciones a la base de datos
- dotnet ef database update Name_Migration -> regresa los cambios hasta una migracion especifica
- dotnet ef migrations script -i -> muestra el script que realiza al crear las tablas de la base de datos y todo lo relacionado al modelo de datos
- dotnet ef migrations script -i --output miDB.sql -> guarda el script de instalacion de la base de datos 

## Ingeniería inversa de una base de datos
- dotnet ef dbcontext scaffold "Server=127.0.0.1;Port=3307;Database=wpm;Uid=root;Pwd=kaiserkey;" Pomelo.EntityFrameworkCore.MySql -> se conecta a la base de datos y hace ingenieria inversa de la misma

## CADENA DE CONEXION
`
"ConnectionStrings": {
  "MySql": "Server=127.0.0.1;Port=3307;Database=APIRESTPRACTICAS;Uid=root;Pwd=kaiserkey;"
}
`
