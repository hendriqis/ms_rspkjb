using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.CommonLibs.MasterPage;

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class BridgingINACBGs : BasePageRegisteredPatient
    {
        private string[] lstSelectedMember = null;

        public override string OnGetMenuCode()
        {
            string id = Page.Request.QueryString["id"];
            switch (id)
            {
                case "ps": return Constant.MenuCode.MedicalRecord.PATIENT_SOAP;                
                default: return Constant.MenuCode.MedicalRecord.PATIENT_FOLDER_STATUS;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                List<Variable> lstVariable = new List<Variable>();
                lstVariable.Add(new Variable { Code = "0", Value = "Semua" });
                lstVariable.Add(new Variable { Code = "1", Value = "Belum Diproses" });
                lstVariable.Add(new Variable { Code = "2", Value = "Sudah Diproses" });
                Methods.SetComboBoxField<Variable>(cboResultType, lstVariable, "Value", "Code");
                cboResultType.Value = "1";

                lstVariable = new List<Variable>();
                lstVariable.Add(new Variable { Code = "0", Value = "Semua" });
                lstVariable.Add(new Variable { Code = "1", Value = "Pulang" });
                lstVariable.Add(new Variable { Code = "2", Value = "Dirawat" });
                Methods.SetComboBoxField<Variable>(cboRegistrationStatus, lstVariable, "Value", "Code");
                cboRegistrationStatus.Value = "1";

                List<Department> lstDept = BusinessLayer.GetDepartmentList(string.Format("DepartmentID != '{0}' AND IsActive = 1", Constant.Facility.PHARMACY));
                Methods.SetComboBoxField<Department>(cboPatientFrom, lstDept, "DepartmentName", "DepartmentID");
                cboPatientFrom.Value = Constant.Facility.INPATIENT;

                txtFromRegistrationDate.Text = DateTime.Today.AddDays(-7).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtToRegistrationDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

                Helper.SetControlEntrySetting(txtFromRegistrationDate, new ControlEntrySetting(false, false, false), "mpPatientList");
                Helper.SetControlEntrySetting(txtToRegistrationDate, new ControlEntrySetting(false, false, false), "mpPatientList");
                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());
            }
        }

        public override void LoadAllWords()
        {
            LoadWords();
        }

        public override string GetFilterExpression()
        {
            string filterExpression = string.Format("(VisitDate BETWEEN '{0}' AND '{1}') AND GCVisitStatus != '{2}'", Helper.GetDatePickerValue(txtFromRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtToRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.VisitStatus.CANCELLED);
            if (hdnServiceUnitID.Value != "0" && hdnServiceUnitID.Value != "")
                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", hdnServiceUnitID.Value);
            else if (cboPatientFrom.Value != null)
                filterExpression += string.Format(" AND DepartmentID = '{0}'", cboPatientFrom.Value);
            if (hdnFilterExpressionQuickSearch.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);

            string registrationStatus = cboRegistrationStatus.Value.ToString();
            if (registrationStatus == "1")
                filterExpression += string.Format(" AND GCVisitStatus = '{0}'", Constant.VisitStatus.CLOSED);
            else if(registrationStatus == "2")
                filterExpression += string.Format(" AND GCVisitStatus != '{0}'", Constant.VisitStatus.CLOSED);

            string id = Page.Request.QueryString["id"];
            if (id == "mfs")
            {
                string resultType = cboResultType.Value.ToString();
                if (resultType == "1")
                    filterExpression += string.Format(" AND VisitID NOT IN (SELECT VisitID FROM VisitMRFolderStatus)");
                else if (resultType == "2")
                    filterExpression += string.Format(" AND VisitID IN (SELECT VisitID FROM VisitMRFolderStatus)");
            }
            else
            {
                string resultType = cboResultType.Value.ToString();
                if (resultType == "1")
                    filterExpression += string.Format(" AND VisitID NOT IN (SELECT VisitID FROM PatientDiagnosis WHERE DiagnoseID IS NOT NULL AND IsDeleted = 0)");
                else if (resultType == "2")
                    filterExpression += string.Format(" AND VisitID IN (SELECT VisitID FROM PatientDiagnosis WHERE DiagnoseID IS NOT NULL AND IsDeleted = 0)");
            }
            return filterExpression;
        }

        public override void OnGrdRowClick(string transactionNo)
        {
            string id = Page.Request.QueryString["id"];
            string url = "";
            
            switch (id)
            {
                case "ps":
                    vConsultVisit entity = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", transactionNo))[0];
                    RegisteredPatient pt = new RegisteredPatient();
                    pt.MRN = entity.MRN;
                    pt.MedicalNo = entity.MedicalNo;
                    pt.RegistrationID = entity.RegistrationID;
                    pt.VisitID = entity.VisitID;
                    pt.VisitDate = entity.VisitDate;
                    pt.VisitTime = entity.VisitTime;
                    pt.ParamedicID = entity.ParamedicID;
                    pt.SpecialtyID = entity.SpecialtyID;
                    pt.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
                    pt.DepartmentID = entity.DepartmentID;
                    pt.ClassID = entity.ClassID;
                    AppSession.RegisteredPatient = pt;
                    url = "~/Program/PatientMedicalRecord/MRPatientSOAP/MRPatientDiagnose/MRPatientDiagnose.aspx"; break;
                case "mfs": url = string.Format("~/Program/PatientMedicalRecord/MedicalFolderStatus/MedicalFolderStatusEntry.aspx?id={0}", transactionNo); break;                
            }
            Response.Redirect(url);
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vPatientPaymentHd entity = e.Row.DataItem as vPatientPaymentHd;
                CheckBox chkIsSelected = e.Row.FindControl("chkIsSelected") as CheckBox;
                if (lstSelectedMember.Contains(entity.PaymentID.ToString()))
                    chkIsSelected.Checked = true;
            }
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            //string filterExpression = string.Format("BusinessPartnerID IN (SELECT BusinessPartnerID FROM vCustomer WHERE BusinessPartnerID = {0} OR CustomerBillTo = {0}) AND GCTransactionStatus != '{1}' AND GCPaymentType = '{2}' AND RegistrationID NOT IN (SELECT RegistrationID FROM vARInvoiceDt WHERE GCTransactionStatus != '{3}') AND PaymentDate BETWEEN '{4}' AND '{5}'", AppSession.BusinessPartnerID, Constant.TransactionStatus.VOID, Constant.PaymentType.AR_PAYER, Constant.TransactionStatus.VOID, Helper.GetDatePickerValue(txtPeriodFrom).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtPeriodTo).ToString(Constant.FormatString.DATE_FORMAT_112));
            //if (cboDepartment.Value != null && cboDepartment.Value.ToString() != "")
            //    filterExpression += string.Format(" AND DepartmentID = '{0}'", cboDepartment.Value);
            //if (isCountPageCount)
            //{
            //    int rowCount = BusinessLayer.GetvPatientPaymentHdRowCount(filterExpression);
            //    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            //}

            //lstSelectedMember = hdnSelectedMember.Value.Split(',');
            //List<vPatientPaymentHd> lstPaymentHd = BusinessLayer.GetvPatientPaymentHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
            //grdView.DataSource = lstPaymentHd;
            //grdView.DataBind();
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
    }
}