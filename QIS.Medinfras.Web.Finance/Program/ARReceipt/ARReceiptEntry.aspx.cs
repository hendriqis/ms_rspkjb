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
    public partial class ARReceiptEntry : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.AR_RECEIPT_PRINT;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            MenuMaster oMenu = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault();
            hdnPageTitle.Value = oMenu.MenuCaption;

            List<SettingParameterDt> setvar = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                    "HealthcareID = '{0}' AND ParameterCode IN ('{1}')",
                                                    AppSession.UserLogin.HealthcareID,
                                                    Constant.SettingParameter.FN_REPORT_CODE_AR_RECEIPT
                                                ));

            hdnReportCode.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.FN_REPORT_CODE_AR_RECEIPT).FirstOrDefault().ParameterValue;
            
            BindGridView();
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnARReceiptID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtReceiptNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtReceiptDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtReceiptTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(hdnBusinessPartnerBillToID, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtBusinessPartnerBillToCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtBusinessPartnerBillToName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(hdnBusinessPartnerVirtualAccountID, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtBankName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtAccountNo, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtPrintAsName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
        }

        protected override void SetControlProperties()
        {
        }

        #region Load Entity
        public override void OnAddRecord()
        {
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
            return BusinessLayer.GetvARReceiptHdRowCount(filterExpression);
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            vARReceiptHd entity = BusinessLayer.GetvARReceiptHd(filterExpression, PageIndex, "ARReceiptID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            PageIndex = BusinessLayer.GetvARReceiptHdRowIndex(filterExpression, keyValue, "ARReceiptID DESC");
            vARReceiptHd entity = BusinessLayer.GetvARReceiptHd(filterExpression, PageIndex, "ARReceiptID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        private void EntityToControl(vARReceiptHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (entity.IsDeleted)
            {
                hdnIsEditable.Value = "0";
                isShowWatermark = true;
                watermarkText = "DELETED";
            }
            else
            {
                hdnIsEditable.Value = "1";
            }

            hdnARReceiptID.Value = entity.ARReceiptID.ToString();
            txtReceiptNo.Text = entity.ARReceiptNo;
            txtReceiptDate.Text = entity.ARReceiptDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtReceiptTime.Text = entity.ARReceiptTime;
            hdnBusinessPartnerBillToID.Value = entity.BusinessPartnerBillToID.ToString();
            txtBusinessPartnerBillToCode.Text = entity.BusinessPartnerBillToCode;
            txtBusinessPartnerBillToName.Text = entity.BusinessPartnerBillToName;
            hdnBusinessPartnerVirtualAccountID.Value = entity.BusinessPartnerVirtualAccountID.ToString();
            txtBankName.Text = entity.BankName;
            txtAccountNo.Text = entity.BankAccountNo;
            txtPrintAsName.Text = entity.PrintAsName;
            txtRemarks.Text = entity.Remarks;
            txtReceiptAmount.Text = entity.ReceiptAmount.ToString(Constant.FormatString.NUMERIC_2);

            hdnPrintNumber.Value = entity.PrintNumber.ToString();
            txtPrintNumber.Text = entity.PrintNumber.ToString();
            txtReprintReason.Text = entity.ReprintReasonWatermark + " " + entity.ReprintReason;
            txtReprintBy.Text = entity.LastUpdatedByName;
            if (entity.LastUpdatedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
            {
                txtReprintDateTime.Text = "";
            }
            else
            {
                txtReprintDateTime.Text = entity.LastUpdatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }


            divCreatedBy.InnerHtml = entity.CreatedByName;
            divCreatedDate.InnerHtml = entity.CreatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            divLastUpdatedBy.InnerHtml = entity.LastUpdatedByName;
            if (entity.LastUpdatedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
            {
                divLastUpdatedDate.InnerHtml = "";
            }
            else
            {
                divLastUpdatedDate.InnerHtml = entity.LastUpdatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }

            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = "IsDeleted = 0";
            if (hdnARReceiptID.Value != "")
            {
                filterExpression = string.Format("ARReceiptID = {0} AND IsDeleted = 0", hdnARReceiptID.Value);
            }

            List<vARReceiptDt> lstEntity = BusinessLayer.GetvARReceiptDtList(filterExpression, int.MaxValue, 1, "ARInvoiceDate, ARInvoiceID");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
        #endregion

        #region Save Header
        public void SaveARReceiptHd(IDbContext ctx, ref int ARReceiptID, ref string ARReceiptNo)
        {
            ARReceiptHdDao entityHdDao = new ARReceiptHdDao(ctx);
            if (hdnARReceiptID.Value == "0")
            {
                ARReceiptHd entityHd = new ARReceiptHd();
                entityHd.ARReceiptDate = Helper.GetDatePickerValue(txtReceiptDate.Text);
                entityHd.ARReceiptTime = txtReceiptTime.Text;
                if(hdnBusinessPartnerBillToID.Value != "" && hdnBusinessPartnerBillToID.Value != "0")
                {
                    entityHd.BusinessPartnerBillToID = Convert.ToInt32(hdnBusinessPartnerBillToID.Value);
                }
                if (hdnBusinessPartnerVirtualAccountID.Value != "" && hdnBusinessPartnerVirtualAccountID.Value != "0")
                {
                    entityHd.BusinessPartnerVirtualAccountID = Convert.ToInt32(hdnBusinessPartnerVirtualAccountID.Value);
                }
                entityHd.PrintAsName = txtPrintAsName.Text;
                entityHd.Remarks = txtRemarks.Text;
                entityHd.ARReceiptNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.AR_RECEIPT, entityHd.ARReceiptDate, ctx);
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                ARReceiptID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
                ARReceiptNo = entityHd.ARReceiptNo;
            }
            else
            {
                ARReceiptID = Convert.ToInt32(hdnARReceiptID.Value);
                ARReceiptNo = txtReceiptNo.Text;
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                int ARReceiptID = 0;
                string ARReceiptNo = "";
                SaveARReceiptHd(ctx, ref ARReceiptID, ref ARReceiptNo);
                retval = ARReceiptNo;
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

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ARReceiptHdDao entityHdDao = new ARReceiptHdDao(ctx);
            try
            {
                ARReceiptHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnARReceiptID.Value));
                if (!entityHd.IsDeleted)
                {
                    if (hdnBusinessPartnerBillToID.Value != "" && hdnBusinessPartnerBillToID.Value != "0")
                    {
                        entityHd.BusinessPartnerBillToID = Convert.ToInt32(hdnBusinessPartnerBillToID.Value);
                    }
                    if (hdnBusinessPartnerVirtualAccountID.Value != "" && hdnBusinessPartnerVirtualAccountID.Value != "0")
                    {
                        entityHd.BusinessPartnerVirtualAccountID = Convert.ToInt32(hdnBusinessPartnerVirtualAccountID.Value);
                    }
                    entityHd.PrintAsName = txtPrintAsName.Text;
                    entityHd.Remarks = txtRemarks.Text;
                    entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entityHd);

                    retval = entityHd.ARReceiptNo;

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = string.Format("Proses kwitansi di nomor " + entityHd.ARReceiptNo + " tidak dapat dilanjutkan. Harap refresh halaman ini.");
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
        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            int ARReceiptID = Convert.ToInt32(hdnARReceiptID.Value);
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "delete")
            {
                if (OnDeleteEntityDt(ref errMessage, ARReceiptID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else if (param[0] == "firstprint")
            {
                if (OnFirstPrint(ref errMessage, ARReceiptID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpARReceiptID"] = ARReceiptID.ToString();
        }

        private bool OnDeleteEntityDt(ref string errMessage, int ARReceiptID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ARReceiptHdDao entityHdDao = new ARReceiptHdDao(ctx);
            ARReceiptDtDao entityDtDao = new ARReceiptDtDao(ctx);
            try
            {
                ARReceiptDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnARReceiptDtID.Value));
                if (!entityDt.IsDeleted)
                {
                    entityDt.IsDeleted = true;
                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Update(entityDt);
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = string.Format("Proses kwitansi di nomor " + entityHdDao.Get(entityDt.ARReceiptID).ARReceiptNo + " tidak dapat dilanjutkan. Harap refresh halaman ini.");
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

        private bool OnFirstPrint(ref string errMessage, int ARReceiptID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ARReceiptHdDao entityHdDao = new ARReceiptHdDao(ctx);
            try
            {
                ARReceiptHd entity = entityHdDao.Get(ARReceiptID);
                if (!entity.IsDeleted)
                {
                    entity.PrintNumber += 1;
                    entity.LastPrintedBy = AppSession.UserLogin.UserID;
                    entity.LastPrintedDate = DateTime.Now;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entity);
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = string.Format("Proses kwitansi di nomor " + entity.ARReceiptNo + " tidak dapat dilanjutkan. Harap refresh halaman ini.");
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