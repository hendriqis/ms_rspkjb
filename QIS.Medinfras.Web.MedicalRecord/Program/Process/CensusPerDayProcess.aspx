<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="CensusPerDayProcess.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.CensusPerDayProcess" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnProcess" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Process")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtPeriod.ClientID %>');
            $('#<%=txtPeriod.ClientID %>').datepicker('option', 'maxDate', '-1');

            $('#<%=btnProcess.ClientID %>').click(function () {
                showToastConfirmation('Lanjutkan Proses Perhitungan Sensus Harian?', function (result) {
                    if (result) {
                        cbpProcess.PerformCallback();
                    }
                });
            });
        });
        function onCbpProcesEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'fail')
                showToast('Process Failed', 'Error Message : ' + param[1]);
            else
                showToast('Process Success', 'Proses Sensus Pasien Berhasil Dilakukan');

            hideLoadingPanel();
        }
    </script>
    <style type="text/css">
        .rblJournalGroup input[type="radio"]
        {
            margin-left: 40px;
            margin-right: 1px;
        }
    </style>
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnPageCount" runat="server" value="" />
    <input type="hidden" id="hdnIsEditable" runat="server" value="" />
    <input type="hidden" value="" id="hdnRecordFilterExpression" runat="server" />
    <input type="hidden" value="" id="hdnIsFiscalYear" runat="server" />
    <input type="hidden" value="" id="hdnFiscalYearPeriod" runat="server" />
    <input type="hidden" value="" id="hdnFiscalYearStartMonth" runat="server" />
    <input type="hidden" value="" id="hdnPeriodNo" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <table>
        <colgroup>
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td>
                <label class="tdLabel">
                    <%=GetLabel("Tanggal Process")%></label>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtPeriod" CssClass="datepicker" />
            </td>
        </tr>
    </table>
    <div style="padding-top: 30px">
        <table width="30%">
            <tr>
                <td>
                    <div class="lblComponent" style="background-color: #2c3e50; font-size: 18px">
                        <font color="white">
                            <%=GetLabel("Informasi Sensus Terakhir") %></font></div>
                    <div style="background-color: #dcdada;">
                        <table width="100%" cellpadding="0" cellspacing="0">
                            <colgroup>
                                <col width="150px" />
                                <col width="25px" />
                                <col />
                            </colgroup>
                            <tr style="padding: 5px">
                                <td>
                                    <%=GetLabel("Proses Sensus Terakhir")%>
                                </td>
                                <td align="center">
                                    :
                                </td>
                                <td>
                                    <div runat="server" id="divCensusDate">
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <hr style="margin: 10 10 10 10;" />
    <div class="lblComponent" style="background-color: #fffa9a; font-size: 14px; font-weight: bold">
        <font color="black">
            <%=GetLabel("Informasi Daftar Registrasi yang Proses Pulang Setelah Proses Sensus Terakhir") %></font></div>
    <div style="padding-top: 20px; height: 400px; overflow-y: auto">
        <table width="100%">
            <tr>
                <td>
                    <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                        position: relative;">
                        <input type="hidden" value="0" id="hdnDisplayCount" runat="server" />
                        <asp:GridView ID="grdView" runat="server" CssClass="grdNormal notAllowSelect" AutoGenerateColumns="false"
                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="RegistrationID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:BoundField DataField="RegistrationNo" HeaderText="No Registrasi" ItemStyle-HorizontalAlign="Left"
                                    HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="cfPatientName" HeaderText="Nama Pasien" ItemStyle-HorizontalAlign="Left"
                                    HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="DepartmentID" HeaderText="Department" HeaderStyle-Width="150px"
                                    ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="ServiceUnitName" HeaderText="Unit Pelayanan" HeaderStyle-Width="240px"
                                    ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="cfRegistrationDateTime" HeaderText="Tgl/Jam Registrasi"
                                    HeaderStyle-Width="140px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="cfDischargeDateTime" HeaderText="Tgl/Jam Pulang" HeaderStyle-Width="140px"
                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="cfActualDischargeDateTime" HeaderText="Tgl/Jam Proses Pulang"
                                    HeaderStyle-Width="140px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("No Data To Display")%>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
    <dxcp:ASPxCallbackPanel ID="cbpProcess" runat="server" Width="100%" ClientInstanceName="cbpProcess"
        ShowLoadingPanel="false" OnCallback="cbpProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>
