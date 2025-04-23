using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Vitro.Models
{
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }

        public string FullName { get; set; }
        public bool Disable { get; set; }
        public string PaisId { get; set; }
        public string FingerPrint { get; set; }

        public VitroSql.Pais Pais { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("VitroContext", throwIfV1Schema: false)
        {
            //Database.SetInitializer(new Migrations.Configuration());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        
        public DbSet<VitroSql.Marca> Marcas { get; set; }
        public DbSet<VitroSql.ProductImages> ProductImages { get; set; }
        public DbSet<VitroSql.Mercado> Mercados { get; set; }
        public DbSet<VitroSql.Modelo> Modelos { get; set; }
        public DbSet<VitroSql.Pais> Paises { get; set; }
        public DbSet<VitroSql.Producto> Productos { get; set; }
        public DbSet<VitroSql.TbProduct> TbProduct { get; set; }
        public DbSet<VitroSql.TbLogErroresCarga> TbLogErroresCarga { get; set; }
        
        public DbSet<VitroSql.TbLogErrores> TbLogErrores { get; set; }
        public DbSet<VitroSql.TipoParte> TipoPartes { get; set; }
        public DbSet<VitroSql.TipoVidrio> TipoVidrios { get; set; }
        public DbSet<VitroSql.Color> Colores { get; set; }
        public DbSet<VitroSql.Procedencia> Procedencias { get; set; }
        public DbSet<VitroSql.Clasificacion> Clasificaciones { get; set; }
        public DbSet<VitroSql.Imagen> Imagenes { get; set; }
        public DbSet<VitroSql.ProductoImagen> ProductoImagenes { get; set; }
        public DbSet<VitroSql.Sugerencia> Sugerencias { get; set; }
        public DbSet<VitroSql.TempProducto> TemporalProductos { get; set; }
        public DbSet<VitroSql.HistoricoCargue> HistoricoCargues { get; set; }
        public DbSet<VitroSql.ProductoPromocion> ProductoPromociones { get; set; }
        public DbSet<VitroSql.Configuracion> Configuraciones { get; set; }
        public DbSet<VitroSql.ImagenCargue> ImagenesCargue { get; set; }
        public DbSet<VitroSql.MailConfig> MailConfigs { get; set; }
        public DbSet<VitroSql.LogUserAccount> LogUserAccounts { get; set; }
    }
}