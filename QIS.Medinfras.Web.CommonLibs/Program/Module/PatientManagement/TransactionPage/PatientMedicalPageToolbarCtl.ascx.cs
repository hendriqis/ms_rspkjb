using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using System.Web.UI.HtmlControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientMedicalPageToolbarCtl : BaseUserControlCtl
    {
        string menuCode = "";
        List<GetUserMenuAccess> lstMenu = null;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                string moduleName = Helper.GetModuleName();
                string ModuleID = Helper.GetModuleID(moduleName);
                hdnModuleID.Value = ModuleID;
                menuCode = ((BasePageContent)Page).OnGetMenuCode();
                
                string parentCode = "";
                switch (ModuleID)
                {
                    case "IP": parentCode = Constant.MenuCode.Inpatient.FOLLOWUP_PATIENT_DISCHARGE; break;
                }
                lstMenu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("(ParentCode = '{0}' OR ParentID IN (SELECT MenuID FROM Menu WHERE ParentID = (SELECT MenuID FROM Menu WHERE MenuCode = '{0}')))", parentCode));
                if (lstMenu.Count > 0) hdnParentCode.Value = parentCode;
                rptHeader.DataSource = lstMenu.Where(p => p.ParentCode == parentCode).OrderBy(p => p.MenuIndex).ToList();
                rptHeader.DataBind();

                GetUserMenuAccess selectedMenu = lstMenu.FirstOrDefault(p => p.MenuCode == menuCode);
                rptMenuChild.DataSource = lstMenu.Where(p => p.ParentID == selectedMenu.ParentID).OrderBy(p => p.MenuIndex).ToList();
                rptMenuChild.DataBind();

                vConsultVisit entity = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
                hdnRegistrationID.Value = entity.RegistrationID.ToString();
                hdnVisitID.Value = entity.VisitID.ToString();
                hdnDepartmentID.Value = entity.DepartmentID;
            }
        }

        protected void rptHeader_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                GetUserMenuAccess obj = (GetUserMenuAccess)e.Item.DataItem;
                HtmlGenericControl liCaption = (HtmlGenericControl)e.Item.FindControl("liCaption");
                IEnumerable<GetUserMenuAccess> mn = lstMenu.Where(p => p.ParentID == obj.MenuID && p.MenuCode == menuCode);
                if (mn.Count() > 0)
                {
                    liCaption.Attributes.Add("class", "selected");
                }
                List<GetUserMenuAccess> lstMn = lstMenu.Where(p => p.ParentID == obj.MenuID).OrderBy(p => p.MenuIndex).ToList();
                if (lstMn.Count > 0)
                    liCaption.Attributes.Add("url", lstMn[0].MenuUrl);
                else
                    liCaption.Visible = false;
            }
        }

        protected void cbpStartStopServiceTime_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            if (OnBeforeProcessRecord(ref errMessage))
            {
                if (OnProcessRecord(ref errMessage))
                    result = "success";
                else
                    result = "fail|" + errMessage;
            }
            else
                result = "fail|" + errMessage;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnBeforeProcessRecord(ref string errMessage)
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private bool OnProcessRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ConsultVisitDao consultVisitDao = new ConsultVisitDao(ctx);
            try
            {
                int visitID = Convert.ToInt32(hdnVisitID.Value);
                ConsultVisit oConsultVisit = BusinessLayer.GetConsultVisit(visitID);
                if ((oConsultVisit.GCVisitStatus == Constant.VisitStatus.OPEN) || (oConsultVisit.GCVisitStatus == Constant.VisitStatus.CHECKED_IN))
                {
                    oConsultVisit.GCVisitStatus = Constant.VisitStatus.RECEIVING_TREATMENT;
                    oConsultVisit.StartServiceDate = DateTime.Now.Date;
                    oConsultVisit.StartServiceTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                }
                else
                {
                    oConsultVisit.GCVisitStatus = Constant.VisitStatus.PHYSICIAN_DISCHARGE;
                    //oConsultVisit.StartServiceDate = DateTime.Now.Date;
                    //oConsultVisit.StartServiceTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT); 
                }
                oConsultVisit.LastUpdatedBy = AppSession.UserLogin.UserID;
                consultVisitDao.Update(oConsultVisit);
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                result = false;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        protected void rptMenuChild_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                GetUserMenuAccess obj = (GetUserMenuAccess)e.Item.DataItem;
                HtmlGenericControl liCaption = (HtmlGenericControl)e.Item.FindControl("liCaption");
                if (obj.MenuCode == menuCode) liCaption.Attributes.Add("class", "selected");
            }
        }
    }
}