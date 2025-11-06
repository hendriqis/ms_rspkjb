<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InfoTransactionDetailEKlaimParameterCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.InfoTransactionDetailEKlaimParameterCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_InfoTransactionDetailEKlaimParameterCtl1">
    //#region E-Klaim Parameter
    $('#lblEKlaimParameter.lblLink').click(function () {
        openSearchDialog('eklaimparameter', 'IsDeleted = 0', function (value) {
            $('#<%=txtEKlaimParameterCodeCtl.ClientID %>').val(value);
            ontxtEKlaimParameterCodeChanged(value);
        });
    });

    $('#<%=txtEKlaimParameterCodeCtl.ClientID %>').change(function () {
        ontxtEKlaimParameterCodeChanged($(this).val());
    });

    function ontxtEKlaimParameterCodeChanged(value) {
        var filterExpression = "EKlaimParameterCode = '" + value + "'";
        Methods.getObject('GetEKlaimParameterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnEKlaimParameterID.ClientID %>').val(result.EKlaimParameterID);
                $('#<%=txtEKlaimParameterNameCtl.ClientID %>').val(result.EKlaimParameterName);
            }
            else {
                $('#<%=hdnEKlaimParameterID.ClientID %>').val('');
                $('#<%=txtEKlaimParameterCodeCtl.ClientID %>').val('');
                $('#<%=txtEKlaimParameterNameCtl.ClientID %>').val('');
            }
        });
    }
    //#endregion
    $(".lblLink.lblSetting").live('click', function () {
        $('#formMaping').show();
        var ItemCode = $(this).attr("valItemCode");
        var filterExpression = "ItemCode='" + ItemCode + "'";
        Methods.getObject('GetItemMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=txtItemCode.ClientID %>').val(result.ItemCode);
                $('#<%=txtItemName.ClientID %>').val(result.ItemName1);
                $('#<%=hdnItemMasterID.ClientID %>').val(result.ItemID);
            }
            else {
                $('#<%=txtItemCode.ClientID %>').val('');
                $('#<%=txtItemName.ClientID %>').val('');
                $('#<%=hdnItemMasterID.ClientID %>').val('');
            }
        });

    });
     
    function ResetForm() {
        $('#<%=txtItemCode.ClientID %>').val('');
        $('#<%=txtItemName.ClientID %>').val('');
        $('#<%=hdnItemMasterID.ClientID %>').val('');
        $('#formMaping').hide();
    }
    $('#btnSave').click(function () {
        var EKlaimParameterID = $('#<%=hdnEKlaimParameterID.ClientID %>').val();
        if (EKlaimParameterID == "") {
            showToast('Failed', 'Silahkan diisi parameter eklaim');
        } else {
            cbpProcessDetail.PerformCallback('SaveEklaimParameter');
        }
    });
  function  onCbpProcessDetailEndCallback(s){
      hideLoadingPanel();
      var param = s.cpResult.split('|');
      if (param[0] == "SaveEklaimParameter") {
          if (param[1] == 'success') {
              ResetForm();
          } else {
              showToast('Failed', 'Error Message : ' + param[2]);
          }
          cbpProcessDetail.PerformCallback('refresh');
     }
   }
