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
    public partial class BPendaftaranRanapRSPM : BaseDailyPortrait2Rpt
    {
        public BPendaftaranRanapRSPM()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();
            vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", param[0])).FirstOrDefault();
            vMedicalResumeEarlyRSSM oData = BusinessLayer.GetvMedicalResumeEarlyRSSMList(string.Format("RegistrationID = {0}", param[0])).FirstOrDefault();
            txtEmail.Text = entityHealthcare.Email;
            lblCreatedBy.Text = entity.CreatedByName;

            lblQueueNo.Text = entity.cfQueueNo;
            
            if (entity.DepartmentID == Constant.Facility.OUTPATIENT ||
                entity.DepartmentID == Constant.Facility.EMERGENCY 
                )
            {
               /// xrTable1.Visible = false;
                //xrTableCell33.Visible = false;
                //xrTableCell36.Visible = false;
                //cRujukan.Visible = false;
                tblCheklist.Visible = true;
            }else {
                tblCheklist.Visible = false;
                
            }

            if (entity.DepartmentID != Constant.Facility.INPATIENT)
            {
                cRujukan.Text = "-";
            }
            else
            {
                cRujukan.Text = oData.AdmissionSource;
            }

            base.InitializeReport(param);

        }
    }
}
