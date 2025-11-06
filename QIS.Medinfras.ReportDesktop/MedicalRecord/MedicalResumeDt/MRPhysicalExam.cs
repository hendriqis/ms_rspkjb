using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.IO;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class MRPhysicalExam : DevExpress.XtraReports.UI.XtraReport
    {
        public MRPhysicalExam()
        {
            InitializeComponent();
        }

        public void InitializeReport(int visitID)
        {
            List<vReviewOfSystemHd> lstROSHd = BusinessLayer.GetvReviewOfSystemHdList(string.Format("VisitID = {0} AND IsDeleted = 0", visitID));
            List<vVitalSignHd> lstTTVHd = BusinessLayer.GetvVitalSignHdList(string.Format("VisitID = {0} AND IsDeleted = 0", visitID));

            if (lstROSHd.Count() > 0)
            {
                subReviewOfSystem.CanGrow = true;
                mrdtReviewOfSystem.InitializeReport(visitID);
            }
            else
            {
                subReviewOfSystem.Visible = false;
            }

            if (lstTTVHd.Count() > 0)
            {
                subVitalSign.CanGrow = true;
                mrdtVitalSign.InitializeReport(visitID);
            }
            else
            {
                subVitalSign.Visible = false;
            }
        }
    }
}
