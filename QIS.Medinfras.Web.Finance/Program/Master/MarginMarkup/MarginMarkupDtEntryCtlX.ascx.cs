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
    public partial class MarginMarkupDtEntryCtlX : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnMarkupID.Value = param;
            MarginMarkupHd entity = BusinessLayer.GetMarginMarkupHd(Convert.ToInt32(hdnMarkupID.Value));
            txtMarkupCode.Text = entity.MarkupCode;
            txtMarkupName.Text = entity.MarkupName;

            BindGridView();


            txtEndingValue.Attributes.Add("validationgroup", "mpEntryPopup");
            txtStartingValue.Attributes.Add("validationgroup", "mpEntryPopup");
            txtMarkupAmount.Attributes.Add("validationgroup", "mpEntryPopup");
            txtMarkupAmount2.Attributes.Add("validationgroup", "mpEntryPopup");
        }

        private void BindGridView()
        {
            grdView.DataSource = BusinessLayer.GetMarginMarkupDtList(string.Format("MarkupID = '{0}' AND IsDeleted = 0", hdnMarkupID.Value));
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

            string param = e.Parameter;

            string result = param + "|";
            string errMessage = "";

            if (param == "save")
            {
                if (hdnSequenceNo.Value.ToString() != "")
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

        private void ControlToEntity(MarginMarkupDt entity)
        {
            entity.StartingValue = Convert.ToDecimal(txtStartingValue.Text);
            entity.EndingValue = Convert.ToDecimal(txtEndingValue.Text);
            entity.MarkupAmount = Convert.ToDecimal(txtMarkupAmount.Text);
            entity.MarkupAmount2 = Convert.ToDecimal(txtMarkupAmount2.Text);
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                MarginMarkupDt entity = new MarginMarkupDt();
                ControlToEntity(entity);
                entity.MarkupID = Convert.ToInt32(hdnMarkupID.Value);
                entity.SequenceNo = Convert.ToInt16(BusinessLayer.GetMarginMarkupDtMaxSequenceNo(string.Format("MarkupID = {0}", hdnMarkupID.Value)) + 1);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertMarginMarkupDt(entity);
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
                MarginMarkupDt entity = BusinessLayer.GetMarginMarkupDt(Convert.ToInt32(hdnMarkupID.Value), Convert.ToInt16(hdnSequenceNo.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateMarginMarkupDt(entity);
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
                MarginMarkupDt entity = BusinessLayer.GetMarginMarkupDt(Convert.ToInt32(hdnMarkupID.Value), Convert.ToInt16(hdnSequenceNo.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateMarginMarkupDt(entity);
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