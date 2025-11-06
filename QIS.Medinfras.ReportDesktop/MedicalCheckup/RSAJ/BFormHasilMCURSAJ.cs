using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Linq;


namespace QIS.Medinfras.ReportDesktop
{
    public partial class BFormHasilMCURSAJ : BaseCustomDailyPotraitRpt
    {
        public BFormHasilMCURSAJ()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            vConsultVisit cv = BusinessLayer.GetvConsultVisitList(param[0].ToString()).FirstOrDefault();
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID)).FirstOrDefault();

            xrPictureBox1.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/logo.png");

            if (cv.MRN != 0 && cv.MRN != null)
            {
                xrPictureBox2.ImageUrl = cv.PatientImageUrl;
            }
            else
            {
                xrPictureBox2.ImageUrl = cv.GuestImageUrl;
            }

            lblReportTitle.Visible = false;
            lblReportSubTitle.Visible = false;
            ttdDokter.ImageUrl = string.Format("{0}{1}/Signature/{2}.png", AppConfigManager.QISPhysicalDirectory, AppConfigManager.QISParamedicImagePath, cv.ParamedicCode);
            ttdDokter.Visible = true;
            #region Detail
            List<MCUFormResultFieldReporting> lstData = new List<MCUFormResultFieldReporting>();
            string FilterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", cv.VisitID);
            List<vMCUResultFormReport> lst = BusinessLayer.GetvMCUResultFormReportList(FilterExpression);
            if (lst.Count > 0)
            {
                int ageInYear = 0;
                foreach (vMCUResultFormReport row in lst)
                {
                    string value = row.FormResult;
                    string[] data = value.Split('|');
                    if (data.Length > 0)
                    {
                        MCUFormResultFieldReporting oData = new MCUFormResultFieldReporting();
                        oData.TransactionNo = row.RegistrationNo;
                        oData.RegistrasiNo = row.RegistrationNo;
                        oData.TanggalPemeriksaan = oData.TglMasuk = row.RegistrationDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

                        if (row.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
                        {
                            oData.TglLahir = row.DateOfBirth.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                            ageInYear = Function.GetPatientAgeInYear(row.DateOfBirth, DateTime.Now);
                        }
                        else
                        {
                            oData.TglLahir = "";
                        }
                        oData.TempatLahir = row.CityOfBirth;
                        oData.JnsKelamin = row.Gender;
                        oData.Lokasi = row.CorporateAccountDepartment;
                        oData.NamaPegawai = row.PatientName;
                        oData.NoPegawai = row.CorporateAccountNo;
                        oData.ParamedicCode = row.ParamedicCode;
                        oData.ParamedicName = row.ParamedicName;
                        oData.Penjamin = row.BusinessPartnerName;
                        oData.PaketMCU = row.ItemName1;
                        oData.AlamatPegawai = row.StreetName;
                        oData.Posisi = row.CorporateAccountDepartment;
                        oData.StatusPerkawinan = row.MaritalStatus;

                        for (int i = 0; i < data.Length; i++)
                        {

                            string[] field = data[i].Split('^');
                            if (field.Length > 1)
                            {
                                string[] dataField = field[1].Split('=');
                                string dataFieldValue = dataField[1];
                                if (dataField.Length > 2)
                                {
                                    for (int indexField = 0; indexField < dataField.Length; indexField++)
                                    {
                                        if (i > 0)
                                        {
                                            dataFieldValue += string.Format("{0}=", dataField[indexField]);
                                        }
                                    }
                                    if (!string.IsNullOrEmpty(dataFieldValue))
                                    {
                                        dataFieldValue = dataFieldValue.Remove(dataFieldValue.Length - 1);
                                    }
                                }

                                #region filter
                                switch (field[0])
                                {
                                    case "1":
                                        oData.Col1 = dataFieldValue;
                                        break;
                                    case "2":
                                        oData.Col2 = dataFieldValue;
                                        break;
                                    case "3":
                                        oData.Col3 = dataFieldValue;
                                        break;
                                    case "4":
                                        oData.Col4 = dataFieldValue;
                                        break;
                                    case "5":
                                        oData.Col5 = dataFieldValue;
                                        break;
                                    case "6":
                                        oData.Col6 = dataFieldValue;
                                        break;
                                    case "7":
                                        oData.Col7 = dataFieldValue;
                                        break;
                                    case "8":
                                        oData.Col8 = dataFieldValue;
                                        break;
                                    case "9":
                                        oData.Col9 = dataFieldValue;
                                        break;
                                    case "10":
                                        oData.Col10 = dataFieldValue;
                                        break;
                                    case "11":
                                        oData.Col11 = dataFieldValue;
                                        break;
                                    case "12":
                                        oData.Col12 = dataFieldValue;
                                        break;
                                    case "13":
                                        oData.Col13 = dataFieldValue;
                                        break;
                                    case "14":
                                        oData.Col14 = dataFieldValue;
                                        break;
                                    case "15":
                                        oData.Col15 = dataFieldValue;
                                        break;
                                    case "16":
                                        oData.Col16 = dataFieldValue;
                                        break;
                                    case "17":
                                        oData.Col17 = dataFieldValue;
                                        break;
                                    case "18":
                                        oData.Col18 = dataFieldValue;
                                        break;
                                    case "19":
                                        oData.Col19 = dataFieldValue;
                                        break;
                                    case "20":
                                        oData.Col20 = dataFieldValue;
                                        break;
                                    case "21":
                                        oData.Col21 = dataFieldValue;
                                        break;
                                    case "22":
                                        oData.Col22 = dataFieldValue;
                                        break;
                                    case "23":
                                        oData.Col23 = dataFieldValue;
                                        break;
                                    case "24":
                                        oData.Col24 = dataFieldValue;
                                        break;
                                    case "25":
                                        oData.Col25 = dataFieldValue;
                                        break;
                                    case "26":
                                        oData.Col26 = dataFieldValue;
                                        break;
                                    case "27":
                                        oData.Col27 = dataFieldValue;
                                        break;
                                    case "28":
                                        oData.Col28 = dataFieldValue;
                                        break;
                                    case "29":
                                        oData.Col29 = dataFieldValue;
                                        break;
                                    case "30":
                                        oData.Col30 = dataFieldValue;
                                        break;
                                    case "31":
                                        oData.Col31 = dataFieldValue;
                                        break;
                                    case "32":
                                        oData.Col32 = dataFieldValue;
                                        break;
                                    case "33":
                                        oData.Col33 = dataFieldValue;
                                        break;
                                    case "34":
                                        oData.Col34 = dataFieldValue;
                                        break;
                                    case "35":
                                        oData.Col35 = dataFieldValue;
                                        break;
                                    case "36":
                                        oData.Col36 = dataFieldValue;
                                        break;
                                    case "37":
                                        oData.Col37 = dataFieldValue;
                                        break;
                                    case "38":
                                        oData.Col38 = dataFieldValue;
                                        break;
                                    case "39":
                                        oData.Col39 = dataFieldValue;
                                        break;
                                    case "40":
                                        oData.Col40 = dataFieldValue;
                                        break;
                                    case "41":
                                        oData.Col41 = dataFieldValue;
                                        break;
                                    case "42":
                                        oData.Col42 = dataFieldValue;
                                        break;
                                    case "43":
                                        oData.Col43 = dataFieldValue;
                                        break;
                                    case "44":
                                        oData.Col44 = dataFieldValue;
                                        break;
                                    case "45":
                                        oData.Col45 = dataFieldValue;
                                        break;
                                    case "46":
                                        oData.Col46 = dataFieldValue;
                                        break;
                                    case "47":
                                        oData.Col47 = dataFieldValue;
                                        break;
                                    case "48":
                                        oData.Col48 = dataFieldValue;
                                        break;
                                    case "49":
                                        oData.Col49 = dataFieldValue;
                                        break;
                                    case "50":
                                        oData.Col50 = dataFieldValue;
                                        break;
                                    case "51":
                                        oData.Col51 = dataFieldValue;
                                        break;
                                    case "52":
                                        oData.Col52 = dataFieldValue;
                                        break;
                                    case "53":
                                        oData.Col53 = dataFieldValue;
                                        break;
                                    case "54":
                                        oData.Col54 = dataFieldValue;
                                        break;
                                    case "55":
                                        oData.Col55 = dataFieldValue;
                                        break;
                                    case "56":
                                        oData.Col56 = dataFieldValue;
                                        break;
                                    case "57":
                                        oData.Col57 = dataFieldValue;
                                        break;
                                    case "58":
                                        oData.Col58 = dataFieldValue;
                                        break;
                                    case "59":
                                        oData.Col59 = dataFieldValue;
                                        break;
                                    case "60":
                                        oData.Col60 = dataFieldValue;
                                        break;
                                    case "61":
                                        oData.Col61 = dataFieldValue;
                                        break;
                                    case "62":
                                        oData.Col62 = dataFieldValue;
                                        break;
                                    case "63":
                                        oData.Col63 = dataFieldValue;
                                        break;
                                    case "64":
                                        oData.Col64 = dataFieldValue;
                                        break;
                                    case "65":
                                        oData.Col65 = dataFieldValue;
                                        break;
                                    case "66":
                                        oData.Col66 = dataFieldValue;
                                        break;
                                    case "67":
                                        oData.Col67 = dataFieldValue;
                                        break;
                                    case "68":
                                        oData.Col68 = dataFieldValue;
                                        break;
                                    case "69":
                                        oData.Col69 = dataFieldValue;
                                        break;
                                    case "70":
                                        oData.Col70 = dataFieldValue;
                                        break;
                                    case "71":
                                        oData.Col71 = dataFieldValue;
                                        break;
                                    case "72":
                                        oData.Col72 = dataFieldValue;
                                        break;
                                    case "73":
                                        oData.Col73 = dataFieldValue;
                                        break;
                                    case "74":
                                        oData.Col74 = dataFieldValue;
                                        break;
                                    case "75":
                                        oData.Col75 = dataFieldValue;
                                        break;
                                    case "76":
                                        oData.Col76 = dataFieldValue;
                                        break;
                                    case "77":
                                        oData.Col77 = dataFieldValue;
                                        break;
                                    case "78":
                                        oData.Col78 = dataFieldValue;
                                        break;
                                    case "79":
                                        oData.Col79 = dataFieldValue;
                                        break;
                                    case "80":
                                        oData.Col80 = dataFieldValue;
                                        break;
                                    case "81":
                                        oData.Col81 = dataFieldValue;
                                        break;
                                    case "82":
                                        oData.Col82 = dataFieldValue;
                                        break;
                                    case "83":
                                        oData.Col83 = dataFieldValue;
                                        break;
                                    case "84":
                                        oData.Col84 = dataFieldValue;
                                        break;
                                    case "85":
                                        oData.Col85 = dataFieldValue;
                                        break;
                                    case "86":
                                        oData.Col86 = dataFieldValue;
                                        break;
                                    case "87":
                                        oData.Col87 = dataFieldValue;
                                        break;
                                    case "88":
                                        oData.Col88 = dataFieldValue;
                                        break;
                                    case "89":
                                        oData.Col89 = dataFieldValue;
                                        break;
                                    case "90":
                                        oData.Col90 = dataFieldValue;
                                        break;
                                    case "91":
                                        oData.Col91 = dataFieldValue;
                                        break;
                                    case "92":
                                        oData.Col92 = dataFieldValue;
                                        break;
                                    case "93":
                                        oData.Col93 = dataFieldValue;
                                        break;
                                    case "94":
                                        oData.Col94 = dataFieldValue;
                                        break;
                                    case "95":
                                        oData.Col95 = dataFieldValue;
                                        break;
                                    case "96":
                                        oData.Col96 = dataFieldValue;
                                        break;
                                    case "97":
                                        oData.Col97 = dataFieldValue;
                                        break;
                                    case "98":
                                        oData.Col98 = dataFieldValue;
                                        break;
                                    case "99":
                                        oData.Col99 = dataFieldValue;
                                        break;
                                    case "100":
                                        oData.Col100 = dataFieldValue;
                                        break;
                                    case "101":
                                        oData.Col101 = dataFieldValue;
                                        break;
                                    case "102":
                                        oData.Col102 = dataFieldValue;
                                        break;
                                    case "103":
                                        oData.Col103 = dataFieldValue;
                                        break;
                                    case "104":
                                        oData.Col104 = dataFieldValue;
                                        break;
                                    case "105":
                                        oData.Col105 = dataFieldValue;
                                        break;
                                    case "106":
                                        oData.Col106 = dataFieldValue;
                                        break;
                                    case "107":
                                        oData.Col107 = dataFieldValue;
                                        break;
                                    case "108":
                                        oData.Col108 = dataFieldValue;
                                        break;
                                    case "109":
                                        oData.Col109 = dataFieldValue;
                                        break;
                                    case "110":
                                        oData.Col110 = dataFieldValue;
                                        break;
                                    case "111":
                                        oData.Col111 = dataFieldValue;
                                        break;
                                    case "112":
                                        oData.Col112 = dataFieldValue;
                                        break;
                                    case "113":
                                        oData.Col113 = dataFieldValue;
                                        break;
                                    case "114":
                                        oData.Col114 = dataFieldValue;
                                        break;
                                    case "115":
                                        oData.Col115 = dataFieldValue;
                                        break;
                                    case "116":
                                        oData.Col116 = dataFieldValue;
                                        break;
                                    case "117":
                                        oData.Col117 = dataFieldValue;
                                        break;
                                    case "118":
                                        oData.Col118 = dataFieldValue;
                                        break;
                                    case "119":
                                        oData.Col119 = dataFieldValue;
                                        break;
                                    case "120":
                                        oData.Col120 = dataFieldValue;
                                        break;
                                    case "121":
                                        oData.Col121 = dataFieldValue;
                                        break;
                                    case "122":
                                        oData.Col122 = dataFieldValue;
                                        break;
                                    case "123":
                                        oData.Col123 = dataFieldValue;
                                        break;
                                    case "124":
                                        oData.Col124 = dataFieldValue;
                                        break;
                                    case "125":
                                        oData.Col125 = dataFieldValue;
                                        break;
                                    case "126":
                                        oData.Col126 = dataFieldValue;
                                        break;
                                    case "127":
                                        oData.Col127 = dataFieldValue;
                                        break;
                                    case "128":
                                        oData.Col128 = dataFieldValue;
                                        break;
                                    case "129":
                                        oData.Col129 = dataFieldValue;
                                        break;
                                    case "130":
                                        oData.Col130 = dataFieldValue;
                                        break;
                                    case "131":
                                        oData.Col131 = dataFieldValue;
                                        break;
                                    case "132":
                                        oData.Col132 = dataFieldValue;
                                        break;
                                    case "133":
                                        oData.Col133 = dataFieldValue;
                                        break;
                                    case "134":
                                        oData.Col134 = dataFieldValue;
                                        break;
                                    case "135":
                                        oData.Col135 = dataFieldValue;
                                        break;
                                    case "136":
                                        oData.Col136 = dataFieldValue;
                                        break;
                                    case "137":
                                        oData.Col137 = dataFieldValue;
                                        break;
                                    case "138":
                                        oData.Col138 = dataFieldValue;
                                        break;
                                    case "139":
                                        oData.Col139 = dataFieldValue;
                                        break;
                                    case "140":
                                        oData.Col140 = dataFieldValue;
                                        break;
                                    case "141":
                                        oData.Col141 = dataFieldValue;
                                        break;
                                    case "142":
                                        oData.Col142 = dataFieldValue;
                                        break;
                                    case "143":
                                        oData.Col143 = dataFieldValue;
                                        break;
                                    case "144":
                                        oData.Col144 = dataFieldValue;
                                        break;
                                }
                                #endregion
                            }
                        }
                        lstData.Add(oData);

                        #region PatientInfo
                        lblRegistrationDate.Text = oData.TanggalPemeriksaan;
                        lblPatientName.Text = oData.NamaPegawai;
                        lblAddress.Text = oData.AlamatPegawai;
                        lblDOB.Text = string.Format("{0} / {1}", oData.TempatLahir, oData.TglLahir);
                        lblGender.Text = oData.JnsKelamin;
                        lblMaritialStatus.Text = oData.StatusPerkawinan;
                        lblBusinessPartner.Text = oData.Penjamin;
                        lblPackageMCU.Text = oData.PaketMCU;
                        #endregion

                        #region Riwayat Penyakit Sekarang
                        Col1.Text = oData.Col1;
                        Col2.Text = oData.Col2;
                        #endregion

                        #region Riwayat Penyakit Dahulu
                        Col111.Text = oData.Col111;
                        Col112.Text = oData.Col112;
                        Col113.Text = oData.Col113;
                        Col114.Text = oData.Col114;
                        Col115.Text = oData.Col115;
                        Col116.Text = oData.Col116;
                        Col117.Text = oData.Col117;
                        Col118.Text = oData.Col118;
                        Col119.Text = oData.Col119;
                        Col120.Text = oData.Col120;
                        Col121.Text = oData.Col121;
                        Col122.Text = oData.Col122;
                        Col123.Text = oData.Col123;
                        Col124.Text = oData.Col124;
                        Col125.Text = oData.Col125;
                        Col126.Text = oData.Col126;
                        Col127.Text = oData.Col127;
                        Col128.Text = oData.Col128;
                        Col129.Text = oData.Col129;
                        Col130.Text = oData.Col130;
                        Col131.Text = oData.Col131;
                        Col132.Text = oData.Col132;
                        Col133.Text = oData.Col133;
                        Col134.Text = oData.Col134;
                        Col135.Text = oData.Col135;
                        Col136.Text = oData.Col136;
                        Col137.Text = oData.Col137;
                        Col138.Text = oData.Col138;
                        Col139.Text = oData.Col139;
                        Col140.Text = oData.Col140;
                        Col141.Text = oData.Col141;
                        Col142.Text = oData.Col142;
                        Col143.Text = oData.Col143;
                        Col144.Text = oData.Col144;
                        #endregion

                        #region Khusus Wanita
                        Col5.Text = oData.Col5;
                        Col6.Text = oData.Col6;
                        Col7.Text = oData.Col7;
                        Col8.Text = oData.Col8;
                        Col9.Text = oData.Col9;
                        Col10.Text = oData.Col10;
                        #endregion

                        #region Riwayat Penyakit Keluarga
                        #region Kakek
                        if (oData.Col11 == "Ya")
                        {
                            Col11.Checked = true;
                        }
                        if (oData.Col12 == "Ya")
                        {
                            Col12.Checked = true;
                        }
                        if (oData.Col13 == "Ya")
                        {
                            Col13.Checked = true;
                        }
                        if (oData.Col14 == "Ya")
                        {
                            Col14.Checked = true;
                        }
                        if (oData.Col15 == "Ya")
                        {
                            Col15.Checked = true;
                        }
                        Col16.Text = oData.Col16;
                        #endregion

                        #region Nenek
                        if (oData.Col17 == "Ya")
                        {
                            Col17.Checked = true;
                        }
                        if (oData.Col18 == "Ya")
                        {
                            Col18.Checked = true;
                        }
                        if (oData.Col19 == "Ya")
                        {
                            Col19.Checked = true;
                        }
                        if (oData.Col20 == "Ya")
                        {
                            Col20.Checked = true;
                        }
                        if (oData.Col21 == "Ya")
                        {
                            Col21.Checked = true;
                        }
                        Col22.Text = oData.Col22;
                        #endregion

                        #region Ayah
                        if (oData.Col23 == "Ya")
                        {
                            Col23.Checked = true;
                        }
                        if (oData.Col24 == "Ya")
                        {
                            Col24.Checked = true;
                        }
                        if (oData.Col25 == "Ya")
                        {
                            Col25.Checked = true;
                        }
                        if (oData.Col26 == "Ya")
                        {
                            Col26.Checked = true;
                        }
                        if (oData.Col27 == "Ya")
                        {
                            Col27.Checked = true;
                        }
                        Col28.Text = oData.Col28;
                        #endregion

                        #region Ibu
                        if (oData.Col29 == "Ya")
                        {
                            Col29.Checked = true;
                        }
                        if (oData.Col30 == "Ya")
                        {
                            Col30.Checked = true;
                        }
                        if (oData.Col31 == "Ya")
                        {
                            Col31.Checked = true;
                        }
                        if (oData.Col32 == "Ya")
                        {
                            Col32.Checked = true;
                        }
                        if (oData.Col33 == "Ya")
                        {
                            Col33.Checked = true;
                        }
                        Col34.Text = oData.Col34;
                        #endregion

                        #region Saudara Kandung
                        if (oData.Col35 == "Ya")
                        {
                            Col35.Checked = true;
                        }
                        if (oData.Col36 == "Ya")
                        {
                            Col36.Checked = true;
                        }
                        if (oData.Col37 == "Ya")
                        {
                            Col37.Checked = true;
                        }
                        if (oData.Col38 == "Ya")
                        {
                            Col38.Checked = true;
                        }
                        if (oData.Col39 == "Ya")
                        {
                            Col39.Checked = true;
                        }
                        Col40.Text = oData.Col40;
                        #endregion
                        #endregion

                        #region Kebiasaan
                        #region Olahraga
                        if (oData.Col41 == "Ya")
                        {
                            Col41.Checked = true;
                        }
                        if (oData.Col42 == "Ya")
                        {
                            Col42.Checked = true;
                        }
                        #endregion

                        #region Merokok
                        if (oData.Col43 == "Ya")
                        {
                            Col43.Checked = true;
                        }
                        if (oData.Col44 == "Ya")
                        {
                            Col44.Checked = true;
                        }
                        if (oData.Col45 != "")
                        {
                            Col45.Text = oData.Col45;
                        }
                        else
                        {
                            Col45.Text = "";
                        }
                        #endregion

                        #region Obat-obatan
                        if (oData.Col46 == "Ya")
                        {
                            Col46.Checked = true;
                        }
                        if (oData.Col47 == "Ya")
                        {
                            Col47.Checked = true;
                        }
                        #endregion

                        #region Alkohol
                        if (oData.Col48 == "Ya")
                        {
                            Col48.Checked = true;
                        }
                        if (oData.Col49 == "Ya")
                        {
                            Col49.Checked = true;
                        }
                        if (oData.Col50 != "")
                        {
                            Col50.Text = oData.Col50;
                        }
                        else
                        {
                            Col50.Text = "";
                        }
                        #endregion
                        #endregion

                        #region Imunisasi
                        #region BCG
                        if (oData.Col51 == "Ya")
                        {
                            Col51.Checked = true;
                        }
                        #endregion

                        #region Hepatitis B
                        if (oData.Col52 == "Ya")
                        {
                            Col52.Checked = true;
                        }
                        #endregion

                        #region DFT
                        if (oData.Col53 == "Ya")
                        {
                            Col53.Checked = true;
                        }
                        #endregion

                        #region Campak
                        if (oData.Col54 == "Ya")
                        {
                            Col54.Checked = true;
                        }
                        #endregion

                        #region Polio
                        if (oData.Col55 == "Ya")
                        {
                            Col55.Checked = true;
                        }
                        #endregion

                        #region Lainnya
                        if (oData.Col56 == "Ya")
                        {
                            Col56.Checked = true;
                            if (oData.Col57 != "")
                            {
                                Col57.Text = oData.Col57;
                            }
                            else
                            {
                                Col57.Text = "";
                            }
                        }
                        #endregion
                        #endregion

                        #region Tanda - tanda vital
                        Col58.Text = oData.Col58;
                        Col59.Text = oData.Col59;
                        Col60.Text = oData.Col60;
                        Col61.Text = oData.Col61;
                        Col62.Text = oData.Col62;
                        Col63.Text = oData.Col63;
                        Col64.Text = oData.Col64;
                        #endregion

                        #region Pemeriksaan Fisik
                        #region Kepala
                        if (oData.Col65 == "Tidak Normal")
                        {
                            Col65.Text = string.Format("{0} , {1}", oData.Col65, oData.Col66);
                        }
                        else
                        {
                            Col65.Text = oData.Col65;
                        }
                        #endregion

                        #region Mata
                        if (oData.Col67 == "Tidak Normal")
                        {
                            Col67.Text = string.Format("{0} , {1}", oData.Col67, oData.Col68);
                        }
                        else
                        {
                            Col67.Text = oData.Col67;
                        }
                        #endregion

                        #region THT
                        if (oData.Col69 == "Tidak Normal")
                        {
                            Col69.Text = string.Format("{0} , {1}", oData.Col69, oData.Col70);
                        }
                        else
                        {
                            Col69.Text = oData.Col69;
                        }
                        #endregion
                        
                        #region Leher
                        if (oData.Col71 == "Tidak Normal")
                        {
                            Col71.Text = string.Format("{0} , {1}", oData.Col71, oData.Col72);
                        }
                        else
                        {
                            Col71.Text = oData.Col71;
                        }
                        #endregion

                        #region Mulut
                        if (oData.Col73 == "Tidak Normal")
                        {
                            Col73.Text = string.Format("{0} , {1}", oData.Col73, oData.Col74);
                        }
                        else
                        {
                            Col73.Text = oData.Col73;
                        }
                        #endregion

                        #region Thorax, Paru-paru, Payudara
                        if (oData.Col75 == "Tidak Normal")
                        {
                            Col75.Text = string.Format("{0} , {1}", oData.Col75, oData.Col76);
                        }
                        else
                        {
                            Col75.Text = oData.Col75;
                        }
                        #endregion

                        #region Jantung dan Pembuluh darah
                        if (oData.Col77 == "Tidak Normal")
                        {
                            Col77.Text = string.Format("{0} , {1}", oData.Col77, oData.Col78);
                        }
                        else
                        {
                            Col77.Text = oData.Col77;
                        }
                        #endregion

                        #region Abdomen
                        if (oData.Col79 == "Tidak Normal")
                        {
                            Col79.Text = string.Format("{0} , {1}", oData.Col79, oData.Col80);
                        }
                        else
                        {
                            Col79.Text = oData.Col79;
                        }
                        #endregion

                        #region Kulit dan Sistem Limfatik
                        if (oData.Col81 == "Tidak Normal")
                        {
                            Col81.Text = string.Format("{0} , {1}", oData.Col81, oData.Col82);
                        }
                        else
                        {
                            Col81.Text = oData.Col81;
                        }
                        #endregion

                        #region Tulang belakang & Anggota gerak
                        if (oData.Col83 == "Tidak Normal")
                        {
                            Col83.Text = string.Format("{0} , {1}", oData.Col83, oData.Col84);
                        }
                        else
                        {
                            Col83.Text = oData.Col83;
                        }
                        #endregion

                        #region Sistem Saraf (Neurologi)
                        if (oData.Col85 == "Tidak Normal")
                        {
                            Col85.Text = string.Format("{0} , {1}", oData.Col85, oData.Col86);
                        }
                        else
                        {
                            Col85.Text = oData.Col85;
                        }
                        #endregion

                        #region Genitalia, Anus, dan Rektum
                        if (oData.Col87 == "Tidak Normal")
                        {
                            Col87.Text = string.Format("{0} , {1}", oData.Col87, oData.Col88);
                        }
                        else
                        {
                            Col87.Text = oData.Col87;
                        }
                        #endregion

                        #region Status Lokalis
                        if (oData.Col89 == "Tidak Normal")
                        {
                            Col89.Text = string.Format("{0} , {1}", oData.Col89, oData.Col90);
                        }
                        else
                        {
                            Col89.Text = oData.Col89;
                        }
                        #endregion

                        #region Lain-lain
                        if (oData.Col91 == "Tidak Normal")
                        {
                            Col91.Text = string.Format("{0} , {1}", oData.Col91, oData.Col92);
                        }
                        else
                        {
                            Col91.Text = oData.Col91;
                        }
                        #endregion

                        #region Gambar Diagram
                        PictDiagram.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/BodyDiagram.png");
                        #endregion
                        #endregion

                        #region Pemeriksaan Penunjang

                        #region Pencitraan
                        if (oData.Col93 != " ")
                        {
                            Col93.Text = oData.Col93;
                        }
                        else
                        {
                            Col93.Text = "";
                        }
                        #endregion

                        #region Laboratorium
                        if (oData.Col94 != " ")
                        {
                            Col94.Text = oData.Col94;
                        }
                        else
                        {
                            Col94.Text = "";
                        }
                        #endregion

                        #region EKG
                        if (oData.Col95 != " ")
                        {
                            Col95.Text = oData.Col95;
                        }
                        else
                        {
                            Col95.Text = "";
                        }
                        #endregion

                        #region Treadmill
                        if (oData.Col96 != " ")
                        {
                            Col96.Text = oData.Col96;
                        }
                        else
                        {
                            Col96.Text = "";
                        }
                        #endregion

                        #region Echocardiografi
                        if (oData.Col97 != " ")
                        {
                            Col97.Text = oData.Col97;
                        }
                        else
                        {
                            Col97.Text = "";
                        }
                        #endregion

                        #region USG
                        if (oData.Col98 != " ")
                        {
                            Col98.Text = oData.Col98;
                        }
                        else
                        {
                            Col98.Text = "";
                        }
                        #endregion

                        #region Audiometri
                        if (oData.Col99 != " ")
                        {
                            Col99.Text = oData.Col99;
                        }
                        else
                        {
                            Col99.Text = "";
                        }
                        #endregion

                        #region Spirometri
                        if (oData.Col100 != " ")
                        {
                            Col100.Text = oData.Col100;
                        }
                        else
                        {
                            Col100.Text = "";
                        }
                        #endregion

                        #region Uroflowmetri
                        if (oData.Col101 != " ")
                        {
                            Col101.Text = oData.Col101;
                        }
                        else
                        {
                            Col101.Text = "";
                        }
                        #endregion

                        #region Buta warna
                        if (oData.Col102 != " ")
                        {
                            Col102.Text = oData.Col102;
                        }
                        else
                        {
                            Col102.Text = "";
                        }
                        #endregion

                        #endregion

                        #region Diagnosa
                        if (oData.Col103 != " ")
                        {
                            Col103.Text = oData.Col103;
                        }
                        else
                        {
                            Col103.Text = "";
                        }
                        #endregion

                        #region Kesimpulan

                        #region Sehat/dapat bekerja
                        if (oData.Col104 == "Ya")
                        {
                            Col104.Checked = true;
                        }
                        #endregion

                        #region Sehat/dengan batasan kerja
                        if (oData.Col105 == "Ya")
                        {
                            Col105.Checked = true;
                        }
                        #endregion

                        #region Sehat/dengan beban kerja khusus
                        if (oData.Col106 == "Ya")
                        {
                            Col106.Checked = true;
                        }
                        #endregion

                        #region Sakit/dapat bekerja
                        if (oData.Col107 == "Ya")
                        {
                            Col107.Checked = true;
                        }
                        #endregion

                        #region Sakit/tidak dapat bekerja sementara
                        if (oData.Col108 == "Ya")
                        {
                            Col108.Checked = true;
                        }
                        #endregion

                        #region Sakit/tidak dapat bekerja
                        if (oData.Col109 == "Ya")
                        {
                            Col109.Checked = true;
                        }
                        #endregion

                        #endregion

                        #region Saran

                        if (oData.Col110 != " ")
                        {
                            Col110.Text = oData.Col110;
                        }
                        else
                        {
                            Col110.Text = "";
                        }

                        #endregion

                        #region TandaTangan
                        lblPrintDate.Text = string.Format("{0}, {1}", oHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT));
                        lblParamedicName.Text = oData.ParamedicName;
                        #endregion
            #endregion
                    }
                }
                lstData = null;
            }

            base.InitializeReport(param);


        }
    }
}
