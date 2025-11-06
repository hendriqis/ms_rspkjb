using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class InformationPatientBillingHistory : BasePageTrx
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.INFORMASI_HISTORY_BILLING_PATIENT;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            MenuMaster oMenu = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault();
            hdnPageTitle.Value = oMenu.MenuCaption;

            SetControlEntrySetting(txtMRN, new ControlEntrySetting(true, true, true));

            txtPeriodFrom.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPeriodTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            List<Department> lstDept = BusinessLayer.GetDepartmentList(string.Format("IsHasRegistration = 1 AND IsActive = 1"));
            lstDept.Insert(0, new Department { DepartmentID = "", DepartmentName = "" });
            Methods.SetComboBoxField<Department>(cboDepartment, lstDept, "DepartmentName", "DepartmentID");
            cboDepartment.SelectedIndex = 0;
        }

        #region Callback

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewRegistration(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewRegistration(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewRegDt_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "refreshRegDt")
                {
                    BindGridViewRegistrationDetail();
                    result = "refreshRegDt";
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpPatientCharges_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "refreshPatientCharges")
                {
                    BindGridViewPatientCharges();
                    result = "refreshPatientCharges";
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewPatientPayment_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "refreshPatientPayment")
                {
                    BindGridViewPatientPayment();
                    result = "refreshPatientPayment";
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewPatientBill_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "refreshPatientBill")
                {
                    BindGridViewPatientBill();
                    result = "refreshPatientBill";
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        public string GetFilterExpression()
        {
            String FromDate = Helper.GetDatePickerValue(txtPeriodFrom.Text).ToString(Constant.FormatString.DATE_FORMAT_112);
            String ToDate = Helper.GetDatePickerValue(txtPeriodTo.Text).ToString(Constant.FormatString.DATE_FORMAT_112);

            string filterExpression = string.Format("RegistrationStatus != '{0}'", Constant.VisitStatus.CANCELLED);

            if (hdnMRN.Value != "0" && hdnMRN.Value != "")
                filterExpression += string.Format(" AND MRN = {0}", hdnMRN.Value);

            if (!chkIsPreviousEpisodePatient.Checked)
            {
                filterExpression += string.Format(" AND (RegistrationDate BETWEEN '{0}' AND '{1}')", FromDate, ToDate);
            }

            if (cboDepartment.Value != null)
            {
                filterExpression += string.Format(" AND DepartmentID IN ('{0}')", cboDepartment.Value.ToString());
            }

            if (hdnFilterExpressionQuickSearchReg.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearchReg.Value);

            return filterExpression;
        }

        #region Binding Grid

        private void BindGridViewRegistration(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvInformationBillingHistoryPatientRegistrationRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MATRIX);
            }

            List<vInformationBillingHistoryPatientRegistration> lstEntity = BusinessLayer.GetvInformationBillingHistoryPatientRegistrationList(filterExpression, Constant.GridViewPageSize.GRID_MATRIX, pageIndex, "RegistrationID DESC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        private void BindGridViewRegistrationDetail()
        {
            if (hdnRegistrationID.Value != null && hdnRegistrationID.Value != "" && hdnRegistrationID.Value != "0")
            {
                int oRegistrationID = Convert.ToInt32(hdnRegistrationID.Value);

                string filterExpressionReg = string.Format("RegistrationID = {0} AND GCRegistrationStatus != '{1}' AND GCVisitStatus != '{1}'", oRegistrationID, Constant.VisitStatus.CANCELLED);
                vInformationBillingHistoryPatientRegistration entityReg = BusinessLayer.GetvInformationBillingHistoryPatientRegistrationList(filterExpressionReg).FirstOrDefault();

                lblRegistrationNo.InnerHtml = entityReg.RegistrationNo;

                if (entityReg.LinkedRegistrationNo != null && entityReg.LinkedRegistrationNo != "")
                {
                    trLinkRegistration.Attributes.Remove("style");

                    lblLinkedRegistrationNo.InnerHtml = entityReg.LinkedRegistrationNo;
                }
                else
                {
                    trLinkRegistration.Attributes.Add("style", "display:none");
                }

                if (entityReg.MRN != null && entityReg.MRN != 0)
                {
                    lblPatient.InnerHtml = string.Format("({0}) {1}", entityReg.MedicalNo, entityReg.PatientName);
                }
                else
                {
                    lblPatient.InnerHtml = string.Format("{0}", entityReg.PatientName);
                }

                if (entityReg.DischargeDate.ToString(Constant.FormatString.DATE_FORMAT) != Constant.ConstantDate.DEFAULT_NULL_DATE_FORMAT)
                {
                    lblRegistrationDischargeDate.InnerHtml = string.Format("{0} {1} s/d {2} {3}", entityReg.RegistrationDate.ToString(Constant.FormatString.DATE_FORMAT), entityReg.RegistrationTime, entityReg.DischargeDate.ToString(Constant.FormatString.DATE_FORMAT), entityReg.DischargeTime);
                }
                else
                {
                    lblRegistrationDischargeDate.InnerHtml = string.Format("{0} {1}", entityReg.RegistrationDate.ToString(Constant.FormatString.DATE_FORMAT), entityReg.RegistrationTime);
                }

                lblServiceUnitName.InnerHtml = entityReg.ServiceUnitName;
                lblDepartment.InnerHtml = entityReg.DepartmentID;

                if (entityReg.BusinessPartnerID != 1)
                {
                    lblBusinessPartnerName.InnerHtml = string.Format("({0}) {1}", entityReg.BusinessPartnerCode, entityReg.BusinessPartnerName);
                }
                else
                {
                    lblBusinessPartnerName.InnerHtml = entityReg.BusinessPartnerName;
                }

                string filterExpressionCharges = string.Format("RegistrationID = '{0}' AND GCTransactionStatus != '{1}'", oRegistrationID, Constant.TransactionStatus.VOID);
                List<vPatientChargesHd> lstEntityCharges = BusinessLayer.GetvPatientChargesHdList(filterExpressionCharges);

                string filterExpressionBill = string.Format("RegistrationID = '{0}' AND GCTransactionStatus != '{1}'", oRegistrationID, Constant.TransactionStatus.VOID);
                List<vPatientBill> lstEntityBill = BusinessLayer.GetvPatientBillList(filterExpressionBill);

                string filterExpressionPayment = string.Format("RegistrationID = '{0}' AND GCTransactionStatus != '{1}'", oRegistrationID, Constant.TransactionStatus.VOID);
                List<vPatientPaymentHd> lstEntityPayment = BusinessLayer.GetvPatientPaymentHdList(filterExpressionPayment);

                decimal totalLineAmount = (entityReg.ChargesAmount + entityReg.SourceAmount + entityReg.AdminAmount - entityReg.DiscountAmount + entityReg.RoundingAmount);

                lblChargesAmount.InnerHtml = lstEntityCharges.Sum(a => a.TotalAmount).ToString(Constant.FormatString.NUMERIC_2);
                lblBillingAmount.InnerHtml = lstEntityBill.Sum(a => a.TotalAmount - a.DiscountAmount).ToString(Constant.FormatString.NUMERIC_2); 
                lblPaymentAmount.InnerHtml = lstEntityPayment.Sum(a => a.TotalPaymentAmount).ToString(Constant.FormatString.NUMERIC_2);
                lblSisaAmount.InnerHtml = (entityReg.PaymentAmount - totalLineAmount).ToString(Constant.FormatString.NUMERIC_2);
            }
            else
            {
                lblRegistrationNo.InnerHtml = "";
                lblPatient.InnerHtml = "";
                lblRegistrationDischargeDate.InnerHtml = "";
                lblDepartment.InnerHtml = "";
                lblServiceUnitName.InnerHtml = "";
                lblBusinessPartnerName.InnerHtml = "";

                lblChargesAmount.InnerHtml = "";
                lblPaymentAmount.InnerHtml = "";
                lblBillingAmount.InnerHtml = "";
                lblSisaAmount.InnerHtml = "";
            }
        }

        private void BindGridViewPatientCharges()
        {
            if (hdnRegistrationID.Value != null && hdnRegistrationID.Value != "" && hdnRegistrationID.Value != "0")
            {
                int oRegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
                string filterExpression = string.Format("RegistrationID = '{0}' ORDER BY ChargesDate, TransactionID, DtID", oRegistrationID);
                List<vInformationBillingHistoryPatient> lstCharges = BusinessLayer.GetvInformationBillingHistoryPatientList(filterExpression);
                lvwPatientCharges.DataSource = lstCharges;
                lvwPatientCharges.DataBind();
            }
        }

        private void BindGridViewPatientBill()
        {
            if (hdnRegistrationID.Value != null && hdnRegistrationID.Value != "" && hdnRegistrationID.Value != "0")
            {
                int oRegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
                string filterExpression = string.Format("RegistrationID = '{0}' AND GCTransactionStatus != '{1}'", oRegistrationID, Constant.TransactionStatus.VOID);
                List<vPatientBill> lstBill = BusinessLayer.GetvPatientBillList(filterExpression, int.MaxValue, 1, "PatientBillingID");
                lvwViewPatientBill.DataSource = lstBill;
                lvwViewPatientBill.DataBind();
            }
        }

        private void BindGridViewPatientPayment()
        {
            if (hdnRegistrationID.Value != null && hdnRegistrationID.Value != "" && hdnRegistrationID.Value != "0")
            {
                int oRegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
                string filterExpression = string.Format("RegistrationID = '{0}' AND GCTransactionStatus != '{1}'", oRegistrationID, Constant.TransactionStatus.VOID);
                List<vInformationBillingHistoryPatientPayment> lstPayment = BusinessLayer.GetvInformationBillingHistoryPatientPaymentList(filterExpression, int.MaxValue, 1, "PaymentID");
                lvwViewPatientPayment.DataSource = lstPayment;
                lvwViewPatientPayment.DataBind();
            }
        }

        #endregion

    }
}