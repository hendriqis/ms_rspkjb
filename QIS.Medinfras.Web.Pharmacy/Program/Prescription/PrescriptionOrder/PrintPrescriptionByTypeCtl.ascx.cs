using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Service;
using DevExpress.Web.ASPxCallbackPanel;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class PrintPrescriptionByTypeCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnTransactionIDCtl.Value = paramInfo[0];
            txtTransactionNo.Text = paramInfo[1];

            List<Variable> lstData = new List<Variable>();
            lstData.Add(new Variable { Code = "0", Value = "Semua" });
            lstData.Add(new Variable { Code = "1", Value = "Obat yang Tidak Diambil Saja" });
            Methods.SetComboBoxField<Variable>(cboPrintType, lstData, "Value", "Code");
            cboPrintType.SelectedIndex = 0;
        }
    }
}