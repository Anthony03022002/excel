using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
// Importa la clase ImportacionesCRUD
using Proyectoimportaciones_v1;
using OfficeOpenXml;
using System.IO;
using Microsoft.Office.Interop.Excel;
using objExcel = Microsoft.Office.Interop.Excel;
using DataTable = System.Data.DataTable;

namespace Proyectoimportaciones_v1
{
    public partial class ImportacionesES : Form
    {
        public ImportacionesES()
        {
            InitializeComponent();
            Obtener();
        }
        private void btnExportarImportacionesEntradas_Click(object sender, EventArgs e)
        {
            // Crea un objeto de tipo EntradaImportaciones y asigna sus valores
            EntradaImportacion entradaImportacion = new EntradaImportacion();
            entradaImportacion.NumeroImportacion = txtNumerodeImportacion.Text;
            entradaImportacion.CodigoProducto = txtCodigoProducto.Text;
            entradaImportacion.NombreProducto = txtNombreProducto.Text;
            entradaImportacion.Lote = txtLote.Text;
            entradaImportacion.Proveedor = cmbProveedor.SelectedItem.ToString();
            int cantidadImportada;
            if (Int32.TryParse(txtCantidadImportada.Text, out cantidadImportada))
            {
                entradaImportacion.CantidadImportada = cantidadImportada;
            }
            else
            {
                MessageBox.Show("El valor de cantidad importada solo acepta números.");
                return;
            }
            int cantidadRecibida;
            if (Int32.TryParse(txtCantidadRecibida.Text, out cantidadRecibida))
            {
                entradaImportacion.CantidadRecibida = cantidadRecibida;
            }
            else
            {
                MessageBox.Show("El valor de cantidad recibida solo acepta números.");
                return;
            }

            // Llama a la función RegistroImportacionesEntradas de la clase ImportacionesCRUD para guardar los datos en la BD
            ImportacionesCRUD.RegistroImportacionesEntradas(entradaImportacion);

            // Muestra un mensaje de éxito
            MessageBox.Show("Los datos se han guardado correctamente.");
            Obtener();
        }
        private void Obtener()
        {
            DBFuncionImportacionesEntities dbfuncionaimportacionesEntities = new DBFuncionImportacionesEntities(); //entity framework
            var importacionesentradas = dbfuncionaimportacionesEntities.EntradaImportacion.ToList(); //referencia al objeto
            dtgImportacionesEntradas.DataSource = importacionesentradas; //cargar el grid
        }

        private void txtNumerodeImportacion_TextChanged(object sender, EventArgs e)
        {

        }

        private void ExportarExcel_Click(object sender, EventArgs e)
        {
            objExcel.Application objAplicacion = new objExcel.Application();
            Workbook objLibro = objAplicacion.Workbooks.Add(XlSheetType.xlWorksheet);
            Worksheet objHoja = (Worksheet)objAplicacion.ActiveSheet;

            objAplicacion.Visible = false;

            foreach (DataGridViewColumn columna in dtgImportacionesEntradas.Columns)
            {
                objHoja.Cells[1, columna.Index + 1] = columna.HeaderText;
                foreach (DataGridViewRow fila in dtgImportacionesEntradas.Rows)
                {
                    objHoja.Cells[fila.Index + 2, columna.Index + 1] = fila.Cells[columna.Index].Value;
                }
            }

            //exportar a excel
            string folderPath = "C:\\REPORTES\\"; //directorio de exportación
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }


            var nombre = this.Name + DateTime.Now.ToString("ddMMyyyy") + ".xlsx";



            var pathGeneral = folderPath + DateTime.Now.ToString("ddMMyyyy") + "-" + DateTime.Now.ToShortTimeString().Replace(":", "") + "-" + nombre;
            FileInfo file = new FileInfo(pathGeneral);

            objLibro.SaveAs(pathGeneral);





            MessageBox.Show("Exportación Exitosa");

        }


    }

    
    
    
}
