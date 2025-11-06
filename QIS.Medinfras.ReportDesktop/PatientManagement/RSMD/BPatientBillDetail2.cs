using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using System.Linq;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BPatientBillDetail2 : BaseDailyPortraitRpt
    {
        vRegistration entity = null;

        public BPatientBillDetail2()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            List<vRegistration> lstRegistration = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID IN ({0})", param[0]));
            
            int countIP = 0;
            foreach (vRegistration regTemp in lstRegistration)
            {
                if (regTemp.DepartmentID == Constant.Facility.INPATIENT)
                {
                    countIP += 1;
                }
            }
            if (countIP == 0)
            {
                entity = lstRegistration.FirstOrDefault();
            }
            else
            {
                entity = lstRegistration.FirstOrDefault(p => p.DepartmentID == Constant.Facility.INPATIENT);
            }

            lblNoReg.Text = string.Format("{0}/{1}", entity.RegistrationNo,entity.MedicalNo);
            lblTanggalMasuk.Text = entity.ActualVisitDate.ToString(Constant.FormatString.DATE_FORMAT);
            lblPatient.Text = entity.PatientName;
            lblTglLahir.Text = entity.DateOfBirthInString;
            lblDoctor.Text = entity.ParamedicName;
            lblRuang.Text = entity.ServiceUnitName;

            DateTime DischargeDate = lstRegistration.FirstOrDefault().DischargeDate;
            String DischargeDateInString = DischargeDate.ToString(Constant.FormatString.DATE_FORMAT);

            if (DischargeDateInString == "01-Jan-1900")
            {
                lblTanggalKeluar.Text = "-";
            }
            else
            {
                lblTanggalKeluar.Text = DischargeDateInString;
            }

            lblKelas.Text = entity.ClassName;
            lblPenjamin.Text = entity.CustomerType;
            lblInstansi.Text = entity.BusinessPartnerName;

            base.InitializeReport(param);
        }
    }
}
