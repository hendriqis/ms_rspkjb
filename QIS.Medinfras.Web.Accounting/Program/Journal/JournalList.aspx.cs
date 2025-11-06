using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class JournalList : BasePageTrx
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;

        List<TransactionType> lstTransactionType = BusinessLayer.GetTransactionTypeList("TransactionCode LIKE '72%'");

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.JOURNAL_LIST;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public string GetGCTransactionStatusOpen()
        {
            return Constant.TransactionStatus.OPEN;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            txtFromJournalDate.Text = DateTime.Today.AddDays(-7).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtToJournalDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            CurrPage = 1;

            List<StandardCode> lstSC = BusinessLayer.GetStandardCodeList(String.Format("StandardCodeID IN ('{0}','{1}','{2}','{3}','{4}')",
                                                    Constant.TransactionStatus.OPEN, Constant.TransactionStatus.APPROVED, Constant.TransactionStatus.PROCESSED,
                                                    Constant.TransactionStatus.CLOSED, Constant.TransactionStatus.VOID));
            Methods.SetComboBoxField<StandardCode>(cboTransactionStatus, lstSC, "StandardCodeName", "StandardCodeID");
            cboTransactionStatus.SelectedIndex = 0;

            btnReOpen.Attributes.Add("style", "display:none");

            List<StandardCode> lstSC1 = BusinessLayer.GetStandardCodeList(String.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.JOURNAL_GROUP));
            Methods.SetComboBoxField<StandardCode>(cboGCJournalGroup, lstSC1, "StandardCodeName", "StandardCodeID");
            cboGCJournalGroup.SelectedIndex = 0;
            hdnJournalGroup.Value = Constant.JournalGroup.PENDAPATAN_PENERIMAAN;

            BindGridView(CurrPage, true, ref PageCount);
        }

        private string GetFilterExpression()
        {
            string filterExpression = "";
            string GCJournalGroup = cboGCJournalGroup.Value.ToString();

            switch (GCJournalGroup) //bagian ini minta tolong jangan dihapus, jika tidak terpakai di comment saja
            {
                case Constant.JournalGroup.PENDAPATAN_PENERIMAAN: filterExpression = string.Format("TransactionCode BETWEEN '7200' AND '7220'"); break;
                case Constant.JournalGroup.HUTANG_PIUTANG: filterExpression = string.Format("TransactionCode BETWEEN '7221' AND '7240'"); break;
                case Constant.JournalGroup.INVENTORY: filterExpression = string.Format("TransactionCode BETWEEN '7241' AND '7260'"); break;
                case Constant.JournalGroup.PHARMACY: filterExpression = string.Format("TransactionCode BETWEEN '7261' AND '7270'"); break;
                case Constant.JournalGroup.FIXED_ASSET: filterExpression = string.Format("TransactionCode BETWEEN '7271' AND '7280'"); break;
                case Constant.JournalGroup.MEMORIAL: filterExpression = string.Format("TransactionCode BETWEEN '7281' AND '7300'"); break;
            }

            filterExpression += string.Format(" AND JournalDate BETWEEN '{0}' AND '{1}'", Helper.GetDatePickerValue(txtFromJournalDate).ToString("yyyyMMdd"), Helper.GetDatePickerValue(txtToJournalDate).ToString("yyyyMMdd"));
            
            if (hdnTransactionStatus.Value == Constant.TransactionStatus.PROCESSED)
            {
                filterExpression += string.Format(" AND GCTransactionStatus = '{0}' AND TransactionCode != '{1}'", cboTransactionStatus.Value, Constant.TransactionCode.JOURNAL_MEMORIAL_IKHTISAR);
            }
            else if (hdnTransactionStatus.Value == Constant.TransactionStatus.APPROVED)
            {
                filterExpression += string.Format(" AND GCTransactionStatus = '{0}' AND TransactionCode != '{1}'", cboTransactionStatus.Value, Constant.TransactionCode.JOURNAL_MEMORIAL_IKHTISAR);
            }
            else
            {
                filterExpression += string.Format(" AND GCTransactionStatus = '{0}'", cboTransactionStatus.Value);
            }

            if (!String.IsNullOrEmpty(hdnTransactionCode.Value))
            {
                filterExpression += string.Format(" AND TransactionCode = '{0}'", hdnTransactionCode.Value);
            }

            if (hdnFilterExpressionQuickSearch.Value != "")
            {
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            }

            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvGLTransactionHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vGLTransactionHd> lstEntity = BusinessLayer.GetvGLTransactionHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "JournalDate, JournalGroup, JournalNo");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cboGCJournalGroup_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string GCStatus = hdnTransactionStatus.Value;
            List<StandardCode> lstSC = null;

            //if (GCStatus == Constant.TransactionStatus.OPEN)
            //{
            //    lstSC = BusinessLayer.GetStandardCodeList(String.Format("ParentID = '{0}' AND StandardCodeID != '{1}' AND IsActive = 1 AND IsDeleted = 0",
            //                                            Constant.StandardCode.JOURNAL_GROUP, Constant.JournalGroup.MEMORIAL));
            //}
            //else
            //{
            //    lstSC = BusinessLayer.GetStandardCodeList(String.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0",
            //                                            Constant.StandardCode.JOURNAL_GROUP));
            //}

            lstSC = BusinessLayer.GetStandardCodeList(String.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.JOURNAL_GROUP));

            Methods.SetComboBoxField<StandardCode>(cboGCJournalGroup, lstSC, "StandardCodeName", "StandardCodeID");
            cboGCJournalGroup.SelectedIndex = 0;

            hdnJournalGroup.Value = cboGCJournalGroup.Value.ToString();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            if (type == "approve")
            {
                ApproveJournal(ref result, ref errMessage);
            }
            else if (type == "reopen")
            {
                ReopenJournal(ref result, ref errMessage);
            }
            else
            {
                result = false;
                errMessage = "Proses Salah";
            }

            return result;
        }

        private void ApproveJournal(ref bool result, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            GLTransactionHdDao glTransactionhdDao = new GLTransactionHdDao(ctx);
            GLTransactionDtDao glTransactionDtDao = new GLTransactionDtDao(ctx);
            ARReceivingHdDao ARReceivingHdDao = new ARReceivingHdDao(ctx);
            try
            {
                GLTransactionHd entityHD = glTransactionhdDao.Get(Convert.ToInt32(hdnID.Value));
                if (entityHD.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    if (entityHD.CreditAmount == entityHD.DebitAmount)
                    {
                        entityHD.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                        entityHD.ApprovedBy = AppSession.UserLogin.UserID;
                        entityHD.ApprovedDate = DateTime.Now;
                        entityHD.LastUpdatedBy = AppSession.UserLogin.UserID;

                        List<GLTransactionDt> lstEntityDt = BusinessLayer.GetGLTransactionDtList(String.Format("GLTransactionID = {0} AND GCItemDetailStatus = '{1}' AND IsDeleted = 0", hdnID.Value, Constant.TransactionStatus.OPEN), ctx);
                        foreach (GLTransactionDt entityDt in lstEntityDt)
                        {
                            entityDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                            entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            ctx.Command.CommandTimeout = 90;
                            glTransactionDtDao.Update(entityDt);

                            // APPROVE ARReceivingHd
                            if (entityHD.GCTreasuryGroup == Constant.TreasuryGroup.AR_RECEIVING && entityDt.DisplayOrder != 1)
                            {
                                if (entityDt.ReferenceNo != null && entityDt.ReferenceNo != "")
                                {
                                    string filterARRcv = string.Format("ARReceivingNo = '{0}'", entityDt.ReferenceNo);
                                    List<ARReceivingHd> rcvHDLst = BusinessLayer.GetARReceivingHdList(filterARRcv, ctx);
                                    if (rcvHDLst.Count() > 0)
                                    {
                                        ARReceivingHd arrcv = rcvHDLst.FirstOrDefault();
                                        arrcv.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                                        arrcv.ApprovedBy = AppSession.UserLogin.UserID;
                                        arrcv.ApprovedDate = DateTime.Now;
                                        arrcv.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        ctx.Command.CommandTimeout = 90;
                                        ARReceivingHdDao.Update(arrcv);
                                    }
                                }
                            }
                        }

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        ctx.Command.CommandTimeout = 90;
                        glTransactionhdDao.Update(entityHD);

                        result = true;
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "GAGAL APPROVE : Jurnal " + entityHD.JournalNo + " Tidak Balance";
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "GAGAL APPROVE : Jurnal " + entityHD.JournalNo + " sudah diproses";
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
        }

        private void ReopenJournal(ref bool result, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            GLTransactionHdDao glTransactionhdDao = new GLTransactionHdDao(ctx);
            try
            {
                GLTransactionHd entityHD = glTransactionhdDao.Get(Convert.ToInt32(hdnID.Value));
                if (entityHD.GCTransactionStatus == Constant.TransactionStatus.APPROVED && !entityHD.FlagStaging)
                {
                    ctx.Command.CommandTimeout = 90;
                    result = BusinessLayer.ReopenJournal(Convert.ToInt32(hdnID.Value), AppSession.UserLogin.UserID, ctx);

                    if (result)
                    {
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "GAGAL RE-OPEN : Detail jurnal " + entityHD.JournalNo + " sudah ada proses alokasi piutang";
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "GAGAL RE-OPEN : Jurnal " + entityHD.JournalNo + " sudah diproses";
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
        }
    }
}