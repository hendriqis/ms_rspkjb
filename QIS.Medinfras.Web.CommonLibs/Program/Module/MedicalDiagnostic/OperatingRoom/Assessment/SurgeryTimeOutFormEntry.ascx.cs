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
    public partial class SurgeryTimeOutFormEntry : BaseEntryPopupCtl
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

            string fileName = string.Format(@"{0}\medicalForm\OperatingRoom\surgeryTimeOutCheckList.html", filePath);
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

            if (paramInfo.Length >= 11)
            {
                hdnPatientChargesDtID.Value = paramInfo[10];
            }

            int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format(
                "GCParamedicMasterType IN ('{0}','{1}') AND IsDeleted = 0",
                Constant.ParamedicType.Nurse, Constant.ParamedicType.Bidan));

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
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtTimeOutDate, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtTimeOutTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
        }

        private void ControlToEntity(SurgicalSafetyCheck entity)
        {
            entity.VisitID = AppSession.RegisteredPatient.VisitID;

            if (hdnTestOrderID.Value != "" && hdnTestOrderID.Value != "0")
                entity.TestOrderID = Convert.ToInt32(hdnTestOrderID.Value);

            if (hdnPatientChargesDtID.Value != "" && hdnPatientChargesDtID.Value != "0")
                entity.PatientChargesDtID = Convert.ToInt32(hdnPatientChargesDtID.Value);
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
            SurgicalSafetyCheckDao entityDao = new SurgicalSafetyCheckDao(ctx);
            try
            {
                SurgicalSafetyCheck entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
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