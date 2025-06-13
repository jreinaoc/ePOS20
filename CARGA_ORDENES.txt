
/****** Object:  Table [dbo].[TB_AGREGADO2]    Script Date: 05/16/2025 15:55:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_AGREGADO2]') AND type in (N'U'))
DROP TABLE [dbo].[TB_AGREGADO2]
GO


/****** Object:  Table [dbo].[TB_AGREGADO2]    Script Date: 05/16/2025 15:55:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TB_AGREGADO2](
	[COD_Agregado] [char](3) NOT NULL,
	[Agregado_DESCRIP] [nvarchar](50) NOT NULL,
	[Agregado_Cond] [nvarchar](150) NULL,
	[Agregado_Producto] [nvarchar](80) NULL,
	[Fec_Crea] [smalldatetime] NOT NULL,
	[Fec_Mod] [smalldatetime] NULL,
	[USER_Crea] [char](5) NOT NULL,
	[USER_Mod] [char](5) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


INSERT [dbo].[TB_AGREGADO2] ([COD_Agregado], [Agregado_DESCRIP], [Agregado_Cond], [Agregado_Producto], [Fec_Crea], [Fec_Mod], [USER_Crea], [USER_Mod]) VALUES (N'005', N'Esfera Der. > 6-10', N'((ESFERA_DER > 6 && ESFERA_DER <= 10) || (ESFERA_DER < -6 && ESFERA_DER >= -10)) && (OJO == "D")', N'"S000103"', CAST(0x975602CE AS SmallDateTime), NULL, N'00001', NULL)
INSERT [dbo].[TB_AGREGADO2] ([COD_Agregado], [Agregado_DESCRIP], [Agregado_Cond], [Agregado_Producto], [Fec_Crea], [Fec_Mod], [USER_Crea], [USER_Mod]) VALUES (N'006', N'Esfera Izq. > 6-10', N'((ESFERA_IZQ > 6 && ESFERA_IZQ <= 10) || (ESFERA_IZQ < -6 && ESFERA_IZQ >= -10)) && (OJO == "I")', N'"S000104"', CAST(0x975602D7 AS SmallDateTime), NULL, N'00001', NULL)
INSERT [dbo].[TB_AGREGADO2] ([COD_Agregado], [Agregado_DESCRIP], [Agregado_Cond], [Agregado_Producto], [Fec_Crea], [Fec_Mod], [USER_Crea], [USER_Mod]) VALUES (N'007', N'Esfera Der. 10-15', N'((ESFERA_DER > 10 && ESFERA_DER <= 15) || (ESFERA_DER >= -15 && ESFERA_DER < -10)) && (OJO == "D")', N'"S000107"', CAST(0x975602DB AS SmallDateTime), NULL, N'00001', NULL)
INSERT [dbo].[TB_AGREGADO2] ([COD_Agregado], [Agregado_DESCRIP], [Agregado_Cond], [Agregado_Producto], [Fec_Crea], [Fec_Mod], [USER_Crea], [USER_Mod]) VALUES (N'008', N'Cilindro Der. >6', N'(CILINDRO_DER < -6 || CILINDRO_DER > 6) && (OJO == "D")', N'"S000117"', CAST(0x975602E1 AS SmallDateTime), NULL, N'00001', NULL)
INSERT [dbo].[TB_AGREGADO2] ([COD_Agregado], [Agregado_DESCRIP], [Agregado_Cond], [Agregado_Producto], [Fec_Crea], [Fec_Mod], [USER_Crea], [USER_Mod]) VALUES (N'009', N'Esfera Izq. 10-15', N'((ESFERA_IZQ > 10 && ESFERA_IZQ <= 15) || (ESFERA_IZQ >= -15 && ESFERA_IZQ < -10)) && (OJO == "I")', N'"S000108"', CAST(0x979203EB AS SmallDateTime), NULL, N'00001', NULL)
INSERT [dbo].[TB_AGREGADO2] ([COD_Agregado], [Agregado_DESCRIP], [Agregado_Cond], [Agregado_Producto], [Fec_Crea], [Fec_Mod], [USER_Crea], [USER_Mod]) VALUES (N'010', N'Esfera Der. > 15', N'((ESFERA_DER > 15 || ESFERA_DER < -15)) && (OJO == "D")', N'"S000111"', CAST(0x979203F1 AS SmallDateTime), NULL, N'00001', NULL)
INSERT [dbo].[TB_AGREGADO2] ([COD_Agregado], [Agregado_DESCRIP], [Agregado_Cond], [Agregado_Producto], [Fec_Crea], [Fec_Mod], [USER_Crea], [USER_Mod]) VALUES (N'011', N'Esfera Izq. > 15', N'((ESFERA_IZQ > 15 || ESFERA_IZQ < -15)) && (OJO == "I")', N'"S000112"', CAST(0x979203F4 AS SmallDateTime), NULL, N'00001', NULL)
INSERT [dbo].[TB_AGREGADO2] ([COD_Agregado], [Agregado_DESCRIP], [Agregado_Cond], [Agregado_Producto], [Fec_Crea], [Fec_Mod], [USER_Crea], [USER_Mod]) VALUES (N'012', N'Cilindro Derecho 3-6', N'((CILINDRO_DER >= -6 && CILINDRO_DER <= -3) || (CILINDRO_DER >= 3 && CILINDRO_DER <= 6)) && (OJO == "D")', N'"S000113"', CAST(0x979203F9 AS SmallDateTime), NULL, N'00001', NULL)
INSERT [dbo].[TB_AGREGADO2] ([COD_Agregado], [Agregado_DESCRIP], [Agregado_Cond], [Agregado_Producto], [Fec_Crea], [Fec_Mod], [USER_Crea], [USER_Mod]) VALUES (N'013', N'Cilindro Izquierdo 3-6', N'((CILINDRO_IZQ >= -6 && CILINDRO_IZQ <= -3) || (CILINDRO_IZQ >= 3 && CILINDRO_IZQ <= 6)) && (OJO == "I")', N'"S000114"', CAST(0x979203FA AS SmallDateTime), NULL, N'00001', NULL)
INSERT [dbo].[TB_AGREGADO2] ([COD_Agregado], [Agregado_DESCRIP], [Agregado_Cond], [Agregado_Producto], [Fec_Crea], [Fec_Mod], [USER_Crea], [USER_Mod]) VALUES (N'014', N'Cilindro Derecho 3', N'((CILINDRO_DER > -3 || CILINDRO_DER < 3) && (CILINDRO_DER != 0)) && (OJO == "D")', NULL, CAST(0x979203FD AS SmallDateTime), CAST(0x9C400224 AS SmallDateTime), N'00001', N'00001')
INSERT [dbo].[TB_AGREGADO2] ([COD_Agregado], [Agregado_DESCRIP], [Agregado_Cond], [Agregado_Producto], [Fec_Crea], [Fec_Mod], [USER_Crea], [USER_Mod]) VALUES (N'015', N'Cilindro Izquierdo 3', N'((CILINDRO_IZQ > -3 || CILINDRO_IZQ < 3) && (CILINDRO_IZQ != 0)) && (OJO == "I")', NULL, CAST(0x979203FD AS SmallDateTime), CAST(0x9C400224 AS SmallDateTime), N'00001', N'00001')
INSERT [dbo].[TB_AGREGADO2] ([COD_Agregado], [Agregado_DESCRIP], [Agregado_Cond], [Agregado_Producto], [Fec_Crea], [Fec_Mod], [USER_Crea], [USER_Mod]) VALUES (N'016', N'Cilindro Izq. >6', N'(CILINDRO_IZQ < -6 || CILINDRO_IZQ > 6) && (OJO == "I")', N'"S000118"', CAST(0x979203FE AS SmallDateTime), NULL, N'00001', NULL)



---------------------------------------------------------------------------------------------------------------
/****** Object:  StoredProcedure [dbo].[CPOS_pPromoCombosConfig2020]    Script Date: 05/15/2025 10:48:53 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CPOS_pPromoCombosConfig2020]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CPOS_pPromoCombosConfig2020]
GO



/****** Object:  StoredProcedure [dbo].[CPOS_pPromoCombosConfig2020]    Script Date: 05/15/2025 10:48:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





--pPromoCombosConfig2020 'M162626','C000394',0,0,'',0,'002','2020-07-25' 	
--EXEC CPOS_pPromoCombosConfig2020 'M144903', 'C000001', '0', '0', 'S000134', 'True', '002', '2022/07/03'	
--EXEC CPOS_pPromoCombosConfig2020 'M144903', 'C000001', '0', '0', '', 'False', '002', '2022/07/03'
CREATE PROCEDURE [dbo].[CPOS_pPromoCombosConfig2020]
	  @MONTURA AS VARCHAR(7) 
	, @CRISTAL AS VARCHAR(7)  
	, @MONTURAPROPIA AS BIT
	, @CRISTALPROPIO AS BIT
	, @AR AS VARCHAR(7)
	, @SERVAR AS BIT
	, @TIPOVENTA AS VARCHAR(3) -- "001 Venta Directa" o "002 CONVENCIONAL"
	, @Fecha  as datetime 
	
AS 
	DECLARE @DESCMONT AS INTEGER
	DECLARE @TIPART AS VARCHAR (3) 
	DECLARE @APLICA AS VARCHAR (11) 
	DECLARE @FULLDOL AS NUMERIC(10,7) 
	--declare  @Fecha datetime 
	--set @Fecha = GETDATE()
	
	
	SET @APLICA = 'NO APLICA'
	SET @FULLDOL = 0
	
	-- MONTURA    
	DECLARE @LETRAMLS AS VARCHAR (3) 
	DECLARE @CODGRUPORANGO AS VARCHAR (3) 
	DECLARE @PRECIOMO AS NUMERIC (18,2) 

	-- CRISTAL
	DECLARE @PRECIOCR AS NUMERIC(28,2) 

	-- ANTIREFLEJO CLEAR
	DECLARE @PRECIOAR AS NUMERIC(28,2)  
	DECLARE @CONTAREG AS INTEGER 

	-- JR
	DECLARE @PVPMONTURA AS NUMERIC(28,2)
	DECLARE @PVPMONTURASINIVA AS NUMERIC(28,2)
	DECLARE @PVPCRISTAL AS NUMERIC(28,2)
	DECLARE @PVPAR AS NUMERIC(28,7)
	DECLARE @PORCIVA AS NUMERIC(28,7)
	DECLARE @PVPVENTAFULL AS NUMERIC(28,7)
	DECLARE @PVPVENTAFULLDOLAR AS NUMERIC(28,7)
	DECLARE @PVPVENTAFULLDOLARCOOMBO AS NUMERIC(28,7)
	DECLARE @MONTODESCUENTO AS NUMERIC(28,7)
	DECLARE @IDTASA AS INTEGER
	DECLARE @TASA AS NUMERIC(28,7)
	DECLARE @FINALPVPMONTURA AS NUMERIC(28,7)
	DECLARE @FINALPVPCRISTAL AS NUMERIC(28,7)
	DECLARE @FINALPVPAR AS NUMERIC(28,7)
	DECLARE @RESTODESCUENTO AS NUMERIC(28,7)
	DECLARE @FINALPVPMONTURAIVA AS NUMERIC(28,7)
	DECLARE @FINALPVPMONTURASINIVA AS NUMERIC(28,7)
	DECLARE @FINALPVPVENTAFULLSINIVA AS NUMERIC(28,7)
	-- JR
	
	DECLARE @APLICAAR AS BIT
	DECLARE @NUMCOMBO AS INTEGER
	
	-----------LETRA DE LA MONTURA-------------------
	SELECT	@LETRAMLS=CodRango ,
			@CODGRUPORANGO = CodGrupoRango, 
			@TIPART = TIPART, 
			@PRECIOMO = ART_PVP,
			@DESCMONT = PorctDescuento
	FROM TB_ARTICULO 
	WHERE CodArticulo = @MONTURA
	
						
	-- PRECIO DEL CRISTAL
		SELECT @PRECIOCR = art_pvp
		FROM TB_ARTICULO 
		WHERE CodArticulo = @CRISTAL

		--SET @PRECIOAR = ''
		-- PRECIO DEL AR
		IF (@AR <> '' AND @AR IS NOT NULL)
		BEGIN
			SELECT @PRECIOAR = art_pvp
			FROM TB_ARTICULO 
			WHERE CodArticulo = @AR
		END 
		
		--Obtengo la ultima tasa para dividir el total de la venta entre la tasa
		--Y asi obtener el total de la venta en dolares
		select @IDTASA = MAX(id_tasa)from TB_TASA WHERE COD_moneda = '01'  
		SELECT @TASA = tasa FROM TB_TASA where ID_Tasa  = @IDTASA  
	
	SET @CONTAREG = 0
	 -- pPromoCombosConfig2020 'M171177', 'C000412', '0', '0', 'S000134', 'True', '002', '2020/07/22'

	-- pPromoCombosConfig2020 'M145816','C000594',0,0,'S000134',0,'002','2020-07-25'  --N  
	-- pPromoCombosConfig2020 'M171177','C000412',0,0,'',0,'002','2020-07-25'  --N 
	-- pPromoCombosConfig2020 'M171177','C000412',0,0,'S000134',0,'002','2020-07-25'  --N   
	-- pPromoCombosConfig2020 'M171177', 'C000412', '0', '0', 'S000134', 'True', '002', '2020/07/22'
    IF @DESCMONT = 0 -- DESC MONTURA
	BEGIN 
		IF (@MONTURAPROPIA = 0 AND @CRISTALPROPIO = 0) 
		BEGIN -- MONTURA Y CRISTAL PROPIO
			--                       VENTA CONVENCIONAL 
			-- *********************************************************************************************************
			IF (@TIPOVENTA = '002')
			BEGIN    
			
			SELECT  Id_Combo, CodAntireflejo, AplicaAntireflejo,  MontoDolar
			INTO #TEMP_COMBO
			FROM  TB_PROMCOMBOCONFIG p
			inner join TB_ARTICULO a
			on p.RangoInicio = a.CodRango 
			WHERE   substring(@LETRAMLS,1,1) between substring(RangoInicio,1,1) and  substring(RangoFin,1,1) 
			AND CodCristal = @CRISTAL  and a.CodGrupoRango = @CODGRUPORANGO 
			AND GETDATE() between FechaInicio and FechaFin +'12:59:00'
			group by Id_Combo, CodAntireflejo, AplicaAntireflejo,  MontoDolar
			
			--select * from #TEMP_COMBO
			

			--SELECT   Id_Combo, CodAntireflejo, AplicaAntireflejo,  MontoDolar
			--INTO #TEMP_COMBO
			--FROM  TB_PROMCOMBOCONFIG
			--WHERE RangoInicio <= @LETRAMLS  AND RangoFin >= @LETRAMLS
			--		  AND CodCristal = @CRISTAL  
			--		  AND ((FechaInicio <= @Fecha) and (FechaFin >= @Fecha)) 
				
            SET @APLICAAR = 0			
            
			--SELECT @APLICAAR = AplicaAntireflejo  FROM #TEMP_COMBO WHERE CodAntireflejo = @AR AND AplicaAntireflejo = 1
			--SELECT @APLICAAR = AplicaAntireflejo  FROM #TEMP_COMBO WHERE AplicaAntireflejo = 1
			
			--IF( @APLICAAR = 0)
			--BEGIN
					SELECT @CONTAREG = COUNT(*) FROM #TEMP_COMBO WHERE AplicaAntireflejo = 0 
					IF @CONTAREG = 1
					BEGIN
						SELECT @NUMCOMBO = Id_Combo, @FULLDOL = MontoDolar FROM #TEMP_COMBO WHERE AplicaAntireflejo = 0
						SET @APLICA = 'APLICA'
						--select @NUMCOMBO
					END	
					ELSE
					BEGIN
						SET @APLICA = 'NO APLICA'
					END		
			--END
			--ELSE IF( @APLICAAR = 1)
			--BEGIN 
			--print 'b'
			--		SELECT @CONTAREG = COUNT(*)  FROM #TEMP_COMBO WHERE CodAntireflejo = @AR 
			--		IF @CONTAREG = 1
			--		BEGIN
			--		print 'b'
			--			SELECT @NUMCOMBO = Id_Combo, @FULLDOL = MontoDolar FROM #TEMP_COMBO WHERE CodAntireflejo = @AR
			--			SET @APLICA = 'APLICA'
			--		END
			--		ELSE
			--		BEGIN
			--			SET @APLICA = 'NO APLICA'
			--		END
			--END 
			
			DROP TABLE #TEMP_COMBO

			
			IF ( @APLICA = 'APLICA' ) 
			BEGIN
			-- JR 
			-- pPromoCombosConfig2020 'M171177','C000412',0,0,'',0,'002','2020-07-25'  --N
		

				IF (@APLICAAR = 0)
				BEGIN
				
					select @PORCIVA = porcentaje from TB_IMPDES where cod_tipo = 'I'
					--En este caso estoy asumiendo que es una venta solo montura y cristal cantidad 2
					--Le sumo el iva a la montura para obtener el total igual al de epos
					select @PVPMONTURASINIVA = art_pvp  from TB_ARTICULO where CodArticulo = @MONTURA 
					select @PVPMONTURA = art_pvp + ((ART_PVP * @PORCIVA)/100) from TB_ARTICULO where CodArticulo = @MONTURA 
					select @PVPCRISTAL = art_pvp * 2 from TB_ARTICULO where CodArticulo = @CRISTAL  			

					--Obtengo el total de la venta en Bs
					SELECT @PVPVENTAFULL = @PVPMONTURA+@PVPCRISTAL

					--Obtengo el total de la venta en Dolares
					select @PVPVENTAFULLDOLAR =  ROUND(@PVPVENTAFULL/@TASA,2)

					--Obtengo el monto en Bs que debe dar el total de mi orden aplicando la promoción
					--Suponiendo que el precio full de la venta es 37 Dolares y aplicando la promo es 30 Dolares 
					--Ese valor 30 es el que se debe configurar en la tabla para el combo montura Letra G cristal C000001 totalcombo 30
					select @PVPVENTAFULLDOLARCOOMBO = (@FULLDOL * @PVPVENTAFULL) /ROUND(@PVPVENTAFULLDOLAR,2)
-- pPromoCombosConfig2020 'M171177','C000412',0,0,'',0,'002','2020-07-25'  --N

					--Obtengo el monto de descuento en Bs Para saber cuanto descontar a los articulos
					select @MONTODESCUENTO = @PVPVENTAFULL - @PVPVENTAFULLDOLARCOOMBO

					--Si el monto a descontar es mayor que la montura, la dejo en 0.01 y el resto de 
					--de mi descuento se lo resto al cristal
					IF  @MONTODESCUENTO	>= @PVPMONTURA
					BEGIN
						 SET @FINALPVPMONTURA = 0.01
						 SET @RESTODESCUENTO = @MONTODESCUENTO	- @PVPMONTURA
						 SET @FINALPVPCRISTAL = @PVPCRISTAL	 - @RESTODESCUENTO  	 
					END
					--Si no es mayor a la montura lo resto todo de la montura y dejo el cristal
					--con su precio original
					ELSE
					BEGIN 
						SET @FINALPVPMONTURA = @PVPMONTURA - @MONTODESCUENTO  
						SET @FINALPVPCRISTAL = @PVPCRISTAL  
					END
					IF @PORCIVA > 0 
					BEGIN
					--Extraigo el Iva del Precio final que obtuve para la montura, para que cuando epos 
						--calcule el Iva sean iguales los montos ya que el precio de la montura en tb_articulo
						--siempre esta sin iva y epos es quien lo calcula
						set @FINALPVPMONTURAIVA  = (@FINALPVPMONTURA * 1.16) - @FINALPVPMONTURA  

						--Resto el Iva al monto final que obtuve para la montura
						set @FINALPVPMONTURA = @FINALPVPMONTURA - @FINALPVPMONTURAIVA
					END
					--Muestro los nuevos precios que enviamos a epos 
					
					SET @FINALPVPCRISTAL = @FINALPVPCRISTAL/2 
					
   
					 SET @FINALPVPAR = @PRECIOAR 
						
					-- JR
				END
				--ELSE
				--BEGIN 
				---- pPromoCombosConfig2020 'M171177', 'C000412', '0', '0', 'S000134', 'True', '002', '2020/07/22'
				--	--SELECT 'S'
				--		select @PORCIVA = porcentaje from TB_IMPDES where cod_tipo = 'I'
				--		--En este caso estoy asumiendo que es una venta solo montura y cristal cantidad 2
				--		--Le sumo el iva a la montura para obtener el total igual al de epos
				--		select @PVPMONTURASINIVA = art_pvp  from TB_ARTICULO where CodArticulo = @MONTURA 
				--		select @PVPMONTURA = art_pvp + ((ART_PVP * @PORCIVA)/100) from TB_ARTICULO where CodArticulo = @MONTURA 
				--		select @PVPCRISTAL = art_pvp * 2 from TB_ARTICULO where CodArticulo = @CRISTAL  
				--		select @PVPAR = art_pvp * 2 from TB_ARTICULO where CodArticulo = @AR  				

				--		--Obtengo el total de la venta en Bs
				--		SELECT @PVPVENTAFULL = @PVPMONTURA+@PVPCRISTAL+@PVPAR
						

				--		--Obtengo el total de la venta en Dolares
				--		select @PVPVENTAFULLDOLAR =  ROUND(@PVPVENTAFULL/@TASA,6)

				--		--Obtengo el monto en Bs que debe dar el total de mi orden aplicando la promoción
				--		--Suponiendo que el precio full de la venta es 37 Dolares y aplicando la promo es 30 Dolares 
				--		--Ese valor 30 es el que se debe configurar en la tabla para el combo montura Letra G cristal C000001 totalcombo 30
				--		select @PVPVENTAFULLDOLARCOOMBO = @FULLDOL * @PVPVENTAFULL  /@PVPVENTAFULLDOLAR  
						
				--		--Obtengo el monto de descuento en Bs Para saber cuanto descontar a los articulos
				--		select @MONTODESCUENTO = @PVPVENTAFULL - @PVPVENTAFULLDOLARCOOMBO 
				--		--Si el monto a descontar es mayor que la montura, la dejo en 0.01 y el resto de 
				--		--de mi descuento se lo resto al cristal
				--		--Si el monto a descontar es mayor que la montura, la dejo en 0.01 y el resto de 
				--		--de mi descuento se lo resto al cristal
						
				--		IF  @MONTODESCUENTO	>= @PVPMONTURA
				--		BEGIN 
				--			SET @FINALPVPMONTURA = 0.01
				--			-- SELECT @MONTODESCUENTO
				--			SET @RESTODESCUENTO = @MONTODESCUENTO	- @PVPMONTURA
							
				--			IF @RESTODESCUENTO	> @PVPCRISTAL
				--			BEGIN
				--				SET @FINALPVPCRISTAL = 0.02
				--				SET @RESTODESCUENTO = @RESTODESCUENTO - (@PVPCRISTAL) 
				--				SET @FINALPVPAR = @PVPAR - @RESTODESCUENTO  
				--			--Si no es mayor a la montura lo resto todo de la montura y dejo el cristal
				--			--con su precio original 
				--			END
				--			ELSE
				--			BEGIN 
				--				SET @FINALPVPCRISTAL = @PVPCRISTAL - @RESTODESCUENTO 
				--				SET @FINALPVPAR = @PVPAR  
				--			END 
				--		END
				--		ELSE
				--		BEGIN
				--			SET @FINALPVPMONTURA = @PVPMONTURA - @MONTODESCUENTO
				--			SET @FINALPVPCRISTAL = @PVPCRISTAL  
				--			SET @FINALPVPAR =  @PVPAR  
				--		END
						 
				--		IF @PORCIVA > 0 
				--		BEGIN
				--			--Extraigo el Iva del Precio final que obtuve para la montura, para que cuando epos 
				--			--calcule el Iva sean iguales los montos ya que el precio de la montura en tb_articulo
				--			--siempre esta sin iva y epos es quien lo calcula
				--			set @FINALPVPMONTURAIVA  = (@FINALPVPMONTURA * 1.16) - @FINALPVPMONTURA  

				--			--Resto el Iva al monto final que obtuve para la montura
				--			set @FINALPVPMONTURA = @FINALPVPMONTURA - @FINALPVPMONTURAIVA
				--		END
				--		--Muestro los nuevos precios que enviamos a epos
				--		--select  @FINALPVPMONTURA FINALPVPMONTURA , @FINALPVPCRISTAL/2 FINALPVPCRISTAL
						
				--		SET @FINALPVPCRISTAL =( @FINALPVPCRISTAL/2)
						
	   
				--		 SET @FINALPVPAR = (@FINALPVPAR/2)
							
				--		-- JR
				--	END
				--END 
			  				
												 
			END -- TIPO VENTA 002
			
			
          END -- MONTURA Y CRISTAL PROPIO
  
  	END -- DESC MONTURA 
DECLARE @PRECIOCOMBOEPOS AS NUMERIC (28,2)
--SET @PRECIOCOMBOEPOS = (@FINALPVPMONTURA  + (@FINALPVPMONTURA*0.16) + (@FINALPVPCRISTAL * 2) + isnull(@FINALPVPAR* 2,0)) / @TASA  
--select @PRECIOCOMBOEPOS

--EXEC pPromoCombosConfig2020 'M144903', 'C000001', '0', '0', 'S000134', 'True', '002', '2022/07/03'	
--EXEC pPromoCombosConfig2020 'M144903', 'C000001', '0', '0', '', 'False', '002', '2022/07/03'

--select @FINALPVPMONTURA,@FINALPVPCRISTAL,@FINALPVPAR,@TASA
IF @PORCIVA > 0 
BEGIN
print 'v'
	DECLARE @DIFERENCIADECIMALES AS NUMERIC (28,2)
	DECLARE @DIFERENCIABS AS NUMERIC (28,2)
	DECLARE @IVADIFERENCIABS AS NUMERIC (28,2)

	--SET @PRECIOCOMBOEPOS = (@FINALPVPMONTURA  + (@FINALPVPMONTURA*0.16) + (@FINALPVPCRISTAL * 2) + isnull(@FINALPVPAR* 2,0)) / @TASA  
	SET @PRECIOCOMBOEPOS = (@FINALPVPMONTURA  + (@FINALPVPMONTURA*0.16) + (@FINALPVPCRISTAL * 2) ) / @TASA  

	SET @DIFERENCIADECIMALES  =   @FULLDOL-@PRECIOCOMBOEPOS

	set @DIFERENCIABS = ( @TASA * @DIFERENCIADECIMALES)
	
	-- pPromoCombosConfig2020 'M161208','C000001',0,0,'',0,'002','2020-07-25'  --N  
	--SELECT @PRECIOCOMBOEPOS,@FULLDOL,@DIFERENCIADECIMALES,@DIFERENCIABS

	set @IVADIFERENCIABS = (@DIFERENCIABS * 1.16) - @DIFERENCIABS  
	select @DIFERENCIABS = @DIFERENCIABS - @IVADIFERENCIABS
	
	--select @DIFERENCIABS
	--Resto el Iva al monto final que obtuve para la montura
	--set @FINALPVPMONTURA = @FINALPVPMONTURA - @FINALPVPMONTURAIVA


	IF  @DIFERENCIABS	>= @FINALPVPMONTURA
	BEGIN
		 SET @FINALPVPMONTURA = @DIFERENCIABS	 
	END
	--Si no es mayor a la montura lo resto todo de la montura y dejo el cristal
	--con su precio original
	ELSE
	BEGIN 
		SET @FINALPVPMONTURA = @FINALPVPMONTURA + @DIFERENCIABS  
		SET @FINALPVPCRISTAL = @FINALPVPCRISTAL  
	END
END					  	
 SELECT @APLICA Resultado,
		'182' CODPROM,
		@LETRAMLS LETRA, 
		@MONTURA MONTURA,
		@PRECIOMO PRECIOMO, 
		case WHEN  CAST(ROUND(@FINALPVPMONTURA,2) as decimal(18,2) ) <= 0 then 0.01
		else CAST(ROUND(@FINALPVPMONTURA,2) as decimal(18,2) ) end  PRECIOMONT_DESC,
		(@FINALPVPMONTURA*@PORCIVA )/100 IVAMONTURAEPOS,
		@CRISTAL CRISTAL, 
		@PRECIOCR PRECIOCR, 
		case WHEN  CAST(ROUND(@FINALPVPMONTURA,2) as decimal(18,2) ) <= 0 then CAST(ROUND(@FINALPVPCRISTAL-0.01,2) as decimal(18,2) )else CAST(ROUND(@FINALPVPCRISTAL,2) as decimal(18,2) )end PRECIOCRT_DESC,
		--
		@AR AR, 
		ISNULL(@PRECIOAR,0) PRECIOAR, 
		ROUND(@FINALPVPAR,2) PRECIOAR_DESC ,
		@APLICAAR APLICARAR,@PVPVENTAFULLDOLAR PVPVENTAFULLDOLAR,@PVPVENTAFULL PVPVENTAFULLBS,@MONTODESCUENTO MONTODESCUENTO ,@TASA TASA
		
		 
		SELECT * FROM  TB_PROMCOMBOCONFIG WHERE	 ID_Combo =  @NUMCOMBO	
		
		IF @APLICAAR = 1
		 select @FINALPVPAR
		ELSE
			 
--set @FINALPVPMONTURASINIVA  = @FINALPVPMONTURA  - @FINALPVPMONTURAIVA	
SET @PRECIOCOMBOEPOS = (@FINALPVPMONTURA  + ((@FINALPVPMONTURA*@PORCIVA )/100) + (@FINALPVPCRISTAL * 2) ) / @TASA  
select @PRECIOCOMBOEPOS PRECIOCOMBOEPOS

					



END



GO



-----------------------------------------------------------------------------------------------------


/****** Object:  StoredProcedure [dbo].[SP_CPOS_ADD_TB_TRABAJO]    Script Date: 05/10/2025 08:51:38 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CPOS_ADD_TB_TRABAJO]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_CPOS_ADD_TB_TRABAJO]
GO


/****** Object:  StoredProcedure [dbo].[SP_CPOS_ADD_TB_TRABAJO]    Script Date: 05/10/2025 08:51:38 ******/
SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER OFF
GO





/* Realizado por Mayerlyn Campos
    Fecha de Creación: 18/03/2008.
    Este stored inserta el registro de tb_trabajo si no se guardo, o se borro
 */
--[SP_CPOS_ADD_TB_TRABAJO]'302','','','21102022','v','001',1,'1','2','3','4','10','10','Ambos','Cerca','Cerca','QUO','001','00001','02','CONVENCIONAL','0','0','0','0'
CREATE PROCEDURE [dbo].[SP_CPOS_ADD_TB_TRABAJO]

@SUC AS VARCHAR(3),
@NUMOS AS VARCHAR(7),
@REV AS VARCHAR(2),
@CEDULA AS VARCHAR(10),
@NACIO AS CHAR(1),
@TIPO_TRABAJO AS VARCHAR(3),
@NUM_EXAMAEN INT,
@HORIZ AS VARCHAR(5),
@VERT AS VARCHAR(5),
@MAX AS VARCHAR(5),
@PTE AS VARCHAR(5),
@ALTD AS VARCHAR(5),
@ALTI AS VARCHAR(5),
@OJO AS VARCHAR(15),
@TVISD AS VARCHAR(15),
@TVISI AS VARCHAR(15),
@LAB AS VARCHAR(11),
@SERV AS VARCHAR(3),
@TIPO_RX AS VARCHAR(1),
@USER AS VARCHAR(5),
@CODDETVTA AS VARCHAR (2),
@TIPO_EXAMEN AS VARCHAR(12),
@DISVERT AS VARCHAR(5),
@ANPANT AS VARCHAR(5),
@ANFAC AS VARCHAR(5),
@DDL AS VARCHAR(5)



AS


SET NOCOUNT ON


DECLARE @ControlErrores AS INT
DECLARE @Error AS INT
SET @ControlErrores = 0


-- COMIENZA LA TRANSACCION
BEGIN TRAN

--	SELECT TB_CAORDSER.NumOrdServ 
--	FROM TB_CAORDSER
--	INNER JOIN TB_TRABAJO
--	ON TB_CAORDSER.NumOrdServ = TB_TRABAJO.T_NumOrdServ
--	WHERE TB_CAORDSER.NumOrdServ = @NUMOS and TB_CAORDSER.Revision = '0'


--IF @@ROWCOUNT = 0 
--BEGIN

	INSERT INTO TB_TRABAJO (T_SUCURSAL,T_NumOrdserv,T_Revision, T_CEDIDEN,T_NACIO,T_TIPOTRABAJO,T_EXAMEN
							,T_HORIZONTAL,T_VERTICAL,T_MAXIMA,T_PUENTE,T_ALTD,T_ALTI,T_OJO,T_TIPOVISIOND,T_TIPOVISIONI										
							,T_LABORATORIO,T_SERVICIO,T_TIPORX,T_FECCREA,USER_CREA,Cod_DetVta,TipoExamen
							,T_DISTANCIAVERTICE,T_ANGULOPANTOSCOPICO,T_ANGULOFACIAL,T_DISTANCIADELECTURA)
	VALUES (@SUC,@NUMOS,'0',@CEDULA ,@NACIO,@TIPO_TRABAJO,@NUM_EXAMAEN
			,@HORIZ,@VERT,@MAX,@PTE,@ALTD,@ALTI,@OJO,@TVISD,@TVISI
			,@LAB,@SERV,@TIPO_RX, getdate(),@USER,@CODDETVTA,@TIPO_EXAMEN
			,@DISVERT,@ANPANT,@ANFAC,@DDL)

--	UPDATE TB_TRABAJO
--	set 	T_Revision = ORD.Revision,
--		T_CEDIDEN = ORD.CTE_CedIden,
--		T_NACIO= ORD.CTE_Nacio,
--		T_TIPOTRABAJO= ORD.Cod_Venta,
--		T_EXAMEN= ORD.NumExamen,
--		T_LABORATORIO= ORD.Cod_Laboratorio,
--		T_SERVICIO = ORD.Cod_Servicio,
--		T_HORAOFRECIDO= ORD.Hor_ofrecido,
--		T_FECHAOFRECIDO= convert (varchar,ORD.Fec_ofrecido,103),
--		T_FECMOD= getdate(),
--		USER_MOD=  @USER,
--		Cod_DetVta= ORD.Cod_DetVta,
--		TipoExamen= ISNULL(VTADET.TipoExamen,'')
--	FROM 	TB_TRABAJO 
--	INNER 	JOIN TB_CAORDSER ORD
--	ON TB_TRABAJO.T_NumOrdServ = ORD.NumOrdServ
--	INNER 	JOIN TB_VENTADETALLE VTADET
--	ON VTADET.Cod_DetVta = ORD.Cod_DetVta
--	WHERE TB_TRABAJO.T_NumOrdServ = @NUMOS

--END 

--ELSE
--BEGIN
--	UPDATE TB_TRABAJO
--	set 	T_HORIZONTAL = @HORIZ,
--		T_VERTICAL = @VERT,
--		T_MAXIMA= @MAX,
--		T_PUENTE= @PTE
--	WHERE TB_TRABAJO.T_NumOrdServ = @NUMOS 
--	AND (T_TIPOTRABAJO = '002') AND (COD_DETVTA = '01' or COD_DETVTA = '08' or COD_DETVTA = '09') 

--END

/*SELECT TB_CAORDSER.NumOrdServ 
FROM TB_CAORDSER
INNER JOIN TB_TRABAJO
ON TB_CAORDSER.NumOrdServ = TB_TRABAJO.T_NumOrdServ
WHERE TB_CAORDSER.NumOrdServ = @NUMOS and TB_CAORDSER.Revision = '0'*/

--rollback tran

	IF @@ERROR <> 0
	SET @ControlErrores = @ControlErrores + 1


	IF @ControlErrores <> 0
		BEGIN
			ROLLBACK TRAN
			SELECT 'FALLIDO' AS MODIFICACIONTRABAJO
--				PRINT 'SE GENERO UN ERROR '  + str(@ControlErrores)
		END
		ELSE
		BEGIN
			COMMIT TRAN
			SELECT 'SATISFACTORIO' AS MODIFICACIONTRABAJO
		END

GO

-----------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[SP_CPOS_ArticuloMaximo]    Script Date: 05/10/2025 08:52:57 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CPOS_ArticuloMaximo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_CPOS_ArticuloMaximo]
GO

/****** Object:  StoredProcedure [dbo].[SP_CPOS_ArticuloMaximo]    Script Date: 05/10/2025 08:52:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





--exec SP_CPOS_BucarNota '0000127','true'
CREATE  PROCEDURE [dbo].[SP_CPOS_ArticuloMaximo] 
@Código varchar(1),
@TipoVenta varchar(10)= ''


AS

select Max_Vta as Max_Vta
from  TB_TIPO_ARTICULO
where  CodTipo= @Código






GO

-----------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[SP_CPOS_BucarTrabajo]    Script Date: 05/10/2025 09:10:42 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CPOS_BucarTrabajo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_CPOS_BucarTrabajo]
GO


/****** Object:  StoredProcedure [dbo].[SP_CPOS_BucarTrabajo]    Script Date: 05/10/2025 09:10:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE  PROCEDURE [dbo].[SP_CPOS_BucarTrabajo] 
@T_SUCURSAL varchar(3),
@T_NACIO varchar(1),
@T_CEDIDEN varchar(10),
@Numero_Orden varchar(7) = NULL

AS


SELECT  *
FROM         TB_TRABAJO

where T_NACIO= @T_NACIO and T_CEDIDEN= @T_CEDIDEN  and T_SUCURSAL= @T_SUCURSAL
AND (@Numero_Orden IS NULL OR T_NumOrdserv = @Numero_Orden)
order by T_FECCREA desc






GO

-----------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[SP_CPOS_BuscarIva]    Script Date: 05/10/2025 09:12:25 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CPOS_BuscarIva]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_CPOS_BuscarIva]
GO

/****** Object:  StoredProcedure [dbo].[SP_CPOS_BuscarIva]    Script Date: 05/10/2025 09:12:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




--exec SP_CPOS_BucarNota '0000127','true'
CREATE PROCEDURE [dbo].[SP_CPOS_BuscarIva] 
@Codigo varchar(1)


AS

SELECT   COD_Tipo, Indice, Descripcion, Porcentaje, Fec_Crea, Fec_Mod, USER_Crea, USER_Mod
FROM     TB_IMPDES
where    COD_Tipo = @Codigo





GO

-----------------------------------------------------------------------------------------------------
/****** Object:  StoredProcedure [dbo].[SP_CPOS_GET_ARTICULO]    Script Date: 05/12/2025 19:41:55 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CPOS_GET_ARTICULO]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_CPOS_GET_ARTICULO]
GO


/****** Object:  StoredProcedure [dbo].[SP_CPOS_GET_ARTICULO]    Script Date: 05/12/2025 19:41:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO







-- =============================================
-- Author:		<FB>
-- Create date: <15/05/2023>
-- Description:	<strored con el que me traigo los detalles del articulo>
-- =============================================
-- exec SP_CPOS_GET_ARTICULO 'C000001','01'
CREATE PROCEDURE [dbo].[SP_CPOS_GET_ARTICULO] 
@CodArticulo NVARCHAR(7) = NULL, -- Parámetro opcional con valor predeterminado NULL
@TipoTrabajo VARCHAR(3)= NULL
AS
BEGIN
    SET NOCOUNT ON;

   
if (@TipoTrabajo = null or @TipoTrabajo = '')
begin
    -- Consulta que filtra por CodArticulo solo si se proporciona un valor
    SELECT * 
    FROM TB_ARTICULO
    WHERE ((@CodArticulo IS NULL OR CodArticulo = @CodArticulo) and ART_ACTIVO= 1)  
    union 
    SELECT * 
    FROM TB_ARTICULO
    WHERE ((@CodArticulo IS NULL OR CodArticulo = @CodArticulo) and tipart = 'A')  
    
end 

--Trabajo Convencional
if (@TipoTrabajo in ('01','09'))
Begin 

SELECT a.*
FROM TB_ARTICULO a
left join  TB_SERVICIOSAR s on 
a.Codarticulo = s.CodServicio  
WHERE ((ART_ACTIVO = 1) 
AND (MANEJAEXISTENCIA = 0) 
AND (TIPART <> 'W') 
AND (TIPART <> 'A') 
AND (MARCA <> 'PROM') 
OR (ART_ACTIVO = 1) 
AND (MANEJAEXISTENCIA = 1) 
AND (ART_EXIST > 0) 
AND (TIPART <> 'W')
AND (TIPART <> 'A') 
AND (CODPRO IS NULL)
AND (MARCA <> 'PROM') 
OR (ART_ACTIVO = 1) 
AND (CODPRO = '065') 
AND (MANEJAEXISTENCIA = 1)
AND (TIPART <> 'W') 
AND (TIPART <> 'A') 
AND (a.CodArticulo NOT LIKE 'A%') 
AND (a.CodArticulo NOT LIKE 'C%') 
--AND (a.CodArticulo NOT LIKE 'S%') 
AND (a.CodArticulo NOT LIKE 'M%')  
AND (MARCA <> 'PROM') ) AND (s.CodServicio is null)
AND a.CodArticulo  <> 'S000138' 
UNION
SELECT a.*
FROM TB_ARTICULO a  
inner join  TB_SERVICIOSAR s on 
a.Codarticulo = s.CodServicio WHERE (s.codarticulo = @CodArticulo) and a.art_activo = 1  
ORDER BY  a.CodArticulo, a.DESART 
    
END
--Trabajo Contacto
if (@TipoTrabajo = '02')
Begin 

SELECT 
   *
FROM 
    TB_ARTICULO a
LEFT JOIN 
    TB_ARTICULOSERVICIO s 
ON 
    a.CodArticulo = s.CodEposArticulo
WHERE 
    (
        (a.TIPART = 'W') 
        OR (a.TIPART = 'S') 
        OR (a.TIPART = 'E') 
        OR ((a.TIPART = 'Q') AND a.ART_EXIST > 0)
    )
    AND (s.CodEposArticulo IS NULL) -- Artículos sin servicio asociado
    AND a.ART_ACTIVO = 1

UNION

-- Segunda Parte: Artículos con Servicio Específico Asociado
SELECT 
    *
FROM 
    TB_ARTICULO a
INNER JOIN 
    TB_ARTICULOSERVICIO s 
ON 
    a.CodArticulo = s.CodEposArticulo
WHERE 
    a.ART_ACTIVO = 1 
    AND s.CodServicio = '017'
ORDER BY 
    a.CodArticulo, a.DESART ;
    
END
--Reparacion
if (@TipoTrabajo = '05')
Begin
SELECT 
 *
FROM 
    TB_ARTICULO
WHERE 
    (
        (ART_ACTIVO = 1) 
        AND (MANEJAEXISTENCIA = 0) 
        AND (TIPART = 'S') 
    )
    OR 
    (
        (ART_ACTIVO = 1) 
        AND (MANEJAEXISTENCIA = 1) 
        AND (ART_EXIST > 0) 
        AND (TIPART = 'S') 
    )
ORDER BY 
    CodArticulo, DESART ;

END

--Venta Directa
if (@TipoTrabajo = '04')
Begin
SELECT 
    *
FROM 
    TB_ARTICULO a
INNER JOIN 
    TB_TIPO_ARTICULO t 
ON 
    a.TIPART = t.CodTipo
WHERE 
    (a.ART_EXIST > 0) -- Solo artículos con existencia mayor a 0
    AND (a.ART_ACTIVO = 1) -- Solo artículos activos
    AND (a.MANEJAEXISTENCIA = 1) -- Solo artículos que manejan existencia
    AND CHARINDEX('04', t.TipoVta) > 0 -- El tipo de venta contiene '04'
    AND (a.MARCA <> 'PROM') -- Excluir artículos con marca 'PROM'
    AND (a.CODPRO IS NULL) -- Solo artículos sin código de promoción
ORDER BY 
   CodArticulo, DESART ;
end



if (@TipoTrabajo = '08')
Begin 

SELECT 
   *
FROM TB_ARTICULO a
left join  TB_SERVICIOSAR s on 
a.Codarticulo = s.CodServicio  
WHERE ((ART_ACTIVO = 1) 
AND (MANEJAEXISTENCIA = 0) 
AND (TIPART <> 'W') 
AND (TIPART <> 'A') 
AND (MARCA <> 'PROM') 
OR (ART_ACTIVO = 1) 
AND (MANEJAEXISTENCIA = 1) 
AND (ART_EXIST > 0) 
AND (TIPART <> 'W')
AND (TIPART <> 'A') 
AND (CODPRO IS NULL)
AND (MARCA <> 'PROM') 
OR (ART_ACTIVO = 1) 
AND (CODPRO = '065') 
AND (MANEJAEXISTENCIA = 1)
AND (TIPART <> 'W') 
AND (TIPART <> 'A') 
AND (a.CodArticulo NOT LIKE 'A%') 
AND (a.CodArticulo NOT LIKE 'C%') 
AND (a.CodArticulo NOT LIKE 'S%') 
AND (a.CodArticulo NOT LIKE 'M%')  
AND (MARCA <> 'PROM') ) AND (s.CodServicio is null)
AND a.CodArticulo  <> 'S000138' 
UNION
SELECT 
    *
FROM TB_ARTICULO a  
inner join  TB_SERVICIOSAR s on 
a.Codarticulo = s.CodServicio WHERE (s.codarticulo = @CodArticulo) and a.art_activo = 1  
ORDER BY 
    a.CodArticulo, a.DESART ;

end

end



GO



-----------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[SP_CPOS_GET_CLIENTEAFILIADO]    Script Date: 05/10/2025 09:16:54 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CPOS_GET_CLIENTEAFILIADO]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_CPOS_GET_CLIENTEAFILIADO]
GO

/****** Object:  StoredProcedure [dbo].[SP_CPOS_GET_CLIENTEAFILIADO]    Script Date: 05/10/2025 09:16:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





-- =============================================
-- Author:		<FB>
-- Create date: <15/05/2023>
-- Description:	<strored con el que me traigo los detalles del articulo>
-- =============================================
-- exec [SP_CPOS_GET_CLIENTEAFILIADO]
CREATE PROCEDURE [dbo].[SP_CPOS_GET_CLIENTEAFILIADO] 
--@CodArticulo NVARCHAR(7) = NULL, -- Parámetro opcional con valor predeterminado NULL
--@TipoTrabajo VARCHAR(3)= NULL
AS
BEGIN
    SET NOCOUNT ON;
    
	SELECT Codigo_Emp,Nombre,PorcentajeDes1,PorcentajeDes2,PorcentajeDes3 FROM TB_EMPAFI where Activo = 1
END


GO

-----------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[SP_CPOS_GET_COLORLC]    Script Date: 05/10/2025 09:18:13 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CPOS_GET_COLORLC]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_CPOS_GET_COLORLC]
GO

/****** Object:  StoredProcedure [dbo].[SP_CPOS_GET_COLORLC]    Script Date: 05/10/2025 09:18:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




 --e
 --exec [SP_CPOS_GET_COLORLC] 'W000029'
CREATE PROCEDURE [dbo].[SP_CPOS_GET_COLORLC]
 @CodArticulo as varchar(7)
AS

SELECT L.CodColor , DESCRIPCIONCOLOR FROM TB_LENTESCONTACTO L INNER JOIN TB_COLORESLC C ON L.CODCOLOR = C.CodColor WHERE CodEposArticulo =@CodArticulo GROUP BY L.CodColor, DESCRIPCIONCOLOR




GO

-----------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[SP_CPOS_MOTIVOSDESCUENTO]    Script Date: 05/10/2025 09:23:32 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CPOS_MOTIVOSDESCUENTO]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_CPOS_MOTIVOSDESCUENTO]
GO

/****** Object:  StoredProcedure [dbo].[SP_CPOS_MOTIVOSDESCUENTO]    Script Date: 05/10/2025 09:23:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



--SP_CPOS_MOTIVOSDESCUENTO
CREATE PROCEDURE [dbo].[SP_CPOS_MOTIVOSDESCUENTO]
@cbCodMotivo as varchar (3)= ''

AS
SET NOCOUNT ON

if (@cbCodMotivo= null or @cbCodMotivo = '')
begin
SELECT CodMotivo as CodMotivo, Descripcion as Descripcion from TB_MOTIVOSDESCUENTO
WHERE CodMotivo IS NOT NULL AND CodMotivo <> ''
  AND Descripcion IS NOT NULL AND Descripcion <> ''
order by CodMotivo desc
end 
else
begin
SELECT CodEmpleadoDesc as CodMotivo, Descripcion as Descripcion from TB_MOTIVOSDESCUENTO
WHERE CodMotivo IS NOT NULL AND CodMotivo <> ''
  AND Descripcion IS NOT NULL AND Descripcion <> ''
  and CodMotivo= @cbCodMotivo
order by CodMotivo desc
end 


GO

-----------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[SP_CPOS_ObtenerExamen]    Script Date: 05/10/2025 09:26:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CPOS_ObtenerExamen]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_CPOS_ObtenerExamen]
GO


/****** Object:  StoredProcedure [dbo].[SP_CPOS_ObtenerExamen]    Script Date: 05/10/2025 09:26:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



--exec SP_CPOS_ObtenerExamen 'V','21102022','302',1
CREATE PROCEDURE [dbo].[SP_CPOS_ObtenerExamen] 
@Nacionalidad as varchar (1),
@CedulaCliente  as varchar (10),
@SucursalActual as varchar (3),
@NumeroExamen as int

AS

SELECT      CTE_Nacio, CTE_CedIden, COD_Sucursal, NUM_Examen, FEC_Examen, ESFD, ESFI, CILD, CILI, EJED, EJEI, ADDD, ADDI, OBSERVACIONES, TIPO_Optm, NOM_Optm, EXA_Feccreacion, 
                      EXA_Fecmod, USER_CREA, USER_MOD, TIPOEXAMEN, NOMBRE_CLINICA_OPTM, TLF_TIPO, TLF_COD, TLF_NUMERO, TLF_EXT, ESFD2, ESFI2, CILD2, CILI2, EJED2, EJEI2, CodigoMimesys
from           TB_Examen
where          COD_SUCURSAL = @SucursalActual AND CTE_NACIO = @Nacionalidad AND CTE_CEDIDEN = @CedulaCliente AND NUM_EXAMEN = @NumeroExamen




GO

-----------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[SP_CPOS_ObtenerPromoVigente]    Script Date: 05/10/2025 09:26:48 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CPOS_ObtenerPromoVigente]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_CPOS_ObtenerPromoVigente]
GO

/****** Object:  StoredProcedure [dbo].[SP_CPOS_ObtenerPromoVigente]    Script Date: 05/10/2025 09:26:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



--exec SP_CPOS_ObtenerPromoVigente
CREATE PROCEDURE [dbo].[SP_CPOS_ObtenerPromoVigente] 

AS

SELECT    COD_Prom, Prom_DESCRIP, Prom_FECINI, Prom_FECFIN, Prom_Cond1, Prom_Cond2, Prom_AccionPrecio, Prom_AccionDescuento, Prom_AccionNuevosPro, Fec_Crea, AceptaFinanciamiento
FROM         TB_PROMOCIONES
WHERE Prom_FECFIN >=  CAST(GETDATE() AS DATE)
ORDER BY COD_Prom DESC





GO

-----------------------------------------------------------------------------------------------------
/****** Object:  StoredProcedure [dbo].[SP_CPOS_ObtenerRx]    Script Date: 05/10/2025 09:27:37 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CPOS_ObtenerRx]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_CPOS_ObtenerRx]
GO


/****** Object:  StoredProcedure [dbo].[SP_CPOS_ObtenerRx]    Script Date: 05/10/2025 09:27:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




--exec SP_CPOS_BucarNota '0000127','true'
CREATE PROCEDURE [dbo].[SP_CPOS_ObtenerRx] 
@Nacionalidad as varchar (1),
@CedulaCliente  as varchar (10),
@SucursalActual as varchar (3),
@NumeroExamen as int

AS

SELECT      CTE_Nacio, CTE_CedIden, COD_Sucursal, NUM_Examen, DPDL, DPDC, DPIL, DPIC, ALTD, ALTI, PRISMAD, PRISMAI, PBASED, PBASEI, OFTI, OFTD, AVD, AVI, RETI, RETD, PRISMAD2, 
                      PRISMAI2, PBASED2, PBASEI2, PROGVISIONLEJOSDISTD, PROGVISIONLEJOSDISTI, PROGVISIONCERCADISTD, PROGVISIONCERCADISTI, PROGVISIONMEDIADISTD, 
                      PROGVISIONMEDIADISTI
from        TB_FICCONV
where       CTE_NACIO= @Nacionalidad AND CTE_CEDIDEN = @CedulaCliente AND COD_SUCURSAL = @SucursalActual  AND NUM_EXAMEN = @NumeroExamen 






GO

-----------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[SP_CPOS_ObtenerServiciosAgregados]    Script Date: 05/10/2025 09:28:58 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CPOS_ObtenerServiciosAgregados]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_CPOS_ObtenerServiciosAgregados]
GO


/****** Object:  StoredProcedure [dbo].[SP_CPOS_ObtenerServiciosAgregados]    Script Date: 05/10/2025 09:28:58 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO







--exec SP_CPOS_BucarNota '0000127','true'
CREATE PROCEDURE [dbo].[SP_CPOS_ObtenerServiciosAgregados] 

AS

SELECT    COD_Agregado, Agregado_DESCRIP, Agregado_Cond, Agregado_Producto, Fec_Crea, Fec_Mod, USER_Crea, USER_Mod
FROM         TB_AGREGADO2
where COD_Agregado <>'XX' 
order by COD_Agregado




GO

-----------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[SP_CPOS_TipoVenta]    Script Date: 05/10/2025 09:31:45 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CPOS_TipoVenta]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_CPOS_TipoVenta]
GO


/****** Object:  StoredProcedure [dbo].[SP_CPOS_TipoVenta]    Script Date: 05/10/2025 09:31:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- exec SP_CPOS_TipoVenta ''
CREATE PROCEDURE [dbo].[SP_CPOS_TipoVenta] 
@Cod_DetVta as varchar (2) = '' 

AS

IF @Cod_DetVta IS NULL OR @Cod_DetVta = ''
begin

SELECT  VD.Cod_DetVta CodModo, VD.Cod_Venta CodVenta, VD.Descripcion Descripcion

from dbo.TB_TIPOVENTA TV 

INNER JOIN dbo.TB_VENTADETALLE VD ON VD.COD_Venta = TV.COD_Venta

where  VD.Activo = 1 

end 


else 
begin 


SELECT  VD.Cod_DetVta CodModo, VD.Cod_Venta CodVenta, VD.Descripcion Descripcion

from dbo.TB_TIPOVENTA TV 

INNER JOIN dbo.TB_VENTADETALLE VD ON VD.COD_Venta = TV.COD_Venta

where  VD.Activo = 1 and VD.Cod_DetVta= @Cod_DetVta

end

GO

-------------------------------------LOS TOME DE SO ORIGINAL------------------------------------------
/****** Object:  StoredProcedure [dbo].[CPOS_pValidoTraza]    Script Date: 05/06/2025 17:09:50 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CPOS_pValidoTraza]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CPOS_pValidoTraza]
GO



/****** Object:  StoredProcedure [dbo].[CPOS_pValidoTraza]    Script Date: 05/06/2025 17:09:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[CPOS_pValidoTraza]
--Usado en: VerificoTrazaMLS
@articulo as Varchar(7)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT CASE WHEN permiteTraza =0 THEN 1 ELSE CAST(tieneTraza AS varchar(1)) END AS tieneTraza 
	FROM TB_ARTICULO with (nolock) 
	WHERE CodArticulo=@articulo 
END
---------

GO


----------------------------------------------------------------------------------------------------



/****** Object:  StoredProcedure [dbo].[CPOS_pGetServiciosAR]    Script Date: 05/06/2025 17:10:21 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CPOS_pGetServiciosAR]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CPOS_pGetServiciosAR]
GO



/****** Object:  StoredProcedure [dbo].[CPOS_pGetServiciosAR]    Script Date: 05/06/2025 17:10:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[CPOS_pGetServiciosAR] 
--Usado en: VerificoIgualAntirefCrist, VerificoCantidadProducto
( @CodCrt VARCHAR (7),
  @CodServ VARCHAR (7) )
AS
BEGIN
	
    DECLARE @AROBLIG as varchar(7)
    DECLARE @AROBLIGACT as bit
    DECLARE @NEWAROBLIG as varchar(7)
    
    SET NOCOUNT ON

    IF @CodCrt = ''
    BEGIN 
    
		SELECT distinct(sa.CodServicio) , sa.obligatorio 
		FROM TB_SERVICIOSAR	sa INNER JOIN TB_ARTICULO ar on ( sa.CodServicio = ar.CodArticulo)
		where 
			sa.CodServicio = @CodServ	
	
    END
    ELSE
    BEGIN
 
		--SI HAY ALGUN OBLIG PARA ESE CRISTAL
		SELECT @AROBLIG = sa.CodServicio --, sa.obligatorio 
		FROM TB_SERVICIOSAR	sa 
		INNER JOIN TB_ARTICULO ar on ( sa.CodServicio = ar.CodArticulo)
		WHERE sa.CodArticulo = @CodCrt  and Obligatorio = 1
		
		
		--SI HAY ALGUN OBLIG PARA ESE CRISTAL
		IF @AROBLIG <> ''
		BEGIN
		   	--BUSCA SI ESTA ACTIVO
			SELECT @AROBLIGACT = art_activo FROM  TB_ARTICULO where CodArticulo = @AROBLIG 
			
			--CUANDO sea S000134 o S000002 y esten activos, quedan ellos mismos, de resto se asigna el S000134
			--SELECT @NEWAROBLIG =  case when @AROBLIG = 'S000134' and @AROBLIGACT = 1 then 'S000134' when @AROBLIG = 'S000002' and @AROBLIGACT = 1 then 'S000002'  else 'S000132' end--, 1 as obligatorio
			SET @NEWAROBLIG = @AROBLIG 
			
			SELECT @NEWAROBLIG CodServicio,1 obligatorio
			union
			SELECT sa.CodServicio CodServicio , sa.obligatorio 
			FROM TB_SERVICIOSAR	sa INNER JOIN TB_ARTICULO ar on ( sa.CodServicio = ar.CodArticulo)
			WHERE sa.CodArticulo = @CodCrt and
			ar.ART_ACTIVO = 1 	and sa.CodServicio <> @AROBLIG and (sa.CodServicio <> @NEWAROBLIG)
		END
		ELSE
		BEGIN 
			SELECT sa.CodServicio , sa.obligatorio 
			FROM TB_SERVICIOSAR	sa INNER JOIN TB_ARTICULO ar on ( sa.CodServicio = ar.CodArticulo)
			WHERE sa.CodArticulo = @CodCrt and
				 ar.ART_ACTIVO = 1 	
		END
			
    END
    
    SELECT distinct(CodServicio)  FROM TB_SERVICIOSAR	
    

END
-------

GO


---------------------------------------------------------------------------------


/****** Object:  StoredProcedure [dbo].[CPOS_pServicioColoracion]    Script Date: 05/06/2025 17:10:45 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CPOS_pServicioColoracion]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CPOS_pServicioColoracion]
GO


/****** Object:  StoredProcedure [dbo].[CPOS_pServicioColoracion]    Script Date: 05/06/2025 17:10:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[CPOS_pServicioColoracion]   
-- Usado en: Servicio_Coloracion
	 @CRISTAL AS VARCHAR(7) 
	, @TIPOCOLOR AS BIT
AS
BEGIN
 
	DECLARE @MATERIAL AS VARCHAR(10) 
	DECLARE @TIPOCOLOR_DESC AS VARCHAR(30) 
	DECLARE @COLORSI AS VARCHAR(2) 
	DECLARE @COLORNO AS VARCHAR(2)

		
	SELECT @COLORSI = colorSi, @COLORno = colorNo FROM TB_Cristales WHERE Cod_Cristal_Optica = @CRISTAL
		 

	IF (@COLORSI = 'X' AND @COLORNO = 'NC')
	BEGIN
			IF @TIPOCOLOR = 1 
				SET @TIPOCOLOR_DESC = 'Full Color'
			ELSE
				SET @TIPOCOLOR_DESC = 'Degradado'
				  
				SELECT @MATERIAL = Material 
				FROM TB_Cristales_Optica
				WHERE Cod_Cristal_Optica = @CRISTAL
	END	
	
	-- IF (@COLORSI = 'NC' AND @COLORNO = 'X')  -- NO LLEVA
	-- IF (@COLORSI = 'NC' AND @COLORNO = 'NC') -- NO LLEVA
	 
			
		SELECT [Cod_Coloracion]
			,[Desc_Material]
			,[Desc_Color]
			,[Porc_Material]
			,[Tipo_Color]
		FROM [dbo].[TB_COLORACION]
		WHERE Desc_Material = @MATERIAL
			 AND Tipo_Color = @TIPOCOLOR_DESC
	
END


GO


----------------------------------------------------------------------------------------------------------------------------



/****** Object:  StoredProcedure [dbo].[Promo_Black_Friday_2024]    Script Date: 05/12/2025 15:48:49 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CPOS_Promo_Black_Friday_2024]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CPOS_Promo_Black_Friday_2024]
GO



/****** Object:  StoredProcedure [dbo].[CPOS_Promo_Black_Friday_2024]    Script Date: 05/12/2025 15:48:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





---exec CPOS_Promo_Black_Friday_2024 'M175970', 'C000001','',0,0,'',0,'002','CONVENCIONAL'
CREATE PROCEDURE [dbo].[CPOS_Promo_Black_Friday_2024]
	  @MONTURA AS VARCHAR(7) 
	, @CRISTAL AS VARCHAR(7)  
	, @LC AS VARCHAR(7)  
	, @MONTURAPROPIA AS BIT
	, @CRISTALPROPIO AS BIT
	, @SERVICIO AS VARCHAR(7)
	, @SERVAR AS BIT
	, @TIPOVENTA AS VARCHAR(3) -- "001 Venta Directa" o "002 CONVENCIONAL"
	, @TipoExamen AS VARCHAR(13)
	
AS
-- MONTURA    
DECLARE @PRECIOMONT AS NUMERIC (18,2) 
DECLARE @LETRAMLS AS VARCHAR (3) 
DECLARE @PRECIOMONT_20 AS NUMERIC(28,2)
DECLARE @TIPART AS VARCHAR (1) 
-- CRISTAL
DECLARE @PRECIOCRT AS NUMERIC(28,2)
DECLARE @PRECIOCRT_20 AS NUMERIC(28,2)
--SERVICIO
DECLARE @PRECIOSERV AS NUMERIC(28,2)
DECLARE @PRECIOSERV_20 AS NUMERIC(28,2)

Declare @PORCDCTO AS INT 
DECLARE @PARTE AS INT
DECLARE @RESULTADO AS VARCHAR(15)
SET @RESULTADO = 'NO APLICA'

	-- LETRA DE LA MONTURA 
	SELECT @LETRAMLS=CodRango , @PRECIOMONT = ART_PVP ,@TIPART=TIPART
	FROM TB_ARTICULO 
	WHERE CodArticulo = @MONTURA
	

	-- PRECIO DE ANTIREFLEJO CLEAR Y DESCUENTO DEL 50 %
	SELECT @PRECIOSERV = Art_PVP
	FROM TB_ARTICULO 
	WHERE CodArticulo = @SERVICIO
						
	-- PRECIO DEL CRISTAL
	SELECT @PRECIOCRT = art_pvp
	FROM TB_ARTICULO 
	WHERE CodArticulo = @CRISTAL
	
Declare @Cristales_2 as varchar (7)
set @Cristales_2= ''
Declare @Pertenece_Marca AS bit

	SELECT @Pertenece_Marca = marca, @Cristales_2=Codigo
	FROM  Cristales_Rodenstock_RLX_Oculus2 
	WHERE Codigo = @CRISTAL
	
	
DECLARE @DESCMONT AS INTEGER -- MONTURA  	
	SELECT	@DESCMONT = PorctDescuento
		FROM TB_ARTICULO 
		WHERE CodArticulo = @MONTURA
		
DECLARE @MARCA_LENTE AS VARCHAR (4)
SET @MARCA_LENTE= (SELECT REPLACE (MARCA, ' ', '')  FROM TB_ARTICULO WHERE CodArticulo = @MONTURA)	
	
	
	IF (@TipoExamen = 'CONVENCIONAL')
BEGIN
	IF (@DESCMONT = 0) -- DESC MONTURA
	BEGIN 	
		IF (@MONTURAPROPIA = 0 AND @CRISTALPROPIO = 0) 
		BEGIN -- MONTURA Y CRISTAL PROPIO	
			IF (@TIPOVENTA = '002')-- VENTA CONVENCIONAL 
			BEGIN 	
					
		--------1-------------	
		
		----Con la compra de cualquier montura de la lista de precios de colores desde los rangos 
		--A1 hasta N: A1, G, H, I, J, K, L, M, N, ms cualquier cristal (con o sin servicio AR), se le otorga 40% de descuento en la compra total.
					
		if ((@LETRAMLS = 'A1') or (@LETRAMLS = 'G') OR (@LETRAMLS = 'H') OR (@LETRAMLS = 'I') OR (@LETRAMLS = 'J') OR (@LETRAMLS = 'K') OR
					(@LETRAMLS = 'L') OR (@LETRAMLS = 'M') OR (@LETRAMLS = 'N')  )							   
		BEGIN 
		
		
				
			 SET @PARTE = 1
			 SET @RESULTADO = 'APLICA'
	         SET @PRECIOMONT_20		= @PRECIOMONT -((@PRECIOMONT * 25)/100)
	         SET @PRECIOSERV_20		= @PRECIOSERV -((@PRECIOSERV * 25)/100)
	         SET @PRECIOCRT_20       = @PRECIOCRT -((@PRECIOCRT * 25)/100)
	         set @PORCDCTO= 40		
		END
				
			
	----Con la compra de cualquier montura de la lista de precios de colores desde los rangos O hasta el nmero 12: 
	--O, P, Q, R, S, T, U, V, W, X, Y, Z, 1, 2, 3, 4, 5, 6,7, 8, 9, 10, 11, 12, 
	--ms cualquier cristal (con o sin servicio AR) excepto la marca Cartier (CodRango CR), se le otorga 25% de descuento en la compra total.
	
 	  	if (((@LETRAMLS = 'O') OR (@LETRAMLS = 'P') OR 
					(@LETRAMLS = 'Q') OR (@LETRAMLS = 'R') OR (@LETRAMLS = 'S') OR (@LETRAMLS = 'T') OR (@LETRAMLS = 'U') OR 
					(@LETRAMLS = 'V') OR (@LETRAMLS = 'W') OR (@LETRAMLS = 'X') OR (@LETRAMLS = 'Y') OR (@LETRAMLS = 'Z') OR
					(@LETRAMLS = '1') OR (@LETRAMLS = '2') OR (@LETRAMLS = '3') OR (@LETRAMLS = '4') OR (@LETRAMLS = '5') OR (@LETRAMLS = '6')or
					(@LETRAMLS = '7') OR (@LETRAMLS = '8') OR (@LETRAMLS = '9') OR (@LETRAMLS = '10') OR (@LETRAMLS = '11') OR (@LETRAMLS = '12'))AND @MARCA_LENTE <> 'CR') 							   
		BEGIN 
				
		     SET @PARTE = 2
		     SET @RESULTADO = 'APLICA'
	         SET @PRECIOMONT_20		= @PRECIOMONT -((@PRECIOMONT * 10)/100)
	         SET @PRECIOSERV_20		= @PRECIOSERV -((@PRECIOSERV * 10)/100)
	         SET @PRECIOCRT_20       = @PRECIOCRT -((@PRECIOCRT * 10)/100)
	         set @PORCDCTO= 25	
		END
		
			
--Con la compra de cualquier montura de la lista de precios de Outlet desde AR hasta el nmero 
--NR: AR, BR, CR, DR, FR, GR, HR, IR, JR, KR, LR, MR, NR, ZR 
--ms cualquier cristal (con o sin servicio AR), se le otorga 50% de descuento en la compra total.
	
	 IF ((@LETRAMLS = 'AR') OR (@LETRAMLS = 'BR') OR (@LETRAMLS = 'CR')  OR (@LETRAMLS = 'ZR'))						BEGIN    					   
	        SET @PARTE = 3
	         print '3'
	         SET @RESULTADO = 'APLICA'
	         SET @PRECIOMONT_20		= @PRECIOMONT -((@PRECIOMONT * 50)/100)
	         SET @PRECIOSERV_20		= @PRECIOSERV -((@PRECIOSERV * 50)/100)
	         SET @PRECIOCRT_20       = @PRECIOCRT -((@PRECIOCRT * 50)/100)
	         set @PORCDCTO= 50		
		END
		
	 IF ((@LETRAMLS = 'DR')  OR (@LETRAMLS = 'ER')  OR (@LETRAMLS = 'FR') OR (@LETRAMLS = 'HR') OR (@LETRAMLS = 'IR') OR (@LETRAMLS = 'JR') OR (@LETRAMLS = 'KR') OR  (@LETRAMLS = 'GR') OR (@LETRAMLS = 'LR') OR (@LETRAMLS = 'MR') OR (@LETRAMLS = 'NR'))
				 BEGIN	
				  SET @PARTE = 3
					 print '3'
					 SET @RESULTADO = 'APLICA'
					 SET @PRECIOMONT_20		= @PRECIOMONT -((@PRECIOMONT * 30)/100)
					 SET @PRECIOSERV_20		= @PRECIOSERV -((@PRECIOSERV * 30)/100)
					 SET @PRECIOCRT_20       = @PRECIOCRT -((@PRECIOCRT * 30)/100)
					 set @PORCDCTO= 50		
				 END
				
		
		
		
		--Con la compra de cualquier montura de la marca Cartier con cualquier cristal de la lista de precios, se le otorga 5% de descuento a la montura
		  --if (@MARCA_LENTE = 'CR') 						   
				--  BEGIN 
				--		-- MONTURAS

			 --SET @PARTE = 3
	   --      print '3'
	   --      SET @RESULTADO = 'APLICA'
	   --      SET @PRECIOMONT_20		= @PRECIOMONT -((@PRECIOMONT * 5)/100)
	   --      SET @PRECIOSERV_20		= @PRECIOSERV 
	   --      SET @PRECIOCRT_20       = @PRECIOCRT 
	   --      set @PORCDCTO= 0		
							
		  --         END -- 2.
	
			
END-- VENTA 002 
END -- MONTURA Y CRISTAL PROPIO
END -- DESCUENTO MONTURA
END--------TipoExamen--------







  IF (@TipoExamen = 'DIRECTA')
BEGIN
	IF (@DESCMONT = 0) -- DESC MONTURA
	BEGIN  
			IF (@TIPOVENTA = '001')-- VENTA DIRECTA 
			BEGIN 	


----Con la compra de cualquier lente de sol de la lista de precios de colores desde los rangos A1 hasta N: A1, G, H, I, J, K, L, M, N, en venta directa o con cristales formulados, se le otorga 40% de descuento en la compra Total.

		if ((@LETRAMLS = 'A1') or (@LETRAMLS = 'G') OR (@LETRAMLS = 'H') OR (@LETRAMLS = 'I') OR (@LETRAMLS = 'J') OR (@LETRAMLS = 'K') OR
					(@LETRAMLS = 'L') OR (@LETRAMLS = 'M') OR (@LETRAMLS = 'N')  )							   
		BEGIN 
	 SET @PARTE = 4
	  SET @RESULTADO = 'APLICA'
	         SET @PRECIOMONT_20		= @PRECIOMONT -((@PRECIOMONT * 20)/100)
	         SET @PRECIOSERV_20		= @PRECIOSERV -((@PRECIOSERV * 20)/100)
	         SET @PRECIOCRT_20       = @PRECIOCRT -((@PRECIOCRT * 20)/100)
	         set @PORCDCTO= 40		
		END
		
	----Con la compra de cualquier montura de la lista de precios de colores desde los rangos O hasta el nmero 12: 
	--O, P, Q, R, S, T, U, V, W, X, Y, Z, 1, 2, 3, 4, 5, 6,7, 8, 9, 10, 11, 12, 
	--ms cualquier cristal (con o sin servicio AR) excepto la marca Cartier (CodRango CR), se le otorga 25% de descuento en la compra total.
	
 	  	if (((@LETRAMLS = 'O') OR (@LETRAMLS = 'P') OR 
					(@LETRAMLS = 'Q') OR (@LETRAMLS = 'R') OR (@LETRAMLS = 'S') OR (@LETRAMLS = 'T') OR (@LETRAMLS = 'U') OR 
					(@LETRAMLS = 'V') OR (@LETRAMLS = 'W') OR (@LETRAMLS = 'X') OR (@LETRAMLS = 'Y') OR (@LETRAMLS = 'Z') OR
					(@LETRAMLS = '1') OR (@LETRAMLS = '2') OR (@LETRAMLS = '3') OR (@LETRAMLS = '4') OR (@LETRAMLS = '5') OR (@LETRAMLS = '6')or
					(@LETRAMLS = '7') OR (@LETRAMLS = '8') OR (@LETRAMLS = '9') OR (@LETRAMLS = '10') OR (@LETRAMLS = '11') OR (@LETRAMLS = '12'))AND @MARCA_LENTE <> 'CR') 							   
	BEGIN 
	 SET @PARTE = 5
	  SET @RESULTADO = 'APLICA'
	         SET @PRECIOMONT_20		= @PRECIOMONT -((@PRECIOMONT * 10)/100)
	         SET @PRECIOSERV_20		= @PRECIOSERV -((@PRECIOSERV * 10)/100)
	         SET @PRECIOCRT_20       = @PRECIOCRT -((@PRECIOCRT * 10)/100)
	         set @PORCDCTO= 25		
		END


--Con la compra de cualquier Lente de Sol de la lista de precio de Outlet desde AR hasta J: 
--AR, BR, CR, ZR, DR, FR, GR, HR, IR, JR, KR, LR, MR, NR, A1, G, H, I, J en 
--venta directa o con cristales formulados, se le otorga 30% de descuento en la compra total.
		  
	 IF ((@LETRAMLS = 'AR') OR (@LETRAMLS = 'BR') OR (@LETRAMLS = 'CR')  OR (@LETRAMLS = 'DR')  OR (@LETRAMLS = 'ER')  OR 
				 (@LETRAMLS = 'FR') OR (@LETRAMLS = 'HR') OR (@LETRAMLS = 'IR') OR (@LETRAMLS = 'JR') OR (@LETRAMLS = 'KR') OR 
				 (@LETRAMLS = 'GR') OR (@LETRAMLS = 'LR') OR (@LETRAMLS = 'MR') OR (@LETRAMLS = 'NR')
				 OR (@LETRAMLS = 'ZR'))					   
	  BEGIN   
	 SET @PARTE = 5
	  SET @RESULTADO = 'APLICA'
	         SET @PRECIOMONT_20		= @PRECIOMONT -((@PRECIOMONT * 30)/100)
	         SET @PRECIOSERV_20		= @PRECIOSERV -((@PRECIOSERV * 30)/100)
	         SET @PRECIOCRT_20       = @PRECIOCRT -((@PRECIOCRT * 30)/100)
	         set @PORCDCTO= 50		
		END
		
		
				--Con la compra de cualquier montura de la marca Cartier con cualquier cristal de la lista de precios, se le otorga 5% de descuento a la montura
		  --if (@MARCA_LENTE = 'CR') 						   
				--  BEGIN 
				--		-- MONTURAS

			 --SET @PARTE = 5
	   --      print '3'
	   --      SET @RESULTADO = 'APLICA'
	   --      SET @PRECIOMONT_20		= @PRECIOMONT -((@PRECIOMONT * 5)/100)
	   --      SET @PRECIOSERV_20		= @PRECIOSERV 
	   --      SET @PRECIOCRT_20       = @PRECIOCRT 
	   --      set @PORCDCTO= 0		
							
		  --         END -- 2.
		
		
END -- VENTA 001  
END -- DESC MONTURA
END -------TipoExamen---------
	
		
	
    IF @RESULTADO = 'APLICA'
	BEGIN
		SELECT @RESULTADO RESULTADO, @PARTE PARTE,  '219' CODPROM, @LETRAMLS LETRAMLS,
		@PRECIOMONT PRECIOMONT, @PRECIOMONT_20 PRECIOMONT_DESC
		,@PRECIOCRT PRECIOCRT, @PRECIOCRT_20 PRECIOCRT_DESC , @PORCDCTO AS PORCDCTO, @Pertenece_Marca as 	Rodenstock_RLX_Oculus
	END
	ELSE
	BEGIN
		SELECT @RESULTADO RESULTADO, '219' CODPROM, @LETRAMLS LETRAMLS
	END
	
	


















GO
---------------------------------------------------------------------------------------------



/****** Object:  StoredProcedure [dbo].[pEvaluoPromociones]    Script Date: 05/12/2025 15:47:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[pEvaluoPromociones]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[pEvaluoPromociones]
GO


/****** Object:  StoredProcedure [dbo].[pEvaluoPromociones]    Script Date: 05/12/2025 15:47:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--exec [pEvaluoPromociones] '230','M144228', 'C000727','002','CONVENCIONAL'
CREATE PROCEDURE [dbo].[pEvaluoPromociones]
	@CODPROMO AS VARCHAR(3),
	@PARAMETRO01 AS VARCHAR(20) = NULL,
    @PARAMETRO02 AS VARCHAR(20)= NULL, 
    @PARAMETRO03 AS VARCHAR(20)= NULL,
    @PARAMETRO04 AS VARCHAR(20)= NULL,
    @PARAMETRO05 AS VARCHAR(20)= NULL, 
    @PARAMETRO06 AS VARCHAR(20)= NULL, 
    @PARAMETRO07 AS VARCHAR(20)= NULL,
    @PARAMETRO08 AS VARCHAR(20)= NULL,
    @PARAMETRO09 AS VARCHAR(20)= NULL,
    @PARAMETRO10 AS VARCHAR(20)= NULL,
    @PARAMETRO11 AS VARCHAR(20)= NULL,
    @PARAMETRO12 AS VARCHAR(20)= NULL,
    @PARAMETRO13 AS VARCHAR(20)= NULL,
    @PARAMETRO14 AS VARCHAR(20)= NULL,
    @PARAMETRO15 AS VARCHAR(20)= NULL,
    @PARAMETRO16 AS VARCHAR(20)= NULL,
    @PARAMETRO17 AS VARCHAR(20)= NULL,
    @PARAMETRO18 AS VARCHAR(20)= NULL,
    @PARAMETRO19 AS VARCHAR(20)= NULL,
    @PARAMETRO20 AS VARCHAR(20)= NULL
    
AS
BEGIN
   DECLARE @PARAMETRO04_BIT BIT;
   DECLARE @PARAMETRO05_BIT BIT;
   DECLARE @PARAMETRO07_BIT BIT;
      
   IF @CODPROMO = '234'
   BEGIN
	----EXEC Promo_Mayo_Junio_2024 'M175970','C000376','','0','0','S000133',0,'002','CONVENCIONAL'
	--select  @PARAMETRO01, @PARAMETRO02, @PARAMETRO03,0,0,@PARAMETRO06,0,@PARAMETRO08,@PARAMETRO09
		EXEC dbo.PromodeMayoJunio_2025 @PARAMETRO01, @PARAMETRO02, @PARAMETRO03,0,0,@PARAMETRO06,0,@PARAMETRO08,@PARAMETRO09
   END
   IF @CODPROMO = '230'
   BEGIN
	----EXEC Promo_Cristales_SHADES  'M144228'  ,  'C000727'  ,'',0,0,'','',    '002'   ,'CONVENCIONAL'
		EXEC Promo_Cristales_SHADES @PARAMETRO01, @PARAMETRO02, @PARAMETRO03,@PARAMETRO04_BIT,@PARAMETRO05_BIT,@PARAMETRO06,@PARAMETRO07_BIT,@PARAMETRO08,@PARAMETRO09
   END
   ELSE
   IF @CODPROMO = '182'
   BEGIN
   
	   SET @PARAMETRO04_BIT = CONVERT(BIT, @PARAMETRO04); 
	   SET @PARAMETRO05_BIT = CONVERT(BIT, @PARAMETRO05);
	   SET @PARAMETRO07_BIT = CONVERT(BIT, @PARAMETRO07);
	   declare @FechaDiaActivo as datetime; 
	   set  @FechaDiaActivo = Convert (datetime, @PARAMETRO10);
		----EXEC Promo_Cristales_SHADES  'M144228'  ,  'C000727'  ,'',0,0,'','',    '002'   ,'CONVENCIONAL'
		EXEC CPOS_pPromoCombosConfig2020 @PARAMETRO01, @PARAMETRO02,@PARAMETRO04_BIT,@PARAMETRO05_BIT,@PARAMETRO06,@PARAMETRO07_BIT,@PARAMETRO08, @FechaDiaActivo
   END
   IF @CODPROMO = '219'
   BEGIN
	   SET @PARAMETRO04_BIT = CONVERT(BIT, @PARAMETRO04);
	   SET @PARAMETRO05_BIT = CONVERT(BIT, @PARAMETRO05);
	   SET @PARAMETRO07_BIT = CONVERT(BIT, @PARAMETRO07);
		----EXEC Promo_Cristales_SHADES  'M144228'  ,  'C000727'  ,'',0,0,'','',    '002'   ,'CONVENCIONAL'
		EXEC CPOS_Promo_Black_Friday_2024 @PARAMETRO01, @PARAMETRO02, @PARAMETRO03,@PARAMETRO04_BIT,@PARAMETRO05_BIT,@PARAMETRO06,@PARAMETRO07_BIT,@PARAMETRO08,@PARAMETRO09
   END
   
END



-----------------------Leyenda------------------------------------------------- 
-----****** Cristal  "PRECIOCRT_DESC"  Valor -----------------------------------------
-----****** Montura  "PRECIOMONT_DESC"  Valor -----------------------------------------
-----****** LenteContacto  "TOTLC"     Valor  -----------------------------------------
-----****** AR  "PRECIOAR_DESC"    Porcentaje -----------------------------------
-----****** Servicio  "PORC_SERVICIO_DESC"  Porcentaje --------------------------
-----****** Otros  "PORCDCTO"  Porcentaje ---------------------------------------


--La tabla 01 recibe "CodArticulo" el codigo Articulo a excluir 
GO





------------------------------------------------------------------------------------------------------------------


/****** Object:  StoredProcedure [dbo].[SP_CPOS_GET_TB_CTEPPAL]    Script Date: 05/06/2025 17:11:42 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CPOS_GET_TB_CTEPPAL]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_CPOS_GET_TB_CTEPPAL]
GO



/****** Object:  StoredProcedure [dbo].[SP_CPOS_GET_TB_CTEPPAL]    Script Date: 05/06/2025 17:11:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- mcll 15-04-25 Crear el Stored Procedure para obtener un registro de la tabla TB_CTEPPAL
CREATE PROCEDURE [dbo].[SP_CPOS_GET_TB_CTEPPAL] (
    @CTE_CedIden VARCHAR(20)= null,
    @CTE_Nacio VARCHAR(1)= null
)
AS
BEGIN
    -- Verificar si existe el registro con la cédula y nacionalidad proporcionadas

IF @CTE_CedIden IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT 1 FROM [dbo].[TB_CTEPPAL] WHERE CTE_CedIden = ltrim(rtrim(@CTE_CedIden)) AND CTE_Nacio = @CTE_Nacio )
    BEGIN
        -- Opcional: Devolver un mensaje indicando que no se encontró el registro
        SELECT NULL AS CTE_CedIden, NULL AS CTE_Nacio, NULL AS CTE_PNombre,  NULL AS CTE_FNac, NULL AS CTE_VIP,
               NULL AS CTE_AfilVip, NULL AS CTE_FecAfil, NULL AS COD_STCTE, NULL AS CTE_Sex,
               NULL AS CTE_CodOcup, NULL AS CTE_EdoCiv, NULL AS COD_Sucursal, NULL AS CTE_FecCreacion,
               NULL AS CTE_FecMod, NULL AS USER_CREA, NULL AS USER_MOD, NULL AS Direccion_fact,
               NULL AS Facebook, NULL AS Twitter, NULL AS Instagram, NULL AS CTE_RETISLR,
               NULL AS CTE_RETIVA, NULL [COD_Edo], NULL [COD_Ciud], NULL EDO_Nombre,
			   NULL CIUD_Nombre, null NumExamen
    END
    END
    -- Seleccionar el registro de la tabla TB_CTEPPAL basado en la cédula y nacionalidad
    SELECT
	
           p.[CTE_CedIden],
            p.[CTE_Nacio],
           [CTE_PNombre],
          
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
            p.[USER_CREA],
           [USER_MOD],
           [Direccion_fact],
           [Facebook],
           [Twitter],
           [Instagram],
           [CTE_RETISLR],
           [CTE_RETIVA],
		   isnull( [DIR_CodCiud],'') [COD_Ciud],
           isnull([DIR_CodEdo],'') [COD_Edo], 
		   E.EDO_Nombre,
		   M.CIUD_Nombre, 
		   dbo.ufn_MaxNumExamen (    @CTE_Nacio ,    @CTE_CedIden          ) NumExamen
    FROM [dbo].[TB_CTEPPAL] p left join  [VTB_MAESDIR] d on  p.[CTE_CedIden]=d.[CTE_CedIden] and 
           p.[CTE_Nacio]=d.[CTE_Nacio] AND RowNumByClient=1
		   LEFT JOIN  TB_MAESEDO E ON D.DIR_CodEdo=E.COD_Edo
		   LEFT JOIN TB_MAESCIUD M ON D.DIR_CodCiud=M.COD_Ciud AND D.DIR_CodEdo=M.COD_EDO
    WHERE 
	(p.CTE_CedIden = @CTE_CedIden OR @CTE_CedIden  IS NULL   ) 
	AND 
	(p.CTE_Nacio = @CTE_Nacio OR @CTE_Nacio  IS NULL   ) 
	
	
	ORDER BY [CTE_FecCreacion] DESC

	--select *  FROM [dbo].[TB_CTEPPAL]  ORDER BY [CTE_FecCreacion] DESC

	--select *  FROM [TB_MAESDIR] ORDER BY [DIR_Feccreacion] DESC
	--select * from TB_MAESCIUD

---      ;
END;

GO


---------------------------------------------------------------------------------


/****** Object:  StoredProcedure [dbo].[SP_CPOS_ObtenerExamenPorNumeroYNacionalidadCedula]    Script Date: 05/06/2025 17:12:08 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CPOS_ObtenerExamenPorNumeroYNacionalidadCedula]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_CPOS_ObtenerExamenPorNumeroYNacionalidadCedula]
GO


/****** Object:  StoredProcedure [dbo].[SP_CPOS_ObtenerExamenPorNumeroYNacionalidadCedula]    Script Date: 05/06/2025 17:12:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_CPOS_ObtenerExamenPorNumeroYNacionalidadCedula]
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

--------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[pGet_Usuarios_ClaveAutorizada_II]    Script Date: 05/06/2025 17:13:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[pGet_Usuarios_ClaveAutorizada_II]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[pGet_Usuarios_ClaveAutorizada_II]
GO



/****** Object:  StoredProcedure [dbo].[pGet_Usuarios_ClaveAutorizada_II]    Script Date: 05/06/2025 17:13:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



--exec pGet_Usuarios_ClaveAutorizada_II '04777'

CREATE PROCEDURE [dbo].[pGet_Usuarios_ClaveAutorizada_II]  

@Parametro as varchar(50)  

AS
BEGIN
    SELECT 
        COD_USR, 
        USER_NOMBRE + ' ' + USER_APELLIDO AS Nombre, 
        USER_CARGO,
        COD_EMPLEADO, 
        Id_Rol, 
        USER_ST,
        Id_especial,
        Email,
        COD_SUCURSAL
    FROM 
        TB_USUARIO
    WHERE 
        COD_EMPLEADO IN (SELECT Item FROM dbo.SplitString(@Parametro, ','))
        AND USER_ST = 'A'
END


GO

--------------------------Me dieron error al probar porque no existian-------------------

/****** Object:  StoredProcedure [dbo].[pGetExamenconPrisma]    Script Date: 05/10/2025 11:09:51 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[pGetExamenconPrisma]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[pGetExamenconPrisma]
GO


/****** Object:  StoredProcedure [dbo].[pGetExamenconPrisma]    Script Date: 05/10/2025 11:09:52 ******/
SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER ON
GO



/* Realizado por Jacqueline Reina
    Fecha de Creación: 28/04/2015
    Este stored obtiene el prisma del examen
 */

--[pGetExamenconPrisma]'4','v','11'

CREATE PROCEDURE [dbo].[pGetExamenconPrisma]
 @numExamen  AS INTEGER
, @cteNacio  AS VARCHAR(1)
, @cteCedula  AS VARCHAR(9)
AS

BEGIN
	SELECT    PRISMAD, PRISMAI, PRISMAD2, PRISMAI2
	FROM         TB_FICCONV
	WHERE     (CTE_CedIden = @cteCedula) AND (NUM_Examen = @numExamen) AND (CTE_Nacio = @cteNacio)
END



GO

-------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[CPOS_Max_Vta]    Script Date: 05/10/2025 11:23:43 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CPOS_Max_Vta]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CPOS_Max_Vta]
GO


/****** Object:  StoredProcedure [dbo].[CPOS_Max_Vta]    Script Date: 05/10/2025 11:23:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--EXEC CPOS_Max_Vta
CREATE PROCEDURE [dbo].[CPOS_Max_Vta] 
@Cod_Producto as varchar (1)= ''
AS
BEGIN

IF @Cod_Producto IS NULL OR @Cod_Producto = ''
begin

SELECT CodTipo, Max_Vta
FROM TB_TIPO_ARTICULO
WHERE CodTipo IN ('M', 'C', 'E', 'L', 'Q', 'S', 'W', 'V', 'X')

END

else 
begin 

SELECT CodTipo, Max_Vta
FROM TB_TIPO_ARTICULO
WHERE CodTipo IN ('M', 'C', 'E', 'L', 'Q', 'S', 'W', 'V', 'X') AND CodTipo= @Cod_Producto


end

end
GO

---------------modifique yo----------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[pValidoParametrosCRT]    Script Date: 05/10/2025 11:33:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[pValidoParametrosCRT]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[pValidoParametrosCRT]
GO


/****** Object:  StoredProcedure [dbo].[pValidoParametrosCRT]    Script Date: 05/10/2025 11:33:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





---pValidoParametrosCRT
  
  
  
  
  
--Fecha: 14/03/2013  
--Creador: Mayerlyn Campos   
--Objetivo:obtiene y compara los atributos de un cristal contra TB_CRISTAlES para permitir la venta o no  
--EXEC pValidoParametrosCRT 	'V','18001066','1','C000001','D','CERCA',0,0,0, 0,0, 'NO', '017', 'QUO', 'False', 'False', 'False', '0'
--EXEC pValidoParametrosCRT 'V','8651696','1','C000001','D','CERCA',0,0,0, 0,0, 'NO', '017', 'QUO', 'False', 'False', 'False', '0'
CREATE Procedure [dbo].[pValidoParametrosCRT]  
 @NACIO AS VARCHAR (2),  
 @CI AS VARCHAR (15),  
 @EXAM AS VARCHAR (3),   
 @CRTDERECHO AS VARCHAR (7),  
 @OJO AS VARCHAR (2),  
 @VISION AS VARCHAR (10),  
 @ALTURA AS NUMERIC (18,0),  
 @DIAMETRO AS NUMERIC (18,2),  
 @DISTVERT AS NUMERIC (18,0),  
 @ANGFAC AS NUMERIC (18,0),  
 @ANGPANT AS NUMERIC (18,0),  
 @COLOR AS VARCHAR (2),  
 @TIEMPOENTREGA AS VARCHAR (15),  
 @MONTAJE AS VARCHAR (15),  
 @MEDDIST AS VARCHAR (7),  
 @MEDANGF AS VARCHAR (7),  
 @MEDANGP AS VARCHAR (7),  
 @MEDDDL AS VARCHAR (7)  
  
 AS  
 /*  
DECLARE @NACIO AS VARCHAR (2)  
DECLARE @CI AS VARCHAR (15)  
DECLARE @EXAM AS VARCHAR (2)  
DECLARE @CRTDERECHO AS VARCHAR (7)  
DECLARE @OJO AS VARCHAR (2)  
DECLARE @VISION AS VARCHAR (10)  
DECLARE @ALTURA AS NUMERIC (18,0)  
DECLARE @DIAMETRO AS NUMERIC (18,2)  
DECLARE @DISTVERT AS NUMERIC (18,0)  
DECLARE @ANGFAC AS NUMERIC (18,0)  
DECLARE @ANGPANT AS NUMERIC (18,0)  
DECLARE @COLOR AS VARCHAR (2)  
DECLARE @TIEMPOENTREGA AS VARCHAR (12)  
DECLARE @MONTAJE AS VARCHAR (15)  
DECLARE @MEDDIST AS VARCHAR (7)  
DECLARE @MEDANGF AS VARCHAR (7)  
DECLARE @MEDANGP AS VARCHAR (7)  
  
---EXEC pValidoParametrosCRT 'V','00000143','3','C000004','D','CERCA',0,0,0, 0,0, 'SI', '004', 'SAM', 'False', 'False', 'False'  
  
set @NACIO ='V'  
set @CI ='00000143'  
set @EXAM = '3'  
SET @CRTDERECHO = 'C000001'  
SET @OJO = 'D'  
SET @VISION = 'LEJOS'  
SET @ALTURA =0  
SET @DIAMETRO =0  
SET @DISTVERT =0  
SET @ANGFAC =0  
SET @ANGPANT =0  
SET @COLOR ='NO'  
SET @TIEMPOENTREGA ='006'  
SET @MONTAJE ='SAM'  
SET @MEDDIST = 'False'  
SET @MEDANGF = 'False'  
SET @MEDANGP = 'False'  
*/  
  
----------------------------------------------------------  
  
----------------------------------------------------------  
DECLARE @DPCERCA AS VARCHAR (5)  
DECLARE @DPLEJOS AS VARCHAR (5)  
DECLARE @ADICION AS  VARCHAR (5)  
DECLARE @PRISMA AS NUMERIC (18,2)  
--DECLARE @MEDDDL AS VARCHAR (5)  
DECLARE @DDLMIN AS VARCHAR (5)  
DECLARE @DDLMAX AS VARCHAR (5)  
--Mimesys
DECLARE @CODMIMESYS AS VARCHAR (20) 
--Big Roll Out
DECLARE @PROGVISIONLEJOSDISTD AS NVARCHAR(10)
DECLARE @PROGVISIONLEJOSDISTI AS NVARCHAR(10)
DECLARE @PROGVISIONCERCADISTD AS NVARCHAR(10)
DECLARE @PROGVISIONCERCADISTI AS NVARCHAR(10)
DECLARE @PROGVISIONMEDIADISTD AS NVARCHAR(10)
DECLARE @PROGVISIONMEDIADISTI AS NVARCHAR(10)

IF @OJO = 'D'  
BEGIN  
 SELECT @DPCERCA= DPDC, @DPLEJOS=DPDL, @PRISMA= PRISMAD 
 ,@PROGVISIONLEJOSDISTD = ISNULL(PROGVISIONLEJOSDISTD,'0') 
 ,@PROGVISIONCERCADISTD = ISNULL(PROGVISIONCERCADISTD,'0')  
 ,@PROGVISIONMEDIADISTD = ISNULL(PROGVISIONMEDIADISTD,'0')  
 FROM TB_FICCONV  WHERE CTE_CedIden = @CI AND CTE_Nacio = @NACIO AND NUM_Examen = @EXAM  
   
 SELECT @ADICION= ADDD   
 FROM TB_Examen  WHERE CTE_CedIden = @CI AND CTE_Nacio = @NACIO AND NUM_Examen = @EXAM  
END  
ELSE  
BEGIN  
 SELECT @DPCERCA= DPIC, @DPLEJOS=DPIL, @PRISMA= PRISMAI  
  ,@PROGVISIONLEJOSDISTI =  ISNULL(PROGVISIONLEJOSDISTI ,'0') 
  ,@PROGVISIONCERCADISTI =  ISNULL(PROGVISIONCERCADISTI ,'0') 
  ,@PROGVISIONMEDIADISTI =  ISNULL(PROGVISIONMEDIADISTI ,'0') 
 FROM TB_FICCONV  WHERE CTE_CedIden = @CI AND CTE_Nacio = @NACIO AND NUM_Examen = @EXAM  
   
 SELECT @ADICION= ADDI   
 FROM TB_Examen  WHERE CTE_CedIden = @CI AND CTE_Nacio = @NACIO AND NUM_Examen = @EXAM  
END  

 SELECT @CODMIMESYS= CodigoMimesys   
 FROM TB_Examen  WHERE CTE_CedIden = @CI AND CTE_Nacio = @NACIO AND NUM_Examen = @EXAM  

----------------------------------------------------------  
  
--select @MEDDDL = T_DISTANCIADELECTURA  
--FROM TB_TRABAJO   
--wHERE T_CEDIDEN = @CI AND T_NACIO = @NACIO AND T_EXAMEN = @EXAM  
  
--print @MEDDDL  
--------------------------------------------------SUMA DE ADICIONES----------------------------------------------------------------------  
------SI LA VISON ES CERCA HAY QUE HACER COMPOSICION O SUMA DE ADICIONES. SE SUMA LA ADICION A LA ESFERA Y SE COLOCA ADD = 0  
------SI LA VISION ES LEJOS, LA ADD ES = 0  
  
IF @VISION = 'CERCA' OR @VISION = 'LEJOS'  
BEGIN  
 SET @ADICION = 0  
END  
---------------------------------------------------------------------------------------------------------------------------------------------  
  
  
---------------------------PARAMETROS DEL CRISTAL DESDE LA VENTA-------------------------------------------------------------------------------  
SELECT  @CRTDERECHO AS CRTDERECHO,  @OJO AS OJO,     @VISION AS VISION,   
  @ADICION AS ADICION,   @ALTURA AS ALTURA,    @PRISMA AS PRISMA,  @DIAMETRO AS DIAMETRO,   
  @DISTVERT AS DISTVERT,   @ANGFAC AS ANGFAC,    @ANGPANT  AS ANGPANT, @COLOR AS COLOR,   
  @TIEMPOENTREGA AS TIEMPOENT, @MONTAJE AS MONTAJE,   @MEDDDL AS DDL  
---------------------------------------------------------------------------------------------------------------------------------------------  
  
------------------VARIABLES PARA OBTENER LOS ATRIBUTOS DEL CRISTAL DESDE LA TABLA TB_CRISTALES----------------------------------------------------------------------------  
DECLARE  @LEJOSC AS VARCHAR(2)  
DECLARE  @CERCAC AS VARCHAR(2)  
DECLARE  @BIFC AS VARCHAR(2)  
DECLARE  @PROGC AS VARCHAR(2)  
DECLARE  @BALANCEC AS VARCHAR(2)  
  
DECLARE  @DPLEJOSC AS VARCHAR(2)  
DECLARE  @DPCERCAC AS VARCHAR(2)  
  
DECLARE  @ADDMIN AS VARCHAR(4)  
DECLARE  @ADDMAX AS VARCHAR(4)  
  
DECLARE  @ALTMIN AS VARCHAR(2)  
DECLARE  @ALTMAX AS VARCHAR(2)  
  
DECLARE  @PRIMAMIN AS VARCHAR(2)  
DECLARE  @PRIMAMAX AS VARCHAR(2)  
  
DECLARE  @DIAMMAX AS VARCHAR(2)  
  
DECLARE  @DVCMIN AS VARCHAR(3)  
DECLARE  @DVCMAX AS VARCHAR(3)  
DECLARE  @AFMIN AS VARCHAR(3)  
DECLARE  @AFMAX AS VARCHAR(3)  
DECLARE  @APMIN AS VARCHAR(3)  
DECLARE  @APMAX AS VARCHAR(3)  
  
DECLARE  @COLORC AS VARCHAR(2)  
  
--DECLARE  @EXPRESSC AS VARCHAR(2)    
DECLARE  @EXPRESS1HRC AS VARCHAR(2)    
DECLARE  @EXPRESS3HRC AS VARCHAR(2)    
DECLARE  @EXPRESS12HRC AS VARCHAR(2)    
  
DECLARE  @5DIASC AS VARCHAR(2)    
DECLARE  @7DIASHABC AS VARCHAR(2)  
DECLARE  @15DIASC AS VARCHAR(2)    
DECLARE  @21DIASC AS VARCHAR(2)    
DECLARE  @30DIASC AS VARCHAR(2)   
DECLARE  @45DIASC AS VARCHAR(2)    
DECLARE  @60DIASC AS VARCHAR(2)   
DECLARE  @90DIASC AS VARCHAR(2)   
  
DECLARE  @MONTREMOTOC AS VARCHAR(2)  
DECLARE  @MONTQUORUMC AS VARCHAR(2)  
DECLARE  @MONTBOSMERC AS VARCHAR(2)  
  
DECLARE  @COLORF12C AS VARCHAR(2)  
DECLARE  @MIMESYS AS VARCHAR(3)  

DECLARE  @ProgVisionLejosDistMin AS NVARCHAR(10)
DECLARE  @ProgVisionLejosDistMax AS NVARCHAR(10)
DECLARE  @ProgVisionCercaDistMin AS NVARCHAR(10)
DECLARE  @ProgVisionCercaDistMax AS NVARCHAR(10)
DECLARE  @ProgVisionMediaDistMin AS NVARCHAR(10)
DECLARE  @ProgVisionMediaDistMax AS NVARCHAR(10)

 
DECLARE  @CRTEXISTE AS VARCHAR(20)  
----------------------------------------------------------------------------------------------------------------------------------------------------------  
  
  
----------------------------OBTENGO Y MUESTRO LOS VALORES DEL CRISTAL SELECCIONADO----------------------------------------------------------  
SELECT  @LEJOSC= Visionlejos,  @CERCAC=Visioncerca, @BIFC= visionBif,  @PROGC= visionProg,  @BALANCEC= visionbalance,  
  @DPLEJOSC=DPLejos,   @DPCERCAC=DPCerca,  @ADDMIN=AddMinima,  @ADDMAX=AddMaxima,  @ALTMIN=AltMinima,  
  @ALTMAX=AltMaxima,   @PRIMAMIN=PrismaMinima, @PRIMAMAX=PrismaMaxima, @DVCMIN=DVCMin,   @DVCMAX=DVCMax,     
  @AFMIN=AFMin,    @AFMAX=AFMax,   @APMIN=APMin,   @APMAX=APMax,     
  @COLORC = case  when colorsi = 'X' then 'SI' else 'NO' end,     @EXPRESS1HRC=Express1Hr,@EXPRESS3HRC=Express3Hr,  
  @EXPRESS12HRC=Express12Hr, @5DIASC=[5Dias],  @15DIASC=[15Dias],  @21DIASC=[21dias],  @45DIASC=[45Dias],    
  @7DIASHABC=[7DiasHab],  @30DIASC =[30DIAS],  @60DIASC =[60DIAS],  @90DIASC =[90DIAS],  @MONTREMOTOC=MontajeRemoto,  
  @MONTQUORUMC=MontajeQuorum, @MONTBOSMERC=MontajeBosmer,      @CRTEXISTE= Cod_Cristal_Optica, @COLORF12C= ColoracionF12,  
  @DDLMIN = ddlmin, @DDLMAX = ddlmax   ,@MIMESYS = Mimesys ,
  @ProgVisionLejosDistMin = ProgVisionLejosDistMin, @ProgVisionLejosDistMax = ProgVisionLejosDistMax,
  @ProgVisionCercaDistMin = ProgVisionCercaDistMin, @ProgVisionCercaDistMax = ProgVisionCercaDistMax, 
  @ProgVisionMediaDistMin = ProgVisionMediaDistMin, @ProgVisionMediaDistMax = ProgVisionMediaDistMax
FROM TB_CRISTALES   
WHERE Cod_Cristal_Optica = @CRTDERECHO  
  
SELECT @LEJOSC AS LEJOSC,   @CERCAC AS CERCAC,   @BIFC AS BIFC,    @PROGC AS PROGC,  
  @BALANCEC AS BALANCEC, @DPLEJOSC AS DPLEJOSC,  @DPCERCAC AS DPCERCAC,  @ADDMIN AS ADDMIN,  
  @ADDMAX AS ADDMAX ,  @ALTMIN AS ALTMIN,   @ALTMAX AS ALTMAX,   @PRIMAMIN AS PRIMAMIN,  
  @PRIMAMAX AS PRIMAMAX, ISNULL(@DIAMMAX,0) AS DIAMMAX,  @DVCMIN AS DVCMIN,   @DVCMAX AS DVCMAX,  
  @AFMIN AS AFMIN,  @AFMAX AS AFMAX,   @APMIN AS APMIN,   @APMAX AS APMAX,  
  @COLORC AS COLORC,  @EXPRESS1HRC AS EXPRESS1HR, @EXPRESS3HRC AS EXPRESS3HR, @EXPRESS12HRC AS EXPRESS12HR,  
  @5DIASC AS [5DIASC], @7DIASHABC AS [7DIASHABC], @15DIASC AS [5DIASC],  @21DIASC AS [21DIASC],  
  @30DIASC AS [30DIASC], @45DIASC AS [45DIASC],  @60DIASC AS [60DIASC],  @90DIASC AS [90DIASC],  
  @MONTREMOTOC AS MONTREMC,@MONTQUORUMC AS MONTQUORUMC,@MONTBOSMERC AS MONTBOSMERC,@COLORF12C as COLORACIONF12,  
  @CRTEXISTE AS CRTEXISTE, @DDLMIN AS DDLMIN,@DDLMAX AS DDLMAX  ,@MIMESYS As MIMESYS,
  @ProgVisionLejosDistMin AS ProgVisionLejosDistMin, @ProgVisionLejosDistMax AS ProgVisionLejosDistMax,
  @ProgVisionCercaDistMin AS ProgVisionCercaDistMin, @ProgVisionCercaDistMax AS ProgVisionCercaDistMax, 
  @ProgVisionMediaDistMin AS ProgVisionMediaDistMin, @ProgVisionMediaDistMax AS ProgVisionMediaDistMax

---------------------------------------------------------------------------------------------------------------------------------------------------------------    
IF @CRTEXISTE IS NOT NULL  
BEGIN   
 SET @CRTEXISTE = 'CRISTAL EXISTE'  
 
-------------------------------------VARIABLES DE RESPUESTA----------------------------------------------------------------------------------  
 DECLARE @RESVISION AS VARCHAR (30)  
 DECLARE @RESDP AS VARCHAR (30)  
 DECLARE @RESALT AS VARCHAR (30)  
 DECLARE @RESADD AS VARCHAR (30)  
 DECLARE @RESPRISMA AS VARCHAR (30)  
 DECLARE @RESDIAM AS VARCHAR (30)  
 DECLARE @RESDVC AS VARCHAR (30)  
 DECLARE @RESAF AS VARCHAR (30)  
 DECLARE @RESAP AS VARCHAR (30)  
 DECLARE @RESCOLOR AS VARCHAR (30)  
 DECLARE @RESTIEMPOEN AS VARCHAR (30)   = '1'
 DECLARE @RESMONTAJE AS VARCHAR (30)  
 DECLARE @RESCOLORF12 AS VARCHAR (50)  
 DECLARE @RESDDL AS VARCHAR (50)  
 DECLARE @RESMIMESYS AS VARCHAR (30)  
 DECLARE @RESPROGVISIONLEJOSDIST AS VARCHAR (50)
 DECLARE @RESPROGVISIONCERCADIST AS VARCHAR (50)
 DECLARE @RESPROGVISIONMEDIADIST AS VARCHAR (50)
 ---------------------------------------------------------------------------------------------------------------------------------------------------------------    
  
  
 -----------------------------------------VALIDO VISION----------------------------------------------------------------------------------  
  
 IF (@LEJOSC = 'X' or  @LEJOSC = 'P') and @Vision = 'LEJOS' SET @RESVISION = '1'  
 IF (@LEJOSC = 'NC') and @Vision = 'LEJOS' SET @RESVISION = 'NO ACEPTA LEJOS'  
  
 IF (@CERCAC = 'X' or  @CERCAC = 'P')  and @Vision = 'CERCA' SET @RESVISION = '1'  
 IF (@CERCAC = 'NC') and @Vision = 'CERCA' SET @RESVISION = 'NO ACEPTA CERCA'  
     
 IF (@BIFC = 'X' or  @BIFC = 'P') and @Vision = 'BIFOCAL' SET @RESVISION = '1'  
 IF (@BIFC = 'NC') and @Vision = 'BIFOCAL' SET @RESVISION = 'NO ACEPTA BIFOCAL'  
  
 IF (@PROGC = 'X' or  @PROGC = 'P')  and @Vision = 'PROGRESIVO' SET @RESVISION = '1'  
  
 IF (@PROGC = 'NC') and @Vision = 'PROGRESIVO' SET @RESVISION = 'NO ACEPTA PROGRESIVO'  
    
 IF (@BALANCEC = 'X' or  @BALANCEC = 'P')  and @Vision = 'BALANCE' SET @RESVISION = '1'  
 IF (@BALANCEC = 'NC') and @Vision = 'BALANCE' SET @RESVISION = 'NO ACEPTA BALANCE'  
 ---------------------------------------------------------------------------------------------------------------------------------------------------------------    
  
 --------------------------------------------VALIDO DP----------------------------------------------------------------------------------  
 IF @DPLEJOSC = 'X' AND @DPLEJOS  <> ''  
  SET @RESDP = '1'  
 ELSE  
  SET @RESDP = 'FALTA DP LEJOS'  
    
 IF @DPLEJOSC = 'P' AND (@DPLEJOS  <> '' OR @DPLEJOS  = '')  
  SET @RESDP = '1'  
  
 IF @DPCERCAC = 'X' AND @DPCERCA  <> ''  
  SET @RESDP = '1'  
 ELSE  
  SET @RESDP = 'FALTA DP CERCA'  
    
 IF @DPCERCAC = 'P' AND (@DPCERCA  <> '' OR @DPCERCA  = '')  
  SET @RESDP = '1'  
 ---------------------------------------------------------------------------------------------------------------------------------------------------------------    
  
 --------------------------------------------VALIDO ALTURA----------------------------------------------------------------------------------  
 IF @ALTURA BETWEEN @ALTMIN AND @ALTMAX  
  SET @RESALT  = '1'  
 ELSE   
  SET @RESALT  = 'ALTURA FUERA DE RANGO ' + '(' + convert(varchar(8),@ALTURA) + ')'  
 ---------------------------------------------------------------------------------------------------------------------------------------------------------------    
  
 --------------------------------------------VALIDO ADICION----------------------------------------------------------------------------------  
 IF @ADDMIN = 'NC' AND @ADDMAX = 'NC' ---AND @ADICION = '0'  
 BEGIN  
  SET @RESADD  = '1'  
 END  
 ELSE  
 BEGIN  
  --IF @ADICION BETWEEN @ADDMIN  AND @ADDMAX   
  IF convert (decimal(18,2),@ADICION) BETWEEN @ADDMIN  AND @ADDMAX  
   SET @RESADD  = '1'  
  ELSE   
   SET @RESADD  = 'ADICION FUERA DE RANGO' + '(' + convert(varchar(8),@ADICION) + ')'  
 END  
 -----------------------------------------------------------------------------------------------------------------------------------------  
  
 --------------------------------------------VALIDO PRISMA----------------------------------------------------------------------------------  
 IF @PRIMAMIN = 'NC' AND @PRIMAMAX = 'NC' AND @PRISMA = 0  
 BEGIN  
  SET @RESPRISMA  = '1'  
 END  
 ELSE  
 BEGIN 
	IF @PRIMAMIN <> 'NC' AND @PRIMAMAX <> 'NC' -- EH. 16/10/2018
	BEGIN 
		IF @PRISMA BETWEEN @PRIMAMIN AND @PRIMAMAX   
			SET @RESPRISMA  = '1'  
		ELSE   
			SET @RESPRISMA  = 'PRISMA FUERA DE RANGO'  
	END 
	ELSE     -- EH. 16/10/2018
		SET @RESPRISMA  = 'PRISMA NO CORRESPONDE'  
 END
 -----------------------------------------------------------------------------------------------------------------------------------------  
--------------------------------------------VALIDO DDL----------------------------------------------------------------------------------  
 IF @DDLMIN = 'NC' AND @DDLMAX = 'NC' AND @MEDDDL = '0'  
 BEGIN  
  SET @RESDDL = '1'  
 END  
 ELSE  
 BEGIN  
 --select  convert (decimal(18,2),@MEDDDL)  
  IF replace(@MEDDDL,',','.') BETWEEN @DDLMIN AND @DDLMAX   
   SET @RESDDL  = '1'  
  ELSE   
   SET @RESDDL  = 'DDL FUERA DE RANGO'  
 END  
 -----------------------------------------------------------------------------------------------------------------------------------------  
 --------------------------------------------VALIDO DIAMETRO----------------------------------------------------------------------------------  
 /*  
 IF @DIAMMAX <>''  
 BEGIN   
  IF @DIAMETRO<= @DIAMMAX  
   SET @RESDIAM  = '1'  
  ELSE  
   SET @RESDIAM  = 'DIAMETRO EXEDIDO'  
 END  
 ELSE  
 BEGIN  
  SET @RESDIAM  = '1'  
 END  
 */  
 -----------------------------------------------------------------------------------------------------------------------------------------  
 --------------------------------------VALIDO PARAMETROS INDIVIDUALES-------------------------------------------------------------------------------  
 ------DISTANCIA VERTICE-----  
 IF @DVCMAX = 'NC' AND @DVCMIN = 'NC'   
 BEGIN  
  SET @RESDVC  = '1'  
 END  
 ELSE  
 BEGIN  
  IF @MEDDIST = 'TRUE'  
  BEGIN  
   IF @DISTVERT  BETWEEN @DVCMIN  AND @DVCMAX    
    SET @RESDVC  = '1'  
   ELSE   
    SET @RESDVC  = 'DIST VERTICE FUERA DE RANGO'  
  END  
  ELSE  
  BEGIN  
   SET @RESDVC  = 'FALTA DIST VERTICE'  
  END  
 END    
    
  
 ----------ANGULO FACIAL-----------  
 IF @AFMAX = 'NC' AND @AFMIN = 'NC'   
 BEGIN  
  SET @RESAF  = '1'  
 END  
 ELSE  
 BEGIN  
  IF @MEDANGF = 'TRUE'  
  BEGIN  
   IF @ANGFAC BETWEEN @AFMIN AND @AFMAX  
    SET @RESAF  = '1'  
   ELSE   
    SET @RESAF  = 'ANGULO FACIAL FUERA DE RANGO'  
  END  
  ELSE  
  BEGIN  
   SET @RESAF  = 'FALTA ANGULO FACIAL'  
  END  
 END  
  
 ---------ANGULO PANTOSCOPICO--------  
 IF @APMAX = 'NC' AND @APMIN = 'NC'   
 BEGIN  
  SET @RESAP  = '1'  
 END  
 ELSE  
 BEGIN  
  IF @MEDANGP = 'TRUE'  
  BEGIN  
   IF @ANGPANT  BETWEEN @APMIN AND @APMAX  
    SET @RESAP  = '1'  
   ELSE   
    SET @RESAP  = 'ANGULO PANT FUERA DE RANGO'  
  END  
  ELSE  
  BEGIN  
   SET @RESAP  = 'FALTA ANGULO PANTOSC.'  
  END  
 END  
 -----------------------------------------------------------------------------------------------------------------------------------------  
  
 -------------------------------------------VALIDO COLOR------------------------------------------------------------------------------------  
 IF @COLORC = 'NO' AND @COLOR = 'SI'  
  SET @RESCOLOR = 'NO PERMITE COLOR'  
 ELSE  
  SET @RESCOLOR = '1'  
 -----------------------------------------------------------------------------------------------------------------------------------------  
  
 -------------------------------------------VALIDO TIEMPO DE ENTREGA------------------------------------------------------------------------------------  
 DECLARE @EXP AS VARCHAR (10)  
 DECLARE @CODSER AS VARCHAR (3)  
   
 SELECT @EXP = CodArticulo, @CODSER = Cod_servicio FROM TB_SERVICIOSLAB WHERE Cod_servicio = @TIEMPOENTREGA and Status_Servicio = 'A'  
   
 --IF @EXP IS NOT NULL  
 --BEGIN  
 -- SET @TIEMPOENTREGA = 'EXPRESS'  
 --END  
 --ELSE  
 --BEGIN   
  --SELECT 'AQUII', @CODSER  
    
  SELECT @TIEMPOENTREGA = CASE WHEN @TIEMPOENTREGA = '007' THEN 'EXPRESS1HR'  
          WHEN @TIEMPOENTREGA = '005' THEN 'EXPRESS3HR'  
          WHEN @TIEMPOENTREGA = '004' THEN 'EXPRESS12HR'  
          WHEN @TIEMPOENTREGA = '006' or  @TIEMPOENTREGA = '008' THEN '5DIAS'  
          --WHEN @TIEMPOENTREGA = '008' THEN '5DIASHAB'  
          WHEN @TIEMPOENTREGA = '011' THEN '30DIAS'  
          WHEN @TIEMPOENTREGA = '010' THEN '45DIAS'  
          WHEN @TIEMPOENTREGA = '012' THEN '60DIAS'  
          WHEN @TIEMPOENTREGA = '014' THEN '90DIAS'  
          WHEN @TIEMPOENTREGA = '015' THEN '7DIASHAB' 
          WHEN @TIEMPOENTREGA = '016' THEN '21DIAS'   
          WHEN @TIEMPOENTREGA = '017' THEN '5DIAS' 
          WHEN @TIEMPOENTREGA = '018' THEN 'EXPRESS3HR'             
          WHEN @TIEMPOENTREGA = '019' THEN 'EXPRESS1HR'             
        END   
   
 --END  
   
  
 --IF @EXPRESSC = 'X' and @TIEMPOENTREGA = 'EXPRESS' SET @RESTIEMPOEN  = '1'  
 --IF @EXPRESSC = 'NC' and @TIEMPOENTREGA = 'EXPRESS' SET @RESTIEMPOEN  = 'NO ACEPTA EXPRESS'  
 --SELECT @TIEMPOENTREGA AS TIEMPOENTREGA  
   
 IF @EXPRESS1HRC = 'X' and @TIEMPOENTREGA = 'EXPRESS1HR' SET @RESTIEMPOEN  = '1'  
 IF @EXPRESS1HRC = 'NC' and @TIEMPOENTREGA = 'EXPRESS1HR' SET @RESTIEMPOEN  = 'NO ACEPTA EXPRESS1HR'  
   
 IF @EXPRESS3HRC = 'X' and @TIEMPOENTREGA = 'EXPRESS3HR'  SET @RESTIEMPOEN  = '1'  
 IF @EXPRESS3HRC = 'NC' and @TIEMPOENTREGA = 'EXPRESS3HR' SET @RESTIEMPOEN  = 'NO ACEPTA EXPRESS3HR'  
   
 IF @EXPRESS12HRC = 'X' and @TIEMPOENTREGA = 'EXPRESS12HR' SET @RESTIEMPOEN  = '1'    
 IF @EXPRESS12HRC = 'NC' and @TIEMPOENTREGA = 'EXPRESS12HR' SET @RESTIEMPOEN  = 'NO ACEPTA EXPRESS12HR' --SET @RESTIEMPOEN  = 'NO ACEPTA EXPRESS12HR'  
   
 IF @5DIASC = 'X' and @TIEMPOENTREGA = '5DIAS' SET @RESTIEMPOEN  = '1'  
 IF @5DIASC = 'NC' and @TIEMPOENTREGA = '5DIAS' SET @RESTIEMPOEN  = 'NO ACEPTA 5DIAS'  
   
 IF @7DIASHABC = 'X' and @TIEMPOENTREGA = '7DIASHAB' SET @RESTIEMPOEN  = '1'  
 IF @7DIASHABC = 'NC' and @TIEMPOENTREGA = '7DIASHAB' SET @RESTIEMPOEN  = 'NO ACEPTA 7DIAS HAB'  
  
 IF @15DIASC = 'X' and @TIEMPOENTREGA = '15DIAS' SET @RESTIEMPOEN  = '1'  
 IF @15DIASC = 'NC' and @TIEMPOENTREGA = '15DIAS' SET @RESTIEMPOEN  = 'NO ACEPTA 15DIAS'  
  
 IF @21DIASC = 'X' and @TIEMPOENTREGA = '21DIAS' SET @RESTIEMPOEN  = '1'  
 IF @21DIASC = 'NC' and @TIEMPOENTREGA = '21DIAS' SET @RESTIEMPOEN  = 'NO ACEPTA 21DIAS'  
   
 IF @30DIASC = 'X' and @TIEMPOENTREGA = '30DIAS' SET @RESTIEMPOEN  = '1'  
 IF @30DIASC = 'NC' and @TIEMPOENTREGA = '30DIAS' SET @RESTIEMPOEN  = 'NO ACEPTA 30DIAS'  
  
 IF @45DIASC = 'X' and @TIEMPOENTREGA = '45DIAS' SET @RESTIEMPOEN  = '1'  
 IF @45DIASC = 'NC' and @TIEMPOENTREGA = '45DIAS' SET @RESTIEMPOEN  = 'NO ACEPTA 45DIAS'  
     
 IF @60DIASC = 'X' and @TIEMPOENTREGA = '60DIAS' SET @RESTIEMPOEN  = '1'  
 IF @60DIASC = 'NC' and @TIEMPOENTREGA = '60DIAS' SET @RESTIEMPOEN  = 'NO ACEPTA 60DIAS'  
  
 IF @90DIASC = 'X' and @TIEMPOENTREGA = '90DIAS' SET @RESTIEMPOEN  = '1'  
 IF @90DIASC = 'NC' and @TIEMPOENTREGA = '90DIAS' SET @RESTIEMPOEN  = 'NO ACEPTA 90DIAS'  
 -----------------------------------------------------------------------------------------------------------------------------------------  
   
 -------------------------------------------VALIDO MONTAJE------------------------------------------------------------------------------------  
 IF @MONTBOSMERC = 'X' and @MONTAJE  = 'BOS'   
 BEGIN  
  --SELECT '1'  
  SET @RESMONTAJE = '1'  
 END  
 ELSE  
 BEGIN  
  /*IF @MONTREMOTOC = 'X' AND (@MONTQUORUMC  = 'X' or @COLOR= 'SI')  
  BEGIN  
   SELECT '2'  
   SET @RESMONTAJE = '1'  
  END  
  ELSE  
  BEGIN*/    
   IF (@MONTREMOTOC = 'X' and @MONTQUORUMC  = 'X')   
   BEGIN  
    --SELECT '3'  
    SET @RESMONTAJE = '1'  
   END  
   ELSE  
   BEGIN  
    IF (@MONTREMOTOC = 'NC' and @MONTQUORUMC  = 'X') and (@MONTAJE  = 'QUO' or @MONTAJE  = 'TAM')    
    BEGIN  
     --SELECT '4'  
     SET @RESMONTAJE   = '1'  
    END  
    ELSE  
    BEGIN  
     --SELECT '4.1'  
     IF (@MONTREMOTOC = 'X' and @MONTQUORUMC  = 'NC') and @MONTAJE  <> 'QUO'   
     BEGIN  
      --SELECT '5'  
      SET @RESMONTAJE   = '1'  
     END  
     ELSE  
     BEGIN  
      --SELECT '5.1'  
      SET @RESMONTAJE   = 'NO ACEPTA MONTAJE '  + @MONTAJE  
     END    
      --SET @RESMONTAJE   = 'NO ACEPTA MONTAJE '  + @MONTAJE  
    END     
        
   END    
  --END   
 END  
   
   
 /*  
  IF (@MONTQUORUMC = 'NC' and @MONTREMOTOC = 'X') and @MONTAJE  <> 'QUO' SET @RESMONTAJE   = '1'  ELSE SET @RESMONTAJE   = 'NO ACEPTA MONTAJE QUORUM'  
  IF (@MONTBOSMERC  = 'NC' and @MONTREMOTOC = 'X') and @MONTAJE  <> 'BOS' SET @RESMONTAJE   = '1'  ELSE SET @RESMONTAJE   = 'NO ACEPTA MONTAJE BOSMER'  
  IF ((@MONTQUORUMC = 'X' or @MONTBOSMERC = 'X') and @MONTREMOTOC = 'NC') and @MONTAJE  = 'QUO' SET @RESMONTAJE   = '1'  ELSE SET @RESMONTAJE   = 'NO ACEPTA MONTAJE REM'  
    
  */  
  
   
   
 -----------------------------------------------------------------------------------------------------------------------------------------  
 ---SELECT @TIEMPOENTREGA AS TIEMPOENTREGA, @COLOR AS COLOR, @EXPRESS12HRC AS EXPRESS12HR  
   
 ------------------------------------------VALIDO COLORACIONF12------------------------------------------------------------------------------------  
 -----esta validacion indica que no permite coloracion con express F12  
 ----- SOLO PERMITE ESTA CONDICION SI EL MONTAJE ES REMOTO Y SI EL CRISTAL ES C000001  
   
 IF @CRTDERECHO = 'C000001'  
 BEGIN  
  --SELECT '1'  
  IF  (@TIEMPOENTREGA = 'EXPRESS12HR' or @TIEMPOENTREGA = 'EXPRESS3HR')  AND @COLOR= 'SI' AND  @MONTAJE = 'QUO'  
  BEGIN  
   --SELECT 'AQUI'  
   SET @RESCOLORF12  = 'NO ACEPTA F12 CON COLORACION EN QUORUM'  
          
  END  
  ELSE  
  BEGIN  
   --SELECT 'AQUI2'  
   SET @RESCOLORF12  = '1'   
     
     
  END  
   
   
 END   
 ELSE  
 BEGIN   
  --SELECT '2'  
  IF (@TIEMPOENTREGA = 'EXPRESS12HR' or @TIEMPOENTREGA = 'EXPRESS3HR')  AND @COLOR= 'SI' AND  @COLORF12C = 'NC'  
  BEGIN  
   ---SELECT 'AQUI'  
   SET @RESCOLORF12  = 'NO ACEPTA F12 CON COLORACION'  
  END  
  ELSE  
  BEGIN  
   ---SELECT 'AQUI2'  
   SET @RESCOLORF12  = '1'        
  END   
 END   
 -----------------------------------------------------------------------------------------------------------------------------------------  
 ---------MIMESYS--------
 IF @MIMESYS = 'X'  -- Si el cristal aplica para mimesys
	 BEGIN 
		IF @CODMIMESYS <> '' --Si el examen tiene cod mimesys
		BEGIN 
			SET @RESMIMESYS  = '1'  
		END
		ELSE
		BEGIN
			SET @RESMIMESYS  = 'FALTA CODIGO DE MIMESYS'  
		END
	END 
	ELSE
	BEGIN
		IF @CODMIMESYS <> '' --Si el examen tiene cod mimesys
		BEGIN 
			SET @RESMIMESYS  = 'NO ACEPTA CODIGO DE MIMESYS'   
		END
		ELSE
		BEGIN
			SET @RESMIMESYS  = '1'
		END
	END
	
 --------------------------------------------VALIDO BIG ROLL OUT----------------------------------------------------------------------------------  
	 --select  CONVERT(INTEGER,@PROGVISIONCERCADISTD)  , CONVERT(INTEGER,@ProgVisionCercaDistMin)  , CONVERT(INTEGER,@ProgVisionCercaDistMax) 
	 
	 -- --IF CONVERT(INTEGER,@PROGVISIONCERCADISTD)  <= CONVERT(INTEGER,@ProgVisionCercaDistMin)  
	 -- --AND CONVERT(INTEGER,@PROGVISIONCERCADISTD) >= CONVERT(INTEGER,@ProgVisionCercaDistMax)  
	 --  IF CONVERT(INTEGER,@PROGVISIONCERCADISTD)  BETWEEN @ProgVisionCercaDistMax  AND @ProgVisionCercaDistMin  
	 --BEGIN
	 -- select 1
	 --END
	 --ELSE
	 --BEGIN
	 -- select 2
	 --END 
	 
IF @OJO = 'D'  
BEGIN	
	--LEJOS
	 IF CONVERT(INTEGER,@PROGVISIONLEJOSDISTD)  BETWEEN @ProgVisionLejosDistMin  AND @ProgVisionLejosDistMax  
	 BEGIN
	  SET @RESPROGVISIONLEJOSDIST   = '1'  
	 END 
	 ELSE  
	 BEGIN 
	  SET @RESPROGVISIONLEJOSDIST  = 'VISION LEJOS PROG. FUERA DE RANGO ' + '(' + convert(varchar(8),@PROGVISIONLEJOSDISTD) + ')'  
	 END 

	 --CERCA
	 IF CONVERT(INTEGER,@PROGVISIONCERCADISTD)  BETWEEN @ProgVisionCercaDistMax  AND @ProgVisionCercaDistMin  
	 BEGIN
	  SET @RESPROGVISIONCERCADIST   = '1'  
	 END
	 ELSE   
	 BEGIN
	  SET @RESPROGVISIONCERCADIST  = 'VISION CERCA PROG. FUERA DE RANGO ' + '(' + convert(varchar(8),@PROGVISIONCERCADISTD) + ')'  
	 END
	  --MEDIA
	 IF CONVERT(INTEGER,@PROGVISIONMEDIADISTD)  BETWEEN @ProgVisionMediaDistMin  AND @ProgVisionMediaDistMax  
	 BEGIN
	  SET @RESPROGVISIONMEDIADIST   = '1'  
	 END
	 ELSE
	 BEGIN   
	  SET @RESPROGVISIONMEDIADIST  = 'VISION MEDIA PROG. FUERA DE RANGO ' + '(' + convert(varchar(8),@PROGVISIONMEDIADISTD) + ')'  
	 END 
END
ELSE
BEGIN  
	--LEJOS
	 IF CONVERT(INTEGER,@PROGVISIONLEJOSDISTI)  BETWEEN @ProgVisionLejosDistMin  AND @ProgVisionLejosDistMax  
	 BEGIN
	  SET @RESPROGVISIONLEJOSDIST   = '1'  
	 END 
	 ELSE  
	 BEGIN 
	  SET @RESPROGVISIONLEJOSDIST  = 'VISION LEJOS PROG. FUERA DE RANGO ' + '(' + convert(varchar(8),@PROGVISIONLEJOSDISTI) + ')'  
	 END 
	--CERCA
	 IF CONVERT(INTEGER,@PROGVISIONCERCADISTI)  BETWEEN @ProgVisionCercaDistMax  AND @ProgVisionCercaDistMin  
	 BEGIN
	  SET @RESPROGVISIONCERCADIST   = '1'  
	 END
	 ELSE   
	 BEGIN
	  SET @RESPROGVISIONCERCADIST  = 'VISION CERCA PROG. FUERA DE RANGO ' + '(' + convert(varchar(8),@PROGVISIONCERCADISTI) + ')'  
	 END
	  --MEDIA
	 IF CONVERT(INTEGER,@PROGVISIONMEDIADISTI)  BETWEEN @ProgVisionMediaDistMin  AND @ProgVisionMediaDistMax  
	 BEGIN
	  SET @RESPROGVISIONMEDIADIST   = '1'  
	 END
	 ELSE
	 BEGIN   
	  SET @RESPROGVISIONMEDIADIST  = 'VISION MEDIA PROG. FUERA DE RANGO ' + '(' + convert(varchar(8),@PROGVISIONMEDIADISTI) + ')'  
	 END 
END
 ---------------------------------------------------------------------------------------------------------------------------------------------------------------    
  	
END  
  
ELSE  
BEGIN  
 SET @CRTEXISTE = 'CRISTAL NO EXISTE'  
 SET @RESVISION = '1'  
 SET @RESDP = '1'  
 SET @RESALT = '1'  
 SET @RESADD = '1'  
 SET @RESPRISMA = '1'  
 SET @RESDIAM = '1'  
 SET @RESDVC = '1'  
 SET @RESAF = '1'  
 SET @RESAP = '1'  
 SET @RESCOLOR = '1'  
 SET @RESTIEMPOEN = '1'  
 SET @RESMONTAJE = '1'  
 SET @RESCOLORF12 = '1'  
 SET @RESMIMESYS = '1'  
 SET @RESPROGVISIONLEJOSDIST = '1'  
 SET @RESPROGVISIONCERCADIST = '1'   
 SET @RESPROGVISIONMEDIADIST = '1'  
END  
  
------------------------------------------------------RESULTADO FINAL------------------------------------------------------------------------  
 SELECT @RESVISION AS RESVISION
 , @RESDP AS RESDISTPUP
 , @RESALT AS RESALTURA
 , @RESADD AS RESADD
 , @RESPRISMA AS RESPRISMA
 , @RESDVC AS RESDISTVERT
 , @RESAF AS RESANGFAC
 , @RESAP AS RESANGPANT
 , @RESCOLOR AS RESCOLOR
 , @RESTIEMPOEN AS RESTIEMPOENT
 , @RESMONTAJE AS RESMONTAJE
 , @RESCOLORF12 AS RESCOLORF12
 , isnull(@RESDDL,1) AS RESDDL
 , @RESMIMESYS AS RESMIMESYS
 , @RESPROGVISIONLEJOSDIST RESPROGVISIONLEJOSDIST
 , @RESPROGVISIONCERCADIST RESPROGVISIONCERCADIST 
 , @RESPROGVISIONMEDIADIST RESPROGVISIONMEDIADIST 
 , @CRTEXISTE AS RESCRISTAL  
 


GO


/****** Object:  StoredProcedure [dbo].[pValidoParametrosCRT]    Script Date: 05/10/2025 11:33:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CPOS_CodServiciosLaboratorio]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_CPOS_CodServiciosLaboratorio]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- EXEC [dbo].[SP_CPOS_CodServiciosLaboratorio]
CREATE PROCEDURE [dbo].[SP_CPOS_CodServiciosLaboratorio]

AS
BEGIN

SELECT 
	Cod_servicio as CodServ,
    Descripcion_servicio as ServicioLab
FROM 
	TB_SERVICIOSLAB

WHERE
	Status_Servicio = 'A'

END

GO

/****** Object:  StoredProcedure [dbo].[pValidoParametrosCRT]    Script Date: 05/10/2025 11:33:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CPOS_ListaLaboratorios]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_CPOS_ListaLaboratorios]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_CPOS_ListaLaboratorios]

AS
BEGIN


SELECT 
	CODIGO_LAB as CodLab,
	DESCRIPCION as Laboratorio

FROM 
	TB_LABORATORIOS

WHERE 
	ST_LABORATORIO = 'A'

END

GO

/****** Object:  StoredProcedure [dbo].[pValidoParametrosCRT]    Script Date: 05/10/2025 11:33:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CPOS_ModificarTrabajo_TB_TRABAJO]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_CPOS_ModificarTrabajo_TB_TRABAJO]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_CPOS_ModificarTrabajo_TB_TRABAJO]
  @CedulaCliente VARCHAR(20),
    @NacioRifCliente VARCHAR(5),
    @OrdenServicio VARCHAR(20),
    @FechaModificacion DATETIME,
    @UsuarioModificacion VARCHAR(50),
    @SucursalActual VARCHAR(10),
    @CorrelativoOS VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE TB_TRABAJO
        SET 
            T_NUMORDSERV = @OrdenServicio,
            T_FECMOD = @FechaModificacion,
            USER_MOD = @UsuarioModificacion
        WHERE
            T_CEDIDEN = @CedulaCliente
            AND T_NACIO = @NacioRifCliente
            AND T_SUCURSAL = @SucursalActual
            AND Correlativo = @CorrelativoOS
            AND T_NUMORDSERV IS NULL;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION

        DECLARE @ErrorMessage NVARCHAR(4000)
        DECLARE @ErrorSeverity INT
        DECLARE @ErrorState INT

        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE()

        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState)
    END CATCH
END

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CPOS_InsertarDetalleOrdenServicio]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_CPOS_InsertarDetalleOrdenServicio]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[SP_CPOS_InsertarDetalleOrdenServicio]
    @CodSucursal VARCHAR(10),
    @NumOrdserv VARCHAR(20),
    @Ordserv_sec INT,
    @Revision VARCHAR(50),
    @Cod_Venta VARCHAR(50),
    @CodArticulo VARCHAR(20),
    @Ordserv_Cant INT,
    @Ordser_Ojo VARCHAR(2) = NULL,
    @Ordserv_Precio DECIMAL(18,2),
    @Ordserv_PorcImp DECIMAL(18,2),
    @Ordserv_PorcDto DECIMAL(18,2),
    @Ordserv_PrecioAnterior DECIMAL(18,2),
    @COD_Prom VARCHAR(20) = NULL,
    @Costo DECIMAL(18,2)


AS

BEGIN TRANSACTION
	
DECLARE @ControlErrores AS INT  
DECLARE @Error AS INT  

DECLARE @UltimoIdTasa AS INTEGER
	DECLARE @TasaActual AS NUMERIC(28,6)
	SELECT @UltimoIdTasa = MAX(ID_TASA) FROM TB_TASA WHERE Cod_Moneda = '01'
	
	SELECT @TasaActual = Tasa  FROM TB_TASA WHERE ID_Tasa = @UltimoIdTasa

    -- Declarar la variable para el impuesto
    DECLARE @Ordserv_Impuesto DECIMAL(28,2)
    DECLARE @Ordserv_Dto DECIMAL(28,2)
    DECLARE @Ordserv_Bruto DECIMAL(28,2)
    DECLARE @Ordserv_Neto DECIMAL(28,2)
    DECLARE @Ordserv_PrecioDOL DECIMAL(28,6)
    -- Calcular el impuesto
    SET @Ordserv_Impuesto = 
        CASE 
            WHEN @Ordserv_PorcImp IS NOT NULL 
            THEN ((@Ordserv_Precio * @Ordserv_Cant) -(@Ordserv_Cant * @Ordserv_Precio * (@Ordserv_PorcDto / 100))) * (@Ordserv_PorcImp / 100)
            ELSE 0
        END
        
          -- Calcular el descuento
    SET @Ordserv_Dto = 
        CASE 
            WHEN @Ordserv_PorcDto IS NOT NULL 
            THEN @Ordserv_Cant * @Ordserv_Precio * (@Ordserv_PorcDto / 100)
            ELSE 0
        END
        
            -- Calcular el bruto (precio * cantidad)
    SET @Ordserv_Bruto = @Ordserv_Precio * @Ordserv_Cant

    -- Calcular el neto (bruto + impuesto - descuento)
    SET @Ordserv_Neto = @Ordserv_Bruto + (((@Ordserv_Precio * @Ordserv_Cant) -(@Ordserv_Cant * @Ordserv_Precio * (@Ordserv_PorcDto / 100))) * (@Ordserv_PorcImp / 100)) - @Ordserv_Dto
    
    set @Ordserv_PrecioDOL =  (@Ordserv_Precio)/@TasaActual 
    
    select @Cod_Venta = cod_venta from dbo.TB_VENTADETALLE where Cod_DetVta = @Cod_Venta
       
        INSERT INTO TB_DEORDSER (
            CodSucursal,
            NumOrdserv,
            Ordserv_sec,
            Revision,
            Cod_Venta,
            CodArticulo,
            Ordserv_Cant,
            Ordser_Ojo,
            Ordserv_Precio,
            Ordserv_PorcImp,
            Ordserv_PorcDto,
            Ordserv_PrecioAnterior,
            COD_Prom,
            Costo,
            Ordserv_Impuesto, Ordserv_Dto, Ordserv_Bruto, Ordserv_Neto, Ordserv_PrecioDOL
        )
        VALUES (
            @CodSucursal,
            @NumOrdserv,
            @Ordserv_sec,
            @Revision,
            @Cod_Venta,
            @CodArticulo,
            @Ordserv_Cant,
            @Ordser_Ojo,
            @Ordserv_Precio,
            @Ordserv_PorcImp,
            @Ordserv_PorcDto,
            @Ordserv_PrecioAnterior,
            @COD_Prom,
            @Costo,
            @Ordserv_Impuesto,@Ordserv_Dto,@Ordserv_Bruto,@Ordserv_Neto,@Ordserv_PrecioDOL
        )

IF @@ERROR <> 0  
    SET @ControlErrores = @ControlErrores + 1  


IF @ControlErrores <> 0  
BEGIN  
	ROLLBACK TRAN  
	SELECT 'FALLIDO' AS resultado
END 
ELSE  
BEGIN  
	COMMIT TRAN   
	SELECT 'SATISFACTORIO' AS resultado
END
 
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CPOS_TipoVenta]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_CPOS_TipoVenta] 
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- exec SP_CPOS_TipoVenta ''
CREATE PROCEDURE [dbo].[SP_CPOS_TipoVenta] 
@Cod_DetVta as varchar (2) = '' 

AS

IF @Cod_DetVta IS NULL OR @Cod_DetVta = ''
begin

SELECT  VD.Cod_DetVta CodModo, VD.Cod_Venta CodVenta, VD.Descripcion Descripcion

from dbo.TB_TIPOVENTA TV 

INNER JOIN dbo.TB_VENTADETALLE VD ON VD.COD_Venta = TV.COD_Venta

where  VD.Activo = 1 

end 


else 
begin 


SELECT  VD.Cod_DetVta CodModo, VD.Cod_Venta CodVenta, VD.Descripcion Descripcion

from dbo.TB_TIPOVENTA TV 

INNER JOIN dbo.TB_VENTADETALLE VD ON VD.COD_Venta = TV.COD_Venta

where  VD.Activo = 1 and VD.Cod_DetVta= @Cod_DetVta

end

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CPOS_AgregarOrdenServicio]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_CPOS_AgregarOrdenServicio] 
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_CPOS_AgregarOrdenServicio]
    @Cod_Sucursal VARCHAR(3),
    @Revision VARCHAR(2),
    @Cod_Venta VARCHAR(3),
    @CTE_Nacio VARCHAR(1),
    @CTE_CedIden VARCHAR(10),
    @NumExamen INT,
    @COD_EMPLEADO VARCHAR(5),
    @Cod_Laboratorio VARCHAR(11),
    @Cod_Servicio VARCHAR(3),
    @Vision VARCHAR(10),
    @Fec_Ofrecido DATE,
    @Hor_Ofrecido VARCHAR(13),
    @Fec_Entrega DATE = NULL,
    @Fec_Envio DATE = NULL,
    @VtaSubTotal DECIMAL(28,2),
    @VtaImpuesto DECIMAL(28,2),
    @VtaDescuento DECIMAL(28,2),
    @VtaTotal DECIMAL(28,2),
    @OrSer_Finan BIT,
    @OrSer_Status VARCHAR(3),
    @OrSer_Observ VARCHAR(100),
    @USER_Crea VARCHAR(5),
    @Fecha DATE,
    @MonturaPropia BIT,
    @Cod_DetVta VARCHAR(2),
    @Aplica BIT,
    @OTCORRESPONDIENTE NVARCHAR(7),
    @VentaAfil BIT,
    @CristalPropio BIT,
    @TipoMonturaPropia VARCHAR(20) = NULL,
    @CodMotivoReposicion VARCHAR(3) = NULL,
    @Cedula_CteAfil VARCHAR(20) = NULL,
    @Codigo_EmpAfil VARCHAR(20) = NULL,
    @Asegurada BIT = NULL,
    @Exonerada BIT = NULL,
    @MonturaEnQuorum BIT,
    @Cod_Coloracion NVARCHAR(6) = NULL

AS


BEGIN TRANSACTION
	
DECLARE @ControlErrores AS INT  
DECLARE @Error AS INT  

    -- Generar el número de orden usando el SP 'SP_CPOS_CrearNumeroOrdenServicio'
    --EXEC SP_CPOS_CrearNumeroOrdenServicio @Cod_Sucursal = @Cod_Sucursal, @NuevoNumeroOrden = @NumOrdserv OUTPUT;
	DECLARE @NuevoNumOrdserv NVARCHAR(7);

        -- Buscar el último número de orden para la sucursal
        SELECT @NuevoNumOrdserv = 
            RIGHT('0000000' + CAST(ISNULL(MAX(CAST(NumOrdserv AS INT)), 0) + 1 AS VARCHAR), 7)
        FROM TB_CAORDSER
        WHERE Cod_Sucursal = @Cod_Sucursal

    INSERT INTO 
		TB_CAORDSER 
	(
        Cod_Sucursal, NumOrdserv, Revision, Cod_Venta, CTE_Nacio, CTE_CedIden, NumExamen, COD_EMPLEADO, Cod_Laboratorio, 
		Cod_Servicio, Vision, Fec_ofrecido, Hor_ofrecido, Fec_Entrega, Fec_Envio, VtaSubTotal, VtaImpuesto, VtaDescuento, 
		VtaTotal, OrSer_Saldo, OrSer_Finan, OrSer_Status, OrSer_Observ, USER_Crea, Fecha, MonturaPropia, Cod_DetVta, Aplica, 
		OTCORRESPONDIENTE, VentaAfil, CristalPropio, TipoMonturaPropia, CodMotivoReposicion, Cedula_CteAfil, Codigo_EmpAfil,
        Asegurada, Exonerada, MonturaEnQuorum, Cod_Coloracion
    )

    VALUES 
	(
        @Cod_Sucursal, @NuevoNumOrdserv, @Revision, @Cod_Venta, @CTE_Nacio, @CTE_CedIden, @NumExamen, @COD_EMPLEADO, @Cod_Laboratorio,
		@Cod_Servicio, @Vision, @Fec_Ofrecido, @Hor_Ofrecido, @Fec_Entrega, @Fec_Envio, @VtaSubTotal, @VtaImpuesto, @VtaDescuento, 
		@VtaTotal, @VtaTotal, @OrSer_Finan, @OrSer_Status, @OrSer_Observ, @USER_Crea, @Fecha, @MonturaPropia, @Cod_DetVta, @Aplica, 
		@OTCORRESPONDIENTE, @VentaAfil, @CristalPropio, @TipoMonturaPropia, @CodMotivoReposicion, NULL, @Codigo_EmpAfil,
        @Asegurada, @Exonerada, @MonturaEnQuorum, @Cod_Coloracion
    )


IF @@ERROR <> 0  
    SET @ControlErrores = @ControlErrores + 1  


IF @ControlErrores <> 0  
BEGIN  
	ROLLBACK TRAN  
	SELECT 'FALLIDO' AS resultado
END 
ELSE  
BEGIN  
	COMMIT TRAN   
	SELECT @NuevoNumOrdserv AS resultado
END 

GO


/****** Object:  StoredProcedure [dbo].[SP_CPOS_CrearNumeroOrdenServicio]    Script Date: 05/12/2025 19:54:21 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CPOS_CrearNumeroOrdenServicio]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_CPOS_CrearNumeroOrdenServicio]
GO


/****** Object:  StoredProcedure [dbo].[SP_CPOS_CrearNumeroOrdenServicio]    Script Date: 05/12/2025 19:54:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_CPOS_CrearNumeroOrdenServicio]

    @Cod_Sucursal VARCHAR(10),
    @NuevoNumeroOrden VARCHAR(10) OUTPUT

AS
BEGIN

    SET NOCOUNT ON

    BEGIN TRY
        BEGIN TRANSACTION;

    DECLARE @UltimoNumero INT

    SELECT TOP 1 @UltimoNumero = CAST(NumOrdserv AS INT)
    FROM TB_CAORDSER
    WHERE Cod_Sucursal = @Cod_Sucursal
    ORDER BY CAST(NumOrdserv AS INT) DESC

    IF @UltimoNumero IS NULL
        SET @UltimoNumero = 1
    ELSE
        SET @UltimoNumero = @UltimoNumero + 1

    -- Rellenar con ceros a la izquierda hasta 7 dígitos
    SET @NuevoNumeroOrden = RIGHT('0000000' + CAST(@UltimoNumero AS VARCHAR), 7)

	 COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION

        DECLARE @ErrorMessage NVARCHAR(4000)
        DECLARE @ErrorSeverity INT
        DECLARE @ErrorState INT

        SELECT 
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE()

        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState)
    END CATCH

END

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CPOS_pGetMonturaAlmacenQUORUM]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_CPOS_pGetMonturaAlmacenQUORUM]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_CPOS_pGetMonturaAlmacenQUORUM]
 @NumordServ as varchar(7),
 @CodArticulo as varchar(7),
 @suc as varchar(3),
 @CodServicio as varchar(3)

AS
SET NOCOUNT ON
/*declare @OS as varchar(15)
set @os = '0003002'
*/

DECLARE @GeneraMovMB AS BIT

SELECT @GeneraMovMB = valor FROM TB_PARAMETRO WHERE Parametro = 'GeneraMovMB'

select * from TB_DEORDSER  d 
inner join TB_MONTURAQUORUM m on
d.CodArticulo = m.CodArticulo 
where numordserv = @NumordServ

--Si el parametro para generar mov de bolnturas boleita esta encendido
IF @GeneraMovMB = 1
BEGIN
	PRINT 1
	--Si es entrega 3 HRS
	IF @CodServicio = '018'
	BEGIN
		--Si es de BOLEITA
		IF EXISTS ( SELECT  CodArticulo FROM TB_MONTURAQUORUM
					WHERE CodArticulo = @CodArticulo
					AND ubicacion = 'BOL')
		BEGIN --Forza selec con filtro para traer tabla en blanco
			SELECT  CodArticulo from TB_MONTURAQUORUM
			WHERE CodArticulo = @CodArticulo
			AND ubicacion = 'SINUBICACION'
		END
		ELSE --Busca en Quorum
		BEGIN
			SELECT  CodArticulo from TB_MONTURAQUORUM
			WHERE CodArticulo = @CodArticulo
			AND ubicacion = 'QUO'
		END
	END
	ELSE
	BEGIN--Busca Monturas en ambos catalogos BOl,QUO
		SELECT  CodArticulo from TB_MONTURAQUORUM
		WHERE CodArticulo = @CodArticulo
	END
END
ELSE --Busca Monturas en ambos catalogos BOl,QUO
BEGIN
	SELECT  CodArticulo from TB_MONTURAQUORUM
	WHERE CodArticulo = @CodArticulo
END

GO

----------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[PromodeMayoJunio_2025]    Script Date: 05/13/2025 20:49:26 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PromodeMayoJunio_2025]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[PromodeMayoJunio_2025]
GO


/****** Object:  StoredProcedure [dbo].[PromodeMayoJunio_2025]    Script Date: 05/13/2025 20:49:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[PromodeMayoJunio_2025]

	  @MONTURA AS VARCHAR(7) 
	, @CRISTAL AS VARCHAR(7)  
	, @LC AS VARCHAR(7)  
	, @MONTURAPROPIA AS BIT
	, @CRISTALPROPIO AS BIT
	, @SERVICIO AS VARCHAR(7)
	, @SERVAR AS BIT
	, @TIPOVENTA AS VARCHAR(3) -- "001 Venta Directa" o "002 CONVENCIONAL"
	, @TipoExamen AS VARCHAR(13)
	
AS
-- MONTURA    
DECLARE @PRECIOMONT AS NUMERIC (18,2) 
DECLARE @LETRAMLS AS VARCHAR (3) 
DECLARE @PRECIOMONT_20 AS NUMERIC(28,2)
-- CRISTAL
DECLARE @PRECIOCRT AS NUMERIC(28,2)
DECLARE @PRECIOCRT_20 AS NUMERIC(28,2)
--SERVICIO
DECLARE @PRECIOSERV AS NUMERIC(28,2)
DECLARE @PRECIOSERV_20 AS NUMERIC(28,2)

Declare @PORCDCTO AS INT 
DECLARE @PARTE AS INT
DECLARE @RESULTADO AS VARCHAR(15)
-- TOTAL --
DECLARE @TOTMONT AS NUMERIC(28,2) -- MONTURA 
DECLARE @TOTCRT AS NUMERIC(28,2)-- CRISTAL
DECLARE @TOTLC AS NUMERIC(28,2)-- CRISTAL
DECLARE @TOTANT AS NUMERIC(28,2)  -- ANTIREFLEJO CLEAR

SET @RESULTADO = 'NO APLICA'

	-- LETRA DE LA MONTURA 
	SELECT @LETRAMLS=CodRango , @PRECIOMONT = ART_PVP 
	FROM TB_ARTICULO 
	WHERE CodArticulo = @MONTURA
	

	-- PRECIO DE ANTIREFLEJO CLEAR Y DESCUENTO DEL 50 %
	SELECT @PRECIOSERV = Art_PVP
	FROM TB_ARTICULO 
	WHERE CodArticulo = @SERVICIO
						
	-- PRECIO DEL CRISTAL
	SELECT @PRECIOCRT = art_pvp
	FROM TB_ARTICULO 
	WHERE CodArticulo = @CRISTAL
	
Declare @Cristales_2 as varchar (7)
set @Cristales_2= ''
Declare @Pertenece_Marca AS bit

	SELECT @Pertenece_Marca = marca, @Cristales_2=Codigo
	FROM  Cristales_Rodenstock_RLX_Oculus2 
	WHERE Codigo = @CRISTAL
	
	
DECLARE @DESCMONT AS INTEGER -- MONTURA  	
	SELECT	@DESCMONT = PorctDescuento
		FROM TB_ARTICULO 
		WHERE CodArticulo = @MONTURA

DECLARE @MARCA_LENTE AS VARCHAR (4)
SET @MARCA_LENTE= (SELECT REPLACE (MARCA, ' ', '')  FROM TB_ARTICULO WHERE CodArticulo = @MONTURA)	

IF (@TipoExamen = 'CONVENCIONAL')
BEGIN

    IF (@DESCMONT = 0) -- DESC MONTURA
    BEGIN

        IF (@MONTURAPROPIA = 0 AND @CRISTALPROPIO = 0) -- MONTURA Y CRISTAL PROPIO
        BEGIN

            IF (@TIPOVENTA = '002') -- VENTA CONVENCIONAL           
            BEGIN

--Con la compra de cualquier montura o lente de sol (formulado o venta directa) de la lista de Precio de colores desde los rangos 
--“A1” hasta la letra “N”: A1, G, H, I, J, K, L, M, N, más cualquier cristal con o sin servicio AR, se le otorga 40% de descuento en la montura.

                IF (@LETRAMLS IN ('A1', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N')) 

                BEGIN

                    IF (((SUBSTRING(@MONTURA,1,1) = 'M') OR (SUBSTRING(@MONTURA,1,1) = 'L')) AND (SUBSTRING(@CRISTAL,1,1) <> ''))

                    BEGIN
                        SET @RESULTADO = 'APLICA'
                        SET @PRECIOMONT_20 = @PRECIOMONT - ((@PRECIOMONT * 40) / 100)
                        SET @PRECIOSERV_20 = @PRECIOSERV 
                        SET @PRECIOCRT_20 = @PRECIOCRT 
                        SET @PORCDCTO = 40
                    END

                END


--Con la compra de cualquier montura o lente de sol (formulado o venta directa) de la lista de Precio de colores desde los rangos “O” 
--hasta el número “V”: O, P, Q, R, S, T, U, V más cualquier cristal con o sin servicio AR, se le otorga 30% de descuento en la montura.

                IF (@LETRAMLS IN ('O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V')) 
                BEGIN

                    IF (((SUBSTRING(@MONTURA,1,1) = 'M') OR (SUBSTRING(@MONTURA,1,1) = 'L')) AND (SUBSTRING(@CRISTAL,1,1) <> ''))
                    BEGIN
                        SET @RESULTADO = 'APLICA'
                        SET @PRECIOMONT_20 = @PRECIOMONT - ((@PRECIOMONT * 30) / 100)
                        SET @PRECIOSERV_20 = @PRECIOSERV
                        SET @PRECIOCRT_20 = @PRECIOCRT 
                        SET @PORCDCTO = 30
                    END

                END


--Con la compra de cualquier montura o lente de sol (formulado o venta directa) de la lista de Precio de colores desde los rangos
--“W” hasta el número “12”: W, X, Y, Z, 1, 2, 3, 4, 5, 6,7, 8, 9, 10, 11, 12, más cualquier cristal con o sin servicio AR, 
--se le otorga 20% de descuento en la compra total.

                IF (@LETRAMLS IN ('W', 'X', 'Y', 'Z', '1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12')) 

                BEGIN

                    IF (((SUBSTRING(@MONTURA,1,1) = 'M') OR (SUBSTRING(@MONTURA,1,1) = 'L')) AND (SUBSTRING(@CRISTAL,1,1) <> ''))

                    BEGIN
                        SET @RESULTADO = 'APLICA'
                        SET @PRECIOMONT_20 = @PRECIOMONT - ((@PRECIOMONT * 20) / 100)
                        SET @PRECIOSERV_20 = @PRECIOSERV 
                        SET @PRECIOCRT_20 = @PRECIOCRT 
                        SET @PORCDCTO = 20
                    END

                END


--Con la compra de cualquier montura o lente de sol (formulado o venta directa) de la lista de Precio de Outlet desde 
--“AR” hasta el número “NR”:AR, BR, CR, ZR, DR, ER, FR, GR, HR, IR, JR, KR, LR, MR, NR, más cualquier cristal (con o sin servicio AR), 
--se le otorga 60% de descuento en la montura.

                IF (@LETRAMLS IN ('AR', 'BR', 'CR', 'DR', 'ER', 'FR', 'GR', 'HR', 'IR', 'JR', 'KR', 'LR', 'MR', 'NR', 'ZR'))
                BEGIN

                    IF (((SUBSTRING(@MONTURA,1,1) = 'M') OR (SUBSTRING(@MONTURA,1,1) = 'L')) AND (SUBSTRING(@CRISTAL,1,1) <> ''))
						BEGIN
							SET @RESULTADO = 'APLICA'
							SET @PRECIOMONT_20 = @PRECIOMONT - ((@PRECIOMONT * 60) / 100)
							SET @PRECIOSERV_20 = @PRECIOSERV
							SET @PRECIOCRT_20 = @PRECIOCRT 
							SET @PORCDCTO = 60
						END

                END


            END -- Cierra VENTA 002
        END -- Cierra MONTURA Y CRISTAL PROPIO
    END -- Cierra DESC MONTURA
END -- Cierra TipoExamen CONVENCIONAL

-------------------------------------------------------------------------------------------------------------

IF (@TipoExamen = 'DIRECTA')
BEGIN
    IF (@DESCMONT = 0) -- DESC MONTURA
    BEGIN  
        IF (@MONTURAPROPIA = 0 AND @CRISTALPROPIO = 0) -- MONTURA Y CRISTAL PROPIO
        BEGIN 
            IF (@TIPOVENTA = '001') -- VENTA DIRECTA
            BEGIN    

--Con la compra de cualquier montura o lente de sol (formulado o venta directa) de la lista de Precio de colores desde los rangos 
--“A1” hasta la letra “N”: A1, G, H, I, J, K, L, M, N, más cualquier cristal con o sin servicio AR, se le otorga 40% de descuento en la montura.

                IF (@LETRAMLS IN ('A1', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N')) 

                BEGIN

                    IF (SUBSTRING(@MONTURA,1,1) = 'L')

                    BEGIN
                        SET @RESULTADO = 'APLICA'
                        SET @PRECIOMONT_20 = @PRECIOMONT - ((@PRECIOMONT * 40) / 100)
                        SET @PRECIOSERV_20 = @PRECIOSERV 
                        SET @PRECIOCRT_20 = @PRECIOCRT 
                        SET @PORCDCTO = 40
                    END

                END




--Con la compra de cualquier montura o lente de sol (formulado o venta directa) de la lista de Precio de colores desde los rangos “O” 
--hasta el número “V”: O, P, Q, R, S, T, U, V más cualquier cristal con o sin servicio AR, se le otorga 30% de descuento en la montura.

                IF (@LETRAMLS IN ('O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V')) 
                BEGIN

                    IF (SUBSTRING(@MONTURA,1,1) = 'L')
                    BEGIN
                        SET @RESULTADO = 'APLICA'
                        SET @PRECIOMONT_20 = @PRECIOMONT - ((@PRECIOMONT * 30) / 100)
                        SET @PRECIOSERV_20 = @PRECIOSERV
                        SET @PRECIOCRT_20 = @PRECIOCRT 
                        SET @PORCDCTO = 30
                    END

                END



--Con la compra de cualquier montura o lente de sol (formulado o venta directa) de la lista de Precio de colores desde los rangos
--“W” hasta el número “12”: W, X, Y, Z, 1, 2, 3, 4, 5, 6,7, 8, 9, 10, 11, 12, más cualquier cristal con o sin servicio AR, 
--se le otorga 20% de descuento en la compra total.

                IF (@LETRAMLS IN ('W', 'X', 'Y', 'Z', '1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12')) 

                BEGIN

                    IF (SUBSTRING(@MONTURA,1,1) = 'L')

                    BEGIN
                        SET @RESULTADO = 'APLICA'
                        SET @PRECIOMONT_20 = @PRECIOMONT - ((@PRECIOMONT * 20) / 100)
                        SET @PRECIOSERV_20 = @PRECIOSERV 
                        SET @PRECIOCRT_20 = @PRECIOCRT 
                        SET @PORCDCTO = 20
                    END

                END


				
--Con la compra de cualquier montura o lente de sol (formulado o venta directa) de la lista de Precio de Outlet desde 
--“AR” hasta el número “NR”:AR, BR, CR, ZR, DR, ER, FR, GR, HR, IR, JR, KR, LR, MR, NR, más cualquier cristal (con o sin servicio AR), 
--se le otorga 60% de descuento en la montura.

                IF (@LETRAMLS IN ('AR', 'BR', 'CR', 'DR', 'ER', 'FR', 'GR', 'HR', 'IR', 'JR', 'KR', 'LR', 'MR', 'NR', 'ZR'))
                BEGIN

                    IF  (SUBSTRING(@MONTURA,1,1) = 'L')
						BEGIN
							SET @RESULTADO = 'APLICA'
							SET @PRECIOMONT_20 = @PRECIOMONT - ((@PRECIOMONT * 60) / 100)
							SET @PRECIOSERV_20 = @PRECIOSERV
							SET @PRECIOCRT_20 = @PRECIOCRT 
							SET @PORCDCTO = 60
						END

                END



            END -- VENTA 001  
        END -- MONTURA Y CRISTAL PROPIO
    END -- DESC MONTURA
END -- TipoExamen

/*******************************************************************************/

	IF @RESULTADO = 'APLICA'
	BEGIN
		SELECT @RESULTADO RESULTADO, '234' CODPROM, @MONTURA MONTURA, @LETRAMLS LETRAMLS,
		@PRECIOMONT PRECIOMONT, @PRECIOMONT_20 PRECIOMONT_DESC,@PRECIOCRT PRECIOCRT, 
		@PRECIOCRT_20 PRECIOCRT_DESC , @PORCDCTO AS PORCDCTO
	END

	ELSE

	BEGIN
		SELECT @RESULTADO RESULTADO, '234' CODPROM, @LETRAMLS LETRAMLS
	END


------------------------------------


GO

--------------------------------------------------------------------------------------------


-----------------------------------------------------------------------------




/****** Object:  StoredProcedure [dbo].[SP_CPOS_tMASTER_diasHorario]    Script Date: 05/16/2025 11:14:12 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CPOS_tMASTER_diasHorario]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_CPOS_tMASTER_diasHorario]
GO


/****** Object:  StoredProcedure [dbo].[SP_CPOS_tMASTER_diasHorario]    Script Date: 05/16/2025 11:14:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



--exec SP_CPOS_tMASTER_diasHorario '2025/05/16'
CREATE PROCEDURE [dbo].[SP_CPOS_tMASTER_diasHorario]
@fecha AS VARCHAR(25) 
AS
BEGIN

SELECT * from tMASTER_diasHorario where  fecha>=  @fecha   and  laboral= 1
order by fecha asc

end
	



GO




-------------------------------------------------------------------------------------------



/****** Object:  StoredProcedure [dbo].[SP_CPOS_TB_SUCURSALES]    Script Date: 05/16/2025 09:42:15 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CPOS_TB_SUCURSALES]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_CPOS_TB_SUCURSALES]
GO



/****** Object:  StoredProcedure [dbo].[SP_CPOS_TB_SUCURSALES]    Script Date: 05/16/2025 09:42:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SP_CPOS_TB_SUCURSALES]
@CodSucursal AS VARCHAR(3) 
AS
BEGIN

SELECT * from TB_SUCURSALES where CodSucursal =  @CodSucursal

end
	


GO


---------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[SP_CPOS_CodServiciosLaboratorio]    Script Date: 05/16/2025 09:42:36 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CPOS_CodServiciosLaboratorio]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_CPOS_CodServiciosLaboratorio]
GO


/****** Object:  StoredProcedure [dbo].[SP_CPOS_CodServiciosLaboratorio]    Script Date: 05/16/2025 09:42:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- EXEC [dbo].[SP_CPOS_CodServiciosLaboratorio]
CREATE PROCEDURE [dbo].[SP_CPOS_CodServiciosLaboratorio]
	  @Cod_servicio AS VARCHAR(3) 
AS
BEGIN

SELECT 
	Cod_servicio as CodServ,
    Descripcion_servicio as ServicioLab,
    CodArticulo, 
    HorasEntrega
FROM 
	TB_SERVICIOSLAB

WHERE
	Status_Servicio = 'A' and Cod_servicio= @Cod_servicio 

END


GO


-------------------------------------------------------------------------------



/****** Object:  StoredProcedure [dbo].[SP_CPOS_AgregarOrdenServicio]    Script Date: 05/16/2025 09:43:00 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CPOS_AgregarOrdenServicio]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_CPOS_AgregarOrdenServicio]
GO


/****** Object:  StoredProcedure [dbo].[SP_CPOS_AgregarOrdenServicio]    Script Date: 05/16/2025 09:43:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




-- EXEC [dbo].[SP_CPOS_AgregarOrdenServicio] '008','0','002','V','','','','','','','','','','','','','','','','','',''
CREATE PROCEDURE [dbo].[SP_CPOS_AgregarOrdenServicio]
    @Cod_Sucursal VARCHAR(3),
    @Revision VARCHAR(2),
    @Cod_Venta VARCHAR(3),
    @CTE_Nacio VARCHAR(1),
    @CTE_CedIden VARCHAR(10),
    @NumExamen INT,
    @COD_EMPLEADO VARCHAR(5),
    @Cod_Laboratorio VARCHAR(11),
    @Cod_Servicio VARCHAR(3),
    @Vision VARCHAR(10),
    @Fec_Ofrecido DATE,
    @Hor_Ofrecido VARCHAR(13),
    @Fec_Entrega DATE = NULL,
    @Fec_Envio DATE = NULL,
    @VtaSubTotal DECIMAL(28,2),
    @VtaImpuesto DECIMAL(28,2),
    @VtaDescuento DECIMAL(28,2),
    @VtaTotal DECIMAL(28,2),
    @OrSer_Finan BIT,
    @OrSer_Status VARCHAR(3),
    @OrSer_Observ VARCHAR(100),
    @USER_Crea VARCHAR(5),
    @Fecha DATE,
    @MonturaPropia BIT,
    @Cod_DetVta VARCHAR(2),
    @Aplica BIT,
    @OTCORRESPONDIENTE NVARCHAR(7),
    @VentaAfil BIT,
    @CristalPropio BIT,
    @TipoMonturaPropia VARCHAR(20) = NULL,
    @CodMotivoReposicion VARCHAR(3) = NULL,
    @Cedula_CteAfil VARCHAR(20) = NULL,
    @Codigo_EmpAfil VARCHAR(20) = NULL,
    @Asegurada BIT = NULL,
    @Exonerada BIT = NULL,
    @MonturaEnQuorum BIT,
    @Cod_Coloracion NVARCHAR(6) = NULL,
    @Codmotivodes VARCHAR(3) = NULL

AS


BEGIN TRANSACTION
	
DECLARE @ControlErrores AS INT  
DECLARE @Error AS INT  

    -- Generar el número de orden usando el SP 'SP_CPOS_CrearNumeroOrdenServicio'
    --EXEC SP_CPOS_CrearNumeroOrdenServicio @Cod_Sucursal = @Cod_Sucursal, @NuevoNumeroOrden = @NumOrdserv OUTPUT;
	DECLARE @NuevoNumOrdserv NVARCHAR(7);

        -- Buscar el último número de orden para la sucursal
        SELECT @NuevoNumOrdserv = 
            RIGHT('0000000' + CAST(ISNULL(MAX(CAST(NumOrdserv AS INT)), 0) + 1 AS VARCHAR), 7)
        FROM TB_CAORDSER
        WHERE Cod_Sucursal = @Cod_Sucursal

DECLARE @UltimoIdTasa AS INTEGER
	DECLARE @TasaActual AS NUMERIC(28,6)
	SELECT @UltimoIdTasa = MAX(ID_TASA) FROM TB_TASA WHERE Cod_Moneda = '01'
	
	SELECT @TasaActual = Tasa  FROM TB_TASA WHERE ID_Tasa = @UltimoIdTasa
	declare @OrSer_Saldo_Mon  DECIMAL(28,6)
	set @OrSer_Saldo_Mon= @VtaTotal/ @TasaActual
	
    INSERT INTO 
		TB_CAORDSER 
	(
        Cod_Sucursal, NumOrdserv, Revision, Cod_Venta, CTE_Nacio, CTE_CedIden, NumExamen, COD_EMPLEADO, Cod_Laboratorio, 
		Cod_Servicio, Vision, Fec_ofrecido, Hor_ofrecido, Fec_Entrega, Fec_Envio, VtaSubTotal, VtaImpuesto, VtaDescuento, 
		VtaTotal, OrSer_Saldo, OrSer_Finan, OrSer_Status, OrSer_Observ, USER_Crea, Fecha, MonturaPropia, Cod_DetVta, Aplica, 
		OTCORRESPONDIENTE, VentaAfil, CristalPropio, TipoMonturaPropia, CodMotivoReposicion, Cedula_CteAfil, Codigo_EmpAfil,
        Asegurada, Exonerada, MonturaEnQuorum, Cod_Coloracion,
        OrSer_Saldo_Mon, OrSer_Tipo_Mon, Orser_Total_Mon, Codmotivodes

    )

    VALUES 
	(
        @Cod_Sucursal, @NuevoNumOrdserv, @Revision, @Cod_Venta, @CTE_Nacio, @CTE_CedIden, @NumExamen, @COD_EMPLEADO, @Cod_Laboratorio,
		@Cod_Servicio, @Vision, @Fec_Ofrecido, @Hor_Ofrecido, @Fec_Entrega, @Fec_Envio, @VtaSubTotal, @VtaImpuesto, @VtaDescuento, 
		@VtaTotal, @VtaTotal, @OrSer_Finan, @OrSer_Status, @OrSer_Observ, @USER_Crea, @Fecha, @MonturaPropia, @Cod_DetVta, @Aplica, 
		@OTCORRESPONDIENTE, @VentaAfil, @CristalPropio, @TipoMonturaPropia, @CodMotivoReposicion, @Cedula_CteAfil, @Codigo_EmpAfil,
        @Asegurada, @Exonerada, @MonturaEnQuorum, @Cod_Coloracion,
        @OrSer_Saldo_Mon,'01',@OrSer_Saldo_Mon,@Codmotivodes
    )


IF @@ERROR <> 0  
    SET @ControlErrores = @ControlErrores + 1  


IF @ControlErrores <> 0  
BEGIN  
	ROLLBACK TRAN  
	SELECT 'FALLIDO' AS resultado
END 
ELSE  
BEGIN  
	COMMIT TRAN   
	SELECT @NuevoNumOrdserv AS resultado
END 




GO

-------------------------------------------------------------




/****** Object:  StoredProcedure [dbo].[pGetExamenconPrisma]    Script Date: 05/16/2025 16:40:33 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[pGetExamenconPrisma]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[pGetExamenconPrisma]
GO



/****** Object:  StoredProcedure [dbo].[pGetExamenconPrisma]    Script Date: 05/16/2025 16:40:34 ******/
SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER ON
GO



/* Realizado por Jacqueline Reina
    Fecha de Creación: 28/04/2015
    Este stored obtiene el prisma del examen
 */

--[pGetExamenconPrisma]'2','v','0052468'

CREATE PROCEDURE [dbo].[pGetExamenconPrisma]
 @numExamen  AS INTEGER
, @cteNacio  AS VARCHAR(1)
, @cteCedula  AS VARCHAR(9)
AS

BEGIN
	SELECT    PRISMAD, PRISMAI, PRISMAD2, PRISMAI2
	FROM         TB_FICCONV
	WHERE     (CTE_CedIden = @cteCedula) AND (NUM_Examen = @numExamen) AND (CTE_Nacio = @cteNacio)
END



GO

---------------------------------------------------------------------



/****** Object:  StoredProcedure [dbo].[SP_CPOS_tMASTER_diasHorario]    Script Date: 05/19/2025 13:14:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CPOS_tMASTER_diasHorario]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_CPOS_tMASTER_diasHorario]
GO


/****** Object:  StoredProcedure [dbo].[SP_CPOS_tMASTER_diasHorario]    Script Date: 05/19/2025 13:14:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--exec SP_CPOS_tMASTER_diasHorario '2025/10/10'
--exec SP_CPOS_tMASTER_diasHorario '2025/05/16'
--exec SP_CPOS_tMASTER_diasHorario '2025/05/16'
--exec SP_CPOS_tMASTER_diasHorario '2025/05/10'
CREATE PROCEDURE [dbo].[SP_CPOS_tMASTER_diasHorario]
@fecha AS VARCHAR(25) 
AS
BEGIN

SELECT fecha from tMASTER_diasHorario where  fecha>=  @fecha   and  laboral= 1
--order by fecha asc

end
	



GO


---------------------------------------------------------------------
/****** Object:  StoredProcedure [dbo].[SP_CPOS_ObtenerCliente]    Script Date: 05/20/2025 21:16:28 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CPOS_ObtenerCliente]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_CPOS_ObtenerCliente]
GO


/****** Object:  StoredProcedure [dbo].[SP_CPOS_ObtenerCliente]    Script Date: 05/20/2025 21:16:28 ******/
SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER OFF
GO






--[SP_CPOS_ObtenerCliente]'v','11550487'
CREATE PROCEDURE [dbo].[SP_CPOS_ObtenerCliente]
 @nacio as varchar(1),
 @cedula as varchar(20)

AS
SET NOCOUNT ON


select CTE_PNombre + ' ' + CTE_PApellido Nombre from tb_cteppal where CTE_Nacio = @nacio and CTE_CedIden = @cedula



GO


----------------------------------------------------------------------
INSERT INTO TB_PARAMETRO (Parametro, Valor, Descripcion, Fecha_Modif)
SELECT Parametro, Valor, Descripcion, Fecha_Modif
FROM (
    VALUES
        ('DiasAddEntrega', '1', 'Indica la cantidad de dias adicionales de entrega a un servicio vendido despues de la hora establecida en HoraMaxVtaServ', GETDATE()),
        ('Hora2MaxVtaServ', '14:00:00', 'Indica cual es la hora maxima de venta para un servicio un dia sabado o domingo, si sobrepasa esta hora se le agrega la cantidad de dias en DiasAddEntrega en formato militar', GETDATE()),
        ('HoraMaxVtaServ', '17:00:00', 'Indica cual es la hora maxima de venta para un servicio, si sobrepasa esta hora se le agrega la cantidad de dias en DiasAddEntrega en formato militar', GETDATE())
) AS nuevos(Parametro, Valor, Descripcion, Fecha_Modif)
WHERE NOT EXISTS (
    SELECT 1
    FROM TB_PARAMETRO
    WHERE TB_PARAMETRO.Parametro = nuevos.Parametro
);



-----------------------------------------------------------------------------------------




-------------------------------------------------------------------------------------


/****** Object:  StoredProcedure [dbo].[CPOS_Promo_Monturas_Outlet_2025]    Script Date: 05/29/2025 12:48:00 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CPOS_Promo_Monturas_Outlet_2025]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CPOS_Promo_Monturas_Outlet_2025]
GO



/****** Object:  StoredProcedure [dbo].[CPOS_Promo_Monturas_Outlet_2025]    Script Date: 05/29/2025 12:48:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






-- EXEC Promo_Monturas_Outlet_2025 'L071954', 'c000013','',0,0,'','','002','CONVENCIONAL'
-- EXEC Promo_Monturas_Outlet_2025 'L070425', '','',0,0,'','','001','DIRECTA'
-- EXEC Promo_Monturas_Outlet_2025 'L071954', '','',0,0,'','','001','DIRECTA'
CREATE PROCEDURE [dbo].[CPOS_Promo_Monturas_Outlet_2025]
	  @MONTURA AS VARCHAR(7) 
	, @CRISTAL AS VARCHAR(7)  
	, @LC AS VARCHAR(7)  
	, @MONTURAPROPIA AS BIT
	, @CRISTALPROPIO AS BIT
	, @AR AS VARCHAR(7)
	, @SERVAR AS BIT
	, @TIPOVENTA AS VARCHAR(3) -- "001 Venta Directa" o "002 CONVENCIONAL"
	, @TipoExamen AS VARCHAR(13)
AS 
 
    DECLARE @LETRAMLS AS VARCHAR (3) -- MONTURA  
    -- DESCUENTO --
	DECLARE @DESCMONT AS INTEGER -- MONTURA  
	-- MARCA --
	DECLARE @MARCA AS VARCHAR (20) -- MONTURA  
    -- PRECIO --
	DECLARE @PRECIOMONT AS NUMERIC (18,2) -- MONTURA  
	DECLARE @PRECIOCRT AS NUMERIC(28,2)-- CRISTAL
	DECLARE @PRECIOLC AS NUMERIC(28,2)-- CRISTAL
	DECLARE @PRECIOANT AS NUMERIC(28,2)  -- ANTIREFLEJO CLEAR	
	-- PORCENTAJE --
	DECLARE @PORCMONT AS NUMERIC(28,2) -- MONTURA 
	DECLARE @PORCCRT AS NUMERIC(28,2)-- CRISTAL
	DECLARE @PORCLC AS NUMERIC(28,2)-- CRISTAL
	DECLARE @PORCANT AS NUMERIC(28,2)  -- ANTIREFLEJO CLEAR
	-- CALCULO --
	DECLARE @CALCMONT AS NUMERIC(28,2) -- MONTURA 
	DECLARE @CALCCRT AS NUMERIC(28,2)-- CRISTAL
	DECLARE @CALCLC AS NUMERIC(28,2)-- CRISTAL
	DECLARE @CALCANT AS NUMERIC(28,2)  -- ANTIREFLEJO CLEAR
	-- TOTAL --
	DECLARE @TOTMONT AS NUMERIC(28,2) -- MONTURA 
	DECLARE @TOTCRT AS NUMERIC(28,2)-- CRISTAL
	DECLARE @TOTLC AS NUMERIC(28,2)-- CRISTAL
	DECLARE @TOTANT AS NUMERIC(28,2)  -- ANTIREFLEJO CLEAR
	-- APLICA --
	DECLARE @APLICA AS VARCHAR (11)  
 	DECLARE @APLICAMONT AS VARCHAR (11)  
 	DECLARE @APLICACRT AS VARCHAR (11) 
 	DECLARE @APLICALC AS VARCHAR (11) 
 	DECLARE @APLICAANT AS VARCHAR (11) 
 	
 	-- PARAMETROS --
 	DECLARE @FECHAINIPROMBF AS VARCHAR (11)  
 	


/*******************************************************************************************************/
 
	-- APLICA --
	SET @APLICA = 'NO APLICA'
	SET @APLICAMONT = 'NO APLICA'
 	SET @APLICACRT = 'NO APLICA'
	SET @APLICALC = 'NO APLICA'
 	SET @APLICAANT = 'NO APLICA'
	-- PRECIOS --
	SET @PRECIOMONT = 0
	SET @PRECIOCRT = 0
	SET @PRECIOLC = 0
	SET @PRECIOANT= 0
	-- PORCENTAJES --
 	SET @PORCMONT = 0
	SET @PORCCRT = 0 
	SET @PORCLC = 0 
	SET @PORCANT = 0 
	-- CALCULO --
 	SET @CALCMONT = 0
	SET @CALCCRT = 0 
	SET @CALCLC = 0 
	SET @CALCANT = 0
	-- TOTAL --
 	SET @TOTMONT = 0
	SET @TOTCRT = 0 
	SET @TOTLC = 0 
	SET @TOTANT = 0  

	--PRB
  
	SELECT @FECHAINIPROMBF = VALOR FROM TB_PARAMETRO WHERE PARAMETRO = 'FECHAINIPROMBF'
	
/*******************************************************************************************************/
/*    PRECIOS DE MONTURA, CRISTAL Y ANTIREFLEJOS                         */
/*******************************************************************************************************/

	-- DESCUENTO, LETRA Y PRECIO DE LA MONTURA 
		SELECT	@LETRAMLS=CodRango ,  
				@PRECIOMONT = ART_PVP,
				@DESCMONT = PorctDescuento,
				@MARCA = MARCA
		FROM TB_ARTICULO 
		WHERE CodArticulo = @MONTURA
					
	-- PRECIO DEL CRISTAL
		SELECT @PRECIOCRT = art_pvp
		FROM TB_ARTICULO 
		WHERE CodArticulo = @CRISTAL

	-- PRECIO DEL LENTE DE CONTACTO
		SELECT @PRECIOLC= art_pvp
		FROM TB_ARTICULO 
		WHERE CodArticulo = @LC
		
	-- PRECIO DEL ANTIREFLEJO
		SELECT @PRECIOANT = art_pvp
		FROM TB_ARTICULO 
		WHERE CodArticulo = @AR

DECLARE @MARCA_LENTE AS VARCHAR (4)
SET @MARCA_LENTE=(SELECT  isnull(REPLACE (MARCA, ' ', ''),'')  FROM TB_ARTICULO WHERE CodArticulo = @MONTURA)	

IF (@TipoExamen = 'CONVENCIONAL')
BEGIN
	IF (@DESCMONT = 0) -- DESC MONTURA
	BEGIN 
		IF (@MONTURAPROPIA = 0 AND @CRISTALPROPIO = 0) 
		BEGIN -- MONTURA Y CRISTAL PROPIO
		
			IF (@TIPOVENTA = '002')-- VENTA CONVENCIONAL 
			BEGIN 	
				
				
--Con la compra de cualquier montura de la lista de precio Outlet desde “AR” hasta el “NR”: 
--AR, BR, CR, ZR, DR, ER, FR, GR, HR, IR, JR, KR, LR, MR, NR, mas cualquier cristal (con o sin servicio AR), se le otorga 30% de descuento a la montura.

			 IF ((@LETRAMLS = 'AR') OR (@LETRAMLS = 'BR') OR (@LETRAMLS = 'CR') OR (@LETRAMLS = 'DR') OR (@LETRAMLS = 'ER') OR 
				 (@LETRAMLS = 'FR') OR (@LETRAMLS = 'HR') OR (@LETRAMLS = 'IR') OR (@LETRAMLS = 'JR') OR (@LETRAMLS = 'KR') OR 
				 (@LETRAMLS = 'GR') OR (@LETRAMLS = 'LR') OR (@LETRAMLS = 'MR') OR (@LETRAMLS = 'NR')OR (@LETRAMLS = 'ZR'))					   
				BEGIN   

						-- MONTURAS
						IF ( SUBSTRING(@MONTURA,1,1) = 'M')
						BEGIN 
						
							SET @PORCMONT = 30   
							SET @CALCMONT = (@PRECIOMONT * (@PORCMONT/100))
							SET @TOTMONT = @PRECIOMONT - @CALCMONT
							 
							SET @APLICAMONT = 'APLICA' 
							SET @APLICA = 'APLICA'
						END
						
						-- LENTES DE SOL
						IF ( SUBSTRING(@MONTURA,1,1) = 'L')
						BEGIN 
							SET @PORCMONT = 30   
							SET @CALCMONT = (@PRECIOMONT * (@PORCMONT/100))
							SET @TOTMONT = @PRECIOMONT - @CALCMONT
							
							SET @APLICAMONT = 'APLICA'
							SET @APLICA = 'APLICA' 
						END
						
						-- CRISTAL - sin Descuento
						IF @CRISTAL <> '' 
						BEGIN 
							SET @PORCCRT = 0
							SET @CALCCRT = (@PRECIOCRT * (@PORCCRT/100))
							SET @TOTCRT = @PRECIOCRT - @CALCCRT
							
							SET @APLICACRT = 'APLICA'
							SET @APLICA = 'APLICA'
							
						END 	
						
						-- ANTIREFLEJO - Sin Descuento 
						IF @AR <> '' 
						BEGIN 
							SET @PORCANT = 0
							SET @CALCANT = (@PRECIOANT * (@PORCANT/100))
							SET @TOTANT = @PRECIOANT - @CALCANT
							
							SET @APLICAANT = ' APLICA'
							SET @APLICA = 'APLICA'
					     END 	
			 END -- 3.
			 
			 
		  else 
          BEGIN 
          

							SET @TOTMONT = @PRECIOMONT 
							
							SET @PORCCRT = 0
							SET @CALCCRT = (@PRECIOCRT * (@PORCCRT/100))
							SET @TOTCRT = @PRECIOCRT - @CALCCRT
							
							SET @PORCANT = 0
							SET @CALCANT = (@PRECIOANT * (@PORCANT/100))
							SET @TOTANT = @PRECIOANT - @CALCANT
							
							SET @APLICAMONT = 'APLICA'
							SET @APLICA = 'APLICA'
							SET @PORCCRT = 0 -- PARA DESCUENTOS
							
	      END

			
END-- VENTA 002 
END -- MONTURA Y CRISTAL PROPIO
END -- DESCUENTO MONTURA
END--------TipoExamen----------
  




IF (@TipoExamen = 'DIRECTA')
BEGIN
	IF (@DESCMONT = 0) -- DESC MONTURA
	BEGIN  
			IF (@TIPOVENTA = '001')-- VENTA DIRECTA 
			BEGIN 	

			
			--Con la compra de cualquier lente de sol de la lista de precio Outlet desde “AR” hasta “NR”: 
			--AR, BR, CR, ZR, DR, ER, FR, GR, HR, IR, JR, KR, LR, MR, NR, más cualquier cristal (con o sin servicio AR), se le otorga 30% de descuento al lente de sol.
			
			 IF ((@LETRAMLS = 'AR') OR (@LETRAMLS = 'BR') OR (@LETRAMLS = 'CR') OR (@LETRAMLS = 'DR') OR (@LETRAMLS = 'ER') OR 
				 (@LETRAMLS = 'FR') OR (@LETRAMLS = 'HR') OR (@LETRAMLS = 'IR') OR (@LETRAMLS = 'JR') OR (@LETRAMLS = 'KR') OR 
				 (@LETRAMLS = 'GR') OR (@LETRAMLS = 'LR') OR (@LETRAMLS = 'MR') OR (@LETRAMLS = 'NR')OR (@LETRAMLS = 'ZR'))					   
			BEGIN 
						-- LENTES DE SOL
						IF (( SUBSTRING(@MONTURA,1,1) = 'L') or ( SUBSTRING(@MONTURA,1,1) = 'M'))
						BEGIN 
						
							SET @PORCMONT = 30 
							SET @CALCMONT = (@PRECIOMONT * (@PORCMONT/100))
							SET @TOTMONT = @PRECIOMONT - @CALCMONT
							
							SET @APLICAMONT = 'APLICA'
							SET @APLICA = 'APLICA'
							SET @PORCCRT = 0 -- PARA DESCUENTOS
						END					
	
			    END -- 2.
          else 
          BEGIN 
          

							SET @TOTMONT = @PRECIOMONT 
							SET @APLICAMONT = 'APLICA'
							SET @APLICA = 'APLICA'
							SET @PORCCRT = 0 -- PARA DESCUENTOS
							
	      END
				
		END -- VENTA 001  
END
END 
/*******************************************************************************/

 --SELECT @APLICAMONT Resultado,'160' CODPROM, @MONTURA MONTURA, @PRECIOMONT PRECIOMONT, @PORCMONT PORCMONT, @CALCMONT CALCMONT, @TOTMONT TOTMONT, @LETRAMLS LETRA, @DESCMONT DESCMONT
 --SELECT @APLICACRT Resultado,'160' CODPROM, @CRISTAL CRISTAL, @PRECIOCRT PRECIOCRT, @PORCCRT PORCCRT, @CALCCRT CALCCRT, @TOTCRT TOTCRT
 --SELECT @APLICALC Resultado,'160' CODPROM, @LC LENTECONT, @PRECIOLC PRECIOLC, @PORCLC PORCLC, @CALCLC CALCLC, @TOTLC TOTLC
 --SELECT @APLICAANT Resultado,'160' CODPROM, @AR ANT, @PRECIOANT PRECIOANT, @PORCANT PORCANT, @CALCANT CALCANT, @TOTANT TOTANT
	
	 IF @APLICA= 'APLICA'
	BEGIN
	--	SELECT @RESULTADO RESULTADO, '229' CODPROM, @MONTURA MONTURA, @LETRAMLS LETRAMLS,
	--	@PRECIOMONT PRECIOMONT, @PRECIOMONT_20 PRECIOMONT_DESC,@PRECIOCRT PRECIOCRT, 
	--	@PRECIOCRT_20 PRECIOCRT_DESC , @PORCDCTO AS PORCDCTO
	--END
	
	 SELECT
        @APLICA AS RESULTADO,
        '224' AS CODPROM,
        @LETRAMLS AS LETRAMLS,
        @PRECIOMONT AS PRECIOMONT,
        @TOTMONT AS PRECIOMONT_DESC, -- Descuento numérico a Monturas
        @PRECIOCRT AS PRECIOCRT,
        @TOTCRT AS PRECIOCRT_DESC   -- Descuento numérico a Cristales
END

	ELSE

	BEGIN
		SELECT @APLICA RESULTADO, '224' CODPROM, @LETRAMLS LETRAMLS
	END
















GO


---------------------------------------------------------------------------------------------



/****** Object:  StoredProcedure [dbo].[CPOS_Promo_Especial_Monturas_Outlet]    Script Date: 05/29/2025 12:48:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CPOS_Promo_Especial_Monturas_Outlet]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CPOS_Promo_Especial_Monturas_Outlet]
GO



/****** Object:  StoredProcedure [dbo].[CPOS_Promo_Especial_Monturas_Outlet]    Script Date: 05/29/2025 12:48:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






-- EXEC Promo_Especial_Monturas_Outlet 'M149397', 'C000001','',0,0,'','','002','CONVENCIONAL'
-- EXEC Promo_Especial_Monturas_Outlet 'M149397', '','',0,0,'','','002','CONVENCIONAL'
CREATE PROCEDURE [dbo].[CPOS_Promo_Especial_Monturas_Outlet]
	  @MONTURA AS VARCHAR(7) 
	, @CRISTAL AS VARCHAR(7)  
	, @LC AS VARCHAR(7)  
	, @MONTURAPROPIA AS BIT
	, @CRISTALPROPIO AS BIT
	, @SERVICIO AS VARCHAR(7)
	, @SERVAR AS BIT
	, @TIPOVENTA AS VARCHAR(3) -- "001 Venta Directa" o "002 CONVENCIONAL"
	, @TipoExamen AS VARCHAR(13)
	
AS
-- MONTURA    
DECLARE @PRECIOMONT AS NUMERIC (18,2) 
DECLARE @LETRAMLS AS VARCHAR (3) 
DECLARE @PRECIOMONT_20 AS NUMERIC(28,2)
-- CRISTAL
DECLARE @PRECIOCRT AS NUMERIC(28,2)
DECLARE @PRECIOCRT_20 AS NUMERIC(28,2)
--SERVICIO
DECLARE @PRECIOSERV AS NUMERIC(28,2)
DECLARE @PRECIOSERV_20 AS NUMERIC(28,2)

Declare @PORCDCTO AS INT 
DECLARE @PARTE AS INT
DECLARE @RESULTADO AS VARCHAR(15)
-- TOTAL --
DECLARE @TOTMONT AS NUMERIC(28,2) -- MONTURA 
DECLARE @TOTCRT AS NUMERIC(28,2)-- CRISTAL
DECLARE @TOTLC AS NUMERIC(28,2)-- CRISTAL
DECLARE @TOTANT AS NUMERIC(28,2)  -- ANTIREFLEJO CLEAR

SET @RESULTADO = 'NO APLICA'

	-- LETRA DE LA MONTURA 
	SELECT @LETRAMLS=CodRango , @PRECIOMONT = ART_PVP 
	FROM TB_ARTICULO 
	WHERE CodArticulo = @MONTURA
	

	-- PRECIO DE ANTIREFLEJO CLEAR Y DESCUENTO DEL 50 %
	SELECT @PRECIOSERV = Art_PVP
	FROM TB_ARTICULO 
	WHERE CodArticulo = @SERVICIO
						
	-- PRECIO DEL CRISTAL
	SELECT @PRECIOCRT = art_pvp
	FROM TB_ARTICULO 
	WHERE CodArticulo = @CRISTAL
	
Declare @Cristales_2 as varchar (7)
set @Cristales_2= ''
Declare @Pertenece_Marca AS bit

	SELECT @Pertenece_Marca = marca, @Cristales_2=Codigo
	FROM  Cristales_Rodenstock_RLX_Oculus2 
	WHERE Codigo = @CRISTAL
	
	
DECLARE @DESCMONT AS INTEGER -- MONTURA  	
	SELECT	@DESCMONT = PorctDescuento
		FROM TB_ARTICULO 
		WHERE CodArticulo = @MONTURA
		

IF (@TipoExamen = 'CONVENCIONAL')

BEGIN
	IF (@DESCMONT = 0) -- DESC MONTURA

	BEGIN 
		IF (@MONTURAPROPIA = 0 AND @CRISTALPROPIO = 0) 

		BEGIN -- MONTURA Y CRISTAL PROPIO
			IF (@TIPOVENTA = '002')-- VENTA CONVENCIONAL 


--Con la compra de cualquier montura o lente de sol de la lista de precios de Outlet desde “IR” hasta el “NR”: IR, JR, KR, LR, MR, NR 
-- mas cualquier cristal (con o sin servicio AR), se le otorga 30% de descuento en la compra total.

			BEGIN 	
			 IF ((@LETRAMLS = 'IR') OR (@LETRAMLS = 'JR') OR (@LETRAMLS = 'KR') 
			  OR (@LETRAMLS = 'LR') OR (@LETRAMLS = 'MR') OR (@LETRAMLS = 'NR'))	
			  
				BEGIN   
						IF (((SUBSTRING(@MONTURA,1,1) = 'M') OR ( SUBSTRING(@MONTURA,1,1) = 'L')) AND (SUBSTRING(@CRISTAL,1,1) <> ''))

						BEGIN

							SET @RESULTADO = 'APLICA'
							SET @PRECIOMONT_20		= @PRECIOMONT -((@PRECIOMONT * 30)/100)
							SET @PRECIOSERV_20		= @PRECIOSERV -((@PRECIOSERV * 30)/100)
							SET @PRECIOCRT_20       = @PRECIOCRT -((@PRECIOCRT * 30)/100)

							SET @PORCDCTO= 30

						END


END-- VENTA 002 
END -- MONTURA Y CRISTAL PROPIO
END -- DESCUENTO MONTURA
END--------TipoExamen--------

/*******************************************************************************/

	IF @RESULTADO = 'APLICA'
	BEGIN
		
		--SELECT @RESULTADO RESULTADO, '226' CODPROM, @MONTURA MONTURA, @LETRAMLS LETRAMLS,
		--@PRECIOMONT PRECIOMONT, @PRECIOMONT_20 PRECIOMONT_DESC,@PRECIOCRT PRECIOCRT, 
		--@PRECIOCRT_20 PRECIOCRT_DESC , @PORCDCTO AS PORCDCTO
		
		 SELECT
        @RESULTADO AS RESULTADO,
        '226' AS CODPROM,
        @LETRAMLS AS LETRAMLS,
        @PRECIOMONT AS PRECIOMONT,
        @PRECIOMONT_20 AS PRECIOMONT_DESC, -- Descuento numérico a Monturas
        @PRECIOCRT AS PRECIOCRT,
        @PRECIOCRT_20 AS PRECIOCRT_DESC,   -- Descuento numérico a Cristales
        @PRECIOSERV AS PRECIOSERV,
        @PORCDCTO AS PRECIOAR_DESC, -- Descuento PORCENTUAL A Servicios AR
        @PORCDCTO AS PORCDCTO,               -- Descuento porcentual general
        @PORCDCTO AS PORC_SERVICIO_DESC      -- Descuento porcentual a servicios distintos de AR
        -- Puedes agregar más columnas aquí si lo necesitas

    -- Si necesitas devolver un segundo resultset con artículos excluidos:
    SELECT 'A000004' AS CodArticulo;
    
	END

	ELSE

	BEGIN
		SELECT @RESULTADO RESULTADO, '226' CODPROM, @LETRAMLS LETRAMLS
	END

END




GO


-----------------------------------------------------------------------------------------------------------



/****** Object:  StoredProcedure [dbo].[CPOS_Descuento_Gama_Club]    Script Date: 05/29/2025 12:49:53 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CPOS_Descuento_Gama_Club]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CPOS_Descuento_Gama_Club]
GO



/****** Object:  StoredProcedure [dbo].[CPOS_Descuento_Gama_Club]    Script Date: 05/29/2025 12:49:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[CPOS_Descuento_Gama_Club]
	  @MONTURA AS VARCHAR(7) 
	, @CRISTAL AS VARCHAR(7)  
	, @LC AS VARCHAR(7)  
	, @MONTURAPROPIA AS BIT
	, @CRISTALPROPIO AS BIT
	, @SERVICIO AS VARCHAR(7)
	, @SERVAR AS BIT
	, @TIPOVENTA AS VARCHAR(3) -- "001 Venta Directa" o "002 CONVENCIONAL"
	, @TipoExamen AS VARCHAR(13)
	
AS
-- MONTURA    
DECLARE @PRECIOMONT AS NUMERIC (18,2) 
DECLARE @LETRAMLS AS VARCHAR (3) 
DECLARE @PRECIOMONT_20 AS NUMERIC(28,2)
-- CRISTAL
DECLARE @PRECIOCRT AS NUMERIC(28,2)
DECLARE @PRECIOCRT_20 AS NUMERIC(28,2)
--SERVICIO
DECLARE @PRECIOSERV AS NUMERIC(28,2)
DECLARE @PRECIOSERV_20 AS NUMERIC(28,2)

Declare @PORCDCTO AS INT 
DECLARE @PARTE AS INT
DECLARE @RESULTADO AS VARCHAR(15)
-- TOTAL --
DECLARE @TOTMONT AS NUMERIC(28,2) -- MONTURA 
DECLARE @TOTCRT AS NUMERIC(28,2)-- CRISTAL
DECLARE @TOTLC AS NUMERIC(28,2)-- CRISTAL
DECLARE @TOTANT AS NUMERIC(28,2)  -- ANTIREFLEJO CLEAR

SET @RESULTADO = 'NO APLICA'

	-- LETRA DE LA MONTURA 
	SELECT @LETRAMLS=CodRango , @PRECIOMONT = ART_PVP 
	FROM TB_ARTICULO 
	WHERE CodArticulo = @MONTURA
	

	-- PRECIO DE ANTIREFLEJO CLEAR Y DESCUENTO DEL 50 %
	SELECT @PRECIOSERV = Art_PVP
	FROM TB_ARTICULO 
	WHERE CodArticulo = @SERVICIO
						
	-- PRECIO DEL CRISTAL
	SELECT @PRECIOCRT = art_pvp
	FROM TB_ARTICULO 
	WHERE CodArticulo = @CRISTAL
	
Declare @Cristales_2 as varchar (7)
set @Cristales_2= ''
Declare @Pertenece_Marca AS bit

	SELECT @Pertenece_Marca = marca, @Cristales_2=Codigo
	FROM  Cristales_Rodenstock_RLX_Oculus2 
	WHERE Codigo = @CRISTAL
	
	
DECLARE @DESCMONT AS INTEGER -- MONTURA  	
	SELECT	@DESCMONT = PorctDescuento
		FROM TB_ARTICULO 
		WHERE CodArticulo = @MONTURA

DECLARE @MARCA_LENTE AS VARCHAR (4)
SET @MARCA_LENTE= (SELECT REPLACE (MARCA, ' ', '')  FROM TB_ARTICULO WHERE CodArticulo = @MONTURA)	

IF (@TipoExamen = 'CONVENCIONAL')
BEGIN

    IF (@DESCMONT = 0) -- DESC MONTURA
    BEGIN

        IF (@MONTURAPROPIA = 0 AND @CRISTALPROPIO = 0) -- MONTURA Y CRISTAL PROPIO
        BEGIN

            IF (@TIPOVENTA = '002') -- VENTA CONVENCIONAL           
            BEGIN

--Con la compra de cualquier montura de la lista de precios de colores desde los rangos “A1” hasta “12”: 
--A1, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z, 1, 2, 3, 4, 5, 6,7, 8, 9, 10, 11, 12 
--más cualquier cristal (con o sin servicio AR) excepto la marca Cartier, se le otorga 20% de descuento en la compra total.

                IF ((@LETRAMLS IN ('A1', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O',
                                   'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 
                                   'Z', '1', '2', '3', '4', '5', '6', '7', '8', '9', 
                                   '10', '11', '12')) 
                    AND (@MARCA_LENTE <> 'CR'))

                BEGIN

                    IF (((SUBSTRING(@MONTURA,1,1) = 'M') OR (SUBSTRING(@MONTURA,1,1) = 'L')) AND (SUBSTRING(@CRISTAL,1,1) <> ''))

                    BEGIN
                        SET @RESULTADO = 'APLICA'
                        SET @PRECIOMONT_20 = @PRECIOMONT - ((@PRECIOMONT * 20) / 100)
                        SET @PRECIOSERV_20 = @PRECIOSERV - ((@PRECIOSERV * 20) / 100)
                        SET @PRECIOCRT_20 = @PRECIOCRT - ((@PRECIOCRT * 20) / 100)
                        SET @PORCDCTO = 20
                    END

                END

--Con la compra de cualquier montura de la lista de precios de Outlet desde “AR” hasta “NR”: 
--AR, BR, CR, ZR, DR, ER, FR, GR, HR, IR, JR, KR, LR, MR, NR 
--más cualquier cristal (con o sin servicio AR) excepto la marca Cartier, se le otorga 20% de descuento en la compra total. 

                IF ((@LETRAMLS IN ('AR', 'BR', 'CR', 'DR', 'ER', 'FR', 'GR', 'HR', 
                                   'IR', 'JR', 'KR', 'LR', 'MR', 'NR', 'ZR')) 
                    AND (@MARCA_LENTE <> 'CR'))
                BEGIN

                    IF (((SUBSTRING(@MONTURA,1,1) = 'M') OR (SUBSTRING(@MONTURA,1,1) = 'L')) AND (SUBSTRING(@CRISTAL,1,1) <> ''))
                    BEGIN
                        SET @RESULTADO = 'APLICA'
                        SET @PRECIOMONT_20 = @PRECIOMONT - ((@PRECIOMONT * 20) / 100)
                        SET @PRECIOSERV_20 = @PRECIOSERV - ((@PRECIOSERV * 20) / 100)
                        SET @PRECIOCRT_20 = @PRECIOCRT - ((@PRECIOCRT * 20) / 100)
                        SET @PORCDCTO = 20
                    END

                END



            END -- Cierra VENTA 002
        END -- Cierra MONTURA Y CRISTAL PROPIO
    END -- Cierra DESC MONTURA
END -- Cierra TipoExamen CONVENCIONAL

-------------------------------------------------------------------------------------------------------------

IF (@TipoExamen = 'DIRECTA')
BEGIN
    IF (@DESCMONT = 0) -- DESC MONTURA
    BEGIN  
        IF (@MONTURAPROPIA = 0 AND @CRISTALPROPIO = 0) -- MONTURA Y CRISTAL PROPIO
        BEGIN 
            IF (@TIPOVENTA = '001') -- VENTA DIRECTA
            BEGIN    

--Con la compra de cualquier lente de sol de la lista de precios de colores desde los rangos “A1” hasta “12”: 
--A1, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z, 1, 2, 3, 4, 5, 6,7, 8, 9, 10, 11, 12 
--en venta directa o con cristales formulados excepto la marca Cartier, se le otorga 20% de descuento en la compra total.

                IF (@LETRAMLS IN ('A1', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 
                                 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '1', '2', '3', '4', '5', '6', 
                                 '7', '8', '9', '10', '11', '12') 
                    AND @MARCA_LENTE <> 'CR') 
                BEGIN   

                    IF (((SUBSTRING(@MONTURA,1,1) = 'M') OR (SUBSTRING(@MONTURA,1,1) = 'L')))
                    BEGIN
                        SET @RESULTADO = 'APLICA'
                        SET @PRECIOMONT_20 = @PRECIOMONT - ((@PRECIOMONT * 20) / 100)
                        SET @PRECIOSERV_20 = @PRECIOSERV - ((@PRECIOSERV * 20) / 100)
                        SET @PRECIOCRT_20  = @PRECIOCRT  - ((@PRECIOCRT  * 20) / 100)
                        SET @PORCDCTO = 20
                    END

                END

--Con la compra de cualquier Lente de Sol de la lista de precios de Outlet desde ““AR” hasta “NR”: 
--AR, BR, CR, ZR, DR, ER, FR, GR, HR, IR, JR, KR, LR, MR, NR en venta directa o con cristales formulados excepto la marca Cartier, 
--se le otorga 20% de descuento en la compra total.

                IF (@LETRAMLS IN ('AR', 'BR', 'CR', 'DR', 'ER', 'FR', 'GR', 'HR', 
                                 'IR', 'JR', 'KR', 'LR', 'MR', 'NR', 'ZR') 
                    AND @MARCA_LENTE <> 'CR')  
                BEGIN

                    IF (((SUBSTRING(@MONTURA,1,1) = 'M') OR (SUBSTRING(@MONTURA,1,1) = 'L')))
                    BEGIN
                        SET @RESULTADO = 'APLICA'
                        SET @PRECIOMONT_20 = @PRECIOMONT - ((@PRECIOMONT * 20) / 100)
                        SET @PRECIOSERV_20 = @PRECIOSERV - ((@PRECIOSERV * 20) / 100)
                        SET @PRECIOCRT_20  = @PRECIOCRT  - ((@PRECIOCRT  * 20) / 100)
                        SET @PORCDCTO = 20
                    END

                END




            END -- VENTA 001  
        END -- MONTURA Y CRISTAL PROPIO
    END -- DESC MONTURA
END -- TipoExamen

/*******************************************************************************/

	IF @RESULTADO = 'APLICA'
	BEGIN
	--	SELECT @RESULTADO RESULTADO, '227' CODPROM, @MONTURA MONTURA, @LETRAMLS LETRAMLS,
	--	@PRECIOMONT PRECIOMONT, @PRECIOMONT_20 PRECIOMONT_DESC,@PRECIOCRT PRECIOCRT, 
	--	@PRECIOCRT_20 PRECIOCRT_DESC , @PORCDCTO AS PORCDCTO
	--END
	
	 SELECT
        @RESULTADO AS RESULTADO,
        '227' AS CODPROM,
        @LETRAMLS AS LETRAMLS,
        @PRECIOMONT AS PRECIOMONT,
        @PRECIOMONT_20 AS PRECIOMONT_DESC, -- Descuento numérico a Monturas
        @PRECIOCRT AS PRECIOCRT,
        @PRECIOCRT_20 AS PRECIOCRT_DESC,   -- Descuento numérico a Cristales
        @PRECIOSERV AS PRECIOSERV,
        @PORCDCTO AS PRECIOAR_DESC, -- Descuento PORCENTUAL A Servicios AR
        @PORCDCTO AS PORCDCTO,               -- Descuento porcentual general
        @PORCDCTO AS PORC_SERVICIO_DESC      -- Descuento porcentual a servicios distintos de AR
        -- Puedes agregar más columnas aquí si lo necesitas

    -- Si necesitas devolver un segundo resultset con artículos excluidos:
    SELECT 'A000004' AS CodArticulo;
END

	ELSE

	BEGIN
		SELECT @RESULTADO RESULTADO, '227' CODPROM, @LETRAMLS LETRAMLS
	END




GO


-----------------------------------------------------------------------------------------------------



/****** Object:  StoredProcedure [dbo].[CPOS_Descuento_Seguros_Mercantil]    Script Date: 05/29/2025 12:50:41 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CPOS_Descuento_Seguros_Mercantil]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CPOS_Descuento_Seguros_Mercantil]
GO


/****** Object:  StoredProcedure [dbo].[CPOS_Descuento_Seguros_Mercantil]    Script Date: 05/29/2025 12:50:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[CPOS_Descuento_Seguros_Mercantil]

	  @MONTURA AS VARCHAR(7) 
	, @CRISTAL AS VARCHAR(7)  
	, @LC AS VARCHAR(7)  
	, @MONTURAPROPIA AS BIT
	, @CRISTALPROPIO AS BIT
	, @SERVICIO AS VARCHAR(7)
	, @SERVAR AS BIT
	, @TIPOVENTA AS VARCHAR(3) -- "001 Venta Directa" o "002 CONVENCIONAL"
	, @TipoExamen AS VARCHAR(13)
	
AS
-- MONTURA    
DECLARE @PRECIOMONT AS NUMERIC (18,2) 
DECLARE @LETRAMLS AS VARCHAR (3) 
DECLARE @PRECIOMONT_20 AS NUMERIC(28,2)
-- CRISTAL
DECLARE @PRECIOCRT AS NUMERIC(28,2)
DECLARE @PRECIOCRT_20 AS NUMERIC(28,2)
--SERVICIO
DECLARE @PRECIOSERV AS NUMERIC(28,2)
DECLARE @PRECIOSERV_20 AS NUMERIC(28,2)

Declare @PORCDCTO AS INT 
DECLARE @PARTE AS INT
DECLARE @RESULTADO AS VARCHAR(15)
-- TOTAL --
DECLARE @TOTMONT AS NUMERIC(28,2) -- MONTURA 
DECLARE @TOTCRT AS NUMERIC(28,2)-- CRISTAL
DECLARE @TOTLC AS NUMERIC(28,2)-- CRISTAL
DECLARE @TOTANT AS NUMERIC(28,2)  -- ANTIREFLEJO CLEAR

SET @RESULTADO = 'NO APLICA'

	-- LETRA DE LA MONTURA 
	SELECT @LETRAMLS=CodRango , @PRECIOMONT = ART_PVP 
	FROM TB_ARTICULO 
	WHERE CodArticulo = @MONTURA
	

	-- PRECIO DE ANTIREFLEJO CLEAR Y DESCUENTO DEL 50 %
	SELECT @PRECIOSERV = Art_PVP
	FROM TB_ARTICULO 
	WHERE CodArticulo = @SERVICIO
						
	-- PRECIO DEL CRISTAL
	SELECT @PRECIOCRT = art_pvp
	FROM TB_ARTICULO 
	WHERE CodArticulo = @CRISTAL
	
Declare @Cristales_2 as varchar (7)
set @Cristales_2= ''
Declare @Pertenece_Marca AS bit

	SELECT @Pertenece_Marca = marca, @Cristales_2=Codigo
	FROM  Cristales_Rodenstock_RLX_Oculus2 
	WHERE Codigo = @CRISTAL
	
	
DECLARE @DESCMONT AS INTEGER -- MONTURA  	
	SELECT	@DESCMONT = PorctDescuento
		FROM TB_ARTICULO 
		WHERE CodArticulo = @MONTURA

DECLARE @MARCA_LENTE AS VARCHAR (4)
SET @MARCA_LENTE= (SELECT REPLACE (MARCA, ' ', '')  FROM TB_ARTICULO WHERE CodArticulo = @MONTURA)	

IF (@TipoExamen = 'CONVENCIONAL')
BEGIN

    IF (@DESCMONT = 0) -- DESC MONTURA
    BEGIN

        IF (@MONTURAPROPIA = 0 AND @CRISTALPROPIO = 0) -- MONTURA Y CRISTAL PROPIO
        BEGIN

            IF (@TIPOVENTA = '002') -- VENTA CONVENCIONAL           
            BEGIN

--Con la compra de cualquier MONTURA/LENTE DE SOL de la lista de precios de colores desde los rangos “A1” hasta “12”: 
--A1, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z, 1, 2, 3, 4, 5, 6,7, 8, 9, 10, 11, 12 
--más cualquier cristal (con o sin servicio AR) excepto la marca Cartier, se le otorga 20% de descuento en la compra total.


                IF ((@LETRAMLS IN ('A1', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O',
                                   'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 
                                   'Z', '1', '2', '3', '4', '5', '6', '7', '8', '9', 
                                   '10', '11', '12')) 
                    AND (@MARCA_LENTE <> 'CR'))

                BEGIN

                    IF (((SUBSTRING(@MONTURA,1,1) = 'M') OR (SUBSTRING(@MONTURA,1,1) = 'L')) AND (SUBSTRING(@CRISTAL,1,1) <> ''))

                    BEGIN
                        SET @RESULTADO = 'APLICA'
                        SET @PRECIOMONT_20 = @PRECIOMONT - ((@PRECIOMONT * 20) / 100)
                        SET @PRECIOSERV_20 = @PRECIOSERV - ((@PRECIOSERV * 20) / 100)
                        SET @PRECIOCRT_20 = @PRECIOCRT - ((@PRECIOCRT * 20) / 100)
                        SET @PORCDCTO = 20
                    END

                END

--Con la compra de cualquier montura de la lista de precios de Outlet desde “AR” hasta “NR”: 
--AR, BR, CR, ZR, DR, ER, FR, GR, HR, IR, JR, KR, LR, MR, NR 
--más cualquier cristal (con o sin servicio AR) excepto la marca Cartier, se le otorga 20% de descuento en la compra total. 

                IF ((@LETRAMLS IN ('AR', 'BR', 'CR', 'DR', 'ER', 'FR', 'GR', 'HR', 
                                   'IR', 'JR', 'KR', 'LR', 'MR', 'NR', 'ZR')) 
                    AND (@MARCA_LENTE <> 'CR'))
                BEGIN

                    IF (((SUBSTRING(@MONTURA,1,1) = 'M') OR (SUBSTRING(@MONTURA,1,1) = 'L')) AND (SUBSTRING(@CRISTAL,1,1) <> ''))
                    BEGIN
                        SET @RESULTADO = 'APLICA'
                        SET @PRECIOMONT_20 = @PRECIOMONT - ((@PRECIOMONT * 20) / 100)
                        SET @PRECIOSERV_20 = @PRECIOSERV - ((@PRECIOSERV * 20) / 100)
                        SET @PRECIOCRT_20 = @PRECIOCRT - ((@PRECIOCRT * 20) / 100)
                        SET @PORCDCTO = 20
                    END

                END



            END -- Cierra VENTA 002
        END -- Cierra MONTURA Y CRISTAL PROPIO
    END -- Cierra DESC MONTURA
END -- Cierra TipoExamen CONVENCIONAL

-------------------------------------------------------------------------------------------------------------

IF (@TipoExamen = 'DIRECTA')
BEGIN
    IF (@DESCMONT = 0) -- DESC MONTURA
    BEGIN  
        IF (@MONTURAPROPIA = 0 AND @CRISTALPROPIO = 0) -- MONTURA Y CRISTAL PROPIO
        BEGIN 
            IF (@TIPOVENTA = '001') -- VENTA DIRECTA
            BEGIN    

--Con la compra de cualquier lente de sol de la lista de precios de colores desde los rangos “A1” hasta “12”: 
--A1, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z, 1, 2, 3, 4, 5, 6,7, 8, 9, 10, 11, 12 
--en venta directa o con cristales formulados excepto la marca Cartier, se le otorga 20% de descuento en la compra total.

                IF (@LETRAMLS IN ('A1', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 
                                 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '1', '2', '3', '4', '5', '6', 
                                 '7', '8', '9', '10', '11', '12') 
                    AND @MARCA_LENTE <> 'CR') 
                BEGIN   

                    IF (SUBSTRING(@MONTURA,1,1) = 'L')
                    BEGIN
                        SET @RESULTADO = 'APLICA'
                        SET @PRECIOMONT_20 = @PRECIOMONT - ((@PRECIOMONT * 20) / 100)
                        SET @PRECIOSERV_20 = @PRECIOSERV - ((@PRECIOSERV * 20) / 100)
                        SET @PRECIOCRT_20  = @PRECIOCRT  - ((@PRECIOCRT  * 20) / 100)
                        SET @PORCDCTO = 20
                    END

                END

--Con la compra de cualquier Lente de Sol de la lista de precios de Outlet desde ““AR” hasta “NR”: 
--AR, BR, CR, ZR, DR, ER, FR, GR, HR, IR, JR, KR, LR, MR, NR en venta directa o con cristales formulados excepto la marca Cartier, 
--se le otorga 20% de descuento en la compra total.

                IF (@LETRAMLS IN ('AR', 'BR', 'CR', 'DR', 'ER', 'FR', 'GR', 'HR', 
                                 'IR', 'JR', 'KR', 'LR', 'MR', 'NR', 'ZR') 
                    AND @MARCA_LENTE <> 'CR')  
                BEGIN

                    IF (SUBSTRING(@MONTURA,1,1) = 'L')
                    BEGIN
                        SET @RESULTADO = 'APLICA'
                        SET @PRECIOMONT_20 = @PRECIOMONT - ((@PRECIOMONT * 20) / 100)
                        SET @PRECIOSERV_20 = @PRECIOSERV - ((@PRECIOSERV * 20) / 100)
                        SET @PRECIOCRT_20  = @PRECIOCRT  - ((@PRECIOCRT  * 20) / 100)
                        SET @PORCDCTO = 20
                    END

                END




            END -- VENTA 001  
        END -- MONTURA Y CRISTAL PROPIO
    END -- DESC MONTURA
END -- TipoExamen



/*******************************************************************************/

	IF @RESULTADO = 'APLICA'
	BEGIN
	--	SELECT @RESULTADO RESULTADO, '229' CODPROM, @MONTURA MONTURA, @LETRAMLS LETRAMLS,
	--	@PRECIOMONT PRECIOMONT, @PRECIOMONT_20 PRECIOMONT_DESC,@PRECIOCRT PRECIOCRT, 
	--	@PRECIOCRT_20 PRECIOCRT_DESC , @PORCDCTO AS PORCDCTO
	--END
	
	 SELECT
        @RESULTADO AS RESULTADO,
        '229' AS CODPROM,
        @LETRAMLS AS LETRAMLS,
        @PRECIOMONT AS PRECIOMONT,
        @PRECIOMONT_20 AS PRECIOMONT_DESC, -- Descuento numérico a Monturas
        @PRECIOCRT AS PRECIOCRT,
        @PRECIOCRT_20 AS PRECIOCRT_DESC,   -- Descuento numérico a Cristales
        @PRECIOSERV AS PRECIOSERV,
        @PORCDCTO AS PRECIOAR_DESC, -- Descuento PORCENTUAL a Servicios AR
        @PORCDCTO AS PORCDCTO,               -- Descuento porcentual general
        @PORCDCTO AS PORC_SERVICIO_DESC      -- Descuento porcentual a servicios distintos de AR
        -- Puedes agregar más columnas aquí si lo necesitas

    -- Si necesitas devolver un segundo resultset con artículos excluidos:
    SELECT 'A000004' AS CodArticulo;
END

	ELSE

	BEGIN
		SELECT @RESULTADO RESULTADO, '229' CODPROM, @LETRAMLS LETRAMLS
	END




GO

-----------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[CPOS_Promo_Sambil]    Script Date: 05/23/2025 16:50:23 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CPOS_Promo_Sambil]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CPOS_Promo_Sambil]
GO


/****** Object:  StoredProcedure [dbo].[CPOS_Promo_Sambil]    Script Date: 05/23/2025 16:50:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



---exec CPOS_Promo_Sambil 'M140692', 'C000001','',0,0,'',0,'002','CONVENCIONAL'
CREATE PROCEDURE [dbo].[CPOS_Promo_Sambil]
    @MONTURA AS VARCHAR(7),
    @CRISTAL AS VARCHAR(7),
    @LC AS VARCHAR(7),
    @MONTURAPROPIA AS BIT,
    @CRISTALPROPIO AS BIT,
    @SERVICIO AS VARCHAR(7),
    @SERVAR AS BIT,
    @TIPOVENTA AS VARCHAR(3), -- "001 Venta Directa" o "002 CONVENCIONAL"
    @TipoExamen AS VARCHAR(13)
AS
BEGIN
    -- Variables
    DECLARE @PRECIOMONT NUMERIC(18,2), @LETRAMLS VARCHAR(3), @TIPART VARCHAR(1)
    DECLARE @PRECIOCRT NUMERIC(28,2), @PRECIOSERV NUMERIC(28,2)
    DECLARE @PRECIOMONT_DESC NUMERIC(28,2), @PRECIOCRT_DESC NUMERIC(28,2), @PRECIOSERV_DESC NUMERIC(28,2)
    DECLARE @RESULTADO VARCHAR(15) = 'NO APLICA'
    DECLARE @PORCDCTO INT = 0
    DECLARE @PORC_SERVICIO_DESC INT = 0 
    DECLARE @MARCA_LENTE VARCHAR(4)

    -- Obtener datos de la montura
    SELECT @LETRAMLS = CodRango, @PRECIOMONT = ART_PVP, @TIPART = TIPART, @MARCA_LENTE = REPLACE(MARCA, ' ', '')
    FROM TB_ARTICULO WHERE CodArticulo = @MONTURA

    -- Precio del servicio
    SELECT @PRECIOSERV = Art_PVP FROM TB_ARTICULO WHERE CodArticulo = @SERVICIO

    -- Precio del cristal
    SELECT @PRECIOCRT = art_pvp FROM TB_ARTICULO WHERE CodArticulo = @CRISTAL

    -- Condiciones de rangos
    IF @MARCA_LENTE <> 'CR'
    BEGIN
        -- Rangos de colores
        IF (
            @LETRAMLS IN ('A1','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z',
                          '1','2','3','4','5','6','7','8','9','10','11','12')
        )
        BEGIN
            -- Montura colores + cualquier cristal (convencional)
            IF @TIPOVENTA = '002' AND @TipoExamen = 'CONVENCIONAL' AND @MONTURAPROPIA = 0 AND @CRISTALPROPIO = 0
            BEGIN
                SET @PORCDCTO = 25
                SET @RESULTADO = 'APLICA'
            END
            -- Lente de sol colores (venta directa o con cristales formulados)
            IF @TIPOVENTA = '001' AND @TipoExamen = 'DIRECTA'
            BEGIN
                SET @PORCDCTO = 25
                SET @RESULTADO = 'APLICA'
            END
        END

        -- Rangos de outlet
        IF (
            @LETRAMLS IN ('AR','BR','CR','ZR','DR','ER','FR','GR','HR','IR','JR','KR','LR','MR','NR')
        )
        BEGIN
            -- Montura outlet + cualquier cristal (convencional)
            IF @TIPOVENTA = '002' AND @TipoExamen = 'CONVENCIONAL' AND @MONTURAPROPIA = 0 AND @CRISTALPROPIO = 0
            BEGIN
                SET @PORCDCTO = 25
                SET @RESULTADO = 'APLICA'
            END
            -- Lente de sol outlet (venta directa o con cristales formulados)
            IF @TIPOVENTA = '001' AND @TipoExamen = 'DIRECTA'
            BEGIN
                SET @PORCDCTO = 25
                SET @RESULTADO = 'APLICA'
            END
        END
    END

    -- Calcular precios con descuento si aplica
    IF @RESULTADO = 'APLICA'
    BEGIN
        SET @PRECIOMONT_DESC = @PRECIOMONT - ((@PRECIOMONT * @PORCDCTO) / 100)
        SET @PRECIOCRT_DESC = @PRECIOCRT - ((@PRECIOCRT * @PORCDCTO) / 100)
        SET @PRECIOSERV_DESC = @PRECIOSERV - ((@PRECIOSERV * @PORCDCTO) / 100)

        SELECT
            @RESULTADO AS RESULTADO,
            '235' as CODPROM,
            @LETRAMLS AS LETRAMLS,
            @PRECIOMONT AS PRECIOMONT,
            @PRECIOMONT_DESC AS PRECIOMONT_DESC, -----Esta variable asigna el descuento Numerico a las Monturas
            @PRECIOCRT AS PRECIOCRT,
            @PRECIOCRT_DESC AS PRECIOCRT_DESC,    -----Esta variable asigna el descuento Numerico a los Cristales
            @PRECIOSERV AS PRECIOSERV,
            @PRECIOSERV_DESC AS PRECIOSERV_DESC,
            @PORCDCTO AS PRECIOAR_DESC,
            @PORCDCTO AS PORCDCTO,  --Esta variable asigna el descuento Porcentual a todo lo que no es servicio (Ar y Otros), Monturas, lentes de contacto y cristales (Tambien aplica para garantia
            @PORCDCTO as PORC_SERVICIO_DESC  --@PORC_SERVICIO_DESC  --Esta variable asigna el descuento Porcentual a los servicios Distintos de Ar (Prisma, Dioptria...) 
            --@PRECIOAR_DESC------ --Esta variable asigna el descuento Porcentual al Ar
            --@TOTLC ---------------- Esta variable asigna el descuento Numerico a los lentes de Contacto
            
            ---Nota
            --tambien se puede Excluir Articulos pasando el codigo del articulo a la tabla 1 con el alias CodArticulo
            select 'A000004' as CodArticulo
    END
    ELSE
    BEGIN
        SELECT @RESULTADO AS RESULTADO, @LETRAMLS AS LETRAMLS
    END
END


GO


------------------------------------------------------------------------------------------


/****** Object:  StoredProcedure [dbo].[pEvaluoPromociones]    Script Date: 05/23/2025 10:22:22 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[pEvaluoPromociones]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[pEvaluoPromociones]
GO



/****** Object:  StoredProcedure [dbo].[pEvaluoPromociones]    Script Date: 05/23/2025 10:22:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--exec [pEvaluoPromociones] '230','M144228', 'C000727','002','CONVENCIONAL'
CREATE PROCEDURE [dbo].[pEvaluoPromociones]
	@CODPROMO AS VARCHAR(3),
	@PARAMETRO01 AS VARCHAR(20) = NULL,
    @PARAMETRO02 AS VARCHAR(20)= NULL, 
    @PARAMETRO03 AS VARCHAR(20)= NULL,
    @PARAMETRO04 AS VARCHAR(20)= NULL,
    @PARAMETRO05 AS VARCHAR(20)= NULL, 
    @PARAMETRO06 AS VARCHAR(20)= NULL, 
    @PARAMETRO07 AS VARCHAR(20)= NULL,
    @PARAMETRO08 AS VARCHAR(20)= NULL,
    @PARAMETRO09 AS VARCHAR(20)= NULL,
    @PARAMETRO10 AS VARCHAR(20)= NULL,
    @PARAMETRO11 AS VARCHAR(20)= NULL,
    @PARAMETRO12 AS VARCHAR(20)= NULL,
    @PARAMETRO13 AS VARCHAR(20)= NULL,
    @PARAMETRO14 AS VARCHAR(20)= NULL,
    @PARAMETRO15 AS VARCHAR(20)= NULL,
    @PARAMETRO16 AS VARCHAR(20)= NULL,
    @PARAMETRO17 AS VARCHAR(20)= NULL,
    @PARAMETRO18 AS VARCHAR(20)= NULL,
    @PARAMETRO19 AS VARCHAR(20)= NULL,
    @PARAMETRO20 AS VARCHAR(20)= NULL
    
AS
BEGIN

--------------------------Parametros enviados desde CaroniPos 
--------------------------                CodigoPromocion
--------------------------                Montura,
--------------------------                Cristal,
--------------------------                LC,
--------------------------                MonturaPropia ? "true" : PromoARObligatorio.ToString(), 
--------------------------                CristalPropio ? "true" : PromoARObligatorio.ToString(), 
--------------------------                AR,
--------------------------                promoAplicaAR ? "true" : PromoARObligatorio.ToString(),
--------------------------                glbTipoTrabajo,
--------------------------                TipoExamen,
--------------------------                _D_Inicio.DiaActivo().ToString("yyyy/MM/dd")
                
   DECLARE @PARAMETRO04_BIT BIT;
   DECLARE @PARAMETRO05_BIT BIT;
   DECLARE @PARAMETRO07_BIT BIT;
      
   IF @CODPROMO = '234'
   BEGIN
	----EXEC Promo_Mayo_Junio_2024 'M175970','C000376','','0','0','S000133',0,'002','CONVENCIONAL'
	--select  @PARAMETRO01, @PARAMETRO02, @PARAMETRO03,0,0,@PARAMETRO06,0,@PARAMETRO08,@PARAMETRO09
		EXEC dbo.PromodeMayoJunio_2025 @PARAMETRO01, @PARAMETRO02, @PARAMETRO03,0,0,@PARAMETRO06,0,@PARAMETRO08,@PARAMETRO09
   END
   IF @CODPROMO = '230'
   BEGIN
	----EXEC Promo_Cristales_SHADES  'M144228'  ,  'C000727'  ,'',0,0,'','',    '002'   ,'CONVENCIONAL'
		EXEC Promo_Cristales_SHADES @PARAMETRO01, @PARAMETRO02, @PARAMETRO03,@PARAMETRO04_BIT,@PARAMETRO05_BIT,@PARAMETRO06,@PARAMETRO07_BIT,@PARAMETRO08,@PARAMETRO09
   END
   ELSE
   IF @CODPROMO = '182'
   BEGIN
   
	   SET @PARAMETRO04_BIT = CONVERT(BIT, @PARAMETRO04); 
	   SET @PARAMETRO05_BIT = CONVERT(BIT, @PARAMETRO05);
	   SET @PARAMETRO07_BIT = CONVERT(BIT, @PARAMETRO07);
	   declare @FechaDiaActivo as datetime; 
	   set  @FechaDiaActivo = Convert (datetime, @PARAMETRO10);
		----EXEC Promo_Cristales_SHADES  'M144228'  ,  'C000727'  ,'',0,0,'','',    '002'   ,'CONVENCIONAL'
		EXEC CPOS_pPromoCombosConfig2020 @PARAMETRO01, @PARAMETRO02,@PARAMETRO04_BIT,@PARAMETRO05_BIT,@PARAMETRO06,@PARAMETRO07_BIT,@PARAMETRO08, @FechaDiaActivo
   END
   IF @CODPROMO = '219'
   BEGIN
	   SET @PARAMETRO04_BIT = CONVERT(BIT, @PARAMETRO04);
	   SET @PARAMETRO05_BIT = CONVERT(BIT, @PARAMETRO05);
	   SET @PARAMETRO07_BIT = CONVERT(BIT, @PARAMETRO07);
		----EXEC Promo_Cristales_SHADES  'M144228'  ,  'C000727'  ,'',0,0,'','',    '002'   ,'CONVENCIONAL'
		EXEC CPOS_Promo_Black_Friday_2024 @PARAMETRO01, @PARAMETRO02, @PARAMETRO03,@PARAMETRO04_BIT,@PARAMETRO05_BIT,@PARAMETRO06,@PARAMETRO07_BIT,@PARAMETRO08,@PARAMETRO09
   END
   
   IF @CODPROMO = '235'
   BEGIN
	   SET @PARAMETRO04_BIT = CONVERT(BIT, @PARAMETRO04);
	   SET @PARAMETRO05_BIT = CONVERT(BIT, @PARAMETRO05);
	   SET @PARAMETRO07_BIT = CONVERT(BIT, @PARAMETRO07);
		----EXEC Promo_Cristales_SHADES  'M144228'  ,  'C000727'  ,'',0,0,'','',    '002'   ,'CONVENCIONAL'
		EXEC CPOS_Promo_Sambil @PARAMETRO01, @PARAMETRO02, @PARAMETRO03,@PARAMETRO04_BIT,@PARAMETRO05_BIT,@PARAMETRO06,@PARAMETRO07_BIT,@PARAMETRO08,@PARAMETRO09
   END
   
  IF @CODPROMO = '229'
   BEGIN
	   SET @PARAMETRO04_BIT = CONVERT(BIT, @PARAMETRO04);
	   SET @PARAMETRO05_BIT = CONVERT(BIT, @PARAMETRO05);
	   SET @PARAMETRO07_BIT = CONVERT(BIT, @PARAMETRO07);
		----EXEC Promo_Cristales_SHADES  'M144228'  ,  'C000727'  ,'',0,0,'','',    '002'   ,'CONVENCIONAL'
		EXEC CPOS_Descuento_Seguros_Mercantil @PARAMETRO01, @PARAMETRO02, @PARAMETRO03,@PARAMETRO04_BIT,@PARAMETRO05_BIT,@PARAMETRO06,@PARAMETRO07_BIT,@PARAMETRO08,@PARAMETRO09
   END
   
     IF @CODPROMO = '227'
   BEGIN
	   SET @PARAMETRO04_BIT = CONVERT(BIT, @PARAMETRO04);
	   SET @PARAMETRO05_BIT = CONVERT(BIT, @PARAMETRO05);
	   SET @PARAMETRO07_BIT = CONVERT(BIT, @PARAMETRO07);
		----EXEC Promo_Cristales_SHADES  'M144228'  ,  'C000727'  ,'',0,0,'','',    '002'   ,'CONVENCIONAL'
		EXEC CPOS_Descuento_Gama_Club @PARAMETRO01, @PARAMETRO02, @PARAMETRO03,@PARAMETRO04_BIT,@PARAMETRO05_BIT,@PARAMETRO06,@PARAMETRO07_BIT,@PARAMETRO08,@PARAMETRO09
   END
   
        IF @CODPROMO = '226'
   BEGIN
	   SET @PARAMETRO04_BIT = CONVERT(BIT, @PARAMETRO04);
	   SET @PARAMETRO05_BIT = CONVERT(BIT, @PARAMETRO05);
	   SET @PARAMETRO07_BIT = CONVERT(BIT, @PARAMETRO07);
		----EXEC Promo_Cristales_SHADES  'M144228'  ,  'C000727'  ,'',0,0,'','',    '002'   ,'CONVENCIONAL'
		EXEC CPOS_Promo_Especial_Monturas_Outlet @PARAMETRO01, @PARAMETRO02, @PARAMETRO03,@PARAMETRO04_BIT,@PARAMETRO05_BIT,@PARAMETRO06,@PARAMETRO07_BIT,@PARAMETRO08,@PARAMETRO09
   END
   
        IF @CODPROMO = '224'
   BEGIN
	   SET @PARAMETRO04_BIT = CONVERT(BIT, @PARAMETRO04);
	   SET @PARAMETRO05_BIT = CONVERT(BIT, @PARAMETRO05);
	   SET @PARAMETRO07_BIT = CONVERT(BIT, @PARAMETRO07);
		----EXEC Promo_Cristales_SHADES  'M144228'  ,  'C000727'  ,'',0,0,'','',    '002'   ,'CONVENCIONAL'
		EXEC CPOS_Promo_Monturas_Outlet_2025 @PARAMETRO01, @PARAMETRO02, @PARAMETRO03,@PARAMETRO04_BIT,@PARAMETRO05_BIT,@PARAMETRO06,@PARAMETRO07_BIT,@PARAMETRO08,@PARAMETRO09
   END
END


-----------------------Leyenda------------------------------------------------- 
-----****** Cristal  "PRECIOCRT_DESC"  Valor -----------------------------------------
-----****** Montura  "PRECIOMONT_DESC"  Valor -----------------------------------------
-----****** LenteContacto  "TOTLC"     Valor  -----------------------------------------
-----****** AR  "PRECIOAR_DESC"    Porcentaje -----------------------------------
-----****** Servicio  "PORC_SERVICIO_DESC"  Porcentaje --------------------------
-----****** Otros  "PORCDCTO"  Porcentaje ---------------------------------------


--La tabla 01 recibe "CodArticulo" el codigo Articulo a excluir 

GO


-------------------------------------------------------------------------------------------
/****** Object:  StoredProcedure [dbo].[SP_CPOS_BuscarOrdenesporRango]    Script Date: 06/04/2025 11:24:25 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CPOS_BuscarOrdenesporRango]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_CPOS_BuscarOrdenesporRango]
GO

/****** Object:  StoredProcedure [dbo].[SP_CPOS_BuscarOrdenesListaOrdenes]    Script Date: 06/04/2025 11:24:25 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CPOS_BuscarOrdenesListaOrdenes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_CPOS_BuscarOrdenesListaOrdenes]
GO


/****** Object:  StoredProcedure [dbo].[SP_CPOS_BuscarOrdenesporRango]    Script Date: 06/04/2025 11:24:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO








--exec SP_CPOS_BuscarOrdenesporRango '20230729','20230829','',''
CREATE PROCEDURE [dbo].[SP_CPOS_BuscarOrdenesporRango] 

@Fechadesde as VARCHAR(8),
@Fechahasta as VARCHAR(8),
@Status VARCHAR(3),
@Cedula NVARCHAR(10),
@Inicio int ,
@Final int
AS
declare @UTasa_Dol as numeric (18,2)
--declare @Cod_Sucursal  varchar (3)
--set @Cod_Sucursal = (select Valor from TB_PARAMETRO where Parametro= 'Sucursal')
--exec pGet_TasaDiaPorFecha (@Cod_Sucursal, getdate())

declare @CantDiasOsAbonada as int

set @CantDiasOsAbonada = (select Valor*-1 from TB_PARAMETRO where Parametro= 'LimTiempoOsAbo')

 SELECT top (1)@UTasa_Dol = Tasa
	 FROM TB_TASA 
 where Cod_Moneda = '01'
 order by FecCreacion desc



BEGIN
SELECT    CONVERT(varchar,CAO.OrSer_fecCrea,103) as Fecha ,CAO.NumOrdserv, CAO.Revision , UPPER(left(CTE.CTE_PNombre, 1))+ LOWER(SUBSTRING(CTE.CTE_PNombre,2,len(CTE.CTE_PNombre)))
 + ' ' + UPPER(left(CTE.CTE_PApellido, 1))+ LOWER(SUBSTRING(CTE.CTE_PApellido,2,len(CTE.CTE_PApellido))) as Nombre, 
CASE
    WHEN CAO.OrSer_Status= '002' THEN 'Facturada'
    WHEN CAO.OrSer_Status= '003'  AND ISNULL(F.Nota,'0') = 0 THEN 'Anulada  '
    WHEN CAO.OrSer_Status= '003'  AND ISNULL(F.Nota,'0') = 1 THEN 'Reversada  '
    WHEN CAO.OrSer_Status= '004' THEN 'Por pagar'
    WHEN CAO.OrSer_Status= '005' THEN 'Abonada  ' 
    else CAO.OrSer_Status
END AS Estatus,
CAO.VtaTotal as Monto, ISNULL(CAST(CAO.Orser_Total_Mon AS DECIMAL(18,2)) ,CAST((CAO.VtaTotal/@UTasa_Dol) AS DECIMAL(18,2))) as Ref, CAO.Cod_Sucursal AS Cod_Sucursal
,CTE.CTE_Nacio as Nacionalidad
,CTE.CTE_CedIden as Cedula
into #TemOrdeness
FROM         TB_CAORDSER CAO
inner join TB_CTEPPAL  CTE
on CTE.CTE_CedIden= CAO.CTE_CedIden 
left join TB_FACTURAS F
on CAO.Cod_Sucursal = F.Cod_Sucursal 
and CAO.NumOrdserv = F.NumOrdServ 
where  OrSer_fecCrea BETWEEN  @Fechadesde + ' 00:00:00' AND @Fechahasta + ' 23:59:59' and  OrSer_Status like  '%'+ case when @Status = '000' THEN '003' else @Status end +'%'
and cao.Revision='0' and CAO.OrSer_Status<> '007'
and CAO.CTE_CedIden like '%'+ @Cedula +'%' -- PARA FILTRAR POR CEDULA
and ISNULL(F.Nota,'0') = case when @Status = '000' THEN 1 else 0 end
and OrSer_fecCrea > CASE
    WHEN CAO.OrSer_Status= '005' THEN  DATEADD(DD, @CantDiasOsAbonada,GETDATE())
    else '19000101'end

GROUP BY CAO.OrSer_fecCrea,CAO.NumOrdserv, CTE.CTE_PNombre,CTE.CTE_PApellido ,CAO.OrSer_Status, CAO.VtaTotal, CAO.Orser_Total_Mon,CAO.Cod_Sucursal,CTE.CTE_Nacio,CTE.CTE_CedIden,CAO.Revision, F.Nota
END



--------------------------NUEVO------------

select  '1' AS Retiene_IVA ,CAO.NumOrdserv, CAO.Revision
into #TemRetieeIVA2
from #TemOrdeness CAO
inner join  TB_ABONO ABO ON
ABO.NumOrdserv= CAO.NumOrdserv
and ABO.Cod_Sucursal= CAO.Cod_Sucursal
and ABO.Revision= cao.Revision
WHERE ABO.Tipo_Pago= '013'


select  '1' AS Retiene_ISLR ,CAO.NumOrdserv, CAO.Revision
into #TemRetieeISLR2
from #TemOrdeness CAO
inner join  TB_ABONO ABO ON
ABO.NumOrdserv= CAO.NumOrdserv
and ABO.Cod_Sucursal= CAO.Cod_Sucursal
and ABO.Revision= cao.Revision
WHERE ABO.Tipo_Pago= '014'


select A.*,ISNULL (B.Retiene_IVA,0)AS PAGOS_IVA, ISNULL (C.Retiene_ISLR,0)AS PAGOS_ISLR
INTO #TemOrdeness2 
from #TemOrdeness A
LEFT JOIN #TemRetieeIVA2 B ON 
A.NumOrdserv=B.NumOrdserv
and B.Revision= A.Revision
LEFT JOIN #TemRetieeISLR2  C ON 
A.NumOrdserv=C.NumOrdserv
and C.Revision= A.Revision


select '1' AS Registro_Comprobante_IVA ,fact.NumOrdServ, fact.ComprobRetencionIva as Comprobante_IVA_Numero, CAO.Revision
into #TemRetieeIVA_AUDI2
from #TemOrdeness2 CAO
inner join   TB_FACTURAS fact ON
fact.NumOrdServ= cao.NumOrdserv
and CAO.Cod_Sucursal= fact.Cod_Sucursal
--and fact.CTE_CedIdenPAG= cao.Cedula
--and fact.CTE_NacioPAG= cao.Nacionalidad
and fact.Revision= cao.Revision
WHERE Anulado= 0 and fact.Fact_Status= 'A' and fact.ComprobRetencionIva is not null 


select '1' AS Registro_Comprobante_ISLR ,fact.NumOrdServ, fact.ComprobRetencionISLR as Comprobante_ISLR_Numero, CAO.Revision
into #TemRetieeISLR_AUDI2
from #TemOrdeness2 CAO
inner join   TB_FACTURAS fact ON
fact.NumOrdServ= cao.NumOrdserv
and CAO.Cod_Sucursal= fact.Cod_Sucursal
--and fact.CTE_CedIdenPAG= cao.Cedula
--and fact.CTE_NacioPAG= cao.Nacionalidad
and fact.Revision= cao.Revision
WHERE Anulado= 0 and fact.Fact_Status= 'A' and fact.ComprobRetencionISLR is not null


select A.*,ISNULL (B.Retiene_IVA,0)AS PAGOS_IVA, ISNULL (C.Retiene_ISLR,0)AS PAGOS_ISLR, ISNULL (D.Registro_Comprobante_IVA,0)AS Comprobante_IVA, ISNULL (E.Registro_Comprobante_ISLR,0)AS Comprobante_ISLR,
 Comprobante_ISLR_Numero,Comprobante_IVA_Numero
into #Final_2
from #TemOrdeness A
LEFT JOIN #TemRetieeIVA2 B ON 
A.NumOrdserv=B.NumOrdserv
and a.Revision= b.Revision
LEFT JOIN #TemRetieeISLR2  C ON 
A.NumOrdserv=C.NumOrdserv
and a.Revision= C.Revision
LEFT JOIN #TemRetieeIVA_AUDI2 D ON 
A.NumOrdserv=D.NumOrdserv
and a.Revision= D.Revision
LEFT JOIN #TemRetieeISLR_AUDI2  E ON 
A.NumOrdserv=E.NumOrdserv
and a.Revision= E.Revision


select *, Row_Number() over (ORDER BY Fecha ) as Numero 
into #Final_2_2
from #Final_2 

select * from  #Final_2_2 where Numero  BETWEEN @Inicio and @final 
select COUNT(*) as cantidad from #Final_2_2


DROP TABLE #TemOrdeness
DROP TABLE #TemOrdeness2
DROP TABLE #TemRetieeIVA2
DROP TABLE #TemRetieeISLR2 
DROP TABLE #TemRetieeIVA_AUDI2
DROP TABLE #TemRetieeISLR_AUDI2 
drop table #Final_2
drop table #Final_2_2









GO

/****** Object:  StoredProcedure [dbo].[SP_CPOS_BuscarOrdenesListaOrdenes]    Script Date: 06/04/2025 11:24:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






--''


--exec SP_CPOS_BuscarOrdenesListaOrdenes '30','000','','',1,12
CREATE  PROCEDURE [dbo].[SP_CPOS_BuscarOrdenesListaOrdenes] 
@Fecha varchar(3),
@Status VARCHAR(3),
@Orden VARCHAR(7),
@Cedula NVARCHAR(10),
@Inicio int ,
@Final int
AS

declare @UTasa_Dol as numeric (18,2)
--declare @Cod_Sucursal  varchar (3)
--set @Cod_Sucursal = (select Valor from TB_PARAMETRO where Parametro= 'Sucursal')
--exec pGet_TasaDiaPorFecha (@Cod_Sucursal, getdate())

declare @CantDiasOsAbonada as int

set @CantDiasOsAbonada = (select Valor*-1 from TB_PARAMETRO where Parametro= 'LimTiempoOsAbo')

 SELECT top (1)@UTasa_Dol = Tasa
	 FROM TB_TASA 
 where Cod_Moneda = '01'
 order by FecCreacion desc

if @Orden<>'' and len(@Orden)= 7 
begin
SELECT    CONVERT(varchar,CAO.OrSer_fecCrea,103) as Fecha , CAO.NumOrdserv ,CAO.Revision, UPPER(left(CTE.CTE_PNombre, 1))+ LOWER(SUBSTRING(CTE.CTE_PNombre,2,len(CTE.CTE_PNombre)))
 + ' ' + UPPER(left(CTE.CTE_PApellido, 1))+ LOWER(SUBSTRING(CTE.CTE_PApellido,2,len(CTE.CTE_PApellido))) as Nombre, 
CASE
    WHEN CAO.OrSer_Status= '002' THEN 'Facturada'
    WHEN CAO.OrSer_Status= '003'  AND ISNULL(F.Nota,'0') = 0 THEN 'Anulada  '
    WHEN CAO.OrSer_Status= '003'  AND ISNULL(F.Nota,'0') = 1 THEN 'Reversada  '
    WHEN CAO.OrSer_Status= '004' THEN 'Por pagar'
    WHEN CAO.OrSer_Status= '005' THEN 'Abonada  ' 
    else CAO.OrSer_Status
END AS Estatus,
CAO.VtaTotal as Monto, ISNULL(CAST(CAO.Orser_Total_Mon AS DECIMAL(18,2)) ,CAST((CAO.VtaTotal/@UTasa_Dol) AS DECIMAL(18,2))) as Ref , CAO.Cod_Sucursal AS Cod_Sucursal
,CTE.CTE_Nacio as Nacionalidad
,CTE.CTE_CedIden as Cedula
into #TemOrdenes
FROM         TB_CAORDSER CAO
inner join TB_CTEPPAL  CTE
on CTE.CTE_CedIden= CAO.CTE_CedIden 
and cte.CTE_Nacio= Cao.CTE_Nacio
left join TB_FACTURAS F
on CAO.Cod_Sucursal = F.Cod_Sucursal 
and CAO.NumOrdserv = F.NumOrdServ 
where  OrSer_Status like  '%'+ case when @Status = '000' THEN '003' else @Status end +'%' and CAO.NumOrdserv= @Orden and cao.Revision='0' and CAO.OrSer_Status<> '007'
and ISNULL(F.Nota,'0') = case when @Status = '000' THEN 1 else 0 end
and OrSer_fecCrea > CASE
    WHEN CAO.OrSer_Status= '005' THEN  DATEADD(DD, @CantDiasOsAbonada,GETDATE())
    else '19000101'end

GROUP BY CAO.OrSer_fecCrea,CAO.NumOrdserv, CTE.CTE_PNombre,CTE.CTE_PApellido ,CAO.OrSer_Status, CAO.VtaTotal, CAO.Orser_Total_Mon,CAO.Cod_Sucursal
,CTE.CTE_Nacio,CTE.CTE_CedIden,CAO.Revision, F.Nota

--------------------------NUEVO------------


select  '1' AS Retiene_IVA ,CAO.NumOrdserv, CAO.Revision
into #TemRetieeIVA
from #TemOrdenes CAO
inner join  TB_ABONO ABO ON
ABO.NumOrdserv= CAO.NumOrdserv
and ABO.Cod_Sucursal= CAO.Cod_Sucursal
and ABO.Revision= cao.Revision
WHERE ABO.Tipo_Pago= '013'



select  '1' AS Retiene_ISLR ,CAO.NumOrdserv, CAO.Revision
into #TemRetieeISLR
from #TemOrdenes CAO
inner join  TB_ABONO ABO ON
ABO.NumOrdserv= CAO.NumOrdserv
and ABO.Cod_Sucursal= CAO.Cod_Sucursal
and ABO.Revision= cao.Revision
WHERE ABO.Tipo_Pago= '014'



select A.*,ISNULL (B.Retiene_IVA,0)AS PAGOS_IVA, ISNULL (C.Retiene_ISLR,0)AS PAGOS_ISLR
INTO #TemOrdenes2 
from #TemOrdenes A
LEFT JOIN #TemRetieeIVA B ON 
A.NumOrdserv=B.NumOrdserv
and B.Revision= A.Revision
LEFT JOIN #TemRetieeISLR  C ON 
A.NumOrdserv=C.NumOrdserv
and C.Revision= A.Revision


select '1' AS Registro_Comprobante_IVA ,fact.NumOrdServ, fact.ComprobRetencionIva as Comprobante_IVA_Numero, CAO.Revision
into #TemRetieeIVA_AUDI
from #TemOrdenes2 CAO
inner join   TB_FACTURAS fact ON
fact.NumOrdServ= CAO.NumOrdserv
and CAO.Cod_Sucursal= fact.Cod_Sucursal
and fact.Revision= CAO.Revision
WHERE Anulado= 0 and fact.Fact_Status= 'A' and fact.ComprobRetencionIva is not null 




select '1' AS Registro_Comprobante_ISLR ,fact.NumOrdServ, fact.ComprobRetencionISLR as Comprobante_ISLR_Numero, CAO.Revision
into #TemRetieeISLR_AUDI
from #TemOrdenes2 CAO
inner join   TB_FACTURAS fact ON
fact.NumOrdServ= cao.NumOrdserv
and CAO.Cod_Sucursal= fact.Cod_Sucursal
--and fact.CTE_CedIdenPAG= cao.Cedula
--and fact.CTE_NacioPAG= cao.Nacionalidad
and fact.Revision= cao.Revision
WHERE Anulado= 0 and fact.Fact_Status= 'A' and fact.ComprobRetencionISLR is not null


select A.*,ISNULL (B.Retiene_IVA,0)AS PAGOS_IVA, ISNULL (C.Retiene_ISLR,0)AS PAGOS_ISLR, ISNULL (D.Registro_Comprobante_IVA,0)AS Comprobante_IVA, ISNULL (E.Registro_Comprobante_ISLR,0)AS Comprobante_ISLR,
 Comprobante_ISLR_Numero,Comprobante_IVA_Numero
into #Final_1
from #TemOrdenes A
LEFT JOIN #TemRetieeIVA B ON 
A.NumOrdserv=B.NumOrdserv
and a.Revision= b.Revision
LEFT JOIN #TemRetieeISLR  C ON 
A.NumOrdserv=C.NumOrdserv
and a.Revision= C.Revision
LEFT JOIN #TemRetieeIVA_AUDI D ON 
A.NumOrdserv=D.NumOrdserv
and a.Revision= D.Revision
LEFT JOIN #TemRetieeISLR_AUDI  E ON 
A.NumOrdserv=E.NumOrdserv
and a.Revision= E.Revision


select *, Row_Number() over (ORDER BY Fecha) as Numero 
into #Final_1_2
from #Final_1 

select* from  #Final_1_2 where Numero  BETWEEN @Inicio and @final order by NumOrdserv DESC
select COUNT(*) as cantidad from #Final_1_2


DROP TABLE #TemOrdenes
DROP TABLE #TemOrdenes2
DROP TABLE #TemRetieeIVA 
DROP TABLE #TemRetieeISLR  
DROP TABLE #TemRetieeIVA_AUDI
DROP TABLE #TemRetieeISLR_AUDI  
drop table #Final_1
drop table #Final_1_2
--select * from #Final_1 where Numero BETWEEN @Inicio and @Final  

--------------------------NUEVO------------

end 

else 
--if @Fecha <> ''
begin

SELECT  CONVERT(varchar,CAO.OrSer_fecCrea,103) as Fecha,CAO.NumOrdserv ,CAO.Revision,UPPER(left(CTE.CTE_PNombre, 1))+ LOWER(SUBSTRING(CTE.CTE_PNombre,2,len(CTE.CTE_PNombre)))
 + ' ' + UPPER(left(CTE.CTE_PApellido, 1))+ LOWER(SUBSTRING(CTE.CTE_PApellido,2,len(CTE.CTE_PApellido))) as Nombre, 
CASE
   WHEN CAO.OrSer_Status= '002' THEN 'Facturada'
    WHEN CAO.OrSer_Status= '003'  AND ISNULL(F.Nota,'0') = 0 THEN 'Anulada  '
    WHEN CAO.OrSer_Status= '003'  AND ISNULL(F.Nota,'0') = 1 THEN 'Reversada  '
    WHEN CAO.OrSer_Status= '004' THEN 'Por pagar'
    WHEN CAO.OrSer_Status= '005' THEN 'Abonada  ' 
    else CAO.OrSer_Status
END AS Estatus,
CAO.VtaTotal as Monto, ISNULL(CAST(CAO.Orser_Total_Mon AS DECIMAL(18,2)) ,CAST((CAO.VtaTotal/@UTasa_Dol) AS DECIMAL(18,2))) as Ref , CAO.Cod_Sucursal AS Cod_Sucursal
,CTE.CTE_Nacio as Nacionalidad
,CTE.CTE_CedIden as Cedula
into #TemOrdeness
FROM         TB_CAORDSER CAO
inner join TB_CTEPPAL  CTE
on CTE.CTE_CedIden= CAO.CTE_CedIden 
and cte.CTE_Nacio= Cao.CTE_Nacio
left join TB_FACTURAS F
on CAO.Cod_Sucursal = F.Cod_Sucursal 
and CAO.NumOrdserv = F.NumOrdServ 

where  OrSer_fecCrea BETWEEN DATEADD(DD, -  convert(int ,@Fecha), CONVERT (DATE,GETDATE() + ' 00:00:00')) AND
CASE @Fecha WHEN 0 THEN DATEADD(DD, -  convert(int ,@Fecha), CONVERT (DATE,GETDATE() + ' 23:59:59')) 
-- Se hace el case para cuando sea el dia de ayer haga una resta del dia de ayer y solo me muestre lo del dia de ayer. 
            WHEN 1 THEN DATEADD(DD, -  convert(int ,@Fecha), CONVERT (DATE,GETDATE() + ' 23:59:59'))  ELSE  GETDATE()+ ' 23:59:59' END 
            
and cao.Revision='0'   
and OrSer_Status like  '%'+ case when @Status = '000' THEN '003' else @Status end +'%' and CAO.CTE_CedIden like '%'+ @Cedula +'%'  and CAO.OrSer_Status<> '007'  
and ISNULL(F.Nota,'0') = case when @Status = '000' THEN 1 else 0 end
and OrSer_fecCrea > CASE
    WHEN CAO.OrSer_Status= '005' THEN  DATEADD(DD, @CantDiasOsAbonada,GETDATE())
    else '19000101'end
-- PARA FILTRAR POR CEDULA  
--where  OrSer_fecCrea BETWEEN DATEADD(mm, - convert(int ,@Fecha), GETDATE()) AND GETDATE()  and OrSer_Status like  '%'+ @Status +'%'
GROUP BY CAO.OrSer_fecCrea, CAO.NumOrdserv, CTE.CTE_PNombre,CTE.CTE_PApellido ,CAO.OrSer_Status, CAO.VtaTotal, CAO.Orser_Total_Mon,CAO.Cod_Sucursal
,CTE.CTE_Nacio,CTE.CTE_CedIden,CAO.Revision, F.Nota



--------------------------NUEVO------------

select  '1' AS Retiene_IVA ,CAO.NumOrdserv, CAO.Revision
into #TemRetieeIVA2
from #TemOrdeness CAO
inner join  TB_ABONO ABO ON
ABO.NumOrdserv= CAO.NumOrdserv
and ABO.Cod_Sucursal= CAO.Cod_Sucursal
and ABO.Revision= cao.Revision
WHERE ABO.Tipo_Pago= '013'


select  '1' AS Retiene_ISLR ,CAO.NumOrdserv, CAO.Revision
into #TemRetieeISLR2
from #TemOrdeness CAO
inner join  TB_ABONO ABO ON
ABO.NumOrdserv= CAO.NumOrdserv
and ABO.Cod_Sucursal= CAO.Cod_Sucursal
and ABO.Revision= cao.Revision
WHERE ABO.Tipo_Pago= '014'


select A.*,ISNULL (B.Retiene_IVA,0)AS PAGOS_IVA, ISNULL (C.Retiene_ISLR,0)AS PAGOS_ISLR
INTO #TemOrdeness2 
from #TemOrdeness A
LEFT JOIN #TemRetieeIVA2 B ON 
A.NumOrdserv=B.NumOrdserv
and B.Revision= A.Revision
LEFT JOIN #TemRetieeISLR2  C ON 
A.NumOrdserv=C.NumOrdserv
and C.Revision= A.Revision


select '1' AS Registro_Comprobante_IVA ,fact.NumOrdServ, fact.ComprobRetencionIva as Comprobante_IVA_Numero, CAO.Revision
into #TemRetieeIVA_AUDI2
from #TemOrdeness2 CAO
inner join   TB_FACTURAS fact ON
fact.NumOrdServ= cao.NumOrdserv
and CAO.Cod_Sucursal= fact.Cod_Sucursal
--and fact.CTE_CedIdenPAG= cao.Cedula
--and fact.CTE_NacioPAG= cao.Nacionalidad
and fact.Revision= cao.Revision
WHERE Anulado= 0 and fact.Fact_Status= 'A' and fact.ComprobRetencionIva is not null 


select '1' AS Registro_Comprobante_ISLR ,fact.NumOrdServ, fact.ComprobRetencionISLR as Comprobante_ISLR_Numero, CAO.Revision
into #TemRetieeISLR_AUDI2
from #TemOrdeness2 CAO
inner join   TB_FACTURAS fact ON
fact.NumOrdServ= cao.NumOrdserv
and CAO.Cod_Sucursal= fact.Cod_Sucursal
--and fact.CTE_CedIdenPAG= cao.Cedula
--and fact.CTE_NacioPAG= cao.Nacionalidad
and fact.Revision= cao.Revision
WHERE Anulado= 0 and fact.Fact_Status= 'A' and fact.ComprobRetencionISLR is not null


select A.*,ISNULL (B.Retiene_IVA,0)AS PAGOS_IVA, ISNULL (C.Retiene_ISLR,0)AS PAGOS_ISLR, ISNULL (D.Registro_Comprobante_IVA,0)AS Comprobante_IVA, ISNULL (E.Registro_Comprobante_ISLR,0)AS Comprobante_ISLR,
 Comprobante_ISLR_Numero,Comprobante_IVA_Numero
into #Final_2
from #TemOrdeness A
LEFT JOIN #TemRetieeIVA2 B ON 
A.NumOrdserv=B.NumOrdserv
and a.Revision= b.Revision
LEFT JOIN #TemRetieeISLR2  C ON 
A.NumOrdserv=C.NumOrdserv
and a.Revision= C.Revision
LEFT JOIN #TemRetieeIVA_AUDI2 D ON 
A.NumOrdserv=D.NumOrdserv
and a.Revision= D.Revision
LEFT JOIN #TemRetieeISLR_AUDI2  E ON 
A.NumOrdserv=E.NumOrdserv
and a.Revision= E.Revision

select *, Row_Number() over (ORDER BY Fecha ) as Numero 
into #Final_2_2
from #Final_2 

select * from  #Final_2_2 where Numero  BETWEEN @Inicio and @final order by NumOrdserv DESC
select COUNT(*) as cantidad from #Final_2_2

DROP TABLE #TemOrdeness
DROP TABLE #TemOrdeness2
DROP TABLE #TemRetieeIVA2
DROP TABLE #TemRetieeISLR2 
DROP TABLE #TemRetieeIVA_AUDI2
DROP TABLE #TemRetieeISLR_AUDI2 
drop table #Final_2
drop table #Final_2_2
--------------------------NUEVO------------

end 













GO


---------------------------------------------------------------------------------
/****** Object:  StoredProcedure [dbo].[SP_CPOS_LibroVentas_Datos]    Script Date: 06/04/2025 11:39:57 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CPOS_LibroVentas_Datos]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_CPOS_LibroVentas_Datos]
GO

/****** Object:  StoredProcedure [dbo].[SP_CPOS_LibroVentas_Datos]    Script Date: 06/04/2025 11:39:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--[SP_CPOS_LibroVentas_Datos]'20250401','20250402'
CREATE PROCEDURE [dbo].[SP_CPOS_LibroVentas_Datos]
    @Fecha_Inicio DATE,
    @Fecha_Fin    DATE
AS
BEGIN

    SELECT
        FC.Fact_FecCrea Fecha,
        ISNULL(LV.CTE_Nacio + LV.CTE_CedIden,'')           AS Rif_Cedula,
        ISNULL(LV.CTE_PNombre + ' ' + LV.CTE_PApellido,'') AS Nombre_RazonSocial,
        ISNULL(LV.Fact_Num,'')                             AS NumeroFactura,
        ISNULL(LV.Fact_NumCtrol,'')                        AS NumeroControl,
        ISNULL(LV.Fact_SerialImpresora,'')                 AS ImpresoraFiscal,
        ''                                      AS NotaDebito,
        ISNULL(LV.NRONOTA,'')                              AS NotaCredito,
        '01'                                    AS TipoTransaccion,
        CASE WHEN LV.Anulado = 1 THEN LV.Fact_Num ELSE '' END AS FacturaAfectada,
        ISNULL(LV.Fact_Total,0)                           AS TotalVentas_Iva,
        ISNULL(LV.Fact_MontoExento,0)                     AS VentasExentas,
		'' AS VentasExoneradas,
		'' AS VentaNoSujetas,
        ISNULL(LV.Fact_MontoExento,0)                     AS TotalNoGravadas,

		--alicuota general
        ISNULL(LV.Fact_MontoGravable,0)                   AS BaseImponible,
        ISNULL(LV.Fact_AlicuotaIva,0)                     AS Alicuota,
        ISNULL(FC.Fact_Impuesto,0)                        AS ImpuestoIVA,

		--Alicuota Reducida
		'' AS BaseImponibleReducida,
		'' AS AlicuotaReducida,
		'' AS ImpuestoIVAReducido,

		--Alicuota General + Adicional
		'' AS BaseImponibleAdicional,
		'' AS AlicuotaAdicional,
		'' AS ImpuestoIVAAdicional,
		'' AS FechaRetencion,

		--retenciones de iva
		'' AS FacturaAfectadaRetencion,
        ISNULL(FC.IvaRetenido,0)                          AS IVARetenido,
        ISNULL(FC.ComprobRetencionIva, '')      AS ComprobanteRetencion

    FROM dbo.TB_LIBRO_VENTA LV

    INNER JOIN dbo.TB_FACTURAS FC
      ON LV.Cod_Sucursal = FC.Cod_Sucursal
     AND LV.CTE_CedIden  = FC.CTE_CedIdenPAG
     AND LV.Fact_Num     = FC.Fact_Num

    WHERE CAST(LV.Fecha AS DATE) BETWEEN @Fecha_Inicio AND @Fecha_Fin


END

GO

-----------------------------------------------------------------



/****** Object:  StoredProcedure [dbo].[SP_CPOS_GET_ARTICULO]    Script Date: 06/04/2025 16:12:20 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CPOS_GET_ARTICULO]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_CPOS_GET_ARTICULO]
GO



/****** Object:  StoredProcedure [dbo].[SP_CPOS_GET_ARTICULO]    Script Date: 06/04/2025 16:12:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO








-- =============================================
-- Author:		<FB>
-- Create date: <15/05/2023>
-- Description:	<strored con el que me traigo los detalles del articulo>
-- =============================================
-- exec SP_CPOS_GET_ARTICULO 'A000004',''
CREATE PROCEDURE [dbo].[SP_CPOS_GET_ARTICULO] 
@CodArticulo NVARCHAR(7) = NULL, -- Parámetro opcional con valor predeterminado NULL
@TipoTrabajo VARCHAR(3)= NULL
AS
BEGIN
    SET NOCOUNT ON;

   
if (@TipoTrabajo = null or @TipoTrabajo = '')
begin
    -- Consulta que filtra por CodArticulo solo si se proporciona un valor
    SELECT * 
    FROM TB_ARTICULO
    WHERE ((@CodArticulo IS NULL OR CodArticulo = @CodArticulo) and ART_ACTIVO= 1)  
    union 
    SELECT * 
    FROM TB_ARTICULO
    WHERE ((@CodArticulo IS NULL OR CodArticulo = @CodArticulo) and tipart = 'A')  
    
end 

--Trabajo Repocicion de Garantia
if (@TipoTrabajo in ('09'))
Begin 
SELECT * 
FROM TB_ARTICULO a  
inner join tb_tipo_articulo t on 
a.TIPART = t.CodTipo 
WHERE  ((ART_ACTIVO = 1) 
and (MANEJAEXISTENCIA = 0) 
AND (TIPART <> 'W') 
AND (TIPART <> 'A') 
AND (MARCA <> 'PROM') 
OR (ART_ACTIVO = 1) 
AND (MANEJAEXISTENCIA = 1) 
AND (ART_EXIST > 0) 
AND (TIPART <> 'W') 
AND (TIPART <> 'A') 
AND (CODPRO IS NULL) 
AND (MARCA <> 'PROM') 
OR (ART_ACTIVO = 1) 
AND (CODPRO = '065') 
AND (MANEJAEXISTENCIA = 1) 
AND (ART_EXIST > 0) 
AND (TIPART <> 'W') 
AND (TIPART <> 'A')
 AND (CodArticulo NOT LIKE 'A%') 
 AND (CodArticulo NOT LIKE 'C%') 
 AND (CodArticulo NOT LIKE 'S%') 
 AND (CodArticulo NOT LIKE 'M%')  
 AND (MARCA <> 'PROM')) 
 AND (a.CodArticulo <> 'S000138')
end 


--Trabajo Convencional
if (@TipoTrabajo in ('01'))
Begin 

SELECT a.*
FROM TB_ARTICULO a
left join  TB_SERVICIOSAR s on 
a.Codarticulo = s.CodServicio  
WHERE ((ART_ACTIVO = 1) 
AND (MANEJAEXISTENCIA = 0) 
AND (TIPART <> 'W') 
AND (TIPART <> 'A') 
AND (MARCA <> 'PROM') 
OR (ART_ACTIVO = 1) 
AND (MANEJAEXISTENCIA = 1) 
AND (ART_EXIST > 0) 
AND (TIPART <> 'W')
AND (TIPART <> 'A') 
AND (CODPRO IS NULL)
AND (MARCA <> 'PROM') 
OR (ART_ACTIVO = 1) 
AND (CODPRO = '065') 
AND (MANEJAEXISTENCIA = 1)
AND (TIPART <> 'W') 
AND (TIPART <> 'A') 
AND (a.CodArticulo NOT LIKE 'A%') 
AND (a.CodArticulo NOT LIKE 'C%') 
--AND (a.CodArticulo NOT LIKE 'S%') 
AND (a.CodArticulo NOT LIKE 'M%')  
AND (MARCA <> 'PROM') ) AND (s.CodServicio is null)
AND a.CodArticulo  <> 'S000138' 
UNION
SELECT a.*
FROM TB_ARTICULO a  
inner join  TB_SERVICIOSAR s on 
a.Codarticulo = s.CodServicio WHERE (s.codarticulo = @CodArticulo) and a.art_activo = 1  
ORDER BY  a.CodArticulo, a.DESART 
    
END
--Trabajo Contacto
if (@TipoTrabajo = '02')
Begin 

SELECT 
   *
FROM 
    TB_ARTICULO a
LEFT JOIN 
    TB_ARTICULOSERVICIO s 
ON 
    a.CodArticulo = s.CodEposArticulo
WHERE 
    (
        (a.TIPART = 'W') 
        OR (a.TIPART = 'S') 
        OR (a.TIPART = 'E') 
        OR ((a.TIPART = 'Q') AND a.ART_EXIST > 0)
    )
    AND (s.CodEposArticulo IS NULL) -- Artículos sin servicio asociado
    AND a.ART_ACTIVO = 1

UNION

-- Segunda Parte: Artículos con Servicio Específico Asociado
SELECT 
    *
FROM 
    TB_ARTICULO a
INNER JOIN 
    TB_ARTICULOSERVICIO s 
ON 
    a.CodArticulo = s.CodEposArticulo
WHERE 
    a.ART_ACTIVO = 1 
    AND s.CodServicio = '017'
ORDER BY 
    a.CodArticulo, a.DESART ;
    
END
--Reparacion
if (@TipoTrabajo = '05')
Begin
SELECT 
 *
FROM 
    TB_ARTICULO
WHERE 
    (
        (ART_ACTIVO = 1) 
        AND (MANEJAEXISTENCIA = 0) 
        AND (TIPART = 'S') 
    )
    OR 
    (
        (ART_ACTIVO = 1) 
        AND (MANEJAEXISTENCIA = 1) 
        AND (ART_EXIST > 0) 
        AND (TIPART = 'S') 
    )
ORDER BY 
    CodArticulo, DESART ;

END

--Venta Directa
if (@TipoTrabajo = '04')
Begin
SELECT 
    *
FROM 
    TB_ARTICULO a
INNER JOIN 
    TB_TIPO_ARTICULO t 
ON 
    a.TIPART = t.CodTipo
WHERE 
    (a.ART_EXIST > 0) -- Solo artículos con existencia mayor a 0
    AND (a.ART_ACTIVO = 1) -- Solo artículos activos
    AND (a.MANEJAEXISTENCIA = 1) -- Solo artículos que manejan existencia
    AND CHARINDEX('04', t.TipoVta) > 0 -- El tipo de venta contiene '04'
    AND (a.MARCA <> 'PROM') -- Excluir artículos con marca 'PROM'
    AND (a.CODPRO IS NULL) -- Solo artículos sin código de promoción
ORDER BY 
   CodArticulo, DESART ;
end



if (@TipoTrabajo = '08')
Begin 

SELECT 
   *
FROM TB_ARTICULO a
left join  TB_SERVICIOSAR s on 
a.Codarticulo = s.CodServicio  
WHERE ((ART_ACTIVO = 1) 
AND (MANEJAEXISTENCIA = 0) 
AND (TIPART <> 'W') 
AND (TIPART <> 'A') 
AND (MARCA <> 'PROM') 
OR (ART_ACTIVO = 1) 
AND (MANEJAEXISTENCIA = 1) 
AND (ART_EXIST > 0) 
AND (TIPART <> 'W')
AND (TIPART <> 'A') 
AND (CODPRO IS NULL)
AND (MARCA <> 'PROM') 
OR (ART_ACTIVO = 1) 
AND (CODPRO = '065') 
AND (MANEJAEXISTENCIA = 1)
AND (TIPART <> 'W') 
AND (TIPART <> 'A') 
AND (a.CodArticulo NOT LIKE 'A%') 
AND (a.CodArticulo NOT LIKE 'C%') 
AND (a.CodArticulo NOT LIKE 'S%') 
AND (a.CodArticulo NOT LIKE 'M%')  
AND (MARCA <> 'PROM') ) AND (s.CodServicio is null)
AND a.CodArticulo  <> 'S000138' 
UNION
SELECT 
    *
FROM TB_ARTICULO a  
inner join  TB_SERVICIOSAR s on 
a.Codarticulo = s.CodServicio WHERE (s.codarticulo = @CodArticulo) and a.art_activo = 1  
ORDER BY 
    a.CodArticulo, a.DESART ;

end

end




GO

----------------------------------------------------------------------------------------
IF NOT EXISTS (
    SELECT 1
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'TB_GARANTIA'
      AND COLUMN_NAME = 'NumExamen'
)
BEGIN
    ALTER TABLE dbo.TB_GARANTIA
    ADD NumExamen int NOT NULL CONSTRAINT DF_TB_GARANTIA_NumExamen DEFAULT (1);
END


-------------------------------------------------------------------------------------------



/****** Object:  StoredProcedure [dbo].[pAddGarantia]    Script Date: 06/06/2025 15:31:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[pAddGarantia]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[pAddGarantia]
GO


/****** Object:  StoredProcedure [dbo].[pAddGarantia]    Script Date: 06/06/2025 15:31:24 ******/
SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER ON
GO






--[pAddGarantia]'0154082','008','00001'
CREATE PROCEDURE [dbo].[pAddGarantia]
 @NUMORDSERV AS VARCHAR (7),
 @SUC AS VARCHAR (3),
 @USER AS VARCHAR (5)

AS              

SET NOCOUNT ON

/*DECLARE @NUMORDSERV AS VARCHAR (7)
DECLARE @SUC AS VARCHAR (3)
DECLARE @USER AS VARCHAR (5)

set @USER = '00001'
SET @NUMORDSERV= '9986193'
SET @SUC= '008'
*/

-------------------
DECLARE @CODDETVTA AS VARCHAR(2)
DECLARE @STATUSOS AS VARCHAR(3)
DECLARE @ASEGURADA AS BIT

--------------------------

SELECT @CODDETVTA = Cod_DetVta, @STATUSOS= OrSer_Status, @ASEGURADA = ASEGURADA FROM TB_CAORDSER WHERE NUMORDSERV = @NUMORDSERV AND Cod_Sucursal = @SUC and Revision = '0'

IF @CODDETVTA <> '09' AND @ASEGURADA = '1'
BEGIN

    INSERT INTO TB_GARANTIA
    SELECT C.COD_SUCURSAL, C.CTE_NACIO, C.CTE_CEDIDEN, CT.CTE_FNAC, C.NUMORDSERV, C.REVISION, C.FECHA,
    FACT_NUM, FACT_SERIALIMPRESORA, FACT_STATUS, F.FECHA, FACT_TOTAL, ESFD, ESFI,CILD,CILI, EJED,EJEI,
    ADDD, ADDI,0, NULL, GETDATE (), NULL, NULL,
     (select codarticulo from TB_DEORDSER where (Codarticulo like '%C%') and (ordser_ojo = 'A' or ordser_ojo = 'D') and NUMORDSERV = @NUMORDSERV and (Revision = '0')) as CrtDerecho,
     (select codarticulo from TB_DEORDSER where (Codarticulo like '%C%') and (ordser_ojo = 'A' or ordser_ojo = 'I') and NUMORDSERV = @NUMORDSERV and (Revision = '0'))as CrtIzquierdo,
     (select codarticulo from TB_DEORDSER where (Codarticulo like '%M%')  and NUMORDSERV = @NUMORDSERV and (Revision = '0')) as Montura,
     (select codarticulo from TB_DEORDSER where (Codarticulo like '%S%' and Codarticulo <> 'S000006') and (ordser_ojo = 'A' or ordser_ojo = 'D') and NUMORDSERV = @NUMORDSERV and (Revision = '0')) as DiopDerecha,
     (select codarticulo from TB_DEORDSER where (Codarticulo like '%S%' and Codarticulo <> 'S000006') and (ordser_ojo = 'A' or ordser_ojo = 'I') and NUMORDSERV = @NUMORDSERV and (Revision = '0'))as DiopIzquierda,
     (select ordserv_precio from TB_DEORDSER where (Codarticulo like '%C%') and (ordser_ojo = 'A' or ordser_ojo = 'D') and NUMORDSERV = @NUMORDSERV and (Revision = '0'))as PVPCrtDerecho,
     (select ordserv_precio from TB_DEORDSER where (Codarticulo like '%C%') and (ordser_ojo = 'A' or ordser_ojo = 'I') and NUMORDSERV = @NUMORDSERV and (Revision = '0'))as PVPCrtIzquierdo,
     (select ordserv_precio from TB_DEORDSER where (Codarticulo like '%S%' and Codarticulo <> 'S000006') and (ordser_ojo = 'A' or ordser_ojo = 'D') and NUMORDSERV = @NUMORDSERV and (Revision = '0'))as PVPDiopDerecha,
     (select ordserv_precio from TB_DEORDSER where (Codarticulo like '%S%' and Codarticulo <> 'S000006') and (ordser_ojo = 'A' or ordser_ojo = 'I') and NUMORDSERV = @NUMORDSERV and (Revision = '0'))as PVPDiopIzquierda,
     (select ordserv_precio from TB_DEORDSER where (Codarticulo like '%M%')  and NUMORDSERV = @NUMORDSERV and (Revision = '0'))as PVPMontura,
     (select ordserv_precio from TB_DEORDSER where (Codarticulo  = 'A000004')  and NUMORDSERV = @NUMORDSERV and (Revision = '0'))as PVPGarantia,
	 (select Codarticulo from TB_DEORDSER where (Codarticulo  = 'S000004')  and NUMORDSERV = @NUMORDSERV and (Revision = '0'))as CodServicioColor,
	 (select Codarticulo from TB_DEORDSER where (Codarticulo  = 'S000006')  and NUMORDSERV = @NUMORDSERV and (Revision = '0'))as CodServicioPrisma,
	 (select Codarticulo from TB_DEORDSER where (Codarticulo  in ('S000002','S000132','S000133','S000134')  and NUMORDSERV = @NUMORDSERV and (Revision = '0'))) as CodServicioAr,
	 (select Codarticulo from TB_DEORDSER where (Codarticulo  = 'S000138')  and NUMORDSERV = @NUMORDSERV and (Revision = '0'))as CodServicioG2, 
	C.NumExamen as NumExamen
    FROM TB_CAORDSER C 
    INNER JOIN TB_CTEPPAL CT ON C.CTE_CEDIDEN = CT.CTE_CEDIDEN AND  C.CTE_NACIO =CT.CTE_NACIO
    INNER JOIN TB_FACTURAS F ON C.NUMORDSERV = F.NUMORDSERV           
    INNER JOIN TB_EXAMEN E ON  C.CTE_CEDIDEN = E.CTE_CEDIDEN AND  C.CTE_NACIO = E.CTE_NACIO AND C.NumExamen = E.NUM_Examen and e.cod_sucursal = c.Cod_sucursal
    WHERE C.NUMORDSERV = @NUMORDSERV AND C.Cod_Sucursal = @SUC and c.Revision = '0'
    and F.Fact_Status <> 'I'

   /* SELECT * FROM TB_GARANTIA WHERE NUMORDSERV = @NUMORDSERV
    
    IF @@ROWCOUNT <> 0
    BEGIN
        INSERT INTO TB_GARANTIADETALLE
        SELECT C.COD_SUCURSAL, C.CTE_NACIO, C.CTE_CEDIDEN, C.NUMORDSERV,
                CODARTICULO, ORDSERV_CANT, ORDSER_OJO, ORDSERV_PRECIO, ORDSERV_PORCDTO, Ordserv_Impuesto
        FROM TB_CAORDSER C 
        INNER JOIN TB_DEORDSER D ON C.NUMORDSERV = D.NUMORDSERV and C.REVISION = D.REVISION
        WHERE C.NUMORDSERV = @NUMORDSERV AND D.REVISION = '0' AND c.Cod_Sucursal = @SUC
    END*/
END

IF @CODDETVTA = '09'
BEGIN
    SELECT 'REPOSCION', @STATUSOS

    SELECT * FROM TB_GARANTIA WHERE NUMORDSERV = @NUMORDSERV AND Cod_Sucursal = @SUC

    if @@rowcount = 0 

    begin
        IF @STATUSOS = '002'
        BEGIN
            --SELECT 'REPOSCION FACTURADA '
            INSERT INTO TB_GARANTIA
            SELECT C.COD_SUCURSAL, C.CTE_NACIO, C.CTE_CEDIDEN, CT.CTE_FNAC, C.NUMORDSERV, C.REVISION, C.FECHA,
            FACT_NUM, FACT_SERIALIMPRESORA, FACT_STATUS, F.FECHA, FACT_TOTAL, ESFD, ESFI,CILD,CILI, EJED,EJEI,
            ADDD, ADDI,'1', NULL, GETDATE (), NULL, NULL,
			 (select codarticulo from TB_DEORDSER where (Codarticulo like '%C%') and (ordser_ojo = 'A' or ordser_ojo = 'D') and NUMORDSERV = @NUMORDSERV and (Revision = '0')) as CrtDerecho,
			 (select codarticulo from TB_DEORDSER where (Codarticulo like '%C%') and (ordser_ojo = 'A' or ordser_ojo = 'I') and NUMORDSERV = @NUMORDSERV and (Revision = '0'))as CrtIzquierdo,
			 (select codarticulo from TB_DEORDSER where (Codarticulo like '%M%')  and NUMORDSERV = @NUMORDSERV and (Revision = '0')) as Montura,
			 (select codarticulo from TB_DEORDSER where (Codarticulo like '%S%' and Codarticulo <> 'S000006') and (ordser_ojo = 'A' or ordser_ojo = 'D') and NUMORDSERV = @NUMORDSERV and (Revision = '0')) as DiopDerecha,
			 (select codarticulo from TB_DEORDSER where (Codarticulo like '%S%' and Codarticulo <> 'S000006') and (ordser_ojo = 'A' or ordser_ojo = 'I') and NUMORDSERV = @NUMORDSERV and (Revision = '0'))as DiopIzquierda,
			 (select isnull(ordserv_precio,0.00) from TB_DEORDSER where (Codarticulo like '%C%') and (ordser_ojo = 'A' or ordser_ojo = 'D') and NUMORDSERV = @NUMORDSERV and (Revision = '0'))as PVPCrtDerecho,
			 (select isnull(ordserv_precio,0.00) from TB_DEORDSER where (Codarticulo like '%C%') and (ordser_ojo = 'A' or ordser_ojo = 'I') and NUMORDSERV = @NUMORDSERV and (Revision = '0'))as PVPCrtIzquierdo,
			 (select isnull(ordserv_precio,0.00) from TB_DEORDSER where (Codarticulo like '%S%' and Codarticulo <> 'S000006') and (ordser_ojo = 'A' or ordser_ojo = 'D') and NUMORDSERV = @NUMORDSERV and (Revision = '0'))as PVPDiopDerecha,
			 (select isnull(ordserv_precio,0.00) from TB_DEORDSER where (Codarticulo like '%S%' and Codarticulo <> 'S000006') and (ordser_ojo = 'A' or ordser_ojo = 'I') and NUMORDSERV = @NUMORDSERV and (Revision = '0'))as PVPDiopIzquierda,
			 (select isnull(ordserv_precio,0.00 )from TB_DEORDSER where (Codarticulo like '%M%')  and NUMORDSERV = @NUMORDSERV and (Revision = '0'))as PVPMontura,
			 (select isnull(ordserv_precio,0.00) from TB_DEORDSER where (Codarticulo  = 'A000004')  and NUMORDSERV = @NUMORDSERV and (Revision = '0'))as PVPGarantia,
				 (select Codarticulo from TB_DEORDSER where (Codarticulo  = 'S000004')  and NUMORDSERV = @NUMORDSERV and (Revision = '0'))as CodServicioColor,
	 (select Codarticulo from TB_DEORDSER where (Codarticulo  = 'S000006')  and NUMORDSERV = @NUMORDSERV and (Revision = '0'))as CodServicioPrisma,
	 (select Codarticulo from TB_DEORDSER where (Codarticulo  in ('S000002','S000132','S000133','S000134')  and NUMORDSERV = @NUMORDSERV and (Revision = '0'))) as CodServicioAr,
	 (select Codarticulo from TB_DEORDSER where (Codarticulo  = 'S000138')  and NUMORDSERV = @NUMORDSERV and (Revision = '0'))as CodServicioG2
      ,C.NumExamen as NumExamen
            FROM TB_CAORDSER C 
            INNER JOIN TB_CTEPPAL CT ON C.CTE_CEDIDEN = CT.CTE_CEDIDEN AND  C.CTE_NACIO =CT.CTE_NACIO
            INNER JOIN TB_FACTURAS F ON C.NUMORDSERV = F.NUMORDSERV
            INNER JOIN TB_EXAMEN E ON  C.CTE_CEDIDEN = E.CTE_CEDIDEN AND  C.CTE_NACIO = E.CTE_NACIO AND C.NumExamen = E.NUM_Examen
            WHERE C.NUMORDSERV = @NUMORDSERV AND C.Cod_Sucursal = @SUC and c.Revision = '0'
            and F.Fact_Status <> 'I'
        END
    
        IF @STATUSOS = '005'
        BEGIN
            --SELECT 'REPOSCION ABONADA '
            INSERT INTO TB_GARANTIA
            SELECT C.COD_SUCURSAL, C.CTE_NACIO, C.CTE_CEDIDEN, CT.CTE_FNAC, C.NUMORDSERV, C.REVISION, C.FECHA,
            '', '', '', C.FECHA, C.VtaTotal, ESFD, ESFI,CILD,CILI, EJED,EJEI,
            ADDD, ADDI,'1', NULL, GETDATE (), NULL, NULL,
			 (select codarticulo from TB_DEORDSER where (Codarticulo like '%C%') and (ordser_ojo = 'A' or ordser_ojo = 'D') and NUMORDSERV = @NUMORDSERV and (Revision = '0')) as CrtDerecho,
			 (select codarticulo from TB_DEORDSER where (Codarticulo like '%C%') and (ordser_ojo = 'A' or ordser_ojo = 'I') and NUMORDSERV = @NUMORDSERV and (Revision = '0'))as CrtIzquierdo,
			 (select codarticulo from TB_DEORDSER where (Codarticulo like '%M%')  and NUMORDSERV = @NUMORDSERV and (Revision = '0')) as Montura,
			 (select codarticulo from TB_DEORDSER where (Codarticulo like '%S%' and Codarticulo <> 'S000006') and (ordser_ojo = 'A' or ordser_ojo = 'D') and NUMORDSERV = @NUMORDSERV and (Revision = '0')) as DiopDerecha,
			 (select codarticulo from TB_DEORDSER where (Codarticulo like '%S%' and Codarticulo <> 'S000006') and (ordser_ojo = 'A' or ordser_ojo = 'I') and NUMORDSERV = @NUMORDSERV and (Revision = '0'))as DiopIzquierda,
			 (select isnull(ordserv_precio,0.00) from TB_DEORDSER where (Codarticulo like '%C%') and (ordser_ojo = 'A' or ordser_ojo = 'D') and NUMORDSERV = @NUMORDSERV and (Revision = '0'))as PVPCrtDerecho,
			 (select isnull(ordserv_precio,0.00) from TB_DEORDSER where (Codarticulo like '%C%') and (ordser_ojo = 'A' or ordser_ojo = 'I') and NUMORDSERV = @NUMORDSERV and (Revision = '0'))as PVPCrtIzquierdo,
			 (select isnull(ordserv_precio,0.00) from TB_DEORDSER where (Codarticulo like '%S%' and Codarticulo <> 'S000006') and (ordser_ojo = 'A' or ordser_ojo = 'D') and NUMORDSERV = @NUMORDSERV and (Revision = '0'))as PVPDiopDerecha,
			 (select isnull(ordserv_precio,0.00) from TB_DEORDSER where (Codarticulo like '%S%' and Codarticulo <> 'S000006') and (ordser_ojo = 'A' or ordser_ojo = 'I') and NUMORDSERV = @NUMORDSERV and (Revision = '0'))as PVPDiopIzquierda,
			 (select isnull(ordserv_precio,0.00) from TB_DEORDSER where (Codarticulo like '%M%')  and NUMORDSERV = @NUMORDSERV and (Revision = '0'))as PVPMontura,
			 (select isnull(ordserv_precio,0.00) from TB_DEORDSER where (Codarticulo  = 'A000004')  and NUMORDSERV = @NUMORDSERV and (Revision = '0'))as PVPGarantia,
		 (select Codarticulo from TB_DEORDSER where (Codarticulo  = 'S000004')  and NUMORDSERV = @NUMORDSERV and (Revision = '0'))as CodServicioColor,
	 (select Codarticulo from TB_DEORDSER where (Codarticulo  = 'S000006')  and NUMORDSERV = @NUMORDSERV and (Revision = '0'))as CodServicioPrisma,
	 (select Codarticulo from TB_DEORDSER where (Codarticulo  in ('S000002','S000132','S000133','S000134')  and NUMORDSERV = @NUMORDSERV and (Revision = '0'))) as CodServicioAr,
	 (select Codarticulo from TB_DEORDSER where (Codarticulo  = 'S000138')  and NUMORDSERV = @NUMORDSERV and (Revision = '0'))as CodServicioG2
	 ,C.NumExamen as NumExamen
            FROM TB_CAORDSER C 
            INNER JOIN TB_CTEPPAL CT ON C.CTE_CEDIDEN = CT.CTE_CEDIDEN AND  C.CTE_NACIO =CT.CTE_NACIO
            --INNER JOIN TB_FACTURAS F ON C.NUMORDSERV = F.NUMORDSERV
            INNER JOIN TB_EXAMEN E ON  C.CTE_CEDIDEN = E.CTE_CEDIDEN AND  C.CTE_NACIO = E.CTE_NACIO AND C.NumExamen = E.NUM_Examen
            WHERE C.NUMORDSERV = @NUMORDSERV AND C.Cod_Sucursal = @SUC and c.Revision = '0'
        END
    
      /*  SELECT * FROM TB_GARANTIA WHERE NUMORDSERV = @NUMORDSERV
        
        IF @@ROWCOUNT <> 0
        BEGIN
            INSERT INTO TB_GARANTIADETALLE
            SELECT C.COD_SUCURSAL, C.CTE_NACIO, C.CTE_CEDIDEN, C.NUMORDSERV,
                    CODARTICULO, ORDSERV_CANT, ORDSER_OJO, ORDSERV_PRECIO, ORDSERV_PORCDTO, Ordserv_Impuesto
            FROM TB_CAORDSER C 
            INNER JOIN TB_DEORDSER D ON C.NUMORDSERV = D.NUMORDSERV  and C.REVISION = D.REVISION
            WHERE C.NUMORDSERV = @NUMORDSERV AND D.REVISION = '0' AND C.Cod_Sucursal = @SUC
        END*/
    END
    ELSE
    BEGIN
       /* UPDATE TB_GARANTIA
            SET FactNum = FACT_NUM,
                SerialImpresora = FACT_SERIALIMPRESORA,
                StatusFactura =FACT_STATUS,
                Fecha_Modif = GETDATE()
            FROM TB_CAORDSER C 
            INNER JOIN TB_CTEPPAL CT ON C.CTE_CEDIDEN = CT.CTE_CEDIDEN AND  C.CTE_NACIO =CT.CTE_NACIO
            INNER JOIN TB_FACTURAS F ON C.NUMORDSERV = F.NUMORDSERV
            INNER JOIN TB_EXAMEN E ON  C.CTE_CEDIDEN = E.CTE_CEDIDEN AND  C.CTE_NACIO = E.CTE_NACIO AND C.NumExamen = E.NUM_Examen
            WHERE C.NUMORDSERV = @NUMORDSERV AND C.Cod_Sucursal = @SUC and c.Revision = '0'
*/

	UPDATE TB_GARANTIA  
		SET FactNum = FACT_NUM,  
		SerialImpresora = FACT_SERIALIMPRESORA,  
		StatusFactura =FACT_STATUS,  
		Fecha_Modif = GETDATE()  
	--select    GETDATE()  ,g.NUMORDSERV, g.cod_sucursal , FACT_NUM, FACT_STATUS, FACT_SERIALIMPRESORA
	FROM TB_GARANTIA G
	 INNER JOIN TB_FACTURAS F ON G.NUMORDSERV = F.NUMORDSERV  and G.Cod_Sucursal = f.Cod_Sucursal
	 --INNER JOIN TB_CAORDSER C ON  G.Cod_Sucursal = C.Cod_Sucursal  AND G.NumOrdServ = G.NumOrdServ and 
               WHERE g.NUMORDSERV = @NUMORDSERV AND g.Cod_Sucursal = @SUC 
                  and F.Fact_Status <> 'I'--and c.Revision = '0'  
    END
END








GO


---------------------------------------------------------------------------------------



/****** Object:  StoredProcedure [dbo].[pGetInfoReposicion]    Script Date: 06/09/2025 09:53:21 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[pGetInfoReposicion]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[pGetInfoReposicion]
GO


/****** Object:  StoredProcedure [dbo].[pGetInfoReposicion]    Script Date: 06/09/2025 09:53:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--[pGetInfoReposicion]'0000386','V','0154084','008','24'
CREATE PROCEDURE [dbo].[pGetInfoReposicion]
 @CI AS VARCHAR (15),
 @NACIO AS VARCHAR (2),
 @OS AS VARCHAR (7),
 @suc AS VARCHAR (3),
 @NEWRX as VARCHAR (2)


AS
SET NOCOUNT ON

/*
DECLARE @NACIO AS VARCHAR (2)
DECLARE @CI AS VARCHAR (10)
DECLARE @OS AS VARCHAR (7)
DECLARE @suc AS VARCHAR (3)
DECLARE @NEWRX AS VARCHAR (2)

SET @NACIO = 'V'
SET @CI ='0041784'
SET @OS ='9986175'
SET @suc ='008'
SET @NEWRX = '3'*/

-------------------------------  
DECLARE @EDADNIÑO AS VARCHAR (2)   
DECLARE @EDADADULTO AS VARCHAR (2)  
DECLARE @PORDCTOPGE06N AS VARCHAR (2)  
DECLARE @PORDCTOPGE06A AS VARCHAR (2)  
DECLARE @PORDCTOPGE69N AS VARCHAR (2)  
DECLARE @PORDCTOPGE69A AS VARCHAR (2)  
DECLARE @PORDCTOPGE912N AS VARCHAR (2)  
DECLARE @PORDCTOPGE912A AS VARCHAR (2)  
----------------------------------  
DECLARE @EDADCTE AS VARCHAR (3)   
DECLARE @FECHAFACT AS datetime   
DECLARE @TIEMPOREPOS AS int  
DECLARE @PORDCTOCRT AS VARCHAR (2)  
DECLARE @PRECIOCRISTAL AS NUMERIC (18,2)  
DECLARE @CODIGOCRISTAL AS VARCHAR (7)  
DECLARE @CAMBIOFORMULA AS BIT  
DECLARE @CODIGODIOPTRIAD AS VARCHAR (7)  
DECLARE @CODIGODIOPTRIAI AS VARCHAR (7)  
DECLARE @PRECIODIOPTRIAD AS NUMERIC (18,2)  
DECLARE @PRECIODIOPTRIAI AS NUMERIC (18,2)  
DECLARE @MESES AS int

DECLARE @CODSERVCOLOR AS VARCHAR (7) 
DECLARE @PRECIOSERVCOLOR AS NUMERIC (18,2) 

DECLARE @CODSERVPRISMA AS VARCHAR (7) 
DECLARE @PRECIOSERVPRISMA AS NUMERIC (18,2) 

DECLARE @CODSERVAR AS VARCHAR (7) 
DECLARE @PRECIOSERVAR AS NUMERIC (18,2) 

DECLARE @CODSERVG2 AS VARCHAR (7) 
DECLARE @PRECIOSERVG2 AS NUMERIC (18,2) 

 
-----------------------------------  
  
set @TIEMPOREPOS = 0  
set @OS=dbo.funCompletar (@OS, '0', 7, 'I')  
  
SELECT @EDADNIÑO = valor FROM TB_PARAMETROSPGE WHERE PARAMETROPGE = 'EdadCteNiño'  
SELECT @EDADADULTO = valor FROM TB_PARAMETROSPGE WHERE PARAMETROPGE = 'EdadCteAdulto'  
SELECT @PORDCTOPGE06N = valor FROM TB_PARAMETROSPGE WHERE PARAMETROPGE = 'PorDctoPGE0-6N'  
SELECT @PORDCTOPGE06A = valor FROM TB_PARAMETROSPGE WHERE PARAMETROPGE = 'PorDctoPGE0-6A'  
SELECT @PORDCTOPGE69N = valor FROM TB_PARAMETROSPGE WHERE PARAMETROPGE = 'PorDctoPGE6-9N'  
SELECT @PORDCTOPGE69A = valor FROM TB_PARAMETROSPGE WHERE PARAMETROPGE = 'PorDctoPGE6-9A'  
SELECT @PORDCTOPGE912N = valor FROM TB_PARAMETROSPGE WHERE PARAMETROPGE = 'PorDctoPGE9-12N'  
SELECT @PORDCTOPGE912A = valor FROM TB_PARAMETROSPGE WHERE PARAMETROPGE = 'PorDctoPGE9-12A'  
  
--SELECT @EDADNIÑO, @EDADADULTO, @PORDCTOPGE06N, @PORDCTOPGE06A, @PORDCTOPGE69N, @PORDCTOPGE69A, @PORDCTOPGE912N, @PORDCTOPGE912A    
  
SELECT @EDADCTE=year(getdate()-Cte_FechaNac -1)-1900 ,    
       @FECHAFACT = convert(datetime,FechaFactura,112),    
       @TIEMPOREPOS = DATEDIFF(dd, @FECHAFACT, getdate())  ,
        @MESES = DATEDIFF(mm, @FECHAFACT, getdate())   
       FROM TB_GARANTIA WHERE Cte_Nacionalidad = @NACIO and  Cte_Cedula=@CI and NumOrdServ=@OS AND Cod_Sucursal = @suc    
--SELECT @TIEMPOREPOS  
--IF (@TIEMPOREPOS < 13) and (DATEPART(DAY,GETDATE())< DATEPART(DAY,@FECHAFACT)  )
--BEGIN   
--	--SELECT @TIEMPOREPOS  
--	SET @TIEMPOREPOS = @TIEMPOREPOS-1   
--	--SELECT @TIEMPOREPOS  
--END   
--SELECT @TIEMPOREPOS  
SELECT @CODIGOCRISTAL = isnull(CristalDerecho,Cristalizquierdo)
            FROM TB_GARANTIA GD INNER JOIN TB_ARTICULO A ON 
				 GD.CristalDerecho = A.CodArticulo   or
                 GD.Cristalizquierdo = A.CodArticulo   
            WHERE Cte_Nacionalidad = @NACIO and  Cte_Cedula=@CI and NumOrdServ=@OS AND Cod_Sucursal = @suc 
                    AND A.TIPART  = 'C'
  
SELECT @PRECIOCRISTAL = ART_PVP  FROM TB_ARTICULO  WHERE CodArticulo = @CODIGOCRISTAL  
   
 --Agregar Dioptrias  
SELECT @CODIGODIOPTRIAD = DioptriaDerecha  
            FROM TB_GARANTIA GD INNER JOIN TB_ARTICULO A ON GD.DioptriaDerecha = A.CodArticulo    
            INNER JOIN TB_AGREGADO AG ON GD.DioptriaDerecha = SUBSTRING(AG.Agregado_Producto , 2, 7)       
            WHERE Cte_Nacionalidad = @NACIO and  Cte_Cedula=@CI and NumOrdServ=@OS AND Cod_Sucursal = @suc  --and GD.Ojo = 'D'  
                   AND A.TIPART  = 'S' --AND GD.CodigoArticulo = AG.Agregado_Producto -- from [TB_AGREGADO]) -- ('S000103','S000104','S000107','S000108','S000111','S000112','S000113','S000114','S000115','S000116','S000117','S000118')  
                      
SELECT @PRECIODIOPTRIAD = ART_PVP   FROM TB_ARTICULO   WHERE CodArticulo = @CODIGODIOPTRIAD  
  
SELECT @CODIGODIOPTRIAI = Dioptriaizquierda  
            FROM TB_GARANTIA GD INNER JOIN TB_ARTICULO A ON GD.Dioptriaizquierda = A.CodArticulo    
            INNER JOIN TB_AGREGADO AG ON GD.Dioptriaizquierda = SUBSTRING(AG.Agregado_Producto , 2, 7)       
            WHERE Cte_Nacionalidad = @NACIO and  Cte_Cedula=@CI and NumOrdServ=@OS AND Cod_Sucursal = @suc  --and GD.Ojo = 'I'  
     AND A.TIPART  = 'S'   
       
SELECT @PRECIODIOPTRIAI = ART_PVP   FROM TB_ARTICULO  WHERE CodArticulo = @CODIGODIOPTRIAI  

SELECT @CODSERVCOLOR = CodServicioColoracion  , @PRECIOSERVCOLOR = A.ART_PVP 
            FROM TB_GARANTIA GD INNER JOIN TB_ARTICULO A ON GD.CodServicioColoracion = A.CodArticulo    
            WHERE Cte_Nacionalidad = @NACIO and  Cte_Cedula=@CI and NumOrdServ=@OS AND Cod_Sucursal = @suc  --and GD.Ojo = 'I'  
     AND A.TIPART  = 'S'   

SELECT @CODSERVPRISMA = CodServicioPrisma  , @PRECIOSERVPRISMA = A.ART_PVP 
            FROM TB_GARANTIA GD INNER JOIN TB_ARTICULO A ON GD.CodServicioPrisma = A.CodArticulo    
            WHERE Cte_Nacionalidad = @NACIO and  Cte_Cedula=@CI and NumOrdServ=@OS AND Cod_Sucursal = @suc  --and GD.Ojo = 'I'  
     AND A.TIPART  = 'S'
     
SELECT @CODSERVAR = CodServicioAR , @PRECIOSERVAR = A.ART_PVP 
            FROM TB_GARANTIA GD INNER JOIN TB_ARTICULO A ON GD.CodServicioAR = A.CodArticulo    
            WHERE Cte_Nacionalidad = @NACIO and  Cte_Cedula=@CI and NumOrdServ=@OS AND Cod_Sucursal = @suc  --and GD.Ojo = 'I'  
     AND A.TIPART  = 'S'     
     
SELECT @CODSERVG2 = CodServicioG2  , @PRECIOSERVG2 = A.ART_PVP 
            FROM TB_GARANTIA GD INNER JOIN TB_ARTICULO A ON GD.CodServicioG2 = A.CodArticulo    
            WHERE Cte_Nacionalidad = @NACIO and  Cte_Cedula=@CI and NumOrdServ=@OS AND Cod_Sucursal = @suc  --and GD.Ojo = 'I'  
     AND A.TIPART  = 'S'                          
    
--select @CODSERVCOLOR,@PRECIOSERVCOLOR,@CODSERVPRISMA, @PRECIOSERVPRISMA,@CODSERVAR,@PRECIOSERVAR,@CODSERVG2,@PRECIOSERVG2
--Agregar Dioptrias  
  
SELECT * FROM TB_EXAMEN E   
    INNER JOIN TB_GARANTIA G ON E.CTE_CedIden = G.Cte_Cedula AND E.CTE_Nacio = G.Cte_Nacionalidad  
    WHERE (NUM_Examen = @NEWRX    
    AND  CTE_Nacio = @NACIO   
    AND  CTE_CedIden = @CI  
    AND G.ESFD =E.ESFD AND G.ESFI =E.ESFI AND G.CILD =E.CILD AND G.CILI =E.CILI  
    AND G.EJED =E.EJED AND G.EJEI =E.EJEI AND G.ADDD =E.ADDD AND G.ADDI =E.ADDI) AND  NumOrdServ=@OS  
  
IF @@ROWCOUNT = 0  
BEGIN  
    SET @CAMBIOFORMULA = 1  
END  
ELSE   
BEGIN  
    SET @CAMBIOFORMULA = 0  
END  
  
--SELECT @CAMBIOFORMULA, @EDADCTE  
--IF @EDADCTE <@EDADADULTO  
--SELECT @CAMBIOFORMULA, @EDADCTE  
  
--IF @EDADCTE >=@EDADNIÑO AND @EDADCTE <@EDADADULTO  
  
--IF @EDADCTE <@EDADADULTO 

--select @TIEMPOREPOS as tiemporepos 

IF convert(Int,@EDADCTE)< convert(Int,@EDADADULTO)    
BEGIN  
	SELECT '8 A 18 AÑOS' AS EDADCLIENTE  
	--IF (@TIEMPOREPOS >= 0 AND @TIEMPOREPOS <= 180) ---AND @CAMBIOFORMULA = 0  
	IF (@TIEMPOREPOS >= '0'  AND @TIEMPOREPOS <= '183')   
	BEGIN  
		IF (@CAMBIOFORMULA = 0)-- and  (@CODIGODIOPTRIAD is not null) and (@CODIGODIOPTRIAI is not null)  
			SELECT @PRECIOCRISTAL AS PRECIOCRISTAL, (@PRECIOCRISTAL * 0.5) AS DESCUENTOCRT, '0 A 6 MESES', @MESES AS MESES, @CODIGODIOPTRIAD as CodDioptriaD,@PRECIODIOPTRIAD AS PRECIODIOPTRIAD, isnull((@PRECIODIOPTRIAD * 0.5),0) AS DESCUENTODIOPD, @CODIGODIOPTRIAI as CodDioptriaI,@PRECIODIOPTRIAI AS PRECIODIOPTRIAI, isnull((@PRECIODIOPTRIAI * 0.5),(0)) AS DESCUENTODIOPI  
			, @PRECIOSERVCOLOR PRECIOSERVCOLOR, isnull(@PRECIOSERVCOLOR * 0.5,0) AS DESCUENTOSERVCOLOR  ,@PRECIOSERVPRISMA PRECIOSERVPRISMA, isnull(@PRECIOSERVPRISMA * 0.5,0) AS DESCUENTOSERVPRISMA, @PRECIOSERVAR PRECIOSERVAR, isnull(@PRECIOSERVAR * 0.5,0) AS DESCUENTOSERVAR,@PRECIOSERVG2 PRECIOSERVG2, isnull(@PRECIOSERVG2 * 0.5,0) AS DESCUENTOSERVG2    
		ELSE  
			SELECT @PRECIOCRISTAL AS PRECIOCRISTAL, (0) AS DESCUENTOCRT, '0 A 6 MESES', @MESES AS MESES, 'CAMBIO DE FORMULA' AS CAMBIO,  @CODIGODIOPTRIAD as CodDioptriaD,@PRECIODIOPTRIAD AS PRECIODIOPTRIAD, (0) AS DESCUENTODIOPD ,@CODIGODIOPTRIAI as CodDioptriaI,@PRECIODIOPTRIAI AS PRECIODIOPTRIAI, (0) AS DESCUENTODIOPI  
			, @PRECIOSERVCOLOR PRECIOSERVCOLOR, (0) AS DESCUENTOSERVCOLOR  ,@PRECIOSERVPRISMA PRECIOSERVPRISMA, (0) AS DESCUENTOSERVPRISMA, @PRECIOSERVAR PRECIOSERVAR, (0) AS DESCUENTOSERVAR, @PRECIOSERVG2 PRECIOSERVG2, (0) AS DESCUENTOSERVG2   
        END     
              
        --IF @TIEMPOREPOS > 180  AND @TIEMPOREPOS <= 270 ---AND @CAMBIOFORMULA = 0  
	IF (@TIEMPOREPOS >= '184'  AND @TIEMPOREPOS <= '273')   
        BEGIN  
		IF(@CAMBIOFORMULA = 0) --and  (@CODIGODIOPTRIAD is not null) and (@CODIGODIOPTRIAI is not null)  
			SELECT @PRECIOCRISTAL AS PRECIOCRISTAL, (@PRECIOCRISTAL * 0.4) AS DESCUENTOCRT, '6 A 9 MESES', @MESES AS MESES, @CODIGODIOPTRIAD as CodDioptriaD,@PRECIODIOPTRIAD AS PRECIODIOPTRIAD, (@PRECIODIOPTRIAD * 0.4) AS DESCUENTODIOPD, @CODIGODIOPTRIAI as CodDioptriaI,@PRECIODIOPTRIAI AS PRECIODIOPTRIAI, isnull((@PRECIODIOPTRIAI * 0.4),0) AS DESCUENTODIOPI  
                 , @PRECIOSERVCOLOR PRECIOSERVCOLOR, isnull(@PRECIOSERVCOLOR * 0.4,0) AS DESCUENTOSERVCOLOR  ,@PRECIOSERVPRISMA PRECIOSERVPRISMA, isnull(@PRECIOSERVPRISMA * 0.4,0) AS DESCUENTOSERVPRISMA, @PRECIOSERVAR PRECIOSERVAR, isnull(@PRECIOSERVAR * 0.4,0) AS DESCUENTOSERVAR,@PRECIOSERVG2 PRECIOSERVG2, isnull(@PRECIOSERVG2 * 0.4,0) AS DESCUENTOSERVG2    
		ELSE  
			SELECT @PRECIOCRISTAL AS PRECIOCRISTAL, (0) AS DESCUENTOCRT, '6 A 9 MESES', @MESES AS MESES, 'CAMBIO DE FORMULA' AS CAMBIO , @CODIGODIOPTRIAD as CodDioptriaD,@PRECIODIOPTRIAD AS PRECIODIOPTRIAD, (0) AS DESCUENTODIOPD ,@CODIGODIOPTRIAI as CodDioptriaI,@PRECIODIOPTRIAI AS PRECIODIOPTRIAI, (0) AS DESCUENTODIOPI  
 			, @PRECIOSERVCOLOR PRECIOSERVCOLOR, (0) AS DESCUENTOSERVCOLOR  ,@PRECIOSERVPRISMA PRECIOSERVPRISMA, (0) AS DESCUENTOSERVPRISMA, @PRECIOSERVAR PRECIOSERVAR, (0) AS DESCUENTOSERVAR, @PRECIOSERVG2 PRECIOSERVG2, (0) AS DESCUENTOSERVG2   
 	END  
  
        --IF @TIEMPOREPOS > 270 AND @TIEMPOREPOS <= 365  
	IF (@TIEMPOREPOS >='273'  AND @TIEMPOREPOS <= '365')   
        BEGIN  
		SELECT @PRECIOCRISTAL AS PRECIOCRISTAL, (@PRECIOCRISTAL * 0.3) AS DESCUENTOCRT, '9 A 12 MESES', @MESES AS MESES, @CODIGODIOPTRIAD as CodDioptriaD,@PRECIODIOPTRIAD AS PRECIODIOPTRIAD, isnull((@PRECIODIOPTRIAD * 0.3),0) AS DESCUENTODIOPD ,  @CODIGODIOPTRIAI as CodDioptriaI,@PRECIODIOPTRIAI AS PRECIODIOPTRIAI, isnull((@PRECIODIOPTRIAI * 0.3),0) AS DESCUENTODIOPI  
        , @PRECIOSERVCOLOR PRECIOSERVCOLOR, isnull(@PRECIOSERVCOLOR * 0.3,0) AS DESCUENTOSERVCOLOR  ,@PRECIOSERVPRISMA PRECIOSERVPRISMA, isnull(@PRECIOSERVPRISMA * 0.3,0) AS DESCUENTOSERVPRISMA, @PRECIOSERVAR PRECIOSERVAR, isnull(@PRECIOSERVAR * 0.3,0) AS DESCUENTOSERVAR,@PRECIOSERVG2 PRECIOSERVG2, isnull(@PRECIOSERVG2 * 0.3,0) AS DESCUENTOSERVG2    

	END  
END  
  
  
IF convert(Int,@EDADCTE)>= convert(Int,@EDADADULTO)   
BEGIN  
	SELECT 'MAYOR 18 AÑOS' AS EDADCLIENTE  
         
	--IF (@TIEMPOREPOS >= 0 AND @TIEMPOREPOS <= 180)   
	IF (@TIEMPOREPOS >= '0'  AND @TIEMPOREPOS <= '183')   
        BEGIN  
		IF  (@CAMBIOFORMULA = 0)   
			SELECT @PRECIOCRISTAL AS PRECIOCRISTAL, @PRECIOCRISTAL AS DESCUENTOCRT, '0 A 6 MESES' AS TIEMPO, @MESES AS MESES , @CODIGODIOPTRIAD as CodDioptriaD,@PRECIODIOPTRIAD AS PRECIODIOPTRIAD, isnull(@PRECIODIOPTRIAD,0) AS DESCUENTODIOPD ,@CODIGODIOPTRIAI as CodDioptriaI,@PRECIODIOPTRIAI AS PRECIODIOPTRIAI, isnull(@PRECIODIOPTRIAI,0) AS DESCUENTODIOPI  
			, @PRECIOSERVCOLOR PRECIOSERVCOLOR, isnull(@PRECIOSERVCOLOR,0) AS DESCUENTOSERVCOLOR  ,@PRECIOSERVPRISMA PRECIOSERVPRISMA, isnull(@PRECIOSERVPRISMA,0) AS DESCUENTOSERVPRISMA, @PRECIOSERVAR PRECIOSERVAR, isnull(@PRECIOSERVAR,0) AS DESCUENTOSERVAR,@PRECIOSERVG2 PRECIOSERVG2, isnull(@PRECIOSERVG2,0) AS DESCUENTOSERVG2   
		ELSE  
            SELECT @PRECIOCRISTAL AS PRECIOCRISTAL, 0 AS DESCUENTOCRT, '0 A 6 MESES' AS TIEMPO, @MESES AS MESES, 'CAMBIO DE FORMULA' AS CAMBIO, @CODIGODIOPTRIAD as CodDioptriaD,@PRECIODIOPTRIAD AS PRECIODIOPTRIAD, (0) AS DESCUENTODIOPD ,@CODIGODIOPTRIAI as CodDioptriaI,@PRECIODIOPTRIAI AS PRECIODIOPTRIAI, (0) AS DESCUENTODIOPI  
            , @PRECIOSERVCOLOR PRECIOSERVCOLOR, (0) AS DESCUENTOSERVCOLOR  ,@PRECIOSERVPRISMA PRECIOSERVPRISMA, (0) AS DESCUENTOSERVPRISMA, @PRECIOSERVAR PRECIOSERVAR, (0) AS DESCUENTOSERVAR, @PRECIOSERVG2 PRECIOSERVG2, (0) AS DESCUENTOSERVG2   
        END              
          
	  
	--IF (@TIEMPOREPOS > 180  AND @TIEMPOREPOS <= 270) --AND @CAMBIOFORMULA = 0  
	IF (@TIEMPOREPOS >= '184'  AND @TIEMPOREPOS <= '273')   
	BEGIN  
		IF (@CAMBIOFORMULA = 0)   
             SELECT @PRECIOCRISTAL AS PRECIOCRISTAL, (@PRECIOCRISTAL * 0.5) AS DESCUENTOCRT,'6 A 9 MESES' AS TIEMPO, @MESES AS MESES,   @CODIGODIOPTRIAD as CodDioptriaD,@PRECIODIOPTRIAD AS PRECIODIOPTRIAD, isnull((@PRECIODIOPTRIAD * 0.5),0) AS DESCUENTODIOPD, @CODIGODIOPTRIAI as CodDioptriaI,@PRECIODIOPTRIAI AS PRECIODIOPTRIAI, isnull((@PRECIODIOPTRIAI * 0.5),0) AS DESCUENTODIOPI 
             , @PRECIOSERVCOLOR PRECIOSERVCOLOR, isnull(@PRECIOSERVCOLOR * 0.5,0) AS DESCUENTOSERVCOLOR  ,@PRECIOSERVPRISMA PRECIOSERVPRISMA, isnull(@PRECIOSERVPRISMA * 0.5,0) AS DESCUENTOSERVPRISMA, @PRECIOSERVAR PRECIOSERVAR, isnull(@PRECIOSERVAR * 0.5,0) AS DESCUENTOSERVAR,@PRECIOSERVG2 PRECIOSERVG2, isnull(@PRECIOSERVG2 * 0.5,0) AS DESCUENTOSERVG2    
        ELSE  
             SELECT @PRECIOCRISTAL AS PRECIOCRISTAL, (0) AS DESCUENTOCRT,'6 A 9 MESES' AS TIEMPO, @MESES AS MESES, 'CAMBIO DE FORMULA' AS CAMBIO, @CODIGODIOPTRIAD as CodDioptriaD,@PRECIODIOPTRIAD AS PRECIODIOPTRIAD, (0) AS DESCUENTODIOPD ,@CODIGODIOPTRIAI as CodDioptriaI,@PRECIODIOPTRIAI AS PRECIODIOPTRIAI, (0) AS DESCUENTODIOPI  
			 , @PRECIOSERVCOLOR PRECIOSERVCOLOR, (0) AS DESCUENTOSERVCOLOR  ,@PRECIOSERVPRISMA PRECIOSERVPRISMA, (0) AS DESCUENTOSERVPRISMA, @PRECIOSERVAR PRECIOSERVAR, (0) AS DESCUENTOSERVAR, @PRECIOSERVG2 PRECIOSERVG2, (0) AS DESCUENTOSERVG2   
        END  

	--IF @TIEMPOREPOS > 270 AND @TIEMPOREPOS <= 365  
	IF (@TIEMPOREPOS >='274'  AND @TIEMPOREPOS <= '365')   
        BEGIN   
  		SELECT @PRECIOCRISTAL AS PRECIOCRISTAL, (@PRECIOCRISTAL * 0.3) AS DESCUENTOCRT, '9 A 12 MESES' AS TIEMPO, @MESES AS MESES,  @CODIGODIOPTRIAD as CodDioptriaD,@PRECIODIOPTRIAD AS PRECIODIOPTRIAD, isnull((@PRECIODIOPTRIAD * 0.3),0) AS DESCUENTODIOPD, @CODIGODIOPTRIAI as CodDioptriaI,@PRECIODIOPTRIAI AS PRECIODIOPTRIAI, isnull((@PRECIODIOPTRIAI * 0.3),0) AS DESCUENTODIOPI  
             , @PRECIOSERVCOLOR PRECIOSERVCOLOR, isnull(@PRECIOSERVCOLOR * 0.3,0) AS DESCUENTOSERVCOLOR  ,@PRECIOSERVPRISMA PRECIOSERVPRISMA, isnull(@PRECIOSERVPRISMA * 0.3,0) AS DESCUENTOSERVPRISMA, @PRECIOSERVAR PRECIOSERVAR, isnull(@PRECIOSERVAR * 0.3,0) AS DESCUENTOSERVAR,@PRECIOSERVG2 PRECIOSERVG2, isnull(@PRECIOSERVG2 * 0.3,0) AS DESCUENTOSERVG2    
        END  
              
END







GO


