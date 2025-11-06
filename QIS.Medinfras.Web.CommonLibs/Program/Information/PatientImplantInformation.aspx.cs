using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientImplantInformation : BasePageContent
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.PATIENT_IMPLANT_INFORMATION;
        }

        private GetUserMenuAccess menu;

        protected String GetMenuCaption()
        {
            if (menu != null)
                return GetLabel(menu.MenuCaption);
            return "";
        }

        protected string GetRefreshGridInterval()
        {
            return "1";
        }

        private string refreshGridInterval = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;

                txtTransactionDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());
                BindGridView(1, true, ref PageCount);
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

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientMedicalDeviceInformationRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_PATIENT_LIST);
            }

            List<vPatientMedicalDeviceInformation> lstEntity = BusinessLayer.GetvPatientMedicalDeviceInformationList(filterExpression, Constant.GridViewPageSize.GRID_PATIENT_LIST, pageIndex, "TransactionDate, MedicalNo, ItemName");
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }


        private string GetFilterExpression()
        {
            string filterExpression = string.Format("IsDeleted = 0 AND PatientMedicalDeviceIsDeleted = 0 AND GCTransactionStatus != '{0}' AND GCTransactionDetailStatus != '{0}'", Constant.TransactionStatus.VOID);

            if (!chkIsPreviousEpisodePatient.Checked)
            {
                filterExpression += string.Format(" AND TransactionDate = '{0}' ", Helper.GetDatePickerValue(txtTransactionDate).ToString(Constant.FormatString.DATE_FORMAT_112));

            }

            if (hdnFilterExpressionQuickSearch.Value != "")
            {
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            }

            if (hdnPhysicianID.Value != "")
            {
                filterExpression += string.Format(" AND ParamedicID = {0}", hdnPhysicianID.Value);
            }
            return filterExpression;
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            //if (e.Item.ItemType == ListViewItemType.DataItem)
            //{
            //    ListViewDataItem dataitem = (ListViewDataItem)e.Item;
            //    if (!Convert.ToBoolean(DataBinder.Eval(dataitem.DataItem, "IsNewPatient")))
            //    {
            //        System.Web.UI.HtmlControls.HtmlTableRow tr = (System.Web.UI.HtmlControls.HtmlTableRow)dataitem.FindControl("trItem");
            //        //tr.BgColor = System.Drawing.Color.AliceBlue.ToString();
            //        tr.Attributes.Add("class", "LvColor");
            //    }
            //}
        }        
    }
}