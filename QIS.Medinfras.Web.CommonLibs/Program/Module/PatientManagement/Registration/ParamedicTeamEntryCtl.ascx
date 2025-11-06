<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ParamedicTeamEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ParamedicTeamEntryCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<script type="text/javascript" id="dxss_paramedicteamentryctl">
    $(function () {
        $('#<%=chkDisplayRMO.ClientID %>').change(function () {
            cbpEntryPopupView.PerformCallback('refresh');
        });
    });

    $('#lblEntryPopupAddData').live('click', function () {
        $('#lblPopupParamedic').attr('class', 'lblLink lblMandatory');
        $('#<%=txtParamedicCode.ClientID %>').removeAttr('readonly');

        $('#<%=hdnParamedicID.ClientID %>').val('');
        $('#<%=txtParamedicCode.ClientID %>').val('');
        $('#<%=txtParamedicName.ClientID %>').val('');
        cboParamedicRole.SetValue('');
        $('#<%=hdnIsAdd.ClientID %>').val('1');
        $('#containerPopupEntryData').show();
    });

    //#region Save and Cancel
    $('#btnEntryPopupSave').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
            cbpEntryPopupView.PerformCallback('save');
        return false;
    });

    $('#btnEntryPopupCancel').live('click', function () {
        $('#containerPopupEntryData').hide();
    });
    //#endregion

    //#region Edit Delete
    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        var entity = rowToObject($row);

        $('#<%=hdnIsAdd.ClientID %>').val('0');
        $('#lblPopupParamedic').removeAttr('class');
        $('#lblPopupParamedic').attr('class', 'lblMandatory');
        $('#<%=txtParamedicCode.ClientID %>').attr('readonly', 'readonly');

        $('#<%=hdnID.ClientID %>').val(entity.ID);
        $('#<%=hdnParamedicID.ClientID %>').val(entity.ParamedicID);
        $('#<%=txtParamedicCode.ClientID %>').val(entity.ParamedicCode);
        $('#<%=txtParamedicName.ClientID %>').val(entity.ParamedicName);
        cboParamedicRole.SetValue(entity.GCParamedicRole);
        $('#containerPopupEntryData').show();
    });

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        showToastConfirmation('Are You Sure Want To Delete?', function (result) {
            if (result) {
                var entity = rowToObject($row);
                $('#<%=hdnID.ClientID %>').val(entity.ID);
                $('#<%=hdnParamedicID.ClientID %>').val(entity.ParamedicID);
                cbpEntryPopupView.PerformCallback('delete');
            }
        });
    });
    //#endregion

    //#region Paramedic
    function onGetPopupParamedicFilterExpression() {
        var filterExpression = "ParamedicID != " + $('#<%=hdnMainParamedicID.ClientID %>').val()
            + " AND GCParamedicMasterType = 'X019^001'"
            + " AND ParamedicID NOT IN (SELECT ParamedicID FROM ParamedicTeam WHERE RegistrationID = " + $('#<%=hdnRegistrationID.ClientID %>').val() + " AND IsDeleted = 0)"
            + " AND IsDeleted = 0";
        return filterExpression;
    }

    $('#lblPopupParamedic.lblLink').click(function () {
        var isAdd = $('#<%=hdnIsAdd.ClientID %>').val();
        if (isAdd == "1") {
            openSearchDialog('paramedic', onGetPopupParamedicFilterExpression(), function (value) {
                $('#<%=txtParamedicCode.ClientID %>').val(value);
                onTxtParamedicCodeChanged(value);
            });
        }
    });

    $('#<%=txtParamedicCode.ClientID %>').change(function () {
        onTxtParamedicCodeChanged($(this).val());
    });

    function onTxtParamedicCodeChanged(value) {
        var filterExpression = onGetPopupParamedicFilterExpression() + " AND ParamedicCode = '" + value + "'";
        Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnParamedicID.ClientID %>').val(result.ParamedicID);
                $('#<%=txtParamedicName.ClientID %>').val(result.ParamedicName);
            }
            else {
                $('#<%=hdnParamedicID.ClientID %>').val('');
                $('#<%=txtParamedicCode.ClientID %>').val('');
                $('#<%=txtParamedicName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    function onCbpEntryPopupViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else
                $('#containerPopupEntryData').hide();
                cbpEntryPopupView.PerformCallback('refresh');
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
        }
        hideLoadingPanel();
    }
</script>

