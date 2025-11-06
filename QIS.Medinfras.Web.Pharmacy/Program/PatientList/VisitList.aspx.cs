using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class VisitList : BasePageRegisteredPatient
    {
        public override string OnGetMenuCode()
        {
            string id = Page.Request.QueryString["id"];
            switch (id)
            {
                case "ptp": return Constant.MenuCode.Pharmacy.PATIENT_TRANSACTION_PAGE;
                case "ct": return Constant.MenuCode.MedicalDiagnostic.CHANGE_PATIENT_TRANSACTION_STATUS;
                default: return Constant.MenuCode.Pharmacy.BILL_SUMMARY;
            }
        }
        private GetUserMenuAccess menu;
        public override bool IsShowRightPanel()
        {
            return false;
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
                string id = Page.Request.QueryString["id"];

                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;

                List<GetServiceUnitUserList> lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.Facility.PHARMACY, "IsUsingRegistration = 1");
                Methods.SetComboBoxField<GetServiceUnitUserList>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                cboServiceUnit.SelectedIndex = 0;

                int count = BusinessLayer.GetServiceUnitParamedicRowCount(string.Format("HealthcareServiceUnitID = {0}", cboServiceUnit.Value));
                if (count > 0)
                    hdnIsHealthcareServiceUnitHasParamedic.Value = "1";
                else
                    hdnIsHealthcareServiceUnitHasParamedic.Value = "0";

                txtRegistrationDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

                hdnIsUsedReopenBilling.Value = AppSession.IsUsedReopenBilling ? "1" : "0";
                if (id == "bs")
                {
                    trFilterReopenBilling.Visible = AppSession.IsUsedReopenBilling;
                }
                else
                {
                    trFilterReopenBilling.Visible = false;
                }

                SettingControlProperties();

                grdRegisteredPatient.InitializeControl();

                Helper.SetControlEntrySetting(txtRegistrationDate, new ControlEntrySetting(true, true, true), "mpPatientList");
                Helper.SetControlEntrySetting(cboServiceUnit, new ControlEntrySetting(true, true, true), "mpPatientList");

                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

                txtBarcodeEntry.Focus();
            }
        }

        private void SettingControlProperties()
        {
            if (AppSession.LastContentVisitListPH != null)
            {
                LastContentVisitListPH lc = AppSession.LastContentVisitListPH;
                txtRegistrationDate.Text = lc.RegistrationDate;
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
            string filterExpression = string.Empty;

            filterExpression = string.Format("HealthcareServiceUnitID = {0}", cboServiceUnit.Value);

            if (hdnIsUsedReopenBilling.Value == "1")
            {
                if (chkIsIncludeReopenBilling.Checked)
                {
                    filterExpression += string.Format(" AND GCVisitStatus IN ('{0}') AND IsBillingReopen = 1",
                                                            Constant.VisitStatus.CLOSED);
                }
                else
                {
                    filterExpression += string.Format(" AND GCVisitStatus NOT IN ('{0}','{1}','{2}')",
                                                            Constant.VisitStatus.OPEN, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED);
                }
            }
            else
            {
                filterExpression += string.Format(" AND GCVisitStatus NOT IN ('{0}','{1}','{2}')",
                                                        Constant.VisitStatus.OPEN, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED);
            }

            if (!chkIsPreviousEpisodePatient.Checked)
            {
                filterExpression += string.Format(" AND VisitDate = '{0}'", Helper.GetDatePickerValue(txtRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112));
            }

            if (txtPhysicianCode.Text != "")
            {
                filterExpression += string.Format(" AND ParamedicID = {0}", hdnPhysicianID.Value);
            }

            if (hdnFilterExpressionQuickSearch.Value != "")
            {
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            }

            if (hdnSearchBarcodeNoRM.Value != "" && hdnSearchBarcodeNoRM.Value != null && hdnSearchBarcodeNoRM.Value != "00-00-00-00")
            {
                filterExpression += string.Format(" AND MedicalNo = '{0}'", hdnSearchBarcodeNoRM.Value);
            }

            return filterExpression;
        }

        public override void OnGrdRowClick(string oVisitID)
        {
            vConsultVisit4 entity = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", oVisitID)).FirstOrDefault();
            RegisteredPatient pt = new RegisteredPatient();
            pt.MRN = entity.MRN;
            pt.MedicalNo = entity.MedicalNo;
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
            AppSession.RegisteredPatient = pt;
            Response.Redirect(GetResponseRedirectUrl(entity));
        }

        private string GetResponseRedirectUrl(vConsultVisit4 entity)
        {
            string url = "";
            string id = Page.Request.QueryString["id"];

            LastContentVisitListPH lc = new LastContentVisitListPH()
            {
                HealthcareServiceUnitID = Convert.ToInt32(cboServiceUnit.Value),
                RegistrationDate = txtRegistrationDate.Text,
                ParamedicID = hdnPhysicianID.Value == string.Empty ? 0 : Convert.ToInt32(hdnPhysicianID.Value),
                QuickText = txtSearchView.Text,
                QuickFilterExpression = hdnFilterExpressionQuickSearch.Value,
                MedicalNo = txtBarcodeEntry.Text
            };
            AppSession.LastContentVisitListPH = lc;

            string paramValue = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN_TRANSAKSI_HANYA_DAPAT_DIUBAH_UNIT_ASAL).ParameterValue;
            AppSession.IsAdminCanCancelAllTransaction = paramValue == "0" ? true : false;

            if (id == "bs")
            {
                RegisteredPatient pt = new RegisteredPatient();
                pt.MRN = entity.MRN;
                pt.MedicalNo = entity.MedicalNo;
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
                AppSession.RegisteredPatient = pt;

                AppSession.HealthcareServiceUnitID = cboServiceUnit.Value.ToString();

                string filterExpression = string.Format("ParentCode = '{0}'", Constant.MenuCode.Pharmacy.BILL_SUMMARY);
                List<GetUserMenuAccess> lstMenu = BusinessLayer.GetUserMenuAccess(Constant.Module.PHARMACY, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, filterExpression);
                int parentID = (int)lstMenu.Where(p => p.MenuIndex > 0).OrderBy(p => p.MenuIndex).FirstOrDefault().MenuID;
                filterExpression = string.Format("ParentID = {0}", parentID);
                lstMenu = BusinessLayer.GetUserMenuAccess(Constant.Module.PHARMACY, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, filterExpression);
                GetUserMenuAccess menu = lstMenu.OrderBy(p => p.MenuIndex).FirstOrDefault();
                url = Page.ResolveUrl(menu.MenuUrl);
            }
            else
            {
                url = string.Format("~/Program/Prescription/PrescriptionEntry/PrescriptionEntryDetail.aspx?id=ptp|{0}|{1}", entity.VisitID, cboServiceUnit.Value);
            }
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