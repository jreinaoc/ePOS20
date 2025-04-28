using CapaEntidades;
using DataAccess.ExportarArchivos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CapaLogica.ExportarArchivos
{
    public abstract class ExportarArchivo_LibroVentas
    {

        public abstract Task<byte[]> ExportAsync<T>(IEnumerable<T> data, FormatoLibroVentasDTO datosEncabezado);
        // Add your interface members here
        public abstract Task<FormatoDeArchivo> ExportWithFormatAsync<T>(IEnumerable<T> data, FormatoLibroVentasDTO datosEncabezado);
        protected virtual List<PropertyInfoWrapper> GetProperties(Type type, string prefix = "")
        {
            List<PropertyInfoWrapper> properties = new List<PropertyInfoWrapper>();
            foreach (PropertyInfo property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                {
                    properties.AddRange(GetProperties(property.PropertyType, prefix + property.Name + "."));
                }
                else
                {
                    properties.Add(new PropertyInfoWrapper { Name = prefix + property.Name, Property = property });
                }
            }
            return properties;
        }

        protected virtual string GetValue(PropertyInfoWrapper propertyWrapper, object obj)
        {
            object value = propertyWrapper.Property.GetValue(obj, null);
            if (value == null)
            {
                return string.Empty;
            }

            Type valueType = value.GetType();
            if (valueType == typeof(float) || valueType == typeof(double) || valueType == typeof(decimal))
            {
                return string.Format("{0:F2}", value);
            }

            return value.ToString() ?? string.Empty;
        }

        protected class PropertyInfoWrapper
        {
            public string Name { get; set; }
            public PropertyInfo Property { get; set; }
        }
    }
}
