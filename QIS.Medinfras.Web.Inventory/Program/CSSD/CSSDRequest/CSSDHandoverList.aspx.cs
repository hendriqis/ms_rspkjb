using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class CSSDHandoverList : BasePageTrx
    {
        protected int PageCount = 1;
        protected string filterExpressionLocation = "";
        protected string filterExpressionLocationTo = "";

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.CSSD_HANDOVER;
        }

        private GetUserMenuAccess menu;

        protected String GetMenuCaption()
        {
            if (menu != null)
                return GetLabel(menu.MenuCaption);
            return "";
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = false;
            IsAllowSave = false;
        }

        protected override void InitializeDataControl()
        {
            MPTrx master = (MPTrx)Master;
            menu = ((MPMain)master.Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

            filterExpressionLocation = string.Format("{0};0;{1};", AppSession.UserLogin.HealthcareID, Constant.TransactionCode.SERVICE_REQUEST);
            
            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = hdnFilterExpression.Value;

            if (filterExpression != "")
            {
                filterExpression += " AND ";
            }

            filterExpression += String.Format("GCServiceStatus IN ('{0}') AND ReceivedBy IS NULL AND ReceivedDate IS NULL", Constant.ServiceStatus.ON_DELIVERY);
            
            if (hdnLocationIDFrom.Value != null && hdnLocationIDFrom.Value.ToString() != "")
            {
                filterExpression += string.Format(" AND FromLocationID = {0}", hdnLocationIDFrom.Value);
            }

            List<vMDServiceRequestHd> lstEntity = BusinessLayer.GetvMDServiceRequestHdList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ASPxComboBox cboPreWashingVolume = (ASPxComboBox)e.Row.FindControl("cboPreWashingVolume");
                ASPxComboBox cboPreWashingCondition = (ASPxComboBox)e.Row.FindControl("cboPreWashingCondition");


                List<StandardCode> listStandardCode = BusinessLayer.GetStandardCodeList(string.Format(
                                                "ParentID IN ('{0}','{1}') AND IsDeleted = 0 AND IsActive = 1",
                                                Constant.StandardCode.PREWASHING_VOLUME, Constant.StandardCode.PREWASHING_CONDITION));

                cboPreWashingVolume.ClientInstanceName = string.Format("cboPreWashingVolume{0}", e.Row.DataItemIndex);

                Methods.SetComboBoxField<StandardCode>(cboPreWashingVolume, listStandardCode.Where(p => p.ParentID == Constant.StandardCode.PREWASHING_VOLUME).ToList<StandardCode>(), "StandardCodeName", "StandardCodeID");
                cboPreWashingVolume.SelectedIndex = 0;


                cboPreWashingCondition.ClientInstanceName = string.Format("cboPreWashingCondition{0}", e.Row.DataItemIndex);

                Methods.SetComboBoxField<StandardCode>(cboPreWashingCondition, listStandardCode.Where(p => p.ParentID == Constant.StandardCode.PREWASHING_CONDITION).ToList<StandardCode>(), "StandardCodeName", "StandardCodeID");
                cboPreWashingCondition.SelectedIndex = 0;
            }
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";

            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "process")
                {
                    if (OnProcessHandover(ref errMessage))
                    {
                        result += string.Format("{0}|success", param[0]);
                    }
                    else
                    {
                        result += string.Format("{0}|fail|{1}", param[0], errMessage);
                    }
                }
                else if (param[0] == "decline")
                {
                    if (OnDeclineHandover(ref errMessage))
                    {
                        result += string.Format("{0}|success", param[0]);
                    }
                    else
                    {
                        result += string.Format("{0}|fail|{1}", param[0], errMessage);
                    }
                }
                else
                {
                    BindGridView();
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnProcessHandover(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            MDServiceRequestHdDao entityDao = new MDServiceRequestHdDao(ctx);
            ItemDistributionHdDao distHdDao = new ItemDistributionHdDao(ctx);
            ItemDistributionDtDao distDtDao = new ItemDistributionDtDao(ctx);

            try
            {
                string[] lstRequestID = hdnSelectedRequestID.Value.Substring(1).Split(',');
                string[] lstVolume = hdnSelectedVolume.Value.Substring(1).Split(',');
                string[] lstCondition = hdnSelectedCondition.Value.Substring(1).Split(',');

                for (int i = 0; i < lstRequestID.Count(); i++)
                {
                    MDServiceRequestHd entity = entityDao.Get(Convert.ToInt32(lstRequestID[i]));
                    entity.GCPreWashingVolume = lstVolume[i];
                    entity.GCPreWashingCondition = lstCondition[i];
                    entity.ReceivedBy = AppSession.UserLogin.UserID;
                    entity.ReceivedDate = DateTime.Now;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entity.LastUpdatedDate = DateTime.Now;
                    entityDao.Update(entity);
                    
                    #region Distribution

                    string filterDistHd = string.Format("ServiceRequestID = '{0}' AND GCDistributionStatus = '{1}'", entity.RequestID, Constant.DistributionStatus.ON_DELIVERY);
                    List<ItemDistributionHd> distHdLst = BusinessLayer.GetItemDistributionHdList(filterDistHd, ctx);
                    if (distHdLst.Count > 0)
                    {
                        foreach (ItemDistributionHd distHd in distHdLst)
                        {
                            string filterDistDt = string.Format("DistributionID = {0} AND IsDeleted = 0 AND GCItemDetailStatus = '{1}'", distHd.DistributionID, Constant.DistributionStatus.ON_DELIVERY);
                            List<ItemDistributionDt> distDtLst = BusinessLayer.GetItemDistributionDtList(filterDistDt, ctx);
                            foreach (ItemDistributionDt distDt in distDtLst)
                            {
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                distDt.GCItemDetailStatus = Constant.DistributionStatus.RECEIVED;
                                distDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                distDtDao.Update(distDt);
                            }

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            distHd.GCDistributionStatus = Constant.DistributionStatus.RECEIVED;
                            distHd.ReceivedBy = BusinessLayer.GetUser(AppSession.UserLogin.UserID).UserName;
                            distHd.ReceivedDate = DateTime.Now;
                            distHd.ReceivedTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                            distHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            distHdDao.Update(distHd);
                        }
                    }

                    #endregion
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

        private bool OnDeclineHandover(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            MDServiceRequestHdDao entityDao = new MDServiceRequestHdDao(ctx);
            ItemDistributionHdDao distHdDao = new ItemDistributionHdDao(ctx);
            ItemDistributionDtDao distDtDao = new ItemDistributionDtDao(ctx);

            try
            {
                string[] lstRequestID = hdnSelectedRequestID.Value.Substring(1).Split(',');
                string[] lstVolume = hdnSelectedVolume.Value.Substring(1).Split(',');
                string[] lstCondition = hdnSelectedCondition.Value.Substring(1).Split(',');

                for (int i = 0; i < lstRequestID.Count(); i++)
                {
                    MDServiceRequestHd entity = entityDao.Get(Convert.ToInt32(lstRequestID[i]));
                    entity.GCServiceStatus = Constant.ServiceStatus.OPEN;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entity.LastUpdatedDate = DateTime.Now;
                    entityDao.Update(entity);

                    #region Distribution

                    string filterDistHd = string.Format("ServiceRequestID = '{0}' AND GCDistributionStatus = '{1}'", entity.RequestID, Constant.DistributionStatus.ON_DELIVERY);
                    List<ItemDistributionHd> distHdLst = BusinessLayer.GetItemDistributionHdList(filterDistHd, ctx);
                    if (distHdLst.Count > 0)
                    {
                        foreach (ItemDistributionHd distHd in distHdLst)
                        {
                            string filterDistDt = string.Format("DistributionID = {0} AND IsDeleted = 0 AND GCItemDetailStatus = '{1}'", distHd.DistributionID, Constant.DistributionStatus.ON_DELIVERY);
                            List<ItemDistributionDt> distDtLst = BusinessLayer.GetItemDistributionDtList(filterDistDt, ctx);
                            foreach (ItemDistributionDt distDt in distDtLst)
                            {
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                distDt.GCItemDetailStatus = Constant.DistributionStatus.OPEN;
                                distDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                distDtDao.Update(distDt);
                            }

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            distHd.GCDistributionStatus = Constant.DistributionStatus.OPEN;
                            distHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            distHdDao.Update(distHd);
                        }
                    }

                    #endregion
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