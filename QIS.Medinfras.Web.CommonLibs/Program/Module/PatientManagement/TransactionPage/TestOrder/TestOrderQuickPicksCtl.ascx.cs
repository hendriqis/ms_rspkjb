using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class TestOrderQuickPicksCtl : BaseEntryPopupCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;

        public override void InitializeDataControl(string param)
        {
            hdnParam.Value = param;
            hdnTestOrderID.Value = hdnParam.Value.Split('|')[0];
            hdnRegistrationIDCtl.Value = hdnParam.Value.Split('|')[12];
            hdnVisitIDCtl.Value = hdnParam.Value.Split('|')[5];
            hdnTransactionDateQuickPicksCtl.Value = hdnParam.Value.Split('|')[2];
            hdnIsLabUnit.Value = hdnParam.Value.Split('|')[13];
            hdnHealthcareServiceUnitIDCTL2.Value = hdnParam.Value.Split('|')[4];
            ConsultVisit visit = BusinessLayer.GetConsultVisit(Convert.ToInt32(hdnVisitIDCtl.Value));
            hdnChargesClassIDCtl.Value = visit.ChargeClassID.ToString();

            SetControlProperties();
            BindGridView(1, true, ref PageCount);
        }

        private void SetControlProperties()
        {
            List<vSettingParameterDt> lstSettingParameter = BusinessLayer.GetvSettingParameterDtList(string.Format(
                                                                "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}')",
                                                                AppSession.UserLogin.HealthcareID, //0
                                                                Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI, //1
                                                                Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM, //2
                                                                Constant.SettingParameter.TEST_ORDER_HANYA_UNTUK_ITEM_PEMERIKSAAN, //3
                                                                Constant.SettingParameter.FN_IS_DISPLAY_PRICE_IN_ORDER_AND_SERVICES_QUICKPICKS_NURSE , //4
                                                                Constant.SettingParameter.MC_ITEMTAMBAHAN_PROPOSE, //5
                                                                Constant.SettingParameter.RT0001 //6
                                                            ));
            hdnImagingServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).ParameterValue;
            hdnLaboratoryServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).ParameterValue;
            hdnIsDisplayPrice.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_IS_DISPLAY_PRICE_IN_ORDER_AND_SERVICES_QUICKPICKS_NURSE).ParameterValue;
            hdnOrderHanyaItemPemeriksaanQPCtl.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.TEST_ORDER_HANYA_UNTUK_ITEM_PEMERIKSAAN).ParameterValue;
            hdnMCUItemTambahanProposeCtl.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.MC_ITEMTAMBAHAN_PROPOSE).ParameterValue;
            hdnRadiotheraphyServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.RT0001).ParameterValue;
           
            if (hdnIsLabUnit.Value == "1")
            {
                hdnGCItemType.Value = Constant.ItemType.LABORATORIUM;
            }
            else if (hdnParam.Value.Split('|')[6] == hdnImagingServiceUnitID.Value)
            {
                hdnGCItemType.Value = Constant.ItemType.RADIOLOGI;
            }
            else if (hdnParam.Value.Split('|')[6] == hdnRadiotheraphyServiceUnitID.Value)
            {
                hdnGCItemType.Value = Constant.ItemType.PENUNJANG_MEDIS;
                hdnGCSubItemType.Value = Constant.SubItemType.RADIOTERAPI;
            }
            else
            {
                hdnGCItemType.Value = Constant.ItemType.PENUNJANG_MEDIS;
                hdnGCSubItemType.Value = string.Empty;
            }

            if (hdnIsDisplayPrice.Value == "1")
            {
                trCalculate.Style.Remove("display");
                thHarga.Style.Remove("display");
            }
            else
            {
                trCalculate.Style.Add("display", "none");
                thHarga.Style.Add("display", "none");
            }

            if (hdnMCUItemTambahanProposeCtl.Value == "1")
            {
                trPhysicanExecuter.Style.Remove("display");
                SetControlEntrySetting(txtExecutorParamedicCode, new ControlEntrySetting(true, true, true));
            }
        }

        protected void cbpViewPopup_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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

            if (hdnOrderHanyaItemPemeriksaanQPCtl.Value == "1")
            {
                if (hdnItemGroupID.Value == "")
                {
                    filterExpression += string.Format("HealthcareServiceUnitID = '{0}' AND IsDeleted = 0 AND ItemName1 LIKE '%{1}%' AND IsTestItem = 1 AND GCItemStatus != '{2}'", medicSupport, hdnFilterItem.Value, Constant.ItemStatus.IN_ACTIVE);
                }
                else
                {
                    filterExpression += string.Format("HealthcareServiceUnitID = '{0}' AND IsDeleted = 0 AND ItemName1 LIKE '%{1}%' AND IsTestItem = 1 AND ItemGroupID IN(SELECT ItemGroupID FROM vItemGroupMaster1 WHERE DisplayPath LIKE '%/{2}/%') AND GCItemStatus != '{3}'", medicSupport, hdnFilterItem.Value, hdnItemGroupID.Value, Constant.ItemStatus.IN_ACTIVE);
                }
            }
            else
            {
                if (hdnItemGroupID.Value == "")
                {
                    filterExpression += string.Format("HealthcareServiceUnitID = '{0}' AND IsDeleted = 0 AND ItemName1 LIKE '%{1}%' AND GCItemStatus != '{2}'", medicSupport, hdnFilterItem.Value, Constant.ItemStatus.IN_ACTIVE);
                }
                else
                {
                    filterExpression += string.Format("HealthcareServiceUnitID = '{0}' AND IsDeleted = 0 AND ItemName1 LIKE '%{1}%' AND ItemGroupID IN(SELECT ItemGroupID FROM vItemGroupMaster1 WHERE DisplayPath LIKE '%/{2}/%') AND GCItemStatus != '{3}'", medicSupport, hdnFilterItem.Value, hdnItemGroupID.Value, Constant.ItemStatus.IN_ACTIVE);
                }
            }

            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
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
                if (lstSelectedID != "")
                {
                    filterExpression += string.Format(" AND ItemID NOT IN ({0})", lstSelectedID.Substring(1));
                }
            }
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvServiceUnitItemRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MATRIX);
            }
            lstSelectedMember = hdnSelectedMember.Value.Split(',');

            List<vServiceUnitItem> lstEntity = BusinessLayer.GetvServiceUnitItemList(filterExpression, Constant.GridViewPageSize.GRID_MATRIX, pageIndex, "ItemName1 ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            TestOrderHdDao entityHdDao = new TestOrderHdDao(ctx);
            TestOrderDtDao entityDao = new TestOrderDtDao(ctx);
            ItemMasterDao entityItemMasterDao = new ItemMasterDao(ctx);
            bool result = true;

            lstSelectedMember = hdnSelectedMember.Value.Split(',');
            string[] lstSelectedMemberQty = hdnSelectedMemberItemQty.Value.Split(',');

            try
            {
                if (hdnMCUItemTambahanProposeCtl.Value == "1")
                {
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
                    {
                        entityHd.TransactionCode = Constant.TransactionCode.IMAGING_TEST_ORDER;
                    }
                    else if (hdnIsLabUnit.Value == "1")
                    {
                        entityHd.TransactionCode = Constant.TransactionCode.LABORATORY_TEST_ORDER;
                    }
                    else if (param[4] == hdnRadiotheraphyServiceUnitID.Value)
                    {
                        entityHd.TransactionCode = Constant.TransactionCode.RADIOTHERAPHY_TEST_ORDER;
                    }
                    else
                    {
                        entityHd.TransactionCode = Constant.TransactionCode.OTHER_TEST_ORDER;
                    }
                    entityHd.TestOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.TestOrderDate, ctx);
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityHd.GCOrderStatus = Constant.OrderStatus.OPEN;
                    entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                    entityHd.CreatedBy = AppSession.UserLogin.UserID;
                    entityHd.TestOrderID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
                }
                else
                {
                    entityHd = entityHdDao.Get(Convert.ToInt32(hdnTestOrderID.Value));
                }
                retval = entityHd.TestOrderNo;

                string DiagnoseID = txtDiagnoseID.Text;

                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    for (int i = 0; i < lstSelectedMember.Count(); i++)
                    {
                        TestOrderDt entity = new TestOrderDt();
                        entity.TestOrderID = entityHd.TestOrderID;
                        entity.ItemID = Convert.ToInt32(lstSelectedMember[i]);
                        entity.ItemQty = Convert.ToDecimal(lstSelectedMemberQty[i]);

                        int? ParamedicID = entityItemMasterDao.Get(entity.ItemID).DefaultParamedicID;
                        if (hdnMCUItemTambahanProposeCtl.Value == "1")
                        {
                            if (!string.IsNullOrEmpty(hdnExecutorParamedicID.Value))
                            {
                                ParamedicID = Convert.ToInt32(hdnExecutorParamedicID.Value);
                            }
                        }
                        entity.ParamedicID = ParamedicID;
                        entity.DiagnoseID = DiagnoseID;
                        entity.GCTestOrderStatus = Constant.TestOrderStatus.OPEN;
                        entity.IsCITO = Convert.ToBoolean(param[11]);
                        entity.ItemUnit = entityItemMasterDao.Get(entity.ItemID).GCItemUnit;
                        entity.IsCreatedFromOrder = true;
                        entity.CreatedBy = AppSession.UserLogin.UserID;
                        entityDao.Insert(entity);
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
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