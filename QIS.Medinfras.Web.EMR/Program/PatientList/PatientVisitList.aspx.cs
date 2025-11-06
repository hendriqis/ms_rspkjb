using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Service;
using QIS.Medinfras.Web.CommonLibs.Controls;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class PatientVisitList : BasePageTrx   {
        protected int PageCount = 1;

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
        }

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.MY_TODAY_PATIENT_VISIT_LIST;
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

        protected override void SetControlProperties()
        {
            txtRealisationDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            List<Department> lstDept = BusinessLayer.GetDepartmentList(string.Format("DepartmentID != '{0}' AND IsHasRegistration = 1 AND IsActive = 1", Constant.Facility.PHARMACY));
            lstDept = lstDept.OrderBy(lst => lst.TabOrder).ToList();
            Methods.SetComboBoxField<Department>(cboPatientFrom, lstDept, "DepartmentName", "DepartmentID");

            string filterExpression = string.Format("ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL, Constant.SettingParameter.EMR_PATIENT_PAGE_BY_DEPARTMENT);
            List<SettingParameter> lstParameter = BusinessLayer.GetSettingParameterList(filterExpression);
            if (lstParameter.Count > 0)
            {
                refreshGridInterval = lstParameter.Where(t => t.ParameterCode == Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).FirstOrDefault().ParameterValue;
                hdnPatientPageByDepartment.Value = lstParameter.Where(t => t.ParameterCode == Constant.SettingParameter.EMR_PATIENT_PAGE_BY_DEPARTMENT).FirstOrDefault().ParameterValue;
            }
            else
            {
                refreshGridInterval = "10";
                hdnPatientPageByDepartment.Value = "0";
            }
            //setpar dt 
            List<SettingParameterDt> lstSetparDt = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID='{0}' AND ParameterCode IN ('{1}') ", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.EM0099));
            hdnEM0099.Value = lstSetparDt.Where(p => p.ParameterCode == Constant.SettingParameter.EM0099).FirstOrDefault().ParameterValue;

            IsNeedRebindingGridView = true;
        }

        protected override void InitializeDataControl()
        {
            base.IsLoadFirstRecord = false;
            if (AppSession.UserConfig == null)
            {
                SetDepartmentFromLastSelected();
            }
            else
            {
                if (!string.IsNullOrEmpty(AppSession.UserConfig.DepartmentID))
                    cboPatientFrom.Value = AppSession.UserConfig.DepartmentID;                 
                else
                    SetDepartmentFromLastSelected();
            }

            AppSession.LastSelectedDepartment = cboPatientFrom.Value.ToString();

            //if (IsNeedRebindingGridView && hdnIsGoToPatientPage.Value == "0")
            //{
            //    BindGridView(1, true, ref PageCount);
            //}

            List<SettingParameterDt> lstSetvarDt = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '001' AND ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.EM_PHYSICIAN_PATIENT_CALL, Constant.SettingParameter.SA_BRIDGING_WITH_MEDINLINK));
            hdnPhysicianPatientCall.Value = lstSetvarDt.Where(w => w.ParameterCode == Constant.SettingParameter.EM_PHYSICIAN_PATIENT_CALL).FirstOrDefault().ParameterValue;
            hdnIsBridgingWithMedinlink.Value = lstSetvarDt.Where(w => w.ParameterCode == Constant.SettingParameter.SA_BRIDGING_WITH_MEDINLINK).FirstOrDefault().ParameterValue;
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                AppSession.LastSelectedDepartment = cboPatientFrom.Value.ToString();
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else if (param[0] == "call")
                {
                    int regID = Convert.ToInt32(param[1]);
                    int visitID = Convert.ToInt32(param[2]);
                    string roomCode = param[3];
                    if (hdnIsBridgingWithMedinlink.Value == "1")
                    {
                        NotificationRegistrationTicketNo(regID, visitID, roomCode);
                    }
                    
                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
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

        public String GetFilterExpression()
        {
            string filterExpression = "";

            if ((cboPatientFrom.Value.ToString() != Constant.Facility.INPATIENT && cboPatientFrom.Value.ToString() != Constant.Facility.EMERGENCY))
            {
                filterExpression += string.Format("VisitDate = '{0}'", Helper.GetDatePickerValue(txtRealisationDate).ToString(Constant.FormatString.DATE_FORMAT_112));
            }

            if (!chkIsIncludeClosed.Checked)
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }
                filterExpression += string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}','{3}','{4}')", Constant.VisitStatus.CANCELLED, Constant.VisitStatus.OPEN, Constant.VisitStatus.PHYSICIAN_DISCHARGE, Constant.VisitStatus.DISCHARGED, Constant.VisitStatus.CLOSED);
            }
            else
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }
                filterExpression += string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}','{3}')", Constant.VisitStatus.CANCELLED, Constant.VisitStatus.OPEN, Constant.VisitStatus.PHYSICIAN_DISCHARGE, Constant.VisitStatus.DISCHARGED);
            }

            if (filterExpression != "")
            {
                filterExpression += " AND ";
            }
            filterExpression += string.Format(" DepartmentID = '{0}'", cboPatientFrom.Value);

            string serviceUnitID = hdnServiceUnitID.Value;
            if (cboServiceUnit.Value != null)
            {
                serviceUnitID = cboServiceUnit.Value.ToString();
            }
            if (serviceUnitID != "0" && serviceUnitID != "")
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }
                filterExpression += string.Format("HealthcareServiceUnitID = {0}", serviceUnitID);
            }

            if (cboPatientFrom.Value.ToString() == Constant.Facility.INPATIENT || cboPatientFrom.Value.ToString() == Constant.Facility.OUTPATIENT || cboPatientFrom.Value.ToString() == Constant.Facility.EMERGENCY || cboPatientFrom.Value.ToString() == Constant.Facility.DIAGNOSTIC)
            {
                //filterExpression += string.Format(" AND ParamedicID = {0}", AppSession.UserLogin.ParamedicID);
                filterExpression += string.Format(" AND (ParamedicID = {0} OR RegistrationID IN (SELECT pt.RegistrationID FROM ParamedicTeam pt WITH(NOLOCK) WHERE pt.IsDeleted = 0 AND pt.RegistrationID = vConsultVisit16.RegistrationID AND pt.ParamedicID = {0}))", AppSession.UserLogin.ParamedicID);
                //filterExpression += string.Format(" AND (ParamedicID = {0} OR {1} IN (SELECT pt.ParamedicID FROM ParamedicTeam pt WITH(NOLOCK) WHERE pt.RegistrationID = vConsultVisit9.RegistrationID AND pt.IsDeleted = 0))", AppSession.UserLogin.ParamedicID, AppSession.UserLogin.ParamedicID);
                //filterExpression += string.Format(" AND ParamedicID IN (SELECT ParamedicID FROM ParamedicMaster WHERE ParamedicID = {0} UNION ALL SELECT pt.ParamedicID FROM ParamedicTeam pt WITH(NOLOCK) WHERE pt.RegistrationID = vConsultVisit9.RegistrationID AND pt.ParamedicID = {0} AND pt.IsDeleted = 0)", AppSession.UserLogin.ParamedicID);
            }
            else if (cboPatientFrom.Value.ToString() == Constant.Facility.MEDICAL_CHECKUP)
            {
                filterExpression += string.Format(" AND (ParamedicID = {0} OR RegistrationID IN (SELECT pt.RegistrationID FROM ParamedicTeam pt WITH(NOLOCK) WHERE pt.IsDeleted = 0 AND pt.RegistrationID = vConsultVisit16.RegistrationID AND pt.ParamedicID = {0}))", AppSession.UserLogin.ParamedicID);
                //filterExpression += string.Format(" AND ParamedicID = {0}", AppSession.UserLogin.ParamedicID);
            }
            else
            {
                filterExpression += string.Format(" AND (ParamedicID = {0})", AppSession.UserLogin.ParamedicID);
            }

            if (hdnFilterExpressionQuickSearch.Value != "")
            {
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            }

            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();

            filterExpression = string.IsNullOrEmpty(filterExpression) ? "1=0" : filterExpression;

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvConsultVisit16RowCountByFieldName(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_PATIENT_LIST);
            }

            string orderBy = "Session ASC, QueueNo ASC";
            if (cboPatientFrom.Value != null)
            {
                if (cboPatientFrom.Value.ToString() == Constant.Facility.INPATIENT)
                {
                    orderBy = "BedCode ASC";
                }
            }

            List<vConsultVisit16> lstEntity = BusinessLayer.GetvConsultVisit16List(filterExpression, Constant.GridViewPageSize.GRID_PATIENT_LIST, pageIndex, orderBy);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vConsultVisit16 entity = e.Item.DataItem as vConsultVisit16;
                HtmlGenericControl divDischargeDate = e.Item.FindControl("divDischargeDate") as HtmlGenericControl;
                HtmlGenericControl divPatientCall = e.Item.FindControl("divPatientCall") as HtmlGenericControl;
                

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

                if (entity.DepartmentID != Constant.Facility.INPATIENT)
                {
                    HtmlTableCell tdServiceFlag = e.Item.FindControl("tdServiceFlag") as HtmlTableCell;
                    if (tdServiceFlag != null)
                    {
                        //if (entity.GCVisitStatus != Constant.VisitStatus.OPEN && entity.GCVisitStatus != Constant.VisitStatus.CHECKED_IN)
                        //{
                        //    tdServiceFlag.Style.Add("background-color", "#192a56");
                        //}

                        //string filterCC = string.Format("VisitID = {0} AND IsDeleted = 0", entity.VisitID);
                        //List<ChiefComplaint> lstCC = BusinessLayer.GetChiefComplaintList(filterCC);

                        //if (entity.GCVisitStatus != Constant.VisitStatus.OPEN && entity.GCVisitStatus != Constant.VisitStatus.CHECKED_IN && lstCC.Count() > 0)
                        //{
                        //    tdServiceFlag.Style.Add("background-color", "#192a56");
                        //}
                        if (entity.GCVisitStatus != Constant.VisitStatus.OPEN && entity.GCVisitStatus != Constant.VisitStatus.CHECKED_IN && entity.IsHasChiefComplaint)
                        {
                            tdServiceFlag.Style.Add("background-color", "#192a56");
                        }
                     }
                }

                if (entity.DepartmentID == Constant.Facility.OUTPATIENT)
                {
                    HtmlImage imgPatientSatisfactionLevelImageUri = (HtmlImage)e.Item.FindControl("imgPatientSatisfactionLevelImageUri");
                    imgPatientSatisfactionLevelImageUri.Src = ResolveUrl(string.Format("~/Libs/Images/Status/PatientStatus_{0}.png", entity.PatientSatisfactionLevel));

                    if (hdnPhysicianPatientCall.Value == "1")
                    {
                        divPatientCall.Style.Remove("display");
                    }
                }
            }
        }


        protected void btnOpenTransactionDt_Click(object sender, EventArgs e)
        {
            base.IsLoadFirstRecord = true;
            IsNeedRebindingGridView = false;
            GoToPatientPage();
        }

        private void GoToPatientPage()
        {
            if (hdnTransactionNo.Value != "")
            {
                IsNeedRebindingGridView = false;

                vConsultVisit4 entity = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", hdnTransactionNo.Value)).FirstOrDefault();
                string departmentID = entity.DepartmentID;
                string serviceUnitID = string.IsNullOrEmpty(hdnServiceUnitID.Value) ? entity.HealthcareServiceUnitID.ToString() : hdnServiceUnitID.Value;
                string serviceUnitName = string.IsNullOrEmpty(hdnServiceUnitID.Value) ? entity.ServiceUnitName : cboServiceUnit.Text;

                RegisteredPatient pt = new RegisteredPatient();
                pt.MRN = entity.MRN;
                pt.MedicalNo = entity.MedicalNo;
                pt.PatientName = entity.PatientName;
                pt.GCGender = entity.GCGender;
                pt.RegistrationID = entity.RegistrationID;
                pt.RegistrationNo = entity.RegistrationNo;
                pt.LinkedRegistrationID = entity.LinkedRegistrationID;
                pt.VisitID = entity.VisitID;
                pt.VisitDate = entity.VisitDate;
                pt.VisitTime = entity.VisitTime;
                pt.ParamedicID = entity.ParamedicID;
                pt.SpecialtyID = entity.SpecialtyID;
                pt.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
                pt.ServiceUnitName = entity.ServiceUnitName;
                pt.RoomCode = entity.RoomCode;
                pt.BedCode = entity.BedCode;
                pt.DepartmentID = entity.DepartmentID;
                pt.ClassID = entity.ClassID;
                pt.ChargeClassID = entity.ChargeClassID;
                pt.StartServiceDate = entity.StartServiceDate;
                pt.StartServiceTime = entity.StartServiceTime;
                pt.GCCustomerType = entity.GCCustomerType;
                pt.BusinessPartnerID = entity.BusinessPartnerID;
                pt.DateOfBirth = entity.DateOfBirth;
                pt.IsPlanDischarge = entity.IsPlanDischarge;
                pt.IsUsingImplant = entity.IsUsingImplant;
                pt.GCRegistrationStatus = entity.GCRegistrationStatus;

                //if (pt.DepartmentID == Constant.Facility.OUTPATIENT && entity.GCVisitStatus == Constant.VisitStatus.CHECKED_IN) // diubah oleh RN 20200721 karna selain OP, tidak ada perubahan ke status Receiving Treatment
                if (entity.GCVisitStatus == Constant.VisitStatus.CHECKED_IN)
                {
                    ConsultVisit entityConsultVisit = BusinessLayer.GetConsultVisit(pt.VisitID);
                    entityConsultVisit.GCVisitStatus = Constant.VisitStatus.RECEIVING_TREATMENT;
                    if (String.IsNullOrEmpty(entityConsultVisit.StartServiceTime))
                    {
                        entityConsultVisit.StartServiceDate = DateTime.Now.Date;
                        entityConsultVisit.StartServiceTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

                        #region Calculate Elapsed Time
                        int minute = Convert.ToInt32(pt.VisitTime.Substring(3));
                        int hour = Convert.ToInt32(pt.VisitTime.Substring(0, 2));
                        DateTime VisitDateTime = new DateTime(pt.VisitDate.Year, pt.VisitDate.Month, pt.VisitDate.Day, hour, minute, 0);
                        double totalHours = (DateTime.Now - VisitDateTime).TotalHours;

                        double hours = Math.Floor(totalHours);
                        double minutes = Math.Floor((totalHours - hours) * 60);
                        #endregion

                        //entityConsultVisit.TimeElapsed1 = string.Format("{0}:{1}", hours.ToString().PadLeft(2, '0'), minutes.ToString().PadLeft(2, '0'));
                    }

                    pt.StartServiceDate = entityConsultVisit.StartServiceDate;
                    pt.StartServiceTime = entityConsultVisit.StartServiceTime;

                    entityConsultVisit.LastUpdatedBy = AppSession.UserLogin.UserID; // ditambahkan oleh RN 20200721

                    BusinessLayer.UpdateConsultVisit(entityConsultVisit);


                    Registration entityRegistration = BusinessLayer.GetRegistration(entityConsultVisit.RegistrationID); // region ini ditambahkan oleh RN 20200721
                    entityRegistration.GCRegistrationStatus = entityConsultVisit.GCVisitStatus;
                    entityRegistration.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateRegistration(entityRegistration);
                }

                AppSession.RegisteredPatient = pt;
                AppSession.LastSelectedDepartment = string.IsNullOrEmpty(hdnDepartmentID.Value) ? departmentID : hdnDepartmentID.Value;
                AppSession.LastSelectedServiceUnit = string.Format("{0}|{1}|{2}", serviceUnitID, departmentID, serviceUnitName);
                Response.Redirect(string.Format("~/Program/PatientPage/PatientDataView.aspx?type={0}", hdnPatientPageByDepartment.Value));
            }
        }

        protected void cbpBarcodeEntryProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string parameter = txtBarcodeEntry.Text.Replace(@"\r\n", Environment.NewLine);
            //string[] registrationInfo = txtBarcodeEntry.Text.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            //string registrationNo = registrationInfo[1];
            string registrationNo = txtBarcodeEntry.Text;

            string filterExpression = string.Format("RegistrationNo = '{0}'", registrationNo);
            vConsultVisit4 entity = BusinessLayer.GetvConsultVisit4List(filterExpression).FirstOrDefault();
            string url = "";
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            if (entity != null)
            {
                hdnTransactionNo.Value = entity.VisitID.ToString();
                url = string.Format("~/Program/PatientPage/PatientDataView.aspx?type={0}", hdnPatientPageByDepartment.Value);
                panel.JSProperties["cpUrl"] = url;
                GoToPatientPage();
            }
            else
            {
                panel.JSProperties["cpUrl"] = string.Empty;
            }
        }

        protected void cbpOnDepartmentChanged_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            PopulateServiceUnitList();
        }

        private void PopulateServiceUnitList()
        {
            if (cboPatientFrom.Value.ToString() != Constant.Facility.INPATIENT)
            {
                List<GetServiceUnitUserList> lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, cboPatientFrom.Value.ToString(), "IsUsingRegistration = 1");
                lstServiceUnit.Insert(0, new GetServiceUnitUserList { HealthcareServiceUnitID = 0, ServiceUnitName = "" });
                lstServiceUnit = lstServiceUnit.OrderBy(lst => lst.ServiceUnitName).ToList();
                Methods.SetComboBoxField<GetServiceUnitUserList>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
            }
            else
            {
                List<vHealthcareServiceUnitCustom> lstData = new List<vHealthcareServiceUnitCustom>();

                string filterExp = string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsDeleted  = 0  ORDER BY ServiceUnitName", AppSession.UserLogin.HealthcareID, Constant.Facility.INPATIENT);
                List<vHealthcareServiceUnitCustom> lstServiceUnit = BusinessLayer.GetvHealthcareServiceUnitCustomList(filterExp);
                if (hdnEM0099.Value == "0")
                {
                    lstServiceUnit.Insert(0, new vHealthcareServiceUnitCustom { HealthcareServiceUnitID = 0, ServiceUnitName = "" });
                }
                lstServiceUnit = lstServiceUnit.OrderBy(lst => lst.ServiceUnitName).ToList();

                Methods.SetComboBoxField<vHealthcareServiceUnitCustom>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
            }

            cboServiceUnit.DataBind();

            if (AppSession.UserConfig == null)
            {
                SetServiceUnitFromLastSelected();
            }
            else
            {
                if (AppSession.UserConfig.HealthcareServiceUnitID != null && AppSession.UserConfig.HealthcareServiceUnitID != 0)
                {
                    if (cboPatientFrom.Value.ToString() == AppSession.UserConfig.DepartmentID)
                    {
                        cboServiceUnit.Value = AppSession.UserConfig.HealthcareServiceUnitID.ToString();
                        hdnServiceUnitID.Value = AppSession.UserConfig.HealthcareServiceUnitID.ToString();
                    }
                    else
                    {
                        cboServiceUnit.SelectedIndex = 0;
                    }
                }
                else
                {
                    SetServiceUnitFromLastSelected();
                }
            }

            AppSession.LastSelectedServiceUnit = string.Format("{0}|{1}|{2}", cboServiceUnit.Value.ToString(), cboPatientFrom.Value.ToString(), cboServiceUnit.Text);
        }

        private void SetDepartmentFromLastSelected()
        {
            if (!String.IsNullOrEmpty(AppSession.LastSelectedDepartment))
            {
                cboPatientFrom.Value = AppSession.LastSelectedDepartment;
            }
            else
            {
                if (AppSession.UserLogin.DepartmentID != null)
                    cboPatientFrom.Value = AppSession.UserLogin.DepartmentID;
                else
                    cboPatientFrom.SelectedIndex = 0;

                AppSession.LastSelectedDepartment = cboPatientFrom.Value.ToString();
            }
        }

        private void SetServiceUnitFromLastSelected()
        {
            if (cboPatientFrom.Value.ToString() == Constant.Facility.INPATIENT)
            {
                string[] serviceUnitInfo = AppSession.LastSelectedServiceUnit.Split('|');
                if (serviceUnitInfo.Length >= 3)
                {
                    if (cboPatientFrom.Value.ToString() == AppSession.LastSelectedDepartment)
                    {
                        //Same Department
                        cboServiceUnit.Value = serviceUnitInfo[0];
                        hdnServiceUnitID.Value = serviceUnitInfo[0];
                    }
                    else
                    {
                        cboServiceUnit.SelectedIndex = 0;
                    }
                }
                else
                {
                    cboServiceUnit.SelectedIndex = 0;
                }
            }
            else
            {
                cboServiceUnit.SelectedIndex = 0;
            }
        }
        private void NotificationRegistrationTicketNo(int registrationID, int visitID, string roomCode)
        {
            vConsultVisit9 entity = BusinessLayer.GetvConsultVisit9List(string.Format("RegistrationID = {0} AND VisitID = {1}", registrationID, visitID)).FirstOrDefault();
            if (entity != null)
            {
                try
                {
                    APIMessageLog entityAPILog = new APIMessageLog()
                    {
                        MessageDateTime = DateTime.Now,
                        Recipient = Constant.BridgingVendor.MEDINLINK,
                        Sender = Constant.BridgingVendor.HIS,
                        IsSuccess = true
                    };
                    QueueService oService = new QueueService();
                    string apiResult = oService.MedinlinkPatientCallNotificationViaAPIMedin(entity, roomCode); ////oService.MedinlinkPatientCallNotification(entity, roomCode);
                    string[] apiResultInfo = apiResult.Split('|');
                    if (apiResultInfo[0] == "0")
                    {
                        entityAPILog.IsSuccess = false;
                        entityAPILog.MessageText = apiResultInfo[1];
                        entityAPILog.Response = apiResultInfo[1];
                        Exception ex = new Exception(apiResultInfo[1]);
                        Helper.InsertErrorLog(ex);
                    }
                    else
                    {
                        entityAPILog.MessageText = apiResultInfo[1];
                    }

                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                }
                catch (Exception ex)
                {
                    Helper.InsertErrorLog(ex);
                }
            }
        }
    }
}