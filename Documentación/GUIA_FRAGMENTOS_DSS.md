# Guía para dibujar los fragmentos de los DSS

Para cada fragmento te digo en qué flecha empieza y en cuál termina, por el texto que muestra la flecha en EA y las lifelines que une (origen → destino). Si el mismo texto aparece varias veces, te doy la flecha anterior para ubicarte. El número es el orden del mensaje, por si querés chequear.

El estado sale del .EAP tal como estaba guardado al armar esta guía (05/10/2026, 12:14).

## DSS-CU006-InsertarProducto (diagrama 131)

Ya están bien: `alt` FA3 - Piqueo: solo el tamaño Único / Bebida: todos menos Único; `alt` FA1 - Datos incompletos o inválidos; `alt` FA2 - Producto ya existente.

Falta o hay que corregir:
- `critical` Una sola transacción de ACCESO (decisión 27):
  - Primera flecha adentro: **18. IniciarTransaccion()** (PRODUCTO_BLL → ACCESO).
  - Última flecha adentro: **38. ConfirmarTransaccion** (PRODUCTO_BLL → ACCESO).
  - La siguiente, "RecalcularDV", ya queda afuera.
- `loop` Por cada tamaño marcado:
  - Primera flecha adentro: **34. GuardarPrecio** (PRODUCTO_BLL → MAPPER_PRODUCTO).
  - Última flecha adentro: **37. Ok** (MAPPER_PRODUCTO → PRODUCTO_BLL), la 4.ª flecha "Ok" del diagrama; viene justo después de "Ok".
  - La siguiente, "ConfirmarTransaccion", ya queda afuera.

## DSS-CU008-ModificarProducto (diagrama 132)

Falta o hay que corregir:
- `alt` FA1 - Datos incompletos o inválidos:
  - Primera flecha adentro: **23. DatosInvalidos** (PRODUCTO_BLL → frmProducto).
  - Última flecha adentro: **24. MuestraDatoACorregir** (frmProducto → Dueño).
  - La siguiente, "CalcularDVH", ya queda afuera.
- `critical` Una sola transacción de ACCESO (decisión 27):
  - Primera flecha adentro: **31. IniciarTransaccion** (PRODUCTO_BLL → ACCESO).
  - Última flecha adentro: **49. ConfirmarTransaccion** (PRODUCTO_BLL → ACCESO).
  - La siguiente, "RegistrarCambio", ya queda afuera.
- `alt` FA2 - Nombre ya existente / tamaño con ventas:
  - Primera flecha adentro: **34. ErrorDeNegocio** (ACCESO → MAPPER_PRODUCTO), la 1.ª flecha "ErrorDeNegocio" del diagrama; viene justo después de "Escribir".
  - Última flecha adentro: **38. MuestraError** (frmProducto → Dueño).
  - La siguiente, "Ok", ya queda afuera.
- `loop` Por cada tamaño quitado:
  - Primera flecha adentro: **41. QuitarPrecio** (PRODUCTO_BLL → MAPPER_PRODUCTO).
  - Última flecha adentro: **44. Ok** (MAPPER_PRODUCTO → PRODUCTO_BLL), la 4.ª flecha "Ok" del diagrama; viene justo después de "Ok".
  - La siguiente, "GuardarPrecio", ya queda afuera.
- `loop` Por cada tamaño marcado:
  - Primera flecha adentro: **45. GuardarPrecio** (PRODUCTO_BLL → MAPPER_PRODUCTO).
  - Última flecha adentro: **48. Ok** (MAPPER_PRODUCTO → PRODUCTO_BLL), la 6.ª flecha "Ok" del diagrama; viene justo después de "Ok".
  - La siguiente, "ConfirmarTransaccion", ya queda afuera.

## DSS-CU010-RegistrarVenta (diagrama 133)

Falta o hay que corregir:
- `loop` Por cada línea del pedido:
  - Primera flecha adentro: **13. ObtenerRecetaEscalada** (VENTA_BLL → RECETA_BLL).
  - Última flecha adentro: **18. RecetaEscalada** (RECETA_BLL → VENTA_BLL).
  - La siguiente, "VerificarDisponibilidad", ya queda afuera.
