using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LTotalPasien : BaseDailyPortraitRpt
    {
        public LTotalPasien()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
            List<GetTotalVisit> lstTotalVisit = BusinessLayer.GetTotalVisitList(param[0]);
            //Int32 TAPS = Convert.ToInt32(lstTotalVisit.Where(a => a.VisitTypeName == "APS").FirstOrDefault().TotalVisit.ToString("D").Count());
            //Int32 TAPD = Convert.ToInt32(lstTotalVisit.Where(a => a.VisitTypeName == "APD").FirstOrDefault().TotalVisit.ToString("D").Count());
            //Int32 TAPP = Convert.ToInt32(lstTotalVisit.Where(a => a.VisitTypeName == "APP").FirstOrDefault().TotalVisit.ToString("D").Count());
            //Int32 TAPL = Convert.ToInt32(lstTotalVisit.Where(a => a.VisitTypeName == "APL").FirstOrDefault().TotalVisit.ToString("D").Count());
            //lblTAPS.Text = string.Format("{0}", TAPS);
            //lblTAPD.Text = string.Format("{0}", TAPD);
            //lblTAPP.Text = string.Format("{0}", TAPP);
            //lblTAPL.Text = string.Format("{0}", TAPL);
            int TAPS = lstTotalVisit.AsEnumerable().Where(a => a.VisitTypeName == ("APS")).Sum(a => a.TotalVisit);
            int TAPD = lstTotalVisit.AsEnumerable().Where(a => a.VisitTypeName == ("APD")).Sum(a => a.TotalVisit);
            int TAPP = lstTotalVisit.AsEnumerable().Where(a => a.VisitTypeName == ("APP")).Sum(a => a.TotalVisit);
            int TAPL = lstTotalVisit.AsEnumerable().Where(a => a.VisitTypeName == ("APL")).Sum(a => a.TotalVisit);

            int BAPS = lstTotalVisit.AsEnumerable().Where(a => a.VisitTypeName == ("APS")).Sum(a => a.PasienBaru);
            int BAPD = lstTotalVisit.AsEnumerable().Where(a => a.VisitTypeName == ("APD")).Sum(a => a.PasienBaru);
            int BAPP = lstTotalVisit.AsEnumerable().Where(a => a.VisitTypeName == ("APP")).Sum(a => a.PasienBaru);
            int BAPL = lstTotalVisit.AsEnumerable().Where(a => a.VisitTypeName == ("APL")).Sum(a => a.PasienBaru);

            int LAPS = lstTotalVisit.AsEnumerable().Where(a => a.VisitTypeName == ("APS")).Sum(a => a.Pasienlama);
            int LAPD = lstTotalVisit.AsEnumerable().Where(a => a.VisitTypeName == ("APD")).Sum(a => a.Pasienlama);
            int LAPP = lstTotalVisit.AsEnumerable().Where(a => a.VisitTypeName == ("APP")).Sum(a => a.Pasienlama);
            int LAPL = lstTotalVisit.AsEnumerable().Where(a => a.VisitTypeName == ("APL")).Sum(a => a.Pasienlama);
            
            if (lstTotalVisit.Where(a => a.VisitTypeName == "APS") != null)
            {
                lblTAPS.Text = string.Format("{0}", TAPS);
                lblBAPS.Text = string.Format("{0}", BAPS);
                lblLAPS.Text = string.Format("{0}", LAPS);
            }
            else
            {
                lblTAPS.Text = string.Format("0");
                lblBAPS.Text = string.Format("0");
                lblLAPS.Text = string.Format("0");
            }
            if (lstTotalVisit.Where(a => a.VisitTypeName == "APD") != null)
            {
                lblTAPD.Text = string.Format("{0}", TAPD);
                lblBAPD.Text = string.Format("{0}", BAPD);
                lblLAPD.Text = string.Format("{0}", LAPD);
            }
            else
            { 
                lblTAPD.Text = string.Format("0");
                lblBAPD.Text = string.Format("0");
                lblLAPD.Text = string.Format("0");
            }
            if (lstTotalVisit.Where(a => a.VisitTypeName == "APP") != null)
            {
                lblTAPP.Text = string.Format("{0}", TAPP);
                lblBAPP.Text = string.Format("{0}", BAPP);
                lblLAPP.Text = string.Format("{0}", LAPP);
            }
            else
            {
                lblTAPP.Text = string.Format("0");
                lblBAPP.Text = string.Format("0");
                lblLAPP.Text = string.Format("0");
            }
            if (lstTotalVisit.Where(a => a.VisitTypeName == "APL") != null)
            {
                lblTAPL.Text = string.Format("{0}", TAPL);
                lblBAPL.Text = string.Format("{0}", BAPL);
                lblLAPL.Text = string.Format("{0}", LAPL);
            }
            else
            {
                lblTAPL.Text = string.Format("0");
                lblBAPL.Text = string.Format("0");
                lblLAPL.Text = string.Format("0");
            }
            base.InitializeReport(param);
        }

    }
}
