using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;
using System.Text;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class EditItemTariff : BasePageTrx
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
        }

        protected int PageCount = 1;
        private List<ClassCareInfo> ListClassCare = null;
        List<SettingParameter> lstSettingParameter = null;
        protected int ClassCount = 0;

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
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

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.FN_UTILITY_EDIT_TARIFF;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            BindingRptClass();
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, true, ""));
            SetControlEntrySetting(txtItemCode, new ControlEntrySetting(true, true, true, ""));
        }

        protected void cbpProcessDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindingRptClass();
                result = e.Parameter;
                //result = "refresh";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindingRptClass()
        {
            lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}','{2}')",
                Constant.SettingParameter.TARIFF_COMPONENT1_TEXT, Constant.SettingParameter.TARIFF_COMPONENT2_TEXT, Constant.SettingParameter.TARIFF_COMPONENT3_TEXT));

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
                if (hdnGCItemType.Value == Constant.ItemGroupMaster.SERVICE || hdnGCItemType.Value == Constant.ItemGroupMaster.RADIOLOGY || hdnGCItemType.Value == Constant.ItemGroupMaster.LABORATORY || hdnGCItemType.Value == Constant.ItemGroupMaster.DIAGNOSTIC)
                    marginPercentage = (int)classCare.MarginPercentage1;
                else if (hdnGCItemType.Value == Constant.ItemGroupMaster.DRUGS || hdnGCItemType.Value == Constant.ItemGroupMaster.SUPPLIES)
                    marginPercentage = (int)classCare.MarginPercentage2;
                else if (String.IsNullOrEmpty(hdnGCItemType.Value))
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
                entity.ClassCode = lstClassCode[i];
                entity.ClassName = lstClassName[i];
                entity.MarginPercentage1 = Convert.ToDecimal(lstClassMargin[i]);
                ListClassCare.Add(entity);
            }

            ClassCount = ListClassCare.Count;

            rptClassCare.DataSource = ListClassCare;
            rptClassCare.DataBind();
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            retval = "";
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ItemTariffDao entityItemTariffDao = new ItemTariffDao(ctx);
            ItemTariffCostDao entityItemTariffCostDao = new ItemTariffCostDao(ctx);
            ItemTariffHistoryDao entityItemTariffHistoryDao = new ItemTariffHistoryDao(ctx);
            ItemTariffCostHistoryDao entityItemTariffCostHistoryDao = new ItemTariffCostHistoryDao(ctx);
            TariffBookHdDao entityTariffBookDHdDao = new TariffBookHdDao(ctx);
            TariffBookDtDao entityTariffBookDtDao = new TariffBookDtDao(ctx);
            TariffBookDtCostDao entityTariffBookDtCostDao = new TariffBookDtCostDao(ctx);

            try
            {
                if (type == "process")
                {
                    int itemID = Convert.ToInt32(hdnItemID.Value);
                    int bookID = Convert.ToInt32(hdnBookID.Value);
                    string gcTariffScheme = hdnGCTariffScheme.Value;
                    if (bookID != 0 && itemID != 0)
                    {
                        TariffBookHd bookHd = entityTariffBookDHdDao.Get(Convert.ToInt32(hdnBookID.Value));

                        decimal baseTariff = Convert.ToDecimal(txtBaseTariff.Text);
                        decimal suggestedTariff = Convert.ToDecimal(Request.Form[txtSuggestedTariff.UniqueID]);
                        string filterExpressionTariff = string.Format("BookID = {0} AND GCTariffScheme = '{1}' AND ItemID = {2} AND HealthcareID = '{3}' AND StartingDate <= '{4}' ORDER BY StartingDate DESC", bookID, hdnGCTariffScheme.Value, hdnItemID.Value, AppSession.UserLogin.HealthcareID, bookHd.StartingDate.ToString(Constant.FormatString.DATE_FORMAT_112));
                        string filterExpressionTariffCost = string.Format("ItemID = {0}", hdnItemID.Value);

                        List<ItemTariff> lstItemTariff = BusinessLayer.GetItemTariffList(filterExpressionTariff, ctx);
                        List<ItemTariffCost> lstItemTariffCost = BusinessLayer.GetItemTariffCostList(filterExpressionTariffCost, ctx);
                        List<TariffBookDt> lstTariffBookDt = BusinessLayer.GetTariffBookDtList(string.Format("BookID = {0} AND ItemID = {1}", bookID, itemID), ctx);
                        List<TariffBookDtCost> lstTariffBookDtCost = BusinessLayer.GetTariffBookDtCostList(string.Format("BookID = {0} AND ItemID = {1}", bookID, itemID), ctx);
                        
                        StringBuilder sbListClassID = new StringBuilder();
                        List<ItemTariffListByBook> lst = BusinessLayer.GetItemTariffListByBook(AppSession.UserLogin.HealthcareID, bookID, itemID);
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
                            ClassCareInfo entity = new ClassCareInfo();
                            entity.ClassID = Convert.ToInt32(lstClassID[i]);
                            ListClassCare.Add(entity);
                        }
                        rptClassCare.DataSource = ListClassCare;
                        rptClassCare.DataBind();

                        foreach (RepeaterItem item in rptClassCare.Items)
                        {
                            TextBox txtClass = (TextBox)item.FindControl("txtClass");
                            TextBox txtComponent1 = (TextBox)item.FindControl("txtComponent1");
                            TextBox txtComponent2 = (TextBox)item.FindControl("txtComponent2");
                            TextBox txtComponent3 = (TextBox)item.FindControl("txtComponent3");
                            HtmlInputHidden hdnClassID = (HtmlInputHidden)item.FindControl("hdnClassID");
                            HtmlInputHidden hdnMarginPercentage = (HtmlInputHidden)item.FindControl("hdnMarginPercentage");
                            HtmlInputHidden hdnPrevBurden = (HtmlInputHidden)item.FindControl("hdnPrevBurden");
                            HtmlInputHidden hdnPrevLabor = (HtmlInputHidden)item.FindControl("hdnPrevLabor");
                            HtmlInputHidden hdnPrevMaterial = (HtmlInputHidden)item.FindControl("hdnPrevMaterial");
                            HtmlInputHidden hdnPrevOverhead = (HtmlInputHidden)item.FindControl("hdnPrevOverhead");
                            HtmlInputHidden hdnPrevSubContract = (HtmlInputHidden)item.FindControl("hdnPrevSubContract");
                            HtmlInputHidden hdnCurrentBurden = (HtmlInputHidden)item.FindControl("hdnCurrentBurden");
                            HtmlInputHidden hdnCurrentLabor = (HtmlInputHidden)item.FindControl("hdnCurrentLabor");
                            HtmlInputHidden hdnCurrentMaterial = (HtmlInputHidden)item.FindControl("hdnCurrentMaterial");
                            HtmlInputHidden hdnCurrentOverhead = (HtmlInputHidden)item.FindControl("hdnCurrentOverhead");
                            HtmlInputHidden hdnCurrentSubContract = (HtmlInputHidden)item.FindControl("hdnCurrentSubContract");
                            HtmlInputHidden hdnTotalBurden = (HtmlInputHidden)item.FindControl("hdnTotalBurden");
                            HtmlInputHidden hdnTotalLabor = (HtmlInputHidden)item.FindControl("hdnTotalLabor");
                            HtmlInputHidden hdnTotalMaterial = (HtmlInputHidden)item.FindControl("hdnTotalMaterial");
                            HtmlInputHidden hdnTotalOverhead = (HtmlInputHidden)item.FindControl("hdnTotalOverhead");
                            HtmlInputHidden hdnTotalSubContract = (HtmlInputHidden)item.FindControl("hdnTotalSubContract");

                            int classID = Convert.ToInt32(hdnClassID.Value);
                            //TariffBookDt entity = lstTariffBookDt.FirstOrDefault(p => p.ClassID == classID);
                            ItemTariff entityItemTariff = lstItemTariff.FirstOrDefault(p => p.ClassID == classID);
                            TariffBookDt entityTariffBookDt = lstTariffBookDt.FirstOrDefault(p => p.ClassID == classID);
                            ItemTariffHistory entityItemTariffHistory = new ItemTariffHistory();

                            entityItemTariffHistory.LogDate = DateTime.Now;
                            entityItemTariffHistory.HealthcareID = AppSession.UserLogin.HealthcareID;
                            entityItemTariffHistory.BookID = bookID;
                            entityItemTariffHistory.ItemID = itemID;
                            entityItemTariffHistory.ClassID = classID;
                            entityItemTariffHistory.Remarks = txtRemarks.Text;

                            if (entityItemTariff != null)
                            {
                                bool isAdd = entityTariffBookDt == null;
                                if (isAdd)
                                    entityTariffBookDt = new TariffBookDt();

                                entityTariffBookDt.BookID = bookID;
                                entityTariffBookDt.ItemID = itemID;
                                entityTariffBookDt.ClassID = Convert.ToInt32(hdnClassID.Value);
                                entityTariffBookDt.MarginPercentage = Convert.ToDecimal(hdnMarginPercentage.Value);
                                entityTariffBookDt.SuggestedTariff = suggestedTariff;
                                entityTariffBookDt.BaseTariff = baseTariff;
                                entityTariffBookDt.ApprovedBaseTariff = baseTariff;
                                entityTariffBookDt.ProposedTariff = Convert.ToDecimal(Request.Form[txtClass.UniqueID]);
                                entityTariffBookDt.ProposedTariffComp1 = Convert.ToDecimal(Request.Form[txtComponent1.UniqueID]);
                                entityTariffBookDt.ProposedTariffComp2 = Convert.ToDecimal(Request.Form[txtComponent2.UniqueID]);
                                entityTariffBookDt.ProposedTariffComp3 = Convert.ToDecimal(Request.Form[txtComponent3.UniqueID]);
                                entityTariffBookDt.ApprovedTariff = entityTariffBookDt.ProposedTariff;
                                entityTariffBookDt.ApprovedTariffComp1 = entityTariffBookDt.ProposedTariffComp1;
                                entityTariffBookDt.ApprovedTariffComp2 = entityTariffBookDt.ProposedTariffComp2;
                                entityTariffBookDt.ApprovedTariffComp3 = entityTariffBookDt.ProposedTariffComp3;
                                entityTariffBookDt.IsApproved = true;
                                if (isAdd)
                                {
                                    entityTariffBookDt.CreatedBy = AppSession.UserLogin.UserID;
                                    entityTariffBookDtDao.Insert(entityTariffBookDt);
                                }
                                else
                                {
                                    entityTariffBookDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entityTariffBookDtDao.Update(entityTariffBookDt);
                                }

                                entityItemTariffHistory.OldTariff = entityItemTariff.Tariff;
                                entityItemTariffHistory.OldTariffComp1 = entityItemTariff.TariffComp1;
                                entityItemTariffHistory.OldTariffComp2 = entityItemTariff.TariffComp2;
                                entityItemTariffHistory.OldTariffComp3 = entityItemTariff.TariffComp3;

                                entityItemTariff.Tariff = Convert.ToDecimal(Request.Form[txtClass.UniqueID]);
                                entityItemTariff.TariffComp1 = Convert.ToDecimal(Request.Form[txtComponent1.UniqueID]);
                                entityItemTariff.TariffComp2 = Convert.ToDecimal(Request.Form[txtComponent2.UniqueID]);
                                entityItemTariff.TariffComp3 = Convert.ToDecimal(Request.Form[txtComponent3.UniqueID]);

                                entityItemTariffHistory.NewTariff = entityItemTariff.Tariff;
                                entityItemTariffHistory.NewTariffComp1 = entityItemTariff.TariffComp1;
                                entityItemTariffHistory.NewTariffComp2 = entityItemTariff.TariffComp2;
                                entityItemTariffHistory.NewTariffComp3 = entityItemTariff.TariffComp3;

                                entityItemTariffDao.Update(entityItemTariff);

                                retval = "Perubahan tarif item sudah berhasil dilakukan";
                            }
                            else
                            {
                                entityTariffBookDt = new TariffBookDt();
                                entityTariffBookDt.BookID = bookID;
                                entityTariffBookDt.ItemID = itemID;
                                entityTariffBookDt.ClassID = Convert.ToInt32(hdnClassID.Value);
                                entityTariffBookDt.MarginPercentage = Convert.ToDecimal(hdnMarginPercentage.Value);
                                entityTariffBookDt.SuggestedTariff = suggestedTariff;
                                entityTariffBookDt.BaseTariff = baseTariff;
                                entityTariffBookDt.ProposedTariff = Convert.ToDecimal(Request.Form[txtClass.UniqueID]);
                                entityTariffBookDt.ProposedTariffComp1 = Convert.ToDecimal(Request.Form[txtComponent1.UniqueID]);
                                entityTariffBookDt.ProposedTariffComp2 = Convert.ToDecimal(Request.Form[txtComponent2.UniqueID]);
                                entityTariffBookDt.ProposedTariffComp3 = Convert.ToDecimal(Request.Form[txtComponent3.UniqueID]);
                                entityTariffBookDt.ApprovedTariff = entityTariffBookDt.ProposedTariff;
                                entityTariffBookDt.ApprovedTariffComp1 = entityTariffBookDt.ProposedTariffComp1;
                                entityTariffBookDt.ApprovedTariffComp2 = entityTariffBookDt.ProposedTariffComp2;
                                entityTariffBookDt.ApprovedTariffComp3 = entityTariffBookDt.ProposedTariffComp3;
                                entityTariffBookDt.IsApproved = true;
                                entityTariffBookDt.CreatedBy = AppSession.UserLogin.UserID;
                                entityTariffBookDtDao.Insert(entityTariffBookDt);

                                entityItemTariffHistory.OldTariff = 0;
                                entityItemTariffHistory.OldTariffComp1 = 0;
                                entityItemTariffHistory.OldTariffComp2 = 0;
                                entityItemTariffHistory.OldTariffComp3 = 0;

                                entityItemTariff = new ItemTariff();
                                entityItemTariff.ItemID = itemID;
                                entityItemTariff.GCItemType = BusinessLayer.GetItemMaster(itemID).GCItemType;
                                entityItemTariff.StartingDate = Helper.GetDatePickerValue(hdnStartDate.Value);
                                entityItemTariff.HealthcareID = AppSession.UserLogin.HealthcareID;
                                entityItemTariff.GCTariffScheme = gcTariffScheme;
                                entityItemTariff.BookID = bookID;
                                entityItemTariff.ClassID = Convert.ToInt32(hdnClassID.Value);
                                entityItemTariff.Tariff = Convert.ToDecimal(Request.Form[txtClass.UniqueID]);
                                entityItemTariff.TariffComp1 = Convert.ToDecimal(Request.Form[txtComponent1.UniqueID]);
                                entityItemTariff.TariffComp2 = Convert.ToDecimal(Request.Form[txtComponent2.UniqueID]);
                                entityItemTariff.TariffComp3 = Convert.ToDecimal(Request.Form[txtComponent3.UniqueID]);
                                entityItemTariffHistory.NewTariff = entityItemTariff.Tariff;
                                entityItemTariffHistory.NewTariffComp1 = entityItemTariff.TariffComp1;
                                entityItemTariffHistory.NewTariffComp2 = entityItemTariff.TariffComp2;
                                entityItemTariffHistory.NewTariffComp3 = entityItemTariff.TariffComp3;
                                entityItemTariff.CreatedBy = AppSession.UserLogin.UserID;
                                entityItemTariff.ID = entityItemTariffDao.InsertReturnPrimaryKeyID(entityItemTariff);
                                retval = "Penambahan Tariff Item baru sudah berhasil dilakukan";
                            }

                            ItemTariffCost entityItemTariffCost = lstItemTariffCost.FirstOrDefault(p => p.ClassID == classID && p.ID == entityItemTariff.ID);
                            TariffBookDtCost entityCost = lstTariffBookDtCost.FirstOrDefault(p => p.ClassID == classID);

                            ItemTariffCostHistory entityItemTariffCostHistory = new ItemTariffCostHistory();
                            entityItemTariffCostHistory.LogDate = DateTime.Now;
                            entityItemTariffCostHistory.HealthcareID = AppSession.UserLogin.HealthcareID;
                            entityItemTariffCostHistory.BookID = bookID;
                            entityItemTariffCostHistory.ItemID = itemID;
                            entityItemTariffCostHistory.ClassID = classID;
                            if (entityCost != null)
                            {
                                if (!string.IsNullOrEmpty(Request.Form[hdnPrevBurden.UniqueID]))
                                {
                                    entityCost.PreviousBurden = Convert.ToDecimal(Request.Form[hdnPrevBurden.UniqueID]);
                                }
                                else
                                {
                                    entityCost.PreviousBurden = 0;
                                }

                                if (!string.IsNullOrEmpty(Request.Form[hdnPrevLabor.UniqueID]))
                                {
                                    entityCost.PreviousLabor = Convert.ToDecimal(Request.Form[hdnPrevLabor.UniqueID]);
                                }
                                else
                                {
                                    entityCost.PreviousLabor = 0;
                                }

                                if (!string.IsNullOrEmpty(Request.Form[hdnPrevMaterial.UniqueID]))
                                {
                                    entityCost.PreviousMaterial = Convert.ToDecimal(Request.Form[hdnPrevMaterial.UniqueID]);
                                }
                                else
                                {
                                    entityCost.PreviousMaterial = 0;
                                }

                                if (!string.IsNullOrEmpty(Request.Form[hdnPrevOverhead.UniqueID]))
                                {
                                    entityCost.PreviousOverhead = Convert.ToDecimal(Request.Form[hdnPrevOverhead.UniqueID]);
                                }
                                else
                                {
                                    entityCost.PreviousOverhead = 0;
                                }

                                if (!string.IsNullOrEmpty(Request.Form[hdnPrevSubContract.UniqueID]))
                                {
                                    entityCost.PreviousSubContract = Convert.ToDecimal(Request.Form[hdnPrevSubContract.UniqueID]);
                                }
                                else
                                {
                                    entityCost.PreviousSubContract = 0;
                                }
                                if (!string.IsNullOrEmpty(Request.Form[hdnCurrentBurden.UniqueID]))
                                {
                                    entityCost.CurrentBurden = Convert.ToDecimal(Request.Form[hdnCurrentBurden.UniqueID]);
                                }
                                else
                                {
                                    entityCost.CurrentBurden = 0;
                                }

                                if (!string.IsNullOrEmpty(Request.Form[hdnCurrentLabor.UniqueID]))
                                {
                                    entityCost.CurrentLabor = Convert.ToDecimal(Request.Form[hdnCurrentLabor.UniqueID]);
                                }
                                else
                                {
                                    entityCost.CurrentLabor = 0;
                                }
                                if (!string.IsNullOrEmpty(Request.Form[hdnCurrentMaterial.UniqueID]))
                                {
                                    entityCost.CurrentMaterial = Convert.ToDecimal(Request.Form[hdnCurrentMaterial.UniqueID]);
                                }
                                else
                                {
                                    entityCost.CurrentMaterial = 0;
                                }
                                if (!string.IsNullOrEmpty(Request.Form[hdnCurrentOverhead.UniqueID]))
                                {
                                    entityCost.CurrentOverhead = Convert.ToDecimal(Request.Form[hdnCurrentOverhead.UniqueID]);
                                }
                                else
                                {
                                    entityCost.CurrentOverhead = 0;
                                }

                                if (!string.IsNullOrEmpty(Request.Form[hdnCurrentSubContract.UniqueID]))
                                {
                                    entityCost.CurrentSubContract = Convert.ToDecimal(Request.Form[hdnCurrentSubContract.UniqueID]);
                                }
                                else
                                {
                                    entityCost.CurrentSubContract = 0;
                                }
                                entityCost.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityTariffBookDtCostDao.Update(entityCost);
                            }
                            else
                            {
                                entityCost = new TariffBookDtCost();
                                entityCost.BookID = bookID;
                                entityCost.ItemID = itemID;
                                entityCost.ClassID = Convert.ToInt32(hdnClassID.Value);
                                if (!string.IsNullOrEmpty(Request.Form[hdnPrevBurden.UniqueID]))
                                {
                                    entityCost.PreviousBurden = Convert.ToDecimal(Request.Form[hdnPrevBurden.UniqueID]);
                                }
                                else
                                {
                                    entityCost.PreviousBurden = 0;
                                }

                                if (!string.IsNullOrEmpty(Request.Form[hdnPrevLabor.UniqueID]))
                                {
                                    entityCost.PreviousLabor = Convert.ToDecimal(Request.Form[hdnPrevLabor.UniqueID]);
                                }
                                else
                                {
                                    entityCost.PreviousLabor = 0;
                                }

                                if (!string.IsNullOrEmpty(Request.Form[hdnPrevMaterial.UniqueID]))
                                {
                                    entityCost.PreviousMaterial = Convert.ToDecimal(Request.Form[hdnPrevMaterial.UniqueID]);
                                }
                                else
                                {
                                    entityCost.PreviousMaterial = 0;
                                }

                                if (!string.IsNullOrEmpty(Request.Form[hdnPrevOverhead.UniqueID]))
                                {
                                    entityCost.PreviousOverhead = Convert.ToDecimal(Request.Form[hdnPrevOverhead.UniqueID]);
                                }
                                else
                                {
                                    entityCost.PreviousOverhead = 0;
                                }

                                if (!string.IsNullOrEmpty(Request.Form[hdnPrevSubContract.UniqueID]))
                                {
                                    entityCost.PreviousSubContract = Convert.ToDecimal(Request.Form[hdnPrevSubContract.UniqueID]);
                                }
                                else
                                {
                                    entityCost.PreviousSubContract = 0;
                                }
                                if (!string.IsNullOrEmpty(Request.Form[hdnCurrentBurden.UniqueID]))
                                {
                                    entityCost.CurrentBurden = Convert.ToDecimal(Request.Form[hdnCurrentBurden.UniqueID]);
                                }
                                else
                                {
                                    entityCost.CurrentBurden = 0;
                                }

                                if (!string.IsNullOrEmpty(Request.Form[hdnCurrentLabor.UniqueID]))
                                {
                                    entityCost.CurrentLabor = Convert.ToDecimal(Request.Form[hdnCurrentLabor.UniqueID]);
                                }
                                else
                                {
                                    entityCost.CurrentLabor = 0;
                                }
                                if (!string.IsNullOrEmpty(Request.Form[hdnCurrentMaterial.UniqueID]))
                                {
                                    entityCost.CurrentMaterial = Convert.ToDecimal(Request.Form[hdnCurrentMaterial.UniqueID]);
                                }
                                else
                                {
                                    entityCost.CurrentMaterial = 0;
                                }
                                if (!string.IsNullOrEmpty(Request.Form[hdnCurrentOverhead.UniqueID]))
                                {
                                    entityCost.CurrentOverhead = Convert.ToDecimal(Request.Form[hdnCurrentOverhead.UniqueID]);
                                }
                                else
                                {
                                    entityCost.CurrentOverhead = 0;
                                }

                                if (!string.IsNullOrEmpty(Request.Form[hdnCurrentSubContract.UniqueID]))
                                {
                                    entityCost.CurrentSubContract = Convert.ToDecimal(Request.Form[hdnCurrentSubContract.UniqueID]);
                                }
                                else
                                {
                                    entityCost.CurrentSubContract = 0;
                                } 
                                entityCost.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityTariffBookDtCostDao.Insert(entityCost);
                            }

                            if (entityItemTariffCost != null)
                            {
                                entityItemTariffCostHistory.OldPreviousMaterial = entityItemTariffCost.PreviousMaterial;
                                entityItemTariffCostHistory.OldCurrentMaterial = entityItemTariffCost.CurrentMaterial;
                                entityItemTariffCostHistory.OldPreviousLabor = entityItemTariffCost.PreviousLabor;
                                entityItemTariffCostHistory.OldCurrentLabor = entityItemTariffCost.CurrentLabor;
                                entityItemTariffCostHistory.OldPreviousOverhead = entityItemTariffCost.PreviousOverhead;
                                entityItemTariffCostHistory.OldCurrentOverhead = entityItemTariffCost.CurrentOverhead;
                                entityItemTariffCostHistory.OldPreviousSubContract = entityItemTariffCost.PreviousSubContract;
                                entityItemTariffCostHistory.OldCurrentSubContract = entityItemTariffCost.CurrentSubContract;
                                entityItemTariffCostHistory.OldPreviousBurden = entityItemTariffCost.PreviousBurden;
                                entityItemTariffCostHistory.OldCurrentBurden = entityItemTariffCost.CurrentBurden;
                                entityItemTariffCost.ItemID = entityItemTariff.ItemID;
                                entityItemTariffCost.ClassID = entityItemTariff.ClassID;
                                entityItemTariffCost.CurrentBurden = entityCost.CurrentBurden;
                                entityItemTariffCost.CurrentLabor = entityCost.CurrentLabor;
                                entityItemTariffCost.CurrentMaterial = entityCost.CurrentMaterial;
                                entityItemTariffCost.CurrentOverhead = entityCost.CurrentOverhead;
                                entityItemTariffCost.CurrentSubContract = entityCost.CurrentSubContract;
                                entityItemTariffCost.PreviousBurden = entityCost.PreviousBurden;
                                entityItemTariffCost.PreviousLabor = entityCost.PreviousLabor;
                                entityItemTariffCost.PreviousMaterial = entityCost.PreviousMaterial;
                                entityItemTariffCost.PreviousOverhead = entityCost.PreviousOverhead;
                                entityItemTariffCost.PreviousSubContract = entityCost.PreviousSubContract;
                                entityItemTariffCostHistory.NewPreviousMaterial = entityItemTariffCost.PreviousMaterial;
                                entityItemTariffCostHistory.NewCurrentMaterial = entityItemTariffCost.CurrentMaterial;
                                entityItemTariffCostHistory.NewPreviousLabor = entityItemTariffCost.PreviousLabor;
                                entityItemTariffCostHistory.NewCurrentLabor = entityItemTariffCost.CurrentLabor;
                                entityItemTariffCostHistory.NewPreviousOverhead = entityItemTariffCost.PreviousOverhead;
                                entityItemTariffCostHistory.NewCurrentOverhead = entityItemTariffCost.CurrentOverhead;
                                entityItemTariffCostHistory.NewPreviousSubContract = entityItemTariffCost.PreviousSubContract;
                                entityItemTariffCostHistory.NewCurrentSubContract = entityItemTariffCost.CurrentSubContract;
                                entityItemTariffCostHistory.NewPreviousBurden = entityItemTariffCost.PreviousBurden;
                                entityItemTariffCostHistory.NewCurrentBurden = entityItemTariffCost.CurrentBurden;
                                entityItemTariffCost.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityItemTariffCostDao.Update(entityItemTariffCost);
                            }
                            else
                            {
                                entityItemTariffCostHistory.OldPreviousMaterial = 0;
                                entityItemTariffCostHistory.OldCurrentMaterial = 0;
                                entityItemTariffCostHistory.OldPreviousLabor = 0;
                                entityItemTariffCostHistory.OldCurrentLabor = 0;
                                entityItemTariffCostHistory.OldPreviousOverhead = 0;
                                entityItemTariffCostHistory.OldCurrentOverhead = 0;
                                entityItemTariffCostHistory.OldPreviousSubContract = 0;
                                entityItemTariffCostHistory.OldCurrentSubContract = 0;
                                entityItemTariffCostHistory.OldPreviousBurden = 0;
                                entityItemTariffCostHistory.OldCurrentBurden = 0;
                                entityItemTariffCost = new ItemTariffCost();
                                entityItemTariffCost.ID = entityItemTariff.ID;
                                entityItemTariffCost.ItemID = entityItemTariff.ItemID;
                                entityItemTariffCost.ClassID = entityItemTariff.ClassID;
                                entityItemTariffCost.CurrentBurden = entityCost.CurrentBurden;
                                entityItemTariffCost.CurrentLabor = entityCost.CurrentLabor;
                                entityItemTariffCost.CurrentMaterial = entityCost.CurrentMaterial;
                                entityItemTariffCost.CurrentOverhead = entityCost.CurrentOverhead;
                                entityItemTariffCost.CurrentSubContract = entityCost.CurrentSubContract;
                                entityItemTariffCost.PreviousBurden = entityCost.PreviousBurden;
                                entityItemTariffCost.PreviousLabor = entityCost.PreviousLabor;
                                entityItemTariffCost.PreviousMaterial = entityCost.PreviousMaterial;
                                entityItemTariffCost.PreviousOverhead = entityCost.PreviousOverhead;
                                entityItemTariffCost.PreviousSubContract = entityCost.PreviousSubContract;
                                entityItemTariffCostHistory.NewPreviousMaterial = entityItemTariffCost.PreviousMaterial;
                                entityItemTariffCostHistory.NewCurrentMaterial = entityItemTariffCost.CurrentMaterial;
                                entityItemTariffCostHistory.NewPreviousLabor = entityItemTariffCost.PreviousLabor;
                                entityItemTariffCostHistory.NewCurrentLabor = entityItemTariffCost.CurrentLabor;
                                entityItemTariffCostHistory.NewPreviousOverhead = entityItemTariffCost.PreviousOverhead;
                                entityItemTariffCostHistory.NewCurrentOverhead = entityItemTariffCost.CurrentOverhead;
                                entityItemTariffCostHistory.NewPreviousSubContract = entityItemTariffCost.PreviousSubContract;
                                entityItemTariffCostHistory.NewCurrentSubContract = entityItemTariffCost.CurrentSubContract;
                                entityItemTariffCostHistory.NewPreviousBurden = entityItemTariffCost.PreviousBurden;
                                entityItemTariffCostHistory.NewCurrentBurden = entityItemTariffCost.CurrentBurden;
                                entityItemTariffCost.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityItemTariffCostDao.Insert(entityItemTariffCost);
                            }
                            entityItemTariffHistory.UserID = AppSession.UserLogin.UserID;
                            entityItemTariffCostHistory.UserID = AppSession.UserLogin.UserID;
                            entityItemTariffHistoryDao.Insert(entityItemTariffHistory);
                            entityItemTariffCostHistoryDao.Insert(entityItemTariffCostHistory);
                        }
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        errMessage = "Invalid Item or Tariff Book ID";
                        result = false;
                        ctx.RollBackTransaction();
                    }
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

        protected override void SetControlProperties()
        {
            //List<StandardCode> lstSC = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.ITEM_TYPE, Constant.StandardCode.CUSTOMER_TYPE));
            //Methods.SetComboBoxField(cboCustomerType, lstSC.Where(p => p.ParentID == Constant.StandardCode.CUSTOMER_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            //cboCustomerType.SelectedIndex = 3;
            //Methods.SetComboBoxField(cboItemType, lstSC.Where(p => p.ParentID == Constant.StandardCode.ITEM_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            //cboItemType.SelectedIndex = 0;
            //List<ClassCare> lstClass = BusinessLayer.GetClassCareList("IsUsedInChargeClass = 1 AND IsDeleted = 0");
            //Methods.SetComboBoxField(cboClass, lstClass, "ClassName", "ClassID");
            //cboClass.Value = hdnOutPatientID.Value;
            //cboClass.ClientEnabled = false;
            //txtTransactionDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
        }

        protected void cbpClassTariff_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter == "refresh")
            {
                result = "refresh|1";
                try
                {
                    GetItemTariffPerClass(Convert.ToInt32(hdnBookID.Value), Convert.ToInt32(hdnItemID.Value));
                }
                catch (Exception ex)
                {
                    result = string.Format("refresh|0|{0}",ex.Message);
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
                if (hdnGCItemType.Value == Constant.ItemGroupMaster.SERVICE || hdnGCItemType.Value == Constant.ItemGroupMaster.RADIOLOGY || hdnGCItemType.Value == Constant.ItemGroupMaster.LABORATORY || hdnGCItemType.Value == Constant.ItemGroupMaster.DIAGNOSTIC)
                    marginPercentage = (int)classCare.MarginPercentage1;
                else if (hdnGCItemType.Value == Constant.ItemGroupMaster.DRUGS || hdnGCItemType.Value == Constant.ItemGroupMaster.SUPPLIES)
                    marginPercentage = (int)classCare.MarginPercentage2;
                else if (String.IsNullOrEmpty(hdnGCItemType.Value))
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

            rptClassCare.DataSource = ListClassCare;
            rptClassCare.DataBind();
        }

    }
}