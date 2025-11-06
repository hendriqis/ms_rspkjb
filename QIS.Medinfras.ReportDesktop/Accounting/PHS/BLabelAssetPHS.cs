using System;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using ThoughtWorks.QRCode.Codec;
using System.IO;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BLabelAssetPHS : BaseRpt
    {
        public BLabelAssetPHS()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string id = param[0];
            vFAItem oData = BusinessLayer.GetvFAItemList(string.Format("FixedAssetID = '{0}'", id)).FirstOrDefault();

            #region QR Codes Image
            string AssetCode = oData.FixedAssetCode;
            string AssetName = oData.FixedAssetName;
            string AssetLocation = oData.FALocationName;
            string RevenueCostCenter ="";
            if (oData.FALocationID > 0) {
                vRevenueCostCenterFALocation oRevenueCostCenter = BusinessLayer.GetvRevenueCostCenterFALocationList(string.Format("FALocationID='{0}'", oData.FALocationID)).FirstOrDefault();
                if (oRevenueCostCenter != null) {
                    RevenueCostCenter = oRevenueCostCenter.RevenueCostCenterName; 
                }
               
            }
            string AssetsGroup = oData.FAGroupName;
            string KategoriAnggaran = oData.BudgetCategory;
            string NomorPenerimaanBarang = "";
            string NomorPo = "";
            if (oData.PurchaseReceiveID > 0) {
                vPurchaseReceiveHd oPrhd = BusinessLayer.GetvPurchaseReceiveHdList(string.Format("PurchaseReceiveID = {0}", oData.PurchaseReceiveID)).FirstOrDefault();
                NomorPenerimaanBarang = oPrhd.PurchaseReceiveNo;
                NomorPo = oPrhd.PurchaseOrderNo;
            }

            Decimal UmurSusut = oData.DepreciationLength;
            Decimal NilaiPerolehan = oData.ProcurementAmount;

            string contents = string.Format("{0}\n\r{1}\n\r{2}\n\r{3}\n\r{4}\n\r{5}\n\r{6}\n\r{7}\n\r{8}\n\r{9}", AssetCode, AssetName, AssetLocation, RevenueCostCenter, AssetsGroup
                , KategoriAnggaran, NomorPenerimaanBarang, NomorPo, UmurSusut, NilaiPerolehan
                );

            QRCodeEncoder qRCodeEncoder = new QRCodeEncoder();
            qRCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qRCodeEncoder.QRCodeScale = 4;
            qRCodeEncoder.QRCodeVersion = 7;
            qRCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
            MemoryStream memoryStream = new MemoryStream();
            //System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
            //imgBarCode.Height = 400;
            //imgBarCode.Width = 400;

            using (Bitmap bitMap = qRCodeEncoder.Encode(contents, System.Text.Encoding.UTF8))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    //byte[] byteImage = ms.ToArray();
                    //imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                    ////pictMRNQR.Image = System.Drawing.Image.FromStream(ms, true, true);
                    qrImg.Image = System.Drawing.Image.FromStream(ms, true, true);
                }
            }
            #endregion
            this.DataSource = null;
        }
    }

    
}
