using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using System.Web.UI.HtmlControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PhysicalExaminationToolbarCtl : BaseUserControlCtl
    {
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
        private string menuCode = "";

        protected override void OnLoad(EventArgs e)
        {
            string moduleName = Helper.GetModuleName();
            string ModuleID = Helper.GetModuleID(moduleName);

            menuCode = BasePageList.OnGetMenuCode();
            //lstMenu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("(ParentCode = '{0}' OR ParentID IN (SELECT MenuID FROM Menu WHERE ParentID = (SELECT MenuID FROM Menu WHERE MenuCode = '{0}')))", Constant.MenuCode.EmergencyCare.PATIENT_EXAMINATION));
            List<GetUserMenuAccess> lstMenu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("ParentID IN (SELECT ParentID FROM Menu WHERE MenuCode = '{0}')", menuCode)).OrderBy(p => p.MenuIndex).ToList();
            rptMenuHeader.DataSource = lstMenu;
            rptMenuHeader.DataBind();

            vConsultVisit entity = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            hdnRegistrationID.Value = entity.RegistrationID.ToString();
            hdnVisitID.Value = entity.VisitID.ToString();
            hdnDepartmentID.Value = entity.DepartmentID;

            List<vPatientVisitNote> lstEntity = BusinessLayer.GetvPatientVisitNoteList(string.Format("VisitID = {0} AND IsDeleted = 0", hdnVisitID.Value));
            if (lstEntity.Count == 0)
            {
                divVisitNote.Attributes.Add("style", "display:none");
            }
        }

        protected void rptMenuHeader_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                GetUserMenuAccess obj = (GetUserMenuAccess)e.Item.DataItem;
                HtmlGenericControl liCaption = (HtmlGenericControl)e.Item.FindControl("liCaption");             
                if (obj.MenuCode.Equals(menuCode))
                {
                    liCaption.Attributes.Add("class", "selected");
                }
                liCaption.Attributes.Add("url", obj.MenuUrl);
            }
        }
    }
}