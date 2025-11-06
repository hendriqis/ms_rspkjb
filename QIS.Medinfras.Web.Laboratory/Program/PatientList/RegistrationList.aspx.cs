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

namespace QIS.Medinfras.Web.Laboratory.Program
{
    public partial class RegistrationList : BasePageRegisteredPatient
    {
        public override string OnGetMenuCode()
        {
            //string id = Page.Request.QueryString["id"];
            //switch (id)
            //{
            //    case "gb": return Constant.MenuCode.Laboratory.GENERATE_BILL;
            //    case "py": return Constant.MenuCode.Laboratory.PAYMENT;
            //    case "rpb": return Constant.MenuCode.Laboratory.RECALCULATION_PATIENT_BILL;
            //    case "pbd": return Constant.MenuCode.Laboratory.PATIENT_BILL_DISCOUNT;
            //    case "pprp": return Constant.MenuCode.Laboratory.PATIENT_PAYMENT_RECEIPT_PRINT;
            //    case "pprr": return Constant.MenuCode.Laboratory.PATIENT_PAYMENT_RECEIPT_REPRINT;
            //    case "vppr": return Constant.MenuCode.Laboratory.VOID_PATIENT_PAYMENT_RECEIPT;
            //    default: return Constant.MenuCode.Laboratory.VOID_PATIENT_BILL;
            //}
            return "";
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
            }
        }

        public override void LoadAllWords()
        {
            LoadWords();
        }

        public override string GetFilterExpression()
        {
            string filterExpression = string.Format("RegistrationDate = '{0}' AND HealthcareServiceUnitID = {1} AND GCRegistrationStatus NOT IN ('{2}','{3}','{4}')", Helper.GetDatePickerValue(txtRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112), hdnHealthcareServiceUnitID.Value, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED, Constant.VisitStatus.OPEN);
            if (txtPhysicianCode.Text != "")
                filterExpression += string.Format(" AND ParamedicID = {0}", hdnPhysicianID.Value);
            if (hdnFilterExpressionQuickSearch.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            return filterExpression;
        }

        public override void OnGrdRowClick(string transactionNo)
        {
            string id = Page.Request.QueryString["id"];
            string url = "";
            switch (id)
            {
                case "gb": url = string.Format("~/Libs/Program/Module/PatientManagement/GenerateBill/GenerateBillEntry.aspx?id={0}", transactionNo); break;
                case "py": url = string.Format("~/Libs/Program/Module/PatientManagement/Payment/PaymentEntry.aspx?id={0}", transactionNo); break;
                case "rpb": url = string.Format("~/Program/RecalculationBill/RecalculationPatientBillDetail.aspx?id={0}", transactionNo); break;
                case "pbd": url = string.Format("~/Libs/Program/Module/PatientManagement/PatientBillDiscount/PatientBillDiscountEntry.aspx?id={0}", transactionNo); break;
                case "pprp": url = string.Format("~/Libs/Program/Module/PatientManagement/PatientPaymentReceipt/PatientPaymentReceiptDetailPrint.aspx?id={0}", transactionNo); break;
                case "pprr": url = string.Format("~/Libs/Program/Module/PatientManagement/PatientPaymentReceipt/PatientPaymentReceiptDetail.aspx?id=r|{0}", transactionNo); break;
                case "vppr": url = string.Format("~/Libs/Program/Module/PatientManagement/PatientPaymentReceipt/PatientPaymentReceiptDetail.aspx?id=v|{0}", transactionNo); break;
                default: url = string.Format("~/Libs/Program/Module/PatientManagement/VoidPatientBill/VoidPatientBillEntry.aspx?id={0}", transactionNo); break;
            }
            Response.Redirect(url);
        }
    }
}