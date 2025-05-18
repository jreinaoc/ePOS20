using CapaDatos.Anulacion;
using CapaDatos.DatosGeneralSucursal_Datos;
using CapaDatos.DetalleOrden_Datos;
using CapaDatos.Inicio_Datos;
using CapaDatos.ListaFacturas_Datos;
using CapaDatos.ListaOrdenes_Datos;
using CapaEntidades;
using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica.DatosGeneralesSucursal_Logica
{
    public class L_DatosGenerales
    {
        //El uso de la clase StringBuilder nos ayudara a devolver los mensajes de las validaciones
        public readonly StringBuilder stringBuilder = new StringBuilder();

        //Instanciamos nuestra clase D_Loguin para poder utilizar sus miembros
        private D_ListaOrdenes _D_ListaOrdenes = new D_ListaOrdenes();

        private D_DetalleOrden _D_DetalleOrden = new D_DetalleOrden();
        private D_Inicio _D_Inicio = new D_Inicio();
        private D_Anulacion _D_Anulacion = new D_Anulacion();
        private D_ListaFactura _D_ListaFactura = new D_ListaFactura();

        private DatosGenerales _D_DatosGenerales = new DatosGenerales();

        public async Task ObtenerDatosSucursalYCompania_Global()
        {
            await Task.Run(() =>
            {
                DataSet ds = _D_DatosGenerales.CargarDatosSucursalYCompania();

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                    throw new Exception("No se encontraron los datos de la sucursal y compañía.");

                DataRow row = ds.Tables[0].Rows[0];

                VariablesGlobales.CodSucursal = row["CodSucursal"]?.ToString();
                VariablesGlobales.Sucursal = row["Sucursal"]?.ToString();
                VariablesGlobales.Compania = row["Compania"]?.ToString();
                VariablesGlobales.Rif = row["Rif"]?.ToString();
            });
        }



    }
}
