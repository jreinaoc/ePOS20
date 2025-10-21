using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaDatos.CierreCaja_Datos;
using CapaEntidades;
using System.Data;
using System.IO;
using CapaDatos.DetalleOrden_Datos;
using System.IO.Compression;
using System.Data.SqlClient;




namespace CapaLogica.CierreCaja_Logica
{
    public class L_CierreCaja
    {

        private D_CierreCaja _D_CierreCaja = new D_CierreCaja();
        private D_DetalleOrden _D_DetalleOrden = new D_DetalleOrden();
        public bool ChequeaFacturasdelDia(string fecha, string usuario)
        {
            DataTable dt = _D_CierreCaja.ChequeaFacturasdelDia(fecha, usuario);

            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ORDSERVCRITERIOSVARIOS(string bandera, string condicion)
        {
            DataTable dt = _D_CierreCaja.ORDSERVCRITERIOSVARIOS(bandera, condicion);

            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CierreFueradeHorario(string codsuc, DateTime fechaIni, DateTime fechaFin)
        {
            try
            {
                DataTable dt = _D_CierreCaja.CierreFueradeHorario(codsuc, fechaIni, fechaFin);

                if (dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Código para manejar el error
                EscribirLog(ex.Message.ToString());
                return false;
            }
        }

        public DataTable CierrePuntodeVenta(string codsuc, string codBanco, string nroLote, DateTime fecha)
        {
            DataTable dt = _D_CierreCaja.CierrePuntodeVenta(codsuc, codBanco, nroLote, fecha);

            //if (dt.Rows.Count > 0)
            //{
            //    return dt;
            //}
            return dt;
            //else
            //{
            //    return false;
            //}
        }

        public DataTable ObtienePuntosdeVenta(string codPunto, DateTime fecha)
        {
            DataTable dt = _D_CierreCaja.ObtienePuntosdeVenta(codPunto, fecha);

            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return dt;
            }
        }

        public bool AgregaPuntosdeVenta(string codBanco, string tipo, DateTime fecha, string nroLote, decimal manualTarjCredito, decimal manualTarjCreditoAmex,
    decimal manualTarjDebito, decimal manualTarjOtros)
        {
            try
            {
                DataTable dt = _D_CierreCaja.AgregaPuntosdeVenta(codBanco, tipo, fecha, nroLote, manualTarjCredito, manualTarjCreditoAmex,
                manualTarjDebito, manualTarjOtros);

                if (dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Código para manejar el error
                EscribirLog(ex.Message.ToString());
                return false;
            }
        }

        public DataTable ObtineneCambioCierre(DateTime fechaIni, string codsuc)
        {
            DataTable dt = _D_CierreCaja.ObtineneCambioCierre(fechaIni, codsuc);

            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return dt;
            }
        }
        public DataTable ObtieneBancosPagoMovil(string codsuc)
        {
            DataTable dt = _D_CierreCaja.ObtieneBancosPagoMovil(codsuc);

            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return dt;
            }
        }

        public DataTable AgregaReferenciaPagoMovil(string codSuc, string nroOs, string referencia, string bancoEmisor)
        {
            DataTable dt = _D_CierreCaja.AgregaReferenciaPagoMovil(codSuc, nroOs, referencia, bancoEmisor);

            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return dt;
            }
        }

        public DataTable VerificaAsistenciaPendiente(string fecha, string tipoAsis)
        {
            DataTable dt = _D_CierreCaja.VerificaAsistenciaPendiente(fecha, "PEND");

            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return dt;
            }
        }

