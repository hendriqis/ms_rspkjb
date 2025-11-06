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

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class CoverageTypeItemClassUploadCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] temp = param.Split('|');

            hdnCoverageTypeID.Value = temp[0];
            hdnItemID.Value = temp[1];

            CoverageTypeUpload entity = BusinessLayer.GetCoverageTypeUpload(Convert.ToInt32(hdnCoverageTypeID.Value));
            txtCoverageType.Text = string.Format("{0} - {1}", entity.CoverageTypeUploadCode, entity.CoverageTypeUploadName);

            ItemMaster entity2 = BusinessLayer.GetItemMaster(Convert.ToInt32(hdnItemID.Value));
            txtItem.Text = string.Format("{0} - {1}", entity2.ItemCode, entity2.ItemName1);

            BindGridView();

            txtClassCode.Attributes.Add("validationgroup", "mpEntryPopup");
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("CoverageTypeUploadID = {0} AND ItemID = {1} AND IsDeleted = 0", hdnCoverageTypeID.Value, hdnItemID.Value);
            List<vCoverageTypeItemClassUpload> lstEntity = BusinessLayer.GetvCoverageTypeItemClassUploadList(filterExpression);
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

            string[] param = e.Parameter.Split('|');

            string result = param[0] + "|";
            string errMessage = "";
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

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(CoverageTypeItemClassUpload entity)
        {
            entity.ClassID = Convert.ToInt32(hdnClassID.Value);

            entity.IsMarkupInPercentage = chkIsMarkupInPercentage.Checked;
            entity.MarkupAmount = txtMarkupAmount.Text == "" ? 0 : Convert.ToDecimal(txtMarkupAmount.Text);
            entity.IsDiscountInPercentage = chkIsDiscountInPercentage.Checked;
            entity.DiscountAmount = txtDiscountAmount.Text == "" ? 0 : Convert.ToDecimal(txtDiscountAmount.Text);
            entity.IsDiscountInPercentageComp1 = chkIsDiscountInPercentageComp1.Checked;
            entity.DiscountAmountComp1 = txtDiscountAmountComp1.Text == "" ? 0 : Convert.ToDecimal(txtDiscountAmountComp1.Text);
            entity.IsDiscountInPercentageComp2 = chkIsDiscountInPercentageComp2.Checked;
            entity.DiscountAmountComp2 = txtDiscountAmountComp2.Text == "" ? 0 : Convert.ToDecimal(txtDiscountAmountComp2.Text);
            entity.IsDiscountInPercentageComp3 = chkIsDiscountInPercentageComp3.Checked;
            entity.DiscountAmountComp3 = txtDiscountAmountComp3.Text == "" ? 0 : Convert.ToDecimal(txtDiscountAmountComp3.Text);
            entity.IsCoverageInPercentage = chkIsCoverageInPercentage.Checked;
            entity.CoverageAmount = txtCoverageAmount.Text == "" ? 0 : Convert.ToDecimal(txtCoverageAmount.Text);
            entity.IsCashBackInPercentage = chkIsCashBackInPercentage.Checked;
            entity.CashBackAmount = txtCashBackAmount.Text == "" ? 0 : Convert.ToDecimal(txtCashBackAmount.Text);
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                CoverageTypeItemClassUpload entity = new CoverageTypeItemClassUpload();
                ControlToEntity(entity);
                entity.ItemID = Convert.ToInt32(Request.Form[hdnItemID.UniqueID]);
                entity.CoverageTypeUploadID = Convert.ToInt32(hdnCoverageTypeID.Value);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertCoverageTypeItemClassUpload(entity);
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
                CoverageTypeItemClassUpload entity = BusinessLayer.GetCoverageTypeItemClassUpload(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateCoverageTypeItemClassUpload(entity);
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
                CoverageTypeItemClassUpload entity = BusinessLayer.GetCoverageTypeItemClassUpload(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateCoverageTypeItemClassUpload(entity);
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