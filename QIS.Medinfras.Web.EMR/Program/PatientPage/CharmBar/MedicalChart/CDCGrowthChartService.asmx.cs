using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using QIS.Medinfras.Data.Service;
using System.Text;
using QIS.Medinfras.Data.Service.DataLayer.Base;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.EMR.Program.PatientPage
{
    /// <summary>
    /// Summary description for CDCGrowthChartService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class CDCGrowthChartService : System.Web.Services.WebService
    {
        [WebMethod(EnableSession = true)]
        public object GetGrowthChartData(string seriesType, string ageGroup, string gender)
        {
            string title = "";
            switch (seriesType)
            {
                case "W": title = "Weight-for-age percentiles"; break;
                case "H": title = "Length-for-age percentiles"; break;
                case "C": title = "Head circumference-for-age percentiles"; break;
            }
            string xLabel = "Age";
            string yLabel = "Point";

            string filterExpression = string.Format("Gender = '{0}' AND AgeGroup = '{1}' AND IsCDC = 1 AND SeriesType = '{2}'", gender, ageGroup, seriesType);
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

            return new { Title = title, XLabel = xLabel, YLabel = yLabel, Legend = sbLegend.ToString(), Series = sbSeries.ToString(), ActualPoint = sbActualPoint.ToString() };
            //return title + "|" + xLabel + "|" + yLabel + "|" + sbLegend.ToString() + "|" + sbSeries.ToString() + "|" + sbActualPoint.ToString();
        }

        private List<ChartBase.ChartPoint> GetActualPoint(string seriesType, string ageGroup)
        {
            List<ChartBase.ChartPoint> list = new List<ChartBase.ChartPoint>();
            string parameterCode = "";
            if (seriesType == "W")
                parameterCode = Constant.SettingParameter.VITAL_SIGN_WEIGHT;
            else if (seriesType == "H")
                parameterCode = Constant.SettingParameter.VITAL_SIGN_HEIGHT;
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
