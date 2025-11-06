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
    public partial class MasterRS2Ctl : BaseViewPopupCtl
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
            PMName();
            testPie();
            Department();
            BindGridView();
            BindGridViewPieChart();
            dischargeCond();
            KelasPasien();
            penjaminBayar();
            DiscMtd();
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

        private void testPie()
        {

                List<GetPatientInformation> lstData = BusinessLayer.GetPatientInformation(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());                
                List<GetPatientInformation> listProses = (from lp in lstData
                                                          where lp.RegistrationStatus != "Cancelled"
                                                          select lp).ToList();

                List<GetPatientInformation> listBatal = (from lb in lstData
                                                         where lb.RegistrationStatus == "Cancelled"
                                                         select lb).ToList();

                Int32[] listSR = new Int32[2];
                listSR[0] = listProses.Count();
                listSR[1] = listBatal.Count();

                JsonChartPieStatus.Value = JsonConvert.SerializeObject(listSR, Formatting.Indented);
        }


        private void penjaminBayar()
        {

            List<GetPatientInformation> lstData = BusinessLayer.GetPatientInformation(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());            
            List<GetPatientInformation> listPersonal = (from lp in lstData
                                                        where lp.BusinessPartnerName == "PERSONAL"
                                                        select lp).ToList();

            List<GetPatientInformation> listInstansi = (from lI in lstData
                                                        where lI.BusinessPartnerName != "PERSONAL"
                                                        select lI).ToList();

            Int32[] listBP = new Int32[2];
            listBP[0] = listPersonal.Count();
            listBP[1] = listInstansi.Count();

            JsonChartPiePenjamin.Value = JsonConvert.SerializeObject(listBP, Formatting.Indented);
        }

        private void dischargeCond()
        {

            List<GetPatientInformation> lstData = BusinessLayer.GetPatientInformation(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());            
            List<GetPatientInformation> listSehat = (from lp in lstData
                                                        where lp.DischargeCondition == "Sehat / Normal"
                                                        select lp).ToList();

            List<GetPatientInformation> listMembaik = (from lI in lstData
                                                       where lI.DischargeCondition == "Membaik"
                                                        select lI).ToList();
            List<GetPatientInformation> listSakit = (from lI in lstData
                                                     where lI.DischargeCondition == "Belum Sembuh"
                                                       select lI).ToList(); 

            Int32[] listDC = new Int32[3];
            listDC[0] = listSehat.Count();
            listDC[1] = listMembaik.Count();
            listDC[2] = listSakit.Count();

            JsonChartPieKondisiPasien.Value = JsonConvert.SerializeObject(listDC, Formatting.Indented);
        }

        private void KelasPasien()
        {

            List<GetPatientInformation> lstData = BusinessLayer.GetPatientInformation(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());            
            List<GetPatientInformation> listKelas1 = (from lp in lstData
                                                     where lp.ChargesClassName == "KELAS 1"
                                                     select lp).ToList();

            List<GetPatientInformation> listKelas2 = (from lI in lstData
                                                      where lI.ChargesClassName == "KELAS 2"
                                                       select lI).ToList();
            List<GetPatientInformation> listKelas3 = (from lI in lstData
                                                      where lI.ChargesClassName == "KELAS 3"
                                                     select lI).ToList();

            Int32[] listKP = new Int32[3];
            listKP[0] = listKelas1.Count();
            listKP[1] = listKelas2.Count();
            listKP[2] = listKelas3.Count();

            JsonChartPieKelasPasien.Value = JsonConvert.SerializeObject(listKP, Formatting.Indented);
        }

        private void DiscMtd()
        {
            List<GetPatientInformation> lstData = BusinessLayer.GetPatientInformation(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());            
            List<GetPatientInformation> resDisc = lstData.GroupBy(test => test.DischargeMethod).Select(grp => grp.First()).ToList();
            List<DataDischarge> lstDM = new List<DataDischarge>();
            foreach (GetPatientInformation e in resDisc)
            {
                DataDischarge entity = new DataDischarge();
                entity.DischargeMethod = e.DischargeMethod;
                entity.Jumlah = lstData.Where(t => t.DischargeMethod == e.DischargeMethod && t.DischargeMethod != "").Count();
                lstDM.Add(entity);
            }
            JsonChartDischarge.Value = JsonConvert.SerializeObject(lstDM, Formatting.Indented);
        }

        private void RegStatus()
        {
            List<GetPatientInformation> lstData = BusinessLayer.GetPatientInformation(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());
            List<GetPatientInformation> resReg2 = lstData.GroupBy(test => test.RegistrationStatus).Select(grp => grp.First()).ToList();
            List<StatusRegistrasi> lstReg = new List<StatusRegistrasi>();
            foreach (GetPatientInformation e in resReg2)
            {
                StatusRegistrasi entity = new StatusRegistrasi();
                entity.RegistrationStatus = e.RegistrationStatus;
                entity.Jumlah = lstData.Where(t => t.DepartmentID == e.DepartmentID).Count();
                lstReg.Add(entity);
            }
            JsonRegStatus.Value = JsonConvert.SerializeObject(lstReg, Formatting.Indented);
        }



        private void PMName()
        {
            List<GetPatientInformation> lstData = BusinessLayer.GetPatientInformation(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());
            List<GetPatientInformation> result = lstData.GroupBy(test => test.ParamedicName).Select(grp => grp.First()).ToList().OrderBy(x => x.ParamedicName).ToList();
            List<DataPerDokter> lstDok = new List<DataPerDokter>();
            foreach (GetPatientInformation e in result)
            {
                DataPerDokter entity = new DataPerDokter();
                entity.ParamedicName = e.ParamedicName;
                entity.Jumlah = lstData.Where(t => t.ParamedicName == e.ParamedicName && t.GCRegistrationStatus != Constant.VisitStatus.CANCELLED).Count();
                lstDok.Add(entity);
            }


            JsonDataPerDokter.Value = JsonConvert.SerializeObject(lstDok, Formatting.Indented);
        }

        private void Department()
        {
            List<GetPatientInformation> lstData = BusinessLayer.GetPatientInformation(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());
            List<GetPatientInformation> result = lstData.GroupBy(test => test.DepartmentID).Select(grp => grp.First()).ToList().OrderBy(x => x.DepartmentID).ToList();
            List<DataPerDepartmentID> lstData2 = new List<DataPerDepartmentID>();
            foreach (GetPatientInformation e in result)
            {
                DataPerDepartmentID entity = new DataPerDepartmentID();
                entity.DepartmentID = e.DepartmentID;
                entity.Jumlah = lstData.Where(t => t.DepartmentID == e.DepartmentID && t.GCRegistrationStatus != Constant.VisitStatus.CANCELLED).Count();
                lstData2.Add(entity);
            }


            JsonDataPerDepartment.Value = JsonConvert.SerializeObject(lstData2, Formatting.Indented);
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

        public class DataPerDepartmentID
        {
            public String DepartmentID { get; set; }
            public Int32 Jumlah { get; set; }
        }

        public class StatusRegistrasi
        {
            public String RegistrationStatus { get; set; }
            public Int32 Jumlah { get; set; }
        }

        public class DataPerDokter
        {
            public String ParamedicName { get; set; }
            public Int32 Jumlah { get; set; }
        }

        public class DataPerPartner
        {
            public String BusinessPartnerName { get; set; }
            public Int32 Jumlah { get; set; }
        }

        public class DataDischarge
        {
            public String DischargeMethod { get; set; }
            public Int32 Jumlah { get; set; }
        }
    }
}