using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Fiscalapi.Common
{
    /// <summary>
    /// Query string de los filtros de una lista: solo los valores asignados (un null no se envía), escapados y en el orden
    /// en que se agregan.
    /// </summary>
    internal sealed class QueryStringBuilder
    {
        private readonly List<KeyValuePair<string, string>> _parameters = new List<KeyValuePair<string, string>>();

        public QueryStringBuilder Add(string key, string value)
        {
            if (value != null)
                _parameters.Add(new KeyValuePair<string, string>(key, value));
            return this;
        }

        public QueryStringBuilder Add(string key, int? value) =>
            Add(key, value?.ToString(CultureInfo.InvariantCulture));

        /// <summary>Agrega la llave una vez por valor (status=a&amp;status=b).</summary>
        public QueryStringBuilder AddEach(string key, IEnumerable<string> values)
        {
            if (values != null)
            {
                foreach (var value in values)
                    Add(key, value);
            }

            return this;
        }

        /// <summary>Solo la fecha (yyyy-MM-dd): el API la lee como fecha civil de México.</summary>
        public QueryStringBuilder AddDate(string key, DateTime? date) =>
            Add(key, date?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));

        /// <summary>La query string sin el signo ?; vacía si no se agregó ningún valor.</summary>
        public override string ToString() =>
            string.Join("&", _parameters.Select(p => $"{p.Key}={Uri.EscapeDataString(p.Value)}"));
    }
}
