using CapaDatos.CargarOrdenes_Datos;
using CapaEntidades;
using CapaLogica.ListaFactura_Logica;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica.Servicios
{
    public class Asignar_Rx
    {
        private readonly L_ListaFacturas _logicaFacturas = new L_ListaFacturas();
        private D_Articulos _D_Articulo = new D_Articulos();
        private List<TB_TRABAJO> _TRABAJO = new List<TB_TRABAJO>();

    //public void AsignarRx(string GlbCodDetVta , string OSaModificar, string sucursal, string nacio, string cediden, string Numero_Orden)
    //{
    //    if (GlbCodDetVta == "08" && !string.IsNullOrEmpty(OSaModificar))
    //    {
    //         var Lista_Trabajo = _D_Articulo.ObtenerTrabajo(sucursal, nacio, cediden, Numero_Orden);  // Trae el detalle del articulo 
    //         _TRABAJO.Clear(); // Limpiar la lista para evitar duplicados
    //         _TRABAJO.AddRange(Lista_Trabajo); // Agregar los datos obtenidos

    //          // Obtener artículos de la OS
    //          DataSet dsOS = _D_Articulo.ArticulosOS(OSaModificar, sucursal);

    //        FechaOfrecida(
    //            dsOS.Tables[0].Rows[0]["CRISTALD"],
    //            dsOS.Tables[0].Rows[0]["MONTURA"],
    //            dsOS.Tables[0].Rows[0]["COLOR"],
    //            dsOS.Tables[0].Rows[0]["AR"],
    //            Command
    //        );

    //        string color = dsOS.Tables[0].Rows[0]["COLOR"].ToString();
    //        string cristalD = dsOS.Tables[0].Rows[0]["CRISTALD"].ToString();
    //        string cristalI = dsOS.Tables[0].Rows[0]["CRISTALI"].ToString();

    //        if (!VerificoParametrosCristales(color == "0" ? "NO" : "SI", cristalD, cristalI, Command))
    //            return;

    //        if (!VerificoRangoDiametroCristales(cristalD, cristalI, dsOS.Tables[0].Rows[0]["MONTURA"].ToString(), Command))
    //            return;

    //        // Modificar trabajo
    //        DataSet dsModificoTrabajo = ManBD.EjecutaStoreProcedure(
    //            "SP_MODIFICATB_TRABAJO",
    //            $"{lbOSaModificar.Text}', '{NumExamen}', '{Convert.ToSingle(txtHorizontal.Text)}', '{Convert.ToSingle(txtVertical.Text)}', '{Convert.ToSingle(txtMaxima.Text)}', '{Convert.ToSingle(txtPuente.Text)}', '{Convert.ToSingle(TxtDisVert.Text)}', '{Convert.ToSingle(TxtAngPant.Text)}', '{Convert.ToSingle(TxtAngFac.Text)}', '{Convert.ToSingle(txtAltD.Text)}', '{Convert.ToSingle(txtAltI.Text)}', '{cbOjo.Text}', '{cbTipoVisionD.Text}', '{cbTipoVisionI.Text}', '{oLaboratorio.Codigo}', '{oServicio.CodServicio}', '{dtpHoraOfre.Text}', '{dtpFechaOfre.Text}', '{OSModificar.CodigoDetalleVenta}', '{oTipoVenta.TipoExamen}', '{glbUsuarioActual}', '{glbSucursalActual}",
    //            Command
    //        );

    //        if (dsModificoTrabajo.Tables[0].Rows[0][0].ToString() == "SATISFACTORIO")
    //        {
    //            // Modificar OS
    //            DataSet dsModificoOS = ManBD.EjecutaStoreProcedure(
    //                "SP_MODIFICATB_CAORDSER_RX",
    //                $"{lbOSaModificar.Text}', '{NumExamen}', '{glbLaboratorio}', '{glbServicio}', '{Convert.ToDateTime(dtpFechaOfre.Text).ToString("yyyy/MM/dd")}', '{glbHoraOfrecido}', '{(GlbCodDetVta == "08" ? "01" : "02")}', '{glbUsuarioActual}', '{glbSucursalActual}",
    //                Command
    //            );

    //            if (dsModificoOS.Tables[0].Rows[0][0].ToString() == "SATISFACTORIO")
    //            {
    //                var Ord = new CapaNegocio.OrdenServicio();
    //                Ord.ObtenerOrdenServicioMayorRevision(lbOSaModificar.Text, glbSucursalActual, Command);

    //                var oUsu = new CapaNegocio.Usuario();
    //                oUsu.ObtenerUsuarioCodigo(glbUsuarioActual, Command);

    //                Transac.Commit();

    //                var dsimprimir = ManBD.EjecutaSPSelectNuevo("Valor", "TB_PARAMETRO", " Parametro = ''Imprimir ''", Command);

    //                var Reim = new frmRptImprimirOrden();
    //                var PrepararImpres = new frmPrepararImpresora();

    //                if (dsimprimir.Tables[0].Rows.Count > 0 && dsimprimir.Tables[0].Rows[0][0].ToString() == "1")
    //                {
    //                    PrepararImpres.ShowDialog();
    //                    if (glbModoPruebas == 1)
    //                        Reim.Reimprimir(lbOSaModificar.Text, 0, frmRptImprimirOrden.Destino.IMPRESORA, Command);
    //                    else
    //                        Reim.Reimprimir(lbOSaModificar.Text, 0, frmRptImprimirOrden.Destino.PANTALLA, Command);
    //                }
    //                else
    //                {
    //                    PrepararImpres.ShowDialog();
    //                    if (glbModoPruebas == 1)
    //                        Reim.Reimprimir(lbOSaModificar.Text, 0, frmRptImprimirOrden.Destino.PANTALLA, Command);
    //                    else
    //                        Reim.Reimprimir(lbOSaModificar.Text, 0, frmRptImprimirOrden.Destino.IMPRESORA, Command);
    //                }
    //            }
    //        }
    //        else
    //        {
    //            Transac.Rollback();
    //            MessageBox.Show("Se produjo un error mientras se modificaba la Orden.", "Proceso no culminado", MessageBoxButtons.OK, MessageBoxIcon.Error);
    //        }
    //    }
    //}
    //    public bool VerificoRangoDiametroCristales(List<TB_TRABAJO> Trabajo, string nacionalidad, string cedula, string Examen, string CristalD, string CristalI, string Montura)
    //    {
    //        bool AceptaCristalD = false;
    //        bool AceptaCristalI = false;
           
    //        string diamD = "";
    //        string diamI = "";

    //        // Obtén el primer trabajo (o el que corresponda según tu lógica)
    //        var _Trabajo = Trabajo.FirstOrDefault();

    //        if (_Trabajo == null)
    //            return false;

    //        // Obtener diámetros efectivos
    //        DataSet dsDiametroEfectivo = _D_Articulo.MostrarDiametroEfectivoCrtGrid(
    //            nacionalidad,
    //            cedula,
    //            Examen,
    //            CristalD,
    //            CristalI,
    //            "A",
    //            _Trabajo.T_TIPOVISIOND,
    //            _Trabajo.T_TIPOVISIONI,
    //            Montura,
    //            txtHorizontal.Text.Replace(".", ""),
    //            txtMaxima.Text.Replace(".", ""),
    //            txtPuente.Text.Replace(".", ""),
    //            _D_Inicio.Sucursal()
    //        );

    //        if (Convert.ToInt32(dsDiametroEfectivo.Tables[1].Rows[0][0]) > 0)
    //        {
    //            diamD = dsDiametroEfectivo.Tables[1].Rows[0]["DIAMETROEFECTIVODERECHO"].ToString();
    //            diamI = dsDiametroEfectivo.Tables[1].Rows[0]["DIAMETROEFECTIVOIZQUIERDO"].ToString();
    //        }
    //        else
    //        {
    //            diamD = "0";
    //            diamI = "0";
    //        }

    //        if (CristalD.StartsWith("C") || CristalI.StartsWith("C"))
    //        {
    //            DataSet dsValidaciones = _D_Articulo.MostrarValidaRangoCrtGrid(
    //                nacionalidad,
    //                cedula,
    //                Examen,
    //                "A",
    //                CristalD,
    //                CristalI,
    //                _Trabajo.T_ALTD,
    //                _Trabajo.T_ALTI,
    //                _Trabajo.T_TIPOVISIOND,
    //                _Trabajo.T_TIPOVISIONI,
    //                diamD,
    //                diamI
    //            );

    //            string ojo;
    //            if (!string.IsNullOrEmpty(CristalD) && !string.IsNullOrEmpty(CristalI))
    //            {
    //                ojo = "A";
    //            }
    //            else if (!string.IsNullOrEmpty(CristalD) && string.IsNullOrEmpty(CristalI))
    //            {
    //                ojo = "D";
    //            }
    //            else
    //            {
    //                ojo = "I";
    //            }

    //            if (ojo == "A")
    //            {
    //                AceptaCristalD = dsValidaciones.Tables[0].Rows.Count > 0;
    //                AceptaCristalI = dsValidaciones.Tables[1].Rows.Count > 0;
    //            }
    //            else if (ojo == "D")
    //            {
    //                AceptaCristalD = dsValidaciones.Tables[0].Rows.Count > 0;
    //                AceptaCristalI = true;
    //            }
    //            else if (ojo == "I")
    //            {
    //                AceptaCristalI = dsValidaciones.Tables[0].Rows.Count > 0;
    //                AceptaCristalD = true;
    //            }

    //            if (!AceptaCristalD || !AceptaCristalI)
    //            {
    //                DataSet dsConsultaCristal = _D_Articulo.MostrarParametrosCrtGrid(CristalD, CristalI);
    //                dgvRangoCrt.DataSource = dsConsultaCristal.Tables[0];

    //                lblDiametroD.Text = diamD;
    //                lblDiametroI.Text = diamI;

    //                lblDiametroD.Visible = lblDiametroI.Visible = true;
    //                LblTitulo.Text = "El cristal seleccionado no se adapta a los siguientes rangos:";
    //                lblClaveAut.Visible = true;
    //                lblLeyenda.Visible = false;
    //                btnAutorizarRangosCrt.Visible = true;
    //                pnlRangoCrt.Show();

    //                return false; // No se adapta a los rangos
    //            }
    //        }

    //        return true; // Todo correcto
    //    }
    }
}

