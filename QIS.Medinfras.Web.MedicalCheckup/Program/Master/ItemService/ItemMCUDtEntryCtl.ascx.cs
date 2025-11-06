using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.Web.MedicalCheckup.Program
{
    public partial class ItemMCUDtEntryCtl : BaseViewPopupCtl
    {

        protected int PageCount = 1;

        public override void InitializeDataControl(string param)
        {
            hdnItemID.Value = param;

            vItemService vItemService = BusinessLayer.GetvItemServiceList(String.Format("ItemID = {0} AND IsDeleted = 0", Convert.ToInt32(param))).FirstOrDefault();
            hdnGCTariffScheme.Value = vItemService.GCTariffScheme;
            txtTariffScheme.Text = vItemService.TariffScheme;
            txtItemServiceName.Text = string.Format("{0} - {1}", vItemService.ItemCode, vItemService.ItemName1);
            hdnIsUsingAccumulatedPrice.Value = vItemService.IsUsingAccumulatedPrice ? "1" : "0";

            List<Department> lstDept = BusinessLayer.GetDepartmentList(string.Format("IsActive = 1 AND DepartmentID IN ('{0}', '{1}')", Constant.Facility.OUTPATIENT, Constant.Facility.DIAGNOSTIC));
            lstDept.Add(new Department { DepartmentID = "", DepartmentName = "" });
            Methods.SetComboBoxField<Department>(cboDepartment, lstDept, "DepartmentName", "DepartmentID");
            cboDepartment.SelectedIndex = 0;
            hdnDepartmentID.Value = cboDepartment.Value.ToString();

            SettingParameterDt setvarMCUClass = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.MEDICAL_CHECKUP_CLASS);
            hdnMCUClassID.Value = setvarMCUClass.ParameterValue;

            decimal totalDetail = 0;
            BindGridView(1, true, ref PageCount, ref totalDetail);
            if (PageCount > 0) {
                hdnIsValueExist.Value = "1";
            }
            SetTariff(totalDetail);

            txtDetailItemCode.Attributes.Add("validationgroup", "mpEntryPopup");
            txtDetailItemQuantity.Attributes.Add("validationgroup", "mpEntryPopup");
        }

        private void SetTariff(decimal totalDetail)
        {
            decimal price = 0;

            string filterExpressionTariff = string.Format("ClassID = {0} AND GCTariffScheme = '{1}' AND ItemID = {2} AND HealthcareID = '{3}' AND StartingDate <= '{4}' ORDER BY StartingDate DESC",
                                                            hdnMCUClassID.Value, hdnGCTariffScheme.Value, hdnItemID.Value, AppSession.UserLogin.HealthcareID, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112));
            List<ItemTariff> entityItemTariffList = BusinessLayer.GetItemTariffList(filterExpressionTariff);
            if (entityItemTariffList.Count > 0)
            {
                ItemTariff entityItemTariff = entityItemTariffList.FirstOrDefault();
                price = entityItemTariff.Tariff;
            }

            hdnPackagePrice.Value = price.ToString();
            hdntxtPackagePrice.Value = txtPackagePrice.Text = price.ToString("N2");
            hdntxtTotalPrice.Value = txtTotalPrice.Text = totalDetail.ToString("N2");
            hdntxtGapPrice.Value = txtGapPrice.Text = (price - totalDetail).ToString("N2");

            if (hdnIsUsingAccumulatedPrice.Value == "1")
            {
                hdntxtGapPrice.Value = txtGapPrice.Text = " - ";
                hdntxtPackagePrice.Value = txtPackagePrice.Text = " - ";
            }
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount, ref decimal totalDetail)
        {
            string filter = "IsDeleted = 0";

            if (cboDepartment.Value != null && cboDepartment.Value.ToString() != "")
            {
                filter += string.Format(" AND DepartmentID = '{0}'", cboDepartment.Value);
            }

            filter += string.Format(" AND ItemID = {0}", hdnItemID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvItemServiceDtRowCount(filter);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_CTL);
            }
            if (pageCount > 0)
            {
                hdnIsValueExist.Value = "1";
            }
            else {
                hdnIsValueExist.Value = "0";
            }
            List<vItemServiceDt> lstItemServiceDt = BusinessLayer.GetvItemServiceDtList(filter, Constant.GridViewPageSize.GRID_CTL, pageIndex, "DepartmentID, ServiceUnitCode, ItemCode");
            List<vItemServiceDtWithPrice> lstItemServiceDtWithPrice = new List<vItemServiceDtWithPrice>();
            SetvItemServiceDtWithPrice(lstItemServiceDt, lstItemServiceDtWithPrice);

            string filter2 = string.Format("IsDeleted = 0 AND ItemID = {0}", hdnItemID.Value);
            List<vItemServiceDt> lstItemServiceDtAll = BusinessLayer.GetvItemServiceDtList(filter2);
            List<vItemServiceDtWithPrice> lstItemServiceDtWithPriceAll = new List<vItemServiceDtWithPrice>();
            SetvItemServiceDtWithPrice(lstItemServiceDtAll, lstItemServiceDtWithPriceAll);

            decimal totalQty = lstItemServiceDtWithPriceAll.Sum(a => a.Quantity);
            decimal totalPrice = lstItemServiceDtWithPriceAll.Sum(a => a.Price);
            decimal totalDisc = lstItemServiceDtWithPriceAll.Sum(a => a.DiscountAmount);

            totalDetail = lstItemServiceDtWithPriceAll.Sum(t => t.Quantity * (t.Price - t.DiscountAmount));

            grdView.DataSource = lstItemServiceDtWithPrice;
            grdView.DataBind();
        }

        private void SetvItemServiceDtWithPrice(List<vItemServiceDt> lstItemServiceDt, List<vItemServiceDtWithPrice> lstItemServiceDtWithPrice)
        {
            foreach (vItemServiceDt entity in lstItemServiceDt)
            {
                vItemServiceDtWithPrice entityvItemServiceDtWithPrice = new vItemServiceDtWithPrice();
                entityvItemServiceDtWithPrice.ID = entity.ID;
                entityvItemServiceDtWithPrice.ItemID = entity.ItemID;
                entityvItemServiceDtWithPrice.ItemCode = entity.ItemCode;
                entityvItemServiceDtWithPrice.ItemName1 = entity.ItemName1;
                entityvItemServiceDtWithPrice.DepartmentID = entity.DepartmentID;
                entityvItemServiceDtWithPrice.ParamedicID = entity.ParamedicID;
                entityvItemServiceDtWithPrice.ParamedicCode = entity.ParamedicCode;
                entityvItemServiceDtWithPrice.ParamedicName = entity.ParamedicName;
                entityvItemServiceDtWithPrice.DepartmentName = entity.DepartmentName;
                entityvItemServiceDtWithPrice.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
                entityvItemServiceDtWithPrice.ServiceUnitID = entity.ServiceUnitID;
                entityvItemServiceDtWithPrice.ServiceUnitCode = entity.ServiceUnitCode;
                entityvItemServiceDtWithPrice.ServiceUnitName = entity.ServiceUnitName;
                entityvItemServiceDtWithPrice.GCItemType = entity.GCItemType;
                entityvItemServiceDtWithPrice.ItemType = entity.ItemType;
                entityvItemServiceDtWithPrice.DetailItemID = entity.DetailItemID;
                entityvItemServiceDtWithPrice.DetailItemCode = entity.DetailItemCode;
                entityvItemServiceDtWithPrice.DetailItemName1 = entity.DetailItemName1;
                entityvItemServiceDtWithPrice.Quantity = entity.Quantity;
                entityvItemServiceDtWithPrice.GCItemUnit = entity.GCItemUnit;
                entityvItemServiceDtWithPrice.ItemUnit = entity.ItemUnit;
                entityvItemServiceDtWithPrice.DiscountAmount = entity.DiscountAmount;
                entityvItemServiceDtWithPrice.DiscountComp1 = entity.DiscountComp1;
                entityvItemServiceDtWithPrice.DiscountComp2 = entity.DiscountComp2;
                entityvItemServiceDtWithPrice.DiscountComp3 = entity.DiscountComp3;
                entityvItemServiceDtWithPrice.IsAutoPosted = entity.IsAutoPosted;
                entityvItemServiceDtWithPrice.IsAllowChanged = entity.IsAllowChanged;
                entityvItemServiceDtWithPrice.IsAllowEntryFromDesktop = entity.IsAllowEntryFromDesktop;
                entityvItemServiceDtWithPrice.IsDeleted = entity.IsDeleted;

                decimal tariff = 0, tariffComp1 = 0, tariffComp2 = 0, tariffComp3 = 0;
                decimal tariffDt = 0, tariffDtComp1 = 0, tariffDtComp2 = 0, tariffDtComp3 = 0;

                bool isPackageItem = false, isUsingAccumulatedPrice = false;

                ItemService iService = BusinessLayer.GetItemService(entity.DetailItemID);
                if (iService != null)
                {
                    isPackageItem = iService.IsPackageItem;
                    isUsingAccumulatedPrice = iService.IsUsingAccumulatedPrice;
                }

                if (isPackageItem && isUsingAccumulatedPrice)
                {
                    List<ItemServiceDt> lstDt = BusinessLayer.GetItemServiceDtList(string.Format("ItemID = {0} AND IsDeleted = 0", entity.DetailItemID));
                    foreach (ItemServiceDt dt in lstDt)
                    {
                        string filterExpressionTariff = string.Format("ClassID = {0} AND GCTariffScheme = '{1}' AND ItemID = {2} AND HealthcareID = '{3}' AND StartingDate <= '{4}' ORDER BY StartingDate DESC",
                                                                        hdnMCUClassID.Value, hdnGCTariffScheme.Value, dt.DetailItemID, AppSession.UserLogin.HealthcareID, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112));
                        ItemTariff entityItemTariff = BusinessLayer.GetItemTariffList(filterExpressionTariff).FirstOrDefault();
                        if (entityItemTariff != null)
                        {
                            tariffDt = entityItemTariff.Tariff;
                            tariffDtComp1 = entityItemTariff.TariffComp1;
                            tariffDtComp2 = entityItemTariff.TariffComp2;
                            tariffDtComp3 = entityItemTariff.TariffComp3;
                        }

                        tariff += (tariffDt * dt.Quantity);
                        tariffComp1 += (tariffDtComp1 * dt.Quantity);
                        tariffComp2 += (tariffDtComp2 * dt.Quantity);
                        tariffComp3 += (tariffDtComp3 * dt.Quantity);
                    }
                }
                else
                {
                    string filterExpressionTariff = string.Format("ClassID = {0} AND GCTariffScheme = '{1}' AND ItemID = {2} AND HealthcareID = '{3}' AND StartingDate <= '{4}' ORDER BY StartingDate DESC",
                                                                    hdnMCUClassID.Value, hdnGCTariffScheme.Value, entity.DetailItemID, AppSession.UserLogin.HealthcareID, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112));
                    ItemTariff entityItemTariff = BusinessLayer.GetItemTariffList(filterExpressionTariff).FirstOrDefault();
                    if (entityItemTariff != null)
                    {
                        tariff = entityItemTariff.Tariff;
                        tariffComp1 = entityItemTariff.TariffComp1;
                        tariffComp2 = entityItemTariff.TariffComp2;
                        tariffComp3 = entityItemTariff.TariffComp3;
                    }
                }

                entityvItemServiceDtWithPrice.Price = tariff;
                entityvItemServiceDtWithPrice.PriceComp1 = tariffComp1;
                entityvItemServiceDtWithPrice.PriceComp2 = tariffComp2;
                entityvItemServiceDtWithPrice.PriceComp3 = tariffComp3;

                lstItemServiceDtWithPrice.Add(entityvItemServiceDtWithPrice);
            }
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            }
        }

        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string[] param = e.Parameter.Split('|');
            string result = param[0] + "|";
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;

            decimal tariff = 0, tariffComp1 = 0, tariffComp2 = 0, tariffComp3 = 0;
            decimal tariffDt = 0, tariffDtComp1 = 0, tariffDtComp2 = 0, tariffDtComp3 = 0;

            bool isPackageItem = false, isUsingAccumulatedPrice = false;

            ItemService iService = BusinessLayer.GetItemService(Convert.ToInt32(param[1]));
            if (iService != null)
            {
                isPackageItem = iService.IsPackageItem;
                isUsingAccumulatedPrice = iService.IsUsingAccumulatedPrice;
            }

            if (isPackageItem && isUsingAccumulatedPrice)
            {
                List<ItemServiceDt> lstDt = BusinessLayer.GetItemServiceDtList(string.Format("ItemID = {0} AND IsDeleted = 0", param[1]));
                foreach (ItemServiceDt dt in lstDt)
                {
                    string filterExpressionTariff = string.Format("ClassID = {0} AND GCTariffScheme = '{1}' AND ItemID = {2} AND HealthcareID = '{3}' AND StartingDate <= '{4}' ORDER BY StartingDate DESC",
                                                                    hdnMCUClassID.Value, hdnGCTariffScheme.Value, dt.DetailItemID, AppSession.UserLogin.HealthcareID, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112));
                    List<ItemTariff> entityItemTariffList = BusinessLayer.GetItemTariffList(filterExpressionTariff);
                    if (entityItemTariffList.Count() > 0)
                    {
                        ItemTariff entityItemTariff = entityItemTariffList.FirstOrDefault();
                        tariffDt = entityItemTariff.Tariff;
                        tariffDtComp1 = entityItemTariff.TariffComp1;
                        tariffDtComp2 = entityItemTariff.TariffComp2;
                        tariffDtComp3 = entityItemTariff.TariffComp3;
                    }

                    tariff += (tariffDt * dt.Quantity);
                    tariffComp1 += (tariffDtComp1 * dt.Quantity);
                    tariffComp2 += (tariffDtComp2 * dt.Quantity);
                    tariffComp3 += (tariffDtComp3 * dt.Quantity);
                }
            }
            else
            {
                string filterExpressionTariff = string.Format("ClassID = {0} AND GCTariffScheme = '{1}' AND ItemID = {2} AND HealthcareID = '{3}' AND StartingDate <= '{4}' ORDER BY StartingDate DESC",
                                                                hdnMCUClassID.Value, hdnGCTariffScheme.Value, param[1], AppSession.UserLogin.HealthcareID, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112));
                List<ItemTariff> entityItemTariffList = BusinessLayer.GetItemTariffList(filterExpressionTariff);
                if (entityItemTariffList.Count() > 0)
                {
                    ItemTariff entityItemTariff = entityItemTariffList.FirstOrDefault();
                    tariff = entityItemTariff.Tariff;
                    tariffComp1 = entityItemTariff.TariffComp1;
                    tariffComp2 = entityItemTariff.TariffComp2;
                    tariffComp3 = entityItemTariff.TariffComp3;
                }
            }

            result += tariff + "|" + tariffComp1 + "|" + tariffComp2 + "|" + tariffComp3;

            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            string[] param = e.Parameter.Split('|');
            int pageCount = 1;
            decimal totalDetail = 0;
            string result = param[0] + "|";
            string errMessage = "";
            decimal totalPackage = Convert.ToDecimal(hdnPackagePrice.Value);
            if (param[0] == "changepage")
            {
                BindGridView(Convert.ToInt32(param[1]), false, ref pageCount, ref totalDetail);
                result = "changepage";
            }
            else if (param[0] == "refresh")
            {
                BindGridView(1, true, ref pageCount, ref totalDetail);
                result = "refresh|" + pageCount;
            }
            else if (param[0] == "refreshScheme")
            {
                BindGridView(1, true, ref pageCount, ref totalDetail);
                SetTariff(totalDetail);
                result = "refreshScheme|" + pageCount;
            }
            else if (param[0] == "save")
            {
                if (hdnID.Value.ToString() != "")
                {
                    if (OnSaveEditRecord(ref errMessage))
                    {
                        result += "success|";
                        BindGridView(1, true, ref pageCount, ref totalDetail);
                        if (hdnIsUsingAccumulatedPrice.Value == "0") result += totalDetail.ToString("N2") + "|" + (totalPackage - totalDetail).ToString("N2");
                        else result += totalDetail.ToString("N2") + "|" + " - ";
                    }
                    else
                    {
                        result += string.Format("fail|{0}", errMessage);
                    }
                }
                else
                {
                    if (OnSaveAddRecord(ref errMessage))
                    {
                        result += "success|";
                        BindGridView(1, true, ref pageCount, ref totalDetail);
                        if (hdnIsUsingAccumulatedPrice.Value == "0") result += totalDetail.ToString("N2") + "|" + (totalPackage - totalDetail).ToString("N2");
                        else result += totalDetail.ToString("N2") + "|" + " - ";
                    }
                    else
                    {
                        result += string.Format("fail|{0}", errMessage);
                    }
                }
            }
            else if (param[0] == "delete")
            {
                if (OnDeleteRecord(ref errMessage))
                {
                    result += "success|";
                    BindGridView(1, true, ref pageCount, ref totalDetail);
                    if (hdnIsUsingAccumulatedPrice.Value == "0") result += totalDetail.ToString("N2") + "|" + (totalPackage - totalDetail).ToString("N2");
                    else result += totalDetail.ToString("N2") + "|" + " - ";
                }
                else
                {
                    result += string.Format("fail|{0}", errMessage);
                }
            }
            result += "|" + pageCount;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void EntityToControl(ItemServiceDt entity)
        {

        }

        private void ControlToEntity(ItemServiceDt entity)
        {
            entity.ItemID = Convert.ToInt32(hdnItemID.Value);
            if (hdnDepartmentID.Value.ToString() == "" || hdnDepartmentID.Value.ToString() == null)
            {
                entity.DepartmentID = hdnDepartmentIDDetail.Value.ToString();
            }
            else
            {
                entity.DepartmentID = hdnDepartmentID.Value.ToString();
            }
            entity.ServiceUnitID = Convert.ToInt32(hdnServiceUnitID.Value);
            entity.GCItemType = Constant.ItemType.PELAYANAN;
            entity.DetailItemID = Convert.ToInt32(hdnDetailItemID.Value);
            entity.Quantity = Convert.ToDecimal(txtDetailItemQuantity.Text);
            entity.IsAllowChanged = chkIsAllowChanged.Checked;
            entity.IsAllowEntryFromDesktop = chkIsAllowEntryFromDesktop.Checked;
            entity.DiscountAmount = Convert.ToDecimal(hdnDiscountAmount.Value);
            entity.DiscountComp1 = Convert.ToDecimal(hdnDiscountAmount1.Value);
            entity.DiscountComp2 = Convert.ToDecimal(hdnDiscountAmount2.Value);
            entity.DiscountComp3 = Convert.ToDecimal(hdnDiscountAmount3.Value);
            if (String.IsNullOrEmpty(hdnParamedicID.Value))
            {
                entity.ParamedicID = null;
            }
            else
            {
                entity.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
            }
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                ItemServiceDt entity = new ItemServiceDt();
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entity.LastUpdatedDate = DateTime.Now;
                BusinessLayer.InsertItemServiceDt(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                ItemServiceDt entity = BusinessLayer.GetItemServiceDt(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateItemServiceDt(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            try
            {
                ItemServiceDt entity = BusinessLayer.GetItemServiceDt(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateItemServiceDt(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        #region vItemServiceDtWithPrice
        public class vItemServiceDtWithPrice
        {
            private Int32 _ID;
            private Int32 _ItemID;
            private String _ItemCode;
            private String _ItemName1;
            private String _DepartmentID;
            private Int32 _ParamedicID;
            private String _ParamedicCode;
            private String _ParamedicName;
            private String _DepartmentName;
            private Int32 _HealthcareServiceUnitID;
            private Int32 _ServiceUnitID;
            private String _ServiceUnitCode;
            private String _ServiceUnitName;
            private String _GCItemType;
            private String _ItemType;
            private Int32 _DetailItemID;
            private String _DetailItemCode;
            private String _DetailItemName1;
            private Decimal _Quantity;
            private String _GCItemUnit;
            private String _ItemUnit;
            private Decimal _DiscountAmount;
            private Decimal _DiscountComp1;
            private Decimal _DiscountComp2;
            private Decimal _DiscountComp3;
            private Boolean _IsAutoPosted;
            private Boolean _IsAllowChanged;
            private Boolean _IsAllowEntryFromDesktop;
            private Boolean _IsDeleted;
            private Decimal _Price;
            private Decimal _PriceComp1;
            private Decimal _PriceComp2;
            private Decimal _PriceComp3;

            public Int32 ID
            {
                get { return _ID; }
                set { _ID = value; }
            }
            public Int32 ItemID
            {
                get { return _ItemID; }
                set { _ItemID = value; }
            }
            public String ItemCode
            {
                get { return _ItemCode; }
                set { _ItemCode = value; }
            }
            public String ItemName1
            {
                get { return _ItemName1; }
                set { _ItemName1 = value; }
            }
            public String DepartmentID
            {
                get { return _DepartmentID; }
                set { _DepartmentID = value; }
            }
            public Int32 ParamedicID
            {
                get { return _ParamedicID; }
                set { _ParamedicID = value; }
            }
            public String ParamedicCode
            {
                get { return _ParamedicCode; }
                set { _ParamedicCode = value; }
            }
            public String ParamedicName
            {
                get { return _ParamedicName; }
                set { _ParamedicName = value; }
            }
            public String DepartmentName
            {
                get { return _DepartmentName; }
                set { _DepartmentName = value; }
            }
            public Int32 HealthcareServiceUnitID
            {
                get { return _HealthcareServiceUnitID; }
                set { _HealthcareServiceUnitID = value; }
            }
            public Int32 ServiceUnitID
            {
                get { return _ServiceUnitID; }
                set { _ServiceUnitID = value; }
            }
            public String ServiceUnitCode
            {
                get { return _ServiceUnitCode; }
                set { _ServiceUnitCode = value; }
            }
            public String ServiceUnitName
            {
                get { return _ServiceUnitName; }
                set { _ServiceUnitName = value; }
            }
            public String GCItemType
            {
                get { return _GCItemType; }
                set { _GCItemType = value; }
            }
            public String ItemType
            {
                get { return _ItemType; }
                set { _ItemType = value; }
            }
            public Int32 DetailItemID
            {
                get { return _DetailItemID; }
                set { _DetailItemID = value; }
            }
            public String DetailItemCode
            {
                get { return _DetailItemCode; }
                set { _DetailItemCode = value; }
            }
            public String DetailItemName1
            {
                get { return _DetailItemName1; }
                set { _DetailItemName1 = value; }
            }
            public Decimal Quantity
            {
                get { return _Quantity; }
                set { _Quantity = value; }
            }
            public String GCItemUnit
            {
                get { return _GCItemUnit; }
                set { _GCItemUnit = value; }
            }
            public String ItemUnit
            {
                get { return _ItemUnit; }
                set { _ItemUnit = value; }
            }
            public Decimal DiscountAmount
            {
                get { return _DiscountAmount; }
                set { _DiscountAmount = value; }
            }
            public Decimal DiscountComp1
            {
                get { return _DiscountComp1; }
                set { _DiscountComp1 = value; }
            }
            public Decimal DiscountComp2
            {
                get { return _DiscountComp2; }
                set { _DiscountComp2 = value; }
            }
            public Decimal DiscountComp3
            {
                get { return _DiscountComp3; }
                set { _DiscountComp3 = value; }
            }
            public Boolean IsAutoPosted
            {
                get { return _IsAutoPosted; }
                set { _IsAutoPosted = value; }
            }
            public Boolean IsAllowChanged
            {
                get { return _IsAllowChanged; }
                set { _IsAllowChanged = value; }
            }
            public Boolean IsAllowEntryFromDesktop
            {
                get { return _IsAllowEntryFromDesktop; }
                set { _IsAllowEntryFromDesktop = value; }
            }
            public Boolean IsDeleted
            {
                get { return _IsDeleted; }
                set { _IsDeleted = value; }
            }
            public Decimal Price
            {
                get { return _Price; }
                set { _Price = value; }
            }
            public Decimal PriceComp1
            {
                get { return _PriceComp1; }
                set { _PriceComp1 = value; }
            }
            public Decimal PriceComp2
            {
                get { return _PriceComp2; }
                set { _PriceComp2 = value; }
            }
            public Decimal PriceComp3
            {
                get { return _PriceComp3; }
                set { _PriceComp3 = value; }
            }

            public String MCUPrice
            {
                get
                {
                    return ((Price - DiscountAmount) * Quantity).ToString("N2");
                }
            }
        }
        #endregion
    }
}