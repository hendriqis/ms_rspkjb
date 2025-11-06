<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GLMappingItemMasterClassEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.GLMappingItemMasterClassEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_GLMappingItemMasterClassEntryCtl">

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

    //#region Revenue Comp1
    function onGetRevenueGLAccountID1FilterExpression() {
        var filterExpression = "IsHeader = 0 AND IsDeleted = 0 AND IsActive = 1";
        return filterExpression;
    }

    $('#lblRevenueGLAccountID1.lblLink').live('click', function () {
        openSearchDialog('chartofaccount', onGetRevenueGLAccountID1FilterExpression(), function (value) {
            $('#<%=txtRevenueGLAccountNo1.ClientID %>').val(value);
            ontxtRevenueGLAccountNo1Changed(value);
        });
    });

    $('#<%=txtRevenueGLAccountNo1.ClientID %>').live('change', function () {
        ontxtRevenueGLAccountNo1Changed($(this).val());
    });

    function ontxtRevenueGLAccountNo1Changed(value) {
        var filterExpression = onGetRevenueGLAccountID1FilterExpression() + " AND GLAccountNo = '" + value + "'";
        Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnRevenueGLAccountID1.ClientID %>').val(result.GLAccountID);
                $('#<%=txtRevenueGLAccountName1.ClientID %>').val(result.GLAccountName);
            }
            else {
                $('#<%=hdnRevenueGLAccountID1.ClientID %>').val('');
                $('#<%=txtRevenueGLAccountNo1.ClientID %>').val('');
                $('#<%=txtRevenueGLAccountName1.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Revenue Comp2
    function onGetRevenueGLAccountID2FilterExpression() {
        var filterExpression = "IsHeader = 0 AND IsDeleted = 0 AND IsActive = 1";
        return filterExpression;
    }

    $('#lblRevenueGLAccountID2.lblLink').live('click', function () {
        openSearchDialog('chartofaccount', onGetRevenueGLAccountID2FilterExpression(), function (value) {
            $('#<%=txtRevenueGLAccountNo2.ClientID %>').val(value);
            ontxtRevenueGLAccountNo2Changed(value);
        });
    });

    $('#<%=txtRevenueGLAccountNo2.ClientID %>').live('change', function () {
        ontxtRevenueGLAccountNo2Changed($(this).val());
    });

    function ontxtRevenueGLAccountNo2Changed(value) {
        var filterExpression = onGetRevenueGLAccountID2FilterExpression() + " AND GLAccountNo = '" + value + "'";
        Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnRevenueGLAccountID2.ClientID %>').val(result.GLAccountID);
                $('#<%=txtRevenueGLAccountName2.ClientID %>').val(result.GLAccountName);
            }
            else {
                $('#<%=hdnRevenueGLAccountID2.ClientID %>').val('');
                $('#<%=txtRevenueGLAccountNo2.ClientID %>').val('');
                $('#<%=txtRevenueGLAccountName2.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Revenue Comp3
    function onGetRevenueGLAccountID3FilterExpression() {
        var filterExpression = "IsHeader = 0 AND IsDeleted = 0 AND IsActive = 1";
        return filterExpression;
    }

    $('#lblRevenueGLAccountID3.lblLink').live('click', function () {
        openSearchDialog('chartofaccount', onGetRevenueGLAccountID3FilterExpression(), function (value) {
            $('#<%=txtRevenueGLAccountNo3.ClientID %>').val(value);
            ontxtRevenueGLAccountNo3Changed(value);
        });
    });

    $('#<%=txtRevenueGLAccountNo3.ClientID %>').live('change', function () {
        ontxtRevenueGLAccountNo3Changed($(this).val());
    });

    function ontxtRevenueGLAccountNo3Changed(value) {
        var filterExpression = onGetRevenueGLAccountID3FilterExpression() + " AND GLAccountNo = '" + value + "'";
        Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnRevenueGLAccountID3.ClientID %>').val(result.GLAccountID);
                $('#<%=txtRevenueGLAccountName3.ClientID %>').val(result.GLAccountName);
            }
            else {
                $('#<%=hdnRevenueGLAccountID3.ClientID %>').val('');
                $('#<%=txtRevenueGLAccountNo3.ClientID %>').val('');
                $('#<%=txtRevenueGLAccountName3.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Discount Comp1
    function onGetDiscountGLAccountID1FilterExpression() {
        var filterExpression = "IsHeader = 0 AND IsDeleted = 0 AND IsActive = 1";
        return filterExpression;
    }

    $('#lblDiscountGLAccountID1.lblLink').live('click', function () {
        openSearchDialog('chartofaccount', onGetDiscountGLAccountID1FilterExpression(), function (value) {
            $('#<%=txtDiscountGLAccountNo1.ClientID %>').val(value);
            ontxtDiscountGLAccountNo1Changed(value);
        });
    });

    $('#<%=txtDiscountGLAccountNo1.ClientID %>').live('change', function () {
        ontxtDiscountGLAccountNo1Changed($(this).val());
    });

    function ontxtDiscountGLAccountNo1Changed(value) {
        var filterExpression = onGetDiscountGLAccountID1FilterExpression() + " AND GLAccountNo = '" + value + "'";
        Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnDiscountGLAccountID1.ClientID %>').val(result.GLAccountID);
                $('#<%=txtDiscountGLAccountName1.ClientID %>').val(result.GLAccountName);
            }
            else {
                $('#<%=hdnDiscountGLAccountID1.ClientID %>').val('');
                $('#<%=txtDiscountGLAccountNo1.ClientID %>').val('');
                $('#<%=txtDiscountGLAccountName1.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Discount Comp2
    function onGetDiscountGLAccountID2FilterExpression() {
        var filterExpression = "IsHeader = 0 AND IsDeleted = 0 AND IsActive = 1";
        return filterExpression;
    }

    $('#lblDiscountGLAccountID2.lblLink').live('click', function () {
        openSearchDialog('chartofaccount', onGetDiscountGLAccountID2FilterExpression(), function (value) {
            $('#<%=txtDiscountGLAccountNo2.ClientID %>').val(value);
            ontxtDiscountGLAccountNo2Changed(value);
        });
    });

    $('#<%=txtDiscountGLAccountNo2.ClientID %>').live('change', function () {
        ontxtDiscountGLAccountNo2Changed($(this).val());
    });

    function ontxtDiscountGLAccountNo2Changed(value) {
        var filterExpression = onGetDiscountGLAccountID2FilterExpression() + " AND GLAccountNo = '" + value + "'";
        Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnDiscountGLAccountID2.ClientID %>').val(result.GLAccountID);
                $('#<%=txtDiscountGLAccountName2.ClientID %>').val(result.GLAccountName);
            }
            else {
                $('#<%=hdnDiscountGLAccountID2.ClientID %>').val('');
                $('#<%=txtDiscountGLAccountNo2.ClientID %>').val('');
                $('#<%=txtDiscountGLAccountName2.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Discount Comp3
    function onGetDiscountGLAccountID3FilterExpression() {
        var filterExpression = "IsHeader = 0 AND IsDeleted = 0 AND IsActive = 1";
        return filterExpression;
    }

    $('#lblDiscountGLAccountID3.lblLink').live('click', function () {
        openSearchDialog('chartofaccount', onGetDiscountGLAccountID3FilterExpression(), function (value) {
            $('#<%=txtDiscountGLAccountNo3.ClientID %>').val(value);
            ontxtDiscountGLAccountNo3Changed(value);
        });
    });

    $('#<%=txtDiscountGLAccountNo3.ClientID %>').live('change', function () {
        ontxtDiscountGLAccountNo3Changed($(this).val());
    });

    function ontxtDiscountGLAccountNo3Changed(value) {
        var filterExpression = onGetDiscountGLAccountID3FilterExpression() + " AND GLAccountNo = '" + value + "'";
        Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnDiscountGLAccountID3.ClientID %>').val(result.GLAccountID);
                $('#<%=txtDiscountGLAccountName3.ClientID %>').val(result.GLAccountName);
            }
            else {
                $('#<%=hdnDiscountGLAccountID3.ClientID %>').val('');
                $('#<%=txtDiscountGLAccountNo3.ClientID %>').val('');
                $('#<%=txtDiscountGLAccountName3.ClientID %>').val('');
            }
        });
    }
    //#endregion

    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=hdnClassID.ClientID %>').val('');
        $('#<%=txtClassCode.ClientID %>').val('');
        $('#<%=txtClassName.ClientID %>').val('');

        $('#<%=hdnRevenueGLAccountID1.ClientID %>').val("");
        $('#<%=txtRevenueGLAccountNo1.ClientID %>').val("");
        $('#<%=txtRevenueGLAccountName1.ClientID %>').val("");

        $('#<%=hdnRevenueGLAccountID2.ClientID %>').val("");
        $('#<%=txtRevenueGLAccountNo2.ClientID %>').val("");
        $('#<%=txtRevenueGLAccountName2.ClientID %>').val("");

        $('#<%=hdnRevenueGLAccountID3.ClientID %>').val("");
        $('#<%=txtRevenueGLAccountNo3.ClientID %>').val("");
        $('#<%=txtRevenueGLAccountName3.ClientID %>').val("");

        $('#<%=hdnDiscountGLAccountID1.ClientID %>').val("");
        $('#<%=txtDiscountGLAccountNo1.ClientID %>').val("");
        $('#<%=txtDiscountGLAccountName1.ClientID %>').val("");

        $('#<%=hdnDiscountGLAccountID2.ClientID %>').val("");
        $('#<%=txtDiscountGLAccountNo2.ClientID %>').val("");
        $('#<%=txtDiscountGLAccountName2.ClientID %>').val("");

        $('#<%=hdnDiscountGLAccountID3.ClientID %>').val("");
        $('#<%=txtDiscountGLAccountNo3.ClientID %>').val("");
        $('#<%=txtDiscountGLAccountName3.ClientID %>').val("");

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
            var itemID = $('#<%=hdnItemIDCtlCOA.ClientID %>').val();
            var classID = $('#<%=hdnClassID.ClientID %>').val();

            if (itemID != 0 && itemID != "" && itemID != null) {
                if (classID != 0 && classID != "" && classID != null) {
                    cbpEntryPopupView.PerformCallback('save');
                } else {
                    displayErrorMessageBox('INFORMASI', "Harap isi isian Kelas terlebih dahulu.");
                }
            } else {
                displayErrorMessageBox('INFORMASI', "Harap isi isian Item terlebih dahulu.");
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
    }

</script>
<div style="height: 450px; overflow-y: auto">
    <input type="hidden" id="hdnItemIDCtlCOA" value="" runat="server" />
    <input type="hidden" id="hdnIDCtl" value="" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 1px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Kode Item")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtItemCodeCtl" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nama Item")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtItemNameCtl" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
                <div id="containerPopupEntryData" style="margin-top: 10px; display: none;">
                    <div class="pageTitle">
                        <%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 180px" />
                                <col />
                            </colgroup>
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
                                <td colspan="3">
                                    <hr style="margin: 0 0 0 0;" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal" style="font-size: small; font-weight: bold">
                                        <%=GetLabel("COA Revenue")%></label>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink lblNormal" id="lblRevenueGLAccountID1" style="font-style: italic;
                                        font-size: small; padding-left: 15px">
                                        <%=GetLabel("Comp1")%></label>
                                </td>
                                <td colspan="2">
                                    <input type="hidden" id="hdnRevenueGLAccountID1" runat="server" />
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 120px" />
                                            <col style="width: 3px" />
                                            <col style="width: 250px" />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtRevenueGLAccountNo1" Width="100%" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtRevenueGLAccountName1" ReadOnly="true" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink lblNormal" id="lblRevenueGLAccountID2" style="font-style: italic;
                                        font-size: small; padding-left: 15px">
                                        <%=GetLabel("Comp2")%></label>
                                </td>
                                <td colspan="2">
                                    <input type="hidden" id="hdnRevenueGLAccountID2" runat="server" />
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 120px" />
                                            <col style="width: 3px" />
                                            <col style="width: 250px" />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtRevenueGLAccountNo2" Width="100%" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtRevenueGLAccountName2" ReadOnly="true" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink lblNormal" id="lblRevenueGLAccountID3" style="font-style: italic;
                                        font-size: small; padding-left: 15px">
                                        <%=GetLabel("Comp3")%></label>
                                </td>
                                <td colspan="2">
                                    <input type="hidden" id="hdnRevenueGLAccountID3" runat="server" />
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 120px" />
                                            <col style="width: 3px" />
                                            <col style="width: 250px" />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtRevenueGLAccountNo3" Width="100%" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtRevenueGLAccountName3" ReadOnly="true" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <hr style="margin: 0 0 0 0;" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal" style="font-size: small; font-weight: bold">
                                        <%=GetLabel("COA Discount")%></label>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink lblNormal" id="lblDiscountGLAccountID1" style="font-style: italic;
                                        font-size: small; padding-left: 15px">
                                        <%=GetLabel("Comp1")%></label>
                                </td>
                                <td colspan="2">
                                    <input type="hidden" id="hdnDiscountGLAccountID1" runat="server" />
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 120px" />
                                            <col style="width: 3px" />
                                            <col style="width: 250px" />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtDiscountGLAccountNo1" Width="100%" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtDiscountGLAccountName1" ReadOnly="true" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink lblNormal" id="lblDiscountGLAccountID2" style="font-style: italic;
                                        font-size: small; padding-left: 15px">
                                        <%=GetLabel("Comp2")%></label>
                                </td>
                                <td colspan="2">
                                    <input type="hidden" id="hdnDiscountGLAccountID2" runat="server" />
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 120px" />
                                            <col style="width: 3px" />
                                            <col style="width: 250px" />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtDiscountGLAccountNo2" Width="100%" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtDiscountGLAccountName2" ReadOnly="true" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink lblNormal" id="lblDiscountGLAccountID3" style="font-style: italic;
                                        font-size: small; padding-left: 15px">
                                        <%=GetLabel("Comp3")%></label>
                                </td>
                                <td colspan="2">
                                    <input type="hidden" id="hdnDiscountGLAccountID3" runat="server" />
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 120px" />
                                            <col style="width: 3px" />
                                            <col style="width: 250px" />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtDiscountGLAccountNo3" Width="100%" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtDiscountGLAccountName3" ReadOnly="true" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <hr style="margin: 0 0 0 0;" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
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
                                        <asp:BoundField DataField="ClassName" ItemStyle-CssClass="ClassName" HeaderText="Class Name"
                                            HeaderStyle-Width="150px" />
                                        <asp:TemplateField HeaderStyle-Width="180px">
                                            <HeaderTemplate>
                                                <div style="text-align: center">
                                                    <%=GetLabel("COA Revenue")%>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <label class="lblNormal" style="font-style: italic; font-size: small;">
                                                    <%=GetLabel("Comp1 : ")%></label><%#: Eval("RevenueGLAccountNo1")%><br />
                                                <label class="lblNormal" style="font-style: italic; font-size: small;">
                                                    <%=GetLabel("Comp2 : ")%></label><%#: Eval("RevenueGLAccountNo2")%><br />
                                                <label class="lblNormal" style="font-style: italic; font-size: small;">
                                                    <%=GetLabel("Comp3 : ")%></label><%#: Eval("RevenueGLAccountNo3")%><br />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="180px">
                                            <HeaderTemplate>
                                                <div style="text-align: center">
                                                    <%=GetLabel("COA Discount")%>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <label class="lblNormal" style="font-style: italic; font-size: small;">
                                                    <%=GetLabel("Comp1 : ")%></label><%#: Eval("DiscountGLAccountNo1")%><br />
                                                <label class="lblNormal" style="font-style: italic; font-size: small;">
                                                    <%=GetLabel("Comp2 : ")%></label><%#: Eval("DiscountGLAccountNo2")%><br />
                                                <label class="lblNormal" style="font-style: italic; font-size: small;">
                                                    <%=GetLabel("Comp3 : ")%></label><%#: Eval("DiscountGLAccountNo3")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="180px">
                                            <HeaderTemplate>
                                                <div style="text-align: center">
                                                    <%=GetLabel("Created Info")%>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="padding-left: 3px; text-align: center">
                                                    <%#: Eval("CreatedByName")%></div>
                                                <div style="padding-left: 3px; text-align: center">
                                                    <%#: Eval("cfCreatedDateInString")%></div>
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
