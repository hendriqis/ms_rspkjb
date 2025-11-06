<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PurchaseOrderTermEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inventory.Program.PurchaseOrderTermEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_PurchaseOrderTermEntryCtl">

    $('#<%:chkIsPurchaseReceiveRequired.ClientID %>').die('change');
    $('#<%:chkIsPurchaseReceiveRequired.ClientID %>').live('change', function () {
        if ($(this).is(':checked')) {
            $('#<%=hdnIsPurchaseReceiveRequired.ClientID %>').val("1");
        } else {
            $('#<%=hdnIsPurchaseReceiveRequired.ClientID %>').val("0");
        }
    });

    //#region Term Calculate

    $('#<%:chkIsTermInPercentage.ClientID %>').die('change');
    $('#<%:chkIsTermInPercentage.ClientID %>').live('change', function () {
        if ($(this).is(':checked')) {
            $('#<%=hdnIsTermInPercentage.ClientID %>').val("1");
            $('#<%=txtTermPercentage.ClientID %>').change();
            $('#<%=txtTermPercentage.ClientID%>').removeAttr('readonly');
            $('#<%=txtTermAmount.ClientID%>').attr('readonly', 'readonly');
        } else {
            $('#<%=hdnIsTermInPercentage.ClientID %>').val("0");
            $('#<%=txtTermPercentage.ClientID%>').attr('readonly', 'readonly');
            $('#<%=txtTermAmount.ClientID %>').change();
            $('#<%=txtTermAmount.ClientID%>').removeAttr('readonly');
        }
    });

    $('#<%=txtTermPercentage.ClientID %>').die('change');
    $('#<%=txtTermPercentage.ClientID %>').live('change', function () {
        var value = $('#<%:txtTermPercentage.ClientID %>').val();
        if (value > 100) {
            ShowSnackbarError("Harap isikan nilai persentase PPH dari angka 0 s/d angka 100.");
            $('#<%=txtTermPercentage.ClientID %>').val("0").trigger('changeValue');
            calculateTermAmount("fromPctg");
        } else {
            if ($('#<%=chkIsTermInPercentage.ClientID %>').is(':checked')) {
                $(this).trigger('changeValue');
                calculateTermAmount("fromPctg");
            } else {
                $(this).trigger('changeValue');
                calculateTermAmount("fromTxt");
            }
        }
    });

    $('#<%=txtTermAmount.ClientID %>').die('change');
    $('#<%=txtTermAmount.ClientID %>').live('change', function () {
        if ($('#<%=chkIsTermInPercentage.ClientID %>').is(':checked')) {
            $(this).trigger('changeValue');
            calculateTermAmount("fromPctg");
        } else {
            $(this).trigger('changeValue');
            calculateTermAmount("fromTxt");
        }
    });

    function calculateTermAmount(kode) {
        var transactionAmount = parseFloat($('#<%=txtTotalOrderSaldoCtl.ClientID %>').val().replace('.00', '').split(',').join(''));
        if (kode == "fromPctg") {
            var pctg1 = parseFloat($('#<%=txtTermPercentage.ClientID %>').val().replace('.00', '').split(',').join(''));
            var amount1 = (transactionAmount * (pctg1 / 100)).toFixed(2);

            $('#<%=txtTermPercentage.ClientID %>').val(pctg1).trigger('changeValue');
            $('#<%=txtTermAmount.ClientID %>').val(amount1).trigger('changeValue');

            $('#<%=hdnTermPercentage.ClientID %>').val(pctg1);
            $('#<%=hdnTermAmount.ClientID %>').val(amount1);
        } else if (kode == "fromTxt") {
            var amount2 = parseFloat($('#<%=txtTermAmount.ClientID %>').val().replace('.00', '').split(',').join(''));
            var pctg2 = (amount2 / (transactionAmount / 100)).toFixed(2);


            $('#<%=txtTermPercentage.ClientID %>').val(pctg2).trigger('changeValue');
            $('#<%=txtTermAmount.ClientID %>').val(amount2).trigger('changeValue');

            $('#<%=hdnTermPercentage.ClientID %>').val(pctg2);
            $('#<%=hdnTermAmount.ClientID %>').val(amount2);
        }
    }
    //#endregion

    //#region Button

    $('#btnApprovedHeader').live('click', function () {
        if ($('#<%=hdnPurchaseOrderIDCtl.ClientID %>').val() != "" && $('#<%=hdnPurchaseOrderIDCtl.ClientID %>').val() != "0") {
            cbpViewCtl.PerformCallback('approved');
        }
    });

    $('#btnVoidHeader').live('click', function () {
        if ($('#<%=hdnPurchaseOrderIDCtl.ClientID %>').val() != "" && $('#<%=hdnPurchaseOrderIDCtl.ClientID %>').val() != "0") {
            cbpViewCtl.PerformCallback('void');
        }
    });

    $('#lblAddDataTerm').live('click', function (evt) {
        $('#<%=hdnEntryID.ClientID %>').val('');

        $('#<%=txtTermRemarks.ClientID %>').val('');
        $('#<%=txtTermCondition.ClientID %>').val('');
        $('#<%=chkIsTermInPercentage.ClientID %>').prop("checked", false);
        $('#<%=hdnIsTermInPercentage.ClientID %>').val('0');
        $('#<%=txtTermPercentage.ClientID %>').val('0');
        $('#<%=hdnTermPercentage.ClientID %>').val('0');
        $('#<%=txtTermAmount.ClientID %>').val('0');
        $('#<%=hdnTermAmount.ClientID %>').val('0');
        $('#<%=chkIsPurchaseReceiveRequired.ClientID %>').prop("checked", false);

        if ($('#<%=chkIsPurchaseReceiveRequired.ClientID %>').is(':checked')) {
            $('#<%=hdnIsTermInPercentage.ClientID %>').val("1");
            $('#<%=txtTermPercentage.ClientID %>').change();
            $('#<%=txtTermPercentage.ClientID%>').removeAttr('readonly');
            $('#<%=txtTermAmount.ClientID%>').attr('readonly', 'readonly');
        } else {
            $('#<%=hdnIsTermInPercentage.ClientID %>').val("0");
            $('#<%=txtTermPercentage.ClientID%>').attr('readonly', 'readonly');
            $('#<%=txtTermAmount.ClientID %>').change();
            $('#<%=txtTermAmount.ClientID%>').removeAttr('readonly');
        }

        $('#<%=txtTermCounter.ClientID %>').val('1');
        $('#<%=txtTermCounter.ClientID%>').removeAttr('readonly');

        $('#containerEntry').show();
    });

    $('#btnRefreshCtl').live('click', function () {
        cbpViewCtl.PerformCallback('load');
    });

    $('#btnCancelCtl').live('click', function () {
        $('#containerEntry').hide();
    });

    $('#btnSaveCtl').live('click', function (evt) {
        cbpViewCtl.PerformCallback('save');
    });

    $('.imgEditPOTermCtl.imgLink').die('click');
    $('.imgEditPOTermCtl.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        var entity = rowToObject($row);

        $('#<%=hdnEntryID.ClientID %>').val(entity.PurchaseOrderTermID);

        $('#<%=txtTermRemarks.ClientID %>').val(entity.TermRemarks);
        $('#<%=txtTermCondition.ClientID %>').val(entity.TermConditions);

        if (entity.TermInPercentage == "True") {
            $('#<%=chkIsTermInPercentage.ClientID %>').prop("checked", true);

            $('#<%=hdnIsTermInPercentage.ClientID %>').val("1");
            $('#<%=txtTermPercentage.ClientID%>').removeAttr('readonly');
            $('#<%=txtTermAmount.ClientID%>').attr('readonly', 'readonly');
        } else {
            $('#<%=chkIsTermInPercentage.ClientID %>').prop("checked", false);

            $('#<%=hdnIsTermInPercentage.ClientID %>').val("0");
            $('#<%=txtTermPercentage.ClientID%>').attr('readonly', 'readonly');
            $('#<%=txtTermAmount.ClientID%>').removeAttr('readonly');
        }
        $('#<%=txtTermPercentage.ClientID %>').val(entity.TermPercentage).trigger('changeValue');
        $('#<%=hdnTermPercentage.ClientID %>').val(entity.TermPercentage);
        $('#<%=txtTermAmount.ClientID %>').val(entity.TermAmount).trigger('changeValue');
        $('#<%=hdnTermAmount.ClientID %>').val(entity.TermAmount);

        if (entity.IsPurchaseReceiveRequired == "True") {
            $('#<%=chkIsPurchaseReceiveRequired.ClientID %>').prop("checked", true);
        } else {
            $('#<%=chkIsPurchaseReceiveRequired.ClientID %>').prop("checked", false);
        }
        $('#<%=txtTransactionStatus.ClientID %>').val(entity.TransactionStatus);

        var counter = entity.TermCounter
        if (counter != '0') {
            $('#<%=txtTermCounter.ClientID%>').attr('readonly', 'readonly');
            $('#<%=txtTermCounter.ClientID %>').val(entity.TermCounter);
        }

        $('#containerEntry').show();
    });

    $('.imgDeletePOTermCtl.imgLink').die('click');
    $('.imgDeletePOTermCtl.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        showToastConfirmation('Are You Sure Want To Delete?', function (result) {
            if (result) {
                var entity = rowToObject($row);
                $('#<%=hdnEntryID.ClientID %>').val(entity.PurchaseOrderTermID);
                cbpViewCtl.PerformCallback('delete');
            }
        });
    });

    $('.imgReopenPOTermCtl.imgLink').die('click');
    $('.imgReopenPOTermCtl.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        showToastConfirmation('Are You Sure Want To Reopen?', function (result) {
            if (result) {
                var entity = rowToObject($row);
                $('#<%=hdnEntryID.ClientID %>').val(entity.PurchaseOrderTermID);
                cbpViewCtl.PerformCallback('reopen');
            }
        });
    });

    //#endregion

    function onCbpViewCtlEndCallback(s, e) {
        var result = s.cpResult.split('|');
        if (result[0] == 'save') {
            if (result[1] != 'success') {
                if (result[2] != '')
                    showToast('Save Failed', 'Error Message : ' + result[2]);
                else
                    showToast('Save Failed', '');
            } else {
                $('#containerEntry').hide();
                cbpViewCtl.PerformCallback('load');
            }
        }
        if (result[0] == 'delete') {
            if (result[1] != 'success') {
                if (result[2] != '')
                    showToast('Delete Failed', 'Error Message : ' + result[2]);
                else
                    showToast('Delete Failed', '');
            } else {
                $('#containerEntry').hide();
                cbpViewCtl.PerformCallback('load');
            }
        }
        if (result[0] == 'approved') {
            if (result[1] != 'success') {
                if (result[2] != '')
                    showToast('Approved Failed', 'Error Message : ' + result[2]);
                else
                    showToast('Approved Failed', '');
            } else {
                $('#containerEntry').hide();
                cbpViewCtl.PerformCallback('load');
            }
        }
        if (result[0] == 'void') {
            if (result[1] != 'success') {
                if (result[2] != '')
                    showToast('Void Failed', 'Error Message : ' + result[2]);
                else
                    showToast('Void Failed', '');
            } else {
                $('#containerEntry').hide();
                cbpViewCtl.PerformCallback('load');
            }
        }
        hideLoadingPanel();
    }

