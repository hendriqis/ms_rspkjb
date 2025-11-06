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
using System.Data;
using QIS.Medinfras.Web.CommonLibs.Service;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class BankChannelDtList : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                hdnBankID.Value = param;
                Bank entity = BusinessLayer.GetBank(Convert.ToInt32(param));
                txtBankName.Text = entity.BankName;

                List<StandardCode> lstProvider = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PAYMENT_GATEWAY_METHOD));
                Methods.SetComboBoxField<StandardCode>(cboProviderMethod, lstProvider, "StandardCodeName", "StandardCodeID");

                cboProviderMethod.Attributes.Add("validationgroup", "mpBankChannelDt");

                BindGridView();
            }
        }

        private void BindGridView()
        {
            grdView.DataSource = BusinessLayer.GetvBankChannelDtList(string.Format("BankID = {0} AND IsDeleted = 0", hdnBankID.Value));
            grdView.DataBind();
        }

        protected void cbpBankChannelDt_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            string result = "";
            string errMessage = "";
            if (e.Parameter == "save")
            {
                result = "save|";
                if (cboProviderMethod.Value != null) 
                {
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
                    result += string.Format("fail|Provider tidak boleh kosong");
                }
            }
            else if (e.Parameter == "delete")
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

        private void ControlToEntity(BankChannelDt entity)
        {
            entity.BankID = Convert.ToInt32(hdnBankID.Value);
            entity.GCProviderMethod = cboProviderMethod.Value.ToString();
            entity.ServiceCode = txtServiceCode.Text;
            entity.ChannelID = txtChannelID.Text;
            entity.SecretKey = txtSecretKey.Text;
            entity.CompanyCode = txtCompanyCode.Text;
            entity.BIN = txtBIN.Text;
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;

            List<BankChannelDt> lstDt = BusinessLayer.GetBankChannelDtList(string.Format("BankID = {0} AND GCProviderMethod = '{1}' AND IsDeleted = 0", hdnBankID.Value, cboProviderMethod.Value.ToString()));

            if (lstDt.Count == 0)
            {
                IDbContext ctx = DbFactory.Configure(true);
                BankChannelDtDao entityDao = new BankChannelDtDao(ctx);

                try
                {
                    BankChannelDt entity = new BankChannelDt();
                    ControlToEntity(entity);
                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    entity.CreatedDate = DateTime.Now;
                    entityDao.InsertReturnPrimaryKeyID(entity);

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
            }
            else
            {
                result = false;
                errMessage = "Sudah ada data konfigurasi untuk provider ini";
            }

            return result;
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                BankChannelDt entity = BusinessLayer.GetBankChannelDt(Convert.ToInt32(hdnID.Value));
                if (entity != null)
                {
                    ControlToEntity(entity);
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entity.LastUpdatedDate = DateTime.Now;
                    BusinessLayer.UpdateBankChannelDt(entity);

                    return true;
                }
                else
                {
                    errMessage = "No Data Found";
                    return true;
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;

            BankChannelDt entity = BusinessLayer.GetBankChannelDt(Convert.ToInt32(hdnID.Value));
            IDbContext ctx = DbFactory.Configure(true);
            BankChannelDtDao entityDao = new BankChannelDtDao(ctx);

            try
            {
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entity.LastUpdatedDate = DateTime.Now;
                entityDao.Update(entity);
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