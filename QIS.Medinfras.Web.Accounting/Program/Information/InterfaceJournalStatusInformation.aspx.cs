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
    public partial class InterfaceJournalStatusInformation : BasePageTrx
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.INTERFACE_JOURNAL_STATUS_INFORMATION;
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
            string paramYear = cboYear.Value.ToString();
            string paramMonth = Convert.ToInt32(cboMonth.Value) < 10 ? ("0" + cboMonth.Value.ToString()) : cboMonth.Value.ToString();

            List<GetInterfaceJournalStatusInformation> lstEntity = BusinessLayer.GetInterfaceJournalStatusInformationList(paramYear, paramMonth);
            grdView.DataSource = lstEntity;
            grdView.DataBind();


            divStatus0.InnerHtml = "Tidak memiliki data jurnal otomatis (sudah maupun belum terbentuk transaksi jurnal)";
            divStatus1.InnerHtml = "Memiliki data transaksi jurnal (otomatis & memorial) yang statusnya tidak void";
            divStatus2.InnerHtml = "Memiliki data transaksi jurnal (otomatis & memorial) yang statusnya masih open";
            divStatus3.InnerHtml = "Ada data jurnal - Belum memiliki transaksi jurnal otomatis - Error jika diproses jurnal otomatis";
            divStatus4.InnerHtml = "Ada data jurnal - Belum memiliki transaksi jurnal otomatis - Akan berhasil jika diproses jurnal otomatis";
        }

    }
}