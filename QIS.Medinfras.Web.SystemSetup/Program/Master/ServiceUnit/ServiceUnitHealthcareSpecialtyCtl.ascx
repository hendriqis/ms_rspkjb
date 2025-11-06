<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServiceUnitHealthcareSpecialtyCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.SystemSetup.Program.ServiceUnitHealthcareSpecialtyCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_serviceunitroomentryctl">
    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=hdnID.ClientID %>').val('');
        $('#<%=txtBPJSCode.ClientID %>').val('');
        $('#<%=txtBPJSName.ClientID %>').val('');
        $('#<%=txtSpecialtyID.ClientID %>').val('');
        $('#<%=txtSpecialtyName.ClientID %>').val('');
        $('#containerPopupEntryData').show();
    });

    //#region SubPoliklinik
    $('#lblBPJSPOliKlinik.lblLink').click(function () {
        openSearchDialog('vklaimpoli', '', function (value) {
            $('#<%=txtBPJSCode.ClientID %>').val(value);
            ontxtvKlaimPoliCodeChanged(value);
        });
    });

    $('#<%=txtBPJSCode.ClientID %>').change(function () {
        ontxtvKlaimPoliCodeChanged($(this).val());
    });

    function ontxtvKlaimPoliCodeChanged(value) {
        var filterExpression = "BPJSCode = '" + value + "'";
        Methods.getObject('GetvBPJSReferencePoliList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=txtBPJSCode.ClientID %>').val(result.BPJSCode);
                $('#<%=txtBPJSName.ClientID %>').val(result.BPJSName);
            }
            else {
                $('#<%=txtBPJSCode.ClientID %>').val('');
                $('#<%=txtBPJSName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Spesilisasi
    $('#lblSpecialty.lblLink').click(function () {
        openSearchDialog('specialty', '', function (value) {
            $('#<%=txtSpecialtyID.ClientID %>').val(value);
            ontxtSpecialtyIDChanged(value);
        });
    });

    $('#<%=txtSpecialtyID.ClientID %>').change(function () {
        ontxtSpecialtyIDChanged($(this).val());
    });

    function ontxtSpecialtyIDChanged(value) {
        var filterExpression = "SpecialtyID = '" + value + "'";
        Methods.getObject('GetSpecialtyList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=txtSpecialtyID.ClientID %>').val(result.SpecialtyID);
                $('#<%=txtSpecialtyName.ClientID %>').val(result.SpecialtyName);
            }
            else {
                $('#<%=txtSpecialtyID.ClientID %>').val('');
                $('#<%=txtSpecialtyName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    $('#btnEntryPopupCancel').live('click', function () {
        $('#containerPopupEntryData').hide();
    });

    $('#btnEntryPopupSave').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup')) {
            cbpEntryPopupView.PerformCallback('save');
        }
    });

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function (evt) {
        evt.stopPropagation();
        evt.preventDefault();
        if (confirm("Are You Sure Want To Delete This Data?")) {
            $row = $(this).closest('tr');
            var id = $row.find('.hdnID').val();
            $('#<%=hdnID.ClientID %>').val(id);
            cbpEntryPopupView.PerformCallback('delete');
        }
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var ID = $row.find('.hdnID').val();
        var BPJSPoliCode = $row.find('.hdnBPJSPoliCode').val();
        var BPJSPoliName = $row.find('.hdnBPJSPoliName').val();
        var SpecialtyID = $row.find('.hdnSpecialtyID').val();
        var SpecialtyName = $row.find('.hdnSpecialtyName').val();

        $('#<%=hdnID.ClientID %>').val(ID);
        $('#<%=txtBPJSCode.ClientID %>').val(BPJSPoliCode);
        $('#<%=txtBPJSName.ClientID %>').val(BPJSPoliName);
        $('#<%=txtSpecialtyID.ClientID %>').val(SpecialtyID);
        $('#<%=txtSpecialtyName.ClientID %>').val(SpecialtyName);

        $('#containerPopupEntryData').show();
    });

    function setPagingDetailItem(pageCount) {
        if (pageCount > 0)
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        setPaging($("#pagingPopup"), pageCount, function (page) {
            cbpEntryPopupView.PerformCallback('changepage|' + page);
        }, 8);
    }

    function onCbpEntryPopupViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else {
                var pageCount = parseInt(param[2]);
                setPagingDetailItem(pageCount);
                $('#containerPopupEntryData').hide();

            }
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
            else {
                var pageCount = parseInt(param[2]);
                setPagingDetailItem(pageCount);
            }
        }
        else if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPagingDetailItem(pageCount);
        }
        else {
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        hideLoadingPanel();
    }

    //#region Paging
    var pageCountAvailable = parseInt('<%=PageCount %>');
    $(function () {
        setPagingDetailItem(pageCountAvailable);
    });
    //#endregion   
</script>
<div style="height: 450px; overflow-y: auto">
    <input type="hidden" id="hdnHealthcareServiceUnitID" value="" runat="server" />
    <div class="pageTitle">
        <%=GetLabel("Sub Klinik")%></div>
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 70%">
                    <colgroup>
                        <col style="width: 160px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Unit Pelayanan")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtServiceUnitName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Rumah Sakit")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtHealthcareName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
                <div id="containerPopupEntryData" style="margin-top: 10px; display: none;">
                    <input type="hidden" id="hdnID" runat="server" value="" />
                    <div class="pageTitle">
                        <%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 200px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory lblLink" id="lblBPJSPOliKlinik">
                                        <%=GetLabel("BPJS Poli")%></label>
                                </td>
                                <td>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 100px" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtBPJSCode" CssClass="required" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBPJSName" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal lblLink" id="lblSpecialty">
                                        <%=GetLabel("Spesialisasi")%></label>
                                </td>
                                <td>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 100px" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtSpecialtyID" CssClass="required" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSpecialtyName" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
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
                <div style="position: relative; height: 300px; overflow-y: auto; overflow-x: hidden;">
                    <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                        ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                        OnRowDataBound="grdView_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                        alt="" style="float: left; margin-left: 7px" />
                                                    <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                        alt="" />
                                                    <input type="hidden" class="hdnID" value="<%#: Eval("ID")%>" />
                                                    <input type="hidden" class="hdnBPJSPoliCode" value="<%#: Eval("cfBPJSPoliCode")%>" />
                                                    <input type="hidden" class="hdnBPJSPoliName" value="<%#: Eval("cfBPJSPoliName")%>" />
                                                    <input type="hidden" class="hdnSpecialtyID" value="<%#: Eval("SpecialtyID")%>" />
                                                    <input type="hidden" class="hdnSpecialtyName" value="<%#: Eval("SpecialtyName")%>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="cfBPJSPoliCode" HeaderText="Kode" HeaderStyle-Width="70px"
                                                HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="cfBPJSPoliName" HeaderText="Nama" HeaderStyle-Width="190px"
                                                HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="SpecialtyName" HeaderText="Spesialisasi" HeaderStyle-Width="200px"
                                                HeaderStyle-HorizontalAlign="Left" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Data Tidak Tersedia")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="pagingPopup">
                            </div>
                        </div>
                    </div>
                </div>
                <div style="width: 100%; text-align: center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblEntryPopupAddData">
                        <%= GetLabel("Tambah Data")%></span>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
