<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VitalSignNormalValueEntryCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.SystemSetup.Program.VitalSignNormalValueEntryCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=hdnID.ClientID %>').val('');
        $('#<%=ddlSex.ClientID %>').val('');
        $('#<%=txtFromAge.ClientID %>').val('');
        $('#<%=txtToAge.ClientID %>').val('');
        $('#<%=ddlAgeUnit.ClientID %>').val('');
        $('#<%=txtMinValue.ClientID %>').val('');
        $('#<%=txtMaxValue.ClientID %>').val('');
        
        $('#containerPopupEntryData').show();
    });

    $('#btnEntryPopupCancel').live('click', function () {
        $('#containerPopupEntryData').hide();
    });

    $('#btnEntryPopupSave').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
            cbpEntryPopupView.PerformCallback('save');
        return false;
    });

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function () {
        if (confirm("Are You Sure Want To Delete This Data?")) {
            $row = $(this).closest('tr');
            var healthcareServiceUnitID = $row.find('.hdnID').val();
            $('#<%=hdnID.ClientID %>').val(healthcareServiceUnitID);

            cbpEntryPopupView.PerformCallback('delete');
        }
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var ID = $row.find('.hdnID').val();
        var GCAgeUnit = $row.find('.hdnGCAgeUnit').val();
        var sex = $row.find('.tdSex').html();
        var fromAge = $row.find('.tdFromAge').html();
        var toAge = $row.find('.tdToAge').html();
        var ageUnit = $row.find('.tdAgeUnit').html();
        var minValue = $row.find('.tdMinValue').html();
        var maxValue = $row.find('.tdMaxValue').html();
        
        $('#<%=hdnID.ClientID %>').val(ID);
        $('#<%=ddlSex.ClientID %>').val(sex);
        $('#<%=txtFromAge.ClientID %>').val(fromAge);
        $('#<%=txtToAge.ClientID %>').val(toAge);
        $('#<%=ddlAgeUnit.ClientID %>').val(GCAgeUnit);
        $('#<%=txtMinValue.ClientID %>').val(minValue);
        $('#<%=txtMaxValue.ClientID %>').val(maxValue);

        $('#containerPopupEntryData').show();
    });

    function onCbpEntryPopupViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else
                $('#containerPopupEntryData').hide();
        }
        $('#containerImgLoadingView').hide();
    }
</script>

<div style="height:440px; overflow-y:auto">
    <input type="hidden" id="hdnVitalSignID" value="" runat="server" />
    <div class="pageTitle"><%=GetLabel("Healthcare")%></div>
    <table class="tblContentArea">
        <colgroup>
            <col style="width:100%"/>
        </colgroup>
        <tr>            
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:70%">
                    <colgroup>
                        <col style="width:160px"/>
                        <col/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Code")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtVitalSignCode" ReadOnly="true" Width="100%" runat="server" /></td>
                    </tr>  
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Name")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtVitalSignName" ReadOnly="true" Width="100%" runat="server" /></td>
                    </tr>  
                </table>

                <div id="containerPopupEntryData" style="margin-top:10px;display:none;">
                    <input type="hidden" id="hdnID" runat="server" value="" />
                    <div class="pageTitle"><%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin:0"> 
                        <table class="tblEntryDetail" style="width:100%">
                            <colgroup>
                                <col style="width:100px"/>
                                <col style="width:150px"/>
                                <col style="width:100px"/>
                                <col style="width:150px"/>
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Sex")%></label></td>
                                <td colspan="3"><asp:DropDownList ID="ddlSex" runat="server" CssClass="required" Width="100%" /></td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("From Age")%></label></td>
                                <td><asp:TextBox ID="txtFromAge" CssClass="number required" runat="server" Width="100%" /></td>
                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("To Age")%></label></td>
                                <td><asp:TextBox ID="txtToAge" CssClass="number required" runat="server" Width="100%" /></td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Age Unit")%></label></td>
                                <td colspan="3"><asp:DropDownList ID="ddlAgeUnit" CssClass="required" Width="100%" runat="server" /></td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Min Value")%></label></td>
                                <td><asp:TextBox ID="txtMinValue" CssClass="number required" runat="server" Width="100%" /></td>
                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Max Value")%></label></td>
                                <td><asp:TextBox ID="txtMaxValue" CssClass="number required" runat="server" Width="100%" /></td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("Save")%>' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnEntryPopupCancel" value='<%= GetLabel("Cancel")%>' />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>

                <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewPopup').show(); }"
                        EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false" OnRowDataBound="grdView_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt="" style="float:left; margin-left:7px" />
                                                <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" />

                                                <input type="hidden" class="hdnID" value="<%#: Eval("ID")%>" />
                                                <input type="hidden" class="hdnGCAgeUnit" value="<%#: Eval("GCAgeUnit")%>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderStyle-Width="150px" DataField="SexLabel" ItemStyle-CssClass="tdSex" HeaderText="HealthcareName" />
                                        <asp:BoundField HeaderStyle-Width="150px" DataField="FromAge" HeaderText="From Age" ItemStyle-CssClass="tdFromAge" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField HeaderStyle-Width="150px" DataField="ToAge" HeaderText="To Age" ItemStyle-CssClass="tdToAge" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField HeaderStyle-Width="150px" DataField="AgeUnit" HeaderText="Age Unit" ItemStyle-CssClass="tdAgeUnit" />
                                        <asp:BoundField HeaderStyle-Width="150px" DataField="MinValue" HeaderText="Min Value" ItemStyle-CssClass="tdMinValue" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField HeaderStyle-Width="150px" DataField="MaxValue" HeaderText="Max Value" ItemStyle-CssClass="tdMaxValue" ItemStyle-HorizontalAlign="Right" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div style="width:100%;text-align:center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblEntryPopupAddData"><%= GetLabel("Add Data")%></span>
                </div>
            </td>
        </tr>
    </table>
    <div style="width:100%;text-align:right">
        <input type="button" value='<%= GetLabel("Close")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>

