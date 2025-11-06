using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using QIS.Medinfras.Web.CommonLibs.Service;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class VoidPhysicianDischarge : BasePageTrx
    {
        private string refreshGridInterval = "";
        protected int PageCount = 1;
        protected int CurrPage = 1;
        private const string LABORATORY = "LABORATORY";
        private const string IMAGING = "IMAGING";

        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.CANCEL_PHYSICIAN_DISCHARGE;
                case Constant.Facility.OUTPATIENT: return Constant.MenuCode.Outpatient.CANCEL_PHYSICIAN_DISCHARGE;
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.CANCEL_PHYSICIAN_DISCHARGE;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.CANCEL_PHYSICIAN_DISCHARGE;
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.CANCEL_PHYSICIAN_DISCHARGE;
                    return Constant.MenuCode.MedicalDiagnostic.CANCEL_PHYSICIAN_DISCHARGE;
                default: return Constant.MenuCode.Inpatient.CANCEL_PHYSICIAN_DISCHARGE;
            }
        }
        private GetUserMenuAccess menu;

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected string GetRefreshGridInterval()
        {
            return refreshGridInterval;
        }

        protected override void InitializeDataControl()
        {
            refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;

            hdnDepartmentID.Value = Page.Request.QueryString["id"];
            if (hdnDepartmentID.Value == LABORATORY || hdnDepartmentID.Value == IMAGING)
                hdnDepartmentID.Value = Constant.Facility.DIAGNOSTIC;

            hdnParam.Value = hdnDepartmentID.Value;

            string filterExpression = "";

            if (hdnDepartmentID.Value == Constant.Facility.EMERGENCY)
            {
                hdnHealthCareServiceUnitID.Value = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, Constant.Facility.EMERGENCY))[0].HealthcareServiceUnitID.ToString();

                filterExpression = string.Format("IsUsingRegistration = 1 AND HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, hdnDepartmentID.Value);
            }
            else if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC && AppSession.MedicalDiagnostic.MedicalDiagnosticType != MedicalDiagnosticType.OtherMedicalDiagnostic)
            {
                hdnHealthCareServiceUnitID.Value = AppSession.MedicalDiagnostic.HealthcareServiceUnitID.ToString();

                filterExpression = string.Format("IsUsingRegistration = 1 AND HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, hdnDepartmentID.Value); 
                
                if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                {
                    filterExpression += string.Format(" AND HealthcareServiceUnitID IN ({0})", AppSession.MedicalDiagnostic.HealthcareServiceUnitID);
                }
                else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                {
                    filterExpression += string.Format(" AND IsLaboratoryUnit = 1");
                }
            }
            else
            {
                int serviceUnitUserCount = BusinessLayer.GetvServiceUnitUserRowCount(string.Format("DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2}", hdnDepartmentID.Value, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID));

                filterExpression = string.Format("IsUsingRegistration = 1 AND HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, hdnDepartmentID.Value);
                if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC)
                    filterExpression += string.Format(" AND HealthcareServiceUnitID NOT IN ({0},{1}) AND IsLaboratoryUnit = 0", AppSession.MedicalDiagnostic.ImagingHealthcareServiceUnitID, AppSession.MedicalDiagnostic.LaboratoryHealthcareServiceUnitID);
                if (serviceUnitUserCount > 0)
                    filterExpression += string.Format(" AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vServiceUnitUser WHERE DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2})", hdnDepartmentID.Value, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID);

            }

            List<vHealthcareServiceUnit> lstServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression);
            lstServiceUnit = lstServiceUnit.OrderBy(unit => unit.ServiceUnitName).ToList();
            lstServiceUnit.Insert(0, new vHealthcareServiceUnit { HealthcareServiceUnitID = 0, ServiceUnitName = "" });
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
            cboServiceUnit.SelectedIndex = 0;

            txtDischargeDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            hdnIsBridgingToAplicares.Value = AppSession.IsBridgingToAPLICARES ? "1" : "0";
            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
                AppSession.UserLogin.HealthcareID,
                Constant.SettingParameter.IP_ICU_SERVICE_UNIT_ID,
                Constant.SettingParameter.IP_NICU_SERVICE_UNIT_ID,
                Constant.SettingParameter.IP_PICU_SERVICE_UNIT_ID));
            hdnHealthcareServiceUnitICUID.Value = lstSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.IP_ICU_SERVICE_UNIT_ID).FirstOrDefault().ParameterValue;
            hdnHealthcareServiceUnitNICUID.Value = lstSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.IP_NICU_SERVICE_UNIT_ID).FirstOrDefault().ParameterValue;
            hdnHealthcareServiceUnitPICUID.Value = lstSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.IP_PICU_SERVICE_UNIT_ID).FirstOrDefault().ParameterValue;

            BindGridView();
            MPTrx master = (MPTrx)Master;
            menu = ((MPMain)master.Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());
            string CRUDMode = menu.CRUDMode;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowNextPrev = IsAllowSave = IsAllowVoid = false;
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("GCVisitStatus IN ('{0}') AND CONVERT(VARCHAR(8),PhysicianDischargedDate,112) = '{1}'",
                            Constant.VisitStatus.PHYSICIAN_DISCHARGE,
                            Helper.GetDatePickerValue(txtDischargeDate).ToString(Constant.FormatString.DATE_FORMAT_112));

            if (cboServiceUnit.Value.ToString() != "0" && cboServiceUnit.Value != null)
            {
                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", cboServiceUnit.Value);
            }
            else
            {
                filterExpression += string.Format(" AND DepartmentID = '{0}'", hdnParam.Value);
            }

            if (hdnFilterExpressionQuickSearch.Value != "Search" && hdnFilterExpressionQuickSearch.Value != "")
            {
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            }

            filterExpression += " ORDER BY RegistrationID";

            List<vConsultVisit1> lstEntity = BusinessLayer.GetvConsultVisit1List(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpCancelPatientDischarge_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            IDbContext ctx = DbFactory.Configure(true);

            BedDao entityBedDao = new BedDao(ctx);
            RegistrationDao entityRegistrationDao = new RegistrationDao(ctx);
            ConsultVisitDao entityConsultVisitDao = new ConsultVisitDao(ctx);
            PatientAccompanyDao entityAccompanyDao = new PatientAccompanyDao(ctx);

            string result = "success|";

            try
            {
                List<ConsultVisit> lstConsultVisit = BusinessLayer.GetConsultVisitList(string.Format("VisitID IN ({0})", hdnSelectedVisit.Value), ctx);
                
                List<ConsultVisit> lstCheckIsMultiVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID IN ({0}) AND IsMainVisit = 0 AND GCVisitStatus != '{1}'", hdnSelectedRegistration.Value, Constant.VisitStatus.CANCELLED), ctx);
                
                string resultReg = "success|";

                foreach (ConsultVisit consultVisit in lstConsultVisit)
                {
                    if (consultVisit.GCVisitStatus == Constant.VisitStatus.PHYSICIAN_DISCHARGE)
                    {
                        int registrationID = consultVisit.RegistrationID;

                        consultVisit.GCVisitStatus = Constant.VisitStatus.RECEIVING_TREATMENT;
                        consultVisit.LastUpdatedBy = AppSession.UserLogin.UserID;
                        consultVisit.PhysicianDischargedBy = null;
                        consultVisit.PhysicianDischargedDate = Convert.ToDateTime(Constant.ConstantDate.DEFAULT_NULL);
                        consultVisit.PhysicianDischargeOrderDate = Convert.ToDateTime(Constant.ConstantDate.DEFAULT_NULL);
                        consultVisit.PhysicianDischargeOrderTime = null;
                        consultVisit.GCDischargeMethod = null;
                        consultVisit.GCDischargeCondition = null;

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityConsultVisitDao.Update(consultVisit);

                        bool isMultiVisit = lstCheckIsMultiVisit.Count > 0;
                        if (!isMultiVisit)
                        {
                            Registration registration = entityRegistrationDao.Get(Convert.ToInt32(registrationID));
                            registration.GCRegistrationStatus = Constant.VisitStatus.RECEIVING_TREATMENT;
                            registration.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityRegistrationDao.Update(registration);
                        }
                    }
                    else
                    {
                        resultReg = "fail|Tidak dapat dibatalkan pulang karena status registrasi sudah berubah. Harap refresh halaman ini.";
                        break;
                    }
                }

                result = resultReg;

                if (result.Contains("success"))
                {
                    ctx.CommitTransaction();
                }
                else
                {
                    ctx.RollBackTransaction();
                }
            }
            catch (Exception ex)
            {
                result = string.Format("fail|{0}", ex.Message);
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}