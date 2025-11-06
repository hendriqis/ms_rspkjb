using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using System.Web.UI.HtmlControls;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.CommonLibs.MasterPage
{
    public partial class MPMain : BaseMP
    {
        protected string moduleName = "";
        public List<GetUserMenuAccess> ListMenu { get { return lstMenu; } }
        protected List<GetUserMenuAccess> lstMenu = null;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!Page.IsPostBack)
            {
                if (AppSession.UserLogin == null)
                    Response.Redirect("~/../SystemSetup/Login.aspx");
                moduleName = Helper.GetModuleName();
                string ModuleID = Helper.GetModuleID(moduleName);
                lstMenu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, "IsShowInPullDownMenu = 1");
                //lstMenu = BusinessLayer.GetMenuList(string.Format("ModuleID = '{0}'", ModuleID));
                rptMenu.DataSource = lstMenu.Where(p => p.ParentID == null).OrderBy(p => p.MenuIndex).ToList();
                rptMenu.DataBind();
            }
        }

        protected string GetResolveUrl(string url)
        {
            if (url == "#")
                return "#";
            return ResolveUrl(url);
        }

        protected void rptMenu_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                GetUserMenuAccess obj = (GetUserMenuAccess)e.Item.DataItem;

                Repeater rptMenuLevel2 = (Repeater)e.Item.FindControl("rptMenuLevel2");

                List<GetUserMenuAccess> lst = GetMenuChild(obj.MenuID);
                if (lst.Count > 0)
                {
                    rptMenuLevel2.DataSource = lst;
                    rptMenuLevel2.DataBind();
                }
            }
        }

        protected void rptMenuLevel2_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                GetUserMenuAccess obj = (GetUserMenuAccess)e.Item.DataItem;

                Repeater rptMenuLevel3 = (Repeater)e.Item.FindControl("rptMenuLevel3");

                List<GetUserMenuAccess> lst = GetMenuChild(obj.MenuID);
                if (lst.Count > 0)
                {
                    rptMenuLevel3.DataSource = lst;
                    rptMenuLevel3.DataBind();
                }
            }
        }

        protected void rptMenuLevel3_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                GetUserMenuAccess obj = (GetUserMenuAccess)e.Item.DataItem;

                Repeater rptMenuLevel4 = (Repeater)e.Item.FindControl("rptMenuLevel4");

                List<GetUserMenuAccess> lst = GetMenuChild(obj.MenuID);
                if (lst.Count > 0)
                {
                    rptMenuLevel4.DataSource = lst;
                    rptMenuLevel4.DataBind();
                }
            }
        }

        protected List<GetUserMenuAccess> GetMenuChild(Int32 ParentID)
        {
            return lstMenu.Where(p => p.ParentID == ParentID).OrderBy(p => p.MenuIndex).ToList();
        }

        protected string GetModuleImage()
        {    
            return Helper.GetModuleImage(this, moduleName);
        }

        protected string GetHospitalName()
        {
            return AppSession.UserLogin.HealthcareName;
        }

        protected string GetUserInfo()
        {
            return string.Format("{0}", AppSession.UserLogin.UserFullName);
        }
        protected int GetUserLoginID() {
            return AppSession.UserLogin.UserID;
        }
        protected void cbpCloseWindow_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            moduleName = Helper.GetModuleName().ToLower();
            if (moduleName != "systemsetup") {
                HttpContext.Current.Session.Clear();
                AppSession.ClearSession();
                /* jika ingin logout semua page
                HttpCookie Cookie2 = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_ASPNET_SessionId];
                Cookie2.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(Cookie2);*/
            }
        }
       
    }
}