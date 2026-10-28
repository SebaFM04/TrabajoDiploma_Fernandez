# Pendientes manuales en EA – entrega 3 (27/10/2026)

Lo que el conector de Enterprise Architect no puede hacer: borrar elementos, cargar parámetros de operaciones (provoca el error de DAO "Referencia circular causada por el alias 'POS'") y dibujar fragmentos. Es lo mismo que quedó a mano en la 2ª entrega (`PENDIENTES_MANUALES_EA_Y_WORD.md`). El detalle de qué se cambió en EA está en `docs/09_CLASES_Y_DER_N03.md`, `docs/08_DSS_N03.md` y `docs/08b_ACTIVIDAD_N03_Y_LOTES.md`.

Antes de empezar: cerrá EA por completo y volvé a abrir el `.EAP` para que se refresquen los mensajes y los diagramas. Respaldo previo a la Fase 9: `Documentación/_respaldo/CorrecionDiagramaActividad-TpDIPLOMA_2026_Fernandez_antes_fase9.EAP` (fuera de git).

## 1. Borrar

**Diagrama de actividad N03 V3 (158), opción A:**
- Acciones `ZZ_BORRAR_Solicita las ventas y los costos de insumos del período` (959), `ZZ_BORRAR_Entrega el resumen de ventas del período` (961) y `ZZ_BORRAR_Entrega la lista de costos y vencimientos de insumos` (962).
- Fork `ZZ_BORRAR_Fork` (960), join `ZZ_BORRAR_Join` (963) y final `ZZ_BORRAR_Fin` (969). Con el 969 se va el flujo `ZZ_BORRAR_no` (2633), que ya no se ve en el diagrama.
- Carril `ZZ_BORRAR_Encargado de Stock` (956).

**DSS:**
- Boundaries `ZZ_BORRAR_` 1033 a 1036 (se crearon sin el estereotipo; las buenas son 1037 a 1040, clonadas de frmInsumo).
- Diagrama `ZZ_BORRAR_DSS-CU032` (167). El bueno es `DSS-CU032-ExportarAnalisisPdf` (175).
- Cuando revises los nuevos: `OLD_DSS-CU010` (133), `OLD_DSS-CU019` (143) y `OLD_DSS-CU023` (149).

**Pendiente de la 2ª entrega:**
- INSUMO_BLL.IngresarStock: borrar el parámetro `ZZ_BORRAR_costoUnidadCompra`. **Hecho el 27/10** (script con la API de EA, ver la sección 2).

## 2. Parámetros de las operaciones

**Hecho el 27/10, sin tocar EA a mano.** Con EA cerrado, un script de PowerShell de 32 bits usó la API de EA (COM, `EA.Repository`, `Method.Parameters.AddNew`) sobre el `.EAP`: cargó los parámetros de las 79 operaciones de la tabla y de las dos de abajo (105 parámetros nuevos), los ordenó como en la firma, dejó en la nota de cada una solo la descripción y borró los 7 parámetros `ZZ_BORRAR_` que quedaban (el de `IngresarStock` y los 6 de la 2ª entrega). Se probó antes sobre una copia y se verificó por Jet: cada operación tiene exactamente los parámetros de su firma, no queda ninguna nota con firma ni ningún `ZZ_BORRAR_` en parámetros, y no cambió ningún otro elemento, conector ni diagrama. La tabla queda como registro.

Las operaciones nuevas se crearon **sin parámetros**: en el diagrama se ven con `()` y la firma completa quedó en su descripción. Las que ya existían conservan los suyos. Hay que cargar estos parámetros (en la clase de su capa y en la homónima de `N03_General` o `N02_General`):

