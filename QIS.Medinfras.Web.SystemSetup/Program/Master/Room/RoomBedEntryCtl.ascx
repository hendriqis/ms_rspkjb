<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RoomBedEntryCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.SystemSetup.Program.RoomBedEntryCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    $('#lblEntryPopupAddData').live('click', function () {
        var filterExpression = "RoomID = " + $('#<%=hdnRoomID.ClientID %>').val();
        Methods.getObject('GetvServiceUnitRoomList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=chkISBORCalculation.ClientID %>').prop('checked', result.ISBORCalculation);
            }
        });
        $('#<%=hdnID.ClientID %>').val('');
        $('#<%=txtBedCode.ClientID %>').val('');
        $('#<%=txtExtensionNo.ClientID %>').val('');
        $('#<%=chkIsNeedConfirmation.ClientID %>').prop('checked', true);
        $('#<%=chkIsTemporary.ClientID %>').prop('checked', false);
        $('#<%=chkIsPatientAccompany.ClientID %>').prop('checked', false);
        $('#<%=chkIsNewBornBed.ClientID %>').prop('checked', false);
        $('#<%=chkIsPandemicBed.ClientID %>').prop('checked', false);
        $('#<%=chkIsWithVentilator.ClientID %>').prop('checked', false);
        $('#<%=chkIsWithPressure.ClientID %>').prop('checked', false);

        $('#containerPopupEntryData').show();
    });

    $('#btnEntryPopupCancel').live('click', function () {
        $('#containerPopupEntryData').hide();
    });

    $('#btnEntryPopupSave').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
            cbpEntryPopupView.PerformCallback('save');
        return false;
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
        var GCBedStatus = $row.find('.hdnGCBedStatus').val();

        var bedStatus = $row.find('.tdBedStatus').val();
        var extensionNo = $row.find('.tdExtensionNo').html();
        var bedCode = $row.find('.tdBedCode').html();
        var isNeedConfirmation = $row.find('.tdIsNeedConfirmation').find('input').is(':checked');
        var isTemporary = $row.find('.tdIsTemporary').find('input').is(':checked');
        var isBORCalculation = $row.find('.tdISBORCalculation').find('input').is(':checked');
        var isPatientAccompany = $row.find('.tdIsPatientAccompany').find('input').is(':checked');
        var isNewBornBed = $row.find('.tdIsNewBornBed').find('input').is(':checked');
        var isPandemicBed = $row.find('.tdIsPandemicBed').find('input').is(':checked');
        var isWithVentilator = $row.find('.tdIsWithVentilator').find('input').is(':checked');
        var isWithPressure = $row.find('.tdIsWithPressure').find('input').is(':checked');

        $('#<%=hdnID.ClientID %>').val(ID);
        $('#<%=txtBedCode.ClientID %>').val(bedCode);
        $('#<%=txtExtensionNo.ClientID %>').val(extensionNo);
        cboBedStatus.SetValue(GCBedStatus);
        $('#<%=chkIsNeedConfirmation.ClientID %>').prop('checked', isNeedConfirmation);
        $('#<%=chkIsTemporary.ClientID %>').prop('checked', isTemporary);
        $('#<%=chkISBORCalculation.ClientID %>').prop('checked', isBORCalculation);
        $('#<%=chkIsPatientAccompany.ClientID %>').prop('checked', isPatientAccompany);
        $('#<%=chkIsNewBornBed.ClientID %>').prop('checked', isNewBornBed);
        $('#<%=chkIsPandemicBed.ClientID %>').prop('checked', isPandemicBed);
        $('#<%=chkIsWithVentilator.ClientID %>').prop('checked', isWithVentilator);
        $('#<%=chkIsWithPressure.ClientID %>').prop('checked', isWithPressure);
        $('#containerPopupEntryData').show();
    });

    //#region APLICARES

    function UpdateRoomAplicares() {
        var roomID = $('#<%=hdnRoomID.ClientID %>').val();
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
        });
    }

    //#endregion

    function onCbpEntryPopupViewEndCallback(s) {
        var isBridgingAplicares = $('#<%=hdnIsBridgingToAplicares.ClientID %>').val();
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail') {
                showToast('Save Failed', 'Error Message : ' + param[2]);
            }
            else {
                $('#containerPopupEntryData').hide();

                if (isBridgingAplicares == "1") {
                    UpdateRoomAplicares();
                }
            }
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail') {
                showToast('Delete Failed', 'Error Message : ' + param[2]);
            } else {
                if (isBridgingAplicares == "1") {
                    UpdateRoomAplicares();
                }
            }
        }
        $('#containerImgLoadingView').hide();
    }
</script>

