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
using System.Globalization;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.EmergencyCare.Program
{
    public partial class ERPhysicalExam : BasePagePatientPageList
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EmergencyCare.PHYSICAL_EXAMINATION;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        protected override void InitializeDataControl()
        {
            ctlToolbar.SetSelectedMenu(3);

            txtObservationDate.Text = AppSession.RegisteredPatient.VisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtObservationTime.Text = AppSession.RegisteredPatient.VisitTime;

            Helper.SetControlEntrySetting(txtObservationDate, new ControlEntrySetting(true, true, true), "mpPhysicalExam");
            Helper.SetControlEntrySetting(txtObservationTime, new ControlEntrySetting(true, true, true), "mpPhysicalExam");

            List<vReviewOfSystemHd> lstvReviewOfSystemHd = BusinessLayer.GetvReviewOfSystemHdList(string.Format("VisitID = '{0}'", AppSession.RegisteredPatient.VisitID));
            if (lstvReviewOfSystemHd.Count > 0)
            {
                vReviewOfSystemHd entity = lstvReviewOfSystemHd.FirstOrDefault();
                hdnID.Value = entity.ID.ToString();
                List<vReviewOfSystemDt> entityDt = BusinessLayer.GetvReviewOfSystemDtList(string.Format("ID = {0}", entity.ID));
                EntityToControl(entity, entityDt);
            }
            else
                hdnID.Value = "";
        }

        private void EntityToControl(vReviewOfSystemHd entity, List<vReviewOfSystemDt> lstEntityDt)
        {
            txtObservationDate.Text = entity.ObservationDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtObservationTime.Text = entity.ObservationTime;

            #region Review Of System Dt
            foreach (RepeaterItem item in rptReviewOfSystem.Items)
            {
                HtmlInputHidden hdnGCROSystem = (HtmlInputHidden)item.FindControl("hdnGCROSystem");

                vReviewOfSystemDt entityDt = lstEntityDt.FirstOrDefault(p => p.GCROSystem == hdnGCROSystem.Value);
                if (entityDt != null)
                {
                    RadioButtonList rblReviewOfSystem = (RadioButtonList)item.FindControl("rblReviewOfSystem");
                    TextBox txtFreeText = (TextBox)item.FindControl("txtFreeText");
                    if (entityDt.IsNormal)
                        rblReviewOfSystem.SelectedValue = "2";
                    else if (entityDt.Remarks != "")
                        rblReviewOfSystem.SelectedValue = "3";
                    else
                        rblReviewOfSystem.SelectedValue = "1";
                    txtFreeText.Text = entityDt.Remarks;
                }
            }
            #endregion
        }

        private void ControlToEntity(ReviewOfSystemHd entity, List<ReviewOfSystemDt> lstEntityDt)
        {
            entity.ObservationDate = Helper.GetDatePickerValue(txtObservationDate);
            entity.ObservationTime = txtObservationTime.Text;

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
                        dt.Remarks = txtFreeText.Text;
                    dt.IsNormal = false;
                }
                if (rblReviewOfSystem.SelectedIndex > -1)
                    lstEntityDt.Add(dt);
            }
            #endregion
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            List<StandardCode> lst = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive=1", Constant.StandardCode.REVIEW_OF_SYSTEM));

            rptReviewOfSystem.DataSource = lst;
            rptReviewOfSystem.DataBind();
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            if (type == "save")
            {
                bool result = true;
                IDbContext ctx = DbFactory.Configure(true);
                ReviewOfSystemHdDao entityDao = new ReviewOfSystemHdDao(ctx);
                ReviewOfSystemDtDao entityDtDao = new ReviewOfSystemDtDao(ctx);
                try
                {
                    if (hdnID.Value != "")
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
                    }
                    else
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
                    
                    }
                    ctx.CommitTransaction();
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    result = false;
                    ctx.RollBackTransaction();
                    txtObservationDate.Text = "";
                }
                finally
                {
                    ctx.Close();
                }
                return result;
            }
            return false;
        }
    }
}