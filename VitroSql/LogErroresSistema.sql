CREATE TABLE LogErroresSistema (
    ID INT IDENTITY(1,1) PRIMARY KEY,  -- Identificador único autoincremental
	Usuario	NVARCHAR(200) NULL, -- usuario que carga el excel
    NumeroError INT NOT NULL,           -- Código del error
    Linea INT NOT NULL,                 -- Línea donde ocurrió el error
    Procedimiento NVARCHAR(200) NULL,   -- Nombre del procedimiento almacenado
    Mensaje NVARCHAR(MAX) NOT NULL,     -- Descripción del error
    Fecha DATETIME DEFAULT GETDATE()    -- Fecha y hora del error
);