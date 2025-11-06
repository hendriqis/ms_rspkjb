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
using QIS.Data.Core.Dal;
using System.Web.UI.HtmlControls;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.EmergencyCare.Program
{
    public partial class ERInitialPhysicalExam : BasePagePatientPageList
    {
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
            Helper.SetControlEntrySetting(txtObservationDate, new ControlEntrySetting(true, true, true), "mpInitialPhysicalExam");
            Helper.SetControlEntrySetting(txtObservationTime, new ControlEntrySetting(true, true, true), "mpInitialPhysicalExam");
            Helper.SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, true, true), "mpInitialPhysicalExam");
            Helper.SetControlEntrySetting(txtPhysicianName, new ControlEntrySetting(true, true, true), "mpInitialPhysicalExam");

            ctlToolbar.SetSelectedMenu(2);
            string filterExpression = string.Format("HealthcareServiceUnitID = '{0}'", AppSession.RegisteredPatient.HealthcareServiceUnitID.ToString());
            hdnHealthcareServiceUnitID.Value = AppSession.RegisteredPatient.HealthcareServiceUnitID.ToString();
            int count = BusinessLayer.GetServiceUnitParamedicRowCount(string.Format("HealthcareServiceUnitID = {0}", AppSession.RegisteredPatient.HealthcareServiceUnitID));
            if (count > 0)
                hdnIsHealthcareServiceUnitHasParamedic.Value = "1";
            else
                hdnIsHealthcareServiceUnitHasParamedic.Value = "0";

            txtObservationDate.Text = AppSession.RegisteredPatient.VisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtObservationTime.Text = AppSession.RegisteredPatient.VisitTime;

            List<vVitalSignHd> lstvVitalSignHd = BusinessLayer.GetvVitalSignHdList(string.Format("VisitID = '{0}'", AppSession.RegisteredPatient.VisitID));
            if (lstvVitalSignHd.Count > 0)
            {
                vVitalSignHd entity = lstvVitalSignHd.FirstOrDefault();
                hdnID.Value = entity.ID.ToString();
                List<vVitalSignDt> entityDt = BusinessLayer.GetvVitalSignDtList(string.Format("ID = {0}", entity.ID));
                EntityToControl(entity, entityDt);
            }
            else
                hdnID.Value = "";
        }

        private void EntityToControl(vVitalSignHd entity, List<vVitalSignDt> lstEntityDt)
        {
            txtObservationDate.Text = entity.ObservationDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtObservationTime.Text = entity.ObservationTime;
            hdnPhysicianID.Value = entity.ParamedicID.ToString();
            txtPhysicianCode.Text = entity.ParamedicCode;
            txtPhysicianName.Text = entity.ParamedicName;

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
        }

        private void ControlToEntity(VitalSignHd entity, List<VitalSignDt> lstEntityDt)
        {
            entity.ObservationDate = Helper.GetDatePickerValue(txtObservationDate);
            entity.ObservationTime = txtObservationTime.Text;
            entity.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
            
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

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            if (type == "save")
            {
                bool result = true;
                IDbContext ctx = DbFactory.Configure(true);
                VitalSignHdDao entityDao = new VitalSignHdDao(ctx);
                VitalSignDtDao entityDtDao = new VitalSignDtDao(ctx);
                try
                {
                    if (hdnID.Value != "")
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
                    }
                    else
                    {
                        VitalSignHd entity = new VitalSignHd();
                        List<VitalSignDt> lstEntityDt = new List<VitalSignDt>();
                        ControlToEntity(entity, lstEntityDt);
                        entity.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
                        entity.VisitID = AppSession.RegisteredPatient.VisitID;
                        entity.CreatedBy = AppSession.UserLogin.UserID;
                        entityDao.Insert(entity);

                        entity.ID = BusinessLayer.GetVitalSignHdMaxID(ctx);

                        foreach (VitalSignDt entityDt in lstEntityDt)
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
                }
                finally
                {
                    ctx.Close();
                }
                return result;
            }
            return false;
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