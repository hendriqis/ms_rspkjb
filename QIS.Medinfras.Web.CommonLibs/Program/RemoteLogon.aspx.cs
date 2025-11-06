using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using System.Web.Security;
using System.Text;
using QISEncryption;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class RemoteLogon : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Form["id"] != null)
            {
                string[] param = Request.Form["id"].Split('|');
                string[] paramLoginInfo = Encryption.DecryptString(param[0]).Split('|');
                string userName = paramLoginInfo[0];
                string password = paramLoginInfo[1];

                List<vUser> lstUser = BusinessLayer.GetvUserList(string.Format("UserName = '{0}' AND IsDeleted = 0", userName));
                if (lstUser.Count > 0)
                {
                    vUser user = lstUser[0];
                    if (user.Password.Trim() == password)
                    {
                        string healthcareID = param[4];

                        UserLogin userLogin = new UserLogin();
                        userLogin.UserID = user.UserID;
                        userLogin.UserName = user.UserName;
                        userLogin.ParamedicID = user.ParamedicID;
                        if (userLogin.ParamedicID != null && userLogin.ParamedicID > 0)
                        {
                            vParamedicMaster userParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("ParamedicID = {0}", userLogin.ParamedicID))[0];
                            userLogin.UserFullName = userParamedic.ParamedicName;
                            userLogin.SpecialtyID = userParamedic.SpecialtyID;
                            userLogin.DepartmentID = userParamedic.DepartmentID;
                            userLogin.GCParamedicMasterType = userParamedic.GCParamedicMasterType;
                            userLogin.IsPrimaryNurse = userParamedic.IsPrimaryNurse;
                            userLogin.IsRMO = userParamedic.IsRMO;
                            userLogin.IsSpecialist = userParamedic.IsSpecialist;
                        }
                        else
                        {
                            userLogin.UserFullName = user.FullName;
                        }

                        userLogin.HealthcareID = healthcareID;

                        List<UserInRole> lstUserSysAdmin = BusinessLayer.GetUserInRoleList(string.Format("UserID = {0} AND HealthcareID = '{1}' AND RoleID = 1", userLogin.UserID, userLogin.HealthcareID));
                        userLogin.IsSysAdmin = (lstUserSysAdmin.Count > 0);
                        userLogin.HealthcareName = BusinessLayer.GetHealthcare(healthcareID).HealthcareName;

                        AppSession.UserLogin = userLogin;

                        MedicalDiagnosticUserLogin medicalDiagnostic = new MedicalDiagnosticUserLogin();
                        MedicalDiagnosticType er = new MedicalDiagnosticType();
                        er = MedicalDiagnosticType.None;
                        medicalDiagnostic.MedicalDiagnosticType = er;

                        string moduleName = Helper.GetModuleName();
                        string ModuleID = Helper.GetModuleID(moduleName);

                        AppSession.UserLogin.ModuleID = ModuleID;

                        if (ModuleID == Constant.Module.LABORATORY)
                        {
                            medicalDiagnostic.MedicalDiagnosticType = MedicalDiagnosticType.Laboratory;
                            string laboratoryID = BusinessLayer.GetSettingParameter(Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID).ParameterValue;
                            medicalDiagnostic.HealthcareServiceUnitID = BusinessLayer.GetvHealthcareServiceUnitCustomList(string.Format("HealthcareID = '{0}' AND ServiceUnitID = {1} AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, laboratoryID))[0].HealthcareServiceUnitID;
                        }
                        else if (ModuleID == Constant.Module.IMAGING)
                        {
                            medicalDiagnostic.MedicalDiagnosticType = MedicalDiagnosticType.Imaging;
                            string imagingID = BusinessLayer.GetSettingParameter(Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID).ParameterValue;
                            medicalDiagnostic.HealthcareServiceUnitID = BusinessLayer.GetvHealthcareServiceUnitCustomList(string.Format("HealthcareID = '{0}' AND ServiceUnitID = {1} AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, imagingID))[0].HealthcareServiceUnitID;
                        }
                        else if (ModuleID == Constant.Module.RADIOTHERAPHY)
                        {
                            medicalDiagnostic.MedicalDiagnosticType = MedicalDiagnosticType.Radiotheraphy;
                            string serviceUnitID = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RT0001).ParameterValue;
                            medicalDiagnostic.HealthcareServiceUnitID = BusinessLayer.GetvHealthcareServiceUnitCustomList(string.Format("HealthcareID = '{0}' AND HealthcareServiceUnitID = {1} AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, serviceUnitID))[0].HealthcareServiceUnitID;
                        }
                        else if (ModuleID == Constant.Module.NUTRITION)
                        {
                            medicalDiagnostic.MedicalDiagnosticType = MedicalDiagnosticType.Nutrition;
                        }
                        else if (ModuleID == Constant.Module.MEDICAL_DIAGNOSTIC)
                        {
                            medicalDiagnostic.MedicalDiagnosticType = MedicalDiagnosticType.OtherMedicalDiagnostic;
                            List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID, Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID));
                            string laboratoryID = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID).ParameterValue;
                            string imagingID = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID).ParameterValue;
                            List<vHealthcareServiceUnitCustom> lstHealthcareServiceUnit = BusinessLayer.GetvHealthcareServiceUnitCustomList(string.Format("HealthcareID = '{0}' AND ServiceUnitID IN ({1},{2}) AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, laboratoryID, imagingID));
                            medicalDiagnostic.ImagingHealthcareServiceUnitID = lstHealthcareServiceUnit.FirstOrDefault(p => p.ServiceUnitID == Convert.ToInt32(imagingID)).HealthcareServiceUnitID;
                            medicalDiagnostic.LaboratoryHealthcareServiceUnitID = lstHealthcareServiceUnit.FirstOrDefault(p => p.ServiceUnitID == Convert.ToInt32(laboratoryID)).HealthcareServiceUnitID;
                        }

                        AppSession.MedicalDiagnostic = medicalDiagnostic;

                        Response.Redirect("Main.aspx");
                    }
                }
            }
        }
    }
}