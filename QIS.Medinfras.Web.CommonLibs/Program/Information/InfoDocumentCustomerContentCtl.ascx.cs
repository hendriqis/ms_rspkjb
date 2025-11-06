using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Web.UI.HtmlControls;
using System.IO;

namespace QIS.Medinfras.Web.CommonLibs.Information
{
    public partial class InfoDocumentCustomerContentCtl : BaseViewPopupCtl
    {
        public static   List<string> ImageExtensions = new List<string>();

        public override void InitializeDataControl(string param)
        {
            hdnContractID.Value = param;
            string filterExpression = "";
            filterExpression = string.Format("ID = {0} AND IsDeleted = 0", param);
            List<vContractDocument> entity = BusinessLayer.GetvContractDocumentIdx(filterExpression, 0, "ID DESC");
            EntityToControl(entity);
        }

        private void EntityToControl(List<vContractDocument> entity)
        {
            spnFileName.InnerHtml = entity.FirstOrDefault().FileName;
            spnDocumentType.InnerHtml = entity.FirstOrDefault().DocumentType;
            spnRemarks.InnerHtml = entity.FirstOrDefault().Remarks;
            
            /*image validasi 
             */
            string imageFormat = hdnImagFormat.Value;
            string[] imgFormatData = imageFormat.Split('|');
            List<string> listimgFormat = new List<string>();

          
            if (imgFormatData.Length > 0)
            {

                for (int i = 0; i < imgFormatData.Length; i++)
                {
                    string format = string.Format("{0}", imgFormatData[i].ToLower());
                    listimgFormat.Add(format);
                }
            }

            string[] arr = listimgFormat.ToArray();
            var target = entity.FirstOrDefault().FileName.ToLower();
            var result = target.Substring(target.Length - 3);
            if (arr.Contains(result))
            {
                ////Console.Write("OK");
                imgContractImage.Src = entity.FirstOrDefault().FileImageUrl;
            }
            else {
                //Console.Write("NO");
                ////entity.FirstOrDefault().FileImageUrl;
                imgContractImage.Src = ResolveUrl("~/libs/Images/Icon/document.png");
            }
            hdnURLFile.Value = entity.FirstOrDefault().FileImageUrl;
           //// imgContractImage.Src = entity.FirstOrDefault().FileImageUrl;
            hdnID.Value = entity.FirstOrDefault().ID.ToString();

        }
    }
}