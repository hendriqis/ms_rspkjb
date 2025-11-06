using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxEditors;
using QIS.Data.Core.Dal;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class ParamedicRevenueSharingCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnID.Value = param;

            List<vRevenueSharingPerRole> rev = BusinessLayer.GetvRevenueSharingPerRoleList(string.Format("ID = {0}", hdnID.Value));
            vRevenueSharingPerRole entityRev = rev.FirstOrDefault();
            txtHealthcare.Text = entityRev.HealthcareName;
            txtItem.Text = entityRev.ItemName1;
            txtParamedic.Text = entityRev.FullName;
            txtParamedicRole.Text = entityRev.ParamedicRole;

            BindGridView();
        }

        
        List<RevenueSharingHd> lstRevShare = null;
        List<RevenueSharingPerRoleClass> lstRevenuePerRoleClass = null;
        private void BindGridView()
        {
            lstRevShare = BusinessLayer.GetRevenueSharingHdList(string.Format("IsDeleted = 0"));
            lstRevShare.Insert(0, new RevenueSharingHd { RevenueSharingID = 0, RevenueSharingName = "" });

            lstRevenuePerRoleClass = BusinessLayer.GetRevenueSharingPerRoleClassList(string.Format("ID = {0}", hdnID.Value));

            List<ClassCare> lstClass = BusinessLayer.GetClassCareList(string.Format("IsDeleted = 0"));
            grdView.DataSource = lstClass;
            grdView.DataBind();
        }

        //List<vRevenueSharingPerRoleClass> lstRevenuePerRoleClass = null;
        //protected List<vRevenueSharingPerRoleClass> initListRevenueSharing()
        //{
        //    List<vRevenueSharingPerRoleClass> lstRevenuePerRoleClass = new List<vRevenueSharingPerRoleClass>();
        //        lstRevenuePerRoleClass = BusinessLayer.GetvRevenueSharingPerRoleClassList("");
        //        return lstRevenuePerRoleClass;
        //}

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ClassCare entity = e.Row.DataItem as ClassCare;

                RevenueSharingPerRoleClass revenuePerRole = lstRevenuePerRoleClass.FirstOrDefault(p => p.ClassID == entity.ClassID);
                ASPxComboBox cboRevenueClass = (ASPxComboBox)e.Row.FindControl("cboRevenueClass");
                Methods.SetComboBoxField<RevenueSharingHd>(cboRevenueClass, lstRevShare, "RevenueSharingName", "RevenueSharingID");
                
                if (revenuePerRole != null)
                    cboRevenueClass.Value = revenuePerRole.RevenueSharingID.ToString();
                else
                    cboRevenueClass.SelectedIndex = 0;
            }
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }

        protected void cbpRevenueSharingClassProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            if (OnSaveRecord(ref errMessage))
                result = "success";
            else
                result = "fail|" + errMessage;

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnSaveRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RevenueSharingPerRoleClassDao entityDao = new RevenueSharingPerRoleClassDao(ctx);
            
            try
            {
                List<RevenueSharingPerRoleClass> lstRevShareClass = BusinessLayer.GetRevenueSharingPerRoleClassList(string.Format("ID = {0}", hdnID.Value));

                string[] listParam = hdnResult.Value.Split('|');
                foreach (string param in listParam)
                {
                    string[] temp = param.Split(';');
                    int ClassID = Convert.ToInt32(temp[0]);
                    int revenueSharingID = Convert.ToInt32(temp[1]);
                    RevenueSharingPerRoleClass entityData = lstRevShareClass.FirstOrDefault(p=>p.ClassID == ClassID);
                    if (entityData == null)
                    {
                        if (revenueSharingID > 0)
                        {
                            entityData = new RevenueSharingPerRoleClass();
                            entityData.ID = Convert.ToInt32(hdnID.Value);
                            entityData.ClassID = ClassID;
                            entityData.RevenueSharingID = revenueSharingID;
                            entityDao.Insert(entityData);
                        }
                    }
                    else
                    {
                        if (revenueSharingID == 0)
                        {
                            entityDao.Delete(Convert.ToInt32(hdnID.Value), ClassID);
                            entityDao.Update(entityData);
                        }
                        else
                        {
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