using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QISEncryption;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.ReportDesktop;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class GeneralCtl : System.Web.UI.UserControl
    {
        protected string TodayDate;
        protected string TodayDateNow;
        protected string defaultDatePickerNow;
        protected string HealthcareID;
        protected int UserID;
        protected string UserFullName;
        protected string UserName;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            LoadRightPanelContent();
        }

        protected string GetLabel(string code)
        {
            BasePage page = (BasePage)this.Page;
            return page.GetLabel(code);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (AppSession.UserLogin != null)
            {
                HealthcareID = AppSession.UserLogin.HealthcareID;
                UserID = AppSession.UserLogin.UserID;
                UserName = AppSession.UserLogin.UserName;
                UserFullName = AppSession.UserLogin.UserFullName;

                //notif
                if (AppSession.SA0200 != null)
                {
                    hdnIsSocketNotification.Value = AppSession.SA0200.ToString();
                }
                if (AppSession.SA0201 != null)
                {
                    hdnURLSocketNoification.Value = AppSession.SA0201.ToString();
                }

            }
            else
            {
                HealthcareID = "";
                UserID = 0;
                UserName = "";
                UserFullName = "";

                hdnIsSocketNotification.Value = "0";
                hdnURLSocketNoification.Value = "";
            }
            defaultDatePickerNow = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            TodayDate = DateTime.Now.ToString("yyyyMMdd");
            TodayDateNow = DateTime.Now.ToString();
            GetListReport();
        }

        private void GetListReport()
        {
            ReportMasterDownloadExcel rpt = BusinessLayer.GetReportMasterDownloadExcelList(string.Format("")).FirstOrDefault();
            if (rpt != null)
            {
                hdnLstReportDownloadExcel.Value = rpt.ReportDownloadExcel;
                hdnLstReportDownloadRawExcel.Value = rpt.ReportDownloadRawExcel;
                hdnLstReportDownloadImageToExcel.Value = rpt.ReportDownloadImageToExcel;
            }
        }
        private void BindGridView(string param, string searchDialogType, string baseFilterExpression, ref string intellisenseHints)
        {
            try
            {
                #region Load XML
                if (param == "open")
                {
                    XDocument xdoc = Helper.LoadXMLFile(this, "search_dialog.xml");
                    var tempSearchDialog = (from sd in xdoc.Descendants("searchdialog").Where(p => p.Attribute("type").Value == searchDialogType)
                                            select new
                                            {
                                                SearchDialogBase = sd.Attribute("searchdialogbase") != null ? sd.Attribute("searchdialogbase").Value : "",
                                                FilterExpression = sd.Attribute("filterexpression") != null ? sd.Attribute("filterexpression").Value : ""
                                            }).FirstOrDefault();
                    if (tempSearchDialog == null)
                        throw new Exception(string.Format("Search Dialog with type {0} is not defined", searchDialogType));
                    if (tempSearchDialog.SearchDialogBase == "")
                    {
                        SearchDialogState = (from sd in xdoc.Descendants("searchdialog").Where(p => p.Attribute("type").Value == searchDialogType)
                                             select new CSearchDialogState
                                             {
                                                 MethodName = sd.Attribute("methodname").Value,
                                                 KeyFieldName = sd.Attribute("keyfieldname").Value,
                                                 FilterExpression = "",
                                                 BaseFilterExpression = sd.Attribute("filterexpression") != null ? sd.Attribute("filterexpression").Value : "",
                                                 IsTreeView = sd.Attribute("istreeview") != null ? (sd.Attribute("istreeview").Value == "1") : false,
                                                 OrderByExpression = sd.Attribute("orderbyexpression") != null ? sd.Attribute("orderbyexpression").Value : "",
                                                 //OrderByColumnIndex = sd.Attribute("orderbycolumnindex") != null ? Convert.ToInt32(sd.Attribute("orderbycolumnindex").Value) : 0,
                                                 //OrderByType = sd.Attribute("orderbytype") != null ? sd.Attribute("orderbytype").Value : "ASC",
                                                 GridColumns = (from grd in sd.Descendants("gridcolumn")
                                                                select new GridColumn
                                                                {
                                                                    DataField = grd.Attribute("datafield").Value,
                                                                    HeaderText = grd.Attribute("headertext").Value,
                                                                    Width = grd.Attribute("width").Value,
                                                                    DisplayCustomField = grd.Attribute("displaycustomfield") != null ? grd.Attribute("displaycustomfield").Value : null,
                                                                    HorizontalAlign = grd.Attribute("horizontalalign") != null ? grd.Attribute("horizontalalign").Value : "left"
                                                                }).ToList<GridColumn>(),
                                                 IntellisenseTexts = (from itx in sd.Descendants("intellisensetext")
                                                                      select new QuickSearchIntellisense
                                                                      {
                                                                          DataField = itx.Attribute("datafield").Value,
                                                                          HeaderText = itx.Attribute("headertext").Value,
                                                                          Description = itx.Attribute("description").Value != null ? itx.Attribute("description").Value : ""
                                                                      }).ToList<QuickSearchIntellisense>()

                                             }).FirstOrDefault();
                    }
                    else
                    {
                        SearchDialogState = (from sd in xdoc.Descendants("searchdialog").Where(p => p.Attribute("type").Value == tempSearchDialog.SearchDialogBase)
                                             select new CSearchDialogState
                                             {
                                                 MethodName = sd.Attribute("methodname").Value,
                                                 KeyFieldName = sd.Attribute("keyfieldname").Value,
                                                 FilterExpression = "",
                                                 BaseFilterExpression = tempSearchDialog.FilterExpression,
                                                 IsTreeView = sd.Attribute("istreeview") != null ? (sd.Attribute("istreeview").Value == "1") : false,
                                                 OrderByExpression = sd.Attribute("orderbyexpression") != null ? sd.Attribute("orderbyexpression").Value : "",
                                                 //OrderByColumnIndex = sd.Attribute("orderbycolumnindex") != null ? Convert.ToInt32(sd.Attribute("orderbycolumnindex").Value) : 0,
                                                 //OrderByType = sd.Attribute("orderbytype") != null ? sd.Attribute("orderbytype").Value : "ASC",
                                                 GridColumns = (from grd in sd.Descendants("gridcolumn")
                                                                select new GridColumn
                                                                {
                                                                    DataField = grd.Attribute("datafield").Value,
                                                                    HeaderText = grd.Attribute("headertext").Value,
                                                                    Width = grd.Attribute("width").Value,
                                                                    DisplayCustomField = grd.Attribute("displaycustomfield") != null ? grd.Attribute("displaycustomfield").Value : null,
                                                                    HorizontalAlign = grd.Attribute("horizontalalign") != null ? grd.Attribute("horizontalalign").Value : "left"
                                                                }).ToList<GridColumn>(),
                                                 IntellisenseTexts = (from itx in sd.Descendants("intellisensetext")
                                                                      select new QuickSearchIntellisense
                                                                      {
                                                                          DataField = itx.Attribute("datafield").Value,
                                                                          HeaderText = itx.Attribute("headertext").Value,
                                                                          Description = itx.Attribute("description").Value != null ? itx.Attribute("description").Value : ""
                                                                      }).ToList<QuickSearchIntellisense>()

                                             }).FirstOrDefault();
                    }

                    SearchDialogState.BaseFilterExpression = SearchDialogState.BaseFilterExpression.Replace("@HealthcareID", AppSession.UserLogin.HealthcareID);
                    if (baseFilterExpression != "")
                    {
                        if (SearchDialogState.BaseFilterExpression == "")
                            SearchDialogState.BaseFilterExpression = baseFilterExpression;
                        else
                            SearchDialogState.BaseFilterExpression += string.Format(" AND {0}", baseFilterExpression);
                    }
                }
                #endregion

                #region Bind Grid View
                string filterExpression = SearchDialogState.BaseFilterExpression;
                if (SearchDialogState.FilterExpression != "")
                {
                    if (filterExpression != "" && filterExpression.Substring(filterExpression.Length - 1) != ";")
                        filterExpression += " AND ";
                    filterExpression += SearchDialogState.FilterExpression;
                }
                //string orderByExpression = string.Format("{0} {1}", SearchDialogState.GridColumns[SearchDialogState.OrderByColumnIndex].DataField, SearchDialogState.OrderByType);
                MethodInfo method = typeof(BusinessLayer).GetMethod(SearchDialogState.MethodName, new[] { typeof(string), typeof(int), typeof(int), typeof(string) });
                IList list = null;
                if (method != null)
                {
                    object obj = method.Invoke(null, new object[] { filterExpression, 50, 1, SearchDialogState.OrderByExpression });
                    list = (IList)obj;
                }
                else
                {
                    method = typeof(BusinessLayer).GetMethod(SearchDialogState.MethodName, new[] { typeof(string) });
                    if (method == null)
                        throw new Exception(string.Format("Method {0} is not found", SearchDialogState.MethodName));
                    if (SearchDialogState.OrderByExpression != "")
                    {
                        if (filterExpression == "")
                            filterExpression = "1 = 1";
                        filterExpression += " ORDER BY " + SearchDialogState.OrderByExpression;
                    }
                    object obj = method.Invoke(null, new string[] { filterExpression });
                    list = (IList)obj;
                }

                List<Words> words = Helper.LoadWords(this);

                BoundField keyField = new BoundField();
                keyField.DataField = SearchDialogState.KeyFieldName;
                keyField.ItemStyle.CssClass = "keyField";
                keyField.HeaderStyle.CssClass = "keyField";
                grdSearch.Columns.Add(keyField);

                int ctr = 0;
                foreach (GridColumn col in SearchDialogState.GridColumns)
                {
                    if (ctr < 1 && SearchDialogState.IsTreeView)
                    {
                        TemplateField codeField = new TemplateField();
                        codeField.ItemTemplate = new ColumnCodeTemplateField(col.DataField);
                        codeField.HeaderStyle.Width = new Unit(col.Width);
                        codeField.HeaderText = Helper.GetWordsLabel(words, col.HeaderText);
                        grdSearch.Columns.Add(codeField);
                    }
                    else
                    {
                        BoundField field = new BoundField();
                        if (col.DisplayCustomField != null)
                            field.DataField = col.DisplayCustomField;
                        else
                            field.DataField = col.DataField;
                        field.HeaderText = Helper.GetWordsLabel(words, col.HeaderText);
                        field.HeaderStyle.Width = new Unit(col.Width);
                        switch (col.HorizontalAlign)
                        {
                            case "center": field.ItemStyle.HorizontalAlign = HorizontalAlign.Center; break;
                            case "right": field.ItemStyle.HorizontalAlign = HorizontalAlign.Right; break;
                            default: field.ItemStyle.HorizontalAlign = HorizontalAlign.Left; break;
                        }
                        grdSearch.Columns.Add(field);
                    }
                    ctr++;
                }

                grdSearch.DataSource = list;
                grdSearch.DataBind();
                #endregion

                #region Quick Search
                foreach (QuickSearchIntellisense col in SearchDialogState.IntellisenseTexts)
                {
                    if (intellisenseHints != "")
                        intellisenseHints += ",";
                    intellisenseHints += string.Format("{{ \"text\":\"{0}\",\"fieldName\":\"{1}\",\"description\":\"{2}\" }}", Helper.GetWordsLabel(words, col.HeaderText), col.DataField, col.Description);
                }

                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public class ColumnCodeTemplateField : ITemplate
        {
            private string codeField = "";
            public ColumnCodeTemplateField(string codeField)
            {
                this.codeField = codeField;
            }

            public void InstantiateIn(Control container)
            {
                HtmlGenericControl div = new HtmlGenericControl("DIV");
                div.DataBinding += new EventHandler(div_DataBinding);
                container.Controls.Add(div);
            }

            void div_DataBinding(object sender, EventArgs e)
            {
                HtmlGenericControl div = (HtmlGenericControl)sender;
                object dataItem = DataBinder.GetDataItem(div.NamingContainer);
                div.Style.Add("margin-left", DataBinder.Eval(dataItem, "Level").ToString() + "0px");
                div.InnerHtml = DataBinder.Eval(dataItem, this.codeField).ToString();
            }
        }

        protected void cbpSearchDialog_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string[] param = e.Parameter.Split('|');
            string searchDialogType = "";
            string baseFilterExpression = "";
            if (param[0] == "open")
            {
                searchDialogType = param[1];
                baseFilterExpression = HttpUtility.HtmlDecode(param[2]);
            }
            else if (param[0] == "refresh")
                SearchDialogState.FilterExpression = param[1];


            string intellisenseHints = "";
            BindGridView(param[0], searchDialogType, baseFilterExpression, ref intellisenseHints);

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpIntellisenseHints"] = intellisenseHints;
        }

        protected void cbpDirectPrintProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string[] param = e.Parameter.Split('|');
            int paramLength = param.Length;
            int id = 0;
            string date = "";
            string mealPlan = "";
            string serviceUnit = "";
            string menuCode = "";

            string reportCode = hdnReportCode.Value;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            try
            {
                #region Form Permintaan Makan - GIZI
                if (paramLength == 3)
                {
                    date = Helper.GetDatePickerValue(param[0]).ToString(Constant.FormatString.DATE_PICKER_FORMAT2);
                    mealPlan = param[1];
                    serviceUnit = param[2];
                }
                #endregion

                else if (paramLength == 2)
                {
                    #region Master Fixed Asset
                    if (param[1] == Constant.MenuCode.Accounting.FA_ITEM)
                    {
                        id = Convert.ToInt32(param[0]);
                        menuCode = param[1];
                    }
                    #endregion
                    #region Berita Acara Asset Dan Inventaris
                    else if (param[1] == Constant.MenuCode.Accounting.FA_ACCEPTANCE)
                    {
                        id = Convert.ToInt32(param[0]);
                        menuCode = param[1];
                    }
                    #endregion
                    #region EXCLUSION
                    else if (reportCode == Constant.PrintCode.LABEL_SAMPLE_LABORATORIUM_DT)
                    {
                        //DIABAIKAN
                    }
                    #endregion
                    #region Bon Pemesanan Makan - GIZI
                    else
                    {
                        date = Helper.GetDatePickerValue(param[0]).ToString(Constant.FormatString.DATE_PICKER_FORMAT2);
                        mealPlan = param[1];
                    }
                    #endregion
                }
                else
                {
                    id = Convert.ToInt32(param[0]);
                }

                string result = string.Empty;
                switch (reportCode)
                {
                    case Constant.PrintCode.TRACER_REKAM_MEDIS:
                        result = PrintMROutguides(id);
                        break;
                    case Constant.PrintCode.TRACER_PERJANJIAN_REKAM_MEDIS:
                        result = PrintAppointmentMROutguides(id);
                        break;
                    case Constant.PrintCode.LABEL_REKAM_MEDIS:
                        result = PrintMRLabel(id, reportCode);
                        break;
                    case Constant.PrintCode.LABEL_REKAM_MEDIS_RSDOSOBA:
                        result = PrintMRLabelRSDOSOBA(id, reportCode);
                        break;
                    case Constant.PrintCode.LABEL_REKAM_MEDIS_2:
                        result = PrintMRLabel(id, reportCode);
                        break;
                    case Constant.PrintCode.LABEL_REKAM_MEDIS_RSRTH:
                        result = PrintMRLabelRSRTH(id);
                        break;
                    case Constant.PrintCode.COVER_REKAM_MEDIS: // MR Cover Label
                        result = PrintMRCoverLabel(id);
                        break;
                    case Constant.PrintCode.BUKTI_PENDAFTARAN:
                        result = PrintBuktiPendaftaran(id);
                        break;
                    case Constant.PrintCode.BUKTI_PENDAFTARAN_BROS:
                        result = PrintBuktiPendaftaranBros(id);
                        break;
                    case Constant.PrintCode.BUKTI_PENDAFTARAN_RSPBT:
                        result = PrintBuktiPendaftaranRspbt(id);
                        break;
                    case Constant.PrintCode.BUKTI_PENDAFTARAN_RSDOSOBA:
                        result = PrintBuktiPendaftaranRsdosoba(id);
                        break;
                    case Constant.PrintCode.BUKTI_PENDAFTARAN_RSCK:
                        result = PrintBuktiPendaftaranRSCK(id);
                        break;
                    case Constant.PrintCode.BUKTI_PENDAFTARAN_PERJANJIAN:
                        result = PrintBuktiPendaftaranPerjanjian(id);
                        break;
                    case Constant.PrintCode.KARTU_PASIEN:
                        result = PrintPatientCard(id);
                        break;
                    case Constant.PrintCode.GELANG_PASIEN:
                        result = PrintPatientWristband(id);
                        break;
                    case Constant.PrintCode.GELANG_PASIEN_ANAK:
                        result = PrintPatientChildWristband(id);
                        break;
                    case Constant.PrintCode.RINGKASAN_TRANSAKSI_RAWAT_JALAN:
                        result = PrintRingkasanTransaksi(id);
                        break;
                    case Constant.PrintCode.RINGKASAN_TRANSAKSI_RAWAT_JALAN_2:
                        result = PrintRingkasanTransaksi2(id);
                        break;
                    case Constant.PrintCode.LABEL_SAMPLE_LABORATORIUM:
                        result = PrintLaboratorySampleLabel(id);
                        break;
                    case Constant.PrintCode.LABEL_SAMPLE_LABORATORIUM_DT:
                        if (param.Length == 2)
                        {
                            result = PrintLaboratorySampleLabelDt(Convert.ToInt32(!string.IsNullOrEmpty(param[0]) ? param[0] : "0"), Convert.ToInt32(!string.IsNullOrEmpty(param[1]) ? param[1] : "0"));
                        }
                        else
                        {
                            result = PrintLaboratorySampleLabelDt(0, id);
                        }
                        break;
                    case Constant.PrintCode.LABEL_BARANG_PRODUKSI:
                        result = PrintProductionLabel(id);
                        break;
                    case Constant.PrintCode.GELANG_PASIEN_RAWAT_JALAN:
                        result = PrintOutPatientWristband(id);
                        break;
                    case Constant.PrintCode.BUKTI_PEMERIKSAAN_DOT_MATRIX_RSMD:
                        result = PrintBuktiPemeriksaan(id);
                        break;
                    case Constant.PrintCode.LABEL_MR_RSMD:
                        result = PrintLabelMR(id);
                        break;
                    case Constant.PrintCode.STICKER_GELANG_RSMD:
                        result = PrintStickerGelang(id);
                        break;
                    case Constant.PrintCode.STICKER_RAWAT_INAP_RSMD:
                        result = PrintStickerRawatInap(id);
                        break;
                    case Constant.PrintCode.STICKER_RAWAT_JALAN_RSMD:
                        result = PrintStickerRawatJalan(id);
                        break;
                    case Constant.PrintCode.LABEL_COVER_HASIL_RADIOLOGI:
                        result = PrintLabelCoverHasilRadiologi(id);
                        break;
                    case Constant.PrintCode.SLIP_GIZI:
                        result = PrintSlipGizi(id);
                        break;
                    case Constant.PrintCode.TRACER_REKAM_MEDIS_PERJANJIAN:
                        result = PrintMROutguidesAppointment(id);
                        break;
                    case Constant.PrintCode.TRACER_REKAM_MEDIS_PERJANJIAN_PER_HARI:
                        result = PrintMROutguidesAppointmentPerDay(param[1]);
                        break;
                    case Constant.PrintCode.LABEL_PASIEN_REGISTRASI_RADIOLOGI:
                        result = PrintLabelPasienRegistrasiRadiologi(id);
                        break;
                    case Constant.PrintCode.LABEL_PASIEN:
                        result = PrintLabelPasien(id);
                        break;
                    case Constant.PrintCode.COVER_LABEL:
                        result = PrintCoverLabel(id);
                        break;
                    case Constant.PrintCode.LABEL_PASIEN_REGISTRASI_LABORATORIUM:
                        result = PrintLabelPasienRegistrasiLaboratorium(id);
                        break;
                    case Constant.PrintCode.GELANG_PASIEN_2:
                        result = PrintGelangPasien2(id);
                        break;
                    case Constant.PrintCode.LABEL_RADIOLOGI:
                        result = PrintLabelRadiologi(id);
                        break;
                    case Constant.PrintCode.NUTRITION_WORKSLIST:
                        result = PrintNutritionWorklist(date, mealPlan, serviceUnit);
                        break;
                    case Constant.PrintCode.LABEL_PASIEN_REGISTRASI_CATH_LAB:
                        result = PrintLabelPasienRegistrasiCathLab(id);
                        break;
                    case Constant.PrintCode.JOB_ORDER_LABORATORY:
                        result = PrintJobOrderLaboratory(id);
                        break;
                    case Constant.PrintCode.NUTRITION_PATIENT_ORDER:
                        result = PrintNutritionPatientOrder(date, mealPlan);
                        break;
                    case Constant.PrintCode.LABEL_REKAM_MEDIS_RSSMC:
                        result = PrintLabelRekamMedisRSSMC(id);
                        break;
                    case Constant.PrintCode.LABEL_PENDAFTARAN_RSDO:
                        result = PrintLabelPendaftaranRSDO(id);
                        break;
                    case Constant.PrintCode.LABEL_RADIOLOGI_RSDO:
                        result = PrintLabelRadiologiRSDO(id);
                        break;
                    case Constant.PrintCode.GELANG_PASIEN_DEWASA_RSDO:
                        //result = PrintGelangPasienDewasaRSDO(id);
                        result = PrintPatientWristband(id);
                        break;
                    case Constant.PrintCode.GELANG_PASIEN_ANAK_RSDO:
                        //result = PrintGelangPasienAnakRSDO(id);
                        result = PrintPatientChildWristband(id);
                        break;
                    case Constant.PrintCode.LABEL_OBAT:
                        result = PrintLabelObat(id);
                        break;
                    case Constant.PrintCode.ORDER_FARMASI:
                        result = PrintOrderFarmasi(id);
                        break;
                    case Constant.PrintCode.LABEL_REKAM_MEDIS_3:
                        result = printLabelPasienRekamMedis2(id);
                        break;
                    case Constant.PrintCode.BUKTI_PERJANJIAN_RSDOSOBA:
                        result = PrintBuktiPendaftaranPerjanjian2(id);
                        break;
                    case Constant.PrintCode.LABEL_MASTER_ITEM:
                        result = PrintLabelMasterItem(id);
                        break;
                    case Constant.PrintCode.LABEL_RADIOLOGI_2:
                        result = PrintLabelRadiologiRSDOSOBA(id);
                        break;
                    case Constant.PrintCode.LABEL_RADIOLOGI_3:
                        result = PrintLabelRadiologiRSUKRIDA(id);
                        break;
                    case Constant.PrintCode.LABEL_RADIOLOGI_RSRA:
                        result = PrintLabelRadiologiRSRA(id);
                        break;
                    case Constant.PrintCode.LABEL_ASSET:
                        result = PrintLabelAsset(id, menuCode);
                        break;
                    case Constant.PrintCode.LABEL_ASSET_PER_BA:
                        result = PrintLabelAssetPerBA(id, menuCode);
                        break;
                    case Constant.PrintCode.BUKTI_TRANSAKSI_FARMASI:
                        result = PrintBuktiTransaksiFarmasi(id);
                        break;
                    case Constant.PrintCode.BUKTI_ORDER_RSSES:
                        result = PrintBuktiOrderRSSES(id);
                        break;
                    case Constant.PrintCode.BUKTI_ORDER_LAB_RSSES:
                        result = PrintBuktiOrderLabRSSES(id);
                        break;
                    case Constant.PrintCode.LABEL_OBAT_RSSES:
                        result = PrintLabelObatRSSES(id);
                        break;
                    case Constant.PrintCode.BUKTI_TRANSAKSI:
                        result = PrintBuktiTransaksi(id);
                        break;
                    case Constant.PrintCode.NUTRITION_PATIENT_REGISTRATION:
                        result = PrintNutritionPatientRegistration(id);
                        break;
                    case Constant.PrintCode.COVER_REKAM_MEDIS_RSBL:
                        result = PrintMRCoverLabelRSBL(id);
                        break;
                    case Constant.PrintCode.SLIP_ANTRIAN_RAWAT_JALAN:
                        result = PrintSlipAntrianRawatJalan(id);
                        break;
                    case Constant.PrintCode.STICKER_RAWAT_JALAN_RSMD_ZEBRA:
                        result = PrintStickerRawatJalanZebra(id);
                        break;
                    case Constant.PrintCode.LABEL_PENUNJANG_MEDIS:
                        result = PrintLabelPenunjangMedis(id);
                        break;
                    case Constant.PrintCode.LABEL_RADIOLOGI_IND:
                        result = PrintLabelRadiologiKKDI(id);
                        break;
                    case Constant.PrintCode.LABEL_RADIOLOGI_ENG:
                        result = PrintLabelRadiologiKKDI(id, "en");
                        break;
                    case Constant.PrintCode.LABEL_AMPLOP_HASIL_LABORATORIUM:
                        result = PrintLabelAmplopHasilLaboratorium(id);
                        break;
                    default:
                        result = "";
                        break;
                }
                panel.JSProperties["cpZebraPrinting"] = result;
            }
            catch (Exception ex)
            {
                Helper.InsertErrorLog(ex);
                panel.JSProperties["cpZebraPrinting"] = "An error occured while sending command to printer";
            }
        }

        private string PrintBuktiTransaksi(int id)
        {
            string result = string.Empty;
            try
            {
                bool isBasedOnIPAddress = AppSession.IsPrinterLocationBasedOnIP;

                if (isBasedOnIPAddress)
                {
                    //Get Printer Address
                    string ipAddress = HttpContext.Current.Request.UserHostAddress;
                    string filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType = '{1}' AND IsDeleted=0",
                        ipAddress, Constant.DirectPrintType.BUKTI_TRANSAKSI);

                    PrinterLocation oPrinter = BusinessLayer.GetPrinterLocationList(filterExp).FirstOrDefault();
                    if (oPrinter != null)
                    {
                        string printerUrl = oPrinter.PrinterName;
                        ZebraPrinting.PrintBuktiTransaksi(id, printerUrl);
                    }
                    else
                    {
                        result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                    }
                }
                else
                {
                    result = "This feature is not available yet";
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }
        private string PrintBuktiOrderRSSES(int id)
        {
            string result = string.Empty;
            try
            {
                bool isBasedOnIPAddress = AppSession.IsPrinterLocationBasedOnIP;

                if (isBasedOnIPAddress)
                {
                    //Get Printer Address
                    string ipAddress = HttpContext.Current.Request.UserHostAddress;
                    string filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType = '{1}' AND IsDeleted=0",
                        ipAddress, Constant.DirectPrintType.BUKTI_ORDER_PENUNJANG);

                    PrinterLocation oPrinter = BusinessLayer.GetPrinterLocationList(filterExp).FirstOrDefault();
                    if (oPrinter != null)
                    {
                        string printerUrl = oPrinter.PrinterName;
                        ZebraPrinting.PrintBuktiOrderRSSES(id, printerUrl);
                    }
                    else
                    {
                        result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                    }
                }
                else
                {
                    result = "This feature is not available yet";
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        private string PrintBuktiOrderLabRSSES(int id)
        {
            string result = string.Empty;
            try
            {
                bool isBasedOnIPAddress = AppSession.IsPrinterLocationBasedOnIP;

                if (isBasedOnIPAddress)
                {
                    //Get Printer Address
                    string ipAddress = HttpContext.Current.Request.UserHostAddress;
                    string filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType = '{1}' AND IsDeleted=0",
                        ipAddress, Constant.DirectPrintType.BUKTI_ORDER_PENUNJANG);

                    PrinterLocation oPrinter = BusinessLayer.GetPrinterLocationList(filterExp).FirstOrDefault();
                    if (oPrinter != null)
                    {
                        string printerUrl = oPrinter.PrinterName;
                        ZebraPrinting.PrintBuktiOrderLabRSSES(id, printerUrl);
                    }
                    else
                    {
                        result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                    }
                }
                else
                {
                    result = "This feature is not available yet";
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        #region cbpDirectPrintProcessDirect_Callback
        protected void cbpDirectPrintProcessDirect_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string[] param = e.Parameter.Split('|');
            string menuCode = "";
            string reportCode = hdnReportCode.Value;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            try
            {
                int id = Convert.ToInt32(param[0]);
                string result = string.Empty;
                switch (reportCode)
                {
                    case Constant.PrintCode.TRACER_REKAM_MEDIS:
                        result = PrintMROutguides(id);
                        break;
                    case Constant.PrintCode.LABEL_REKAM_MEDIS:
                        result = PrintMRLabel(id, reportCode);
                        break;
                    case Constant.PrintCode.LABEL_REKAM_MEDIS_2:
                        result = PrintMRLabel(id, reportCode);
                        break;
                    case Constant.PrintCode.COVER_REKAM_MEDIS: // MR Cover Label
                        result = PrintMRCoverLabel(id);
                        break;
                    case Constant.PrintCode.BUKTI_PENDAFTARAN:
                        result = PrintBuktiPendaftaran(id);
                        break;
                    case Constant.PrintCode.KARTU_PASIEN:
                        result = PrintPatientCard(id);
                        break;
                    case Constant.PrintCode.GELANG_PASIEN:
                        result = PrintPatientWristband(id);
                        break;
                    case Constant.PrintCode.GELANG_PASIEN_ANAK:
                        result = PrintPatientChildWristband(id);
                        break;
                    case Constant.PrintCode.RINGKASAN_TRANSAKSI_RAWAT_JALAN:
                        result = PrintRingkasanTransaksi(id);
                        break;
                    case Constant.PrintCode.RINGKASAN_TRANSAKSI_RAWAT_JALAN_2:
                        result = PrintRingkasanTransaksi2(id);
                        break;
                    case Constant.PrintCode.LABEL_SAMPLE_LABORATORIUM:
                        result = PrintLaboratorySampleLabel(id);
                        break;
                    case Constant.PrintCode.LABEL_BARANG_PRODUKSI:
                        result = PrintProductionLabel(id);
                        break;
                    case Constant.PrintCode.GELANG_PASIEN_RAWAT_JALAN:
                        result = PrintOutPatientWristband(id);
                        break;
                    case Constant.PrintCode.BUKTI_PEMERIKSAAN_DOT_MATRIX_RSMD:
                        result = PrintBuktiPemeriksaan(id);
                        break;
                    case Constant.PrintCode.LABEL_MR_RSMD:
                        result = PrintLabelMR(id);
                        break;
                    case Constant.PrintCode.STICKER_GELANG_RSMD:
                        result = PrintStickerGelang(id);
                        break;
                    case Constant.PrintCode.STICKER_RAWAT_INAP_RSMD:
                        result = PrintStickerRawatInap(id);
                        break;
                    case Constant.PrintCode.STICKER_RAWAT_JALAN_RSMD:
                        result = PrintStickerRawatJalan(id);
                        break;
                    case Constant.PrintCode.BUKTI_PEMERIKSAAN_LAB_RSMD:
                        result = PrintBuktiPemeriksaanLab(id);
                        break;
                    case Constant.PrintCode.LABEL_COVER_HASIL_RADIOLOGI:
                        result = PrintLabelCoverHasilRadiologiUsingClientService(id);
                        break;
                    case Constant.PrintCode.DAFTAR_PASIEN_DIRAWAT_RSMD:
                        result = PrintDaftarPasienDirawat();
                        break;
                    case Constant.PrintCode.BUKTI_PENDAFTARAN_THERMAL:
                        result = PrintBuktiPendaftaranThermal(id);
                        break;
                    case Constant.PrintCode.LABEL_ASSET:
                        result = PrintLabelAsset(id, menuCode);
                        break;
                    case Constant.PrintCode.COVER_REKAM_MEDIS_RSBL: // MR Cover Label
                        result = PrintMRCoverLabelRSBL(id);
                        break;
                    case Constant.PrintCode.STICKER_RAWAT_JALAN_RSMD_ZEBRA:
                        result = PrintStickerRawatJalanZebra(id);
                        break;
                    default:
                        result = "";
                        break;
                }
                panel.JSProperties["cpZebraPrinting"] = result;
            }
            catch (Exception ex)
            {
                Helper.InsertErrorLog(ex);
                panel.JSProperties["cpZebraPrinting"] = "An error occured while sending command to printer";
            }
        }
        #endregion

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string oReportParameter = "";

            string result = "";
            string reportCode = string.Format("ReportCode = '{0}'", hdnReportCode.Value);
            ReportMaster rm = BusinessLayer.GetReportMasterList(reportCode).FirstOrDefault();
            string fileName = string.Format(@"{0}_{1}_{2}_{3}", rm.ClassName, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112), DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString());
            try
            {
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".csv");
                Response.Charset = "";
                Response.ContentType = "application/text";

                StringBuilder sbResult = new StringBuilder();

                if (rm.GCDataSourceType == Constant.DataSourceType.STORED_PROCEDURE)
                {
                    List<dynamic> lstDynamic = null;
                    List<Variable> lstVariable = new List<Variable>();
                    List<vReportParameter> listReportParameter = BusinessLayer.GetvReportParameterList(string.Format("ReportID = {0} ORDER BY DisplayOrder", rm.ReportID));
                    string[] value = hdnReportParam.Value.Split('|');
                    for (int i = 0; i < listReportParameter.Count; ++i)
                    {
                        vReportParameter reportParameter = listReportParameter[i];
                        lstVariable.Add(new Variable { Code = reportParameter.FieldName, Value = GetFilterExpression(value[i]) });

                        oReportParameter += string.Format("{0} = {1}|", reportParameter.FieldName, GetFilterExpression(value[i]));
                    }

                    lstDynamic = BusinessLayer.GetDataReport(rm.ObjectTypeName, lstVariable);

                    dynamic fields = lstDynamic[0];

                    foreach (var prop in fields.GetType().GetProperties())
                    {
                        sbResult.Append(prop.Name);
                        sbResult.Append(",");
                    }

                    sbResult.Append("\r\n");

                    for (int i = 0; i < lstDynamic.Count; ++i)
                    {
                        dynamic entity = lstDynamic[i];

                        foreach (var prop in entity.GetType().GetProperties())
                        {
                            //sbResult.Append(prop.GetValue(entity, null));
                            sbResult.Append(prop.GetValue(entity, null).ToString().Replace(',', '_'));
                            sbResult.Append(",");
                        }

                        sbResult.Append("\r\n");
                    }
                }
                else
                {
                    List<vReportParameter> listReportParameter = BusinessLayer.GetvReportParameterList(string.Format("ReportID = {0} ORDER BY DisplayOrder", rm.ReportID));
                    string filterExpression = String.Empty;
                    string[] value = hdnReportParam.Value.Split('|');
                    for (int i = 0; i < listReportParameter.Count; ++i)
                    {
                        string filterParameter = String.Empty;
                        vReportParameter reportParameter = listReportParameter[i];
                        if (reportParameter.GCFilterParameterType == Constant.FilterParameterType.FREE_TEXT)
                        {
                            if (i > 0 && filterExpression != "")
                                filterExpression += " AND ";
                            filterParameter += value[i];
                            filterExpression += filterParameter;
                        }
                        else
                        {
                            if (reportParameter.GCFilterParameterType == Constant.FilterParameterType.TIME_RANGE)
                            {
                                if (i > 0 && filterExpression != "")
                                    filterExpression += " AND ";
                                string[] value2 = value[i].Split(';');
                                string valueFrom = value2[0];
                                string valueTo = value2[1];
                                filterParameter = string.Format("{0} BETWEEN '{1}' AND '{2}'", reportParameter.FieldName, valueFrom, valueTo);
                                filterExpression += filterParameter;
                            }
                            else if (reportParameter.GCFilterParameterType == Constant.FilterParameterType.DATE ||
                                reportParameter.GCFilterParameterType == Constant.FilterParameterType.PAST_PERIOD ||
                                reportParameter.GCFilterParameterType == Constant.FilterParameterType.UPCOMING_PERIOD)
                            {
                                if (i > 0 && filterExpression != "")
                                    filterExpression += " AND ";
                                string[] date = value[i].Split(';');
                                string startDate = date[0];
                                string endDate = date[1];
                                filterParameter = string.Format("{0} BETWEEN '{1}' AND '{2}'", reportParameter.FieldName, startDate, endDate);
                                filterExpression += filterParameter;
                            }
                            else if (reportParameter.GCFilterParameterType == Constant.FilterParameterType.SINGLE_DATE)
                            {
                                string[] paramSplit = value[i].Split(';');
                                string value2 = paramSplit[0];
                                if (i > 0 && filterExpression != "")
                                    filterExpression += " AND ";
                                filterParameter = string.Format("{0} = '{1}'", reportParameter.FieldName, value2);
                                filterExpression += filterParameter;
                            }
                            else if (reportParameter.GCFilterParameterType == Constant.FilterParameterType.COMBO_BOX || reportParameter.GCFilterParameterType == Constant.FilterParameterType.YEAR_COMBO_BOX || reportParameter.GCFilterParameterType == Constant.FilterParameterType.CUSTOM_COMBO_BOX || reportParameter.GCFilterParameterType == Constant.FilterParameterType.SEARCH_DIALOG)
                            {
                                string[] paramSplit = value[i].Split(';');
                                string value2 = paramSplit[0];
                                if (i > 0 && filterExpression != "")
                                {
                                    if (!reportParameter.IsAllowSelectAll || value2 != "")
                                        filterExpression += " AND ";
                                }
                                if (!reportParameter.IsAllowSelectAll || value2 != "")
                                    filterParameter = string.Format("{0} = '{1}'", reportParameter.FieldName, value2);
                                filterExpression += filterParameter;
                            }
                            else
                            {
                                if (i > 0 && filterExpression != "")
                                    filterExpression += " AND ";
                                string[] paramSplit = value[i].Split(';');
                                StringBuilder sbFilterExpressionVal = new StringBuilder();
                                StringBuilder sbTemp = new StringBuilder();

                                for (int idxValue = 0; idxValue < paramSplit.Length; idxValue++)
                                {
                                    string value2 = paramSplit[idxValue];
                                    if (sbTemp.ToString() != "")
                                        sbTemp.Append(",");

                                    sbTemp.Append("'").Append(value2).Append("'");
                                }
                                sbFilterExpressionVal.Append(" IN (").Append(sbTemp.ToString()).Append(")");
                                filterParameter = string.Format("{0}{1}", reportParameter.FieldName, sbFilterExpressionVal.ToString());
                                filterExpression += filterParameter;
                            }
                        }

                    }

                    string additionalFilterExpression = GetFilterExpression(rm.AdditionalFilterExpression);
                    if (filterExpression != "" && additionalFilterExpression != "")
                        filterExpression += " AND ";
                    filterExpression += additionalFilterExpression;
                    if (filterExpression != "" && rm.IsReportBasedOnUserLogin)
                        //filterExpression += string.Format(" AND (CreatedBy = {0} OR LastUpdatedBy = {0})",AppSession.UserLogin.UserID.ToString());
                        filterExpression += string.Format(" AND (CreatedBy = '{0}')", AppSession.UserLogin.UserID.ToString());


                    oReportParameter = filterExpression;
                    MethodInfo method = typeof(BusinessLayer).GetMethod(rm.ObjectTypeName, new[] { typeof(string) });
                    Object obj = method.Invoke(null, new string[] { filterExpression });
                    IList collection = (IList)obj;
                    dynamic fields = collection[0];

                    foreach (var prop in fields.GetType().GetProperties())
                    {
                        sbResult.Append(prop.Name);
                        sbResult.Append(",");
                    }
                    sbResult.Append("\r\n");

                    foreach (object temp in collection)
                    {
                        foreach (var prop in temp.GetType().GetProperties())
                        {
                            #region old
                            //sbResult.Append(prop.GetValue(temp, null).ToString().Replace(',', ';'));
                            //sbResult.Append(",");
                            #endregion

                            var text = prop.GetValue(temp, null);
                            string textValid = "";

                            if (text != null)
                            {
                                textValid = text.ToString();
                            }

                            sbResult.Append(textValid.Replace(',', '_'));
                            sbResult.Append(",");
                        }

                        sbResult.Append("\r\n");
                    }

                }

                Response.Output.Write(sbResult.ToString());
                result = "success";
            }
            catch (Exception ex)
            {
                result = string.Format("fail|{0}", ex.Message);
            }
            finally
            {
                Response.Flush();
                Response.End();
            }
            InsertReportPrintLog(reportCode, oReportParameter);
        }

        protected void btnRAWExport_Click(object sender, EventArgs e)
        {
            string oReportParameter = "";

            string result = "";
            //string filePath = HttpContext.Current.Server.MapPath("~/Libs/App_Data/temp");
            string reportCode = string.Format("ReportCode = '{0}'", hdnReportCode.Value);
            ReportMaster rm = BusinessLayer.GetReportMasterList(reportCode).FirstOrDefault();
            string fileName = string.Format(@"{0}_{1}_{2}_{3}", rm.ClassName, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112), DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString());
            try
            {
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".csv");
                Response.Charset = "";
                Response.ContentType = "application/text";

                StringBuilder sbResult = new StringBuilder();
                SqlConnection sqlCon = new SqlConnection(getCNSetting());
                sqlCon.Open();

                if (rm.GCDataSourceType == Constant.DataSourceType.STORED_PROCEDURE)
                {
                    List<vReportParameter> listReportParameter = BusinessLayer.GetvReportParameterList(string.Format("ReportID = {0} ORDER BY DisplayOrder", rm.ReportID));
                    string[] value = hdnReportParam.Value.Split('|');

                    string query = string.Format("EXEC {0}", rm.ObjectTypeName);
                    for (int i = 0; i < listReportParameter.Count; ++i)
                    {
                        if (i == 0)
                        {
                            query += string.Format(" '{0}'", GetFilterExpression(value[i]));
                        }
                        else
                        {
                            query += string.Format(",'{0}'", GetFilterExpression(value[i]));
                        }
                    }

                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    SqlDataReader reader = sqlCmd.ExecuteReader();
                    object[] output = new object[reader.FieldCount];

                    for (int i = 0; i < reader.FieldCount; i++)
                        output[i] = reader.GetName(i);

                    sbResult.Append(string.Join(",", output));
                    sbResult.Append("\r\n");

                    while (reader.Read())
                    {
                        reader.GetValues(output);

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            output[i] = output[i].ToString().Replace(',', '_');
                        }

                        sbResult.Append(string.Join(",", output));
                        sbResult.Append("\r\n");
                    }

                    reader.Close();
                }
                else
                {
                    List<vReportParameter> listReportParameter = BusinessLayer.GetvReportParameterList(string.Format("ReportID = {0} ORDER BY DisplayOrder", rm.ReportID));
                    string filterExpression = String.Empty;
                    string[] value = hdnReportParam.Value.Split('|');
                    for (int i = 0; i < listReportParameter.Count; ++i)
                    {
                        string filterParameter = String.Empty;
                        vReportParameter reportParameter = listReportParameter[i];
                        if (reportParameter.GCFilterParameterType == Constant.FilterParameterType.FREE_TEXT)
                        {
                            if (i > 0 && filterExpression != "")
                                filterExpression += " AND ";
                            filterParameter += value[i];
                            filterExpression += filterParameter;
                        }
                        else
                        {
                            if (reportParameter.GCFilterParameterType == Constant.FilterParameterType.TIME_RANGE)
                            {
                                if (i > 0 && filterExpression != "")
                                    filterExpression += " AND ";
                                string[] value2 = value[i].Split(';');
                                string valueFrom = value2[0];
                                string valueTo = value2[1];
                                filterParameter = string.Format("{0} BETWEEN '{1}' AND '{2}'", reportParameter.FieldName, valueFrom, valueTo);
                                filterExpression += filterParameter;
                            }
                            else if (reportParameter.GCFilterParameterType == Constant.FilterParameterType.DATE ||
                                reportParameter.GCFilterParameterType == Constant.FilterParameterType.PAST_PERIOD ||
                                reportParameter.GCFilterParameterType == Constant.FilterParameterType.UPCOMING_PERIOD)
                            {
                                if (i > 0 && filterExpression != "")
                                    filterExpression += " AND ";
                                string[] date = value[i].Split(';');
                                string startDate = date[0];
                                string endDate = date[1];
                                filterParameter = string.Format("{0} BETWEEN '{1}' AND '{2}'", reportParameter.FieldName, startDate, endDate);
                                filterExpression += filterParameter;
                            }
                            else if (reportParameter.GCFilterParameterType == Constant.FilterParameterType.SINGLE_DATE)
                            {
                                string[] paramSplit = value[i].Split(';');
                                string value2 = paramSplit[0];
                                if (i > 0 && filterExpression != "")
                                    filterExpression += " AND ";
                                filterParameter = string.Format("{0} = '{1}'", reportParameter.FieldName, value2);
                                filterExpression += filterParameter;
                            }
                            else if (reportParameter.GCFilterParameterType == Constant.FilterParameterType.COMBO_BOX || reportParameter.GCFilterParameterType == Constant.FilterParameterType.YEAR_COMBO_BOX || reportParameter.GCFilterParameterType == Constant.FilterParameterType.CUSTOM_COMBO_BOX || reportParameter.GCFilterParameterType == Constant.FilterParameterType.SEARCH_DIALOG)
                            {
                                string[] paramSplit = value[i].Split(';');
                                string value2 = paramSplit[0];
                                if (i > 0 && filterExpression != "")
                                {
                                    if (!reportParameter.IsAllowSelectAll || value2 != "")
                                        filterExpression += " AND ";
                                }
                                if (!reportParameter.IsAllowSelectAll || value2 != "")
                                    filterParameter = string.Format("{0} = '{1}'", reportParameter.FieldName, value2);
                                filterExpression += filterParameter;
                            }
                            else
                            {
                                if (i > 0 && filterExpression != "")
                                    filterExpression += " AND ";
                                string[] paramSplit = value[i].Split(';');
                                StringBuilder sbFilterExpressionVal = new StringBuilder();
                                StringBuilder sbTemp = new StringBuilder();

                                for (int idxValue = 0; idxValue < paramSplit.Length; idxValue++)
                                {
                                    string value2 = paramSplit[idxValue];
                                    if (sbTemp.ToString() != "")
                                        sbTemp.Append(",");

                                    sbTemp.Append("'").Append(value2).Append("'");
                                }
                                sbFilterExpressionVal.Append(" IN (").Append(sbTemp.ToString()).Append(")");
                                filterParameter = string.Format("{0}{1}", reportParameter.FieldName, sbFilterExpressionVal.ToString());
                                filterExpression += filterParameter;
                            }
                        }

                    }

                    string additionalFilterExpression = GetFilterExpression(rm.AdditionalFilterExpression);
                    if (filterExpression != "" && additionalFilterExpression != "")
                        filterExpression += " AND ";
                    filterExpression += additionalFilterExpression;
                    if (filterExpression != "" && rm.IsReportBasedOnUserLogin)
                        //filterExpression += string.Format(" AND (CreatedBy = {0} OR LastUpdatedBy = {0})",AppSession.UserLogin.UserID.ToString());
                        filterExpression += string.Format(" AND (CreatedBy = '{0}')", AppSession.UserLogin.UserID.ToString());

                    oReportParameter = filterExpression;

                    MethodInfo method = typeof(BusinessLayer).GetMethod(rm.ObjectTypeName, new[] { typeof(string) });

                    string[] paramName = method.ReturnParameter.ToString().Replace("]", "").Split('.');
                    string query = string.Format("SELECT * FROM {0} WHERE {1}", paramName[7], filterExpression);
                    if (string.IsNullOrEmpty(filterExpression))
                    {
                        query = string.Format("SELECT * FROM {0}", paramName[7]);
                    }

                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    object[] output = new object[reader.FieldCount];

                    for (int i = 0; i < reader.FieldCount; i++)
                        output[i] = reader.GetName(i);

                    sbResult.Append(string.Join(",", output));
                    sbResult.Append("\r\n");

                    while (reader.Read())
                    {
                        reader.GetValues(output);

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            output[i] = output[i].ToString().Replace(',', '_');
                        }

                        sbResult.Append(string.Join(",", output));
                        sbResult.Append("\r\n");
                    }
                    reader.Close();
                }

                sqlCon.Close();
                Response.Output.Write(sbResult.ToString());
                result = "success";
            }
            catch (Exception ex)
            {
                result = string.Format("fail|{0}", ex.Message);
            }
            finally
            {
                Response.Flush();
                Response.End();
            }
            InsertReportPrintLog(reportCode, oReportParameter);
        }

        #region PrintCoverLabel
        private string PrintCoverLabel(int id)
        {
            string result = string.Empty;
            string ipAddress = HttpContext.Current.Request.UserHostAddress;
            string filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0", ipAddress, Constant.DirectPrintType.LABEL_RM);
            List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);
            string printerUrl1 = lstPrinter.FirstOrDefault().PrinterName;

            vPatient patient = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", id)).FirstOrDefault();
            ZebraPrinting.printCoverLabel(patient, printerUrl1);
            return result;
        }
        #endregion

        #region PrintLabelPasien
        private string PrintLabelPasien(int id)
        {
            string result = string.Empty;
            string ipAddress = HttpContext.Current.Request.UserHostAddress;
            string filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0", ipAddress, Constant.DirectPrintType.STICKER_GELANG_PASIEN);
            List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);
            string printerUrl1 = lstPrinter.FirstOrDefault().PrinterName;

            vRegistration registration = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", id)).FirstOrDefault();
            ZebraPrinting.printLabelPasien(registration, printerUrl1);
            return result;
        }
        #endregion

        #region PrintGelangPasien2
        private string PrintGelangPasien2(int id)
        {
            string result = string.Empty;
            string ipAddress = HttpContext.Current.Request.UserHostAddress;
            string filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0", ipAddress, Constant.DirectPrintType.STICKER_GELANG_PASIEN);
            List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);
            string printerUrl1 = lstPrinter.FirstOrDefault().PrinterName;

            vRegistration registration = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", id)).FirstOrDefault();
            ZebraPrinting.printGelangPasien(registration, printerUrl1);
            return result;
        }
        #endregion

        #region PrintGelangPasienDewasaRSDO
        private string PrintGelangPasienDewasaRSDO(int id)
        {
            string errorMsg = string.Empty;

            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_FORMAT_GELANG_PASIEN_DEWASA, Constant.SettingParameter.RM_FORMAT_GELANG_PASIEN_ANAK, Constant.SettingParameter.RM_FORMAT_GELANG_PASIEN_BAYI,
                    Constant.SettingParameter.RM_GELANG_PASIEN_ANAK_KHUSUS, Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_LAKI,
                    Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_PEREMPUAN, Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_ANAK,
                    Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_BAYI);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                if (lstParam != null)
                {
                    Registration oRegistration = BusinessLayer.GetRegistration(id);
                    if (oRegistration != null)
                    {
                        vConsultVisit oVisit = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", oRegistration.RegistrationID)).FirstOrDefault();
                        if (oVisit != null)
                        {

                            string printFormat = string.Empty;
                            string printerUrl = string.Empty;

                            if (hdnIsMultiLocation.Value == "0")
                            {
                                printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_GELANG_PASIEN_DEWASA)).FirstOrDefault().ParameterValue;
                                //Choose Printer Url based on Patient Gender
                                switch (oVisit.GCGender)
                                {
                                    case Constant.Gender.MALE:
                                        printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_LAKI)).FirstOrDefault().ParameterValue;
                                        break;
                                    case Constant.Gender.FEMALE:
                                        printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_PEREMPUAN)).FirstOrDefault().ParameterValue;
                                        break;
                                    default:
                                        printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_LAKI)).FirstOrDefault().ParameterValue;
                                        break;
                                }
                            }
                            else
                            {
                                //Get Printer Url from Location DropDown
                                StandardCode oStandardCode = BusinessLayer.GetStandardCode(cboPrinterUrl.Value.ToString());
                                if (oStandardCode != null)
                                {
                                    if (!String.IsNullOrEmpty(oStandardCode.TagProperty))
                                    {
                                        string[] tagField = oStandardCode.TagProperty.Split('|');
                                        //Printer_Label| Printer_Wristband_Male;Printer_Wristband_Female;Printer_Wristband_Children_Male;Printer_Wristband_Children_Female|Printer_Wristband_Infant|Printer_RegistrationSlip
                                        string[] wristbandUrl = tagField[1].Split(';');
                                        printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_GELANG_PASIEN_DEWASA)).FirstOrDefault().ParameterValue;
                                        //Choose Printer Url based on Patient Gender
                                        switch (oVisit.GCGender)
                                        {
                                            case Constant.Gender.MALE:
                                                printerUrl = wristbandUrl[0];
                                                break;
                                            case Constant.Gender.FEMALE:
                                                printerUrl = wristbandUrl[1];
                                                break;
                                            default:
                                                printerUrl = wristbandUrl[0];
                                                break;
                                        }
                                    }
                                }

                            }
                            if (!string.IsNullOrEmpty(printerUrl))
                            {
                                int labelCount = Convert.ToInt16(txtJmlLabel.Text);
                                ZebraPrinting.printGelangPasienDewasa(oVisit, printerUrl);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return errorMsg;
        }
        #endregion

        #region PrintGelangPasienAnakRSDO
        private string PrintGelangPasienAnakRSDO(int id)
        {
            string errorMsg = string.Empty;

            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_FORMAT_GELANG_PASIEN_DEWASA, Constant.SettingParameter.RM_FORMAT_GELANG_PASIEN_ANAK, Constant.SettingParameter.RM_FORMAT_GELANG_PASIEN_BAYI,
                    Constant.SettingParameter.RM_GELANG_PASIEN_ANAK_KHUSUS, Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_LAKI,
                    Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_PEREMPUAN, Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_ANAK,
                    Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_BAYI);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                if (lstParam != null)
                {
                    Registration oRegistration = BusinessLayer.GetRegistration(id);
                    if (oRegistration != null)
                    {
                        vConsultVisit oVisit = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", oRegistration.RegistrationID)).FirstOrDefault();
                        if (oVisit != null)
                        {

                            string printFormat = string.Empty;
                            string printerUrl = string.Empty;

                            if (hdnIsMultiLocation.Value == "0")
                            {
                                printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_GELANG_PASIEN_DEWASA)).FirstOrDefault().ParameterValue;
                                //Choose Printer Url based on Patient Gender
                                switch (oVisit.GCGender)
                                {
                                    case Constant.Gender.MALE:
                                        printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_LAKI)).FirstOrDefault().ParameterValue;
                                        break;
                                    case Constant.Gender.FEMALE:
                                        printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_PEREMPUAN)).FirstOrDefault().ParameterValue;
                                        break;
                                    default:
                                        printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_LAKI)).FirstOrDefault().ParameterValue;
                                        break;
                                }
                            }
                            else
                            {
                                //Get Printer Url from Location DropDown
                                StandardCode oStandardCode = BusinessLayer.GetStandardCode(cboPrinterUrl.Value.ToString());
                                if (oStandardCode != null)
                                {
                                    if (!String.IsNullOrEmpty(oStandardCode.TagProperty))
                                    {
                                        string[] tagField = oStandardCode.TagProperty.Split('|');
                                        //Printer_Label| Printer_Wristband_Male;Printer_Wristband_Female;Printer_Wristband_Children_Male;Printer_Wristband_Children_Female|Printer_Wristband_Infant|Printer_RegistrationSlip
                                        string[] wristbandUrl = tagField[1].Split(';');
                                        printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_GELANG_PASIEN_DEWASA)).FirstOrDefault().ParameterValue;
                                        //Choose Printer Url based on Patient Gender
                                        switch (oVisit.GCGender)
                                        {
                                            case Constant.Gender.MALE:
                                                printerUrl = wristbandUrl[2];
                                                break;
                                            case Constant.Gender.FEMALE:
                                                printerUrl = wristbandUrl[3];
                                                break;
                                            default:
                                                printerUrl = wristbandUrl[2];
                                                break;
                                        }
                                    }
                                }

                            }
                            if (!string.IsNullOrEmpty(printerUrl))
                            {
                                int labelCount = Convert.ToInt16(txtJmlLabel.Text);
                                ZebraPrinting.printGelangPasienAnak(oVisit, printerUrl);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return errorMsg;
        }
        #endregion

        #region PrintLabelPenunjangMedis
        private string PrintLabelPenunjangMedis(int id)
        {
            string result = string.Empty;
            try
            {
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')",
                AppSession.UserLogin.HealthcareID, Constant.SettingParameter.MD_FORMAT_CETAKAN_PENUNJANG_MEDIS, Constant.SettingParameter.MD_JENIS_PRINTER_PENUNJANG_MEDIS);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.MD_FORMAT_CETAKAN_PENUNJANG_MEDIS)).FirstOrDefault().ParameterValue;
                string printerType = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.MD_JENIS_PRINTER_PENUNJANG_MEDIS)).FirstOrDefault().ParameterValue;
                bool isBasedOnIPAddress = AppSession.IsPrinterLocationBasedOnIP;

                if (isBasedOnIPAddress)
                {
                    //Check Printer Type
                    switch (printerType)
                    {
                        case Constant.PrinterType.ZEBRA_PRINTER:
                            //Get Printer Address
                            string ipAddress = HttpContext.Current.Request.UserHostAddress;

                            filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                ipAddress, Constant.DirectPrintType.LABEL_PENUNJANG_MEDIS);

                            List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);

                            if (lstPrinter.Count > 0)
                            {
                                string printerUrl1 = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.LABEL_PENUNJANG_MEDIS).FirstOrDefault().PrinterName;
                                ZebraPrinting.PrintLabelPenunjangMedis(printFormat, id, printerUrl1);
                            }
                            else
                            {
                                result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                            }
                            break;
                    }
                }
                else
                {
                    result = string.Format("Printer Configuration is not available");
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }
        #endregion

        #region PrintLabelRadiologi
        private string PrintLabelRadiologi(int id)
        {
            string result = string.Empty;
            string ipAddress = HttpContext.Current.Request.UserHostAddress;
            string filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0", ipAddress, Constant.DirectPrintType.LABEL_RADIOLOGI);
            List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);
            string printerUrl1 = lstPrinter.FirstOrDefault().PrinterName;

            Healthcare oHealthcare = BusinessLayer.GetHealthcare(AppSession.UserLogin.HealthcareID);
            vPatientChargesHd1 charges = BusinessLayer.GetvPatientChargesHd1List(string.Format("TransactionID = {0}", id)).FirstOrDefault();

            if (oHealthcare.Initial == "RSSES")
            {
                vPatientChargesHd9 charges1 = BusinessLayer.GetvPatientChargesHd9List(string.Format("TransactionID = {0}", id)).FirstOrDefault();
                ZebraPrinting.printLabelRadiologiRSSES(charges1, printerUrl1);
            }
            else
            {

                ZebraPrinting.printLabelRadiologi(charges, printerUrl1);
            }


            return result;
        }
        #endregion

        #region PrintLabelCoverHasilRadiologiUsingClientService
        private string PrintLabelCoverHasilRadiologiUsingClientService(int id)
        {
            string result = string.Empty;
            try
            {
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IS_FORMAT_CETAKAN_LABEL_COVER);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                if (lstParam != null)
                {
                    string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.IS_FORMAT_CETAKAN_LABEL_COVER)).FirstOrDefault().ParameterValue;
                    vPatientChargesHd5 oHeader = BusinessLayer.GetvPatientChargesHd5List(string.Format("TransactionID = {0}", id)).FirstOrDefault();
                    result = ZebraPrinting.PrintCoverHasilRadiologiUsingClientService(oHeader, printFormat);
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }
        #endregion

        #region PrintBuktiTransaksiFarmasi
        private string PrintBuktiTransaksiFarmasi(int id)
        {
            string result = string.Empty;
            try
            {
                bool isBasedOnIPAddress = AppSession.IsPrinterLocationBasedOnIP;

                if (isBasedOnIPAddress)
                {
                    //Get Printer Address
                    string ipAddress = HttpContext.Current.Request.UserHostAddress;
                    string filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType = '{1}' AND IsDeleted=0",
                        ipAddress, Constant.DirectPrintType.BUKTI_TRANSAKSI_FARMASI);

                    PrinterLocation oPrinter = BusinessLayer.GetPrinterLocationList(filterExp).FirstOrDefault();
                    if (oPrinter != null)
                    {
                        string printerUrl = oPrinter.PrinterName;
                        ZebraPrinting.PrintBuktiTransaksiFarmasi(id, printerUrl);
                    }
                    else
                    {
                        result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                    }
                }
                else
                {
                    result = "This feature is not available yet";
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }
        #endregion

        #region PrintRingkasanTransaksi
        private string PrintRingkasanTransaksi(int id)
        {
            string result = string.Empty;
            try
            {
                bool isBasedOnIPAddress = AppSession.IsPrinterLocationBasedOnIP;

                if (isBasedOnIPAddress)
                {
                    //Get Printer Address
                    string ipAddress = HttpContext.Current.Request.UserHostAddress;
                    string filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType = '{1}' AND IsDeleted=0",
                        ipAddress, Constant.DirectPrintType.BUKTI_PEMBAYARAN);

                    PrinterLocation oPrinter = BusinessLayer.GetPrinterLocationList(filterExp).FirstOrDefault();
                    if (oPrinter != null)
                    {
                        string printerUrl = oPrinter.PrinterName;
                        ZebraPrinting.PrintBuktiPembayaran1(id, printerUrl);
                    }
                    else
                    {
                        result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                    }
                }
                else
                {
                    result = "This feature is not available yet";
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }
        #endregion

        #region PrintRingkasanTransaksi2
        private string PrintRingkasanTransaksi2(int id)
        {
            string result = string.Empty;
            try
            {
                bool isBasedOnIPAddress = AppSession.IsPrinterLocationBasedOnIP;

                if (isBasedOnIPAddress)
                {
                    //Get Printer Address
                    string ipAddress = HttpContext.Current.Request.UserHostAddress;
                    string filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType = '{1}' AND IsDeleted=0",
                        ipAddress, Constant.DirectPrintType.BUKTI_PEMBAYARAN);

                    PrinterLocation oPrinter = BusinessLayer.GetPrinterLocationList(filterExp).FirstOrDefault();
                    if (oPrinter != null)
                    {
                        string printerUrl = oPrinter.PrinterName;
                        ZebraPrinting.PrintBuktiPembayaran2(id, printerUrl);
                    }
                    else
                    {
                        result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                    }
                }
                else
                {
                    result = "This feature is not available yet";
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }
        #endregion

        #region PrintPatientCard
        private string PrintPatientCard(int id)
        {
            string result = string.Empty;
            try
            {
                SettingParameterDt oParameter = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_ALAMAT_PRINTER_DATACARD);
                if (oParameter != null)
                {
                    if (!String.IsNullOrEmpty(oParameter.ParameterValue))
                    {
                        try
                        {
                            Patient oPatient = BusinessLayer.GetPatient(id);
                            if (oPatient != null)
                            {
                                string msgPart1 = oPatient.Name;
                                string msgPart2 = oPatient.MedicalNo;
                                string msgPart3 = oPatient.RegisteredDate.ToString(Constant.FormatString.DATE_FORMAT);
                                string msgPart4 = oParameter.ParameterValue;
                                string msgData = String.Format("{0}|{1}|{2}|{3}", msgPart1, msgPart2, msgPart3, msgPart4);

                                TcpClient client = new TcpClient();
                                client.Connect(IPAddress.Parse(oParameter.ParameterValue), 6000);
                                NetworkStream stream = client.GetStream();
                                using (BinaryWriter w = new BinaryWriter(stream))
                                {
                                    using (BinaryReader r = new BinaryReader(stream))
                                    {
                                        w.Write(string.Format(@"{0}", msgData.ToString()).ToCharArray());
                                    }
                                }
                            }
                            else
                            {
                                result = String.Format("Cannot find registration information.");
                            }
                        }
                        catch (Exception ex)
                        {
                            result = String.Format("Cannot send information to printer service \n {0}", ex.Message);
                        }
                    }
                    else
                    {
                        result = String.Format("Printer Location is not been configured yet.");
                    }
                }
                else
                {
                    result = String.Format("Cannot find parameter information to printer service");
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }
        #endregion

        #region PrintLabelPasienRegistrasiRadiologi
        private string PrintLabelPasienRegistrasiRadiologi(int id)
        {
            string result = string.Empty;
            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IS_FORMAT_CETAKAN_LABEL_COVER, Constant.SettingParameter.IS_JENIS_PRINTER_CETAKAN_RADIOLOGI);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                if (lstParam != null)
                {
                    string printerType = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.IS_JENIS_PRINTER_CETAKAN_RADIOLOGI)).FirstOrDefault().ParameterValue;
                    string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.IS_FORMAT_CETAKAN_LABEL_COVER)).FirstOrDefault().ParameterValue;
                    //string printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;

                    string ipAddress = HttpContext.Current.Request.UserHostAddress;

                    filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}', '{2}') AND IsDeleted=0",
                                    ipAddress, Constant.DirectPrintType.LABEL_REGISTRASI_RADIOLOGI, Constant.DirectPrintType.LABEL_RADIOLOGI);

                    List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);

                    Healthcare entityHSU = BusinessLayer.GetHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID)).FirstOrDefault();

                    vLabelPatientRegistrationImaging oLabel = BusinessLayer.GetvLabelPatientRegistrationImagingList(string.Format("TransactionID = {0}", id)).FirstOrDefault();
                    if (entityHSU.Initial == "DEMO")
                    {
                        if (oLabel != null)
                        {
                            switch (printerType)
                            {
                                case Constant.PrinterType.ZEBRA_PRINTER:
                                    //Get Printer Address
                                    if (lstPrinter.Count > 0)
                                    {
                                        string printerUrl1 = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.LABEL_RADIOLOGI).FirstOrDefault().PrinterName;
                                        ZebraPrinting.PrintLabelPasienRadiologiRSCK_ZPL(printFormat, id, printerUrl1);
                                    }
                                    else
                                    {
                                        result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                    }
                                    break;
                                default:
                                    if (lstPrinter.Count > 0)
                                        ZebraPrinting.PrintLabelPasienRadiologiRSCK(oLabel, lstPrinter[0].PrinterName);
                                    else
                                        result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                    break;
                            }
                        }
                    }
                    else
                    {
                        if (oLabel != null)
                        {
                            switch (printerType)
                            {
                                case Constant.PrinterType.EPSON_DOT_MATRIX:
                                    //Get Printer Address

                                    if (lstPrinter.Count > 0)
                                        ZebraPrinting.PrintLabelPasienRadiologi(oLabel, lstPrinter[0].PrinterName);
                                    else
                                        result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                    break;
                                default:
                                    ZebraPrinting.PrintLabelPasienRadiologi(oLabel, lstPrinter[0].PrinterName);
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }
        #endregion

        #region PrintLabelPasienRegistrasiLaboratorium
        private string PrintLabelPasienRegistrasiLaboratorium(int id)
        {
            string result = string.Empty;
            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_FORMAT_BUKTI_PENDAFTARAN, Constant.SettingParameter.RM_ALAMAT_PRINTER_BUKTI_PENDAFTARAN, Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                if (lstParam != null)
                {
                    string printerType = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                    string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                    string printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;

                    string ipAddress = HttpContext.Current.Request.UserHostAddress;

                    filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                    ipAddress, Constant.DirectPrintType.LABEL_REGISTRASI_LABORATORIUM);

                    List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);

                    vLabelPatientRegistration oLabel = BusinessLayer.GetvLabelPatientRegistrationList(string.Format("RegistrationID = {0}", id)).FirstOrDefault();
                    if (oLabel != null)
                    {
                        switch (printerType)
                        {
                            case Constant.PrinterType.EPSON_DOT_MATRIX:
                                //Get Printer Address

                                if (lstPrinter.Count > 0)
                                    ZebraPrinting.PrintLabelPasienLaboratorium(oLabel, lstPrinter[0].PrinterName);
                                else
                                    result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                break;
                            default:
                                ZebraPrinting.PrintLabelPasienLaboratorium(oLabel, lstPrinter[0].PrinterName);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }
        #endregion

        #region PrintLabelPasienRegistrasiCathLab
        private string PrintLabelPasienRegistrasiCathLab(int id)
        {
            string result = string.Empty;
            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_FORMAT_BUKTI_PENDAFTARAN, Constant.SettingParameter.RM_ALAMAT_PRINTER_BUKTI_PENDAFTARAN, Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                if (lstParam != null)
                {
                    string printerType = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                    string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                    string printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;

                    string ipAddress = HttpContext.Current.Request.UserHostAddress;

                    filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                    ipAddress, Constant.DirectPrintType.LABEL_REGISTRASI_RADIOLOGI);

                    List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);

                    vLabelPatientRegistrationCathLab oLabel = BusinessLayer.GetvLabelPatientRegistrationCathLabList(string.Format("TransactionID = {0}", id)).FirstOrDefault();
                    if (oLabel != null)
                    {
                        switch (printerType)
                        {
                            case Constant.PrinterType.EPSON_DOT_MATRIX:
                                //Get Printer Address

                                if (lstPrinter.Count > 0)
                                    ZebraPrinting.PrintLabelPasienCathLab(oLabel, lstPrinter[0].PrinterName);
                                else
                                    result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                break;
                            default:
                                ZebraPrinting.PrintLabelPasienCathLab(oLabel, lstPrinter[0].PrinterName);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }
        #endregion

        #region PrintBuktiPendaftaran
        private string PrintBuktiPendaftaran(int id)
        {
            string result = string.Empty;
            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_FORMAT_BUKTI_PENDAFTARAN, Constant.SettingParameter.RM_ALAMAT_PRINTER_BUKTI_PENDAFTARAN, Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                if (lstParam != null)
                {
                    string printerType = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                    string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                    string printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                    if (hdnIsMultiLocation.Value == "1")
                    {
                        //Get Printer Url from Location DropDown
                        StandardCode oStandardCode = BusinessLayer.GetStandardCode(cboPrinterUrl.Value.ToString());
                        if (oStandardCode != null)
                        {
                            if (!String.IsNullOrEmpty(oStandardCode.TagProperty))
                            {
                                string[] tagField = oStandardCode.TagProperty.Split('|');
                                //Printer_Label| Printer_Wristband_Male;Printer_Wristband_Female;Printer_Wristband_Children_Male;Printer_Wristband_Children_Female|Printer_Wristband_Infant|Printer_RegistrationSlip
                                printerUrl = tagField[3];
                            }
                        }
                    }

                    Registration oRegistration = BusinessLayer.GetRegistration(id);
                    if (oRegistration != null)
                    {
                        vConsultVisit oVisit = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", oRegistration.RegistrationID)).FirstOrDefault();
                        if (oVisit != null)
                        {
                            string ipAddress = HttpContext.Current.Request.UserHostAddress;

                            switch (printerType)
                            {
                                case Constant.PrinterType.ZEBRA_PRINTER:
                                    ZebraPrinting.PrintBuktiPendaftaran(oVisit, printerUrl, printFormat);
                                    break;
                                case Constant.PrinterType.EPSON_DOT_MATRIX:
                                    ZebraPrinting.PrintBuktiPendaftaran3(oVisit, printerUrl, printFormat);
                                    break;
                                case Constant.PrinterType.DOT_MATRIX_FORMAT_1:
                                    result = ZebraPrinting.PrintBuktiPendaftaran4(oVisit, printerUrl, printFormat);
                                    break;
                                case Constant.PrinterType.THERMAL_RECEIPT_PRINTER:
                                    //Get Printer Address

                                    filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                        ipAddress, Constant.DirectPrintType.BUKTI_PENDAFTARAN);

                                    List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);
                                    if (lstPrinter.Count > 0)
                                    {
                                        if (printFormat == Constant.PrintFormat.BUKTI_PENDAFTARAN_RSSY)
                                        {
                                            ZebraPrinting.PrintBuktiPendaftaranRSSY(oVisit, lstPrinter[0].PrinterName);
                                        }
                                        else if (printFormat == Constant.PrintFormat.BIXOLON_BUKTI_PENDAFTARAN_RSPKSB)
                                        {
                                            bool isUsePrintingTools = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.BUKTI_PENDAFTARAN).FirstOrDefault().IsUsingPrintingTools;
                                            ZebraPrinting.PrintBuktiPendaftaranBixolon(oVisit, lstPrinter[0].PrinterName, isUsePrintingTools);
                                        }
                                        else
                                        {
                                            ZebraPrinting.PrintBuktiPendaftaran5(oVisit, lstPrinter[0].PrinterName);
                                        }
                                    }
                                    else
                                    {
                                        result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                    }
                                    break;
                                case Constant.PrinterType.THERMAL_FORMAT_RSSES:
                                    //Get Printer Address
                                    filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                        ipAddress, Constant.DirectPrintType.BUKTI_PENDAFTARAN);

                                    List<PrinterLocation> lstPrinter2 = BusinessLayer.GetPrinterLocationList(filterExp);

                                    if (lstPrinter2.Count > 0)
                                        ZebraPrinting.PrintBuktiPendaftaranRSSES(oVisit, lstPrinter2[0].PrinterName);
                                    else
                                        result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                    break;
                                case Constant.PrinterType.THERMAL_FORMAT_RSRT:
                                    //Get Printer Address
                                    if (string.IsNullOrEmpty(printerUrl))
                                    {
                                        filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                           ipAddress, Constant.DirectPrintType.BUKTI_PENDAFTARAN);

                                        List<PrinterLocation> lstPrinterRsrt = BusinessLayer.GetPrinterLocationList(filterExp);

                                        if (lstPrinterRsrt.Count > 0)
                                        {
                                            printerUrl = lstPrinterRsrt[0].PrinterName;
                                        }

                                    }
                                    if (string.IsNullOrEmpty(printerUrl))
                                    {
                                        result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                        break;
                                    }
                                    else
                                    {
                                        ZebraPrinting.PrintBuktiPendaftaranRSRT(oVisit, printerUrl);
                                    }

                                    break;
                                case Constant.PrinterType.THERMAL_FORMAT_RSSM:
                                    //Get Printer Address

                                    filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                        ipAddress, Constant.DirectPrintType.BUKTI_PENDAFTARAN);

                                    List<PrinterLocation> lstPrinter3 = BusinessLayer.GetPrinterLocationList(filterExp);
                                    if (lstPrinter3.Count > 0)
                                        ZebraPrinting.PrintBuktiPendaftaranRSSM(oVisit, lstPrinter3[0].PrinterName);
                                    else
                                        result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                    break;
                                default:
                                    ZebraPrinting.PrintBuktiPendaftaran(oVisit, printerUrl, printFormat);
                                    break;

                                case Constant.PrinterType.THERMAL_FORMAT_RSUKRIDA:
                                    //Get Printer Address

                                    filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                        ipAddress, Constant.DirectPrintType.BUKTI_PENDAFTARAN);

                                    List<PrinterLocation> lstPrinter4 = BusinessLayer.GetPrinterLocationList(filterExp);
                                    if (lstPrinter4.Count > 0)
                                        ZebraPrinting.PrintBuktiPendaftaranRSUKRIDA(oVisit, lstPrinter4[0].PrinterName);
                                    else
                                        result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                    break;
                            }
                        }
                        //vConsultVisit9 oVisit9 = BusinessLayer.GetvConsultVisit9List(string.Format("RegistrationID = {0} AND IsMainVisit = 1", oRegistration.RegistrationID)).FirstOrDefault();
                        //if (oVisit9 != null)
                        //{
                        //    switch (printerType)
                        //    {
                        //    }
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }
        #endregion

        #region PrintBuktiPendaftaranBros
        private string PrintBuktiPendaftaranBros(int id)
        {
            string result = string.Empty;
            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_FORMAT_BUKTI_PENDAFTARAN, Constant.SettingParameter.RM_ALAMAT_PRINTER_BUKTI_PENDAFTARAN, Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                if (lstParam != null)
                {
                    string printerType = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                    string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                    string printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                    if (hdnIsMultiLocation.Value == "1")
                    {
                        //Get Printer Url from Location DropDown
                        StandardCode oStandardCode = BusinessLayer.GetStandardCode(cboPrinterUrl.Value.ToString());
                        if (oStandardCode != null)
                        {
                            if (!String.IsNullOrEmpty(oStandardCode.TagProperty))
                            {
                                string[] tagField = oStandardCode.TagProperty.Split('|');
                                //Printer_Label| Printer_Wristband_Male;Printer_Wristband_Female;Printer_Wristband_Children_Male;Printer_Wristband_Children_Female|Printer_Wristband_Infant|Printer_RegistrationSlip
                                printerUrl = tagField[3];
                            }
                        }
                    }

                    Registration oRegistration = BusinessLayer.GetRegistration(id);
                    if (oRegistration != null)
                    {
                        vConsultVisit oVisit = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", oRegistration.RegistrationID)).FirstOrDefault();
                        if (oVisit != null)
                        {
                            switch (printerType)
                            {
                                case Constant.PrinterType.ZEBRA_PRINTER:
                                    ZebraPrinting.PrintBuktiPendaftaran(oVisit, printerUrl, printFormat);
                                    break;
                                case Constant.PrinterType.EPSON_DOT_MATRIX:
                                    ZebraPrinting.PrintBuktiPendaftaran3(oVisit, printerUrl, printFormat);
                                    break;
                                case Constant.PrinterType.DOT_MATRIX_FORMAT_1:
                                    result = ZebraPrinting.PrintBuktiPendaftaran4(oVisit, printerUrl, printFormat);
                                    break;
                            }
                        }
                        vConsultVisit9 oVisit9 = BusinessLayer.GetvConsultVisit9List(string.Format("RegistrationID = {0} AND IsMainVisit = 1", oRegistration.RegistrationID)).FirstOrDefault();
                        if (oVisit9 != null)
                        {
                            switch (printerType)
                            {
                                case Constant.PrinterType.THERMAL_RECEIPT_PRINTER:

                                    int labelCount = Convert.ToInt16(txtJmlLabel.Text);
                                    int maxLabelNo = Convert.ToInt16(hdnMaxLabelNo.Value);

                                    if (!string.IsNullOrEmpty(printerUrl))
                                    {
                                        ZebraPrinting.PrintBuktiPendaftaranBros(oVisit9, printerUrl, labelCount > maxLabelNo ? maxLabelNo : labelCount);
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }
        #endregion

        #region PrintBuktiPendaftaranRspbt
        private string PrintBuktiPendaftaranRspbt(int id)
        {
            string result = string.Empty;
            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_FORMAT_BUKTI_PENDAFTARAN, Constant.SettingParameter.RM_ALAMAT_PRINTER_BUKTI_PENDAFTARAN, Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                if (lstParam != null)
                {
                    string printerType = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                    string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                    string printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                    if (hdnIsMultiLocation.Value == "1")
                    {
                        //Get Printer Url from Location DropDown
                        StandardCode oStandardCode = BusinessLayer.GetStandardCode(cboPrinterUrl.Value.ToString());
                        if (oStandardCode != null)
                        {
                            if (!String.IsNullOrEmpty(oStandardCode.TagProperty))
                            {
                                string[] tagField = oStandardCode.TagProperty.Split('|');
                                //Printer_Label| Printer_Wristband_Male;Printer_Wristband_Female;Printer_Wristband_Children_Male;Printer_Wristband_Children_Female|Printer_Wristband_Infant|Printer_RegistrationSlip
                                printerUrl = tagField[3];
                            }
                        }
                    }

                    Registration oRegistration = BusinessLayer.GetRegistration(id);
                    if (oRegistration != null)
                    {
                        vConsultVisit oVisit = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", oRegistration.RegistrationID)).FirstOrDefault();
                        if (oVisit != null)
                        {
                            switch (printerType)
                            {
                                case Constant.PrinterType.ZEBRA_PRINTER:
                                    ZebraPrinting.PrintBuktiPendaftaran(oVisit, printerUrl, printFormat);
                                    break;
                                case Constant.PrinterType.EPSON_DOT_MATRIX:
                                    ZebraPrinting.PrintBuktiPendaftaran3(oVisit, printerUrl, printFormat);
                                    break;
                                case Constant.PrinterType.DOT_MATRIX_FORMAT_1:
                                    result = ZebraPrinting.PrintBuktiPendaftaran4(oVisit, printerUrl, printFormat);
                                    break;
                            }
                        }
                        vConsultVisit9 oVisit9 = BusinessLayer.GetvConsultVisit9List(string.Format("RegistrationID = {0} AND IsMainVisit = 1", oRegistration.RegistrationID)).FirstOrDefault();
                        if (oVisit9 != null)
                        {
                            switch (printerType)
                            {
                                case Constant.PrinterType.THERMAL_RECEIPT_PRINTER:
                                    //Get Printer Address
                                    string ipAddress = HttpContext.Current.Request.UserHostAddress;

                                    filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                        ipAddress, Constant.DirectPrintType.BUKTI_PENDAFTARAN);

                                    List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);

                                    int labelCount = Convert.ToInt16(txtJmlLabel.Text);
                                    int maxLabelNo = Convert.ToInt16(hdnMaxLabelNo.Value);

                                    if (lstPrinter.Count > 0)
                                        ZebraPrinting.PrintBuktiPendaftaranRspbt(oVisit9, lstPrinter[0].PrinterName, labelCount > maxLabelNo ? maxLabelNo : labelCount);
                                    else
                                        result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                    break;
                                default:
                                    ZebraPrinting.PrintBuktiPendaftaran(oVisit, printerUrl, printFormat);
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }
        #endregion

        #region PrintBuktiPendaftaranRsdosoba
        private string PrintBuktiPendaftaranRsdosoba(int id)
        {
            string result = string.Empty;
            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_FORMAT_BUKTI_PENDAFTARAN, Constant.SettingParameter.RM_ALAMAT_PRINTER_BUKTI_PENDAFTARAN, Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                if (lstParam != null)
                {
                    string printerType = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                    string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                    string printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                    if (hdnIsMultiLocation.Value == "1")
                    {
                        //Get Printer Url from Location DropDown
                        StandardCode oStandardCode = BusinessLayer.GetStandardCode(cboPrinterUrl.Value.ToString());
                        if (oStandardCode != null)
                        {
                            if (!String.IsNullOrEmpty(oStandardCode.TagProperty))
                            {
                                string[] tagField = oStandardCode.TagProperty.Split('|');
                                //Printer_Label| Printer_Wristband_Male;Printer_Wristband_Female;Printer_Wristband_Children_Male;Printer_Wristband_Children_Female|Printer_Wristband_Infant|Printer_RegistrationSlip
                                printerUrl = tagField[3];
                            }
                        }
                    }

                    Registration oRegistration = BusinessLayer.GetRegistration(id);
                    if (oRegistration != null)
                    {
                        vConsultVisit oVisit = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", oRegistration.RegistrationID)).FirstOrDefault();
                        if (oVisit != null)
                        {
                            switch (printerType)
                            {
                                case Constant.PrinterType.ZEBRA_PRINTER:
                                    ZebraPrinting.PrintBuktiPendaftaran(oVisit, printerUrl, printFormat);
                                    break;
                                case Constant.PrinterType.EPSON_DOT_MATRIX:
                                    ZebraPrinting.PrintBuktiPendaftaran3(oVisit, printerUrl, printFormat);
                                    break;
                                case Constant.PrinterType.DOT_MATRIX_FORMAT_1:
                                    result = ZebraPrinting.PrintBuktiPendaftaran4(oVisit, printerUrl, printFormat);
                                    break;
                            }
                        }
                        vConsultVisit9 oVisit9 = BusinessLayer.GetvConsultVisit9List(string.Format("RegistrationID = {0} AND IsMainVisit = 1", oRegistration.RegistrationID)).FirstOrDefault();
                        if (oVisit9 != null)
                        {
                            switch (printerType)
                            {
                                case Constant.PrinterType.THERMAL_RECEIPT_PRINTER:
                                    //Get Printer Address
                                    string ipAddress = HttpContext.Current.Request.UserHostAddress;

                                    filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                        ipAddress, Constant.DirectPrintType.BUKTI_PENDAFTARAN);

                                    List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);

                                    if (lstPrinter.Count > 0)
                                        ZebraPrinting.PrintBuktiPendaftaranRsdosoba(oVisit9, lstPrinter[0].PrinterName);
                                    else
                                        result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                    break;
                                default:
                                    ZebraPrinting.PrintBuktiPendaftaran(oVisit, printerUrl, printFormat);
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }
        #endregion

        #region PrintBuktiPendaftaranRSCK
        private string PrintBuktiPendaftaranRSCK(int id)
        {
            string result = string.Empty;
            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_FORMAT_BUKTI_PENDAFTARAN, Constant.SettingParameter.RM_ALAMAT_PRINTER_BUKTI_PENDAFTARAN, Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                if (lstParam != null)
                {
                    string printerType = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                    string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                    string printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                    if (hdnIsMultiLocation.Value == "1")
                    {
                        //Get Printer Url from Location DropDown
                        StandardCode oStandardCode = BusinessLayer.GetStandardCode(cboPrinterUrl.Value.ToString());
                        if (oStandardCode != null)
                        {
                            if (!String.IsNullOrEmpty(oStandardCode.TagProperty))
                            {
                                string[] tagField = oStandardCode.TagProperty.Split('|');
                                //Printer_Label| Printer_Wristband_Male;Printer_Wristband_Female;Printer_Wristband_Children_Male;Printer_Wristband_Children_Female|Printer_Wristband_Infant|Printer_RegistrationSlip
                                printerUrl = tagField[3];
                            }
                        }
                    }

                    Registration oRegistration = BusinessLayer.GetRegistration(id);
                    if (oRegistration != null)
                    {
                        vConsultVisit oVisit = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", oRegistration.RegistrationID)).FirstOrDefault();
                        if (oVisit != null)
                        {
                            switch (printerType)
                            {
                                case Constant.PrinterType.ZEBRA_PRINTER:
                                    ZebraPrinting.PrintBuktiPendaftaran(oVisit, printerUrl, printFormat);
                                    break;
                                case Constant.PrinterType.EPSON_DOT_MATRIX:
                                    ZebraPrinting.PrintBuktiPendaftaran3(oVisit, printerUrl, printFormat);
                                    break;
                                case Constant.PrinterType.DOT_MATRIX_FORMAT_1:
                                    result = ZebraPrinting.PrintBuktiPendaftaran4(oVisit, printerUrl, printFormat);
                                    break;
                            }
                        }
                        vConsultVisit9 oVisit9 = BusinessLayer.GetvConsultVisit9List(string.Format("RegistrationID = {0} AND IsMainVisit = 1", oRegistration.RegistrationID)).FirstOrDefault();
                        if (oVisit9 != null)
                        {
                            switch (printerType)
                            {
                                case Constant.PrinterType.THERMAL_RECEIPT_PRINTER:
                                    //Get Printer Address
                                    string ipAddress = HttpContext.Current.Request.UserHostAddress;

                                    filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                        ipAddress, Constant.DirectPrintType.BUKTI_PENDAFTARAN);

                                    List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);

                                    if (lstPrinter.Count > 0)
                                        ZebraPrinting.PrintBuktiPendaftaranRSCK(oVisit9, lstPrinter[0].PrinterName);
                                    else
                                        result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                    break;
                                default:
                                    ZebraPrinting.PrintBuktiPendaftaran(oVisit, printerUrl, printFormat);
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }
        #endregion

        #region PrintOrderFarmasi
        private string PrintOrderFarmasi(int id)
        {
            string result = string.Empty;
            string ipAddress = HttpContext.Current.Request.UserHostAddress;
            string filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0", ipAddress, Constant.DirectPrintType.ORDER_FARMASI);
            List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);
            string printerUrl1 = lstPrinter.FirstOrDefault().PrinterName;

            //vPrescriptionOrderDt1 prescription = BusinessLayer.GetvPrescriptionOrderDt1List(string.Format("PrescriptionOrderID = {0} AND IsRFlag = 1", id)).FirstOrDefault();
            ZebraPrinting.PrintOrderFarmasi(id, printerUrl1);
            return result;
        }
        #endregion

        #region PrintBuktiPendaftaranThermal
        private string PrintBuktiPendaftaranThermal(int id)
        {
            string result = string.Empty;
            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_FORMAT_BUKTI_PENDAFTARAN, Constant.SettingParameter.RM_ALAMAT_PRINTER_BUKTI_PENDAFTARAN, Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                if (lstParam != null)
                {
                    string printerType = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                    string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                    string printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                    if (hdnIsMultiLocation.Value == "1")
                    {
                        //Get Printer Url from Location DropDown
                        StandardCode oStandardCode = BusinessLayer.GetStandardCode(cboPrinterUrl.Value.ToString());
                        if (oStandardCode != null)
                        {
                            if (!String.IsNullOrEmpty(oStandardCode.TagProperty))
                            {
                                string[] tagField = oStandardCode.TagProperty.Split('|');
                                //Printer_Label| Printer_Wristband_Male;Printer_Wristband_Female;Printer_Wristband_Children_Male;Printer_Wristband_Children_Female|Printer_Wristband_Infant|Printer_RegistrationSlip
                                printerUrl = tagField[3];
                            }
                        }
                    }

                    Registration oRegistration = BusinessLayer.GetRegistration(id);
                    if (oRegistration != null)
                    {
                        vConsultVisit oVisit = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", oRegistration.RegistrationID)).FirstOrDefault();
                        if (oVisit != null)
                        {
                            switch (printerType)
                            {
                                case Constant.PrinterType.ZEBRA_PRINTER:
                                    ZebraPrinting.PrintBuktiPendaftaran(oVisit, printerUrl, printFormat);
                                    break;
                                case Constant.PrinterType.EPSON_DOT_MATRIX:
                                    ZebraPrinting.PrintBuktiPendaftaran3(oVisit, printerUrl, printFormat);
                                    break;
                                case Constant.PrinterType.DOT_MATRIX_FORMAT_1:
                                    result = ZebraPrinting.PrintBuktiPendaftaranThermal(oVisit, printerUrl, printFormat);
                                    break;
                                default:
                                    ZebraPrinting.PrintBuktiPendaftaran(oVisit, printerUrl, printFormat);
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }
        #endregion

        #region PrintBuktiPendaftaranPerjanjian
        private string PrintBuktiPendaftaranPerjanjian(int id)
        {
            string result = string.Empty;
            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_FORMAT_BUKTI_PENDAFTARAN, Constant.SettingParameter.RM_ALAMAT_PRINTER_BUKTI_PENDAFTARAN, Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                if (lstParam != null)
                {
                    string printerType = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                    string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                    string printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                    if (hdnIsMultiLocation.Value == "1")
                    {
                        //Get Printer Url from Location DropDown
                        StandardCode oStandardCode = BusinessLayer.GetStandardCode(cboPrinterUrl.Value.ToString());
                        if (oStandardCode != null)
                        {
                            if (!String.IsNullOrEmpty(oStandardCode.TagProperty))
                            {
                                string[] tagField = oStandardCode.TagProperty.Split('|');
                                //Printer_Label| Printer_Wristband_Male;Printer_Wristband_Female;Printer_Wristband_Children_Male;Printer_Wristband_Children_Female|Printer_Wristband_Infant|Printer_RegistrationSlip
                                printerUrl = tagField[3];
                            }
                        }
                    }
                    vAppointment oAppointment = BusinessLayer.GetvAppointmentList(string.Format("AppointmentID = {0}", id)).FirstOrDefault();
                    if (oAppointment != null)
                    {
                        switch (printerType)
                        {
                            case Constant.PrinterType.THERMAL_RECEIPT_PRINTER:
                                //Get Printer Address
                                string ipAddress = HttpContext.Current.Request.UserHostAddress;

                                filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                    ipAddress, Constant.DirectPrintType.BUKTI_PENDAFTARAN_PERJANJIAN);
                                
                                List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);
                                if (lstPrinter.Count > 0)
                                {
                                    if (printFormat == Constant.PrintFormat.BUKTI_PENDAFTARAN_RSSY)
                                    {
                                        ZebraPrinting.PrintBuktiPerjanjianRSSY(oAppointment, lstPrinter[0].PrinterName);
                                    }
                                    else
                                    {
                                        ZebraPrinting.PrintBuktiPerjanjian(oAppointment, lstPrinter[0].PrinterName);
                                    }
                                }
                                else
                                {
                                    result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }
        #endregion

        #region PrintBuktiPendaftaranPerjanjian2
        private string PrintBuktiPendaftaranPerjanjian2(int id)
        {
            string result = string.Empty;
            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_FORMAT_BUKTI_PENDAFTARAN, Constant.SettingParameter.RM_ALAMAT_PRINTER_BUKTI_PENDAFTARAN, Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                if (lstParam != null)
                {
                    string printerType = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                    string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                    string printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                    if (hdnIsMultiLocation.Value == "1")
                    {
                        //Get Printer Url from Location DropDown
                        StandardCode oStandardCode = BusinessLayer.GetStandardCode(cboPrinterUrl.Value.ToString());
                        if (oStandardCode != null)
                        {
                            if (!String.IsNullOrEmpty(oStandardCode.TagProperty))
                            {
                                string[] tagField = oStandardCode.TagProperty.Split('|');
                                //Printer_Label| Printer_Wristband_Male;Printer_Wristband_Female;Printer_Wristband_Children_Male;Printer_Wristband_Children_Female|Printer_Wristband_Infant|Printer_RegistrationSlip
                                printerUrl = tagField[3];
                            }
                        }
                    }
                    vAppointment oAppointment = BusinessLayer.GetvAppointmentList(string.Format("AppointmentID = {0}", id)).FirstOrDefault();
                    if (oAppointment != null)
                    {
                        switch (printerType)
                        {
                            case Constant.PrinterType.THERMAL_RECEIPT_PRINTER:
                                //Get Printer Address
                                string ipAddress = HttpContext.Current.Request.UserHostAddress;

                                filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                    ipAddress, Constant.DirectPrintType.BUKTI_PENDAFTARAN_PERJANJIAN);

                                List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);

                                if (lstPrinter.Count > 0)
                                    ZebraPrinting.PrintBuktiPerjanjian2(oAppointment, lstPrinter[0].PrinterName);
                                else
                                    result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }
        #endregion

        #region PrintSlipAntrianRawatJalan
        private string PrintSlipAntrianRawatJalan(int id)
        {
            string result = string.Empty;
            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_FORMAT_BUKTI_PENDAFTARAN, Constant.SettingParameter.RM_ALAMAT_PRINTER_BUKTI_PENDAFTARAN, Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN);
                
                Healthcare oHealthcare = BusinessLayer.GetHealthcare(AppSession.UserLogin.HealthcareID);
                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                if (lstParam != null)
                {
                    string printerType = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                    string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                    string printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                    if (hdnIsMultiLocation.Value == "1")
                    {
                        //Get Printer Url from Location DropDown
                        StandardCode oStandardCode = BusinessLayer.GetStandardCode(cboPrinterUrl.Value.ToString());
                        if (oStandardCode != null)
                        {
                            if (!String.IsNullOrEmpty(oStandardCode.TagProperty))
                            {
                                string[] tagField = oStandardCode.TagProperty.Split('|');
                                //Printer_Label| Printer_Wristband_Male;Printer_Wristband_Female;Printer_Wristband_Children_Male;Printer_Wristband_Children_Female|Printer_Wristband_Infant|Printer_RegistrationSlip
                                printerUrl = tagField[3];
                            }
                        }
                    }

                    Registration oRegistration = BusinessLayer.GetRegistration(id);
                    if (oRegistration != null)
                    {
                        vConsultVisit oVisit = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", oRegistration.RegistrationID)).FirstOrDefault();
                        if (oVisit != null)
                        {
                            switch (printerType)
                            {
                                case Constant.PrinterType.ZEBRA_PRINTER:
                                    ZebraPrinting.PrintBuktiPendaftaran(oVisit, printerUrl, printFormat);
                                    break;
                                case Constant.PrinterType.EPSON_DOT_MATRIX:
                                    ZebraPrinting.PrintBuktiPendaftaran3(oVisit, printerUrl, printFormat);
                                    break;
                                case Constant.PrinterType.DOT_MATRIX_FORMAT_1:
                                    result = ZebraPrinting.PrintBuktiPendaftaran4(oVisit, printerUrl, printFormat);
                                    break;
                            }
                        }
                        vConsultVisit9 oVisit9 = BusinessLayer.GetvConsultVisit9List(string.Format("RegistrationID = {0} AND IsMainVisit = 1", oRegistration.RegistrationID)).FirstOrDefault();
                        if (oVisit9 != null)
                        {
                            switch (printerType)
                            {
                                case Constant.PrinterType.THERMAL_RECEIPT_PRINTER:

                                    if (!string.IsNullOrEmpty(printerUrl))
                                    {
                                        if (oHealthcare.Initial == "RSBL")
                                        {
                                            ZebraPrinting.PrintSlipAntrianRawatJalan(oVisit9, printerUrl);
                                        }
                                        else if (oHealthcare.Initial == "RSSA")
                                        {
                                            ZebraPrinting.PrintSlipAntrianRawatJalanRSSA(oVisit9, printerUrl);
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }
        #endregion

        #region PrintSlipGizi
        private string PrintSlipGizi(int id)
        {
            string result = string.Empty;
            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_FORMAT_BUKTI_PENDAFTARAN, Constant.SettingParameter.RM_ALAMAT_PRINTER_BUKTI_PENDAFTARAN, Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                if (lstParam != null)
                {
                    string printerType = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                    string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                    string printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                    if (hdnIsMultiLocation.Value == "1")
                    {
                        //Get Printer Url from Location DropDown
                        StandardCode oStandardCode = BusinessLayer.GetStandardCode(cboPrinterUrl.Value.ToString());
                        if (oStandardCode != null)
                        {
                            if (!String.IsNullOrEmpty(oStandardCode.TagProperty))
                            {
                                string[] tagField = oStandardCode.TagProperty.Split('|');
                                printerUrl = tagField[3];
                            }
                        }
                    }
                    vAppointment oAppointment = BusinessLayer.GetvAppointmentList(string.Format("AppointmentID = {0}", id)).FirstOrDefault();
                    if (oAppointment != null)
                    {
                        switch (printerType)
                        {
                            case Constant.PrinterType.THERMAL_RECEIPT_PRINTER:
                                //Get Printer Address
                                string ipAddress = HttpContext.Current.Request.UserHostAddress;

                                filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                    ipAddress, Constant.DirectPrintType.BUKTI_PENDAFTARAN_PERJANJIAN);

                                List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);

                                if (lstPrinter.Count > 0)
                                    ZebraPrinting.PrintBuktiPerjanjian(oAppointment, lstPrinter[0].PrinterName);
                                else
                                    result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }
        #endregion

        #region PrintNutritionWorklist
        private string PrintNutritionWorklist(string date, string mealPlan, string serviceUnit)
        {
            string result = string.Empty;
            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_FORMAT_BUKTI_PENDAFTARAN, Constant.SettingParameter.RM_ALAMAT_PRINTER_BUKTI_PENDAFTARAN, Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                if (lstParam != null)
                {
                    string printerType = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;

                    List<vNutritionWorkListInformation> oNutrition = BusinessLayer.GetvNutritionWorkListInformationList(string.Format("ScheduleDate = '{0}' AND GCMealTime = '{1}' AND ServiceUnitCode = '{2}'", date, mealPlan, serviceUnit));
                    if (oNutrition != null)
                    {
                        switch (printerType)
                        {
                            case Constant.PrinterType.THERMAL_RECEIPT_PRINTER:
                                //Get Printer Address
                                string ipAddress = HttpContext.Current.Request.UserHostAddress;

                                filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                    ipAddress, Constant.DirectPrintType.FORM_PERMINTAAN_MAKANAN);

                                List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);

                                if (lstPrinter.Count > 0)
                                    ZebraPrinting.PrintNutritionWorklist(oNutrition, lstPrinter[0].PrinterName);
                                else
                                    result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }
        #endregion

        #region PrintNutritionPatientOrder
        private string PrintNutritionPatientOrder(string date, string mealPlan)
        {
            string result = string.Empty;
            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_FORMAT_BUKTI_PENDAFTARAN, Constant.SettingParameter.RM_ALAMAT_PRINTER_BUKTI_PENDAFTARAN, Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                if (lstParam != null)
                {
                    string printerType = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;

                    List<vNutritionOrderDtWorkList1> oNutrition = BusinessLayer.GetvNutritionOrderDtWorkList1List(string.Format("ScheduleDate = '{0}' AND GCMealTime = '{1}'", date, mealPlan));
                    if (oNutrition != null)
                    {
                        string ipAddress = HttpContext.Current.Request.UserHostAddress;
                        switch (printerType)
                        {
                            case Constant.PrinterType.THERMAL_RECEIPT_PRINTER:
                                //Get Printer Address


                                filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                    ipAddress, Constant.DirectPrintType.BON_PERMINTAAN_MAKANAN);

                                List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);

                                if (lstPrinter.Count > 0)
                                    ZebraPrinting.PrintNutritionPatientOrder(oNutrition, lstPrinter[0].PrinterName);
                                else
                                    result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                break;
                            case Constant.PrinterType.THERMAL_FORMAT_RSSES:

                                filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                    ipAddress, Constant.DirectPrintType.BON_PERMINTAAN_MAKANAN);
                                List<PrinterLocation> lstPrinter1 = BusinessLayer.GetPrinterLocationList(filterExp);

                                if (lstPrinter1.Count > 0)
                                    ZebraPrinting.PrintNutritionPatientOrder(oNutrition, lstPrinter1[0].PrinterName);
                                else
                                    result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }
        #endregion

        #region PrintJobOrderLaboratory
        private string PrintJobOrderLaboratory(int id)
        {
            string result = string.Empty;
            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_FORMAT_BUKTI_PENDAFTARAN, Constant.SettingParameter.RM_ALAMAT_PRINTER_BUKTI_PENDAFTARAN, Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                if (lstParam != null)
                {
                    string printerType = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;

                    List<vJobOrderLab> oTestOrderDt = BusinessLayer.GetvJobOrderLabList(string.Format("TestOrderID = {0}", id));
                    if (oTestOrderDt != null)
                    {
                        switch (printerType)
                        {
                            case Constant.PrinterType.THERMAL_RECEIPT_PRINTER:
                                //Get Printer Address
                                string ipAddress = HttpContext.Current.Request.UserHostAddress;

                                filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                    ipAddress, Constant.DirectPrintType.JOB_ORDER_LABORATORIUM);

                                List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);

                                if (lstPrinter.Count > 0)
                                    ZebraPrinting.PrintJobOrderLaboratory(oTestOrderDt, lstPrinter[0].PrinterName);
                                else
                                    result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }
        #endregion

        #region PrintDaftarPasienDirawat
        private string PrintDaftarPasienDirawat()
        {
            string result = string.Empty;
            try
            {
                result = ZebraPrinting.PrintDaftarPasienRanap();
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }
        #endregion

        #region PrintBuktiPemeriksaanLab
        private string PrintBuktiPemeriksaanLab(int id)
        {
            string result = string.Empty;
            try
            {
                result = ZebraPrinting.PrintBuktiPemeriksaanLab(id);
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }
        #endregion

        #region PrintStickerGelang
        private string PrintStickerGelang(int id)
        {
            string result = string.Empty;

            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_FORMAT_GELANG_PASIEN_DEWASA, Constant.SettingParameter.RM_FORMAT_GELANG_PASIEN_ANAK, Constant.SettingParameter.RM_FORMAT_GELANG_PASIEN_BAYI,
                    Constant.SettingParameter.RM_GELANG_PASIEN_ANAK_KHUSUS, Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_LAKI,
                    Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_PEREMPUAN, Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_ANAK,
                    Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_BAYI);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                if (lstParam != null)
                {
                    Registration oRegistration = BusinessLayer.GetRegistration(id);
                    if (oRegistration != null)
                    {
                        vConsultVisit oVisit = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", oRegistration.RegistrationID)).FirstOrDefault();
                        if (oVisit != null)
                        {

                            string printFormat = string.Empty;
                            string printerUrl = string.Empty;

                            if (hdnIsMultiLocation.Value == "0")
                            {
                                printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_GELANG_PASIEN_DEWASA)).FirstOrDefault().ParameterValue;
                                //Choose Printer Url based on Patient Gender
                                switch (oVisit.GCGender)
                                {
                                    case Constant.Gender.MALE:
                                        printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_LAKI)).FirstOrDefault().ParameterValue;
                                        break;
                                    case Constant.Gender.FEMALE:
                                        printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_PEREMPUAN)).FirstOrDefault().ParameterValue;
                                        break;
                                    default:
                                        printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_LAKI)).FirstOrDefault().ParameterValue;
                                        break;
                                }
                            }
                            else
                            {
                                //Get Printer Url from Location DropDown
                                StandardCode oStandardCode = BusinessLayer.GetStandardCode(cboPrinterUrl.Value.ToString());
                                if (oStandardCode != null)
                                {
                                    if (!String.IsNullOrEmpty(oStandardCode.TagProperty))
                                    {
                                        string[] tagField = oStandardCode.TagProperty.Split('|');
                                        //Printer_Label| Printer_Wristband_Male;Printer_Wristband_Female;Printer_Wristband_Children_Male;Printer_Wristband_Children_Female|Printer_Wristband_Infant|Printer_RegistrationSlip
                                        string[] wristbandUrl = tagField[1].Split(';');
                                        printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_GELANG_PASIEN_DEWASA)).FirstOrDefault().ParameterValue;
                                        //Choose Printer Url based on Patient Gender
                                        switch (oVisit.GCGender)
                                        {
                                            case Constant.Gender.MALE:
                                                printerUrl = wristbandUrl[0];
                                                break;
                                            case Constant.Gender.FEMALE:
                                                printerUrl = wristbandUrl[1];
                                                break;
                                            default:
                                                printerUrl = wristbandUrl[0];
                                                break;
                                        }
                                    }
                                }

                            }
                            if (!string.IsNullOrEmpty(printerUrl))
                            {
                                int labelCount = Convert.ToInt16(txtJmlLabel.Text);
                                ZebraPrinting.PrintStikerGelang(id, printerUrl, labelCount);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }
        #endregion

        #region PrintStickerRawatInap
        private string PrintStickerRawatInap(int id)
        {
            string result = string.Empty;
            try
            {
                result = ZebraPrinting.PrintStikerRI(id, "");
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }
        #endregion

        #region PrintStickerRawatJalan
        private string PrintStickerRawatJalan(int id)
        {
            string result = string.Empty;
            try
            {
                result = ZebraPrinting.PrintStikerRJ(id, "");
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }
        #endregion

        #region PrintBuktiPemeriksaan
        private string PrintBuktiPemeriksaan(int id)
        {
            string result = string.Empty;
            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_FORMAT_BUKTI_PENDAFTARAN, Constant.SettingParameter.RM_ALAMAT_PRINTER_BUKTI_PENDAFTARAN, Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                if (lstParam != null)
                {
                    string printerType = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_JENIS_PRINTER_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                    string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                    string printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;
                    if (hdnIsMultiLocation.Value == "1")
                    {
                        //Get Printer Url from Location DropDown
                        StandardCode oStandardCode = BusinessLayer.GetStandardCode(cboPrinterUrl.Value.ToString());
                        if (oStandardCode != null)
                        {
                            if (!String.IsNullOrEmpty(oStandardCode.TagProperty))
                            {
                                string[] tagField = oStandardCode.TagProperty.Split('|');
                                //Printer_Label| Printer_Wristband_Male;Printer_Wristband_Female;Printer_Wristband_Children_Male;Printer_Wristband_Children_Female|Printer_Wristband_Infant|Printer_RegistrationSlip
                                printerUrl = tagField[3];
                            }
                        }
                    }

                    switch (printerType)
                    {
                        case Constant.PrinterType.DOT_MATRIX_FORMAT_1:
                            result = ZebraPrinting.PrintBuktiPemeriksaan(id, printerUrl, printFormat);
                            break;
                        default:
                            ZebraPrinting.PrintBuktiPemeriksaan(id, printerUrl, printFormat);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }
        #endregion

        #region PrintLabelMR
        //pending
        private string PrintLabelMR(int id)
        {
            string result = string.Empty;

            try
            {
                result = ZebraPrinting.PrintLabelMR(id, "");
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }
        #endregion

        #region PrintMROutguides
        private string PrintMROutguides(int id)
        {
            string result = string.Empty;
            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_CETAK_TRACER_OTOMATIS, Constant.SettingParameter.RM_CETAK_TRACER_PASIEN_BARU, Constant.SettingParameter.RM_JENIS_PRINTER_TRACER,
                    Constant.SettingParameter.RM_FORMAT_CETAKAN_TRACER, Constant.SettingParameter.RM_ALAMAT_PRINTER_TRACER);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                if (lstParam != null)
                {
                    bool isAutoPrint = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_CETAK_TRACER_OTOMATIS)).FirstOrDefault().ParameterValue == "1" ? true : false;
                    string printerType = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_JENIS_PRINTER_TRACER)).FirstOrDefault().ParameterValue;
                    string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_CETAKAN_TRACER)).FirstOrDefault().ParameterValue;
                    string printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_TRACER)).FirstOrDefault().ParameterValue;

                    Registration oRegistration = BusinessLayer.GetRegistration(id);
                    if (oRegistration != null)
                    {
                        vConsultVisit oVisit = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", oRegistration.RegistrationID)).FirstOrDefault();
                        if (oVisit != null)
                        {
                            //Check Printer Type
                            switch (printerType)
                            {
                                case Constant.PrinterType.ZEBRA_PRINTER:
                                    ZebraPrinting.PrintTracerRM(oVisit, printerUrl, printFormat);
                                    //ZebraPrinting.PrintTracerRM(oVisit, printerUrl, printFormat);
                                    break;
                                case Constant.PrinterType.DOT_MATRIX_FORMAT_1:
                                    ZebraPrinting.PrintTracerRMDotMatrix(oVisit, printerUrl, printFormat);
                                    break;
                                case Constant.PrinterType.THERMAL_RECEIPT_PRINTER:
                                    //Get Printer Address
                                    string ipAddress = HttpContext.Current.Request.UserHostAddress;

                                    filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                        ipAddress, Constant.DirectPrintType.TRACER_RM);

                                    List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);

                                    if (lstPrinter.Count > 0)
                                        ZebraPrinting.PrintTracerRM1(oVisit, lstPrinter[0].PrinterName);
                                    else
                                        result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                    break;
                                case Constant.PrinterType.THERMAL_RECEIPT_PRINTER_1:
                                    //Get Printer Address
                                    string ipAddress1 = HttpContext.Current.Request.UserHostAddress;

                                    filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                        ipAddress1, Constant.DirectPrintType.TRACER_RM);

                                    List<PrinterLocation> lstPrinter1 = BusinessLayer.GetPrinterLocationList(filterExp);

                                    if (lstPrinter1.Count > 0)
                                        ZebraPrinting.PrintTracerRM2(oVisit, lstPrinter1[0].PrinterName);
                                    else
                                        result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress1);
                                    break;
                                case Constant.PrinterType.EPSON_DOT_MATRIX_RSRA:
                                    //Get Printer Address
                                    string ipAddress2 = HttpContext.Current.Request.UserHostAddress;

                                    filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                        ipAddress2, Constant.DirectPrintType.TRACER_RM);

                                    List<PrinterLocation> lstPrinter2 = BusinessLayer.GetPrinterLocationList(filterExp);

                                    if (lstPrinter2.Count > 0)
                                        ZebraPrinting.PrintTracerRMRSRA(oVisit, lstPrinter2[0].PrinterName);
                                    else
                                        result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress2);
                                    break;
                                default:
                                    ZebraPrinting.PrintTracerRM(oVisit, printerUrl, printFormat);
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }
        #endregion

        #region PrintAppointmentMROutguides

        private string PrintAppointmentMROutguides(int id)
        {
            string result = string.Empty;
            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_CETAK_TRACER_OTOMATIS, Constant.SettingParameter.RM_CETAK_TRACER_PASIEN_BARU, Constant.SettingParameter.RM_JENIS_PRINTER_TRACER,
                    Constant.SettingParameter.RM_FORMAT_CETAKAN_TRACER_APPOINTMENT, Constant.SettingParameter.RM_ALAMAT_PRINTER_TRACER);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                if (lstParam != null)
                {
                    bool isAutoPrint = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_CETAK_TRACER_OTOMATIS)).FirstOrDefault().ParameterValue == "1" ? true : false;
                    string printerType = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_JENIS_PRINTER_TRACER)).FirstOrDefault().ParameterValue;
                    string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_CETAKAN_TRACER_APPOINTMENT)).FirstOrDefault().ParameterValue;
                    string printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_TRACER)).FirstOrDefault().ParameterValue;

                    if (printFormat == Constant.PrintFormat.TRACER_APPOINTMENT_RSSMP)
                    {
                        Appointment oAppointment = BusinessLayer.GetAppointment(id);
                        if (oAppointment != null)
                        {
                            vAppointment oApp = BusinessLayer.GetvAppointmentList(string.Format("AppointmentID = {0}", oAppointment.AppointmentID)).FirstOrDefault();
                            if (oApp != null)
                            {
                                switch (printerType)
                                {
                                    default:
                                        ZebraPrinting.PrintTracerAppointmentRM(oApp, printerUrl, printFormat);
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }

        #endregion

        #region PrintMROutguidesAppointment
        private string PrintMROutguidesAppointment(int id)
        {
            string result = string.Empty;
            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_CETAK_TRACER_OTOMATIS, Constant.SettingParameter.RM_CETAK_TRACER_PASIEN_BARU, Constant.SettingParameter.RM_JENIS_PRINTER_TRACER,
                    Constant.SettingParameter.RM_FORMAT_CETAKAN_TRACER, Constant.SettingParameter.RM_ALAMAT_PRINTER_TRACER);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                if (lstParam != null)
                {
                    bool isAutoPrint = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_CETAK_TRACER_OTOMATIS)).FirstOrDefault().ParameterValue == "1" ? true : false;
                    string printerType = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_JENIS_PRINTER_TRACER)).FirstOrDefault().ParameterValue;
                    string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_CETAKAN_TRACER)).FirstOrDefault().ParameterValue;
                    string printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_TRACER)).FirstOrDefault().ParameterValue;

                    vAppointment oAppointment = BusinessLayer.GetvAppointmentList(string.Format("AppointmentID = {0} AND GCAppointmentStatus != '{1}'", id, Constant.AppointmentStatus.DELETED)).FirstOrDefault();
                    if (oAppointment != null)
                    {
                        //Check Printer Type
                        switch (printerType)
                        {
                            case Constant.PrinterType.THERMAL_RECEIPT_PRINTER_1:
                                //Get Printer Address
                                string ipAddress1 = HttpContext.Current.Request.UserHostAddress;

                                filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                    ipAddress1, Constant.DirectPrintType.TRACER_RM);

                                List<PrinterLocation> lstPrinter1 = BusinessLayer.GetPrinterLocationList(filterExp);

                                if (lstPrinter1.Count > 0)
                                    ZebraPrinting.PrintTracerRM3(oAppointment, lstPrinter1[0].PrinterName);
                                else
                                    result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress1);
                                break;
                            default:
                                ZebraPrinting.PrintTracerRM3(oAppointment, printerUrl);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }
        #endregion

        #region PrintMROutguidesAppointmentPerDay
        private string PrintMROutguidesAppointmentPerDay(string param)
        {
            string result = string.Empty;
            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_CETAK_TRACER_OTOMATIS, Constant.SettingParameter.RM_CETAK_TRACER_PASIEN_BARU, Constant.SettingParameter.RM_JENIS_PRINTER_TRACER,
                    Constant.SettingParameter.RM_FORMAT_CETAKAN_TRACER, Constant.SettingParameter.RM_ALAMAT_PRINTER_TRACER);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                if (lstParam != null)
                {
                    bool isAutoPrint = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_CETAK_TRACER_OTOMATIS)).FirstOrDefault().ParameterValue == "1" ? true : false;
                    string printerType = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_JENIS_PRINTER_TRACER)).FirstOrDefault().ParameterValue;
                    string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_CETAKAN_TRACER)).FirstOrDefault().ParameterValue;
                    string printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_TRACER)).FirstOrDefault().ParameterValue;

                    List<vAppointment> lstAppointment = BusinessLayer.GetvAppointmentList(string.Format("StartDate = '{0}' AND GCAppointmentStatus NOT IN ('{1}','{2}')", Helper.GetDatePickerValue(param), Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.COMPLETE));
                    if (lstAppointment.Count > 0)
                    {
                        //Check Printer Type
                        switch (printerType)
                        {
                            case Constant.PrinterType.THERMAL_RECEIPT_PRINTER_1:
                                //Get Printer Address
                                string ipAddress1 = HttpContext.Current.Request.UserHostAddress;

                                filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0",
                                    ipAddress1, Constant.DirectPrintType.TRACER_RM);

                                List<PrinterLocation> lstPrinter1 = BusinessLayer.GetPrinterLocationList(filterExp);

                                if (lstPrinter1.Count > 0)
                                    ZebraPrinting.PrintTracerRM4(lstAppointment, lstPrinter1[0].PrinterName);
                                else
                                    result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress1);
                                break;
                            default:
                                ZebraPrinting.PrintTracerRM4(lstAppointment, printerUrl);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }
        #endregion

        #region PrintMRCoverLabel
        private string PrintMRCoverLabel(int id)
        {
            string result = string.Empty;

            //Get Printing Configuration
            string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')",
                AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_FORMAT_CETAKAN_COVER, Constant.SettingParameter.RM_ALAMAT_PRINTER_COVER);

            List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
            if (lstParam != null)
            {
                string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_CETAKAN_COVER)).FirstOrDefault().ParameterValue;
                string printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_COVER)).FirstOrDefault().ParameterValue;

                try
                {
                    Registration oRegistration = BusinessLayer.GetRegistration(id);
                    if (oRegistration != null)
                    {
                        ConsultVisit oVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", oRegistration.RegistrationID)).FirstOrDefault();
                        if (oVisit != null)
                        {
                            vPatient oPatient = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", oRegistration.MRN)).FirstOrDefault();
                            ZebraPrinting.PrintCoverRM(oPatient, printerUrl, printFormat);
                        }
                    }
                }
                catch (Exception ex)
                {
                    result = ex.Message;
                    Helper.InsertErrorLog(ex);
                }
            }

            return result;
        }
        #endregion

        #region PrintMRCoverLabelRSBL
        private string PrintMRCoverLabelRSBL(int id)
        {
            string result = string.Empty;

            //Get Printing Configuration
            string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')",
                AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_FORMAT_CETAKAN_COVER, Constant.SettingParameter.RM_ALAMAT_PRINTER_COVER);

            List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
            if (lstParam != null)
            {
                string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_CETAKAN_COVER)).FirstOrDefault().ParameterValue;

                try
                {
                    string printerUrl = string.Empty;

                    Registration oRegistration = BusinessLayer.GetRegistration(id);
                    if (oRegistration != null)
                    {
                        vConsultVisit oVisit = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", oRegistration.RegistrationID)).FirstOrDefault();
                        if (oVisit != null)
                        {
                            if (hdnIsMultiLocation.Value == "0")
                            {
                                switch (oVisit.DepartmentID)
                                {
                                    case Constant.Facility.EMERGENCY:
                                        printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_LABEL_RD)).FirstOrDefault().ParameterValue;
                                        break;
                                    case Constant.Facility.OUTPATIENT:
                                        printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_LABEL_RJ)).FirstOrDefault().ParameterValue;
                                        break;
                                    case Constant.Facility.INPATIENT:
                                        printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_LABEL_RI)).FirstOrDefault().ParameterValue;
                                        break;
                                    default:
                                        printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_LABEL_MD)).FirstOrDefault().ParameterValue;
                                        break;
                                }
                            }
                            else
                            {
                                //Get Printer Url from Location DropDown
                                StandardCode oStandardCode = BusinessLayer.GetStandardCode(cboPrinterUrl.Value.ToString());
                                if (oStandardCode != null)
                                {
                                    if (!String.IsNullOrEmpty(oStandardCode.TagProperty))
                                    {
                                        string[] tagField = oStandardCode.TagProperty.Split('|');
                                        //Printer_Label| Printer_Wristband_Male;Printer_Wristband_Female;Printer_Wristband_Children_Male;Printer_Wristband_Children_Female|Printer_Wristband_Infant|Printer_RegistrationSlip
                                        printerUrl = tagField[0].TrimEnd();
                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(printerUrl))
                            {
                                vPatient oPatient = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", oRegistration.MRN)).FirstOrDefault();
                                ZebraPrinting.PrintCoverRM(oPatient, printerUrl, printFormat);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    result = ex.Message;
                    Helper.InsertErrorLog(ex);
                }
            }

            return result;
        }
        #endregion

        #region PrintMRLabel
        private string PrintMRLabel(int id, string reportCode)
        {
            string errorMsg = string.Empty;

            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_FORMAT_CETAKAN_LABEL, Constant.SettingParameter.RM_FORMAT_CETAKAN_LABEL_2,
                    Constant.SettingParameter.RM_JUMLAH_CETAKAN_LABEL_RD, Constant.SettingParameter.RM_JUMLAH_CETAKAN_LABEL_RJ,
                    Constant.SettingParameter.RM_JUMLAH_CETAKAN_LABEL_RI, Constant.SettingParameter.RM_JUMLAH_CETAKAN_LABEL_MD,
                    Constant.SettingParameter.RM_ALAMAT_PRINTER_LABEL_RD, Constant.SettingParameter.RM_ALAMAT_PRINTER_LABEL_RJ,
                    Constant.SettingParameter.RM_ALAMAT_PRINTER_LABEL_RI, Constant.SettingParameter.RM_ALAMAT_PRINTER_LABEL_MD);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                if (lstParam != null)
                {
                    Registration oRegistration = BusinessLayer.GetRegistration(id);
                    if (oRegistration != null)
                    {
                        vLabelPatientRegistrationInfo oData = BusinessLayer.GetvLabelPatientRegistrationInfoList(string.Format("RegistrationID = {0}", oRegistration.RegistrationID)).FirstOrDefault();
                        if (oData != null)
                        {
                            string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_CETAKAN_LABEL)).FirstOrDefault().ParameterValue;
                            if (reportCode == BusinessLayer.GetSettingParameterDt("001", Constant.SettingParameter.RM_REPORT_CODE_CUSTOM_LABEL).ParameterValue)
                            {
                                printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_CETAKAN_LABEL_2)).FirstOrDefault().ParameterValue;
                            }

                            string printerUrl = string.Empty;

                            if (hdnIsMultiLocation.Value == "0")
                            {
                                switch (oData.DepartmentID)
                                {
                                    case Constant.Facility.EMERGENCY:
                                        printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_LABEL_RD)).FirstOrDefault().ParameterValue;
                                        break;
                                    case Constant.Facility.OUTPATIENT:
                                        printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_LABEL_RJ)).FirstOrDefault().ParameterValue;
                                        break;
                                    case Constant.Facility.INPATIENT:
                                        printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_LABEL_RI)).FirstOrDefault().ParameterValue;
                                        break;
                                    default:
                                        printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_LABEL_MD)).FirstOrDefault().ParameterValue;
                                        break;
                                }
                            }
                            else
                            {
                                //Get Printer Url from Location DropDown
                                StandardCode oStandardCode = BusinessLayer.GetStandardCode(cboPrinterUrl.Value.ToString());
                                if (oStandardCode != null)
                                {
                                    if (!String.IsNullOrEmpty(oStandardCode.TagProperty))
                                    {
                                        string[] tagField = oStandardCode.TagProperty.Split('|');
                                        //Printer_Label| Printer_Wristband_Male;Printer_Wristband_Female;Printer_Wristband_Children_Male;Printer_Wristband_Children_Female|Printer_Wristband_Infant|Printer_RegistrationSlip
                                        printerUrl = tagField[0].TrimEnd();
                                    }
                                }

                            }

                            if (!string.IsNullOrEmpty(printerUrl))
                            {
                                int labelCount = Convert.ToInt16(txtJmlLabel.Text);
                                int maxLabelNo = Convert.ToInt16(hdnMaxLabelNo.Value);

                                if (printFormat == Constant.PrintFormat.BROTHER_LABELRM_RSSY)
                                {
                                    string ipAddress = HttpContext.Current.Request.UserHostAddress;
                                    filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted = 0", ipAddress, Constant.DirectPrintType.LABEL_RM);
                                    List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);
                                    if (lstPrinter.Count > 0)
                                    {
                                        printerUrl = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.LABEL_RM).FirstOrDefault().PrinterName;
                                    }
                                    BrotherPrinting.PrintLabelRM_RSSY(oData, printerUrl, printFormat, labelCount);
                                }
                                if (printFormat == Constant.PrintFormat.BROTHER_LABELRM_RSSK)
                                {
                                    string ipAddress = HttpContext.Current.Request.UserHostAddress;
                                    filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted = 0", ipAddress, Constant.DirectPrintType.LABEL_RM);
                                    List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);
                                    if (lstPrinter.Count > 0)
                                    {
                                        printerUrl = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.LABEL_RM).FirstOrDefault().PrinterName;
                                        bool isUsePrintingTools = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.LABEL_RM).FirstOrDefault().IsUsingPrintingTools;
                                        BrotherPrinting.PrintLabelRM_RSSK(oData, printerUrl, printFormat, labelCount, isUsePrintingTools);
                                    }
                                }
                                else if (printFormat == Constant.PrintFormat.ZEBRA_LABELRM_RSSY)
                                {
                                    string ipAddress = HttpContext.Current.Request.UserHostAddress;
                                    filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted = 0", ipAddress, Constant.DirectPrintType.LABEL_RM);
                                    List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);
                                    if (lstPrinter.Count > 0)
                                    {
                                        printerUrl = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.LABEL_RM).FirstOrDefault().PrinterName;
                                    }
                                    ZebraPrinting.PrintMRLabel(oData, printerUrl, printFormat, labelCount > maxLabelNo ? maxLabelNo : labelCount);
                                }
                                else if (printFormat == Constant.PrintFormat.BIXOLON_LABELRM_RSPKSB)
                                {
                                    string ipAddress = HttpContext.Current.Request.UserHostAddress;
                                    filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted = 0", ipAddress, Constant.DirectPrintType.LABEL_RM);
                                    List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);
                                    if (lstPrinter.Count > 0)
                                    {
                                        printerUrl = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.LABEL_RM).FirstOrDefault().PrinterName;
                                    }
                                    ZebraPrinting.PrintMRLabelBixolon(oData, printerUrl, printFormat, labelCount > maxLabelNo ? maxLabelNo : labelCount);
                                }
                                else if (printFormat == Constant.PrintFormat.LABEL_RSP_RAJAL || printFormat == Constant.PrintFormat.LABEL_RSP_RANAP)
                                {
                                    if (oData.DepartmentID == "INPATIENT")
                                    {
                                        printFormat = Constant.PrintFormat.LABEL_RSP_RANAP;
                                    }
                                    else
                                    {
                                        printFormat = Constant.PrintFormat.LABEL_RSP_RAJAL;
                                    }
                                    ZebraPrinting.PrintMRLabel(oData, printerUrl, printFormat, labelCount > maxLabelNo ? maxLabelNo : labelCount);
                                }
                                else
                                {
                                    ZebraPrinting.PrintMRLabel(oData, printerUrl, printFormat, labelCount > maxLabelNo ? maxLabelNo : labelCount);
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return errorMsg;
        }
        #endregion

        #region PrintMRLabelRSDOSOBA
        private string PrintMRLabelRSDOSOBA(int id, string reportCode)
        {
            string errorMsg = string.Empty;

            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_FORMAT_CETAKAN_LABEL, Constant.SettingParameter.RM_FORMAT_CETAKAN_LABEL_2,
                    Constant.SettingParameter.RM_JUMLAH_CETAKAN_LABEL_RD, Constant.SettingParameter.RM_JUMLAH_CETAKAN_LABEL_RJ,
                    Constant.SettingParameter.RM_JUMLAH_CETAKAN_LABEL_RI, Constant.SettingParameter.RM_JUMLAH_CETAKAN_LABEL_MD,
                    Constant.SettingParameter.RM_ALAMAT_PRINTER_LABEL_RD, Constant.SettingParameter.RM_ALAMAT_PRINTER_LABEL_RJ,
                    Constant.SettingParameter.RM_ALAMAT_PRINTER_LABEL_RI, Constant.SettingParameter.RM_ALAMAT_PRINTER_LABEL_MD);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                if (lstParam != null)
                {
                    //Registration oRegistration = BusinessLayer.GetRegistration(id);
                    vPatient oPatient = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", id)).FirstOrDefault();
                    if (oPatient != null)
                    {
                        string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_CETAKAN_LABEL)).FirstOrDefault().ParameterValue;
                        if (reportCode == BusinessLayer.GetSettingParameterDt("001", Constant.SettingParameter.RM_REPORT_CODE_CUSTOM_LABEL).ParameterValue)
                        {
                            printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_CETAKAN_LABEL_2)).FirstOrDefault().ParameterValue;
                        }

                        string printerUrl = string.Empty;
                        printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_LABEL_RJ)).FirstOrDefault().ParameterValue;

                        //if (hdnIsMultiLocation.Value == "0")
                        //{
                        //    switch (oVisit.DepartmentID)
                        //    {
                        //        case Constant.Facility.EMERGENCY:
                        //            printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_LABEL_RD)).FirstOrDefault().ParameterValue;
                        //            break;
                        //        case Constant.Facility.OUTPATIENT:
                        //            printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_LABEL_RJ)).FirstOrDefault().ParameterValue;
                        //            break;
                        //        case Constant.Facility.INPATIENT:
                        //            printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_LABEL_RI)).FirstOrDefault().ParameterValue;
                        //            break;
                        //        default:
                        //            printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_LABEL_MD)).FirstOrDefault().ParameterValue;
                        //            break;
                        //    }
                        //}
                        //else
                        //{
                        //    //Get Printer Url from Location DropDown
                        //    StandardCode oStandardCode = BusinessLayer.GetStandardCode(cboPrinterUrl.Value.ToString());
                        //    if (oStandardCode != null)
                        //    {
                        //        if (!String.IsNullOrEmpty(oStandardCode.TagProperty))
                        //        {
                        //            string[] tagField = oStandardCode.TagProperty.Split('|');
                        //            printerUrl = tagField[0].TrimEnd();
                        //        }
                        //    }
                        //}
                        if (!string.IsNullOrEmpty(printerUrl))
                        {
                            int labelCount = Convert.ToInt16(txtJmlLabel.Text);
                            int maxLabelNo = Convert.ToInt16(hdnMaxLabelNo.Value);
                            ZebraPrinting.PrintMRLabel(oPatient, printerUrl, printFormat, labelCount > maxLabelNo ? maxLabelNo : labelCount);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return errorMsg;
        }
        #endregion

        #region PrintMRLabelRSRTH
        private string PrintMRLabelRSRTH(int id)
        {
            string errorMsg = string.Empty;

            try
            {

                string result = string.Empty;
                string ipAddress = HttpContext.Current.Request.UserHostAddress;

                StandardCode oStandardCode = BusinessLayer.GetStandardCode(cboPrinterUrl.Value.ToString());
                string printerUrl = string.Empty;
                if (oStandardCode != null)
                {
                    if (!String.IsNullOrEmpty(oStandardCode.TagProperty))
                    {
                        string[] tagField = oStandardCode.TagProperty.Split('|');
                        printerUrl = tagField[0].TrimEnd();
                    }
                }

                vLabelPatientRegistrationInfo oPatient = BusinessLayer.GetvLabelPatientRegistrationInfoList(string.Format("RegistrationID = {0}", id)).FirstOrDefault();

                if (!string.IsNullOrEmpty(printerUrl))
                {
                    int labelCount = Convert.ToInt16(txtJmlLabel.Text);
                    int maxLabelNo = Convert.ToInt16(hdnMaxLabelNo.Value);
                    ZebraPrinting.PrintMRLabelRSRTH(oPatient, printerUrl, labelCount > maxLabelNo ? maxLabelNo : labelCount);
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return errorMsg;
        }
        #endregion

        #region PrintLabelCoverHasilRadiologi
        private string PrintLabelCoverHasilRadiologi(int id)
        {
            string result = string.Empty;
            try
            {
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IS_FORMAT_CETAKAN_LABEL_COVER);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                if (lstParam != null)
                {
                    string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.IS_FORMAT_CETAKAN_LABEL_COVER)).FirstOrDefault().ParameterValue;

                    bool isBasedOnIPAddress = AppSession.IsPrinterLocationBasedOnIP;

                    if (isBasedOnIPAddress)
                    {
                        //Get Printer Address
                        string ipAddress = HttpContext.Current.Request.UserHostAddress;
                        filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType = '{1}' AND IsDeleted = 0", ipAddress, Constant.DirectPrintType.LABEL_COVER_RADIOLOGI);
                        PrinterLocation oPrinter = BusinessLayer.GetPrinterLocationList(filterExp).FirstOrDefault();
                        if (oPrinter != null)
                        {
                            string printerUrl = oPrinter.PrinterName;
                            int labelCount = Convert.ToInt16(txtJmlLabel.Text);
                            vPatientChargesHd5 oHeader = BusinessLayer.GetvPatientChargesHd5List(string.Format("TransactionID = {0}", id)).FirstOrDefault();

                            if (printFormat == Constant.PrintFormat.BROTHER_LABEL_COVER_HASIL_RADIOLOGI_RSSY)
                            {
                                BrotherPrinting.PrintLabelCoverHasilRadiologi_RSSY(oHeader, printerUrl, printFormat, labelCount);
                            }
                            else
                            {
                                ZebraPrinting.PrintCoverHasilRadiologi(oHeader, printerUrl, printFormat, labelCount);
                            }
                        }
                        else
                        {
                            result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                        }
                    }
                    else
                    {
                        result = "Printer format has not been set yet";
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }
        #endregion

        #region PrintLabelAmplopHasilLaboratorium
        private string PrintLabelAmplopHasilLaboratorium(int id)
        {
            string errorMsg = string.Empty;

            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.LB_FORMAT_LABEL_AMPLOP_LABORATORIUM);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.LB_FORMAT_LABEL_AMPLOP_LABORATORIUM)).FirstOrDefault().ParameterValue;
                string ipAddress = HttpContext.Current.Request.UserHostAddress;

                string filterPrint = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0", ipAddress, Constant.DirectPrintType.LABEL_AMPLOP_HASIL_LABORATORIUM);
                List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterPrint);
                string printerUrl = lstPrinter.FirstOrDefault().PrinterName;
                vLabelPatientRegistrationLaboratory charges = BusinessLayer.GetvLabelPatientRegistrationLaboratoryList(string.Format("TransactionID = {0}", id)).FirstOrDefault();

                if (lstParam != null)
                {
                    if (!string.IsNullOrEmpty(printerUrl))
                    {
                        int labelCount = Convert.ToInt16(txtJmlLabel.Text);
                        int maxLabelNo = Convert.ToInt16(hdnMaxLabelNo.Value);
                        ZebraPrinting.PrintLabelAmplopHasilLaboratorium(charges, printerUrl, printFormat, labelCount > maxLabelNo ? maxLabelNo : labelCount);
                    }
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            return errorMsg;
        }
        #endregion

        #region PrintLaboratorySampleLabel
        private string PrintLaboratorySampleLabel(int id)
        {
            string errorMsg = string.Empty;

            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.LB_FORMAT_CETAKAN_LABEL, Constant.SettingParameter.LB_ALAMAT_PRINTER_LABEL);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                if (lstParam != null)
                {
                    string filterExpression = string.Format("TransactionID = {0}", id);
                    vPatientChargesHdVisit oChargesHd = BusinessLayer.GetvPatientChargesHdVisitList(filterExpression).FirstOrDefault();
                    if (oChargesHd != null)
                    {
                        string printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.LB_ALAMAT_PRINTER_LABEL)).FirstOrDefault().ParameterValue;
                        string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.LB_FORMAT_CETAKAN_LABEL)).FirstOrDefault().ParameterValue;

                        if (!string.IsNullOrEmpty(printerUrl))
                        {
                            int labelCount = Convert.ToInt16(txtJmlLabel.Text);
                            int maxLabelNo = Convert.ToInt16(hdnMaxLabelNo.Value);
                            ZebraPrinting.PrintLaboratorySampleLabel(oChargesHd, printerUrl, printFormat, labelCount > maxLabelNo ? maxLabelNo : labelCount);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return errorMsg;
        }
        #endregion

        #region PrintLaboratorySampleLabelDt
        private string PrintLaboratorySampleLabelDt(int registrationID = 0, int transactionID = 0)
        {
            string errorMsg = string.Empty;

            try
            {
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')",
                        AppSession.UserLogin.HealthcareID, Constant.SettingParameter.LB_FORMAT_CETAKAN_LABEL);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.LB_FORMAT_CETAKAN_LABEL)).FirstOrDefault().ParameterValue;

                //Get Printer Url from Location DropDown
                StandardCode oStandardCode = BusinessLayer.GetStandardCode(cboPrinterUrl.Value.ToString());
                string filterExpression = string.Empty;
                if (oStandardCode != null)
                {
                    string[] tagField = oStandardCode.TagProperty.Split('|');
                    //Printer_Label| Printer_Wristband_Male;Printer_Wristband_Female;Printer_Wristband_Children_Male;Printer_Wristband_Children_Female|Printer_Wristband_Infant|Printer_RegistrationSlip
                    string printerUrl = tagField[0];
                    if (registrationID == 0)
                    {
                        filterExpression = string.Format("TransactionID = {0}", transactionID);
                    }
                    else
                    {
                        filterExpression = string.Format("RegistrationID = {0}", registrationID);
                    }
                    List<vLabelPatientRegistrationLaboratory> lstHd = BusinessLayer.GetvLabelPatientRegistrationLaboratoryList(filterExpression);
                    if (oStandardCode != null)
                    {
                        if (!String.IsNullOrEmpty(oStandardCode.TagProperty))
                        {
                            if (!string.IsNullOrEmpty(printerUrl))
                            {
                                int labelCount = Convert.ToInt16(txtJmlLabel.Text);
                                int maxLabelNo = Convert.ToInt16(hdnMaxLabelNo.Value);
                                foreach (vLabelPatientRegistrationLaboratory oChargesHd in lstHd)
                                {
                                    ZebraPrinting.PrintLaboratorySampleLabelDt(oChargesHd, printerUrl, printFormat, labelCount > maxLabelNo ? maxLabelNo : labelCount);
                                }
                            }
                        }
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return errorMsg;
        }

        private string PrintLaboratorySampleLabelDt(int id)
        {
            string errorMsg = string.Empty;

            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.LB_FORMAT_CETAKAN_LABEL, Constant.SettingParameter.LB_ALAMAT_PRINTER_LABEL);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                if (lstParam != null)
                {
                    string filterExpression = string.Format("TransactionID = {0}", id);
                    vLabelPatientRegistrationLaboratory oChargesHd = BusinessLayer.GetvLabelPatientRegistrationLaboratoryList(filterExpression).FirstOrDefault();
                    if (oChargesHd != null)
                    {
                        string printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.LB_ALAMAT_PRINTER_LABEL)).FirstOrDefault().ParameterValue;
                        string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.LB_FORMAT_CETAKAN_LABEL)).FirstOrDefault().ParameterValue;

                        if (!string.IsNullOrEmpty(printerUrl))
                        {
                            int labelCount = Convert.ToInt16(txtJmlLabel.Text);
                            int maxLabelNo = Convert.ToInt16(hdnMaxLabelNo.Value);
                            ZebraPrinting.PrintLaboratorySampleLabelDt(oChargesHd, printerUrl, printFormat, labelCount > maxLabelNo ? maxLabelNo : labelCount);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return errorMsg;
        }
        #endregion

        #region PrintPatientWristband
        private string PrintPatientWristband(int id)
        {
            string errorMsg = string.Empty;

            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_FORMAT_GELANG_PASIEN_DEWASA, Constant.SettingParameter.RM_FORMAT_GELANG_PASIEN_ANAK, Constant.SettingParameter.RM_FORMAT_GELANG_PASIEN_BAYI,
                    Constant.SettingParameter.RM_GELANG_PASIEN_ANAK_KHUSUS, Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_LAKI,
                    Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_PEREMPUAN, Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_ANAK,
                    Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_BAYI);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                if (lstParam != null)
                {
                    Registration oRegistration = BusinessLayer.GetRegistration(id);
                    if (oRegistration != null)
                    {
                        vConsultVisit oVisit = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", oRegistration.RegistrationID)).FirstOrDefault();
                        if (oVisit != null)
                        {

                            string printFormat = string.Empty;
                            string printerUrl = string.Empty;

                            if (hdnIsMultiLocation.Value == "0")
                            {
                                printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_GELANG_PASIEN_DEWASA)).FirstOrDefault().ParameterValue;
                                //Choose Printer Url based on Patient Gender
                                switch (oVisit.GCGender)
                                {
                                    case Constant.Gender.MALE:
                                        printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_LAKI)).FirstOrDefault().ParameterValue;
                                        break;
                                    case Constant.Gender.FEMALE:
                                        printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_PEREMPUAN)).FirstOrDefault().ParameterValue;
                                        break;
                                    default:
                                        printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_LAKI)).FirstOrDefault().ParameterValue;
                                        break;
                                }
                            }
                            else
                            {
                                //Get Printer Url from Location DropDown
                                StandardCode oStandardCode = BusinessLayer.GetStandardCode(cboPrinterUrl.Value.ToString());
                                if (oStandardCode != null)
                                {
                                    if (!String.IsNullOrEmpty(oStandardCode.TagProperty))
                                    {
                                        string[] tagField = oStandardCode.TagProperty.Split('|');
                                        //Printer_Label| Printer_Wristband_Male;Printer_Wristband_Female;Printer_Wristband_Children_Male;Printer_Wristband_Children_Female|Printer_Wristband_Infant|Printer_RegistrationSlip
                                        string[] wristbandUrl = tagField[1].Split(';');
                                        printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_GELANG_PASIEN_DEWASA)).FirstOrDefault().ParameterValue;
                                        //Choose Printer Url based on Patient Gender
                                        switch (oVisit.GCGender)
                                        {
                                            case Constant.Gender.MALE:
                                                printerUrl = wristbandUrl[0];
                                                break;
                                            case Constant.Gender.FEMALE:
                                                printerUrl = wristbandUrl[1];
                                                break;
                                            default:
                                                printerUrl = wristbandUrl[0];
                                                break;
                                        }
                                    }
                                }

                            }
                            if (!string.IsNullOrEmpty(printerUrl))
                            {
                                if (printFormat == Constant.PrintFormat.BIXOLON_WRISTBAND_RSPKSB)
                                {
                                    int labelCount = Convert.ToInt16(txtJmlLabel.Text);
                                    ZebraPrinting.PrintWristbandBixolon(oVisit, printerUrl, printFormat);
                                }
                                else
                                {
                                    int labelCount = Convert.ToInt16(txtJmlLabel.Text);
                                    ZebraPrinting.PrintWristband(oVisit, printerUrl, printFormat);
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return errorMsg;
        }
        #endregion

        #region PrintOutPatientWristband
        private string PrintOutPatientWristband(int id)
        {
            string result = string.Empty;

            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')",
                    AppSession.UserLogin.HealthcareID,
                    Constant.SettingParameter.RM_FORMAT_GELANG_PASIEN_RAWAT_JALAN);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);

                bool isBasedOnIPAddress = AppSession.IsPrinterLocationBasedOnIP;

                if (isBasedOnIPAddress)
                {
                    //Get Printer Address
                    string ipAddress = HttpContext.Current.Request.UserHostAddress;
                    filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}','{2}') AND IsDeleted=0",
                        ipAddress,
                        Constant.DirectPrintType.GELANG_PASIEN_RJ_L,
                        Constant.DirectPrintType.GELANG_PASIEN_RJ_P);

                    List<PrinterLocation> oPrinterList = BusinessLayer.GetPrinterLocationList(filterExp);
                    if (oPrinterList.Count > 0)
                    {
                        Registration oRegistration = BusinessLayer.GetRegistration(id);
                        if (oRegistration != null)
                        {
                            vConsultVisit oVisit = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", oRegistration.RegistrationID)).FirstOrDefault();
                            if (oVisit != null)
                            {

                                string printFormat = string.Empty;
                                string printerUrl = string.Empty;

                                printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_GELANG_PASIEN_RAWAT_JALAN)).FirstOrDefault().ParameterValue;

                                //Choose Printer Url based on Patient Gender
                                switch (oVisit.GCGender)
                                {
                                    case Constant.Gender.MALE:
                                        printerUrl = oPrinterList.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.GELANG_PASIEN_RJ_L).FirstOrDefault().PrinterName;
                                        break;
                                    case Constant.Gender.FEMALE:
                                        printerUrl = oPrinterList.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.GELANG_PASIEN_RJ_P).FirstOrDefault().PrinterName;
                                        break;
                                    default:
                                        break;
                                }

                                if (!string.IsNullOrEmpty(printerUrl))
                                {
                                    ZebraPrinting.PrintWristband(oVisit, printerUrl, printFormat);
                                }
                            }
                        }
                    }
                    else
                    {
                        result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                    }
                }
                else
                {
                    result = "This feature is not available yet";
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }
        #endregion

        #region PrintProductionLabel
        private string PrintProductionLabel(int id)
        {
            string result = string.Empty;
            try
            {
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IM_FORMAT_CETAKAN_LABEL_PRODUKSI);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                if (lstParam != null)
                {
                    string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.IM_FORMAT_CETAKAN_LABEL_PRODUKSI)).FirstOrDefault().ParameterValue;

                    bool isBasedOnIPAddress = AppSession.IsPrinterLocationBasedOnIP;

                    if (isBasedOnIPAddress)
                    {
                        //Get Printer Address
                        string ipAddress = HttpContext.Current.Request.UserHostAddress;
                        filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}','{2}') AND IsDeleted=0",
                            ipAddress, Constant.DirectPrintType.LABEL_BARANG_PRODUKSI, Constant.DirectPrintType.LABEL_BARANG_PRODUKSI_LUAR);

                        List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);
                        if (lstPrinter.Count > 0)
                        {
                            string printerUrl1 = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.LABEL_BARANG_PRODUKSI).FirstOrDefault().PrinterName;
                            string printerUrl2 = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.LABEL_BARANG_PRODUKSI_LUAR).FirstOrDefault().PrinterName;
                            int labelCount = Convert.ToInt16(txtJmlLabel.Text);
                            vItemProductionHd oHeader = BusinessLayer.GetvItemProductionHdList(string.Format("ProductionID = {0}", id)).FirstOrDefault();
                            ZebraPrinting.PrintProductionLabel(oHeader, printerUrl1, printerUrl2, printFormat, labelCount);
                        }
                        else
                        {
                            result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                        }
                    }
                    else
                    {
                        result = "Printer format has not been set yet";
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }
        #endregion

        #region PrintPatientChildWristband
        private string PrintPatientChildWristband(int id)
        {
            string errorMsg = string.Empty;

            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_FORMAT_GELANG_PASIEN_DEWASA, Constant.SettingParameter.RM_FORMAT_GELANG_PASIEN_ANAK, Constant.SettingParameter.RM_FORMAT_GELANG_PASIEN_BAYI,
                    Constant.SettingParameter.RM_GELANG_PASIEN_ANAK_KHUSUS, Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_LAKI,
                    Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_PEREMPUAN, Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_ANAK,
                    Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_BAYI);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                if (lstParam != null)
                {
                    Registration oRegistration = BusinessLayer.GetRegistration(id);
                    if (oRegistration != null)
                    {
                        vConsultVisit oVisit = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", oRegistration.RegistrationID)).FirstOrDefault();
                        if (oVisit != null)
                        {

                            string printFormat = string.Empty;
                            string printerUrl = string.Empty;

                            if (hdnIsMultiLocation.Value == "0")
                            {
                                //if new born
                                if (oVisit.IsNewBorn)
                                {
                                    printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_BAYI)).FirstOrDefault().ParameterValue;
                                    printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_GELANG_PASIEN_BAYI)).FirstOrDefault().ParameterValue;
                                }
                                else
                                {
                                    bool isUsingChildrenWristband = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_GELANG_PASIEN_ANAK_KHUSUS)).FirstOrDefault().ParameterValue == "1" ? true : false;
                                    if (!isUsingChildrenWristband)
                                    {
                                        printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_GELANG_PASIEN_DEWASA)).FirstOrDefault().ParameterValue;
                                        //Choose Printer Url based on Patient Gender
                                        switch (oVisit.GCGender)
                                        {
                                            case Constant.Gender.MALE:
                                                printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_LAKI)).FirstOrDefault().ParameterValue;
                                                break;
                                            case Constant.Gender.FEMALE:
                                                printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_PEREMPUAN)).FirstOrDefault().ParameterValue;
                                                break;
                                            default:
                                                printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_LAKI)).FirstOrDefault().ParameterValue;
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_GELANG_PASIEN_ANAK)).FirstOrDefault().ParameterValue;
                                        printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_GELANG_PASIEN_ANAK)).FirstOrDefault().ParameterValue;
                                    }
                                }
                            }
                            else
                            {
                                //Get Printer Url from Location DropDown
                                StandardCode oStandardCode = BusinessLayer.GetStandardCode(cboPrinterUrl.Value.ToString());
                                if (oStandardCode != null)
                                {
                                    if (!String.IsNullOrEmpty(oStandardCode.TagProperty))
                                    {
                                        string[] tagField = oStandardCode.TagProperty.Split('|');
                                        //Printer_Label| Printer_Wristband_Male;Printer_Wristband_Female;Printer_Wristband_Children_Male;Printer_Wristband_Children_Female|Printer_Wristband_Infant|Printer_RegistrationSlip
                                        if (oVisit.IsNewBorn)
                                        {
                                            string[] wristbandUrl = tagField[2].Split(';');
                                            if (wristbandUrl[0] != "")
                                            {
                                                printerUrl = wristbandUrl[0];
                                                printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_GELANG_PASIEN_BAYI)).FirstOrDefault().ParameterValue;
                                            }
                                            else
                                            {
                                                string[] wristbandUrl1 = tagField[1].Split(';');
                                                switch (oVisit.GCGender)
                                                {
                                                    case Constant.Gender.MALE:
                                                        printerUrl = wristbandUrl1[2];
                                                        break;
                                                    case Constant.Gender.FEMALE:
                                                        printerUrl = wristbandUrl1[3];
                                                        break;
                                                    default:
                                                        printerUrl = wristbandUrl1[2];
                                                        break;
                                                }
                                                printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_GELANG_PASIEN_ANAK)).FirstOrDefault().ParameterValue;
                                            }
                                        }
                                        else
                                        {
                                            string[] wristbandUrl = tagField[1].Split(';');
                                            switch (oVisit.GCGender)
                                            {
                                                case Constant.Gender.MALE:
                                                    printerUrl = wristbandUrl[2];
                                                    break;
                                                case Constant.Gender.FEMALE:
                                                    printerUrl = wristbandUrl[3];
                                                    break;
                                                default:
                                                    printerUrl = wristbandUrl[2];
                                                    break;
                                            }
                                            printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_FORMAT_GELANG_PASIEN_ANAK)).FirstOrDefault().ParameterValue;
                                        }
                                    }
                                }

                            }
                            if (!string.IsNullOrEmpty(printerUrl))
                            {
                                if (printFormat == Constant.PrintFormat.BIXOLON_WRISTBAND_CHILD_RSPKSB)
                                {
                                    int labelCount = Convert.ToInt16(txtJmlLabel.Text);
                                    ZebraPrinting.PrintWristbandBixolon(oVisit, printerUrl, printFormat);
                                }
                                else
                                {
                                    int labelCount = Convert.ToInt16(txtJmlLabel.Text);
                                    ZebraPrinting.PrintWristband(oVisit, printerUrl, printFormat);
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return errorMsg;
        }
        #endregion

        #region PrintLabelRekamMedisRSSMC
        private string PrintLabelRekamMedisRSSMC(int id)
        {
            string result = string.Empty;
            string ipAddress = HttpContext.Current.Request.UserHostAddress;
            string filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0", ipAddress, Constant.DirectPrintType.LABEL_RM);
            List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);
            string printerUrl1 = lstPrinter.FirstOrDefault().PrinterName;
            if (!string.IsNullOrEmpty(printerUrl1))
            {
                int labelCount = Convert.ToInt16(txtJmlLabel.Text);
                int maxLabelNo = Convert.ToInt16(hdnMaxLabelNo.Value);

                vPatient patient = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", id)).FirstOrDefault();
                ZebraPrinting.PrintLabelRekamMedisRSSMC(patient, printerUrl1, labelCount > maxLabelNo ? maxLabelNo : labelCount);
            }
            return result;
        }
        #endregion

        #region PrintLabelPendaftaranRSDO
        private string PrintLabelPendaftaranRSDO(int id)
        {
            string result = string.Empty;
            string ipAddress = HttpContext.Current.Request.UserHostAddress;
            int labelCount = Convert.ToInt16(txtJmlLabel.Text);
            string filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0", ipAddress, Constant.DirectPrintType.LABEL_RM);
            List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);
            string printerUrl1 = lstPrinter.FirstOrDefault().PrinterName;

            vPatient patient = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", id)).FirstOrDefault();
            ZebraPrinting.PrintLabelPasienRSDO(patient, printerUrl1, labelCount);
            return result;
        }
        #endregion

        #region PrintLabelRadiologiRSDO
        private string PrintLabelRadiologiRSDO(int id)
        {
            string result = string.Empty;
            string ipAddress = HttpContext.Current.Request.UserHostAddress;
            string filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0", ipAddress, Constant.DirectPrintType.LABEL_RADIOLOGI);
            List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);
            string printerUrl1 = lstPrinter.FirstOrDefault().PrinterName;

            vLabelPatientRegistrationImagingRSDOSKA charges = BusinessLayer.GetvLabelPatientRegistrationImagingRSDOSKAList(string.Format("TransactionID = {0}", id)).FirstOrDefault();
            ZebraPrinting.printLabelRadiologiRSDO(charges, printerUrl1);
            return result;
        }
        #endregion

        #region PrintLabelRadiologiRSDOSOBA
        private string PrintLabelRadiologiRSDOSOBA(int id)
        {
            string result = string.Empty;
            string ipAddress = HttpContext.Current.Request.UserHostAddress;
            string filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0", ipAddress, Constant.DirectPrintType.LABEL_RADIOLOGI_2);
            List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);
            string printerUrl1 = lstPrinter.FirstOrDefault().PrinterName;

            vLabelPatientRegistrationImagingRSDOSOBA charges = BusinessLayer.GetvLabelPatientRegistrationImagingRSDOSOBAList(string.Format("TransactionID = {0}", id)).FirstOrDefault();
            ZebraPrinting.printLabelRadiologiRSDOSOBA(charges, printerUrl1);
            return result;
        }
        #endregion

        #region PrintLabelRadiologiRSUKRIDA
        private string PrintLabelRadiologiRSUKRIDA(int id)
        {
            string result = string.Empty;
            string ipAddress = HttpContext.Current.Request.UserHostAddress;
            string filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0", ipAddress, Constant.DirectPrintType.LABEL_RADIOLOGI_2);
            List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);
            vLabelPatientRegistrationImagingRSUKRIDA charges = BusinessLayer.GetvLabelPatientRegistrationImagingRSUKRIDAList(string.Format("TransactionID = {0}", id)).FirstOrDefault();
            string printerUrl1 = lstPrinter.FirstOrDefault().PrinterName;
            Healthcare entityHSU = BusinessLayer.GetHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID)).FirstOrDefault();

            if (lstPrinter.Count > 0)
            {
                if (entityHSU.Initial == "RSUKRIDA")
                {
                    ZebraPrinting.printLabelRadiologiRSUKRIDA(charges, printerUrl1);
                    return result;
                }
                else if (entityHSU.Initial == "granostic")
                {
                    ZebraPrinting.printLabelRadiologiGRANOSTIC(charges, printerUrl1);
                    return result;
                }
            }
            else
            {
                return result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
            }
            return result;
        }
        #endregion

        #region PrintLabelRadiologiRSRA
        private string PrintLabelRadiologiRSRA(int id)
        {
            string result = string.Empty;
            string ipAddress = HttpContext.Current.Request.UserHostAddress;
            string filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0", ipAddress, Constant.DirectPrintType.LABEL_RADIOLOGI_2);
            List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);
            string printerUrl1 = lstPrinter.FirstOrDefault().PrinterName;

            Healthcare entityHSU = BusinessLayer.GetHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID)).FirstOrDefault();

            if (entityHSU.Initial == "RSRA")
            {
                vLabelPatientRegistrationImagingRSRA charges = BusinessLayer.GetvLabelPatientRegistrationImagingRSRAList(string.Format("TransactionID = {0}", id)).FirstOrDefault();
                ZebraPrinting.printLabelRadiologiRSRA(charges, printerUrl1);
                return result;
            }
            if (entityHSU.Initial == "RSRT")
            {
                vLabelPatientRegistrationImagingRSRT charges = BusinessLayer.GetvLabelPatientRegistrationImagingRSRTList(string.Format("TransactionID = {0}", id)).FirstOrDefault();
                ZebraPrinting.printLabelRadiologiRSRT(charges, printerUrl1);
                return result;
            }
            else
            {
                vLabelPatientRegistrationImagingRSSM charges = BusinessLayer.GetvLabelPatientRegistrationImagingRSSMList(string.Format("TransactionID = {0}", id)).FirstOrDefault();
                ZebraPrinting.printLabelRadiologiRSSM(charges, printerUrl1);
                return result;
            }
        }
        #endregion

        #region PringLabelRadiologiKKDI

        #region Label Ind
        private string PrintLabelRadiologiKKDI(int id, string lang = "id")
        {
            string result = string.Empty;
            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
                    AppSession.UserLogin.HealthcareID,
                    Constant.SettingParameter.IS_FORMAT_CETAKAN_LABEL_COVER,
                    Constant.SettingParameter.IS_FORMAT_CETAKAN_LABEL_COVER_ENG, 
                    Constant.SettingParameter.IS_JENIS_PRINTER_CETAKAN_RADIOLOGI);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                if (lstParam != null)
                {
                    string printerType = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.IS_JENIS_PRINTER_CETAKAN_RADIOLOGI)).FirstOrDefault().ParameterValue;
                    string printFormatInd = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.IS_FORMAT_CETAKAN_LABEL_COVER)).FirstOrDefault().ParameterValue;
                    string printFormatEng = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.IS_FORMAT_CETAKAN_LABEL_COVER_ENG)).FirstOrDefault().ParameterValue;

                    string ipAddress = HttpContext.Current.Request.UserHostAddress;

                    filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}', '{2}') AND IsDeleted = 0",
                        ipAddress, Constant.DirectPrintType.LABEL_REGISTRASI_RADIOLOGI, Constant.DirectPrintType.LABEL_RADIOLOGI);

                    List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);
                    Healthcare entityHSU = BusinessLayer.GetHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID)).FirstOrDefault();
                    List<vLabelPatientRegistrationImagingKKDI> lstLabel = BusinessLayer.GetvLabelPatientRegistrationImagingKKDIList(string.Format("TransactionID = {0}", id));
                    if (lstLabel.Count() > 0)
                    {
                        switch (printerType)
                        {
                            case Constant.PrinterType.ZEBRA_PRINTER:
                                //Get Printer Address
                                if (lstPrinter.Count > 0)
                                {
                                    foreach (vLabelPatientRegistrationImagingKKDI oChargesHd in lstLabel)
                                    {
                                        string printerUrl1 = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.LABEL_RADIOLOGI).FirstOrDefault().PrinterName;
                                        ZebraPrinting.PrintLabelPasienRadiologiKKDI(oChargesHd, lang == "id" ? printFormatInd : printFormatEng, id, printerUrl1);
                                    }
                                }
                                else
                                {
                                    result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                }
                                break;
                            default:
                                    result = string.Format("Hanya Tersedia Format Untuk Printer Zebra");
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }
        #endregion

        #region Label Eng
        private string PrintLabelRadiologiKKDIEng(int id)
        {
            string result = string.Empty;
            try
            {
                //Get Printing Configuration
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IS_FORMAT_CETAKAN_LABEL_COVER, Constant.SettingParameter.IS_JENIS_PRINTER_CETAKAN_RADIOLOGI);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                if (lstParam != null)
                {
                    string printerType = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.IS_JENIS_PRINTER_CETAKAN_RADIOLOGI)).FirstOrDefault().ParameterValue;
                    string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.IS_FORMAT_CETAKAN_LABEL_COVER)).FirstOrDefault().ParameterValue;
                    //string printerUrl = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.RM_ALAMAT_PRINTER_BUKTI_PENDAFTARAN)).FirstOrDefault().ParameterValue;

                    string ipAddress = HttpContext.Current.Request.UserHostAddress;

                    filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}', '{2}') AND IsDeleted=0",
                                    ipAddress, Constant.DirectPrintType.LABEL_REGISTRASI_RADIOLOGI, Constant.DirectPrintType.LABEL_RADIOLOGI);

                    List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);

                    Healthcare entityHSU = BusinessLayer.GetHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID)).FirstOrDefault();

                    vLabelPatientRegistrationImaging oLabel = BusinessLayer.GetvLabelPatientRegistrationImagingList(string.Format("TransactionID = {0}", id)).FirstOrDefault();
                    if (entityHSU.Initial == "DEMO")
                    {
                        if (oLabel != null)
                        {
                            switch (printerType)
                            {
                                case Constant.PrinterType.ZEBRA_PRINTER:
                                    //Get Printer Address
                                    if (lstPrinter.Count > 0)
                                    {
                                        string printerUrl1 = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.LABEL_RADIOLOGI).FirstOrDefault().PrinterName;
                                        ZebraPrinting.PrintLabelPasienRadiologiRSCK_ZPL(printFormat, id, printerUrl1);
                                    }
                                    else
                                    {
                                        result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                    }
                                    break;
                                default:
                                    if (lstPrinter.Count > 0)
                                        ZebraPrinting.PrintLabelPasienRadiologiRSCK(oLabel, lstPrinter[0].PrinterName);
                                    else
                                        result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                    break;
                            }
                        }
                    }
                    else
                    {
                        if (oLabel != null)
                        {
                            switch (printerType)
                            {
                                case Constant.PrinterType.EPSON_DOT_MATRIX:
                                    //Get Printer Address

                                    if (lstPrinter.Count > 0)
                                        ZebraPrinting.PrintLabelPasienRadiologi(oLabel, lstPrinter[0].PrinterName);
                                    else
                                        result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                    break;
                                default:
                                    ZebraPrinting.PrintLabelPasienRadiologi(oLabel, lstPrinter[0].PrinterName);
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }
        #endregion

        #endregion

        #region printLabelPasienRekamMedis2
        private string printLabelPasienRekamMedis2(int id)
        {
            string result = string.Empty;
            string ipAddress = HttpContext.Current.Request.UserHostAddress;

            StandardCode oStandardCode = BusinessLayer.GetStandardCode(cboPrinterUrl.Value.ToString());
            string printerUrl = string.Empty;
            if (oStandardCode != null)
            {
                if (!String.IsNullOrEmpty(oStandardCode.TagProperty))
                {
                    string[] tagField = oStandardCode.TagProperty.Split('|');
                    printerUrl = tagField[0].TrimEnd();
                }
            }

            vPatient patient = BusinessLayer.GetvPatientList(string.Format("MRN = {0} AND IsDeleted = 0", id)).FirstOrDefault();

            if (!string.IsNullOrEmpty(printerUrl))
            {
                int labelCount = Convert.ToInt16(txtJmlLabel.Text);
                int maxLabelNo = Convert.ToInt16(hdnMaxLabelNo.Value);
                ZebraPrinting.printLabelPasienRekamMedis2(patient, printerUrl, labelCount > maxLabelNo ? maxLabelNo : labelCount);
            }

            return result;
        }
        #endregion

        #region PrintLabelObat
        private string PrintLabelObat(int id)
        {
            string result = string.Empty;
            string ipAddress = HttpContext.Current.Request.UserHostAddress;
            string filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0", ipAddress, Constant.DirectPrintType.LABEL_OBAT);
            List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);
            string printerUrl1 = lstPrinter.FirstOrDefault().PrinterName;

            vPrescriptionOrderDt1 prescription = BusinessLayer.GetvPrescriptionOrderDt1List(string.Format("PrescriptionOrderID = {0} AND IsRFlag = 1", id)).FirstOrDefault();
            ZebraPrinting.PrintLabelObat(prescription, printerUrl1);
            return result;
        }

        private string PrintLabelObatRSSES(int id)
        {
            string result = string.Empty;
            string ipAddress = HttpContext.Current.Request.UserHostAddress;
            string filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0", ipAddress, Constant.DirectPrintType.LABEL_OBAT);
            List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);
            string printerUrl1 = lstPrinter.FirstOrDefault().PrinterName;

            vPrescriptionOrderDtLabelRSSES prescription = BusinessLayer.GetvPrescriptionOrderDtLabelRSSESList(string.Format("PrescriptionOrderID = {0} AND IsRFlag = 1", id)).FirstOrDefault();
            ZebraPrinting.PrintLabelObatRSSES(prescription, printerUrl1);
            return result;
        }
        #endregion

        #region PrintLabelMasterItem
        private string PrintLabelMasterItem(int id)
        {
            string result = string.Empty;
            string ipAddress = HttpContext.Current.Request.UserHostAddress;
            int labelCount = Convert.ToInt16(txtJmlLabel.Text);
            string filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted=0", ipAddress, Constant.DirectPrintType.LABEL_MASTER_ITEM);
            List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);
            string printerUrl1 = lstPrinter.FirstOrDefault().PrinterName;

            vItemProduct item = BusinessLayer.GetvItemProductList(string.Format("ItemID = {0}", id)).FirstOrDefault();
            ZebraPrinting.PrintLabelMasterItem(item, printerUrl1, labelCount);
            return result;
        }
        #endregion

        #region PrintNutritionPatientRegistration
        private string PrintNutritionPatientRegistration(int id)
        {
            string errorMsg = string.Empty;

            try
            {
                //Get Printing Configuration
                string filterExpression = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')",
                    AppSession.UserLogin.HealthcareID, Constant.SettingParameter.NT_FORMAT_CETAKAN_ETIKET_GIZI);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExpression);
                if (lstParam != null)
                {
                    string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.NT_FORMAT_CETAKAN_ETIKET_GIZI)).FirstOrDefault().ParameterValue;
                    Registration oRegistration = BusinessLayer.GetRegistration(id);
                    if (oRegistration != null)
                    {
                        vLabelPatientRegistrationInfo oVisit = BusinessLayer.GetvLabelPatientRegistrationInfoList(string.Format("RegistrationID = {0}", oRegistration.RegistrationID)).FirstOrDefault();
                        if (oVisit != null)
                        {
                            string ipAddress = HttpContext.Current.Request.UserHostAddress;
                            string filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted = 0", ipAddress, Constant.DirectPrintType.LABEL_DISTRIBUSI_GIZI);
                            List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);
                            string printerUrl = lstPrinter.FirstOrDefault().PrinterName;

                            if (!string.IsNullOrEmpty(printerUrl))
                            {
                                ZebraPrinting.PrintNutritionPatientRegistration(oVisit, printerUrl, printFormat);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return errorMsg;
        }
        #endregion

        #region PrintLabelAsset

        private string PrintLabelAsset(int id, string menuCode)
        {
            string result = string.Empty;
            try
            {
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')",
                AppSession.UserLogin.HealthcareID, Constant.SettingParameter.AC_FORMAT_CETAKAN_LABEL_ASSET, Constant.SettingParameter.AC_JENIS_PRINTER_LABEL_ASSET);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.AC_FORMAT_CETAKAN_LABEL_ASSET)).FirstOrDefault().ParameterValue;
                string printerType = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.AC_JENIS_PRINTER_LABEL_ASSET)).FirstOrDefault().ParameterValue;
                bool isBasedOnIPAddress = AppSession.IsPrinterLocationBasedOnIP;

                if (printFormat == Constant.PrintFormat.LABLE_ASSET_RSMD)
                {
                    result = ZebraPrinting.PrintLabelAssetRSMD(printFormat, id);
                }
                else
                {
                    if (isBasedOnIPAddress)
                    {
                        //Get Printer Address
                        string ipAddress = HttpContext.Current.Request.UserHostAddress;
                        filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted = 0", ipAddress, Constant.DirectPrintType.LABEL_ASSET);
                        List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);
                        string printerUrl = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.LABEL_ASSET).FirstOrDefault().PrinterName;

                        vFAItem item = BusinessLayer.GetvFAItemList(string.Format("FixedAssetID = {0}", id)).FirstOrDefault();

                        //Check Printer Type
                        switch (printerType)
                        {
                            case Constant.PrinterType.ZEBRA_PRINTER:
                                if (lstPrinter.Count > 0)
                                {
                                    ZebraPrinting.PrintLabelAsset(printFormat, id, printerUrl);
                                }
                                else
                                {
                                    result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                }
                                break;
                            case Constant.PrinterType.EPSON_DOT_MATRIX_RSSB:
                                if (lstPrinter.Count > 0)
                                    ZebraPrinting.PrintLabelAssetRSSB(item, printerUrl);
                                else
                                    result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                break;
                            case Constant.PrinterType.BROTHER_PRINTER:
                                if (lstPrinter.Count > 0)
                                {
                                    if (printFormat == Constant.PrintFormat.BROTHER_LABEL_ASSET_RSSY)
                                    {
                                        BrotherPrinting.PrintLabelAsset_RSSY(printFormat, id, printerUrl);
                                    }
                                }
                                else
                                {
                                    result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                }
                                break;
                        }
                    }
                    else
                    {
                        result = string.Format("Printer Configuration is not available");
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }

        private string PrintLabelAssetPerBA(int id, string menuCode)
        {
            string result = string.Empty;
            try
            {
                string filterExp = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')",
                AppSession.UserLogin.HealthcareID, Constant.SettingParameter.AC_FORMAT_CETAKAN_LABEL_ASSET, Constant.SettingParameter.AC_JENIS_PRINTER_LABEL_ASSET);

                List<vSettingParameterDt> lstParam = BusinessLayer.GetvSettingParameterDtList(filterExp);
                string printFormat = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.AC_FORMAT_CETAKAN_LABEL_ASSET)).FirstOrDefault().ParameterValue;
                string printerType = lstParam.Where(lst => lst.ParameterCode.Equals(Constant.SettingParameter.AC_JENIS_PRINTER_LABEL_ASSET)).FirstOrDefault().ParameterValue;
                bool isBasedOnIPAddress = AppSession.IsPrinterLocationBasedOnIP;

                if (isBasedOnIPAddress)
                {
                    //Get Printer Address
                    string ipAddress = HttpContext.Current.Request.UserHostAddress;
                    filterExp = string.Format("IPAddress = '{0}' AND GCPrinterType IN ('{1}') AND IsDeleted = 0", ipAddress, Constant.DirectPrintType.LABEL_ASSET);

                    string filterBA = string.Format("IsDeleted = 0 AND FAAcceptanceID = {0}", id);
                    List<FAAcceptanceDt> baList = BusinessLayer.GetFAAcceptanceDtList(filterBA);
                    foreach (FAAcceptanceDt badt in baList)
                    {
                        vFAItem item = BusinessLayer.GetvFAItemList(string.Format("FixedAssetID = {0}", badt.FixedAssetID)).FirstOrDefault();
                        List<PrinterLocation> lstPrinter = BusinessLayer.GetPrinterLocationList(filterExp);
                        string printerUrl = lstPrinter.Where(lst => lst.GCPrinterType == Constant.DirectPrintType.LABEL_ASSET).FirstOrDefault().PrinterName;

                        //Check Printer Type
                        switch (printerType)
                        {
                            case Constant.PrinterType.EPSON_DOT_MATRIX_RSSB:
                                if (lstPrinter.Count > 0)
                                    ZebraPrinting.PrintLabelAssetRSSB(item, printerUrl);
                                else
                                    result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                break;
                            case Constant.PrinterType.BROTHER_PRINTER:
                                if (lstPrinter.Count > 0)
                                {
                                    if (printFormat == Constant.PrintFormat.BROTHER_LABEL_ASSET_RSSY)
                                    {
                                        BrotherPrinting.PrintLabelAsset_RSSY(printFormat, badt.FixedAssetID, printerUrl);
                                    }
                                }
                                else
                                {
                                    result = string.Format("Printer Configuration is not available for IP Address {0}", ipAddress);
                                }
                                break;
                        }
                    }
                }
                else
                {
                    result = string.Format("Printer Configuration is not available");
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }

        #endregion

        #region PrintStikerRawatJalanZebra
        private string PrintStickerRawatJalanZebra(int id)
        {
            string result = string.Empty;
            try
            {
                result = ZebraPrinting.PrintStikerRJZebra("X210^54", id);
            }
            catch (Exception ex)
            {
                result = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }
        #endregion

        #region Search Dialog State
        private class CSearchDialogState
        {
            public string MethodName { get; set; }
            public string KeyFieldName { get; set; }
            public string BaseFilterExpression { get; set; }
            public bool IsTreeView { get; set; }
            public string OrderByExpression { get; set; }
            public string FilterExpression { get; set; }
            public List<GridColumn> GridColumns { get; set; }
            public List<QuickSearchIntellisense> IntellisenseTexts { get; set; }
        }

        private const string SESSION_SEARCH_DIALOG_STATE = "SearchDialogState";
        private static CSearchDialogState SearchDialogState
        {
            get
            {
                if (HttpContext.Current.Session[SESSION_SEARCH_DIALOG_STATE] == null) HttpContext.Current.Session[SESSION_SEARCH_DIALOG_STATE] = new CSearchDialogState();
                return (CSearchDialogState)HttpContext.Current.Session[SESSION_SEARCH_DIALOG_STATE];
            }
            set
            {
                HttpContext.Current.Session[SESSION_SEARCH_DIALOG_STATE] = value;
            }
        }

        private class GridColumn
        {
            private String _HorizontalAlign;
            public String HorizontalAlign
            {
                get { return _HorizontalAlign; }
                set { _HorizontalAlign = value; }
            }

            private String _DisplayCustomField;
            public String DisplayCustomField
            {
                get { return _DisplayCustomField; }
                set { _DisplayCustomField = value; }
            }

            private String _DataField;
            public String DataField
            {
                get { return _DataField; }
                set { _DataField = value; }
            }

            private String _HeaderText;
            public String HeaderText
            {
                get { return _HeaderText; }
                set { _HeaderText = value; }
            }

            private String _Width;
            public String Width
            {
                get { return _Width; }
                set { _Width = value; }
            }
        }

        private class QuickSearchIntellisense
        {
            private String _DataField;
            public String DataField
            {
                get { return _DataField; }
                set { _DataField = value; }
            }

            private String _HeaderText;
            public String HeaderText
            {
                get { return _HeaderText; }
                set { _HeaderText = value; }
            }

            private String _Description;
            public String Description
            {
                get { return _Description; }
                set { _Description = value; }
            }
        }
        #endregion

        #region LoadRightPanelContent
        private void LoadRightPanelContent()
        {
            try
            {
                string IsLoadContent = Request.Form["hdnRightPanelContentIsLoadContent"] ?? "0";
                if (IsLoadContent == "1")
                {
                    string url = Request.Form["hdnRightPanelContentUrl"] ?? "";
                    Control ctlParent = pnlRightPanelContentArea;
                    BaseContentPopupCtl ctl = (BaseContentPopupCtl)LoadControl(url);
                    ctl.PopupTitle = Request.Form["hdnPopupControlTitle"] ?? "";
                    ctl.TagProperty = Request.Form["hdnRightPanelContentCode"] ?? "";

                    ctlParent.Controls.Clear();
                    ctlParent.Controls.Add(ctl);

                    string firstTimeLoad = Request.Form["hdnRightPanelContentFirstTimeLoad"] ?? "0";
                    ctl.LoadMasterControl();
                    if (firstTimeLoad == "1")
                    {
                        string param = Request.Form["hdnRightPanelContentParam"] ?? "";
                        ctl.InitializeControl(param);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0} ({1})", ex.Message, ex.Source));
            }
        }
        #endregion

        protected void cbpRightPanelContent_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
        }

        private BaseRpt GetReport(string className)
        {
            Assembly assembly = Assembly.Load("QIS.Medinfras.Report, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
            Object o = assembly.CreateInstance("QIS.Medinfras.Report." + className);
            return (BaseRpt)o;
        }

        protected bool InsertReportPrintLog(string ReportCode, string ReportParameter)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ReportPrintLogDao entityDao = new ReportPrintLogDao(ctx);
            try
            {
                if (ReportCode != null && ReportCode != "")
                {
                    ReportMaster rm = BusinessLayer.GetReportMasterList(string.Format("ReportCode = '{0}'", ReportCode), ctx).FirstOrDefault();

                    ReportPrintLog entity = new ReportPrintLog();
                    entity.ReportID = rm.ReportID;
                    entity.ReportCode = rm.ReportCode;
                    entity.ReportParameter = ReportParameter;
                    entity.PrintedBy = AppSession.UserLogin.UserID;
                    entity.PrintedDate = DateTime.Now;
                    entityDao.Insert(entity);

                    ctx.CommitTransaction();
                }
            }
            catch (Exception ex)
            {
                result = false;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        protected void cbpCloselogout_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            if (HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME] != null)
            {

                HttpCookie Cookie = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_NAME];
                Cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(Cookie);

                HttpCookie Cookie2 = HttpContext.Current.Request.Cookies[Constant.SessionName.COOKIES_ASPNET_SessionId];
                Cookie2.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(Cookie2);

            }

        }

        public String getCNSetting()
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["cnsetting"];
            string cnstring = settings.ConnectionString;
            string paramDec = Encryption.DecryptString(cnstring);

            return paramDec;
        }

        private string GetFilterExpression(string value)
        {
            StringBuilder sbResult = new StringBuilder(value);
            sbResult.Replace("@HealthcareID", AppSession.UserLogin.HealthcareID);
            sbResult.Replace("@UserID", AppSession.UserLogin.UserID.ToString());
            if (value.Contains("@LaboratoryID") || value.Contains("@ImagingID"))
            {
                List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID, Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID));
                string laboratoryID = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID).ParameterValue;
                string imagingID = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IMAGING_SERVICE_UNIT_ID).ParameterValue;
                List<vHealthcareServiceUnit> lstHealthcareServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND ServiceUnitID IN ({1},{2}) AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, laboratoryID, imagingID));
                sbResult.Replace("@LaboratoryID", lstHealthcareServiceUnit.FirstOrDefault(p => p.ServiceUnitID == Convert.ToInt32(laboratoryID)).HealthcareServiceUnitID.ToString());
                sbResult.Replace("@ImagingID", lstHealthcareServiceUnit.FirstOrDefault(p => p.ServiceUnitID == Convert.ToInt32(imagingID)).HealthcareServiceUnitID.ToString());
            }
            return sbResult.ToString();
        }
    }
}