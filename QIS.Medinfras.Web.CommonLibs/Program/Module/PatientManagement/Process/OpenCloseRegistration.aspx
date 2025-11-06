<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="OpenCloseRegistration.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.OpenCloseRegistration" %>

<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnOpenRegistration" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Buka")%></div>
    </li>
    <li id="btnCloseRegistration" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/stop.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Tutup")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=GetMenuCaption()%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        var isRegistrationClosed = false;
        function onLoad() {
            $('#<%=btnOpenRegistration.ClientID %>').click(function () {
                var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
                if (registrationID == '')
                    showToast('Warning', '<%=GetErrorMsgSelectRegistrationFirst() %>');
                else {
                    if (isRegistrationClosed)
                        onCustomButtonClick('openregistration');
                    else
                        showToast('Warning', '<%=GetErrorMsgOpenedRegistration() %>');
                }
            });

            $('#<%=btnCloseRegistration.ClientID %>').click(function () {
                var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
                var filterExpression = 'RegistrationID = ' + registrationID;
                var messageError = '';

                if (registrationID == '')
                    showToast('Warning', '<%=GetErrorMsgSelectRegistrationFirst() %>');
                else {
                    if (!isRegistrationClosed)
                        if ($('#<%=hdnIsCheckResultTest.ClientID %>').val() != '1') {
                            onCustomButtonClick('closeregistration');
                        }
                        else {
                            Methods.getListObject('GetvTestOrderImagingLabWithoutResultList', filterExpression, function (result) {
                                if (result.length > 0) {
                                    for (i = 0; i < result.length; i++) {
                                        if (messageError == '') {
                                            messageError = "Masih ada pemeriksaan yang belum memiliki hasil, di nomor transaksi <b>" + result[i].TransactionNo + "</b>";
                                        }
                                        else {
                                            var info = "Masih ada pemeriksaan yang belum memiliki hasil, di nomor transaksi <b>" + result[i].TransactionNo + "</b>"; ;
                                            messageError = messageError + '<br>' + info;
                                        }
                                    }
                                }
                            });

                            if (messageError != '') {
                                messageError = messageError + '<br> Tetap lanjutkan tutup pendaftaran ?';
                                showToastConfirmation(messageError, function (result) {
                                    if (result) onCustomButtonClick('closeregistration');
                                });
                            }
                            else {
                                onCustomButtonClick('closeregistration');
                            }
                        }
                    else {
                        showToast('Warning', '<%=GetErrorMsgClosedRegistration() %>');
                    }
                }
            });

            //#region No Registrasi
            $('#lblNoReg.lblLink').click(function () {
                var filterExpression = "DepartmentID = '" + $('#<%=hdnDepartmentID.ClientID %>').val() + "'";
                openSearchDialog('registration', filterExpression, function (value) {
                    $('#<%=txtRegistrationNo.ClientID %>').val(value);
                    onTxtRegistrationNoChanged(value);
                });
            });
            $('#<%=txtRegistrationNo.ClientID %>').change(function () {
                onTxtRegistrationNoChanged($(this).val());
            });
            function onTxtRegistrationNoChanged(value) {
                var filterExpression = "DepartmentID = '" + $('#<%=hdnDepartmentID.ClientID %>').val() + "' AND RegistrationNo = '" + value + "'";
                Methods.getObject('GetvRegistrationList', filterExpression, function (entity) {
                    if (entity != null) {
                        if (entity.GCRegistrationStatus != Constant.RegistrationStatus.CANCELLED) {
                            $('#<%=hdnRegistrationID.ClientID %>').val(entity.RegistrationID);
                            $('#<%=txtRegistrationDate.ClientID %>').val(entity.RegistrationDateInString);
                            $('#<%=txtPayer.ClientID %>').val(entity.BusinessPartner);
                            $('#<%=txtRegistrationTime.ClientID %>').val(entity.RegistrationTime);
                            $('#<%=txtServiceUnit.ClientID %>').val(entity.ServiceUnitName);
                            isRegistrationClosed = entity.IsClosed;
                            $('#<%=chkIsClose.ClientID %>').prop('checked', entity.IsClosed);
                            $('#<%=txtMRN.ClientID %>').val(entity.MedicalNo);
                            $('#<%=txtPatientName.ClientID %>').val(entity.PatientName);
                        }
                        else {
                            showToast('Warning', 'Registrasi yang anda pilih sudah di batalkan.');
                            initializeControl();
                        }
                    }
                    else {
                        initializeControl();
                    }
                });
            }
            //#endregion
        }

        function initializeControl() {
            $('#<%=hdnRegistrationID.ClientID %>').val('');
            $('#<%=txtRegistrationNo.ClientID %>').val('');
            $('#<%=txtRegistrationDate.ClientID %>').val('');
            $('#<%=txtPayer.ClientID %>').val('');
            $('#<%=txtRegistrationTime.ClientID %>').val('');
            $('#<%=txtServiceUnit.ClientID %>').val('');

            $('#<%=chkIsClose.ClientID %>').prop('checked', false);
            $('#<%=txtMRN.ClientID %>').val('');
            $('#<%=txtPatientName.ClientID %>').val('');
        }

        function onAfterCustomClickSuccess(type) {
            isRegistrationClosed = !isRegistrationClosed;
            $('#<%=chkIsClose.ClientID %>').prop('checked', isRegistrationClosed);
        }
    </script>
    <input type="hidden" id="hdnDepartmentID" value="" runat="server" />
    <input type="hidden" id="hdnRegistrationID" value="" runat="server" />
    <input type="hidden" id="hdnIsCheckResultTest" value="" runat="server" />
    <input type="hidden" id="hdnIsHasOutstandingOrder" value="" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td colspan="2">
                <h4>
                    <%=GetLabel("Data Registrasi")%></h4>
            </td>
        </tr>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 30%" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink" id="lblNoReg">
                                <%=GetLabel("No Registrasi")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRegistrationNo" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tanggal Registrasi")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRegistrationDate" Width="120px" runat="server" Style="text-align: center" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Pembayar")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPayer" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 30%" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Status")%></label>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsClose" runat="server" /><%=GetLabel("Tutup")%>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Jam Registrasi")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRegistrationTime" Width="80px" runat="server" Style="text-align: center" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Unit Pelayanan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtServiceUnit" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <h4>
                    <%=GetLabel("Data Pasien")%></h4>
            </td>
        </tr>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 30%" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. RM")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMRN" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nama Pasien")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPatientName" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
