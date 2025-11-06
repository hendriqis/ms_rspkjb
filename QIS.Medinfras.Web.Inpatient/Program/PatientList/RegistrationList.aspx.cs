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

namespace QIS.Medinfras.Web.Inpatient.Program
{
    public partial class RegistrationList : BasePageContent
    {
        String ServiceUnit = "";
        public override string OnGetMenuCode()
        {
            string id = Page.Request.QueryString["id"];
            switch (id)
            {
                //case "gb": return Constant.MenuCode.Inpatient.GENERATE_BILL;
                //case "py": return Constant.MenuCode.Inpatient.PAYMENT;
                //case "rpb": return Constant.MenuCode.Inpatient.RECALCULATION_PATIENT_BILL;
                case "pt": return Constant.MenuCode.Inpatient.PATIENT_TRANSFER;
                default: return Constant.MenuCode.Inpatient.TRANSFER_PATIENT_BILL;
                //case "pbd": return Constant.MenuCode.Inpatient.PATIENT_BILL_DISCOUNT;
                //case "pprp": return Constant.MenuCode.Inpatient.PATIENT_PAYMENT_RECEIPT_PRINT;
                //case "pprr": return Constant.MenuCode.Inpatient.PATIENT_PAYMENT_RECEIPT_REPRINT;
                //case "vppr": return Constant.MenuCode.Inpatient.VOID_PATIENT_PAYMENT_RECEIPT;
                //default: return Constant.MenuCode.Inpatient.VOID_PATIENT_BILL;
            }
            //return Constant.MenuCode.Inpatient.TRANSFER_PATIENT_BILL;
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
        protected int PageCount = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;

                List<GetServiceUnitUserList> lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.Facility.INPATIENT, "IsUsingRegistration = 1");
                lstServiceUnit.Insert(0, new GetServiceUnitUserList { HealthcareServiceUnitID = 0, ServiceUnitName = "" });
                Methods.SetComboBoxField<GetServiceUnitUserList>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                cboServiceUnit.SelectedIndex = 1;

                string hsuID = "";
                foreach (GetServiceUnitUserList hsu in lstServiceUnit)
                {
                    hsuID += hsu.HealthcareServiceUnitID + ",";
                }
                hsuID = hsuID.Substring(0, hsuID.Length - 1);
                hdnFilterHealthcareServiceUnit.Value = hsuID;

                BindGridView(1, true, ref PageCount);

