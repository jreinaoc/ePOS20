-- Corrección SP_CPOSC_AgregarActualizarExamen en BD014JR:
-- 1) USER_MOD = @USER_MOD activo en la rama UPDATE.
-- 2) El UPDATE no pisa EXA_Feccreacion con GETDATE() (conserva la fecha de creación).
-- 3) EXA_Fecmod / USER_MOD con ISNULL: si la app envía NULL (autoguardado sin
--    cambios reales), se preserva el valor existente en lugar de borrarlo.
USE [BD014JR]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SP_CPOSC_AgregarActualizarExamen] (
    @CTE_Nacio VARCHAR(20) = NULL,
    @CTE_CedIden VARCHAR(20) = NULL,
    @COD_Sucursal VARCHAR(3),
    @NUM_Examen INT,
    @FEC_Examen DATETIME,
    @ESFD DECIMAL(18, 2),
    @ESFI DECIMAL(18, 2),
    @CILD DECIMAL(18, 2),
    @CILI DECIMAL(18, 2),
    @EJED DECIMAL(18, 2),
    @EJEI DECIMAL(18, 2),
    @ADDD DECIMAL(18, 2),
    @ADDI DECIMAL(18, 2),
    @OBSERVACIONES VARCHAR(200) = NULL,
    @TIPO_Optm VARCHAR(50) = NULL,
    @NOM_Optm VARCHAR(100) = NULL,
    @EXA_Feccreacion DATETIME,
    @EXA_Fecmod DATETIME,
    @USER_CREA VARCHAR(5),
    @USER_MOD VARCHAR(5),
    @TIPOEXAMEN VARCHAR(50) = NULL,
    @NOMBRE_CLINICA_OPTM VARCHAR(100) = NULL,
    @ESFD2 DECIMAL(18, 2),
    @ESFI2 DECIMAL(18, 2),
    @CILD2 DECIMAL(18, 2),
    @CILI2 DECIMAL(18, 2),
    @EJED2 DECIMAL(18, 2),
    @EJEI2 DECIMAL(18, 2),
    @CodigoMimesys VARCHAR(50) = NULL
)
AS
BEGIN

IF @TIPO_Optm='EXTERNO'
BEGIN
    SET @TIPO_Optm='02'
END
ELSE
BEGIN
    SET @TIPO_Optm='01'
END

    -- Verificar si existe un registro con el mismo NUM_Examen para actualizar
    IF EXISTS (SELECT 1 FROM TB_EXAMEN WHERE NUM_Examen = @NUM_Examen AND CTE_Nacio = @CTE_Nacio AND
            CTE_CedIden = @CTE_CedIden)
    BEGIN
        -- Actualizar el registro existente
        UPDATE TB_EXAMEN
        SET
            COD_Sucursal = @COD_Sucursal,
            FEC_Examen = @FEC_Examen,
            ESFD = @ESFD,
            ESFI = @ESFI,
            CILD = @CILD,
            CILI = @CILI,
            EJED = @EJED,
            EJEI = @EJEI,
            ADDD = @ADDD,
            ADDI = @ADDI,
            OBSERVACIONES = @OBSERVACIONES,
            TIPO_Optm = @TIPO_Optm,
            NOM_Optm = @NOM_Optm,
            EXA_Fecmod = ISNULL(@EXA_Fecmod, EXA_Fecmod),
            USER_CREA = @USER_CREA,
            USER_MOD = ISNULL(@USER_MOD, USER_MOD),
            TIPOEXAMEN = @TIPOEXAMEN,
            NOMBRE_CLINICA_OPTM = @NOMBRE_CLINICA_OPTM,
            ESFD2 = @ESFD2,
            ESFI2 = @ESFI2,
            CILD2 = @CILD2,
            CILI2 = @CILI2,
            EJED2 = @EJED2,
            EJEI2 = @EJEI2,
            CodigoMimesys = @CodigoMimesys
        WHERE NUM_Examen = @NUM_Examen AND CTE_Nacio = @CTE_Nacio AND
            CTE_CedIden = @CTE_CedIden
    END
    ELSE
    BEGIN
        -- Insertar un nuevo registro
        INSERT INTO TB_EXAMEN (
            CTE_Nacio,
            CTE_CedIden,
            COD_Sucursal,
            NUM_Examen,
            FEC_Examen,
            ESFD,
            ESFI,
            CILD,
            CILI,
            EJED,
            EJEI,
            ADDD,
            ADDI,
            OBSERVACIONES,
            TIPO_Optm,
            NOM_Optm,
            EXA_Feccreacion,
            EXA_Fecmod,
            USER_CREA,
            USER_MOD,
            TIPOEXAMEN,
            NOMBRE_CLINICA_OPTM,
            ESFD2,
            ESFI2,
            CILD2,
            CILI2,
            EJED2,
            EJEI2,
            CodigoMimesys
        )
        VALUES (
            @CTE_Nacio,
            @CTE_CedIden,
            @COD_Sucursal,
            @NUM_Examen,
            @FEC_Examen,
            @ESFD,
            @ESFI,
            @CILD,
            @CILI,
            @EJED,
            @EJEI,
            @ADDD,
            @ADDI,
            @OBSERVACIONES,
            @TIPO_Optm,
            @NOM_Optm,
            GETDATE(),
            @EXA_Fecmod,
            @USER_CREA,
            @USER_MOD,
            @TIPOEXAMEN,
            @NOMBRE_CLINICA_OPTM,
            @ESFD2,
            @ESFI2,
            @CILD2,
            @CILI2,
            @EJED2,
            @EJEI2,
            @CodigoMimesys
        );
    END
END;
