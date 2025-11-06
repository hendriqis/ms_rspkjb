using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Utils;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class TreasuryInformationDetail : BasePageTrx
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.BALANCE_ACCOUNT_DETAIL;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            #region Data Month + Year
            cboMonth.DataSource = Enumerable.Range(1, 12).Select(a => new
            {
                MonthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(a),
                MonthNumber = a
            });
            cboMonth.TextField = "MonthName";
            cboMonth.ValueField = "MonthNumber";
            cboMonth.EnableCallbackMode = false;
            cboMonth.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
            cboMonth.DropDownStyle = DropDownStyle.DropDownList;
            cboMonth.DataBind();
            cboMonth.Value = DateTime.Now.Month.ToString();

            cboYear.DataSource = Enumerable.Range(DateTime.Now.Year - 99, 100).Reverse();
            cboYear.EnableCallbackMode = false;
            cboYear.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
            cboYear.DropDownStyle = DropDownStyle.DropDownList;
            cboYear.DataBind();
            cboYear.SelectedIndex = 0;
            #endregion

            List<HealthcareParameter> lstSerVarDt = BusinessLayer.GetHealthcareParameterList(string.Format(
                                "ParameterCode IN ('{0}','{1}','{2}')",
                                Constant.HealthcareParameter.AC_DEFAULT_SEGMENT_DEPARTMENT, //0
                                Constant.HealthcareParameter.AC_DEFAULT_SEGMENT_SERVICE_UNIT, //1
                                Constant.HealthcareParameter.AC_DEFAULT_SEGMENT_BUSINESS_PARTNER //2
                            ));

            List<Department> lstD = BusinessLayer.GetDepartmentList("GLAccountNoSegment IS NOT NULL AND IsActive = 1");
            lstD.Insert(0, new Department { DepartmentID = "", DepartmentName = "" });

            List<TempDataSource> lst = new List<TempDataSource>();
            lst.Insert(0, new TempDataSource { SourceID = "1", SourceName = "Transaksi APPROVED" });
            lst.Insert(1, new TempDataSource { SourceID = "2", SourceName = "Transaksi NON-VOID" });
            Methods.SetComboBoxField<TempDataSource>(cboStatus, lst, "SourceName", "SourceID");
            cboStatus.SelectedIndex = 1;

        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridView();
                SetTotalText();
                result = "refresh|" + PageCount;
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void SetTotalText()
        {
            List<GetTreasuryActivityHdDetail> lstEntity = null;

            if (hdnGLAccountID.Value == "")
            {
                PageCount = 0;
                lstEntity = new List<GetTreasuryActivityHdDetail>();
                grdView.DataSource = lstEntity;
                grdView.DataBind();
                hdnSaldo.Value = "";
            }
            else
            {
                int GLAccountID = Convert.ToInt32(hdnGLAccountID.Value);
                int Year = Convert.ToInt32(cboYear.Value.ToString());
                int Month = Convert.ToInt32(cboMonth.Value.ToString());
                int Status = Convert.ToInt32(cboStatus.Value.ToString());

                lstEntity = BusinessLayer.GetTreasuryActivityHdDetailList(Year, Month, GLAccountID, Status);

                hdnSaldo.Value = lstEntity.LastOrDefault().BalanceEND.ToString(Constant.FormatString.NUMERIC_2);
            }

            txtTotalBalanceEnd.Text = hdnSaldo.Value;
        }

        private void BindGridView()
        {
            if (hdnGLAccountID.Value != "")
            {
                int GLAccountID = Convert.ToInt32(hdnGLAccountID.Value);
                int Year = Convert.ToInt32(cboYear.Value.ToString());
                int Month = Convert.ToInt32(cboMonth.Value.ToString());
                int Status = Convert.ToInt32(cboStatus.Value.ToString());

                List<GetTreasuryActivityHdDetail> lstEntity = BusinessLayer.GetTreasuryActivityHdDetailList(Year, Month, GLAccountID, Status);

                grdView.DataSource = lstEntity;
                grdView.DataBind();
            }
        }

        protected class TempDataSource
        {
            String _SourceID;
            String _SourceName;

            public String SourceID
            {
                get { return _SourceID; }
                set { _SourceID = value; }
            }

            public String SourceName
            {
                get { return _SourceName; }
                set { _SourceName = value; }
            }
        }
    }
}