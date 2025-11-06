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

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class PrescriptionStatusEditv2 : BasePageTrx
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        private string refreshGridInterval = "10";

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Pharmacy.PRESCRIPTION_STATUS_V2;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

        protected override void InitializeDataControl()
        {
            OnControlEntrySetting();
            TabMenu(string.Empty);
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            BindGridView();
        }
        public void TabMenu(string GCTaskLogStatus)
        {
            List<BarMenuResep> lstBar = new List<BarMenuResep>();

            string cssTab1 = "";
            string cssTab2 = "";
            string cssTab3 = "";

            if (GCTaskLogStatus == Constant.PrescriptionTaskLogStatus.Started)
            {
                cssTab1 = "selected";

            }
            else if (GCTaskLogStatus == Constant.PrescriptionTaskLogStatus.Completed)
            {
                cssTab2 = "selected";
            }
            else if (GCTaskLogStatus == Constant.PrescriptionTaskLogStatus.Closed)
            {
                cssTab3 = "selected";
            }

            BarMenuResep menu1 = new BarMenuResep
            {
                CssClass = cssTab1,
                BarTitle = "SEDANG DIKERJAKAN",
            };
            BarMenuResep menu2 = new BarMenuResep
            {
                CssClass = cssTab2,
                BarTitle = "SIAP DISERAHKAN",
            };
            BarMenuResep menu3 = new BarMenuResep
            {
                CssClass = cssTab3,
                BarTitle = "SUDAH DISERAHKAN",
            };

            lstBar.Add(menu1);
            lstBar.Add(menu2);
            lstBar.Add(menu3);
            rptHSU.DataSource = lstBar;
            rptHSU.DataBind();
        }
        protected override void OnControlEntrySetting() 
        {
            
        }

        private string GetFilterExpression()
        {
            string filterExpression = "";
             
            return filterExpression;
        }

        private void BindGridView()
        {
            string filterexpression = string.Format("TransactionNo='{0}' AND GCTransactionStatus <> '{1}'", hdnTransactionNo.Value, Constant.TransactionStatus.VOID);
            List<vPrescriptionOrderHdInfo> lstPrecriptionOrderHDinfo = BusinessLayer.GetvPrescriptionOrderHdInfoList(filterexpression);
            if (lstPrecriptionOrderHDinfo.Count > 0) {
                vPrescriptionOrderHdInfo rowData = lstPrecriptionOrderHDinfo.FirstOrDefault();
                TabMenu(rowData.GCTaskLogStatus);
            }
            
            grdView.DataSource = lstPrecriptionOrderHDinfo;
            grdView.DataBind();
             
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            string errMessage = ""; 
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView();
                    result = "changepage";
                }
                if (param[0] == "setStatus")
                {
                    if (OnSaveEditStatus(ref errMessage))
                        result = "setStatus|success";
                    else
                        result = string.Format("setStatus|fail|{0}", errMessage);
                }
                else // refresh
                {
                    BindGridView();
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnSaveEditStatus(ref String errMessage)
        {
            bool result = true;

            string patientCharges = string.Format("TransactionNo='{0}'", hdnTransactionNo.Value);
            PatientChargesHd oPchd = BusinessLayer.GetPatientChargesHdList(patientCharges).FirstOrDefault();
            if (oPchd != null)
            {
                IDbContext ctx = DbFactory.Configure(true);
                PrescriptionOrderHdInfoDao entityOrderHdInfoDao = new PrescriptionOrderHdInfoDao(ctx);
                
                try
                {
                    int PrescriptionOrderID = Convert.ToInt32(oPchd.PrescriptionOrderID);
                    PrescriptionOrderHdInfo oPrescriptionOrderHdInfo = BusinessLayer.GetPrescriptionOrderHdInfo(PrescriptionOrderID);
                    Boolean isValid = true;
                    if (oPrescriptionOrderHdInfo != null)
                    {
                        if (oPrescriptionOrderHdInfo.GCTaskLogStatus == Constant.PrescriptionTaskLogStatus.Received)
                        {
                            oPrescriptionOrderHdInfo.GCTaskLogStatus = Constant.PrescriptionTaskLogStatus.Started;
                            oPrescriptionOrderHdInfo.PrescriptionStartedBy = AppSession.UserLogin.UserID;
                            oPrescriptionOrderHdInfo.PrescriptionStartedDateTime = DateTime.Now;
                           
                        }
                        else if (oPrescriptionOrderHdInfo.GCTaskLogStatus == Constant.PrescriptionTaskLogStatus.Started)
                        {
                            oPrescriptionOrderHdInfo.GCTaskLogStatus = Constant.PrescriptionTaskLogStatus.Completed;
                            oPrescriptionOrderHdInfo.PrescriptionCompletedBy = AppSession.UserLogin.UserID;
                            oPrescriptionOrderHdInfo.PrescriptionCompletedDateTime = DateTime.Now;
                           
                        }
                        else if (oPrescriptionOrderHdInfo.GCTaskLogStatus == Constant.PrescriptionTaskLogStatus.Completed)
                        {
                            oPrescriptionOrderHdInfo.GCTaskLogStatus = Constant.PrescriptionTaskLogStatus.Closed;
                            oPrescriptionOrderHdInfo.PrescriptionClosedBy = AppSession.UserLogin.UserID;
                            oPrescriptionOrderHdInfo.PrescriptionClosedDateTime = DateTime.Now;
                        }
                        else {
                            isValid = false;
                        }

                        if (isValid) {
                            entityOrderHdInfoDao.Update(oPrescriptionOrderHdInfo);
                            Helper.InsertPrescriptionOrderTaskLog(ctx, oPrescriptionOrderHdInfo.PrescriptionOrderID, oPrescriptionOrderHdInfo.GCTaskLogStatus, AppSession.UserLogin.UserID, false);
                            ctx.CommitTransaction();
                        }
                        
                    }

                   
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    ctx.RollBackTransaction();
                    result = false;
                }
                finally
                {
                    ctx.Close();
                }
            }
            else {
                result = false;
            }

           
            return result;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public class BarMenuResep {
            public string BarTitle {get; set;}
            public string CssClass { get; set; }
        }
    }
}