using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class FluidBalanceOutputEntry : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                string[] paramInfo = param.Split('|');
                hdnOutputGroup.Value = paramInfo[1];
                IsAdd = paramInfo[0] == "0";
                SetControlProperties(paramInfo);
            }
        }

        private void SetControlProperties(string[] paramInfo)
        {
            hdnID.Value = paramInfo[0];

            int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format(
                                                                        "IsDeleted = 0 AND GCParamedicMasterType IN ('{0}','{1}','{2}')",
                                                                        Constant.ParamedicType.Nurse,
                                                                        Constant.ParamedicType.Bidan,
                                                                        Constant.ParamedicType.Nutritionist
                                                                    ));
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");

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
            vFluidBalance obj = BusinessLayer.GetvFluidBalanceList(string.Format("ID = {0}", hdnID.Value)).FirstOrDefault();
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
            SetControlEntrySetting(cboFluidType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtFluidName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtFluidAmount, new ControlEntrySetting(true, true, true, "0"));
        }

        private void ControlToEntity(FluidBalance entity)
        {
            entity.VisitID = AppSession.RegisteredPatient.VisitID;
            entity.LogDate = Helper.GetDatePickerValue(txtLogDate);
            entity.LogTime = txtLogTime.Text;
            entity.HealthcareServiceUnitID = Convert.ToInt32(AppSession.HealthcareServiceUnitID);
            entity.ParamedicID = Convert.ToInt32(cboParamedicID.Value.ToString());
            entity.GCFluidGroup = hdnOutputGroup.Value == "1" ? Constant.FluidBalanceGroup.Output : Constant.FluidBalanceGroup.Output_Tidak_Diukur;
            entity.GCFluidType = cboFluidType.Value.ToString();
            entity.FluidName = txtFluidName.Text;
            if (hdnOutputGroup.Value == "1")
            {
                entity.FluidAmount = Convert.ToDecimal(txtFluidAmount.Text);
                entity.Frequency = 0;
            }
            else
            {
                entity.FluidAmount = 0;
                entity.Frequency = Convert.ToInt16(txtFrequency.Text);
            }
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
                FluidBalanceDao entityDao = new FluidBalanceDao(ctx);
                try
                {
                    FluidBalance entity = new FluidBalance();
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
                FluidBalanceDao entityDao = new FluidBalanceDao(ctx);
                try
                {
                    FluidBalance entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
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