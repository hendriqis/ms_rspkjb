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
    public partial class BTransferPasien : BaseCustomDailyPotraitRpt
    {
        public BTransferPasien()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            vRegistration entityReg = BusinessLayer.GetvRegistrationList(param[0])[0];

            vPatientTransfer entityTransfer = BusinessLayer.GetvPatientTransferList(string.Format("RegistrationID = {0} ", entityReg.RegistrationID)).FirstOrDefault();
            string MRN = string.Format("{0}", entityReg.MRN);
            //string RegistrationID = string.Format("{0}", entityTransfer.RegistrationID);

            vPatient entityPat = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", MRN))[0];

            ConsultVisit entitycv = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0} ", entityReg.RegistrationID))[0];

            PatientAllergy entityPa = BusinessLayer.GetPatientAllergyList(string.Format("MRN = {0}", entityPat.MRN)).FirstOrDefault();

            vParamedicTeam entityParam = BusinessLayer.GetvParamedicTeamList(string.Format("RegistrationID = {0}  AND GCParamedicRole = '{1}' ", entityReg.RegistrationID, Constant.ParamedicRole.PELAKSANA))[0];
            vRegistrationPatientPerPeriod entityphd = BusinessLayer.GetvRegistrationPatientPerPeriodList(string.Format("VisitID = {0}", entityReg.VisitID)).FirstOrDefault();
            vParamedicTeam entityParammedic = BusinessLayer.GetvParamedicTeamList(string.Format("RegistrationID = {0}  AND GCParamedicRole = '{1}' ", entityReg.RegistrationID, Constant.ParamedicRole.PENGIRIM)).FirstOrDefault();
            lbldpjp.Text = entityParam.ParamedicName;

            string FirstName = entityPat.FirstName;
            string MiddleName = entityPat.MiddleName;
            string LastName = entityPat.LastName;


            lblNoReg.Text = entityReg.RegistrationNo;
            lblNama.Text = string.Format("{0} {1} {2}", FirstName, MiddleName, LastName);
            lblTgl.Text = entityPat.cfDateOfBirth;
            lblGender.Text = entityPat.Gender;

            lblDiagnose.Text = entityphd.PatientDiagnosis;
           
            
            lblDate.Text = Convert.ToString(entitycv.VisitDate.ToString(Constant.FormatString.DATE_FORMAT));
            lblTime.Text = entitycv.VisitTime;

            if (entityTransfer != null)
            {
                string FromClassName = entityTransfer.FromClassName;
                string FromServiceUnit = entityTransfer.FromServiceUnitName;
                string FromRoomName = entityTransfer.FromRoomName;
                string ToServiceUnit = entityTransfer.ToServiceUnitName;
                string ToClassName = entityTransfer.ToClassName;
                string ToRoomName = entityTransfer.ToRoomName; ;
                lblTo.Text = string.Format("{0} {1} {2}", FromClassName, FromServiceUnit, FromRoomName);
                lblFrom.Text = string.Format("{0} {1} {2}",ToClassName, ToServiceUnit, ToRoomName);
            }
            else
            {
                lblTo.Text="";
                lblFrom.Text="";
            }


            if (entityParammedic != null)
            {
                lblKonsulen.Text = entityParammedic.ParamedicName;
            }
            else
            {
                lblKonsulen.Text = "";
            }

            if (entityPa != null)
            {
                lblAllergy.Text = entityPa.Allergen;
            }
            else
            {
                lblAllergy.Text = "";
            }
            base.InitializeReport(param);
        }

    }
}
