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

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class CoverageTypeItemEntryCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        public override void InitializeDataControl(string param)
        {
            hdnCoverageTypeID.Value = param;
            CoverageType entity = BusinessLayer.GetCoverageType(Convert.ToInt32(hdnCoverageTypeID.Value));
            txtCoverageType.Text = string.Format("{0} - {1}", entity.CoverageTypeCode, entity.CoverageTypeName);

            BindGridView(1, true, ref PageCount);

            txtItemCode.Attributes.Add("validationgroup", "mpEntryPopup");
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("CoverageTypeID = {0} AND IsDeleted = 0", hdnCoverageTypeID.Value);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvCoverageTypeItemRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 8);
            }

            List<vCoverageTypeItem> lstEntity = BusinessLayer.GetvCoverageTypeItemList(filterExpression, 8, pageIndex, "ItemName1 ASC");
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
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

        private void ControlToEntity(CoverageTypeItem entity)
        {
            entity.ItemID = Convert.ToInt32(hdnItemID.Value);

            entity.IsMarkupInPercentage = chkIsMarkupInPercentage.Checked;
            entity.MarkupAmount = txtMarkupAmount.Text == "" ? 0 : Convert.ToDecimal(txtMarkupAmount.Text);
            entity.IsDiscountInPercentage = chkIsDiscountInPercentage.Checked;
            entity.DiscountAmount = txtDiscountAmount.Text == "" ? 0 : Convert.ToDecimal(txtDiscountAmount.Text);
            entity.IsCoverageInPercentage = chkIsCoverageInPercentage.Checked;
            entity.CoverageAmount = txtCoverageAmount.Text == "" ? 0 : Convert.ToDecimal(txtCoverageAmount.Text);
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                CoverageTypeItem entity = new CoverageTypeItem();
                ControlToEntity(entity);
                entity.CoverageTypeID = Convert.ToInt32(hdnCoverageTypeID.Value);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertCoverageTypeItem(entity);
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
                CoverageTypeItem entity = BusinessLayer.GetCoverageTypeItem(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateCoverageTypeItem(entity);
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
                CoverageTypeItem entity = BusinessLayer.GetCoverageTypeItem(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateCoverageTypeItem(entity);
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