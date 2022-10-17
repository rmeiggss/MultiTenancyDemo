namespace MultiTenancyDemo.Entidades
{
    public class Pais: IEntidadComun
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
    }
}