</script>
<div id="containerPopup">
    <input type="hidden" ID="hdnItemMasterID" value="" runat="server" />
    <input type="hidden" id="hdnRegistrationID" runat="server" value="" />
    <input type="hidden" id="hdnEKlaimParameterID" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td align="left">
                <table>
                    <colgroup>
                        <col style="width: 140px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <%=GetLabel("Registration No")%>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRegistrationNo" Width="170px" runat="server" ReadOnly="true"
                                Style="text-align: left" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=GetLabel("SEP No")%>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSEPNo" Width="170px" runat="server" ReadOnly="true" Style="text-align: left" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=GetLabel("Patient")%>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPatient" Width="350px" runat="server" ReadOnly="true" Style="text-align: left" />
                        </td>
                    </tr>
                </table>
            </td>
            <td align="right">
                <table width="600px" runat="server" cellspacing="0" rules="all">
                    <colgroup>
                        <col style="width: 170px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <b>
                                <%=GetLabel("Amount")%></b>
                        </td>
                        <td style="text-align: right">
                            <b>
                                <%=GetLabel("Patient")%></b>
                        </td>
                        <td style="text-align: right">
                            <b>
                                <%=GetLabel("Payer")%></b>
                        </td>
                        <td style="text-align: right">
                            <b>
                                <%=GetLabel("Total")%></b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=GetLabel("Seluruhnya")%>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTotalPatientAmount" Width="140px" runat="server" ReadOnly="true"
                                Style="text-align: right" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtTotalPayerAmount" Width="140px" runat="server" ReadOnly="true"
                                Style="text-align: right" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtTotalLineAmount" Width="140px" runat="server" ReadOnly="true"
                                Style="text-align: right; font-weight: bold" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=GetLabel("Obat (Ditagihkan)")%>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTotalPatientObatDitagihkanAmount" Width="140px" runat="server"
                                ReadOnly="true" Style="text-align: right" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtTotalPayerObatDitagihkanAmount" Width="140px" runat="server"
                                ReadOnly="true" Style="text-align: right" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtTotalLineObatDitagihkanAmount" Width="140px" runat="server" ReadOnly="true"
                                Style="text-align: right; font-weight: bold" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <%=GetLabel("Tanpa Obat (Ditagihkan)")%>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTotalPatientTanpaObatDitagihkanAmount" Width="140px" runat="server"
                                ReadOnly="true" Style="text-align: right" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtTotalPayerTanpaObatDitagihkanAmount" Width="140px" runat="server"
                                ReadOnly="true" Style="text-align: right" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtTotalLineTanpaObatDitagihkanAmount" Width="140px" runat="server"
                                ReadOnly="true" Style="text-align: right; font-weight: bold" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>   
            <div id="formMaping" style="display:none;">
            <h4>Mapping E-Klaim Parameter</h4>
            
                   <table width="100%">
                    <colgroup>
                        <col style="width: 140px">
                        <col>
                    </colgroup>
                    <tbody><tr>
                        <td>
                            Kode Item
                        </td>
                        <td>
                            <asp:TextBox ID="txtItemCode" runat="server" ReadOnly="true" style="width: 100%;" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Nama Item
                        </td>
                        <td>
                            <asp:TextBox ID="txtItemName" runat="server" ReadOnly="true" style="width: 100%;"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label class="lblLink" id="lblEKlaimParameter">
                                    <%=GetLabel("Parameter E-Klaim")%></label>
                        </td>
                         <td>
                                
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td> 
                                           
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEKlaimParameterCodeCtl" runat="server" />
                                        </td>
                                         <td>
                                            <asp:TextBox ID="txtEKlaimParameterNameCtl" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                    </tr>
                    <tr>
                        <td>
                           
                        </td>
                        <td>
                             <input type="button" class="btn btn-w3" value="SIMPAN" id="btnSave" />
                        </td>
                    </tr>
                </tbody>
                </table>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="padding: 5px; vertical-align: top">
                <div style="height: 350px; overflow-y: auto; overflow-x: hidden">
                    <dxcp:ASPxCallbackPanel ID="cbpProcessDetail" runat="server" Width="100%" ClientInstanceName="cbpProcessDetail"
                        ShowLoadingPanel="false" OnCallback="cbpProcessDetail_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpProcessDetailEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em;">
                                    <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th style="width: 70px">
                                                        <%=GetLabel("E-Klaim Parameter Code")%>
                                                    </th>
                                                    <th style="width: 150px" align="left">
                                                        <%=GetLabel("E-Klaim Parameter Name")%>
                                                    </th>
                                                    <th style="width: 70px">
                                                        <%=GetLabel("Item Code")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("Item Name")%>
                                                    </th>
                                                    <th style="width: 120px" align="right">
                                                        <%=GetLabel("Patient Amount")%>
                                                    </th>
                                                    <th style="width: 120px" align="right">
                                                        <%=GetLabel("Payer Amount")%>
                                                    </th>
                                                    <th style="width: 120px" align="right">
                                                        <%=GetLabel("Line Amount")%>
                                                    </th>
                                                    <th></th>
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="10">
                                                        <%=GetLabel("No Data To Display")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th style="width: 70px">
                                                        <%=GetLabel("E-Klaim Parameter Code")%>
                                                    </th>
                                                    <th style="width: 200px" align="left">
                                                        <%=GetLabel("E-Klaim Parameter Name")%>
                                                    </th>
                                                    <th style="width: 70px">
                                                        <%=GetLabel("Item Code")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("Item Name")%>
                                                    </th>
                                                    <th style="width: 120px" align="right">
                                                        <%=GetLabel("Patient Amount")%>
                                                    </th>
                                                    <th style="width: 120px" align="right">
                                                        <%=GetLabel("Payer Amount")%>
                                                    </th>
                                                    <th style="width: 120px" align="right">
                                                        <%=GetLabel("Line Amount")%>
                                                    </th>
                                                    <th></th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <input type="hidden" class="EKlaimParameterID" id="EKlaimParameterID" runat="server"
                                                        value='<%#: Eval("EKlaimParameterID")%>' />
                                                    <%#: Eval("EKlaimParameterCode")%>
                                                </td>
                                                <td>
                                                    <%#: Eval("EKlaimParameterName")%>
                                                </td>
                                                <td>
                                                    <%#: Eval("ItemCode")%>
                                                </td>
                                                <td>
                                                    <%#: Eval("ItemName1")%>
                                                </td>
                                                <td align="right">
                                                    <%#: Eval("cfPatientAmountInString")%>
                                                </td>
                                                <td align="right">
                                                    <%#: Eval("cfPayerAmountInString")%>
                                                </td>
                                                <td align="right">
                                                    <%#: Eval("cfLineAmountInString")%>
                                                </td>
                                                <td><label class="lblLink lblSetting" valItemCode="<%#: Eval("ItemCode")%>">MAPPING</label>
                                                    
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="containerPaging" style="display: none">
                        <div class="wrapperPaging">
                            <div id="pagingPopup">
                            </div>
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