| Clase | Operación | Parámetros |
|---|---|---|
| ANALISIS_BLL | NombreArchivo | `BE.ANALISIS_RENTABILIDAD analisis` |
| ANALISIS_BLL | GuardarAnalisis | `BE.ANALISIS_RENTABILIDAD analisis, string ruta` |
| ANALISIS_BLL | AbrirAnalisis | `string ruta` |
| ANALISIS_BLL | TextoMotivo | `BE.ANALISIS_ITEM item` |
| ANALISIS_BLL | TextoCriterio | `string criterio` |
| ANALISIS_BLL | ExportarPdf | `BE.ANALISIS_RENTABILIDAD analisis, List<KeyValuePair<string, byte[]>> graficos` |
| ANALISIS_BLL | GenerarAnalisis | `string criterio, DateTime desde, DateTime hasta, int diasPorVencer` |
| ANALISIS_BLL | CalcularCostosActuales | `List<BE.PRODUCTO_TAMANIO> productos` |
| LOTE_BLL | ValidarFechaVencimiento | `BE.INSUMO insumo, DateTime? fechaVencimiento` |
| LOTE_BLL | CrearLote | `BE.INSUMO insumo, int? idRecepcion, decimal cantidad, decimal costoUnidadCompra, DateTime? fechaVencimiento, ACCESO acceso` |
| LOTE_BLL | ListarLotesInsumo | `int idInsumo` |
| LOTE_BLL | ListarLotesPorVencer | `int dias` |
| LOTE_BLL | DarDeBajaLoteVencido | `int idLote` |
| PROMOCION_BLL | ListarPromociones | `string estado` |
| PROMOCION_BLL | ObtenerPromocion | `int idPromocion` |
| PROMOCION_BLL | PrecioSugerido | `decimal precioLista` |
| PROMOCION_BLL | ArmarPromocionDesdeAnalisis | `BE.ANALISIS_RENTABILIDAD analisis, List<BE.ANALISIS_ITEM> items` |
| PROMOCION_BLL | ValidarPromocion | `BE.PROMOCION promocion` |
| PROMOCION_BLL | BuscarSuperposiciones | `BE.PROMOCION promocion` |
| PROMOCION_BLL | ObtenerMargenesNegativos | `BE.PROMOCION promocion` |
| PROMOCION_BLL | CrearPromocion | `BE.PROMOCION promocion, bool aceptaMargenNegativo` |
| PROMOCION_BLL | EvaluarPromocion | `int idPromocion` |
| PROMOCION_BLL | RegistrarEvaluacion | `int idPromocion, string resultado, string observaciones` |
| PROMOCION_BLL | ArmarRenovacion | `int idOrigen` |
| PROMOCION_BLL | RenovarPromocion | `BE.PROMOCION nueva, bool aceptaMargenNegativo` |
| SIMULADOR_BLL | ObtenerEstado | `out DateTime? desde, out DateTime? hasta` |
| SIMULADOR_BLL | Vigente | `DateTime fecha` |
| SIMULADOR_BLL | GenerarSimulacion | `int meses, int ventasPorNoche, int semilla, bool incluirPromocion, out BE.PROMOCION promocion` |
| VENTA_BLL | CalcularCostoLineas | `Dictionary<BE.VENTA_DETALLE, List<BE.RECETA>> recetas, Dictionary<int, decimal> costos` |
| MAPPER_ANALISIS | ListarVentasPorProducto | `DateTime desde, DateTime hasta` |
| MAPPER_ANALISIS | ListarVentasDiarias | `DateTime desde, DateTime hasta` |
| MAPPER_ANALISIS | ListarUnidadesPorMes | `DateTime desde, DateTime hasta` |
| MAPPER_ANALISIS | ListarConsumoInsumos | `DateTime desde, DateTime hasta` |
| MAPPER_ANALISIS | Leer | `string nombreSP, DateTime desde, DateTime hasta` |
| MAPPER_LOTE | InsertarLote | `BE.LOTE_INSUMO lote` |
| MAPPER_LOTE | BuscarLote | `int idLote` |
| MAPPER_LOTE | ListarLotesInsumo | `int idInsumo` |
| MAPPER_LOTE | ListarLotesVendibles | `int idInsumo` |
| MAPPER_LOTE | ListarLotesPorVencer | `int dias` |
| MAPPER_LOTE | DescontarLote | `int idLote, decimal cantidad` |
| MAPPER_LOTE | DarDeBajaLote | `int idLote, DateTime fechaBaja` |
| MAPPER_LOTE | Listar | `string nombreSP, List<SqlParameter> parametros = null` |
| MAPPER_LOTE | MapearLote | `DataRow u` |
| MAPPER_PROMOCION | GuardarPromocion | `BE.PROMOCION promocion` |
| MAPPER_PROMOCION | ListarPromociones | `string estado` |
| MAPPER_PROMOCION | BuscarPromocion | `int idPromocion` |
| MAPPER_PROMOCION | BuscarSuperpuestas | `int idProducto, int idTamanio, DateTime desde, DateTime hasta` |
| MAPPER_PROMOCION | ListarPreciosVigentes | `DateTime hoy` |
| MAPPER_PROMOCION | ListarUnidadesDiarias | `int idPromocion, DateTime desde, DateTime hasta` |
| MAPPER_PROMOCION | GuardarEvaluacion | `int idPromocion, string resultado, DateTime fechaEvaluacion, string observaciones` |
| MAPPER_PROMOCION | MarcarRenovada | `int idPromocion` |
| MAPPER_PROMOCION | ActualizarEstados | `DateTime hoy` |
| MAPPER_PROMOCION | MapearPromocion | `DataRow u` |
| MAPPER_VENTA | GuardarVentasSimuladas | `List<BE.VENTA> ventas` |
| MAPPER_VENTA | ContarVentasSimuladas | `out DateTime? desde, out DateTime? hasta` |
| GeneradorPdf | GenerarInforme | `string ruta, string titulo, IList<string> encabezado, IList<SeccionInforme> secciones, bool apaisado = true` |
| Serializador | Serializar<T> | `T objeto, string ruta` |
| Serializador | Deserializar<T> | `string ruta` |

