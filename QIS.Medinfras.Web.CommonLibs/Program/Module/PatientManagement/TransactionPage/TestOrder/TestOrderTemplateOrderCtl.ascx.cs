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
    public partial class TestOrderTemplateOrderCtl : BaseEntryPopupCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;

        public override void InitializeDataControl(string param)
        {
            hdnParam.Value = param;
            hdnTestOrderID.Value = hdnParam.Value.Split('|')[0];
            hdnIsLabUnit.Value = hdnParam.Value.Split('|')[13];
            SetControlProperties();
            BindGridView(1, true, ref PageCount);
        }

        private void SetControlProperties()
        {
            List<vSettingParameterDt> lstSettingParameter = BusinessLayer.GetvSettingParameterDtList(string.Format("ParameterCode IN ('{0}','{1}') AND HealthcareID = '{2}'", Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI, Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM, AppSession.UserLogin.HealthcareID));
            hdnImagingServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).ParameterValue;
            hdnLaboratoryServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).ParameterValue;

            if (hdnParam.Value.Split('|')[6] == hdnImagingServiceUnitID.Value)
            {
                hdnGCItemType.Value = Constant.ItemType.RADIOLOGI;
            }

            if (hdnIsLabUnit.Value == "1")
            {
                hdnGCItemType.Value = Constant.ItemType.LABORATORIUM;
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

            filterExpression = string.Format("HealthcareServiceUnitID = '{0}' AND IsDeleted = 0 AND ItemName1 LIKE '%{1}%' AND IsTestItem = 1", medicSupport, hdnFilterItem.Value);

            filterExpression += string.Format(" AND GCItemStatus != '{0}'", Constant.ItemStatus.IN_ACTIVE);

            filterExpression += string.Format(" AND ItemID IN (SELECT ItemID FROM TestTemplateDt WHERE TestTemplateID = {0})", hdnTestTemplateID.Value);

            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();

            if (hdnTestOrderID.Value != "0" && hdnTestOrderID.Value != "")
            {
                List<vTestOrderDt> lstItemID = BusinessLayer.GetvTestOrderDtList(string.Format("TestOrderID = {0}", hdnTestOrderID.Value));
                string lstSelectedID = "";
                foreach (vTestOrderDt itm in lstItemID)
                {
                    lstSelectedID += "," + itm.ItemID;
                }
                filterExpression += string.Format(" AND ItemID NOT IN({0})", lstSelectedID.Substring(1));
            }
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvServiceUnitItemRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_FIVE);
            }
            lstSelectedMember = hdnSelectedMember.Value.Split(',');

            List<vServiceUnitItem> lstEntity = BusinessLayer.GetvServiceUnitItemList(filterExpression, Constant.GridViewPageSize.GRID_FIVE, pageIndex, "ItemName1 ASC");
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

            try
            {
                TestOrderHd entityHd = null;
                string[] param = hdnParam.Value.Split('|');
                if (hdnTestOrderID.Value == "" || hdnTestOrderID.Value == "0")
                {
                    //TestOrderID|ParamedicID|TestOrderDate|TestOrderTime|HealthcareServiceUnitID|VisitID|ServiceUnitID|Remarks

                    entityHd = new TestOrderHd();
                    entityHd.HealthcareServiceUnitID = Convert.ToInt32(param[4]);
                    entityHd.FromHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                    entityHd.ParamedicID = Convert.ToInt32(param[1]);
                    entityHd.VisitID = Convert.ToInt32(param[5]);
                    entityHd.TestOrderDate = Helper.GetDatePickerValue(param[2]);
                    entityHd.TestOrderTime = param[3];
                    entityHd.GCToBePerformed = param[9];
                    entityHd.ScheduledDate = Helper.GetDatePickerValue(param[7]);
                    entityHd.ScheduledTime = param[8];
                    entityHd.Remarks = param[10];
                    entityHd.IsCITO = Convert.ToBoolean(param[11]);
                    if (param[6] == hdnImagingServiceUnitID.Value)
                    {
                        entityHd.TransactionCode = Constant.TransactionCode.IMAGING_TEST_ORDER;
                    }
                    else if (hdnIsLabUnit.Value == "1")
                    {
                        entityHd.TransactionCode = Constant.TransactionCode.LABORATORY_TEST_ORDER;
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
                    entityHdDao.Insert(entityHd);

                    entityHd.TestOrderID = BusinessLayer.GetTestOrderHdMaxID(ctx);
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
                        TestOrderDt entity = new TestOrderDt();
                        entity.TestOrderID = entityHd.TestOrderID;
                        entity.ItemID = Int32.Parse(id);
                        entity.DiagnoseID = DiagnoseID;
                        entity.GCTestOrderStatus = Constant.TestOrderStatus.OPEN;
                        entity.ItemQty = 1;
                        entity.IsCITO = Convert.ToBoolean(param[11]);
                        entity.ItemUnit = entityItemMasterDao.Get(entity.ItemID).GCItemUnit;
                        entity.CreatedBy = AppSession.UserLogin.UserID;
                        entityDao.Insert(entity);
                    }
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Transaksi tidak dapat diubah. Harap refresh halaman ini.";
                    ctx.RollBackTransaction();
                }
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