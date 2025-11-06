<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master" AutoEventWireup="true" 
    CodeBehind="HCPList.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.HCPList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            var grd = new customGridView();
            grd.init('<%=grdView.ClientID %>', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');
        });

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
        $(function () {
            Methods.checkImageError('imgPhysicianImage', 'paramedic', 'null');
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            Methods.checkImageError('imgPhysicianImage', 'paramedic', 'null');
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

        $('.lnkParamedicItemCharges a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            openMatrixControl('PhysicianItemCharges', id, 'Physician Item (Pelayanan)');
        });

        $('.lnkExclusionParamedicItem a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            openMatrixControl('ExclusionPhysicianItem', id, 'Restriksi Item (Pembatasan Item Pelayanan Dokter)');
        });

        $('.lnkParamedicItemDrugs a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            openMatrixControl('PhysicianItemDrugs', id, 'Physician Item (Obat & Alkes)');
        });

        $('.lnkParamedicItemLogistics a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            openMatrixControl('PhysicianItemLogistics', id, 'Physician Item (Barang Umum)');
        });

        $('.lnkParamedicTeam a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/Master/HCP/HCPTeamEntryCtl.ascx");
            openUserControlPopup(url, id, 'Tim Dokter / Medis', 1200, 500);
        });

        $('.lnkParamedicGuaranteePayment a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/Master/HCP/ParamedicGuaranteePaymentCtl.ascx");
            openUserControlPopup(url, id, 'Jasa Minimal', 1100, 500);
        });

        $('.lnkParamedicFixedPayment a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/Master/HCP/ParamedicFixedPaymentCtl.ascx");
            openUserControlPopup(url, id, 'Jasa Tetap', 1100, 500);
        });

        $('.lnkParamedicHospitalFee a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/Master/HCP/ParamedicHospitalFeeCtl.ascx");
            openUserControlPopup(url, id, 'Jasa RS', 1100, 500);
        });

        $('#<%=grdView.ClientID %> tr:gt(0) td.tdExpand').live('click', function () {
            $tr = $(this).parent();
            $trDetail = $(this).parent().next();
            if ($trDetail.attr('class') != 'trDetail') {
                $trCollapse = $('.trDetail');

                $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/down-arrow.png")%>');
                $newTr = $("<tr><td></td><td colspan='10'></td></tr>").attr('class', 'trDetail');
                $newTr.insertAfter($tr);
                $newTr.find('td').last().append($('#containerGrdDetail'));

                if ($trCollapse != null) {
                    $trCollapse.prev().find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');
                    $trCollapse.remove();
                }

                $('#<%=grdDetail.ClientID %> tr:gt(0)').remove();
                $('#<%=hdnExpandID.ClientID %>').val($tr.find('.keyField').html());
                cbpViewDetail.PerformCallback();
            }
            else {
                $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');
                $('#tempContainerGrdDetail').append($('#containerGrdDetail'));

                $trDetail.remove();
            }
        });

        $('.lnkVisitType a').live('click', function () {
            var paramedicID = $('#<%=hdnExpandID.ClientID %>').val();
            var healthcareServiceUnitID = $(this).closest('tr').find('.keyField').html();
            var id = paramedicID + '|' + healthcareServiceUnitID;
            var url = ResolveUrl("~/Libs/Program/Master/ParamedicSchedule/PSVisitTypeEntryCtl.ascx");
            openUserControlPopup(url, id, 'Jenis Kunjungan', 900, 500);
        });

        $('.lnkSchedule a').live('click', function () {
            var paramedicID = $('#<%=hdnExpandID.ClientID %>').val();
            var healthcareServiceUnitID = $(this).closest('tr').find('.keyField').html();
            var id = paramedicID + '|' + healthcareServiceUnitID;
            var url = ResolveUrl("~/Libs/Program/Master/ParamedicSchedule/PSScheduleDayEntryCtl.ascx");
            openUserControlPopup(url, id, 'Jadwal Dokter', 800, 500);
        });

        $('.lnkSchedulePerDate a').live('click', function () {
            var paramedicID = $('#<%=hdnExpandID.ClientID %>').val();
            var healthcareServiceUnitID = $(this).closest('tr').find('.keyField').html();
            var id = paramedicID + '|' + healthcareServiceUnitID;
            var url = ResolveUrl("~/Libs/Program/Master/ParamedicSchedule/PSScheduleWithDateEntryCtl.ascx");
            openUserControlPopup(url, id, 'Jadwal Dokter Per Tanggal', 900, 500);
        });

        $('.lnkLeaveSchedule a').live('click', function () {
            var paramedicID = $('#<%=hdnExpandID.ClientID %>').val();
            var healthcareServiceUnitID = $(this).closest('tr').find('.keyField').html();
            var id = paramedicID + '|' + healthcareServiceUnitID;
            var url = ResolveUrl("~/Libs/Program/Master/ParamedicSchedule/PSleaveScheduleEntryCtl.ascx");
            openUserControlPopup(url, id, 'Jadwal Cuti', 900, 500);
        });
    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" value="" id="hdnExpandID" runat="server" />
    <input type="hidden" value="" id="hdnIsBridgingToMedinfrasMobileApps" runat="server" />
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ $('#tempContainerGrdDetail').append($('#containerGrdDetail')); showLoadingPanel(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="ParamedicID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:TemplateField ItemStyle-Width="20px" ItemStyle-CssClass="tdExpand" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Panel ID="Panel2" runat="server" Visible='<%# Eval("Healthcare") != DBNull.Value ? Eval("Healthcare").ToString() != "" : false %>'>
                                            <img class="imgExpand imgLink" src='<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>' alt='' />
                                        </asp:Panel>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <%=GetLabel("INFORMASI DOKTER / TENAGA MEDIS")%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div style="padding:3px">
                                            <input type="hidden" value="<%#: Eval("Healthcare")%>" class="hdnHealthcare" />
                                            <img class="imgPhysicianImage" src='<%# Eval("PhysicianImageUrl")%>' alt="" height="55px" style="float:left;margin-right: 10px;" />
                                            <div style="font-weight:bold"><%#: Eval("ParamedicName")%></div>
                                            <table cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col style="width:150px"/>
                                                    <col style="width:6px"/>
                                                    <col style="width:100px"/>
                                                    <col style="width:150px"/>
                                                    <col style="width:6px"/>
                                                    <col style="width:220px"/>
                                                </colgroup>
                                                <tr>
                                                    <td align="right">
                                                        <div class="inline-grid-label"><%=GetLabel("Kode Dokter / Paramedis")%></div>
                                                        <div class="inline-grid-label"><%=GetLabel("Inisial")%></div>
                                                    </td>
                                                    <td>&nbsp;</td>
                                                    <td>
                                                        <div><%#: Eval("ParamedicCode")%></div>
                                                        <div><%#: Eval("Initial")%></div>
                                                    </td>
                                                    <td align="right">
                                                        <div class="inline-grid-label"><%=GetLabel("Tipe Dokter / Paramedis")%></div>
                                                        <div class="inline-grid-label"><%=GetLabel("Spesialisasi")%></div>
                                                    </td>
                                                    <td>&nbsp;</td>
                                                    <td>
                                                        <div><%#: Eval("ParamedicMasterType")%></div>
                                                        <div class="specialText"><%#: Eval("SpecialtyName")%></div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>                                        
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:CheckBoxField DataField="IsAvailable" HeaderText="Available" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" />
                                <asp:HyperLinkField HeaderText="Item Pelayanan" Text="Item Pelayanan" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkParamedicItemCharges" HeaderStyle-Width="120px" />
                                <asp:HyperLinkField HeaderText="Restriksi Item" Text="Restriksi Item" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkExclusionParamedicItem" HeaderStyle-Width="120px"  />
                                <%--<asp:HyperLinkField HeaderText="Item Obat & Alkes" Text="Item Obat & Alkes" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkParamedicItemDrugs" HeaderStyle-Width="120px" />--%>
                                <%--<asp:HyperLinkField HeaderText="Item Barang Umum" Text="Item Barang Umum" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkParamedicItemLogistics" HeaderStyle-Width="120px" />--%>
                                <asp:HyperLinkField HeaderText="Tim Dokter / Medis" Text="Tim" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkParamedicTeam" HeaderStyle-Width="120px" />
                                <asp:HyperLinkField HeaderText="Jasa Minimal" Text="Jasa Minimal" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkParamedicGuaranteePayment" HeaderStyle-Width="100px" />
                                <asp:HyperLinkField HeaderText="Jasa Tetap" Text="Jasa Tetap" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkParamedicFixedPayment" HeaderStyle-Width="100px" />
                                <asp:HyperLinkField HeaderText="Jasa RS" Text="Jasa RS" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkParamedicHospitalFee" HeaderStyle-Width="100px" />
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("Data Tidak Tersedia")%>
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

    <div id="tempContainerGrdDetail" style="display:none">
        <div id="containerGrdDetail" class="borderBox" style="width: 100%;padding: 10px 5px;">
            <div style="position: relative;">
                <dxcp:ASPxCallbackPanel ID="cbpViewDetail" runat="server" Width="100%" ClientInstanceName="cbpViewDetail"
                    ShowLoadingPanel="false" OnCallback="cbpViewDetail_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDetail').show(); }"
                        EndCallback="function(s,e){ $('#containerImgLoadingViewDetail').hide(); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server">
                            <asp:Panel runat="server" ID="Panel1" Style="width: 100%; margin-left: auto; margin-right: auto">
                                <asp:GridView ID="grdDetail" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="HealthcareServiceUnitID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:BoundField DataField="HealthcareName" HeaderText="Rumah Sakit" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="250px" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="ServiceUnitName" HeaderText="Unit Pelayanan" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:HyperLinkField HeaderText="Jenis Kunjungan" Text="Setting" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkVisitType" HeaderStyle-Width="120px" />
                                        <asp:HyperLinkField HeaderText="Jadwal" Text="Setting" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkSchedule" HeaderStyle-Width="120px" />
                                        <asp:HyperLinkField HeaderText="Jadwal Per Tanggal" Text="Setting" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkSchedulePerDate" HeaderStyle-Width="120px" />
                                        <asp:HyperLinkField HeaderText="Jadwal Cuti" Text="Setting" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkLeaveSchedule" HeaderStyle-Width="120px" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Data Tidak Tersedia")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>    
                <div class="imgLoadingGrdView" id="containerImgLoadingViewDetail">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
            </div>
        </div>
    </div>
</asp:Content>