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
    public partial class BPenerimaanUangJaminanNHS : BaseDailyPortraitRpt
    {
        public BPenerimaanUangJaminanNHS()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string city = "";
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            if (entityHealthcare.ZipCodeID != 0)
            {
                ZipCodes entityZipCode = BusinessLayer.GetZipCodesList(String.Format("ID = {0}", entityHealthcare.ZipCodeID))[0];
                city = entityZipCode.City;
            }

            vPatientPaymentHd entityPayment = BusinessLayer.GetvPatientPaymentHdList(string.Format("{0}", param[0]))[0];
            lblLastUpdatedDate.Text = city + ", " + entityPayment.PaymentDate.ToString(Constant.FormatString.DATE_FORMAT);
            lblLastUpdatedBy.Text = appSession.UserFullName;
            lblTotalAmountString.Text = "# " + entityPayment.TotalPaymentAmountInString + " #";

            vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", entityPayment.RegistrationID))[0];
            lblUnitPelayanan.Text = entityReg.ServiceUnitName;
            lblParamedicName.Text = entityReg.ParamedicName;

            base.InitializeReport(param);
        }

    }
}
