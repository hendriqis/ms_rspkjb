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
using System.Dynamic;
using System.Data;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class LaboratoryResultList1 : BasePage
    {
        protected int PageCount = 1;
        protected int PageCount2 = 1;
        protected static List<PatientLaboratoryResult> lstResult;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                List<SettingParameter> setvar = BusinessLayer.GetSettingParameterList(String.Format("ParameterCode IN ('{0}')",
                        Constant.SettingParameter.LB_IS_PREVIEW_RESULT_AFTER_PROPOSED_RESULT
                        ));

                hdnIsShowResultAfterProposed.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.LB_IS_PREVIEW_RESULT_AFTER_PROPOSED_RESULT).FirstOrDefault().ParameterValue;

                txtPeriodFrom.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtPeriodTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

                hdnHealthcareServiceUnitIDCBCtl.Value = BusinessLayer.GetHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND ServiceUnitID = {1}", AppSession.UserLogin.HealthcareID, BusinessLayer.GetSettingParameter(Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID).ParameterValue))[0].HealthcareServiceUnitID.ToString();
                hdnDocumentPathCBCtl.Value = string.Format(@"{0}/{1}/", AppConfigManager.QISVirtualDirectory, AppConfigManager.QISPatientDocumentsPath.Replace("#MRN", AppSession.RegisteredPatient.MedicalNo));

                hdnMRNCBCtl.Value = AppSession.RegisteredPatient.MRN.ToString();
                hdnVisitIDCBCtl.Value = AppSession.RegisteredPatient.VisitID.ToString();
                hdnRegistrationIDCBCtl.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
                hdnIsBridgingToLIS.Value = AppSession.IsBridgingToLIS.ToString();
                hdnRISWebViewURL.Value = AppSession.RIS_WEB_VIEW_URL;
                hdnRISHL7MessageFormat.Value = AppSession.RIS_HL7_MESSAGE_FORMAT;
                hdnRISConsumerPassword.Value = AppSession.RIS_Consumer_Pwd;

                BindGridView(1, true, ref PageCount);
                BindGridViewDt(1, true, ref PageCount);

                BindGridView2(1, true, ref PageCount2);
                //BindGridViewDt2(1, true, ref PageCount2);
            }
        }

        protected string GetISBridgingToLIS()
        {
            return hdnIsBridgingToLIS.Value;
        }

        #region Header
        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpressionCBCtl.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            
            filterExpression += string.Format("MRN = {0} AND IsLaboratoryUnit = 1 AND GCTransactionStatus != '{1}'", hdnMRNCBCtl.Value, Constant.TransactionStatus.VOID);

            if (rblItemType.SelectedValue == "1")
            {
                filterExpression += string.Format(" AND VisitID = {0}", hdnVisitIDCBCtl.Value.ToString());
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientChargesHdLaboratoryResultRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_LIMA_PULUH);
            }

            List<vPatientChargesHdLaboratoryResult> lstEntity = BusinessLayer.GetvPatientChargesHdLaboratoryResultList(filterExpression, Constant.GridViewPageSize.GRID_LIMA_PULUH, pageIndex, "TransactionID DESC");
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
            string filterExpression = "1 = 0";
            string orderBy = "FractionDisplayOrder";

            if (hdnIDCBCtl.Value != "")
            {
                filterExpression = string.Format("ChargeTransactionID = {0}", hdnIDCBCtl.Value);

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvLaboratoryResultDtRowCount(filterExpression, orderBy);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_LIMA_PULUH);
                }
            }

            List<vLaboratoryResultDt> lstEntity = BusinessLayer.GetvLaboratoryResultDtList(filterExpression, Constant.GridViewPageSize.GRID_LIMA_PULUH, pageIndex, orderBy);
            grdViewDt.DataSource = lstEntity;
            grdViewDt.DataBind();

            GridViewHelper helper = new GridViewHelper(grdViewDt);
            helper.RegisterGroup("ItemGroupName1", true, true);
            helper.ApplyGroupSort();
        }

        protected void grdViewDt_Sorting(object sender, GridViewSortEventArgs e)
        {
            grdViewDt.DataBind();
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
        protected void grdViewDt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (hdnIsShowResultAfterProposed.Value == "1")
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    HtmlControl divHiddenResult = (HtmlControl)e.Row.FindControl("divHiddenResult");
                    HtmlControl divHiddenResultUnit = (HtmlControl)e.Row.FindControl("divHiddenResultUnit");

                    vLaboratoryResultDt obj = e.Row.DataItem as vLaboratoryResultDt;
                    if (obj.ResultGCTransactionStatus == Constant.TransactionStatus.OPEN || obj.ResultGCTransactionStatus == Constant.TransactionStatus.VOID)
                    {
                        divHiddenResult.Attributes.Add("class", "divHiddenResult");
                        divHiddenResultUnit.Attributes.Add("class", "divHiddenResult");
                    }
                }
            }
        }
        #endregion

        #region Header2
        private void BindGridView2(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string startDate = Helper.GetDatePickerValue(txtPeriodFrom).ToString(Constant.FormatString.DATE_FORMAT_112);
            string endDate = Helper.GetDatePickerValue(txtPeriodTo).ToString(Constant.FormatString.DATE_FORMAT_112);
            Int32 mrn = Convert.ToInt32(hdnMRNCBCtl.Value);
            Int32 registrationID = Convert.ToInt32(hdnRegistrationIDCBCtl.Value);
            if (rblItemTypeTab2.SelectedValue != "1")
            {
                registrationID = 0;
            }

            lstResult = BusinessLayer.GetPatientLaboratoryResult(startDate, endDate, mrn, 0, registrationID);
            List<cFractionGroup> lstTemp = new List<cFractionGroup>();

            List<PatientLaboratoryResult> lstGroup = lstResult.GroupBy(lst => lst.ItemGroupCode).Select(grp => grp.First()).OrderBy(lst => lst.PrintOrder).ToList();
            foreach (PatientLaboratoryResult item in lstGroup)
            {
                lstTemp.Add(new cFractionGroup() { ItemGroupCode = item.ItemGroupCode, ItemGroupName = item.ItemGroupName1 });
            }

            grdViewTab2.DataSource = lstTemp;
            pageCount = 1;
            grdViewTab2.DataBind();
        }

        protected void cbpViewTab2_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                BindGridView2(1, true, ref pageCount);
                result = "refresh|" + pageCount;
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        #region Detail2
        //private void BindGridViewDt2(int pageIndex, bool isCountPageCount, ref int pageCount)
        //{
        //    string startDate = Helper.GetDatePickerValue(txtPeriodFrom).ToString(Constant.FormatString.DATE_FORMAT_112);
        //    string endDate = Helper.GetDatePickerValue(txtPeriodTo).ToString(Constant.FormatString.DATE_FORMAT_112);
        //    Int32 mrn = Convert.ToInt32(hdnMRNCBCtl.Value);
        //    Int32 fractionID = 0;
        //    if (!String.IsNullOrEmpty(hdnItemGroupCodeCBCtl.Value))
        //    {
        //        fractionID = Convert.ToInt32(hdnItemGroupCodeCBCtl.Value);
        //    }
        //    Int32 registrationID = Convert.ToInt32(hdnRegistrationIDCBCtl.Value);
        //    if (rblItemTypeTab2.SelectedValue != "1")
        //    {
        //        registrationID = 0;
        //    }

        //    List<GetDistinctFractionPerDetail> lstEntity = BusinessLayer.GetDistinctFractionPerDetail(startDate, endDate, mrn, 0, fractionID, registrationID);
        //    List<PartialData> lstData = new List<PartialData>();
        //    List<FractionValue> lstValue = new List<FractionValue>();

        //    if (lstEntity.Count > 0)
        //    {
        //        PartialData entity = new PartialData();
        //        entity.SequenceNo = 1;
        //        entity.cfCreatedDate = lstEntity.FirstOrDefault().cfCreatedDate;

        //        foreach (GetDistinctFractionPerDetail e in lstEntity)
        //        {
        //            FractionValue obj = new FractionValue();
        //            obj.SequenceNo = 1;
        //            obj.IsNormal = e.IsNormal;
        //            obj.Fractionvalue = e.ResultValue;
        //            obj.MetricUnitName = e.MetricUnitName;
        //            lstValue.Add(obj);
        //        }

        //        entity.FractionValue = lstValue;
        //        lstData.Add(entity);

        //        lvwViewDt.DataSource = lstData;
        //        lvwViewDt.DataBind();

        //        rptFractionDateHeader.DataSource = lstEntity;
        //        rptFractionDateHeader.DataBind();
        //    }
        //    else
        //    {
        //        lvwViewDt.DataSource = lstData;
        //        lvwViewDt.DataBind();

        //        rptFractionDateHeader.DataSource = lstEntity;
        //        rptFractionDateHeader.DataBind();
        //    }
        //}

        //protected void lvwViewDt_ItemDataBound(object sender, ListViewItemEventArgs e)
        //{
        //    if (e.Item.ItemType == ListViewItemType.DataItem)
        //    {
        //        PartialData obj = (PartialData)e.Item.DataItem;
        //        Repeater rptFractionDetailValue = (Repeater)e.Item.FindControl("rptFractionDetailValue");
        //        rptFractionDetailValue.DataSource = obj.FractionValue;
        //        rptFractionDetailValue.DataBind();
        //    }
        //}

        protected void cbpViewDtTab2_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                List<cFractionInfo> lstFractionInfo = new List<cFractionInfo>();

                #region Create Baseline List
                //Fraction List
                List<PatientLaboratoryResult> lstDetail = lstResult.Where(lst => lst.ItemGroupCode == hdnItemGroupCodeCBCtl.Value).ToList();
                if (lstDetail.Count > 0)
                {
                    List<PatientLaboratoryResult> lstFraction = lstDetail.GroupBy(lst => lst.FractionID).Select(grp => grp.First()).OrderBy(lst => lst.PrintOrder).ToList();
                    if (lstFraction.Count > 0)
                    {
                        foreach (PatientLaboratoryResult fraction in lstFraction)
                        {
                            cFractionInfo obj = new cFractionInfo();
                            obj.ItemGroupCode = fraction.ItemGroupCode;
                            obj.FractionID = fraction.FractionID.ToString();
                            obj.FractionCode = fraction.FractionCode;
                            obj.FractionName = fraction.FractionName1;
                            lstFractionInfo.Add(obj);
                        }
                    }
                }

                //List By Date and Time
                List<PatientLaboratoryResult> lstResultByDateTime = lstDetail.GroupBy(x => new { x.ResultDate, x.ResultTime }).Select(grp => grp.First()).ToList();

                List<cDateTimeColumn> lstDateTimeColumn = new List<cDateTimeColumn>();
                if (lstResultByDateTime.Count > 0)
                {
                    foreach (PatientLaboratoryResult obj in lstResultByDateTime)
                    {
                        cDateTimeColumn objData = new cDateTimeColumn();
                        objData.ResultDate = obj.ResultDate.ToString(Constant.FormatString.DATE_FORMAT);
                        objData.ResultTime = obj.ResultTime;
                        lstDateTimeColumn.Add(objData);
                    }
                }

                DataTable dt = new DataTable();
                CreateDataTable(ref dt, lstFractionInfo, lstDateTimeColumn);
                #endregion

                #region Populate List for Display
                if (dt.Columns.Count > 0)
                {
                    int totalResultDateTimeColumn = lstDateTimeColumn.Count;
                    foreach (cFractionInfo item in lstFractionInfo)
                    {
                        int columnIndex = 1;

                        DataRow dtRow = dt.NewRow();
                        dtRow["FractionCode"] = item.FractionCode;
                        dtRow["FractionName"] = item.FractionName;

                        foreach (cDateTimeColumn objDateTime in lstDateTimeColumn)
                        {
                            string index = columnIndex.ToString().PadLeft(3, '0');
                            string columnName = string.Format("ResultDate{0}", index);
                            cResultData columnValue = new cResultData();

                            PatientLaboratoryResult resultData = lstDetail.Where(lst => lst.FractionID.ToString() == item.FractionID && lst.ResultDate.ToString(Constant.FormatString.DATE_FORMAT) == objDateTime.ResultDate && lst.ResultTime == objDateTime.ResultTime).FirstOrDefault();
                            if (resultData != null)
                            {
                                cResultData oData = new cResultData();

                                oData.ResultValue = resultData.cfTestResultValueNew;
                                oData.ReferenceRange = resultData.ReferenceRange;
                                oData.IsNormalValue = resultData.IsNormal;
                                oData.ResultUnit = resultData.MetricUnit;
                                oData.ResultFlag = resultData.ResultFlag;

                                columnValue = oData;
                            }

                            dtRow[columnName] = columnValue;
                            columnIndex += 1;
                        }

                        //Adding Rows to Data Table
                        dt.Rows.Add(dtRow);
                    }

                    //Create GridView Columns
                    foreach (DataColumn column in dt.Columns)
                    {
                        if (column.ExtendedProperties["CssClass"].ToString() != "resultData")
                        {
                            BoundField gridColumn = new BoundField();
                            gridColumn.HeaderText = SetColumnHeaderText(column); ;
                            gridColumn.HeaderStyle.CssClass = column.ExtendedProperties["CssClass"].ToString();
                            gridColumn.ItemStyle.CssClass = column.ExtendedProperties["CssClass"].ToString();
                            gridColumn.DataField = column.ExtendedProperties["DataField"].ToString();
                            gridColumn.HtmlEncode = false;
                            grdViewDt2.Columns.Add(gridColumn);
                        }
                        else
                        {
                            TemplateField gridColumn = new TemplateField();
                            gridColumn.HeaderText = SetColumnHeaderText(column); ;
                            gridColumn.HeaderStyle.CssClass = column.ExtendedProperties["CssClass"].ToString();
                            gridColumn.ItemStyle.CssClass = column.ExtendedProperties["CssClass"].ToString();
                            gridColumn.ItemTemplate = new MyTemplate();
                            grdViewDt2.Columns.Add(gridColumn);
                        }
                    }

                    grdViewDt2.DataSource = dt;
                    grdViewDt2.DataBind();
                }
                #endregion
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private string SetColumnHeaderText(DataColumn column)
        {
            string headerText = column.ExtendedProperties["ColumnHeader"].ToString();
            switch (column.ExtendedProperties["CssClass"].ToString())
            {
                case "resultData":
                    headerText = headerText.Replace("_", "<br />");
                    break;
                default:
                    break;
            }
            return headerText;
        }

        private void CreateDataTable(ref DataTable dt, List<cFractionInfo> lstFractionInfo, List<cDateTimeColumn> lstDateTimeColumn)
        {
            DataColumn column = new DataColumn("FractionCode", typeof(string));
            column.ExtendedProperties.Add("ColumnHeader", "Artikel");
            column.ExtendedProperties.Add("CssClass", "keyField");
            column.ExtendedProperties.Add("DataField", "FractionCode");
            dt.Columns.Add(column);

            column = new DataColumn("FractionName", typeof(string));
            column.ExtendedProperties.Add("ColumnHeader", "Artikel");
            column.ExtendedProperties.Add("CssClass", "fractionInfo");
            column.ExtendedProperties.Add("DataField", "FractionName");
            dt.Columns.Add(column);

            int columnIndex = 1;
            foreach (cDateTimeColumn item in lstDateTimeColumn)
            {
                string index = columnIndex.ToString().PadLeft(3, '0');
                string columnHeader = string.Format("{0}_{1}", item.ResultDate, item.ResultTime);
                string columnName = string.Format("ResultDate{0}", index);

                column = new DataColumn(columnName, typeof(cResultData));
                column.ExtendedProperties.Add("ColumnHeader", columnHeader);
                column.ExtendedProperties.Add("CssClass", "resultData");
                column.ExtendedProperties.Add("DataField", columnName);
                dt.Columns.Add(column);
                columnIndex += 1;
            }
        }

        private void AddProperty(dynamic dsObject, string propertyName, string propertyValue)
        {
            var exDict = dsObject as IDictionary<string, object>;
            if (exDict.ContainsKey(propertyName))
                exDict[propertyName] = propertyValue;
            else
                exDict.Add(propertyName, propertyValue);
        }

        protected void grdViewDt2_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    if (i >= 0 && i < grdViewDt.Columns.Count && grdViewDt.Columns[i] is TemplateField)
                    {
                        string columnIndex = (i-1).ToString().PadLeft(3, '0');
                        string columnName = string.Format("ResultDate{0}", columnIndex);
                        Label lblResultValue = new Label();
                        lblResultValue.ID = string.Format("lblResultValue{0}", columnIndex);

                        cResultData oData = (cResultData)DataBinder.Eval(e.Row.DataItem, columnName);

                        if (!oData.IsNormalValue && oData.ResultFlag != "*")
                        {
                            lblResultValue.CssClass = "isAbnormalColor";
                        }
                        else if (oData.IsCriticalValue)
                        {
                            lblResultValue.CssClass = "isPanicRangeColor";
                        }

                        lblResultValue.Text = oData.ResultValue;
                        if (!string.IsNullOrEmpty(oData.ReferenceRange))
                        {
                            StringBuilder sbToolTip = new StringBuilder();
                            sbToolTip.AppendLine(string.Format("{0}: {1}", "Result Flag".PadRight(16,' '), oData.ResultFlag));
                            sbToolTip.AppendLine(string.Format("{0}: {1}", "Reference Value".PadRight(16, ' '), oData.ReferenceRange));
                            lblResultValue.ToolTip = sbToolTip.ToString(); 
                        }
                        
                        e.Row.Cells[i].Controls.Add(lblResultValue);
                    }
                }
            }
        }

        #endregion

        public string GetLaboratoryResultImage()
        {
            string result = "";
            result = GeneratePreviewUrl(hdnReferenceNoCBCtl.Value);
            return result;
        }

        private string GeneratePreviewUrl(string referenceNo)
        {
            string result = "";
            string postData = referenceNo;

            string url = string.Format("{0}?{1}", hdnRISWebViewURL.Value, postData);
            if (hdnRISHL7MessageFormat.Value == Constant.RIS_HL7MessageFormat.MEDAVIS)
            {
                url = string.Format("{0}{1}", hdnRISWebViewURL.Value, referenceNo);
            }
            else
            {
                #region Post Parameter Data
                TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
                Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                string consID = referenceNo;
                string pass = hdnRISConsumerPassword.Value;
                string data = unixTimestamp.ToString() + consID;

                postData = string.Format("X-cons-id={0}&X-timestamp={1}&X-signature={2}", consID, unixTimestamp.ToString(), HttpUtility.UrlEncode(Methods.GenerateSignature(data, pass)));

                UTF8Encoding encoding = new UTF8Encoding();
                byte[] postDataBytes = encoding.GetBytes(postData);
                #endregion

                url = string.Format("{0}?{1}", hdnRISWebViewURL.Value, postData);
            }

            return result = string.Format("{0}|{1}", "1", url);
        }

        #region Custom Class
        public class cFractionGroup
        {
            public string ItemGroupCode { get; set; }
            public string ItemGroupName { get; set; }
        }

        public class cDateTimeColumn
        {
            public string ResultDate { get; set; }
            public string ResultTime { get; set; }
        }

        public class cResultData
        {
            public string ResultDate { get; set; }
            public string ResultTime { get; set; }
            public string ResultValue { get; set; }
            public string ResultUnit { get; set; }
            public Boolean IsNormalValue { get; set; }
            public string ReferenceRange { get; set; }
            public string ResultFlag { get; set; }
            public Boolean IsCriticalValue { get; set; }
        }

        public class cFractionInfo
        {
            public string ItemGroupCode { get; set; }
            public string FractionID { get; set; }
            public string FractionCode { get; set; }
            public string FractionName { get; set; }
        }

        public class MyTemplate : ITemplate
        {
            public void InstantiateIn(Control container)
            {
                Label label = new Label();
                label.ID = "lblResultValue";
                container.Controls.Add(label);
            }
        }
        #endregion

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
}