Además, en operaciones que ya existían y cambiaron de firma:
- INSUMO_BLL.InsertarInsumo: agregar el parámetro `DateTime? fechaVencimiento` (después de `insumo`).
- INSUMO_BLL.DescontarStock: revisar que los parámetros queden `List<BE.RECETA> consumos, ACCESO acceso` (el tipo de retorno ya se cambió a `Dictionary<int, decimal>`).

**Alternativa (descartada):** con EA cerrado, Claude Code puede escribir estos parámetros directo en la tabla `t_operationparams` del `.EAP` (Jet, sobre el archivo, sin el conector), con respaldo previo. Ahorra la carga a mano, pero es la única escritura del `.EAP` por fuera de EA.

## 3. Fragmentos de los DSS

El conector no crea fragmentos (alt/opt/loop/critical). En cada DSS, dibujalos sobre los mensajes indicados (los números son el orden del mensaje en el diagrama, empezando en 1). Dentro de cada `alt` va solo el camino del flujo alternativo; el camino bueno sigue afuera. Los fragmentos anidados están con sangría. Criterio: `Documentación/GUIA_FRAGMENTOS_DSS.md`.

### DSS-CU031-GenerarAnalisisRentabilidad (diagrama 166)
- `alt` FA1 - Período inválido: mensajes 7 a 8
- `alt` FA2 - Sin ventas en el período: mensajes 15 a 16
- `loop` Por cada producto y tamaño a la venta: mensajes 24 a 25
- `alt` Criterio elegido: mensajes 34 a 57
  - operando [Ventas]: mensaje 34
  - operando [Rotación]: mensajes 35 a 41
  - operando [Vencimiento]: mensajes 42 a 52
  - operando [Estacionalidad]: mensajes 53 a 57
- `loop` Por cada producto sugerido: mensajes 60 a 61
- `opt` FA4 - Guardar el análisis: mensajes 64 a 77
  - `alt` FA5 - Error al guardar el análisis: mensajes 74 a 75
- `opt` FA6 - Abrir un análisis guardado: mensajes 78 a 92
  - `alt` FA7 - Archivo de análisis inválido: mensajes 88 a 89

### DSS-CU032-ExportarAnalisisPdf (diagrama 175)
- `alt` FA1 - No hay análisis a la vista: mensajes 4 a 5
- `loop` Por cada producto sugerido: mensaje 7
- `alt` FA2 - Error al guardar: mensajes 9 a 11

### DSS-CU033-CrearPromocion (diagrama 168)
- `alt` Cómo se abre promociones: mensajes 1 a 6
  - operando [desde el menú]: mensaje 1
  - operando [FA6 - desde el análisis de rentabilidad]: mensajes 2 a 6
- `loop` Por cada producto y tamaño: mensaje 18
- `loop` Por cada producto que se agrega: mensajes 30 a 31
- `loop` Por cada producto y tamaño de la promoción: mensajes 36 a 39
- `alt` FA1, FA2 y FA3 - Datos incompletos, precio o vigencia inválidos: mensajes 40 a 41
- `alt` FA4 - Promoción superpuesta: mensajes 42 a 45
- `opt` FA5 - Margen negativo: mensajes 49 a 50
- `critical` Una sola transacción de ACCESO: mensajes 54 a 61
  - `loop` Por cada línea de la promoción: mensajes 58 a 59

### DSS-CU034-EvaluarResultadoPromocion (diagrama 169)
- `alt` FA1 - La promoción todavía no terminó: mensajes 23 a 24
- `loop` Por cada producto y tamaño de la promoción: mensaje 33
- `alt` FA4 - Resultado u observaciones inválidos: mensajes 45 a 46
- `alt` FA5 - Otro usuario la evaluó antes: mensajes 53 a 54

