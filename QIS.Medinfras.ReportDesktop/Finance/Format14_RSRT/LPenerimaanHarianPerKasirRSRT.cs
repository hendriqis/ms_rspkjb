using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using System.Linq;
namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPenerimaanHarianPerKasirRSRT : BaseDailyLandscapeRpt
    {
        public LPenerimaanHarianPerKasirRSRT()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string shift = "";
            if(!string.IsNullOrEmpty(param[2]))
            {
                StandardCode oStandardCode = BusinessLayer.GetStandardCodeList(string.Format("StandardCodeID='{0}'", param[2])).FirstOrDefault();
                if(oStandardCode != null){
                    shift = oStandardCode.StandardCodeName;
                }

            }
            string[] date = param[0].Split(';');
            string filterExpression = ""; 
            string dtPeriode = "";
            string dt1From = Helper.YYYYMMDDToDate(date[0]).ToString(Constant.FormatString.DATE_FORMAT);
            string dt1To = Helper.YYYYMMDDToDate(date[1]).ToString(Constant.FormatString.DATE_FORMAT);
                
             
            if (date.Length > 1)
            {
                string dtFrom = Helper.YYYYMMDDToDate(date[0]).ToString(Constant.FormatString.DATE_FORMAT);
                string dtTo = Helper.YYYYMMDDToDate(date[1]).ToString(Constant.FormatString.DATE_FORMAT);
                if (dtFrom == dtTo)
                {
                    dtPeriode = string.Format("{0}", dtFrom);
                }
                else {
                    dtPeriode = string.Format("{0} s.d {1}", dtFrom, dtTo);
                }
                
            }
            else {
                dtPeriode = string.Format("{0}", Helper.YYYYMMDDToDate(date[0]).ToString(Constant.FormatString.DATE_FORMAT));
            }

            lblUnit.Text = string.Format("Tanggal {0} Shift : {1}", dtPeriode, shift.ToUpper());
            lblPetugas.Text = appSession.UserFullName;

            subCN.CanGrow = true;


            List<GetPatientPaymentDtRekapReportRSRT> lstData = BusinessLayer.GetPatientPaymentDtRekapReportRSRT(param[0], param[1], param[2], param[3], Convert.ToInt32(param[4]));
            bTotalRekapPembayaranPerkasirRSRT.InitializeReport(lstData);
            base.InitializeReport(param);


        }

    }
}
