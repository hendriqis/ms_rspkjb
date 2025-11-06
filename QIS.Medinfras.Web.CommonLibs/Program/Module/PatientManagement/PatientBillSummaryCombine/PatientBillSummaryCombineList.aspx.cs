using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Service;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;
using System.Text;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientBillSummaryCombineList : BasePageRegisteredPatient
    {
        string id = "bs";
        protected int PageCountDate = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.FN_COMBINE_PATIENT_BILLING;
        }

        private GetUserMenuAccess menu;
        public override bool IsShowRightPanel()
        {
            return true;
        }
        protected String GetMenuCaption()
        {
            if (menu != null)
                return GetLabel(menu.MenuCaption);
            return "";
        }
        protected string GetRefreshGridInterval()
        {
            return refreshGridInterval;
        }
        private string refreshGridInterval = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;

                List<GetServiceUnitUserList> lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.Facility.OUTPATIENT, "IsUsingRegistration = 1");
                StringBuilder sbLstHealthcareServiceUnitID = new StringBuilder();
                foreach (GetServiceUnitUserList serviceUnit in lstServiceUnit)
                {
                    if (sbLstHealthcareServiceUnitID.ToString() != "")
                        sbLstHealthcareServiceUnitID.Append(",");
                    sbLstHealthcareServiceUnitID.Append(serviceUnit.HealthcareServiceUnitID.ToString());
                }
                hdnLstHealthcareServiceUnitID.Value = sbLstHealthcareServiceUnitID.ToString();
                lstServiceUnit.Insert(0, new GetServiceUnitUserList { HealthcareServiceUnitID = 0, ServiceUnitName = "" });

                List<Department> lstDept = BusinessLayer.GetDepartmentList(string.Format("IsActive = 1 AND IsHasRegistration = 1"));

                Department dept = new Department();
                dept.DepartmentID = "ALL";
                dept.DepartmentName = "ALL";
                lstDept.Add(dept);

                lstDept = lstDept.OrderBy(lst => lst.TabOrder).ToList();
                Methods.SetComboBoxField<Department>(cboDepartment, lstDept.OrderBy(t => t.DepartmentID).ToList(), "DepartmentName", "DepartmentID");
                cboDepartment.SelectedIndex = 0;

                List<StandardCode> lstFilterDate = new List<StandardCode>();
                StandardCode sc = new StandardCode();
                sc.StandardCodeID = "1";
                sc.StandardCodeName = "Tanggal Pendaftaran";

                StandardCode sc2 = new StandardCode();
                sc2.StandardCodeID = "2";
                sc2.StandardCodeName = "Tanggal Pulang";

                lstFilterDate.Add(sc);
                lstFilterDate.Add(sc2);

                Methods.SetComboBoxField<StandardCode>(cboDateFilter, lstFilterDate, "StandardCodeName", "StandardCodeID");
                cboDateFilter.SelectedIndex = 0;

                txtDateFrom.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtDateTo.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                SettingControlProperties();
                grdRegisteredOutpatientPatient.InitializeControl();

                Helper.SetControlEntrySetting(txtDateFrom, new ControlEntrySetting(true, true, true), "mpPatientList");
                Helper.SetControlEntrySetting(txtDateTo, new ControlEntrySetting(true, true, true), "mpPatientList");
                Helper.SetControlEntrySetting(hdnHealthcareServiceUnitID, new ControlEntrySetting(true, true, true), "mpPatientList");

                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

                txtBarcodeEntry.Focus();

                GetSettingParameter();
            }
        }

        private void SettingControlProperties()
        {
            if (AppSession.LastContentVisitListOP != null)
            {
                LastContentVisitListOP lc = AppSession.LastContentVisitListOP;
                txtDateFrom.Text = lc.RegistrationDate;
                txtDateTo.Text = lc.RegistrationDate2;
                hdnPhysicianID.Value = lc.ParamedicID.ToString();
                ParamedicMaster pm = BusinessLayer.GetParamedicMaster(lc.ParamedicID);
                if (pm != null)
                {
                    txtPhysicianCode.Text = pm.ParamedicCode;
                    txtPhysicianName.Text = pm.FullName;
                }
                txtBarcodeEntry.Text = lc.MedicalNo;
                hdnQuickText.Value = lc.QuickText.Equals("Search") ? "" : lc.QuickText;
                hdnFilterExpressionQuickSearch.Value = lc.QuickFilterExpression;
            }
        }

        public override void LoadAllWords()
        {
            LoadWords();
        }

        public override string GetFilterExpression()
        {
            string id = Page.Request.QueryString["id"];
            string filterExpression = string.Empty;

            filterExpression += string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}')",
                    Constant.VisitStatus.OPEN, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED);

            if (cboDateFilter.Value.ToString() == "1")
            {
                filterExpression += string.Format(" AND (VisitDate BETWEEN '{0}' AND '{1}')", Helper.GetDatePickerValue(txtDateFrom).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtDateTo).ToString(Constant.FormatString.DATE_FORMAT_112));
            }
            else
            {
                filterExpression += string.Format(" AND (DischargeDate BETWEEN '{0}' AND '{1}')", Helper.GetDatePickerValue(txtDateFrom).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtDateTo).ToString(Constant.FormatString.DATE_FORMAT_112));
            }

            if (cboDepartment.Value.ToString() != "ALL")
            {
                filterExpression += string.Format(" AND DepartmentID = '{0}'", cboDepartment.Value.ToString());
            }

            if (!String.IsNullOrEmpty(hdnHealthcareServiceUnitID.Value.ToString()))
            {
                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", hdnHealthcareServiceUnitID.Value);
            }

            if (txtPhysicianCode.Text != "")
                filterExpression += string.Format(" AND ParamedicID = {0}", hdnPhysicianID.Value);

            if (hdnFilterExpressionQuickSearch.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);

            if (hdnSearchBarcodeNoRM.Value != "" && hdnSearchBarcodeNoRM.Value != null && hdnSearchBarcodeNoRM.Value != "00-00-00-00")
            {
                filterExpression += string.Format(" AND MedicalNo = '{0}'", hdnSearchBarcodeNoRM.Value);
            }

            return filterExpression;
        }

        private void GetSettingParameter()
        {
            List<SettingParameter> lstSetPar = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IS_OUTPATIENT_REGISTRATION_AUTOMATICALLY_CHECKED_IN));
            List<SettingParameterDt> lstSetParDt = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')", Constant.SettingParameter.IS_BRIDGING_TO_GATEWAY, Constant.SettingParameter.PROVIDER_GATEWAY_SERVICE, Constant.SettingParameter.OP_IS_USING_CALL_PATIENT_FEATURE, Constant.SettingParameter.OP_IS_USING_CLINIC_SERVICE_FEATURE, Constant.SettingParameter.FN_KONTROL_KODE_BIAYA_ADMINISTRASI_RJ, Constant.SettingParameter.FN_KODE_BIAYA_ADMINISTRASI_RJ_INSTANSI, Constant.SettingParameter.FN_KONTROL_BIAYA_ADM_PASIEN_RJ, Constant.SettingParameter.FN_KODE_BIAYA_ADMINISTRASI, Constant.SettingParameter.FN_KONTROL_BIAYA_KARTU, Constant.SettingParameter.FN_KODE_PELAYANAN_KARTU, Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI, Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM));
            hdnIsBridgingToGateway.Value = lstSetParDt.Where(t => t.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_GATEWAY).FirstOrDefault().ParameterValue;
            hdnProviderGatewayService.Value = lstSetParDt.Where(t => t.ParameterCode == Constant.SettingParameter.PROVIDER_GATEWAY_SERVICE).FirstOrDefault().ParameterValue;
            hdnIsAutomaticallyCheckedIn.Value = lstSetPar.Where(t => t.ParameterCode == Constant.SettingParameter.IS_OUTPATIENT_REGISTRATION_AUTOMATICALLY_CHECKED_IN).FirstOrDefault().ParameterValue;
            hdnIsUsingPatientCall.Value = lstSetParDt.Where(t => t.ParameterCode == Constant.SettingParameter.OP_IS_USING_CALL_PATIENT_FEATURE).FirstOrDefault().ParameterValue;
            hdnIsUsingClinicService.Value = lstSetParDt.Where(t => t.ParameterCode == Constant.SettingParameter.OP_IS_USING_CLINIC_SERVICE_FEATURE).FirstOrDefault().ParameterValue;
            hdnIsControlAdministrationCharges.Value = lstSetParDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_KONTROL_KODE_BIAYA_ADMINISTRASI_RJ).ParameterValue;
            hdnChargeCodeAdministrationForInstansi.Value = lstSetParDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_KODE_BIAYA_ADMINISTRASI_RJ_INSTANSI).ParameterValue;
            hdnIsControlAdmCost.Value = lstSetParDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_KONTROL_BIAYA_ADM_PASIEN_RJ).ParameterValue;
            hdnAdminID.Value = lstSetParDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_KODE_BIAYA_ADMINISTRASI).ParameterValue;
            hdnIsControlPatientCardPayment.Value = lstSetParDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_KONTROL_BIAYA_KARTU).ParameterValue;
            hdnItemCardFee.Value = lstSetParDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_KODE_PELAYANAN_KARTU).ParameterValue;
            hdnServiceUntIDLab.Value = lstSetParDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).ParameterValue;
            hdnServiceUntIDRadiologi.Value = lstSetParDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).ParameterValue;
        }

        public override void OnGrdRowClick(string transactionNo)
        {
            vConsultVisit4 entity = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", transactionNo))[0];
            Response.Redirect(GetResponseRedirectUrl(entity));
        }

        private string GetResponseRedirectUrl(vConsultVisit4 entity)
        {
            string url = "";
           
            LastContentVisitListOP lc = new LastContentVisitListOP()
            {
                //HealthcareServiceUnitID = Convert.ToInt32(cboServiceUnit.Value),
                RegistrationDate = txtDateFrom.Text,
                RegistrationDate2 = txtDateTo.Text,
                ParamedicID = hdnPhysicianID.Value == string.Empty ? 0 : Convert.ToInt32(hdnPhysicianID.Value),
                QuickText = txtSearchView.Text,
                QuickFilterExpression = hdnFilterExpressionQuickSearch.Value,
                MedicalNo = txtBarcodeEntry.Text
            };
            AppSession.LastContentVisitListOP = lc;

            string paramValue = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN_TRANSAKSI_HANYA_DAPAT_DIUBAH_UNIT_ASAL).ParameterValue;
            AppSession.IsAdminCanCancelAllTransaction = paramValue == "0" ? true : false;

            RegisteredPatient pt = new RegisteredPatient();
            pt.MRN = entity.MRN;
            pt.MedicalNo = entity.MedicalNo;
            pt.PatientName = entity.PatientName;
            pt.GCGender = entity.GCGender;
            pt.GCSex = entity.GCSex;
            pt.DateOfBirth = entity.DateOfBirth;
            pt.RegistrationID = entity.RegistrationID;
            pt.RegistrationNo = entity.RegistrationNo;
            pt.RegistrationDate = entity.RegistrationDate;
            pt.RegistrationTime = entity.RegistrationTime;
            pt.VisitID = entity.VisitID;
            pt.VisitDate = entity.VisitDate;
            pt.VisitTime = entity.VisitTime;
            pt.StartServiceDate = entity.StartServiceDate;
            pt.StartServiceTime = entity.StartServiceTime;
            pt.DischargeDate = entity.DischargeDate;
            pt.DischargeTime = entity.DischargeTime;
            pt.GCCustomerType = entity.GCCustomerType;
            pt.BusinessPartnerID = entity.BusinessPartnerID;
            pt.ParamedicID = entity.ParamedicID;
            pt.ParamedicCode = entity.ParamedicCode;
            pt.ParamedicName = entity.ParamedicName;
            pt.SpecialtyID = entity.SpecialtyID;
            pt.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
            pt.DepartmentID = entity.DepartmentID;
            pt.ServiceUnitName = entity.ServiceUnitName;
            pt.RoomCode = entity.RoomCode;
            pt.BedCode = entity.BedCode;
            pt.DepartmentID = entity.DepartmentID;
            pt.ChargeClassID = entity.ChargeClassID;
            pt.ClassID = entity.ClassID;
            pt.GCRegistrationStatus = entity.GCVisitStatus;
            pt.IsLockDown = entity.IsLockDown;
            pt.IsBillingReopen = entity.IsBillingReopen;
            pt.LinkedRegistrationID = entity.LinkedRegistrationID;
            pt.LinkedToRegistrationID = entity.LinkedToRegistrationID;
            pt.LastAcuteInitialAssessmentDate = entity.LastAcuteInitialAssessmentDate;
            pt.LastChronicInitialAssessmentDate = entity.LastChronicInitialAssessmentDate;
            pt.IsNeedRenewalAcuteInitialAssessment = Helper.GetIsNeedRenewalOPInitialAssessment(pt.LastAcuteInitialAssessmentDate, Convert.ToInt16(AppSession.OP0027));
            pt.IsNeedRenewalChronicInitialAssessment = Helper.GetIsNeedRenewalOPInitialAssessment(pt.LastChronicInitialAssessmentDate, Convert.ToInt16(AppSession.OP0027));
            pt.OpenFromModuleID = "FN";
            AppSession.RegisteredPatient = pt;
           
            string parentCode = Constant.MenuCode.Outpatient.BILL_SUMMARY;
            switch (pt.DepartmentID)
            {
                case Constant.Facility.EMERGENCY: parentCode = Constant.MenuCode.EmergencyCare.BILL_SUMMARY;
                    break;
                case Constant.Facility.INPATIENT: parentCode = Constant.MenuCode.Inpatient.BILL_SUMMARY;
                    break;
                case Constant.Facility.PHARMACY: parentCode = Constant.MenuCode.Pharmacy.BILL_SUMMARY;
                    break;
                case Constant.Facility.MEDICAL_CHECKUP: parentCode = Constant.MenuCode.MedicalCheckup.BILL_SUMMARY;
                    break;
                case Constant.Facility.DIAGNOSTIC:
                    if (pt.HealthcareServiceUnitID == Convert.ToInt32(hdnServiceUntIDRadiologi.Value))
                    {
                        parentCode = Constant.MenuCode.Imaging.BILL_SUMMARY;
                    }
                    else if (pt.HealthcareServiceUnitID == Convert.ToInt32(hdnServiceUntIDLab.Value))
                    {
                        parentCode = Constant.MenuCode.Laboratory.BILL_SUMMARY;
                    }
                    else
                    {
                        parentCode = Constant.MenuCode.MedicalDiagnostic.BILL_SUMMARY;
                    }
                    break;
                default:
                    parentCode = Constant.MenuCode.Outpatient.BILL_SUMMARY;
                    break;
            }

            string filterExpression = string.Format("ParentCode = '{0}'", parentCode);
            
            string moduleID = Constant.Module.OUTPATIENT;
            switch (pt.DepartmentID)
            {
                case Constant.Facility.EMERGENCY: moduleID = Constant.Module.EMERGENCY;
                    break;
                case Constant.Facility.INPATIENT: moduleID = Constant.Module.INPATIENT;
                    break;
                case Constant.Facility.PHARMACY: moduleID = Constant.Module.PHARMACY;
                    break;
                case Constant.Facility.MEDICAL_CHECKUP: moduleID = Constant.Module.MEDICAL_CHECKUP;
                    break;
                case Constant.Facility.DIAGNOSTIC:
                    if (pt.HealthcareServiceUnitID == Convert.ToInt32(hdnServiceUntIDRadiologi.Value))
                    {
                        moduleID = Constant.Module.IMAGING;
                        
                    }
                    else if (pt.HealthcareServiceUnitID == Convert.ToInt32(hdnServiceUntIDLab.Value))
                    {
                        moduleID = Constant.Module.LABORATORY;
                    }
                    else
                    {
                        moduleID = Constant.Module.MEDICAL_DIAGNOSTIC;
                    }
                    break;
                default:
                    moduleID = Constant.Module.OUTPATIENT;
                    break;
            }

            List<GetUserMenuAccess> lstMenu = BusinessLayer.GetUserMenuAccess(moduleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, filterExpression);
            int parentID = (int)lstMenu.Where(p => p.MenuIndex > 0).OrderBy(p => p.MenuIndex).FirstOrDefault().MenuID;

            filterExpression = string.Format("ParentID = {0}", parentID);
            lstMenu = BusinessLayer.GetUserMenuAccess(moduleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, filterExpression);
            GetUserMenuAccess menu = lstMenu.OrderBy(p => p.MenuIndex).FirstOrDefault();
            url = string.Format("{0}?FNVisitID={1}", Page.ResolveUrl(menu.MenuUrl), entity.VisitID);

            switch (pt.DepartmentID)
            {
                case Constant.Facility.EMERGENCY: url = url.Replace("Finance", string.Format("{0}Care", Constant.Facility.EMERGENCY.ToLower()));
                    break;
                case Constant.Facility.INPATIENT: url = url.Replace("Finance", Constant.Facility.INPATIENT.ToLower());
                    break;
                case Constant.Facility.PHARMACY: url = url.Replace("Finance", Constant.Facility.PHARMACY.ToLower());
                    break;
                case Constant.Facility.MEDICAL_CHECKUP: url = url.Replace("Finance", "MedicalCheckup");
                    break;
                case Constant.Facility.DIAGNOSTIC:
                    if (pt.HealthcareServiceUnitID == Convert.ToInt32(hdnServiceUntIDRadiologi.Value))
                    {
                        url = url.Replace("Finance", Constant.Facility.IMAGING.ToLower());
                    }
                    else if (pt.HealthcareServiceUnitID == Convert.ToInt32(hdnServiceUntIDLab.Value))
                    {
                        url = url.Replace("Finance", Constant.Facility.LABORATORY.ToLower());
                    }
                    else
                    {
                        url = url.Replace("Finance", "MedicalDiagnostic");
                    }
                    break;
                default:
                    url = url.Replace("Finance", Constant.Facility.OUTPATIENT.ToLower());
                    break;
            }

            MedicalDiagnosticUserLogin medicalDiagnostic = new MedicalDiagnosticUserLogin();
            MedicalDiagnosticType er = new MedicalDiagnosticType();
            er = MedicalDiagnosticType.None;
            medicalDiagnostic.MedicalDiagnosticType = er;

            string ModuleID = moduleID;
            AppSession.UserLogin.ModuleID = ModuleID;

            if (ModuleID == Constant.Module.LABORATORY)
            {
                medicalDiagnostic.MedicalDiagnosticType = MedicalDiagnosticType.Laboratory;
                string laboratoryID = BusinessLayer.GetSettingParameter(Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID).ParameterValue;
                medicalDiagnostic.HealthcareServiceUnitID = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND ServiceUnitID = {1} AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, laboratoryID))[0].HealthcareServiceUnitID;
            }
            else if (ModuleID == Constant.Module.IMAGING)
            {
                medicalDiagnostic.MedicalDiagnosticType = MedicalDiagnosticType.Imaging;
                string imagingID = BusinessLayer.GetSettingParameter(Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID).ParameterValue;
                medicalDiagnostic.HealthcareServiceUnitID = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND ServiceUnitID = {1} AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, imagingID))[0].HealthcareServiceUnitID;
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
                List<vHealthcareServiceUnit> lstHealthcareServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND ServiceUnitID IN ({1},{2}) AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, laboratoryID, imagingID));
                medicalDiagnostic.ImagingHealthcareServiceUnitID = lstHealthcareServiceUnit.FirstOrDefault(p => p.ServiceUnitID == Convert.ToInt32(imagingID)).HealthcareServiceUnitID;
                medicalDiagnostic.LaboratoryHealthcareServiceUnitID = lstHealthcareServiceUnit.FirstOrDefault(p => p.ServiceUnitID == Convert.ToInt32(laboratoryID)).HealthcareServiceUnitID;
            }

            AppSession.MedicalDiagnostic = medicalDiagnostic;


            return url;
        }

        protected void cbpBarcodeEntryProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string filterExpression = GetFilterExpression();
            filterExpression += string.Format(" AND MedicalNo = '{0}'", txtBarcodeEntry.Text);
            vConsultVisit4 entity = BusinessLayer.GetvConsultVisit4List(filterExpression).FirstOrDefault();

            string url = "";
            if (entity != null)
                url = GetResponseRedirectUrl(entity);

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpUrl"] = url;
        }
    }
}