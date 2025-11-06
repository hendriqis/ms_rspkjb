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

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class TestPartnerTransactionEntry : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.TEST_PARTNER_TRANSACTION;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            MenuMaster oMenu = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault();
            hdnPageTitle.Value = oMenu.MenuCaption;

            string moduleName = Helper.GetModuleName();
            string ModuleID = Helper.GetModuleID(moduleName);
            GetUserMenuAccess menu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault();
            string CRUDMode = menu.CRUDMode;
            hdnIsAllowVoid.Value = CRUDMode.Contains("X") ? "1" : "0";

            SetControlProperties();
            BindGridView();

            hdnIsEditable.Value = "1";
        }

        protected string GetVATPercentage()
        {
            return hdnVATPercentage.Value;
        }

        protected override void SetControlProperties()
        {
            List<SettingParameterDt> lstSettingParameter = BusinessLayer.GetSettingParameterDtList(string.Format(
                        "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
                            AppSession.UserLogin.HealthcareID, //0
                            Constant.SettingParameter.VAT_PERCENTAGE, //1
                            Constant.SettingParameter.SA_IS_USED_PRODUCT_LINE, //2
                            Constant.SettingParameter.FN_IS_PPN_ALLOW_CHANGED //3
            ));

            txtTransactionDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            hdnIsUsedProductLine.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA_IS_USED_PRODUCT_LINE).ParameterValue;

            if (hdnIsUsedProductLine.Value == "1")
            {
                trProductLine.Style.Remove("display");
                lblProductLine.Attributes.Add("class", "lblLink lblMandatory");
            }
            else
            {
                trProductLine.Style.Add("display", "none");
                lblProductLine.Attributes.Remove("class");
            }

            string vatPercent = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.VAT_PERCENTAGE).ParameterValue;
            if (vatPercent != "" && vatPercent != null)
            {
                hdnVATPercentage.Value = vatPercent;
                hdnVATPercentageFromSetvar.Value = vatPercent;
            }
            else
            {
                hdnVATPercentage.Value = "0";
                hdnVATPercentageFromSetvar.Value = "0";
            }

            txtVATPct.Text = hdnVATPercentage.Value;

            hdnIsPpnAllowChanged.Value = lstSettingParameter.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.FN_IS_PPN_ALLOW_CHANGED).FirstOrDefault().ParameterValue;

        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnTransactionID, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtTransactionNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblTransactionDate, new ControlEntrySetting(true, false));
            SetControlEntrySetting(txtTransactionDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(lblBusinessPartner, new ControlEntrySetting(true, false));
            SetControlEntrySetting(hdnBusinessPartnerID, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtBusinessPartnerCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtBusinessPartnerName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(lblProductLine, new ControlEntrySetting(true, false));
            SetControlEntrySetting(hdnProductLineID, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtProductLineCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtProductLineName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(hdnGCTransactionStatus, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(txtTotalPartnerAmount, new ControlEntrySetting(false, false, true, "0"));
            SetControlEntrySetting(chkVAT, new ControlEntrySetting(true, true, false, false));
            SetControlEntrySetting(txtVATPct, new ControlEntrySetting(false, false, true, "0"));
            SetControlEntrySetting(txtVATAmount, new ControlEntrySetting(false, false, true, "0"));
            SetControlEntrySetting(chkPPHPercent, new ControlEntrySetting(true, true, false, false));
            SetControlEntrySetting(txtPPHPct, new ControlEntrySetting(false, false, true, "0"));
            SetControlEntrySetting(txtPPHAmount, new ControlEntrySetting(false, false, true, "0"));
            SetControlEntrySetting(txtNettPartnerTransactionAmount, new ControlEntrySetting(false, false, true, "0"));

            Helper.SetControlEntrySetting(hdnPatientChargesDtID, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(hdnPatientChargesID, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(hdnItemID, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(hdnRegistrationID, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtRegistrationNo, new ControlEntrySetting(false, false, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtPatientName, new ControlEntrySetting(false, false, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtItemCode, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtItemName, new ControlEntrySetting(false, false, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtLineAmount, new ControlEntrySetting(false, false, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtPartnerAmount, new ControlEntrySetting(false, false, true), "mpTrxPopup");

            trApprovedBy.Attributes.Add("style", "display:none");
            trApprovedDate.Attributes.Add("style", "display:none");
            trVoidBy.Attributes.Add("style", "display:none");
            trVoidDate.Attributes.Add("style", "display:none");
        }

        #region Load Entity
        public override void OnAddRecord()
        {
            hdnTransactionID.Value = "0";
            hdnIsEditable.Value = "1";
        }

        protected string IsEditable()
        {
            return hdnIsEditable.Value;
        }

        protected string GetFilterExpression()
        {
            return "1=1";
        }

        public override int OnGetRowCount()
        {
            string filterExpression = GetFilterExpression();
            return BusinessLayer.GetvTestPartnerTransactionHdRowCount(filterExpression);
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            vTestPartnerTransactionHd entity = BusinessLayer.GetvTestPartnerTransactionHd(filterExpression, PageIndex, "TransactionID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            PageIndex = BusinessLayer.GetvTestPartnerTransactionHdRowIndex(filterExpression, keyValue, "TransactionID DESC");
            vTestPartnerTransactionHd entity = BusinessLayer.GetvTestPartnerTransactionHd(filterExpression, PageIndex, "TransactionID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        private void EntityToControl(vTestPartnerTransactionHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            hdnGCTransactionStatus.Value = entity.GCTransactionStatus;
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                hdnIsEditable.Value = "0";
                isShowWatermark = true;
                watermarkText = entity.TransactionStatus;

                SetControlEntrySetting(txtRemarks, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(chkVAT, new ControlEntrySetting(true, false, false));
                SetControlEntrySetting(chkPPHPercent, new ControlEntrySetting(true, false, false));
            }
            else
            {
                hdnIsEditable.Value = "1";
            }

            hdnTransactionID.Value = entity.TransactionID.ToString();
            txtTransactionNo.Text = entity.TransactionNo;
            txtTransactionDate.Text = entity.TransactionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            hdnBusinessPartnerID.Value = entity.BusinessPartnerID.ToString();
            txtBusinessPartnerCode.Text = entity.BusinessPartnerCode;
            txtBusinessPartnerName.Text = entity.BusinessPartnerName;

            if (hdnIsUsedProductLine.Value == "1")
            {
                trProductLine.Style.Remove("display");
                lblProductLine.Attributes.Add("class", "lblLink lblMandatory");
            }
            else
            {
                trProductLine.Style.Add("display", "none");
                lblProductLine.Attributes.Remove("class");
            }
            hdnProductLineID.Value = entity.ProductLineID.ToString();
            txtProductLineCode.Text = entity.ProductLineCode;
            txtProductLineName.Text = entity.ProductLineName;

            txtRemarks.Text = entity.Remarks;

            txtTotalPartnerAmount.Text = entity.TotalPartnerAmount.ToString();

            if (hdnIsPpnAllowChanged.Value == "1")
            {
                txtVATPct.ReadOnly = false;

                chkVAT.Enabled = false;
            }
            else
            {
                txtVATPct.ReadOnly = true;

                chkVAT.Enabled = true;
            }

            if (entity.IsIncludeVAT && entity.VATPercentage != 0)
            {
                chkVAT.Checked = true;
            }
            else
            {
                chkVAT.Checked = false;
            }
            txtVATPct.Text = entity.VATPercentage.ToString();

            chkPPHPercent.Checked = entity.IsPPHInPercentage;
            txtPPHPct.Text = entity.PPHPercentage.ToString();
            txtPPHAmount.Text = entity.PPHAmount.ToString();

            txtNettPartnerTransactionAmount.Text = entity.NettPartnerTransactionAmount.ToString();

            BindGridView();

            divCreatedBy.InnerHtml = entity.CreatedByName;
            divCreatedDate.InnerHtml = entity.cfCreatedDateInStringFullFormat;

            if (entity.ApprovedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
            {
                trApprovedBy.Attributes.Add("style", "display:none");
                trApprovedDate.Attributes.Add("style", "display:none");

                divApprovedBy.InnerHtml = "";
                divApprovedDate.InnerHtml = "";
            }
            else
            {
                trApprovedBy.Attributes.Remove("style");
                trApprovedDate.Attributes.Remove("style");

                divApprovedBy.InnerHtml = entity.ApprovedByName;
                divApprovedDate.InnerHtml = entity.ApprovedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }

            if (entity.VoidDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
            {
                trVoidReason.Attributes.Add("style", "display:none");
                trVoidBy.Attributes.Add("style", "display:none");
                trVoidDate.Attributes.Add("style", "display:none");

                divVoidReason.InnerHtml = "";
                divVoidBy.InnerHtml = "";
                divVoidDate.InnerHtml = "";
            }
            else
            {
                trVoidReason.Attributes.Remove("style");
                trVoidBy.Attributes.Remove("style");
                trVoidDate.Attributes.Remove("style");

                divVoidReason.InnerHtml = string.Format("[{0}] {1}", entity.VoidReasonText, entity.VoidReason);
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

        private void BindGridView()
        {
            string filterExpression = "1 = 0";
            if (hdnTransactionID.Value != null && hdnTransactionID.Value != "" && hdnTransactionID.Value != "0")
            {
                filterExpression = string.Format("TransactionID = {0} AND IsDeleted = 0", hdnTransactionID.Value);
            }

            List<vTestPartnerTransactionDt> lstEntity = BusinessLayer.GetvTestPartnerTransactionDtList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            BindGridView();
            result = string.Format("refresh|");

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        #region Save Header

        private void ControlToEntityHd(TestPartnerTransactionHd entityHd)
        {
            entityHd.Remarks = txtRemarks.Text;

            entityHd.IsIncludeVAT = chkVAT.Checked;
            if (entityHd.IsIncludeVAT)
            {
                entityHd.VATPercentage = Convert.ToDecimal(Request.Form[txtVATPct.UniqueID]);
            }
            else
            {
                entityHd.VATPercentage = 0;
            }

            entityHd.IsPPHInPercentage = chkPPHPercent.Checked;
            entityHd.PPHPercentage = Convert.ToDecimal(Request.Form[txtPPHPct.UniqueID]);
            entityHd.PPHAmount = Convert.ToDecimal(Request.Form[txtPPHAmount.UniqueID]);

            entityHd.NettPartnerTransactionAmount = entityHd.TotalPartnerAmount + (entityHd.TotalPartnerAmount * entityHd.VATPercentage / 100) + entityHd.PPHAmount;
        }

        public void SaveTestPartnerTransactionHd(IDbContext ctx, ref int TransactionID, ref string errorMessage)
        {
            TestPartnerTransactionHdDao entityHdDao = new TestPartnerTransactionHdDao(ctx);
            if (hdnTransactionID.Value == null || hdnTransactionID.Value == "" || hdnTransactionID.Value == "0")
            {
                #region Add

                TestPartnerTransactionHd entityHd = new TestPartnerTransactionHd();
                entityHd.TransactionDate = Helper.GetDatePickerValue(Request.Form[txtTransactionDate.UniqueID]);
                entityHd.TransactionNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.TEST_PARTNER_TRANSACTION, entityHd.TransactionDate, ctx);
                entityHd.BusinessPartnerID = Convert.ToInt32(hdnBusinessPartnerID.Value);
                if (hdnProductLineID.Value != null && hdnProductLineID.Value != "" && hdnProductLineID.Value != "0")
                {
                    entityHd.ProductLineID = Convert.ToInt32(hdnProductLineID.Value);
                }

                ControlToEntityHd(entityHd);

                entityHd.VATPercentage = Convert.ToDecimal(Request.Form[txtVATPct.UniqueID]);
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                TransactionID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);

                #endregion
            }
            else
            {
                #region Update

                TestPartnerTransactionHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnTransactionID.Value));

                ControlToEntityHd(entityHd);

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityHdDao.Update(entityHd);
                TransactionID = Convert.ToInt32(hdnTransactionID.Value);

                #endregion
            }
        }

        public void SaveTestPartnerTransactionHdfromDt(IDbContext ctx, ref int TransactionID, ref string errorMessage)
        {
            TestPartnerTransactionHdDao entityHdDao = new TestPartnerTransactionHdDao(ctx);

            TestPartnerTransactionHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnTransactionID.Value));

            if (hdnTransactionID.Value != null && hdnTransactionID.Value != "" && hdnTransactionID.Value != "0")
            {
                entityHd.Remarks = txtRemarks.Text;

                entityHd.IsIncludeVAT = chkVAT.Checked;
                if (entityHd.IsIncludeVAT)
                {
                    entityHd.VATPercentage = Convert.ToDecimal(Request.Form[txtVATPct.UniqueID]);
                }
                else
                {
                    entityHd.VATPercentage = 0;
                }

                entityHd.IsPPHInPercentage = chkPPHPercent.Checked;
                entityHd.PPHPercentage = Convert.ToDecimal(Request.Form[txtPPHPct.UniqueID]);

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityHdDao.Update(entityHd);
                TransactionID = Convert.ToInt32(hdnTransactionID.Value);
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TestPartnerTransactionHdDao entityHdDao = new TestPartnerTransactionHdDao(ctx);
            try
            {
                int TransactionID = 0;
                string errorMessage = "";
                SaveTestPartnerTransactionHd(ctx, ref TransactionID, ref errorMessage);
                if (String.IsNullOrEmpty(errorMessage))
                {
                    retval = TransactionID.ToString();
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
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TestPartnerTransactionHdDao entityHdDao = new TestPartnerTransactionHdDao(ctx);
            try
            {
                if (hdnTransactionID.Value != null && hdnTransactionID.Value != "" && hdnTransactionID.Value != "0")
                {
                    TestPartnerTransactionHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnTransactionID.Value));
                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        ControlToEntityHd(entityHd);

                        entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityHdDao.Update(entityHd);

                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = string.Format("Transaksi dgn nomor {0} tidak dapat diubah karena statusnya tidak open lagi.", entityHd.TransactionNo);
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Transaksi tidak ditemukan, silahkan pilih ulang.";
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

        protected override bool OnApproveRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TestPartnerTransactionHdDao entityHdDao = new TestPartnerTransactionHdDao(ctx);
            try
            {
                if (hdnTransactionID.Value != null && hdnTransactionID.Value != "" && hdnTransactionID.Value != "0")
                {
                    TestPartnerTransactionHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnTransactionID.Value));
                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        ControlToEntityHd(entityHd);

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
                        errMessage = string.Format("Transaksi dgn nomor {0} tidak dapat diubah karena statusnya tidak open lagi.", entityHd.TransactionNo);
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Transaksi tidak ditemukan, silahkan pilih ulang.";
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

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TestPartnerTransactionHdDao entityHdDao = new TestPartnerTransactionHdDao(ctx);
            try
            {
                if (hdnTransactionID.Value != null && hdnTransactionID.Value != "" && hdnTransactionID.Value != "0")
                {
                    TestPartnerTransactionHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnTransactionID.Value));
                    if (type.Contains("voidbyreason"))
                    {
                        if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                        {
                            string[] param = type.Split(';');
                            string gcDeleteReason = param[1];
                            string reason = param[2];
                            entityHd.GCVoidReason = gcDeleteReason;
                            entityHd.VoidReason = reason;
                            entityHd.VoidBy = AppSession.UserLogin.UserID;
                            entityHd.VoidDate = DateTime.Now;

                            ControlToEntityHd(entityHd);

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
                            errMessage = string.Format("Transaksi dgn nomor {0} tidak dapat diubah karena statusnya tidak open lagi.", entityHd.TransactionNo);
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Transaksi tidak ditemukan, silahkan pilih ulang.";
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

        #region Process Detail
        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            int TestPartnerTransactionID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                if (hdnDtID.Value.ToString() != "")
                {
                    TestPartnerTransactionID = Convert.ToInt32(hdnTransactionID.Value);
                    if (OnSaveEditRecordEntityDt(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecordEntityDt(ref errMessage, ref TestPartnerTransactionID))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "delete")
            {
                TestPartnerTransactionID = Convert.ToInt32(hdnTransactionID.Value);
                if (OnDeleteEntityDt(ref errMessage, TestPartnerTransactionID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTestPartnerTransactionID"] = TestPartnerTransactionID.ToString();
        }

        private void ControlToEntity(TestPartnerTransactionDt entityDt)
        {
            entityDt.RegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
            entityDt.PatientChargesID = Convert.ToInt32(hdnPatientChargesID.Value);
            entityDt.PatientChargesDtID = Convert.ToInt32(hdnPatientChargesDtID.Value);
            entityDt.ItemID = Convert.ToInt32(hdnItemID.Value);
            entityDt.LineAmount = Convert.ToDecimal(Request.Form[txtLineAmount.UniqueID]);
            entityDt.PartnerAmount = Convert.ToDecimal(Request.Form[txtPartnerAmount.UniqueID]);
        }

        private bool OnSaveAddRecordEntityDt(ref string errMessage, ref int TestPartnerTransactionID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TestPartnerTransactionHdDao entityHdDao = new TestPartnerTransactionHdDao(ctx);
            TestPartnerTransactionDtDao entityDtDao = new TestPartnerTransactionDtDao(ctx);

            try
            {
                string errorMessage = "";
                SaveTestPartnerTransactionHdfromDt(ctx, ref TestPartnerTransactionID, ref errorMessage);
                if (String.IsNullOrEmpty(errorMessage))
                {
                    if (entityHdDao.Get(TestPartnerTransactionID).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        TestPartnerTransactionDt entityDt = new TestPartnerTransactionDt();
                        ControlToEntity(entityDt);
                        entityDt.TransactionID = TestPartnerTransactionID;
                        entityDt.CreatedBy = AppSession.UserLogin.UserID;
                        int oID = entityDtDao.InsertReturnPrimaryKeyID(entityDt);

                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = string.Format("Transaksi dgn nomor {0} tidak dapat diubah karena statusnya tidak open lagi.", entityHdDao.Get(TestPartnerTransactionID).TransactionNo);
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

        private bool OnSaveEditRecordEntityDt(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TestPartnerTransactionHdDao entityHdDao = new TestPartnerTransactionHdDao(ctx);
            TestPartnerTransactionDtDao entityDtDao = new TestPartnerTransactionDtDao(ctx);

            try
            {
                TestPartnerTransactionDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnDtID.Value));
                TestPartnerTransactionHd entityHd = entityHdDao.Get(entityDt.TransactionID);
                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN && entityDt.IsDeleted == false)
                {
                    ControlToEntity(entityDt);
                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Update(entityDt);
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = string.Format("Transaksi dgn nomor {0} tidak dapat diubah karena statusnya tidak open lagi.", entityHd.TransactionNo);
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

        private bool OnDeleteEntityDt(ref string errMessage, int ID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            TestPartnerTransactionHdDao entityHdDao = new TestPartnerTransactionHdDao(ctx);
            TestPartnerTransactionDtDao entityDtDao = new TestPartnerTransactionDtDao(ctx);

            try
            {
                TestPartnerTransactionDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnDtID.Value));
                TestPartnerTransactionHd entityHd = entityHdDao.Get(entityDt.TransactionID);
                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN && entityDt.IsDeleted == false)
                {
                    entityDt.IsDeleted = true;
                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Update(entityDt);
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = string.Format("Transaksi dgn nomor {0} tidak dapat diubah karena statusnya tidak open lagi.", entityHd.TransactionNo);
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