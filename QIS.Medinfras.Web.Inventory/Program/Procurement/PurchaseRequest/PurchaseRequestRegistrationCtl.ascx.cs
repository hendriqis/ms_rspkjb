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

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class PurchaseRequestRegistrationCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnPurchaseRequestID.Value = param;

            vPurchaseRequestHd entityRequest = BusinessLayer.GetvPurchaseRequestHdList(String.Format("PurchaseRequestID = {0}", Convert.ToInt32(hdnPurchaseRequestID.Value)))[0];
            txtPurchaseRequestNo.Text = entityRequest.PurchaseRequestNo;

            BindGridView();
            SetControlProperties();
        }
        private void SetControlProperties()
        {
            txtPurchaseRequestNo.Attributes.Add("validationgroup", "mpPurchaseRequest");
        }

        private void BindGridView()
        {
            grdPurchaseRequestRegistration.DataSource = BusinessLayer.GetvPurchaseRequestRegistrationList(string.Format("PurchaseRequestID = {0} AND IsDeleted = 0", hdnPurchaseRequestID.Value));
            grdPurchaseRequestRegistration.DataBind();
        }

        protected void cbpPurchaseRequestRegistration_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            string result = "";
            string errMessage = "";
            if (e.Parameter == "save")
            {
                result = "save|";
                if (hdnID.Value.ToString() != "")
                {
                    if (OnSaveEditRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else
            {
                result = "delete|";
                if (OnDeleteRecord(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(PurchaseRequestRegistration entity)
        {
            entity.PurchaseRequestID = Convert.ToInt32(hdnPurchaseRequestID.Value);
            entity.RegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                PurchaseRequestRegistration entity = new PurchaseRequestRegistration();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entity.CreatedDate = DateTime.Now;
                BusinessLayer.InsertPurchaseRequestRegistration(entity);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
            }
            return result;
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                PurchaseRequestRegistration entity = BusinessLayer.GetPurchaseRequestRegistration(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entity.LastUpdatedDate = DateTime.Now;
                BusinessLayer.UpdatePurchaseRequestRegistration(entity);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
            }
            return result;
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                PurchaseRequestRegistration entity = BusinessLayer.GetPurchaseRequestRegistration(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entity.LastUpdatedDate = DateTime.Now;
                entity.IsDeleted = true;
                BusinessLayer.UpdatePurchaseRequestRegistration(entity);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
            }
            return result;
        }
    }
}