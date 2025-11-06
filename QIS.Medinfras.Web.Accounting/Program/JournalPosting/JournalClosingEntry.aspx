<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx.master"
    AutoEventWireup="true" CodeBehind="JournalClosingEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Accounting.Program.JournalClosingEntry" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnClosing" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbapprove.png")%>' alt="" /><div>
            <%=GetLabel("Closing")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#<%=btnClosing.ClientID %>').click(function () {
                showToastConfirmation('Apakah Anda Yakin?<br>Nb: Tidak Bisa dilakukan Batal Closing', function (result) {
                    if (result) {
                        cbpProcess.PerformCallback('closing');
                    }
                });
            });
        });

        function onCbpProcesEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[1] == 'fail')
                showToast('Process Failed', 'Error Message : ' + param[2]);
            else
                showToast('Process Success', 'Proses Closing Jurnal Berhasil Dilakukan');

            onLoadObject();
            hideLoadingPanel();
        }
    </script>
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
            <col style="width: 120px" />
            <col />
        </colgroup>
        <tr>
            <td class="tdLabel" style="padding-top: 3px">
                <label class="tdLabel">
                    <%=GetLabel("Periode")%></label>
            </td>
            <td style="padding-top: 3px">
                <asp:TextBox runat="server" ID="txtPeriod" CssClass="datepicker" ReadOnly="true" />
            </td>
        </tr>
    </table>
    <div style="padding-top: 30px">
        <table width="50%">
            <tr>
                <td>
                    <div class="lblComponent" style="background-color: #2c3e50; font-size: 18px ">
                        <font color="white"><%=GetLabel("Informasi Closing Terakhir")%></font></div>
                    <div style="background-color: #dcdada;">
                        <table width="100%" cellpadding="0" cellspacing="0">
                            <colgroup>
                                <col width="150px" />
                                <col width="25px" />
                                <col />
                            </colgroup>
                            <tr style="padding: 5px">
                                <td>
                                    <%=GetLabel("Closing Terakhir") %>
                                </td>
                                <td align="center">
                                    :
                                </td>
                                <td>
                                    <div runat="server" id="divJournalNo">
                                    </div>
                                </td>
                            </tr>
                            <tr style="padding: 5px">
                                <td>
                                    <%=GetLabel("Periode Closing Terakhir")%>
                                </td>
                                <td align="center">
                                    :
                                </td>
                                <td>
                                    <div runat="server" id="divJournalDate">
                                    </div>
                                </td>
                            </tr>
                            <tr style="outline-style: dotted; outline-width: thin; outline-color: Black; padding: 7px">
                            </tr>
                            <tr style="padding: 5px">
                                <td>
                                    <%=GetLabel("Dibuat Oleh") %>
                                </td>
                                <td align="center">
                                    :
                                </td>
                                <td>
                                    <div runat="server" id="divCreatedBy">
                                    </div>
                                </td>
                            </tr>
                            <tr style="padding: 5px">
                                <td>
                                    <%=GetLabel("Dibuat Pada") %>
                                </td>
                                <td align="center">
                                    :
                                </td>
                                <td>
                                    <div runat="server" id="divCreatedDate">
                                    </div>
                                </td>
                            </tr>
                            <tr style="padding: 5px">
                                <td>
                                    <%=GetLabel("Terakhir Dibuat Oleh") %>
                                </td>
                                <td align="center">
                                    :
                                </td>
                                <td>
                                    <div runat="server" id="divLastUpdatedBy">
                                    </div>
                                </td>
                            </tr>
                            <tr style="padding: 5px">
                                <td>
                                    <%=GetLabel("Terakhir Dibuat Pada")%>
                                </td>
                                <td align="center">
                                    :
                                </td>
                                <td>
                                    <div runat="server" id="divLastUpdatedDate">
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <dxcp:ASPxCallbackPanel ID="cbpProcess" runat="server" Width="100%" ClientInstanceName="cbpProcess"
        ShowLoadingPanel="false" OnCallback="cbpProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>
