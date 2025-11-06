<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master" AutoEventWireup="true"
    CodeBehind="InformationStatusKontrakRekanan.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.InformationStatusKontrakRekanan" %>

<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dxpc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">

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
        function onCboCustomerTypeChanged() {
            $('#<%=hdnCustomerType.ClientID %>').val(cboCustomerType.GetValue());
            cbpView.PerformCallback('refresh');
        }

        $(function () {
            setDatePicker('<%=txtStartDate.ClientID %>');
            setDatePicker('<%=txtEndDate.ClientID %>');

            $('#<%=txtStartDate.ClientID %>').change(function () {
                cbpView.PerformCallback('refresh');
            });
            $('#<%=txtEndDate.ClientID %>').change(function () {
                cbpView.PerformCallback('refresh');
            });
        })

        $('#<%=chkIsExpired.ClientID %>').die();
        $('#<%=chkIsExpired.ClientID %>').live('change', function () {
            cbpView.PerformCallback('refresh');
        });

        $('.lblInformasiRekanan').live('click', function () {
            $tr = $(this).closest('tr');
            var businessPartnersID = $tr.find('.hdnBusinessPartnersID').val();
            var url = ResolveUrl("~/Program/Information/InformationStatusKontrakRekananCtl.ascx");
            openUserControlPopup(url, businessPartnersID, 'Detail Information', 1200, 550);
        });

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
                    $('#<%=lvwView.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=lvwView.ClientID %> tr:eq(1)').click();
        }
        //#endregion
        $('.lblLink.lnkContractSummary').live('click', function () {
            $tr = $(this).closest('tr');
            var id = $tr.find('.hdnBusinessPartnersID').val();
            var url = ResolveUrl("~/Libs/Program/Information/CustomerContractSummaryViewCtl.ascx");
            openUserControlPopup(url, id, 'Ringkasan Kontrak', 700, 600);
        });
        $('.lblLink.lnkCoverageType').live('click', function () {
            $tr = $(this).closest('tr');
            var id = $tr.find('.hdnBusinessPartnersID').val();
            var url = ResolveUrl("~/Program/Information/InformationSkemaPenjaminPerBussinesPartnetCtl.ascx");
            openUserControlPopup(url, id, 'Ringkasan Kontrak', 700, 600);
        });

        $('.lblLink.lnkContractDocument').live('click', function () {
            $tr = $(this).closest('tr');
            var id = $tr.find('.hdnBusinessPartnersID').val();

            var url = ResolveUrl("~/Program/Information/InformationContractDocumentPerBussinesPartnetCtl.ascx");
            openUserControlPopup(url, id, 'Dokumen Kontrak', 700, 600);
        });
        $('.lblLink.lnkDocumentNote').live('click', function () {
            $tr = $(this).closest('tr');
            var id =  $tr.find('.hdnBusinessPartnersID').val();
            var url = ResolveUrl("~/Program/Information/InformationDocumentNotesPerBussinesPartnerCtl.ascx");
            openUserControlPopup(url, id, 'Catatan Kontrak', 700, 600);
        });

       
    </script>
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" id="hdnCustomerType" runat="server" value="" />
    <input type="hidden" id="hdnContractDate" runat="server" value="" />
    <div style="position: relative;">
        <div>
            <table>
                <colgroup>
                        <col style="width: 150px" />
                        <col style="width: 900px" />
                        <col style="width: 150px" /> 
                        <col style="width: 200px" />
                        <col style="width: 400px" />
                </colgroup>
                <tr>
                    <td class="tdLabel">
                       <label class="lblNormal">
                           <%=GetLabel("Periode Kontrak")%></label>
                    </td>
                    <td>
                         <asp:TextBox ID="txtStartDate" Width="130px" runat="server" CssClass="datepicker" />
                         <%=GetLabel("s/d")%>
                         <asp:TextBox ID="txtEndDate" Width="130px" runat="server" CssClass="datepicker" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkIsExpired" runat="server" Checked="false" /><%:GetLabel(" Expired (Kontrak yang masa berlaku nya habis dalam periode kontrak tersebut)")%>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal">
                            <%=GetLabel("Tipe Instansi")%></label>
                    </td>
                    <td>
                        <dxe:ASPxComboBox ID="cboCustomerType" ClientInstanceName="cboCustomerType" Width="200px"
                            runat="server">
                            <ClientSideEvents ValueChanged="function(s,e) { onCboCustomerTypeChanged(); }" />
                        </dxe:ASPxComboBox>
                    </td>
                </tr>
            </table>
        </div>
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                        <asp:ListView runat="server" ID="lvwView">
                            <EmptyDataTemplate>
                                <table id="tblView" runat="server" class="grdView lvwView" cellspacing="0"
                                    rules="all">
                                    <tr>
                                        <th style="width: 150px" align="left">
                                            <%=GetLabel("Kode Rekanan")%>
                                        </th>
                                        <th style="width: 450px" align="left">
                                            <%=GetLabel("Nama Rekanan")%>
                                        </th>
                                        <th style="width: 130px" align="left">
                                            <%=GetLabel("No. Kontrak")%>
                                        </th>
                                        <th style="width: 220px" align="center">
                                            <%=GetLabel("Periode Kontrak")%>
                                        </th>
                                        <th style="width: 200px" align="center">
                                            <%=GetLabel("Informasi Rekanan")%>
                                        </th>
                                         <th style="width: 200px" align="center">
                                            <%=GetLabel("Ringkasan Kontrak")%>
                                        </th>
                                         <th style="width: 200px" align="center">
                                            <%=GetLabel("Skema Penjaminan")%>
                                        </th>
                                         <th style="width: 200px" align="center">
                                            <%=GetLabel("Dokumen Kontrak")%>
                                        </th>
                                         <th style="width: 200px" align="center">
                                            <%=GetLabel("Catatan Kontrak")%>
                                        </th>
                                        <th style="width: 150px" align="center">
                                            <%=GetLabel("Status")%>
                                        </th>
                                    </tr>
                                    <tr class="trEmpty">
                                        <td colspan="10">
                                            <%=GetLabel("No Data To Display")%>
                                        </td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                            <LayoutTemplate>
                                <table id="tblViewOrder" runat="server" class="grdView lvwView" cellspacing="0"
                                    rules="all">
                                    <tr>
                                        <th style="width: 150px" align="left">
                                            <%=GetLabel("Kode Rekanan")%>
                                        </th>
                                        <th style="width: 450px" align="left">
                                            <%=GetLabel("Nama Rekanan")%>
                                        </th>
                                        <th style="width: 130px" align="left">
                                            <%=GetLabel("No. Kontrak")%>
                                        </th>
                                        <th style="width: 220px" align="center">
                                            <%=GetLabel("Periode Kontrak")%>
                                        </th>
                                        <th style="width: 200px" align="center">
                                            <%=GetLabel("Informasi Rekanan")%>
                                        </th>
                                         <th style="width: 200px" align="center">
                                            <%=GetLabel("Ringkasan Kontrak")%>
                                        </th>
                                         <th style="width: 200px" align="center">
                                            <%=GetLabel("Skema Penjaminan")%>
                                        </th>
                                         <th style="width: 200px" align="center">
                                            <%=GetLabel("Dokumen Kontrak")%>
                                        </th>
                                         <th style="width: 200px" align="center">
                                            <%=GetLabel("Catatan Kontrak")%>
                                        </th>
                                        <th style="width: 150px" align="center">
                                            <%=GetLabel("Status")%>
                                        </th>
                                    </tr>
                                    <tr runat="server" id="itemPlaceholder">
                                    </tr>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <div>
                                            <%#: Eval("BusinessPartnersCode") %></div>
                                    </td>
                                    <td>
                                        <div>
                                            <%#: Eval("BusinessPartnersName") %></div>
                                    </td>
                                    <td>
                                        <div>
                                            <%#: Eval("BusinessPartnersContractNo") %></div>
                                    </td>
                                    <td align="center">
                                        <div>
                                            <%#: Eval("BusinessPartnersContractPeriode") %></div>
                                    </td>
                                    <td align="center">
                                        <input type="hidden" class="hdnBusinessPartnersID" value="<%#: Eval("BusinessPartnersID")%>" />
                                        <label class="lblLink lblInformasiRekanan">Detail Information</label>
                                    </td>
                                     <td style="width: 200px" align="center">
                                            <label class="lblLink lnkContractSummary">Ringkasan Kontrak</label>
                                        </td>
                                         <td style="width: 200px" align="center">
                                               <label class="lblLink lnkCoverageType">Skema Penjaminan</label>
                                        </td>
                                         <td style="width: 200px" align="center">
                                           <label class="lblLink lnkContractDocument"><%=GetLabel("Dokumen Kontrak")%></label> 
                                        </td>
                                         <td style="width: 200px" align="center">
                                             <label class="lblLink lnkDocumentNote"><%=GetLabel("Catatan Kontrak")%></label> 
                                        </td>
                                    <td align="center">
                                        <img class="imgStatus" title='<%#: Eval("cfIsActiveInString") %>' style="width:20px; height:20px" src='<%# Eval("IsActive").ToString() == "False" ? ResolveUrl("~/Libs/Images/Status/cancel.png") : ResolveUrl("~/Libs/Images/Status/done.png")%>'
                                            alt="" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>
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