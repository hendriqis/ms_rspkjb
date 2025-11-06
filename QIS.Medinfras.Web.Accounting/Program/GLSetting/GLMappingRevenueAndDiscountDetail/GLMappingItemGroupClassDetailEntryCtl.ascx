<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GLMappingItemGroupClassDetailEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Accounting.Program.GLMappingItemGroupClassDetailEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_GLMappingItemGroupClassEntryCtl">

    function onGetGLAccountFilterExpression() {
        var filterExpression = "IsHeader = 0 AND IsDeleted = 0";
        return filterExpression;
    }

    //#region ItemGroup
    function getItemGroupFilterExpression() {
        var filterExpression = "IsDeleted = 0";
        return filterExpression;
    }

    $('#lblItemGroup.lblLink').live('click', function () {
        openSearchDialog('itemgroup', getItemGroupFilterExpression(), function (value) {
            $('#<%=txtItemGroupCode.ClientID %>').val(value);
            onTxtItemGroupCodeChanged(value);
        });
    });

    $('#<%=txtItemGroupCode.ClientID %>').die('change');
    $('#<%=txtItemGroupCode.ClientID %>').live('change', function () {
        onTxtItemGroupCodeChanged($(this).val());
    });

    function onTxtItemGroupCodeChanged(value) {
        var filterExpression = getItemGroupFilterExpression() + " AND ItemGroupCode = '" + value + "'";
        Methods.getObject('GetItemGroupMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnItemGroupID.ClientID %>').val(result.ItemGroupID);
                $('#<%=txtItemGroupName.ClientID %>').val(result.ItemGroupName1);
            }
            else {
                $('#<%=hdnItemGroupID.ClientID %>').val('');
                $('#<%=txtItemGroupCode.ClientID %>').val('');
                $('#<%=txtItemGroupName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Class
    function getClassFilterExpression() {
        var filterExpression = "IsDeleted = 0";
        return filterExpression;
    }

    $('#lblClass.lblLink').live('click', function () {
        openSearchDialog('classcare', getClassFilterExpression(), function (value) {
            $('#<%=txtClassCode.ClientID %>').val(value);
            onTxtClassCodeChanged(value);
        });
    });

    $('#<%=txtClassCode.ClientID %>').die('change');
    $('#<%=txtClassCode.ClientID %>').live('change', function () {
        onTxtClassCodeChanged($(this).val());
    });

    function onTxtClassCodeChanged(value) {
        var filterExpression = getClassFilterExpression() + " AND ClassCode = '" + value + "'";
        Methods.getObject('GetClassCareList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnClassID.ClientID %>').val(result.ClassID);
                $('#<%=txtClassName.ClientID %>').val(result.ClassName);
            }
            else {
                $('#<%=hdnClassID.ClientID %>').val('');
                $('#<%=txtClassCode.ClientID %>').val('');
                $('#<%=txtClassName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Button
    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=hdnItemGroupID.ClientID %>').val('');
        $('#<%=txtItemGroupCode.ClientID %>').val('');
        $('#<%=txtItemGroupName.ClientID %>').val('');
        $('#<%=hdnClassID.ClientID %>').val('');
        $('#<%=txtClassCode.ClientID %>').val('');
        $('#<%=txtClassName.ClientID %>').val('');

        $('#containerPopupEntryData').show();
    });

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function () {
        if (confirm("Are You Sure Want To Delete This Data?")) {
            var ID = $(this).closest('tr').find('.ID').val();
            $('#<%=hdnIDCtl.ClientID %>').val(ID);

            cbpEntryPopupView.PerformCallback('delete');
        }
    });

    $('#btnEntryPopupSave').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup')) {
            var itemGroupID = $('#<%=hdnItemGroupID.ClientID %>').val();
            var classID = $('#<%=hdnClassID.ClientID %>').val();

            if (itemGroupID != 0 && itemGroupID != "" && itemGroupID != null) {
                if (classID != 0 && classID != "" && classID != null) {
                    cbpEntryPopupView.PerformCallback('save');
                } else {
                    displayErrorMessageBox('INFORMASI', "Harap isi isian Kelas terlebih dahulu.");
                }
            } else {
                displayErrorMessageBox('INFORMASI', "Harap isi isian Kelompok Item terlebih dahulu.");
            }
        }
        return false;
    });

    $('#btnEntryPopupCancel').live('click', function () {
        $('#containerPopupEntryData').hide();
    });

    function onCbpEntryPopupViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail') {
                showToast('Save Failed', 'Error Message : ' + param[2]);
                $('#containerImgLoadingViewPopup').hide();
            }
            cbpEntryPopupView.PerformCallback('refresh');
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail') {
                showToast('Delete Failed', 'Error Message : ' + param[2]);
                $('#containerImgLoadingViewPopup').hide();
            }
            cbpEntryPopupView.PerformCallback('refresh');
        }
        $('#containerPopupEntryData').hide();
        $('#containerImgLoadingViewPopup').hide();
    } //#endregion

    //#region GL Account 1
    $('#lblGLAccount1.lblLink').click(function () {
        openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
            $('#<%=txtGLAccount1Code.ClientID %>').val(value);
            onTxtGLAccount1CodeChanged(value);
        });
    });

    $('#<%=txtGLAccount1Code.ClientID %>').change(function () {
        onTxtGLAccount1CodeChanged($(this).val());
    });

    function onTxtGLAccount1CodeChanged(value) {
        var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
        Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnGLAccount1ID.ClientID %>').val(result.GLAccountID);
                $('#<%=txtGLAccount1Code.ClientID %>').val(result.GLAccountNo);
                $('#<%=txtGLAccount1Name.ClientID %>').val(result.GLAccountName);
            }
            else {
                $('#<%=hdnGLAccount1ID.ClientID %>').val('');
                $('#<%=txtGLAccount1Code.ClientID %>').val('');
                $('#<%=txtGLAccount1Name.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region GL Account 2
    $('#lblGLAccount2.lblLink').click(function () {
        openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
            $('#<%=txtGLAccount2Code.ClientID %>').val(value);
            onTxtGLAccount2CodeChanged(value);
        });
    });

    $('#<%=txtGLAccount2Code.ClientID %>').change(function () {
        onTxtGLAccount2CodeChanged($(this).val());
    });

    function onTxtGLAccount2CodeChanged(value) {
        var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
        Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnGLAccount2ID.ClientID %>').val(result.GLAccountID);
                $('#<%=txtGLAccount2Code.ClientID %>').val(result.GLAccountNo);
                $('#<%=txtGLAccount2Name.ClientID %>').val(result.GLAccountName);
            }
            else {
                $('#<%=hdnGLAccount2ID.ClientID %>').val('');
                $('#<%=txtGLAccount2Code.ClientID %>').val('');
                $('#<%=txtGLAccount2Name.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region GL Account 3
    $('#lblGLAccount3.lblLink').click(function () {
        openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
            $('#<%=txtGLAccount3Code.ClientID %>').val(value);
            onTxtGLAccount3CodeChanged(value);
        });
    });

    $('#<%=txtGLAccount3Code.ClientID %>').change(function () {
        onTxtGLAccount3CodeChanged($(this).val());
    });

    function onTxtGLAccount3CodeChanged(value) {
        var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
        Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnGLAccount3ID.ClientID %>').val(result.GLAccountID);
                $('#<%=txtGLAccount3Code.ClientID %>').val(result.GLAccountNo);
                $('#<%=txtGLAccount3Name.ClientID %>').val(result.GLAccountName);
            }
            else {
                $('#<%=hdnGLAccount3ID.ClientID %>').val('');
                $('#<%=txtGLAccount3Code.ClientID %>').val('');
                $('#<%=txtGLAccount3Name.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Discount GL Account 1
    $('#lblDiscountGLAccount1.lblLink').click(function () {
        openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
            $('#<%=txtDiscountGLAccount1Code.ClientID %>').val(value);
            onTxtDiscountGLAccount1CodeChanged(value);
        });
    });

    $('#<%=txtDiscountGLAccount1Code.ClientID %>').change(function () {
        onTxtDiscountGLAccount1CodeChanged($(this).val());
    });

    function onTxtDiscountGLAccount1CodeChanged(value) {
        var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
        Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnDiscountGLAccount1ID.ClientID %>').val(result.GLAccountID);
                $('#<%=txtDiscountGLAccount1Code.ClientID %>').val(result.GLAccountNo);
                $('#<%=txtDiscountGLAccount1Name.ClientID %>').val(result.GLAccountName);
            }
            else {
                $('#<%=hdnDiscountGLAccount1ID.ClientID %>').val('');
                $('#<%=txtDiscountGLAccount1Code.ClientID %>').val('');
                $('#<%=txtDiscountGLAccount1Name.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Discount GL Account 2
    $('#lblDiscountGLAccount2.lblLink').click(function () {
        openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
            $('#<%=txtDiscountGLAccount2Code.ClientID %>').val(value);
            onTxtDiscountGLAccount2CodeChanged(value);
        });
    });

    $('#<%=txtDiscountGLAccount2Code.ClientID %>').change(function () {
        onTxtDiscountGLAccount2CodeChanged($(this).val());
    });

    function onTxtDiscountGLAccount2CodeChanged(value) {
        var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
        Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnDiscountGLAccount2ID.ClientID %>').val(result.GLAccountID);
                $('#<%=txtDiscountGLAccount2Code.ClientID %>').val(result.GLAccountNo);
                $('#<%=txtDiscountGLAccount2Name.ClientID %>').val(result.GLAccountName);
            }
            else {
                $('#<%=hdnDiscountGLAccount2ID.ClientID %>').val('');
                $('#<%=txtDiscountGLAccount2Code.ClientID %>').val('');
                $('#<%=txtDiscountGLAccount2Name.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Discount GL Account 3
    $('#lblDiscountGLAccount3.lblLink').click(function () {
        openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
            $('#<%=txtDiscountGLAccount3Code.ClientID %>').val(value);
            onTxtDiscountGLAccount3CodeChanged(value);
        });
    });

    $('#<%=txtDiscountGLAccount3Code.ClientID %>').change(function () {
        onTxtDiscountGLAccount3CodeChanged($(this).val());
    });

    function onTxtDiscountGLAccount3CodeChanged(value) {
        var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
        Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnDiscountGLAccount3ID.ClientID %>').val(result.GLAccountID);
                $('#<%=txtDiscountGLAccount3Code.ClientID %>').val(result.GLAccountNo);
                $('#<%=txtDiscountGLAccount3Name.ClientID %>').val(result.GLAccountName);
            }
            else {
                $('#<%=hdnDiscountGLAccount3ID.ClientID %>').val('');
                $('#<%=txtDiscountGLAccount3Code.ClientID %>').val('');
                $('#<%=txtDiscountGLAccount3Name.ClientID %>').val('');
            }
        });
    }
    //#endregion

