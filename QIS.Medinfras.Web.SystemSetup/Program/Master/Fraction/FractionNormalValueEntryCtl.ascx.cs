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

namespace QIS.Medinfras.Web.SystemSetup.Program
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

            BindGridView(1, true, ref PageCount);

            SetControlProperties();
            //txtItemCode.Attributes.Add("validationgroup", "mpEntryPopup");
            //txtMinimum.Attributes.Add("validationgroup", "mpEntryPopup");
            //txtMaximum.Attributes.Add("validationgroup", "mpEntryPopup");
        }

        private void SetControlProperties()
        {
            List<StandardCode> lstSC = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}','{2}') AND IsDeleted = 0", Constant.StandardCode.AGE_UNIT, Constant.StandardCode.LABORATORY_UNIT, Constant.StandardCode.GENDER));
            Methods.SetComboBoxField<StandardCode>(cboAgeUnit, lstSC.Where(p => p.ParentID == Constant.StandardCode.AGE_UNIT).ToList(), "StandardCodeName", "StandardCodeID");//field ke 3 untuk yang ditampilin ke layar, field ke 4 itu yg dikirim untuk diproses
            Methods.SetComboBoxField<StandardCode>(cboMetricUnit, lstSC.Where(p => p.ParentID == Constant.StandardCode.LABORATORY_UNIT).ToList(), "StandardCodeName", "StandardCodeID");//field ke 3 untuk yang ditampilin ke layar, field ke 4 itu yg dikirim untuk diproses
            Methods.SetComboBoxField<StandardCode>(cboInternationalUnit, lstSC.Where(p => p.ParentID == Constant.StandardCode.LABORATORY_UNIT).ToList(), "StandardCodeName", "StandardCodeID");//field ke 3 untuk yang ditampilin ke layar, field ke 4 itu yg dikirim untuk diproses
           
            lstSC.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboSex, lstSC.Where(p => p.ParentID == Constant.StandardCode.GENDER).ToList(), "StandardCodeName", "StandardCodeID");
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

            //lvwView.DataSource = BusinessLayer.GetvItemBalanceList(string.Format("LocationID = {0} AND IsDeleted = 0 ORDER BY ItemName1 ASC", hdnLocationID.Value));
            //lvwView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            }
            
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

                BindGridView(1, true, ref pageCount);
                result += "|" + pageCount;
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(FractionNormalValue entity)
        {
            if (cboSex.Value.ToString() == "" || cboSex.Value == null)
                entity.GCSex = "";
            else
                entity.GCSex = cboSex.Value.ToString();
            entity.FromAge = Convert.ToInt16(txtFromAge.Text);
            entity.ToAge = Convert.ToInt16(txtToAge.Text);
            entity.GCAgeUnit = cboAgeUnit.Value.ToString();
            entity.IsPregnant = chkIsPregnant.Checked;

            entity.MetricUnitMin = Convert.ToDecimal(txtMetricUnitMin.Text);
            entity.MetricUnitMax = Convert.ToDecimal(txtMetricUnitMax.Text);
            entity.MetricUnitLabel = txtMetricUnitLabel.Text;
            entity.GCMetricUnit = cboMetricUnit.Value.ToString();

            entity.InternationalUnitMin = Convert.ToDecimal(txtInternationalUnitMin.Text);
            entity.InternationalUnitMax = Convert.ToDecimal(txtInternationalUnitMax.Text);
            entity.InternationalUnitLabel = txtInternationalUnitLabel.Text;
            entity.GCInternationalUnit = cboInternationalUnit.Value.ToString();
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                FractionNormalValue entity = new FractionNormalValue();
                ControlToEntity(entity);
                entity.FractionID = Convert.ToInt32(hdnFractionID.Value);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertFractionNormalValue(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                FractionNormalValue entity = BusinessLayer.GetFractionNormalValue(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
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