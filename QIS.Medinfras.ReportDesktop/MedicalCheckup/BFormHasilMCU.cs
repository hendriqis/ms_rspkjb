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
    public partial class BFormHasilMCU : BaseCustomDailyPotraitRpt
    {
        public BFormHasilMCU()
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
                                }
                                #endregion
                            }
                        }
                        lstData.Add(oData);

                        #region PatientInfo
                        lblRegisrationNo.Text = oData.RegistrasiNo;
                        lblRegistrationDate.Text = oData.TanggalPemeriksaan;
                        lblPatientName.Text = oData.NamaPegawai;
                        lblPatientNo.Text = oData.NoPegawai;
                        lblBusinessPartner.Text = oData.Penjamin;
                        lblJobLocation.Text = oData.Lokasi;
                        lblDOB.Text = oData.TglLahir;
                        lblGender.Text = string.Format("{0} / {1} Thn", oData.JnsKelamin, ageInYear);
                        lblMaritialStatus.Text = oData.Posisi;
                        lblParamedicTeam.Text = oData.ParamedicName;
                        #endregion

                        lblTigaSatu.Text = oData.Col1;
                        lblTigaDua.Text = oData.Col2;
                        lblTigaTiga.Text = "";

                        lblEmpatSatu.Text = oData.Col3;
                        lblEmpatDua.Text = oData.Col4;
                        lblEmpatTiga.Text = oData.Col5;

                        lblLimaSatu.Text = oData.Col6;
                        lblLimaDua.Text = oData.Col7;

                        lblEnamSatu.Text = oData.Col8;
                        if (lblEnamSatu.Text == "Ya") {
                            lblEnamSatu.Text = string.Format("{0}, {1}", oData.Col8, oData.Col9);
                        }
                        lblEnamDua.Text = oData.Col10;

                        Col11.Text = oData.Col11;

                        if (Col11.Text == "Ya")
                        {
                            Col12.Visible = true; 
                            lbl1Col12.Visible = true;
                            lbl2Col12.Visible = true;
                            lbl3Col12.Visible = true;
                        }
                        else
                        {
                            Col12.Visible = false; 
                            lbl1Col12.Visible = false;
                            lbl2Col12.Visible = false;
                            lbl3Col12.Visible = false;
                        }

                        Col13.Text = oData.Col13;

                        if (Col13.Text == "Ya")
                        {
                            Col14.Visible = true; 
                            lbl1Col14.Visible = true;
                            lbl2Col14.Visible = true;
                            lbl3Col14.Visible = true;
                        }
                        else {
                            Col14.Visible = false; 
                            lbl1Col14.Visible = false;
                            lbl2Col14.Visible = false;
                            lbl3Col14.Visible = false;
                        }

                        Col15.Text = oData.Col16;
                        Col16.Text = oData.Col15;
                        if (Col16.Text == "Ya")
                        {
                            Col15.Visible = true; 
                            lbl1Col15.Visible = true;
                            lbl2Col15.Visible = true;
                            lbl3Col15.Visible = true;
                        }
                        else {
                            Col15.Visible = false; 
                            lbl1Col15.Visible = false;
                            lbl2Col15.Visible = false;
                            lbl3Col15.Visible = false;
                        }
                        Col17.Text = oData.Col17;

                        if (Col17.Text == "Ya")
                        {
                            Col18.Visible = true;
                            lbl1Col18.Visible = true;
                            lbl2Col18.Visible = true;
                            lbl3Col18.Visible = true;
                        }
                        else {
                            Col18.Visible = false;
                            lbl1Col18.Visible = false;
                            lbl2Col18.Visible = false;
                            lbl3Col18.Visible = false;
                        }

                        Col18.Text = oData.Col18;

                        Col14.Text = oData.Col14;
                        Col12.Text = oData.Col12; 

                        if (oData.Col19 == "Ya")
                        {
                            Col19.Text = string.Format("{0} , {1}", oData.Col19, oData.Col20);
                        }
                        else
                        {
                            Col19.Text = oData.Col19;
                        }

                        if (oData.Col21 == "Ya")
                        {
                            Col21.Text = string.Format("{0} , {1}", oData.Col21, oData.Col22);
                        }
                        else
                        {
                            Col21.Text = oData.Col21;
                        }

                        if (oData.Col23 == "Ya")
                        {
                            Col23.Text = string.Format("{0} , {1}", oData.Col23, oData.Col24);
                        }
                        else
                        {
                            Col23.Text = oData.Col23;
                        }

                        if (oData.Col25 == "Ya")
                        {
                            Col25.Text = string.Format("{0} , {1}", oData.Col25, oData.Col26);
                        }
                        else
                        {
                            Col25.Text = oData.Col25;
                        }

                        if (oData.Col27 == "Ya")
                        {
                            Col27.Text = string.Format("{0} , {1}", oData.Col27, oData.Col28);
                        }
                        else
                        {
                            Col27.Text = oData.Col27;
                        }

                        if (oData.Col29 == "Ya")
                        {
                            Col29.Text = string.Format("{0} , {1}", oData.Col29, oData.Col30);
                        }
                        else
                        {
                            Col29.Text = oData.Col29; ;
                        }

                        Col31.Text = oData.Col31;
                        if (Col31.Text == "Ya")
                        {
                            Col32.Text = oData.Col32;
                        }
                        else {
                            Col32.Text = "";
                        }
                       
                        Col33.Text = oData.Col33;
                        if (Col33.Text == "Ya")
                        {
                            Col34.Text = oData.Col34;
                            Col35.Text = oData.Col35;
                        }
                        else {
                            Col34.Text = "";
                            Col35.Text = "";
                        }
                       

                        Col36.Text = oData.Col36;
                        Col37.Text = oData.Col37;
                        Col38.Text = oData.Col38;
                        Col39.Text = oData.Col39;
                        Col40.Text = oData.Col40;

                        if (oData.Col41 == "Abnormal")
                        {
                            Col41.Text = string.Format("{0} , {1}", oData.Col41, oData.Col42);
                        }
                        else
                        {
                            Col41.Text = oData.Col41;
                        }

                        if (oData.Col43 == "Abnormal")
                        {
                            Col43.Text = string.Format("{0} , {1}", oData.Col43, oData.Col44);
                        }
                        else
                        {
                            Col43.Text = oData.Col43;
                        }

                        Col45.Text = oData.Col45;
                        Col46.Text = oData.Col46;
                        Col47.Text = oData.Col47;

                        Col55.Text = oData.Col55;
                        Col56.Text = oData.Col56;
                        Col57.Text = oData.Col57;

                        if (oData.Col48 == "Ya")
                        {
                            Col48.Checked = true;
                        }

                        if (oData.Col49 == "Ya")
                        {
                            Col49.Checked = true;
                        }

                        if (oData.Col50 == "Ya")
                        {
                            Col50.Checked = true;
                        }

                        if (oData.Col51 == "Ya")
                        {
                            Col51.Checked = true;
                        }

                        if (oData.Col58 == "Ya")
                        {
                            Col58.Checked = true;
                        }

                        if (oData.Col59 == "Ya")
                        {
                            Col59.Checked = true;
                        }

                        if (oData.Col60 == "Ya")
                        {
                            Col60.Checked = true;
                        }

                        if (oData.Col61 == "Ya")
                        {
                            Col61.Checked = true;
                        }
                        
                        Col52.Text = oData.Col52;
                        Col62.Text = oData.Col62;
                        Col63.Text = oData.Col63;
                        Col64.Text = oData.Col64;
                        Col65.Text = oData.Col65;
                        Col53.Text = oData.Col53;
                        Col54.Text = oData.Col54;

                        Col66.Text = oData.Col66;
                        Col67.Text = oData.Col67;
                        Col69.Text = oData.Col69;
                        Col70.Text = oData.Col70;

                        Col72.Text = oData.Col72;
                        Col73.Text = oData.Col73;

                        Col68.Text = oData.Col68;
                        Col71.Text = oData.Col71; 

                        if (oData.Col74 == "Tidak Baik")
                        {
                            Col74.Text = string.Format("{0} , {1}", oData.Col74, oData.Col75);
                        }
                        else
                        {
                            Col74.Text = oData.Col74;
                        }

                        if (oData.Col76 == "Ya")
                        {
                            Col76.Checked = true;
                        }

                        if (oData.Col77 == "Ya")
                        {
                            Col77.Checked = true;
                        }

                        if (oData.Col78 == "Ya")
                        {
                            Col78.Checked = true;
                        }

                        if (oData.Col79 == "Ya")
                        {
                            Col79.Checked = true;
                        }

                        if (oData.Col80 == "Ya")
                        {
                            Col80.Checked = true;
                        }

                        if (oData.Col81 == "Ya")
                        {
                            Col81.Checked = true;
                        }

                        if (oData.Col82 == "Tidak Baik")
                        {
                            Col82.Text = string.Format("{0} , {1}", oData.Col82, oData.Col83);
                        }
                        else
                        {
                            Col82.Text = oData.Col82;
                        }

                        if (oData.Col84 == "Ya")
                        {
                            Col84.Checked = true;
                        }

                        if (oData.Col85 == "Ya")
                        {
                            Col85.Checked = true;
                        }

                        if (oData.Col86 == "Ya")
                        {
                            Col86.Checked = true;
                        }

                        if (oData.Col87 == "Ya")
                        {
                            Col87.Checked = true;
                        }

                        if (oData.Col88 == "Ya")
                        {
                            Col88.Checked = true;
                        }

                        if (oData.Col89 == "Ya")
                        {
                            Col89.Checked = true;
                        }

                        if (oData.Col90 == "Abnormal")
                        {
                            Col90.Text = string.Format("{0} , {1}", oData.Col90, oData.Col91);
                        }
                        else
                        {
                            Col90.Text = oData.Col90;
                        }

                        if (oData.Col92 == "Hypertropi")
                        {
                            Col92.Text = string.Format("{0} , {1}", oData.Col92, oData.Col93);
                        }
                        else
                        {
                            Col92.Text = oData.Col92;
                        }

                        if (oData.Col94 == "Abnormal")
                        {
                            Col94.Text = string.Format("{0} , {1}", oData.Col94, oData.Col95);
                        }
                        else
                        {
                            Col94.Text = oData.Col94;
                        }


                        if (oData.Col187 == "Ya") {
                            Col187.Checked = true; 
                        }
                        if (oData.Col188 == "Ya")
                        {
                            Col188.Checked = true;
                        }
                        if (oData.Col189 == "Ya")
                        {
                            Col189.Checked = true;
                        }
                        if (oData.Col190 == "Ya")
                        {
                            Col190.Checked = true;
                        }
                        if (oData.Col191 == "Ya")
                        {
                            Col191.Checked = true;
                        }
                        if (oData.Col192 == "Ya")
                        {
                            Col192.Checked = true;
                        }
                        if (oData.Col193 == "Ya")
                        {
                            Col193.Checked = true;
                        }
                        if (oData.Col194 == "Ya")
                        {
                            Col194.Checked = true;
                        }
                        if (oData.Col195 == "Ya")
                        {
                            Col195.Checked = true;
                        }
                        if (oData.Col196 == "Ya")
                        {
                            Col196.Checked = true;
                        }
                        if (oData.Col197 == "Ya")
                        {
                            Col197.Checked = true;
                        }
                        if (oData.Col198 == "Ya")
                        {
                            Col198.Checked = true;
                        }
                        if (oData.Col199 == "Ya")
                        {
                            Col199.Checked = true;
                        }
                        if (oData.Col200 == "Ya")
                        {
                            Col200.Checked = true;
                        }
                        if (oData.Col201 == "Ya")
                        {
                            Col201.Checked = true;
                        }
                        if (oData.Col202 == "Ya")
                        {
                            Col202.Checked = true;
                        }

                        Col96.Text = oData.Col96;
                        Col97.Text = oData.Col97;
                        Col98.Text = oData.Col98;
                        if (Col98 .Text== "Irreguler") {
                            Col98.Text = string.Format("{0} , {1}", Col98.Text , oData.Col99);
                        }
                        Col100.Text = oData.Col100;
                        Col101.Text = oData.Col101;
                        Col102.Text = oData.Col102;
                        Col103.Text = oData.Col103;

                        Col104.Text = oData.Col104;
                        Col105.Text = string.Format("{0} /menit", oData.Col105);
                        Col106.Text = oData.Col106;
                        Col107.Text = oData.Col107;
                        Col108.Text = oData.Col108;
                        Col109.Text = oData.Col109;
                        Col110.Text = oData.Col110;
                        Col111.Text = oData.Col111;
                       
                        Col112.Text = oData.Col112;
                        Col113.Text = oData.Col113;
                        Col114.Text = oData.Col114;
                        Col115.Text = oData.Col115;
                        Col116.Text = oData.Col116;
                        Col117.Text = oData.Col117;

                        if (oData.Col118 == "Abnormal")
                        {
                            Col118.Text = string.Format("{0} , {1}", oData.Col118, oData.Col119);
                        }
                        else
                        {
                            Col118.Text = oData.Col118;
                        }

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
                        if (Col129.Text == "Abnormal") {
                            Col129.Text = string.Format("{0}, {1}", Col129.Text, oData.Col130);
                        }
                        if (oData.Col132 == "Abnormal")
                        {
                            Col132.Text = string.Format("{0} , {1}", oData.Col132, oData.Col133);
                        }
                        else
                        {
                            Col132.Text = oData.Col132;
                        }

                        if (oData.Col134 == "Abnormal")
                        {
                            Col134.Text = string.Format("{0} , {1}", oData.Col134, oData.Col135);
                        }
                        else
                        {
                            Col134.Text = oData.Col134;
                        }

                        if (oData.Col136 == "Abnormal")
                        {
                            Col136.Text = string.Format("{0} , {1}", oData.Col136, oData.Col137);
                        }
                        else
                        {
                            Col136.Text = oData.Col136;
                        }                       

                        Col138.Text = oData.Col138;
                        Col139.Text = oData.Col139;
                        Col140.Text = oData.Col140;

                        Col141.Text = oData.Col141;
                        Col142.Text = oData.Col142;

                        if (oData.Col143 == "Abnormal")
                        {
                            Col143.Text = string.Format("{0} , {1}", oData.Col143, oData.Col144);
                        }
                        else
                        {
                            Col143.Text = oData.Col143;
                        }

                        if (oData.Col147 == "Abnormal")
                        {
                            Col147.Text = string.Format("{0} , {1}", oData.Col147, oData.Col148);
                        }
                        else
                        {
                            Col147.Text = oData.Col147;
                        }
                        
                        
                        Col149.Text = oData.Col149;
                        Col157.Text = oData.Col157;
                        Col145.Text = oData.Col145;

                        Col150.Text = oData.Col150;
                        Col151.Text = oData.Col151;
                        Col161.Text = oData.Col161;
                        Col159.Text = oData.Col159;

                        #region MCU Result
                        subMCUResult.CanGrow = true;
                        bFormHasilMCUDt.InitializeReport(oData);
                        #endregion

                        #region TandaTangan
                        lblPrintDate.Text = string.Format("{0}, {1}", oHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));
                        lblParamedicName.Text = oData.ParamedicName;
                        #endregion
                    }
                }
                lstData = null;
            }
            
            base.InitializeReport(param);

            lblReportProperties.Text = string.Format("{0}", cv.PatientName);
            #endregion
        }
    }
}
