using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Linq;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BSuratPengantarTagihanRSDO : BaseDailyPortraitRpt
    {
        public BSuratPengantarTagihanRSDO()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            #region Sign
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            lblSignDate.Text = String.Format("{0}, {1}", oHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT)); // Surakarta, 24-Jul-2019
            lblSignHealthcareName.Text = oHealthcare.HealthcareName;
            base.InitializeReport(param);
            #endregion
        }
    }

    
}
