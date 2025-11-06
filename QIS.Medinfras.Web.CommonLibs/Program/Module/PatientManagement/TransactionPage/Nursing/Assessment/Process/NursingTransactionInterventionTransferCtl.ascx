<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NursingTransactionInterventionTransferCtl.ascx.cs" 
Inherits="QIS.Medinfras.Web.CommonLibs.Program.NursingTransactionInterventionTransferCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript">
    var iRowIndex3 = 0;

    $('#<%=grdViewIntervention.ClientID %> tr:gt(0)').live('click', function () {
        iRowIndex3 = $('#<%=grdViewIntervention.ClientID %> tr').index(this);
        $('#<%=grdViewIntervention.ClientID %> tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
        getCheckedInterventionItem("");
        cbpViewIntervention1.PerformCallback('refresh');
    });

    function onCbpViewIntervention1EndCallback(s) {
        $('#<%=hdnOldID.ClientID %>').val($('#<%=hdnID.ClientID %>').val());

        onAfterSaveRecordDtSuccess(s.cpTransactionID);
        cbpViewIntervention.PerformCallback('refresh');
        cbpViewImplementation.PerformCallback('refresh');

        setHiddenValueFromDiagnoseItem(s.cpEvaluation);
        hideLoadingPanel();
    }

    function resetCheckedInterventionItem() {
        $('#<%=hdnListNursingInterventionItemID.ClientID %>').val('|');
        $('#<%=hdnListNursingInterventionItemText.ClientID %>').val('|');
        $('#<%=hdnListIsInterventionEditedByUser.ClientID %>').val('|');
        $('#<%=hdnListInterventionImplementation.ClientID %>').val('|');
    }

    function getCheckedInterventionItem(errMessage) {
        var lstNursingInterventionItemID = $('#<%=hdnListNursingInterventionItemID.ClientID %>').val().split('|');
        var lstNursingInterventionItemText = $('#<%=hdnListNursingInterventionItemText.ClientID %>').val().split('|');
        var lstIsInterventionEditedByUser = $('#<%=hdnListIsInterventionEditedByUser.ClientID %>').val().split('|');
        var lstInterventionImplementation = $('#<%=hdnListInterventionImplementation.ClientID %>').val().split('|');


        $('.grdSelected .chkIsSelectedIntervention input').each(function () {
            if ($(this).is(':checked')) {
                $tr = $(this).closest('tr');
                var key = $tr.find('.keyField').html();
                var nursingItemText = $tr.find('.txtNursingItemText').val();
                var isEditedByUser = $tr.find('.chkIsInterventionEditedByUser input').is(':checked');
                var idx = lstNursingInterventionItemID.indexOf(key);
                if (idx < 0) {
                    lstNursingInterventionItemID.push(key);
                    lstNursingInterventionItemText.push(nursingItemText);
                    lstIsInterventionEditedByUser.push(isEditedByUser);
                }
                else {
                    lstNursingInterventionItemText[idx] = nursingItemText;
                    lstIsInterventionEditedByUser[idx] = isEditedByUser;
                }
            }
            else {
                var key = $(this).closest('tr').find('.keyField').html();
                var idx = lstNursingInterventionItemID.indexOf(key);
                if (idx > -1) {
                    lstNursingInterventionItemID.splice(idx, 1);
                    lstNursingInterventionItemText.splice(idx, 1);
                    lstIsInterventionEditedByUser.splice(idx, 1);
                }
            }
        });
        if (lstNursingInterventionItemID.length > 2 && lstNursingInterventionItemID[1] == '') {
            lstNursingInterventionItemID.splice(0, 1);
            lstNursingInterventionItemText.splice(0, 1);
            lstIsInterventionEditedByUser.splice(0, 1);
        }


        $('#<%=hdnListNursingInterventionItemID.ClientID %>').val(lstNursingInterventionItemID.join('|'));
        $('#<%=hdnListNursingInterventionItemText.ClientID %>').val(lstNursingInterventionItemText.join('|'));
        $('#<%=hdnListIsInterventionEditedByUser.ClientID %>').val(lstIsInterventionEditedByUser.join('|'));

        if ($('#<%=hdnListNursingInterventionItemID.ClientID %>').val() == '')
            resetCheckedInterventionItem();
    }

    $('.grdSelected .chkIsInterventionEditedByUser input').live('change', function () {
        $tr = $(this).closest('tr');
        var key = $tr.find('.keyField').html();
        $txtNursingItemText = $tr.find('.divTxtInterventionItemText');
        $lblNursingItemText = $tr.find('.divLblInterventionItemText');
        if ($(this).is(':checked')) {
            $lblNursingItemText.hide();
            $txtNursingItemText.show();
        }
        else {
            $txtNursingItemText.hide();
            $lblNursingItemText.show();
        }
    });

    function onCbpViewInterventionEndCallback(s) {
        if (s.cpResult == "delete")
            $('#<%=hdnID.ClientID %>').val('');
        if (iRowIndex3 > 0) {
            $("#<%=grdViewIntervention.ClientID %> tr:eq(" + iRowIndex3 + ")").addClass('selected');
        }

        hideLoadingPanel();
    }

    //#region Intervention
    function onGetInterventionFilterExpression() {
        cbpFilterExpression.PerformCallback();
    }

    $('#lblIntervention.lblLink').die('click');
    $('#lblIntervention.lblLink').live('click', function () {
        cbpFilterExpression.PerformCallback();
    });


    function onTxtInterventionCodeChanged(value) {
        var filterExpression = " NurseInterventionCode = '" + value + "'";
        Methods.getObject('GetNursingInterventionList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnInterventionID.ClientID %>').val(result.NurseInterventionID);
            }
            else {
                $('#<%=hdnInterventionID.ClientID %>').val('');
            }
        });
    }

    function onCbpFilterExpressionEndCallback(s) {
        $('#<%=hdnFilterExpression.ClientID %>').val(s.cpResult);
        openSearchDialog('nurseDiagnoseIntervention', $('#<%=hdnFilterExpression.ClientID %>').val(), function (value) {
            onTxtInterventionCodeChanged(value);
        });
    }
    //#endregion

    //#region Add Intervention
    $('.btnAddIntervention').die('click');
    $('.btnAddIntervention').live('click', function () {
        if ($('#<%=hdnInterventionID.ClientID %>').val() != '' && $('.txtTransactionNo').val() != '0') {
            cbpViewIntervention.PerformCallback('save');
            $('#<%=hdnInterventionID.ClientID %>').val('');
        }
    });
    //#endregion

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function (evt) {
        $row = $(this).closest('tr').parent().closest('tr');
        if (confirm("Are You Sure Want To Delete This Data?")) {
            var entity = rowToObject($row);
            $('#<%=hdnID.ClientID %>').val(entity.ID);
            cbpViewIntervention.PerformCallback('delete');
        }
    });

