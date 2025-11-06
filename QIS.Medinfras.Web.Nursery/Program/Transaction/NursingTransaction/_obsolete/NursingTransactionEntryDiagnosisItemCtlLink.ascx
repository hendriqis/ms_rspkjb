<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NursingTransactionEntryDiagnosisItemCtlLink.ascx.cs" 
Inherits="QIS.Medinfras.Web.Nursing.Program.NursingTransactionEntryDiagnosisItemCtlLink" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript">
    $('#<%=grdView.ClientID %> tr:gt(0)').live('click', function () {
        $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
        getCheckedDiagnoseItem("");
        cbpView1.PerformCallback('refresh');

    });

    function onCbpViewEndCallback(s) {
        hideLoadingPanel();
    }

    function onCbpView1EndCallback(s) {
        $('#<%=hdnOldID.ClientID %>').val($('#<%=hdnID.ClientID %>').val());
        onAfterSaveRecordDtSuccess(s.cpTransactionID);
        cbpView.PerformCallback('refresh');
        cbpViewOutcome.PerformCallback('refresh');
        cbpViewIntervention.PerformCallback('refresh');
        setHiddenValueFromDiagnoseItem(s.cpEvaluation);

        hideLoadingPanel();
    }

    function resetCheckedDiagnoseItem() {
        $('#<%=hdnListNursingDiagnoseItemID.ClientID %>').val('|');
        $('#<%=hdnListNursingDiagnoseItemText.ClientID %>').val('|');
        $('#<%=hdnListIsEditedByUser.ClientID %>').val('|');
    }

    function getCheckedDiagnoseItem(errMessage) {
        var lstNursingDiagnoseItemID = $('#<%=hdnListNursingDiagnoseItemID.ClientID %>').val().split('|');
        var lstNursingDiagnoseItemText = $('#<%=hdnListNursingDiagnoseItemText.ClientID %>').val().split('|');
        var lstIsEditedByUser = $('#<%=hdnListIsEditedByUser.ClientID %>').val().split('|');

        $('.grdSelected .chkIsSelected input').each(function () {
            if ($(this).is(':checked')) {
                $tr = $(this).closest('tr');
                var key = $tr.find('.keyField').html();
                var nursingItemText = $tr.find('.txtNursingItemText').val();
                var isEditedByUser = $tr.find('.chkIsEditedByUser input').is(':checked');
                var idx = lstNursingDiagnoseItemID.indexOf(key);
                if (idx < 0) {
                    lstNursingDiagnoseItemID.push(key);
                    lstNursingDiagnoseItemText.push(nursingItemText);
                    lstIsEditedByUser.push(isEditedByUser);
                }
                else {
                    lstNursingDiagnoseItemText[idx] = nursingItemText;
                    lstIsEditedByUser[idx] = isEditedByUser;
                }
            }
            else {
                var key = $(this).closest('tr').find('.keyField').html();
                var idx = lstNursingDiagnoseItemID.indexOf(key);
                if (idx > -1) {
                    lstNursingDiagnoseItemID.splice(idx, 1);
                    lstNursingDiagnoseItemText.splice(idx, 1);
                    lstIsEditedByUser.splice(idx, 1);
                }
            }
        });
        if (lstNursingDiagnoseItemID.length > 2 && lstNursingDiagnoseItemID[1] == '') {
            lstNursingDiagnoseItemID.splice(0, 1);
            lstNursingDiagnoseItemText.splice(0, 1);
            lstIsEditedByUser.splice(0, 1);
        }
        $('#<%=hdnListNursingDiagnoseItemID.ClientID %>').val(lstNursingDiagnoseItemID.join('|'));
        $('#<%=hdnListNursingDiagnoseItemText.ClientID %>').val(lstNursingDiagnoseItemText.join('|'));
        $('#<%=hdnListIsEditedByUser.ClientID %>').val(lstIsEditedByUser.join('|'));

        if ($('#<%=hdnListNursingDiagnoseItemID.ClientID %>').val() == '')
            resetCheckedDiagnoseItem();
    }

    $('.grdSelected .chkIsEditedByUser input').live('change', function () {
        $tr = $(this).closest('tr');
        var key = $tr.find('.keyField').html();
        $txtNursingItemText = $tr.find('.divTxtNursingItemText');
        $lblNursingItemText = $tr.find('.divLblNursingItemText');
        if ($(this).is(':checked')) {
            $lblNursingItemText.hide();
            $txtNursingItemText.show();
        }
        else {
            $txtNursingItemText.hide();
            $lblNursingItemText.show();
        }
    });

</script>

<input type="hidden" id="hdnTransactionID" runat="server" value="" />
<input type="hidden" id="hdnNursingDiagnoseID" runat="server" value="" />
<input type="hidden" value="" id="hdnOldID" runat="server" />
<input type="hidden" value="" id="hdnID" runat="server" />

<table width="100%">
    <colgroup>
        <col width="30%" />
        <col />
    </colgroup>
    <tr valign="top">
        <td>
            <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                    EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent1" runat="server">
                        <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                            <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="NursingItemGroupSubGroupID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <div style="padding-left:3px">
                                                <%=GetLabel("Text")%>
                                            </div>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style='margin-left:<%#: Eval("CfMargin") %>0px;'><%#: Eval("NursingItemGroupSubGroupText")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Jml" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblTotalSelected"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    No Data To Display
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
            <dxcp:ASPxCallbackPanel ID="cbpView1" runat="server" Width="100%" ClientInstanceName="cbpView1"
                ShowLoadingPanel="false" OnCallback="cbpView1_Callback">
                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}"
                    EndCallback="function(s,e){ onCbpView1EndCallback(s); }" />
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent2" runat="server">
                        <asp:Panel runat="server" ID="pnlView1" CssClass="pnlContainerGrid">
                            <input type="hidden" id="hdnListNursingDiagnoseItemID" runat="server" />
                            <input type="hidden" id="hdnListNursingDiagnoseItemText" runat="server" />
                            <input type="hidden" id="hdnListIsEditedByUser" runat="server" />
                            <center><asp:TextBox runat="server" ID="txtNewDiagnoseItem" Width="95%"></asp:TextBox></center>
                            <asp:GridView ID="grdView1" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView1_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="NursingDiagnoseItemID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                    <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkIsSelected" class="chkIsSelected" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Diagnosis Item">
                                        <ItemTemplate>
                                            <div runat="server" id="divLblNursingItemText" class="divLblNursingItemText">
                                                <asp:Label runat="server" ID="lblNursingItemText" Text='<%#: Eval("NursingItemText") %>' class="lblNursingItemText"/>
                                            </div>
                                            <div runat="server" id="divTxtNursingItemText" class="divTxtNursingItemText" style="display:none">
                                                <asp:TextBox runat="server" ID="txtNursingItemText" Text='<%#: Eval("NursingItemText") %>' Width="100%" class="txtNursingItemText" />
                                            </div>
                                            
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Edit" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <div <%#: Eval("IsEditableByUser").ToString() == "False" ? "style='display:none'" : ""%>>
                                                <asp:CheckBox runat="server" ID="chkIsEditedByUser" class="chkIsEditedByUser" />
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                                <EmptyDataTemplate>
                                    No Data To Display
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </asp:Panel>
                    </dx:PanelContent>
                </PanelCollection>
            </dxcp:ASPxCallbackPanel>     
        </td>
    </tr>
</table>