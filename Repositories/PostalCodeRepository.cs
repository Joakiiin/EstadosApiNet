using EstadosApiNet.Helpers;
using EstadosApiNet.Models;
using EstadosApiNet.Repositories.IRepositories;
using OfficeOpenXml;

namespace EstadosApiNet.Repositories
{
    public class PostalCodeRepository : IPostalCodeRepository
    {
        private static readonly Lazy<Dictionary<string, List<Settlements>>> _settlementsByCode =
       new Lazy<Dictionary<string, List<Settlements>>>(LoadData);
        private static Dictionary<string, List<Settlements>> LoadData()
        {
            Dictionary<string, List<Settlements>> data = new Dictionary<string, List<Settlements>>();
            WebHostEnvironmentWrapper env = new WebHostEnvironmentWrapper();
            string filePath = Path.Combine(env.ContentRootPath, "Data", "CPdescarga.xlsx");

            using ExcelPackage package = new ExcelPackage(new FileInfo(filePath));

            foreach (ExcelWorksheet worksheet in package.Workbook.Worksheets)
            {
                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    Settlements settlement = new Settlements
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
                    if (!data.ContainsKey(settlement.d_codigo))
                    {
                        data[settlement.d_codigo] = new List<Settlements>();
                    }
                    data[settlement.d_codigo].Add(settlement);
                }
            }

            return data;
        }

        public List<string> SearchPostalCodes(string pattern, int? limit = null)
        {
            // Obtenemos todos los códigos postales únicos
            List<string> allCodes = _settlementsByCode.Value.Keys.ToList();

            // Filtramos por patrón (coincidencia parcial)
            List<string> filteredCodes = allCodes
                .Where(code => code.Contains(pattern))
                .ToList();

            // Aplicamos límite si es necesario
            return limit.HasValue && limit > 0
                ? filteredCodes.Take(limit.Value).ToList()
                : filteredCodes;
        }
        public List<string> GetUniqueEstados()
        {
            return _settlementsByCode.Value.Values
                .SelectMany(list => list)
                .Select(a => a.d_estado)
                .Distinct()
                .OrderBy(e => e)
                .ToList();
        }
        public List<string> GetMunicipalitiesByState(string state)
        {
            if (string.IsNullOrEmpty(state))
            {
                return new List<string>();
            }

            return _settlementsByCode.Value.Values
                .SelectMany(list => list)
                .Where(a => a.d_estado.Equals(state, StringComparison.OrdinalIgnoreCase)) // ¡FILTRO POR ESTADO!
                .Select(a => a.D_mnpio)
                .Distinct()
                .OrderBy(m => m)  // Orden alfabético
                .ToList();
        }
        public List<string> GetPostalCodesByMunicipalities(string municipality)
        {
            if (string.IsNullOrEmpty(municipality))
            {
                return new List<string>();
            }
            return _settlementsByCode.Value.Values
            .SelectMany(list => list)
            .Where(a => a.D_mnpio.Equals(municipality, StringComparison.OrdinalIgnoreCase))
            .Select(a => a.d_codigo)
            .Distinct()
            .ToList();
        }
        public List<Settlements> GetByCodigo(string code)
        {
            if (_settlementsByCode.Value.TryGetValue(code, out List<Settlements> Settlements))
            {
                return Settlements;
            }
            return new List<Settlements>();
        }
    }
}