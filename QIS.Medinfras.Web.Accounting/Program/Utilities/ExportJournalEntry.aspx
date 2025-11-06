<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPList.master" AutoEventWireup="true" 
CodeBehind="ExportJournalEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Accounting.Program.ExportJournalEntry" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtPeriod.ClientID %>');
            $('#<%=txtPeriod.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            $('#btnProcess').click(function () {
                showToastConfirmation('Proses Export Transaksi Jurnal?', function (result) {
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
                showToast('Process Success', 'Proses Export Transaksi Jurnal Berhasil Dilakukan dengan Nama File ' + param[1]);

            hideLoadingPanel();
        }
    </script>
    <style type="text/css">
        .rblJournalGroup input[type="radio"]            { margin-left: 40px; margin-right: 1px; }
    </style>
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnPageCount" runat="server" value="" />
    <input type="hidden" id="hdnIsEditable" runat="server" value="" />
    <input type="hidden" value="" id="hdnRecordFilterExpression" runat="server" />
    <input type="hidden" value="" id="hdnIsFiscalYear" runat="server" />
    <input type="hidden" value="" id="hdnFiscalYearPeriod" runat="server" />
    <input type="hidden" value="" id="hdnFiscalYearStartMonth" runat="server" />
    <input type="hidden" value="" id="hdnPeriodNo" runat="server" />
    <table>
        <colgroup>
            <col style="width:50%"/>
        </colgroup>
        <tr>
            <td><label class="tdLabel"><%=GetLabel("Periode Transaksi")%></label></td>
            <td><asp:TextBox runat="server" ID="txtPeriod" CssClass="datepicker" /></td>
            <td colspan="2"><input type="button" id="btnProcess" value="Export" /></td>
        </tr>
    </table>

    <div>
        <table width="100%">
            <tr>
                <td>
                    <div style="width: 450px;">
                        <div class="lblComponent" style="text-align:center"><%=GetLabel("Konfigurasi Export Jurnal") %></div>
                        <div style="background-color: #EAEAEA;">
                            <table width="300px" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col width="130px" />
                                    <col width="30px" />
                                </colgroup>
                                <tr>
                                    <td align="right"><%=GetLabel("Metode Export") %></td>
                                    <td align="center">:</td>
                                    <td><div runat="server" id="divImportMethod"></div></td>
                                </tr>
                                <tr>
                                    <td align="right"><%=GetLabel("File Name") %></td>
                                    <td align="center">:</td>
                                    <td><div runat="server" id="divFileName">HIS_GL_yyyymmdd_hh_mm.csv</div></td>
                                </tr>
                            </table>
                        </div>
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
