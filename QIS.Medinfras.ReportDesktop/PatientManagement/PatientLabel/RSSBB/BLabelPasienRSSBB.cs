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
    public partial class BLabelPasienRSSBB : BaseRpt
    {
        public BLabelPasienRSSBB()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            int RegistrationID = Convert.ToInt32(param[0]);
            int PrintCount = Convert.ToInt16(param[1]);
            List<vLabelPatientRegistrationInfo> lstEntity = BusinessLayer.GetvLabelPatientRegistrationInfoList(String.Format("RegistrationID = {0}", RegistrationID));

            if (PrintCount == 1)
            {
                subSection1.Visible = true;
                subSection2.Visible = false;
                subSection3.Visible = false;
                subSection4.Visible = false;
            }
            else if (PrintCount == 2)
            {
                subSection1.Visible = true;
                subSection2.Visible = true;
                subSection3.Visible = false;
                subSection4.Visible = false;
            }
            else if (PrintCount == 3)
            {
                subSection1.Visible = true;
                subSection2.Visible = true;
                subSection3.Visible = true;
                subSection4.Visible = false;
            }
            else if (PrintCount == 4)
            {
                subSection1.Visible = true;
                subSection2.Visible = true;
                subSection3.Visible = true;
                subSection4.Visible = true;
            }

            #region Section 1
            subSection1.CanGrow = true;
            bLabelPasienRSSBBSection11.InitializeReport(lstEntity);
            #endregion

            #region Section 2
            subSection2.CanGrow = true;
            bLabelPasienRSSBBSection21.InitializeReport(lstEntity);
            #endregion

            #region Section 3
            subSection3.CanGrow = true;
            bLabelPasienRSSBBSection31.InitializeReport(lstEntity);
            #endregion

            #region Section 4
            subSection4.CanGrow = true;
            bLabelPasienRSSBBSection41.InitializeReport(lstEntity);
            #endregion
        }
    }
}
