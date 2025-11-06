using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Program;
using QIS.Medinfras.Web.CommonLibs.Service;
using DevExpress.Web.ASPxCallbackPanel;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.MedicalCheckup.Program
{
    public partial class MCUResultFormEntry : BasePagePatientPageList
    {
        protected int PageCount = 1;

        private   int sizePopupWidth = 1600, sizepopupHeight = 700;

        protected string GetPageTitle()
        {
            string filterMenu = string.Format("MenuCode = '{0}'", OnGetMenuCode());
            MenuMaster menu = BusinessLayer.GetMenuMasterList(filterMenu).FirstOrDefault();
            hdnPageTitle.Value = menu.MenuCaption;

            return hdnPageTitle.Value;
        }

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalCheckup.MCU_RESULT_FORM;
        }

        protected override void InitializeDataControl()
        {
            if (AppSession.LastPatientVisitMCUForm != null)
            {
                ctlToolbar.SetSelectedMenu(1); 
                string param = AppSession.LastPatientVisitMCUForm.VisitID.ToString(); 
                hdnVisitID.Value = param;
                hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
                hdnHealthcareServiceUnitID.Value = AppSession.RegisteredPatient.HealthcareServiceUnitID.ToString();
                hdnRegistrationNo.Value = AppSession.RegisteredPatient.RegistrationNo;
                hdnMRN.Value = AppSession.RegisteredPatient.MRN.ToString();
                hdnGCGender.Value = AppSession.RegisteredPatient.GCGender;

                BindGridView(1, false, ref PageCount);
            }
            //if (Page.Request.QueryString.Count > 0)
            //{
            //    string param = Page.Request.QueryString["id"];
            //    hdnVisitID.Value = param;
            //    hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            //    hdnHealthcareServiceUnitID.Value = AppSession.RegisteredPatient.HealthcareServiceUnitID.ToString();
            //    hdnRegistrationNo.Value = AppSession.RegisteredPatient.RegistrationNo;
            //    hdnMRN.Value = AppSession.RegisteredPatient.MRN.ToString();
            //    hdnGCGender.Value = AppSession.RegisteredPatient.GCGender;

            //    BindGridView(1, false, ref PageCount);
            //}
        }
        
        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("ParentID = '{0}' AND IsDeleted=0 AND IsActive = 1 ORDER BY StandardCodeID", Constant.StandardCode.MCU_RESULT_TYPE);
            List<StandardCode> lstEntity = BusinessLayer.GetStandardCodeList(filterExpression);
            grdFormList.DataSource = lstEntity;
            grdFormList.DataBind();
        }

        private void BindGridViewDt(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID IN ({0}) AND GCResultType = '{1}' AND IsDeleted = 0", hdnVisitID.Value, hdnGCResultType.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvMCUResultFormRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vMCUResultForm> lstEntity = BusinessLayer.GetvMCUResultFormList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpFormList_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
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

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else if (param[0] == "printout")
                {
                    BindPrintout();
                    result = "printout";
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

        protected void PrintToExcelButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hdnID.Value) && hdnID.Value != "0")
            {
                List<vMCUResultForm> lstMCUResultForm = BusinessLayer.GetvMCUResultFormList(string.Format("RegistrationID = '{0}' AND IsDeleted = 0 ORDER BY ID ASC", AppSession.RegisteredPatient.RegistrationID));
                if (lstMCUResultForm.Count > 0)
                {
                    string lstJson = string.Empty;
                    string jsonTemp = string.Empty;
                    foreach (vMCUResultForm row in lstMCUResultForm)
                    {

                        string[] qnaArr = row.FormResult.Split('|'); //Q=A
                        int count = 0;
                        Dictionary<String, dynamic> obj = new Dictionary<String, dynamic>();
                        for (int i = 0; i < qnaArr.Length; i++)
                        {
                            count += 1;
                            string[] qnaDtArr = qnaArr[i].Split('=');
                            if (qnaArr.Length > 0)
                            {
                                string question = qnaDtArr[0];
                                string answer = qnaDtArr[1];
                                obj[question] = answer;
                            }
                        }
                        jsonTemp += JsonConvert.SerializeObject(obj);
                    }
                    lstJson = ("[" + jsonTemp + "]").Replace(" ", "_").Trim();
                    dynamic jsonResponse = JsonConvert.DeserializeObject(lstJson);
                    ///string str = "Jumlah Rokok(Batang/hari)=0|Alkohol(kali/Minggu)=0|Minum Kopi (Gelas/hari)=0|Olahraga(Jam/Satu Minggu)=0|Klg yg Hypertensi=|Klg yg Diabetes=|Klg yg Sakit Jantung=|Klg yg Sakit Ginjal=|Klg yg Sakit Ganguan Jiwa=|Klg yg Sakit Lain2/ALergi=|Sudah Berapa Lama=|Sudah Berapa Lama=|Tinggi Badan=|Berat Badan=|Anjuran Berat Badan=|Body Mass Index=|keterangan Abnormal=|Visus + Koreksi Kanan=|Visus + Koreksi Kiri=|Add Visus Kanan=|Add Visus Kiri=|Lain-lain=|Gigi Atas - Keterangan=|Gigi Bawah - Keterangan=|Keterangan Tonsil=|Nilai Pernapasan=|Tekanan Darah (mm/Hg)=|Frequensi Nadi=|Irama Kardiovaskular=|Ket Abnormal Tengkorak=|Ket Abnormal Tengkorak=|Ket Abnormal Anggota Gerak Atas=|Ket Abnormal Anggota Gerak Atas=|Nama Perusahaan 1=0|Nama Perusahaan 2=0|Jenis Pekerjaan 1=0|Jenis Pekerjaan 2=0|Faktor Fisika 1=0|Faktor Fisika 2=0|Faktor Kimia 1=0|Faktor Kimia 1=0|Faktor Biologi 1=0|Faktor Biologi 2=0|Faktor Psikologi 1=0|Faktor Psikologi 2=0|Faktor Ergonomi 1=0|Faktor Ergonomi 2=0|Lama tahun bekerja 1=0|Lama Bulan bekerja 1=0|Lama tahun bekerja 2=0|Lama bulan bekerja 2=0|Penyakit apa dan sejak kapan=|Penyakit apa dan sejak kapan=|Operasi Apa dan kapan=|Kecelakan Apa dan Kapan=|Jika Ya, Penyakit Apa=|Jika Ya, Penyakit Apa=|Pengobatan Apa Dan Apakah Terkontrol=|AutoRef Kanan=|AutoRef Kiri=|NCT / Tonometri Kanan=|NCT / Tonometri Kiri=|Kacamata Kanan=|Kacamata Kanan=|Anamnesa=|Pemeriksaan Fisik=|Radiologi Abnormal=|Elektro Kardiografi Description=|Audiometri=|Laboratorium=|Lain-lain=|Saran=|Saran=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Kesimpulan=|Penyakit Diabetes Keterangan=|Spirometri=|Pap Smear=|Penyakit Yang pernah diderita=Tidak|Berapa lama=-|Pernah Kecelakaan=Tidak|Kecelakaan dimana=Tidak Ada|Merokok=Tidak|Alkohol=Tidak|Diabetes=Tidak|Apakah Sedang Menderita Penyakit=Tidak|Apakah Sedang Menjalani Pengobatan=Tidak|Status Gizi=UnderWeight|Kulit=Normal|Rambut=Normal|Visus Kanan=Abnormal|Visus Kiri=Abnormal|Konjungtiva=Tidak Hiperemis|Skelera=Normal|Pupil=Isokor|Buta Warna=Total|Bola Mata=Simetris|Cornea=Keruh|Hidung=Normal|Lidah=Normal|Gigi Atas=Baik|Gigi Bawah=Baik|Pharing=Normal|Pharing Abnormal=-|Tonsil=Tidak Hypertropi|Tiroid=Normal|Tiroid Abnormal=-|Frekuensi Pernapasan=Normal|Paru paru=Normal|Vesikuer=Normal|Telinga Kanan - Telinga Luar=Serumen Propt (-)|Telinga Luar - Membran Tympani=Utuh|Telinga Luar - Membran Timpany tidak Utuh=-|Telinga Kiri - Telinga Luar=Serumen Propt (-)|Telinga Kiri - Membran Tympani=Utuh|Telinga Kiri - Membran Timpany tidak Utuh=-|Irama=Reguler|Hasil Tensi=Normal|Iktrus Kordis=Tidak Teraba|Auskultasi=BJ Murni|Kesan Batas Jantung=Melebar|Inspeksi=Normal|Nyeri Tekan=Ada|Nyeri Lepas=Ada|Hati=Tidak Teraba|Limpa=Tidak Teraba|Hernia=Tidak|Rectal Touche=Tidak Dilakukan|=-|Ginjal=Normal|Ballotement=Normal|Nyeri Ketok Kanan=Negatif|Nyeri Ketok Kiri=Negatif|Genital=1|Refleks Fisiologis=Positif|Refleks Patologis=Negatif|Fungsi Motorik=Normal|Fungsi Sensorik=Normal|Tonus Otot=Eutoni|Ket Abnormal Tonus Otot=Ischialgia|Tulang Belakang=Normal|Anggota Gerak Atas=Normal|Anggota Gerak Atas=Normal|Leher=Tidak Membesar|Axila=Tidak Membesar|Inguinal=Tidak Membesar|Radiologi=Tidak Dilakukan|EKG=Tidak Dilakukan|Penyakit akibat kerja=Tidak|Hasil Hipertensi=Tidak|Hasil Hipertensi Derajat=-|Jumlah Rokok(Batang/hari)=0|Alkohol(kali/Minggu)=0|Minum Kopi (Gelas/hari)=0|Olahraga(Jam/Satu Minggu)=0|Klg yg Hypertensi=|Klg yg Diabetes=|Klg yg Sakit Jantung=|Klg yg Sakit Ginjal=|Klg yg Sakit Ganguan Jiwa=|Klg yg Sakit Lain2/ALergi=|Sudah Berapa Lama=|Sudah Berapa Lama=|Tinggi Badan=|Berat Badan=|Anjuran Berat Badan=|Body Mass Index=|keterangan Abnormal=|Visus + Koreksi Kanan=|Visus + Koreksi Kiri=|Add Visus Kanan=|Add Visus Kiri=|Lain-lain=|Gigi Atas - Keterangan=|Gigi Bawah - Keterangan=|Keterangan Tonsil=|Nilai Pernapasan=|Tekanan Darah (mm/Hg)=|Frequensi Nadi=|Irama Kardiovaskular=|Ket Abnormal Tengkorak=|Ket Abnormal Tengkorak=|Ket Abnormal Anggota Gerak Atas=|Ket Abnormal Anggota Gerak Atas=|Nama Perusahaan 1=0|Nama Perusahaan 2=0|Jenis Pekerjaan 1=0|Jenis Pekerjaan 2=0|Faktor Fisika 1=0|Faktor Fisika 2=0|Faktor Kimia 1=0|Faktor Kimia 1=0|Faktor Biologi 1=0|Faktor Biologi 2=0|Faktor Psikologi 1=0|Faktor Psikologi 2=0|Faktor Ergonomi 1=0|Faktor Ergonomi 2=0|Lama tahun bekerja 1=0|Lama Bulan bekerja 1=0|Lama tahun bekerja 2=0|Lama bulan bekerja 2=0|Penyakit apa dan sejak kapan=|Penyakit apa dan sejak kapan=|Operasi Apa dan kapan=|Kecelakan Apa dan Kapan=|Jika Ya, Penyakit Apa=|Jika Ya, Penyakit Apa=|Pengobatan Apa Dan Apakah Terkontrol=|AutoRef Kanan=|AutoRef Kiri=|NCT / Tonometri Kanan=|NCT / Tonometri Kiri=|Kacamata Kanan=|Kacamata Kanan=|Anamnesa=|Pemeriksaan Fisik=|Radiologi Abnormal=|Elektro Kardiografi Description=|Audiometri=|Laboratorium=|Lain-lain=|Saran=|Saran=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Kesimpulan=|Penyakit Diabetes Keterangan=|Spirometri=|Pap Smear=|Jumlah Rokok(Batang/hari)=0|Alkohol(kali/Minggu)=0|Minum Kopi (Gelas/hari)=0|Olahraga(Jam/Satu Minggu)=0|Klg yg Hypertensi=|Klg yg Diabetes=|Klg yg Sakit Jantung=|Klg yg Sakit Ginjal=|Klg yg Sakit Ganguan Jiwa=|Klg yg Sakit Lain2/ALergi=|Sudah Berapa Lama=|Sudah Berapa Lama=|Tinggi Badan=|Berat Badan=|Anjuran Berat Badan=|Body Mass Index=|keterangan Abnormal=|Visus + Koreksi Kanan=|Visus + Koreksi Kiri=|Add Visus Kanan=|Add Visus Kiri=|Lain-lain=|Gigi Atas - Keterangan=|Gigi Bawah - Keterangan=|Keterangan Tonsil=|Nilai Pernapasan=|Tekanan Darah (mm/Hg)=|Frequensi Nadi=|Irama Kardiovaskular=|Ket Abnormal Tengkorak=|Ket Abnormal Tengkorak=|Ket Abnormal Anggota Gerak Atas=|Ket Abnormal Anggota Gerak Atas=|Nama Perusahaan 1=0|Nama Perusahaan 2=0|Jenis Pekerjaan 1=0|Jenis Pekerjaan 2=0|Faktor Fisika 1=0|Faktor Fisika 2=0|Faktor Kimia 1=0|Faktor Kimia 1=0|Faktor Biologi 1=0|Faktor Biologi 2=0|Faktor Psikologi 1=0|Faktor Psikologi 2=0|Faktor Ergonomi 1=0|Faktor Ergonomi 2=0|Lama tahun bekerja 1=0|Lama Bulan bekerja 1=0|Lama tahun bekerja 2=0|Lama bulan bekerja 2=0|Penyakit apa dan sejak kapan=|Penyakit apa dan sejak kapan=|Operasi Apa dan kapan=|Kecelakan Apa dan Kapan=|Jika Ya, Penyakit Apa=|Jika Ya, Penyakit Apa=|Pengobatan Apa Dan Apakah Terkontrol=|AutoRef Kanan=|AutoRef Kiri=|NCT / Tonometri Kanan=|NCT / Tonometri Kiri=|Kacamata Kanan=|Kacamata Kanan=|Anamnesa=|Pemeriksaan Fisik=|Radiologi Abnormal=|Elektro Kardiografi Description=|Audiometri=|Laboratorium=|Lain-lain=|Saran=|Saran=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Kesimpulan=|Penyakit Diabetes Keterangan=|Spirometri=|Pap Smear=|Jumlah Rokok(Batang/hari)=0|Alkohol(kali/Minggu)=0|Minum Kopi (Gelas/hari)=0|Olahraga(Jam/Satu Minggu)=0|Klg yg Hypertensi=|Klg yg Diabetes=|Klg yg Sakit Jantung=|Klg yg Sakit Ginjal=|Klg yg Sakit Ganguan Jiwa=|Klg yg Sakit Lain2/ALergi=|Sudah Berapa Lama=|Sudah Berapa Lama=|Tinggi Badan=|Berat Badan=|Anjuran Berat Badan=|Body Mass Index=|keterangan Abnormal=|Visus + Koreksi Kanan=|Visus + Koreksi Kiri=|Add Visus Kanan=|Add Visus Kiri=|Lain-lain=|Gigi Atas - Keterangan=|Gigi Bawah - Keterangan=|Keterangan Tonsil=|Nilai Pernapasan=|Tekanan Darah (mm/Hg)=|Frequensi Nadi=|Irama Kardiovaskular=|Ket Abnormal Tengkorak=|Ket Abnormal Tengkorak=|Ket Abnormal Anggota Gerak Atas=|Ket Abnormal Anggota Gerak Atas=|Nama Perusahaan 1=0|Nama Perusahaan 2=0|Jenis Pekerjaan 1=0|Jenis Pekerjaan 2=0|Faktor Fisika 1=0|Faktor Fisika 2=0|Faktor Kimia 1=0|Faktor Kimia 1=0|Faktor Biologi 1=0|Faktor Biologi 2=0|Faktor Psikologi 1=0|Faktor Psikologi 2=0|Faktor Ergonomi 1=0|Faktor Ergonomi 2=0|Lama tahun bekerja 1=0|Lama Bulan bekerja 1=0|Lama tahun bekerja 2=0|Lama bulan bekerja 2=0|Penyakit apa dan sejak kapan=|Penyakit apa dan sejak kapan=|Operasi Apa dan kapan=|Kecelakan Apa dan Kapan=|Jika Ya, Penyakit Apa=|Jika Ya, Penyakit Apa=|Pengobatan Apa Dan Apakah Terkontrol=|AutoRef Kanan=|AutoRef Kiri=|NCT / Tonometri Kanan=|NCT / Tonometri Kiri=|Kacamata Kanan=|Kacamata Kanan=|Anamnesa=|Pemeriksaan Fisik=|Radiologi Abnormal=|Elektro Kardiografi Description=|Audiometri=|Laboratorium=|Lain-lain=|Saran=|Saran=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Kesimpulan=|Penyakit Diabetes Keterangan=|Spirometri=|Pap Smear=|Jumlah Rokok(Batang/hari)=0|Alkohol(kali/Minggu)=0|Minum Kopi (Gelas/hari)=0|Olahraga(Jam/Satu Minggu)=0|Klg yg Hypertensi=|Klg yg Diabetes=|Klg yg Sakit Jantung=|Klg yg Sakit Ginjal=|Klg yg Sakit Ganguan Jiwa=|Klg yg Sakit Lain2/ALergi=|Sudah Berapa Lama=|Sudah Berapa Lama=|Tinggi Badan=|Berat Badan=|Anjuran Berat Badan=|Body Mass Index=|keterangan Abnormal=|Visus + Koreksi Kanan=|Visus + Koreksi Kiri=|Add Visus Kanan=|Add Visus Kiri=|Lain-lain=|Gigi Atas - Keterangan=|Gigi Bawah - Keterangan=|Keterangan Tonsil=|Nilai Pernapasan=|Tekanan Darah (mm/Hg)=|Frequensi Nadi=|Irama Kardiovaskular=|Ket Abnormal Tengkorak=|Ket Abnormal Tengkorak=|Ket Abnormal Anggota Gerak Atas=|Ket Abnormal Anggota Gerak Atas=|Nama Perusahaan 1=0|Nama Perusahaan 2=0|Jenis Pekerjaan 1=0|Jenis Pekerjaan 2=0|Faktor Fisika 1=0|Faktor Fisika 2=0|Faktor Kimia 1=0|Faktor Kimia 1=0|Faktor Biologi 1=0|Faktor Biologi 2=0|Faktor Psikologi 1=0|Faktor Psikologi 2=0|Faktor Ergonomi 1=0|Faktor Ergonomi 2=0|Lama tahun bekerja 1=0|Lama Bulan bekerja 1=0|Lama tahun bekerja 2=0|Lama bulan bekerja 2=0|Penyakit apa dan sejak kapan=|Penyakit apa dan sejak kapan=|Operasi Apa dan kapan=|Kecelakan Apa dan Kapan=|Jika Ya, Penyakit Apa=|Jika Ya, Penyakit Apa=|Pengobatan Apa Dan Apakah Terkontrol=|AutoRef Kanan=|AutoRef Kiri=|NCT / Tonometri Kanan=|NCT / Tonometri Kiri=|Kacamata Kanan=|Kacamata Kanan=|Anamnesa=|Pemeriksaan Fisik=|Radiologi Abnormal=|Elektro Kardiografi Description=|Audiometri=|Laboratorium=|Lain-lain=|Saran=|Saran=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Kesimpulan=|Penyakit Diabetes Keterangan=|Spirometri=|Pap Smear=|Jumlah Rokok(Batang/hari)=0|Alkohol(kali/Minggu)=0|Minum Kopi (Gelas/hari)=0|Olahraga(Jam/Satu Minggu)=0|Klg yg Hypertensi=|Klg yg Diabetes=|Klg yg Sakit Jantung=|Klg yg Sakit Ginjal=|Klg yg Sakit Ganguan Jiwa=|Klg yg Sakit Lain2/ALergi=|Sudah Berapa Lama=|Sudah Berapa Lama=|Tinggi Badan=|Berat Badan=|Anjuran Berat Badan=|Body Mass Index=|keterangan Abnormal=|Visus + Koreksi Kanan=|Visus + Koreksi Kiri=|Add Visus Kanan=|Add Visus Kiri=|Lain-lain=|Gigi Atas - Keterangan=|Gigi Bawah - Keterangan=|Keterangan Tonsil=|Nilai Pernapasan=|Tekanan Darah (mm/Hg)=|Frequensi Nadi=|Irama Kardiovaskular=|Ket Abnormal Tengkorak=|Ket Abnormal Tengkorak=|Ket Abnormal Anggota Gerak Atas=|Ket Abnormal Anggota Gerak Atas=|Nama Perusahaan 1=0|Nama Perusahaan 2=0|Jenis Pekerjaan 1=0|Jenis Pekerjaan 2=0|Faktor Fisika 1=0|Faktor Fisika 2=0|Faktor Kimia 1=0|Faktor Kimia 1=0|Faktor Biologi 1=0|Faktor Biologi 2=0|Faktor Psikologi 1=0|Faktor Psikologi 2=0|Faktor Ergonomi 1=0|Faktor Ergonomi 2=0|Lama tahun bekerja 1=0|Lama Bulan bekerja 1=0|Lama tahun bekerja 2=0|Lama bulan bekerja 2=0|Penyakit apa dan sejak kapan=|Penyakit apa dan sejak kapan=|Operasi Apa dan kapan=|Kecelakan Apa dan Kapan=|Jika Ya, Penyakit Apa=|Jika Ya, Penyakit Apa=|Pengobatan Apa Dan Apakah Terkontrol=|AutoRef Kanan=|AutoRef Kiri=|NCT / Tonometri Kanan=|NCT / Tonometri Kiri=|Kacamata Kanan=|Kacamata Kanan=|Anamnesa=|Pemeriksaan Fisik=|Radiologi Abnormal=|Elektro Kardiografi Description=|Audiometri=|Laboratorium=|Lain-lain=|Saran=|Saran=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Kesimpulan=|Penyakit Diabetes Keterangan=|Spirometri=|Pap Smear=|Jumlah Rokok(Batang/hari)=0|Alkohol(kali/Minggu)=0|Minum Kopi (Gelas/hari)=0|Olahraga(Jam/Satu Minggu)=0|Klg yg Hypertensi=|Klg yg Diabetes=|Klg yg Sakit Jantung=|Klg yg Sakit Ginjal=|Klg yg Sakit Ganguan Jiwa=|Klg yg Sakit Lain2/ALergi=|Sudah Berapa Lama=|Sudah Berapa Lama=|Tinggi Badan=|Berat Badan=|Anjuran Berat Badan=|Body Mass Index=|keterangan Abnormal=|Visus + Koreksi Kanan=|Visus + Koreksi Kiri=|Add Visus Kanan=|Add Visus Kiri=|Lain-lain=|Gigi Atas - Keterangan=|Gigi Bawah - Keterangan=|Keterangan Tonsil=|Nilai Pernapasan=|Tekanan Darah (mm/Hg)=|Frequensi Nadi=|Irama Kardiovaskular=|Ket Abnormal Tengkorak=|Ket Abnormal Tengkorak=|Ket Abnormal Anggota Gerak Atas=|Ket Abnormal Anggota Gerak Atas=|Nama Perusahaan 1=0|Nama Perusahaan 2=0|Jenis Pekerjaan 1=0|Jenis Pekerjaan 2=0|Faktor Fisika 1=0|Faktor Fisika 2=0|Faktor Kimia 1=0|Faktor Kimia 1=0|Faktor Biologi 1=0|Faktor Biologi 2=0|Faktor Psikologi 1=0|Faktor Psikologi 2=0|Faktor Ergonomi 1=0|Faktor Ergonomi 2=0|Lama tahun bekerja 1=0|Lama Bulan bekerja 1=0|Lama tahun bekerja 2=0|Lama bulan bekerja 2=0|Penyakit apa dan sejak kapan=|Penyakit apa dan sejak kapan=|Operasi Apa dan kapan=|Kecelakan Apa dan Kapan=|Jika Ya, Penyakit Apa=|Jika Ya, Penyakit Apa=|Pengobatan Apa Dan Apakah Terkontrol=|AutoRef Kanan=|AutoRef Kiri=|NCT / Tonometri Kanan=|NCT / Tonometri Kiri=|Kacamata Kanan=|Kacamata Kanan=|Anamnesa=|Pemeriksaan Fisik=|Radiologi Abnormal=|Elektro Kardiografi Description=|Audiometri=|Laboratorium=|Lain-lain=|Saran=|Saran=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Kesimpulan=|Penyakit Diabetes Keterangan=|Spirometri=|Pap Smear=|Jumlah Rokok(Batang/hari)=0|Alkohol(kali/Minggu)=0|Minum Kopi (Gelas/hari)=0|Olahraga(Jam/Satu Minggu)=0|Klg yg Hypertensi=|Klg yg Diabetes=|Klg yg Sakit Jantung=|Klg yg Sakit Ginjal=|Klg yg Sakit Ganguan Jiwa=|Klg yg Sakit Lain2/ALergi=|Sudah Berapa Lama=|Sudah Berapa Lama=|Tinggi Badan=|Berat Badan=|Anjuran Berat Badan=|Body Mass Index=|keterangan Abnormal=|Visus + Koreksi Kanan=|Visus + Koreksi Kiri=|Add Visus Kanan=|Add Visus Kiri=|Lain-lain=|Gigi Atas - Keterangan=|Gigi Bawah - Keterangan=|Keterangan Tonsil=|Nilai Pernapasan=|Tekanan Darah (mm/Hg)=|Frequensi Nadi=|Irama Kardiovaskular=|Ket Abnormal Tengkorak=|Ket Abnormal Tengkorak=|Ket Abnormal Anggota Gerak Atas=|Ket Abnormal Anggota Gerak Atas=|Nama Perusahaan 1=0|Nama Perusahaan 2=0|Jenis Pekerjaan 1=0|Jenis Pekerjaan 2=0|Faktor Fisika 1=0|Faktor Fisika 2=0|Faktor Kimia 1=0|Faktor Kimia 1=0|Faktor Biologi 1=0|Faktor Biologi 2=0|Faktor Psikologi 1=0|Faktor Psikologi 2=0|Faktor Ergonomi 1=0|Faktor Ergonomi 2=0|Lama tahun bekerja 1=0|Lama Bulan bekerja 1=0|Lama tahun bekerja 2=0|Lama bulan bekerja 2=0|Penyakit apa dan sejak kapan=|Penyakit apa dan sejak kapan=|Operasi Apa dan kapan=|Kecelakan Apa dan Kapan=|Jika Ya, Penyakit Apa=|Jika Ya, Penyakit Apa=|Pengobatan Apa Dan Apakah Terkontrol=|AutoRef Kanan=|AutoRef Kiri=|NCT / Tonometri Kanan=|NCT / Tonometri Kiri=|Kacamata Kanan=|Kacamata Kanan=|Anamnesa=|Pemeriksaan Fisik=|Radiologi Abnormal=|Elektro Kardiografi Description=|Audiometri=|Laboratorium=|Lain-lain=|Saran=|Saran=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Kesimpulan=|Penyakit Diabetes Keterangan=|Spirometri=|Pap Smear=|Jumlah Rokok(Batang/hari)=0|Alkohol(kali/Minggu)=0|Minum Kopi (Gelas/hari)=0|Olahraga(Jam/Satu Minggu)=0|Klg yg Hypertensi=|Klg yg Diabetes=|Klg yg Sakit Jantung=|Klg yg Sakit Ginjal=|Klg yg Sakit Ganguan Jiwa=|Klg yg Sakit Lain2/ALergi=|Sudah Berapa Lama=|Sudah Berapa Lama=|Tinggi Badan=|Berat Badan=|Anjuran Berat Badan=|Body Mass Index=|keterangan Abnormal=|Visus + Koreksi Kanan=|Visus + Koreksi Kiri=|Add Visus Kanan=|Add Visus Kiri=|Lain-lain=|Gigi Atas - Keterangan=|Gigi Bawah - Keterangan=|Keterangan Tonsil=|Nilai Pernapasan=|Tekanan Darah (mm/Hg)=|Frequensi Nadi=|Irama Kardiovaskular=|Ket Abnormal Tengkorak=|Ket Abnormal Tengkorak=|Ket Abnormal Anggota Gerak Atas=|Ket Abnormal Anggota Gerak Atas=|Nama Perusahaan 1=0|Nama Perusahaan 2=0|Jenis Pekerjaan 1=0|Jenis Pekerjaan 2=0|Faktor Fisika 1=0|Faktor Fisika 2=0|Faktor Kimia 1=0|Faktor Kimia 1=0|Faktor Biologi 1=0|Faktor Biologi 2=0|Faktor Psikologi 1=0|Faktor Psikologi 2=0|Faktor Ergonomi 1=0|Faktor Ergonomi 2=0|Lama tahun bekerja 1=0|Lama Bulan bekerja 1=0|Lama tahun bekerja 2=0|Lama bulan bekerja 2=0|Penyakit apa dan sejak kapan=|Penyakit apa dan sejak kapan=|Operasi Apa dan kapan=|Kecelakan Apa dan Kapan=|Jika Ya, Penyakit Apa=|Jika Ya, Penyakit Apa=|Pengobatan Apa Dan Apakah Terkontrol=|AutoRef Kanan=|AutoRef Kiri=|NCT / Tonometri Kanan=|NCT / Tonometri Kiri=|Kacamata Kanan=|Kacamata Kanan=|Anamnesa=|Pemeriksaan Fisik=|Radiologi Abnormal=|Elektro Kardiografi Description=|Audiometri=|Laboratorium=|Lain-lain=|Saran=|Saran=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Kesimpulan=|Penyakit Diabetes Keterangan=|Spirometri=|Pap Smear=|Jumlah Rokok(Batang/hari)=0|Alkohol(kali/Minggu)=0|Minum Kopi (Gelas/hari)=0|Olahraga(Jam/Satu Minggu)=0|Klg yg Hypertensi=|Klg yg Diabetes=|Klg yg Sakit Jantung=|Klg yg Sakit Ginjal=|Klg yg Sakit Ganguan Jiwa=|Klg yg Sakit Lain2/ALergi=|Sudah Berapa Lama=|Sudah Berapa Lama=|Tinggi Badan=|Berat Badan=|Anjuran Berat Badan=|Body Mass Index=|keterangan Abnormal=|Visus + Koreksi Kanan=|Visus + Koreksi Kiri=|Add Visus Kanan=|Add Visus Kiri=|Lain-lain=|Gigi Atas - Keterangan=|Gigi Bawah - Keterangan=|Keterangan Tonsil=|Nilai Pernapasan=|Tekanan Darah (mm/Hg)=|Frequensi Nadi=|Irama Kardiovaskular=|Ket Abnormal Tengkorak=|Ket Abnormal Tengkorak=|Ket Abnormal Anggota Gerak Atas=|Ket Abnormal Anggota Gerak Atas=|Nama Perusahaan 1=0|Nama Perusahaan 2=0|Jenis Pekerjaan 1=0|Jenis Pekerjaan 2=0|Faktor Fisika 1=0|Faktor Fisika 2=0|Faktor Kimia 1=0|Faktor Kimia 1=0|Faktor Biologi 1=0|Faktor Biologi 2=0|Faktor Psikologi 1=0|Faktor Psikologi 2=0|Faktor Ergonomi 1=0|Faktor Ergonomi 2=0|Lama tahun bekerja 1=0|Lama Bulan bekerja 1=0|Lama tahun bekerja 2=0|Lama bulan bekerja 2=0|Penyakit apa dan sejak kapan=|Penyakit apa dan sejak kapan=|Operasi Apa dan kapan=|Kecelakan Apa dan Kapan=|Jika Ya, Penyakit Apa=|Jika Ya, Penyakit Apa=|Pengobatan Apa Dan Apakah Terkontrol=|AutoRef Kanan=|AutoRef Kiri=|NCT / Tonometri Kanan=|NCT / Tonometri Kiri=|Kacamata Kanan=|Kacamata Kanan=|Anamnesa=|Pemeriksaan Fisik=|Radiologi Abnormal=|Elektro Kardiografi Description=|Audiometri=|Laboratorium=|Lain-lain=|Saran=|Saran=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Kesimpulan=|Penyakit Diabetes Keterangan=|Spirometri=|Pap Smear=|Jumlah Rokok(Batang/hari)=0|Alkohol(kali/Minggu)=0|Minum Kopi (Gelas/hari)=0|Olahraga(Jam/Satu Minggu)=0|Klg yg Hypertensi=|Klg yg Diabetes=|Klg yg Sakit Jantung=|Klg yg Sakit Ginjal=|Klg yg Sakit Ganguan Jiwa=|Klg yg Sakit Lain2/ALergi=|Sudah Berapa Lama=|Sudah Berapa Lama=|Tinggi Badan=|Berat Badan=|Anjuran Berat Badan=|Body Mass Index=|keterangan Abnormal=|Visus + Koreksi Kanan=|Visus + Koreksi Kiri=|Add Visus Kanan=|Add Visus Kiri=|Lain-lain=|Gigi Atas - Keterangan=|Gigi Bawah - Keterangan=|Keterangan Tonsil=|Nilai Pernapasan=|Tekanan Darah (mm/Hg)=|Frequensi Nadi=|Irama Kardiovaskular=|Ket Abnormal Tengkorak=|Ket Abnormal Tengkorak=|Ket Abnormal Anggota Gerak Atas=|Ket Abnormal Anggota Gerak Atas=|Nama Perusahaan 1=0|Nama Perusahaan 2=0|Jenis Pekerjaan 1=0|Jenis Pekerjaan 2=0|Faktor Fisika 1=0|Faktor Fisika 2=0|Faktor Kimia 1=0|Faktor Kimia 1=0|Faktor Biologi 1=0|Faktor Biologi 2=0|Faktor Psikologi 1=0|Faktor Psikologi 2=0|Faktor Ergonomi 1=0|Faktor Ergonomi 2=0|Lama tahun bekerja 1=0|Lama Bulan bekerja 1=0|Lama tahun bekerja 2=0|Lama bulan bekerja 2=0|Penyakit apa dan sejak kapan=|Penyakit apa dan sejak kapan=|Operasi Apa dan kapan=|Kecelakan Apa dan Kapan=|Jika Ya, Penyakit Apa=|Jika Ya, Penyakit Apa=|Pengobatan Apa Dan Apakah Terkontrol=|AutoRef Kanan=|AutoRef Kiri=|NCT / Tonometri Kanan=|NCT / Tonometri Kiri=|Kacamata Kanan=|Kacamata Kanan=|Anamnesa=|Pemeriksaan Fisik=|Radiologi Abnormal=|Elektro Kardiografi Description=|Audiometri=|Laboratorium=|Lain-lain=|Saran=|Saran=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Kesimpulan=|Penyakit Diabetes Keterangan=|Spirometri=|Pap Smear=|Jumlah Rokok(Batang/hari)=0|Alkohol(kali/Minggu)=0|Minum Kopi (Gelas/hari)=0|Olahraga(Jam/Satu Minggu)=0|Klg yg Hypertensi=|Klg yg Diabetes=|Klg yg Sakit Jantung=|Klg yg Sakit Ginjal=|Klg yg Sakit Ganguan Jiwa=|Klg yg Sakit Lain2/ALergi=|Sudah Berapa Lama=|Sudah Berapa Lama=|Tinggi Badan=|Berat Badan=|Anjuran Berat Badan=|Body Mass Index=|keterangan Abnormal=|Visus + Koreksi Kanan=|Visus + Koreksi Kiri=|Add Visus Kanan=|Add Visus Kiri=|Lain-lain=|Gigi Atas - Keterangan=|Gigi Bawah - Keterangan=|Keterangan Tonsil=|Nilai Pernapasan=|Tekanan Darah (mm/Hg)=|Frequensi Nadi=|Irama Kardiovaskular=|Ket Abnormal Tengkorak=|Ket Abnormal Tengkorak=|Ket Abnormal Anggota Gerak Atas=|Ket Abnormal Anggota Gerak Atas=|Nama Perusahaan 1=0|Nama Perusahaan 2=0|Jenis Pekerjaan 1=0|Jenis Pekerjaan 2=0|Faktor Fisika 1=0|Faktor Fisika 2=0|Faktor Kimia 1=0|Faktor Kimia 1=0|Faktor Biologi 1=0|Faktor Biologi 2=0|Faktor Psikologi 1=0|Faktor Psikologi 2=0|Faktor Ergonomi 1=0|Faktor Ergonomi 2=0|Lama tahun bekerja 1=0|Lama Bulan bekerja 1=0|Lama tahun bekerja 2=0|Lama bulan bekerja 2=0|Penyakit apa dan sejak kapan=|Penyakit apa dan sejak kapan=|Operasi Apa dan kapan=|Kecelakan Apa dan Kapan=|Jika Ya, Penyakit Apa=|Jika Ya, Penyakit Apa=|Pengobatan Apa Dan Apakah Terkontrol=|AutoRef Kanan=|AutoRef Kiri=|NCT / Tonometri Kanan=|NCT / Tonometri Kiri=|Kacamata Kanan=|Kacamata Kanan=|Anamnesa=|Pemeriksaan Fisik=|Radiologi Abnormal=|Elektro Kardiografi Description=|Audiometri=|Laboratorium=|Lain-lain=|Saran=|Saran=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Kesimpulan=|Penyakit Diabetes Keterangan=|Spirometri=|Pap Smear=|Jumlah Rokok(Batang/hari)=0|Alkohol(kali/Minggu)=0|Minum Kopi (Gelas/hari)=0|Olahraga(Jam/Satu Minggu)=0|Klg yg Hypertensi=|Klg yg Diabetes=|Klg yg Sakit Jantung=|Klg yg Sakit Ginjal=|Klg yg Sakit Ganguan Jiwa=|Klg yg Sakit Lain2/ALergi=|Sudah Berapa Lama=|Sudah Berapa Lama=|Tinggi Badan=|Berat Badan=|Anjuran Berat Badan=|Body Mass Index=|keterangan Abnormal=|Visus + Koreksi Kanan=|Visus + Koreksi Kiri=|Add Visus Kanan=|Add Visus Kiri=|Lain-lain=|Gigi Atas - Keterangan=|Gigi Bawah - Keterangan=|Keterangan Tonsil=|Nilai Pernapasan=|Tekanan Darah (mm/Hg)=|Frequensi Nadi=|Irama Kardiovaskular=|Ket Abnormal Tengkorak=|Ket Abnormal Tengkorak=|Ket Abnormal Anggota Gerak Atas=|Ket Abnormal Anggota Gerak Atas=|Nama Perusahaan 1=0|Nama Perusahaan 2=0|Jenis Pekerjaan 1=0|Jenis Pekerjaan 2=0|Faktor Fisika 1=0|Faktor Fisika 2=0|Faktor Kimia 1=0|Faktor Kimia 1=0|Faktor Biologi 1=0|Faktor Biologi 2=0|Faktor Psikologi 1=0|Faktor Psikologi 2=0|Faktor Ergonomi 1=0|Faktor Ergonomi 2=0|Lama tahun bekerja 1=0|Lama Bulan bekerja 1=0|Lama tahun bekerja 2=0|Lama bulan bekerja 2=0|Penyakit apa dan sejak kapan=|Penyakit apa dan sejak kapan=|Operasi Apa dan kapan=|Kecelakan Apa dan Kapan=|Jika Ya, Penyakit Apa=|Jika Ya, Penyakit Apa=|Pengobatan Apa Dan Apakah Terkontrol=|AutoRef Kanan=|AutoRef Kiri=|NCT / Tonometri Kanan=|NCT / Tonometri Kiri=|Kacamata Kanan=|Kacamata Kanan=|Anamnesa=|Pemeriksaan Fisik=|Radiologi Abnormal=|Elektro Kardiografi Description=|Audiometri=|Laboratorium=|Lain-lain=|Saran=|Saran=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Kesimpulan=|Penyakit Diabetes Keterangan=|Spirometri=|Pap Smear=|Jumlah Rokok(Batang/hari)=0|Alkohol(kali/Minggu)=0|Minum Kopi (Gelas/hari)=0|Olahraga(Jam/Satu Minggu)=0|Klg yg Hypertensi=|Klg yg Diabetes=|Klg yg Sakit Jantung=|Klg yg Sakit Ginjal=|Klg yg Sakit Ganguan Jiwa=|Klg yg Sakit Lain2/ALergi=|Sudah Berapa Lama=|Sudah Berapa Lama=|Tinggi Badan=|Berat Badan=|Anjuran Berat Badan=|Body Mass Index=|keterangan Abnormal=|Visus + Koreksi Kanan=|Visus + Koreksi Kiri=|Add Visus Kanan=|Add Visus Kiri=|Lain-lain=|Gigi Atas - Keterangan=|Gigi Bawah - Keterangan=|Keterangan Tonsil=|Nilai Pernapasan=|Tekanan Darah (mm/Hg)=|Frequensi Nadi=|Irama Kardiovaskular=|Ket Abnormal Tengkorak=|Ket Abnormal Tengkorak=|Ket Abnormal Anggota Gerak Atas=|Ket Abnormal Anggota Gerak Atas=|Nama Perusahaan 1=0|Nama Perusahaan 2=0|Jenis Pekerjaan 1=0|Jenis Pekerjaan 2=0|Faktor Fisika 1=0|Faktor Fisika 2=0|Faktor Kimia 1=0|Faktor Kimia 1=0|Faktor Biologi 1=0|Faktor Biologi 2=0|Faktor Psikologi 1=0|Faktor Psikologi 2=0|Faktor Ergonomi 1=0|Faktor Ergonomi 2=0|Lama tahun bekerja 1=0|Lama Bulan bekerja 1=0|Lama tahun bekerja 2=0|Lama bulan bekerja 2=0|Penyakit apa dan sejak kapan=|Penyakit apa dan sejak kapan=|Operasi Apa dan kapan=|Kecelakan Apa dan Kapan=|Jika Ya, Penyakit Apa=|Jika Ya, Penyakit Apa=|Pengobatan Apa Dan Apakah Terkontrol=|AutoRef Kanan=|AutoRef Kiri=|NCT / Tonometri Kanan=|NCT / Tonometri Kiri=|Kacamata Kanan=|Kacamata Kanan=|Anamnesa=|Pemeriksaan Fisik=|Radiologi Abnormal=|Elektro Kardiografi Description=|Audiometri=|Laboratorium=|Lain-lain=|Saran=|Saran=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Kesimpulan=|Penyakit Diabetes Keterangan=|Spirometri=|Pap Smear=|Jumlah Rokok(Batang/hari)=0|Alkohol(kali/Minggu)=0|Minum Kopi (Gelas/hari)=0|Olahraga(Jam/Satu Minggu)=0|Klg yg Hypertensi=|Klg yg Diabetes=|Klg yg Sakit Jantung=|Klg yg Sakit Ginjal=|Klg yg Sakit Ganguan Jiwa=|Klg yg Sakit Lain2/ALergi=|Sudah Berapa Lama=|Sudah Berapa Lama=|Tinggi Badan=|Berat Badan=|Anjuran Berat Badan=|Body Mass Index=|keterangan Abnormal=|Visus + Koreksi Kanan=|Visus + Koreksi Kiri=|Add Visus Kanan=|Add Visus Kiri=|Lain-lain=|Gigi Atas - Keterangan=|Gigi Bawah - Keterangan=|Keterangan Tonsil=|Nilai Pernapasan=|Tekanan Darah (mm/Hg)=|Frequensi Nadi=|Irama Kardiovaskular=|Ket Abnormal Tengkorak=|Ket Abnormal Tengkorak=|Ket Abnormal Anggota Gerak Atas=|Ket Abnormal Anggota Gerak Atas=|Nama Perusahaan 1=0|Nama Perusahaan 2=0|Jenis Pekerjaan 1=0|Jenis Pekerjaan 2=0|Faktor Fisika 1=0|Faktor Fisika 2=0|Faktor Kimia 1=0|Faktor Kimia 1=0|Faktor Biologi 1=0|Faktor Biologi 2=0|Faktor Psikologi 1=0|Faktor Psikologi 2=0|Faktor Ergonomi 1=0|Faktor Ergonomi 2=0|Lama tahun bekerja 1=0|Lama Bulan bekerja 1=0|Lama tahun bekerja 2=0|Lama bulan bekerja 2=0|Penyakit apa dan sejak kapan=|Penyakit apa dan sejak kapan=|Operasi Apa dan kapan=|Kecelakan Apa dan Kapan=|Jika Ya, Penyakit Apa=|Jika Ya, Penyakit Apa=|Pengobatan Apa Dan Apakah Terkontrol=|AutoRef Kanan=|AutoRef Kiri=|NCT / Tonometri Kanan=|NCT / Tonometri Kiri=|Kacamata Kanan=|Kacamata Kanan=|Anamnesa=|Pemeriksaan Fisik=|Radiologi Abnormal=|Elektro Kardiografi Description=|Audiometri=|Laboratorium=|Lain-lain=|Saran=|Saran=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Kesimpulan=|Penyakit Diabetes Keterangan=|Spirometri=|Pap Smear=|Jumlah Rokok(Batang/hari)=0|Alkohol(kali/Minggu)=0|Minum Kopi (Gelas/hari)=0|Olahraga(Jam/Satu Minggu)=0|Klg yg Hypertensi=|Klg yg Diabetes=|Klg yg Sakit Jantung=|Klg yg Sakit Ginjal=|Klg yg Sakit Ganguan Jiwa=|Klg yg Sakit Lain2/ALergi=|Sudah Berapa Lama=|Sudah Berapa Lama=|Tinggi Badan=|Berat Badan=|Anjuran Berat Badan=|Body Mass Index=|keterangan Abnormal=|Visus + Koreksi Kanan=|Visus + Koreksi Kiri=|Add Visus Kanan=|Add Visus Kiri=|Lain-lain=|Gigi Atas - Keterangan=|Gigi Bawah - Keterangan=|Keterangan Tonsil=|Nilai Pernapasan=|Tekanan Darah (mm/Hg)=|Frequensi Nadi=|Irama Kardiovaskular=|Ket Abnormal Tengkorak=|Ket Abnormal Tengkorak=|Ket Abnormal Anggota Gerak Atas=|Ket Abnormal Anggota Gerak Atas=|Nama Perusahaan 1=0|Nama Perusahaan 2=0|Jenis Pekerjaan 1=0|Jenis Pekerjaan 2=0|Faktor Fisika 1=0|Faktor Fisika 2=0|Faktor Kimia 1=0|Faktor Kimia 1=0|Faktor Biologi 1=0|Faktor Biologi 2=0|Faktor Psikologi 1=0|Faktor Psikologi 2=0|Faktor Ergonomi 1=0|Faktor Ergonomi 2=0|Lama tahun bekerja 1=0|Lama Bulan bekerja 1=0|Lama tahun bekerja 2=0|Lama bulan bekerja 2=0|Penyakit apa dan sejak kapan=|Penyakit apa dan sejak kapan=|Operasi Apa dan kapan=|Kecelakan Apa dan Kapan=|Jika Ya, Penyakit Apa=|Jika Ya, Penyakit Apa=|Pengobatan Apa Dan Apakah Terkontrol=|AutoRef Kanan=|AutoRef Kiri=|NCT / Tonometri Kanan=|NCT / Tonometri Kiri=|Kacamata Kanan=|Kacamata Kanan=|Anamnesa=|Pemeriksaan Fisik=|Radiologi Abnormal=|Elektro Kardiografi Description=|Audiometri=|Laboratorium=|Lain-lain=|Saran=|Saran=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Kesimpulan=|Penyakit Diabetes Keterangan=|Spirometri=|Pap Smear=|Jumlah Rokok(Batang/hari)=0|Alkohol(kali/Minggu)=0|Minum Kopi (Gelas/hari)=0|Olahraga(Jam/Satu Minggu)=0|Klg yg Hypertensi=|Klg yg Diabetes=|Klg yg Sakit Jantung=|Klg yg Sakit Ginjal=|Klg yg Sakit Ganguan Jiwa=|Klg yg Sakit Lain2/ALergi=|Sudah Berapa Lama=|Sudah Berapa Lama=|Tinggi Badan=|Berat Badan=|Anjuran Berat Badan=|Body Mass Index=|keterangan Abnormal=|Visus + Koreksi Kanan=|Visus + Koreksi Kiri=|Add Visus Kanan=|Add Visus Kiri=|Lain-lain=|Gigi Atas - Keterangan=|Gigi Bawah - Keterangan=|Keterangan Tonsil=|Nilai Pernapasan=|Tekanan Darah (mm/Hg)=|Frequensi Nadi=|Irama Kardiovaskular=|Ket Abnormal Tengkorak=|Ket Abnormal Tengkorak=|Ket Abnormal Anggota Gerak Atas=|Ket Abnormal Anggota Gerak Atas=|Nama Perusahaan 1=0|Nama Perusahaan 2=0|Jenis Pekerjaan 1=0|Jenis Pekerjaan 2=0|Faktor Fisika 1=0|Faktor Fisika 2=0|Faktor Kimia 1=0|Faktor Kimia 1=0|Faktor Biologi 1=0|Faktor Biologi 2=0|Faktor Psikologi 1=0|Faktor Psikologi 2=0|Faktor Ergonomi 1=0|Faktor Ergonomi 2=0|Lama tahun bekerja 1=0|Lama Bulan bekerja 1=0|Lama tahun bekerja 2=0|Lama bulan bekerja 2=0|Penyakit apa dan sejak kapan=|Penyakit apa dan sejak kapan=|Operasi Apa dan kapan=|Kecelakan Apa dan Kapan=|Jika Ya, Penyakit Apa=|Jika Ya, Penyakit Apa=|Pengobatan Apa Dan Apakah Terkontrol=|AutoRef Kanan=|AutoRef Kiri=|NCT / Tonometri Kanan=|NCT / Tonometri Kiri=|Kacamata Kanan=|Kacamata Kanan=|Anamnesa=|Pemeriksaan Fisik=|Radiologi Abnormal=|Elektro Kardiografi Description=|Audiometri=|Laboratorium=|Lain-lain=|Saran=|Saran=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Kesimpulan=|Penyakit Diabetes Keterangan=|Spirometri=|Pap Smear=|Jumlah Rokok(Batang/hari)=0|Alkohol(kali/Minggu)=0|Minum Kopi (Gelas/hari)=0|Olahraga(Jam/Satu Minggu)=0|Klg yg Hypertensi=|Klg yg Diabetes=|Klg yg Sakit Jantung=|Klg yg Sakit Ginjal=|Klg yg Sakit Ganguan Jiwa=|Klg yg Sakit Lain2/ALergi=|Sudah Berapa Lama=|Sudah Berapa Lama=|Tinggi Badan=|Berat Badan=|Anjuran Berat Badan=|Body Mass Index=|keterangan Abnormal=|Visus + Koreksi Kanan=|Visus + Koreksi Kiri=|Add Visus Kanan=|Add Visus Kiri=|Lain-lain=|Gigi Atas - Keterangan=|Gigi Bawah - Keterangan=|Keterangan Tonsil=|Nilai Pernapasan=|Tekanan Darah (mm/Hg)=|Frequensi Nadi=|Irama Kardiovaskular=|Ket Abnormal Tengkorak=|Ket Abnormal Tengkorak=|Ket Abnormal Anggota Gerak Atas=|Ket Abnormal Anggota Gerak Atas=|Nama Perusahaan 1=0|Nama Perusahaan 2=0|Jenis Pekerjaan 1=0|Jenis Pekerjaan 2=0|Faktor Fisika 1=0|Faktor Fisika 2=0|Faktor Kimia 1=0|Faktor Kimia 1=0|Faktor Biologi 1=0|Faktor Biologi 2=0|Faktor Psikologi 1=0|Faktor Psikologi 2=0|Faktor Ergonomi 1=0|Faktor Ergonomi 2=0|Lama tahun bekerja 1=0|Lama Bulan bekerja 1=0|Lama tahun bekerja 2=0|Lama bulan bekerja 2=0|Penyakit apa dan sejak kapan=|Penyakit apa dan sejak kapan=|Operasi Apa dan kapan=|Kecelakan Apa dan Kapan=|Jika Ya, Penyakit Apa=|Jika Ya, Penyakit Apa=|Pengobatan Apa Dan Apakah Terkontrol=|AutoRef Kanan=|AutoRef Kiri=|NCT / Tonometri Kanan=|NCT / Tonometri Kiri=|Kacamata Kanan=|Kacamata Kanan=|Anamnesa=|Pemeriksaan Fisik=|Radiologi Abnormal=|Elektro Kardiografi Description=|Audiometri=|Laboratorium=|Lain-lain=|Saran=|Saran=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Kesimpulan=|Penyakit Diabetes Keterangan=|Spirometri=|Pap Smear=|Jumlah Rokok(Batang/hari)=0|Alkohol(kali/Minggu)=0|Minum Kopi (Gelas/hari)=0|Olahraga(Jam/Satu Minggu)=0|Klg yg Hypertensi=|Klg yg Diabetes=|Klg yg Sakit Jantung=|Klg yg Sakit Ginjal=|Klg yg Sakit Ganguan Jiwa=|Klg yg Sakit Lain2/ALergi=|Sudah Berapa Lama=|Sudah Berapa Lama=|Tinggi Badan=|Berat Badan=|Anjuran Berat Badan=|Body Mass Index=|keterangan Abnormal=|Visus + Koreksi Kanan=|Visus + Koreksi Kiri=|Add Visus Kanan=|Add Visus Kiri=|Lain-lain=|Gigi Atas - Keterangan=|Gigi Bawah - Keterangan=|Keterangan Tonsil=|Nilai Pernapasan=|Tekanan Darah (mm/Hg)=|Frequensi Nadi=|Irama Kardiovaskular=|Ket Abnormal Tengkorak=|Ket Abnormal Tengkorak=|Ket Abnormal Anggota Gerak Atas=|Ket Abnormal Anggota Gerak Atas=|Nama Perusahaan 1=0|Nama Perusahaan 2=0|Jenis Pekerjaan 1=0|Jenis Pekerjaan 2=0|Faktor Fisika 1=0|Faktor Fisika 2=0|Faktor Kimia 1=0|Faktor Kimia 1=0|Faktor Biologi 1=0|Faktor Biologi 2=0|Faktor Psikologi 1=0|Faktor Psikologi 2=0|Faktor Ergonomi 1=0|Faktor Ergonomi 2=0|Lama tahun bekerja 1=0|Lama Bulan bekerja 1=0|Lama tahun bekerja 2=0|Lama bulan bekerja 2=0|Penyakit apa dan sejak kapan=|Penyakit apa dan sejak kapan=|Operasi Apa dan kapan=|Kecelakan Apa dan Kapan=|Jika Ya, Penyakit Apa=|Jika Ya, Penyakit Apa=|Pengobatan Apa Dan Apakah Terkontrol=|AutoRef Kanan=|AutoRef Kiri=|NCT / Tonometri Kanan=|NCT / Tonometri Kiri=|Kacamata Kanan=|Kacamata Kanan=|Anamnesa=|Pemeriksaan Fisik=|Radiologi Abnormal=|Elektro Kardiografi Description=|Audiometri=|Laboratorium=|Lain-lain=|Saran=|Saran=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Kesimpulan=|Penyakit Diabetes Keterangan=|Spirometri=|Pap Smear=|Jumlah Rokok(Batang/hari)=0|Alkohol(kali/Minggu)=0|Minum Kopi (Gelas/hari)=0|Olahraga(Jam/Satu Minggu)=0|Klg yg Hypertensi=|Klg yg Diabetes=|Klg yg Sakit Jantung=|Klg yg Sakit Ginjal=|Klg yg Sakit Ganguan Jiwa=|Klg yg Sakit Lain2/ALergi=|Sudah Berapa Lama=|Sudah Berapa Lama=|Tinggi Badan=|Berat Badan=|Anjuran Berat Badan=|Body Mass Index=|keterangan Abnormal=|Visus + Koreksi Kanan=|Visus + Koreksi Kiri=|Add Visus Kanan=|Add Visus Kiri=|Lain-lain=|Gigi Atas - Keterangan=|Gigi Bawah - Keterangan=|Keterangan Tonsil=|Nilai Pernapasan=|Tekanan Darah (mm/Hg)=|Frequensi Nadi=|Irama Kardiovaskular=|Ket Abnormal Tengkorak=|Ket Abnormal Tengkorak=|Ket Abnormal Anggota Gerak Atas=|Ket Abnormal Anggota Gerak Atas=|Nama Perusahaan 1=0|Nama Perusahaan 2=0|Jenis Pekerjaan 1=0|Jenis Pekerjaan 2=0|Faktor Fisika 1=0|Faktor Fisika 2=0|Faktor Kimia 1=0|Faktor Kimia 1=0|Faktor Biologi 1=0|Faktor Biologi 2=0|Faktor Psikologi 1=0|Faktor Psikologi 2=0|Faktor Ergonomi 1=0|Faktor Ergonomi 2=0|Lama tahun bekerja 1=0|Lama Bulan bekerja 1=0|Lama tahun bekerja 2=0|Lama bulan bekerja 2=0|Penyakit apa dan sejak kapan=|Penyakit apa dan sejak kapan=|Operasi Apa dan kapan=|Kecelakan Apa dan Kapan=|Jika Ya, Penyakit Apa=|Jika Ya, Penyakit Apa=|Pengobatan Apa Dan Apakah Terkontrol=|AutoRef Kanan=|AutoRef Kiri=|NCT / Tonometri Kanan=|NCT / Tonometri Kiri=|Kacamata Kanan=|Kacamata Kanan=|Anamnesa=|Pemeriksaan Fisik=|Radiologi Abnormal=|Elektro Kardiografi Description=|Audiometri=|Laboratorium=|Lain-lain=|Saran=|Saran=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Kesimpulan=|Penyakit Diabetes Keterangan=|Spirometri=|Pap Smear=|Jumlah Rokok(Batang/hari)=0|Alkohol(kali/Minggu)=0|Minum Kopi (Gelas/hari)=0|Olahraga(Jam/Satu Minggu)=0|Klg yg Hypertensi=|Klg yg Diabetes=|Klg yg Sakit Jantung=|Klg yg Sakit Ginjal=|Klg yg Sakit Ganguan Jiwa=|Klg yg Sakit Lain2/ALergi=|Sudah Berapa Lama=|Sudah Berapa Lama=|Tinggi Badan=|Berat Badan=|Anjuran Berat Badan=|Body Mass Index=|keterangan Abnormal=|Visus + Koreksi Kanan=|Visus + Koreksi Kiri=|Add Visus Kanan=|Add Visus Kiri=|Lain-lain=|Gigi Atas - Keterangan=|Gigi Bawah - Keterangan=|Keterangan Tonsil=|Nilai Pernapasan=|Tekanan Darah (mm/Hg)=|Frequensi Nadi=|Irama Kardiovaskular=|Ket Abnormal Tengkorak=|Ket Abnormal Tengkorak=|Ket Abnormal Anggota Gerak Atas=|Ket Abnormal Anggota Gerak Atas=|Nama Perusahaan 1=0|Nama Perusahaan 2=0|Jenis Pekerjaan 1=0|Jenis Pekerjaan 2=0|Faktor Fisika 1=0|Faktor Fisika 2=0|Faktor Kimia 1=0|Faktor Kimia 1=0|Faktor Biologi 1=0|Faktor Biologi 2=0|Faktor Psikologi 1=0|Faktor Psikologi 2=0|Faktor Ergonomi 1=0|Faktor Ergonomi 2=0|Lama tahun bekerja 1=0|Lama Bulan bekerja 1=0|Lama tahun bekerja 2=0|Lama bulan bekerja 2=0|Penyakit apa dan sejak kapan=|Penyakit apa dan sejak kapan=|Operasi Apa dan kapan=|Kecelakan Apa dan Kapan=|Jika Ya, Penyakit Apa=|Jika Ya, Penyakit Apa=|Pengobatan Apa Dan Apakah Terkontrol=|AutoRef Kanan=|AutoRef Kiri=|NCT / Tonometri Kanan=|NCT / Tonometri Kiri=|Kacamata Kanan=|Kacamata Kanan=|Anamnesa=|Pemeriksaan Fisik=|Radiologi Abnormal=|Elektro Kardiografi Description=|Audiometri=|Laboratorium=|Lain-lain=|Saran=|Saran=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Kesimpulan=|Penyakit Diabetes Keterangan=|Spirometri=|Pap Smear=|Jumlah Rokok(Batang/hari)=0|Alkohol(kali/Minggu)=0|Minum Kopi (Gelas/hari)=0|Olahraga(Jam/Satu Minggu)=0|Klg yg Hypertensi=|Klg yg Diabetes=|Klg yg Sakit Jantung=|Klg yg Sakit Ginjal=|Klg yg Sakit Ganguan Jiwa=|Klg yg Sakit Lain2/ALergi=|Sudah Berapa Lama=|Sudah Berapa Lama=|Tinggi Badan=|Berat Badan=|Anjuran Berat Badan=|Body Mass Index=|keterangan Abnormal=|Visus + Koreksi Kanan=|Visus + Koreksi Kiri=|Add Visus Kanan=|Add Visus Kiri=|Lain-lain=|Gigi Atas - Keterangan=|Gigi Bawah - Keterangan=|Keterangan Tonsil=|Nilai Pernapasan=|Tekanan Darah (mm/Hg)=|Frequensi Nadi=|Irama Kardiovaskular=|Ket Abnormal Tengkorak=|Ket Abnormal Tengkorak=|Ket Abnormal Anggota Gerak Atas=|Ket Abnormal Anggota Gerak Atas=|Nama Perusahaan 1=0|Nama Perusahaan 2=0|Jenis Pekerjaan 1=0|Jenis Pekerjaan 2=0|Faktor Fisika 1=0|Faktor Fisika 2=0|Faktor Kimia 1=0|Faktor Kimia 1=0|Faktor Biologi 1=0|Faktor Biologi 2=0|Faktor Psikologi 1=0|Faktor Psikologi 2=0|Faktor Ergonomi 1=0|Faktor Ergonomi 2=0|Lama tahun bekerja 1=0|Lama Bulan bekerja 1=0|Lama tahun bekerja 2=0|Lama bulan bekerja 2=0|Penyakit apa dan sejak kapan=|Penyakit apa dan sejak kapan=|Operasi Apa dan kapan=|Kecelakan Apa dan Kapan=|Jika Ya, Penyakit Apa=|Jika Ya, Penyakit Apa=|Pengobatan Apa Dan Apakah Terkontrol=|AutoRef Kanan=|AutoRef Kiri=|NCT / Tonometri Kanan=|NCT / Tonometri Kiri=|Kacamata Kanan=|Kacamata Kanan=|Anamnesa=|Pemeriksaan Fisik=|Radiologi Abnormal=|Elektro Kardiografi Description=|Audiometri=|Laboratorium=|Lain-lain=|Saran=|Saran=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Kesimpulan=|Penyakit Diabetes Keterangan=|Spirometri=|Pap Smear=|Jumlah Rokok(Batang/hari)=0|Alkohol(kali/Minggu)=0|Minum Kopi (Gelas/hari)=0|Olahraga(Jam/Satu Minggu)=0|Klg yg Hypertensi=|Klg yg Diabetes=|Klg yg Sakit Jantung=|Klg yg Sakit Ginjal=|Klg yg Sakit Ganguan Jiwa=|Klg yg Sakit Lain2/ALergi=|Sudah Berapa Lama=|Sudah Berapa Lama=|Tinggi Badan=|Berat Badan=|Anjuran Berat Badan=|Body Mass Index=|keterangan Abnormal=|Visus + Koreksi Kanan=|Visus + Koreksi Kiri=|Add Visus Kanan=|Add Visus Kiri=|Lain-lain=|Gigi Atas - Keterangan=|Gigi Bawah - Keterangan=|Keterangan Tonsil=|Nilai Pernapasan=|Tekanan Darah (mm/Hg)=|Frequensi Nadi=|Irama Kardiovaskular=|Ket Abnormal Tengkorak=|Ket Abnormal Tengkorak=|Ket Abnormal Anggota Gerak Atas=|Ket Abnormal Anggota Gerak Atas=|Nama Perusahaan 1=0|Nama Perusahaan 2=0|Jenis Pekerjaan 1=0|Jenis Pekerjaan 2=0|Faktor Fisika 1=0|Faktor Fisika 2=0|Faktor Kimia 1=0|Faktor Kimia 1=0|Faktor Biologi 1=0|Faktor Biologi 2=0|Faktor Psikologi 1=0|Faktor Psikologi 2=0|Faktor Ergonomi 1=0|Faktor Ergonomi 2=0|Lama tahun bekerja 1=0|Lama Bulan bekerja 1=0|Lama tahun bekerja 2=0|Lama bulan bekerja 2=0|Penyakit apa dan sejak kapan=|Penyakit apa dan sejak kapan=|Operasi Apa dan kapan=|Kecelakan Apa dan Kapan=|Jika Ya, Penyakit Apa=|Jika Ya, Penyakit Apa=|Pengobatan Apa Dan Apakah Terkontrol=|AutoRef Kanan=|AutoRef Kiri=|NCT / Tonometri Kanan=|NCT / Tonometri Kiri=|Kacamata Kanan=|Kacamata Kanan=|Anamnesa=|Pemeriksaan Fisik=|Radiologi Abnormal=|Elektro Kardiografi Description=|Audiometri=|Laboratorium=|Lain-lain=|Saran=|Saran=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Kesimpulan=|Penyakit Diabetes Keterangan=|Spirometri=|Pap Smear=|Jumlah Rokok(Batang/hari)=0|Alkohol(kali/Minggu)=0|Minum Kopi (Gelas/hari)=0|Olahraga(Jam/Satu Minggu)=0|Klg yg Hypertensi=|Klg yg Diabetes=|Klg yg Sakit Jantung=|Klg yg Sakit Ginjal=|Klg yg Sakit Ganguan Jiwa=|Klg yg Sakit Lain2/ALergi=|Sudah Berapa Lama=|Sudah Berapa Lama=|Tinggi Badan=|Berat Badan=|Anjuran Berat Badan=|Body Mass Index=|keterangan Abnormal=|Visus + Koreksi Kanan=|Visus + Koreksi Kiri=|Add Visus Kanan=|Add Visus Kiri=|Lain-lain=|Gigi Atas - Keterangan=|Gigi Bawah - Keterangan=|Keterangan Tonsil=|Nilai Pernapasan=|Tekanan Darah (mm/Hg)=|Frequensi Nadi=|Irama Kardiovaskular=|Ket Abnormal Tengkorak=|Ket Abnormal Tengkorak=|Ket Abnormal Anggota Gerak Atas=|Ket Abnormal Anggota Gerak Atas=|Nama Perusahaan 1=0|Nama Perusahaan 2=0|Jenis Pekerjaan 1=0|Jenis Pekerjaan 2=0|Faktor Fisika 1=0|Faktor Fisika 2=0|Faktor Kimia 1=0|Faktor Kimia 1=0|Faktor Biologi 1=0|Faktor Biologi 2=0|Faktor Psikologi 1=0|Faktor Psikologi 2=0|Faktor Ergonomi 1=0|Faktor Ergonomi 2=0|Lama tahun bekerja 1=0|Lama Bulan bekerja 1=0|Lama tahun bekerja 2=0|Lama bulan bekerja 2=0|Penyakit apa dan sejak kapan=|Penyakit apa dan sejak kapan=|Operasi Apa dan kapan=|Kecelakan Apa dan Kapan=|Jika Ya, Penyakit Apa=|Jika Ya, Penyakit Apa=|Pengobatan Apa Dan Apakah Terkontrol=|AutoRef Kanan=|AutoRef Kiri=|NCT / Tonometri Kanan=|NCT / Tonometri Kiri=|Kacamata Kanan=|Kacamata Kanan=|Anamnesa=|Pemeriksaan Fisik=|Radiologi Abnormal=|Elektro Kardiografi Description=|Audiometri=|Laboratorium=|Lain-lain=|Saran=|Saran=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Kesimpulan=|Penyakit Diabetes Keterangan=|Spirometri=|Pap Smear=|Jumlah Rokok(Batang/hari)=0|Alkohol(kali/Minggu)=0|Minum Kopi (Gelas/hari)=0|Olahraga(Jam/Satu Minggu)=0|Klg yg Hypertensi=|Klg yg Diabetes=|Klg yg Sakit Jantung=|Klg yg Sakit Ginjal=|Klg yg Sakit Ganguan Jiwa=|Klg yg Sakit Lain2/ALergi=|Sudah Berapa Lama=|Sudah Berapa Lama=|Tinggi Badan=|Berat Badan=|Anjuran Berat Badan=|Body Mass Index=|keterangan Abnormal=|Visus + Koreksi Kanan=|Visus + Koreksi Kiri=|Add Visus Kanan=|Add Visus Kiri=|Lain-lain=|Gigi Atas - Keterangan=|Gigi Bawah - Keterangan=|Keterangan Tonsil=|Nilai Pernapasan=|Tekanan Darah (mm/Hg)=|Frequensi Nadi=|Irama Kardiovaskular=|Ket Abnormal Tengkorak=|Ket Abnormal Tengkorak=|Ket Abnormal Anggota Gerak Atas=|Ket Abnormal Anggota Gerak Atas=|Nama Perusahaan 1=0|Nama Perusahaan 2=0|Jenis Pekerjaan 1=0|Jenis Pekerjaan 2=0|Faktor Fisika 1=0|Faktor Fisika 2=0|Faktor Kimia 1=0|Faktor Kimia 1=0|Faktor Biologi 1=0|Faktor Biologi 2=0|Faktor Psikologi 1=0|Faktor Psikologi 2=0|Faktor Ergonomi 1=0|Faktor Ergonomi 2=0|Lama tahun bekerja 1=0|Lama Bulan bekerja 1=0|Lama tahun bekerja 2=0|Lama bulan bekerja 2=0|Penyakit apa dan sejak kapan=|Penyakit apa dan sejak kapan=|Operasi Apa dan kapan=|Kecelakan Apa dan Kapan=|Jika Ya, Penyakit Apa=|Jika Ya, Penyakit Apa=|Pengobatan Apa Dan Apakah Terkontrol=|AutoRef Kanan=|AutoRef Kiri=|NCT / Tonometri Kanan=|NCT / Tonometri Kiri=|Kacamata Kanan=|Kacamata Kanan=|Anamnesa=|Pemeriksaan Fisik=|Radiologi Abnormal=|Elektro Kardiografi Description=|Audiometri=|Laboratorium=|Lain-lain=|Saran=|Saran=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Keterangan Penyakit Akibat Kerja Dicurigai / Ada=|Kesimpulan=|Penyakit Diabetes Keterangan=|Spirometri=|Pap Smear=|Jumlah Rokok(Batang/hari)=0|Alkohol(kali/Minggu)=0|Minum Kopi (Gelas/hari)=0|Olahraga(Jam/Satu Minggu)=0|Klg yg Hypertensi=|Klg yg Diabetes=|Klg yg Sakit Jantung=|Klg yg Sakit Ginjal=|Klg yg Sakit Ganguan Jiwa=|Klg yg Sakit Lain2/ALergi=|Sudah Berapa Lama=|Sudah Berapa Lama=|Tinggi Badan=|Berat=10";
                   
                    //foreach (vMCUResultForm
                    

                    /*string contentLayout = "";
                    List<string> content = new List<string>();
                    string strhtml = "";
                    string strhtml2 = "";
                    foreach (vMCUResultForm row in lstMCUResultForm)
                    {
                        string FormLayout = row.FormLayout.ToString();
                        string ParamValue = row.FormValue.ToString();
                        contentLayout = Helper.ParsingFormElektronikLabelWithValue(FormLayout, ParamValue, row.ResultType);
                        content.Add(contentLayout);
                        strhtml += FormLayout;
                    }
                    strhtml2 = strhtml;
                    hdnFileString.Value = string.Join("", content);
                    */
                    //List<string> content = new List<string>();
                    //string strhtml = "";
                    //string strhtml2 = "";
                    //string str = string.Empty; 
                    /////"No. Pegawai=02121|Divisi=IT|Fisika=Pencahayaan|Kimia=Partike (Debu/Asap)|Biologi=Bakteri|Ergonomi=Angkat Beban Berat|Psikologi=Stress|Nama Perusahaan=PTABDEC|Nama Perusahaan=PTBDECE|Jenis Pekerjaan=Konsultasi|Jenis Pekerjaan=accounting|Fx Fisika=FxFisika1|Fx Fisika=FxFisika2|Fx Kimia=Kimia1|Fx Kimia=Kimia2|Fx Biologi=Bilogi1|Fx Biologi=Biologi2|Fx Biologi=Psikologi1|Fx Psikologi=Psikologi2|Fx Ergonomi=Ergonomi1|Fx Ergonomi=Ergonomi2|Lama Bekerja=1|Lama Bekerja=2|Pernah mengidap penyakit=Tidak|Sakit Apa=0|Terkontrol=Tidak|sakit apa=0|Lama Dirawat=0|";

                    //StringBuilder sb = new StringBuilder();
                    //List<FormQuestionModel> headerForm = new List<FormQuestionModel>();
                    //List<FormQuestionModel> lstForm = new List<FormQuestionModel>(); 

                    //foreach (vMCUResultForm row in lstMCUResultForm)
                    //{
                    //    sb.Append(row.FormResult);
                    //}
                    ////strhtml2 = strhtml;
                    //str = sb.ToString();
                    //string final = string.Empty;
                    //if (!string.IsNullOrEmpty(str))
                    //{

                    //    string[] param = str.Split('|');
                    //    ///List<String> strHeader = new List; 
                    //    List<String> strHeader = new List<string>();
                    //    List<String> strValue = new List<String>();
                    //    if (param.Length > 0)
                    //    {
                    //        for (int i = 0; i < param.Length; i++)
                    //        {

                    //            if (!string.IsNullOrEmpty(param[i]))
                    //            {
                    //                string[] tagField = param[i].Split('=');
                                   
                    //                strHeader.Add(tagField[0]);
                    //                strValue.Add(tagField[1]);
                    //            }
                    //        }
                    //      final  = String.Join(", ", strHeader);
                    //      final += '\n' + String.Join(", ", strValue); 
                    //    }
                    //}

                    hdnFileString.Value = String.Join(", ",jsonResponse); 

                    Response.AppendHeader("content-disposition", "attachment;filename=MCUResult.xls");
                    Response.Charset = "";
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "application/vnd.ms-excel";
                    this.EnableViewState = false;
                    Response.Write(hdnFileString.Value);
                    Response.End();  
                    
                }
            }
            
        }

        private void BindPrintout()
        {
            if (!string.IsNullOrEmpty(hdnID.Value) && hdnID.Value != "0")
            {
                List<vMCUResultForm> lstMCUResultForm = BusinessLayer.GetvMCUResultFormList(string.Format("RegistrationID = '{0}' AND IsDeleted = 0 ORDER BY ID ASC", AppSession.RegisteredPatient.RegistrationID));
                if (lstMCUResultForm.Count > 0 )
                {
                   string contentLayout = "";
                   List<string>content = new List<string>();
                   string strhtml = "";
                   string strhtml2 = "";
                   foreach(vMCUResultForm row in lstMCUResultForm)
                   {
                       string FormLayout = row.FormLayout.ToString();
                       string ParamValue = row.FormValue.ToString();
                       contentLayout= Helper.ParsingFormElektronik(FormLayout, ParamValue, row.ResultType);
                       content.Add(contentLayout);
                       strhtml += FormLayout;
                   }
                   strhtml2= strhtml;
                   hdnFileString.Value = string.Join("", content);

                   //#region createpdf

                   //StringReader sr = new StringReader(hdnFileString.Value);
                   //Document pdfDoc = new Document(PageSize.LETTER_LANDSCAPE);
                   //HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                   //using (MemoryStream memorystream = new MemoryStream())
                   //{
                   //    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memorystream);
                   //    pdfDoc.Open();
                   //    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                   //    //htmlparser.Parse(sr);
                   //    pdfDoc.Close();

                   //    byte[] bytes = memorystream.ToArray();
                   //    hdnFileString.Value = Convert.ToBase64String(bytes);

                   //    memorystream.Close();
                   //}
                   //byte[] bytes;
                   //using (StringReader sr = new StringReader(hdnFileString.Value))
                   //{
                   //    using (MemoryStream memorystream = new MemoryStream())
                   //    {
                   //        using (Document pdfDoc = new Document(PageSize.LETTER_LANDSCAPE))
                   //        {
                   //            //Bind a parser to our PDF document
                   //            using (HTMLWorker htmlparser = new HTMLWorker(pdfDoc))
                   //            {
                   //                //Bind the writer to our document and our final stream
                   //                using (PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memorystream))
                   //                {
                   //                    pdfDoc.Open();

                   //                    //Parse the HTML directly into the document
                   //                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                   //                    //htmlparser.Parse(sr);

                   //                    pdfDoc.Close();

                   //                    //Grab the bytes from the stream before closing it
                   //                    bytes = memorystream.ToArray();
                   //                    hdnFileString.Value = Convert.ToBase64String(bytes);
                   //                }
                   //            }
                   //        }
                   //    }
                   //}
                   //#endregion
                }
            }
            
        }

        protected override bool OnAddRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
           List<MCUResultForm> lstFormData = BusinessLayer.GetMCUResultFormList(string.Format("IsDeleted = 0 AND VisitID={0}", AppSession.RegisteredPatient.VisitID));
           SettingParameterDt oSetPar = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID='001' AND ParameterCode='{0}' ", Constant.SettingParameter.MC_PENGISIAN_HASIL_LEBIH_DARISATU)).FirstOrDefault();

           if (oSetPar.ParameterValue == "0") 
           {
               if (lstFormData.Count > 0)
               {
                   errMessage = string.Format("Maaf, Sudah memiliki form hasil sebelumnya.");
                   return false;
               }
           }

               url = ResolveUrl("~/Program/MCUResultForm/MCUResultFormEntryDetailCtl.ascx");
               queryString = string.Format("{0}|{1}|{2}|{3}|{4}|{5}", "1", hdnGCResultType.Value, hdnResultType.Value, hdnID.Value, hdnFormValue.Value, hdnRemarks.Value);
               popupWidth = sizePopupWidth;
               popupHeight = sizepopupHeight;
               popupHeaderText = string.Format("MCU Result : {0} (MRN = {1})", AppSession.RegisteredPatient.PatientName, AppSession.RegisteredPatient.MedicalNo);
               return true;
          
            
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            if (hdnID.Value != "")
            {
                url = ResolveUrl("~/Program/MCUResultForm/MCUResultFormEntryDetailCtl.ascx");
                queryString = string.Format("{0}|{1}|{2}|{3}|{4}|{5}", "0", hdnGCResultType.Value, hdnResultType.Value, hdnID.Value, hdnFormValue.Value, hdnRemarks.Value);
                popupWidth = sizePopupWidth;
                popupHeight = sizepopupHeight;
                popupHeaderText = string.Format("MCU Result : {0} (MRN = {1})", AppSession.RegisteredPatient.PatientName, AppSession.RegisteredPatient.MedicalNo);
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value != "")
            {
                MCUResultForm entity = BusinessLayer.GetMCUResultForm(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateMCUResultForm(entity);
                return true;
            }
            return false;
        }

    }
}