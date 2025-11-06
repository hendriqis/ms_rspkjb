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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class VitalSignChart : BasePage
    {
        public string chartData = "";
        protected int VitalSignChartPageCount = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                hdnMinDate.Value = String.Format("{0} {1}", AppSession.RegisteredPatient.VisitDate.ToString(Constant.FormatString.DATE_FORMAT_2), "00:00");
                chartData = GetChartData();

                BindGridView(1, true, ref VitalSignChartPageCount);

                //List<VisitVitalSignSummary> lst1 = BusinessLayer.GetVisitVitalSignSummaryList(AppSession.RegisteredPatient.VisitID);
                //lvwViewVS.DataSource = lst1;
                //lvwViewVS.DataBind();
            }
        }

        private void GenerateThumbs(int max)
        {
            imgContainer.Controls.Clear();
            for (int i = 0; i <= max; ++i)
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

        public string GetChartData()
        {
            ChartBase.ChartInfo chartInfo = new ChartBase.ChartInfo { Title = "Tanda Vital dan Indikator Lainnya", XLabel = "Period", YLabel = "Point" };

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
                    sbSeries.Append(GetSeriesData(list.Where(p => p.SeriesName == seriesName).OrderBy(p => p.XPoint).ToList()));
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

        private static string GetSeriesData(List<ChartBase.ChartPoint> list)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            int ctr = 1;
            foreach (ChartBase.ChartPoint obj in list)
            {
                //sb.Append(string.Format("['{0}',{1},{2},'{3}']", obj.XPoint, obj.YPoint, obj.YPoint, obj.DataLabel));
                sb.Append(string.Format("['{0}',{1},{2}]", obj.XPoint, obj.YPoint, obj.YPoint));
                if (ctr < list.Count)
                    sb.Append(",");
                ctr++;
            }
            sb.Append("]");
            return sb.ToString();
        }

        private List<ChartBase.ChartPoint> GetList(ref string thumbChartData)
        {
            string filterExpression = string.Format("MRN = {0} AND VisitID = {1} AND VitalSignValue != '' AND VitalSignID IN (SELECT VitalSignID FROM VitalSignType WHERE IsDisplayInChart = 1) AND ISNUMERIC(VitalSignValue) = 1 AND IsDeleted = 0 ORDER BY VitalSignID ASC", AppSession.RegisteredPatient.MRN, AppSession.RegisteredPatient.VisitID);

            if (rblItemType.SelectedValue == "2")
                filterExpression = string.Format("MRN = {0} AND VitalSignValue != '' AND VitalSignID IN (SELECT VitalSignID FROM VitalSignType WHERE IsDisplayInChart = 1) AND ISNUMERIC(VitalSignValue) = 1 AND IsDeleted = 0 ORDER BY VitalSignID ASC", AppSession.RegisteredPatient.MRN, AppSession.RegisteredPatient.VisitID);

            List<ChartBase.ChartPoint> list = new List<ChartBase.ChartPoint>();
            List<vVitalSignDt> lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(filterExpression);

            if (lstVitalSignDt.Count > 0)
            {
                //list = (from p in lstVitalSignDt
                //        select new ChartBase.ChartPoint { SeriesName = p.VitalSignLabel, XPoint = string.Format("{0} {1}", p.ObservationDate.ToString("yyyy-MM-dd"), p.ObservationTime), YPoint = Convert.ToDouble(p.VitalSignValue), DataLabel = string.Format("{0};{1}", (string.IsNullOrEmpty(p.Remarks) ? "-" : p.Remarks.Replace("'"," ")), p.VitalSignValue) }).ToList();

                list = (from p in lstVitalSignDt
                        select new ChartBase.ChartPoint { SeriesName = p.VitalSignLabel, XPoint = string.Format("{0} {1}", p.ObservationDate.ToString("yyyy-MM-dd"), p.ObservationTime), YPoint = Convert.ToDouble(p.VitalSignValue), DataLabel = string.Format("{0}",string.IsNullOrEmpty(p.Remarks) ? "-" : p.Remarks.Replace("'", " ").Replace("\r", "").Replace("\n", " ")) }).ToList();


                IEnumerable<int> lstSeriesID = (from p in lstVitalSignDt
                                                select p.VitalSignID).Distinct();

                StringBuilder sbFilterExpression = new StringBuilder();
                foreach (int seriesID in lstSeriesID)
                {
                    if (sbFilterExpression.ToString() != "")
                        sbFilterExpression.Append(",");
                    sbFilterExpression.Append(seriesID);
                }

                filterExpression = string.Format("VitalSignID IN ({0}) ORDER BY VitalSignID ASC", sbFilterExpression.ToString());
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

        protected void lvwViewVS_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                VisitVitalSignSummary entity = e.Item.DataItem as VisitVitalSignSummary;
                TextBox txtVSAverageValue = e.Item.FindControl("txtVSAverageValue") as TextBox;
                TextBox txtVSMinValue = e.Item.FindControl("txtVSMinValue") as TextBox;
                TextBox txtVSMaxValue = e.Item.FindControl("txtVSMaxValue") as TextBox;
                TextBox txtVSLastValue = e.Item.FindControl("txtVSLastValue") as TextBox;
                TextBox txtVSLastDate = e.Item.FindControl("txtVSLastDate") as TextBox;
                txtVSAverageValue.Text = entity.AvgValue.ToString("N");
                txtVSMinValue.Text = entity.MinValue.ToString("N");
                txtVSMaxValue.Text = entity.MaxValue.ToString("N");
                txtVSLastValue.Text = entity.LatestValue;
                txtVSLastDate.Text = entity.LatestDate.ToString(Constant.FormatString.DATE_FORMAT);
            }
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            int cvLinkedID = 0;
            if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.INPATIENT)
            {
                vConsultVisit4 entityLinkedRegistration = BusinessLayer.GetvConsultVisit4List(string.Format("RegistrationID = (SELECT LinkedRegistrationID FROM Registration WHERE RegistrationID = {0})", AppSession.RegisteredPatient.RegistrationID)).FirstOrDefault();
                if (entityLinkedRegistration != null)
                {
                    cvLinkedID = entityLinkedRegistration.VisitID;
                }
            }

            string filterExpression = ""; //hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";

            filterExpression += string.Format("VisitID IN ({0},{1}) AND (Remarks IS NOT NULL AND Remarks != '') AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, cvLinkedID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvVitalSignHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vVitalSignHd> lstEntity = BusinessLayer.GetvVitalSignHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");
            grdVitalSignView.DataSource = lstEntity;
            grdVitalSignView.DataBind();
        }

        protected void cbpVitalSignView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref VitalSignChartPageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridView(1, true, ref VitalSignChartPageCount);
                    result = "refresh|" + VitalSignChartPageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}