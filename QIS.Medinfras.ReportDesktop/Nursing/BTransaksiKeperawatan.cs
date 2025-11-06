using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using System.Linq;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BTransaksiKeperawatan : BaseDailyLandscapeRpt
    {
        public BTransaksiKeperawatan()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string menuType = "";
            string patientName = "";
            string medicalNo = "";
            string serviceUnitName = "";
            string cityOfBirth = "";
            string dateOfBirth = "";
            string paramedicName = "";
            string filterExpression = param[0];
            vNursingTransactionHd entity = BusinessLayer.GetvNursingTransactionHdList(filterExpression).FirstOrDefault();


            if (entity.VisitID > 0)
            {
                vConsultVisit entityVisit = BusinessLayer.GetvConsultVisitList(String.Format("VisitID = {0}",entity.VisitID)).FirstOrDefault();
                vHealthcareServiceUnit entityServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(String.Format("ServiceUnitCode = '{0}'", entity.ServiceUnitCode)).FirstOrDefault();
                patientName = entityVisit.PatientName;
                medicalNo = entityVisit.MedicalNo;
                cityOfBirth = entityVisit.CityOfBirth;
                dateOfBirth = entityVisit.DateOfBirth.ToString("dd-MMM-yyyy");
                paramedicName = entityVisit.ParamedicName;
                serviceUnitName = entityServiceUnit.ServiceUnitName;
            }
            else
            {

                menuType = entity.LinkField.Split('|')[1];
                if (menuType == "RI")
                {
                    vInpatientPatientListLink entityRegistration = BusinessLayer.GetvInpatientPatientListLinkList(String.Format("RegistrationNo = '{0}'", entity.RegistrationNo)).FirstOrDefault();
                    vInpatientServiceUnitLink entitiyServiceUnit = BusinessLayer.GetvInpatientServiceUnitLinkList(String.Format("ServiceUnitCode = '{0}'", entity.ServiceUnitCode)).FirstOrDefault();
                    patientName = entityRegistration.PatientName;
                    medicalNo = entityRegistration.MedicalNo;
                    cityOfBirth = entityRegistration.CityOfBirth;
                    dateOfBirth = entityRegistration.DateOfBirth.ToString("dd-MMM-yyyy");
                    paramedicName = entityRegistration.ParamedicName;
                    serviceUnitName = entitiyServiceUnit.ServiceUnitName;
                }
                else if (menuType == "RD")
                {
                    vEmergencyPatientListLink entityRegistration = BusinessLayer.GetvEmergencyPatientListLinkList(String.Format("RegistrationNo = '{0}'", entity.RegistrationNo)).FirstOrDefault();
                    patientName = entityRegistration.PatientName;
                    medicalNo = entityRegistration.MedicalNo;
                    cityOfBirth = entityRegistration.CityOfBirth;
                    dateOfBirth = entityRegistration.DateOfBirth.ToString("dd-MMM-yyyy");
                    paramedicName = entityRegistration.ParamedicName;
                    serviceUnitName = "IGD";
                }
                else
                {
                    vOutPatientPatientListLink entityRegistration = BusinessLayer.GetvOutPatientPatientListLinkList(String.Format("RegistrationNo = '{0}'", entity.RegistrationNo)).FirstOrDefault();
                    vServiceUnitLink entitiyServiceUnit = BusinessLayer.GetvServiceUnitLinkList(String.Format("ServiceUnitCode = '{0}'", entity.ServiceUnitCode)).FirstOrDefault();
                    patientName = entityRegistration.PatientName;
                    medicalNo = entityRegistration.MedicalNo;
                    cityOfBirth = entityRegistration.CityOfBirth;
                    dateOfBirth = entityRegistration.DateOfBirth.ToString("dd-MMM-yyyy");
                    paramedicName = entityRegistration.ParamedicName;
                    serviceUnitName = entitiyServiceUnit.ServiceUnitName;
                }
            }

            lblNamaPasien.Text = String.Format("{0} ({1})", patientName.Trim(), medicalNo.Trim());
            lblServiceUnit.Text = serviceUnitName;
            lblTmptTglLahir.Text = String.Format("{0}, {1}", cityOfBirth.Trim(), dateOfBirth);
            lblParamedicName.Text = paramedicName;

            //sbrNursingDiagnoseItem.CanGrow = true;
            bNursingDiagnoseItemSubRpt1.InitializeReport(filterExpression);
            bNursingOutcomeItemSubRpt1.InitializeReport(filterExpression);
            bNursingInterventionItemSubRpt1.InitializeReport(filterExpression);
            bNursingSOAPSubRpt1.InitializeReport(filterExpression);
            bNursingImplementationSubRpt1.InitializeReport(String.Format("RegistrationNo = '{0}' AND {1}", entity.RegistrationNo, filterExpression));

            base.InitializeReport(param);

        }

    }
}
