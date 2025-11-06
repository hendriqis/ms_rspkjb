using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Controls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class MPBasePatientPage : BaseMP
    {
        protected string moduleName = "";
        string menuCode = "";

        #region Top Charm Bar Menu
        public class TopCharmBarMenu
        {
            public string Title { get; set; }
            public string Postbackurl { get; set; }
            public string Normalsrc { get; set; }
            public string Hoversrc { get; set; }
            public string Selectedsrc { get; set; }
            public string Pressedsrc { get; set; }
            public string Disabledsrc { get; set; }
        }
        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!Page.IsPostBack)
            {
                if (AppSession.UserLogin == null)
                    Response.Redirect("~/../SystemSetup/Login.aspx");

                vConsultVisit2 entity = BusinessLayer.GetvConsultVisit2List(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID))[0];
                EntityToControl(entity);

                string moduleName = Helper.GetModuleName();
                string ModuleID = Helper.GetModuleID(moduleName);
                string topBarMenuCode = string.Empty;

                switch (ModuleID)
                {
                    case Constant.Module.EMERGENCY:
                        topBarMenuCode = Constant.MenuCode.EmergencyCare.PATIENT_PAGE_CHARM_BAR;
                        break;
                    case Constant.Module.EMR:
                        topBarMenuCode = Constant.MenuCode.EMR.INFORMATION;
                        break;
                    case Constant.Module.FINANCE:
                        topBarMenuCode = Constant.MenuCode.Finance.PATIENT_PAGE_CHARM_BAR;
                        break;
                    case Constant.Module.IMAGING:
                        topBarMenuCode = Constant.MenuCode.Imaging.PATIENT_PAGE_CHARM_BAR;
                        break;
                    case Constant.Module.INPATIENT:
                        topBarMenuCode = Constant.MenuCode.Inpatient.PATIENT_PAGE_CHARM_BAR;
                        break;
                    case Constant.Module.LABORATORY:
                        topBarMenuCode = Constant.MenuCode.Laboratory.PATIENT_PAGE_CHARM_BAR;
                        break;
                    case Constant.Module.MEDICAL_CHECKUP:
                        topBarMenuCode = Constant.MenuCode.MedicalCheckup.PATIENT_PAGE_CHARM_BAR;
                        break;
                    case Constant.Module.MEDICAL_DIAGNOSTIC:
                        topBarMenuCode = Constant.MenuCode.MedicalDiagnostic.PATIENT_PAGE_CHARM_BAR;
                        break;
                    case Constant.Module.RADIOTHERAPHY:
                        topBarMenuCode = Constant.MenuCode.Radiotheraphy.PATIENT_PAGE_CHARM_BAR;
                        break;
                    case Constant.Module.MEDICAL_RECORD:
                        topBarMenuCode = Constant.MenuCode.MedicalRecord.PATIENT_PAGE_CHARM_BAR;
                        break;
                    case Constant.Module.NUTRITION:
                        topBarMenuCode = Constant.MenuCode.Nutrition.PATIENT_PAGE_CHARM_BAR;
                        break;
                    case Constant.Module.OUTPATIENT:
                        topBarMenuCode = Constant.MenuCode.Outpatient.PATIENT_PAGE_CHARM_BAR;
                        break;
                    case Constant.Module.PHARMACY:
                        topBarMenuCode = Constant.MenuCode.Pharmacy.PATIENT_PAGE_CHARM_BAR;
                        break;
                    default:
                        break;
                }

                #region Top Charm Bar
                if (!string.IsNullOrEmpty(topBarMenuCode))
                {
                    List<GetUserMenuAccess> lstMenu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("MenuCode = '{0}' OR ParentCode = '{0}'", topBarMenuCode));
                    GetUserMenuAccess menuInformation = lstMenu.FirstOrDefault(p => p.MenuCode == topBarMenuCode);
                    if (menuInformation != null)
                    {
                        List<GetUserMenuAccess> lstUserMenuCharmBar = lstMenu.Where(p => p.ParentID == menuInformation.MenuID).OrderBy(p => p.MenuIndex).ToList();
                        List<TopCharmBarMenu> ListTopCharmBarMenu = new List<TopCharmBarMenu>();
                        foreach (GetUserMenuAccess menu in lstUserMenuCharmBar)
                        {
                            TopCharmBarMenu topCharmBar = new TopCharmBarMenu();
                            topCharmBar.Title = menu.MenuCaption;
                            topCharmBar.Postbackurl = menu.MenuUrl;
                            topCharmBar.Normalsrc = Page.ResolveUrl(menu.ImageUrl);
                            topCharmBar.Hoversrc = string.Format("{0}_hover.png", topCharmBar.Normalsrc.Substring(0, topCharmBar.Normalsrc.Length - 4));
                            topCharmBar.Selectedsrc = string.Format("{0}_selected.png", topCharmBar.Normalsrc.Substring(0, topCharmBar.Normalsrc.Length - 4));
                            topCharmBar.Pressedsrc = string.Format("{0}_pressed.png", topCharmBar.Normalsrc.Substring(0, topCharmBar.Normalsrc.Length - 4));
                            topCharmBar.Disabledsrc = string.Format("{0}_disabled.png", topCharmBar.Normalsrc.Substring(0, topCharmBar.Normalsrc.Length - 4));
                            ListTopCharmBarMenu.Add(topCharmBar);
                        }
                        rptTopCharmBarMenu.DataSource = ListTopCharmBarMenu;
                        rptTopCharmBarMenu.DataBind();
                    }
                }
                #endregion
            }
        }

        private void EntityToControl(vConsultVisit2 entity)
        {
            ((PatientBannerCtl)ctlPatientBanner).InitializePatientBanner(entity);
        }

        protected string GetModuleImage()
        {
            return Helper.GetModuleImage(this, Helper.GetModuleName());
        }

        protected string GetHospitalName()
        {
            return AppSession.UserLogin.HealthcareName;
        }

        protected string GetUserInfo()
        {
            return string.Format("{0}", AppSession.UserLogin.UserFullName);
        }

        protected void cbpCloseWindow_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            moduleName = Helper.GetModuleName().ToLower();
            if (moduleName != "systemsetup")
                HttpContext.Current.Session.Clear();
        }

        protected void cbpPatientBannerView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            if (e.Parameter != null && e.Parameter != "")
            {
                vConsultVisit2 entity = BusinessLayer.GetvConsultVisit2List(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID))[0];
                string[] param = e.Parameter.Split('|');
                if (param[0] == "allergy")
                {
                    RefreshAllergyInfo(entity);
                }
            }
        }

        private void RefreshAllergyInfo(vConsultVisit2 entity)
        {
            ctlPatientBanner.RefreshAllergyStatus(entity);
        }
    }
}