- `loop` Por cada insumo (consumos sumados):
  - Primera flecha adentro: **19. VerificarDisponibilidad** (VENTA_BLL → INSUMO_BLL), la 1.ª flecha "VerificarDisponibilidad" del diagrama; viene justo después de "RecetaEscalada".
  - Última flecha adentro: **24. Disponible** (INSUMO_BLL → VENTA_BLL), la 1.ª flecha "Disponible" del diagrama; viene justo después de "Insumo".
  - La siguiente, "Faltantes", ya queda afuera.
- `alt` FA1 - Insumo insuficiente:
  - Primera flecha adentro: **25. Faltantes** (VENTA_BLL → frmVenta), la 1.ª flecha "Faltantes" del diagrama; viene justo después de "Disponible".
  - Última flecha adentro: **26. MuestraInsumoFaltanteYProductosAfectados** (frmVenta → Vendedor), la 1.ª flecha "MuestraInsumoFaltanteYProductosAfectados" del diagrama; viene justo después de "Faltantes".
  - La siguiente, "SinFaltantes", ya queda afuera.
- `critical` Una sola transacción de ACCESO (decisión 27):
  - Primera flecha adentro: **33. IniciarTransaccion** (VENTA_BLL → ACCESO).
  - Última flecha adentro: **72. ConfirmarTransaccion** (VENTA_BLL → ACCESO).
  - La siguiente, "VentaRegistrada", ya queda afuera.
- `loop` Segunda verificación de stock, por cada insumo:
  - Primera flecha adentro: **34. VerificarDisponibilidad** (VENTA_BLL → INSUMO_BLL), la 2.ª flecha "VerificarDisponibilidad" del diagrama; viene justo después de "IniciarTransaccion".
  - Última flecha adentro: **39. Disponible** (INSUMO_BLL → VENTA_BLL), la 2.ª flecha "Disponible" del diagrama; viene justo después de "Insumo".
  - La siguiente, "DeshacerTransaccion", ya queda afuera.
- `alt` FA3 - Stock insuficiente al confirmar el cobro:
  - Primera flecha adentro: **40. DeshacerTransaccion** (VENTA_BLL → ACCESO).
  - Última flecha adentro: **42. MuestraInsumoFaltanteYProductosAfectados** (frmVenta → Vendedor), la 2.ª flecha "MuestraInsumoFaltanteYProductosAfectados" del diagrama; viene justo después de "Faltantes".
  - La siguiente, "DescontarStock", ya queda afuera.
- `opt` FA2 - Insumo bajo el umbral y sin aviso pendiente (decisión 47):
  - Primera flecha adentro: **50. GenerarAvisoStockBajo** (INSUMO_BLL → INSUMO_BLL).
  - Última flecha adentro: **54. AvisoGuardado** (MAPPER_INSUMO → INSUMO_BLL).
  - La siguiente, "InsumosBajoUmbral", ya queda afuera.
- `opt` El pedido incluye piqueos:
  - Primera flecha adentro: **66. GenerarComanda** (VENTA_BLL → COMANDA_BLL).
  - Última flecha adentro: **71. Comanda** (COMANDA_BLL → VENTA_BLL), la 2.ª flecha "Comanda" del diagrama; viene justo después de "Comanda".
  - La siguiente, "ConfirmarTransaccion", ya queda afuera.

## DSS-CU011-ConfirmarEntregaPiqueos (diagrama 130)

Falta o hay que corregir:
- `alt` FA1 - Comanda ya confirmada:
  - Primera flecha adentro: **23. ComandaYaConfirmada** (COMANDA_BLL → frmComanda).
  - Última flecha adentro: **24. MuestraLaComandaYaFueConfirmada** (frmComanda → Cocinero).
  - La siguiente, "ActualizarEstado", ya queda afuera.

## DSS-CU012-ConfirmarEntregaBebidas (diagrama 134)

Falta o hay que corregir:
- `alt` FA1 - Vale inválido o ya utilizado:
  - Primera flecha adentro: **7. null** (VALE_BLL → frmEntrega).
  - Última flecha adentro: **8. MuestraValeInvalido** (frmEntrega → Bartender).
  - La siguiente, "ValeConDetalle", ya queda afuera.
