# StockHelper — Manual de Usuario

> Versión del documento: 1.0
> Aplicación: StockHelper — gestión de inventario, productos, proveedores y compras.

---

## 1. Introducción

**StockHelper** es una aplicación de escritorio para gestionar el inventario y las
compras de un negocio. Permite administrar insumos (ítems) y sus categorías,
construir productos a partir de esos insumos, controlar el stock, gestionar
proveedores, generar órdenes de reposición y de compra, ver reportes y
administrar usuarios y permisos.

El acceso a cada módulo depende de los **permisos** del usuario, definidos por su
**rol**. Un usuario solo ve en el menú las secciones para las que tiene permiso.

---

## 2. Primer inicio de sesión

Al abrir la aplicación aparece la pantalla de **inicio de sesión**.

1. Ingresá el **usuario** y la **contraseña**.
   - En una instalación nueva, el usuario administrador inicial es:
     - **Usuario:** `admin`
     - **Contraseña:** `admin`
2. Podés usar el botón de mostrar/ocultar contraseña para verificar lo que escribís.
3. Presioná **Iniciar sesión**.

> ⚠️ **Cambiá la contraseña de `admin` apenas ingreses** (ver §5, módulo de
> Usuarios). La contraseña por defecto es solo para el primer acceso.

Si las credenciales son incorrectas o el usuario está inactivo, la aplicación
muestra un mensaje de error y no permite el ingreso.

---

## 3. La ventana principal

Tras iniciar sesión se abre la **ventana principal**, compuesta por:

- Una **barra de menú** en la parte superior, organizada en grupos.
- Un **área de contenido** donde se muestra el módulo seleccionado.

### 3.1 Organización del menú

| Grupo de menú | Módulos incluidos |
|---|---|
| **Sistema** | Configuración, Registros (Logs) |
| **Usuarios y permisos** | Usuarios, Permisos y roles |
| **Gestión de catálogo** | Ítems y categorías, Proveedores, Constructor de productos |
| **Inventario y compras** | Gestión de stock, Órdenes de reposición, Órdenes de compra, Analíticas |
| **Cerrar sesión** | Sale de la sesión actual |

> Los grupos y módulos que **no** correspondan a tus permisos **no aparecen** en el
> menú. Si intentás abrir un módulo sin permiso, la aplicación muestra
> "Acceso denegado".

### 3.2 Cambiar el idioma

El idioma se cambia desde **Sistema → Configuración** (disponible para todos los
usuarios). Los idiomas disponibles son **Español (es-ES)** e **Inglés (en-EN)**.
El cambio se aplica de inmediato en toda la aplicación y se recuerda para la
próxima vez.

### 3.3 Cerrar sesión

**Cerrar sesión** vuelve a la pantalla de inicio de sesión sin cerrar el
programa, para que ingrese otro usuario.

---

## 4. Módulo Sistema

### 4.1 Configuración

Permite seleccionar el **idioma** de la interfaz. Elegí el idioma en la lista y el
cambio se aplica al instante.

### 4.2 Registros (Logs)

Muestra el **registro de actividad** del sistema (auditoría y eventos). Es útil
para revisar qué operaciones se hicieron y diagnosticar problemas.

- Filtrá por **rango de fechas**.
- Filtrá por **nivel** (Información, Advertencia, Error, etc.).
- Los eventos de auditoría de operaciones aparecen marcados como `[AUDIT]`.

---

## 5. Módulo Usuarios y permisos

### 5.1 Usuarios

Permite administrar las cuentas que pueden ingresar a la aplicación.
Requiere el permiso **UserManagement**.

**Crear un usuario:**
1. Abrí **Usuarios y permisos → Usuarios**.
2. Presioná **Nuevo usuario**.
3. Completá el **nombre de usuario**, la **contraseña** y el **rol**.
4. Marcá si el usuario está **activo**.
5. Guardá.

**Modificar un usuario:** seleccioná el usuario en la lista y presioná
**Modificar**. Podés cambiar el rol, activar/desactivar y restablecer la
contraseña.

