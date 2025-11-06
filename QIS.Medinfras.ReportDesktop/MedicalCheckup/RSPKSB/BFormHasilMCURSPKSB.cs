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
    public partial class BFormHasilMCURSPKSB : BaseCustomDailyPotraitRpt
    {
        public BFormHasilMCURSPKSB()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            vConsultVisit cv = BusinessLayer.GetvConsultVisitList(param[0].ToString()).FirstOrDefault();
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID)).FirstOrDefault();

            xrPictureBox1.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/logo.png");

            lblReportTitle.Visible = false;
            lblReportSubTitle.Visible = false;

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
                        oData.TanggalPemeriksaan = oData.TglMasuk = row.RegistrationDate.ToString(Constant.FormatString.DATE_FORMAT_1);

                        if (row.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
                        {
                            oData.TglLahir = row.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT_1);
                            ageInYear = Function.GetPatientAgeInYear(row.DateOfBirth, DateTime.Now);
                        }
                        else
                        {
                            oData.TglLahir = "";
                        }

                        oData.JnsKelamin = row.Gender;
                        oData.Lokasi = row.CorporateAccountDepartment;
                        oData.NamaPegawai = row.PatientName;
                        oData.NoPegawai = row.CorporateAccountNo;
                        oData.ParamedicCode = row.ParamedicCode;
                        oData.ParamedicName = row.ParamedicName;
                        oData.Penjamin = row.BusinessPartnerName;
                        oData.Posisi = row.CorporateAccountDepartment;

                        for (int i = 0; i < data.Length; i++)
                        {

                            string[] field = data[i].Split('^');
                            if (field.Length > 1)
                            {
                                string[] dataField = field[1].Split('=');
                                string dataFieldValue = dataField[1];
                                if (dataField.Length > 2)
                                {
                                    for(int indexField= 0; indexField < dataField.Length; indexField++)
                                    {
                                        if(i > 0)
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
                                    case "145":
                                        oData.Col145 = dataFieldValue;
                                        break;
                                    case "146":
                                        oData.Col146 = dataFieldValue;
                                        break;
                                    case "147":
                                        oData.Col147 = dataFieldValue;
                                        break;
                                    case "148":
                                        oData.Col148 = dataFieldValue;
                                        break;
                                    case "149":
                                        oData.Col149 = dataFieldValue;
                                        break;
                                    case "150":
                                        oData.Col150 = dataFieldValue;
                                        break;
                                    case "151":
                                        oData.Col151 = dataFieldValue;
                                        break;
                                    case "152":
                                        oData.Col152 = dataFieldValue;
                                        break;
                                    case "153":
                                        oData.Col153 = dataFieldValue;
                                        break;
                                    case "154":
                                        oData.Col154 = dataFieldValue;
                                        break;
                                    case "155":
                                        oData.Col155 = dataFieldValue;
                                        break;
                                    case "156":
                                        oData.Col156 = dataFieldValue;
                                        break;
                                    case "157":
                                        oData.Col157 = dataFieldValue;
                                        break;
                                    case "158":
                                        oData.Col158 = dataFieldValue;
                                        break;
                                    case "159":
                                        oData.Col159 = dataFieldValue;
                                        break;
                                    case "160":
                                        oData.Col160 = dataFieldValue;
                                        break;
                                    case "161":
                                        oData.Col161 = dataFieldValue;
                                        break;
                                    case "162":
                                        oData.Col162 = dataFieldValue;
                                        break;
                                    case "163":
                                        oData.Col163 = dataFieldValue;
                                        break;
                                    case "164":
                                        oData.Col164 = dataFieldValue;
                                        break;
                                    case "165":
                                        oData.Col165 = dataFieldValue;
                                        break;
                                    case "166":
                                        oData.Col166 = dataFieldValue;
                                        break;
                                    case "167":
                                        oData.Col168 = dataFieldValue;
                                        break;
                                    case "168":
                                        oData.Col168 = dataFieldValue;
                                        break;
                                    case "169":
                                        oData.Col169 = dataFieldValue;
                                        break;
                                    case "170":
                                        oData.Col170 = dataFieldValue;
                                        break;
                                    case "171":
                                        oData.Col171 = dataFieldValue;
                                        break;
                                    case "172":
                                        oData.Col172 = dataFieldValue;
                                        break;
                                    case "173":
                                        oData.Col173 = dataFieldValue;
                                        break;
                                    case "174":
                                        oData.Col174 = dataFieldValue;
                                        break;
                                    case "175":
                                        oData.Col175 = dataFieldValue;
                                        break;
                                    case "176":
                                        oData.Col176 = dataFieldValue;
                                        break;
                                    case "177":
                                        oData.Col177 = dataFieldValue;
                                        break;
                                    case "178":
                                        oData.Col178 = dataFieldValue;
                                        break;
                                    case "179":
                                        oData.Col179 = dataFieldValue;
                                        break;
                                    case "180":
                                        oData.Col180 = dataFieldValue;
                                        break;
                                    case "181":
                                        oData.Col181 = dataFieldValue;
                                        break;
                                    case "182":
                                        oData.Col182 = dataFieldValue;
                                        break;
                                    case "183":
                                        oData.Col183 = dataFieldValue;
                                        break;
                                    case "184":
                                        oData.Col184 = dataFieldValue;
                                        break;
                                    case "185":
                                        oData.Col185 = dataFieldValue;
                                        break;
                                    case "186":
                                        oData.Col186 = dataFieldValue;
                                        break;
                                    case "187":
                                        oData.Col187 = dataFieldValue;
                                        break;
                                    case "188":
                                        oData.Col188 = dataFieldValue;
                                        break;
                                    case "189":
                                        oData.Col189 = dataFieldValue;
                                        break;
                                    case "190":
                                        oData.Col190 = dataFieldValue;
                                        break;
                                    case "191":
                                        oData.Col191 = dataFieldValue;
                                        break;
                                    case "192":
                                        oData.Col192 = dataFieldValue;
                                        break;
                                    case "193":
                                        oData.Col193 = dataFieldValue;
                                        break;
                                    case "194":
                                        oData.Col194 = dataFieldValue;
                                        break;
                                    case "195":
                                        oData.Col195 = dataFieldValue;
                                        break;
                                    case "196":
                                        oData.Col196 = dataFieldValue;
                                        break;
                                    case "197":
                                        oData.Col197 = dataFieldValue;
                                        break;
                                    case "198":
                                        oData.Col198 = dataFieldValue;
                                        break;
                                    case "199":
                                        oData.Col199 = dataFieldValue;
                                        break;
                                    case "200":
                                        oData.Col200 = dataFieldValue;
                                        break;
                                    case "201":
                                        oData.Col201 = dataFieldValue;
                                        break;
                                    case "202":
                                        oData.Col202 = dataFieldValue;
                                        break;
                                    case "203":
                                        oData.Col203 = dataFieldValue;
                                        break;
                                    case "204":
                                        oData.Col204 = dataFieldValue;
                                        break;
                                    case "205":
                                        oData.Col205 = dataFieldValue;
                                        break;
                                    case "206":
                                        oData.Col206 = dataFieldValue;
                                        break;
                                    case "207":
                                        oData.Col207 = dataFieldValue;
                                        break;
                                    case "208":
                                        oData.Col208 = dataFieldValue;
                                        break;
                                    case "209":
                                        oData.Col209 = dataFieldValue;
                                        break;
                                    case "210":
                                        oData.Col210 = dataFieldValue;
                                        break;
                                    case "211":
                                        oData.Col211 = dataFieldValue;
                                        break;
                                    case "212":
                                        oData.Col212 = dataFieldValue;
                                        break;
                                    case "213":
                                        oData.Col213 = dataFieldValue;
                                        break;
                                    case "214":
                                        oData.Col214 = dataFieldValue;
                                        break;
                                    case "215":
                                        oData.Col215 = dataFieldValue;
                                        break;
                                    case "254":
                                        oData.Col254 = dataFieldValue;
                                        break;
                                    case "255":
                                        oData.Col255 = dataFieldValue;
                                        break;
                                    case "256":
                                        oData.Col256 = dataFieldValue;
                                        break;
                                    case "257":
                                        oData.Col257 = dataFieldValue;
                                        break;
                                    case "258":
                                        oData.Col258 = dataFieldValue;
                                        break;
                                    case "259":
                                        oData.Col259 = dataFieldValue;
                                        break;
                                }
                                #endregion
                            }
                        }
                        lstData.Add(oData);

                        #region PatientInfo
                        lblRegisrationNo.Text = oData.RegistrasiNo;
                        lblRegistrationDate.Text = oData.TanggalPemeriksaan;
                        lblPatientName.Text = oData.NamaPegawai;
                        lblPatientNamee.Text = oData.NamaPegawai;
                        lblJobLocation.Text = oData.Lokasi;
                        lblDOB.Text = oData.TglLahir;
                        lblGender.Text = string.Format("{0} / {1} Thn", oData.JnsKelamin, ageInYear);
                        lblParamedicTeam.Text = oData.ParamedicName;

                        lblPatientName1.Text = string.Format("({0})", oData.NamaPegawai);
                        lblPatientName2.Text = string.Format("({0})", oData.NamaPegawai);
                        #endregion

                        //Jenis Pemeriksaan
                        if (oData.Col1 == "Ya")
                        {
                            Col1.Checked = true;
                        }

                        if (oData.Col2 == "Ya")
                        {
                            Col2.Checked = true;
                        }

                        if (oData.Col3 == "Ya")
                        {
                            Col3.Checked = true;
                        }

                        if (oData.Col4 == "Ya")
                        {
                            Col4.Checked = true;
                        }

                        if (oData.Col5 == "Ya")
                        {
                            Col5.Checked = true;
                        }

                        if (oData.Col6 == "Ya")
                        {
                            Col6.Checked = true;
                        }


                        //ANAMNESA
                        lblKeluhan.Text = string.Format("{0}, {1}", oData.Col7, oData.Col8);
                        lblRiwayatPenyakit.Text = string.Format("{0}, {1}", oData.Col9, oData.Col10);
                        lblPenyakitFam.Text = string.Format("{0}, {1}", oData.Col11, oData.Col12);
                        lblLimaSatu.Text = string.Format("{0}, {1}", oData.Col13, oData.Col14);

                        //KEBIASAAN
                        Col15.Text = oData.Col15;
                        Col16.Text = oData.Col16;
                        Col17.Text = oData.Col17;
                        Col18.Text = oData.Col18;
                        Col19.Text = oData.Col19;
                        Col20.Text = oData.Col20;
                        Col21.Text = oData.Col21;
                        Col22.Text = oData.Col22;

                        //PEMERIKSAAN FISIK
                        Col23.Text = oData.Col23;
                        Col24.Text = oData.Col24;
                        Col25.Text = oData.Col25;
                        Col26.Text = oData.Col26;
                        Col27.Text = oData.Col27;
                        Col28.Text = oData.Col28;
                        Col29.Text = oData.Col29;
                        Col30.Text = string.Format("{0}, {1}", oData.Col30, oData.Col31);
                        Col32.Text = string.Format("{0}, {1}", oData.Col32, oData.Col33);
                        Col34.Text = oData.Col34;
                        Col35.Text = oData.Col35;
                        Col36.Text = oData.Col36;
                        Col37.Text = oData.Col37;
                        Col38.Text = string.Format("{0}, {1}", oData.Col38, oData.Col39);
                        Col40.Text = oData.Col40;
                        Col41.Text = string.Format("{0}, {1}", oData.Col41, oData.Col42);
                        Col43.Text = oData.Col43;
                        Col44.Text = oData.Col44;
                        Col45.Text = oData.Col45;
                        Col46.Text = string.Format("{0}, {1}", oData.Col46, oData.Col48);
                        Col47.Text = string.Format("{0}, {1}", oData.Col47, oData.Col49);
                        Col50.Text = oData.Col50;
                        Col51.Text = oData.Col51;
                        Col52.Text = oData.Col52;
                        Col53.Text = string.Format("{0}, {1}", oData.Col41, oData.Col42);
                        Col53.Text = oData.Col53;
                        Col57.Text = string.Format("{0}, {1}", oData.Col57, oData.Col58);
                        Col65.Text = oData.Col65;
                        Col66.Text = string.Format("{0}, {1}", oData.Col66, oData.Col67);
                        Col68.Text = string.Format("{0}, {1}", oData.Col68, oData.Col69);
                        Col70.Text = oData.Col70;

                        //KESIMPULAN
                        Col71.Text = oData.Col71;
                        Col72.Text = oData.Col72;
                        Col73.Text = string.Format("{0}, {1}", oData.Col73, oData.Col74);
                        Col75.Text = string.Format("{0}, {1}", oData.Col75, oData.Col76);
                        Col77.Text = string.Format("{0}, {1}", oData.Col77, oData.Col78);
                        Col79.Text = string.Format("{0}, {1}", oData.Col79, oData.Col80);
                        Col81.Text = oData.Col81;
                        Col82.Text = oData.Col82;
                        Col88.Text = oData.Col88;

                        #region MCU Result
                        //subMCUResult.CanGrow = true;
                        //bFormHasilMCUDt.InitializeReport(oData);
                        #endregion

                        #region TandaTangan
                        lblPrintDate.Text = string.Format("{0}, {1}", oHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT));
                        lblParamedicName.Text = oData.ParamedicName;
                        #endregion
                    }
                }
                lstData = null;
            }
            
            base.InitializeReport(param);

            
            #endregion

            //#region Hasil Radiologi

            //subImagingResult.CanGrow = true;
            //NewMCUImagingResultRSPKSB.InitializeReport(cv.VisitID);

            //#endregion
            
            #region Hasil Diagnostic
            subDiagnosticResult.CanGrow = true;
            NewMCUDiagnosticResultRSPKSB.InitializeReport(cv.VisitID);
            #endregion
        }
    }
}
