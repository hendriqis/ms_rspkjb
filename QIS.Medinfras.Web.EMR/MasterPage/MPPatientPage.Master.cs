using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Web.UI.HtmlControls;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.EMR.MasterPage
{
    public partial class MPPatientPage : BaseMP
    {
        #region Top Charm Bar Menu
        public class TopCharmBarMenu
        {
            public string Title { get; set; }
            public string Postbackurl { get; set; }
            public string Normalsrc { get; set; }
            public string Hoversrc { get; set; }
            public string Selectedsrc { get; set; }
            public string Pressedsrc { get; set; }
            public string Disabledsrc { get; set; }
        }
        #endregion

        public List<GetUserMenuAccess> ListMenu { get { return lstMenu; } }
        protected List<GetUserMenuAccess> lstMenu = null;

        private BasePageContent _basePageList;
        private BasePageContent BasePageList
        {
            get
            {
                if (_basePageList == null)
                    _basePageList = (BasePageContent)Page;
                return _basePageList;
            }
        }

        string menuCode = "";

        protected string urlReferrer = "";
        protected int PageCount = 1;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!Page.IsPostBack)
            {
                if (AppSession.UserLogin == null)
                    Response.Redirect("~/../SystemSetup/Login.aspx");
                urlReferrer = AppSession.UrlReferrer;
                #region Menu
                string moduleName = Helper.GetModuleName();
                string ModuleID = Helper.GetModuleID(moduleName);
                menuCode = BasePageList.OnGetMenuCode();

                string parentCode = Constant.MenuCode.EMR.PATIENT_PAGE;
                string topBarMenuCode = Constant.MenuCode.EMR.INFORMATION;

                if (AppSession.IsPatientPageByDepartment)
                {
                    switch (AppSession.RegisteredPatient.DepartmentID)
                    {
                        case Constant.Facility.EMERGENCY:
                            parentCode = Constant.MenuCode.EMR.PATIENT_PAGE_EMERGENCY;
                            topBarMenuCode = Constant.MenuCode.EMR.PATIENT_PAGE_CHARM_BAR;
                            break;
                        default:
                            break;
                    } 
                }

                lstMenu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("(ParentCode = '{0}' OR ParentID IN (SELECT MenuID FROM Menu WHERE ParentID IN (SELECT MenuID FROM Menu WHERE MenuCode IN ('{0}','{1}'))))", parentCode, Constant.MenuCode.EMR.PATIENT_PAGE_CHARM_BAR));
                GetUserMenuAccess menuInformation = lstMenu.FirstOrDefault(p => p.MenuCode == topBarMenuCode);

                if (menuInformation != null)
                {
                    rptMenuHeader.DataSource = lstMenu.Where(p => p.MenuID != menuInformation.MenuID && p.ParentCode == parentCode).OrderBy(p => p.MenuIndex).ToList();
                    rptMenuHeader.DataBind();
                }
                else
                {
                    rptMenuHeader.DataSource = lstMenu.Where(p => p.ParentCode == parentCode).OrderBy(p => p.MenuIndex).ToList();
                    rptMenuHeader.DataBind();
                }

                GetUserMenuAccess selectedMenu = lstMenu.FirstOrDefault(p => p.MenuCode == menuCode);
                rptMenuChild.DataSource = lstMenu.Where(p => p.ParentID == selectedMenu.ParentID).OrderBy(p => p.MenuIndex).ToList();
                rptMenuChild.DataBind();
                #endregion

                #region Top Charm Bar
                if (menuInformation != null)
                {
                    List<GetUserMenuAccess> lstUserMenuCharmBar = lstMenu.Where(p => p.ParentID == menuInformation.MenuID).OrderBy(p => p.MenuIndex).ToList();
                    List<TopCharmBarMenu> ListTopCharmBarMenu = new List<TopCharmBarMenu>();
                    foreach (GetUserMenuAccess menu in lstUserMenuCharmBar)
                    {
                        TopCharmBarMenu topCharmBar = new TopCharmBarMenu();
                        topCharmBar.Title = menu.MenuCaption;
                        topCharmBar.Postbackurl = menu.MenuUrl;
                        topCharmBar.Normalsrc = Page.ResolveUrl(menu.ImageUrl);
                        topCharmBar.Hoversrc = string.Format("{0}_hover.png", topCharmBar.Normalsrc.Substring(0, topCharmBar.Normalsrc.Length - 4));
                        topCharmBar.Selectedsrc = string.Format("{0}_selected.png", topCharmBar.Normalsrc.Substring(0, topCharmBar.Normalsrc.Length - 4));
                        topCharmBar.Pressedsrc = string.Format("{0}_pressed.png", topCharmBar.Normalsrc.Substring(0, topCharmBar.Normalsrc.Length - 4));
                        topCharmBar.Disabledsrc = string.Format("{0}_disabled.png", topCharmBar.Normalsrc.Substring(0, topCharmBar.Normalsrc.Length - 4));
                        ListTopCharmBarMenu.Add(topCharmBar);
                    }
                    rptTopCharmBarMenu.DataSource = ListTopCharmBarMenu;
                    rptTopCharmBarMenu.DataBind();
                }
                #endregion

                hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();

                vConsultVisit2 entity = BusinessLayer.GetvConsultVisit2List(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID))[0];
                EntityToControl(entity);
            }
        }

        protected void rptMenuHeader_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                GetUserMenuAccess obj = (GetUserMenuAccess)e.Item.DataItem;
                HtmlGenericControl liCaption = (HtmlGenericControl)e.Item.FindControl("liCaption");
                IEnumerable<GetUserMenuAccess> mn = lstMenu.Where(p => p.ParentID == obj.MenuID && p.MenuCode == menuCode);
                if (mn.Count() > 0)
                {
                    liCaption.Attributes.Add("class", "selected");
                }
                List<GetUserMenuAccess> lstMn = lstMenu.Where(p => p.ParentID == obj.MenuID).OrderBy(p => p.MenuIndex).ToList();
                if (lstMn.Count > 0)
                    liCaption.Attributes.Add("url", lstMn[0].MenuUrl);
                else
                    liCaption.Visible = false;

                liCaption.Visible = ((obj.DepartmentID == AppSession.RegisteredPatient.DepartmentID) || string.IsNullOrEmpty(obj.DepartmentID));
            }
        }

        protected void rptMenuChild_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                GetUserMenuAccess obj = (GetUserMenuAccess)e.Item.DataItem;
                HtmlGenericControl liCaption = (HtmlGenericControl)e.Item.FindControl("liCaption");
                if (obj.MenuCode == menuCode)
                    liCaption.Attributes.Add("class", "selected");

                liCaption.Visible = ((obj.DepartmentID == AppSession.RegisteredPatient.DepartmentID) || string.IsNullOrEmpty(obj.DepartmentID));
            }
        }

        private void EntityToControl(vConsultVisit2 entity)
        {
            ctlPatientBanner.InitializePatientBanner(entity);

            //BindDocumentGridView(1, true, ref PageCount);
        }

        private void RefreshAllergyInfo(vConsultVisit2 entity)
        {
            ctlPatientBanner.RefreshAllergyStatus(entity);
        }

        protected string GetModuleImage()
        {
            return Helper.GetModuleImage(this, Helper.GetModuleName());
        }

        protected string GetHospitalName()
        {
            return AppSession.UserLogin.HealthcareName;
        }

        protected string GetUserInfo()
        {
            return string.Format("{0}", AppSession.UserLogin.UserFullName);
        }

        protected void cbpCloseWindow_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            if (Helper.GetModuleName().ToLower() != "systemsetup")
                HttpContext.Current.Session.Clear();
        }

        protected void cbpPatientBannerView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            if (e.Parameter != null && e.Parameter != "")
            {
                vConsultVisit2 entity = BusinessLayer.GetvConsultVisit2List(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID))[0];
                string[] param = e.Parameter.Split('|');
                if (param[0] == "allergy")
                {
                    RefreshAllergyInfo(entity); 
                }
            }
       }

        private void BindDocumentGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            //string filterExpression = string.Format("MRN = {0} AND GCFileType != '{1}'", AppSession.RegisteredPatient.MRN, Constant.FileType.IMAGE);

            //if (isCountPageCount)
            //{
            //    int rowCount = BusinessLayer.GetvPatientDocumentRowCount(filterExpression);
            //    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            //}

            //List<vPatientDocument> lstEntity = BusinessLayer.GetvPatientDocumentList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "DocumentDate DESC");
            //grdView.DataSource = lstEntity;
            //grdView.DataBind();
        }

        //protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        //{
        //    int pageCount = 1;
        //    string result = "";
        //    if (e.Parameter != null && e.Parameter != "")
        //    {
        //        string[] param = e.Parameter.Split('|');
        //        if (param[0] == "changepage")
        //        {
        //            BindDocumentGridView(Convert.ToInt32(param[1]), false, ref pageCount);
        //            result = "changepage";
        //        }
        //        else // refresh
        //        {

        //            BindDocumentGridView(1, true, ref pageCount);
        //            result = "refresh|" + pageCount;
        //        }
        //    }

        //    ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
        //    panel.JSProperties["cpResult"] = result;
        //}

        //protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    //if (e.Row.RowType == DataControlRowType.DataRow)
        //    //{
        //    //    vPatientDocument item = e.Row.DataItem as vPatientDocument;
        //    //    var hyperLink = e.Row.FindControl("lnkActionLog") as HyperLink;
        //    //    if (hyperLink != null)
        //    //    {
        //    //        hyperLink.NavigateUrl = string.Format(@"{0}/{1}/{2}", AppConfigManager.QISVirtualDirectory, AppConfigManager.QISPatientDocumentsPath.Replace("#MRN", AppSession.RegisteredPatient.MedicalNo), item.FileName);
        //    //        hyperLink.Target = "_blank";
        //    //    }
        //    //}
        //}
    }
}