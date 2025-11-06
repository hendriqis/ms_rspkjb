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
using DevExpress.Web.ASPxEditors;
using QIS.Data.Core.Dal;
using System.Web.UI.HtmlControls;


namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class ParamedicRevenueSharingList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.PARAMEDIC_REVENUE_SHARING;
        }

        protected string OnGetItemFilterExpression()
        {
            return string.Format("GCItemType NOT IN ('{0}','{1}','{2}') AND IsDeleted = 0", Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, Constant.ItemType.BARANG_UMUM);
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            hdnFilterExpression.Value = filterExpression;
            hdnID.Value = keyValue;
            filterExpression = GetFilterExpression();

            Helper.SetControlEntrySetting(txtItemCode, new ControlEntrySetting(true, true, true), "mpPatientEntry");
            Helper.SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, true, false), "mpPatientEntry");
            
            List<vHealthcare> lstHealthCare = BusinessLayer.GetvHealthcareList("");
            Methods.SetComboBoxField<vHealthcare>(cboHealthCare, lstHealthCare, "HealthcareName", "HealthcareID");
            cboHealthCare.SelectedIndex = 0;

            BindGridView();
        }


        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowDelete = IsAllowEdit = false;
        }

        private string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += "IsDeleted = 0";
                       
            return filterExpression;
        }


        List<vRevenueSharingPerRole> lstRevenuePerRole = null;
        List<RevenueSharingHd> lstRevShare = null;
  
        private void BindGridView()
        {
            string paramedicFilter;

            if (string.IsNullOrEmpty(hdnPhysicianID.Value))
            {
                paramedicFilter = "ParamedicID IS NULL";
            }
            else
            {
                paramedicFilter = string.Format("ParamedicID = '{0}'", hdnPhysicianID.Value);
            }

            string filterExpression = string.Format(
                "HealthcareID = '{0}' AND ItemID = '{1}' AND {2} AND IsDeleted = 0",
                cboHealthCare.Value,
                hdnItemID.Value,
                paramedicFilter
            );

            List<RevenueSharingPerRole> lstRev = BusinessLayer.GetRevenueSharingPerRoleList(filterExpression);

            lstRevenuePerRole = BusinessLayer.GetvRevenueSharingPerRoleList(filterExpression);
            lstRevShare = BusinessLayer.GetRevenueSharingHdList(string.Format("IsDeleted = 0"));
            lstRevShare.Insert(0, new RevenueSharingHd { RevenueSharingID = 0, RevenueSharingName = "" });

            List<StandardCode> lstEntity = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsActive = 1", Constant.StandardCode.PARAMEDIC_ROLE));
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected List<vRevenueSharingPerRole> initListRevenueSharing()
        {
            List<vRevenueSharingPerRole> lstRevenuePerRole = new List<vRevenueSharingPerRole>();
                lstRevenuePerRole = BusinessLayer.GetvRevenueSharingPerRoleList(string.Format("HealthcareID ='{0}' AND ItemID = '{1}' AND ParamedicID = '{2}' AND IsDeleted = 0", cboHealthCare.Value, hdnItemID.Value, hdnPhysicianID.Value));
            return lstRevenuePerRole;
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType == DataControlRowType.DataRow)
            {
                StandardCode entity = e.Row.DataItem as StandardCode;

                vRevenueSharingPerRole revenuePerRole = lstRevenuePerRole.FirstOrDefault(p => p.GCParamedicRole == entity.StandardCodeID);
                ASPxComboBox cboRevenue = (ASPxComboBox)e.Row.FindControl("cboRevenue");
                HtmlAnchor lnkClass = (HtmlAnchor)e.Row.FindControl("lnkClass");

                Methods.SetComboBoxField<RevenueSharingHd>(cboRevenue, lstRevShare, "RevenueSharingName", "RevenueSharingID");
                if (revenuePerRole != null)
                {
                    cboRevenue.Value = revenuePerRole.RevenueSharingID.ToString();
                    HtmlInputHidden hdnRowID = (HtmlInputHidden)e.Row.FindControl("hdnRowID");
                    hdnRowID.Value = revenuePerRole.ID.ToString();
                }
                else
                {
                    cboRevenue.SelectedIndex = 0;
                    lnkClass.Style.Add("display", "none");
                }
            }
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
        }

        protected override bool OnCustomButtonClick(string type, ref string retval, ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RevenueSharingPerRoleDao entityDao = new RevenueSharingPerRoleDao(ctx);
            
            try
            {
                string paramedicFilter;

                if (string.IsNullOrEmpty(hdnPhysicianID.Value))
                {
                    paramedicFilter = "ParamedicID IS NULL";
                }
                else
                {
                    paramedicFilter = string.Format("ParamedicID = '{0}'", hdnPhysicianID.Value);
                }

                string filterExpression = string.Format(
                    "HealthcareID = '{0}' AND ItemID = '{1}' AND {2} AND IsDeleted = 0",
                    cboHealthCare.Value,
                    hdnItemID.Value,
                    paramedicFilter
                );

                List<RevenueSharingPerRole> lstRev = BusinessLayer.GetRevenueSharingPerRoleList(filterExpression);

                string[] listParam = hdnResult.Value.Split('|');
                foreach (string param in listParam)
                {
                    string[] temp = param.Split(';');
                    string GCParamedicRole = temp[0];
                    int revenueSharingID = Convert.ToInt32(temp[1]);
                    RevenueSharingPerRole entityData = lstRev.FirstOrDefault(p => p.GCParamedicRole == GCParamedicRole);
                    if (entityData == null)
                    {
                        if (revenueSharingID > 0)
                        {
                            entityData = new RevenueSharingPerRole();
                            entityData.RevenueSharingID = revenueSharingID;
                            entityData.HealthcareID = AppSession.UserLogin.HealthcareID;
                            //entityData.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
                            
                            // cek ParamedicID
                            if (string.IsNullOrWhiteSpace(hdnPhysicianID.Value))
                                entityData.ParamedicID = null;   // jika kosong → NULL
                            else
                                entityData.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);

                            entityData.GCParamedicRole = GCParamedicRole;
                            entityData.GCParamedicRole = GCParamedicRole;
                            entityData.ItemID = Convert.ToInt32(hdnItemID.Value); 
                            entityData.CreatedBy = AppSession.UserLogin.UserID;
                            entityDao.Insert(entityData);
                        }
                    }
                    else
                    {
                        if (revenueSharingID > 0)
                        {
                            entityData.RevenueSharingID = revenueSharingID;
                            entityData.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDao.Update(entityData);
                        }
                        else
                        {
                            entityData.IsDeleted = true;
                            entityData.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDao.Update(entityData);
                        }
                    }
                }         
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
       
    }
}