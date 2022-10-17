using MultiTenancyDemo.Entidades;

namespace MultiTenancyDemo.Models
{
    public class HomeIndexViewModel
    {
        public IEnumerable<Producto> Productos { get; set; } = new List<Producto>();
        public IEnumerable<Pais> Paises { get; set; } = new List<Pais>();
    }
}
