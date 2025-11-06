<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserMenuAccessEntryCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.SystemSetup.Program.UserMenuAccessEntryCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_userrolemenuaccessentryctl">
    function addMenuAccessFilterRow() {
        $trHeader = $('#<%=grdView.ClientID %> tr:eq(0)');
        $trFilter = $("<tr><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td></tr>");
        $cboRead = $("<select id='cboFilterRead' style='width:100%' />");
        $cboRead.append($("<option value=''></option>"));
        $cboRead.append($("<option value='1'>checked</option>"));
        $cboRead.append($("<option value='0'>unchecked</option>"));
        $cboRead.val($('#<%=hdnFilterRead.ClientID %>').val());
        $trFilter.find('td').eq(0).append($cboRead);

        $input = $("<input type='text' id='txtFilterMenuCaption' style='width:100%' />").val($('#<%=hdnFilterMenuCaption.ClientID %>').val());
        $trFilter.find('td').eq(1).append($input);
        $trFilter.insertAfter($trHeader);
    }

    $('#txtFilterMenuCaption').live('keypress', function (e) {
        var code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13) {
            $('#<%=hdnFilterMenuCaption.ClientID %>').val($(this).val());
            e.preventDefault();
            $('#<%=hdnCRUDMode.ClientID %>').val(getCRUDMode());
            $('#<%=hdnListMenuID.ClientID %>').val(getListMenuID());
            cbpMenuAccess.PerformCallback('refresh');
        }
    });

    $('#cboFilterRead').live('change', function () {
        $('#<%=hdnFilterRead.ClientID %>').val($(this).val());
        $('#<%=hdnCRUDMode.ClientID %>').val(getCRUDMode());
        $('#<%=hdnListMenuID.ClientID %>').val(getListMenuID());
        cbpMenuAccess.PerformCallback('refresh');
    });

    $(function () {
        addMenuAccessFilterRow();

        $('#<%=ddlHealthcare.ClientID %>').change(function () {
            $('#<%=hdnSelectedHealthcare.ClientID %>').val($(this).val());
            $('#<%=hdnCRUDMode.ClientID %>').val(getCRUDMode());
            $('#<%=hdnListMenuID.ClientID %>').val(getListMenuID());
            cbpMenuAccess.PerformCallback('changemodule');
        });

        $('#<%=ddlModule.ClientID %>').change(function () {
            $('#<%=hdnSelectedModule.ClientID %>').val($(this).val());
            $('#<%=hdnCRUDMode.ClientID %>').val(getCRUDMode());
            $('#<%=hdnListMenuID.ClientID %>').val(getListMenuID());
            cbpMenuAccess.PerformCallback('changemodule');
        });

        $('#btnSaveMatrix').live('click',function () {
            $('#<%=hdnCRUDMode.ClientID %>').val(getCRUDMode());
            $('#<%=hdnListMenuID.ClientID %>').val(getListMenuID());
            cbpMenuAccessProcess.PerformCallback();
        });

        $('#<%=hdnPrevSelectedHealthcare.ClientID %>').val($('#<%=ddlHealthcare.ClientID %>').val());
    });

    //#region Paging
    var pageCountMenuAccess = parseInt('<%=PageCount %>');
    $(function () {
        setPaging($("#pagingMenuAccesss"), pageCountMenuAccess, function (page) {
            $('#<%=hdnCRUDMode.ClientID %>').val(getCRUDMode());
            $('#<%=hdnListMenuID.ClientID %>').val(getListMenuID());

            cbpMenuAccess.PerformCallback('changepage|' + page);
        });
    });

    function onCbpMenuAccessEndCallback(s) {
        $('#<%=hdnPrevSelectedHealthcare.ClientID %>').val($('#<%=hdnSelectedHealthcare.ClientID %>').val());
        $('#containerImgLoadingMenuAccess').hide();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdView.ClientID %> tr:eq(1)').click();

            setPaging($("#pagingMenuAccesss"), pageCount, function (page) {
                $('#<%=hdnCRUDMode.ClientID %>').val(getCRUDMode());
                $('#<%=hdnListMenuID.ClientID %>').val(getListMenuID());
                cbpMenuAccess.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        addMenuAccessFilterRow();
    }
    //#endregion

    $('.chkMenuAccessRead input').live('click', function () {
        $tr = $(this).closest('tr');
        $chkCreate = $tr.find('.chkMenuAccessCreate').find('input');
        $chkUpdate = $tr.find('.chkMenuAccessUpdate').find('input');
        $chkDelete = $tr.find('.chkMenuAccessDelete').find('input');
        $chkPrint = $tr.find('.chkMenuAccessPrint').find('input');
        $chkApprove = $tr.find('.chkMenuAccessApprove').find('input');
        $chkVoid = $tr.find('.chkMenuAccessVoid').find('input');
        $chkVoidWithReason = $tr.find('.chkMenuAccessVoidWithReason').find('input');

        if ($(this).is(':checked')) {
            $chkCreate.removeAttr("disabled");
            $chkUpdate.removeAttr("disabled");
            $chkDelete.removeAttr("disabled");
            $chkPrint.removeAttr("disabled");
            $chkApprove.removeAttr("disabled");
            $chkVoid.removeAttr("disabled");
            $chkVoidWithReason.removeAttr("disabled");

            $chkCreate.prop('checked', true);
            $chkUpdate.prop('checked', true);
            $chkDelete.prop('checked', true);
            $chkPrint.prop('checked', true);
            $chkApprove.prop('checked', true);
            $chkVoid.prop('checked', true);
            $chkVoidWithReason.prop('checked', true);
        }
        else {
            $chkCreate.attr("disabled", true);
            $chkUpdate.attr("disabled", true);
            $chkDelete.attr("disabled", true);
            $chkPrint.attr("disabled", true);
            $chkApprove.attr("disabled", true);
            $chkVoid.attr("disabled", true);
            $chkVoidWithReason.attr("disabled", true);

            $chkCreate.prop('checked', false);
            $chkUpdate.prop('checked', false);
            $chkDelete.prop('checked', false);
            $chkPrint.prop('checked', false);
            $chkApprove.prop('checked', false);
            $chkVoid.prop('checked', false);
            $chkVoidWithReason.prop('checked', false);
        }
    });

    function getListMenuID() {
        var menuID = '';
        $('.hdnMenuID').each(function () {
            if (menuID != '')
                menuID += "|";
            menuID += $(this).val();
        });
        return menuID;
    }

    function getCRUDMode() {
        var CRUDMode = "";
        $('.chkMenuAccessRead input').each(function () {
            if (CRUDMode != '')
                CRUDMode += "|";
            $tr = $(this).closest('tr');
            $chkCreate = $tr.find('.chkMenuAccessCreate').find('input');
            $chkUpdate = $tr.find('.chkMenuAccessUpdate').find('input');
            $chkDelete = $tr.find('.chkMenuAccessDelete').find('input');
            $chkPrint = $tr.find('.chkMenuAccessPrint').find('input');
            $chkApprove = $tr.find('.chkMenuAccessApprove').find('input');
            $chkVoid = $tr.find('.chkMenuAccessVoid').find('input');
            $chkVoidWithReason = $tr.find('.chkMenuAccessVoidWithReason').find('input');

            CRUDMode += checkToChar($chkCreate, "C") + "-";
            CRUDMode += checkToChar($(this), "R") + "-";
            CRUDMode += checkToChar($chkUpdate, "U") + "-";
            CRUDMode += checkToChar($chkDelete, "D") + "-";
            CRUDMode += checkToChar($chkPrint, "P") + "-";
            CRUDMode += checkToChar($chkApprove, "A") + "-";
            CRUDMode += checkToChar($chkVoid, "V") + "-";
            CRUDMode += checkToChar($chkVoidWithReason, "X");
        });
        return CRUDMode;
    }

    function checkToChar($chk, ch) {
        if ($chk.is(':checked'))
            return ch;
        return "";
    }

    function onCbpMenuAccessProcessEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'success')
            pcRightPanelContent.Hide();
        else
            showToast('Save Failed', 'Error Message : ' + param[1]);
        hideLoadingPanel();
    }
