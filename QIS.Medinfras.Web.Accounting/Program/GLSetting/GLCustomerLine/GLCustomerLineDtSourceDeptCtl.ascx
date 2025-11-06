<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GLCustomerLineDtSourceDeptCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Accounting.Program.GLCustomerLineDtSourceDeptCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<script type="text/javascript" id="dxss_GLCustomerLineDtSourceDeptCtl">

    function onGetGLAccountFilterExpression() {
        var filterExpression = "IsHeader = 0 AND IsDeleted = 0";
        return filterExpression;
    }

    //#region SourceDepartment
    function getSourceDepartmentFilterExpression() {
        var filterExpression = "IsHasRegistration = 1 AND IsActive = 1";
        return filterExpression;
    }

    $('#lblSourceDepartment.lblLink').live('click', function () {
        openSearchDialog('department', getSourceDepartmentFilterExpression(), function (value) {
            $('#<%=txtSourceDepartmentID.ClientID %>').val(value);
            ontxtSourceDepartmentIDChanged(value);
        });
    });

    $('#<%=txtSourceDepartmentID.ClientID %>').die('change');
    $('#<%=txtSourceDepartmentID.ClientID %>').live('change', function () {
        ontxtSourceDepartmentIDChanged($(this).val());
    });

    function ontxtSourceDepartmentIDChanged(value) {
        var filterExpression = getSourceDepartmentFilterExpression() + " AND DepartmentID = '" + value + "'";
        Methods.getObject('GetDepartmentList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnSourceDepartmentID.ClientID %>').val(result.DepartmentID);
                $('#<%=txtSourceDepartmentID.ClientID %>').val(result.DepartmentID);
                $('#<%=txtSourceDepartmentName.ClientID %>').val(result.DepartmentName);
            }
            else {
                $('#<%=hdnSourceDepartmentID.ClientID %>').val('');
                $('#<%=txtSourceDepartmentID.ClientID %>').val('');
                $('#<%=txtSourceDepartmentName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region AR
    $('#lblAR.lblLink').click(function () {
        openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
            $('#<%=txtARGLAccountNo.ClientID %>').val(value);
            onTxtARGLAccountCodeChanged(value);
        });
    });

    $('#<%=txtARGLAccountNo.ClientID %>').change(function () {
        onTxtARGLAccountCodeChanged($(this).val());
    });

    function onTxtARGLAccountCodeChanged(value) {
        var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
        Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnARID.ClientID %>').val(result.GLAccountID);
                $('#<%=txtARGLAccountName.ClientID %>').val(result.GLAccountName);
            }
            else {
                $('#<%=hdnARID.ClientID %>').val('');
                $('#<%=txtARGLAccountNo.ClientID %>').val('');
                $('#<%=txtARGLAccountName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region ARInProcess
    $('#lblARInProcess.lblLink').click(function () {
        openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
            $('#<%=txtARInProcessGLAccountNo.ClientID %>').val(value);
            onTxtARInProcessGLAccountCodeChanged(value);
        });
    });

    $('#<%=txtARInProcessGLAccountNo.ClientID %>').change(function () {
        onTxtARInProcessGLAccountCodeChanged($(this).val());
    });

    function onTxtARInProcessGLAccountCodeChanged(value) {
        var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
        Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnARInProcessID.ClientID %>').val(result.GLAccountID);
                $('#<%=txtARInProcessGLAccountName.ClientID %>').val(result.GLAccountName);
            }
            else {
                $('#<%=hdnARInProcessID.ClientID %>').val('');
                $('#<%=txtARInProcessGLAccountNo.ClientID %>').val('');
                $('#<%=txtARInProcessGLAccountName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region ARInCare
    $('#lblARInCare.lblLink').click(function () {
        openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
            $('#<%=txtARInCareGLAccountNo.ClientID %>').val(value);
            onTxtARInCareGLAccountCodeChanged(value);
        });
    });

    $('#<%=txtARInCareGLAccountNo.ClientID %>').change(function () {
        onTxtARInCareGLAccountCodeChanged($(this).val());
    });

    function onTxtARInCareGLAccountCodeChanged(value) {
        var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
        Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnARInCareID.ClientID %>').val(result.GLAccountID);
                $('#<%=txtARInCareGLAccountName.ClientID %>').val(result.GLAccountName);
            }
            else {
                $('#<%=hdnARInCareID.ClientID %>').val('');
                $('#<%=txtARInCareGLAccountNo.ClientID %>').val('');
                $('#<%=txtARInCareGLAccountName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region ARAdjustment
    $('#lblARAdjustment.lblLink').click(function () {
        openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
            $('#<%=txtARAdjustmentGLAccountNo.ClientID %>').val(value);
            onTxtARAdjustmentGLAccountCodeChanged(value);
        });
    });

    $('#<%=txtARAdjustmentGLAccountNo.ClientID %>').change(function () {
        onTxtARAdjustmentGLAccountCodeChanged($(this).val());
    });

    function onTxtARAdjustmentGLAccountCodeChanged(value) {
        var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
        Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnARAdjustmentID.ClientID %>').val(result.GLAccountID);
                $('#<%=txtARAdjustmentGLAccountName.ClientID %>').val(result.GLAccountName);
            }
            else {
                $('#<%=hdnARAdjustmentID.ClientID %>').val('');
                $('#<%=txtARAdjustmentGLAccountNo.ClientID %>').val('');
                $('#<%=txtARAdjustmentGLAccountName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region ARDiscount
    $('#lblARDiscount.lblLink').click(function () {
        openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
            $('#<%=txtARDiscountGLAccountNo.ClientID %>').val(value);
            onTxtARDiscountGLAccountCodeChanged(value);
        });
    });

    $('#<%=txtARDiscountGLAccountNo.ClientID %>').change(function () {
        onTxtARDiscountGLAccountCodeChanged($(this).val());
    });

    function onTxtARDiscountGLAccountCodeChanged(value) {
        var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
        Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnARDiscountID.ClientID %>').val(result.GLAccountID);
                $('#<%=txtARDiscountGLAccountName.ClientID %>').val(result.GLAccountName);
            }
            else {
                $('#<%=hdnARDiscountID.ClientID %>').val('');
                $('#<%=txtARDiscountGLAccountNo.ClientID %>').val('');
                $('#<%=txtARDiscountGLAccountName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=hdnID.ClientID %>').val('');
        $('#<%=hdnSourceDepartmentID.ClientID %>').val('');
        $('#<%=txtSourceDepartmentID.ClientID %>').val('');
        $('#<%=txtSourceDepartmentName.ClientID %>').val('');
        $('#<%=hdnARID.ClientID %>').val('');
        $('#<%=txtARGLAccountNo.ClientID %>').val('');
        $('#<%=txtARGLAccountName.ClientID %>').val('');
        $('#<%=hdnARInProcessID.ClientID %>').val('');
        $('#<%=txtARInProcessGLAccountNo.ClientID %>').val('');
        $('#<%=txtARInProcessGLAccountName.ClientID %>').val('');
        $('#<%=hdnARInCareID.ClientID %>').val('');
        $('#<%=txtARInCareGLAccountNo.ClientID %>').val('');
        $('#<%=txtARInCareGLAccountName.ClientID %>').val('');
        $('#<%=hdnARAdjustmentID.ClientID %>').val('');
        $('#<%=txtARAdjustmentGLAccountNo.ClientID %>').val('');
        $('#<%=txtARAdjustmentGLAccountName.ClientID %>').val('');
        $('#<%=hdnARDiscountID.ClientID %>').val('');
        $('#<%=txtARDiscountGLAccountNo.ClientID %>').val('');
        $('#<%=txtARDiscountGLAccountName.ClientID %>').val('');

        $('#containerPopupEntryData').show();
    });

    $('#btnEntryPopupCancel').live('click', function () {
        $('#containerPopupEntryData').hide();
    });

    $('#btnEntryPopupSave').live('click', function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup')) {
            cbpEntryPopupView.PerformCallback('save');
        }
        else {
            return false;
        }

    });

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function (evt) {
        evt.stopPropagation();
        evt.preventDefault();
        if (confirm("Are You Sure Want To Delete This Data?")) {
            $row = $(this).closest('tr');
            var id = $row.find('.ID').val();
            $('#<%=hdnID.ClientID %>').val(id);

            cbpEntryPopupView.PerformCallback('delete');
        }
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');

        var ID = $row.find('.ID').val();
        var SourceDepartmentID = $row.find('.SourceDepartmentID').val();
        var SourceDepartmentName = $row.find('.SourceDepartmentName').val();
        var AR = $row.find('.AR').val();
        var ARGLAccountNo = $row.find('.ARGLAccountNo').val();
        var ARGLAccountName = $row.find('.ARGLAccountName').val();
        var ARInProcess = $row.find('.ARInProcess').val();
        var ARInProcessGLAccountNo = $row.find('.ARInProcessGLAccountNo').val();
        var ARInProcessGLAccountName = $row.find('.ARInProcessGLAccountName').val();
        var ARInCare = $row.find('.ARInCare').val();
        var ARInCareGLAccountNo = $row.find('.ARInCareGLAccountNo').val();
        var ARInCareGLAccountName = $row.find('.ARInCareGLAccountName').val();
        var ARAdjustment = $row.find('.ARAdjustment').val();
        var ARAdjustmentGLAccountNo = $row.find('.ARAdjustmentGLAccountNo').val();
        var ARAdjustmentGLAccountName = $row.find('.ARAdjustmentGLAccountName').val();
        var ARDiscount = $row.find('.ARDiscount').val();
        var ARDiscountGLAccountNo = $row.find('.ARDiscountGLAccountNo').val();
        var ARDiscountGLAccountName = $row.find('.ARDiscountGLAccountName').val();

        $('#<%=hdnID.ClientID %>').val(ID);
        $('#<%=hdnSourceDepartmentID.ClientID %>').val(SourceDepartmentID);
        $('#<%=txtSourceDepartmentID.ClientID %>').val(SourceDepartmentID);
        $('#<%=txtSourceDepartmentName.ClientID %>').val(SourceDepartmentName);
        $('#<%=hdnARID.ClientID %>').val(AR);
        $('#<%=txtARGLAccountNo.ClientID %>').val(ARGLAccountNo);
        $('#<%=txtARGLAccountName.ClientID %>').val(ARGLAccountName);
        $('#<%=hdnARInProcessID.ClientID %>').val(ARInProcess);
        $('#<%=txtARInProcessGLAccountNo.ClientID %>').val(ARInProcessGLAccountNo);
        $('#<%=txtARInProcessGLAccountName.ClientID %>').val(ARInProcessGLAccountName);
        $('#<%=hdnARInCareID.ClientID %>').val(ARInCare);
        $('#<%=txtARInCareGLAccountNo.ClientID %>').val(ARInCareGLAccountNo);
        $('#<%=txtARInCareGLAccountName.ClientID %>').val(ARInCareGLAccountName);
        $('#<%=hdnARAdjustmentID.ClientID %>').val(ARAdjustment);
        $('#<%=txtARAdjustmentGLAccountNo.ClientID %>').val(ARAdjustmentGLAccountNo);
        $('#<%=txtARAdjustmentGLAccountName.ClientID %>').val(ARAdjustmentGLAccountName);
        $('#<%=hdnARDiscountID.ClientID %>').val(ARDiscount);
        $('#<%=txtARDiscountGLAccountNo.ClientID %>').val(ARDiscountGLAccountNo);
        $('#<%=txtARDiscountGLAccountName.ClientID %>').val(ARDiscountGLAccountName);

        $('#containerPopupEntryData').show();
    });

    function setPagingDetailItem(pageCount) {
        if (pageCount > 0)
            $('.grdPopup tr:eq(1)').click();
        setPaging($("#pagingPopup"), pageCount, function (page) {
            cbpEntryPopupView.PerformCallback('changepage|' + page);
        }, 10);
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
                $('#containerPopupEntryData').hide();
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

    function onRefreshControl(filterExpression) {
        cbpEntryPopupView.PerformCallback('refresh');
    }

    //#region Paging
    var pageCountAvailable = parseInt('<%=PageCount %>');
    $(function () {
        setPagingDetailItem(pageCountAvailable);
    });
    //#endregion

</script>
<div style="height: 500px; overflow-y: auto; overflow-x: hidden">
    <input type="hidden" id="hdnCustomerLineIDCtl" value="" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 70%">
                    <colgroup>
                        <col style="width: 150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Customer Line")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCustomerLine" ReadOnly="true" Width="100%" runat="server" />
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
                                    <label class="lblLink lblMandatory" id="lblSourceDepartment">
                                        <%=GetLabel("Asal Department Pasien")%></label>
                                </td>
                                <td>
                                    <input type="hidden" value="" id="hdnSourceDepartmentID" runat="server" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30%" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtSourceDepartmentID" Width="45%" runat="server" />
                                                <asp:TextBox ID="txtSourceDepartmentName" ReadOnly="true" Width="45%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink" id="lblAR">
                                        <%=GetLabel("COA Piutang Usaha")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnARID" runat="server" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30%" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtARGLAccountNo" Width="100%" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtARGLAccountName" ReadOnly="true" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink" id="lblARInProcess">
                                        <%=GetLabel("COA Piutang dalam Proses")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnARInProcessID" runat="server" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30%" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtARInProcessGLAccountNo" Width="100%" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtARInProcessGLAccountName" ReadOnly="true" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink" id="lblARInCare">
                                        <%=GetLabel("COA Piutang dalam Perawatan")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnARInCareID" runat="server" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30%" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtARInCareGLAccountNo" Width="100%" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtARInCareGLAccountName" ReadOnly="true" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink" id="lblARAdjustment">
                                        <%=GetLabel("COA Penyesuaian Piutang")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnARAdjustmentID" runat="server" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30%" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtARAdjustmentGLAccountNo" Width="100%" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtARAdjustmentGLAccountName" ReadOnly="true" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink" id="lblARDiscount">
                                        <%=GetLabel("COA Diskon Piutang")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnARDiscountID" runat="server" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30%" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtARDiscountGLAccountNo" Width="100%" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtARDiscountGLAccountName" ReadOnly="true" Width="100%" />
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
                                        <asp:TemplateField HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                    alt="" style="float: left; margin-left: 7px" />
                                                <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                    alt="" />
                                                <input type="hidden" class="ID" value="<%#: Eval("ID")%>" />
                                                <input type="hidden" class="SourceDepartmentID" value="<%#: Eval("SourceDepartmentID")%>" />
                                                <input type="hidden" class="SourceDepartmentName" value="<%#: Eval("SourceDepartmentName")%>" />
                                                <input type="hidden" class="AR" value="<%#: Eval("AR")%>" />
                                                <input type="hidden" class="ARGLAccountNo" value="<%#: Eval("ARGLAccountNo")%>" />
                                                <input type="hidden" class="ARGLAccountName" value="<%#: Eval("ARGLAccountName")%>" />
                                                <input type="hidden" class="ARInProcess" value="<%#: Eval("ARInProcess")%>" />
                                                <input type="hidden" class="ARInProcessGLAccountNo" value="<%#: Eval("ARInProcessGLAccountNo")%>" />
                                                <input type="hidden" class="ARInProcessGLAccountName" value="<%#: Eval("ARInProcessGLAccountName")%>" />
                                                <input type="hidden" class="ARInCare" value="<%#: Eval("ARInCare")%>" />
                                                <input type="hidden" class="ARInCareGLAccountNo" value="<%#: Eval("ARInCareGLAccountNo")%>" />
                                                <input type="hidden" class="ARInCareGLAccountName" value="<%#: Eval("ARInCareGLAccountName")%>" />
                                                <input type="hidden" class="ARAdjustment" value="<%#: Eval("ARAdjustment")%>" />
                                                <input type="hidden" class="ARAdjustmentGLAccountNo" value="<%#: Eval("ARAdjustmentGLAccountNo")%>" />
                                                <input type="hidden" class="ARAdjustmentGLAccountName" value="<%#: Eval("ARAdjustmentGLAccountName")%>" />
                                                <input type="hidden" class="ARDiscount" value="<%#: Eval("ARDiscount")%>" />
                                                <input type="hidden" class="ARDiscountGLAccountNo" value="<%#: Eval("ARDiscountGLAccountNo")%>" />
                                                <input type="hidden" class="ARDiscountGLAccountName" value="<%#: Eval("ARDiscountGLAccountName")%>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="150px" HeaderText="Department" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <div>
                                                    <b>
                                                        <%#:Eval("SourceDepartmentID") %></b></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="COA" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <div>
                                                    <i>
                                                        <%= GetLabel("COA Piutang Usaha : ")%>
                                                    </i><b>
                                                        <%#:Eval("cfAR") %></b></div>
                                                <div>
                                                    <i>
                                                        <%= GetLabel("COA Piutang dalam Proses : ")%>
                                                    </i><b>
                                                        <%#:Eval("cfARInProcess") %></b></div>
                                                <div>
                                                    <i>
                                                        <%= GetLabel("COA Piutang dalam Perawatan : ")%>
                                                    </i><b>
                                                        <%#:Eval("cfARInCare") %></b></div>
                                                <div>
                                                    <i>
                                                        <%= GetLabel("COA Penyesuaian Piutang : ")%>
                                                    </i><b>
                                                        <%#:Eval("cfARAdjustment") %></b></div>
                                                <div>
                                                    <i>
                                                        <%= GetLabel("COA Diskon Piutang : ")%>
                                                    </i><b>
                                                        <%#:Eval("cfARDiscount") %></b></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="170px" HeaderText="Dibuat Oleh" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <div style="font-size:small; font-style:italic">
                                                    <%#:Eval("CreatedByName") %></div>
                                                <div style="font-size:x-small">
                                                    <%#:Eval("cfCreatedDateTimeInString") %></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="170px" HeaderText="Diubah Oleh" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <div style="font-size:small; font-style:italic">
                                                    <%#:Eval("LastUpdatedByName") %></div>
                                                <div style="font-size:x-small">
                                                    <%#:Eval("cfLastUpdatedDateTimeInString") %></div>
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
