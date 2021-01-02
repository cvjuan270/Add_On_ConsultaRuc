using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Add_On_ConsultaRuc.Model
{
    public class SocioNegocio
    {
        public string CardType { get; set; }
        public string ruc { get; set; }
        public string razonSocial { get; set; }
        public string nombreComercial { get; set; }
        public List<object> telefonos { get; set; }
        public string tipo { get; set; }
        public string estado { get; set; }
        public string condicion { get; set; }
        public string direccion { get; set; }
        public string departamento { get; set; }
        public string provincia { get; set; }
        public string distrito { get; set; }
        public DateTime fechaInscripcion { get; set; }
        public string sistEmsion { get; set; }
        public string sistContabilidad { get; set; }
        public string actExterior { get; set; }
        public List<string> actEconomicas { get; set; }
        public List<string> cpPago { get; set; }
        public List<string> sistElectronica { get; set; }
        public DateTime fechaEmisorFe { get; set; }
        public List<string> cpeElectronico { get; set; }
        public object fechaPle { get; set; }
        public List<string> padrones { get; set; }
        public object fechaBaja { get; set; }
        public string profesion { get; set; }
    }
}
