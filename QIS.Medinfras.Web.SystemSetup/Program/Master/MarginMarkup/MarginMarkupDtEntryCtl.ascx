<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MarginMarkupDtEntryCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.SystemSetup.Program.MarginMarkupDtEntryCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=hdnSequenceNo.ClientID %>').val('');
        $('#<%=txtStartingValue.ClientID %>').val('0').trigger('changeValue');
        $('#<%=txtEndingValue.ClientID %>').val('0').trigger('changeValue');
        $('#<%=txtMarkupAmount.ClientID %>').val('0').trigger('changeValue');
        
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
            var sequenceNo = $row.find('.hdnSequenceNo').val();
            $('#<%=hdnSequenceNo.ClientID %>').val(sequenceNo);

            cbpEntryPopupView.PerformCallback('delete');
        }
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var sequenceNo = $row.find('.hdnSequenceNo').val();
        var startingValue = $row.find('.hdnStartingValue').val();
        var endingValue = $row.find('.hdnEndingValue').val();
        var markupAmount = $row.find('.hdnMarkupAmount').val();

        $('#<%=hdnSequenceNo.ClientID %>').val(sequenceNo);
        $('#<%=txtStartingValue.ClientID %>').val(startingValue).trigger('changeValue');
        $('#<%=txtEndingValue.ClientID %>').val(endingValue).trigger('changeValue');
        $('#<%=txtMarkupAmount.ClientID %>').val(markupAmount).trigger('changeValue');

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
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
        }
        $('#containerImgLoadingViewPopup').hide();
    }
</script>

<div style="height:440px; overflow-y:auto">
    <input type="hidden" id="hdnMarkupID" value="" runat="server" />
    <div class="pageTitle"><%=GetLabel("Margin Markup Dt")%></div>
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
                        <td colspan="2"><asp:TextBox ID="txtMarkupCode" ReadOnly="true" Width="100%" runat="server" /></td>
                    </tr>  
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Name")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtMarkupName" ReadOnly="true" Width="100%" runat="server" /></td>
                    </tr>  
                </table>

                <div id="containerPopupEntryData" style="margin-top:10px;display:none;">
                    <input type="hidden" id="hdnSequenceNo" runat="server" value="" />
                    <div class="pageTitle"><%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin:0"> 
                        <table class="tblEntryDetail" style="width:100%">
                            <colgroup>
                                <col style="width:200px"/>
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Starting Value")%></label></td>
                                <td><asp:TextBox ID="txtStartingValue" CssClass="required txtCurrency" Width="150px" runat="server" /></td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Ending Value")%></label></td>
                                <td><asp:TextBox ID="txtEndingValue" CssClass="required txtCurrency" Width="150px" runat="server" /></td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Markup Amount")%></label></td>
                                <td><asp:TextBox ID="txtMarkupAmount" CssClass="required txtCurrency" Width="150px" runat="server" /></td>
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
                            <asp:Panel runat="server" ID="pnlPatientVisitTransHdGrdView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false" OnRowDataBound="grdView_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt="" style="float:left; margin-left:7px" />
                                                <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" />

                                                <input type="hidden" class="hdnSequenceNo" value="<%#: Eval("SequenceNo")%>" />
                                                <input type="hidden" class="hdnStartingValue" value="<%#: Eval("StartingValue")%>" />
                                                <input type="hidden" class="hdnEndingValue" value="<%#: Eval("EndingValue")%>" />
                                                <input type="hidden" class="hdnMarkupAmount" value="<%#: Eval("MarkupAmount")%>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="StartingValue" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" HeaderText="Starting Value" ItemStyle-CssClass="tdStartingValue" />
                                        <asp:BoundField DataField="EndingValue" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" HeaderText="Ending Value" ItemStyle-CssClass="tdEndingValue" />
                                        <asp:BoundField DataField="MarkupAmount" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" HeaderText="Markup Amount" ItemStyle-CssClass="tdMarkupAmount" />
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

