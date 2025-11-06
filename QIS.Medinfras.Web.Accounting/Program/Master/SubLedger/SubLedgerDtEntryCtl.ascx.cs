using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class SubLedgerDtEntryCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        public override void InitializeDataControl(string param)
        {
            hdnSubLedgerID.Value = param;

            SubLedgerHd entity = BusinessLayer.GetSubLedgerHd(Convert.ToInt32(param));
            txtSubLedgerName.Text = string.Format("{0} - {1}", entity.SubLedgerCode, entity.SubLedgerName);

            BindGridView(1, true, ref PageCount);

            Helper.SetControlEntrySetting(txtSubLedgerDtCode, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtSubLedgerDtName, new ControlEntrySetting(true, true, true), "mpEntryPopup");
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("SubLedgerID = {0} AND IsDeleted = 0", hdnSubLedgerID.Value);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetSubLedgerDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 8);
            }

            List<SubLedgerDt> lstEntity = BusinessLayer.GetSubLedgerDtList(filterExpression, 8, pageIndex, "SubLedgerDtCode ASC");
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

        private void ControlToEntity(SubLedgerDt entity)
        {
            entity.SubLedgerDtCode = txtSubLedgerDtCode.Text;
            entity.SubLedgerDtName = txtSubLedgerDtName.Text;
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                SubLedgerDt entity = new SubLedgerDt();
                ControlToEntity(entity);
                entity.SubLedgerID = Convert.ToInt32(hdnSubLedgerID.Value);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertSubLedgerDt(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                SubLedgerDt entity = BusinessLayer.GetSubLedgerDt(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateSubLedgerDt(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            try
            {
                SubLedgerDt entity = BusinessLayer.GetSubLedgerDt(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateSubLedgerDt(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
    }
}