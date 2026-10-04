using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace UI
{
    // Grillas: las columnas llenan el ancho disponible, pero nunca quedan más angostas que su encabezado y su contenido.
    // Si no entran, la grilla muestra la barra de desplazamiento horizontal (con Fill solo, las columnas se comprimían).
    public static class AjusteGrilla
    {
        // Ancho máximo que puede exigir una columna (textos muy largos, como el detalle de la Bitácora)
        const int AnchoMaximo = 600;

        static readonly HashSet<DataGridView> configuradas = new HashSet<DataGridView>();
        static readonly HashSet<DataGridView> pendientes = new HashSet<DataGridView>();

        public static void Configurar(DataGridView dgv)
        {
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.ScrollBars = ScrollBars.Both;
            if (!configuradas.Add(dgv))
            {
                Programar(dgv);
                return;
            }
            dgv.DataBindingComplete += (s, e) => Programar(dgv);
            dgv.RowsAdded += (s, e) => Programar(dgv);
            dgv.RowsRemoved += (s, e) => Programar(dgv);
            dgv.ColumnAdded += (s, e) => Programar(dgv);
            // También se dispara al cambiar el texto de un encabezado (RowIndex = -1), por ejemplo al cambiar el idioma
            dgv.CellValueChanged += (s, e) => Programar(dgv);
            dgv.HandleCreated += (s, e) => Programar(dgv);
            dgv.Disposed += (s, e) =>
            {
                configuradas.Remove(dgv);
                pendientes.Remove(dgv);
            };
            Programar(dgv);
        }

        // Ancho mínimo de cada columna visible = ancho preferido de su encabezado y sus celdas
        public static void Recalcular(DataGridView dgv)
        {
            if (dgv.IsDisposed) return;
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (!col.Visible) continue;
                int ancho = col.GetPreferredWidth(DataGridViewAutoSizeColumnMode.AllCells, true);
                col.MinimumWidth = Math.Max(20, Math.Min(ancho, AnchoMaximo));
            }
        }

        // Un solo recálculo después de la carga (no uno por fila agregada)
        static void Programar(DataGridView dgv)
        {
            if (dgv.IsDisposed || !dgv.IsHandleCreated) return;
            if (!pendientes.Add(dgv)) return;
            dgv.BeginInvoke((Action)(() =>
            {
                pendientes.Remove(dgv);
                Recalcular(dgv);
            }));
        }
    }
}