- `alt` FA2 - Vale sin bebidas (no se marca el vale): rodea solo la flecha **10. MuestraValeSinBebidas** (frmEntrega → Bartender). La siguiente, "ObtenerRecetaEscalada", ya queda afuera.
- `loop` Por cada trago del vale:
  - Primera flecha adentro: **11. ObtenerRecetaEscalada** (frmEntrega → RECETA_BLL).
  - Última flecha adentro: **16. RecetaEscalada** (RECETA_BLL → frmEntrega).
  - La siguiente, "MuestraBebidasYRecetas", ya queda afuera.

## DSS-CU013-InsertarRecetas (diagrama 135)

Falta o hay que corregir:
- `alt` FA1 - Sin productos pendientes: rodea solo la flecha **8. MuestraNoHayProductosSinReceta** (frmReceta → Cocinero). La siguiente, "MuestraProductosSinReceta", ya queda afuera.
- `alt` FA3 - Cancelación:
  - Primera flecha adentro: **18. CancelarCarga** (Cocinero → frmReceta).
  - Última flecha adentro: **19. VuelveAlListadoDeProductos** (frmReceta → Cocinero).
  - La siguiente, "AgregarInsumosYConfirmar", ya queda afuera.
- `alt` FA2 - Receta inválida:
  - Primera flecha adentro: **22. RecetaInvalida** (RECETA_BLL → frmReceta).
  - Última flecha adentro: **23. MuestraErrorDeReceta** (frmReceta → Cocinero).
  - La siguiente, "RecetaValida", ya queda afuera.
- `loop` Por cada insumo de la receta:
  - Primera flecha adentro: **30. BuscarInsumo** (RECETA_BLL → MAPPER_INSUMO).
  - Última flecha adentro: **33. InsumoActivo** (MAPPER_INSUMO → RECETA_BLL).
  - La siguiente, "GuardarReceta", ya queda afuera.

## DSS-CU014-ModificarRecetas (diagrama 136)

Falta o hay que corregir:
- `alt` FA2 - Cancelación:
  - Primera flecha adentro: **23. CancelarModificacion** (Cocinero → frmReceta).
  - Última flecha adentro: **24. VuelveAlListadoDeProductos** (frmReceta → Cocinero).
  - La siguiente, "ModificarInsumosYConfirmar", ya queda afuera.
- `alt` FA1 - Receta inválida:
  - Primera flecha adentro: **27. RecetaInvalida** (RECETA_BLL → frmReceta).
  - Última flecha adentro: **28. MuestraErrorDeReceta** (frmReceta → Cocinero).
  - La siguiente, "RecetaValida", ya queda afuera.
- `loop` Por cada insumo de la receta:
  - Primera flecha adentro: **35. BuscarInsumo** (RECETA_BLL → MAPPER_INSUMO).
  - Última flecha adentro: **38. InsumoActivo** (MAPPER_INSUMO → RECETA_BLL).
  - La siguiente, "ActualizarReceta", ya queda afuera.
- `critical` Transacción: borra las líneas y las vuelve a guardar:
  - Primera flecha adentro: **40. IniciarTransaccion** (MAPPER_RECETA → ACCESO).
  - Última flecha adentro: **45. ConfirmarTransaccion** (MAPPER_RECETA → ACCESO).
  - La siguiente, "Ok", ya queda afuera.

## DSS-CU015-EmitirFactura (diagrama 137)

Falta o hay que corregir:
- `alt` FA1 - Datos del cliente inválidos:
  - Primera flecha adentro: **4. DatosInvalidos** (FACTURA_BLL → frmVenta).
  - Última flecha adentro: **5. MuestraErrorDeDatos** (frmVenta → Vendedor).
  - La siguiente, "DatosValidos", ya queda afuera.
- `critical` Transacción propia, posterior a la venta (decisión 42):
  - Primera flecha adentro: **8. IniciarTransaccion** (FACTURA_BLL → ACCESO).
  - Última flecha adentro: **22. ConfirmarTransaccion** (FACTURA_BLL → ACCESO).
  - La siguiente, "Factura", ya queda afuera.
- `alt` FA2 - Error al registrar la factura:
  - Primera flecha adentro: **15. Error** (ACCESO → MAPPER_FACTURA), la 1.ª flecha "Error" del diagrama; viene justo después de "Escribir".
  - Última flecha adentro: **19. MuestraNoSePudoEmitirLaFacturaYPermiteReintentar** (frmVenta → Vendedor).
  - La siguiente, "Ok", ya queda afuera.

