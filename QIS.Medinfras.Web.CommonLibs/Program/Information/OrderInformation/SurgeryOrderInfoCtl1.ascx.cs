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
    public partial class SurgeryOrderInfoCtl1 : BaseViewPopupCtl
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

            _orderID = hdnID.Value;
            string filterExpression = string.Format("TestOrderID = {0}", hdnID.Value);
            vSurgeryTestOrderHd1 entity = BusinessLayer.GetvSurgeryTestOrderHd1List(filterExpression).FirstOrDefault();
            OnControlEntrySettingLocal(entity);
            EntityToControl(entity);

            if (entity.IsUsedRequestTime)
                txtScheduleTime.Enabled = true;
        }

        private void OnControlEntrySettingLocal(vSurgeryTestOrderHd1 entity)
        {
            int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format(
                                                    "GCParamedicMasterType IN ('{0}') AND ParamedicID = {1}",
                                                    Constant.ParamedicType.Physician, entity.ParamedicID));
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");
            cboParamedicID.SelectedIndex = 0;

            List<StandardCode> lstCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}','{2}') AND IsActive = 1 AND IsDeleted = 0",
                    Constant.StandardCode.SURGERY_TEAM_ROLE,
                    Constant.StandardCode.TIPE_PENYAKIT_INFEKSI,
                    Constant.StandardCode.TIPE_KOMORBID));

            List<StandardCode> lstCode1 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.SURGERY_TEAM_ROLE).ToList();
            List<StandardCode> lstCode2 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.TIPE_PENYAKIT_INFEKSI).ToList();
            List<StandardCode> lstCode3 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.TIPE_KOMORBID).ToList();

            Methods.SetComboBoxField<StandardCode>(cboGCInfectiousDisease, lstCode2, "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGCComorbidities, lstCode3, "StandardCodeName", "StandardCodeID");
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
            chkIsCITO.Checked = entity.IsCITO;
            chkIsUsedRequestTime.Checked = entity.IsUsedRequestTime;
            chkIsUsingSpecificItem.Checked = entity.IsUsingSpecificItem;
            txtScheduleDate.Text = entity.ScheduledDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtScheduleTime.Text = entity.ScheduledTime;

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

            chkIsUsingSpecificItem.Checked = entity.IsUsingSpecificItem;
            txtEstimatedDuration.Text = entity.EstimatedDuration.ToString();
            hdnRoomID.Value = entity.RoomID.ToString();
            txtRoomCode.Text = entity.RoomCode;
            txtRoomName.Text = entity.RoomName;
            txtRemarks.Text = entity.Remarks;

            rblIsHasInfectiousDisease.SelectedValue = entity.IsHasInfectiousDisease ? "1" : "0";
            if (entity.IsHasInfectiousDisease)
            {
                cboGCInfectiousDisease.Value = entity.GCInfectiousDisease;
                txtOtherInfectiousDisease.Text = entity.OtherInfectiousDisease;
                trInfectiousInfo.Style.Add("display", "table-row");
                if (cboGCInfectiousDisease.Value != null)
                {
                    if (cboGCInfectiousDisease.Value.ToString() == Constant.StandardCode.InfectiousDisease.OTHERS)
                        txtOtherInfectiousDisease.ReadOnly = false;
                }
            }
            else
            {
                trInfectiousInfo.Style.Add("display", "none");
            }

            rblIsHasComorbidities.SelectedValue = entity.IsHasComorbidities ? "1" : "0";
            if (entity.IsHasComorbidities)
            {
                cboGCComorbidities.Value = entity.GCComorbidities;
                txtOtherComorbidities.Text = entity.OtherComorbidities;
                trComorbiditiesInfo.Style.Add("display", "table-row");
                if (cboGCComorbidities.Value != null)
                {
                    if (cboGCComorbidities.Value.ToString() == Constant.StandardCode.Comorbidities.OTHERS)
                        txtOtherComorbidities.ReadOnly = false;
                }
            }
            else
            {
                trComorbiditiesInfo.Style.Add("display", "none");
            }

            BindGridViewProcedureGroup(1, false, ref gridProcedureGroupPageCount);
            BindGridViewParamedicTeam(1, false, ref gridParamedicTeamPageCount);
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
            entityHd.EstimatedDuration = Convert.ToInt32(txtEstimatedDuration.Text);
            entityHd.ScheduledDate = Helper.GetDatePickerValue(txtScheduleDate);
            entityHd.ScheduledTime = txtScheduleTime.Text;
            entityHd.IsUsedRequestTime = chkIsUsedRequestTime.Checked;
            entityHd.IsUsingSpecificItem = chkIsUsingSpecificItem.Checked;
            entityHd.IsOperatingRoomOrder = true;
            entityHd.IsEmergency = rblIsEmergency.SelectedValue == "1" ? true : false;

            if (!string.IsNullOrEmpty(hdnRoomID.Value))
            {
                entityHd.RoomID = Convert.ToInt32(hdnRoomID.Value);
            }

            if (chkIsNextVisit.Checked)
            {
                entityHd.GCToBePerformed = Constant.ToBePerformed.PRIOR_TO_NEXT_VISIT;
                entityHd.IsODSVisit = rblNextVisitType.SelectedValue == "1" ? true : false;
            }

            entityHd.IsHasInfectiousDisease = rblIsHasInfectiousDisease.SelectedValue == "1" ? true : false;
            if (entityHd.IsHasInfectiousDisease)
            {
                if (cboGCInfectiousDisease.Value != null)
                {
                    if (!string.IsNullOrEmpty(cboGCInfectiousDisease.Value.ToString()))
                    {
                        entityHd.GCInfectiousDisease = cboGCInfectiousDisease.Value.ToString();
                        if (cboGCInfectiousDisease.Value.ToString() == "X522^999")
                            entityHd.OtherInfectiousDisease = Page.Request.Form[txtOtherInfectiousDisease.UniqueID].ToString();
                        else
                            entityHd.OtherInfectiousDisease = null;
                    }
                }
            }

            entityHd.IsHasComorbidities = rblIsHasComorbidities.SelectedValue == "1" ? true : false;
            if (entityHd.IsHasComorbidities)
            {
                if (cboGCComorbidities.Value != null)
                {
                    if (!string.IsNullOrEmpty(cboGCComorbidities.Value.ToString()))
                    {
                        entityHd.GCComorbidities = cboGCComorbidities.Value.ToString();
                        if (cboGCComorbidities.Value.ToString() == "X523^999")
                            entityHd.OtherComorbidities = Page.Request.Form[txtOtherComorbidities.UniqueID];
                        else
                            entityHd.OtherComorbidities = null;
                    }
                }
            }

            entityHd.IsCITO = chkIsCITO.Checked;
            entityHd.Remarks = txtRemarks.Text;
            entityHd.TransactionCode = Constant.TransactionCode.OTHER_TEST_ORDER;
            entityHd.IsOperatingRoomOrder = true;
        }

        protected string GetUserID()
        {
            return AppSession.UserLogin.UserID.ToString();
        }

        #region Procedure Group
        private void BindGridViewProcedureGroup(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            if (Page.IsCallback && _orderID != "0")
            {
                hdnID.Value = _orderID;
            }

            List<vPatientSurgeryProcedureGroup> lstEntity1 = new List<vPatientSurgeryProcedureGroup>();
            if (hdnID.Value != "0")
            {
                string filterExpression = string.Format("VisitID = {0} AND TestOrderID = {1} AND IsDeleted = 0", hdnVisitID.Value, _orderID);

                lstEntity1 = BusinessLayer.GetvPatientSurgeryProcedureGroupList(filterExpression + " ORDER BY ProcedureGroupCode");
                grdProcedureGroupView.DataSource = lstEntity1;
                grdProcedureGroupView.DataBind();

                txtProcedureGroupSource.Text = "Laporan Operasi";
            }


            if (lstEntity1.Count == 0)
            {
                if (hdnID.Value != "0")
                {
                    List<vTestOrderDtProcedureGroup> lstEntity2 = new List<vTestOrderDtProcedureGroup>();
                    string filterExpression = string.Format("VisitID = {0} AND TestOrderID = {1} AND IsDeleted = 0", hdnVisitID.Value, _orderID);

                    lstEntity2 = BusinessLayer.GetvTestOrderDtProcedureGroupList(filterExpression + " ORDER BY ProcedureGroupCode");
                    grdProcedureGroupView.DataSource = lstEntity2;
                    grdProcedureGroupView.DataBind();

                    txtProcedureGroupSource.Text = "Order Kamar Operasi";
                } 
            }


        }
        protected void cbpProcedureGroupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewProcedureGroup(Convert.ToInt32(param[1]), true, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewProcedureGroup(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = string.Empty;
        }
        #endregion

        #region Paramedic Team
        private void BindGridViewParamedicTeam(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            if (Page.IsCallback && _orderID != "0")
            {
                hdnID.Value = _orderID;
            }

            List<vPatientSurgeryTeam> lstEntity1 = new List<vPatientSurgeryTeam>();
            if (hdnID.Value != "0")
            {
                string filterExpression = string.Format("VisitID = {0} AND TestOrderID = {1} AND IsDeleted = 0", hdnVisitID.Value, _orderID);

                lstEntity1 = BusinessLayer.GetvPatientSurgeryTeamList(filterExpression);
                grdParamedicTeamView.DataSource = lstEntity1;
                grdParamedicTeamView.DataBind();

                txtParamedicTeamSource.Text = "Laporan Operasi";
            }

            if (lstEntity1.Count == 0)
            {
                List<vTestOrderDtParamedicTeam> lstEntity = new List<vTestOrderDtParamedicTeam>();
                if (hdnID.Value != "0")
                {
                    string filterExpression = string.Format("VisitID = {0} AND TestOrderID = {1} AND IsDeleted = 0", hdnVisitID.Value, _orderID);
                    lstEntity = BusinessLayer.GetvTestOrderDtParamedicTeamList(filterExpression);
                }

                grdParamedicTeamView.DataSource = lstEntity;
                grdParamedicTeamView.DataBind();

                txtParamedicTeamSource.Text = "Order Kamar Operasi"; 
            }
        }
        protected void cbpParamedicTeamView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewParamedicTeam(Convert.ToInt32(param[1]), true, ref gridParamedicTeamPageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewParamedicTeam(1, true, ref gridParamedicTeamPageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion
    }
}