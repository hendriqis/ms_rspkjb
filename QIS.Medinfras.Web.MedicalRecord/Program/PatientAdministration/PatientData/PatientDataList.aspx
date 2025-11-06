<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPList.master"
    AutoEventWireup="true" CodeBehind="PatientDataList.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.PatientDataList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnMPListView" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbnotes.png")%>' alt="" /><div>
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

        $('.lnkPatientFamily a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Registration/PatientFamilyCtl.ascx");
            openUserControlPopup(url, id, 'Keluarga Pasien', 900, 500);
        });

        $('.lnkPatientDataHistory').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            id = id.trim();
            var url = ResolveUrl("~/Program/PatientAdministration/PatientData/PatientDataHistoryCtl.ascx");
            openUserControlPopup(url, id, 'Patient Data History', 1200, 500);
        });

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var TransactionID = $('#<%=hdnID.ClientID %>').val();

            if (code == "MR-00008") {
                if (TransactionID == '' || TransactionID == '0') {
                    errMessage.text = 'Please Save Transaction First!';
                    return false;
                }
                else {
                    filterExpression.text = "TransactionID = " + TransactionID;
                    return true;
                }
            }
            else if (code == "PM-00131" || code == "PM-00137" || code == "PM-00428" || code == "PM-00429" || code == "PM-00430" || code == "PM-90039") {
                filterExpression.text = TransactionID;
                return true;
            }
            else if (code == "PM-00106" || code == "PM-00440") {
                filterExpression.text = TransactionID;
                return true;
            }
            else if (code == "PM-00617") {
                filterExpression.text = "MRN = " + TransactionID;
                return true;
            }
            else {
                return false;
            }
        }

    </script>
    <div style="display: none">
        <asp:Button ID="btnOpenTransactionDt" runat="server" UseSubmitBehavior="false" OnClientClick="return onBeforeOpenTransactionDt();"
            OnClick="btnOpenTransactionDt_Click" /></div>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnMRN" value="" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" id="hdnIsBridgingToMedinfrasMobileApps" runat="server" value="" />
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
                                <asp:TemplateField HeaderStyle-Width="550px" HeaderStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <%=GetLabel("INFORMASI PASIEN")%></HeaderTemplate>
                                    <ItemTemplate>
                                        <div style="padding: 3px">
                                            <img class="imgPatientImage" src='<%#Eval("PatientImageUrl") %>' alt="" height="55px"
                                                width="40px" style="float: left; margin-right: 10px;" />
                                            <div>
                                                <%#: Eval("PatientName") %></div>
                                            <input type="hidden" value='<%#: Eval("GCSex")%>' class="hdnPatientGender" />
                                            <table cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col style="width: 170px" />
                                                    <col style="width: 10px" />
                                                    <col style="width: 120px" />
                                                    <col style="width: 50px" />
                                                    <col style="width: 10px" />
                                                    <col style="width: 120px" />
                                                    <col style="width: 100px" />
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
                                                    <td style="text-align: right; font-size: 0.9em; font-style: italic">
                                                        <%=GetLabel("RM Lama")%>
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <%#: Eval("OldMedicalNo")%>
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
                                                        <%#: Eval("DateOfBirthInString")%>
                                                    </td>
                                                    <td style="text-align: right; font-size: 0.9em; font-style: italic">
                                                        <%=GetLabel("Umur")%>
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <%#: Eval("PatientAge")%>
                                                    </td>
                                                    <td style="text-align: right; font-size: 0.9em; font-style: italic">
                                                        <%=GetLabel("Sts Berkas")%>
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <%#: Eval("MedicalFileStatus")%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right; font-size: 0.9em; font-style: italic">
                                                        <%=GetLabel("NIK")%>
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <%#: Eval("SSN")%>
                                                    </td>
                                                    <td>
                                                        <%#: Eval("cfIdentity")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <%=GetLabel("INFORMASI KONTAK")%></HeaderTemplate>
                                    <ItemTemplate>
                                        <div style="padding: 3px">
                                            <div>
                                                <%#: Eval("HomeAddress")%></div>
                                            <img src='<%= ResolveUrl("~/Libs/Images/homephone.png")%>' alt='' style="float: left;" /><div
                                                style="margin-left: 30px">
                                                <%#: Eval("PhoneNo1")%>&nbsp;</div>
                                            <img src='<%= ResolveUrl("~/Libs/Images/mobilephone.png")%>' alt='' style="float: left;" /><div
                                                style="margin-left: 30px">
                                                <%#: Eval("MobilePhoneNo1")%>&nbsp;</div>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="BusinessPartner" HeaderText="PEMBAYAR" HeaderStyle-Width="150px"
                                    ItemStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Notes" HeaderText="KETERANGAN"
                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                <asp:HyperLinkField HeaderText="KELUARGA" Text="Keluarga Pasien" ItemStyle-HorizontalAlign="Center"
                                    ItemStyle-CssClass="lnkPatientFamily" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" />
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div style="vertical-align: middle;">
                                            <img class="imgIsDied" title="<%=GetLabel("Died") %>" src='<%# ResolveUrl("~/Libs/Images/Status/RIP.png")%>'
                                                alt="" width="30px" style='<%# Eval("IsAlive").ToString() == "False" ? "float:left; display:inline": "display:none" %>' />
                                            <img class="imgIsRetention" style='<%# Eval("cfIsRetention").ToString() == "True" ? "padding-left:2px;padding-right:2px;float:left; display:inline": "display:none" %>'
                                                title="<%=GetLabel("Retention") %>" src='<%# ResolveUrl("~/Libs/Images/Status/archieved.png")%>'
                                                alt="" width="30px" />
                                            <img class="imgIsArchieved" style='<%# Eval("cfIsArchieved").ToString() == "True" ? "padding-left:2px;padding-right:2px;float:left; display:inline": "display:none" %>'
                                                title="<%=GetLabel("Archieved") %>" src='<%# ResolveUrl("~/Libs/Images/Status/archieved.png")%>'
                                                alt="" width="30px" />
                                            <img class="lnkPatientDataHistory imgLink" title="<%=GetLabel("View") %>" src='<%# ResolveUrl("~/Libs/Images/Toolbar/registration_notes.png")%>'
                                                alt="" width="30px" style="padding-left: 2px; float: left; display: inline" />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                No Data To Display
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