## DSS-CU016-GenerarOrdenCompra (diagrama 140)

Falta o hay que corregir:
- `alt` FA1 - Sin avisos pendientes:
  - Primera flecha adentro: **8. ListarInsumosActivos** (frmOrdenCompra → INSUMO_BLL), la 1.ª flecha "ListarInsumosActivos" del diagrama; viene justo después de "InsumosConAvisoPendiente".
  - Última flecha adentro: **14. MuestraNoHayAvisosPendientesYTodosLosInsumos** (frmOrdenCompra → Encargado de Stock).
  - La siguiente, "ListarProveedores", ya queda afuera.
- `alt` FA2 - Proveedor no registrado:
  - Primera flecha adentro: **22. NuevoProveedor** (Encargado de Stock → frmOrdenCompra).
  - Última flecha adentro: **23. EjecutaCU022YDejaElegidoElProveedorNuevo** (frmOrdenCompra → Encargado de Stock).
  - La siguiente, "SeleccionarProveedor", ya queda afuera.
- `alt` FA3 - Orden inválida (sin insumos, cantidad <= 0, repetidos, insumo que el proveedor no ofrece o proveedor inactivo):
  - Primera flecha adentro: **37. OrdenInvalida** (ORDEN_COMPRA_BLL → frmOrdenCompra).
  - Última flecha adentro: **38. MuestraErrorCorrespondiente** (frmOrdenCompra → Encargado de Stock).
  - La siguiente, "IniciarTransaccion", ya queda afuera.
- `critical` Una sola transacción de ACCESO (decisión 27):
  - Primera flecha adentro: **39. IniciarTransaccion** (ORDEN_COMPRA_BLL → ACCESO).
  - Última flecha adentro: **50. ConfirmarTransaccion** (ORDEN_COMPRA_BLL → ACCESO).
  - La siguiente, "OrdenGenerada", ya queda afuera.
- `loop` Por cada insumo con aviso pendiente:
  - Primera flecha adentro: **45. ActualizarAvisos** (INSUMO_BLL → MAPPER_INSUMO).
  - Última flecha adentro: **48. AvisoEnCompra** (MAPPER_INSUMO → INSUMO_BLL).
  - La siguiente, "AvisosAsociados", ya queda afuera.

## DSS-CU017-AprobarOrdenCompra (diagrama 141)

Falta o hay que corregir:
- `alt` FA1 - Sin órdenes pendientes: rodea solo la flecha **8. MuestraNoHayOrdenesPendientesDeAprobacion** (frmAprobacionOrden → Dueño). La siguiente, "MuestraOrdenesPendientes", ya queda afuera.
- `alt` FA2 - Devolución con observaciones:
  - Primera flecha adentro: **18. DevolverOrden** (Dueño → frmAprobacionOrden).
  - Última flecha adentro: **27. MuestraLaOrdenFueDevueltaAlEncargadoDeStock** (frmAprobacionOrden → Dueño).
  - La siguiente, "AprobarOrden", ya queda afuera.
- `alt` FA3 - Observaciones vacias:
  - Primera flecha adentro: **20. ObservacionesVacias** (ORDEN_COMPRA_BLL → frmAprobacionOrden).
  - Última flecha adentro: **21. MuestraDebeIngresarLasObservaciones** (frmAprobacionOrden → Dueño).
  - La siguiente, "ActualizarEstado", ya queda afuera.
- `alt` La orden ya no está Pendiente de aprobación:
  - Primera flecha adentro: **30. EstadoNoEsperado** (ORDEN_COMPRA_BLL → frmAprobacionOrden).
  - Última flecha adentro: **31. MuestraActualiceElListado** (frmAprobacionOrden → Dueño).
  - La siguiente, "ActualizarEstado", ya queda afuera.

## DSS-CU018-AjustarOrdenObservada (diagrama 142)

Falta o hay que corregir:
- `alt` FA1 - Sin órdenes observadas: rodea solo la flecha **8. MuestraNoHayOrdenesObservadas** (frmOrdenCompra → Encargado de Stock). La siguiente, "MuestraOrdenesObservadas", ya queda afuera.
- `alt` FA2 - Orden inválida:
  - Primera flecha adentro: **27. OrdenInvalida** (ORDEN_COMPRA_BLL → frmOrdenCompra).
  - Última flecha adentro: **28. MuestraErrorCorrespondiente** (frmOrdenCompra → Encargado de Stock).
  - La siguiente, "IniciarTransaccion", ya queda afuera.
