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
    public partial class RevenueSharingAdjustmentTransactionEntry : BasePageTrx
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.REVENUE_SHARING_ADJUSTMENT_TRANSACTION;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected string IsEditable()
        {
            return hdnIsEditable.Value;
        }

        protected string OnGetRevenueSharingFilterExpression()
        {
            return string.Format("ParamedicID = {0} AND GCTransactionStatus = '{1}'", AppSession.ParamedicID, Constant.TransactionStatus.OPEN);
        }

        protected string OnGetAdjustmentGroupPlus()
        {
            return Constant.RevenueSharingAdjustmentGroup.PENAMBAHAN;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            GetUserMenuAccess menu = BusinessLayer.GetUserMenuAccess(Constant.Module.FINANCE, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault();
            string CRUDMode = menu.CRUDMode;
            hdnIsAllowApprove.Value = CRUDMode.Contains("A") ? "1" : "0";

            hdnParamedicID.Value = AppSession.ParamedicID.ToString();

            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.REVENUE_SHARING_ADJUSTMENT_GROUP, Constant.StandardCode.REVENUE_SHARING_ADJUSTMENT_TYPE));
            Methods.SetRadioButtonListField(rblAdjustment, lstSc.Where(p => p.ParentID == Constant.StandardCode.REVENUE_SHARING_ADJUSTMENT_GROUP).ToList(), "StandardCodeName", "StandardCodeID");

            Methods.SetComboBoxField(cboAdjustmentTypeAdd, lstSc.Where(p => p.ParentID == Constant.StandardCode.REVENUE_SHARING_ADJUSTMENT_TYPE && p.TagProperty == "1").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboAdjustmentTypeMin, lstSc.Where(p => p.ParentID == Constant.StandardCode.REVENUE_SHARING_ADJUSTMENT_TYPE && p.TagProperty == "2").ToList(), "StandardCodeName", "StandardCodeID");

            rblAdjustment.SelectedValue = OnGetAdjustmentGroupPlus();

            Helper.SetControlEntrySetting(cboAdjustmentTypeAdd, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(cboAdjustmentTypeMin, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtAdjustmentAmount, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(chkRevenueSharingFee, new ControlEntrySetting(true, true, false), "mpEntryPopup");

            BindGridDetail();
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnRSAdjustmentID, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(hdnRSAdjustmentNo, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRSAdjustmentNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtRSAdjustmentDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtTotalAdjustmentAmountBruto, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtTotalAdjustmentAmount, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(hdnGCTransactionStatus, new ControlEntrySetting(true, true, false));
        }

        protected string GetFilterExpression()
        {
            return string.Format("ParamedicID = {0}", AppSession.ParamedicID);
        }

        public override int OnGetRowCount()
        {
            string filterExpression = GetFilterExpression();
            return BusinessLayer.GetvTransRevenueSharingAdjustmentHdRowCount(filterExpression);
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            vTransRevenueSharingAdjustmentHd entity = BusinessLayer.GetvTransRevenueSharingAdjustmentHdList(filterExpression, PageIndex, " RSAdjustmentID DESC").FirstOrDefault();
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            PageIndex = BusinessLayer.GetvTransRevenueSharingAdjustmentHdRowIndex(filterExpression, keyValue, "RSAdjustmentID DESC");
            vTransRevenueSharingAdjustmentHd entity = BusinessLayer.GetvTransRevenueSharingAdjustmentHdList(filterExpression, PageIndex, "RSAdjustmentID DESC").FirstOrDefault();
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        private void EntityToControl(vTransRevenueSharingAdjustmentHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            hdnRSAdjustmentID.Value = entity.RSAdjustmentID.ToString();
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                watermarkText = entity.TransactionStatus;
                hdnIsEditable.Value = "0";

                SetControlEntrySetting(txtRSAdjustmentNo, new ControlEntrySetting(true, false, false));
                SetControlEntrySetting(txtRSAdjustmentDate, new ControlEntrySetting(true, false, false));
                SetControlEntrySetting(txtTotalAdjustmentAmountBruto, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(txtTotalAdjustmentAmount, new ControlEntrySetting(false, false, false));
            }
            else
            {
                hdnIsEditable.Value = "1";

            }

            txtRSAdjustmentNo.Text = entity.RSAdjustmentNo;
            txtRSAdjustmentDate.Text = entity.cfRSAdjustmentDateInDatePickerString;
            txtTotalAdjustmentAmount.Text = entity.cfTotalAdjustmentAmountInString;
            hdnGCTransactionStatus.Value = entity.GCTransactionStatus;

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

            BindGridDetail();
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TransRevenueSharingAdjustmentHdDao entityHdDao = new TransRevenueSharingAdjustmentHdDao(ctx);
            TransRevenueSharingAdjustmentDtDao entityDtDao = new TransRevenueSharingAdjustmentDtDao(ctx);

            try
            {
                TransRevenueSharingAdjustmentHd entityHd = new TransRevenueSharingAdjustmentHd();
                entityHd.RSAdjustmentDate = Helper.GetDatePickerValue(txtRSAdjustmentDate.Text);
                entityHd.RSAdjustmentNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.TRANS_REVENUE_SHARING_ADJUSTMENT_ENTRY, entityHd.RSAdjustmentDate, ctx);
                entityHd.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                int RSAdjustmentID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);

                hdnRSAdjustmentID.Value = RSAdjustmentID.ToString();
                retval = hdnRSAdjustmentNo.Value = entityHd.RSAdjustmentNo;

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

        protected override bool OnVoidRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TransRevenueSharingAdjustmentHdDao entityHdDao = new TransRevenueSharingAdjustmentHdDao(ctx);
            TransRevenueSharingAdjustmentDtDao entityDtDao = new TransRevenueSharingAdjustmentDtDao(ctx);

            try
            {
                if (hdnRSAdjustmentID.Value != "" && hdnRSAdjustmentID.Value != "0")
                {
                    TransRevenueSharingAdjustmentHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnRSAdjustmentID.Value));
                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        entityHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                        entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityHdDao.Update(entityHd);

                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
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

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TransRevenueSharingAdjustmentHdDao entityHdDao = new TransRevenueSharingAdjustmentHdDao(ctx);
            TransRevenueSharingAdjustmentDtDao entityDtDao = new TransRevenueSharingAdjustmentDtDao(ctx);
            TransRevenueSharingSummaryHdDao srsHdDao = new TransRevenueSharingSummaryHdDao(ctx);

            try
            {
                if (hdnRSAdjustmentID.Value != "" && hdnRSAdjustmentID.Value != "0")
                {
                    TransRevenueSharingAdjustmentHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnRSAdjustmentID.Value));

                    if (type == "approve")
                    {
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                        {
                            entityHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                            entityHd.ApprovedBy = AppSession.UserLogin.UserID;
                            entityHd.ApprovedDate = DateTime.Now;
                            entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityHdDao.Update(entityHd);

                            ctx.CommitTransaction();
                        }
                        else
                        {
                            result = false;
                            errMessage = "Transaksi penyesuaian dengan nomor " + entityHd.RSAdjustmentNo + " tidak dapat diubah. Harap refresh halaman ini.";
                            ctx.RollBackTransaction();
                        }
                    }
                    else if (type == "reopen")
                    {
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.APPROVED && (entityHd.RSSummaryID == null || entityHd.RSSummaryID == 0))
                        {
                            entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                            entityHd.ApprovedBy = null;
                            entityHd.ApprovedDate = null;
                            entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityHdDao.Update(entityHd);

                            ctx.CommitTransaction();
                        }
                        else
                        {
                            if (entityHd.RSSummaryID != null && entityHd.RSSummaryID != 0)
                            {
                                TransRevenueSharingSummaryHd srsHd = srsHdDao.Get(Convert.ToInt32(entityHd.RSSummaryID));
                                errMessage = "Transaksi penyesuaian dengan nomor " + entityHd.RSAdjustmentNo + " tidak dapat diubah karena sudah digunakan dalam penyesuaian rekap jasmed di nomor " + srsHd.RSSummaryNo + ".";
                            }
                            else
                            {
                                errMessage = "Transaksi penyesuaian dengan nomor " + entityHd.RSAdjustmentNo + " tidak dapat diubah. Harap refresh halaman ini.";
                            }
                            result = false;
                            ctx.RollBackTransaction();
                        }
                    }

                    retval = entityHd.RSAdjustmentNo;
                }
                else
                {
                    result = false;
                    errMessage = "Transaksi tidak ditemukan. Harap refresh halaman ini.";
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

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridDetail();
        }

        private void BindGridDetail()
        {
            string filterExpression = "";
            int RSAdjustmentID = hdnRSAdjustmentID.Value != "" ? Convert.ToInt32(hdnRSAdjustmentID.Value) : 0;

            filterExpression = string.Format("IsDeleted = 0 AND ParamedicID = {0} AND RSAdjustmentID = {1} AND GCRSAdjustmentGroup = '{2}'", AppSession.ParamedicID, RSAdjustmentID, rblAdjustment.SelectedValue);

            List<vTransRevenueSharingAdjustmentDt> lstEntity = BusinessLayer.GetvTransRevenueSharingAdjustmentDtList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();

            txtTotalAdjustmentAmountBruto.Text = lstEntity.Sum(a => a.AdjustmentAmountBRUTO).ToString(Constant.FormatString.NUMERIC_2);
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            string param = e.Parameter;

            string result = param + "|";
            string retval = "";
            string errMessage = "";

            if (param == "save")
            {
                if (hdnID.Value.ToString() != "")
                {
                    if (OnSaveEditDtRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddDtRecord(ref errMessage, ref retval))
                        result += string.Format("success|{0}", retval);
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param == "delete")
            {
                if (OnDeleteDtRecord(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(TransRevenueSharingAdjustmentDt entity)
        {
            entity.RSAdjustmentID = Convert.ToInt32(hdnRSAdjustmentID.Value);
            entity.GCRSAdjustmentGroup = rblAdjustment.SelectedValue;
            if (rblAdjustment.SelectedValue == OnGetAdjustmentGroupPlus())
            {
                entity.GCRSAdjustmentType = cboAdjustmentTypeAdd.Value.ToString();
            }
            else
            {
                entity.GCRSAdjustmentType = cboAdjustmentTypeMin.Value.ToString();
            }
            entity.AdjustmentAmountBRUTO = Convert.ToDecimal(txtAdjustmentAmountBruto.Text);
            entity.AdjustmentAmount = Convert.ToDecimal(txtAdjustmentAmount.Text);
            entity.IsTaxed = chkRevenueSharingFee.Checked;
            entity.Remarks = txtRemarks.Text;
            if (hdnRevenueSharingID.Value != null && hdnRevenueSharingID.Value != "0" && hdnRevenueSharingID.Value != "")
            {
                entity.RevenueSharingID = Convert.ToInt32(hdnRevenueSharingID.Value);
            }
            else
            {
                entity.RevenueSharingID = null;
            }
            entity.RegistrationNo = txtRegistrationNo.Text;
            entity.RegistrationDate = txtRegistrationDate.Text;
            entity.DischargeDate = txtDischargeDate.Text;
            entity.ReceiptNo = txtReceiptNo.Text;
            entity.InvoiceNo = txtInvoiceNo.Text;
            entity.ReferenceNo = txtReferenceNo.Text;
            entity.BusinessPartnerName = txtBusinessPartnerName.Text;
            entity.MedicalNo = txtMedicalNo.Text;
            entity.PatientName = txtPatientName.Text;
            entity.TransactionNo = txtTransactionNo.Text;
            if (txtTransactionDate.Text != "")
            {
                entity.TransactionDate = Helper.GetDatePickerValue(txtTransactionDate.Text);
            }
            else
            {
                entity.TransactionDate = null;
            }
            entity.ItemName1 = txtItemName1.Text;
            entity.ChargedQty = Convert.ToDecimal(txtChargedQty.Text);
        }

        private bool OnSaveAddDtRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TransRevenueSharingAdjustmentHdDao entityHdDao = new TransRevenueSharingAdjustmentHdDao(ctx);
            TransRevenueSharingAdjustmentDtDao entityDtDao = new TransRevenueSharingAdjustmentDtDao(ctx);

            try
            {
                int RSAdjustmentID = (hdnRSAdjustmentID.Value == "" || hdnRSAdjustmentID.Value == "0" || hdnRSAdjustmentID.Value == null) ? 0 : Convert.ToInt32(hdnRSAdjustmentID.Value);

                if (RSAdjustmentID == 0)
                {
                    TransRevenueSharingAdjustmentHd entityHd = new TransRevenueSharingAdjustmentHd();
                    entityHd.RSAdjustmentDate = Helper.GetDatePickerValue(txtRSAdjustmentDate.Text);
                    entityHd.RSAdjustmentNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.TRANS_REVENUE_SHARING_ADJUSTMENT_ENTRY, entityHd.RSAdjustmentDate, ctx);
                    entityHd.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
                    entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                    entityHd.CreatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    RSAdjustmentID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);

                    hdnRSAdjustmentID.Value = RSAdjustmentID.ToString();
                    retval = hdnRSAdjustmentNo.Value = entityHd.RSAdjustmentNo;
                }

                TransRevenueSharingAdjustmentDt entityDt = new TransRevenueSharingAdjustmentDt();
                ControlToEntity(entityDt);
                entityDt.CreatedBy = AppSession.UserLogin.UserID;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                entityDtDao.Insert(entityDt);

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

        private bool OnSaveEditDtRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TransRevenueSharingAdjustmentHdDao entityHdDao = new TransRevenueSharingAdjustmentHdDao(ctx);
            TransRevenueSharingAdjustmentDtDao entityDtDao = new TransRevenueSharingAdjustmentDtDao(ctx);

            try
            {
                if (hdnRSAdjustmentID.Value != "" && hdnRSAdjustmentID.Value != "0")
                {
                    TransRevenueSharingAdjustmentHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnRSAdjustmentID.Value));
                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        TransRevenueSharingAdjustmentDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnID.Value));
                        ControlToEntity(entityDt);
                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityDtDao.Update(entityDt);

                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
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

        private bool OnDeleteDtRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TransRevenueSharingAdjustmentHdDao entityHdDao = new TransRevenueSharingAdjustmentHdDao(ctx);
            TransRevenueSharingAdjustmentDtDao entityDtDao = new TransRevenueSharingAdjustmentDtDao(ctx);

            try
            {
                if (hdnRSAdjustmentID.Value != "" && hdnRSAdjustmentID.Value != "0")
                {
                    TransRevenueSharingAdjustmentHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnRSAdjustmentID.Value));
                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        TransRevenueSharingAdjustmentDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnID.Value));
                        entityDt.IsDeleted = true;
                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityDtDao.Update(entityDt);

                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
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