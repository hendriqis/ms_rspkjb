<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="CSSDHandoverList.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.CSSDHandoverList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png")%>' alt="" /><div>
            <%=GetLabel("Refresh")%></div>
    </li>
    <li id="btnProcess" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbapprove.png")%>' alt="" /><div>
            <%=GetLabel("Process")%></div>
    </li>
    <li id="btnDecline" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><div>
            <%=GetLabel("Decline")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetMenuCaption())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            $('#<%=btnRefresh.ClientID %>').click(function () {
                cbpView.PerformCallback('refresh');
            });

            $('#<%=btnProcess.ClientID %>').click(function () {
                getCheckedMember();
                cbpView.PerformCallback('process');
            });

            $('#<%=btnDecline.ClientID %>').click(function () {
                getCheckedMember();
                cbpView.PerformCallback('decline');
            });
        });
        
        //#region Location From
        function getLocationFilterExpression() {
            var filterExpression = "<%:filterExpressionLocation %>";
            return filterExpression;
        }
        $('#<%=lblLocation.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('locationroleuser', getLocationFilterExpression(), function (value) {
                $('#<%=txtLocationCode.ClientID %>').val(value);
                onTxtLocationCodeChanged(value);
            });
        });

        $('#<%=txtLocationCode.ClientID %>').live('change', function () {
            onTxtLocationCodeChanged($(this).val());
        });

        function onTxtLocationCodeChanged(value) {
            var filterExpression = getLocationFilterExpression() + "LocationCode = '" + value + "'";
            Methods.getObject('GetLocationUserAccessList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnLocationIDFrom.ClientID %>').val(result.LocationID);
                    $('#<%=txtLocationName.ClientID %>').val(result.LocationName);

                    filterExpression = "LocationID = " + result.LocationID;
                    Methods.getObject('GetLocationList', filterExpression, function (result) {
                        $('#<%=hdnFromLocationItemGroupID.ClientID %>').val(result.ItemGroupID);
                        $('#<%=hdnGCLocationGroupFrom.ClientID %>').val(result.GCLocationGroup);
                    });
                }
                else {
                    $('#<%=hdnLocationIDFrom.ClientID %>').val('');
                    $('#<%=txtLocationCode.ClientID %>').val('');
                    $('#<%=txtLocationName.ClientID %>').val('');
                    $('#<%=hdnFromLocationItemGroupID.ClientID %>').val('');
                    $('#<%=hdnGCLocationGroupFrom.ClientID %>').val('');
                }
                cbpView.PerformCallback('refresh');
            });
        }
        //#endregion
        
        function getCheckedMember() {
            var lstSelectedMember = $('#<%=hdnSelectedRequestID.ClientID %>').val().split(',');
            var lstSelectedMemberVolume = $('#<%=hdnSelectedVolume.ClientID %>').val().split(',');
            var lstSelectedMemberCondition = $('#<%=hdnSelectedCondition.ClientID %>').val().split(',');

            $('#<%=grdView.ClientID %> .chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    var key = $(this).closest('tr').find('.keyField').html();

                    var idx = lstSelectedMember.indexOf(key);

                    var indexCBOVolume = $(this).closest('tr').find('.hdnItemIndexVolume').val();
                    cboPreWashingVolume = eval('cboPreWashingVolume' + indexCBOVolume);
                    var cboVolume = '';
                    if (cboPreWashingVolume.GetValue() != null) {
                        cboVolume = cboPreWashingVolume.GetValue();
                    }

                    var indexCBOCondition = $(this).closest('tr').find('.hdnItemIndexCondition').val();
                    cboPreWashingCondition = eval('cboPreWashingCondition' + indexCBOCondition);
                    var cboCondition = '';
                    if (cboPreWashingCondition.GetValue() != null) {
                        cboCondition = cboPreWashingCondition.GetValue();
                    }

                    if (idx < 0) {
                        lstSelectedMember.push(key);
                        lstSelectedMemberVolume.push(cboVolume);
                        lstSelectedMemberCondition.push(cboCondition);
                    }
                }
                else {
                    var key = $(this).closest('tr').find('.keyField').html();
                    var idx = lstSelectedMember.indexOf(key);

                    if (idx > -1) {
                        lstSelectedMember.splice(idx, 1);
                        lstSelectedMemberVolume.splice(idx, 1);
                        lstSelectedMemberCondition.splice(idx, 1);
                    }
                }
            });
            $('#<%=hdnSelectedRequestID.ClientID %>').val(lstSelectedMember.join(','));
            $('#<%=hdnSelectedVolume.ClientID %>').val(lstSelectedMemberVolume.join(','));
            $('#<%=hdnSelectedCondition.ClientID %>').val(lstSelectedMemberCondition.join(','));
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'process') {
                if (param[1] == 'fail') {
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                }

                cbpView.PerformCallback('refresh');
            }
            else if (param[0] == 'decline') {
                if (param[1] == 'fail') {
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                }

                cbpView.PerformCallback('refresh');
            }
        }

        function onBeforeLoadRightPanelContent(code) {
            var key = '';
            return key
        }
    </script>
    <input type="hidden" value="" id="hdnFilterExpression" runat="server" />
    <input type="hidden" value="" id="hdnSelectedRequestID" runat="server" />
    <input type="hidden" value="" id="hdnSelectedVolume" runat="server" />
    <input type="hidden" value="" id="hdnSelectedCondition" runat="server" />
    <div style="position: relative">
        <table style="width: 100%">
            <tr>
                <td>
                    <table class="tblContentArea" style="width: 100%">
                        <colgroup>
                            <col style="width: 5%" />
                            <col style="width: 50%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal lblLink" runat="server" id="lblLocation">
                                    <%=GetLabel("Dari Lokasi")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnLocationIDFrom" value="" runat="server" />
                                <input type="hidden" id="hdnFromLocationItemGroupID" value="" runat="server" />
                                <input type="hidden" id="hdnGCLocationGroupFrom" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 10%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtLocationCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLocationName" Width="30%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="width:100%">
                    <table style="width: 100%">
                        <tr>
                            <td style="vertical-align: top">
                                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                                    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent1" runat="server">
                                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridProcessList">
                                                <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                                    <Columns>
                                                        <asp:BoundField DataField="RequestID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:TemplateField HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:CheckBox runat="server" ID="chkIsSelectedWashing" CssClass="chkIsSelected" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="RequestNo" HeaderText="Request No" HeaderStyle-HorizontalAlign="Left"
                                                            HeaderStyle-Width="150px" />
                                                        <asp:BoundField DataField="FromLocationName" HeaderText="From Location" HeaderStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="PackageCode" HeaderText="Package Code" HeaderStyle-HorizontalAlign="Left"
                                                            HeaderStyle-Width="120px" />
                                                        <asp:BoundField DataField="PackageName" HeaderText="Package Name" HeaderStyle-HorizontalAlign="Left" />
                                                        <asp:TemplateField HeaderStyle-Width="180px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                <%=GetLabel("Sent Information") %>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <%#:Eval("SentByName")%>
                                                                <br />
                                                                (<%#:Eval("cfSentDateInStringDTF")%>)
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="CountDt" HeaderText="Count" HeaderStyle-HorizontalAlign="Right"
                                                            ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" />
                                                        <asp:TemplateField HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                <%=GetLabel("Pre-Washing Volume") %>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <input type="hidden" class="hdnItemIndexVolume" value='<%#: Container.DataItemIndex %>' />
                                                                <dxe:ASPxComboBox ID="cboPreWashingVolume" ClientInstanceName="cboPreWashingVolume" runat="server" Width="90%" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                <%=GetLabel("Pre-Washing Condition") %>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <input type="hidden" class="hdnItemIndexCondition" value='<%#: Container.DataItemIndex %>' />
                                                                <dxe:ASPxComboBox ID="cboPreWashingCondition" ClientInstanceName="cboPreWashingCondition" runat="server" Width="90%" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <%=GetLabel("No data to display")%>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>
                                <div class="imgLoadingGrdView" id="containerImgLoadingView">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
