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
    public partial class BSuratJaminanPelayananBenefitInhealth : BaseRpt
    {
        private int _lineNumber = 0;
        public BSuratJaminanPelayananBenefitInhealth()
        {
            _lineNumber = 0;
            InitializeComponent();
        }

        public void InitializeReport(List<InhealthPatientBenefits> lst)
        {
            this.DataSource = lst;
        }
    }
}
