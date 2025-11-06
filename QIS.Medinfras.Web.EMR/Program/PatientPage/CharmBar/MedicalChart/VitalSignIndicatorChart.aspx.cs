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
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class VitalSignIndicatorChart : BasePage
    {
        public string chartData = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                chartData = GetChartDataSample();
            }
        }

        private void GenerateThumbs(int max)
        {
            imgContainer.Controls.Clear();
            for (int i = 0; i < max; ++i)
            {
                imgContainer.Controls.Add(CreateThumbnails(i));
            }
        }

        private HtmlGenericControl CreateThumbnails(int idx)
        {
            HtmlGenericControl div = new HtmlGenericControl("DIV");
            div.Attributes.Add("class", "content");

            HtmlGenericControl divChart = new HtmlGenericControl("DIV");
            divChart.Attributes.Add("id", "thumbChart" + idx);
            div.Controls.Add(divChart);

            HtmlInputHidden inputIdx = new HtmlInputHidden();
            inputIdx.Value = idx.ToString();
            inputIdx.Attributes.Add("class", "indexVal");
            div.Controls.Add(inputIdx);

            HtmlInputHidden inputIdChart = new HtmlInputHidden();
            inputIdChart.Value = "thumbChart" + idx;
            inputIdChart.Attributes.Add("class", "idChart");
            div.Controls.Add(inputIdChart);
            return div;
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

            GenerateThumbs(lstSeriesName.Count());
            if (lstSeriesName.Count() > 0)
            {
                sbSeries.Append("[");
                int ctr = 1;
                foreach (string seriesName in lstSeriesName)
                {
                    sbLegend.Append(seriesName);
                    sbSeries.Append(GetSeriesSample(list.Where(p => p.SeriesName == seriesName).OrderBy(p => p.XPoint).ToList()));
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
            List<vVitalSignDt> lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("MRN = {0} AND VitalSignValue != '' AND VitalSignID IN (SELECT VitalSignID FROM VitalSignType WHERE IsDisplayInChart = 1) AND IsDeleted = 0 ORDER BY VitalSignID ASC", AppSession.RegisteredPatient.MRN));

            if (lstVitalSignDt.Count > 0)
            {
                list = (from p in lstVitalSignDt
                        select new ChartBase.ChartPoint { SeriesName = p.VitalSignLabel, XPoint = p.ObservationTime, YPoint = Convert.ToDouble(p.VitalSignValue) }).ToList();

                IEnumerable<int> lstSeriesID = (from p in lstVitalSignDt
                                                select p.VitalSignID).Distinct();

                StringBuilder sbFilterExpression = new StringBuilder();
                foreach (int seriesID in lstSeriesID)
                {
                    if (sbFilterExpression.ToString() != "")
                        sbFilterExpression.Append(",");
                    sbFilterExpression.Append(seriesID);
                }
                string filterExpression = string.Format("VitalSignID IN ({0}) ORDER BY VitalSignID ASC", sbFilterExpression.ToString());
                List<VitalSignType> lstVitalSignType = BusinessLayer.GetVitalSignTypeList(filterExpression);
                foreach (VitalSignType vitalSignType in lstVitalSignType)
                {
                    if (thumbChartData != "")
                        thumbChartData += ";";
                    thumbChartData += string.Format("{0}^{1}^{2}", vitalSignType.VitalSignLabel, vitalSignType.ChartMinValue, vitalSignType.ChartMaxValue);
                }
            }

            return list;
        }
    }
}