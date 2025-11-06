<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuestionEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.SystemSetup.Program.QuestionEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_questionentryctl">

    //#region Parent
    function getParentFilterExpression() {
        var filterExpression = "IsDeleted = 0";
        return filterExpression;
    }

    $('#lblAnswer.lblLink').click(function () {
        openSearchDialog('Answer', getParentFilterExpression(), function (value) {
            $('#<%=txtAnswerCode.ClientID %>').val(value);
            onTxtAnswerCodeChanged(value);
        });
    });

    $('#<%=txtAnswerCode.ClientID %>').change(function () {
        onTxtAnswerCodeChanged($(this).val());
    });

    function onTxtAnswerCodeChanged(value) {
        var filterExpression = getParentFilterExpression() + " AND AnswerCode = '" + value + "'";
        Methods.getObject('GetAnswerList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnAnswerID.ClientID %>').val(result.AnswerID);
                $('#<%=txtAnswerName.ClientID %>').val(result.AnswerText1);
            }
            else {
                $('#<%=hdnAnswerID.ClientID %>').val('');
                $('#<%=txtAnswerCode.ClientID %>').val('');
                $('#<%=txtAnswerName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=txtQuestionID.ClientID %>').val('');
        $('#<%=txtQuestionAnswerCode.ClientID %>').val('');
        $('#<%=hdnAnswerID.ClientID %>').val('');
        $('#<%=txtDisplayOrder.ClientID %>').val('0');
        $('#<%=hdnQuestionAnswerID.ClientID %>').val('');
        $('#containerPopupEntryData').show();
    });

    $('#btnEntryPopupCancel').live('click', function () {
        $('#containerPopupEntryData').hide();
    });

    $('#btnEntryPopupSave').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
            cbpEntryPopupView.PerformCallback('save');
        return false;
    });

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function () {
        if (confirm("Are You Sure Want To Delete This Data?")) {
            $row = $(this).closest('tr');
            var QuestionAnswerID = $row.find('.hdnQuestionAnswerID').val();
            $('#<%=hdnQuestionAnswerID.ClientID %>').val(QuestionAnswerID);

            cbpEntryPopupView.PerformCallback('delete');
        }
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $('#<%=hdnIsAdd.ClientID %>').val('0');
        $row = $(this).closest('tr');
        var QuestionAnswerID = $row.find('.hdnQuestionAnswerID').val();
        var AnswerID = $row.find('.hdnAnswerID').val();
        var AnswerCode = $row.find('.hdnAnswerCode').val();
        var AnswerName = $row.find('.hdnAnswerName').val();
        var DisplayOrder = $row.find('.hdnDisplayOrder').val();

        $('#<%=hdnQuestionAnswerID.ClientID %>').val(QuestionAnswerID);
        $('#<%=hdnAnswerID.ClientID %>').val(AnswerID);
        $('#<%=txtAnswerCode.ClientID %>').val(AnswerCode);
        $('#<%=txtAnswerName.ClientID %>').val(AnswerName);
        $('#<%=txtDisplayOrder.ClientID %>').val(DisplayOrder);

        $('#containerPopupEntryData').show();
    });

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
</script>
<div style="height:440px; overflow-y:auto;overflow-x: hidden">
    <input type="hidden" id="hdnQuestionID" value="" runat="server" />
    <input type="hidden" id="hdnQuestionAnswerID" value="" runat="server" />
    <input type="hidden" value="" id="hdnIsAdd" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 70%">
                    <colgroup>
                        <col style="width:160"/>
                        <col/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Kode")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtQuestionCode" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nama")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtQuestionName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
                <div id="containerPopupEntryData" style=" display: none;">
                    <div class="pageTitle">
                        <%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width:110px"/>
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink lblMandatory" id="lblAnswer">
                                        <%=GetLabel("Jawaban")%></label>
                                </td>
                                <td colspan="2">
                                  <input type="hidden" value="" runat="server" id="hdnAnswerID" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 100px" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtAnswerCode" CssClass="required" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtAnswerName" ReadOnly="true" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                             <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Display Order")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDisplayOrder" Width="100px" runat="server" />
                                </td>
                                </tr>
                                   <tr style="display:none">
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%=GetLabel("Question AnswerID")%></label>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtQuestionAnswerCode" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                            <tr style="display:none">
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Question ID")%></label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtQuestionID" Width="100%" runat="server" />
                                </td>
                            </tr>
                             <tr style="display:none">
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("AnswerID")%></label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtAnswerID" Width="100%" runat="server" />
                                </td>
                            </tr>
                             <tr style="display:none">
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Urutan Cetak")%></label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtUrutanCetak" Width="100%" runat="server" />
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
                                                <input type="button" id="btnEntryPopupCancel" value='<%= GetLabel("Batal")%>' class="w3-btn w3-hover-blue"/>
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
                                                
                                                <input type="hidden" class="hdnQuestionAnswerID" value="<%#: Eval("QuestionAnswerID")%>" />
                                                <input type="hidden" class="hdnAnswerID" value="<%#: Eval("AnswerID")%>" />
                                                <input type="hidden" class="hdnAnswerCode" value="<%#: Eval("AnswerCode")%>" />
                                                <input type="hidden" class="hdnAnswerName" value="<%#: Eval("AnswerText1")%>" />
                                                <input type="hidden" class="hdnDisplayOrder" value="<%#: Eval("DisplayOrder")%>" />
                                                
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                       <asp:BoundField DataField="AnswerText1" ItemStyle-CssClass="tdQuestionAnswerID"
                                            HeaderText="Answer Text 1" />
                                        <asp:BoundField HeaderStyle-Width="175px" DataField="DisplayOrder" HeaderText="Urutan Cetak"
                                            ItemStyle-CssClass="tdDisplayOrder" />
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
