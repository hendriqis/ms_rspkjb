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
    public partial class TreasuryCopySupplierPaymentCtl : BaseEntryPopupCtl
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
            hdnJournalDateCtl.Value = paramList[7];

            string filterSetVar = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN0171);
            List<SettingParameterDt> lstSetVar = BusinessLayer.GetSettingParameterDtList(filterSetVar);

            hdnFN0171.Value = lstSetVar.Where(a => a.ParameterCode == Constant.SettingParameter.FN0171).FirstOrDefault().ParameterValue;

            string filterDisplay = string.Format("GLTransactionID = {0} AND TransactionDtID = (SELECT MAX(TransactionDtID) FROM GLTransactionDt WITH(NOLOCK) WHERE GLTransactionID = {0} AND IsDeleted = 0 AND GCItemDetailStatus != '{1}')", hdnGLTransactionIDctl.Value, Constant.TransactionStatus.VOID);
            GLTransactionDt glDT = BusinessLayer.GetGLTransactionDtList(filterDisplay).FirstOrDefault();
            if (glDT != null)
            {
                hdnDisplayOrderTemp.Value = glDT.DisplayOrder.ToString();
            }

            List<Variable> lstFilterData = new List<Variable>();
            if (hdnFN0171.Value == "1")
            {
                lstFilterData.Add(new Variable { Code = "0", Value = "---SEMUA---" });
            }
            lstFilterData.Add(new Variable { Code = "1", Value = "Tgl Rencana Bayar <= Tgl Voucher" });
            lstFilterData.Add(new Variable { Code = "2", Value = "Tgl Rencana Bayar > Tgl Voucher" });
            Methods.SetComboBoxField<Variable>(cboFilterData, lstFilterData, "Value", "Code");
            cboFilterData.Value = "1";

            List<StandardCode> listStandardCode = BusinessLayer.GetStandardCodeList(string.Format(
                                                                    "ParentID IN ('{0}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.SUPPLIER_PAYMENT_METHOD));
            listStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboPaymentMethod, listStandardCode.Where(a => a.ParentID == Constant.StandardCode.SUPPLIER_PAYMENT_METHOD || a.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            cboPaymentMethod.Value = "";

            List<Bank> lstBank = BusinessLayer.GetBankList(string.Format("IsDeleted = 0 AND (GCBankType = '{0}' OR GCBankType IS NULL)", Constant.BankType.BANK_HUTANG));
            lstBank.Insert(0, new Bank { BankID = 0, BankName = "" });
            Methods.SetComboBoxField<Bank>(cboBank, lstBank, "BankName", "BankID");

            BindGridView();
        }

        #region Bind Grid
        private void BindGridView()
        {
            string filterExpression = string.Format("GCTransactionStatus = '{0}' AND GCApprovalTransactionStatus = '{1}'", Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionStatus.APPROVED);
            
            filterExpression += string.Format(" AND SupplierPaymentID IN (SELECT SupplierPaymentID FROM vSupplierPaymentAP WHERE AP IS NOT NULL)");

            if (cboFilterData.Value.ToString() == "1")
            {
                filterExpression += string.Format(" AND CONVERT(VARCHAR(8), PlanningPaymentDate, 112) <= '{0}'", hdnJournalDateCtl.Value);
            }
            else if (cboFilterData.Value.ToString() == "2")
            {
                filterExpression += string.Format(" AND CONVERT(VARCHAR(8), PlanningPaymentDate, 112) > '{0}'", hdnJournalDateCtl.Value);
            }

            if (cboPaymentMethod.Value != null)
            {
                filterExpression += string.Format(" AND GCSupplierPaymentMethod = '{0}'", cboPaymentMethod.Value.ToString());
            }

            if (cboBank.Value != null)
            {
                filterExpression += string.Format(" AND BankID = '{0}'", cboBank.Value.ToString());
            }

            if (hdnFilterExpressionQuickSearch.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);

            filterExpression += string.Format(" ORDER BY PaymentDate DESC");

            List<vSupplierPaymentHd> lstEntity = BusinessLayer.GetvSupplierPaymentHdList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vSupplierPaymentHd entity = e.Item.DataItem as vSupplierPaymentHd;

                CheckBox chkSelectSupplierPayment = e.Item.FindControl("chkSelectSupplierPayment") as CheckBox;
                chkSelectSupplierPayment.Checked = false;

                if (hdnFN0171.Value == "0")
                {
                    if (cboFilterData.Value.ToString() == "1")
                    {
                        chkSelectSupplierPayment.Enabled = true;
                    }
                    else
                    {
                        chkSelectSupplierPayment.Enabled = false;
                    }
                }
                else
                {
                    chkSelectSupplierPayment.Enabled = true;
                }

                TextBox txtRemarks = e.Item.FindControl("txtRemarks") as TextBox;
                txtRemarks.Text = entity.Remarks.ToString();

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
            SupplierPaymentHdDao sphDao = new SupplierPaymentHdDao(ctx);
            SupplierPaymentDtDao spdDao = new SupplierPaymentDtDao(ctx);

            string filterExpression = string.Format("SupplierPaymentID IN ({0})", hdnSelectedSupplierPaymentID.Value.Substring(1));
            List<vSupplierPaymentHd> lstvSPH = BusinessLayer.GetvSupplierPaymentHdList(filterExpression, ctx);
            List<SupplierPaymentHd> lstSPH = BusinessLayer.GetSupplierPaymentHdList(filterExpression, ctx);

            int count = 0;
            List<String> lstSelectedSupplierPayment = hdnSelectedSupplierPaymentID.Value.Split(',').ToList();
            List<String> lstSelectedRemarks = hdnSelectedRemarks.Value.Split(',').ToList();
            lstSelectedSupplierPayment.RemoveAt(0);
            lstSelectedRemarks.RemoveAt(0);

            GLTransactionHd treasuryHd = glTransactionHdDao.Get(Convert.ToInt32(hdnGLTransactionIDctl.Value));

            foreach (vSupplierPaymentHd entity in lstvSPH)
            {
                SupplierPaymentHd entitySPH = lstSPH.FirstOrDefault(p => p.SupplierPaymentID == entity.SupplierPaymentID);

                string filterAP = string.Format("SupplierPaymentID = {0}", entitySPH.SupplierPaymentID);
                List<vSupplierPaymentAP> lstPaymentAP = BusinessLayer.GetvSupplierPaymentAPList(filterAP, ctx);
                foreach (vSupplierPaymentAP paymentAP in lstPaymentAP)
                {
                    GLTransactionDt entityDt = new GLTransactionDt();
                    entityDt.ReferenceNo = entitySPH.SupplierPaymentNo;
                    entityDt.HealthcareID = AppSession.UserLogin.HealthcareID;

                    entityDt.CashFlowTypeID = Convert.ToInt32(hdnCashFlowTypeIDCtl.Value);

                    if (paymentAP.BusinessPartnerID != null)
                    {
                        entityDt.BusinessPartnerID = paymentAP.BusinessPartnerID;
                    }
                    else
                    {
                        if (hdnBusinessPartnerIDCtl.Value != "" && hdnBusinessPartnerIDCtl.Value != null)
                        {
                            entityDt.BusinessPartnerID = Convert.ToInt32(hdnBusinessPartnerIDCtl.Value);
                        }
                    }

                    entityDt.DepartmentID = hdnDepartmentIDCtl.Value;
                    if (hdnServiceUnitIDCtl.Value != "" && hdnServiceUnitIDCtl.Value != null)
                    {
                        entityDt.ServiceUnitID = Convert.ToInt32(hdnServiceUnitIDCtl.Value);
                    }

                    entityDt.GLAccount = paymentAP.AP;

                    if (hdnTreasuryTypectl.Value == Constant.TreasuryType.PENERIMAAN)
                    {
                        entityDt.Position = "K";
                        entityDt.DebitAmount = 0;
                        entityDt.CreditAmount = paymentAP.PaymentAmount;
                    }
                    else
                    {
                        if (paymentAP.PaymentAmount < 0)
                        {
                            entityDt.Position = "K";
                            entityDt.DebitAmount = 0;
                            entityDt.CreditAmount = paymentAP.PaymentAmount * -1;
                        }
                        else
                        {
                            entityDt.Position = "D";
                            entityDt.DebitAmount = paymentAP.PaymentAmount;
                            entityDt.CreditAmount = 0;
                        }
                    }

                    int displayOrder = Convert.ToInt16(hdnDisplayOrderTemp.Value) + count + 1;
                    entityDt.DisplayOrder = Convert.ToInt16(displayOrder);

                    string remarks = lstSelectedRemarks[count];
                    entityDt.Remarks = string.Format("{0}|{1}|{2}|{3}",
                                                        paymentAP.PurchaseInvoiceNo,
                                                        paymentAP.BusinessPartnerName,
                                                        paymentAP.VerificationDate.ToString(Constant.FormatString.DATE_FORMAT),
                                                        remarks);
                    entityDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;

                    entityDt.IsReferenceNoGeneratedBySystem = true;

                    entityDt.GLTransactionID = treasuryHd.GLTransactionID;
                    entityDt.CreatedBy = AppSession.UserLogin.UserID;
                    int glDtID = glTransactionDtDao.InsertReturnPrimaryKeyID(entityDt);

                    SupplierPaymentDt entitySPD = spdDao.Get(paymentAP.SupplierPaymentID, paymentAP.PurchaseInvoiceID);
                    entitySPD.GLTransactionDtID = glDtID;
                    entitySPD.LastUpdatedBy = AppSession.UserLogin.UserID;
                    spdDao.Update(entitySPD);

                    entitySPH.GLTransactionID = treasuryHd.GLTransactionID;
                    entitySPH.PaymentDate = treasuryHd.JournalDate;
                    entitySPH.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                    entitySPH.ApprovedBy = AppSession.UserLogin.UserID;
                    entitySPH.ApprovedDate = treasuryHd.JournalDate;
                    entitySPH.LastUpdatedBy = AppSession.UserLogin.UserID;
                    sphDao.Update(entitySPH);

                    lstEntity.Add(entityDt);
                }

                count++;
            }

        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            GLTransactionHdDao glHdDao = new GLTransactionHdDao(ctx);
            GLTransactionDtDao glDtDao = new GLTransactionDtDao(ctx);
            SupplierPaymentHdDao sphDao = new SupplierPaymentHdDao(ctx);
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

        #region Get Constant
        protected string GetSupplierPaymentMethodTransfer()
        {
            return Constant.SupplierPaymentMethod.TRANSFER;
        }
        protected string GetSupplierPaymentMethodGiro()
        {
            return Constant.SupplierPaymentMethod.GIRO;
        }
        protected string GetSupplierPaymentMethodCheque()
        {
            return Constant.SupplierPaymentMethod.CHEQUE;
        }
        #endregion
    }
}