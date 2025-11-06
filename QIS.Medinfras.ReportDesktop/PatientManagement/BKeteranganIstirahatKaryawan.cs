using System;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BKeteranganIstirahatKaryawan : BaseDailyPortraitRpt
    {
        public BKeteranganIstirahatKaryawan()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();
            vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", param[0])).FirstOrDefault();
            ConsultVisit entityCV = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", entityReg.RegistrationID))[0];
            ParamedicMaster entityPM = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", entityCV.ParamedicID))[0];

            String Day = param[3];
            String Tanggal = param[4];
            String Jam = param[5];
            String CountDate = string.Format ("dan perlu istirahat selama {0} hari terhitung mulai tanggal {1} s/d tanggal {2}",param[6],param[1],param[2]);

            lblHari.Text = Day;
            lblTanggal.Text = Tanggal;
            lblJam.Text = Jam;
            lblIstirahat.Text = CountDate;
            lblPrintDate.Text = string.Format("{0}, {1}", entityHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));
            lblParamedicName.Text = entityPM.FullName;
            base.InitializeReport(param);

        }
    }
}
