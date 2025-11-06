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
    public partial class ARReceiptQuickPicksCtl : BaseEntryPopupCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;

        private ARReceiptEntry DetailPage
        {
            get { return (ARReceiptEntry)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            hdnARReceiptIDCtl.Value = param;

            txtPeriodFrom.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPeriodTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            BindGridView();
        }

        #region Bind Grid
        private void BindGridView()
        {
            string filterExpression = string.Format(
                                            "GCTransactionStatus = '{0}' AND TotalPaymentAmount = 0 AND ARInvoiceID NOT IN (SELECT ARInvoiceID FROM ARReceiptDt WHERE IsDeleted = 0 AND ARReceiptID IN (SELECT ARReceiptID FROM ARReceiptHd WHERE IsDeleted = 0))",
                                            Constant.TransactionStatus.APPROVED );

            filterExpression += string.Format(" AND ARInvoiceDate BETWEEN '{0}' AND '{1}'", Helper.GetDatePickerValue(txtPeriodFrom), Helper.GetDatePickerValue(txtPeriodTo));

            if (hdnBusinessPartnerID.Value != "" && hdnBusinessPartnerID.Value != "0")
            {
                filterExpression += string.Format(" AND BusinessPartnerID = {0}", hdnBusinessPartnerID.Value);
            }

            lstSelectedMember = hdnSelectedARInvoiceID.Value.Split(',');

            List<vARInvoiceHd> lstEntity = BusinessLayer.GetvARInvoiceHdList(filterExpression);
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
        private void ControlToEntity(IDbContext ctx, List<ARReceiptDt> lstEntityDt)
        {
            string filterExpression = string.Format("ARInvoiceID IN ({0})", hdnSelectedARInvoiceID.Value.Substring(1));
            List<ARInvoiceHd> lstInvoiceCtx = BusinessLayer.GetARInvoiceHdList(filterExpression, ctx);

            int count = 0;
            List<String> lstSelectedARInvoiceID = hdnSelectedARInvoiceID.Value.Split(',').ToList();
            lstSelectedARInvoiceID.RemoveAt(0);

            foreach (ARInvoiceHd invoice in lstInvoiceCtx)
            {
                ARReceiptDt entityDt = new ARReceiptDt();
                entityDt.ARInvoiceID = invoice.ARInvoiceID;
                entityDt.CreatedBy = AppSession.UserLogin.UserID;
                lstEntityDt.Add(entityDt);

                count++;
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ARReceiptHdDao entityHdDao = new ARReceiptHdDao(ctx);
            ARReceiptDtDao entityDtDao = new ARReceiptDtDao(ctx);

            int arReceiptID = 0;
            string arReceiptNo = "";
            try
            {
                DetailPage.SaveARReceiptHd(ctx, ref arReceiptID, ref arReceiptNo);
                if (!entityHdDao.Get(arReceiptID).IsDeleted)
                {
                    List<ARReceiptDt> lstEntityDt = new List<ARReceiptDt>();
                    ControlToEntity(ctx, lstEntityDt);
                    foreach (ARReceiptDt entityDt in lstEntityDt)
                    {
                        entityDt.ARReceiptID = arReceiptID;
                        entityDt.CreatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Insert(entityDt);
                    }

                    ARReceiptHd entityHd = entityHdDao.Get(arReceiptID);
                    entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entityHd);

                    retval = entityHd.ARReceiptID.ToString();
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = string.Format("Proses kwitansi di nomor " + arReceiptNo + " tidak dapat dilanjutkan. Harap refresh halaman ini.");
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