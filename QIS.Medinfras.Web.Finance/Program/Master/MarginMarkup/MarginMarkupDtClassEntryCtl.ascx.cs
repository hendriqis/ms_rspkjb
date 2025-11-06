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
    public partial class MarginMarkupDtClassEntryCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnMarkupIDCtl.Value = paramInfo[0];
            hdnSequenceNoCtl.Value = paramInfo[1];

            MarginMarkupHd entityHd = BusinessLayer.GetMarginMarkupHd(Convert.ToInt32(hdnMarkupIDCtl.Value));
            txtMarkupCodeCtl.Text = entityHd.MarkupCode;
            txtMarkupNameCtl.Text = entityHd.MarkupName;
            txtSequenceNoCtl.Text = hdnSequenceNoCtl.Value;

            BindGridView();
        }

        private void BindGridView()
        {
            string filter = string.Format("MarkupID = {0} AND SequenceNo = {1} AND IsDeleted = 0", hdnMarkupIDCtl.Value, hdnSequenceNoCtl.Value);
            List<vMarginMarkupDtClass> lst = BusinessLayer.GetvMarginMarkupDtClassList(filter, 100, 1, "ClassCode ASC");
            grdView.DataSource = lst;
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
                if (hdnIDCtl.Value.ToString() != "")
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

        private void ControlToEntity(MarginMarkupDtClass entity)
        {
            entity.ClassID = Convert.ToInt32(hdnClassID.Value);
            entity.MarkupAmount = Convert.ToDecimal(txtMarkupAmount.Text);
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                MarginMarkupDtClass entity = new MarginMarkupDtClass();
                ControlToEntity(entity);
                entity.MarkupID = Convert.ToInt32(hdnMarkupIDCtl.Value);
                entity.SequenceNo = Convert.ToInt16(hdnSequenceNoCtl.Value);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertMarginMarkupDtClass(entity);
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
                MarginMarkupDtClass entity = BusinessLayer.GetMarginMarkupDtClass(Convert.ToInt32(hdnIDCtl.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateMarginMarkupDtClass(entity);
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
                MarginMarkupDtClass entity = BusinessLayer.GetMarginMarkupDtClass(Convert.ToInt32(hdnIDCtl.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateMarginMarkupDtClass(entity);
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