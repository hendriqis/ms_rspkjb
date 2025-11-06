<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="ArchiveMedicalRecord.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.ArchiveMedicalRecord" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetMenuCaption())%></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnArchive" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><div>
            <%=GetLabel("Archive")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtLastVisitDate.ClientID %>');
            $('#<%=txtLastVisitDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%=txtLastVisitDate.ClientID %>').change(function (evt) {
                onRefreshGridView();
            });

            $('#lblRefresh.lblLink').click(function (evt) {
                onRefreshGridView();
            });
        });

        var interval = parseInt('<%=GetRefreshGridInterval() %>') * 60000;
        var intervalID = window.setInterval(function () {
            onRefreshGridView();
        }, interval);

        $('#chkSelectAll').die('change');
        $('#chkSelectAll').live('change', function () {
            var isChecked = $(this).is(":checked");
            $('.chkIsSelected input').each(function () {
                $(this).prop('checked', isChecked);
                $(this).change();
            });
        });

        function onRefreshGridView() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
                window.clearInterval(intervalID);
                refreshGrdRegisteredPatient();
                intervalID = window.setInterval(function () {
                    onRefreshGridView();
                }, interval);
            }
        }

        function refreshGrdRegisteredPatient() {
            cbpView.PerformCallback('refresh');
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
        }

        $('#<%=btnArchive.ClientID %>').live('click', function () {
            getCheckedMember();
            if ($('#<%=hdnSelectedMember.ClientID %>').val() == '')
                showToast('Warning', 'Please Select Item First');
            else
                onCustomButtonClick('archive');
        });

        function getCheckedMember() {
            var lstSelectedMember = [];
            $('#<%=pnlGridView.ClientID %> tr .chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    var key = $(this).closest('tr').find('.keyField').html();
                    lstSelectedMember.push(key);
                }
            });
            $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join('|'));
        }

        function onAfterCustomClickSuccess(type, retval) {
            cbpView.PerformCallback('refresh');
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var LastVisitDate = $('#<%:txtLastVisitDate.ClientID %>').val();
            if (LastVisitDate == '') {
                errMessage.text = 'Please Choose Date First!';
                return false;
            }
            else {
                if (code == 'MR-00010') {
                    var param = '<%=GetParameterDate() %>';
                    filterExpression.text = param;
                    return true;
                }
                else {
                    errMessage.text = 'Maaf, kode cetakan tidak ditemukan.';
                    return false;
                }

            }
        }
    </script>
    <input type="hidden" id="hdnFilterExpressionQuickSearch" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
    <div>
        <table class="tblContentArea" style="width: 100%">
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <fieldset id="fsPatientList">
                        <table>
                            <colgroup>
                                <col style="width: 60%" />
                                <col style="width: 5%" />
                                <col style="width: 35%" />
                            </colgroup>
                            <tr>
                                <td>
                                    <table class="tblEntryContent" style="width: 90%;">
                                        <colgroup>
                                            <col style="width: 150px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Tgl Kunjungan Terakhir")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtLastVisitDate" Width="120px" runat="server" CssClass="datepicker" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkIsIgnoreDate" runat="server" Checked="false" Text="  Abaikan Tanggal" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td align="right" style="vertical-align: middle;" class="blink-alert">
                                    <img height="60px" src='<%= ResolveUrl("~/Libs/Images/warning.png")%>' alt='' />
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td style="font-size: medium; color: Red">
                                                <b>
                                                    <%=GetLabel("WARNING !")%></b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="font-size: small; color: Red">
                                                <%=GetLabel("Proses Tidak Dapat Dibatalkan !")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="font-size: small; color: Red">
                                                <%=GetLabel("Harap teliti data rekam medis pasien sebelum proses archieve.")%>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <div style="padding: 7px 0 0 3px; font-size: 0.95em">
                        <%=GetLabel("Halaman Ini Akan")%>
                        <span class="lblLink" id="lblRefresh">[refresh]</span>
                        <%=GetLabel("Setiap")%>
                        <%=GetRefreshGridInterval() %>
                        <%=GetLabel("Menit")%>
                    </div>
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s);}" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlGridView" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em; height: 400px; overflow-y: scroll;">
                                    <asp:ListView runat="server" ID="lvwView">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdView lvwView" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th class="keyField">
                                                        &nbsp;
                                                    </th>
                                                    <th style="width: 20px">
                                                        &nbsp;
                                                    </th>
                                                    <th style="width: 150px">
                                                        <%=GetLabel("No. RM")%>
                                                    </th>
                                                    <th style="text-align: left">
                                                        <%=GetLabel("Nama Pasien")%>
                                                    </th>
                                                    <th style="width: 450px; text-align: left">
                                                        <%=GetLabel("Informasi Pasien")%>
                                                    </th>
                                                    <th style="width: 150px">
                                                        <%=GetLabel("Kunjungan Terakhir")%>
                                                    </th>
                                                    <th style="width: 150px">
                                                        <%=GetLabel("Jumlah Kunjungan")%>
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
                                            <table id="tblView" runat="server" class="grdView lvwView" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th class="keyField">
                                                        &nbsp;
                                                    </th>
                                                    <th style="width: 20px">
                                                        &nbsp;
                                                    </th>
                                                    <th style="width: 150px">
                                                        <%=GetLabel("No. RM")%>
                                                    </th>
                                                    <th style="text-align: left">
                                                        <%=GetLabel("Nama Pasien")%>
                                                    </th>
                                                    <th style="width: 450px; text-align: left">
                                                        <%=GetLabel("Informasi Pasien")%>
                                                    </th>
                                                    <th style="width: 150px">
                                                        <%=GetLabel("Kunjungan Terakhir")%>
                                                    </th>
                                                    <th style="width: 150px">
                                                        <%=GetLabel("Jumlah Kunjungan")%>
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr runat="server" id="trItem">
                                                <td class="keyField">
                                                    <%#: Eval("MRN")%>
                                                </td>
                                                <td align="center">
                                                    <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                </td>
                                                <td align="center">
                                                    <div style="font-weight: bold; font-size: medium">
                                                        <%#: Eval("MedicalNo") %>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div>
                                                        <%#: Eval("FullName") %>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div>
                                                        <i>
                                                            <label style="font-size: smaller">
                                                                <%=GetLabel("Tgl. Lahir : ")%></label></i><%#: Eval("cfDateOfBirthInString") %><br />
                                                        <i>
                                                            <label style="font-size: smaller">
                                                                <%=GetLabel("Jenis Kelamin : ")%></label></i><%#: Eval("Gender") %><br />
                                                        <i>
                                                            <label style="font-size: smaller">
                                                                <%=GetLabel("No.Telp : ")%></label></i><%#: Eval("PhoneNo1") %><br />
                                                        <i>
                                                            <label style="font-size: smaller">
                                                                <%=GetLabel("Alamat : ")%></label></i><%#: Eval("StreetName") %><%#: Eval("City") %></div>
                                                </td>
                                                <td align="center">
                                                    <div>
                                                        <%#: Eval("cfLastVisitDateInString") %></div>
                                                </td>
                                                <td align="center">
                                                    <div>
                                                        <%#: Eval("JumlahKunjungan") %></div>
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
