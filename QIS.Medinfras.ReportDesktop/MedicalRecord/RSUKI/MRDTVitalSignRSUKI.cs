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
    public partial class MRDTVitalSignRSUKI : DevExpress.XtraReports.UI.XtraReport
    {
        public MRDTVitalSignRSUKI()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID)
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0 ORDER BY ID", VisitID);
            vVitalSignHd entityHd = BusinessLayer.GetvVitalSignHdList(filterExpression).FirstOrDefault();
            vVitalSignDt entityDt = BusinessLayer.GetvVitalSignDtList(filterExpression).FirstOrDefault();

            if (entityHd != null)
            {
                List<vVitalSignHd> lstVitalSignHd = BusinessLayer.GetvVitalSignHdList(string.Format("ID = {0} AND IsDeleted = 0", entityHd.ID));
                List<vVitalSignRSSEB> lstVitalSignDt = BusinessLayer.GetvVitalSignRSSEBList(string.Format("ID = {0} AND IsDeleted = 0", entityHd.ID));

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
}
