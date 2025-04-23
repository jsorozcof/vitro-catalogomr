

ALTER TABLE [dbo].[ProductoPromocion]
ADD ProductId NVARCHAR(128);

ALTER TABLE [dbo].ProductoPromocion
ADD CONSTRAINT FK_ProductoPromocion_Product
FOREIGN KEY (ProductId) -- columna en ProductoPromocion
REFERENCES dbo.[Product](ProductId); -- columna en Product


ALTER TABLE [dbo].[ProductoImagen]
ADD ProductId NVARCHAR(128);

ALTER TABLE [dbo].ProductoImagen
ADD CONSTRAINT FK_ProductoImagen_Product
FOREIGN KEY (ProductId) -- columna en ProductoPromocion
REFERENCES dbo.[Product](ProductId); -- columna en Product



CREATE PROCEDURE SP_SaveLogErroresCarga
    @USUARIO NVARCHAR(100),
    @FILA INT,
    @COLUMNA NVARCHAR(100),
    @VALOR_INCORRECTO NVARCHAR(MAX),
    @DESCRIPCION_ERROR NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO LogErroresCarga (USUARIO, FILA, COLUMNA, VALOR_INCORRECTO, DESCRIPCION_ERROR)
    VALUES (@USUARIO, @FILA, @COLUMNA, @VALOR_INCORRECTO, @DESCRIPCION_ERROR);
END;
GO

CREATE TABLE LogErroresCarga (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    FECHA_PROCESO DATETIME DEFAULT GETDATE(),  -- Fecha en que se procesó el archivo
    USUARIO NVARCHAR(100) NULL,                -- Usuario que subió el archivo (si es aplicable)
    FILA INT,                                  -- Número de fila con el error en el Excel
    COLUMNA NVARCHAR(100),                     -- Nombre de la columna con el error
    VALOR_INCORRECTO NVARCHAR(MAX) NULL,       -- Valor que generó el error
    DESCRIPCION_ERROR NVARCHAR(MAX),           -- Descripción clara del error
    ESTADO_CORREGIDO BIT DEFAULT 0             -- 0 = Pendiente, 1 = Corregido
);



-- 1. Crear el tipo de tabla para recibir los datos
CREATE TYPE dbo.ProductType AS TABLE
(	 
    [FILA]	INT,
    [PRODUCTOID] NVARCHAR(128),
	[PAISID] NVARCHAR(128),	  
    [SAP] NVARCHAR (max),
	[NAGS] NVARCHAR (max),
	[MARCAID]  NVARCHAR(128),
	[MODELOID] NVARCHAR (128),
	[DESCRIPCION] NVARCHAR(max),
	[MERCADOID] NVARCHAR (128),
	[COLORID] NVARCHAR(128),
	[TIPOVIDRIOID] NVARCHAR (128),
	[TIPOPARTEID] NVARCHAR (128),
	[ANCHO] FLOAT,
	[ALTO] FLOAT,
	[BOTON] BIT,
	[RED] BIT,
	[SERIGRAFIA] BIT,
	[SENSORLLUVIA] BIT,
	[MOLDURA] BIT,
	[HOLDER] BIT,
	[SENSORCONDENSACION] BIT,
	[HOMOLOGO] BIT,
	[CLASIFICACION]	 NVARCHAR (128),
	[PERFORACION] FLOAT,
	[STARTYEAR] INT,
	[ENDYEAR] INT,
	[PROCEDENCIAID] NVARCHAR(128),
	[ACTIVO] BIT,
	[FECHACREACION] DATETIME,
	[FECHAMODIFICACION] DATETIME,
	[ANTENA] BIT,
	[SUBENSAMBLE] BIT,
	[USUARIO]	 NVARCHAR (128)
);
GO

--INSERT INTO dbo.ErroresCarga(PAIS,SAP,NAGS,MARCA ,MODELO ,STARTYEAR ,ENDYEAR ,DESCRIPCION ,TIPOPARTE ,PERFORACION ,ANCHO ,ALTO ,BOTON ,RED ,SERIGRAFIA ,SENSORLLUVIA ,MOLDURA ,HOLDER, 
											--ANTENA, SUBENSAMBLE,SENSORCONDENSACION ,COLOR ,TIPOVIDRIO ,PROCEDENCIA ,HOMOLOGO ,CLASIFICACION ,MERCADO ,DESCRIPCION_ERRORES)

