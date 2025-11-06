using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientEducationCtl : BasePagePatientPageEntryCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            string id = paramInfo[0];

            if (id != "")
            {
                IsAdd = false;
                hdnID.Value = id;
                PatientEducationHd entity = BusinessLayer.GetPatientEducationHd(Convert.ToInt32(hdnID.Value));
                List<PatientEducationDt> entityDt = BusinessLayer.GetPatientEducationDtList(string.Format("ID = {0}", hdnID.Value));
                EntityToControl(entity, entityDt);
            }
            else
            {
                hdnID.Value = "0";
                IsAdd = true;
            }
        }


        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtObservationDate, new ControlEntrySetting(true, true, true, DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtObservationTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
        }

        private void EntityToControl(PatientEducationHd entity, List<PatientEducationDt> lstEntityDt)
        {
            txtObservationDate.Text = entity.EducationDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtObservationTime.Text = entity.EducationTime;

            #region Patient Education Dt
            foreach (RepeaterItem item in rptEducationType.Items)
            {
                HtmlInputHidden hdnGCEducationType = (HtmlInputHidden)item.FindControl("hdnGCEducationType");
                RadioButtonList rblEducationType = (RadioButtonList)item.FindControl("rblEducationTypeStatus");

                PatientEducationDt entityDt = lstEntityDt.FirstOrDefault(p => p.GCPatientEducationType == hdnGCEducationType.Value);
                if (entityDt != null)
                {
                    TextBox txtFreeText = (TextBox)item.FindControl("txtFreeText");
                    txtFreeText.Text = entityDt.Remarks; 
                    ///rblEducationType.SelectedValue = !string.IsNullOrEmpty(entityDt.Remarks) ? "1" : "0";
                    string SelectedValue = "0"; 
                    if (entityDt.IsEducationTypeStatus) {
                        SelectedValue = "1"; 
                    }
                    rblEducationType.SelectedValue = SelectedValue;
                }
            }
            #endregion

        }

        private void ControlToEntity(PatientEducationHd entity, List<PatientEducationDt> lstEntityDt)
        {
            entity.EducationDate = Helper.GetDatePickerValue(txtObservationDate);
            entity.EducationTime = txtObservationTime.Text;
            entity.ParamedicID = AppSession.UserLogin.ParamedicID;

            #region Patient Education Dt
            foreach (RepeaterItem item in rptEducationType.Items)
            {
                HtmlInputHidden hdnGCEducationType = (HtmlInputHidden)item.FindControl("hdnGCEducationType");

                RadioButtonList rblEducationType = (RadioButtonList)item.FindControl("rblEducationTypeStatus");
                TextBox txtFreeText = (TextBox)item.FindControl("txtFreeText");
                if (rblEducationType.SelectedIndex > -1)
                {
                    if (rblEducationType.SelectedValue == "1")
                    {
                        PatientEducationDt dt = new PatientEducationDt();
                        dt.GCPatientEducationType = hdnGCEducationType.Value;
                        dt.Remarks = txtFreeText.Text;
                        dt.IsEducationTypeStatus = true;
                        lstEntityDt.Add(dt);
                    }
                    else if(rblEducationType.SelectedValue == "0") {
                        PatientEducationDt dt = new PatientEducationDt();
                        dt.GCPatientEducationType = hdnGCEducationType.Value;
                        dt.Remarks = null;
                        dt.IsEducationTypeStatus = false;
                        lstEntityDt.Add(dt);
                    }
                }
            }
            #endregion
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;

            if (AppSession.UserLogin.ParamedicID == null)
            {
                errMessage = string.Format("{0}<br/><br/><i>{1}</i>", "Field Tenaga Medis tidak boleh dikosongkan", "User Login saat ini tidak terhubung dengan kode tenaga medis");
                result = false;
                return result;
            }

            IDbContext ctx = DbFactory.Configure(true);
            PatientEducationHdDao entityDao = new PatientEducationHdDao(ctx);
            PatientEducationDtDao entityDtDao = new PatientEducationDtDao(ctx);
            try
            {
                PatientEducationHd entity = new PatientEducationHd();
                List<PatientEducationDt> lstEntityDt = new List<PatientEducationDt>();

                ControlToEntity(entity, lstEntityDt);
                entity.VisitID = AppSession.RegisteredPatient.VisitID;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                int id = entityDao.InsertReturnPrimaryKeyID(entity);

                foreach (PatientEducationDt entityDt in lstEntityDt)
                {
                    entityDt.ID = id;
                    entityDtDao.Insert(entityDt);
                }

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                errMessage = string.Format("{0}<br/><br/><i>{1}</i>", ex.Message, ex.Source);
                result = false;
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientEducationHdDao entityDao = new PatientEducationHdDao(ctx);
            PatientEducationDtDao entityDtDao = new PatientEducationDtDao(ctx);
            try
            {
                PatientEducationHd entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                List<PatientEducationDt> lstEntityDt = BusinessLayer.GetPatientEducationDtList(string.Format("ID = {0}", hdnID.Value), ctx);
                List<PatientEducationDt> lstNewEntityDt = new List<PatientEducationDt>();

                ControlToEntity(entity, lstNewEntityDt);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                entityDao.Update(entity);

                foreach (PatientEducationDt entityDt in lstNewEntityDt)
                {
                    PatientEducationDt obj = lstEntityDt.FirstOrDefault(p => p.GCPatientEducationType == entityDt.GCPatientEducationType);
                    entityDt.ID = entity.ID;
                    if (obj == null)
                        entityDtDao.Insert(entityDt);
                    else
                        entityDtDao.Update(entityDt);
                }

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                errMessage = string.Format("{0}<br/><br/><i>{1}</i>", ex.Message, ex.Source);
                result = false;
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            List<StandardCode> lst = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.PATIENT_EDUCATION_TYPE));

            rptEducationType.DataSource = lst;
            rptEducationType.DataBind();
        }

    }
}