using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxEditors;
using System.Text;
using System.IO;
using DevExpress.Web.ASPxCallbackPanel;
using System.Net;
using System.Text.RegularExpressions;
using System.Globalization;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class SurgerySignOutFormEntry : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                string[] paramInfo = param.Split('|');
                IsAdd = paramInfo[0] == "0";
                SetControlProperties(paramInfo);
            }
        }

        private void PopulateFormContent()
        {
            string filePath = HttpContext.Current.Server.MapPath("~/Libs/App_Data");

            string fileName = string.Format(@"{0}\medicalForm\OperatingRoom\surgerySignOutCheckList.html", filePath);
            IEnumerable<string> lstText = File.ReadAllLines(fileName);
            StringBuilder innerHtml = new StringBuilder();
            foreach (string text in lstText)
            {
                innerHtml.AppendLine(text);
            }

            divFormContent.InnerHtml = innerHtml.ToString();
            hdnDivHTML.Value = innerHtml.ToString();
        }

        private void SetControlProperties(string[] paramInfo)
        {
            hdnID.Value = paramInfo[0];
            hdnTestOrderID.Value = paramInfo[1];

            if (String.IsNullOrEmpty(hdnID.Value))
            {
                if (paramInfo.Length >= 11)
                {
                    hdnPatientChargesDtID.Value = paramInfo[10];
                }
            }
            else
            {
                if (paramInfo.Length >= 13)
                {
                    hdnPatientChargesDtID.Value = paramInfo[12];
                }
            }


            int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format(
                "GCParamedicMasterType IN ('{0}','{1}','{2}','{3}') AND IsDeleted = 0",
                Constant.ParamedicType.Nurse, Constant.ParamedicType.Physician, Constant.ParamedicType.Bidan, Constant.ParamedicType.AsistenLuar));

            Methods.SetComboBoxField<vParamedicMaster>(cboPhysician, lstParamedic.Where(lst => lst.GCParamedicMasterType == Constant.ParamedicType.Physician).ToList(), "ParamedicName", "ParamedicID");
            Methods.SetComboBoxField<vParamedicMaster>(cboAnesthetist, lstParamedic.Where(lst => lst.GCParamedicMasterType == Constant.ParamedicType.Physician).ToList(), "ParamedicName", "ParamedicID");
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic.Where(lst => lst.GCParamedicMasterType == Constant.ParamedicType.Nurse || lst.GCParamedicMasterType == Constant.ParamedicType.AsistenLuar).ToList(), "ParamedicName", "ParamedicID");

            if (IsAdd)
            {
                if (AppSession.UserLogin.ParamedicID != 0 && AppSession.UserLogin.ParamedicID != null)
                {
                    cboParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
                }
                else
                {
                    cboParamedicID.SelectedIndex = 0;
                }

                PopulateFormContent();
            }
            else
            {
                if (!string.IsNullOrEmpty(paramInfo[2]))
                {
                    txtSignOutDate.Text = paramInfo[2];
                    txtSignOutTime.Text = paramInfo[3];
                }
                else
                {
                    txtSignOutDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtSignOutTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                }

                if (!string.IsNullOrEmpty(paramInfo[5]))
                {
                    cboPhysician.Value = paramInfo[5];
                }

                if (!string.IsNullOrEmpty(paramInfo[6]) && paramInfo[6] != "0")
                {
                    cboAnesthetist.Value = paramInfo[6];
                }
                else
                {
                    cboAnesthetist.Value = "";
                }

                if (cboPhysician.Value == null && cboAnesthetist.Value == null)
                {
                    if (!string.IsNullOrEmpty(hdnID.Value) && hdnID.Value != "0")
                    {
                        SurgicalSafetyCheck obj = BusinessLayer.GetSurgicalSafetyCheck(Convert.ToInt32(hdnID.Value));
                        if (obj != null)
                        {
                            cboPhysician.Value = obj.SignOutPhysicianID.ToString();
                            cboAnesthetist.Value = obj.SignOutAnesthetistID.ToString();
                        }
                    }
                }

                if (!string.IsNullOrEmpty(paramInfo[7]))
                    cboParamedicID.Value = paramInfo[7];
                else
                    cboParamedicID.Value = paramedicID.ToString();

                if (string.IsNullOrEmpty(paramInfo[8]))
                    PopulateFormContent();
                else
                    hdnDivHTML.Value = paramInfo[8];

                hdnFormValues.Value = paramInfo[9];

                if (paramInfo.Length >= 12)
                {
                    txtSurgeryEndDate.Text = paramInfo[10];
                    txtSurgeryEndTime.Text = paramInfo[11];
                }
                else
                {
                    txtSurgeryEndDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtSurgeryEndTime.Text = string.Format("00:00");
                }
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtSignOutDate, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtSignOutTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(cboPhysician, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboAnesthetist, new ControlEntrySetting(true, true, false));
        }

        private void ControlToEntity(SurgicalSafetyCheck entity)
        {
            entity.VisitID = AppSession.RegisteredPatient.VisitID;
            if (hdnTestOrderID.Value != "" && hdnTestOrderID.Value != "0")
                entity.TestOrderID = Convert.ToInt32(hdnTestOrderID.Value);

            if (hdnPatientChargesDtID.Value != "" && hdnPatientChargesDtID.Value != "0")
                entity.PatientChargesDtID = Convert.ToInt32(hdnPatientChargesDtID.Value);
        }

        private bool IsValidToSave(ref string errMessage)
        {
            StringBuilder errMsg = new StringBuilder();

            DateTime date;
            string format = Constant.FormatString.DATE_PICKER_FORMAT;
            try
            {
                date = DateTime.ParseExact(txtSignOutDate.Text, format, CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                errMsg.AppendLine(string.Format("Format Tanggal Sign Out {0} tidak sesuai dengan format", txtSignOutDate.Text));
            }
            DateTime sdate = Helper.GetDatePickerValue(txtSignOutDate.Text);
            if (DateTime.Compare(sdate, Helper.GetDatePickerValue(txtSignOutDate.Text)) > 0)
            {
                errMsg.AppendLine("Tanggal Sign Out harus lebih kecil atau sama dengan tanggal saat ini.");
            }

            try
            {
                date = DateTime.ParseExact(txtSurgeryEndDate.Text, format, CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                errMsg.AppendLine(string.Format("Format Tanggal Sign Out {0} tidak sesuai dengan format", txtSignOutDate.Text));
            }
            sdate = Helper.GetDatePickerValue(txtSurgeryEndDate.Text);
            if (DateTime.Compare(sdate, Helper.GetDatePickerValue(txtSurgeryEndDate.Text)) > 0)
            {
                errMsg.AppendLine("Tanggal Selesai Operasi harus lebih kecil atau sama dengan tanggal saat ini.");
            }

            if (!string.IsNullOrEmpty(txtSignOutTime.Text) || txtSignOutTime.Text != "")
            {
                Regex reg = new Regex(@"^(?:[01][0-9]|2[0-3]):[0-5][0-9]$");
                if (!reg.IsMatch(txtSignOutTime.Text))
                {
                    errMsg.AppendLine("Jam sign out harus sesuai dengan format jam (00:00 s/d 23:59)");
                }
            }

            if (!string.IsNullOrEmpty(txtSurgeryEndTime.Text) || txtSurgeryEndTime.Text != "")
            {
                Regex reg = new Regex(@"^(?:[01][0-9]|2[0-3]):[0-5][0-9]$");
                if (!reg.IsMatch(txtSurgeryEndTime.Text))
                {
                    errMsg.AppendLine("Jam selesai operasi harus sesuai dengan format jam (00:00 s/d 23:59)");
                }
            }

            errMessage = errMsg.ToString();

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;
            bool isError = false;

            if (!IsValidToSave(ref errMessage))
            {
                isError = true;
                retVal = "0";
                result = false;
            }

            if (!isError)
            {
                IDbContext ctx = DbFactory.Configure(true);
                SurgicalSafetyCheckDao entityDao = new SurgicalSafetyCheckDao(ctx);
                try
                {
                    SurgicalSafetyCheck entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                    ControlToEntity(entity);
                    entity.SignOutDate = Helper.GetDatePickerValue(txtSignOutDate);
                    entity.SignOutTime = txtSignOutTime.Text;
                    entity.SignOutNurseID = Convert.ToInt32(cboParamedicID.Value);
                    entity.SignOutPhysicianID = Convert.ToInt32(cboPhysician.Value);
                    entity.SignOutAnesthetistID = Convert.ToInt32(cboAnesthetist.Value);

                    if (!string.IsNullOrEmpty(txtSurgeryEndTime.Text))
                    {
                        entity.SurgeryEndDate = Helper.GetDatePickerValue(txtSurgeryEndDate);
                        entity.SurgeryEndTime = txtSurgeryEndTime.Text;
                    }

                    entity.SignOutLayout = hdnDivHTML.Value;
                    entity.SignOutValues = hdnFormValues.Value;
                    entity.SignOutLastUpdatedDate = DateTime.Now.Date;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDao.Update(entity);

                    retVal = entity.ID.ToString();

                    ctx.CommitTransaction();

                    result = true;
                }
                catch (Exception ex)
                {
                    result = false;
                    errMessage = ex.Message;
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                }
                finally
                {
                    ctx.Close();
                }
            }
            return result;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
    }
}