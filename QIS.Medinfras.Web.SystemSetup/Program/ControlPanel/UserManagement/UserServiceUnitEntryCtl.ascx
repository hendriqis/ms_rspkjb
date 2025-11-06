<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserServiceUnitEntryCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.SystemSetup.Program.UserServiceUnitEntryCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    var pageCountPopup = parseInt('<%=PageCount %>');
    $(function () {
        setPaging($("#pagingPopup"), pageCountPopup, function (page) {
            getCheckedServiceUnit();
            cbpEntryPopupView.PerformCallback('changepage|' + page);
        });
    });

    function getCheckedServiceUnit() {
        var lstSelectedServiceUnit = $('#<%=hdnSelectedServiceUnit.ClientID %>').val().split(',');
        var result = '';
        $('#<%=grdView.ClientID %> .chkServiceUnit input').each(function () {
            if ($(this).is(':checked')) {
                var key = $(this).closest('tr').find('.keyField').html();
                if (lstSelectedServiceUnit.indexOf(key) < 0)
                    lstSelectedServiceUnit.push(key);
            }
            else {
                var key = $(this).closest('tr').find('.keyField').html();
                if (lstSelectedServiceUnit.indexOf(key) > -1)
                    lstSelectedServiceUnit.splice(lstSelectedServiceUnit.indexOf(key), 1);
            }
        });
        $('#<%=hdnSelectedServiceUnit.ClientID %>').val(lstSelectedServiceUnit.join(','));
    }


    function onCbpEntryPopupViewEndCallback(s) {
        hideLoadingPanel();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);

            setPaging($("#pagingPopup"), pageCount, function (page) {
                getCheckedServiceUnit();
                cbpEntryPopupView.PerformCallback('changepage|' + page);
            });
        }
    }

    function onCboDepartmentValueChanged(s) {
        getCheckedServiceUnit();
        cbpEntryPopupView.PerformCallback('refresh');
    }

    function onCboHealthcareValueChanged(s) {
        getCheckedServiceUnit();
        cbpEntryPopupView.PerformCallback('refresh');
    }

    function onCbpViewPopupProcessEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        if (param[1] == 'fail')
            showToast('Save Failed', 'Error Message : ' + param[2]);
        else
            pcRightPanelContent.Hide();
    }

    $(function () {
        $('#btnSaveServiceUnitUser').click(function () {
            getCheckedServiceUnit();
            cbpViewPopupProcess.PerformCallback('save');
        });
    });

    $('#chkSelectAllServiceUnit').die('change');
    $('#chkSelectAllServiceUnit').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkServiceUnit').each(function () {
            $(this).find('input').prop('checked', isChecked);
        });
    });
</script>

<div style="height:440px; overflow-y:auto;overflow-x:hidden">
    <input type="hidden" id="hdnUserID" value="" runat="server" />
    <input type="hidden" id="hdnOldSelectedServiceUnit" runat="server" value="" />
    <input type="hidden" id="hdnSelectedServiceUnit" runat="server" value="" />
    <div class="pageTitle"><%=GetLabel("Unit Pelayanan")%></div>
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
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Nama Pengguna")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtUserName" ReadOnly="true" Width="100%" runat="server" /></td>
                    </tr>  
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Rumah Sakit")%></label></td>
                        <td colspan="2">
                            <dxe:ASPxComboBox ID="cboHealthcare" runat="server" Width="100%">
                                <ClientSideEvents ValueChanged="function(s,e){ onCboHealthcareValueChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr> 
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Instalasi")%></label></td>
                        <td colspan="2">
                            <dxe:ASPxComboBox ID="cboDepartment" runat="server" Width="100%">
                                <ClientSideEvents ValueChanged="function(s,e){ onCboDepartmentValueChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>  
                </table>                

                <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewPopup').show(); }"
                        EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlPatientVisitTransHdGrdView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false" OnRowDataBound="grdView_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <input id="chkSelectAllServiceUnit" type="checkbox" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkServiceUnit" runat="server" CssClass="chkServiceUnit" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="HealthcareServiceUnitID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:BoundField HeaderStyle-Width="250px" DataField="DepartmentName" ItemStyle-CssClass="tdDepartmentName" HeaderText="Instalasi" />
                                        <asp:BoundField DataField="ServiceUnitName" HeaderText="Unit Pelayanan" ItemStyle-CssClass="tdServiceUnitName" />
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
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="pagingPopup"></div>
                    </div>
                </div> 
            </td>
        </tr>
    </table>

    <div style="display:none">
        <dxcp:ASPxCallbackPanel ID="cbpViewPopupProcess" runat="server" Width="100%" ClientInstanceName="cbpViewPopupProcess"
            ShowLoadingPanel="false" OnCallback="cbpViewPopupProcess_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                EndCallback="function(s,e){ onCbpViewPopupProcessEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>

    <div style="width:100%;text-align:center">
    <table style="margin-left: auto; margin-right: auto; margin-top: 10px;">
        <tr>
            <td><input type="button" value='<%= GetLabel("Simpan")%>' style="width:70px" id="btnSaveServiceUnitUser" /></td>
            <td><input type="button" value='<%= GetLabel("Tutup")%>' style="width:70px" onclick="pcRightPanelContent.Hide();" /></td>
        </tr>
    </table>
</div>
</div>

