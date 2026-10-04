using BE;
using DAL;
using SERVICIO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    // CU010 Registrar Venta (DSS-CU010)
    public class VENTA_BLL
    {
        RECETA_BLL GestorReceta = new RECETA_BLL();
        INSUMO_BLL GestorInsumo = new INSUMO_BLL();
        VALE_BLL GestorVale = new VALE_BLL();
        COMANDA_BLL GestorComanda = new COMANDA_BLL();
        MAPPER_PRODUCTO GestorProducto = new MAPPER_PRODUCTO();

        // Valores de VENTA.MedioPago (CHECK CK_VENTA_MedioPago, decisión 15)
        public string[] ListarMediosPago()
        {
            return new[] { "Efectivo", "Débito", "Crédito", "Transferencia", "QR" };
        }

        // CU010 paso 1: productos activos con receta (S1), con cada tamaño y su precio
        public List<BE.PRODUCTO_TAMANIO> ListarProductosParaVenta()
        {
            return GestorProducto.ListarProductosConPrecios();
        }

        // Consumo total por insumo de todo el pedido (decisión 4): suma las recetas escaladas de cada línea.
        // Cada elemento lleva en IdProducto uno de los productos que lo usan (para informar en FA1).
        private List<BE.RECETA> CalcularConsumos(BE.VENTA pedido, out Dictionary<int, List<int>> productosPorInsumo)
        {
            var consumos = new Dictionary<int, BE.RECETA>();
            productosPorInsumo = new Dictionary<int, List<int>>();
            foreach (var linea in pedido.Detalles)
            {
                // Mensajes 3-8: receta del producto escalada al tamaño y la cantidad pedidos
                foreach (var r in GestorReceta.ObtenerRecetaEscalada(linea.IdProducto, linea.ProductoTamanio.Tamanio.CantidadMagnitud, linea.Cantidad))
                {
                    if (!consumos.ContainsKey(r.IdInsumo))
                    {
                        consumos[r.IdInsumo] = new BE.RECETA { IdInsumo = r.IdInsumo, IdProducto = r.IdProducto, Proporcion = 0, Insumo = r.Insumo };
                        productosPorInsumo[r.IdInsumo] = new List<int>();
                    }
                    consumos[r.IdInsumo].Proporcion += r.Proporcion;
                    if (!productosPorInsumo[r.IdInsumo].Contains(linea.IdProducto))
                        productosPorInsumo[r.IdInsumo].Add(linea.IdProducto);
                }
            }
            return consumos.Values.ToList();
        }

        private void ValidarPedido(BE.VENTA pedido)
        {
            // Condición del CU: al menos un producto con tamaño y precio
            if (pedido == null || pedido.Detalles == null || pedido.Detalles.Count == 0)
                throw new ArgumentException("msgVentaPedidoVacio");
            if (pedido.Detalles.Any(d => d.Cantidad <= 0 || d.ProductoTamanio == null))
                throw new ArgumentException("msgVentaCantidadInvalida");
        }

        // CU010 paso 2 (mensaje 2). Devuelve los insumos que no alcanzan (FA1): una línea por insumo y producto afectado,
        // con el consumo requerido en Proporcion y el insumo (con su stock) en Insumo. Lista vacía = hay stock.
        public List<BE.RECETA> VerificarDisponibilidadInsumos(BE.VENTA pedido)
        {
            ValidarPedido(pedido);
            var faltantes = new List<BE.RECETA>();
            foreach (var consumo in CalcularConsumos(pedido, out var productosPorInsumo))
            {
                // Mensajes 9-17
                if (!GestorInsumo.VerificarDisponibilidad(consumo))
                {
                    foreach (int idProducto in productosPorInsumo[consumo.IdInsumo])
                        faltantes.Add(new BE.RECETA { IdInsumo = consumo.IdInsumo, IdProducto = idProducto, Proporcion = consumo.Proporcion, Insumo = consumo.Insumo });
                }
            }
            return faltantes;
        }

        // CU010 paso 3 (mensajes 25-26): importe de cada línea y total
        public decimal CalcularMontoTotal(BE.VENTA pedido)
        {
            ValidarPedido(pedido);
            foreach (var linea in pedido.Detalles)
                linea.MontoLinea = linea.ProductoTamanio.Precio * linea.Cantidad;
            pedido.Monto = pedido.Detalles.Sum(d => d.MontoLinea);
            return pedido.Monto;
        }

        // CU010 pasos 4 y 5 (mensajes 29-52). Todo en una sola transacción (decisiones 27, 42 y 47):
        // segunda verificación de stock, descuento, aviso de stock bajo, venta con detalles, vale y comanda.
        public BE.VENTA RegistrarVenta(BE.VENTA venta)
        {
            if (Array.IndexOf(ListarMediosPago(), venta.MedioPago) < 0)
                throw new ArgumentException("msgVentaMedioPagoObligatorio");
            CalcularMontoTotal(venta);
            var consumos = CalcularConsumos(venta, out _);
            venta.Fecha = DateTime.Now;

            var acceso = new ACCESO();
            acceso.IniciarTransaccion();
            try
            {
                // Ajuste del paso 5: se vuelve a verificar el stock dentro de la transacción
                if (consumos.Any(c => !GestorInsumo.VerificarDisponibilidad(c, acceso)))
                    throw new ArgumentException("msgVentaStockInsuficiente");

                GestorInsumo.DescontarStock(consumos, acceso);
                GestorInsumo.VerificarStockBajoUmbral(consumos.Select(c => c.IdInsumo).ToList(), acceso);
                new MAPPER_VENTA(acceso).GuardarVenta(venta);
                venta.Vale = GestorVale.GenerarVale(venta, acceso);
                venta.Comanda = venta.Detalles.Any(d => d.ProductoTamanio.Producto?.Tipo == "Piqueo")
                    ? GestorComanda.GenerarComanda(venta, acceso)
                    : null;
                acceso.ConfirmarTransaccion();
            }
            catch
            {
                acceso.DeshacerTransaccion();
                venta.IdVenta = 0;
                venta.Vale = null;
                venta.Comanda = null;
                throw;
            }

            new BITACORA_BLL().RegistrarEvento(SessionManager.Instancia.UsuarioActual.IdUsuario, "Registro de venta",
                $"Venta {venta.IdVenta}: {venta.Monto:0.00} ({venta.MedioPago}), vale {venta.Vale.IdVale}" + (venta.Comanda != null ? $", comanda {venta.Comanda.IdComanda}" : ""));
            return venta;
        }
    }
}
