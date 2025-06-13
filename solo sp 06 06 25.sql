USE [BD302JR]
GO

/****** Object:  StoredProcedure [dbo].[SP_CPOSC_ObtenerExamenPorNumeroYNacionalidadCedula]    Script Date: 06/06/2025 0:26:41 ******/
DROP PROCEDURE [dbo].[SP_CPOSC_ObtenerExamenPorNumeroYNacionalidadCedula]
GO

/****** Object:  StoredProcedure [dbo].[SP_CPOSC_InsertarTB_CTEPPALP]    Script Date: 06/06/2025 0:26:41 ******/
DROP PROCEDURE [dbo].[SP_CPOSC_InsertarTB_CTEPPALP]
GO

/****** Object:  StoredProcedure [dbo].[SP_CPOSC_InsertarTB_CTEPPAL]    Script Date: 06/06/2025 0:26:41 ******/
DROP PROCEDURE [dbo].[SP_CPOSC_InsertarTB_CTEPPAL]
GO

/****** Object:  StoredProcedure [dbo].[SP_CPOSC_Insertar_TB_CTETLF]    Script Date: 06/06/2025 0:26:41 ******/
DROP PROCEDURE [dbo].[SP_CPOSC_Insertar_TB_CTETLF]
GO

/****** Object:  StoredProcedure [dbo].[SP_CPOSC_Insertar_TB_CTEMAIL]    Script Date: 06/06/2025 0:26:41 ******/
DROP PROCEDURE [dbo].[SP_CPOSC_Insertar_TB_CTEMAIL]
GO

/****** Object:  StoredProcedure [dbo].[SP_CPOSC_GET_TB_TrabajoPorOrdenServicio]    Script Date: 06/06/2025 0:26:41 ******/
DROP PROCEDURE [dbo].[SP_CPOSC_GET_TB_TrabajoPorOrdenServicio]
GO

/****** Object:  StoredProcedure [dbo].[SP_CPOSC_GET_TB_CTEPPAL]    Script Date: 06/06/2025 0:26:41 ******/
DROP PROCEDURE [dbo].[SP_CPOSC_GET_TB_CTEPPAL]
GO

/****** Object:  StoredProcedure [dbo].[SP_CPOSC_AgregarActualizarTrabajo]    Script Date: 06/06/2025 0:26:41 ******/
DROP PROCEDURE [dbo].[SP_CPOSC_AgregarActualizarTrabajo]
GO

/****** Object:  StoredProcedure [dbo].[SP_CPOSC_AgregarActualizarQuerato]    Script Date: 06/06/2025 0:26:41 ******/
DROP PROCEDURE [dbo].[SP_CPOSC_AgregarActualizarQuerato]
GO

/****** Object:  StoredProcedure [dbo].[SP_CPOSC_AgregarActualizarFicConv]    Script Date: 06/06/2025 0:26:41 ******/
DROP PROCEDURE [dbo].[SP_CPOSC_AgregarActualizarFicConv]
GO

/****** Object:  StoredProcedure [dbo].[SP_CPOSC_AgregarActualizarFicCont]    Script Date: 06/06/2025 0:26:41 ******/
DROP PROCEDURE [dbo].[SP_CPOSC_AgregarActualizarFicCont]
GO

/****** Object:  StoredProcedure [dbo].[SP_CPOSC_AgregarActualizarExamen]    Script Date: 06/06/2025 0:26:41 ******/
DROP PROCEDURE [dbo].[SP_CPOSC_AgregarActualizarExamen]
GO

/****** Object:  StoredProcedure [dbo].[SP_CPOSC_AgregarActualizarExamen]    Script Date: 06/06/2025 0:26:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO











-- Stored Procedure para insertar o actualizar datos en TB_EXAMEN
CREATE PROCEDURE [dbo].[SP_CPOSC_AgregarActualizarExamen] (
    @CTE_Nacio VARCHAR(20) = NULL,
    @CTE_CedIden VARCHAR(20) = NULL,
    @COD_Sucursal VARCHAR(10) = '003',
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
    @USER_CREA VARCHAR(50) = NULL,
    @USER_MOD VARCHAR(50) = NULL,
    @TIPOEXAMEN VARCHAR(50) = NULL,
    @NOMBRE_CLINICA_OPTM VARCHAR(100) = NULL,
    --@TLF_TIPO VARCHAR(10) = NULL,
    --@TLF_COD VARCHAR(10) = NULL,
    --@TLF_NUMERO VARCHAR(20) = NULL,
    --@TLF_EXT VARCHAR(10) = NULL,
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
SET @COD_Sucursal = '03'
SET @USER_CREA='XX'

if @TIPO_Optm='EXTERNO' 
BEGIN
SET @TIPO_Optm='02' 
END
ELSE
BEGIN
SET @TIPO_Optm='01' 
END


---SELECT * FROM TB_EXAMEN   WHERE  CTE_CedIden=  '16573553'
    -- Verificar si existe un registro con el mismo NUM_Examen para actualizar
    IF EXISTS (SELECT 1 FROM TB_EXAMEN WHERE NUM_Examen = @NUM_Examen and  CTE_Nacio = @CTE_Nacio and
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
            EXA_Feccreacion = @EXA_Feccreacion,
            EXA_Fecmod = @EXA_Fecmod,
            USER_CREA = @USER_CREA,
            USER_MOD = @USER_MOD,
            TIPOEXAMEN = @TIPOEXAMEN,
            NOMBRE_CLINICA_OPTM = @NOMBRE_CLINICA_OPTM,
            --TLF_TIPO = @TLF_TIPO,
            --TLF_COD = @TLF_COD,
            --TLF_NUMERO = @TLF_NUMERO,
            --TLF_EXT = @TLF_EXT,
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
            --TLF_TIPO,
            --TLF_COD,
            --TLF_NUMERO,
            --TLF_EXT,
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
            @EXA_Feccreacion,
            @EXA_Fecmod,
            @USER_CREA,
            @USER_MOD,
            @TIPOEXAMEN,
            @NOMBRE_CLINICA_OPTM,
            --@TLF_TIPO,
            --@TLF_COD,
            --@TLF_NUMERO,
            --@TLF_EXT,
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
GO

/****** Object:  StoredProcedure [dbo].[SP_CPOSC_AgregarActualizarFicCont]    Script Date: 06/06/2025 0:26:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO








--
/*
exec SP_CPOS_AgregarActualizarFicCont @CTE_Nacio=N'V',@CTE_CedIden=N'14123926',
@COD_Sucursal='001',@NUM_Examen=1,@COLOR=1,@Observaciones='fgfgsfg  gdfgdfg'


*/


-- Stored Procedure para insertar o actualizar datos en TB_FICCONT
CREATE PROCEDURE [dbo].[SP_CPOSC_AgregarActualizarFicCont] (
    @CTE_Nacio VARCHAR(20),
    @CTE_CedIden VARCHAR(20) ,
    @COD_Sucursal VARCHAR(10) = 'OI',
    @NUM_Examen INT,
    @ESFD FLOAT = NULL,
    @ESFI FLOAT = NULL,
    @CILD FLOAT = NULL,
    @CILI FLOAT = NULL,
    @EJED FLOAT = NULL,
    @EJEI FLOAT = NULL,
    @ADDD FLOAT = NULL,
    @ADDI FLOAT = NULL,
    @CBD FLOAT = NULL,
    @CBI FLOAT = NULL,
    @DIAMD FLOAT = NULL,
    @DIAMI FLOAT = NULL,
    @COLOR VARCHAR(50) = NULL,
    @AVD FLOAT = NULL,
    @AVI FLOAT = NULL,
    @OBSERVACIONES VARCHAR(200) = NULL
)
AS
BEGIN

