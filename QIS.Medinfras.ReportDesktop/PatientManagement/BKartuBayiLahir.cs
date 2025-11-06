using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.XtraReports.UI;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BKartuBayiLahir : BaseRpt
    {
        public BKartuBayiLahir()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            int registrationID = Convert.ToInt32(param[0]);
            
            List<vConsultVisit> entity = BusinessLayer.GetvConsultVisitList(String.Format("RegistrationID = '{0}'", registrationID));
            if (entity.FirstOrDefault().IsNewBorn == true)
            {
                PatientBirthRecord entityBirth = BusinessLayer.GetPatientBirthRecordList(string.Format("MRN = {0}", entity.FirstOrDefault().MRN)).FirstOrDefault();
                List<vConsultVisit> entityMother = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", entityBirth.MotherVisitID));
                List<vHealthcareServiceUnit> entityServiceUnitMother = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", entityMother.FirstOrDefault().HealthcareServiceUnitID));
                Room entityRoomMother = BusinessLayer.GetRoomList(string.Format("RoomID = {0}", entityMother.FirstOrDefault().RoomID)).FirstOrDefault();
                Bed entityBedMother = BusinessLayer.GetBedList(string.Format("BedID = {0}", entityMother.FirstOrDefault().BedID)).FirstOrDefault();
                cKamarIbu.Text = string.Format("{0}, {1}, {2}", entityServiceUnitMother.FirstOrDefault().ServiceUnitName, entityRoomMother.RoomName, entityBedMother.BedCode);
            }
            else {
                cKamarIbu.Text = "-";
            }

            this.DataSource = entity;
        }
    }
}
