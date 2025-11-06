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
using System.Data;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class TestOrderEntryQuickPicksCtl : BasePagePatientPageEntryCtl
    {
        private void InitializeListMatrix()
        {
            string filterExpression = "";
            string healthcareServiceUnitID = hdnParam.Value.Split('|')[4];

            hdnPopupHealthcareServiceUnitID.Value = healthcareServiceUnitID;

            if (hdnTestOrderID.Value != "")
                filterExpression = string.Format("HealthcareServiceUnitID = {0} AND ItemID NOT IN (SELECT ItemID FROM TestOrderDt WHERE TestOrderID = {1}) AND IsDeleted = 0 AND ISNULL(GCItemStatus,'') != '{2}'", healthcareServiceUnitID, hdnTestOrderID.Value, Constant.ItemStatus.IN_ACTIVE);
            else
                filterExpression = string.Format("HealthcareServiceUnitID = {0} AND IsDeleted = 0 AND ISNULL(GCItemStatus,'') != '{1}'", healthcareServiceUnitID, Constant.ItemStatus.IN_ACTIVE);

            filterExpression += " AND IsTestItem = 1  ORDER BY ItemName1 ASC";

            List<vServiceUnitItem> ListAvailableItem = BusinessLayer.GetvServiceUnitItemList(filterExpression);
            List<vTestOrderDt> ListSelectedItem = BusinessLayer.GetvTestOrderDtList("1 = 0");

            ListAvailable = (from p in ListAvailableItem
                             select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1 }).ToList();

            ListSelected = (from p in ListSelectedItem
                            select new CMatrix { IsChecked = false, ID = p.ItemID.ToString(), Name = p.ItemName1 }).ToList();
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            TestOrderHdDao entityHdDao = new TestOrderHdDao(ctx);
            TestOrderDtDao entityDao = new TestOrderDtDao(ctx);
            ItemMasterDao entityItemMasterDao = new ItemMasterDao(ctx);
            bool result = false;
            try
            {
                TestOrderHd entityHd = null;
                if (hdnTestOrderID.Value == "")
                {
                    //TestOrderID|ParamedicID|TestOrderDate|TestOrderTime|HealthcareServiceUnitID|VisitID|ServiceUnitID|realizationDate|realizationTime|gcToBePerformed|isCITO
                    string[] param = hdnParam.Value.Split('|'); 

                    entityHd = new TestOrderHd();
                    entityHd.HealthcareServiceUnitID = Convert.ToInt32(param[4]);
                    entityHd.FromHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                    entityHd.VisitHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                    entityHd.ParamedicID = Convert.ToInt32(param[1]);
                    entityHd.VisitID = AppSession.RegisteredPatient.VisitID;
                    entityHd.TestOrderDate = Helper.GetDatePickerValue(param[2]);
                    entityHd.TestOrderTime = param[3];
                    entityHd.GCToBePerformed = param[9];
                    entityHd.ScheduledDate = Helper.GetDatePickerValue(param[7]);
                    entityHd.ScheduledTime = param[8];
                    entityHd.IsCITO = param[10] == "true" ? true : false;
                    entityHd.GCOrderStatus = Constant.OrderStatus.OPEN;
                    entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;

                    if (hdnPopupHealthcareServiceUnitID.Value == AppSession.ImagingServiceUnitID)
                        entityHd.TransactionCode = Constant.TransactionCode.IMAGING_TEST_ORDER;
                    else if (hdnPopupHealthcareServiceUnitID.Value == AppSession.LaboratoryServiceUnitID)
                        entityHd.TransactionCode = Constant.TransactionCode.LABORATORY_TEST_ORDER;
                    else if (hdnPopupHealthcareServiceUnitID.Value == AppSession.RT0001)
                        entityHd.TransactionCode = Constant.TransactionCode.RADIOTHERAPHY_TEST_ORDER;                    
                    else
                        entityHd.TransactionCode = Constant.TransactionCode.OTHER_TEST_ORDER;


                    entityHd.TestOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.TestOrderDate, ctx);
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityHd.CreatedBy = AppSession.UserLogin.UserID;
                    entityHd.TestOrderID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
                }
                else
                {
                    entityHd = entityHdDao.Get(Convert.ToInt32(hdnTestOrderID.Value));
                }
                retval = entityHd.TestOrderID.ToString();

                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        TestOrderDt entity = new TestOrderDt();
                        entity.TestOrderID = entityHd.TestOrderID;
                        entity.ItemID = Int32.Parse(row.ID);
                        if (cboDiagnose.Value != null)
                            entity.DiagnoseID = cboDiagnose.Value.ToString();   
                        entity.GCTestOrderStatus = Constant.TestOrderStatus.OPEN;
                        entity.ItemQty = 1;
                        entity.ItemUnit = entityItemMasterDao.Get(entity.ItemID).GCItemUnit;
                        entity.Remarks = txtRemarks.Text;
                        entity.CreatedBy = AppSession.UserLogin.UserID;
                        entityDao.Insert(entity);
                    }
                }
                ctx.CommitTransaction();
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        protected int PageCountAvailable = 1;
        protected int PageCountSelected = 1;

        public override void InitializeDataControl(string queryString)
        {
            IsAdd = true;

            List<PatientDiagnosis> lstDiagnose = BusinessLayer.GetPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID));
            Methods.SetComboBoxField<PatientDiagnosis>(cboDiagnose, lstDiagnose, "DiagnosisText", "DiagnoseID");

            ListProceedEntity.Clear();
            hdnParam.Value = queryString;
            hdnTestOrderID.Value = queryString.Split('|')[0];

            InitializeListMatrix();

            BindGridAvailable(1, true, ref PageCountAvailable);
            BindGridSelected(1, true, ref PageCountSelected);
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(cboDiagnose, new ControlEntrySetting(true, true, false));
        }

        #region Available
        private void BindGridAvailable(int pageIndex, bool isCountPageCount, ref int pageCount, List<string> listCheckedAvailable = null)
        {
            List<CMatrix> lstEntity = ListAvailable.Where(p => p.Name.Contains(hdnAvailableSearchText.Value)).ToList();
            if (isCountPageCount)
            {
                pageCount = Helper.GetPageCount(lstEntity.Count, Constant.GridViewPageSize.GRID_MATRIX);
            }
            List<CMatrix> lst = lstEntity.Skip((pageIndex - 1) * 10).Take(10).ToList();
            foreach (CMatrix mtx in lst)
            {
                if (listCheckedAvailable != null && listCheckedAvailable.Contains(mtx.ID.ToString()))
                {
                    mtx.IsChecked = true;
                    listCheckedAvailable.Remove(mtx.ID.ToString());
                }
                else
                    mtx.IsChecked = false;
            }

            grdAvailable.DataSource = lst;
            grdAvailable.DataBind();
        }

        protected void cbpMatrixAvailable_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            List<string> listCheckedAvailable = hdnCheckedAvailable.Value.Split(';').ToList();
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    string[] newCheckedAvailable = param[2].Split(';');
                    foreach (string a in newCheckedAvailable)
                    {
                        if (a != "")
                            listCheckedAvailable.Add(a);
                    }

                    BindGridAvailable(Convert.ToInt32(param[1]), false, ref pageCount, listCheckedAvailable);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridAvailable(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpCheckedAvailable"] = string.Join(";", listCheckedAvailable.ToArray());
        }
        #endregion

        #region Selected
        private void BindGridSelected(int pageIndex, bool isCountPageCount, ref int pageCount, List<string> listCheckedSelected = null)
        {
            List<CMatrix> lstEntity = ListSelected.Where(p => p.Name.Contains(hdnSelectedSearchText.Value)).ToList();
            if (isCountPageCount)
            {
                pageCount = Helper.GetPageCount(lstEntity.Count, Constant.GridViewPageSize.GRID_MATRIX);
            }
            List<CMatrix> lst = lstEntity.Skip((pageIndex - 1) * 10).Take(10).ToList();
            foreach (CMatrix mtx in lst)
            {
                if (listCheckedSelected != null && listCheckedSelected.Contains(mtx.ID.ToString()))
                {
                    mtx.IsChecked = true;
                    listCheckedSelected.Remove(mtx.ID.ToString());
                }
                else
                    mtx.IsChecked = false;
            }

            grdSelected.DataSource = lst;
            grdSelected.DataBind();
        }

        protected void cbpMatrixSelected_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            List<string> listCheckedSelected = hdnCheckedSelected.Value.Split(';').ToList();
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    string[] newCheckedSelected = param[2].Split(';');
                    foreach (string a in newCheckedSelected)
                    {
                        if (a != "")
                            listCheckedSelected.Add(a);
                    }

                    BindGridSelected(Convert.ToInt32(param[1]), false, ref pageCount, listCheckedSelected);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridSelected(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpCheckedSelected"] = string.Join(";", listCheckedSelected.ToArray());

        }
        #endregion


        protected void cbpMatrixProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string[] param = e.Parameter.Split('|');
            string result = param[0] + "|";
            if (param[0] == "right")
            {
                List<string> listCheckedAvailable = hdnCheckedAvailable.Value.Split(';').ToList();
                string[] newCheckedAvailable = param[1].Split(';');
                foreach (string a in newCheckedAvailable)
                {
                    if (a != "")
                        listCheckedAvailable.Add(a);
                }

                foreach (string value in listCheckedAvailable)
                {
                    if (value != "")
                    {
                        ProceedEntity obj = ListProceedEntity.FirstOrDefault(p => p.ID == value.ToString());
                        if (obj != null)
                            ListProceedEntity.Remove(obj);
                        else
                        {
                            ProceedEntity proceedEntity = new ProceedEntity();
                            proceedEntity.ID = value.ToString();
                            proceedEntity.Status = ProceedEntity.ProceedEntityStatus.Add;
                            ListProceedEntity.Add(proceedEntity);
                        }

                        CMatrix removeObj = ListAvailable.FirstOrDefault(p => p.ID.ToString() == value);
                        if (removeObj != null)
                        {
                            ListSelected.Add(removeObj);
                            ListAvailable.Remove(removeObj);
                        }
                    }
                }

                ListSelected = ListSelected.OrderBy(p => p.Name).ToList();
            }
            else if (param[0] == "left")
            {
                List<string> listCheckedSelected = hdnCheckedSelected.Value.Split(';').ToList();
                string[] newCheckedSelected = param[1].Split(';');
                foreach (string a in newCheckedSelected)
                {
                    if (a != "")
                        listCheckedSelected.Add(a);
                }

                foreach (string value in listCheckedSelected)
                {
                    if (value != "")
                    {
                        ProceedEntity obj = ListProceedEntity.FirstOrDefault(p => p.ID == value.ToString());
                        if (obj != null)
                            ListProceedEntity.Remove(obj);
                        else
                        {
                            ProceedEntity proceedEntity = new ProceedEntity();
                            proceedEntity.ID = value.ToString();
                            proceedEntity.Status = ProceedEntity.ProceedEntityStatus.Remove;
                            ListProceedEntity.Add(proceedEntity);
                        }

                        CMatrix removeObj = ListSelected.FirstOrDefault(p => p.ID.ToString() == value);
                        if (removeObj != null)
                        {
                            ListAvailable.Add(removeObj);
                            ListSelected.Remove(removeObj);
                        }
                    }
                }

                ListAvailable = ListAvailable.OrderBy(p => p.Name).ToList();
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private const string SESSION_NAME_SELECTED_ENTITY = "SelectedEntity";
        private const string SESSION_NAME_AVAILABLE_ENTITY = "AvailableEntity";
        private const string SESSION_PROCEED_ENTITY = "ProceedEntity";

        #region Matrix
        public static List<CMatrix> ListSelected
        {
            get
            {
                if (HttpContext.Current.Session[SESSION_NAME_SELECTED_ENTITY] == null) HttpContext.Current.Session[SESSION_NAME_SELECTED_ENTITY] = new List<CMatrix>();
                return (List<CMatrix>)HttpContext.Current.Session[SESSION_NAME_SELECTED_ENTITY];
            }
            set
            {
                HttpContext.Current.Session[SESSION_NAME_SELECTED_ENTITY] = value;
            }
        }
        public static List<CMatrix> ListAvailable
        {
            get
            {
                if (HttpContext.Current.Session[SESSION_NAME_AVAILABLE_ENTITY] == null) HttpContext.Current.Session[SESSION_NAME_AVAILABLE_ENTITY] = new List<CMatrix>();
                return (List<CMatrix>)HttpContext.Current.Session[SESSION_NAME_AVAILABLE_ENTITY];
            }
            set
            {
                HttpContext.Current.Session[SESSION_NAME_AVAILABLE_ENTITY] = value;
            }
        }

        private static List<ProceedEntity> ListProceedEntity
        {
            get
            {
                if (HttpContext.Current.Session[SESSION_PROCEED_ENTITY] == null) HttpContext.Current.Session[SESSION_PROCEED_ENTITY] = new List<ProceedEntity>();
                return (List<ProceedEntity>)HttpContext.Current.Session[SESSION_PROCEED_ENTITY];
            }
            set
            {
                HttpContext.Current.Session[SESSION_PROCEED_ENTITY] = value;
            }
        }

        public static List<CMatrix> SelectListAvailableEntity()
        {
            return ListAvailable;
        }
        public static List<CMatrix> SelectListSelectedEntity()
        {
            return ListSelected;
        }
        #endregion
    }
}