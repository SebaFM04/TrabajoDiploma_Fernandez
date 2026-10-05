using System;
using System.Drawing;
using System.Windows.Forms;

namespace UI
{
    // Decisión 64: casilla "Activo" en las grillas de los ABM. Desmarcarla da de baja y marcarla reactiva.
    // La grilla sigue de solo lectura: el clic avisa al formulario, que hace la baja o la reactivación y recarga la grilla.
    public static class ColumnaActivo
    {
        public static void Agregar(DataGridView dgv, string nombreColumna, string encabezado, Action<DataGridViewRow, bool> alCambiar)
        {
            dgv.Columns.Add(new DataGridViewCheckBoxColumn { Name = nombreColumna, HeaderText = encabezado, ReadOnly = true });
            dgv.CellContentClick += (s, e) =>
            {
                if (e.RowIndex < 0 || dgv.Columns[e.ColumnIndex].Name != nombreColumna) return;
                var fila = dgv.Rows[e.RowIndex];
                bool activo = fila.Cells[nombreColumna].Value is bool b && b;
                // Se selecciona la fila para que el formulario muestre sus datos, como con un clic normal
                fila.Selected = true;
                alCambiar(fila, !activo);
            };
        }

        // Las filas dadas de baja se ven en gris
        public static void Pintar(DataGridViewRow fila, bool activo)
        {
            fila.DefaultCellStyle.ForeColor = activo ? Color.Empty : Color.Gray;
            fila.DefaultCellStyle.SelectionForeColor = activo ? Color.Empty : Color.Gainsboro;
        }
    }
}