CREATE TABLE [dbo].ErroresCarga  
(

    [SAP] NVARCHAR (max) NULL,
	[DESCRIPCION_ERRORES] NVARCHAR (max) NULL
	--[NAGS] NVARCHAR (max) NULL,
	--[MARCA] NVARCHAR (128) NULL,
	--[MODELO] NVARCHAR (128) NULL,
	--[STARTYEAR] INT NOT NULL,
	--[ENDYEAR] INT NOT NULL,
	--[DESCRIPCION] NVARCHAR(max) NULL,
	--[TIPOPARTE] NVARCHAR (128) NULL,
	--[PERFORACION] FLOAT NOT NULL,
	--[ANCHO] FLOAT NOT NULL,
	--[ALTO] FLOAT NOT NULL,
	--[BOTON] BIT NOT NULL,
	--[RED] BIT NOT NULL,
	--[SERIGRAFIA] BIT NOT NULL,
	--[SENSORLLUVIA] BIT NOT NULL,
	--[MOLDURA] BIT NOT NULL,
	--[HOLDER] BIT NOT NULL,
	--[ANTENA] BIT NULL,
	--[SUBENSAMBLE] BIT NULL,
	--[SENSORCONDENSACION] BIT NOT NULL,
	--[COLOR] NVARCHAR(128) NULL,
	--[TIPOVIDRIO] NVARCHAR (128) NULL,
	--[PROCEDENCIA] NVARCHAR(128) NULL,
	--[HOMOLOGO] BIT NOT NULL,
	--[CLASIFICACION] NVARCHAR (128) NULL,
	--[MERCADO] NVARCHAR (128) NULL,
	
);
GO



USE [catalogomr]
GO

/****** Object:  Table [dbo].[Product]    Script Date: 19/03/2025 1:58:09 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Product](
	[ProductId] [nvarchar](128) NOT NULL,
	[PaisId] [nvarchar](128) NOT NULL,
	[SAP] [nvarchar](max) NULL,
	[NAGS] [nvarchar](max) NULL,
	[MarcaId] [nvarchar](128) NULL,
	[ModeloId] [nvarchar](128) NULL,
	[Descripcion] [nvarchar](max) NULL,
	[MercadoId] [nvarchar](128) NULL,
	[ColorId] [nvarchar](128) NULL,
	[TipoVidrioId] [nvarchar](128) NULL,
	[TipoParteId] [nvarchar](128) NULL,
	[Ancho] [float] NOT NULL,
	[Alto] [float] NOT NULL,
	[Boton] [bit] NOT NULL,
	[Red] [bit] NOT NULL,
	[Serigrafia] [bit] NOT NULL,
	[SensorLluvia] [bit] NOT NULL,
	[Moldura] [bit] NOT NULL,
	[Holder] [bit] NOT NULL,
	[SensorCondensacion] [bit] NOT NULL,
	[Homologo] [bit] NOT NULL,
	[Clasificacion] [nvarchar](128) NULL,
	[Perforacion] [float] NOT NULL,
	[StartYear] [int] NOT NULL,
	[EndYear] [int] NOT NULL,
	[ProcedenciaId] [nvarchar](128) NULL,
	[Activo] [bit] NOT NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[FechaModificacion] [datetime] NULL,
	[Antena] [bit] NULL,
	[SubEnsamble] [bit] NULL,
	[CreadoPor] [nvarchar](200) NULL,
	[ModificadoPor] [nvarchar](200) NULL,

 CONSTRAINT [PK_dbo.Product] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Product_dbo.Color_ColorId] FOREIGN KEY([ColorId])
REFERENCES [dbo].[Color] ([ColorId])
GO

ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_dbo.Product_dbo.Color_ColorId]
GO

ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Product_dbo.Marca_MarcaId] FOREIGN KEY([MarcaId])
REFERENCES [dbo].[Marca] ([MarcaId])
GO

ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_dbo.Product_dbo.Marca_MarcaId]
GO

ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Product_dbo.Mercado_MercadoId] FOREIGN KEY([MercadoId])
REFERENCES [dbo].[Mercado] ([MercadoId])
GO

ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_dbo.Product_dbo.Mercado_MercadoId]
GO

ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Product_dbo.Modelo_ModeloId] FOREIGN KEY([ModeloId])
REFERENCES [dbo].[Modelo] ([ModeloId])
GO

ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_dbo.Product_dbo.Modelo_ModeloId]
GO

ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Product_dbo.Pais_PaisId] FOREIGN KEY([PaisId])
REFERENCES [dbo].[Pais] ([PaisId])
GO

ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_dbo.Product_dbo.Pais_PaisId]
GO

ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Product_dbo.Procedencia_ProcedenciaId] FOREIGN KEY([ProcedenciaId])
REFERENCES [dbo].[Procedencia] ([ProcedenciaId])
GO

ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_dbo.Product_dbo.Procedencia_ProcedenciaId]
GO

ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Product_dbo.TipoParte_TipoParteId] FOREIGN KEY([TipoParteId])
REFERENCES [dbo].[TipoParte] ([TipoParteId])
GO

ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_dbo.Product_dbo.TipoParte_TipoParteId]
GO

ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Product_dbo.TipoVidrio_TipoVidrioId] FOREIGN KEY([TipoVidrioId])
REFERENCES [dbo].[TipoVidrio] ([TipoVidrioId])
GO

ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_dbo.Product_dbo.TipoVidrio_TipoVidrioId]
GO





