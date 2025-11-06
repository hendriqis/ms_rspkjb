<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GenerateOrderItemDtCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.MedicalCheckup.Program.GenerateOrderItemDtCtl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_generateorderitemdtctl">
    $('#btnSavePhysicianCtl').die('click');
    $('#btnSavePhysicianCtl').live('click',function () {
        var lstDetailItemID = [];
        var lstPhysicianID = [];
        $('table.grdView > tbody > tr').not(':first').each(function () {
            var detailItemID = $(this).find('.hdnDetailItemID').val();
            var paramedicID = $(this).find('.hdnParamedicID').val();
            lstDetailItemID.push(detailItemID);
            lstPhysicianID.push(paramedicID);
        });
        setSavePhysicianAfterPopupClose(lstDetailItemID.join('|'), lstPhysicianID.join('|'));
    });
    
    function getParamedicFilterExpression() {
        return '';
    }
    
    $('.lblParamedicName.lblLink').live('click', function () {
        var healthcareServiceUnitID = $(this).closest('tr').find('.hdnHealthareServiceUnitID').val();
        $td = $(this).parent();
        var paramedicID = $td.children('.hdnParamedicID').val();
        openSearchDialog('paramedic', getParamedicFilterExpression(), function (value) {
            onTxtParamedicChanged(value, $td);
        });
    });

    function onTxtParamedicChanged(value, $td) {
        var filterExpression = "ParamedicCode = '" + value + "'";
        Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
            if (result != null) {
                $td.find('.lblParamedicName').html(result.ParamedicName);
                $td.find('.hdnParamedicID').val(result.ParamedicID);
            }
        });
    }
</script>
<div style="padding:10px;">
    <input type="hidden" id="hdnItemID" runat="server" />
    <table width="100%">
        <colgroup>
            <col width="120px" />
            <col />
        </colgroup>
        <input type="button" id="btnSavePhysicianCtl" value='<%=GetLabel("Save Dokter") %>' />
        <tr>
            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Paket MCU")%></label></td>
            <td colspan="2"><asp:TextBox ID="txtItemServiceName" ReadOnly="true" Width="350px" runat="server" /></td>
        </tr>  
        <tr>
            <td colspan="3">
                <dxcp:ASPxCallbackPanel ID="cbpViewPopup" runat="server" Width="100%" ClientInstanceName="cbpViewPopup" ShowLoadingPanel="false" OnCallback="cbpViewPopup_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                        EndCallback="function(s,e){ onCbpViewPopupEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                position: relative; font-size: 0.95em; max-height: 420px; overflow-y: auto">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect"
                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" >
                                    <Columns>
                                        <asp:BoundField DataField="DetailItemID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField HeaderStyle-Width="250px" HeaderText = "Unit Pelayanan" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <input type="hidden" value="<%#: Eval("HealthcareServiceUnitID") %>" class="hdnHealthareServiceUnitID" />
                                                <div><%#:Eval("ServiceUnitName") %></div>
                                                <div style="font-style:italic"><%#:Eval("DepartmentName") %></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="250px" HeaderText = "Dokter/Paramedis" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <input type="hidden" value="<%= defaultParamedicID %>" class="hdnParamedicID"/>
                                                <label class="lblParamedicName lblLink"><%= defaultParamedicName %></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText = "Pelayanan" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <input type="hidden" value="<%#:Eval("DetailItemID") %>" class="hdnDetailItemID"/>
                                                <div><%#:Eval("DetailItemName1") %></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Quantity" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" HeaderText="Quantity" HeaderStyle-Width="80px" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Tidak ada detail pemeriksaan MCU")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
            </td>
        </tr>
    </table>
</div>