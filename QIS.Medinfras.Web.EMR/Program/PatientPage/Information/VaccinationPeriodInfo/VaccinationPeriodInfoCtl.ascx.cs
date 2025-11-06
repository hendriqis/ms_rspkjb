using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class VaccinationPeriodInfoCtl : BaseViewPopupCtl
    {
        private List<VaccinationPeriod> lstVaccinationPeriod = null;
        private int[] listAgeInMonth = { 0, 1, 2, 3, 4, 5, 6, 9, 12, 15, 18, 24, 36, 60, 72, 84, 96, 120, 144, 216 };
        public override void InitializeDataControl(string param)
        {
            lstVaccinationPeriod = BusinessLayer.GetVaccinationPeriodList("");

            lvwView.DataSource = BusinessLayer.GetvChildrenVaccinationList("1 = 1 ORDER BY DisplayOrder");
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vChildrenVaccination obj = (vChildrenVaccination)e.Item.DataItem;
                List<VaccinationPeriod> lstPeriod = lstVaccinationPeriod.Where(p => p.VaccinationTypeID == obj.VaccinationTypeID).OrderBy(p => p.AgeInMonth).ToList();
                for (int i = 0; i < lstPeriod.Count; ++i)
                {
                    VaccinationPeriod period = lstPeriod[i];
                    HtmlTableCell tdCol = (HtmlTableCell)e.Item.FindControl("tdCol" + period.AgeInMonth);
                    if (period.VaccinationNo != "")
                        tdCol.InnerHtml = period.VaccinationNo;
                    else
                    {
                        tdCol.InnerHtml = period.PeriodLabel;
                        int idxAgeInMonth = Array.IndexOf(listAgeInMonth, period.AgeInMonth);
                        int colspan = 1;
                        int ctr = 1;
                        for (i = i + 1; i < lstPeriod.Count; ++i)
                        {
                            VaccinationPeriod nextPeriod = lstPeriod[i];
                            if (nextPeriod.PeriodLabel == period.PeriodLabel && nextPeriod.AgeInMonth == listAgeInMonth[idxAgeInMonth + ctr])
                            {
                                colspan++;
                                HtmlTableCell deletedCol = (HtmlTableCell)e.Item.FindControl("tdCol" + nextPeriod.AgeInMonth);
                                deletedCol.Visible = false;
                                ctr++;
                            }
                            else
                            {
                                i--;
                                break;
                            }
                        }
                        tdCol.ColSpan = colspan;
                    }
                    tdCol.Style.Add("background-color", obj.DisplayColor);
                }
            }
        }
    }
}