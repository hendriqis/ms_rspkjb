﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ActiveRegistrationByMRNCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] lstParam = param.Split('|');
            hdnMRNCtl.Value = lstParam[0];
            hdnRegistrationNoCtl.Value = lstParam[3];

            txtPatient.Text = string.Format("({0}) {1}", lstParam[1], lstParam[2]);

            BindGridView(param);
        }

        private void BindGridView(string param)
        {
            string filterExpression = string.Format("MRN = {0} AND GCRegistrationStatus NOT IN ('{1}','{2}') ORDER BY RegistrationDate DESC, RegistrationID DESC", hdnMRNCtl.Value, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED);
            List<vRegistrationActiveByMRN> lstEntity = BusinessLayer.GetvRegistrationActiveByMRNList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            HtmlImage imgIsHasOutstanding = (HtmlImage)e.Row.FindControl("imgIsHasOutstanding");

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vRegistrationActiveByMRN entity = e.Row.DataItem as vRegistrationActiveByMRN;

                if (entity.RegistrationNo == hdnRegistrationNoCtl.Value)
                {
                    e.Row.BackColor = System.Drawing.Color.Aqua;
                }

                string filterReg = string.Format("RegistrationID = {0}", entity.RegistrationID);
                Registration entityReg = BusinessLayer.GetRegistrationList(filterReg).FirstOrDefault();

                decimal totalLineAmount = (entityReg.ChargesAmount + entityReg.SourceAmount + entityReg.AdminAmount - entityReg.DiscountAmount + entityReg.RoundingAmount - entityReg.TransferAmount);
                decimal remaining = entityReg.PaymentAmount - totalLineAmount;

                imgIsHasOutstanding.Attributes.Add("title", String.Format("Remaining Amount = {0}",
                                                    remaining.ToString(Constant.FormatString.NUMERIC_2)));
            }
        }
    }
}