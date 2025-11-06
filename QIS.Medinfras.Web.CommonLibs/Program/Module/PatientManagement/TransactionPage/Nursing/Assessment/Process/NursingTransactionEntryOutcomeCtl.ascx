<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NursingTransactionEntryOutcomeCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.NursingTransactionEntryOutcomeCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript">
    var iRowIndex2 = 0;

    $('#<%=grdView.ClientID %> tr:gt(0)').live('click', function () {
        iRowIndex2 = $(this).closest("tr").prevAll("tr").length;
        $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
        getCheckedOutcomeItem("");
        cbpViewOutcome1.PerformCallback('refresh');
    });

    function oncbpViewOutcomeEndCallback(s) {
        if (iRowIndex2 > 0) {
            $("#<%=grdView.ClientID %> tr:eq(" + iRowIndex2 + ")").addClass('selected');
        }
        hideLoadingPanel();
    }

    function oncbpViewOutcome1EndCallback(s) {
        $('#<%=hdnOldID.ClientID %>').val($('#<%=hdnID.ClientID %>').val());

        onAfterSaveRecordDtSuccess(s.cpTransactionID);

        cbpViewOutcome.PerformCallback('refresh');
        cbpViewIntervention.PerformCallback('refresh');

        hideLoadingPanel();
    }

    function onRefreshParentGrid() {
        cbpViewOutcome.PerformCallback('refresh');
        cbpViewIntervention.PerformCallback('refresh');

        hideLoadingPanel();
    }

    function resetCheckedOutcomeItem() {
        $('#<%=hdnListNursingDiagnoseItemIndicatorID.ClientID %>').val('|');
        $('#<%=hdnListScaleScore.ClientID %>').val('|');
        $('#<%=hdnListRemarks.ClientID %>').val('|');
    }

    function getCheckedOutcomeItem(errMessage) {
        var lstNursingDiagnoseItemIndicatorID = $('#<%=hdnListNursingDiagnoseItemIndicatorID.ClientID %>').val().split('|');
        var lstScaleScore = $('#<%=hdnListScaleScore.ClientID %>').val().split('|');
        var lstRemarks = $('#<%=hdnListRemarks.ClientID %>').val().split('|');

        $('.grdView1 .chkIsSelectedOutcome input').each(function () {
            if ($(this).is(':checked')) {
                $tr = $(this).closest('tr');
                var key = $tr.find('.keyField').html();
                var remarks = $tr.find('.txtRemarksOutcome').val();
                var score1 = $tr.find('.rbtScale1Text input').is(':checked');
                var score2 = $tr.find('.rbtScale2Text input').is(':checked');
                var score3 = $tr.find('.rbtScale3Text input').is(':checked');
                var score4 = $tr.find('.rbtScale4Text input').is(':checked');
                var score5 = $tr.find('.rbtScale5Text input').is(':checked');
                var idx = lstNursingDiagnoseItemIndicatorID.indexOf(key);
                if (idx < 0) {
                    lstNursingDiagnoseItemIndicatorID.push(key);
                    if (score1)
                        lstScaleScore.push("1");
                    else if (score2)
                        lstScaleScore.push("2");
                    else if (score3)
                        lstScaleScore.push("3");
                    else if (score4)
                        lstScaleScore.push("4");
                    else if (score5)
                        lstScaleScore.push("5");
                    else
                        lstScaleScore.push("0");
                    lstRemarks.push(remarks);
                }
                else {
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
                    lstRemarks[idx] = remarks;
                }
            }
            else {
                var key = $(this).closest('tr').find('.keyField').html();
                var idx = lstNursingDiagnoseItemIndicatorID.indexOf(key);
                if (idx > -1) {
                    lstNursingDiagnoseItemIndicatorID.splice(idx, 1);
                    lstScaleScore.splice(idx, 1);
                    lstRemarks.splice(idx, 1);
                }
            }
        });

        if (lstNursingDiagnoseItemIndicatorID.length > 2 && lstNursingDiagnoseItemIndicatorID[1] == '') {
            lstNursingDiagnoseItemIndicatorID.splice(0, 1);
            lstScaleScore.splice(0, 1);
            lstRemarks.splice(0, 1);
        }
        $('#<%=hdnListNursingDiagnoseItemIndicatorID.ClientID %>').val(lstNursingDiagnoseItemIndicatorID.join('|'));
        $('#<%=hdnListScaleScore.ClientID %>').val(lstScaleScore.join('|'));
        $('#<%=hdnListRemarks.ClientID %>').val(lstRemarks.join('|'));

        if ($('#<%=hdnListNursingDiagnoseItemIndicatorID.ClientID %>').val() == '')
            resetCheckedOutcomeItem();
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
            <dxcp:ASPxCallbackPanel ID="cbpViewOutcome" runat="server" Width="100%" ClientInstanceName="cbpViewOutcome"
                ShowLoadingPanel="false" OnCallback="cbpViewOutcome_Callback">
                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ oncbpViewOutcomeEndCallback(s); }" />
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent1" runat="server">
                        <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                            <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="NursingDiagnoseItemID" HeaderStyle-CssClass="keyField"
                                        ItemStyle-CssClass="keyField" />
                                    <asp:BoundField DataField="NursingItemText" HeaderText="Deskripsi Luaran" HeaderStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="NursingOutcomeResult" HeaderText="Ekspektasi" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="100px" />
                                    <asp:TemplateField HeaderText="Item" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblTotalSelectedOutcome"></asp:Label>
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
            <dxcp:ASPxCallbackPanel ID="cbpViewOutcome1" runat="server" Width="100%" ClientInstanceName="cbpViewOutcome1"
                ShowLoadingPanel="false" OnCallback="cbpViewOutcome1_Callback">
                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ oncbpViewOutcome1EndCallback(s); }" />
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent2" runat="server">
                        <asp:Panel runat="server" ID="pnlView1" CssClass="pnlContainerGrid">
                            <input type="hidden" id="hdnListNursingDiagnoseItemIndicatorID" runat="server" />
                            <input type="hidden" id="hdnListScaleScore" runat="server" />
                            <input type="hidden" id="hdnListRemarks" runat="server" />
                            <fieldset id="fsTrx" style="margin: 0">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" Style="font-weight: bold"><%=GetLabel("Setelah dilakukan tindakan keperawatan selama ")%></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtNOCInterval" Width="20px" CssClass="txtNumeric"
                                                Style="text-align: center; color: Red" Text="0"></asp:TextBox>
                                        </td>
                                        <td>
                                            <dxe:ASPxComboBox ID="cboIntervalType" ClientInstanceName="cboIntervalType" runat="server"
                                                Width="60px" ItemStyle-ForeColor="Red">
                                            </dxe:ASPxComboBox>
                                        </td>
                                        <td>
                                            <asp:Label runat="server" Style="font-weight: bold"><%=GetLabel(" masalah teratasi dengan kriteria")%></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                            <asp:GridView ID="grdView1" runat="server" CssClass="grdSelected grdView1" AutoGenerateColumns="false"
                                ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView1_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="NursingDiagnoseItemIndicatorID" HeaderStyle-CssClass="keyField"
                                        ItemStyle-CssClass="keyField" />
                                    <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkIsSelectedOutcome" class="chkIsSelectedOutcome" />
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
                                                        <asp:RadioButton ID="rbtScale1Text" runat="server" CssClass="rbtScale1Text" GroupName="ScaleText"
                                                            Text='<%#: "1-" + Eval("Scale1Text") %>' />
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rbtScale2Text" runat="server" CssClass="rbtScale2Text" GroupName="ScaleText"
                                                            Text='<%#: "2-" + Eval("Scale2Text") %>' />
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rbtScale3Text" runat="server" CssClass="rbtScale3Text" GroupName="ScaleText"
                                                            Text='<%#: "3-" + Eval("Scale3Text") %>' />
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rbtScale4Text" runat="server" CssClass="rbtScale4Text" GroupName="ScaleText"
                                                            Text='<%#: "4-" + Eval("Scale4Text") %>' />
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="rbtScale5Text" runat="server" CssClass="rbtScale5Text" GroupName="ScaleText"
                                                            Text='<%#: "5-" + Eval("Scale5Text") %>' />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ket" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtRemarksOutcome" CssClass="txtRemarksOutcome" Width="100%"
                                                TextMode="MultiLine" Wrap="true" />
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
