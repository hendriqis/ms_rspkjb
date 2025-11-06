using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class LocationDetailListCtl : BaseViewPopupCtl
    {

        protected int PageCount = 1;

        public override void InitializeDataControl(string param)
        {
            hdnParentLocationID.Value = param;
            vLocation entity = BusinessLayer.GetvLocationList(string.Format("LocationID = '{0}'", hdnParentLocationID.Value)).FirstOrDefault();
            txtParentLocationCode.Text = entity.LocationCode;
            txtParentLocationName.Text = entity.LocationName;
            hdnHealthcareID.Value = entity.HealthcareID;
            BindGridView(1, true, ref PageCount);


            txtLocationCode.Attributes.Add("validationgroup", "mpEntryPopup");
            txtLocationName.Attributes.Add("validationgroup", "mpEntryPopup");

            List<StandardCode> lstLocationGroup = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.LOCATION_GROUP));
            lstLocationGroup.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboGCLocationGroup, lstLocationGroup, "StandardCodeName", "StandardCodeID");

            string filterExpression = string.Format("ParentID IN ('{0}') AND IsDeleted = 0", Constant.StandardCode.TRANSACTION_TYPE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboTransactionType, lstStandardCode, "StandardCodeName", "StandardCodeID");

            List<StandardCode> lstHealthcare = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.HEALTHCARE_UNIT));
            lstHealthcare.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboHealthcareUnit, lstHealthcare, "StandardCodeName", "StandardCodeID");
            cboHealthcareUnit.SelectedIndex = 0;
        }

        protected string OnGetItemGroupFilterExpression()
        {
            return string.Format("GCItemType IN ('{0}','{1}','{2}') AND IsDeleted = 0", Constant.ItemGroupMaster.DRUGS, Constant.ItemGroupMaster.SUPPLIES, Constant.ItemGroupMaster.LOGISTIC);
        }

        protected string OnGetItemBedChargesFilterExpression()
        {
            return string.Format("GCItemType = '{0}' AND IsDeleted = 0", Constant.ItemGroupMaster.SERVICE);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            String filterExpression = string.Format("ParentID = '{0}' AND IsDeleted = 0", hdnParentLocationID.Value);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvLocationRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 8);
            }
            List<vLocation> lstEntity = BusinessLayer.GetvLocationList(filterExpression, 8, pageIndex, "LocationCode ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            int pageCount = 1;
            string[] param = e.Parameter.Split('|');

            string result = param[0] + "|";
            string errMessage = "";

            if (param[0] == "save")
            {
                if (hdnLocationID.Value.ToString() != "")
                {
                    if (OnSaveEditRecord(ref errMessage))
                    {
                        result += "success";
                        BindGridView(1, true, ref pageCount);
                    }
                    else
                    {
                        result += string.Format("fail|{0}", errMessage);
                    }
                }
                else
                {
                    if (OnSaveAddRecord(ref errMessage))
                    {
                        result += "success";
                        BindGridView(1, true, ref pageCount);
                    }
                    else
                    {
                        result += string.Format("fail|{0}", errMessage);
                    }
                }
            }
            else if (param[0] == "delete")
            {
                if (OnDeleteRecord(ref errMessage))
                {
                    result += "success";
                    BindGridView(1, true, ref pageCount);
                }
                else
                {
                    result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "changepage")
            {
                BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                result = "changepage";
            }
            else
            {
                BindGridView(1, true, ref pageCount);
            }

            result += "|" + pageCount;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(Location entity)
        {
            entity.LocationCode = txtLocationCode.Text;
            entity.LocationName = txtLocationName.Text;
            entity.ShortName = txtShortName.Text;
            if (hdnItemGroupID.Value == "" || hdnItemGroupID.Value == "0")
                entity.ItemGroupID = null;
            else
                entity.ItemGroupID = Convert.ToInt32(hdnItemGroupID.Value);
            if (hdnRestrictionID.Value == "" || hdnRestrictionID.Value == "0")
                entity.RestrictionID = null;
            else
                entity.RestrictionID = Convert.ToInt32(hdnRestrictionID.Value);

            if (hdnDistributionLocationID.Value == "" || hdnDistributionLocationID.Value == "0")
                entity.DistributionLocationID = null;
            else
                entity.DistributionLocationID = Convert.ToInt32(hdnDistributionLocationID.Value);

            if (hdnRequestLocationID.Value == "" || hdnRequestLocationID.Value == "0")
                entity.RequestLocationID = null;
            else
                entity.RequestLocationID = Convert.ToInt32(hdnRequestLocationID.Value);

            if (cboGCLocationGroup.Value != null)
            {
                entity.GCLocationGroup = string.IsNullOrEmpty(cboGCLocationGroup.Value.ToString()) ? null : cboGCLocationGroup.Value.ToString();
            }
            else
            {
                entity.GCLocationGroup = null;
            }

            if (cboTransactionType.Value != null)
            {
                entity.GCItemRequestType = cboTransactionType.Value.ToString();
            }

            if (cboHealthcareUnit.Value != null)
            {
                entity.GCHealthcareUnit = cboHealthcareUnit.Value.ToString();
            }

            entity.IsAllowOverIssued = chkIsAllowOverIssued.Checked;
            entity.IsAvailable = chkIsAvailable.Checked;
            entity.IsHoldForTransaction = chkIsHoldForTransaction.Checked;
            entity.IsNettable = chkIsNettable.Checked;
            entity.IsMinMaxReadOnly = chkIsMinMaxReadOnly.Checked;
            entity.IsUsingLocationAverageQty = chkisUsingLocationAverageQty.Checked;
            entity.IsPatientUseLocation = chkisUsingLocationPatient.Checked;
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                List<Location> lstCheckCode = BusinessLayer.GetLocationList(string.Format("LocationCode = '{0}' AND IsDeleted = 0", txtLocationCode.Text));
                if (lstCheckCode.Count() == 0)
                {
                    Location entity = new Location();
                    ControlToEntity(entity);
                    entity.ParentID = Convert.ToInt32(hdnParentLocationID.Value);
                    entity.HealthcareID = hdnHealthcareID.Value;
                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    entity.IsHeader = false;
                    BusinessLayer.InsertLocation(entity);
                    return true;
                }
                else
                {
                    errMessage = "Maaf, kode lokasi " + txtLocationCode.Text + " sudah digunakan di lokasi lain.";
                    return false;
                }

            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                List<Location> lstCheckCode = BusinessLayer.GetLocationList(string.Format("LocationCode = '{0}' AND LocationID != {1} AND IsDeleted = 0", txtLocationCode.Text, hdnLocationID.Value));
                if (lstCheckCode.Count() == 0)
                {
                    Location entity = BusinessLayer.GetLocation(Convert.ToInt32(hdnLocationID.Value));
                    ControlToEntity(entity);
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateLocation(entity);
                    return true;
                }
                else
                {
                    errMessage = "Maaf, kode lokasi " + txtLocationCode.Text + " sudah digunakan di lokasi lain.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            try
            {
                Location entity = BusinessLayer.GetLocation(Convert.ToInt32(hdnLocationID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateLocation(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }
    }
}