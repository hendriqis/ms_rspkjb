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

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class RevenueSharingToolbarCtl : BaseUserControlCtl
    {
        string menuCode = "";
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                menuCode = ((BasePageContent)Page).OnGetMenuCode();

                List<GetUserMenuAccess> lstMenu = BusinessLayer.GetUserMenuAccess(Constant.Module.FINANCE, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("ParentCode = '{0}'", Constant.MenuCode.Finance.PARAMEDIC_LIST)).OrderBy(p => p.MenuIndex).ToList();
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