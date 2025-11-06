using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPendapatanLaboratoriumPerKelompokItemPerBulan : BaseCustomDailyPotraitRpt
    {
        public LPendapatanLaboratoriumPerKelompokItemPerBulan()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            #region subTotal
            subTotalPerItemGroup.CanGrow = true;
            if (!String.IsNullOrEmpty(param[2].ToString()))
            {
                lPendapatanLaboratoriumPerKelompokItemPerBulanRekap1.InitializeReport(Convert.ToInt32(param[0].ToString()), Convert.ToInt32(param[1].ToString()), Convert.ToInt32(param[2].ToString()));
            }
            else
            {
                lPendapatanLaboratoriumPerKelompokItemPerBulanRekap1.InitializeReport(Convert.ToInt32(param[0].ToString()), Convert.ToInt32(param[1].ToString()), 0);
            }
            #endregion
            
            base.InitializeReport(param);
        }

    }
}
