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
using QIS.Data.Core.Dal;
using System.Text;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class LaboratoryResultList2 : BasePage
    {
        protected int PageCount = 1;
        protected string viewerUrl = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                txtPeriodFrom.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtPeriodTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

                hdnMedicalNoCBCtl.Value = AppSession.RegisteredPatient.MedicalNo;
                hdnMRNCBCtl.Value = AppSession.RegisteredPatient.MRN.ToString();
            }
        }

        #region Header
        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string startDate = Helper.GetDatePickerValue(txtPeriodFrom).ToString(Constant.FormatString.DATE_FORMAT_112);
            string endDate = Helper.GetDatePickerValue(txtPeriodTo).ToString(Constant.FormatString.DATE_FORMAT_112);
            Int32 mrn = Convert.ToInt32(hdnMRNCBCtl.Value);
            Int32 itemID = Convert.ToInt32(hdnItemID.Value);

            List<GetDistinctFraction> lstEntity = BusinessLayer.GetDistinctFraction(startDate, endDate, mrn, itemID, 0);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        #region Detail
        private void BindGridViewDt(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string startDate = Helper.GetDatePickerValue(txtPeriodFrom).ToString(Constant.FormatString.DATE_FORMAT_112);
            string endDate = Helper.GetDatePickerValue(txtPeriodTo).ToString(Constant.FormatString.DATE_FORMAT_112);
            Int32 mrn = Convert.ToInt32(hdnMRNCBCtl.Value);
            Int32 itemID = Convert.ToInt32(hdnItemID.Value);
            Int32 fractionID = Convert.ToInt32(hdnIDCBCtl.Value);

            List<GetDistinctFractionPerDetail> lstEntity = BusinessLayer.GetDistinctFractionPerDetail(startDate, endDate, mrn, itemID, fractionID, 0);
            List<PartialData> lstData = new List<PartialData>();
            List<FractionValue> lstValue = new List<FractionValue>();

            PartialData entity = new PartialData();
            entity.SequenceNo = 1;
            entity.cfCreatedDate = lstEntity.FirstOrDefault().cfCreatedDate;

            foreach (GetDistinctFractionPerDetail e in lstEntity)
            {
                FractionValue obj = new FractionValue();
                obj.SequenceNo = 1;
                obj.IsNormal = e.IsNormal;
                obj.Fractionvalue = e.ResultValue;
                obj.MetricUnitName = e.MetricUnitName;
                lstValue.Add(obj);
            }

            entity.FractionValue = lstValue;
            lstData.Add(entity);

            lvwViewDt.DataSource = lstData;
            lvwViewDt.DataBind();

            rptFractionDateHeader.DataSource = lstEntity;
            rptFractionDateHeader.DataBind();
        }

        protected void cbpViewDt_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewDt(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void lvwViewDt_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                PartialData obj = (PartialData)e.Item.DataItem;
                Repeater rptFractionDetailValue = (Repeater)e.Item.FindControl("rptFractionDetailValue");
                rptFractionDetailValue.DataSource = obj.FractionValue;
                rptFractionDetailValue.DataBind();
            }
        }
        #endregion
    }

    public class PartialData
    {
        private int _SequenceNo;

        public int SequenceNo
        {
            get { return _SequenceNo; }
            set { _SequenceNo = value; }
        }

        private string _cfCreatedDate;

        public string cfCreatedDate
        {
            get { return _cfCreatedDate; }
            set { _cfCreatedDate = value; }
        }

        private List<FractionValue> _FractionValue;

        public List<FractionValue> FractionValue
        {
            get { return _FractionValue; }
            set { _FractionValue = value; }
        }
    }

    public partial class FractionValue
    {
        public int SequenceNo;
        public bool IsNormal { get; set; }
        public String Fractionvalue { get; set; }
        public String MetricUnitName { get; set; }
    }
}