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

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class GLSubLedgerInformation : BasePageTrx
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.BALANCE_INFORMATION_SUB_ACCOUNT;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            #region Data Month
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

            List<TempDataSource> lst = new List<TempDataSource>();
            lst.Insert(0, new TempDataSource { SourceID = "1", SourceName = "Transaksi APPROVED" });
            lst.Insert(1, new TempDataSource { SourceID = "2", SourceName = "Transaksi NON-VOID" });
            Methods.SetComboBoxField<TempDataSource>(cboStatus, lst, "SourceName", "SourceID");
            cboStatus.SelectedIndex = 1;
            hdnDataSource.Value = "2";

            BindGridView();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridView();
                result = "refresh|" + PageCount;
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView()
        {
            int SubLedgerID = 0, ServiceUnitID = 0, BusinessPartnerID = 0;
            string HealthcareID = "0", DepartmentID = "0";
            List<GetGLBalanceDtInformationPerPeriode> lstEntity = null;

            if (hdnGLAccountID.Value == "")
            {
                PageCount = 0;
                lstEntity = new List<GetGLBalanceDtInformationPerPeriode>();
                grdView.DataSource = lstEntity;
                grdView.DataBind();
            }
            else
            {
                lstEntity = BusinessLayer.GetGLBalanceDtInformationPerPeriodeList(
                                                    Convert.ToInt32(hdnGLAccountID.Value),
                                                    SubLedgerID,
                                                    HealthcareID,
                                                    DepartmentID,
                                                    ServiceUnitID,
                                                    BusinessPartnerID,
                                                    Convert.ToInt32(cboYear.Value),
                                                    Convert.ToInt32(cboMonth.Value),
                                                    cboStatus.Value.ToString(),
                                                    0);
                
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