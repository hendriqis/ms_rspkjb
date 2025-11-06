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
    public partial class RevenueSharingSummaryEntryAdjTransCtlX : BaseEntryPopupCtl
    {
        private RevenueSharingSummaryAdjustmentEntry DetailPage
        {
            get { return (RevenueSharingSummaryAdjustmentEntry)Page; }
        }

        protected string GetFilterExpression()
        {
            return string.Format("ParamedicID = {0} AND GCTransactionStatus = '{1}' AND RSSummaryID IS NULL", AppSession.ParamedicID, Constant.TransactionStatus.APPROVED);
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

            TransRevenueSharingSummaryHd summaryHd = BusinessLayer.GetTransRevenueSharingSummaryHd(Convert.ToInt32(hdnRSSummaryIDCtl.Value));
            txtRSSummaryNoCtl.Text = summaryHd.RSSummaryNo;
            txtRSSummaryAmountCtl.Text = summaryHd.TotalRevenueSharingAmount.ToString(Constant.FormatString.NUMERIC_2);
            txtMaxTotalAdjustmentAmountCtl.Text = Convert.ToDecimal(hdnRSSummaryMaxAmountCtl.Value).ToString(Constant.FormatString.NUMERIC_2);

            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.REVENUE_SHARING_ADJUSTMENT_GROUP, Constant.StandardCode.REVENUE_SHARING_ADJUSTMENT_TYPE));
            Methods.SetRadioButtonListField(rblAdjustmentCtl, lstSc.Where(p => p.ParentID == Constant.StandardCode.REVENUE_SHARING_ADJUSTMENT_GROUP).ToList(), "StandardCodeName", "StandardCodeID");
            hdnAdjustmentGroup.Value = rblAdjustmentCtl.SelectedValue = Constant.RevenueSharingAdjustmentGroup.PENAMBAHAN;
        }

        protected void cbpViewAdjCtl_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridDetail();
        }

        private void BindGridDetail()
        {
            string filterExpression = "";
            int RSAdjustmentID = hdnRSAdjustmentID.Value != "" ? Convert.ToInt32(hdnRSAdjustmentID.Value) : 0;

            filterExpression = string.Format("IsDeleted = 0 AND ParamedicID = {0} AND RSAdjustmentID = {1} AND GCRSAdjustmentGroup = '{2}'", AppSession.ParamedicID, RSAdjustmentID, hdnAdjustmentGroup.Value);

            List<vTransRevenueSharingAdjustmentDt> lstEntity = BusinessLayer.GetvTransRevenueSharingAdjustmentDtList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

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
                    decimal maxAmount = Convert.ToDecimal(hdnRSSummaryMaxAmountCtl.Value);
                    if (hdnRSAdjustmentID.Value != "" && hdnRSAdjustmentID.Value != "0")
                    {
                        TransRevenueSharingAdjustmentHd entityHd = entityAdjHdDao.Get(Convert.ToInt32(hdnRSAdjustmentID.Value));
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.APPROVED)
                        {
                            List<TransRevenueSharingAdjustmentDt> entityDtList = BusinessLayer.GetTransRevenueSharingAdjustmentDtList(string.Format("RSAdjustmentID = {0} AND IsDeleted = 0", entityHd.RSAdjustmentID), ctx);
                            foreach (TransRevenueSharingAdjustmentDt entityDt in entityDtList)
                            {
                                TransRevenueSharingSummaryAdj adj = new TransRevenueSharingSummaryAdj();
                                adj.RSSummaryID = rsSummaryHd.RSSummaryID;
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
                                entityAdjDtDao.Update(entityDt);
                            }

                            entityHd.RSSummaryID = rsSummaryHd.RSSummaryID;
                            entityHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                            entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityAdjHdDao.Update(entityHd);

                            retval = RSSummaryID.ToString();
                            ctx.CommitTransaction();
                        }
                        else
                        {
                            result = false;
                            errMessage = "Transaksi tidak dapat disalin. Harap refresh halaman ini.";
                            ctx.RollBackTransaction();
                        }
                    }
                    else
                    {
                        result = false;
                        errMessage = "Transaksi tidak ditemukan. Harap refresh halaman ini.";
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = string.Format("Transaksi rekap jasa medis di nomor {0} tidak dapat diubah karena sudah diproses.", rsSummaryHd.RSSummaryNo);
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