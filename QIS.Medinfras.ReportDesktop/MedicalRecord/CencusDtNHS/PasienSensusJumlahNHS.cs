using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class PasienSensusJumlahNHS : BaseRpt
    {
        public PasienSensusJumlahNHS()
        {

            InitializeComponent();
        }
        public void InitializeReport(List<PatientCencusInformation> lstPatientCensusClass)
        {


            this.DataSource = lstPatientCensusClass;
        }
    }
}
