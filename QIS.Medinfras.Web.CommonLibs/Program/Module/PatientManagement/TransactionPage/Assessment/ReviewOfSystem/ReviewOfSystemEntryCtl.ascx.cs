using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.CommonLibs.Program.PatientPage
{
    public partial class ReviewOfSystemEntryCtl : BasePagePatientPageEntryCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                IsAdd = false;
                hdnID.Value = param;
                ReviewOfSystemHd entity = BusinessLayer.GetReviewOfSystemHd(Convert.ToInt32(hdnID.Value));
                List<ReviewOfSystemDt> entityDt = BusinessLayer.GetReviewOfSystemDtList(string.Format("ID = {0}", param));
                EntityToControl(entity, entityDt);
            }
            else
            {
                hdnID.Value = "";
                IsAdd = true;
            }
        }


        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtObservationDate, new ControlEntrySetting(true, true, true, DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtObservationTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
        }

        private void EntityToControl(ReviewOfSystemHd entity, List<ReviewOfSystemDt> lstEntityDt)
        {
            txtObservationDate.Text = entity.ObservationDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtObservationTime.Text = entity.ObservationTime;

            #region Review Of System Dt
            foreach (RepeaterItem item in rptReviewOfSystem.Items)
            {
                HtmlInputHidden hdnGCROSystem = (HtmlInputHidden)item.FindControl("hdnGCROSystem");

                ReviewOfSystemDt entityDt = lstEntityDt.FirstOrDefault(p => p.GCROSystem == hdnGCROSystem.Value);
                if (entityDt != null)
                {
                    RadioButtonList rblReviewOfSystem = (RadioButtonList)item.FindControl("rblReviewOfSystem");
                    TextBox txtFreeText = (TextBox)item.FindControl("txtFreeText");
                    if (entityDt.IsNotExamined)
                        rblReviewOfSystem.SelectedValue = "1";
                    else if (entityDt.IsNormal)
                        rblReviewOfSystem.SelectedValue = "2";
                    else if (entityDt.IsOther)
                        rblReviewOfSystem.SelectedValue = "4";
                    else
                        rblReviewOfSystem.SelectedValue = "3";
                    txtFreeText.Text = entityDt.Remarks;
                }
            }
            #endregion

        }

        private void ControlToEntity(ReviewOfSystemHd entity, List<ReviewOfSystemDt> lstEntityDt)
        {
            entity.ObservationDate = Helper.GetDatePickerValue(txtObservationDate);
            entity.ObservationTime = txtObservationTime.Text;
            entity.ParamedicID = AppSession.UserLogin.ParamedicID;

            #region Review Of System Dt
            foreach (RepeaterItem item in rptReviewOfSystem.Items)
            {
                HtmlInputHidden hdnGCROSystem = (HtmlInputHidden)item.FindControl("hdnGCROSystem");

                RadioButtonList rblReviewOfSystem = (RadioButtonList)item.FindControl("rblReviewOfSystem");
                TextBox txtFreeText = (TextBox)item.FindControl("txtFreeText");
                ReviewOfSystemDt dt = new ReviewOfSystemDt();
                dt.GCROSystem = hdnGCROSystem.Value;
                if (rblReviewOfSystem.SelectedValue == "2")
                    dt.IsNormal = true;
                else
                {
                    if (rblReviewOfSystem.SelectedValue == "3")
                        dt.IsNormal = false;
                    else if (rblReviewOfSystem.SelectedValue == "4")
                        dt.IsOther = true;
                    else
                        dt.IsNotExamined = true;
                }
                dt.Remarks = txtFreeText.Text;
                if (rblReviewOfSystem.SelectedIndex > -1)
                    lstEntityDt.Add(dt);
            }
            #endregion
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = false;
            if (OnValidateDateEntry(ref errMessage))
            {
                IDbContext ctx = DbFactory.Configure(true);
                ReviewOfSystemHdDao entityDao = new ReviewOfSystemHdDao(ctx);
                ReviewOfSystemDtDao entityDtDao = new ReviewOfSystemDtDao(ctx);
                try
                {
                    ReviewOfSystemHd entity = new ReviewOfSystemHd();
                    List<ReviewOfSystemDt> lstEntityDt = new List<ReviewOfSystemDt>();
                    ControlToEntity(entity, lstEntityDt);
                    entity.VisitID = AppSession.RegisteredPatient.VisitID;
                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    entityDao.Insert(entity);

                    entity.ID = BusinessLayer.GetReviewOfSystemHdMaxID(ctx);

                    foreach (ReviewOfSystemDt entityDt in lstEntityDt)
                    {
                        entityDt.ID = entity.ID;
                        entityDtDao.Insert(entityDt);
                    }

                    ctx.CommitTransaction();
                    result = true;
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    result = false;
                    ctx.RollBackTransaction();
                }
                finally
                {
                    ctx.Close();
                }
            }
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = false;
            if (OnValidateDateEntry(ref errMessage))
            {
                IDbContext ctx = DbFactory.Configure(true);
                ReviewOfSystemHdDao entityDao = new ReviewOfSystemHdDao(ctx);
                ReviewOfSystemDtDao entityDtDao = new ReviewOfSystemDtDao(ctx);
                try
                {
                    ReviewOfSystemHd entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                    List<ReviewOfSystemDt> lstEntityDt = BusinessLayer.GetReviewOfSystemDtList(string.Format("ID = {0}", hdnID.Value), ctx);
                    List<ReviewOfSystemDt> lstNewEntityDt = new List<ReviewOfSystemDt>();

                    ControlToEntity(entity, lstNewEntityDt);
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                    entityDao.Update(entity);

                    foreach (ReviewOfSystemDt entityDt in lstNewEntityDt)
                    {
                        ReviewOfSystemDt obj = lstEntityDt.FirstOrDefault(p => p.GCROSystem == entityDt.GCROSystem);
                        entityDt.ID = entity.ID;
                        if (obj == null)
                            entityDtDao.Insert(entityDt);
                        else
                            entityDtDao.Update(entityDt);
                    }

                    result = true;
                    ctx.CommitTransaction();
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    result = false;
                    ctx.RollBackTransaction();
                }
                finally
                {
                    ctx.Close();
                }
            }
            return result;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            string specialtyID = AppSession.UserLogin.SpecialtyID;
            if (string.IsNullOrEmpty(specialtyID))
                specialtyID = AppSession.RegisteredPatient.SpecialtyID;

            List<vSpecialtyROS> lst1 = BusinessLayer.GetvSpecialtyROSList(string.Format("SpecialtyID = '{0}' ORDER BY TagProperty", specialtyID));
            if (lst1.Count == 0)
            {
                List<StandardCode> lst2 = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0 ORDER BY TagProperty", Constant.StandardCode.REVIEW_OF_SYSTEM));
                rptReviewOfSystem.DataSource = lst2;
            }
            else
            {
                rptReviewOfSystem.DataSource = lst1;
            }
            
            rptReviewOfSystem.DataBind();
        }

        private bool OnValidateDateEntry(ref string errMessage)
        {
            bool result = true;
            DateTime logDate = Helper.GetDatePickerValue(txtObservationDate);
            if (logDate < AppSession.RegisteredPatient.VisitDate)
            {
                errMessage = "Tanggal pemeriksaan tidak boleh lebih kecil dari tanggal kunjungan";
                result = false;
            }
            return result;
        }
    }
}