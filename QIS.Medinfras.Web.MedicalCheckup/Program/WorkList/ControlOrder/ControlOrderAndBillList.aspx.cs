using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;
using System.Data;

namespace QIS.Medinfras.Web.MedicalCheckup.Program
{
    public partial class ControlOrderAndBillList : BasePageContent
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalCheckup.CONTROL_ORDER_AND_BILL;
        }

        #region Error Message
        protected string GetErrMessageSelectRegistrationFirst()
        {
            return Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_SELECT_REGISTRATION_FIRST_VALIDATION);
        }
        #endregion

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
                List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}','{2}')", Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID, Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID, Constant.SettingParameter.FN_PEMBUATAN_TAGIHAN_JIKA_TIDAK_ADA_OUTSTANDING_ORDER));
                hdnImagingServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID).ParameterValue;
                hdnLaboratoryServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID).ParameterValue;
                hdnPembuatanTagihanTidakAdaOutstandingOrder.Value = lstSettingParameter.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_PEMBUATAN_TAGIHAN_JIKA_TIDAK_ADA_OUTSTANDING_ORDER).FirstOrDefault().ParameterValue;

                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;

                txtRegistrationDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                BindGridView();

                Helper.SetControlEntrySetting(txtRegistrationDate, new ControlEntrySetting(true, true, true), "mpPatientList");
                
                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());
            }
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string[] param = e.Parameter.Split('|');

            if (param[0] == "process")
            {
                result = "process|";
                if (OnProcessPatientBillPayment(ref errMessage))
                {
                    result += "success";
                }
                else
                {
                    result += "fail|" + errMessage;
                }

                BindGridView();
            }
            else
            {
                BindGridView();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vConsultVisitMCUItem entity = e.Item.DataItem as vConsultVisitMCUItem;
                HtmlInputHidden hdnKey = (HtmlInputHidden)e.Item.FindControl("hdnKey");
                HtmlInputHidden hdnRegistrationID = (HtmlInputHidden)e.Item.FindControl("hdnRegistrationID");
                HtmlInputHidden hdnVisitID = (HtmlInputHidden)e.Item.FindControl("hdnVisitID");
                HtmlInputHidden hdnRegistrationNo = (HtmlInputHidden)e.Item.FindControl("hdnRegistrationNo");
                HtmlInputHidden hdnPatientName = (HtmlInputHidden)e.Item.FindControl("hdnPatientName");
                HtmlInputHidden hdnReferenceNo = (HtmlInputHidden)e.Item.FindControl("hdnReferenceNo");
                HtmlInputHidden hdnItemID = (HtmlInputHidden)e.Item.FindControl("hdnItemID");
                HtmlInputHidden hdnItemCode = (HtmlInputHidden)e.Item.FindControl("hdnItemCode");
                HtmlInputHidden hdnItemName1 = (HtmlInputHidden)e.Item.FindControl("hdnItemName1");
                hdnKey.Value = entity.ID.ToString();
                hdnRegistrationID.Value = entity.RegistrationID.ToString();
                hdnVisitID.Value = entity.VisitID.ToString();
                hdnRegistrationNo.Value = entity.RegistrationNo;
                hdnPatientName.Value = entity.PatientName;
                hdnReferenceNo.Value = entity.ReferenceNo;
                hdnItemID.Value = entity.ItemID.ToString();
                hdnItemCode.Value = entity.ItemCode;
                hdnItemName1.Value = entity.ItemName1;
            }
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("VisitDate = '{0}' AND GCItemDetailStatus = '{1}' AND GCRegistrationStatus = '{2}' AND BusinessPartnerID != 1 AND RegistrationID IN (SELECT RegistrationID FROM Registration WHERE PaymentAmount = 0 AND RegistrationDate = '{0}') AND VisitID IN (SELECT VisitID FROM PatientChargesHd WHERE GCTransactionStatus != '{3}' AND TransactionDate = '{0}')", Helper.GetDatePickerValue(txtRegistrationDate.Text).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.TransactionStatus.PROCESSED, Constant.VisitStatus.CHECKED_IN, Constant.TransactionStatus.VOID);
            if (hdnFilterExpressionQuickSearch.Value != "")
            {
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            }
            filterExpression += " ORDER BY RegistrationID, ID";

            List<vConsultVisitMCUItem> lstEntity = BusinessLayer.GetvConsultVisitMCUItemList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();

            RowCountTable rc = new RowCountTable()
            {
                TotalRow = lstEntity.Count()
            };

            List<RowCountTable> rcList = new List<RowCountTable>();
            rcList.Add(rc);

            lvwViewCount.DataSource = rcList;
            lvwViewCount.DataBind();
        }

        private bool OnProcessPatientBillPayment(ref string errMessage)
        {
            bool result = true;

            string[] lstRegID = hdnSelectedMemberRegID.Value.ToString().Split(',');
            string[] lstVisitID = hdnSelectedMemberVisitID.Value.ToString().Split(',');

            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao entityDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            PatientBillDao patientBillDao = new PatientBillDao(ctx);
            RegistrationDao registrationDao = new RegistrationDao(ctx);
            RegistrationPayerDao registrationPayerDao = new RegistrationPayerDao(ctx);
            AuditLogDao entityAuditLogDao = new AuditLogDao(ctx);
            PatientPaymentHdDao entityPaymentHdDao = new PatientPaymentHdDao(ctx);
            PatientPaymentDtDao entityPaymentDtDao = new PatientPaymentDtDao(ctx);
            PatientPaymentDtInfoDao entityDtInfoDao = new PatientPaymentDtInfoDao(ctx);
            PatientBillPaymentDao patientBillPaymentDao = new PatientBillPaymentDao(ctx);
            HealthcareServiceUnitDao healthcareServiceUnitDao = new HealthcareServiceUnitDao(ctx);

            try
            {
                for (int i = 0; i < lstRegID.Length; i++)
                {
                    if (!string.IsNullOrEmpty(lstRegID[i]))
                    {
                        if (hdnPembuatanTagihanTidakAdaOutstandingOrder.Value == "0")
                        {
                            #region Tanpa Cek Outstanding Order

                            Registration entityReg = BusinessLayer.GetRegistration(Convert.ToInt32(lstRegID[i]));
                            string statusRegistration = entityReg.GCRegistrationStatus;

                            entityReg.IsLockDown = true;
                            entityReg.LockDownBy = AppSession.UserLogin.UserID;
                            entityReg.LockDownDate = DateTime.Now;
                            registrationDao.Update(entityReg);

                            if ((statusRegistration != Constant.VisitStatus.CLOSED)
                                || (!AppSession.IsUsedReopenBilling && !AppSession.RegisteredPatient.IsBillingReopen && statusRegistration != Constant.VisitStatus.CLOSED)
                                || (AppSession.IsUsedReopenBilling && AppSession.RegisteredPatient.IsBillingReopen && statusRegistration == Constant.VisitStatus.CLOSED))
                            {
                                PatientBill patientBill = null;
                                OnProcessBill(ctx, entityReg, ref patientBill, patientBillDao, entityDao, entityDtDao, lstVisitID[i]);
                                OnProcessPayment(ctx, entityReg, patientBill, entityPaymentHdDao, entityPaymentDtDao, entityDtInfoDao, patientBillPaymentDao, patientBillDao, entityDao, healthcareServiceUnitDao);
                                //ctx.CommitTransaction();
                            }
                            else
                            {
                                result = false;
                                errMessage = "Registrasi untuk pasien ini sudah di tutup/batal, tagihan sudah di transfer ke rawat inap";
                                Exception ex = new Exception(errMessage);
                                Helper.InsertErrorLog(ex);
                                ctx.RollBackTransaction();
                            }
                            #endregion
                        }
                        else
                        {
                            #region Cek Outstanding Order

                            List<TestOrderHd> lstPendingTestOrderHd = BusinessLayer.GetTestOrderHdList(string.Format("VisitID IN ({0}) AND GCTransactionStatus = '{1}'", lstVisitID[i], Constant.TransactionStatus.WAIT_FOR_APPROVAL), ctx);
                            if (lstPendingTestOrderHd.Count > 0)
                            {
                                result = false;
                                errMessage = "Masih Ada Order Penunjang Medis Yang Belum Direalisasi.";
                                Exception ex = new Exception(errMessage);
                                Helper.InsertErrorLog(ex);
                                ctx.RollBackTransaction();
                            }
                            else
                            {
                                List<ServiceOrderHd> lstPendingServiceOrderHd = BusinessLayer.GetServiceOrderHdList(string.Format("VisitID IN ({0}) AND GCTransactionStatus = '{1}'", lstVisitID[i], Constant.TransactionStatus.WAIT_FOR_APPROVAL), ctx);
                                if (lstPendingServiceOrderHd.Count > 0)
                                {
                                    result = false;
                                    errMessage = "Masih Ada Order Pelayanan Yang Belum Direalisasi.";
                                    Exception ex = new Exception(errMessage);
                                    Helper.InsertErrorLog(ex);
                                    ctx.RollBackTransaction();
                                }
                                else
                                {
                                    Registration entityReg = BusinessLayer.GetRegistration(Convert.ToInt32(lstRegID[i]));
                                    string statusRegistration = entityReg.GCRegistrationStatus;

                                    entityReg.IsLockDown = true;
                                    entityReg.LockDownBy = AppSession.UserLogin.UserID;
                                    entityReg.LockDownDate = DateTime.Now;
                                    registrationDao.Update(entityReg);

                                    if ((statusRegistration != Constant.VisitStatus.CLOSED)
                                        || (!AppSession.IsUsedReopenBilling && !AppSession.RegisteredPatient.IsBillingReopen && statusRegistration != Constant.VisitStatus.CLOSED)
                                        || (AppSession.IsUsedReopenBilling && AppSession.RegisteredPatient.IsBillingReopen && statusRegistration == Constant.VisitStatus.CLOSED))
                                    {
                                        PatientBill patientBill = null;
                                        OnProcessBill(ctx, entityReg, ref patientBill, patientBillDao, entityDao, entityDtDao, lstVisitID[i]);
                                        OnProcessPayment(ctx, entityReg, patientBill, entityPaymentHdDao, entityPaymentDtDao, entityDtInfoDao, patientBillPaymentDao, patientBillDao, entityDao, healthcareServiceUnitDao);
                                        //ctx.CommitTransaction();
                                    }
                                    else
                                    {
                                        result = false;
                                        errMessage = "Registrasi untuk pasien ini sudah di tutup/batal, tagihan sudah di transfer ke rawat inap";
                                        Exception ex = new Exception(errMessage);
                                        Helper.InsertErrorLog(ex);
                                        ctx.RollBackTransaction();
                                    }
                                }
                            }

                            #endregion
                        }
                    }
                }

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                ctx.RollBackTransaction();
                result = false;
            }
            finally
            {
                ctx.Close();
            }

            return result;
        }
        private void OnProcessBill(IDbContext ctx, Registration entityReg, ref PatientBill patientBill, PatientBillDao patientBillDao, PatientChargesHdDao entityDao, PatientChargesDtDao entityDtDao, string lstVisitID)
        {
            List<PatientChargesHd> lstPatientChargesHd = BusinessLayer.GetPatientChargesHdList(string.Format("VisitID IN ({0}) AND GCTransactionStatus IN ('{1}','{2}')", lstVisitID, Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionStatus.OPEN), ctx);
            if (lstPatientChargesHd.Count > 0)
            {
                patientBill = new PatientBill();
                patientBill.BillingDate = DateTime.Now;
                patientBill.BillingTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                patientBill.RegistrationID = entityReg.RegistrationID;
                string transactionCode = Constant.TransactionCode.MCU_PATIENT_BILL;

                patientBill.CoverageAmount = entityReg.ChargesAmount;

                patientBill.AdministrationFeeAmount = 0;
                patientBill.PatientAdminFeeAmount = 0;
                patientBill.ServiceFeeAmount = 0;
                patientBill.PatientServiceFeeAmount = 0;
                patientBill.DiffCoverageAmount = 0;
                patientBill.PatientBillingNo = BusinessLayer.GenerateTransactionNo(transactionCode, patientBill.BillingDate, ctx);
                patientBill.GCTransactionStatus = Constant.TransactionStatus.CLOSED;

                patientBill.TotalPatientAmount = 0;
                patientBill.TotalPayerAmount = entityReg.ChargesAmount;

                patientBill.GCVoidReason = null;
                patientBill.VoidReason = null;
                patientBill.CreatedBy = AppSession.UserLogin.UserID;

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                patientBill.PatientBillingID = patientBillDao.InsertReturnPrimaryKeyID(patientBill);

                DateTime maxProposedDate = Convert.ToDateTime(Constant.ConstantDate.DEFAULT_NULL);
                Int32 maxProposedBy = 0;
                decimal totalAmount = 0;

                foreach (PatientChargesHd hd in lstPatientChargesHd)
                {
                    List<PatientChargesDt> lstAllPatientChargesDt = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID IN ({0}) AND LocationID IS NOT NULL AND IsApproved = 0 AND IsDeleted = 0", hd.TransactionID), ctx);

                    totalAmount += hd.TotalAmount;

                    if (hd.ProposedDate != null)
                    {
                        if (maxProposedDate == Convert.ToDateTime(Constant.ConstantDate.DEFAULT_NULL))
                        {
                            maxProposedDate = hd.ProposedDate;
                            maxProposedBy = Convert.ToInt32(hd.ProposedBy);
                        }
                        else
                        {
                            if (hd.ProposedDate > maxProposedDate)
                            {
                                maxProposedDate = hd.ProposedDate;
                                maxProposedBy = Convert.ToInt32(hd.ProposedBy);
                            }
                        }
                    }

                    hd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                    hd.PatientBillingID = patientBill.PatientBillingID;
                    hd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDao.Update(hd);

                    List<PatientChargesDt> lstPatientChargesDt = lstAllPatientChargesDt.Where(p => p.TransactionID == hd.TransactionID).ToList();
                    foreach (PatientChargesDt patientChargesDt in lstPatientChargesDt)
                    {
                        patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.PROCESSED; // dibuka comment nya oleh RN di 20200531
                        patientChargesDt.IsApproved = true;
                        patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(patientChargesDt);
                    }
                }
                if (maxProposedBy != 0)
                {
                    patientBill.LastChargesProposedBy = maxProposedBy;
                    patientBill.LastChargesProposedDate = maxProposedDate;
                }
                patientBill.LastUpdatedBy = AppSession.UserLogin.UserID;
                patientBill.TotalAmount = totalAmount;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                patientBillDao.Update(patientBill);
            }
        }
        private void OnProcessPayment(IDbContext ctx, Registration entityReg, PatientBill entityBill, PatientPaymentHdDao entityPaymentHdDao, PatientPaymentDtDao entityPaymentDtDao, PatientPaymentDtInfoDao entityDtInfoDao, PatientBillPaymentDao  patientBillPaymentDao, PatientBillDao patientBillDao, PatientChargesHdDao patientChargesHdDao, HealthcareServiceUnitDao healthcareServiceUnitDao)
        {
            #region Payment Hd
            PatientPaymentHd entityHd = new PatientPaymentHd();
            entityHd.PaymentDate = DateTime.Now;
            entityHd.PaymentTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            entityHd.GCPaymentType = Constant.PaymentType.AR_PAYER;
            entityHd.RegistrationID = entityReg.RegistrationID;
            //entityHd.GCCashierGroup = cboCashierGroup.Value.ToString();

            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(String.Format(
                                            "ParentID IN ('{0}','{1}') AND IsActive = 1 AND IsDeleted = 0",
                                            Constant.StandardCode.CASHIER_GROUP, //0
                                            Constant.StandardCode.SHIFT //1
                                        ), ctx);
            List<StandardCode> lstShift = lstSc.Where(p => p.ParentID == Constant.StandardCode.SHIFT).ToList();
            hdnShift.Value = string.Join("|", lstShift.Select(p => string.Format("{0},{1}", p.StandardCodeID, p.TagProperty)).ToList());
            List<StandardCode> lstCashier = lstSc.Where(p => p.ParentID == Constant.StandardCode.CASHIER_GROUP).ToList();
            hdnCashierGroup.Value = lstCashier.FirstOrDefault().StandardCodeID;
            SetGCShiftValue();
            entityHd.GCShift = hdnShift.Value.ToString();
            entityHd.GCCashierGroup = hdnCashierGroup.Value;

            entityHd.PatientRoundingAmount = 0;

            entityHd.TotalPatientBillAmount = 0;
            entityHd.TotalPaymentAmount = entityBill.TotalPayerAmount;
            entityHd.TotalPayerBillAmount = entityBill.TotalPayerAmount;
            entityHd.CashBackAmount = 0;

            entityHd.Remarks = string.Empty;
            entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;

            string transactionCode = "";
            switch (entityHd.GCPaymentType)
            {
                case Constant.PaymentType.SETTLEMENT: transactionCode = Constant.TransactionCode.MCU_PATIENT_PAYMENT_SETTLEMENT; break;
                case Constant.PaymentType.AR_PATIENT: transactionCode = Constant.TransactionCode.MCU_PATIENT_PAYMENT_AR_PATIENT; break;
                default: transactionCode = Constant.TransactionCode.MCU_PATIENT_PAYMENT_AR_PATIENT; break;
            }
            entityHd.PaymentNo = BusinessLayer.GenerateTransactionNo(transactionCode, entityHd.PaymentDate, ctx);
            //entityHd.CreatedBy = entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
            entityHd.CreatedBy = AppSession.UserLogin.UserID;
            ctx.CommandType = CommandType.Text;
            ctx.Command.Parameters.Clear();
            entityHd.PaymentID = entityPaymentHdDao.InsertReturnPrimaryKeyID(entityHd);
            #endregion

            #region Payment Dt
            PatientPaymentDt entityDt = new PatientPaymentDt();
            entityDt.PaymentID = entityHd.PaymentID;
            entityDt.GCPaymentMethod = Constant.PaymentMethod.CREDIT;
            entityDt.BusinessPartnerID = entityReg.BusinessPartnerID;
            entityDt.PaymentAmount = entityBill.TotalPayerAmount;
            entityDt.CardFeeAmount = 0;
            entityDt.CreatedBy = AppSession.UserLogin.UserID;
            ctx.CommandType = CommandType.Text;
            ctx.Command.Parameters.Clear();
            int paymentDetailID = entityPaymentDtDao.InsertReturnPrimaryKeyID(entityDt);

            PatientPaymentDtInfo dtInfo = new PatientPaymentDtInfo();
            dtInfo.PaymentDetailID = paymentDetailID;
            dtInfo.GCClaimStatus = Constant.ClaimStatus.APPROVED;
            dtInfo.GCFinalStatus = Constant.FinalStatus.APPROVED;
            dtInfo.GrouperAmountClaim = dtInfo.GrouperAmountFinal = entityDt.PaymentAmount;
            dtInfo.ClaimBy = dtInfo.FinalBy = AppSession.UserLogin.UserID;
            dtInfo.ClaimDate = dtInfo.FinalDate = DateTime.Now;
            dtInfo.SequenceNo = Convert.ToInt32(entityReg.RegistrationDate.ToString("dd"));
            ctx.CommandType = CommandType.Text;
            ctx.Command.Parameters.Clear();
            entityDtInfoDao.Insert(dtInfo);
            #endregion

            #region Update Billing
            ctx.CommandType = CommandType.Text;
            ctx.Command.Parameters.Clear();
            List<PatientChargesHd> lstPatientChargesHd = BusinessLayer.GetPatientChargesHdList(string.Format("PatientBillingID IN ({0})", entityBill.PatientBillingID), ctx);
            decimal totalPaymentAmount = entityHd.TotalPaymentAmount;
            string oldBillStatus = null;
            if (totalPaymentAmount > 0)
            {
                oldBillStatus = entityBill.GCTransactionStatus;

                entityBill.PaymentID = entityHd.PaymentID;
                entityBill.TotalPatientPaymentAmount = entityBill.TotalPayerAmount;

                PatientBillPayment patientBillPayment = new PatientBillPayment();
                patientBillPayment.PaymentID = entityHd.PaymentID;
                patientBillPayment.PatientBillingID = entityBill.PatientBillingID;
                patientBillPayment.PatientPaymentAmount = 0;

                if (entityBill.RemainingAmount < 1)
                {
                    entityBill.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                    //lstChargesHd = lstPatientChargesHd.Where(p => p.PatientBillingID == entityBill.PatientBillingID).ToList();
                    foreach (PatientChargesHd patientChargesHd in lstPatientChargesHd)
                    {
                        patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                        patientChargesHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        patientChargesHdDao.Update(patientChargesHd);
                    }
                }

                if (oldBillStatus != entityBill.GCTransactionStatus)
                {
                    entityBill.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityBill.LastUpdatedDate = DateTime.Now;
                }

                patientBillDao.Update(entityBill);

                patientBillPaymentDao.Insert(patientBillPayment);
            }
            #endregion
        }

        private void SetGCShiftValue()
        {
            string timeNow = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            string[] arrListShift = hdnShift.Value.Split('|');
            foreach (string listShift in arrListShift)
            {
                string[] temp = listShift.Split(',');
                string[] shiftTime = temp[1].Split(';');
                if (string.Compare(shiftTime[0], shiftTime[1]) < 0)
                {
                    if (string.Compare(shiftTime[0], timeNow) <= 0 && string.Compare(shiftTime[1], timeNow) >= 0)
                    {
                        hdnShift.Value = temp[0];
                        break;
                    }
                }
                else
                {
                    if (string.Compare(shiftTime[0], timeNow) >= 0 || string.Compare(shiftTime[1], timeNow) <= 0)
                    {
                        hdnShift.Value = temp[0];
                        break;
                    }
                }
            }
        }
        private class RowCountTable
        {
            private Int32 totalRow;

            public Int32 TotalRow
            {
                get { return totalRow; }
                set { totalRow = value; }
            }
        }
    }
}