SET    @COD_Sucursal='03'

    -- Verificar si existe un registro con la combinación de claves para actualizar
    IF EXISTS (SELECT 1 FROM TB_FICCONT 
	WHERE CTE_Nacio = @CTE_Nacio AND CTE_CedIden = @CTE_CedIden and NUM_Examen = @NUM_Examen)
    BEGIN
        -- Actualizar el registro existente
        UPDATE TB_FICCONT
        SET
            ESFD = @ESFD,
            ESFI = @ESFI,
            CILD = @CILD,
            CILI = @CILI,
            EJED = @EJED,
            EJEI = @EJEI,
            ADDD = @ADDD,
            ADDI = @ADDI,
            CBD = @CBD,
            CBI = @CBI,
            DIAMD = @DIAMD,
            DIAMI = @DIAMI,
            COLOR = @COLOR,
            AVD = @AVD,
            AVI = @AVI,
            OBSERVACIONES = @OBSERVACIONES,COD_Sucursal = @COD_Sucursal
        WHERE CTE_Nacio = @CTE_Nacio AND CTE_CedIden = @CTE_CedIden AND  NUM_Examen = @NUM_Examen;
        
      
    END
    ELSE
    BEGIN

	
        -- Insertar un nuevo registro
        INSERT INTO TB_FICCONT (
            CTE_Nacio,
            CTE_CedIden,
            COD_Sucursal,
            NUM_Examen,
            ESFD,
            ESFI,
            CILD,
            CILI,
            EJED,
            EJEI,
            ADDD,
            ADDI,
            CBD,
            CBI,
            DIAMD,
            DIAMI,
            COLOR,
            AVD,
            AVI,
            OBSERVACIONES
        )
        VALUES (
            @CTE_Nacio,
            @CTE_CedIden,
            @COD_Sucursal,
            @NUM_Examen,
            @ESFD,
            @ESFI,
            @CILD,
            @CILI,
            @EJED,
            @EJEI,
            @ADDD,
            @ADDI,
            @CBD,
            @CBI,
            @DIAMD,
            @DIAMI,
            @COLOR,
            @AVD,
            @AVI,
            @OBSERVACIONES
        );
    END      
	

		SELECT   CTE_Nacio,
            CTE_CedIden,
            COD_Sucursal,
            NUM_Examen,
            ESFD,
            ESFI,
            CILD,
            CILI,
            EJED,
            EJEI,
            ADDD,
            ADDI,
            CBD,
            CBI,
            DIAMD,
            DIAMI,
            COLOR,
            AVD,
            AVI,
            OBSERVACIONES FROM TB_FICCONT 
	WHERE CTE_Nacio = @CTE_Nacio AND CTE_CedIden = @CTE_CedIden and NUM_Examen = @NUM_Examen
      union all
        SELECT     @CTE_Nacio,
            @CTE_CedIden,
            @COD_Sucursal,
            @NUM_Examen,
            @ESFD ESFD,
            @ESFI,
            @CILD,
            @CILI,
            @EJED,
            @EJEI,
            @ADDD ADDD,
            @ADDI ADDI,
            @CBD,
            @CBI,
            @DIAMD DIAMD,
            @DIAMI DIAMI,
            @COLOR,
            @AVD,
            @AVI,
            @OBSERVACIONES;
  
END;
GO

/****** Object:  StoredProcedure [dbo].[SP_CPOSC_AgregarActualizarFicConv]    Script Date: 06/06/2025 0:26:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO









--
/*
exec SP_CPOS_AgregarActualizarFicConv @CTE_Nacio=N'V',@CTE_CedIden=N'14123926',@COD_Sucursal=default,
@NUM_Examen=5,@DPDL=NULL,@DPDC=1,@DPIL=NULL,@DPIC=NULL,@ALTD=NULL,@ALTI=NULL,
@PRISMAD=NULL,@PRISMAI=NULL,@AVD=1,@AVI=NULL,@PRISMAD2=NULL,@PRISMAI2=NULL,@PROGVISIONLEJOSDISTD=NULL,
@PROGVISIONLEJOSDISTI=NULL,@PROGVISIONCERCADISTD=NULL,@PROGVISIONCERCADISTI=NULL,@PROGVISIONMEDIADISTD=NULL,
@PROGVISIONMEDIADISTI=NULL


*/

-- Stored Procedure para insertar o actualizar datos en TB_ficconv
CREATE PROCEDURE [dbo].[SP_CPOSC_AgregarActualizarFicConv] (
    @CTE_Nacio VARCHAR(20),
    @CTE_CedIden VARCHAR(20),
    @COD_Sucursal VARCHAR(10)=null,
    @NUM_Examen INT,
    @DPDL FLOAT = NULL,
    @DPDC FLOAT = NULL,
    @DPIL FLOAT = NULL,
    @DPIC FLOAT = NULL,
    @ALTD FLOAT = NULL,
    @ALTI FLOAT = NULL,
    @PRISMAD FLOAT = NULL,
    @PRISMAI FLOAT = NULL,
    @PBASED FLOAT = NULL,
    @PBASEI FLOAT = NULL,
    @OFTI VARCHAR(200),
    @OFTD VARCHAR(200),
    @AVD FLOAT = NULL,
    @AVI FLOAT = NULL,
    @RETI VARCHAR(200),
    @RETD VARCHAR(200),
    @PRISMAD2 FLOAT = NULL,
    @PRISMAI2 FLOAT = NULL,
    @PBASED2 FLOAT = NULL,
    @PBASEI2 FLOAT = NULL,
    @PROGVISIONLEJOSDISTD FLOAT = NULL,
    @PROGVISIONLEJOSDISTI FLOAT = NULL,
    @PROGVISIONCERCADISTD FLOAT = NULL,
    @PROGVISIONCERCADISTI FLOAT = NULL,
    @PROGVISIONMEDIADISTD FLOAT = NULL,
    @PROGVISIONMEDIADISTI FLOAT = NULL
)
AS
BEGIN


SET    @COD_Sucursal='03'
    -- Verificar si existe un registro con la combinación de claves para actualizar
    IF EXISTS (SELECT 1 FROM TB_ficconv WHERE CTE_Nacio = @CTE_Nacio AND CTE_CedIden = @CTE_CedIden AND COD_Sucursal = @COD_Sucursal AND NUM_Examen = @NUM_Examen)
    BEGIN
        -- Actualizar el registro existente
        UPDATE TB_ficconv
        SET
            DPDL = @DPDL,
            DPDC = @DPDC,
            DPIL = @DPIL,
            DPIC = @DPIC,
            ALTD = @ALTD,
            ALTI = @ALTI,
            PRISMAD = @PRISMAD,
            PRISMAI = @PRISMAI,
            PBASED = @PBASED,
            PBASEI = @PBASEI,
            OFTI =  LEFT  (@OFTI,100),
            OFTD =  LEFT (@OFTD,100),
            AVD = @AVD,
            AVI = @AVI,
            RETI = LEFT (@RETI,100),
            RETD =  LEFT (@RETD,100),
            PRISMAD2 = @PRISMAD2,
            PRISMAI2 = @PRISMAI2,
            PBASED2 = @PBASED2,
            PBASEI2 = @PBASEI2,
            PROGVISIONLEJOSDISTD = @PROGVISIONLEJOSDISTD,
            PROGVISIONLEJOSDISTI = @PROGVISIONLEJOSDISTI,
            PROGVISIONCERCADISTD = @PROGVISIONCERCADISTD,
            PROGVISIONCERCADISTI = @PROGVISIONCERCADISTI,
            PROGVISIONMEDIADISTD = @PROGVISIONMEDIADISTD,
            PROGVISIONMEDIADISTI = @PROGVISIONMEDIADISTI,
			COD_Sucursal = @COD_Sucursal
        WHERE CTE_Nacio = @CTE_Nacio AND CTE_CedIden = @CTE_CedIden  AND NUM_Examen = @NUM_Examen;

        -- Devolver el número de filas afectadas (1 si se actualizó)
        SELECT @@ROWCOUNT;
    END
    ELSE
    BEGIN
        -- Insertar un nuevo registro
        INSERT INTO TB_ficconv (
            CTE_Nacio,
            CTE_CedIden,
            COD_Sucursal,
            NUM_Examen,
            DPDL,
            DPDC,
            DPIL,
            DPIC,
            ALTD,
            ALTI,
            PRISMAD,
            PRISMAI,
            PBASED,
            PBASEI,
            OFTI,
            OFTD,
            AVD,
            AVI,
            RETI,
            RETD,
            PRISMAD2,
            PRISMAI2,
            PBASED2,
            PBASEI2,
            PROGVISIONLEJOSDISTD,
            PROGVISIONLEJOSDISTI,
            PROGVISIONCERCADISTD,
            PROGVISIONCERCADISTI,
            PROGVISIONMEDIADISTD,
            PROGVISIONMEDIADISTI
        )
        VALUES (
            @CTE_Nacio,
            @CTE_CedIden,
            @COD_Sucursal,
            @NUM_Examen,
            @DPDL,
            @DPDC,
            @DPIL,
            @DPIC,
            @ALTD,
            @ALTI,
            @PRISMAD,
            @PRISMAI,
            @PBASED,
            @PBASEI,
            @OFTI,
            @OFTD,
            @AVD,
            @AVI,
            @RETI,
            @RETD,
            @PRISMAD2,
            @PRISMAI2,
            @PBASED2,
            @PBASEI2,
            @PROGVISIONLEJOSDISTD,
            @PROGVISIONLEJOSDISTI,
            @PROGVISIONCERCADISTD,
            @PROGVISIONCERCADISTI,
            @PROGVISIONMEDIADISTD,
            @PROGVISIONMEDIADISTI
        );

        -- Devolver el número de filas afectadas (1 si se insertó)
        SELECT @@ROWCOUNT;
    END
