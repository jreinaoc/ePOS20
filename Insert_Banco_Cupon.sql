/* ============================================================
   Script: Insert_Banco_Cupon.sql
   Base de Datos: BD155DONACION (Conexion tomada de CapaPresentacioCaroniPOS\App.config)
   Proposito: Insertar el banco "Cupon" en TB_BANCOS con el proximo correlativo de CODBAN.
   ============================================================ */

BEGIN TRY
    BEGIN TRANSACTION;

    DECLARE @ProximoCodBan VARCHAR(3)
          , @Usuario VARCHAR(5) = '00001';

    /* Proximo correlativo de la tabla (maximo + 1), ignorando codigos especiales */
    SELECT @ProximoCodBan = RIGHT('000' + CAST((MAX(CAST(CODBAN AS INT)) + 1) AS VARCHAR), 3)
    FROM TB_BANCOS
    WHERE CODBAN NOT IN ('400');

    /* Validar que el correlativo calculado no exista ya */
    IF EXISTS (SELECT 1 FROM TB_BANCOS WHERE CODBAN = @ProximoCodBan)
    BEGIN
        RAISERROR ('El correlativo %s ya existe en TB_BANCOS. Revise manualmente.', 16, 1, @ProximoCodBan);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    INSERT INTO TB_BANCOS
    (
        CODBAN,
        NOMBREBANCO,
        ST_BANCOS,
        FEC_CREA,
        USER_CREA,
        MONEDAEXTRANJERA,
        PagoMovil,
        Deposito
    )
    VALUES
    (
        @ProximoCodBan,   -- CODBAN
        'CUPON',          -- NOMBREBANCO
        'A',              -- ST_BANCOS (I = Inactivo / A = Activo)
        GETDATE(),        -- FEC_CREA
        @Usuario,         -- USER_CREA
        0,                -- MONEDAEXTRANJERA
        0,                -- PagoMovil
        0                 -- Deposito
    );

    SELECT @ProximoCodBan AS CODBAN_INSERTADO;

    COMMIT TRANSACTION;
    PRINT 'Banco "Cupon" insertado correctamente con CODBAN = ' + @ProximoCodBan;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    DECLARE @ErrMsg NVARCHAR(MAX) = ERROR_MESSAGE();
    PRINT 'Error: ' + @ErrMsg;
    RAISERROR (@ErrMsg, 16, 1);
END CATCH;
