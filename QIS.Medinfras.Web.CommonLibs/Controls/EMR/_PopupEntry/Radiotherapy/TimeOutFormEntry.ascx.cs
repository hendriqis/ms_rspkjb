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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class TimeOutFormEntry : BaseEntryPopupCtl
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

            string fileName = string.Format(@"{0}\medicalForm\Radiotherapy\TimeOutCheckList.html", filePath);
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
            hdnProgramID.Value = paramInfo[1];

            if (paramInfo.Length >= 13)
            {
                hdnPatientChargesDtID.Value = paramInfo[10];
                hdnTotalFraction.Value = paramInfo[12];
            }

            int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format(
                "GCParamedicMasterType IN ('{0}','{1}','{2}') AND IsDeleted = 0",
                Constant.ParamedicType.Nurse, Constant.ParamedicType.Bidan, Constant.ParamedicType.Fisikawan));

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

                PopulateFormContent();
            }
            else
            {
                cboParamedicID.ClientEnabled = false;

                if (!string.IsNullOrEmpty(paramInfo[2]))
                {
                    txtTimeOutDate.Text = paramInfo[2];
                    txtTimeOutTime.Text = paramInfo[3];
                }
                else
                {
                    txtTimeOutDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtTimeOutTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                }
                if (!string.IsNullOrEmpty(paramInfo[4]))
                    cboParamedicID.Value = paramInfo[4];
                else
                    cboParamedicID.Value = paramedicID.ToString();

                if (string.IsNullOrEmpty(paramInfo[8]))
                    PopulateFormContent();
                else
                    hdnDivHTML.Value = paramInfo[8]; 

                hdnFormValues.Value = paramInfo[9];

                txtFractionNo.Text = paramInfo[11];
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtTimeOutDate, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtTimeOutTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
        }

        private void ControlToEntity(BrachytherapySafetyCheck entity)
        {
            entity.VisitID = AppSession.RegisteredPatient.VisitID;

            if (hdnProgramID.Value != "" && hdnProgramID.Value != "0")
                entity.ProgramID = Convert.ToInt32(hdnProgramID.Value);

            if (hdnPatientChargesDtID.Value != "" && hdnPatientChargesDtID.Value != "0")
                entity.PatientChargesDtID = Convert.ToInt32(hdnPatientChargesDtID.Value);

            entity.FractionNo = Convert.ToInt32(txtFractionNo.Text);
        }

        private bool IsValidToProceed(ref string errMessage)
        {
            StringBuilder errMsg = new StringBuilder();

            if (!string.IsNullOrEmpty(txtFractionNo.Text))
            {
                if (!Methods.IsNumeric(txtFractionNo.Text))
                {
                    errMsg.AppendLine("Nilai Fraksi Ke- harus berupa numerik/angka dan lebih besar dari 0");
                }
                else
                {
                    int fractionNo = Convert.ToInt32(txtFractionNo.Text);
                    if (fractionNo == 0)
                    {
                        errMsg.AppendLine("Nilai Fraksi Ke- harus berupa numerik/angka dan lebih besar dari 0");
                    }
                    else
                    {
                        if (fractionNo > Convert.ToInt32(hdnTotalFraction.Value))
                        {
                            errMsg.AppendLine(string.Format("Nilai Fraksi Ke- tidak boleh lebih besar dari jumlah Fraksi Program ({0})", hdnTotalFraction.Value));
                        }
                    }
                }
            }

            errMessage = errMsg.ToString().Replace(Environment.NewLine, "<br />");

            return (errMsg.ToString() == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retVal)
        {
            //Never Add 
            bool result = true;
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            BrachytherapySafetyCheckDao entityDao = new BrachytherapySafetyCheckDao(ctx);
            try
            {
                if (IsValidToProceed(ref errMessage))
                {
                    BrachytherapySafetyCheck entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                    ControlToEntity(entity);
                    entity.TimeOutDate = Helper.GetDatePickerValue(txtTimeOutDate);
                    entity.TimeOutTime = txtTimeOutTime.Text;
                    entity.TimeOutParamedicID = Convert.ToInt32(cboParamedicID.Value);
                    entity.TimeOutLayout = hdnDivHTML.Value;
                    entity.TimeOutValues = hdnFormValues.Value;
                    entity.TimeOutUserID = AppSession.UserLogin.UserID;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDao.Update(entity);

                    retVal = entity.ID.ToString();

                    ctx.CommitTransaction();
                }
                else 
                {
                    result = false;
                    ctx.RollBackTransaction();
                }
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
            return result;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
    }
}