- `critical` Una sola transacción de ACCESO (decisión 27):
  - Primera flecha adentro: **29. IniciarTransaccion** (ORDEN_COMPRA_BLL → ACCESO).
  - Última flecha adentro: **49. ConfirmarTransaccion** (ORDEN_COMPRA_BLL → ACCESO).
  - La siguiente, "OrdenAjustada", ya queda afuera.
- `loop` Por cada insumo que sigue en la orden:
  - Primera flecha adentro: **40. ActualizarAvisos** (INSUMO_BLL → MAPPER_INSUMO).
  - Última flecha adentro: **43. AvisoEnCompra** (MAPPER_INSUMO → INSUMO_BLL).
  - La siguiente, "AvisosReasociados", ya queda afuera.

## DSS-CU019-RegistrarRecepcion (diagrama 143)

Falta o hay que corregir:
- `alt` FA1 - Sin órdenes para recibir: rodea solo la flecha **8. MuestraNoHayOrdenesPendientesDeRecepcion** (frmRecepcion → Encargado de Stock). La siguiente, "MuestraOrdenes", ya queda afuera.
- `alt` FA2 - Datos de recepción inválidos:
  - Primera flecha adentro: **21. DatosInvalidos** (RECEPCION_BLL → frmRecepcion).
  - Última flecha adentro: **22. MuestraErrorCorrespondiente** (frmRecepcion → Encargado de Stock).
  - La siguiente, "IniciarTransaccion", ya queda afuera.
- `critical` Una sola transacción de ACCESO (decisión 27):
  - Primera flecha adentro: **23. IniciarTransaccion** (RECEPCION_BLL → ACCESO).
  - Última flecha adentro: **63. ConfirmarTransaccion** (RECEPCION_BLL → ACCESO).
  - La siguiente, "EstadoResultante", ya queda afuera.
- `loop` Por cada insumo recibido:
  - Primera flecha adentro: **29. BuscarInsumo** (INSUMO_BLL → MAPPER_INSUMO).
  - Última flecha adentro: **44. AvisosResueltos** (MAPPER_INSUMO → INSUMO_BLL).
  - La siguiente, "StockIngresado", ya queda afuera.
- `opt` Stock >= umbral (decisión 59):
  - Primera flecha adentro: **41. ResolverAvisos** (INSUMO_BLL → MAPPER_INSUMO).
  - Última flecha adentro: **44. AvisosResueltos** (MAPPER_INSUMO → INSUMO_BLL).
  - La siguiente, "StockIngresado", ya queda afuera.
- `loop` Por cada insumo recibido:
  - Primera flecha adentro: **47. ActualizarCantidadRecibida** (ORDEN_COMPRA_BLL → MAPPER_ORDEN_COMPRA).
  - Última flecha adentro: **50. Ok** (MAPPER_ORDEN_COMPRA → ORDEN_COMPRA_BLL), la 8.ª flecha "Ok" del diagrama; viene justo después de "Ok".
  - La siguiente, "CambiarEstado", ya queda afuera.
- `opt` La orden quedó Cerrada (decisión 60):
  - Primera flecha adentro: **57. ResolverReclamos** (RECEPCION_BLL → RECLAMO_BLL).
  - Última flecha adentro: **62. ReclamosResueltos** (RECLAMO_BLL → RECEPCION_BLL).
  - La siguiente, "ConfirmarTransaccion", ya queda afuera.
- `alt` FA3 - Recepción incompleta: rodea solo la flecha **65. MuestraQuedanPendientesYEjecutaCU020** (frmRecepcion → Encargado de Stock). La siguiente, "MuestraRecepcionRegistradaOrdenCerrada", ya queda afuera.

## DSS-CU020-RegistrarReclamo (diagrama 144)

Falta o hay que corregir:
- `alt` FA1 - Motivo faltante:
  - Primera flecha adentro: **15. MotivoFaltante** (RECLAMO_BLL → frmRecepcion).
  - Última flecha adentro: **16. MuestraDebeIndicarElMotivoDeCadaDiferencia** (frmRecepcion → Encargado de Stock).
  - La siguiente, "GuardarReclamo", ya queda afuera.

