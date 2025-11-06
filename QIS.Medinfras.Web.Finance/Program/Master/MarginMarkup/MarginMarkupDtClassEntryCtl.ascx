<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MarginMarkupDtClassEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.MarginMarkupDtClassEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_marginmarkupdtclassentry">

    //#region Class Care
    function getFilterClassCare() {
        var MarkupID = $('#<%:hdnMarkupIDCtl.ClientID %>').val();
        var SequenceNo = $('#<%:hdnSequenceNoCtl.ClientID %>').val();
        var filter = "IsUsedInChargeClass = 1 AND IsDeleted = 0 AND ClassID NOT IN (SELECT ClassID FROM MarginMarkupDtClass WHERE IsDeleted = 0 AND MarkupID = " + MarkupID + " AND SequenceNo = " + SequenceNo + ")";
        return filter;
    }

    $('#<%:lblClass.ClientID %>.lblLink').live('click', function () {
        openSearchDialog('classcare', getFilterClassCare(), function (value) {
            $('#<%:txtClassCode.ClientID %>').val(value);
            onTxtClassCodeChanged(value);
        });
    });

    $('#<%:txtClassCode.ClientID %>').live('change', function () {
        onTxtClassCodeChanged($(this).val());
    });

    function onTxtClassCodeChanged(value) {
        var filterExpression = getFilterClassCare() + " AND ClassCode = '" + value + "'";
        Methods.getObject('GetClassCareList', filterExpression, function (result) {
            if (result != null) {
                $('#<%:hdnClassID.ClientID %>').val(result.ClassID);
                $('#<%:txtClassName.ClientID %>').val(result.ClassName);
            }
            else {
                $('#<%:hdnClassID.ClientID %>').val('');
                $('#<%:txtClassCode.ClientID %>').val('');
                $('#<%:txtClassName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    $('#lblEntryClassPopupAddData').live('click', function () {
        $('#<%=hdnIDCtl.ClientID %>').val('');
        $('#<%=hdnClassID.ClientID %>').val('');
        $('#<%:txtClassCode.ClientID %>').val('');
        $('#<%:txtClassName.ClientID %>').val('');
        $('#<%=txtMarkupAmount.ClientID %>').val('0').trigger('changeValue');

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
            var ID = $row.find('.ID').val();

            $('#<%=hdnIDCtl.ClientID %>').val(ID);
            cbpEntryPopupView.PerformCallback('delete');
        }
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var ID = $row.find('.ID').val();
        var ClassID = $row.find('.ClassID').val();
        var ClassCode = $row.find('.ClassCode').val();
        var ClassName = $row.find('.ClassName').val();
        var MarkupAmount = $row.find('.MarkupAmount').val();

        $('#<%=hdnIDCtl.ClientID %>').val(ID);
        $('#<%=hdnClassID.ClientID %>').val(ClassID);
        $('#<%=txtClassCode.ClientID %>').val(ClassCode);
        $('#<%=txtClassName.ClientID %>').val(ClassName);
        $('#<%=txtMarkupAmount.ClientID %>').val(MarkupAmount).trigger('changeValue');

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
<div style="width: 600px; height:500px; overflow-y:auto">
    <input type="hidden" id="hdnMarkupIDCtl" value="" runat="server" />
    <input type="hidden" id="hdnSequenceNoCtl" value="" runat="server" />
    <input type="hidden" id="hdnIDCtl" value="" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td>
                <table style="width: 100%">
                    <colgroup>
                        <col style="width: 160px" />
                        <col style="width: 3px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Kode Markup")%></label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtMarkupCodeCtl" Width="100px" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nama Markup")%></label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtMarkupNameCtl" Width="250px" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Sequence")%></label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtSequenceNoCtl" Width="100px" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                </table>
                <div id="containerPopupEntryData" style="display: none;">
                    <div class="pageTitle">
                        <%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 160px" />
                                <col style="width: 3px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink lblMandatory" runat="server" id="lblClass">
                                        <%:GetLabel("Kelas")%></label>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <input type="hidden" id="hdnClassID" value="" runat="server" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 80px" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtClassCode" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtClassName" Width="300px" runat="server" ReadOnly="true" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Margin Default")%></label>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMarkupAmount" CssClass="required txtCurrency" Width="200px" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("Simpan")%>' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnEntryPopupCancel" value='<%= GetLabel("Batal")%>' />
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
                            <asp:Panel runat="server" ID="pnlMarginMarkupDtClass" Style="width: 100%; font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                    OnRowDataBound="grdView_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                    alt="" style="float: left; margin-left: 7px" />
                                                <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                    alt="" />
                                                <input type="hidden" class="ID" value="<%#: Eval("ID")%>" />
                                                <input type="hidden" class="SequenceNo" value="<%#: Eval("SequenceNo")%>" />
                                                <input type="hidden" class="ClassID" value="<%#: Eval("ClassID")%>" />
                                                <input type="hidden" class="ClassCode" value="<%#: Eval("ClassCode")%>" />
                                                <input type="hidden" class="ClassName" value="<%#: Eval("ClassName")%>" />
                                                <input type="hidden" class="MarkupAmount" value="<%#: Eval("MarkupAmount")%>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ClassName" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" HeaderText="Class Name" />
                                        <asp:BoundField DataField="MarkupAmount" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Right"
                                            ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" HeaderText="Margin Default"
                                            ItemStyle-CssClass="tdMarkupAmount" />
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
                    <span class="lblLink" id="lblEntryClassPopupAddData">
                        <%= GetLabel("Tambah Data")%></span>
                </div>
            </td>
        </tr>
    </table>
</div>
