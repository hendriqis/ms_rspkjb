<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPList.master"
    AutoEventWireup="true" CodeBehind="ARInvoicePatientList.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.ARInvoicePatientList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnMPListView" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbitems.png")%>' alt="" /><div>
            <%=GetLabel("View")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomContextMenu" runat="server">
    <li class="list-devider">
        <hr />
    </li>
    <li id="ctxMenuView" runat="server"><a href="#">
        <%=GetLabel("View")%></a> </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            var grd = new customGridView();
            grd.init('<%=grdView.ClientID %>', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');

            $('#<%=btnMPListView.ClientID %>').click(function () {
                clickView();
            });

            $('#<%=ctxMenuView.ClientID %>').click(function () {
                clickView();
            });

            $('#trEmployeeID').attr('style', 'display:none');

            $('#<%=chkIsEmployeeOnly.ClientID %>').change(function () {
                if ($('#<%:chkIsEmployeeOnly.ClientID %>').is(':checked')) {
                    $('#trEmployeeID').removeAttr('style');
                    cbpView.PerformCallback('refresh');
                }
                else {
                    $('#trEmployeeID').attr('style', 'display:none');
                    $('#<%=txtEmployeeName.ClientID %>').val('');
                    cbpView.PerformCallback('refresh');
                }
            });

            $('#<%=txtEmployeeName.ClientID %>').change(function () {
                cbpView.PerformCallback('refresh');
            });
        });

        function clickView() {
            if ($('#<%=grdView.ClientID %> tr.selected').length > 0) {
                showLoadingPanel();
                $('#<%=hdnID.ClientID %>').val($('#<%=grdView.ClientID %> tr.selected').find('.keyField').html());
                __doPostBack('<%=btnOpenTransactionDt.UniqueID%>', '');
            }
        }

        function onBeforeOpenTransactionDt() {
            return ($('#<%=hdnID.ClientID %>').val() != '');
        }

        function onRefreshControl(filterExpression) {
            $('#<%=hdnFilterExpression.ClientID %>').val(filterExpression);
            cbpView.PerformCallback('refresh');
        }

        function onGetCurrID() {
            return $('#<%=hdnID.ClientID %>').val();
        }

        function onGetFilterExpression() {
            return $('#<%=hdnFilterExpression.ClientID %>').val();
        }

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        var currPage = parseInt('<%=CurrPage %>');
        $(function () {
            if (pageCount > 100) pageCount = 100;
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            }, null, currPage);
            Methods.checkImageError('imgPatientImage', 'patient', 'hdnPatientGender');
        });


        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            Methods.checkImageError('imgPatientImage', 'patient', 'hdnPatientGender');

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();

                if (pageCount > 100) pageCount = 100;
                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion
    </script>
    <div style="display: none">
        <asp:Button ID="btnOpenTransactionDt" runat="server" UseSubmitBehavior="false" OnClientClick="return onBeforeOpenTransactionDt();"
            OnClick="btnOpenTransactionDt_Click" /></div>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <table>
        <tr>
            <td>
                <asp:CheckBox ID="chkIsEmployeeOnly" runat="server" /><%=GetLabel("Tampilkan Karyawan Saja") %>
            </td>
        </tr>
        <tr id="trEmployeeID">
            <td class="tdLabel">
                <label>
                    <%=GetLabel("Nama Karyawan")%></label>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtEmployeeName" Width="460px" />
            </td>
        </tr>
    </table>
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="overflow-y: scroll;">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="MRN" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:TemplateField HeaderStyle-Width="470px">
                                    <HeaderTemplate>
                                        <%=GetLabel("Informasi Pasien")%></HeaderTemplate>
                                    <ItemTemplate>
                                        <div style="padding: 3px">
                                            <img class="imgPatientImage" src='<%#Eval("cfPatientImageUrl") %>' alt="" height="55px"
                                                width="40px" style="float: left; margin-right: 10px;" />
                                            <div>
                                                <%#: Eval("PatientName") %></div>
                                            <input type="hidden" value='<%#: Eval("GCSex")%>' class="hdnPatientGender" />
                                            <table cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col style="width: 100px" />
                                                    <col style="width: 10px" />
                                                    <col style="width: 120px" />
                                                    <col style="width: 50px" />
                                                    <col style="width: 10px" />
                                                    <col style="width: 120px" />
                                                </colgroup>
                                                <tr>
                                                    <td style="text-align: right; font-size: 0.9em; font-style: italic">
                                                        <%=GetLabel("Nama Panggilan")%>
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <%#: Eval("PreferredName")%>
                                                    </td>
                                                    <td style="text-align: right; font-size: 0.9em; font-style: italic">
                                                        <%=GetLabel("MRN")%>
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <%#: Eval("MedicalNo")%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right; font-size: 0.9em; font-style: italic">
                                                        <%=GetLabel("Tanggal Lahir")%>
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <%#: Eval("cfDateOfBirthInString")%>
                                                    </td>
                                                    <td style="text-align: right; font-size: 0.9em; font-style: italic">
                                                        <%=GetLabel("Umur")%>
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <%#: Eval("cfPatientAge")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <%=GetLabel("Informasi Kontak")%></HeaderTemplate>
                                    <ItemTemplate>
                                        <div style="padding: 3px">
                                            <div>
                                                <%#: Eval("cfHomeAddress")%></div>
                                            <img src='<%= ResolveUrl("~/Libs/Images/homephone.png")%>' alt='' style="float: left;" /><div
                                                style="margin-left: 30px">
                                                <%#: Eval("PhoneNo1")%>&nbsp;</div>
                                            <img src='<%= ResolveUrl("~/Libs/Images/mobilephone.png")%>' alt='' style="float: left;" /><div
                                                style="margin-left: 30px">
                                                <%#: Eval("MobilePhoneNo1")%>&nbsp;</div>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="cfBusinessPartner" HeaderText="Pembayar" HeaderStyle-Width="150px"
                                    ItemStyle-VerticalAlign="Top" />
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("No Data To Display")%>
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