> Un usuario **inactivo** no puede iniciar sesión. La contraseña se guarda
> cifrada; nadie (ni el administrador) puede verla, solo restablecerla.

### 5.2 Permisos y roles

Permite administrar los **roles** (conjuntos de permisos) y qué **permisos** tiene
cada rol. Requiere el permiso **PermissionManagement**.

Conceptos:

- **Permiso (Patent):** una autorización concreta para acceder a un módulo
  (por ejemplo, "Gestión de stock").
- **Rol (Family):** un conjunto de permisos (por ejemplo, "Administrator" tiene
  todos los permisos).
- Cada **usuario** tiene un rol, y hereda los permisos de ese rol.

**Crear un rol:**
1. Abrí **Permisos y roles**.
2. Presioná **Nuevo rol**, asigná un **nombre** y una **descripción**.
3. Seleccioná los **permisos** que tendrá el rol.
4. Guardá.

**Modificar / eliminar un rol:** seleccioná el rol y usá **Modificar** o
**Eliminar**. Un rol **no se puede eliminar** si hay usuarios que lo están usando.

**Permisos disponibles:**

| Permiso | Da acceso a |
|---|---|
| UserManagement | Gestión de usuarios |
| PermissionManagement | Gestión de permisos y roles |
| ItemCategoryManagment | Ítems y categorías |
| SupplierManagment | Proveedores |
| ProductBuilder | Constructor de productos |
| StockManagment | Gestión de stock |
| PurchaseManagement | Órdenes de reposición y de compra |
| Analytics | Reportes y analíticas |
| SystemLogs | Registros (logs) del sistema |
| SystemConfiguration | Configuración del sistema |

---

## 6. Módulo Gestión de catálogo

### 6.1 Ítems y categorías

Administra los **insumos** (ítems) y sus **categorías**. Requiere el permiso
**ItemCategoryManagment**.

- **Categorías:** agrupan ítems (por ejemplo, "Bebidas", "Limpieza"). Podés crear
  y eliminar categorías.
- **Ítems:** cada insumo tiene **nombre**, **unidad de medida**, **categoría** y
  **stock actual**. La unidad puede ser entera (por unidades) o decimal
  (por peso/volumen).

**Crear un ítem:**
1. Abrí **Ítems y categorías**.
2. Presioná **Nuevo ítem**.
3. Completá nombre, unidad, categoría y stock inicial.
4. Guardá.

### 6.2 Proveedores

Administra los **proveedores**. Requiere el permiso **SupplierManagment**.

Cada proveedor tiene: **CUIT** (11 dígitos numéricos), **nombre**,
**razón social**, **categoría de ítems que provee**, **teléfono de contacto** y
**email**.

**Crear un proveedor:**
1. Abrí **Proveedores → Nuevo proveedor**.
2. Completá los datos. El **CUIT** debe tener exactamente **11 dígitos**.
3. Guardá.

> 🔒 Por seguridad, el **CUIT, teléfono y email** de los proveedores se almacenan
> **cifrados**. Se muestran normalmente en la aplicación, pero no son legibles
> directamente en la base de datos.

Un proveedor **no se puede eliminar** si tiene órdenes de compra activas asociadas.

### 6.3 Constructor de productos

Permite definir **productos** compuestos por varios ítems (una "receta" o lista de
materiales). Requiere el permiso **ProductBuilder**.

Cada producto tiene un **nombre**, un **código** y una lista de
**ítems con la cantidad a consumir** de cada uno.

**Crear un producto:**
1. Abrí **Constructor de productos → Nuevo producto**.
2. Ingresá nombre y código.
3. Agregá los ítems que lo componen y la **cantidad a consumir** de cada uno.
4. Guardá.

> Esta "receta" se usa luego para descontar stock automáticamente cuando se
> registra el consumo (ver §7.1, importación de cierre de turno).

---

## 7. Módulo Inventario y compras

### 7.1 Gestión de stock

Permite consultar y ajustar el **stock** de los ítems. Requiere el permiso
**StockManagment**.

