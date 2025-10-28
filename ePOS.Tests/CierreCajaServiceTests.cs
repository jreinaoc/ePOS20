using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CapaLogica.CierreCaja_Logica; // Ajusta según tu namespace
using System.Data;

namespace ePOS.Tests
{
    [TestClass]
public class CierreCajaServiceTests
{
    [TestMethod]
    public void EjecutarCierre_DeberiaRetornarTrue()
    {
        // Arrange
        var service = new CierreCajaService();

        // Simular tabla de caja
        var datosCaja = CrearDataTableSimulado();

        // Simular parámetros
        var diaActivo = new DateTime(2025, 09, 30);
        var sucursal = "131";
        var observaciones = "Cierre de prueba";
        var usuario = "00001";

        // Capturar logs generados
        //var logs = new List<(string Descripcion, string Resultado)>();
        //service.OnLogActualizado += (desc, res) => logs.Add((desc, res));

        // Act
        bool resultado = service.EjecutarCierre(datosCaja, diaActivo, sucursal, observaciones, usuario);

        // Assert
        Assert.IsTrue(resultado, "El cierre debería completarse correctamente.");
        //Assert.IsTrue(logs.Count > 0, "Se deberían haber generado logs.");
        //Assert.IsTrue(logs.Exists(l => l.Descripcion == "Cierre de Caja" && l.Resultado == "✔ Completado"));
    }

    private DataTable CrearDataTableSimulado()
    {
        var dt = new DataTable();
        dt.Columns.Add("Total", typeof(double));
        dt.Columns.Add("Descripcion", typeof(string));

        var fila = dt.NewRow();
        fila["Total"] = 1500.0;
        fila["Descripcion"] = "Efectivo";
        dt.Rows.Add(fila);

        return dt;
    }
}
}
