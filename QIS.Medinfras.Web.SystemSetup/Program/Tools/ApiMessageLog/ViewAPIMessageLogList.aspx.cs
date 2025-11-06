using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class ViewAPIMessageLogList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.VIEW_APIMESSAGE_LOG;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowDelete = IsAllowEdit = false;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            txtLogDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            List<StandardCode> lstSC = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.API_PARTY));
            lstSC.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });

            Methods.SetComboBoxField<StandardCode>(cboSender, lstSC, "StandardCodeName", "StandardCodeID");
            cboSender.SelectedIndex = 0;

            Methods.SetComboBoxField<StandardCode>(cboRecipient, lstSC, "StandardCodeName", "StandardCodeID");
            cboRecipient.SelectedIndex = 0;

            BindGridView(CurrPage, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string date = Helper.GetDatePickerValue(txtLogDate).ToString("yyyyMMdd");
            String filterExpression = String.Format("CONVERT(VARCHAR, MessageDateTime, 112) = '{0}'", date);

            if (cboSender.Value != null)
            {
                if (!string.IsNullOrEmpty(cboSender.Value.ToString()))
                {
                    filterExpression += " AND ";
                    filterExpression += string.Format("Sender = '{0}'", cboSender.Text.ToString());
                }
            }

            if (cboRecipient.Value != null)
            {
                if (cboRecipient.Value.ToString() != "0")
                {
                    filterExpression += " AND ";
                    filterExpression += string.Format("Recipient = '{0}'", cboRecipient.Text.ToString());
                }
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvAPIMessageLogRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vAPIMessageLog> lstEntity = BusinessLayer.GetvAPIMessageLogList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "MessageDateTime DESC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vAPIMessageLog entity = e.Row.DataItem as vAPIMessageLog;
                TextBox txtMessageText = e.Row.FindControl("txtMessageText") as TextBox;
                TextBox txtRespose = e.Row.FindControl("txtRespose") as TextBox;

                txtMessageText.Text = entity.cfMessageText;
                txtRespose.Text = entity.cfResponse;
            }
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}