<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomerVirtualAccountEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.CustomerVirtualAccountEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_customermemberentryctl">
    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=hdnID.ClientID %>').val('');
        $('#<%=hdnBankID.ClientID %>').val('');
        $('#<%=txtBankCode.ClientID %>').val('');
        $('#<%=txtBankName.ClientID %>').val('');
        $('#<%=txtAccountNo.ClientID %>').val('');

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
            var entity = rowToObject($row);
            $('#<%=hdnID.ClientID %>').val(entity.ID);
            cbpEntryPopupView.PerformCallback('delete');
        }
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var entity = rowToObject($row);

        $('#<%=hdnID.ClientID %>').val(entity.ID);
        $('#<%=hdnBankID.ClientID %>').val(entity.BankID);
        $('#<%=txtBankCode.ClientID %>').val(entity.BankCode);
        $('#<%=txtBankName.ClientID %>').val(entity.BankName);
        $('#<%=txtAccountNo.ClientID %>').val(entity.AccountNo);
        if (entity.IsVirtualAccount == "True") {
            $('#<%=chkIsVirtualAccount.ClientID %>').prop('checked', true);
        } else {
            $('#<%=chkIsVirtualAccount.ClientID %>').prop('checked', false);
        }
        if (entity.IsDefault == "True") {
            $('#<%=chkIsDefault.ClientID %>').prop('checked', true);
        } else {
            $('#<%=chkIsDefault.ClientID %>').prop('checked', false);
        } 

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
                $('#lblEntryPopupAddData').click();
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

        $('#containerPopupEntryData').hide();
        hideLoadingPanel();
        addItemFilterRow();
    }

    //#region Paging
    var pageCountAvailable = parseInt('<%=PageCount %>');
    $(function () {
        setPagingDetailItem(pageCountAvailable);
    });
    //#endregion

    //#region bank
    function onGetCustomerVirtualAccountFilterExpression() {
        var filterExpression = "IsDeleted = 0";
        return filterExpression;
    }

    $('#lblBank.lblLink').live('click', function () {
        openSearchDialog('bank', onGetCustomerVirtualAccountFilterExpression(), function (value) {
            $('#<%=txtBankCode.ClientID %>').val(value);
            onTxtCustomerVirtualAccountCodeChanged(value);
        });
    });

    $('#<%=txtBankCode.ClientID %>').live('change', function () {
        onTxtCustomerVirtualAccountCodeChanged($(this).val());
    });

    function onTxtCustomerVirtualAccountCodeChanged(value) {
        var filterExpression = onGetCustomerVirtualAccountFilterExpression() + " AND BankID = '" + value + "'";
        Methods.getObject('GetBankList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnBankID.ClientID %>').val(result.BankID);
                $('#<%=txtBankCode.ClientID %>').val(result.BankCode);
                $('#<%=txtBankName.ClientID %>').val(result.BankName);
            }
            else {
                $('#<%=hdnBankID.ClientID %>').val('');
                $('#<%=txtBankCode.ClientID %>').val('');
                $('#<%=txtBankName.ClientID %>').val('');
            }
        });
    }
    //#endregion 
</script>
<div style="height: 440px; overflow-y: auto; overflow-x: hidden;">
    <input type="hidden" id="hdnBusinessPartnerID" value="" runat="server" />
    <div class="pageTitle">
        <%=GetLabel("Virtual Account")%></div>
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 70%">
                    <colgroup>
                        <col style="width: 160px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Instansi")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtCustomerName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
                <div id="containerPopupEntryData" style="margin-top: 10px; display: none;">
                    <div class="pageTitle">
                        <%=GetLabel("Entry")%></div>
                    <input type="hidden" id="hdnID" runat="server" value="" />
                    <fieldset id="fsEntryPopup" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 150px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory lblLink" id="lblBank">
                                        <%=GetLabel("Bank")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnBankID" runat="server" value="" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 100px" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtBankCode" CssClass="required" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBankName" ReadOnly="true" Width="80%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory" id="lblAccountNo">
                                        <%=GetLabel("Nomor Account")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAccountNo" Width="25%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal" id="lblIsVA">
                                        <%=GetLabel("Virtual Account")%></label>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkIsVirtualAccount" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal" id="Label1">
                                        <%=GetLabel("Default")%></label>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkIsDefault" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnEntryPopupSave" value='<%=GetLabel("Simpan")%>' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnEntryPopupCancel" value='<%=GetLabel("Batal")%>' />
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
                    <ClientSideEvents BeginCallback="function(s,e){$('#containerImgLoadingViewPopup').show();}"
                        EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s);}" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                    OnRowDataBound="grdView_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                    alt="" style="float: left; margin-left: 7px" />
                                                <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                    alt="" />
                                                <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                <input type="hidden" value="<%#:Eval("BankID") %>" bindingfield="BankID" />
                                                <input type="hidden" value="<%#:Eval("BankCode") %>" bindingfield="BankCode" />
                                                <input type="hidden" value="<%#:Eval("BankName") %>" bindingfield="BankName" />
                                                <input type="hidden" value="<%#:Eval("AccountNo") %>" bindingfield="AccountNo" />
                                                <input type="hidden" value="<%#:Eval("IsVirtualAccount") %>" bindingfield="IsVirtualAccount" />
                                                <input type="hidden" value="<%#:Eval("IsDefault") %>" bindingfield="IsDefault" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="BankCode" ItemStyle-CssClass="tdBankCode" HeaderText="Kode Bank"
                                            HeaderStyle-Width="100px" />
                                        <asp:BoundField DataField="BankName" ItemStyle-CssClass="tdBankName" HeaderText="Nama Bank" />
                                        <asp:TemplateField HeaderStyle-Width="300px" HeaderText='Nomor'>
                                            <ItemTemplate>
                                                <label class="lblNormal">
                                                    <%#:Eval("cfAccountNoCaption") %>
                                                    :
                                                    <%#:Eval("AccountNo") %></label>
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
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="pagingPopup">
                        </div>
                    </div>
                </div>
                <div style="width: 100%; text-align: center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblEntryPopupAddData">
                        <%= GetLabel("Tambah Data")%></span>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
