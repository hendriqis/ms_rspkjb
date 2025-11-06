<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.Master" AutoEventWireup="true" 
    CodeBehind="PatientBillSummaryPayReceipt.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientBillSummaryPayReceipt" %>

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
    <input type="hidden" value="" id="hdnPaymentReceiptID" runat="server" />  
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" /> 
    <input type="hidden" value="" id="hdnKode" runat="server" />  
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">   
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">        
        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var receiptID = $('#<%=hdnPaymentReceiptID.ClientID %>').val();
            if (receiptID == '') {
                errMessage.text = 'Please Save Header First!';
                return false;
            }
            else {
                filterExpression.text = "PaymentReceiptID = " + receiptID + " AND IsDeleted = 0";
                return true;
            }
        }

        $(function () {
            //onLoadGenerateBill();
            var grd = new customGridView();
            grd.init('<%=grdView.ClientID %>', '<%=hdnPaymentReceiptID.ClientID %>', '<%=pnlService.ClientID %>', cbpView);

            $('#<%=btnProcessPaymentReceipt.ClientID %>').click(function () {
                if ($('#<%=hdnKode.ClientID %>').val() == 'r') {
                    onCustomButtonClick('reprint');
                    var errMessage = { text: "" };
                    var filterExpression = { text: "" };
                    var reportCode = "PM-00404";
                    if (reportCode != "") {
                        var isAllowPrint = true;
                        if (typeof onBeforeRightPanelPrint == 'function') {
                            isAllowPrint = onBeforeRightPanelPrint(reportCode, filterExpression, errMessage);
                        }
                        if (isAllowPrint) {
                            openReportViewer(reportCode, filterExpression.text);
                        }
                        else
                            showToast('Warning', errMessage.text);
                    }
                }
                else {
                    showLoadingPanel();
                    var url = ResolveUrl('~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientPaymentReceipt/PatientBillSummaryPayReceiptVoidCtl.ascx');
                    var receiptID = $('#<%=hdnPaymentReceiptID.ClientID %>').val();
                    var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
                    var departmentID = $('#<%=hdnDepartmentID.ClientID %>').val();
                    var id = registrationID + '|' + receiptID + '|' + departmentID;
                    openUserControlPopup(url, id, 'Void Receipt', 350, 250);
                }

            });

            $('.lnkPaymentReceipt').click(function () {
                var id = $(this).closest('tr').find('.keyField').html();
                var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientPaymentReceipt/PatientBillSummaryPayReceiptDtCtl.ascx");
                openUserControlPopup(url, id, 'Patient Payment', 1100, 500);
            });
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
        }
    </script> 
    
    <input type="hidden" value="" id="hdnTotalPatientAmount" runat="server" />
    <input type="hidden" value="" id="hdnTotalPayerAmount" runat="server" />
    <input type="hidden" value="" id="hdnTotalAmount" runat="server" />
    <input type="hidden" value="" id="hdnParam" runat="server" />
    <input type="hidden" value="" id="hdnTransactionHdID" runat="server" />  
    <div>
        <div class="pageTitle">
            <div style="font-size: 1.1em"><%=GetMenuCaption()%></div>
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
                                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                            <Columns>
                                                <asp:BoundField DataField="PaymentReceiptID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <div style="padding:3px;text-align:left;">
                                                            <div><%= GetLabel("No Kuitansi")%></div>
                                                            <div><%= GetLabel("Tanggal")%></div>                                                    
                                                        </div>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div style="padding:3px;text-align:left;">
                                                            <a class="lnkPaymentReceipt"><%#: Eval("PaymentReceiptNo")%></a>
                                                            <div><%#: Eval("ReceiptDateInString")%></div>                                                    
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <div style="padding:3px;text-align:left;">
                                                            <div><%= GetLabel("Dibuat Oleh")%></div>                                                      
                                                        </div>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div style="padding:3px;text-align:left;">
                                                            <div><%#: Eval("LastUpdatedByUserName")%></div>                                                    
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <div style="text-align:right;">
                                                            <%=GetLabel("Total Bayar")%>
                                                        </div>
                                                        
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div style="padding:3px;text-align:right;">
                                                            <input type="hidden" class="hdnReceiptAmount" value='<%#: Eval("ReceiptAmount")%>' />
                                                            <div><%#: Eval("ReceiptAmount", "{0:N}")%></div>                                                   
                                                        </div>                                                    
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <%=GetLabel("No Data To Display")%>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
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