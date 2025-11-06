<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="PatientPaymentCashList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientPaymentCashList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li runat="server" id="btnProcess">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><div>
            <%=GetLabel("Process")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">



        $(function () {

            $('#<%=btnProcess.ClientID %>').click(function () {
                var errMessage = { text: "" };
                var filterExpression = { text: "" };

                getCheckedPayment();
                if ($('#<%=hdnSelectedPaymentID.ClientID %>').val() == '')
                    showToast('Warning', 'Please Select Payment First');
                else
                    cbpProcessPayment.PerformCallback();
            });

            setDatePicker('<%=txtPaymentDate.ClientID %>');
            $('#<%=txtPaymentDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%=txtPaymentDate.ClientID %>').change(function (evt) {
                onRefreshGridView();
            });

            $('#lblRefresh.lblLink').click(function (evt) {
                onRefreshGridView();
            });

            refreshGrdRegisteredPatient();
        });
        function refreshGrdRegisteredPatient() { cbpView.PerformCallback('refresh'); }
        var interval = parseInt('<%=GetRefreshGridInterval() %>') * 600000;
        var intervalID = window.setInterval(function () {
            onRefreshGridView();
        }, interval);

        function onRefreshGridView() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
                window.clearInterval(intervalID);
                $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
                refreshGrdRegisteredPatient();
                intervalID = window.setInterval(function () {
                    onRefreshGridView();
                }, interval);
            }
        }

        function onRefreshGridView() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
                window.clearInterval(intervalID);
                $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
                refreshGrdRegisteredPatient();
                intervalID = window.setInterval(function () {
                    onRefreshGridView();
                }, interval);
            }
        }

        $('#chkSelectAllPatient').die('change');
        $('#chkSelectAllPatient').live('change', function () {
            var isChecked = $(this).is(":checked");
            $('.chkPatient').each(function () {
                $(this).find('input').prop('checked', isChecked);
            });
        });
        function getCheckedPayment() {
            var lstSelectedPayment = '';
             
            var result = '';
            $('#<%=grdView.ClientID %> .chkPatient input').each(function () {
                if ($(this).is(':checked')) {
                    var key = $(this).closest('tr').find('.keyField').html();
                    if (lstSelectedPayment != '')
                        lstSelectedPayment += ',';
                    lstSelectedPayment += key;
                }
            });
            
            $('#<%=hdnSelectedPaymentID.ClientID %>').val(lstSelectedPayment);
            
        }
    
        function onCbpProcessPaymentEndCallback(s) {
            
            var result = s.cpResult.split('|');
            if (result[0] == 'success') {
                showToast('Proses success');
                cbpView.PerformCallback('refresh');
            }
            else {
                if (result[1] != '')
                    showToast('Process Failed', 'Error Message : ' + result[1]);
                else
                    showToast('Process Failed', '');
            }
            hideLoadingPanel();
        }
        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
        }
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnParam" runat="server" />
    <input type="hidden" value="" id="hdnParamHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnParamRoomID" runat="server" />
    <input type="hidden" value="" id="hdnParamClassID" runat="server" />
    <input type="hidden" value="" id="hdnHealthCareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <input type="hidden" value="" id="hdnSelectedPaymentID" runat="server" />
   
    <div style="padding: 15px">
        <table class="tblContentArea" style="width: 100%">
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <fieldset id="fsPatientList">
                        <table>
                            <colgroup>
                                <col style="width: 150px" />
                            </colgroup>
                            <tr>
                                <td>
                                    <%=GetLabel("Tanggal Pembayaran")%>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPaymentDate" Width="120px" runat="server" CssClass="datepicker" />
                                </td>
                            </tr>
                            <tr id="trServiceUnit" runat="server">
                                <td>
                                    <label>
                                        <%=GetLabel("Kasir") %></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboCashier" ClientInstanceName="cboCashier" Width="200px"
                                        runat="server">
                                        <ClientSideEvents ValueChanged="function(s,e) { onCboCashierValueChanged(s); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                             <tr>
                                <td class="tdLabel">
                                    <label>
                                        <%=GetLabel("Quick Search")%></label>
                                </td>
                                <td>
                                    <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                        Width="300px" Watermark="Search">
                                        <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                        <IntellisenseHints>
                                            <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                            <qis:QISIntellisenseHint Text="No RM" FieldName="MedicalNo" />
                                            <qis:QISIntellisenseHint Text="No Registrasi" FieldName="RegistrationNo" />
                                        </IntellisenseHints>
                                    </qis:QISIntellisenseTextBox>
                                </td>
                            </tr>
                        </table>
                        <div style="padding: 7px 0 0 3px; font-size: 0.95em">
                            <%=GetLabel("Halaman Ini Akan")%>
                            <span class="lblLink" id="lblRefresh">[refresh]</span>
                            <%=GetLabel("Setiap")%>
                            <%=GetRefreshGridInterval() %>
                            <%=GetLabel("Menit")%>
                        </div>
                        <hr />
                    </fieldset>
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlGridView" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em; height: 365px; overflow-y: scroll;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <input id="chkSelectAllPatient" type="checkbox" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <input type="hidden" class="PaymentID" value='<%#:Eval("PaymentID") %>' />
                                                    <asp:CheckBox ID="chkPatient" runat="server" CssClass="chkPatient" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="PaymentID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="PaymentNo" HeaderText="Payment No" HeaderStyle-Width="150px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="RegistrationNo" HeaderText="Registrasi No" HeaderStyle-Width="150px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="PaymentReceiptNo" HeaderText="Kwitansi No" HeaderStyle-Width="150px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                              <asp:BoundField DataField="PaymentDate" HeaderText="Tanggal Pembayaran" HeaderStyle-Width="150px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                             <asp:BoundField DataField="MedicalNo" HeaderText="Medical No" HeaderStyle-Width="150px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                             <asp:BoundField DataField="PatientName" HeaderText="Nama Pasien" HeaderStyle-Width="150px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                              <asp:BoundField DataField="ServiceUnitName" HeaderText="Service Unit" HeaderStyle-Width="150px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                             <asp:BoundField DataField="CashAmount" HeaderText="Pembayaran Cash" HeaderStyle-Width="150px"
                                                HeaderStyle-HorizontalAlign="right" ItemStyle-HorizontalAlign="right" /> 
                                              <asp:BoundField DataField="CashBackAmount" HeaderText="Pembayaran Kembalian" HeaderStyle-Width="150px"
                                                HeaderStyle-HorizontalAlign="right" ItemStyle-HorizontalAlign="right" /> 
                                            <asp:BoundField DataField="ReceiveCashAmount" HeaderText="Uang Yang diterima" HeaderStyle-Width="150px"
                                                HeaderStyle-HorizontalAlign="right" ItemStyle-HorizontalAlign="right" /> 
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No Data To Display")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </td>
            </tr>
        </table>
    </div>
    <div>
        <dxcp:ASPxCallbackPanel ID="cbpProcessPayment" runat="server" Width="100%"
            ClientInstanceName="cbpProcessPayment" ShowLoadingPanel="false" OnCallback="cbpProcessPayment_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpProcessPaymentEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent2" runat="server">
                    <asp:Panel runat="server" ID="Panel1" Style="width: 100%; margin-left: auto; margin-right: auto;
                        position: relative; font-size: 0.95em; height: 365px; overflow-y: scroll;">
                         
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>
    </div>
</asp:Content>
