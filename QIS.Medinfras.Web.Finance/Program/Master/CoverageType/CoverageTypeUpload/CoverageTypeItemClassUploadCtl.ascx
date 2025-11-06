<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CoverageTypeItemClassUploadCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.Finance.Program.CoverageTypeItemClassEntryCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=hdnID.ClientID %>').val('');
        $('#<%=hdnClassID.ClientID %>').val('');
        $('#<%=txtClassCode.ClientID %>').val('');
        $('#<%=txtClassName.ClientID %>').val('');

        $('#<%=chkIsMarkupInPercentage.ClientID %>').prop('checked', false);
        $('#<%=txtMarkupAmount.ClientID %>').val('0').trigger('changeValue');
        $('#<%=chkIsDiscountInPercentage.ClientID %>').prop('checked', false);
        $('#<%=txtDiscountAmount.ClientID %>').val('0').trigger('changeValue');
        $('#<%=chkIsDiscountInPercentageComp1.ClientID %>').prop('checked', false);
        $('#<%=txtDiscountAmountComp1.ClientID %>').val('0').trigger('changeValue');
        $('#<%=chkIsDiscountInPercentageComp2.ClientID %>').prop('checked', false);
        $('#<%=txtDiscountAmountComp2.ClientID %>').val('0').trigger('changeValue');
        $('#<%=chkIsDiscountInPercentageComp3.ClientID %>').prop('checked', false);
        $('#<%=txtDiscountAmountComp3.ClientID %>').val('0').trigger('changeValue');
        $('#<%=chkIsCoverageInPercentage.ClientID %>').prop('checked', false);
        $('#<%=txtCoverageAmount.ClientID %>').val('0').trigger('changeValue');
        $('#<%=chkIsCashBackInPercentage.ClientID %>').prop('checked', false);
        $('#<%=txtCashBackAmount.ClientID %>').val('0').trigger('changeValue');
        
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
        $row = $(this).closest('tr').parent().closest('tr');
        var ID = $row.find('.hdnID').val();
        showToastConfirmation('Are You Sure Want To Delete?', function (result) {
            if (result) {
                $('#<%=hdnID.ClientID %>').val(ID);
                cbpEntryPopupView.PerformCallback('delete');
            }
        });
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        var ID = $row.find('.hdnID').val();
        var classID = $row.find('.hdnClassID').val();
        var classCode = $row.find('.hdnClassCode').val();
        var className = $row.find('.tdClassName').html();

        var markupAmount = $row.find('.tdMarkupAmount').html();
        var discountAmount = $row.find('.tdDiscountAmount').html();
        var discountAmountComp1 = $row.find('.DiscountAmountComp1').val();
        var discountAmountComp2 = $row.find('.DiscountAmountComp2').val();
        var discountAmountComp3 = $row.find('.DiscountAmountComp3').val();
        var coverageAmount = $row.find('.tdCoverageAmount').html();
        var cashBackAmount = $row.find('.tdCashBackAmount').html();

        $('#<%=hdnID.ClientID %>').val(ID);
        $('#<%=hdnClassID.ClientID %>').val(classID);
        $('#<%=txtClassCode.ClientID %>').val(classCode);
        $('#<%=txtClassName.ClientID %>').val(className);

        $('#<%=chkIsMarkupInPercentage.ClientID %>').prop('checked', markupAmount.indexOf("%") > -1);
        $('#<%=txtMarkupAmount.ClientID %>').val(markupAmount.replace('%', '').replace('-', '0').replace(/,/g, '')).trigger('changeValue');
        $('#<%=chkIsDiscountInPercentage.ClientID %>').prop('checked', discountAmount.indexOf("%") > -1);
        $('#<%=txtDiscountAmount.ClientID %>').val(discountAmount.replace('%', '').replace('-', '0').replace(/,/g, '')).trigger('changeValue');
        $('#<%=chkIsDiscountInPercentageComp1.ClientID %>').prop('checked', discountAmountComp1.indexOf("%") > -1);
        $('#<%=txtDiscountAmountComp1.ClientID %>').val(discountAmountComp1.replace('%', '').replace('-', '0').replace(/,/g, '')).trigger('changeValue');
        $('#<%=chkIsDiscountInPercentageComp2.ClientID %>').prop('checked', discountAmountComp2.indexOf("%") > -1);
        $('#<%=txtDiscountAmountComp2.ClientID %>').val(discountAmountComp2.replace('%', '').replace('-', '0').replace(/,/g, '')).trigger('changeValue');
        $('#<%=chkIsDiscountInPercentageComp3.ClientID %>').prop('checked', discountAmountComp3.indexOf("%") > -1);
        $('#<%=txtDiscountAmountComp3.ClientID %>').val(discountAmountComp3.replace('%', '').replace('-', '0').replace(/,/g, '')).trigger('changeValue');
        $('#<%=chkIsCoverageInPercentage.ClientID %>').prop('checked', coverageAmount.indexOf("%") > -1);
        $('#<%=txtCoverageAmount.ClientID %>').val(coverageAmount.replace('%', '').replace('-', '0').replace(/,/g, '')).trigger('changeValue');
        $('#<%=chkIsCashBackInPercentage.ClientID %>').prop('checked', cashBackAmount.indexOf("%") > -1);
        $('#<%=txtCashBackAmount.ClientID %>').val(cashBackAmount.replace('%', '').replace('-', '0').replace(/,/g, '')).trigger('changeValue');

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
        hideLoadingPanel();
    }

    //#region Class
    function onGetCoverageTypeItemClassCodeFilterExpression() {
        var filterExpression = "IsUsedInChargeClass = 1 AND ClassID NOT IN (SELECT ClassID FROM CoverageTypeItemClass WHERE CoverageTypeID = " + $('#<%=hdnCoverageTypeID.ClientID %>').val() + " AND ItemID = '" + $('#<%=hdnItemID.ClientID %>').val() + "' AND IsDeleted = 0) AND IsDeleted = 0";
        return filterExpression;
    }

    $('#lblClass.lblLink').click(function () {
        openSearchDialog('classcare', onGetCoverageTypeItemClassCodeFilterExpression(), function (value) {
            $('#<%=txtClassCode.ClientID %>').val(value);
            onTxtCoverageTypeDepartmentClassCodeChanged(value);
        });
    });

    $('#<%=txtClassCode.ClientID %>').change(function () {
        onTxtCoverageTypeDepartmentClassCodeChanged($(this).val());
    });

    function onTxtCoverageTypeDepartmentClassCodeChanged(value) {
        var filterExpression = onGetCoverageTypeItemClassCodeFilterExpression() + " AND ClassCode = '" + value + "'";
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
</script>

<div style="height:450px; overflow-y:auto">
    <input type="hidden" id="hdnCoverageTypeID" value="" runat="server" />
    <input type="hidden" id="hdnItemID" value="" runat="server" />
    <div class="pageTitle"><%=GetLabel("Tipe Jaminan Kelas Item")%></div>
    <table class="tblContentArea">
        <colgroup>
            <col style="width:100%"/>
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
                                <%=GetLabel("Tipe Jaminan")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtCoverageType" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Item")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtItem" ReadOnly="true" Width="100%" runat="server" />
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
                                <col style="width: 150px" />
                                <col style="width: 600px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory lblLink" id="lblClass">
                                        <%=GetLabel("Kelas")%></label>
                                </td>
                                <td>
                                    <input type="hidden" runat="server" id="hdnClassID" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 100px" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtClassCode" CssClass="required" Width="100%" runat="server" />
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
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30px" />
                                            <col style="width: 3px" />
                                            <col style="width: 170px" />
                                        </colgroup>
                                        <tr>
                                            <td align="center">
                                                <div class="lblComponent">
                                                    <%=GetLabel("%")%></div>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                <div class="lblComponent">
                                                    <%=GetLabel("Jumlah")%></div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Jumlah")%></label>
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30px" />
                                            <col style="width: 3px" />
                                            <col style="width: 170px" />
                                        </colgroup>
                                        <tr>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsMarkupInPercentage" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtMarkupAmount" CssClass="txtCurrency" runat="server" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <hr style="margin: 0 0 0 0;" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Diskon")%></label>
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30px" />
                                            <col style="width: 3px" />
                                            <col style="width: 170px" />
                                        </colgroup>
                                        <tr>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsDiscountInPercentage" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtDiscountAmount" CssClass="txtCurrency" runat="server" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal" style="font-style: italic; font-size: small; padding-left: 15px">
                                        <%=GetLabel("Diskon Komponen 1")%></label>
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30px" />
                                            <col style="width: 3px" />
                                            <col style="width: 170px" />
                                        </colgroup>
                                        <tr>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsDiscountInPercentageComp1" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtDiscountAmountComp1" CssClass="txtCurrency" runat="server" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal" style="font-style: italic; font-size: small; padding-left: 15px">
                                        <%=GetLabel("Diskon Komponen 2")%></label>
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30px" />
                                            <col style="width: 3px" />
                                            <col style="width: 170px" />
                                        </colgroup>
                                        <tr>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsDiscountInPercentageComp2" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtDiscountAmountComp2" CssClass="txtCurrency" runat="server" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal" style="font-style: italic; font-size: small; padding-left: 15px">
                                        <%=GetLabel("Diskon Komponen 3")%></label>
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30px" />
                                            <col style="width: 3px" />
                                            <col style="width: 170px" />
                                        </colgroup>
                                        <tr>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsDiscountInPercentageComp3" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtDiscountAmountComp3" CssClass="txtCurrency" runat="server" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <hr style="margin: 0 0 0 0;" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Jaminan")%></label>
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30px" />
                                            <col style="width: 3px" />
                                            <col style="width: 170px" />
                                        </colgroup>
                                        <tr>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsCoverageInPercentage" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtCoverageAmount" CssClass="txtCurrency" runat="server" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <hr style="margin: 0 0 0 0;" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Cashback")%></label>
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30px" />
                                            <col style="width: 3px" />
                                            <col style="width: 170px" />
                                        </colgroup>
                                        <tr>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsCashBackInPercentage" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtCashBackAmount" CssClass="txtCurrency" runat="server" Width="100%" />
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

                <div style="position:relative;height:297px;overflow-y:auto; overflow-x: hidden;">
                    <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                        ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                            EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                                    <asp:ListView runat="server" ID="lvwView">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect grdPopup" cellspacing="0" rules="all" >
                                                <tr>  
                                                    <th style="width:40px">&nbsp;</th>
                                                    <th style="width:350px" align="left"><%=GetLabel("Item")%></th>
                                                    <th style="width:120px" align="right"><%=GetLabel("Kenaikan Harga")%></th>
                                                    <th style="width:120px" align="right"><%=GetLabel("Diskon")%></th>
                                                    <th style="width:120px" align="right"><%=GetLabel("Jaminan")%></th>
                                                    <th style="width:120px" align="right"><%=GetLabel("CashBack")%></th>
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="7">
                                                        <%=GetLabel("Data Tidak Tersedia")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0" rules="all" >
                                                <tr>  
                                                    <th style="width:40px">&nbsp;</th>
                                                    <th style="width:350px" align="left"><%=GetLabel("Item")%></th>
                                                    <th style="width:120px" align="right"><%=GetLabel("Kenaikan Harga")%></th>
                                                    <th style="width:120px" align="right"><%=GetLabel("Diskon")%></th>
                                                    <th style="width:120px" align="right"><%=GetLabel("Jaminan")%></th>
                                                    <th style="width:120px" align="right"><%=GetLabel("Cashback")%></th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder" ></tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td><img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt=""  /></td>
                                                            <td style="width:1px">&nbsp;</td>
                                                            <td><img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt=""  /></td>
                                                        </tr>
                                                    </table>

                                                    <input type="hidden" class="hdnID" value="<%#: Eval("ID")%>" />
                                                    <input type="hidden" class="hdnClassID" value="<%#: Eval("ClassID")%>" />
                                                    <input type="hidden" class="hdnClassCode" value="<%#: Eval("ClassCode")%>" />
                                                    <input type="hidden" class="DiscountAmountComp1" value="<%#: Eval("DisplayDiscountAmountComp1")%>" />
                                                    <input type="hidden" class="DiscountAmountComp2" value="<%#: Eval("DisplayDiscountAmountComp2")%>" />
                                                    <input type="hidden" class="DiscountAmountComp3" value="<%#: Eval("DisplayDiscountAmountComp3")%>" />
                                                </td>
                                                <td class="tdIClassName"><%#: Eval("ClassName")%></td>
                                                <td class="tdMarkupAmount" align="right"><%#: Eval("DisplayMarkupAmount")%></td>
                                                <td class="tdDiscountAmount" align="right"><%#: Eval("DisplayDiscountAmount")%></td>
                                                <td class="tdCoverageAmount" align="right"><%#: Eval("DisplayCoverageAmount")%></td>
                                                <td class="tdCashBackAmount" align="right"><%#: Eval("DisplayCashBackAmount")%></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                </div>
                <div style="width:100%;text-align:center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblEntryPopupAddData"><%= GetLabel("Tambah Data")%></span>
                </div>
            </td>
        </tr>
    </table>
    <div style="width:100%;text-align:right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>

