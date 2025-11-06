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

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class INACBGSToolbarCtl : BaseUserControlCtl
    {
        string menuCode = "";
        List<GetUserMenuAccess> lstMenu = null;
        int healthcareServiceUnitID = 0;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                vConsultVisit2 entity = BusinessLayer.GetvConsultVisit2List(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
                hdnRegistrationID.Value = entity.RegistrationID.ToString();
                hdnDepartmentID.Value = entity.DepartmentID;
                healthcareServiceUnitID = entity.HealthcareServiceUnitID;
                string moduleName = Helper.GetModuleName();
                string ModuleID = Helper.GetModuleID(moduleName);
                string linkedRegistration = entity.LinkedRegistrationID.ToString();
                hdnLinkedRegistrationID.Value = linkedRegistration;
                menuCode = ((BasePageContent)Page).OnGetMenuCode();
                string filterExpression = "";
                if (linkedRegistration != "" && linkedRegistration != "0")
                    filterExpression = string.Format("(VisitID = {0} OR (RegistrationID = {1} AND IsChargesTransfered = 1))", AppSession.RegisteredPatient.VisitID, linkedRegistration);
                else
                    filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);
                string parentCode = Constant.MenuCode.MedicalRecord.INACBGS;/*
                switch (ModuleID)
                {
                    case "OP": parentCode = Constant.MenuCode.Outpatient.BILL_SUMMARY; break;
                    case "IP": parentCode = Constant.MenuCode.Inpatient.BILL_SUMMARY; break;
                    case "ER": parentCode = Constant.MenuCode.EmergencyCare.BILL_SUMMARY; break;
                    case "LB": parentCode = Constant.MenuCode.Laboratory.BILL_SUMMARY; break;
                    case "IS": parentCode = Constant.MenuCode.Imaging.BILL_SUMMARY; break;
                    case "MD": parentCode = Constant.MenuCode.MedicalDiagnostic.BILL_SUMMARY; break;
                    case "PH": parentCode = Constant.MenuCode.Pharmacy.BILL_SUMMARY; break;
                    case "MC": parentCode = Constant.MenuCode.MedicalCheckup.BILL_SUMMARY; break;
                }*/

                lstMenu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("(ParentCode = '{0}' OR ParentID IN (SELECT MenuID FROM Menu WHERE ParentID = (SELECT MenuID FROM Menu WHERE MenuCode = '{0}')))", parentCode));
                rptHeader.DataSource = lstMenu.Where(p => p.ParentCode == parentCode).OrderBy(p => p.MenuIndex).ToList();
                rptHeader.DataBind();
                GetUserMenuAccess selectedMenu = lstMenu.FirstOrDefault(p => p.MenuCode == menuCode);
                rptMenuChild.DataSource = lstMenu.Where(p => p.ParentID == selectedMenu.ParentID).OrderBy(p => p.MenuIndex).ToList();
                rptMenuChild.DataBind();
                List<vPatientVisitNote> lstEntity = BusinessLayer.GetvPatientVisitNoteList(string.Format("VisitID = {0} AND IsDeleted = 0", entity.VisitID));
                if (lstEntity.Count == 0)
                {
                    divVisitNote.Attributes.Add("style", "display:none");
                }
                //TogglePatientNotification(entity);
            }
        }

        private void TogglePatientNotification(vConsultVisit2 entity)
        {
            
        }

        protected void rptHeader_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
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

        protected void rptMenuChild_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                GetUserMenuAccess obj = (GetUserMenuAccess)e.Item.DataItem;
                HtmlGenericControl liCaption = (HtmlGenericControl)e.Item.FindControl("liCaption");
                if (obj.MenuCode == menuCode) liCaption.Attributes.Add("class", "selected");
                HttpRequest temp = HttpContext.Current.Request;
            }
        }
    }
}