        public DataTable ActualizaAsistencia(string fecha, string hora, string codEmp, string usuario, string TIPO_HORA)
        {
            DataTable dt = _D_CierreCaja.ActualizaAsistencia(fecha, hora, codEmp, usuario, TIPO_HORA);

            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return dt;
            }
        }

        public DataTable ConsultaOsDia(DateTime fecha, string suc)
        {
            DataTable dt = _D_CierreCaja.ConsultaOsDia(fecha, suc);

            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return dt;
            }
        }

        public DataTable ObtieneUsuarios(string suc)
        {
            DataTable dt = _D_CierreCaja.ObtieneUsuarios(suc);

            if (dt.Rows.Count > 0)
            {
                return dt;
            }
            else
            {
                return dt;
            }
        }

        public bool GeneraInvenvioTXT(string codSuc, string fechaCierre, string ruta, string nombreArchivo, SqlCommand command = null)
        {
            try
            {
                // 1) Defino la carpeta donde quiero dejar el archivo
                //string carpetaDestino = @"C:\EnviosTXT";
                // 2) Me aseguro de que exista
                Directory.CreateDirectory(ruta);

                // 3) Construyo el nombre de archivo (puedes incluir fecha/hora para evitar colisiones)
                //string nombreArchivo = $"INVENVIO.txt";

                // 4) Combino carpeta y nombre
                string rutaCompleta = Path.Combine(ruta, nombreArchivo);

                // 5) Genero el DataTable
                DataTable dt = _D_CierreCaja.GeneraInvenvioTXT(codSuc, fechaCierre, command);

                // 6) Grabo el archivo
                using (var writer = new StreamWriter(rutaCompleta))
                {
                    foreach (DataRow row in dt.Rows)
                        writer.WriteLine(row[0]?.ToString());
                }

                // 7) Feedback al usuario (opcional)
                //MessageBox.Show($"Archivo generado en:\n{rutaCompleta}", "Listo",
                //                MessageBoxButtons.OK, MessageBoxIcon.Information);

                return true;
            }
            catch (Exception ex)
            {
                // Código para manejar el error
                EscribirLog(ex.Message.ToString());
                return false;
            }
        }

        public bool HayDatosInvenvioTXT(string codSuc, string fechaCierre, string ruta, string nombreArchivo, SqlCommand command = null)
        {
            try
            {
                DataTable dt = _D_CierreCaja.GeneraInvenvioTXT(codSuc, fechaCierre, command);

                if (dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Código para manejar el error
                EscribirLog(ex.Message.ToString());
                return false;
            }
        }

        public void FiltrarUsuario(string filtro, System.Windows.Forms.RadioButton Rd_Pnl3_Descripcion, System.Windows.Forms.RadioButton Rd_Pnl3_Codigo, System.Windows.Forms.DataGridView Dgv_Pnl3_ClienteAfiliado, List<Usuario> listaClienteAfiliado, List<Usuario> listaTemporal)
        {

            // Verificar si el filtro está vacío
            if (string.IsNullOrWhiteSpace(filtro))
            {
                // Restablecer la información original en el DataGridView

                listaTemporal = new List<Usuario>(listaClienteAfiliado); // Restaurar desde la lista original
                Dgv_Pnl3_ClienteAfiliado.DataSource = listaTemporal;
                return;
            }

            // Convertir el filtro a minúsculas para una búsqueda insensible a mayúsculas
            filtro = filtro.ToLower();

            // Crear una lista para almacenar los resultados filtrados
            var datosFiltrados = new List<Usuario>();

            // Recorrer la lista original (listaArticulos) para aplicar el filtro
            foreach (var clienteAfiliado in listaClienteAfiliado)
            {
                // Filtrar según la opción seleccionada
                if (Rd_Pnl3_Descripcion.Checked && clienteAfiliado.Nombre != null && clienteAfiliado.Nombre.ToLower().Contains(filtro))
                {
                    datosFiltrados.Add(clienteAfiliado);
                }
                else if (Rd_Pnl3_Codigo.Checked && clienteAfiliado.COD_USR != null && clienteAfiliado.COD_USR.ToLower().Contains(filtro))
                {
                    datosFiltrados.Add(clienteAfiliado);
                }
            }

            // Actualizar la lista temporal con los datos filtrados
            listaTemporal = datosFiltrados;

            // Actualizar la fuente de datos del DataGridView con los resultados filtrados
            Dgv_Pnl3_ClienteAfiliado.DataSource = datosFiltrados;
        }


        public DataTable ObtienePagosCierreCaja(string suc)
        {
            try
            {
                DataTable dt = _D_CierreCaja.ObtienePagosCierreCaja(suc);

                if (dt.Rows.Count > 0)
                {
                    return dt;
                }
                else
                {
                    return dt;
                }
            }

            catch (Exception ex)
            {
                // Código para manejar el error
                EscribirLog(ex.Message.ToString());
                return null;
            }
        }

        public bool ValidaExistenciaCaja(DataGridView dgvCierredecaja)
        {
            try
            {
                double existenteEnCaja = GetValorFila(dgvCierredecaja, 0);
                if (existenteEnCaja > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Código para manejar el error
                EscribirLog(ex.Message.ToString());
                return false;
            }
        }

        private double GetValorFila(DataGridView dgvCierredecaja, int filaIndex)
        {
            var valor = dgvCierredecaja.Rows[filaIndex].Cells["Total"].Value;
            if (valor != null && double.TryParse(valor.ToString(), out double resultado))
            {
                return resultado;
            }
            return 0;
        }

        public bool CierreDeCaja(DataGridView dgvCierredecaja, DateTime fecha, string codSucursal, string observacion, string usuario, Action<string> mostrarError, SqlCommand command = null)
        {
            try
            {

                decimal M_TotalIngresos = (decimal)GetValorFila(dgvCierredecaja, 0);
                decimal M_Efectivo = (decimal)GetValorFila(dgvCierredecaja, 5);
                decimal M_Cheques = 0;
                decimal M_Cupones = 0;
                decimal M_TicketsSalud = 0;
                decimal M_TicketsSaludEfec = 0;
                decimal M_TarjetaC = (decimal)GetValorFila(dgvCierredecaja, 7);
                decimal M_TarjetaD = (decimal)GetValorFila(dgvCierredecaja, 6);
                decimal M_NotaCredito = (decimal)GetValorFila(dgvCierredecaja, 3);
                decimal M_Credito = 0;
                decimal M_Reintegro = (decimal)GetValorFila(dgvCierredecaja, 2);
                decimal M_Gastos = (decimal)GetValorFila(dgvCierredecaja, 1);
                decimal M_Financiamiento = 0;
                decimal M_NotaDevolucion = (decimal)GetValorFila(dgvCierredecaja, 4);
                decimal M_OrdenPago = 0;
                decimal M_IVARetenido = (decimal)GetValorFila(dgvCierredecaja, 8);
                decimal M_ISRLRetenido = (decimal)GetValorFila(dgvCierredecaja, 9);
                decimal M_Transferencia = (decimal)GetValorFila(dgvCierredecaja, 10);
                decimal M_Vuelto = 0;
                string M_Observacion = observacion;
                string M_Usuario = usuario;
                bool cierreParcial = false;
                bool trabajaDomingos = false;
                string userEntrega = "";
                string userRecibe = "";

                DataSet dts = _D_CierreCaja.CierreDeCaja(fecha, codSucursal, M_TotalIngresos, M_Efectivo, M_Cheques, M_Cupones, M_TicketsSalud, M_TicketsSaludEfec, M_TarjetaC, M_TarjetaD, M_NotaCredito, M_Credito, M_Reintegro, M_Gastos, M_Financiamiento, M_NotaDevolucion, M_OrdenPago, M_IVARetenido, M_ISRLRetenido, M_Transferencia, M_Vuelto, M_Observacion, M_Usuario, cierreParcial, trabajaDomingos, userEntrega, userRecibe, command);

                if (dts != null && dts.Tables[0] != null)
                {
                    if (dts.Tables[0].Rows[0][0].ToString() == "SATISFACTORIO")
                    {
                        return true;
                    }
                    else
                    {
                        if (dts.Tables[0] != null && dts.Tables[0].Rows[0]["REPORTE_DIA"].ToString() == "0")
                        {
                            mostrarError("No hay reporte Z del día");
                            return false;
                        }
                        else
                        return false;
                    }
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                // Código para manejar el error
                EscribirLog(ex.Message.ToString());
                return false;
            }
        }

        public bool ActualizarParamCierreCaja(string codSuc)
        {
            try
            {
                DataTable dt = _D_CierreCaja.ActualizarParamCierreCaja(codSuc);

                return true;
            }
            catch (Exception ex)
            {
                // Código para manejar el error
                EscribirLog(ex.Message.ToString());
                return false;
            }
        }

        public bool DesbloqueSistema(string bloqueo, string codSuc)
        {
            try
            {
                DataTable dt = _D_CierreCaja.DesbloqueSistema(bloqueo, codSuc);

                return true;
            }
            catch (Exception ex)
            {
                // Código para manejar el error
                EscribirLog(ex.Message.ToString());
                return false;
            }
        }

        public bool ActualizarFacturas(string codSuc, SqlCommand command = null)
        {
            try
            {
                DataTable dt = _D_CierreCaja.ActualizarFacturas(codSuc, command);

                return true;
            }
            catch (Exception ex)
            {
                // Código para manejar el error
                EscribirLog(ex.Message.ToString());
                return false;
            }
        }

        public bool LibroVenta(DateTime fechaIni, DateTime fechaFin, SqlCommand command = null)
        {
            try
            {
                DataTable dt = _D_CierreCaja.LibroVenta(fechaIni, fechaFin, command);

                return true;
            }
            catch (Exception ex)
            {
                // Código para manejar el error
                EscribirLog(ex.Message.ToString());
                return false;
            }
        }

        public bool InventarioFaltante(DateTime fechaIni, SqlCommand command = null)
        {
            try
            {
                DataTable dt = _D_CierreCaja.InventarioFaltante(fechaIni, command);

                return true;
            }
            catch (Exception ex)
            {
                // Código para manejar el error
                EscribirLog(ex.Message.ToString());
                return false;
            }
        }

        public bool CreaAcc(DateTime fecha, string sucursal, SqlCommand command = null)
        {
            try
            {
                int nroDias = Convert.ToInt32(_D_DetalleOrden.TB_PARAMETRO("CantDiasOSXML"));
                DataTable dt = _D_CierreCaja.CreaAcc(fecha, nroDias, sucursal, command);

                return true;
            }
            catch (Exception ex)
            {
                // Código para manejar el error
                EscribirLog(ex.Message.ToString());
                return false;
            }
        }

        public bool CreaXMLACC(string sucursal, SqlCommand command = null)
        {
            try
            {
                string _xmlRutaDestino = _D_DetalleOrden.TB_PARAMETRO("RutaACC") + sucursal;

                DataTable dt = _D_CierreCaja.ObtieneTablasAcc("", command);

                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        // SE OBTIENE EL NOMBRE DE LA TABLA ACC
                        string ACCNombre = dt.Rows[i]["Tabla"].ToString();

                        // SE CREA EL DATASET CON LOS DATOS DE LA TABLA ACC
                        DataTable ds = _D_CierreCaja.ObtieneTablasAcc(ACCNombre, command);

                        ds.TableName = "ACC";

                        DataSet dataSet = new DataSet("DocumentElement");
                        dataSet.Tables.Add(ds.Copy()); // Usa Copy para evitar conflictos si el DataTable ya pertenece a otro DataSet

                        dataSet.WriteXml(System.IO.Path.Combine(_xmlRutaDestino, ACCNombre + ".xml"), XmlWriteMode.IgnoreSchema);

                    }

                    //string ruta = @"C:\Ruta\Donde\EstánLosXml";
                    ComprimirXmlEnCarpeta(_xmlRutaDestino, sucursal);

                    return true;
                }

                return true;
            }
            catch (Exception ex)
            {
                // Código para manejar el error
                EscribirLog(ex.Message.ToString());
                return false;
            }
        }

        public static void ComprimirXmlEnCarpeta(string carpetaPath, string sucursal)
        {
            string zipPath = Path.Combine(carpetaPath, "ZIP" + sucursal + ".zip");

            // Elimina zip previo si existe
            if (File.Exists(zipPath))
                File.Delete(zipPath);

            string[] archivosXml = Directory.GetFiles(carpetaPath, "*.xml");

            using (FileStream zipToOpen = new FileStream(zipPath, FileMode.Create))
            using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Create))
            {
                foreach (var archivo in archivosXml)
                {
                    string nombreArchivo = Path.GetFileName(archivo);

                    // Añade el archivo al zip manualmente
                    ZipArchiveEntry entry = archive.CreateEntry(nombreArchivo);

                    using (var entryStream = entry.Open())
                    using (var fileStream = File.OpenRead(archivo))
                    {
                        fileStream.CopyTo(entryStream);
                    }
                }
            }

            foreach (var xmlPath in Directory.EnumerateFiles(carpetaPath, "*.xml"))
            {
                try
                {
                    // Si el archivo es readonly, primero quita el atributo
                    File.SetAttributes(xmlPath, FileAttributes.Normal);
                    File.Delete(xmlPath);
                }
                catch (IOException ex)
                {
                    //// Aquí podrías hacer retry, loggear, o notificar al usuario
                    //Console.Error.WriteLine($"No se pudo eliminar '{xmlPath}': {ex.Message}");
                }
            }


            Console.WriteLine("✅ Archivos XML comprimidos en: " + zipPath);
        }

        public static void EscribirLog(string mensaje)
        {
            string ruta = "log.txt";
            string entrada = $"[{DateTime.Now}] {mensaje}";
            System.IO.File.AppendAllText(ruta, entrada + Environment.NewLine);
        }

        public bool BuscoAsistencia2(string fecha, Func<string, string, DialogResult> mostrarPregunta, Action<string> mostrarError, SqlCommand sqlCom = null)
        {
            try
            {
                bool OkAsis = false;

                // EH: 24/05/2021 Actualizar lista de asistencia del marca huella
                string IActivarAsisDia = _D_DetalleOrden.TB_PARAMETRO("ActivarAsisDia"); // eh: 25/05/2021
                if (IActivarAsisDia == "0")
                {
                    // Definir que hacer aqui
                    //DataSet dsInsertDetalle = ManBD.EjecutaStoreProcedure("SP_INSERTATB_ASISTENCIA", glbSucursalActual, Command);
                }


                if (TB_USUARIO.Id_Rol.Trim() != "000" && TB_USUARIO.Id_Rol.Trim() != "017" && (TB_USUARIO.Id_Rol.Trim() != "013" && _D_DetalleOrden.TB_PARAMETRO("BloqUsuSistemas") != "1")) // And GlbBloqUsuarioSistemas = True)
                {
                    // No es Propietario
                    DataSet ds = _D_CierreCaja.ObtieneAsistenciaPendiente(fecha, TB_USUARIO.COD_USR);

                    // Verifico que haya marcado asistencia el día de hoy
                    //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0 || string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CodEmpleado"].ToString()))
                    {
                        if (string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CodEmpleado"].ToString()))
                        {
                            if (IActivarAsisDia == "1") // EPOS
                            {
                                // ********** Antigua ****************************************
                                mostrarError("No ha marcado asistencia para la entrada del turno");

                                // ***************esto es para levantar el formulario de Asistencia***********************

                                //DialogResult x = mostrarPregunta("No ha marcado asistencia para el día de hoy, desea hacerlo ahora?", "Falta la Asistencia");
                                //if (x == DialogResult.OK)
                                //{
                                //    Asist.ShowDialog();
                                //    if (Asist.SeMarcoAsistencia == true)
                                //    {
                                //        // Vuelvo a llamar a esta misma funcion pra verificar si se marco o no la hora
                                //        OkAsis = BuscoAsistencia(Codigo, ref sqlCom);
                                //    }
                                //}
                                //else
                                //{
                                //    OkAsis = false;
                                //}
                            }
                            else // BIOADMIN
                            {
                                mostrarError("No ha marcado asistencia para el día de hoy");
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(ds.Tables[0].Rows[0]["HoraDeSalidaPrimerTurno"].ToString().Trim()))
                            {
                                OkAsis = true;
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["HoraDeEntradaSegundoTurno"].ToString().Trim()) && string.IsNullOrEmpty(ds.Tables[0].Rows[0]["HoraDeSalidaSegundoTurno"].ToString().Trim()))
                                {
                                    OkAsis = true;
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["HoraDeEntradaTercerTurno"].ToString().Trim()) && string.IsNullOrEmpty(ds.Tables[0].Rows[0]["HoraDeSalidaTercerTurno"].ToString().Trim()))
                                    {
                                        OkAsis = true;
                                    }
                                    else
                                    {
                                        if (IActivarAsisDia == "1") // EPOS
                                        {
                                            // Mientras se desarrolla el formulario de Asistencia
                                            mostrarError("No ha marcado asistencia para la entrada del turno");

                                            // ***************esto es para levantar el formulario de Asistencia***********************

                                            //DialogResult x = mostrarPregunta("No ha marcado asistencia para la entrada del turno, desea hacerlo ahora?", "Falta la hora de entrada");
                                            //if (x == DialogResult.OK)
                                            //{
                                            //    Asist.ShowDialog();
                                            //    if (Asist.SeMarcoAsistencia == true)
                                            //    {
                                            //        // Vuelvo a llamar a esta misma funcion pra verificar si se marco o no la hora
                                            //        OkAsis = BuscoAsistencia(Codigo, ref sqlCom);
                                            //    }
                                            //}
                                            //else
                                            //{
                                            //    OkAsis = false;
                                            //}
                                        }
                                        else // BIOADMIN
                                        {
                                            mostrarError("No ha marcado asistencia para la entrada del turno");
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        mostrarError("No ha marcado asistencia para el día de hoy");
                        return OkAsis;
                    }
                    // Despues de verificar la asistencia verifico el horario de descanso
                    if (OkAsis == true)
                    {
                        if (VerificarTiempoMaxTrabajo(ds, mostrarError, sqlCom) == true)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    // Si es Propietario
                    return true;
                }
            }
            catch (Exception ex)
            {
                mostrarError($"Error en la función BuscaoAsistencia : {ex.Message}");
                return false;
            }
        }

        public bool BuscoAsistencia(string fecha, Func<string, string, DialogResult> mostrarPregunta, Action<string> mostrarError, SqlCommand sqlCom = null)
        {
            try
            {
                bool OkAsis = false;

                // EH: 24/05/2021 Actualizar lista de asistencia del marca huella
                string IActivarAsisDia = _D_DetalleOrden.TB_PARAMETRO("ActivarAsisDia"); // eh: 25/05/2021
                if (IActivarAsisDia == "0")
                {
                    // Definir que hacer aqui
                    //DataSet dsInsertDetalle = ManBD.EjecutaStoreProcedure("SP_INSERTATB_ASISTENCIA", glbSucursalActual, Command);
                }


                if ((TB_USUARIO.Id_Rol.Trim() != "000" && TB_USUARIO.Id_Rol.Trim() != "017")) // And GlbBloqUsuarioSistemas = True)
                {
                    if (TB_USUARIO.Id_Rol.Trim() == "013")
                    {
                        if (_D_DetalleOrden.TB_PARAMETRO("BloqUsuSistemas") == "0")
                        {
                            // Si es Propietario
                            return true;
                        }
                    }
                    // No es Propietario
                    DataSet ds = _D_CierreCaja.ObtieneAsistenciaPendiente(fecha, TB_USUARIO.COD_USR);

                    // Verifico que haya marcado asistencia el día de hoy
                    //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0 || string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CodEmpleado"].ToString()))
                    {
                        if (IActivarAsisDia == "1") // EPOS
                        {
                            // ********** Antigua ****************************************
                            mostrarError("No ha marcado asistencia para el día de hoy");

                            //DialogResult x = mostrarPregunta("No ha marcado asistencia para el día de hoy, desea hacerlo ahora?", "Falta la Asistencia");
                            //if (x == DialogResult.Yes)
                            //{
                            //    //mostrarFormularioAsistencia();
                            //    if (seMarcoAsistencia())
                            //    {
                            //        // Vuelvo a llamar a esta misma funcion para verificar si se marco o no la hora
                            //        return BuscoAsistencia(codigo, mostrarPregunta, mostrarError, mostrarFormularioAsistencia, seMarcoAsistencia, sqlCom);
                            //    }
                            //}
                            //else
                            //{
                            //    OkAsis = false;
                            //}
                        }
                        else // BIOADMIN
                        {
                            mostrarError("No ha marcado asistencia para el día de hoy");
                            OkAsis = false;
                        }
                    }
                    else
                    {
                        DataRow row = ds.Tables[0].Rows[0];

                        if (string.IsNullOrEmpty(row["HoraDeSalidaPrimerTurno"].ToString().Trim()))
                        {
                            OkAsis = true;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(row["HoraDeEntradaSegundoTurno"].ToString().Trim()) && string.IsNullOrEmpty(row["HoraDeSalidaSegundoTurno"].ToString().Trim()))
                            {
                                OkAsis = true;
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(row["HoraDeEntradaTercerTurno"].ToString().Trim()) && string.IsNullOrEmpty(row["HoraDeSalidaTercerTurno"].ToString().Trim()))
                                {
                                    OkAsis = true;
                                }
                                else
                                {
                                    if (IActivarAsisDia == "1") // EPOS
                                    {
                                        mostrarError("No ha marcado asistencia para la entrada del turno");

                                        //DialogResult x = mostrarPregunta("No ha marcado asistencia para la entrada del turno, desea hacerlo ahora?", "Falta la hora de entrada");
                                        //if (x == DialogResult.Yes)
                                        //{
                                        //    mostrarFormularioAsistencia();
                                        //    if (seMarcoAsistencia())
                                        //    {
                                        //        // Vuelvo a llamar a esta misma funcion para verificar si se marco o no la hora
                                        //        return BuscoAsistencia(codigo, mostrarPregunta, mostrarError, mostrarFormularioAsistencia, seMarcoAsistencia, sqlCom);
                                        //    }
                                        //}
                                        //else
                                        //{
                                        //    OkAsis = false;
                                        //}
                                    }
                                    else // BIOADMIN
                                    {
                                        mostrarError("No ha marcado asistencia para la entrada del turno");
                                        OkAsis = false;
                                    }
                                }
                            }
                        }
                    }


                    // Despues de verificar la asistencia verifico el horario de descanso
                    if (OkAsis == true)
                    {
                        if (VerificarTiempoMaxTrabajo(ds, mostrarError, sqlCom) == true) 
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    // Si es Propietario
                    return true;
                }
            }
            catch (Exception ex)
            {
                mostrarError($"Error en la función BuscaoAsistencia : {ex.Message}");
                return false;
            }
        }

        public bool VerificarTiempoMaxTrabajo(DataSet dtAsis, Action<string> mostrarError, SqlCommand sqlCom)
        {
            if (TB_USUARIO.Id_Rol != "013")
            {
                // Verifico el primer turno
                if (string.IsNullOrWhiteSpace(dtAsis.Tables[0].Rows[0]["HoraDeSalidaPrimerTurno"]?.ToString()))
                {
                    DateTime entradaPrimerTurno = Convert.ToDateTime(dtAsis.Tables[0].Rows[0]["HoraDeEntradaPrimerTurno"]?.ToString());
                    int tiempoMax = Convert.ToInt32(_D_DetalleOrden.TB_PARAMETRO("TiempMaxTraba"));
                    double horasTrabajadas = DateTime.Now.Subtract(entradaPrimerTurno).TotalHours;

                    if (horasTrabajadas < tiempoMax)
                    {
                        double avisoHoras = DateTime.Now.Subtract(entradaPrimerTurno.AddMinutes(-30)).TotalHours;
                        if (avisoHoras >= tiempoMax)
                        {
                            //var Usus = new CapaNegocio.Usuario();
                            //Usus.ObtenerUsuarioCodigo(CodigoUsuario, sqlCom);
                            int minutosRestantes = 30 - DateTime.Now.Subtract(entradaPrimerTurno.AddMinutes(-30)).Minutes;

                            mostrarError(TB_USUARIO.USER_NOMBRE + " iniciará su período de descanso obligatorio en los próximos " + minutosRestantes + " minutos");
                            return true;
                        }

                        return true;
                    }
                    else
                    {
                        mostrarError("Han transcurrido más de " + tiempoMax + " horas desde su última marca de asistencia, registre su salida");
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }

        }

        public bool ObtieneAsistenciaPendiente(string fecha, string usuario)
        {
            DataSet ds = _D_CierreCaja.ObtieneAsistenciaPendiente(fecha, usuario);

            DataTable dtExiste = ds.Tables[0];
            DataTable dtAsistenciaPendiente = ds.Tables[1];
            //Si existe en la tabla
            if (dtExiste.Rows.Count > 0)
            {
                //No tiene marcas asistencia
                if (dtAsistenciaPendiente.Rows.Count > 0)
                {
                    return true;
                }
            }
            //else
            //{
            //    return false;
            //}
            return false;
        }

        public DataTable ObtieneAsistenciaPendienteds(string fecha, string usuario)
        {
            DataSet ds = _D_CierreCaja.ObtieneAsistenciaPendiente(fecha, usuario);

            DataTable dtExiste = ds.Tables[0];
            DataTable dtAsistenciaPendiente = ds.Tables[1];
            //Si existe en la tabla
            //if (dtExiste.Rows.Count > 0)
            //{
            //    //No tiene marcas asistencia
            //    if (dtAsistenciaPendiente.Rows.Count > 0)
            //    {
            //        return true;
            //    }
            //}
            //else
            //{
            //    return false;
            //}
            return dtExiste;
        }

        public DataTable RelacionMonedaEx(string fechaIni, string fechaFin, string codsuc, string docum)
        {
            try
            {
                DataTable dt = _D_CierreCaja.RelacionMonedaEx(fechaIni, fechaFin, codsuc, docum);


                return dt;

            }
            catch (Exception ex)
            {
                // Código para manejar el error
                EscribirLog(ex.Message.ToString());
                return null;
            }
        }

        public DataTable FechasTrnSol(string codsuc)
        {
            try
            {
                DataTable dt = _D_CierreCaja.FechasTrnSol(codsuc);


                return dt;

            }
            catch (Exception ex)
            {
                // Código para manejar el error
                EscribirLog(ex.Message.ToString());
                return null;
            }
        }

        public DataTable ModificaVendedor(string order, string codEmpleadoNew, string usuario, string Suc)
        {
            try
            {
                DataTable dt = _D_CierreCaja.ModificaVendedor(order, codEmpleadoNew, usuario, Suc);
                return dt;

            }
            catch (Exception ex)
            {
                // Código para manejar el error
                EscribirLog(ex.Message.ToString());
                return null;
            }
        }
    }
}