</script>
<div style="height: 450px; overflow-y: auto">
    <input type="hidden" id="hdnGLAccountIDCtl" value="" runat="server" />
    <input type="hidden" id="hdnGLPositionCtl" value="" runat="server" />
    <input type="hidden" id="hdnIDCtl" value="" runat="server" />
    <input type="hidden" id="hdnComp1Text" runat="server" value="" />
    <input type="hidden" id="hdnComp2Text" runat="server" value="" />
    <input type="hidden" id="hdnComp3Text" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 1px; vertical-align: top">
                <div id="containerPopupEntryData" style="margin-top: 10px; display: none;">
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
                                    <label class="lblLink lblMandatory" id="lblItemGroup">
                                        <%=GetLabel("Item Group")%></label>
                                </td>
                                <td colspan="2">
                                    <input type="hidden" value="" id="hdnItemGroupID" runat="server" />
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 120px" />
                                            <col style="width: 3px" />
                                            <col style="width: 250px" />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtItemGroupCode" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtItemGroupName" ReadOnly="true" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink lblMandatory" id="lblClass">
                                        <%=GetLabel("Class")%></label>
                                </td>
                                <td colspan="2">
                                    <input type="hidden" value="" id="hdnClassID" runat="server" />
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 120px" />
                                            <col style="width: 3px" />
                                            <col style="width: 250px" />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtClassCode" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtClassName" ReadOnly="true" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink lblMandatory" id="lblGLAccount1">
                                        <%=GetLabel("Perkiraan Tariff Sarana")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnGLAccount1ID" runat="server" />
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 200px" />
                                            <col style="width: 3px" />
                                            <col style="width: 350px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtGLAccount1Code" Width="100%" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtGLAccount1Name" ReadOnly="true" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink lblMandatory" id="lblGLAccount2">
                                        <%=GetLabel("Perkiraan Tariff Pelayanan")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnGLAccount2ID" runat="server" />
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 200px" />
                                            <col style="width: 3px" />
                                            <col style="width: 350px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtGLAccount2Code" Width="100%" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtGLAccount2Name" ReadOnly="true" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink lblMandatory" id="lblGLAccount3">
                                        <%=GetLabel("Perkiraan Tariff Lain-lain")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnGLAccount3ID" runat="server" />
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 200px" />
                                            <col style="width: 3px" />
                                            <col style="width: 350px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtGLAccount3Code" Width="100%" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtGLAccount3Name" ReadOnly="true" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink lblMandatory" id="lblDiscountGLAccount1">
                                        <%=GetLabel("Diskon Tariff Sarana")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnDiscountGLAccount1ID" runat="server" />
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 200px" />
                                            <col style="width: 3px" />
                                            <col style="width: 350px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtDiscountGLAccount1Code" Width="100%" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtDiscountGLAccount1Name" ReadOnly="true" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink lblMandatory" id="lblDiscountGLAccount2">
                                        <%=GetLabel("Diskon Tariff Pelayanan")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnDiscountGLAccount2ID" runat="server" />
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 200px" />
                                            <col style="width: 3px" />
                                            <col style="width: 350px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtDiscountGLAccount2Code" Width="100%" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtDiscountGLAccount2Name" ReadOnly="true" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink lblMandatory" id="lblDiscountGLAccount3">
                                        <%=GetLabel("Diskon Tariff Lain-Lain")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnDiscountGLAccount3ID" runat="server" />
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 200px" />
                                            <col style="width: 3px" />
                                            <col style="width: 350px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtDiscountGLAccount3Code" Width="100%" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtDiscountGLAccount3Name" ReadOnly="true" Width="100%" />
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
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; position: relative;
                                font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                    alt="" />
                                                <input type="hidden" class="ID" value="<%#: Eval("ID")%>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ItemGroupCode" ItemStyle-CssClass="ItemGroupCode" HeaderText="ItemGroupCode"
                                            HeaderStyle-Width="120px" />
                                        <asp:BoundField DataField="ItemGroupName1" ItemStyle-CssClass="ItemGroupName1" HeaderText="ItemGroupName1" />
                                        <asp:BoundField DataField="ClassName" ItemStyle-CssClass="ClassName" HeaderText="ClassName"
                                            HeaderStyle-Width="150px" />
                                        <asp:TemplateField HeaderStyle-Width="180px">
                                            <HeaderTemplate>
                                                <div style="padding-left: 3px; text-align: center">
                                                    <%=GetLabel("Sarana")%>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="padding-left: 3px; text-align: center">
                                                    <%#: Eval("RevenueGLAccountNo1")%></div>
                                                <div style="padding-left: 3px; text-align: center">
                                                    <%#: Eval("RevenueGLAccountName1")%></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="180px">
                                            <HeaderTemplate>
                                                <div style="padding-left: 3px; text-align: center">
                                                    <%=GetLabel("Pelayanan")%>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="padding-left: 3px; text-align: center">
                                                    <%#: Eval("RevenueGLAccountNo2")%></div>
                                                <div style="padding-left: 3px; text-align: center">
                                                    <%#: Eval("RevenueGLAccountName2")%></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="180px">
                                            <HeaderTemplate>
                                                <div style="padding-left: 3px; text-align: center">
                                                    <%=GetLabel("Lain-Lain")%>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="padding-left: 3px; text-align: center">
                                                    <%#: Eval("RevenueGLAccountNo3")%></div>
                                                <div style="padding-left: 3px; text-align: center">
                                                    <%#: Eval("RevenueGLAccountName3")%></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="180px">
                                            <HeaderTemplate>
                                                <div style="padding-left: 3px; text-align: center">
                                                    <%=GetLabel("Diskon Sarana")%>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="padding-left: 3px; text-align: center">
                                                    <%#: Eval("DiscountGLAccountNo1")%></div>
                                                <div style="padding-left: 3px; text-align: center">
                                                    <%#: Eval("DiscountGLAccountName1")%></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="180px">
                                            <HeaderTemplate>
                                                <div style="padding-left: 3px; text-align: center">
                                                    <%=GetLabel("Diskon Pelayanan")%>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="padding-left: 3px; text-align: center">
                                                    <%#: Eval("DiscountGLAccountNo2")%></div>
                                                <div style="padding-left: 3px; text-align: center">
                                                    <%#: Eval("DiscountGLAccountName2")%></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="180px">
                                            <HeaderTemplate>
                                                <div style="padding-left: 3px; text-align: center">
                                                    <%=GetLabel("Diskon Pelayanan")%>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="padding-left: 3px; text-align: center">
                                                    <%#: Eval("DiscountGLAccountNo2")%></div>
                                                <div style="padding-left: 3px; text-align: center">
                                                    <%#: Eval("DiscountGLAccountName2")%></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="180px">
                                            <HeaderTemplate>
                                                <div style="padding-left: 3px; text-align: center">
                                                    <%=GetLabel("Diskon Lain-Lain")%>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="padding-left: 3px; text-align: center">
                                                    <%#: Eval("DiscountGLAccountNo3")%></div>
                                                <div style="padding-left: 3px; text-align: center">
                                                    <%#: Eval("DiscountGLAccountName3")%></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="180px">
                                            <HeaderTemplate>
                                                <div style="padding-left: 3px; text-align: center">
                                                    <%=GetLabel("Created Information")%>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="padding-left: 3px; text-align: center">
                                                    <%#: Eval("CreatedByName")%></div>
                                                <div style="padding-left: 3px; text-align: center">
                                                    <%#: Eval("cfCreatedDateInString")%></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="180px">
                                            <HeaderTemplate>
                                                <div style="padding-left: 3px; text-align: center">
                                                    <%=GetLabel("Last Updated Information")%>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="padding-left: 3px; text-align: center">
                                                    <%#: Eval("LastUpdatedByName")%></div>
                                                <div style="padding-left: 3px; text-align: center">
                                                    <%#: Eval("cfLastUpdatedDateInString")%></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="imgLoadingGrdView" id="Div1">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div style="width: 100%; text-align: center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblEntryPopupAddData">
                        <%= GetLabel("Tambah Data")%></span>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Close")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
