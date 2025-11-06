<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPMain.master" AutoEventWireup="true"
    CodeBehind="InformationAplicares.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.InformationAplicares" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">
    <script type="text/javascript">
        //#region Service Unit
        $('#lblServiceUnit.lblLink').live('click', function () {
            var DepartmentID = "INPATIENT";
            var filterExpression = '';
            if (DepartmentID != '')
                filterExpression = "DepartmentID = '" + DepartmentID + "' AND IsDeleted = 0 AND IsUsingRegistration = 1";
            openSearchDialog('serviceunitperhealthcare', filterExpression, function (value) {
                $('#<%=txtServiceUnitCode.ClientID %>').val(value);
                onTxtServiceUnitCodeChanged(value);
            });
        });

        $('#<%=txtServiceUnitCode.ClientID %>').live('change', function () {
            onTxtServiceUnitCodeChanged($(this).val());
        });

        function onTxtServiceUnitCodeChanged(value) {
            var filterExpression = "ServiceUnitCode = '" + value + "'";
            Methods.getObject('GetvHealthcareServiceUnitList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                    $('#<%=txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);
                }
                else {
                    $('#<%=hdnServiceUnitID.ClientID %>').val('');
                    $('#<%=txtServiceUnitCode.ClientID %>').val('');
                    $('#<%=txtServiceUnitName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        $('.btnUpdate').live('click', function () {
            var HealthcareServiceUnitID = $(this).closest('tr').find('.HealthcareServiceUnitID').val();
            var ServiceUnitName = $(this).closest('tr').find('.ServiceUnitName').val();
            var ClassID = $(this).closest('tr').find('.ClassID').val();
            var ClassName = $(this).closest('tr').find('.ClassName').val();

            var messageConfirm = "Yakin ingin <b>menyimpan ulang</b> ketersediaan kamar dari Aplicares untuk <b>" + ServiceUnitName + " " + ClassName + "</b> ?";
            showToastConfirmation(messageConfirm, function (result) {
                if (result) {
                    var isBridgingAplicares = $('#<%=hdnIsBridgingToAplicares.ClientID %>').val();
                    if (isBridgingAplicares == "1") {
                        UpdateRoomAplicares(HealthcareServiceUnitID, ClassID);
                    }
                }
            });
        });

        $('.btnDelete').live('click', function () {
            var HealthcareServiceUnitID = $(this).closest('tr').find('.HealthcareServiceUnitID').val();
            var ServiceUnitName = $(this).closest('tr').find('.ServiceUnitName').val();
            var ClassID = $(this).closest('tr').find('.ClassID').val();
            var ClassName = $(this).closest('tr').find('.ClassName').val();

            var messageConfirm = "Yakin ingin <b>menghapus</b> ketersediaan kamar dari Aplicares untuk <b>" + ServiceUnitName + " " + ClassName + "</b> ?";
            showToastConfirmation(messageConfirm, function (result) {
                if (result) {
                    var isBridgingAplicares = $('#<%=hdnIsBridgingToAplicares.ClientID %>').val();
                    if (isBridgingAplicares == "1") {
                        DeleteRoomAplicares(HealthcareServiceUnitID, ClassID);
                    }
                }
            });
        });

        //#region APLICARES

        function UpdateRoomAplicares(HealthcareServiceUnitID, ClassID) {
            var filterExpression = "HealthcareServiceUnitID = " + HealthcareServiceUnitID + " AND ClassID = " + ClassID;
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
                                                    refreshGrdAplicares();
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
                                                    refreshGrdAplicares();
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

        function DeleteRoomAplicares(HealthcareServiceUnitID, ClassID) {
            var filterExpression = "HealthcareServiceUnitID = " + HealthcareServiceUnitID + " AND ClassID = " + ClassID;
            Methods.getListObject('GetvServiceUnitAplicaresList', filterExpression, function (result) {
                for (i = 0; i < result.length; i++) {
                    var kodeKelas = result[i].AplicaresClassCode;
                    var kodeRuang = result[i].ServiceUnitCode;

                    var hsuID = result[i].HealthcareServiceUnitID;
                    var classID = result[i].ClassID;

                    AplicaresService.deleteClassRoom(kodeKelas, kodeRuang, function (resultRoom) {
                        if (resultRoom != null) {
                            try {
                                AplicaresService.updateStatusDeleteFromAplicares(hsuID, classID, function (resultUpdate) {
                                    if (resultUpdate != null) {
                                        try {
                                            var resultUpdate = resultUpdate.split('|');
                                            if (resultUpdate[0] == "1") {
                                                showToast('INFORMATION', "SUCCESS");
                                                refreshGrdAplicares();
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
            });
        }

        //#endregion

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            Methods.checkImageError('imgPatientImage', 'patient', 'hdnPatientGender');
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
        }

        function refreshGrdAplicares() {
            cbpView.PerformCallback('refresh');
        }

        $('#lblRefresh.lblLink').live('click', function (evt) {
            refreshGrdAplicares();
        });
    </script>
    <input type="hidden" value="" id="hdnParamHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnParamClassID" runat="server" />
    <input type="hidden" id="hdnIsBridgingToAplicares" value="" runat="server" />
    <div style="padding: 15px">
        <div class="pageTitle">
            <%=GetMenuCaption()%></div>
        <table class="tblContentArea" style="width: 100%">
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <fieldset id="fsPatientList">
                        <table class="tblEntryContent" style="width: 60%;">
                            <colgroup>
                                <col style="width: 15%" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink" id="lblServiceUnit">
                                        <%=GetLabel("Unit Pelayanan")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnServiceUnitID" runat="server" value="" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30%" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtServiceUnitCode" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtServiceUnitName" ReadOnly="true" Width="100%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Kelas Aplicares")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboAplicaresClass" ClientInstanceName="cboAplicaresClass" Width="30%"
                                        runat="server">
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <div style="padding: 7px 0 0 3px; font-size: 0.95em">
                                <span class="lblLink" id="lblRefresh">[refresh]</span>
                            </div>
                        </table>
                    </fieldset>
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlGridView" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em; height: 450px; overflow-y: scroll;">
                                    <asp:ListView runat="server" ID="lvwView">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdCollapsible lvwView" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th style="text-align: center;" colspan="2">
                                                        <%=GetLabel("Unit Pelayanan")%>
                                                    </th>
                                                    <th style="width: 120px; text-align: left;" rowspan="2">
                                                        <%=GetLabel("Kelas Perawatan")%>
                                                    </th>
                                                    <th style="text-align: center;" colspan="2">
                                                        <%=GetLabel("Aplicares")%>
                                                    </th>
                                                    <th style="text-align: center;" colspan="3">
                                                        <%=GetLabel("Jumlah Tempat Tidur")%>
                                                    </th>
                                                    <th style="width: 100px; text-align: center;" rowspan="2">
                                                        <%=GetLabel("Update")%>
                                                    </th>
                                                    <th style="width: 100px; text-align: center;" rowspan="2">
                                                        <%=GetLabel("Delete")%>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th style="width: 100px; text-align: left;">
                                                        <%=GetLabel("Kode Unit")%>
                                                    </th>
                                                    <th style="width: 200px; text-align: left;">
                                                        <%=GetLabel("Nama Unit")%>
                                                    </th>
                                                    <th style="width: 70px; text-align: left;">
                                                        <%=GetLabel("Kode Kelas")%>
                                                    </th>
                                                    <th style="width: 100px; text-align: left;">
                                                        <%=GetLabel("Nama Kelas")%>
                                                    </th>
                                                    <th style="width: 100px; text-align: right;">
                                                        <%=GetLabel("Seluruh")%>
                                                    </th>
                                                    <th style="width: 100px; text-align: right;">
                                                        <%=GetLabel("Terisi")%>
                                                    </th>
                                                    <th style="width: 100px; text-align: right;">
                                                        <%=GetLabel("Kosong")%>
                                                    </th>
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="10">
                                                        <%=GetLabel("No data to display.")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdCollapsible lvwView" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th style="text-align: center;" colspan="2">
                                                        <%=GetLabel("Unit Pelayanan")%>
                                                    </th>
                                                    <th style="width: 120px; text-align: left;" rowspan="2">
                                                        <%=GetLabel("Kelas Perawatan")%>
                                                    </th>
                                                    <th style="text-align: center;" colspan="2">
                                                        <%=GetLabel("Aplicares")%>
                                                    </th>
                                                    <th style="text-align: center;" colspan="3">
                                                        <%=GetLabel("Jumlah Tempat Tidur")%>
                                                    </th>
                                                    <th style="width: 100px; text-align: center;" rowspan="2">
                                                        <%=GetLabel("Update")%>
                                                    </th>
                                                    <th style="width: 100px; text-align: center;" rowspan="2">
                                                        <%=GetLabel("Delete")%>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th style="width: 100px; text-align: left;">
                                                        <%=GetLabel("Kode Unit")%>
                                                    </th>
                                                    <th style="width: 200px; text-align: left;">
                                                        <%=GetLabel("Nama Unit")%>
                                                    </th>
                                                    <th style="width: 70px; text-align: left;">
                                                        <%=GetLabel("Kode Kelas")%>
                                                    </th>
                                                    <th style="width: 100px; text-align: left;">
                                                        <%=GetLabel("Nama Kelas")%>
                                                    </th>
                                                    <th style="width: 100px; text-align: right;">
                                                        <%=GetLabel("Seluruh")%>
                                                    </th>
                                                    <th style="width: 100px; text-align: right;">
                                                        <%=GetLabel("Terisi")%>
                                                    </th>
                                                    <th style="width: 100px; text-align: right;">
                                                        <%=GetLabel("Kosong")%>
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
                                                        <input type="hidden" class="HealthcareServiceUnitID" value='<%#: Eval("HealthcareServiceUnitID") %>' />
                                                        <input type="hidden" class="ServiceUnitName" value='<%#: Eval("ServiceUnitName") %>' />
                                                        <input type="hidden" class="ClassID" value='<%#: Eval("ClassID") %>' />
                                                        <input type="hidden" class="ClassName" value='<%#: Eval("ClassName") %>' />
                                                        <%#: Eval("ServiceUnitCode") %>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div>
                                                        <%#: Eval("ServiceUnitName") %>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div>
                                                        <%#: Eval("ClassName") %>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div>
                                                        <%#: Eval("AplicaresClassCode") %>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div>
                                                        <%#: Eval("AplicaresClassName") %>
                                                    </div>
                                                </td>
                                                <td align="right">
                                                    <div>
                                                        <%#: Eval("cfCountBedAllInString") %>
                                                    </div>
                                                </td>
                                                <td align="right">
                                                    <div>
                                                        <%#: Eval("cfCountBedOccupiedInString") %>
                                                    </div>
                                                </td>
                                                <td align="right">
                                                    <div>
                                                        <%#: Eval("cfCountBedEmptyInString") %>
                                                    </div>
                                                </td>
                                                <td align="center">
                                                    <div>
                                                        <input type="button" id="btnUpdate" class="btnUpdate" value="Update" runat="server" />
                                                    </div>
                                                </td>
                                                <td align="center">
                                                    <div>
                                                        <input type="button" id="btnDelete" class="btnDelete" value="Delete" runat="server" />
                                                    </div>
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
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
