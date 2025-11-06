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
    public partial class BSuratPulang : BaseCustomDailyLandscapeA5Rpt
    {
        public BSuratPulang()
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

            string FirstName = entityPat.FirstName;
            string MiddleName = entityPat.MiddleName;
            string LastName = entityPat.LastName;
           

            lblNama.Text = string.Format("{0} {1} {2}", FirstName, MiddleName, LastName);
            lblDate.Text = string.Format("{0}", DateTime.Now.ToString("dd-MMM-yyyy"));
            lblHSU.Text =entitycv.ServiceUnitName;
            if (entitycv.DischargeDate != null && entitycv.DischargeDate.ToString(Constant.ConstantDate.DEFAULT_NULL) != "01-Jan-1900")
            {
                lblDate.Text = "";
            }
            else
            {
                lblTgl.Text = Convert.ToString(entitycv.DischargeDate.ToString(Constant.FormatString.DATE_FORMAT));
                
            }
           
           
            base.InitializeReport(param);
        }
    }
}
