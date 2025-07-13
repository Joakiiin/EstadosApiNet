namespace EstadosApiNet.Models
{
    public class NonGroupedPostalCodeResponse
    {
        public string cp { get; set; }
        public string asentamiento { get; set; }
        public string tipo_asentamiento { get; set; }
        public string municipio { get; set; }
        public string estado { get; set; }
        public string ciudad { get; set; }
        public string pais { get; set; } = "México";
    }

}