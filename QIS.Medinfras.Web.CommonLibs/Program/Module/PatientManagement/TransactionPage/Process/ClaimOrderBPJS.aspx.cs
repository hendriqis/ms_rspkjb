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
using DevExpress.Web.ASPxCallbackPanel;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ClaimOrderBPJS : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            switch (hdnRequestID.Value)
            {
                case Constant.Facility.OUTPATIENT: return Constant.MenuCode.Outpatient.BPJS_PROCESS_CLAIM_ORDER_IN_NURSING_MENU;
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.BPJS_PROCESS_CLAIM_ORDER_IN_NURSING_MENU;
                case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.BPJS_PROCESS_CLAIM_ORDER_IN_NURSING_MENU;
                default: return Constant.MenuCode.Outpatient.BPJS_PROCESS_CLAIM_ORDER_IN_NURSING_MENU;
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString["id"] != null)
            {
                hdnRequestID.Value = Page.Request.QueryString["id"];
            }
            else
            {
                hdnRequestID.Value = "ALL";
            }

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnDepartmentID.Value = AppSession.RegisteredPatient.DepartmentID;
            hdnHealthcareServiceUnitID.Value = AppSession.RegisteredPatient.HealthcareServiceUnitID.ToString();

            BindGridDetail();
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

        private void BindGridDetail()
        {
            List<vConsultVisitCasemix> lst = BusinessLayer.GetvConsultVisitCasemixList(string.Format("RegistrationID = {0}", hdnRegistrationID.Value));
            lvwView.DataSource = lst;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vConsultVisitCasemix entity = e.Item.DataItem as vConsultVisitCasemix;

                HtmlGenericControl divDiagnosis = e.Item.FindControl("divDiagnosis") as HtmlGenericControl;
                if (entity.DiagnoseID != null)
                {
                    divDiagnosis.InnerHtml = "O";
                    divDiagnosis.Style.Add("color", "blue");
                }
                else
                {
                    divDiagnosis.InnerHtml = "X";
                    divDiagnosis.Style.Add("color", "red");
                }

                HtmlGenericControl divOrderClaim = e.Item.FindControl("divOrderClaim") as HtmlGenericControl;
                if (entity.IsOrderCoding)
                {
                    divOrderClaim.InnerHtml = "O";
                    divOrderClaim.Style.Add("color", "blue");
                }
                else
                {
                    divOrderClaim.InnerHtml = "X";
                    divOrderClaim.Style.Add("color", "red");
                }
            }
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridDetail();
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            if (type == "claimorder")
            {
                bool result = true;
                IDbContext ctx = DbFactory.Configure(true);
                RegistrationBPJSDao regBPJSDao = new RegistrationBPJSDao(ctx);
                try
                {
                    RegistrationBPJS regBPJS = BusinessLayer.GetRegistrationBPJS(Convert.ToInt32(hdnRegistrationID.Value));
                    if (regBPJS != null)
                    {
                        regBPJS.IsOrderCoding = true;
                        regBPJS.OrderCodingBy = AppSession.UserLogin.UserID;
                        regBPJS.OrderCodingDate = DateTime.Now;
                        regBPJS.LastUpdatedBy = AppSession.UserLogin.UserID;
                        regBPJSDao.Update(regBPJS);

                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Maaf, registrasi ini penjamin bayarnya bukan BPJS.";
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
            else if (type == "cancelclaimorder")
            {
                bool result = true;
                IDbContext ctx = DbFactory.Configure(true);
                RegistrationBPJSDao regBPJSDao = new RegistrationBPJSDao(ctx);
                try
                {
                    RegistrationBPJS regBPJS = BusinessLayer.GetRegistrationBPJS(Convert.ToInt32(hdnRegistrationID.Value));
                    if(regBPJS != null)
                    {
                        regBPJS.IsOrderCoding = false;
                        regBPJS.OrderCodingBy = null;
                        regBPJS.OrderCodingDate = Convert.ToDateTime("01-01-1900");
                        regBPJS.LastUpdatedBy = AppSession.UserLogin.UserID;
                        regBPJSDao.Update(regBPJS);

                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Maaf, registrasi ini penjamin bayarnya bukan BPJS.";
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
            return true;
        }
    }
}