<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemLaboratoryFractionEntryCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.SystemSetup.Program.ItemLaboratoryFractionEntryCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=hdnID.ClientID %>').val('');
        $('#<%=hdnFractionID.ClientID %>').val('');
        $('#<%=txtFractionCode.ClientID %>').val('');
        $('#<%=txtFractionName.ClientID %>').val('');
        $('#<%=txtDisplayOrder.ClientID %>').val('');

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
    $('.imgDelete.imgLink').live('click', function (evt) {
        evt.stopPropagation();
        evt.preventDefault();
        if (confirm("Are You Sure Want To Delete This Data?")) {
            $row = $(this).closest('tr');
            var id = $row.find('.hdnID').val();
            $('#<%=hdnID.ClientID %>').val(id);

            cbpEntryPopupView.PerformCallback('delete');
        }
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var ID = $row.find('.hdnID').val();
        var fractionID = $row.find('.hdnFractionID').val();
        var fractionCode = $row.find('.hdnFractionCode').val();

        var fractionName = $row.find('.tdFractionName1').html();
        var commCode = $row.find('.tdCommCode').html();
        var displayOrder = $row.find('.tdDisplayOrder').html();
        
        $('#<%=hdnID.ClientID %>').val(ID);
        $('#<%=hdnFractionID.ClientID %>').val(fractionID);
        $('#<%=txtFractionCode.ClientID %>').val(fractionCode);
        $('#<%=txtFractionName.ClientID %>').val(fractionName);
        $('#<%=txtDisplayOrder.ClientID %>').val(displayOrder);

        $('#containerPopupEntryData').show();
    });

    function setPagingDetailItem(pageCount) {
        if (pageCount > 0)
            $('.grdPopup tr:eq(1)').click();
            setPaging($("#pagingPopup"), pageCount, function (page) {
                cbpEntryPopupView.PerformCallback('changepage|' + page);
            }, 8);
    }

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
        else if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPagingDetailItem(pageCount);
        }
        else {
            $('.grdPopup tr:eq(1)').click();
        }
        hideLoadingPanel();
    }

    //#region Fraction
    $('#lblFraction.lblLink').die('click');
    $('#lblFraction.lblLink').live('click', function () {
        var filterExpression = "FractionID NOT IN (SELECT FractionID FROM ItemLaboratoryFraction WHERE ItemID = " + $('#<%=hdnItemID.ClientID %>').val() + " AND IsDeleted = 0)";
        openSearchDialog('fraction', filterExpression, function (value) {
            $('#<%=txtFractionCode.ClientID %>').val(value);
            onTxtFractionCodeChanged(value);
        });
    });

    $('#<%=txtFractionCode.ClientID %>').die('change');
    $('#<%=txtFractionCode.ClientID %>').live('change', function () {
        onTxtFractionCodeChanged($(this).val());
    });

    function onTxtFractionCodeChanged(value) {
        var filterExpression = "FractionCode = '" + value + "'";
        Methods.getObject('GetFractionList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnFractionID.ClientID %>').val(result.FractionID);
                $('#<%=txtFractionName.ClientID %>').val(result.FractionName1);
            }
            else {
                $('#<%=hdnFractionID.ClientID %>').val('');
                $('#<%=txtFractionCode.ClientID %>').val('');
                $('#<%=txtFractionName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Paging
    var pageCountAvailable = parseInt('<%=PageCount %>');
    $(function () {
        setPagingDetailItem(pageCountAvailable);
    });
    //#endregion
</script>

<div style="height:440px; overflow-y:auto;overflow-x: hidden">
    <input type="hidden" id="hdnItemID" value="" runat="server" />
    <div class="pageTitle"><%=GetLabel("Item Laboratory Fraction")%></div>
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
                        <td colspan="2"><asp:TextBox ID="txtItemCode" ReadOnly="true" Width="100%" runat="server" /></td>
                    </tr>  
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Name")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtItemName" ReadOnly="true" Width="100%" runat="server" /></td>
                    </tr>  
                </table>

                <div id="containerPopupEntryData" style="margin-top:10px;display:none;">
                    <input type="hidden" id="hdnID" runat="server" value="" />
                    <div class="pageTitle"><%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin:0"> 
                        <table class="tblEntryDetail" style="width:100%">
                            <colgroup>
                                <col style="width:150px"/>
                                <col style="width:400px"/>
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory lblLink" id="lblFraction"><%=GetLabel("Fraction")%></label></td>
                                <td>
                                    <input type="hidden" runat="server" id="hdnFractionID" />
                                    <table style="width:100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width:30%"/>
                                            <col style="width:3px"/>
                                            <col/>
                                        </colgroup>
                                        <tr>
                                            <td><asp:TextBox ID="txtFractionCode" CssClass="required" Width="100%" runat="server" /></td>
                                            <td>&nbsp;</td>
                                            <td><asp:TextBox ID="txtFractionName" ReadOnly="true" Width="100%" runat="server" /></td>
                                        </tr>
                                    </table>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label><%=GetLabel("Communication Code")%></label></td>
                                <td><asp:TextBox ID="txtCommunicationCode" runat="server" Width="100px" /></td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Display Order")%></label></td>
                                <td><asp:TextBox ID="txtDisplayOrder" CssClass="number required" runat="server" Width="100px" /></td>
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
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
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
                                                <input type="hidden" class="hdnFractionID" value="<%#: Eval("FractionID")%>" />
                                                <input type="hidden" class="hdnFractionCode" value="<%#: Eval("FractionCode")%>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FractionName1" ItemStyle-CssClass="tdFractionName1" HeaderText="Fraction" />
                                        <asp:BoundField DataField="CommCode" ItemStyle-CssClass="tdCommCode" HeaderText="Communication Code" HeaderStyle-Width="120px" />
                                        <asp:BoundField DataField="DisplayOrder" ItemStyle-CssClass="tdDisplayOrder" HeaderText="Display Order" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Right" />
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
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="pagingPopup"></div>
                    </div>
                </div> 
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

