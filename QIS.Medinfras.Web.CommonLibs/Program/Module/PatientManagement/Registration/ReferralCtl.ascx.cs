using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ReferralCtl : BaseViewPopupCtl
    {

        private const string DEFAULT_GRDVIEW_FILTER = "BusinessPartnerID > 1 AND GCBusinessPartnerType = '{0}' AND (HealthcareID = '{1}' OR HealthcareID IS NULL) AND IsDeleted = 0";
        protected int PageCount = 1;

        public override void InitializeDataControl(string param)
        {
            List<SettingParameterDt> lstSettingParameter = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN ('{0}')",
                Constant.SettingParameter.IS_BRIDGING_TO_INHEALTH));
            hdnIsBridgingToInhealth.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_INHEALTH).ParameterValue;

            Helper.SetControlEntrySetting(txtReferrerNameCtl, new ControlEntrySetting(true, true, true), "mpReferrer");
            Helper.SetControlEntrySetting(cboReferrerGroupCtl, new ControlEntrySetting(true, true, true), "mpReferrer");
            Helper.SetControlEntrySetting(cboReferrerSearchCode, new ControlEntrySetting(true, true, false), "mpReferrer");
            Helper.SetControlEntrySetting(txtAddressCtl, new ControlEntrySetting(true, true, true), "mpReferrer");
            Helper.SetControlEntrySetting(txtContactReferrerCtl, new ControlEntrySetting(true, true, true), "mpReferrer");
            Helper.SetControlEntrySetting(txtTelephoneNoCtl, new ControlEntrySetting(true, true, true), "mpReferrer");
            SetControlProperties();
            BindGridView(1, true, ref PageCount);
        }

        #region Popup Filter Expression
        protected string OnGetSCProvinceFilterExpression()
        {
            return string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PROVINCE);
        }
        #endregion

        private void SetControlProperties()
        {
            List<StandardCode> listSc = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.REFERRER_GROUP));
            Methods.SetComboBoxField<StandardCode>(cboReferrerGroupCtl, listSc, "StandardCodeName", "StandardCodeID");
            cboReferrerGroupCtl.SelectedIndex = 0;
            listSc.Insert(0, new StandardCode { StandardCodeName = "", StandardCodeID = "" });
            Methods.SetComboBoxField<StandardCode>(cboReferrerSearchCode, listSc, "StandardCodeName", "StandardCodeID");
            cboReferrerSearchCode.SelectedIndex = 0;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = String.Format(DEFAULT_GRDVIEW_FILTER, Constant.BusinessObjectType.RUJUKAN_DARI_PIHAK_KETIGA, AppSession.UserLogin.HealthcareID);
            if (cboReferrerSearchCode.Value != null)
            {
                if (cboReferrerSearchCode.Value.ToString() != "")
                {
                    filterExpression = String.Format("BusinessPartnerID > 1 AND GCBusinessPartnerType = '{0}' AND (HealthcareID = '{1}' OR HealthcareID IS NULL) AND IsDeleted = 0 AND GCReferrerGroup = '{2}'", Constant.BusinessObjectType.RUJUKAN_DARI_PIHAK_KETIGA, AppSession.UserLogin.HealthcareID, cboReferrerSearchCode.Value.ToString());
                }
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvReferrerRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 8);
            }

            List<vReferrer> lstEntity = BusinessLayer.GetvReferrerList(filterExpression, 8, pageIndex, "BusinessPartnerName ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            int pageCount = 1;

            string[] param = e.Parameter.Split('|');

            string result = param[0] + "|";
            string errMessage = "";

            if (param[0] == "changepage")
            {
                BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                result = "changepage";
            }
            else if (param[0] == "refresh")
            {
                BindGridView(1, true, ref pageCount);
                result = "refresh|" + pageCount;
            }
            else
            {
                if (param[0] == "save")
                {
                    if (hdnIDCtl.Value.ToString() != "")
                    {
                        if (OnSaveEditRecord(ref errMessage))
                        {
                            result += "success";
                            BindGridView(1, true, ref pageCount);
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
                            result += "success";
                            BindGridView(1, true, ref pageCount);
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
                        result += "success";
                        BindGridView(1, true, ref pageCount);
                    }
                    else
                    {
                        result += string.Format("fail|{0}", errMessage);
                    }
                }
            }
            result += "|" + pageCount;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(BusinessPartners entityBusinessPartners, Referrer entityReferrer, Address entityAddress, BusinessPartnerTagField entityTagField)
        {
            #region General Information
            entityBusinessPartners.BusinessPartnerName = txtReferrerNameCtl.Text.ToString();
            entityBusinessPartners.ShortName = txtShortNameReferrerCtl.Text.ToString();
            entityBusinessPartners.ContactPerson = txtContactReferrerCtl.Text.ToString();
            entityBusinessPartners.CommCode = txtKodeExternalCtl.Text.ToString();
            if (hdnIsBridgingToInhealth.Value == "1")
            {
                entityBusinessPartners.InhealthCode = txtKodeInhealthProviderRujukan.Text.ToString() + "|" + txtNamaInhealthProviderRujukan.Text.ToString();
            }
            #endregion

            #region Customer Information
            entityReferrer.GCReferrerGroup = cboReferrerGroupCtl.Value.ToString();
            #endregion

            #region Address
            entityAddress.StreetName = txtAddressCtl.Text;
            entityAddress.County = txtCountyCtl.Text; // Desa
            entityAddress.District = txtDistrictCtl.Text; //Kabupaten
            entityAddress.City = txtCityCtl.Text;
            entityAddress.GCState = txtProvinceCodeCtl.Text == "" ? null : string.Format("{0}^{1}", Constant.StandardCode.PROVINCE, txtProvinceCodeCtl.Text);
            if (hdnZipCodeCtl.Value == "")
                entityAddress.ZipCode = null;
            else
                entityAddress.ZipCode = Convert.ToInt32(hdnZipCodeCtl.Value);
            entityAddress.PhoneNo1 = txtTelephoneNoCtl.Text;
            #endregion
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            String retval;
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            BusinessPartnersDao entityDao = new BusinessPartnersDao(ctx);
            ReferrerDao entityRefDao = new ReferrerDao(ctx);
            AddressDao entityAddressDao = new AddressDao(ctx);
            BusinessPartnerTagFieldDao entityTagFieldDao = new BusinessPartnerTagFieldDao(ctx);

            try
            {
                BusinessPartners entity = new BusinessPartners();
                Referrer entityRef = new Referrer();
                Address entityAddress = new Address();
                BusinessPartnerTagField entityTagField = new BusinessPartnerTagField();
                ControlToEntity(entity, entityRef, entityAddress, entityTagField);
                entity.BusinessPartnerCode = Helper.GeneratePartnerCode(ctx, entity.BusinessPartnerName);

                entity.GCBusinessPartnerType = Constant.BusinessObjectType.RUJUKAN_DARI_PIHAK_KETIGA;
                entity.IsActive = true;

                entityAddress.CreatedBy = AppSession.UserLogin.UserID;
                entityAddressDao.Insert(entityAddress);
                entity.AddressID = BusinessLayer.GetAddressMaxID(ctx);

                entity.CreatedBy = AppSession.UserLogin.UserID;

                if (String.IsNullOrEmpty(entity.CommCode))
                {
                    entity.CommCode = entity.BusinessPartnerCode;
                }

                entityDao.Insert(entity);

                entity.BusinessPartnerID = BusinessLayer.GetBusinessPartnersMaxID(ctx);
                entityRef.BusinessPartnerID = entity.BusinessPartnerID;
                entityRefDao.Insert(entityRef);

                entityTagField.BusinessPartnerID = entity.BusinessPartnerID;
                entityTagFieldDao.Insert(entityTagField);

                retval = entity.BusinessPartnerID.ToString();
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            BusinessPartnersDao entityDao = new BusinessPartnersDao(ctx);
            ReferrerDao entityRefDao = new ReferrerDao(ctx);
            AddressDao entityAddressDao = new AddressDao(ctx);
            BusinessPartnerTagFieldDao entityTagFieldDao = new BusinessPartnerTagFieldDao(ctx);
            try
            {
                int BusinessPartnerID = Convert.ToInt32(hdnIDCtl.Value);
                BusinessPartners entity = entityDao.Get(BusinessPartnerID);
                if (!entity.IsDeleted)
                {
                    Referrer entityRef = entityRefDao.Get(BusinessPartnerID);
                    BusinessPartnerTagField entityTagField = entityTagFieldDao.Get(BusinessPartnerID);
                    Address entityAddress = entityAddressDao.Get((int)entity.AddressID);

                    ControlToEntity(entity, entityRef, entityAddress, entityTagField);
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityRef.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityTagField.LastUpdatedBy = AppSession.UserLogin.UserID;

                    if (String.IsNullOrEmpty(entity.CommCode))
                    {
                        entity.CommCode = entity.BusinessPartnerCode;
                    }

                    entityAddress.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityAddressDao.Update(entityAddress);
                    entityRefDao.Update(entityRef);
                    entityTagFieldDao.Update(entityTagField);
                    entityDao.Update(entity);
                    ctx.CommitTransaction();
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            Boolean result = true;
            IDbContext ctx = DbFactory.Configure(true);
            BusinessPartnersDao entityDao = new BusinessPartnersDao(ctx);

            BusinessPartners entity = BusinessLayer.GetBusinessPartners(Convert.ToInt32(hdnIDCtl.Value));
            if (!entity.IsDeleted)
            {
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);
            }
            return result;
        }
    }
}