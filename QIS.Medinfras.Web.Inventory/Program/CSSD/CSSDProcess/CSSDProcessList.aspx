<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.Master" AutoEventWireup="true"
    CodeBehind="CSSDProcessList.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.CSSDProcessList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png")%>' alt="" /><div>
            <%=GetLabel("Refresh")%></div>
    </li>
    <li id="btnProcess" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><div>
            <%=GetLabel("Process")%></div>
    </li>
    <li id="btnDeclineQC" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbcancel.png")%>' alt="" /><div>
            <%=GetLabel("Decline QC")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#ulTabTransactionCSSD li').click(function () {
                $('#ulTabTransactionCSSD li.selected').removeAttr('class');
                $('.containerCSSD').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#' + $contentID).show();
                $('#<%=hdnContentID.ClientID %>').val($contentID);
                $(this).addClass('selected');
            });

            $('#<%=btnRefresh.ClientID %>').click(function () {
                cbpViewWashing.PerformCallback('refresh');
                cbpViewPackaging.PerformCallback('refresh');
                cbpViewSterilitation.PerformCallback('refresh');
                cbpViewQualityControl.PerformCallback('refresh');
            });

            $('#<%=btnProcess.ClientID %>').click(function () {
                var contentID = $('#<%=hdnContentID.ClientID %>').val();
                if (contentID == "containerWashing") {
                    getCheckedMemberWashing();
                    cbpViewWashing.PerformCallback('process');
                }
                else if (contentID == "containerPackaging") {
                    getCheckedMemberPackaging();
                    cbpViewPackaging.PerformCallback('process');
                }
                else if (contentID == "containerSterilitation") {
                    getCheckedMemberSterilitation();
                    cbpViewSterilitation.PerformCallback('process');
                }
                else if (contentID == "containerQualityControl") {
                    getCheckedMemberQualityControl();
                    cbpViewQualityControl.PerformCallback('process');
                }
            });

            $('#<%=btnDeclineQC.ClientID %>').click(function () {
                var contentID = $('#<%=hdnContentID.ClientID %>').val();
                if (contentID != "containerQualityControl") {
                    showToast('FAILED', 'Tombol DECLINE hanya dapat digunakan dalam proses Quality Control !');
                } else {
                    getCheckedMemberQualityControl();
                    cbpViewQualityControl.PerformCallback('declineqc');
                }
            });

        });

        //#region WASHING

        function onCbpViewWashingEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'process') {
                if (param[1] == 'fail') {
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                }

                cbpViewWashing.PerformCallback('refresh');
                cbpViewPackaging.PerformCallback('refresh');
                cbpViewSterilitation.PerformCallback('refresh');
                cbpViewQualityControl.PerformCallback('refresh');
            }
        }

        function getCheckedMemberWashing() {
            var lstSelectedMember = $('#<%=hdnSelectedWashingProcess.ClientID %>').val().split(',');
            var lstSelectedMemberWashingMethod = $('#<%=hdnSelectedWashingProcessMethod.ClientID %>').val().split(',');
            $('#<%=grdViewWashing.ClientID %> .chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    var key = $(this).closest('tr').find('.keyField').html();
                    var idx = lstSelectedMember.indexOf(key);

                    var indexCBO = $(this).closest('tr').find('.hdnItemIndexWashing').val();
                    cboWashingMethod = eval('cboWashingMethod' + indexCBO);
                    var cbo = '';
                    if (cboWashingMethod.GetValue() != null) {
                        cbo = cboWashingMethod.GetValue();
                    }

                    if (idx < 0) {
                        lstSelectedMember.push(key);
                        lstSelectedMemberWashingMethod.push(cbo);
                    }
                }
                else {
                    var key = $(this).closest('tr').find('.keyField').html();
                    var idx = lstSelectedMember.indexOf(key);

                    if (idx > -1) {
                        lstSelectedMember.splice(idx, 1);
                        lstSelectedMemberWashingMethod.splice(idx, 1);
                    }
                }
            });
            $('#<%=hdnSelectedWashingProcess.ClientID %>').val(lstSelectedMember.join(','));
            $('#<%=hdnSelectedWashingProcessMethod.ClientID %>').val(lstSelectedMemberWashingMethod.join(','));
        }
        //#endregion

        //#region PACKAGING

        function onCbpViewPackagingEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'process') {
                if (param[1] == 'fail') {
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                }

                cbpViewWashing.PerformCallback('refresh');
                cbpViewPackaging.PerformCallback('refresh');
                cbpViewSterilitation.PerformCallback('refresh');
                cbpViewQualityControl.PerformCallback('refresh');
            }
        }

        function getCheckedMemberPackaging() {
            var lstSelectedMember = $('#<%=hdnSelectedPackagingProcess.ClientID %>').val().split(',');
            var lstSelectedMemberPackagingType = $('#<%=hdnSelectedPackagingProcessType.ClientID %>').val().split(',');
            $('#<%=grdViewPackaging.ClientID %> .chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    var key = $(this).closest('tr').find('.keyField').html();
                    var idx = lstSelectedMember.indexOf(key);

                    var indexCBO = $(this).closest('tr').find('.hdnItemIndexPackaging').val();
                    cboPackagingType = eval('cboPackagingType' + indexCBO);
                    var cbo = '';
                    if (cboPackagingType.GetValue() != null) {
                        cbo = cboPackagingType.GetValue();
                    }

                    if (idx < 0) {
                        lstSelectedMember.push(key);
                        lstSelectedMemberPackagingType.push(cbo);
                    }
                }
                else {
                    var key = $(this).closest('tr').find('.keyField').html();
                    var idx = lstSelectedMember.indexOf(key);

                    if (idx > -1) {
                        lstSelectedMember.splice(idx, 1);
                        lstSelectedMemberPackagingType.splice(idx, 1);
                    }
                }
            });
            $('#<%=hdnSelectedPackagingProcess.ClientID %>').val(lstSelectedMember.join(','));
            $('#<%=hdnSelectedPackagingProcessType.ClientID %>').val(lstSelectedMemberPackagingType.join(','));
        }
        //#endregion

        //#region STERILITATION

        function onCbpViewSterilitationEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'process') {
                if (param[1] == 'fail') {
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                }

                cbpViewWashing.PerformCallback('refresh');
                cbpViewPackaging.PerformCallback('refresh');
                cbpViewSterilitation.PerformCallback('refresh');
                cbpViewQualityControl.PerformCallback('refresh');
            }
        }

        function getCheckedMemberSterilitation() {
            var lstSelectedMember = $('#<%=hdnSelectedSterilitationProcess.ClientID %>').val().split(',');
            var lstSelectedMemberSterilitationType = $('#<%=hdnSelectedSterilitationProcessType.ClientID %>').val().split(',');
            var lstSelectedMemberSterilitationCycle = $('#<%=hdnSelectedSterilitationProcessCycle.ClientID %>').val().split(',');
            var lstSelectedMemberSterilitationExpired = $('#<%=hdnSelectedSterilitationProcessExpired.ClientID %>').val().split(',');

            $('#<%=grdViewSterilitation.ClientID %> .chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    var key = $(this).closest('tr').find('.keyField').html();
                    var idx = lstSelectedMember.indexOf(key);

                    var indexCBO = $(this).closest('tr').find('.hdnItemIndexSterilitation').val();
                    cboSterilitationType = eval('cboSterilitationType' + indexCBO);
                    var cbo = '';
                    if (cboSterilitationType.GetValue() != null) {
                        cbo = cboSterilitationType.GetValue();
                    }

                    var cycle = parseInt($(this).closest('tr').find('.txtSterilitationCycle').val());
                    var expired = $(this).closest('tr').find('.txtExpiredDate').val();

                    if (idx < 0) {
                        lstSelectedMember.push(key);
                        lstSelectedMemberSterilitationType.push(cbo);
                        lstSelectedMemberSterilitationCycle.push(cycle);
                        lstSelectedMemberSterilitationExpired.push(expired);
                    }
                }
                else {
                    var key = $(this).closest('tr').find('.keyField').html();
                    var idx = lstSelectedMember.indexOf(key);

                    if (idx > -1) {
                        lstSelectedMember.splice(idx, 1);
                        lstSelectedMemberSterilitationType.splice(idx, 1);
                        lstSelectedMemberSterilitationCycle.splice(idx, 1);
                        lstSelectedMemberSterilitationExpired.splice(idx, 1);
                    }
                }
            });
            $('#<%=hdnSelectedSterilitationProcess.ClientID %>').val(lstSelectedMember.join(','));
            $('#<%=hdnSelectedSterilitationProcessType.ClientID %>').val(lstSelectedMemberSterilitationType.join(','));
            $('#<%=hdnSelectedSterilitationProcessCycle.ClientID %>').val(lstSelectedMemberSterilitationCycle.join(','));
            $('#<%=hdnSelectedSterilitationProcessExpired.ClientID %>').val(lstSelectedMemberSterilitationExpired.join(','));
        }
        //#endregion

        //#region QUALITY CONTROL

        function onCbpViewQualityControlEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'process') {
                if (param[1] == 'fail') {
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                }

                cbpViewWashing.PerformCallback('refresh');
                cbpViewPackaging.PerformCallback('refresh');
                cbpViewSterilitation.PerformCallback('refresh');
                cbpViewQualityControl.PerformCallback('refresh');
            }
            else if (param[0] == 'declineqc') {
                if (param[1] == 'fail') {
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                }

                cbpViewWashing.PerformCallback('refresh');
                cbpViewPackaging.PerformCallback('refresh');
                cbpViewSterilitation.PerformCallback('refresh');
                cbpViewQualityControl.PerformCallback('refresh');
            }
        }

        function getCheckedMemberQualityControl() {
            var lstSelectedMember = $('#<%=hdnSelectedQualityControlProcess.ClientID %>').val().split(',');
            $('#<%=grdViewQualityControl.ClientID %> .chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    var key = $(this).closest('tr').find('.keyField').html();
                    var idx = lstSelectedMember.indexOf(key);
                    if (idx < 0) {
                        lstSelectedMember.push(key);
                    }
                }
                else {
                    var key = $(this).closest('tr').find('.keyField').html();
                    var idx = lstSelectedMember.indexOf(key);
                    if (idx > -1) {
                        lstSelectedMember.splice(idx, 1);
                    }
                }
            });
            $('#<%=hdnSelectedQualityControlProcess.ClientID %>').val(lstSelectedMember.join(','));
        }

        //#endregion

    </script>
    <input type="hidden" value="containerWashing" id="hdnContentID" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnSelectedWashingProcess" runat="server" />
    <input type="hidden" value="" id="hdnSelectedWashingProcessMethod" runat="server" />
    <input type="hidden" value="" id="hdnSelectedPackagingProcess" runat="server" />
    <input type="hidden" value="" id="hdnSelectedPackagingProcessType" runat="server" />
    <input type="hidden" value="" id="hdnSelectedSterilitationProcess" runat="server" />
    <input type="hidden" value="" id="hdnSelectedSterilitationProcessType" runat="server" />
    <input type="hidden" value="" id="hdnSelectedSterilitationProcessCycle" runat="server" />
    <input type="hidden" value="" id="hdnSelectedSterilitationProcessExpired" runat="server" />
    <input type="hidden" value="" id="hdnSelectedQualityControlProcess" runat="server" />
    <div style="height: 500px;">
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 100%" />
            </colgroup>
            <tr>
                <td>
                    <div class="containerUlTabPage">
                        <ul class="ulTabPage" id="ulTabTransactionCSSD">
                            <li class="selected" contentid="containerWashing">
                                <%=GetLabel(GetLabel("WASHING")) %></li>
                            <li contentid="containerPackaging">
                                <%=GetLabel("PACKAGING") %></li>
                            <li contentid="containerSterilitation">
                                <%=GetLabel("STERILITATION") %></li>
                            <li contentid="containerQualityControl">
                                <%=GetLabel("QUALITY CONTROL") %></li>
                        </ul>
                    </div>
                    <div id="containerWashing" class="containerCSSD">
                        <div id="containerWashingEntry" style="margin-top: 4px; height: 450px; overflow-y: scroll;
                            overflow-x: hidden">
                            <div style="position: relative;">
                                <dxcp:ASPxCallbackPanel ID="cbpViewWashing" runat="server" Width="100%" ClientInstanceName="cbpViewWashing"
                                    ShowLoadingPanel="false" OnCallback="cbpViewWashing_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){showLoadingPanel()}" EndCallback="function(s,e){ onCbpViewWashingEndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent1" runat="server">
                                            <asp:Panel runat="server" ID="pnlViewWashing" CssClass="pnlContainerGrid">
                                                <asp:GridView ID="grdViewWashing" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdViewWashing_RowDataBound">
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
                                                        <asp:BoundField DataField="PackageName" HeaderText="Package Name" HeaderStyle-HorizontalAlign="Left"
                                                            HeaderStyle-Width="100px" />
                                                        <asp:BoundField DataField="CountDt" HeaderText="Count" HeaderStyle-HorizontalAlign="Right"
                                                            ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" />
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
                                                        <asp:TemplateField HeaderStyle-Width="180px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                <%=GetLabel("Received Information") %>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <%#:Eval("ReceivedByName")%>
                                                                <br />
                                                                (<%#:Eval("cfReceivedDateInStringDTF")%>)
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                <%=GetLabel("Washing Method") %>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <input type="hidden" class="hdnItemIndexWashing" value='<%#: Container.DataItemIndex %>' />
                                                                <dxe:ASPxComboBox ID="cboWashingMethod" ClientInstanceName="cboWashingMethod" runat="server"
                                                                    Width="90%" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <%=GetLabel("No data to display.")%>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>
                                <div class="imgLoadingGrdView" id="containerImgLoadingView1">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="containerPackaging" class="containerCSSD" style="display: none">
                        <div id="containerPackagingEntry" style="margin-top: 4px; height: 450px; overflow-y: scroll;
                            overflow-x: hidden">
                            <div style="position: relative;">
                                <dxcp:ASPxCallbackPanel ID="cbpViewPackaging" runat="server" Width="100%" ClientInstanceName="cbpViewPackaging"
                                    ShowLoadingPanel="false" OnCallback="cbpViewPackaging_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){showLoadingPanel()}" EndCallback="function(s,e){ onCbpViewPackagingEndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent2" runat="server">
                                            <asp:Panel runat="server" ID="pnlViewPackaging" CssClass="pnlContainerGrid">
                                                <asp:GridView ID="grdViewPackaging" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdViewPackaging_RowDataBound">
                                                    <Columns>
                                                        <asp:BoundField DataField="RequestID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:TemplateField HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:CheckBox runat="server" ID="chkIsSelectedPackaging" CssClass="chkIsSelected" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="RequestNo" HeaderText="Request No" HeaderStyle-HorizontalAlign="Left"
                                                            HeaderStyle-Width="150px" />
                                                        <asp:BoundField DataField="FromLocationName" HeaderText="From Location" HeaderStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="PackageName" HeaderText="Package Name" HeaderStyle-HorizontalAlign="Left"
                                                            HeaderStyle-Width="100px" />
                                                        <asp:BoundField DataField="CountDt" HeaderText="Count" HeaderStyle-HorizontalAlign="Right"
                                                            ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" />
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
                                                        <asp:TemplateField HeaderStyle-Width="180px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                <%=GetLabel("Received Information") %>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <%#:Eval("ReceivedByName")%>
                                                                <br />
                                                                (<%#:Eval("cfReceivedDateInStringDTF")%>)
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                <%=GetLabel("Packaging Type") %>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <input type="hidden" class="hdnItemIndexPackaging" value='<%#: Container.DataItemIndex %>' />
                                                                <dxe:ASPxComboBox ID="cboPackagingType" ClientInstanceName="cboPackagingType" runat="server"
                                                                    Width="90%" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <%=GetLabel("No data to display.")%>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>
                                <div class="imgLoadingGrdView" id="containerImgLoadingView">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="containerSterilitation" class="containerCSSD" style="display: none">
                        <div id="containerSterilitationEntry" style="margin-top: 4px; height: 450px; overflow-y: scroll;
                            overflow-x: hidden">
                            <div style="position: relative;">
                                <dxcp:ASPxCallbackPanel ID="cbpViewSterilitation" runat="server" Width="100%" ClientInstanceName="cbpViewSterilitation"
                                    ShowLoadingPanel="false" OnCallback="cbpViewSterilitation_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){showLoadingPanel()}" EndCallback="function(s,e){ onCbpViewSterilitationEndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent3" runat="server">
                                            <asp:Panel runat="server" ID="pnlViewSterilitation" CssClass="pnlContainerGrid">
                                                <asp:GridView ID="grdViewSterilitation" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdViewSterilitation_RowDataBound">
                                                    <Columns>
                                                        <asp:BoundField DataField="RequestID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:TemplateField HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:CheckBox runat="server" ID="chkIsSelectedSterilitation" CssClass="chkIsSelected" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="RequestNo" HeaderText="Request No" HeaderStyle-HorizontalAlign="Left"
                                                            HeaderStyle-Width="150px" />
                                                        <asp:BoundField DataField="FromLocationName" HeaderText="From Location" HeaderStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="PackageName" HeaderText="Package Name" HeaderStyle-HorizontalAlign="Left"
                                                            HeaderStyle-Width="100px" />
                                                        <asp:BoundField DataField="CountDt" HeaderText="Count" HeaderStyle-HorizontalAlign="Right"
                                                            ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="50px" />
                                                        <asp:TemplateField HeaderStyle-Width="180px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                <%=GetLabel("Information") %>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <b>Sent By :</b>
                                                                <%#:Eval("SentByName")%>
                                                                <br />
                                                                <b>Sent Date :</b>
                                                                <%#:Eval("cfSentDateInStringDTF")%>
                                                                <br />
                                                                <b>Received By :</b>
                                                                <%#:Eval("ReceivedByName")%>
                                                                <br />
                                                                <b>Received Date :</b>
                                                                <%#:Eval("cfReceivedDateInStringDTF")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                <%=GetLabel("Service Type") %>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <%#:Eval("ServiceType")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-Width="170px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                <%=GetLabel("Sterilitation Type") %>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <input type="hidden" class="hdnItemIndexSterilitation" value='<%#: Container.DataItemIndex %>' />
                                                                <dxe:ASPxComboBox ID="cboSterilitationType" ClientInstanceName="cboSterilitationType"
                                                                    runat="server" Width="90%" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                <%=GetLabel("Cycle") %>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <input type="text" class="txtSterilitationCycle txtCurrency" value="0" id="txtSterilitationCycle" runat="server"
                                                                    style="width: 70%" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                <%=GetLabel("Expired Date") %>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <input type="text" class="datepicker txtExpiredDate" id="txtExpiredDate" runat="server"
                                                                    style="width: 70%" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <%=GetLabel("No data to display.")%>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>
                                <div class="imgLoadingGrdView" id="containerImgLoadingView">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="containerQualityControl" class="containerCSSD" style="display: none">
                        <div id="containerQualityControlEntry" style="margin-top: 4px; height: 450px; overflow-y: scroll;
                            overflow-x: hidden">
                            <div style="position: relative;">
                                <dxcp:ASPxCallbackPanel ID="cbpViewQualityControl" runat="server" Width="100%" ClientInstanceName="cbpViewQualityControl"
                                    ShowLoadingPanel="false" OnCallback="cbpViewQualityControl_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){showLoadingPanel()}" EndCallback="function(s,e){ onCbpViewQualityControlEndCallback(s); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent4" runat="server">
                                            <asp:Panel runat="server" ID="pnlViewQualityControl" CssClass="pnlContainerGrid">
                                                <asp:GridView ID="grdViewQualityControl" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:BoundField DataField="RequestID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:TemplateField HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:CheckBox runat="server" ID="chkIsSelectedQualityControl" CssClass="chkIsSelected" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="RequestNo" HeaderText="Request No" HeaderStyle-HorizontalAlign="Left"
                                                            HeaderStyle-Width="150px" />
                                                        <asp:BoundField DataField="FromLocationName" HeaderText="From Location" HeaderStyle-HorizontalAlign="Left" />
                                                        <asp:BoundField DataField="PackageName" HeaderText="Package Name" HeaderStyle-HorizontalAlign="Left"
                                                            HeaderStyle-Width="100px" />
                                                        <asp:BoundField DataField="CountDt" HeaderText="Count" HeaderStyle-HorizontalAlign="Right"
                                                            ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="80px" />
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
                                                        <asp:TemplateField HeaderStyle-Width="180px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                            <HeaderTemplate>
                                                                <%=GetLabel("Received Information") %>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <%#:Eval("ReceivedByName")%>
                                                                <br />
                                                                (<%#:Eval("cfReceivedDateInStringDTF")%>)
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <%=GetLabel("No data to display.")%>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>
                                <div class="imgLoadingGrdView" id="containerImgLoadingView">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
