namespace AlquileresSistemaPrincipal.Models
{
    public class Propiedad
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Direccion { get; set; }
        public required string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public required string ImagenUrl { get; set; }
    }

}
