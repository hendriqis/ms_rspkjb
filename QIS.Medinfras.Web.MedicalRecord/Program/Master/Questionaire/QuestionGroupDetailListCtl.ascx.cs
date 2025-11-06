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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class QuestionGroupDetailListCtl : BaseViewPopupCtl
    {

        protected int PageCount = 1;

        public override void InitializeDataControl(string param)
        {
            hdnParentItemGroupID.Value = param;
            vItemGroupMaster entity = BusinessLayer.GetvItemGroupMasterList(string.Format("ItemGroupID = '{0}'", hdnParentItemGroupID.Value)).FirstOrDefault();
            txtParentItemGroupCode.Text = entity.ItemGroupCode;
            txtParentItemGroupName.Text = entity.ItemGroupName1;
            hdnGCItemType.Value = entity.GCItemType;
            if (hdnGCItemType.Value.Equals(Constant.ItemGroupMaster.SERVICE))
            {
                tdItemService.Style.Remove("visibility");
                hdnGCItemType.Value = entity.GCItemType;
                hdnParentCito.Value = entity.IsCitoInPercentage.ToString();
                hdnParentComplication.Value = entity.IsComplicationInPercentage.ToString();
                hdnPrintDoctorName.Value = entity.IsPrintWithDoctorName.ToString();
                hdnParentCitoValue.Value = entity.CitoAmount.ToString();
                hdnParentComplicationValue.Value = entity.ComplicationAmount.ToString();
            }
            BindGridView(1, true, ref PageCount);

            txtItemGroupCode.Attributes.Add("validationgroup", "mpEntryPopup");
            txtItemGroupName.Attributes.Add("validationgroup", "mpEntryPopup");
            txtPrintOrder.Attributes.Add("validationgroup", "mpEntryPopup");
        }

        protected string OnGetItemGroupFilterExpression()
        {
            return string.Format("GCItemType IN ('{0}','{1}','{2}') AND IsDeleted = 0", Constant.ItemGroupMaster.DRUGS, Constant.ItemGroupMaster.SUPPLIES, Constant.ItemGroupMaster.LOGISTIC);
        }

        protected string OnGetItemBedChargesFilterExpression()
        {
            return string.Format("GCItemType = '{0}' AND IsDeleted = 0", Constant.ItemGroupMaster.SERVICE);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            String filterExpression = String.Format("ParentID = '{0}' AND IsDeleted = 0", hdnParentItemGroupID.Value);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvItemGroupMasterRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 8);
            }
            grdView.DataSource = BusinessLayer.GetvItemGroupMasterList(filterExpression, 8, pageIndex, "ItemGroupID ASC");
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {

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
                result = "refresh|" + pageCount;
            }
            else if (param[0] == "save")
            {
                if (hdnItemGroupID.Value.ToString() != "")
                {
                    if (OnSaveEditRecord(ref errMessage))
                    {
                        result += "success";
                        BindGridView(1, true, ref pageCount);
                    }
                    else
                    {
                        result += string.Format("fail|{0}", errMessage);
                    }
                }
                else
                {
                    if (OnSaveAddRecord(ref errMessage))
                    {
                        result += "success";
                        BindGridView(1, true, ref pageCount);
                    }
                    else
                    {
                        result += string.Format("fail|{0}", errMessage);
                    }
                }
            }
            else if (param[0] == "delete")
            {
                if (OnDeleteRecord(ref errMessage))
                {
                    result += "success";
                    BindGridView(1, true, ref pageCount);
                }
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            result += "|" + pageCount;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(ItemGroupMaster entity)
        {
            entity.ItemGroupCode = txtItemGroupCode.Text;
            entity.ItemGroupName1 = txtItemGroupName.Text;
            entity.GCItemType = hdnGCItemType.Value;
            entity.PrintOrder = Convert.ToInt16(txtPrintOrder.Text);
            if (hdnRevenueSharingID.Value == "" || hdnRevenueSharingID.Value == "0")
                entity.RevenueSharingID = null;
            else
                entity.RevenueSharingID = Convert.ToInt32(hdnRevenueSharingID.Value);
            entity.ParentID = Convert.ToInt32(hdnParentItemGroupID.Value);
            entity.IsHeader = false;

            if (!string.IsNullOrEmpty(txtCITOAmountCtl.Text))
                entity.CitoAmount = Convert.ToDecimal(txtCITOAmountCtl.Text);
            else
                entity.CitoAmount = 0;

            entity.IsCitoInPercentage = chkIsCITOInPercentageCtl.Checked;
            if (!string.IsNullOrEmpty(txtCITOAmountCtl.Text))
                entity.ComplicationAmount = Convert.ToDecimal(txtComplicationAmountCtl.Text);             
            else
                entity.ComplicationAmount = 0;  

            entity.IsComplicationInPercentage = chkIsComplicationInPercentageCtl.Checked;
            entity.IsPrintWithDoctorName = chkIsPrintWithDoctorNameCtl.Checked;

            //Account 1
            if (hdnGLAccountID1.Value != "" && hdnGLAccountID1.Value != "0")
            {
                entity.GLAccount1 = Convert.ToInt32(hdnGLAccountID1.Value);
            }
            else
            {
                entity.GLAccount1 = null;
            }

            if (hdnSubLedgerDtID1.Value != "" && hdnSubLedgerDtID1.Value != "0")
            {
                entity.SubLedger1 = Convert.ToInt32(hdnSubLedgerDtID1.Value);
            }
            else
            {
                entity.SubLedger1 = null;
            }

            //Account 2
            if (hdnGLAccountID2.Value != "" && hdnGLAccountID2.Value != "0")
            {
                entity.GLAccount2 = Convert.ToInt32(hdnGLAccountID2.Value);
            }
            else
            {
                entity.GLAccount2 = null;
            }

            if (hdnSubLedgerDtID2.Value != "" && hdnSubLedgerDtID2.Value != "0")
            {
                entity.SubLedger2 = Convert.ToInt32(hdnSubLedgerDtID2.Value);
            }
            else
            {
                entity.SubLedger2 = null;
            }

            //Account 2
            if (hdnGLAccountID3.Value != "" && hdnGLAccountID3.Value != "0")
            {
                entity.GLAccount3 = Convert.ToInt32(hdnGLAccountID3.Value);
            }
            else
            {
                entity.GLAccount3 = null;
            }

            if (hdnSubLedgerDtID3.Value != "" && hdnSubLedgerDtID3.Value != "0")
            {
                entity.SubLedger3 = Convert.ToInt32(hdnSubLedgerDtID3.Value);
            }
            else
            {
                entity.SubLedger3 = null;
            }
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                ItemGroupMaster entity = new ItemGroupMaster();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entity.IsHeader = false;
                BusinessLayer.InsertItemGroupMaster(entity);
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
                ItemGroupMaster entity = BusinessLayer.GetItemGroupMaster(Convert.ToInt32(hdnItemGroupID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateItemGroupMaster(entity);
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
                ItemGroupMaster entity = BusinessLayer.GetItemGroupMaster(Convert.ToInt32(hdnItemGroupID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateItemGroupMaster(entity);
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