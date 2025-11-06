<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditQtyItemPackageDetail.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.EditQtyItemPackageDetail" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_EditQtyItemPackageDetail">
    $('#ulTabLabResult li').click(function () {
        $('#ulTabLabResult li.selected').removeAttr('class');
        $('.containerTransDtCtl').filter(':visible').hide();
        $contentID = $(this).attr('contentid');
        $('#' + $contentID).show();
        $(this).addClass('selected');
    });

    //#region SERVICES
    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var ID = $row.find('.ID').val();
        var ItemID = $row.find('.ItemID').val();
        var ItemCode = $row.find('.ItemCode').val();
        var ItemName1 = $row.find('.ItemName1').val();
        var ParamedicID = $row.find('.ParamedicID').val();
        var ParamedicCode = $row.find('.ParamedicCode').val();
        var ParamedicName = $row.find('.ParamedicName').val();
        var ChargedQuantity = $row.find('.ChargedQuantity').val();

        $('#<%=hdnID.ClientID %>').val(ID);
        $('#<%=hdnDetailItemID.ClientID %>').val(ItemID);
        $('#<%=txtDetailItemCode.ClientID %>').val(ItemCode);
        $('#<%=txtDetailItemName1.ClientID %>').val(ItemName1);
        $('#<%=hdnParamedicID.ClientID %>').val(ParamedicID);
        $('#<%=txtParamedicCode.ClientID %>').val(ParamedicCode);
        $('#<%=txtParamedicName.ClientID %>').val(ParamedicName);
        $('#<%=txtChargedQuantity.ClientID %>').val(ChargedQuantity);
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

    function onCbpEntryPopupViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else {
                $('#containerPopupEntryData').hide();
                cbpService.PerformCallback();
            }
        }
        $('#containerImgLoadingViewPopup').hide();
    }
    //#endregion

    //#region OBAT
    $('.imgEditObat.imgLink').die('click');
    $('.imgEditObat.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var ID = $row.find('.IDObat').val();
        var ItemID = $row.find('.ItemIDObat').val();
        var ItemCode = $row.find('.ItemCodeObat').val();
        var ItemName1 = $row.find('.ItemName1Obat').val();
        var ParamedicID = $row.find('.ParamedicIDObat').val();
        var ParamedicCode = $row.find('.ParamedicCodeObat').val();
        var ParamedicName = $row.find('.ParamedicNameObat').val();
        var ChargedQuantity = $row.find('.ChargedQuantityObat').val();

        $('#<%=hdnIDObat.ClientID %>').val(ID);
        $('#<%=hdnDetailItemIDObat.ClientID %>').val(ItemID);
        $('#<%=txtDetailItemCodeObat.ClientID %>').val(ItemCode);
        $('#<%=txtDetailItemName1Obat.ClientID %>').val(ItemName1);
        $('#<%=hdnParamedicIDObat.ClientID %>').val(ParamedicID);
        $('#<%=txtParamedicCodeObat.ClientID %>').val(ParamedicCode);
        $('#<%=txtParamedicNameObat.ClientID %>').val(ParamedicName);
        $('#<%=txtChargedQuantityObat.ClientID %>').val(ChargedQuantity);
        $('#containerPopupEntryDataObat').show();
    });

    $('#btnEntryPopupCancelObat').live('click', function () {
        $('#containerPopupEntryDataObat').hide();
    });

    $('#btnEntryPopupSaveObat').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
            cbpEntryPopupViewObat.PerformCallback('save');
        return false;
    });

    function onCbpEntryPopupViewObatEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else {
                $('#containerPopupEntryDataObat').hide();
                cbpService.PerformCallback();
            }
        }
        $('#containerImgLoadingViewPopupObat').hide();
    }
    //#endregion

    //#region LOGISTICS
    $('.imgEditBarang.imgLink').die('click');
    $('.imgEditBarang.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var ID = $row.find('.IDBarang').val();
        var ItemID = $row.find('.ItemIDBarang').val();
        var ItemCode = $row.find('.ItemCodeBarang').val();
        var ItemName1 = $row.find('.ItemName1Barang').val();
        var ParamedicID = $row.find('.ParamedicIDBarang').val();
        var ParamedicCode = $row.find('.ParamedicCodeBarang').val();
        var ParamedicName = $row.find('.ParamedicNameBarang').val();
        var ChargedQuantity = $row.find('.ChargedQuantityBarang').val();

        $('#<%=hdnIDBarang.ClientID %>').val(ID);
        $('#<%=hdnDetailItemIDBarang.ClientID %>').val(ItemID);
        $('#<%=txtDetailItemCodeBarang.ClientID %>').val(ItemCode);
        $('#<%=txtDetailItemName1Barang.ClientID %>').val(ItemName1);
        $('#<%=hdnParamedicIDBarang.ClientID %>').val(ParamedicID);
        $('#<%=txtParamedicCodeBarang.ClientID %>').val(ParamedicCode);
        $('#<%=txtParamedicNameBarang.ClientID %>').val(ParamedicName);
        $('#<%=txtChargedQuantityBarang.ClientID %>').val(ChargedQuantity);
        $('#containerPopupEntryDataBarang').show();
    });

    $('#btnEntryPopupCancelBarang').live('click', function () {
        $('#containerPopupEntryDataBarang').hide();
    });

    $('#btnEntryPopupSaveBarang').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
            cbpEntryPopupViewBarang.PerformCallback('save');
        return false;
    });

    function onCbpEntryPopupViewBarangEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else {
                $('#containerPopupEntryDataBarang').hide();
                cbpService.PerformCallback();
            }
        }
        $('#containerImgLoadingViewPopupBarang').hide();
    }
    //#endregion
