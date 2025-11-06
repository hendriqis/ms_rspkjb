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

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class ItemAlternateUnitCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;

        public override void InitializeDataControl(string param)
        {
            string[] paramLst = param.Split('|');
            hdnItemIDCtlAlternate.Value = paramLst[0];
            hdnDrugDetailTypeCtlAlternate.Value = paramLst[1];

            ItemMaster im = BusinessLayer.GetItemMaster(Convert.ToInt32(hdnItemIDCtlAlternate.Value));
            txtItemCodeCtlAlternate.Text = im.ItemCode;
            txtItemNameCtlAlternate.Text = im.ItemName1;

            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND StandardCodeID NOT IN ('{1}') AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.ITEM_UNIT, im.GCItemUnit));
            lstSc.Add(new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboGCAlternateUnit, lstSc, "StandardCodeName", "StandardCodeID");
            cboGCAlternateUnit.Value = "";

            BindGridView();

        }

        private void BindGridView()
        {
            string filterExpression = string.Format("ItemID = {0} AND IsDeleted = 0 ORDER BY ItemID, GCAlternateUnit", hdnItemIDCtlAlternate.Value);

            List<vItemAlternateUnit> lstEntity = BusinessLayer.GetvItemAlternateUnitList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            int pageCount = 1;

            string[] param = e.Parameter.Split('|');

            string result = param[0] + "|";
            string errMessage = "";

            if (param[0] == "refresh")
            {
                BindGridView();
                result = string.Format("refresh|{0}", pageCount);
            }
            else if (param[0] == "save")
            {
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
            else if (param[0] == "delete")
            {
                if (OnDeleteRecord(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(ItemAlternateUnit entity)
        {
            entity.ItemID = Convert.ToInt32(hdnItemIDCtlAlternate.Value);
            entity.GCAlternateUnit = cboGCAlternateUnit.Value.ToString();
            entity.ConversionFactor = Convert.ToDecimal(txtConversionFactor.Text);
            entity.IsActive = chkIsActive.Checked;
            entity.IsDeleted = false;
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ItemAlternateUnitDao entityDao = new ItemAlternateUnitDao(ctx);
            try
            {
                ItemAlternateUnit entity = new ItemAlternateUnit();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;

                string filter = string.Format("ItemID = {0} AND GCAlternateUnit = '{1}' AND IsDeleted = 0", entity.ItemID, entity.GCAlternateUnit);
                List<ItemAlternateUnit> lst = BusinessLayer.GetItemAlternateUnitList(filter, ctx);
                if (lst.Count() == 0)
                {
                    entityDao.Insert(entity);
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Satuan Alternatif dengan satuan ini sudah tersedia";
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

        private bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ItemAlternateUnitDao entityDao = new ItemAlternateUnitDao(ctx);
            try
            {
                ItemAlternateUnit entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                string filter = string.Format("ItemID = {0} AND GCAlternateUnit = '{1}' AND ID != {2} AND IsDeleted = 0", entity.ItemID, entity.GCAlternateUnit, hdnID.Value);
                List<ItemAlternateUnit> lst = BusinessLayer.GetItemAlternateUnitList(filter, ctx);
                if (lst.Count() == 0)
                {
                    entityDao.Update(entity);
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Satuan Alternatif dengan satuan ini sudah tersedia";
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

        private bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ItemAlternateUnitDao entityDao = new ItemAlternateUnitDao(ctx);
            try
            {
                ItemAlternateUnit entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
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
            finally
            {
                ctx.Close();
            }
            return result;
        }
    }
}