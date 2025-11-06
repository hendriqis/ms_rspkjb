<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPMain.master"
    AutoEventWireup="true" CodeBehind="MedicalRecordList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.MedicalRecordList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Src="~/Libs/Controls/PatientGrid/GridMedicalRecordStatusCtl.ascx"
    TagName="ctlGrdMedicalRecordStatus" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#lblRefresh.lblLink').click(function (evt) {
                onRefreshGridView();
            });

            setDatePicker('<%=txtLogDate.ClientID %>');
            $('#<%=txtLogDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            $('#<%=txtLogDate.ClientID %>').change(function () {
                $('#divErrorDetail').html('');
                onRefreshGridView();
            });
        });

        function onCboChanged() {
            onRefreshGridView();
        }

        var interval = parseInt('<%=GetRefreshGridInterval() %>') * 60000;
        var intervalID = window.setInterval(function () {
            onRefreshGridView();
        }, interval);

        function onRefreshGridView() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
                window.clearInterval(intervalID);
                refreshGrdRegisteredPatient();
                intervalID = window.setInterval(function () {
                    onRefreshGridView();
                }, interval);
            }
        }         
    </script>
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <div style="padding: 15px">
        <div class="pageTitle">
            <%=GetLabel("Informasi Berkas Rekam Medis")%></div>
        <table class="tblContentArea" style="width: 100%">
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <fieldset id="fsPatientList">
                        <table>
                            <colgroup>
                                <col style="width: 25%" />
                            </colgroup>
                            <tr>
                                <td width="200px" class="tdLabel">
                                    <label class="lblNormal"><%=GetLabel("Tanggal Proses Berkas RM : ")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLogDate" Width="120px" runat="server" CssClass="datepicker" />
                                </td>
                            </tr>
                        </table>
                        <div style="padding: 7px 0 0 3px; font-size: 0.95em">
                            <%=GetLabel("Halaman Ini Akan")%>
                            <span class="lblLink" id="lblRefresh">[refresh]</span>
                            <%=GetLabel("Setiap")%>
                            <%=GetRefreshGridInterval() %>
                            <%=GetLabel("Menit")%>
                        </div>
                    </fieldset>
                    <uc1:ctlGrdMedicalRecordStatus runat="server" ID="grdInpatientReg" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
