using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using System.Data;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class TestOrderLabQuickPicksCtl2 : BasePagePatientPageEntryCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;

        public override void InitializeDataControl(string param)
        {
            hdnParam.Value = param;
            string[] paramInfo = param.Split('|');
            hdnGCItemType.Value = paramInfo[0];
            hdnTestOrderID.Value = paramInfo[1];
            if (paramInfo.Count() >= 3)
                hdnDiagnosisSummary.Value = paramInfo[2];
            if (paramInfo.Count() >= 4)
                hdnChiefComplaint.Value = paramInfo[3];
            if (paramInfo.Count() >= 5)
                hdnHealthcareServiceUnitID.Value = paramInfo[4];

            if (paramInfo.Length >= 6)
            {
                hdnPostSurgeryInstructionID.Value = paramInfo[5];
            }
            else
            {
                hdnPostSurgeryInstructionID.Value = "0";
            }

            txtRemarks.Text = string.IsNullOrEmpty(hdnDiagnosisSummary.Value) ? hdnChiefComplaint.Value : hdnDiagnosisSummary.Value;

            SetControlProperties();
            BindGridView();
        }

        private void SetControlProperties()
        {
            List<SettingParameterDt> lstSettingParameter = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM, Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI));
            hdnImagingServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).ParameterValue;
            hdnLaboratoryServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).ParameterValue;

            string hsuFilterExp = string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsDeleted = 0 AND IsUsingJobOrder = 1", AppSession.UserLogin.HealthcareID, Constant.Facility.DIAGNOSTIC);
            if (hdnGCItemType.Value == Constant.ItemType.LABORATORIUM)
                hsuFilterExp += string.Format(" AND IsLaboratoryUnit = 1");

            List<vHealthcareServiceUnitCustom> lstServiceUnit = BusinessLayer.GetvHealthcareServiceUnitCustomList(hsuFilterExp);
            Methods.SetComboBoxField<vHealthcareServiceUnitCustom>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
            //if (hdnHealthcareServiceUnitID.Value == "" || hdnHealthcareServiceUnitID.Value == "0")
            //{
            //    if (hdnGCItemType.Value == Constant.ItemType.LABORATORIUM)
            //    {
            //        cboServiceUnit.Enabled = false;
            //        cboServiceUnit.Value = hdnLaboratoryServiceUnitID.Value;
            //        hdnHealthcareServiceUnitID.Value = cboServiceUnit.Value.ToString();
            //        tdPATest.Style.Remove("display");
            //        chkIsPathologicalAnatomyTest.Visible = true;
            //    }
            //    else
            //    {
            //        cboServiceUnit.Enabled = false;
            //        cboServiceUnit.Value = hdnImagingServiceUnitID.Value;
            //        hdnHealthcareServiceUnitID.Value = hdnImagingServiceUnitID.Value;
            //        tdPATest.Style.Add("display", "none");
            //        chkIsPathologicalAnatomyTest.Visible = false;
            //    }
            //}
            //else
            //{
            //    cboServiceUnit.Enabled = false;
            //    cboServiceUnit.Value = hdnHealthcareServiceUnitID.Value;
            //}

            if (hdnGCItemType.Value == Constant.ItemType.LABORATORIUM)
            {
                cboServiceUnit.Enabled = false;
                cboServiceUnit.Value = hdnLaboratoryServiceUnitID.Value;
                hdnHealthcareServiceUnitID.Value = cboServiceUnit.Value.ToString();
                tdPATest.Style.Remove("display");
                chkIsPathologicalAnatomyTest.Visible = true;
            }
            else
            {
                cboServiceUnit.Enabled = false;
                cboServiceUnit.Value = hdnImagingServiceUnitID.Value;
                hdnHealthcareServiceUnitID.Value = hdnImagingServiceUnitID.Value;
                tdPATest.Style.Add("display", "none");
                chkIsPathologicalAnatomyTest.Visible = false;
            }

            List<PatientDiagnosis> lstDiagnose = BusinessLayer.GetPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID));
            Methods.SetComboBoxField<PatientDiagnosis>(cboDiagnose, lstDiagnose, "DiagnosisText", "DiagnoseID");

            if (hdnTestOrderID.Value != "" || hdnTestOrderID.Value != "0")
            {
                TestOrderHd oHeader = BusinessLayer.GetTestOrderHd(Convert.ToInt32(hdnTestOrderID.Value));
                if (oHeader != null)
                {
                    txtDefaultStartDate.Text = hdnDefaultStartDate.Value = oHeader.ScheduledDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtDefaultStartTime.Text = hdnDefaultStartTime.Value = oHeader.ScheduledTime;
                    txtRemarks.Text = oHeader.Remarks;
                }
                else
                {
                    txtDefaultStartDate.Text = hdnDefaultStartDate.Value = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtDefaultStartTime.Text = hdnDefaultStartTime.Value = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                }
            }
            else
            {
                txtDefaultStartDate.Text = hdnDefaultStartDate.Value = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtDefaultStartTime.Text = hdnDefaultStartTime.Value = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            }
        }

        protected void cbpViewPopup_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
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

        private string GetFilterExpression()
        {

            string filterExpression = "";
            string itemType = string.IsNullOrEmpty(hdnGCItemType.Value) ? hdnParam.Value.Split('|')[0] : hdnGCItemType.Value;
            string healthcareServiceUnitID = GetHealthcareServiceUnitID(itemType);

            filterExpression += string.Format("HealthcareServiceUnitID = '{0}' AND IsDeleted = 0 AND ItemName1 LIKE '%{1}%' AND IsTestItem = 1", healthcareServiceUnitID, hdnFilterItem.Value);

            List<SettingParameterDt> lstSettingParameter = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.EM_PEMBATASAN_CPOE_BPJS, Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN));
            SettingParameterDt oParam1 = lstSettingParameter.Where(lst => lst.ParameterCode == Constant.SettingParameter.EM_PEMBATASAN_CPOE_BPJS).FirstOrDefault();
            SettingParameterDt oParam2 = lstSettingParameter.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN).FirstOrDefault();
            bool isLimitedCPOEItemForBPJS = oParam1 != null ? (oParam1.ParameterValue == "1" ? true : false) : false;
            int businnessPartnerID = oParam2 != null ? Convert.ToInt32(oParam2.ParameterValue) : 0;

            if ((AppSession.RegisteredPatient.BusinessPartnerID == AppSession.BusinessPartnerIDBPJS) && AppSession.IsLimitedCPOEItemForBPJS)
            {
                filterExpression += " AND IsBPJS = 1";
            }

            if (hdnFilterExpressionQuickSearch.Value != "")
            {
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            }

            return filterExpression;
        }

        private string GetHealthcareServiceUnitID(string itemType)
        {
            string serviceUnit = "0";

            switch (itemType)
            {
                case Constant.ItemType.LABORATORIUM:
                    serviceUnit = hdnHealthcareServiceUnitID.Value;
                    break;
                case Constant.ItemType.RADIOLOGI:
                    serviceUnit = hdnImagingServiceUnitID.Value;
                    break;
                default:
                    serviceUnit = hdnLaboratoryServiceUnitID.Value;
                    break;
            }
            return serviceUnit;
        }

        private void BindGridView()
        {
            string filterExpression = GetFilterExpression();

            if (hdnTestOrderID.Value != "0" && hdnTestOrderID.Value != "")
            {
                List<TestOrderDt> lstItemID = BusinessLayer.GetTestOrderDtList(string.Format("TestOrderID = {0} AND IsDeleted=0 ", hdnTestOrderID.Value));
                if (lstItemID.Count > 0)
                {
                    string lstSelectedID = "";
                    foreach (TestOrderDt itm in lstItemID)
                    {
                        lstSelectedID += "," + itm.ItemID;
                    }
                    filterExpression += string.Format(" AND ItemID NOT IN ({0})", lstSelectedID.Substring(1));

                }
            }



            lstSelectedMember = hdnSelectedMember.Value.Split(',');
            filterExpression += string.Format(" AND IsTestItem = 1 AND (IsDeleted = 0 AND GCItemStatus = '{0}') ORDER BY GroupOrder",Constant.ItemStatus.ACTIVE);
            lstEntity = BusinessLayer.GetvServiceUnitItemList(filterExpression);

            List<ItemGroupMaster> lstItemGroupMaster = (from p in lstEntity
                                                        select new ItemGroupMaster { ItemGroupID = p.ItemGroupID, ItemGroupCode = p.ItemGroupCode, ItemGroupName1 = p.ItemGroupName1, DisplayBackColor1 = p.DisplayBackColor1, DisplayBackColor2 = p.DisplayBackColor2, DisplayForeColor = p.DisplayForeColor }).GroupBy(p => p.ItemGroupCode).Select(p => p.First()).ToList();

            rptView.DataSource = lstItemGroupMaster;
            rptView.DataBind();
        }

        protected void rptView_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ItemGroupMaster entity = (ItemGroupMaster)e.Item.DataItem;
                HtmlGenericControl divGroupHeader = e.Item.FindControl("divGroupHeader") as HtmlGenericControl;
                HtmlGenericControl divGroupDetail = e.Item.FindControl("divGroupDetail") as HtmlGenericControl;

                if (divGroupHeader != null)
                {
                    if (!string.IsNullOrEmpty(entity.DisplayBackColor1))
                    {
                        divGroupHeader.Style.Add("background-color", entity.DisplayBackColor1);
                        divGroupDetail.Style.Add("background-color", entity.DisplayBackColor2);
                        divGroupHeader.Style.Add("color", entity.DisplayForeColor);
                    }
                    else 
                    {
                        divGroupHeader.Style.Add("background-color", "#747d8c");
                        divGroupDetail.Style.Add("background-color", "#dfe4ea");
                        divGroupHeader.Style.Add("color", "#2f3542");
                    }
                }
                else
                {
                    divGroupHeader.Style.Add("background-color", "#747d8c");
                    divGroupDetail.Style.Add("background-color", "#dfe4ea");
                    divGroupHeader.Style.Add("color", "#2f3542");
                }

                DataList rptDetail = (DataList)e.Item.FindControl("rptDetail");
                rptDetail.DataSource = lstEntity.Where(p => p.ItemGroupID == entity.ItemGroupID && p.IsTestItem == true).OrderBy(p => p.PrintOrder).ToList();
                rptDetail.DataBind();
            }
        }

        List<vServiceUnitItem> lstEntity = null;

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            TestOrderHdDao entityHdDao = new TestOrderHdDao(ctx);
            TestOrderDtDao entityDao = new TestOrderDtDao(ctx);
            ItemMasterDao entityItemMasterDao = new ItemMasterDao(ctx);
            bool result = false;
            lstSelectedMember = hdnSelectedMember.Value.Split(',');

            try
            {
                TestOrderHd entityHd = null;

                //TestOrderID|ParamedicID|TestOrderDate|TestOrderTime|HealthcareServiceUnitID|VisitID|ServiceUnitID|realizationDate|realizationTime|gcToBePerformed|isCITO
                string[] param = hdnParam.Value.Split('|');
                bool isAddNew = true;

                if (hdnTestOrderID.Value == "" || hdnTestOrderID.Value == "0")
                {
                    entityHd = new TestOrderHd();

                    ControlToEntity(entityHd);
                    entityHd.CreatedBy = AppSession.UserLogin.UserID;

                    entityHd.TestOrderID = BusinessLayer.GetTestOrderHdMaxID(ctx);
                }
                else
                {
                    isAddNew = false;
                    entityHd = BusinessLayer.GetTestOrderHd(Convert.ToInt32(hdnTestOrderID.Value));

                    ControlToEntity(entityHd);

                    entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entityHd);
                }

                string diagnoseID = string.Empty;

                if (cboDiagnose.Value != null)
                {
                    diagnoseID = cboDiagnose.Value.ToString();
                }

                List<TestOrderDt> lstDetail = new List<TestOrderDt>();

                foreach (String id in lstSelectedMember)
                {
                    if (!String.IsNullOrEmpty(id))
                    {
                        TestOrderDt entity = new TestOrderDt();
                        entity.TestOrderID = entityHd.TestOrderID;
                        entity.ItemID = Int32.Parse(id);
                        if (!string.IsNullOrEmpty(diagnoseID))
                        {
                            entity.DiagnoseID = diagnoseID;
                        }
                        entity.GCTestOrderStatus = Constant.TestOrderStatus.OPEN;
                        entity.IsCITO = chkIsCITO.Checked;
                        entity.ItemQty = 1;
                        entity.ItemUnit = "X003^X";
                        entity.ParamedicID = entityItemMasterDao.Get(entity.ItemID).DefaultParamedicID;
                        entity.Remarks = txtRemarks.Text;
                        entity.IsCreatedFromOrder = true;
                        entity.CreatedBy = AppSession.UserLogin.UserID;
                        lstDetail.Add(entity);
                    }
                }

                if (isAddNew)
                {
                    entityHd.TestOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.TestOrderDate);
                    entityHdDao.Insert(entityHd);
                    entityHd.TestOrderID = BusinessLayer.GetTestOrderHdMaxID(ctx);
                    hdnTestOrderID.Value = entityHd.TestOrderID.ToString(); 
                }

                foreach (TestOrderDt item in lstDetail)
                {
                    item.TestOrderID = entityHd.TestOrderID;
                    entityDao.Insert(item);
                }

                retval = entityHd.TestOrderID.ToString();
                hdnTestOrderID.Value = retval;

                ctx.CommitTransaction();
                result = true;
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

        private void ControlToEntity(TestOrderHd entityHd)
        {
            string itemType = string.IsNullOrEmpty(hdnGCItemType.Value) ? hdnParam.Value.Split('|').FirstOrDefault() : hdnGCItemType.Value;
            DateTime testOrderDate = Helper.GetDatePickerValue(hdnDefaultStartDate.Value);
            string testOrderTime = hdnDefaultStartTime.Value;
            //bool isCITO = hdnSelectedMemberIsCITO.Value.Contains('1');

            //entityHd.HealthcareServiceUnitID = Convert.ToInt32(GetHealthcareServiceUnitID(itemType));
            entityHd.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
            entityHd.FromHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
            entityHd.VisitHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
            entityHd.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
            entityHd.VisitID = AppSession.RegisteredPatient.VisitID;
            entityHd.TestOrderDate = testOrderDate;
            entityHd.TestOrderTime = testOrderTime;

            if (DateTime.Compare(testOrderDate, DateTime.Now.Date) > 0)
            {
                entityHd.GCToBePerformed = Constant.ToBePerformed.SCHEDULLED;
                entityHd.ScheduledDate = testOrderDate;
                entityHd.ScheduledTime = testOrderTime;
            }
            else
            {
                entityHd.GCToBePerformed = Constant.ToBePerformed.CURRENT_EPISODE;
                entityHd.ScheduledDate = entityHd.TestOrderDate;
                entityHd.ScheduledTime = entityHd.TestOrderTime;
            }

            if (hdnPostSurgeryInstructionID.Value != "0" && hdnPostSurgeryInstructionID.Value != "")
            {
                entityHd.PostSurgeryInstructionID = Convert.ToInt32(hdnPostSurgeryInstructionID.Value);
            }

            entityHd.IsCITO = chkIsCITO.Checked;
            entityHd.Remarks = txtRemarks.Text;

            if (hdnGCItemType.Value == Constant.ItemType.RADIOLOGI)
                entityHd.TransactionCode = Constant.TransactionCode.IMAGING_TEST_ORDER;
            else if (hdnGCItemType.Value == Constant.ItemType.LABORATORIUM)
            {
                entityHd.TransactionCode = Constant.TransactionCode.LABORATORY_TEST_ORDER;
                entityHd.IsPathologicalAnatomyTest = chkIsPathologicalAnatomyTest.Checked;
            }
            else
                entityHd.TransactionCode = Constant.TransactionCode.OTHER_TEST_ORDER;

            entityHd.GCOrderStatus = Constant.OrderStatus.OPEN;
            entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
        }
    }
}