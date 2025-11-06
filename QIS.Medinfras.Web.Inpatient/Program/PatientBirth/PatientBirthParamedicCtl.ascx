<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientBirthParamedicCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inpatient.Program.PatientBirthParamedicCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_patientbirthparamedicctl">
    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=hdnID.ClientID %>').val('');
        $('#<%=hdnPhysicianID.ClientID %>').val('');
        $('#<%=cboParamedicType.ClientID %>').val('');
        $('#<%=txtPhysicianCode.ClientID %>').val('');
        $('#<%=txtPhysicianName.ClientID %>').val('');
        $('#<%=txtParamedicName.ClientID %>').val('');

        $('#<%=chkOtherParamedic.ClientID %>').prop('checked', false);
        $('#<%=chkOtherParamedic.ClientID %>').change();
        $('#containerPopupEntryData').show();
    });

    $('#<%=chkOtherParamedic.ClientID %>').change(function () {
        if ($(this).is(':checked')) {
            $('#<%=hdnPhysicianID.ClientID %>').val('');
            $('#<%=txtPhysicianCode.ClientID %>').val('');
            $('#<%=txtPhysicianName.ClientID %>').val('');
            $('#<%=txtParamedicName.ClientID %>').removeAttr('readonly');
            $('#<%=txtPhysicianCode.ClientID %>').attr('readonly', 'readonly');
            $('#lblPhysician').attr('class', 'lblDisabled');
        }
        else {
            $('#<%=txtParamedicName.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtPhysicianCode.ClientID %>').removeAttr('readonly');
            $('#<%=txtParamedicName.ClientID %>').val('');
            $('#lblPhysician').attr('class', 'lblLink lblMandatory');
        }
    });

    $('#btnEntryPopupCancel').live('click', function () {
        $('#containerPopupEntryData').hide();
    });

    $('#btnEntryPopupSave').click(function (evt) {
        if (IsValid(null, 'fsEntryPopup', 'mpPatientBirthParamedic'))
            cbpEntryPopupView.PerformCallback('save');
        return false;
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
        var physicianID = $row.find('.hdnPhysicianID').val();
        var physicianCode = $row.find('.hdnPhysicianCode').val();
        var physicianName = $row.find('.hdnPhysicianName').val();
        var paramedicType = $row.find('.hdnParamedicType').val();
        var paramedicCode = $row.find('.tdParamedicCode').html();
        var paramedicName = $row.find('.hdnParamedicName').val();
        var isParamedicExternal = $row.find('.hdnIsParamedicExternal').val();
        
        $('#<%=hdnID.ClientID %>').val(ID);
        $('#<%=hdnPhysicianID.ClientID %>').val(physicianID);
        $('#<%=hdnParamedicType.ClientID %>').val(paramedicType);
        cboParamedicType.SetValue(paramedicType);
        $('#<%=chkOtherParamedic.ClientID %>').prop('checked', (isParamedicExternal == 'True'));
        $('#<%=txtPhysicianCode.ClientID %>').val(physicianCode);
        $('#<%=txtPhysicianName.ClientID %>').val(physicianName);
        $('#<%=txtParamedicName.ClientID %>').val(paramedicName);
        $('#<%=chkOtherParamedic.ClientID %>').change();
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
        $('#containerImgLoadingView').hide();
    }

    //#region Physician
    function onGetPatientBirthParamedicCodeFilterExpression() {
        var birthRecordID = $('#<%=hdnBirthRecordID.ClientID %>').val();
        var filterExpression = "ParamedicID NOT IN (SELECT ParamedicID FROM PatientBirthRecordParamedic WHERE BirthRecordID = " + birthRecordID + " AND ParamedicID IS NOT NULL AND IsDeleted = 0) AND IsDeleted = 0";
        return filterExpression;
    }

    $('#lblPhysician.lblLink').die('click');
    $('#lblPhysician.lblLink').live('click', function () {
        openSearchDialog('paramedic', onGetPatientBirthParamedicCodeFilterExpression(), function (value) {
            $('#<%=txtPhysicianCode.ClientID %>').val(value);
            onTxtPatientBirthPhysicianCodeChanged(value);
        });
    });

    $('#<%=txtPhysicianCode.ClientID %>').die('change');
    $('#<%=txtPhysicianCode.ClientID %>').live('change', function () {
        onTxtPatientBirthPhysicianCodeChanged($(this).val());
    });

    function onTxtPatientBirthPhysicianCodeChanged(value) {
        var filterExpression = onGetPatientBirthParamedicCodeFilterExpression() + " AND ParamedicCode = '" + value + "'";
        Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnPhysicianID.ClientID %>').val(result.ParamedicID);
                $('#<%=txtPhysicianName.ClientID %>').val(result.ParamedicName);
            }
            else {
                $('#<%=hdnPhysicianID.ClientID %>').val('');
                $('#<%=txtPhysicianCode.ClientID %>').val('');
                $('#<%=txtPhysicianName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    
</script>
<div style="height: 440px; overflow-y: auto">
    <input type="hidden" id="hdnMRN" value="" runat="server" />
    <input type="hidden" id="hdnBirthRecordID" value="" runat="server" />
    <div class="pageTitle">
        <%=GetLabel("Dokter / Paramedis")%></div>
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
                                <%=GetLabel("No. Registrasi")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtNoReg" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No.RM")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtMRN" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nama Bayi")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtPatientName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
                <div id="containerPopupEntryData" style="margin-top: 10px; display: none;">
                    <input type="hidden" id="hdnID" runat="server" value="" />
                    <div class="pageTitle">
                        <%=GetLabel("Isi Dokter")%></div>
                    <fieldset id="fsEntryPopup" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 150px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Tipe Dokter")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnParamedicType" runat="server" value="" />
                                    <dxe:ASPxComboBox ID="cboParamedicType" ClientInstanceName="cboParamedicType" Width="300px"
                                        runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkOtherParamedic" runat="server" /><%=GetLabel("Dokter Luar RS")%>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory lblLink" id="lblPhysician">
                                        <%=GetLabel("Dokter / Paramedis")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnPhysicianID" runat="server" value="" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30%" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtPhysicianCode" CssClass="required" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPhysicianName" ReadOnly="true" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal lblMandatory">
                                        <%=GetLabel("Nama Dokter")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtParamedicName" runat="server" Width="300px" ReadOnly="true" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("Save")%>' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnEntryPopupCancel" value='<%= GetLabel("Cancel")%>' />
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
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                    OnRowDataBound="grdView_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                    alt="" style="float: left; margin-left: 7px" />
                                                <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                    alt="" />
                                                <input type="hidden" class="hdnID" value="<%#: Eval("ID")%>" />
                                                <input type="hidden" class="hdnPhysicianID" value="<%#: Eval("ParamedicID")%>" />
                                                <input type="hidden" class="hdnPhysicianCode" value="<%#: Eval("ParamedicCode")%>" />
                                                <input type="hidden" class="hdnPhysicianName" value="<%#: Eval("FullName")%>" />
                                                <input type="hidden" class="hdnOtherParamedicName" value="<%#: Eval("ParamedicName")%>" />
                                                <input type="hidden" class="hdnParamedicType" value="<%#: Eval("GCParamedicType")%>" />
                                                <input type="hidden" class="hdnParamedicName" value="<%#: Eval("ParamedicName")%>" />
                                                <input type="hidden" class="hdnIsParamedicExternal" value="<%#: Eval("IsParamedicExternal")%>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ParamedicCode" ItemStyle-CssClass="tdParamedicCode" HeaderText="Kode Dokter" />
                                        <asp:BoundField DataField="cfParamedicName" ItemStyle-CssClass="tdOtherParamedicName"
                                            HeaderText="Nama Dokter" />
                                        <asp:BoundField DataField="ParamedicTypeDescription" ItemStyle-CssClass="tdParamedicType"
                                            HeaderText="Tipe Dokter" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display")%>
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
                        <%= GetLabel("Add Data")%></span>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Close")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
