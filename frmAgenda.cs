
using ExamenOP2; // Asegúrate de que coincida con tu namespace
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
    public partial class frmAgenda : Form
    {
        Datos datos = new Datos();
        int idEmpleado = 0; // 0 significa nuevo, >0 significa editar

        // Constructor modificado para recibir el ID
        public frmAgenda(int id = 0)
        {
            InitializeComponent();
            this.idEmpleado = id;
        }

        private void frmAgenda_Load(object sender, EventArgs e)
        {
            if (idEmpleado > 0)
            {
                CargarDatosParaEditar();
                // Si estamos editando, ajustamos los botones visualmente
                btnAgregar.Enabled = false;
                btnActualizar.Enabled = true;
            }
            else
            {
                // Si es nuevo
                btnAgregar.Enabled = true;
                btnActualizar.Enabled = false;
            }
        }

        private void CargarDatosParaEditar()
        {
            // Traemos los datos de AMBAS tablas para llenar los textbox
            string consulta = "SELECT " +
                              "  t1.nombre, " +
                              "  t1.apellidos, " +
                              "  t1.direccion, " +
                              "  t2.nom_empresa, " +
                              "  t2.dir_empresa " +
                              "FROM public.\"Empleado\" t1 " +
                              "INNER JOIN public.\"Empresa\" t2 ON t1.id_empresa = t2.id_empresa " +
                              "WHERE t1.id_empleado = " + idEmpleado;

            DataSet ds = datos.getAllData(consulta);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow fila = ds.Tables[0].Rows[0];
                // Asumiendo que tus TextBox se llaman así (si no, cámbiales el nombre aquí)
                txtNombre.Text = fila["nombre"].ToString();
                txtApellidos.Text = fila["apellidos"].ToString();
                rtbDireccion.Text = fila["direccion"].ToString();
                txtNombreEmpresa.Text = fila["nom_empresa"].ToString();
                txtDireccionEmpresa.Text = fila["dir_empresa"].ToString();
            }
        }

        // --- BOTÓN AGREGAR (INSERTAR) ---
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (ValidarCampos())
            {
                // TRUCO AVANZADO DE POSTGRES (CTE):
                // Esto inserta la empresa primero, agarra el ID que se creó, y lo usa para insertar al empleado.
                // Todo en una sola instrucción.
                string sql = "WITH nueva_empresa AS ( " +
                             "   INSERT INTO public.\"Empresa\" (nom_empresa, dir_empresa) " +
                             "   VALUES ('" + txtNombreEmpresa.Text + "', '" + txtDireccionEmpresa.Text + "') " +
                             "   RETURNING id_empresa " +
                             ") " +
                             "INSERT INTO public.\"Empleado\" (nombre, apellidos, direccion, id_empresa) " +
                             "SELECT '" + txtNombre.Text + "', '" + txtApellidos.Text + "', '" + rtbDireccion.Text + "', id_empresa " +
                             "FROM nueva_empresa;";

                if (datos.ExecuteQuery(sql))
                {
                    MessageBox.Show("Guardado con éxito.", "Sistema");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Error al guardar.", "Sistema");
                }
            }
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtNombreEmpresa.Text))
            {
                MessageBox.Show("El nombre del empleado y la empresa son obligatorios.");
                return false;
            }
            return true;
        }

        private void btnActualizar_Click_1(object sender, EventArgs e)
        {
            if (ValidarCampos())
            {
                // Aquí actualizamos las dos tablas por separado usando el ID del empleado

                // 1. Primero actualizamos la Empresa (usando una subconsulta para encontrar cuál es la empresa de este empleado)
                string sqlEmpresa = "UPDATE public.\"Empresa\" " +
                                    "SET nom_empresa='" + txtNombreEmpresa.Text + "', dir_empresa='" + txtDireccionEmpresa.Text + "' " +
                                    "WHERE id_empresa = (SELECT id_empresa FROM public.\"Empleado\" WHERE id_empleado=" + idEmpleado + ");";

                // 2. Luego actualizamos al Empleado
                string sqlEmpleado = "UPDATE public.\"Empleado\" " +
                                     "SET nombre='" + txtNombre.Text + "', apellidos='" + txtApellidos.Text + "', direccion='" + rtbDireccion.Text + "' " +
                                     "WHERE id_empleado=" + idEmpleado + ";";

                // Ejecutamos ambas (puedes concatenarlas en un solo string para ExecuteQuery)
                if (datos.ExecuteQuery(sqlEmpresa + sqlEmpleado))
                {
                    MessageBox.Show("Actualizado con éxito.", "Sistema");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Error al actualizar.", "Sistema");
                }
            }
        }

        private void btnCancelar_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
