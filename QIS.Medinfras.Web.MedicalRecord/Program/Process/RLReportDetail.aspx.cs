using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using System.Globalization;
using System.Web.UI.HtmlControls;
using DevExpress.Web.ASPxEditors;
using System.Text;


namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class RLReportDetail : BasePageTrx
    {
        protected int PageCount = 1;
        public int ctr;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.RL_REPORT_CONFIGURATION;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            BindGridView();
        }

        protected override void InitializeDataControl()
        {
            cboMonth.DataSource = Enumerable.Range(1, 12).Select(a => new
            {
                MonthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(a),
                MonthNumber = a
            });
            cboMonth.TextField = "MonthName";
            cboMonth.ValueField = "MonthNumber";
            cboMonth.EnableCallbackMode = false;
            cboMonth.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
            cboMonth.DropDownStyle = DropDownStyle.DropDownList;
            cboMonth.DataBind();
            cboMonth.Value = DateTime.Now.Month.ToString();

            cboYear.DataSource = Enumerable.Range(DateTime.Now.Year - 99, 100).Reverse();
            cboYear.EnableCallbackMode = false;
            cboYear.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
            cboYear.DropDownStyle = DropDownStyle.DropDownList;
            cboYear.DataBind();
            cboYear.SelectedIndex = 0;
            BindGridView();
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowNextPrev = IsAllowVoid = false;//= IsAllowSave 
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            }
        }
        private void BindGridView()
        {
            string filterExpression = "1 = 0";
            if (hdnRLReportTypeID.Value != "")
            {
                ListCustomAttribute = initListCustomAttribute();
                rptHeader.DataSource = ListCustomAttribute;
                rptHeader.DataBind();

                filterExpression = string.Format("RLReportID = {0} AND IsDeleted = 0 ORDER BY DisplayOrder", hdnRLReportTypeID.Value);

                string periodNo = String.Format("{0}{1:00}", hdnSelectedYear.Value, Convert.ToInt32(hdnSelectedMonth.Value));
                lstEntity = BusinessLayer.GetRLReportDataList(string.Format("RLReportID = {0} AND PeriodNo = '{1}' AND HealthcareID = '{2}'", hdnRLReportTypeID.Value, periodNo, AppSession.UserLogin.HealthcareID));
            }

            rptView.DataSource = BusinessLayer.GetRLReportRowList(filterExpression);
            rptView.DataBind();
        }

        List<RLReportData> lstEntity = null;
        protected void rptView_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                RLReportRow entity = e.Item.DataItem as RLReportRow;

                HtmlInputHidden hdnRowID = e.Item.FindControl("hdnRowID") as HtmlInputHidden;
                hdnRowID.Value = entity.ID.ToString();

                
                Repeater rptViewDetail = e.Item.FindControl("rptViewDetail") as Repeater;
                rptViewDetail.DataSource = ListCustomAttribute;
                rptViewDetail.DataBind();

                if (lstEntity != null)
                {
                    RLReportData rlReportData = lstEntity.FirstOrDefault(p => p.RowID == entity.ID);
                    if (rlReportData != null)
                    {
                        foreach (RepeaterItem item in rptViewDetail.Items)
                        {
                            TextBox txtRLReportData = (TextBox)item.FindControl("txtRLReportData");
                            HtmlInputHidden hdn = (HtmlInputHidden)item.FindControl("hdnRLReportCode");
                            object val = rlReportData.GetType().GetProperty("Column" + hdn.Value).GetValue(rlReportData, null);
                            txtRLReportData.Text = (val == null ? "" : val.ToString());
                            Helper.SetControlEntrySetting(txtRLReportData, new ControlEntrySetting(true, true, true), "mpPatientList");
                        }
                    }
                }
            }
        }

        List<Variable> ListCustomAttribute = null;
        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            RLReport rpt = BusinessLayer.GetRLReport(Convert.ToInt32(hdnRLReportTypeID.Value));
            thColumnCaption.InnerHtml = string.Format("{0}",rpt.ColumnCaption);
            lblPeriod.InnerHtml = string.Format("Period : {0} {1}", cboMonth.Text, cboYear.Text);
            BindGridView();
        }

        protected List<Variable> initListCustomAttribute()
        {
            List<Variable> ListCustomAttribute = new List<Variable>();
            RLReportColumn column = BusinessLayer.GetRLReportColumn(Convert.ToInt32(hdnRLReportTypeID.Value));
            if (column != null)
            {
                if (column.Column1Title != "") { ListCustomAttribute.Add(new Variable { Code = "1", Value = column.Column1Title }); }
                if (column.Column2Title != "") { ListCustomAttribute.Add(new Variable { Code = "2", Value = column.Column2Title }); }
                if (column.Column3Title != "") { ListCustomAttribute.Add(new Variable { Code = "3", Value = column.Column3Title }); }
                if (column.Column4Title != "") { ListCustomAttribute.Add(new Variable { Code = "4", Value = column.Column4Title }); }
                if (column.Column5Title != "") { ListCustomAttribute.Add(new Variable { Code = "5", Value = column.Column5Title }); }
                if (column.Column6Title != "") { ListCustomAttribute.Add(new Variable { Code = "6", Value = column.Column6Title }); }
                if (column.Column7Title != "") { ListCustomAttribute.Add(new Variable { Code = "7", Value = column.Column7Title }); }
                if (column.Column8Title != "") { ListCustomAttribute.Add(new Variable { Code = "8", Value = column.Column8Title }); }
                if (column.Column9Title != "") { ListCustomAttribute.Add(new Variable { Code = "9", Value = column.Column9Title }); }
                if (column.Column10Title != "") { ListCustomAttribute.Add(new Variable { Code = "10", Value = column.Column10Title }); }
                if (column.Column11Title != "") { ListCustomAttribute.Add(new Variable { Code = "11", Value = column.Column11Title }); }
                if (column.Column12Title != "") { ListCustomAttribute.Add(new Variable { Code = "12", Value = column.Column12Title }); }
                if (column.Column13Title != "") { ListCustomAttribute.Add(new Variable { Code = "13", Value = column.Column13Title }); }
                if (column.Column14Title != "") { ListCustomAttribute.Add(new Variable { Code = "14", Value = column.Column14Title }); }
                if (column.Column15Title != "") { ListCustomAttribute.Add(new Variable { Code = "15", Value = column.Column15Title }); }
                if (column.Column16Title != "") { ListCustomAttribute.Add(new Variable { Code = "16", Value = column.Column16Title }); }
                if (column.Column17Title != "") { ListCustomAttribute.Add(new Variable { Code = "17", Value = column.Column17Title }); }
                if (column.Column18Title != "") { ListCustomAttribute.Add(new Variable { Code = "18", Value = column.Column18Title }); }
                if (column.Column19Title != "") { ListCustomAttribute.Add(new Variable { Code = "19", Value = column.Column19Title }); }
                if (column.Column20Title != "") { ListCustomAttribute.Add(new Variable { Code = "20", Value = column.Column20Title }); }
            }
            return ListCustomAttribute;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RLReportDataDao entityDao = new RLReportDataDao(ctx);
            try
            {
                int RLReportID = Convert.ToInt32(hdnRLReportTypeID.Value);
                string periodNo = String.Format("{0}{1:00}", hdnSelectedYear.Value, Convert.ToInt32(hdnSelectedMonth.Value));
                List<RLReportData> lstEntity = BusinessLayer.GetRLReportDataList(string.Format("RLReportID = {0} AND PeriodNo = '{1}' AND HealthcareID = '{2}'", hdnRLReportTypeID.Value, periodNo, AppSession.UserLogin.HealthcareID), ctx);
                foreach (RepeaterItem rptViewItem in rptView.Items)
                {
                    HtmlInputHidden hdnRowID = rptViewItem.FindControl("hdnRowID") as HtmlInputHidden;
                    int rowID = Convert.ToInt32(hdnRowID.Value);

                    bool IsAdd = false;
                    RLReportData entityData = lstEntity.FirstOrDefault(p => p.RowID == rowID);
                    if (entityData == null)
                    {
                        entityData = new RLReportData();
                        IsAdd = true;
                        entityData.HealthcareID = AppSession.UserLogin.HealthcareID;
                        entityData.RLReportID = Convert.ToInt32(hdnRLReportTypeID.Value);
                        entityData.RowID = rowID;
                        entityData.PeriodNo = periodNo;
                    }
                    bool IsHasData = false;
                    Repeater rptViewDetail = rptViewItem.FindControl("rptViewDetail") as Repeater;
                    foreach (RepeaterItem item in rptViewDetail.Items)
                    {
                        TextBox txtRLReportData = (TextBox)item.FindControl("txtRLReportData");
                        HtmlInputHidden hdn = (HtmlInputHidden)item.FindControl("hdnRLReportCode");
                        Int32? data = null;
                        if (Request.Form[txtRLReportData.UniqueID] != "")
                        {
                            data = Convert.ToInt32(Request.Form[txtRLReportData.UniqueID]);
                            IsHasData = true;
                        }
                        entityData.GetType().GetProperty("Column" + hdn.Value).SetValue(entityData, data, null);
                    }
                    if (IsAdd)
                    {
                        if (IsHasData)
                        {
                            entityData.CreatedBy = AppSession.UserLogin.UserID;
                            entityDao.Insert(entityData);
                        }
                    }
                    else
                    {
                        entityData.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDao.Update(entityData);
                    }
                }
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string result = "";
            string periodNo = String.Format("{0}{1:00}", hdnSelectedYear.Value, Convert.ToInt32(hdnSelectedMonth.Value));
            RLReport fn = BusinessLayer.GetRLReport(Convert.ToInt32(hdnRLReportTypeID.Value));
            string filename = string.Format("{0}//{1}//{2}", fn.RLReportCode, periodNo, DateTime.Now.ToString("yyyyMMddHHmmss"));
            RLReportColumn rc = BusinessLayer.GetRLReportColumn(Convert.ToInt32(hdnRLReportTypeID.Value));

            try
            {
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=" + filename + ".csv");
                Response.Charset = "";
                Response.ContentType = "application/text";


                StringBuilder sbResult = new StringBuilder();

                List<vRLReportData> lstData = BusinessLayer.GetvRLReportDataList(string.Format("RLReportID = '{0}' AND PeriodNo = '{1}' AND HealthCareID = '{2}' ", hdnRLReportTypeID.Value,periodNo,AppSession.UserLogin.HealthcareID));

                int ct = 0;

                if (ct == 0)
                {
                    sbResult.Append(fn.ColumnCaption).Append(",");
                    ct = 1;
                }                    
                
                if(ct==1)
                {
                    foreach (Variable data in ListCustomAttribute)
                    {
                        sbResult.Append(data.Value);
                        sbResult.Append(",");
                    }                           
                }
                sbResult.Append("\r\n");

                foreach (RepeaterItem rptViewItem in rptView.Items)
                {

                    HtmlInputHidden hdnRowID = rptViewItem.FindControl("hdnRowID") as HtmlInputHidden;
                    RLReportRow rowName = BusinessLayer.GetRLReportRow(Convert.ToInt32(hdnRowID.Value));
                    sbResult.Append(rowName.RowTitle).Append(",");

                    Repeater rptViewDetail = rptViewItem.FindControl("rptViewDetail") as Repeater;
                    foreach (RepeaterItem item in rptViewDetail.Items)
                    {
                        TextBox txtRLReportData = (TextBox)item.FindControl("txtRLReportData");
                        string data = Request.Form[txtRLReportData.UniqueID];
                        if (data == "")
                            data = "0";
                        sbResult.Append(data).Append(",");
                    }
                    sbResult.Append("\n");
                }

                Response.Output.Write(sbResult.ToString());
                result = "success";
            }
            catch (Exception ex)
            {
                result = string.Format("fail|{0}", ex.Message);
            }
            finally
            {
                Response.Flush();
                Response.End();
            }
        }

        private string CsvEscape(string value)
        {
            if (value.Contains(","))
            {
                return "\"" + value.Replace("\"", "\"\"") + "\"";
            }
            return value;
        }

    }
}
