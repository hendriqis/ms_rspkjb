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
    public partial class BLabelPasienRSSK : BaseRpt
    {
        public BLabelPasienRSSK()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            int RegistrationID = 0;
            int PrintCount = 0;

            if (param.Length > 1)
            {
                RegistrationID = Convert.ToInt32(param[0]);
                PrintCount = Convert.ToInt16(param[1]);
            }
            else
            {
                RegistrationID = Convert.ToInt32(param[0]);
                PrintCount = 1;
            }

            List<vLabelInpatientRegistration> lstEntity = BusinessLayer.GetvLabelInpatientRegistrationList(String.Format("RegistrationID = {0}", RegistrationID));

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
            bLabelPasienRSSKSection11.InitializeReport(lstEntity);
            #endregion

            #region Section 2
            subSection2.CanGrow = true;
            bLabelPasienRSSKSection12.InitializeReport(lstEntity);
            #endregion

            #region Section 3
            subSection3.CanGrow = true;
            bLabelPasienRSSKSection13.InitializeReport(lstEntity);
            #endregion

            #region Section 4
            subSection4.CanGrow = true;
            bLabelPasienRSSKSection14.InitializeReport(lstEntity);
            #endregion
        }
    }
}
