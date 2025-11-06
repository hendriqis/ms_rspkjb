using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class UserInRoleEntryCtl : BaseViewPopupCtl
    {
        protected int PageCountAvailable = 1;
        protected int PageCountSelected = 1;

        public override void InitializeDataControl(string param)
        {
            UserRole ur = BusinessLayer.GetUserRole(Convert.ToInt32(param));
            txtHeader.Text = ur.RoleName;

            ListProceedEntity.Clear();
            hdnParam.Value = param;

            List<Healthcare> lstHealthcare = BusinessLayer.GetHealthcareList("");
            Methods.SetComboBoxField<Healthcare>(ddlHealthcare, lstHealthcare, "HealthcareName", "HealthcareID");

            hdnSelectedHealthcare.Value = lstHealthcare[0].HealthcareID;

            ListAllAvailable = BusinessLayer.GetvUserList("IsDeleted = 0");
            List<vUserInRole> lstSelected = BusinessLayer.GetvUserInRoleList(string.Format("RoleID = {0}", param));
            ListSelected = (from p in lstSelected
                            select new CMatrix { IsChecked = false, UserID = p.UserID, UserName = p.UserName, HealthcareID = p.HealthcareID }).ToList();

            BindGridAvailable(1, true, ref PageCountAvailable);
            BindGridSelected(1, true, ref PageCountSelected);
        }

        private bool SaveMatrix(string queryString, ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            bool result = false;
            try
            {
                int RoleID = Convert.ToInt32(queryString);
                UserInRoleDao entityDao = new UserInRoleDao(ctx);
                foreach (ProceedEntity row in ListProceedEntity)
                {
                    if (row.Status == ProceedEntity.ProceedEntityStatus.Add)
                    {
                        UserInRole entity = new UserInRole();
                        entity.RoleID = RoleID;
                        entity.UserID = row.ID;
                        entity.HealthcareID = row.HealthcareID;
                        entityDao.Insert(entity);
                    }
                    else
                        entityDao.Delete(row.ID, row.HealthcareID, RoleID);
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

        #region Available
        private void BindGridAvailable(int pageIndex, bool isCountPageCount, ref int pageCount, List<string> listCheckedAvailable = null)
        {
            ListAvailable = (from item1 in ListAllAvailable
                             where !(ListSelected.Any(item2 => item2.UserID == item1.UserID && item2.HealthcareID == hdnSelectedHealthcare.Value))
                            select new CMatrix { IsChecked = false, UserID = item1.UserID, UserName = item1.UserName, HealthcareID = hdnSelectedHealthcare.Value }).ToList();

            List<CMatrix> lstEntity = ListAvailable.Where(p => p.UserName.Contains(hdnAvailableSearchText.Value)).ToList();
            if (isCountPageCount)
            {
                pageCount = Helper.GetPageCount(lstEntity.Count, Constant.GridViewPageSize.GRID_MATRIX);
            }
            List<CMatrix> lst = lstEntity.Skip((pageIndex - 1) * 10).Take(10).ToList();
            foreach (CMatrix mtx in lst)
            {
                if (listCheckedAvailable != null && listCheckedAvailable.Contains(mtx.UserID.ToString()))
                {
                    mtx.IsChecked = true;
                    listCheckedAvailable.Remove(mtx.UserID.ToString());
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
            List<CMatrix> lstEntity = ListSelected.Where(p => p.HealthcareID == hdnSelectedHealthcare.Value && p.UserName.Contains(hdnSelectedSearchText.Value)).ToList();
            if (isCountPageCount)
            {
                pageCount = Helper.GetPageCount(lstEntity.Count, Constant.GridViewPageSize.GRID_MATRIX);
            }
            List<CMatrix> lst = lstEntity.Skip((pageIndex - 1) * 10).Take(10).ToList();
            foreach (CMatrix mtx in lst)
            {
                if (listCheckedSelected != null && listCheckedSelected.Contains(mtx.UserID.ToString()))
                {
                    mtx.IsChecked = true;
                    listCheckedSelected.Remove(mtx.UserID.ToString());
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
            if (param[0] == "rightAll")
            {
                List<CMatrix> lst = ListAvailable.Where(p => p.HealthcareID == hdnSelectedHealthcare.Value && p.UserName.Contains(hdnAvailableSearchText.Value)).ToList();
                foreach (CMatrix row in lst)
                {
                    ListSelected.Add(row);

                    ProceedEntity obj = ListProceedEntity.FirstOrDefault(p => p.ID == Convert.ToInt32(row.UserID));
                    if (obj != null)
                        ListProceedEntity.Remove(obj);
                    else
                    {
                        ProceedEntity proceedEntity = new ProceedEntity();
                        proceedEntity.ID = row.UserID;
                        proceedEntity.HealthcareID = hdnSelectedHealthcare.Value;
                        proceedEntity.Status = ProceedEntity.ProceedEntityStatus.Add;
                        ListProceedEntity.Add(proceedEntity);
                    }
                }
                ListSelected = ListSelected.OrderBy(p => p.UserName).ToList();
            }
            else if (param[0] == "right")
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
                        ProceedEntity obj = ListProceedEntity.FirstOrDefault(p => p.ID == Convert.ToInt32(value) && p.HealthcareID == hdnSelectedHealthcare.Value);
                        if (obj != null)
                            ListProceedEntity.Remove(obj);
                        else
                        {
                            ProceedEntity proceedEntity = new ProceedEntity();
                            proceedEntity.ID = Convert.ToInt32(value);
                            proceedEntity.HealthcareID = hdnSelectedHealthcare.Value;
                            proceedEntity.Status = ProceedEntity.ProceedEntityStatus.Add;
                            ListProceedEntity.Add(proceedEntity);
                        }

                        CMatrix removeObj = ListAvailable.FirstOrDefault(p => p.UserID == Convert.ToInt32(value) && p.HealthcareID == hdnSelectedHealthcare.Value);
                        if (removeObj != null)
                            ListSelected.Add(removeObj);
                    }
                }

                ListSelected = ListSelected.OrderBy(p => p.UserName).ToList();
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
                        ProceedEntity obj = ListProceedEntity.FirstOrDefault(p => p.ID == Convert.ToInt32(value) && p.HealthcareID == hdnSelectedHealthcare.Value);
                        if (obj != null)
                            ListProceedEntity.Remove(obj);
                        else
                        {
                            ProceedEntity proceedEntity = new ProceedEntity();
                            proceedEntity.ID = Convert.ToInt32(value);
                            proceedEntity.HealthcareID = hdnSelectedHealthcare.Value;
                            proceedEntity.Status = ProceedEntity.ProceedEntityStatus.Remove;
                            ListProceedEntity.Add(proceedEntity);
                        }

                        CMatrix removeObj = ListSelected.FirstOrDefault(p => p.UserID == Convert.ToInt32(value));
                        if (removeObj != null)
                            ListSelected.Remove(removeObj);
                    }
                }

                ListAvailable = ListAvailable.OrderBy(p => p.UserName).ToList();
            }
            else if (param[0] == "leftAll")
            {
                List<CMatrix> lst = ListSelected.Where(p => p.HealthcareID == hdnSelectedHealthcare.Value && p.UserName.Contains(hdnSelectedSearchText.Value)).ToList();
                foreach (CMatrix row in lst)
                {
                    ProceedEntity obj = ListProceedEntity.FirstOrDefault(p => p.ID == row.UserID);
                    if (obj != null)
                        ListProceedEntity.Remove(obj);
                    else
                    {
                        ProceedEntity proceedEntity = new ProceedEntity();
                        proceedEntity.ID = row.UserID;
                        proceedEntity.HealthcareID = hdnSelectedHealthcare.Value;
                        proceedEntity.Status = ProceedEntity.ProceedEntityStatus.Remove;
                        ListProceedEntity.Add(proceedEntity);
                    }
                }
                ListSelected.RemoveAll(x => x.HealthcareID == hdnSelectedHealthcare.Value && x.UserName.Contains(hdnSelectedSearchText.Value));
            }
            else if (param[0] == "save")
            {
                string errMessage = "";
                if (SaveMatrix(hdnParam.Value, ref errMessage))
                    result += "success";
                else
                    result += "fail|" + errMessage;
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        public class CMatrix
        {
            public bool IsChecked { get; set; }
            public Int32 UserID { get; set; }
            public String UserName { get; set; }
            public String HealthcareID { get; set; }
        }

        private const string SESSION_NAME_SELECTED_ENTITY = "SelectedEntityUserRole";
        private const string SESSION_NAME_AVAILABLE_ENTITY = "AvailableEntityUserRole";
        private const string SESSION_NAME_ALL_AVAILABLE_ENTITY = "AllAvailableEntityUserRole";
        private const string SESSION_PROCEED_ENTITY = "ProceedEntity";

        #region Matrix
        private class ProceedEntity
        {
            private Int32 _ID;
            private String _HealthcareID;
            private ProceedEntityStatus _Status;

            public Int32 ID
            {
                get { return _ID; }
                set { _ID = value; }
            }

            public String HealthcareID
            {
                get { return _HealthcareID; }
                set { _HealthcareID = value; }
            }

            public ProceedEntityStatus Status
            {
                get { return _Status; }
                set { _Status = value; }
            }

            public enum ProceedEntityStatus
            {
                Remove = 0,
                Add = 1
            }
        }

        public class CEntity
        {
            private string _ID;

            public string ID
            {
                get { return _ID; }
                set { _ID = value; }
            }
            private string _Name;

            public string Name
            {
                get { return _Name; }
                set { _Name = value; }
            }
        }

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
        public static List<vUser> ListAllAvailable
        {
            get
            {
                if (HttpContext.Current.Session[SESSION_NAME_ALL_AVAILABLE_ENTITY] == null) HttpContext.Current.Session[SESSION_NAME_ALL_AVAILABLE_ENTITY] = new List<vUser>();
                return (List<vUser>)HttpContext.Current.Session[SESSION_NAME_ALL_AVAILABLE_ENTITY];
            }
            set
            {
                HttpContext.Current.Session[SESSION_NAME_ALL_AVAILABLE_ENTITY] = value;
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
        #endregion
    }
}