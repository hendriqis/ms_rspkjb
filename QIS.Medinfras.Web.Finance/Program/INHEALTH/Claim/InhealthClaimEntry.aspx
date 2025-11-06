<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="InhealthClaimEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.InhealthClaimEntry" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Refresh")%></div>
    </li>
    <li id="btnSimpanTindakan" crudmode="R" runat="server" style="display:none">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Simpan Tindakan")%></div>
    </li>
    <li id="btnHapusTindakan" crudmode="R" runat="server" style="display:none">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/close.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Hapus Tindakan")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetMenuCaption())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtSearchRegistrationDateFrom.ClientID %>');
            $('#<%=txtSearchRegistrationDateFrom.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
            setDatePicker('<%=txtSearchRegistrationDateTo.ClientID %>');
            $('#<%=txtSearchRegistrationDateTo.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            var grd = new customGridView();
            grd.init('<%=grdView.ClientID %>', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');

            $('#<%=grdView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
                if ($(this).attr('class') == 'selected') {
                    $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                    $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
                    cbpViewDt.PerformCallback('refresh');
                }
            });

            reinitButton();
        });

//        function onCboSentStatusChanged() {
////            reinitButton();
////            cbpView.PerformCallback('refresh');
//        }

        $('.txtCurrency').each(function () {
            $(this).trigger('changeValue');
        });

        $('#<%=btnRefresh.ClientID %>').live('click', function () {
            cbpView.PerformCallback('refresh');
        });

        $('#<%=btnSimpanTindakan.ClientID %>').live('click', function () {
            if ($('#<%=hdnRegistrationID.ClientID %>').val() != '') {
                if (cboSentStatus.GetValue() == "1" || cboSentStatus.GetValue() == "3") {
                    getCheckedMember();
                    cbpView.PerformCallback('send|' + $('#<%=hdnSelectedMember.ClientID %>').val());
                }
                else {
                    displayMessageBox("WARNING", "Harap refresh terlebih dahulu");    
                }
            }
            else {
                displayMessageBox("WARNING", "Harap pilih nomor registrasi terlebih dahulu");
            }
        });

        $('#<%=btnHapusTindakan.ClientID %>').live('click', function () {
            if ($('#<%=hdnRegistrationID.ClientID %>').val() != '') {
                if (cboSentStatus.GetValue() == "2") {
                    getCheckedMember();
                    cbpView.PerformCallback('delete|' + $('#<%=hdnSelectedMember.ClientID %>').val());
                }
                else {
                    displayMessageBox("WARNING", "Harap refresh terlebih dahulu");
                }
            }
            else {
                displayMessageBox("WARNING", "Harap pilih nomor registrasi terlebih dahulu");
            }
        });

        $('.txtCoverageAmount').live('change', function () {
            $tr = $(this).closest('tr');
            var realCostAmount = parseFloat($tr.find('.hdnRealCostAmount').val());
            var coverageAmount = parseFloat($tr.find('.txtCoverageAmount').val());
            var patientAmount = parseFloat($tr.find('.txtPatientAmount').val());
            var differenceAmount = realCostAmount - coverageAmount - patientAmount;

            $tr.find('.txtDifferenceAmount').val(differenceAmount).trigger('changeValue');
        });

        $('.txtPatientAmount').live('change', function () {
            $tr = $(this).closest('tr');
            var realCostAmount = parseFloat($tr.find('.hdnRealCostAmount').val());
            var coverageAmount = parseFloat($tr.find('.txtCoverageAmount').val());
            var patientAmount = parseFloat($tr.find('.txtPatientAmount').val());
            var differenceAmount = realCostAmount - coverageAmount - patientAmount;

            $tr.find('.txtDifferenceAmount').val(differenceAmount).trigger('changeValue');
        });

        $('.btnSave').live('click', function () {
            $tr = $(this).closest('tr');
            var historyID = $tr.find('.keyField').html();
            if (historyID == 0) {
                var realCostAmount = parseFloat($tr.find('.hdnRealCostAmount').val());
                var coverageAmount = parseFloat($tr.find('.txtCoverageAmount').val());
                var occupiedAmount = parseFloat($tr.find('.txtOccupiedAmount').val());
                var patientAmount = parseFloat($tr.find('.txtPatientAmount').val());
                var differenceAmount = parseFloat($tr.find('.txtDifferenceAmount').attr('hiddenVal'));

                var param = 'save|' + historyID + '|' + realCostAmount + '|' + coverageAmount + '|' + occupiedAmount + '|' + patientAmount + '|' + differenceAmount;

                cbpViewDt.PerformCallback(param);
            }
        });

        function onAfterCustomClickSuccess(type) {
            cbpView.PerformCallback('refresh');
        }

        function onRefreshControl(filterExpression) {
            $('#<%=hdnFilterExpression.ClientID %>').val(filterExpression);
            cbpView.PerformCallback('refresh');
        }

        function reinitButton() {
            if (cboSentStatus.GetValue() == "1" || cboSentStatus.GetValue() == "2" || cboSentStatus.GetValue() == "3") {
                if (cboSentStatus.GetValue() == "1" || cboSentStatus.GetValue() == "3") {
                    $('#<%=btnHapusTindakan.ClientID %>').attr("style", "display:none");
                    $('#<%=btnSimpanTindakan.ClientID %>').removeAttr("style");

                    $('#trCancelReason').hide();
                }
                else if (cboSentStatus.GetValue() == "2") {
                    $('#<%=btnSimpanTindakan.ClientID %>').attr("style", "display:none");
                    $('#<%=btnHapusTindakan.ClientID %>').removeAttr("style");

                    $('#trCancelReason').show();
                }
            }
            else {
                $('#<%=btnSimpanTindakan.ClientID %>').attr("style", "display:none");
                $('#trCancelReason').hide();
                $('#<%=btnHapusTindakan.ClientID %>').attr("style", "display:none");
            }
        }


        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            reinitButton();
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else if (param[0] == "send") {
                if (param[1] == "success") {
                    ShowSnackbarSuccess(param[2]);
                }
                else {
                    ShowSnackbarError(param[2]);
                }
                cbpView.PerformCallback('refresh');
            }
            else if (param[0] == "delete") {
                if (param[1] == "success") {
                    ShowSnackbarSuccess(param[2]);
                }
                else {
                    ShowSnackbarError(param[2]);
                }
                cbpView.PerformCallback('refresh');
            }
            else {
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
            }
        }
        //#endregion

        function onBeforeLoadRightPanelContent(code) {
            var regID = $('#<%:hdnID.ClientID %>').val();

        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var regID = $('#<%:hdnID.ClientID %>').val();

            if (code != "") {
                if (regID != "" && regID != null) {
                    filterExpression.text = "RegistrationID = " + regID;
                    return true;
                } else {
                    displayErrorMessageBox("Silahkan Coba Lagi", "Pilih nomor registrasi terlebih dahulu.");
                    errMessage.text = "Belum pilih nomor registrasi.";
                    return false;
                }
            } else {
                errMessage.text = "No data to display.";
                return false;
            }
        }
        
        function onAfterCustomClickSuccess(type, paramUrl) {
            var url = ResolveUrl(paramUrl);
            showLoadingPanel();
            window.location.href = url;
        }

        //#region Item Master
        function onGetitemMasterFilterExpression() {
            var filterExpression = "IsDeleted = 0 AND GCItemType NOT IN ('X001^002','X001^003','X001^008','X001^009')";
            return filterExpression;
        }

        $('#lblItem.lblLink').live('click', function () {
            openSearchDialog('vItem', onGetitemMasterFilterExpression(), function (value) {
                $('#<%=txtItemCode.ClientID %>').val(value);
                ontxtItemCodeChanged(value);
            });
        });

        $('#<%=txtItemCode.ClientID %>').live('change', function () {
            ontxtItemCodeChanged($(this).val());
        });

        function ontxtItemCodeChanged(value) {
            var filterExpression = onGetitemMasterFilterExpression() + " AND ItemCode = '" + value + "'";
            Methods.getObject('GetvItemMasterList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnItemID.ClientID %>').val(result.ItemID);
                    $('#<%=txtItemName.ClientID %>').val(result.ItemName1);
                }
                else {
                    $('#<%=hdnItemID.ClientID %>').val('');
                    $('#<%=txtItemCode.ClientID %>').val('');
                    $('#<%=txtItemName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Item Master
        function onGetRegistrationFilterExpression() {
            var fromDate = $('#<%=txtSearchRegistrationDateFrom.ClientID %>').val();
            var toDate = $('#<%=txtSearchRegistrationDateTo.ClientID %>').val();
            var year = fromDate.substring(6, 10);
            var month = fromDate.substring(3, 5);
            var day = fromDate.substring(0, 2);
            var fromDate112 = year + month + day;

            var toYear = toDate.substring(6, 10);
            var toMonth = toDate.substring(3, 5);
            var toDay = toDate.substring(0, 2);

            var toDate112 = toYear + toMonth + toDay;

            var filterExpression = "(RegistrationDate BETWEEN '" + fromDate112 + "' AND '" + toDate112 + "') AND GCRegistrationStatus != 'X020^006' AND BusinessPartnerID IN (SELECT BusinessPartnerID FROM Customer WHERE GCCustomerType = (SELECT ISNULL(spd.ParameterValue,0) FROM SettingParameterDt spd WITH(NOLOCK) WHERE spd.ParameterCode = 'FN0073'))";
            return filterExpression;
        }

        $('#lblRegistration.lblLink').live('click', function () {
            openSearchDialog('registration5', onGetRegistrationFilterExpression(), function (value) {
                $('#<%=txtRegistrationNo.ClientID %>').val(value);
                ontxtRegistrationNoChanged(value);
            });
        });

        $('#<%=txtRegistrationNo.ClientID %>').live('change', function () {
            ontxtRegistrationNoChanged($(this).val());
        });

        function ontxtRegistrationNoChanged(value) {
            var filterExpression = onGetRegistrationFilterExpression() + " AND RegistrationNo = '" + value + "'";
            Methods.getObject('GetvRegistration12List', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnRegistrationID.ClientID %>').val(result.RegistrationID);
                    $('#<%=txtRegistrationNo.ClientID %>').val(result.RegistrationNo);
                }
                else {
                    $('#<%=hdnRegistrationID.ClientID %>').val('');
                    $('#<%=txtRegistrationNo.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Check Box
        $('#chkSelectAll').die('change');
        $('#chkSelectAll').live('change', function () {
            var isChecked = $(this).is(":checked");
            $('.chkIsSelected input').each(function () {
                $(this).prop('checked', isChecked);
                $(this).change();
            });
        });

        function getCheckedMember() {
            var lstSelectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().split(',');
            $('#<%=grdView.ClientID %> .chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    var key = $(this).closest('tr').find('.keyField').html();
                    var idx = lstSelectedMember.indexOf(key);
                    if (idx < 0) {
                        lstSelectedMember.push(key);
                    }
                }
                else {
                    var key = $(this).closest('tr').find('.keyField').html();
                    var idx = lstSelectedMember.indexOf(key);
                    if (idx > -1) {
                        lstSelectedMember.splice(idx, 1);
                    }
                }
            });
            $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
        }
    </script>
    <input type="hidden" value="" id="hdnRequestID" runat="server" />
    <input type="hidden" value="" id="hdnParam" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnRecordFilterExpression" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpression" runat="server" />
    <input type="hidden" value="" id="hdnSelectedRealCostAmount" runat="server" />
    <input type="hidden" value="" id="hdnSelectedCoverageAmount" runat="server" />
    <input type="hidden" value="" id="hdnSelectedOccupiedAmount" runat="server" />
    <input type="hidden" value="" id="hdnSelectedPatientAmount" runat="server" />
    <input type="hidden" value="" id="hdnSelectedDifferenceAmount" runat="server" />
    <input type="hidden" value="" id="hdnJenisPelayanan" runat="server" />
    <input type="hidden" value="" id="hdnSelectedMember" runat="server" />
    <div style="position: relative">
        <table style="width: 100%">
            <tr>
                <td>
                    <table class="tblContentArea" style="width: 100%">
                        <colgroup>
                            <col style="width: 10%" />
                            <col />
                        </colgroup>
                        <tr id="trTanggal" runat="server">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tanggal Registrasi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSearchRegistrationDateFrom" Width="130px" runat="server" CssClass="datepicker" />
                                <%=GetLabel("s/d")%>
                                <asp:TextBox ID="txtSearchRegistrationDateTo" Width="130px" runat="server" CssClass="datepicker" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="lblMandatory lblLink" id="lblRegistration">
                                    <%=GetLabel("No. Registrasi")%></label>
                            </td>
                            <td>
                                <input type="hidden" value="" runat="server" id="hdnRegistrationID" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 100px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtRegistrationNo" Width="300px" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="lblNormal lblLink" id="lblItem">
                                    <%=GetLabel("Item Pelayanan")%></label>
                            </td>
                            <td>
                                <input type="hidden" value="" runat="server" id="hdnItemID" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 100px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtItemCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtItemName" Width="200px" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Status Kirim")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboSentStatus" ClientInstanceName="cboSentStatus" Width="300px" runat="server">
                                    <%--<ClientSideEvents ValueChanged="function(s,e){ onCboSentStatusChanged(); }" />--%>
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr id="trCancelReason">
                            <td>
                                <label class="lblNormal">
                                    <%=GetLabel("Alasan Batal")%></label>
                            </td>
                            <td>
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 100px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtCancelReason" Width="300px" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table style="width: 100%">
                        <colgroup>
                            <col style="width: 100%" />
                        </colgroup>
                        <tr>
                            <td style="vertical-align: top">
                                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                                    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent1" runat="server">
                                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridProcessList">
                                                <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                <input id="chkSelectAll" type="checkbox" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-Width="300px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                            <HeaderTemplate>
                                                                <%=GetLabel("No. Registrasi") %>
                                                                <br />
                                                                <%=GetLabel("No. SJP") %>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <b><%#:Eval("RegistrationNo") %> (<%#:Eval("DepartmentID") %>)</b>
                                                                <br />
                                                                <b>No SJP : <%#:Eval("NoSJP") %></b>
                                                                <br />
                                                                <i><%= GetLabel("Registrasi Asal : ")%>
                                                                        <%#:Eval("LinkedRegistrationNo").ToString() != "" ? Eval("LinkedRegistrationNo") : "" %></i>
                                                                <br />
                                                                <i><%= GetLabel("SJP Asal : ")%>
                                                                        <%#:Eval("LinkedRegistrationNo").ToString() != "" ? Eval("LinkedNoSJP") : "" %></i>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <%--<asp:BoundField DataField="PatientBillingNo" HeaderText="No. Tagihan" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
                                                        <asp:BoundField DataField="TransactionDateTimeInString" HeaderText="Tgl Transaksi" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
                                                        <asp:BoundField DataField="TransactionNo" HeaderText="No. Transaksi" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>--%>
                                                        <asp:TemplateField HeaderStyle-Width="250px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                            <HeaderTemplate>
                                                                <%=GetLabel("Transaksi") %>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <b>No Tagihan : <%#:Eval("PatientBillingNo") %></b>
                                                                <br />
                                                                <b>No Transaksi : <%#:Eval("TransactionNo") %></b>
                                                                <br />
                                                                <%#:Eval("TransactionDateTimeInString") %>
                                                                <br />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <%--<asp:BoundField DataField="ItemName1" HeaderText="Item Pelayanan" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>--%>
                                                        <asp:TemplateField HeaderStyle-Width="400px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                            <HeaderTemplate>
                                                                <%=GetLabel("Item Pelayanan") %>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <%#:Eval("ItemName1") %> (<%#:Eval("ItemCode") %>)
                                                                <br />
                                                                <i>
                                                                    <%= GetLabel("Kode Inhealth : ")%></i> <b>
                                                                        <%#:Eval("DepartmentID").ToString() == "OUTPATIENT" ? Eval("InhealthKodeJenPelRajal") : Eval("InhealthKodeJenPelRanap") %></b>
                                                                <br />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <%--<asp:BoundField DataField="ChargesParamedicName" HeaderText="Dokter" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>--%>
                                                        <asp:TemplateField HeaderStyle-Width="400px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                            <HeaderTemplate>
                                                                <%=GetLabel("Dokter") %>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <%#:Eval("ChargesParamedicName") %>
                                                                <br />
                                                                <i>
                                                                    <%=GetLabel("Kode Inhealth : ")%></i> <b>
                                                                        <%#:Eval("InhealthReferenceInfo") %></b>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="cfPayerAmount" HeaderText="Biaya" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"/>
                                                        <asp:BoundField DataField="IsSentToInhealthInString" HeaderText="Status" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
                                                        <asp:BoundField DataField="SentToInhealthByName" HeaderText="Pengirim" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
                                                        <asp:BoundField DataField="SentToInhealthDateTimeInString" HeaderText="Tgl Kirim" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
                                                        <asp:BoundField DataField="UnsentToInhealthByName" HeaderText="Pengirim Pembatalan" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
                                                        <asp:BoundField DataField="UnsentToInhealthDateTimeInString" HeaderText="Tgl Kirim Pembatalan" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
                                                        <asp:BoundField DataField="UnsentToInhealthReason" HeaderText="Alasan Batal" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"/>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <%=GetLabel("No data to display.")%>
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
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
