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
    public partial class BTransaksiRSRT : BaseDailyPortrait1Rpt
    {
        public BTransaksiRSRT()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();
            vPatientChargesHd entity = BusinessLayer.GetvPatientChargesHdList(string.Format("TransactionID = {0}", param[0])).FirstOrDefault();
            vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", entity.RegistrationID)).FirstOrDefault();

            string ParamedicName = entityReg.ParamedicName;
            if(entity.PrescriptionOrderID >0){
                vPrescriptionOrderHd oPhd = BusinessLayer.GetvPrescriptionOrderHdList(string.Format("PrescriptionOrderID='{0}'", entity.PrescriptionOrderID)).FirstOrDefault();
                if (oPhd != null) {
                    if (!string.IsNullOrEmpty(oPhd.ParamedicName)) {
                        ParamedicName = oPhd.ParamedicName;
                    }
                }
            }

            cParamedicName.Text = ParamedicName;
            cBusinessPartnerName.Text = entityReg.BusinessPartnerName;
            cTransactionDate.Text = entity.TransactionDateInString;
            cServiceUnitName.Text = string.Format("{0} ({1})", entityReg.ServiceUnitName, entityReg.ChargeClassName);
                        
            if (entityReg.DepartmentID == Constant.Facility.INPATIENT)
                lblServiceUnit.Text = "Ruang Perawatan";
            else if (entityReg.DepartmentID == Constant.Facility.OUTPATIENT)
                lblServiceUnit.Text = "Klinik";
            else if (entityReg.DepartmentID == Constant.Facility.EMERGENCY)
                lblServiceUnit.Text = "Unit Pelayanan";
            else
                lblServiceUnit.Text = "Penunjang Medis";

            lblLastUpdatedBy.Text = appSession.UserFullName;
            lblLastUpdatedDate.Text = string.Format("{0}, {1}", entityHealthcare.City, entity.cfDateForSignInString);

            base.InitializeReport(param);
        }
    }
}
