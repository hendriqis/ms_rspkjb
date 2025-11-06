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
    public partial class BLabelPasienRSBW : BaseRpt
    {
        public BLabelPasienRSBW()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            List<vLabelPatientRegistration> lstEntity = BusinessLayer.GetvLabelPatientRegistrationList(String.Format("RegistrationID = {0}", param));
            
            #region Section 1
            subSection1.CanGrow = true;
            bLabelPasienRSBWSection11.InitializeReport(lstEntity);
            #endregion

            #region Section 2
            subSection2.CanGrow = true;
            bLabelPasienRSBWSection21.InitializeReport(lstEntity);
            #endregion

            #region Section 3
            subSection3.CanGrow = true;
            bLabelPasienRSBWSection31.InitializeReport(lstEntity);
            #endregion

            #region Section 4
            subSection4.CanGrow = true;
            bLabelPasienRSBWSection41.InitializeReport(lstEntity);
            #endregion
        }
    }
}
