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
    public partial class SubTransGroupTotalRSRT : DevExpress.XtraReports.UI.XtraReport
    {
        public SubTransGroupTotalRSRT()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<GetTRSSummaryDtTransRegistrationDetailRSRT> lst, Decimal pphAmount ,
            decimal TotalHonorSetelahPajak, decimal PenambahanLainLain, decimal PotonganLainLain, decimal honorBersih)
        {
            lblPPH21.Text = pphAmount.ToString(Constant.FormatString.NUMERIC_2);
            lblTotalHonorSetelahPajak.Text = TotalHonorSetelahPajak.ToString(Constant.FormatString.NUMERIC_2);
            lblPenambahan.Text = PenambahanLainLain.ToString(Constant.FormatString.NUMERIC_2);
            lblPengurangan.Text = PotonganLainLain.ToString(Constant.FormatString.NUMERIC_2);
            lblHonorBersih.Text = honorBersih.ToString(Constant.FormatString.NUMERIC_2);

            List<GetTRSSummaryDtTransRegistrationDetailRSRT> lstData =
                lst.GroupBy(p => p.Department)
                .Select(cl => new GetTRSSummaryDtTransRegistrationDetailRSRT
                { 
                    Department = cl.FirstOrDefault().Department,
                    RevenueSharingAmount = cl.Sum(c =>c.RevenueSharingAmount),
                    BrutoAmount = cl.Sum(c => c.BrutoAmount)
                }).ToList();

            this.DataSource = lstData;
        }

    }
}
