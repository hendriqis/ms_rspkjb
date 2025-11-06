using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BTransaksi3 : BaseDailyPortrait2Rpt
    {
        public BTransaksi3()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();
            vPatientChargesHd entity = BusinessLayer.GetvPatientChargesHdList(string.Format("RegistrationID = {0}", param[0])).FirstOrDefault();
            vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", param[0])).FirstOrDefault();

            if (entityReg.DepartmentID == Constant.Facility.OUTPATIENT)
            {
                lblQueueNo.Text = Convert.ToString(entityReg.QueueNo);
            }
            else
            {
                lblQueueNo.Visible = false;
            }

            cParamedicName.Text = entityReg.ParamedicName;
            cBusinessPartnerName.Text = entityReg.BusinessPartnerName;
            cTransactionDate.Text = entity.TransactionDateInString;
            cVisitServiceUnitName.Text = string.Format("{0}", entityReg.ServiceUnitName);
            cServiceUnitName.Text = entity.ServiceUnitName;
            cDateOfBirth.Text = string.Format("{0} / {1} Tahun / {2}", entityReg.DateOfBirthInString, entityReg.PatientAgeInYear, entityReg.cfGenderInitial);
                        
            if (entityReg.DepartmentID == Constant.Facility.INPATIENT)
                lblServiceUnit.Text = "RUANG";
            else if (entityReg.DepartmentID == Constant.Facility.OUTPATIENT)
                lblServiceUnit.Text = "KLINIK";
            else if (entityReg.DepartmentID == Constant.Facility.EMERGENCY)
                lblServiceUnit.Text = "UNIT PELAYANAN";
            else
                lblServiceUnit.Text = "PENUNJANG";

            lblTTDParamedic.Text = entityReg.ParamedicName;
            lblLastUpdatedBy.Text = appSession.UserFullName;
            lblLastUpdatedDate.Text = string.Format("{0}, {1} {2}", entityHealthcare.City, entity.cfDateForSignInString, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT));

            base.InitializeReport(param);
        }
    }
}
