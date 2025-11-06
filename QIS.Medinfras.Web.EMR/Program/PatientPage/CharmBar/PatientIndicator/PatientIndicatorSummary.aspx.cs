using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using System.Text;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class PatientIndicatorSummary : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                List<vVitalSignDtInformation> lst1 = BusinessLayer.GetvVitalSignDtInformationList(string.Format("MRN = {0}", AppSession.RegisteredPatient.MRN));
                lvwViewVS.DataSource = lst1;
                lvwViewVS.DataBind();

                List<vLaboratoryResultSummary> lst2 = BusinessLayer.GetvLaboratoryResultSummaryList(string.Format("MRN = {0}", AppSession.RegisteredPatient.MRN));
                lvwView.DataSource = lst2;
                lvwView.DataBind();
            }
        }

        protected void lvwViewVS_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vVitalSignDtInformation entity = e.Item.DataItem as vVitalSignDtInformation;
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

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vLaboratoryResultSummary entity = e.Item.DataItem as vLaboratoryResultSummary;
                TextBox txtAverageValue = e.Item.FindControl("txtAverageValue") as TextBox;
                TextBox txtMinValue = e.Item.FindControl("txtMinValue") as TextBox;
                TextBox txtMaxValue = e.Item.FindControl("txtMaxValue") as TextBox;
                TextBox txtLastValue = e.Item.FindControl("txtLastValue") as TextBox;
                TextBox txtLastDate = e.Item.FindControl("txtLastDate") as TextBox;
                txtAverageValue.Text = entity.AVGMetric.ToString("N");
                txtMinValue.Text = entity.MINMetric.ToString("N");
                txtMaxValue.Text = entity.MAXMetric.ToString("N");

                //GET LABORATORY LATEST VALUE
                string filterExpression = string.Format("MRN = {0} AND ItemID = {1} AND FractionID = {2} AND IsDeleted = 0", entity.MRN, entity.ItemID, entity.FractionID);
                vLaboratoryResultDt resultInfo = BusinessLayer.GetvLaboratoryResultDtList(filterExpression, 1, 1, "ID DESC").FirstOrDefault();
                if (resultInfo != null)
                {
                    txtLastValue.Text = resultInfo.MetricResultValue.ToString("N");
                    txtLastDate.Text = resultInfo.ResultDate.ToString(Constant.FormatString.DATE_FORMAT);
                }
            }
        }
    }
}