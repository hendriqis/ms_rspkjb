using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using System.Text;
using QIS.Medinfras.Data.Service.DataLayer.Base;

namespace QIS.Medinfras.Web.EmergencyCare.Program
{
    public partial class EpisodeSummaryMedicalChartContainerCtl : BaseViewPopupCtl
    {
        public string chartData = "";
        public override void InitializeDataControl(string queryString)
        {
            chartData = GetChartDataSample();
        }

        public string GetChartDataSample()
        {
            ChartBase.ChartInfo chartInfo = new ChartBase.ChartInfo { Title = "Medical Chart", XLabel = "Period", YLabel = "Point" };

            //Title, yMin, yMax
            //string thumbChartData = "Temperature^36.5^40;Systolic^95^140;Diastolic^60^120;";
            //thumbChartData += "HDL^0^200;LDL^0^200;Triglycerids^0^400;Cholestrol^0^400";
            string thumbChartData = "";

            List<ChartBase.ChartPoint> list = GetList(ref thumbChartData);
            StringBuilder sbSeries = new StringBuilder();
            StringBuilder sbLegend = new StringBuilder();
            IEnumerable<string> lstSeriesName = (from p in list
                                                 select p.SeriesName).Distinct();

            if (lstSeriesName.Count() > 0)
            {
                sbSeries.Append("[");
                int ctr = 1;
                foreach (string seriesName in lstSeriesName)
                {
                    sbLegend.Append(seriesName);
                    sbSeries.Append(GetSeriesSample(list.Where(p => p.SeriesName == seriesName).ToList()));
                    if (ctr < lstSeriesName.Count())
                    {
                        sbLegend.Append(";");
                        sbSeries.Append(",");
                    }
                    else
                        break;
                    ctr++;
                }
                sbSeries.Append("]");
            }
            return thumbChartData + "|" + chartInfo.Title + "|" + chartInfo.XLabel + "|" + chartInfo.YLabel + "|" + sbLegend.ToString() + "|" + sbSeries.ToString();
            //return "[[[1,1],[2,2],[3,2.1],[4,3]] , [[1,4],[2,3],[3,2.5],[4,1]]]";
        }

        private static string GetSeriesSample(List<ChartBase.ChartPoint> list)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            int ctr = 1;
            foreach (ChartBase.ChartPoint obj in list)
            {
                sb.Append("['").Append(obj.XPoint).Append("',").Append(obj.YPoint).Append(",").Append(obj.YPoint).Append("]");
                if (ctr < list.Count)
                    sb.Append(",");
                ctr++;
            }
            sb.Append("]");
            return sb.ToString();
        }

        private List<ChartBase.ChartPoint> GetList(ref string thumbChartData)
        {
            List<ChartBase.ChartPoint> list = new List<ChartBase.ChartPoint>();
            List<vVitalSignDt> lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("MRN = {0} AND VitalSignValue != '' AND VitalSignID IN (SELECT VitalSignID FROM VitalSignType WHERE IsDisplayInChart = 1) AND IsDeleted = 0", AppSession.RegisteredPatient.MRN));

            list = (from p in lstVitalSignDt
                    select new ChartBase.ChartPoint { SeriesName = p.VitalSignLabel, XPoint = p.ObservationDate.ToString("yyyy-MM-dd"), YPoint = Convert.ToDouble(p.VitalSignValue) }).ToList();

            IEnumerable<int> lstSeriesID = (from p in lstVitalSignDt
                                            select p.VitalSignID).Distinct();

            StringBuilder sbFilterExpression = new StringBuilder();
            foreach (int seriesID in lstSeriesID)
            {
                if (sbFilterExpression.ToString() != "")
                    sbFilterExpression.Append(",");
                sbFilterExpression.Append(seriesID);
            }
            string filterExpression = string.Format("VitalSignID IN ({0})", sbFilterExpression.ToString());
            List<VitalSignType> lstVitalSignType = BusinessLayer.GetVitalSignTypeList(filterExpression);
            foreach (VitalSignType vitalSignType in lstVitalSignType)
            {
                if (thumbChartData != "")
                    thumbChartData += ";";
                thumbChartData += string.Format("{0}^{1}^{2}", vitalSignType.VitalSignLabel, vitalSignType.MinNormalValue, vitalSignType.MaxNormalValue);
            }

            return list;
        }
    }
}