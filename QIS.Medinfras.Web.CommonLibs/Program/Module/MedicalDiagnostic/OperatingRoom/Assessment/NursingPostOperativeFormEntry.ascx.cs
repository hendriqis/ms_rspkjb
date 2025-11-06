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
    public partial class NursingPostOperativeFormEntry : BaseEntryPopupCtl
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

            string fileName = string.Format(@"{0}\medicalForm\OperatingRoom\perioperativePostEntry.html", filePath);
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

            int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format(
                "GCParamedicMasterType IN ('{0}','{1}')",
                Constant.ParamedicType.Nurse, Constant.ParamedicType.Bidan));
            if (lstParamedic.Count == 0)
            {
                lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType = '{0}'", Constant.ParamedicType.Nurse));
            }

            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID2, lstParamedic, "ParamedicName", "ParamedicID");

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
                    txtDate.Text = paramInfo[2];
                    txtTime.Text = paramInfo[3];
                }
                else
                {
                    txtDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                }

                if (!string.IsNullOrEmpty(paramInfo[4]))
                    cboParamedicID.Value = paramInfo[4];
                else
                    cboParamedicID.Value = paramedicID.ToString();

                cboParamedicID2.Value = paramInfo[5];

                if (string.IsNullOrEmpty(paramInfo[8]))
                    PopulateFormContent();
                else
                    hdnDivHTML.Value = paramInfo[8]; 

                hdnFormValues.Value = paramInfo[9];
                txtRemarks.Text = paramInfo[10];
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtDate, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(cboParamedicID2, new ControlEntrySetting(true, true, true));
        }

        private void ControlToEntity(PerioperativeNursing entity)
        {
            entity.PostOperativeDate = Helper.GetDatePickerValue(txtDate);
            entity.PostOperativeTime = txtTime.Text;
            entity.VisitID = AppSession.RegisteredPatient.VisitID;
            entity.TestOrderID = Convert.ToInt32(hdnTestOrderID.Value);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PerioperativeNursingDao entityDao = new PerioperativeNursingDao(ctx);
            try
            {
                PerioperativeNursing entity = new PerioperativeNursing();
                ControlToEntity(entity);
                entity.PostOperativeLayout = hdnDivHTML.Value;
                entity.PostOperativeValues = hdnFormValues.Value;
                entity.PostOperativeRemarks = txtRemarks.Text;
                entity.PostOperativeWardNurseID = Convert.ToInt32(cboParamedicID2.Value);
                entity.PostOperativeRecoveryRoomNurseID = Convert.ToInt32(cboParamedicID.Value);
                entity.PostOperativeUserID = AppSession.UserLogin.UserID;
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
            PerioperativeNursingDao entityDao = new PerioperativeNursingDao(ctx);
            try
            {
                PerioperativeNursing entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.PostOperativeLayout = hdnDivHTML.Value;
                entity.PostOperativeValues = hdnFormValues.Value;
                entity.PostOperativeRemarks = txtRemarks.Text;
                entity.PostOperativeWardNurseID = Convert.ToInt32(cboParamedicID2.Value);
                entity.PostOperativeRecoveryRoomNurseID = Convert.ToInt32(cboParamedicID.Value);
                entity.PostOperativeUserID = AppSession.UserLogin.UserID;
                entity.PostOperativeLastUpdatedDate = DateTime.Now.Date;
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