## DSS-CU021-RegistrarPagoProveedor (diagrama 145)

Falta o hay que corregir:
- `loop` Por cada orden cerrada:
  - Primera flecha adentro: **8. CalcularTotal** (frmPagoProveedor → ORDEN_COMPRA_BLL).
  - Última flecha adentro: **9. Total** (ORDEN_COMPRA_BLL → frmPagoProveedor).
  - La siguiente, "MuestraNoHayOrdenesPendientesDePago", ya queda afuera.
- `alt` FA1 - Sin órdenes pendientes de pago: rodea solo la flecha **10. MuestraNoHayOrdenesPendientesDePago** (frmPagoProveedor → Dueño). La siguiente, "MuestraOrdenesCerradasConSuTotal", ya queda afuera.
- `alt` FA2 - Datos de pago inválidos:
  - Primera flecha adentro: **31. DatosInvalidos** (PAGO_PROVEEDOR_BLL → frmPagoProveedor).
  - Última flecha adentro: **32. MuestraErrorCorrespondiente** (frmPagoProveedor → Dueño).
  - La siguiente, "IniciarTransaccion", ya queda afuera.
- `critical` Una sola transacción de ACCESO (decisión 27):
  - Primera flecha adentro: **33. IniciarTransaccion** (PAGO_PROVEEDOR_BLL → ACCESO).
  - Última flecha adentro: **44. ConfirmarTransaccion** (PAGO_PROVEEDOR_BLL → ACCESO).
  - La siguiente, "PagoRegistrado", ya queda afuera.

## DSS-CU022-InsertarProveedor (diagrama 146)

Falta o hay que corregir:
- `alt` FA1 - Datos incompletos o inválidos:
  - Primera flecha adentro: **11. DatosInvalidos** (PROVEEDOR_BLL → frmProveedor).
  - Última flecha adentro: **12. MuestraDatoACorregir** (frmProveedor → Encargado de Stock).
  - La siguiente, "GuardarProveedor", ya queda afuera.
- `alt` FA2 - Proveedor ya existente (CUIT repetido):
  - Primera flecha adentro: **15. ErrorCuitDuplicado** (ACCESO → MAPPER_PROVEEDOR), la 1.ª flecha "ErrorCuitDuplicado" del diagrama; viene justo después de "Escribir".
  - Última flecha adentro: **18. MuestraElProveedorYaExiste** (frmProveedor → Encargado de Stock).
  - La siguiente, "IdProveedor", ya queda afuera.

## DSS-CU023-InsertarInsumo (diagrama 149)

Falta o hay que corregir:
- `alt` FA1 - Datos incompletos o inválidos:
  - Primera flecha adentro: **9. DatosInvalidos** (INSUMO_BLL → frmInsumo).
  - Última flecha adentro: **10. MuestraDatoACorregir** (frmInsumo → Encargado de Stock).
  - La siguiente, "AltaInsumo", ya queda afuera.
- `alt` FA2 - Insumo ya existente:
  - Primera flecha adentro: **13. ErrorNombreDuplicado** (ACCESO → MAPPER_INSUMO), la 1.ª flecha "ErrorNombreDuplicado" del diagrama; viene justo después de "Escribir".
  - Última flecha adentro: **16. MuestraElInsumoYaExiste** (frmInsumo → Encargado de Stock).
  - La siguiente, "IdInsumo", ya queda afuera.

## DSS-CU024-ModificarInsumo (diagrama 150)

Falta o hay que corregir:
- `alt` FA1 - Datos incompletos o inválidos:
  - Primera flecha adentro: **13. DatosInvalidos** (INSUMO_BLL → frmInsumo).
  - Última flecha adentro: **14. MuestraDatoACorregir** (frmInsumo → Encargado de Stock).
  - La siguiente, "ModificarInsumo", ya queda afuera.
- `alt` FA2 - Nombre ya existente:
  - Primera flecha adentro: **17. ErrorNombreDuplicado** (ACCESO → MAPPER_INSUMO), la 1.ª flecha "ErrorNombreDuplicado" del diagrama; viene justo después de "Escribir".
  - Última flecha adentro: **20. MuestraElNombreYaExiste** (frmInsumo → Encargado de Stock).
  - La siguiente, "Ok", ya queda afuera.

