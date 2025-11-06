using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class VitalSignEntryCtl : BasePagePatientPageEntryCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                IsAdd = false;
                hdnID.Value = param;
                SetControlProperties();
                VitalSignHd entity = BusinessLayer.GetVitalSignHd(Convert.ToInt32(hdnID.Value));
                List<vVitalSignDt> entityDt = BusinessLayer.GetvVitalSignDtList(string.Format("ID = {0}", param));
                EntityToControl(entity, entityDt);
            }
            else
            {
                hdnID.Value = "";
                IsAdd = true;
                SetControlProperties();
            }
        }

        private void SetControlProperties()
        {
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType = '{0}' AND ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {1})", Constant.ParamedicType.Nurse, AppSession.RegisteredPatient.HealthcareServiceUnitID));
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");

            if (IsAdd)
            {
                if (AppSession.UserLogin.ParamedicID != 0)
                {
                    cboParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
                }
                else cboParamedicID.SelectedIndex = 0;


                Registration oRegistration = BusinessLayer.GetRegistration(AppSession.RegisteredPatient.RegistrationID);
                if (oRegistration != null)
                {
                    chkIsFallRisk.Checked = oRegistration.IsFallRisk;
                    chkIsDNR.Checked = oRegistration.IsDNR;
                }
            }
            else
            {
                if (AppSession.UserLogin.GCParamedicMasterType != Constant.ParamedicType.Nurse)
                {
                    int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                    cboParamedicID.ClientEnabled = false;
                    cboParamedicID.Value = userLoginParamedic.ToString();
                }
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtObservationDate, new ControlEntrySetting(true, true, true, AppSession.RegisteredPatient.VisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtObservationTime, new ControlEntrySetting(true, true, true, AppSession.RegisteredPatient.VisitTime));
            SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(chkIsFallRisk, new ControlEntrySetting(true, true, false, false));
            SetControlEntrySetting(chkIsDNR, new ControlEntrySetting(true, true, false, false));
        }

        private void EntityToControl(VitalSignHd entity, List<vVitalSignDt> lstEntityDt)
        {
            txtObservationDate.Text = entity.ObservationDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtObservationTime.Text = entity.ObservationTime;
            if (AppSession.UserLogin.ParamedicID != null) cboParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
            else cboParamedicID.Value = entity.ParamedicID.ToString();

            #region Vital Sign Dt
            foreach (RepeaterItem item in rptVitalSign.Items)
            {
                HtmlInputHidden hdnVitalSignID = (HtmlInputHidden)item.FindControl("hdnVitalSignID");
                HtmlInputHidden hdnVitalSignType = (HtmlInputHidden)item.FindControl("hdnVitalSignType");

                vVitalSignDt entityDt = lstEntityDt.FirstOrDefault(p => p.VitalSignID == Convert.ToInt32(hdnVitalSignID.Value));
                if (entityDt != null)
                {
                    if (hdnVitalSignType.Value == Constant.ControlType.TEXT_BOX)
                    {
                        TextBox txt = (TextBox)item.FindControl("txtVitalSignType");
                        txt.Text = entityDt.VitalSignValue;
                    }
                    else if (hdnVitalSignType.Value == Constant.ControlType.COMBO_BOX)
                    {
                        ASPxComboBox ddl = (ASPxComboBox)item.FindControl("cboVitalSignType");
                        ddl.Value = entityDt.GCVitalSignValue;
                    }
                }
            }
            #endregion

            Registration oRegistration = BusinessLayer.GetRegistration(AppSession.RegisteredPatient.RegistrationID);
            if (oRegistration != null)
            {
                chkIsFallRisk.Checked = oRegistration.IsFallRisk;
                chkIsDNR.Checked = oRegistration.IsDNR;
            }
        }

        private void ControlToEntity(VitalSignHd entity, List<VitalSignDt> lstEntityDt)
        {
            entity.ObservationDate = Helper.GetDatePickerValue(txtObservationDate);
            entity.ObservationTime = txtObservationTime.Text;

            #region Vital Sign Dt
            foreach (RepeaterItem item in rptVitalSign.Items)
            {
                HtmlInputHidden hdnVitalSignID = (HtmlInputHidden)item.FindControl("hdnVitalSignID");
                HtmlInputHidden hdnVitalSignType = (HtmlInputHidden)item.FindControl("hdnVitalSignType");
                if (hdnVitalSignType.Value == Constant.ControlType.TEXT_BOX)
                {
                    TextBox txt = (TextBox)item.FindControl("txtVitalSignType");
                    VitalSignDt entityDt = new VitalSignDt();
                    entityDt.VitalSignID = Convert.ToInt32(hdnVitalSignID.Value);
                    entityDt.VitalSignValue = txt.Text;
                    if (entityDt.VitalSignValue != "")
                        lstEntityDt.Add(entityDt);
                }
                else if (hdnVitalSignType.Value == Constant.ControlType.COMBO_BOX)
                {
                    ASPxComboBox ddl = (ASPxComboBox)item.FindControl("cboVitalSignType");
                    VitalSignDt entityDt = new VitalSignDt();
                    if (ddl.Value != null)
                    {
                        entityDt.VitalSignID = Convert.ToInt32(hdnVitalSignID.Value);
                        entityDt.VitalSignValue = ddl.Value.ToString();
                        if (entityDt.VitalSignValue != "")
                            lstEntityDt.Add(entityDt);
                    }
                }
            }
            #endregion
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            VitalSignHdDao entityDao = new VitalSignHdDao(ctx);
            VitalSignDtDao entityDtDao = new VitalSignDtDao(ctx);
            RegistrationDao regDao = new RegistrationDao(ctx);
            try
            {
                VitalSignHd entity = new VitalSignHd();
                List<VitalSignDt> lstEntityDt = new List<VitalSignDt>();
                ControlToEntity(entity, lstEntityDt);
                entity.VisitID = AppSession.RegisteredPatient.VisitID;
                entity.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);

                entity.ID = BusinessLayer.GetVitalSignHdMaxID(ctx);

                foreach (VitalSignDt entityDt in lstEntityDt)
                {
                    entityDt.ID = entity.ID;
                    entityDtDao.Insert(entityDt);
                }

                Registration oRegistration = regDao.Get(AppSession.RegisteredPatient.RegistrationID);
                if (oRegistration != null)
                {
                    oRegistration.IsFallRisk = chkIsFallRisk.Checked;
                    oRegistration.IsDNR = chkIsDNR.Checked;
                    regDao.Update(oRegistration);
                }
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
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            VitalSignHdDao entityDao = new VitalSignHdDao(ctx);
            VitalSignDtDao entityDtDao = new VitalSignDtDao(ctx);
            RegistrationDao regDao = new RegistrationDao(ctx);
            try
            {
                VitalSignHd entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                List<VitalSignDt> lstEntityDt = BusinessLayer.GetVitalSignDtList(string.Format("ID = {0}", hdnID.Value), ctx);
                List<VitalSignDt> lstNewEntityDt = new List<VitalSignDt>();

                ControlToEntity(entity, lstNewEntityDt);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                entityDao.Update(entity);

                foreach (VitalSignDt entityDt in lstNewEntityDt)
                {
                    VitalSignDt obj = lstEntityDt.FirstOrDefault(p => p.VitalSignID == entityDt.VitalSignID);
                    entityDt.ID = entity.ID;
                    if (obj == null)
                        entityDtDao.Insert(entityDt);
                    else
                        entityDtDao.Update(entityDt);
                }

                Registration oRegistration = regDao.Get(AppSession.RegisteredPatient.RegistrationID);
                if (oRegistration != null)
                {
                    oRegistration.IsFallRisk = chkIsFallRisk.Checked;
                    oRegistration.IsDNR = chkIsDNR.Checked;
                    regDao.Update(oRegistration);
                }

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
            return result;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            List<vSpecialtyVitalSign> lst = BusinessLayer.GetvSpecialtyVitalSignList(string.Format("SpecialtyID ='{0}' ", AppSession.RegisteredPatient.SpecialtyID)).OrderBy(p => p.DisplayOrder).ToList();

            rptVitalSign.DataSource = lst;
            rptVitalSign.DataBind();
        }

        protected void rptVitalSign_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                vSpecialtyVitalSign obj = (vSpecialtyVitalSign)e.Item.DataItem;
                HtmlGenericControl div = null;

                if (obj.GCValueType == Constant.ControlType.TEXT_BOX)
                {
                    div = (HtmlGenericControl)e.Item.FindControl("divTxt");
                    TextBox txt = (TextBox)e.Item.FindControl("txtVitalSignType");
                    if (obj.IsNumericValue)
                    {
                        txt.Attributes.Add("validationgroup", "mpEntryPopup");
                        txt.CssClass = "number";
                    }
                    //ctl = (TextBox)e.Item.FindControl("txtVitalSignType");
                }
                else if (obj.GCValueType == Constant.ControlType.COMBO_BOX)
                {
                    div = (HtmlGenericControl)e.Item.FindControl("divDdl");
                    ASPxComboBox ddl = (ASPxComboBox)e.Item.FindControl("cboVitalSignType");
                    List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", obj.GCValueCode));
                    lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
                    Methods.SetComboBoxField<StandardCode>(ddl, lstSc, "StandardCodeName", "StandardCodeID");
                }

                if (div != null)
                    div.Visible = true;

            }
        }

    }
}