</script>
<input type="hidden" value="" id="hdnPurchaseOrderIDCtl" runat="server" />
<div style="height: 450px; overflow-y: scroll;">
    <dxcp:ASPxCallbackPanel ID="cbpMainPopup" runat="server" Width="100%" ClientInstanceName="cbpMainPopup"
        ShowLoadingPanel="false">
        <PanelCollection>
            <dx:PanelContent ID="PanelContent2">
                <fieldset id="fsEntryPopup" style="margin: 0">
                    <asp:Panel runat="server" ID="Panel1" Style="width: 100%; margin-left: auto; margin-right: auto;
                        position: relative; font-size: 0.95em;">
                        <table>
                            <tr>
                                <td>
                                    <input type="button" id="btnApprovedHeader" class="w3-button w3-green w3-round-large"
                                        value='<%= GetLabel("Approved")%>' />
                                </td>
                                <td>
                                    <input type="button" id="btnVoidHeader" class="w3-button w3-red w3-round-large" value='<%= GetLabel("Void")%>' />
                                </td>
                                <td style="font-size: medium; color: Maroon; font-style: italic; font-weight: bold;
                                    text-align: right" class="blink-alert">
                                    <%=GetLabel("<b>Approved dan Void</b> berfungsi hanya utk semua detail termin dgn status OPEN saja.") %>
                                </td>
                            </tr>
                        </table>
                        <table class="tblContentArea">
                            <colgroup>
                                <col style="width: 50%" />
                                <col style="width: 50%" />
                            </colgroup>
                            <tr>
                                <td style="padding: 5px; vertical-align: top">
                                    <table class="tblEntryContent" style="width: 100%">
                                        <colgroup>
                                            <col style="width: 150px" />
                                            <col style="width: 500px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                                <label>
                                                    <%=GetLabel("No. Pemesanan")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPurchaseOrderNoCtl" Width="150px" runat="server" ReadOnly="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                                <label>
                                                    <%=GetLabel("Supplier/Penyedia")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSupplierCtl" Width="100%" runat="server" ReadOnly="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                                <label>
                                                    <%=GetLabel("Total Nilai Pemesanan")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTotalOrderSaldoCtl" Width="150px" runat="server" ReadOnly="true"
                                                    CssClass="txtCurrency" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="padding: 5px; vertical-align: top">
                                    <table class="tblEntryContent" style="width: 100%">
                                        <colgroup>
                                            <col style="width: 150px" />
                                            <col style="width: 500px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal" style="text-decoration: underline">
                                                    <%=GetLabel("Filter Berdasarkan")%></label>
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox ID="cboFilterBy" ClientInstanceName="cboFilterBy" Width="250px"
                                                    runat="server" BackColor="Yellow">
                                                </dxe:ASPxComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <input type="button" id="btnRefreshCtl" value="R e f r e s h" class="btnRefreshCtl w3-button w3-blue w3-border w3-border-blue w3-round-large" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div id="containerEntry" style="margin-top: 4px; display: none;">
                                        <div class="pageTitle">
                                            <%=GetLabel("Entry")%></div>
                                        <fieldset id="fsTrxPopup" style="margin: 0">
                                            <input type="hidden" value="" id="hdnEntryID" runat="server" />
                                            <table style="width: 100%" class="tblEntryDetail">
                                                <colgroup>
                                                    <col style="width: 50%" />
                                                </colgroup>
                                                <tr>
                                                    <td valign="top">
                                                        <table style="width: 100%">
                                                            <colgroup>
                                                                <col style="width: 170px" />
                                                            </colgroup>
                                                            <tr>
                                                                <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                                                    <label class="lblNormal">
                                                                        <%=GetLabel("Remarks")%></label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtTermRemarks" Width="400px" runat="server" TextMode="MultiLine" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                                                    <label class="lblNormal">
                                                                        <%=GetLabel("Condition")%></label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtTermCondition" Width="400px" runat="server" TextMode="MultiLine" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdLabel">
                                                                    <label class="lblNormal">
                                                                        <%=GetLabel("Term (Percent / Amount)")%></label>
                                                                </td>
                                                                <td>
                                                                    <table cellpadding="0" cellspacing="0">
                                                                        <colgroup>
                                                                            <col style="width: 10%" />
                                                                            <col style="width: 100px" />
                                                                            <col style="width: 10px" />
                                                                            <col />
                                                                        </colgroup>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:CheckBox ID="chkIsTermInPercentage" Checked="true" Width="100%" runat="server" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtTermPercentage" CssClass="number" Width="100%" runat="server"
                                                                                    ReadOnly="true" />
                                                                            </td>
                                                                            <td>
                                                                                &nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <input type="hidden" value="0" id="hdnIsTermInPercentage" runat="server" />
                                                                                <input type="hidden" value="0" id="hdnTermAmount" runat="server" />
                                                                                <input type="hidden" value="0" id="hdnTermPercentage" runat="server" />
                                                                                <asp:TextBox ID="txtTermAmount" CssClass="txtCurrency" Width="100%" runat="server"
                                                                                    ReadOnly="true" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdLabel">
                                                                    <label class="lblNormal">
                                                                        <%=GetLabel("Jenis PPh")%></label>
                                                                </td>
                                                                <td>
                                                                    <dxe:ASPxComboBox ID="cboPPHType" ClientInstanceName="cboPPHType" Width="185px" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdLabel">
                                                                    <label class="lblNormal">
                                                                        <%=GetLabel("POR Required")%></label>
                                                                </td>
                                                                <td>
                                                                    <table cellpadding="0" cellspacing="0">
                                                                        <colgroup>
                                                                            <col style="width: 10%" />
                                                                            <col style="width: 100px" />
                                                                            <col style="width: 10px" />
                                                                            <col />
                                                                        </colgroup>
                                                                        <tr>
                                                                            <td>
                                                                                <input type="hidden" value="0" id="hdnIsPurchaseReceiveRequired" runat="server" />
                                                                                <asp:CheckBox ID="chkIsPurchaseReceiveRequired" Checked="true" Width="100%" runat="server" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                                                    <label class="lblNormal">
                                                                        <%=GetLabel("Status")%></label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtTransactionStatus" Width="130px" runat="server" ReadOnly="true"
                                                                        Style="text-align: center" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="tdLabel">
                                                                    <label class="lblNormal">
                                                                        <%=GetLabel("Cicilan")%></label>
                                                                </td>
                                                                <td>
                                                                    <table cellpadding="0" cellspacing="0">
                                                                        <colgroup>
                                                                            <col style="width: 65px" />
                                                                            <col style="width: 10px" />
                                                                            <col />
                                                                        </colgroup>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:TextBox ID="txtTermCounter" CssClass="number" Min="0" Width="50px" runat="server" />
                                                                            </td>
                                                                            <td>
                                                                                <label class="lblNormal">
                                                                                    <%=GetLabel("X")%></label>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <input type="button" id="btnSaveCtl" value='<%= GetLabel("Save")%>' />
                                                                            </td>
                                                                            <td>
                                                                                <input type="button" id="btnCancelCtl" value='<%= GetLabel("Cancel")%>' />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </div>
                                    <dxcp:ASPxCallbackPanel ID="cbpViewCtl" runat="server" Width="100%" ClientInstanceName="cbpViewCtl"
                                        ShowLoadingPanel="false" OnCallback="cbpViewCtl_Callback">
                                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewCtlEndCallback(s,e); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent1" runat="server">
                                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                                    position: relative; font-size: 0.95em;">
                                                    <asp:GridView ID="grdViewTerm" runat="server" CssClass="grdService grdNormal notAllowSelect"
                                                        OnRowDataBound="grdViewTerm_RowDataBound" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true"
                                                        EmptyDataRowStyle-CssClass="trEmpty">
                                                        <Columns>
                                                            <asp:BoundField DataField="PurchaseOrderTermID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                            <asp:TemplateField ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <table cellpadding="0" cellspacing="0">
                                                                        <tr>
                                                                            <td>
                                                                                <img class="imgEditPOTermCtl <%#: Eval("GCTransactionStatus").ToString() == "X121^001" ? "imgLink" : "imgDisabled"%>"
                                                                                    title='<%=GetLabel("Edit")%>' src='<%#: Eval("GCTransactionStatus").ToString() == "X121^001" ? ResolveUrl("~/Libs/Images/Button/edit.png") : ResolveUrl("~/Libs/Images/Button/edit_disabled.png")%>'
                                                                                    alt="" style="margin-right: 2px" />
                                                                            </td>
                                                                            <td style="width: 1px">
                                                                                &nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <img class="imgDeletePOTermCtl <%#: Eval("GCTransactionStatus").ToString() == "X121^001" ? "imgLink" : "imgDisabled"%>"
                                                                                    title='<%=GetLabel("Delete")%>' src='<%#: Eval("GCTransactionStatus").ToString() == "X121^001" ? ResolveUrl("~/Libs/Images/Button/delete.png") : ResolveUrl("~/Libs/Images/Button/delete_disabled.png")%>'
                                                                                    alt="" style="margin-right: 2px" />
                                                                            </td>
                                                                            <td style="width: 1px">
                                                                                &nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <img class="imgReopenPOTermCtl <%#: Eval("GCTransactionStatus").ToString() == "X121^003" ? "imgLink" : "imgDisabled"%>"
                                                                                    title='<%=GetLabel("Reopen")%>' src='<%#: Eval("GCTransactionStatus").ToString() == "X121^003" ? ResolveUrl("~/Libs/Images/Button/unlock.png") : ResolveUrl("~/Libs/Images/Button/unlock_disabled.png")%>'
                                                                                    alt="" style="margin-right: 2px" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <input type="hidden" value="<%#:Eval("PurchaseOrderTermID") %>" bindingfield="PurchaseOrderTermID" />
                                                                    <input type="hidden" value="<%#:Eval("TermRemarks") %>" bindingfield="TermRemarks" />
                                                                    <input type="hidden" value="<%#:Eval("TermConditions") %>" bindingfield="TermConditions" />
                                                                    <input type="hidden" value="<%#:Eval("TermInPercentage") %>" bindingfield="TermInPercentage" />
                                                                    <input type="hidden" value="<%#:Eval("TermPercentage") %>" bindingfield="TermPercentage" />
                                                                    <input type="hidden" value="<%#:Eval("TermAmount") %>" bindingfield="TermAmount" />
                                                                    <input type="hidden" value="<%#:Eval("TermCounter") %>" bindingfield="TermCounter" />
                                                                    <input type="hidden" value="<%#:Eval("IsPurchaseReceiveRequired") %>" bindingfield="IsPurchaseReceiveRequired" />
                                                                    <input type="hidden" value="<%#:Eval("TransactionStatus") %>" bindingfield="TransactionStatus" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ItemStyle-Width="140px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                HeaderText="Remarks">
                                                                <ItemTemplate>
                                                                    <div>
                                                                        <label class="lblNormal">
                                                                            <%#:Eval("TermRemarks") %></label>
                                                                    </div>
                                                                    <div>
                                                                        <label class="lblNormal">
                                                                            <%#:Eval("PurchaseInvoiceNo") %></label>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="TermConditions" HeaderText="Conditions" HeaderStyle-Width="200px"
                                                                HeaderStyle-HorizontalAlign="Left" />
                                                            <asp:BoundField DataField="cfTermPercentageInString" HeaderStyle-HorizontalAlign="Right"
                                                                ItemStyle-HorizontalAlign="Right" HeaderText="Percentage" HeaderStyle-Width="50px" />
                                                            <asp:BoundField DataField="cfTermAmountInString" HeaderStyle-HorizontalAlign="Right"
                                                                ItemStyle-HorizontalAlign="Right" HeaderText="Amount" HeaderStyle-Width="100px" />
                                                            <asp:BoundField DataField="IsPurchaseReceiveRequired" HeaderStyle-HorizontalAlign="Center"
                                                                ItemStyle-HorizontalAlign="Center" HeaderText="POR Required" HeaderStyle-Width="60px" />
                                                            <asp:BoundField DataField="TransactionStatus" HeaderStyle-HorizontalAlign="Center"
                                                                ItemStyle-HorizontalAlign="Center" HeaderText="Status" HeaderStyle-Width="100px" />
                                                            <asp:TemplateField ItemStyle-Width="140px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="Created Info">
                                                                <ItemTemplate>
                                                                    <div>
                                                                        <label class="lblNormal">
                                                                            <%#:Eval("CreatedByName") %></label>
                                                                    </div>
                                                                    <div>
                                                                        <label class="lblNormal">
                                                                            <%#:Eval("cfCreatedDateInStringFull") %></label>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ItemStyle-Width="140px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                                HeaderText="Last Updated Info">
                                                                <ItemTemplate>
                                                                    <div>
                                                                        <label class="lblNormal">
                                                                            <%#:Eval("LastUpdatedByName") %></label>
                                                                    </div>
                                                                    <div>
                                                                        <label class="lblNormal">
                                                                            <%#:Eval("cfLastUpdatedDateInStringFull") %></label>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            <%=GetLabel("No Data To Display")%>
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dxcp:ASPxCallbackPanel>
                                    <div class="imgLoadingGrdView" id="containerImgLoadingView">
                                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                    </div>
                                    <div style="width: 100%; text-align: center">
                                        <span class="lblLink" id="lblAddDataTerm">
                                            <%= GetLabel("Add Data")%></span>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </fieldset>
            </dx:PanelContent>
        </PanelCollection>
    </dxcp:ASPxCallbackPanel>
</div>
