using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class MRAnamnesis : DevExpress.XtraReports.UI.XtraReport
    {
        public MRAnamnesis()
        {
            InitializeComponent();
        }

        public void InitializeReport(int visitID)
        {
            vConsultVisit entityCV = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", visitID)).FirstOrDefault();

            //lblVisitType.Text = string.Format("{0} - {1}", entityCV.VisitTypeCode, entityCV.VisitTypeName);
            if (entityCV.GCVisitReason != null && entityCV.GCVisitReason != "")
            {
                StandardCode entitiSC_VisitReason = BusinessLayer.GetStandardCode(entityCV.GCVisitReason);
                lblVisitReason.Text = entitiSC_VisitReason.StandardCodeName;
            }
            else
            {
                lblVisitReason.Text = "";
            }
            //lblLOS.Text = entityCV.LOS;

            subChiefComplaint.CanGrow = true;
            mrdtChiefComplaint.InitializeReport(visitID);

            subAllergies.CanGrow = true;
            mrdtAllergies.InitializeReport(entityCV.MRN);
        }

    }
}
