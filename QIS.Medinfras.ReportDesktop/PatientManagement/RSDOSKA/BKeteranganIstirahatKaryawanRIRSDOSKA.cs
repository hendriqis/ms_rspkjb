using System;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BKeteranganIstirahatKaryawanRIRSDOSKA : BaseDailyPortraitRpt
    {
        public BKeteranganIstirahatKaryawanRIRSDOSKA()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();
            vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", param[0])).FirstOrDefault();
            ConsultVisit entityCV = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", entityReg.RegistrationID))[0];
            ParamedicMaster entityPM = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", entityCV.ParamedicID))[0];

            String DischargeDate = "";
            if (entityReg.DischargeDate.ToString(Constant.FormatString.DATE_FORMAT) != "01-Jan-1900")
            {
                DischargeDate = entityReg.DischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            }
            else
            {
                if (entityReg.PhysicianDischargedDate.ToString(Constant.FormatString.DATE_FORMAT) != "01-Jan-1900")
                {
                    DischargeDate = entityReg.PhysicianDischargedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                }
                else
                {
                    if (entityReg.PlanDischargeDate.ToString(Constant.FormatString.DATE_FORMAT) != "01-Jan-1900")
                    {
                        DischargeDate = entityReg.PlanDischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    }
                    else
                    {
                        DischargeDate = "";
                    }
                }
            }

            String NIK = param[4];
            String Unit = param[5];
            String CountDate = "";

            if (DischargeDate == null || DischargeDate == "")
            {
                CountDate = string.Format("Betul-betul dirawat karena sakit/partus/abortus/kecelakaan kerja (*) mulai tanggal {0} dan perlu istirahat di rumah selama {1} hari, mulai tanggal {2} s/d tanggal {3}.", entityReg.ActualVisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT), param[3], param[1], param[2]);
            }
            else
            {
                CountDate = string.Format("Betul-betul dirawat karena sakit/partus/abortus/kecelakaan kerja (*) mulai tanggal {0} s/d {1} dan perlu istirahat di rumah selama {2} hari, mulai tanggal {3} s/d tanggal {4}.", entityReg.ActualVisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT), DischargeDate, param[3], param[1], param[2]);
            }

            lblHeader.Text = string.Format("Yang bertanda tangan di bawah ini, Dokter di {0},", entityHealthcare.HealthcareName);
            lblName.Text = entityReg.PatientName;
            lblNIK.Text = NIK;
            lblUnit.Text = Unit;
            lblIstirahat.Text = CountDate;
            lblPrintDate.Text = string.Format("{0}, {1}", entityHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT));
            lblParamedicName.Text = entityPM.FullName;
            base.InitializeReport(param);

        }
    }
}
