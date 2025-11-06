<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master" AutoEventWireup="true"
    CodeBehind="InformationPatientBillingHistory.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.InformationPatientBillingHistory" %>

<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPivotGrid" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v11.1.Export, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPivotGrid.Export" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png")%>' alt="" /><div>
            <%=GetLabel("Refresh")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtPeriodFrom.ClientID %>');
            setDatePicker('<%=txtPeriodTo.ClientID %>');

            var grd = new customGridView();
            grd.init('<%=grdView.ClientID %>', '<%=hdnRegistrationID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');

            $('#<%=grdView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
                if ($(this).attr('class') == 'selected') {
                    $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                    $('#<%=hdnRegistrationID.ClientID %>').val($(this).find('.keyField').html());
                    cbpViewRegDt.PerformCallback('refreshRegDt');
                    cbpPatientCharges.PerformCallback('refreshPatientCharges');
                    cbpViewPatientPayment.PerformCallback('refreshPatientPayment');
                    cbpViewPatientBill.PerformCallback('refreshPatientBill');

                    $('.grdViewARInvoice tr:gt(0)').remove();
                }
            });

            $('#ulTabGrdDetail li').live('click', function () {
                $('#ulTabGrdDetail li.selected').removeAttr('class');
                $('.containerGrdDt').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#' + $contentID).show();
                $(this).addClass('selected');
            });

            $('.grdViewPatientCharges td.tdExpand').live('click', function () {
                $tr = $(this).parent();
                $trDetail = $(this).parent().next();
                if ($trDetail.attr('class') != 'trDetail') {
                    $('#ulTabGrdDetail li:eq(0)').click();

                    $trCollapse = $('.trDetail');

                    $newTr = $("<tr><td></td><td colspan='20'></td></tr>").attr('class', 'trDetail');
                    $newTr.insertAfter($tr);
                    $newTr.find('td').last().append($('#containerDtGridAR'));

                    if ($trCollapse != null) {
                        $trCollapse.remove();
                    }

                    $('#<%=hdnPaymentExpandID.ClientID %>').val($tr.find('.keyField').html().trim());
                }
                else {
                    $trDetail.remove();
                }
            });

            $('.grdViewPatientBill td.tdExpand').live('click', function () {
                $tr = $(this).parent();
                $trDetail = $(this).parent().next();
                if ($trDetail.attr('class') != 'trDetail') {
                    $('#ulTabGrdDetail li:eq(0)').click();

                    $trCollapse = $('.trDetail');

                    $newTr = $("<tr><td></td><td colspan='20'></td></tr>").attr('class', 'trDetail');
                    $newTr.insertAfter($tr);
                    $newTr.find('td').last().append($('#containerDtGridAR'));

                    if ($trCollapse != null) {
                        $trCollapse.remove();
                    }

                    $('#<%=hdnPaymentExpandID.ClientID %>').val($tr.find('.keyField').html().trim());
                }
                else {
                    $trDetail.remove();
                }
            });

            $('.grdViewPatientPayment td.tdExpand').live('click', function () {
                $tr = $(this).parent();
                $trDetail = $(this).parent().next();
                if ($trDetail.attr('class') != 'trDetail') {
                    $('#ulTabGrdDetail li:eq(0)').click();

                    $trCollapse = $('.trDetail');

                    $newTr = $("<tr><td></td><td colspan='20'></td></tr>").attr('class', 'trDetail');
                    $newTr.insertAfter($tr);
                    $newTr.find('td').last().append($('#containerDtGridAR'));

                    if ($trCollapse != null) {
                        $trCollapse.remove();
                    }

                    $('#<%=hdnPaymentExpandID.ClientID %>').val($tr.find('.keyField').html().trim());
                }
                else {
                    $trDetail.remove();
                }
            });

            $('.grdViewPatientBill td.tdExpand').live('click', function () {
                $tr = $(this).parent();
                $trDetail = $(this).parent().next();
                if ($trDetail.attr('class') != 'trDetail') {
                    $('#ulTabGrdDetail li:eq(0)').click();

                    $trCollapse = $('.trDetail');

                    $newTr = $("<tr><td></td><td colspan='20'></td></tr>").attr('class', 'trDetail');
                    $newTr.insertAfter($tr);
                    $newTr.find('td').last().append($('#containerDtGridAR'));

                    if ($trCollapse != null) {
                        $trCollapse.remove();
                    }

                    $('#<%=hdnPaymentExpandID.ClientID %>').val($tr.find('.keyField').html().trim());
                }
                else {
                    $trDetail.remove();
                }
            });
        });


        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                $('#<%=hdnFilterExpressionQuickSearchReg.ClientID %>').val(txtSearchView.GenerateFilterExpression());
                if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
                    cbpView.PerformCallback('refresh');
                }
            }, 0);
        }

        $('#btnRefreshLeftPage').die('click');
        $('#btnRefreshLeftPage').live('click', function () {
            cbpView.PerformCallback('refresh');
        });

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        //#region MRN
        $('#lblMRN.lblLink').live('click', function () {
            openSearchDialog('patient', '', function (value) {
                $('#<%=txtMRN.ClientID %>').val(value);
                onTxtMRNChanged(value);
            });
        });
        $('#<%=txtMRN.ClientID %>').live('change', function () {
            onTxtMRNChanged($(this).val());
        });
        function onTxtMRNChanged(value) {
            var filterExpression = "MedicalNo = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetvPatientList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnMRN.ClientID %>').val(result.MRN);
                    $('#<%=txtPatientName.ClientID %>').val(result.PatientName);
                }
                else {
                    $('#<%=hdnMRN.ClientID %>').val('');
                    $('#<%=txtPatientName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        function onRefreshGrdReg() {
            $('#<%=hdnFilterExpressionQuickSearchReg.ClientID %>').val(txtSearchViewReg.GenerateFilterExpression());
            cbpView.PerformCallback('refresh');
        }

        $('#<%=chkIsPreviousEpisodePatient.ClientID %>').die();
        $('#<%=chkIsPreviousEpisodePatient.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
                $('#<%=txtPeriodFrom.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtPeriodTo.ClientID %>').attr('readonly', 'readonly');
            }
            else {
                $('#<%=txtPeriodFrom.ClientID %>').removeAttr('readonly');
                $('#<%=txtPeriodTo.ClientID %>').removeAttr('readonly');
            }
        });

        $('#<%=btnRefresh.ClientID %>').live('click', function () {
            if ($('#<%:hdnMRN.ClientID %>').val() == "") {
                showToast("ERROR", 'Error Message : ' + "Nomor Rekam Medis harus dipilih terlebih dahulu !");
            }
            else {
                onRefreshGrdReg();
            }
        });

        function onTxtSearchViewRegSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                onRefreshGrdReg();
                setTimeout(function () {
                    s.SetFocus();
                }, 0);
            }, 0);
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0) {
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();
                } else {
                    $('#<%=hdnRegistrationID.ClientID %>').val("0");
                    cbpViewRegDt.PerformCallback('refreshRegDt');
                    cbpPatientCharges.PerformCallback('refreshPatientCharges');
                    cbpViewPatientPayment.PerformCallback('refreshPatientPayment');
                    cbpViewPatientBill.PerformCallback('refreshPatientBill');
                }

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else {
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
            }
        }

        $('#<%=txtPeriodFrom.ClientID %>').live('change', function () {
            var start = $('#<%=txtPeriodFrom.ClientID %>').val();
            var end = $('#<%=txtPeriodTo.ClientID %>').val();

            $('#<%=txtPeriodFrom.ClientID %>').val(validateDateFromTo(start, end));
        });

        $('#<%=txtPeriodTo.ClientID %>').live('change', function () {
            var start = $('#<%=txtPeriodFrom.ClientID %>').val();
            var end = $('#<%=txtPeriodTo.ClientID %>').val();

            $('#<%=txtPeriodTo.ClientID %>').val(validateDateToFrom(start, end));
        });

        function onAfterSaveAddRecordEntryPopup() {
            cbpPatientCharges.PerformCallback('refreshPatientCharges');
        }

        function onAfterSaveEditRecordEntryPopup() {
            cbpPatientCharges.PerformCallback('refreshPatientCharges');
        }

        function getCheckedMember() {
            var lstSelectedChargesDtID = $('#<%=hdnSelectedChargesDtID.ClientID %>').val().split(',');
            var lstSelectedParamedicID = $('#<%=hdnSelectedParamedicID.ClientID %>').val().split(',');
            var lstSelectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().split(',');
            $('.grdViewPatientCharges input').each(function () {
                if ($(this).is(':checked')) {
                    var oID = $(this).closest('tr').find('.ID').val();
                    var oParamedicID = $(this).closest('tr').find('.ParamedicID').val();
                    var oParam = oID + "|" + oParamedicID;
                    if (lstSelectedChargesDtID.indexOf(oID) < 0) {
                        lstSelectedChargesDtID.push(oID);
                        lstSelectedParamedicID.push(oParamedicID);
                        lstSelectedMember.push(oParam);
                    }
                }
                else {
                    var oID = $(this).closest('tr').find('.ID').val();
                    var oParamedicID = $(this).closest('tr').find('.ParamedicID').val();
                    var oParam = oID + "|" + oParamedicID;
                    if (lstSelectedChargesDtID.indexOf(oID) > -1) {
                        lstSelectedChargesDtID.splice(lstSelectedChargesDtID.indexOf(oID), 1);
                        lstSelectedParamedicID.splice(lstSelectedParamedicID.indexOf(oParamedicID), 1);
                        lstSelectedMember.splice(lstSelectedChargesDtID.indexOf(oParam), 1);
                    }
                }
            });

            $('#<%=hdnSelectedChargesDtID.ClientID %>').val(lstSelectedChargesDtID.join(','));
            $('#<%=hdnSelectedParamedicID.ClientID %>').val(lstSelectedParamedicID.join(','));
            $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
        }

        function onCbpProcessEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'fail')
                showToast('Process Failed', 'Error Message : ' + param[1]);
            else {
                cbpPatientCharges.PerformCallback('refreshPatientCharges');
            }

            $('#<%=hdnSelectedChargesDtID.ClientID %>').val("");
            $('#<%=hdnSelectedParamedicID.ClientID %>').val("");
            $('#<%=hdnSelectedMember.ClientID %>').val("");

            hideLoadingPanel();
        }

    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" value="" />
    <input type="hidden" id="hdnRegistrationID" runat="server" value="" />
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearchReg" runat="server" />
    <input type="hidden" id="hdnPaymentExpandID" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMember" value="" runat="server" />
    <input type="hidden" id="hdnSelectedChargesDtID" value="" runat="server" />
    <input type="hidden" id="hdnSelectedParamedicID" value="" runat="server" />
    <div style="overflow-x: hidden;">
        <table class="tblContentArea">
            <tr>
                <td style="padding: 5px; vertical-align: top; width: 100%">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                            <col style="width: 70%" />
                        </colgroup>
                        <tr>
                            <td valign="top" align="left">
                                <table style="width: 100%">
                                    <colgroup>
                                        <col style="width: 110px" />
                                        <col style="width: 150px" />
                                        <col style="width: 15px" />
                                        <col style="width: 150px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblMandatory lblLink" id="lblMRN" style="font-size: smaller">
                                                <%=GetLabel("No. RM")%></label>
                                        </td>
                                        <td colspan="3">
                                            <table>
                                                <colgroup>
                                                    <col style="width: 130px" />
                                                    <col style="width: 250px" />
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td>
                                                        <input type="hidden" id="hdnMRN" value="" runat="server" />
                                                        <asp:TextBox ID="txtMRN" Width="100%" runat="server" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPatientName" ReadOnly="true" Width="100%" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label class="lblNormal" style="font-size: smaller">
                                                <%=GetLabel("Tanggal Registrasi") %></label>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" Width="120px" ID="txtPeriodFrom" CssClass="datepicker" />
                                        </td>
                                        <td style="text-align: center; font-size: small">
                                            <%=GetLabel("s/d") %>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" Width="120px" ID="txtPeriodTo" CssClass="datepicker" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsPreviousEpisodePatient" runat="server" Checked="false" Text=" Abaikan Tanggal" style="font-size: smaller" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label class="lblNormal" style="font-size: smaller">
                                                <%=GetLabel("Department") %></label>
                                        </td>
                                        <td colspan="3">
                                            <dxe:ASPxComboBox ID="cboDepartment" ClientInstanceName="cboDepartment" Width="50%"
                                                runat="server">
                                            </dxe:ASPxComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label style="font-size: smaller">
                                                <%=GetLabel("Quick Filter") %></label>
                                        </td>
                                        <td colspan="3">
                                            <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchViewReg"
                                                ID="txtSearchViewReg" Width="100%" Watermark="Search">
                                                <ClientSideEvents SearchClick="function(s){ onTxtSearchViewRegSearchClick(s); }" />
                                                <IntellisenseHints>
                                                    <qis:QISIntellisenseHint Text="RegistrationNo" FieldName="RegistrationNo" />
                                                    <qis:QISIntellisenseHint Text="ServiceUnitName" FieldName="ServiceUnitName" />
                                                </IntellisenseHints>
                                            </qis:QISIntellisenseTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <hr style="padding: 0 0 0 0; margin: 0 0 0 0;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <div style="position: relative;" id="divView">
                                                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                                                    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                                                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                                                    <PanelCollection>
                                                        <dx:PanelContent ID="PanelContent1" runat="server">
                                                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridProcessList" Style="height: 400px;
                                                                overflow-y: auto">
                                                                <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                    <Columns>
                                                                        <asp:BoundField DataField="RegistrationID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                        <asp:TemplateField HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                            <HeaderTemplate>
                                                                                <%=GetLabel("Data Registrasi") %>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <label style="font-size: large; font-weight: bolder">
                                                                                    <%#:Eval("RegistrationNo") %></label>
                                                                                <label style="font-size: small; font-weight: bolder; font-style: italic">
                                                                                    <%#:Eval("LinkedRegistrationNo") %></label>
                                                                                <br />
                                                                                <label style="font-size: smaller">
                                                                                    (<%#:Eval("MedicalNo") %>)</label>
                                                                                <label style="font-size: medium;">
                                                                                    <%#:Eval("PatientName") %></label>
                                                                                <br />
                                                                                <label style="font-size: smaller">
                                                                                    (<%#:Eval("RegistrationStatus") %>)</label>
                                                                                <br />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <EmptyDataTemplate>
                                                                        <%=GetLabel("---Tidak Ada Data---")%>
                                                                    </EmptyDataTemplate>
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                        </dx:PanelContent>
                                                    </PanelCollection>
                                                </dxcp:ASPxCallbackPanel>
                                                <div class="imgLoadingGrdView" id="containerImgLoadingView">
                                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                                </div>
                                                <div class="containerPaging">
                                                    <div class="wrapperPaging">
                                                        <div id="paging">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td valign="top" align="left">
                                <table style="width: 100%">
                                    <tr>
                                        <td valign="top" align="left">
                                            <dxcp:ASPxCallbackPanel ID="cbpViewRegDt" runat="server" Width="100%" Height="50%"
                                                ClientInstanceName="cbpViewRegDt" ShowLoadingPanel="false" OnCallback="cbpViewRegDt_Callback">
                                                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent2" runat="server">
                                                        <asp:Panel runat="server" ID="pnlViewRegDt" CssClass="pnlContainerGridProcessList"
                                                            Style="height: auto">
                                                            <table style="width: 100%">
                                                                <colgroup>
                                                                    <col style="width: 65%" />
                                                                    <col style="width: 35%" />
                                                                </colgroup>
                                                                <tr>
                                                                    <td valign="top" align="left">
                                                                        <table style="width: 100%;">
                                                                            <colgroup>
                                                                                <col style="width: 170px" />
                                                                                <col />
                                                                            </colgroup>
                                                                            <tr>
                                                                                <td style="font-size: smaller; font-style: italic">
                                                                                    <%=GetLabel("No. Registrasi") %>
                                                                                </td>
                                                                                <td>
                                                                                    <label runat="server" id="lblRegistrationNo" style="font-weight: bold" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trLinkRegistration" runat="server">
                                                                                <td style="font-size: smaller; font-style: italic">
                                                                                    <%=GetLabel("No. Registrasi Link") %>
                                                                                </td>
                                                                                <td>
                                                                                    <label runat="server" id="lblLinkedRegistrationNo" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="font-size: smaller; font-style: italic">
                                                                                    <%=GetLabel("Pasien") %>
                                                                                </td>
                                                                                <td>
                                                                                    <label runat="server" id="lblPatient" style="font-weight: bold" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="font-size: smaller; font-style: italic">
                                                                                    <%=GetLabel("Tgl/Jam Registrasi s/d Pulang") %>
                                                                                </td>
                                                                                <td>
                                                                                    <label runat="server" id="lblRegistrationDischargeDate" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="font-size: smaller; font-style: italic">
                                                                                    <%=GetLabel("Department") %>
                                                                                </td>
                                                                                <td>
                                                                                    <label runat="server" id="lblDepartment" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="font-size: smaller; font-style: italic">
                                                                                    <%=GetLabel("Unit Registrasi") %>
                                                                                </td>
                                                                                <td>
                                                                                    <label runat="server" id="lblServiceUnitName" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="font-size: smaller; font-style: italic">
                                                                                    <%=GetLabel("Penjamin Bayar") %>
                                                                                </td>
                                                                                <td>
                                                                                    <label runat="server" id="lblBusinessPartnerName" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td valign="top" align="left">
                                                                        <table style="width: 100%">
                                                                            <colgroup>
                                                                                <col style="width: 140px" />
                                                                                <col />
                                                                            </colgroup>
                                                                            <tr>
                                                                                <td style="font-size: smaller; font-style: italic">
                                                                                    <%=GetLabel("Total Transaksi") %>
                                                                                </td>
                                                                                <td align="right">
                                                                                    <label runat="server" id="lblChargesAmount" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="font-size: smaller; font-style: italic">
                                                                                    <%=GetLabel("Total Tagihan") %>
                                                                                </td>
                                                                                <td align="right">
                                                                                    <label runat="server" id="lblBillingAmount" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="font-size: smaller; font-style: italic">
                                                                                    <%=GetLabel("Total Pembayaran") %>
                                                                                </td>
                                                                                <td align="right">
                                                                                    <label runat="server" id="lblPaymentAmount" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="font-size: smaller; font-style: italic">
                                                                                    <%=GetLabel("Sisa Tagihan") %>
                                                                                </td>
                                                                                <td align="right">
                                                                                    <label runat="server" id="lblSisaAmount" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dxcp:ASPxCallbackPanel>
                                        </td>
                                    </tr>
                                    <tr id="trLine0" runat="server">
                                        <td colspan="2">
                                            <hr style="padding: 0 0 0 0; margin: 0 0 0 0;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dxcp:ASPxCallbackPanel ID="cbpPatientCharges" runat="server" Width="100%" ClientInstanceName="cbpPatientCharges"
                                                ShowLoadingPanel="false" OnCallback="cbpPatientCharges_Callback">
                                                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent3" runat="server">
                                                        <asp:Panel runat="server" ID="Panel2" CssClass="pnlContainerGrid" Style="height: 300px;
                                                            overflow-y: auto">
                                                            <table class="grdViewPatientCharges grdSelected" cellspacing="0" width="100%" rules="all">
                                                                <tr>
                                                                    <th class="keyField">
                                                                    </th>
                                                                    <th align="left" style="width: 160px; font-size: smaller">
                                                                        <%=GetLabel("Info Transaksi") %>
                                                                    </th>
                                                                    <th align="left" style="font-size: smaller">
                                                                        <%=GetLabel("Detail Transaksi") %>
                                                                    </th>
                                                                    <th align="right" style="width: 100px; font-size: smaller">
                                                                        <%=GetLabel("Harga Satuan") %>
                                                                    </th>
                                                                    <th align="right" style="width: 100px; font-size: smaller">
                                                                        <%=GetLabel("Jumlah Transaksi") %>
                                                                    </th>
                                                                    <th align="center" style="width: 180px; font-size: smaller">
                                                                        <%=GetLabel("Info Dibuat") %>
                                                                    </th>
                                                                </tr>
                                                                <asp:ListView runat="server" ID="lvwPatientCharges">
                                                                    <EmptyDataTemplate>
                                                                        <tr class="trEmpty">
                                                                            <td colspan="20">
                                                                                <%=GetLabel("---Tidak Ada Data---") %>
                                                                            </td>
                                                                        </tr>
                                                                    </EmptyDataTemplate>
                                                                    <ItemTemplate>
                                                                        <tr>
                                                                            <td class="keyField">
                                                                                <%#: Eval("DtID")%>
                                                                            </td>
                                                                            <td align="left">
                                                                                <div>
                                                                                    <label title="No Transaksi" style="font-size: small; font-weight: bold">
                                                                                        <%#: Eval("ChargesNo")%></label></div>
                                                                                <div>
                                                                                    <label title="Tanggal Transaksi" style="font-size: smaller">
                                                                                        <%#: Eval("ChargesDateTimeInString")%></label></div>
                                                                                <div>
                                                                                    <label title="Unit Pelayanan" style="font-size: smaller">
                                                                                        <%#: Eval("ChargesUnit")%></label></div>
                                                                                <div>
                                                                                    <label style="font-size: x-small">
                                                                                        <%=GetLabel("Status Transaksi : ") %></label>
                                                                                    <label title="Status Transaksi" style="font-size: smaller">
                                                                                        <%#: Eval("TransactionStatus")%></label></div>
                                                                            </td>
                                                                            <td align="left">
                                                                                <div>
                                                                                <label title="Kode Item" style="font-size: smaller; font-weight: bold">
                                                                                    (<%#: Eval("ItemCode")%>)</label>
                                                                                <label title="Nama Item" style="font-size: small; font-weight: bold">
                                                                                    <%#: Eval("ItemName1")%></label></div>
                                                                                <div>
                                                                                    <label style="font-size: x-small">
                                                                                        <%=GetLabel("Jumlah = ") %></label>
                                                                                    <label title="Jumlah Transaksi" style="font-size: small">
                                                                                        <%#: Eval("ChargedQuantity")%></label>
                                                                                    <label title="Item Unit" style="font-size: small">
                                                                                        <%#: Eval("ItemUnit")%></label></div>
                                                                                    <label title="Charges Paramedic Name" style="font-size: small">
                                                                                        <%#: Eval("ChargesParamedicName")%></label></div>
                                                                            </td>
                                                                            <td align="right">
                                                                                <div>
                                                                                    <label title="Harga Satuan" style="font-size: small">
                                                                                        <%#: Eval("cfTariffAmountInString")%></label></div>
                                                                            </td>
                                                                            <td align="right">
                                                                                <div>
                                                                                    <label title="Jumlah Transaksi" style="font-size: small">
                                                                                        <%#: Eval("cfLineAmountInString")%></label></div>
                                                                            </td>
                                                                            <td align="center">
                                                                                <div>
                                                                                    <label title="Dibuat Oleh" style="font-size: small">
                                                                                        <%#: Eval("ChargesCreatedBy")%></label></div>
                                                                                <div>
                                                                                    <label title="Dibuat Pada" style="font-size: small">
                                                                                        <%#: Eval("cfChargesCreatedDateTimeInString")%></label></div>
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                </asp:ListView>
                                                            </table>
                                                        </asp:Panel>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dxcp:ASPxCallbackPanel>
                                        </td>
                                    </tr>
                                    <tr id="trLine2" runat="server">
                                        <td colspan="2">
                                            <hr style="padding: 0 0 0 0; margin: 0 0 0 0;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dxcp:ASPxCallbackPanel ID="cbpViewPatientBill" runat="server" Width="100%" ClientInstanceName="cbpViewPatientBill"
                                                ShowLoadingPanel="false" OnCallback="cbpViewPatientBill_Callback">
                                                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent6" runat="server">
                                                        <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGrid" Style="height: 200px;
                                                            overflow-y: auto">
                                                            <table class="grdViewPatientBill grdSelected" cellspacing="0" width="100%" rules="all">
                                                                <tr>
                                                                    <th class="keyField">
                                                                    </th>
                                                                    <th align="left" style="font-size: smaller">
                                                                        <%=GetLabel("Info Tagihan") %>
                                                                    </th>
                                                                    <th align="right" style="width: 100px; font-size: smaller">
                                                                        <%=GetLabel("Tagihan Pasien") %>
                                                                    </th>
                                                                    <th align="right" style="width: 100px; font-size: smaller">
                                                                        <%=GetLabel("Tagihan Instansi") %>
                                                                    </th>
                                                                    <th align="right" style="width: 100px; font-size: smaller">
                                                                        <%=GetLabel("Total Tagihan") %>
                                                                    </th>
                                                                    <th align="center" style="width: 190px; font-size: smaller">
                                                                        <%=GetLabel("Info Dibuat") %>
                                                                    </th>
                                                                </tr>
                                                                <asp:ListView runat="server" ID="lvwViewPatientBill">
                                                                    <EmptyDataTemplate>
                                                                        <tr class="trEmpty">
                                                                            <td colspan="15">
                                                                                <%=GetLabel("---Tidak Ada Data---") %>
                                                                            </td>
                                                                        </tr>
                                                                    </EmptyDataTemplate>
                                                                    <ItemTemplate>
                                                                        <tr>
                                                                            <td class="keyField">
                                                                                <%#: Eval("PatientBillingID")%>
                                                                            </td>
                                                                            <td align="left">
                                                                                <div>
                                                                                    <label title="No Tagihan" style="font-size: small; font-weight: bold">
                                                                                        <%#: Eval("PatientBillingNo")%></label></div>
                                                                                <div>
                                                                                    <label title="Tanggal Tagihan" style="font-size: smaller">
                                                                                        <%#: Eval("BillingDateTimeInString")%></label></div>
                                                                            </td>
                                                                            <td align="right">
                                                                                <div>
                                                                                    <label title="Total Tagihan Pasien" style="font-size: small">
                                                                                        <%#: Eval("TotalPatientAmountInString")%></label></div>
                                                                            </td>
                                                                            <td align="right">
                                                                                <div>
                                                                                    <label title="Total Tagihan Instansi" style="font-size: small">
                                                                                        <%#: Eval("TotalPayerAmountInString")%></label></div>
                                                                            </td>
                                                                            <td align="right">
                                                                                <div>
                                                                                    <label title="Nilai Tagihan" style="font-size: small">
                                                                                        <%#: Eval("TotalAmountInString")%></label></div>
                                                                            </td>
                                                                            <td align="center">
                                                                                <div>
                                                                                    <label title="Dibuat Oleh" style="font-size: small">
                                                                                        <%#: Eval("CreatedByFullName")%></label></div>
                                                                                <div>
                                                                                    <label title="Dibuat Pada" style="font-size: small">
                                                                                        <%#: Eval("cfBillCreatedDateTimeInString")%></label></div>
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                </asp:ListView>
                                                            </table>
                                                        </asp:Panel>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dxcp:ASPxCallbackPanel>
                                        </td>
                                    </tr>
                                    <tr id="trLine3" runat="server">
                                        <td colspan="2">
                                            <hr style="padding: 0 0 0 0; margin: 0 0 0 0;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dxcp:ASPxCallbackPanel ID="cbpViewPatientPayment" runat="server" Width="100%" ClientInstanceName="cbpViewPatientPayment"
                                                ShowLoadingPanel="false" OnCallback="cbpViewPatientPayment_Callback">
                                                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent4" runat="server">
                                                        <asp:Panel runat="server" ID="Panel3" CssClass="pnlContainerGrid" Style="height: 200px;
                                                            overflow-y: auto">
                                                            <table class="grdViewPatientPayment grdSelected" cellspacing="0" width="100%" rules="all">
                                                                <tr>
                                                                    <th class="keyField">
                                                                    </th>
                                                                    <th align="left" style="width: 160px; font-size: smaller">
                                                                        <%=GetLabel("No Bayar") %>
                                                                    </th>
                                                                    <th align="left" style="width: 120px; font-size: smaller">
                                                                        <%=GetLabel("Jenis Bayar") %>
                                                                    </th>
                                                                    <th align="left" style="font-size: smaller">
                                                                        <%=GetLabel("Informasi Instansi") %>
                                                                    </th>
                                                                    <th align="right" style="width: 150px; font-size: smaller">
                                                                        <%=GetLabel("Nilai Bayar") %>
                                                                    </th>
                                                                    <th align="center" style="width: 190px; font-size: smaller">
                                                                        <%=GetLabel("Info Dibuat") %>
                                                                    </th>
                                                                </tr>
                                                                <asp:ListView runat="server" ID="lvwViewPatientPayment">
                                                                    <EmptyDataTemplate>
                                                                        <tr class="trEmpty">
                                                                            <td colspan="15">
                                                                                <%=GetLabel("---Tidak Ada Data---") %>
                                                                            </td>
                                                                        </tr>
                                                                    </EmptyDataTemplate>
                                                                    <ItemTemplate>
                                                                        <tr>
                                                                            <td class="keyField">
                                                                                <%#: Eval("PaymentID")%>
                                                                            </td>
                                                                            <td align="left">
                                                                                <div>
                                                                                    <label title="No Bayar" style="font-size: small; font-weight: bold">
                                                                                        <%#: Eval("PaymentNo")%></label></div>
                                                                                <div>
                                                                                    <label title="Tgl Bayar" style="font-size: small">
                                                                                        <%#: Eval("PaymentDateTimeInString")%></label></div>
                                                                            </td>
                                                                            <td align="left">
                                                                                <div>
                                                                                    <label title="Jenis Bayar" style="font-size: small">
                                                                                        <%#: Eval("PaymentType")%></label></div>
                                                                            </td>
                                                                            <td align="left">
                                                                                <div>
                                                                                    <label title="Informasi Instansi" style="font-size: small">
                                                                                        <%#: Eval("BusinessPartnerInformation")%></label></div>
                                                                            </td>
                                                                            <td align="right">
                                                                                <div>
                                                                                    <label title="Nilai Bayar" style="font-size: small">
                                                                                        <%#: Eval("ReceiveAmountInString2")%></label></div>
                                                                            </td>
                                                                            <td align="center">
                                                                                <div>
                                                                                    <label title="Dibuat Oleh" style="font-size: small">
                                                                                        <%#: Eval("CreatedByUser")%></label></div>
                                                                                <div>
                                                                                    <label title="Dibuat Pada" style="font-size: small">
                                                                                        <%#: Eval("cfCreatedDateTimeInString")%></label></div>
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                </asp:ListView>
                                                            </table>
                                                        </asp:Panel>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dxcp:ASPxCallbackPanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
