using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.Inpatient.Program
{
    public partial class PatientBirthParamedicCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnBirthRecordID.Value = param;

            vPatientBirthRecord entity = BusinessLayer.GetvPatientBirthRecordList(string.Format("BirthRecordID = {0}", param)).FirstOrDefault();
            txtNoReg.Text = entity.RegistrationNo;
            hdnMRN.Value = entity.MRN.ToString();
            txtMRN.Text = entity.MedicalNo;
            txtPatientName.Text = entity.FullName;
            BindGridView();

            Helper.SetControlEntrySetting(txtNoReg, new ControlEntrySetting(false, false, false), "mpPatientBirthParamedic");
            Helper.SetControlEntrySetting(txtMRN, new ControlEntrySetting(false, false, false), "mpPatientBirthParamedic");
            Helper.SetControlEntrySetting(txtPatientName, new ControlEntrySetting(true, true, true), "mpPatientBirthParamedic");
            Helper.SetControlEntrySetting(cboParamedicType, new ControlEntrySetting(true, true, true), "mpPatientBirthParamedic");
            Helper.SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, false, true), "mpPatientBirthParamedic");
            Helper.SetControlEntrySetting(txtPhysicianName, new ControlEntrySetting(true, false, true), "mpPatientBirthParamedic");
            Helper.SetControlEntrySetting(txtParamedicName, new ControlEntrySetting(true, true, true), "mpPatientBirthParamedic");

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList("");
            Methods.SetComboBoxField<StandardCode>(cboParamedicType, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.PARAMEDIC_ROLE).ToList(), "StandardCodeName", "StandardCodeID");
        }
        private void BindGridView()
        {
            grdView.DataSource = BusinessLayer.GetvPatientBirthRecordParamedicList(string.Format("BirthRecordID = {0} AND IsDeleted = 0", hdnBirthRecordID.Value));
            grdView.DataBind();
        }
        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            }
        }
        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            string param = e.Parameter;

            string result = param + "|";
            string errMessage = "";

            if (param == "save")
            {
                if (hdnID.Value.ToString() != "")
                {
                    if (OnSaveEditRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param == "delete")
            {
                if (OnDeleteRecord(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        private void ControlToEntity(PatientBirthRecordParamedic entity)
        {
            entity.BirthRecordID = Convert.ToInt32(hdnBirthRecordID.Value);
            entity.GCParamedicType = cboParamedicType.Value.ToString();
            if (chkOtherParamedic.Checked)
            {
                entity.ParamedicID = null;
                entity.ParamedicName = Request.Form[txtParamedicName.UniqueID];
            }
            else
            {
                entity.ParamedicName = "";
                entity.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
            }
        }
        private bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                PatientBirthRecordParamedic entity = new PatientBirthRecordParamedic();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertPatientBirthRecordParamedic(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                PatientBirthRecordParamedic entity = BusinessLayer.GetPatientBirthRecordParamedic(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePatientBirthRecordParamedic(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            try
            {
                PatientBirthRecordParamedic entity = BusinessLayer.GetPatientBirthRecordParamedic(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePatientBirthRecordParamedic(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
    }
}