using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Web.UI.HtmlControls;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.EMR.MasterPage
{
    public partial class MPPatientDataPage : BaseMP
    {
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
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!Page.IsPostBack)
            {
                if (AppSession.UserLogin == null)
                    Response.Redirect("~/../SystemSetup/Login.aspx");
                urlReferrer = Session["_UrlReferrerPatientDataPage"].ToString();
                #region Menu
                string moduleName = Helper.GetModuleName();
                string ModuleID = Helper.GetModuleID(moduleName);
                menuCode = BasePageList.OnGetMenuCode();

                lstMenu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("(ParentCode = '{0}' OR ParentID IN (SELECT MenuID FROM Menu WHERE ParentID = (SELECT MenuID FROM Menu WHERE MenuCode = '{0}')))", Constant.MenuCode.EMR.PATIENT_EMR_VIEW));
                GetUserMenuAccess menuInformation = lstMenu.FirstOrDefault(p => p.MenuCode == Constant.MenuCode.EMR.INFORMATION);

                if (menuInformation != null)
                {
                    rptMenuHeader.DataSource = lstMenu.Where(p => p.MenuID != menuInformation.MenuID && p.ParentCode == Constant.MenuCode.EMR.PATIENT_EMR_VIEW).OrderBy(p => p.MenuIndex).ToList();
                    rptMenuHeader.DataBind();
                }
                else
                {
                    rptMenuHeader.DataSource = lstMenu.Where(p => p.ParentCode == Constant.MenuCode.EMR.PATIENT_EMR_VIEW).OrderBy(p => p.MenuIndex).ToList();
                    rptMenuHeader.DataBind();
                }
                #endregion
                GetUserMenuAccess selectedMenu = lstMenu.FirstOrDefault(p => p.MenuCode == menuCode);
                rptMenuChild.DataSource = lstMenu.Where(p => p.ParentID == selectedMenu.ParentID).OrderBy(p => p.MenuIndex).ToList();
                rptMenuChild.DataBind();

                vPatientBanner entity = BusinessLayer.GetvPatientBannerList(string.Format("MRN = {0}", AppSession.PatientDetail.MRN))[0];
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

            }
        }

        private void EntityToControl(vPatientBanner entity)
        {
            hdnPatientGender.Value = entity.GCSex;
            imgPatientProfilePicture.Src = entity.PatientImageUrl;
            lblPatientName.InnerHtml = entity.PatientName;

            lblMRN.InnerHtml = entity.MedicalNo;
            lblDOB.InnerHtml = entity.DateOfBirthInString;
            BasePage page = (BasePage)this.Page;
            lblPatientAge.InnerHtml = Helper.GetPatientAge(page.GetWords(), entity.DateOfBirth);
            lblGender.InnerHtml = entity.Sex;
            lblAllergy.InnerHtml = entity.PatientAllergy;
        }
    }
}