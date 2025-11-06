using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.MasterPage
{
    public partial class MPEntry : BaseMP
    {
        private BasePageEntry _basePageEntry;
        private BasePageEntry BasePageEntry
        {
            get
            {
                if (_basePageEntry == null)
                    _basePageEntry = (BasePageEntry)Page;
                return _basePageEntry;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.Form["id"] != null)
                    hdnListID.Value = Request.Form["id"].ToString();
                if (Request.Form["txtSearchView"] != null)
                    hdnListTextSearch.Value = Request.Form["txtSearchView"].ToString();
                if (Request.Form["filterExpression"] != null)
                    hdnListFilterExpression.Value = Request.Form["filterExpression"].ToString();

                hdnIsAdd.Value = BasePageEntry.IsAdd ? "1" : "0";
                string menuCode = BasePageEntry.OnGetMenuCode();
                bool IsAllowSaveAndNew, IsAllowSaveAndClose;
                IsAllowSaveAndNew = IsAllowSaveAndClose = true;
                BasePageEntry.SetCRUDMode(ref IsAllowSaveAndNew, ref IsAllowSaveAndClose);
                string CRUDMode = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == menuCode).CRUDMode;

                if (!IsAllowSaveAndNew) CRUDMode = CRUDMode.Replace("C", "");
                if (!IsAllowSaveAndClose) CRUDMode = CRUDMode.Replace("U", "").Replace("C", "");

                if (!CRUDMode.Contains("C"))
                    btnMPEntrySaveNew.Style.Add("display", "none");
                if (!CRUDMode.Contains("C") && !CRUDMode.Contains("U"))
                    btnMPEntrySaveClose.Style.Add("display", "none");
            }
        }

        protected void cbpMPEntryContent_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string param = e.Parameter;
            if (param == "refresh")
                BasePageEntry.RefreshControl();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpParam"] = param;
        }

        protected void cbpMPEntryProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string retval = "";
            string[] param = e.Parameter.Split('|');
            if (param[0] == "savenew" || param[0] == "saveclose")
            {
                bool isAdd = (param[1] == "1");
                result = param[0] + "|";
                BasePageEntry.OnBtnSaveClick(ref result, ref retval, isAdd);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = retval;
        }
    }
}