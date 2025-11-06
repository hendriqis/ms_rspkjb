using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxEditors;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.Nursing.Program
{
    public partial class PatientVisitListLink : BasePageTrx
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Nursing.NURSING_PATIENT;
        }

        protected string GetRefreshGridInterval()
        {
            return refreshGridInterval;
        }

        protected String GetServiceUnitUserRoleFilterParameter()
        {
            return String.Format("{0};{1}", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID);
        }

        private string refreshGridInterval = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                txtBarcodeEntry.Focus();
            }
        }

        protected override void InitializeDataControl()
        {   
            InitializeControls();

            //BindGridView(1, true, ref PageCount);
        }

        private void InitializeControls()
        {
            //Manually set department's combo box data source - Read from department.lnk files
            #region Set Department DropDown
            SetDepartmentDropDown(cboPatientFrom, "DepartmentName", "DepartmentID");
            cboPatientFrom.SelectedIndex = 0; 
            #endregion

            #region Set Registration Date and Service Unit based on Department
            PopulateServiceUnit(cboPatientFrom.Value.ToString());
            #endregion
        }

        private void PopulateServiceUnit(string departmentCode) 
        {
            txtRegistrationDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            if (departmentCode == Constant.Facility.INPATIENT)
            {
                List<vlnk_ServiceUnitPerUser> lstServiceUnit = BusinessLayer.GetvlnkServiceUnitPerUserList(String.Format("UserID = '{0}' AND Department = '{1}'", AppSession.UserLogin.UserName, Constant.Facility.INPATIENT));
                Methods.SetComboBoxField<vlnk_ServiceUnitPerUser>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "ServiceUnitCode");
                cboServiceUnit.SelectedIndex = 0;
            }
            else if (departmentCode == Constant.Facility.EMERGENCY)
            {
            }
            else
            {
                //List<vServiceUnitLinkPerUser> lstServiceUnit = BusinessLayer.GetvServiceUnitLinkPerUserList(String.Format("UserID = '{0}' AND Tipe = '{1}'", AppSession.UserLogin.UserName, Constant.ServiceUnitLinkType.OUTPATIENT));
                //Methods.SetComboBoxField<vServiceUnitLinkPerUser>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "ServiceUnitCode");
                //cboServiceUnit.SelectedIndex = 0;
            } 
        }

        private void SetDepartmentDropDown(ASPxComboBox ctl, string textField, string valueField)
        {
            string departmentLinkFile = HttpContext.Current.Server.MapPath(Constant.Interface_Link_File.Link_Department);
            IEnumerable<string> lstDepartment = File.ReadAllLines(departmentLinkFile);
            foreach (string item in lstDepartment)
            {
                string[] info = item.Split('|');
                ctl.Items.Add(new ListEditItem(info[1],info[0]));
            }

            ctl.TextField = textField;
            ctl.ValueField = valueField;
            ctl.CallbackPageSize = 50;
            ctl.EnableCallbackMode = false;
            ctl.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
            ctl.DropDownStyle = DropDownStyle.DropDownList;
            ctl.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            PopulateServiceUnit(cboPatientFrom.Value.ToString());
            //int pageCount = 1;
            //string result = "";
            //if (e.Parameter != null && e.Parameter != "")
            //{
            //    string[] param = e.Parameter.Split('|');
            //    if (param[0] == "changepage")
            //    {
            //        BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
            //        result = "changepage";
            //    }
            //    else // refresh
            //    {

            //        BindGridView(1, true, ref pageCount);
            //        result = "refresh|" + pageCount;
            //    }
            //}

            //ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            //panel.JSProperties["cpResult"] = result;
        }

        public String GetFilterExpression() 
        {
            string filterExpression = string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}')", Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED, Constant.VisitStatus.OPEN);

            //if (hdnServiceUnitID.Value != "0" && hdnServiceUnitID.Value != "")
            //    filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", hdnServiceUnitID.Value);
            //else
            //{
            //    List<GetServiceUnitUserList> lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, cboPatientFrom.Value.ToString(), "");
            //    string lstHealtcareServiceUnitID = String.Join(",", lstServiceUnit.Select(p => p.HealthcareServiceUnitID));

            //    filterExpression += string.Format(" AND HealthcareServiceUnitID IN ({0})", lstHealtcareServiceUnitID);
            //}
            
            //if (cboPatientFrom.Value != null)
            //    filterExpression += string.Format(" AND DepartmentID = '{0}'", cboPatientFrom.Value);
            
            //if (cboPatientFrom.Value != null && cboPatientFrom.Value.ToString() != Constant.Facility.INPATIENT)
            //    filterExpression += string.Format(" AND VisitDate = '{0}'", Helper.GetDatePickerValue(txtRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112));
            //if (hdnFilterExpressionQuickSearchReg.Value != "")
            //    filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearchReg.Value);

            return filterExpression;
        }

        //private string GetFilterExpressionInpatient()
        //{
        //    string filterExpression = "";
        //    string id = Page.Request.QueryString["id"];
        //    filterExpression = string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}')", Constant.RegistrationStatus.CANCELLED, Constant.RegistrationStatus.OPEN, Constant.RegistrationStatus.CLOSED);
        //    if (cboServiceUnit.Value.ToString() != "0")
        //        filterExpression += string.Format(" AND ServiceUnitCode = '{0}'", cboServiceUnit.Value);
        //    else
        //        filterExpression += string.Format(" AND ServiceUnitCode IN ({0})", hdnLstHealthcareServiceUnitID.Value);
        //    if (hdnFilterExpressionQuickSearch.Value != "")
        //        filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
        //    filterExpression += " AND DepartmentID = 'INPATIENT'";

        //    return filterExpression;
        //}

        //private string GetFilterExpressionEmergency()
        //{
        //    string filterExpression = "";
        //    string id = Page.Request.QueryString["id"];
        //    filterExpression = string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}') AND VisitDate = '{3}'", Constant.RegistrationStatus.CANCELLED, Constant.RegistrationStatus.OPEN, Constant.RegistrationStatus.CLOSED, Helper.GetDatePickerValue(txtRegistrationDate.Text).ToString("yyyy-MM-dd"));
        //    if (hdnFilterExpressionQuickSearch.Value != "")
        //        filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
        //    filterExpression += " AND DepartmentID = 'EMERGENCY'";

        //    return filterExpression;
        //}

        //private string GetFilterExpressionOutpatient()
        //{
        //    string filterExpression = "";
        //    string id = Page.Request.QueryString["id"];
        //    filterExpression = string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}') AND VisitDate = '{3}'", Constant.RegistrationStatus.CANCELLED, Constant.RegistrationStatus.OPEN, Constant.RegistrationStatus.CLOSED, Helper.GetDatePickerValue(txtRegistrationDate.Text).ToString("yyyy-MM-dd"));
        //    if (cboServiceUnit.Value.ToString() != "0")
        //        filterExpression += string.Format(" AND ServiceUnitCode = '{0}'", cboServiceUnit.Value);
        //    else
        //        filterExpression += string.Format(" AND ServiceUnitCode IN ({0})", hdnLstHealthcareServiceUnitID.Value);
        //    if (hdnFilterExpressionQuickSearch.Value != "")
        //        filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
        //    filterExpression += " AND DepartmentID = 'OUTPATIENT'";

        //    return filterExpression;
        //}

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvConsultVisitRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_PATIENT_LIST);
            }

            List<vConsultVisit> lstEntity = BusinessLayer.GetvConsultVisitList(filterExpression, Constant.GridViewPageSize.GRID_PATIENT_LIST, pageIndex);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }
        
        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vConsultVisit entity = e.Item.DataItem as vConsultVisit;
                HtmlGenericControl divDischargeDate = e.Item.FindControl("divDischargeDate") as HtmlGenericControl;
                if (entity.DischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
                    divDischargeDate.Style.Add("display", "none");
                else
                    divDischargeDate.InnerHtml = string.Format("{0} : {1} {2}", GetLabel("Pulang"), entity.DischargeDateInString, entity.DischargeTime);

                HtmlImage imgPatientSatisfactionLevelImageUri = (HtmlImage)e.Item.FindControl("imgPatientSatisfactionLevelImageUri");
                imgPatientSatisfactionLevelImageUri.Src = ResolveUrl(string.Format("~/Libs/Images/Status/PatientStatus_{0}.png", entity.PatientSatisfactionLevel));
            }
        }


        protected void btnOpenTransactionDt_Click(object sender, EventArgs e)
        {
            if (hdnTransactionNo.Value != "")
            {   
                vConsultVisit entity = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", hdnTransactionNo.Value))[0];
                RegisteredPatient pt = new RegisteredPatient();
                pt.MRN = entity.MRN;
                pt.MedicalNo = entity.MedicalNo;
                pt.RegistrationID = entity.RegistrationID;
                pt.VisitID = entity.VisitID;
                pt.VisitDate = entity.VisitDate;
                pt.VisitTime = entity.VisitTime;
                pt.ParamedicID = entity.ParamedicID;
                pt.SpecialtyID = entity.SpecialtyID;
                pt.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
                pt.DepartmentID = entity.DepartmentID;
                pt.ClassID = entity.ClassID;
                AppSession.RegisteredPatient = pt;

                Response.Redirect("~/Program/PatientPage/PatientDataView.aspx");
            }
        }
    }
}