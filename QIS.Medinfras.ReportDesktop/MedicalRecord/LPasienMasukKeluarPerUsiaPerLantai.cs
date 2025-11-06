using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;


namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPasienMasukKeluarPerUsiaPerLantai : BaseCustomDailyLandscapeA3Rpt
    {
        public LPasienMasukKeluarPerUsiaPerLantai()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            if (reportMaster.ObjectTypeName == "GetPatientVisitPerAgePerFloor")
            {
                List<GetPatientVisitPerAgePerFloor> lstEntity = BusinessLayer.GetPatientVisitPerAgePerFloorList(Convert.ToInt32(param[0]), Convert.ToInt32(param[1]));

                int FemaleOld_28hr = lstEntity.Sum(t => t.FemaleOld_28hr);
                int FemaleOld_1th = lstEntity.Sum(t => t.FemaleOld_1th);
                int FemaleOld_1th_4th = lstEntity.Sum(t => t.FemaleOld_1th_4th);
                int FemaleOld_4th_14th = lstEntity.Sum(t => t.FemaleOld_4th_14th);
                int FemaleOld_14th_24th = lstEntity.Sum(t => t.FemaleOld_14th_24th);
                int FemaleOld_24th_44th = lstEntity.Sum(t => t.FemaleOld_24th_44th);
                int FemaleOld_44th_64th = lstEntity.Sum(t => t.FemaleOld_44th_64th);
                int FemaleOld_64th = lstEntity.Sum(t => t.FemaleOld_64th);
                int TotalFemaleOld = FemaleOld_28hr + FemaleOld_1th + FemaleOld_1th_4th + FemaleOld_4th_14th + FemaleOld_14th_24th + FemaleOld_24th_44th + FemaleOld_44th_64th + FemaleOld_64th;


                int FemaleNew_28hr = lstEntity.Sum(t => t.FemaleNew_28hr);
                int FemaleNew_1th = lstEntity.Sum(t => t.FemaleNew_1th);
                int FemaleNew_1th_4th = lstEntity.Sum(t => t.FemaleNew_1th_4th);
                int FemaleNew_4th_14th = lstEntity.Sum(t => t.FemaleNew_4th_14th);
                int FemaleNew_14th_24th = lstEntity.Sum(t => t.FemaleNew_14th_24th);
                int FemaleNew_24th_44th = lstEntity.Sum(t => t.FemaleNew_24th_44th);
                int FemaleNew_44th_64th = lstEntity.Sum(t => t.FemaleNew_44th_64th);
                int FemaleNew_64th = lstEntity.Sum(t => t.FemaleNew_64th);
                int TotalFemaleNew = FemaleNew_28hr + FemaleNew_1th + FemaleNew_1th_4th + FemaleNew_4th_14th + FemaleNew_14th_24th + FemaleNew_24th_44th + FemaleNew_44th_64th + FemaleNew_64th;

                int MaleOld_28hr = lstEntity.Sum(t => t.MaleOld_28hr);
                int MaleOld_1th = lstEntity.Sum(t => t.MaleOld_1th);
                int MaleOld_1th_4th = lstEntity.Sum(t => t.MaleOld_1th_4th);
                int MaleOld_4th_14th = lstEntity.Sum(t => t.MaleOld_4th_14th);
                int MaleOld_14th_24th = lstEntity.Sum(t => t.MaleOld_14th_24th);
                int MaleOld_24th_44th = lstEntity.Sum(t => t.MaleOld_24th_44th);
                int MaleOld_44th_64th = lstEntity.Sum(t => t.MaleOld_44th_64th);
                int MaleOld_64th = lstEntity.Sum(t => t.MaleOld_64th);
                int TotalMaleOld = MaleOld_28hr + MaleOld_1th + MaleOld_1th_4th + MaleOld_4th_14th + MaleOld_14th_24th + MaleOld_24th_44th + MaleOld_44th_64th + MaleOld_64th;

                int MaleNew_28hr = lstEntity.Sum(t => t.MaleNew_28hr);
                int MaleNew_1th = lstEntity.Sum(t => t.MaleNew_1th);
                int MaleNew_1th_4th = lstEntity.Sum(t => t.MaleNew_1th_4th);
                int MaleNew_4th_14th = lstEntity.Sum(t => t.MaleNew_4th_14th);
                int MaleNew_14th_24th = lstEntity.Sum(t => t.MaleNew_14th_24th);
                int MaleNew_24th_44th = lstEntity.Sum(t => t.MaleNew_24th_44th);
                int MaleNew_44th_64th = lstEntity.Sum(t => t.MaleNew_44th_64th);
                int MaleNew_64th = lstEntity.Sum(t => t.MaleNew_64th);
                int TotalMaleNew = MaleNew_28hr + MaleNew_1th + MaleNew_1th_4th + MaleNew_4th_14th + MaleNew_14th_24th + MaleNew_24th_44th + MaleNew_44th_64th + MaleNew_64th;


                cfemaleold.Text = TotalFemaleOld.ToString();
                cfemalenew.Text = TotalFemaleNew.ToString();
                cmaleold.Text = TotalMaleOld.ToString();
                cmalenew.Text = TotalMaleNew.ToString();
            }
            else if (reportMaster.ObjectTypeName == "GetPatientDischargePerAgePerFloor")
            {
                List<GetPatientDischargePerAgePerFloor> lstEntity = BusinessLayer.GetPatientDischargePerAgePerFloorList(Convert.ToInt32(param[0]), Convert.ToInt32(param[1]));

                int FemaleOld_28hr = lstEntity.Sum(t => t.FemaleOld_28hr);
                int FemaleOld_1th = lstEntity.Sum(t => t.FemaleOld_1th);
                int FemaleOld_1th_4th = lstEntity.Sum(t => t.FemaleOld_1th_4th);
                int FemaleOld_4th_14th = lstEntity.Sum(t => t.FemaleOld_4th_14th);
                int FemaleOld_14th_24th = lstEntity.Sum(t => t.FemaleOld_14th_24th);
                int FemaleOld_24th_44th = lstEntity.Sum(t => t.FemaleOld_24th_44th);
                int FemaleOld_44th_64th = lstEntity.Sum(t => t.FemaleOld_44th_64th);
                int FemaleOld_64th = lstEntity.Sum(t => t.FemaleOld_64th);
                int TotalFemaleOld = FemaleOld_28hr + FemaleOld_1th + FemaleOld_1th_4th + FemaleOld_4th_14th + FemaleOld_14th_24th + FemaleOld_24th_44th + FemaleOld_44th_64th + FemaleOld_64th;


                int FemaleNew_28hr = lstEntity.Sum(t => t.FemaleNew_28hr);
                int FemaleNew_1th = lstEntity.Sum(t => t.FemaleNew_1th);
                int FemaleNew_1th_4th = lstEntity.Sum(t => t.FemaleNew_1th_4th);
                int FemaleNew_4th_14th = lstEntity.Sum(t => t.FemaleNew_4th_14th);
                int FemaleNew_14th_24th = lstEntity.Sum(t => t.FemaleNew_14th_24th);
                int FemaleNew_24th_44th = lstEntity.Sum(t => t.FemaleNew_24th_44th);
                int FemaleNew_44th_64th = lstEntity.Sum(t => t.FemaleNew_44th_64th);
                int FemaleNew_64th = lstEntity.Sum(t => t.FemaleNew_64th);
                int TotalFemaleNew = FemaleNew_28hr + FemaleNew_1th + FemaleNew_1th_4th + FemaleNew_4th_14th + FemaleNew_14th_24th + FemaleNew_24th_44th + FemaleNew_44th_64th + FemaleNew_64th;

                int MaleOld_28hr = lstEntity.Sum(t => t.MaleOld_28hr);
                int MaleOld_1th = lstEntity.Sum(t => t.MaleOld_1th);
                int MaleOld_1th_4th = lstEntity.Sum(t => t.MaleOld_1th_4th);
                int MaleOld_4th_14th = lstEntity.Sum(t => t.MaleOld_4th_14th);
                int MaleOld_14th_24th = lstEntity.Sum(t => t.MaleOld_14th_24th);
                int MaleOld_24th_44th = lstEntity.Sum(t => t.MaleOld_24th_44th);
                int MaleOld_44th_64th = lstEntity.Sum(t => t.MaleOld_44th_64th);
                int MaleOld_64th = lstEntity.Sum(t => t.MaleOld_64th);
                int TotalMaleOld = MaleOld_28hr + MaleOld_1th + MaleOld_1th_4th + MaleOld_4th_14th + MaleOld_14th_24th + MaleOld_24th_44th + MaleOld_44th_64th + MaleOld_64th;

                int MaleNew_28hr = lstEntity.Sum(t => t.MaleNew_28hr);
                int MaleNew_1th = lstEntity.Sum(t => t.MaleNew_1th);
                int MaleNew_1th_4th = lstEntity.Sum(t => t.MaleNew_1th_4th);
                int MaleNew_4th_14th = lstEntity.Sum(t => t.MaleNew_4th_14th);
                int MaleNew_14th_24th = lstEntity.Sum(t => t.MaleNew_14th_24th);
                int MaleNew_24th_44th = lstEntity.Sum(t => t.MaleNew_24th_44th);
                int MaleNew_44th_64th = lstEntity.Sum(t => t.MaleNew_44th_64th);
                int MaleNew_64th = lstEntity.Sum(t => t.MaleNew_64th);
                int TotalMaleNew = MaleNew_28hr + MaleNew_1th + MaleNew_1th_4th + MaleNew_4th_14th + MaleNew_14th_24th + MaleNew_24th_44th + MaleNew_44th_64th + MaleNew_64th;


                cfemaleold.Text = TotalFemaleOld.ToString();
                cfemalenew.Text = TotalFemaleNew.ToString();
                cmaleold.Text = TotalMaleOld.ToString();
                cmalenew.Text = TotalMaleNew.ToString();
            }

            base.InitializeReport(param);
            
        }
    }
}
