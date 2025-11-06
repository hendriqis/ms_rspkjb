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

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class TestOrderLabQuickPicksCtl : BasePagePatientPageEntryCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;

        public override void InitializeDataControl(string param)
        {
            hdnParam.Value = param;
            hdnGCItemType.Value = hdnParam.Value.Split('|')[0];
            SetControlProperties();
            BindGridView();
        }

        private void SetControlProperties()
        {
            List<SettingParameterDt> lstSettingParameter = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM, Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI));
            hdnImagingServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).ParameterValue;
            hdnLaboratoryServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).ParameterValue;
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
            string medicSupport = "";
            medicSupport += hdnParam.Value.Split('|')[4];

            filterExpression += string.Format("HealthcareServiceUnitID = '{0}' AND IsDeleted = 0 AND ISNULL(GCItemStatus,'') != '{1}' AND ItemName1 LIKE '%{2}%' AND IsTestItem = 1", medicSupport, Constant.ItemStatus.IN_ACTIVE, hdnFilterItem.Value);

            List<SettingParameterDt> lstSettingParameter = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.EM_PEMBATASAN_CPOE_BPJS, Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN));
            SettingParameterDt oParam1 = lstSettingParameter.Where(lst => lst.ParameterCode == Constant.SettingParameter.EM_PEMBATASAN_CPOE_BPJS).FirstOrDefault();
            SettingParameterDt oParam2 = lstSettingParameter.Where(lst => lst.ParameterCode == Constant.SettingParameter.FN_PENJAMIN_BPJS_KESEHATAN).FirstOrDefault();
            bool isLimitedCPOEItemForBPJS = oParam1 != null ? (oParam1.ParameterValue == "1" ? true : false) : false;
            int businnessPartnerID = oParam2 != null ? Convert.ToInt32(oParam2.ParameterValue) : 0;

            if (AppSession.RegisteredPatient.BusinessPartnerID == businnessPartnerID)
            {
                filterExpression += " AND IsBPJS = 1";
            }

            return filterExpression;
        }

        private void BindGridView()
        {
            string filterExpression = GetFilterExpression();

            if (hdnTestOrderID.Value != "0" && hdnTestOrderID.Value != "")
            {
                List<vTestOrderDt> lstItemID = BusinessLayer.GetvTestOrderDtList(string.Format("TestOrderID = {0} AND IsDeleted=0", hdnTestOrderID.Value));
                if (lstItemID.Count > 0)
                {
                    string lstSelectedID = "";
                    foreach (vTestOrderDt itm in lstItemID)
                    {
                        lstSelectedID += "," + itm.ItemID;
                    }
                    filterExpression += string.Format(" AND ItemID NOT IN ({0})", lstSelectedID.Substring(1));

                }
            }



            lstSelectedMember = hdnSelectedMember.Value.Split(',');
            filterExpression += " AND IsTestItem = 1 ORDER BY ItemGroupCode,PrintOrder";
            lstEntity = BusinessLayer.GetvServiceUnitItemList(filterExpression);

            List<ItemGroupMaster> lstItemGroupMaster = (from p in lstEntity
                                                        select new ItemGroupMaster { ItemGroupID = p.ItemGroupID, ItemGroupCode = p.ItemGroupCode, ItemGroupName1 = p.ItemGroupName1 }).GroupBy(p => p.ItemGroupCode).Select(p => p.First()).ToList();

            rptView.DataSource = lstItemGroupMaster;
            rptView.DataBind();
        }

        protected void rptView_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ItemGroupMaster entity = (ItemGroupMaster)e.Item.DataItem;
                Repeater rptDetail = (Repeater)e.Item.FindControl("rptDetail");
                rptDetail.DataSource = lstEntity.Where(p => p.ItemGroupID == entity.ItemGroupID && p.IsTestItem == true).OrderBy(p => p.ItemGroupCode).ToList();
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

                if (hdnTestOrderID.Value == "" || hdnTestOrderID.Value == "0")
                {
                    entityHd = new TestOrderHd();
                    entityHd.HealthcareServiceUnitID = Convert.ToInt32(param[4]);
                    entityHd.FromHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                    entityHd.VisitHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                    entityHd.ParamedicID = Convert.ToInt32(param[1]);
                    entityHd.VisitID = Convert.ToInt32(param[5]);
                    entityHd.TestOrderDate = Helper.GetDatePickerValue(param[2]);
                    entityHd.TestOrderTime = param[3];
                    entityHd.GCToBePerformed = param[9];
                    entityHd.ScheduledDate = Helper.GetDatePickerValue(param[7]);
                    entityHd.ScheduledTime = param[8];
                    entityHd.IsCITO = param[10] == "true" ? true : false;
                    if (param[4] == hdnImagingServiceUnitID.Value)
                        entityHd.TransactionCode = Constant.TransactionCode.IMAGING_TEST_ORDER;
                    else if (param[4] == hdnLaboratoryServiceUnitID.Value)
                        entityHd.TransactionCode = Constant.TransactionCode.LABORATORY_TEST_ORDER;
                    else
                        entityHd.TransactionCode = Constant.TransactionCode.OTHER_TEST_ORDER;
                    entityHd.TestOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.TestOrderDate, ctx);
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityHd.GCOrderStatus = Constant.OrderStatus.OPEN;
                    entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                    entityHd.CreatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Insert(entityHd);

                    entityHd.TestOrderID = BusinessLayer.GetTestOrderHdMaxID(ctx);
                }
                else
                {
                    entityHd = BusinessLayer.GetTestOrderHd(Convert.ToInt32(hdnTestOrderID.Value));
                }
                retval = entityHd.TestOrderID.ToString();
                hdnTestOrderID.Value = retval;

                string DiagnoseID = txtDiagnoseID.Text;

                foreach (String id in lstSelectedMember)
                {
                    TestOrderDt entity = new TestOrderDt();
                    entity.TestOrderID = entityHd.TestOrderID;
                    entity.ItemID = Int32.Parse(id);
                    entity.DiagnoseID = DiagnoseID;
                    entity.GCTestOrderStatus = Constant.TestOrderStatus.OPEN;
                    entity.IsCITO = param[10] == "true" ? true : false;
                    entity.ItemQty = 1;
                    entity.ItemUnit = entityItemMasterDao.Get(entity.ItemID).GCItemUnit;
                    entity.Remarks = txtRemarks.Text;
                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    entityDao.Insert(entity);
                }
                ctx.CommitTransaction();
                result = true;
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                result = false;
                errMessage = ex.Message;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
    }
}