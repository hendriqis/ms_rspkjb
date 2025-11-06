<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master" AutoEventWireup="true" 
    CodeBehind="PatientLogList.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.PatientLogList" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>

<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" CRUDMode="R" runat="server"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png")%>' alt="" /><br style="clear:both"/><div><%=GetLabel("Refresh")%></div></li>
    <li id="btnSendToRIS" CRUDMode="R" runat="server"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><br style="clear:both"/><div><%=GetLabel("RIS (HL7)")%></div></li>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtFromLogDate.ClientID %>');
            setDatePicker('<%=txtToLogDate.ClientID %>');

            $('#<%=txtFromLogDate.ClientID %>').datepicker('option', 'maxDate', '0');
            $('#<%=txtToLogDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            $('#<%=btnRefresh.ClientID %>').click(function () {
                if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                    onRefreshGrid();
                }
            });

            $('#<%=btnSendToRIS.ClientID %>').click(function () {
                if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                    cbpSendToRIS.PerformCallback($('#<%=hdnMRN.ClientID %>').val());
                }
            });
        });

        $('#<%=grdView.ClientID %> > tbody > tr:gt(0):not(.trDetail):not(.trEmpty)').live('click', function () {
            var detail = $(this).find('.keyField').html();
            var mrn = $(this).find('.hiddenColumn').html();
            $('#<%=hdnID.ClientID %>').val(detail);
            $('#<%=hdnMRN.ClientID %>').val(mrn);
            $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
            $(this).addClass('selected');
            $('#divErrorDetail').html('');
            $('#divErrorDetail').append(convert(detail));
        });

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                onRefreshGrid();
            }, 0);
        }

        var convert = function (convert) {
            return $('<span />', { html: convert }).text();
        }

        function onGetCurrID() {
            return $('#<%=hdnID.ClientID %>').val();
        }

        function onGetFilterExpression() {
            return $('#<%=hdnFilterExpression.ClientID %>').val();
        }

        function onRefreshGrid() {
            $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
            cbpView.PerformCallback('refresh');
        }

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        var currPage = parseInt('<%=CurrPage %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            }, null, currPage);
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

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

        function onCbpSendToRISEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'success') {
                displayMessageBox('SEND TO RIS : SUCCESS', 'Proses Notifikasi Perubahan Data Pasien ke RIS sukses dilakukan (' + param[1] + ')');
            }
            else {
                displayErrorMessageBox('SEND TO RIS : FAILED', 'Error Message : ' + param[1]);
            }
        }
    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnMRN" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <div style="position: relative;">
        <table class="tblEntryContent" style="width:60%;">
            <colgroup>
                <col style="width:120px"/>
                <col/>
            </colgroup>
            <tr>
                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Change Date")%></label></td>
                <td>
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td><asp:TextBox runat="server" CssClass="datepicker" ID="txtFromLogDate" Width="120px" /></td>
                            <td>&nbsp;</td>
                            <td><asp:TextBox runat="server" CssClass="datepicker" ID="txtToLogDate" Width="120px" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label>
                        <%=GetLabel("Quick Filter")%></label>
                </td>
                <td>
                    <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                        Width="378px" Watermark="Search">
                        <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                        <IntellisenseHints>
                            <qis:QISIntellisenseHint Text="Nama" FieldName="PatientName" />
                            <qis:QISIntellisenseHint Text="No. RM" FieldName="MedicalNo" />
                        </IntellisenseHints>
                    </qis:QISIntellisenseTextBox>
                </td>
            </tr>
        </table>
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:BoundField DataField="MRN" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn" />
                                <asp:BoundField DataField="cfLogDate" HeaderText="Log Date" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" />
                                <asp:BoundField DataField="cfLogTime" HeaderText="Log Date" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                                <asp:BoundField DataField="MedicalNo" HeaderText="Medical No." ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" />
                                <asp:BoundField DataField="PatientName" HeaderText="Patient Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("Tidak ada history perubahan data pasien")%>
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
    
    <dxcp:ASPxCallbackPanel ID="cbpSendToRIS" runat="server" Width="100%" ClientInstanceName="cbpSendToRIS"
        ShowLoadingPanel="false" OnCallback="cbpSendToRIS_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpSendToRISEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>