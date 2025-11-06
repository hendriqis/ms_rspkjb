using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class TreasuryTransactionChangeBusinessPartnerCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;

        public override void InitializeDataControl(string param)
        {
            hdnGLTransactionID.Value = param;

            GLTransactionHd entity = BusinessLayer.GetGLTransactionHd(Convert.ToInt32(hdnGLTransactionID.Value));
            txtJournalNo.Text = entity.JournalNo;

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("GLTransactionID = {0} AND GCItemDetailStatus != '{1}' AND IsDeleted = 0 AND DisplayOrder != 1", hdnGLTransactionID.Value, Constant.TransactionStatus.VOID);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvGLTransactionDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MATRIX);
            }

            List<vGLTransactionDt> lstEntity = BusinessLayer.GetvGLTransactionDtList(filterExpression, Constant.GridViewPageSize.GRID_MATRIX, pageIndex, "DisplayOrder, TransactionDtID");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            }

        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            int pageCount = 1;

            string[] param = e.Parameter.Split('|');

            string result = param[0] + "|";
            string errMessage = "";

            if (param[0] == "changepage")
            {
                BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                result = "changepage";
            }
            else if (param[0] == "refresh")
            {
                BindGridView(1, true, ref pageCount);
                result = string.Format("refresh|{0}", pageCount);
            }
            else
            {
                if (param[0] == "save")
                {

                    if (OnSaveEditRecord(ref errMessage))
                    {
                        result += "success";
                    }
                    else
                    {
                        result += string.Format("fail|{0}", errMessage);
                    }

                }

                BindGridView(1, true, ref pageCount);
                result += "|" + pageCount;
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            GLTransactionDtDao glTransDtDao = new GLTransactionDtDao(ctx);
            ARReceivingHdDao arRcvHdDao = new ARReceivingHdDao(ctx);
            try
            {
                bool isAllowChange = false;
                GLTransactionDt glTransDt = glTransDtDao.Get(Convert.ToInt32(hdnID.Value));

                string filterARRcv = string.Format("ARReceivingNo = '{0}' AND GLTransactionID = {1} AND GLTransactionDtID = {2}", glTransDt.ReferenceNo, glTransDt.GLTransactionID, glTransDt.TransactionDtID);
                List<ARReceivingHd> arRcvHdLst = BusinessLayer.GetARReceivingHdList(filterARRcv, ctx);
                foreach (ARReceivingHd arRcvHd in arRcvHdLst)
                {
                    string filterARInvRcv = string.Format("ARReceivingID = {0} AND IsDeleted = 0 AND ReceivingAmount != 0", arRcvHd.ARReceivingID);
                    List<ARInvoiceReceiving> arInvRcvLst = BusinessLayer.GetARInvoiceReceivingList(filterARInvRcv, ctx);

                    if ((arRcvHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || arRcvHd.GCTransactionStatus == Constant.TransactionStatus.APPROVED) && arInvRcvLst.Count() == 0)
                    {
                        if (hdnCustomerGroupIDCBP.Value != "")
                        {
                            arRcvHd.CustomerGroupID = Convert.ToInt32(hdnCustomerGroupIDCBP.Value);
                        }
                        else
                        {
                            arRcvHd.CustomerGroupID = null;
                        }

                        if (hdnBusinessPartnerIDCBP.Value != "")
                        {
                            arRcvHd.BusinessPartnerID = Convert.ToInt32(hdnBusinessPartnerIDCBP.Value);
                        }
                        else
                        {
                            arRcvHd.BusinessPartnerID = null;
                        }

                        arRcvHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        arRcvHdDao.Update(arRcvHd);

                        isAllowChange = true;
                    }
                }

                if (isAllowChange)
                {
                    if (hdnCustomerGroupIDCBP.Value != "")
                    {
                        glTransDt.CustomerGroupID = Convert.ToInt32(hdnCustomerGroupIDCBP.Value);
                    }
                    else
                    {
                        glTransDt.CustomerGroupID = null;
                    }

                    if (hdnBusinessPartnerIDCBP.Value != "")
                    {
                        glTransDt.BusinessPartnerID = Convert.ToInt32(hdnBusinessPartnerIDCBP.Value);
                    }
                    else
                    {
                        glTransDt.BusinessPartnerID = null;
                    }


                    glTransDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    glTransDtDao.Update(glTransDt);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Maaf tidak bisa mengubah penjamin bayar karena sudah ada alokasi penerimaan piutang.";
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
    }
}