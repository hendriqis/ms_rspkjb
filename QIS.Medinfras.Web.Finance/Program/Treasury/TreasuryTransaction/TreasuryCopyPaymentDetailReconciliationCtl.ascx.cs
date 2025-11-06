using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class TreasuryCopyPaymentDetailReconciliationCtl : BaseEntryPopupCtl
    {
        private TreasuryTransactionEntry DetailPage
        {
            get { return (TreasuryTransactionEntry)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            string[] paramList = param.Split('|');
            hdnGLTransactionIDctl.Value = paramList[0];
            hdnGLAccountTreasuryIDctl.Value = paramList[1];
            hdnTreasuryTypectl.Value = paramList[2];
            hdnDepartmentIDCtl.Value = paramList[3];
            hdnServiceUnitIDCtl.Value = paramList[4];
            hdnBusinessPartnerIDCtl.Value = paramList[5];
            hdnCashFlowTypeIDCtl.Value = paramList[6];

            txtPeriodFrom.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPeriodTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            string filterDisplay = string.Format("GLTransactionID = {0} AND TransactionDtID = (SELECT MAX(TransactionDtID) FROM GLTransactionDt WHERE GLTransactionID = {0} AND IsDeleted = 0 AND GCItemDetailStatus != '{1}')", hdnGLTransactionIDctl.Value, Constant.TransactionStatus.VOID);
            GLTransactionDt glDT = BusinessLayer.GetGLTransactionDtList(filterDisplay).FirstOrDefault();
            if (glDT != null)
            {
                hdnDisplayOrderTemp.Value = glDT.DisplayOrder.ToString();
            }

            BindGridView();
        }

        #region Bind Grid
        private void BindGridView()
        {
            string filterExpression = string.Format("PaymentAmount != 0");

            filterExpression += " AND ";
            filterExpression += string.Format("PaymentDate BETWEEN '{0}' AND '{1}'", Helper.GetDatePickerValue(txtPeriodFrom.Text).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtPeriodTo.Text).ToString(Constant.FormatString.DATE_FORMAT_112));

            if (hdnFilterExpressionQuickSearch.Value != null && hdnFilterExpressionQuickSearch.Value != "")
            {
                filterExpression += " AND ";
                filterExpression += hdnFilterExpressionQuickSearch.Value;
            }

            List<vPatientPaymentDtCopyReconciled> lstEntity = BusinessLayer.GetvPatientPaymentDtCopyReconciledList(filterExpression, int.MaxValue, 1, "CreatedDate, PaymentID, PaymentDetailID");
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
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
        private void ControlToEntity(IDbContext ctx, List<GLTransactionDt> lstEntity)
        {
            GLTransactionDtDao glTransactionDtDao = new GLTransactionDtDao(ctx);
            GLTransactionHdDao glTransactionHdDao = new GLTransactionHdDao(ctx);
            PatientPaymentDtInfoDao ppdtInfoDao = new PatientPaymentDtInfoDao(ctx);
            PatientPaymentHdDao paymentHdDao = new PatientPaymentHdDao(ctx);

            int count = 0;
            List<String> lstSelectedPaymentDetailID = hdnSelectedPaymentDetailID.Value.Split(',').ToList();
            List<String> lstSelectedPaymentAmount = hdnSelectedPaymentAmount.Value.Split(',').ToList();
            List<String> lstSelectedRemarks = hdnSelectedRemarks.Value.Split(',').ToList();

            lstSelectedPaymentDetailID.RemoveAt(0);
            lstSelectedPaymentAmount.RemoveAt(0);
            lstSelectedRemarks.RemoveAt(0);

            string filterExpression = string.Format("PaymentDetailID IN ({0})", hdnSelectedPaymentDetailID.Value.Substring(1));
            List<vPatientPaymentDtCopyReconciled> lstHd = BusinessLayer.GetvPatientPaymentDtCopyReconciledList(filterExpression, ctx);

            foreach (vPatientPaymentDtCopyReconciled entity in lstHd)
            {
                PatientPaymentDtInfo ppdtInfo = ppdtInfoDao.Get(entity.PaymentDetailID);

                PatientPaymentHd paymentHd = paymentHdDao.Get(entity.PaymentID);

                string filterVisit = string.Format("RegistrationID = {0}", paymentHd.RegistrationID);
                ConsultVisit visit = BusinessLayer.GetConsultVisitList(filterVisit, ctx).FirstOrDefault();

                GLTransactionDt entityDt = new GLTransactionDt();
                entityDt.ReferenceNo = string.Format("{0}", entity.PaymentDetailID);
                entityDt.HealthcareID = AppSession.UserLogin.HealthcareID;
                entityDt.BusinessPartnerID = Convert.ToInt32(hdnBusinessPartnerIDCtl.Value);
                entityDt.DepartmentID = hdnDepartmentIDCtl.Value;
                if (hdnServiceUnitIDCtl.Value != "" && hdnServiceUnitIDCtl.Value != null)
                {
                    entityDt.ServiceUnitID = Convert.ToInt32(hdnServiceUnitIDCtl.Value);
                }

                entityDt.CashFlowTypeID = Convert.ToInt32(hdnCashFlowTypeIDCtl.Value);

                int oEDCMachineID = entity.EDCMachineID != null ? entity.EDCMachineID : 0;
                int oBankID = entity.BankID != null ? entity.BankID : 0;

                //string filterPPMEB = string.Format("GCPaymentMethod = '{0}' AND ISNULL(EDCMachineID,0) = {1} AND ISNULL(BankID,0) = {2} AND GCCashierGroup = '{3}' AND IsDeleted = 0",
                //                                        entity.GCPaymentMethod, oEDCMachineID, oBankID, entity.GCCashierGroup);
                //List<GLPatientPaymentMethodEDCBank> ppMethodEDCBankLst = BusinessLayer.GetGLPatientPaymentMethodEDCBankList(filterPPMEB, ctx);
                //if (ppMethodEDCBankLst.Count() > 0)
                //{
                //    entityDt.GLAccount = ppMethodEDCBankLst.LastOrDefault().GLAccount;
                //}

                List<GetGLAccountForTreasuryCopyPaymentDetailRecon> entityGL = BusinessLayer.GetGLAccountForTreasuryCopyPaymentDetailRecon(entity.GCPaymentMethod, oEDCMachineID, oBankID, entity.GCCashierGroup, visit.HealthcareServiceUnitID);
                if (entityGL.Count() > 0)
                {
                    entityDt.GLAccount = entityGL.LastOrDefault().GLAccount;
                }

                if (entityDt.GLAccount == null || entityDt.GLAccount == 0)
                {
                    string filterPPM = string.Format("GCPaymentMethod = '{0}' AND IsDeleted = 0", entity.GCPaymentMethod);
                    List<GLPatientPaymentMethod> ppMethodLst = BusinessLayer.GetGLPatientPaymentMethodList(filterPPM, ctx);
                    if (ppMethodLst.Count() > 0)
                    {
                        entityDt.GLAccount = ppMethodLst.LastOrDefault().GLAccount;
                    }
                }

                decimal paymentAmount = entity.PaymentAmount;

                if (paymentAmount < 0)
                {
                    paymentAmount = paymentAmount * -1;

                    entityDt.Position = "D";
                    entityDt.DebitAmount = paymentAmount;
                    entityDt.CreditAmount = 0;
                }
                else
                {
                    if (hdnTreasuryTypectl.Value == Constant.TreasuryType.PENERIMAAN)
                    {
                        entityDt.Position = "K";
                        entityDt.DebitAmount = 0;
                        entityDt.CreditAmount = paymentAmount;
                    }
                    else
                    {
                        entityDt.Position = "D";
                        entityDt.DebitAmount = paymentAmount;
                        entityDt.CreditAmount = 0;
                    }
                }

                int displayOrder = Convert.ToInt16(hdnDisplayOrderTemp.Value) + count + 1;
                entityDt.DisplayOrder = Convert.ToInt16(displayOrder);

                string remarks = lstSelectedRemarks[count];
                entityDt.Remarks = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}",
                                                    entity.PaymentDate.ToString(Constant.FormatString.DATE_FORMAT),
                                                    entity.PaymentMethod,
                                                    entity.CreatedByName,
                                                    entity.PaymentNo,
                                                    entity.MedicalNo,
                                                    entity.PatientName,
                                                    remarks);
                entityDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;

                entityDt.IsReferenceNoGeneratedBySystem = true;

                entityDt.GLTransactionID = Convert.ToInt32(hdnGLTransactionIDctl.Value);
                entityDt.CreatedBy = AppSession.UserLogin.UserID;
                int glDtID = glTransactionDtDao.InsertReturnPrimaryKeyID(entityDt);

                lstEntity.Add(entityDt);

                ppdtInfo.IsReconciled = true;
                ppdtInfo.ReconciledBy = AppSession.UserLogin.UserID;
                ppdtInfo.ReconciledDate = DateTime.Now;
                ppdtInfo.GLTransactionDtID = glDtID;
                ppdtInfoDao.Update(ppdtInfo);

                count++;

            }

        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            GLTransactionHdDao glHdDao = new GLTransactionHdDao(ctx);
            GLTransactionDtDao glDtDao = new GLTransactionDtDao(ctx);
            TransactionTypeLockDao entityLockDao = new TransactionTypeLockDao(ctx);

            int GLTransactionID = 0;
            string errorMessage = "";
            try
            {
                DetailPage.SaveGLTransactionHd(ctx, ref GLTransactionID, ref errorMessage);
                GLTransactionHd entityHd = glHdDao.Get(GLTransactionID);
                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    if (entityHd.TransactionCode == "7281" || entityHd.TransactionCode == "7282" || entityHd.TransactionCode == "7283" || entityHd.TransactionCode == "7284" || entityHd.TransactionCode == "7285" || entityHd.TransactionCode == "7286" || entityHd.TransactionCode == "7287" || entityHd.TransactionCode == "7288" || entityHd.TransactionCode == "7299")
                    {
                        TransactionTypeLock entityLock = entityLockDao.Get(entityHd.TransactionCode);
                        if (entityLock.LockedUntilDate != null)
                        {
                            DateTime DateLock = Convert.ToDateTime(entityLock.LockedUntilDate);
                            DateTime DateNow = entityHd.JournalDate;
                            if (DateNow > DateLock)
                            {
                                List<GLTransactionDt> lstEntityDt = new List<GLTransactionDt>();

                                ControlToEntity(ctx, lstEntityDt);

                                retval = GLTransactionID.ToString();
                                ctx.CommitTransaction();
                            }
                            else
                            {
                                errMessage = string.Format("This Transaction Type has been Locked until {0}. Please contact an authorized personnel.", Convert.ToDateTime(entityLock.LockedUntilDate).ToString(Constant.FormatString.DATE_FORMAT));
                                result = false;
                                Exception ex = new Exception(errMessage);
                                Helper.InsertErrorLog(ex);
                                ctx.RollBackTransaction();
                            }
                        }
                    }
                    else
                    {
                        List<GLTransactionDt> lstEntityDt = new List<GLTransactionDt>();

                        ControlToEntity(ctx, lstEntityDt);

                        retval = GLTransactionID.ToString();
                        ctx.CommitTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
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

        #endregion
    }
}