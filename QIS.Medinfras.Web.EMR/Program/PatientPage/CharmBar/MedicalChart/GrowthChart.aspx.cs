using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Data.Service.DataLayer.Base;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class GrowthChart : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Patient entity = BusinessLayer.GetPatient(AppSession.RegisteredPatient.MRN);
                if (entity.GCGender == Constant.Gender.MALE)
                    hdnGender.Value = "1";
                else
                    hdnGender.Value = "2";

                ddlChartType.Items.Add(new ListItem { Text = "Weight-for-age", Value = "W" });
                ddlChartType.Items.Add(new ListItem { Text = "Length-for-age", Value = "H" });
                ddlChartType.Items.Add(new ListItem { Text = "Head Circumference", Value = "C" });
                ddlChartType.Items.Add(new ListItem { Text = "BMI", Value = "B" });
                
                int ageInMonth = Function.GetPatientAgeInMonth(entity.DateOfBirth, DateTime.Now);
                if (ageInMonth <= 36)
                    rblAgeGroup.SelectedValue = "36M";
                else
                    rblAgeGroup.SelectedValue = "20Y";

                ddlChartType.SelectedIndex = 0;
            }
        }

        protected void cbpCDCGrowthChartProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            string seriesType = Request.Form[ddlChartType.UniqueID];
            string ageGroup = rblAgeGroup.SelectedValue;
            panel.JSProperties["cpResult"] = GetGrowthChartData(seriesType, ageGroup);
        }

        private string GetGrowthChartData(string seriesType, string ageGroup)
        {
            string title = "";
            switch (seriesType)
            {
                case "W": title = "Weight-for-age percentiles"; break;
                case "H": title = "Length-for-age percentiles"; break;
                case "B": title = "BMI percentiles"; break;
                case "C": title = "Head circumference-for-age percentiles"; break;
            }
            string xLabel = "Age";
            string yLabel = "Point";

            string filterExpression = string.Format("Gender = '{0}' AND AgeGroup = '{1}' AND IsCDC = 1 AND SeriesType = '{2}'", hdnGender.Value, ageGroup, seriesType);
            List<GrowthChartPoint> list = BusinessLayer.GetGrowthChartPointList(filterExpression);

            StringBuilder sbSeries = new StringBuilder();
            StringBuilder sbLegend = new StringBuilder();
            StringBuilder sbActualPoint = new StringBuilder();
            IEnumerable<string> lstSeriesName = (from p in list
                                                 select p.SeriesName).Distinct();

            if (lstSeriesName.Count() > 0)
            {
                sbSeries.Append("[");
                int ctr = 1;
                foreach (string seriesName in lstSeriesName)
                {
                    sbLegend.Append(seriesName);
                    sbSeries.Append(GetSeries(list.Where(p => p.SeriesName == seriesName).ToList()));
                    if (ctr < lstSeriesName.Count())
                    {
                        sbLegend.Append(";");
                        sbSeries.Append(",");
                    }
                    ctr++;
                }
                sbSeries.Append("]");
            }

            List<ChartBase.ChartPoint> lstActualPoint = GetActualPoint(seriesType, ageGroup);
            if (lstActualPoint.Count > 0)
            {
                sbActualPoint.Append("[");
                int ctrActualPoint = 1;
                foreach (ChartBase.ChartPoint actualPoint in lstActualPoint)
                {
                    sbActualPoint.Append("[").Append(actualPoint.XPoint).Append(",").Append(actualPoint.YPoint).Append("]");
                    if (ctrActualPoint < lstActualPoint.Count)
                        sbActualPoint.Append(",");
                    ctrActualPoint++;
                }
                sbActualPoint.Append("]");
            }

            return title + "|" + xLabel + "|" + yLabel + "|" + sbLegend.ToString() + "|" + sbSeries.ToString() + "|" + sbActualPoint.ToString() + "|" + seriesType;
        }

        private List<ChartBase.ChartPoint> GetActualPoint(string seriesType, string ageGroup)
        {
            List<ChartBase.ChartPoint> list = new List<ChartBase.ChartPoint>();
            string parameterCode = "";
            if (seriesType == "W")
                parameterCode = Constant.SettingParameter.VITAL_SIGN_WEIGHT;
            else if(seriesType == "H")
                parameterCode = Constant.SettingParameter.VITAL_SIGN_HEIGHT;
            else if(seriesType == "B")
                parameterCode = Constant.SettingParameter.VITAL_SIGN_BMI;
            else
                parameterCode = Constant.SettingParameter.VITAL_SIGN_HEAD_CIRCUMFERENCE;

            String vitalSignID = BusinessLayer.GetSettingParameter(parameterCode).ParameterValue;

            List<vVitalSignDt> lst = BusinessLayer.GetvVitalSignDtList(string.Format("MRN = {0} AND VitalSignID = {1} AND VitalSignValue != '' AND IsDeleted = 0", AppSession.RegisteredPatient.MRN, vitalSignID));
            Patient patient = BusinessLayer.GetPatient(AppSession.RegisteredPatient.MRN);
            foreach (vVitalSignDt entity in lst)
            {
                if (entity.VitalSignValue != "")
                {
                    int ageInMonth = ((entity.ObservationDate.Year - patient.DateOfBirth.Year) * 12) + entity.ObservationDate.Month - patient.DateOfBirth.Month;
                    if (ageGroup == "36M" && ageInMonth > 36)
                        continue;
                    if (ageGroup == "20Y" && ageInMonth > 240)
                        continue;
                    list.Add(new ChartBase.ChartPoint { XPoint = ageInMonth.ToString(), YPoint = Convert.ToDouble(entity.VitalSignValue) });
                }
            }
            return list;
        }

        private static string GetSeries(List<GrowthChartPoint> list)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            int ctr = 1;
            foreach (GrowthChartPoint obj in list)
            {
                sb.Append("[").Append(obj.Age).Append(",").Append(obj.Point).Append(",").Append(obj.Age).Append("]");
                if (ctr < list.Count)
                    sb.Append(",");
                ctr++;
            }
            sb.Append("]");
            return sb.ToString();
        }
    }
}