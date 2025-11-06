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
    public partial class BPJSInformationCtl : BaseViewPopupCtl
    {
        private const string STATUS_IMAGE_PATH = "~/libs/Images/Dashboard/";
        List<ParamedicMaster> lstDoctor;
        List<ParamedicMaster> lstNurse;
        List<vHealthcareServiceUnit> lstClinic;
        List<vBed> lstBed;
        List<vRegistrationBPJS4> lstData;

        public override void InitializeDataControl(string param)
        {
            string filterExp = string.Format("RegistrationDate = '{0}'", DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT2));            
            lstData = BusinessLayer.GetvRegistrationBPJS4List(filterExp);

            string filterExpressionDoctor = string.Format("GCParamedicMasterType = '{0}' AND IsDeleted = 0", Constant.ParamedicType.Physician);
            string filterExpressionNurse = string.Format("GCParamedicMasterType = '{0}' AND IsDeleted = 0", Constant.ParamedicType.Nurse);
            string filterExpressionClinic = string.Format("DepartmentID = '{0}' AND IsDeleted = 0", Constant.Facility.OUTPATIENT);
            string filterExpressionBed = string.Format("IsDeleted = 0");
            lstDoctor = BusinessLayer.GetParamedicMasterList(filterExpressionDoctor);
            lstNurse = BusinessLayer.GetParamedicMasterList(filterExpressionNurse);
            lstClinic = BusinessLayer.GetvHealthcareServiceUnitList(filterExpressionClinic);
            lstBed = BusinessLayer.GetvBedList(filterExpressionBed);
            List<Department> lst = BusinessLayer.GetDepartmentList(string.Format("IsHasRegistration=1 and IsActive=1"));

            #region Patient
            var resultdoctor = lstDoctor.GroupBy(doctor => doctor.ParamedicID).Select(grp => grp.First()).ToList().OrderBy(x => x.ParamedicID);
            var resultnurse = lstNurse.GroupBy(nurse => nurse.ParamedicID).Select(grp => grp.First()).ToList().OrderBy(x => x.ParamedicID);
            var resultclinic = lstClinic.GroupBy(clinic => clinic.ServiceUnitID).Select(grp => grp.First()).ToList().OrderBy(x => x.ServiceUnitID);
            var resultbed = lstBed.GroupBy(bed => bed.BedID).Select(grp => grp.First()).ToList().OrderBy(x => x.BedID);
            #endregion
            NamaPoli();
            KelasTanggungan();
            JenisPeserta();
            PMName();
            NamaDiagnosa();
            Gender();
            Stacked1();
            Stacked2();
            ESPManual();
        }

        private void KelasTanggungan()
        {

            //List<GetPatientInformation> lstData = BusinessLayer.GetPatientInformation(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());
            List<vRegistrationBPJS4> listKelas1 = (from lp in lstData
                                                   where lp.NamaKelasTanggungan == "1 - KELAS I"
                                                    select lp).ToList();

            List<vRegistrationBPJS4> listKelas2 = (from lI in lstData
                                                   where lI.NamaKelasTanggungan == "2 - KELAS II"
                                                    select lI).ToList();
            List<vRegistrationBPJS4> listKelas3 = (from lI in lstData
                                                   where lI.NamaKelasTanggungan == "3 - KELAS III"
                                                    select lI).ToList();

            Int32[] listKT = new Int32[3];
            listKT[0] = listKelas1.Count();
            listKT[1] = listKelas2.Count();
            listKT[2] = listKelas3.Count();

            JsonChartPieKelasTanggungan.Value = JsonConvert.SerializeObject(listKT, Formatting.Indented);
        }

        private void Gender()
        {

            //List<GetPatientInformation> lstData = BusinessLayer.GetPatientInformation(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());
            List<vRegistrationBPJS4> listLL = (from lp in lstData
                                                   where lp.Gender == "Laki-Laki"
                                                   select lp).ToList();

            List<vRegistrationBPJS4> listP = (from lI in lstData
                                                   where lI.Gender == "Perempuan"
                                                   select lI).ToList();

            Int32[] listGender = new Int32[2];
            listGender[0] = listLL.Count();
            listGender[1] = listP.Count();

            JsonChartPieGender.Value = JsonConvert.SerializeObject(listGender, Formatting.Indented);
        }

        private void NamaPoli()
        {

            //List<GetPatientInformation> lstData = BusinessLayer.GetPatientInformation(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());

            List<vRegistrationBPJS4> listRes = lstData.GroupBy(test => test.NamaPoliklinik).Select(grp => grp.First()).ToList().OrderByDescending(x => x.NamaPoliklinik).Where(x => x.NamaPoliklinik.Length > 2).ToList();
            List<DataPerPoli> listPoli = new List<DataPerPoli>();

            foreach (vRegistrationBPJS4 e in listRes)
            {
                DataPerPoli entity = new DataPerPoli();
                entity.NamaPoliklinik = e.NamaPoliklinik;
                string str = entity.NamaPoliklinik;
                entity.NamaPoliklinik = String.Join(String.Empty, str.Skip(4));
                entity.Jumlah = lstData.Where(t => t.NamaPoliklinik == e.NamaPoliklinik && t.GCRegistrationStatus != Constant.VisitStatus.CANCELLED).Count();
                listPoli.Add(entity);
            }

            
            JsonChartBarPoli.Value = JsonConvert.SerializeObject(listPoli, Formatting.Indented);
        }

        private void JenisPeserta()
        {
            List<vRegistrationBPJS4> listRes = lstData.GroupBy(test => test.JenisPeserta).Select(grp => grp.First()).ToList().OrderByDescending(x => x.JenisPeserta).Where(x => x.JenisPeserta.Length > 1).ToList();
            List<DataJenisPeserta> listJP = new List<DataJenisPeserta>();

            foreach (vRegistrationBPJS4 e in listRes)
            {
                DataJenisPeserta entity = new DataJenisPeserta();
                entity.JenisPeserta = e.JenisPeserta;
                string str = entity.JenisPeserta;
                entity.Jumlah = lstData.Where(t => t.NamaPoliklinik == e.NamaPoliklinik && t.GCRegistrationStatus != Constant.VisitStatus.CANCELLED).Count();
                listJP.Add(entity);
            }

            JsonChartBarJenisPeserta.Value = JsonConvert.SerializeObject(listJP, Formatting.Indented);
        }

        private void Stacked1()
        {
            List<vRegistrationBPJS4> listRes = lstData.GroupBy(test => test.JenisPeserta).Select(grp => grp.First()).ToList().OrderByDescending(x => x.JenisPeserta).Where(x => x.JenisPeserta.Length > 1 && x.Gender == "Laki-Laki").ToList();
            List<DataJenisPeserta1> listS1 = new List<DataJenisPeserta1>();

            foreach (vRegistrationBPJS4 e in listRes)
            {
                DataJenisPeserta1 entity = new DataJenisPeserta1();
                entity.JenisPeserta = e.JenisPeserta;
                string str = entity.JenisPeserta;
                entity.Jumlah = lstData.Where(t => t.NamaPoliklinik == e.NamaPoliklinik && t.GCRegistrationStatus != Constant.VisitStatus.CANCELLED && t.Gender == "Laki-Laki").Count();
                listS1.Add(entity);
            }

            JsonChartBarStacked1.Value = JsonConvert.SerializeObject(listS1, Formatting.Indented);
        }

        private void Stacked2()
        {
            List<vRegistrationBPJS4> listRes = lstData.GroupBy(test => test.JenisPeserta).Select(grp => grp.First()).ToList().OrderByDescending(x => x.JenisPeserta).Where(x => x.JenisPeserta.Length > 1 && x.Gender == "Perempuan").ToList();
            List<DataJenisPeserta2> listS2 = new List<DataJenisPeserta2>();

            foreach (vRegistrationBPJS4 e in listRes)
            {
                DataJenisPeserta2 entity = new DataJenisPeserta2();
                entity.JenisPeserta = e.JenisPeserta;
                string str = entity.JenisPeserta;
                entity.Jumlah = lstData.Where(t => t.NamaPoliklinik == e.NamaPoliklinik && t.GCRegistrationStatus != Constant.VisitStatus.CANCELLED && t.Gender == "Perempuan").Count();
                listS2.Add(entity);
            }

            JsonChartBarStacked2.Value = JsonConvert.SerializeObject(listS2, Formatting.Indented);
        }

        private void NamaDiagnosa()
        {
            List<vRegistrationBPJS4> listRes = lstData.GroupBy(test => test.NamaDiagnosa).Select(grp => grp.First()).ToList().OrderByDescending(x => x.NamaDiagnosa).Where(x => x.NamaDiagnosa != null && x.NamaDiagnosa.Length > 1).ToList();
            List<DataNamaDiagnosa> listND = new List<DataNamaDiagnosa>();

            foreach (vRegistrationBPJS4 e in listRes)
            {
                DataNamaDiagnosa entity = new DataNamaDiagnosa();
                entity.NamaDiagnosa = e.NamaDiagnosa;
                entity.Jumlah = lstData.Where(t => t.NamaDiagnosa == e.NamaDiagnosa && t.NamaDiagnosa != null).Count();
                listND.Add(entity);
            }

            List<DataNamaDiagnosa> listSort = listND.GroupBy(test => test.NamaDiagnosa).Select(grp => grp.First()).ToList().OrderByDescending(x => x.Jumlah).Take(10).ToList();
            List<DataNamaDiagnosaSort> listNDSort = new List<DataNamaDiagnosaSort>();
            foreach (DataNamaDiagnosa e in listSort)
            {
                DataNamaDiagnosaSort entity = new DataNamaDiagnosaSort();
                entity.NamaDiagnosa = e.NamaDiagnosa;
                entity.Jumlah = lstData.Where(t => t.NamaDiagnosa == e.NamaDiagnosa).Count();
                
                listNDSort.Add(entity);
            }
            JsonChartBarNamaDiagnosa.Value = JsonConvert.SerializeObject(listNDSort, Formatting.Indented);
        }

        private void PMName()
        {
            //List<GetPatientInformation> lstData = BusinessLayer.GetPatientInformation(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());
            List<vRegistrationBPJS4> result = lstData.GroupBy(test => test.ParamedicName).Select(grp => grp.First()).ToList().OrderBy(x => x.ParamedicName).ToList();
            List<DataPerPM> lstPM = new List<DataPerPM>();
            foreach (vRegistrationBPJS4 e in result)
            {
                DataPerPM entity = new DataPerPM();
                entity.ParamedicName = e.ParamedicName;
                entity.Jumlah = lstData.Where(t => t.ParamedicName == e.ParamedicName).Count();
                lstPM.Add(entity);
            }

            List<DataPerPM> listSort = lstPM.GroupBy(test => test.ParamedicName).Select(grp => grp.First()).ToList().OrderByDescending(x => x.Jumlah).Take(10).ToList();
            List<DataNamaPMSort> listPMSort = new List<DataNamaPMSort>();
            foreach (DataPerPM e in listSort)
            {
                DataNamaPMSort entity = new DataNamaPMSort();
                entity.ParamedicName = e.ParamedicName;
                entity.Jumlah = lstData.Where(t => t.ParamedicName == e.ParamedicName).Count();

                listPMSort.Add(entity);
            }

            JsonDataPerPM.Value = JsonConvert.SerializeObject(listPMSort, Formatting.Indented);
        }

        private void ESPManual()
        {

            //List<GetPatientInformation> lstData = BusinessLayer.GetPatientInformation(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());
            List<vRegistrationBPJS4> listManual = (from lp in lstData
                                                   where lp.IsManualSEP == true
                                                   select lp).ToList();

            List<vRegistrationBPJS4> listNonManual = (from lI in lstData
                                                      where lI.IsManualSEP == false
                                                   select lI).ToList();


            Int32[] listESP = new Int32[2];
            listESP[0] = listManual.Count();
            listESP[1] = listNonManual.Count();

            JsonChartPieESP.Value = JsonConvert.SerializeObject(listESP, Formatting.Indented);
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }


        public class DataPerPoli
        {
            public String NamaPoliklinik { get; set; }
            public Int32 Jumlah { get; set; }
        }

        public class DataJenisPeserta
        {
            public String JenisPeserta { get; set; }
            public Int32 Jumlah { get; set; }
        }

        public class DataJenisPeserta1
        {
            public String JenisPeserta { get; set; }
            public Int32 Jumlah { get; set; }
        }

        public class DataJenisPeserta2
        {
            public String JenisPeserta { get; set; }
            public Int32 Jumlah { get; set; }
        }

        public class DataPerPM 
        {
            public String ParamedicName { get; set; }
            public Int32 Jumlah { get; set; }
        }

        public class DataNamaDiagnosa 
        {
            public String NamaDiagnosa { get; set; }
            public Int32 Jumlah { get; set; }
        }

        public class DataNamaDiagnosaSort
        {
            public String NamaDiagnosa { get; set; }
            public Int32 Jumlah { get; set; }
        }

        public class DataNamaPMSort
        {
            public String ParamedicName { get; set; }
            public Int32 Jumlah { get; set; }
        }
    }
}