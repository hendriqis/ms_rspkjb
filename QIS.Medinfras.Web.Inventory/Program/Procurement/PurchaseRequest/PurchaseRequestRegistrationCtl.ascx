<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PurchaseRequestRegistrationCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inventory.Program.PurchaseRequestRegistrationCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_purchaseRequestRegistrationCtl">
    $('#lblPurchaseRequestRegistrationAddData').die('click');
    $('#lblPurchaseRequestRegistrationAddData').live('click', function () {
        $('#<%:hdnID.ClientID %>').val('');
        $('#<%:txtRegistrationNo.ClientID %>').val('');
        $('#<%:txtMedicalNo.ClientID %>').val('');
        $('#<%:txtPatientName.ClientID %>').val('');

        $('#containerPurchaseRequestRegistrationEntryData').show();
    });

    $('#btnCancel').click(function () {
        $('#containerPurchaseRequestRegistrationEntryData').hide();
    });

    $('#btnSave').click(function (evt) {
        if (IsValid(evt, 'fsPurchaseRequest', 'mpPurchaseRequest'))
            cbpPurchaseRequestRegistration.PerformCallback('save');
        return false;
    });

    //#region Registrasi
    $('#lblNoReg.lblLink').live('click', function () {
        openSearchDialog('registration3', '1=1', function (value) {
            $('#<%:txtRegistrationNo.ClientID %>').val(value);
            onTxtRegistrationNoChanged(value);
        });
    });
    $('#<%:txtRegistrationNo.ClientID %>').live('change', function () {
        onTxtRegistrationNoChanged($(this).val());
    });

    function onTxtRegistrationNoChanged(value) {
        var filterExpression = "RegistrationNo = '" + value + "'";
        Methods.getObject('GetvRegistrationList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnRegistrationID.ClientID %>').val(result.RegistrationID);
                $('#<%=txtRegistrationNo.ClientID %>').val(result.RegistrationNo);
                $('#<%=txtPatientName.ClientID %>').val(result.PatientName);
                $('#<%=txtMedicalNo.ClientID %>').val(result.MedicalNo);
            }
            else {
                $('#<%=hdnRegistrationID.ClientID %>').val('');
                $('#<%:txtRegistrationNo.ClientID %>').val('');
                $('#<%=txtPatientName.ClientID %>').val('');
                $('#<%=txtMedicalNo.ClientID %>').val('');
            }
        });
    }
    //#endregion

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        showToastConfirmation('Are You Sure Want To Delete?', function (result) {
            if (result) {
                var entity = rowToObject($row);
                $('#<%=hdnID.ClientID %>').val(entity.ID);
                cbpPurchaseRequestRegistration.PerformCallback('delete');
            }
        });
    });

    $('.imgEdit.imgLink').die('click')
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var entity = rowToObject($row);
        $('#<%=hdnID.ClientID %>').val(entity.ID);
        $('#<%=txtRegistrationNo.ClientID %>').val(entity.RegistrationNo);
        $('#<%=txtMedicalNo.ClientID %>').val(entity.MedicalNo);
        $('#<%=txtPatientName.ClientID %>').val(entity.PatientName);

        $('#containerPurchaseRequestRegistrationEntryData').show();
    });

    function onPurchaseRequestRegistrationCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else
                $('#containerPurchaseRequestRegistrationEntryData').hide();
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
        }
        hideLoadingPanel();
    }
</script>
<div style="height: 440px; overflow-y: auto">
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <table class="tblContentArea">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 400px">
                    <colgroup>
                        <col style="width: 350px" />
                        <col style="width: 300px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. Permintaan Pembelian")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPurchaseRequestNo" Width="150px" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                </table>
                <div id="containerPurchaseRequestRegistrationEntryData" style="margin-top: 10px;
                    display: none;">
                    <div class="pageTitle">
                        <%=GetLabel("Entry")%></div>
                    <input type="hidden" id="hdnPurchaseRequestID" runat="server" value="" />
                    <fieldset id="fsPurchaseRequest" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 400px" />
                            </colgroup>
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblLink" id="lblNoReg">
                                                    <%:GetLabel("No. Registrasi")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtRegistrationNo" Width="200px" runat="server" ReadOnly="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("No. Rekam Medis")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtMedicalNo" Width="100px" runat="server" ReadOnly="true" />
                                            </td>
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Nama Pasien")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPatientName" Width="350px" runat="server" ReadOnly="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnSave" value='<%= GetLabel("Simpan")%>' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnCancel" value='<%= GetLabel("Batal")%>' />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <dxcp:ASPxCallbackPanel ID="cbpPurchaseRequestRegistration" runat="server" Width="100%"
                    ClientInstanceName="cbpPurchaseRequestRegistration" ShowLoadingPanel="false"
                    OnCallback="cbpPurchaseRequestRegistration_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onPurchaseRequestRegistrationCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlDocumentNotesGrdView" Style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdPurchaseRequestRegistration" runat="server" CssClass="grdView notAllowSelect"
                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                    alt="" style="float: left; margin-left: 7px" />
                                                <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                    alt="" />
                                                <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                <input type="hidden" value="<%#:Eval("RegistrationNo") %>" bindingfield="RegistrationNo" />
                                                <input type="hidden" value="<%#:Eval("MedicalNo") %>" bindingfield="MedicalNo" />
                                                <input type="hidden" value="<%#:Eval("PatientName") %>" bindingfield="PatientName" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="RegistrationNo" HeaderText="No.Registrasi" HeaderStyle-Width="200px"
                                            HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="PatientName" HeaderText="Nama Pasien" HeaderStyle-Width="200px"
                                            HeaderStyle-HorizontalAlign="Left" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Data Tidak Tersedia")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                                <div class="imgLoadingGrdView" id="containerImgLoadingDocumentNote">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div style="width: 100%; text-align: center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblPurchaseRequestRegistrationAddData">
                        <%= GetLabel("Tambah Data")%></span>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
