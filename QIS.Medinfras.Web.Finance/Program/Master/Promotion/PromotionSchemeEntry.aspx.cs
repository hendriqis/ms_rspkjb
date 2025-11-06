using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;
using System.Data;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class PromotionSchemeEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.PROMOTION_SCHEME;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;
                PromotionScheme entity = BusinessLayer.GetPromotionScheme(Convert.ToInt32(ID));
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
                divCopyPromotion.Visible = true;
            }
            txtPromotionSchemeCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void OnControlEntrySetting()
        {
            List<StandardCode> lstCodes = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}') AND IsActive = 1",
    Constant.StandardCode.PROMOTION_TYPE));
            Methods.SetComboBoxField(cboPromotionType, lstCodes, "StandardCodeName", "StandardCodeID");

            cboPromotionType.SelectedIndex = 0;

            SetControlEntrySetting(txtPromotionSchemeCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtPromotionSchemeName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboPromotionType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDiscount, new ControlEntrySetting(true, true, true, 0));
            SetControlEntrySetting(txtStartDate, new ControlEntrySetting(true, true, true, Helper.GetCurrentDate().ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtEndDate, new ControlEntrySetting(true, true, true, Helper.GetCurrentDate().ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtMultiplyQtyRevenueSharing, new ControlEntrySetting(true, true, true, 1));
            SetControlEntrySetting(txtPromotionSummary, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(PromotionScheme entity)
        {
            txtPromotionSchemeCode.Text = entity.PromotionSchemeCode;
            txtPromotionSchemeName.Text = entity.PromotionSchemeName;
            if (!string.IsNullOrEmpty(entity.GCPromotionType))
            {
                cboPromotionType.Value = entity.GCPromotionType;

                if (entity.GCPromotionType == Constant.PromotionType.GLOBAL)
                {
                    trDiscount.Style.Remove("display");
                    trMinAmount.Style.Remove("display");
                    trMinFrom.Style.Remove("display");
                    //trMultipleRevenueSharing.Style.Add("display", "none");
                    //trPromotionSummary.Style.Remove("display");
                    chkDiscountInPercentage.Checked = entity.IsDiscountInPercentage;
                    txtDiscount.Text = entity.DiscountAmount.ToString();
                    txtPromotionSummary.Text = entity.PromotionSummary;
                    txtMinimumTransaction.Text = entity.MinimumTransactionAmount.ToString();
                }
                else
                {
                    trDiscount.Style.Add("display", "none");
                    trMinAmount.Style.Add("display", "none");
                    trMinFrom.Style.Add("display", "none");
                    //trMultipleRevenueSharing.Style.Remove("display");
                    //                    trPromotionSummary.Style.Add("display", "none");
                    //txtMultiplyQtyRevenueSharing.Text = entity.MultiplyRevenueSharing.ToString();
                }
            }

            chkService.Checked = entity.IsMinimumTransactionFromService;
            chkDrugs.Checked = entity.IsMinimumTransactionFromDrug;
            chkGeneralGoods.Checked = entity.IsMinimumTransactionFromGeneralGoods;

            txtStartDate.Text = entity.StartDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtEndDate.Text = entity.EndDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtRemarks.Text = entity.Remarks;
        }

        private void ControlToEntity(PromotionScheme entity)
        {
            entity.PromotionSchemeCode = txtPromotionSchemeCode.Text;
            entity.PromotionSchemeName = txtPromotionSchemeName.Text;
            //if (cboPromotionType.Value != null)
            //{
            //entity.GCPromotionType = cboPromotionType.Value.ToString();
            entity.GCPromotionType = Constant.PromotionType.DETAIL;

            if (entity.GCPromotionType == Constant.PromotionType.GLOBAL)
            {
                entity.PromotionSummary = txtPromotionSummary.Text;
                entity.IsDiscountInPercentage = chkDiscountInPercentage.Checked;
                if (!String.IsNullOrEmpty(txtDiscount.Text))
                {
                    entity.DiscountAmount = Convert.ToDecimal(txtDiscount.Text);
                }
                else
                {
                    entity.DiscountAmount = 0;
                }

                if (!String.IsNullOrEmpty(txtMinimumTransaction.Text))
                {
                    entity.MinimumTransactionAmount = Convert.ToDecimal(txtMinimumTransaction.Text);
                }
                else
                {
                    entity.MinimumTransactionAmount = 0;
                }
            }
            else
            {
                entity.IsDiscountInPercentage = false;
                entity.DiscountAmount = 0;
                entity.MinimumTransactionAmount = 0;
                //entity.MultiplyRevenueSharing = Convert.ToInt32(txtMultiplyQtyRevenueSharing.Text);
            }
            //}

            entity.IsMinimumTransactionFromService = chkService.Checked;
            entity.IsMinimumTransactionFromDrug = chkDrugs.Checked;
            entity.IsMinimumTransactionFromGeneralGoods = chkGeneralGoods.Checked;

            entity.StartDate = Helper.GetDatePickerValue(txtStartDate);
            entity.EndDate = Helper.GetDatePickerValue(txtEndDate);
            entity.Remarks = txtRemarks.Text;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("PromotionSchemeCode = '{0}'", txtPromotionSchemeCode.Text);
            List<PromotionScheme> lst = BusinessLayer.GetPromotionSchemeList(FilterExpression);

            var DiscountAmount = Convert.ToDecimal(txtDiscount.Text);

            if (lst.Count > 0)
                errMessage = " Skema Promo dengan kode " + txtPromotionSchemeCode.Text + " telah digunakan!";


            if (Helper.GetDatePickerValue(txtEndDate) < Helper.GetDatePickerValue(txtStartDate))
                errMessage = " Tanggal akhir berlaku harus lebih besar atau sama dengan Tanggal Mulai Berlaku " + txtPromotionSchemeCode.Text + " telah digunakan!";

            //if (cboPromotionType.Value.ToString() == Constant.PromotionType.GLOBAL)
            //{
            //    if (DiscountAmount <= 0)
            //        errMessage = " Diskon tidak boleh kosong atau nol";
            //}

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            Int32 ID = Convert.ToInt32(hdnID.Value);
            string FilterExpression = string.Format("PromotionSchemeCode = '{0}' AND PromotionSchemeID != {1}", txtPromotionSchemeCode.Text, ID);
            List<PromotionScheme> lst = BusinessLayer.GetPromotionSchemeList(FilterExpression);

            var DiscountAmount = Convert.ToDecimal(txtDiscount.Text);

            if (lst.Count > 0)
                errMessage = " Skema Promo dengan kode " + txtPromotionSchemeCode.Text + " telah digunakan!";

            if (Helper.GetDatePickerValue(txtEndDate) < Helper.GetDatePickerValue(txtStartDate))
                errMessage = " Tanggal akhir berlaku harus lebih besar atau sama dengan Tanggal Mulai Berlaku " + txtPromotionSchemeCode.Text + " telah digunakan!";

            //if (cboPromotionType.Value.ToString() == Constant.PromotionType.GLOBAL)
            //{
            //    if (DiscountAmount <= 0)
            //        errMessage = " Diskon tidak boleh kosong atau nol";
            //}

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            PromotionSchemeDao entityDao = new PromotionSchemeDao(ctx);
            PromotionSchemeItemDao entityPromotionSchemeItemDao = new PromotionSchemeItemDao(ctx);
            PromotionSchemeItemGroupDao entityPromotionSchemeItemGroupDao = new PromotionSchemeItemGroupDao(ctx);
            PromotionSchemeDepartmentDao entityPromotionSchemeDepartmentDao = new PromotionSchemeDepartmentDao(ctx);
            HealthcarePromotionSchemeDao entityHealthcarePromotionDao = new HealthcarePromotionSchemeDao(ctx);
            PromotionSchemeServiceUnitDao entityPromotionSchemeServiceUnitDao = new PromotionSchemeServiceUnitDao(ctx);
            bool result = false;
            try
            {
                PromotionScheme entity = new PromotionScheme();
                if (string.IsNullOrEmpty(hdnCopyPromotionSchemeID.Value))
                {
                    ControlToEntity(entity);
                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    retval = entityDao.InsertReturnPrimaryKeyID(entity).ToString();
                    ctx.CommitTransaction();
                    result = true;
                }
                else
                {
                    PromotionScheme oldEntity = BusinessLayer.GetPromotionSchemeList(string.Format("PromotionSchemeID = {0}", hdnCopyPromotionSchemeID.Value), ctx).FirstOrDefault();
                    if (entity != null)
                    {
                        entity = oldEntity;
                        ControlToEntity(entity);
                        entity.CreatedBy = AppSession.UserLogin.UserID;
                        entity.CreatedDate = Helper.GetCurrentDate();
                        entity.IsDeleted = false;
                        entity.PromotionSchemeID = entityDao.InsertReturnPrimaryKeyID(entity);

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();

                        retval = entity.PromotionSchemeID.ToString();

                        if (entity.GCPromotionType == Constant.PromotionType.DETAIL)
                        {
                            #region Insert to PromotionSchemeDepartment
                            List<PromotionSchemeDepartment> oldLstPromotionSchemeDepartment = BusinessLayer.GetPromotionSchemeDepartmentList(string.Format("PromotionSchemeID = {0} AND IsDeleted = 0", hdnCopyPromotionSchemeID.Value), ctx);
                            if (oldLstPromotionSchemeDepartment.Count > 0)
                            {
                                foreach (PromotionSchemeDepartment department in oldLstPromotionSchemeDepartment)
                                {
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();

                                    PromotionSchemeDepartment entityDepartment = new PromotionSchemeDepartment();
                                    entityDepartment = department;
                                    entityDepartment.PromotionSchemeID = entity.PromotionSchemeID;
                                    entityDepartment.IsDeleted = false;
                                    entityDepartment.CreatedBy = AppSession.UserLogin.UserID;
                                    entityDepartment.CreatedDate = Helper.GetCurrentDate();
                                    entityPromotionSchemeDepartmentDao.Insert(entityDepartment);
                                }
                            }
                            #endregion

                            #region Insert to PromotionSchemeItem
                            List<PromotionSchemeItem> oldLstPromotionSchemeItem = BusinessLayer.GetPromotionSchemeItemList(string.Format("PromotionSchemeID = {0} AND IsDeleted = 0", hdnCopyPromotionSchemeID.Value), ctx);
                            if (oldLstPromotionSchemeItem.Count > 0)
                            {
                                foreach (PromotionSchemeItem item in oldLstPromotionSchemeItem)
                                {
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();

                                    PromotionSchemeItem entityItem = new PromotionSchemeItem();
                                    entityItem = item;
                                    entityItem.PromotionSchemeID = entity.PromotionSchemeID;
                                    entityItem.IsDeleted = false;
                                    entityItem.CreatedBy = AppSession.UserLogin.UserID;
                                    entityItem.CreatedDate = Helper.GetCurrentDate();
                                    entityPromotionSchemeItemDao.Insert(entityItem);
                                }
                            }
                            #endregion

                            #region Insert to PromotionSchemeItemGroup
                            List<PromotionSchemeItemGroup> oldLstPromotionSchemeItemGroup = BusinessLayer.GetPromotionSchemeItemGroupList(string.Format("PromotionSchemeID = {0} AND IsDeleted = 0", hdnCopyPromotionSchemeID.Value), ctx);

                            if (oldLstPromotionSchemeItemGroup.Count > 0)
                            {
                                foreach (PromotionSchemeItemGroup itemGroup in oldLstPromotionSchemeItemGroup)
                                {
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();

                                    PromotionSchemeItemGroup entityItemGroup = new PromotionSchemeItemGroup();
                                    entityItemGroup = itemGroup;
                                    entityItemGroup.PromotionSchemeID = entity.PromotionSchemeID;
                                    entityItemGroup.IsDeleted = false;
                                    entityItemGroup.CreatedBy = AppSession.UserLogin.UserID;
                                    entityItemGroup.CreatedDate = Helper.GetCurrentDate();
                                    entityPromotionSchemeItemGroupDao.Insert(entityItemGroup);
                                }
                            }
                            #endregion
                        }

                        #region Insert to HealthcarePromotionScheme
                        List<HealthcarePromotionScheme> oldLstPromotionScheme = BusinessLayer.GetHealthcarePromotionSchemeList(string.Format("PromotionSchemeID = {0}", hdnCopyPromotionSchemeID.Value), ctx);
                        if (oldLstPromotionScheme.Count > 0)
                        {
                            foreach (HealthcarePromotionScheme healthcare in oldLstPromotionScheme)
                            {
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();

                                HealthcarePromotionScheme entityHealthcare = new HealthcarePromotionScheme();
                                entityHealthcare = healthcare;
                                entityHealthcare.PromotionSchemeID = entity.PromotionSchemeID;
                                entityHealthcarePromotionDao.Insert(entityHealthcare);
                            }
                        }
                        #endregion

                        #region Insert to PromotionSchemeServiceUnit
                        List<PromotionSchemeServiceUnit> oldLstPromotionSchemeServiceUnit = BusinessLayer.GetPromotionSchemeServiceUnitList(string.Format("PromotionSchemeID = {0}", hdnCopyPromotionSchemeID.Value), ctx);
                        if (oldLstPromotionScheme.Count > 0)
                        {
                            foreach (PromotionSchemeServiceUnit promo in oldLstPromotionSchemeServiceUnit)
                            {
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();

                                PromotionSchemeServiceUnit entityPromo = new PromotionSchemeServiceUnit();
                                entityPromo = promo;
                                entityPromo.PromotionSchemeID = entity.PromotionSchemeID;
                                entityPromotionSchemeServiceUnitDao.Insert(entityPromo);
                            }
                        }
                        #endregion

                    }

                    ctx.CommitTransaction();
                    result = true;
                }
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

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                PromotionScheme entity = BusinessLayer.GetPromotionScheme(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePromotionScheme(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
    }
}