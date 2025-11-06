<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="ChangeStatusMCUList.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalCheckup.Program.ChangeStatusMCUList" %>

<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPivotGrid" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v11.1.Export, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPivotGrid.Export" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Refresh")%></div>
    </li>
    <li id="btnClose" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbapprove.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Close")%></div>
    </li>
    <li id="btnReopen" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Reopen")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtRegistrationDate.ClientID %>');
            $('#<%=txtRegistrationDate.ClientID %>').datepicker('option', 'maxDate', '0');
        });

        $('#<%=btnRefresh.ClientID %>').live('click', function () {
            cbpView.PerformCallback('refresh');
        });

        $('#<%=btnClose.ClientID %>').live('click', function () {
            if (cboMCUStatus.GetValue() == Constant.TransactionStatus.OPEN) {
                showToastConfirmation('Apakah anda yakin akan menutup transaksi hasil MCU dari registrasi terpilih ?', function (result) {
                    if (result) {
                        $('#<%=hdnSelectedRegistrationID.ClientID %>').val("");
                        getSelectedRegistration();
                        if ($('#<%=hdnSelectedRegistrationID.ClientID %>').val() != "") {
                            onCustomButtonClick('close');
                        } else {
                            showToast('Warning', 'Silakan pilih registrasi terlebih dahulu.');
                        }
                    }
                });
            } else {
                showToast('Warning', 'Silahkan pilih status transaksi OPEN jika ingin melakukan proses CLOSE status.');
            }
        });

        $('#<%=btnReopen.ClientID %>').live('click', function () {
            if (cboMCUStatus.GetValue() == Constant.TransactionStatus.CLOSED) {
                showToastConfirmation('Apakah anda yakin akan membuka ulang transaksi hasil MCU dari registrasi terpilih ?', function (result) {
                    if (result) {
                        $('#<%=hdnSelectedRegistrationID.ClientID %>').val("");
                        getSelectedRegistration();
                        if ($('#<%=hdnSelectedRegistrationID.ClientID %>').val() != "") {
                            onCustomButtonClick('reopen');
                        } else {
                            showToast('Warning', 'Silakan pilih registrasi terlebih dahulu.');
                        }
                    }
                });
            } else {
                showToast('Warning', 'Silahkan pilih status transaksi CLOSED jika ingin melakukan proses REOPEN status.');
            }
        });

        $('#chkSelectAll').die('change');
        $('#chkSelectAll').live('change', function () {
            var isChecked = $(this).is(":checked");
            $('.lvwView .chkIsSelected').each(function () {
                $chk = $(this).find('input');
                $chk.prop('checked', isChecked);
                $chk.change();
            });
        });

        function getSelectedRegistration() {
            var lstSeletectedRegID = $('#<%=hdnSelectedRegistrationID.ClientID %>').val().split(',');
            $('.lvwView .chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    var $tr = $(this).closest('tr');
                    var key = $tr.find('.keyField').val();
                    var idx = lstSeletectedRegID.indexOf(key);
                    if (idx < 0) {
                        lstSeletectedRegID.push(key);
                    }
                    else {
                    }
                }
                else {
                    var $tr = $(this).closest('tr');
                    var key = $tr.find('.keyField').html();
                    var idx = lstSeletectedRegID.indexOf(key);
                    if (idx > -1) {
                        lstSeletectedRegID.splice(idx, 1);
                    }
                }
            });

            $('#<%=hdnSelectedRegistrationID.ClientID %>').val(lstSeletectedRegID.join(','));
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
        }

        function onAfterCustomClickSuccess(type, retval) {
            cbpView.PerformCallback('refresh');
        }

    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnSelectedRegistrationID" runat="server" />
    <table width="100%">
        <colgroup>
            <col style="width: 50%" />
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="vertical-align: top" align="left">
                <table>
                    <colgroup>
                        <col style="width: 150px" />
                        <col style="width: 350px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="tdLabel">
                                <%=GetLabel("Status Transaksi")%></label>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox ID="cboMCUStatus" ClientInstanceName="cboMCUStatus" Width="120px"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tanggal Registrasi")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRegistrationDate" Width="120px" runat="server" CssClass="datepicker" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                position: relative; font-size: 0.95em;">
                                <asp:ListView runat="server" ID="lvwView">
                                    <EmptyDataTemplate>
                                        <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                            rules="all">
                                            <tr>
                                                <th style="width: 35px" align="center">
                                                    <input id="chkSelectAll" type="checkbox" />
                                                </th>
                                                <th style="width: 150px" align="left">
                                                    <%=GetLabel("No / Tgl Registrasi")%>
                                                </th>
                                                <th style="width: 150px" align="left">
                                                    <%=GetLabel("No / Tgl Perjanjian")%>
                                                </th>
                                                <th align="left">
                                                    <%=GetLabel("Pasien")%>
                                                </th>
                                                <th style="width: 180px" align="left">
                                                    <%=GetLabel("Penjamin Bayar")%>
                                                </th>
                                                <th style="width: 180px" align="left">
                                                    <%=GetLabel("Dokter")%>
                                                </th>
                                                <th style="width: 180px" align="left">
                                                    <%=GetLabel("Item MCU")%>
                                                </th>
                                                <th style="width: 125px" align="center">
                                                    <%=GetLabel("Informasi Close MCU")%>
                                                </th>
                                                <th style="width: 125px" align="center">
                                                    <%=GetLabel("Informasi Reopen MCU")%>
                                                </th>
                                            </tr>
                                            <tr class="trEmpty">
                                                <td colspan="20">
                                                    <%=GetLabel("No Data To Display")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <table id="tblView" runat="server" class="lvwView grdView notAllowSelect" cellspacing="0"
                                            rules="all">
                                            <tr>
                                                <th style="width: 35px" align="center">
                                                    <input id="chkSelectAll" type="checkbox" />
                                                </th>
                                                <th style="width: 150px" align="left">
                                                    <%=GetLabel("No / Tgl Registrasi")%>
                                                </th>
                                                <th align="left">
                                                    <%=GetLabel("Pasien")%>
                                                </th>
                                                <th style="width: 170px" align="left">
                                                    <%=GetLabel("Penjamin Bayar")%>
                                                </th>
                                                <th style="width: 170px" align="left">
                                                    <%=GetLabel("Dokter")%>
                                                </th>
                                                <th style="width: 170px" align="left">
                                                    <%=GetLabel("Item MCU")%>
                                                </th>
                                                <th style="width: 125px" align="center">
                                                    <%=GetLabel("Status MCU")%>
                                                </th>
                                                <th style="width: 150px" align="center">
                                                    <%=GetLabel("Informasi Close MCU")%>
                                                </th>
                                                <th style="width: 150px" align="center">
                                                    <%=GetLabel("Informasi Reopen MCU")%>
                                                </th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td align="center">
                                                <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                <input type="hidden" class="keyField" id="keyField" runat="server" value='<%#: Eval("RegistrationID")%>' />
                                            </td>
                                            <td>
                                                <div style="font-weight: bold">
                                                    <%#:Eval("RegistrationNo") %></div>
                                                <div style="font-size: smaller">
                                                    <%#:Eval("cfRegistrationDateInString") %></div>
                                                <div style="font-size: smaller">
                                                    <%=GetLabel("No. Perjanjian : ")%><%#:Eval("AppointmentNo") %>
                                                </div>
                                            </td>
                                            <td>
                                                <div style="font-weight: bold">
                                                    <%#:Eval("PatientName") %></div>
                                                <div style="font-size: smaller">
                                                    <%=GetLabel("No. RM : ")%><%#:Eval("MedicalNo") %>
                                                </div>
                                                <div style="font-size: smaller">
                                                    <%=GetLabel("No. Pengunjung : ")%><%#:Eval("GuestNo") %>
                                                </div>
                                                <div style="font-size: smaller">
                                                    <%=GetLabel("No. Karyawan : ")%><%#:Eval("CorporateAccountNo") %>
                                                </div>
                                                <div style="font-size: smaller">
                                                    <%=GetLabel("Unit Karyawan : ")%><%#:Eval("CorporateAccountDepartment") %>
                                                </div>
                                            </td>
                                            <td>
                                                <%#:Eval("BusinessPartnerName") %><br />
                                                <i>
                                                    <%#:Eval("BusinessPartnerCode") %></i>
                                            </td>
                                            <td>
                                                <%#:Eval("ParamedicName") %><br />
                                                <i>
                                                    <%#:Eval("ParamedicCode") %></i>
                                            </td>
                                            <td>
                                                <%#:Eval("ItemName1") %><br />
                                                <i>
                                                    <%#:Eval("ItemCode") %></i>
                                            </td>
                                            <td align="center">
                                                <%#:Eval("MCUStatus") %>
                                            </td>
                                            <td align="center">
                                                <div>
                                                    <%#:Eval("MCUClosedByName") %></div>
                                                <div style="font-size: smaller">
                                                    <%#:Eval("cfMCUClosedDateTimeInString") %></div>
                                            </td>
                                            <td align="center">
                                                <div>
                                                    <%#:Eval("MCUReopenByName") %></div>
                                                <div style="font-size: smaller">
                                                    <%#:Eval("cfMCUReopenDateTimeInString") %></div>
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
</asp:Content>