</script>


<input type="hidden" value="" id="hdnNursingDiagnoseID" runat="server" />
<input type="hidden" value="" id="hdnFilterExpression" runat="server" />
<input type="hidden" value="" id="hdnOldID" runat="server" />
<input type="hidden" value="" id="hdnID" runat="server" />

<table width="100%">
    <colgroup>
        <col width="30%" />
        <col />
    </colgroup>
    <tr valign="top">
        <td>
            <table>
                <colgroup>
                    <col width="20%" />
                </colgroup>
                <tr>
                    <%--<td class="tdLabel"><label id="lblIntervention" class="lblLink"><%=GetLabel("Intervensi")%></label></td>--%>
                    <td>
                        <input type="hidden" runat="server" id="hdnInterventionID" />
                        <table style="width:100%" cellpadding="0" cellspacing="0">
                            <colgroup>
                                <col style="width:25%"/>
                                <col style="width:3px"/>
                                <col/>
                            </colgroup>
                            <tr>
                              <%--  <td><asp:TextBox ID="txtInterventionCode" CssClass="required" Width="100%" runat="server" /></td>
                                <td>&nbsp;</td>
                                <td><asp:TextBox ID="txtInterventionName" ReadOnly="true" Width="100%" runat="server" /></td>--%>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align:right">
                      <%--  <input type="button" id="btnAddIntervention" class="btnAddIntervention  w3-btn w3-hover-blue" value="Tambah Intervensi"/>--%>
                    </td>
                </tr>
            </table>
            <dxcp:ASPxCallbackPanel ID="cbpViewIntervention" runat="server" Width="100%" ClientInstanceName="cbpViewIntervention"
                ShowLoadingPanel="false"  OnCallback="cbpViewIntervention_Callback">
                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                    EndCallback="function(s,e){ onCbpViewInterventionEndCallback(s); }" />
                <PanelCollection>
                    <dx:PanelContent ID="PanelViewIntervention" runat="server">
                        <asp:Panel runat="server" ID="pnlViewIntervention" CssClass="pnlContainerGrid">
                            <input type="hidden" id="hdnTransactionID" runat="server" value="" />
                            <asp:GridView ID="grdViewIntervention" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdViewIntervention_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10px">
                                        <ItemTemplate>
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td><img runat="server" id="imgDelete" class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" /></td>
                                                </tr>
                                            </table>
                                            <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                            <input type="hidden" value="<%#:Eval("NurseInterventionName") %>" bindingfield="NurseInterventionName" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="NurseInterventionName" HeaderText="Intervensi" HeaderStyle-HorizontalAlign="Left" />
                                    <asp:TemplateField HeaderText="Item" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblTotalSelected"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    Tidak ada informasi intervensi asuhan keperawatan
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </asp:Panel>
                    </dx:PanelContent>
                </PanelCollection>
            </dxcp:ASPxCallbackPanel>    
            <div class="imgLoadingGrdView" id="containerImgLoadingView">
                <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
            </div>
        </td>
        <td>
            <dxcp:ASPxCallbackPanel ID="cbpViewIntervention1" runat="server" Width="100%" ClientInstanceName="cbpViewIntervention1"
                ShowLoadingPanel="false" OnCallback="cbpViewIntervention1_Callback">
                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                    EndCallback="function(s,e){ onCbpViewIntervention1EndCallback(s); }" />
                <PanelCollection>
                    <dx:PanelContent ID="PanelViewIntervention1" runat="server">
                        <asp:Panel runat="server" ID="pnlViewIntervention1" CssClass="pnlContainerGrid">
                            <input type="hidden" id="hdnListNursingInterventionItemID" runat="server" />
                            <input type="hidden" id="hdnListNursingInterventionItemText" runat="server" />
                            <input type="hidden" id="hdnListIsInterventionEditedByUser" runat="server" />
                            <input type="hidden" id="hdnListInterventionImplementation" runat="server" />
                            <asp:GridView ID="grdViewIntervention1" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdViewIntervention1_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="NursingInterventionItemID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                    <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkIsSelectedIntervention" Enabled="False" class="chkIsSelectedIntervention" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="DisplayOrder" HeaderText="Kode" HeaderStyle-HorizontalAlign="Left" />
                                    <asp:TemplateField HeaderText="Aktifitas Intervensi" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <div runat="server" id="divLblInterventionItemText" class="divLblInterventionItemText">
                                                <asp:Label runat="server" ID="lblNursingItemText" Text='<%#: Eval("NursingItemText") %>' class="lblNursingItemText"/>
                                            </div>
                                            <div runat="server" id="divTxtInterventionItemText" class="divTxtInterventionItemText" style="display:none">
                                                <asp:TextBox runat="server" ID="txtNursingItemText" Text='<%#: Eval("NursingItemText") %>' Width="100%" class="txtNursingItemText" />
                                            </div>                                            
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Edit" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <div <%#: Eval("IsEditableByUser").ToString() == "False" ? "style='display:none'" : ""%>>
                                                <asp:CheckBox runat="server" ID="chkIsInterventionEditedByUser" class="chkIsInterventionEditedByUser" Enabled = "False"/>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    Tidak ada informasi intervensi asuhan keperawatan
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </asp:Panel>
                    </dx:PanelContent>
                </PanelCollection>
            </dxcp:ASPxCallbackPanel>   
            
            <div style="display: none">
                <dxcp:ASPxCallbackPanel ID="cbpFilterExpression" runat="server" Width="100%" ClientInstanceName="cbpFilterExpression"
                    ShowLoadingPanel="false" OnCallback="cbpFilterExpression_Callback">
                    <ClientSideEvents EndCallback="function(s,e){onCbpFilterExpressionEndCallback(s); }" />
                </dxcp:ASPxCallbackPanel>
            </div>              
        </td>
    </tr>
</table>