<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master" AutoEventWireup="true" 
    CodeBehind="RoomList.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.RoomList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            var grd = new customGridView();
            grd.init('<%=grdView.ClientID %>', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');
        });

        function onCboHealthcareValueChanged(s) {
            cbpView.PerformCallback('refresh');
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

        $('.lnkBed a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/Master/Room/RoomBedEntryCtl.ascx");
            openUserControlPopup(url, id, 'Kamar', 900, 500);
        });

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();
                else
                    $('#<%=hdnID.ClientID %>').val('');

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else {
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
            }
        }
        //#endregion

        function onAfterDeleteClickSuccess() {
            var isBridgingAplicares = $('#<%=hdnIsBridgingToAplicares.ClientID %>').val();
            if (isBridgingAplicares == "1") {
                UpdateRoomAplicares();
            }
        }

        //#region APLICARES

        function UpdateRoomAplicares() {
            var roomID = $('#<%=hdnID.ClientID %>').val();
            var filterSUR = "RoomID = " + roomID + " AND IsDeleted = 0 AND IsAplicares = 1";
            Methods.getListObject('GetServiceUnitRoomList', filterSUR, function (resultSUR) {
                for (i = 0; i < resultSUR.length; i++) {
                    var hsuID = resultSUR[i].HealthcareServiceUnitID;
                    var classID = resultSUR[i].ClassID;
                    var filterExpression = "HealthcareServiceUnitID = " + hsuID + " AND ClassID = " + classID;
                    Methods.getObject('GetvServiceUnitAplicaresList', filterExpression, function (result) {
                        if (result != null) {
                            var kodeKelas = result.AplicaresClassCode;
                            var kodeRuang = result.ServiceUnitCode;
                            var namaRuang = result.ServiceUnitName;
                            var jumlahKapasitas = result.CountBedAll;
                            var jumlahKosong = result.CountBedEmpty;
                            var jumlahKosongPria = 0;
                            var jumlahKosongWanita = 0;
                            var jumlahKosongPriaWanita = result.CountBedEmpty;

                            if (result.CountIsSendToAplicares == 0) {
                                AplicaresService.createRoom(kodeKelas, kodeRuang, namaRuang, jumlahKapasitas, jumlahKosong, jumlahKosongPria, jumlahKosongWanita, jumlahKosongPriaWanita, function (resultRoom) {
                                    if (resultRoom != null) {
                                        try {
                                            AplicaresService.updateStatusSendToAplicares(result.HealthcareServiceUnitID, result.ClassID, function (resultUpdate) {
                                                if (resultUpdate != null) {
                                                    try {
                                                        var resultUpdate = resultUpdate.split('|');
                                                        if (resultUpdate[0] == "1") {
                                                            showToast('INFORMATION', "SUCCESS");

                                                            cbpView.PerformCallback('refresh');
                                                        }
                                                        else {
                                                            showToast('FAILED', resultUpdate[2]);
                                                        }
                                                    } catch (error) {
                                                        showToast('FAILED', error);
                                                    }
                                                }
                                            });
                                        } catch (err) {
                                            showToast('FAILED', err);
                                        }
                                    }
                                });
                            } else {
                                AplicaresService.updateRoomStatus(kodeKelas, kodeRuang, namaRuang, jumlahKapasitas, jumlahKosong, jumlahKosongPria, jumlahKosongWanita, jumlahKosongPriaWanita, function (resultRoom) {
                                    if (resultRoom != null) {
                                        try {
                                            AplicaresService.updateStatusSendToAplicares(result.HealthcareServiceUnitID, result.ClassID, function (resultUpdate) {
                                                if (resultUpdate != null) {
                                                    try {
                                                        var resultUpdate = resultUpdate.split('|');
                                                        if (resultUpdate[0] == "1") {
                                                            var oDataUpdate = jQuery.parseJSON(resultUpdate[1]);
                                                            showToast('INFORMATION', "SUCCESS");

                                                            cbpView.PerformCallback('refresh');
                                                        }
                                                        else {
                                                            showToast('FAILED', resultUpdate[2]);
                                                        }
                                                    } catch (error) {
                                                        showToast('FAILED', error);
                                                    }
                                                }
                                            });
                                        } catch (err) {
                                            showToast('FAILED', err);
                                        }
                                    }
                                });
                            }
                        }
                    });
                }
            });
        }

        //#endregion
    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" id="hdnIsBridgingToAplicares" value="" runat="server" />
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td style="width:100px"><%=GetLabel("Rumah Sakit") %></td>
            <td>
                <dxe:ASPxComboBox ID="cboHealthcare" runat="server" ClientInstanceName="cboHealthcare" Width="450px">
                    <ClientSideEvents ValueChanged="function(s,e){ onCboHealthcareValueChanged(s); }" />
                </dxe:ASPxComboBox>
            </td>
        </tr>
    </table>
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="RoomID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:BoundField DataField="RoomCode" HeaderText="Kode Kamar" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="RoomName" HeaderText="Nama Kamar" HeaderStyle-HorizontalAlign="Left" />
                                <asp:CheckBoxField DataField="IsWardRoom" HeaderText="Rawat Inap" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkBed" HeaderText="Tempat Tidur" HeaderStyle-Width="100px">
                                    <ItemTemplate>
                                        <a <%#: Eval("IsWardRoom").ToString() == "False" ? "style='display:none'" : ""%>>Bed</a>
                                    </ItemTemplate>
                                </asp:TemplateField>
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
</asp:Content>