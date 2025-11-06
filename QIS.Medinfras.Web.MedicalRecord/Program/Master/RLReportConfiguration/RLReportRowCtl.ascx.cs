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


namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class RLReportRowCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnReportID.Value = param;
            RLReport entity = BusinessLayer.GetRLReport(Convert.ToInt32(hdnReportID.Value));
            txtHeaderText.Text = string.Format("{0} - {1}", entity.RLReportCode, entity.RLReportName);

            BindGridView();

            txtRowTitle.Attributes.Add("validationgroup","mpEntryPopup");
            txtDisplayOrder.Attributes.Add("validationgroup","mpEntryPopup");
        }

        private void BindGridView()
        {
            grdView.DataSource = BusinessLayer.GetRLReportRowList(string.Format("RLReportID = {0} AND IsDeleted = 0 ORDER BY DisplayOrder", hdnReportID.Value));
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
                if (hdnIsAdd.Value.ToString() != "1")
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

        private void ControlToEntity(RLReportRow entity)
        {
            entity.RLReportID = Convert.ToInt32(hdnReportID.Value);
            entity.RowTitle = txtRowTitle.Text;
            entity.DisplayOrder = Convert.ToInt16(txtDisplayOrder.Text);
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                RLReportRow entity = new RLReportRow();
                ControlToEntity(entity);
                BusinessLayer.InsertRLReportRow(entity);
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
                RLReportRow entity = BusinessLayer.GetRLReportRow(Convert.ToInt16(hdnMainID.Value));
                ControlToEntity(entity);
                BusinessLayer.UpdateRLReportRow(entity);
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
                RLReportRow entity = BusinessLayer.GetRLReportRow(Convert.ToInt32(hdnMainID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.DeleteRLReportRow(Convert.ToInt32(hdnMainID.Value));
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