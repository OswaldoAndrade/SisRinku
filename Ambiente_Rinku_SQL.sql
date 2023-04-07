USE master
GO
IF NOT EXISTS (
   SELECT name
   FROM sys.databases
   WHERE name = N'SisRinku'
)
CREATE DATABASE [SisRinku]
GO

--EJECUTAR HASTA AQUI PARA CREAR LA BD




--EJECUTAR TODO DESPUES DE CREAR LA BD
USE SisRinku
GO

IF OBJECT_ID('dbo.[PK_CtlMovimientos]', 'PK') IS NOT NULL AND OBJECT_ID('dbo.[FK_CatEmpleados]', 'F') IS NOT NULL
BEGIN
	ALTER TABLE CtlMovimientos
	DROP CONSTRAINT IF EXISTS PK_CtlMovimientos, FK_CatEmpleados
END 

IF OBJECT_ID('dbo.[PK_CatEmpleados]', 'PK') IS NOT NULL AND OBJECT_ID('dbo.[FK_CatRoles]', 'F') IS NOT NULL
BEGIN
	ALTER TABLE CatEmpleados
	DROP CONSTRAINT IF EXISTS PK_CatEmpleados, FK_CatRoles
END 

IF OBJECT_ID('dbo.[PK_CatRoles]', 'PK') IS NOT NULL
BEGIN
	ALTER TABLE CatRoles
	DROP CONSTRAINT IF EXISTS PK_CatRoles
END 
 


DROP TABLE IF EXISTS CatRoles
CREATE TABLE CatRoles
(
	Id SMALLINT IDENTITY(1,1),
	Descripcion VARCHAR(50) NOT NULL,
	Estatus INT NOT NULL DEFAULT 1,
	CONSTRAINT PK_CatRoles PRIMARY KEY (Id)
)	

INSERT INTO CatRoles(Descripcion)
VALUES('Chofer'),('Cargador'),('Auxiliar')


DROP TABLE IF EXISTS CatEmpleados
CREATE TABLE CatEmpleados
(	
	Id INT IDENTITY(1,1),
	Nombre VARCHAR(100) NOT NULL,
	IdRol SMALLINT NOT NULL DEFAULT 1,
	Estatus BIT NOT NULL DEFAULT 1,
	CONSTRAINT PK_CatEmpleados PRIMARY KEY (Id),
	CONSTRAINT FK_CatRoles FOREIGN KEY (IdRol) REFERENCES CatRoles(Id) 
)

DROP TABLE IF EXISTS CtlMovimientos
CREATE TABLE CtlMovimientos
(
	Id INT IDENTITY(1,1),
	IdEmpleado INT NOT NULL DEFAULT 1,
	Mes TINYINT NOT NULL,
	Entregas SMALLINT NOT NULL,
	SueldoBruto DECIMAL(10,2),
	Retenciones DECIMAL(10,2),
	BonoDespensa DECIMAL(10,2),
	SueldoNeto DECIMAL(10,2)
	CONSTRAINT PK_CtlMovimientos PRIMARY KEY (Id,IdEmpleado,Mes),
	CONSTRAINT FK_CatEmpleados FOREIGN KEY (IdEmpleado) REFERENCES CatEmpleados(Id) 
)

GO
DROP PROCEDURE IF EXISTS GetRoles
GO
CREATE PROCEDURE GetRoles
AS 
BEGIN
	SELECT Id, Descripcion, Estatus FROM CatRoles(NOLOCK) 
END
GO
DROP PROCEDURE IF EXISTS GetEmpleados
GO
CREATE PROCEDURE GetEmpleados
AS 
BEGIN
	SELECT 
		Id = e.Id, 
		Nombre = e.Nombre, 
		IdRol = e.IdRol, 
		Rol = r.Descripcion,
		Estatus = e.Estatus
	FROM CatEmpleados(NOLOCK) e
	INNER JOIN CatRoles(NOLOCK) r ON r.Id = e.IdRol 
	WHERE e.Estatus = 1
END
GO 
DROP FUNCTION IF EXISTS CalculaSueldoEmpleado 
GO
CREATE FUNCTION CalculaSueldoEmpleado (@IdEmpleado INT, @IdROl INT, @Entregas INT)
RETURNS @tmpSueldo TABLE(SueldoBase DECIMAL(10,2), BonoEntregas DECIMAL(10,2), BonoHora DECIMAL(10,2), RetencionISR DECIMAL(10,2),
						BonoDespensa DECIMAL(10,2), TotalPercepcion DECIMAL(10,2), TotalNeto DECIMAL(10,2))
