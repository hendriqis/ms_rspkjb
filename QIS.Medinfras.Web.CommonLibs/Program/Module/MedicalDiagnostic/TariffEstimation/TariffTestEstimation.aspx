<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="TariffTestEstimation.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.TariffTestEstimation" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnCalculate" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><div>
            <%=GetLabel("Calculate")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        var totalPayer = null;
        var totalPatient = null;
        var grandTotal = null;

        function onLoad() {
            $('#<%=grdView.ClientID %> .chkIsSelected input').removeAttr('checked');
        }

        function addItemFilterRow() {
            getCheckedMember();
            $trHeader = $('#<%=grdView.ClientID %> tr:eq(0)');
            $trFilter = $("<tr><td></td><td></td></tr>");

            $input = $("<input type='text' id='txtFilterItem' style='width:100%;height:20px' />").val($('#<%=hdnFilterItem.ClientID %>').val());
            $trFilter.find('td').eq(1).append($input);
            $trFilter.insertAfter($trHeader);
        }

        $('#txtFilterItem').live('keypress', function (e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            if (code == 13) {
                getCheckedMember();
                $('#<%=hdnFilterItem.ClientID %>').val($(this).val());
                e.preventDefault();
                cbpView.PerformCallback('refresh');
            }
        });

        //#region Calculate Item Tariff Estimation
        function calculateTariffEstimation($row) {
            var totalPrice = $row.find('.hdnPrice').val() * $row.find('.txtQty').val();
            var payer, patient, discount = 0;
            if ($row.find('.hdnIsDiscountInPercentage').val())
                discount = ($row.find('.hdnDiscountAmount').val() / 100) * totalPrice;
            else
                discount = $row.find('.hdnDiscountAmount').val();

            totalPrice -= discount;
            if ($row.find('.hdnIsCoverageInPercentage').val())
                payer = ($row.find('.hdnCoverageAmount').val() / 100) * totalPrice;
            else
                payer = $row.find('.hdnCoverageAmount').val();
            patient = totalPrice - payer;
            totalPayer += payer;
            totalPatient += patient;
            grandTotal += totalPrice;

            $row.find('.txtPayer').val(payer).trigger('changeValue');
            $row.find('.txtPatient').val(patient).trigger('changeValue');
            $row.find('.txtTotal').val(totalPrice).trigger('changeValue');
            $('#trFooter').find('.txtPayerTotal').val(totalPayer).trigger('changeValue');
            $('#trFooter').find('.txtPatientTotal').val(totalPatient).trigger('changeValue');
            $('#trFooter').find('.txtGrandTotal').val(grandTotal).trigger('changeValue');
        }
        //#endregion

        $('#tblSelectedItem .txtQty').live('change', function () {
            $row = $(this).closest('tr');
            totalPayer = 0;
            totalPatient = 0;
            grandTotal = 0;
            $('#tblSelectedItem tr').each(function () {
                if ($(this).find('.keyField').val() != undefined) {
                    calculateTariffEstimation($(this));
                }
            });
        });

        $(function () {
            setDatePicker('<%=txtTransactionDate.ClientID %>');
            $('#<%=txtTransactionDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
            addItemFilterRow();

            var type = $("#rblJnsPasien input:checked").val();
            SetCboClass(type);
            var department = "";

            if (type == "RJ") department = 'OUTPATIENT';
            else department = 'INPATIENT';

            $("#rblJnsPasien").die('change');
            $("#rblJnsPasien").live('change', function () {
                var t = $("#rblJnsPasien input:checked").val();
                SetCboClass(t);
            });

            $('#<%=btnCalculate.ClientID %>').click(function () {
                getCheckedMember();
                var selectedItem = $('#<%=hdnSelectedMember.ClientID %>').val();
                if (selectedItem == '')
                    showToast('Warning', 'Please Select Item First');
                else {
                    showLoadingPanel();
                    var businessPartnerID = $('#<%=hdnBusinessPartnerID.ClientID %>').val();
                    if (businessPartnerID == '')
                        businessPartnerID = '1';
                    var coverageTypeID = $('#<%=hdnCoverageTypeID.ClientID %>').val();
                    if (coverageTypeID == '')
                        coverageTypeID = '0';
                    Methods.getTariffEstimation(cboClass.GetValue(), businessPartnerID, coverageTypeID, selectedItem, getTrxDate(), department, 1, function (result) {
                        if (result != null) {
                            totalPatient = 0;
                            totalPayer = 0;
                            grandTotal = 0;
                            for (var i = 0; i < result.length; i++) {
                                $row = null;
                                $('#tblSelectedItem .trSelectedItem').each(function () {

                                    if ($(this).find('.keyField').val() == result[i].ItemID) {
                                        $row = $(this);
                                        $row.find('.hdnCoverageAmount').val(result[i].CoverageAmount);
                                        $row.find('.hdnDiscountAmount').val(result[i].DiscountAmount);
                                        $row.find('.hdnIsCoverageInPercentage').val(result[i].IsCoverageInPercentage);
                                        $row.find('.hdnIsDiscountInPercentage').val(result[i].IsDiscountInPercentage);
                                        $row.find('.hdnPrice').val(result[i].Price);
                                        calculateTariffEstimation($row);
                                    }
                                });
                            }
                        }
                        hideLoadingPanel();
                    });
                }
            });
        });

        function SetCboClass(type) {
            if (type == "RJ") {
                cboClass.SetValue($('#<%=hdnOutPatientID.ClientID %>').val());
                cboClass.SetEnabled(false);
            }
            else {
                cboClass.SetEnabled(true);
            }
        }

        function getTrxDate() {
            var date = Methods.getDatePickerDate($('#<%=txtTransactionDate.ClientID %>').val());
            var dateInYMD = Methods.dateToYMD(date);
            return dateInYMD;
        }

        //#region Business Partner
        function getBusinessPartnerFilterExpression() {
            var FilterExpression = "GCCustomerType = '" + cboCustomerType.GetValue() + "' AND IsDeleted=0";
            return FilterExpression;
        }

        $('#lblBusinessPartner.lblLink').live('click', function () {
            openSearchDialog('payer', getBusinessPartnerFilterExpression(), function (value) {
                $('#<%=txtBusinessPartnerCode.ClientID %>').val(value);
                onTxtBusinessPartnerCodeChanged(value);
            })
        });

        $('#<%=txtBusinessPartnerCode.ClientID %>').live('change', function () {
            onTxtBusinessPartnerCodeChanged($(this).val());
        });

        function onTxtBusinessPartnerCodeChanged(value) {
            var filterExpression = getBusinessPartnerFilterExpression() + " AND BusinessPartnerCode = '" + value + "'";
            Methods.getObject('GetvCustomerList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnBusinessPartnerID.ClientID %>').val(result.BusinessPartnerID);
                    $('#<%=txtBusinessPartnerName.ClientID %>').val(result.BusinessPartnerName);
                }
                else {
                    $('#<%=hdnBusinessPartnerID.ClientID %>').val('');
                    $('#<%=txtBusinessPartnerCode.ClientID %>').val('');
                    $('#<%=txtBusinessPartnerName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Coverage Type
        function getCoverageTypeFilterExpression() {
            if ($('#<%=hdnBusinessPartnerID.ClientID %>').val() == "") {
                var FilterExpression = "BusinessPartnerID = 0";
                return FilterExpression;
            }
            else {
                var FilterExpression = "BusinessPartnerID = " + $('#<%=hdnBusinessPartnerID.ClientID %>').val();
                return FilterExpression;
            }
        }

        $('#lblCoverageType.lblLink').live('click', function () {
            openSearchDialog('customercoverage', getCoverageTypeFilterExpression(), function (value) {
                $('#<%=txtCoverageTypeCode.ClientID %>').val(value);
                onTxtCoverageTypeCodeChanged(value);
            })
        });

        $('#<%=txtCoverageTypeCode.ClientID %>').live('change', function () {
            onTxtCoverageTypeCodeChanged($(this).val());
        });

        function onTxtCoverageTypeCodeChanged(value) {
            var filterExpression = getCoverageTypeFilterExpression() + " AND CoverageTypeCode = '" + value + "'";
            Methods.getObject('GetvContractCoverageList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnCoverageTypeID.ClientID %>').val(result.CoverageTypeID);
                    $('#<%=txtCoverageTypeName.ClientID %>').val(result.CoverageTypeName);
                }
                else {
                    $('#<%=hdnCoverageTypeID.ClientID %>').val('');
                    $('#<%=txtCoverageTypeCode.ClientID %>').val('');
                    $('#<%=txtCoverageTypeName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        function getTrxDate() {
            var date = Methods.getDatePickerDate($('#<%=txtTransactionDate.ClientID %>').val());
            var dateInYMD = Methods.dateToYMD(date);
            return dateInYMD;
        }

        function getCheckedMember() {
            //            var lstSelectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().split(',');
            //            var result = '';
            //            $('#<%=grdView.ClientID %> .chkIsSelected input').each(function () {
            //                if ($(this).is(':checked')) {
            //                    var key = $(this).closest('tr').find('.keyField').html();
            //                    if (lstSelectedMember.indexOf(key) < 0)
            //                        lstSelectedMember.push(key);
            //                }
            //                else {
            //                    var key = $(this).closest('tr').find('.keyField').html();
            //                    if (lstSelectedMember.indexOf(key) > -1)
            //                        lstSelectedMember.splice(lstSelectedMember.indexOf(key), 1);
            //                }
            //            });
            //            $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
            var lstSelectedMember = [];
            var lstSelectedMemberQty = [];
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
            setPaging($("#paging"), pageCount, function (page) {
                getCheckedMember();
                cbpView.PerformCallback('changepage|' + page);
            });

            //#region Item Group
            $('#lblItemGroup.lblLink').click(function () {
                var filterExpression = "GCItemType = '" + $('#<%=hdnGCItemType.ClientID %>').val() + "' AND IsDeleted = 0";
                openSearchDialog('itemgroup', filterExpression, function (value) {
                    $('#<%=txtItemGroupCode.ClientID %>').val(value);
                    onTxtItemGroupCodeChanged(value);
                });
            });

            $('#<%=txtItemGroupCode.ClientID %>').change(function () {
                onTxtItemGroupCodeChanged($(this).val());
            });

            function onTxtItemGroupCodeChanged(value) {
                var filterExpression = "ItemGroupCode = '" + value + "'";
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
                    cbpView.PerformCallback('refresh');
                });
            }
            //#endregion
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                setPaging($("#paging"), pageCount, function (page) {
                    getCheckedMember();
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else { }
            addItemFilterRow();
        }
        //#endregion

        $('#<%=grdView.ClientID %> .chkIsSelected input').live('change', function () {
            if ($(this).is(':checked')) {
                $selectedTr = $(this).closest('tr');

                $newTr = $('#tmplSelectedTestItem').html();
                $newTr = $newTr.replace(/\$\{ItemName1}/g, $selectedTr.find('.tdItemName1').html());
                $newTr = $newTr.replace(/\$\{ItemID}/g, $selectedTr.find('.keyField').html());
                $newTr = $($newTr);
                $newTr.insertBefore($('#trFooter'));
            }
            else {
                totalPatient = 0;
                totalPayer = 0;
                grandTotal = 0;
                var ct = 0;
                var id = $(this).closest('tr').find('.keyField').html();
                $('#tblSelectedItem tr').each(function () {
                    if ($(this).find('.keyField').val() == id) {
                        $(this).remove();
                    }
                });
                $('#tblSelectedItem tr').each(function () {
                    if ($(this).find('.keyField').val() != undefined) {
                        calculateTariffEstimation($(this));
                        ct++;
                    }
                });
                if (ct == 0) {
                    $('#trFooter').find('.txtPayerTotal').val(totalPayer).trigger('changeValue');
                    $('#trFooter').find('.txtPatientTotal').val(totalPatient).trigger('changeValue');
                    $('#trFooter').find('.txtGrandTotal').val(grandTotal).trigger('changeValue');
                }
            }
        });

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
                totalPatient = 0;
                totalPayer = 0;
                grandTotal = 0;
                var ct = 0;
                $(this).closest('tr').remove();
                $('#tblSelectedItem tr').each(function () {
                    if ($(this).find('.keyField').val() != undefined) {
                        calculateTariffEstimation($(this));
                        ct++;
                    }
                });
                if (ct == 0) {
                    $('#trFooter').find('.txtPayerTotal').val(totalPayer).trigger('changeValue');
                    $('#trFooter').find('.txtPatientTotal').val(totalPatient).trigger('changeValue');
                    $('#trFooter').find('.txtGrandTotal').val(grandTotal).trigger('changeValue');
                }
            }
        });

        function onCboCustomerTypeChanged() {
            $('#<%=hdnBusinessPartnerID.ClientID %>').val('');
            $('#<%=txtBusinessPartnerCode.ClientID %>').val('');
            $('#<%=txtBusinessPartnerName.ClientID %>').val('');
            $('#<%=hdnCoverageTypeID.ClientID %>').val('');
            $('#<%=txtCoverageTypeCode.ClientID %>').val('');
            $('#<%=txtCoverageTypeName.ClientID %>').val('');
        }

        function onCboServiceUnitChanged() {
            showLoadingPanel();
            cbpView.PerformCallback('refresh');
            hideLoadingPanel();
        }

        function onCboItemGroupValueChanged(evt) {
            cbpView.PerformCallback('refresh');
        }
    </script>
    <div style="padding: 10px;">
        <script id="tmplSelectedTestItem" type="text/x-jquery-tmpl">
            <tr class="trSelectedItem">
                <td align="center">
                    <input type="checkbox" class="chkIsSelected2" />
                    <input type="hidden" class="keyField" value='${ItemID}' />
                    <input type="hidden" class="hdnCoverageAmount" value="0" />
                    <input type="hidden" class="hdnIsCoverageInPercentage" value="0" />
                    <input type="hidden" class="hdnDiscountAmount" value="0" />
                    <input type="hidden" class="hdnIsDiscountInPercentage" value="0" />
                    <input type="hidden" class="hdnPrice" value="0" />
                </td>
                <td>${ItemName1}</td>
                <td><input type="text" class="txtQty number" value="1" style="width:100%" /></td>
                <td><input type="text" class="txtPayer txtCurrency" value="0" readonly="readonly" style="width:100%" /></td>
                <td><input type="text" class="txtPatient txtCurrency" value="0" readonly="readonly" style="width:100%" /></td>
                <td><input type="text" class="txtTotal txtCurrency" value="0" readonly="readonly" style="width:100%" /></td>
            </tr>
        </script>
        <input type="hidden" id="hdnPageTitle" runat="server" />
        <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
        <input type="hidden" id="hdnOutPatientID" runat="server" value="" />
        <input type="hidden" id="hdnID" runat="server" />
        <input type="hidden" id="hdnFilterItem" runat="server" />
        <input type="hidden" id="hdnGCItemType" runat="server" />
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                            <col />
                        </colgroup>
                        <tr id="trServiceUnit" runat="server">
                            <td class="tdLabel">
                                <label id="lblServiceUnit" class="lblMandatory" runat="server">
                                    <%=GetLabel("Penunjang Medis")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" Width="100%"
                                    runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e) { onCboServiceUnitChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label id="lblPenjamin" class="lblMandatory" runat="server">
                                    <%=GetLabel("Pembayar")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboCustomerType" ClientInstanceName="cboCustomerType" Width="100%"
                                    runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e) { onCboCustomerTypeChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblBusinessPartner">
                                    <%=GetLabel("Instansi")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnBusinessPartnerID" runat="server" value="" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 100px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtBusinessPartnerCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBusinessPartnerName" ReadOnly="true" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblCoverageType">
                                    <%=GetLabel("Tipe Jaminan")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnCoverageTypeID" runat="server" value="" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 100px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtCoverageTypeCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCoverageTypeName" ReadOnly="true" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblItemGroup">
                                    <%=GetLabel("Kelompok Item")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnItemGroupID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 100px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtItemGroupCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtItemGroupName" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Jenis Pasien")%>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblJnsPasien" runat="server" RepeatDirection="Horizontal"
                                    RepeatLayout="Flow" ClientIDMode="Static">
                                    <asp:ListItem Text="RJ" Value="RJ" Selected="True" />
                                    <asp:ListItem Text="RI" Value="RI" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Kelas") %>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboClass" ClientInstanceName="cboClass" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal") %>
                            </td>
                            <td style="padding-right: 1px; width: 145px">
                                <asp:TextBox ID="txtTransactionDate" Width="120px" CssClass="datepicker" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 35%" />
                <col style="width: 65%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <input type="hidden" value="" id="hdnParam" runat="server" />
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative; font-size: 0.95em;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="ItemID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ItemName1" HeaderText="Pelayanan" ItemStyle-CssClass="tdItemName1" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Data Tidak Tersedia")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="paging">
                            </div>
                        </div>
                    </div>
                </td>
                <td style="padding: 5px; vertical-align: top">
                    <table id="tblSelectedItem" class="grdView notAllowSelect" cellspacing="0" rules="all">
                        <tr>
                            <th rowspan="2" style="width: 40px">
                                &nbsp;
                            </th>
                            <th rowspan="2" align="center">
                                <%=GetLabel("Pelayanan")%>
                            </th>
                            <th style="width: 60px" rowspan="2" align="center">
                                <%=GetLabel("Jumlah")%>
                            </th>
                            <th colspan="3" align="center">
                                <%=GetLabel("Harga")%>
                            </th>
                        </tr>
                        <tr>
                            <th style="width: 120px" align="center">
                                <%=GetLabel("Instansi")%>
                            </th>
                            <th style="width: 120px" align="center">
                                <%=GetLabel("Pasien")%>
                            </th>
                            <th style="width: 120px" align="center">
                                <%=GetLabel("Total")%>
                            </th>
                        </tr>
                        <tr id="trFooter">
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <%=GetLabel("Grand Total Tariff")%>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <input type="text" class="txtPayerTotal txtCurrency" value="0" readonly="readonly"
                                    style="width: 100%" />
                            </td>
                            <td>
                                <input type="text" class="txtPatientTotal txtCurrency" value="0" readonly="readonly"
                                    style="width: 100%" />
                            </td>
                            <td>
                                <input type="text" class="txtGrandTotal txtCurrency" value="0" readonly="readonly"
                                    style="width: 100%" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
    </div>
</asp:Content>
