<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.Master" AutoEventWireup="true" 
    CodeBehind="PatientBillSummaryPayReceiptPrint.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientBillSummaryPayReceiptPrint" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientBillSummaryToolbarCtl.ascx" TagName="ToolbarCtl" TagPrefix="uc1" %>

<asp:Content ID="Content4" ContentPlaceHolderID="plhMPPatientPageNavigationPane" runat="server">   
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnProcessPaymentReceipt" CRUDMode="R" runat="server"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><br style="clear:both"/><div><%=GetLabel("Process")%></div></li>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">   
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />  
    <input type="hidden" value="" id="hdnPaymentID" runat="server" />  
    <input type="hidden" value="" id="hdnPaymentAmount" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />  
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">   
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            onLoadPaymentReceiptPrint();

            $('#<%=btnProcessPaymentReceipt.ClientID %>').click(function () {
                showLoadingPanel();
                if ($('.chkIsSelected input:checked').length < 1) {
                    showToast('Warning', 'Please Select Payment First');
                    hideLoadingPanel();
                }
                else {
                    var param = '';
                    $('.chkIsSelected input:checked').each(function () {
                        var paymentID = $(this).closest('tr').find('.hdnKeyField').val();
                        if (param != '')
                            param += '|';
                        param += paymentID;
                    });
                    $('#<%=hdnParam.ClientID %>').val(param);

                    var url = ResolveUrl('~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientPaymentReceipt/PatientBillSummaryPayReceiptDetailPrintCtl.ascx');
                    var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
                    var departmentID = $('#<%=hdnDepartmentID.ClientID %>').val();
                    var paymentAmount = $('#<%=hdnTotalAmount.ClientID %>').val();
                    var patientName = $('#<%=hdnPatientName.ClientID %>').val();
                    var paymentID = $('#<%=hdnParam.ClientID %>').val();
                    var id = registrationID + '|' + departmentID + '|' + paymentAmount + '|' + patientName + ';' + paymentID;
                    openUserControlPopup(url, id, 'Print Receipt', 520, 250);
                }
            });

            $('.lnkPaymentNo').click(function () {
                var id = $(this).closest('tr').find('.hdnKeyField').val();
                var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientPaymentReceipt/PatientBillSummaryPayDetailCtl.ascx");
                openUserControlPopup(url, id, 'Patient Payment', 1100, 500);
            });

            var isChecked = true;
            $('.chkSelectAll').find('input').prop('checked', isChecked);
            $('.chkIsSelected').each(function () {
                $(this).find('input').prop('checked', isChecked);
            });
            calculateTotal();

        });

        function onAfterCustomClickSuccess(type) {
            cbpView.PerformCallback();
        }

        function onLoadPaymentReceiptPrint() {
            calculateTotal();
            $('.chkIsSelected input').change(function () {
                $('.chkSelectAll input').prop('checked', false);
                calculateTotal();
            });

            $('.chkSelectAll input').change(function () {
                var isChecked = $(this).is(":checked");
                $('.chkIsSelected').each(function () {
                    $(this).find('input').prop('checked', isChecked);
                });
                calculateTotal();
            });
        }

        function calculateTotal() {
            var total = 0;
            $('.chkIsSelected input:checked').each(function () {
                $tr = $(this).closest('tr');
                total += parseFloat($tr.find('.hdnPaymentAmount').val());
            });

            $('#tdTotalAll').html(total.formatMoney(2, '.', ','));

            $('#<%=hdnTotalAmount.ClientID %>').val(total);
        }

        function onCbpViewEndCallback(s) {
            onLoadPaymentReceiptPrint();
            hideLoadingPanel();
        }        
    </script> 

    <input type="hidden" value="" id="hdnTotalPatientAmount" runat="server" />
    <input type="hidden" value="" id="hdnTotalPayerAmount" runat="server" />
    <input type="hidden" value="" id="hdnTotalAmount" runat="server" />
    <input type="hidden" value="" id="hdnParam" runat="server" />
    <input type="hidden" value="" id="hdnPatientName" runat="server" />
    <input type="hidden" value="" id="hdnTransactionHdID" runat="server" />  
    <div>
        <div class="pageTitle">
            <div style="font-size: 1.1em"><%=GetLabel("Cetak Kwitansi Pasien")%></div>
        </div>
        <table class="tblContentArea" style="width:100%">
            <tr>
                <td>
                    <div style="padding: 5px;min-height: 150px;">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                                EndCallback="function(s,e) { onCbpViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlService" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                                        <asp:ListView ID="lvwView" runat="server">
                                            <EmptyDataTemplate>
                                                <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0" rules="all" >
                                                    <tr>  
                                                        <th style="width:40px">
                                                            <div style="padding:3px">
                                                                <asp:CheckBox ID="chkSelectAll" runat="server" CssClass="chkSelectAll" />
                                                            </div>
                                                        </th>
                                                        <th align="left">
                                                            <div style="padding:3px; text-align:left">
                                                                <div><%= GetLabel("No Pembayaran")%></div>
                                                                <div><%= GetLabel("Tanggal")%></div>                                                    
                                                            </div>
                                                        </th>
                                                        <th>
                                                            <div style="padding:3px; text-align:left">
                                                                <div><%= GetLabel("Dibuat Oleh")%></div>                                                      
                                                            </div>
                                                        </th>
                                                        <th>
                                                            <div style="text-align:right">
                                                                <%=GetLabel("Total Bayar")%>
                                                            </div>
                                                        </th>
                                                     </tr>
                                                     <tr class="trEmpty">
                                                        <td colspan="4">
                                                            <%=GetLabel("No Data To Display") %>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>                                
                                                <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0" rules="all" >
                                                    <tr>  
                                                        <th style="width:40px">
                                                            <div style="padding:3px">
                                                                <asp:CheckBox ID="chkSelectAll" runat="server" CssClass="chkSelectAll" />
                                                            </div>
                                                        </th>
                                                        <th align="left">
                                                            <div style="padding:3px; text-align:left">
                                                                <div><%= GetLabel("No Pembayaran")%></div>
                                                                <div><%= GetLabel("Tanggal")%></div>                                                    
                                                            </div>
                                                        </th>
                                                        <th>
                                                            <div style="padding:3px; text-align:left">
                                                                <div><%= GetLabel("Dibuat Oleh")%></div>                                                      
                                                            </div>
                                                        </th>
                                                        <th>
                                                            <div style="text-align:right">
                                                                <%=GetLabel("Total Bayar")%>
                                                            </div>
                                                        </th>
                                                      </tr>
                                                      <tr runat="server" id="itemPlaceholder" ></tr>
                                                      <tr class="trFooter">  
                                                            <td>&nbsp;</td>
                                                            <td colspan="2">
                                                                <div style="text-align:right;padding:3px">
                                                                    <%=GetLabel("Total")%>
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div style="text-align:right;padding:3px" id="tdTotalAll">
                                                                    <%=GetLabel("Total")%>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                     </table>
                                                 </LayoutTemplate>
                                                 <ItemTemplate>
                                                      <tr>
                                                        <td align="center">
                                                            <div style="padding:3px">
                                                                <asp:CheckBox ID="chkIsSelected" CssClass="chkIsSelected" runat="server" />
                                                                <input type="hidden" class="hdnKeyField" value="<%#: Eval("PaymentID")%>" />
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding:3px;float:left;">
                                                                <a class="lnkPaymentNo"><%#: Eval("PaymentNo")%></a>
                                                                <div><%#: Eval("PaymentDateInString")%></div>                                                    
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding:3px;">
                                                                <div><%#: Eval("LastUpdatedByUserName")%></div>                                                    
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="padding:3px;text-align:right;">
                                                                <input type="hidden" class="hdnPaymentAmount" value='<%#: Eval("ReceiveAmount")%>' />
                                                                <div><%#: Eval("ReceiveAmount", "{0:N}")%></div>                                                   
                                                            </div>   
                                                        </td>
                                                     </tr>
                                                 </ItemTemplate>
                                            </asp:ListView>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
