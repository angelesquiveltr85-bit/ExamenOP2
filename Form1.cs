using Octubre.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExamenOP2
{
    public partial class Form1 : Form
    {
        Datos datos = new Datos();
        public Form1()
        {
            InitializeComponent();
        }

        private void mostrardatos()
        {
            string consulta = "SELECT " +
                  "  t1.id_empleado as \"ID\", " +
                  "  t1.nombre as \"Nombre\", " +
                  "  t1.apellidos as \"Apellidos\", " +
                  "  t1.direccion as \"Dirección\", " +
                  "  t2.id_empresa as \"ID Empresa\", " +   
                  "  t2.nom_empresa as \"Empresa\", " +    
                  "  t2.dir_empresa as \"Dirección Empresa\" " +
                  "FROM public.\"Empleado\" t1 " +
                  "INNER JOIN public.\"Empresa\" t2 ON t1.id_empresa = t2.id_empresa";

            DataSet ds = datos.getAllData(consulta);

            if (ds != null && ds.Tables.Count > 0)
            {
                dgvTablas.DataSource = ds.Tables[0];
            }
            else
            {
                // Tu lógica de manejo de errores
                if (!datos.TestConnection())
                {
                    MessageBox.Show("Error de conexión.", "Sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("No hay registros.", "Sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void dgvTablas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            mostrardatos();
        }

        private void btnReporte_Click(object sender, EventArgs e)
        {
            FrmCriystal reporte = new FrmCriystal();
            reporte.ShowDialog();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            // Verificamos si hay una fila seleccionada en la tabla
            if (dgvTablas.CurrentRow != null && dgvTablas.CurrentRow.Index >= 0)
            {
                try
                {
                    // 1. Obtenemos el ID de la primera columna (Celda 0)
                    string idString = dgvTablas.CurrentRow.Cells[0].Value.ToString();
                    int idSeleccionado = Convert.ToInt32(idString);

                    // 2. Abrimos el formulario frmAgenda enviándole el ID
                    // Esto hará que frmAgenda se ponga en "Modo Editar"
                    frmAgenda agenda = new frmAgenda(idSeleccionado);
                    agenda.ShowDialog();

                    // 3. Cuando cierres la ventana de editar, refrescamos la tabla
                    mostrardatos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al seleccionar: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un empleado de la lista para editar.", "Aviso");
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmAgenda agenda = new frmAgenda();
            agenda.ShowDialog();
            mostrardatos();
        }
    }
}
