namespace Vitro.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Vitro.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<Vitro.Models.ApplicationDbContext>
    {
        //CreateDatabaseIfNotExists, DbMigrationsConfiguration
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Vitro.Models.ApplicationDbContext context)
        {
            if (!context.Roles.Any())
            {
                context.Roles.Add(new IdentityRole() { Id = $"{Guid.NewGuid()}", Name = "Administrador" });
                context.Roles.Add(new IdentityRole() { Id = $"{Guid.NewGuid()}", Name = "Cliente" });
                context.Roles.Add(new IdentityRole() { Id = $"{Guid.NewGuid()}", Name = "Comercial" });
                context.Roles.Add(new IdentityRole() { Id = $"{Guid.NewGuid()}", Name = "Mercadotecnia" });
                context.Roles.Add(new IdentityRole() { Id = $"{Guid.NewGuid()}", Name = "Ingenieria" });
            }

            if (!context.Users.Any())
            {
                var container = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(container);
                var fingerprint = new VitroCore.EncodeHashManager().EncodeHash("Temporal123");
                var pais = context.Paises.Add(new VitroSql.Pais() { PaisId = $"{Guid.NewGuid()}", Nombre = "COLOMBIA", Activo = true });

                var user = new ApplicationUser() { Id = $"{Guid.NewGuid()}", UserName = "administrador", Email = "administrador@email.com", FullName = "SYSADMIN", FingerPrint = fingerprint, PaisId = pais.PaisId };
                manager.Create(user, "Temporal123");
                manager.AddToRole(user.Id, "Administrador");
            }

            if (!context.Configuraciones.Any())
            {
                context.Configuraciones.Add(new VitroSql.Configuracion() { ConfiguracionId = $"{Guid.NewGuid()}", DiasVigenciaNuevosProductos = 10 });
            }
        }

        //public override void InitializeDatabase(ApplicationDbContext context)
        //{
        //    base.InitializeDatabase(context);
        //}
    }
}
