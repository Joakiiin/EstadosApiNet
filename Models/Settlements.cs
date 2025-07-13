namespace EstadosApiNet.Models
{
    public class Settlements
    {
        public string d_codigo { get; set; } = null!;
        public string d_asenta { get; set; } = null!;
        public string d_tipo_asenta { get; set; } = null!;
        public string D_mnpio { get; set; } = null!;
        public string d_estado { get; set; } = null!;
        public string d_ciudad { get; set; } = null!;
        public string d_CP { get; set; } = null!;
        public string c_estado { get; set; } = null!;
        public string c_oficina { get; set; } = null!;
        public string c_CP { get; set; } = null!;
        public string c_tipo_asenta { get; set; } = null!;
        public string c_mnpio { get; set; } = null!;
        public string id_asenta_cpcons { get; set; } = null!;
        public string d_zona { get; set; } = null!;
        public string c_cve_ciudad { get; set; } = null!;
    }
}