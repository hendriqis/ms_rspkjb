<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PurchaseRequestSearchCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inventory.Program.PurchaseRequestSearchCtl" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_drugslogisticsquickpicksctl">
    function addItemFilterRow() {
        $trHeader = $('#<%=grdView.ClientID %> tr:eq(0)');
        $trFilter = $("<tr><td></td><td></td><td></td></tr>");

        //        $input = $("<input type='text' id='txtFilterItemCode' style='width:100%;height:20px' />").val($('#<%=hdnFilterItemCode.ClientID %>').val());
        //        $trFilter.find('td').eq(1).append($input);
        //        $input = $("<input type='text' id='txtFilterItemName' style='width:100%;height:20px' />").val($('#<%=hdnFilterItemName.ClientID %>').val());
        //        $trFilter.find('td').eq(2).append($input);
        //        $trFilter.insertAfter($trHeader);
    }

    ////    $('#txtFilterItemCode').live('keypress', function (e) {
    ////        var code = (e.keyCode ? e.keyCode : e.which);
    ////        if (code == 7) {
    ////            getCheckedMember();
    ////            $('#<%=hdnFilterItemCode.ClientID %>').val($(this).val());
    ////            e.preventDefault();
    ////            cbpPopup.PerformCallback('refresh');
    ////        }
    ////    });

    ////    $('#txtFilterItemName').live('keypress', function (e) {
    ////        var code = (e.keyCode ? e.keyCode : e.which);
    ////        if (code == 7) {
    ////            getCheckedMember();
    ////            $('#<%=hdnFilterItemName.ClientID %>').val($(this).val());
    ////            e.preventDefault();
    ////            cbpPopup.PerformCallback('refresh');
    ////        }
    ////    });

    $(function () {
        hideLoadingPanel();
        addItemFilterRow();

        $('#<%=btnMPEntryPopupSave.ClientID %>').click(function () {
            var lstSelectedID = [];
            var lstSelectedCode = [];
            $('#tblSelectedItem .trSelectedItem').each(function () {
                var key = $(this).find('.keyField').val();
                var lobClassCode = $(this).find('.tdPurchaseRequestNo').html();
                lstSelectedID.push(key);
                lstSelectedCode.push(lobClassCode);
            });
            setPurchaseRequest(lstSelectedID.join(','), lstSelectedCode.join(','));
            pcRightPanelContent.Hide();
        });
    });

    function onBeforeSaveRecord(errMessage) {
        if (IsValid(null, 'fsDrugsQuickPicks', 'mpDrugsQuickPicks')) {
            getCheckedMember();
            if ($('#<%=hdnSelectedMember.ClientID %>').val() != '')
                return true;
            else {
                errMessage.text = 'Please Select Item First';
                return false;
            }
        }
        return false;
    }

    function getCheckedMember() {
        var lstSelectedMember = [];
        var result = '';
        $('#tblSelectedItem .trSelectedItem').each(function () {
            var key = $(this).find('.keyField').val();
            lstSelectedMember.push(key);
        });
        $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
    }

    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');

    $(function () {
        setPaging($("#pagingPopup"), pageCount, function (page) {
            getCheckedMember();
            cbpPopup.PerformCallback('changepage|' + page);
        });
    });

    function onCbpPopupEndCallback(s) {
        hideLoadingPanel();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPaging($("#pagingPopup"), pageCount, function (page) {
                getCheckedMember();
                cbpPopup.PerformCallback('changepage|' + page);
            });
        }
        addItemFilterRow();
    }
    //#endregion

    $('#<%=grdView.ClientID %> .chkIsSelected input').die('change');
    $('#<%=grdView.ClientID %> .chkIsSelected input').live('change', function () {
        if ($(this).is(':checked')) {
            $selectedTr = $(this).closest('tr');

            $newTr = $('#tmplSelectedTestItem').html();
            $newTr = $newTr.replace(/\$\{PurchaseRequestNo}/g, $selectedTr.find('.tdPurchaseRequestNo').html());
            $newTr = $newTr.replace(/\$\{cfRequestCreatedInformation}/g, $selectedTr.find('.tdRequestInformation').html());
            $newTr = $newTr.replace(/\$\{cfRemarks}/g, $selectedTr.find('.tdRemarks').html());
            $newTr = $newTr.replace(/\$\{PurchaseRequestID}/g, $selectedTr.find('.keyField').html());
            $newTr = $($newTr);
            $newTr.insertBefore($('#trFooter'));
        }
        else {
            var id = $(this).closest('tr').find('.keyField').html();
            $('#tblSelectedItem tr').each(function () {
                if ($(this).find('.keyField').val() == id) {
                    $(this).remove();
                }
            });
        }
    });

    $('#tblSelectedItem .chkIsSelected2').die('change');
    $('#tblSelectedItem .chkIsSelected2').live('change', function () {
        if ($(this).is(':checked')) {
            $selectedTr = $(this).closest('tr');
            var id = $selectedTr.find('.keyField').val();
            var isFound = false;
            $('#<%=grdView.ClientID %> tr').each(function () {
                if (id == $(this).find('.keyField').html()) {
                    $(this).find('.chkIsSelected').find('input').prop('checked', false);
                    isFound = true;
                }
            });
            if (!isFound) {
                var lstSelectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().split(',');
                lstSelectedMember.splice(lstSelectedMember.indexOf(id), 1);
                $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
            }
            $(this).closest('tr').remove();
        }
    });

    function onRefreshGrid() {
        $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
        cbpPopup.PerformCallback('refresh');
    }

    function onTxtSearchViewSearchClick(s) {
        setTimeout(function () {
            s.SetBlur();
            onRefreshGrid();
        }, 0);
    }
