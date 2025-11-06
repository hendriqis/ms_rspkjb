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

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class InfoHistoryRegistrationCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        public override void InitializeDataControl(string param)
        {
            txtDateFrom.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtDateTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            string filterExpressionBSU = string.Format("HealthcareID = '{0}' AND BusinessPartnerID IN (SELECT DISTINCT BusinessPartnerID FROM Registration WHERE GCRegistrationStatus = '{1}') AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, Constant.VisitStatus.CHECKED_IN);
            List<BusinessPartners> lstBSU = BusinessLayer.GetBusinessPartnersList(filterExpressionBSU);
            lstBSU.Insert(0, new BusinessPartners { BusinessPartnerID = 0, BusinessPartnerName = "" });
            Methods.SetComboBoxField<BusinessPartners>(cboBusinessPartner, lstBSU, "BusinessPartnerName", "BusinessPartnerID");
            cboBusinessPartner.SelectedIndex = 0;
            hdnMRN.Value = param;
            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("MRN = {0} AND GCRegistrationStatus NOT IN ('{1}')", hdnMRN.Value, Constant.VisitStatus.CANCELLED);

            if (txtDateFrom.Text != "" && txtDateTo.Text != "")
            {
                filterExpression += String.Format(" AND RegistrationDate BETWEEN '{0}' AND '{1}'",
                        Helper.GetDatePickerValue(txtDateFrom.Text).ToString(Constant.FormatString.DATE_FORMAT_112),
                        Helper.GetDatePickerValue(txtDateTo.Text).ToString(Constant.FormatString.DATE_FORMAT_112)
                    );
            }

            if (cboBusinessPartner.Value != null && cboBusinessPartner.Value.ToString() != "0")
            {
                filterExpression += string.Format(" AND BusinessPartnerID = {0}", cboBusinessPartner.Value);
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvRegistrationHistoryRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vRegistrationHistory> lstEntity = BusinessLayer.GetvRegistrationHistoryList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void cbpInfoRegistrationView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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