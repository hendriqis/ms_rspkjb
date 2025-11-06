<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataMigrationTableLinkCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Tools.DataMigrationTableLinkCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=hdnIsAdd.ClientID %>').val('1');
         
        $('#<%=txtTableName.ClientID %>').removeAttr('readonly');
        $('#<%=txtColumnName.ClientID %>').removeAttr('readonly');

        $('#<%=txtTableName.ClientID %>').val('');
        $('#<%=txtColumnName.ClientID %>').val('');
        $('#<%=txtLinkTableName.ClientID %>').val('');
        $('#<%=txtLinkColumnName.ClientID %>').val('');

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

            var tableName = $row.find('.tdTableName').html();
            var columnName = $row.find('.tdColumnName').html();

            $('#<%=txtTableName.ClientID %>').val(tableName);
            $('#<%=txtColumnName.ClientID %>').val(columnName);

            cbpEntryPopupView.PerformCallback('delete');
        }
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $('#<%=hdnIsAdd.ClientID %>').val('0');

        $row = $(this).closest('tr');
        var id = $row.find('.hdnID').val();

        var tableName = $row.find('.tdTableName').html();
        var columnName = $row.find('.tdColumnName').html();
        var linkTableName = $row.find('.tdLinkTableName').html();
        var linkColumnName = $row.find('.tdLinkColumnName').html();

        $('#<%=txtTableName.ClientID %>').attr('readonly', 'readonly');
        $('#<%=txtColumnName.ClientID %>').attr('readonly', 'readonly');

        $('#<%=txtTableName.ClientID %>').val(tableName);
        $('#<%=txtColumnName.ClientID %>').val(columnName);
        $('#<%=txtLinkTableName.ClientID %>').val(linkTableName);
        $('#<%=txtLinkColumnName.ClientID %>').val(linkColumnName);

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
        hideLoadingPanel();
    }
</script>

<div style="height:440px; overflow-y:auto">
    <input type="hidden" id="hdnID" value="" runat="server" />
    <input type="hidden" id="hdnIsAdd" value="" runat="server" />
    <div class="pageTitle"><%=GetLabel("Table Link")%></div>
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
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("From Table - To Table")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtHeaderText" ReadOnly="true" Width="100%" runat="server" /></td>
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
                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Table Name")%></label></td>
                                <td><asp:TextBox ID="txtTableName" CssClass="required" Width="100%" runat="server" /></td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Column Name")%></label></td>
                                <td><asp:TextBox ID="txtColumnName" CssClass="required" Width="100%" runat="server" /></td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Link Table Name")%></label></td>
                                <td><asp:TextBox ID="txtLinkTableName" CssClass="required" Width="100%" runat="server" /></td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Link Column Name")%></label></td>
                                <td><asp:TextBox ID="txtLinkColumnName" CssClass="required" Width="100%" runat="server" /></td>
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
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                        EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlPatientVisitTransHdGrdView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false" OnRowDataBound="grdView_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt="" style="float:left; margin-left:7px" />
                                                <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="TableName" ItemStyle-CssClass="tdTableName" HeaderText="Table Name" />
                                        <asp:BoundField DataField="ColumnName" ItemStyle-CssClass="tdColumnName" HeaderText="Column Name" />
                                        <asp:BoundField DataField="LinkTableName" ItemStyle-CssClass="tdLinkTableName" HeaderText="Link Table Name" />
                                        <asp:BoundField DataField="LinkTableColumn" ItemStyle-CssClass="tdLinkColumnName" HeaderText="Link Column Name" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div style="width:100%;text-align:center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblEntryPopupAddData"><%= GetLabel("Add Data")%></span>
                </div>
            </td>
        </tr>
    </table>
    <div style="width:100%;text-align:right">
        <input type="button" value='<%= GetLabel("Close")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>

