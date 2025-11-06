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
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;
using System.Data;
using QIS.Medinfras.Web.CommonLibs.Program;

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
                String transactionID = Page.Request.QueryString["id"];
                hdnTransactionHdID.Value = transactionID;
                vPatientChargesHd entityPatient = BusinessLayer.GetvPatientChargesHdList(string.Format("TransactionID = {0}", hdnTransactionHdID.Value))[0];
                vConsultVisit entity = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", entityPatient.VisitID))[0];
                hdnLabResultID.Value = BusinessLayer.GetLaboratoryResultHdList(string.Format("ChargeTransactionID = {0}", hdnTransactionHdID.Value))[0].ID.ToString();
                List<vLaboratoryResultDt> entityLabDt = BusinessLayer.GetvLaboratoryResultDtList(string.Format("ID = {0} AND IsVerified = 1 AND IsDeleted = 0",hdnLabResultID.Value));
                foreach (vLaboratoryResultDt labDt in entityLabDt)
                {
                    lstMember += labDt.CustomID;
                    lstMember += ",";
                }
                hdnSelectedMember.Value = lstMember;
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
            string filterExpression = "";

            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("ID = '{0}'", hdnLabResultID.Value);
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
            if (type == "process")
            {
                bool result = true;
                IDbContext ctx = DbFactory.Configure(true);
                LaboratoryResultHdDao labResultHdDao = new LaboratoryResultHdDao(ctx);
                LaboratoryResultDtDao labResultDtDao = new LaboratoryResultDtDao(ctx);
                try
                {
                    string filterExpression = String.Format("ID = {0}", hdnLabResultID.Value);
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
                                labDt.LastUpdatedDate = DateTime.Now;
                                labResultDtDao.Update(labDt);
                            }
                        }
                        else
                        {
                            if (labDt.IsVerified)
                            {
                                labDt.IsVerified = false;
                                labDt.VerifiedBy = null;
                                labDt.VerifiedDate = new DateTime(1900, 1, 1);
                                labDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                labResultDtDao.Update(labDt);
                            }
                        }
                    }
                    LaboratoryResultHd labHd = labResultHdDao.Get(Convert.ToInt32(hdnLabResultID.Value));
                    labHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                    labHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    labResultHdDao.Update(labHd);
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
            else
            {
                try
                {
                    LaboratoryResultHd entity = BusinessLayer.GetLaboratoryResultHd(Convert.ToInt32(hdnLabResultID.Value));
                    entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateLaboratoryResultHd(entity);
                    return true;
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    return false;
                }
            }
        }
    }
}