<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPMain.master" AutoEventWireup="true" 
    CodeBehind="MyTodayPatientVisitList.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.MyTodayPatientVisitList" %>

<%@ Register Src="~/Libs/Controls/PatientGrid/GridPatientVisitCtl.ascx" TagName="ctlGrdRegisteredPatient" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#<%=txtRegistrationNo.ClientID %>').change(function () {
                onRefreshGridView();
            });

            $('#lblRefresh.lblLink').click(function () {
                onRefreshGridView();
            });
        });

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
    <div style="padding:15px">
        <div class="pageTitle"><%=GetLabel("My Today Patient Visit")%></div>
        <table class="tblContentArea" style="width:100%">
            <tr>
                <td style="padding:5px;vertical-align:top">
                <fieldset id="fsPatientList">
                    <table class="tblEntryContent" style="width:60%;">
                        <colgroup>
                            <col style="width:25%"/>
                            <col/>
                        </colgroup>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Registration No")%></label></td>
                            <td>
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width:30%"/>
                                        <col style="width:3px"/>
                                        <col/>
                                    </colgroup>
                                    <tr>
                                        <td><asp:TextBox ID="txtRegistrationNo" Width="100%" runat="server" /></td>
                                        <td>&nbsp;</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    </fieldset>
                    <div style="padding:7px 0 0 3px;font-size:0.95em">
                        <%=GetLabel("Halaman Ini Akan")%> <span class="lblLink" id="lblRefresh">[refresh]</span> <%=GetLabel("Setiap")%> <%=GetRefreshGridInterval() %> <%=GetLabel("Menit")%>
                    </div>
                    <uc1:ctlGrdRegisteredPatient runat="server" id="grdRegisteredPatient" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
