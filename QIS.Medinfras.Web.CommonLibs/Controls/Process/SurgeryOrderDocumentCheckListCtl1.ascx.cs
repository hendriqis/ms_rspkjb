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
    public partial class SurgeryOrderDocumentCheckListCtl1 : BaseEntryPopupCtl
    {
        protected int gridProcedureGroupPageCount = 1;
        protected int gridParamedicTeamPageCount = 1;

        protected static string _orderID = "0";

        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');

            hdnHealthcareServiceUnitID.Value = AppSession.MD0006;

            hdnVisitID.Value = paramInfo[0];
            hdnID.Value = paramInfo[1];

            IsAdd = false;
            _orderID = hdnID.Value;
            string filterExpression = string.Format("TestOrderID = {0}", hdnID.Value);
            vSurgeryTestOrderHd1 entity = BusinessLayer.GetvSurgeryTestOrderHd1List(filterExpression).FirstOrDefault();
            OnControlEntrySettingLocal(entity);
            ReInitControl();
            EntityToControl(entity);
        }

        private void PopulateFormContent()
        {
            string filePath = HttpContext.Current.Server.MapPath("~/Libs/App_Data");

            #region Check List Document
            StringBuilder innerHtml = Methods.LoadHTMLFormContent(string.Format(@"{0}\medicalForm\OperatingRoom\", filePath), "documentCheckList.html");

            divFormContent1.InnerHtml = innerHtml.ToString();
            hdnDocumentCheckListLayout.Value = innerHtml.ToString();
            #endregion
        }

        private void OnControlEntrySettingLocal(vSurgeryTestOrderHd1 entity)
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

        private void EntityToControl(vSurgeryTestOrderHd1 entity)
        {
            txtOrderDate.Text = entity.TestOrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtOrderTime.Text = entity.TestOrderTime;
            txtMedicalNo.Text = entity.MedicalNo;
            txtPatientName.Text = entity.PatientName;
            txtRegistrationNo.Text = entity.RegistrationNo;
            txtOrderNo.Text = entity.TestOrderNo;
            cboParamedicID.Value = entity.ParamedicID.ToString();
            chkIsUsedRequestTime.Checked = entity.IsUsedRequestTime;
            txtScheduleDate.Text = entity.ScheduledDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtScheduleTime.Text = entity.IsUsedRequestTime ? entity.ScheduledTime : string.Empty;
            if (entity.IsUsedRequestTime)
                divScheduleInfo.Style.Add("display", "block");
            else
                divScheduleInfo.Style.Add("display", "none");

            if (entity.GCToBePerformed == Constant.ToBePerformed.PRIOR_TO_NEXT_VISIT)
            {
                chkIsNextVisit.Checked = true;
                rblNextVisitType.SelectedValue = entity.IsODSVisit ? "1" : "2";

                if (entity.IsUsedRequestTime)
                    trNextVisit.Style.Add("display", "table-row");
                else
                    trNextVisit.Style.Add("display", "none");
            }
            else
            {
                chkIsNextVisit.Checked = false;
                rblNextVisitType.SelectedValue = "2";
            }

            rblIsEmergency.SelectedValue = entity.IsEmergency ? "1" : "0";

            txtEstimatedDuration.Text = entity.EstimatedDuration.ToString();
            hdnRoomID.Value = entity.RoomID.ToString();
            txtRoomCode.Text = entity.RoomCode;
            txtRoomName.Text = entity.RoomName;

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

        protected string GetUserID()
        {
            return AppSession.UserLogin.UserID.ToString();
        }
    }
}