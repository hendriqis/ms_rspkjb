using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    //public partial class BLabelObat : DevExpress.XtraReports.UI.XtraReport
    public partial class BLabelObatX : BaseRpt
    {
        public BLabelObatX()
        {
            InitializeComponent();
        }

        //public void InitializeReport(List<vPrescriptionOrderDt1> lst)
        public override void InitializeReport(string[] param)
        {
            string filter = string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID);
            List<vPrescriptionOrderDt1> lst = BusinessLayer.GetvPrescriptionOrderDt1List(filter);
            
            //this.DataSource = lst;
            base.InitializeReport(param);
        }
    }
}
