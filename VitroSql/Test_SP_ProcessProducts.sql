DECLARE @CODIGOATRIBUTO NVARCHAR(128) = '' 
DECLARE @PAISID	  NVARCHAR(128)

ALTER TABLE ProductoImagen DROP CONSTRAINT FK_dbo.ProductoImagen_dbo.Imagen_ImagenId;
ALTER TABLE ProductImages DROP CONSTRAINT FK_ProductImages_Product;

SELECT 
    fk.name AS ConstraintName,
    tp.name AS ParentTable,
    cp.name AS ParentColumn,
    tr.name AS ReferencedTable,
    cr.name AS ReferencedColumn
FROM sys.foreign_keys fk
INNER JOIN sys.foreign_key_columns fkc ON fkc.constraint_object_id = fk.object_id
INNER JOIN sys.tables tp ON fk.parent_object_id = tp.object_id
INNER JOIN sys.columns cp ON fkc.parent_column_id = cp.column_id AND cp.object_id = tp.object_id
INNER JOIN sys.tables tr ON fk.referenced_object_id = tr.object_id
INNER JOIN sys.columns cr ON fkc.referenced_column_id = cr.column_id AND cr.object_id = tr.object_id
WHERE tp.name = 'ProductoPromocion'; -- Cambia por el nombre de tu tabla

TRUNCATE TABLE dbo.Product;

ALTER TABLE [dbo].ProductImages
ADD CONSTRAINT FK_ProductImages_Product
FOREIGN KEY (ProductId) -- columna en ProductoPromocion
REFERENCES dbo.[Product](ProductId); -- columna en Product

ALTER TABLE [dbo].ProductoImagen
ADD CONSTRAINT FK_ProductoImagen_Product
FOREIGN KEY (ProductId) -- columna en ProductoPromocion
REFERENCES dbo.[Product](ProductId); -- columna en Product


truncate table dbo.TempProducto
truncate table dbo.Product
DELETE T
FROM dbo.Product AS T
INNER JOIN [dbo].[ProductoPromocion] AS J ON T.ProductId = J.ProductId
INNER JOIN [dbo].[ProductoImagen] AS M ON M.ProductId = J.ProductId
WHERE T.ProductId IN (
'5a62eb01-1e69-4d60-aeb0-ba822ca660e3',
'6ce8b249-8920-4cee-9ce9-e88a2026412c',
'b2ae4a51-957b-4f04-bb8a-22128c3f8e5b'
);
DELETE H
FROM dbo.HistoricoCargue AS H
WHERE H.SAP IN (
'2000107187',
'2000099932',
'2000120400'
);

truncate table dbO.LogErroresSistema

select * from dbo.TempProducto
select * from dbo.Product
SELECT *
  FROM [catalogomr].[dbo].[ProductImages]	where SAP = '2000120400'
  where SAP = '2000099932' ORDER BY Posicion asc
select * from LogErroresSistema
								  
drop table 	[catalogomr].[dbo].[ProductImages]
drop table 	[catalogomr].[dbo].[Product]



DECLARE @DATA dbo.ProductType;

-- Insertar datos de prueba en la tabla tipo TVP
INSERT INTO @DATA (PRODUCTOID, PAISID, SAP, NAGS, MARCAID, MODELOID, DESCRIPCION, MERCADOID, COLORID, TIPOVIDRIOID, TIPOPARTEID,ANCHO, ALTO, BOTON, RED, 
SERIGRAFIA, SENSORLLUVIA, MOLDURA, HOLDER, SENSORCONDENSACION, HOMOLOGO,CLASIFICACION, PERFORACION, STARTYEAR, ENDYEAR, PROCEDENCIAID, ACTIVO, FECHACREACION, 
FECHAMODIFICACION,ANTENA, SUBENSAMBLE, USUARIO)
VALUES (NEWID(), 'COLOMBIA', '2000107187', 'DQ13218GTN', 'FORD', 'ESCAPE', 'DQ13218GTN CTOTI FORESCAPE 20- CGAMRN  ', 'AUTOMOTRIZ', 'VERDE SIN BANDA', 
'TEMPLADO', 'CUARTO TRASERO IZQUIERDO',393,356,0, 0, 1, 0, 0, 0, 0, 0, 'LATERALES', 1, 2020, 2023, 'CHINA', 1, GETDATE(), NULL, 0, 0,'Test');



INSERT INTO @DATA (PRODUCTOID, PAISID, SAP, NAGS, MARCAID, MODELOID, DESCRIPCION, MERCADOID, COLORID, TIPOVIDRIOID, TIPOPARTEID,ANCHO, ALTO, BOTON, RED, 
SERIGRAFIA, SENSORLLUVIA, MOLDURA, HOLDER, SENSORCONDENSACION, HOMOLOGO,CLASIFICACION, PERFORACION, STARTYEAR, ENDYEAR, PROCEDENCIAID, ACTIVO, FECHACREACION, 
FECHAMODIFICACION,ANTENA, SUBENSAMBLE, USUARIO)
VALUES (NEWID(), 'COLOMBIA', '2000099932', 'CW908774CLNR', 'BYD', 'ALIMENTADOR', 'CW908774CLN PBSDI ICC1160*1446 VITMRN', 'CARROCERO', 'INCOLORO SIN BANDA', 
'LAMINADO', 'PARABRISAS DELANTERO', 
1160,1446, 0, 0, 1, 0, 0, 0, 0, 0, 'PARABRISA', 0, 2021, 2022, 'COLOMBIA', 1,
GETDATE(), NULL, 0, 0,'Test');


INSERT INTO @DATA (PRODUCTOID, PAISID, SAP, NAGS, MARCAID, MODELOID, DESCRIPCION, MERCADOID, COLORID, TIPOVIDRIOID, TIPOPARTEID,ANCHO, ALTO, BOTON, RED, 
SERIGRAFIA, SENSORLLUVIA, MOLDURA, HOLDER, SENSORCONDENSACION, HOMOLOGO,CLASIFICACION, PERFORACION, STARTYEAR, ENDYEAR, PROCEDENCIAID, ACTIVO, FECHACREACION, 
FECHAMODIFICACION,ANTENA, SUBENSAMBLE, USUARIO)
VALUES (NEWID(), 'COLOMBIA', '2000120400', 'CD911532GTNI', 'VOLKSWAGEN', 'CRAFTER', 'CD911532GTN PTADI VOLCRAFTER 17- CGAMRN', 'AUTOMOTRIZ', 'VERDE SIN BANDA', 
'TEMPLADO', 'PUERTA DELANTERA IZQUIERDA', 
509,845, 0, 0, 0, 0, 0, 0, 0, 0, 'LATERALES
', 1, 2017, 2024, 'CHINA', 1,
GETDATE(), NULL, 0, 0,'Test');


-- Ejecutar el procedimiento almacenado con datos de prueba
EXEC dbo.SP_ProcessProducts_v2
    @PAIS = 'COLOMBIA',
    @USERNAME = 'AdminTest',
    @ACTUALIZAPRODUCTOS = 0,
    @DATA = @DATA;
