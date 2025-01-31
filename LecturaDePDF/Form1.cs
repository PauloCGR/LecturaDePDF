using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Text.RegularExpressions;

namespace LecturaDePDF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void ExtraerDatosPDF(string rutaPDF)
        {
          
            using (PdfReader reader = new PdfReader(rutaPDF))
            {

                for (int pageNumber = 1; pageNumber <= reader.NumberOfPages; pageNumber++)
                {
                   
                    string text = PdfTextExtractor.GetTextFromPage(reader, pageNumber);

                    string rfc = FiltrarInformacion(text, "RFC: ([A-Z0-9]+)");
                    string curp = FiltrarInformacion(text, "CURP: ([A-Z0-9]+)");
                    string nombre = FiltrarInformacion(text, "Nombre \\(s\\): ([A-Z]+)");
                    //string apellido = FiltrarInformacion(text, "Primer Apellido: ([A-ZñÑ]+)");
                    //string segundoApellido = FiltrarInformacion(text, "Segundo Apellido: ([A-Za-zñÑ ]+)");
                    string apellido = ObtenerApellidoCompleto(text, "Primer Apellido: ([A-ZñÑ]+)");
                    var Lista = apellido.Split(' ');
                    
                    string fechaInicio = FiltrarInformacion(text, "Fecha inicio de operaciones: ([0-9A-Z ]+)");
                    string estatus = FiltrarInformacion(text, "Estatus en el padrón: ([A-Za-z ]+)");
                    string fechaCambio = FiltrarInformacion(text, "Fecha de último cambio de estado: ([0-9A-Z ]+)");
                    string nombreComercial = FiltrarInformacion(text, "Nombre Comercial: ([A-Za-zñÑ ]+)");

                    txtRFC.Text += rfc + Environment.NewLine;
                    txtCURP.Text += curp + Environment.NewLine;
                    txtNombre.Text += nombre + Environment.NewLine;
                    //txtPApellido.Text += Lista[0] + Environment.NewLine;
                    //txtSApellido.Text += Lista[1] + Environment.NewLine;
                    txtPApellido.Text += Lista.Length >= 2 ? Lista[0] : string.Empty + Environment.NewLine;
                    txtSApellido.Text += Lista.Length >= 2 ? Lista[1] : string.Empty + Environment.NewLine;
                
                    txtFecha.Text += fechaInicio + Environment.NewLine;
                    txtEstatus.Text += estatus + Environment.NewLine;
                    txtFechaCambioEstado.Text += fechaCambio + Environment.NewLine;
                    txtNombreComercial.Text += nombreComercial + Environment.NewLine;
                }
            }
        }

        private string FiltrarInformacion(string texto, string patron)
        {
            string resultado = string.Empty;

            Match match = Regex.Match(texto, patron);
            if (match.Success)
            {
                resultado = match.Groups[1].Value;
            }

            return resultado;
        }

        private string ObtenerApellidoCompleto(string texto, string patron)
        {
            string resultado = string.Empty;

            Match match = Regex.Match(texto, patron);
            if (match.Success)
            {
                int indice = match.Index + match.Length; // Obtener el índice después del patrón encontrado
                string[] lineas = texto.Substring(indice).Split('\n'); // Dividir el texto restante en líneas

                if (lineas.Length >= 2) // Asegurarse de que haya al menos dos líneas
                {
                    resultado = match.Groups[1].Value + " " + lineas[1].Trim(); // Concatenar el valor encontrado y el siguiente renglón sin espacios extras
                }
            }

            return resultado;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            string rutaPDF = @"C:\Users\Paulo Cesar\Desktop\Lectura\SAT_HOUSE.pdf";
            ExtraerDatosPDF(rutaPDF);

            MessageBox.Show("Datos extraídos exitosamente");
        }
    }
}