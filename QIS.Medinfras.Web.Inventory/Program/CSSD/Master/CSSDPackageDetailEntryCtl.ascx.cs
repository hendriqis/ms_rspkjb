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
    public partial class CSSDPackageDetailEntryCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;

        public override void InitializeDataControl(string param)
        {
            hdnPackageID.Value = param;

            CSSDItemPackageHd cssd = BusinessLayer.GetCSSDItemPackageHd(Convert.ToInt32(param));
            txtPackageCodeCtl.Text = cssd.PackageCode;
            txtPackageNameCtl.Text = cssd.PackageName;

            BindGridView(1, true, ref PageCount);

            txtItemCode.Attributes.Add("validationgroup", "mpEntryPopup");
            txtItemName.Attributes.Add("validationgroup", "mpEntryPopup");
            txtQuantity.Attributes.Add("validationgroup", "mpEntryPopup");
            cboItemUnit.Attributes.Add("validationgroup", "mpEntryPopup");
            chkIsConsumption.Attributes.Add("validationgroup", "mpEntryPopup");
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("PackageID = {0} AND IsDeleted = 0", hdnPackageID.Value);

            if (hdnFilterExpressionQuickSearch.Value != "" && hdnFilterExpressionQuickSearch.Value != null)
            {
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvCSSDItemPackageDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_FIVE);
            }

            List<vCSSDItemPackageDt> lstEntity = BusinessLayer.GetvCSSDItemPackageDtList(filterExpression, Constant.GridViewPageSize.GRID_FIVE, pageIndex, "IsConsumption, ItemName1 ASC");
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

        protected void cboItemUnit_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            if (!string.IsNullOrEmpty(hdnItemID.Value))
            {
                List<StandardCode> lst = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1 AND (StandardCodeID IN (SELECT GCAlternateUnit FROM ItemAlternateUnit WHERE IsDeleted = 0 AND ItemID = {1}) OR StandardCodeID = (SELECT GCItemUnit FROM ItemMaster WHERE ItemID = {1}))", Constant.StandardCode.ITEM_UNIT, hdnItemID.Value));
                Methods.SetComboBoxField<StandardCode>(cboItemUnit, lst, "StandardCodeName", "StandardCodeID");
                cboItemUnit.SelectedIndex = 0;
            }
            else
            {
                cboItemUnit.Items.Clear();
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

                BindGridView(1, true, ref pageCount);
                result += "|" + pageCount;
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(CSSDItemPackageDt entity)
        {
            entity.ItemID = Convert.ToInt32(hdnItemID.Value);
            entity.IsConsumption = chkIsConsumption.Checked;
            entity.Quantity = Convert.ToDecimal(txtQuantity.Text);
            entity.GCItemUnit = cboItemUnit.Value.ToString();
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            CSSDItemPackageDtDao entityDao = new CSSDItemPackageDtDao(ctx);

            try
            {
                CSSDItemPackageDt entity = new CSSDItemPackageDt();
                ControlToEntity(entity);
                entity.PackageID = Convert.ToInt32(hdnPackageID.Value);
                entity.CreatedBy = AppSession.UserLogin.UserID;

                string filter = string.Format("PackageID = {0} AND ItemID = {1} AND IsDeleted = 0", entity.PackageID, entity.ItemID);
                List<CSSDItemPackageDt> lst = BusinessLayer.GetCSSDItemPackageDtList(filter);
                if (lst.Count() == 0)
                {
                    entityDao.Insert(entity);
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Detail paket dengan item ini sudah ada.";
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

        private bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            CSSDItemPackageDtDao entityDao = new CSSDItemPackageDtDao(ctx);

            try
            {
                CSSDItemPackageDt entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.PackageID = Convert.ToInt32(hdnPackageID.Value);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                string filter = string.Format("PackageID = {0} AND ItemID = {1} AND ID != {2} AND IsDeleted = 0", entity.PackageID, entity.ItemID, hdnID.Value);
                List<CSSDItemPackageDt> lst = BusinessLayer.GetCSSDItemPackageDtList(filter);
                if (lst.Count() == 0)
                {
                    entityDao.Update(entity);
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Detail paket dengan item ini sudah ada.";
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

        private bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            CSSDItemPackageDtDao entityDao = new CSSDItemPackageDtDao(ctx);

            try
            {
                CSSDItemPackageDt entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
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