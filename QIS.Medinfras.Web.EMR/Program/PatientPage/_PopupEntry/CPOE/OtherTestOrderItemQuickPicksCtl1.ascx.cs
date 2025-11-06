using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class OtherTestOrderItemQuickPicksCtl1 : BasePagePatientPageEntryCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;

        public override void InitializeDataControl(string param)
        {
            hdnDateToday.Value = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            Int32 hour = DateTime.Now.Hour;
            Int32 minute = DateTime.Now.Minute;
            string hourInString = "";
            string minuteInString = "";
            if (hour < 10)
            {
                hourInString = string.Format("0{0}", hour);
            }
            else
            {
                hourInString = string.Format("{0}", hour);
            }

            if (minute < 10)
            {
                minuteInString = string.Format("0{0}", minute);
            }
            else
            {
                minuteInString = string.Format("{0}", minute);
            }
            hdnTimeToday.Value = string.Format("{0}:{1}", hourInString, minuteInString);

            IsAdd = true;

            hdnParam.Value = param;

            string[] paramInfo = param.Split('|');
            hdnGCItemType.Value = paramInfo[0];
            hdnHealthcareServiceUnitID.Value = paramInfo[1];
            hdnTestOrderID.Value = paramInfo[2];

            if (paramInfo.Length >= 4)
                hdnDiagnosisSummary.Value = paramInfo[3];
            if (paramInfo.Length >= 5)
                hdnChiefComplaint.Value = paramInfo[4];

            hdnGCToBePerformed.Value = paramInfo[5];
            txtToBePerformed.Text = paramInfo[6];
            txtPerformDateCtl.Text = paramInfo[7];
            txtPerformTimeCtl.Text = paramInfo[8];

            if (paramInfo.Length >= 10)
            {
                hdnIsDIsabledHSUOption.Value = paramInfo[9];
            }
            else
            {
                hdnIsDIsabledHSUOption.Value = "0";
            }

            if (paramInfo.Length >= 11)
            {
                if (!string.IsNullOrEmpty(paramInfo[10]))
                {
                    hdnCheckedIsUsingMultiVisitScheduleOrder.Value = paramInfo[10];
                }
            }

            txtRemarks.Text = string.IsNullOrEmpty(hdnDiagnosisSummary.Value) ? hdnChiefComplaint.Value : hdnDiagnosisSummary.Value;
           
            SetControlProperties();
            BindGridView(1, true, ref PageCount);
        }

        private void SetControlProperties()
        {
            List<SettingParameterDt> lstSettingParameter = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')",
                AppSession.UserLogin.HealthcareID,
                Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM,
                Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI,
                Constant.SettingParameter.EM0094,
                Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN,
                Constant.SettingParameter.OP0017,
                Constant.SettingParameter.OP0018,
                Constant.SettingParameter.OP0019,
                Constant.SettingParameter.NOTES_TEST_ORDER_COPY_DIAGNOSE,
                Constant.SettingParameter.MD_IS_USING_MULTIVISIT_SCHEDULE));

            hdnImagingServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).ParameterValue;
            hdnLaboratoryServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).ParameterValue;
            hdnOPMaxLBTestItem.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.OP0017).ParameterValue;
            hdnOPMaxISTestItem.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.OP0018).ParameterValue;
            hdnOPMaxMDTestItem.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.OP0019).ParameterValue;
            hdnIsNotesCopyDiagnose.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.NOTES_TEST_ORDER_COPY_DIAGNOSE).ParameterValue;
            hdnDefaultVisitParamedicID.Value = AppSession.RegisteredPatient.ParamedicID.ToString();
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnIsUsingMultiVisitScheduleOrder.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.MD_IS_USING_MULTIVISIT_SCHEDULE).ParameterValue;

            SettingParameterDt oParam2 = lstSettingParameter.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN).FirstOrDefault();
            bool isLimitedCPOEItemForBPJS = hdnIsLimitedCPOEItemForBPJSOth.Value != null ? (hdnIsLimitedCPOEItemForBPJSOth.Value == "1" ? true : false) : false;
            int businessPartnerID = oParam2 != null ? Convert.ToInt32(oParam2.ParameterValue) : 0;

            hdnBusinessPartnerJKN.Value = businessPartnerID.ToString();

            if (hdnIsNotesCopyDiagnose.Value == "1")
            {
                string filterDiag = string.Format("VisitID = {0} AND ParamedicID = {1} AND IsDeleted = 0 ORDER BY GCDiagnoseType", hdnVisitID.Value, hdnDefaultVisitParamedicID.Value);
                List<vPatientDiagnosis> lstDiagnosis = BusinessLayer.GetvPatientDiagnosisList(filterDiag);

                //Create Diagnosis Summary for : CPOE Clinical Notes
                StringBuilder strDiagnosis = new StringBuilder();
                foreach (var item in lstDiagnosis)
                {
                    if (item.GCDifferentialStatus != Constant.DifferentialDiagnosisStatus.RULED_OUT)
                    {
                        strDiagnosis.AppendLine(string.Format("{0}", item.cfDiagnosisText));
                    }
                }
                hdnDefaultDiagnosa.Value = strDiagnosis.ToString();
            }
            else
            {
                hdnDefaultDiagnosa.Value = "";
            }

            string filterCC = string.Format("VisitID = {0} AND ParamedicID = {1} AND IsDeleted = 0 ORDER BY ID", hdnVisitID.Value, hdnDefaultVisitParamedicID.Value);
            List<vChiefComplaint> lstChiefComplaint = BusinessLayer.GetvChiefComplaintList(filterCC);
            if (lstChiefComplaint.Count > 0)
            {
                //Create Diagnosis Summary for : CPOE Clinical Notes
                StringBuilder strChiefComplaint = new StringBuilder();
                foreach (var item in lstChiefComplaint)
                {
                    strChiefComplaint.AppendLine(string.Format("{0}", item.ChiefComplaintText));
                }
                hdnDefaultChiefComplaint.Value = strChiefComplaint.ToString();
            }
            else
            {
                hdnDefaultChiefComplaint.Value = "";
            }

            txtRemarks.Text = string.IsNullOrEmpty(hdnDefaultDiagnosa.Value) ? hdnDefaultChiefComplaint.Value : hdnDefaultDiagnosa.Value;
            hdnRemarks.Value = string.IsNullOrEmpty(hdnDefaultDiagnosa.Value) ? hdnDefaultChiefComplaint.Value : hdnDefaultDiagnosa.Value;

            List<vHealthcareServiceUnit> lstServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format(
                                                                    "HealthcareID = '{0}' AND DepartmentID = '{1}' AND HealthcareServiceUnitID NOT IN ({2},{3}) AND IsDeleted = 0 AND IsLaboratoryUnit = 0 AND IsUsingJobOrder = 1",
                                                                    AppSession.UserLogin.HealthcareID, Constant.Facility.DIAGNOSTIC,
                                                                    hdnLaboratoryServiceUnitID.Value, hdnImagingServiceUnitID.Value));
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
            cboServiceUnit.Enabled = true;
            cboServiceUnit.SelectedIndex = 0;

            if (hdnIsDIsabledHSUOption.Value == "1")
            {
                if (hdnHealthcareServiceUnitID.Value == "" || hdnHealthcareServiceUnitID.Value == "0")
                {
                    cboServiceUnit.Enabled = true;
                    cboServiceUnit.SelectedIndex = 0;
                }
                else
                {
                    cboServiceUnit.Enabled = false;
                    cboServiceUnit.Value = hdnHealthcareServiceUnitID.Value;

                    if (hdnIsUsingMultiVisitScheduleOrder.Value == "1")
                    {
                        vHealthcareServiceUnitCustom hsu = BusinessLayer.GetvHealthcareServiceUnitCustomList(string.Format("HealthcareServiceUnitID = {0}", hdnHealthcareServiceUnitID.Value)).FirstOrDefault();
                        if (hsu != null)
                        {
                            if (hsu.IsAllowMultiVisitSchedule)
                            {
                                if (!string.IsNullOrEmpty(hdnCheckedIsUsingMultiVisitScheduleOrder.Value))
                                {
                                    chkIsMultiVisitScheduleOrder.Checked = Convert.ToBoolean(hdnCheckedIsUsingMultiVisitScheduleOrder.Value);
                                }
                                else
                                {
                                    chkIsMultiVisitScheduleOrder.Checked = false;
                                }
                                trMultiVisitScheduleOrder.Attributes.Remove("style");
                                //SetControlEntrySetting(chkIsMultiVisitScheduleOrder, new ControlEntrySetting(true, false, false));
                                chkIsMultiVisitScheduleOrder.Attributes.Add("Enabled", "false");
                            }
                            else
                            {
                                trMultiVisitScheduleOrder.Attributes.Add("style", "display:none");
                                chkIsMultiVisitScheduleOrder.Checked = false;
                            }
                        }
                        else
                        {
                            trMultiVisitScheduleOrder.Attributes.Add("style", "display:none");
                            chkIsMultiVisitScheduleOrder.Checked = false;
                        }
                    }
                    else
                    {
                        trMultiVisitScheduleOrder.Attributes.Add("style", "display:none");
                        chkIsMultiVisitScheduleOrder.Checked = false;
                    }
                }
            }

            if (hdnIsLimitedCPOEItemForBPJSOth.Value == "1")
            {
                if (AppSession.RegisteredPatient.BusinessPartnerID == businessPartnerID)
                {
                    rblItemType.SelectedValue = "2";
                    rblItemType.Enabled = !isLimitedCPOEItemForBPJS;
                }
            }

            txtDefaultStartDate.Enabled = false;
            txtDefaultStartTime.Enabled = false;
            txtToBePerformed.Enabled = false;
            txtPerformDateCtl.Enabled = false;
            txtPerformTimeCtl.Enabled = false;

            if (hdnTestOrderID.Value != "" || hdnTestOrderID.Value != "0")
            {
                TestOrderHd oHeader = BusinessLayer.GetTestOrderHd(Convert.ToInt32(hdnTestOrderID.Value));
                if (oHeader != null)
                {
                    txtDefaultStartDate.Text = oHeader.TestOrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtDefaultStartTime.Text = oHeader.TestOrderTime;
                    if (oHeader.Remarks == null || oHeader.Remarks == "")
                    {
                        txtRemarks.Text = hdnRemarks.Value;
                    }
                    else
                    {
                        txtRemarks.Text = oHeader.Remarks;
                    }

                    if (hdnIsUsingMultiVisitScheduleOrder.Value == "1")
                    {
                        vHealthcareServiceUnitCustom hsu = BusinessLayer.GetvHealthcareServiceUnitCustomList(string.Format("HealthcareServiceUnitID = {0}", oHeader.HealthcareServiceUnitID)).FirstOrDefault();
                        if (hsu != null)
                        {
                            if (hsu.IsAllowMultiVisitSchedule)
                            {
                                if (!string.IsNullOrEmpty(hdnCheckedIsUsingMultiVisitScheduleOrder.Value))
                                {
                                    chkIsMultiVisitScheduleOrder.Checked = Convert.ToBoolean(hdnCheckedIsUsingMultiVisitScheduleOrder.Value);
                                }
                                else
                                {
                                    chkIsMultiVisitScheduleOrder.Checked = false;
                                }
                                trMultiVisitScheduleOrder.Attributes.Remove("style");
                            }
                            else
                            {
                                trMultiVisitScheduleOrder.Attributes.Remove("style");
                                chkIsMultiVisitScheduleOrder.Checked = false;
                            }
                        }
                        else
                        {
                            trMultiVisitScheduleOrder.Attributes.Remove("style");
                            chkIsMultiVisitScheduleOrder.Checked = false;
                        }
                    }
                    else
                    {
                        trMultiVisitScheduleOrder.Attributes.Remove("style");
                        chkIsMultiVisitScheduleOrder.Checked = false;
                    }

                    cboServiceUnit.Enabled = false;
                    hdnHealthcareServiceUnitID.Value = oHeader.HealthcareServiceUnitID.ToString();
                    cboServiceUnit.Value = oHeader.HealthcareServiceUnitID.ToString();
                }
                else
                {
                    txtDefaultStartTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                }
            }

            ParamedicMaster oParamedic = BusinessLayer.GetParamedicMaster(Convert.ToInt32(AppSession.UserLogin.ParamedicID));
            if (oParamedic != null)
            {
                hdnParamedicID.Value = oParamedic.ParamedicID.ToString();
                txtParamedicCode.Text = oParamedic.ParamedicCode;
                txtParamedicName.Text = oParamedic.FullName;
            }
        }

        protected void cbpPopup_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private string GetFilterExpression()
        {
            string filterExpression = "";
            string itemType = string.IsNullOrEmpty(hdnGCItemType.Value) ? hdnParam.Value.Split('|')[0] : hdnGCItemType.Value;
            string healthcareServiceUnitID = GetHealthcareServiceUnitID(itemType);

            filterExpression += string.Format("HealthcareServiceUnitID = '{0}' AND IsDeleted = 0 AND GCItemStatus = '{2}' AND (ItemName1 LIKE '%{1}%' OR ItemName2 LIKE '%{1}%' OR AlternateItemName LIKE '%{1}%') AND IsTestItem = 1", healthcareServiceUnitID, hdnFilterItem.Value, Constant.ItemStatus.ACTIVE);

            if (hdnItemGroupID.Value != "")
                filterExpression += string.Format(" AND ItemGroupID IN(SELECT ItemGroupID FROM vItemGroupMaster WHERE GCItemType IN ('{0}') AND DisplayPath LIKE '%/{1}/%') AND IsDeleted = 0", hdnGCItemType.Value, hdnItemGroupID.Value);

            if (hdnTestTemplateID.Value != "")
            {
                filterExpression += string.Format(" AND ItemID IN (SELECT ItemID FROM TestTemplateDt WITH(NOLOCK) WHERE TestTemplateID = {0})", hdnTestTemplateID.Value);
            }

            switch (rblItemType.SelectedValue)
            {
                case "2":
                    filterExpression += " AND IsBPJS = 1 ";
                    break;
                default:
                    break;
            }

            return filterExpression;
        }

        private string GetHealthcareServiceUnitID(string itemType)
        {
            string serviceUnit = "0";

            switch (itemType)
            {
                case Constant.ItemType.LABORATORIUM:
                    serviceUnit = hdnLaboratoryServiceUnitID.Value;
                    break;
                case Constant.ItemType.RADIOLOGI:
                    serviceUnit = hdnImagingServiceUnitID.Value;
                    break;
                default:
                    serviceUnit = hdnHealthcareServiceUnitID.Value;
                    break;
            }
            return serviceUnit;
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vServiceUnitItem entity = e.Row.DataItem as vServiceUnitItem;
                CheckBox chkIsSelected = e.Row.FindControl("chkIsSelected") as CheckBox;
                if (lstSelectedMember.Contains(entity.ItemID.ToString()))
                    chkIsSelected.Checked = true;
            }
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (hdnTestOrderID.Value != "0" && hdnTestOrderID.Value != "")
            {
                List<vTestOrderDt> lstItemID = BusinessLayer.GetvTestOrderDtList(string.Format(
                    "TestOrderID = {0} AND GCTestOrderStatus != '{1}' AND IsDeleted = 0", hdnTestOrderID.Value, Constant.TestOrderStatus.CANCELLED));
                string lstSelectedID = "";
                if (lstItemID.Count > 0)
                {
                    foreach (vTestOrderDt itm in lstItemID)
                        lstSelectedID += "," + itm.ItemID;
                    filterExpression += string.Format(" AND ItemID NOT IN ({0})", lstSelectedID.Substring(1));
                }
            }
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvServiceUnitItemRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 10);
            }
            lstSelectedMember = hdnSelectedMember.Value.Split(',');

            List<vServiceUnitItem> lstEntity = BusinessLayer.GetvServiceUnitItemList(filterExpression, 10, pageIndex, "ItemName1 ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        private bool IsValidated(string lstItemID, ref string result)
        {
            StringBuilder tempMsg = new StringBuilder();
            string[] testItem = lstItemID.Split(',');

            string message = string.Empty;

            if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.OUTPATIENT)
            {
                if (AppSession.RegisteredPatient.BusinessPartnerID == Convert.ToInt32(hdnBusinessPartnerJKN.Value))
                {
                    string itemType = string.IsNullOrEmpty(hdnGCItemType.Value) ? hdnParam.Value.Split('|')[0] : hdnGCItemType.Value;
                    int serviceUnitID = Convert.ToInt32(GetHealthcareServiceUnitID(itemType));

                    string transactionCode = "";
                    int maxTest = 0;

                    if (serviceUnitID == Convert.ToInt32(hdnImagingServiceUnitID.Value))
                    {
                        transactionCode = Constant.TransactionCode.IMAGING_TEST_ORDER;
                        maxTest = Convert.ToInt32(hdnOPMaxISTestItem.Value);
                    }
                    else if (serviceUnitID == Convert.ToInt32(hdnLaboratoryServiceUnitID.Value))
                    {
                        transactionCode = Constant.TransactionCode.LABORATORY_TEST_ORDER;
                        maxTest = Convert.ToInt32(hdnOPMaxLBTestItem.Value);
                    }
                    else
                    {
                        transactionCode = Constant.TransactionCode.OTHER_TEST_ORDER;
                        maxTest = Convert.ToInt32(hdnOPMaxMDTestItem.Value);
                    }

                    string filterExpression = string.Format("TestOrderID IN (SELECT TestOrderID FROM TestOrderHd WHERE VisitID = {0} AND TransactionCode = '{1}') AND (GCTestOrderStatus != '{2}' OR IsDeleted = 0)", AppSession.RegisteredPatient.VisitID, transactionCode, Constant.OrderStatus.CANCELLED);
                    int noOfTest = BusinessLayer.GetvTestOrderDtRowCount(filterExpression);
                    int noOfNewTest = noOfTest + testItem.Length;

                    if (maxTest > 0 && noOfNewTest > maxTest)
                    {
                        tempMsg.AppendLine(string.Format("Jumlah Pemeriksaan untuk Pasien Rawat Jalan JKN telah melebihi kebijakan pengaturan jumlah pemeriksaan per kunjungan (maks. {0} pemeriksaan)", maxTest));
                    }
                }
            }

            message = tempMsg.ToString();
            result = message;

            return result == string.Empty;
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = false;

            if (!IsValidated(hdnSelectedMember.Value, ref errMessage))
            {
                result = false;
                retval = "0";
                return result;
            }

            IDbContext ctx = DbFactory.Configure(true);
            TestOrderHdDao entityOrderHdDao = new TestOrderHdDao(ctx);
            TestOrderDtDao entityOrderDtDao = new TestOrderDtDao(ctx);

            try
            {
                if (IsValid(ref errMessage))
                {
                    lstSelectedMember = hdnSelectedMember.Value.Split(',');
                    string[] lstSelectedMemberQty = hdnSelectedMemberItemQty.Value.Split(',');
                    string[] lstSelectedMemberName = hdnSelectedMemberName.Value.Split('^');
                    string[] lstSelectedMemberPRN = hdnSelectedMemberIsCITO.Value.Split(',');
                    string[] lstSelectedMemberRemarks = hdnSelectedMemberRemarks.Value.Split('^');

                    int testOrderID = 0;
                    string itemName = string.Empty;
                    string transactionNo = string.Empty;
                    string itemType = string.IsNullOrEmpty(hdnGCItemType.Value) ? hdnParam.Value.Split('|')[0] : hdnGCItemType.Value;

                    TestOrderHd entityHd;

                    if (hdnTestOrderID.Value == "" || hdnTestOrderID.Value == "0")
                    {
                        #region Test Order Header
                        entityHd = new TestOrderHd();

                        ControlToEntity(entityHd);

                        entityHd.CreatedBy = AppSession.UserLogin.UserID;
                        entityHd.TestOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.TestOrderDate);
                        testOrderID = entityOrderHdDao.InsertReturnPrimaryKeyID(entityHd);
                        entityHd.TestOrderID = testOrderID;
                        hdnTestOrderID.Value = testOrderID.ToString();
                        #endregion

                        result = true;
                    }
                    else
                    {
                        #region Test Order Header
                        entityHd = entityOrderHdDao.Get(Convert.ToInt32(hdnTestOrderID.Value));

                        if (entityHd.HealthcareServiceUnitID == Convert.ToInt32(hdnHealthcareServiceUnitID.Value))
                        {
                            ControlToEntity(entityHd);

                            testOrderID = entityHd.TestOrderID;

                            entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityOrderHdDao.Update(entityHd);
                        }
                        else
                        {
                            entityHd = new TestOrderHd();

                            ControlToEntity(entityHd);

                            entityHd.CreatedBy = AppSession.UserLogin.UserID;
                            entityHd.TestOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.TestOrderDate);
                            testOrderID = entityOrderHdDao.InsertReturnPrimaryKeyID(entityHd);
                            entityHd.TestOrderID = testOrderID;
                            hdnTestOrderID.Value = testOrderID.ToString();
                        }
                        #endregion
                    }

                    if (entityHd != null)
                    {
                        #region Test Order Detail
                        int counter = 0;
                        string[] lstCITO = hdnSelectedMemberIsCITO.Value.Split(',');

                        foreach (String id in lstSelectedMember)
                        {
                            TestOrderDt entity = new TestOrderDt();
                            entity.TestOrderID = entityHd.TestOrderID;
                            entity.ItemID = Int32.Parse(id);
                            entity.GCTestOrderStatus = Constant.TestOrderStatus.OPEN;
                            entity.IsCITO = lstCITO[counter] == "1" ? true : false;
                            entity.ItemQty = Convert.ToDecimal(lstSelectedMemberQty[counter]);
                            entity.ItemUnit = "X003^X";
                            entity.Remarks = lstSelectedMemberRemarks[counter];
                            entity.CreatedBy = AppSession.UserLogin.UserID;

                            if (string.IsNullOrEmpty(lstSelectedMemberRemarks[counter]))
                            {
                                entity.Remarks = entityHd.Remarks;
                            }
                            else
                            {
                                entity.Remarks = lstSelectedMemberRemarks[counter];
                            }

                            entityOrderDtDao.Insert(entity);

                            string remarks = string.IsNullOrEmpty(lstSelectedMemberRemarks[counter]) ? "" : "(" + lstSelectedMemberRemarks[counter] + ")";
                            if (itemName == string.Empty)
                            {
                                itemName = lstSelectedMemberName[counter] + remarks;
                            }
                            else
                            {
                                itemName = string.Format("{0};{1}{2}", itemName, lstSelectedMemberName[counter], remarks);
                            }

                            counter += 1;
                        }
                        #endregion

                        result = true;
                    }
                    else
                    {
                        errMessage = "Invalid Test Order Header information";
                        result = false;
                    }

                    if (result == true)
                    {
                        retval = string.Format("{0}|{1}", testOrderID.ToString(), itemName);
                        ctx.CommitTransaction();
                    }
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                ctx.RollBackTransaction();
                Helper.InsertErrorLog(ex);
                errMessage = ex.ToString();

                if (errMessage.IndexOf("truncated", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    errMessage = "Input tidak dapat diproses karena terdapat data yang terlalu panjang.";
                }
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        private void ControlToEntity(TestOrderHd entityHd)
        {
            string itemType = string.IsNullOrEmpty(hdnGCItemType.Value) ? hdnParam.Value.Split('|')[0] : hdnGCItemType.Value;
            DateTime testOrderDate = Helper.GetDatePickerValue(txtDefaultStartDate);
            string testOrderTime = txtDefaultStartTime.Text;
            bool isCITO = hdnSelectedMemberIsCITO.Value.Contains('1');

            entityHd.HealthcareServiceUnitID = Convert.ToInt32(GetHealthcareServiceUnitID(itemType));
            entityHd.FromHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
            entityHd.ParamedicID = (!string.IsNullOrEmpty(hdnParamedicID.Value) && hdnParamedicID.Value != "0") ? Convert.ToInt32(hdnParamedicID.Value) : Convert.ToInt32(AppSession.UserLogin.ParamedicID);
            entityHd.VisitID = AppSession.RegisteredPatient.VisitID;
            entityHd.VisitHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
            entityHd.TestOrderDate = testOrderDate;
            entityHd.TestOrderTime = testOrderTime;
            entityHd.IsMultiVisitScheduleOrder = chkIsMultiVisitScheduleOrder.Checked;

            if (entityHd.TestOrderDate.ToString("dd-MM-yyyy") == Constant.ConstantDate.DEFAULT_NULL)
            {
                entityHd.TestOrderDate = DateTime.Now;
            }

            if (String.IsNullOrEmpty(entityHd.TestOrderTime))
            {
                Int32 hour = DateTime.Now.Hour;
                Int32 minute = DateTime.Now.Minute;
                string hourInString = "";
                string minuteInString = "";
                if (hour < 10)
                {
                    hourInString = string.Format("0{0}", hour);
                }
                else
                {
                    hourInString = string.Format("{0}", hour);
                }

                if (minute < 10)
                {
                    minuteInString = string.Format("0{0}", minute);
                }
                else
                {
                    minuteInString = string.Format("{0}", minute);
                }
                entityHd.TestOrderTime = string.Format("{0}:{1}", hourInString, minuteInString);
            }

            entityHd.GCToBePerformed = hdnGCToBePerformed.Value.ToString();

            if (hdnGCToBePerformed.Value.ToString() == Constant.ToBePerformed.SCHEDULLED)
            {
                entityHd.ScheduledDate = Helper.GetDatePickerValue(Request.Form[txtPerformDateCtl.UniqueID]);
                entityHd.ScheduledTime = Request.Form[txtPerformTimeCtl.UniqueID];
            }
            else
            {
                entityHd.ScheduledDate = entityHd.TestOrderDate;
                entityHd.ScheduledTime = entityHd.TestOrderTime;
            }

            //if (DateTime.Compare(testOrderDate, DateTime.Now.Date) > 0)
            //{
            //    entityHd.GCToBePerformed = Constant.ToBePerformed.SCHEDULLED;
            //    entityHd.ScheduledDate = testOrderDate;
            //    entityHd.ScheduledTime = testOrderTime;
            //}
            //else
            //{
            //    entityHd.GCToBePerformed = Constant.ToBePerformed.CURRENT_EPISODE;
            //    entityHd.ScheduledDate = entityHd.TestOrderDate;
            //    entityHd.ScheduledTime = entityHd.TestOrderTime;
            //}

            entityHd.IsCITO = isCITO;
            entityHd.Remarks = txtRemarks.Text;

            if (hdnHealthcareServiceUnitID.Value == AppSession.ImagingServiceUnitID)
                entityHd.TransactionCode = Constant.TransactionCode.IMAGING_TEST_ORDER;
            else if (hdnHealthcareServiceUnitID.Value == AppSession.LaboratoryServiceUnitID)
                entityHd.TransactionCode = Constant.TransactionCode.LABORATORY_TEST_ORDER;
            else if (hdnHealthcareServiceUnitID.Value == AppSession.RT0001)
                entityHd.TransactionCode = Constant.TransactionCode.RADIOTHERAPHY_TEST_ORDER;
            else
                entityHd.TransactionCode = Constant.TransactionCode.OTHER_TEST_ORDER;

            entityHd.GCOrderStatus = Constant.OrderStatus.OPEN;
            entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
        }

        private bool IsValid(ref string errMessage)
        {
            bool isValid = true;
            if (string.IsNullOrEmpty(txtRemarks.Text) || txtRemarks.Text == "\n")
            {
                isValid = false;
                errMessage = "Catatan Klinis/Order harus disertakan untuk kebutuhan akreditasi";
            }
            return isValid;
        }
    }
}