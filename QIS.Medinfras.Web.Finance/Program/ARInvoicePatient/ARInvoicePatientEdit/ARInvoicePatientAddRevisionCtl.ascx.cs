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
    public partial class ARInvoicePatientAddRevisionCtl : BaseEntryPopupCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;

        private ARInvoicePatientEditEntry DetailPage
        {
            get { return (ARInvoicePatientEditEntry)Page; }
        }

        protected string DateTimeNowDatePicker()
        {
            return DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
        }

        public override void InitializeDataControl(string param)
        {
            hdnARInvoiceIDCtl.Value = param;

            txtPeriodFrom.Text = DateTime.Now.AddDays(-14).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPeriodTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            List<Department> lstDepartment = BusinessLayer.GetDepartmentList("IsActive = 1");
            lstDepartment.Insert(0, new Department { DepartmentID = "", DepartmentName = "" });
            Methods.SetComboBoxField<Department>(cboDepartment, lstDepartment, "DepartmentName", "DepartmentID");
            cboDepartment.SelectedIndex = 0;

            List<Variable> lstRegistrationStatus = new List<Variable>();
            lstRegistrationStatus.Add(new Variable { Code = "0", Value = "NON CLOSED" });
            lstRegistrationStatus.Add(new Variable { Code = "1", Value = "CLOSED" });
            Methods.SetComboBoxField<Variable>(cboRegistrationStatus, lstRegistrationStatus, "Value", "Code");
            cboRegistrationStatus.Value = "1";

            List<Variable> lstFilterBy = new List<Variable>();
            lstFilterBy.Add(new Variable { Code = "0", Value = "Tanggal Pengakuan Piutang" });
            lstFilterBy.Add(new Variable { Code = "1", Value = "Tanggal Pulang" });
            Methods.SetComboBoxField<Variable>(cboFilterBy, lstFilterBy, "Value", "Code");
            cboFilterBy.Value = "0";

            List<Variable> lstSortBy = new List<Variable>();
            lstSortBy.Add(new Variable { Code = "0", Value = "Tanggal Registrasi (ASC)" });
            lstSortBy.Add(new Variable { Code = "1", Value = "Tanggal Registrasi (DESC)" });
            lstSortBy.Add(new Variable { Code = "2", Value = "Tanggal Keluar (ASC)" });
            lstSortBy.Add(new Variable { Code = "3", Value = "Tanggal Keluar (DESC)" });
            Methods.SetComboBoxField<Variable>(cboSortBy, lstSortBy, "Value", "Code");
            cboSortBy.Value = "0";

            BindGridView();
        }

        #region Bind Grid
        private void BindGridView()
        {
            lstSelectedMember = hdnSelectedMember.Value.Split(',');

            string paramDate = string.Format("{0}|{1}", Helper.GetDatePickerValue(txtPeriodFrom).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtPeriodTo).ToString(Constant.FormatString.DATE_FORMAT_112));
            string paramDept = "";
            if (cboDepartment.Value != null)
            {
                paramDept = cboDepartment.Value.ToString();
            }
            string isExclusion = chkIsExclusion.Checked ? "1" : "0";
            string registrationStatus = cboRegistrationStatus.Value.ToString();

            string paramFilterBy = cboFilterBy.Value.ToString();
            string paramSortBy = cboSortBy.Value.ToString();

            List<GetPatientPaymentDtARPatient> lstPaymentDt = BusinessLayer.GetPatientPaymentDtARPatientList(AppSession.PatientDetail.MRN.ToString(), paramDate, paramDept, isExclusion, registrationStatus, paramFilterBy, paramSortBy);
            grdView.DataSource = lstPaymentDt;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GetPatientPaymentDtARPatient entity = e.Row.DataItem as GetPatientPaymentDtARPatient;
                CheckBox chkIsSelected = e.Row.FindControl("chkIsSelected") as CheckBox;
                if (lstSelectedMember.Contains(entity.PaymentDetailID.ToString()))
                    chkIsSelected.Checked = true;
            }
        }

        protected void cbpProcessDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridView();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        #region Save Entity
        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ARInvoiceHdDao entityHdDao = new ARInvoiceHdDao(ctx);
            ARInvoiceDtDao entityDtDao = new ARInvoiceDtDao(ctx);
            TransactionTypeLockDao entityLockDao = new TransactionTypeLockDao(ctx);

            try
            {
                ARInvoiceHd arInvoiceHd = entityHdDao.Get(Convert.ToInt32(hdnARInvoiceIDCtl.Value));

                String[] paramPaymentDetailID = hdnSelectedMember.Value.Substring(1).Split(',');
                String[] paramPaymentID = hdnSelectedMemberPaymentID.Value.Substring(1).Split(',');

                List<PatientPaymentHd> lstPaymentHd = BusinessLayer.GetPatientPaymentHdList(string.Format("PaymentID IN ({0})", hdnSelectedMemberPaymentID.Value.Substring(1)), ctx);
                List<PatientPaymentDt> lstPaymentDt = BusinessLayer.GetPatientPaymentDtList(string.Format("PaymentDetailID IN ({0})", hdnSelectedMember.Value.Substring(1)), ctx);
                int countNotOpen = lstPaymentHd.Count(a => a.GCTransactionStatus != Constant.TransactionStatus.OPEN);

                if (countNotOpen == 0)
                {
                    string filterCheck = "ARInvoiceID NOT IN (SELECT ARInvoiceID FROM ARInvoiceHd WHERE GCTransactionStatus != 'X121^999') AND ISNULL(GCTransactionDetailStatus,'') != 'X121^999'";
                    List<ARInvoiceDt> lstCheckPayment = BusinessLayer.GetARInvoiceDtList(string.Format("PaymentDetailID IN ({0}) AND {1}",
                            hdnSelectedMember.Value.Substring(1), filterCheck), ctx);
                    if (lstCheckPayment.Count == 0)
                    {
                        bool isTransactionLock = false;
                        TransactionTypeLock entityLock = entityLockDao.Get(Constant.TransactionCode.AR_INVOICE_PATIENT);
                        if (entityLock.LockedUntilDate != null)
                        {
                            DateTime DateLock = Convert.ToDateTime(entityLock.LockedUntilDate);
                            DateTime DateNow = arInvoiceHd.ARInvoiceDate;
                            if (DateNow > DateLock)
                            {
                                isTransactionLock = false;
                            }
                            else
                            {
                                isTransactionLock = true;
                            }
                        }
                        else
                        {
                            isTransactionLock = false;
                        }

                        if (!isTransactionLock)
                        {
                            for (int i = 0; i < paramPaymentDetailID.Count(); ++i)
                            {
                                PatientPaymentDt paymentDt = lstPaymentDt.FirstOrDefault(p => p.PaymentDetailID == Convert.ToInt32(paramPaymentDetailID[i]));
                                PatientPaymentHd paymentHd = lstPaymentHd.FirstOrDefault(p => p.PaymentID == paymentDt.PaymentID);
                                ARInvoiceDt entityDt = new ARInvoiceDt();
                                entityDt.ARInvoiceID = arInvoiceHd.ARInvoiceID;
                                entityDt.RegistrationID = paymentHd.RegistrationID;
                                entityDt.PaymentID = paymentHd.PaymentID;
                                entityDt.PaymentDetailID = paymentDt.PaymentDetailID;
                                entityDt.GCTransactionDetailStatus = Constant.TransactionStatus.OPEN;
                                entityDt.TransactionAmount = entityDt.ClaimedAmount = paymentDt.PaymentAmount;
                                entityDt.CreatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Insert(entityDt);
                            }

                            arInvoiceHd.RevisionCount += 1;
                            arInvoiceHd.LastRevisionBy = AppSession.UserLogin.UserID;
                            arInvoiceHd.LastRevisionDate = DateTime.Now;
                            arInvoiceHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityHdDao.Update(arInvoiceHd);

                            retval = arInvoiceHd.ARInvoiceNo;
                            ctx.CommitTransaction();
                        }
                        else
                        {
                            result = false;
                            errMessage = string.Format("This Transaction Type has been Locked until {0}. Please contact an authorized personnel.", Convert.ToDateTime(entityLock.LockedUntilDate).ToString(Constant.FormatString.DATE_FORMAT));
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
                    }
                    else
                    {
                        ctx.RollBackTransaction();
                        errMessage = "Piutang tidak dapat diproses. Harap refresh halaman ini.";
                        result = false;
                    }
                }
                else
                {
                    ctx.RollBackTransaction();
                    errMessage = "Piutang tidak dapat diproses. Ada nomor piutang yang sudah diproses.";
                    result = false;
                }
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
        #endregion
    }
}