<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PathologyAnatomyTestInfoEntryCtl1.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PathologyAnatomyTestInfoEntryCtl1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<script type="text/javascript" id="dxss_NewSurgeryOrderEntryCtl1">
    $(function () {
    });

    function onBeforeSaveRecord(errMessage) {
        var resultFinal = true;
        return resultFinal;
    }
    function onCbpProcessPAEndCallback(s) {
        var hdnNoPA = $('#<%:hdnNoPA.ClientID %>').val();
        $('#<%:txtNoPA.ClientID %>').val(hdnNoPA);

    }
    $('#btnGenerateNoPA').live('click', function (evt) {
        cbpProcessPA.PerformCallback('generateNo');
    });
</script>
<style type="text/css">
    
</style>
<div>
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnVisitID" value="" />
    <input type="hidden" runat="server" id="hdnFromHealthcareServiceUnitID" value="" />
    <input type="hidden" runat="server" id="hdnHealthcareServiceUnitID" value="" />
    <input type="hidden" runat="server" id="hdnTestOrderID" value="" />
    <input type="hidden" runat="server" id="hdnTransactionID" value="" />
    <input type="hidden" runat="server" id="hdnParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnParamedicTeamID" value="" />
    <input type="hidden" runat="server" id="hdnSpecimenDeliveryID" value="" />
    <input type="hidden" runat="server" id="hdnIsAllowFreeTextMode" value="0" />
    <input type="hidden" value="1" id="hdnSpecimenProcessMode" runat="server" />
    <input type="hidden" runat="server" id="hdnOrderDtSpecimenID" value="" />
    <input type="hidden" runat="server" id="hdnEntrySpecimenID" value="" />

    <table class="tblContentArea">
        <colgroup>
            <col style="width: 22%" />
            <col style="width: 78%" />
        </colgroup>
        <tr>
            <td style="vertical-align: top;" colspan="2">
                <div id="divPage1" class="w3-border divPageNavPanelContent w3-animate-left">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 150px" />
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td></td>
                            <td><input type="button" value="Generate Nomor PA" id="btnGenerateNoPA"  /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label class="lblNormal">
                                    <%=GetLabel("No. PA") %></label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtNoPA" runat="server"    />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                            </td>
                            <td colspan="2">
                                 <asp:CheckBox ID="chkIsPATest" Width="150px" runat="server" Text=" Pemeriksaan PA" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label class="lblNormal">
                                    <%=GetLabel("Catatan Makroskopik") %></label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtMacroscopicRemarks" runat="server" Width="99%" TextMode="Multiline" Height="150px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label class="lblNormal">
                                    <%=GetLabel("Lokasi Jaringan") %></label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtSpecimenLocation" runat="server" Width="99%" TextMode="Multiline" Height="150px" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</div>
<div style="display: none">
</div>
 <dxcp:aspxcallbackpanel id="cbpProcessPA" runat="server" width="100%" clientinstancename="cbpProcessPA"
                            showloadingpanel="false" oncallback="cbpProcessPA_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); onCbpProcessPAEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent2" runat="server">
                                    <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGrid" Style="height: 200px">
                                      <input type="hidden" id="hdnNoPA" runat="server" />
                                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"></asp:GridView>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                            </dxcp:aspxcallbackpanel>