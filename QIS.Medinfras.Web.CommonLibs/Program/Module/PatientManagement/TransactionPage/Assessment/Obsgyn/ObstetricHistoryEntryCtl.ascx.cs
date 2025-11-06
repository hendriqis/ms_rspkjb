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
    public partial class ObstetricHistoryEntryCtl : BasePagePatientPageEntryCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            if (paramInfo[0] != "")
            {
                IsAdd = false;
                hdnID.Value = paramInfo[0];
                SetControlProperties();
                ObstetricHistory entity = BusinessLayer.GetObstetricHistory(Convert.ToInt32(hdnID.Value));
                EntityToControl(entity);
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
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}') AND IsActive = 1 AND IsDeleted = 0 ORDER BY StandardCodeName ASC",
                   Constant.StandardCode.BIRTH_METHOD, Constant.StandardCode.CAESAR_METHOD, Constant.StandardCode.BORN_CONDITION, Constant.StandardCode.HEALTHCARE_PROFESSIONAL_TYPE, Constant.StandardCode.GENDER, Constant.StandardCode.KOMPLIKASI_NON_OBSTETRI));
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            
            Methods.SetComboBoxField<StandardCode>(cboBirthMethod, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.BIRTH_METHOD || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboCaesarMethod, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.CAESAR_METHOD || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboBornCondition, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.BORN_CONDITION || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboParamedicType, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.HEALTHCARE_PROFESSIONAL_TYPE || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboSex, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.GENDER || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboNonObstetri, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.KOMPLIKASI_NON_OBSTETRI || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

            txtWeight.Text = "0.00";
            txtLength.Text = "0.00";
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtPregnancyNo, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtPregnancyDuration, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboBirthMethod, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboCaesarMethod, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboBornCondition, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboParamedicType, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboSex, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtWeight, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtLength, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtOtherComplicationRemarks, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboNonObstetri, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(ObstetricHistory entity)
        {
            txtPregnancyNo.Text = entity.PregnancyNo.ToString();
            txtPregnancyDuration.Text = entity.PregnancyDuration.ToString();
            cboBirthMethod.Value = entity.GCBirthMethod;
            cboCaesarMethod.Value = entity.GCCaesarMethod;
            cboBornCondition.Value = entity.GCBornCondition;
            chkIsAntepartumBleeding.Checked = entity.IsAntepartumBleeding;
            chkIsPostpartumBleeding.Checked = entity.IsPostpartumBleeding;
            chkIsPreclampsia.Checked = entity.IsPreeclampsia;
            chkIsEclampsia.Checked = entity.IsEclampsia;
            chkIsHasInfectious.Checked = entity.IsHasInfectious;
            chkIsOtherComplication.Checked = entity.IsOtherComplication;
            txtOtherComplicationRemarks.Text = entity.OtherComplicationRemarks;
            cboParamedicType.Value = entity.GCParamedicType;
            cboSex.Value = entity.GCSex;
            txtWeight.Text = entity.Weight.ToString();
            txtLength.Text = entity.Length.ToString();
            txtRemarks.Text = entity.Remarks;
            cboNonObstetri.Value = entity.GCNonObstetri;
            chkIsIbuNifas.Checked = entity.IsTrueIbuNifas;
        }

        private void ControlToEntity(ObstetricHistory entity)
        {
            entity.RegistrationID = AppSession.RegisteredPatient.RegistrationID;
            entity.MRN = AppSession.RegisteredPatient.MRN;
            entity.PregnancyNo = Convert.ToInt16(txtPregnancyNo.Text);
            entity.PregnancyDuration = Convert.ToInt16(txtPregnancyDuration.Text);
            if (cboBirthMethod.Value == null)
                entity.GCBirthMethod = "";
            else
                entity.GCBirthMethod = cboBirthMethod.Value.ToString();
            if (cboCaesarMethod.Value == null)
                entity.GCCaesarMethod = "";
            else
                entity.GCCaesarMethod = cboCaesarMethod.Value.ToString();
            if (cboBornCondition.Value == null)
                entity.GCBornCondition = "";
            else
                entity.GCBornCondition = cboBornCondition.Value.ToString();
            if (cboParamedicType.Value == null)
                entity.GCParamedicType = "";
            else
                entity.GCParamedicType = cboParamedicType.Value.ToString();
            if (cboSex.Value == null)
                entity.GCSex = "";
            else
                entity.GCSex = cboSex.Value.ToString();
            if (cboNonObstetri.Value == null)
                entity.GCNonObstetri = "";
            else
                entity.GCNonObstetri = cboNonObstetri.Value.ToString();

            entity.IsTrueIbuNifas = chkIsIbuNifas.Checked;
            entity.IsAntepartumBleeding = chkIsAntepartumBleeding.Checked;
            entity.IsPostpartumBleeding = chkIsPostpartumBleeding.Checked;
            entity.IsPreeclampsia = chkIsPreclampsia.Checked;
            entity.IsEclampsia = chkIsEclampsia.Checked;
            entity.IsHasInfectious = chkIsHasInfectious.Checked;
            entity.IsOtherComplication = chkIsOtherComplication.Checked;

            if (chkIsOtherComplication.Checked)
            {
                entity.OtherComplicationRemarks = txtOtherComplicationRemarks.Text;
            }
            entity.Weight = Convert.ToDecimal(txtWeight.Text);
            entity.Length = Convert.ToDecimal(txtLength.Text);
            entity.Remarks = txtRemarks.Text;
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                ObstetricHistory entity = new ObstetricHistory();
                ControlToEntity(entity);

                entity.CreatedBy = AppSession.UserLogin.UserID;
                entity.CreatedDate = DateTime.Now;
                BusinessLayer.InsertObstetricHistory(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                ObstetricHistory entity = BusinessLayer.GetObstetricHistory(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateObstetricHistory(entity);
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