                //Helper.SetControlEntrySetting(cboServiceUnit, new ControlEntrySetting(true, true, true), "mpPatientList");

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
            string filterExpression = "";
            if (Page.Request.QueryString["id"] == "tpb")
            {
                bool isCheckOutstanding = true;
                SettingParameterDt oParam = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN_VALIDASI_TAGIHAN_KETIKA_PULANG);
                if (oParam != null)
                    isCheckOutstanding = oParam.ParameterValue == "1" ? true : false;
                if (isCheckOutstanding)
                    filterExpression = string.Format("HealthcareServiceUnitID = {0} AND DepartmentID = '{1}' AND GCRegistrationStatus NOT IN ('{2}','{3}','{4}','{5}') AND LinkedRegistrationID IS NOT NULL AND LinkedGCRegistrationStatus != '{5}' AND EXISTS (SELECT RegistrationID FROM Registration r2 WHERE r2.IsChargesTransfered = 0 AND r2.RegistrationID = vRegistration.LinkedRegistrationID)", cboServiceUnit.Value, Constant.Facility.INPATIENT, Constant.VisitStatus.OPEN, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED, Constant.VisitStatus.DISCHARGED);
                else
                    filterExpression = string.Format("HealthcareServiceUnitID = {0} AND DepartmentID = '{1}' AND GCRegistrationStatus NOT IN ('{2}','{3}','{4}') AND LinkedRegistrationID IS NOT NULL AND EXISTS (SELECT RegistrationID FROM Registration r2 WHERE r2.IsChargesTransfered = 0 AND r2.RegistrationID = vRegistration.LinkedRegistrationID)", cboServiceUnit.Value, Constant.Facility.INPATIENT, Constant.VisitStatus.OPEN, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED);
            }
            else
            ServiceUnit = cboServiceUnit.Value.ToString();
            if (ServiceUnit != "0")
            {
                filterExpression = string.Format("HealthcareServiceUnitID = {0} AND DepartmentID = '{1}' AND GCRegistrationStatus NOT IN ('{2}','{3}','{4}','{5}')", cboServiceUnit.Value, Constant.Facility.INPATIENT, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED, Constant.VisitStatus.OPEN, Constant.VisitStatus.DISCHARGED);
            }
            else
            {
                filterExpression = string.Format(" HealthcareServiceUnitID IN ({0}) AND DepartmentID = '{1}' AND GCRegistrationStatus NOT IN ('{2}','{3}','{4}','{5}')", hdnFilterHealthcareServiceUnit.Value, Constant.Facility.INPATIENT, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED, Constant.VisitStatus.OPEN, Constant.VisitStatus.DISCHARGED);
            }
            if (hdnFilterExpressionQuickSearch.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvRegistrationRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_PATIENT_LIST_2);
            }

            List<vRegistration> lstEntity = BusinessLayer.GetvRegistrationList(filterExpression, Constant.GridViewPageSize.GRID_PATIENT_LIST_2, pageIndex);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void btnOpenTransactionDt_Click(object sender, EventArgs e)
        {
            if (hdnTransactionNo.Value != "")
            {
                vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", hdnTransactionNo.Value)).FirstOrDefault();
                RegisteredPatient pt = new RegisteredPatient();
                pt.MRN = entity.MRN;
                pt.MedicalNo = entity.MedicalNo;
                pt.RegistrationID = entity.RegistrationID;
                pt.ParamedicID = entity.ParamedicID;
                pt.ParamedicCode = entity.ParamedicCode;
                pt.SpecialtyID = entity.SpecialtyID;
                pt.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
                pt.ServiceUnitName = entity.ServiceUnitName;
                pt.RoomCode = entity.RoomCode;
                pt.BedCode = entity.BedCode;
                pt.DepartmentID = entity.DepartmentID;
                pt.ClassID = entity.ClassID;
                pt.ChargeClassID = entity.ChargeClassID;
                pt.IsLockDown = entity.IsLockDown;
                AppSession.RegisteredPatient = pt;

                string id = Page.Request.QueryString["id"];
                string url = "";
                switch (id)
                {
                    case "gb": url = string.Format("~/Libs/Program/Module/PatientManagement/GenerateBill/GenerateBillEntry.aspx?id={0}", hdnTransactionNo.Value); break;
                    case "py": url = string.Format("~/Libs/Program/Module/PatientManagement/Payment/PaymentEntry.aspx?id={0}", hdnTransactionNo.Value); break;
                    case "rpb": url = string.Format("~/Libs/Program/Module/PatientManagement/RecalculationBill/RecalculationPatientBillDetail.aspx?id={0}", hdnTransactionNo.Value); break;
                    case "pt": url = string.Format("~/Program/PatientTransfer/PatientTransferEntry.aspx?id={0}", hdnTransactionNo.Value); break;
                    case "tpb": url = string.Format("~/Libs/Program/Module/PatientManagement/TransferPatientBill/TransferPatientBillDetail.aspx?id={0}", hdnTransactionNo.Value); break;
                    case "pbd": url = string.Format("~/Libs/Program/Module/PatientManagement/PatientBillDiscount/PatientBillDiscountEntry.aspx?id={0}", hdnTransactionNo.Value); break;
                    case "pprp": url = string.Format("~/Libs/Program/Module/PatientManagement/PatientPaymentReceipt/PatientPaymentReceiptDetailPrint.aspx?id={0}", hdnTransactionNo.Value); break;
                    case "pprr": url = string.Format("~/Libs/Program/Module/PatientManagement/PatientPaymentReceipt/PatientPaymentReceiptDetail.aspx?id=r|{0}", hdnTransactionNo.Value); break;
                    case "vppr": url = string.Format("~/Libs/Program/Module/PatientManagement/PatientPaymentReceipt/PatientPaymentReceiptDetail.aspx?id=v|{0}", hdnTransactionNo.Value); break;
                    default: url = string.Format("~/Libs/Program/Module/PatientManagement/VoidPatientBill/VoidPatientBillEntry.aspx?id={0}", hdnTransactionNo.Value); break;
                }
                Response.Redirect(url);
            }
        }
    }
}