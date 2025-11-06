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
    public partial class RevenueSharingPerRegistrationEntry : BasePageTrx
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.REVENUE_SHARING_PER_REGISTRATION;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected string GetErrorMsgSelectTransactionFirst()
        {
            return Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_SELECT_TRANSACTION_FIRST_VALIDATION);
        }

        protected override void InitializeDataControl()
        {
            MenuMaster oMenu = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault();
            hdnPageTitle.Value = oMenu.MenuCaption;

            txtRegistrationDateFrom.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtRegistrationDateTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            txtLastPaymentDateFrom.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtLastPaymentDateTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            List<Department> lstDepartment = BusinessLayer.GetDepartmentList("IsActive = 1 AND IsHasRegistration = 1");
            lstDepartment.Add(new Department { DepartmentID = "", DepartmentName = "" });
            Methods.SetComboBoxField<Department>(cboDepartment, lstDepartment, "DepartmentName", "DepartmentID");
            cboDepartment.Value = "";

            List<Variable> lstPaidType = new List<Variable>();
            lstPaidType.Add(new Variable { Code = "0", Value = "Semua" });
            lstPaidType.Add(new Variable { Code = "1", Value = "Belum Lunas" });
            lstPaidType.Add(new Variable { Code = "2", Value = "Sudah Lunas" });
            Methods.SetComboBoxField<Variable>(cboPaidType, lstPaidType, "Value", "Code");
            cboPaidType.Value = "2";

            txtRevenueSharingDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format(
                                                                    "IsDeleted = 0 AND IsActive = 1 AND ParentID IN ('{0}','{1}','{2}','{3}')",
                                                                    Constant.StandardCode.REVENUE_REDUCTION,
                                                                    Constant.StandardCode.REVENUE_PAYMENT_METHOD,
                                                                    Constant.StandardCode.REVENUE_PERIODE_TYPE,
                                                                    Constant.StandardCode.CLINIC_GROUP
                                                                ));

            Methods.SetComboBoxField<StandardCode>(cboReduction, lstStandardCode.Where(a => a.ParentID == Constant.StandardCode.REVENUE_REDUCTION).ToList(), "StandardCodeName", "StandardCodeID");
            cboReduction.Value = Constant.RevenueReduction.REDUCTION_0;

            Methods.SetComboBoxField<StandardCode>(cboPaymentMethod, lstStandardCode.Where(a => a.ParentID == Constant.StandardCode.REVENUE_PAYMENT_METHOD).ToList(), "StandardCodeName", "StandardCodeID");
            cboPaymentMethod.Value = Constant.RevenuePaymentMethod.TUNAI;

            Methods.SetComboBoxField<StandardCode>(cboClinicGroup, lstStandardCode.Where(a => a.ParentID == Constant.StandardCode.CLINIC_GROUP).ToList(), "StandardCodeName", "StandardCodeID");
            cboClinicGroup.SelectedIndex = 0;

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

        protected void cbpViewARInvoice_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "refreshARInvoice")
                {
                    BindGridViewARInvoice();
                    result = "refreshARInvoice";
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";

            if (e.Parameter == "save")
            {
                if (OnProcessRecord(ref errMessage))
                    result = "success";
                else
                    result = "fail|" + errMessage;
            }
            else if (e.Parameter == "deletePRS")
            {
                if (OnDeleteRecordPRS(ref errMessage))
                    result = "success";
                else
                    result = "fail|" + errMessage;
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        #endregion

        #region Binding Grid

        private void BindGridViewRegistration(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            String filterExpression = "";

            if (cboPaidType.Value.ToString() == "2")
            {
                filterExpression += string.Format("IsPaidOff = 1 AND CONVERT(DATE, LastPaymentDate) BETWEEN '{0}' AND '{1}'", Helper.GetDatePickerValue(txtLastPaymentDateFrom.Text), Helper.GetDatePickerValue(txtLastPaymentDateTo.Text));
            }
            else
            {
                filterExpression += string.Format("RegistrationDate BETWEEN '{0}' AND '{1}'", Helper.GetDatePickerValue(txtRegistrationDateFrom.Text).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtRegistrationDateTo.Text).ToString(Constant.FormatString.DATE_FORMAT_112));
            }

            if (cboDepartment.Value != null)
            {
                filterExpression += string.Format(" AND DepartmentID LIKE '%{0}%'", cboDepartment.Value.ToString());
            }

            if (hdnPayerID.Value != null && hdnPayerID.Value != "" && hdnPayerID.Value != "0")
            {
                if (chkIsFilterPayerExclusion.Checked)
                {
                    filterExpression += string.Format(" AND BusinessPartnerID NOT IN ({0})", hdnPayerID.Value);
                }
                else
                {
                    filterExpression += string.Format(" AND BusinessPartnerID IN ({0})", hdnPayerID.Value);
                }
            }

            if (hdnFilterExpressionQuickSearch.Value != "")
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }
                filterExpression += string.Format("{0}", hdnFilterExpressionQuickSearch.Value);
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvRegistrationForRevenueSharingRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MATRIX);
            }

            List<vRegistrationForRevenueSharing> lstEntity = BusinessLayer.GetvRegistrationForRevenueSharingList(filterExpression, Constant.GridViewPageSize.GRID_MATRIX, pageIndex, "RegistrationID DESC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        private void BindGridViewRegistrationDetail()
        {
            if (hdnRegistrationID.Value != null && hdnRegistrationID.Value != "" && hdnRegistrationID.Value != "0")
            {
                int oRegistrationID = Convert.ToInt32(hdnRegistrationID.Value);

                vRegistrationForRevenueSharing entityReg = BusinessLayer.GetvRegistrationForRevenueSharingList(string.Format("RegistrationID = {0}", oRegistrationID)).FirstOrDefault();

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

                if (entityReg.BusinessPartnerID != 1)
                {
                    lblBusinessPartnerName.InnerHtml = string.Format("({0}) {1}", entityReg.BusinessPartnerCode, entityReg.BusinessPartnerName);
                }
                else
                {
                    lblBusinessPartnerName.InnerHtml = entityReg.BusinessPartnerName;
                }

                lblChargesAmount.InnerHtml = (entityReg.ChargesAmount + entityReg.SourceAmount).ToString(Constant.FormatString.NUMERIC_2);
                lblPaymentAmount.InnerHtml = entityReg.PaymentAmount.ToString(Constant.FormatString.NUMERIC_2);
                lblARInProcessAmount.InnerHtml = entityReg.ARInProcessAmount.ToString(Constant.FormatString.NUMERIC_2);
                lblARInvoiceAmount.InnerHtml = entityReg.ARInvoiceAmount.ToString(Constant.FormatString.NUMERIC_2);
                lblARReceivingAmount.InnerHtml = entityReg.ARReceivingAmount.ToString(Constant.FormatString.NUMERIC_2);
            }
            else
            {
                lblRegistrationNo.InnerHtml = "";
                lblPatient.InnerHtml = "";
                lblRegistrationDischargeDate.InnerHtml = "";
                lblServiceUnitName.InnerHtml = "";
                lblBusinessPartnerName.InnerHtml = "";

                lblChargesAmount.InnerHtml = "";
                lblPaymentAmount.InnerHtml = "";
                lblARInProcessAmount.InnerHtml = "";
                lblARInvoiceAmount.InnerHtml = "";
                lblARReceivingAmount.InnerHtml = "";
            }
        }

        private void BindGridViewPatientCharges()
        {
            if (hdnRegistrationID.Value != null && hdnRegistrationID.Value != "" && hdnRegistrationID.Value != "0")
            {
                int oRegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
                List<GetPatientChargesHdDtForRevenueSharingPerRegistration> lstCharges = BusinessLayer.GetPatientChargesHdDtForRevenueSharingPerRegistrationList(oRegistrationID);
                lvwPatientCharges.DataSource = lstCharges;
                lvwPatientCharges.DataBind();
            }
        }

        private void BindGridViewPatientPayment()
        {
            if (hdnRegistrationID.Value != null && hdnRegistrationID.Value != "" && hdnRegistrationID.Value != "0")
            {
                int oRegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
                string filterExpression = string.Format("RegistrationID = '{0}' AND GCTransactionStatus != '{1}'", oRegistrationID, Constant.TransactionStatus.VOID);
                List<vPatientPaymentHd> lstPayment = BusinessLayer.GetvPatientPaymentHdList(filterExpression, int.MaxValue, 1, "PaymentID");
                lvwViewPatientPayment.DataSource = lstPayment;
                lvwViewPatientPayment.DataBind();
            }
        }

        private void BindGridViewARInvoice()
        {
            if (hdnRegistrationID.Value != null && hdnRegistrationID.Value != "" && hdnRegistrationID.Value != "0")
            {
                int oRegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
                int oPaymentID = Convert.ToInt32(hdnPaymentExpandID.Value);
                string filterExpression = string.Format("RegistrationID = {0} AND PaymentID = {1}", oRegistrationID, oPaymentID);
                List<vARInvoiceReceivingRevenueSharing> lstInvoice = BusinessLayer.GetvARInvoiceReceivingRevenueSharingList(filterExpression, int.MaxValue, 1, "ARInvoiceID, ID");
                lvwARInvoice.DataSource = lstInvoice;
                lvwARInvoice.DataBind();
            }
        }

        #endregion

        private bool OnProcessRecord(ref string errMessage)
        {
            bool result = true;
            string returnValue;
            try
            {
                DateTime oRevenueSharingDate = Helper.GetDatePickerValue(txtRevenueSharingDate.Text);
                String oDepartmentID = cboDepartment.Value != null ? cboDepartment.Value.ToString() : "%%";
                String oGCReduction = cboReduction.Value != null ? cboReduction.Value.ToString() : "%%";
                String oGCPaymentMethod = cboReduction.Value != null ? cboPaymentMethod.Value.ToString() : "%%";
                String oGCClinicGroup = "%%";
                if (chkIsFilterClinicGroup.Checked)
                {
                    oGCClinicGroup = cboClinicGroup.Value.ToString();
                }

                returnValue = BusinessLayer.GenerateParamedicRevenueSharingPerRegistration(
                                            oRevenueSharingDate,
                                            oDepartmentID,
                                            oGCReduction,
                                            oGCPaymentMethod,
                                            oGCClinicGroup,
                                            hdnSelectedMember.Value.Substring(1),
                                            AppSession.UserLogin.UserID
                                        );
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }

        private bool OnDeleteRecordPRS(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TransRevenueSharingHdDao prsHdDao = new TransRevenueSharingHdDao(ctx);
            TransRevenueSharingDtDao prsDtDao = new TransRevenueSharingDtDao(ctx);

            try
            {
                int prsID = hdnRSTransactionIDForDelete.Value != "" ? Convert.ToInt32(hdnRSTransactionIDForDelete.Value) : 0;
                int dtID = hdnRSTransactionIDForDelete.Value != "" ? Convert.ToInt32(hdnChargesDtIDForDelete.Value) : 0;
                int dtPckgID = hdnRSTransactionIDForDelete.Value != "" ? Convert.ToInt32(hdnChargesDtPckgIDForDelete.Value) : 0;

                if (prsID != 0 && dtID != 0)
                {
                    TransRevenueSharingHd prsHd = prsHdDao.Get(prsID);
                    if (prsHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        if (dtPckgID != 0)
                        {
                            string filterPRSDt = string.Format("IsDeleted = 0 AND RSTransactionID = {0} AND PatientChargesDtID = {1} AND PatientChargesDtPackageID = {2}", prsHd.RSTransactionID, dtID, dtPckgID);
                            List<TransRevenueSharingDt> lstPRSDt = BusinessLayer.GetTransRevenueSharingDtList(filterPRSDt, ctx);
                            foreach (TransRevenueSharingDt prsDt in lstPRSDt)
                            {
                                prsDt.IsDeleted = true;
                                prsDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                prsDtDao.Update(prsDt);
                            }
                        }
                        else
                        {
                            string filterPRSDt = string.Format("IsDeleted = 0 AND RSTransactionID = {0} AND PatientChargesDtID = {1} AND PatientChargesDtPackageID IS NULL", prsHd.RSTransactionID, dtID);
                            List<TransRevenueSharingDt> lstPRSDt = BusinessLayer.GetTransRevenueSharingDtList(filterPRSDt, ctx);
                            foreach (TransRevenueSharingDt prsDt in lstPRSDt)
                            {
                                prsDt.IsDeleted = true;
                                prsDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                prsDtDao.Update(prsDt);
                            }
                        }
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = string.Format("Tidak dapat menghapus detail transaksi jasmed ini karena status transaksi nomor {0} sudah diproses.", prsHd.RevenueSharingNo);
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Tidak ada data transaksi jasmed yg terpilih.";
                    ctx.RollBackTransaction();
                }
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            return result;
        }
    }
}