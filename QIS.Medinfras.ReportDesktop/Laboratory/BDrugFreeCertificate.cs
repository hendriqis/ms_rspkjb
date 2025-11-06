using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Text.RegularExpressions;
using System.Text;
using System.Linq;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BDrugFreeCertificate : BaseDailyPortraitRpt
    {
        StringBuilder sbFooterNote = new StringBuilder();

        public BDrugFreeCertificate()
        {
            InitializeComponent();
        }

        public static string ToRoman(int number)
        {
            if ((number < 0) || (number > 3999)) throw new ArgumentOutOfRangeException("Value must be between 1 and 3999");
            if (number < 1) return string.Empty;
            if (number >= 1000) return "M" + ToRoman(number - 1000);
            if (number >= 900) return "CM" + ToRoman(number - 900); //EDIT: i've typed 400 instead 900
            if (number >= 500) return "D" + ToRoman(number - 500);
            if (number >= 400) return "CD" + ToRoman(number - 400);
            if (number >= 100) return "C" + ToRoman(number - 100);
            if (number >= 90) return "XC" + ToRoman(number - 90);
            if (number >= 50) return "L" + ToRoman(number - 50);
            if (number >= 40) return "XL" + ToRoman(number - 40);
            if (number >= 10) return "X" + ToRoman(number - 10);
            if (number >= 9) return "IX" + ToRoman(number - 9);
            if (number >= 5) return "V" + ToRoman(number - 5);
            if (number >= 4) return "IV" + ToRoman(number - 4);
            if (number >= 1) return "I" + ToRoman(number - 1);
            throw new ArgumentOutOfRangeException("Value must be between 1 and 3999");
        }

        public override void InitializeReport(string[] param)
        {
            vConsultVisit entityCV = BusinessLayer.GetvConsultVisitList(string.Format(param[0]))[0];
            vVitalSignDt entityWeight = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID = {0} AND VitalSignLabel = 'WEIGHT' AND IsDeleted = 0", entityCV.VisitID.ToString())).FirstOrDefault();
            vVitalSignDt entityHeight = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID = {0} AND VitalSignLabel = 'HEIGHT' AND IsDeleted = 0", entityCV.VisitID.ToString())).FirstOrDefault();
            LaboratoryResultHd entityHd = BusinessLayer.GetLaboratoryResultHdList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID))[0];
            string toNoteRemarks = param[1];
            string toRemarks = param[2];

            if (entityWeight == null || entityHeight == null)
            {
                lblBBTB.Text = string.Format("-");
            }
            else
            {
                lblBBTB.Text = string.Format("{0} Kg / {1} Cm", entityWeight.VitalSignValue, entityHeight.VitalSignValue);
            }

            xrAmpNone.Checked = true;
            xrMetNone.Checked = true;
            xrMorNone.Checked = true;
            xrMarNone.Checked = true;
            xrBenNone.Checked = true;
            xrKokNone.Checked = true;

            string filterExpression = string.Format("VisitID = {0}", entityHd.VisitID.ToString());
            List<vLaboratoryResultDt> lstDetail = BusinessLayer.GetvLaboratoryResultDtList(filterExpression);
            foreach (vLaboratoryResultDt fraction in lstDetail)
            {
                if (lstDetail.Count > 0)
                {

                    if (fraction.FractionCode == "AMPHE")
                    {
                        if (fraction.IsNormal == true)
                        {
                            xrAmpPositif.Checked = true;
                            xrAmpNegatif.Checked = false;
                            xrAmpNone.Checked = false;
                        }
                        else
                        {
                            xrAmpPositif.Checked = false;
                            xrAmpNegatif.Checked = true;
                            xrAmpNone.Checked = false;
                        }
                    }
                    else if (fraction.FractionCode == "METHAM")
                    {
                        if (fraction.IsNormal == true)
                        {
                            xrMetPositif.Checked = true;
                            xrMetNegatif.Checked = false;
                            xrMetNone.Checked = false;
                        }
                        else
                        {
                            xrMetPositif.Checked = false;
                            xrMetNegatif.Checked = true;
                            xrMetNone.Checked = false;
                        }
                    }
                    else if (fraction.FractionCode == "MORFIN")
                    {
                        if (fraction.IsNormal == true)
                        {
                            xrMorPositif.Checked = true;
                            xrMorNegatif.Checked = false;
                            xrMorNone.Checked = false;
                        }
                        else
                        {
                            xrMorPositif.Checked = false;
                            xrMorNegatif.Checked = true;
                            xrMorNone.Checked = false;
                        }
                    }
                    else if (fraction.FractionCode == "MARIYU")
                    {
                        if (fraction.IsNormal == true)
                        {
                            xrMarPositif.Checked = true;
                            xrMarNegatif.Checked = false;
                            xrMarNone.Checked = false;
                        }
                        else
                        {
                            xrMarPositif.Checked = false;
                            xrMarNegatif.Checked = true;
                            xrMarNone.Checked = false;
                        }
                    }
                    else if (fraction.FractionCode == "BENZO")
                    {
                        if (fraction.IsNormal == true)
                        {
                            xrBenPositif.Checked = true;
                            xrBenNegatif.Checked = false;
                            xrBenNone.Checked = false;
                        }
                        else
                        {
                            xrBenPositif.Checked = false;
                            xrBenNegatif.Checked = true;
                            xrBenNone.Checked = false;
                        }
                    }
                    else if (fraction.FractionCode == "COCAIN")
                    {
                        if (fraction.IsNormal == true)
                        {
                            xrKokPositif.Checked = true;
                            xrKokNegatif.Checked = false;
                            xrKokNone.Checked = false;
                        }
                        else
                        {
                            xrKokPositif.Checked = false;
                            xrKokNegatif.Checked = true;
                            xrKokNone.Checked = false;
                        }
                    }
                }
            }

            lblGender.Text = entityCV.Gender;
            lblTglLahir.Text = entityCV.DateOfBirthInString;
            lblPatientName.Text = entityCV.PatientName;
            lblPrintDate.Text = string.Format("Jakarta, {0}", DateTime.Now.ToString("dd-MMMM-yyyy"));
            lblText.Text = string.Format("Telah dilaksanakan pemeriksaan Narkoba / NAPZA terhadap yang bersangkutan pada hari : {0}", DateTime.Now.ToString("dddd", new System.Globalization.CultureInfo("id-ID")));
            lblText2.Text = string.Format("Tanggal : {0} dengan metode Drugs Urine Screening Test, dengan hasil sebagai", entityHd.ResultDate.ToString("dd-MMMM-yyyy"));
            lblStreet.Text = entityCV.StreetName;
            base.InitializeReport(param);
            lblNotesRemarks.Text = toNoteRemarks;
            lblRemarks.Text = String.Format("Surat keterangan ini dibuat untuk {0}", toRemarks);
            lblParamedicDPJP.Text = entityCV.ParamedicName;
            lblParamedicLab.Text = string.Format("Yang bertanda tangan dibawah ini a.n {0} ", entityCV.ParamedicName);

            #region Nomor Surat Kematian
            SettingParameterDt entitySetPar = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, Constant.SettingParameter.RM_IS_PREFIK_SURAT_KETERANGAN_BEBAS_NARKOBA);
            SettingParameterDt entitySetName = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, Constant.SettingParameter.RM_IS_NAMA_RS_SURAT_KETERANGAN_BEBAS_NARKOBA);
            List<vReportPrintLog> entityReportLog = BusinessLayer.GetvReportPrintLogList(string.Format("ReportCode = '{0}' AND PrintedDate = '{1}'", reportMaster.ReportCode, DateTime.Now.ToString()));

            Int32 countReportLog = entityReportLog.Count;
            String countReport = countReportLog.ToString("D3");

            String date = DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112);
            String roman = ToRoman(DateTime.Now.Month);
            String year = DateTime.Now.ToString(Constant.FormatString.YEAR_FORMAT);

            String noSuratNarkoba = string.Format("{0}/{1}/{2}/{3}/{4}", entitySetPar.ParameterValue, countReport, entitySetName.ParameterValue, roman, year);
            lblSuratNarkoba.Text = string.Format("No : {0}", noSuratNarkoba);
            #endregion

        }
    }
}
