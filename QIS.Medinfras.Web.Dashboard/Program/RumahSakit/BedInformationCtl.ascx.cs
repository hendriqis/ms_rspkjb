using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.Dashboard.Program
{
    public partial class BedInformationCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            List<Department> lst = BusinessLayer.GetDepartmentList(string.Format("IsHasRegistration=1 and IsActive=1"));
            //Methods.SetComboBoxField<Department>(cboDepartment, lst, "DepartmentName", "DepartmentID");
            //cboDepartment.SelectedIndex = 0;
            p1.InnerText = "TES";

            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = "isDeleted = 0 AND GCBedStatus != '0116^C' AND HealthcareServiceUnitID IS NOT NULL";
            List<vBed> lstEntity = BusinessLayer.GetvBedList(filterExpression);
            if (lstEntity.Count > 0)
            {
                DataTable.Value = JsonConvert.SerializeObject(lstEntity, Formatting.None);
            }
        }

        //protected void cbpView1_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        //{
        //    string result = "";
        //    if (e.Parameter != null && e.Parameter != "")
        //    {
        //        string[] param = e.Parameter.Split('|');
        //        BindGridView();
        //        result = string.Format("refresh");
        //    }

        //    ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
        //    panel.JSProperties["cpResult"] = result;
        //}

        //private void BindGridView()
        //{
        //    List<GetCountVisitPerDepartmentDashboard> lstEntity = BusinessLayer.GetCountVisitPerDepartmentDashboard(DateTime.Now.Year, DateTime.Now.Month, Convert.ToInt32(AppSession.UserLogin.ParamedicID), cboDepartment.Value.ToString());
        //    if (lstEntity.Count > 0)
        //    {
        //        List<ChartGraphV1> lstChart = new List<ChartGraphV1>();
        //        foreach (GetCountVisitPerDepartmentDashboard row in lstEntity)
        //        {
        //            ChartGraphV1 entity = new ChartGraphV1();
        //            entity.ID = row.ServiceUnitName;
        //            entity.Value = row.CountVisit.ToString();
        //            lstChart.Add(entity);
        //        }

        //        JsonChartData1.Value = JsonConvert.SerializeObject(lstChart, Formatting.Indented);
        //    }
        //}

        //protected void cboDepartment_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        //{
        //    List<Department> lst = BusinessLayer.GetDepartmentList(string.Format("IsHasRegistration=1 and IsActive=1"));
        //    Methods.SetComboBoxField<Department>(cboDepartment, lst, "DepartmentName", "DepartmentID");
        //    cboDepartment.SelectedIndex = -1;
        //}
    }
}