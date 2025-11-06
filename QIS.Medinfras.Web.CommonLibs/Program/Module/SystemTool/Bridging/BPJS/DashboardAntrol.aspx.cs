using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Common;
using QIS.Medinfras.Web.CommonLibs.Service;
using QIS.Data.Core.Dal;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class DashboardAntrol : BasePageTrx
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override string OnGetMenuCode()
        {
            switch (hdnModuleID.Value)
            {
                case Constant.Module.SYSTEM_SETUP:
                    return Constant.MenuCode.SystemSetup.BPJS_AntrianOnline_Dashboard;
                default:
                    return Constant.MenuCode.SystemSetup.BPJS_AntrianOnline_Dashboard;
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected string GetRefreshGridInterval()
        {
            return AppSession.RefreshGridInterval;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowDelete = IsAllowEdit = false;
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString["id"] != null)
            {
                hdnModuleID.Value = Page.Request.QueryString["id"];
            }

            txtFromDate.Text = DateTime.Today.AddDays(-1).ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            List<Variable> lstBulan = new List<Variable>();
            lstBulan.Add(new Variable { Code = "01", Value = "Januari" });
            lstBulan.Add(new Variable { Code = "02", Value = "Februari" });
            lstBulan.Add(new Variable { Code = "03", Value = "Maret" });
            lstBulan.Add(new Variable { Code = "04", Value = "April" });
            lstBulan.Add(new Variable { Code = "05", Value = "Mei" });
            lstBulan.Add(new Variable { Code = "06", Value = "Juni" });
            lstBulan.Add(new Variable { Code = "07", Value = "Juli" });
            lstBulan.Add(new Variable { Code = "08", Value = "Agustus" });
            lstBulan.Add(new Variable { Code = "09", Value = "September" });
            lstBulan.Add(new Variable { Code = "10", Value = "Oktober" });
            lstBulan.Add(new Variable { Code = "11", Value = "November" });
            lstBulan.Add(new Variable { Code = "12", Value = "Desember" });
            Methods.SetComboBoxField<Variable>(cboBulan, lstBulan, "Value", "Code");
            cboBulan.SelectedIndex = 0;

            List<Variable> lstTahun = new List<Variable>();
            int todayYear = DateTime.Now.Year;
            int lastYear = DateTime.Now.AddYears(-1).Year;
            lstTahun.Add(new Variable { Code = todayYear.ToString(), Value = todayYear.ToString() });
            lstTahun.Add(new Variable { Code = lastYear.ToString(), Value = lastYear.ToString() });
            Methods.SetComboBoxField<Variable>(cboTahun, lstTahun, "Value", "Code");
            cboTahun.SelectedIndex = 0;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount, ref string errMessage)
        {
            BPJSService oService = new BPJSService();
            string parameter = string.Empty;
            if (rbDateFilter.SelectedValue == "BULAN")
            {
                parameter = string.Format("{0}|{1}|{2}", cboBulan.Value.ToString(), cboTahun.Value.ToString(), rbTipeWaktu.SelectedValue);
            }
            else
            {
                parameter = string.Format("{0}|{1}", Helper.GetDatePickerValue(txtFromDate.Text).ToString(Constant.FormatString.DATE_PICKER_FORMAT2), rbTipeWaktu.SelectedValue);
            }
            string response = oService.GetDashboardAntrianOnline(rbDateFilter.SelectedValue, parameter);
            string[] respInfo = response.Split('|');
            if (respInfo[0] == "1")
            {
                ResponseAntrol responseAntrol = JsonConvert.DeserializeObject<ResponseAntrol>(respInfo[1]);
                grdView.DataSource = responseAntrol.list.OrderBy(o => o.tanggal).OrderBy(o => o.namapoli).ToList();
                grdView.DataBind();
            }
            else
            {
                errMessage = respInfo[1];
            }
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            string errMessage = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount, ref errMessage);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridView(1, true, ref pageCount, ref errMessage);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}