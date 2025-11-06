<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="RecipeHistory.aspx.cs" Inherits="QIS.Medinfras.Web.Pharmacy.Program.RecipeHistory" %>

<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dxpc" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetMenuCaption())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtDateFrom.ClientID %>');
            setDatePicker('<%=txtDateTo.ClientID %>');

            $('#<%=txtDateFrom.ClientID %>').change(function () {
                onRefreshGrid();
            });
            $('#<%=txtDateTo.ClientID %>').change(function () {
                onRefreshGrid();
            });

            var grd = new customGridView();
            grd.init('<%=grdView.ClientID %>', '<%=hdnParamID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');
            $('#<%=grdView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
                if ($(this).attr('class') == 'selected') {
                    $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                    $row = $(this).closest('tr');
                    var entity = rowToObject($row);
                    $('#<%=hdnPrescriptionOrderID.ClientID %>').val(entity.PrescriptionOrderID);
                    $('#<%=hdnChargesTransactionID.ClientID %>').val(entity.ChargesTransactionID);
                    $('#<%=hdnGCChargesTransactionStatus.ClientID %>').val(entity.GCChargesTransactionStatus);
                }
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();

        });

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        var currPage = parseInt('<%=CurrPage %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            }, null, currPage);
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();
                else
                    $('#<%=hdnID.ClientID %>').val('');

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        function onRefreshGrid() {
            $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
            cbpView.PerformCallback('refresh');
        }

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                onRefreshGrid();
            }, 0);
        }

        function onCboTypeValueChanged() {
            onRefreshGrid();
        }

        function onCboDepartmentValueChanged() {
            onRefreshGrid();
        }

        $('.lblDetail').die('click');
        $('.lblDetail').live('click', function () {
            $tr = $(this).closest('tr');
            var param = $tr.find('.keyField').html()
            var url = ResolveUrl("~/Program/Information/RecipeHistoryDetailCtl.ascx");
            openUserControlPopup(url, param, 'Recipe History Detail Information', 1100, 550);
        });

        $('.imgPrint').die('click');
        $('.imgPrint').live('click', function () {
            $tr = $(this).closest('tr');
            var param = $tr.find('.keyField').html()
            var url = ResolveUrl("~/Program/Information/PrintEticketCtl.ascx");
            openUserControlPopup(url, param, 'Recipe History Detail Information', 800, 600);
        });

        function clickView() {
            if ($('#<%=grdView.ClientID %> tr.selected').length > 0) {
                showLoadingPanel();
                $('#<%=hdnChargesTransactionID.ClientID %>').val($('#<%=grdView.ClientID %> tr.selected').find('.ChargesTransactionID').val());
            }
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var PrescriptionOrderID = $('#<%=hdnPrescriptionOrderID.ClientID %>').val();
            var ChargesTransactionID = $('#<%=hdnChargesTransactionID.ClientID %>').val();
            var GCChargesTransactionStatus = $('#<%=hdnGCChargesTransactionStatus.ClientID%>').val();

            if (PrescriptionOrderID == '' || PrescriptionOrderID == '0') {
                errMessage.text = 'Pilih transaksi resep terlebih dahulu.';
                return false;
            }
            else {
                if (code == 'PH-00018' || code == 'PH-00029' || code == 'PM-00214' || code == 'PH-00007' || code == 'PH-00035' || code == 'PH-00050') {
                    filterExpression.text = ChargesTransactionID;
                    return true;
                }
                else if (code == 'PH-00028' || code == 'PH-00038' || code == 'PH-00041' || code == 'PH-00044' || code == 'PH-00048' || code == 'PH-00067'
                        || code == 'PH-00068' || code == 'PH-00074' || code == 'PH-00075') {
                    filterExpression.text = PrescriptionOrderID;
                    return true;
                }
                else {
                    if (GCChargesTransactionStatus != Constant.TransactionStatus.OPEN && GCChargesTransactionStatus != Constant.TransactionStatus.VOID) {
                        if (code == 'PH-00003') {
                            filterExpression.text = PrescriptionOrderID;
                            return true;
                        }
                        else if (code == 'PH-00002' || code == 'PH-00005' || code == 'PH-00006' || code == 'PH-00010'
                                    || code == 'PH-00015' || code == 'PH-00017' || code == 'PH-00019'
                                    || code == 'PH-00021' || code == 'PH-00022' || code == 'PH-00024'
                                    || code == 'PH-00011' || code == 'PH-00012' || code == 'PH-00014'
                                    || code == 'PH-00031' || code == 'PM-00201' || code == 'PM-00236'
                                    || code == 'PM-002361' || code == 'PM-00239' || code == 'PM-00288'
                                    || code == 'PM-00413' || code == 'PH-00033' || code == 'PH-00034'
                                    || code == 'PH-00036' || code == 'PH-00040' || code == 'PH-00043'
                                    || code == 'PH-00047' || code == 'PH-00051' || code == 'PH-00052'
                                    || code == 'PH-00054' || code == 'PH-00087' || code == 'PH-00108') {
                            if (ChargesTransactionID == '' || ChargesTransactionID == '0') {
                                errMessage.text = 'Order Resep harus direalisasi terlebih dahulu.';
                            }
                            else {
                                filterExpression.text = ChargesTransactionID;
                                return true;
                            }
                        }
                        else if (code == 'PH-00023') {
                            filterExpression.text = PrescriptionOrderID;
                            return true;
                        }
                        else {
                            filterExpression.text = "PrescriptionOrderID = " + PrescriptionOrderID;

                            return true;
                        }
                    } else {
                        errMessage.text = 'Transaksi harus dipropose terlebih dahulu sebelum proses bisa dilakukan.';
                        return false;
                    }
                }
            }
        }


        $('#<%=chkIsIgnoreDate.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
                $('#<%=txtDateFrom.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtDateTo.ClientID %>').attr('readonly', 'readonly');
            }
            else
                setDatePicker('<%=txtDateFrom.ClientID %>');
            setDatePicker('<%=txtDateTo.ClientID %>');
            onRefreshGrid();
        });

        function onBeforeLoadRightPanelContent(code) {
            if (code == 'infoOriginalPrescription') {
                return $('#<%:hdnPrescriptionOrderID.ClientID %>').val();
            }
        }
    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnParamID" runat="server" />
    <input type="hidden" value="" id="hdnChargesTransactionID" runat="server" />
    <input type="hidden" value="" id="hdnPrescriptionOrderID" runat="server" />
    <input type="hidden" value="" id="hdnGCChargesTransactionStatus" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <table width="100%">
        <tr>
            <td>
                <div>
                    <table class="tblEntryContent" style="width: 600px;">
                        <colgroup>
                            <col style="width: 150px" />
                            <col style="width: 180px" />
                            <col style="width: 50px" />
                            <col style="width: 180px" />
                            <col style="width: 180px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Quick Filter")%></label>
                            </td>
                            <td colspan="4">
                                <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                    Width="100%" Watermark="Search">
                                    <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                    <IntellisenseHints>
                                        <qis:QISIntellisenseHint Text="Nama" FieldName="PatientName" />
                                        <qis:QISIntellisenseHint Text="No RM" FieldName="MedicalNo" />
                                        <qis:QISIntellisenseHint Text="No. Registrasi" FieldName="RegistrationNo" />
                                        <qis:QISIntellisenseHint Text="No. Transaksi" FieldName="ChargesTransactionNo" />
                                        <qis:QISIntellisenseHint Text="Instansi" FieldName="BusinessPartnerName" />
                                    </IntellisenseHints>
                                </qis:QISIntellisenseTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="lblNormal">
                                    <%=GetLabel("Periode")%></label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtDateFrom" Width="120px" CssClass="datepicker" />
                            </td>
                            <td>
                                s/d
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtDateTo" Width="120px" CssClass="datepicker" />
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsIgnoreDate" runat="server" Checked="false" /><%:GetLabel("Abaikan Tanggal")%>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){showLoadingPanel()}" EndCallback="function(s,e){onCbpViewEndCallback(s)}" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="overflow-y: scroll;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="2px">
                                                <ItemTemplate>
                                                    <input type="hidden" value="<%#:Eval("PrescriptionOrderID") %>" bindingfield="PrescriptionOrderID" />
                                                    <input type="hidden" value="<%#:Eval("ChargesTransactionID") %>" bindingfield="ChargesTransactionID" />
                                                    <input type="hidden" value="<%#:Eval("GCChargesTransactionStatus") %>" bindingfield="GCChargesTransactionStatus" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="PrescriptionOrderID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="200px">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Data Pasien") %>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <label class="lblNormal" style="font-weight: bold">
                                                        <%#:Eval("MedicalNo") %></label><br />
                                                    <label class="lblNormal">
                                                        <%#:Eval("PatientName") %></label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="170px">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Data Order Resep") %>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <label class="lblNormal" style="font-weight: bold">
                                                        <%#:Eval("PrescriptionOrderNo") %></label><br />
                                                    <label class="lblNormal">
                                                        <%#:Eval("cfPrescriptionDateInString") %></label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="170px">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Data Transaksi Resep") %>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <label class="lblNormal" style="font-weight: bold">
                                                        <%#:Eval("ChargesTransactionNo") %></label><br />
                                                    <label class="lblNormal">
                                                        <%#:Eval("cfChargesTransactionDateInString") %></label><br />
                                                    <label class="lblNormal">
                                                        <%#:Eval("TransactionStatus") %></label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="250px">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Data Kunjungan") %>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <label class="lblNormal" style="font-weight: bold">
                                                        <%#:Eval("RegistrationNo") %></label><br />
                                                    <label class="lblNormal">
                                                        <%#:Eval("ServiceUnitName") %></label><br />
                                                    <label class="lblNormal" style="font-size: smaller">
                                                        <%=GetLabel("Status Registrasi : ") %><%#:Eval("RegistrationStatus") %></label>
                                                    <br />
                                                    <label class="lblNormal" style="font-size: smaller">
                                                        <%=GetLabel("Penjamin Bayar : ") %><%#:Eval("BusinessPartnerName") %></label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ParamedicName" HeaderText="Dokter" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="180px" />
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px">
                                                <ItemTemplate>
                                                    <label class="lblLink lblDetail">
                                                        <%=GetLabel("Detail") %></label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px">
                                                <ItemTemplate>
                                                    <img class="imgLink imgPrint" title='<%=GetLabel("Print")%>' src='<%=ResolveUrl("~/Libs/Images/Button/print.png")%>'
                                                        alt="" style="margin-right: 2px" />
                                                    <div>
                                                        <%=GetLabel("Print Etiket")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada informasi pelayanan resep")%>
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
</asp:Content>
