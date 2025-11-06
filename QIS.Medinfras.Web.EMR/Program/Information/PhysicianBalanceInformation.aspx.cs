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

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class PhysicianBalanceInformation : BasePageTrx
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.INFORMATION_PHYSICIAN_BALANCE;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            txtDateFrom.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtDateTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            List<Department> lstDepartment = BusinessLayer.GetDepartmentList("IsActive = 1 AND IsHasRegistration = 1");
            lstDepartment.Add(new Department { DepartmentID = "", DepartmentName = "ALL" });
            Methods.SetComboBoxField<Department>(cboDepartment, lstDepartment, "DepartmentName", "DepartmentID");
            cboDepartment.Value = "ALL";

            hdnParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
            BindGridView(CurrPage, true, ref PageCount);
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        private string GetFilterExpression()
        {
            string filterExpression = string.Format("ParamedicID = {0} AND IsDeleted = 0", hdnParamedicID.Value);            
            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            //string filterExpression = GetFilterExpression();

            //List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(filterExpression);
            //grdView.DataSource = lstParamedic;
            //grdView.DataBind();

            DateTime FromDate = Helper.GetDatePickerValue(txtDateFrom.Text);
            DateTime ToDate = Helper.GetDatePickerValue(txtDateTo.Text);
            string DepartmentID = hdnParamDepartmentID.Value.ToString() != "" ? hdnParamDepartmentID.Value : "";

            List<GetPatientChargesHdRevenueSharing2> lst = BusinessLayer.GetPatientChargesHdRevenueSharing2List(
                                                                                        Convert.ToInt32(AppSession.UserLogin.ParamedicID),
                                                                                        DepartmentID,
                                                                                        0,
                                                                                        0,
                                                                                        "",
                                                                                        "%%",
                                                                                        Constant.RevenuePeriodeType.TANGGAL_TRANSAKSI,
                                                                                        FromDate,
                                                                                        ToDate,
                                                                                        "00:00",
                                                                                        "23:59",
                                                                                        0,
                                                                                        1, //PageIndex
                                                                                        10000, //NumRows
                                                                                        ""
                                                                                    ).Where(t => t.SharingAmount != 0).ToList();
            grdView.DataSource = lst;
            grdView.DataBind();
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
                else 
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