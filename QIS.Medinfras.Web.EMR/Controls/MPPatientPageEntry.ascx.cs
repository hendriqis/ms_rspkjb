using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.EMR.Controls
{
    public partial class MPPatientPageEntry : BaseMPPopupEntry
    {
        private BasePagePatientPageEntryCtl ctl = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            ctl = (BasePagePatientPageEntryCtl)pnlEntryPopup.Controls[0];
        }

        public override void SetIsAdd(bool IsAdd)
        {
            if (IsAdd)
                hdnIsAdd.Value = "1";
            else
                hdnIsAdd.Value = "0";
        }

        public override Control GetPanelEntryPopup()
        {
            return pnlEntryPopup;
        }

        protected void cbpMPEntryPopupContent_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            ctl.ReInitControl();
        }

        protected void cbpMPEntryPopupProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string retval = "";
            string param = e.Parameter;
            if (param == "save")
            {
                bool isAdd = (hdnIsAdd.Value.ToString() == "1");
                ctl.OnBtnSaveClick(ref result, isAdd, ref retval);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = retval;
        }

        public string GetLabel(string code)
        {
            return ctl.GetLabel(code);
        }
    }
}