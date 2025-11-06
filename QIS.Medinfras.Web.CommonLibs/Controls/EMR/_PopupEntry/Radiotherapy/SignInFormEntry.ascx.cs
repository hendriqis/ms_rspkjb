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
    public partial class SignInFormEntry : BaseEntryPopupCtl3
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

            string fileName = string.Format(@"{0}\medicalForm\Radiotherapy\SignInCheckList.html", filePath);
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
                "GCParamedicMasterType IN ('{0}','{1}','{2}','{3}','{4}') AND IsDeleted = 0",
                Constant.ParamedicType.Nurse, Constant.ParamedicType.Physician, Constant.ParamedicType.Bidan, Constant.ParamedicType.AsistenLuar, Constant.ParamedicType.Fisikawan));
            if (lstParamedic.Count == 0)
            {
                lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType = '{0}'", Constant.ParamedicType.Nurse));
            }

            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic.Where(lst => lst.GCParamedicMasterType == Constant.ParamedicType.Fisikawan).ToList(), "ParamedicName", "ParamedicID");
            Methods.SetComboBoxField<vParamedicMaster>(cboPhysician, lstParamedic.Where(lst => lst.GCParamedicMasterType == Constant.ParamedicType.Physician).ToList(), "ParamedicName", "ParamedicID");
            Methods.SetComboBoxField<vParamedicMaster>(cboAnesthetist, lstParamedic.Where(lst => lst.GCParamedicMasterType == Constant.ParamedicType.Physician).ToList(), "ParamedicName", "ParamedicID");
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedic2, lstParamedic.Where(lst => lst.GCParamedicMasterType == Constant.ParamedicType.Nurse || lst.GCParamedicMasterType == Constant.ParamedicType.AsistenLuar).ToList(), "ParamedicName", "ParamedicID");

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
                cboParamedic2.Value = paramInfo[7];

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
            SetControlEntrySetting(txtFractionNo, new ControlEntrySetting(true, true, true, 0));
            SetControlEntrySetting(txtSignInDate, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtSignInTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(cboPhysician, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboAnesthetist, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboParamedic2, new ControlEntrySetting(true, true, true));
        }

        private void ControlToEntity(BrachytherapySafetyCheck entity)
        {
            entity.FractionNo = Convert.ToInt32(txtFractionNo.Text);
            entity.SignInDate = Helper.GetDatePickerValue(txtSignInDate);
            entity.SignInTime = txtSignInTime.Text;
            entity.VisitID = AppSession.RegisteredPatient.VisitID;
            if (hdnProgramID.Value != "" && hdnProgramID.Value != "0")
            entity.ProgramID = Convert.ToInt32(hdnProgramID.Value);

            if (hdnPatientChargesDtID.Value != "" && hdnPatientChargesDtID.Value != "0")
            entity.PatientChargesDtID = Convert.ToInt32(hdnPatientChargesDtID.Value);
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
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            BrachytherapySafetyCheckDao entityDao = new BrachytherapySafetyCheckDao(ctx);
            try
            {
                if (IsValidToProceed(ref errMessage))
                {
                    BrachytherapySafetyCheck entity = new BrachytherapySafetyCheck();
                    ControlToEntity(entity);
                    entity.SignInLayout = hdnDivHTML.Value;
                    entity.SignInValues = hdnFormValues.Value;
                    entity.SignInParamedicID = Convert.ToInt32(cboParamedicID.Value);
                    entity.SignOutPhysicianID = Convert.ToInt32(cboPhysician.Value);
                    entity.SignOutAnesthetistID = Convert.ToInt32(cboAnesthetist.Value);
                    entity.SignOutParamedicID = Convert.ToInt32(cboParamedic2.Value);
                    entity.SignInUserID = AppSession.UserLogin.UserID;
                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    entity.ID = entityDao.InsertReturnPrimaryKeyID(entity);

                    retVal = entity.ID.ToString();

                    ctx.CommitTransaction();
                }
                else {
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

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            BrachytherapySafetyCheckDao entityDao = new BrachytherapySafetyCheckDao(ctx);
            try
            {
                BrachytherapySafetyCheck entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                if (IsValidToProceed(ref errMessage))
                {
                    ControlToEntity(entity);
                    entity.SignInParamedicID = Convert.ToInt32(cboParamedicID.Value);
                    entity.SignOutPhysicianID = Convert.ToInt32(cboPhysician.Value);
                    entity.SignOutAnesthetistID = Convert.ToInt32(cboAnesthetist.Value);
                    entity.SignOutParamedicID = Convert.ToInt32(cboParamedic2.Value);
                    entity.SignInLayout = hdnDivHTML.Value;
                    entity.SignInValues = hdnFormValues.Value;
                    entity.SignInLastUpdatedDate = DateTime.Now.Date;
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