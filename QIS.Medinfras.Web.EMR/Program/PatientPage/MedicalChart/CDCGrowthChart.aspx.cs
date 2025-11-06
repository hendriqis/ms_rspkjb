using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Data.Service.DataLayer.Base;
using System.Text;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class CDCGrowthChart : BasePagePatientPageList
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.CDC_GROWTH_CHART;
        }

        protected override void InitializeDataControl()
        {
            Patient entity = BusinessLayer.GetPatient(AppSession.RegisteredPatient.MRN);
            if (entity.GCGender == Constant.Gender.MALE)
                hdnGender.Value = "1";
            else
                hdnGender.Value = "2";

            ddlChartType.Items.Add(new ListItem { Text = "Weight-for-age", Value = "W" });
            ddlChartType.Items.Add(new ListItem { Text = "Length-for-age", Value = "H" });
            ddlChartType.Items.Add(new ListItem { Text = "Head Circumference", Value = "C" });

            ddlChartType.SelectedIndex = 0;
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

            return title + "|" + xLabel + "|" + yLabel + "|" + sbLegend.ToString() + "|" + sbSeries.ToString() + "|" + sbActualPoint.ToString();
        }

        private List<ChartBase.ChartPoint> GetActualPoint(string seriesType, string ageGroup)
        {
            List<ChartBase.ChartPoint> list = new List<ChartBase.ChartPoint>();
            //if (seriesType == "W")
            //{
            //    list.Add(new ChartBase.ChartPoint { XPoint = 4, YPoint = 7.2 });
            //    list.Add(new ChartBase.ChartPoint { XPoint = 6, YPoint = 7.05 });
            //    list.Add(new ChartBase.ChartPoint { XPoint = 8, YPoint = 8.35 });
            //    list.Add(new ChartBase.ChartPoint { XPoint = 10, YPoint = 8.85 });
            //    list.Add(new ChartBase.ChartPoint { XPoint = 12, YPoint = 9.80 });
            //    list.Add(new ChartBase.ChartPoint { XPoint = 14, YPoint = 10.45 });
            //    list.Add(new ChartBase.ChartPoint { XPoint = 16, YPoint = 11.7 });
            //    list.Add(new ChartBase.ChartPoint { XPoint = 18, YPoint = 12.6 });
            //    list.Add(new ChartBase.ChartPoint { XPoint = 20, YPoint = 12.35 });
            //    list.Add(new ChartBase.ChartPoint { XPoint = 22, YPoint = 12.6 });
            //}
            //else if (seriesType == "C")
            //{
            //    list.Add(new ChartBase.ChartPoint { XPoint = 4, YPoint = 45 });
            //    list.Add(new ChartBase.ChartPoint { XPoint = 6, YPoint = 45 });
            //    list.Add(new ChartBase.ChartPoint { XPoint = 8, YPoint = 46 });
            //    list.Add(new ChartBase.ChartPoint { XPoint = 10, YPoint = 47 });
            //    list.Add(new ChartBase.ChartPoint { XPoint = 12, YPoint = 48 });
            //    list.Add(new ChartBase.ChartPoint { XPoint = 14, YPoint = 49 });
            //    list.Add(new ChartBase.ChartPoint { XPoint = 16, YPoint = 49 });
            //    list.Add(new ChartBase.ChartPoint { XPoint = 18, YPoint = 50.5 });
            //    list.Add(new ChartBase.ChartPoint { XPoint = 20, YPoint = 50.5 });
            //    list.Add(new ChartBase.ChartPoint { XPoint = 22, YPoint = 50.5 });
            //}
            //else if (seriesType == "H")
            //{
            //    list.Add(new ChartBase.ChartPoint { XPoint = 4, YPoint = 70 });
            //    list.Add(new ChartBase.ChartPoint { XPoint = 6, YPoint = 70 });
            //    list.Add(new ChartBase.ChartPoint { XPoint = 8, YPoint = 72 });
            //    list.Add(new ChartBase.ChartPoint { XPoint = 10, YPoint = 76 });
            //    list.Add(new ChartBase.ChartPoint { XPoint = 12, YPoint = 79 });
            //    list.Add(new ChartBase.ChartPoint { XPoint = 14, YPoint = 80 });
            //    list.Add(new ChartBase.ChartPoint { XPoint = 16, YPoint = 83 });
            //    list.Add(new ChartBase.ChartPoint { XPoint = 18, YPoint = 88.5 });
            //    list.Add(new ChartBase.ChartPoint { XPoint = 20, YPoint = 89.5 });
            //    list.Add(new ChartBase.ChartPoint { XPoint = 22, YPoint = 90 });
            //}
            string parameterCode = "";
            if (seriesType == "W")
                parameterCode = Constant.SettingParameter.VITAL_SIGN_WEIGHT;
            else if(seriesType == "H")
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