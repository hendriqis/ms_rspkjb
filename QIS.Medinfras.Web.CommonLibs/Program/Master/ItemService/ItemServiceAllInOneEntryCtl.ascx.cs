using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ItemServiceAllInOneEntryCtl : BaseEntryPopupCtl
    {
        class ClassCareInfo
        {
            public int ClassID { get; set; }
            public string ClassCode { get; set; }
            public string ClassName { get; set; }
            public Decimal MarginPercentage1 { get; set; }
            public Decimal MarginPercentage2 { get; set; }
            public Decimal MarginPercentage3 { get; set; }
            public Decimal Tariff { get; set; }
            public Decimal TariffComponent1 { get; set; }
            public Decimal TariffComponent2 { get; set; }
            public Decimal TariffComponent3 { get; set; }
            public Decimal DiscountAmount { get; set; }
            public Decimal DiscountComp1 { get; set; }
            public Decimal DiscountComp2 { get; set; }
            public Decimal DiscountComp3 { get; set; }
        }

        protected int ClassCount = 0;
        private List<ClassCareInfo> ListClassCare = null;
        List<SettingParameter> lstSettingParameter = null;

        protected int PageCount = 1;

        public override void SetToolbarVisibility(ref bool IsAllowAdd)
        {
            IsAllowAdd = false;
        }

        public override void InitializeDataControl(string param)
        {
            string[] paramList = param.Split('|');
            hdnProcessCtl.Value = paramList[0];
            hdnItemServiceDtIDCtl.Value = paramList[1];
            hdnPackageItemIDCtl.Value = paramList[2];

            IsAdd = hdnProcessCtl.Value == "add" ? true : false;

            BindingRptClass();
            lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}','{2}')",
                                                                                        Constant.SettingParameter.TARIFF_COMPONENT1_TEXT,
                                                                                        Constant.SettingParameter.TARIFF_COMPONENT2_TEXT,
                                                                                        Constant.SettingParameter.TARIFF_COMPONENT3_TEXT
                                                                                    ));

            ItemMaster pim = BusinessLayer.GetItemMaster(Convert.ToInt32(hdnPackageItemIDCtl.Value));
            hdnPackageGCItemTypeCtl.Value = pim.GCItemType;

            string filterSrvDt = string.Format("ID = {0} AND IsDeleted = 0", hdnItemServiceDtIDCtl.Value);
            vItemServiceDtAIO iSrvDt = BusinessLayer.GetvItemServiceDtAIOList(filterSrvDt).FirstOrDefault();
            if (iSrvDt != null)
            {
                string filterDept = string.Format("IsActive = 1 AND IsHasRegistration = 1");
                if (iSrvDt.PackageGCItemType == Constant.ItemType.LABORATORIUM || iSrvDt.PackageGCItemType == Constant.ItemType.RADIOLOGI || iSrvDt.PackageGCItemType == Constant.ItemType.PENUNJANG_MEDIS)
                {
                    filterDept += string.Format(" AND DepartmentID IN ('{0}')", Constant.Facility.DIAGNOSTIC);
                }
                else if (iSrvDt.PackageGCItemType == Constant.ItemType.PELAYANAN)
                {
                    filterDept += string.Format(" AND DepartmentID IN ('{0}','{1}','{2}')", Constant.Facility.DIAGNOSTIC, Constant.Facility.OUTPATIENT, Constant.Facility.INPATIENT);
                }
                else
                {
                    filterDept += string.Format(" AND DepartmentID IN ('{0}','{1}')", Constant.Facility.DIAGNOSTIC, Constant.Facility.OUTPATIENT);
                }
                List<Department> lstDept = BusinessLayer.GetDepartmentList(filterDept);
                Methods.SetComboBoxField<Department>(cboDepartment, lstDept, "DepartmentName", "DepartmentID");
                cboDepartment.Value = iSrvDt.DepartmentID;
                hdnDepartmentID.Value = iSrvDt.DepartmentID;

                hdnHealthcareServiceUnitID.Value = iSrvDt.HealthcareServiceUnitID.ToString();
                hdnServiceUnitID.Value = iSrvDt.ServiceUnitID.ToString();
                txtServiceUnitCode.Text = iSrvDt.ServiceUnitCode;
                txtServiceUnitName.Text = iSrvDt.ServiceUnitName;

                hdnDetailGCItemType.Value = iSrvDt.GCItemType;
                hdnDetailItemID.Value = iSrvDt.DetailItemID.ToString();
                txtDetailItemCode.Text = iSrvDt.DetailItemCode;
                txtDetailItemName.Text = iSrvDt.DetailItemName1;

                chkIsControlAmount.Checked = iSrvDt.IsControlAmount;
                if (iSrvDt.IsControlAmount)
                {
                    tdIsControlAmount.Attributes.Remove("style");
                }
                else
                {
                    tdIsControlAmount.Attributes.Add("style", "display:none");
                }

                txtDetailItemQuantity.Text = iSrvDt.Quantity.ToString();

                StringBuilder sbListClassID = new StringBuilder();
                StringBuilder sbListClassName = new StringBuilder();

                String filterTariff = string.Format("ItemServiceDtID = {0} AND IsDeleted = 0", iSrvDt.ID);
                List<vItemServiceDtTariff> lstTariff = BusinessLayer.GetvItemServiceDtTariffList(filterTariff);
                if (lstTariff.Count() > 0)
                {
                    vItemServiceDtTariff tariff = lstTariff.FirstOrDefault();
                    hdnBookID.Value = tariff.BookID.ToString();
                    txtDocumentNo.Text = tariff.DocumentNo;
                    hdnGCTariffScheme.Value = tariff.GCTariffScheme;
                    txtTariffScheme.Text = tariff.TariffScheme;
                    hdnStartDate.Value = tariff.StartingDateInDatePicker;
                    txtStartDate.Text = tariff.StartingDateInString;

                    List<ItemTariffListByBook> lst = BusinessLayer.GetItemTariffListByBook(AppSession.UserLogin.HealthcareID, Convert.ToInt32(lstTariff.FirstOrDefault().BookID), iSrvDt.DetailItemID);
                    foreach (ItemTariffListByBook classCare in lst)
                    {
                        if (sbListClassID.ToString() != "")
                        {
                            sbListClassID.Append("|");
                        }
                        sbListClassID.Append(classCare.ClassID);

                        if (sbListClassName.ToString() != "")
                        {
                            sbListClassName.Append("|");
                        }
                        sbListClassName.Append(classCare.ClassName);
                    }
                }
                else
                {
                    List<ClassCare> lstClassCare = BusinessLayer.GetClassCareList("IsUsedInChargeClass = 1 AND IsDeleted = 0 ORDER BY ClassCode");
                    foreach (ClassCare classCare in lstClassCare)
                    {
                        if (sbListClassID.ToString() != "")
                        {
                            sbListClassID.Append("|");
                            sbListClassName.Append("|");
                        }
                        sbListClassID.Append(classCare.ClassID);
                        sbListClassName.Append(classCare.ClassName);
                    }
                }
                string tmpClassID = sbListClassID.ToString();
                string tmpClassName = sbListClassName.ToString();

                string[] lstClassID = tmpClassID.Split('|');
                string[] lstClassName = tmpClassName.Split('|');
                ListClassCare = new List<ClassCareInfo>();
                for (int i = 0; i < lstClassID.Length; ++i)
                {
                    ClassCareInfo entityClassCare = new ClassCareInfo();
                    entityClassCare.ClassID = Convert.ToInt32(lstClassID[i]);
                    entityClassCare.ClassName = lstClassName[i];
                    ListClassCare.Add(entityClassCare);
                }
                rptClassTariff.DataSource = ListClassCare;
                rptClassTariff.DataBind();

                rptClassDiscount.DataSource = ListClassCare;
                rptClassDiscount.DataBind();

                foreach (RepeaterItem item in rptClassTariff.Items)
                {
                    TextBox txtClassTariff = (TextBox)item.FindControl("txtClassTariff");
                    TextBox txtTariffComponent1 = (TextBox)item.FindControl("txtTariffComponent1");
                    TextBox txtTariffComponent2 = (TextBox)item.FindControl("txtTariffComponent2");
                    TextBox txtTariffComponent3 = (TextBox)item.FindControl("txtTariffComponent3");
                    HtmlInputHidden hdnClassID = (HtmlInputHidden)item.FindControl("hdnClassID");

                    int classID = Convert.ToInt32(hdnClassID.Value);

                    vItemServiceDtTariff itemData = lstTariff.Where(t => t.ClassID == classID).FirstOrDefault();
                    if (itemData != null)
                    {
                        txtClassTariff.Text = itemData.Tariff.ToString(Constant.FormatString.NUMERIC_2);
                        txtTariffComponent1.Text = itemData.TariffComp1.ToString(Constant.FormatString.NUMERIC_2);
                        txtTariffComponent2.Text = itemData.TariffComp2.ToString(Constant.FormatString.NUMERIC_2);
                        txtTariffComponent3.Text = itemData.TariffComp3.ToString(Constant.FormatString.NUMERIC_2);
                    }
                    else
                    {
                        txtClassTariff.Text = "0.00";
                        txtTariffComponent1.Text = "0.00";
                        txtTariffComponent2.Text = "0.00";
                        txtTariffComponent3.Text = "0.00";
                    }
                }

                foreach (RepeaterItem item in rptClassDiscount.Items)
                {
                    TextBox txtClassDiscount = (TextBox)item.FindControl("txtClassDiscount");
                    TextBox txtDiscountComponent1 = (TextBox)item.FindControl("txtDiscountComponent1");
                    TextBox txtDiscountComponent2 = (TextBox)item.FindControl("txtDiscountComponent2");
                    TextBox txtDiscountComponent3 = (TextBox)item.FindControl("txtDiscountComponent3");
                    HtmlInputHidden hdnClassID = (HtmlInputHidden)item.FindControl("hdnClassIDDiscount");

                    int classID = Convert.ToInt32(hdnClassID.Value);

                    vItemServiceDtTariff itemData = lstTariff.Where(t => t.ClassID == classID).FirstOrDefault();
                    if (itemData != null)
                    {
                        txtClassDiscount.Text = itemData.DiscountAmount.ToString(Constant.FormatString.NUMERIC_2);
                        txtDiscountComponent1.Text = itemData.DiscountComp1.ToString(Constant.FormatString.NUMERIC_2);
                        txtDiscountComponent2.Text = itemData.DiscountComp2.ToString(Constant.FormatString.NUMERIC_2);
                        txtDiscountComponent3.Text = itemData.DiscountComp3.ToString(Constant.FormatString.NUMERIC_2);
                    }
                    else
                    {
                        txtClassDiscount.Text = "0.00";
                        txtDiscountComponent1.Text = "0.00";
                        txtDiscountComponent2.Text = "0.00";
                        txtDiscountComponent3.Text = "0.00";
                    }
                }
            }
            else
            {
                string filterDept = string.Format("IsActive = 1 AND IsHasRegistration = 1");
                if (hdnPackageGCItemTypeCtl.Value == Constant.ItemType.LABORATORIUM || hdnPackageGCItemTypeCtl.Value == Constant.ItemType.RADIOLOGI || hdnPackageGCItemTypeCtl.Value == Constant.ItemType.PENUNJANG_MEDIS)
                {
                    filterDept += string.Format(" AND DepartmentID IN ('{0}')", Constant.Facility.DIAGNOSTIC);
                }
                else if (hdnPackageGCItemTypeCtl.Value == Constant.ItemType.PELAYANAN)
                {
                    filterDept += string.Format(" AND DepartmentID IN ('{0}','{1}','{2}')", Constant.Facility.DIAGNOSTIC, Constant.Facility.OUTPATIENT, Constant.Facility.INPATIENT);
                }
                else
                {
                    filterDept += string.Format(" AND DepartmentID IN ('{0}','{1}')", Constant.Facility.DIAGNOSTIC, Constant.Facility.OUTPATIENT);
                }
                List<Department> lstDept = BusinessLayer.GetDepartmentList(filterDept);
                Methods.SetComboBoxField<Department>(cboDepartment, lstDept, "DepartmentName", "DepartmentID");
                cboDepartment.SelectedIndex = 0;
                hdnDepartmentID.Value = cboDepartment.Value.ToString();

                hdnHealthcareServiceUnitID.Value = "";
                hdnServiceUnitID.Value = "";
                txtServiceUnitCode.Text = "";
                txtServiceUnitName.Text = "";

                hdnDetailGCItemType.Value = "";
                hdnDetailItemID.Value = "";
                txtDetailItemCode.Text = "";
                txtDetailItemName.Text = "";

                txtDetailItemQuantity.Text = "1";
            }

        }

        private void BindingRptClass()
        {
            List<ClassCare> lstClassCare = BusinessLayer.GetClassCareList("IsUsedInChargeClass = 1 AND IsDeleted = 0 ORDER BY ClassCode");
            StringBuilder sbListClassID = new StringBuilder();
            StringBuilder sbListClassCode = new StringBuilder();
            StringBuilder sbListClassName = new StringBuilder();
            StringBuilder sbListClassMargin = new StringBuilder();

            foreach (ClassCare classCare in lstClassCare)
            {
                if (sbListClassID.ToString() != "")
                {
                    sbListClassID.Append("|");
                    sbListClassCode.Append("|");
                    sbListClassName.Append("|");
                    sbListClassMargin.Append("|");
                }
                sbListClassID.Append(classCare.ClassID);
                sbListClassCode.Append(classCare.ClassCode);
                sbListClassName.Append(classCare.ClassName);
                int marginPercentage = 0;
                if (hdnDetailGCItemType.Value == Constant.ItemType.PELAYANAN || hdnDetailGCItemType.Value == Constant.ItemType.RADIOLOGI || hdnDetailGCItemType.Value == Constant.ItemType.LABORATORIUM || hdnDetailGCItemType.Value == Constant.ItemType.PENUNJANG_MEDIS || hdnDetailGCItemType.Value == Constant.ItemType.MEDICAL_CHECKUP)
                    marginPercentage = (int)classCare.MarginPercentage1;
                else if (hdnDetailGCItemType.Value == Constant.ItemType.OBAT_OBATAN || hdnDetailGCItemType.Value == Constant.ItemType.BARANG_MEDIS || hdnDetailGCItemType.Value == Constant.ItemType.BARANG_UMUM)
                    marginPercentage = (int)classCare.MarginPercentage2;
                else if (String.IsNullOrEmpty(hdnDetailGCItemType.Value))
                    marginPercentage = 0;
                else
                    marginPercentage = (int)classCare.MarginPercentage3;
                sbListClassMargin.Append(marginPercentage);
            }
            hdnListClassID.Value = sbListClassID.ToString();
            hdnListClassCode.Value = sbListClassCode.ToString();
            hdnListClassName.Value = sbListClassName.ToString();
            hdnListClassMargin.Value = sbListClassMargin.ToString();

            string[] lstClassID = hdnListClassID.Value.Split('|');
            string[] lstClassCode = hdnListClassCode.Value.Split('|');
            string[] lstClassName = hdnListClassName.Value.Split('|');
            string[] lstClassMargin = hdnListClassMargin.Value.Split('|');
            ListClassCare = new List<ClassCareInfo>();
            for (int i = 0; i < lstClassID.Length; ++i)
            {
                ClassCareInfo entity = new ClassCareInfo();
                entity.ClassID = Convert.ToInt32(lstClassID[i]);
                entity.ClassName = lstClassName[i];
                ListClassCare.Add(entity);
            }

            ClassCount = ListClassCare.Count;

            rptClassTariff.DataSource = ListClassCare;
            rptClassTariff.DataBind();

            rptClassDiscount.DataSource = ListClassCare;
            rptClassDiscount.DataBind();
        }

        protected string GetTariffComponent1Text()
        {
            return lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.TARIFF_COMPONENT1_TEXT).ParameterValue;
        }

        protected string GetTariffComponent2Text()
        {
            return lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.TARIFF_COMPONENT2_TEXT).ParameterValue;
        }

        protected string GetTariffComponent3Text()
        {
            return lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.TARIFF_COMPONENT3_TEXT).ParameterValue;
        }

        protected void cbpClassTariff_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter == "refresh")
            {
                result = "refresh|1";
                try
                {
                    GetItemTariffPerClass(Convert.ToInt32(hdnBookID.Value), Convert.ToInt32(hdnDetailItemID.Value));
                }
                catch (Exception ex)
                {
                    result = string.Format("refresh|0|{0}", ex.Message);
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpClassDiscount_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter == "refresh")
            {
                result = "refresh|1";
                try
                {

                }
                catch (Exception ex)
                {
                    result = string.Format("refresh|0|{0}", ex.Message);
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void GetItemTariffPerClass(int bookID, int itemID)
        {
            List<ItemTariffListByBook> lst = BusinessLayer.GetItemTariffListByBook(AppSession.UserLogin.HealthcareID, bookID, itemID);

            StringBuilder sbListClassID = new StringBuilder();
            StringBuilder sbListClassCode = new StringBuilder();
            StringBuilder sbListClassName = new StringBuilder();
            StringBuilder sbListClassMargin = new StringBuilder();
            StringBuilder sbListTariff = new StringBuilder();
            StringBuilder sbListTariffComp1 = new StringBuilder();
            StringBuilder sbListTariffComp2 = new StringBuilder();
            StringBuilder sbListTariffComp3 = new StringBuilder();

            foreach (ItemTariffListByBook classCare in lst)
            {
                if (sbListClassID.ToString() != "")
                {
                    sbListClassID.Append("|");
                    sbListClassCode.Append("|");
                    sbListClassName.Append("|");
                    sbListTariff.Append("|");
                    sbListTariffComp1.Append("|");
                    sbListTariffComp2.Append("|");
                    sbListTariffComp3.Append("|");
                    sbListClassMargin.Append("|");
                }
                sbListClassID.Append(classCare.ClassID);
                sbListClassName.Append(classCare.ClassName);
                int marginPercentage = 0;
                if (hdnDetailGCItemType.Value == Constant.ItemType.PELAYANAN || hdnDetailGCItemType.Value == Constant.ItemType.RADIOLOGI || hdnDetailGCItemType.Value == Constant.ItemType.LABORATORIUM || hdnDetailGCItemType.Value == Constant.ItemType.PENUNJANG_MEDIS || hdnDetailGCItemType.Value == Constant.ItemType.MEDICAL_CHECKUP)
                    marginPercentage = (int)classCare.MarginPercentage1;
                else if (hdnDetailGCItemType.Value == Constant.ItemType.OBAT_OBATAN || hdnDetailGCItemType.Value == Constant.ItemType.BARANG_MEDIS || hdnDetailGCItemType.Value == Constant.ItemType.BARANG_UMUM)
                    marginPercentage = (int)classCare.MarginPercentage2;
                else if (String.IsNullOrEmpty(hdnDetailGCItemType.Value))
                    marginPercentage = 0;
                else
                    marginPercentage = (int)classCare.MarginPercentage3;
                sbListClassMargin.Append(marginPercentage);
                sbListTariff.Append(classCare.ApprovedTariff);
                sbListTariffComp1.Append(classCare.ApprovedTariffComp1);
                sbListTariffComp2.Append(classCare.ApprovedTariffComp2);
                sbListTariffComp3.Append(classCare.ApprovedTariffComp3);
            }
            string tmpClassID = sbListClassID.ToString();
            string tmpClassCode = sbListClassID.ToString();
            string tmpClassName = sbListClassName.ToString();
            string tmpClassTariff = sbListTariff.ToString();
            string tmpClassTariffComp1 = sbListTariffComp1.ToString();
            string tmpClassTariffComp2 = sbListTariffComp2.ToString();
            string tmpClassTariffComp3 = sbListTariffComp3.ToString();
            string tmpClassMargin = sbListClassMargin.ToString();

            string[] lstClassID = tmpClassID.Split('|');
            string[] lstClassCode = tmpClassCode.Split('|');
            string[] lstClassName = tmpClassName.Split('|');
            string[] lstClassTariff = tmpClassTariff.Split('|');
            string[] lstClassTariffComp1 = tmpClassTariffComp1.Split('|');
            string[] lstClassTariffComp2 = tmpClassTariffComp2.Split('|');
            string[] lstClassTariffComp3 = tmpClassTariffComp3.Split('|');
            string[] lstClassMargin = tmpClassMargin.Split('|');
            ListClassCare = new List<ClassCareInfo>();
            for (int i = 0; i < lstClassID.Length; ++i)
            {
                ClassCareInfo entity = new ClassCareInfo();
                entity.ClassID = Convert.ToInt32(lstClassID[i]);
                entity.ClassCode = lstClassCode[i];
                entity.ClassName = lstClassName[i];
                entity.Tariff = Convert.ToDecimal(lstClassTariff[i]);
                entity.TariffComponent1 = Convert.ToDecimal(lstClassTariffComp1[i]);
                entity.TariffComponent2 = Convert.ToDecimal(lstClassTariffComp2[i]);
                entity.TariffComponent3 = Convert.ToDecimal(lstClassTariffComp3[i]);
                entity.MarginPercentage1 = Convert.ToDecimal(lstClassMargin[i]);
                ListClassCare.Add(entity);
            }

            ClassCount = ListClassCare.Count;

            rptClassTariff.DataSource = ListClassCare;
            rptClassTariff.DataBind();
        }

        private void ControlToEntity(ItemServiceDt entity)
        {
            entity.ItemID = Convert.ToInt32(hdnPackageItemIDCtl.Value);

            if (hdnDepartmentID.Value != null && hdnDepartmentID.Value != "" && hdnDepartmentID.Value != "0")
            {
                entity.DepartmentID = hdnDepartmentID.Value;
            }
            else
            {
                entity.DepartmentID = null;
            }

            if (hdnServiceUnitID.Value != null && hdnServiceUnitID.Value != "" && hdnServiceUnitID.Value != "0")
            {
                entity.ServiceUnitID = Convert.ToInt32(hdnServiceUnitID.Value);
            }
            else
            {
                entity.ServiceUnitID = null;
            }

            entity.GCItemType = hdnDetailGCItemType.Value;
            entity.DetailItemID = Convert.ToInt32(hdnDetailItemID.Value);
            entity.Quantity = Convert.ToDecimal(txtDetailItemQuantity.Text);
            entity.IsControlAmount = chkIsControlAmount.Checked;
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ItemServiceDtDao entityDtDao = new ItemServiceDtDao(ctx);
            ItemServiceDtTariffDao entityTariffDao = new ItemServiceDtTariffDao(ctx);
            try
            {
                ItemServiceDt entity = new ItemServiceDt();
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entity.LastUpdatedDate = DateTime.Now;
                int dtID = entityDtDao.InsertReturnPrimaryKeyID(entity);

                StringBuilder sbListClassID = new StringBuilder();
                List<ItemTariffListByBook> lst = BusinessLayer.GetItemTariffListByBook(AppSession.UserLogin.HealthcareID, Convert.ToInt32(hdnBookID.Value), entity.DetailItemID);
                foreach (ItemTariffListByBook classCare in lst)
                {
                    if (sbListClassID.ToString() != "")
                    {
                        sbListClassID.Append("|");
                    }
                    sbListClassID.Append(classCare.ClassID);
                }
                string tmpClassID = sbListClassID.ToString();

                string[] lstClassID = tmpClassID.Split('|');
                ListClassCare = new List<ClassCareInfo>();
                for (int i = 0; i < lstClassID.Length; ++i)
                {
                    ClassCareInfo entityClassCare = new ClassCareInfo();
                    entityClassCare.ClassID = Convert.ToInt32(lstClassID[i]);
                    ListClassCare.Add(entityClassCare);
                }

                rptClassTariff.DataSource = ListClassCare;
                rptClassTariff.DataBind();

                rptClassDiscount.DataSource = ListClassCare;
                rptClassDiscount.DataBind();

                List<ItemServiceDtTariff> lstDiscount = new List<ItemServiceDtTariff>();
                foreach (RepeaterItem discount in rptClassDiscount.Items)
                {
                    TextBox txtClassDiscount = (TextBox)discount.FindControl("txtClassDiscount");
                    TextBox txtDiscountComponent1 = (TextBox)discount.FindControl("txtDiscountComponent1");
                    TextBox txtDiscountComponent2 = (TextBox)discount.FindControl("txtDiscountComponent2");
                    TextBox txtDiscountComponent3 = (TextBox)discount.FindControl("txtDiscountComponent3");
                    HtmlInputHidden hdnClassID = (HtmlInputHidden)discount.FindControl("hdnClassIDDiscount");

                    int classID = Convert.ToInt32(hdnClassID.Value);
                    ItemServiceDtTariff entityTariff = new ItemServiceDtTariff();
                    entityTariff.ClassID = classID;
                    entityTariff.DiscountAmount = Convert.ToDecimal(Request.Form[txtClassDiscount.UniqueID]);
                    entityTariff.DiscountComp1 = Convert.ToDecimal(Request.Form[txtDiscountComponent1.UniqueID]);
                    entityTariff.DiscountComp2 = Convert.ToDecimal(Request.Form[txtDiscountComponent2.UniqueID]);
                    entityTariff.DiscountComp3 = Convert.ToDecimal(Request.Form[txtDiscountComponent3.UniqueID]);
                    lstDiscount.Add(entityTariff);
                }


                foreach (RepeaterItem item in rptClassTariff.Items)
                {
                    TextBox txtClassTariff = (TextBox)item.FindControl("txtClassTariff");
                    TextBox txtTariffComponent1 = (TextBox)item.FindControl("txtTariffComponent1");
                    TextBox txtTariffComponent2 = (TextBox)item.FindControl("txtTariffComponent2");
                    TextBox txtTariffComponent3 = (TextBox)item.FindControl("txtTariffComponent3");
                    HtmlInputHidden hdnClassID = (HtmlInputHidden)item.FindControl("hdnClassID");

                    int classID = Convert.ToInt32(hdnClassID.Value);

                    ItemServiceDtTariff entityTariff = new ItemServiceDtTariff();
                    entityTariff.ItemServiceDtID = dtID;
                    entityTariff.GCTariffScheme = hdnGCTariffScheme.Value;
                    entityTariff.BookID = Convert.ToInt32(hdnBookID.Value);
                    entityTariff.ClassID = classID;
                    entityTariff.Tariff = Convert.ToDecimal(Request.Form[txtClassTariff.UniqueID]);
                    entityTariff.TariffComp1 = Convert.ToDecimal(Request.Form[txtTariffComponent1.UniqueID]);
                    entityTariff.TariffComp2 = Convert.ToDecimal(Request.Form[txtTariffComponent2.UniqueID]);
                    entityTariff.TariffComp3 = Convert.ToDecimal(Request.Form[txtTariffComponent3.UniqueID]);

                    ItemServiceDtTariff discount = lstDiscount.Where(t => t.ClassID == classID).FirstOrDefault();
                    entityTariff.DiscountAmount = discount.DiscountAmount;
                    entityTariff.DiscountComp1 = discount.DiscountComp1;
                    entityTariff.DiscountComp2 = discount.DiscountComp2;
                    entityTariff.DiscountComp3 = discount.DiscountComp3;

                    entityTariff.CreatedBy = AppSession.UserLogin.UserID;
                    entityTariffDao.Insert(entityTariff);
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

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ItemServiceDtDao entityDtDao = new ItemServiceDtDao(ctx);
            ItemServiceDtTariffDao entityTariffDao = new ItemServiceDtTariffDao(ctx);
            try
            {
                ItemServiceDt entity = entityDtDao.Get(Convert.ToInt32(hdnItemServiceDtIDCtl.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDtDao.Update(entity);

                string filterTariff = string.Format("ItemServiceDtID = '{0}' AND IsDeleted = 0", entity.ID);
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                List<ItemServiceDtTariff> lstTariffMaster = BusinessLayer.GetItemServiceDtTariffList(filterTariff, ctx);
                List<ItemServiceDtTariff> lstTariff = lstTariffMaster.Where(t => t.BookID == Convert.ToInt32(hdnBookID.Value)).ToList();

                if (lstTariffMaster.FirstOrDefault().BookID != Convert.ToInt32(hdnBookID.Value))
                {
                    foreach (ItemServiceDtTariff e in lstTariffMaster)
                    {
                        e.IsDeleted = true;
                        e.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityTariffDao.Update(e);
                    }
                }

                StringBuilder sbListClassID = new StringBuilder();
                List<ItemTariffListByBook> lst = BusinessLayer.GetItemTariffListByBook(AppSession.UserLogin.HealthcareID, Convert.ToInt32(hdnBookID.Value), entity.DetailItemID);
                foreach (ItemTariffListByBook classCare in lst)
                {
                    if (sbListClassID.ToString() != "")
                    {
                        sbListClassID.Append("|");
                    }
                    sbListClassID.Append(classCare.ClassID);
                }
                string tmpClassID = sbListClassID.ToString();

                string[] lstClassID = tmpClassID.Split('|');
                ListClassCare = new List<ClassCareInfo>();
                for (int i = 0; i < lstClassID.Length; ++i)
                {
                    ClassCareInfo entityClassCare = new ClassCareInfo();
                    entityClassCare.ClassID = Convert.ToInt32(lstClassID[i]);
                    ListClassCare.Add(entityClassCare);
                }

                rptClassTariff.DataSource = ListClassCare;
                rptClassTariff.DataBind();

                rptClassDiscount.DataSource = ListClassCare;
                rptClassDiscount.DataBind();

                List<ItemServiceDtTariff> lstDiscount = new List<ItemServiceDtTariff>();
                foreach (RepeaterItem discount in rptClassDiscount.Items)
                {
                    TextBox txtClassDiscount = (TextBox)discount.FindControl("txtClassDiscount");
                    TextBox txtDiscountComponent1 = (TextBox)discount.FindControl("txtDiscountComponent1");
                    TextBox txtDiscountComponent2 = (TextBox)discount.FindControl("txtDiscountComponent2");
                    TextBox txtDiscountComponent3 = (TextBox)discount.FindControl("txtDiscountComponent3");
                    HtmlInputHidden hdnClassID = (HtmlInputHidden)discount.FindControl("hdnClassIDDiscount");

                    int classID = Convert.ToInt32(hdnClassID.Value);
                    ItemServiceDtTariff entityTariff = new ItemServiceDtTariff();
                    entityTariff.ClassID = classID;
                    entityTariff.DiscountAmount = Convert.ToDecimal(Request.Form[txtClassDiscount.UniqueID]);
                    entityTariff.DiscountComp1 = Convert.ToDecimal(Request.Form[txtDiscountComponent1.UniqueID]);
                    entityTariff.DiscountComp2 = Convert.ToDecimal(Request.Form[txtDiscountComponent2.UniqueID]);
                    entityTariff.DiscountComp3 = Convert.ToDecimal(Request.Form[txtDiscountComponent3.UniqueID]);
                    lstDiscount.Add(entityTariff);
                }

                foreach (RepeaterItem item in rptClassTariff.Items)
                {
                    TextBox txtClassTariff = (TextBox)item.FindControl("txtClassTariff");
                    TextBox txtTariffComponent1 = (TextBox)item.FindControl("txtTariffComponent1");
                    TextBox txtTariffComponent2 = (TextBox)item.FindControl("txtTariffComponent2");
                    TextBox txtTariffComponent3 = (TextBox)item.FindControl("txtTariffComponent3");
                    HtmlInputHidden hdnClassID = (HtmlInputHidden)item.FindControl("hdnClassID");

                    int classID = Convert.ToInt32(hdnClassID.Value);

                    ItemServiceDtTariff entityTariff = lstTariff.Where(t => t.ClassID == classID).FirstOrDefault();
                    if (entityTariff != null)
                    {
                        entityTariff.Tariff = Convert.ToDecimal(Request.Form[txtClassTariff.UniqueID]);
                        entityTariff.TariffComp1 = Convert.ToDecimal(Request.Form[txtTariffComponent1.UniqueID]);
                        entityTariff.TariffComp2 = Convert.ToDecimal(Request.Form[txtTariffComponent2.UniqueID]);
                        entityTariff.TariffComp3 = Convert.ToDecimal(Request.Form[txtTariffComponent3.UniqueID]);

                        ItemServiceDtTariff discount = lstDiscount.Where(t => t.ClassID == classID).FirstOrDefault();
                        entityTariff.DiscountAmount = discount.DiscountAmount;
                        entityTariff.DiscountComp1 = discount.DiscountComp1;
                        entityTariff.DiscountComp2 = discount.DiscountComp2;
                        entityTariff.DiscountComp3 = discount.DiscountComp3;

                        entityTariff.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityTariffDao.Update(entityTariff);
                    }
                    else
                    {
                        entityTariff = new ItemServiceDtTariff();
                        entityTariff.ItemServiceDtID = entity.ID;
                        entityTariff.GCTariffScheme = hdnGCTariffScheme.Value;
                        entityTariff.BookID = Convert.ToInt32(hdnBookID.Value);
                        entityTariff.ClassID = classID;
                        entityTariff.Tariff = Convert.ToDecimal(Request.Form[txtClassTariff.UniqueID]);
                        entityTariff.TariffComp1 = Convert.ToDecimal(Request.Form[txtTariffComponent1.UniqueID]);
                        entityTariff.TariffComp2 = Convert.ToDecimal(Request.Form[txtTariffComponent2.UniqueID]);
                        entityTariff.TariffComp3 = Convert.ToDecimal(Request.Form[txtTariffComponent3.UniqueID]);

                        ItemServiceDtTariff discount = lstDiscount.Where(t => t.ClassID == classID).FirstOrDefault();
                        entityTariff.DiscountAmount = discount.DiscountAmount;
                        entityTariff.DiscountComp1 = discount.DiscountComp1;
                        entityTariff.DiscountComp2 = discount.DiscountComp2;
                        entityTariff.DiscountComp3 = discount.DiscountComp3;

                        entityTariff.CreatedBy = AppSession.UserLogin.UserID;
                        entityTariffDao.Insert(entityTariff);
                    }
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