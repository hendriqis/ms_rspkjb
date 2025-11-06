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
    public partial class ReportParameterEntryCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnReportID.Value = param;
            ReportMaster entity = BusinessLayer.GetReportMaster(Convert.ToInt32(hdnReportID.Value));
            txtHeaderText.Text = string.Format("{0} - {1}", entity.ReportCode, entity.ReportTitle1);

            BindGridView();

            txtFilterParameterCode.Attributes.Add("validationgroup", "mpEntryPopup");
            txtFilterParameterName.Attributes.Add("validationgroup", "mpEntryPopup");
            txtDisplayOrder.Attributes.Add("validationgroup", "mpEntryPopup");
        }

        private void BindGridView()
        {
            grdView.DataSource = BusinessLayer.GetvReportParameterList(string.Format("ReportID = {0} ORDER BY FilterParameterName ASC", hdnReportID.Value));
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

        private void ControlToEntity(ReportParameter entity)
        {
            entity.DisplayOrder = Convert.ToInt16(txtDisplayOrder.Text);
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                ReportParameter entity = new ReportParameter();
                ControlToEntity(entity);
                entity.FilterParameterID = Convert.ToInt32(hdnFilterParameterID.Value);
                entity.ReportID = Convert.ToInt32(hdnReportID.Value);
                BusinessLayer.InsertReportParameter(entity);
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
                ReportParameter entity = BusinessLayer.GetReportParameter(Convert.ToInt32(hdnReportID.Value), Convert.ToInt32(hdnFilterParameterID.Value));
                ControlToEntity(entity);
                BusinessLayer.UpdateReportParameter(entity);
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
                BusinessLayer.DeleteReportParameter(Convert.ToInt32(hdnReportID.Value), Convert.ToInt32(hdnFilterParameterID.Value));
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