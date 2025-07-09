using EstadosApiNet.Helpers;
using EstadosApiNet.Models;
using EstadosApiNet.Repositories.IRepositories;
using OfficeOpenXml;

namespace EstadosApiNet.Repositories
{
    public class CodigoPostalRepository : ICodigoPostalRepository
    {
        private static readonly Lazy<Dictionary<string, List<Asentamiento>>> _asentamientosPorCodigo =
       new Lazy<Dictionary<string, List<Asentamiento>>>(CargarDatos);
        private static Dictionary<string, List<Asentamiento>> CargarDatos()
        {
            Dictionary<string, List<Asentamiento>> datos = new Dictionary<string, List<Asentamiento>>();
            WebHostEnvironmentWrapper env = new WebHostEnvironmentWrapper();
            string filePath = Path.Combine(env.ContentRootPath, "Data", "CPdescarga.xlsx");

            using ExcelPackage package = new ExcelPackage(new FileInfo(filePath));

            foreach (ExcelWorksheet worksheet in package.Workbook.Worksheets)
            {
                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    Asentamiento asentamiento = new Asentamiento
                    {
                        d_codigo = worksheet.Cells[row, 1].Text,
                        d_asenta = worksheet.Cells[row, 2].Text,
                        d_tipo_asenta = worksheet.Cells[row, 3].Text,
                        D_mnpio = worksheet.Cells[row, 4].Text,
                        d_estado = worksheet.Cells[row, 5].Text,
                        d_ciudad = worksheet.Cells[row, 6].Text,
                        d_CP = worksheet.Cells[row, 7].Text,
                        c_estado = worksheet.Cells[row, 8].Text,
                        c_oficina = worksheet.Cells[row, 9].Text,
                        c_CP = worksheet.Cells[row, 10].Text,
                        c_tipo_asenta = worksheet.Cells[row, 11].Text,
                        c_mnpio = worksheet.Cells[row, 12].Text,
                        id_asenta_cpcons = worksheet.Cells[row, 13].Text,
                        d_zona = worksheet.Cells[row, 14].Text,
                        c_cve_ciudad = worksheet.Cells[row, 15].Text
                    };

                    // Agrupa por código postal
                    if (!datos.ContainsKey(asentamiento.d_codigo))
                    {
                        datos[asentamiento.d_codigo] = new List<Asentamiento>();
                    }
                    datos[asentamiento.d_codigo].Add(asentamiento);
                }
            }

            return datos;
        }

        public List<Asentamiento> GetByCodigo(string codigo)
        {
            if (_asentamientosPorCodigo.Value.TryGetValue(codigo, out List<Asentamiento> asentamientos))
            {
                return asentamientos;
            }
            return new List<Asentamiento>();
        }
    }
}