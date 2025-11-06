using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using System.Text;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BJurnalKeperawatan : QIS.Medinfras.ReportDesktop.BaseDailyPortraitRpt
    {
        public BJurnalKeperawatan()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            base.InitializeReport(param);

            int visitID = Convert.ToInt32(GetCurrentColumnValue("VisitID"));
            string linkfield = GetCurrentColumnValue("LinkField").ToString();

            if (visitID > 0)
            {
                vConsultVisit entity = BusinessLayer.GetvConsultVisitList(param[0]).FirstOrDefault();
                if (entity != null)
                {
                    lblRegistrationNo.Text = entity.RegistrationNo;
                    lblRegistrationDate.Text = entity.VisitDate.ToString("dd-MMM-yyyy");
                    lblPatientName.Text = String.Format("{0} ({1})", entity.PatientName.Trim(), entity.MedicalNo.Trim());
                    lblDateOfBirth.Text = entity.DateOfBirthInString;
                    lblParamedicName.Text = entity.ParamedicName;
                    lblDischargeDate.Text = entity.DischargeDateInString;
                }
            }
            else
            {
                string module = linkfield.Split('|')[1];
                if (module == "RI")
                {
                    vInpatientPatientListLink entity = BusinessLayer.GetvInpatientPatientListLinkList(param[0]).FirstOrDefault();
                    if (entity != null)
                    {
                        lblRegistrationNo.Text = entity.RegistrationNo;
                        lblRegistrationDate.Text = entity.VisitDate.ToString("dd-MMM-yyyy");
                        lblPatientName.Text = String.Format("{0} ({1})", entity.PatientName.Trim(), entity.MedicalNo.Trim());
                        lblDateOfBirth.Text = entity.DateOfBirthInString;
                        lblParamedicName.Text = entity.ParamedicName;
                        lblDischargeDate.Text = entity.DischargeDateInString;
                    }
                }
                else if (module == "RD")
                {
                    vEmergencyPatientListLink entity = BusinessLayer.GetvEmergencyPatientListLinkList(param[0]).FirstOrDefault();
                    if (entity != null)
                    {
                        lblRegistrationNo.Text = entity.RegistrationNo;
                        lblRegistrationDate.Text = entity.VisitDate.ToString("dd-MMM-yyyy");
                        lblPatientName.Text = String.Format("{0} ({1})", entity.PatientName.Trim(), entity.MedicalNo.Trim());
                        lblDateOfBirth.Text = entity.DateOfBirthInString;
                        lblParamedicName.Text = entity.ParamedicName;
                        lblDischargeDate.Text = entity.DischargeDateInString;
                    }
                }
                else
                {
                    vOutPatientPatientListLink entity = BusinessLayer.GetvOutPatientPatientListLinkList(param[0]).FirstOrDefault();
                    if (entity != null)
                    {
                        lblRegistrationNo.Text = entity.RegistrationNo;
                        lblRegistrationDate.Text = entity.VisitDate.ToString("dd-MMM-yyyy");
                        lblPatientName.Text = String.Format("{0} ({1})", entity.PatientName.Trim(), entity.MedicalNo.Trim());
                        lblDateOfBirth.Text = entity.DateOfBirthInString;
                        lblParamedicName.Text = entity.ParamedicName;
                        lblDischargeDate.Text = entity.DischargeDateInString;
                    }
                }
            }
            
        }

        private void xrLabel18_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            StringBuilder strBuild = new StringBuilder();
            strBuild.AppendLine(String.Format(String.Format("{0} - {1}", GetCurrentColumnValue("CreatedByName"), GetCurrentColumnValue("CreatedDateInString"))));
            strBuild.AppendLine(String.Format(String.Format("{0} - {1}", GetCurrentColumnValue("LastUpdatedByName"), GetCurrentColumnValue("LastUpdatedDateInString"))));
            ((XRLabel)sender).Text = strBuild.ToString();
        }
    }
}
