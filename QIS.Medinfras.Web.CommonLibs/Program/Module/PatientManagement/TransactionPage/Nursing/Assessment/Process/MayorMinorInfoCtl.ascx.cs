using System;
using System.Collections.Generic;
using System.Data;
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
//using QIS.Medinfras.Common;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Linq;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class MayorMinorInfoCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnTransactionIDCtl.Value = paramInfo[0];
            txtNursingTransactionNo.Text = paramInfo[1];
            hdnNursingDiagnoseIDCtl.Value = paramInfo[2];

            string filter = string.Format("TransactionID = '{0}'", hdnTransactionIDCtl.Value);
            vNursingTransactionHd entity = BusinessLayer.GetvNursingTransactionHdList(filter).FirstOrDefault();

            divSubjectiveMayorCtl.InnerHtml = entity.PercentageSubjectiveMayor.ToString("N0");
            divObjectiveMayorCtl.InnerHtml = entity.PercentageObjectiveMayor.ToString("N0");
            divSubjectiveMinorCtl.InnerHtml = entity.PercentageSubjectiveMinor.ToString("N0");
            divObjectiveMinorCtl.InnerHtml = entity.PercentageObjectiveMinor.ToString("N0");
        }
    }
}