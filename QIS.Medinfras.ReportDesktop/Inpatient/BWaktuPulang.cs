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
    public partial class BWaktuPulang : BaseCustomDailyLandscapeA5Rpt
    {
        public BWaktuPulang()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            vRegistration entityReg = BusinessLayer.GetvRegistrationList(param[0])[0];
            string MRN = string.Format("{0}", entityReg.MRN);

            string RegistrationID = string.Format("{0}", entityReg.RegistrationID);
            vPatient entityPat = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", MRN))[0];
            vConsultVisit entitycv = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0} ", RegistrationID))[0];
            PatientPaymentHd entityphd = BusinessLayer.GetPatientPaymentHdList(string.Format("RegistrationID = {0}", entityReg.RegistrationID)).FirstOrDefault();

            string FirstName = entityPat.FirstName;
            string MiddleName = entityPat.MiddleName;
            string LastName = entityPat.LastName;


            lblNama.Text = string.Format("{0} {1} {2}", FirstName, MiddleName, LastName);
            lblDate.Text = string.Format("{0}", DateTime.Now.ToString("dd-MMM-yyyy"));
            lblHSU.Text = entitycv.ServiceUnitName;
        


            if (entityphd != null)
            {
                lblTgl.Text = Convert.ToString(entityphd.PaymentDate.ToString(Constant.FormatString.DATE_FORMAT));
                lblTime.Text = entityphd.PaymentTime;
            }
            else
            {
                lblTgl.Text = "";
                lblTime.Text = "";
            }

            base.InitializeReport(param);
        }

    }
}
