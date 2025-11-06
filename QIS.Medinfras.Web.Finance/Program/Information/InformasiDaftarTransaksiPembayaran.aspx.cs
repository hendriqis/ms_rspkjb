using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
//using DevExpress.Web.ASPxPivotGrid;
using QIS.Medinfras.Web.Common;
using DevExpress.Utils;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxEditors;


namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class InformasiDaftarTransaksiPembayaran : BasePageTrx
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.INFORMASI_TRANSAKSI_VS_PEMBAYARAN;
        }

        protected override void InitializeDataControl()
        {
            List<Department> lstDepartment = BusinessLayer.GetDepartmentList("IsActive = 1 AND IsHasRegistration = 1");
            Methods.SetComboBoxField<Department>(cboDepartment, lstDepartment, "DepartmentName", "DepartmentID");
            cboDepartment.SelectedIndex = 0;

            List<Variable> lstPeriodeType = new List<Variable>();
            lstPeriodeType.Add(new Variable { Code = "0", Value = "Tanggal Registrasi" });
            lstPeriodeType.Add(new Variable { Code = "1", Value = "Tanggal Keluar" });
            Methods.SetComboBoxField<Variable>(cboPeriodeType, lstPeriodeType, "Value", "Code");
            cboPeriodeType.SelectedIndex = 0;

            txtPeriodFrom.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPeriodTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            List<Variable> lstRegistrationStatus = new List<Variable>();
            lstRegistrationStatus.Add(new Variable { Code = "0", Value = "CLOSED" });
            lstRegistrationStatus.Add(new Variable { Code = "1", Value = "CANCELLED" });
            lstRegistrationStatus.Add(new Variable { Code = "2", Value = "NON CLOSED & NON CANCELLED" });
            Methods.SetComboBoxField<Variable>(cboRegistrastionStatus, lstRegistrationStatus, "Value", "Code");
            cboRegistrastionStatus.SelectedIndex = 0;

            BindGridView(1, true, ref PageCount);

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }
        protected override void OnControlEntrySetting()
        {
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            //int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref PageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridView(1, true, ref PageCount);
                    result = "refresh|" + PageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            DateTime FromDate = Helper.GetDatePickerValue(txtPeriodFrom.Text);
            DateTime ToDate = Helper.GetDatePickerValue(txtPeriodTo.Text);

            string filterExpression = "";

            if (chkIsFilter.Checked)
            {
                filterExpression = string.Format("DepartmentID != '{0}'", cboDepartment.Value);
            }
            else
            {
                filterExpression = string.Format("DepartmentID = '{0}'", cboDepartment.Value);
            }

            if (cboPeriodeType.Value.ToString() == "0")
            {
                filterExpression += string.Format(" AND RegistrationDate BETWEEN '{0}' AND '{1}'", FromDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT2), ToDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT2));
            }
            else
            {
                filterExpression += string.Format(" AND DischargeDate BETWEEN '{0}' AND '{1}'", FromDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT2), ToDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT2));
            }

            if (cboRegistrastionStatus.Value.ToString() == "0")
            {
                filterExpression += string.Format(" AND GCRegistrationStatus = '{0}'", Constant.VisitStatus.CLOSED);
            }
            else if (cboRegistrastionStatus.Value.ToString() == "1")
            {
                filterExpression += string.Format(" AND GCRegistrationStatus = '{0}'", Constant.VisitStatus.CANCELLED);
            }
            else
            {
                filterExpression += string.Format("AND GCRegistrationStatus NOT IN ('{0}', '{1}')", Constant.VisitStatus.CLOSED, Constant.VisitStatus.CANCELLED);
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvInformasiTransaksiVSPembayaranRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vInformasiTransaksiVSPembayaran> lstEntity = BusinessLayer.GetvInformasiTransaksiVSPembayaranList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "RegistrationID");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }
    }
}