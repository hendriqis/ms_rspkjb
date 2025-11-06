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
    public partial class BDaftarTerapiPasien : BaseCustomDailyPotraitRpt
    {
        public BDaftarTerapiPasien()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string visitID = param[0];
            string display = param[1];
            string isUDD = param[2];

            #region Header
            vConsultVisit16 entity = BusinessLayer.GetvConsultVisit16List(string.Format("VisitID = {0}", visitID)).FirstOrDefault();

            xrTableCell3.Text = entity.MedicalNo;
            xrTableCell28.Text = entity.PatientName;
            xrTableCell42.Text = string.Format("{0} ( {1} yr {2} mth {3} day )", entity.DateOfBirthInString, entity.AgeInYear, entity.AgeInMonth, entity.AgeInDay);
            cRegNo.Text = entity.RegistrationNo;
            cRegPhysician.Text = entity.ParamedicName;
            cServiceUnit.Text = entity.ServiceUnitName;
            cRoom.Text = entity.RoomName;
            cBed.Text = entity.BedCode;
            #endregion

            List<PrescriptionUDDListByDispense> lstEntity = BusinessLayer.GetPrescriptionUDDListByDispense(visitID, display, isUDD);
            lstEntity = lstEntity.OrderBy(lst => lst.DrugName).ToList();
            this.DataSource = lstEntity;


            base.InitializeReport(param);
        }

        protected override bool IsSkipBinding()
        {
            return true;
        }
    }
}
