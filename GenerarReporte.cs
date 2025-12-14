using CrystalDecisions.CrystalReports.Engine;
using Npgsql;
using ExamenOP2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CristalApp.Clases
{
    public class GenerarReporte
    {
        private string connectionString =
            "Host=localhost;Port=5439;Username=angel;Password=12345;" +
            "Database=examen2";
        public ReportDocument CrearReporte()
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    string Query = @"SELECT 
                                t1.id_empleado, 
                                t1.nombre, 
                                t1.apellidos, 
                                t1.direccion, 
                                t1.id_empresa,
                                t2.nom_empresa, 
                                t2.dir_empresa
                             FROM public.""Empleado"" t1 
                             INNER JOIN public.""Empresa"" t2 ON t1.id_empresa = t2.id_empresa";

                    NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(Query, connection);

                    DataSetApp ds = new DataSetApp();
                    dataAdapter.Fill(ds, "Productos");

                    ReportDocument reporte = new ReportDocument();

                    reporte.Load(@"C:\Users\DELL\OneDrive\Escritorio\Semestres\Semestre 4\Topicos AP\Unidad 3\ExamenOP2\Reporte.rpt");
                    reporte.SetDataSource(ds.Tables["Reporte"]);
                    return reporte;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al generar el reporte: " + ex.Message);
                }
            }

        }

    }
}