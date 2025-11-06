using System;
using System.Collections.Generic;
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
    public partial class RevenueSharingEditEntry : BasePageTrx
    {
        List<StandardCode> lstFormulaType = null;
        List<TransRevenueSharingDtComp> lstTransRevenueSharingDtComp = null;
        List<TransRevenueSharingDtComp> lstEntityComp = null;

        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.REVENUE_SHARING_EDIT;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            hdnParamedicID.Value = AppSession.ParamedicID.ToString();
            txtDiscount1.Text = "0";

            string moduleName = Helper.GetModuleName();
            string ModuleID = Helper.GetModuleID(moduleName);
            GetUserMenuAccess menu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault();
            string CRUDMode = menu.CRUDMode;
            hdnIsAllowVoidByReason.Value = CRUDMode.Contains("X") ? "1" : "0";
        }

        protected string IsEditable()
        {
            return hdnIsEditable.Value;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = false;
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            decimal TotalRsAmount = 0;
            decimal TotalBrutoAmount = 0;
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    TotalRsAmount = -1;
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount, ref TotalRsAmount, ref TotalBrutoAmount);
                    result = "changepage" + "|" + TotalRsAmount.ToString("N");
                }
                else // refresh
                {
                    BindGridView(1, true, ref pageCount, ref TotalRsAmount, ref TotalBrutoAmount);
                    txtTotalRSAmount.Text = TotalRsAmount.ToString(Constant.FormatString.NUMERIC_2);
                    txtTotalBrutoAmount.Text = TotalBrutoAmount.ToString(Constant.FormatString.NUMERIC_2);
                    result = "refresh|" + pageCount + "|" + TotalRsAmount.ToString(Constant.FormatString.NUMERIC_2) + "|" + TotalBrutoAmount.ToString(Constant.FormatString.NUMERIC_2);
                }
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        #region Load Entity
        protected string OnGetRevenueSharingFilterExpression()
        {
            return string.Format("ParamedicID = {0}", AppSession.ParamedicID);
        }

        public override int OnGetRowCount()
        {
            return BusinessLayer.GetvTransRevenueSharingHdRowCount(OnGetRevenueSharingFilterExpression());
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            vTransRevenueSharingHd entity = BusinessLayer.GetvTransRevenueSharingHd(OnGetRevenueSharingFilterExpression(), PageIndex, "RSTransactionID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = OnGetRevenueSharingFilterExpression();
            PageIndex = BusinessLayer.GetvTransRevenueSharingHdRowIndex(filterExpression, keyValue, "RSTransactionID DESC");
            vTransRevenueSharingHd entity = BusinessLayer.GetvTransRevenueSharingHd(filterExpression, PageIndex, "RSTransactionID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        private void EntityToControl(vTransRevenueSharingHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                watermarkText = entity.TransactionStatusWatermark;
                hdnIsEditable.Value = "0";
            }
            else
            {
                hdnIsEditable.Value = "1";
            }

            hdnGCTransactionStatus.Value = entity.GCTransactionStatus;

            hdnRSTransactionID.Value = entity.RSTransactionID.ToString();
            txtRevenueSharingNo.Text = entity.RevenueSharingNo;
            txtProcessedDate.Text = entity.ProcessedDate.ToString(Constant.FormatString.DATE_FORMAT);
            txtReduction.Text = entity.Reduction;
            txtPaymentMethod.Text = entity.PaymentMethod;
            txtClinicGroup.Text = entity.ClinicGroup;
            txtPeriodeType.Text = entity.PeriodeType;
            txtPeriode.Text = entity.cfPeriodeText;
            txtRemarks.Text = entity.Remarks;

            if (entity.GCTransactionStatus != Constant.TransactionStatus.VOID)
            {
                hdnFilterExpression.Value = string.Format("IsDeleted = 0 AND ParamedicID = {0} AND RSTransactionID = {1}", AppSession.ParamedicID, hdnRSTransactionID.Value);
            }
            else
            {
                hdnFilterExpression.Value = string.Format("IsDeleted = 1 AND ParamedicID = {0} AND RSTransactionID = {1}", AppSession.ParamedicID, hdnRSTransactionID.Value);
            }

            txtTotalBrutoAmount.Text = entity.TotalTransactionAmount.ToString(Constant.FormatString.NUMERIC_2);
            txtTotalRSAmount.Text = entity.TotalRevenueSharingAmount.ToString(Constant.FormatString.NUMERIC_2);

            Decimal TotalRSAmount = 0;
            Decimal TotalBrutoAmount = 0;
            BindGridView(1, true, ref PageCount, ref TotalRSAmount, ref TotalBrutoAmount);

            hdnPageCount.Value = PageCount.ToString();

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

            if (entity.VoidDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
            {
                trVoidBy.Style.Add("display", "none");
                trVoidDate.Style.Add("display", "none");

                divVoidBy.InnerHtml = "";
                divVoidDate.InnerHtml = "";
            }
            else
            {
                trVoidBy.Style.Remove("display");
                trVoidDate.Style.Remove("display");

                divVoidBy.InnerHtml = entity.VoidByName;
                divVoidDate.InnerHtml = entity.VoidDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
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

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount, ref decimal TotalRsAmount, ref decimal TotalBrutoAmount)
        {
            rptFormulaType.DataSource = lstFormulaType;
            rptFormulaType.DataBind();

            if (hdnRSTransactionID.Value != "")
            {
                string filterExpression = hdnFilterExpression.Value;

                if (hdnFilterExpressionQuickSearch.Value != null && hdnFilterExpressionQuickSearch.Value != "")
                    filterExpression += String.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvTransRevenueSharingDtRowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_REVENUE_SHARING_LIST);
                }

                List<vTransRevenueSharingDt> lstEntity = BusinessLayer.GetvTransRevenueSharingDtList(filterExpression, Constant.GridViewPageSize.GRID_REVENUE_SHARING_LIST, pageIndex, "RegistrationNo, TransactionNo, ItemName ASC");

                filterExpression = string.Join(",", lstEntity.Select(p => p.TransactionDtID).ToArray());
                if (filterExpression != "")
                    lstTransRevenueSharingDtComp = BusinessLayer.GetTransRevenueSharingDtCompList(string.Format("TransactionDtID IN ({0})", filterExpression));

                if (TotalRsAmount > -1)
                {
                    TotalRsAmount = BusinessLayer.GetTransRevenueSharingHd(Convert.ToInt32(hdnRSTransactionID.Value)).TotalRevenueSharingAmount;
                }

                if (TotalBrutoAmount > -1)
                {
                    TotalBrutoAmount = BusinessLayer.GetTransRevenueSharingHd(Convert.ToInt32(hdnRSTransactionID.Value)).TotalTransactionAmount;
                }

                lvwView.DataSource = lstEntity;
                lvwView.DataBind();

            }
            else
            {
                lvwView.DataSource = null;
                lvwView.DataBind();
            }
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vTransRevenueSharingDt entity = e.Item.DataItem as vTransRevenueSharingDt;
                lstEntityComp = lstTransRevenueSharingDtComp.Where(p => p.TransactionDtID == entity.TransactionDtID).ToList();

                Repeater rptSharingComp = (Repeater)e.Item.FindControl("rptSharingComp");
                rptSharingComp.DataSource = lstEntityComp;
                rptSharingComp.DataBind();
            }
        }

        protected int GetFormulaTypeCount()
        {
            if (lstFormulaType != null)
            {
                return lstFormulaType.Count;
            }
            else 
            {
                return 0;
            }
        }

        #endregion

        #region Header
        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TransRevenueSharingHdDao entityHdDao = new TransRevenueSharingHdDao(ctx);

            try
            {
                TransRevenueSharingHd entity = entityHdDao.Get(Convert.ToInt32(hdnRSTransactionID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entity.Remarks = txtRemarks.Text;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entity);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Jasa Medis dengan nomor transaksi " + entity.RevenueSharingNo + " tidak dapat diubah. Harap refresh halaman ini.";
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

        #region Custom Button
        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TransRevenueSharingHdDao entityHdDao = new TransRevenueSharingHdDao(ctx);
            TransRevenueSharingDtDao entityDtDao = new TransRevenueSharingDtDao(ctx);

            try
            {
                TransRevenueSharingHd entityHd = BusinessLayer.GetTransRevenueSharingHd(Convert.ToInt32(hdnRSTransactionID.Value));
                if (type.Contains("justvoid"))
                {
                    #region Void

                    string[] param = type.Split(';');
                    string gcDeleteReason = param[1];
                    string reason = param[2];

                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        List<TransRevenueSharingDt> lstEntity = BusinessLayer.GetTransRevenueSharingDtList(string.Format(
                                    "RSTransactionID = {0} AND IsDeleted = 0", hdnRSTransactionID.Value));

                        foreach (TransRevenueSharingDt entityDt in lstEntity)
                        {
                            entityDt.IsDeleted = true;
                            entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDt.LastUpdatedDate = DateTime.Now;
                            entityDtDao.Update(entityDt);
                        }

                        entityHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                        entityHd.GCVoidReason = gcDeleteReason;
                        entityHd.VoidReason = reason;
                        entityHd.VoidBy = AppSession.UserLogin.UserID;
                        entityHd.VoidDate = DateTime.Now;
                        entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityHd.LastUpdatedDate = DateTime.Now;
                        entityHdDao.Update(entityHd);

                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Jasa Medis nomor " + entityHd.RevenueSharingNo + " tidak dapat diubah. Harap refresh halaman ini.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }

                    #endregion
                }
                else if (type.Contains("reopen"))
                {
                    #region Re-Open

                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.APPROVED)
                    {
                        string filterSummaryDt = string.Format("RSTransactionID = {0} AND IsDeleted = 0 AND GCTransactionStatus != '{1}'", entityHd.RSTransactionID, Constant.TransactionStatus.VOID);
                        List<vTransRevenueSharingSummaryDt> lstSummaryDt = BusinessLayer.GetvTransRevenueSharingSummaryDtList(filterSummaryDt, ctx);

                        if (lstSummaryDt.Count == 0)
                        {
                            entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                            entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityHd.LastUpdatedDate = DateTime.Now;
                            entityHdDao.Update(entityHd);

                            ctx.CommitTransaction();
                        }
                        else
                        {
                            result = false;
                            errMessage = "Jasa Medis nomor " + entityHd.RevenueSharingNo + " tidak dapat diubah karena sudah ada proses Rekap Jasa Medis.";
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
                    }
                    else
                    {
                        result = false;
                        errMessage = "Jasa Medis nomor " + entityHd.RevenueSharingNo + " tidak dapat diubah. Harap refresh halaman ini.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }

                    #endregion
                }

                retval = entityHd.RevenueSharingNo;
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
        
        #region Process Detail
        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";

            if (e.Parameter == "save")
            {
                if (OnProcessRecord(ref errMessage))
                    result = "success";
                else
                    result = "fail|" + errMessage;
            }
            else if (e.Parameter == "delete")
            {
                if (OnDeleteRecord(ref errMessage))
                    result = "success";
                else
                    result = "fail|" + errMessage;
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            bool result = false;
            try
            {
                int ID = Convert.ToInt32(hdnTransactionDtID.Value);
                TransRevenueSharingDt entity = BusinessLayer.GetTransRevenueSharingDt(ID);
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entity.LastUpdatedDate = DateTime.Now;
                BusinessLayer.UpdateTransRevenueSharingDt(entity);
                result = true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                result = false;
            }

            return result;
        }

        private bool OnProcessRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TransRevenueSharingDtDao entityDtDao = new TransRevenueSharingDtDao(ctx);
            TransRevenueSharingDtCompDao entityCompDao = new TransRevenueSharingDtCompDao(ctx);

            try
            {
                int TransactionDtID = Convert.ToInt32(hdnTransactionDtID.Value);
                TransRevenueSharingDt entity = entityDtDao.Get(TransactionDtID);
                List<TransRevenueSharingDtComp> lstEntityComp = BusinessLayer.GetTransRevenueSharingDtCompList(string.Format("TransactionDtID = {0}", entity.TransactionDtID), ctx);
                ControlToEntity(entity, lstEntityComp);

                foreach (TransRevenueSharingDtComp obj in lstEntityComp)
                {
                    entityCompDao.Update(obj);
                }
                entityDtDao.Update(entity);

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

        private void ControlToEntity(TransRevenueSharingDt entity, List<TransRevenueSharingDtComp> lstEntityComp)
        {
            entity.CreditCardFeeAmount = Convert.ToDecimal(txtCCFee.Text);
            entity.DiscountAmount = Convert.ToDecimal(txtDiscountAmount.Text);
            entity.RevenueSharingAmount = Convert.ToDecimal(Request.Form[txtNettAfterDiscount.UniqueID]);

            if (chkServiceIsVariable.Checked)
            {
                entity.IsVariable = true;
                entity.TransactionAmount = Convert.ToDecimal(txtTransactionAmount.Text);
            }
            else
            {
                entity.IsVariable = false;
            }

            #region RevenueSharingDt
            foreach (RepeaterItem item in rptSharingComp.Items)
            {
                HtmlInputHidden hdnGCSharingComponent = (HtmlInputHidden)item.FindControl("hdnGCSharingComponent");

                TextBox txtCompValue = (TextBox)item.FindControl("txtCompValue");
                TransRevenueSharingDtComp entityComp = lstEntityComp.FirstOrDefault(p => p.GCSharingComponent == hdnGCSharingComponent.Value);
                if (entityComp == null)
                {
                    entityComp = new TransRevenueSharingDtComp();
                    entityComp.GCSharingComponent = hdnGCSharingComponent.Value;
                    entityComp.ComponentAmount = Convert.ToDecimal(txtCompValue.Text);
                    lstEntityComp.Add(entityComp);
                }
                else
                {
                    entityComp.ComponentAmount = Convert.ToDecimal(txtCompValue.Text);
                }

            }
            #endregion
        }
        #endregion

        #region Discount
        protected void cbpProcessDiscount_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";

            if (ApplyDiscountToAll(ref errMessage))
                result = "success";
            else
                result = "fail|" + errMessage;

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private Boolean ApplyDiscountToAll(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TransRevenueSharingDtDao entityDtDao = new TransRevenueSharingDtDao(ctx);
            try
            {
                string filterExpression = string.Format("IsDeleted = 0 AND RSTransactionID = {0}", hdnRSTransactionID.Value);

                Decimal discount = Convert.ToDecimal(txtDiscount1.Text);
                List<TransRevenueSharingDt> lstRevenueSharingDt = BusinessLayer.GetTransRevenueSharingDtList(filterExpression, ctx);

                string lstID = "";
                foreach (TransRevenueSharingDt entity in lstRevenueSharingDt)
                {
                    if (lstID != "")
                        lstID += ",";
                    lstID += entity.TransactionDtID.ToString();
                }

                lstTransRevenueSharingDtComp = BusinessLayer.GetTransRevenueSharingDtCompList(string.Format("TransactionDtID IN ({0})", lstID));
                Decimal RevenueSharingAmount = 0;

                foreach (TransRevenueSharingDt obj in lstRevenueSharingDt)
                {
                    RevenueSharingAmount = 0;
                    decimal totalComponentAmount = lstTransRevenueSharingDtComp.Where(p => p.TransactionDtID == obj.TransactionDtID).Sum(p => p.ComponentAmount);
                    RevenueSharingAmount = obj.TransactionAmount - totalComponentAmount;

                    Decimal pengurang = Convert.ToDecimal(RevenueSharingAmount * discount / 100);
                    obj.RevenueSharingAmount = RevenueSharingAmount - pengurang;
                    obj.DiscountAmount = pengurang;
                    entityDtDao.Update(obj);
                }

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

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            lstFormulaType = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND StandardCodeID != '{1}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.REVENUE_SHARING_COMPONENT, Constant.RevenueSharingComponent.PARAMEDIC));
            rptSharingComp.DataSource = lstFormulaType;
            rptSharingComp.DataBind();
        }
    }
}