# Pendientes manuales (EA y Word) - 05/10/2026

Lo que el conector de Enterprise Architect no puede hacer (borrar y dibujar fragmentos) y lo que falta en el Word. Antes de empezar: cerrá y volvé a abrir el proyecto .EAP para que se refresquen los mensajes y diagramas.

## 1. Enterprise Architect: borrar
- VENTA_BLL: borrar operación ZZ_SINUSO_BORRAR_VerificarStockBajoUmbral (op 334)
- VENTA_BLL.VerificarDisponibilidadInsumos: borrar parámetro ZZ_BORRAR_detalles
- VENTA_BLL.CalcularMontoTotal: borrar parámetro ZZ_BORRAR_detalles
- VENTA_BLL.RegistrarVenta: borrar parámetros ZZ_BORRAR_medioPago y ZZ_BORRAR_monto
- INSUMO_BLL.IngresarStock: borrar parámetro ZZ_BORRAR_costoUnidadCompra
- RECEPCION_BLL.ValidarRecepcion: borrar parámetro ZZ_BORRAR_orden
- PAGO_PROVEEDOR_BLL.ValidarPago: borrar parámetro ZZ_BORRAR_total
- Paquete "Capas" (T00): borrar la clase ZZ_BORRAR_FormatoMoneda (quedó con estereotipo Java::static; la buena es FormatoMoneda)

## Diagramas General (rehechos)
- Nuevos: 153 "N01 - General" (paq N01/DiagramaClases) y 154 "N02 - General" (paq N02/Clases), con las clases fusionadas de los paquetes N01_General y N02_General.
- Borrar los diagramas OLD_N01 - General (78) y OLD_N02 - General (114).
- Borrar las clases ZZ_BORRAR_ de los paquetes DiagramaClases de N01 (566-575, 632) y Clases de N02 (708-716). Solo las usaban los diagramas OLD_.

- Diagramas de secuencia viejos renombrados con OLD_ (95-100, 45, 47 y 115-120): borrarlos cuando revises los nuevos.

## 2. Enterprise Architect: fragmentos de los DSS nuevos

El MCP de EA no crea fragmentos (alt/opt/loop/critical). En cada DSS nuevo, dibujá estos fragmentos sobre los mensajes indicados (los números son el orden del mensaje en el diagrama, empezando en 1). Dentro de cada `alt` va solo el camino del flujo alternativo; el camino bueno sigue afuera.

## DSS-CU006-InsertarProducto (diagrama 131)
- `alt` FA3 - Piqueo: solo el tamaño Único / Bebida: todos menos Único: mensajes 11 a 11
- `alt` FA1 - Datos incompletos o inválidos: mensajes 16 a 17
- `critical` Una sola transacción de ACCESO (decisión 27): mensajes 18 a 38
- `alt` FA2 - Producto ya existente: mensajes 21 a 25
- `loop` Por cada tamaño marcado: mensajes 34 a 37

## DSS-CU008-ModificarProducto (diagrama 132)
- `alt` FA1 - Datos incompletos o inválidos: mensajes 23 a 24
- `critical` Una sola transacción de ACCESO (decisión 27): mensajes 31 a 49
- `alt` FA2 - Nombre ya existente / tamaño con ventas: mensajes 34 a 38
- `loop` Por cada tamaño quitado: mensajes 41 a 44
- `loop` Por cada tamaño marcado: mensajes 45 a 48

## DSS-CU010-RegistrarVenta (diagrama 133)
- `loop` Por cada línea del pedido: mensajes 13 a 18
- `loop` Por cada insumo (consumos sumados): mensajes 19 a 24
- `alt` FA1 - Insumo insuficiente: mensajes 25 a 26
- `critical` Una sola transacción de ACCESO (decisión 27): mensajes 33 a 72
- `loop` Segunda verificación de stock, por cada insumo: mensajes 34 a 39
- `alt` FA3 - Stock insuficiente al confirmar el cobro: mensajes 40 a 42
- `opt` FA2 - Insumo bajo el umbral y sin aviso pendiente (decisión 47): mensajes 50 a 54
- `opt` El pedido incluye piqueos: mensajes 66 a 71

