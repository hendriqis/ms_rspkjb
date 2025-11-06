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
using System.Text;
using System.Globalization;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class VaccinationHistoryEntryCtl1 : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnPopupVaccinationTypeID.Value = paramInfo[1];
            hdnIsCovid19.Value = paramInfo[3];
            hdnPopupVisitID.Value = paramInfo[4];
            hdnPopupMRN.Value = paramInfo[5];
            
            if (paramInfo[0] != "" && paramInfo[0] != "0")
            {
                IsAdd = false;
                hdnPopupID.Value = paramInfo[0];
                SetControlProperties();
                VaccinationHistory entity = BusinessLayer.GetVaccinationHistory(Convert.ToInt32(hdnPopupID.Value));
                EntityToControl(entity);
            }
            else
            {
                hdnPopupID.Value = "";
                IsAdd = true;
                SetControlProperties();
            }
            txtVaccinationType.Text = paramInfo[2];
        }

        private void SetControlProperties()
        {
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}', '{1}','{2}') AND IsActive = 1 AND IsDeleted = 0 ORDER BY StandardCodeName ASC",
                   Constant.StandardCode.JENIS_VAKSINASI_COVID_19, Constant.StandardCode.VACCINATION_ROUTE, Constant.StandardCode.ITEM_UNIT));
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });

            Methods.SetComboBoxField<StandardCode>(cboCovid19Vaccin, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.JENIS_VAKSINASI_COVID_19 || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboRoute, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.VACCINATION_ROUTE || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDosingUnit, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ITEM_UNIT && sc.TagProperty == "1").ToList(), "StandardCodeName", "StandardCodeID");

            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList("IsDeleted = 0");
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");
            cboParamedicID.Value = AppSession.RegisteredPatient.ParamedicID.ToString();

            hdnDateToday.Value = Helper.GetCurrentDate().ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            if (hdnIsCovid19.Value == "1")
            {
                trCovidVaccination.Style.Add("display", "table-row");
                trItemInfo.Style.Add("display", "none");
            }
            else
            {
                trItemInfo.Style.Add("display", "table-row");
                trCovidVaccination.Style.Add("display", "none");
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtVaccinationDate, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtSequenceNo, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboCovid19Vaccin, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtBatchNo, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDosingDose, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(VaccinationHistory entity)
        {
            if (!entity.VaccinationDate.Equals(new DateTime(1900, 1, 1)))
            {
                txtVaccinationDate.Text = entity.VaccinationDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            }
            txtSequenceNo.Text = entity.SequenceNo.ToString();
            chkIsBooster.Checked = entity.IsBooster;
            chkIsExternalProvider.Checked = entity.IsExternalProvider;
            if (entity.IsExternalProvider)
            {
                txtProvider.Text = entity.Provider;
                cboParamedicID.Value = null;

                trProviderInfo.Style.Add("display", "table-row");
                trParamedicInfo.Style.Add("display", "none");
             }
            else
            {
                trProviderInfo.Style.Add("display", "none");
                trParamedicInfo.Style.Add("display", "table-row");
                txtProvider.Text = string.Empty;
                cboParamedicID.Value = entity.ParamedicID.ToString();
            }
            chkIsCurrentVisit.Checked = entity.IsCurrentVisit;
            if (entity.ItemID != 0)
            {
                ledItem.Value = entity.ItemName.ToString();
            }
            txtBatchNo.Text = entity.BatchNo;
            cboCovid19Vaccin.Value = entity.GCCovid19Vaccin;
            cboRoute.Value = entity.GCVaccinationRoute;
            txtDosingDose.Text = entity.Dose.ToString("G29");
            cboDosingUnit.Value = entity.GCDoseUnit;
            txtRemarks.Text = entity.Remarks;
        }

        private void ControlToEntity(VaccinationHistory entity)
        {
            entity.MRN = AppSession.RegisteredPatient.MRN;
            entity.VaccinationTypeID = Convert.ToInt32(hdnPopupVaccinationTypeID.Value);
            entity.VaccinationDate = Helper.GetDatePickerValue(txtVaccinationDate);
            entity.SequenceNo = Convert.ToInt16(txtSequenceNo.Text);
            entity.IsBooster = chkIsBooster.Checked;
            entity.IsExternalProvider = chkIsExternalProvider.Checked;
            if (chkIsExternalProvider.Checked)
            {
                entity.Provider = txtProvider.Text;
                entity.VisitID = null;
                entity.ParamedicID = null;
                entity.IsCurrentVisit = false;
            }
            else
            {
                entity.Provider = null;
                entity.VisitID = Convert.ToInt32(hdnPopupVisitID.Value);
                if (cboParamedicID.Value != null)
                    if (cboParamedicID.Value.ToString() != "")
                        entity.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
                entity.IsCurrentVisit = chkIsCurrentVisit.Checked;
            }

            if (hdnIsCovid19.Value == "1")
            {
                entity.GCCovid19Vaccin = cboCovid19Vaccin.Value.ToString();
            }
            else
            {
                if (!string.IsNullOrEmpty(hdnDrugID.Value))
                {
                    entity.ItemID = Convert.ToInt32(hdnDrugID.Value);
                    entity.ItemName = hdnDrugName.Value;
                }
            }

            if (cboRoute.Value != null && !string.IsNullOrEmpty(cboRoute.Value.ToString()))
            {
                entity.GCVaccinationRoute = cboRoute.Value.ToString();
            }

            entity.BatchNo = txtBatchNo.Text;
            entity.Dose = Convert.ToDecimal(txtDosingDose.Text);
            if (cboDosingUnit.Value != null && !string.IsNullOrEmpty(cboDosingUnit.Value.ToString()))
            {
                entity.GCDoseUnit = cboDosingUnit.Value.ToString();
            }           
            entity.Remarks = txtRemarks.Text;
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            VaccinationHistoryDao entityDao = new VaccinationHistoryDao(ctx);

            try
            {
                if (IsValid(ref errMessage))
                {
                    VaccinationHistory entity = new VaccinationHistory();
                    ControlToEntity(entity);

                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    entity.CreatedDate = DateTime.Now;
                    int id = entityDao.InsertReturnPrimaryKeyID(entity);

                    ctx.CommitTransaction();
                    retVal = id.ToString();
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
                retVal = "0";
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        private bool IsValid(ref string errMessage)
        {
            StringBuilder message = new StringBuilder();

            string itemName = ledItem.Text;

            if (string.IsNullOrEmpty(txtSequenceNo.Text))
                message.Append("Informasi vaksinasi ke harus diisi / tidak boleh kosong|");
            else
            {
                if (!Methods.IsNumeric(txtSequenceNo.Text))
                {
                    message.Append("Informasi vaksinasi ke harus dalam bentuk numerik|");
                }
                else {
                    if (Convert.ToDecimal(txtSequenceNo.Text) < 0)
                    {
                        message.Append("Informasi vaksinasi ke harus lebih besar atau sama dengan 0|");
                    }
                }
            }

            if (string.IsNullOrEmpty(txtDosingDose.Text))
                message.AppendLine("Dosis pemberian vaksinasi ke harus diisi / tidak boleh kosong|");
            else
            {
                if (!Methods.IsNumeric(txtDosingDose.Text))
                {
                    message.AppendLine("Dosis Pemberian ke harus dalam bentuk numerik|");
                }
                else
                {
                    if (Convert.ToDecimal(txtDosingDose.Text) <= 0)
                    {
                        message.AppendLine("Informasi vaksinasi ke harus lebih besar atau sama dengan 1|");
                    }
                }
            }

            DateTime vaccinationDateTime = DateTime.ParseExact(string.Format("{0} {1}", txtVaccinationDate.Text, "00:00"), Common.Constant.FormatString.DATE_TIME_FORMAT_4, CultureInfo.InvariantCulture, DateTimeStyles.None);

            if (vaccinationDateTime.Date > DateTime.Now.Date)
            {
                message.AppendLine("Tanggal Vaksinasi harus lebih kecil atau sama dengan tanggal hari ini.|");
            }

            errMessage = message.ToString().Replace(@"|", "<br />");

            return string.IsNullOrEmpty(errMessage);
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            VaccinationHistoryDao entityDao = new VaccinationHistoryDao(ctx);

            try
            {
                if (IsValid(ref errMessage))
                {
                    VaccinationHistory entity = BusinessLayer.GetVaccinationHistory(Convert.ToInt32(hdnPopupID.Value));
                    ControlToEntity(entity);

                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entity.LastUpdatedDate = DateTime.Now;
                    entityDao.Update(entity);

                    ctx.CommitTransaction();
                    retVal = entity.ID.ToString();
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
                retVal = "0";
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
    }
}