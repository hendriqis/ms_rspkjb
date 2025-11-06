<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServiceUnitRoomEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.SystemSetup.Program.ServiceUnitRoomEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_serviceunitroomentryctl">
    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=hdnID.ClientID %>').val('');
        $('#<%=hdnRoomID.ClientID %>').val('');
        $('#<%=txtRoomCode.ClientID %>').val('');
        $('#<%=txtRoomName.ClientID %>').val('');
        $('#<%=chkISBORCalculation.ClientID %>').prop('checked', false);
        $('#<%=txtDiscount.ClientID %>').val('0');
        cboClassID.SetSelectedIndex(0);
        $('#<%=chkIsAplicares.ClientID %>').prop('checked', false);
        $('#<%=txtAplicaresClassCode.ClientID %>').val('');
        $('#<%=txtAplicaresClassName.ClientID %>').val('');
        
        $('#containerPopupEntryData').show();
    });

    $('#btnEntryPopupCancel').live('click', function () {
        $('#containerPopupEntryData').hide();
    });

    $('#btnEntryPopupSave').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup')) {
            cbpEntryPopupView.PerformCallback('save');
        }
    });

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function (evt) {
        evt.stopPropagation();
        evt.preventDefault();
        if (confirm("Are You Sure Want To Delete This Data?")) {
            $row = $(this).closest('tr');
            var id = $row.find('.hdnID').val();
            $('#<%=hdnID.ClientID %>').val(id);

            cbpEntryPopupView.PerformCallback('delete');
        }
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var ID = $row.find('.hdnID').val();
        var roomID = $row.find('.hdnRoomID').val();
        var roomCode = $row.find('.hdnRoomCode').val();
        var classID = $row.find('.hdnClassID').val();
        var chargeClassID = $row.find('.hdnChargeClassID').val();
        var discount = $row.find('.hdnDiscount').val();

        var roomName = $row.find('.tdRoomName').html();
        var isBORCalculation = $row.find('.tdISBORCalculation').find('input').is(':checked');
        var isAplicares = $row.find('.tdIsAplicares').find('input').is(':checked');
        var aplicaresClassCode = $row.find('.hdnAplicaresClassCode').val();
        var aplicaresClassName = $row.find('.hdnAplicaresClassName').val();

        $('#<%=hdnID.ClientID %>').val(ID);
        $('#<%=hdnSelectedClassID.ClientID %>').val(classID);
        $('#<%=hdnRoomID.ClientID %>').val(roomID);
        $('#<%=txtRoomCode.ClientID %>').val(roomCode);
        cboClassID.SetValue(classID);
        cboChargeClassID.SetValue(chargeClassID);
        $('#<%=txtDiscount.ClientID %>').val(discount);
        $('#<%=txtRoomName.ClientID %>').val(roomName);
        $('#<%=chkISBORCalculation.ClientID %>').prop('checked', isBORCalculation);
        $('#<%=chkIsAplicares.ClientID %>').prop('checked', isAplicares);
        $('#<%=txtAplicaresClassCode.ClientID %>').val(aplicaresClassCode);
        $('#<%=txtAplicaresClassName.ClientID %>').val(aplicaresClassName);

        $('#containerPopupEntryData').show();
    });

    //#region Room
    function onGetServiceUnitRoomFilterExpression() {
        var filterExpression = 'RoomID NOT IN (SELECT RoomID FROM ServiceUnitRoom WHERE IsDeleted = 0 AND HealthcareServiceUnitID = ' + $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val() + ') AND IsDeleted = 0';
        if ($('#<%:hdnDepartmentID.ClientID %>').val() != "INPATIENT") {
            filterExpression += " AND IsWardRoom = 0";
        }
        else {
            filterExpression += " AND IsWardRoom = 1";
        }
        return filterExpression;
    }

    $('#lblRoom.lblLink').live('click', function () {
        openSearchDialog('room', onGetServiceUnitRoomFilterExpression(), function (value) {
            $('#<%=txtRoomCode.ClientID %>').val(value);
            onTxtServiceUnitRoomCodeChanged(value);
        });
    });

    $('#<%=txtRoomCode.ClientID %>').live('change', function () {
        onTxtServiceUnitRoomCodeChanged($(this).val());
    });

    function onTxtServiceUnitRoomCodeChanged(value) {
        var filterExpression = onGetServiceUnitRoomFilterExpression() + " AND RoomCode = '" + value + "'";
        Methods.getObject('GetRoomList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnRoomID.ClientID %>').val(result.RoomID);
                $('#<%=txtRoomName.ClientID %>').val(result.RoomName);
            }
            else {
                $('#<%=hdnRoomID.ClientID %>').val('');
                $('#<%=txtRoomCode.ClientID %>').val('');
                $('#<%=txtRoomName.ClientID %>').val('');
            }
        });
    }
    //#endregion 

    //#region APLICARES

    function UpdateRoomAplicares() {
        var hsuID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
        var classID = $('#<%=hdnSelectedClassID.ClientID %>').val();
        var filterExpression = "HealthcareServiceUnitID = " + hsuID + " AND ClassID = " + classID;
        Methods.getObject('GetvServiceUnitAplicaresList', filterExpression, function (result) {
            if (result != null) {
                var kodeKelas = result.AplicaresClassCode;
                var kodeRuang = result.ServiceUnitCode;
                var namaRuang = result.ServiceUnitName;
                var jumlahKapasitas = result.CountBedAll;
                var jumlahKosong = result.CountBedEmpty;
                var jumlahKosongPria = result.CountBedEmptyMale;
                var jumlahKosongWanita = result.CountBedEmptyFemale;
                var jumlahKosongPriaWanita = result.CountBedEmptyMale + result.CountBedEmptyFemale;

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

                                                cbpEntryPopupView.PerformCallback('refresh');
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
                                                showToast('INFORMATION', "SUCCESS");

                                                cbpEntryPopupView.PerformCallback('refresh');
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

    //#endregion

    function setPagingDetailItem(pageCount) {
        if (pageCount > 0)
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        setPaging($("#pagingPopup"), pageCount, function (page) {
            cbpEntryPopupView.PerformCallback('changepage|' + page);
        }, 8);
    }

    function onCbpEntryPopupViewEndCallback(s) {
        var isBridgingAplicares = $('#<%=hdnIsBridgingToAplicares.ClientID %>').val();
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else {
                var pageCount = parseInt(param[2]);
                setPagingDetailItem(pageCount);
                $('#containerPopupEntryData').hide();

                if (isBridgingAplicares == "1") {
                    UpdateRoomAplicares();
                }
            }
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
            else {
                var pageCount = parseInt(param[2]);
                setPagingDetailItem(pageCount);
            }
        }
        else if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPagingDetailItem(pageCount);
        }
        else {
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        hideLoadingPanel();
    }

    //#region Paging
    var pageCountAvailable = parseInt('<%=PageCount %>');
    $(function () {
        setPagingDetailItem(pageCountAvailable);
    });
    //#endregion   
</script>
<div style="height: 450px; overflow-y: auto">
    <input type="hidden" id="hdnHealthcareServiceUnitID" value="" runat="server" />
    <input type="hidden" id="hdnDepartmentID" value="" runat="server" />
    <input type="hidden" id="hdnSelectedClassID" value="" runat="server" />
    <input type="hidden" id="hdnIsBridgingToAplicares" value="" runat="server" />
    <div class="pageTitle">
        <%=GetLabel("Kamar Unit Pelayanan")%></div>
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 70%">
                    <colgroup>
                        <col style="width: 160px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Unit Pelayanan")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtServiceUnitName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Rumah Sakit")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtHealthcareName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
                <div id="containerPopupEntryData" style="margin-top: 10px; display: none;">
                    <input type="hidden" id="hdnID" runat="server" value="" />
                    <div class="pageTitle">
                        <%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 200px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory lblLink" id="lblRoom">
                                        <%=GetLabel("Kamar")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnRoomID" runat="server" value="" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 100px" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtRoomCode" CssClass="required" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtRoomName" ReadOnly="true" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Perhitungan BOR")%></label>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkISBORCalculation" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Kelas")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboClassID" ClientInstanceName="cboClassID" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Kelas Tagihan")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboChargeClassID" ClientInstanceName="cboChargeClassID" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Diskon Tempat Tidur Sementara")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDiscount" Width="50px" CssClass="number" runat="server" />
                                    %
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Bridging Aplicares")%></label>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkIsAplicares" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Kode Aplicares")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAplicaresClassCode" Width="100px" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Nama Aplicares")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAplicaresClassName" Width="100px" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("Simpan")%>' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnEntryPopupCancel" value='<%= GetLabel("Batal")%>' />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <div style="position: relative; height: 300px; overflow-y: auto; overflow-x: hidden;">
                    <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                        ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                        OnRowDataBound="grdView_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                        alt="" style="float: left; margin-left: 7px" />
                                                    <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                        alt="" />
                                                    <input type="hidden" class="hdnID" value="<%#: Eval("ID")%>" />
                                                    <input type="hidden" class="hdnRoomID" value="<%#: Eval("RoomID")%>" />
                                                    <input type="hidden" class="hdnRoomCode" value="<%#: Eval("RoomCode")%>" />
                                                    <input type="hidden" class="hdnClassID" value="<%#: Eval("ClassID")%>" />
                                                    <input type="hidden" class="hdnChargeClassID" value="<%#: Eval("ChargeClassID")%>" />
                                                    <input type="hidden" class="hdnDiscount" value="<%#: Eval("TemporaryBedDiscount")%>" />
                                                    <input type="hidden" class="hdnAplicaresClassCode" value="<%#: Eval("AplicaresClassCode")%>" />
                                                    <input type="hidden" class="hdnAplicaresClassName" value="<%#: Eval("AplicaresClassName")%>" />
                                                </ItemTemplate>

<HeaderStyle Width="70px"></HeaderStyle>

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="RoomCode" ItemStyle-CssClass="tdRoomName" HeaderText="Kode Kamar"
                                                HeaderStyle-Width="70px" >
<HeaderStyle Width="70px"></HeaderStyle>

<ItemStyle CssClass="tdRoomName"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="RoomName" ItemStyle-CssClass="tdRoomName" 
                                                HeaderText="Nama Kamar" >
<ItemStyle CssClass="tdRoomName"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ClassName" HeaderText="Kelas" 
                                                HeaderStyle-Width="100px" >
<HeaderStyle Width="100px"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ChargeClassName" HeaderText="Kelas Tagihan" 
                                                HeaderStyle-Width="80px" >
<HeaderStyle Width="80px"></HeaderStyle>
                                            </asp:BoundField>
                                            <asp:CheckBoxField DataField="ISBORCalculation" ItemStyle-CssClass="tdISBORCalculation"
                                                HeaderText="Perhitungan BOR" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" >
<HeaderStyle HorizontalAlign="Center" Width="80px"></HeaderStyle>

<ItemStyle HorizontalAlign="Center" CssClass="tdISBORCalculation"></ItemStyle>
                                            </asp:CheckBoxField>
                                            <asp:CheckBoxField DataField="IsAplicares" ItemStyle-CssClass="tdIsAplicares" HeaderText="Bridging Aplicares"
                                                HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" 
                                                ItemStyle-HorizontalAlign="Center" >
<HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>

<ItemStyle HorizontalAlign="Center" CssClass="tdIsAplicares"></ItemStyle>
                                            </asp:CheckBoxField>
                                            <asp:CheckBoxField DataField="IsSendToAplicares" ItemStyle-CssClass="tdIsSendToAplicares"
                                                HeaderText="Terkirim ke Aplicares" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" >
<HeaderStyle HorizontalAlign="Center" Width="60px"></HeaderStyle>

<ItemStyle HorizontalAlign="Center" CssClass="tdIsSendToAplicares"></ItemStyle>
                                            </asp:CheckBoxField>
                                        </Columns>

<EmptyDataRowStyle CssClass="trEmpty"></EmptyDataRowStyle>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Data Tidak Tersedia")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="pagingPopup">
                            </div>
                        </div>
                    </div>
                </div>
                <div style="width: 100%; text-align: center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblEntryPopupAddData">
                        <%= GetLabel("Tambah Data")%></span>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