</script>
<div class="toolbarArea">
    <ul>
        <li runat="server" id="btnMPEntryPopupSave">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><div>
                <%=GetLabel("Set")%></div>
        </li>
    </ul>
</div>
<div style="padding: 10px;">
    <script id="tmplSelectedTestItem" type="text/x-jquery-tmpl">
        <tr class="trSelectedItem">
            <td align="center">
                <input type="checkbox" class="chkIsSelected2" />
                <input type="hidden" class="keyField" value='${PurchaseRequestID}' />
            </td>
            <td class="tdPurchaseRequestNo">${PurchaseRequestNo}</td>
            <td class="tdRequestInformation">${cfRequestCreatedInformation}</td>
            <td>${cfRemarks}</td>
        </tr>
    </script>
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
    <input type="hidden" id="hdnTransactionID" runat="server" value="" />
    <input type="hidden" id="hdnParam" runat="server" value="" />
    <input type="hidden" id="hdnLocationParam" runat="server" value="" />
    <input type="hidden" id="hdnPurchaseOrderType" runat="server" value="" />
    <input type="hidden" id="hdnFilterItemCode" runat="server" />
    <input type="hidden" id="hdnFilterItemName" runat="server" />
    <input type="hidden" id="hdnProductLineIDCtl" runat="server" />
    <input type="hidden" id="hdnSelectedMemberQty" runat="server" value="" />
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <table>
        <colgroup>
            <col style="width: 10%" />
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label>
                    <%=GetLabel("Quick Filter")%></label>
            </td>
            <td>
                <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                    Width="378px" Watermark="Search">
                    <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                    <IntellisenseHints>
                        <qis:QISIntellisenseHint Text="No. Permintaan" FieldName="PurchaseRequestNo" />
                    </IntellisenseHints>
                </qis:QISIntellisenseTextBox>
            </td>
        </tr>
    </table>
    <div style="height: 400px; overflow-y: auto; overflow-x: hidden;">
    <table style="width: 100%">
        <colgroup>
            <col style="width: 50%" />
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <h4>
                    <%=GetLabel("Tersedia")%></h4>
                <dxcp:ASPxCallbackPanel ID="cbpPopup" runat="server" Width="100%" ClientInstanceName="cbpPopup"
                    ShowLoadingPanel="false" OnCallback="cbpPopup_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){ onCbpPopupEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="PurchaseRequestID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                <div <%# Eval("IsUrgent").ToString() != "True" ? "Style='display:none'":"" %>>
                                                    <img src='<%# ResolveUrl("~/Libs/Images/Button/warning.png") %>' width="17" height="17"
                                                        alt="" visible="true" title='<%=GetLabel("Urgent") %>' />
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="PurchaseRequestNo" HeaderText="Request No" ItemStyle-CssClass="tdPurchaseRequestNo"
                                            HeaderStyle-Width="130px" />
                                        <%--<asp:BoundField DataField="LocationName" HeaderText="Lokasi" ItemStyle-CssClass="tdLocationName" />--%>
                                        <asp:TemplateField HeaderStyle-Width="180px" HeaderText="Information" ItemStyle-CssClass="tdRequestInformation">
                                            <ItemTemplate>
                                                <%#:Eval("cfPurchaseOrderTypeInformation") %>
                                                <br />
                                                Created :
                                                <%#:Eval("cfRequestCreatedInformation") %>
                                                <br />
                                                Approved :
                                                <%#:Eval("cfRequestApprovedInformation") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="cfRemarks" HeaderText="Remarks" ItemStyle-CssClass="tdRemarks" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="pagingPopup">
                        </div>
                    </div>
                </div>
            </td>
            <td style="padding: 5px; vertical-align: top">
                <h4>
                    <%=GetLabel("Dipilih")%></h4>
                <fieldset id="fsDrugsQuickPicks">
                    <table id="tblSelectedItem" class="grdView notAllowSelect" cellspacing="0" rules="all">
                        <tr id="trHeader2">
                            <th style="width: 40px">
                                &nbsp;
                            </th>
                            <th align="left" style="width: 130px">
                                <%=GetLabel("Request No")%>
                            </th>
                            <th align="left" style="width: 180px">
                                <%=GetLabel("Information")%>
                            </th>
                            <th align="left">
                                <%=GetLabel("Remarks")%>
                            </th>
                        </tr>
                        <asp:Repeater ID="rptSelected" runat="server">
                            <ItemTemplate>
                                <tr class="trSelectedItem">
                                    <td align="center">
                                        <input type="checkbox" class="chkIsSelected2" />
                                        <input type="hidden" class="keyField" value='<%#:Eval("PurchaseRequestID") %>' />
                                    </td>
                                    <td class="tdPurchaseRequestNo">
                                        <%#:Eval("PurchaseRequestNo")%>
                                    </td>
                                    <td>
                                        Created :
                                        <%#:Eval("cfRequestCreatedInformation") %>
                                        <br />
                                        Approved :
                                        <%#:Eval("cfRequestApprovedInformation") %>
                                    </td>
                                    <td>
                                        <%#:Eval("cfRemarks") %>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <tr id="trFooter">
                        </tr>
                    </table>
                </fieldset>
            </td>
        </tr>
    </table>
</div>
