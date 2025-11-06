<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="EKlaimSendOnlineCollective.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.EKlaimSendOnlineCollective" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnSendKlaimCollective" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("SEND")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            hideLoadingPanel();

            setDatePicker('<%=txtParameterDateFrom.ClientID %>');
            $('#<%=txtParameterDateFrom.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            setDatePicker('<%=txtParameterDateTo.ClientID %>');
            $('#<%=txtParameterDateTo.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
        });

        $('#<%=btnSendKlaimCollective.ClientID %>').click(function (evt) {
            var isBridgingToEKlaim = $('#<%=hdnIsBridgingToEKlaim.ClientID %>').val();
            if (isBridgingToEKlaim == "1") {
                var messageBeforeSendClaim = '<b>' + 'Proses mengirim klaim kolektif ke data center INACBGs.' + '</b>' + ' Lanjutkan ?';
                showToastConfirmation(messageBeforeSendClaim, function (resultConfirmSendClaim) {
                    if (resultConfirmSendClaim) {
                        cbpProcess.PerformCallback('sendclaimcollective');
                    }
                });
            } else {
                showToast('INFORMATION', 'Status bridging dengan e-klaim sedang nonaktif.');
            }
        });

        function onCbpProcesEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == "sendclaimcollective") {
                if (param[1] == 'success') {
                    var respInfo = JSON.parse(param[2]);
                    if (respInfo.metadata.code != "200") {
                        alert(respInfo.metadata.message);
                    } else {
                        alert(respInfo.metadata.message);
                    }
                } else {
                    showToast('Failed', 'Error Message : ' + param[2]);
                }
            }
        }
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnIsBridgingToEKlaim" value="" runat="server" />
    <div>
        <table>
            <colgroup>
                <col style="width: 60%" />
                <col style="width: 40%" />
            </colgroup>
            <tr>
                <td>
                    <table>
                        <colgroup>
                            <col style="width: 150px" />
                            <col style="width: 400px" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Jenis Rawat")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboServiceUnitType" ClientInstanceName="cboServiceUnitType"
                                    Width="305px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Filter Berdasarkan")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboDataType" ClientInstanceName="cboDataType" Width="165px"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Tanggal Parameter")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtParameterDateFrom" Width="120px" CssClass="datepicker" runat="server" />
                                <label class="lblNormal">
                                    <%=GetLabel("s/d")%></label>
                                <asp:TextBox ID="txtParameterDateTo" Width="120px" CssClass="datepicker" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td align="right" style="width: 100%">
                    <table>
                        <colgroup>
                            <col style="width: 90%" />
                            <col style="width: 10%" />
                        </colgroup>
                        <tr>
                            <td align="right" style="width: 100%">
                                <div style="font-size: large; color: Red;">
                                    <b>
                                        <%=GetLabel("Data yang sudah diproses,")%></b>
                                    <br />
                                    <b>
                                        <%=GetLabel("tidak bisa dikembalikan lagi.")%></b>
                                    <br />
                                </div>
                            </td>
                            <td align="right" style="width: 100%">
                                <img height="60" src='<%= ResolveUrl("~/Libs/Images/Warning.png")%>' alt='' />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>    
    <dxcp:ASPxCallbackPanel ID="cbpProcess" runat="server" Width="100%" ClientInstanceName="cbpProcess"
        ShowLoadingPanel="false" OnCallback="cbpProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>
