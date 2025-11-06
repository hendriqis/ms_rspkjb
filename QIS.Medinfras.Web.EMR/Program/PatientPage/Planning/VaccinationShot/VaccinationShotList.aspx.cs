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
using System.Drawing;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class VaccinationShotList : BasePagePatientPageList
    {
        private int[] lstVaccinationBlock = { 0, 1, 2, 3, 4, 5, 6, 9, 12, 15, 18, 24, 36, 60, 72, 84, 96, 120, 144, 216 };
        private List<vVaccinationShotDt> lstVaccinationShotDt;
        private List<VaccinationPeriod> lstVaccinationPeriod = null;
        public override string OnGetMenuCode()
        {
            if (Page.Request.QueryString.Count > 0)
                return Constant.MenuCode.EMR.HEALTH_RECORD_VACCINATION_SHOT;
            return Constant.MenuCode.EMR.VACCINATION_SHOT;
        }

        public override bool IsEntryUsePopup()
        {
            return false;
        }

        protected override void InitializeDataControl()
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

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView();
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridView();
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected override bool OnAddRecord(ref string url, ref string errMessage)
        {
            if (Page.Request.QueryString.Count > 0)
                url = ResolveUrl("~/Program/PatientPage/Planning/VaccinationShot/VaccinationShotHistoryEntry.aspx");
            else
                url = ResolveUrl("~/Program/PatientPage/Planning/VaccinationShot/VaccinationShotEntry.aspx");
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            if (hdnID.Value != "")
            {
                VaccinationShotHd entityHd = BusinessLayer.GetVaccinationShotHd(Convert.ToInt32(hdnID.Value));
                if (Page.Request.QueryString.Count > 0)
                {
                    if (!entityHd.IsInternal)
                        url = ResolveUrl(string.Format("~/Program/PatientPage/Planning/VaccinationShot/VaccinationShotHistoryEntry.aspx?id={0}", hdnID.Value));
                    else
                    {
                        errMessage = "Only Allow Edit Record Created From Health Record";
                        return false;
                    }
                }
                else
                {
                    if (entityHd.IsInternal)
                    {
                        if (entityHd.ParamedicID == AppSession.RegisteredPatient.ParamedicID)
                            url = ResolveUrl(string.Format("~/Program/PatientPage/Planning/VaccinationShot/VaccinationShotEntry.aspx?id={0}", hdnID.Value));
                        else
                        {
                            errMessage = "Cannot Edit Record Created From Other Physician";
                            return false;
                        }
                    }
                    else
                    {
                        errMessage = "Cannot Edit Record Created From Health Record";
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value != "")
            {
                VaccinationShotHd entity = BusinessLayer.GetVaccinationShotHd(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateVaccinationShotHd(entity);

                List<VaccinationShotDt> lstEntityDt = BusinessLayer.GetVaccinationShotDtList(string.Format("HeaderID = {0} AND IsDeleted = 0", entity.ID));
                if (lstEntityDt.Count > 0)
                {
                    foreach (VaccinationShotDt dt in lstEntityDt)
                    {
                        dt.IsDeleted = true;
                        dt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        dt.LastUpdatedDate = DateTime.Now;
                        BusinessLayer.UpdateVaccinationShotDt(dt);
                    }
                }

                return true;
            }
            return false;
        }
    }
}