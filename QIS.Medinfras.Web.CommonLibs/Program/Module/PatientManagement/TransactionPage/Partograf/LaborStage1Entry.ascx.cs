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
    public partial class LaborStage1Entry : BaseEntryPopupCtl3
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

            string fileName = string.Format(@"{0}\medicalForm\LaborStage\laborstage{1}.html", filePath, hdnStage.Value);
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
            hdnAntenatalRecordID.Value = paramInfo[1];
            hdnStage.Value = paramInfo[2];

            int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format(
                "GCParamedicMasterType IN ('{0}','{1}','{2}','{3}') AND IsDeleted = 0",
                Constant.ParamedicType.Nurse, Constant.ParamedicType.Physician, Constant.ParamedicType.Bidan, Constant.ParamedicType.AsistenLuar));
            if (lstParamedic.Count == 0)
            {
                lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType = '{0}'", Constant.ParamedicType.Nurse));
            }

            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic.Where(lst => lst.GCParamedicMasterType == Constant.ParamedicType.Bidan).ToList(), "ParamedicName", "ParamedicID");

            if (IsAdd)
            {
                if (AppSession.UserLogin.ParamedicID != 0 && AppSession.UserLogin.ParamedicID != null)
                {
                    cboParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
                    cboParamedicID.Enabled = false;
                }
                else
                {
                    cboParamedicID.SelectedIndex = 0;
                }
                PopulateFormContent();
            }
            else
            {
                cboParamedicID.Enabled = false;

                if (!string.IsNullOrEmpty(paramInfo[3]))
                {
                    txtLaborStageDate.Text = paramInfo[3];
                    txtLaborStageTime.Text = paramInfo[4];
                }
                else
                {
                    txtLaborStageDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtLaborStageTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                }

                if (!string.IsNullOrEmpty(paramInfo[5]))
                    cboParamedicID.Value = paramInfo[5];
                else
                    cboParamedicID.Value = paramedicID.ToString();

                if (string.IsNullOrEmpty(paramInfo[6]))
                    PopulateFormContent();
                else
                    hdnDivHTML.Value = paramInfo[6];

                hdnFormValues.Value = paramInfo[7];
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtLaborStageDate, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtLaborStageTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(true, true, true));
        }

        private void ControlToEntity(LaborStage entity)
        {
            switch (hdnStage.Value)
            {
                case "1":
                    entity.Stage1Date = Helper.GetDatePickerValue(txtLaborStageDate);
                    entity.Stage1Time = txtLaborStageTime.Text;
                    entity.Stage1ParamedicID = Convert.ToInt32(cboParamedicID.Value);
                    entity.Stage1Layout = hdnDivHTML.Value;
                    entity.Stage1Values = hdnFormValues.Value;
                    entity.Stage1UserID = AppSession.UserLogin.UserID;
                    entity.Stage1LastUpdatedDate = DateTime.Now.Date;
                    break;
                case "2":
                    entity.Stage2Date = Helper.GetDatePickerValue(txtLaborStageDate);
                    entity.Stage2Time = txtLaborStageTime.Text;
                    entity.Stage2ParamedicID = Convert.ToInt32(cboParamedicID.Value);
                    entity.Stage2Layout = hdnDivHTML.Value;
                    entity.Stage2Values = hdnFormValues.Value;
                    entity.Stage2UserID = AppSession.UserLogin.UserID;
                    entity.Stage2LastUpdatedDate = DateTime.Now.Date;
                    break;
                case "3":
                    entity.Stage3Date = Helper.GetDatePickerValue(txtLaborStageDate);
                    entity.Stage3Time = txtLaborStageTime.Text;
                    entity.Stage3ParamedicID = Convert.ToInt32(cboParamedicID.Value);
                    entity.Stage3Layout = hdnDivHTML.Value;
                    entity.Stage3Values = hdnFormValues.Value;
                    entity.Stage3UserID = AppSession.UserLogin.UserID;
                    entity.Stage3LastUpdatedDate = DateTime.Now.Date;
                    break;
                case "4":
                    entity.Stage4Date = Helper.GetDatePickerValue(txtLaborStageDate);
                    entity.Stage4Time = txtLaborStageTime.Text;
                    entity.Stage4ParamedicID = Convert.ToInt32(cboParamedicID.Value);
                    entity.Stage4Layout = hdnDivHTML.Value;
                    entity.Stage4Values = hdnFormValues.Value;
                    entity.Stage4UserID = AppSession.UserLogin.UserID;
                    entity.Stage4LastUpdatedDate = DateTime.Now.Date;
                    break;
                default:
                    entity.Stage1Date = Helper.GetDatePickerValue(txtLaborStageDate);
                    entity.Stage1Time = txtLaborStageTime.Text;
                    entity.Stage1ParamedicID = Convert.ToInt32(cboParamedicID.Value);
                    entity.Stage1Layout = hdnDivHTML.Value;
                    entity.Stage1Values = hdnFormValues.Value;
                    entity.Stage1UserID = AppSession.UserLogin.UserID;
                    entity.Stage1LastUpdatedDate = DateTime.Now.Date;
                    break;
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            LaborStageDao entityDao = new LaborStageDao(ctx);
            try
            {
                LaborStage entity = new LaborStage();
                ControlToEntity(entity);

                entity.VisitID = AppSession.RegisteredPatient.VisitID;
                if (hdnAntenatalRecordID.Value != "" && hdnAntenatalRecordID.Value != "0")
                    entity.AntenatalRecordID = Convert.ToInt32(hdnAntenatalRecordID.Value);

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
            LaborStageDao entityDao = new LaborStageDao(ctx);
            try
            {
                LaborStage entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);

                entity.VisitID = AppSession.RegisteredPatient.VisitID;
                if (hdnAntenatalRecordID.Value != "" && hdnAntenatalRecordID.Value != "0")
                    entity.AntenatalRecordID = Convert.ToInt32(hdnAntenatalRecordID.Value);

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