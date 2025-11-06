using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Controls;
using QIS.Medinfras.Web.CommonLibs.Program;
using QIS.Medinfras.Web.CommonLibs.Service;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Laboratory.Program
{
    public partial class LaboratoryVerificationResultDetail : BasePageTrx
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;
        private string lstMember = "";

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Laboratory.LAB_RESULT_VERIFICATION;
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString.Count > 0)
            {
                hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

                String transactionID = Page.Request.QueryString["id"];
                hdnTransactionHdID.Value = transactionID;
                hdnLabResultID.Value = BusinessLayer.GetLaboratoryResultHdList(string.Format("ChargeTransactionID = {0}", hdnTransactionHdID.Value)).FirstOrDefault().ID.ToString();

                List<vLaboratoryResultDt> entityLabDt = BusinessLayer.GetvLaboratoryResultDtList(string.Format("ID = {0} AND IsVerified = 1 AND IsDeleted = 0", hdnLabResultID.Value));
                foreach (vLaboratoryResultDt labDt in entityLabDt)
                {
                    lstMember += labDt.CustomID;
                    lstMember += ",";
                }
                hdnSelectedMember.Value = lstMember;

                vConsultVisit2 entity = BusinessLayer.GetvConsultVisit2List(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
                ctlPatientBanner.InitializePatientBanner(entity);

                BindGridView();
            }
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridView();
                result = "refresh";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private string GetFilterExpression()
        {
            string filterExpression = string.Format("ID = {0} AND IsDeleted = 0 AND ResultGCTransactionStatus = '{1}'", hdnLabResultID.Value, Constant.TransactionStatus.WAIT_FOR_APPROVAL);
            
            return filterExpression;
        }

        private void BindGridView()
        {
            string filterExpression = GetFilterExpression();
            lstSelectedMember = hdnSelectedMember.Value.Split(',');

            List<vLaboratoryResultDt> lstEntity = BusinessLayer.GetvLaboratoryResultDtList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vLaboratoryResultDt entity = e.Item.DataItem as vLaboratoryResultDt;
                CheckBox chkIsSelected = e.Item.FindControl("chkIsSelected") as CheckBox;
                if (lstSelectedMember.Contains(entity.CustomID))
                    chkIsSelected.Checked = true;
                else
                    chkIsSelected.Checked = false;
            }
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            LaboratoryResultHdDao labResultHdDao = new LaboratoryResultHdDao(ctx);
            LaboratoryResultDtDao labResultDtDao = new LaboratoryResultDtDao(ctx);
            try
            {
                if (type == "verified")
                {
                    #region Verified

                    string filterExpression = String.Format("ID = {0} AND IsDeleted = 0", hdnLabResultID.Value);
                    List<LaboratoryResultDt> lstLabResultDt = BusinessLayer.GetLaboratoryResultDtList(filterExpression, ctx);
                    foreach (ListViewDataItem item in lvwView.Items)
                    {
                        CheckBox chkIsSelected = (CheckBox)item.FindControl("chkIsSelected");
                        HtmlInputHidden hdnCustomID = (HtmlInputHidden)item.FindControl("keyField");
                        string[] param = hdnCustomID.Value.Split('|');
                        LaboratoryResultDt labDt = lstLabResultDt.FirstOrDefault(p => p.ItemID == Convert.ToInt32(param[0]) && p.FractionID == Convert.ToInt32(param[1]));
                        if (chkIsSelected.Checked)
                        {
                            if (!labDt.IsVerified)
                            {
                                labDt.IsVerified = true;
                                labDt.VerifiedBy = AppSession.UserLogin.UserID;
                                labDt.VerifiedDate = DateTime.Now;
                                labDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                labResultDtDao.Update(labDt);
                            }
                        }
                        else
                        {
                            if (labDt.IsVerified)
                            {
                                labDt.IsVerified = false;
                                labDt.VerifiedBy = null;
                                labDt.VerifiedDate = null;
                                labDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                labResultDtDao.Update(labDt);
                            }
                        }
                    }

                    int rowCount = BusinessLayer.GetLaboratoryResultDtRowCount(string.Format("ID = {0} AND IsVerified = 0 AND IsDeleted = 0", hdnLabResultID.Value), ctx);
                    if (rowCount < 1)
                    {
                        LaboratoryResultHd labHd = labResultHdDao.Get(Convert.ToInt32(hdnLabResultID.Value));
                        labHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                        labHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        labResultHdDao.Update(labHd);
                    }

                    ctx.CommitTransaction();

                    #endregion
                }
                else if (type == "unverified")
                {
                    #region Unverified

                    string filterExpression = String.Format("ID = {0} AND IsDeleted = 0", hdnLabResultID.Value);
                    List<LaboratoryResultDt> lstLabResultDt = BusinessLayer.GetLaboratoryResultDtList(filterExpression, ctx);
                    foreach (LaboratoryResultDt item in lstLabResultDt)
                    {
                        item.IsVerified = false;
                        item.VerifiedBy = null;
                        item.VerifiedDate = null;
                        item.LastUpdatedBy = AppSession.UserLogin.UserID;
                        labResultDtDao.Update(item);
                    }

                    int rowCount = BusinessLayer.GetLaboratoryResultDtRowCount(string.Format("ID = {0} AND IsVerified = 1 AND IsDeleted = 0", hdnLabResultID.Value), ctx);
                    if (rowCount < 1)
                    {
                        LaboratoryResultHd labHd = labResultHdDao.Get(Convert.ToInt32(hdnLabResultID.Value));
                        labHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                        labHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        labResultHdDao.Update(labHd);
                    }

                    ctx.CommitTransaction();

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

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }
    }
}