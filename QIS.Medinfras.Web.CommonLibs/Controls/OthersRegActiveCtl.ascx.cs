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
    public partial class OthersRegActiveCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            txtMRN.Text = paramInfo[1];
            txtPatientName.Text = paramInfo[2];

            string otherActiveReg = "";
            List<Registration> lstOtherActiveReg = BusinessLayer.GetRegistrationList(string.Format("RegistrationNo != '{0}' AND MRN = {1} AND GCRegistrationStatus NOT IN ('{2}','{3}') ORDER BY RegistrationDate, RegistrationID", paramInfo[3], paramInfo[0], Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED));
            if (lstOtherActiveReg.Count() > 0)
            {
                foreach (Registration oReg in lstOtherActiveReg)
                {
                    StandardCode regStatus = BusinessLayer.GetStandardCode(oReg.GCRegistrationStatus);

                    if (!String.IsNullOrEmpty(otherActiveReg))
                    {
                        otherActiveReg += string.Format("\n{0} ({1})", oReg.RegistrationNo, regStatus.StandardCodeName);
                    }
                    else
                    {
                        otherActiveReg = string.Format("{0} ({1})", oReg.RegistrationNo, regStatus.StandardCodeName);
                    }
                }
                txtInformation.Text = otherActiveReg;
            }
        }
    }
}