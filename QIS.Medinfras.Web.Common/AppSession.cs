using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using QIS.Medinfras.Data.Service;
using QISEncryption;

namespace QIS.Medinfras.Web.Common
{
    public class AppSession
    {
        public static UserLogin UserLogin
        {
            get
            {
                HttpContext context = HttpContext.Current;
                if (context.Session != null)
                {
                    if (context.Session["_UserName"] == null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                        {
                            if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_UserName"] != null)
                            {
                                string[] temp = Encryption.DecryptString(HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_UserName"]).Split('|');
                                string userID = temp[0];
                                string healthcareID = temp[1];
                                vUser user = BusinessLayer.GetvUserList(string.Format("UserID = {0} AND IsDeleted = 0", userID)).FirstOrDefault();
                                if (user == null)
                                    return null;
                                UserLogin userLogin = new UserLogin();
                                userLogin.HealthcareID = healthcareID;
                                userLogin.UserID = user.UserID;
                                userLogin.UserName = user.UserName;
                                userLogin.ParamedicID = user.ParamedicID;
                                userLogin.ParamedicName = user.ParamedicName;
                                userLogin.SSN = user.SSN;
                                if (userLogin.ParamedicID != null && userLogin.ParamedicID > 0)
                                {
                                    vParamedicMaster userParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("ParamedicID = {0}", userLogin.ParamedicID))[0];
                                    userLogin.UserFullName = userParamedic.ParamedicName;
                                    userLogin.DepartmentID = userParamedic.DepartmentID;
                                    userLogin.SpecialtyID = userParamedic.SpecialtyID;
                                    userLogin.GCParamedicMasterType = userParamedic.GCParamedicMasterType;
                                    userLogin.IsSpecialist = userParamedic.IsSpecialist;
                                    userLogin.IsPrimaryNurse = userParamedic.IsPrimaryNurse;
                                }
                                else
                                {
                                    userLogin.UserFullName = user.FullName;
                                }

                                List<UserInRole> lstUserSysAdmin = BusinessLayer.GetUserInRoleList(string.Format("UserID = {0} AND HealthcareID = '{1}' AND RoleID = 1", userLogin.UserID, userLogin.HealthcareID));
                                userLogin.IsSysAdmin = (lstUserSysAdmin.Count > 0);
                                if (healthcareID != "")
                                    userLogin.HealthcareName = BusinessLayer.GetHealthcare(healthcareID).HealthcareName;

                                HttpContext.Current.Session["_UserName"] = userLogin;
                                return userLogin;
                            }
                        }
                        return null;
                    }
                    return ((UserLogin)(HttpContext.Current.Session["_UserName"]));
                }
                else
                {
                    string userID = "0";
                    string healthcareID = "001";
                    vUser user = BusinessLayer.GetvUserList(string.Format("UserID = {0} AND IsDeleted = 0", userID)).FirstOrDefault();
                    if (user == null)
                        return null;
                    UserLogin userLogin = new UserLogin();
                    userLogin.HealthcareID = healthcareID;
                    userLogin.UserID = user.UserID;
                    userLogin.UserName = user.UserName;
                    userLogin.ParamedicID = user.ParamedicID;
                    userLogin.ParamedicName = user.ParamedicName;
                    userLogin.SSN = user.SSN;
                    if (userLogin.ParamedicID != null && userLogin.ParamedicID > 0)
                    {
                        vParamedicMaster userParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("ParamedicID = {0}", userLogin.ParamedicID))[0];
                        userLogin.UserFullName = userParamedic.ParamedicName;
                        userLogin.DepartmentID = userParamedic.DepartmentID;
                        userLogin.SpecialtyID = userParamedic.SpecialtyID;
                        userLogin.GCParamedicMasterType = userParamedic.GCParamedicMasterType;
                        userLogin.IsSpecialist = userParamedic.IsSpecialist;
                        userLogin.IsPrimaryNurse = userParamedic.IsPrimaryNurse;
                    }
                    else
                    {
                        userLogin.UserFullName = user.FullName;
                    }

                    List<UserInRole> lstUserSysAdmin = BusinessLayer.GetUserInRoleList(string.Format("UserID = {0} AND HealthcareID = '{1}' AND RoleID = 1", userLogin.UserID, userLogin.HealthcareID));
                    userLogin.IsSysAdmin = (lstUserSysAdmin.Count > 0);
                    if (healthcareID != "")
                        userLogin.HealthcareName = BusinessLayer.GetHealthcare(healthcareID).HealthcareName;

                    HttpContext.Current.Session["_UserName"] = userLogin;
                    return userLogin;
                }
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] == null || HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_UserName"] == null)
                {
                    HttpCookie userLoginCookie = new HttpCookie(Constant.SessionName.COOKIES_NAME);
                    userLoginCookie["_UserName"] = Encryption.EncryptString(string.Format("{0}|{1}", value.UserID, value.HealthcareID));
                    userLoginCookie.Expires = DateTime.Now.AddDays(1d);
                    HttpContext.Current.Response.Cookies.Add(userLoginCookie);
                }
                else
                {
                    HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_UserName"] = Encryption.EncryptString(string.Format("{0}|{1}", value.UserID, value.HealthcareID));
                    HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME].Expires = DateTime.Now.AddDays(1d);
                }
                HttpContext.Current.Session["_UserName"] = value;
            }
        }

        public static Boolean IsCashier
        {

            get
            {
                if (HttpContext.Current.Session["_IsCashier"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsCashier"] != null)
                        {
                            Boolean value = Convert.ToBoolean( HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsCashier"]);
                            HttpContext.Current.Session["_IsCashier"] = value;
                            return value;
                        }
                    }
                    return false;
                }

                return ((Boolean)(HttpContext.Current.Session["_IsCashier"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsCashier"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsCashier"] = value;
            }
            //get
            //{
            //    if (HttpContext.Current.Session["_IsCashier"] == null)
            //    {
            //        return false;
            //    }
            //    return ((Boolean)(HttpContext.Current.Session["_IsCashier"]));
            //}
            //set
            //{
            //    HttpContext.Current.Session["_IsCashier"] = value;
            //}
        }

        public static String CashierGroup
        {
            get
            {
                if (HttpContext.Current.Session["_CashierGroup"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_CashierGroup"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_CashierGroup"];
                            HttpContext.Current.Session["_CashierGroup"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_CashierGroup"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_CashierGroup"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_CashierGroup"] = value;
            }

            //get
            //{
            //    if (HttpContext.Current.Session["_CashierGroup"] == null)
            //    {
            //        return "";
            //    }
            //    return ((String)(HttpContext.Current.Session["_CashierGroup"]));
            //}
            //set
            //{
            //    HttpContext.Current.Session["_CashierGroup"] = value;
            //}
        }

        public static IPDefaultConfig UserConfig
        {
            get
            {
                if (HttpContext.Current.Session["_UserConfig"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_UserName"] != null)
                        {
                            string ipAddress = HttpContext.Current.Request.UserHostAddress;
                            string filterExp = string.Format("IPAddress = '{0}' AND IsDeleted=0", ipAddress);
                            IPDefaultConfig oConfig = BusinessLayer.GetIPDefaultConfigList(filterExp).FirstOrDefault();

                            IPDefaultConfig userConfig = new IPDefaultConfig();
                            if (oConfig != null)
                            {
                                userConfig.IPAddress = ipAddress;
                                if (!string.IsNullOrEmpty(oConfig.DepartmentID))
                                    userConfig.DepartmentID = oConfig.DepartmentID;
                                else
                                    userConfig.DepartmentID = null;

                                if (oConfig.HealthcareServiceUnitID != null)
                                    userConfig.HealthcareServiceUnitID = oConfig.HealthcareServiceUnitID;
                                else
                                    userConfig.HealthcareServiceUnitID = 0;

                                if (oConfig.LocationID != null)
                                    userConfig.LocationID = oConfig.LocationID;
                                else
                                    userConfig.LocationID = 0;

                                if (string.IsNullOrEmpty(oConfig.GCCashierGroup))
                                    userConfig.GCCashierGroup = oConfig.GCCashierGroup;
                                else
                                    userConfig.GCCashierGroup = null;
                            }
                            else
                            {
                                userConfig.IPAddress = ipAddress;
                                userConfig.DepartmentID = string.Empty;
                                userConfig.HealthcareServiceUnitID = Convert.ToInt32(0);
                                userConfig.LocationID = Convert.ToInt32(0);
                                userConfig.GCCashierGroup = null;
                            }

                            HttpContext.Current.Session["_UserConfig"] = userConfig;
                            return userConfig;
                        }
                    }
                    return null;
                }
                return ((IPDefaultConfig)(HttpContext.Current.Session["_UserConfig"]));
            }
            set
            {
                HttpContext.Current.Session["_UserConfig"] = value;
            }
        }

        public static void ClearSession()
        {
            if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
            {
                HttpCookie myCookie = new HttpCookie(Constant.SessionName.COOKIES_NAME);
                myCookie.Expires = DateTime.Now.AddDays(-1d);
                HttpContext.Current.Response.Cookies.Add(myCookie);
            }

            HttpContext.Current.Session.Clear();
        }

        public static String UrlReferrer
        {
            get
            {
                if (HttpContext.Current.Session["_UrlReferrer"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_UrlReferrer"] != null)
                        {
                            String UrlReferrer = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_UrlReferrer"];
                            HttpContext.Current.Session["_UrlReferrer"] = UrlReferrer;
                            return UrlReferrer;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_UrlReferrer"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_UrlReferrer"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_UrlReferrer"] = value;
            }
        }

        public static LastContentFarmasiKlinis LastContentFarmasiKlinis
        {
            get
            {
                if (HttpContext.Current.Session["_LastContentFarmasiKlinis"] == null)
                {
                    return null;
                }
                return ((LastContentFarmasiKlinis)(HttpContext.Current.Session["_LastContentFarmasiKlinis"]));
            }
            set
            {
                HttpContext.Current.Session["_LastContentFarmasiKlinis"] = value;
            }
        }

        public static LastContentPrescriptionEntry LastContentPrescriptionEntry
        {
            get
            {
                if (HttpContext.Current.Session["_LastContentPrescriptionEntry"] == null)
                {
                    return null;
                }
                return ((LastContentPrescriptionEntry)(HttpContext.Current.Session["_LastContentPrescriptionEntry"]));
            }
            set
            {
                HttpContext.Current.Session["_LastContentPrescriptionEntry"] = value;
            }
        }

        public static LastContentPrescriptionEntry LastContentUDDList
        {
            get
            {
                if (HttpContext.Current.Session["_LastContentUDDList"] == null)
                {
                    return null;
                }
                return ((LastContentPrescriptionEntry)(HttpContext.Current.Session["_LastContentUDDList"]));
            }
            set
            {
                HttpContext.Current.Session["_LastContentUDDList"] = value;
            }
        }

        public static LastContentVisitListPH LastContentVisitListPH
        {
            get
            {
                if (HttpContext.Current.Session["_LastContentVisitListPH"] == null)
                {
                    return null;
                }
                return ((LastContentVisitListPH)(HttpContext.Current.Session["_LastContentVisitListPH"]));
            }
            set
            {
                HttpContext.Current.Session["_LastContentVisitListPH"] = value;
            }
        }

        public static LastContentVisitListOP LastContentVisitListOP
        {
            get
            {
                if (HttpContext.Current.Session["_LastContentVisitListOP"] == null)
                {
                    return null;
                }
                return ((LastContentVisitListOP)(HttpContext.Current.Session["_LastContentVisitListOP"]));
            }
            set
            {
                HttpContext.Current.Session["_LastContentVisitListOP"] = value;
            }
        }

        public static LastContentVisitListER LastContentVisitListER
        {
            get
            {
                if (HttpContext.Current.Session["_LastContentVisitListER"] == null)
                {
                    return null;
                }
                return ((LastContentVisitListER)(HttpContext.Current.Session["_LastContentVisitListER"]));
            }
            set
            {
                HttpContext.Current.Session["_LastContentVisitListER"] = value;
            }
        }

        public static LastContentVisitListIP LastContentVisitListIP
        {
            get
            {
                if (HttpContext.Current.Session["_LastContentVisitListIP"] == null)
                {
                    return null;
                }
                return ((LastContentVisitListIP)(HttpContext.Current.Session["_LastContentVisitListIP"]));
            }
            set
            {
                HttpContext.Current.Session["_LastContentVisitListIP"] = value;
            }
        }

        public static LastContentVisitListMD LastContentVisitListMD
        {
            get
            {
                if (HttpContext.Current.Session["_LastContentVisitListMD"] == null)
                {
                    return null;
                }
                return ((LastContentVisitListMD)(HttpContext.Current.Session["_LastContentVisitListMD"]));
            }
            set
            {
                HttpContext.Current.Session["_LastContentVisitListMD"] = value;
            }
        }

        public static LastContentFollowupPatientVisitList LastContentFollowupPatientVisitList //untuk follow-up pasien pulang
        {
            get
            {
                if (HttpContext.Current.Session["_LastContentFollowupPatientVisitList"] == null)
                {
                    return null;
                }
                return ((LastContentFollowupPatientVisitList)(HttpContext.Current.Session["_LastContentFollowupPatientVisitList"]));
            }
            set
            {
                HttpContext.Current.Session["_LastContentFollowupPatientVisitList"] = value;
            }
        }

        public static LastContentVisitListMR LastContentVisitListMR
        {
            get
            {
                if (HttpContext.Current.Session["_LastContentVisitListMR"] == null)
                {
                    return null;
                }
                return ((LastContentVisitListMR)(HttpContext.Current.Session["_LastContentVisitListMR"]));
            }
            set
            {
                HttpContext.Current.Session["_LastContentVisitListMR"] = value;
            }
        }

        public static LastContentVisitListMR2 LastContentVisitListMR2
        {
            get
            {
                if (HttpContext.Current.Session["LastContentVisitListMR2"] == null)
                {
                    return null;
                }
                return ((LastContentVisitListMR2)(HttpContext.Current.Session["LastContentVisitListMR2"]));
            }
            set
            {
                HttpContext.Current.Session["LastContentVisitListMR2"] = value;
            }
        }

        public static LastPagingMR1 LastPagingMR1
        {
            get
            {
                if (HttpContext.Current.Session["LastPagingMR1"] == null)
                {
                    return null;
                }
                return ((LastPagingMR1)(HttpContext.Current.Session["LastPagingMR1"]));
            }
            set
            {
                HttpContext.Current.Session["LastPagingMR1"] = value;
            }
        }

        public static LastPagingMR2 LastPagingMR2
        {
            get
            {
                if (HttpContext.Current.Session["LastPagingMR2"] == null)
                {
                    return null;
                }
                return ((LastPagingMR2)(HttpContext.Current.Session["LastPagingMR2"]));
            }
            set
            {
                HttpContext.Current.Session["LastPagingMR2"] = value;
            }
        }

        public static LastPagingMR3 LastPagingMR3
        {
            get
            {
                if (HttpContext.Current.Session["LastPagingMR3"] == null)
                {
                    return null;
                }
                return ((LastPagingMR3)(HttpContext.Current.Session["LastPagingMR3"]));
            }
            set
            {
                HttpContext.Current.Session["LastPagingMR3"] = value;
            }
        }

        public static LastContentLaboratoryRealization LastContentLaboratoryRealization
        {
            get
            {
                if (HttpContext.Current.Session["_LastContentLaboratoryRealization"] == null)
                {
                    return null;
                }
                return ((LastContentLaboratoryRealization)(HttpContext.Current.Session["_LastContentLaboratoryRealization"]));
            }
            set
            {
                HttpContext.Current.Session["_LastContentLaboratoryRealization"] = value;
            }
        }

        public static LastContentImagingRealization LastContentImagingRealization
        {
            get
            {
                if (HttpContext.Current.Session["_LastContentImagingRealization"] == null)
                {
                    return null;
                }
                return ((LastContentImagingRealization)(HttpContext.Current.Session["_LastContentImagingRealization"]));
            }
            set
            {
                HttpContext.Current.Session["_LastContentImagingRealization"] = value;
            }
        }

        public static LastContentImagingRealizationResult LastContentImagingRealizationResult
        {
            get
            {
                if (HttpContext.Current.Session["_LastContentImagingRealizationResult"] == null)
                {
                    return null;
                }
                return ((LastContentImagingRealizationResult)(HttpContext.Current.Session["_LastContentImagingRealizationResult"]));
            }
            set
            {
                HttpContext.Current.Session["_LastContentImagingRealizationResult"] = value;
            }
        }

        public static LastContentEMRPatientListTrx LastContentEMRPatientListTrx
        {
            get
            {
                if (HttpContext.Current.Session["_LastContentEMRPatientListTrx"] == null)
                {
                    return null;
                }
                return ((LastContentEMRPatientListTrx)(HttpContext.Current.Session["_LastContentEMRPatientListTrx"]));
            }
            set
            {
                HttpContext.Current.Session["_LastContentEMRPatientListTrx"] = value;
            }
        }

        public static LastContentPreviousPatientList LastContentPreviousPatientList
        {
            get
            {
                if (HttpContext.Current.Session["_LastContentPreviousPatientList"] == null)
                {
                    return null;
                }
                return ((LastContentPreviousPatientList)(HttpContext.Current.Session["_LastContentPreviousPatientList"]));
            }
            set
            {
                HttpContext.Current.Session["_LastContentVisitListOP"] = value;
            }
        }
        public static LastPatientVisitMCUForm LastPatientVisitMCUForm {
            get
            {
                if (HttpContext.Current.Session["_LastPatientVisitMCUForm"] == null)
                {
                    return null;
                }
                return ((LastPatientVisitMCUForm)(HttpContext.Current.Session["_LastPatientVisitMCUForm"]));
            }
            set
            {
                HttpContext.Current.Session["_LastPatientVisitMCUForm"] = value;
            }
        }
        public static LastContentEKlaimList LastContentEKlaimList
        {
            get
            {
                if (HttpContext.Current.Session["_LastContentEKlaimList"] == null)
                {
                    return null;
                }
                return ((LastContentEKlaimList)(HttpContext.Current.Session["_LastContentEKlaimList"]));
            }
            set
            {
                HttpContext.Current.Session["_LastContentEKlaimList"] = value;
            }
        }
        public static LastContentClaimDiagnoseProcedure LastContentClaimDiagnoseProcedure
        {
            get
            {
                if (HttpContext.Current.Session["_LastContentClaimDiagnoseProcedure"] == null)
                {
                    return null;
                }
                return ((LastContentClaimDiagnoseProcedure)(HttpContext.Current.Session["_LastContentClaimDiagnoseProcedure"]));
            }
            set
            {
                HttpContext.Current.Session["_LastContentClaimDiagnoseProcedure"] = value;
            }
        }
        public static LastContentClaimDiagnoseProcedureList LastContentClaimDiagnoseProcedureList
        {
            get
            {
                if (HttpContext.Current.Session["_LastContentClaimDiagnoseProcedureList"] == null)
                {
                    return null;
                }
                return ((LastContentClaimDiagnoseProcedureList)(HttpContext.Current.Session["_LastContentClaimDiagnoseProcedureList"]));
            }
            set
            {
                HttpContext.Current.Session["_LastContentClaimDiagnoseProcedureList"] = value;
            }
        }
        public static LastContentClaimDiagnoseProcedureListHaveDiag LastContentClaimDiagnoseProcedureListHaveDiag
        {
            get
            {
                if (HttpContext.Current.Session["_LastContentClaimDiagnoseProcedureListHaveDiag"] == null)
                {
                    return null;
                }
                return ((LastContentClaimDiagnoseProcedureListHaveDiag)(HttpContext.Current.Session["_LastContentClaimDiagnoseProcedureListHaveDiag"]));
            }
            set
            {
                HttpContext.Current.Session["_LastContentClaimDiagnoseProcedureListHaveDiag"] = value;
            }
        }
        public static bool IsAdminCanCancelAllTransaction
        {
            get
            {
                if (HttpContext.Current.Session["_IsAdminCanCancelAllTransaction"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsAdminCanCancelAllTransaction"] != null)
                        {
                            HttpContext.Current.Session["_IsAdminCanCancelAllTransaction"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsAdminCanCancelAllTransaction"];
                            return Convert.ToBoolean(HttpContext.Current.Request.Cookies["_IsAdminCanCancelAllTransaction"]);
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
                return Convert.ToBoolean(HttpContext.Current.Session["_IsAdminCanCancelAllTransaction"]);
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsAdminCanCancelAllTransaction"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsAdminCanCancelAllTransaction"] = value;
            }
        }

        public static MedicalDiagnosticUserLogin MedicalDiagnostic
        {
            get
            {
                if (HttpContext.Current.Session["_MedicalDiagnosticUserLogin"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_MedicalDiagnosticUserLogin"] != null)
                        {
                            string[] temp = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_MedicalDiagnosticUserLogin"].Split('|');
                            MedicalDiagnosticUserLogin entity = new MedicalDiagnosticUserLogin();
                            entity.HealthcareServiceUnitID = Convert.ToInt32(temp[0]);
                            entity.ImagingHealthcareServiceUnitID = Convert.ToInt32(temp[1]);
                            entity.LaboratoryHealthcareServiceUnitID = Convert.ToInt32(temp[2]);
                            if (temp[3] == "1")
                                entity.MedicalDiagnosticType = MedicalDiagnosticType.Imaging;
                            else if (temp[3] == "2")
                                entity.MedicalDiagnosticType = MedicalDiagnosticType.Laboratory;
                            else
                                entity.MedicalDiagnosticType = MedicalDiagnosticType.OtherMedicalDiagnostic;
                            HttpContext.Current.Session["_MedicalDiagnosticUserLogin"] = entity;
                            return entity;
                        }
                    }
                    return null;
                }
                return ((MedicalDiagnosticUserLogin)(HttpContext.Current.Session["_MedicalDiagnosticUserLogin"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    string type = "0";
                    if (value.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        type = "1";
                    else if (value.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        type = "2";
                    myCookie.Values["_MedicalDiagnosticUserLogin"] = string.Format("{0}|{1}|{2}|{3}", value.HealthcareServiceUnitID, value.ImagingHealthcareServiceUnitID, value.LaboratoryHealthcareServiceUnitID, type);
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }

                HttpContext.Current.Session["_MedicalDiagnosticUserLogin"] = value;
            }
        }

        public static RegisteredPatient RegisteredPatient
        {
            get
            {
                if (HttpContext.Current.Session["_RegisteredPatient"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_RegisteredPatient"] != null)
                        {
                            string[] param = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_RegisteredPatient"].Split('|');
                            var visitID = param[0];

                            vConsultVisit4 entity = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", visitID)).FirstOrDefault();
                            RegisteredPatient pt = new RegisteredPatient();
                            pt.MRN = entity.MRN;
                            pt.MedicalNo = entity.MedicalNo;
                            pt.PatientName = entity.PatientName;
                            pt.GCGender = entity.GCGender;
                            pt.GCSex = entity.GCSex;
                            pt.DateOfBirth = entity.DateOfBirth;
                            pt.RegistrationID = entity.RegistrationID;
                            pt.RegistrationNo = entity.RegistrationNo;
                            pt.RegistrationDate = entity.RegistrationDate;
                            pt.RegistrationTime = entity.RegistrationTime;
                            pt.VisitID = entity.VisitID;
                            pt.VisitDate = entity.VisitDate;
                            pt.VisitTime = entity.VisitTime;
                            pt.StartServiceDate = entity.StartServiceDate;
                            pt.StartServiceTime = entity.StartServiceTime;
                            pt.DischargeDate = entity.DischargeDate;
                            pt.DischargeTime = entity.DischargeTime;
                            pt.GCCustomerType = entity.GCCustomerType;
                            pt.BusinessPartnerID = entity.BusinessPartnerID;
                            pt.ParamedicID = entity.ParamedicID;
                            pt.ParamedicCode = entity.ParamedicCode;
                            pt.ParamedicName = entity.ParamedicName;
                            pt.SpecialtyID = entity.SpecialtyID;
                            pt.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
                            pt.DepartmentID = entity.DepartmentID;
                            pt.ServiceUnitName = entity.ServiceUnitName;
                            pt.RoomCode = entity.RoomCode;
                            pt.BedCode = entity.BedCode;
                            pt.DepartmentID = entity.DepartmentID;
                            pt.ChargeClassID = entity.ChargeClassID;
                            pt.ClassID = entity.ClassID;
                            pt.GCRegistrationStatus = entity.GCVisitStatus;
                            pt.IsLockDown = entity.IsLockDown;
                            pt.IsBillingReopen = entity.IsBillingReopen;
                            pt.LinkedRegistrationID = entity.LinkedRegistrationID;
                            pt.LinkedToRegistrationID = entity.LinkedToRegistrationID;

                            if (param.Length > 1)
                            {
                                pt.OpenFromModuleID = param[1];
                            }

                            HttpContext.Current.Session["_RegisteredPatient"] = pt;
                            return pt;
                        }
                        return null;
                    }
                    //return null;
                }

                return ((RegisteredPatient)(HttpContext.Current.Session["_RegisteredPatient"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    string id = string.Format("{0}|{1}", value.VisitID, value.OpenFromModuleID);
                    myCookie.Values["_RegisteredPatient"] = id;
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_RegisteredPatient"] = value;
            }
        }

        public static LastContentPreviousPatientList2 LastPreviousPatientList
        {
            get
            {
                if (HttpContext.Current.Session["_LastPreviousPatientList"] == null)
                {
                    return null;
                }
                return ((LastContentPreviousPatientList2)(HttpContext.Current.Session["_LastPreviousPatientList"]));
            }
            set
            {
                HttpContext.Current.Session["_LastPreviousPatientList"] = value;
            }
        }


        public static PatientDetail PatientDetail
        {
            get
            {
                if (HttpContext.Current.Session["_PatientDetail"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_PatientDetail"] != null)
                        {
                            string[] temp = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_PatientDetail"].Split('|');
                            PatientDetail entity = new PatientDetail();
                            entity.MRN = Convert.ToInt32(temp[0]);
                            entity.MedicalNo = temp[1];
                            HttpContext.Current.Session["_PatientDetail"] = entity;
                            return entity;
                        }
                    }
                    return null;
                }

                return ((PatientDetail)(HttpContext.Current.Session["_PatientDetail"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_PatientDetail"] = string.Format("{0}|{1}", value.MRN, value.MedicalNo);
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_PatientDetail"] = value;
            }
        }

        public static Int32 ParamedicID
        {
            get
            {
                if (HttpContext.Current.Session["_ParamedicID"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_ParamedicID"] != null)
                        {
                            int paramedicID = Convert.ToInt32(HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_ParamedicID"]);
                            HttpContext.Current.Session["_ParamedicID"] = paramedicID;
                            return paramedicID;
                        }
                    }
                    return 0;
                }

                return ((Int32)(HttpContext.Current.Session["_ParamedicID"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_ParamedicID"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_ParamedicID"] = value;
            }
        }

        public static Int32 BusinessPartnerID
        {
            get
            {
                if (HttpContext.Current.Session["_BusinessPartnerID"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_BusinessPartnerID"] != null)
                        {
                            int businessPartnerID = Convert.ToInt32(HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_BusinessPartnerID"]);
                            HttpContext.Current.Session["_BusinessPartnerID"] = businessPartnerID;
                            return businessPartnerID;
                        }
                    }
                    return 0;
                }
                return ((Int32)(HttpContext.Current.Session["_BusinessPartnerID"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_BusinessPartnerID"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_BusinessPartnerID"] = value;
            }
        }

        public static String BusinessPartnerName
        {
            get
            {
                if (HttpContext.Current.Session["_BusinessPartnerName"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_BusinessPartnerName"] != null)
                        {
                            string BusinessPartnerName = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_BusinessPartnerName"];
                            HttpContext.Current.Session["_BusinessPartnerName"] = BusinessPartnerName;
                            return BusinessPartnerName;
                        }
                    }
                    return "";
                }
                return ((String)(HttpContext.Current.Session["_BusinessPartnerName"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_BusinessPartnerName"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_BusinessPartnerName"] = value;
            }
        }

        public static Int32 CustomerGroupID
        {
            get
            {
                if (HttpContext.Current.Session["_CustomerGroupID"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_CustomerGroupID"] != null)
                        {
                            int CustomerGroupID = Convert.ToInt32(HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_CustomerGroupID"]);
                            HttpContext.Current.Session["_CustomerGroupID"] = CustomerGroupID;
                            return CustomerGroupID;
                        }
                    }
                    return 0;
                }
                return ((Int32)(HttpContext.Current.Session["_CustomerGroupID"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_CustomerGroupID"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_CustomerGroupID"] = value;
            }
        }

        public static Int32 FixedAssetID
        {
            get
            {
                if (HttpContext.Current.Session["_FixedAssetID"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_FixedAssetID"] != null)
                        {
                            int businessPartnerID = Convert.ToInt32(HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_FixedAssetID"]);
                            HttpContext.Current.Session["_FixedAssetID"] = businessPartnerID;
                            return businessPartnerID;
                        }
                    }
                    return 0;
                }
                return ((Int32)(HttpContext.Current.Session["_FixedAssetID"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_FixedAssetID"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_FixedAssetID"] = value;
            }
        }

        public static bool IsLicensed
        {
            get
            {
                if (HttpContext.Current.Session["_IsLicensed"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsLicensed"] != null)
                        {
                            HttpContext.Current.Session["_IsLicensed"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsLicensed"];
                            return Convert.ToBoolean(HttpContext.Current.Request.Cookies["_IsLicensed"]);
                        }
                    }
                }
                return (bool)HttpContext.Current.Session["_IsLicensed"];
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsLicensed"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsLicensed"] = value;
            }
        }

        #region Aplicares Bridging Session Parameter
        public static bool IsBridgingToAPLICARES
        {
            get
            {
                if (HttpContext.Current.Session["_IsBridgingToAPLICARES"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsBridgingToAPLICARES"] != null)
                        {
                            HttpContext.Current.Session["_IsBridgingToAPLICARES"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsBridgingToAPLICARES"];
                            return Convert.ToBoolean(HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsBridgingToAPLICARES"]);
                        }
                    }
                    return false;
                }

                return Convert.ToBoolean(HttpContext.Current.Session["_IsBridgingToAPLICARES"]);
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsBridgingToAPLICARES"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsBridgingToAPLICARES"] = value;
            }
        }

        public static string APLICARES_WS_URL
        {
            get
            {
                if (HttpContext.Current.Session["_APLICARES_WS_URL"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_APLICARES_WS_URL"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_APLICARES_WS_URL"];
                            HttpContext.Current.Session["_APLICARES_WS_URL"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_APLICARES_WS_URL"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_APLICARES_WS_URL"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_APLICARES_WS_URL"] = value;
            }

        }
        public static string APLICARES_Consumer_ID
        {
            get
            {
                if (HttpContext.Current.Session["_APLICARES_Consumer_ID"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_APLICARES_Consumer_ID"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_APLICARES_Consumer_ID"];
                            HttpContext.Current.Session["_APLICARES_Consumer_ID"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_APLICARES_Consumer_ID"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_APLICARES_Consumer_ID"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_APLICARES_Consumer_ID"] = value;
            }

        }

        public static string APLICARES_Consumer_Pwd
        {
            get
            {
                if (HttpContext.Current.Session["_APLICARES_Consumer_Pwd"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_APLICARES_Consumer_Pwd"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_APLICARES_Consumer_Pwd"];
                            HttpContext.Current.Session["_APLICARES_Consumer_Pwd"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_APLICARES_Consumer_Pwd"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_APLICARES_Consumer_Pwd"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_APLICARES_Consumer_Pwd"] = value;
            }

        }
        #endregion

        #region BPJS Bridging Session Parameter
        public static bool IsBridgingToBPJS
        {
            get
            {
                if (HttpContext.Current.Session["_IsBridgingToBPJS"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsBridgingToBPJS"] != null)
                        {
                            HttpContext.Current.Session["_IsBridgingToBPJS"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsBridgingToBPJS"];
                            return Convert.ToBoolean(HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsBridgingToBPJS"]);
                        }
                    }
                    return false;
                }

                return Convert.ToBoolean(HttpContext.Current.Session["_IsBridgingToBPJS"]);
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsBridgingToBPJS"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsBridgingToBPJS"] = value;
            }

        }

        public static string BPJS_WS_URL
        {
            get
            {
                if (HttpContext.Current.Session["_BPJS_WS_URL"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_BPJS_WS_URL"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_BPJS_WS_URL"];
                            HttpContext.Current.Session["_BPJS_WS_URL"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_BPJS_WS_URL"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_BPJS_WS_URL"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_BPJS_WS_URL"] = value;
            }

        }

        public static string BPJS_Consumer_ID
        {
            get
            {
                if (HttpContext.Current.Session["_BPJS_Consumer_ID"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_BPJS_Consumer_ID"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_BPJS_Consumer_ID"];
                            HttpContext.Current.Session["_BPJS_Consumer_ID"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_BPJS_Consumer_ID"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_BPJS_Consumer_ID"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_BPJS_Consumer_ID"] = value;
            }

        }

        public static string BPJS_Consumer_Pwd
        {
            get
            {
                if (HttpContext.Current.Session["_BPJS_Consumer_Pwd"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_BPJS_Consumer_Pwd"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_BPJS_Consumer_Pwd"];
                            HttpContext.Current.Session["_BPJS_Consumer_Pwd"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_BPJS_Consumer_Pwd"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_BPJS_Consumer_Pwd"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_BPJS_Consumer_Pwd"] = value;
            }

        }

        public static string BPJS_Code
        {
            get
            {
                if (HttpContext.Current.Session["_BPJS_Code"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_BPJS_Code"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_BPJS_Code"];
                            HttpContext.Current.Session["_BPJS_Code"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_BPJS_Code"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_BPJS_Code"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_BPJS_Code"] = value;
            }

        }
        #endregion

        #region INACBGs / E-Klaim Bridging Session Parameter
        public static bool IsBridgingToEKlaim
        {
            get
            {
                if (HttpContext.Current.Session["_IsBridgingToEKlaim"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsBridgingToEKlaim"] != null)
                        {
                            HttpContext.Current.Session["_IsBridgingToEKlaim"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsBridgingToEKlaim"];
                            return Convert.ToBoolean(HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsBridgingToEKlaim"]);
                        }
                    }
                    return false;
                }

                return Convert.ToBoolean(HttpContext.Current.Session["_IsBridgingToEKlaim"]);
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsBridgingToEKlaim"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsBridgingToEKlaim"] = value;
            }
        }

        public static string EKlaim_WebService_URL
        {
            get
            {
                if (HttpContext.Current.Session["_EKlaim_WebService_URL"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_EKlaim_WebService_URL"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_EKlaim_WebService_URL"];
                            HttpContext.Current.Session["_EKlaim_WebService_URL"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_EKlaim_WebService_URL"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_EKlaim_WebService_URL"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_EKlaim_WebService_URL"] = value;
            }

        }

        public static string EKlaim_HospitalCode
        {
            get
            {
                if (HttpContext.Current.Session["_EKlaim_HospitalCode"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_EKlaim_HospitalCode"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_EKlaim_HospitalCode"];
                            HttpContext.Current.Session["_EKlaim_HospitalCode"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_EKlaim_HospitalCode"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_EKlaim_HospitalCode"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_EKlaim_HospitalCode"] = value;
            }

        }

        public static string EKlaim_EncryptionKey
        {
            get
            {
                if (HttpContext.Current.Session["_EKlaim_EncryptionKey"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_EKlaim_EncryptionKey"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_EKlaim_EncryptionKey"];
                            HttpContext.Current.Session["_EKlaim_EncryptionKey"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_EKlaim_EncryptionKey"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_EKlaim_EncryptionKey"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_EKlaim_EncryptionKey"] = value;
            }

        }
        #endregion

        #region LIS Bridging Session Parameter
        public static bool IsBridgingToLIS
        {
            get
            {
                if (HttpContext.Current.Session["_IsBridgingToLIS"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsBridgingToLIS"] != null)
                        {
                            HttpContext.Current.Session["_IsBridgingToRIS"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsBridgingToLIS"];
                            return Convert.ToBoolean(HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsBridgingToLIS"]);
                        }
                    }
                    return false;
                }

                return Convert.ToBoolean(HttpContext.Current.Session["_IsBridgingToLIS"]);
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsBridgingToLIS"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsBridgingToLIS"] = value;
            }

        }

        public static string LIS_BRIDGING_PROTOCOL
        {
            get
            {
                if (HttpContext.Current.Session["_LIS_BRIDGING_PROTOCOL"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_LIS_BRIDGING_PROTOCOL"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_LIS_BRIDGING_PROTOCOL"];
                            HttpContext.Current.Session["_LIS_BRIDGING_PROTOCOL"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_LIS_BRIDGING_PROTOCOL"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_LIS_BRIDGING_PROTOCOL"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_LIS_BRIDGING_PROTOCOL"] = value;
            }

        }

        public static string LIS_WEB_API_URL
        {
            get
            {
                if (HttpContext.Current.Session["_LIS_WEB_API_URL"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_LIS_WEB_API_URL"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_LIS_WEB_API_URL"];
                            HttpContext.Current.Session["_LIS_WEB_API_URL"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_LIS_WEB_API_URL"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_LIS_WEB_API_URL"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_LIS_WEB_API_URL"] = value;
            }

        }

        public static string LIS_Consumer_ID
        {
            get
            {
                if (HttpContext.Current.Session["_LIS_Consumer_ID"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_LIS_Consumer_ID"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_LIS_Consumer_ID"];
                            HttpContext.Current.Session["_LIS_Consumer_ID"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_LIS_Consumer_ID"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_LIS_Consumer_ID"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_LIS_Consumer_ID"] = value;
            }

        }

        public static string LIS_Consumer_Pwd
        {
            get
            {
                if (HttpContext.Current.Session["_LIS_Consumer_Pwd"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_LIS_Consumer_Pwd"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_LIS_Consumer_Pwd"];
                            HttpContext.Current.Session["_LIS_Consumer_Pwd"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_LIS_Consumer_Pwd"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_LIS_Consumer_Pwd"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_LIS_Consumer_Pwd"] = value;
            }

        }
        public static string LIS_PROVIDER
        {
            get
            {
                if (HttpContext.Current.Session["_LIS_PROVIDER"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_LIS_PROVIDER"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_LIS_PROVIDER"];
                            HttpContext.Current.Session["_LIS_PROVIDER"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_LIS_PROVIDER"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_LIS_PROVIDER"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_LIS_PROVIDER"] = value;
            }

        }
        #endregion

        #region RIS Bridging Session Parameter
        public static bool IsBridgingToRIS
        {
            get
            {
                if (HttpContext.Current.Session["_IsBridgingToRIS"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsBridgingToRIS"] != null)
                        {
                            HttpContext.Current.Session["_IsBridgingToRIS"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsBridgingToRIS"];
                            return Convert.ToBoolean(HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsBridgingToRIS"]);
                        }
                    }
                    return false;
                }

                return Convert.ToBoolean(HttpContext.Current.Session["_IsBridgingToRIS"]);
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsBridgingToRIS"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsBridgingToRIS"] = value;
            }

        }

        public static string RIS_BRIDGING_PROTOCOL
        {
            get
            {
                if (HttpContext.Current.Session["_RIS_BRIDGING_PROTOCOL"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_RIS_BRIDGING_PROTOCOL"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_RIS_BRIDGING_PROTOCOL"];
                            HttpContext.Current.Session["_RIS_BRIDGING_PROTOCOL"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_RIS_BRIDGING_PROTOCOL"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_RIS_BRIDGING_PROTOCOL"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_RIS_BRIDGING_PROTOCOL"] = value;
            }

        }

        public static string RIS_HL7_MESSAGE_FORMAT
        {
            get
            {
                if (HttpContext.Current.Session["_RIS_HL7_MESSAGE_FORMAT"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_RIS_HL7_MESSAGE_FORMAT"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_RIS_HL7_MESSAGE_FORMAT"];
                            HttpContext.Current.Session["_RIS_HL7_MESSAGE_FORMAT"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_RIS_HL7_MESSAGE_FORMAT"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_RIS_HL7_MESSAGE_FORMAT"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_RIS_HL7_MESSAGE_FORMAT"] = value;
            }

        }

        public static string RIS_WEB_API_URL
        {
            get
            {
                if (HttpContext.Current.Session["_RIS_WEB_API_URL"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_RIS_WEB_API_URL"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_RIS_WEB_API_URL"];
                            HttpContext.Current.Session["_RIS_WEB_API_URL"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_RIS_WEB_API_URL"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_RIS_WEB_API_URL"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_RIS_WEB_API_URL"] = value;
            }

        }

        public static string RIS_WEB_VIEW_URL
        {
            get
            {
                if (HttpContext.Current.Session["_RIS_WEB_VIEW_URL"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_RIS_WEB_VIEW_URL"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_RIS_WEB_VIEW_URL"];
                            HttpContext.Current.Session["_RIS_WEB_VIEW_URL"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_RIS_WEB_VIEW_URL"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_RIS_WEB_VIEW_URL"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_RIS_WEB_VIEW_URL"] = value;
            }

        }

        public static string RIS_Consumer_ID
        {
            get
            {
                if (HttpContext.Current.Session["_RIS_Consumer_ID"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_RIS_Consumer_ID"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_RIS_Consumer_ID"];
                            HttpContext.Current.Session["_RIS_Consumer_ID"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_RIS_Consumer_ID"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_RIS_Consumer_ID"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_RIS_Consumer_ID"] = value;
            }

        }

        public static string RIS_Consumer_Pwd
        {
            get
            {
                if (HttpContext.Current.Session["_RIS_Consumer_Pwd"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_RIS_Consumer_Pwd"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_RIS_Consumer_Pwd"];
                            HttpContext.Current.Session["_RIS_Consumer_Pwd"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_RIS_Consumer_Pwd"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_RIS_Consumer_Pwd"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_RIS_Consumer_Pwd"] = value;
            }

        }

        public static string BRIDGING_TOOLS
        {
            get
            {
                if (HttpContext.Current.Session["_BRIDGING_TOOLS"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_BRIDGING_TOOLS"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_BRIDGING_TOOLS"];
                            HttpContext.Current.Session["_BRIDGING_TOOLS"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_BRIDGING_TOOLS"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_BRIDGING_TOOLS"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_BRIDGING_TOOLS"] = value;
            }

        }

        public static string RESULT_SEND_TO_BRIDGING
        {
            get
            {
                if (HttpContext.Current.Session["_RESULT_SEND_TO_BRIDGING"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_RESULT_SEND_TO_BRIDGING"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_RESULT_SEND_TO_BRIDGING"];
                            HttpContext.Current.Session["_RESULT_SEND_TO_BRIDGING"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_RESULT_SEND_TO_BRIDGING"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_RESULT_SEND_TO_BRIDGING"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_RESULT_SEND_TO_BRIDGING"] = value;
            }

        }
        #endregion

        #region Queue Bridging Session Parameter
        public static bool IsBridgingToQueue
        {
            get
            {
                if (HttpContext.Current.Session["_IsBridgingToQueue"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsBridgingToQueue"] != null)
                        {
                            HttpContext.Current.Session["_IsBridgingToQueue"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsBridgingToQueue"];
                            return Convert.ToBoolean(HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsBridgingToQueue"]);
                        }
                    }
                    return false;
                }

                return Convert.ToBoolean(HttpContext.Current.Session["_IsBridgingToQueue"]);
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsBridgingToQueue"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsBridgingToQueue"] = value;
            }

        }

        public static string QUEUE_WEB_API_URL
        {
            get
            {
                if (HttpContext.Current.Session["_QUEUE_WEB_API_URL"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_QUEUE_WEB_API_URL"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_QUEUE_WEB_API_URL"];
                            HttpContext.Current.Session["_QUEUE_WEB_API_URL"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_QUEUE_WEB_API_URL"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_QUEUE_WEB_API_URL"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_QUEUE_WEB_API_URL"] = value;
            }

        }

        public static string QUEUE_Consumer_ID
        {
            get
            {
                if (HttpContext.Current.Session["_QUEUE_Consumer_ID"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_QUEUE_Consumer_ID"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_QUEUE_Consumer_ID"];
                            HttpContext.Current.Session["_QUEUE_Consumer_ID"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_QUEUE_Consumer_ID"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_QUEUE_Consumer_ID"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_QUEUE_Consumer_ID"] = value;
            }

        }

        public static string QUEUE_Consumer_Pwd
        {
            get
            {
                if (HttpContext.Current.Session["_QUEUE_Consumer_Pwd"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_QUEUE_Consumer_Pwd"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_QUEUE_Consumer_Pwd"];
                            HttpContext.Current.Session["_QUEUE_Consumer_Pwd"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_QUEUE_Consumer_Pwd"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_QUEUE_Consumer_Pwd"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_QUEUE_Consumer_Pwd"] = value;
            }

        }
        #endregion

        #region Inhealth Bridging Session Parameter
        public static bool IsBridgingToInhealth
        {
            get
            {
                if (HttpContext.Current.Session["_IsBridgingToInhealth"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsBridgingToInhealth"] != null)
                        {
                            HttpContext.Current.Session["_IsBridgingToInhealth"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsBridgingToInhealth"];
                            return Convert.ToBoolean(HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsBridgingToInhealth"]);
                        }
                    }
                    return false;
                }

                return Convert.ToBoolean(HttpContext.Current.Session["_IsBridgingToInhealth"]);
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsBridgingToInhealth"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsBridgingToInhealth"] = value;
            }
        }

        public static string Inhealth_WebService_URL
        {
            get
            {
                if (HttpContext.Current.Session["_Inhealth_WebService_URL"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_Inhealth_WebService_URL"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_Inhealth_WebService_URL"];
                            HttpContext.Current.Session["_Inhealth_WebService_URL"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_Inhealth_WebService_URL"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_Inhealth_WebService_URL"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_Inhealth_WebService_URL"] = value;
            }

        }

        public static string Inheatlh_Access_Token
        {
            get
            {
                if (HttpContext.Current.Session["_Inheatlh_Access_Token"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["Inheatlh_Access_Token"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["Inheatlh_Access_Token"];
                            HttpContext.Current.Session["Inheatlh_Access_Token"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["Inheatlh_Access_Token"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["Inheatlh_Access_Token"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["Inheatlh_Access_Token"] = value;
            }

        }

        public static string Inhealth_Provider_Code
        {
            get
            {
                if (HttpContext.Current.Session["_Inhealth_Provider_Code"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_Inhealth_Provider_Code"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_Inhealth_Provider_Code"];
                            HttpContext.Current.Session["_Inhealth_Provider_Code"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_Inhealth_Provider_Code"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_Inhealth_Provider_Code"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_Inhealth_Provider_Code"] = value;
            }

        }
        #endregion

        public static bool IsPrinterLocationBasedOnIP
        {
            get
            {
                if (HttpContext.Current.Session["_IsPrinterLocationBasedOnIP"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsPrinterLocationBasedOnIP"] != null)
                        {
                            HttpContext.Current.Session["_IsPrinterLocationBasedOnIP"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsPrinterLocationBasedOnIP"];
                            return Convert.ToBoolean(HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsPrinterLocationBasedOnIP"]);
                        }
                    }
                    return false;
                }

                return Convert.ToBoolean(HttpContext.Current.Session["_IsPrinterLocationBasedOnIP"]);
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsPrinterLocationBasedOnIP"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsPrinterLocationBasedOnIP"] = value;
            }

        }

        public static string IsUsingMultiRegistrationPrinterLocation
        {
            get
            {
                if (HttpContext.Current.Session["_IsUsingMultiRegistrationPrinterLocation"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsingMultiRegistrationPrinterLocation"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsingMultiRegistrationPrinterLocation"];
                            HttpContext.Current.Session["_IsUsingMultiRegistrationPrinterLocation"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_IsUsingMultiRegistrationPrinterLocation"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsUsingMultiRegistrationPrinterLocation"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsUsingMultiRegistrationPrinterLocation"] = value;
            }

        }

        public static string PreviewReportInPDF
        {
            get
            {
                if (HttpContext.Current.Session["_PreviewReportInPDF"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_PreviewReportInPDF"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_PreviewReportInPDF"];
                            HttpContext.Current.Session["_PreviewReportInPDF"] = value;
                            return value;
                        }
                    }
                    return "0";
                }

                return ((String)(HttpContext.Current.Session["_PreviewReportInPDF"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_PreviewReportInPDF"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_PreviewReportInPDF"] = value;
            }

        }

        public static string LastSelectedDepartment
        {
            get
            {
                if (HttpContext.Current.Session["_LastSelectedDepartment"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_LastSelectedDepartment"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_LastSelectedDepartment"];
                            HttpContext.Current.Session["_LastSelectedDepartment"] = value;
                            return value;
                        }
                    }
                    return string.Empty;
                }

                return ((String)(HttpContext.Current.Session["_LastSelectedDepartment"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_LastSelectedDepartment"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_LastSelectedDepartment"] = value;
            }

        }


        public static string LastSelectedServiceUnit
        {
            get
            {
                if (HttpContext.Current.Session["_LastSelectedServiceUnit"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_LastSelectedServiceUnit"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_LastSelectedServiceUnit"];
                            HttpContext.Current.Session["_LastSelectedServiceUnit"] = value;
                            return value;
                        }
                    }
                    return string.Empty;
                }

                return ((String)(HttpContext.Current.Session["_LastSelectedServiceUnit"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_LastSelectedServiceUnit"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_LastSelectedServiceUnit"] = value;
            }

        }

        public static bool IsPatientPageByDepartment
        {
            get
            {
                if (HttpContext.Current.Session["_IsPatientPageByDepartment"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsPatientPageByDepartment"] != null)
                        {
                            HttpContext.Current.Session["_IsPatientPageByDepartment"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsPatientPageByDepartment"];
                            return Convert.ToBoolean(HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsPatientPageByDepartment"]);
                        }
                    }
                    return false;
                }

                return Convert.ToBoolean(HttpContext.Current.Session["_IsPatientPageByDepartment"]);
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsPatientPageByDepartment"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsPatientPageByDepartment"] = value;
            }

        }

        public static string IsUsedProductLine
        {
            get
            {
                if (HttpContext.Current.Session["_IsUsedProductLine"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedProductLine"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedProductLine"];
                            HttpContext.Current.Session["_IsUsedProductLine"] = value;
                            return value;
                        }
                    }
                    return string.Empty;
                }

                return ((String)(HttpContext.Current.Session["_IsUsedProductLine"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsUsedProductLine"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsUsedProductLine"] = value;
            }

        }

        public static string IsAPConsignmentFromOrder
        {
            get
            {
                if (HttpContext.Current.Session["_IsAPConsignmentFromOrder"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsAPConsignmentFromOrder"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsAPConsignmentFromOrder"];
                            HttpContext.Current.Session["_IsAPConsignmentFromOrder"] = value;
                            return value;
                        }
                    }
                    return string.Empty;
                }

                return ((String)(HttpContext.Current.Session["_IsAPConsignmentFromOrder"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsAPConsignmentFromOrder"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsAPConsignmentFromOrder"] = value;
            }

        }

        public static bool IsLimitedCPOEItemForBPJS
        {
            get
            {
                if (HttpContext.Current.Session["_IsLimitedCPOEItemForBPJS"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsLimitedCPOEItemForBPJS"] != null)
                        {
                            HttpContext.Current.Session["_IsLimitedCPOEItemForBPJS"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsLimitedCPOEItemForBPJS"];
                            return Convert.ToBoolean(HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsLimitedCPOEItemForBPJS"]);
                        }
                    }
                    return false;
                }

                return Convert.ToBoolean(HttpContext.Current.Session["_IsLimitedCPOEItemForBPJS"]);
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsLimitedCPOEItemForBPJS"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsLimitedCPOEItemForBPJS"] = value;
            }

        }

        public static Int32 BusinessPartnerIDBPJS
        {
            get
            {
                if (HttpContext.Current.Session["_BusinessPartnerIDBPJS"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_BusinessPartnerIDBPJS"] != null)
                        {
                            HttpContext.Current.Session["_BusinessPartnerIDBPJS"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_BusinessPartnerIDBPJS"];
                            return Convert.ToInt32(HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_BusinessPartnerIDBPJS"]);
                        }
                    }
                    return 0;
                }

                return Convert.ToInt32(HttpContext.Current.Session["_BusinessPartnerIDBPJS"]);
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_BusinessPartnerIDBPJS"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_BusinessPartnerIDBPJS"] = value;
            }

        }
        public static String ImagingServiceUnitID
        {
            get
            {
                if (HttpContext.Current.Session["_ImagingServiceUnitID"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_ImagingServiceUnitID"] != null)
                        {
                            HttpContext.Current.Session["_ImagingServiceUnitID"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_ImagingServiceUnitID"];
                            return HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_ImagingServiceUnitID"];
                        }
                    }
                    return "0";
                }

                return Convert.ToString(HttpContext.Current.Session["_ImagingServiceUnitID"]);
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_ImagingServiceUnitID"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_ImagingServiceUnitID"] = value;
            }

        }

        public static String LaboratoryServiceUnitID
        {
            get
            {
                if (HttpContext.Current.Session["_LaboratoryServiceUnitID"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_LaboratoryServiceUnitID"] != null)
                        {
                            HttpContext.Current.Session["_LaboratoryServiceUnitID"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_LaboratoryServiceUnitID"];
                            return HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_LaboratoryServiceUnitID"];
                        }
                    }
                    return "0";
                }

                return Convert.ToString(HttpContext.Current.Session["_LaboratoryServiceUnitID"]);
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_LaboratoryServiceUnitID"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_LaboratoryServiceUnitID"] = value;
            }

        }

        public static string IsAutoRelocateDispensary
        {
            get
            {
                if (HttpContext.Current.Session["_IsAutoRelocateDispensary"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsAutoRelocateDispensary"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsAutoRelocateDispensary"];
                            HttpContext.Current.Session["_IsAutoRelocateDispensary"] = value;
                            return value;
                        }
                    }
                    return string.Empty;
                }

                return ((String)(HttpContext.Current.Session["_IsAutoRelocateDispensary"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsAutoRelocateDispensary"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsAutoRelocateDispensary"] = value;
            }

        }

        public static string AutoRelocateDispensaryTime
        {
            get
            {
                if (HttpContext.Current.Session["_AutoRelocateDispensaryTime"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_AutoRelocateDispensaryTime"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_AutoRelocateDispensaryTime"];
                            HttpContext.Current.Session["_AutoRelocateDispensaryTime"] = value;
                            return value;
                        }
                    }
                    return string.Empty;
                }

                return ((String)(HttpContext.Current.Session["_AutoRelocateDispensaryTime"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_AutoRelocateDispensaryTime"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_AutoRelocateDispensaryTime"] = value;
            }
        }

        public static string AutoRelocateDispensaryID
        {
            get
            {
                if (HttpContext.Current.Session["_AutoRelocateDispensaryID"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_AutoRelocateDispensaryID"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_AutoRelocateDispensaryID"];
                            HttpContext.Current.Session["_AutoRelocateDispensaryID"] = value;
                            return value;
                        }
                    }
                    return string.Empty;
                }

                return ((String)(HttpContext.Current.Session["_AutoRelocateDispensaryID"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_AutoRelocateDispensaryID"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_AutoRelocateDispensaryID"] = value;
            }
        }

        public static string UseBPJSPrescriptionEntryMode
        {
            get
            {
                if (HttpContext.Current.Session["_UseBPJSPrescriptionEntryMode"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_UseBPJSPrescriptionEntryMode"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_UseBPJSPrescriptionEntryMode"];
                            HttpContext.Current.Session["_UseBPJSPrescriptionEntryMode"] = value;
                            return value;
                        }
                    }
                    return string.Empty;
                }

                return ((String)(HttpContext.Current.Session["_UseBPJSPrescriptionEntryMode"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_UseBPJSPrescriptionEntryMode"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_UseBPJSPrescriptionEntryMode"] = value;
            }

        }

        public static string BPJSTakenQtyFormula
        {
            get
            {
                if (HttpContext.Current.Session["_BPJSTakenQtyFormula"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_BPJSTakenQtyFormula"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_BPJSTakenQtyFormula"];
                            HttpContext.Current.Session["_BPJSTakenQtyFormula"] = value;
                            return value;
                        }
                    }
                    return string.Empty;
                }

                return ((String)(HttpContext.Current.Session["_BPJSTakenQtyFormula"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_BPJSTakenQtyFormula"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_BPJSTakenQtyFormula"] = value;
            }

        }

        public static string IsAutoVoidPrescriptionOrderCreatedBySystem
        {
            get
            {
                if (HttpContext.Current.Session["_IsAutoVoidPrescriptionOrderCreatedBySystem"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsAutoVoidPrescriptionOrderCreatedBySystem"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsAutoVoidPrescriptionOrderCreatedBySystem"];
                            HttpContext.Current.Session["_IsAutoVoidPrescriptionOrderCreatedBySystem"] = value;
                            return value;
                        }
                    }
                    return string.Empty;
                }

                return ((String)(HttpContext.Current.Session["_IsAutoVoidPrescriptionOrderCreatedBySystem"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsAutoVoidPrescriptionOrderCreatedBySystem"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsAutoVoidPrescriptionOrderCreatedBySystem"] = value;
            }

        }

        public static String VisitNoteID
        {
            get
            {
                if (HttpContext.Current.Session["_VisitNoteID"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_VisitNoteID"] != null)
                        {
                            HttpContext.Current.Session["_VisitNoteID"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_VisitNoteID"];
                            return HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_VisitNoteID"];
                        }
                    }
                    return "0";
                }

                return Convert.ToString(HttpContext.Current.Session["_VisitNoteID"]);
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_VisitNoteID"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_VisitNoteID"] = value;
            }

        }

        public static String RefreshGridInterval
        {
            get
            {
                if (HttpContext.Current.Session["_RefreshGridInterval"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_RefreshGridInterval"] != null)
                        {
                            HttpContext.Current.Session["_RefreshGridInterval"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_RefreshGridInterval"];
                            return HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_RefreshGridInterval"];
                        }
                    }
                    return "10";
                }

                return HttpContext.Current.Session["_RefreshGridInterval"].ToString();
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_RefreshGridInterval"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_RefreshGridInterval"] = value;
            }
        }

        public static String IsUsedPatientOwnerStatus
        {
            get
            {
                if (HttpContext.Current.Session["_IsUsedPatientOwnerStatus"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedPatientOwnerStatus"] != null)
                        {
                            HttpContext.Current.Session["_IsUsedPatientOwnerStatus"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedPatientOwnerStatus"];
                            return HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedPatientOwnerStatus"];
                        }
                    }
                    return "0";
                }

                return HttpContext.Current.Session["_IsUsedPatientOwnerStatus"].ToString();
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsUsedPatientOwnerStatus"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsUsedPatientOwnerStatus"] = value;
            }
        }

        public static String IsUsedPatientOwnerStatusInInpatientRegistration
        {
            get
            {
                if (HttpContext.Current.Session["_IsUsedPatientOwnerStatusInInpatientRegistration"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedPatientOwnerStatusInInpatientRegistration"] != null)
                        {
                            HttpContext.Current.Session["_IsUsedPatientOwnerStatusInInpatientRegistration"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedPatientOwnerStatusInInpatientRegistration"];
                            return HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedPatientOwnerStatusInInpatientRegistration"];
                        }
                    }
                    return "0";
                }

                return HttpContext.Current.Session["_IsUsedPatientOwnerStatusInInpatientRegistration"].ToString();
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsUsedPatientOwnerStatusInInpatientRegistration"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsUsedPatientOwnerStatusInInpatientRegistration"] = value;
            }
        }

        public static String IsUsedPatientOwnerStatusInOutpatientRegistration
        {
            get
            {
                if (HttpContext.Current.Session["_IsUsedPatientOwnerStatusInOutpatientRegistration"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedPatientOwnerStatusInOutpatientRegistration"] != null)
                        {
                            HttpContext.Current.Session["_IsUsedPatientOwnerStatusInOutpatientRegistration"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedPatientOwnerStatusInOutpatientRegistration"];
                            return HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedPatientOwnerStatusInOutpatientRegistration"];
                        }
                    }
                    return "0";
                }

                return HttpContext.Current.Session["_IsUsedPatientOwnerStatusInOutpatientRegistration"].ToString();
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsUsedPatientOwnerStatusInOutpatientRegistration"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsUsedPatientOwnerStatusInOutpatientRegistration"] = value;
            }
        }

        public static String IsUsedPatientOwnerStatusInEmergencyRegistration
        {
            get
            {
                if (HttpContext.Current.Session["_IsUsedPatientOwnerStatusInEmergencyRegistration"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedPatientOwnerStatusInEmergencyRegistration"] != null)
                        {
                            HttpContext.Current.Session["_IsUsedPatientOwnerStatusInEmergencyRegistration"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedPatientOwnerStatusInEmergencyRegistration"];
                            return HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedPatientOwnerStatusInEmergencyRegistration"];
                        }
                    }
                    return "0";
                }

                return HttpContext.Current.Session["_IsUsedPatientOwnerStatusInEmergencyRegistration"].ToString();
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsUsedPatientOwnerStatusInEmergencyRegistration"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsUsedPatientOwnerStatusInEmergencyRegistration"] = value;
            }
        }

        public static String IsUsedPatientOwnerStatusInMCURegistration
        {
            get
            {
                if (HttpContext.Current.Session["_IsUsedPatientOwnerStatusInMCURegistration"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedPatientOwnerStatusInMCURegistration"] != null)
                        {
                            HttpContext.Current.Session["_IsUsedPatientOwnerStatusInMCURegistration"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedPatientOwnerStatusInMCURegistration"];
                            return HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedPatientOwnerStatusInMCURegistration"];
                        }
                    }
                    return "0";
                }

                return HttpContext.Current.Session["_IsUsedPatientOwnerStatusInMCURegistration"].ToString();
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsUsedPatientOwnerStatusInMCURegistration"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsUsedPatientOwnerStatusInMCURegistration"] = value;
            }
        }

        public static String IsUsedPatientOwnerStatusInLaboratoryRegistration
        {
            get
            {
                if (HttpContext.Current.Session["_IsUsedPatientOwnerStatusInLaboratoryRegistration"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedPatientOwnerStatusInLaboratoryRegistration"] != null)
                        {
                            HttpContext.Current.Session["_IsUsedPatientOwnerStatusInLaboratoryRegistration"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedPatientOwnerStatusInLaboratoryRegistration"];
                            return HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedPatientOwnerStatusInLaboratoryRegistration"];
                        }
                    }
                    return "0";
                }

                return HttpContext.Current.Session["_IsUsedPatientOwnerStatusInLaboratoryRegistration"].ToString();
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsUsedPatientOwnerStatusInLaboratoryRegistration"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsUsedPatientOwnerStatusInLaboratoryRegistration"] = value;
            }
        }

        public static String IsUsedPatientOwnerStatusInImagingRegistration
        {
            get
            {
                if (HttpContext.Current.Session["_IsUsedPatientOwnerStatusInImagingRegistration"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedPatientOwnerStatusInImagingRegistration"] != null)
                        {
                            HttpContext.Current.Session["_IsUsedPatientOwnerStatusInImagingRegistration"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedPatientOwnerStatusInImagingRegistration"];
                            return HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedPatientOwnerStatusInImagingRegistration"];
                        }
                    }
                    return "0";
                }

                return HttpContext.Current.Session["_IsUsedPatientOwnerStatusInImagingRegistration"].ToString();
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsUsedPatientOwnerStatusInImagingRegistration"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsUsedPatientOwnerStatusInImagingRegistration"] = value;
            }
        }

        public static String IsUsedPatientOwnerStatusInDiagnosticRegistration
        {
            get
            {
                if (HttpContext.Current.Session["_IsUsedPatientOwnerStatusInDiagnosticRegistration"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedPatientOwnerStatusInDiagnosticRegistration"] != null)
                        {
                            HttpContext.Current.Session["_IsUsedPatientOwnerStatusInDiagnosticRegistration"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedPatientOwnerStatusInDiagnosticRegistration"];
                            return HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedPatientOwnerStatusInDiagnosticRegistration"];
                        }
                    }
                    return "0";
                }

                return HttpContext.Current.Session["_IsUsedPatientOwnerStatusInDiagnosticRegistration"].ToString();
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsUsedPatientOwnerStatusInDiagnosticRegistration"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsUsedPatientOwnerStatusInDiagnosticRegistration"] = value;
            }
        }

        public static String IsUsedPatientOwnerStatusInPharmacyRegistration
        {
            get
            {
                if (HttpContext.Current.Session["_IsUsedPatientOwnerStatusInPharmacyRegistration"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedPatientOwnerStatusInPharmacyRegistration"] != null)
                        {
                            HttpContext.Current.Session["_IsUsedPatientOwnerStatusInPharmacyRegistration"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedPatientOwnerStatusInPharmacyRegistration"];
                            return HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedPatientOwnerStatusInPharmacyRegistration"];
                        }
                    }
                    return "0";
                }

                return HttpContext.Current.Session["_IsUsedPatientOwnerStatusInPharmacyRegistration"].ToString();
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsUsedPatientOwnerStatusInPharmacyRegistration"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsUsedPatientOwnerStatusInPharmacyRegistration"] = value;
            }
        }

        public static String IsUsedInputItemAIOInOutpatientRegistration
        {
            get
            {
                if (HttpContext.Current.Session["_IsUsedInputItemAIOInOutpatientRegistration"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedInputItemAIOInOutpatientRegistration"] != null)
                        {
                            HttpContext.Current.Session["_IsUsedInputItemAIOInOutpatientRegistration"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedInputItemAIOInOutpatientRegistration"];
                            return HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedInputItemAIOInOutpatientRegistration"];
                        }
                    }
                    return "0";
                }

                return HttpContext.Current.Session["_IsUsedInputItemAIOInOutpatientRegistration"].ToString();
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsUsedInputItemAIOInOutpatientRegistration"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsUsedInputItemAIOInOutpatientRegistration"] = value;
            }
        }

        public static String IsUsedInputItemAIOInLaboratoryRegistration
        {
            get
            {
                if (HttpContext.Current.Session["_IsUsedInputItemAIOInLaboratoryRegistration"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedInputItemAIOInLaboratoryRegistration"] != null)
                        {
                            HttpContext.Current.Session["_IsUsedInputItemAIOInLaboratoryRegistration"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedInputItemAIOInLaboratoryRegistration"];
                            return HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedInputItemAIOInLaboratoryRegistration"];
                        }
                    }
                    return "0";
                }

                return HttpContext.Current.Session["_IsUsedInputItemAIOInLaboratoryRegistration"].ToString();
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsUsedInputItemAIOInLaboratoryRegistration"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsUsedInputItemAIOInLaboratoryRegistration"] = value;
            }
        }

        public static String IsUsedInputItemAIOInImagingRegistration
        {
            get
            {
                if (HttpContext.Current.Session["_IsUsedInputItemAIOInImagingRegistration"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedInputItemAIOInImagingRegistration"] != null)
                        {
                            HttpContext.Current.Session["_IsUsedInputItemAIOInImagingRegistration"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedInputItemAIOInImagingRegistration"];
                            return HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedInputItemAIOInImagingRegistration"];
                        }
                    }
                    return "0";
                }

                return HttpContext.Current.Session["_IsUsedInputItemAIOInImagingRegistration"].ToString();
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsUsedInputItemAIOInImagingRegistration"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsUsedInputItemAIOInImagingRegistration"] = value;
            }
        }

        public static String IsUsedInputItemAIOInDiagnosticRegistration
        {
            get
            {
                if (HttpContext.Current.Session["_IsUsedInputItemAIOInDiagnosticRegistration"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedInputItemAIOInDiagnosticRegistration"] != null)
                        {
                            HttpContext.Current.Session["_IsUsedInputItemAIOInDiagnosticRegistration"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedInputItemAIOInDiagnosticRegistration"];
                            return HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedInputItemAIOInDiagnosticRegistration"];
                        }
                    }
                    return "0";
                }

                return HttpContext.Current.Session["_IsUsedInputItemAIOInDiagnosticRegistration"].ToString();
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsUsedInputItemAIOInDiagnosticRegistration"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsUsedInputItemAIOInDiagnosticRegistration"] = value;
            }
        }

        public static String IsUsedInputItemAIOInPharmacyRegistration
        {
            get
            {
                if (HttpContext.Current.Session["_IsUsedInputItemAIOInPharmacyRegistration"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedInputItemAIOInPharmacyRegistration"] != null)
                        {
                            HttpContext.Current.Session["_IsUsedInputItemAIOInPharmacyRegistration"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedInputItemAIOInPharmacyRegistration"];
                            return HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedInputItemAIOInPharmacyRegistration"];
                        }
                    }
                    return "0";
                }

                return HttpContext.Current.Session["_IsUsedInputItemAIOInPharmacyRegistration"].ToString();
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsUsedInputItemAIOInPharmacyRegistration"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsUsedInputItemAIOInPharmacyRegistration"] = value;
            }
        }

        public static String IsUsedInputItemAIOInInpatientRegistration
        {
            get
            {
                if (HttpContext.Current.Session["_IsUsedInputItemAIOInInpatientRegistration"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedInputItemAIOInInpatientRegistration"] != null)
                        {
                            HttpContext.Current.Session["_IsUsedInputItemAIOInInpatientRegistration"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedInputItemAIOInInpatientRegistration"];
                            return HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedInputItemAIOInInpatientRegistration"];
                        }
                    }
                    return "0";
                }

                return HttpContext.Current.Session["_IsUsedInputItemAIOInInpatientRegistration"].ToString();
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsUsedInputItemAIOInInpatientRegistration"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsUsedInputItemAIOInInpatientRegistration"] = value;
            }
        }

        public static String IsUsedCalculateCoveragePerBillingGroup
        {
            get
            {
                if (HttpContext.Current.Session["_IsUsedCalculateCoveragePerBillingGroup"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedCalculateCoveragePerBillingGroup"] != null)
                        {
                            HttpContext.Current.Session["_IsUsedCalculateCoveragePerBillingGroup"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedCalculateCoveragePerBillingGroup"];
                            return HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedCalculateCoveragePerBillingGroup"];
                        }
                    }
                    return "0";
                }

                return HttpContext.Current.Session["_IsUsedCalculateCoveragePerBillingGroup"].ToString();
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsUsedCalculateCoveragePerBillingGroup"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsUsedCalculateCoveragePerBillingGroup"] = value;
            }
        }

        public static bool IsUseVerifyAllButton
        {
            get
            {
                if (HttpContext.Current.Session["_IsUseVerifyAllButton"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUseVerifyAllButton"] != null)
                        {
                            HttpContext.Current.Session["_IsUseVerifyAllButton"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUseVerifyAllButton"];
                            return Convert.ToBoolean(HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUseVerifyAllButton"]);
                        }
                    }
                    return false;
                }

                return Convert.ToBoolean(HttpContext.Current.Session["_IsUseVerifyAllButton"]);
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsUseVerifyAllButton"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsUseVerifyAllButton"] = value;
            }
        }

        public static bool IsHasAllergy
        {
            get
            {
                if (HttpContext.Current.Session["_IsHasAllergy"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsHasAllergy"] != null)
                        {
                            HttpContext.Current.Session["_IsHasAllergy"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsHasAllergy"];
                            return Convert.ToBoolean(HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsHasAllergy"]);
                        }
                    }
                    return false;
                }

                return Convert.ToBoolean(HttpContext.Current.Session["_IsHasAllergy"]);
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsHasAllergy"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsHasAllergy"] = value;
            }
        }

        public static String PatientAllergyInfo
        {
            get
            {
                if (HttpContext.Current.Session["_PatientAllergyInfo"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_PatientAllergyInfo"] != null)
                        {
                            HttpContext.Current.Session["_PatientAllergyInfo"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_PatientAllergyInfo"];
                            return HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_PatientAllergyInfo"];
                        }
                    }
                    return "10";
                }

                return HttpContext.Current.Session["_PatientAllergyInfo"].ToString();
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_PatientAllergyInfo"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_PatientAllergyInfo"] = value;
            }
        }

        public static string IsUsedRevenueCostCenter
        {
            get
            {
                if (HttpContext.Current.Session["_IsUsedRevenueCostCenter"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedRevenueCostCenter"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedRevenueCostCenter"];
                            HttpContext.Current.Session["_IsUsedRevenueCostCenter"] = value;
                            return value;
                        }
                    }
                    return string.Empty;
                }

                return ((String)(HttpContext.Current.Session["_IsUsedRevenueCostCenter"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsUsedRevenueCostCenter"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsUsedRevenueCostCenter"] = value;
            }

        }

        public static bool IsUsedReopenBilling
        {
            get
            {
                if (HttpContext.Current.Session["_IsUsedReopenBilling"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedReopenBilling"] != null)
                        {
                            HttpContext.Current.Session["_IsUsedReopenBilling"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedReopenBilling"];
                            return Convert.ToBoolean(HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedReopenBilling"]);
                        }
                    }
                    return false;
                }

                return Convert.ToBoolean(HttpContext.Current.Session["_IsUsedReopenBilling"]);
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsUsedReopenBilling"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsUsedReopenBilling"] = value;
            }
        }

        public static bool IsUsedClaimFinal
        {
            get
            {
                if (HttpContext.Current.Session["_IsUsedClaimFinal"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedClaimFinal"] != null)
                        {
                            HttpContext.Current.Session["_IsUsedClaimFinal"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedClaimFinal"];
                            return Convert.ToBoolean(HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedClaimFinal"]);
                        }
                    }
                    return false;
                }

                return Convert.ToBoolean(HttpContext.Current.Session["_IsUsedClaimFinal"]);
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsUsedClaimFinal"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsUsedClaimFinal"] = value;
            }
        }

        public static bool IsClaimFinalAfterARInvoice
        {
            get
            {
                if (HttpContext.Current.Session["_IsClaimFinalAfterARInvoice"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsClaimFinalAfterARInvoice"] != null)
                        {
                            HttpContext.Current.Session["_IsClaimFinalAfterARInvoice"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsClaimFinalAfterARInvoice"];
                            return Convert.ToBoolean(HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsClaimFinalAfterARInvoice"]);
                        }
                    }
                    return false;
                }

                return Convert.ToBoolean(HttpContext.Current.Session["_IsClaimFinalAfterARInvoice"]);
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsClaimFinalAfterARInvoice"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsClaimFinalAfterARInvoice"] = value;
            }
        }

        public static bool IsClaimFinalBeforeARInvoiceAndSkipProcessClaim
        {
            get
            {
                if (HttpContext.Current.Session["_IsClaimFinalBeforeARInvoiceAndSkipProcessClaim"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsClaimFinalBeforeARInvoiceAndSkipProcessClaim"] != null)
                        {
                            HttpContext.Current.Session["_IsClaimFinalBeforeARInvoiceAndSkipProcessClaim"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsClaimFinalBeforeARInvoiceAndSkipProcessClaim"];
                            return Convert.ToBoolean(HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsClaimFinalBeforeARInvoiceAndSkipProcessClaim"]);
                        }
                    }
                    return false;
                }

                return Convert.ToBoolean(HttpContext.Current.Session["_IsClaimFinalBeforeARInvoiceAndSkipProcessClaim"]);
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsClaimFinalBeforeARInvoiceAndSkipProcessClaim"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsClaimFinalBeforeARInvoiceAndSkipProcessClaim"] = value;
            }
        }

        public static ParamedicMasterRevenueSharingProcess ParamedicMasterRevenueSharingProcess
        {
            get
            {
                if (HttpContext.Current.Session["_ParamedicMasterRevenueSharingProcess"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_ParamedicMasterRevenueSharingProcess"] != null)
                        {
                            ParamedicMasterRevenueSharingProcess pt = new ParamedicMasterRevenueSharingProcess();

                            HttpContext.Current.Session["_ParamedicMasterRevenueSharingProcess"] = pt;

                            return pt;
                        }
                    }
                    return null;
                }

                return ((ParamedicMasterRevenueSharingProcess)(HttpContext.Current.Session["_ParamedicMasterRevenueSharingProcess"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_ParamedicMasterRevenueSharingProcess"] = value.ParamedicID.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_ParamedicMasterRevenueSharingProcess"] = value;
            }
        }

        public static string RegistrationButtonTransactionDirectMenu
        {
            get
            {
                if (HttpContext.Current.Session["_RegistrationButtonTransactionDirectMenu"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_RegistrationButtonTransactionDirectMenu"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_RegistrationButtonTransactionDirectMenu"];
                            HttpContext.Current.Session["_RegistrationButtonTransactionDirectMenu"] = value;
                            return value;
                        }
                    }
                    return string.Empty;
                }

                return ((String)(HttpContext.Current.Session["_RegistrationButtonTransactionDirectMenu"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_RegistrationButtonTransactionDirectMenu"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_RegistrationButtonTransactionDirectMenu"] = value;
            }

        }

        public static string IsPORWithPriceInformation
        {
            get
            {
                if (HttpContext.Current.Session["_IsPORWithPriceInformation"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsPORWithPriceInformation"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsPORWithPriceInformation"];
                            HttpContext.Current.Session["_IsPORWithPriceInformation"] = value;
                            return value;
                        }
                    }
                    return string.Empty;
                }

                return ((String)(HttpContext.Current.Session["_IsPORWithPriceInformation"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsPORWithPriceInformation"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsPORWithPriceInformation"] = value;
            }

        }

        public static string UDDReceiptPrinterType
        {
            get
            {
                if (HttpContext.Current.Session["_UDDReceiptPrinterType"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_UDDReceiptPrinterType"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_UDDReceiptPrinterType"];
                            HttpContext.Current.Session["_UDDReceiptPrinterType"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_UDDReceiptPrinterType"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_UDDReceiptPrinterType"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_UDDReceiptPrinterType"] = value;
            }
        }

        /// <summary>
        /// Format Label Etiket UDD
        /// </summary>
        public static string UDDDrugLabelType
        {
            get
            {
                if (HttpContext.Current.Session["_UDDDrugLabelType"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_UDDDrugLabelType"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_UDDDrugLabelType"];
                            HttpContext.Current.Session["_UDDDrugLabelType"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_UDDDrugLabelType"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_UDDDrugLabelType"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_UDDDrugLabelType"] = value;
            }
        }

        public static bool IsUsedUDDDrugLabel
        {
            get
            {
                if (HttpContext.Current.Session["_IsUsedUDDDrugLabel"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedUDDDrugLabel"] != null)
                        {
                            HttpContext.Current.Session["_IsUsedUDDDrugLabel"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedUDDDrugLabel"];
                            return Convert.ToBoolean(HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedUDDDrugLabel"]);
                        }
                    }
                    return false;
                }

                return Convert.ToBoolean(HttpContext.Current.Session["_IsUsedUDDDrugLabel"]);
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsUsedUDDDrugLabel"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsUsedUDDDrugLabel"] = value;
            }
        }

        /// <summary>
        /// Pembatasan Formularium Inhealth berlaku untuk semua Modul
        /// </summary>
        public static bool IsLimitedCPOEItemForInhealth
        {
            get
            {
                if (HttpContext.Current.Session["_IsLimitedCPOEItemForInhealth"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsLimitedCPOEItemForInhealth"] != null)
                        {
                            HttpContext.Current.Session["_IsLimitedCPOEItemForInhealth"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsLimitedCPOEItemForInhealth"];
                            return Convert.ToBoolean(HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsLimitedCPOEItemForInhealth"]);
                        }
                    }
                    return false;
                }

                return Convert.ToBoolean(HttpContext.Current.Session["_IsLimitedCPOEItemForInhealth"]);
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsLimitedCPOEItemForInhealth"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsLimitedCPOEItemForInhealth"] = value;
            }
        }

        /// <summary>
        /// Pembatasan Formularium Inhealth berlaku untuk Modul Rawat Inap
        /// </summary>
        public static bool IsLimitedCPOEItemForInhealthIP
        {
            get
            {
                if (HttpContext.Current.Session["_IsLimitedCPOEItemForInhealthIP"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsLimitedCPOEItemForInhealthIP"] != null)
                        {
                            HttpContext.Current.Session["_IsLimitedCPOEItemForInhealthIP"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsLimitedCPOEItemForInhealthIP"];
                            return Convert.ToBoolean(HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsLimitedCPOEItemForInhealthIP"]);
                        }
                    }
                    return false;
                }

                return Convert.ToBoolean(HttpContext.Current.Session["_IsLimitedCPOEItemForInhealthIP"]);
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsLimitedCPOEItemForInhealthIP"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsLimitedCPOEItemForInhealthIP"] = value;
            }
        }

        /// <summary>
        /// Pembatasan Formularium Inhealth berlaku untuk Modul Rawat Jalan
        /// </summary>
        public static bool IsLimitedCPOEItemForInhealthOP
        {
            get
            {
                if (HttpContext.Current.Session["_IsLimitedCPOEItemForInhealthOP"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsLimitedCPOEItemForInhealthOP"] != null)
                        {
                            HttpContext.Current.Session["_IsLimitedCPOEItemForInhealthOP"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsLimitedCPOEItemForInhealthOP"];
                            return Convert.ToBoolean(HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsLimitedCPOEItemForInhealthOP"]);
                        }
                    }
                    return false;
                }

                return Convert.ToBoolean(HttpContext.Current.Session["_IsLimitedCPOEItemForInhealthOP"]);
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsLimitedCPOEItemForInhealthOP"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsLimitedCPOEItemForInhealthOP"] = value;
            }
        }

        /// <summary>
        /// Pembatasan Formularium Inhealth berlaku untuk Modul Rawat Darurat
        /// </summary>
        public static bool IsLimitedCPOEItemForInhealthER
        {
            get
            {
                if (HttpContext.Current.Session["_IsLimitedCPOEItemForInhealthER"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsLimitedCPOEItemForInhealthER"] != null)
                        {
                            HttpContext.Current.Session["_IsLimitedCPOEItemForInhealthER"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsLimitedCPOEItemForInhealthER"];
                            return Convert.ToBoolean(HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsLimitedCPOEItemForInhealthER"]);
                        }
                    }
                    return false;
                }

                return Convert.ToBoolean(HttpContext.Current.Session["_IsLimitedCPOEItemForInhealthER"]);
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsLimitedCPOEItemForInhealthER"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsLimitedCPOEItemForInhealthER"] = value;
            }
        }

        /// <summary>
        /// Pembatasan Formularium Inhealth berlaku untuk Modul Penunjang Medis
        /// </summary>
        public static bool IsLimitedCPOEItemForInhealthMD
        {
            get
            {
                if (HttpContext.Current.Session["_IsLimitedCPOEItemForInhealthMD"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsLimitedCPOEItemForInhealthMD"] != null)
                        {
                            HttpContext.Current.Session["_IsLimitedCPOEItemForInhealthMD"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsLimitedCPOEItemForInhealthMD"];
                            return Convert.ToBoolean(HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsLimitedCPOEItemForInhealthMD"]);
                        }
                    }
                    return false;
                }

                return Convert.ToBoolean(HttpContext.Current.Session["_IsLimitedCPOEItemForInhealthMD"]);
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsLimitedCPOEItemForInhealthMD"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsLimitedCPOEItemForInhealthMD"] = value;
            }
        }

        /// <summary>
        /// Pembatasan Formularium Inhealth berlaku untuk Modul Pharmacy
        /// </summary>
        public static bool IsLimitedCPOEItemForInhealthPH
        {
            get
            {
                if (HttpContext.Current.Session["_IsLimitedCPOEItemForInhealthPH"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsLimitedCPOEItemForInhealthPH"] != null)
                        {
                            HttpContext.Current.Session["_IsLimitedCPOEItemForInhealthPH"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsLimitedCPOEItemForInhealthPH"];
                            return Convert.ToBoolean(HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsLimitedCPOEItemForInhealthPH"]);
                        }
                    }
                    return false;
                }

                return Convert.ToBoolean(HttpContext.Current.Session["_IsLimitedCPOEItemForInhealthPH"]);
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsLimitedCPOEItemForInhealthPH"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsLimitedCPOEItemForInhealthPH"] = value;
            }
        }

        /// <summary>
        /// Pembatasan Formularium Inhealth berlaku untuk Modul Medical Checkup
        /// </summary>
        public static bool IsLimitedCPOEItemForInhealthMC
        {
            get
            {
                if (HttpContext.Current.Session["_IsLimitedCPOEItemForInhealthMC"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsLimitedCPOEItemForInhealthMC"] != null)
                        {
                            HttpContext.Current.Session["_IsLimitedCPOEItemForInhealthMC"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsLimitedCPOEItemForInhealthMC"];
                            return Convert.ToBoolean(HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsLimitedCPOEItemForInhealthMC"]);
                        }
                    }
                    return false;
                }

                return Convert.ToBoolean(HttpContext.Current.Session["_IsLimitedCPOEItemForInhealthMC"]);
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsLimitedCPOEItemForInhealthMC"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsLimitedCPOEItemForInhealthMC"] = value;
            }
        }

        public static string BusinessPartnerIDInhealth
        {
            get
            {
                if (HttpContext.Current.Session["_BusinessPartnerIDInhealth"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_BusinessPartnerIDInhealth"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_BusinessPartnerIDInhealth"];
                            HttpContext.Current.Session["_BusinessPartnerIDInhealth"] = value;
                            return value;
                        }
                    }
                    return "0";
                }
                return ((String)(HttpContext.Current.Session["_BusinessPartnerIDInhealth"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_BusinessPartnerIDInhealth"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_BusinessPartnerIDInhealth"] = value;
            }

        }

        public static string TipeCustomerBPJS
        {
            get
            {
                if (HttpContext.Current.Session["_TipeCustomerBPJS"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_TipeCustomerBPJS"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_TipeCustomerBPJS"];
                            HttpContext.Current.Session["_TipeCustomerBPJS"] = value;
                            return value;
                        }
                    }
                    return "0";
                }
                return ((String)(HttpContext.Current.Session["_TipeCustomerBPJS"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_TipeCustomerBPJS"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_TipeCustomerBPJS"] = value;
            }

        }

        public static bool IsUsedInpatientPrescriptionTypeFilter
        {
            get
            {
                if (HttpContext.Current.Session["_IsUsedInpatientPrescriptionTypeFilter"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedInpatientPrescriptionTypeFilter"] != null)
                        {
                            HttpContext.Current.Session["_IsUsedInpatientPrescriptionTypeFilter"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedInpatientPrescriptionTypeFilter"];
                            return Convert.ToBoolean(HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsUsedInpatientPrescriptionTypeFilter"]);
                        }
                    }
                    return false;
                }

                return Convert.ToBoolean(HttpContext.Current.Session["_IsUsedInpatientPrescriptionTypeFilter"]);
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsUsedInpatientPrescriptionTypeFilter"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsUsedInpatientPrescriptionTypeFilter"] = value;
            }
        }

        public static string InpatientPrescriptionTypeFilter
        {
            get
            {
                if (HttpContext.Current.Session["_InpatientPrescriptionTypeFilter"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_InpatientPrescriptionTypeFilter"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_InpatientPrescriptionTypeFilter"];
                            HttpContext.Current.Session["_InpatientPrescriptionTypeFilter"] = value;
                            return value;
                        }
                    }
                    return "0";
                }

                return ((String)(HttpContext.Current.Session["_InpatientPrescriptionTypeFilter"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_InpatientPrescriptionTypeFilter"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_InpatientPrescriptionTypeFilter"] = value;
            }

        }

        public static string DefaultDaysPRMRJ
        {
            get
            {
                if (HttpContext.Current.Session["_DefaultDaysPRMRJ"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_DefaultDaysPRMRJ"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_DefaultDaysPRMRJ"];
                            HttpContext.Current.Session["_DefaultDaysPRMRJ"] = value;
                            return value;
                        }
                    }
                    return "0";
                }

                return ((String)(HttpContext.Current.Session["_DefaultDaysPRMRJ"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_DefaultDaysPRMRJ"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_DefaultDaysPRMRJ"] = value;
            }

        }

        public static bool IsPrintPHOrderTracerFromEMR
        {
            get
            {
                if (HttpContext.Current.Session["_IsPrintPHOrderTracerFromEMR"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsPrintPHOrderTracerFromEMR"] != null)
                        {
                            HttpContext.Current.Session["_IsPrintPHOrderTracerFromEMR"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsPrintPHOrderTracerFromEMR"];
                            return Convert.ToBoolean(HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IsPrintPHOrderTracerFromEMR"]);
                        }
                    }
                    return false;
                }

                return Convert.ToBoolean(HttpContext.Current.Session["_IsPrintPHOrderTracerFromEMR"]);
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IsPrintPHOrderTracerFromEMR"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IsPrintPHOrderTracerFromEMR"] = value;
            }
        }

        /// <summary>
        /// Jumlah Hari Kebutuhan Pembaharuan Awal Asesmen Pasien Rawat Jalan
        /// </summary>
        public static string OP0027
        {
            get
            {
                if (HttpContext.Current.Session["_OP0027"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_OP0027"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_OP0027"];
                            HttpContext.Current.Session["_OP0027"] = value;
                            return value;
                        }
                    }
                    return "0";
                }

                return ((String)(HttpContext.Current.Session["_OP0027"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_OP0027"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_OP0027"] = value;
            }

        }

        /// <summary>
        /// Format Layout Form Elektronik : Pemeriksaan Fisik
        /// </summary>
        public static string OP0028
        {
            get
            {
                if (HttpContext.Current.Session["_OP0028"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_OP0028"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_OP0028"];
                            HttpContext.Current.Session["_OP0028"] = value;
                            return value;
                        }
                    }
                    return "0";
                }

                return ((String)(HttpContext.Current.Session["_OP0028"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_OP0028"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_OP0028"] = value;
            }

        }

        /// <summary>
        /// Format Layout Form Elektronik : Psikososial dan Spiritual
        /// </summary>
        public static string OP0029
        {
            get
            {
                if (HttpContext.Current.Session["_OP0029"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_OP0029"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_OP0029"];
                            HttpContext.Current.Session["_OP0029"] = value;
                            return value;
                        }
                    }
                    return "0";
                }

                return ((String)(HttpContext.Current.Session["_OP0029"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_OP0029"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_OP0029"] = value;
            }

        }

        /// <summary>
        /// Format Layout Form Elektronik : Kebutuhan Edukasi
        /// </summary>
        public static string OP0030
        {
            get
            {
                if (HttpContext.Current.Session["_OP0030"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_OP0030"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_OP0030"];
                            HttpContext.Current.Session["_OP0030"] = value;
                            return value;
                        }
                    }
                    return "0";
                }

                return ((String)(HttpContext.Current.Session["_OP0030"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_OP0030"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_OP0030"] = value;
            }

        }

        /// <summary>
        /// Format Layout Form Elektronik : Pemeriksaan Fisik
        /// </summary>
        public static string IP0024
        {
            get
            {
                if (HttpContext.Current.Session["_IP0024"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IP0024"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IP0024"];
                            HttpContext.Current.Session["_IP0024"] = value;
                            return value;
                        }
                    }
                    return "0";
                }

                return ((String)(HttpContext.Current.Session["_IP0024"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IP0024"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IP0024"] = value;
            }

        }

        /// <summary>
        /// Format Layout Form Elektronik : Psikososial dan Spiritual
        /// </summary>
        public static string IP0025
        {
            get
            {
                if (HttpContext.Current.Session["_IP0025"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IP0025"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IP0025"];
                            HttpContext.Current.Session["_IP0025"] = value;
                            return value;
                        }
                    }
                    return "0";
                }

                return ((String)(HttpContext.Current.Session["_IP0025"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IP0025"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IP0025"] = value;
            }

        }

        /// <summary>
        /// Format Layout Form Elektronik : Psikososial dan Spiritual
        /// </summary>
        public static string IP0026
        {
            get
            {
                if (HttpContext.Current.Session["_IP0026"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IP0026"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IP0026"];
                            HttpContext.Current.Session["_IP0026"] = value;
                            return value;
                        }
                    }
                    return "0";
                }

                return ((String)(HttpContext.Current.Session["_IP0026"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IP0026"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IP0026"] = value;
            }

        }

        /// <summary>
        /// Format Layout Form Elektronik : Perencanaan Pasien Pulang
        /// </summary>
        public static string IP0027
        {
            get
            {
                if (HttpContext.Current.Session["_IP0027"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IP0027"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_IP0027"];
                            HttpContext.Current.Session["_IP0027"] = value;
                            return value;
                        }
                    }
                    return "0";
                }

                return ((String)(HttpContext.Current.Session["_IP0027"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_IP0027"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_IP0027"] = value;
            }

        }

        /// <summary>
        /// Format Layout Form Elektronik : Pemeriksaan Fisik
        /// </summary>
        public static string MD0012
        {
            get
            {
                if (HttpContext.Current.Session["_MD0012"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_MD0012"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_MD0012"];
                            HttpContext.Current.Session["_MD0012"] = value;
                            return value;
                        }
                    }
                    return "0";
                }

                return ((String)(HttpContext.Current.Session["_MD0012"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_MD0012"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_MD0012"] = value;
            }

        }

        /// <summary>
        /// Format Layout Form Elektronik : Psikososial dan Spiritual
        /// </summary>
        public static string MD0013
        {
            get
            {
                if (HttpContext.Current.Session["_MD0013"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_MD0013"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_MD0013"];
                            HttpContext.Current.Session["_MD0013"] = value;
                            return value;
                        }
                    }
                    return "0";
                }

                return ((String)(HttpContext.Current.Session["_MD0013"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_MD0013"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_MD0013"] = value;
            }

        }

        /// <summary>
        /// Kode Unit Pelayanan Kamar Operasi
        /// </summary>
        public static string MD0006
        {
            get
            {
                if (HttpContext.Current.Session["_MD0006"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_MD0006"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_MD0006"];
                            HttpContext.Current.Session["_MD0006"] = value;
                            return value;
                        }
                    }
                    return "0";
                }

                return ((String)(HttpContext.Current.Session["_MD0006"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_MD0006"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_MD0006"] = value;
            }

        }

        /// <summary>
        /// Format Layout Form Elektronik : Kebutuhan Edukasi
        /// </summary>
        public static string MD0014
        {
            get
            {
                if (HttpContext.Current.Session["_MD0014"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_MD0014"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_MD0014"];
                            HttpContext.Current.Session["_MD0014"] = value;
                            return value;
                        }
                    }
                    return "0";
                }

                return ((String)(HttpContext.Current.Session["_MD0014"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_MD0014"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_MD0014"] = value;
            }

        }

        /// <summary>
        /// Unit Pelayanan One-Day Surgery
        /// </summary>
        public static string MD0016
        {
            get
            {
                if (HttpContext.Current.Session["_MD0016"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_MD0016"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_MD0016"];
                            HttpContext.Current.Session["_MD0016"] = value;
                            return value;
                        }
                    }
                    return "0";
                }

                return ((String)(HttpContext.Current.Session["_MD0016"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_MD0016"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_MD0016"] = value;
            }

        }

        /// <summary>
        /// Default Kode Tindakan Kamar Operasi
        /// </summary>
        public static string MD0017
        {
            get
            {
                if (HttpContext.Current.Session["_MD0017"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_MD0017"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_MD0017"];
                            HttpContext.Current.Session["_MD0017"] = value;
                            return value;
                        }
                    }
                    return "0";
                }

                return ((String)(HttpContext.Current.Session["_MD0017"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_MD0017"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_MD0017"] = value;
            }

        }

        /// <summary>
        /// Kode Unit Pelayanan Bank Darah
        /// </summary>
        public static string MD0018
        {
            get
            {
                if (HttpContext.Current.Session["_MD0018"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_MD0018"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_MD0018"];
                            HttpContext.Current.Session["_MD0018"] = value;
                            return value;
                        }
                    }
                    return "0";
                }

                return ((String)(HttpContext.Current.Session["_MD0018"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_MD0018"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_MD0018"] = value;
            }

        }

        /// <summary>
        /// Adverse Reaction diperlakukan sebagai stopper (1 = YA; 0 = TIDAK)
        /// </summary>
        public static string PH0006
        {
            get
            {
                if (HttpContext.Current.Session["_PH0006"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_PH0006"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_PH0006"];
                            HttpContext.Current.Session["_PH0006"] = value;
                            return value;
                        }
                    }
                    return "0";
                }

                return ((String)(HttpContext.Current.Session["_PH0006"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_PH0006"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_PH0006"] = value;
            }
        }

        /// <summary>
        /// Batas Maksimum Jumlah Hari Pemberian Obat Narkotika/Psikotropika
        /// </summary>
        public static string PH0007
        {
            get
            {
                if (HttpContext.Current.Session["_PH0007"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_PH0007"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_PH0007"];
                            HttpContext.Current.Session["_PH0007"] = value;
                            return value;
                        }
                    }
                    return "0";
                }

                return ((String)(HttpContext.Current.Session["_PH0007"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_PH0007"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_PH0007"] = value;
            }
        }

        /// <summary>
        /// Kontrol Duplikasi Terapi (1 = YA; 0 = TIDAK)
        /// </summary>
        public static string PH0008
        {
            get
            {
                if (HttpContext.Current.Session["_PH0008"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_PH0008"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_PH0008"];
                            HttpContext.Current.Session["_PH0008"] = value;
                            return value;
                        }
                    }
                    return "0";
                }

                return ((String)(HttpContext.Current.Session["_PH0008"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_PH0008"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_PH0008"] = value;
            }
        }

        /// <summary>
        /// Alergi Obat diperlakukan sebagai stopper (1 = YA; 0 = TIDAK)
        /// </summary>
        public static string PH0033
        {
            get
            {
                if (HttpContext.Current.Session["_PH0033"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_PH0033"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_PH0033"];
                            HttpContext.Current.Session["_PH0033"] = value;
                            return value;
                        }
                    }
                    return "0";
                }

                return ((String)(HttpContext.Current.Session["_PH0033"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_PH0033"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_PH0033"] = value;
            }
        }

        /// <summary>
        /// Alamat Web-Service (API) Dinas Kesehatan untuk Informasi Tempat Tidur
        /// </summary>
        public static string SA0111
        {
            get
            {
                if (HttpContext.Current.Session["_SA0111"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0111"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0111"];
                            HttpContext.Current.Session["_SA0111"] = value;
                            return value;
                        }
                    }
                    return "0";
                }

                return ((String)(HttpContext.Current.Session["_SA0111"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_SA0111"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_SA0111"] = value;
            }
        }

        /// <summary>
        /// Kode Rumah Sakit yang diberikan oleh Dinas Kesehatan Provinsi
        /// </summary>
        public static string SA0112
        {
            get
            {
                if (HttpContext.Current.Session["_SA0112"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0112"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0112"];
                            HttpContext.Current.Session["_SA0112"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_SA0112"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_SA0112"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_SA0112"] = value;
            }
        }

        /// <summary>
        /// 	Unique Key yang diberikan oleh Dinas Kesehatan Provinsi untuk masing-masing Rumah Sakit
        /// </summary>
        public static string SA0113
        {
            get
            {
                if (HttpContext.Current.Session["_SA0113"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0113"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0113"];
                            HttpContext.Current.Session["_SA0113"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_SA0113"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_SA0113"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_SA0113"] = value;
            }
        }

        /// <summary>
        /// Kode Akses Rumah Sakit untuk Web Service SiRanap (ConsID)
        /// </summary>
        public static string SA0126
        {
            get
            {
                if (HttpContext.Current.Session["_SA0126"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0126"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0126"];
                            HttpContext.Current.Session["_SA0126"] = value;
                            return value;
                        }
                    }
                    return "0";
                }

                return ((String)(HttpContext.Current.Session["_SA0126"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_SA0126"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_SA0126"] = value;
            }
        }

        /// <summary>
        /// Password Kode Akses Rumah Sakit untuk Web Service SiRanap (Pass)
        /// </summary>
        public static string SA0127
        {
            get
            {
                if (HttpContext.Current.Session["_SA0127"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0127"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0127"];
                            HttpContext.Current.Session["_SA0127"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_SA0127"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_SA0127"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_SA0127"] = value;
            }
        }

        /// <summary>
        /// 	Alamat Web Service SiRanap (Url)
        /// </summary>
        public static string SA0128
        {
            get
            {
                if (HttpContext.Current.Session["_SA0128"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0128"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0128"];
                            HttpContext.Current.Session["_SA0128"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_SA0128"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_SA0128"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_SA0128"] = value;
            }
        }


        /// <summary>
        /// 	Kode Tanda Vital EWS
        /// </summary>
        public static string EM0010
        {
            get
            {
                if (HttpContext.Current.Session["_EM0010"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_EM0010"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_EM0010"];
                            HttpContext.Current.Session["_EM0010"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_EM0010"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_EM0010"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_EM0010"] = value;
            }
        }

        /// <summary>
        /// 	Validasi Pengisian Kajian Awal ketika Pengisian CPPT oleh DPJP
        /// </summary>
        public static string EM0034
        {
            get
            {
                if (HttpContext.Current.Session["_EM0034"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_EM0034"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_EM0034"];
                            HttpContext.Current.Session["_EM0034"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_EM0034"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_EM0034"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_EM0034"] = value;
            }
        }

        /// <summary>
        /// 	Previous Patient List untuk Emergency dilakukan pembatasan akses untuk Dokter Emergency
        /// </summary>
        public static string EM0036
        {
            get
            {
                if (HttpContext.Current.Session["_EM0036"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_EM0036"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_EM0036"];
                            HttpContext.Current.Session["_EM0036"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_EM0036"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_EM0036"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_EM0036"] = value;
            }
        }

        /// <summary>
        /// 	Default Interval Pemantauan Tanda Vital dan Indikator lainnya untuk Pengkajian Status Anestesi (dalam menit)
        /// </summary>
        public static string EM0049
        {
            get
            {
                if (HttpContext.Current.Session["_EM0049"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_EM0049"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_EM0049"];
                            HttpContext.Current.Session["_EM0049"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_EM0049"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_EM0049"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_EM0049"] = value;
            }
        }

        /// <summary>
        /// 	Default Interval Pemantauan Tanda Vital dan Indikator lainnya untuk Pengkajian Intra Hemodialisis (dalam menit)
        /// </summary>
        public static string EM0050
        {
            get
            {
                if (HttpContext.Current.Session["_EM0050"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_EM0050"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_EM0050"];
                            HttpContext.Current.Session["_EM0050"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_EM0050"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_EM0050"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_EM0050"] = value;
            }
        }

        /// <summary>
        /// 	ID HealthcareserviceUnit yang dipilih di halaman list pasien
        /// </summary>
        public static string HealthcareServiceUnitID
        {
            get
            {
                if (HttpContext.Current.Session["_ListHealthcareServiceUnitID"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_ListHealthcareServiceUnitID"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_ListHealthcareServiceUnitID"];
                            HttpContext.Current.Session["_ListHealthcareServiceUnitID"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_ListHealthcareServiceUnitID"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_ListHealthcareServiceUnitID"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_ListHealthcareServiceUnitID"] = value;
            }
        }

        /// <summary>
        ///   Bridging dengan Pharmacogenomic Test Report (Nalagenetic) (1 = YA; 0 = TIDAK)
        /// </summary>
        public static string SA0119
        {
            get
            {
                if (HttpContext.Current.Session["_SA0119"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0119"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0119"];
                            HttpContext.Current.Session["_SA0119"] = value;
                            return value;
                        }
                    }
                    return "0";
                }

                return ((String)(HttpContext.Current.Session["_SA0119"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_SA0119"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_SA0119"] = value;
            }
        }

        /// <summary>
        ///    API Key untuk Bridging Pharmacogenomic Test Report (Nalagenetic)
        /// </summary>
        public static string SA0120
        {
            get
            {
                if (HttpContext.Current.Session["_SA01209"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0120"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0120"];
                            HttpContext.Current.Session["_SA0120"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_SA0120"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_SA0120"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_SA0120"] = value;
            }
        }

        /// <summary>
        ///   Alamat API Pharmacogenomic Test Report (Nalagenetic)
        /// </summary>
        public static string SA0121
        {
            get
            {
                if (HttpContext.Current.Session["_SA0121"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0121"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0121"];
                            HttpContext.Current.Session["_SA0121"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_SA0121"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_SA0121"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_SA0121"] = value;
            }
        }

        /// <summary>
        ///   Alamat email untuk keperluan Bridging dengan Pharmacogenomic Test
        /// </summary>
        public static string SA0129
        {
            get
            {
                if (HttpContext.Current.Session["_SA0129"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0129"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0129"];
                            HttpContext.Current.Session["_SA0129"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_SA0129"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_SA0129"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_SA0129"] = value;
            }
        }

        /// <summary>
        ///   Consumer ID untuk mengakses WEB API Center-back API
        /// </summary>
        public static string SA0130
        {
            get
            {
                if (HttpContext.Current.Session["_SA0130"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0130"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0130"];
                            HttpContext.Current.Session["_SA0130"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_SA0130"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_SA0130"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_SA0130"] = value;
            }
        }

        /// <summary>
        ///   Consumer Password untuk mengakses WEB API Center-back API
        /// </summary>
        public static string SA0131
        {
            get
            {
                if (HttpContext.Current.Session["_SA0131"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0131"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0131"];
                            HttpContext.Current.Session["_SA0131"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_SA0131"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_SA0131"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_SA0131"] = value;
            }
        }

        /// <summary>
        ///   Url/Alamat WEB API Center-back API
        /// </summary>
        public static string SA0132
        {
            get
            {
                if (HttpContext.Current.Session["_SA0132"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0132"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0132"];
                            HttpContext.Current.Session["_SA0132"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_SA0132"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_SA0132"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_SA0132"] = value;
            }
        }

        /// <summary>
        ///   Center-back API Consumer
        /// </summary>
        public static string SA0133
        {
            get
            {
                if (HttpContext.Current.Session["_SA0133"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0133"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0133"];
                            HttpContext.Current.Session["_SA0133"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_SA0133"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_SA0133"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_SA0133"] = value;
            }
        }

        /// <summary>
        ///   Is Bridging to Medifras V1
        /// </summary>
        public static string SA0137
        {
            get
            {
                if (HttpContext.Current.Session["_SA0137"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0137"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0137"];
                            HttpContext.Current.Session["_SA0137"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_SA0137"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_SA0137"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_SA0137"] = value;
            }
        }

        /// <summary>
        ///   Bridging dengan Mobile JKN
        /// </summary>
        public static string SA0138
        {
            get
            {
                if (HttpContext.Current.Session["_SA0138"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0138"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0138"];
                            HttpContext.Current.Session["_SA0138"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_SA0138"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_SA0138"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_SA0138"] = value;
            }
        }

        /// <summary>
        ///   Url/Alamat API Mobile JKN
        /// </summary>
        public static string SA0139
        {
            get
            {
                if (HttpContext.Current.Session["_SA0139"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0139"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0139"];
                            HttpContext.Current.Session["_SA0139"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_SA0139"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_SA0139"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_SA0139"] = value;
            }
        }

        /// <summary>
        ///   Consumer ID untuk mengakses Web API Mobile JKN
        /// </summary>
        public static string SA0140
        {
            get
            {
                if (HttpContext.Current.Session["_SA0140"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0140"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0140"];
                            HttpContext.Current.Session["_SA0140"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_SA0140"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_SA0140"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_SA0140"] = value;
            }
        }

        /// <summary>
        ///   Consumer Password untuk mengakses Web API Mobile JKN
        /// </summary>
        public static string SA0141
        {
            get
            {
                if (HttpContext.Current.Session["_SA0141"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0141"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0141"];
                            HttpContext.Current.Session["_SA0141"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_SA0141"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_SA0141"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_SA0141"] = value;
            }
        }

        /// <summary>
        ///   	Version Bridging BPJS V-Klaim
        /// </summary>
        public static string SA0167
        {
            get
            {
                if (HttpContext.Current.Session["_SA0167"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0167"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0167"];
                            HttpContext.Current.Session["_SA0167"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_SA0167"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_SA0167"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_SA0167"] = value;
            }
        }

        /// <summary>
        ///   	Mobile JKN otomatis kirim ke BPJS Task Log
        /// </summary>
        public static string SA0175
        {
            get
            {
                if (HttpContext.Current.Session["_SA0175"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0175"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0175"];
                            HttpContext.Current.Session["_SA0175"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_SA0175"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_SA0175"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_SA0175"] = value;
            }
        }

        /// <summary>
        ///   	Validasi Assessment Pra Bedah ketika pengisian Assesment Pra Anestesi
        /// </summary>
        public static string EM0046
        {
            get
            {
                if (HttpContext.Current.Session["_EM0046"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_EM0046"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_EM0046"];
                            HttpContext.Current.Session["_EM0046"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_EM0046"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_EM0046"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_EM0046"] = value;
            }
        }

        /// <summary>
        ///   	Proses Pengisian Catatan Pemberian Obat menggunakan Proses Scan QRCode Gelang dan Label Dispensing / Bukti Penyerahan Obat
        /// </summary>
        public static string NR0001
        {
            get
            {
                if (HttpContext.Current.Session["_NR0001"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_NR0001"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_NR0001"];
                            HttpContext.Current.Session["_NR0001"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_NR0001"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_NR0001"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_NR0001"] = value;
            }
        }

        /// <summary>
        ///   	Default Pilihan Tampilan Transaksi Pasien di Menu Doctor Fee menggunakan ALL
        /// </summary>
        public static string EM0051
        {
            get
            {
                if (HttpContext.Current.Session["_EM0051"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_EM0051"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_EM0051"];
                            HttpContext.Current.Session["_EM0051"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_EM0051"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_EM0051"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_EM0051"] = value;
            }
        }

        /// <summary>
        ///   	Is Bridging To IPTV
        /// </summary>
        public static bool Is_Bridging_To_IPTV
        {
            get
            {
                if (HttpContext.Current.Session["_Is_Bridging_To_IPTV"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_Is_Bridging_To_IPTV"] != null)
                        {
                            HttpContext.Current.Session["_Is_Bridging_To_IPTV"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_Is_Bridging_To_IPTV"];
                            return Convert.ToBoolean(HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_Is_Bridging_To_IPTV"]);
                        }
                    }
                    return false;
                }

                return Convert.ToBoolean(HttpContext.Current.Session["_Is_Bridging_To_IPTV"]);
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_Is_Bridging_To_IPTV"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_Is_Bridging_To_IPTV"] = value;
            }
        }

        /// <summary>
        ///   	Generate Nomor Referensi Hasil secara otomatis untuk Pemeriksaan Patologi Anatomy
        /// </summary>
        public static string LB0034
        {
            get
            {
                if (HttpContext.Current.Session["_LB0034"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_LB0034"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_LB0034"];
                            HttpContext.Current.Session["_LB0034"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_LB0034"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_LB0034"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_LB0034"] = value;
            }
        }

        /// <summary>
        ///   	Prefix Nomor Referensi Hasil Pemeriksaan Patologi Anatomy
        /// </summary>
        public static string LB0035
        {
            get
            {
                if (HttpContext.Current.Session["_LB0035"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_LB0035"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_LB0035"];
                            HttpContext.Current.Session["_LB0035"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_LB0035"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_LB0035"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_LB0035"] = value;
            }
        }

        /// <summary>
        ///   	Rekonsiliasi Obat otomatis membuat Jadwal Pemberian Obat
        /// </summary>
        public static string PH0070
        {
            get
            {
                if (HttpContext.Current.Session["_PH0070"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_PH0070"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_PH0070"];
                            HttpContext.Current.Session["_PH0070"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_PH0070"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_PH0070"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_PH0070"] = value;
            }
        }

        /// <summary>
        ///   	Organization ID Rumah Sakit untuk Integrasi dengan SATUSEHAT
        /// </summary>
        public static string SA0197
        {
            get
            {
                if (HttpContext.Current.Session["_SA0197"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0197"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0197"];
                            HttpContext.Current.Session["_SA0197"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_SA0197"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_SA0197"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_SA0197"] = value;
            }
        }

        /// <summary>
        ///   Client ID Rumah Sakit untuk Integrasi dengan SATUSEHAT
        /// </summary>
        public static string SA0194
        {
            get
            {
                if (HttpContext.Current.Session["_SA0194"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0194"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0194"];
                            HttpContext.Current.Session["_SA0194"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_SA0194"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_SA0194"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_SA0194"] = value;
            }
        }

        /// <summary>
        ///   Client Secret Rumah Sakit untuk Integrasi dengan SATUSEHAT
        /// </summary>
        public static string SA0195
        {
            get
            {
                if (HttpContext.Current.Session["_SA0195"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0195"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0195"];
                            HttpContext.Current.Session["_SA0195"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_SA0195"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_SA0195"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_SA0195"] = value;
            }
        }

        /// <summary>
        ///   Base Url API SATUSEHAT
        /// </summary>
        public static string SA0196
        {
            get
            {
                if (HttpContext.Current.Session["_SA0196"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0196"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0196"];
                            HttpContext.Current.Session["_SA0196"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_SA0196"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_SA0196"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_SA0196"] = value;
            }
        }

        /// <summary>
        ///   Menggunakan Paket Kunjungan atau tidak?
        /// </summary>
        public static bool SA0198
        {
            get
            {
                if (HttpContext.Current.Session["SA0198"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["SA0198"] != null)
                        {
                            HttpContext.Current.Session["SA0198"] = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["SA0198"];
                            return Convert.ToBoolean(HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["SA0198"]);
                        }
                    }
                    return false;
                }

                return Convert.ToBoolean(HttpContext.Current.Session["SA0198"]);
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["SA0198"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["SA0198"] = value;
            }
        }

        /// <summary>
        ///   Default kadaluarsa untuk item pake kunjungan (dalam minggu)
        /// </summary>
        public static string SA0199
        {
            get
            {
                if (HttpContext.Current.Session["_SA0199"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0199"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0199"];
                            HttpContext.Current.Session["_SA0199"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_SA0199"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_SA0199"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_SA0199"] = value;
            }
        }

        /// <summary>
        ///   MEDINFRAS API Consumer ID
        /// </summary>
        public static string SA0058
        {
            get
            {
                if (HttpContext.Current.Session["_SA0058"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0058"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0058"];
                            HttpContext.Current.Session["_SA0058"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_SA0058"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_SA0058"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_SA0058"] = value;
            }
        }

        /// <summary>
        ///   MEDINFRAS API Consumer ID
        /// </summary>
        public static string SA0059
        {
            get
            {
                if (HttpContext.Current.Session["_SA0059"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0059"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0059"];
                            HttpContext.Current.Session["_SA0059"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_SA0059"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_SA0059"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_SA0059"] = value;
            }
        }

        public static string SA0200
        {
            get
            {
                if (HttpContext.Current.Session["_SA0200"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0200"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0200"];
                            HttpContext.Current.Session["_SA0200"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_SA0200"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_SA0200"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_SA0200"] = value;
            }
        }
        public static string SA0201
        {
            get
            {
                if (HttpContext.Current.Session["_SA0201"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0201"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_SA0201"];
                            HttpContext.Current.Session["_SA0201"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_SA0201"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_SA0201"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_SA0201"] = value;
            }
        }

        public static string ReportFooterPrintedByInfo
        {
            get
            {
                if (HttpContext.Current.Session["_ReportFooterPrintedByInfo"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_ReportFooterPrintedByInfo"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_ReportFooterPrintedByInfo"];
                            HttpContext.Current.Session["_ReportFooterPrintedByInfo"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_ReportFooterPrintedByInfo"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_ReportFooterPrintedByInfo"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_ReportFooterPrintedByInfo"] = value;
            }
        }

        public static string RegistrationOrderBy_ER
        {
            get
            {
                if (HttpContext.Current.Session["_RegistrationOrderBy_ER"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_RegistrationOrderBy_ER"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_RegistrationOrderBy_ER"];
                            HttpContext.Current.Session["_RegistrationOrderBy_ER"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_RegistrationOrderBy_ER"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_RegistrationOrderBy_ER"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_RegistrationOrderBy_ER"] = value;
            }
        }

        public static string RegistrationOrderBy_MC
        {
            get
            {
                if (HttpContext.Current.Session["_RegistrationOrderBy_MC"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_RegistrationOrderBy_MC"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_RegistrationOrderBy_MC"];
                            HttpContext.Current.Session["_RegistrationOrderBy_MC"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_RegistrationOrderBy_MC"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_RegistrationOrderBy_MC"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_RegistrationOrderBy_MC"] = value;
            }
        }

        public static string RegistrationOrderBy_MD
        {
            get
            {
                if (HttpContext.Current.Session["_RegistrationOrderBy_MD"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_RegistrationOrderBy_MD"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_RegistrationOrderBy_MD"];
                            HttpContext.Current.Session["_RegistrationOrderBy_MD"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_RegistrationOrderBy_MD"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_RegistrationOrderBy_MD"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_RegistrationOrderBy_MD"] = value;
            }
        }

        public static string RegistrationOrderBy_PH
        {
            get
            {
                if (HttpContext.Current.Session["_RegistrationOrderBy_PH"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_RegistrationOrderBy_PH"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_RegistrationOrderBy_PH"];
                            HttpContext.Current.Session["_RegistrationOrderBy_PH"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_RegistrationOrderBy_PH"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_RegistrationOrderBy_PH"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_RegistrationOrderBy_PH"] = value;
            }
        }

        /// <summary>
        /// 	Apakah Notifikasi Catatan Terintegrasi yang harus dikonfirmasi oleh DPJP menjadi blocker atau hanya warning saja ?
        /// </summary>
        public static string EM0058
        {
            get
            {
                if (HttpContext.Current.Session["_EM0058"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_EM0058"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_EM0058"];
                            HttpContext.Current.Session["_EM0058"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_EM0058"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_EM0058"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_EM0058"] = value;
            }
        }

        /// <summary>
        ///   	Order Pemeriksaan Lab menggunakan Form pemisahan antara Pemeriksaan Non-CITO dan CITO
        /// </summary>
        public static string EM0072
        {
            get
            {
                if (HttpContext.Current.Session["_EM0072"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_EM0072"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_EM0072"];
                            HttpContext.Current.Session["_EM0072"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_EM0072"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_EM0072"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_EM0072"] = value;
            }
        }

        /// <summary>
        ///   	Catatan Klinis untuk Order Radiologi otomatis terisi pada saat pengentrian order
        /// </summary>
        public static string EM0079
        {
            get
            {
                if (HttpContext.Current.Session["_EM0079"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_EM0079"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_EM0079"];
                            HttpContext.Current.Session["_EM0079"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_EM0079"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_EM0079"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_EM0079"] = value;
            }
        }

        /// <summary>
        /// 	Kode Unit Pelayanan Radioterapi
        /// </summary>
        public static string RT0001
        {
            get
            {
                if (HttpContext.Current.Session["_RT0001"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_RT0001"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_RT0001"];
                            HttpContext.Current.Session["_RT0001"] = value;
                            return value;
                        }
                    }
                    return "";
                }

                return ((String)(HttpContext.Current.Session["_RT0001"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_RT0001"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_RT0001"] = value;
            }
        }

        /// <summary>
        /// 	Notifikasi Pengisian Form Program Pengendalian Resistensi Antimikroba (PPRA) ketika Pengiriman Resep Online Dokter ke Farmasi 
        /// </summary>
        public static string EM0088
        {
            get
            {
                if (HttpContext.Current.Session["_EM0088"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_EM0088"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_EM0088"];
                            HttpContext.Current.Session["_EM0088"] = value;
                            return value;
                        }
                    }
                    return "0";
                }

                return ((String)(HttpContext.Current.Session["_EM0088"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_EM0088"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_EM0088"] = value;
            }
        }

        /// <summary>
        /// 	Jenis Kunjungan Program Bayi Tabung 
        /// </summary>
        public static string EM0090
        {
            get
            {
                if (HttpContext.Current.Session["_EM0090"] == null)
                {
                    if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                    {
                        if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_EM0090"] != null)
                        {
                            String value = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME]["_EM0090"];
                            HttpContext.Current.Session["_EM0090"] = value;
                            return value;
                        }
                    }
                    return "0";
                }

                return ((String)(HttpContext.Current.Session["_EM0090"]));
            }
            set
            {
                if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
                {
                    HttpCookie myCookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                    myCookie.Values["_EM0090"] = value.ToString();
                    HttpContext.Current.Response.Cookies.Add(myCookie);
                }
                HttpContext.Current.Session["_EM0090"] = value;
            }
        }
    }
}
