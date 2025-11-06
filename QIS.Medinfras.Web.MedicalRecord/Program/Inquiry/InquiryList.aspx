<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPMain.master" AutoEventWireup="true" 
CodeBehind="InquiryList.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.InquiryList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<%@ Register Src="~/Libs/Controls/PatientGrid/CheckGridPatientVisitCtl.ascx" TagName="ctlGrdRegisteredPatient" TagPrefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">

    <script type="text/javascript">
        $(function () {
            //tanggal
            setDatePicker('<%=txtRegistrationDate.ClientID %>');
            $('#<%=txtRegistrationDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%=txtRegistrationDate.ClientID %>').change(function (evt) {
                if (IsValid(evt, 'fsPatientList', 'mpPatientList'))
                    onRefreshGridView();
            });
            //refresh
            $('#lblRefresh.lblLink').click(function (evt) {
                if (IsValid(evt, 'fsPatientList', 'mpPatientList'))
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

        function onCboChanged() {
            onRefreshGridView();
        }

    </script>

    <div style="padding:=15px">
        <table class="tblContentArea" style="width:100%">
            <tr>
                <td style="padding:5px; vertical-align:top">
                    <fieldset id="fsPatientList">
                        <table>
                            <%--<colgroup>
                                <col style="width:100%">
                            </colgroup>--%>
                            <tr>
                                <td><%=GetLabel("Tanggal  ")%></label></td><td><asp:TextBox ID="txtRegistrationDate" Width="120px" runat="server" CssClass="datepicker" /></td>
                            <%--</tr>
                            <tr>--%>
                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Klinik")%></label></td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboKlinik" Width="200px" ClientInstanceName="cboKlinik" runat="server">
                                        <ClientSideEvents ValueChanged="function(s,e){ onCboChanged(); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            <%--</tr>
                            <tr>--%>
                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Dokter / Paramedis")%></label></td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboDokter" Width="200px" ClientInstanceName="cboDokter" runat="server">
                                        <ClientSideEvents ValueChanged="function(s,e){ onCboChanged(); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                        </table>
                        <table>
                            <div style="padding: 7px 0 0 3px; font-size: 0.95em">
                                <%=GetLabel("Halaman Ini Akan")%>
                                <span class="lblLink" id="lblRefresh">[refresh]</span>
                                <%=GetLabel("Setiap")%>
                                <%=GetRefreshGridInterval() %> <%=GetLabel("Menit")%>
                            </div>
                        </table>
                    </fieldset>
                    <uc1:ctlGrdRegisteredPatient runat="server" id="grdRegisteredPatient" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>