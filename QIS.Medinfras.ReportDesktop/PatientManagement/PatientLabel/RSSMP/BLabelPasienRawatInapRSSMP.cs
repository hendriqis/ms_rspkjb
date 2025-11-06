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
    public partial class BLabelPasienRawatInapRSSMP : BaseRpt
    {
        public BLabelPasienRawatInapRSSMP()
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
            bLabelPasienRawatInapRSSMPSection1.InitializeReport(lstEntity);
            #endregion

            #region Section 2
            subSection1.CanGrow = true;
            bLabelPasienRawatInapRSSMPSection2.InitializeReport(lstEntity);
            #endregion

            #region Section 3
            subSection1.CanGrow = true;
            bLabelPasienRawatInapRSSMPSection3.InitializeReport(lstEntity);
            #endregion

            #region Section 4
            subSection1.CanGrow = true;
            bLabelPasienRawatInapRSSMPSection4.InitializeReport(lstEntity);
            #endregion

            #region Section 5
            subSection1.CanGrow = true;
            bLabelPasienRawatInapRSSMPSection5.InitializeReport(lstEntity);
            #endregion
        }
    }
}
