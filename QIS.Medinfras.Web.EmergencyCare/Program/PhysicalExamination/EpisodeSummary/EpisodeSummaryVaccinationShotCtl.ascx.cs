using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using System.Text;
using System.Drawing;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.Emergen.Program
{
    public partial class EpisodeSummaryVaccinationShotCtl : BaseViewPopupCtl
    {
        private int[] lstVaccinationBlock = { 0, 1, 2, 3, 4, 5, 6, 9, 12, 15, 18, 24, 36, 60, 72, 84, 96, 120, 144, 216 };
        private List<vVaccinationShotDt> lstVaccinationShotDt;
        private List<VaccinationPeriod> lstVaccinationPeriod = null;

        public override void InitializeDataControl(string paneButtonCode)
        {
            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("MRN = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.MRN);
            lstVaccinationShotDt = BusinessLayer.GetvVaccinationShotDtList(filterExpression);
            lstVaccinationPeriod = BusinessLayer.GetVaccinationPeriodList("");

            filterExpression = "IsDeleted = 0";
            List<VaccinationType> lstEntity = BusinessLayer.GetVaccinationTypeList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                VaccinationType obj = (VaccinationType)e.Row.DataItem;
                e.Row.Cells[1].BackColor = Color.FromName(obj.DisplayColor);

                Repeater rptVaccinationShotDt = e.Row.FindControl("rptVaccinationShotDt") as Repeater;
                rptVaccinationShotDt.DataSource = lstVaccinationShotDt.Where(p => p.VaccinationTypeID == obj.VaccinationTypeID);
                rptVaccinationShotDt.DataBind();
            }
        }

        protected void rptVaccinationShotDt_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlImage imgVaccinationStatus = e.Item.FindControl("imgVaccinationStatus") as HtmlImage;

                vVaccinationShotDt obj = (vVaccinationShotDt)e.Item.DataItem;
                int ageInMonth = obj.AgeInYear * 12 + obj.AgeInMonth;
                VaccinationPeriod vaccinationPeriod = lstVaccinationPeriod.FirstOrDefault(p => p.VaccinationTypeID == obj.VaccinationTypeID && p.VaccinationNo == obj.VaccinationNo);
                if (vaccinationPeriod != null)
                {
                    int idx = Array.IndexOf(lstVaccinationBlock, vaccinationPeriod.AgeInMonth);
                    if (idx < lstVaccinationBlock.Length - 1)
                    {
                        if (ageInMonth >= lstVaccinationBlock[idx + 1])
                            imgVaccinationStatus.Src = ResolveUrl("~/Libs/Images/Status/PatientStatus_3.png");
                        else
                            imgVaccinationStatus.Src = ResolveUrl("~/Libs/Images/Status/PatientStatus_4.png");
                    }
                    else
                        imgVaccinationStatus.Src = ResolveUrl("~/Libs/Images/Status/PatientStatus_4.png");
                }
                else
                    imgVaccinationStatus.Src = ResolveUrl("~/Libs/Images/Status/PatientStatus_4.png");
            }
        }
    }
}