## DSS-CU025-DarDeBajaInsumo (diagrama 151)

Falta o hay que corregir:
- `alt` FA1 - Insumo usado en recetas: rodea solo la flecha **8. MuestraProductosQueLoUsanYCancelaLaBaja** (frmInsumo → Encargado de Stock). La siguiente, "VerificarOrdenesAbiertas", ya queda afuera.
- `alt` FA2 - Insumo con órdenes de compra abiertas: rodea solo la flecha **15. MuestraOrdenesYCancelaLaBaja** (frmInsumo → Encargado de Stock). La siguiente, "MuestraDatosYSolicitaConfirmacion", ya queda afuera.
- `alt` FA3 - Baja cancelada:
  - Primera flecha adentro: **17. CancelarBaja** (Encargado de Stock → frmInsumo).
  - Última flecha adentro: **18. MuestraListadoDeInsumos** (frmInsumo → Encargado de Stock).
  - La siguiente, "ConfirmarBaja", ya queda afuera.

## DSS-CU026-ModificarProveedor (diagrama 147)

Falta o hay que corregir:
- `alt` FA3 - Insumo en una orden de compra abierta: rodea solo la flecha **30. MuestraOrdenAfectada** (frmProveedor → Encargado de Stock). La siguiente, "ModificarProveedor", ya queda afuera.
- `alt` FA1 - Datos incompletos o inválidos:
  - Primera flecha adentro: **32. DatosInvalidos** (PROVEEDOR_BLL → frmProveedor).
  - Última flecha adentro: **33. MuestraDatoACorregir** (frmProveedor → Encargado de Stock).
  - La siguiente, "ModificarProveedor", ya queda afuera.
- `alt` FA2 - CUIT ya existente:
  - Primera flecha adentro: **36. ErrorCuitDuplicado** (ACCESO → MAPPER_PROVEEDOR), la 1.ª flecha "ErrorCuitDuplicado" del diagrama; viene justo después de "Escribir".
  - Última flecha adentro: **39. MuestraElCuitYaExiste** (frmProveedor → Encargado de Stock).
  - La siguiente, "Ok", ya queda afuera.

## DSS-CU027-DarDeBajaProveedor (diagrama 148)

Falta o hay que corregir:
- `alt` FA1 - Proveedor con órdenes de compra abiertas: rodea solo la flecha **8. MuestraOrdenesAbiertasYCancelaLaBaja** (frmProveedor → Encargado de Stock). La siguiente, "SolicitaConfirmacionDeBaja", ya queda afuera.
- `alt` FA2 - Baja cancelada:
  - Primera flecha adentro: **10. CancelarBaja** (Encargado de Stock → frmProveedor).
  - Última flecha adentro: **11. MuestraListadoDeProveedores** (frmProveedor → Encargado de Stock).
  - La siguiente, "ConfirmarBaja", ya queda afuera.

## DSS-CU028-ConsultarVentas (diagrama 138)

Falta o hay que corregir:
- `alt` FA1 - Sin ventas en el periodo: rodea solo la flecha **10. MuestraNoHayVentas** (frmConsultaVentas → Vendedor). La siguiente, "MuestraVentas", ya queda afuera.
- `opt` El usuario pide ver la factura:
  - Primera flecha adentro: **20. VerFactura** (Vendedor → frmConsultaVentas).
  - Última flecha adentro: **23. AbrePdfDeLaFactura** (frmConsultaVentas → Vendedor).

## DSS-CU029-ConsultarVales (diagrama 139)

Falta o hay que corregir:
- `alt` FA1 - Sin vales en el periodo: rodea solo la flecha **10. MuestraNoHayVales** (frmConsultaVales → Vendedor). La siguiente, "MuestraValesEmitidos", ya queda afuera.

## DSS-CU030-ConsultarCompras (diagrama 152)

Falta o hay que corregir:
- `alt` FA1 - Sin órdenes para los filtros: rodea solo la flecha **18. MuestraNoHayOrdenes** (frmConsultaCompras → Encargado de Stock). La siguiente, "MuestraOrdenes", ya queda afuera.
