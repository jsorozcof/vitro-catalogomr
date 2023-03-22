namespace Vitro.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReleaseDatabaseSchema : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clasificacion",
                c => new
                    {
                        ClasificacionId = c.String(nullable: false, maxLength: 128),
                        Nombre = c.String(),
                        Activo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ClasificacionId);
            
            CreateTable(
                "dbo.Color",
                c => new
                    {
                        ColorId = c.String(nullable: false, maxLength: 128),
                        Nombre = c.String(),
                        Codigo = c.String(),
                        Activo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ColorId);
            
            CreateTable(
                "dbo.Configuracion",
                c => new
                    {
                        ConfiguracionId = c.String(nullable: false, maxLength: 128),
                        DiasVigenciaNuevosProductos = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ConfiguracionId);
            
            CreateTable(
                "dbo.HistoricoCargue",
                c => new
                    {
                        IdRegistro = c.String(nullable: false, maxLength: 128),
                        SAP = c.String(),
                        NAGS = c.String(),
                        ModeloId = c.String(),
                        Descripcion = c.String(),
                        MercadoId = c.String(),
                        ColorId = c.String(),
                        TipoVidrioId = c.String(),
                        TipoParteId = c.String(),
                        Ancho = c.Double(nullable: false),
                        Alto = c.Double(nullable: false),
                        Boton = c.Boolean(nullable: false),
                        Red = c.Boolean(nullable: false),
                        Serigrafia = c.Boolean(nullable: false),
                        SensorLluvia = c.Boolean(nullable: false),
                        Moldura = c.Boolean(nullable: false),
                        Holder = c.Boolean(nullable: false),
                        Antena = c.Boolean(nullable: false),
                        SubEnsamble = c.Boolean(nullable: false),
                        SensorCondensacion = c.Boolean(nullable: false),
                        Homologo = c.Boolean(nullable: false),
                        Perforacion = c.Double(nullable: false),
                        StartYear = c.Int(nullable: false),
                        EndYear = c.Int(nullable: false),
                        ProcedenciaId = c.String(),
                        Activo = c.Boolean(nullable: false),
                        FechaCreacion = c.DateTime(),
                        Valido = c.Boolean(nullable: false),
                        IdCargue = c.Int(nullable: false),
                        UserName = c.String(),
                        Resultado = c.String(),
                    })
                .PrimaryKey(t => t.IdRegistro);
            
            CreateTable(
                "dbo.Imagen",
                c => new
                    {
                        ImagenId = c.String(nullable: false, maxLength: 128),
                        Indice = c.Int(nullable: false),
                        Nombre = c.String(),
                        ImageType = c.String(),
                        ImageSize = c.Long(nullable: false),
                        FechaCreacion = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ImagenId);
            
            CreateTable(
                "dbo.ImagenCargue",
                c => new
                    {
                        ImagenId = c.String(nullable: false, maxLength: 128),
                        Indice = c.Int(nullable: false),
                        Nombre = c.String(),
                        CargueRef = c.String(),
                        ImageSize = c.Long(nullable: false),
                        ImageType = c.String(),
                        Procesado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ImagenId);
            
            CreateTable(
                "dbo.LogUserAccount",
                c => new
                    {
                        LogUserAccountId = c.String(nullable: false, maxLength: 128),
                        FechaHora = c.DateTime(nullable: false),
                        UserName = c.String(),
                        IpAddress = c.String(),
                    })
                .PrimaryKey(t => t.LogUserAccountId);
            
            CreateTable(
                "dbo.MailConfig",
                c => new
                    {
                        MailConfigId = c.String(nullable: false, maxLength: 128),
                        Puerto = c.Int(nullable: false),
                        HabilitarSSL = c.Boolean(),
                        Host = c.String(),
                        MailAccount = c.String(),
                        MailPassword = c.String(),
                    })
                .PrimaryKey(t => t.MailConfigId);
            
            CreateTable(
                "dbo.Marca",
                c => new
                    {
                        MarcaId = c.String(nullable: false, maxLength: 128),
                        Nombre = c.String(),
                        Activo = c.Boolean(nullable: false),
                        PaisId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.MarcaId)
                .ForeignKey("dbo.Pais", t => t.PaisId)
                .Index(t => t.PaisId);
            
            CreateTable(
                "dbo.Pais",
                c => new
                    {
                        PaisId = c.String(nullable: false, maxLength: 128),
                        Nombre = c.String(),
                        Activo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PaisId);
            
            CreateTable(
                "dbo.Mercado",
                c => new
                    {
                        MercadoId = c.String(nullable: false, maxLength: 128),
                        Nombre = c.String(),
                        Descripcion = c.String(),
                        Activo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.MercadoId);
            
            CreateTable(
                "dbo.Modelo",
                c => new
                    {
                        ModeloId = c.String(nullable: false, maxLength: 128),
                        Nombre = c.String(),
                        MarcaId = c.String(maxLength: 128),
                        Activo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ModeloId)
                .ForeignKey("dbo.Marca", t => t.MarcaId)
                .Index(t => t.MarcaId);
            
            CreateTable(
                "dbo.Procedencia",
                c => new
                    {
                        ProcedenciaId = c.String(nullable: false, maxLength: 128),
                        Nombre = c.String(),
                        Activo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ProcedenciaId);
            
            CreateTable(
                "dbo.ProductoImagen",
                c => new
                    {
                        ProductoImagenId = c.String(nullable: false, maxLength: 128),
                        ProductoId = c.String(maxLength: 128),
                        ImagenId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ProductoImagenId)
                .ForeignKey("dbo.Imagen", t => t.ImagenId)
                .ForeignKey("dbo.Producto", t => t.ProductoId)
                .Index(t => t.ProductoId)
                .Index(t => t.ImagenId);
            
            CreateTable(
                "dbo.Producto",
                c => new
                    {
                        ProductoId = c.String(nullable: false, maxLength: 128),
                        SAP = c.String(),
                        NAGS = c.String(),
                        ModeloId = c.String(maxLength: 128),
                        Descripcion = c.String(),
                        MercadoId = c.String(maxLength: 128),
                        ColorId = c.String(maxLength: 128),
                        TipoVidrioId = c.String(maxLength: 128),
                        TipoParteId = c.String(maxLength: 128),
                        Ancho = c.Double(nullable: false),
                        Alto = c.Double(nullable: false),
                        Boton = c.Boolean(nullable: false),
                        Red = c.Boolean(nullable: false),
                        Serigrafia = c.Boolean(nullable: false),
                        SensorLluvia = c.Boolean(nullable: false),
                        Moldura = c.Boolean(nullable: false),
                        Holder = c.Boolean(nullable: false),
                        SensorCondensacion = c.Boolean(nullable: false),
                        Homologo = c.Boolean(nullable: false),
                        Perforacion = c.Double(nullable: false),
                        StartYear = c.Int(nullable: false),
                        EndYear = c.Int(nullable: false),
                        ProcedenciaId = c.String(maxLength: 128),
                        Activo = c.Boolean(nullable: false),
                        Antena = c.Boolean(nullable: false),
                        SubEnsamble = c.Boolean(nullable: false),
                        FechaCreacion = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(),
                    })
                .PrimaryKey(t => t.ProductoId)
                .ForeignKey("dbo.Color", t => t.ColorId)
                .ForeignKey("dbo.Mercado", t => t.MercadoId)
                .ForeignKey("dbo.Modelo", t => t.ModeloId)
                .ForeignKey("dbo.Procedencia", t => t.ProcedenciaId)
                .ForeignKey("dbo.TipoParte", t => t.TipoParteId)
                .ForeignKey("dbo.TipoVidrio", t => t.TipoVidrioId)
                .Index(t => t.ModeloId)
                .Index(t => t.MercadoId)
                .Index(t => t.ColorId)
                .Index(t => t.TipoVidrioId)
                .Index(t => t.TipoParteId)
                .Index(t => t.ProcedenciaId);
            
            CreateTable(
                "dbo.TipoParte",
                c => new
                    {
                        TipoParteId = c.String(nullable: false, maxLength: 128),
                        Nombre = c.String(),
                        ClasificacionId = c.String(maxLength: 128),
                        Activo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.TipoParteId)
                .ForeignKey("dbo.Clasificacion", t => t.ClasificacionId)
                .Index(t => t.ClasificacionId);
            
            CreateTable(
                "dbo.TipoVidrio",
                c => new
                    {
                        TipoVidrioId = c.String(nullable: false, maxLength: 128),
                        Nombre = c.String(),
                        Activo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.TipoVidrioId);
            
            CreateTable(
                "dbo.ProductoPromocion",
                c => new
                    {
                        PromocionId = c.String(nullable: false, maxLength: 128),
                        FechaInicio = c.DateTime(nullable: false),
                        FechaFinal = c.DateTime(nullable: false),
                        DiasVigencia = c.Int(nullable: false),
                        ProductoId = c.String(maxLength: 128),
                        Precio = c.Double(nullable: false),
                        Stock = c.Int(nullable: false),
                        FechaCreacion = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(),
                    })
                .PrimaryKey(t => t.PromocionId)
                .ForeignKey("dbo.Producto", t => t.ProductoId)
                .Index(t => t.ProductoId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Sugerencia",
                c => new
                    {
                        SugerenciaId = c.String(nullable: false, maxLength: 128),
                        Marca = c.String(),
                        Modelo = c.String(),
                        TipoParte = c.String(),
                        Descripcion = c.String(),
                        FechaCreacion = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SugerenciaId);
            
            CreateTable(
                "dbo.TempProducto",
                c => new
                    {
                        ProductoId = c.String(nullable: false, maxLength: 128),
                        Pais = c.String(),
                        SAP = c.String(),
                        NAGS = c.String(),
                        MarcaId = c.String(),
                        ModeloId = c.String(),
                        Descripcion = c.String(),
                        MercadoId = c.String(),
                        ColorId = c.String(),
                        TipoVidrioId = c.String(),
                        TipoParteId = c.String(),
                        Ancho = c.Double(nullable: false),
                        Alto = c.Double(nullable: false),
                        Boton = c.Boolean(nullable: false),
                        Red = c.Boolean(nullable: false),
                        Serigrafia = c.Boolean(nullable: false),
                        SensorLluvia = c.Boolean(nullable: false),
                        Moldura = c.Boolean(nullable: false),
                        Holder = c.Boolean(nullable: false),
                        Antena = c.Boolean(nullable: false),
                        SubEnsamble = c.Boolean(nullable: false),
                        SensorCondensacion = c.Boolean(nullable: false),
                        Homologo = c.Boolean(nullable: false),
                        Perforacion = c.Double(nullable: false),
                        ClasificacionId = c.String(),
                        StartYear = c.Int(nullable: false),
                        EndYear = c.Int(nullable: false),
                        ProcedenciaId = c.String(),
                        Activo = c.Boolean(nullable: false),
                        RefImagen = c.String(),
                        Procesado = c.Boolean(nullable: false),
                        Valido = c.Boolean(nullable: false),
                        FechaCreacion = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ProductoId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FullName = c.String(),
                        Disable = c.Boolean(nullable: false),
                        PaisId = c.String(maxLength: 128),
                        FingerPrint = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pais", t => t.PaisId)
                .Index(t => t.PaisId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "PaisId", "dbo.Pais");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.ProductoPromocion", "ProductoId", "dbo.Producto");
            DropForeignKey("dbo.ProductoImagen", "ProductoId", "dbo.Producto");
            DropForeignKey("dbo.Producto", "TipoVidrioId", "dbo.TipoVidrio");
            DropForeignKey("dbo.Producto", "TipoParteId", "dbo.TipoParte");
            DropForeignKey("dbo.TipoParte", "ClasificacionId", "dbo.Clasificacion");
            DropForeignKey("dbo.Producto", "ProcedenciaId", "dbo.Procedencia");
            DropForeignKey("dbo.Producto", "ModeloId", "dbo.Modelo");
            DropForeignKey("dbo.Producto", "MercadoId", "dbo.Mercado");
            DropForeignKey("dbo.Producto", "ColorId", "dbo.Color");
            DropForeignKey("dbo.ProductoImagen", "ImagenId", "dbo.Imagen");
            DropForeignKey("dbo.Modelo", "MarcaId", "dbo.Marca");
            DropForeignKey("dbo.Marca", "PaisId", "dbo.Pais");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "PaisId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.ProductoPromocion", new[] { "ProductoId" });
            DropIndex("dbo.TipoParte", new[] { "ClasificacionId" });
            DropIndex("dbo.Producto", new[] { "ProcedenciaId" });
            DropIndex("dbo.Producto", new[] { "TipoParteId" });
            DropIndex("dbo.Producto", new[] { "TipoVidrioId" });
            DropIndex("dbo.Producto", new[] { "ColorId" });
            DropIndex("dbo.Producto", new[] { "MercadoId" });
            DropIndex("dbo.Producto", new[] { "ModeloId" });
            DropIndex("dbo.ProductoImagen", new[] { "ImagenId" });
            DropIndex("dbo.ProductoImagen", new[] { "ProductoId" });
            DropIndex("dbo.Modelo", new[] { "MarcaId" });
            DropIndex("dbo.Marca", new[] { "PaisId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.TempProducto");
            DropTable("dbo.Sugerencia");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.ProductoPromocion");
            DropTable("dbo.TipoVidrio");
            DropTable("dbo.TipoParte");
            DropTable("dbo.Producto");
            DropTable("dbo.ProductoImagen");
            DropTable("dbo.Procedencia");
            DropTable("dbo.Modelo");
            DropTable("dbo.Mercado");
            DropTable("dbo.Pais");
            DropTable("dbo.Marca");
            DropTable("dbo.MailConfig");
            DropTable("dbo.LogUserAccount");
            DropTable("dbo.ImagenCargue");
            DropTable("dbo.Imagen");
            DropTable("dbo.HistoricoCargue");
            DropTable("dbo.Configuracion");
            DropTable("dbo.Color");
            DropTable("dbo.Clasificacion");
        }
    }
}
