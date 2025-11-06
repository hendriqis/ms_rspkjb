using System;
using System.Data;
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
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class EstimatedCharges : BasePageTrx
    {
        protected int PageCount = 1;

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inpatient.IP_ESTIMATED_CHARGES;
        }

        protected override void InitializeDataControl()
        {
            hdnOutPatientID.Value = BusinessLayer.GetSettingParameter(Constant.SettingParameter.OUTPATIENT_CLASS).ParameterValue;
            SetControlProperties();

            decimal tempTransactionAmount = -1;
            BindGridView(1, true, ref PageCount, ref tempTransactionAmount);
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected string IsEditable()
        {
            return hdnIsEditable.Value;
        }

        public override void OnAddRecord()
        {
            hdnPageCount.Value = "0";
            hdnIsEditable.Value = "1";
            hdnTransactionID.Value = "";
            hdnRegistrationID.Value = "";
            hdnBusinessPartnerID.Value = "";
            hdnCoverageTypeID.Value = "";

            txtRegistrationNo.Text = "";
            txtBusinessPartnerCode.Text = "";
            txtCoverageTypeCode.Text = "";

            cboCustomerType.Enabled = true;
            cboClass.Enabled = true;
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> lstSC = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.CUSTOMER_TYPE));
            Methods.SetComboBoxField(cboCustomerType, lstSC.Where(p => p.ParentID == Constant.StandardCode.CUSTOMER_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            cboCustomerType.Value = Constant.CustomerType.PERSONAL;

            List<ClassCare> lstClass = BusinessLayer.GetClassCareList("IsDeleted = 0");
            Methods.SetComboBoxField(cboClass, lstClass, "ClassName", "ClassID");
            cboClass.Value = hdnOutPatientID.Value;
            txtTransactionDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnRegistrationID, new ControlEntrySetting(true, false, true, ""));
            SetControlEntrySetting(hdnBusinessPartnerID, new ControlEntrySetting(true, false, false, ""));
            SetControlEntrySetting(hdnCoverageTypeID, new ControlEntrySetting(true, false, false, ""));

            SetControlEntrySetting(txtRegistrationNo, new ControlEntrySetting(true, false, true, ""));
            SetControlEntrySetting(txtBusinessPartnerCode, new ControlEntrySetting(true, false, false, ""));
            SetControlEntrySetting(txtCoverageTypeCode, new ControlEntrySetting(true, false, false, ""));
            SetControlEntrySetting(txtTransactionDate, new ControlEntrySetting(true, false, false, ""));
            SetControlEntrySetting(txtTransactionDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            vEstimatedChargesHd entity = BusinessLayer.GetvEstimatedChargesHd(filterExpression, PageIndex, "ID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            PageIndex = BusinessLayer.GetvEstimatedChargesHdRowIndex(filterExpression, keyValue, "ID DESC");
            vEstimatedChargesHd entity = BusinessLayer.GetvEstimatedChargesHd(filterExpression, PageIndex, "ID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        public override int OnGetRowCount()
        {
            string filterExpression = GetFilterExpression();
            return BusinessLayer.GetvEstimatedChargesHdRowCount(filterExpression);
        }

        private void EntityToControl(vEstimatedChargesHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            cboCustomerType.Enabled = false;
            cboClass.Enabled = false;

            SetControlEntrySetting(txtRegistrationNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtBusinessPartnerCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtCoverageTypeCode, new ControlEntrySetting(false, false, false));

            txtTransactionDate.Enabled = false;

            lblRegNo.Attributes.Add("class", "lblDisabled lblMandatory");
            lblBusinessPartner.Attributes.Add("class", "lblDisabled");
            lblCoverageType.Attributes.Add("class", "lblDisabled");

            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                hdnIsEditable.Value = "0";
                isShowWatermark = true;
                watermarkText = entity.TransactionStatusWatermark;
            }
            else
            {
                hdnIsEditable.Value = "1";
            }

            hdnTransactionID.Value = entity.ID.ToString();
            txtTransactionNo.Text = entity.TransactionNo;
            txtRegistrationNo.Text = entity.RegistrationNo;
            txtMedicalNo.Text = entity.MedicalNo;
            txtPatientName.Text = entity.PatientName;
            txtServiceUnitName.Text = entity.ServiceUnitName;
            cboCustomerType.Value = entity.GCCustomerType;
            hdnBusinessPartnerID.Value = entity.BusinessPartnerID.ToString();
            txtBusinessPartnerCode.Text = entity.BusinessPartnerCode;
            txtBusinessPartnerName.Text = entity.BusinessPartnerName;
            hdnCoverageTypeID.Value = entity.CoverageTypeID.ToString();
            txtCoverageTypeCode.Text = entity.CoverageTypeCode;
            txtCoverageTypeName.Text = entity.CoverageTypeName;

            List<ClassCare> lstClass = BusinessLayer.GetClassCareList(string.Format("IsUsedInChargeClass = 1 AND IsDeleted = 0 AND ClassID = {0}", entity.ClassID));
            Methods.SetComboBoxField(cboClass, lstClass, "ClassName", "ClassID");
            cboClass.SelectedIndex = 0;

            txtTransactionDate.Text = entity.TransactionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            divCreatedBy.InnerHtml = entity.CreatedByFullName;
            divCreatedDate.InnerHtml = entity.CreatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            divLastUpdatedBy.InnerHtml = entity.LastUpdateByFullName;
            if (entity.LastUpdatedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
            {
                divLastUpdatedDate.InnerHtml = "";
            }
            else
            {
                divLastUpdatedDate.InnerHtml = entity.LastUpdatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }

            decimal tempTransactionAmount = -1;
            BindGridView(1, true, ref PageCount, ref tempTransactionAmount);
            hdnPageCount.Value = PageCount.ToString();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            decimal transactionAmount = 0;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount, ref transactionAmount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridView(1, true, ref pageCount, ref transactionAmount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    ItemMaster entity = e.Row.DataItem as ItemMaster;
            //    CheckBox chkIsSelected = e.Row.FindControl("chkIsSelected") as CheckBox;
            //    if (lstSelectedMember.Contains(entity.ItemID.ToString()))
            //        chkIsSelected.Checked = true;
            //}
        }

        private string GetFilterExpression()
        {
            return "TransactionNo != ''";
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            EstimatedChargesHdDao entityHdDao = new EstimatedChargesHdDao();
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                EstimatedChargesHd entity = new EstimatedChargesHd();
                entity.TransactionDate = DateTime.Now;
                entity.TransactionNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.ESTIMATED_CHARGES, entity.TransactionDate, ctx);
                entity.RegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
                entity.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                entity.GCCustomerType = cboCustomerType.Value.ToString();
                entity.BusinessPartnerID = Convert.ToInt32(hdnBusinessPartnerID.Value);

                if (!String.IsNullOrEmpty(hdnCoverageTypeID.Value))
                {
                    entity.CoverageTypeID = Convert.ToInt32(hdnCoverageTypeID.Value);
                }

                entity.ClassID = Convert.ToInt32(cboClass.Value);
                entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                entity.CreatedBy = AppSession.UserLogin.UserID;

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                int id = entityHdDao.InsertReturnPrimaryKeyID(entity);
                retval = entity.TransactionNo;

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
            try
            {
                result = true;
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
            EstimatedChargesHdDao entityHdDao = new EstimatedChargesHdDao(ctx);
            EstimatedChargesDtDao entityDtDao = new EstimatedChargesDtDao(ctx);

            try
            {
                EstimatedChargesHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnTransactionID.Value));
                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entityHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                    entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entityHd);

                    ctx.CommitTransaction();
                }
                else
                {
                    errMessage = "Transaksi tidak bisa diubah. Harap untuk merefresh halaman ini";
                    result = false;
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

        protected override bool OnVoidRecord(ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            EstimatedChargesHdDao entityHdDao = new EstimatedChargesHdDao(ctx);
            EstimatedChargesDtDao entityDtDao = new EstimatedChargesDtDao(ctx);
            try
            {
                EstimatedChargesHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnTransactionID.Value));
                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entityHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                    entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entityHd);
                    ctx.CommitTransaction();
                    return true;
                }
                else
                {
                    errMessage = "Transaksi tidak bisa diubah. Harap untuk merefresh halaman ini";
                    ctx.RollBackTransaction();
                    return false;
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
                return false;
            }
            finally
            {
                ctx.Close();
            }
        }

        #region Process Detail
        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            int ID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                ID = Convert.ToInt32(hdnID.Value);
                if (hdnID.Value.ToString() != "")
                {
                    if (OnSaveEditRecordEntityDt(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "delete")
            {
                ID = Convert.ToInt32(hdnID.Value);
                if (OnDeleteEntityDt(ref errMessage, ID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpOrderID"] = hdnID.ToString();
        }

        private bool OnSaveEditRecordEntityDt(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            EstimatedChargesHdDao entityHdDao = new EstimatedChargesHdDao(ctx);
            EstimatedChargesDtDao entityDtDao = new EstimatedChargesDtDao(ctx);
            try
            {
                EstimatedChargesDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnID.Value));
                if (entityHdDao.Get(entityDt.EstimatedChargesHdID).GCTransactionStatus == Constant.TransactionStatus.OPEN && entityDt.IsDeleted == false)
                {
                    Decimal payerAmountPerOne = entityDt.PayerAmount / entityDt.Qty;
                    Decimal patientAmountPerOne = entityDt.PatientAmount / entityDt.Qty;

                    entityDt.Qty = Convert.ToDecimal(txtQuantity.Text);
                    entityDt.PayerAmount = payerAmountPerOne * entityDt.Qty;
                    entityDt.PatientAmount = patientAmountPerOne * entityDt.Qty;
                    entityDt.LineAmount = entityDt.PatientAmount + entityDt.PayerAmount;
                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;

                    entityDtDao.Update(entityDt);
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Transaksi tidak bisa diubah. Harap untuk merefresh halaman ini";
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
            EstimatedChargesHdDao entityHdDao = new EstimatedChargesHdDao(ctx);
            EstimatedChargesDtDao entityDtDao = new EstimatedChargesDtDao(ctx);

            try
            {
                EstimatedChargesDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnID.Value));
                if (entityHdDao.Get(entityDt.EstimatedChargesHdID).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entityDt.IsDeleted = true;
                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Update(entityDt);
                    ctx.CommitTransaction();
                }
                else
                {
                    ctx.RollBackTransaction();
                    errMessage = "Transaksi tidak bisa diubah. Harap untuk merefresh halaman ini";
                    result = false;
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

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount, ref decimal transactionAmount)
        {
            string filterExpression = "1 = 0";
            if (hdnTransactionID.Value != "" && hdnTransactionID.Value != "0")
                filterExpression = string.Format("EstimatedChargesHdID = {0} AND IsDeleted = 0", hdnTransactionID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvEstimatedChargesDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vEstimatedChargesDt> lstEntity = BusinessLayer.GetvEstimatedChargesDtList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ItemName1 ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
    }
}