### DSS-CU035-RenovarPromocion (diagrama 170)
- `alt` FA1 y FA2 - No finalizada, ya renovada o sin evaluar: mensajes 10 a 11
- `alt` FA3 - Datos inválidos o superposición: mensajes 22 a 23
- `opt` FA4 - Margen negativo: mensajes 27 a 28
- `critical` Una sola transacción de ACCESO: mensajes 33 a 47
  - `alt` FA5 - Otro usuario la renovó antes: mensajes 38 a 40
  - `loop` Por cada línea de la promoción: mensajes 44 a 45

### DSS-CU036-DarDeBajaLoteVencido (diagrama 171)
- `alt` FA2 - Sin lotes vencidos: mensaje 8
- `opt` FA5 - Consultar lotes por vencer: mensajes 10 a 17
- `alt` FA3 - Sin lote seleccionado: mensaje 19
- `alt` Respuesta del Encargado de Stock: mensajes 21 a 22
  - operando [FA4 - no confirma]: mensaje 21
  - operando [confirma]: mensaje 22
- `critical` Una sola transacción de ACCESO: mensajes 28 a 44
  - `alt` FA1 - El lote ya no está vencido o no tiene cantidad: mensajes 33 a 35

### DSS-CU010-RegistrarVenta (diagrama 172)
- `loop` Por cada línea del pedido: mensajes 26 a 31
- `loop` Por cada insumo (consumos sumados): mensajes 32 a 37
- `alt` FA1 - Insumo insuficiente: mensajes 38 a 39
- `alt` FA4 - Producto o tamaño no disponible al cobrar: mensajes 47 a 48 **(nuevo)**
- `loop` Por cada línea: receta escalada para el consumo y el costo: mensajes 50 a 51 **(nuevo)**
- `critical` Una sola transacción de ACCESO (decisión 27): mensajes 52 a 104
  - `loop` Segunda verificación de stock, por cada insumo: mensajes 53 a 58
  - `alt` FA3 - Stock insuficiente al confirmar el cobro: mensajes 59 a 61
  - `loop` Por cada insumo consumido: mensajes 63 a 78 **(nuevo)**

### DSS-CU019-RegistrarRecepcion (diagrama 173)
- `alt` FA1 - Sin órdenes para recibir: mensaje 8
- `alt` FA4 - Fecha con formato inválido (la detecta la pantalla): mensaje 19 **(nuevo)**
- `loop` Por cada insumo recibido: mensajes 22 a 27 **(nuevo)**
- `alt` FA2 - Datos de recepción inválidos: mensajes 28 a 29
- `alt` FA4 - Fecha de vencimiento faltante o no posterior a hoy: mensajes 30 a 31 **(nuevo)**
- `critical` Una sola transacción de ACCESO (decisión 27): mensajes 32 a 78
  - `loop` Por cada insumo recibido: mensajes 38 a 59

### DSS-CU023-InsertarInsumo (diagrama 174)
- `alt` FA1 - Datos incompletos: mensajes 9 a 10
- `opt` Tiene stock inicial: mensajes 11 a 12 **(nuevo)**
- `alt` FA3 - Fecha de vencimiento faltante o inválida: mensajes 13 a 14 **(nuevo)**
- `critical` Una sola transacción de ACCESO: mensajes 15 a 31 **(nuevo)**
  - `alt` FA2 - Insumo ya existente: mensajes 18 a 22
  - `opt` Tiene stock inicial: mensajes 25 a 30 **(nuevo)**

## 4. Retoques de layout

- **Actividad N03 V3 (158):** el flujo `si` de "¿Hay productos convenientes para promocionar?" ahora va al nodo de fusión nuevo (a la derecha de "Arma la promoción"); revisar que se vea la etiqueta `si` y acomodar las líneas si hace falta.
- **Actividad "N02 Control de lotes vencidos" (176, nuevo):** el carril quedó con el título a la izquierda (orientación horizontal). Si querés que se vea como los otros diagramas (título arriba), cambiale la orientación al carril.
- **Actividad N02 (101):** la nota nueva "Lote de insumo [disponible]" quedó arriba a la izquierda, cerca de la etiqueta "¿Coincide con lo pedido?".
- **Modelo conceptual N01:** hay etiquetas de asociaciones superpuestas cerca de LOTE_INSUMO.
- **Casos de uso de N02 (103):** revisar la ubicación de CU036 "Dar de Baja Lote Vencido".
