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
    public partial class IVTheraphyNoteEntry : BaseEntryPopupCtl
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
        }

        private void EntityToControl(string recordID)
        {
            vIVTheraphyNote obj = BusinessLayer.GetvIVTheraphyNoteList(string.Format("ID = {0}", hdnID.Value)).FirstOrDefault();
            if (obj != null)
            {
                txtLogDate.Text = obj.IVTherapyNoteDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtLogTime.Text = obj.IVTherapyNoteTime;
                cboParamedicID.Value = obj.ParamedicID.ToString();
                txtRemarks.Text = obj.IVTherapyNotes;
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtLogDate, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtLogTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
        }

        private void ControlToEntity(IVTheraphyNote entity)
        {
            entity.VisitID = AppSession.RegisteredPatient.VisitID;
            entity.IVTherapyNoteDate = Helper.GetDatePickerValue(txtLogDate);
            entity.IVTherapyNoteTime = txtLogTime.Text;
            entity.ParamedicID = Convert.ToInt32(cboParamedicID.Value.ToString());
            entity.IVTherapyNotes = txtRemarks.Text;
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
                IVTheraphyNoteDao entityDao = new IVTheraphyNoteDao(ctx);
                try
                {
                    IVTheraphyNote entity = new IVTheraphyNote();
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
                IVTheraphyNoteDao entityDao = new IVTheraphyNoteDao(ctx);
                try
                {
                    IVTheraphyNote entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
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