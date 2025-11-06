<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="TemporaryClaim.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.TemporaryClaim" %>

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
    <li id="btnUploadDocument" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbnotes.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("E-Document")%></div>
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
            grd.init('<%=grdView.ClientID %>', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpViewTemp, 'paging');

            $('#<%=grdView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
                if ($(this).attr('class') == 'selected') {
                    $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                    $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
                    cbpViewDt.PerformCallback('refresh');
                }
            });
        });

        $('.txtCurrency').each(function () {
            $(this).trigger('changeValue');
        });

        $('#<%=btnRefresh.ClientID %>').live('click', function () {
            cbpViewTemp.PerformCallback('refresh');
        });

        //#region INACBGMaster
        $('.lblINACBGMaster.lblLink').live('click', function () {
            $td = $(this).parent();
            $tr = $(this).closest('tr');
            var filterExpression = "IsDeleted = 0";
            openSearchDialog('inacbgmaster', filterExpression, function (value) {
                onINACBGMasterSelected(value);
            });
        });

        function onINACBGMasterSelected(value) {
            var filterExpression = "IsDeleted = 0 AND ID = '" + value + "'";
            Methods.getObject('GetINACBGMasterList', filterExpression, function (result) {
                if (result != null) {
                    $td.find('.txtGrouperCodeClaim').val(result.GrouperCode);
                    $td.find('.txtGrouperTypeClaim').val(result.GrouperDescription);
                    $tr.find('.txtCoverageAmount').val(result.GrouperTariff).trigger('changeValue');
                }
                else {
                    $td.find('.txtGrouperCodeClaim').val("");
                    $td.find('.txtGrouperTypeClaim').val("");
                    $td.find('.txtCoverageAmount').val("0").trigger('changeValue');
                }
            });
        }
        //#endregion

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
                var grouperCodeClaim = ($tr.find('.txtGrouperCodeClaim').val());
                var grouperTypeClaim = ($tr.find('.txtGrouperTypeClaim').val());
                var realCostAmount = parseFloat($tr.find('.hdnRealCostAmount').val().replace(/,/g, ''));
                var coverageAmount = parseFloat($tr.find('.txtCoverageAmount').val().replace(/,/g, ''));
                var occupiedAmount = parseFloat($tr.find('.txtOccupiedAmount').val().replace(/,/g, ''));
                var patientAmount = parseFloat("0");
                var differenceAmount = parseFloat(realCostAmount - coverageAmount - patientAmount);

                var param = 'save|' + historyID + '|' + grouperCodeClaim + '|' + grouperTypeClaim + '|' + realCostAmount + '|' + coverageAmount + '|' + occupiedAmount + '|' + patientAmount + '|' + differenceAmount;

                cbpViewDt.PerformCallback(param);
            }
        });

        function onAfterCustomClickSuccess(type) {
            cbpViewTemp.PerformCallback('refresh');
        }

        function onRefreshControl(filterExpression) {
            $('#<%=hdnFilterExpression.ClientID %>').val(filterExpression);
            cbpViewTemp.PerformCallback('refresh');
        }

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
                if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
                    cbpViewTemp.PerformCallback('refresh');
                }
            }, 0);
        }

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpViewTemp.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewTempEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpViewTemp.PerformCallback('changepage|' + page);
                });
            }
            else {
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
            }
        }
        //#endregion

        //#region Paging Dt
        function onCbpViewDtEndCallback(s) {
            $('#containerImgLoadingViewDt').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdViewDt.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt"), pageCount, function (page) {
                    cbpViewDt.PerformCallback('changepage|' + page);
                });
            }
            else if (param[0] == 'save') {
                if (param[1] == 'fail') {
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                }

                cbpViewDt.PerformCallback('refresh');
            }
            else {
                $('#<%=grdViewDt.ClientID %> tr:eq(1)').click();
            }
        }
        //#endregion

        function onBeforeLoadRightPanelContent(code) {
            var regID = $('#<%:hdnID.ClientID %>').val();
            if (code == 'uploadDocument' || code == 'infoTransactionParameter' || code == 'infoTransactionDetailParameter' || code == 'infoTransactionParameterFromBilling' || code == 'infoTransactionDetailParameterFromBilling' || code == 'infoDiagnosticResult') {
                if (regID != "" && regID != null) {
                    return regID;
                } else {
                    displayErrorMessageBox("Silahkan Coba Lagi", "Pilih nomor registrasi terlebih dahulu.");
                    return false;
                }
            }
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

        $('#<%=btnUploadDocument.ClientID %>').live('click', function () {
            if ($('#<%=hdnID.ClientID %>').val() != "") {
                onCustomButtonClick('upload');
            }
            else showToast('Warning', 'Pilih nomor registrasi terlebih dahulu.');
        });

        function onAfterCustomClickSuccess(type, paramUrl) {
            var url = ResolveUrl(paramUrl);
            showLoadingPanel();
            window.location.href = url;
        }

    </script>
    <input type="hidden" value="" id="hdnRequestID" runat="server" />
    <input type="hidden" value="" id="hdnParam" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnRecordFilterExpression" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpression" runat="server" />
    <div style="position: relative">
        <table style="width: 100%">
            <tr>
                <td>
                    <table class="tblContentArea" style="width: 100%">
                        <colgroup>
                            <col style="width: 150px" />
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
                        <tr id="trCustomerType" runat="server">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Jenis Instansi")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboCustomerType" ClientInstanceName="cboCustomerType" Width="150px"
                                    runat="server">
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr id="trStatusOrder" runat="server">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Status")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboOrderStatus" ClientInstanceName="cboOrderStatus" Width="150px"
                                    runat="server">
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Quick Filter")%></label>
                            </td>
                            <td>
                                <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                    Width="400px" Watermark="Search">
                                    <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                    <IntellisenseHints>
                                        <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                        <qis:QISIntellisenseHint Text="No RM" FieldName="MedicalNo" />
                                        <qis:QISIntellisenseHint Text="No SEP" FieldName="NoSEP" />
                                        <qis:QISIntellisenseHint Text="No Kartu BPJS" FieldName="NHSRegistrationNo" />
                                        <qis:QISIntellisenseHint Text="No Registrasi" FieldName="RegistrationNo" />
                                    </IntellisenseHints>
                                </qis:QISIntellisenseTextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                            <col style="width: 70%" />
                        </colgroup>
                        <tr>
                            <td style="vertical-align: top">
                                <dxcp:ASPxCallbackPanel ID="cbpViewTemp" runat="server" Width="100%" ClientInstanceName="cbpViewTemp"
                                    ShowLoadingPanel="false" OnCallback="cbpViewTemp_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewTempEndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent1" runat="server">
                                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridProcessList">
                                                <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:BoundField DataField="RegistrationID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:TemplateField HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                            <HeaderTemplate>
                                                                <%=GetLabel("Data Registrasi") %>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <i>
                                                                    <%=GetLabel("Tanggal-Jam SEP : ")%></i> <b>
                                                                        <%#:Eval("cfSEPDateTimeInString") %></b>
                                                                <h2>
                                                                    <%#:Eval("cfSEPNo") %></h2>
                                                                <i>
                                                                    <%=GetLabel("Pasien : ")%></i> <b>
                                                                        <%#:Eval("cfPatientName") %></b>
                                                                <br />
                                                                <i>
                                                                    <%=GetLabel("No Kartu BPJS : ")%></i> <b>
                                                                        <%#:Eval("NHSRegistrationNo") %></b>
                                                                <br />
                                                                <i>
                                                                    <%=GetLabel("No Registrasi : ")%></i> <b>
                                                                        <%#:Eval("RegistrationNo") %></b> (<i><%#:Eval("RegistrationStatus") %></i>)
                                                                <br />
                                                                <i>
                                                                    <%=GetLabel("Unit Pelayanan : ")%></i> <b>
                                                                        <%#:Eval("ServiceUnitName") %></b>
                                                                <br />
                                                                <i>
                                                                    <%=GetLabel("Penjamin Bayar : ")%></i> <b>
                                                                        <%#:Eval("BusinessPartnerName") %></b>
                                                                <br />
                                                                <i>
                                                                    <%=GetLabel("Diagnosa (Dokter) : ")%></i> <b>
                                                                        <%#:Eval("cfDiagnoseInfo") %></b>
                                                                <br />
                                                                <i>
                                                                    <%=GetLabel("Diagnosa (RM) : ")%></i> <b>
                                                                        <%#:Eval("cfFinalDiagnosisInfo") %></b>
                                                                <br />
                                                                <i>
                                                                    <%=GetLabel("Diagnosa (Klaim) : ")%></i> <b>
                                                                        <%#:Eval("cfClaimDiagnosisInfo") %></b>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
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
                            <td style="vertical-align: top; width: 100%">
                                <div style="position: relative;">
                                    <dxcp:ASPxCallbackPanel ID="cbpViewDt" runat="server" Width="100%" ClientInstanceName="cbpViewDt"
                                        OnCallback="cbpViewDt_Callback" ShowLoadingPanel="false">
                                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt').show(); }"
                                            EndCallback="function(s,e){ onCbpViewDtEndCallback(s); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent2" runat="server">
                                                <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridProcessList">
                                                    <asp:GridView ID="grdViewDt" runat="server" CssClass="grdSelected grdPatientPage"
                                                        OnRowDataBound="grdViewDt_RowDataBound" AutoGenerateColumns="false" ShowHeaderWhenEmpty="false"
                                                        EmptyDataRowStyle-CssClass="trEmpty">
                                                        <Columns>
                                                            <asp:BoundField DataField="HistoryID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                            <asp:TemplateField HeaderStyle-Width="150px" HeaderText="Info Log" ItemStyle-HorizontalAlign="Center"
                                                                HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <div style="font-size: small; font-style: italic">
                                                                        <%#:Eval("CodingByName") %></div>
                                                                    <div style="font-size: x-small">
                                                                        <%#:Eval("cfCodingDateInString") %></div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-Width="250px" HeaderText="Info Grouper" ItemStyle-HorizontalAlign="Left"
                                                                HeaderStyle-HorizontalAlign="Left">
                                                                <ItemTemplate>
                                                                    <label runat="server" id="lblINACBGMaster" class="lblLink lblINACBGMaster">
                                                                        <%=GetLabel("Pilih Master INACBG")%></label>
                                                                    <input type="text" runat="server" value='<%#:Eval("GrouperCodeClaim") %>' id="txtGrouperCodeClaim"
                                                                        class="txtGrouperCodeClaim" style="width: 100%" placeholder="GrouperCodeClaim" />
                                                                    <input type="text" runat="server" value='<%#:Eval("GrouperTypeClaim") %>' id="txtGrouperTypeClaim"
                                                                        class="txtGrouperTypeClaim" style="width: 100%" placeholder="GrouperDescriptionClaim" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
                                                                <HeaderTemplate>
                                                                    <%=GetLabel("Nilai") %>
                                                                    <br />
                                                                    <%=GetLabel("Transaksi") %>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <input type="hidden" class="hdnItemIndex" value='<%#: Container.DataItemIndex %>' />
                                                                    <input type="hidden" class="hdnRealCostAmount" value='<%#:Eval("RealCostAmount") %>' />
                                                                    <input type="text" runat="server" value='<%#:Eval("cfRealCostAmountInString") %>'
                                                                        id="txtRealCostAmount" class="txtRealCostAmount txtCurrency" style="width: 100%" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
                                                                <HeaderTemplate>
                                                                    <%=GetLabel("INACBG") %>
                                                                    <br />
                                                                    <%=GetLabel("Hak Pasien") %>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <input type="text" runat="server" value='<%#:Eval("cfCoverageAmountInString") %>'
                                                                        id="txtCoverageAmount" class="txtCoverageAmount txtCurrency" style="width: 100%" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
                                                                <HeaderTemplate>
                                                                    <%=GetLabel("INACBG") %>
                                                                    <br />
                                                                    <%=GetLabel("Ditempati") %>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <input type="text" runat="server" value='<%#:Eval("cfOccupiedAmountInString") %>'
                                                                        id="txtOccupiedAmount" class="txtOccupiedAmount txtCurrency" style="width: 100%" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                                Visible="false">
                                                                <HeaderTemplate>
                                                                    <%=GetLabel("Nilai") %>
                                                                    <br />
                                                                    <%=GetLabel("Dibayar Pasien") %>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <input type="text" runat="server" value='<%#:Eval("cfPatientAmountInString") %>'
                                                                        id="txtPatientAmount" class="txtPatientAmount txtCurrency" style="width: 100%" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                                Visible="false">
                                                                <HeaderTemplate>
                                                                    <%=GetLabel("Nilai") %>
                                                                    <br />
                                                                    <%=GetLabel("Selisih") %>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <input type="text" runat="server" value='<%#:Eval("cfCountDifferenceInString") %>'
                                                                        id="txtDifferenceAmount" class="txtDifferenceAmount txtCurrency" style="width: 100%" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                                <HeaderTemplate>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <input type="button" id="btnSave" class="btnSave w3-button w3-blue" value="Simpan"
                                                                        runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            <%=GetLabel("No data to display.")%>
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dxcp:ASPxCallbackPanel>
                                    <div class="imgLoadingGrdView" id="containerImgLoadingViewDt">
                                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                    </div>
                                    <div class="containerPaging">
                                        <div class="wrapperPaging">
                                            <div id="pagingDt">
                                            </div>
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
