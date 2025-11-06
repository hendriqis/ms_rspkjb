<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LocationItemEntryCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.SystemSetup.Program.LocationItemEntryCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=hdnID.ClientID %>').val('');
        $('#<%=hdnItemID.ClientID %>').val('');
        $('#<%=txtItemCode.ClientID %>').val('');
        $('#<%=txtItemName.ClientID %>').val('');
        $('#<%=txtMinimum.ClientID %>').val('');
        $('#<%=txtMaximum.ClientID %>').val('');
        
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
        var itemID = $row.find('.hdnItemID').val();
        var itemCode = $row.find('.hdnItemCode').val();

        var itemName = $row.find('.tdItemName1').html();
        var minimum = $row.find('.tdMinimum').html();
        var maximum = $row.find('.tdMaximum').html();
        var beginningBalance = $row.find('.tdBeginningBalance').html();
        var qtyIn = $row.find('.tdQtyIn').html();
        var qtyOut = $row.find('.tdQtyOut').html();
        var endingBalance = $row.find('.tdEndingBalance').html();
        
        $('#<%=hdnID.ClientID %>').val(ID);
        $('#<%=hdnItemID.ClientID %>').val(itemID);
        $('#<%=txtItemCode.ClientID %>').val(itemCode);
        $('#<%=txtItemName.ClientID %>').val(itemName);
        $('#<%=txtMinimum.ClientID %>').val(minimum);
        $('#<%=txtMaximum.ClientID %>').val(maximum);

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
            else {
                $('#containerPopupEntryData').hide();
                var pageCount = parseInt(param[2]);
                setPagingDetailItem(pageCount);
            }
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
            else {
                var pageCount = parseInt(param[2]);
                setPagingDetailItem(pageCount);
            }
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

    //#region Item
    $('#lblItem.lblLink').die('click');
    $('#lblItem.lblLink').live('click', function () {
        var filterExpression = "ItemID NOT IN (SELECT ItemID FROM ItemBalance WHERE LocationID = " + $('#<%=hdnLocationID.ClientID %>').val() + " AND IsDeleted = 0)";
        openSearchDialog('item', filterExpression, function (value) {
            $('#<%=txtItemCode.ClientID %>').val(value);
            onTxtItemCodeChanged(value);
        });
    });

    $('#<%=txtItemCode.ClientID %>').die('change');
    $('#<%=txtItemCode.ClientID %>').live('change', function () {
        onTxtItemCodeChanged($(this).val());
    });

    function onTxtItemCodeChanged(value) {
        var filterExpression = "ItemCode = '" + value + "'";
        Methods.getObject('GetItemMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnItemID.ClientID %>').val(result.ItemID);
                $('#<%=txtItemName.ClientID %>').val(result.ItemName1);
            }
            else {
                $('#<%=hdnItemID.ClientID %>').val('');
                $('#<%=txtItemCode.ClientID %>').val('');
                $('#<%=txtItemName.ClientID %>').val('');
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
    <input type="hidden" id="hdnLocationID" value="" runat="server" />
    <div class="pageTitle"><%=GetLabel("Item Balance")%></div>
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
                        <td colspan="2"><asp:TextBox ID="txtLocationCode" ReadOnly="true" Width="100%" runat="server" /></td>
                    </tr>  
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Name")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtLocationName" ReadOnly="true" Width="100%" runat="server" /></td>
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
                                <td class="tdLabel"><label class="lblMandatory lblLink" id="lblItem"><%=GetLabel("Item")%></label></td>
                                <td>
                                    <input type="hidden" runat="server" id="hdnItemID" />
                                    <table style="width:100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width:30%"/>
                                            <col style="width:3px"/>
                                            <col/>
                                        </colgroup>
                                        <tr>
                                            <td><asp:TextBox ID="txtItemCode" CssClass="required" Width="100%" runat="server" /></td>
                                            <td>&nbsp;</td>
                                            <td><asp:TextBox ID="txtItemName" ReadOnly="true" Width="100%" runat="server" /></td>
                                        </tr>
                                    </table>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Reorder Minimum")%></label></td>
                                <td><asp:TextBox ID="txtMinimum" CssClass="number required" runat="server" Width="100px" /></td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Reorder Maximum")%></label></td>
                                <td><asp:TextBox ID="txtMaximum" CssClass="number required" runat="server" Width="100px" /></td>
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
                                <asp:ListView runat="server" ID="lvwView">
                                    <EmptyDataTemplate>
                                        <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0" rules="all" >
                                            <tr>
                                                <th style="width:70px" rowspan="2">&nbsp;</th>
                                                <th style="width:250px" rowspan="2" align="center"><%=GetLabel("Item")%></th>
                                                <th colspan="2" align="center"><%=GetLabel("Reorder Point")%></th>
                                                <th colspan="4" align="center"><%=GetLabel("Balance")%></th>
                                            </tr>
                                            <tr>
                                                <th style="width:100px" align="center"><%=GetLabel("Minimum")%></th>
                                                <th style="width:100px" align="center"><%=GetLabel("Maximum")%></th>
                                                <th style="width:100px" align="center"><%=GetLabel("Beginning")%></th>
                                                <th style="width:100px" align="center"><%=GetLabel("In")%></th>
                                                <th style="width:100px" align="center"><%=GetLabel("Out")%></th>
                                                <th style="width:100px" align="center"><%=GetLabel("Ending")%></th>
                                            </tr>
                                            <tr class="trEmpty">
                                                <td colspan="8">
                                                    <%=GetLabel("No Data To Display")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0" rules="all" >
                                            <tr>
                                                <th style="width:70px" rowspan="2">&nbsp;</th>
                                                <th style="width:250px" rowspan="2" align="center"><%=GetLabel("Item")%></th>
                                                <th colspan="2" align="center"><%=GetLabel("Reorder Point")%></th>
                                                <th colspan="4" align="center"><%=GetLabel("Balance")%></th>
                                            </tr>
                                            <tr>
                                                <th style="width:100px" align="center"><%=GetLabel("Minimum")%></th>
                                                <th style="width:100px" align="center"><%=GetLabel("Maximum")%></th>
                                                <th style="width:100px" align="center"><%=GetLabel("Beginning")%></th>
                                                <th style="width:100px" align="center"><%=GetLabel("In")%></th>
                                                <th style="width:100px" align="center"><%=GetLabel("Out")%></th>
                                                <th style="width:100px" align="center"><%=GetLabel("Ending")%></th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder" ></tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt="" style="float:left; margin-left:7px" />
                                                <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" style="margin-left:2px" />

                                                <input type="hidden" class="hdnID" value="<%#: Eval("ID")%>" />
                                                <input type="hidden" class="hdnItemID" value="<%#: Eval("ItemID")%>" />
                                                <input type="hidden" class="hdnItemCode" value="<%#: Eval("ItemCode")%>" />
                                            </td>
                                            <td class="tdItemName1"><%#: Eval("ItemName1")%></td>
                                            <td class="tdMinimum" align="right"><%#: Eval("QuantityMIN")%></td>
                                            <td class="tdMaximum" align="right"><%#: Eval("QuantityMAX")%></td>
                                            <td class="tdBeginningBalance" align="right"><%#: Eval("QuantityBEGIN")%></td>
                                            <td class="tdQtyIn" align="right"><%#: Eval("QuantityIN")%></td>
                                            <td class="tdQtyOut" align="right"><%#: Eval("QuantityOUT")%></td>
                                            <td class="tdEndingBalance" align="right"><%#: Eval("QunatityEND")%></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
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

