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

namespace QIS.Medinfras.Web.Imaging.Program
{
    public partial class RegistrationList : BasePageRegisteredPatient
    {
        public override string OnGetMenuCode()
        {
            string id = Page.Request.QueryString["id"];
            return Constant.MenuCode.Imaging.PATIENT_TRANSACTION_PAGE;
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
                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;

                hdnHealthcareServiceUnitID.Value = AppSession.MedicalDiagnostic.HealthcareServiceUnitID.ToString();
                int count = BusinessLayer.GetServiceUnitParamedicRowCount(string.Format("HealthcareServiceUnitID = {0}", hdnHealthcareServiceUnitID.Value));
                if (count > 0)
                    hdnIsHealthcareServiceUnitHasParamedic.Value = "1";
                else
                    hdnIsHealthcareServiceUnitHasParamedic.Value = "0";

                txtRegistrationDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                grdRegisteredPatient.InitializeControl();

                Helper.SetControlEntrySetting(txtRegistrationDate, new ControlEntrySetting(true, true, true), "mpPatientList");

                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

                if (AppSession.LastContentImagingRealization != null)
                {
                    LastContentImagingRealization lc = AppSession.LastContentImagingRealization;
                    txtRegistrationDate.Text = lc.ImagingDate;
                }

                txtBarcodeEntry.Focus();
            }
        }

        public override void LoadAllWords()
        {
            LoadWords();
        }

        public override string GetFilterExpression()
        {
            string filterExpression = string.Empty;

            filterExpression = string.Format("HealthcareServiceUnitID = {0} AND GCVisitStatus NOT IN ('{1}','{2}','{3}')",
                AppSession.MedicalDiagnostic.HealthcareServiceUnitID, Constant.VisitStatus.OPEN, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED);

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

            return filterExpression;
        }

        public override void OnGrdRowClick(string param)
        {
            string[] paramInfo = param.Split('|');

            string filterICV;
            if (paramInfo[1] != null && paramInfo[1] != "" && paramInfo[1] != "0")
            {
                filterICV = string.Format("VisitID = {0} AND TransactionID = {1}", paramInfo[0], paramInfo[1]);
            }
            else
            {
                filterICV = string.Format("VisitID = {0}", paramInfo[0]);
            }
            vImagingConsultVisit entity = BusinessLayer.GetvImagingConsultVisitList(filterICV).FirstOrDefault();
            Response.Redirect(GetResponseRedirectUrl(entity, paramInfo));
        }

        private string GetResponseRedirectUrl(vImagingConsultVisit entity, string[] paramInfo)
        {
            LastContentImagingRealization lc = new LastContentImagingRealization()
            {
                ImagingDate = txtRegistrationDate.Text
            };
            AppSession.LastContentImagingRealization = lc;

            string paramValue = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN_TRANSAKSI_HANYA_DAPAT_DIUBAH_UNIT_ASAL).ParameterValue;
            AppSession.IsAdminCanCancelAllTransaction = paramValue == "0" ? true : false;

            string url = "";
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
            AppSession.RegisteredPatient = pt;

            string parentCode = "";
            parentCode = Constant.MenuCode.Imaging.PATIENT_TRANSACTION_PAGE;
            string filterExpression = string.Format("ParentCode = '{0}'", parentCode);
            List<GetUserMenuAccess> lstMenu = BusinessLayer.GetUserMenuAccess(Constant.Module.IMAGING, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, filterExpression);
            int parentID = (int)lstMenu.Where(p => p.MenuIndex > 0).OrderBy(p => p.MenuIndex).FirstOrDefault().MenuID;

            filterExpression = string.Format("ParentID = {0}", parentID);
            lstMenu = BusinessLayer.GetUserMenuAccess(Constant.Module.IMAGING, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, filterExpression);
            GetUserMenuAccess menu = lstMenu.OrderBy(p => p.MenuIndex).FirstOrDefault();
            url = Page.ResolveUrl(string.Format("{0}?id={1}", menu.MenuUrl, paramInfo[1]));

            return url;
        }

        protected void cbpBarcodeEntryProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string filterExpression = GetFilterExpression();
            filterExpression += string.Format(" AND MedicalNo = '{0}'", txtBarcodeEntry.Text);
            vImagingConsultVisit entity = BusinessLayer.GetvImagingConsultVisitList(filterExpression).FirstOrDefault();

            string url = "";
            string[] paramInfo = new string[] { entity.VisitID.ToString(), entity.TransactionID.ToString() };

            if (entity != null)
                url = GetResponseRedirectUrl(entity, paramInfo);

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpUrl"] = url;
        }
    }
}