</script>
<div style="height: 500px; overflow-y: auto; overflow-x: hidden">
    <input type="hidden" id="hdnGCItemType" runat="server" value="" />
    <input type="hidden" id="hdnChargesDtID" runat="server" value="" />
    <div class="containerUlTabPage">
        <ul class="ulTabPage" id="ulTabLabResult">
            <li class="selected" contentid="containerServiceCtl">
                <%=GetLabel("PELAYANAN") %></li>
            <li contentid="containerDrugMSCtl">
                <%=GetLabel("OBAT & ALKES") %></li>
            <li contentid="containerLogisticsCtl">
                <%=GetLabel("BARANG UMUM") %></li>
        </ul>
    </div>
    <div id="containerServiceCtl" class="containerTransDtCtl">
        <table class="tblContentArea">
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 90%">
                        <colgroup>
                            <col style="width: 120px" />
                            <col style="width: 400px" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Item")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtItemServiceName" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                        </tr>
                    </table>
                    <div id="containerPopupEntryData" style="margin-top: 10px; display: none;">
                        <input type="hidden" id="hdnID" runat="server" value="" />
                        <div class="pageTitle">
                            <%=GetLabel("Entry")%></div>
                        <fieldset id="fsEntryPopup" style="margin: 0">
                            <table class="tblEntryDetail" style="width: 100%">
                                <colgroup>
                                    <col style="width: 200px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal" id="lblItem">
                                            <%=GetLabel("Detail Paket")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" value="" id="hdnDetailItemID" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 120px" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtDetailItemCode" ReadOnly="true" Width="100%" runat="server" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDetailItemName1" ReadOnly="true" Width="80%" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal" id="lblParamedicDetail">
                                            <%=GetLabel("Dokter/Tenaga Medis")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" value="" id="hdnParamedicID" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 120px" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtParamedicCode" ReadOnly="true" CssClass="required" Width="100%"
                                                        runat="server" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtParamedicName" ReadOnly="true" CssClass="required" Width="80%"
                                                        runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblMandatory">
                                            <%=GetLabel("Jumlah")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtChargedQuantity" CssClass="number" Min="0" Width="120px" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <center>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("SAVE")%>' class="btnEntryPopupSave w3-btn w3-hover-blue"
                                                            style="width: 80px;" />
                                                    </td>
                                                    <td>
                                                        <input type="button" id="btnEntryPopupCancel" value='<%= GetLabel("CANCEL")%>' class="btnEntryPopupCancel w3-btn w3-hover-blue"
                                                            style="width: 80px;" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <div>
                        <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                            ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewPopup').show(); }"
                                EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                        margin-right: auto; position: relative; font-size: 0.95em;">
                                        <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                            OnRowDataBound="grdView_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <img class="imgEdit imgLink" <%#: Eval("IsAllowChanged").ToString() == "True" ?  "" : "style='display:none'" %>
                                                            src='<%# Eval("IsAllowChanged").ToString() == "True" ? ResolveUrl("~/Libs/Images/Button/edit.png") : ""%>'
                                                            alt="" style="text-align:center" />
                                                        <input type="hidden" class="ID" value="<%#: Eval("ID")%>" />
                                                        <input type="hidden" class="ItemID" value="<%#: Eval("ItemID")%>" />
                                                        <input type="hidden" class="ItemCode" value="<%#: Eval("ItemCode")%>" />
                                                        <input type="hidden" class="ItemName1" value="<%#: Eval("ItemName1")%>" />
                                                        <input type="hidden" class="ParamedicID" value="<%#: Eval("ParamedicID")%>" />
                                                        <input type="hidden" class="ParamedicCode" value="<%#: Eval("ParamedicCode")%>" />
                                                        <input type="hidden" class="ParamedicName" value="<%#: Eval("ParamedicName")%>" />
                                                        <input type="hidden" class="ChargedQuantity" value="<%#: Eval("ChargedQuantity")%>" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <%=GetLabel("Pelayanan")%>
                                                        <br />
                                                        <%=GetLabel("Dokter/Tenaga Medis")%>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <label style="font-size: x-small; font-style: italic">
                                                            <%#: Eval("ItemType")%>
                                                        </label>
                                                        <br />
                                                        <label style="font-weight: bold">
                                                            <%#: Eval("ItemName1")%>
                                                        </label>
                                                        <br />
                                                        <label style="font-size: small; font-style: oblique">
                                                            <%#: Eval("ParamedicName")%>
                                                        </label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Quantity" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                    HeaderStyle-Width="80px">
                                                    <ItemTemplate>
                                                        <%#: Eval("ChargedQuantity", "{0:N}")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
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
                    </div>
                </td>
            </tr>
        </table>
        <div style="width: 100%; text-align: right">
            <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
        </div>
    </div>
    <div id="containerDrugMSCtl" style="display: none" class="containerTransDtCtl">
        <table class="tblContentArea">
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 90%">
                        <colgroup>
                            <col style="width: 120px" />
                            <col style="width: 400px" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Item")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtItemServiceName3" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                        </tr>
                    </table>
                    <div id="containerPopupEntryDataObat" style="margin-top: 10px; display: none;">
                        <input type="hidden" id="hdnIDObat" runat="server" value="" />
                        <div class="pageTitle">
                            <%=GetLabel("Entry")%></div>
                        <fieldset id="fsEntryPopupObat" style="margin: 0">
                            <table class="tblEntryDetail" style="width: 100%">
                                <colgroup>
                                    <col style="width: 150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal" id="lblItemObat">
                                            <%=GetLabel("Detail Paket")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" value="" id="hdnDetailItemIDObat" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 120px" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtDetailItemCodeObat" ReadOnly="true" Width="100%" runat="server" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDetailItemName1Obat" ReadOnly="true" Width="80%" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal" id="lblParamedicDetailObat">
                                            <%=GetLabel("Dokter/Tenaga Medis")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" value="" id="hdnParamedicIDObat" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 120px" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtParamedicCodeObat" ReadOnly="true" CssClass="required" Width="100%"
                                                        runat="server" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtParamedicNameObat" ReadOnly="true" CssClass="required" Width="80%"
                                                        runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblMandatory">
                                            <%=GetLabel("Jumlah")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtChargedQuantityObat" CssClass="number" Min="0" Width="120px"
                                            runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <center>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <input type="button" id="btnEntryPopupSaveObat" value='<%= GetLabel("SAVE")%>' class="btnEntryPopupSaveObat w3-btn w3-hover-blue"
                                                            style="width: 80px;" />
                                                    </td>
                                                    <td>
                                                        <input type="button" id="btnEntryPopupCancelObat" value='<%= GetLabel("CANCEL")%>'
                                                            class="btnEntryPopupCancelObat w3-btn w3-hover-blue" style="width: 80px;" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <dxcp:ASPxCallbackPanel ID="cbpEntryPopupViewObat" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupViewObat"
                        ShowLoadingPanel="false" OnCallback="cbpEntryPopupViewObat_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewPopupObat').show(); }"
                            EndCallback="function(s,e){ onCbpEntryPopupViewObatEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent2" runat="server">
                                <asp:Panel runat="server" ID="pnlEntryPopupgrdViewObat" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em;">
                                    <asp:GridView ID="grdViewObat" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                        OnRowDataBound="grdViewObat_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <img class="imgEditObat imgLink" <%#: Eval("IsAllowChanged").ToString() == "True" ?  "" : "style='display:none'" %>
                                                        src='<%# Eval("IsAllowChanged").ToString() == "True" ? ResolveUrl("~/Libs/Images/Button/edit.png") : ""%>'
                                                        alt="" style="text-align:center" />
                                                    <input type="hidden" class="IDObat" value="<%#: Eval("ID")%>" />
                                                    <input type="hidden" class="ItemIDObat" value="<%#: Eval("ItemID")%>" />
                                                    <input type="hidden" class="ItemCodeObat" value="<%#: Eval("ItemCode")%>" />
                                                    <input type="hidden" class="ItemName1Obat" value="<%#: Eval("ItemName1")%>" />
                                                    <input type="hidden" class="ParamedicIDObat" value="<%#: Eval("ParamedicID")%>" />
                                                    <input type="hidden" class="ParamedicCodeObat" value="<%#: Eval("ParamedicCode")%>" />
                                                    <input type="hidden" class="ParamedicNameObat" value="<%#: Eval("ParamedicName")%>" />
                                                    <input type="hidden" class="ChargedQuantityObat" value="<%#: Eval("ChargedQuantity")%>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <%=GetLabel("Pelayanan")%>
                                                    <br />
                                                    <%=GetLabel("Dokter/Tenaga Medis")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <label style="font-size: x-small; font-style: italic">
                                                        <%#: Eval("ItemType")%>
                                                    </label>
                                                    <br />
                                                        <label style="font-weight: bold">
                                                            <%#: Eval("ItemName1")%>
                                                        </label>
                                                    <br />
                                                    <label style="font-size: small; font-style: oblique">
                                                        <%#: Eval("ParamedicName")%>
                                                    </label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Quantity" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <%#: Eval("ChargedQuantity", "{0:N}")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Data Tidak Tersedia")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </td>
            </tr>
        </table>
        <div style="width: 100%; text-align: right">
            <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
        </div>
    </div>
    <div id="containerLogisticsCtl" style="display: none" class="containerTransDtCtl">
        <table class="tblContentArea">
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 90%">
                        <colgroup>
                            <col style="width: 120px" />
                            <col style="width: 400px" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Item")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtItemServiceName2" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                        </tr>
                    </table>
                    <div id="containerPopupEntryDataBarang" style="margin-top: 10px; display: none;">
                        <input type="hidden" id="hdnIDBarang" runat="server" value="" />
                        <div class="pageTitle">
                            <%=GetLabel("Entry")%></div>
                        <fieldset id="fsEntryPopupBarang" style="margin: 0">
                            <table class="tblEntryDetail" style="width: 100%">
                                <colgroup>
                                    <col style="width: 150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal" id="lblItemBarang">
                                            <%=GetLabel("Detail Item Name")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" value="" id="hdnDetailItemIDBarang" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 120px" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtDetailItemCodeBarang" ReadOnly="true" Width="100%" runat="server" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDetailItemName1Barang" ReadOnly="true" Width="80%" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal" id="lblParamedicDetailBarang">
                                            <%=GetLabel("Paramedic")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" value="" id="hdnParamedicIDBarang" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 120px" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtParamedicCodeBarang" ReadOnly="true" CssClass="required" Width="100%"
                                                        runat="server" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtParamedicNameBarang" ReadOnly="true" CssClass="required" Width="80%"
                                                        runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblMandatory">
                                            <%=GetLabel("Quantity")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtChargedQuantityBarang" CssClass="number" Min="0" Width="120px"
                                            runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <center>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <input type="button" id="btnEntryPopupSaveBarang" value='<%= GetLabel("SAVE")%>'
                                                            class="btnEntryPopupSaveBarang w3-btn w3-hover-blue" style="width: 80px;" />
                                                    </td>
                                                    <td>
                                                        <input type="button" id="btnEntryPopupCancelBarang" value='<%= GetLabel("CANCEL")%>'
                                                            class="btnEntryPopupCancelBarang w3-btn w3-hover-blue" style="width: 80px;" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <dxcp:ASPxCallbackPanel ID="cbpEntryPopupViewBarang" runat="server" Width="100%"
                        ClientInstanceName="cbpEntryPopupViewBarang" ShowLoadingPanel="false" OnCallback="cbpEntryPopupViewBarang_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewPopupBarang').show(); }"
                            EndCallback="function(s,e){ onCbpEntryPopupViewBarangEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent3" runat="server">
                                <asp:Panel runat="server" ID="pnlEntryPopupgrdViewBarang" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em;">
                                    <asp:GridView ID="grdViewBarang" runat="server" CssClass="grdView notAllowSelect"
                                        AutoGenerateColumns="false" OnRowDataBound="grdViewBarang_RowDataBound" ShowHeaderWhenEmpty="true"
                                        EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <img class="imgEditBarang imgLink" <%#: Eval("IsAllowChanged").ToString() == "True" ?  "" : "style='display:none'" %>
                                                        src='<%# Eval("IsAllowChanged").ToString() == "True" ? ResolveUrl("~/Libs/Images/Button/edit.png") : ""%>'
                                                        alt="" style="text-align:center" />
                                                    <input type="hidden" class="IDBarang" value="<%#: Eval("ID")%>" />
                                                    <input type="hidden" class="ItemIDBarang" value="<%#: Eval("ItemID")%>" />
                                                    <input type="hidden" class="ItemCodeBarang" value="<%#: Eval("ItemCode")%>" />
                                                    <input type="hidden" class="ItemName1Barang" value="<%#: Eval("ItemName1")%>" />
                                                    <input type="hidden" class="ParamedicIDBarang" value="<%#: Eval("ParamedicID")%>" />
                                                    <input type="hidden" class="ParamedicCodeBarang" value="<%#: Eval("ParamedicCode")%>" />
                                                    <input type="hidden" class="ParamedicNameBarang" value="<%#: Eval("ParamedicName")%>" />
                                                    <input type="hidden" class="RevenueSharingIDBarang" value="<%#: Eval("RevenueSharingID")%>" />
                                                    <input type="hidden" class="RevenueSharingNameBarang" value="<%#: Eval("RevenueSharingCode")%>" />
                                                    <input type="hidden" class="ChargedQuantityBarang" value="<%#: Eval("ChargedQuantity")%>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <%=GetLabel("Pelayanan")%>
                                                    <br />
                                                    <%=GetLabel("Dokter/Tenaga Medis")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <label style="font-size: x-small; font-style: italic">
                                                        <%#: Eval("ItemType")%>
                                                    </label>
                                                    <br />
                                                        <label style="font-weight: bold">
                                                            <%#: Eval("ItemName1")%>
                                                        </label>
                                                    <br />
                                                    <label style="font-size: small; font-style: oblique">
                                                        <%#: Eval("ParamedicName")%>
                                                    </label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Quantity" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <%#: Eval("ChargedQuantity", "{0:N}")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Data Tidak Tersedia")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </td>
            </tr>
        </table>
        <div style="width: 100%; text-align: right">
            <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
        </div>
    </div>
</div>