- Consultá el stock actual de cada ítem.
- Identificá ítems con **stock bajo** o **sin stock**.
- **Importación de cierre de turno:** podés cargar un archivo de reporte de ventas
  del cierre de turno; la aplicación identifica los productos vendidos por su
  código y nombre, y **descuenta automáticamente** del stock los ítems consumidos
  según la receta de cada producto.

**Importar un cierre de turno:**
1. Abrí **Gestión de stock**.
2. Elegí **Importar cierre de turno** y seleccioná el archivo de reporte.
3. Revisá el consumo calculado y confirmá para descontar el stock.

### 7.2 Órdenes de reposición

Permite crear **órdenes de reposición** dirigidas a un proveedor, con los ítems y
cantidades a reponer. Requiere el permiso **PurchaseManagement**.

**Crear una orden de reposición:**
1. Abrí **Órdenes de reposición → Nueva**.
2. Elegí el **proveedor**.
3. Agregá los **ítems** y las **cantidades** a pedir.
4. Guardá. Se genera un **número de orden** automático
   (formato `REP-añoMes-...-NNN`).

**Notificar al proveedor:** desde la orden podés enviar la solicitud al proveedor
por **WhatsApp** (se abre WhatsApp con el mensaje prearmado hacia el teléfono del
proveedor).

### 7.3 Órdenes de compra

Gestiona las **órdenes de compra** derivadas de las reposiciones. Requiere el
permiso **PurchaseManagement**.

Estados posibles de una orden de compra:

| Estado | Significado |
|---|---|
| Enviada al proveedor | La orden fue enviada y se espera la entrega/factura |
| Factura recibida | Se recibió la mercadería y la factura |
| Cancelada | La orden fue anulada |

**Recibir una orden (registrar la entrega):**
1. Seleccioná la orden en estado "Enviada al proveedor".
2. Elegí **Recibir** / **Cargar factura**.
3. Adjuntá el **archivo de la factura** e ingresá el **monto total**.
4. Confirmá. La orden pasa a "Factura recibida" y **el stock de los ítems se
   incrementa automáticamente** con las cantidades recibidas.

**Cancelar una orden:** seleccioná la orden y usá **Cancelar** (indicá el motivo
si se solicita).

### 7.4 Analíticas

Muestra **reportes** de compras. Requiere el permiso **Analytics**.

- **Por categoría:** total y gasto de compras agrupado por categoría de ítems, con
  su porcentaje.
- **Por proveedor:** total y gasto de compras agrupado por proveedor, con su
  porcentaje.
- Se puede filtrar por **rango de fechas**.
- Los reportes pueden **enviarse por email** (se abre el cliente de correo
  predeterminado con el contenido prearmado).

> Las órdenes **canceladas** no se incluyen en los totales de los reportes.

---

## 8. Respaldos automáticos

La aplicación realiza un **respaldo automático diario** de la base de datos. El
respaldo se dispara al iniciar sesión, como máximo una vez por día, y se guarda en
la carpeta de respaldos configurada. Los respaldos antiguos se eliminan según los
días de retención configurados (por defecto, 14 días). Este proceso es automático
y no requiere acción del usuario.

---

## 9. Preguntas frecuentes

**No veo un módulo en el menú.**
No tenés el permiso correspondiente. Pedile a un administrador que ajuste tu rol
en **Permisos y roles**.

**Olvidé mi contraseña.**
Un usuario con permiso de gestión de usuarios puede **restablecerla** desde el
módulo Usuarios. Las contraseñas no se pueden recuperar, solo restablecer.

**¿Puedo cambiar el idioma?**
Sí, desde **Sistema → Configuración**. Está disponible para todos los usuarios.

**El CUIT no me deja guardarlo.**
Debe tener exactamente **11 dígitos numéricos**, sin puntos ni guiones.

**No puedo eliminar un proveedor / un rol.**
No se puede eliminar un proveedor con órdenes de compra activas, ni un rol que esté
asignado a algún usuario. Resolvé esa dependencia primero.

---

*Fin del Manual de Usuario.*
