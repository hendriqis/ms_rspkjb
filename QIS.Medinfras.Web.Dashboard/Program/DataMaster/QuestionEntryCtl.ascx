<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuestionEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.SystemSetup.Program.QuestionEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_QuestionEntryCtl">
    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=hdnQuestionID.ClientID %>').val('');
        $('#<%=txtQuestionCode.ClientID %>').val('');
        $('#<%=txtQuestionName.ClientID %>').val('');
        $('#<%=txtQuestionNotes.ClientID %>').val('');
        $('#<%=txtAnswer1.ClientID %>').val('');
        $('#<%=txtAnswer2.ClientID %>').val('');
        $('#<%=txtAnswer3.ClientID %>').val('');
        $('#<%=txtAnswer4.ClientID %>').val('');
        $('#<%=txtAnswer5.ClientID %>').val('');

        $('#containerPopupEntryData').show();
    });

    $('#btnEntryPopupCancel').live('click', function () {
        $('#containerPopupEntryData').hide();
    });

    $('#btnEntryPopupSave').click(function (evt) {
        var Jawaban1 = $('#<%=txtAnswer1.ClientID %>').val();
        var Jawaban2 = $('#<%=txtAnswer2.ClientID %>').val();
        var Jawaban3 = $('#<%=txtAnswer3.ClientID %>').val();
        var Jawaban4 = $('#<%=txtAnswer4.ClientID %>').val();
        var Jawaban5 = $('#<%=txtAnswer5.ClientID %>').val();
        var QType = cboQuestionType.GetValue();
        if (QType != "X553^003") {
            if (Jawaban1 == "" && Jawaban2 == "" && Jawaban3 == "" && Jawaban4 == "" && Jawaban5 == "") {
                showToast('Warning', 'Minimal Terdapat 1 Jawaban Survey Untuk Tipe Pertanyaan Multiple-Choice');
            }
            else if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup')) {
                cbpEntryPopupView.PerformCallback('save');
                return false;
            }
        } else if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
            cbpEntryPopupView.PerformCallback('save');
        return false;
    });

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function () {
        if (confirm("Are You Sure Want To Delete This Data?")) {
            $row = $(this).closest('tr');
            var QuestionID = $row.find('.hdnQuestionID').val();
            $('#<%=hdnQuestionID.ClientID %>').val(QuestionID);

            cbpEntryPopupView.PerformCallback('delete');
        }
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var QuestionID = $row.find('.hdnQuestionID').val();
        var QuestionCode = $row.find('.hdnQuestionCode').val();
        var QuestionName = $row.find('.hdnQuestionName').val();
        var QuestionNotes = $row.find('.hdnQuestionNotes').val();

        $('#<%=hdnQuestionID.ClientID %>').val(QuestionID);
        $('#<%=txtQuestionCode.ClientID %>').val(QuestionCode);
        $('#<%=txtQuestionName.ClientID %>').val(QuestionName);
        $('#<%=txtQuestionNotes.ClientID %>').val(QuestionNotes);
        

        $('#containerPopupEntryData').show();
    });

    //#endregion

    function onCbpEntryPopupViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else
                $('#containerPopupEntryData').hide();
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
        }
        $('#containerImgLoadingViewPopup').hide();
    }

    //#region AnswerField
    function onCboQuestionTypeChanged() {
        var cboValue = cboQuestionType.GetValue();
        if (cboValue == 'X553^003') {
            $('#trAnswerText1').attr('style', 'display:none');
            $('#trAnswerText2').attr('style', 'display:none');
            $('#trAnswerText3').attr('style', 'display:none');
            $('#trAnswerText4').attr('style', 'display:none');
            $('#trAnswerText5').attr('style', 'display:none');
        }
        else {
            $('#trAnswerText1').removeAttr('style');
            $('#trAnswerText2').removeAttr('style');
            $('#trAnswerText3').removeAttr('style');
            $('#trAnswerText4').removeAttr('style');
            $('#trAnswerText5').removeAttr('style');
        }
    }
    //#endregion
</script>
<div style="height: 450px; overflow-y: auto">
    <input type="hidden" id="hdnSurveyID" value="" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 70%">
                    <colgroup>
                        <col style="width: 120px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel>
                            <label class="lblNormal">
                                <%=GetLabel("Kode")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtSurveyCode" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nama")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtSurveyName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
                <div id="containerPopupEntryData" style="margin-top: 10px; display: none;">
                    <input type="hidden" id="hdnQuestionID" runat="server" value="" />
                    <div class="pageTitle">
                        <%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 200px" />
                                <col style="width: 100px" />
                                <col style="width: 600px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal lblMandatory">
                                        <%=GetLabel("Kode Pertanyaan")%></label>
                                </td>
                                <td colspan="1">
                                    <asp:TextBox ID="txtQuestionCode" CssClass="required" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr id="trDepartment" runat="server">
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Tipe Pertanyaan")%></label>
                                </td>
                                <td colspan="2">
                                    <dxe:ASPxComboBox ID="cboQuestionType" ClientInstanceName="cboQuestionType" runat="server"
                                        Width="100%" TextField="OptionValue" ValueField="OptionValue" ValueType="System.String">
                                        <ClientSideEvents ValueChanged="function(s,e) { onCboQuestionTypeChanged(); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal lblMandatory">
                                        <%=GetLabel("Pertanyaan")%></label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtQuestionName" CssClass="required" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr id="trAnswerText1">
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Jawaban 1")%></label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtAnswer1" CssClass="required" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr id="trAnswerText2">
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Jawaban 2")%></label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtAnswer2" CssClass="required" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr id="trAnswerText3">
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Jawaban 3")%></label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtAnswer3" CssClass="required" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr id="trAnswerText4">
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Jawaban 4")%></label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtAnswer4" CssClass="required" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr id="trAnswerText5">
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Jawaban 5")%></label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtAnswer5" CssClass="required" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Catatan")%></label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtQuestionNotes" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("Simpan")%>' class="w3-btn w3-hover-blue" />
                                            </td>
                                            <td>
                                                <input type="button" id="btnEntryPopupCancel" value='<%= GetLabel("Batal")%>' class="w3-btn w3-hover-blue" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewPopup').show(); }"
                        EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlPatientVisitTransHdGrdView" Style="width: 100%;
                                margin-left: auto; margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                    OnRowDataBound="grdView_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                    alt="" style="float: left; margin-left: 7px" />
                                                <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                    alt="" />
                                                <input type="hidden" class="hdnQuestionID" value="<%#: Eval("QuestionID")%>" />
                                                <input type="hidden" class="hdnQuestionCode" value="<%#: Eval("QuestionCode")%>" />
                                                <input type="hidden" class="hdnQuestionName" value="<%#: Eval("QuestionName")%>" />
                                                <input type="hidden" class="hdnQuestionNotes" value="<%#: Eval("QuestionNotes")%>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderStyle-Width="120px" DataField="QuestionCode" ItemStyle-CssClass="tdQuestionCode"
                                            HeaderText="Kode" />
                                        <asp:BoundField DataField="QuestionName" HeaderText="Pertanyaan"
                                            ItemStyle-CssClass="tdLocationName" />
                                        <asp:BoundField HeaderStyle-Width="400px" DataField="QuestionNotes" HeaderText="Catatan"
                                            ItemStyle-CssClass="tdLogisticLocationName" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Data Tidak Tersedia")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div style="width: 100%; text-align: center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblEntryPopupAddData">
                        <%= GetLabel("Tambah Data")%></span>
                </div>
            </td>
        </tr>
    </table>
</div>
