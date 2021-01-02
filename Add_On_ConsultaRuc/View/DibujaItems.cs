
namespace Add_On_ConsultaRuc.View
{
    public class DibujaItems
    {
        private static SAPbouiCOM.Item oItem;
        private static SAPbouiCOM.Button oButton;
        private static SAPbouiCOM.StaticText oStaticText;
        public static void BtnAddSN(SAPbouiCOM.Form oForm)
        {
            oItem = oForm.Items.Add("btnAddSN", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
            oItem.Top = 78;
            oItem.Height = 20;
            oItem.Width = 100;
            oItem.Left = 300;
            oButton = ((SAPbouiCOM.Button)(oItem.Specific));
            oButton.Caption = "Crear Modo Rapido";
        }

        public static void BtnCoRuc(SAPbouiCOM.Form oForm)
        {
            oItem = oForm.Items.Add("btnCRu", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
            oItem.Top = 55;
            oItem.Height = 20;
            oItem.Width = 80;
            oItem.Left = 300;
            oButton = ((SAPbouiCOM.Button)(oItem.Specific));
            oButton.Caption = "Consulta RUC";
        }

        public static void stxtDire(SAPbouiCOM.Form oForm)
        {
            oItem = oForm.Items.Add("stxtDire", SAPbouiCOM.BoFormItemTypes.it_STATIC);
            oItem.Top = 300;
            oItem.Height = 20;
            oItem.Width = 200;
            oItem.Left = 290;
            oItem.FontSize = 14;
            oStaticText = ((SAPbouiCOM.StaticText)(oItem.Specific));
            oStaticText.Caption = "";
            oItem.FromPane = 7;
            oItem.ToPane = 7;
        }
    }
}
