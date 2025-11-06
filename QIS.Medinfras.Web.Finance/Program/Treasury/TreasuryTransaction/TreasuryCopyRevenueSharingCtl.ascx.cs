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
    public partial class TreasuryCopyRevenueSharingCtl : BaseEntryPopupCtl
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
            hdnCOARevenueSharingCtl.Value = paramList[7];

            string filterDisplay = string.Format("GLTransactionID = {0} AND TransactionDtID = (SELECT MAX(TransactionDtID) FROM GLTransactionDt WHERE GLTransactionID = {0} AND IsDeleted = 0 AND GCItemDetailStatus != '{1}')", hdnGLTransactionIDctl.Value, Constant.TransactionStatus.VOID);
            GLTransactionDt glDT = BusinessLayer.GetGLTransactionDtList(filterDisplay).FirstOrDefault();
            if (glDT != null)
            {
                hdnDisplayOrderTemp.Value = glDT.DisplayOrder.ToString();
            }

            List<StandardCode> listStandardCode = BusinessLayer.GetStandardCodeList(string.Format(
                                                                    "ParentID IN ('{0}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.REVENUE_PAYMENT_METHOD));
            listStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboPaymentMethod, listStandardCode.Where(a => a.ParentID == Constant.StandardCode.REVENUE_PAYMENT_METHOD || a.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            cboPaymentMethod.Value = "";

            List<Bank> lstBank = BusinessLayer.GetBankList(string.Format("IsDeleted = 0 AND (GCBankType = '{0}' OR GCBankType IS NULL)", Constant.BankType.BANK_HUTANG));
            lstBank.Insert(0, new Bank { BankID = 0, BankName = "" });
            Methods.SetComboBoxField<Bank>(cboBank, lstBank, "BankName", "BankID");

            BindGridView();
        }

        #region Bind Grid
        private void BindGridView()
        {
            string filterExpression = string.Format("GCTransactionStatus = '{0}' AND GLTransactionID IS NULL", Constant.TransactionStatus.WAIT_FOR_APPROVAL);

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

            List<vTransRevenueSharingPaymentHd> lstEntity = BusinessLayer.GetvTransRevenueSharingPaymentHdList(filterExpression);
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
            TransRevenueSharingPaymentHdDao paymentHdDao = new TransRevenueSharingPaymentHdDao(ctx);
            TransRevenueSharingPaymentDtDao paymentDtDao = new TransRevenueSharingPaymentDtDao(ctx);
            RevenueSharingFeeHdDao revSharingFeeHdDao = new RevenueSharingFeeHdDao(ctx);

            int count = 0;
            List<String> lstSelectedRSPayment = hdnSelectedRSPaymentID.Value.Split(',').ToList();
            List<String> lstSelectedRemarks = hdnSelectedRemarks.Value.Split(',').ToList();
            lstSelectedRSPayment.RemoveAt(0);
            lstSelectedRemarks.RemoveAt(0);

            string filterExpression = string.Format("RSPaymentID IN ({0})", hdnSelectedRSPaymentID.Value.Substring(1));
            List<TransRevenueSharingPaymentHd> lstHd = BusinessLayer.GetTransRevenueSharingPaymentHdList(filterExpression, ctx);

            GLTransactionHd treasuryHd = glTransactionHdDao.Get(Convert.ToInt32(hdnGLTransactionIDctl.Value));

            foreach (TransRevenueSharingPaymentHd entity in lstHd)
            {
                TransRevenueSharingPaymentHd paymentHd = lstHd.FirstOrDefault(p => p.RSPaymentID == entity.RSPaymentID);

                #region TransRevenueSharingPaymentDt

                string filterDt = string.Format("RSPaymentID = {0} AND IsDeleted = 0", entity.RSPaymentID);
                List<TransRevenueSharingPaymentDt> lstPaymentDt = BusinessLayer.GetTransRevenueSharingPaymentDtList(filterDt, ctx);
                foreach (TransRevenueSharingPaymentDt paymentDt in lstPaymentDt)
                {
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();

                    GLTransactionDt entityDt = new GLTransactionDt();
                    entityDt.ReferenceNo = paymentHd.RSPaymentNo;
                    entityDt.HealthcareID = AppSession.UserLogin.HealthcareID;

                    entityDt.CashFlowTypeID = Convert.ToInt32(hdnCashFlowTypeIDCtl.Value);

                    vTransRevenueSharingSummaryHd summaryHd = BusinessLayer.GetvTransRevenueSharingSummaryHdList(string.Format("RSSummaryID = {0}", paymentDt.RSSummaryID), ctx).FirstOrDefault();
                    string filterBP = string.Format("ParamedicID = {0}", summaryHd.ParamedicID);
                    List<BusinessPartners> lstBP = BusinessLayer.GetBusinessPartnersList(filterBP, ctx);
                    if (lstBP.Count() > 0)
                    {
                        entityDt.BusinessPartnerID = lstBP.FirstOrDefault().BusinessPartnerID;
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

                    entityDt.GLAccount = hdnCOARevenueSharingCtl.Value != null && hdnCOARevenueSharingCtl.Value != "" ? Convert.ToInt32(hdnCOARevenueSharingCtl.Value) : 0;

                    if (hdnTreasuryTypectl.Value == Constant.TreasuryType.PENERIMAAN)
                    {
                        entityDt.Position = "K";
                        entityDt.DebitAmount = 0;
                        entityDt.CreditAmount = paymentDt.TotalRevenueSharingAmount;
                    }
                    else
                    {
                        entityDt.Position = "D";
                        entityDt.DebitAmount = paymentDt.TotalRevenueSharingAmount;
                        entityDt.CreditAmount = 0;
                    }

                    int displayOrder = Convert.ToInt16(hdnDisplayOrderTemp.Value) + count + 1;
                    entityDt.DisplayOrder = Convert.ToInt16(displayOrder);

                    string remarks = lstSelectedRemarks[count];
                    entityDt.Remarks = string.Format("{0}|{1}|{2}|{3}",
                                                        summaryHd.RSSummaryNo,
                                                        summaryHd.ParamedicName,
                                                        paymentHd.VerificationDate.ToString(Constant.FormatString.DATE_FORMAT),
                                                        remarks);
                    entityDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;

                    entityDt.IsReferenceNoGeneratedBySystem = true;

                    entityDt.GLTransactionID = treasuryHd.GLTransactionID;
                    entityDt.CreatedBy = AppSession.UserLogin.UserID;
                    int glDtID = glTransactionDtDao.InsertReturnPrimaryKeyID(entityDt);

                    paymentDt.GLTransactionDtID = glDtID;
                    paymentDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    paymentDtDao.Update(paymentDt);

                    paymentHd.GLTransactionID = treasuryHd.GLTransactionID;
                    paymentHd.RSPaymentDate = treasuryHd.JournalDate;
                    paymentHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                    paymentHd.ApprovedBy = AppSession.UserLogin.UserID;
                    paymentHd.ApprovedDate = DateTime.Now;
                    paymentHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    paymentHdDao.Update(paymentHd);


                    lstEntity.Add(entityDt);
                }

                #endregion

                #region RevenueSharingFeeHd

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();

                decimal feeHdAmount = 0;
                string infoRevSharingFeeID = "";
                string filterFeeHd = string.Format("RSPaymentID = {0} AND IsPosting = 1 AND IsPayment = 1", entity.RSPaymentID);
                List<RevenueSharingFeeHd> lstRevSharingFeeHd = BusinessLayer.GetRevenueSharingFeeHdList(filterFeeHd, ctx);
                if (lstRevSharingFeeHd.Count > 0)
                {
                    foreach (RevenueSharingFeeHd feeHd in lstRevSharingFeeHd)
                    {
                        feeHdAmount += ((feeHd.PPH21Setor + feeHd.PPH21Ekstra) * -1);

                        if (infoRevSharingFeeID != "")
                        {
                            infoRevSharingFeeID += ", ";
                        }
                        infoRevSharingFeeID += feeHd.RevenueSharingFeeID.ToString();
                    }

                    GLTransactionDt entityDtFee = new GLTransactionDt();
                    entityDtFee.ReferenceNo = paymentHd.RSPaymentNo;
                    entityDtFee.HealthcareID = AppSession.UserLogin.HealthcareID;
                    if (hdnBusinessPartnerIDCtl.Value != "" && hdnBusinessPartnerIDCtl.Value != null)
                    {
                        entityDtFee.BusinessPartnerID = Convert.ToInt32(hdnBusinessPartnerIDCtl.Value);
                    }
                    entityDtFee.DepartmentID = hdnDepartmentIDCtl.Value;
                    if (hdnServiceUnitIDCtl.Value != "" && hdnServiceUnitIDCtl.Value != null)
                    {
                        entityDtFee.ServiceUnitID = Convert.ToInt32(hdnServiceUnitIDCtl.Value);
                    }
                    entityDtFee.GLAccount = hdnCOARevenueSharingCtl.Value != null && hdnCOARevenueSharingCtl.Value != "" ? Convert.ToInt32(hdnCOARevenueSharingCtl.Value) : 0;

                    if (hdnTreasuryTypectl.Value == Constant.TreasuryType.PENERIMAAN)
                    {
                        entityDtFee.Position = "K";
                        entityDtFee.DebitAmount = 0;
                        entityDtFee.CreditAmount = feeHdAmount;
                    }
                    else
                    {
                        entityDtFee.Position = "D";
                        entityDtFee.DebitAmount = feeHdAmount;
                        entityDtFee.CreditAmount = 0;
                    }

                    int displayOrderFee = Convert.ToInt16(hdnDisplayOrderTemp.Value) + count + 2;
                    entityDtFee.DisplayOrder = Convert.ToInt16(displayOrderFee);
                    entityDtFee.Remarks = string.Format("{0}|{1}|RevenueSharingFeeID IN ({2})",
                                                        paymentHd.RSPaymentNo,
                                                        paymentHd.VerificationDate.ToString(Constant.FormatString.DATE_FORMAT),
                                                        infoRevSharingFeeID);
                    entityDtFee.GCItemDetailStatus = Constant.TransactionStatus.OPEN;

                    entityDtFee.GLTransactionID = treasuryHd.GLTransactionID;
                    entityDtFee.CreatedBy = AppSession.UserLogin.UserID;
                    glTransactionDtDao.Insert(entityDtFee);
                }

                #endregion

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

        #region Get Constant
        protected string GetRevenuePaymentMethodTransfer()
        {
            return Constant.RevenuePaymentMethod.TRANSFER;
        }
        #endregion
    }
}