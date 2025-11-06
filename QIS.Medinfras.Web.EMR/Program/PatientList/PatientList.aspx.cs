using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using System.Web.UI.HtmlControls;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class PatientList : BasePageTrx
    {
        private const string STATUS_IMAGE_PATH = "~/libs/Images/Dashboard/";
        List<GetCountVisitDashboard> lstData;
        public List<PatientListInfo> lstPatientData = new List<PatientListInfo>();
        public List<vConsultVisit16> lstVisitData;
        public class PatientListInfo
        {
            public int HealthcareServiceUnitID { get; set; }
            public string ServiceUnitCode { get; set; }
            public string ServiceUnitName { get; set; }
            public int NoOfPatients { get; set; }
        }
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.PATIENT_LIST;
        }
        protected override void InitializeDataControl()
        {
            SetControlProperties(); 
            //if (AppSession.UserConfig == null)
            //{
            //    SetDepartmentFromLastSelected();
            //}
            //else
            //{
            //    if (!string.IsNullOrEmpty(AppSession.UserConfig.DepartmentID))
            //        cboPatientFrom.Value = AppSession.UserConfig.DepartmentID;
            //    else
            //        SetDepartmentFromLastSelected();
            //}
           /// AppSession.LastSelectedDepartment = cboPatientFrom.Value.ToString();

            txtRealisationDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
           
 
        }
        protected override void SetControlProperties()
        {
            string filterExpression = string.Format("ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL, Constant.SettingParameter.EMR_PATIENT_PAGE_BY_DEPARTMENT);
            List<SettingParameter> lstParameter = BusinessLayer.GetSettingParameterList(filterExpression);
            if (lstParameter.Count > 0)
            {
                hdnPatientPageByDepartment.Value = lstParameter.Where(t => t.ParameterCode == Constant.SettingParameter.EMR_PATIENT_PAGE_BY_DEPARTMENT).FirstOrDefault().ParameterValue;
            }
            else
            {
                 hdnPatientPageByDepartment.Value = "0";
            }

             List<Department> lstDept = BusinessLayer.GetDepartmentList(string.Format("DepartmentID != '{0}' AND IsHasRegistration = 1 AND IsActive = 1", Constant.Facility.PHARMACY));
            lstDept = lstDept.OrderBy(lst => lst.TabOrder).ToList();
            Methods.SetComboBoxField<Department>(cboPatientFrom, lstDept, "DepartmentName", "DepartmentID");
            cboPatientFrom.SelectedIndex = 0;
              
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
        #region service unit

        protected void cbpViewServiceUnit_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                BindGridViewServiceUnit(); 
                result = string.Format("refresh");
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        private void BindGridViewServiceUnit()
        {
            string date = Helper.GetDatePickerValue(txtRealisationDate).ToString(Constant.FormatString.DATE_FORMAT_112); 
            List<PatientListInfo> lstPatient = new List<PatientListInfo>();
            List<PhysicianPatientList> lstEntity = BusinessLayer.GetPhysicianPatientList(Convert.ToInt32(AppSession.UserLogin.ParamedicID), cboPatientFrom.Value.ToString(), date);
            List<PhysicianPatientList> list = lstEntity.GroupBy(lst => lst.HealthcareServiceUnitID).Select(grp => grp.First()).OrderBy(lst => lst.ServiceUnitName).ToList();
           
            foreach (PhysicianPatientList item in list)
            {
                PatientListInfo obj = new PatientListInfo();
                obj.HealthcareServiceUnitID = item.HealthcareServiceUnitID;
                obj.ServiceUnitCode = item.ServiceUnitCode;
                obj.ServiceUnitName = item.ServiceUnitName;
                obj.NoOfPatients = lstEntity.Count(lst => lst.ServiceUnitCode == item.ServiceUnitCode);
                lstPatient.Add(obj);
               
            }
            lstPatientData = lstPatient; 
          
            lvwView.DataSource = lstPatient;
            lvwView.DataBind();
             

           
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        { }
        #endregion

        protected void cbpViewDt_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                AppSession.LastSelectedDepartment = cboPatientFrom.Value.ToString();
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
         

        public String GetFilterExpression()
        {
            string filterExpression = "";

            if ((cboPatientFrom.Value.ToString() != Constant.Facility.INPATIENT && cboPatientFrom.Value.ToString() != Constant.Facility.EMERGENCY))
            {

                filterExpression += string.Format("VisitDate = '{0}'", Helper.GetDatePickerValue(txtRealisationDate).ToString(Constant.FormatString.DATE_FORMAT_112));
            }

            if (filterExpression != "")
            {
                filterExpression += " AND ";
            }
            filterExpression += string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}','{3}')", Constant.VisitStatus.CANCELLED, Constant.VisitStatus.OPEN, Constant.VisitStatus.PHYSICIAN_DISCHARGE, Constant.VisitStatus.DISCHARGED);

            if (filterExpression != "")
            {
                filterExpression += " AND ";
            }
            filterExpression += string.Format(" DepartmentID = '{0}'", cboPatientFrom.Value);

            string serviceUnitID = hdnHealthcareServiceUnitID.Value;
           
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

             

            return filterExpression;
        }

        private void BindGridViewDt(int pageIndex, bool isCountPageCount, ref int pageCount)
        {

            string filterExpression = GetFilterExpression();

            filterExpression = string.IsNullOrEmpty(filterExpression) ? "1=0" : filterExpression;

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvConsultVisit16RowCountByFieldName(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_LIMA_PULUH);
            }

            string orderBy = "Session ASC, QueueNo ASC";
            if (cboPatientFrom.Value != null)
            {
                if (cboPatientFrom.Value.ToString() == Constant.Facility.INPATIENT)
                {
                    orderBy = "BedCode ASC";
                }
            }

            List<vConsultVisit16> lstEntity = BusinessLayer.GetvConsultVisit16List(filterExpression, Constant.GridViewPageSize.GRID_LIMA_PULUH, pageIndex, orderBy);

            lvwViewDt.DataSource = lstEntity;
            lvwViewDt.DataBind();
        }
        protected void btnOpenTransactionDt_Click(object sender, EventArgs e)
        {
            base.IsLoadFirstRecord = true;
            IsNeedRebindingGridView = false;
            GoToPatientPage();
        }

        private void GoToPatientPage()
        {
            if (hdnVisitID.Value != "")
            {
                IsNeedRebindingGridView = false;

                vConsultVisit4 entity = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", hdnVisitID.Value)).FirstOrDefault();
                string departmentID = entity.DepartmentID;
              //  string serviceUnitID = entity.HealthcareServiceUnitID;
                    //string.IsNullOrEmpty(hdnServiceUnitID.Value) ? entity.HealthcareServiceUnitID.ToString() : hdnServiceUnitID.Value;
                //string serviceUnitName = 
                    ///string.IsNullOrEmpty(hdnServiceUnitID.Value) ? entity.ServiceUnitName : cboServiceUnit.Text;

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
               /// AppSession.LastSelectedDepartment = string.IsNullOrEmpty(hdnDepartmentID.Value) ? departmentID : hdnDepartmentID.Value;
                ////AppSession.LastSelectedServiceUnit = string.Format("{0}|{1}|{2}", serviceUnitID, departmentID, serviceUnitName);
                Response.Redirect(string.Format("~/Program/PatientPage/PatientDataView.aspx?type={0}", hdnPatientPageByDepartment.Value));
            }
        }

        private void BindGridView()
        {
             
        }

        private void BindGridViewPieChart()
        {
             
        }

        protected void cboDepartment_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            
        }

    }
}