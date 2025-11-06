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
    public partial class ARInvoicePayerProcessEntry : BasePageTrx
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.AR_INVOICE_PAYER_PROCESS;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

        protected string OnGetCustomerFilterExpression()
        {
            return string.Format("IsDeleted = 0");
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            hdnBusinessPartnerID.Value = AppSession.BusinessPartnerID.ToString();

            hdnIsFinalisasiKlaimAfterARInvoice.Value = AppSession.IsClaimFinalAfterARInvoice ? "1" : "0";
            hdnIsFinalisasiKlaimBeforeARInvoiceAndSkipProcessKlaim.Value = AppSession.IsClaimFinalBeforeARInvoiceAndSkipProcessClaim ? "1" : "0";

            string filterSetvar = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}','{7}')",
                                                        AppSession.UserLogin.HealthcareID, //0
                                                        Constant.SettingParameter.FN_AR_LEAD_TIME, //1
                                                        Constant.SettingParameter.FN_AR_DUE_DATE_COUNT_FROM, //2
                                                        Constant.SettingParameter.FN_PU_NAME, //3
                                                        Constant.SettingParameter.FN_DEFAULT_BINDING_TERM_TERMID_ARINVOICE, //4
                                                        Constant.SettingParameter.FN_DEFAULT_SELISIH_HARI_UNTUK_FILTER_PERIODE_TRANSAKSI_PROSES_PIUTANG, //5
                                                        Constant.SettingParameter.FN_IS_ARINVOICEDATE_ALLOW_BACKDATE, //6
                                                        Constant.SettingParameter.FN_IS_ARINVOICEDATE_ALLOW_FUTUREDATE //7
                                                    );
            List<SettingParameterDt> lstSetvarDt = BusinessLayer.GetSettingParameterDtList(filterSetvar);

            hdnSetvarLeadTime.Value = lstSetvarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_AR_LEAD_TIME).FirstOrDefault().ParameterValue;
            hdnSetvarHitungJatuhTempoDari.Value = lstSetvarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_AR_DUE_DATE_COUNT_FROM).FirstOrDefault().ParameterValue;
            hdnUPName.Value = lstSetvarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_PU_NAME).FirstOrDefault().ParameterValue;
            hdnDefaultSelisihHariUntukFilterPeriodeTransaksi.Value = lstSetvarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_DEFAULT_SELISIH_HARI_UNTUK_FILTER_PERIODE_TRANSAKSI_PROSES_PIUTANG).FirstOrDefault().ParameterValue;

            hdnIsAllowBackDate.Value = lstSetvarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_ARINVOICEDATE_ALLOW_BACKDATE).FirstOrDefault().ParameterValue;
            hdnIsAllowFutureDate.Value = lstSetvarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_ARINVOICEDATE_ALLOW_FUTUREDATE).FirstOrDefault().ParameterValue;
            hdnDateToday.Value = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            string setvarTermID = lstSetvarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_DEFAULT_BINDING_TERM_TERMID_ARINVOICE).FirstOrDefault().ParameterValue;

            int selisihHari = hdnDefaultSelisihHariUntukFilterPeriodeTransaksi.Value != "" ? (Convert.ToInt32(hdnDefaultSelisihHariUntukFilterPeriodeTransaksi.Value) * -1) : 0;
            txtPeriodFrom.Text = DateTime.Now.AddDays(selisihHari).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPeriodTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            List<Department> lstDepartment = BusinessLayer.GetDepartmentList("IsActive = 1");
            lstDepartment.Insert(0, new Department { DepartmentID = "", DepartmentName = "" });
            Methods.SetComboBoxField<Department>(cboDepartment, lstDepartment, "DepartmentName", "DepartmentID");
            cboDepartment.SelectedIndex = 0;

            List<Bank> lstBank = BusinessLayer.GetBankList(string.Format("IsDeleted = 0 AND (GCBankType = '{0}' OR GCBankType IS NULL)", Constant.BankType.BANK_PIUTANG));
            Methods.SetComboBoxField<Bank>(cboBank, lstBank, "BankName", "BankID");
            cboBank.SelectedIndex = 0;

            string filterBPVA = string.Format("BusinessPartnerID = {0} AND IsDeleted = 0 AND IsDefault = 1", hdnBusinessPartnerID.Value);
            List<vBusinessPartnerVirtualAccount> lstBPVA = BusinessLayer.GetvBusinessPartnerVirtualAccountList(filterBPVA);
            if (lstBPVA.Count > 0)
            {
                hdnBusinessPartnerVirtualAccountID.Value = lstBPVA.FirstOrDefault().ID.ToString();
                txtBankName.Text = lstBPVA.FirstOrDefault().BankName;
                txtAccountNo.Text = lstBPVA.FirstOrDefault().AccountNo;
            }

            txtInvoiceDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtDocumentDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtARDocumentReceiveDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            Term term = BusinessLayer.GetTermList(string.Format("IsDeleted = 0 AND TermID IN (SELECT TermID FROM BusinessPartners WHERE BusinessPartnerID = {0})", AppSession.BusinessPartnerID)).FirstOrDefault();
            List<Term> listTerm = BusinessLayer.GetTermList(string.Format("IsDeleted = 0"));
            Methods.SetComboBoxField<Term>(cboTerm, listTerm, "TermName", "TermID");
            if (term != null)
            {
                cboTerm.Value = term.TermID.ToString();
            }
            else if (!string.IsNullOrEmpty(setvarTermID))
            {
                cboTerm.Value = setvarTermID;
            }
            else
            {
                cboTerm.SelectedIndex = 1;
            }

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
            lstSortBy.Add(new Variable { Code = "4", Value = "No SEP (ASC)" });
            lstSortBy.Add(new Variable { Code = "5", Value = "No SEP (DESC)" });
            lstSortBy.Add(new Variable { Code = "6", Value = "No Rujukan (ASC)" });
            lstSortBy.Add(new Variable { Code = "7", Value = "No Rujukan (DESC)" });
            lstSortBy.Add(new Variable { Code = "8", Value = "Tanggal Piutang (ASC), Nama Pasien (ASC)" });
            lstSortBy.Add(new Variable { Code = "9", Value = "Tanggal Piutang (DESC), Nama Pasien (DESC)" });
            lstSortBy.Add(new Variable { Code = "10", Value = "No Urutan (ASC)" });
            lstSortBy.Add(new Variable { Code = "11", Value = "No Urutan (DESC)" });
            Methods.SetComboBoxField<Variable>(cboSortBy, lstSortBy, "Value", "Code");
            cboSortBy.Value = "0";

            vBusinessPartners entityCP = BusinessLayer.GetvBusinessPartnersList(string.Format("BusinessPartnersID = {0}", Convert.ToInt32(hdnBusinessPartnerID.Value))).FirstOrDefault();
            if (entityCP.BillToID != 0 && entityCP.BillToID != null)
            {
                txtPrintAsName.Text = entityCP.BillToName;
            }
            else
            {
                txtPrintAsName.Text = AppSession.BusinessPartnerName;
            }

            if (hdnUPName.Value == "1")
            {
                txtRecipientName.Text = entityCP.BusinessPartnersCP;
            }

            BindGridView();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridView();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnSelectedMember, new ControlEntrySetting(false, false, false, ""));
            SetControlEntrySetting(hdnSelectedMemberReferenceNoPrefix, new ControlEntrySetting(false, false, false, ""));
            SetControlEntrySetting(hdnSelectedMemberReferenceNo, new ControlEntrySetting(false, false, false, ""));
        }

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

            List<GetPatientPaymentDtARPayer> lstPaymentDt = BusinessLayer.GetPatientPaymentDtARPayerList(AppSession.BusinessPartnerID, paramDate, paramDept, isExclusion, registrationStatus, paramFilterBy, paramSortBy);
            grdView.DataSource = lstPaymentDt;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GetPatientPaymentDtARPayer entity = e.Row.DataItem as GetPatientPaymentDtARPayer;
                CheckBox chkIsSelected = e.Row.FindControl("chkIsSelected") as CheckBox;
                if (lstSelectedMember.Contains(entity.PaymentDetailID.ToString()))
                    chkIsSelected.Checked = true;
            }
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ARInvoiceHdDao entityHdDao = new ARInvoiceHdDao(ctx);
            ARInvoiceDtDao entityDtDao = new ARInvoiceDtDao(ctx);
            TermDao termDao = new TermDao(ctx);
            TransactionTypeLockDao entityLockDao = new TransactionTypeLockDao(ctx);

            try
            {
                String[] paramPaymentDetailID = hdnSelectedMember.Value.Substring(1).Split(',');
                String[] paramPaymentID = hdnSelectedMemberPaymentID.Value.Substring(1).Split(',');
                String[] paramReferenceNoPrefix = hdnSelectedMemberReferenceNoPrefix.Value.Substring(1).Split(',');
                String[] paramReferenceNo = hdnSelectedMemberReferenceNo.Value.Substring(1).Split(',');

                List<PatientPaymentHd> lstPaymentHd = BusinessLayer.GetPatientPaymentHdList(string.Format("PaymentID IN ({0})", hdnSelectedMemberPaymentID.Value.Substring(1)), ctx);
                List<PatientPaymentDt> lstPaymentDt = BusinessLayer.GetPatientPaymentDtList(string.Format("PaymentDetailID IN ({0})", hdnSelectedMember.Value.Substring(1)), ctx);
                List<PatientPaymentDtInfo> lstPaymentDtInfo = BusinessLayer.GetPatientPaymentDtInfoList(string.Format("PaymentDetailID IN ({0})", hdnSelectedMember.Value.Substring(1)), ctx);
                int countNotOpen = lstPaymentHd.Count(a => a.GCTransactionStatus != Constant.TransactionStatus.OPEN);

                if (countNotOpen == 0)
                {
                    string filterCheck = "ARInvoiceID NOT IN (SELECT ARInvoiceID FROM ARInvoiceHd WHERE GCTransactionStatus != 'X121^999') AND ISNULL(GCTransactionDetailStatus,'') != 'X121^999'";
                    List<ARInvoiceDt> lstCheckPayment = BusinessLayer.GetARInvoiceDtList(string.Format(
                                                                            "PaymentDetailID IN ({0}) AND {1}",
                                                                            hdnSelectedMember.Value.Substring(1), filterCheck), ctx);
                    if (lstCheckPayment.Count == 0)
                    {
                        bool isTransactionLock = false;
                        TransactionTypeLock entityLock = entityLockDao.Get(Constant.TransactionCode.AR_INVOICE_PAYER);
                        if (entityLock.LockedUntilDate != null)
                        {
                            DateTime DateLock = Convert.ToDateTime(entityLock.LockedUntilDate);
                            DateTime DateNow = Helper.GetDatePickerValue(txtInvoiceDate);
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
                            ARInvoiceHd entityHd = new ARInvoiceHd();
                            entityHd.ARInvoiceDate = Helper.GetDatePickerValue(txtInvoiceDate);
                            entityHd.TransactionCode = Constant.TransactionCode.AR_INVOICE_PAYER;
                            //entityHd.BankID = Convert.ToInt32(cboBank.Value); // ditutup oleh RN 20191118 karna dialihkan ke BPVA
                            entityHd.BusinessPartnerVirtualAccountID = Convert.ToInt32(hdnBusinessPartnerVirtualAccountID.Value);
                            entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                            entityHd.MRN = null;
                            entityHd.BusinessPartnerID = AppSession.BusinessPartnerID;
                            entityHd.Remarks = txtRemarks.Text;
                            entityHd.TermID = Convert.ToInt32(cboTerm.Value);
                            entityHd.DocumentDate = Helper.GetDatePickerValue(txtDocumentDate);
                            entityHd.ARDocumentReceiveDate = Helper.GetDatePickerValue(txtARDocumentReceiveDate);
                            entityHd.ARDocumentReceiveByName = txtARDocumentReceiveByName.Text;
                            entityHd.RecipientName = txtRecipientName.Text;
                            entityHd.PrintAsName = txtPrintAsName.Text;

                            Term term = termDao.Get(entityHd.TermID);

                            DateTime tempdue1; // 1 = TglInvoice(ARInvoiceDate) || 2 = TglDokumen(DocumentDate) || 3 = TglTerimaDokumen(ARDocumentReceiveDate)
                            if (hdnSetvarHitungJatuhTempoDari.Value == "1")
                            {
                                tempdue1 = entityHd.ARInvoiceDate.AddDays(term.TermDay);
                            }
                            else if (hdnSetvarHitungJatuhTempoDari.Value == "2")
                            {
                                tempdue1 = entityHd.DocumentDate.AddDays(term.TermDay);
                            }
                            else if (hdnSetvarHitungJatuhTempoDari.Value == "3")
                            {
                                tempdue1 = entityHd.ARDocumentReceiveDate.AddDays(term.TermDay);
                            }
                            else
                            {
                                tempdue1 = entityHd.DocumentDate.AddDays(term.TermDay);
                            }

                            DateTime tempdue2 = tempdue1.AddDays(Convert.ToInt32(hdnSetvarLeadTime.Value));
                            entityHd.DueDate = tempdue2;
                            
                            entityHd.ARInvoiceNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.AR_INVOICE_PAYER, entityHd.ARInvoiceDate, ctx);
                            entityHd.CreatedBy = AppSession.UserLogin.UserID;

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityHd.ARInvoiceID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);

                            for (int i = 0; i < paramPaymentDetailID.Count(); ++i)
                            {
                                PatientPaymentDtInfo paymentDtInfo = lstPaymentDtInfo.FirstOrDefault(p => p.PaymentDetailID == Convert.ToInt32(paramPaymentDetailID[i]));
                                PatientPaymentDt paymentDt = lstPaymentDt.FirstOrDefault(p => p.PaymentDetailID == Convert.ToInt32(paramPaymentDetailID[i]));
                                PatientPaymentHd paymentHd = lstPaymentHd.FirstOrDefault(p => p.PaymentID == paymentDt.PaymentID);

                                ARInvoiceDt entityDt = new ARInvoiceDt();
                                entityDt.ARInvoiceID = entityHd.ARInvoiceID;
                                entityDt.PaymentDetailID = paymentDt.PaymentDetailID;
                                entityDt.RegistrationID = paymentHd.RegistrationID;
                                entityDt.PaymentID = paymentHd.PaymentID;
                                entityDt.GCTransactionDetailStatus = Constant.TransactionStatus.OPEN;
                                if (paramReferenceNoPrefix[i] != "" && paramReferenceNo[i] != "")
                                {
                                    entityDt.ReferenceNo = string.Format("{0}/{1}", paramReferenceNoPrefix[i], paramReferenceNo[i]);
                                }
                                else if (paramReferenceNoPrefix[i] != "" && paramReferenceNo[i] == "")
                                {
                                    entityDt.ReferenceNo = paramReferenceNoPrefix[i];
                                }
                                else if (paramReferenceNoPrefix[i] == "" && paramReferenceNo[i] != "")
                                {
                                    entityDt.ReferenceNo = paramReferenceNo[i];
                                }
                                else
                                {
                                    entityDt.ReferenceNo = "";
                                }

                                if (hdnIsFinalisasiKlaimAfterARInvoice.Value == "0")
                                {
                                    entityDt.TransactionAmount = entityDt.ClaimedAmount = entityDt.GrouperAmountFinal = paymentDtInfo.GrouperAmountFinal;
                                    entityDt.GrouperAmountClaim = paymentDtInfo.GrouperAmountClaim;
                                }
                                else
                                {
                                    entityDt.TransactionAmount = entityDt.ClaimedAmount = entityDt.GrouperAmountClaim = paymentDtInfo.GrouperAmountClaim;
                                }

                                entityDt.CreatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Insert(entityDt);
                            }
                            retval = entityHd.ARInvoiceNo;
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
                        result = false;
                        errMessage = "Piutang tidak dapat diproses. Harap refresh halaman ini.";
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Piutang tidak dapat diproses. Ada nomor piutang yang sudah diproses.";
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
            finally
            {
                ctx.Close();
            }
            return result;
        }
    }
}