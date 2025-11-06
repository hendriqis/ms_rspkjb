using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Laboratory.Program
{
    public partial class FractionNormalValueEntryCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        public override void InitializeDataControl(string param)
        {
            hdnFractionID.Value = param;
            Fraction entity = BusinessLayer.GetFraction(Convert.ToInt32(hdnFractionID.Value));
            txtFractionCode.Text = entity.FractionCode;
            txtFractionName.Text = entity.FractionName1;
            hdnConversionFactor.Value = entity.ConversionFactor.ToString();
            hdnGCMetricUnit.Value = entity.GCMetricUnit;
            hdnGCInternationalUnit.Value = entity.GCInternationalUnit;

            StandardCode metricUnit = BusinessLayer.GetStandardCode(entity.GCMetricUnit);
            if (metricUnit != null)
            {
                txtMetricUnit.Text = metricUnit.StandardCodeName;
            }

            StandardCode internationalUnit = BusinessLayer.GetStandardCode(entity.GCInternationalUnit);
            if (internationalUnit != null)
            {
                txtInternationalUnit.Text = internationalUnit.StandardCodeName;
            }

            BindGridView(1, true, ref PageCount);
            SetControlProperties();
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("FractionID = {0} AND IsDeleted = 0", hdnFractionID.Value);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvFractionNormalValueRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 8);
            }

            List<vFractionNormalValue> lstEntity = BusinessLayer.GetvFractionNormalValueList(filterExpression, 8, pageIndex, "ID ASC");
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            }
        }

        private void SetControlProperties()
        {
            List<StandardCode> lstSC = BusinessLayer.GetStandardCodeList(string.Format(
                    "ParentID IN ('{0}','{1}','{2}') AND IsDeleted = 0 AND StandardCodeID NOT IN ('{3}')",
                    Constant.StandardCode.AGE_UNIT, Constant.StandardCode.GENDER, Constant.StandardCode.LABORATORY_UNIT, Constant.Gender.UNSPECIFIED));

            Methods.SetComboBoxField<StandardCode>(cboAgeUnit, lstSC.Where(p => p.ParentID == Constant.StandardCode.AGE_UNIT).ToList(), "StandardCodeName", "StandardCodeID");//field ke 3 untuk yang ditampilin ke layar, field ke 4 itu yg dikirim untuk diproses
            lstSC.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            lstSC.Add(new StandardCode() { StandardCodeID = "GCCustom^LP", StandardCodeName = "Laki & Perempuan" });
            Methods.SetComboBoxField<StandardCode>(cboSex, lstSC.Where(p => p.ParentID == Constant.StandardCode.GENDER || p.StandardCodeID == "" || p.StandardCodeID == "GCCustom^LP").ToList(), "StandardCodeName", "StandardCodeID");
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            int pageCount = 1;
            string[] param = e.Parameter.Split('|');

            string result = param[0] + "|";
            string errMessage = "";

            if (param[0] == "changepage")
            {
                BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                result = "changepage";
            }
            else if (param[0] == "refresh")
            {
                BindGridView(1, true, ref pageCount);
                result = string.Format("refresh|{0}", pageCount);
            }
            else
            {
                if (param[0] == "save")
                {
                    if (hdnID.Value.ToString() != "")
                    {
                        if (OnSaveEditRecord(ref errMessage))
                            result += "success";
                        else
                            result += string.Format("fail|{0}", errMessage);
                    }
                    else
                    {
                        if (OnSaveAddRecord(ref errMessage))
                            result += "success";
                        else
                            result += string.Format("fail|{0}", errMessage);
                    }
                }
                else if (param[0] == "delete")
                {
                    if (OnDeleteRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }

                BindGridView(1, true, ref PageCount);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(FractionNormalValue entity, string jk)
        {
            if (jk == "")
                entity.GCSex = "";
            else
                entity.GCSex = jk;
            entity.FromAge = Convert.ToInt32(txtFromAge.Text);
            entity.ToAge = Convert.ToInt32(txtToAge.Text);
            entity.GCAgeUnit = cboAgeUnit.Value.ToString();
            entity.IsPregnant = chkIsPregnant.Checked;

            switch (cboAgeUnit.Value.ToString())
            {
                case "X008^001": entity.FromAgeInDay = entity.FromAge; break;
                case "X008^002": entity.FromAgeInDay = entity.FromAge * 7; break;
                case "X008^003": entity.FromAgeInDay = entity.FromAge * 30; break;
                case "X008^004": entity.FromAgeInDay = entity.FromAge * 365; break;
            }

            switch (cboAgeUnit.Value.ToString())
            {
                case "X008^001": entity.ToAgeInDay = entity.ToAge; break;
                case "X008^002": entity.ToAgeInDay = entity.ToAge * 7; break;
                case "X008^003": entity.ToAgeInDay = entity.ToAge * 30; break;
                case "X008^004": entity.ToAgeInDay = entity.ToAge * 365; break;
            }

            //Metric Unit
            entity.GCMetricUnit = hdnGCMetricUnit.Value;
            entity.MetricUnitMin = Convert.ToDecimal(txtMetricUnitMin.Text);
            entity.MetricUnitMax = Convert.ToDecimal(txtMetricUnitMax.Text);
            entity.MetricUnitLabel = txtMetricUnitLabel.Text;
            //entity.MetricUnitLabel = String.IsNullOrEmpty(txtMetricUnitLabel.Text) ? string.Format("{0}-{1} {2}", entity.MetricUnitMin, entity.MetricUnitMax, cboMetricUnit.Text) : txtMetricUnitLabel.Text;
            entity.PanicMetricUnitMin = Convert.ToDecimal(txtPanicMetricUnitMin.Text);
            entity.PanicMetricUnitMax = Convert.ToDecimal(txtPanicMetricUnitMax.Text);
            entity.PanicMetricUnitLabel = txtPanicMetricUnitLabel.Text;
            //entity.PanicMetricUnitLabel = String.IsNullOrEmpty(txtPanicMetricUnitLabel.Text) ? string.Format("{0}-{1} {2}", entity.PanicMetricUnitMin, entity.PanicMetricUnitMax, cboMetricUnit.Text) : txtPanicMetricUnitLabel.Text;

            //International Unit
            entity.GCInternationalUnit = hdnGCInternationalUnit.Value;
            entity.InternationalUnitMin = Convert.ToDecimal(txtInternationalUnitMin.Text);
            entity.InternationalUnitMax = Convert.ToDecimal(txtInternationalUnitMax.Text);
            entity.InternationalUnitLabel = txtInternationalUnitLabel.Text;
            //entity.InternationalUnitLabel = String.IsNullOrEmpty(txtInternationalUnitLabel.Text) ? string.Format("{0}-{1} {2}", entity.InternationalUnitMin, entity.InternationalUnitMax, cboInternationalUnit.Text) : txtInternationalUnitLabel.Text;
            entity.PanicInternationalUnitMin = Convert.ToDecimal(txtPanicInternationalUnitMin.Text);
            entity.PanicInternationalUnitMax = Convert.ToDecimal(txtPanicInternationalUnitMax.Text);
            entity.PanicInternationalUnitLabel = txtPanicInternationalUnitLabel.Text;
            //entity.PanicInternationalUnitLabel = String.IsNullOrEmpty(txtPanicInternationalUnitLabel.Text) ? string.Format("{0}-{1} {2}", entity.PanicInternationalUnitMin, entity.PanicInternationalUnitMax, cboInternationalUnit.Text) : txtPanicInternationalUnitLabel.Text;
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
             IDbContext ctx = DbFactory.Configure(true);
             FractionNormalValueDao entityDao = new FractionNormalValueDao(ctx);
            try
            {               
                FractionNormalValue entity = new FractionNormalValue();
                int flag = 1;
                if (cboSex.Value.ToString() == "GCCustom^LP")
                {
                    flag = 2;
                }
                if (flag == 1)
                {
                    ControlToEntity(entity, cboSex.Value.ToString());
                    entity.FractionID = Convert.ToInt32(hdnFractionID.Value);
                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    entityDao.Insert(entity);;
                }
                else
                {
                    ControlToEntity(entity, Constant.Gender.MALE);
                    entity.FractionID = Convert.ToInt32(hdnFractionID.Value);
                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    entityDao.Insert(entity);
                    entity.GCSex = Constant.Gender.FEMALE;
                    entityDao.Insert(entity);
                }
                ctx.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                ctx.RollBackTransaction();
                return false;
            }
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                FractionNormalValue entity = BusinessLayer.GetFractionNormalValue(Convert.ToInt32(hdnID.Value));
                int flag = 1;
                if (cboSex.Value.ToString() == "GCCustom^LP")
                {
                    flag = 2;
                }
                if (flag == 1)
                {
                    ControlToEntity(entity, cboSex.Value.ToString());
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateFractionNormalValue(entity);
                }
                else
                {
                    errMessage = "Untuk edit harap di lakukan per gender (laki-laki atau perempuan)";
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            try
            {
                FractionNormalValue entity = BusinessLayer.GetFractionNormalValue(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateFractionNormalValue(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
    }
}