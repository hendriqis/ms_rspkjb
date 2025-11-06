<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx2.master"
    AutoEventWireup="true" CodeBehind="OutstandingTransactionOrderList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.OutstandingTransactionOrderList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div style="font-size: 1.4em">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnIsAutoPropose" runat="server" />
    <input type="hidden" value="" id="hdnLaboratoryHSUID" runat="server" />
    <input type="hidden" value="" id="hdnRadiologiHSUID" runat="server" />
    <input type="hidden" value="" id="hdnGCRegistrationStatus" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnClassID" runat="server" />
    <input type="hidden" value="" id="hdnImagingServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnLaboratoryServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnProcessOrder" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Process")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onCboServiceUnitPerHealthcareValueChanged() {
            cbpView.PerformCallback('refresh');
        }

        $('#<%=grdView.ClientID %> #chkSelectAll').live('change', function () {
            var isChecked = $(this).is(':checked');
            $('.chkIsSelected input').each(function () {
                $(this).prop('checked', isChecked);
            });
        });

        $('#<%=btnProcessOrder.ClientID %>').click(function (evt) {
            getCheckedMember();
            if ($('#<%=hdnSelectedMember.ClientID %>').val() == '' && $('#<%=hdnPhysicianID.ClientID %>').val() == '') {
                showToast('Warning', 'Pilih Dokter Realisasi dan Pilih Transaksi Terlebih Dahulu');
            }
            else if ($('#<%=hdnPhysicianID.ClientID %>').val() == '') {
                showToast('Warning', 'Pilih Dokter Realisasi Terlebih Dahulu');
            } 
            else if ($('#<%=hdnSelectedMember.ClientID %>').val() == '') {
                showToast('Warning', 'Pilih Transaksi Terlebih Dahulu');
            }
            else {
                onCustomButtonClick('Process');
            }
        });

        function getCheckedMember() {
            var lstSelectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().split(',');
            var lstSelectedMemberType = $('#<%=hdnSelectedMemberType.ClientID %>').val().split(',');
            var lstSelectedMemberStatus = $('#<%=hdnSelectedMemberStatus.ClientID %>').val().split(',');
            $('#<%=grdView.ClientID %> .chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    $tr = $(this).closest('tr');
                    var key = $tr.find('.keyField').html();
                    var ordertype = $tr.find('.OrderType').val();
                    var orderstatus = $tr.find('.GCTransactionStatus').val();
                    var idx = lstSelectedMember.indexOf(key);
                    if (idx < 0) {
                        lstSelectedMember.push(key);
                        lstSelectedMemberType.push(ordertype);
                        lstSelectedMemberStatus.push(orderstatus);
                    }
                    else {
                        lstSelectedMemberType[idx] = ordertype;
                        lstSelectedMemberStatus[idx] = orderstatus;
                    }
                }
                else {
                    $tr = $(this).closest('tr');
                    var key = $tr.find('.keyField').html();
                    var ordertype = $tr.find('.OrderType').val();
                    var orderstatus = $tr.find('.GCTransactionStatus').val();
                    var idx = lstSelectedMember.indexOf(key);
                    if (idx > -1) {
                        lstSelectedMember.splice(idx, 1);
                        lstSelectedMemberType.splice(idx, 1);
                        lstSelectedMemberStatus.splice(idx, 1);
                    }
                }
            });
            $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
            $('#<%=hdnSelectedMemberType.ClientID %>').val(lstSelectedMemberType.join(','));
            $('#<%=hdnSelectedMemberStatus.ClientID %>').val(lstSelectedMemberStatus.join(','));
        }

        function onCboVoidReasonValueChanged() {
            if (cboVoidReason.GetValue() == Constant.DeleteReason.OTHER)
                $('#trVoidOtherReason').show();
            else
                $('#trVoidOtherReason').hide();
        }

        function onAfterCustomClickSuccess() {
            cbpView.PerformCallback('refresh');
        }

        function onClosePopUp() {
            cbpView.PerformCallback('refresh');
        }

        function onCboServiceUnitPerHealthcareValueChanged(s) {
            $('#<%=hdnPhysicianID.ClientID %>').val('');
            $('#<%=txtPhysicianCode.ClientID %>').val('');
            $('#<%=txtPhysicianName.ClientID %>').val('');
            cbpView.PerformCallback('refresh');
        }


        //#region Physician
        function onGetPhysicianFilterExpression() {
            var filterExpression = "";
            var healthcareServiceUnitID = cboServiceUnitPerHealthcare.GetValue();
            if (healthcareServiceUnitID != '0')
                filterExpression = "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = " + healthcareServiceUnitID + ") AND IsDeleted = 0";
            else
                filterExpression = "IsDeleted = 0";
            return filterExpression;
        }

        $('#lblPhysician.lblLink').live('click', function () {
            openSearchDialog('paramedic', onGetPhysicianFilterExpression(), function (value) {
                $('#<%=txtPhysicianCode.ClientID %>').val(value);
                onTxtPhysicianCodeChanged(value);
            });
        });

        $('#<%=txtPhysicianCode.ClientID %>').live('change', function () {
            onTxtPhysicianCodeChanged($(this).val());
        });

        function onTxtPhysicianCodeChanged(value) {
            var filterExpression = onGetPhysicianFilterExpression() + " AND ParamedicCode = '" + value + "'";
            Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnPhysicianID.ClientID %>').val(result.ParamedicID);
                    $('#<%=txtPhysicianName.ClientID %>').val(result.ParamedicName);
                }
                else {
                    $('#<%=hdnPhysicianID.ClientID %>').val('');
                    $('#<%=txtPhysicianCode.ClientID %>').val('');
                    $('#<%=txtPhysicianName.ClientID %>').val('');
                }
            });
        }
        //#endregion
    </script>
    <input type="hidden" value="" id="TestOrderID" runat="server" />
    <input type="hidden" id="hdnTransactionID" runat="server" value="" />
    <input type="hidden" id="hdnTestOrderID" value="" runat="server" />
    <input type="hidden" id="hdnFromHealthcareServiceUnitID" value="" runat="server" />
    <input type="hidden" id="hdnHealthcareServiceUnitID" value="" runat="server" />
    <input type="hidden" value="" id="hdnTransactionHdID" runat="server" />
    <input type="hidden" value="" id="hdnSelectedMember" runat="server" />
    <input type="hidden" value="" id="hdnSelectedMemberType" runat="server" />
    <input type="hidden" value="" id="hdnSelectedMemberStatus" runat="server" />
    <div>
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label id="lblPenunjangMedis">
                                    <%=GetLabel("Unit Pelayanan ")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox runat="server" ID="cboServiceUnitPerHealthcare" ClientInstanceName="cboServiceUnitPerHealthcare"
                                    Width="400px">
                                    <ClientSideEvents ValueChanged="function(s,e){ onCboServiceUnitPerHealthcareValueChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink lblMandatory" id="lblPhysician">
                                    <%=GetLabel("Dokter / Tenaga Medis")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnPhysicianID" runat="server" value="" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 120px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtPhysicianCode" Width="120px" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPhysicianName" ReadOnly="true" Width="99%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative; font-size: 0.95em;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdService grdNormal notAllowSelect"
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="OrderID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <input id="chkSelectAll" type="checkbox" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                    <input type="hidden" class="TestOrderID" value='<%#: Eval("OrderID")%>' />
                                                    <input type="hidden" class="OrderType" value='<%#: Eval("OrderType")%>' />
                                                    <input type="hidden" class="GCTransactionStatus" value='<%#: Eval("GCTransactionStatus")%>' />
                                                    <input type="hidden" class="hdnHealthcareServiceUnitID" value='<%#: Eval("HealthcareServiceUnitID")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="cfOrderDateInString" HeaderText="Tanggal Order" HeaderStyle-Width="90px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="OrderTime" HeaderText="Jam Order" HeaderStyle-Width="30px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="OrderNo" HeaderText="No. Order" HeaderStyle-Width="150px"
                                                HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="cfScheduledDateInString" HeaderText="Tanggal Dijadwalkan"
                                                HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="ScheduledTime" HeaderText="Jam Dijadwalkan" HeaderStyle-Width="30px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="ServiceUnitName" HeaderText="Unit Pelayanan" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="150px" />
                                            <asp:BoundField DataField="ParamedicName" HeaderText="Diorder (Physician)" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="200px" />
                                            <asp:BoundField DataField="CreatedByName" HeaderText="Diorder (User)" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="200px" />
                                            <asp:BoundField DataField="Remarks" HeaderText="Catatan" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="150px" />
                                            <asp:BoundField DataField="TransactionStatusWatermark" HeaderText="Status" HeaderStyle-Width="80px"
                                                HeaderStyle-HorizontalAlign="Left" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No Data To Display")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                    <table class="tblEntryContent" style="width: 70%">
                                        <colgroup>
                                            <col style="width: 130px" />
                                            <col />
                                        </colgroup>
                                    </table>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="imgLoadingGrdView" id="containerImgLoadingView">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                </td>
            </tr>
            <%--<tr>
                <td style="padding: 5px; vertical-align: top">
                </td>
            </tr>--%>
        </table>
    </div>
</asp:Content>
