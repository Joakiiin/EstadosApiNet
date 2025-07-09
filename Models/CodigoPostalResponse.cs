namespace EstadosApiNet.Models
{
    public class CodigoPostalResponse
    {
        public string cp { get; set; } = null!;
        public List<string> asentamientos { get; set; } = new();
        public List<string> tipos_asentamiento { get; set; }
        public string municipio { get; set; } = null!;
        public string estado { get; set; } = null!;
        public string ciudad { get; set; } = null!;
        public string pais { get; set; } = "México";
    }

}