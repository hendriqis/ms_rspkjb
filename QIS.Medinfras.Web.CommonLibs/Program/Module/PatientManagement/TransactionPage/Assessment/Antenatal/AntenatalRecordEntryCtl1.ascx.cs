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
    public partial class AntenatalRecordEntryCtl1 : BasePagePatientPageEntryCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            if (paramInfo[0] != "")
            {
                IsAdd = false;
                hdnID.Value = paramInfo[0];
                SetControlProperties();
                Antenatal entity = BusinessLayer.GetAntenatal(Convert.ToInt32(hdnID.Value));
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
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtLogDate, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtLMP, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtEDB, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtMenstrualHistory, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtMedicalHistory, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(Antenatal entity)
        {
            txtPregnancyNo.Text = entity.PregnancyNo.ToString();            
            txtLogDate.Text = entity.LogDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtLMP.Text = entity.LMP.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtEDB.Text = entity.EDB.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtGravida.Text = entity.Gravida.ToString();
            txtPara.Text = entity.Para.ToString();
            txtAbortion.Text = entity.Abortion.ToString();
            txtLife.Text = entity.Life.ToString();
            txtMenstrualHistory.Text = entity.MenstrualHistory;
            txtMedicalHistory.Text = entity.MedicalHistory;
        }

        private void ControlToEntity(Antenatal entity)
        {
            entity.LogDate = Helper.GetDatePickerValue(txtLogDate);
            entity.PregnancyNo = Convert.ToInt16(txtPregnancyNo.Text);
            entity.LMP = Helper.GetDatePickerValue(txtLMP);
            entity.EDB = Helper.GetDatePickerValue(txtEDB);
            entity.Gravida = Convert.ToInt16(txtGravida.Text);
            entity.Para = Convert.ToInt16(txtPara.Text);
            entity.Abortion = Convert.ToInt16(txtAbortion.Text);
            entity.Life = Convert.ToInt16(txtLife.Text);
            entity.MenstrualHistory = txtMenstrualHistory.Text;
            entity.MedicalHistory = txtMedicalHistory.Text;
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                Antenatal entity = new Antenatal();
                ControlToEntity(entity);

                entity.MRN = AppSession.RegisteredPatient.MRN;
                entity.FirstVisitID = AppSession.RegisteredPatient.VisitID;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entity.CreatedDate = DateTime.Now;
                BusinessLayer.InsertAntenatal(entity);

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
                Antenatal entity = BusinessLayer.GetAntenatal(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateAntenatal(entity);
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