## DSS-CU011-ConfirmarEntregaPiqueos (diagrama 130)
- `alt` FA1 - Comanda ya confirmada: mensajes 23 a 24

## DSS-CU012-ConfirmarEntregaBebidas (diagrama 134)
- `alt` FA1 - Vale inválido o ya utilizado: mensajes 7 a 8
- `alt` FA2 - Vale sin bebidas (no se marca el vale): mensajes 10 a 10
- `loop` Por cada trago del vale: mensajes 11 a 16

## DSS-CU013-InsertarRecetas (diagrama 135)
- `alt` FA1 - Sin productos pendientes: mensajes 8 a 8
- `alt` FA3 - Cancelación: mensajes 18 a 19
- `alt` FA2 - Receta inválida: mensajes 22 a 23
- `loop` Por cada insumo de la receta: mensajes 30 a 33

## DSS-CU014-ModificarRecetas (diagrama 136)
- `alt` FA2 - Cancelación: mensajes 23 a 24
- `alt` FA1 - Receta inválida: mensajes 27 a 28
- `loop` Por cada insumo de la receta: mensajes 35 a 38
- `critical` Transacción: borra las líneas y las vuelve a guardar: mensajes 40 a 45

## DSS-CU015-EmitirFactura (diagrama 137)
- `alt` FA1 - Datos del cliente inválidos: mensajes 4 a 5
- `critical` Transacción propia, posterior a la venta (decisión 42): mensajes 8 a 22
- `alt` FA2 - Error al registrar la factura: mensajes 15 a 19

## DSS-CU016-GenerarOrdenCompra (diagrama 140)
- `alt` FA1 - Sin avisos pendientes: mensajes 8 a 14
- `alt` FA2 - Proveedor no registrado: mensajes 22 a 23
- `alt` FA3 - Orden inválida (sin insumos, cantidad <= 0, repetidos, insumo que el proveedor no ofrece o proveedor inactivo): mensajes 37 a 38
- `critical` Una sola transacción de ACCESO (decisión 27): mensajes 39 a 50
- `loop` Por cada insumo con aviso pendiente: mensajes 45 a 48

## DSS-CU017-AprobarOrdenCompra (diagrama 141)
- `alt` FA1 - Sin órdenes pendientes: mensajes 8 a 8
- `alt` FA2 - Devolución con observaciones: mensajes 18 a 27
- `alt` FA3 - Observaciones vacias: mensajes 20 a 21
- `alt` La orden ya no está Pendiente de aprobación: mensajes 30 a 31

## DSS-CU018-AjustarOrdenObservada (diagrama 142)
- `alt` FA1 - Sin órdenes observadas: mensajes 8 a 8
- `alt` FA2 - Orden inválida: mensajes 27 a 28
- `critical` Una sola transacción de ACCESO (decisión 27): mensajes 29 a 49
- `loop` Por cada insumo que sigue en la orden: mensajes 40 a 43

## DSS-CU019-RegistrarRecepcion (diagrama 143)
- `alt` FA1 - Sin órdenes para recibir: mensajes 8 a 8
- `alt` FA2 - Datos de recepción inválidos: mensajes 21 a 22
- `critical` Una sola transacción de ACCESO (decisión 27): mensajes 23 a 63
- `loop` Por cada insumo recibido: mensajes 29 a 44
- `opt` Stock >= umbral (decisión 59): mensajes 41 a 44
- `loop` Por cada insumo recibido: mensajes 47 a 50
- `opt` La orden quedó Cerrada (decisión 60): mensajes 57 a 62
- `alt` FA3 - Recepción incompleta: mensajes 65 a 65

## DSS-CU020-RegistrarReclamo (diagrama 144)
- `alt` FA1 - Motivo faltante: mensajes 15 a 16

