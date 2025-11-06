<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MPPatientPageListEntry.master" AutoEventWireup="true" CodeBehind="ParamedicTeamList.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.ParamedicTeamList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="plhRightToolbar" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhScript" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        });

        function onRefreshControl(filterExpression) {
            $('#<%=hdnFilterExpression.ClientID %>').val(filterExpression);
            cbpView.PerformCallback('refresh');
        }

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewEndCallback(s) {
            $('#containerImgLoadingView').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        function onSelectedRowChanged(value) {
            var idx = $('#<%=grdView.ClientID %> tr').index($('#<%=grdView.ClientID %> tr.selected'));
            idx += value;
            if (idx < 1)
                idx = 1;
            if (idx == $('#<%=grdView.ClientID %> tr').length)
                idx--;
            $('#<%=grdView.ClientID %> tr:eq(' + idx + ')').click();
        }

        function getSelectedRow() {
            return $('#<%=grdView.ClientID %> tr.selected');
        }

        function onButtonCancelClick() {
            $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
        }

        //#region Entity To Control
        function entityToControl(entity) {
            cboPhysicianRole.SetFocus();
            $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
            if (entity != null) {
                $('#<%=grdView.ClientID %> tr.focus').addClass('selected');
                $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
                $('#<%=hdnParamedicID.ClientID %>').val(entity.ParamedicID);
                cboPhysicianRole.SetValue(entity.GCParamedicRole);
                $('#<%=txtPhysicianCode.ClientID %>').val(entity.ParamedicCode);
                $('#<%=txtPhysicianName.ClientID %>').val(entity.ParamedicName);
            }
            else {
                $('#<%=hdnEntryID.ClientID %>').val('');
                $('#<%=hdnParamedicID.ClientID %>').val('');
                cboPhysicianRole.SetValue('');
                $('#<%=txtPhysicianCode.ClientID %>').val('');
                $('#<%=txtPhysicianName.ClientID %>').val('');
            }
        }
        //#endregion

        //#region Physician
        function onGetPhysicianFilterExpression() {
            var fromParamedicID = $('#<%:hdnRegisteredParamedic.ClientID %>').val();
            var filterExpression = "ParamedicID <> " + fromParamedicID + " AND IsDeleted = 0 AND GCParamedicMasterType = 'X019^001'";
            return filterExpression;
        }

        $('#<%:lblPhysician.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('paramedic', onGetPhysicianFilterExpression(), function (value) {
                $('#<%:txtPhysicianCode.ClientID %>').val(value);
                onTxtPhysicianCodeChanged(value);
            });
        });

        $('#<%:txtPhysicianCode.ClientID %>').live('change', function () {
            onTxtPhysicianCodeChanged($(this).val());
        });

        function onTxtPhysicianCodeChanged(value) {
            var filterExpression = onGetPhysicianFilterExpression() + " AND ParamedicCode = '" + value + "'";
            Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%:hdnParamedicID.ClientID %>').val(result.ParamedicID);
                    $('#<%:txtPhysicianName.ClientID %>').val(result.ParamedicName);
                }
                else {
                    $('#<%:hdnParamedicID.ClientID %>').val('');
                    $('#<%:txtPhysicianCode.ClientID %>').val('');
                    $('#<%:txtPhysicianName.ClientID %>').val('');
                }
            });
        }
        //#endregion
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhQuickEntry" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" value="" id="hdnEntryID" runat="server" />
    <input type="hidden" id="hdnRegisteredParamedic" value="" runat="server" />
    <input type="hidden" id="hdnParamedicID" value="" runat="server" />
    <table style="width:100%" class="tblEntryDetail">
        <colgroup>
            <col style="width:150px"/>
            <col style="width:120px"/>
            <col />
        </colgroup>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Physician Role")%></label></td>
            <td colspan="2"><dxe:ASPxComboBox runat="server" ID="cboPhysicianRole" ClientInstanceName="cboPhysicianRole" Width="300px" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblMandatory lblLink" runat="server" id="lblPhysician"><%=GetLabel("Dokter")%></label></td>
            <td><asp:TextBox ID="txtPhysicianCode" Width="100%" runat="server" /></td>
            <td><asp:TextBox ID="txtPhysicianName" Width="180px" runat="server" ReadOnly="true" /></td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="plhList" runat="server">
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height:300px">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn" >
                                    <ItemTemplate>
                                        <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                        <input type="hidden" value="<%#:Eval("ParamedicID") %>" bindingfield="ParamedicID" />
                                        <input type="hidden" value="<%#:Eval("ParamedicCode") %>" bindingfield="ParamedicCode" />
                                        <input type="hidden" value="<%#:Eval("ParamedicName") %>" bindingfield="ParamedicName" />
                                        <input type="hidden" value="<%#:Eval("GCParamedicRole") %>" bindingfield="GCParamedicRole" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ParamedicName" HeaderText="Physician" HeaderStyle-Width="350px" HeaderStyle-HorizontalAlign="Left"  />
                                <asp:BoundField DataField="ParamedicRole" HeaderText="Role" HeaderStyle-HorizontalAlign="Left"  />
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("No Record To Display")%>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>    
        <div class="imgLoadingGrdView" id="containerImgLoadingView" >
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <div class="containerPaging">
            <div class="wrapperPaging">
                <div id="paging"></div>
            </div>
        </div> 
    </div>
</asp:Content>
