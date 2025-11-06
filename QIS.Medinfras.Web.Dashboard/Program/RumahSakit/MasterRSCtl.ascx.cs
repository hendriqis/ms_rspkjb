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
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.Dashboard.Program
{
    public partial class MasterRSCtl : BaseViewPopupCtl
    {
        private const string STATUS_IMAGE_PATH = "~/libs/Images/Dashboard/";
        List<ParamedicMaster> lstDoctor;
        List<ParamedicMaster> lstNurse;
        List<vHealthcareServiceUnit> lstClinic;
        List<vBed> lstBed;
        public override void InitializeDataControl(string param)
        {
            string filterExpressionDoctor = string.Format("GCParamedicMasterType = '{0}' AND IsDeleted = 0", Constant.ParamedicType.Physician);
            string filterExpressionNurse = string.Format("GCParamedicMasterType = '{0}' AND IsDeleted = 0", Constant.ParamedicType.Nurse);
            string filterExpressionClinic = string.Format("DepartmentID = '{0}' AND IsDeleted = 0", Constant.Facility.OUTPATIENT);
            string filterExpressionBed = string.Format("IsDeleted = 0");
            lstDoctor = BusinessLayer.GetParamedicMasterList(filterExpressionDoctor);
            lstNurse = BusinessLayer.GetParamedicMasterList(filterExpressionNurse);
            lstClinic = BusinessLayer.GetvHealthcareServiceUnitList(filterExpressionClinic);
            lstBed = BusinessLayer.GetvBedList(filterExpressionBed);
            List<Department> lst = BusinessLayer.GetDepartmentList(string.Format("IsHasRegistration=1 and IsActive=1"));
            Methods.SetComboBoxField<Department>(cboDepartment, lst, "DepartmentName", "DepartmentID");
            cboDepartment.SelectedIndex = 0;

            #region Patient
            var resultdoctor = lstDoctor.GroupBy(doctor => doctor.ParamedicID).Select(grp => grp.First()).ToList().OrderBy(x => x.ParamedicID);
            var resultnurse = lstNurse.GroupBy(nurse => nurse.ParamedicID).Select(grp => grp.First()).ToList().OrderBy(x => x.ParamedicID);
            var resultclinic = lstClinic.GroupBy(clinic => clinic.ServiceUnitID).Select(grp => grp.First()).ToList().OrderBy(x => x.ServiceUnitID);
            var resultbed = lstBed.GroupBy(bed => bed.BedID).Select(grp => grp.First()).ToList().OrderBy(x => x.BedID);
            lblParamedicCount.InnerText = string.Format("{0}", resultdoctor.Count());
            lblNurseCount.InnerText = string.Format("{0}", resultnurse.Count());
            lblClinicCount.InnerText = string.Format("{0}", resultclinic.Count());
            lblBedCount.InnerText = string.Format("{0}", resultbed.Count());
            imgDoctor.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "doctor.png");
            imgNurse.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "nurse.png");
            imgClinic.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "clinic.png");
            imgBed.Src = string.Format("{0}{1}", STATUS_IMAGE_PATH, "bed.png");
            #endregion

            BindGridView();
            BindGridViewPieChart();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                BindGridView();
                result = string.Format("refresh");
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView()
        {
            List<GetCountVisitPerDepartmentDashboard> lstEntity = BusinessLayer.GetCountVisitPerDepartmentDashboard(DateTime.Now.Year, DateTime.Now.Month, Convert.ToInt32(AppSession.UserLogin.ParamedicID), cboDepartment.Value.ToString());
            if (lstEntity.Count > 0)
            {
                List<ChartGraphV1> lstChart = new List<ChartGraphV1>();
                foreach (GetCountVisitPerDepartmentDashboard row in lstEntity)
                {
                    ChartGraphV1 entity = new ChartGraphV1();
                    entity.ID = row.ServiceUnitName;
                    entity.Value = row.CountVisit.ToString();
                    lstChart.Add(entity);
                }

                JsonChartData.Value = JsonConvert.SerializeObject(lstChart, Formatting.Indented);
            }
        }

        private void BindGridViewPieChart()
        {
            if (lstBed.Count > 0)
            {
                List<vBed> lstDataMale = lstBed.Where(t => t.GCGender == Constant.Gender.MALE && t.GCBedStatus == Constant.BedStatus.OCCUPIED).ToList();
                List<vBed> lstDataFemale = lstBed.Where(t => t.GCGender == Constant.Gender.FEMALE && t.GCBedStatus == Constant.BedStatus.OCCUPIED).ToList();
                List<vBed> lstDataUn = lstBed.Where(t => t.GCGender == Constant.Gender.UNSPECIFIED && t.GCBedStatus == Constant.BedStatus.OCCUPIED).ToList();

                Int32[] listData = new Int32[3];
                listData[0] = lstDataMale.Count();
                listData[1] = lstDataFemale.Count();
                listData[2] = lstDataUn.Count();

                JsonChartPieData.Value = JsonConvert.SerializeObject(listData, Formatting.Indented);
            }
        }

        protected void cboDepartment_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            List<Department> lst = BusinessLayer.GetDepartmentList(string.Format("IsHasRegistration=1 and IsActive=1"));
            Methods.SetComboBoxField<Department>(cboDepartment, lst, "DepartmentName", "DepartmentID");
            cboDepartment.SelectedIndex = -1;
        }
    }
}