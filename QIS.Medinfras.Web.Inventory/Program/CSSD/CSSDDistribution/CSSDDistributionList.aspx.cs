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

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class CSSDDistributionList : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.CSSD_DISTRIBUTION;
        }

        private GetUserMenuAccess menu;

        protected String GetMenuCaption()
        {
            if (menu != null)
            {
                return GetLabel(menu.MenuCaption);
            }
            else
            {
                return "";
            }
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        protected override void InitializeDataControl()
        {
            MPTrx master = (MPTrx)Master;
            menu = ((MPMain)master.Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
            {
                filterExpression += " AND ";
            }
            filterExpression += String.Format("GCServiceStatus = '{0}' AND GCDistributionStatus = '{1}'", Constant.ServiceStatus.ON_STORAGE, Constant.DistributionStatus.OPEN);

            List<vMDServiceRequestDistributionReturnHd> lstEntity = BusinessLayer.GetvMDServiceRequestDistributionReturnHdList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                BindGridView();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ItemDistributionHdDao distHdDao = new ItemDistributionHdDao(ctx);
            ItemDistributionDtDao distDtDao = new ItemDistributionDtDao(ctx);
            MDServiceRequestHdDao reqHdDao = new MDServiceRequestHdDao(ctx);
            MDServiceRequestDtDao reqDtDao = new MDServiceRequestDtDao(ctx);

            try
            {
                if (type == "approve")
                {
                    #region Approve

                    string filter = string.Format("DistributionID IN ({0})", hdnParam.Value);
                    List<ItemDistributionHd> lstHd = BusinessLayer.GetItemDistributionHdList(filter, ctx);

                    foreach (ItemDistributionHd hd in lstHd)
                    {
                        if (hd.GCDistributionStatus == Constant.DistributionStatus.OPEN)
                        {
                            #region Distribution

                            string filterDistDt = string.Format("DistributionID = {0} AND IsDeleted = 0 AND GCItemDetailStatus = '{1}'", hd.DistributionID, Constant.DistributionStatus.OPEN);
                            List<ItemDistributionDt> distDtLst = BusinessLayer.GetItemDistributionDtList(filterDistDt, ctx);
                            foreach (ItemDistributionDt distDt in distDtLst)
                            {
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                distDt.GCItemDetailStatus = Constant.DistributionStatus.ON_DELIVERY;
                                distDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                distDtDao.Update(distDt);
                            }

                            hd.GCDistributionStatus = Constant.DistributionStatus.ON_DELIVERY;
                            hd.ReceivedBy = BusinessLayer.GetUser(AppSession.UserLogin.UserID).UserName;
                            hd.ReceivedDate = DateTime.Now;
                            hd.ReceivedTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                            hd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            distHdDao.Update(hd);

                            #endregion

                            #region ServiceRequest

                            MDServiceRequestHd reqHd = reqHdDao.Get(Convert.ToInt32(hd.ServiceRequestID));
                            reqHd.GCServiceStatus = Constant.ServiceStatus.ON_RETURN;
                            reqHd.ReturnedBy = AppSession.UserLogin.UserID;
                            reqHd.ReturnedDate = DateTime.Now;
                            reqHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            reqHdDao.Update(reqHd);

                            #endregion
                        }
                        else
                        {
                            result = false;
                            errMessage = "Distribusi CSSD dengan nomor " + hd.DistributionNo + " tidak dapat diubah. Harap refresh halaman ini.";
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            break;
                        }
                    }

                    if (result)
                    {
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        ctx.RollBackTransaction();
                    }

                    #endregion
                }
                else if (type == "void")
                {
                    #region Void

                    string filter = string.Format("DistributionID IN ({0})", hdnParam.Value);
                    List<ItemDistributionHd> lstHd = BusinessLayer.GetItemDistributionHdList(filter, ctx);

                    foreach (ItemDistributionHd hd in lstHd)
                    {
                        if (hd.GCDistributionStatus == Constant.DistributionStatus.OPEN)
                        {
                            #region Distribution

                            string filterDistDt = string.Format("DistributionID = {0} AND IsDeleted = 0 AND GCItemDetailStatus = '{1}'", hd.DistributionID, Constant.DistributionStatus.OPEN);
                            List<ItemDistributionDt> distDtLst = BusinessLayer.GetItemDistributionDtList(filterDistDt, ctx);
                            foreach (ItemDistributionDt distDt in distDtLst)
                            {
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                distDt.GCItemDetailStatus = Constant.DistributionStatus.VOID;
                                distDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                distDtDao.Update(distDt);
                            }

                            hd.GCDistributionStatus = Constant.DistributionStatus.VOID;
                            hd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            distHdDao.Update(hd);

                            #endregion

                            #region ServiceRequest

                            MDServiceRequestHd reqHd = reqHdDao.Get(Convert.ToInt32(hd.ServiceRequestID));

                            string filterlstReqDt = string.Format("RequestID = {0} AND IsDeleted = 0", reqHd.RequestID);
                            List<MDServiceRequestDt> lstReqDt = BusinessLayer.GetMDServiceRequestDtList(filterlstReqDt, ctx);
                            foreach (MDServiceRequestDt reqDt in lstReqDt)
                            {
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();

                                reqDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                reqDtDao.Update(reqDt);
                            }

                            reqHd.GCServiceStatus = Constant.ServiceStatus.QUALITY_CONTROL;
                            reqHd.StorageBy = null;
                            reqHd.StorageDate = Helper.GetDatePickerValue("01-01-1900");
                            reqHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            reqHdDao.Update(reqHd);

                            #endregion
                        }
                        else
                        {
                            result = false;
                            errMessage = "Distribusi CSSD dengan nomor " + hd.DistributionNo + " tidak dapat diubah. Harap refresh halaman ini.";
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            break;
                        }
                    }

                    if (result)
                    {
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        ctx.RollBackTransaction();
                    }

                    #endregion
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
    }
}