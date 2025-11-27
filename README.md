ProductInventoryApp 
Luisander Arroyo Rivera larroyo6794@interbayamon.edu

Descripción

ProductInventoryApp es una aplicación de consola (CLI) en C# para manejo seguro de inventario de productos. Implementa autenticación de usuarios, CRUD de productos, control de acceso por roles y registro de auditoría, siguiendo principios de Secure by Design y buenas prácticas de programación segura.

Credenciales de prueba
Administrador
Usuario: admin
Contraseña: admin123
Rol: admin (puede crear, actualizar, eliminar y ver productos)

Usuario estándar
Puede registrarse con cualquier nombre de usuario y contraseña, solo tiene acceso de lectura a los productos.

Nota: Al ejecutar por primera vez, la aplicación crea automáticamente el usuario administrador (admin/admin123) si no existe.

Requisitos

.NET 6 o superior instalado
Sistema operativo: Windows, macOS o Linux

Ejecución

Clone el repositorio:
git clone https://github.com/luisander1830/ProductInventoryApp.git

Ingrese a la carpeta del proyecto:
cd ProductInventoryApp

Ejecute el proyecto:
dotnet run

Use las credenciales del administrador (admin/admin123) o regístrese como usuario estándar.


Estructura del proyecto
Program.cs: Punto de entrada del programa

Auth/: Manejo de autenticación y usuarios

ProductService.cs: CRUD y lógica de productos

Logging/: Registro de auditoría (audit.log)

data/: Almacena usuarios (users.json) y productos (products.json)

Funcionalidades principales

Registro e inicio de sesión de usuarios

CRUD de productos con control de acceso por rol

Hashing seguro de contraseñas (SHA256)

Validación de entradas para evitar errores o ataques

Logging de acciones y cambios importantes

Observaciones

No se usa base de datos, toda la persistencia es en archivos JSON.

El archivo audit.log registra todas las acciones de usuarios.

El usuario admin ya viene preconfigurado para pruebas de entrega.
