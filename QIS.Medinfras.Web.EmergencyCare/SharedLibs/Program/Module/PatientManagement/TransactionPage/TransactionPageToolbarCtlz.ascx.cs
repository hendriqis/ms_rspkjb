using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using System.Web.UI.HtmlControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class TransactionPageToolbarCtl : BaseUserControlCtl
    {
        string menuCode = "";
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                string moduleName = Helper.GetModuleName();
                string ModuleID = Helper.GetModuleID(moduleName);

                menuCode = ((BasePageContent)Page).OnGetMenuCode();

                string parentCode = "";
                switch (ModuleID)
                {
                    case "OP": parentCode = Constant.MenuCode.Outpatient.PATIENT_TRANSACTION_PAGE; break;
                    case "IP": parentCode = Constant.MenuCode.Inpatient.PATIENT_TRANSACTION_PAGE; break;
                    case "ER": parentCode = Constant.MenuCode.EmergencyCare.PATIENT_TRANSACTION_PAGE; break;
                    case "LB": parentCode = Constant.MenuCode.Laboratory.PATIENT_TRANSACTION_PAGE; break;
                    case "IS": parentCode = Constant.MenuCode.Imaging.PATIENT_TRANSACTION_PAGE; break;
                    case "MD": parentCode = Constant.MenuCode.MedicalDiagnostic.PATIENT_TRANSACTION_PAGE; break;
                }

                List<GetUserMenuAccess> lstMenu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("ParentCode = '{0}'", parentCode)).OrderBy(p => p.MenuIndex).ToList();
                rptHeader.DataSource = lstMenu;
                rptHeader.DataBind();
            }
        }

        protected void rptHeader_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                GetUserMenuAccess obj = (GetUserMenuAccess)e.Item.DataItem;
                HtmlGenericControl liCaption = (HtmlGenericControl)e.Item.FindControl("liCaption");                
                if (obj.MenuCode == menuCode)
                    liCaption.Attributes.Add("class", "selected");
            }
        }
    }
}