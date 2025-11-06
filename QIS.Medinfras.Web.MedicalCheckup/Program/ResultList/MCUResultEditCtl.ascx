<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MCUResultEditCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.MedicalCheckup.Program.MCUResultEditCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_patientvisitnotesctl">
    $('#btnCancelCtl').click(function () {
        $('#containerEntryDataCtl').hide();
    });

    $('#btnSaveCtl').click(function (evt) {
        if (IsValid(evt, 'fsPatientVisitNotes', 'mpPatientVisitNotes'))
            cbpChangeResultCtl.PerformCallback('save');
        return false;
    });

    $('.imgEditCtl.imgLink').die('click')
    $('.imgEditCtl.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var entity = rowToObject($row);
        var result = entity.Result;

        $('#<%=hdnMCUResultID.ClientID %>').val(entity.ID);
        $('#<%=txtTestResult.ClientID %>').val(entity.TestResult);
        $('#<%=txtRemarks.ClientID %>').val(entity.RemarksORI);

        if (result == 'True') {
            $('#<%=chkIsSelected.ClientID %>').prop('checked', true);
        }
        else {
            $('#<%=chkIsSelected.ClientID %>').prop('checked', false);
        }

        $('#containerEntryDataCtl').show();
    });

    $('.imgDeleteCtl.imgLink').die('click');
    $('.imgDeleteCtl.imgLink').live('click', function (evt) {
        evt.stopPropagation();
        evt.preventDefault();
        if (confirm("Are You Sure Want To Delete This Data?")) {
            $row = $(this).closest('tr');
            var entity = rowToObject($row);
            $('#<%=hdnMCUResultID.ClientID %>').val(entity.ID);
            cbpChangeResultCtl.PerformCallback('delete');
        }
    });

    function onChangeResultCtlCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else
                $('#containerEntryDataCtl').hide();
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
        }
        hideLoadingPanel();
    }

    function oncboTestResultSearchCodeValueChanged() {
        onRefreshGridView();
    }

    function onRefreshGridView() {
        cbpChangeResultCtl.PerformCallback('refresh');
    }

    $('#btnCloseCtl').click(function (evt) {
        pcRightPanelContent.Hide();
        onAfterChangeResult();
    });
</script>
<div style="height: 440px; overflow-y: auto">
    <input type="hidden" value="" runat="server" id="hdnVisitID" />
    <input type="hidden" value="" runat="server" id="hdnGCGender" />
    <table class="tblContentArea">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 800px">
                    <colgroup>
                        <col style="width: 100px" />
                        <col style="width: 300px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <b>
                                    <%=GetLabel("No. Registrasi")%></label>
                            </b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRegistrationNo" ReadOnly="true" Width="160px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <b>
                                    <%=GetLabel("Nama Pasien")%></label>
                            </b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPatientName" ReadOnly="true" Width="560px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <b>
                                    <%=GetLabel("Pemeriksaan")%></label>
                            </b>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboTestResultSearchCode" ClientInstanceName="cboTestResultSearchCode"
                                Width="100%" runat="server">
                                <ClientSideEvents ValueChanged="function(s,e) { oncboTestResultSearchCodeValueChanged(); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                </table>
                <div id="containerEntryDataCtl" style="margin-top: 10px; display: none;">
                    <div class="pageTitle">
                        <%=GetLabel("Entry")%></div>
                    <input type="hidden" id="hdnMCUResultID" runat="server" value="" />
                    <fieldset id="fsPatientVisitNotes" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 10%" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <b>
                                        <%=GetLabel("Judul") %>
                                    </b>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTestResult" ReadOnly="true" CssClass="required" Width="70%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <b>
                                        <%=GetLabel("Hasil") %>
                                    </b>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <b>
                                        <%=GetLabel("Catatan") %>
                                    </b>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRemarks" CssClass="required" Width="70%" TextMode="MultiLine"
                                        Rows="4" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnSaveCtl" value='<%= GetLabel("Simpan")%>' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnCancelCtl" value='<%= GetLabel("Batal")%>' />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <dxcp:ASPxCallbackPanel ID="cbpChangeResultCtl" runat="server" Width="100%" ClientInstanceName="cbpChangeResultCtl"
                    ShowLoadingPanel="false" OnCallback="cbpChangeResultCtl_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onChangeResultCtlCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                                <div style="width: 100%; height: 400px; overflow: scroll">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="50%">
                                                <ItemTemplate>
                                                    <img class="imgLink imgEditCtl" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                        alt="" style="float: center; margin-right: 2px;" />
                                                    <img class="imgLink imgDeleteCtl" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                        alt="" style="float: center; margin-right: 2px;" />
                                                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                    <input type="hidden" value="<%#:Eval("GCTestResult") %>" bindingfield="GCTestResult" />
                                                    <input type="hidden" value="<%#:Eval("TestResult") %>" bindingfield="TestResult" />
                                                    <input type="hidden" value="<%#:Eval("Result") %>" bindingfield="Result" />
                                                    <input type="hidden" value="<%#:Eval("RemarksORI") %>" bindingfield="RemarksORI" />
                                                </ItemTemplate>
                                                <HeaderStyle Width="7%"></HeaderStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderStyle-Width="50%">
                                                <HeaderTemplate>
                                                    <div style="text-align: left; padding-left: 3px">
                                                        <%=GetLabel("Judul")%>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="padding: 3px">
                                                        <div>
                                                            <%#: Eval("TestResult")%></div>
                                                    </div>
                                                </ItemTemplate>
                                                <ItemStyle VerticalAlign="Top"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderStyle-Width="5%">
                                                <HeaderTemplate>
                                                    <div style="text-align: center; padding-left: 3px">
                                                        <%=GetLabel("")%>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="text-align: center; padding: 3px">
                                                        <div>
                                                            <asp:CheckBox ID="chkIsSelectedCtl" CssClass="chkIsSelected" Checked='<%# bool.Parse(Eval("Result").ToString() == "True" ? "True": "False") %>'
                                                                Enabled="false" runat="server" />
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                                <ItemStyle VerticalAlign="Top"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderStyle-Width="50%">
                                                <HeaderTemplate>
                                                    <div style="text-align: left; padding-left: 3px">
                                                        <%=GetLabel("Catatan")%>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="padding: 3px">
                                                        <div>
                                                            <%#: Eval("RemarksORI")%></div>
                                                    </div>
                                                </ItemTemplate>
                                                <ItemStyle VerticalAlign="Top"></ItemStyle>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div class="imgLoadingGrdView" id="containerImgLoadingPatientFamily">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Close")%>' id="btnCloseCtl" style="width: 100px" />
    </div>
</div>
