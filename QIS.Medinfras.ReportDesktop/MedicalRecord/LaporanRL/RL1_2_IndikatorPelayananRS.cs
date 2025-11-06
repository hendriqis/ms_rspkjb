using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class RL1_2_IndikatorPelayananRS : BaseCustomDailyLandscapeA3Rpt
    {
        int CensusDate = 0;
        int IsNeedCodification = 0;
        public RL1_2_IndikatorPelayananRS()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            Healthcare entityHC = BusinessLayer.GetHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID))[0];

            lblNamaRS.Text = entityHC.HealthcareName;
            CensusDate = Convert.ToInt32(param[0]);
            IsNeedCodification = Convert.ToInt32(param[1]);
            base.InitializeReport(param);
        }

        private void lblBOR_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            int lamaHariRawat = 0;
            String bed = Convert.ToString(GetCurrentColumnValue("JmlhTempatTidur"));
            Int32 countBed = Convert.ToInt32(bed);

            List<GetIndicatorHospitalServicePerMonthPerYear> lst = BusinessLayer.GetIndicatorHospitalServicePerMonthPerYear(CensusDate, IsNeedCodification);

            foreach (GetIndicatorHospitalServicePerMonthPerYear hd in lst)
            {
                lamaHariRawat += hd.JmlhHariPerwatan;
            }

            Decimal BOR = countBed * 365;
            Decimal BOR2 = lamaHariRawat / BOR;
            Decimal BORPersen = BOR2 * 100;
            lblBOR.Text = BORPersen.ToString("N2");
        }
    }
}
