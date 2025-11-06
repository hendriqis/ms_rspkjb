<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master" AutoEventWireup="true"
    CodeBehind="InformationOutstandingUDDList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.InformationOutstandingUDDList" %>

<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div style="font-size: 1.4em">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnProcess" style='display: none' runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><div>
            <%=GetLabel("Send Email")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            setDatePicker('<%=txtRegistrationDate.ClientID %>');
        }

        $('#<%=txtRegistrationDate.ClientID %>').live('change', function () {
        });

        //#region Process
        $('#<%=btnProcess.ClientID %>').live('click', function () {
            getCheckedMember();
            if ($('#<%=hdnSelectedMember.ClientID %>').val() == '') {
                showToast('Warning', 'Harap Pilih Data Terlebih Dahulu');
            }
            else {
                showToastConfirmation('Apakah yakin akan proses kirim email ?', function (result) {
                    if (result) {
                        onCustomButtonClick('process');
                    }
                });
            }
        });
        //#endregion

        function onAfterCustomClickSuccess(type, retval) {
            $('#<%=hdnSelectedMember.ClientID %>').val('');
            cbpView.PerformCallback('refresh');
        }

        $('#chkSelectAll').die('change');
        $('#chkSelectAll').live('change', function () {
            var isChecked = $(this).is(":checked");
            $('.chkIsSelected input').each(function () {
                $(this).prop('checked', isChecked);
                $(this).change();
            });
        });
        function getCheckedMember() {
            var lstSelectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().split(',');
            $('.grdEmail .chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    $tr = $(this).closest('tr');
                    var key = $tr.find('.keyField').html().trim();
                    var idx = lstSelectedMember.indexOf(key);
                    if (idx < 0) {
                        lstSelectedMember.push(key);
                    }
                }
            });
            $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
        }

        $('#lblServiceUnit.lblLink').live('click', function () {
            var filterExpression = "DepartmentID = 'INPATIENT' AND IsDeleted = 0 AND IsUsingRegistration = 1";
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
                    $('#<%=hdnServiceUnitID.ClientID %>').val('0');
                    $('#<%=txtServiceUnitCode.ClientID %>').val('');
                    $('#<%=txtServiceUnitName.ClientID %>').val('');
                }
            });
        }

        function refreshGrdRegisteredPatient() {
            cbpView.PerformCallback('refresh');
        }

        $('#btnRefresh').live('click', function () {
            cbpView.PerformCallback('refresh');
        });

        function onCboSequenceValueChanged(evt) {
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
        }
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" value="" />
    <input type="hidden" value="" id="hdnSelectedMember" runat="server" />
    <div>
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 40%" />
                <col style="width: 60%" />
            </colgroup>
            <tr>
                <td>
                    <h4>
                        <%=GetLabel("Data Pencarian")%></h4>
                    <div class="containerTblEntryContent">
                        <table class="tblEntryContent" style="width: 100%">
                            <colgroup>
                                <col style="width: 10%" />
                                <col style="width: 15%" />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Farmasi")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" Width="100%"
                                        runat="server">
                                        <ClientSideEvents ValueChanged="function(s,e) { refreshGrdRegisteredPatient(); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink" id="lblServiceUnit">
                                        <%=GetLabel("Unit Pelayanan")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnServiceUnitID" runat="server" value="0" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 120px" />
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
                            <tr id="trDate" runat="server">
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Tanggal Registrasi")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRegistrationDate" Width="120px" runat="server" CssClass="datepicker" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Sequence")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboSequence" ClientInstanceName="cboSequence" Width="10%" runat="server">
                                        <ClientSideEvents ValueChanged="function(s,e) { onCboSequenceValueChanged(); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <input type="button" id="btnRefresh" value="R e f r e s h" class="btnRefresh w3-button w3-blue w3-border w3-border-blue w3-round-large" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height: 350px;
                                    overflow-y: scroll;">
                                    <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdEmail grdSelected" cellspacing="0" rules="all">
                                                <tr>
                                                    <th class="keyField">
                                                        &nbsp;
                                                    </th>
                                                    <th style="width: 20px; display: none">
                                                        &nbsp;
                                                    </th>
                                                    <th style="width: 250px">
                                                        <%=GetLabel("Informasi Registrasi")%>
                                                    </th>
                                                    <th style="width: 250px">
                                                        <%=GetLabel("Informasi Kunjungan")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("Pasien")%>
                                                    </th>
                                                    <th style="width: 350px">
                                                        <%=GetLabel("Dokter")%>
                                                    </th>
                                                    <th style="width: 500px">
                                                        <%=GetLabel("Item")%>
                                                    </th>
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="6">
                                                        <%=GetLabel("No Data To Display")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdEmail grdSelected" cellspacing="0" rules="all">
                                                <tr>
                                                    <th class="keyField">
                                                    </th>
                                                    <th style="width: 20px; text-align: center; display: none">
                                                        <input id="chkSelectAll" type="checkbox" />
                                                    </th>
                                                    <th style="width: 250px">
                                                        <%=GetLabel("Informasi Registrasi")%>
                                                    </th>
                                                    <th style="width: 250px">
                                                        <%=GetLabel("Informasi Kunjungan")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("Pasien")%>
                                                    </th>
                                                    <th style="width: 350px">
                                                        <%=GetLabel("Dokter")%>
                                                    </th>
                                                    <th style="width: 500px">
                                                        <%=GetLabel("Item")%>
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td class="keyField">
                                                    <%#: Eval("VisitID")%>
                                                </td>
                                                <td align="center" style='display: none'>
                                                    <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                </td>
                                                <td>
                                                    <input type="hidden" class="hdnRegistrationID" value='<%#: Eval("RegistrationID") %>'
                                                        bindingfield="hdnRegistrationID" />
                                                    <div style="padding: 3px; text-align: left">
                                                        <%#: Eval("RegistrationNo")%>
                                                        <br>
                                                        <%#: Eval("cfRegistrationDateInString")%>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: left">
                                                        <%#: Eval("DepartmentID")%><br>
                                                        <%#: Eval("ServiceUnitName")%>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: left">
                                                        (<%#: Eval("MedicalNo")%>)
                                                        <%#: Eval("PatientName")%>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: left">
                                                        (<%#: Eval("ParamedicCode")%>)
                                                        <%#: Eval("ParamedicName")%>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="padding: 3px; text-align: left">
                                                        <asp:Repeater ID="rptItemName" runat="server">
                                                            <ItemTemplate>
                                                                <div style="padding-left: 2px; margin-top: 2px;">
                                                                    <div>
                                                                        <%#: Eval("NamaItem")%>
                                                                    </div>
                                                                </div>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <br style="clear: both" />
                                                            </FooterTemplate>
                                                        </asp:Repeater>
                                                    </div>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
