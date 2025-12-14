using CristalApp.Clases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.ReportAppServer;
using CrystalDecisions.CrystalReports.Engine;

namespace ExamenOP2
{
    public partial class FrmCriystal : Form
    {
        public FrmCriystal()
        {
            InitializeComponent();
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {
            
            try
            {
                GenerarReporte generador = new GenerarReporte();
                ReportDocument reporteGenerado = generador.CrearReporte();
                crystalReportViewer1.ReportSource = reporteGenerado;
                crystalReportViewer1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el reporte: " + ex.Message,
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

                this.Close();
            }
        
    }

        private void FrmCriystal_Load(object sender, EventArgs e)
        {

        }
    }
}