AS 
BEGIN
	
	DECLARE @SueldoBase DECIMAL(10,2)
			,@BonoEntregas DECIMAL(10,2)
			,@BonoHora DECIMAL(10,2)
			,@TotalBruto DECIMAL(10,2)
			,@RetencionISR DECIMAL(10,2)
			,@BonoDespensa DECIMAL(10,2)
			,@TotalNeto DECIMAL(10,2)
			,@HorasPorMes INT
			
	

	SET @HorasPorMes  = 8 * 6 * 4 --Horas al dia *  Dias a la semana * Semanas al mes
	SET @SueldoBase = 30 * @HorasPorMes --Cobro por hora * Horas al mes
	SET @BonoEntregas = @Entregas * 5
	SET @BonoHora = CASE @IdRol WHEN 1 THEN 10 * @HorasPorMes
								   WHEN 2 THEN 5 * @HorasPorMes
								   WHEN 3 THEN 0
								   END
	SET @TotalBruto = @SueldoBase + @BonoEntregas + @BonoHora
	
	SELECT @RetencionISR = IIF(@TotalBruto > 10000,  ROUND(@TotalBruto * .12, 2), ROUND(@TotalBruto * .09, 2) ) --Si el sueldo es mayor a 10,000 cobrar 12% de ISR, sino 9%

	SET @BonoDespensa = ROUND((@TotalBruto - @RetencionISR) * 0.04, 2) --Vale de despensa de 4%

	SET @TotalNeto = ROUND(@TotalBruto + @BonoDespensa - @RetencionISR, 2) --El sueldo neto es el total bruto + Vale de despensa - ISR



	INSERT INTO @tmpSueldo(SueldoBase, BonoEntregas, BonoHora, RetencionISR, BonoDespensa, TotalPercepcion, TotalNeto)
					VALUES(@SueldoBase, @BonoEntregas, @BonoHora, @RetencionISR, @BonoDespensa, @TotalBruto, @TotalNeto)

	RETURN
END
GO
GO
DROP PROCEDURE IF EXISTS GetMovimientos
GO
CREATE PROCEDURE GetMovimientos
AS 
BEGIN
	SELECT 
		Id = m.Id,
		IdEmpleado = e.Id,
		Nombre = e.Nombre,
		Rol = r.Descripcion,
		Mes = m.Mes, 
		Entregas = m.Entregas,
		SueldoBruto = m.SueldoBruto,
		Retenciones = m.Retenciones,
		BonoDespensa = m.BonoDespensa,
		SueldoNeto = m.SueldoNeto
	FROM CtlMovimientos(NOLOCK) m
	INNER JOIN CatEmpleados(NOLOCK) e ON e.Id = m.IdEmpleado 
	INNER JOIN CatRoles(NOLOCK) r ON r.Id = e.IdRol
END
GO
DROP PROCEDURE IF EXISTS AddEmpleado
GO
CREATE PROCEDURE AddEmpleado 
@Nombre VARCHAR(100),
@Rol INT
AS
BEGIN
	INSERT INTO CatEmpleados(Nombre,IdRol) VALUES(@Nombre, @Rol)
END
GO
DROP PROCEDURE IF EXISTS UpdateEmpleado
GO
CREATE PROCEDURE UpdateEmpleado 
@Id INT,
@Nombre VARCHAR(100),
@Rol INT,
@Estatus BIT
AS
BEGIN
	UPDATE CatEmpleados SET Nombre = @Nombre, IdRol = @Rol, Estatus  = @Estatus WHERE Id = @Id
	SELECT @@ROWCOUNT AS Exito
END
GO
DROP PROCEDURE IF EXISTS AddOrUpdateMovimiento
GO
CREATE PROCEDURE AddOrUpdateMovimiento 
@IdEmpleado INT,
@Mes INT,
@Entregas INT 
AS
BEGIN
	DECLARE @IdRol INT 

	DECLARE @TotalBruto DECIMAL(10,2)
		,@RetencionISR DECIMAL(10,2)
		,@BonoDespensa DECIMAL(10,2)
		,@TotalNeto DECIMAL(10,2)

	SELECT @IdRol = IdRol FROM CatEmpleados(NOLOCK) WHERE Id = @IdEmpleado

	SELECT @TotalBruto = TotalPercepcion
		,@RetencionISR = RetencionISR
		,@BonoDespensa = BonoDespensa
		,@TotalNeto = TotalNeto
	FROM CalculaSueldoEmpleado(@IdEmpleado,@IdRol, @Entregas)

	IF EXISTS (SELECT 1 FROM CtlMovimientos(NOLOCK) WHERE IdEmpleado = @IdEmpleado AND Mes = @Mes)
	BEGIN 
		UPDATE CtlMovimientos 
		SET Entregas = @Entregas, 
			SueldoBruto = @TotalBruto,
			Retenciones = @RetencionISR,
			BonoDespensa = @BonoDespensa,
			SueldoNeto = @TotalNeto
		WHERE IdEmpleado = @IdEmpleado AND Mes = @Mes
		SELECT @@ROWCOUNT AS Exito, CASE @@ROWCOUNT WHEN 0 THEN 'No se pudo actualizar movimiento' ELSE 'Se actualizó el movimiento' END AS Mensaje
	END
	ELSE
	BEGIN
		INSERT INTO CtlMovimientos(IdEmpleado, Mes, Entregas, SueldoBruto, Retenciones, BonoDespensa, SueldoNeto) 
							VALUES(@IdEmpleado, @Mes, @Entregas, @TotalBruto, @RetencionISR, @BonoDespensa, @TotalNeto)
		SELECT @@ROWCOUNT AS Exito, CASE @@ROWCOUNT WHEN 0 THEN 'No se pudo registrar el movimiento' ELSE 'Se registró el movimiento' END AS Mensaje
	END 
		
END 

