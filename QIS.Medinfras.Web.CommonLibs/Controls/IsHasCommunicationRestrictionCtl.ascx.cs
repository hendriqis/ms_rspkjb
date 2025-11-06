﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class IsHasCommunicationRestrictionCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            txtMRN.Text = paramInfo[1];
            txtPatientName.Text = paramInfo[2];

            Patient opatient = BusinessLayer.GetPatient(Convert.ToInt32(paramInfo[0]));
            if (opatient != null)
            {
                if (opatient.IsHasCommunicationRestriction)
                {
                    if (opatient.GCCommunicationRestriction != "")
                    {
                        StandardCode sc = BusinessLayer.GetStandardCode(opatient.GCCommunicationRestriction);
                        txtInformation.Text = sc.StandardCodeName;
                    }
                }
            }
        }
    }
}