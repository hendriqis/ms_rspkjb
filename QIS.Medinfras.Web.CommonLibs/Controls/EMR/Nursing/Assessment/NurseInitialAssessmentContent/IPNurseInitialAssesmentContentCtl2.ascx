<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IPNurseInitialAssesmentContentCtl2.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.IPNurseInitialAssesmentContentCtl2" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_nurseInitialAssessmentctl2">
    $(function () {
        $('#<%=grdDiagnosisView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            $('#<%=grdDiagnosisView.ClientID %> tr.selected').removeClass('selected');
            $(this).addClass('selected');
        });
        $('#<%=grdDiagnosisView.ClientID %> tr:eq(1)').click();

        $('#<%=grdAllergyView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            $('#<%=grdAllergyView.ClientID %> tr.selected').removeClass('selected');
            $(this).addClass('selected');
        });
        $('#<%=grdAllergyView.ClientID %> tr:eq(1)').click();

        registerCollapseExpandHandler();
    });

    var pageCount = parseInt('<%=gridDiagnosisPageCount %>');
    $(function () {
        setPaging($("#diagnosisPaging"), pageCount, function (page) {
            cbpDiagnosisView.PerformCallback('changepage|' + page);
        });
    });

    function onCbpDiagnosisViewEndCallback(s) {
        var param = s.cpResult.split('|');
        var isMainDiagnosisExists = s.cpRetval;

        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdDiagnosisView.ClientID %> tr:eq(1)').click();

            setPaging($("#diagnosisPaging"), pageCount, function (page) {
                cbpDiagnosisView.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdDiagnosisView.ClientID %> tr:eq(1)').click();
    }

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
    <%=GetLabel("Diagnosa Pasien")%></h4>
<div class="containerTblEntryContent containerEntryPanel1">
    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 2px;">
        <tr>
            <td>
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpDiagnosisView" runat="server" Width="100%" ClientInstanceName="cbpDiagnosisView"
                        ShowLoadingPanel="false" OnCallback="cbpDiagnosisView_Callback">
                        <ClientSideEvents EndCallback="function(s,e){ onCbpDiagnosisViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent6" runat="server">
                                <asp:Panel runat="server" ID="Panel5" CssClass="pnlContainerGrid" Style="height: 300px">
                                    <asp:GridView ID="grdDiagnosisView" runat="server" CssClass="grdSelected grdPatientPage" 
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" 
                                                ItemStyle-CssClass="keyField" >
                                                <HeaderStyle CssClass="keyField"></HeaderStyle>
                                                <ItemStyle CssClass="keyField"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                <ItemTemplate>
                                                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                    <input type="hidden" value="<%#:Eval("DiagnoseID") %>" bindingfield="DiagnoseID" />
                                                    <input type="hidden" value="<%#:Eval("GCDiagnoseType") %>" bindingfield="GCDiagnoseType" />
                                                    <input type="hidden" value="<%#:Eval("GCDifferentialStatus") %>" bindingfield="GCDifferentialStatus" />
                                                    <input type="hidden" value="<%#:Eval("DiagnosisText") %>" bindingfield="DiagnosisText" />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="hiddenColumn"></HeaderStyle>
                                                <ItemStyle CssClass="hiddenColumn"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <div <%# Eval("cfIsRuledOut").ToString() == "True" ? "Style='text-decoration: line-through'":"" %>>
                                                        <%#: Eval("DifferentialDateInString")%>,
                                                        <%#: Eval("DifferentialTime")%>,                                                                           
                                                        <%#: Eval("ParamedicName")%></div>
                                                    <div <%# Eval("cfIsRuledOut").ToString() == "True" ? "Style='text-decoration: line-through'":"" %>>
                                                        <span Style="color: Blue; font-size: 1.1em">
                                                            <%#: Eval("DiagnosisText")%></span> (<b><%#: Eval("DiagnoseID")%></b>)
                                                    </div>
                                                    <div <%# Eval("cfIsRuledOut").ToString() == "True" ? "Style='text-decoration: line-through'":"" %>>
                                                        <%#: Eval("ICDBlockName")%></div>
                                                    <div <%# Eval("cfIsRuledOut").ToString() == "True" ? "Style='text-decoration: line-through'":"" %>>
                                                        <b>
                                                            <%#: Eval("DiagnoseType")%></b> -
                                                        <%#: Eval("DifferentialStatus")%></div>
                                                    <div <%# Eval("cfIsRuledOut").ToString() == "True" ? "Style='text-decoration: line-through'":"" %>>
                                                        <%#: Eval("Remarks")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                <ItemTemplate>
                                                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                    <input type="hidden" value="<%#:Eval("DiagnoseID") %>" bindingfield="DiagnoseID" />
                                                    <input type="hidden" value="<%#:Eval("DiagnosisText") %>" bindingfield="DiagnosisText" />
                                                    <input type="hidden" value="<%#:Eval("GCDiagnoseType") %>" bindingfield="GCDiagnoseType" />
                                                    <input type="hidden" value="<%#:Eval("ParamedicID") %>" bindingfield="ParamedicID" />
                                                    <input type="hidden" value="<%#:Eval("GCDifferentialStatus") %>" bindingfield="GCDifferentialStatus" />
                                                    <input type="hidden" value="<%#:Eval("MorphologyID") %>" bindingfield="MorphologyID" />
                                                    <input type="hidden" value="<%#:Eval("DifferentialDateInDatePickerFormat") %>" bindingfield="DifferentialDate" />
                                                    <input type="hidden" value="<%#:Eval("DifferentialTime") %>" bindingfield="DifferentialTime" />
                                                    <input type="hidden" value="<%#:Eval("IsChronicDisease") %>" bindingfield="IsChronicDisease" />
                                                    <input type="hidden" value="<%#:Eval("IsFollowUpCase") %>" bindingfield="IsFollowUpCase" />
                                                    <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataRowStyle CssClass="trEmpty">
                                        </EmptyDataRowStyle>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Belum ada informasi diagnosa untuk pasien ini") %>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="diagnosisPaging">
                            </div>
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
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
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" 
                                                ItemStyle-CssClass="keyField" >
                                            </asp:BoundField>
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
                                                HeaderStyle-HorizontalAlign="Left" >
                                            </asp:BoundField>
                                            <asp:BoundField DataField="AllergySource" HeaderText="Finding Source" HeaderStyle-Width="150px"
                                                HeaderStyle-HorizontalAlign="Left" >
                                            </asp:BoundField>
                                            <asp:BoundField DataField="DisplayDate" HeaderText="Since" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Left" >
                                            </asp:BoundField>
                                            <asp:BoundField DataField="AllergySeverity" HeaderText="Severity" HeaderStyle-Width="120px"
                                                HeaderStyle-HorizontalAlign="Left" >
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Reaction" HeaderText="Reaction" 
                                                HeaderStyle-HorizontalAlign="Left" >
                                            </asp:BoundField>
                                        </Columns>        
                                        <EmptyDataRowStyle CssClass="trEmpty">
                                        </EmptyDataRowStyle>
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

