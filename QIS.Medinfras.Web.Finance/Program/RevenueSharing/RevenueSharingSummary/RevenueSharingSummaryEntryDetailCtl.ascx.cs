using System;
using System.Collections.Generic;
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
    public partial class RevenueSharingSummaryEntryDetailCtl : BaseEntryPopupCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;

        private RevenueSharingSummaryEntry DetailPage
        {
            get { return (RevenueSharingSummaryEntry)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            hdnRSSummaryIDCtl.Value = param;

            txtPeriodFrom.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPeriodTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            BindGridView();
        }

        #region Bind Grid
        private void BindGridView()
        {
            string filterExpression = string.Format("GCTransactionStatus = '{0}' AND ParamedicID = {1}", Constant.TransactionStatus.OPEN, AppSession.ParamedicID);
            filterExpression += string.Format(" AND RSTransactionID NOT IN (SELECT RSTransactionID FROM TransRevenueSharingSummaryDt WHERE IsDeleted = 0 AND RSSummaryID IN (SELECT RSSummaryID FROM TransRevenueSharingSummaryHd WHERE GCTransactionStatus != '{0}'))", Constant.TransactionStatus.VOID);
            
            if (hdnRSSummaryIDCtl.Value != "" && hdnRSSummaryIDCtl.Value != "0")
            {
                filterExpression += string.Format(" AND RSTransactionID NOT IN (SELECT RSTransactionID FROM TransRevenueSharingSummaryDt WHERE IsDeleted = 0 AND RSSummaryID IN (SELECT RSSummaryID FROM TransRevenueSharingSummaryHd WHERE RSSummaryID = '{0}'))", hdnRSSummaryIDCtl.Value);
            }

            filterExpression += string.Format(" AND ProcessedDate BETWEEN '{0}' AND '{1}'", Helper.GetDatePickerValue(txtPeriodFrom), Helper.GetDatePickerValue(txtPeriodTo));

            lstSelectedMember = hdnSelectedRSTransaction.Value.Split(',');

            List<vTransRevenueSharingHd> lstEntity = BusinessLayer.GetvTransRevenueSharingHdList(filterExpression);
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
        private void ControlToEntity(IDbContext ctx, List<TransRevenueSharingSummaryDt> lstEntityDt)
        {
            TransRevenueSharingHdDao entityHdDao = new TransRevenueSharingHdDao(ctx);

            string filterExpression = string.Format("RSTransactionID IN ({0})", hdnSelectedRSTransaction.Value.Substring(1));
            List<vTransRevenueSharingHd> lstViewHd = BusinessLayer.GetvTransRevenueSharingHdList(filterExpression, ctx);

            List<TransRevenueSharingHd> lstHd = BusinessLayer.GetTransRevenueSharingHdList(filterExpression, ctx);

            foreach (vTransRevenueSharingHd entityInsert in lstViewHd)
            {
                TransRevenueSharingHd entityHd = lstHd.FirstOrDefault(p => p.RSTransactionID == entityInsert.RSTransactionID);
                entityHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                entityHd.ApprovedBy = AppSession.UserLogin.UserID;
                entityHd.ApprovedDate = DateTime.Now;
                entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityHdDao.Update(entityHd);

                TransRevenueSharingSummaryDt entitySummaryDt = new TransRevenueSharingSummaryDt();
                entitySummaryDt.RSTransactionID = entityInsert.RSTransactionID;
                entitySummaryDt.RevenueSharingAmount = entityInsert.TotalRevenueSharingAmount;
                entitySummaryDt.CreatedBy = AppSession.UserLogin.UserID;
                lstEntityDt.Add(entitySummaryDt);
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TransRevenueSharingSummaryHdDao entitySummaryHdDao = new TransRevenueSharingSummaryHdDao(ctx);
            TransRevenueSharingSummaryDtDao entitySummaryDtDao = new TransRevenueSharingSummaryDtDao(ctx);
            TransRevenueSharingHdDao entityHdDao = new TransRevenueSharingHdDao(ctx);

            int oRSSummaryID = 0;
            try
            {
                string errorMessage = "";

                List<GetPRSCheckCountDataDouble> lstCheck = BusinessLayer.GetPRSCheckCountDataDoubleList(hdnSelectedRSTransaction.Value.Substring(1));
                if (lstCheck.Count() == 0)
                {
                    DetailPage.SaveTransRevenueSharingSummaryHd(ctx, ref oRSSummaryID, ref errorMessage);
                    if (String.IsNullOrEmpty(errorMessage))
                    {
                        if (entitySummaryHdDao.Get(oRSSummaryID).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                        {
                            List<TransRevenueSharingSummaryDt> lstEntitySummaryDt = new List<TransRevenueSharingSummaryDt>();

                            ControlToEntity(ctx, lstEntitySummaryDt);
                            foreach (TransRevenueSharingSummaryDt entitySummaryDt in lstEntitySummaryDt)
                            {
                                entitySummaryDt.RSSummaryID = oRSSummaryID;
                                entitySummaryDt.CreatedBy = AppSession.UserLogin.UserID;
                                entitySummaryDtDao.Insert(entitySummaryDt);

                                TransRevenueSharingHd entityHd = entityHdDao.Get(entitySummaryDt.RSTransactionID);
                                entityHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                                entityHd.ApprovedBy = AppSession.UserLogin.UserID;
                                entityHd.ApprovedDate = DateTime.Now;
                                entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityHdDao.Update(entityHd);
                            }

                            retval = oRSSummaryID.ToString();
                            ctx.CommitTransaction();
                        }
                        else
                        {
                            result = false;
                            errMessage = string.Format("Transaksi rekap jasa medis di nomor {0} tidak dapat diubah. Harap refresh halaman ini.", entitySummaryHdDao.Get(oRSSummaryID).RSSummaryNo);
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
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
                else
                {
                    string doublePRS = "";
                    foreach (GetPRSCheckCountDataDouble dt in lstCheck)
                    {
                        if (doublePRS != "")
                        {
                            doublePRS += ", ";
                        }
                        doublePRS += dt.RevenueSharingNo;
                    }

                    result = false;
                    errMessage = string.Format("Tidak dapat proses Approved untuk [{0}] karena masih memiliki detail PRS yang charges detailnya diproses PRS >1x.", doublePRS);
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