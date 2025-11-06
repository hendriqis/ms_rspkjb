using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class VaccinationScheduleInfo : BasePage
    {
        private List<VaccinationPeriod> lstVaccinationPeriod = null;
        private int[] listAgeInMonth = { 0, 1, 2, 3, 4, 5, 6, 9, 12, 15, 18, 24, 36, 48, 60, 72, 84, 96, 108, 120, 132, 144, 156, 168, 180, 192, 204, 216 };
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                lstVaccinationPeriod = BusinessLayer.GetVaccinationPeriodList("");

                lvwView.DataSource = BusinessLayer.GetvChildrenVaccinationList("1 = 1 ORDER BY DisplayOrder");
                lvwView.DataBind();
            }
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
                        for (int j = i + 1; j < lstPeriod.Count; ++j)
                        {
                            VaccinationPeriod nextPeriod = lstPeriod[j];
                            if (nextPeriod.PeriodLabel == period.PeriodLabel && nextPeriod.AgeInMonth == listAgeInMonth[idxAgeInMonth + ctr]
                                && nextPeriod.IsOptimum == period.IsOptimum
                                && nextPeriod.IsCatchup == period.IsCatchup
                                && nextPeriod.IsBooster == period.IsBooster
                                && nextPeriod.IsEndemic == period.IsEndemic
                                && nextPeriod.IsHighRisk == period.IsHighRisk)
                            {
                                if (nextPeriod.VaccinationNo == "")
                                {
                                    colspan++;
                                    HtmlTableCell deletedCol = (HtmlTableCell)e.Item.FindControl("tdCol" + nextPeriod.AgeInMonth);
                                    deletedCol.Visible = false;
                                    ctr++;
                                }
                                else
                                {
                                    j--;
                                    break;
                                }
                            }
                            else
                            {
                                j--;
                                break;
                            }
                        }
                        tdCol.ColSpan = colspan;
                    }
                    if (period.IsOptimum)
                        tdCol.Style.Add("background-color", "#89e1fb");
                    else if (period.IsCatchup)
                        tdCol.Style.Add("background-color", "#fbf489");
                    else if (period.IsBooster)
                        tdCol.Style.Add("background-color", "#89fb89");
                    else if (period.IsEndemic)
                        tdCol.Style.Add("background-color", "#fb89fb");
                    else if (period.IsHighRisk)
                        tdCol.Style.Add("background-color", "#ffa233");
                }
            }
        }
    }
}