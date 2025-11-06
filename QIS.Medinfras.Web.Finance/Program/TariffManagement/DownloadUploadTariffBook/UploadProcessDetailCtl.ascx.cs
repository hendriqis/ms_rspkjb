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
    public partial class UploadProcessDetailCtl : BaseEntryPopupCtl
    {
        private DownloadUploadTariffBook DetailPage
        {
            get { return (DownloadUploadTariffBook)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            hdnBookIDCtl.Value = param;

            BindGridView();
        }

        #region Bind Grid
        private void BindGridView()
        {
            string filterExpression = string.Format("BookID = {0} ORDER BY BookID, ItemCode, ClassName", hdnBookIDCtl.Value);

            List<TariffBookDtTemp> lstEntity = BusinessLayer.GetTariffBookDtTempList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void cbpProcessDetailCtl_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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
            TariffBookHdDao bookHdDao = new TariffBookHdDao(ctx);
            TariffBookDtDao bookDtDao = new TariffBookDtDao(ctx);
            TariffBookDtTempDao bookDtTempDao = new TariffBookDtTempDao(ctx);
            TariffBookDtCostDao bookDtCost = new TariffBookDtCostDao(ctx);
            try
            {
                TariffBookHd bookHd = bookHdDao.Get(Convert.ToInt32(hdnBookIDCtl.Value));
                if (bookHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    
                    string filterDtTemp = string.Format("BookID = {0}", hdnBookIDCtl.Value);
                    List<TariffBookDtTemp> lstEntityDtTemp = BusinessLayer.GetTariffBookDtTempList(filterDtTemp, ctx);
                    foreach (TariffBookDtTemp dtTemp in lstEntityDtTemp)
                    {
                        string filterDt = string.Format("BookID = {0} AND ItemID = {1} AND ClassID = {2}", hdnBookIDCtl.Value, dtTemp.ItemID, dtTemp.ClassID);
                        List<TariffBookDt> lstEntityDt = BusinessLayer.GetTariffBookDtList(filterDt, ctx);
                        if (lstEntityDt.Count == 0)
                        {
                            TariffBookDt bookDt = new TariffBookDt();
                            bookDt.BookID = bookHd.BookID;
                            bookDt.ItemID = dtTemp.ItemID;
                            bookDt.ClassID = dtTemp.ClassID;
                            bookDt.MarginPercentage = dtTemp.MarginPercentage;
                            bookDt.SuggestedTariff = dtTemp.SuggestedTariff;
                            bookDt.BaseTariff = dtTemp.BaseTariff;
                            bookDt.ProposedTariff = dtTemp.ProposedTariff;
                            bookDt.ProposedTariffComp1 = dtTemp.ProposedTariffComp1;
                            bookDt.ProposedTariffComp2 = dtTemp.ProposedTariffComp2;
                            bookDt.ProposedTariffComp3 = dtTemp.ProposedTariffComp3;
                            bookDt.CreatedBy = AppSession.UserLogin.UserID;
                            bookDtDao.Insert(bookDt);

                            TariffBookDtCost dtCost = new TariffBookDtCost();
                            dtCost.BookID = bookHd.BookID;
                            dtCost.ItemID = dtTemp.ItemID;
                            dtCost.ClassID = dtTemp.ClassID;
                            dtCost.PreviousMaterial = 0;
                            dtCost.CurrentMaterial = 0;
                            dtCost.TotalMaterial =0;
                            dtCost.PreviousLabor =0;
                            dtCost.CurrentLabor = 0;
                            dtCost.TotalLabor = 0;
                            dtCost.PreviousOverhead = 0;
                            dtCost.CurrentOverhead = 0;
                            dtCost.TotalOverhead = 0;
                            dtCost.PreviousSubContract = 0;
                            dtCost.CurrentSubContract = 0;
                            dtCost.TotalSubContract = 0;
                            dtCost.PreviousBurden = 0;
                            dtCost.CurrentBurden = 0;
                            dtCost.TotalBurden = 0;
                            dtCost.LastUpdatedBy = AppSession.UserLogin.UserID;
                            dtCost.LastUpdatedDate = DateTime.Now;
                            bookDtCost.Insert(dtCost);

                            bookDtTempDao.Delete(dtTemp.BookID, dtTemp.ItemID, dtTemp.ClassID);
                        }
                    }


                    retval = bookHd.BookID.ToString();
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "GAGAL UPLOAD, status buku tariff sudah tidak open lagi.";
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