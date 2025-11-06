using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BPurchaseOrderReceive : BaseCustomDailyPotraitRpt
    {
        public BPurchaseOrderReceive()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vPurchaseReceiveHd entityHD = BusinessLayer.GetvPurchaseReceiveHdList(param[0])[0];
            BusinessPartners entitySup = BusinessLayer.GetBusinessPartners(entityHD.SupplierID);
            Address entitySupAd = BusinessLayer.GetAddress(Convert.ToInt32(entitySup.AddressID));

            #region Header
            cSupplier.Text = entityHD.SupplierName;
            cSupplierAddress.Text = entitySupAd.StreetName + " " + entitySupAd.City;
            cPhoneNo.Text = entitySupAd.PhoneNo1;
            cContactPerson.Text = entitySup.ContactPerson;
            cLocation.Text = entityHD.LocationName;
            cVAT.Text = entityHD.VATPercentage.ToString("N2") + "%";
            cNotes.Text = entityHD.Remarks;
            #endregion

            #region Footer
            cTTDReviewedBy.Text = entityHD.LastUpdatedByName;
            cTTDPrintedBy.Text = appSession.UserFullName;
            #endregion

            base.InitializeReport(param);
        }

    }
}
