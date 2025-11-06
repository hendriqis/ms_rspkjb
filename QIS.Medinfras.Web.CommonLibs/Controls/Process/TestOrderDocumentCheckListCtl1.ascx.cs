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
using System.Globalization;
using DevExpress.Web.ASPxCallbackPanel;
using System.Text;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class TestOrderDocumentCheckListCtl1 : BaseEntryPopupCtl
    {
        protected int gridProcedureGroupPageCount = 1;
        protected int gridParamedicTeamPageCount = 1;

        protected static string _orderID = "0";

        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');

            hdnVisitID.Value = paramInfo[0];
            hdnID.Value = paramInfo[1];
            string[] patientInfo = paramInfo[2].Split(';');
            hdnHealthcareServiceUnitID.Value = paramInfo[3];
            hdnOperatingRoomID.Value = paramInfo[4];

            IsAdd = false;
            _orderID = hdnID.Value;
            string filterExpression = string.Format("TestOrderID = {0}", hdnID.Value);
            vTestOrderHd2 entity = BusinessLayer.GetvTestOrderHd2List(filterExpression).FirstOrDefault();
            OnControlEntrySettingLocal(entity);
            ReInitControl();
            EntityToControl(entity, patientInfo);
        }

        private void PopulateFormContent()
        {
            string filePath = HttpContext.Current.Server.MapPath("~/Libs/App_Data");

            #region Check List Document
            StringBuilder innerHtml = Methods.LoadHTMLFormContent(string.Format(@"{0}\medicalForm\DocumentChecklist\", filePath), "documentCheckList.html");

            divFormContent1.InnerHtml = innerHtml.ToString();
            hdnDocumentCheckListLayout.Value = innerHtml.ToString();
            #endregion
        }

        private void OnControlEntrySettingLocal(vTestOrderHd2 entity)
        {
            int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format(
                                                    "GCParamedicMasterType IN ('{0}') AND ParamedicID = {1}",
                                                    Constant.ParamedicType.Physician, entity.ParamedicID));
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");
            cboParamedicID.SelectedIndex = 0;

            SetControlEntrySetting(txtOrderDate, new ControlEntrySetting(false, false, false, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtOrderTime, new ControlEntrySetting(false, false, false, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));

            SetControlProperties();
        }

        private void EntityToControl(vTestOrderHd2 entity, string[] patientInfo)
        {
            txtOrderDate.Text = entity.TestOrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtOrderTime.Text = entity.TestOrderTime;
            txtMedicalNo.Text = patientInfo[0];
            txtPatientName.Text = patientInfo[1];
            txtRegistrationNo.Text = entity.RegistrationNo;
            txtOrderNo.Text = entity.TestOrderNo;
            cboParamedicID.Value = entity.ParamedicID.ToString();
            txtScheduleDate.Text = entity.ScheduledDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtScheduleTime.Text = entity.ScheduledTime;
            rblIsEmergency.SelectedValue = entity.IsEmergency ? "1" : "0";
            txtDocumentChecklistDateTime.Text = entity.cfDocumentCheckListLastInfo;

            if (!string.IsNullOrEmpty(entity.DocumentCheckListLayout))
            {
                hdnDocumentCheckListLayout.Value = entity.DocumentCheckListLayout;
                divFormContent1.InnerHtml = entity.DocumentCheckListLayout;
                hdnDocumentCheckListValue.Value = entity.DocumentCheckListValue;
            }
            else
            {
                PopulateFormContent();
            }
        }

        private void ControlToEntity(TestOrderHd entityHd)
        {
            entityHd.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
            entityHd.FromHealthcareServiceUnitID = entityHd.FromHealthcareServiceUnitID;
            entityHd.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
            entityHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
            entityHd.TestOrderDate = Helper.GetDatePickerValue(txtOrderDate);
            entityHd.TestOrderTime = txtOrderTime.Text.Replace('.', ':');
            entityHd.GCToBePerformed = Constant.ToBePerformed.SCHEDULLED;
            entityHd.ScheduledDate = Helper.GetDatePickerValue(txtScheduleDate);
            entityHd.ScheduledTime = txtScheduleTime.Text;
            entityHd.IsOperatingRoomOrder = true;
            entityHd.IsEmergency = rblIsEmergency.SelectedValue == "1" ? true : false;
            entityHd.TransactionCode = Constant.TransactionCode.OTHER_TEST_ORDER;
            entityHd.IsOperatingRoomOrder = true;
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;

            IDbContext ctx = DbFactory.Configure(true);
            TestOrderHdDao oOrderHdDao = new TestOrderHdDao(ctx);
            bool isError = false;

            try
            {
                TestOrderHd entityUpdate = oOrderHdDao.Get(Convert.ToInt32(hdnID.Value));
                entityUpdate.DocumentCheckListDateTime = DateTime.Now;
                entityUpdate.DocumentCheckListBy = AppSession.UserLogin.UserID;
                entityUpdate.DocumentCheckListLayout = hdnDocumentCheckListLayout.Value;
                entityUpdate.DocumentCheckListValue = hdnDocumentCheckListValue.Value;
                entityUpdate.IsDocumentCheckListCompleted = true;
                entityUpdate.LastUpdatedBy = AppSession.UserLogin.UserID;
                oOrderHdDao.Update(entityUpdate);

                ctx.CommitTransaction();

                retVal = hdnID.Value;
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                result = false;
            }
            finally
            {
                ctx.Close();
            }

            return result;
        }

        protected string GetUserID()
        {
            return AppSession.UserLogin.UserID.ToString();
        }
    }
}