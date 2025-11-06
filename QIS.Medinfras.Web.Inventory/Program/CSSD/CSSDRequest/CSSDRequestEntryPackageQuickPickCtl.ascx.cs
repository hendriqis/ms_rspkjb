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
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class CSSDRequestEntryPackageQuickPickCtl : BaseEntryPopupCtl
    {
        private CSSDRequestEntry DetailPage
        {
            get { return (CSSDRequestEntry)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            hdnParam.Value = param;
            string[] temp = param.Split('|');
            hdnRequestIDCtl.Value = temp[0];
            hdnLocationIDFromCtl.Value = temp[1];
            hdnLocationHealthcareUnitFromCtl.Value = temp[2];
            hdnLocationIDToCtl.Value = temp[3];
            hdnPackageIDCtl.Value = temp[4];
            BindGridView();
        }

        protected void cbpPopup_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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

        private string GetFilterExpression()
        {
            string filterExpression = string.Format("ItemName1 LIKE '%{0}%' AND IsDeleted = 0 AND PackageID = {1}", hdnFilterItem.Value, hdnPackageIDCtl.Value);

            return filterExpression;
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vCSSDItemPackageDt entity = e.Row.DataItem as vCSSDItemPackageDt;

            }
        }

        private void BindGridView()
        {
            string filterExpression = GetFilterExpression();

            if (hdnRequestIDCtl.Value != "0" && hdnRequestIDCtl.Value != "")
            {
                List<vMDServiceRequestDt> lstItemID = BusinessLayer.GetvMDServiceRequestDtList(string.Format("RequestID = {0} AND IsDeleted = 0", hdnRequestIDCtl.Value));
                string lstSelectedID = "";
                if (lstItemID.Count > 0)
                {
                    foreach (vMDServiceRequestDt itm in lstItemID)
                    {
                        lstSelectedID += "," + itm.ItemID;
                    }
                    filterExpression += string.Format(" AND ItemID NOT IN ({0})", lstSelectedID.Substring(1));
                }
            }

            filterExpression += " ORDER BY IsConsumption, ItemName1";

            List<vCSSDItemPackageDt> lstEntity = BusinessLayer.GetvCSSDItemPackageDtList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            MDServiceRequestHdDao entityHdDao = new MDServiceRequestHdDao(ctx);
            MDServiceRequestDtDao entityDtDao = new MDServiceRequestDtDao(ctx);
            ItemDistributionHdDao distHdDao = new ItemDistributionHdDao(ctx);
            ItemDistributionDtDao distDtDao = new ItemDistributionDtDao(ctx);
            ItemTransactionHdDao consumptionHdDao = new ItemTransactionHdDao(ctx);
            ItemTransactionDtDao consumptionDtDao = new ItemTransactionDtDao(ctx);

            try
            {
                string[] lstSelectedMember = hdnSelectedMember.Value.Substring(1).Split(',');
                string[] lstSelectedMemberIsConsumption = hdnSelectedMemberIsConsumption.Value.Substring(1).Split(',');
                string[] lstSelectedMemberBaseQty = hdnSelectedMemberBaseQty.Value.Substring(1).Split(',');
                string[] lstSelectedMemberQty = hdnSelectedMemberQty.Value.Substring(1).Split(',');
                string[] lstSelectedMemberItemUnit = hdnSelectedMemberItemUnit.Value.Substring(1).Split(',');

                int RequestID = 0;
                DetailPage.SaveMDServiceRequestHd(ctx, ref RequestID);

                int oDistributionID = 0;
                string filterDistHd = string.Format("ServiceRequestID = {0} AND TransactionCode = '{1}'", RequestID, Constant.TransactionCode.SERVICE_DISTRIBUTION);
                List<ItemDistributionHd> distHdLst = BusinessLayer.GetItemDistributionHdList(filterDistHd, ctx);

                if (distHdLst.Count == 0)
                {
                    ItemDistributionHd distHd = new ItemDistributionHd();
                    distHd.TransactionCode = Constant.TransactionCode.SERVICE_DISTRIBUTION;
                    distHd.ServiceRequestID = RequestID;
                    distHd.FromLocationID = Convert.ToInt32(hdnLocationIDFromCtl.Value);
                    distHd.ToLocationID = Convert.ToInt32(hdnLocationIDToCtl.Value);
                    distHd.DeliveryDate = DateTime.Now;
                    distHd.DeliveryTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                    distHd.DeliveryRemarks = "Distribusi dari permintaan CSSD di nomor " + entityHdDao.Get(RequestID).RequestNo;
                    distHd.GCDistributionStatus = Constant.DistributionStatus.OPEN;
                    distHd.DistributionNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.SERVICE_DISTRIBUTION, distHd.DeliveryDate, ctx);
                    distHd.CreatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    oDistributionID = distHdDao.InsertReturnPrimaryKeyID(distHd);
                }
                else
                {
                    oDistributionID = distHdLst.FirstOrDefault().DistributionID;
                }

                int oConsumtionID = 0;
                string filterConsumptionHd = string.Format("ServiceRequestID = {0} AND TransactionCode = '{1}'", RequestID, Constant.TransactionCode.SERVICE_CONSUMPTION);
                List<ItemTransactionHd> consumptionLst = BusinessLayer.GetItemTransactionHdList(filterConsumptionHd, ctx);

                if (consumptionLst.Count == 0)
                {
                    ItemTransactionHd consHd = new ItemTransactionHd();
                    consHd.TransactionCode = Constant.TransactionCode.SERVICE_CONSUMPTION;
                    consHd.ServiceRequestID = RequestID;
                    consHd.FromLocationID = Convert.ToInt32(hdnLocationIDToCtl.Value);
                    consHd.GCHealthcareUnit = hdnLocationHealthcareUnitFromCtl.Value;
                    consHd.TransactionDate = DateTime.Now;
                    consHd.IsBySystem = true;
                    consHd.Remarks = "Pemakaian dari permintaan CSSD di nomor " + entityHdDao.Get(RequestID).RequestNo;
                    consHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                    consHd.TransactionNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.SERVICE_CONSUMPTION, consHd.TransactionDate, ctx);
                    consHd.CreatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    oConsumtionID = consumptionHdDao.InsertReturnPrimaryKeyID(consHd);
                }
                else
                {
                    oConsumtionID = consumptionLst.FirstOrDefault().TransactionID;
                }

                if (entityHdDao.Get(RequestID).GCServiceStatus == Constant.ServiceStatus.OPEN)
                {
                    int ct = 0;
                    foreach (String itemID in lstSelectedMember)
                    {
                        MDServiceRequestDt entityDt = new MDServiceRequestDt();
                        entityDt.RequestID = RequestID;
                        entityDt.ItemID = Convert.ToInt32(lstSelectedMember[ct]);
                        entityDt.Quantity = Convert.ToDecimal(lstSelectedMemberQty[ct]);
                        entityDt.GCItemUnit = lstSelectedMemberItemUnit[ct];
                        entityDt.BaseQuantity = Convert.ToDecimal(lstSelectedMemberBaseQty[ct]);
                        entityDt.GCBaseUnit = lstSelectedMemberItemUnit[ct];
                        entityDt.ConversionFactor = 1;

                        if (lstSelectedMemberIsConsumption[ct] == "V")
                        {
                            entityDt.IsConsumption = true;
                        }
                        else
                        {
                            entityDt.IsConsumption = false;
                        }

                        entityDt.CreatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Insert(entityDt);

                        ItemPlanning entityPlanning = BusinessLayer.GetItemPlanningList(string.Format("ItemID = {0}", entityDt.ItemID), ctx).FirstOrDefault();

                        if (lstSelectedMemberIsConsumption[ct] == "-")
                        {
                            ItemDistributionDt distDt = new ItemDistributionDt();
                            distDt.DistributionID = oDistributionID;
                            distDt.ItemID = entityDt.ItemID;
                            distDt.Quantity = entityDt.Quantity;
                            distDt.GCItemUnit = entityDt.GCItemUnit;
                            distDt.GCBaseUnit = entityDt.GCBaseUnit;
                            distDt.ConversionFactor = entityDt.ConversionFactor;
                            distDt.GCItemDetailStatus = Constant.DistributionStatus.OPEN;
                            distDt.CreatedBy = AppSession.UserLogin.UserID;

                            if (entityPlanning != null)
                            {
                                distDt.AveragePrice = entityPlanning.AveragePrice;
                            }
                            else
                            {
                                distDt.AveragePrice = 0;
                            }

                            distDtDao.Insert(distDt);
                        }

                        if (lstSelectedMemberIsConsumption[ct] == "V")
                        {
                            ItemTransactionDt consDt = new ItemTransactionDt();
                            consDt.TransactionID = oConsumtionID;
                            consDt.ItemID = entityDt.ItemID;
                            consDt.Quantity = entityDt.Quantity;
                            consDt.GCItemUnit = entityDt.GCItemUnit;
                            consDt.GCBaseUnit = entityDt.GCBaseUnit;
                            consDt.ConversionFactor = entityDt.ConversionFactor;
                            consDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                            consDt.CreatedBy = AppSession.UserLogin.UserID;

                            if (entityPlanning != null)
                            {
                                consDt.CostAmount = entityPlanning.AveragePrice;
                            }
                            else
                            {
                                consDt.CostAmount = 0;
                            }

                            consumptionDtDao.Insert(consDt);
                        }

                        ct++;
                    }
                    retval = RequestID.ToString();
                    ctx.CommitTransaction();
                }
                else 
                {
                    result = false;
                    errMessage = "Permintaan CSSD tidak dapat diubah. Harap refresh halaman ini.";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
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