</script>


<input type="hidden" value="" runat="server" id="hdnFilterMenuCaption" />
<input type="hidden" value="" runat="server" id="hdnFilterRead" />

<input type="hidden" runat="server" id="hdnPrevSelectedHealthcare" value="" />
<input type="hidden" runat="server" id="hdnSelectedHealthcare" value="" />
<input type="hidden" runat="server" id="hdnSelectedModule" value="" />
<input type="hidden" runat="server" id="hdnCRUDMode" value="" />
<input type="hidden" runat="server" id="hdnListMenuID" value="" />
<input type="hidden" runat="server" id="hdnUserID" value="" />
<table class="tblEntryContent" style="width:70%">
    <colgroup>
        <col style="width:160px"/>
        <col/>
    </colgroup>
    <tr>
        <td class="tdLabel"><label id="Label1" class="lblNormal" runat="server"><%=GetLabel("Nama Pengguna")%></label></td>
        <td colspan="2"><asp:TextBox ID="txtUserName" ReadOnly="true" Width="100%" runat="server" /></td>
    </tr>    
    <tr>
        <td class="tdLabel"><label id="Label2" class="lblNormal" runat="server" ><%=GetLabel("Rumah Sakit")%></label></td>
        <td colspan="2"><asp:DropDownList ID="ddlHealthcare" ReadOnly="true" Width="100%" runat="server" /></td>
    </tr>   
    <tr>
        <td class="tdLabel"><label id="Label3" class="lblNormal" runat="server" ><%=GetLabel("Module")%></label></td>
        <td colspan="2"><asp:DropDownList ID="ddlModule" ReadOnly="true" Width="100%" runat="server" /></td>
    </tr>   
