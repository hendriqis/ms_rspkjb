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
using System.Data;
using System.Text;
using System.IO;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class BrachytherapyProgramLogEntryCtl : BaseEntryPopupCtl3
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            if (paramInfo[0] != "0")
            {
                IsAdd = false;
                SetControlProperties(paramInfo);
                BrachytherapyProgramLog entity = BusinessLayer.GetBrachytherapyProgramLog(Convert.ToInt32(hdnProgramLogID.Value));
                EntityToControl(entity);
            }
            else
            {
                hdnProgramLogID.Value = "";
                IsAdd = true;
                SetControlProperties(paramInfo);
                if (paramInfo.Length >= 4)
                {
                    //copy mode
                    if (!string.IsNullOrEmpty(paramInfo[3]))
                    {
                        int copyRecordID = Convert.ToInt32(paramInfo[3]);
                        BrachytherapyProgramLog entity = BusinessLayer.GetBrachytherapyProgramLog(Convert.ToInt32(copyRecordID));
                        EntityToControl(entity);

                        txtLogDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                        txtLogTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                    }
                }
            }

            if (IsAdd)
            {
                PopulateFormContent();
            }
        }

        private void PopulateFormContent()
        {
            string filePath = HttpContext.Current.Server.MapPath("~/Libs/App_Data");

            string fileName = string.Format(@"{0}\medicalForm\Radiotherapy\{1}.html", filePath, "brachytherapyLog01");
            IEnumerable<string> lstText = File.ReadAllLines(fileName);
            StringBuilder innerHtml = new StringBuilder();
            foreach (string text in lstText)
            {
                innerHtml.AppendLine(text);
            }

            hdnFormLayout.Value = innerHtml.ToString();
            divFormContent.InnerHtml = innerHtml.ToString();
        }

        private void SetControlProperties(string[] paramInfo)
        {
            hdnProgramLogID.Value = paramInfo[0];
            hdnProgramID.Value = paramInfo[1];
            hdnPopupVisitID.Value = paramInfo[2];
            hdnTotalFraction.Value = paramInfo[3];
        }

        protected override void OnControlEntrySetting()
        {
            if (AppSession.RegisteredPatient.DepartmentID != Constant.Facility.INPATIENT)
            {
                SetControlEntrySetting(txtLogDate, new ControlEntrySetting(true, true, true, AppSession.RegisteredPatient.VisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
                SetControlEntrySetting(txtLogTime, new ControlEntrySetting(true, true, true, AppSession.RegisteredPatient.VisitTime));
            }
            else
            {
                SetControlEntrySetting(txtLogDate, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
                SetControlEntrySetting(txtLogTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            }

            List<StandardCode> lstCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}') AND IsActive = 1 AND IsDeleted = 0",
Constant.StandardCode.BRACHYTHERAPY_TYPE, Constant.StandardCode.APPLICATOR_TYPE));

            List<StandardCode> lstCode1 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.BRACHYTHERAPY_TYPE).ToList();
            Methods.SetComboBoxField<StandardCode>(cboGCBrachyTherapyType, lstCode1, "StandardCodeName", "StandardCodeID");
            List<StandardCode> lstCode2 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.APPLICATOR_TYPE).ToList();
            Methods.SetComboBoxField<StandardCode>(cboGCApplicatorType, lstCode2, "StandardCodeName", "StandardCodeID");

            SetControlEntrySetting(txtFractionNo, new ControlEntrySetting(true, true, true,"0"));
            SetControlEntrySetting(txtTotalDosage, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtTotalChannel, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));

        }

        private void EntityToControl(BrachytherapyProgramLog entity)
        {
           txtLogDate.Text = entity.LogDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
           txtLogTime.Text = entity.LogTime;
           cboGCBrachyTherapyType.Value = entity.GCBrachytherapyType;
           cboGCApplicatorType.Value = entity.GCApplicatorType;
           txtFractionNo.Text = entity.FractionNo.ToString();
           txtTotalChannel.Text = entity.TotalChannel.ToString();
           if (!string.IsNullOrEmpty(entity.ChannelPositionFormValue))
           {
               divFormContent.InnerHtml = entity.ChannelPositionFormLayout;
               hdnFormLayout.Value = entity.ChannelPositionFormLayout;
               hdnFormValues.Value = entity.ChannelPositionFormValue;
               trChannelLayout.Style.Add("display", "table-row");
               hdnIsUsingForm.Value = "1";
           }
           else
           {
               trChannelLayout.Style.Add("display", "none");
               hdnIsUsingForm.Value = "0";
               PopulateFormContent();
           }

           txtTotalDosage.Text = entity.TotalDosage.ToString();
           txtRemarks.Text = entity.Remarks;
        }

        private void ControlToEntity(BrachytherapyProgramLog entity)
        {
            entity.LogDate = Helper.GetDatePickerValue(txtLogDate);
            entity.LogTime = txtLogTime.Text;
            entity.ProgramID = Convert.ToInt32(hdnProgramID.Value);
            entity.VisitID = Convert.ToInt32(hdnPopupVisitID.Value);
            entity.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
            entity.GCBrachytherapyType = cboGCBrachyTherapyType.Value.ToString();
            entity.GCApplicatorType = cboGCApplicatorType.Value.ToString();
            entity.FractionNo = Convert.ToInt32(txtFractionNo.Text);
            entity.TotalChannel = Convert.ToInt32(txtTotalChannel.Text);
            if (hdnIsUsingForm.Value == "1")
            {
                entity.ChannelPositionFormLayout = hdnFormLayout.Value;
                entity.ChannelPositionFormValue = hdnFormValues.Value;
            }
            entity.TotalDosage = Convert.ToDecimal(txtTotalDosage.Text);
            entity.Remarks = txtRemarks.Text;
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;

            if (IsValidToProceed(ref errMessage))
            {

                IDbContext ctx = DbFactory.Configure(true);
                BrachytherapyProgramLogDao objDao = new BrachytherapyProgramLogDao(ctx);

                try
                {
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    int recordID = 0;

                    BrachytherapyProgramLog entity = new BrachytherapyProgramLog();
                    ControlToEntity(entity);
                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    recordID = objDao.InsertReturnPrimaryKeyID(entity);

                    retVal = recordID.ToString();

                    ctx.CommitTransaction();
                }
                catch (Exception ex)
                {
                    result = false;
                    retVal = "0";
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
            bool isError = false;
            IDbContext ctx = DbFactory.Configure(true);
            BrachytherapyProgramLogDao objDao = new BrachytherapyProgramLogDao(ctx);

            if (!IsValidToProceed(ref errMessage))
            {
                isError = true;
                result = false;
            }

            if (!isError)
            {
                try
                {
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    BrachytherapyProgramLog entity = objDao.Get(Convert.ToInt32(hdnProgramLogID.Value));
                    if (entity != null)
                    {
                        ControlToEntity(entity);
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        objDao.Update(entity);

                        retVal = hdnProgramLogID.Value;

                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Tidak ditemukan program radioterapi yang dilakukan perubahan";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
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
            }
            return result;
        }

        private bool IsValidToProceed(ref string errMessage)
        {
            StringBuilder errMsg = new StringBuilder();

            if (cboGCBrachyTherapyType.Value == null)
            {
                errMsg.AppendLine("Jenis Brakiterapi harus diisi");
            }
            if (cboGCApplicatorType.Value == null)
            {
                errMsg.AppendLine("Aplikator harus diisi");                
            }

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

            if (!string.IsNullOrEmpty(txtTotalChannel.Text))
            {
                if (!Methods.IsNumeric(txtTotalChannel.Text))
                {
                    errMsg.AppendLine("Nilai Total Channel harus berupa numerik/angka");
                }
            }

            if (!string.IsNullOrEmpty(txtTotalDosage.Text))
            {
                if (!Methods.IsNumeric(txtTotalDosage.Text))
                {
                    errMsg.AppendLine("Nilai Dosis yang diberikan harus berupa numerik/angka");
                }
            }

            errMessage = errMsg.ToString().Replace(Environment.NewLine, "<br />");

            return (errMsg.ToString() == string.Empty);
        }
    }
}