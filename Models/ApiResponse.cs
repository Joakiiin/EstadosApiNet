namespace EstadosApiNet.Models
{
    public class ApiResponse<T>
    {
        public bool error { get; set; }
        public int code_error { get; set; }
        public string? error_message { get; set; }
        public T? response { get; set; }
    }
}