<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MarginMarkupDtEntryCtlX.ascx.cs" 
    Inherits="QIS.Medinfras.Web.Finance.Program.MarginMarkupDtEntryCtlX" %>

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
        $('#<%=txtMarkupAmount2.ClientID %>').val('0').trigger('changeValue');
        
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
        var markupAmount2 = $row.find('.hdnMarkupAmount2').val();

        $('#<%=hdnSequenceNo.ClientID %>').val(sequenceNo);
        $('#<%=txtStartingValue.ClientID %>').val(startingValue).trigger('changeValue');
        $('#<%=txtEndingValue.ClientID %>').val(endingValue).trigger('changeValue');
        $('#<%=txtMarkupAmount.ClientID %>').val(markupAmount).trigger('changeValue');
        $('#<%=txtMarkupAmount2.ClientID %>').val(markupAmount2).trigger('changeValue');

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
    <table class="tblContentArea">
        <colgroup>
            <col style="width:100%"/>
        </colgroup>
        <tr>            
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:100%">
                    <colgroup>
                        <col style="width:100px"/>
                        <col style="width:100px"/>
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Markup")%></label></td>
                        <td><asp:TextBox ID="txtMarkupCode" ReadOnly="true" Width="100%" runat="server" /></td>
                        <td><asp:TextBox ID="txtMarkupName" ReadOnly="true" Width="100%" runat="server" /></td>
                    </tr>  
                </table>

                <div id="containerPopupEntryData" style="margin-top:10px;display:none;">
                    <input type="hidden" id="hdnSequenceNo" runat="server" value="" />
                    <div class="pageTitle"><%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin:0"> 
                        <table class="tblEntryDetail" style="width:100%">
                            <colgroup>
                                <col style="width:160px"/>
                                <col style="width:3px"/>
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory" ><%=GetLabel("Rentang Awal")%></label></td>
                                <td>&nbsp;</td>
                                <td><asp:TextBox ID="txtStartingValue" CssClass="required txtCurrency" Width="150px" runat="server" /></td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Rentang Akhir")%></label></td>
                                <td>&nbsp;</td>
                                <td><asp:TextBox ID="txtEndingValue" CssClass="required txtCurrency" Width="150px" runat="server" /></td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Margin Default")%></label></td>
                                <td>&nbsp;</td>
                                <td><asp:TextBox ID="txtMarkupAmount" CssClass="required txtCurrency" Width="150px" runat="server" /></td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Margin Rawat Inap")%></label></td>
                                <td>&nbsp;</td>
                                <td><asp:TextBox ID="txtMarkupAmount2" CssClass="required txtCurrency" Width="150px" runat="server" /></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("Simpan")%>' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnEntryPopupCancel" value='<%= GetLabel("Batal")%>' />
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
                                                <input type="hidden" class="hdnMarkupAmount2" value="<%#: Eval("MarkupAmount2")%>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="StartingValue" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" HeaderText="Rentang Nilai Awal" ItemStyle-CssClass="tdStartingValue" />
                                        <asp:BoundField DataField="EndingValue" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" HeaderText="Rentang Nilai Akhir" ItemStyle-CssClass="tdEndingValue" />
                                        <asp:BoundField DataField="MarkupAmount" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" HeaderText="Margin Default" ItemStyle-CssClass="tdMarkupAmount" />
                                        <asp:BoundField DataField="MarkupAmount2" ItemStyle-Width="100px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" HeaderText="Margin Rawat Inap" ItemStyle-CssClass="tdMarkupAmount2" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Data Tidak Tersedia")%>
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
                    <span class="lblLink" id="lblEntryPopupAddData"><%= GetLabel("Tambah Data")%></span>
                </div>
            </td>
        </tr>
    </table>
    <div style="width:100%;text-align:right; padding-top:20px">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>

