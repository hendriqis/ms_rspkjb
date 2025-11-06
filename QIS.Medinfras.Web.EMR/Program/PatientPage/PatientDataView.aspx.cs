using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class PatientDataView : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AppSession.UrlReferrer = Request.UrlReferrer.ToString();

            string moduleName = Helper.GetModuleName();
            string ModuleID = Helper.GetModuleID(moduleName);
            string patientPageMenuCode = Constant.MenuCode.EMR.PATIENT_PAGE;

            if (AppSession.IsPatientPageByDepartment)
            {
                switch (AppSession.RegisteredPatient.DepartmentID)
                {
                    case Constant.Facility.EMERGENCY:
                        patientPageMenuCode = Constant.MenuCode.EMR.PATIENT_PAGE_EMERGENCY;
                        break;
                    default:
                        break;
                } 
            }
            string filterExpression = string.Format("ParentCode = '{0}'", patientPageMenuCode);

            List<GetUserMenuAccess> lstMenu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, filterExpression);
            int parentID = (int)lstMenu.Where(p => p.MenuIndex > 0 && (p.DepartmentID.Contains(AppSession.RegisteredPatient.DepartmentID) || string.IsNullOrEmpty(p.DepartmentID))).OrderBy(p => p.MenuIndex).FirstOrDefault().MenuID;

            filterExpression = string.Format("ParentID = {0}", parentID, AppSession.RegisteredPatient.DepartmentID);
            lstMenu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, filterExpression);
            GetUserMenuAccess menu = lstMenu.Where(p => (p.DepartmentID.Contains(AppSession.RegisteredPatient.DepartmentID) || string.IsNullOrEmpty(p.DepartmentID))).OrderBy(p => p.MenuIndex).FirstOrDefault();
            Response.Redirect(Page.ResolveUrl(menu.MenuUrl));
        }
    }
}