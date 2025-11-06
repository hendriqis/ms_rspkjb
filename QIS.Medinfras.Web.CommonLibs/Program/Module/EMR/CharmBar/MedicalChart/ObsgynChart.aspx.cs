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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ObsgynChart : BasePage
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

                string filterExpression = string.Format("MRN = {0} AND IsDeleted = 0",AppSession.RegisteredPatient.MRN);
                List<Antenatal> lstAntenatal = BusinessLayer.GetAntenatalList(filterExpression);
                if (lstAntenatal.Count > 0)
                {
                    foreach (Antenatal item in lstAntenatal)
                    {
                        ddlPregnancyNo.Items.Add(new ListItem { Text = item.PregnancyNo.ToString(), Value = item.PregnancyNo.ToString() });
                    }
                }
                else
                {
                    ddlPregnancyNo.Items.Add(new ListItem { Text = "1", Value = "1" });
                }

                ddlPregnancyNo.SelectedIndex = 0;

                ddlChartType.Items.Add(new ListItem { Text = "Abdominal Circumference", Value = "AC" });
                ddlChartType.Items.Add(new ListItem { Text = "Biparietal Diameter", Value = "BPD" });
                ddlChartType.Items.Add(new ListItem { Text = "Humerus Length", Value = "HL" });
                ddlChartType.Items.Add(new ListItem { Text = "Femur Length", Value = "FL" });
                ddlChartType.Items.Add(new ListItem { Text = "Head Circumference", Value = "HC" });
                ddlChartType.Items.Add(new ListItem { Text = "Estimated Fetal Weight", Value = "EFW" });
                ddlChartType.Items.Add(new ListItem { Text = "Occipotofrontal Diameter", Value = "OFD" });
                
                int ageInMonth = Function.GetPatientAgeInMonth(entity.DateOfBirth, DateTime.Now);
                if (ageInMonth <= 36)
                    rblAgeGroup.SelectedValue = "36M";
                else
                    rblAgeGroup.SelectedValue = "20Y";

                ddlChartType.SelectedIndex = 0;
            }
        }

        protected void cbpChartProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            string seriesType = Request.Form[ddlChartType.UniqueID];
            string ageGroup = rblAgeGroup.SelectedValue;
            panel.JSProperties["cpResult"] = GetChartData(seriesType, ageGroup);
        }

        private string GetChartData(string seriesType, string ageGroup)
        {
            string title = "";
            switch (seriesType)
            {
                case "AC": title = "Abdominal Circumference"; break;
                case "BPD": title = "Biparietal Diameter"; break;
                case "HL": title = "Humerus Length"; break;
                case "FL": title = "Femur Length"; break;
                case "HC": title = "Head Circumference"; break;
                case "EFW": title = "Estimated Fetal Weight"; break;
                case "OFD": title = "Occipotofrontal Diameter"; break;
            }
            string xLabel = "Age";
            string yLabel = "Point";

            string filterExpression = string.Format("SeriesType = '{0}'", seriesType);
            List<ObstetricChartPoint> list = BusinessLayer.GetObstetricChartPointList(filterExpression);

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

            List<ChartBase.ChartPoint> lstActualPoint = GetActualPoint(seriesType);
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

        private List<ChartBase.ChartPoint> GetActualPoint(string seriesType)
        {
            List<ChartBase.ChartPoint> list = new List<ChartBase.ChartPoint>();

            List<vFetalMeasurement> lst = BusinessLayer.GetvFetalMeasurementList(string.Format("MRN = {0} AND PregnancyNo = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.MRN, ddlPregnancyNo.SelectedValue));
            foreach (vFetalMeasurement entity in lst)
            {
                decimal value = 0;
                switch (seriesType)
                {
                    case "AC":
                        value = entity.AC;
                        break;
                    case "BPD":
                        value = entity.BPD;
                        break;
                    case "HL":
                        value = entity.HL;
                        break;
                    case "HC":
                        value = entity.HC;
                        break;
                    case "FL":
                        value = entity.FL;
                        break;
                    case "EFW":
                        value = entity.EFW;
                        break;
                    case "OFD":
                        value = entity.OFD;
                        break;
                    case "CRL":
                        value = entity.CRL;
                        break;
                    case "FHR":
                        value = entity.FHR;
                        break;
                    case "GS":
                        value = entity.GS;
                        break;
                    default:
                        value = 0;
                        break;
                }
                if (value != 0)
                {
                    list.Add(new ChartBase.ChartPoint { XPoint = entity.GestationalWeek.ToString(), YPoint = Convert.ToDouble(value) });
                }
            }
            return list;
        }

        private static string GetSeries(List<ObstetricChartPoint> list)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            int ctr = 1;
            foreach (ObstetricChartPoint obj in list)
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