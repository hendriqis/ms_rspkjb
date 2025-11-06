using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class TestOrderLabQuickPicksCtl : BaseEntryPopupCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;

        public override void InitializeDataControl(string param)
        {
            hdnParam.Value = param;
            hdnTestOrderID.Value = hdnParam.Value.Split('|')[0];
            hdnRegistrationID.Value = hdnParam.Value.Split('|')[12];
            hdnIsPATest.Value = hdnParam.Value.Split('|')[15];
            hdnIsLabUnit.Value = hdnParam.Value.Split('|')[13];

            Registration reg = BusinessLayer.GetRegistration(Convert.ToInt32(hdnRegistrationID.Value));
            hdnIsBPJSRegistration.Value = reg.GCCustomerType == Constant.CustomerType.BPJS ? "1" : "0";

            txtRemarks.Text = hdnParam.Value.Split('|')[10];
            hdnHealthcareServiceUnitIDCTL.Value = hdnParam.Value.Split('|')[4]; ;
            SetControlProperties();
            BindGridView();
        }

        private void SetControlProperties()
        {
            List<vSettingParameterDt> lstSettingParameter = BusinessLayer.GetvSettingParameterDtList(string.Format(
                                                                "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}')",
                                                                AppSession.UserLogin.HealthcareID, //0
                                                                Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID, //1
                                                                Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID, //2
                                                                Constant.SettingParameter.TEST_ORDER_HANYA_UNTUK_ITEM_PEMERIKSAAN, //3
                                                                Constant.SettingParameter.LB_PEMERIKSAAN_LABORATORIUM_HANYA_BPJS, //4, 
                                                                  Constant.SettingParameter.MC_ITEMTAMBAHAN_PROPOSE, //5
                                                                  Constant.SettingParameter.RT0001
                                                            ));
            hdnImagingServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID).ParameterValue;
            hdnLaboratoryServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID).ParameterValue;
            hdnOrderHanyaItemPemeriksaanQPCtl.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.TEST_ORDER_HANYA_UNTUK_ITEM_PEMERIKSAAN).ParameterValue;
            hdnIsOnlyBPJSItem.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LB_PEMERIKSAAN_LABORATORIUM_HANYA_BPJS).ParameterValue;
            hdnMCUItemTambahanProposeCtl.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.MC_ITEMTAMBAHAN_PROPOSE).ParameterValue;
            //string hdnImagingHSUID = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = {0} AND ServiceUnitID = {1}", AppSession.UserLogin.HealthcareID ,hdnImagingServiceUnitID.Value))[0].HealthcareServiceUnitID.ToString();
            //string hdnLabHSUID = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = {0} AND ServiceUnitID = {1}", AppSession.UserLogin.HealthcareID, hdnLaboratoryServiceUnitID.Value))[0].HealthcareServiceUnitID.ToString();
            hdnRadiotheraphyUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.RT0001).ParameterValue;

            hdnGCSubItemType.Value = string.Empty;
            if (hdnParam.Value.Split('|')[6] == hdnLaboratoryServiceUnitID.Value)
                hdnGCItemType.Value = Constant.ItemGroupMaster.LABORATORY;
            else if (hdnParam.Value.Split('|')[6] == hdnImagingServiceUnitID.Value)
                hdnGCItemType.Value = Constant.ItemGroupMaster.RADIOLOGY;
            else if (hdnParam.Value.Split('|')[4] == hdnRadiotheraphyUnitID.Value)
            {
                hdnGCItemType.Value = Constant.ItemGroupMaster.DIAGNOSTIC;
                hdnGCSubItemType.Value = Constant.SubItemType.RADIOTERAPI;
            }
            else
                hdnGCItemType.Value = Constant.ItemGroupMaster.DIAGNOSTIC;
            
            if (hdnMCUItemTambahanProposeCtl.Value == "1") 
            { 
                trPhysicanExecuter.Style.Remove("display");
                SetControlEntrySetting(txtExecutorParamedicCode, new ControlEntrySetting(true, true, true));
            }
        }

        protected void cbpViewPopup_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }

        protected void rptDetail_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                vServiceUnitItem entityDt = e.Item.DataItem as vServiceUnitItem;
                CheckBox chkIsSelected = (CheckBox)e.Item.FindControl("chkIsSelected");
                if (entityDt != null)
                {
                    if (lstSelectedMember.Contains(entityDt.ItemID.ToString()))
                    {
                        int idx = Array.IndexOf(lstSelectedMember, entityDt.ItemID.ToString());
                        chkIsSelected.Checked = true;
                    }
                }
            }
        }

        private string GetFilterExpression()
        {
            string filterExpression = "";
            string medicSupport = "";
            medicSupport += hdnParam.Value.Split('|')[4];
       
            if (hdnOrderHanyaItemPemeriksaanQPCtl.Value == "1")
            {
                filterExpression += string.Format("HealthcareServiceUnitID = '{0}' AND IsDeleted = 0 AND IsTestItem = 1", medicSupport);
            }
            else
            {
                filterExpression += string.Format("HealthcareServiceUnitID = '{0}' AND IsDeleted = 0", medicSupport);
            }

            filterExpression += string.Format(" AND GCItemStatus != '{0}'", Constant.ItemStatus.IN_ACTIVE);

            if (hdnIsBPJSRegistration.Value == "1")
            {
                if (hdnIsOnlyBPJSItem.Value == "1")
                {
                    filterExpression += " AND IsBPJS = 1";
                }
            }

            if (hdnFilterExpressionQuickSearch.Value != "")
            {
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            }

            return filterExpression;
        }

        private void BindGridView()
        {
            string filterExpression = GetFilterExpression();

            if (hdnTestOrderID.Value != "0" && hdnTestOrderID.Value != "")
            {
                List<vTestOrderDt> lstItemID = BusinessLayer.GetvTestOrderDtList(string.Format("TestOrderID = {0} AND IsDeleted = 0", hdnTestOrderID.Value));
                string lstSelectedID = "";
                foreach (vTestOrderDt itm in lstItemID)
                {
                    lstSelectedID += "," + itm.ItemID;
                }
                if (!lstSelectedID.Equals(string.Empty)) filterExpression += string.Format(" AND ItemID NOT IN ({0})", lstSelectedID.Substring(1));
            }

            if (hdnSelectedMember.Value != "")
            {
                lstSelectedMember = hdnSelectedMember.Value.Substring(1).Split(',');
            }
            else
            {
                lstSelectedMember = hdnSelectedMember.Value.Split(',');
            }

            filterExpression += " ORDER BY GroupOrder";
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
            bool result = true;

            string member = hdnSelectedMember.Value.Substring(1);
            lstSelectedMember = member.Split(',');

            try
            {
                if (hdnMCUItemTambahanProposeCtl.Value == "1") {
                    if (string.IsNullOrEmpty(hdnExecutorParamedicID.Value))
                    {
                        errMessage = "Harap pilih dokter pelaksana.";
                        result = false;
                        return result;
                    }
                }
                TestOrderHd entityHd = null;
                string[] param = hdnParam.Value.Split('|');
                if (hdnTestOrderID.Value == "" || hdnTestOrderID.Value == "0")
                {
                    //TestOrderID|ParamedicID|TestOrderDate|TestOrderTime|HealthcareServiceUnitID|VisitID|ServiceUnitID|Remarks

                    entityHd = new TestOrderHd();
                    entityHd.HealthcareServiceUnitID = Convert.ToInt32(param[4]);
                    if (!string.IsNullOrEmpty(AppSession.HealthcareServiceUnitID))
                    {
                        entityHd.FromHealthcareServiceUnitID = Convert.ToInt32(AppSession.HealthcareServiceUnitID);
                    }
                    else
                    {
                        entityHd.FromHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                    }
                    entityHd.VisitHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                    entityHd.ParamedicID = Convert.ToInt32(param[1]);
                    entityHd.VisitID = Convert.ToInt32(param[5]);
                    entityHd.TestOrderDate = Helper.GetDatePickerValue(param[2]);
                    entityHd.TestOrderTime = param[3];
                    entityHd.GCToBePerformed = param[9];
                    entityHd.ScheduledDate = Helper.GetDatePickerValue(param[7]);
                    entityHd.ScheduledTime = param[8];
                    entityHd.Remarks = param[10];
                    entityHd.IsCITO = Convert.ToBoolean(param[11]);
                    entityHd.IsMultiVisitScheduleOrder = Convert.ToBoolean(param[16]);
                    if (param[6] == hdnImagingServiceUnitID.Value)
                        entityHd.TransactionCode = Constant.TransactionCode.IMAGING_TEST_ORDER;
                    else if (hdnIsLabUnit.Value == "1")
                        entityHd.TransactionCode = Constant.TransactionCode.LABORATORY_TEST_ORDER;
                    else if (param[4] == hdnRadiotheraphyUnitID.Value)
                        entityHd.TransactionCode = Constant.TransactionCode.RADIOTHERAPHY_TEST_ORDER;
                    else
                        entityHd.TransactionCode = Constant.TransactionCode.OTHER_TEST_ORDER;
                    entityHd.TestOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.TestOrderDate, ctx);
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityHd.IsPathologicalAnatomyTest = Convert.ToBoolean(hdnIsPATest.Value);
                    entityHd.GCOrderStatus = Constant.OrderStatus.OPEN;
                    entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                    entityHd.CreatedBy = AppSession.UserLogin.UserID;
                    entityHd.TestOrderID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
                }
                else
                {
                    entityHd = BusinessLayer.GetTestOrderHd(Convert.ToInt32(hdnTestOrderID.Value));
                }
                retval = entityHd.TestOrderNo;

                string DiagnoseID = txtDiagnoseID.Text;

                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    foreach (String id in lstSelectedMember)
                    {
                        if (!String.IsNullOrEmpty(id))
                        {
                            TestOrderDt entity = new TestOrderDt();
                            entity.TestOrderID = entityHd.TestOrderID;
                            entity.ItemID = Int32.Parse(id);
                            entity.DiagnoseID = DiagnoseID;

                             int?  ParamedicID =  entityItemMasterDao.Get(entity.ItemID).DefaultParamedicID;
                            if (hdnMCUItemTambahanProposeCtl.Value == "1") {
                                if (!string.IsNullOrEmpty(hdnExecutorParamedicID.Value)) {
                                    ParamedicID = Convert.ToInt32(hdnExecutorParamedicID.Value);
                                }     
                            }

                            entity.ParamedicID = ParamedicID ;
                            entity.GCTestOrderStatus = Constant.TestOrderStatus.OPEN;
                            entity.ItemQty = 1;
                            entity.IsCITO = Convert.ToBoolean(param[11]);
                            entity.ItemUnit = entityItemMasterDao.Get(entity.ItemID).GCItemUnit;
                            entity.IsCreatedFromOrder = true;
                            entity.CreatedBy = AppSession.UserLogin.UserID;
                            entityDao.Insert(entity);
                        }
                    }
                }
                else
                {
                    errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
                    result = false;
                    ctx.RollBackTransaction();
                }
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
    }
}