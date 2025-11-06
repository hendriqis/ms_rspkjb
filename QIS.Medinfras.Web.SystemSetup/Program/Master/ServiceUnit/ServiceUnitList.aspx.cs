using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.Service;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class ServiceUnitList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override string OnGetMenuCode()
        {
            String DepartmentID = Request.QueryString["id"];
            switch (DepartmentID)
            {
                case Constant.Facility.OUTPATIENT: return Constant.MenuCode.SystemSetup.CLINIC;
                case Constant.Facility.INPATIENT: return Constant.MenuCode.SystemSetup.WARD;
                case Constant.Facility.DIAGNOSTIC: return Constant.MenuCode.SystemSetup.DIAGNOSTIC_SUPPORT;
                case Constant.Facility.PHARMACY: return Constant.MenuCode.SystemSetup.PHARMACY;
                case Constant.Facility.MEDICAL_CHECKUP: return Constant.MenuCode.SystemSetup.MEDICAL_CHECKUP;
                default: return Constant.MenuCode.SystemSetup.EMERGENCY;
            }
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "Service Unit Code", "Service Unit Name" };
            fieldListValue = new string[] { "ServiceUnitCode", "ServiceUnitName" };
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            hdnIsBridgingToAplicares.Value = AppSession.IsBridgingToAPLICARES ? "1" : "0";

            String DepartmentID = Request.QueryString["id"];
            hdnDepartmentID.Value = DepartmentID;

            hdnFilterExpression.Value = filterExpression;
            hdnID.Value = keyValue;
            filterExpression = GetFilterExpression();
            if (keyValue != "")
            {
                int row = BusinessLayer.GetServiceUnitMasterRowIndex(filterExpression, keyValue) + 1;
                CurrPage = Helper.GetPageCount(row, Constant.GridViewPageSize.GRID_MASTER);
            }
            else
                CurrPage = 1;

            BindGridView(CurrPage, true, ref PageCount);
            GetSettingParameter();

            List<vHealthcareServiceUnit> lstHSU = BusinessLayer.GetvHealthcareServiceUnitList("1 = 0");
            grdDetail.DataSource = lstHSU;
            grdDetail.DataBind();
        }

        private void GetSettingParameter()
        {
            List<SettingParameterDt> lstSetParDt = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS));
            hdnIsBridgingToMedinfrasMobileApps.Value = lstSetParDt.Where(t => t.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS).FirstOrDefault().ParameterValue;
        }

        private string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("DepartmentID = '{0}' AND IsDeleted = 0", hdnDepartmentID.Value);
            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetServiceUnitMasterRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<ServiceUnitMaster> lstEntity = BusinessLayer.GetServiceUnitMasterList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ServiceUnitName ASC");
            grdView.DataSource = lstEntity;
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
                else // refresh
                {
                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            List<vHealthcareServiceUnit> lstHSU = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("ServiceUnitID = {0} AND IsDeleted = 0", hdnCollapseID.Value));
            grdDetail.DataSource = lstHSU;
            grdDetail.DataBind();
        }

        protected override bool OnAddRecord(ref string url, ref string errMessage)
        {
            url = ResolveUrl(string.Format("~/Program/Master/ServiceUnit/ServiceUnitEntry.aspx?id={0}", hdnDepartmentID.Value));
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                url = ResolveUrl(string.Format("~/Program/Master/ServiceUnit/ServiceUnitEntry.aspx?id={0}|{1}", hdnDepartmentID.Value, hdnID.Value));
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {

            if (hdnID.Value.ToString() != "")
            {
                ServiceUnitMaster entity = BusinessLayer.GetServiceUnitMaster(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateServiceUnitMaster(entity);

                string filterExpression = string.Format("ServiceUnitID = {0} AND IsDeleted = 0", hdnID.Value);
                List<HealthcareServiceUnit> lstHSU = BusinessLayer.GetHealthcareServiceUnitList(filterExpression);
                for(int a = 0; a < lstHSU.Count; a++)
                {
                    HealthcareServiceUnit hsu = BusinessLayer.GetHealthcareServiceUnit(lstHSU[0].HealthcareServiceUnitID);
                    hsu.IsDeleted = true;
                    hsu.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateHealthcareServiceUnit(hsu);
                }
                BridgingToMedinfrasMobileApps(entity.ServiceUnitID.ToString(), entity.ServiceUnitName, entity.ServiceUnitName2, entity.ShortName, "003");
                return true;
            }
            return false;
        }

        private void BridgingToMedinfrasMobileApps(string serviceUnitID, string serviceUnitName, string serviceUnitName2, string shortName, string eventype)
        {

            if (hdnIsBridgingToMedinfrasMobileApps.Value == "1")
            {
                if (serviceUnitID != string.Empty)
                {
                    MedinfrasMobileAppsService oService = new MedinfrasMobileAppsService();
                    APIMessageLog entityAPILog = new APIMessageLog();
                    string apiResult = oService.OnServiceUnitMasterChanged(serviceUnitID, serviceUnitName, serviceUnitName2, shortName, eventype);
                    string[] apiResultInfo = apiResult.Split('|');
                    if (apiResultInfo[0] == "0")
                    {
                        entityAPILog.IsSuccess = false;
                        entityAPILog.MessageText = apiResultInfo[1];
                        entityAPILog.Response = apiResultInfo[1];
                        Exception ex = new Exception(apiResultInfo[1]);
                        Helper.InsertErrorLog(ex);
                    }
                    else
                    {
                        entityAPILog.MessageText = apiResultInfo[0];
                        BusinessLayer.InsertAPIMessageLog(entityAPILog);
                    }
                }
            }
        }
    }
}