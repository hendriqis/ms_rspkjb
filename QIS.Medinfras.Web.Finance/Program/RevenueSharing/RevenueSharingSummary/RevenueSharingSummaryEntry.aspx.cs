using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class RevenueSharingSummaryEntry : BasePageTrx
    {
        protected int PageCount = 1;
        protected decimal TotalBrutoAmount = 0;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.REVENUE_SHARING_SUMMARY_ENTRY;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected string OnGetRevenueSharingFilterExpression()
        {
            return string.Format("ParamedicID = {0}", AppSession.ParamedicID);
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            hdnParamedicID.Value = AppSession.ParamedicID.ToString();

            BindGridView(1, true, ref PageCount, false, ref TotalBrutoAmount);
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnIsEditableCustom, new ControlEntrySetting(false, false, false, "1"));
            SetControlEntrySetting(hdnRSSummaryID, new ControlEntrySetting(false, false, false, ""));
            SetControlEntrySetting(txtRSSummaryNo, new ControlEntrySetting(false, false, false, ""));
            SetControlEntrySetting(txtRSSummaryDate, new ControlEntrySetting(true, false, true, Constant.DefaultValueEntry.DATE_NOW));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false, ""));
            SetControlEntrySetting(txtBrutoAmount, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtSummaryAmount, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtSummaryAdjustmentAmount, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtSummaryEndAmount, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(hdnGCTransactionStatus, new ControlEntrySetting(false, false, false, Constant.TransactionStatus.OPEN));
        }

        public override void OnAddRecord()
        {
            hdnPageCount.Value = "0";
        }

        #region Load Entity
        protected string GetFilterExpression()
        {
            return string.Format("ParamedicID = {0}", hdnParamedicID.Value);
        }

        public override int OnGetRowCount()
        {
            string filterExpression = GetFilterExpression();
            return BusinessLayer.GetvTransRevenueSharingSummaryHdRowCount(filterExpression);
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            vTransRevenueSharingSummaryHd entity = BusinessLayer.GetvTransRevenueSharingSummaryHd(filterExpression, PageIndex, "RSSummaryID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            PageIndex = BusinessLayer.GetvTransRevenueSharingSummaryHdRowIndex(filterExpression, keyValue, "RSSummaryID DESC");
            vTransRevenueSharingSummaryHd entity = BusinessLayer.GetvTransRevenueSharingSummaryHd(filterExpression, PageIndex, "RSSummaryID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        private void EntityToControl(vTransRevenueSharingSummaryHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                hdnIsEditableCustom.Value = "0";
                isShowWatermark = true;
                watermarkText = entity.TransactionStatus;

                SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, false, false));
            }
            else
            {
                hdnIsEditableCustom.Value = "1";
                isShowWatermark = false;

                SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
            }

            hdnGCTransactionStatus.Value = entity.GCTransactionStatus;

            hdnRSSummaryID.Value = entity.RSSummaryID.ToString();
            txtRSSummaryNo.Text = entity.RSSummaryNo;
            txtRSSummaryDate.Text = entity.RSSummaryDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtRemarks.Text = entity.Remarks;

            hdnPageCount.Value = PageCount.ToString();
            Decimal TotalBrutoAmount = 0;
            BindGridView(1, true, ref PageCount, true, ref TotalBrutoAmount);

            txtBrutoAmount.Text = TotalBrutoAmount.ToString(Constant.FormatString.NUMERIC_2);
            txtSummaryAmount.Text = (entity.TotalRevenueSharingAmount - entity.TotalAdjustmentAmount).ToString(Constant.FormatString.NUMERIC_2);
            txtSummaryAdjustmentAmount.Text = entity.TotalAdjustmentAmount.ToString(Constant.FormatString.NUMERIC_2);
            txtSummaryEndAmount.Text = entity.TotalRevenueSharingAmount.ToString(Constant.FormatString.NUMERIC_2);

            divCreatedBy.InnerHtml = entity.CreatedByName;
            divCreatedDate.InnerHtml = entity.CreatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);

            if (entity.ApprovedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
            {
                trApprovedBy.Style.Add("display", "none");
                trApprovedDate.Style.Add("display", "none");

                divApprovedBy.InnerHtml = "";
                divApprovedDate.InnerHtml = "";
            }
            else
            {
                trApprovedBy.Style.Remove("display");
                trApprovedDate.Style.Remove("display");

                divApprovedBy.InnerHtml = entity.ApprovedByName;
                divApprovedDate.InnerHtml = entity.ApprovedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }

            if (entity.LastUpdatedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
            {
                divLastUpdatedBy.InnerHtml = "";
                divLastUpdatedDate.InnerHtml = "";
            }
            else
            {
                divLastUpdatedBy.InnerHtml = entity.LastUpdatedByName;
                divLastUpdatedDate.InnerHtml = entity.LastUpdatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }

        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount, bool isCountTotalAmount, ref decimal TotalBrutoAmount)
        {
            string filterExpression = "1 = 0";
            if (hdnRSSummaryID.Value != "" && hdnRSSummaryID.Value != "0")
            {
                filterExpression = string.Format("RSSummaryID = {0} AND IsDeleted = 0", hdnRSSummaryID.Value);
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvTransRevenueSharingSummaryDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vTransRevenueSharingSummaryDt> lstEntity = BusinessLayer.GetvTransRevenueSharingSummaryDtList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();

            TotalBrutoAmount = lstEntity.Sum(a => a.BrutoTransactionAmount);
        }
        #endregion

        #region Save Edit Header
        public void SaveTransRevenueSharingSummaryHd(IDbContext ctx, ref int RSSummaryID, ref string errorMessage)
        {
            TransRevenueSharingSummaryHdDao entitySummaryHdDao = new TransRevenueSharingSummaryHdDao(ctx);

            if (hdnRSSummaryID.Value == "")
            {
                #region Add

                TransRevenueSharingSummaryHd entitySummaryHd = new TransRevenueSharingSummaryHd();
                entitySummaryHd.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
                entitySummaryHd.RSSummaryDate = Helper.GetDatePickerValue(txtRSSummaryDate.Text);
                entitySummaryHd.RSSummaryNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.TRANS_REVENUE_SHARING_SUMMARY_ENTRY, entitySummaryHd.RSSummaryDate, ctx);
                entitySummaryHd.Remarks = txtRemarks.Text;
                entitySummaryHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                entitySummaryHd.CreatedBy = AppSession.UserLogin.UserID;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                RSSummaryID = entitySummaryHdDao.InsertReturnPrimaryKeyID(entitySummaryHd);

                #endregion
            }
            else
            {
                #region Update

                TransRevenueSharingSummaryHd entitySummaryHd = BusinessLayer.GetTransRevenueSharingSummaryHd(Convert.ToInt32(hdnRSSummaryID.Value));
                entitySummaryHd.Remarks = txtRemarks.Text;
                entitySummaryHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                entitySummaryHdDao.Update(entitySummaryHd);
                RSSummaryID = Convert.ToInt32(hdnRSSummaryID.Value);

                #endregion
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                int RSSummaryID = 0;
                string errorMessage = "";
                SaveTransRevenueSharingSummaryHd(ctx, ref RSSummaryID, ref errorMessage);
                if (String.IsNullOrEmpty(errorMessage))
                {
                    retval = RSSummaryID.ToString();
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = errorMessage;
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

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            try
            {
                TransRevenueSharingSummaryHd entitySummaryHd = BusinessLayer.GetTransRevenueSharingSummaryHd(Convert.ToInt32(hdnRSSummaryID.Value));
                if (entitySummaryHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entitySummaryHd.Remarks = txtRemarks.Text;
                    entitySummaryHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateTransRevenueSharingSummaryHd(entitySummaryHd);
                    return true;
                }
                else
                {
                    errMessage = string.Format("Transaksi rekap jasa medis di nomor {0} tidak dapat diubah. Harap refresh halaman ini.", entitySummaryHd.RSSummaryNo);
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    return false;
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        protected override bool OnVoidRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TransRevenueSharingHdDao entityHdDao = new TransRevenueSharingHdDao(ctx);
            TransRevenueSharingSummaryHdDao entitySummaryHdDao = new TransRevenueSharingSummaryHdDao(ctx);
            TransRevenueSharingSummaryAdjDao entityAdjDao = new TransRevenueSharingSummaryAdjDao(ctx);
            TransRevenueSharingAdjustmentHdDao entityAdjHdDao = new TransRevenueSharingAdjustmentHdDao(ctx);
            TransRevenueSharingAdjustmentDtDao entityAdjDtDao = new TransRevenueSharingAdjustmentDtDao(ctx);
            ParamedicTaxBalanceDao taxBalanceDao = new ParamedicTaxBalanceDao(ctx);
            ARReceivingHdDao receivingHdDao = new ARReceivingHdDao(ctx);
            ARReceivingDtDao receivingDtDao = new ARReceivingDtDao(ctx);
            ARInvoiceReceivingDao invReceivingDao = new ARInvoiceReceivingDao(ctx);
            ARInvoiceReceivingDtDao invReceivingDtDao = new ARInvoiceReceivingDtDao(ctx);
            ARInvoiceHdDao eInvoiceHd = new ARInvoiceHdDao(ctx);
            ARInvoiceDtDao eInvoiceDt = new ARInvoiceDtDao(ctx);

            try
            {
                TransRevenueSharingSummaryHd entitySummaryHd = entitySummaryHdDao.Get(Convert.ToInt32(hdnRSSummaryID.Value));
                if (entitySummaryHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entitySummaryHd.Remarks = txtRemarks.Text;
                    entitySummaryHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                    entitySummaryHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entitySummaryHdDao.Update(entitySummaryHd);

                    string filterDt = string.Format("RSSummaryID = {0} AND IsDeleted = 0", entitySummaryHd.RSSummaryID);
                    List<vTransRevenueSharingSummaryDt> lstDt = BusinessLayer.GetvTransRevenueSharingSummaryDtList(filterDt, ctx);
                    foreach (vTransRevenueSharingSummaryDt entitySummaryDt in lstDt)
                    {
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();

                        TransRevenueSharingHd entityHd = entityHdDao.Get(entitySummaryDt.RSTransactionID);
                        entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        entityHd.ApprovedBy = null;
                        entityHd.ApprovedDate = Convert.ToDateTime(Constant.ConstantDate.DEFAULT_NULL);
                        entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityHdDao.Update(entityHd);
                    }
                    
                    string filterTaxBalance = string.Format("RSSummaryID = {0} AND ParamedicID = {1}", entitySummaryHd.RSSummaryID, entitySummaryHd.ParamedicID);
                    List<ParamedicTaxBalance> taxBalanceList = BusinessLayer.GetParamedicTaxBalanceList(filterTaxBalance, ctx);
                    foreach (ParamedicTaxBalance taxBalance in taxBalanceList)
                    {
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        taxBalanceDao.Delete(taxBalance.ParamedicID, taxBalance.PeriodNo, taxBalance.TaxPercent);
                    }

                    //string filterAdjHd = string.Format("RSSummaryID = {0} AND GCTransactionStatus != '{1}'", entitySummaryHd.RSSummaryID, Constant.TransactionStatus.VOID);
                    //List<TransRevenueSharingAdjustmentHd> adjHdLst = BusinessLayer.GetTransRevenueSharingAdjustmentHdList(filterAdjHd, ctx);
                    //foreach (TransRevenueSharingAdjustmentHd adjHd in adjHdLst)
                    //{
                    //    adjHd.RSSummaryID = null;
                    //    adjHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                    //    adjHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    //    ctx.CommandType = CommandType.Text;
                    //    ctx.Command.Parameters.Clear();
                    //    entityAdjHdDao.Update(adjHd);

                    //    string filterAdjDt = string.Format("RSAdjustmentID = {0} AND IsDeleted = 0", adjHd.RSAdjustmentID);
                    //    List<TransRevenueSharingAdjustmentDt> adjDtLst = BusinessLayer.GetTransRevenueSharingAdjustmentDtList(filterAdjDt, ctx);
                    //    foreach (TransRevenueSharingAdjustmentDt adjDt in adjDtLst)
                    //    {
                    //        adjDt.RSSummaryAdjustmentID = null;
                    //        adjDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    //        ctx.CommandType = CommandType.Text;
                    //        ctx.Command.Parameters.Clear();
                    //        entityAdjDtDao.Update(adjDt);
                    //    }
                    //}

                    string filterAdj = string.Format("RSSummaryID = {0} AND IsDeleted = 0", entitySummaryHd.RSSummaryID);
                    List<TransRevenueSharingSummaryAdj> lstAdj = BusinessLayer.GetTransRevenueSharingSummaryAdjList(filterAdj, ctx);
                    foreach (TransRevenueSharingSummaryAdj adj in lstAdj)
                    {
                        string filterAdjDt = string.Format("RSSummaryAdjustmentID = {0} AND IsDeleted = 0", adj.ID);
                        List<TransRevenueSharingAdjustmentDt> adjDtList = BusinessLayer.GetTransRevenueSharingAdjustmentDtList(filterAdjDt, ctx);
                        foreach (TransRevenueSharingAdjustmentDt adjDt in adjDtList)
                        {
                            adjDt.RSSummaryAdjustmentID = null;
                            adjDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityAdjDtDao.Update(adjDt);

                            string filterAdjDtCek = string.Format("RSAdjustmentID = {0} AND IsDeleted = 0 AND RSSummaryAdjustmentID IS NOT NULL", adjDt.RSAdjustmentID);
                            List<TransRevenueSharingAdjustmentDt> adjDtLstCek = BusinessLayer.GetTransRevenueSharingAdjustmentDtList(filterAdjDtCek, ctx);
                            if (adjDtLstCek.Count() == 0)
                            {
                                TransRevenueSharingAdjustmentHd adjHd = entityAdjHdDao.Get(adjDt.RSAdjustmentID);
                                adjHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                                adjHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                entityAdjHdDao.Update(adjHd);
                            }
                            else if (adjDtLstCek.Count() > 0)
                            {
                                TransRevenueSharingAdjustmentHd adjHd = entityAdjHdDao.Get(adjDt.RSAdjustmentID);
                                adjHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                                adjHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                entityAdjHdDao.Update(adjHd);
                            }
                        }

                        adj.IsDeleted = true;
                        adj.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityAdjDao.Update(adj);

                        if (adj.GCRSAdjustmentType == Constant.RevenueSharingAdjustmentType.POTONGAN_PIUTANG)
                        {
                            string filterARHD = string.Format("RSSummaryAdjustmentID = '{0}' AND GCTransactionStatus = '{1}'", adj.ID, Constant.TransactionStatus.OPEN);
                            List<ARReceivingHd> lstRcvHD = BusinessLayer.GetARReceivingHdList(filterARHD, ctx);
                            if (lstRcvHD.Count() > 0)
                            {
                                ARReceivingHd rcvHD = lstRcvHD.FirstOrDefault();
                                rcvHD.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                rcvHD.GCVoidReason = Constant.DeleteReason.OTHER;
                                rcvHD.VoidReason = "Delete from Revenue Sharing Summary";
                                rcvHD.VoidBy = AppSession.UserLogin.UserID;
                                rcvHD.VoidDate = DateTime.Now;
                                rcvHD.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                receivingHdDao.Update(rcvHD);

                                string filterInvRcv = string.Format("ARReceivingID = '{0}' AND IsDeleted = 0", rcvHD.ARReceivingID);
                                List<ARInvoiceReceiving> lstInvRcv = BusinessLayer.GetARInvoiceReceivingList(filterInvRcv, ctx);
                                foreach (ARInvoiceReceiving invRcv in lstInvRcv)
                                {
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();

                                    invRcv.IsDeleted = true;
                                    invRcv.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    invReceivingDao.Update(invRcv);

                                    string filterARInvRcvDt = string.Format("ARInvoiceReceivingID = {0} AND IsDeleted = 0", invRcv.ID);
                                    List<ARInvoiceReceivingDt> lstARInvRcvDt = BusinessLayer.GetARInvoiceReceivingDtList(filterARInvRcvDt, ctx);
                                    foreach (ARInvoiceReceivingDt invrcvDt in lstARInvRcvDt)
                                    {
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();

                                        invrcvDt.IsDeleted = true;
                                        invrcvDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        invReceivingDtDao.Update(invrcvDt);
                                    }

                                    ARInvoiceHd invHd = eInvoiceHd.Get(invRcv.ARInvoiceID);
                                    invHd.TotalPaymentAmount -= rcvHD.TotalReceivingAmount;
                                    if (invHd.TotalPaymentAmount == 0)
                                    {
                                        invHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;

                                        decimal rcvAll = rcvHD.TotalReceivingAmount;
                                        decimal rcvNow = 0;

                                        string filterInvDt = string.Format("ARInvoiceID = {0} AND ISNULL(GCTransactionDetailStatus,'') != '{1}'", invRcv.ARInvoiceID, Constant.TransactionStatus.VOID);
                                        List<ARInvoiceDt> lstInvoiceDt = BusinessLayer.GetARInvoiceDtList(filterInvDt, ctx);
                                        foreach (ARInvoiceDt oInvoiceDt in lstInvoiceDt)
                                        {
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();

                                            if (oInvoiceDt.ClaimedAmount <= rcvAll)
                                            {
                                                rcvNow = oInvoiceDt.ClaimedAmount;
                                            }
                                            else
                                            {
                                                rcvNow = rcvAll;
                                            }

                                            if (oInvoiceDt.PaymentAmount >= 0)
                                            {
                                                oInvoiceDt.PaymentAmount -= rcvNow;
                                            }

                                            oInvoiceDt.GCTransactionDetailStatus = Constant.TransactionStatus.APPROVED;
                                            oInvoiceDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            eInvoiceDt.Update(oInvoiceDt);
                                        }
                                    }
                                    else
                                    {
                                        invHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                                    }
                                    invHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    eInvoiceHd.Update(invHd);

                                }
                            }
                        }
                    }

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = string.Format("Transaksi rekap jasa medis di nomor {0} tidak dapat diubah. Harap refresh halaman ini.", entitySummaryHd.RSSummaryNo);
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

        #region callBack Trigger
        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            decimal TotalBrutoAmount = 0;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount, false, ref TotalBrutoAmount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridView(1, true, ref pageCount, true, ref TotalBrutoAmount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        #region Process Detail
        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            int RSSummaryID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "delete")
            {
                RSSummaryID = Convert.ToInt32(hdnRSSummaryID.Value);
                if (OnDeleteEntityDt(ref errMessage, RSSummaryID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRSSummaryID"] = RSSummaryID.ToString();
        }

        private bool OnDeleteEntityDt(ref string errMessage, int ID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TransRevenueSharingSummaryDtDao entitySummaryDtDao = new TransRevenueSharingSummaryDtDao(ctx);
            TransRevenueSharingHdDao entityHdDao = new TransRevenueSharingHdDao(ctx);

            try
            {
                TransRevenueSharingSummaryDt entitySummaryDt = entitySummaryDtDao.Get(Convert.ToInt32(hdnRSSummaryDtID.Value));
                entitySummaryDt.IsDeleted = true;
                entitySummaryDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                entitySummaryDt.LastUpdatedDate = DateTime.Now;
                entitySummaryDtDao.Update(entitySummaryDt);

                TransRevenueSharingHd entityHd = entityHdDao.Get(entitySummaryDt.RSTransactionID);
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                entityHd.ApprovedBy = null;
                entityHd.ApprovedDate = Convert.ToDateTime(Constant.ConstantDate.DEFAULT_NULL);
                entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityHdDao.Update(entityHd);

                ctx.CommitTransaction();
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