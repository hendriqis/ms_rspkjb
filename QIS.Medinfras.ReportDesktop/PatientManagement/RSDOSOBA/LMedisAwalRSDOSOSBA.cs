using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using ThoughtWorks.QRCode.Codec;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LMedisAwalRSDOSOBA : BaseDailyPortraitRpt
    {
        public LMedisAwalRSDOSOBA()
        {
           
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            #region Header 1 : Healthcare
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            if (oHealthcare != null)
            {

                lblTxt.Text = string.Format("Dengan ini saya memberikan kuasa kepada dokter {0} yang telah memeriksa, melakukan tindakan/ operasi dan atau melakukan perawatan kepada saya karena sebab apapun, untuk memberikan keterangan lengkap termasuk riwayat medis saya sebelumnya kepada perusahaan/ asuransi tersebut diatas. Dalam hal ini saya akan mengganti kepada perusahaan/ asuransi, atas biaya yang tidak dipertanggungkan dalam polis.", oHealthcare.HealthcareName);
                lblCity.Text = oHealthcare.City;
            }
            #endregion

            base.InitializeReport(param);
        }
    }
}