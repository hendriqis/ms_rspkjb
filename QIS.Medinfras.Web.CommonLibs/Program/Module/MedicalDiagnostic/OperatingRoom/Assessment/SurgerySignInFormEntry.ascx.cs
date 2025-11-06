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
    public partial class SurgerySignInFormEntry : BaseEntryPopupCtl
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

            string fileName = string.Format(@"{0}\medicalForm\OperatingRoom\surgerySignInCheckList.html", filePath);
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
                "GCParamedicMasterType IN ('{0}','{1}','{2}','{3}') AND IsDeleted = 0",
                Constant.ParamedicType.Nurse, Constant.ParamedicType.Physician, Constant.ParamedicType.Bidan, Constant.ParamedicType.AsistenLuar));
            if (lstParamedic.Count == 0)
            {
                lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType = '{0}'", Constant.ParamedicType.Nurse));
            }

            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic.Where(lst => lst.GCParamedicMasterType == Constant.ParamedicType.Nurse).ToList(), "ParamedicName", "ParamedicID");
            Methods.SetComboBoxField<vParamedicMaster>(cboPhysician, lstParamedic.Where(lst => lst.GCParamedicMasterType == Constant.ParamedicType.Physician).ToList(), "ParamedicName", "ParamedicID");
            Methods.SetComboBoxField<vParamedicMaster>(cboAnesthetist, lstParamedic.Where(lst => lst.GCParamedicMasterType == Constant.ParamedicType.Physician).ToList(), "ParamedicName", "ParamedicID");
            Methods.SetComboBoxField<vParamedicMaster>(cboScrubNurse, lstParamedic.Where(lst => lst.GCParamedicMasterType == Constant.ParamedicType.Nurse || lst.GCParamedicMasterType == Constant.ParamedicType.AsistenLuar).ToList(), "ParamedicName", "ParamedicID");

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
                    txtSignInDate.Text = paramInfo[2];
                    txtSignInTime.Text = paramInfo[3];
                }
                else
                {
                    txtSignInDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtSignInTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                }

                if (!string.IsNullOrEmpty(paramInfo[4]))
                    cboParamedicID.Value = paramInfo[4];
                else
                    cboParamedicID.Value = paramedicID.ToString();

                cboPhysician.Value = paramInfo[5];
                if (!string.IsNullOrEmpty(paramInfo[6]) && paramInfo[6] != "0")
                {
                    cboAnesthetist.Value = paramInfo[6];
                }
                else
                {
                    cboAnesthetist.Value = "";
                }
                cboScrubNurse.Value = paramInfo[7];

                if (string.IsNullOrEmpty(paramInfo[8]))
                    PopulateFormContent();
                else
                    hdnDivHTML.Value = paramInfo[8];

                hdnFormValues.Value = paramInfo[9];
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtSignInDate, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtSignInTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(cboPhysician, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboAnesthetist, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboScrubNurse, new ControlEntrySetting(true, true, true));
        }

        private void ControlToEntity(SurgicalSafetyCheck entity)
        {
            entity.SignInDate = Helper.GetDatePickerValue(txtSignInDate);
            entity.SignInTime = txtSignInTime.Text;
            entity.VisitID = AppSession.RegisteredPatient.VisitID;
            if (hdnTestOrderID.Value != "" && hdnTestOrderID.Value != "0")
            entity.TestOrderID = Convert.ToInt32(hdnTestOrderID.Value);

            if (hdnPatientChargesDtID.Value != "" && hdnPatientChargesDtID.Value != "0")
            entity.PatientChargesDtID = Convert.ToInt32(hdnPatientChargesDtID.Value);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            SurgicalSafetyCheckDao entityDao = new SurgicalSafetyCheckDao(ctx);
            try
            {
                SurgicalSafetyCheck entity = new SurgicalSafetyCheck();
                ControlToEntity(entity);
                entity.SignInLayout = hdnDivHTML.Value;
                entity.SignInValues = hdnFormValues.Value;
                entity.SignInParamedicID = Convert.ToInt32(cboParamedicID.Value);
                entity.SignOutPhysicianID = Convert.ToInt32(cboPhysician.Value);
                entity.SignOutAnesthetistID = Convert.ToInt32(cboAnesthetist.Value);
                entity.SignOutNurseID = Convert.ToInt32(cboScrubNurse.Value);
                entity.SignInUserID = AppSession.UserLogin.UserID;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entity.ID = entityDao.InsertReturnPrimaryKeyID(entity);

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

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            SurgicalSafetyCheckDao entityDao = new SurgicalSafetyCheckDao(ctx);
            try
            {
                SurgicalSafetyCheck entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.SignInParamedicID = Convert.ToInt32(cboParamedicID.Value);
                entity.SignOutPhysicianID = Convert.ToInt32(cboPhysician.Value);
                entity.SignOutAnesthetistID = Convert.ToInt32(cboAnesthetist.Value);
                entity.SignOutNurseID = Convert.ToInt32(cboScrubNurse.Value);
                entity.SignInLayout = hdnDivHTML.Value;
                entity.SignInValues = hdnFormValues.Value;
                entity.SignInLastUpdatedDate = DateTime.Now.Date;
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