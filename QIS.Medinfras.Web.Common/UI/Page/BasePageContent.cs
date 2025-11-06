using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Xml.Linq;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.Common.UI
{
    public abstract class BasePageContent : BasePage
    {
        public Repeater _RptTasks
        {
            get
            {
                Control ctlGeneral = Helper.FindControlRecursive(Master, "ctlGeneral");
                return (Repeater)Helper.FindControlRecursive(ctlGeneral, "rptTasks");
            }
        }
        public Repeater _RptInformation
        {
            get
            {
                Control ctlGeneral = Helper.FindControlRecursive(Master, "ctlGeneral");
                return (Repeater)Helper.FindControlRecursive(ctlGeneral, "rptInformation");
            }
        }
        public Repeater _RptPrint
        {
            get
            {
                Control ctlGeneral = Helper.FindControlRecursive(Master, "ctlGeneral");
                return (Repeater)Helper.FindControlRecursive(ctlGeneral, "rptPrint");
            }
        }

        public TextBox _txtJmlLabel
        {
            get
            {
                Control txtJmlLabel = Helper.FindControlRecursive(Master, "txtJmlLabel");
                return (TextBox)Helper.FindControlRecursive(txtJmlLabel, "txtJmlLabel");
            }
        }

        public ASPxComboBox _cboPrinterUrl
        {
            get
            {
                Control cboPrinterUrl = Helper.FindControlRecursive(Master, "cboPrinterUrl");
                return (ASPxComboBox)Helper.FindControlRecursive(cboPrinterUrl, "cboPrinterUrl");
            }
        }

        public HtmlTableCell _tdPrinterLocation
        {
            get
            {
                Control tdPrinterLocation = Helper.FindControlRecursive(Master, "tdPrinterLocation");
                return (HtmlTableCell)Helper.FindControlRecursive(tdPrinterLocation, "tdPrinterLocation");
            }
        }

        public HtmlInputHidden _hdnIsMultiLocation
        {
            get
            {
                Control hdnIsMultiLocation = Helper.FindControlRecursive(Master, "hdnIsMultiLocation");
                return (HtmlInputHidden)Helper.FindControlRecursive(hdnIsMultiLocation, "hdnIsMultiLocation");
            }
        }

        public HtmlInputHidden _hdnMaxLabelNo
        {
            get
            {
                Control hdnMaxLabelNo = Helper.FindControlRecursive(Master, "hdnMaxLabelNo");
                return (HtmlInputHidden)Helper.FindControlRecursive(hdnMaxLabelNo, "hdnMaxLabelNo");
            }
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            if (!Page.IsPostBack)
            {
                string menuCode = OnGetMenuCode();
                bool isShowRightPanel = IsShowRightPanel();

                if (menuCode != "" && isShowRightPanel)
                {
                    Repeater rptTasks = _RptTasks;
                    Repeater rptInformation = _RptInformation;
                    Repeater rptPrint = _RptPrint;

                    string moduleName = Helper.GetModuleName();
                    string ModuleID = Helper.GetModuleID(moduleName);

                    Healthcare oHealthcare = BusinessLayer.GetHealthcare(AppSession.UserLogin.HealthcareID);
                    XDocument xdoc = Helper.LoadXMLFile(this, string.Format("right_panel/{0}.xml", ModuleID));
                    if (xdoc != null)
                    {
                        var lstQuickMenu = (from pg in xdoc.Descendants("page").Where(p => p.Attribute("menucode").Value == menuCode)
                                            select new
                                            {
                                                Tasks = (from qm in pg.Descendants("task")
                                                         select new
                                                         {
                                                             ID = qm.Attribute("id") == null ? "" : qm.Attribute("id").Value,
                                                             Code = qm.Attribute("code").Value,
                                                             Title = qm.Attribute("title").Value,
                                                             Description = qm.Attribute("description").Value,
                                                             Url = qm.Attribute("url").Value,
                                                             Visible = qm.Attribute("visible") == null ? "1" : qm.Attribute("visible").Value,
                                                             Width = qm.Attribute("width") == null ? "950" : qm.Attribute("width").Value,
                                                             Height = qm.Attribute("height") == null ? "600" : qm.Attribute("height").Value,
                                                             HealthcareAccess = qm.Attribute("isByHealthcare") == null ? oHealthcare.Initial : qm.Attribute("isByHealthcare").Value == "1" ? qm.Attribute("healthcareName").Value : oHealthcare.Initial
                                                             //Url = Page.ResolveUrl(qm.Attribute("url").Value)
                                                         }),
                                                Information = (from qm in pg.Descendants("information")
                                                               select new
                                                               {
                                                                   ID = qm.Attribute("id") == null ? "" : qm.Attribute("id").Value,
                                                                   Code = qm.Attribute("code").Value,
                                                                   Title = qm.Attribute("title").Value,
                                                                   Description = qm.Attribute("description").Value,
                                                                   Url = qm.Attribute("url").Value,
                                                                   Visible = qm.Attribute("visible") == null ? "1" : qm.Attribute("visible").Value,
                                                                   Width = qm.Attribute("width") == null ? "950" : qm.Attribute("width").Value,
                                                                   Height = qm.Attribute("height") == null ? "600" : qm.Attribute("height").Value,
                                                                   HealthcareAccess = qm.Attribute("isByHealthcare") == null ? oHealthcare.Initial : qm.Attribute("isByHealthcare").Value == "1" ? qm.Attribute("healthcareName").Value : oHealthcare.Initial
                                                                   //Url = Page.ResolveUrl(qm.Attribute("url").Value)
                                                               }),
                                                Print = (from qm in pg.Descendants("print")
                                                         select new
                                                         {
                                                             Title = qm.Attribute("title").Value,
                                                             Visible = qm.Attribute("visible") == null ? "1" : qm.Attribute("visible").Value,
                                                             IsDisplayPrintCount = qm.Attribute("isDisplayPrintCount") == null ? "0" : qm.Attribute("isDisplayPrintCount").Value,
                                                             ReportCode = qm.Attribute("reportcode").Value,
                                                             HealthcareAccess = qm.Attribute("isByHealthcare") == null ? oHealthcare.Initial : qm.Attribute("isByHealthcare").Value == "1" ? qm.Attribute("healthcareName").Value : oHealthcare.Initial
                                                         }),
                                                Utilities = (from qm in pg.Descendants("utility")
                                                             select new
                                                             {
                                                                 ID = qm.Attribute("id") == null ? "" : qm.Attribute("id").Value,
                                                                 Code = qm.Attribute("code").Value,
                                                                 Title = qm.Attribute("title").Value,
                                                                 Description = qm.Attribute("description").Value,
                                                                 Url = qm.Attribute("url").Value,
                                                                 Visible = qm.Attribute("visible") == null ? "1" : qm.Attribute("visible").Value,
                                                                 Width = qm.Attribute("width") == null ? "950" : qm.Attribute("width").Value,
                                                                 Height = qm.Attribute("height") == null ? "600" : qm.Attribute("height").Value,
                                                                 HealthcareAccess = qm.Attribute("isByHealthcare") == null ? oHealthcare.Initial : qm.Attribute("isByHealthcare").Value == "1" ? qm.Attribute("healthcareName").Value : oHealthcare.Initial
                                                             })
                                            }).FirstOrDefault();
                        if (lstQuickMenu != null)
                        {
                            var lstTasks = lstQuickMenu.Tasks.Where(p => p.Visible == "1" && p.HealthcareAccess == oHealthcare.Initial);
                            if (lstTasks.Count() > 0)
                            {
                                rptTasks.DataSource = lstTasks;
                                rptTasks.DataBind();
                            }
                            var lstInformation = lstQuickMenu.Information.Where(p => p.Visible == "1" && p.HealthcareAccess == oHealthcare.Initial);
                            if (lstInformation.Count() > 0)
                            {
                                rptInformation.DataSource = lstInformation;
                                rptInformation.DataBind();
                            }
                            var lstPrint = lstQuickMenu.Print.Where(p => p.Visible == "1" && p.HealthcareAccess == oHealthcare.Initial);
                            if (lstPrint.Count() > 0)
                            {
                                rptPrint.DataSource = lstPrint;
                                rptPrint.DataBind();
                            }

                            LoadDirectPrintConfiguration(ModuleID, menuCode);
                        }
                    }
                }
            }
        }
        public abstract string OnGetMenuCode();
        public virtual bool IsShowRightPanel()
        {
            return true;
        }
        public virtual bool IsEntryUsePopup()
        {
            return true;
        }
        public virtual void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = true;
        }

        private void LoadDirectPrintConfiguration(string moduleID, string menuCode)
        {
            if ((AppSession.UserLogin != null))
            {
                TextBox txtJmlLabel = _txtJmlLabel;
                ASPxComboBox cboPrinterUrl = _cboPrinterUrl;
                HtmlTableCell tdPrinterLocation = _tdPrinterLocation;
                HtmlInputHidden hdnIsMultiLocation = _hdnIsMultiLocation;
                HtmlInputHidden hdnMaxLabelNo = _hdnMaxLabelNo;

                ArrayList menuList = new ArrayList {
                    Constant.MenuCode.EmergencyCare.REGISTRATION,
                    Constant.MenuCode.Inpatient.REGISTRATION,
                    Constant.MenuCode.Outpatient.REGISTRATION,
                    Constant.MenuCode.Laboratory.REGISTRATION,
                    Constant.MenuCode.Imaging.REGISTRATION,
                    Constant.MenuCode.MedicalDiagnostic.REGISTRATION,
                    Constant.MenuCode.MedicalCheckup.REGISTRATION,
                    Constant.MenuCode.MedicalRecord.PATIENT_DATA,
                    Constant.MenuCode.MedicalCheckup.CONTROL_ORDER,
                    Constant.MenuCode.Outpatient.BILL_SUMMARY_LABORATORY,
                    Constant.MenuCode.Outpatient.BILL_SUMMARY_IMAGING,
                    Constant.MenuCode.Outpatient.BILL_SUMMARY_CHARGES,
                    Constant.MenuCode.Imaging.BILL_SUMMARY_LABORATORY,
                    Constant.MenuCode.Imaging.WORK_LIST,
                    Constant.MenuCode.Imaging.BILL_SUMMARY_CHARGES,
                    Constant.MenuCode.Laboratory.WORK_LIST,
                    Constant.MenuCode.Laboratory.BILL_SUMMARY_IMAGING,
                    Constant.MenuCode.Laboratory.BILL_SUMMARY_CHARGES,
                    Constant.MenuCode.MedicalCheckup.BILL_SUMMARY_LABORATORY,
                    Constant.MenuCode.MedicalCheckup.BILL_SUMMARY_IMAGING,
                    Constant.MenuCode.MedicalCheckup.BILL_SUMMARY_CHARGES,
                    Constant.MenuCode.MedicalCheckup.BILL_SUMMARY_CHARGES_OUTPATIENT,
                    Constant.MenuCode.Pharmacy.PRESCRIPTION_ENTRY
                };

                if (menuList.Contains(menuCode))
                {
                    string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}','{7}')",
    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_FORMAT_CETAKAN_LABEL, Constant.SettingParameter.RM_JUMLAH_CETAKAN_LABEL_RD, Constant.SettingParameter.RM_JUMLAH_CETAKAN_LABEL_RJ,
    Constant.SettingParameter.RM_JUMLAH_CETAKAN_LABEL_RI, Constant.SettingParameter.RM_JUMLAH_CETAKAN_LABEL_MD, Constant.SettingParameter.RM_MULTI_LOKASI_PENDAFTARAN, Constant.SettingParameter.RM_MAX_JUMLAH_LABEL);

                    List<vSettingParameterDt> oParamList = BusinessLayer.GetvSettingParameterDtList(filterExp);
                    if (oParamList.Count > 0)
                    {
                        vSettingParameterDt oParam;

                        switch (moduleID)
                        {
                            case Constant.Module.INPATIENT:
                                oParam = oParamList.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_JUMLAH_CETAKAN_LABEL_RI)).FirstOrDefault();
                                break;
                            case Constant.Module.EMERGENCY:
                                oParam = oParamList.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_JUMLAH_CETAKAN_LABEL_RD)).FirstOrDefault();
                                break;
                            case Constant.Module.OUTPATIENT:
                                oParam = oParamList.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_JUMLAH_CETAKAN_LABEL_RJ)).FirstOrDefault();
                                break;
                            default:
                                oParam = oParamList.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_JUMLAH_CETAKAN_LABEL_MD)).FirstOrDefault();
                                break;
                        }
                        txtJmlLabel.Text = oParam != null ? oParam.ParameterValue : "1";

                        bool isMultiLocation = oParamList.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_MULTI_LOKASI_PENDAFTARAN)).FirstOrDefault().ParameterValue == "1" ? true : false;
                        hdnMaxLabelNo.Value = oParamList.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_MAX_JUMLAH_LABEL)).FirstOrDefault().ParameterValue;

                        tdPrinterLocation.Visible = isMultiLocation;
                        hdnIsMultiLocation.Value = isMultiLocation ? "1" : "0";
                        if (isMultiLocation)
                        {
                            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.LOKASI_PENDAFTARAN));
                            Methods.SetComboBoxField(cboPrinterUrl, lstStandardCode, "StandardCodeName", "StandardCodeID", DevExpress.Web.ASPxEditors.DropDownStyle.DropDownList);
                            if (cboPrinterUrl.Items.Count > 0)
                            {
                                ////Set Default Value based on IP Address
                                //string ipAddress = HttpContext.Current.Request.UserHostAddress;
                                //filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType = '{1}' AND IsDeleted=0",
                                //    ipAddress, Constant.DirectPrintType.BUKTI_PEMBAYARAN);

                                ////Get Printer Address
                                //PrinterLocation oPrinter = BusinessLayer.GetPrinterLocationList(filterExp).FirstOrDefault();

                                cboPrinterUrl.SelectedIndex = 0;
                            }
                        }
                    }
                }
            }

        }
    }
}
