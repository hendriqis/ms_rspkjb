using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Controls;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class PreviousPatientList : BasePageTrx
    {
        protected string IntellisenseHints = "";
        protected string TextSearch = "";
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.REGISTERED_PATIENT;
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

        protected override void InitializeDataControl()
        {
            string filterSetVar = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')",
                                                    AppSession.UserLogin.HealthcareID,
                                                    Constant.SettingParameter.EM_DURATION_FILTER_DATE_PREVIOUS_PATIENT);
            List<SettingParameterDt> lstSetVarDt = BusinessLayer.GetSettingParameterDtList(filterSetVar);

            string filterPeriodeDateStrSetvar = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.EM_DURATION_FILTER_DATE_PREVIOUS_PATIENT).FirstOrDefault().ParameterValue;
            int filterPeriodeDate = -7;
            if (filterPeriodeDateStrSetvar != "")
            {
                filterPeriodeDate = Convert.ToInt32(filterPeriodeDateStrSetvar) * -1;
            }

            txtRealisationDateOPFrom.Text = DateTime.Now.AddDays(filterPeriodeDate).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtRealisationDateOPTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtRealisationDateIPFrom.Text = DateTime.Today.AddDays(filterPeriodeDate).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtRealisationDateIPTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            List<Department> lstDept = BusinessLayer.GetDepartmentList(string.Format("DepartmentID != '{0}' AND IsHasRegistration = 1 AND IsActive = 1", Constant.Facility.PHARMACY));
            lstDept = lstDept.OrderBy(lst => lst.TabOrder).ToList();
            Methods.SetComboBoxField<Department>(cboPatientFrom, lstDept, "DepartmentName", "DepartmentID");

            if (AppSession.UserLogin.DepartmentID != null)
                cboPatientFrom.Value = AppSession.UserLogin.DepartmentID;
            else
                cboPatientFrom.SelectedIndex = 0;

            int lastSessionpageIndex = 1;
            LastOpenFilteringForm(ref lastSessionpageIndex); //terakhir buka filter apa saja

            BindGridView(1, true, ref PageCount);

            refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;
        }

        private void LastOpenFilteringForm(ref int lastpageIndex)
        {
            if (AppSession.LastPreviousPatientList != null)
            {
                LastContentPreviousPatientList2 oData = AppSession.LastPreviousPatientList;
                if (oData != null)
                {
                    txtBarcodeEntry.Text = oData.BarcodeNo;
                    if (!string.IsNullOrEmpty(oData.DepartmentID))
                    {
                        cboPatientFrom.Value = oData.DepartmentID;
                        hdnServiceUnitID.Value = oData.HealthcareServiceUnitID.ToString();
                        txtServiceUnitCode.Text = oData.ServiceUnitCode;
                        txtServiceUnitName.Text = oData.ServiceUnitName;
                        if (cboPatientFrom.Value.ToString() == Constant.Facility.INPATIENT)
                        {
                            txtRealisationDateIPFrom.Text = oData.RealisationDateFrom;
                            txtRealisationDateIPTo.Text = oData.RealisationDateTo;
                        }
                        else
                        {
                            txtRealisationDateOPFrom.Text = oData.RealisationDateFrom;
                            txtRealisationDateOPTo.Text = oData.RealisationDateTo;
                        }
                        txtSearchView.Text = oData.QuickText;
                        hdnQuickText.Value = oData.QuickText.Equals("Search") ? "" : oData.QuickText;
                        hdnFilterExpressionQuickSearch.Value = oData.QuickFilterExpression;
                        chkIncludeOpenPatient.Checked = oData.IsIncludeOpenPatient;
                        chkIsIgnoreDate.Checked = oData.IsIgnoreDate;
                        chkIsIgnoreDateInpatient.Checked = oData.IsIgnoreDateInpatient;
                        lastpageIndex = oData.LastPageIndex;
                        hdnLastpageIndex.Value = lastpageIndex.ToString();
                    }
                }
                AppSession.LastPreviousPatientList = null;
            }

        }
        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                int paggingIndex = 1;
                if (param[0] == "changepage")
                {
                    paggingIndex = Convert.ToInt32(param[1]);
                    BindGridView(paggingIndex, false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridView(paggingIndex, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }

                hdnLastpageIndex.Value = paggingIndex.ToString();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        public String GetFilterExpression()
        {
            string filterExpression = "";

            if ((cboPatientFrom.Value.ToString() != Constant.Facility.INPATIENT) && !chkIsIgnoreDate.Checked)
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }
                filterExpression += string.Format("VisitDate BETWEEN '{0}' AND '{1}'", Helper.GetDatePickerValue(txtRealisationDateOPFrom).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtRealisationDateOPTo).ToString(Constant.FormatString.DATE_FORMAT_112));
            }

            if ((cboPatientFrom.Value.ToString() == Constant.Facility.INPATIENT) && !chkIsIgnoreDateInpatient.Checked)
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }
                filterExpression += string.Format("VisitDate BETWEEN '{0}' AND '{1}'", Helper.GetDatePickerValue(txtRealisationDateIPFrom).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtRealisationDateIPTo).ToString(Constant.FormatString.DATE_FORMAT_112));
            }

            if (cboPatientFrom.Value.ToString() == Constant.Facility.EMERGENCY)
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }

                if (!chkIncludeOpenPatient.Checked)
                {
                    filterExpression += string.Format("GCVisitStatus IN ('{0}','{1}','{2}')", Constant.VisitStatus.CLOSED, Constant.VisitStatus.PHYSICIAN_DISCHARGE, Constant.VisitStatus.DISCHARGED);
                }
                else
                {
                    filterExpression += string.Format("GCVisitStatus IN ('{0}','{1}')", Constant.VisitStatus.CHECKED_IN, Constant.VisitStatus.RECEIVING_TREATMENT);
                }
            }
            else
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }
                filterExpression += string.Format("GCVisitStatus IN ('{0}','{1}','{2}')", Constant.VisitStatus.CLOSED, Constant.VisitStatus.PHYSICIAN_DISCHARGE, Constant.VisitStatus.DISCHARGED);
            }

            if (cboPatientFrom.Value != null)
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }
                filterExpression += string.Format("DepartmentID = '{0}'", cboPatientFrom.Value);
            }

            if (hdnServiceUnitID.Value != "0" && hdnServiceUnitID.Value != "")
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }
                filterExpression += string.Format("HealthcareServiceUnitID = {0}", hdnServiceUnitID.Value);
            }
            else
            {
                List<GetServiceUnitUserList> lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, cboPatientFrom.Value.ToString(), "");
                string lstHealtcareServiceUnitID = String.Join(",", lstServiceUnit.Select(p => p.HealthcareServiceUnitID));

                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }
                filterExpression += string.Format("HealthcareServiceUnitID IN ({0})", lstHealtcareServiceUnitID);
            }

            if (hdnFilterExpressionQuickSearch.Value != "")
            {
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            }

            if (AppSession.EM0036 == "1")
            {
                if (cboPatientFrom.Value.ToString() != Constant.Facility.OUTPATIENT)
                {
                    //filterExpression += string.Format(" AND ParamedicID = {0}", AppSession.UserLogin.ParamedicID);
                    filterExpression += string.Format(" AND (ParamedicID = {0} OR RegistrationID IN (SELECT pt.RegistrationID FROM ParamedicTeam pt WITH(NOLOCK) WHERE pt.IsDeleted = 0 AND pt.RegistrationID = vConsultVisit16.RegistrationID AND pt.ParamedicID = {0}))", AppSession.UserLogin.ParamedicID);
                    //filterExpression += string.Format(" AND (ParamedicID = {0} OR {1} IN (SELECT ParamedicID FROM ParamedicTeam WITH (NOLOCK) WHERE RegistrationID = vConsultVisit9.RegistrationID))", AppSession.UserLogin.ParamedicID, AppSession.UserLogin.ParamedicID);
                }
                else
                {
                    filterExpression += string.Format(" AND ParamedicID = {0}", AppSession.UserLogin.ParamedicID);
                }
            }
            else
            {
                if (cboPatientFrom.Value.ToString() != Constant.Facility.EMERGENCY)
                {
                    //filterExpression += string.Format(" AND ParamedicID = {0}", AppSession.UserLogin.ParamedicID);
                    filterExpression += string.Format(" AND (ParamedicID = {0} OR RegistrationID IN (SELECT pt.RegistrationID FROM ParamedicTeam pt WITH(NOLOCK) WHERE pt.IsDeleted = 0 AND pt.RegistrationID = vConsultVisit16.RegistrationID AND pt.ParamedicID = {0}))", AppSession.UserLogin.ParamedicID);
                    //filterExpression += string.Format(" AND (ParamedicID = {0} OR {1} IN (SELECT ParamedicID FROM ParamedicTeam WITH (NOLOCK) WHERE RegistrationID = vConsultVisit9.RegistrationID))", AppSession.UserLogin.ParamedicID, AppSession.UserLogin.ParamedicID);
                }
            }

            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvConsultVisit16RowCountByFieldName(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_PATIENT_LIST);
            }

            List<vConsultVisit16> lstEntity = BusinessLayer.GetvConsultVisit16List(filterExpression, Constant.GridViewPageSize.GRID_PATIENT_LIST, pageIndex, "RegistrationNo DESC");
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vConsultVisit16 entity = e.Item.DataItem as vConsultVisit16;
                HtmlGenericControl divDischargeDate = e.Item.FindControl("divDischargeDate") as HtmlGenericControl;

                if (entity.DischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
                    divDischargeDate.Style.Add("display", "none");
                else
                    divDischargeDate.InnerHtml = string.Format("{0} : {1} {2}", GetLabel("Pulang"), entity.DischargeDateInString, entity.DischargeTime);

                if (entity.DepartmentID == Constant.Facility.EMERGENCY)
                {
                    HtmlTableCell tdIndicator = e.Item.FindControl("tdIndicator") as HtmlTableCell;
                    if (!String.IsNullOrEmpty(entity.TriageColor))
                    {
                        tdIndicator.Style.Add("background-color", entity.TriageColor);
                    }
                }

                //if (entity.GCVisitStatus != Constant.VisitStatus.OPEN)
                //{
                //    HtmlGenericControl divChiefComplaint = e.Item.FindControl("divChiefComplaint") as HtmlGenericControl;
                //    vChiefComplaint oChiefComplaint = BusinessLayer.GetvChiefComplaintList(string.Format("VisitID = {0} AND IsDeleted = 0", entity.VisitID)).FirstOrDefault();
                //    divChiefComplaint.InnerHtml = oChiefComplaint != null ? "X" : "";

                //    HtmlGenericControl divDiagnosis = e.Item.FindControl("divDiagnosis") as HtmlGenericControl;
                //    vPatientDiagnosis oDiagnosis = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", entity.VisitID)).FirstOrDefault();
                //    divDiagnosis.InnerHtml = oDiagnosis != null ? "X" : "";
                //}

                if (entity.DepartmentID != Constant.Facility.INPATIENT)
                {
                    HtmlTableCell tdServiceFlag = e.Item.FindControl("tdServiceFlag") as HtmlTableCell;
                    if (tdServiceFlag != null)
                    {
                        if (entity.GCVisitStatus != Constant.VisitStatus.OPEN && entity.GCVisitStatus != Constant.VisitStatus.CHECKED_IN)
                        {
                            tdServiceFlag.Style.Add("background-color", "#192a56");
                        }
                    }
                }

                if (entity.DepartmentID == Constant.Facility.OUTPATIENT)
                {
                    HtmlImage imgPatientSatisfactionLevelImageUri = (HtmlImage)e.Item.FindControl("imgPatientSatisfactionLevelImageUri");
                    imgPatientSatisfactionLevelImageUri.Src = ResolveUrl(string.Format("~/Libs/Images/Status/PatientStatus_{0}.png", entity.PatientSatisfactionLevel));

                }
            }
        }


        protected void btnOpenTransactionDt_Click(object sender, EventArgs e)
        {
            if (hdnTransactionNo.Value != "")
            {
                //LastContentPreviousPatientList lc = new LastContentPreviousPatientList()
                //{
                //    DepartmentID = cboPatientFrom.Value.ToString(),
                //    HealthcareServiceUnitID = Convert.ToInt32(hdnServiceUnitID.Value),
                //    ServiceUnitCode = txtServiceUnitCode.Text,
                //    RegistrationDate = txtRealisationDate.Text,
                //    QuickFilterExpression = hdnFilterExpressionQuickSearch.Value
                //};
                //AppSession.LastContentPreviousPatientList = lc;

                vConsultVisit4 entity = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", hdnTransactionNo.Value))[0];
                RegisteredPatient pt = new RegisteredPatient();
                pt.MRN = entity.MRN;
                pt.MedicalNo = entity.MedicalNo;
                pt.GCGender = entity.GCGender;
                pt.GCSex = entity.GCSex;
                pt.DateOfBirth = entity.DateOfBirth;
                pt.RegistrationID = entity.RegistrationID;
                pt.RegistrationNo = entity.RegistrationNo;
                pt.RegistrationDate = entity.RegistrationDate;
                pt.RegistrationTime = entity.RegistrationTime;
                pt.VisitID = entity.VisitID;
                pt.VisitDate = entity.VisitDate;
                pt.VisitTime = entity.VisitTime;
                pt.StartServiceDate = entity.StartServiceDate;
                pt.StartServiceTime = entity.StartServiceTime;
                pt.DischargeDate = entity.DischargeDate;
                pt.DischargeTime = entity.DischargeTime;
                pt.GCCustomerType = entity.GCCustomerType;
                pt.BusinessPartnerID = entity.BusinessPartnerID;
                pt.ParamedicID = entity.ParamedicID;
                pt.ParamedicCode = entity.ParamedicCode;
                pt.ParamedicName = entity.ParamedicName;
                pt.SpecialtyID = entity.SpecialtyID;
                pt.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
                pt.DepartmentID = entity.DepartmentID;
                pt.ServiceUnitName = entity.ServiceUnitName;
                pt.RoomCode = entity.RoomCode;
                pt.BedCode = entity.BedCode;
                pt.DepartmentID = entity.DepartmentID;
                pt.ChargeClassID = entity.ChargeClassID;
                pt.ClassID = entity.ClassID;
                pt.GCRegistrationStatus = entity.GCVisitStatus;
                pt.IsLockDown = entity.IsLockDown;
                pt.IsBillingReopen = entity.IsBillingReopen;
                pt.IsPlanDischarge = entity.IsPlanDischarge;
                AppSession.RegisteredPatient = pt;

                LastContentPreviousPatientList2 lcp = new LastContentPreviousPatientList2()
                {
                    BarcodeNo = tmptxtBarcodeEntry.Value,
                    DepartmentID = tmphdnDepartmentID.Value,
                    HealthcareServiceUnitID = entity.HealthcareServiceUnitID.ToString(),
                    ServiceUnitCode = entity.ServiceUnitCode,
                    ServiceUnitName = entity.ServiceUnitName,
                    RealisationDate = tmptxtRealisationDateOPFrom.Value,
                    RealisationDateFrom = tmptxtRealisationDateFrom.Value,
                    RealisationDateTo = tmptxtRealisationDateTo.Value,
                    IsIgnoreDate = chkIsIgnoreDate.Checked,
                    IsIgnoreDateInpatient = chkIsIgnoreDateInpatient.Checked,
                    IsIncludeOpenPatient = chkIncludeOpenPatient.Checked,
                    QuickFilterExpression = tmphdnFilterExpressionQuickSearch.Value,
                    QuickText = tmphdnQuickText.Value,
                    LastPageIndex = Convert.ToInt32(tmplastpageIndex.Value),
                };
                AppSession.LastPreviousPatientList = lcp;
                //AppSession.LastFilteringForm = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}", txtBarcodeEntry.Text, cboPatientFrom.Value.ToString(), hdnServiceUnitID.Value, txtRealisationDate.Text, chkIsIgnoreDate.Checked, txtSearchView.Text, chkIncludeOpenPatient.Checked, hdnFilterExpressionQuickSearch.Value);
                Response.Redirect("~/Program/PatientPage/PatientDataView.aspx");
            }
        }

        private string GetResponseRedirectUrl(vConsultVisit4 entity)
        {
            //LastContentPreviousPatientList lc = new LastContentPreviousPatientList()
            //{
            //    DepartmentID = cboPatientFrom.Value.ToString(),
            //    HealthcareServiceUnitID = Convert.ToInt32(hdnServiceUnitID.Value),
            //    ServiceUnitCode = txtServiceUnitCode.Text,
            //    RegistrationDate = txtRealisationDate.Text,
            //    QuickFilterExpression = hdnFilterExpressionQuickSearch.Value
            //};
            //AppSession.LastContentPreviousPatientList = lc;

            string url = "";
            if (entity != null)
            {
                RegisteredPatient pt = new RegisteredPatient();
                pt.MRN = entity.MRN;
                pt.MedicalNo = entity.MedicalNo;
                pt.GCGender = entity.GCGender;
                pt.GCSex = entity.GCSex;
                pt.DateOfBirth = entity.DateOfBirth;
                pt.RegistrationID = entity.RegistrationID;
                pt.RegistrationNo = entity.RegistrationNo;
                pt.RegistrationDate = entity.RegistrationDate;
                pt.RegistrationTime = entity.RegistrationTime;
                pt.VisitID = entity.VisitID;
                pt.VisitDate = entity.VisitDate;
                pt.VisitTime = entity.VisitTime;
                pt.StartServiceDate = entity.StartServiceDate;
                pt.StartServiceTime = entity.StartServiceTime;
                pt.DischargeDate = entity.DischargeDate;
                pt.DischargeTime = entity.DischargeTime;
                pt.GCCustomerType = entity.GCCustomerType;
                pt.BusinessPartnerID = entity.BusinessPartnerID;
                pt.ParamedicID = entity.ParamedicID;
                pt.ParamedicCode = entity.ParamedicCode;
                pt.ParamedicName = entity.ParamedicName;
                pt.SpecialtyID = entity.SpecialtyID;
                pt.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
                pt.DepartmentID = entity.DepartmentID;
                pt.ServiceUnitName = entity.ServiceUnitName;
                pt.RoomCode = entity.RoomCode;
                pt.BedCode = entity.BedCode;
                pt.DepartmentID = entity.DepartmentID;
                pt.ChargeClassID = entity.ChargeClassID;
                pt.ClassID = entity.ClassID;
                pt.GCRegistrationStatus = entity.GCVisitStatus;
                pt.IsLockDown = entity.IsLockDown;
                pt.IsBillingReopen = entity.IsBillingReopen;
                pt.IsPlanDischarge = entity.IsPlanDischarge;
                AppSession.RegisteredPatient = pt;

                url = ResolveClientUrl("~/Program/PatientPage/PatientDataView.aspx");
            }
            return url;
        }

        protected void cbpBarcodeEntryProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string parameter = txtBarcodeEntry.Text.Replace(@"\r\n", Environment.NewLine);
            string registrationNo = txtBarcodeEntry.Text;

            string filterExpression = string.Format("RegistrationNo = '{0}'", registrationNo);
            vConsultVisit4 entity = BusinessLayer.GetvConsultVisit4List(filterExpression).FirstOrDefault();
            string url = "";
            if (entity != null)
                url = GetResponseRedirectUrl(entity);

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpUrl"] = url;
        }
    }
}