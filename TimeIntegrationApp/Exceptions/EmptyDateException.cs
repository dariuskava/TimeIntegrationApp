using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeIntegrationApp
{
    internal class EmptyDateException : ArgumentNullException
    {
        public EmptyDateException(string paramName, string details = "") : base(paramName, $"{paramName} date cannot be empty.{(string.IsNullOrEmpty(details) ? "" : $"  {details}")}") { }
    }   
}
