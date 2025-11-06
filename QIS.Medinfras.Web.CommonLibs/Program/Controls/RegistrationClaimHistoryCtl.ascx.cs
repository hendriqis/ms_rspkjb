using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class RegistrationClaimHistoryCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;

        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                vConsultVisitCasemix entity = BusinessLayer.GetvConsultVisitCasemixList(string.Format("RegistrationID = {0}", param)).FirstOrDefault();
                hdnVisitIDRCHCtl.Value = entity.VisitID.ToString();
                hdnRegistrationIDRCHCtl.Value = entity.RegistrationID.ToString();
                txtRegistrationNo.Text = entity.RegistrationNo;
                txtNoSEP.Text = entity.NoSEP;
                txtPatientInfo.Text = string.Format("({0}) {1}", entity.MedicalNo, entity.PatientName);
                txtChargesClassName.Text = entity.ChargeClassName;
                hdnClassChargesID.Value = entity.ChargeClassID.ToString();
              
                BindGridViewDt(1, true, ref PageCount);
            }
        }

        private void BindGridViewDt(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationIDRCHCtl.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvRegistrationClaimHistoryRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_CTL);
            }

            List<vRegistrationClaimHistory> lstEntity = BusinessLayer.GetvRegistrationClaimHistoryList(filterExpression, Constant.GridViewPageSize.GRID_CTL, pageIndex, "RegistrationID DESC, SortID DESC");
            grdViewDt.DataSource = lstEntity;
            grdViewDt.DataBind();
        }

        protected void grdViewDt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vRegistrationClaimHistory entity = e.Row.DataItem as vRegistrationClaimHistory;

                HtmlGenericControl lblINACBGMaster = (HtmlGenericControl)e.Row.FindControl("lblINACBGMaster");
                HtmlInputText txtGrouperCodeClaim = (HtmlInputText)e.Row.FindControl("txtGrouperCodeClaim");
                HtmlInputText txtGrouperTypeClaim = (HtmlInputText)e.Row.FindControl("txtGrouperTypeClaim");
                HtmlInputText txtRealCostAmount = (HtmlInputText)e.Row.FindControl("txtRealCostAmount");
                HtmlInputText txtCoverageAmount = (HtmlInputText)e.Row.FindControl("txtCoverageAmount");
                HtmlInputText txtOccupiedAmount = (HtmlInputText)e.Row.FindControl("txtOccupiedAmount");
                HtmlInputText txtPatientAmount = (HtmlInputText)e.Row.FindControl("txtPatientAmount");
                HtmlInputText txtDifferenceAmount = (HtmlInputText)e.Row.FindControl("txtDifferenceAmount");
                HtmlInputButton btnSave = e.Row.FindControl("btnSave") as HtmlInputButton;

                if (entity.HistoryID == 0)
                {
                    lblINACBGMaster.Attributes.Remove("style");
                    txtGrouperCodeClaim.Attributes.Add("readonly", "readonly");
                    txtGrouperTypeClaim.Attributes.Add("readonly", "readonly");
                    txtRealCostAmount.Attributes.Add("readonly", "readonly");
                    txtCoverageAmount.Attributes.Remove("readonly");
                    txtOccupiedAmount.Attributes.Remove("readonly");
                    txtPatientAmount.Attributes.Remove("readonly");
                    txtDifferenceAmount.Attributes.Add("readonly", "readonly");
                    btnSave.Style.Remove("display");
                }
                else
                {
                    lblINACBGMaster.Attributes.Add("style", "display:none");
                    txtGrouperCodeClaim.Attributes.Add("readonly", "readonly");
                    txtGrouperTypeClaim.Attributes.Add("readonly", "readonly");
                    txtRealCostAmount.Attributes.Add("readonly", "readonly");
                    txtCoverageAmount.Attributes.Add("readonly", "readonly");
                    txtOccupiedAmount.Attributes.Add("readonly", "readonly");
                    txtPatientAmount.Attributes.Add("readonly", "readonly");
                    txtDifferenceAmount.Attributes.Add("readonly", "readonly");
                    btnSave.Style.Add("display", "none");
                }
            }
        }

        protected void cbpViewDt_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            string errMessage = "";

            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else if (param[0] == "save")
                {
                    if (OnSaveClaim(param, ref errMessage))
                    {
                        result += string.Format("{0}|success", param[0]);
                    }
                    else
                    {
                        result += string.Format("{0}|fail|{1}", param[0], errMessage);
                    }
                }
                else // refresh
                {
                    BindGridViewDt(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnSaveClaim(string[] param, ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationBPJSDao regBPJSDao = new RegistrationBPJSDao(ctx);

            try
            {
                RegistrationBPJS regBPJS = regBPJSDao.Get(Convert.ToInt32(hdnRegistrationIDRCHCtl.Value));
                regBPJS.CodingBy = AppSession.UserLogin.UserID;
                regBPJS.CodingDate = DateTime.Now;
                regBPJS.GrouperCodeClaim = param[2];
                regBPJS.GrouperTypeClaim = param[3];
                regBPJS.RealCostAmount = Convert.ToDecimal(param[4]);
                regBPJS.INAHakPasien = regBPJS.CoverageAmount = regBPJS.GrouperAmountClaim = Convert.ToDecimal(param[5]);
                regBPJS.INADitempati = regBPJS.OccupiedAmount = Convert.ToDecimal(param[6]);
                regBPJS.PatientAmount = Convert.ToDecimal(param[7]);
                regBPJS.DifferenceAmount = Convert.ToDecimal(param[8]);

                regBPJS.LastUpdatedBy = AppSession.UserLogin.UserID;
                regBPJSDao.Update(regBPJS);

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