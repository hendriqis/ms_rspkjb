<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QueuePharmacyEditCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.QueuePharmacyEditCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_apinvoicesupplierprocessctl">
   
    function getCheckedPurchaseReceive() {
        var totalNetAmount = 0;
        var param = '';
        $('.chkData input').each(function () {
           
            if ($(this).is(':checked')) {
               
                var $tr = $(this).closest('tr');
                var key = $(this).closest('tr').find('.keyField').val();
                var txtNoAntrian = $tr.find('.txtNoAntrian').val();
             
                if (param == '') {
                    param = '$setData|' + key + '|' + txtNoAntrian ;
                }
                else {
                    param += '$setData|' + key + '|' + txtNoAntrian;
                }
                 
            }
        });
       
        $('#<%=hdnDataSave.ClientID %>').val(param);
        
    }

 
 
 

    function onBeforeSaveRecord(errMessage) {
        var result = false;
        getCheckedPurchaseReceive();
        if ($('#<%=hdnDataSave.ClientID %>').val() == '') {
            errMessage.text = 'data yang di pilih kosong';
        }
        else {
            result = true;
        }
        return result;
    }

 
</script>
<div style="height: 500px; overflow-y: auto; overflow-x: hidden">
    <input type="hidden" id="hdnRegistrationIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnSelectedReferenceNo" runat="server" value="" />
    <input type="hidden" id="hdnSelectedReferenceDate" runat="server" value="" />
    <input type="hidden" id="hdnSelectedTaxInvoiceNo" runat="server" value="" />
    <input type="hidden" id="hdnSelectedTaxInvoiceDate" runat="server" value="" />
    <input type="hidden" id="hdnSelectedNetAmount" runat="server" value="" />
    <input type="hidden" id="hdnPurchaseInvoiceIDCtl" value="" runat="server" />
    <input type="hidden" id="hdnItemID" value="" runat="server" />
    <input type="hidden" id="hdnLabResultID" value="" runat="server" />
    <input type="hidden" id="hdnTransactionID" value="" runat="server" />
    <input type="hidden" id="hdnDueDate" value="" runat="server" />
    <input type="hidden" id="hdnIsChecked" value="" runat="server" />
    <input type="hidden" id="hdnFilterDate" value="" runat="server" />
    <input type="hidden" id="hdnFilterDay" value="" runat="server" />
    <input type="hidden" id="hdnIsUsedProductLineCtl" value="" runat="server" />
    <input type="hidden" id="hdnIsRemarksDtCopyFromPOR" value="" runat="server" />
    <input type="hidden" id="hdnProductLineIDCtl" value="" runat="server" />
    <input type="hidden" id="hdnDataSave" value="" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td valign="top">
                <table>
                    <colgroup>
                        <col style="width: 200px" />
                        <col style="width: 150px" />
                    </colgroup>
                   
                    <tr>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 3px; padding-left: 30px">
                            <%=GetLabel("Nomor Registrasi") %>
                        </td>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 3px; padding-left: 5px">
                            <asp:TextBox ID="txtRegistrationNo" Width="120px"   runat="server" />
                        </td>
                    </tr>
                     <tr>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 3px; padding-left: 30px">
                            <%=GetLabel("Nomor RM") %>
                        </td>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 3px; padding-left: 5px">
                            <asp:TextBox ID="txtRM" Width="120px"   runat="server" />
                        </td>
                    </tr>
                    <tr>
                       <td class="tdLabel" style="vertical-align: top; padding-top: 3px; padding-left: 30px">
                            <%=GetLabel("Nama Pasien") %>
                        </td>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 3px; padding-left: 5px">
                            <asp:TextBox ID="txtPatientName" Width="120px"   runat="server" />
                        </td>
                    </tr>
                   
                </table>
            </td>
             
        </tr>
        <tr>
            <td colspan="2" style="padding: 5px; vertical-align: top">
                <dxcp:aspxcallbackpanel id="cbpProcessDetail" runat="server" width="100%" clientinstancename="cbpProcessDetail"
                    showloadingpanel="false" oncallback="cbpProcessDetail_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                    <EmptyDataTemplate>
                                        <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                            rules="all">
                                            <tr>
                                                <th  style="display:none;"></th>
                                               
                                                <th style="width: 100px">
                                                    <%=GetLabel("Unit Pelayanan")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("No. Order")%>
                                                </th>
                                                <th style="width: 250px" align="center">
                                                    <%=GetLabel("Dokter/Paramedic")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Jenis Order")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Informasi dibuat")%>
                                                </th>
                                                <th style="width: 100px" align="right">
                                                    <%=GetLabel("Nomor Antrian")%>
                                                </th>
                                                
                                                <th style="width: 30px" align="center">
                                                </th>
                                            </tr>
                                            <tr class="trEmpty">
                                                <td colspan="15">
                                                    <%=GetLabel("No Data To Display")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                            rules="all">
                                            <tr>
                                                <th  style="display:none;">
                                                
                                                </th>
                                                
                                                <th style="width: 100px">
                                                    <%=GetLabel("Unit Pelayanan")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("No. Order")%>
                                                </th>
                                                <th style="width: 250px" align="center">
                                                    <%=GetLabel("Dokter/Paramedic")%>
                                                </th>
                                                
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Informasi dibuat")%>
                                                </th>
                                                <th style="width: 100px" align="center">
                                                    <%=GetLabel("Nomor Antrian")%>
                                                </th>
                                                
                                                <th style="width: 30px" align="center">
                                                </th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td style="display:none;"> <asp:CheckBox ID="chkData" runat="server" CssClass="chkData" Checked="true" />
                                             <input type="hidden" class="keyField" id="keyField" runat="server" value='<%#: Eval("PrescriptionOrderID")%>' />
                                               
                                               </td>
                                            
                                            <td>
                                               <%#: Eval("ServiceUnitName")%>
                                            </td>
                                              <td>
                                               <%#: Eval("PrescriptionOrderNo")%>
                                            </td>
                                            <td>
                                                 <%#: Eval("ParamedicName")%>
                                            </td>
                                             <td>
                                                 <%#: Eval("CreatedByName")%>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtNoAntrian" CssClass="txtNoAntrian" Width="95%" />
                                            </td>
                                            <td align="center">
                                                 
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:aspxcallbackpanel>
                <div class="containerPaging" style="display: none">
                    <div class="wrapperPaging">
                        <div id="pagingPopup">
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Close")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
