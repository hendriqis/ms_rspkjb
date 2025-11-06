using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientEMRView : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["_UrlReferrerPatientDataPage"] = Request.UrlReferrer.ToString();

            string moduleName = Helper.GetModuleName();
            string ModuleID = Helper.GetModuleID(moduleName);

            string parentCode = Constant.MenuCode.EMR.PATIENT_EMR_VIEW;

            switch (ModuleID)
            {
                //case Constant.Facility.MEDICAL_CHECKUP: return Constant.MenuCode.MedicalCheckup.REGISTRATION;
                case Constant.Module.INPATIENT: parentCode = Constant.MenuCode.Inpatient.MEDICAL_RECORD_VIEW; break;
                case Constant.Module.EMERGENCY: parentCode = Constant.MenuCode.EmergencyCare.MEDICAL_RECORD_VIEW; break;
                case Constant.Module.OUTPATIENT: parentCode = Constant.MenuCode.Outpatient.MEDICAL_RECORD_VIEW; break;
                case Constant.Module.MEDICAL_DIAGNOSTIC: parentCode = Constant.MenuCode.MedicalDiagnostic.MEDICAL_RECORD_VIEW; break;
                case Constant.Module.MEDICAL_CHECKUP: parentCode = Constant.MenuCode.MedicalCheckup.MEDICAL_RECORD_VIEW; break;
                default: parentCode = Constant.MenuCode.EMR.PATIENT_EMR_VIEW; break;
            }

            string filterExpression = string.Format("ParentCode = '{0}'", parentCode);
            List<GetUserMenuAccess> lstMenu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, filterExpression);
            int parentID = (int)lstMenu.Where(p => p.MenuIndex > 0).OrderBy(p => p.MenuIndex).FirstOrDefault().MenuID;

            filterExpression = string.Format("ParentID = {0}", parentID);
            lstMenu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, filterExpression);
            GetUserMenuAccess menu = lstMenu.OrderBy(p => p.MenuIndex).FirstOrDefault();
            Response.Redirect(Page.ResolveUrl(menu.MenuUrl));
        }
    }
}