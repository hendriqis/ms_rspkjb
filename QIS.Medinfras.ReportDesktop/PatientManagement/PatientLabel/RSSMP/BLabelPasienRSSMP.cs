using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.XtraReports.UI;
//cetakan ini pakai kertas kecil
namespace QIS.Medinfras.ReportDesktop
{
    public partial class BLabelPasienRSSMP : BaseRpt
    {
        public BLabelPasienRSSMP()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            int RegistrationID = Convert.ToInt32(param[0]);
            List<vLabelPatientRegistration> lstEntity = BusinessLayer.GetvLabelPatientRegistrationList(String.Format("RegistrationID = {0}", RegistrationID));

            subSection1.Visible = true;

            #region Section 1
            subSection1.CanGrow = true;
            bLabelPasienRSSMPSection1.InitializeReport(lstEntity);
            #endregion

            #region Section 2
            subSection1.CanGrow = true;
            bLabelPasienRSSMPSection2.InitializeReport(lstEntity);
            #endregion

            #region Section 3
            subSection1.CanGrow = true;
            bLabelPasienRSSMPSection3.InitializeReport(lstEntity);
            #endregion

            #region Section 4
            subSection1.CanGrow = true;
            bLabelPasienRSSMPSection4.InitializeReport(lstEntity);
            #endregion

            #region Section 5
            subSection1.CanGrow = true;
            bLabelPasienRSSMPSection5.InitializeReport(lstEntity);
            #endregion
        }
    }
}
