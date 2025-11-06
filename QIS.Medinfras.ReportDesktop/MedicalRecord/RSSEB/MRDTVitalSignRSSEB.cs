using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.Parameters;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class MRDTVitalSignRSSEB : DevExpress.XtraReports.UI.XtraReport
    {
        public MRDTVitalSignRSSEB()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID)
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0 AND GCParamedicMasterType = 'X019^001'", VisitID);
            List<vVitalSignHd> lstVitalSignHd = BusinessLayer.GetvVitalSignHdList(filterExpression);
            List<vVitalSignRSSEB> lstVitalSignDt = BusinessLayer.GetvVitalSignRSSEBList(filterExpression);

            var lst = (from p in lstVitalSignHd
                       select new
                       {
                           ObservationDateInString = p.ObservationDateInString,
                           ObservationTime = p.ObservationTime,
                           ParamedicName = p.ParamedicName,
                           VitalSignDts = lstVitalSignDt.Where(dt => dt.ID == p.ID).ToList()
                       }).ToList();

            this.DataSource = lst;
            DetailReport.DataMember = "VitalSignDts";
        }
    }
}
