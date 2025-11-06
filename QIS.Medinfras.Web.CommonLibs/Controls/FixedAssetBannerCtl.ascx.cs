using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class FixedAssetBannerCtl : BaseUserControlCtl
    {
        public void InitializeFixedAssetBanner(vFAItem entity)
        {
            lblFixedAssetName.InnerHtml = entity.FixedAssetName;
            lblFixedAssetCode.InnerHtml = entity.FixedAssetCode;
            lblSerialNo.InnerHtml = entity.SerialNumber;
            lblFAItemGroup.InnerHtml = entity.FAGroupName;
            lblFALocation.InnerHtml = entity.FALocationName;
        }
    }
}