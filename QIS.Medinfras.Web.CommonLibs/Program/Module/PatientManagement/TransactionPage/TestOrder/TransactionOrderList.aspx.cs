using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class TransactionOrderList : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.PATIENT_TRANSACTION_ORDER_LIST;
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.PATIENT_ORDER_LIST;
                case Constant.Facility.OUTPATIENT: return Constant.MenuCode.Outpatient.PATIENT_PAGE_ORDER_LIST;
                case Constant.Facility.LABORATORY: return Constant.MenuCode.Laboratory.PATIENT_ORDER_LIST;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.PATIENT_ORDER_LIST;
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.PATIENT_ORDER_LIST;
                    return Constant.MenuCode.MedicalDiagnostic.PATIENT_ORDER_LIST;
                default: return Constant.MenuCode.MedicalDiagnostic.PATIENT_ORDER_LIST;
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnDepartmentID.Value = AppSession.RegisteredPatient.DepartmentID;
            
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            String filterExpression = string.Format("HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vPatientOrderAll WHERE RegistrationID = {0})", hdnRegistrationID.Value);
            List<vHealthcareServiceUnit> lstEntity = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression);
            lstEntity.Insert(0, new vHealthcareServiceUnit() { ServiceUnitName = "", ServiceUnitCode = "" });

            txtServiceOrderDateFrom.Text = DateTime.Today.AddDays(-7).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtServiceOrderDateTo.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            BindGridView();
            BindGridViewDetail();
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("RegistrationID = {0} AND GCTransactionStatus != '{1}'", hdnRegistrationID.Value, Constant.TransactionStatus.VOID);
            if (txtServiceUnitCode.Text != null && txtServiceUnitCode.Text != "")
            {
                filterExpression += string.Format(" AND ServiceUnitCode = '{0}'", txtServiceUnitCode.Text);
            }

            filterExpression += string.Format(" AND OrderDate BETWEEN '{0}' AND '{1}'", Helper.GetDatePickerValue(txtServiceOrderDateFrom).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtServiceOrderDateTo).ToString(Constant.FormatString.DATE_FORMAT_112));
            filterExpression += " ORDER BY OrderDate, OrderTime";
            List<vPatientOrderAll> lstEntity = BusinessLayer.GetvPatientOrderAllList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
        
        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridView();
                result = "refresh";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridViewDetail()
        {
            string filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);
            if (txtServiceUnitCode.Text != null && txtServiceUnitCode.Text != "")
            {
                filterExpression += string.Format(" AND ServiceUnitCode = '{0}'", txtServiceUnitCode.Text);
            }

            filterExpression += string.Format(" AND OrderDate BETWEEN '{0}' AND '{1}'", Helper.GetDatePickerValue(txtServiceOrderDateFrom).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtServiceOrderDateTo).ToString(Constant.FormatString.DATE_FORMAT_112));
            filterExpression += " ORDER BY ServiceUnitName, ItemName1";
            List<vPatientOrderAllPerDetailSummary> lstEntity = BusinessLayer.GetvPatientOrderAllPerDetailSummaryList(filterExpression);
            grdViewDetail.DataSource = lstEntity;
            grdViewDetail.DataBind();
        }

        protected void cbpViewDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridViewDetail();
                result = "refresh";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}