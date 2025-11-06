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
    public partial class CoverageTypeDepartmentClassUploadCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] temp = param.Split('|');

            hdnCoverageTypeID.Value = temp[0];
            hdnDepartmentID.Value = temp[1];

            CoverageTypeUpload entity = BusinessLayer.GetCoverageTypeUpload(Convert.ToInt32(hdnCoverageTypeID.Value));
            txtCoverageType.Text = string.Format("{0} - {1}", entity.CoverageTypeUploadCode, entity.CoverageTypeUploadName);

            Department entity2 = BusinessLayer.GetDepartment(hdnDepartmentID.Value);
            txtDepartment.Text = string.Format("{0} - {1}", entity2.DepartmentID, entity2.DepartmentName);

            BindGridView();

            txtClassCode.Attributes.Add("validationgroup", "mpEntryPopup");
        }

        private void BindGridView()
        {
            lvwView.DataSource = BusinessLayer.GetvCoverageTypeDepartmentClassUploadList(string.Format("CoverageTypeUploadID = {0} AND DepartmentID = '{1}' AND IsDeleted = 0", hdnCoverageTypeID.Value, hdnDepartmentID.Value));
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

            string param = e.Parameter;

            string result = param + "|";
            string errMessage = "";

            if (param == "save")
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
            else if (param == "delete")
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

        private void ControlToEntity(CoverageTypeDepartmentClassUpload entity)
        {
            entity.ClassID = Convert.ToInt32(hdnClassID.Value);

            entity.IsMarkupInPercentage1 = chkIsMarkupInPercentage1.Checked;
            entity.MarkupAmount1 = txtMarkupAmount1.Text == "" ? 0 : Convert.ToDecimal(txtMarkupAmount1.Text);
            entity.IsDiscountInPercentage1 = chkIsDiscountInPercentage1.Checked;
            entity.DiscountAmount1 = txtDiscountAmount1.Text == "" ? 0 : Convert.ToDecimal(txtDiscountAmount1.Text);
            entity.IsDiscountInPercentage1Comp1 = chkIsDiscountInPercentage1Comp1.Checked;
            entity.DiscountAmount1Comp1 = txtDiscountAmount1Comp1.Text == "" ? 0 : Convert.ToDecimal(txtDiscountAmount1Comp1.Text);
            entity.IsDiscountInPercentage1Comp2 = chkIsDiscountInPercentage1Comp2.Checked;
            entity.DiscountAmount1Comp2 = txtDiscountAmount1Comp2.Text == "" ? 0 : Convert.ToDecimal(txtDiscountAmount1Comp2.Text);
            entity.IsDiscountInPercentage1Comp3 = chkIsDiscountInPercentage1Comp3.Checked;
            entity.DiscountAmount1Comp3 = txtDiscountAmount1Comp3.Text == "" ? 0 : Convert.ToDecimal(txtDiscountAmount1Comp3.Text);
            entity.IsCoverageInPercentage1 = chkIsCoverageInPercentage1.Checked;
            entity.CoverageAmount1 = txtCoverageAmount1.Text == "" ? 0 : Convert.ToDecimal(txtCoverageAmount1.Text);
            entity.IsCashBackInPercentage1 = chkIsCashBackInPercentage1.Checked;
            entity.CashBackAmount1 = txtCashBackAmount1.Text == "" ? 0 : Convert.ToDecimal(txtCashBackAmount1.Text);

            entity.IsMarkupInPercentage2 = chkIsMarkupInPercentage2.Checked;
            entity.MarkupAmount2 = txtMarkupAmount2.Text == "" ? 0 : Convert.ToDecimal(txtMarkupAmount2.Text);
            entity.IsDiscountInPercentage2 = chkIsDiscountInPercentage2.Checked;
            entity.DiscountAmount2 = txtDiscountAmount2.Text == "" ? 0 : Convert.ToDecimal(txtDiscountAmount2.Text);
            entity.IsDiscountInPercentage2Comp1 = chkIsDiscountInPercentage2Comp1.Checked;
            entity.DiscountAmount2Comp1 = txtDiscountAmount2Comp1.Text == "" ? 0 : Convert.ToDecimal(txtDiscountAmount2Comp1.Text);
            entity.IsDiscountInPercentage2Comp2 = chkIsDiscountInPercentage2Comp2.Checked;
            entity.DiscountAmount2Comp2 = txtDiscountAmount2Comp2.Text == "" ? 0 : Convert.ToDecimal(txtDiscountAmount2Comp2.Text);
            entity.IsDiscountInPercentage2Comp3 = chkIsDiscountInPercentage2Comp3.Checked;
            entity.DiscountAmount2Comp3 = txtDiscountAmount2Comp3.Text == "" ? 0 : Convert.ToDecimal(txtDiscountAmount2Comp3.Text);
            entity.IsCoverageInPercentage2 = chkIsCoverageInPercentage2.Checked;
            entity.CoverageAmount2 = txtCoverageAmount2.Text == "" ? 0 : Convert.ToDecimal(txtCoverageAmount2.Text);
            entity.IsCashBackInPercentage2 = chkIsCashBackInPercentage2.Checked;
            entity.CashBackAmount2 = txtCashBackAmount2.Text == "" ? 0 : Convert.ToDecimal(txtCashBackAmount2.Text);

            entity.IsMarkupInPercentage3 = chkIsMarkupInPercentage3.Checked;
            entity.MarkupAmount3 = txtMarkupAmount3.Text == "" ? 0 : Convert.ToDecimal(txtMarkupAmount3.Text);
            entity.IsDiscountInPercentage3 = chkIsDiscountInPercentage3.Checked;
            entity.DiscountAmount3 = txtDiscountAmount3.Text == "" ? 0 : Convert.ToDecimal(txtDiscountAmount3.Text);
            entity.IsDiscountInPercentage3Comp1 = chkIsDiscountInPercentage3Comp1.Checked;
            entity.DiscountAmount3Comp1 = txtDiscountAmount3Comp1.Text == "" ? 0 : Convert.ToDecimal(txtDiscountAmount3Comp1.Text);
            entity.IsDiscountInPercentage3Comp2 = chkIsDiscountInPercentage3Comp2.Checked;
            entity.DiscountAmount3Comp2 = txtDiscountAmount3Comp2.Text == "" ? 0 : Convert.ToDecimal(txtDiscountAmount3Comp2.Text);
            entity.IsDiscountInPercentage3Comp3 = chkIsDiscountInPercentage3Comp3.Checked;
            entity.DiscountAmount3Comp3 = txtDiscountAmount3Comp3.Text == "" ? 0 : Convert.ToDecimal(txtDiscountAmount3Comp3.Text);
            entity.IsCoverageInPercentage3 = chkIsCoverageInPercentage3.Checked;
            entity.CoverageAmount3 = txtCoverageAmount3.Text == "" ? 0 : Convert.ToDecimal(txtCoverageAmount3.Text);
            entity.IsCashBackInPercentage3 = chkIsCashBackInPercentage3.Checked;
            entity.CashBackAmount3 = txtCashBackAmount3.Text == "" ? 0 : Convert.ToDecimal(txtCashBackAmount3.Text);
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                CoverageTypeDepartmentClassUpload entity = new CoverageTypeDepartmentClassUpload();
                ControlToEntity(entity);
                entity.CoverageTypeUploadID = Convert.ToInt32(hdnCoverageTypeID.Value);
                entity.DepartmentID = hdnDepartmentID.Value;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertCoverageTypeDepartmentClassUpload(entity);
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
                CoverageTypeDepartmentClassUpload entity = BusinessLayer.GetCoverageTypeDepartmentClassUpload(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateCoverageTypeDepartmentClassUpload(entity);
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
                CoverageTypeDepartmentClassUpload entity = BusinessLayer.GetCoverageTypeDepartmentClassUpload(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateCoverageTypeDepartmentClassUpload(entity);
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