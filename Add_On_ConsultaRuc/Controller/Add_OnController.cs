using Add_On_ConsultaRuc.View;
using Microsoft.VisualBasic;
using System;
using ConsultaRuc;
using Add_On_ConsultaRuc.Model;

namespace Add_On_ConsultaRuc.Controller
{
    public class Add_OnController
    {
        private SAPbouiCOM.Application SBO_Application;
        private SAPbouiCOM.Form oOrderForm;
        private SAPbobsCOM.Company oCompany;
        private SAPbouiCOM.ComboBox oCardType;
        private SAPbouiCOM.EditText txtRuc;

        private string Ruc;
        private SocioNegocio oSocioNegocio;
        private string[] DatosSunat;
        private void setApplication()
        {
            SAPbouiCOM.ISboGuiApi sboGuiApi = null;
            string sConnectionString = null;
            sboGuiApi = new SAPbouiCOM.SboGuiApi();

            sConnectionString = Interaction.Command();
            sboGuiApi.Connect(sConnectionString);
            SBO_Application = sboGuiApi.GetApplication(-1);
        }

        private void SBO_Application_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            // SOCIO DE NEGOCIOS
            if ((pVal.FormType == 134 & pVal.EventType != SAPbouiCOM.BoEventTypes.et_FORM_UNLOAD) & (pVal.Before_Action == true))
            {
                oOrderForm = SBO_Application.Forms.GetFormByTypeAndCount(pVal.FormType, pVal.FormTypeCount);
                if ((pVal.EventType == SAPbouiCOM.BoEventTypes.et_FORM_LOAD) & (pVal.BeforeAction == true))
                {

                    DibujaItems.BtnCoRuc(oOrderForm);

                }

                if (pVal.ItemUID == "btnCRu" & (pVal.EventType == SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED))
                {
                    /*VALIDAMOS LICENCIA*/
                    DateTime dt1, dt2;
                    dt1 = DateTime.Parse("01/01/2021");
                    dt2 = DateTime.Now;
                    TimeSpan ts = dt2 - dt1;
                    if (ts.Days<15)
                    {
                        if ((oOrderForm.Mode == SAPbouiCOM.BoFormMode.fm_ADD_MODE))
                        {

                            try
                            {
                                txtRuc = (SAPbouiCOM.EditText)oOrderForm.Items.Item("41").Specific;
                                oCardType = (SAPbouiCOM.ComboBox)oOrderForm.Items.Item("40").Specific;
                                Ruc = txtRuc.Value;

                                if (Ruc.Length == 11)
                                {
                                    oSocioNegocio = new SocioNegocio();
                                    DatosSunat = ClassDatosSN.GetDatosSN(Ruc);
                                    oSocioNegocio.CardType = oCardType.Value; // [C] cliente [S] Proveedor
                                    oSocioNegocio.ruc = DatosSunat[0];
                                    oSocioNegocio.razonSocial = DatosSunat[1];
                                    oSocioNegocio.estado = DatosSunat[2];
                                    oSocioNegocio.condicion = DatosSunat[3];
                                    oSocioNegocio.direccion = DatosSunat[4];
                                    oSocioNegocio.departamento = DatosSunat[5];
                                    oSocioNegocio.provincia = DatosSunat[6];
                                    oSocioNegocio.distrito = DatosSunat[7];


                                    if (!(SetConnectionContext() == 0))
                                    {
                                        SBO_Application.MessageBox("Failed setting a connection to DI API", 1, "Ok", "", "");
                                        System.Environment.Exit(0); //  Terminating the Add-On Application
                                    }

                                    if (!(ConnectToCompany() == 0))
                                    {
                                        SBO_Application.MessageBox("Failed connecting to the company's Data Base", 1, "Ok", "", "");
                                        System.Environment.Exit(0); //  Terminating the Add-On Application
                                    }


                                    if (oSocioNegocio.estado == "ACTIVO" && oSocioNegocio.condicion == "HABIDO")
                                    {
                                        if (SBO_Application.MessageBox("Esta seguro de [Cear] Socio de negocios:" + oSocioNegocio.razonSocial, 2, "Ok", "Cancelar") == 1)
                                        {
                                            int res = AddSocioNegocioController.CreaSoNe(oCompany, oSocioNegocio);
                                            if (res == 0)
                                            {
                                                SBO_Application.MessageBox("Socio de negocios creado: " + oCompany.CompanyName + Constants.vbNewLine, 1, "Ok", "", "");
                                                oOrderForm.Close();
                                            }
                                            else
                                            {
                                                string LastError = "";
                                                oCompany.GetLastError(out res, out LastError);
                                                SBO_Application.MessageBox(res + "-" + LastError);
                                            }
                                        }

                                    }
                                    else
                                    {
                                        if (SBO_Application.MessageBox("El SN: " + oSocioNegocio.razonSocial + "\r\n" + "Estado: " + oSocioNegocio.estado + " Condición: " + oSocioNegocio.condicion + "\r\n" + "Desea crear SN de todas formas?", 2, "Ok", "Cancelar") == 1)
                                        {
                                            int res = AddSocioNegocioController.CreaSoNe(oCompany, oSocioNegocio);
                                            if (res == 0)
                                            {
                                                SBO_Application.MessageBox("Socio de negocios creado: " + oCompany.CompanyName + Constants.vbNewLine, 1, "Ok", "", "");
                                                oOrderForm.Close();
                                            }
                                            else
                                            {
                                                string LastError = "";
                                                oCompany.GetLastError(out res, out LastError);
                                                SBO_Application.MessageBox(res + "-" + LastError);
                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    SBO_Application.MessageBox("Nùmero de RUC debe de contener 12 caracteres");
                                }

                            }
                            catch (Exception ex)
                            {

                                SBO_Application.MessageBox(ex.Message);
                            }

                        }
                        else
                        {
                            SBO_Application.MessageBox("Esta obcion solo esta activada para [Crear] Socio de negocio.");
                        }
                    }
                    else
                    {
                        SBO_Application.MessageBox("Periodo de prueba [terminado]");
                    }

                }
            }
        }
        private int SetConnectionContext()

                {
                    int setConnectionContextReturn = 0;
                    string sCookie = null;
                    string sConnectionContext = null;
                    oCompany = new SAPbobsCOM.Company();
                    sCookie = oCompany.GetContextCookie();
                    sConnectionContext = SBO_Application.Company.GetConnectionContext(sCookie);
                    setConnectionContextReturn = oCompany.SetSboLoginContext(sConnectionContext);
                    return setConnectionContextReturn;
                }

        private int ConnectToCompany()
        {
            int ConnectToCompanyReturn = 0;
            ConnectToCompanyReturn = oCompany.Connect();
            return ConnectToCompanyReturn;
        }

        public Add_OnController()
        {
            setApplication();
            SBO_Application.ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);

        }
    }
}
