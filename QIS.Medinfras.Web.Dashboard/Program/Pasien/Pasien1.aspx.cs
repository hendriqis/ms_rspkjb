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
using System.Web.UI.HtmlControls;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.Dashboard.Program
{
    public partial class Pasien1 : BasePageTrx
    {
        private const string STATUS_IMAGE_PATH = "~/libs/Images/Dashboard/";
        List<GetCountVisitDashboard> lstData;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Dashboard.Pasien1;
        }
        protected override void InitializeDataControl()
        {
            lstData = BusinessLayer.GetCountVisitDashboard(DateTime.Now.Year, DateTime.Now.Month, Convert.ToInt32(AppSession.UserLogin.ParamedicID));
            List<Department> lst = BusinessLayer.GetDepartmentList(string.Format("IsHasRegistration=1 and IsActive=1"));
            Methods.SetComboBoxField<Department>(cboDepartment, lst, "DepartmentName", "DepartmentID");
            cboDepartment.SelectedIndex = 0;
            lblDateTime.InnerText = DateTime.Now.ToString(Constant.FormatString.DATE_TIME_FORMAT);
            vParamedicMaster entity = BusinessLayer.GetvParamedicMasterList(string.Format("ParamedicID = {0} AND IsDeleted = 0", Convert.ToInt32(AppSession.UserLogin.ParamedicID))).FirstOrDefault();

            #region patient
            var result = lstData.GroupBy(test => test.MRN).Select(grp => grp.First()).ToList().OrderBy(x => x.MRN);
            var result2 = lstData.GroupBy(test => test.MRN).Select(grp => grp.First()).ToList().Where(z => z.RegistrationDate == DateTime.Now).OrderBy(x => x.MRN);
            var result3 = lstData.GroupBy(test => test.MRN).Select(grp => grp.First()).ToList().Where(z => z.RegistrationDate == DateTime.Now && z.IsNewPatient == false).OrderBy(x => x.MRN);
            var result4 = lstData.GroupBy(test => test.MRN).Select(grp => grp.First()).ToList().Where(z => z.RegistrationDate == DateTime.Now && z.IsNewPatient == true).OrderBy(x => x.MRN);
            lblPatientCount.InnerText = string.Format("{0}", result.Count());
            lblTodayPatient.InnerText = string.Format("{0}", result2.Count());
            lblOldPatient.InnerText = string.Format("{0}", result3.Count());
            lblNewPatient.InnerText = string.Format("{0}", result4.Count());
            lblHeader.InnerText = string.Format("Hello, {0}", AppSession.UserLogin.UserFullName);
            imgPatient.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "patient.png");
            imgToday.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "today.png");
            #endregion

            #region paramedic
            if(entity.IsAvailable == true)
            {
                lblEmployee.InnerText = "AKTIF";
            }
            else
            {
                lblEmployee.InnerText = "TIDAK AKTIF";
            }
            lblStatus.InnerText = string.Format("{0}", entity.EmploymentStatus);
            if (entity.HiredDate != null)
            {
                var timeSpan = DateTime.Now - entity.HiredDate;
                int age = new DateTime(timeSpan.Ticks).Year - 1;
                lblYear.InnerText = string.Format("{0} Tahun", age); ;
            }
            else
            {
                lblYear.InnerText = "";
            }
            imgStatus.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "employee.png");
            #endregion

            BindGridView();
            BindGridViewPieChart();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            lstData = BusinessLayer.GetCountVisitDashboard(DateTime.Now.Year, DateTime.Now.Month, Convert.ToInt32(AppSession.UserLogin.ParamedicID));
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                BindGridView();
                BindGridViewPieChart();
                result = string.Format("refresh");
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewTime_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter == "refreshHour")
            {
                hdnTimeNow.Value = DateTime.Now.ToString(Constant.FormatString.DATE_TIME_FORMAT);
                result = string.Format("refreshHour");
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView()
        {
            List<GetCountVisitPerDepartmentDashboard> lstEntity = BusinessLayer.GetCountVisitPerDepartmentDashboard(DateTime.Now.Year, DateTime.Now.Month, Convert.ToInt32(AppSession.UserLogin.ParamedicID), cboDepartment.Value.ToString());
            if (lstEntity.Count > 0)
            {
                List<ChartGraphV1> lstChart = new List<ChartGraphV1>();
                foreach (GetCountVisitPerDepartmentDashboard row in lstEntity)
                {
                    ChartGraphV1 entity = new ChartGraphV1();
                    entity.ID = row.ServiceUnitName;
                    entity.Value = row.CountVisit.ToString();
                    lstChart.Add(entity);
                }

                JsonChartData.Value = JsonConvert.SerializeObject(lstChart, Formatting.Indented);
            }
        }

        private void BindGridViewPieChart()
        {
            if (lstData.Count > 0)
            {
                List<GetCountVisitDashboard> lstDataMale = lstData.Where(t => t.GCGender == Constant.Gender.MALE).ToList();
                List<GetCountVisitDashboard> lstDataFemale = lstData.Where(t => t.GCGender == Constant.Gender.FEMALE).ToList();
                List<GetCountVisitDashboard> lstDataUn = lstData.Where(t => t.GCGender == Constant.Gender.UNSPECIFIED).ToList();

                Int32[] listData = new Int32[3];
                listData[0] = lstDataMale.Count();
                listData[1] = lstDataFemale.Count();
                listData[2] = lstDataUn.Count();

                JsonChartPieData.Value = JsonConvert.SerializeObject(listData, Formatting.Indented);
            }
        }

        protected void cboDepartment_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            List<Department> lst = BusinessLayer.GetDepartmentList(string.Format("IsHasRegistration=1 and IsActive=1"));
            Methods.SetComboBoxField<Department>(cboDepartment, lst, "DepartmentName", "DepartmentID");
            cboDepartment.SelectedIndex = -1;
        }

    }
}