</table>

<dxcp:ASPxCallbackPanel ID="cbpMenuAccess" runat="server" Width="100%" ClientInstanceName="cbpMenuAccess"
    ShowLoadingPanel="false" OnCallback="cbpMenuAccess_Callback">
    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingMenuAccess').show(); }"
        EndCallback="function(s,e){ onCbpMenuAccessEndCallback(s); }" />
    <PanelCollection>
        <dx:PanelContent ID="PanelContent1" runat="server">
            <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;height: 273px;overflow-y:auto; ">
                <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                    <Columns>
                        <asp:BoundField DataField="MenuID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                        <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                #
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkRead" runat="server" CssClass="chkMenuAccessRead"
                                    Checked='<%# Eval("READ") %>' Visible='<%# Eval("RVISIBLE") %>' />
                                <input type="hidden" value='<%#: Eval("MenuID") %>' class="hdnMenuID" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="300px" HeaderStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <div style="padding-left:3px">
                                    <%=GetLabel("Menu Caption")%>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div style='margin-left:<%#: Eval("Level") %>0px;'><%#: Eval("MenuCaption") %></div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField  HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <%=GetLabel("Create")%>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkCreate" runat="server" CssClass="chkMenuAccessCreate"
                                    Checked ='<%# Eval("CREATE") %>' Visible='<%# Eval("CVISIBLE") %>' Enabled='<%# Eval("ENABLED") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <%=GetLabel("Update")%>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkUpdate" runat="server" CssClass="chkMenuAccessUpdate"
                                    Checked ='<%# Eval("UPDATE") %>' Visible='<%# Eval("UVISIBLE") %>' Enabled='<%# Eval("ENABLED") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <%=GetLabel("Delete")%>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkDelete" runat="server" CssClass="chkMenuAccessDelete"
                                    Checked ='<%# Eval("DELETE") %>' Visible='<%# Eval("DVISIBLE") %>' Enabled='<%# Eval("ENABLED") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <%=GetLabel("Print")%>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkPrint" runat="server" CssClass="chkMenuAccessPrint"
                                    Checked ='<%# Eval("PRINT") %>' Visible='<%# Eval("PVISIBLE") %>' Enabled='<%# Eval("ENABLED") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <HeaderTemplate>                                
                                <%=GetLabel("Approve")%>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkApprove" runat="server" CssClass="chkMenuAccessApprove"
                                    Checked ='<%# Eval("APPROVE") %>' Visible='<%# Eval("AVISIBLE") %>' Enabled='<%# Eval("ENABLED") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <%=GetLabel("Void")%>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkVoid" runat="server" CssClass="chkMenuAccessVoid"
                                    Checked ='<%# Eval("VOID") %>' Visible='<%# Eval("VVISIBLE") %>' Enabled='<%# Eval("ENABLED") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                            <HeaderTemplate>
                                <%=GetLabel("Void With Reason")%>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkVoidWithReason" runat="server" CssClass="chkMenuAccessVoidWithReason"
                                    Checked ='<%# Eval("VOIDWITHREASON") %>' Visible='<%# Eval("XVISIBLE") %>' Enabled='<%# Eval("ENABLED") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <%=GetLabel("Data Tidak Tersedia")%>
                    </EmptyDataTemplate>
                </asp:GridView>
            </asp:Panel>
        </dx:PanelContent>
    </PanelCollection>
</dxcp:ASPxCallbackPanel>    
<div class="imgLoadingGrdView" id="containerImgLoadingMenuAccess" >
    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
</div>
<div class="containerPaging">
    <div class="wrapperPaging">
        <div id="pagingMenuAccesss"></div>
    </div>
</div> 
<div style="width:100%;text-align:center">
    <table style="margin-left: auto; margin-right: auto; margin-top: 10px;">
        <tr>
            <td><input type="button" value='<%= GetLabel("Simpan")%>' style="width:70px" id="btnSaveMatrix" /></td>
            <td><input type="button" value='<%= GetLabel("Tutup")%>' style="width:70px" onclick="pcRightPanelContent.Hide();" /></td>
        </tr>
    </table>
</div>
<dxcp:ASPxCallbackPanel ID="cbpMenuAccessProcess" runat="server" Width="100%" ClientInstanceName="cbpMenuAccessProcess"
    ShowLoadingPanel="false" OnCallback="cbpMenuAccessProcess_Callback">
    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpMenuAccessProcessEndCallback(s); }" />
</dxcp:ASPxCallbackPanel>