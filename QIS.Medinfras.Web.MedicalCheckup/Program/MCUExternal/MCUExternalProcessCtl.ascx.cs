using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxCallbackPanel;
using System.Data;
using Newtonsoft.Json;
using System.Text;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.MedicalCheckup.Program
{
    public partial class MCUExternalProcessCtl : BaseViewPopupCtl
    {
        protected string defaultParamedicName = string.Empty;
        protected string defaultParamedicID = string.Empty;
        List<PatientChargesDt> lstEntityDt = new List<PatientChargesDt>();
        protected int counter = 0;

        #region Session
        public class MCUOrder
        {
            private int _Key;
            private int _VisitID;
            private int _ItemID;
            private int _DetailItemID;
            private string _DetailItemCode;
            private string _DetailItemName1;
            private string _ServiceUnitName;
            private string _DepartmentID;
            private string _DepartmentName;
            private int _ParamedicID;
            private int _HealthcareServiceUnitID;
            private string _ParamedicName;
            private bool _IsConfirm;
            private bool _IsParamedicDummy;
            private decimal _Quantity;
            private decimal _DiscountAmount;
            private decimal _DiscountAmount1;
            private decimal _DiscountAmount2;
            private decimal _DiscountAmount3;
            private string _GCItemUnit;

            public MCUOrder(int detailItemID, int paramedicID, string paramedicName, bool isConfirm)
            {
                _DetailItemID = detailItemID;
                _ParamedicID = paramedicID;
                _ParamedicName = paramedicName;
                _IsConfirm = isConfirm;
            }

            public int Key
            {
                get { return _Key; }
                set { _Key = value; }
            }
            public int VisitID
            {
                get { return _VisitID; }
                set { _VisitID = value; }
            }
            public int ItemID
            {
                get { return _ItemID; }
                set { _ItemID = value; }
            }
            public int DetailItemID
            {
                get { return _DetailItemID; }
                set { _DetailItemID = value; }
            }

            public string DetailItemCode
            {
                get { return _DetailItemCode; }
                set { _DetailItemCode = value; }
            }

            public string DetailItemName1
            {
                get { return _DetailItemName1; }
                set { _DetailItemName1 = value; }
            }

            public string DepartmentID
            {
                get { return _DepartmentID; }
                set { _DepartmentID = value; }
            }

            public string ServiceUnitName
            {
                get { return _ServiceUnitName; }
                set { _ServiceUnitName = value; }
            }

            public string DepartmentName
            {
                get { return _DepartmentName; }
                set { _DepartmentName = value; }
            }

            public int ParamedicID
            {
                get { return _ParamedicID; }
                set { _ParamedicID = value; }
            }

            public int HealthcareServiceUnitID
            {
                get { return _HealthcareServiceUnitID; }
                set { _HealthcareServiceUnitID = value; }
            }

            public string ParamedicName
            {
                get { return _ParamedicName; }
                set { _ParamedicName = value; }
            }

            public bool IsConfirm
            {
                get { return _IsConfirm; }
                set { _IsConfirm = value; }
            }

            public bool IsParamedicDummy
            {
                get { return _IsParamedicDummy; }
                set { _IsParamedicDummy = value; }
            }
            public decimal Quantity
            {
                get { return _Quantity; }
                set { _Quantity = value; }
            }

            public decimal DiscountAmount
            {
                get { return _DiscountAmount; }
                set { _DiscountAmount = value; }
            }

            public decimal DiscountAmount1
            {
                get { return _DiscountAmount1; }
                set { _DiscountAmount1 = value; }
            }

            public decimal DiscountAmount2
            {
                get { return _DiscountAmount2; }
                set { _DiscountAmount2 = value; }
            }

            public decimal DiscountAmount3
            {
                get { return _DiscountAmount3; }
                set { _DiscountAmount3 = value; }
            }

            public string GCItemUnit
            {
                get { return _GCItemUnit; }
                set { _GCItemUnit = value; }
            }
        }

        public List<MCUOrder> MCUOrderGroupList
        {
            get
            {
                if (Session["__MCUOrderGroupList"] == null)
                    Session["__MCUOrderGroupList"] = new List<MCUOrder>();

                return (List<MCUOrder>)Session["__MCUOrderGroupList"];
            }
            set { Session["__MCUOrderGroupList"] = value; }
        }

        public class MCUOrderLast
        {
            private int _Key;
            private int _VisitID;
            private int _ItemID;
            private int _DetailItemID;
            private string _DetailItemCode;
            private string _DetailItemName1;
            private string _ServiceUnitName;
            private string _DepartmentID;
            private string _DepartmentName;
            private int _ParamedicID;
            private int _HealthcareServiceUnitID;
            private string _ParamedicName;
            private bool _IsConfirm;
            private bool _IsParamedicDummy;
            private decimal _Quantity;
            private decimal _DiscountAmount;
            private decimal _DiscountAmount1;
            private decimal _DiscountAmount2;
            private decimal _DiscountAmount3;
            private string _GCItemUnit;

            public MCUOrderLast(int detailItemID, int paramedicID, string paramedicName, bool isConfirm)
            {
                _DetailItemID = detailItemID;
                _ParamedicID = paramedicID;
                _ParamedicName = paramedicName;
                _IsConfirm = isConfirm;
            }

            public int Key
            {
                get { return _Key; }
                set { _Key = value; }
            }
            public int VisitID
            {
                get { return _VisitID; }
                set { _VisitID = value; }
            }
            public int ItemID
            {
                get { return _ItemID; }
                set { _ItemID = value; }
            }
            public int DetailItemID
            {
                get { return _DetailItemID; }
                set { _DetailItemID = value; }
            }

            public string DetailItemCode
            {
                get { return _DetailItemCode; }
                set { _DetailItemCode = value; }
            }

            public string DetailItemName1
            {
                get { return _DetailItemName1; }
                set { _DetailItemName1 = value; }
            }

            public string DepartmentID
            {
                get { return _DepartmentID; }
                set { _DepartmentID = value; }
            }

            public string ServiceUnitName
            {
                get { return _ServiceUnitName; }
                set { _ServiceUnitName = value; }
            }

            public string DepartmentName
            {
                get { return _DepartmentName; }
                set { _DepartmentName = value; }
            }

            public int ParamedicID
            {
                get { return _ParamedicID; }
                set { _ParamedicID = value; }
            }

            public int HealthcareServiceUnitID
            {
                get { return _HealthcareServiceUnitID; }
                set { _HealthcareServiceUnitID = value; }
            }

            public string ParamedicName
            {
                get { return _ParamedicName; }
                set { _ParamedicName = value; }
            }

            public bool IsConfirm
            {
                get { return _IsConfirm; }
                set { _IsConfirm = value; }
            }

            public bool IsParamedicDummy
            {
                get { return _IsParamedicDummy; }
                set { _IsParamedicDummy = value; }
            }
            public decimal Quantity
            {
                get { return _Quantity; }
                set { _Quantity = value; }
            }

            public decimal DiscountAmount
            {
                get { return _DiscountAmount; }
                set { _DiscountAmount = value; }
            }

            public decimal DiscountAmount1
            {
                get { return _DiscountAmount1; }
                set { _DiscountAmount1 = value; }
            }

            public decimal DiscountAmount2
            {
                get { return _DiscountAmount2; }
                set { _DiscountAmount2 = value; }
            }

            public decimal DiscountAmount3
            {
                get { return _DiscountAmount3; }
                set { _DiscountAmount3 = value; }
            }

            public string GCItemUnit
            {
                get { return _GCItemUnit; }
                set { _GCItemUnit = value; }
            }
        }

        public List<MCUOrderLast> MCUOrderGroupListLast
        {
            get
            {
                if (Session["__MCUOrderGroupListLast"] == null)
                    Session["__MCUOrderGroupListLast"] = new List<MCUOrderLast>();

                return (List<MCUOrderLast>)Session["__MCUOrderGroupListLast"];
            }
            set { Session["__MCUOrderGroupListLast"] = value; }
        }
        #endregion
        public override void InitializeDataControl(string Param)
        {
            this.PopupTitle = "Generate Order Kelompok MCU";


            List<vSettingParameterDt> lstSettingParameter = BusinessLayer.GetvSettingParameterDtList(string.Format("ParameterCode IN ('{0}','{1}','{2}','{3}','{4}') AND HealthcareID = '{5}'",
                                                                                                    Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI,
                                                                                                    Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM,
                                                                                                    Constant.SettingParameter.MC_KODE_DEFAULT_DOKTER,
                                                                                                    Constant.SettingParameter.FN_ITEM_ID_FOR_MCU_PACKAGE_COST,
                                                                                                    Constant.SettingParameter.MC_IS_USING_REGISTRATION_PARAMEDICID,
                                                                                                    AppSession.UserLogin.HealthcareID));

            hdnDefaultItemIDMCUPackage.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_ITEM_ID_FOR_MCU_PACKAGE_COST).ParameterValue;
            hdnImagingServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).ParameterValue;
            hdnLaboratoryServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).ParameterValue;
            hdnIsUsingRegistrationParamedicID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.MC_IS_USING_REGISTRATION_PARAMEDICID).ParameterValue;

            ///hdnRegistrationParamedicID.Value = entity.ParamedicID.ToString();
            defaultParamedicID = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.MC_KODE_DEFAULT_DOKTER).ParameterValue;
            hdnDefaultParamedicID.Value = defaultParamedicID;
            defaultParamedicName = BusinessLayer.GetParamedicMaster(Convert.ToInt32(defaultParamedicID)).FullName;
            hdnDefaultParamedicName.Value = defaultParamedicName;

            MCUOrderGroupList.Clear();
            MCUOrderGroupListLast.Clear();

            BindGridView();
        }

        private void cboHealthcareServiceUnit()
        {
            List<MCUOrder> lstData = new List<MCUOrder>();
            lstData = MCUOrderGroupList.GroupBy(x => x.HealthcareServiceUnitID).Select(g => g.FirstOrDefault()).ToList();
            List<vHealthcareServiceUnit> lstHc = new List<vHealthcareServiceUnit>();
            string hsuID = string.Empty;
            if (lstData.Count > 0)
            {
                foreach (MCUOrder i in lstData)
                {
                    vHealthcareServiceUnit row = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID ='{0}' AND IsDeleted=0", i.HealthcareServiceUnitID)).FirstOrDefault();
                    if (row != null)
                    {
                        // lstHc.Add(row);
                        hsuID += string.Format("{0},", row.HealthcareServiceUnitID);
                    }
                }

                if (!string.IsNullOrEmpty(hsuID))
                {
                    hsuID = hsuID.Remove(hsuID.Length - 1);
                    hdnlstHealthcareServiceUnitID.Value = hsuID;

                }

                // lstHc.OrderBy(p => p.ServiceUnitName);
            }



            // Methods.SetComboBoxField<vHealthcareServiceUnit>(cboServiceUnit, lstHc, "ServiceUnitName", "HealthcareServiceUnitID");
            // cboServiceUnit.SelectedIndex = 0;
        }
        private void BindGridView()
        {

            string filterExpression = string.Format("RequestBatchNo = '{0}' AND RegistrationID IS NOT NULL AND GCRegistrationStatus NOT IN('{1}') AND DepartmentID = '{2}' AND GCVisitStatus IN ('{3}','{4}') AND  VisitID IN (SELECT VisitID FROM ConsultVisitItemPackage WHERE GCItemDetailStatus = '{5}') ",
                hdnBatchNo.Value,
                Constant.VisitStatus.CANCELLED,
                Constant.Facility.MEDICAL_CHECKUP,
                Constant.VisitStatus.CHECKED_IN,
                Constant.VisitStatus.RECEIVING_TREATMENT,
                Constant.TransactionStatus.OPEN);

            List<vAppointmentRequest> lstEntity = BusinessLayer.GetvAppointmentRequestList(filterExpression);

            InitialMCUOrder(lstEntity);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
        private void InitialMCUOrder(List<vAppointmentRequest> lstApmRequest)
        {
            MCUOrderGroupList.Clear();

            int counter = 0;
            //int savedCounter = 0;
            if (MCUOrderGroupListLast.Count == 0)
            {
                foreach (vAppointmentRequest row in lstApmRequest)
                {
                    string filterExpression = string.Format("VisitID = '{0}' AND IsDeleted = 0 AND GCItemDetailStatus = '{1}'", row.VisitID, Constant.TransactionStatus.OPEN);
                    List<ConsultVisitItemPackage> lstEntityItemPackage = BusinessLayer.GetConsultVisitItemPackageList(filterExpression);

                    string lstItemPackageID = string.Join(",", lstEntityItemPackage.Select(x => x.ItemID));

                    filterExpression = string.Format("ItemID IN ({0}) AND IsDeleted = 0 ORDER BY ItemID ASC", lstItemPackageID);
                    List<vItemServiceDt> lstEntity = BusinessLayer.GetvItemServiceDtList(filterExpression);
                    List<vItemServiceDt> lstHSU = lstEntity.GroupBy(x => x.HealthcareServiceUnitID).Select(x => x.First()).ToList();

                    SaveMCUOrderSessionState(lstEntity, row.VisitID, ref counter);
                    //savedCounter = counter;
                }
            }
            else
            {
                MCUOrderGroupList = SetMCUOrderSession(MCUOrderGroupListLast);
            }
        }
        protected void cbpCboView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {

            string result = "";
            string errMessage = string.Empty;
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "update")
                {
                    if (UpdateParamedicMCU(ref errMessage))
                    {
                        result = string.Format("update|success|{0}", errMessage);
                    }
                    else
                    {
                        result = string.Format("update|failed|{0}", errMessage);
                    }


                }
                else
                {
                    cboHealthcareServiceUnit();
                }
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        public Boolean UpdateParamedicMCU(ref string errMessage)
        {

            Boolean result = true;

            if (hdnHealthcareServiceUnitID.Value != null && !string.IsNullOrEmpty(hdnParamedicID.Value))
            {
                if (MCUOrderGroupList.Count > 0)
                {
                    List<MCUOrder> lstDataOrder = MCUOrderGroupList.Where(p => p.HealthcareServiceUnitID == Convert.ToInt32(hdnHealthcareServiceUnitID.Value.ToString())).ToList();

                    foreach (MCUOrder row in lstDataOrder)
                    {
                        MCUOrder oMCU = MCUOrderGroupList.Where(p => p.Key == row.Key).FirstOrDefault();
                        if (oMCU != null)
                        {
                            vParamedicMaster oParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("ParamedicID IN ({0})", hdnParamedicID.Value)).FirstOrDefault();
                            if (oParamedic != null)
                            {

                                row.ParamedicID = oParamedic.ParamedicID;
                                row.ParamedicName = oParamedic.ParamedicName;
                                row.IsParamedicDummy = oParamedic.IsDummy;
                                MCUOrderGroupList.Remove(row);
                                MCUOrderGroupList.Add(row);
                            }
                        }

                    }
                }


            }
            else
            {

                result = false;
                errMessage = string.Format("Silahkan di isi dahulu service unit dan dokter pelaksana");
            }



            return result;
        }
        protected void cbpGeneratOrderView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView();
                    result = "changepage";
                }
                else if (param[0] == "GenerateOrder")
                {
                    GenerateOrder(ref result);

                }
                else if (param[0] == "changebatch")
                {
                    MCUOrderGroupList.Clear();
                    MCUOrderGroupListLast.Clear();

                    BindGridView();
                    result = "refresh|" + pageCount;
                }
                else // refresh
                {
                    if (MCUOrderGroupList.Count > 0)
                    {
                        MCUOrderGroupListLast = SaveLastMCUOrderSession(MCUOrderGroupList);
                    }
                    BindGridView();
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpGeneratOrderDtView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt();
                    result = "changepage";
                }
                if (param[0] == "changeParamedic")
                {
                    UpdateMCUOrderSessionState();
                    BindGridViewDt();
                    result = "changeParamedic";
                }
                else // refresh
                {

                    BindGridViewDt();
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        private void BindGridViewDt()
        {
            string filterExpression = string.Format("VisitID = '{0}' AND IsDeleted = 0 AND GCItemDetailStatus = '{1}'", hdnVisitID.Value, Constant.TransactionStatus.OPEN);
            List<ConsultVisitItemPackage> lstEntityItemPackage = BusinessLayer.GetConsultVisitItemPackageList(filterExpression);
            string lstItemPackageID = string.Join(",", lstEntityItemPackage.Select(x => x.ItemID));

            filterExpression = string.Format("ItemID IN ({0}) AND IsDeleted = 0 ORDER BY ItemID ASC", lstItemPackageID);
            List<MCUOrder> lstEntity = MCUOrderGroupList.Where(p => p.VisitID.ToString() == hdnVisitID.Value).OrderBy(p => p.Key).ToList(); //.ToList();
            grdViewDt.DataSource = lstEntity;
            grdViewDt.DataBind();
        }
        protected void grdViewDt_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                MCUOrder entity = (MCUOrder)e.Row.DataItem;

                //int paramedicID = Convert.ToInt32(hdnDefaultParamedicID.Value);
                //string paramedicName = hdnDefaultParamedicName.Value;

                //if (hdnIsUsingRegistrationParamedicID.Value == "0")
                //{
                //    if (entity.ParamedicID != 0)
                //    {
                //        paramedicID = entity.ParamedicID;
                //        paramedicName = entity.ParamedicName;
                //    }
                //}
                //else
                //{
                //    ParamedicMaster entityParamedic = BusinessLayer.GetParamedicMaster(Convert.ToInt32(hdnDefaultRegistrationParamedicID.Value));
                //    paramedicID = entityParamedic.ParamedicID;
                //    paramedicName = entityParamedic.FullName;
                //}

                HtmlInputHidden hdnKey = (HtmlInputHidden)e.Row.FindControl("hdnKey");
                HtmlInputHidden hdnParamedicID = (HtmlInputHidden)e.Row.FindControl("hdnParamedicID");
                HtmlInputHidden hdnDetailItemID = (HtmlInputHidden)e.Row.FindControl("hdnDetailItemID");
                HtmlGenericControl lblParamedicName = (HtmlGenericControl)e.Row.FindControl("lblParamedicName");

                hdnKey.Value = entity.Key.ToString();
                lblParamedicName.InnerText = entity.ParamedicName;
                hdnParamedicID.Value = entity.ParamedicID.ToString();
                hdnDetailItemID.Value = entity.DetailItemID.ToString();
            }
        }

        private void SaveMCUOrderSessionState(List<vItemServiceDt> lstEntity, int VisitID, ref int counter)
        {
            int a = VisitID;
            foreach (vItemServiceDt entity in lstEntity)
            {
                counter += 1;
                //int paramedicID = Convert.ToInt32(defaultParamedicID);
                //string paramedicName = defaultParamedicName;
                //if (entity.ParamedicID != 0)
                //{
                //    paramedicID = entity.ParamedicID;
                //    paramedicName = entity.ParamedicName;
                //}

                int paramedicID = Convert.ToInt32(hdnDefaultParamedicID.Value);
                ParamedicMaster Defaultparamedic = BusinessLayer.GetParamedicMaster(paramedicID);
                string paramedicName = hdnDefaultParamedicName.Value;
                bool isDummy = Defaultparamedic.IsDummy;

                if (hdnIsUsingRegistrationParamedicID.Value == "0")
                {
                    if (entity.ParamedicID != 0)
                    {
                        ParamedicMaster oParamedic = BusinessLayer.GetParamedicMaster(entity.ParamedicID);
                        paramedicID = entity.ParamedicID;
                        paramedicName = entity.ParamedicName;
                        isDummy = oParamedic.IsDummy;
                    }

                }
                else
                {
                    ConsultVisit consultVisit = BusinessLayer.GetConsultVisit(VisitID);
                    ParamedicMaster entityParamedic = BusinessLayer.GetParamedicMaster(Convert.ToInt32(consultVisit.ParamedicID));
                    paramedicID = entityParamedic.ParamedicID;
                    paramedicName = entityParamedic.FullName;
                    isDummy = entityParamedic.IsDummy;
                }

                MCUOrder eMCUOrder = new MCUOrder(entity.DetailItemID, Convert.ToInt32(paramedicID), paramedicName, true);
                //eMCUOrder.Key = a;
                eMCUOrder.Key = counter; //counter
                eMCUOrder.VisitID = a;
                eMCUOrder.ItemID = entity.ItemID;
                eMCUOrder.DetailItemID = entity.DetailItemID;
                eMCUOrder.DetailItemCode = entity.DetailItemCode;
                eMCUOrder.DetailItemName1 = entity.DetailItemName1;
                eMCUOrder.ServiceUnitName = entity.ServiceUnitName;
                eMCUOrder.DepartmentID = entity.DepartmentID;
                eMCUOrder.DepartmentName = entity.DepartmentName;
                eMCUOrder.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
                eMCUOrder.Quantity = entity.Quantity;
                eMCUOrder.GCItemUnit = entity.GCItemUnit;
                eMCUOrder.DiscountAmount = entity.DiscountAmount;
                eMCUOrder.DiscountAmount1 = entity.DiscountComp1;
                eMCUOrder.DiscountAmount2 = entity.DiscountComp2;
                eMCUOrder.DiscountAmount3 = entity.DiscountComp3;
                eMCUOrder.IsParamedicDummy = isDummy;
                MCUOrderGroupList.Add(eMCUOrder);
                //a++;
            }
        }

        private List<MCUOrderLast> SaveLastMCUOrderSession(List<MCUOrder> lstObj)
        {
            List<MCUOrderLast> lstEntity = new List<MCUOrderLast>();
            foreach (MCUOrder entity in lstObj)
            {
                MCUOrderLast eMCUOrder = new MCUOrderLast(entity.DetailItemID, entity.ParamedicID, entity.ParamedicName, true);
                eMCUOrder.Key = entity.Key; //counter
                eMCUOrder.VisitID = entity.VisitID;
                eMCUOrder.ItemID = entity.ItemID;
                eMCUOrder.DetailItemID = entity.DetailItemID;
                eMCUOrder.DetailItemCode = entity.DetailItemCode;
                eMCUOrder.DetailItemName1 = entity.DetailItemName1;
                eMCUOrder.ServiceUnitName = entity.ServiceUnitName;
                eMCUOrder.DepartmentID = entity.DepartmentID;
                eMCUOrder.DepartmentName = entity.DepartmentName;
                eMCUOrder.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
                eMCUOrder.Quantity = entity.Quantity;
                eMCUOrder.GCItemUnit = entity.GCItemUnit;
                eMCUOrder.DiscountAmount = entity.DiscountAmount;
                eMCUOrder.DiscountAmount1 = entity.DiscountAmount1;
                eMCUOrder.DiscountAmount2 = entity.DiscountAmount2;
                eMCUOrder.DiscountAmount3 = entity.DiscountAmount3;
                eMCUOrder.IsParamedicDummy = entity.IsParamedicDummy;
                lstEntity.Add(eMCUOrder);
            }

            return lstEntity;
        }

        private List<MCUOrder> SetMCUOrderSession(List<MCUOrderLast> lstObj)
        {
            List<MCUOrder> lstEntity = new List<MCUOrder>();
            foreach (MCUOrderLast entity in lstObj)
            {
                MCUOrder eMCUOrder = new MCUOrder(entity.DetailItemID, entity.ParamedicID, entity.ParamedicName, true);
                eMCUOrder.Key = entity.Key; //counter
                eMCUOrder.VisitID = entity.VisitID;
                eMCUOrder.ItemID = entity.ItemID;
                eMCUOrder.DetailItemID = entity.DetailItemID;
                eMCUOrder.DetailItemCode = entity.DetailItemCode;
                eMCUOrder.DetailItemName1 = entity.DetailItemName1;
                eMCUOrder.ServiceUnitName = entity.ServiceUnitName;
                eMCUOrder.DepartmentID = entity.DepartmentID;
                eMCUOrder.DepartmentName = entity.DepartmentName;
                eMCUOrder.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
                eMCUOrder.Quantity = entity.Quantity;
                eMCUOrder.GCItemUnit = entity.GCItemUnit;
                eMCUOrder.DiscountAmount = entity.DiscountAmount;
                eMCUOrder.DiscountAmount1 = entity.DiscountAmount1;
                eMCUOrder.DiscountAmount2 = entity.DiscountAmount2;
                eMCUOrder.DiscountAmount3 = entity.DiscountAmount3;
                eMCUOrder.IsParamedicDummy = entity.IsParamedicDummy;
                lstEntity.Add(eMCUOrder);
            }

            return lstEntity;
        }

        private void UpdateMCUOrderSessionState()
        {
            MCUOrder oData = MCUOrderGroupList.Where(p => p.Key.ToString() == hdnKeyDtSession.Value).FirstOrDefault();
            if (oData != null)
            {
                vParamedicMaster Paramedic = BusinessLayer.GetvParamedicMasterList(string.Format("ParamedicID IN ({0})", hdnParamedicIDDtSession.Value)).FirstOrDefault();
                // oData.DetailItemID = Convert.ToInt32(listDetailItemID[a]);
                oData.ParamedicID = Convert.ToInt32(hdnParamedicIDDtSession.Value);
                oData.ParamedicName = Paramedic.ParamedicName;
                oData.IsParamedicDummy = Paramedic.IsDummy;
                MCUOrderGroupList.Remove(oData);
                MCUOrderGroupList.Add(oData);
            }

        }
        //private void UpdateMCUOrderSessionState()
        //{
        //    String[] listKey = hdnListKey.Value.Split('|');
        //    String[] listDetailItemID = hdnListDetailItemID.Value.Split('|');
        //    String[] listParamedicID = hdnListParamedicID.Value.Split('|');
        //    String[] listIsChecked = hdnListIsChecked.Value.Split('|');
        //    string paramParamedic = hdnListParamedicID.Value.Replace('|', ',');
        //    List<ParamedicMaster> lstParamedic = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID IN ({0})", paramParamedic));

        //    for (int a = 0; a < listKey.Length; a++)
        //    {
        //        foreach (MCUOrder entity in MCUOrderGroupList)
        //        {
        //            int b = entity.VisitID;
        //            if (entity.Key == Convert.ToInt32(listKey[a]))
        //            {
        //                entity.DetailItemID = Convert.ToInt32(listDetailItemID[a]);
        //                entity.ParamedicID = Convert.ToInt32(listParamedicID[a]);
        //                entity.ParamedicName = lstParamedic.Where(t => t.ParamedicID == Convert.ToInt32(listParamedicID[a])).FirstOrDefault().FullName;
        //                entity.IsConfirm = listIsChecked[a] == "1" ? true : false;
        //            }
        //        }
        //    }
        //}

        #region Generate Proses
        private void InsertMCUDefaultCharges(List<ConsultVisitItemPackage> lstEntityItemPackage, ConsultVisit entityVisit, IDbContext ctx)
        {
            ItemServiceDao itemServiceDao = new ItemServiceDao(ctx);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao patientChargesDtDao = new PatientChargesDtDao(ctx);

            List<PatientChargesDt> lstPatientChargesDt = new List<PatientChargesDt>();
            foreach (ConsultVisitItemPackage entity in lstEntityItemPackage)
            {
                ItemService entityItemServicePackage = itemServiceDao.Get(entity.ItemID);
                List<GetCurrentItemTariff> listPackage = BusinessLayer.GetCurrentItemTariff(entityVisit.RegistrationID, entityVisit.VisitID, Convert.ToInt32(entityVisit.ChargeClassID), entity.ItemID, 1, DateTime.Now, ctx);
                decimal itemPackagePrice = listPackage.FirstOrDefault().Price;
                decimal itemPackagePriceComp1 = listPackage.FirstOrDefault().PriceComp1;
                decimal itemPackagePriceComp2 = listPackage.FirstOrDefault().PriceComp2;
                decimal itemPackagePriceComp3 = listPackage.FirstOrDefault().PriceComp3;

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                string filterPCD = string.Format("ItemPackageID = {0} AND TransactionID IN (SELECT TransactionID FROM PatientChargesHd WHERE VisitID = {1} AND GCTransactionStatus != '{2}') AND IsDeleted = 0",
                                    entity.ItemID, entityVisit.VisitID, Constant.TransactionStatus.VOID);
                List<PatientChargesDt> lstPCD = BusinessLayer.GetPatientChargesDtList(filterPCD, ctx);
                decimal totalChargesDTBaseTariff = lstPCD.Sum(a => a.BaseTariff);
                decimal totalChargesDTBaseComp1 = lstPCD.Sum(a => a.BaseComp1);
                decimal totalChargesDTBaseComp2 = lstPCD.Sum(a => a.BaseComp2);
                decimal totalChargesDTBaseComp3 = lstPCD.Sum(a => a.BaseComp3);
                decimal totalChargesDTTariff = lstPCD.Sum(a => a.Tariff);
                decimal totalChargesDTTariffComp1 = lstPCD.Sum(a => a.TariffComp1);
                decimal totalChargesDTTariffComp2 = lstPCD.Sum(a => a.TariffComp2);
                decimal totalChargesDTTariffComp3 = lstPCD.Sum(a => a.TariffComp3);

                if (!entityItemServicePackage.IsUsingAccumulatedPrice)
                {
                    PatientChargesDt patientChargesDt = new PatientChargesDt();
                    patientChargesDt.ItemID = Convert.ToInt32(hdnDefaultItemIDMCUPackage.Value);
                    patientChargesDt.ChargeClassID = Convert.ToInt32(entityVisit.ChargeClassID);
                    patientChargesDt.ItemPackageID = entity.ItemID;
                    patientChargesDt.ReferenceDtID = entity.ItemID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();

                    List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(entityVisit.RegistrationID, entityVisit.VisitID, patientChargesDt.ChargeClassID, entity.ItemID, 1, DateTime.Now, ctx);
                    decimal discountAmount = 0;
                    decimal coverageAmount = 0;
                    decimal price = 0;
                    decimal basePrice = 0;
                    bool isCoverageInPercentage = false;
                    bool isDiscountInPercentage = false;
                    if (list.Count > 0)
                    {
                        GetCurrentItemTariff obj = list[0];
                        discountAmount = obj.DiscountAmount;
                        coverageAmount = obj.CoverageAmount;
                        price = obj.Price;
                        basePrice = obj.BasePrice;
                        isCoverageInPercentage = obj.IsCoverageInPercentage;
                        isDiscountInPercentage = obj.IsDiscountInPercentage;
                    }
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();

                    ItemMaster entityItemMaster = BusinessLayer.GetItemMasterList(string.Format("ItemID = {0}", patientChargesDt.ItemID), ctx).FirstOrDefault();
                    patientChargesDt.GCBaseUnit = patientChargesDt.GCItemUnit = entityItemMaster.GCItemUnit;
                    patientChargesDt.ParamedicID = Convert.ToInt32(entityVisit.ParamedicID);
                    patientChargesDt.RevenueSharingID = BusinessLayer.GetItemRevenueSharing(entityItemMaster.ItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, entityVisit.VisitID, entityVisit.HealthcareServiceUnitID, DateTime.Now, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)).FirstOrDefault().RevenueSharingID;
                    if (patientChargesDt.RevenueSharingID == 0)
                        patientChargesDt.RevenueSharingID = null;
                    patientChargesDt.IsVariable = false;
                    patientChargesDt.IsUnbilledItem = false;
                    patientChargesDt.IsCITO = false;
                    patientChargesDt.CITOAmount = 0;

                    patientChargesDt.IsComplication = false;
                    patientChargesDt.ComplicationAmount = 0;
                    patientChargesDt.IsDiscount = false;
                    patientChargesDt.UsedQuantity = patientChargesDt.BaseQuantity = patientChargesDt.ChargedQuantity = 1;
                    patientChargesDt.IsCreatedBySystem = true;
                    patientChargesDt.CreatedBy = patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;

                    patientChargesDt.BaseTariff = itemPackagePrice - totalChargesDTBaseTariff;
                    patientChargesDt.BaseComp1 = itemPackagePriceComp1 - totalChargesDTBaseComp1;
                    patientChargesDt.BaseComp2 = itemPackagePriceComp2 - totalChargesDTBaseComp2;
                    patientChargesDt.BaseComp3 = itemPackagePriceComp3 - totalChargesDTBaseComp3;

                    patientChargesDt.Tariff = itemPackagePrice - totalChargesDTTariff;
                    patientChargesDt.TariffComp1 = itemPackagePriceComp1 - totalChargesDTTariffComp1;
                    patientChargesDt.TariffComp2 = itemPackagePriceComp2 - totalChargesDTTariffComp2;
                    patientChargesDt.TariffComp3 = itemPackagePriceComp3 - totalChargesDTTariffComp3;

                    decimal total = patientChargesDt.Tariff;
                    decimal totalPayer = 0;
                    if (isCoverageInPercentage)
                        totalPayer = total * coverageAmount / 100;
                    else
                        totalPayer = coverageAmount * 1;

                    if (total == 0)
                    {
                        totalPayer = total;
                    }
                    else
                    {
                        if (totalPayer < 0 && totalPayer < total)
                        {
                            totalPayer = total;
                        }
                        else if (totalPayer > 0 & totalPayer > total)
                        {
                            totalPayer = total;
                        }
                    }

                    patientChargesDt.PatientAmount = total - totalPayer;
                    patientChargesDt.PayerAmount = totalPayer;
                    patientChargesDt.LineAmount = total;

                    lstPatientChargesDt.Add(patientChargesDt);
                }
            }

            if (lstPatientChargesDt.Count > 0)
            {
                PatientChargesHd patientChargesHd = BusinessLayer.GetPatientChargesHdList(string.Format("VisitID = {0} AND IsAutoTransaction = 1 AND HealthcareServiceUnitID = {1} AND GCTransactionStatus <> '{2}'", entityVisit.VisitID, entityVisit.HealthcareServiceUnitID, Constant.TransactionStatus.VOID), ctx).FirstOrDefault();
                if (patientChargesHd == null)
                {
                    patientChargesHd = new PatientChargesHd();
                    patientChargesHd.VisitID = entityVisit.VisitID;
                    patientChargesHd.HealthcareServiceUnitID = entityVisit.HealthcareServiceUnitID;
                    patientChargesHd.TransactionCode = Constant.TransactionCode.MCU_CHARGES;
                    patientChargesHd.TransactionDate = DateTime.Now;
                    patientChargesHd.TransactionTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                    patientChargesHd.PatientBillingID = null;
                    patientChargesHd.ReferenceNo = "";
                    patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                    patientChargesHd.GCVoidReason = null;
                    patientChargesHd.IsAutoTransaction = true;
                    patientChargesHd.TransactionNo = BusinessLayer.GenerateTransactionNo(patientChargesHd.TransactionCode, patientChargesHd.TransactionDate, ctx);
                    patientChargesHd.CreatedBy = 0;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    patientChargesHd.TransactionID = patientChargesHdDao.InsertReturnPrimaryKeyID(patientChargesHd);
                }

                foreach (PatientChargesDt entityPatientChargesDt in lstPatientChargesDt)
                {
                    entityPatientChargesDt.TransactionID = patientChargesHd.TransactionID;
                    entityPatientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.OPEN;
                    patientChargesDtDao.Insert(entityPatientChargesDt);
                }
            }
        }

        #region old
        //private void SaveTestOrderold(IDbContext ctx, ConsultVisit visit, List<vItemServiceDt> lstItemServiceDt, DateTime dateNow, String timeNow)
        //{

        //    TestOrderHdDao entityHdDao = new TestOrderHdDao(ctx);
        //    TestOrderDtDao entityDao = new TestOrderDtDao(ctx);
        //    PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
        //    PatientChargesDtDao patientChargesDtDao = new PatientChargesDtDao(ctx);
        //    PatientChargesDtPackageDao patientChargesDtPackageDao = new PatientChargesDtPackageDao(ctx);

        //    TestOrderHd entityHd = new TestOrderHd();
        //    PatientChargesHd patientChargesHd = new PatientChargesHd();
        //    entityHd.FromHealthcareServiceUnitID = visit.HealthcareServiceUnitID; //Convert.ToInt32(hdnFromHealthcareServiceUnitID.Value);
        //    entityHd.HealthcareServiceUnitID = lstItemServiceDt[0].HealthcareServiceUnitID;
        //    entityHd.ParamedicID = Convert.ToInt32(visit.ParamedicID);
        //    entityHd.VisitID = visit.VisitID;
        //    entityHd.TestOrderDate = dateNow;
        //    entityHd.TestOrderTime = timeNow;
        //    entityHd.ScheduledDate = dateNow;
        //    entityHd.ScheduledTime = timeNow;
        //    if (entityHd.HealthcareServiceUnitID == Convert.ToInt32(hdnImagingServiceUnitID.Value))
        //    {
        //        entityHd.TransactionCode = Constant.TransactionCode.IMAGING_TEST_ORDER;
        //        patientChargesHd.TransactionCode = Constant.TransactionCode.IMAGING_CHARGES;
        //    }
        //    else if (entityHd.HealthcareServiceUnitID == Convert.ToInt32(hdnLaboratoryServiceUnitID.Value))
        //    {
        //        entityHd.TransactionCode = Constant.TransactionCode.LABORATORY_TEST_ORDER;
        //        patientChargesHd.TransactionCode = Constant.TransactionCode.LABORATORY_CHARGES;
        //    }
        //    else
        //    {
        //        entityHd.TransactionCode = Constant.TransactionCode.OTHER_TEST_ORDER;
        //        patientChargesHd.TransactionCode = Constant.TransactionCode.OTHER_DIAGNOSTIC_CHARGES;
        //    }
        //    entityHd.TestOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.TestOrderDate, ctx);
        //    ctx.CommandType = CommandType.Text;
        //    ctx.Command.Parameters.Clear();
        //    entityHd.GCOrderStatus = Constant.OrderStatus.OPEN;
        //    entityHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
        //    entityHd.GCToBePerformed = Constant.ToBePerformed.CURRENT_EPISODE;
        //    entityHd.CreatedBy = AppSession.UserLogin.UserID;
        //    entityHdDao.Insert(entityHd);
        //    entityHd.TestOrderID = BusinessLayer.GetTestOrderHdMaxID(ctx);

        //    #region Patient Charges HD
        //    patientChargesHd.VisitID = visit.VisitID;
        //    patientChargesHd.LinkedChargesID = null;
        //    patientChargesHd.TestOrderID = entityHd.TestOrderID;
        //    patientChargesHd.HealthcareServiceUnitID = lstItemServiceDt[0].HealthcareServiceUnitID;
        //    patientChargesHd.TransactionDate = DateTime.Now;
        //    patientChargesHd.TransactionTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
        //    patientChargesHd.PatientBillingID = null;
        //    patientChargesHd.ReferenceNo = "";
        //    patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;// Constant.TransactionStatus.WAIT_FOR_APPROVAL;
        //    patientChargesHd.ProposedBy = AppSession.UserLogin.UserID;
        //    patientChargesHd.ProposedDate = DateTime.Now;
        //    patientChargesHd.LastUpdatedBy = AppSession.UserLogin.UserID;
        //    patientChargesHd.LastUpdatedDate = DateTime.Now;
        //    patientChargesHd.GCVoidReason = null;
        //    patientChargesHd.TotalPatientAmount = 0;
        //    patientChargesHd.TotalPayerAmount = 0;
        //    patientChargesHd.TotalAmount = 0;
        //    patientChargesHd.IsAutoTransaction = true;
        //    patientChargesHd.TransactionNo = BusinessLayer.GenerateTransactionNo(patientChargesHd.TransactionCode, patientChargesHd.TransactionDate, ctx);
        //    patientChargesHd.CreatedBy = AppSession.UserLogin.UserID;
        //    ctx.CommandType = CommandType.Text;
        //    ctx.Command.Parameters.Clear();
        //    patientChargesHd.TransactionID = patientChargesHdDao.InsertReturnPrimaryKeyID(patientChargesHd);
        //    #endregion

        //    #region Patient Charges DT
        //    foreach (vItemServiceDt itemServiceDt in lstItemServiceDt)
        //    {
        //        int paramedicID = Convert.ToInt32(hdnDefaultParamedicID.Value);
        //        string paramedicName = defaultParamedicName;

        //        if (hdnIsUsingRegistrationParamedicID.Value == "0")
        //        {
        //            if (itemServiceDt.ParamedicID != 0)
        //            {
        //                paramedicID = itemServiceDt.ParamedicID;
        //                //paramedicName = itemServiceDt.ParamedicName;
        //            }
        //        }
        //        else
        //        {
        //            //ParamedicMaster entityParamedic = BusinessLayer.GetParamedicMaster(Convert.ToInt32(hdnRegistrationParamedicID.Value));
        //            paramedicID = Convert.ToInt32( visit.ParamedicID);
        //            //paramedicName = entityParamedic.FullName;
        //        }

        //        TestOrderDt entity = new TestOrderDt();
        //        entity.TestOrderID = entityHd.TestOrderID;
        //        entity.ItemID = itemServiceDt.DetailItemID;
        //        entity.ItemPackageID = itemServiceDt.ItemID;
        //        entity.DiagnoseID = null;
        //        entity.GCTestOrderStatus = Constant.TestOrderStatus.IN_PROGRESS;
        //        entity.ItemQty = itemServiceDt.Quantity;
        //        entity.ItemUnit = itemServiceDt.GCItemUnit;
        //        entity.CreatedBy = AppSession.UserLogin.UserID;
        //        entityDao.Insert(entity);
        //        entity.ID = BusinessLayer.GetTestOrderDtMaxID(ctx);
        //        PatientChargesDt patientChargesDt = new PatientChargesDt();
        //        patientChargesDt.TransactionID = patientChargesHd.TransactionID;
        //        patientChargesDt.ItemID = entity.ItemID;
        //        patientChargesDt.ChargeClassID = Convert.ToInt32(visit.ChargeClassID);
        //        patientChargesDt.ItemPackageID = entity.ItemPackageID;
        //        patientChargesDt.ReferenceDtID = entity.ID;
        //        List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(visit.RegistrationID, visit.VisitID, patientChargesDt.ChargeClassID, entity.ItemID, 1, DateTime.Now, ctx);

        //        decimal basePrice = 0;
        //        decimal basePriceComp1 = 0;
        //        decimal basePriceComp2 = 0;
        //        decimal basePriceComp3 = 0;
        //        decimal price = 0;
        //        decimal priceComp1 = 0;
        //        decimal priceComp2 = 0;
        //        decimal priceComp3 = 0;
        //        bool isDiscountUsedComp = false;
        //        decimal discountAmount = 0;
        //        decimal discountAmountComp1 = 0;
        //        decimal discountAmountComp2 = 0;
        //        decimal discountAmountComp3 = 0;
        //        decimal coverageAmount = 0;
        //        bool isDiscountInPercentage = false;
        //        bool isDiscountInPercentageComp1 = false;
        //        bool isDiscountInPercentageComp2 = false;
        //        bool isDiscountInPercentageComp3 = false;
        //        bool isCoverageInPercentage = false;
        //        decimal costAmount = 0;

        //        decimal totalDiscountAmount = 0;
        //        decimal totalDiscountAmount1 = 0;
        //        decimal totalDiscountAmount2 = 0;
        //        decimal totalDiscountAmount3 = 0;

        //        if (list.Count > 0)
        //        {
        //            GetCurrentItemTariff obj = list[0];
        //            basePrice = obj.BasePrice;
        //            basePriceComp1 = obj.BasePriceComp1;
        //            basePriceComp2 = obj.BasePriceComp2;
        //            basePriceComp3 = obj.BasePriceComp3;
        //            price = obj.Price;
        //            priceComp1 = obj.PriceComp1;
        //            priceComp2 = obj.PriceComp2;
        //            priceComp3 = obj.PriceComp3;
        //            isDiscountUsedComp = obj.IsDiscountUsedComp;
        //            discountAmount = obj.DiscountAmount;
        //            discountAmountComp1 = obj.DiscountAmountComp1;
        //            discountAmountComp2 = obj.DiscountAmountComp2;
        //            discountAmountComp3 = obj.DiscountAmountComp3;
        //            coverageAmount = obj.CoverageAmount;
        //            isDiscountInPercentage = obj.IsDiscountInPercentage;
        //            isDiscountInPercentageComp1 = obj.IsDiscountInPercentageComp1;
        //            isDiscountInPercentageComp2 = obj.IsDiscountInPercentageComp2;
        //            isDiscountInPercentageComp3 = obj.IsDiscountInPercentageComp3;
        //            isCoverageInPercentage = obj.IsCoverageInPercentage;
        //            costAmount = obj.CostAmount;
        //        }

        //        patientChargesDt.Tariff = price - itemServiceDt.DiscountAmount;
        //        patientChargesDt.TariffComp1 = priceComp1 - itemServiceDt.DiscountComp1;
        //        patientChargesDt.TariffComp2 = priceComp2 - itemServiceDt.DiscountComp2;
        //        patientChargesDt.TariffComp3 = priceComp3 - itemServiceDt.DiscountComp3;

        //        patientChargesDt.BaseTariff = basePrice - itemServiceDt.DiscountAmount;
        //        patientChargesDt.BaseComp1 = basePriceComp1 - itemServiceDt.DiscountComp1;
        //        patientChargesDt.BaseComp2 = basePriceComp2 - itemServiceDt.DiscountComp2;
        //        patientChargesDt.BaseComp3 = basePriceComp3 - itemServiceDt.DiscountComp3;

        //        ItemMaster entityItemMaster = BusinessLayer.GetItemMasterList(string.Format("ItemID = {0}", entity.ItemID)).FirstOrDefault();
        //        patientChargesDt.GCBaseUnit = patientChargesDt.GCItemUnit = entityItemMaster.GCItemUnit;
        //        patientChargesDt.ParamedicID = paramedicID; // itemServiceDt.ParamedicID;
        //        patientChargesDt.RevenueSharingID = BusinessLayer.GetItemRevenueSharing(entityItemMaster.ItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, visit.VisitID, patientChargesHd.HealthcareServiceUnitID, patientChargesHd.TransactionDate, patientChargesHd.TransactionTime).FirstOrDefault().RevenueSharingID;
        //        if (patientChargesDt.RevenueSharingID == 0)
        //            patientChargesDt.RevenueSharingID = null;
        //        patientChargesDt.IsVariable = false;
        //        patientChargesDt.IsUnbilledItem = false;
        //        patientChargesDt.UsedQuantity = patientChargesDt.BaseQuantity = patientChargesDt.ChargedQuantity = entity.ItemQty;
        //        patientChargesDt.IsCITO = false;
        //        patientChargesDt.CITOAmount = 0;

        //        // diskon ini sudah tidak dipakai lagi karena untuk harga mcu sudah net 

        //        //decimal totalDiscountAmount = 0;
        //        //if (isDiscountInPercentage)
        //        //{
        //        //    totalDiscountAmount = grossLineAmount * discountAmount / 100;
        //        //}
        //        //else
        //        //{
        //        //    totalDiscountAmount = discountAmount * patientChargesDt.ChargedQuantity;
        //        //}

        //        //if (totalDiscountAmount > grossLineAmount)
        //        //{
        //        //    totalDiscountAmount = grossLineAmount;
        //        //}

        //        //totalDiscountAmount += (itemServiceDt.DiscountAmount * patientChargesDt.ChargedQuantity);

        //        //var tempDiscountTotal = totalDiscountAmount;
        //        //decimal discountComp1 = (itemServiceDt.DiscountAmount1 * patientChargesDt.ChargedQuantity);
        //        //decimal discountComp2 = (itemServiceDt.DiscountAmount2 * patientChargesDt.ChargedQuantity);
        //        //decimal discountComp3 = (itemServiceDt.DiscountAmount3 * patientChargesDt.ChargedQuantity);

        //        //if (discountComp1 > priceComp1)
        //        //{
        //        //    discountComp1 = priceComp1;
        //        //}

        //        //if (discountComp2 > priceComp2)
        //        //{
        //        //    discountComp2 = priceComp2;
        //        //}

        //        //if (discountComp3 > priceComp3)
        //        //{
        //        //    discountComp3 = priceComp3;
        //        //}

        //        patientChargesDt.DiscountAmount = 0;
        //        patientChargesDt.DiscountComp1 = 0;
        //        patientChargesDt.DiscountComp2 = 0;
        //        patientChargesDt.DiscountComp3 = 0;

        //        decimal grossLineAmount = patientChargesDt.ChargedQuantity * patientChargesDt.Tariff;
        //        decimal total = grossLineAmount - 0;
        //        decimal totalPayer = 0;
        //        if (isCoverageInPercentage)
        //            totalPayer = total * coverageAmount / 100;
        //        else
        //            totalPayer = coverageAmount * 1;

        //        if (total == 0)
        //        {
        //            totalPayer = total;
        //        }
        //        else
        //        {
        //            if (totalPayer < 0 && totalPayer < total)
        //            {
        //                totalPayer = total;
        //            }
        //            else if (totalPayer > 0 & totalPayer > total)
        //            {
        //                totalPayer = total;
        //            }
        //        }

        //        patientChargesDt.IsComplication = false;
        //        patientChargesDt.ComplicationAmount = 0;
        //        patientChargesDt.IsDiscount = false;
        //        patientChargesDt.PatientAmount = total - totalPayer;
        //        patientChargesDt.PayerAmount = totalPayer;
        //        patientChargesDt.LineAmount = total;
        //        patientChargesDt.IsCreatedBySystem = true;
        //       // patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
        //        patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.OPEN;
        //        patientChargesDt.CreatedBy = patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
        //        patientChargesDt.CreatedDate = DateTime.Now;
        //        ctx.CommandType = CommandType.Text;
        //        ctx.Command.Parameters.Clear();
        //        lstEntityDt.Add(patientChargesDt);
        //        int ID = patientChargesDtDao.InsertReturnPrimaryKeyID(patientChargesDt);

        //        #region Patient Charges DT Package

        //        string filterPackage = string.Format("ItemID = {0} AND IsDeleted = 0", patientChargesDt.ItemID);
        //        List<vItemServiceDt> isdList = BusinessLayer.GetvItemServiceDtList(filterPackage);
        //        foreach (vItemServiceDt isd in isdList)
        //        {
        //            PatientChargesDtPackage dtpackage = new PatientChargesDtPackage();
        //            dtpackage.PatientChargesDtID = ID;
        //            dtpackage.ItemID = isd.DetailItemID;
        //            dtpackage.ParamedicID = patientChargesDt.ParamedicID;

        //            int revID = BusinessLayer.GetItemRevenueSharing(isd.DetailItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, visit.VisitID, patientChargesHd.HealthcareServiceUnitID, patientChargesHd.TransactionDate, patientChargesHd.TransactionTime, ctx).FirstOrDefault().RevenueSharingID;
        //            if (revID != 0 && revID != null)
        //            {
        //                dtpackage.RevenueSharingID = revID;
        //            }
        //            else
        //            {
        //                dtpackage.RevenueSharingID = null;
        //            }

        //            dtpackage.ChargedQuantity = (isd.Quantity * patientChargesDt.ChargedQuantity);

        //            ctx.CommandType = CommandType.Text;
        //            ctx.Command.Parameters.Clear();
        //            GetCurrentItemTariff tariff = BusinessLayer.GetCurrentItemTariff(visit.RegistrationID , patientChargesHd.VisitID, patientChargesDt.ChargeClassID, isd.DetailItemID, 1, DateTime.Now, ctx).FirstOrDefault();

        //            basePrice = tariff.BasePrice;
        //            basePriceComp1 = tariff.BasePriceComp1;
        //            basePriceComp2 = tariff.BasePriceComp2;
        //            basePriceComp3 = tariff.BasePriceComp3;
        //            price = tariff.Price;
        //            priceComp1 = tariff.PriceComp1;
        //            priceComp2 = tariff.PriceComp2;
        //            priceComp3 = tariff.PriceComp3;
        //            isDiscountUsedComp = tariff.IsDiscountUsedComp;
        //            discountAmount = tariff.DiscountAmount;
        //            discountAmountComp1 = tariff.DiscountAmountComp1;
        //            discountAmountComp2 = tariff.DiscountAmountComp2;
        //            discountAmountComp3 = tariff.DiscountAmountComp3;
        //            coverageAmount = tariff.CoverageAmount;
        //            isDiscountInPercentage = tariff.IsDiscountInPercentage;
        //            isDiscountInPercentageComp1 = tariff.IsDiscountInPercentageComp1;
        //            isDiscountInPercentageComp2 = tariff.IsDiscountInPercentageComp2;
        //            isDiscountInPercentageComp3 = tariff.IsDiscountInPercentageComp3;
        //            isCoverageInPercentage = tariff.IsCoverageInPercentage;
        //            costAmount = tariff.CostAmount;
        //            grossLineAmount = dtpackage.ChargedQuantity * price;

        //            dtpackage.BaseTariff = tariff.BasePrice;
        //            dtpackage.BaseComp1 = tariff.BasePriceComp1;
        //            dtpackage.BaseComp2 = tariff.BasePriceComp2;
        //            dtpackage.BaseComp3 = tariff.BasePriceComp3;
        //            dtpackage.Tariff = tariff.Price;
        //            dtpackage.TariffComp1 = tariff.PriceComp1;
        //            dtpackage.TariffComp2 = tariff.PriceComp2;
        //            dtpackage.TariffComp3 = tariff.PriceComp3;
        //            dtpackage.CostAmount = tariff.CostAmount;

        //            if (isDiscountInPercentage)
        //            {
        //                //totalDiscountAmount = grossLineAmount * discountAmount / 100;

        //                if (isDiscountUsedComp)
        //                {
        //                    if (priceComp1 > 0)
        //                    {
        //                        totalDiscountAmount1 = priceComp1 * discountAmountComp1 / 100;
        //                        dtpackage.DiscountPercentageComp1 = discountAmountComp1;
        //                    }

        //                    if (priceComp2 > 0)
        //                    {
        //                        totalDiscountAmount2 = priceComp2 * discountAmountComp2 / 100;
        //                        dtpackage.DiscountPercentageComp2 = discountAmountComp2;
        //                    }

        //                    if (priceComp3 > 0)
        //                    {
        //                        totalDiscountAmount3 = priceComp3 * discountAmountComp3 / 100;
        //                        dtpackage.DiscountPercentageComp3 = discountAmountComp3;
        //                    }
        //                }
        //                else
        //                {
        //                    if (priceComp1 > 0)
        //                    {
        //                        totalDiscountAmount1 = priceComp1 * discountAmount / 100;
        //                        dtpackage.DiscountPercentageComp1 = discountAmount;
        //                    }

        //                    if (priceComp2 > 0)
        //                    {
        //                        totalDiscountAmount2 = priceComp2 * discountAmount / 100;
        //                        dtpackage.DiscountPercentageComp2 = discountAmount;
        //                    }

        //                    if (priceComp3 > 0)
        //                    {
        //                        totalDiscountAmount3 = priceComp3 * discountAmount / 100;
        //                        dtpackage.DiscountPercentageComp3 = discountAmount;
        //                    }
        //                }

        //                if (totalDiscountAmount1 > 0)
        //                {
        //                    dtpackage.IsDiscountInPercentageComp1 = true;
        //                }

        //                if (totalDiscountAmount2 > 0)
        //                {
        //                    dtpackage.IsDiscountInPercentageComp2 = true;
        //                }

        //                if (totalDiscountAmount3 > 0)
        //                {
        //                    dtpackage.IsDiscountInPercentageComp3 = true;
        //                }
        //            }
        //            else
        //            {
        //                //totalDiscountAmount = discountAmount * 1;

        //                if (isDiscountUsedComp)
        //                {
        //                    if (priceComp1 > 0)
        //                        totalDiscountAmount1 = discountAmountComp1;
        //                    if (priceComp2 > 0)
        //                        totalDiscountAmount2 = discountAmountComp2;
        //                    if (priceComp3 > 0)
        //                        totalDiscountAmount3 = discountAmountComp3;
        //                }
        //                else
        //                {
        //                    if (priceComp1 > 0)
        //                        totalDiscountAmount1 = discountAmount;
        //                    if (priceComp2 > 0)
        //                        totalDiscountAmount2 = discountAmount;
        //                    if (priceComp3 > 0)
        //                        totalDiscountAmount3 = discountAmount;
        //                }
        //            }

        //            totalDiscountAmount = (totalDiscountAmount1 + totalDiscountAmount2 + totalDiscountAmount3) * (dtpackage.ChargedQuantity);

        //            if (grossLineAmount >= 0)
        //            {
        //                if (totalDiscountAmount > grossLineAmount)
        //                {
        //                    totalDiscountAmount = grossLineAmount;
        //                }
        //            }

        //            dtpackage.DiscountAmount = totalDiscountAmount;
        //            dtpackage.DiscountComp1 = totalDiscountAmount1;
        //            dtpackage.DiscountComp2 = totalDiscountAmount2;
        //            dtpackage.DiscountComp3 = totalDiscountAmount3;

        //            ctx.CommandType = CommandType.Text;
        //            ctx.Command.Parameters.Clear();
        //            string filterIP = string.Format("ItemID = {0} AND IsDeleted = 0", isd.DetailItemID);
        //            List<ItemPlanning> iplan = BusinessLayer.GetItemPlanningList(filterIP, ctx);
        //            if (iplan.Count() > 0)
        //            {
        //                dtpackage.AveragePrice = iplan.FirstOrDefault().AveragePrice;
        //            }
        //            else
        //            {
        //                dtpackage.AveragePrice = 0;
        //            }

        //            dtpackage.CreatedBy = AppSession.UserLogin.UserID;

        //            ctx.CommandType = CommandType.Text;
        //            ctx.Command.Parameters.Clear();
        //            patientChargesDtPackageDao.Insert(dtpackage);
        //        }

        //        #endregion

        //    }
        //    #endregion
        //}

        //private void SaveServiceOrderold(IDbContext ctx, ConsultVisit visit, List<vItemServiceDt> lstItemServiceDt, DateTime dateNow, String timeNow)
        //{
        //    //int paramedicID = Convert.ToInt32(hdnDefaultParamedicID.Value);

        //    ServiceOrderHdDao entityHdDao = new ServiceOrderHdDao(ctx);
        //    ServiceOrderDtDao entityDao = new ServiceOrderDtDao(ctx);
        //    PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
        //    PatientChargesDtDao patientChargesDtDao = new PatientChargesDtDao(ctx);
        //    PatientChargesDtPackageDao patientChargesDtPackageDao = new PatientChargesDtPackageDao(ctx);

        //    ServiceOrderHd entityHd = new ServiceOrderHd();
        //    PatientChargesHd patientChargesHd = new PatientChargesHd();
        //    entityHd.HealthcareServiceUnitID = lstItemServiceDt[0].HealthcareServiceUnitID;
        //    entityHd.ParamedicID = Convert.ToInt32(visit.ParamedicID);
        //    entityHd.VisitID = visit.VisitID;
        //    entityHd.ServiceOrderDate = dateNow;
        //    entityHd.ServiceOrderTime = timeNow;
        //    if (lstItemServiceDt[0].DepartmentID == Constant.Facility.EMERGENCY)
        //    {
        //        entityHd.TransactionCode = Constant.TransactionCode.MCU_EMERGENCY_ORDER;
        //        patientChargesHd.TransactionCode = Constant.TransactionCode.ER_CHARGES;
        //    }
        //    else
        //    {
        //        entityHd.TransactionCode = Constant.TransactionCode.MCU_OUTPATIENT_ORDER;
        //        patientChargesHd.TransactionCode = Constant.TransactionCode.OP_CHARGES;
        //    }
        //    entityHd.ServiceOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.ServiceOrderDate, ctx);
        //    ctx.CommandType = CommandType.Text;
        //    ctx.Command.Parameters.Clear();
        //    entityHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
        //    entityHd.CreatedBy = AppSession.UserLogin.UserID;
        //    entityHdDao.Insert(entityHd);
        //    entityHd.ServiceOrderID = BusinessLayer.GetServiceOrderHdMaxID(ctx);

        //    #region Patient Charges HD

        //    patientChargesHd.VisitID = visit.VisitID;
        //    patientChargesHd.LinkedChargesID = null;
        //    patientChargesHd.ServiceOrderID = entityHd.ServiceOrderID;
        //    patientChargesHd.HealthcareServiceUnitID = lstItemServiceDt[0].HealthcareServiceUnitID;
        //    patientChargesHd.TransactionDate = DateTime.Now;
        //    patientChargesHd.TransactionTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
        //    patientChargesHd.PatientBillingID = null;
        //    patientChargesHd.ReferenceNo = "";
        //    patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.OPEN; //Constant.TransactionStatus.WAIT_FOR_APPROVAL;
        //    patientChargesHd.GCVoidReason = null;
        //    patientChargesHd.TotalPatientAmount = 0;
        //    patientChargesHd.TotalPayerAmount = 0;
        //    patientChargesHd.TotalAmount = 0;
        //    patientChargesHd.IsAutoTransaction = true;
        //    patientChargesHd.TransactionNo = BusinessLayer.GenerateTransactionNo(patientChargesHd.TransactionCode, patientChargesHd.TransactionDate, ctx);
        //    patientChargesHd.CreatedBy = AppSession.UserLogin.UserID;
        //    patientChargesHd.LastUpdatedBy = AppSession.UserLogin.UserID;
        //    patientChargesHd.LastUpdatedDate = DateTime.Now;
        //    patientChargesHd.ProposedBy = AppSession.UserLogin.UserID;
        //    patientChargesHd.ProposedDate = DateTime.Now;
        //    ctx.CommandType = CommandType.Text;
        //    ctx.Command.Parameters.Clear();
        //    patientChargesHd.TransactionID = patientChargesHdDao.InsertReturnPrimaryKeyID(patientChargesHd);

        //    #endregion

        //    #region Patient Charges DT
        //    foreach (vItemServiceDt itemServiceDt in lstItemServiceDt)
        //    {
        //        int paramedicID = Convert.ToInt32(hdnDefaultParamedicID.Value);
        //        if (hdnIsUsingRegistrationParamedicID.Value == "0")
        //        {
        //            if (itemServiceDt.ParamedicID != 0)
        //            {
        //                paramedicID = itemServiceDt.ParamedicID;
        //                //paramedicName = itemServiceDt.ParamedicName;
        //            }
        //        }
        //        else
        //        {
        //            //ParamedicMaster entityParamedic = BusinessLayer.GetParamedicMaster(Convert.ToInt32(hdnRegistrationParamedicID.Value));
        //            paramedicID = Convert.ToInt32(visit.ParamedicID);
        //            //paramedicName = entityParamedic.FullName;
        //        }

        //        ServiceOrderDt entity = new ServiceOrderDt();
        //        entity.ServiceOrderID = entityHd.ServiceOrderID;
        //        entity.ItemID = itemServiceDt.DetailItemID;
        //        entity.ItemPackageID = itemServiceDt.ItemID;
        //        entity.ItemQty = itemServiceDt.Quantity;
        //        entity.ItemUnit = itemServiceDt.GCItemUnit;
        //        entity.GCServiceOrderStatus = Constant.TestOrderStatus.IN_PROGRESS;
        //        entity.CreatedBy = AppSession.UserLogin.UserID;
        //        entityDao.Insert(entity);

        //        PatientChargesDt patientChargesDt = new PatientChargesDt();
        //        patientChargesDt.TransactionID = patientChargesHd.TransactionID;
        //        patientChargesDt.ItemID = entity.ItemID;
        //        patientChargesDt.ChargeClassID = Convert.ToInt32(visit.ChargeClassID);
        //        patientChargesDt.ItemPackageID = entity.ItemPackageID;
        //        patientChargesDt.ReferenceDtID = entity.ID;

        //        List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(visit.RegistrationID, visit.VisitID, patientChargesDt.ChargeClassID, entity.ItemID, 1, DateTime.Now, ctx);

        //        decimal basePrice = 0;
        //        decimal basePriceComp1 = 0;
        //        decimal basePriceComp2 = 0;
        //        decimal basePriceComp3 = 0;
        //        decimal price = 0;
        //        decimal priceComp1 = 0;
        //        decimal priceComp2 = 0;
        //        decimal priceComp3 = 0;
        //        bool isDiscountUsedComp = false;
        //        decimal discountAmount = 0;
        //        decimal discountAmountComp1 = 0;
        //        decimal discountAmountComp2 = 0;
        //        decimal discountAmountComp3 = 0;
        //        decimal coverageAmount = 0;
        //        bool isDiscountInPercentage = false;
        //        bool isDiscountInPercentageComp1 = false;
        //        bool isDiscountInPercentageComp2 = false;
        //        bool isDiscountInPercentageComp3 = false;
        //        bool isCoverageInPercentage = false;
        //        decimal costAmount = 0;

        //        if (list.Count > 0)
        //        {
        //            GetCurrentItemTariff obj = list[0];
        //            basePrice = obj.BasePrice;
        //            basePriceComp1 = obj.BasePriceComp1;
        //            basePriceComp2 = obj.BasePriceComp2;
        //            basePriceComp3 = obj.BasePriceComp3;
        //            price = obj.Price;
        //            priceComp1 = obj.PriceComp1;
        //            priceComp2 = obj.PriceComp2;
        //            priceComp3 = obj.PriceComp3;
        //            isDiscountUsedComp = obj.IsDiscountUsedComp;
        //            discountAmount = obj.DiscountAmount;
        //            discountAmountComp1 = obj.DiscountAmountComp1;
        //            discountAmountComp2 = obj.DiscountAmountComp2;
        //            discountAmountComp3 = obj.DiscountAmountComp3;
        //            coverageAmount = obj.CoverageAmount;
        //            isDiscountInPercentage = obj.IsDiscountInPercentage;
        //            isDiscountInPercentageComp1 = obj.IsDiscountInPercentageComp1;
        //            isDiscountInPercentageComp2 = obj.IsDiscountInPercentageComp2;
        //            isDiscountInPercentageComp3 = obj.IsDiscountInPercentageComp3;
        //            isCoverageInPercentage = obj.IsCoverageInPercentage;
        //            costAmount = obj.CostAmount;
        //        }

        //        patientChargesDt.Tariff = price - itemServiceDt.DiscountAmount;
        //        patientChargesDt.TariffComp1 = priceComp1 - itemServiceDt.DiscountComp1;
        //        patientChargesDt.TariffComp2 = priceComp2 - itemServiceDt.DiscountComp2;
        //        patientChargesDt.TariffComp3 = priceComp3 - itemServiceDt.DiscountComp3;

        //        patientChargesDt.BaseTariff = basePrice - itemServiceDt.DiscountAmount;
        //        patientChargesDt.BaseComp1 = basePriceComp1 - itemServiceDt.DiscountComp1;
        //        patientChargesDt.BaseComp2 = basePriceComp2 - itemServiceDt.DiscountComp2;
        //        patientChargesDt.BaseComp3 = basePriceComp3 - itemServiceDt.DiscountComp3;

        //        vItemService entityItemMaster = BusinessLayer.GetvItemServiceList(string.Format("ItemID = {0}", entity.ItemID)).FirstOrDefault();
        //        patientChargesDt.GCBaseUnit = patientChargesDt.GCItemUnit = entityItemMaster.GCItemUnit;
        //        patientChargesDt.ParamedicID = paramedicID;  //itemServiceDt.ParamedicID;
        //        patientChargesDt.RevenueSharingID = BusinessLayer.GetItemRevenueSharing(entityItemMaster.ItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, visit.VisitID, patientChargesHd.HealthcareServiceUnitID, patientChargesHd.TransactionDate, patientChargesHd.TransactionTime).FirstOrDefault().RevenueSharingID;
        //        if (patientChargesDt.RevenueSharingID == 0)
        //            patientChargesDt.RevenueSharingID = null;
        //        patientChargesDt.IsVariable = false;
        //        patientChargesDt.IsUnbilledItem = false;

        //        patientChargesDt.UsedQuantity = patientChargesDt.BaseQuantity = patientChargesDt.ChargedQuantity = entity.ItemQty;

        //        // diskon ini sudah tidak dipakai lagi karena untuk harga mcu sudah net 

        //        //if (isDiscountInPercentage)
        //        //    totalDiscountAmount = grossLineAmount * discountAmount / 100;
        //        //else
        //        //    totalDiscountAmount = discountAmount * patientChargesDt.ChargedQuantity;
        //        //if (totalDiscountAmount > grossLineAmount)
        //        //    totalDiscountAmount = grossLineAmount;
        //        //totalDiscountAmount += (itemServiceDt.DiscountAmount * patientChargesDt.ChargedQuantity);

        //        //decimal totalDiscountAmount = 0;
        //        //var tempDiscountTotal = totalDiscountAmount;
        //        //decimal discountComp1 = (itemServiceDt.DiscountAmount1 * patientChargesDt.ChargedQuantity);
        //        //decimal discountComp2 = (itemServiceDt.DiscountAmount2 * patientChargesDt.ChargedQuantity);
        //        //decimal discountComp3 = (itemServiceDt.DiscountAmount3 * patientChargesDt.ChargedQuantity);

        //        //if (discountComp1 > priceComp1)
        //        //{
        //        //    discountComp1 = priceComp1;
        //        //}

        //        //if (discountComp2 > priceComp2)
        //        //{
        //        //    discountComp2 = priceComp2;
        //        //}

        //        //if (discountComp3 > priceComp3)
        //        //{
        //        //    discountComp3 = priceComp3;
        //        //}

        //        patientChargesDt.DiscountAmount = 0;
        //        patientChargesDt.DiscountComp1 = 0;
        //        patientChargesDt.DiscountComp2 = 0;
        //        patientChargesDt.DiscountComp3 = 0;

        //        decimal grossLineAmount = patientChargesDt.ChargedQuantity * patientChargesDt.Tariff;
        //        decimal total = grossLineAmount - 0;
        //        decimal totalPayer = 0;
        //        if (isCoverageInPercentage)
        //            totalPayer = total * coverageAmount / 100;
        //        else
        //            totalPayer = coverageAmount * 1;

        //        if (total == 0)
        //        {
        //            totalPayer = total;
        //        }
        //        else
        //        {
        //            if (totalPayer < 0 && totalPayer < total)
        //            {
        //                totalPayer = total;
        //            }
        //            else if (totalPayer > 0 & totalPayer > total)
        //            {
        //                totalPayer = total;
        //            }
        //        }

        //        patientChargesDt.PatientAmount = total - totalPayer;
        //        patientChargesDt.PayerAmount = totalPayer;
        //        patientChargesDt.LineAmount = total;

        //        patientChargesDt.IsCITO = false;
        //        patientChargesDt.CITOAmount = 0;
        //        patientChargesDt.IsComplication = false;
        //        patientChargesDt.ComplicationAmount = 0;
        //        patientChargesDt.IsDiscount = false;
        //        patientChargesDt.IsCreatedBySystem = true;
        //        patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.OPEN;  //Constant.TransactionStatus.WAIT_FOR_APPROVAL;
        //        patientChargesDt.CreatedBy = patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
        //        patientChargesDt.LastUpdatedDate = DateTime.Now;
        //        ctx.CommandType = CommandType.Text;
        //        ctx.Command.Parameters.Clear();
        //        lstEntityDt.Add(patientChargesDt);
        //        int ID = patientChargesDtDao.InsertReturnPrimaryKeyID(patientChargesDt);

        //        #region Patient Charges DT Package

        //        string filterPackage = string.Format("ItemID = {0} AND IsDeleted = 0", patientChargesDt.ItemID);
        //        List<vItemServiceDt> isdList = BusinessLayer.GetvItemServiceDtList(filterPackage);
        //        foreach (vItemServiceDt isd in isdList)
        //        {
        //            PatientChargesDtPackage dtpackage = new PatientChargesDtPackage();
        //            dtpackage.PatientChargesDtID = ID;
        //            dtpackage.ItemID = isd.DetailItemID;
        //            dtpackage.ParamedicID = patientChargesDt.ParamedicID;

        //            int revID = BusinessLayer.GetItemRevenueSharing(isd.DetailItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, visit.VisitID, patientChargesHd.HealthcareServiceUnitID, patientChargesHd.TransactionDate, patientChargesHd.TransactionTime, ctx).FirstOrDefault().RevenueSharingID;
        //            if (revID != 0 && revID != null)
        //            {
        //                dtpackage.RevenueSharingID = revID;
        //            }
        //            else
        //            {
        //                dtpackage.RevenueSharingID = null;
        //            }

        //            dtpackage.ChargedQuantity = (isd.Quantity * patientChargesDt.ChargedQuantity);

        //            ctx.CommandType = CommandType.Text;
        //            ctx.Command.Parameters.Clear();
        //            GetCurrentItemTariff tariff = BusinessLayer.GetCurrentItemTariff(visit.RegistrationID, patientChargesHd.VisitID, patientChargesDt.ChargeClassID, isd.DetailItemID, 1, DateTime.Now, ctx).FirstOrDefault();

        //            basePrice = tariff.BasePrice;
        //            basePriceComp1 = tariff.BasePriceComp1;
        //            basePriceComp2 = tariff.BasePriceComp2;
        //            basePriceComp3 = tariff.BasePriceComp3;
        //            price = tariff.Price;
        //            priceComp1 = tariff.PriceComp1;
        //            priceComp2 = tariff.PriceComp2;
        //            priceComp3 = tariff.PriceComp3;
        //            isDiscountUsedComp = tariff.IsDiscountUsedComp;
        //            discountAmount = tariff.DiscountAmount;
        //            discountAmountComp1 = tariff.DiscountAmountComp1;
        //            discountAmountComp2 = tariff.DiscountAmountComp2;
        //            discountAmountComp3 = tariff.DiscountAmountComp3;
        //            coverageAmount = tariff.CoverageAmount;
        //            isDiscountInPercentage = tariff.IsDiscountInPercentage;
        //            isDiscountInPercentageComp1 = tariff.IsDiscountInPercentageComp1;
        //            isDiscountInPercentageComp2 = tariff.IsDiscountInPercentageComp2;
        //            isDiscountInPercentageComp3 = tariff.IsDiscountInPercentageComp3;
        //            isCoverageInPercentage = tariff.IsCoverageInPercentage;
        //            costAmount = tariff.CostAmount;
        //            grossLineAmount = dtpackage.ChargedQuantity * price;

        //            dtpackage.BaseTariff = tariff.BasePrice;
        //            dtpackage.BaseComp1 = tariff.BasePriceComp1;
        //            dtpackage.BaseComp2 = tariff.BasePriceComp2;
        //            dtpackage.BaseComp3 = tariff.BasePriceComp3;
        //            dtpackage.Tariff = tariff.Price;
        //            dtpackage.TariffComp1 = tariff.PriceComp1;
        //            dtpackage.TariffComp2 = tariff.PriceComp2;
        //            dtpackage.TariffComp3 = tariff.PriceComp3;
        //            dtpackage.CostAmount = tariff.CostAmount;

        //            decimal totalDiscountAmount = 0;
        //            decimal totalDiscountAmount1 = 0;
        //            decimal totalDiscountAmount2 = 0;
        //            decimal totalDiscountAmount3 = 0;

        //            if (isDiscountInPercentage)
        //            {
        //                //totalDiscountAmount = grossLineAmount * discountAmount / 100;

        //                if (isDiscountUsedComp)
        //                {
        //                    if (priceComp1 > 0)
        //                    {
        //                        totalDiscountAmount1 = priceComp1 * discountAmountComp1 / 100;
        //                        dtpackage.DiscountPercentageComp1 = discountAmountComp1;
        //                    }

        //                    if (priceComp2 > 0)
        //                    {
        //                        totalDiscountAmount2 = priceComp2 * discountAmountComp2 / 100;
        //                        dtpackage.DiscountPercentageComp2 = discountAmountComp2;
        //                    }

        //                    if (priceComp3 > 0)
        //                    {
        //                        totalDiscountAmount3 = priceComp3 * discountAmountComp3 / 100;
        //                        dtpackage.DiscountPercentageComp3 = discountAmountComp3;
        //                    }
        //                }
        //                else
        //                {
        //                    if (priceComp1 > 0)
        //                    {
        //                        totalDiscountAmount1 = priceComp1 * discountAmount / 100;
        //                        dtpackage.DiscountPercentageComp1 = discountAmount;
        //                    }

        //                    if (priceComp2 > 0)
        //                    {
        //                        totalDiscountAmount2 = priceComp2 * discountAmount / 100;
        //                        dtpackage.DiscountPercentageComp2 = discountAmount;
        //                    }

        //                    if (priceComp3 > 0)
        //                    {
        //                        totalDiscountAmount3 = priceComp3 * discountAmount / 100;
        //                        dtpackage.DiscountPercentageComp3 = discountAmount;
        //                    }
        //                }

        //                if (totalDiscountAmount1 > 0)
        //                {
        //                    dtpackage.IsDiscountInPercentageComp1 = true;
        //                }

        //                if (totalDiscountAmount2 > 0)
        //                {
        //                    dtpackage.IsDiscountInPercentageComp2 = true;
        //                }

        //                if (totalDiscountAmount3 > 0)
        //                {
        //                    dtpackage.IsDiscountInPercentageComp3 = true;
        //                }
        //            }
        //            else
        //            {
        //                //totalDiscountAmount = discountAmount * 1;

        //                if (isDiscountUsedComp)
        //                {
        //                    if (priceComp1 > 0)
        //                        totalDiscountAmount1 = discountAmountComp1;
        //                    if (priceComp2 > 0)
        //                        totalDiscountAmount2 = discountAmountComp2;
        //                    if (priceComp3 > 0)
        //                        totalDiscountAmount3 = discountAmountComp3;
        //                }
        //                else
        //                {
        //                    if (priceComp1 > 0)
        //                        totalDiscountAmount1 = discountAmount;
        //                    if (priceComp2 > 0)
        //                        totalDiscountAmount2 = discountAmount;
        //                    if (priceComp3 > 0)
        //                        totalDiscountAmount3 = discountAmount;
        //                }
        //            }

        //            totalDiscountAmount = (totalDiscountAmount1 + totalDiscountAmount2 + totalDiscountAmount3) * (dtpackage.ChargedQuantity);

        //            if (grossLineAmount >= 0)
        //            {
        //                if (totalDiscountAmount > grossLineAmount)
        //                {
        //                    totalDiscountAmount = grossLineAmount;
        //                }
        //            }

        //            dtpackage.DiscountAmount = totalDiscountAmount;
        //            dtpackage.DiscountComp1 = totalDiscountAmount1;
        //            dtpackage.DiscountComp2 = totalDiscountAmount2;
        //            dtpackage.DiscountComp3 = totalDiscountAmount3;

        //            ctx.CommandType = CommandType.Text;
        //            ctx.Command.Parameters.Clear();
        //            string filterIP = string.Format("ItemID = {0} AND IsDeleted = 0", isd.DetailItemID);
        //            List<ItemPlanning> iplan = BusinessLayer.GetItemPlanningList(filterIP, ctx);
        //            if (iplan.Count() > 0)
        //            {
        //                dtpackage.AveragePrice = iplan.FirstOrDefault().AveragePrice;
        //            }
        //            else
        //            {
        //                dtpackage.AveragePrice = 0;
        //            }

        //            dtpackage.CreatedBy = AppSession.UserLogin.UserID;

        //            ctx.CommandType = CommandType.Text;
        //            ctx.Command.Parameters.Clear();
        //            patientChargesDtPackageDao.Insert(dtpackage);
        //        }

        //        #endregion

        //    }
        //    #endregion
        //}

        //private void GenerateOrderold(ref string result)
        //{
        //    IDbContext ctx = DbFactory.Configure(true);
        //    RegistrationDao entityRegistrationDao = new RegistrationDao(ctx);
        //    PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
        //    ConsultVisitItemPackageDao consultVisitItemPackageDao = new ConsultVisitItemPackageDao(ctx);
        //    try
        //    {

        //        DateTime dateNow = DateTime.Now;
        //        string timeNow = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
        //        //List<MCUOrder> lstMCUOrder = MCUOrderList.Where(t => t.IsConfirm).GroupBy(x => x.HealthcareServiceUnitID).Select(x => x.First()).ToList();

        //        string param = hdnSelectedAppointmentRequestID.Value;

        //        List<AppointmentRequest> lstAppointmentRequest = BusinessLayer.GetAppointmentRequestList(string.Format("AppointmentRequestID IN({0})", param));
        //        if (lstAppointmentRequest.Count > 0) {
        //            string ParamVisitID = string.Empty;
        //            foreach (AppointmentRequest row in lstAppointmentRequest) {

        //                ConsultVisit entityVisit = BusinessLayer.GetConsultVisitList(string.Format("VisitID = '{0}'", row.VisitID), ctx).FirstOrDefault();
        //                string filterExpression = string.Format("VisitID = '{0}' AND IsDeleted = 0 AND GCItemDetailStatus = '{1}'", row.VisitID, Constant.TransactionStatus.OPEN);

        //                List<ConsultVisitItemPackage> lstEntityItemPackage = BusinessLayer.GetConsultVisitItemPackageList(filterExpression);
        //                string lstItemPackageID = string.Join(",", lstEntityItemPackage.Select(x => x.ItemID));
        //                filterExpression = string.Format("ItemID IN ({0}) AND IsDeleted = 0 ORDER BY ItemID ASC", lstItemPackageID);
        //                List<vItemServiceDt> lstMCUOrderDt = BusinessLayer.GetvItemServiceDtList(filterExpression);
        //                List<vItemServiceDt> lstOrderMCU = lstMCUOrderDt.GroupBy(x => x.HealthcareServiceUnitID).Select(x => x.First()).ToList();

        //                if (lstOrderMCU.Count > 0)
        //                {

        //                    foreach (vItemServiceDt healthcareServiceUnit in lstOrderMCU)
        //                    {
        //                        List<vItemServiceDt> lstSelectedItemServiceDt = lstMCUOrderDt.Where(p => p.HealthcareServiceUnitID == healthcareServiceUnit.HealthcareServiceUnitID).ToList();
        //                        if (healthcareServiceUnit.DepartmentID == Constant.Facility.DIAGNOSTIC)
        //                        {
        //                            SaveTestOrder(ctx, entityVisit, lstSelectedItemServiceDt, dateNow, timeNow);
        //                        }
        //                        else
        //                        {
        //                            SaveServiceOrder(ctx, entityVisit, lstSelectedItemServiceDt, dateNow, timeNow);
        //                        }
        //                    }

        //                }

        //                foreach (ConsultVisitItemPackage entity in lstEntityItemPackage)
        //                {

        //                    entity.GCItemDetailStatus = Constant.TransactionStatus.PROCESSED;
        //                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
        //                    consultVisitItemPackageDao.Update(entity);
        //                }
        //                InsertMCUDefaultCharges(lstEntityItemPackage, entityVisit, ctx);

        //            }


        //        }

        //        result = "generate|success";
        //        ctx.CommitTransaction();
        //    }
        //    catch (Exception ex)
        //    {
        //        Helper.InsertErrorLog(ex);
        //        result = string.Format("generate|fail|{0}", ex.Message);
        //        ctx.RollBackTransaction();
        //    }
        //    finally
        //    {
        //        ctx.Close();
        //    }
        //}
        //
        #endregion

        private void SaveTestOrder(IDbContext ctx, ConsultVisit visit, List<MCUOrder> lstItemServiceDt, DateTime dateNow, String timeNow)
        {
            TestOrderHdDao entityHdDao = new TestOrderHdDao(ctx);
            TestOrderDtDao entityDao = new TestOrderDtDao(ctx);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao patientChargesDtDao = new PatientChargesDtDao(ctx);
            PatientChargesDtPackageDao patientChargesDtPackageDao = new PatientChargesDtPackageDao(ctx);

            TestOrderHd entityHd = new TestOrderHd();
            PatientChargesHd patientChargesHd = new PatientChargesHd();
            entityHd.FromHealthcareServiceUnitID = visit.HealthcareServiceUnitID; //Convert.ToInt32(hdnFromHealthcareServiceUnitID.Value);
            entityHd.HealthcareServiceUnitID = lstItemServiceDt[0].HealthcareServiceUnitID;
            entityHd.ParamedicID = Convert.ToInt32(visit.ParamedicID);
            entityHd.VisitID = visit.VisitID;
            entityHd.TestOrderDate = dateNow;
            entityHd.TestOrderTime = timeNow;
            entityHd.ScheduledDate = dateNow;
            entityHd.ScheduledTime = timeNow;
            if (entityHd.HealthcareServiceUnitID == Convert.ToInt32(hdnImagingServiceUnitID.Value))
            {
                entityHd.TransactionCode = Constant.TransactionCode.IMAGING_TEST_ORDER;
                patientChargesHd.TransactionCode = Constant.TransactionCode.IMAGING_CHARGES;
            }
            else if (entityHd.HealthcareServiceUnitID == Convert.ToInt32(hdnLaboratoryServiceUnitID.Value))
            {
                entityHd.TransactionCode = Constant.TransactionCode.LABORATORY_TEST_ORDER;
                patientChargesHd.TransactionCode = Constant.TransactionCode.LABORATORY_CHARGES;
            }
            else
            {
                entityHd.TransactionCode = Constant.TransactionCode.OTHER_TEST_ORDER;
                patientChargesHd.TransactionCode = Constant.TransactionCode.OTHER_DIAGNOSTIC_CHARGES;
            }
            entityHd.TestOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.TestOrderDate, ctx);
            ctx.CommandType = CommandType.Text;
            ctx.Command.Parameters.Clear();
            entityHd.GCOrderStatus = Constant.OrderStatus.OPEN;
            entityHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
            entityHd.GCToBePerformed = Constant.ToBePerformed.CURRENT_EPISODE;
            entityHd.CreatedBy = AppSession.UserLogin.UserID;
            entityHdDao.Insert(entityHd);
            entityHd.TestOrderID = BusinessLayer.GetTestOrderHdMaxID(ctx);

            #region Patient Charges HD
            patientChargesHd.VisitID = visit.VisitID;
            patientChargesHd.LinkedChargesID = null;
            patientChargesHd.TestOrderID = entityHd.TestOrderID;
            patientChargesHd.HealthcareServiceUnitID = lstItemServiceDt[0].HealthcareServiceUnitID;
            patientChargesHd.TransactionDate = DateTime.Now;
            patientChargesHd.TransactionTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            patientChargesHd.PatientBillingID = null;
            patientChargesHd.ReferenceNo = "";
            patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
            patientChargesHd.GCVoidReason = null;
            patientChargesHd.TotalPatientAmount = 0;
            patientChargesHd.TotalPayerAmount = 0;
            patientChargesHd.TotalAmount = 0;
            patientChargesHd.IsAutoTransaction = true;
            patientChargesHd.TransactionNo = BusinessLayer.GenerateTransactionNo(patientChargesHd.TransactionCode, patientChargesHd.TransactionDate, ctx);
            patientChargesHd.CreatedBy = AppSession.UserLogin.UserID;
            patientChargesHd.ProposedBy = AppSession.UserLogin.UserID;
            patientChargesHd.ProposedDate = DateTime.Now;
            ctx.CommandType = CommandType.Text;
            ctx.Command.Parameters.Clear();
            patientChargesHd.TransactionID = patientChargesHdDao.InsertReturnPrimaryKeyID(patientChargesHd);
            #endregion

            #region Patient Charges DT
            foreach (MCUOrder itemServiceDt in lstItemServiceDt)
            {
                TestOrderDt entity = new TestOrderDt();
                entity.TestOrderID = entityHd.TestOrderID;
                entity.ItemID = itemServiceDt.DetailItemID;
                entity.ItemPackageID = itemServiceDt.ItemID;
                entity.DiagnoseID = null;
                entity.GCTestOrderStatus = Constant.TestOrderStatus.IN_PROGRESS;
                entity.ItemQty = itemServiceDt.Quantity;
                entity.ItemUnit = itemServiceDt.GCItemUnit;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                entity.ID = BusinessLayer.GetTestOrderDtMaxID(ctx);
                PatientChargesDt patientChargesDt = new PatientChargesDt();
                patientChargesDt.TransactionID = patientChargesHd.TransactionID;
                patientChargesDt.ItemID = entity.ItemID;
                patientChargesDt.ChargeClassID = Convert.ToInt32(visit.ChargeClassID);
                patientChargesDt.ItemPackageID = entity.ItemPackageID;
                patientChargesDt.ReferenceDtID = entity.ID;
                List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(visit.RegistrationID, visit.VisitID, patientChargesDt.ChargeClassID, entity.ItemID, 1, DateTime.Now, ctx);

                decimal basePrice = 0;
                decimal basePriceComp1 = 0;
                decimal basePriceComp2 = 0;
                decimal basePriceComp3 = 0;
                decimal price = 0;
                decimal priceComp1 = 0;
                decimal priceComp2 = 0;
                decimal priceComp3 = 0;
                bool isDiscountUsedComp = false;
                decimal discountAmount = 0;
                decimal discountAmountComp1 = 0;
                decimal discountAmountComp2 = 0;
                decimal discountAmountComp3 = 0;
                decimal coverageAmount = 0;
                bool isDiscountInPercentage = false;
                bool isDiscountInPercentageComp1 = false;
                bool isDiscountInPercentageComp2 = false;
                bool isDiscountInPercentageComp3 = false;
                bool isCoverageInPercentage = false;
                decimal costAmount = 0;

                decimal totalDiscountAmount = 0;
                decimal totalDiscountAmount1 = 0;
                decimal totalDiscountAmount2 = 0;
                decimal totalDiscountAmount3 = 0;

                if (list.Count > 0)
                {
                    GetCurrentItemTariff obj = list[0];
                    basePrice = obj.BasePrice;
                    basePriceComp1 = obj.BasePriceComp1;
                    basePriceComp2 = obj.BasePriceComp2;
                    basePriceComp3 = obj.BasePriceComp3;
                    price = obj.Price;
                    priceComp1 = obj.PriceComp1;
                    priceComp2 = obj.PriceComp2;
                    priceComp3 = obj.PriceComp3;
                    isDiscountUsedComp = obj.IsDiscountUsedComp;
                    discountAmount = obj.DiscountAmount;
                    discountAmountComp1 = obj.DiscountAmountComp1;
                    discountAmountComp2 = obj.DiscountAmountComp2;
                    discountAmountComp3 = obj.DiscountAmountComp3;
                    coverageAmount = obj.CoverageAmount;
                    isDiscountInPercentage = obj.IsDiscountInPercentage;
                    isDiscountInPercentageComp1 = obj.IsDiscountInPercentageComp1;
                    isDiscountInPercentageComp2 = obj.IsDiscountInPercentageComp2;
                    isDiscountInPercentageComp3 = obj.IsDiscountInPercentageComp3;
                    isCoverageInPercentage = obj.IsCoverageInPercentage;
                    costAmount = obj.CostAmount;
                }

                patientChargesDt.Tariff = price - itemServiceDt.DiscountAmount;
                patientChargesDt.TariffComp1 = priceComp1 - itemServiceDt.DiscountAmount1;
                patientChargesDt.TariffComp2 = priceComp2 - itemServiceDt.DiscountAmount2;
                patientChargesDt.TariffComp3 = priceComp3 - itemServiceDt.DiscountAmount3;

                patientChargesDt.BaseTariff = basePrice - itemServiceDt.DiscountAmount;
                patientChargesDt.BaseComp1 = basePriceComp1 - itemServiceDt.DiscountAmount1;
                patientChargesDt.BaseComp2 = basePriceComp2 - itemServiceDt.DiscountAmount2;
                patientChargesDt.BaseComp3 = basePriceComp3 - itemServiceDt.DiscountAmount3;

                ItemMaster entityItemMaster = BusinessLayer.GetItemMasterList(string.Format("ItemID = {0}", entity.ItemID)).FirstOrDefault();
                patientChargesDt.GCBaseUnit = patientChargesDt.GCItemUnit = entityItemMaster.GCItemUnit;
                patientChargesDt.ParamedicID = itemServiceDt.ParamedicID;
                patientChargesDt.RevenueSharingID = BusinessLayer.GetItemRevenueSharing(entityItemMaster.ItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, visit.VisitID, patientChargesHd.HealthcareServiceUnitID, patientChargesHd.TransactionDate, patientChargesHd.TransactionTime).FirstOrDefault().RevenueSharingID;
                if (patientChargesDt.RevenueSharingID == 0)
                    patientChargesDt.RevenueSharingID = null;
                patientChargesDt.IsVariable = false;
                patientChargesDt.IsUnbilledItem = false;
                patientChargesDt.UsedQuantity = patientChargesDt.BaseQuantity = patientChargesDt.ChargedQuantity = entity.ItemQty;
                patientChargesDt.IsCITO = false;
                patientChargesDt.CITOAmount = 0;

                // diskon ini sudah tidak dipakai lagi karena untuk harga mcu sudah net 

                //decimal totalDiscountAmount = 0;
                //if (isDiscountInPercentage)
                //{
                //    totalDiscountAmount = grossLineAmount * discountAmount / 100;
                //}
                //else
                //{
                //    totalDiscountAmount = discountAmount * patientChargesDt.ChargedQuantity;
                //}

                //if (totalDiscountAmount > grossLineAmount)
                //{
                //    totalDiscountAmount = grossLineAmount;
                //}

                //totalDiscountAmount += (itemServiceDt.DiscountAmount * patientChargesDt.ChargedQuantity);

                //var tempDiscountTotal = totalDiscountAmount;
                //decimal discountComp1 = (itemServiceDt.DiscountAmount1 * patientChargesDt.ChargedQuantity);
                //decimal discountComp2 = (itemServiceDt.DiscountAmount2 * patientChargesDt.ChargedQuantity);
                //decimal discountComp3 = (itemServiceDt.DiscountAmount3 * patientChargesDt.ChargedQuantity);

                //if (discountComp1 > priceComp1)
                //{
                //    discountComp1 = priceComp1;
                //}

                //if (discountComp2 > priceComp2)
                //{
                //    discountComp2 = priceComp2;
                //}

                //if (discountComp3 > priceComp3)
                //{
                //    discountComp3 = priceComp3;
                //}

                patientChargesDt.DiscountAmount = 0;
                patientChargesDt.DiscountComp1 = 0;
                patientChargesDt.DiscountComp2 = 0;
                patientChargesDt.DiscountComp3 = 0;

                decimal grossLineAmount = patientChargesDt.ChargedQuantity * patientChargesDt.Tariff;
                decimal total = grossLineAmount - 0;
                decimal totalPayer = 0;
                if (isCoverageInPercentage)
                    totalPayer = total * coverageAmount / 100;
                else
                    totalPayer = coverageAmount * 1;

                if (total == 0)
                {
                    totalPayer = total;
                }
                else
                {
                    if (totalPayer < 0 && totalPayer < total)
                    {
                        totalPayer = total;
                    }
                    else if (totalPayer > 0 & totalPayer > total)
                    {
                        totalPayer = total;
                    }
                }

                patientChargesDt.IsComplication = false;
                patientChargesDt.ComplicationAmount = 0;
                patientChargesDt.IsDiscount = false;
                patientChargesDt.PatientAmount = total - totalPayer;
                patientChargesDt.PayerAmount = totalPayer;
                patientChargesDt.LineAmount = total;
                patientChargesDt.IsCreatedBySystem = true;
                patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                patientChargesDt.CreatedBy = patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                lstEntityDt.Add(patientChargesDt);
                int ID = patientChargesDtDao.InsertReturnPrimaryKeyID(patientChargesDt);

                #region Patient Charges DT Package

                string filterPackage = string.Format("ItemID = {0} AND IsDeleted = 0", patientChargesDt.ItemID);
                List<vItemServiceDt> isdList = BusinessLayer.GetvItemServiceDtList(filterPackage);
                foreach (vItemServiceDt isd in isdList)
                {
                    PatientChargesDtPackage dtpackage = new PatientChargesDtPackage();
                    dtpackage.PatientChargesDtID = ID;
                    dtpackage.ItemID = isd.DetailItemID;
                    dtpackage.ParamedicID = patientChargesDt.ParamedicID;

                    int revID = BusinessLayer.GetItemRevenueSharing(isd.DetailItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, visit.VisitID, patientChargesHd.HealthcareServiceUnitID, patientChargesHd.TransactionDate, patientChargesHd.TransactionTime, ctx).FirstOrDefault().RevenueSharingID;
                    if (revID != 0 && revID != null)
                    {
                        dtpackage.RevenueSharingID = revID;
                    }
                    else
                    {
                        dtpackage.RevenueSharingID = null;
                    }

                    dtpackage.ChargedQuantity = (isd.Quantity * patientChargesDt.ChargedQuantity);

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    GetCurrentItemTariff tariff = BusinessLayer.GetCurrentItemTariff(AppSession.RegisteredPatient.RegistrationID, patientChargesHd.VisitID, patientChargesDt.ChargeClassID, isd.DetailItemID, 1, DateTime.Now, ctx).FirstOrDefault();

                    basePrice = tariff.BasePrice;
                    basePriceComp1 = tariff.BasePriceComp1;
                    basePriceComp2 = tariff.BasePriceComp2;
                    basePriceComp3 = tariff.BasePriceComp3;
                    price = tariff.Price;
                    priceComp1 = tariff.PriceComp1;
                    priceComp2 = tariff.PriceComp2;
                    priceComp3 = tariff.PriceComp3;
                    isDiscountUsedComp = tariff.IsDiscountUsedComp;
                    discountAmount = tariff.DiscountAmount;
                    discountAmountComp1 = tariff.DiscountAmountComp1;
                    discountAmountComp2 = tariff.DiscountAmountComp2;
                    discountAmountComp3 = tariff.DiscountAmountComp3;
                    coverageAmount = tariff.CoverageAmount;
                    isDiscountInPercentage = tariff.IsDiscountInPercentage;
                    isDiscountInPercentageComp1 = tariff.IsDiscountInPercentageComp1;
                    isDiscountInPercentageComp2 = tariff.IsDiscountInPercentageComp2;
                    isDiscountInPercentageComp3 = tariff.IsDiscountInPercentageComp3;
                    isCoverageInPercentage = tariff.IsCoverageInPercentage;
                    costAmount = tariff.CostAmount;
                    grossLineAmount = dtpackage.ChargedQuantity * price;

                    dtpackage.BaseTariff = tariff.BasePrice;
                    dtpackage.BaseComp1 = tariff.BasePriceComp1;
                    dtpackage.BaseComp2 = tariff.BasePriceComp2;
                    dtpackage.BaseComp3 = tariff.BasePriceComp3;
                    dtpackage.Tariff = tariff.Price;
                    dtpackage.TariffComp1 = tariff.PriceComp1;
                    dtpackage.TariffComp2 = tariff.PriceComp2;
                    dtpackage.TariffComp3 = tariff.PriceComp3;
                    dtpackage.CostAmount = tariff.CostAmount;

                    if (isDiscountInPercentage)
                    {
                        //totalDiscountAmount = grossLineAmount * discountAmount / 100;

                        if (isDiscountUsedComp)
                        {
                            if (priceComp1 > 0)
                            {
                                totalDiscountAmount1 = priceComp1 * discountAmountComp1 / 100;
                                dtpackage.DiscountPercentageComp1 = discountAmountComp1;
                            }

                            if (priceComp2 > 0)
                            {
                                totalDiscountAmount2 = priceComp2 * discountAmountComp2 / 100;
                                dtpackage.DiscountPercentageComp2 = discountAmountComp2;
                            }

                            if (priceComp3 > 0)
                            {
                                totalDiscountAmount3 = priceComp3 * discountAmountComp3 / 100;
                                dtpackage.DiscountPercentageComp3 = discountAmountComp3;
                            }
                        }
                        else
                        {
                            if (priceComp1 > 0)
                            {
                                totalDiscountAmount1 = priceComp1 * discountAmount / 100;
                                dtpackage.DiscountPercentageComp1 = discountAmount;
                            }

                            if (priceComp2 > 0)
                            {
                                totalDiscountAmount2 = priceComp2 * discountAmount / 100;
                                dtpackage.DiscountPercentageComp2 = discountAmount;
                            }

                            if (priceComp3 > 0)
                            {
                                totalDiscountAmount3 = priceComp3 * discountAmount / 100;
                                dtpackage.DiscountPercentageComp3 = discountAmount;
                            }
                        }

                        if (totalDiscountAmount1 > 0)
                        {
                            dtpackage.IsDiscountInPercentageComp1 = true;
                        }

                        if (totalDiscountAmount2 > 0)
                        {
                            dtpackage.IsDiscountInPercentageComp2 = true;
                        }

                        if (totalDiscountAmount3 > 0)
                        {
                            dtpackage.IsDiscountInPercentageComp3 = true;
                        }
                    }
                    else
                    {
                        //totalDiscountAmount = discountAmount * 1;

                        if (isDiscountUsedComp)
                        {
                            if (priceComp1 > 0)
                                totalDiscountAmount1 = discountAmountComp1;
                            if (priceComp2 > 0)
                                totalDiscountAmount2 = discountAmountComp2;
                            if (priceComp3 > 0)
                                totalDiscountAmount3 = discountAmountComp3;
                        }
                        else
                        {
                            if (priceComp1 > 0)
                                totalDiscountAmount1 = discountAmount;
                            if (priceComp2 > 0)
                                totalDiscountAmount2 = discountAmount;
                            if (priceComp3 > 0)
                                totalDiscountAmount3 = discountAmount;
                        }
                    }

                    totalDiscountAmount = (totalDiscountAmount1 + totalDiscountAmount2 + totalDiscountAmount3) * (dtpackage.ChargedQuantity);

                    if (grossLineAmount >= 0)
                    {
                        if (totalDiscountAmount > grossLineAmount)
                        {
                            totalDiscountAmount = grossLineAmount;
                        }
                    }

                    dtpackage.DiscountAmount = totalDiscountAmount;
                    dtpackage.DiscountComp1 = totalDiscountAmount1;
                    dtpackage.DiscountComp2 = totalDiscountAmount2;
                    dtpackage.DiscountComp3 = totalDiscountAmount3;

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    string filterIP = string.Format("ItemID = {0} AND IsDeleted = 0", isd.DetailItemID);
                    List<ItemPlanning> iplan = BusinessLayer.GetItemPlanningList(filterIP, ctx);
                    if (iplan.Count() > 0)
                    {
                        dtpackage.AveragePrice = iplan.FirstOrDefault().AveragePrice;
                    }
                    else
                    {
                        dtpackage.AveragePrice = 0;
                    }

                    dtpackage.CreatedBy = AppSession.UserLogin.UserID;

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    patientChargesDtPackageDao.Insert(dtpackage);
                }

                #endregion

            }
            #endregion
        }

        private void SaveServiceOrder(IDbContext ctx, ConsultVisit visit, List<MCUOrder> lstItemServiceDt, DateTime dateNow, String timeNow)
        {
            ServiceOrderHdDao entityHdDao = new ServiceOrderHdDao(ctx);
            ServiceOrderDtDao entityDao = new ServiceOrderDtDao(ctx);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao patientChargesDtDao = new PatientChargesDtDao(ctx);
            PatientChargesDtPackageDao patientChargesDtPackageDao = new PatientChargesDtPackageDao(ctx);

            ServiceOrderHd entityHd = new ServiceOrderHd();
            PatientChargesHd patientChargesHd = new PatientChargesHd();
            entityHd.HealthcareServiceUnitID = lstItemServiceDt[0].HealthcareServiceUnitID;
            entityHd.ParamedicID = Convert.ToInt32(visit.ParamedicID);
            entityHd.VisitID = visit.VisitID;
            entityHd.ServiceOrderDate = dateNow;
            entityHd.ServiceOrderTime = timeNow;
            if (lstItemServiceDt[0].DepartmentID == Constant.Facility.EMERGENCY)
            {
                entityHd.TransactionCode = Constant.TransactionCode.MCU_EMERGENCY_ORDER;
                patientChargesHd.TransactionCode = Constant.TransactionCode.ER_CHARGES;
            }
            else
            {
                entityHd.TransactionCode = Constant.TransactionCode.MCU_OUTPATIENT_ORDER;
                patientChargesHd.TransactionCode = Constant.TransactionCode.OP_CHARGES;
            }
            entityHd.ServiceOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.ServiceOrderDate, ctx);
            ctx.CommandType = CommandType.Text;
            ctx.Command.Parameters.Clear();
            entityHd.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
            entityHd.CreatedBy = AppSession.UserLogin.UserID;
            entityHdDao.Insert(entityHd);
            entityHd.ServiceOrderID = BusinessLayer.GetServiceOrderHdMaxID(ctx);

            #region Patient Charges HD

            patientChargesHd.VisitID = visit.VisitID;
            patientChargesHd.LinkedChargesID = null;
            patientChargesHd.ServiceOrderID = entityHd.ServiceOrderID;
            patientChargesHd.HealthcareServiceUnitID = lstItemServiceDt[0].HealthcareServiceUnitID;
            patientChargesHd.TransactionDate = DateTime.Now;
            patientChargesHd.TransactionTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            patientChargesHd.PatientBillingID = null;
            patientChargesHd.ReferenceNo = "";
            patientChargesHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
            patientChargesHd.GCVoidReason = null;
            patientChargesHd.TotalPatientAmount = 0;
            patientChargesHd.TotalPayerAmount = 0;
            patientChargesHd.TotalAmount = 0;
            patientChargesHd.IsAutoTransaction = true;
            patientChargesHd.TransactionNo = BusinessLayer.GenerateTransactionNo(patientChargesHd.TransactionCode, patientChargesHd.TransactionDate, ctx);
            patientChargesHd.CreatedBy = AppSession.UserLogin.UserID;
            patientChargesHd.ProposedBy = AppSession.UserLogin.UserID;
            patientChargesHd.ProposedDate = DateTime.Now;
            ctx.CommandType = CommandType.Text;
            ctx.Command.Parameters.Clear();
            patientChargesHd.TransactionID = patientChargesHdDao.InsertReturnPrimaryKeyID(patientChargesHd);

            #endregion

            #region Patient Charges DT
            foreach (MCUOrder itemServiceDt in lstItemServiceDt)
            {
                ServiceOrderDt entity = new ServiceOrderDt();
                entity.ServiceOrderID = entityHd.ServiceOrderID;
                entity.ItemID = itemServiceDt.DetailItemID;
                entity.ItemPackageID = itemServiceDt.ItemID;
                entity.ItemQty = itemServiceDt.Quantity;
                entity.ItemUnit = itemServiceDt.GCItemUnit;
                entity.GCServiceOrderStatus = Constant.TestOrderStatus.IN_PROGRESS;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                entity.ID = BusinessLayer.GetServiceOrderDtMaxID(ctx);

                PatientChargesDt patientChargesDt = new PatientChargesDt();
                patientChargesDt.TransactionID = patientChargesHd.TransactionID;
                patientChargesDt.ItemID = entity.ItemID;
                patientChargesDt.ChargeClassID = Convert.ToInt32(visit.ChargeClassID);
                patientChargesDt.ItemPackageID = entity.ItemPackageID;
                patientChargesDt.ReferenceDtID = entity.ID;

                List<GetCurrentItemTariff> list = BusinessLayer.GetCurrentItemTariff(visit.RegistrationID, visit.VisitID, patientChargesDt.ChargeClassID, entity.ItemID, 1, DateTime.Now, ctx);

                decimal basePrice = 0;
                decimal basePriceComp1 = 0;
                decimal basePriceComp2 = 0;
                decimal basePriceComp3 = 0;
                decimal price = 0;
                decimal priceComp1 = 0;
                decimal priceComp2 = 0;
                decimal priceComp3 = 0;
                bool isDiscountUsedComp = false;
                decimal discountAmount = 0;
                decimal discountAmountComp1 = 0;
                decimal discountAmountComp2 = 0;
                decimal discountAmountComp3 = 0;
                decimal coverageAmount = 0;
                bool isDiscountInPercentage = false;
                bool isDiscountInPercentageComp1 = false;
                bool isDiscountInPercentageComp2 = false;
                bool isDiscountInPercentageComp3 = false;
                bool isCoverageInPercentage = false;
                decimal costAmount = 0;

                if (list.Count > 0)
                {
                    GetCurrentItemTariff obj = list[0];
                    basePrice = obj.BasePrice;
                    basePriceComp1 = obj.BasePriceComp1;
                    basePriceComp2 = obj.BasePriceComp2;
                    basePriceComp3 = obj.BasePriceComp3;
                    price = obj.Price;
                    priceComp1 = obj.PriceComp1;
                    priceComp2 = obj.PriceComp2;
                    priceComp3 = obj.PriceComp3;
                    isDiscountUsedComp = obj.IsDiscountUsedComp;
                    discountAmount = obj.DiscountAmount;
                    discountAmountComp1 = obj.DiscountAmountComp1;
                    discountAmountComp2 = obj.DiscountAmountComp2;
                    discountAmountComp3 = obj.DiscountAmountComp3;
                    coverageAmount = obj.CoverageAmount;
                    isDiscountInPercentage = obj.IsDiscountInPercentage;
                    isDiscountInPercentageComp1 = obj.IsDiscountInPercentageComp1;
                    isDiscountInPercentageComp2 = obj.IsDiscountInPercentageComp2;
                    isDiscountInPercentageComp3 = obj.IsDiscountInPercentageComp3;
                    isCoverageInPercentage = obj.IsCoverageInPercentage;
                    costAmount = obj.CostAmount;
                }

                patientChargesDt.Tariff = price - itemServiceDt.DiscountAmount;
                patientChargesDt.TariffComp1 = priceComp1 - itemServiceDt.DiscountAmount1;
                patientChargesDt.TariffComp2 = priceComp2 - itemServiceDt.DiscountAmount2;
                patientChargesDt.TariffComp3 = priceComp3 - itemServiceDt.DiscountAmount3;

                patientChargesDt.BaseTariff = basePrice - itemServiceDt.DiscountAmount;
                patientChargesDt.BaseComp1 = basePriceComp1 - itemServiceDt.DiscountAmount1;
                patientChargesDt.BaseComp2 = basePriceComp2 - itemServiceDt.DiscountAmount2;
                patientChargesDt.BaseComp3 = basePriceComp3 - itemServiceDt.DiscountAmount3;

                vItemService entityItemMaster = BusinessLayer.GetvItemServiceList(string.Format("ItemID = {0}", entity.ItemID)).FirstOrDefault();
                patientChargesDt.GCBaseUnit = patientChargesDt.GCItemUnit = entityItemMaster.GCItemUnit;
                patientChargesDt.ParamedicID = itemServiceDt.ParamedicID;
                patientChargesDt.RevenueSharingID = BusinessLayer.GetItemRevenueSharing(entityItemMaster.ItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, visit.VisitID, patientChargesHd.HealthcareServiceUnitID, patientChargesHd.TransactionDate, patientChargesHd.TransactionTime).FirstOrDefault().RevenueSharingID;
                if (patientChargesDt.RevenueSharingID == 0)
                    patientChargesDt.RevenueSharingID = null;
                patientChargesDt.IsVariable = false;
                patientChargesDt.IsUnbilledItem = false;

                patientChargesDt.UsedQuantity = patientChargesDt.BaseQuantity = patientChargesDt.ChargedQuantity = entity.ItemQty;

                // diskon ini sudah tidak dipakai lagi karena untuk harga mcu sudah net 

                //if (isDiscountInPercentage)
                //    totalDiscountAmount = grossLineAmount * discountAmount / 100;
                //else
                //    totalDiscountAmount = discountAmount * patientChargesDt.ChargedQuantity;
                //if (totalDiscountAmount > grossLineAmount)
                //    totalDiscountAmount = grossLineAmount;
                //totalDiscountAmount += (itemServiceDt.DiscountAmount * patientChargesDt.ChargedQuantity);

                //decimal totalDiscountAmount = 0;
                //var tempDiscountTotal = totalDiscountAmount;
                //decimal discountComp1 = (itemServiceDt.DiscountAmount1 * patientChargesDt.ChargedQuantity);
                //decimal discountComp2 = (itemServiceDt.DiscountAmount2 * patientChargesDt.ChargedQuantity);
                //decimal discountComp3 = (itemServiceDt.DiscountAmount3 * patientChargesDt.ChargedQuantity);

                //if (discountComp1 > priceComp1)
                //{
                //    discountComp1 = priceComp1;
                //}

                //if (discountComp2 > priceComp2)
                //{
                //    discountComp2 = priceComp2;
                //}

                //if (discountComp3 > priceComp3)
                //{
                //    discountComp3 = priceComp3;
                //}

                patientChargesDt.DiscountAmount = 0;
                patientChargesDt.DiscountComp1 = 0;
                patientChargesDt.DiscountComp2 = 0;
                patientChargesDt.DiscountComp3 = 0;

                decimal grossLineAmount = patientChargesDt.ChargedQuantity * patientChargesDt.Tariff;
                decimal total = grossLineAmount - 0;
                decimal totalPayer = 0;
                if (isCoverageInPercentage)
                    totalPayer = total * coverageAmount / 100;
                else
                    totalPayer = coverageAmount * 1;

                if (total == 0)
                {
                    totalPayer = total;
                }
                else
                {
                    if (totalPayer < 0 && totalPayer < total)
                    {
                        totalPayer = total;
                    }
                    else if (totalPayer > 0 & totalPayer > total)
                    {
                        totalPayer = total;
                    }
                }

                patientChargesDt.PatientAmount = total - totalPayer;
                patientChargesDt.PayerAmount = totalPayer;
                patientChargesDt.LineAmount = total;

                patientChargesDt.IsCITO = false;
                patientChargesDt.CITOAmount = 0;
                patientChargesDt.IsComplication = false;
                patientChargesDt.ComplicationAmount = 0;
                patientChargesDt.IsDiscount = false;
                patientChargesDt.IsCreatedBySystem = true;
                patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                patientChargesDt.CreatedBy = patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                lstEntityDt.Add(patientChargesDt);
                int ID = patientChargesDtDao.InsertReturnPrimaryKeyID(patientChargesDt);

                #region Patient Charges DT Package

                string filterPackage = string.Format("ItemID = {0} AND IsDeleted = 0", patientChargesDt.ItemID);
                List<vItemServiceDt> isdList = BusinessLayer.GetvItemServiceDtList(filterPackage);
                foreach (vItemServiceDt isd in isdList)
                {
                    PatientChargesDtPackage dtpackage = new PatientChargesDtPackage();
                    dtpackage.PatientChargesDtID = ID;
                    dtpackage.ItemID = isd.DetailItemID;
                    dtpackage.ParamedicID = patientChargesDt.ParamedicID;

                    int revID = BusinessLayer.GetItemRevenueSharing(isd.DetailItemCode, patientChargesDt.ParamedicID, patientChargesDt.ChargeClassID, Constant.ParamedicRole.PELAKSANA, visit.VisitID, patientChargesHd.HealthcareServiceUnitID, patientChargesHd.TransactionDate, patientChargesHd.TransactionTime, ctx).FirstOrDefault().RevenueSharingID;
                    if (revID != 0 && revID != null)
                    {
                        dtpackage.RevenueSharingID = revID;
                    }
                    else
                    {
                        dtpackage.RevenueSharingID = null;
                    }

                    dtpackage.ChargedQuantity = (isd.Quantity * patientChargesDt.ChargedQuantity);

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    GetCurrentItemTariff tariff = BusinessLayer.GetCurrentItemTariff(AppSession.RegisteredPatient.RegistrationID, patientChargesHd.VisitID, patientChargesDt.ChargeClassID, isd.DetailItemID, 1, DateTime.Now, ctx).FirstOrDefault();

                    basePrice = tariff.BasePrice;
                    basePriceComp1 = tariff.BasePriceComp1;
                    basePriceComp2 = tariff.BasePriceComp2;
                    basePriceComp3 = tariff.BasePriceComp3;
                    price = tariff.Price;
                    priceComp1 = tariff.PriceComp1;
                    priceComp2 = tariff.PriceComp2;
                    priceComp3 = tariff.PriceComp3;
                    isDiscountUsedComp = tariff.IsDiscountUsedComp;
                    discountAmount = tariff.DiscountAmount;
                    discountAmountComp1 = tariff.DiscountAmountComp1;
                    discountAmountComp2 = tariff.DiscountAmountComp2;
                    discountAmountComp3 = tariff.DiscountAmountComp3;
                    coverageAmount = tariff.CoverageAmount;
                    isDiscountInPercentage = tariff.IsDiscountInPercentage;
                    isDiscountInPercentageComp1 = tariff.IsDiscountInPercentageComp1;
                    isDiscountInPercentageComp2 = tariff.IsDiscountInPercentageComp2;
                    isDiscountInPercentageComp3 = tariff.IsDiscountInPercentageComp3;
                    isCoverageInPercentage = tariff.IsCoverageInPercentage;
                    costAmount = tariff.CostAmount;
                    grossLineAmount = dtpackage.ChargedQuantity * price;

                    dtpackage.BaseTariff = tariff.BasePrice;
                    dtpackage.BaseComp1 = tariff.BasePriceComp1;
                    dtpackage.BaseComp2 = tariff.BasePriceComp2;
                    dtpackage.BaseComp3 = tariff.BasePriceComp3;
                    dtpackage.Tariff = tariff.Price;
                    dtpackage.TariffComp1 = tariff.PriceComp1;
                    dtpackage.TariffComp2 = tariff.PriceComp2;
                    dtpackage.TariffComp3 = tariff.PriceComp3;
                    dtpackage.CostAmount = tariff.CostAmount;

                    decimal totalDiscountAmount = 0;
                    decimal totalDiscountAmount1 = 0;
                    decimal totalDiscountAmount2 = 0;
                    decimal totalDiscountAmount3 = 0;

                    if (isDiscountInPercentage)
                    {
                        //totalDiscountAmount = grossLineAmount * discountAmount / 100;

                        if (isDiscountUsedComp)
                        {
                            if (priceComp1 > 0)
                            {
                                totalDiscountAmount1 = priceComp1 * discountAmountComp1 / 100;
                                dtpackage.DiscountPercentageComp1 = discountAmountComp1;
                            }

                            if (priceComp2 > 0)
                            {
                                totalDiscountAmount2 = priceComp2 * discountAmountComp2 / 100;
                                dtpackage.DiscountPercentageComp2 = discountAmountComp2;
                            }

                            if (priceComp3 > 0)
                            {
                                totalDiscountAmount3 = priceComp3 * discountAmountComp3 / 100;
                                dtpackage.DiscountPercentageComp3 = discountAmountComp3;
                            }
                        }
                        else
                        {
                            if (priceComp1 > 0)
                            {
                                totalDiscountAmount1 = priceComp1 * discountAmount / 100;
                                dtpackage.DiscountPercentageComp1 = discountAmount;
                            }

                            if (priceComp2 > 0)
                            {
                                totalDiscountAmount2 = priceComp2 * discountAmount / 100;
                                dtpackage.DiscountPercentageComp2 = discountAmount;
                            }

                            if (priceComp3 > 0)
                            {
                                totalDiscountAmount3 = priceComp3 * discountAmount / 100;
                                dtpackage.DiscountPercentageComp3 = discountAmount;
                            }
                        }

                        if (totalDiscountAmount1 > 0)
                        {
                            dtpackage.IsDiscountInPercentageComp1 = true;
                        }

                        if (totalDiscountAmount2 > 0)
                        {
                            dtpackage.IsDiscountInPercentageComp2 = true;
                        }

                        if (totalDiscountAmount3 > 0)
                        {
                            dtpackage.IsDiscountInPercentageComp3 = true;
                        }
                    }
                    else
                    {
                        //totalDiscountAmount = discountAmount * 1;

                        if (isDiscountUsedComp)
                        {
                            if (priceComp1 > 0)
                                totalDiscountAmount1 = discountAmountComp1;
                            if (priceComp2 > 0)
                                totalDiscountAmount2 = discountAmountComp2;
                            if (priceComp3 > 0)
                                totalDiscountAmount3 = discountAmountComp3;
                        }
                        else
                        {
                            if (priceComp1 > 0)
                                totalDiscountAmount1 = discountAmount;
                            if (priceComp2 > 0)
                                totalDiscountAmount2 = discountAmount;
                            if (priceComp3 > 0)
                                totalDiscountAmount3 = discountAmount;
                        }
                    }

                    totalDiscountAmount = (totalDiscountAmount1 + totalDiscountAmount2 + totalDiscountAmount3) * (dtpackage.ChargedQuantity);

                    if (grossLineAmount >= 0)
                    {
                        if (totalDiscountAmount > grossLineAmount)
                        {
                            totalDiscountAmount = grossLineAmount;
                        }
                    }

                    dtpackage.DiscountAmount = totalDiscountAmount;
                    dtpackage.DiscountComp1 = totalDiscountAmount1;
                    dtpackage.DiscountComp2 = totalDiscountAmount2;
                    dtpackage.DiscountComp3 = totalDiscountAmount3;

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    string filterIP = string.Format("ItemID = {0} AND IsDeleted = 0", isd.DetailItemID);
                    List<ItemPlanning> iplan = BusinessLayer.GetItemPlanningList(filterIP, ctx);
                    if (iplan.Count() > 0)
                    {
                        dtpackage.AveragePrice = iplan.FirstOrDefault().AveragePrice;
                    }
                    else
                    {
                        dtpackage.AveragePrice = 0;
                    }

                    dtpackage.CreatedBy = AppSession.UserLogin.UserID;

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    patientChargesDtPackageDao.Insert(dtpackage);
                }

                #endregion

            }
            #endregion
        }

        private void GenerateOrder(ref string result)
        {
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationDao entityRegistrationDao = new RegistrationDao(ctx);
            PatientChargesHdDao patientChargesHdDao = new PatientChargesHdDao(ctx);
            ConsultVisitItemPackageDao consultVisitItemPackageDao = new ConsultVisitItemPackageDao(ctx);
            try
            {
                string param = hdnSelectedAppointmentRequestID.Value;

                List<AppointmentRequest> lstAppointmentRequest = BusinessLayer.GetAppointmentRequestList(string.Format("AppointmentRequestID IN({0})", param));
                if (lstAppointmentRequest.Count > 0)
                {
                    bool isDummy = false;
                    foreach (AppointmentRequest row in lstAppointmentRequest)
                    {
                        int CountIsDummy = MCUOrderGroupList.Where(p => p.IsParamedicDummy == true && p.VisitID == row.VisitID).Count();
                        if (CountIsDummy > 0)
                        {
                            string Json = JsonConvert.SerializeObject(MCUOrderGroupList.Where(p => p.IsParamedicDummy == true && p.VisitID == row.VisitID).ToList());
                            isDummy = true;
                            break;
                        }
                        DateTime dateNow = DateTime.Now;
                        string timeNow = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                        List<MCUOrder> lstMCUOrder = MCUOrderGroupList.Where(t => t.IsConfirm).GroupBy(x => x.HealthcareServiceUnitID).Select(x => x.First()).ToList();
                        ConsultVisit entityVisit = BusinessLayer.GetConsultVisit(Convert.ToInt32(row.VisitID));
                        foreach (MCUOrder healthcareServiceUnit in lstMCUOrder)
                        {
                            List<MCUOrder> lstSelectedItemServiceDt = MCUOrderGroupList.Where(p => p.HealthcareServiceUnitID == healthcareServiceUnit.HealthcareServiceUnitID && p.VisitID == row.VisitID).ToList();
                            if (lstSelectedItemServiceDt.Count > 0)
                            {
                                if (healthcareServiceUnit.DepartmentID == Constant.Facility.DIAGNOSTIC)
                                {
                                    SaveTestOrder(ctx, entityVisit, lstSelectedItemServiceDt, dateNow, timeNow);
                                }
                                else
                                {
                                    SaveServiceOrder(ctx, entityVisit, lstSelectedItemServiceDt, dateNow, timeNow);
                                }
                            }

                        }
                        string filterExpression = string.Format("VisitID = '{0}' AND IsDeleted = 0 AND GCItemDetailStatus = '{1}'", row.VisitID, Constant.TransactionStatus.OPEN);
                        List<ConsultVisitItemPackage> lstEntityItemPackage = BusinessLayer.GetConsultVisitItemPackageList(filterExpression, ctx);
                        string lstItemPackageID = string.Join(",", lstEntityItemPackage.Select(x => x.ItemID));
                        foreach (ConsultVisitItemPackage entity in lstEntityItemPackage)
                        {
                            entity.GCItemDetailStatus = Constant.TransactionStatus.PROCESSED;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            consultVisitItemPackageDao.Update(entity);
                        }
                        InsertMCUDefaultCharges(lstEntityItemPackage, entityVisit, ctx);
                    }

                    if (isDummy)
                    {
                        result = "GenerateOrder|fail|Maaf, harap cek kembali dokter pelaksana untuk masing-masing item. Terdapat data dengan dokter dummy, silahkan ubah ke dokter yang dituju.";
                        ctx.RollBackTransaction();
                    }
                    else
                    {
                        result = "GenerateOrder|success|Success";
                        ctx.CommitTransaction();
                    }
                }
                else
                {
                    result = "GenerateOrder|fail|Silahkan dipilih nomor registrasi yang akan di proses";
                    ctx.RollBackTransaction();
                }

            }
            catch (Exception ex)
            {
                Helper.InsertErrorLog(ex);
                result = string.Format("GenerateOrder|fail|{0}", ex.Message);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
        }
        #endregion
    }
}