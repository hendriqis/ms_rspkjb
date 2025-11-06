using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class TemplateChargesEntryItemCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                string[] lstParam = param.Split('|');
                string chargesID = lstParam[0].ToString();
                hdnChargesTemplateID.Value = chargesID;
                hdnParamItemType.Value = lstParam[1].ToString();
                vChargesTemplateHd entity = BusinessLayer.GetvChargesTemplateHdList(string.Format("ChargesTemplateID = {0}", hdnChargesTemplateID.Value)).FirstOrDefault();
                txtChargesTemplateName.Text = string.Format("{0} - {1}", entity.ChargesTemplateCode, entity.ChargesTemplateName);
                hdnHealthcareServiceUnitID.Value = Convert.ToString(entity.HealthcareServiceUnitID);
                vHealthcareServiceUnit vsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", entity.HealthcareServiceUnitID)).FirstOrDefault();
                hdnLocationID.Value = Convert.ToString(vsu.LocationID);
                hdnLogisticLocationID.Value = Convert.ToString(vsu.LogisticLocationID);
                SetControlProperties();
                BindGridView();
            }
        }

        private void SetControlProperties()
        {
            Helper.SetControlEntrySetting(txtItemCode, new ControlEntrySetting(true, true, true), "mpTemplateCharges");
            Helper.SetControlEntrySetting(txtItemName, new ControlEntrySetting(false, false, true), "mpTemplateCharges");
            Helper.SetControlEntrySetting(txtQty, new ControlEntrySetting(true, true, true), "mpTemplateCharges");
        }

        #region Popup Filter Expression
        protected string onGetItemMasterFilter()
        {
            string result = "";
            if (hdnParamItemType.Value == "service")
            {
                result = string.Format("ItemID NOT IN (SELECT ItemID FROM ChargesTemplateDt WHERE ChargesTemplateID = {0}) AND ItemID IN (SELECT ItemID FROM ServiceUnitItem WHERE HealthcareServiceUnitID = {1}) AND GCItemStatus != '{2}' AND IsDeleted = 0", hdnChargesTemplateID.Value, hdnHealthcareServiceUnitID.Value, Constant.ItemStatus.IN_ACTIVE);
            }
            else if (hdnParamItemType.Value == "drug")
            {
                result = string.Format("ItemID NOT IN (SELECT ItemID FROM ChargesTemplateDt WHERE ChargesTemplateID = {0}) AND ItemID IN (SELECT ItemID FROM vItemBalanceQuickPick WHERE LocationID = '{1}' AND IsDeleted = 0 AND GCItemStatus != '{2}' AND GCItemType IN ('{3}','{4}') AND IsDeleted = 0) AND IsDeleted = 0 AND GCItemStatus != '{2}'", hdnChargesTemplateID.Value, hdnLocationID.Value, Constant.ItemStatus.IN_ACTIVE, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS);
            }
            else
            {
                result = string.Format("ItemID NOT IN (SELECT ItemID FROM ChargesTemplateDt WHERE ChargesTemplateID = {0}) AND ItemID IN (SELECT ItemID FROM vItemBalance WHERE LocationID = {2} AND GCItemType = '{3}' AND IsDeleted = 0 AND IsChargeToPatient = 1 AND QuantityEND > 0 AND GCItemStatus != '{4}')", hdnChargesTemplateID.Value, hdnHealthcareServiceUnitID.Value, hdnLogisticLocationID.Value, Constant.ItemType.BARANG_UMUM, Constant.ItemStatus.IN_ACTIVE);
            }
            return result;
        }
        #endregion

        private void BindGridView()
        {
            List<vChargesTemplateDt> lstEntity = new List<vChargesTemplateDt>();
            if (hdnParamItemType.Value == "service")
            {
                lstEntity = BusinessLayer.GetvChargesTemplateDtList(string.Format(
                    "ChargesTemplateID = {0} AND ItemID IN (SELECT ItemID FROM ServiceUnitItem WHERE HealthcareServiceUnitID = {1}) AND IsDeleted = 0 ORDER BY ItemName1 ASC",
                    hdnChargesTemplateID.Value, hdnHealthcareServiceUnitID.Value));
            }
            else if (hdnParamItemType.Value == "drug")
            {
                lstEntity = BusinessLayer.GetvChargesTemplateDtList(string.Format(
                    "ChargesTemplateID = {0} AND DetailGCItemType IN ('{1}','{2}') AND IsDeleted = 0 ORDER BY ItemName1 ASC",
                    hdnChargesTemplateID.Value, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS));
            }
            else
            {
                lstEntity = BusinessLayer.GetvChargesTemplateDtList(string.Format(
                    "ChargesTemplateID = {0} AND DetailGCItemType IN ('{1}') AND IsDeleted = 0 ORDER BY ItemName1 ASC",
                    hdnChargesTemplateID.Value, Constant.ItemType.BARANG_UMUM));
            }

            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpTemplateChargesCtl_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            string result = "";
            string errMessage = "";
            if (e.Parameter == "save")
            {
                result = "save|";
                if (hdnID.Value.ToString() != "")
                {
                    if (OnSaveEditRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (e.Parameter == "refresh")
            {
                result += "success";
                BindGridView();
            }
            else
            {
                result = "delete|";
                if (OnDeleteRecord(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(ChargesTemplateDt entity)
        {
            entity.ItemID = Convert.ToInt32(hdnItemID.Value);
            entity.Quantity = Convert.ToDecimal(txtQty.Text);
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ChargesTemplateDtDao entityDao = new ChargesTemplateDtDao(ctx);
            try
            {
                ChargesTemplateDt entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                if (!entity.IsDeleted)
                {
                    string filterExp = string.Format("ChargesTemplateID = {0} AND ItemID = {1} AND IsDeleted = 0 AND ID != {2}", entity.ChargesTemplateID, entity.ItemID, entity.ID);
                    List<ChargesTemplateDt> lstEntity = BusinessLayer.GetChargesTemplateDtList(filterExp, ctx);
                    if (lstEntity.Count <= 0)
                    {
                        ControlToEntity(entity);
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDao.Update(entity);
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Maaf, Item ini sudah ada. Harap Refresh Halaman Ini.";
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Harap Refresh Halaman Ini.";
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
            return result;
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ChargesTemplateDtDao entityDao = new ChargesTemplateDtDao(ctx);
            try
            {
                ChargesTemplateDt entity = new ChargesTemplateDt();
                entity.ChargesTemplateID = Convert.ToInt32(hdnChargesTemplateID.Value);
                if (!entity.IsDeleted)
                {
                    ControlToEntity(entity);
                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    entityDao.Insert(entity);
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Harap Refresh Halaman Ini.";
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
            return result;
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ChargesTemplateDtDao entityDao = new ChargesTemplateDtDao(ctx);
            try
            {
                ChargesTemplateDt entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            return result;
        }
    }
}