<div style="height:440px; overflow-y:auto">
    <input type="hidden" id="hdnID" value="" runat="server" />
    <input type="hidden" id="hdnRegistrationID" value="" runat="server" />
    <input type="hidden" id="hdnMainParamedicID" value="" runat="server" />
    <input type="hidden" id="hdnIsAdd" value="" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width:100%"/>
        </colgroup>
        <tr>            
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:70%">
                    <colgroup>
                        <col style="width:160px"/>
                        <col/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("No. Registrasi")%></label></td>
                        <td><asp:TextBox ID="txtRegistrationNo" ReadOnly="true" Width="160px" runat="server" /></td>
                    </tr>  
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Nama Pasien")%></label></td>
                        <td><asp:TextBox ID="txtPatientName" ReadOnly="true" Width="100%" runat="server" /></td>
                        <td><asp:CheckBox ID="chkDisplayRMO" runat="server" Text=" Termasuk Dokter Jaga" Checked="false" /></td>
                    </tr> 
                </table>
                <div id="containerPopupEntryData" style="margin-top:10px;display:none;">
                    <div class="pageTitle"><%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin:0"> 
                        <table class="tblEntryDetail" style="width:100%">
                            <colgroup>
                                <col style="width:150px"/>
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel"><label class="lblLink lblMandatory" id="lblPopupParamedic"><%=GetLabel("Dokter ")%></label></td> 
                                <td>
                                    <input type="hidden" value="" id="hdnParamedicID" runat="server" />
                                    <table style="width:100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width:120px"/>
                                            <col style="width:3px"/>
                                            <col/>
                                        </colgroup>
                                        <tr>
                                            <td><asp:TextBox ID="txtParamedicCode" Width="100%" runat="server" /></td>
                                            <td>&nbsp;</td>
                                            <td><asp:TextBox ID="txtParamedicName" ReadOnly="true" CssClass="required" Width="70%" runat="server" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Peranan")%></label></td>
                                <td><dxe:ASPxComboBox ID="cboParamedicRole" ClientInstanceName="cboParamedicRole" Width="150px" runat="server" /></td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <center>
                                        <table>
                                            <tr>
                                                <td>
                                                    <input type="button" id="btnEntryPopupSave" class="w3-btn w3-hover-blue" value='<%= GetLabel("Save")%>' style="width:80px" />
                                                </td>
                                                <td>
                                                    <input type="button" id="btnEntryPopupCancel" class="w3-btn w3-hover-blue" value='<%= GetLabel("Cancel")%>' style="width:80px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>

                <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                    EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlPatientVisitTransHdGrdView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px">
                                            <ItemTemplate>
                                               <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td><img class="imgEdit <%# Eval("GCParamedicRole").ToString() == GCTypeRMO || Eval("GCParamedicRole").ToString() == GCTypeDPJP ? "imgDisabled" : "imgLink"%>" src='<%# Eval("GCParamedicRole").ToString() == GCTypeRMO || Eval("GCParamedicRole").ToString() == GCTypeDPJP ? ResolveUrl("~/Libs/Images/Button/edit_disabled.png") : ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt="" style="float:left; margin-left:7px" /></td>
                                                        <td style="width:3px">&nbsp;</td>
                                                        <td><img class="imgDelete <%# Eval("GCParamedicRole").ToString() == GCTypeDPJP ? "imgDisabled" : "imgLink"%>" src='<%# Eval("GCParamedicRole").ToString() == GCTypeDPJP ? ResolveUrl("~/Libs/Images/Button/delete_disabled.png") : ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" /></td>
                                                    </tr>
                                                </table>
                                                <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                <input type="hidden" value="<%#:Eval("RegistrationID") %>" bindingfield="RegistrationID" />
                                                <input type="hidden" value="<%#:Eval("ParamedicID") %>" bindingfield="ParamedicID" />
                                                <input type="hidden" value="<%#:Eval("ParamedicCode") %>" bindingfield="ParamedicCode" />
                                                <input type="hidden" value="<%#:Eval("ParamedicName") %>" bindingfield="ParamedicName" />
                                                <input type="hidden" value="<%#:Eval("GCParamedicRole") %>" bindingfield="GCParamedicRole" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ParamedicName" HeaderText="Dokter" ItemStyle-CssClass="tdParamedicName" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="ParamedicRole" HeaderText="Peranan" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="80px" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div style="width:100%;text-align:center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblEntryPopupAddData"><%= GetLabel("Tambah Dokter")%></span>
                </div>
            </td>
        </tr>
    </table>
</div>