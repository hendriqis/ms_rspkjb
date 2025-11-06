using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.XtraReports.UI;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class PrintToolsList : BasePagePatientPageList
    {
        private string GetFilterExpression(string value)
        {
            StringBuilder sbResult = new StringBuilder(value);
            sbResult.Replace("@HealthcareID", AppSession.UserLogin.HealthcareID);
            sbResult.Replace("@UserID", AppSession.UserLogin.UserID.ToString());
            if (value.Contains("@LaboratoryID") || value.Contains("@ImagingID"))
            {
                List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID, Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID));
                string laboratoryID = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID).ParameterValue;
                string imagingID = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID).ParameterValue;
                List<vHealthcareServiceUnit> lstHealthcareServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND ServiceUnitID IN ({1},{2}) AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, laboratoryID, imagingID));
                sbResult.Replace("@LaboratoryID", lstHealthcareServiceUnit.FirstOrDefault(p => p.ServiceUnitID == Convert.ToInt32(laboratoryID)).HealthcareServiceUnitID.ToString());
                sbResult.Replace("@ImagingID", lstHealthcareServiceUnit.FirstOrDefault(p => p.ServiceUnitID == Convert.ToInt32(imagingID)).HealthcareServiceUnitID.ToString());
            }
            return sbResult.ToString();
        }

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.PRINT_TOOLS;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            string id = Request.Form[hdnReportCode.UniqueID] == null ? "" : Request.Form[hdnReportCode.UniqueID];
            if (id != "")
                SetReportParameter();
        }

        private String ModuleID = "";
        List<GetUserMenuAccess> lstAllMenu = null;
        protected override void InitializeDataControl()
        {
            string moduleName = Helper.GetModuleName();
            ModuleID = Helper.GetModuleID(moduleName);
            lstAllMenu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("(ParentCode = '{0}' OR ParentID IN (SELECT MenuID FROM Menu WHERE ParentID = (SELECT MenuID FROM Menu WHERE MenuCode = '{0}')))", OnGetMenuCode()));
            List<GetUserMenuAccess> lstMenuParent = lstAllMenu.Where(p => p.ParentCode == OnGetMenuCode()).OrderBy(p => p.MenuIndex).ToList();
            PopulateNodes(lstMenuParent, tvwView.Nodes);
        }

        private void PopulateNodes(List<GetUserMenuAccess> lstMenu, TreeNodeCollection nodes)
        {
            foreach (GetUserMenuAccess menu in lstMenu)
            {
                Int32 childCount = lstAllMenu.Where(p => p.ParentID == menu.MenuID).Count();
                TreeNode tn = new TreeNode();
                tn.Text = HttpUtility.HtmlEncode(menu.MenuCaption);
                tn.Value = menu.MenuID.ToString();
                if (childCount > 0)
                    tn.SelectAction = TreeNodeSelectAction.Expand;
                else
                    tn.NavigateUrl = string.Format("{0}|{1}", menu.MenuID, menu.MenuCode);
                nodes.Add(tn);

                tn.PopulateOnDemand = (childCount > 0);
            }
        }

        private void PopulateSubLevel(Int32 parentID, TreeNode parentNode)
        {
            PopulateNodes(lstAllMenu.Where(p => p.ParentID == parentID).OrderBy(p => p.MenuIndex).ToList(), parentNode.ChildNodes);
        }

        protected void tvwView_TreeNodePopulate(object sender, TreeNodeEventArgs e)
        {
            PopulateSubLevel(Convert.ToInt32(e.Node.Value), e.Node);
        }

        private void BindGridView()
        {
            List<vReportParameter> lstReportParameter = BusinessLayer.GetvReportParameterList(string.Format("ReportCode = '{0}' AND IsDeleted = 0 ORDER BY DisplayOrder ASC", Request.Form[hdnReportCode.UniqueID]));
            rptReportParameter.DataSource = lstReportParameter;
            rptReportParameter.DataBind();
        }

        private void SetReportParameter()
        {
            ReportMaster entity = BusinessLayer.GetReportMasterList(string.Format("ReportCode = '{0}'", Request.Form[hdnReportCode.UniqueID])).FirstOrDefault();
            if (entity != null)
            {
                if (entity.IsCustomSetting)
                {
                    hdnIsCustomSetting.Value = "1";
                    Control ctlParent = pnlReportCtl;
                    BaseCustomReportSettingCtl ctl = (BaseCustomReportSettingCtl)LoadControl(entity.CustomSettingUrl);
                    ctl.ID = "ctlCustomSetting";
                    ctlParent.Controls.Clear();
                    ctlParent.Controls.Add(ctl);
                    ctl.InitializeDataControl();
                }
                else
                {
                    hdnIsCustomSetting.Value = "0";
                    BindGridView();
                }
            }
        }

        protected void cbpReportParameter_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            SetReportParameter();
        }

        protected void rptReportParameter_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                vReportParameter entity = (vReportParameter)e.Item.DataItem;
                entity.FilterExpression = GetFilterExpression(entity.FilterExpression);
                HtmlGenericControl div = null;

                if (entity.GCFilterParameterType == Constant.FilterParameterType.CONSTANT)
                {
                    HtmlTableRow trReportParameter = (HtmlTableRow)e.Item.FindControl("trReportParameter");
                    TextBox txtValue = (TextBox)e.Item.FindControl("txtValue");
                    txtValue.Text = entity.DefaultValue;
                    trReportParameter.Style.Add("display", "none");
                }
                else if (entity.GCFilterParameterType == Constant.FilterParameterType.TEXT_BOX)
                {
                    div = (HtmlGenericControl)e.Item.FindControl("divTxt");
                    HtmlGenericControl lbl = (HtmlGenericControl)e.Item.FindControl("lblColumn");
                    lbl.Attributes.Add("class", "lblMandatory");
                    TextBox txtValue = (TextBox)e.Item.FindControl("txtValue");
                    txtValue.Text = entity.DefaultValue;
                    if (entity.TxtCssClass != "")
                        txtValue.CssClass = entity.TxtCssClass;
                    Helper.SetControlEntrySetting(txtValue, new ControlEntrySetting(true, true, true), "mpReport");
                }
                else if (entity.GCFilterParameterType == Constant.FilterParameterType.SINGLE_DATE)
                {
                    div = (HtmlGenericControl)e.Item.FindControl("divDte");
                    HtmlGenericControl lbl = (HtmlGenericControl)e.Item.FindControl("lblColumn");
                    lbl.Attributes.Add("class", "lblMandatory");
                    TextBox txtDteValue = (TextBox)e.Item.FindControl("txtDteValue");
                    if (entity.DefaultValue == "@DateNow")
                        txtDteValue.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    else
                        txtDteValue.Text = entity.DefaultValue;
                    Helper.SetControlEntrySetting(txtDteValue, new ControlEntrySetting(true, true, true), "mpReport");
                }
                else if (entity.GCFilterParameterType == Constant.FilterParameterType.SEARCH_DIALOG)
                {
                    div = (HtmlGenericControl)e.Item.FindControl("divSd");
                    HtmlGenericControl lbl = (HtmlGenericControl)e.Item.FindControl("lblColumn");

                    HtmlInputHidden hdnSdNewID = (HtmlInputHidden)e.Item.FindControl("hdnSdNewID");
                    HtmlInputHidden hdnSearchDialogFilterExpression = (HtmlInputHidden)e.Item.FindControl("hdnSearchDialogFilterExpression");
                    TextBox txtSdNewCode = (TextBox)e.Item.FindControl("txtSdNewCode");
                    TextBox txtSdNewText = (TextBox)e.Item.FindControl("txtSdNewText");

                    StringBuilder sbFilterExpression = new StringBuilder();
                    sbFilterExpression.Append(entity.SearchDialogFilterExpression).Replace("@HealthcareID", AppSession.UserLogin.HealthcareID).Replace("@UserID", AppSession.UserLogin.UserID.ToString());
                    hdnSearchDialogFilterExpression.Value = sbFilterExpression.ToString();

                    if (entity.SearchDialogCodeField == entity.SearchDialogNameField)
                        txtSdNewText.Visible = false;
                    if (entity.IsAllowSelectAll)
                        lbl.Attributes.Add("class", "lblLink lblReport");
                    else
                        lbl.Attributes.Add("class", "lblLink lblReport lblMandatory");
                    Helper.SetControlEntrySetting(txtSdNewCode, new ControlEntrySetting(true, true, !entity.IsAllowSelectAll), "mpReport");
                }
                else if (entity.GCFilterParameterType == Constant.FilterParameterType.COMBO_BOX || entity.GCFilterParameterType == Constant.FilterParameterType.CUSTOM_COMBO_BOX || entity.GCFilterParameterType == Constant.FilterParameterType.YEAR_COMBO_BOX)
                {
                    div = (HtmlGenericControl)e.Item.FindControl("divCbo");
                    ASPxComboBox cboValue = (ASPxComboBox)e.Item.FindControl("cboValue");

                    if (entity.GCFilterParameterType == Constant.FilterParameterType.COMBO_BOX)
                    {
                        MethodInfo method = typeof(BusinessLayer).GetMethod(entity.MethodName, new[] { typeof(string) });
                        object obj = method.Invoke(null, new string[] { entity.FilterExpression });
                        IList list = (IList)obj;

                        cboValue.DataSource = list;
                        cboValue.TextField = entity.TextFieldName;
                        cboValue.ValueField = entity.ValueFieldName;
                        cboValue.CallbackPageSize = 50;
                        cboValue.EnableCallbackMode = false;
                        cboValue.ClientInstanceName = entity.ClientInstanceName;
                        cboValue.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
                        cboValue.DropDownStyle = DropDownStyle.DropDownList;
                        cboValue.DataBind();
                    }
                    else if (entity.GCFilterParameterType == Constant.FilterParameterType.CUSTOM_COMBO_BOX)
                    {
                        string[] lstText = entity.ListText.Split('|');
                        string[] lstValue = entity.ListValue.Split('|');
                        for (int i = 0; i < lstText.Length; ++i)
                            cboValue.Items.Add(new ListEditItem { Value = lstValue[i], Text = lstText[i] });
                        cboValue.CallbackPageSize = 50;
                        cboValue.EnableCallbackMode = false;
                        cboValue.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
                        cboValue.DropDownStyle = DropDownStyle.DropDownList;
                    }
                    else
                    {
                        cboValue.DataSource = Enumerable.Range(DateTime.Now.Year - entity.YearMinusNYear, entity.YearPlusNYear + entity.YearMinusNYear + 1).Reverse();
                        cboValue.EnableCallbackMode = false;
                        cboValue.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
                        cboValue.DropDownStyle = DropDownStyle.DropDownList;
                        cboValue.DataBind();
                    }
                    if (entity.IsAllowSelectAll)
                        cboValue.Items.Insert(0, new ListEditItem { Value = "", Text = "" });

                    cboValue.SelectedIndex = 0;
                    Helper.SetControlEntrySetting(cboValue, new ControlEntrySetting(true, true, !entity.IsAllowSelectAll), "mpReport");
                }
                else if (entity.GCFilterParameterType == Constant.FilterParameterType.TIME_RANGE)
                {
                    div = (HtmlGenericControl)e.Item.FindControl("divRange");
                    HtmlGenericControl lbl = (HtmlGenericControl)e.Item.FindControl("lblColumn");

                    TextBox txtValueRangeFrom = (TextBox)e.Item.FindControl("txtValueRangeFrom");
                    TextBox txtValueRangeTo = (TextBox)e.Item.FindControl("txtValueRangeTo");
                    txtValueRangeFrom.Text = entity.DefaultValue;
                    txtValueRangeTo.Text = entity.DefaultValue;
                    if (entity.TxtCssClass != "")
                    {
                        txtValueRangeFrom.CssClass = entity.TxtCssClass;
                        txtValueRangeTo.CssClass = entity.TxtCssClass;
                    }
                    Helper.SetControlEntrySetting(txtValueRangeFrom, new ControlEntrySetting(true, true, true), "mpReport");
                    Helper.SetControlEntrySetting(txtValueRangeTo, new ControlEntrySetting(true, true, true), "mpReport");
                }
                else if (entity.GCFilterParameterType == Constant.FilterParameterType.FREE_TEXT)
                {
                }
                else
                {
                    div = (HtmlGenericControl)e.Item.FindControl("divCbo");
                    TextBox txtValueDateFrom = (TextBox)e.Item.FindControl("txtValueDateFrom");
                    TextBox txtValueDateTo = (TextBox)e.Item.FindControl("txtValueDateTo");
                    TextBox txtValueNum = (TextBox)e.Item.FindControl("txtValueNum");

                    txtValueDateTo.Text = txtValueDateFrom.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    Helper.SetControlEntrySetting(txtValueDateFrom, new ControlEntrySetting(true, true, true), "mpReport");
                    Helper.SetControlEntrySetting(txtValueDateTo, new ControlEntrySetting(true, true, true), "mpReport");
                    Helper.SetControlEntrySetting(txtValueNum, new ControlEntrySetting(true, true, true), "mpReport");

                    string filterExpression = "";
                    switch (entity.GCFilterParameterType)
                    {
                        case Constant.FilterParameterType.PAST_PERIOD: filterExpression = string.Format("ParentID = '{0}' AND StandardCodeID NOT BETWEEN '{0}^050' AND '{0}^060' AND IsDeleted = 0", Constant.StandardCode.REPORTING_PERIOD); break;
                        case Constant.FilterParameterType.UPCOMING_PERIOD: filterExpression = string.Format("ParentID = '{0}' AND StandardCodeID NOT BETWEEN '{0}^010' AND '{0}^020' AND IsDeleted = 0", Constant.StandardCode.REPORTING_PERIOD); break;
                        case Constant.FilterParameterType.DATE: filterExpression = string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.REPORT_TYPE); break;
                    }
                    ASPxComboBox cboValue = (ASPxComboBox)e.Item.FindControl("cboValue");
                    List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
                    Methods.SetComboBoxField<StandardCode>(cboValue, lstStandardCode, "StandardCodeName", "StandardCodeID");
                    cboValue.SelectedIndex = 0;
                }
                if (div != null)
                    div.Visible = true;
            }
        }

        protected void cbpReportProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string param = "";
            string errMessage = "";
            string result = "";
            if (OnProcessReport(ref param, ref errMessage))
                result = "success";
            else
                result = "fail|" + errMessage;

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpParam"] = param;
            panel.JSProperties["cpResult"] = result;
        }
        private string filterParam = "";
        private bool OnProcessReport(ref string param, ref string errMessage)
        {
            try
            {
                if (hdnIsCustomSetting.Value == "0")
                {
                    int ctr = 0;
                    foreach (RepeaterItem itemDt in rptReportParameter.Items)
                    {
                        if (ctr > 0)
                            param += "|";
                        HtmlInputHidden hdnGCFilterParameterType = (HtmlInputHidden)itemDt.FindControl("hdnGCFilterParameterType");
                        if (hdnGCFilterParameterType.Value == Constant.FilterParameterType.TEXT_BOX || hdnGCFilterParameterType.Value == Constant.FilterParameterType.CONSTANT)
                        {
                            TextBox txtValue = (TextBox)itemDt.FindControl("txtValue");
                            param += txtValue.Text;
                        }
                        else if (hdnGCFilterParameterType.Value == Constant.FilterParameterType.SINGLE_DATE)
                        {
                            TextBox txtDteValue = (TextBox)itemDt.FindControl("txtDteValue");
                            param += Helper.GetDatePickerValue(txtDteValue.Text).ToString("yyyyMMdd");
                        }
                        else if (hdnGCFilterParameterType.Value == Constant.FilterParameterType.SEARCH_DIALOG)
                        {
                            HtmlInputHidden hdnSdNewID = (HtmlInputHidden)itemDt.FindControl("hdnSdNewID");
                            param += hdnSdNewID.Value;
                        }
                        else if (hdnGCFilterParameterType.Value == Constant.FilterParameterType.COMBO_BOX || hdnGCFilterParameterType.Value == Constant.FilterParameterType.CUSTOM_COMBO_BOX || hdnGCFilterParameterType.Value == Constant.FilterParameterType.YEAR_COMBO_BOX)
                        {
                            ASPxComboBox cboValue = (ASPxComboBox)itemDt.FindControl("cboValue");
                            if (cboValue.Value != null && cboValue.Value.ToString() != "")
                                param += cboValue.Value.ToString();
                        }
                        else if (hdnGCFilterParameterType.Value == Constant.FilterParameterType.TIME_RANGE)
                        {
                            TextBox txtValueRangeFrom = (TextBox)itemDt.FindControl("txtValueRangeFrom");
                            TextBox txtValueRangeTo = (TextBox)itemDt.FindControl("txtValueRangeTo");
                            param += string.Format("{0};{1}", txtValueRangeFrom.Text, txtValueRangeTo.Text);
                        }
                        else if (hdnGCFilterParameterType.Value == Constant.FilterParameterType.FREE_TEXT)
                        {
                            param += string.Format("RegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID);
                        }
                        else
                        {
                            ASPxComboBox cboValue = (ASPxComboBox)itemDt.FindControl("cboValue");
                            TextBox txtValueNum = (TextBox)itemDt.FindControl("txtValueNum");
                            TextBox txtValueDateFrom = (TextBox)itemDt.FindControl("txtValueDateFrom");
                            TextBox txtValueDateTo = (TextBox)itemDt.FindControl("txtValueDateTo");
                            DateTime startDate = DateTime.Today;
                            DateTime endDate = DateTime.Today;
                            int num = Convert.ToInt32(txtValueNum.Text);
                            switch (cboValue.Value.ToString())
                            {
                                //Custom
                                case "X106^090": startDate = Helper.GetDatePickerValue(txtValueDateFrom.Text); endDate = Helper.GetDatePickerValue(txtValueDateTo.Text); break;
                                //Last n Years
                                case "X106^010": startDate = DateTime.Today.AddYears(-num); break;
                                //Last n Months
                                case "X106^011": startDate = DateTime.Today.AddMonths(-num); break;
                                //Last n Weeks
                                case "X106^012": startDate = DateTime.Today.AddDays(-7 * num); break;
                                //Last n Days
                                case "X106^013": startDate = DateTime.Today.AddDays(-num); break;
                                //Last Year
                                case "X106^014": startDate = DateTime.Today.AddYears(-1); break;
                                //Last Month
                                case "X106^015": startDate = DateTime.Today.AddMonths(-1); break;
                                //Last Week
                                case "X106^016": startDate = DateTime.Today.AddDays(-7); break;
                                //Yesterday
                                case "X106^017": startDate = DateTime.Today.AddDays(-1); break;

                                //Next n Years
                                case "X106^050": endDate = DateTime.Today.AddYears(num); break;
                                //Next n Months
                                case "X106^051": endDate = DateTime.Today.AddMonths(num); break;
                                //Next n Weeks
                                case "X106^052": endDate = DateTime.Today.AddDays(7 * num); break;
                                //Next n Days
                                case "X106^053": endDate = DateTime.Today.AddDays(num); break;
                                //Next Year
                                case "X106^054": endDate = DateTime.Today.AddYears(1); break;
                                //Next Month
                                case "X106^055": endDate = DateTime.Today.AddMonths(1); break;
                                //Next Week
                                case "X106^056": endDate = DateTime.Today.AddDays(7); break;
                                //Tomorrow
                                case "X106^057": endDate = DateTime.Today.AddDays(1); break;
                            }
                            param += string.Format("{0};{1}", startDate.ToString("yyyyMMdd"), endDate.ToString("yyyyMMdd"));
                        }
                        ctr++;
                    }
                    filterParam = param;
                    return true;
                }
                else
                {
                    Control ctlParent = pnlReportCtl;
                    BaseCustomReportSettingCtl ctl = (BaseCustomReportSettingCtl)ctlParent.Controls[0];
                    param = ctl.GetReportParameter();
                    return true;
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string param = "";
            string errMessage = "";
            if (OnProcessReport(ref param, ref errMessage))
            {
                string result = "";

                string reportCode = string.Format("ReportCode = '{0}'", hdnReportCode.Value);
                ReportMaster rm = BusinessLayer.GetReportMasterList(reportCode).FirstOrDefault();
                string fileName = string.Format(@"{0}_{1}_{2}_{3}", rm.ClassName, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112), DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString());
                try
                {
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".csv");
                    Response.Charset = "";
                    Response.ContentType = "application/text";

                    StringBuilder sbResult = new StringBuilder();

                    if (rm.GCDataSourceType == Constant.DataSourceType.STORED_PROCEDURE)
                    {
                        List<dynamic> lstDynamic = null;
                        List<Variable> lstVariable = new List<Variable>();
                        List<vReportParameter> listReportParameter = BusinessLayer.GetvReportParameterList(string.Format("ReportID = {0} ORDER BY DisplayOrder", rm.ReportID));
                        string[] value = filterParam.Split('|');
                        for (int i = 0; i < listReportParameter.Count; ++i)
                        {
                            vReportParameter reportParameter = listReportParameter[i];
                            lstVariable.Add(new Variable { Code = reportParameter.FieldName, Value = GetFilterExpression(value[i]) });
                        }

                        lstDynamic = BusinessLayer.GetDataReport(rm.ObjectTypeName, lstVariable);

                        dynamic fields = lstDynamic[0];

                        foreach (var prop in fields.GetType().GetProperties())
                        {
                            sbResult.Append(prop.Name);
                            sbResult.Append(",");
                        }

                        sbResult.Append("\r\n");

                        for (int i = 0; i < lstDynamic.Count; ++i)
                        {
                            dynamic entity = lstDynamic[i];

                            foreach (var prop in entity.GetType().GetProperties())
                            {
                                sbResult.Append(prop.GetValue(entity, null));
                                sbResult.Append(",");
                            }

                            sbResult.Append("\r\n");
                        }
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
        }
    }
}