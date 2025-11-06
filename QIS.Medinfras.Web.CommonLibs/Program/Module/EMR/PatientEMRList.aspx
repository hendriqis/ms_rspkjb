<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPList.master" AutoEventWireup="true" 
    CodeBehind="PatientEMRList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientEMRList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnMPListView" runat="server"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbnotes.png")%>' alt="" /><div><%=GetLabel("View")%></div></li>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomContextMenu" runat="server">
    <li class="list-devider"><hr /></li>
    <li id="ctxMenuView" runat="server"><a href="#"><%=GetLabel("View")%></a> </li>
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

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion
    </script>
    <div style="display:none"><asp:Button ID="btnOpenTransactionDt" runat="server" UseSubmitBehavior="false" OnClientClick="return onBeforeOpenTransactionDt();" OnClick="btnOpenTransactionDt_Click" /></div>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="overflow-y: scroll;height:465px">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="MRN" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:TemplateField HeaderStyle-Width="550px" HeaderStyle-HorizontalAlign="Left">
                                    <HeaderTemplate><%=GetLabel("Informasi Pasien")%></HeaderTemplate>
                                    <ItemTemplate>
                                        <div style="padding:3px">
                                            <img class="imgPatientImage" src='<%#Eval("PatientImageUrl") %>' alt="" height="55px" width="40px" style="float:left;margin-right: 10px;" />
                                            <div><%#: Eval("PatientName") %></div>
                                            <input type="hidden" value='<%#: Eval("GCSex")%>' class="hdnPatientGender" />
                                            <table cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col style="width:170px"/>
                                                    <col style="width:10px"/>
                                                    <col style="width:120px"/>
                                                    <col style="width:50px"/>
                                                    <col style="width:10px"/>
                                                    <col style="width:120px"/>
                                                    <col style="width:100px"/>
                                                    <col style="width:10px"/>
                                                    <col style="width:120px"/>
                                                </colgroup>
                                                <tr>
                                                    <td style="text-align:right;font-size:0.9em;font-style:italic"><%=GetLabel("Nama Panggilan")%></td>
                                                    <td>&nbsp;</td>
                                                    <td><%#: Eval("PreferredName")%></td>
                                                    <td style="text-align:right;font-size:0.9em;font-style:italic"><%=GetLabel("MRN")%></td>
                                                    <td>&nbsp;</td>
                                                    <td><%#: Eval("MedicalNo")%></td>
                                                    <td style="text-align:right;font-size:0.9em;font-style:italic"><%=GetLabel("RM Lama")%></td>
                                                    <td>&nbsp;</td>
                                                    <td><%#: Eval("OldMedicalNo")%></td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align:right;font-size:0.9em;font-style:italic"><%=GetLabel("Tanggal Lahir")%></td>
                                                    <td>&nbsp;</td>
                                                    <td><%#: Eval("DateOfBirthInString")%></td>
                                                    <td style="text-align:right;font-size:0.9em;font-style:italic"><%=GetLabel("Umur")%></td>
                                                    <td>&nbsp;</td>
                                                    <td><%#: Eval("PatientAge")%></td>
                                                    <td style="text-align:right;font-size:0.9em;font-style:italic"><%=GetLabel("Sts Berkas")%></td>
                                                    <td>&nbsp;</td>
                                                    <td><%#: Eval("MedicalFileStatus")%></td>
                                                </tr>
                                            </table>                                                                                    
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                    <HeaderTemplate><%=GetLabel("Informasi Kontak")%></HeaderTemplate>
                                    <ItemTemplate>
                                        <div style="padding:3px">
                                            <div><%#: Eval("HomeAddress")%></div>
                                            <img src='<%= ResolveUrl("~/Libs/Images/homephone.png")%>' alt='' style="float:left;" /><div style="margin-left:30px"><%#: Eval("PhoneNo1")%>&nbsp;</div>
                                            <img src='<%= ResolveUrl("~/Libs/Images/mobilephone.png")%>' alt='' style="float:left;" /><div style="margin-left:30px"><%#: Eval("MobilePhoneNo1")%>&nbsp;</div>                                                  
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="BusinessPartner" HeaderText="Pembayar" HeaderStyle-Width="150px" ItemStyle-VerticalAlign="Top" />
                            </Columns>
                            <EmptyDataTemplate>
                                No Data To Display
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