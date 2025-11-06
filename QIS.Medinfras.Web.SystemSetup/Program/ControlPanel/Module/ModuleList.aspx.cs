using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class ModuleList : BasePageList
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.MODULE;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            hdnID.Value = keyValue;
            List<Module> lstEntity = BusinessLayer.GetModuleList("");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            
        }

        protected override bool OnAddRecord(ref string url, ref string errMessage)
        {
            url = ResolveUrl("~/Program/ControlPanel/Module/ModuleEntry.aspx");
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                url = ResolveUrl(string.Format("~/Program/ControlPanel/Module/ModuleEntry.aspx?id={0}", hdnID.Value));
                return true;
            }
            return false;
        }
    }
}