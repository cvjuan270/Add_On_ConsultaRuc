using Add_On_ConsultaRuc.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Add_On_ConsultaRuc.Controller
{
    public class AddSocioNegocioController
    {
        private static string oCardCode;
        public static int ActuDire(SAPbobsCOM.Company oCompany,SocioNegocio oSocioNegocio)
        {
            //Ajustamos numero de caracteres de direccion no mayor a 100

            if (oSocioNegocio.direccion.Length > 100)
            {
                int oIndexOf = oSocioNegocio.direccion.IndexOf('(');
                int oIndexOf1 = oSocioNegocio.direccion.IndexOf(')');
                oSocioNegocio.direccion = (oSocioNegocio.direccion.Remove(oIndexOf, (oIndexOf1 - oIndexOf) + 1).Substring(0, 100));
            }

            int ActuDireReturn = 0;
            SAPbobsCOM.BusinessPartners bp = null;
            bp = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners);
            bp.GetByKey(oCardCode);
            bp.Addresses.Delete();

            bp.Addresses.AddressName = "FT";
            bp.Addresses.Street = oSocioNegocio.direccion;
            bp.Addresses.City = oSocioNegocio.distrito;
            bp.Addresses.County = oSocioNegocio.provincia;
            //bp.Addresses.State = oSocioNegocio.estado;
            bp.Addresses.Country = "PE";
            bp.Addresses.AddressType = SAPbobsCOM.BoAddressType.bo_BillTo; //Direccion de factura
            bp.Addresses.Add();

            //AddDirec1return = bp.Update();
            bp.Addresses.AddressName = "GR";
            bp.Addresses.Street = oSocioNegocio.direccion;
            bp.Addresses.City = oSocioNegocio.distrito;
            bp.Addresses.County = oSocioNegocio.provincia;
            //bp.Addresses.State = oSocioNegocio.estado;
            bp.Addresses.Country = "PE";
            bp.Addresses.AddressType = SAPbobsCOM.BoAddressType.bo_ShipTo;
            bp.Addresses.Add();
            ActuDireReturn = bp.Update();
            return ActuDireReturn;
        }
        public static int CreaSoNe(SAPbobsCOM.Company oCompany, SocioNegocio oSocioNegocio)
        {
            oCardCode = oSocioNegocio.ruc + oSocioNegocio.CardType;
            int CreaSoNeReturn = 0;
            SAPbobsCOM.BusinessPartners bp = null;
            bp = (SAPbobsCOM.BusinessPartners)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners);
            bp.CardCode = oCardCode;
            bp.FederalTaxID = oSocioNegocio.ruc;
            bp.CardName = oSocioNegocio.razonSocial;
            if (oSocioNegocio.CardType=="C")
            {
                bp.CardType = SAPbobsCOM.BoCardTypes.cCustomer;
            }
            else
            {
                bp.CardType = SAPbobsCOM.BoCardTypes.cSupplier;
            }
            CreaSoNeReturn = bp.Add();

            //Inserta Direccion
            if (CreaSoNeReturn == 0)
            {
                CreaSoNeReturn = ActuDire(oCompany,oSocioNegocio);
            }

            return CreaSoNeReturn;
        }
    }
}
