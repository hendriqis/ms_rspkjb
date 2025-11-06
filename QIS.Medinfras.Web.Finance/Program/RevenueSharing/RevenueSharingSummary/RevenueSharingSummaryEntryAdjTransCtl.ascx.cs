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
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class RevenueSharingSummaryEntryAdjTransCtl : BaseEntryPopupCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;

        private RevenueSharingSummaryAdjustmentEntry DetailPage
        {
            get { return (RevenueSharingSummaryAdjustmentEntry)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            string[] paramCtl = param.Split('|');
            hdnRSSummaryIDCtl.Value = paramCtl[0];
            hdnRSSummaryMaxAmountCtl.Value = paramCtl[1];

            string filterSetVar = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN_REVENUE_SHARING_ADJUSTMENT_USING_BRUTO);
            List<SettingParameterDt> lstServarDt = BusinessLayer.GetSettingParameterDtList(filterSetVar);
            if (lstServarDt.Count() > 0)
            {
                hdnIsUsedAdjustmentAmountBRUTO.Value = lstServarDt.FirstOrDefault().ParameterValue;
            }

            txtPeriodFrom.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPeriodTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            BindGridView();
        }

        #region Bind Grid
        private void BindGridView()
        {
            string filterExpression = string.Format("RSSummaryAdjustmentID IS NULL AND GCTransactionStatus IN ('{0}','{1}') AND ParamedicID = {2} AND IsDeleted = 0", Constant.TransactionStatus.APPROVED, Constant.TransactionStatus.PROCESSED, AppSession.ParamedicID);

            filterExpression += string.Format(" AND RSAdjustmentDate BETWEEN '{0}' AND '{1}'", Helper.GetDatePickerValue(txtPeriodFrom), Helper.GetDatePickerValue(txtPeriodTo));

            List<vTransRevenueSharingAdjustmentDt> lstEntity = BusinessLayer.GetvTransRevenueSharingAdjustmentDtList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
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
            TransRevenueSharingSummaryHdDao rsSummaryHdDao = new TransRevenueSharingSummaryHdDao(ctx);
            TransRevenueSharingSummaryAdjDao rsSummaryAdjDao = new TransRevenueSharingSummaryAdjDao(ctx);
            TransRevenueSharingAdjustmentHdDao entityAdjHdDao = new TransRevenueSharingAdjustmentHdDao(ctx);
            TransRevenueSharingAdjustmentDtDao entityAdjDtDao = new TransRevenueSharingAdjustmentDtDao(ctx);

            try
            {
                int RSSummaryID = Convert.ToInt32(hdnRSSummaryIDCtl.Value);
                TransRevenueSharingSummaryHd rsSummaryHd = rsSummaryHdDao.Get(RSSummaryID);
                if (rsSummaryHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    List<TransRevenueSharingAdjustmentDt> entityDtList = BusinessLayer.GetTransRevenueSharingAdjustmentDtList(string.Format("ID IN ({0}) AND IsDeleted = 0", hdnSelectedDtID.Value.Substring(1)), ctx);
                    foreach (TransRevenueSharingAdjustmentDt entityDt in entityDtList)
                    {
                        TransRevenueSharingAdjustmentHd entityHd = entityAdjHdDao.Get(entityDt.RSAdjustmentID);
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.APPROVED || entityHd.GCTransactionStatus == Constant.TransactionStatus.PROCESSED)
                        {
                            TransRevenueSharingSummaryAdj adj = new TransRevenueSharingSummaryAdj();
                            adj.RSSummaryID = RSSummaryID;
                            adj.GCRSAdjustmentGroup = entityDt.GCRSAdjustmentGroup;
                            adj.GCRSAdjustmentType = entityDt.GCRSAdjustmentType;
                            adj.Remarks = entityDt.Remarks;
                            adj.RevenueSharingID = entityDt.RevenueSharingID;
                            adj.RegistrationNo = entityDt.RegistrationNo;
                            adj.RegistrationDate = entityDt.RegistrationDate;
                            adj.DischargeDate = entityDt.DischargeDate;
                            adj.ReceiptNo = entityDt.ReceiptNo;
                            adj.InvoiceNo = entityDt.InvoiceNo;
                            adj.ReferenceNo = entityDt.ReferenceNo;
                            adj.BusinessPartnerName = entityDt.BusinessPartnerName;
                            adj.MedicalNo = entityDt.MedicalNo;
                            adj.PatientName = entityDt.PatientName;
                            adj.TransactionNo = entityDt.TransactionNo;
                            adj.TransactionDate = entityDt.TransactionDate;
                            adj.ItemName1 = entityDt.ItemName1;
                            adj.ChargedQty = entityDt.ChargedQty;
                            adj.AdjustmentAmountBRUTO = entityDt.AdjustmentAmountBRUTO;
                            if (hdnIsUsedAdjustmentAmountBRUTO.Value == "1")
                            {
                                adj.AdjustmentAmount = entityDt.AdjustmentAmountBRUTO;
                            }
                            else
                            {
                                adj.AdjustmentAmount = entityDt.AdjustmentAmount;
                            }
                            adj.IsTaxed = entityDt.IsTaxed;
                            adj.CreatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            int rsSummaryAdjID = rsSummaryAdjDao.InsertReturnPrimaryKeyID(adj);

                            entityDt.RSSummaryAdjustmentID = rsSummaryAdjID;
                            entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityAdjDtDao.Update(entityDt);

                            List<TransRevenueSharingAdjustmentDt> entityDtListNotCopied = BusinessLayer.GetTransRevenueSharingAdjustmentDtList(string.Format("RSAdjustmentID IN ({0}) AND IsDeleted = 0 AND RSSummaryAdjustmentID IS NULL", entityHd.RSAdjustmentID), ctx);
                            if (entityDtListNotCopied.Count == 0)
                            {
                                entityHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                                entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                entityAdjHdDao.Update(entityHd);
                            }
                            else
                            {
                                entityHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                                entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                entityAdjHdDao.Update(entityHd);
                            }
                        }
                        else
                        {
                            result = false;
                            errMessage = string.Format("Transaksi penyesuaian jasa medis di nomor {0} tidak dapat diubah karena sudah diproses.", entityHd.RSAdjustmentNo);
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                        }
                    }
                }
                else
                {
                    result = false;
                    errMessage = string.Format("Transaksi rekap jasa medis di nomor {0} tidak dapat diubah karena sudah diproses.", rsSummaryHd.RSSummaryNo);
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                }

                if (result)
                {
                    ctx.CommitTransaction();
                }
                else
                {
                    ctx.RollBackTransaction();
                }

                retval = RSSummaryID.ToString();
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