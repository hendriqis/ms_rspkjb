using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class MPProcessPopupCtl : BaseMPPopupEntry
    {
        private BaseProcessPopupCtl ctl = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            bool IsAllowAdd = false;
            bool IsUsingProcessButton = false;
            ctl = (BaseProcessPopupCtl)pnlEntryPopup.Controls[0];            
            ctl.SetToolbarVisibility(ref IsAllowAdd);
            ctl.SetProcessButtonVisibility(ref IsUsingProcessButton);
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
            if (e.Parameter == "refresh")
            {
                ctl.OnRefreshCtlControl();
            }
        }

        protected void cbpMPEntryPopupProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string retval = "";
            string param = e.Parameter;
            if (param == "process")
            {
                bool isAdd = (hdnIsAdd.Value.ToString() == "1");
                ctl.OnBtnProcessClick(ref result, ref retval);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = retval;
        }

        public string GetLabel(string code)
        {
            return ctl.GetLabel(code);
        }

        public string GetPopupTitle()
        {
            return ctl.PopupTitle;
        }
    }
}