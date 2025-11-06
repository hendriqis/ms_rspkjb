<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TestOrderDtCtl.ascx.cs" Inherits="QIS.Medinfras.Web.Imaging.Program.WorkList.TestOrderDtCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>


<script type="text/javascript" id="dxss_itemlaboratoryfractionentryctl">

    $('#btnApprove').click(function () {
        if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
            if ($('.chkIsSelected input:checked').length < 1) {
                showToast('Warning', 'Please Select Approved Item First');
            }
            else {
                var param = '';
                $('.chkIsSelected input:checked').each(function () {
                    var itemRequestHdID = $(this).closest('tr').find('.keyField').html();
                    if (param != '')
                        param += ',';
                    param += itemRequestHdID;
                });
                alert(param);
                $('#<%=hdnParam.ClientID %>').val(param);
                cbpEntryPopupView.PerformCallback('approve');
            }
        }
    });

    $('#btnDecline').click(function () {
        if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
            if ($('.chkIsSelected input:checked').length < 1) {
                showToast('Warning', 'Please Select Decline Item First');
            }
            else {
                var param = '';
                $('.chkIsSelected input:checked').each(function () {
                    var itemRequestHdID = $(this).closest('tr').find('.keyField').html();
                    if (param != '')
                        param += ',';
                    param += itemRequestHdID;
                });
                alert(param);
                $('#<%=hdnParam.ClientID %>').val(param);
                cbpEntryPopupView.PerformCallback('decline');
            }
        }
    });

    function onCbpEntryPopupViewEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        if (param[0] == 'approve') {
            if (param[1] == 'fail')
                showToast('Approve Failed', 'Error Message : ' + param[2]);
        }
        else if (param[0] == 'decline') {
            if (param[1] == 'fail')
                showToast('Decline Failed', 'Error Message : ' + param[2]);
        }
    }

    function getHSUIDExpression() {
        var filterExpression = "HealthcareServiceUnitID = '" + $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val() + "'";
        return filterExpression;
    }

    $('#lblTransactionNo.lblLink').click(function () {
        openSearchDialog('patientchargeshd', getHSUIDExpression(), function (value) {
            var filterExpression = "TransactionNo = '" + value + "'";
            Methods.getObject('GetPatientChargesHdList', filterExpression, function (value) {
                $('#<%=txtTransactionNo.ClientID %>').val(value.TransactionNo);
                $('#<%=txtTestOrderDate.ClientID %>').val(value.TransactionDateInString);
                $('#<%=txtTestOrderTime.ClientID %>').val(value.TransactionTime);
            });
        });
    });

    //#region Physician
    function onGetPhysicianFilterExpression() {
        var filterExpression = "";
        if ($('#<%=hdnIsHealthcareServiceUnitHasParamedic.ClientID %>').val() == '1')
            filterExpression = "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = '" + $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val() + "') AND IsDeleted = 0";
        else
            filterExpression = "IsDeleted = 0";
        return filterExpression;
    }

    $('#lblPhysician.lblLink').live('click', function () {
        openSearchDialog('paramedic', onGetPhysicianFilterExpression(), function (value) {
            $('#<%=txtPhysicianCode.ClientID %>').val(value);
            onTxtPhysicianCodeChanged(value);
        });
    });

    $('#<%=txtPhysicianCode.ClientID %>').live('change', function () {
        onTxtPhysicianCodeChanged($(this).val());
    });

    function onTxtPhysicianCodeChanged(value) {
        var filterExpression = onGetPhysicianFilterExpression() + " AND ParamedicCode = '" + value + "'";
        Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnPhysicianID.ClientID %>').val(result.ParamedicID);
                $('#<%=txtPhysicianName.ClientID %>').val(result.ParamedicName);
            }
            else {
                $('#<%=hdnPhysicianID.ClientID %>').val('');
                $('#<%=txtPhysicianCode.ClientID %>').val('');
                $('#<%=txtPhysicianName.ClientID %>').val('');
            }
        });
        cbpEntryPopupView.PerformCallback('refresh');
    }
    //#endregion

</script>

<div style="height:440px; overflow-y:auto;overflow-x: hidden">
    <input type="hidden" id="hdnHealthcareServiceUnitID" value="" runat="server" />
    <input type="hidden" id="hdnVisitID" value="" runat="server" />
    <input type="hidden" id="hdnTestOrderID" value="" runat="server" />
    <input type="hidden" id="hdnOrderDate" value="" runat="server" />
    <input type="hidden" id="hdnClassID" value="" runat="server" />
    <input type="hidden" id="hdnRegistrationID" value="" runat="server" />
    <input type="hidden" id="hdnHSUImagingID" value="" runat="server" />
    <input type="hidden" id="hdnHSULaboratoryID" value="" runat="server" />
    <input type="hidden" id="hdnIsHealthcareServiceUnitHasParamedic" value="" runat="server" />
    <div class="pageTitle"><%=GetLabel("Test Order Detail")%></div>
    <table class="tblContentArea">
        <colgroup>
            <col style="width:100%"/>
        </colgroup>
        <tr>            
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:100%">
                    <colgroup>
                        <col style="width:160px"/>
                        <col/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblLink" id="lblTransactionNo"><%=GetLabel("Transaction No")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtTransactionNo" ReadOnly="true" Width="150px" runat="server" /></td>
                    </tr>  
                    <tr>
                       <td class="tdLabel">Order <%=GetLabel("Date") %> / <%=GetLabel("Time") %></td>
                         <td>
                          <table cellpadding="0" cellspacing="0">
                             <tr>
                                 <td style="padding-right: 1px;width:140px"><asp:TextBox ID="txtTestOrderDate" Width="120px" CssClass="datepicker" runat="server" /></td>
                                 <td style="width:3px">&nbsp;</td>
                                 <td><asp:TextBox ID="txtTestOrderTime" Width="80px" CssClass="time" runat="server" Style="text-align:center" /></td>
                             </tr>
                          </table>
                        </td>
                   </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink lblMandatory" id="lblPhysician">
                                <%=GetLabel("Physician")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnPhysicianID" runat="server" value="" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30%" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtPhysicianCode" Width="100px" runat="server" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPhysicianName" Width="200px" ReadOnly="true" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
               <input type="hidden" value="" id="hdnParam" runat="server" />
               <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                        EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="ItemID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField"/>
                                        <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ItemCode" HeaderText="Item Code" HeaderStyle-Width="120px" />
                                        <asp:BoundField DataField="ItemName1" HeaderText="Item Name" HeaderStyle-Width="300px" />
                                        <asp:BoundField DataField="DiagnoseName" HeaderText="Diagnose Name" HeaderStyle-Width="300px" ItemStyle-HorizontalAlign="Right" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
            </td>
        </tr>
    </table>
    <div style="width:100%;text-align:right">
        <input type="button" value='<%= GetLabel("Close")%>' onclick="pcRightPanelContent.Hide();" />
        <table>
           <tr>
              <td>
                  <input type="button" id="btnApprove" value='<%= GetLabel("Approve")%>' />
              </td>
              <td>
                  <input type="button" id="btnDecline" value='<%= GetLabel("Decline")%>' />
              </td>
           </tr>
       </table>
    </div>
</div>
