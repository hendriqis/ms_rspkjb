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
    public partial class RekamMedis1 : BasePageTrx
    {
        private const string STATUS_IMAGE_PATH = "~/libs/Images/Dashboard/";
        List<vConsultVisit9> lstData;
        List<vConsultVisit9> lstDataInpatient;
        List<vConsultVisit9> lstDataOutpatient;
        List<vConsultVisit9> lstDataIGD;
        List<vConsultVisit9> lstDataTotal;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Dashboard.RekamMedis1;
        }
        protected override void InitializeDataControl()
        {
            string filterExpressionInpatient = string.Format("DepartmentID = '{0}' AND GCVisitStatus != '{1}' AND MONTH(VisitDate) = '{2}' AND YEAR(VisitDate) = '{3}'", Constant.Facility.INPATIENT, Constant.VisitStatus.CANCELLED, DateTime.Now.Month, DateTime.Now.Year);
            string filterExpressionOutpatient = string.Format("DepartmentID = '{0}' AND GCVisitStatus != '{1}' AND MONTH(VisitDate) = '{2}' AND YEAR(VisitDate) = '{3}'", Constant.Facility.OUTPATIENT, Constant.VisitStatus.CANCELLED, DateTime.Now.Month, DateTime.Now.Year);
            string filterExpressionIGD = string.Format("DepartmentID = '{0}' AND GCVisitStatus != '{1}' AND MONTH(VisitDate) = '{2}' AND YEAR(VisitDate) = '{3}'", Constant.Facility.EMERGENCY, Constant.VisitStatus.CANCELLED, DateTime.Now.Month, DateTime.Now.Year);
            string filterExpressionTotal = string.Format("DepartmentID IN ('{0}','{1}','{2}') AND GCVisitStatus != '{3}' AND MONTH(VisitDate) = '{4}' AND YEAR(VisitDate) = '{5}'", Constant.Facility.INPATIENT, Constant.Facility.OUTPATIENT, Constant.Facility.EMERGENCY, Constant.VisitStatus.CANCELLED, DateTime.Now.Month, DateTime.Now.Year);
            string filterExpression = string.Format("GCVisitStatus != '{0}' AND MONTH(VisitDate) = '{1}' AND YEAR(VisitDate) = '{2}'", Constant.VisitStatus.CANCELLED, DateTime.Now.Month, DateTime.Now.Year);
            lstData = BusinessLayer.GetvConsultVisit9List(filterExpression);
            List<Department> lst = BusinessLayer.GetDepartmentList(string.Format("IsHasRegistration=1 and IsActive=1"));
            Methods.SetComboBoxField<Department>(cboDepartment, lst, "DepartmentName", "DepartmentID");
            cboDepartment.SelectedIndex = 0;
            lblDateTime.InnerText = DateTime.Now.ToString(Constant.FormatString.DATE_TIME_FORMAT);
            lstDataInpatient = BusinessLayer.GetvConsultVisit9List(filterExpressionInpatient);
            lstDataOutpatient = BusinessLayer.GetvConsultVisit9List(filterExpressionOutpatient);
            lstDataIGD = BusinessLayer.GetvConsultVisit9List(filterExpressionIGD);
            lstDataTotal = BusinessLayer.GetvConsultVisit9List(filterExpressionTotal);

            #region patient
            var resultinpatient = lstDataInpatient.GroupBy(inpatient => inpatient.MRN).Select(grp => grp.First()).ToList().OrderBy(x => x.MRN);
            var resultoutpatient = lstDataOutpatient.GroupBy(outpatient => outpatient.MRN).Select(grp => grp.First()).ToList().OrderBy(x => x.MRN);
            var resultigd = lstDataIGD.GroupBy(igd => igd.MRN).Select(grp => grp.First()).ToList().OrderBy(x => x.MRN);
            var resulttotal = lstDataTotal.GroupBy(total => total.MRN).Select(grp => grp.First()).ToList().OrderBy(x => x.MRN);
            lblInpatientVisitCount.InnerText = string.Format("{0}", resultinpatient.Count());
            lblOutpatientVisitCount.InnerText = string.Format("{0}", resultoutpatient.Count());
            lblIGDVisitCount.InnerText = string.Format("{0}", resultigd.Count());
            lblTotalVisitCount.InnerText = string.Format("{0}", resulttotal.Count());
            lblHeader.InnerText = string.Format("Hello, {0}", AppSession.UserLogin.ParamedicName);
            imgInpatient.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "user.png");
            imgOutpatient.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "user.png");
            imgIGD.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "user.png");
            imgTotal.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "user.png");
            #endregion

            BindGridView();
            BindGridViewPieChart();
            BindGridDetail();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string filterExpression = string.Format("GCVisitStatus != '{0}' AND MONTH(VisitDate) = '{1}' AND YEAR(VisitDate) = '{2}'", Constant.VisitStatus.CANCELLED, DateTime.Now.Month, DateTime.Now.Year);
            lstData = BusinessLayer.GetvConsultVisit9List(filterExpression);
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
                List<vConsultVisit9> lstDataMale = lstData.Where(t => t.GCGender == Constant.Gender.MALE).ToList();
                List<vConsultVisit9> lstDataFemale = lstData.Where(t => t.GCGender == Constant.Gender.FEMALE).ToList();
                List<vConsultVisit9> lstDataUn = lstData.Where(t => t.GCGender == Constant.Gender.UNSPECIFIED).ToList();

                Int32[] listData = new Int32[3];
                listData[0] = lstDataMale.Count();
                listData[1] = lstDataFemale.Count();
                listData[2] = lstDataUn.Count();

                JsonChartPieData.Value = JsonConvert.SerializeObject(listData, Formatting.Indented);
            }
        }

        private void BindGridDetail()
        {
            List<vAppointment> lstAppointment = BusinessLayer.GetvAppointmentList(string.Format("ParamedicID = {0} AND StartDate = '{1}' AND GCAppointmentStatus NOT IN ('{2}','{3}')", AppSession.UserLogin.ParamedicID, DateTime.Now.ToString(), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED));
            lvwView.DataSource = lstAppointment;
            lvwView.DataBind();
        }

        protected void cbpViewAppointment_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridDetail();
        }

        protected void cboDepartment_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            List<Department> lst = BusinessLayer.GetDepartmentList(string.Format("IsHasRegistration=1 and IsActive=1"));
            Methods.SetComboBoxField<Department>(cboDepartment, lst, "DepartmentName", "DepartmentID");
            cboDepartment.SelectedIndex = -1;
        }

    }
}