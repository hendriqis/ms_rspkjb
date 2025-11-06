<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="JournalRecalculationBalance.aspx.cs" Inherits="QIS.Medinfras.Web.Accounting.Program.JournalRecalculationBalance" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRecalculate" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrecalculate.png")%>' alt="" /><div>
            <%=GetLabel("Recalculate")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#<%=btnRecalculate.ClientID %>').click(function () {
                showToastConfirmation('Apakah Anda Yakin Proses Rekalkulasi Saldo di Periode ini?', function (result) {
                    if (result) {
                        cbpProcess.PerformCallback('recalculate');
                    }
                });
            });
        });

        $('#<%=txtPeriodYear.ClientID %>').live('change', function () {
            var periodYear = parseInt($(this).val());
            if (periodYear > 1900 && periodYear < 2999) {
            } else {
                $(this).val($('#<%=hdnTodayYear.ClientID %>').val());
            }
        });

        $('#<%=txtPeriodMonth.ClientID %>').live('change', function () {
            var periodMonth = parseInt($(this).val());
            if (periodMonth >= 1 && periodMonth <= 12) {
            } else {
                $(this).val($('#<%=hdnTodayMonth.ClientID %>').val());
            }
        });

        function onCbpProcesEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[1] == 'fail')
                showToast('Process Failed', 'Error Message : ' + param[2]);
            else
                showToast('Process Success', 'Proses Rekalkulasi Saldo Berhasil Dilakukan');

            onLoadObject();
            hideLoadingPanel();
        }
        
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnTodayYear" runat="server" />
    <input type="hidden" value="" id="hdnTodayMonth" runat="server" />
    <table>
        <colgroup>
            <col style="width: 50%" />
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td align="left" style="vertical-align: middle">
                <table>
                    <colgroup>
                        <col style="width: 100px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel" style="padding-top: 3px">
                            <label class="tdLabel" style="font-weight:bold; font-size:medium">
                                <%=GetLabel("Tahun")%></label>
                        </td>
                        <td style="padding-top: 3px">
                            <asp:TextBox runat="server" ID="txtPeriodYear" style="text-align:center" MaxLength="4" oninput="this.value = this.value.replace(/[^0-9.]/g, '').replace(/(\..*)\./g, '$1');"/>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="padding-top: 3px">
                            <label class="tdLabel" style="font-weight:bold; font-size:medium">
                                <%=GetLabel("Bulan")%></label>
                        </td>
                        <td style="padding-top: 3px">
                            <asp:TextBox runat="server" ID="txtPeriodMonth" style="text-align:center" MaxLength="2" oninput="this.value = this.value.replace(/[^0-9.]/g, '').replace(/(\..*)\./g, '$1');"/>
                        </td>
                    </tr>
                </table>
            </td>
            <td align="right" style="vertical-align: middle">
                <table>
                    <colgroup>
                        <col style="width: 100px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td style="vertical-align: middle" align="center" class="blink-alert">
                            <img height="70" src='<%= ResolveUrl("~/Libs/Images/Warning.png")%>' alt='' class="blink-alert" />
                        </td>
                        <td align="right">
                            <label id="lblInfoTahun" class="lblWarning" runat="server">
                                <%=GetLabel("Isi periode <b>Tahun</b> dengan 4 digit angka <br />( <i> Contoh = <u>2018</u> / <u>2019</u> / <u>2020</u> </i> )")%></label>
                            <br />
                            <label id="lblInfoBulan" class="lblWarning" runat="server">
                                <%=GetLabel("Isi periode <b>Bulan</b> dengan 2 digit angka 01 s/d 12 <br />( <i> Contoh = <u>08</u> / <u>10</u> / <u>12</u> </i> )")%></label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <dxcp:ASPxCallbackPanel ID="cbpProcess" runat="server" Width="100%" ClientInstanceName="cbpProcess"
        ShowLoadingPanel="false" OnCallback="cbpProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>
