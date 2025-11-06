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

namespace QIS.Medinfras.Web.EmergencyCare.Program
{
    public partial class FollowupPatientDischargeList : BasePageContent
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EmergencyCare.FOLLOWUP_PATIENT_DISCHARGE;
        }

        private GetUserMenuAccess menu;
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
        protected int PageCount = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;

                string filterExpression = string.Format("IsUsingRegistration = 1 AND HealthcareServiceUnitID NOT IN ({0},{1})", AppSession.MedicalDiagnostic.ImagingHealthcareServiceUnitID, AppSession.MedicalDiagnostic.LaboratoryHealthcareServiceUnitID);

                hdnIsPatientDischarge.Value = "0";
                if (Page.Request.QueryString["id"] == "pd")
                    hdnIsPatientDischarge.Value = "1";

                List<GetServiceUnitUserList> lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.Facility.EMERGENCY, filterExpression);
                Methods.SetComboBoxField<GetServiceUnitUserList>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                cboServiceUnit.SelectedIndex = 0;

                int count = BusinessLayer.GetServiceUnitParamedicRowCount(string.Format("HealthcareServiceUnitID = {0}", cboServiceUnit.Value));
                if (count > 0)
                    hdnIsHealthcareServiceUnitHasParamedic.Value = "1";
                else
                    hdnIsHealthcareServiceUnitHasParamedic.Value = "0";

                txtDischargeDateFrom.Text = DateTime.Today.AddDays(-7).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtDischargeDateTo.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

                Helper.SetControlEntrySetting(cboServiceUnit, new ControlEntrySetting(true, true, true), "mpPatientList");

                BindGridView(1, true, ref PageCount);

                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());
            }
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("DepartmentID = '{0}' AND HealthcareServiceUnitID = {1} AND GCVisitStatus IN ('{2}','{3}')", Constant.Facility.EMERGENCY, cboServiceUnit.Value, Constant.VisitStatus.DISCHARGED, Constant.VisitStatus.CLOSED);
            string orderBy = "";

            if (txtPhysicianCode.Text != "")
                filterExpression += string.Format(" AND ParamedicID = {0}", hdnPhysicianID.Value);

            if (rblDataSource.SelectedValue == "filterRegistrationDate")
            {
                filterExpression += string.Format(" AND RegistrationDate BETWEEN '{0}' AND '{1}'",
                Helper.GetDatePickerValue(txtDischargeDateFrom).ToString(Constant.FormatString.DATE_FORMAT_112),
                Helper.GetDatePickerValue(txtDischargeDateTo).ToString(Constant.FormatString.DATE_FORMAT_112));
                orderBy = "ClosedDate, DischargeDate, RegistrationID";
            }
            else if (rblDataSource.SelectedValue == "filterClosedDate")
            {
                filterExpression += string.Format(" AND ClosedDate BETWEEN '{0}' AND '{1}'",
                Helper.GetDatePickerValue(txtDischargeDateFrom).ToString(Constant.FormatString.DATE_FORMAT_112),
                Helper.GetDatePickerValue(txtDischargeDateTo).ToString(Constant.FormatString.DATE_FORMAT_112));
                orderBy = "ClosedDate, DischargeDate, RegistrationID";
            }
            else
            {
                filterExpression += string.Format(" AND DischargeDate BETWEEN '{0}' AND '{1}'",
                Helper.GetDatePickerValue(txtDischargeDateFrom).ToString(Constant.FormatString.DATE_FORMAT_112),
                Helper.GetDatePickerValue(txtDischargeDateTo).ToString(Constant.FormatString.DATE_FORMAT_112));
                orderBy = "DischargeDate, DischargeTime, RegistrationID";
            }
            
            if (hdnFilterExpressionQuickSearch.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvConsultVisit9RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_PATIENT_LIST_2);
            }

            List<vConsultVisit9> lstEntity = BusinessLayer.GetvConsultVisit9List(filterExpression, Constant.GridViewPageSize.GRID_PATIENT_LIST_2, pageIndex, orderBy);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void btnOpenTransactionDt_Click(object sender, EventArgs e)
        {
            if (hdnTransactionNo.Value != "")
            {
                vConsultVisit4 entity = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", hdnTransactionNo.Value)).FirstOrDefault();
                Response.Redirect(GetResponseRedirectUrl(entity));
            }
        }

        private string GetResponseRedirectUrl(vConsultVisit4 entity)
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

            string parentCode = OnGetMenuCode();

            string filterExpression = string.Format("ParentCode = '{0}'", parentCode);
            List<GetUserMenuAccess> lstMenu = BusinessLayer.GetUserMenuAccess(Constant.Module.EMERGENCY, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, filterExpression);
            
            int parentID = (int)lstMenu.Where(p => p.MenuIndex > 0).OrderBy(p => p.MenuIndex).FirstOrDefault().MenuID;
            filterExpression = string.Format("ParentID = {0}", parentID);
            lstMenu = BusinessLayer.GetUserMenuAccess(Constant.Module.EMERGENCY, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, filterExpression);
            GetUserMenuAccess menu = lstMenu.OrderBy(p => p.MenuIndex).FirstOrDefault();
            return Page.ResolveUrl(menu.MenuUrl);
        }

    }
}