## DSS-CU021-RegistrarPagoProveedor (diagrama 145)
- `loop` Por cada orden cerrada: mensajes 8 a 9
- `alt` FA1 - Sin órdenes pendientes de pago: mensajes 10 a 10
- `alt` FA2 - Datos de pago inválidos: mensajes 31 a 32
- `critical` Una sola transacción de ACCESO (decisión 27): mensajes 33 a 44

## DSS-CU022-InsertarProveedor (diagrama 146)
- `alt` FA1 - Datos incompletos o inválidos: mensajes 11 a 12
- `alt` FA2 - Proveedor ya existente (CUIT repetido): mensajes 15 a 18

## DSS-CU023-InsertarInsumo (diagrama 149)
- `alt` FA1 - Datos incompletos o inválidos: mensajes 9 a 10
- `alt` FA2 - Insumo ya existente: mensajes 13 a 16

## DSS-CU024-ModificarInsumo (diagrama 150)
- `alt` FA1 - Datos incompletos o inválidos: mensajes 13 a 14
- `alt` FA2 - Nombre ya existente: mensajes 17 a 20

## DSS-CU025-DarDeBajaInsumo (diagrama 151)
- `alt` FA1 - Insumo usado en recetas: mensajes 8 a 8
- `alt` FA2 - Insumo con órdenes de compra abiertas: mensajes 15 a 15
- `alt` FA3 - Baja cancelada: mensajes 17 a 18

## DSS-CU026-ModificarProveedor (diagrama 147)
- `alt` FA3 - Insumo en una orden de compra abierta: mensajes 30 a 30
- `alt` FA1 - Datos incompletos o inválidos: mensajes 32 a 33
- `alt` FA2 - CUIT ya existente: mensajes 36 a 39

## DSS-CU027-DarDeBajaProveedor (diagrama 148)
- `alt` FA1 - Proveedor con órdenes de compra abiertas: mensajes 8 a 8
- `alt` FA2 - Baja cancelada: mensajes 10 a 11

## DSS-CU028-ConsultarVentas (diagrama 138)
- `alt` FA1 - Sin ventas en el periodo: mensajes 10 a 10
- `opt` El usuario pide ver la factura: mensajes 20 a 23

## DSS-CU029-ConsultarVales (diagrama 139)
- `alt` FA1 - Sin vales en el periodo: mensajes 10 a 10

## DSS-CU030-ConsultarCompras (diagrama 152)
- `alt` FA1 - Sin órdenes para los filtros: mensajes 18 a 18

## 3. Enterprise Architect: layout
- Reacomodar a mano los diagramas nuevos (DSS 130-152, N01 - General 153, N02 - General 154, BE de N01 y N02) si algún conector queda cruzado.

## 4. Word (archivo `..._control_cambios.docx`)
- Revisá y aceptá o rechazá los cambios (autor "Claude").
- Pegar las imágenes nuevas exportadas desde EA:
  - G07 (BE, BLL, DAL) y G08 (DER general).
  - N01: diagramas de secuencias de CU006, CU008, CU010 a CU015, CU023 a CU025, CU028 y CU029; E (N01 - General) y F (DER de N01).
  - N02: diagramas de secuencias de CU016 a CU022, CU026, CU027 y CU030; E (N02 - General) y F (DER de N02).
  - T01.6, T01.7 (BE, BLL, DAL, SERVICIOS con FormatoMoneda y GeneradorPdf) y T01.8.
  - Casos de uso de N01 y N02 (actores Vendedor, Cocinero y Dueño; CU028 a CU030).
- Actualizar el índice (clic derecho > Actualizar campos): cambió el título de 6.6 (CU007 Dar de baja producto) y hay CU nuevos.
- T01.3: la tabla de roles todavía muestra "Cliente" y "Vendedor" con los accesos de T00. No estaba en la lista de cambios, por eso no la toqué.
- Credenciales de prueba: saqué las contraseñas de T04 (quedaron como cambio rechazable). El documento aparte de credenciales sigue pendiente de tu decisión.
- `docs/DESVIOS.md`: poner "Documentación actualizada = Sí" en las filas corregidas (dejalo para Claude Code).
