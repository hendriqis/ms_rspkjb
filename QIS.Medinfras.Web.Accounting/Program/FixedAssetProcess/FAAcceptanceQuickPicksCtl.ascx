<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FAAcceptanceQuickPicksCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Accounting.Program.FAAcceptanceQuickPicksCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_faacceptancequickpicksctl">
    function addItemFilterRow() {
        $trHeader = $('#<%=grdView.ClientID %> tr:eq(0)');
        $trFilter = $("<tr><td></td><td></td><td></td><td></td></tr>");

        $inputCode = $("<input type='text' id='txtFilterItemCode' style='height:20px' />").val($('#<%=hdnFilterItemCode.ClientID %>').val());
        $inputName = $("<input type='text' id='txtFilterItemName' style='height:20px' />").val($('#<%=hdnFilterItemName.ClientID %>').val());
        $inputSerialNumber = $("<input type='text' id='txtFilterSerialNumber' style='height:20px' />").val($('#<%=hdnFilterSerialNumber.ClientID %>').val());
        $trFilter.find('td').eq(1).append($inputCode);
        $trFilter.find('td').eq(2).append($inputName);
        $trFilter.find('td').eq(3).append($inputSerialNumber);
        $trFilter.insertAfter($trHeader);
    }

    $('#txtFilterItemCode').live('keypress', function (e) {
        var code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13) {
            getCheckedMember();
            $('#<%=hdnFilterItemCode.ClientID %>').val($(this).val());
            e.preventDefault();
            cbpPopup.PerformCallback('refresh');
        }
    });

    $('#txtFilterItemName').live('keypress', function (e) {
        var code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13) {
            getCheckedMember();
            $('#<%=hdnFilterItemName.ClientID %>').val($(this).val());
            e.preventDefault();
            cbpPopup.PerformCallback('refresh');
        }
    });

    $('#txtFilterSerialNumber').live('keypress', function (e) {
        var code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13) {
            getCheckedMember();
            $('#<%=hdnFilterSerialNumber.ClientID %>').val($(this).val());
            e.preventDefault();
            cbpPopup.PerformCallback('refresh');
        }
    });

    $(function () {
        hideLoadingPanel();
        addItemFilterRow();
    });

    function onBeforeSaveRecord(errMessage) {
        if (IsValid(null, 'fsFAAcceptanceQuickPicks', 'mpFAAcceptanceQuickPicks')) {
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
        var lstSelectedMemberQty = [];
        var result = '';
        $('#tblSelectedItem .trSelectedItem').each(function () {
            var key = $(this).find('.keyField').val();
            var qty = $(this).find('.txtQty').val();
            lstSelectedMember.push(key);
            lstSelectedMemberQty.push(qty);
        });
        $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
        $('#<%=hdnSelectedMemberQty.ClientID %>').val(lstSelectedMemberQty.join(','));
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
            $newTr = $newTr.replace(/\$\{SerialNumber}/g, $selectedTr.find('.tdSerialNumber').html());
            $newTr = $newTr.replace(/\$\{FixedAssetName}/g, $selectedTr.find('.tdFixedAssetName').html());
            $newTr = $newTr.replace(/\$\{FixedAssetCode}/g, $selectedTr.find('.tdFixedAssetCode').html());
            $newTr = $newTr.replace(/\$\{FixedAssetID}/g, $selectedTr.find('.keyField').html());
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
</script>
<div style="padding: 10px;">
    <script id="tmplSelectedTestItem" type="text/x-jquery-tmpl">
        <tr class="trSelectedItem">
            <td align="center">
                <input type="checkbox" class="chkIsSelected2" />
                <input type="hidden" class="keyField" value='${FixedAssetID}' />
            </td>
            <td>${FixedAssetCode}</td>
            <td>${FixedAssetName}</td>
            <td>${SerialNumber}</td>
        </tr>
    </script>
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
    <input type="hidden" id="hdnFAAcceptanceIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnProductLineIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnFAGroupIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnFALocationIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnFilterItemCode" runat="server" />
    <input type="hidden" id="hdnFilterItemName" runat="server" />
    <input type="hidden" id="hdnFilterSerialNumber" runat="server" />
    <input type="hidden" id="hdnSelectedMemberQty" runat="server" value="" />
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
                                        <asp:BoundField DataField="FixedAssetID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="FixedAssetCode" HeaderText="Kode Aset" ItemStyle-CssClass="tdFixedAssetCode"
                                            HeaderStyle-Width="30px" />
                                        <asp:BoundField DataField="FixedAssetName" HeaderText="Nama Aset" ItemStyle-CssClass="tdFixedAssetName" />
                                        <asp:BoundField DataField="SerialNumber" HeaderText="No Serial" ItemStyle-CssClass="tdSerialNumber" HeaderStyle-Width="100px" />
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
                <fieldset id="fsFAAcceptanceQuickPicks">
                    <table id="tblSelectedItem" class="grdView notAllowSelect" cellspacing="0" rules="all">
                        <tr id="trHeader2">
                            <th style="width: 40px">
                                &nbsp;
                            </th>
                            <th align="center">
                                <%=GetLabel("Kode Aset")%>
                            </th>
                            <th align="center">
                                <%=GetLabel("Nama Aset")%>
                            </th>
                            <th align="center">
                                <%=GetLabel("No Serial")%>
                            </th>
                        </tr>
                        <tr id="trFooter">
                        </tr>
                    </table>
                </fieldset>
            </td>
        </tr>
    </table>
    <div class="imgLoadingGrdView" id="containerImgLoadingView">
        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
    </div>
</div>
