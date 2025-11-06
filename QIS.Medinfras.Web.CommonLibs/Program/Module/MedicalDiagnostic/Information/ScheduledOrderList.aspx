<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPMain.master"
    AutoEventWireup="true" CodeBehind="ScheduledOrderList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.ScheduledOrderList" %>

<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">
    <script type="text/javascript">
        $(function () {
            //#region Medical No
            $('#lblMRN.lblLink').click(function () {
                openSearchDialog('patient', "", function (value) {
                    $('#<%=txtMRN.ClientID %>').val(value);
                    ontxtMRNChanged(value);
                });
            });

            $('#<%=txtMRN.ClientID %>').change(function () {
                ontxtMRNChanged($(this).val());
            });

            function ontxtMRNChanged(value) {
                var filterExpression = "MedicalNo = '" + value + "'";
                Methods.getObject('GetvPatientList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnMRN.ClientID %>').val(result.MRN);
                        $('#<%=txtMRN.ClientID %>').val(result.MedicalNo);
                        $('#<%=txtPatientName.ClientID %>').val(result.PatientName);
                    }
                    else {
                        $('#<%=hdnMRN.ClientID %>').val('');
                        $('#<%=txtMRN.ClientID %>').val('');
                        $('#<%=txtPatientName.ClientID %>').val('');
                    }
                    if (IsValid(null, 'fsPatientList', 'mpPatientList'))
                        cbpView.PerformCallback('refresh');
                });
            }

            //#endregion
        });

        function onCboMedicSupportValueChanged() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList'))
                cbpView.PerformCallback('refresh');
        }

        $('.lnkTransactionNo').live('click', function () {
            var $tr = $(this).closest('tr');
            var id = $tr.find('.hdnKeyField').val();
            var transactionCode = $tr.find('.hdnTransactionCode').val();
            var transactionCode = $(this).closest('tr').find('.hdnTransactionCode').val();
            var prescriptionOrderID = parseInt($tr.find('.hdnPrescriptionOrderID').val());
            var prescriptionReturnOrderID = parseInt($tr.find('.hdnPrescriptionReturnOrderID').val());
            var url = '';
            if (prescriptionOrderID > 0) {
                id = prescriptionOrderID;
                url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientBillSummary/GenerateBill/PatientBillSummaryGenerateBillDtPrescriptionCtl.ascx");
            }
            else if (prescriptionReturnOrderID > 0) {
                id = prescriptionReturnOrderID;
                url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientBillSummary/GenerateBill/PatientBillSummaryGenerateBillDtPrescriptionReturnCtl.ascx");
            }
            else if (transactionCode == '<%=laboratoryTransactionCode %>')
                url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientBillSummary/GenerateBill/PatientBillSummaryGenerateBillDtLaboratoryCtl.ascx");
            else
                url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientBillSummary/GenerateBill/PatientBillSummaryGenerateBillDtCtl.ascx");
            openUserControlPopup(url, id, 'Detail Item', 1100, 500);
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
                    $('.grdHistory tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
        }
        //#endregion
    </script>
    <div style="padding: 15px">
        <div class="pageTitle">
            <%=GetLabel("Informasi Pendaftaran Pasien")%></div>
        <table class="tblContentArea" style="width: 100%">
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <fieldset id="fsPatientList">
                        <table>
                            <colgroup>
                                <col style="width: 150px" />
                                <col style="width: 400px" />
                            </colgroup>
                            <tr id="trServiceUnit" runat="server">
                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Penunjang Medis")%></label></td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboMedicSupport" ClientInstanceName="cboMedicSupport" runat="server" Width="350px">
                                        <ClientSideEvents ValueChanged="function(s,e) { onCboMedicSupportValueChanged(); }"/>
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblLink lblNormal" id="lblMRN"><%=GetLabel("No Rekam Medis")%></label></td>
                                <td>
                                    <input type="hidden" id="hdnMRN" value="" runat="server" />
                                    <table style="width:100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width:30%"/>
                                            <col style="width:3px"/>
                                            <col/>
                                        </colgroup>
                                        <tr>
                                            <td><asp:TextBox ID="txtMRN" Width="100%" runat="server" /></td>
                                            <td>&nbsp;</td>
                                            <td><asp:TextBox ID="txtPatientName" ReadOnly="true" Width="100%" runat="server" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                           
                        </table>
                    </fieldset>

                </td>
            </tr>
            <tr>
                <td>
                    <input type="hidden" value="" id="hdnID" runat="server" />
                    <div style="position: relative;">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="overflow-y: scroll;">
                                        <asp:ListView ID="lvwView" runat="server">
                                            <EmptyDataTemplate>
                                                <table id="tblView" runat="server" class="grdNormal notAllowSelect grdHistory" cellspacing="0" rules="all" >
                                                    <tr>  
                                                        <th rowspan="2" align="left">
                                                            <div style="padding:3px;float:left;">
                                                                <div><%= GetLabel("No Bukti")%></div>
                                                                <div><%= GetLabel("Tanggal")%></div>                                                    
                                                            </div>
                                                            <div style="padding:3px;float:left;margin-left: 150px;">
                                                                <div><%= GetLabel("Unit Pelayanan")%></div>
                                                                <div><%= GetLabel("Dibuat Oleh")%></div>                                                      
                                                            </div>
                                                            <div style="padding:3px;margin-left: 400px;">
                                                                <div><%= GetLabel("Catatan")%></div>
                                                            </div>
                                                        </th>
                                                        <th colspan="3"><%=GetLabel("Jumlah")%></th>
                                                    </tr>
                                                    <tr>
                                                        <th style="width:150px">
                                                            <div style="text-align:right;padding-right:3px">
                                                                <%=GetLabel("Instansi")%>
                                                            </div>
                                                        </th>
                                                        <th style="width:150px">
                                                            <div style="text-align:right;padding-right:3px">
                                                                <%=GetLabel("Pasien")%>
                                                            </div>
                                                        </th>
                                                        <th style="width:150px">
                                                            <div style="text-align:right;padding-right:3px">
                                                                <%=GetLabel("Total")%>
                                                            </div>
                                                        </th>
                                                    </tr>
                                                    <tr class="trEmpty">
                                                        <td colspan="5">
                                                            <%=GetLabel("No Data To Display") %>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>                                
                                                <table id="tblView" runat="server" class="grdNormal notAllowSelect grdHistory" cellspacing="0" rules="all" >
                                                    <tr>  
                                                        <th rowspan="2" align="left">
                                                            <div style="padding:3px;float:left;">
                                                                <div><%= GetLabel("No Bukti")%></div>
                                                                <div><%= GetLabel("Tanggal")%></div>                                                 
                                                            </div>
                                                            <div style="padding:3px;float:left;margin-left: 150px;">
                                                                <div><%= GetLabel("Unit Pelayanan")%></div>
                                                                <div><%= GetLabel("Dibuat Oleh")%></div>                                                      
                                                            </div>
                                                            <div style="padding:3px;margin-left: 400px;">
                                                                <div><%= GetLabel("Catatan")%></div>
                                                            </div>
                                                        </th>
                                                        <th colspan="3"><%=GetLabel("Jumlah")%></th>
                                                    </tr>
                                                    <tr>
                                                        <th style="width:150px">
                                                            <div style="text-align:right;padding-right:3px">
                                                                <%=GetLabel("Instansi")%>
                                                            </div>
                                                        </th>
                                                        <th style="width:150px">
                                                            <div style="text-align:right;padding-right:3px">
                                                                <%=GetLabel("Pasien")%>
                                                            </div>
                                                        </th>
                                                        <th style="width:150px">
                                                            <div style="text-align:right;padding-right:3px">
                                                                <%=GetLabel("Total")%>
                                                            </div>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder" ></tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <div style="padding:3px;float:right;margin-right:50px;<%#: Eval("IsPendingRecalculated").ToString() == "False" ? "display:none" : ""%>">
                                                            <table>
                                                                <tr>
                                                                    <td><img height="24" src='<%= ResolveUrl("~/Libs/Images/Button/warning.png")%>' alt='' /></td>
                                                                    <td><label class="lblInfo"><%=GetLabel("Pending Recalculated") %></label></td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div style="padding:3px;float:left; width: 200px;">
                                                            <input type="hidden" class="hdnKeyField" value="<%#: Eval("TransactionID")%>" />
                                                            <input type="hidden" class="hdnTransactionCode" value='<%#: Eval("TransactionCode")%>' />
                                                            <input type="hidden" class="hdnPrescriptionOrderID" value="<%#: Eval("PrescriptionOrderID")%>" />
                                                            <input type="hidden" class="hdnPrescriptionReturnOrderID" value="<%#: Eval("PrescriptionReturnOrderID")%>" />
                                                            <a class="lnkTransactionNo"><%#: Eval("TransactionNo")%></a>
                                                            <div><%#: Eval("TransactionDateInString")%> <%#: Eval("TransactionTime")%></div>                                                    
                                                        </div>
                                                        <div style="padding:3px;float:left;">
                                                            <div><%#: Eval("ServiceUnitName")%></div>
                                                            <div><%#: Eval("LastUpdatedByUserName")%></div>                                                    
                                                        </div>
                                                        <div style="padding:3px;margin-left: 400px;">
                                                            <div><%#: Eval("Remarks")%></div>
                                                            <div>&nbsp;</div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding:3px;text-align:right;">
                                                            <input type="hidden" class="hdnPayerAmount" value='<%#: Eval("TotalPayerAmount")%>' />
                                                            <div><%#: Eval("TotalPayerAmount", "{0:N}")%></div>                                                   
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding:3px;text-align:right;">
                                                            <input type="hidden" class="hdnPatientAmount" value='<%#: Eval("TotalPatientAmount")%>' />
                                                            <div><%#: Eval("TotalPatientAmount", "{0:N}")%></div>                                                   
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding:3px;text-align:right;">
                                                            <input type="hidden" class="hdnLineAmount" value='<%#: Eval("TotalAmount")%>' />
                                                            <div><%#: Eval("TotalAmount", "{0:N}")%></div>                                                   
                                                        </div>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>    
                        <div class="imgLoadingGrdView" id="containerImgLoadingView" >
                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                        </div>
                        <div class="containerPaging">
                            <div class="wrapperPaging">
                                <div id="paging"></div>
                            </div>
                        </div> 
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
