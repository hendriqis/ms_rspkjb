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
    public partial class PatientEducationListCtl : BasePagePatientPageEntryCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                IsAdd = false;
                hdnID.Value = param;
                PatientEducationHd entity = BusinessLayer.GetPatientEducationHd(Convert.ToInt32(hdnID.Value));
                List<PatientEducationDt> entityDt = BusinessLayer.GetPatientEducationDtList(string.Format("ID = {0}", param));
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

            int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("ParamedicID = {0}", paramedicID));
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");

            if (IsAdd)
            {
                if (AppSession.UserLogin.ParamedicID != 0 && AppSession.UserLogin.ParamedicID != null)
                {
                    cboParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
                    cboParamedicID.ClientEnabled = false;
                }
                else
                {
                    cboParamedicID.SelectedIndex = 0;
                }
            }

            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.EDUCATION_UNDERSTANDING_LEVEL, Constant.StandardCode.EDUCATION_METHOD, Constant.StandardCode.EDUCATION_MATERIAL, Constant.StandardCode.EDUCATION_EVALUATION, Constant.StandardCode.FAMILY_RELATION);
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(filterExpression);
            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField(cboGCUnderstandingLevel, lstSc.Where(p => p.ParentID == Constant.StandardCode.EDUCATION_UNDERSTANDING_LEVEL || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboGCEducationMethod, lstSc.Where(p => p.ParentID == Constant.StandardCode.EDUCATION_METHOD || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboGCEducationMaterial, lstSc.Where(p => p.ParentID == Constant.StandardCode.EDUCATION_MATERIAL || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboGCEducationEvaluation, lstSc.Where(p => p.ParentID == Constant.StandardCode.EDUCATION_EVALUATION || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboFamilyRelation, lstSc.Where(p => p.ParentID == Constant.StandardCode.FAMILY_RELATION || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
        }

        private void EntityToControl(PatientEducationHd entity, List<PatientEducationDt> lstEntityDt)
        {
            txtObservationDate.Text = entity.EducationDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtObservationTime.Text = entity.EducationTime;
            cboParamedicID.Value = entity.ParamedicID.ToString();
            rblIsPatientFamily.SelectedValue = entity.IsPatientFamily ? "1" : "0";
            cboGCUnderstandingLevel.Value = entity.GCUnderstandingLevel;
            cboGCEducationMaterial.Value = entity.GCEducationMaterial;
            cboGCEducationMethod.Value = entity.GCEducationMethod;
            cboGCEducationEvaluation.Value = entity.GCEducationEvaluation;
            trFamilyInfo.Visible = entity.IsPatientFamily;
            if (entity.IsPatientFamily)
                trFamilyInfo.Style.Add("display", "table-row");
            else
                trFamilyInfo.Style.Add("display", "none");
            cboFamilyRelation.Value = entity.GCFamilyRelation;
            txtSignature2Name.Text = entity.SignatureName2;


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

                    rblEducationType.SelectedValue = !string.IsNullOrEmpty(entityDt.Remarks) ? "1" : "0";
                }
            }
            #endregion

        }

        private void ControlToEntity(PatientEducationHd entity, List<PatientEducationDt> lstEntityDt)
        {
            entity.EducationDate = Helper.GetDatePickerValue(txtObservationDate);
            entity.EducationTime = txtObservationTime.Text;
            entity.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
            entity.SignatureName1 = cboParamedicID.Text;
            entity.IsPatientFamily = rblIsPatientFamily.SelectedValue == "1" ? true : false;
            entity.SignatureName2 = rblIsPatientFamily.SelectedValue == "1" ? txtSignature2Name.Text : AppSession.RegisteredPatient.PatientName;
            if (entity.IsPatientFamily)
            {
                if (cboFamilyRelation.Value != null)
                {
                    if (!string.IsNullOrEmpty(cboFamilyRelation.Value.ToString()))
                        entity.GCFamilyRelation = cboFamilyRelation.Value.ToString();
                }
            }
            else
            {
                entity.GCFamilyRelation = null;
            }

            if (cboGCUnderstandingLevel.Value != null)
            {
                if (!string.IsNullOrEmpty(cboGCUnderstandingLevel.Value.ToString()))
                    entity.GCUnderstandingLevel = cboGCUnderstandingLevel.Value.ToString();
            }
            if (cboGCEducationMethod.Value != null)
            {
                if (!string.IsNullOrEmpty(cboGCEducationMethod.Value.ToString()))
                    entity.GCEducationMethod = cboGCEducationMethod.Value.ToString();
            }
            if (cboGCEducationMaterial.Value != null)
            {
                if (!string.IsNullOrEmpty(cboGCEducationMaterial.Value.ToString()))
                    entity.GCEducationMaterial = cboGCEducationMaterial.Value.ToString();
            }
            if (cboGCEducationEvaluation.Value != null)
            {
                if (!string.IsNullOrEmpty(cboGCEducationEvaluation.Value.ToString()))
                    entity.GCEducationEvaluation = cboGCEducationEvaluation.Value.ToString();
            }

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