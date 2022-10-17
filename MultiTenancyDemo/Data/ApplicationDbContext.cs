using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MultiTenancyDemo.Entidades;
using MultiTenancyDemo.Servicios;
using System.Linq.Expressions;

namespace MultiTenancyDemo.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        private string tenantId;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
            IServicioTenant servicioTenant)
            : base(options)
        {
            tenantId = servicioTenant.ObtenerTenant();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var item in ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added && e.Entity is IEntidadTenant))
            {
                if (string.IsNullOrEmpty(tenantId))
                {
                    throw new Exception("TenantId no encontrado al momento de crear el registro");
                }

                var entidad = item.Entity as IEntidadTenant;
                entidad!.TenantId = tenantId;
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Pais>().HasData(new Entidades.Pais[]
            {
                new Pais{ Id = 1, Nombre= "Republica Dominicana"},
                new Pais{ Id = 2, Nombre= "Mexico"},
                new Pais{ Id = 3, Nombre= "Colombia"}
            });

            foreach (var entidad in builder.Model.GetEntityTypes())
            {
                var tipo = entidad.ClrType;

                if (typeof(IEntidadTenant).IsAssignableFrom(tipo))
                {
                    var metodo = typeof(ApplicationDbContext)
                        .GetMethod(nameof(ArmarFiltroGlobalTenant),
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)?.MakeGenericMethod(tipo);

                    var filtro = metodo?.Invoke(null, new object[] { this })!;

                    entidad.SetQueryFilter((LambdaExpression)filtro);
                    entidad.AddIndex(entidad.FindProperty(nameof(IEntidadTenant.TenantId))!);
                }
                else if (tipo.DebeSaltarValidacionTenant())
                {
                    continue;
                }
                else
                {
                    throw new Exception($"La entidad {entidad} no ha sido marcada como tenant o común");
                }
            }
        }

        private static LambdaExpression ArmarFiltroGlobalTenant<TEntidad>(
            ApplicationDbContext context)
            where TEntidad : class, IEntidadTenant
        {
            Expression<Func<TEntidad, bool>> filtro = x => x.TenantId == context.tenantId;
            return filtro;
        }

        public DbSet<Producto> Productos => Set<Producto>();
        public DbSet<Pais> Paises => Set<Pais>();
    }
}