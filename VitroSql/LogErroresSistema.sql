CREATE TABLE LogErroresSistema (
    ID INT IDENTITY(1,1) PRIMARY KEY,  -- Identificador �nico autoincremental
	Usuario	NVARCHAR(200) NULL, -- usuario que carga el excel
    NumeroError INT NOT NULL,           -- C�digo del error
    Linea INT NOT NULL,                 -- L�nea donde ocurri� el error
    Procedimiento NVARCHAR(200) NULL,   -- Nombre del procedimiento almacenado
    Mensaje NVARCHAR(MAX) NOT NULL,     -- Descripci�n del error
    Fecha DATETIME DEFAULT GETDATE()    -- Fecha y hora del error
);