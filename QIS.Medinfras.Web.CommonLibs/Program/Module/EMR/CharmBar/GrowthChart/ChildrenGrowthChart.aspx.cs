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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ChildrenGrowthChart : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (AppSession.RegisteredPatient.GCGender == Constant.Gender.MALE)
                    hdnGender.Value = "1";
                else
                    hdnGender.Value = "2";

                ddlChartType.Items.Add(new ListItem { Text = "Weight-for-age", Value = "W" });
                ddlChartType.Items.Add(new ListItem { Text = "Length-for-age", Value = "H" });
                ddlChartType.Items.Add(new ListItem { Text = "Head Circumference", Value = "C" });
                ddlChartType.Items.Add(new ListItem { Text = "BMI", Value = "B" });

                ddlChartType2.Items.Add(new ListItem { Text = "Weight-for-age", Value = "wfa" });
                ddlChartType2.Items.Add(new ListItem { Text = "Length-for-age", Value = "lfa" });
                ddlChartType2.Items.Add(new ListItem { Text = "Head Circumference", Value = "hca" });
                ddlChartType2.Items.Add(new ListItem { Text = "BMI", Value = "bfa" });
                //ddlChartType2.Items.Add(new ListItem { Text = "Weight-for-length/height", Value = "wfh" });


                int ageInMonth = Function.GetPatientAgeInMonth(AppSession.RegisteredPatient.DateOfBirth, DateTime.Now);
                if (ageInMonth <= 36)
                    rblAgeGroup.SelectedValue = "36M";
                else
                    rblAgeGroup.SelectedValue = "20Y";

                if (ageInMonth <= 3)
                    rblAgeGroup2.SelectedValue = "W01";
                else if (ageInMonth > 3 && ageInMonth < 36)
                    rblAgeGroup.SelectedValue = "W02";
                else
                    rblAgeGroup.SelectedValue = "W03";

                ddlChartType.SelectedIndex = 0;
                ddlChartType2.SelectedIndex = 0;
            }
        }

        protected void cbpCDCGrowthChartProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            string seriesType = Request.Form[ddlChartType.UniqueID];
            string ageGroup = rblAgeGroup.SelectedValue;
            panel.JSProperties["cpResult"] = GetGrowthChartData(seriesType, ageGroup);
        }

        protected void cbpWHOGrowthChartProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            string seriesType = Request.Form[ddlChartType2.UniqueID];
            string ageGroup = rblAgeGroup2.SelectedValue;
            panel.JSProperties["cpResult"] = GetWHOGrowthChartData(seriesType, ageGroup);
        }

        private string GetGrowthChartData(string seriesType, string ageGroup, bool isCDC = true)
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

        private string GetWHOGrowthChartData(string seriesType, string ageGroup, bool isCDC = true)
        {
            string title = "";
            string xLabel = "Age";
            string yLabel = "Point";

            switch (seriesType)
            {
                case "wfa": title = "Weight-for-age"; yLabel = "Weight (kg)"; break;
                case "lfa": title = "Length-for-age"; yLabel = "Length"; break;
                case "bfa": title = "BMI"; break;
                case "hca": title = "Head circumference-for-age"; break;
                case "wfh": title = "Weight-for-length/height"; break;
            }

            string filterExpression = string.Format("Gender = '{0}' AND AgeGroup = 'W00' AND IsCDC = 0 AND SeriesType = '{1}'", hdnGender.Value, seriesType);
            double fromAgeInDay = 0;
            double toAgeInDay = 0;
            switch (ageGroup)
            {
                //Since age value in data is in day
                case "W01" : // birth to 6 months
                    toAgeInDay = 180; //6 months * 30
                    filterExpression += string.Format(" AND (Age BETWEEN {0} AND {1})", fromAgeInDay, toAgeInDay);
                    xLabel = "Age (months)";
                    break;
                case "W02": // birth to 2 years
                    fromAgeInDay = 0; 
                    toAgeInDay = 720; 
                    filterExpression += string.Format(" AND (Age BETWEEN {0} AND {1})", 0, toAgeInDay);
                    xLabel = "Age (months)";
                    break;
                case "W03": // 6 months to 2 years
                    fromAgeInDay = 180;
                    toAgeInDay = 720;
                    filterExpression += string.Format(" AND (Age BETWEEN {0} AND {1})", 0, toAgeInDay);
                    xLabel = "Age (months)";
                    break;
                case "W04": // 2 years to 5 years
                    fromAgeInDay = 720;
                    toAgeInDay = 1800;
                    filterExpression += string.Format(" AND (Age BETWEEN {0} AND {1})", 0, toAgeInDay);
                    xLabel = "Age (months)";
                    break;
                case "W05": // Birth to 5 years
                    fromAgeInDay = 0;
                    toAgeInDay = 1800;
                    filterExpression += string.Format(" AND (Age BETWEEN {0} AND {1})", 0, toAgeInDay);
                    xLabel = "Age (completed months and years)";
                    break;
                default:
                    toAgeInDay = 180; //6 months * 30
                    filterExpression += string.Format(" AND (Age BETWEEN {0} AND {1})", fromAgeInDay, toAgeInDay);
                    xLabel = "Age (months)";
                    break;
            }

            List<GrowthChartPoint> list = BusinessLayer.GetGrowthChartPointList(filterExpression + " ORDER BY SeriesOrder");

            StringBuilder sbSeries = new StringBuilder();
            StringBuilder sbLegend = new StringBuilder();
            StringBuilder sbSeriesColor = new StringBuilder();
            StringBuilder sbActualPoint = new StringBuilder();

            IEnumerable<string> lstSeriesName = (from p in list
                                                 select p.SeriesName+"|"+p.SeriesColor).Distinct();


            if (lstSeriesName.Count() > 0)
            {
                sbSeries.Append("[");
                int ctr = 1;
                foreach (string seriesName in lstSeriesName)
                {
                    string[] seriesNameInfo = seriesName.Split('|');

                    sbLegend.Append(seriesNameInfo[0]);
                    sbSeriesColor.Append(seriesNameInfo[1]);
                    sbSeries.Append(GetWHOSeries(list.Where(p => p.SeriesName == seriesNameInfo[0]).ToList(), 2));
                    if (ctr < lstSeriesName.Count())
                    {
                        sbLegend.Append(";");
                        sbSeries.Append(",");
                        sbSeriesColor.Append(";");
                    }
                    ctr++;
                }
                sbSeries.Append("]");

            }

            List<ChartBase.ChartPoint> lstActualPoint = GetWHOActualPoint(seriesType, ageGroup);
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

            return title + "|" + xLabel + "|" + yLabel + "|" + sbLegend.ToString() + "|" + sbSeries.ToString() + "|" + sbActualPoint.ToString() + "|" + seriesType + "|" + sbSeriesColor.ToString() + "|" + AppSession.RegisteredPatient.GCGender;
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

        private List<ChartBase.ChartPoint> GetWHOActualPoint(string seriesType, string ageGroup)
        {
            List<ChartBase.ChartPoint> list = new List<ChartBase.ChartPoint>();
            string parameterCode = "";
            if (seriesType == "wfa")
                parameterCode = Constant.SettingParameter.VITAL_SIGN_WEIGHT;
            else if (seriesType == "H")
                parameterCode = Constant.SettingParameter.VITAL_SIGN_HEIGHT;
            else if (seriesType == "B")
                parameterCode = Constant.SettingParameter.VITAL_SIGN_BMI;
            else
                parameterCode = Constant.SettingParameter.VITAL_SIGN_HEAD_CIRCUMFERENCE;

            String vitalSignID = BusinessLayer.GetSettingParameter(parameterCode).ParameterValue;

            List<GrowthChartPointData> lst = BusinessLayer.GetGrowthChartPointDataList(AppSession.RegisteredPatient.MRN, Convert.ToInt32(vitalSignID));
            foreach (GrowthChartPointData entity in lst)
            {
                if (entity.VitalSignValue != "")
                {
                    double ageInDays = (entity.ObservationDate - entity.DateOfBirth).TotalDays;
                    double ageInMonth = (ageInDays / 30.4375);   // 1 month = 30.4375 days
                    double ageInYear = ageInMonth / 12;

                    if (ageGroup == "W01" && ageInMonth > 13)
                        continue;
                    if (ageGroup == "W02" && ageInMonth > 104)
                        continue;
                    if (ageGroup == "20Y" && ageInMonth > 260)
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

        /// <summary>
        /// Get Standard Data based on Age Unit
        /// </summary>
        /// <param name="list"></param>
        /// <param name="ageUnit">1 = day (default), 2 = month, 3 = year</param>
        /// <returns></returns>
        private static string GetWHOSeries(List<GrowthChartPoint> list, int ageUnit = 1)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            int ctr = 1;
            foreach (GrowthChartPoint obj in list)
            {
                decimal age = 0;
                switch (ageUnit)
                {
                    case 1:
                        age = obj.Age;
                        break;
                    case 2:
                        // 1 month = 30.4375 days
                        age = Math.Round((obj.Age / Convert.ToDecimal(30.4375)),2);
                        break;
                    case 3:
                        age = Math.Round((obj.Age / Convert.ToDecimal(30.4375 * 12)),2);
                        break;
                    default:
                        break;
                }
                sb.Append("[").Append(age).Append(",").Append(obj.Point).Append(",").Append(age).Append("]");
                if (ctr < list.Count)
                    sb.Append(",");
                ctr++;
            }
            sb.Append("]");
            return sb.ToString();
        }
    }
}