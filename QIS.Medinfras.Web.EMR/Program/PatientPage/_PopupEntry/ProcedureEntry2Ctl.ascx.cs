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
    public partial class ProcedureEntry2Ctl : BasePagePatientPageEntryCtl
    {
        private void InitializeListMatrix()
        {
            string filterExpression = "";
            filterExpression = string.Format("ProcedureID NOT IN (SELECT ProcedureID FROM PatientProcedure WHERE VisitID = {0} AND IsDeleted = 0) AND IsDeleted = 0 ORDER BY ProcedureName ASC", AppSession.RegisteredPatient.VisitID);

            List<Procedures> ListAvailableItem = BusinessLayer.GetProceduresList(filterExpression);
            List<vPatientProcedure> ListSelectedItem = BusinessLayer.GetvPatientProcedureList("1 = 0");

            ListAvailable = (from p in ListAvailableItem
                             select new CMatrix { IsChecked = false, ID = p.ProcedureID.ToString(), Name = p.ProcedureName }).ToList();

            ListSelected = (from p in ListSelectedItem
                            select new CMatrix { IsChecked = false, ID = p.ProcedureID.ToString(), Name = p.ProcedureName }).ToList();
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            PatientProcedureDao entityDao = new PatientProcedureDao(ctx);
            bool result = false;
            try
            {
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        PatientProcedure entity = new PatientProcedure();
                        entity.VisitID = AppSession.RegisteredPatient.VisitID;
                        entity.ParamedicID = (int)AppSession.UserLogin.ParamedicID;
                        entity.ProcedureDate = DateTime.Now.Date;
                        entity.ProcedureTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                        entity.ProcedureID = row.ID;
                        entity.IsCreatedBySystem = false;
                        entity.CreatedBy = AppSession.UserLogin.UserID;
                        entityDao.Insert(entity);
                    }
                }
                ctx.CommitTransaction();
                result = true;
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                result = false;
                errMessage = ex.Message;
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

            ListProceedEntity.Clear();
            hdnParam.Value = queryString;

            InitializeListMatrix();

            BindGridAvailable(1, true, ref PageCountAvailable);
            BindGridSelected(1, true, ref PageCountSelected);
        }

        protected override void OnControlEntrySetting()
        {
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