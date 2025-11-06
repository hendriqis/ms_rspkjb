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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class HSUFluidBalanceOutputEntry : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                string[] paramInfo = param.Split('|');
                hdnAssessmentType.Value = paramInfo[0];
                hdnAssessmentID.Value = paramInfo[1];
                hdnID.Value = paramInfo[2];
                IsAdd = paramInfo[2] == "0";
                hdnOutputGroup.Value = paramInfo[3];
                hdnIsCopy.Value = paramInfo[4];
                SetControlProperties(paramInfo);
                if (paramInfo.Length > 4)
                {
                    if (hdnID.Value != "0")
                        IsAdd = hdnIsCopy.Value == "1";

                    if (hdnIsCopy.Value == "1")
                    {
                        txtLogDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                        txtLogTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                    }
                }
            }
        }

        private void SetControlProperties(string[] paramInfo)
        {
            hdnID.Value = paramInfo[2];

            int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format(
                "GCParamedicMasterType IN ('{0}','{1}')",
                Constant.ParamedicType.Nurse, Constant.ParamedicType.Physician));
            if (lstParamedic.Count == 0)
            {
                lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType = '{0}'", Constant.ParamedicType.Nurse));
            }

            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic.Where(lst => lst.GCParamedicMasterType == Constant.ParamedicType.Nurse).ToList(), "ParamedicName", "ParamedicID");

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}'", Constant.StandardCode.JENIS_OUTPUT));
            Methods.SetComboBoxField<StandardCode>(cboFluidType,lstStandardCode,"StandardCodeName","StandardCodeID");

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
            }
            else
            {
                EntityToControl(hdnID.Value);
                cboParamedicID.ClientEnabled = false;
            }

            if (hdnOutputGroup.Value == "2")
            {

                lblFluidType.InnerText = "Jenis";
                trFrequency.Style.Add("display", "table-row");
                trFluidAmount.Style.Add("display", "none");
            }
            else
            {
                lblFluidType.InnerText = "Jenis Cairan";
                trFrequency.Style.Add("display", "none");
                trFluidAmount.Style.Add("display", "table-row");
            }
        }

        private void EntityToControl(string recordID)
        {
            vHSUFluidBalance obj = BusinessLayer.GetvHSUFluidBalanceList(string.Format("ID = {0}", hdnID.Value)).FirstOrDefault();
            if (obj != null)
            {
                txtLogDate.Text = obj.LogDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtLogTime.Text = obj.LogTime;
                cboParamedicID.Value = obj.ParamedicID.ToString();
                cboFluidType.Value = obj.GCFluidType;
                txtFluidName.Text = obj.FluidName;
                txtFluidAmount.Text = obj.FluidAmount.ToString();
                txtFrequency.Text = obj.Frequency.ToString();
                txtRemarks.Text = obj.Remarks;
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtLogDate, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtLogTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
        }

        private void ControlToEntity(HSUFluidBalance entity)
        {
            entity.VisitID = AppSession.RegisteredPatient.VisitID;
            entity.LogDate = Helper.GetDatePickerValue(txtLogDate);
            entity.LogTime = txtLogTime.Text;
            entity.HealthcareServiceUnitID = Convert.ToInt32(AppSession.HealthcareServiceUnitID);
            entity.ParamedicID = Convert.ToInt32(cboParamedicID.Value.ToString());
            entity.GCFluidGroup = Constant.FluidBalanceGroup.Output;
            entity.GCMonitoringType = this.TagProperty;
            switch (hdnAssessmentType.Value)
            {
                case Constant.FluidBalanceAssessmentType.INTRA_HEMODIALYSIS:
                    entity.PreHDAssessmentID = Convert.ToInt32(hdnAssessmentID.Value);
                    break;
                default:
                    break;
            }
            entity.GCFluidType = cboFluidType.Value.ToString();
            entity.FluidName = txtFluidName.Text;
            entity.FluidAmount = Convert.ToDecimal(txtFluidAmount.Text);
            entity.Remarks = txtRemarks.Text;
        }

        private bool IsValid(ref string validationErrMsg)
        {
            string message = string.Empty;

            if (Helper.GetDatePickerValue(txtLogDate) > DateTime.Now)
            {
                message = "Tanggal Pencatatan tidak boleh lebih besar dari tanggal sekarang";
            }
            else if (string.IsNullOrEmpty(txtLogTime.Text))
            {
                message = "Jam Pencatatan tidak boleh kosong";
            }
            else
            {
                Regex reg = new Regex(@"^(?:[01][0-9]|2[0-3]):[0-5][0-9]$");
                if (!reg.IsMatch(txtLogTime.Text))
                {
                    message = "Jam Pencatatan harus dalam format hh:mm";
                }
            }

            if (!string.IsNullOrEmpty(message))
            {
                validationErrMsg = message;
            }

            return message == string.Empty;
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;
            if (IsValid(ref errMessage))
            {
                IDbContext ctx = DbFactory.Configure(true);
                HSUFluidBalanceDao entityDao = new HSUFluidBalanceDao(ctx);
                try
                {
                    HSUFluidBalance entity = new HSUFluidBalance();
                    ControlToEntity(entity);
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
            }
            else
            {
                result = false;
            } 
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;
            if (IsValid(ref errMessage))
            {
                IDbContext ctx = DbFactory.Configure(true);
                HSUFluidBalanceDao entityDao = new HSUFluidBalanceDao(ctx);
                try
                {
                    HSUFluidBalance entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                    ControlToEntity(entity);
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
            }
            else
            {
                result = false;
            }
            return result;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
    }
}