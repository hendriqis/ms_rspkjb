using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Controls;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.CommonLibs.Program
{

    public partial class BroadcastMessageList : BasePageTrx
    {
        protected int pageCount = 1;
        protected int pageOutCount = 1;
        public override string OnGetMenuCode()
        {
            switch (hdnUnit.Value)
            {
                default: return Constant.MenuCode.SystemSetup.Brodcast_Message;
            }
            
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override bool IsShowRightPanel()
        {
            return false;
        }
 

        protected override void InitializeDataControl()
        {
            
            hdnPageTitle.Value = BusinessLayer.GetvMenuList(string.Format("MenuCode = '{0}'", this.OnGetMenuCode())).FirstOrDefault().MenuCaption;
            List<UserRole> lstUserRole = BusinessLayer.GetUserRoleList(string.Format("IsDeleted=0"));
            Methods.SetComboBoxField<UserRole>(cboUserGroup, lstUserRole, "LoweredRoleName", "RoleID");
            cboUserGroup.SelectedIndex = 0;

            BindInbox(1, true, ref pageCount);
            BindOutbox(1, true, ref pageCount);
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowNextPrev = IsAllowSave = IsAllowVoid = false;
        }

        private void SettingControlProperties()
        {
        }

        #region inbox

        protected void cbpInboxView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            int pageCount = 1;
               string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage"){
                  BindInbox(1, true,ref pageCount);
                }else{
                  BindInbox(1, true, ref pageCount);
                }
          
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindInbox(int pageIndex, bool isCountPageCount, ref int pageCount) {
            string filterexpression = string.Format("ToIsDeleted = 0 and ToUserID ={0}", AppSession.UserLogin.UserID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvBroadcastMessageRowCount(filterexpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                hdnPageCountInbox.Value = pageCount.ToString();
            }
            List<vBroadcastMessage> lstBroadcastMessage = BusinessLayer.GetvBroadcastMessageList(filterexpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");
            gridViewInbox.DataSource = lstBroadcastMessage;
            gridViewInbox.DataBind();

        }
        #endregion

        #region outbox
        protected void cbpOutboxView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            int pageCount = 1;
            string errMessage = "";

            string[] param = e.Parameter.Split('|');
            if (param[0] == "save")
            {
                result = "save|";
                if (onSendMessage(ref  errMessage))
                {
                    result += "success";
                } else{
                    result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0]  == "changepage")
            {
                BindOutbox(Convert.ToInt32(param[1]), false, ref pageOutCount);
                result = "changepage";
            }
            else
            {
                BindOutbox(1, true, ref pageCount);
            }
           
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        private bool onSendMessage(ref string errMessage)
        {
            bool isResult = true;
            IDbContext ctx = DbFactory.Configure(true);
            BroadcastMessageDao entityDao = new BroadcastMessageDao(ctx);
            try
            {
                string UserRoleID = cboUserGroup.Value.ToString();
                List<UserInRole> lstUser = BusinessLayer.GetUserInRoleList(string.Format("RoleID='{0}'", UserRoleID));
                if (lstUser.Count > 0)
                {
                    List<BroadcastMessage> lstUserSend = new List<BroadcastMessage>();
                    List<UserNotification> lstNofification = new List<UserNotification>();
                    foreach (UserInRole row in lstUser)
                    {
                        BroadcastMessage BcMessage = new BroadcastMessage();
                        BcMessage.ToUserID = row.UserID;
                        BcMessage.BodyMessage = txtMessage.Text;
                        BcMessage.SubjectMessage = txtSubject.Text;
                        BcMessage.CreatedBy = AppSession.UserLogin.UserID;
                        BcMessage.CreatedDate = DateTime.Now;
                        entityDao.Insert(BcMessage);

                        int countUnread = BusinessLayer.GetBroadcastMessageList(string.Format("IsRead = 0 and ToUserID='{0}' AND ToIsDeleted=0", row.UserID), ctx).Count();
                        #region FormatNotification
                        UserNotification userNotif = new UserNotification();
                        userNotif.UserID = row.UserID;
                        userNotif.Title = txtSubject.Text;
                        userNotif.Message = txtMessage.Text;
                        userNotif.TotalUnReadMessage = countUnread;
                        lstNofification.Add(userNotif);
                        #endregion
                    }
                    
                    hdnjsonSendNotification.Value = JsonConvert.SerializeObject(lstNofification);
                    ctx.CommitTransaction();
                     
                }
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                isResult = false;
                errMessage = ex.Message;
            }
            finally {
                ctx.Close();
            }
            return isResult;
        }
        private void BindOutbox(int pageIndex, bool isCountPageCount, ref int pageOutCount)
        {

            string filterexpression = string.Format("FromIsDeleted = 0 and CreatedBy ={0}", AppSession.UserLogin.UserID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvBroadcastMessageRowCount(filterexpression);
                pageOutCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }
            List<vBroadcastMessage> lstBroadcastMessage = BusinessLayer.GetvBroadcastMessageList(filterexpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");
            gridOutboxView.DataSource = lstBroadcastMessage;
            gridOutboxView.DataBind();

        }
        #endregion
        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
             
            return false;
        }
    }
}