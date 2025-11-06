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
    public partial class VaccinationCovid19EntryCtl : BasePagePatientPageEntryCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            if (paramInfo[0] != "")
            {
                IsAdd = false;
                hdnID.Value = paramInfo[0];
                SetControlProperties();
                Covid19Vaccination entity = BusinessLayer.GetCovid19Vaccination(Convert.ToInt32(hdnID.Value));
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
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}') AND IsActive = 1 AND IsDeleted = 0 ORDER BY StandardCodeName ASC",
                   Constant.StandardCode.JENIS_VAKSINASI_COVID_19));
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });

            Methods.SetComboBoxField<StandardCode>(cboCovid19Vaccin, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.JENIS_VAKSINASI_COVID_19 || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            hdnDateToday.Value = Helper.GetCurrentDate().ToString(Constant.FormatString.DATE_PICKER_FORMAT);
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtVaccinationDate, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtSequenceNo, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtProvider, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboCovid19Vaccin, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtBatchNo, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(Covid19Vaccination entity)
        {
            if (!entity.VaccinationDate.Equals(new DateTime(1900, 1, 1)))
            {
                txtVaccinationDate.Text = entity.VaccinationDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            }
            txtSequenceNo.Text = entity.SequenceNo.ToString();
            txtProvider.Text = entity.Provider;
            cboCovid19Vaccin.Value = entity.GCCovid19Vaccin;
            txtBatchNo.Text = entity.BatchNo;
            txtRemarks.Text = entity.Remarks;
        }

        private void ControlToEntity(Covid19Vaccination entity)
        {
            entity.MRN = AppSession.RegisteredPatient.MRN;
            entity.SequenceNo = Convert.ToInt16(txtSequenceNo.Text);
            entity.VaccinationDate = Helper.GetDatePickerValue(txtVaccinationDate);
            entity.Provider = txtProvider.Text;
            entity.GCCovid19Vaccin = cboCovid19Vaccin.Value.ToString();
            entity.BatchNo = txtBatchNo.Text;
            entity.Remarks = txtRemarks.Text;
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                Covid19Vaccination entity = new Covid19Vaccination();
                ControlToEntity(entity);

                entity.MRN = AppSession.RegisteredPatient.MRN;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entity.CreatedDate = DateTime.Now;
                BusinessLayer.InsertCovid19Vaccination(entity);
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
                Covid19Vaccination entity = BusinessLayer.GetCovid19Vaccination(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateCovid19Vaccination(entity);
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