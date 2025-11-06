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
    public partial class NewPatientVisitList : BasePageTrx
    {
        protected int PageCount = 1;

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
        }

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.PATIENT_LIST;
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
        }

        protected override void InitializeDataControl()
        {
            txtRealisationDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            /////lblDateTime.InnerText = DateTime.Now.ToString(Constant.FormatString.DATE_TIME_FORMAT);
            ////lblHeader.InnerText = string.Format("Hello, {0}", AppSession.UserLogin.UserFullName);
            
            if (string.IsNullOrEmpty(AppSession.LastSelectedDepartment) && AppSession.LastSelectedDepartment == null) {
                hdnDepartmentID.Value = Constant.Facility.OUTPATIENT;
            }
            base.IsLoadFirstRecord = false;
            if (AppSession.UserConfig == null)
            {
                SetDepartmentFromLastSelected();
            }
            else
            {
                if (!string.IsNullOrEmpty(AppSession.UserConfig.DepartmentID))
                {
                    hdnSelectedDepartment.Value = AppSession.UserConfig.DepartmentID;
                }else if(!string.IsNullOrEmpty(AppSession.LastSelectedDepartment) && AppSession.LastSelectedDepartment != null){
                    hdnSelectedDepartment.Value = AppSession.LastSelectedDepartment;
                }
                else{
                    SetDepartmentFromLastSelected();
                }
            }

            AppSession.LastSelectedDepartment = hdnSelectedDepartment.Value;

            RepeterData();
            BindGridView();
        }
        private void SetDepartmentFromLastSelected()
        {
            if (!String.IsNullOrEmpty(AppSession.LastSelectedDepartment))
            {
                hdnSelectedDepartment.Value = AppSession.LastSelectedDepartment;
            }
            else
            {
                if (AppSession.UserLogin.DepartmentID != null)
                {
                    hdnSelectedDepartment.Value = AppSession.UserLogin.DepartmentID;
                }
                else if (AppSession.LastSelectedDepartment != null) 
                {
                    hdnSelectedDepartment.Value = AppSession.LastSelectedDepartment.ToString();
                }
                else
                {
                    hdnSelectedDepartment.Value = "0";
                }

                AppSession.LastSelectedDepartment = hdnSelectedDepartment.Value.ToString();
            }
        }
        protected void cbpViewTime_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter == "refreshHour")
            {
               // hdnTimeNow.Value = DateTime.Now.ToString(Constant.FormatString.DATE_TIME_FORMAT);
                result = string.Format("refreshHour");
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView();
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridView();
                    result = "refresh";
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewDt_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            string HsuID = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "find")
                {
                    BindGridViewDt(ref HsuID);
                    result = string.Format("find|{0}", HsuID);
                }
                else // refresh
                {

                    BindGridViewDt(ref HsuID);
                    result = "refresh";
                }




            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        public void RepeterData()
        {
            List<Department> lstDept = BusinessLayer.GetDepartmentList(string.Format("DepartmentID != '{0}' AND IsHasRegistration = 1 AND IsActive = 1 ORDER BY TabOrder ASC", Constant.Facility.PHARMACY));
            RepterDepartement.DataSource = lstDept;
            RepterDepartement.DataBind();
        }
        private void BindGridView()
        {
            Int32 ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
            String deptID = hdnDepartmentID.Value;
            String tglVisit = Helper.GetDatePickerValue(txtRealisationDate).ToString(Constant.FormatString.DATE_FORMAT_112);
            Int32 isCloseReg = 0;
            if (chkIsIncludeClosed.Checked)
            {
                isCloseReg = 1;
            }
           
            List<GetPatientDoctorListSummary> lstData = BusinessLayer.GetPatientDoctorListSummary(ParamedicID, deptID, tglVisit, isCloseReg, 0);
            grdView.DataSource = lstData;
            grdView.DataBind();
        }

        private void BindGridViewDt(ref String Hsuid)
        {
            Int32 ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
            String deptID = hdnDepartmentID.Value;
            String tglVisit = Helper.GetDatePickerValue(txtRealisationDate).ToString(Constant.FormatString.DATE_FORMAT_112);
            Hsuid = string.Empty;
            Int32 isCloseReg = 0;
            if (chkIsIncludeClosed.Checked)
            {
                isCloseReg = 1;
            }

            if (!string.IsNullOrEmpty(hdnHsuID.Value))
            {
                Int32 hsuID = Convert.ToInt32(hdnHsuID.Value);
                List<GetPatientDoctorListSummaryDetail> lstData = BusinessLayer.GetPatientDoctorListSummaryDetail(ParamedicID, deptID, tglVisit, isCloseReg, hsuID, txtFindPatientName.Text);
                GetPatientDoctorListSummaryDetail oData = lstData.FirstOrDefault();
                if(oData != null){
                  Hsuid = oData.HealthcareServiceUnitID.ToString();
                }
              
                lvwView.DataSource = lstData;
                lvwView.DataBind();
            }
            else
            {
                List<GetPatientDoctorListSummaryDetail> lstData = new List<GetPatientDoctorListSummaryDetail>();
                lvwView.DataSource = lstData;
                lvwView.DataBind();
            }
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                GetPatientDoctorListSummaryDetail entity = e.Item.DataItem as GetPatientDoctorListSummaryDetail;
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

                    //if (hdnPhysicianPatientCall.Value == "1")
                    //{
                    //    divPatientCall.Style.Remove("display");
                    //}
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
                //string serviceUnitID = string.IsNullOrEmpty(hdnServiceUnitID.Value) ? entity.HealthcareServiceUnitID.ToString() : hdnServiceUnitID.Value;
                //string serviceUnitName = string.IsNullOrEmpty(hdnServiceUnitID.Value) ? entity.ServiceUnitName : cboServiceUnit.Text;

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
                AppSession.LastSelectedServiceUnit = string.Format("{0}|{1}|{2}", entity.ServiceUnitID, departmentID, entity.ServiceUnitName);
                Response.Redirect(string.Format("~/Program/PatientPage/PatientDataView.aspx?type={0}", hdnPatientPageByDepartment.Value));
            }
        }
    }
}