END;
GO

/****** Object:  StoredProcedure [dbo].[SP_CPOSC_AgregarActualizarQuerato]    Script Date: 06/06/2025 0:26:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO











-- Stored Procedure para insertar o actualizar datos en TB_QUERATO
CREATE PROCEDURE [dbo].[SP_CPOSC_AgregarActualizarQuerato] (
    @CTE_Nacio VARCHAR(20) = NULL,
    @CTE_CedIden VARCHAR(20) = NULL,
    @COD_Sucursal VARCHAR(10) = NULL,
    @NUM_Examen INT,
    @QUERATOMD1 FLOAT = NULL,
    @QUERATOGD1 FLOAT = NULL,
    @QUERATOMD2 FLOAT = NULL,
    @QUERATOGD2 FLOAT = NULL,
    @QUERATOMI1 FLOAT = NULL,
    @QUERATOGI1 FLOAT = NULL,
    @QUERATOMI2 FLOAT = NULL,
    @QUERATOGI2 FLOAT = NULL,
    @QueObserv VARCHAR(200) = NULL

)
AS
BEGIN


SET @COD_Sucursal='03'
    -- Verificar si existe un registro con la combinación de claves para actualizar
    IF EXISTS (SELECT 1 FROM TB_QUERATO WHERE CTE_Nacio = @CTE_Nacio AND CTE_CedIden = @CTE_CedIden AND COD_Sucursal = @COD_Sucursal AND NUM_Examen = @NUM_Examen)
    BEGIN
        -- Actualizar el registro existente
        UPDATE TB_QUERATO
        SET
            QUERATOMD1 = @QUERATOMD1,
            QUERATOGD1 = @QUERATOGD1,
            QUERATOMD2 = @QUERATOMD2,
            QUERATOGD2 = @QUERATOGD2,
            QUERATOMI1 = @QUERATOMI1,
            QUERATOGI1 = @QUERATOGI1,
            QUERATOMI2 = @QUERATOMI2,
            QUERATOGI2 = @QUERATOGI2,
            QUE_OBSERV = @QueObserv
        WHERE CTE_Nacio = @CTE_Nacio AND CTE_CedIden = @CTE_CedIden AND COD_Sucursal = @COD_Sucursal AND NUM_Examen = @NUM_Examen;

        -- Devolver el número de filas afectadas (1 si se actualizó)
        SELECT @@ROWCOUNT;
    END
    ELSE
    BEGIN
        -- Insertar un nuevo registro
        INSERT INTO TB_QUERATO (
            CTE_Nacio,
            CTE_CedIden,
            COD_Sucursal,
            NUM_Examen,
            QUERATOMD1,
            QUERATOGD1,
            QUERATOMD2,
            QUERATOGD2,
            QUERATOMI1,
            QUERATOGI1,
            QUERATOMI2,
            QUERATOGI2,
            QUE_OBSERV
        )
        VALUES (
            @CTE_Nacio,
            @CTE_CedIden,
            @COD_Sucursal,
            @NUM_Examen,
            @QUERATOMD1,
            @QUERATOGD1,
            @QUERATOMD2,
            @QUERATOGD2,
            @QUERATOMI1,
            @QUERATOGI1,
            @QUERATOMI2,
            @QUERATOGI2,
            @QueObserv
        );

        -- Devolver el número de filas afectadas (1 si se insertó)
        SELECT @@ROWCOUNT;
    END
END;
GO

/****** Object:  StoredProcedure [dbo].[SP_CPOSC_AgregarActualizarTrabajo]    Script Date: 06/06/2025 0:26:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO










-- Stored Procedure para insertar o actualizar datos en TB_TRABAJO
CREATE PROCEDURE [dbo].[SP_CPOSC_AgregarActualizarTrabajo] -- Stored Procedure para insertar o actualizar datos en TB_TRABAJO
(
    @TSucursal VARCHAR(10) = NULL,
    @TNumOrdserv VARCHAR(20) = NULL,
    @TRevision VARCHAR(10) = 1,
    @TCEDIDEN VARCHAR(20) = NULL,
    @TNACIO VARCHAR(10) = NULL,
    @TTIPOTRABAJO VARCHAR(50) = NULL,
    @TEXAMEN VARCHAR(50) = NULL,
    @THORIZONTAL FLOAT = NULL,
    @TVERTICAL FLOAT = NULL,
    @TMAXIMA FLOAT = NULL,
    @TPUENTE FLOAT = NULL,
    @TALTD FLOAT = NULL,
    @TALTI FLOAT = NULL,
    @TOJO VARCHAR(10) = NULL,
    @TTIPOVISIOND VARCHAR(50) = NULL,
    @TTIPOVISIONI VARCHAR(50) = NULL,
    @TLABORATORIO VARCHAR(100) = NULL,
    @TSERVICIO VARCHAR(100) = NULL,
    @THORAOFRECIDO VARCHAR(10) = NULL,
    @TTIPORX VARCHAR(50) = NULL,
    @TFECHAOFRECIDO DATETIME = NULL,
    @TFECCREA DATETIME = NULL,
    @TFECMOD DATETIME = NULL,
    @USERCREA VARCHAR(50) = NULL,
    @USERMOD VARCHAR(50) = NULL,
    @CodDetVta INT = NULL,
    @TipoExamen VARCHAR(50) = NULL,
    @TDISTANCIAVERTICE FLOAT = NULL,
    @TANGULOPANTOSCOPICO FLOAT = NULL,
    @TANGULOFACIAL FLOAT = NULL,
    @Correlativo INT = NULL,
    @TDISTANCIADELECTURA FLOAT = NULL
)
AS
BEGIN



   --set @TSucursal  = '03'
   SET @TRevision=0 
   SET   @TLABORATORIO=0
   SET @TFECCREA=GETDATE()
   --SET @USERCREA='XXX'



    -- Verificar si existe un registro con la combinación de claves para actualizar
    IF EXISTS (SELECT 1 FROM TB_TRABAJO 
	WHERE    T_EXAMEN = @TEXAMEN  
	AND T_CEDIDEN = @TCEDIDEN AND T_NACIO = @TNACIO)
    BEGIN
        -- Actualizar el registro existente
        UPDATE TB_TRABAJO
        SET
            T_TIPOTRABAJO = @TTIPOTRABAJO,
            T_EXAMEN = @TEXAMEN,
            T_HORIZONTAL = @THORIZONTAL,
            T_VERTICAL = @TVERTICAL,
            T_MAXIMA = @TMAXIMA,
            T_PUENTE = @TPUENTE,
            T_ALTD = @TALTD,
            T_ALTI = @TALTI,
            T_OJO = @TOJO,
            T_TIPOVISIOND = @TTIPOVISIOND,
            T_TIPOVISIONI = @TTIPOVISIONI,
            T_LABORATORIO = @TLABORATORIO,
            T_SERVICIO = @TSERVICIO,
            T_HORAOFRECIDO = @THORAOFRECIDO,
            T_TIPORX = @TTIPORX,
            T_FECHAOFRECIDO = @TFECHAOFRECIDO,
            T_FECCREA = @TFECCREA,
            T_FECMOD = @TFECMOD,
            USER_CREA = @USERCREA,
            USER_MOD = @USERMOD,
            Cod_DetVta = @CodDetVta,
            TipoExamen = @TipoExamen,
            T_DISTANCIAVERTICE = @TDISTANCIAVERTICE,
            T_ANGULOPANTOSCOPICO = @TANGULOPANTOSCOPICO,
            T_ANGULOFACIAL = @TANGULOFACIAL,
         
            T_DISTANCIADELECTURA = @TDISTANCIADELECTURA
        WHERE  T_EXAMEN = @TEXAMEN AND   T_CEDIDEN = @TCEDIDEN AND T_NACIO = @TNACIO;

        -- Devolver el número de filas afectadas (1 si se actualizó)
        SELECT @@ROWCOUNT , 'update';
    END
    ELSE
    BEGIN
        -- Insertar un nuevo registro
        INSERT INTO TB_TRABAJO (
            T_SUCURSAL,
            T_NumOrdserv,
            T_Revision,
            T_CEDIDEN,
            T_NACIO,
            T_TIPOTRABAJO,
            T_EXAMEN,
            T_HORIZONTAL,
            T_VERTICAL,
            T_MAXIMA,
            T_PUENTE,
            T_ALTD,
            T_ALTI,
            T_OJO,
            T_TIPOVISIOND,
            T_TIPOVISIONI,
            T_LABORATORIO,
            T_SERVICIO,
            T_HORAOFRECIDO,
            T_TIPORX,
            T_FECHAOFRECIDO,
            T_FECCREA,
            T_FECMOD,
            USER_CREA,
            USER_MOD,
            Cod_DetVta,
            TipoExamen,
            T_DISTANCIAVERTICE,
            T_ANGULOPANTOSCOPICO,
            T_ANGULOFACIAL,
         
            T_DISTANCIADELECTURA
        )
        VALUES (
            @TSucursal,
            @TNumOrdserv,
            @TRevision,
            @TCEDIDEN,
            @TNACIO,
            @TTIPOTRABAJO,
            @TEXAMEN,
            @THORIZONTAL,
            @TVERTICAL,
            @TMAXIMA,
            @TPUENTE,
            @TALTD,
            @TALTI,
            @TOJO,
            @TTIPOVISIOND,
            @TTIPOVISIONI,
            @TLABORATORIO,
            @TSERVICIO,
            @THORAOFRECIDO,
            @TTIPORX,
            @TFECHAOFRECIDO,
            @TFECCREA,
            @TFECMOD,
            @USERCREA,
            @USERMOD,
            @CodDetVta,
            @TipoExamen,
            @TDISTANCIAVERTICE,
            @TANGULOPANTOSCOPICO,
            @TANGULOFACIAL,
       
            @TDISTANCIADELECTURA
        );

        -- Devolver el número de filas afectadas (1 si se insertó)
        --SELECT @@ROWCOUNT;


		

    END

	select * from TB_TRABAJO   WHERE  T_EXAMEN = @TEXAMEN AND   T_CEDIDEN = @TCEDIDEN AND T_NACIO = @TNACIO;
END;
GO

/****** Object:  StoredProcedure [dbo].[SP_CPOSC_GET_TB_CTEPPAL]    Script Date: 06/06/2025 0:26:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- mcll 15-04-25 Alterar el Stored Procedure para obtener registros de la tabla TB_CTEPPAL
-- con prioridad de búsqueda por Cédula/Nacionalidad o por filtro general.
CREATE PROCEDURE [dbo].[SP_CPOSC_GET_TB_CTEPPAL]
    @CTE_CedIden VARCHAR(20) = NULL,      -- Parámetro para búsqueda exacta por Cédula
    @CTE_Nacio VARCHAR(1) = NULL,         -- Parámetro para búsqueda exacta por Nacionalidad
    @Filtro NVARCHAR(255) = NULL,         -- Parámetro para el texto de búsqueda general
    @BuscarPorCedula BIT = 0,             -- 1 si se busca por cédula en el filtro general
    @BuscarPorNombre BIT = 0              -- 1 si se busca por nombre en el filtro general
AS
BEGIN
    SET NOCOUNT ON; -- Evita que se devuelvan recuentos de filas afectados

    -- Condición principal: Si @CTE_CedIden y @CTE_Nacio están llenos, priorizar la búsqueda exacta
    IF (@CTE_CedIden IS NOT NULL AND LTRIM(RTRIM(@CTE_CedIden)) <> '' AND
        @CTE_Nacio IS NOT NULL AND LTRIM(RTRIM(@CTE_Nacio)) <> '')
    BEGIN
        -- Lógica original para búsqueda exacta y devolución de NULLs si no se encuentra
        IF NOT EXISTS (SELECT 1 FROM [dbo].[TB_CTEPPAL] WHERE CTE_CedIden = LTRIM(RTRIM(@CTE_CedIden)) AND CTE_Nacio = @CTE_Nacio)
        BEGIN
            -- Devolver una fila de NULLs si el registro no se encontró
            SELECT
                NULL AS CTE_Nacio, NULL AS CTE_CedIden, NULL AS CTE_id, NULL AS CTE_PNombre, NULL AS CTE_FNac, NULL AS CTE_VIP,
                NULL AS CTE_AfilVip, NULL AS CTE_FecAfil, NULL AS COD_STCTE, NULL AS CTE_Sex,
                NULL AS CTE_CodOcup, NULL AS CTE_EdoCiv, NULL AS COD_Sucursal, NULL AS CTE_FecCreacion,
                NULL AS CTE_FecMod, NULL AS USER_CREA, NULL AS USER_MOD, NULL AS Direccion_fact,
                NULL AS Facebook, NULL AS Twitter, NULL AS Instagram, NULL AS CTE_RETISLR,
                NULL AS CTE_RETIVA, NULL AS COD_Edo, NULL AS COD_Ciud, NULL AS EDO_Nombre,
                NULL AS CIUD_Nombre, NULL AS NumExamen,
                NULL AS TLF_Cod002, NULL AS TLF_Numero002, NULL AS TLF_Ext002,
                NULL AS TLF_Cod003, NULL AS TLF_Numero003, NULL AS TLF_Ext03,
                NULL AS Mail_Loogin;
            RETURN; -- Salir del procedimiento
        END
        ELSE
        BEGIN
            -- Seleccionar el cliente que coincide exactamente
            SELECT
                p.[CTE_Nacio],
                p.[CTE_CedIden],
                p.[CTE_Nacio] + p.[CTE_CedIden] AS CTE_id,
                ISNULL(p.CTE_PNombre, '') + ' ' + ISNULL(p.CTE_SNombre, '') + ' ' + ISNULL(p.CTE_PApellido, '') + ' ' + ISNULL(p.CTE_SApellido, '') AS CTE_PNombre,
                p.[CTE_FNac],
                p.[CTE_VIP],
                p.[CTE_AfilVip],
                p.[CTE_FecAfil],
                p.[COD_STCTE],
                p.[CTE_Sex],
                p.[CTE_CodOcup],
                p.[CTE_EdoCiv],
                p.[COD_Sucursal],
                p.[CTE_FecCreacion],
                p.[CTE_FecMod],
                p.[USER_CREA],
                p.[USER_MOD],
                p.[Direccion_fact],
                p.[Facebook],
                p.[Twitter],
                p.[Instagram],
                p.[CTE_RETISLR],
                p.[CTE_RETIVA],
                m.COD_Ciud,
                m.COD_Edo,
                E.EDO_Nombre,
                M.CIUD_Nombre,
                dbo.ufn_MaxNumExamen(p.CTE_Nacio, p.CTE_CedIden) AS NumExamen,
                ISNULL(tlf2.[TLF_Cod], NULL) AS TLF_Cod002,
                ISNULL(tlf2.[TLF_Numero], NULL) AS TLF_Numero002,
                ISNULL(tlf2.[TLF_Ext], NULL) AS TLF_Ext002,
                ISNULL(tlf3.[TLF_Cod], NULL) AS TLF_Cod003,
                ISNULL(tlf3.[TLF_Numero], NULL) AS TLF_Numero003,
                ISNULL(tlf3.[TLF_Ext], NULL) AS TLF_Ext03,
                CASE
                    WHEN CHARINDEX('@', Mail_Loogin) > 0 THEN Mail_Loogin
                    ELSE Mail_Loogin + '@' + Mail_Dominio + '.' + Mail_Ext
                END AS Mail_Loogin
            FROM
                [dbo].[TB_CTEPPAL] p
            INNER JOIN [VTB_MAESDIR] d ON p.[CTE_CedIden] = d.[CTE_CedIden] AND p.[CTE_Nacio] = d.[CTE_Nacio] AND d.RowNumByClient = 1
            INNER JOIN TB_MAESEDO E ON d.DIR_CodEdo = E.COD_Edo
            INNER JOIN TB_MAESCIUD M ON d.DIR_CodCiud = M.COD_Ciud AND d.DIR_CodEdo = M.COD_EDO
            LEFT JOIN [dbo].[TB_CTETLF] tlf2 ON p.[CTE_CedIden] = tlf2.[CTE_CedIden] AND p.[CTE_Nacio] = tlf2.[CTE_Nacio] AND tlf2.TLF_Tipo = '002' AND tlf2.Ind_Tlf = 1 AND tlf2.[CTE_Nacio] <> '' AND tlf2.[CTE_CedIden] <> ''
            LEFT JOIN [dbo].[TB_CTETLF] tlf3 ON p.[CTE_CedIden] = tlf3.[CTE_CedIden] AND p.[CTE_Nacio] = tlf3.[CTE_Nacio] AND tlf3.TLF_Tipo = '003' AND tlf3.Ind_Tlf = 1 AND tlf3.[CTE_Nacio] <> '' AND tlf3.[CTE_CedIden] <> ''
            LEFT JOIN [dbo].[TB_CTEMAIL] em ON p.[CTE_CedIden] = em.[CTE_CedIden] AND p.[CTE_Nacio] = em.[CTE_Nacio] AND em.Ind_Mail = 1 AND em.[CTE_Nacio] <> '' AND em.[CTE_CedIden] <> ''
            WHERE
                p.CTE_CedIden = LTRIM(RTRIM(@CTE_CedIden)) AND p.CTE_Nacio = @CTE_Nacio
                AND p.[CTE_CedIden] != '' AND p.[CTE_CedIden] IS NOT NULL
                AND p.[CTE_Nacio] <> '' AND p.[CTE_CedIden] <> '';
        END
    END
    ELSE -- Si @CTE_CedIden o @CTE_Nacio no están llenos, aplicar los filtros generales
    BEGIN
        SELECT
            p.[CTE_Nacio],
            p.[CTE_CedIden],
            p.[CTE_Nacio] + p.[CTE_CedIden] AS CTE_id,
            ISNULL(p.CTE_PNombre, '') + ' ' + ISNULL(p.CTE_SNombre, '') + ' ' + ISNULL(p.CTE_PApellido, '') + ' ' + ISNULL(p.CTE_SApellido, '') AS CTE_PNombre,
            p.[CTE_FNac],
            p.[CTE_VIP],
            p.[CTE_AfilVip],
            p.[CTE_FecAfil],
            p.[COD_STCTE],
            p.[CTE_Sex],
            p.[CTE_CodOcup],
            p.[CTE_EdoCiv],
            p.[COD_Sucursal],
            p.[CTE_FecCreacion],
            p.[CTE_FecMod],
            p.[USER_CREA],
            p.[USER_MOD],
            p.[Direccion_fact],
            p.[Facebook],
            p.[Twitter],
            p.[Instagram],
            p.[CTE_RETISLR],
            p.[CTE_RETIVA],
            m.COD_Ciud,
            m.COD_Edo,
            E.EDO_Nombre,
            M.CIUD_Nombre,
            dbo.ufn_MaxNumExamen(p.CTE_Nacio, p.CTE_CedIden) AS NumExamen,
            ISNULL(tlf2.[TLF_Cod], NULL) AS TLF_Cod002,
            ISNULL(tlf2.[TLF_Numero], NULL) AS TLF_Numero002,
            ISNULL(tlf2.[TLF_Ext], NULL) AS TLF_Ext002,
            ISNULL(tlf3.[TLF_Cod], NULL) AS TLF_Cod003,
            ISNULL(tlf3.[TLF_Numero], NULL) AS TLF_Numero003,
            ISNULL(tlf3.[TLF_Ext], NULL) AS TLF_Ext03,
            CASE
                WHEN CHARINDEX('@', Mail_Loogin) > 0 THEN Mail_Loogin
                ELSE Mail_Loogin + '@' + Mail_Dominio + '.' + Mail_Ext
            END AS Mail_Loogin
        FROM
            [dbo].[TB_CTEPPAL] p
        INNER JOIN [VTB_MAESDIR] d ON p.[CTE_CedIden] = d.[CTE_CedIden] AND p.[CTE_Nacio] = d.[CTE_Nacio] AND d.RowNumByClient = 1
        INNER JOIN TB_MAESEDO E ON d.DIR_CodEdo = E.COD_Edo
        INNER JOIN TB_MAESCIUD M ON d.DIR_CodCiud = M.COD_Ciud AND d.DIR_CodEdo = M.COD_EDO
        LEFT JOIN [dbo].[TB_CTETLF] tlf2 ON p.[CTE_CedIden] = tlf2.[CTE_CedIden] AND p.[CTE_Nacio] = tlf2.[CTE_Nacio] AND tlf2.TLF_Tipo = '002' AND tlf2.Ind_Tlf = 1 AND tlf2.[CTE_Nacio] <> '' AND tlf2.[CTE_CedIden] <> ''
        LEFT JOIN [dbo].[TB_CTETLF] tlf3 ON p.[CTE_CedIden] = tlf3.[CTE_CedIden] AND p.[CTE_Nacio] = tlf3.[CTE_Nacio] AND tlf3.TLF_Tipo = '003' AND tlf3.Ind_Tlf = 1 AND tlf3.[CTE_Nacio] <> '' AND tlf3.[CTE_CedIden] <> ''
        LEFT JOIN [dbo].[TB_CTEMAIL] em ON p.[CTE_CedIden] = em.[CTE_CedIden] AND p.[CTE_Nacio] = em.[CTE_Nacio] AND em.Ind_Mail = 1 AND em.[CTE_Nacio] <> '' AND em.[CTE_CedIden] <> ''
        WHERE
            -- Condiciones base para asegurar datos válidos
            p.[CTE_CedIden] != '' AND p.[CTE_CedIden] IS NOT NULL AND
            p.[CTE_Nacio] <> '' AND p.[CTE_CedIden] <> '' AND
            -- Lógica de filtrado dinámico para búsqueda general
            (@Filtro IS NULL OR @Filtro = '' OR -- Si el filtro general está vacío o nulo, no aplica la condición de filtro
             (@BuscarPorCedula = 1 AND p.CTE_CedIden LIKE '%' + @Filtro + '%') OR -- Busca por cédula con LIKE
             (@BuscarPorNombre = 1 AND ( -- Busca por nombre completo (concatenado) con LIKE
                 ISNULL(p.CTE_PNombre, '') + ' ' +
                 ISNULL(p.CTE_SNombre, '') + ' ' +
                 ISNULL(p.CTE_PApellido, '') + ' ' +
                 ISNULL(p.CTE_SApellido, '') LIKE '%' + @Filtro + '%'
             ))
            );
        -- Puedes descomentar la siguiente línea si necesitas ordenar los resultados en la búsqueda general
        -- ORDER BY p.[CTE_FecCreacion] DESC;
    END
END;
GO

/****** Object:  StoredProcedure [dbo].[SP_CPOSC_GET_TB_TrabajoPorOrdenServicio]    Script Date: 06/06/2025 0:26:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





-- Stored Procedure: dbo.ObtenerTrabajoPorOrdenServicio
-- Description: Retrieves the top 1 record from TB_TRABAJO based on Nacionalidad, Cedula, and NumOrdServ,
--              ordering by T_VERTICAL in descending order. NULL values for specific columns are returned as 0.
CREATE PROCEDURE [dbo].[SP_CPOSC_GET_TB_TrabajoPorOrdenServicio]
    @Nacionalidad NVARCHAR(1), -- Adjust data type and length as per your column definition
    @Cedula NVARCHAR(9),       -- Adjust data type and length as per your column definition
    @NumOrdServ INT             -- Adjust data type as per your column definition
AS
BEGIN
    SET NOCOUNT ON; -- Prevents the count of the number of rows affected from being returned.

    SELECT TOP 1
        T_SUCURSAL,
        T_NumOrdserv,
        T_Revision,
        T_CEDIDEN,
        T_NACIO,
        T_TIPOTRABAJO,
        T_EXAMEN,
        ISNULL(T_HORIZONTAL, 0) AS T_HORIZONTAL,
        ISNULL(T_VERTICAL, 0) AS T_VERTICAL,
        ISNULL(T_MAXIMA, 0) AS T_MAXIMA,
        ISNULL(T_PUENTE, 0) AS T_PUENTE,
        ISNULL(T_ALTD, 0) AS T_ALTD,
        ISNULL(T_ALTI, 0) AS T_ALTI,
        T_OJO,
        T_TIPOVISIOND,
        T_TIPOVISIONI,
        T_LABORATORIO,
        T_SERVICIO,
        T_HORAOFRECIDO,
        T_TIPORX,
        T_FECHAOFRECIDO,
        T_FECCREA,
        T_FECMOD,
        USER_CREA,
        USER_MOD,
        Cod_DetVta,
        TipoExamen,
        ISNULL(T_DISTANCIAVERTICE, 0) AS T_DISTANCIAVERTICE,
        ISNULL(T_ANGULOPANTOSCOPICO, 0) AS T_ANGULOPANTOSCOPICO,
        ISNULL(T_ANGULOFACIAL, 0) AS T_ANGULOFACIAL,
        Correlativo,
        ISNULL(T_DISTANCIADELECTURA, 0) AS T_DISTANCIADELECTURA
    FROM
        [dbo].[TB_TRABAJO]
    WHERE
        T_NACIO = @Nacionalidad
        AND T_CEDIDEN = @Cedula
        AND T_EXAMEN = @NumOrdServ
    ORDER BY
        T_VERTICAL DESC;
END;
GO

/****** Object:  StoredProcedure [dbo].[SP_CPOSC_Insertar_TB_CTEMAIL]    Script Date: 06/06/2025 0:26:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO











CREATE PROCEDURE [dbo].[SP_CPOSC_Insertar_TB_CTEMAIL] (
@CTE_Nacio CHAR(1),
@CTE_CedIden NVARCHAR(10),
@Mail_Loogin NVARCHAR(50),
@Mail_Dominio NVARCHAR(50) = '',
@Mail_Ext NVARCHAR(10) = '',
@Mail_Pref BIT = 0,
@Mail_FecCrea DATETIME = NULL,
@USER_Crea CHAR(5) = NULL
)
AS
BEGIN
SET NOCOUNT ON;

DECLARE @correlativo INT;

IF @Mail_FecCrea IS NULL
BEGIN
SET @Mail_FecCrea = GETDATE();
END;

SET @Mail_Dominio = '';-- Esta línea parece sobrescribir el valor pasado, ¿es intencional?

BEGIN TRY
-- Verificar si existe el cliente en TB_CTEPPAL
IF NOT EXISTS (SELECT 1 FROM TB_CTEPPAL WHERE CTE_Nacio = @CTE_Nacio AND CTE_CedIden = @CTE_CedIden)
BEGIN
RAISERROR('El cliente especificado no existe en la tabla TB_CTEPPAL.', 16, 1);
RETURN;
END;

-- Verificar si ya existe un registro con la misma clave primaria (sin considerar Mail_Pref para la búsqueda de existencia)
IF EXISTS (SELECT 1 FROM TB_CTEMAIL
WHERE
CTE_Nacio = @CTE_Nacio AND
CTE_CedIden = @CTE_CedIden )
BEGIN
			-- Si existe, actualizar el registro existente
			UPDATE TB_CTEMAIL
			SET
			Mail_FecCrea = @Mail_FecCrea,
			USER_Crea = @USER_Crea,
			Mail_Loogin = @Mail_Loogin,
			Mail_Dominio='',
			Mail_Ext=''
			WHERE
			CTE_Nacio = @CTE_Nacio AND
			CTE_CedIden = @CTE_CedIden 

			SELECT 'Email del cliente actualizado correctamente.' AS Mensaje;


RETURN;
END;
ELSE
BEGIN

						-- Si no existe, insertar un nuevo registro
					INSERT INTO TB_CTEMAIL (
					[Ind_Mail],
					CTE_Nacio,
					CTE_CedIden,
					Mail_Loogin,
					Mail_Dominio,
					Mail_Ext,
					Mail_Pref,
					Mail_FecCrea,
					USER_Crea
					)
					VALUES (
					1,
					@CTE_Nacio,
					@CTE_CedIden,
					@Mail_Loogin,
					@Mail_Dominio,
					@Mail_Ext,
					1,
					@Mail_FecCrea,
					@USER_Crea
					);
					SELECT 'Email del cliente insertado correctamente.' AS Mensaje;
END;



END TRY
			BEGIN CATCH
							DECLARE @ErrorMessage NVARCHAR(4000),
							@ErrorSeverity INT,
							@ErrorState INT;

							SELECT
							@ErrorMessage = ERROR_MESSAGE(),
							@ErrorSeverity = ERROR_SEVERITY(),
							@ErrorState = ERROR_STATE();

							RAISERROR (
							@ErrorMessage, -- Mensaje de error devuelto al llamador
							@ErrorSeverity, -- Nivel de gravedad
							@ErrorState-- Número de estado.
							);
							RETURN;
			END CATCH;
END;
GO

/****** Object:  StoredProcedure [dbo].[SP_CPOSC_Insertar_TB_CTETLF]    Script Date: 06/06/2025 0:26:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO












CREATE PROCEDURE [dbo].[SP_CPOSC_Insertar_TB_CTETLF] (
    @CTE_Nacio CHAR(1),
    @CTE_CedIden NVARCHAR(10),
    @TLF_Tipo CHAR(3),
    @TLF_Cod CHAR(4),
    @TLF_Numero CHAR(7),
    @TLF_Ext NVARCHAR(4) = NULL,
    @TLF_FecCrea DATETIME = NULL,
    @USER_Crea CHAR(5)
)
AS
BEGIN
    SET NOCOUNT ON;

	
	declare @correlativo int


    IF @TLF_FecCrea IS NULL
    BEGIN
        SET @TLF_FecCrea = GETDATE();
    END;

    BEGIN TRY
        -- Verificar si el cliente existe en TB_CTEPPAL
        IF NOT EXISTS (SELECT 1 FROM TB_CTEPPAL WHERE CTE_Nacio = @CTE_Nacio AND CTE_CedIden = @CTE_CedIden)
        BEGIN
            RAISERROR('El cliente especificado no existe en la tabla TB_CTEPPAL.', 16, 1);
            RETURN;
        END;

        -- Verificar si ya existe un registro con la misma clave primaria (Ind_Tlf, CTE_Nacio, CTE_CedIden, TLF_Tipo)
        IF EXISTS (SELECT 1 FROM TB_CTETLF 
		WHERE 
		 CTE_Nacio = @CTE_Nacio AND 
		 CTE_CedIden = @CTE_CedIden AND 
		  TLF_Tipo = @TLF_Tipo )
        BEGIN
            -- Si existe, actualizar el registro existente
            UPDATE TB_CTETLF
            SET
                TLF_Cod = @TLF_Cod,                
                TLF_Ext = @TLF_Ext,
                TLF_FecCrea = @TLF_FecCrea,
                USER_Crea = @USER_Crea ,-- Considera si quieres actualizar el usuario de creación en una actualización
				   TLF_Numero = @TLF_Numero 
            WHERE
             
                CTE_Nacio = @CTE_Nacio AND
                CTE_CedIden = @CTE_CedIden AND
                TLF_Tipo = @TLF_Tipo;

            SELECT 'Teléfono del cliente actualizado correctamente' AS Mensaje;
            RETURN;
        END;
        ELSE
        BEGIN


		--SELECT @correlativo = MAX(Ind_Tlf) FROM TB_CTETLF 
		--WHERE 
		--CTE_Nacio = @CTE_Nacio AND
		--CTE_CedIden = @CTE_CedIden AND
  --              TLF_Tipo = @TLF_Tipo;

		--set @correlativo= ISNULL(@correlativo,0)+1




            -- Si no existe, insertar un nuevo registro
            INSERT INTO TB_CTETLF (
                Ind_Tlf,
                CTE_Nacio,
                CTE_CedIden,
                TLF_Tipo,
                TLF_Cod,
                TLF_Numero,
                TLF_Ext,
                TLF_FecCrea,
                USER_Crea
            )
            VALUES (
                1, -- Valor fijo para Ind_Tlf
                @CTE_Nacio,
                @CTE_CedIden,
                @TLF_Tipo,
                @TLF_Cod,
                @TLF_Numero,
                @TLF_Ext,
                @TLF_FecCrea,
                @USER_Crea
            );

            SELECT 'Teléfono del cliente insertado correctamente' AS Mensaje;
        END;

    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000),
                @ErrorSeverity INT,
                @ErrorState INT;

        SELECT
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        RAISERROR (
            @ErrorMessage,  -- Mensaje de error devuelto al llamador
            @ErrorSeverity, -- Nivel de gravedad
            @ErrorState    -- Número de estado.
        );
        RETURN;
    END CATCH;
END;
GO

/****** Object:  StoredProcedure [dbo].[SP_CPOSC_InsertarTB_CTEPPAL]    Script Date: 06/06/2025 0:26:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO














CREATE PROCEDURE [dbo].[SP_CPOSC_InsertarTB_CTEPPAL] (
@CTE_CedIden VARCHAR(20),
@CTE_Nacio VARCHAR(1),
@CTE_PNombre VARCHAR(50),
@CTE_SNombre VARCHAR(50) = '',
@CTE_PApellido VARCHAR(50) = '',
@CTE_SApellido VARCHAR(50) = '',
@CTE_FNac DATE,
@CTE_VIP BIT = 0,
@CTE_AfilVip VARCHAR(2) = NULL,
@CTE_FecAfil DATETIME = NULL,
@COD_STCTE VARCHAR(3),
@CTE_Sex VARCHAR(1) = NULL,
@CTE_CodOcup VARCHAR(4) = NULL,
@CTE_EdoCiv VARCHAR(1) = NULL,
@COD_Sucursal VARCHAR(4) = NULL,
@CTE_FecCreacion DATETIME,
@CTE_FecMod DATETIME = NULL,
@USER_CREA VARCHAR(50) = NULL,
@USER_MOD VARCHAR(50) = NULL,
@Direccion_fact VARCHAR(200) = '',
@Facebook VARCHAR(100) = NULL,
@Twitter VARCHAR(100) = NULL,
@Instagram VARCHAR(100) = NULL,
@CTE_RETISLR BIT = 0,
@CTE_RETIVA BIT = 0, 
 
@COD_Edo VARCHAR(2),
@COD_Ciud VARCHAR(3)
)
AS
BEGIN
SET NOCOUNT ON;


SET @Direccion_fact=''

BEGIN TRY






-- Verificar si ya existe un registro con la misma cédula y nacionalidad
IF EXISTS (SELECT 1 FROM [dbo].[TB_CTEPPAL] WHERE CTE_CedIden = @CTE_CedIden AND CTE_Nacio = @CTE_Nacio)
BEGIN
				-- Actualizar el registro existente
				UPDATE [dbo].[TB_CTEPPAL]
				SET
				CTE_PNombre =  @CTE_PNombre   ,
				CTE_SNombre =   '',
				CTE_PApellido = '',
				CTE_SApellido = '',
				CTE_FNac = @CTE_FNac,
				CTE_VIP = @CTE_VIP,
				CTE_AfilVip = @CTE_AfilVip,
				CTE_FecAfil = @CTE_FecAfil,
				--COD_STCTE = @COD_STCTE,
				COD_STCTE = '01',
				CTE_Sex = @CTE_Sex,
				CTE_CodOcup = @CTE_CodOcup,
				CTE_EdoCiv = @CTE_EdoCiv,
				COD_Sucursal = @COD_Sucursal,
				CTE_FecMod = @CTE_FecMod,
				USER_MOD = @USER_MOD,
				Direccion_fact = @Direccion_fact,
				Facebook = @Facebook,
				Twitter = @Twitter,
				Instagram = @Instagram,
				CTE_RETISLR = @CTE_RETISLR,
				CTE_RETIVA = @CTE_RETIVA
				--CTE_CedIdenP = @CTE_CedIdenP ,
				--CTE_NacioP = @CTE_NacioP,
				--COD_Edo= @COD_Edo ,
				--COD_Ciud=@COD_Ciud
				WHERE
				CTE_CedIden = @CTE_CedIden AND CTE_Nacio = @CTE_Nacio;





SELECT 'Cliente actualizado correctamente.';
END
ELSE
BEGIN


						IF  @CTE_CedIden  IS NOT NULL  
						BEGIN
						-- Insertar un nuevo registro
						INSERT INTO [dbo].[TB_CTEPPAL] (
						[CTE_CedIden],
						[CTE_Nacio],
						[CTE_PNombre],
						[CTE_SNombre],
						[CTE_PApellido],
						[CTE_SApellido],
						[CTE_FNac],
						[CTE_VIP],
						[CTE_AfilVip],
						[CTE_FecAfil],
						[COD_STCTE],
						[CTE_Sex],
						[CTE_CodOcup],
						[CTE_EdoCiv],
						[COD_Sucursal],
						[CTE_FecCreacion],
						[CTE_FecMod],
						[USER_CREA],
						[USER_MOD],
						[Direccion_fact],
						[Facebook],
						[Twitter],
						[Instagram],
						[CTE_RETISLR],
						[CTE_RETIVA]
						--[CTE_CedIdenP],
						--[CTE_NacioP],
						--COD_Edo,
						--COD_Ciud
						)
						VALUES (
						@CTE_CedIden,
						@CTE_Nacio,
						@CTE_PNombre,
						@CTE_SNombre,
						@CTE_PApellido,
						@CTE_SApellido,
						@CTE_FNac,
						@CTE_VIP,
						@CTE_AfilVip,
						@CTE_FecAfil,
						--@COD_STCTE,
						'01',
						@CTE_Sex,
						@CTE_CodOcup,
						@CTE_EdoCiv,
						@COD_Sucursal,
						@CTE_FecCreacion,
						@CTE_FecMod,
						@USER_CREA,
						@USER_MOD,
						@Direccion_fact,
						@Facebook,
						@Twitter,
						@Instagram,
						@CTE_RETISLR,
						@CTE_RETIVA
						--@CTE_CedIdenP,
						--@CTE_NacioP,
						--@COD_Edo ,
						--@COD_Ciud
						)
						END




						SELECT 'Cliente insertado correctamente.';
END




IF NOT EXISTS (SELECT [CTE_CedIden] FROM [dbo].[TB_MAESDIR]
                WHERE  ltrim(rtrim(CTE_Nacio)) =ltrim(rtrim(@CTE_Nacio))
                 AND ltrim(rtrim(CTE_CedIden)) = ltrim(rtrim(@CTE_CedIden))
            )
BEGIN

SELECT [CTE_CedIden] FROM [dbo].[TB_MAESDIR]
                WHERE  ltrim(rtrim(CTE_Nacio)) =ltrim(rtrim(@CTE_Nacio))
                 AND ltrim(rtrim(CTE_CedIden)) = ltrim(rtrim(@CTE_CedIden))

					  INSERT INTO [dbo].[TB_MAESDIR]
					  (
						  [DIR_Tipo],
						  [CTE_Nacio],
						  [CTE_CedIden],
						  [DIR_CodUrbSec],
						  [DIR_CodCiud],
						  [DIR_CodEdo],
						  [DIR_CodTipViv],
						  [DIR_Feccreacion],
						  [DIR_FecModif],
						  [USER_Crea],
						  [USER_Modif]
					  )
					  VALUES
					  (
						  '01',
						  @CTE_Nacio,
						  @CTE_CedIden,
						  '0001',
						  @COD_Ciud,
						  @COD_Edo,
						  '02',
						  @CTE_FecCreacion,
						  @CTE_FecMod,
						  @USER_CREA,
						  @USER_MOD
					  );
END
ELSE
BEGIN

--select 'update'
			  UPDATE [dbo].[TB_MAESDIR]
			  SET
				  DIR_CodCiud = @COD_Ciud,
				  DIR_CodEdo = @COD_Edo,
				  DIR_FecModif = @CTE_FecMod,
				  USER_Modif = @USER_MOD
			  WHERE
				 CTE_Nacio = @CTE_Nacio
				  AND CTE_CedIden = @CTE_CedIden;
END






END TRY
BEGIN CATCH
-- Manejar errores
DECLARE @ErrorMessage NVARCHAR(4000);
DECLARE @ErrorSeverity INT;
DECLARE @ErrorState INT;

SELECT
@ErrorMessage = ERROR_MESSAGE(),
@ErrorSeverity = ERROR_SEVERITY(),
@ErrorState = ERROR_STATE();

RAISERROR (@ErrorMessage, -- Mensaje de error devuelto al llamador
@ErrorSeverity, -- Nivel de gravedad
@ErrorState); -- Número de estado.
RETURN;
END CATCH;
END;
GO

/****** Object:  StoredProcedure [dbo].[SP_CPOSC_InsertarTB_CTEPPALP]    Script Date: 06/06/2025 0:26:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO













CREATE PROCEDURE [dbo].[SP_CPOSC_InsertarTB_CTEPPALP] (
@CTE_CedIden VARCHAR(20),
@CTE_Nacio VARCHAR(1),
@CTE_PNombre VARCHAR(50),
@CTE_SNombre VARCHAR(50) = '',
@CTE_PApellido VARCHAR(50) = '',
@CTE_SApellido VARCHAR(50) = '',
@CTE_FNac DATE,
@CTE_VIP BIT = 0,
@CTE_AfilVip VARCHAR(2) = NULL,
@CTE_FecAfil DATETIME = NULL,
@COD_STCTE VARCHAR(3),
@CTE_Sex VARCHAR(1) = NULL,
@CTE_CodOcup VARCHAR(4) = NULL,
@CTE_EdoCiv VARCHAR(1) = NULL,
@COD_Sucursal VARCHAR(4) = NULL,
@CTE_FecCreacion DATETIME,
@CTE_FecMod DATETIME = NULL,
@USER_CREA VARCHAR(50) = NULL,
@USER_MOD VARCHAR(50) = NULL,
@Direccion_fact VARCHAR(200) = '',
@Facebook VARCHAR(100) = NULL,
@Twitter VARCHAR(100) = NULL,
@Instagram VARCHAR(100) = NULL,
@CTE_RETISLR BIT = 0,
@CTE_RETIVA BIT = 0, 
 
@COD_Edo VARCHAR(2),
@COD_Ciud VARCHAR(3)
)
AS
BEGIN
SET NOCOUNT ON;


SET @Direccion_fact=''

BEGIN TRY






-- Verificar si ya existe un registro con la misma cédula y nacionalidad
IF EXISTS (SELECT 1 FROM [dbo].[TB_CTEPPAL] WHERE CTE_CedIden = @CTE_CedIden AND CTE_Nacio = @CTE_Nacio)
BEGIN
				-- Actualizar el registro existente
				UPDATE [dbo].[TB_CTEPPAL]
				SET
				CTE_PNombre =  @CTE_PNombre   ,
				CTE_SNombre =   '',
				CTE_PApellido = '',
				CTE_SApellido = '',
				CTE_FNac = @CTE_FNac,
				CTE_VIP = @CTE_VIP,
				CTE_AfilVip = @CTE_AfilVip,
				CTE_FecAfil = @CTE_FecAfil,
				--COD_STCTE = @COD_STCTE,
				COD_STCTE = '01',
				CTE_Sex = @CTE_Sex,
				CTE_CodOcup = @CTE_CodOcup,
				CTE_EdoCiv = @CTE_EdoCiv,
				COD_Sucursal = @COD_Sucursal,
				CTE_FecMod = @CTE_FecMod,
				USER_MOD = @USER_MOD,
				Direccion_fact = @Direccion_fact,
				Facebook = @Facebook,
				Twitter = @Twitter,
				Instagram = @Instagram,
				CTE_RETISLR = @CTE_RETISLR,
				CTE_RETIVA = @CTE_RETIVA
				--CTE_CedIdenP = @CTE_CedIdenP ,
				--CTE_NacioP = @CTE_NacioP,
				--COD_Edo= @COD_Edo ,
				--COD_Ciud=@COD_Ciud
				WHERE
				CTE_CedIden = @CTE_CedIden AND CTE_Nacio = @CTE_Nacio;





SELECT 'Cliente actualizado correctamente.';
END
ELSE
BEGIN


						IF  @CTE_CedIden  IS NOT NULL  
						BEGIN
						-- Insertar un nuevo registro
						INSERT INTO [dbo].[TB_CTEPPAL] (
						[CTE_CedIden],
						[CTE_Nacio],
						[CTE_PNombre],
						[CTE_SNombre],
						[CTE_PApellido],
						[CTE_SApellido],CTE_FNac,COD_Sucursal ,USER_CREA,USER_MOD
						
						)
						VALUES (
						@CTE_CedIden,
						@CTE_Nacio,
						@CTE_PNombre,
						@CTE_SNombre,
						@CTE_PApellido,
						@CTE_SApellido,'01/01/9001', @COD_Sucursal,  @USER_CREA,  @USER_CREA
						)
						END




						SELECT 'Cliente insertado correctamente.';
END







END TRY
BEGIN CATCH
-- Manejar errores
DECLARE @ErrorMessage NVARCHAR(4000);
DECLARE @ErrorSeverity INT;
DECLARE @ErrorState INT;

SELECT
@ErrorMessage = ERROR_MESSAGE(),
@ErrorSeverity = ERROR_SEVERITY(),
@ErrorState = ERROR_STATE();

RAISERROR (@ErrorMessage, -- Mensaje de error devuelto al llamador
@ErrorSeverity, -- Nivel de gravedad
@ErrorState); -- Número de estado.
RETURN;
END CATCH;
END;
GO

/****** Object:  StoredProcedure [dbo].[SP_CPOSC_ObtenerExamenPorNumeroYNacionalidadCedula]    Script Date: 06/06/2025 0:26:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO








CREATE PROCEDURE [dbo].[SP_CPOSC_ObtenerExamenPorNumeroYNacionalidadCedula]
    @NumeroExamen INT,
    @Nacionalidad VARCHAR(10),
    @Cedula VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        CTE_Nacio,
        CTE_CedIden,
        COD_Sucursal,
        NUM_Examen,
        FEC_Examen,
        ISNULL(ESFD, 0) AS ESFD,
        ISNULL(ESFI, 0) AS ESFI,
        ISNULL(CILD, 0) AS CILD,
        ISNULL(CILI, 0) AS CILI,
        ISNULL(EJED, 0) AS EJED,
        ISNULL(EJEI, 0) AS EJEI,
        ISNULL(ADDD, 0) AS ADDD,
        ISNULL(ADDI, 0) AS ADDI,
        ISNULL(OBSERVACIONES, '') AS OBSERVACIONES, -- Devuelve cadena vacía para texto
        TIPO_Optm,
        NOM_Optm,
        EXA_Feccreacion,
        EXA_Fecmod,
        USER_CREA,
        USER_MOD,
        TIPOEXAMEN,
        NOMBRE_CLINICA_OPTM,
        ISNULL(TLF_TIPO, 0) AS TLF_TIPO,
        ISNULL(TLF_COD, 0) AS TLF_COD,
        ISNULL(TLF_NUMERO, 0) AS TLF_NUMERO,
        ISNULL(TLF_EXT, 0) AS TLF_EXT,
        ISNULL(ESFD2, 0) AS ESFD2,
        ISNULL(ESFI2, 0) AS ESFI2,
        ISNULL(CILD2, 0) AS CILD2,
        ISNULL(CILI2, 0) AS CILI2,
        ISNULL(EJED2, 0) AS EJED2,
        ISNULL(EJEI2, 0) AS EJEI2,
        ISNULL(CodigoMimesys, 0) AS CodigoMimesys
    FROM
        [dbo].[TB_EXAMEN]
    WHERE
        NUM_Examen = @NumeroExamen AND CTE_Nacio = @Nacionalidad AND CTE_CedIden = @Cedula;
END;
GO


