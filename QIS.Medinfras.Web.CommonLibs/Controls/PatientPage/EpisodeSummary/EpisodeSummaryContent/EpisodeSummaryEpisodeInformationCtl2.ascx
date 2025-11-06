<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EpisodeSummaryEpisodeInformationCtl2.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.EpisodeSummaryEpisodeInformationCtl2" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_erpatientstatus1">
    $(function () {
        registerCollapseExpandHandler();
    });

    function onCbpAllergyViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdAllergyView.ClientID %> tr:eq(1)').click();

            setPaging($("#allergyPaging"), pageCount, function (page) {
                cbpAllergyView.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdAllergyView.ClientID %> tr:eq(1)').click();
    }
</script>

<style type="text/css">
    .warnaTeks { color:#AD3400;}
    .warnaHeader { color:#064E73;}    
    li  { list-style-type:none; font-size: 14px;list-style-position:inside;margin:0;padding:0; }   
    .divContent { font-size:14px;}
    .divNotAvailableContent { margin-left:25px; font-size:11px; font-style:italic; color:red}
</style>

<h4 class="w3-blue h4expanded">
    <%=GetLabel("Riwayat Penyakit Dahulu")%></h4>
<div class="containerTblEntryContent">
    <div style="max-height:220px;overflow-y:auto; padding : 5px 0px 5px 0px">
        <table border="0" cellpadding="0" cellspacing="0" style="width:99%">
            <colgroup>
                <col style="width: 150px" />
                <col />
            </colgroup>        
            <tr>
                <td class="tdLabel" valign="top">
                    <label class="lblNormal">
                        <%=GetLabel("Riwayat Penyakit Dahulu")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtMedicalHistory" Width="100%" runat="server" TextMode="MultiLine" Rows="5" ReadOnly="true" />
                </td>
            </tr>
        </table>
    </div>
</div>

<h4 class="w3-blue h4collapsed">
    <%=GetLabel("Riwayat Penggunaan Obat")%></h4>
<div class="containerTblEntryContent">
    <div style="max-height:220px;overflow-y:auto; padding : 5px 0px 5px 0px">
        <table border="0" cellpadding="0" cellspacing="0" style="width:99%">
            <colgroup>
                <col style="width: 150px" />
                <col />
            </colgroup>        
            <tr>
                <td class="tdLabel" valign="top">
                    <label class="lblNormal">
                        <%=GetLabel("Riwayat Penggunaan Obat")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtMedicationHistory" Width="100%" runat="server" TextMode="MultiLine" Rows="5" ReadOnly="true" />
                </td>
            </tr>
        </table>
    </div>
</div>

<h4 class="w3-blue h4collapsed">
    <%=GetLabel("Riwayat Penyakit Keluarga")%></h4>
<div class="containerTblEntryContent">
    <div style="max-height:220px;overflow-y:auto; padding : 5px 0px 5px 0px">
        <table border="0" cellpadding="0" cellspacing="0" style="width:99%">
            <colgroup>
                <col style="width: 150px" />
                <col />
            </colgroup>    
            <tr>
                <td class="tdLabel" valign="top">
                    <label class="lblNormal">
                        <%=GetLabel("Riwayat Penyakit Keluarga")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtFamilyHistory" Width="100%" runat="server" TextMode="MultiLine" Rows="5" ReadOnly="true" />
                </td>
            </tr>
        </table> 
    </div>
</div>

<h4 class="w3-blue h4collapsed">
    <%=GetLabel("Riwayat Alergi")%></h4>
<div class="containerTblEntryContent">
    <div style="max-height:450px;overflow-y:auto; padding : 5px 0px 5px 0px">
        <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px">
            <tr>
                <td>
                    <asp:CheckBox ID="chkIsPatientAllergyExists" runat="server" Text="Tidak ada Alergi"
                        Checked="false" Enabled="false" />
                </td>
            </tr>
            <tr>
                <td>
                    <dxcp:ASPxCallbackPanel ID="cbpAllergyView" runat="server" Width="100%" ClientInstanceName="cbpAllergyView"
                        ShowLoadingPanel="false" OnCallback="cbpAllergyView_Callback">
                        <ClientSideEvents EndCallback="function(s,e){ onCbpAllergyViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent3" runat="server">
                                <asp:Panel runat="server" ID="Panel2" CssClass="pnlContainerGridPatientPage3">
                                    <asp:GridView ID="grdAllergyView" runat="server" CssClass="grdSelected grdPatientPage"
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                <ItemTemplate>
                                                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                    <input type="hidden" value="<%#:Eval("Allergen") %>" bindingfield="Allergen" />
                                                    <input type="hidden" value="<%#:Eval("GCAllergenType") %>" bindingfield="GCAllergenType" />
                                                    <input type="hidden" value="<%#:Eval("GCAllergySource") %>" bindingfield="GCAllergySource" />
                                                    <input type="hidden" value="<%#:Eval("GCAllergySeverity") %>" bindingfield="GCAllergySeverity" />
                                                    <input type="hidden" value="<%#:Eval("KnownDate") %>" bindingfield="KnownDate" />
                                                    <input type="hidden" value="<%#:Eval("Reaction") %>" bindingfield="Reaction" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Allergen" HeaderText="Allergen Name" HeaderStyle-Width="200px"
                                                HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="AllergySource" HeaderText="Finding Source" HeaderStyle-Width="150px"
                                                HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="DisplayDate" HeaderText="Since" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="AllergySeverity" HeaderText="Severity" HeaderStyle-Width="120px"
                                                HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="Reaction" HeaderText="Reaction" HeaderStyle-HorizontalAlign="Left" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada data alergi pasien dalam episode ini")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="allergyPaging">
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</div>

<h4 class="w3-blue h4collapsed">
    <%=GetLabel("Sasaran Asuhan")%></h4>
<div class="containerTblEntryContent">
    <div style="max-height:220px;overflow-y:auto; padding : 5px 0px 5px 0px">
        <table border="0" cellpadding="0" cellspacing="0" style="width:99%">
            <colgroup>
                <col style="width: 150px" />
                <col />
            </colgroup>    
            <tr>
                <td class="tdLabel" valign="top">
                    <label class="lblNormal">
                        <%=GetLabel("Sasaran Asuhan")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtNursingObjectives" Width="100%" runat="server" TextMode="MultiLine" Rows="5" ReadOnly="true" />
                </td>
            </tr>
        </table> 
    </div>
</div>

<h4 class="w3-blue h4collapsed">
    <%=GetLabel("Perencanaan Pulang (Pasien Rawat Inap)")%></h4>
<div class="containerTblEntryContent">
    <table border="0" cellpadding="0" cellspacing="0">
        <colgroup>
            <col style="width:250px" />
            <col style="width:60px" />
            <col />
        </colgroup>
        <tr>
            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                <%=GetLabel("Perkiraan lama hari perawatan") %>
            </td>
            <td>
                <asp:TextBox ID="txtEstimatedLOS" runat="server" Width="60px" CssClass="number" ReadOnly="true" />
            </td>
            <td>
                <asp:RadioButtonList ID="rblEstimatedLOSUnit" runat="server" RepeatDirection="Horizontal" Enabled="false">
                    <asp:ListItem Text=" hari" Value="1" />
                    <asp:ListItem Text=" minggu" Value="0"  />
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>                                    
            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                <%=GetLabel("Rencana Pemulangan Kritis sehingga membutuhkan rencana pemulangan") %>
            </td>
            <td colspan="2">
                <asp:RadioButtonList ID="rblIsNeedDischargePlan" runat="server" RepeatDirection="Horizontal" Enabled="false">
                    <asp:ListItem Text="Ya" Value="1" />
                    <asp:ListItem Text="Tidak" Value="0"  />
                </asp:RadioButtonList>
            </td>
        </tr>
    </table>
</div>