<div style="height:440px; overflow-y:auto">
    <input type="hidden" id="hdnRoomID" value="" runat="server" />
    <input type="hidden" id="hdnIsBridgingToAplicares" value="" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width:100%"/>
        </colgroup>
        <tr>            
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:70%">
                    <colgroup>
                        <col style="width:160px"/>
                        <col/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Kamar")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtHeaderText" ReadOnly="true" Width="100%" runat="server" /></td>
                    </tr>  
                </table>

                <div id="containerPopupEntryData" style="margin-top:10px;display:none;">
                    <input type="hidden" id="hdnID" runat="server" value="" />
                    <div class="pageTitle"><%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin:0"> 
                        <table class="tblEntryDetail" style="width:100%">
                            <colgroup>
                                <col style="width:150px"/>
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Kode Tempat Tidur")%></label></td>
                                <td><asp:TextBox ID="txtBedCode" CssClass="required" runat="server" Width="200px" /></td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("No Extension")%></label></td>
                                <td><asp:TextBox ID="txtExtensionNo" CssClass="required" runat="server" Width="200px" /></td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Status Kamar")%></label></td>
                                <td><dxe:ASPxComboBox ID="cboBedStatus" CssClass="required" ClientInstanceName="cboBedStatus" runat="server" Width="200px" /></td>
                            </tr>
                            <tr style="display: none">
                                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Butuh Konfirmasi")%></label></td>
                                <td><asp:CheckBox ID="chkIsNeedConfirmation" runat="server" /></td>
                            </tr>
                            <tr>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                    <colgroup>
                                        <col width="5%" />
                                        <col width="10%" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsTemporary" runat="server" Text=" Sementara" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsPandemicBed" runat="server" Text=" Pandemic" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkISBORCalculation" runat="server" Text=" Perhitungan BOR" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsWithVentilator" runat="server" Text=" Dengan Ventilator" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsPatientAccompany" runat="server" Text=" Penunggu Pasien" />
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkIsWithPressure" runat="server" Text=" Dengan Tekanan" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsNewBornBed" runat="server" Text=" Bayi Baru Lahir" />
                                        </td>
                                    </tr>
                                </table>
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

                <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewPopup').show(); }"
                        EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false" OnRowDataBound="grdView_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="130px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <img class='imgEdit <%#: Eval("IsAllowEditBed").ToString() == "False" ? "imgDisabled" : "imgLink"%>' title='<%=GetLabel("Edit")%>' 
                                                    src='<%# Eval("IsAllowEditBed").ToString() == "False" ? ResolveUrl("~/Libs/Images/Button/edit_disabled.png") : ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt="" style="float:left;margin-right: 2px;" />
                                                <img class='imgDelete <%#: Eval("IsAllowEditBed").ToString() == "False" ? "imgDisabled" : "imgLink"%>' title='<%=GetLabel("Delete")%>' 
                                                    src='<%# Eval("IsAllowEditBed").ToString() == "False" ? ResolveUrl("~/Libs/Images/Button/delete_disabled.png") : ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" />
                                                
                                                <input type="hidden" class="hdnID" value="<%#: Eval("BedID")%>" />
                                                <input type="hidden" class="hdnGCBedStatus" value="<%#: Eval("GCBedStatus")%>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="BedCode" ItemStyle-CssClass="tdBedCode" HeaderText="Kode Tempat Tidur" HeaderStyle-Width="100px" />
                                        <asp:BoundField DataField="ExtensionNo" ItemStyle-CssClass="tdExtensionNo" HeaderText="No Extension" HeaderStyle-Width="150px" />
                                        <asp:BoundField DataField="BedStatus" ItemStyle-CssClass="tdBedStatus" HeaderText="Status Tempat Tidur" HeaderStyle-Width="200px" />
                                        <asp:CheckBoxField DataField="IsTemporary" ItemStyle-CssClass="tdIsTemporary" HeaderText="Sementara" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:CheckBoxField DataField="IsBORCalculation" ItemStyle-CssClass="tdISBORCalculation" HeaderText="Perhitungan BOR" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  />
                                        <asp:CheckBoxField DataField="IsPatientAccompany" ItemStyle-CssClass="tdIsPatientAccompany" HeaderText="Penunggu Pasien" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:CheckBoxField DataField="IsNewBornBed" ItemStyle-CssClass="tdIsNewBornBed" HeaderText="Bayi Baru Lahir" HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:CheckBoxField DataField="IsPandemicBed" ItemStyle-CssClass="tdIsPandemicBed" HeaderText="Pandemic" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:CheckBoxField DataField="IsWithVentilator" ItemStyle-CssClass="tdIsWithVentilator" HeaderText="Dengan Ventilator" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:CheckBoxField DataField="IsWithPressure" ItemStyle-CssClass="tdIsWithPressure" HeaderText="Dengan Tekanan" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Data Tidak Tersedia")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div style="width:100%;text-align:center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblEntryPopupAddData"><%= GetLabel("Tambah Data")%></span>
                </div>
            </td>
        </tr>
    </table>
</div>

