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
using QIS.Data.Core.Dal;
using Newtonsoft.Json;
using QIS.Medinfras.Web.CommonLibs.Service;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class SearchPraRegistrationInhealthCtl : BaseProcessPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            txtDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
        }

        //private string GetBPJSPatientReferralList(string noPeserta, string asalRujukan)
        //{
        //    StandardCode entitySC = BusinessLayer.GetStandardCodeList(string.Format("StandardCodeID = '{0}'", asalRujukan)).FirstOrDefault();
        //    string resultList = string.Empty;
        //    string resultListRS = string.Empty;
        //    string result = string.Empty;
        //    string resultRS = string.Empty;
        //    string finalResult = string.Empty;

        //    string type = string.Empty;
        //    if (entitySC != null)
        //    {
        //        if (entitySC.TagProperty.Contains('|'))
        //        {
        //            string[] tagProp = entitySC.TagProperty.Split('|');
        //            type = tagProp[1];
        //        }
        //    }

        //    BPJSService bpjsAPI = new BPJSService();
        //    if (type == "1") // Faskes 1
        //    {
        //        if (hdnIsBridgingBPJSVClaimVersion.Value == Constant.BPJS_Version_Release.v1_0)
        //        {
        //            resultList = bpjsAPI.GetRujukanListByNoPeserta(noPeserta, asalRujukan); //Rujukan Berdasarkan Nomor Kartu (Multi Record)
        //            string[] resultInfoList = resultList.Split('|');
        //            if (resultInfoList[0] == "1")
        //            {
        //                RujukanListAPIResponse apiResponseList = JsonConvert.DeserializeObject<RujukanListAPIResponse>(resultInfoList[1]);
        //                if (apiResponseList.response.rujukan.Count > 1)
        //                {
        //                    finalResult = resultList;
        //                    grdView.DataSource = ConvertToRujukanListDataModel(apiResponseList);
        //                    grdView.DataBind();
        //                }
        //                else
        //                {
        //                    result = bpjsAPI.GetRujukanByNoPeserta(noPeserta, asalRujukan); //Rujukan Berdasarkan Nomor Kartu (1 Record)
        //                    string[] resultInfo = result.Split('|');
        //                    if (resultInfo[0] == "1")
        //                    {
        //                        RujukanAPIResponse apiResponse = JsonConvert.DeserializeObject<RujukanAPIResponse>(resultInfo[1]);
        //                        finalResult = result;
        //                        grdView.DataSource = ConvertToRujukanDataModel(apiResponse);
        //                        grdView.DataBind();
        //                    }
        //                    else
        //                    {
        //                        grdView.DataSource = ConvertToRujukanDataModel(null);
        //                        grdView.DataBind();
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                grdView.DataSource = ConvertToRujukanDataModel(null);
        //                grdView.DataBind();
        //            }
        //        }
        //        else
        //        {
        //            resultList = bpjsAPI.GetRujukanListByNoPeserta_MEDINFRASAPI(noPeserta, asalRujukan); //Rujukan Berdasarkan Nomor Kartu (Multi Record) API
        //            string[] resultInfoList = resultList.Split('|');
        //            if (resultInfoList[0] == "1")
        //            {
        //                RujukanListAPIResponse apiResponseList = JsonConvert.DeserializeObject<RujukanListAPIResponse>(resultInfoList[1]);
        //                if (resultInfoList[2] == "1")
        //                {
        //                    finalResult = resultList;
        //                    grdView.DataSource = ConvertToRujukanListDataModel(apiResponseList);
        //                    grdView.DataBind();
        //                }
        //                else
        //                {
        //                    result = bpjsAPI.GetRujukanByNoPeserta_MEDINFRASAPI(noPeserta, asalRujukan); //Rujukan Berdasarkan Nomor Kartu (1 Record) API
        //                    string[] resultInfo = result.Split('|');
        //                    if (resultInfo[0] == "1")
        //                    {
        //                        RujukanAPIResponse apiResponse = JsonConvert.DeserializeObject<RujukanAPIResponse>(resultInfo[1]);
        //                        finalResult = result;
        //                        grdView.DataSource = ConvertToRujukanDataModel(apiResponse);
        //                        grdView.DataBind();
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    else if (type == "2")
        //    {
        //        if (hdnIsBridgingBPJSVClaimVersion.Value == Constant.BPJS_Version_Release.v1_0)
        //        {
        //            resultListRS = bpjsAPI.GetRujukanListByNoPesertaRS(noPeserta, asalRujukan); //Rujukan Berdasarkan Nomor Kartu (Multi Record) RS
        //            string[] resultInfoListRS = resultListRS.Split('|');
        //            if (resultInfoListRS[0] == "1")
        //            {
        //                RujukanListAPIResponse apiResponseList = JsonConvert.DeserializeObject<RujukanListAPIResponse>(resultInfoListRS[1]);
        //                if (apiResponseList.response.rujukan.Count > 1)
        //                {
        //                    finalResult = resultListRS;
        //                    grdView.DataSource = ConvertToRujukanListDataModel(apiResponseList);
        //                    grdView.DataBind();
        //                }
        //                else
        //                {
        //                    resultRS = bpjsAPI.GetRujukanByNoPesertaRS(noPeserta, asalRujukan); //Rujukan Berdasarkan Nomor Kartu (1 Record) RS
        //                    string[] resultInfoRS = resultRS.Split('|');
        //                    if (resultInfoRS[0] == "1")
        //                    {
        //                        RujukanAPIResponse apiResponse = JsonConvert.DeserializeObject<RujukanAPIResponse>(resultInfoRS[1]);
        //                        finalResult = resultRS;
        //                        grdView.DataSource = ConvertToRujukanDataModel(apiResponse);
        //                        grdView.DataBind();
        //                    }
        //                    else
        //                    {
        //                        grdView.DataSource = ConvertToRujukanDataModel(null);
        //                        grdView.DataBind();
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                grdView.DataSource = ConvertToRujukanDataModel(null);
        //                grdView.DataBind();
        //            }
        //        }
        //        else
        //        {
        //            resultListRS = bpjsAPI.GetRujukanListByNoPesertaRS_MEDINFRASAPI(noPeserta, asalRujukan); //Rujukan Berdasarkan Nomor Kartu (Multi Record) RS API
        //            string[] resultInfoListRS = resultListRS.Split('|');
        //            if (resultInfoListRS[0] == "1")
        //            {
        //                RujukanListAPIResponse apiResponseList = JsonConvert.DeserializeObject<RujukanListAPIResponse>(resultInfoListRS[1]);
        //                if (resultInfoListRS[2] == "1")
        //                {
        //                    finalResult = resultList;
        //                    grdView.DataSource = ConvertToRujukanListDataModel(apiResponseList);
        //                    grdView.DataBind();
        //                }
        //                else
        //                {
        //                    result = bpjsAPI.GetRujukanByNoPesertaRS_MEDINFRASAPI(noPeserta, asalRujukan); //Rujukan Berdasarkan Nomor Kartu (1 Record) RS API
        //                    string[] resultInfo = result.Split('|');
        //                    if (resultInfo[0] == "1")
        //                    {
        //                        RujukanAPIResponse apiResponse = JsonConvert.DeserializeObject<RujukanAPIResponse>(resultInfo[1]);
        //                        finalResult = result;
        //                        grdView.DataSource = ConvertToRujukanDataModel(apiResponse);
        //                        grdView.DataBind();
        //                    }
        //                    else
        //                    {
        //                        grdView.DataSource = ConvertToRujukanDataModel(null);
        //                        grdView.DataBind();
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                grdView.DataSource = ConvertToRujukanDataModel(null);
        //                grdView.DataBind();
        //            }
        //        }
        //    }
        //    return finalResult;

        //}

        private string GetInhealthPraRegistration()
        {
            string result = string.Empty;

            InhealthService oService = new InhealthService();
            result = oService.ListPraRegistrasi(Helper.GetDatePickerValue(txtDate.Text).ToString(Constant.FormatString.DATE_FORMAT_3), txtQuery.Text);
            if (result.Split('|')[0] == "1")
            {
                List<PraRegistrasiListResponse> listPraRegistration = JsonConvert.DeserializeObject<List<PraRegistrasiListResponse>>(result.Split('|')[2]);
                grdView.DataSource = listPraRegistration;
                grdView.DataBind();
            }
                

            return result;
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            string[] param = e.Parameter.Split('|');
            string result = param[0] + "|";
            result = GetInhealthPraRegistration();
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        public override void SetProcessButtonVisibility(ref bool IsUsingProcessButton)
        {
            IsUsingProcessButton = true;
        }

        protected override bool OnProcessRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            return result;
        }
    }
}