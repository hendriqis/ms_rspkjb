<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NursingTransactionEntryEvaluationCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.NursingTransactionEntryEvaluationCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript">
    var iRowIndex5 = 0;
    $('#<%=grdViewOutcomeEvaluation.ClientID %> tr:gt(0)').live('click', function () {
        iRowIndex5 = $('#<%=grdViewOutcomeEvaluation.ClientID %> tr').index(this);
        $('#<%=grdViewOutcomeEvaluation.ClientID %> tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $('#<%=hdnID1.ClientID %>').val($(this).find('.keyField').html());
        getCheckedOutcomeEvaluationItem("");
        cbpViewOutcomeEvaluation1.PerformCallback('refresh');
    });

    function oncbpViewOutcomeEvaluationEndCallback(s) {
        if (iRowIndex5 > 0) {
            $("#<%=grdViewOutcomeEvaluation.ClientID %> tr:eq(" + iRowIndex5 + ")").addClass('selected');
        }
        hideLoadingPanel();
    }

    function oncbpViewOutcomeEvaluation1EndCallback(s) {

        $('#<%=hdnOldID1.ClientID %>').val($('#<%=hdnID1.ClientID %>').val());

        cbpViewOutcomeEvaluation.PerformCallback('refresh');
        cbpViewIntervention.PerformCallback('refresh');

        hideLoadingPanel();
    }

    function onRefreshParentGrid() {
        cbpViewOutcomeEvaluation.PerformCallback('refresh');
        cbpViewIntervention.PerformCallback('refresh');

        hideLoadingPanel();
    }

    function resetCheckedOutcomeEvaluationItem() {
        $('#<%=hdnListNursingDiagnoseItemIndicatorID1.ClientID %>').val('|');
        $('#<%=hdnListScaleScore1.ClientID %>').val('|');
        $('#<%=hdnListRemarks1.ClientID %>').val('|');
    }

    function getCheckedOutcomeEvaluationItem(errMessage) {
        var lstNursingDiagnoseItemIndicatorID1 = $('#<%=hdnListNursingDiagnoseItemIndicatorID1.ClientID %>').val().split('|');
        var lstScaleScore1 = $('#<%=hdnListScaleScore1.ClientID %>').val().split('|');
        var lstRemarks1 = $('#<%=hdnListRemarks1.ClientID %>').val().split('|');

        $('.grdViewOutcomeEvaluation1 .chkIsSelectedEvaluationOutcome input').each(function () {
            if ($(this).is(':checked')) {
                $tr = $(this).closest('tr');
                var key = $tr.find('.keyField').html();
                var remarks = $tr.find('.txtRemarksEvaluationOutcome').val();
                var score1 = $tr.find('.rbtScaleEvalution1Text input').is(':checked');
                var score2 = $tr.find('.rbtScaleEvalution2Text input').is(':checked');
                var score3 = $tr.find('.rbtScaleEvalution3Text input').is(':checked');
                var score4 = $tr.find('.rbtScaleEvalution4Text input').is(':checked');
                var score5 = $tr.find('.rbtScaleEvalution5Text input').is(':checked');
                var idx = lstNursingDiagnoseItemIndicatorID1.indexOf(key);

                if (idx < 0) {
                    lstNursingDiagnoseItemIndicatorID1.push(key);
                    if (score1)
                        lstScaleScore1.push("1");
                    else if (score2)
                        lstScaleScore1.push("2");
                    else if (score3)
                        lstScaleScore1.push("3");
                    else if (score4)
                        lstScaleScore1.push("4");
                    else if (score5)
                        lstScaleScore1.push("5");
                    else
                        lstScaleScore1.push("0");
                    lstRemarks1.push(remarks);
                }
                else {
                    if (score1)
                        lstScaleScore1[idx] = "1";
                    else if (score2)
                        lstScaleScore1[idx] = "2";
                    else if (score3)
                        lstScaleScore1[idx] = "3";
                    else if (score4)
                        lstScaleScore1[idx] = "4";
                    else if (score5)
                        lstScaleScore1[idx] = "5";
                    else
                        lstScaleScore1[idx] = "0";
                    lstRemarks1[idx] = remarks;
                }
            }
            else {
                var key = $(this).closest('tr').find('.keyField').html();
                var idx = lstNursingDiagnoseItemIndicatorID1.indexOf(key);
                if (idx > -1) {
                    lstNursingDiagnoseItemIndicatorID1.splice(idx, 1);
                    lstScaleScore1.splice(idx, 1);
                    lstRemarks1.splice(idx, 1);
                }
            }
        });

        if (lstNursingDiagnoseItemIndicatorID1.length > 2 && lstNursingDiagnoseItemIndicatorID1[1] == '') {
            lstNursingDiagnoseItemIndicatorID1.splice(0, 1);
            lstScaleScore1.splice(0, 1);
            lstRemarks1.splice(0, 1);
        }
        $('#<%=hdnListNursingDiagnoseItemIndicatorID1.ClientID %>').val(lstNursingDiagnoseItemIndicatorID1.join('|'));
        $('#<%=hdnListScaleScore1.ClientID %>').val(lstScaleScore1.join('|'));
        $('#<%=hdnListRemarks1.ClientID %>').val(lstRemarks1.join('|'));

        if ($('#<%=hdnListNursingDiagnoseItemIndicatorID1.ClientID %>').val() == '')
            resetCheckedOutcomeEvaluationItem();
    }
</script>
<style>
   .hide{display:none;}
</style>
<input type="hidden" id="hdnTransactionID1" runat="server" value="" />
<input type="hidden" id="hdnNursingDiagnoseID1" runat="server" value="" />
<input type="hidden" value="" id="hdnOldID1" runat="server" />
<input type="hidden" value="" id="hdnID1" runat="server" />
<input type="hidden" id="hdnListNursingEvaluationDiagnoseItemID1" runat="server" />
<input type="hidden" id="hdnListNursingTransactionEvaluationID1" runat="server" />
<input type="hidden" id="hdnListNursingEvaluationDiagnoseItemText1" runat="server" />
<input type="hidden" id="hdnListIsEvaluationEditedByUser1" runat="server" />
<input type="hidden" id="hdnListSaveNursingEvaluationDiagnoseItemID1" runat="server" />
<input type="hidden" id="hdnListSaveNursingTransactionEvaluationID1" runat="server" />
<input type="hidden" id="hdnListSaveNursingEvaluationDiagnoseItemText1" runat="server" />
<input type="hidden" id="hdnListSaveIsEvaluationEditedByUser1" runat="server" />
<input type="hidden" id="hdnListOutcomeDtID1" runat="server" />
<input type="hidden" id="hdnListScaleScoreEvaluation1" runat="server" />
<input type="hidden" id="hdnListNursingInterventionEvaluation1" runat="server" />
<input type="hidden" id="hdnListIsContinued1" runat="server" />
<input type="hidden" id="hdnProblemName" runat="server" />
<table width="100%">
    <colgroup>
        <col width="30%" />
        <col />
    </colgroup>
    <tr valign="top">
        <td>
            <dxcp:ASPxCallbackPanel ID="cbpViewOutcomeEvaluation" runat="server" Width="100%"
                ClientInstanceName="cbpViewOutcomeEvaluation" ShowLoadingPanel="false" OnCallback="cbpViewOutcomeEvaluation_Callback">
                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ oncbpViewOutcomeEvaluationEndCallback(s); }" />
                <PanelCollection>
                    <dx:PanelContent ID="PanelContentEvaluation1" runat="server">
                        <asp:Panel runat="server" ID="pnlViewEvaluation1" CssClass="pnlContainerGrid">
                            <asp:GridView ID="grdViewOutcomeEvaluation" runat="server" CssClass="grdSelected"
                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                OnRowDataBound="grdView_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="NursingDiagnoseItemID" HeaderStyle-CssClass="keyField"
                                        ItemStyle-CssClass="keyField" />
                                    <asp:BoundField DataField="NursingItemText" HeaderText="Deskripsi Luaran" HeaderStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="NursingOutcomeResult" HeaderText="Ekspektasi" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="100px" />
                                    <asp:TemplateField HeaderText="Item" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblTotalSelectedEvaluation"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    Belum ada data luaran yang dipilih
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
            <dxcp:ASPxCallbackPanel ID="cbpViewOutcomeEvaluation1" runat="server" Width="100%"
                ClientInstanceName="cbpViewOutcomeEvaluation1" ShowLoadingPanel="false" OnCallback="cbpViewOutcomeEvaluation1_Callback">
                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ oncbpViewOutcomeEvaluation1EndCallback(s); }" />
                <PanelCollection>
                    <dx:PanelContent ID="PanelContentEvaluation2" runat="server">
                        <asp:Panel runat="server" ID="pnlViewEvaluation2" CssClass="pnlContainerGrid">
                            <input type="hidden" id="hdnListNursingDiagnoseItemIndicatorID1" runat="server" />
                            <input type="hidden" id="hdnListScaleScore1" runat="server" />
                            <input type="hidden" id="hdnListRemarks1" runat="server" />
                            <fieldset id="fsTrx" style="margin: 0">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" Style="font-weight: bold"><%=GetLabel("Setelah dilakukan tindakan keperawatan selama ")%></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtNOCInterval" Width="20px" CssClass="txtNumeric"
                                                Style="text-align: center; color: red" Text="0"></asp:TextBox>
                                        </td>
                                        <td>
                                            <dxe:ASPxComboBox ID="cboIntervalType" ClientInstanceName="cboIntervalType" runat="server"
                                                Width="60px">
                                            </dxe:ASPxComboBox>
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rblProblemStatus" runat="server" RepeatDirection="Horizontal"
                                                RepeatLayout="Table" Style="font-weight: bold">
                                                <asp:ListItem Text=" teratasi" Value="1" Selected="True" />
                                                <asp:ListItem Text=" tidak teratasi" Value="0" />
                                            </asp:RadioButtonList>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label1" runat="server" Style="font-weight: bold"><%=GetLabel(" dengan kriteria ")%></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                            <asp:GridView ID="grdViewOutcomeEvaluation1" runat="server" CssClass="grdSelected grdViewOutcomeEvaluation1"
                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                OnRowDataBound="grdView1_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="NursingDiagnoseItemIndicatorID" HeaderStyle-CssClass="keyField"
                                        ItemStyle-CssClass="keyField" />
                                    <asp:BoundField DataField="NursingDiagnoseItemIndicatorID"  HeaderStyle-CssClass="hide"
                                        ItemStyle-CssClass="hide"  />
                                    <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkIsSelectedEvaluationOutcome" class="chkIsSelectedEvaluationOutcome" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Kriteria Hasil" DataField="NursingIndicatorText" HeaderStyle-Width="200px"
                                        HeaderStyle-HorizontalAlign="Left" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <table>
                                                <colgroup>
                                                    <col width="20%" />
                                                    <col width="20%" />
                                                    <col width="20%" />
                                                    <col width="20%" />
                                                    <col width="20%" />
                                                </colgroup>
                                                <tr valign="top">
                                                    <td>
                                                        <asp:RadioButton ID="rbtScaleEvalution1Text" runat="server" CssClass="rbtScaleEvalution1Text"
                                                            GroupName="ScaleEvalutionText" Text='<%#: "1-" + Eval("Scale1Text") %>' />
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rbtScaleEvalution2Text" runat="server" CssClass="rbtScaleEvalution2Text"
                                                            GroupName="ScaleEvalutionText" Text='<%#: "2-" + Eval("Scale2Text") %>' />
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rbtScaleEvalution3Text" runat="server" CssClass="rbtScaleEvalution3Text"
                                                            GroupName="ScaleEvalutionText" Text='<%#: "3-" + Eval("Scale3Text") %>' />
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rbtScaleEvalution4Text" runat="server" CssClass="rbtScaleEvalution4Text"
                                                            GroupName="ScaleEvalutionText" Text='<%#: "4-" + Eval("Scale4Text") %>' />
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rbtScaleEvalution5Text" runat="server" CssClass="rbtScaleEvalution5Text"
                                                            GroupName="ScaleEvalutionText" Text='<%#: "5-" + Eval("Scale5Text") %>' />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ket" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtRemarksEvaluationOutcome" CssClass="txtRemarksEvaluationOutcome"
                                                Width="100%" TextMode="MultiLine" Wrap="true" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    Belum ada kriteria hasil dari luaran yang dipilih
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </asp:Panel>
                    </dx:PanelContent>
                </PanelCollection>
            </dxcp:ASPxCallbackPanel>
        </td>
    </tr>
</table>
