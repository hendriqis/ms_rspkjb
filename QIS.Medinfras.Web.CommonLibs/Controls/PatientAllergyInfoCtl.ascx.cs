﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using System.Drawing;
using System.IO;
using QIS.Medinfras.Data.Service;
using ThoughtWorks.QRCode.Codec;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientAllergyInfoCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            BindGridView();    
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("MRN = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.MRN);

            List<vPatientAllergy> lstEntity = BusinessLayer.GetvPatientAllergyList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
    }
}
