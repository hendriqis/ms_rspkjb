using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BTransaksiUnit1 : BaseDailyPortrait1Rpt
    {
        public BTransaksiUnit1()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = {0}",appSession.HealthcareID))[0];
            lblLastUpdatedDate.Text = entityHealthcare.City+", ";

            vPatientChargesHd entity = BusinessLayer.GetvPatientChargesHdList(string.Format("TransactionID = {0}", param[0]))[0];
            lblLastUpdatedBy.Text = appSession.UserFullName;
            lblLastUpdatedDate.Text += entity.LastUpdatedDateInString;

            vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", entity.RegistrationID))[0];
            lblRegistrationNo.Text = string.Format("{0}", entityReg.RegistrationNo);
            lblPayer.Text = entityReg.BusinessPartner;
            lblPatientName.Text = entityReg.PatientName;
            lblTransactionNo.Text = entity.TransactionNo;
            lblTransactionDate.Text = entity.TransactionDateInString;
            lblServiceUnitName.Text = entity.ServiceUnitName;
            lblVisitServiceUnitName.Text = entity.VisitServiceUnitName;
            lblParamedic.Text = entityReg.ParamedicName;


            //if (entityReg.DepartmentID == Constant.Facility.INPATIENT)
            //    lblServiceUnit.Text = GetLabel("Ruang Perawatan");
            //else if (entityReg.DepartmentID == Constant.Facility.OUTPATIENT)
            //    lblServiceUnit.Text = GetLabel("Klinik");
            //else if (entityReg.DepartmentID == Constant.Facility.EMERGENCY)
            //    lblServiceUnit.Text = GetLabel("Unit Pelayanan");
            //else
            //    lblServiceUnit.Text = GetLabel("Penunjang Medis");

            if (entityReg.DepartmentID == Constant.Facility.INPATIENT)
                lblServiceUnit.Text = "Ruang Perawatan";
            else if (entityReg.DepartmentID == Constant.Facility.OUTPATIENT)
                lblServiceUnit.Text = "Klinik";
            else if (entityReg.DepartmentID == Constant.Facility.EMERGENCY)
                lblServiceUnit.Text = "Unit Pelayanan";
            else
                lblServiceUnit.Text = "Penunjang Medis";

            base.InitializeReport(param);
        }
    }
}
