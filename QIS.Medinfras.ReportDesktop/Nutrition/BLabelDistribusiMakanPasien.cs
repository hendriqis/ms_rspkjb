using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.XtraReports.UI;
//cetakan ini pakai kertas kecil
namespace QIS.Medinfras.ReportDesktop
{
    public partial class BLabelDistribusiMakanPasien : BaseRpt
    {
        public BLabelDistribusiMakanPasien()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string scheduleDate = param[0];
            string mealTime = string.Empty;
            if (param.Length == 2)
            {
                mealTime = param[1];
            }
            string filterExpression = string.Format("ScheduleDate = '{0}'", scheduleDate);
            if (!string.IsNullOrEmpty(mealTime))
            {
                filterExpression += string.Format(" AND GCMealTime = '{0}'", mealTime);
            }
            filterExpression += string.Format(" AND NutritionOrderDtID IN ({0})", param[2]);
            List<vNutritionOrderDtCustom> lst = BusinessLayer.GetvNutritionOrderDtCustomList(filterExpression);
            this.DataSource = lst;
        }
    }
}
