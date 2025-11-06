<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NursingTransactionEntryEvaluationCtl_Old.ascx.cs" 
Inherits="QIS.Medinfras.Web.CommonLibs.Program.NursingTransactionEntryEvaluationCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript">
    var GCAssessment = '<%= GetGCNursingEvaluationAssessment() %>';
    var GCPlanning = '<%= GetGCNursingEvaluationPlanning() %>';

    $('#<%=grdView.ClientID %> tr:gt(0)').live('click', function () {
        $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
        getCheckedEvaluation("");
        cbpViewEvaluation1.PerformCallback('refresh');

        if ($('#<%=hdnID.ClientID %>').val().trim() == GCAssessment.trim()) {
            $('.dvSubjectiveObjective').hide();
            $('.dvAssessment').show();
            $('.dvPlanning').hide();
        }
        else if ($('#<%=hdnID.ClientID %>').val().trim() == GCPlanning.trim()) {
            $('.dvSubjectiveObjective').hide();
            $('.dvAssessment').hide();
            $('.dvPlanning').show();
        }
        else {
            $('.dvSubjectiveObjective').show();
            $('.dvAssessment').hide();
            $('.dvPlanning').hide();
        }
    });

    function onCbpViewEvaluationEndCallback(s) {
        hideLoadingPanel();
    }

    function onCbpViewEvaluation1EndCallback(s) {
        $('#<%=hdnOldID.ClientID %>').val($('#<%=hdnID.ClientID %>').val());
        cbpViewEvaluation2.PerformCallback('refresh');
    }

    function onCbpViewEvaluation2EndCallback(s) {
        cbpViewEvaluation3.PerformCallback('refresh');
    }

    function onCbpViewEvaluation3EndCallback(s) {
        hideLoadingPanel();
    }

    function resetCheckedEvaluation() {
        resetCheckedEvaluationDiagnoseItem();
        resetCheckedEvaluationOutcome();
        resetCheckedEvaluationIntervention();
    }
    function resetCheckedEvaluationDiagnoseItem() {
        $('#<%=hdnListSaveNursingEvaluationDiagnoseItemID.ClientID %>').val('|');
        $('#<%=hdnListSaveNursingTransactionEvaluationID.ClientID %>').val('|');
        $('#<%=hdnListSaveNursingEvaluationDiagnoseItemText.ClientID %>').val('|');
        $('#<%=hdnListSaveIsEvaluationEditedByUser.ClientID %>').val('|');
    }
    function resetCheckedEvaluationOutcome(){
        $('#<%=hdnListOutcomeDtID.ClientID %>').val('|');
        $('#<%=hdnListScaleScoreEvaluation.ClientID %>').val('|');

    }
    function resetCheckedEvaluationIntervention() {
        $('#<%=hdnListNursingInterventionEvaluation.ClientID %>').val('|');
        $('#<%=hdnListIsContinued.ClientID %>').val('|');
    }

    function getCheckedEvaluation(errMessage) {
        getCheckedEvaluationDiagnoseItem("");
        getCheckedEvaluationOutcome("");
        getCheckedEvaluationIntervention("");
    }
    function getCheckedEvaluationDiagnoseItem(errMessage) {
        var lstNursingDiagnoseItemID = $('#<%=hdnListSaveNursingEvaluationDiagnoseItemID.ClientID %>').val().split('|');
        var lstNursingTransactionEvaluationDtID = $('#<%=hdnListSaveNursingTransactionEvaluationID.ClientID %>').val().split('|');
        var lstNursingDiagnoseItemText = $('#<%=hdnListSaveNursingEvaluationDiagnoseItemText.ClientID %>').val().split('|');
        var lstIsEditedByUser = $('#<%=hdnListSaveIsEvaluationEditedByUser.ClientID %>').val().split('|');

        $('.grdSelected .chkIsSelectedEvaluation input').each(function () {
            if ($(this).is(':checked')) {
                $tr = $(this).closest('tr');
                var key = $tr.find('.keyField').html();
                var key2 = $tr.find('.hdnKeyField2').val();
                var nursingItemText = $tr.find('.txtNursingItemText').val();
                var isEditedByUser = $tr.find('.chkIsEvaluationEditedByUser input').is(':checked');
                var idx = lstNursingDiagnoseItemID.indexOf(key);
                if (idx < 0) {
                    lstNursingDiagnoseItemID.push(key);
                    lstNursingTransactionEvaluationDtID.push(key2);
                    lstNursingDiagnoseItemText.push(nursingItemText);
                    lstIsEditedByUser.push(isEditedByUser);
                }
                else {
                    lstNursingTransactionEvaluationDtID[idx] = key2;
                    lstNursingDiagnoseItemText[idx] = nursingItemText;
                    lstIsEditedByUser[idx] = isEditedByUser;
                }
            }
            else {
                var key = $(this).closest('tr').find('.keyField').html();
                var key2 = $(this).closest('tr').find('.hdnKeyField2').val();
                if (key != "0") {
                    var idx = lstNursingDiagnoseItemID.indexOf(key);
                    if (idx > -1) {
                        lstNursingDiagnoseItemID.splice(idx, 1);
                        lstNursingTransactionEvaluationDtID.splice(idx, 1);
                        lstNursingDiagnoseItemText.splice(idx, 1);
                        lstIsEditedByUser.splice(idx, 1);
                    }
                }
                else {
                    var idx = lstNursingTransactionEvaluationDtID.indexOf(key2);
                    if (idx > -1) {
                        lstNursingDiagnoseItemID.splice(idx, 1);
                        lstNursingTransactionEvaluationDtID.splice(idx, 1);
                        lstNursingDiagnoseItemText.splice(idx, 1);
                        lstIsEditedByUser.splice(idx, 1);
                    }
                }
            }
        });
        if (lstNursingDiagnoseItemID.length > 2 && lstNursingDiagnoseItemID[1] == '') {
            lstNursingDiagnoseItemID.splice(0, 1);
            lstNursingTransactionEvaluationDtID.splice(0, 1);
            lstNursingDiagnoseItemText.splice(0, 1);
            lstIsEditedByUser.splice(0, 1);
        }
        $('#<%=hdnListSaveNursingEvaluationDiagnoseItemID.ClientID %>').val(lstNursingDiagnoseItemID.join('|'));
        $('#<%=hdnListSaveNursingTransactionEvaluationID.ClientID %>').val(lstNursingTransactionEvaluationDtID.join('|'));
        $('#<%=hdnListSaveNursingEvaluationDiagnoseItemText.ClientID %>').val(lstNursingDiagnoseItemText.join('|'));
        $('#<%=hdnListSaveIsEvaluationEditedByUser.ClientID %>').val(lstIsEditedByUser.join('|'));

//        $('#<%=hdnListSaveNursingEvaluationDiagnoseItemID.ClientID %>').val($('#<%=hdnListNursingEvaluationDiagnoseItemID.ClientID %>').val());
//        $('#<%=hdnListSaveNursingTransactionEvaluationID.ClientID %>').val($('#<%=hdnListNursingTransactionEvaluationID.ClientID %>').val());
//        $('#<%=hdnListSaveNursingEvaluationDiagnoseItemText.ClientID %>').val($('#<%=hdnListNursingEvaluationDiagnoseItemText.ClientID %>').val());
//        $('#<%=hdnListSaveIsEvaluationEditedByUser.ClientID %>').val($('#<%=hdnListIsEvaluationEditedByUser.ClientID %>').val());

        if ($('#<%=hdnListSaveNursingEvaluationDiagnoseItemID.ClientID %>').val() == '')
            resetCheckedEvaluationDiagnoseItem();
    }
    function getCheckedEvaluationOutcome(errMessage) {
        var lstNursingDiagnoseItemIndicatorID = $('#<%=hdnListOutcomeDtID.ClientID %>').val().split('|');
        var lstScaleScore = $('#<%=hdnListScaleScoreEvaluation.ClientID %>').val().split('|');

        $('.grdSelected .indicatorTextEvaluation').each(function () {
            $tr = $(this).closest('tr');
            var key = $tr.find('.keyField').html();
            var score1 = $tr.find('.rbtScale1Text input').is(':checked');
            var score2 = $tr.find('.rbtScale2Text input').is(':checked');
            var score3 = $tr.find('.rbtScale3Text input').is(':checked');
            var score4 = $tr.find('.rbtScale4Text input').is(':checked');
            var score5 = $tr.find('.rbtScale5Text input').is(':checked');
            var idx = lstNursingDiagnoseItemIndicatorID.indexOf(key);
            
            if (score1)
                lstScaleScore[idx] = "1";
            else if (score2)
                lstScaleScore[idx] = "2";
            else if (score3)
                lstScaleScore[idx] = "3";
            else if (score4)
                lstScaleScore[idx] = "4";
            else if (score5)
                lstScaleScore[idx] = "5";
            else
                lstScaleScore[idx] = "0";
        });

        $('#<%=hdnListOutcomeDtID.ClientID %>').val(lstNursingDiagnoseItemIndicatorID.join('|'));
        $('#<%=hdnListScaleScoreEvaluation.ClientID %>').val(lstScaleScore.join('|'));

        if ($('#<%=hdnListOutcomeDtID.ClientID %>').val() == '')
            resetCheckedEvaluationOutcome();
    }
    function getCheckedEvaluationIntervention(errMessage) {
        var lstNursingDiagnoseItemIndicatorID = $('#<%=hdnListNursingInterventionEvaluation.ClientID %>').val().split('|');
        var lstScaleScore = $('#<%=hdnListIsContinued.ClientID %>').val().split('|');

        $('.grdSelected .interventionItemEvaluation').each(function () {
            $tr = $(this).closest('tr');
            var key = $tr.find('.keyField').html();
            var isContinue = $tr.find('.rbtContinue input').is(':checked');
            var idx = lstNursingDiagnoseItemIndicatorID.indexOf(key);

            if (isContinue)
                lstScaleScore[idx] = "1";
            else
                lstScaleScore[idx] = "0";
        });

        $('#<%=hdnListNursingInterventionEvaluation.ClientID %>').val(lstNursingDiagnoseItemIndicatorID.join('|'));
        $('#<%=hdnListIsContinued.ClientID %>').val(lstScaleScore.join('|'));

        if ($('#<%=hdnListNursingInterventionEvaluation.ClientID %>').val() == '')
            resetCheckedEvaluationIntervention();
    }

    $('.grdSelected .chkIsEvaluationEditedByUser input').live('change', function () {
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

    function setHiddenValueFromDiagnoseItem(value) {
        var arrValue = value.split('#');

        $('#<%=hdnListNursingEvaluationDiagnoseItemID.ClientID %>').val(arrValue[0]);
        $('#<%=hdnListNursingTransactionEvaluationID.ClientID %>').val(arrValue[1]);
        $('#<%=hdnListNursingEvaluationDiagnoseItemText.ClientID %>').val(arrValue[2]);
        $('#<%=hdnListIsEvaluationEditedByUser.ClientID %>').val(arrValue[3]);

        $('#<%=hdnListOutcomeDtID.ClientID %>').val(arrValue[4]);
        $('#<%=hdnListScaleScoreEvaluation.ClientID %>').val(arrValue[5]);

        $('#<%=hdnListNursingInterventionEvaluation.ClientID %>').val(arrValue[6]);
        $('#<%=hdnListIsContinued.ClientID %>').val(arrValue[7]);
    }

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
            <dxcp:ASPxCallbackPanel ID="cbpViewEvaluation" runat="server" Width="100%" ClientInstanceName="cbpViewEvaluation"
                ShowLoadingPanel="false" OnCallback="cbpViewEvaluation_Callback">
                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                    EndCallback="function(s,e){ onCbpViewEvaluationEndCallback(s); }" />
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent1" runat="server">
                        <input type="hidden" id="hdnListNursingEvaluationDiagnoseItemID" runat="server" />
                        <input type="hidden" id="hdnListNursingTransactionEvaluationID" runat="server" />
                        <input type="hidden" id="hdnListNursingEvaluationDiagnoseItemText" runat="server" />
                        <input type="hidden" id="hdnListIsEvaluationEditedByUser" runat="server" />
                        <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                            <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="StandardCodeID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                    <asp:BoundField DataField="StandardCodeName" HeaderText="Jenis Evaluasi" />
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
            <div class="dvSubjectiveObjective">
                <dxcp:ASPxCallbackPanel ID="cbpViewEvaluation1" runat="server" Width="100%" ClientInstanceName="cbpViewEvaluation1"
                    ShowLoadingPanel="false" OnCallback="cbpViewEvaluation1_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                        EndCallback="function(s,e){ onCbpViewEvaluation1EndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server">
                            <asp:Panel runat="server" ID="pnlView1" CssClass="pnlContainerGrid">
                                
                                <input type="hidden" id="hdnListSaveNursingEvaluationDiagnoseItemID" runat="server" />
                                <input type="hidden" id="hdnListSaveNursingTransactionEvaluationID" runat="server" />
                                <input type="hidden" id="hdnListSaveNursingEvaluationDiagnoseItemText" runat="server" />
                                <input type="hidden" id="hdnListSaveIsEvaluationEditedByUser" runat="server" />
                                <center><asp:TextBox runat="server" ID="txtNewDiagnoseItem" Width="95%"></asp:TextBox></center>
                                <asp:GridView ID="grdView1" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView1_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="NursingDiagnoseItemID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:CheckBox runat="server" ID="chkIsSelectedEvaluation" class="chkIsSelectedEvaluation" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Diagnosis Item">
                                            <ItemTemplate>
                                                <div runat="server" id="divLblNursingItemText" class="divLblNursingItemText">
                                                    <input type="hidden" class="hdnKeyField2" runat="server" value='<%#: Eval("NursingTransactionEvaluationDt") %>' />
                                                    <asp:Label runat="server" ID="lblNursingItemText" Text='<%#: Eval("NursingItemText") %>' class="lblNursingItemText"/>
                                                </div>
                                                <div runat="server" id="divTxtNursingItemText" class="divTxtNursingItemText" style="display:none">
                                                    <asp:TextBox runat="server" ID="txtNursingItemText" Text='<%#: Eval("NursingItemText") %>' Width="100%" class="txtNursingItemText" />
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Edit" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <div>
                                                    <asp:CheckBox runat="server" ID="chkIsEvaluationEditedByUser" class="chkIsEvaluationEditedByUser" />
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
            </div> 
            <div class="dvAssessment">
                <dxcp:ASPxCallbackPanel ID="cbpViewEvaluation2" runat="server" Width="100%" ClientInstanceName="cbpViewEvaluation2"
                    ShowLoadingPanel="false" OnCallback="cbpViewEvaluation2_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                        EndCallback="function(s,e){ onCbpViewEvaluation2EndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent3" runat="server">
                            <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGrid">
                                <input type="hidden" id="hdnListOutcomeDtID" runat="server" />
                                <input type="hidden" id="hdnListScaleScoreEvaluation" runat="server" />
                                <asp:GridView ID="grdViewAssessment" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" 
                                    OnRowDataBound="grdViewAssessment_RowDataBound" OnRowCreated="grdViewAssessment_RowCreated">
                                    <Columns>
                                    <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                    <asp:BoundField HeaderText="Item Indikator" DataField="NursingIndicatorText" ItemStyle-CssClass="indicatorTextEvaluation" />
                                    <asp:BoundField HeaderText="Target" DataField="ScaleScore" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center" />
                                    <asp:TemplateField HeaderStyle-Width="500px" HeaderText="Evaluasi">
                                        <ItemTemplate>
                                            <table>
                                                <colgroup>
                                                    <col width="100px" />
                                                    <col width="100px" />
                                                    <col width="100px" />
                                                    <col width="100px" />
                                                    <col width="100px" />
                                                </colgroup>
                                                <tr valign="top">
                                                    <td><asp:RadioButton ID="rbtScale1Text" runat="server" CssClass="rbtScale1Text" GroupName="ScaleText" Text='<%#: "1-" + Eval("Scale1Text") %>' /></td>
                                                    <td><asp:RadioButton ID="rbtScale2Text" runat="server" CssClass="rbtScale2Text" GroupName="ScaleText" Text='<%#: "2-" + Eval("Scale2Text") %>' /></td>
                                                    <td><asp:RadioButton ID="rbtScale3Text" runat="server" CssClass="rbtScale3Text" GroupName="ScaleText" Text='<%#: "3-" + Eval("Scale3Text") %>' /></td>
                                                    <td><asp:RadioButton ID="rbtScale4Text" runat="server" CssClass="rbtScale4Text" GroupName="ScaleText" Text='<%#: "4-" + Eval("Scale4Text") %>' /></td>
                                                    <td><asp:RadioButton ID="rbtScale5Text" runat="server" CssClass="rbtScale5Text" GroupName="ScaleText" Text='<%#: "5-" + Eval("Scale5Text") %>' /></td>
                                                </tr>
                                            </table>
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
            </div>
             <div class="dvPlanning">
                <dxcp:ASPxCallbackPanel ID="cbpViewEvaluation3" runat="server" Width="100%" ClientInstanceName="cbpViewEvaluation3"
                    ShowLoadingPanel="false" OnCallback="cbpViewEvaluation3_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                        EndCallback="function(s,e){ onCbpViewEvaluation3EndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent4" runat="server">
                            <asp:Panel runat="server" ID="Panel2" CssClass="pnlContainerGrid">
                                <input type="hidden" id="hdnListNursingInterventionEvaluation" runat="server" />
                                <input type="hidden" id="hdnListIsContinued" runat="server" />
                                <asp:GridView ID="grdViewPlanning" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" 
                                    OnRowDataBound="grdViewPlanning_RowDataBound" OnRowCreated="grdViewPlanning_RowCreated">
                                    <Columns>
                                    <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                    <asp:BoundField HeaderText="Kode" DataField="DisplayOrder" HeaderStyle-Width="100px" />
                                    <asp:BoundField HeaderText="Item Indikator" DataField="NursingItemText" ItemStyle-CssClass="interventionItemEvaluation" />
                                    <asp:TemplateField HeaderStyle-Width="200px" HeaderText="Evaluasi" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <table>
                                                <colgroup>
                                                    <col width="100px" />
                                                    <col width="100px" />
                                                </colgroup>
                                                <tr valign="top">
                                                    <td><asp:RadioButton ID="rbtContinue" runat="server" CssClass="rbtContinue" GroupName="ContinueIntervention" Text='Lanjutkan' /></td>
                                                    <td><asp:RadioButton ID="rbtNotContinue" runat="server" CssClass="rbtNotContinue" GroupName="ContinueIntervention" Text='Hentikan' /></td>
                                                </tr>
                                            </table>
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
            </div>
        </td>
    </tr>
</table>