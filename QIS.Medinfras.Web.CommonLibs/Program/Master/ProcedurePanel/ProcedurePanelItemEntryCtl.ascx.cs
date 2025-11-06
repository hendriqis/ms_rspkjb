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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ProcedurePanelItemEntryCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;

        public override void InitializeDataControl(string param)
        {
            hdnProcedureID.Value = param;

            BindGridView(1, true, ref PageCount);

            Helper.SetControlEntrySetting(txtItemCode, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtFormulaPercentage, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtDisplayOrder, new ControlEntrySetting(true, true, true), "mpEntryPopup");
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("ProcedurePanelID = {0} AND IsDeleted = 0", hdnProcedureID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvProcedurePanelDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 8);
            }

            List<vProcedurePanelDt> lstEntity = BusinessLayer.GetvProcedurePanelDtList(filterExpression, 8, pageIndex, "DisplayOrder ASC");
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

        private void ControlToEntity(ProcedurePanelDt entity)
        {
            entity.ItemID = Convert.ToInt32(hdnItemID.Value);
            entity.ProcedurePanelID = Convert.ToInt32(hdnProcedureID.Value);
            entity.FormulaPercentage = Convert.ToDecimal(txtFormulaPercentage.Text);
            entity.DisplayOrder = Convert.ToInt16(txtDisplayOrder.Text);
            entity.IsControlItem = chkIsControlItem.Checked;
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ProcedurePanelDtDao entityDao = new ProcedurePanelDtDao(ctx);
            try
            {
                if (hdnIsUpdateControlItem.Value == "1")
                {
                    string filterIsControlItem = string.Format("IsDeleted = 0 AND IsControlItem = 1 AND ProcedurePanelID = {0}", hdnProcedureID.Value);
                    ProcedurePanelDt updateDt = BusinessLayer.GetProcedurePanelDtList(filterIsControlItem, ctx).FirstOrDefault();
                    updateDt.IsControlItem = false;
                    updateDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDao.Update(updateDt);

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                }

                ProcedurePanelDt entity = new ProcedurePanelDt();
                entity.ItemID = Convert.ToInt32(hdnItemID.Value);
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);

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

        private bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ProcedurePanelDtDao entityDao = new ProcedurePanelDtDao(ctx);
            try
            {
                if (hdnIsUpdateControlItem.Value == "1")
                {
                    string filterIsControlItem = string.Format("IsDeleted = 0 AND IsControlItem = 1 AND ProcedurePanelID = {0} AND ID != {0}", hdnProcedureID.Value, hdnID.Value);
                    ProcedurePanelDt updateDt = BusinessLayer.GetProcedurePanelDtList(filterIsControlItem, ctx).FirstOrDefault();
                    updateDt.IsControlItem = false;
                    updateDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDao.Update(updateDt);

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                }

                ProcedurePanelDt entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
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

        private bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ProcedurePanelDtDao entityDao = new ProcedurePanelDtDao(ctx);
            try
            {
                ProcedurePanelDt entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
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