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

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class FAWriteOffEntry : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.FA_WRITE_OFF;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public String GetFAWriteOffDateFilterExpression() 
        {
            DateTime date = Helper.GetDatePickerValue(txtFAWriteOffDate.Text);
            string filterExpression = String.Format("YEAR(DepreciationDate) = {0} AND MONTH(DepreciationDate) = {1}", date.Year, date.Month);
            return filterExpression;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowProposed = IsAllowNextPrev = false;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            hdnFixedAssetID.Value = AppSession.FixedAssetID.ToString();

            string filterExpression = String.Format("ParentID IN ('{0}','{1}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.ASSET_SALES_TYPE, Constant.StandardCode.TIPE_PEMUSNAHAN);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            
            Methods.SetComboBoxField(cboAssetWriteOffType, lstStandardCode.Where(x => x.ParentID == Constant.StandardCode.TIPE_PEMUSNAHAN).ToList(), "StandardCodeName", "StandardCodeID");
            cboAssetWriteOffType.SelectedIndex = 0;

            Methods.SetComboBoxField(cboAssetSalesType, lstStandardCode.Where(x => x.ParentID == Constant.StandardCode.ASSET_SALES_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            cboAssetSalesType.SelectedIndex = 0;

            List<vFAWriteOff> lstEntity = BusinessLayer.GetvFAWriteOffList(String.Format("FixedAssetID = {0} AND GCTransactionStatus = '{1}'", hdnFixedAssetID.Value, Constant.TransactionStatus.APPROVED));
            if (lstEntity.Count > 0)
            {
                EntityToControl(lstEntity[0]);
                IsLoadFirstRecord = true;
            }
            else
            {
                string filterDepreciation = string.Format("FixedAssetID = {0} AND YEAR(DepreciationDate) = '{1}' AND MONTH(DepreciationDate) = '{2}'", hdnFixedAssetID.Value, DateTime.Now.Year, DateTime.Now.Month);
                List<vFADepreciation> lstDepreciation = BusinessLayer.GetvFADepreciationList(filterDepreciation);
                if (lstDepreciation.Count > 0)
                {
                    vFADepreciation oFADepre = lstDepreciation.LastOrDefault();
                    txtProcurementAmount.Text = oFADepre.ProcurementAmount.ToString();
                    txtTotalDepreciationAmount.Text = oFADepre.TotalDepreciationAmount.ToString();
                    txtAssetValue.Text = oFADepre.AssetValue.ToString();
                    txtWriteOffAmount.Text = oFADepre.AssetValue.ToString();
                    txtSelisih.Text = (oFADepre.ProcurementAmount - oFADepre.AssetValue).ToString();
                }
                else
                {
                    string filterFAItem = string.Format("FixedAssetID = {0}", hdnFixedAssetID.Value);
                    List<vFAItem> lstFAItem = BusinessLayer.GetvFAItemList(filterFAItem);
                    vFAItem oFAItem = lstFAItem.LastOrDefault();
                    txtProcurementAmount.Text = oFAItem.ProcurementAmount.ToString();
                    txtTotalDepreciationAmount.Text = oFAItem.ProcurementAmount.ToString();
                    txtAssetValue.Text = oFAItem.AssetFinalValue.ToString();
                    txtWriteOffAmount.Text = "0";
                    txtSelisih.Text = "0";
                }

                IsLoadFirstRecord = false;
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnFAWriteOffID, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtFAWriteOffNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtFAWriteOffDate, new ControlEntrySetting(false, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(cboAssetWriteOffType, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(cboAssetSalesType, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtWriteOffAmount, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(false, false, false));
        }

        private string GetFilterExpression()
        {
            string filterExpression = "";
            if (hdnFAWriteOffID.Value != "")
            {
                filterExpression = string.Format("FAWriteOffID = {0}", hdnFAWriteOffID.Value);
            }
            else
            {
                filterExpression = string.Format("FixedAssetID = {0} AND GCTransactionStatus != '{1}'", hdnFixedAssetID.Value, Constant.TransactionStatus.VOID);
            }
            return filterExpression;
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            vFAWriteOff entity = BusinessLayer.GetvFAWriteOffList(filterExpression).LastOrDefault();
            EntityToControl(entity);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            vFAWriteOff entity = BusinessLayer.GetvFAWriteOffList(filterExpression).LastOrDefault();
            EntityToControl(entity);
        }

        private void EntityToControl(vFAWriteOff entity) 
        {
            txtFAWriteOffNo.Text = entity.FAWriteOffNo;
            hdnFAWriteOffID.Value = entity.FAWriteOffID.ToString();
            txtFAWriteOffDate.Text = entity.FAWriteOffDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            cboAssetWriteOffType.Value = entity.GCAssetWriteOffType;
            cboAssetSalesType.Value = entity.GCAssetSalesType;
            txtProcurementAmount.Text = entity.AssetValue.ToString();
            txtAssetValue.Text = entity.NilaiBuku.ToString();
            txtWriteOffAmount.Text = entity.WriteOffAmount.ToString();
            txtSelisih.Text = entity.Selisih.ToString();
            txtRemarks.Text = entity.Remarks;
            hdnGCTransactionStatus.Value = entity.GCTransactionStatus;

            string filterDepreciation = string.Format("FixedAssetID = {0} AND DepreciationDate < '{1}'", hdnFixedAssetID.Value, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112));
            List<vFADepreciation> lstDepreciation = BusinessLayer.GetvFADepreciationList(filterDepreciation);
            if (lstDepreciation.Count > 0)
            {
                vFADepreciation oFADepre = lstDepreciation.LastOrDefault();
                txtTotalDepreciationAmount.Text = oFADepre.TotalDepreciationAmount.ToString();
            }
            else
            {
                txtTotalDepreciationAmount.Text = entity.AssetValue.ToString();
            }
        }

        private void ControlToEntity(FAWriteOff entity) 
        {
            entity.FixedAssetID = Convert.ToInt32(hdnFixedAssetID.Value);
            entity.FAWriteOffDate = Helper.GetDatePickerValue(txtFAWriteOffDate.Text);
            entity.GCAssetWriteOffType = cboAssetWriteOffType.Value.ToString();
            entity.GCAssetSalesType = cboAssetSalesType.Value.ToString();
            entity.AssetValue = Convert.ToDecimal(Request.Form[txtProcurementAmount.UniqueID]);
            entity.WriteOffAmount = Convert.ToDecimal(txtWriteOffAmount.Text);
            entity.Remarks = txtRemarks.Text;
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            FAWriteOffDao faWriteOffDao = new FAWriteOffDao(ctx);
            FAItemDao faItemDao = new FAItemDao(ctx);

            try
            {
                FAWriteOff faWriteOff = new FAWriteOff();
                ControlToEntity(faWriteOff);
                
                FAItem faItem = faItemDao.Get(faWriteOff.FixedAssetID);
                faItem.GCItemStatus = Constant.ItemStatus.IN_ACTIVE;
                faItem.LastUpdatedBy = AppSession.UserLogin.UserID;
                ctx.CommandType = System.Data.CommandType.Text;
                ctx.Command.Parameters.Clear();
                faItemDao.Update(faItem);

                faWriteOff.AssetValue = faItem.ProcurementAmount;
                faWriteOff.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                faWriteOff.FAWriteOffNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.FIXED_ASSET_WRITE_OFF, faWriteOff.FAWriteOffDate, ctx);
                faWriteOff.CreatedBy = AppSession.UserLogin.UserID;
                ctx.CommandType = System.Data.CommandType.Text;
                ctx.Command.Parameters.Clear();
                hdnFAWriteOffID.Value = faWriteOffDao.InsertReturnPrimaryKeyID(faWriteOff).ToString();

                retval = hdnFAWriteOffID.Value;

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
            FAWriteOffDao faWriteOffDao = new FAWriteOffDao(ctx);
            
            try
            {
                FAWriteOff faWriteOff = faWriteOffDao.Get(Convert.ToInt32(hdnFAWriteOffID.Value));
                faWriteOff.Remarks = txtRemarks.Text;
                faWriteOff.LastUpdatedBy = AppSession.UserLogin.UserID;
                faWriteOffDao.Update(faWriteOff);

                hdnFAWriteOffID.Value = faWriteOff.FAWriteOffID.ToString();

                retval = hdnFAWriteOffID.Value;

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
    }
}