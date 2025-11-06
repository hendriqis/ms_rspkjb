<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="PharmacyJournalList.aspx.cs" Inherits="QIS.Medinfras.Web.Pharmacy.Program.PharmacyJournalList" %>

<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <table cellpadding="0" cellspacing="0" style="width: 100%">
        <colgroup>
            <col style="width: 150px" />
            <col style="width: 145px" />
            <col style="width: 10px" />
            <col style="width: 145px" />
            <col />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal" style="font-weight: bold">
                    <%=GetLabel("Tanggal ")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtFromDate" Width="120px" CssClass="datepicker" runat="server" />
            </td>
            <td style="text-align: center">
                <%=GetLabel("s/d") %>
            </td>
            <td>
                <asp:TextBox ID="txtToDate" Width="120px" CssClass="datepicker" runat="server" />
            </td>
            <td>
                <div class="menuTitle">
                    <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" id="dxss_erevaluationnotes">
        $(function () {
            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
                $('#<%=hdnUserID.ClientID %>').val($(this).find('.keyUser').html());
                $('#<%=hdnIsEditable.ClientID %>').val($(this).find('.hiddenColumn').html());

            });

            setDatePicker('<%=txtFromDate.ClientID %>');
            $('#<%=txtFromDate.ClientID %>').datepicker('option', 'maxDate', '0');

            setDatePicker('<%=txtToDate.ClientID %>');
            $('#<%=txtToDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%=txtFromDate.ClientID %>').change(function (evt) {
                cbpView.PerformCallback('refresh');
            });

            $('#<%=txtToDate.ClientID %>').change(function (evt) {
                cbpView.PerformCallback('refresh');
            });

            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        });
        function onBeforeBasePatientPageListEdit() {
            if ($('#<%=hdnCurrentUserID.ClientID %>').val() == $('#<%=hdnUserID.ClientID %>').val()) {
               return true;
            }
            else {
                displayErrorMessageBox('Catatan Jurnal Farmasi', 'Maaf, tidak diijinkan mengedit catatan user lain.');
                return false;
            }
        }

        function onAfterSaveRecordPatientPageEntry() {
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

        function onBeforeLoadRightPanelContent(code) {
            return $('#<%:hdnRegistrationID.ClientID %>').val();
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            var hdnID = $('#<%=hdnID.ClientID %>').val();
            if (hdnID == '' || hdnID == '0') {
                errMessage.text = 'Pasien tidak memiliki Catatan Perawat';
                return false;
            }
            else {
                filterExpression.text = 'VisitID = ' + visitID;
                return true;
            }
        }
    </script>
    <style type="text/css">
        .keyUser
        {
            display:none;
        }
    </style>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnVisitID" runat="server" />
    <input type="hidden" id="hdnRegistrationID" runat="server" />
    <input type="hidden" id="hdnDepartmentID" runat="server" />
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnCurrentUserID" value="" />
    <input type="hidden" runat="server" id="hdnIsEditable" value="0" />
    <input type="hidden" runat="server" id="hdnSubMenuType" value="0" />
    <input type="hidden" runat="server" id="hdnUserID" value="" />
    <div style="position: relative; width:98%">
        <div id="filterArea">
            <table style="margin-top:10px; margin-bottom:10px">
                <tr>
                    <td class="tdLabel" style="width:150px">
                        <label>
                            <%=GetLabel("Display Option")%></label>
                    </td>
                    <td>
                        <dxe:ASPxComboBox ID="cboDisplay" ClientInstanceName="cboDisplay" runat="server"
                            Width="300px">
                            <ClientSideEvents ValueChanged="function() { cbpView.PerformCallback('refresh'); }" />
                        </dxe:ASPxComboBox>
                    </td>
                </tr>
            </table>
        </div>
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                            ShowHeaderWhenEmpty="false" EmptyDataRowStyle-CssClass="trEmpty" ShowHeader="true">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:BoundField DataField="CreatedBy" HeaderStyle-CssClass="keyUser" ItemStyle-CssClass="keyUser" />
                                <asp:BoundField DataField="cfJournalDate1" HeaderText="Tanggal" HeaderStyle-Width="100px"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="JournalTime" HeaderText="Jam" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="CreatedByName" HeaderText="User" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left" />
                                <asp:TemplateField HeaderText="Catatan" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <div style="height:130px; overflow-y:auto;">
                                            <%#Eval("JournalText").ToString().Replace("\n","<br />")%><br />
                                        </div>                                                                            
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="150px">
                                    <ItemTemplate>
                                        <div style="color: blue; font-style: italic; vertical-align:top">
                                            <%#:Eval("cfLastUpdatedDate") %>,                                            
                                        </div>
                                        <div>
                                            <b><%#:Eval("cfLastUpdatedByName") %></b>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("Tidak ada catatan jurnal farmasi untuk pasien ini") %>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <div class="containerPaging">
            <div class="wrapperPaging">
                <div id="paging">
                </div>
            </div>
        </div>
    </div>
</asp:Content>
