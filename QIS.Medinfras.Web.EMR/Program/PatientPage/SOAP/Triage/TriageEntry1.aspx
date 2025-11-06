<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MPPatientPageList.master"
    AutoEventWireup="true" CodeBehind="TriageEntry1.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.TriageEntry1" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>


<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnPatientStatusSave" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><div>
            <%=GetLabel("Save")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" id="dxss_erpatientstatus1">
        $(function () {
            setDatePicker('<%=txtServiceDate.ClientID %>');
            $('#<%=txtServiceDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%:txtChiefComplaint.ClientID %>').focus();

            $('#<%=btnPatientStatusSave.ClientID %>').click(function (evt) {
                if (IsValid(evt, 'fsPatientStatus', 'mpPatientStatus'))
                    onCustomButtonClick('save');
            });

            $('#<%=grdVitalSignView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdVitalSignView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnVitalSignRecordID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdVitalSignView.ClientID %> tr:eq(1)').click();

            $('#<%=grdProcedureView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdProcedureView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnProcedureID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdProcedureView.ClientID %> tr:eq(1)').click();

            registerCollapseExpandHandler();
        });
        
        function onAfterCustomClickSuccess(type, retval) {
        }

        function onCboVisitReasonValueChanged() {
            if (cboVisitReason.GetValue() == Constant.VisitReason.OTHER)
                $('#<%=txtVisitNotes.ClientID %>').removeAttr('readonly');
            else
                $('#<%=txtVisitNotes.ClientID %>').attr('readonly', 'readonly');
        }

        function onCboReferralValueChanged(s) {
            $('#<%:hdnReferrerID.ClientID %>').val('');
            $('#<%:hdnReferrerParamedicID.ClientID %>').val('');
            $('#<%:txtReferralDescriptionCode.ClientID %>').val('');
            $('#<%:txtReferralDescriptionName.ClientID %>').val('');
            if (cboReferral.GetValue() != '' && cboReferral.GetValue() != null) {
                $('#<%:lblReferralDescription.ClientID %>').attr('class', 'lblLink');
                $('#<%:txtReferralDescriptionCode.ClientID %>').removeAttr('readonly');
            }
            else {
                $('#<%:lblReferralDescription.ClientID %>').attr('class', 'lblDisabled');
                $('#<%:txtReferralDescriptionCode.ClientID %>').attr('readonly', 'readonly');
            }
        }

        //#region Diagnose
        $('#<%:lblDiagnose.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('diagnose', "IsDeleted = 0", function (value) {
                $('#<%=txtDiagnoseCode.ClientID %>').val(value);
                onTxtDiagnoseCodeChanged(value);
            });
        });

        $('#<%=txtDiagnoseCode.ClientID %>').live('change', function () {
            onTxtDiagnoseCodeChanged($(this).val());
        });

        function onTxtDiagnoseCodeChanged(value) {
            var filterExpression = "DiagnoseID = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetDiagnoseList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=txtDiagnoseName.ClientID %>').val(result.DiagnoseName);
                    $('#<%=txtDiagnoseText.ClientID %>').val(result.DiagnoseName);
                }
                else {
                    $('#<%=txtDiagnoseCode.ClientID %>').val('');
                    $('#<%=txtDiagnoseName.ClientID %>').val('');
                    $('#<%=txtDiagnoseText.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Referral Description
        function getReferralDescriptionFilterExpression() {
            var filterExpression = "GCReferrerGroup = '" + cboReferral.GetValue() + "' AND IsDeleted = 0";
            return filterExpression;
        }

        function getReferralParamedicFilterExpression() {
            var filterExpression = "GCParamedicMasterType = '" + Constant.ParamedicType.Physician + "'";
            return filterExpression;
        }

        $('#<%:lblReferralDescription.ClientID %>.lblLink').live('click', function () {
            var referral = cboReferral.GetValue();
            if (referral == Constant.ReferrerGroup.DOKTERRS) {
                openSearchDialog('referrerparamedic', getReferralParamedicFilterExpression(), function (value) {
                    $('#<%:txtReferralDescriptionCode.ClientID %>').val(value);
                    onTxtReferralDescriptionCodeChanged(value);
                });
            } else {
                openSearchDialog('referrer', getReferralDescriptionFilterExpression(), function (value) {
                    $('#<%:txtReferralDescriptionCode.ClientID %>').val(value);
                    onTxtReferralDescriptionCodeChanged(value);
                });
            }
        });

        $('#<%:txtReferralDescriptionCode.ClientID %>').live('change', function () {
            onTxtReferralDescriptionCodeChanged($(this).val());
        });

        function onTxtReferralDescriptionCodeChanged(value) {
            var filterExpression = "";
            var referral = cboReferral.GetValue();
            if (referral == Constant.ReferrerGroup.DOKTERRS) {
                filterExpression = getReferralParamedicFilterExpression() + " AND ParamedicCode = '" + value + "'";
                Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%:hdnReferrerParamedicID.ClientID %>').val(result.ParamedicID);
                        $('#<%:txtReferralDescriptionName.ClientID %>').val(result.ParamedicName);
                    }
                    else {
                        $('#<%:hdnReferrerParamedicID.ClientID %>').val('');
                        $('#<%:txtReferralDescriptionCode.ClientID %>').val('');
                        $('#<%:txtReferralDescriptionName.ClientID %>').val('');
                    }
                });
            } else {
                filterExpression = getReferralDescriptionFilterExpression() + " AND BusinessPartnerCode = '" + value + "'";
                Methods.getObject('GetvReferrerList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%:hdnReferrerID.ClientID %>').val(result.BusinessPartnerID);
                        $('#<%:txtReferralDescriptionName.ClientID %>').val(result.BusinessPartnerName);
                    }
                    else {
                        $('#<%:hdnReferrerID.ClientID %>').val('');
                        $('#<%:txtReferralDescriptionCode.ClientID %>').val('');
                        $('#<%:txtReferralDescriptionName.ClientID %>').val('');
                    }
                });
            }
        }
        //#endregion

        var pageCount = parseInt('<%=gridVitalSignPageCount %>');
        var procedurePageCount = parseInt('<%=gridProcedurePageCount %>');
        $(function () {
            setPaging($("#vitalSignPaging"), pageCount, function (page) {
                cbpVitalSignView.PerformCallback('changepage|' + page);
            });
            setPaging($("#procedurePaging"), procedurePageCount, function (page) {
                cbpProcedureView.PerformCallback('changepage|' + page);
            });
        });

        //#region Vital Sign Paging
        $('#lblAddVitalSign').die('click');
        $('#lblAddVitalSign').live('click', function (evt) {
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/VitalSignEntry1Ctl.ascx","", "Vital Sign & Indicator", 700, 500);
        });

        $('.imgEditVitalSign.imgLink').die('click');
        $('.imgEditVitalSign.imgLink').live('click', function () {
            openUserControlPopup("~/Program/PatientPage/_PopupEntry/VitalSignEntry1Ctl.ascx", $('#<%=hdnVitalSignRecordID.ClientID %>').val(), "Vital Sign & Indicator", 700, 500);
        });

        $('.imgDeleteVitalSign.imgLink').die('click');
        $('.imgDeleteVitalSign.imgLink').live('click', function () {
            var message = "Are you sure to delete this vital sign record ?";
            showToastConfirmation(message, function (result) {
                if (result) {
                    cbpDeleteVitalSign.PerformCallback();
                }
            });
        });

        function onCbpVitalSignViewEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdVitalSignView.ClientID %> tr:eq(1)').click();

                setPaging($("#vitalSignPaging"), pageCount, function (page) {
                    cbpVitalSignView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdVitalSignView.ClientID %> tr:eq(1)').click();
        }

        function onCbpVitalSignDeleteEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == '1') {
                cbpVitalSignView.PerformCallback('refresh');
            }
            else {
                showToast("ERROR", 'Error Message : ' + param[1]);
            }
        }

        function onRefreshVitalSignGrid() {
            cbpVitalSignView.PerformCallback('refresh');
        }
        //#endregion

        //#region Procedure
        $('#lblAddProcedure').die('click');
        $('#lblAddProcedure').live('click', function (evt) {
            var url = '';
            var width = 800;
            url = ResolveUrl('~/Program/PatientPage/_PopupEntry/ProcedureEntry1Ctl.ascx');
            var id = '';
            openUserControlPopup(url, id, 'Quick Picks', width, 600);
        });

        function onCbpProcedureViewEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var procedurePageCount = parseInt(param[1]);
                if (procedurePageCount > 0)
                    $('#<%=grdProcedureView.ClientID %> tr:eq(1)').click();

                setPaging($("#procedurePaging"), procedurePageCount, function (page) {
                    cbpProcedureView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdProcedureView.ClientID %> tr:eq(1)').click();
        }

        function onRefreshProcedureGrid() {
            cbpProcedureView.PerformCallback('refresh');
        }
        //#endregion
    </script>
    <div>
        <input type="hidden" runat="server" id="hdnChiefComplaintID" value="" />
        <input type="hidden" value="" id="hdnVitalSignRecordID" runat="server" />
        <input type="hidden" value="" id="hdnProcedureID" runat="server" />
        <input type="hidden" runat="server" id="hdnTimeElapsed0hour" value="00" />
        <input type="hidden" runat="server" id="hdnTimeElapsed0minute" value="00" />
        <input type="hidden" runat="server" id="hdnTimeElapsed1hour" value="00" />
        <input type="hidden" runat="server" id="hdnTimeElapsed1minute" value="00" />
        <input type="hidden" runat="server" id="hdnDepartmentID" value="" />
        <fieldset id="mpPatientStatus">
            <table class="tblContentArea">
                <colgroup>
                    <col style="width: 45%" />
                    <col style="width: 55%" />
                </colgroup>
                <tr>
                    <td style="padding: 5px; vertical-align: top">
                        <table class="tblEntryContent">
                            <colgroup>
                                <col style="width: 150px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel" valign="top">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Tanggal dan Waktu")%></label>
                                </td>
                                <td>
                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtServiceDate" Width="120px" CssClass="datepicker" runat="server" />
                                            </td>
                                            <td style="padding-left:5px">
                                                <asp:TextBox ID="txtServiceTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                   <label class="lblMandatory">
                                    <%=GetLabel("Jenis Kunjungan") %></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox runat="server" ID="cboVisitType" ClientInstanceName="cboVisitType" Width="100%" />
                                </td>
                            </tr>    
                            <tr>
                                <td class="tdLabel" valign="top">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Keluhan Utama")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtChiefComplaint" runat="server" TextMode="MultiLine" Rows="15"
                                        Width="100%" />
                                </td>
                            </tr>           
                            <tr>
                                <td class="tdLabel">
                                    &nbsp;
                                </td>
                                <td>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <td style="width: 50%">
                                            <asp:CheckBox ID="chkAutoAnamnesis" runat="server" Text="Autoanamnesis" Checked="false" />
                                        </td>
                                        <td style="width: 50%">
                                            <asp:CheckBox ID="chkAlloAnamnesis" runat="server" Text="Alloanamnesis / Heteroanamnesis"
                                                Checked="false" />
                                        </td>
                                    </table>
                                </td>
                            </tr>                                              
                            <tr>
                                <td>
                                   <label class="lblMandatory">
                                    <%=GetLabel("Triage") %></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox runat="server" ID="cboTriage" ClientInstanceName="cboTriage" Width="100%" />
                                </td>
                            </tr> 
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink" id="lblDiagnose" runat="server">
                                        <%=GetLabel("Diagnosis")%></label>
                                </td>
                                <td>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col width="80px" />
                                            <col width="3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <input type="hidden" id="hdnBPJSDiagnoseCode" value="" runat="server" />
                                                <asp:TextBox ID="txtDiagnoseCode" Width="100%" runat="server" />
                                            </td>
                                            <td />
                                            <td>
                                                <asp:TextBox ID="txtDiagnoseName" Width="100%" ReadOnly="true" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%:GetLabel("Diagnosa Text")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDiagnoseText" Width="100%" runat="server" />
                                </td>
                            </tr>                                                                                                                                                                             
                        </table>
                    </td>
                    <td style="vertical-align:top">
                        <h4 class="h4expanded">
                            <%=GetLabel("Rujukan dan Cara Masuk")%></h4>
                        <div class="containerTblEntryContent">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                                <colgroup>
                                    <col width="150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%:GetLabel("Rujukan")%></label>
                                    </td>
                                    <td>
                                        <dxe:ASPxComboBox ID="cboReferral" ClientInstanceName="cboReferral" Width="100%"
                                            runat="server">
                                            <ClientSideEvents ValueChanged="function(s){ onCboReferralValueChanged(s); }" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblLink" runat="server" id="lblReferralDescription">
                                            <%:GetLabel("Deskripsi Rujukan")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnReferrerID" value="" runat="server" />
                                        <input type="hidden" id="hdnReferrerParamedicID" value="" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 80px" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtReferralDescriptionCode" Width="100%" runat="server" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtReferralDescriptionName" Width="100%" runat="server" ReadOnly="true" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Alasan Kunjungan")%></label>
                                    </td>
                                    <td>
                                        <dxe:ASPxComboBox ID="cboVisitReason" ClientInstanceName="cboVisitReason" Width="100%"
                                            runat="server">
                                            <ClientSideEvents Init="function(s,e){ onCboVisitReasonValueChanged(s); }" ValueChanged="function(s,e){ onCboVisitReasonValueChanged(); }" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>    
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal" id="lblOtherVisitNotesLabel" runat="server">
                                        </label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVisitNotes" Width="100%" runat="server" />
                                    </td>
                                </tr>    
                            </table>
                        </div>
                        <h4 class="h4collapsed">
                            <%=GetLabel("Vital Signs & Indicator")%></h4>
                        <div class="containerTblEntryContent">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                                <colgroup>
                                    <col width="150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Keadaan Datang")%></label>
                                    </td>
                                    <td colspan="2">
                                        <dxe:ASPxComboBox ID="cboAdmissionCondition" ClientInstanceName="cboAdmissionCondition"
                                            Width="100%" runat="server" />
                                    </td>
                                </tr>  
                                                                                                                                                                                                                                                                                    <tr>
                                <td colspan="2">
                                    <div>
                                        <dxcp:ASPxCallbackPanel ID="cbpVitalSignView" runat="server" Width="100%" ClientInstanceName="cbpVitalSignView"
                                            ShowLoadingPanel="false" OnCallback="cbpVitalSignView_Callback">
                                            <ClientSideEvents EndCallback="function(s,e){ onCbpVitalSignViewEndCallback(s); }" />
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContent1" runat="server">
                                                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                                                        <asp:GridView ID="grdVitalSignView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                                            OnRowDataBound="grdVitalSignView_RowDataBound" >
                                                            <Columns>
                                                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                                    <ItemTemplate>
                                                                        <table cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <img class="imgEditVitalSign imgLink"
                                                                                        title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                        alt="" />
                                                                                </td>
                                                                                <td style="width: 1px">
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <img class="imgDeleteVitalSign imgLink"
                                                                                        title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                        alt="" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>    
                                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                    <HeaderTemplate>
                                                                        <div style="text-align:right">
                                                                            <span class="lblLink" id="lblAddVitalSign"><%= GetLabel("+ Tambah Tanda Vital")%></span>
                                                                        </div>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <div>
                                                                            <b>
                                                                                <%#: Eval("ObservationDateInString")%>,
                                                                                <%#: Eval("ObservationTime") %>,
                                                                                <%#: Eval("ParamedicName") %>
                                                                            </b>
                                                                        </div>
                                                                        <div>
                                                                            <asp:Repeater ID="rptVitalSignDt" runat="server">
                                                                                <ItemTemplate>
                                                                                    <div style="padding-left:20px;float:left;width:300px;" >
                                                                                        <strong><div style="width:110px;float:left;" class="labelColumn"><%#: DataBinder.Eval(Container.DataItem, "VitalSignLabel") %></div>
                                                                                        <div style="width:20px;float:left;">:</div></strong>
                                                                                        <div style="float:left;"><%#: DataBinder.Eval(Container.DataItem, "DisplayVitalSignValue") %></div>
                                                                                    </div>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate> 
                                                                                    <br style="clear:both" />
                                                                                </FooterTemplate>
                                                                            </asp:Repeater>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <EmptyDataTemplate>
                                                                <%=GetLabel("No Data To Display") %>
                                                            </EmptyDataTemplate>
                                                        </asp:GridView>
                                                    </asp:Panel>
                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dxcp:ASPxCallbackPanel>    
                                        <div class="containerPaging">
                                            <div class="wrapperPaging">
                                                <div id="vitalSignPaging"></div>
                                            </div>
                                        </div> 
                                    </div>                                
                                </td>
                            </tr> 
                            </table>
                        </div>
                        <h4 class="h4collapsed">
                            <%=GetLabel("Tindakan Awal")%></h4>
                        <div class="containerTblEntryContent">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                                <colgroup>
                                    <col width="150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                        <%=GetLabel("Perencanaan Tindakan") %> 
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPlanningNotes" runat="server" Width="100%" TextMode="Multiline"
                                            Rows="10" />
                                    </td>
                                </tr>    
                                <tr style="display:none">
                                    <td colspan="2">
                                        <div>
                                            <dxcp:ASPxCallbackPanel ID="cbpProcedureView" runat="server" Width="100%" ClientInstanceName="cbpProcedureView"
                                                ShowLoadingPanel="false" OnCallback="cbpProcedureView_Callback">
                                                <ClientSideEvents EndCallback="function(s,e){ onCbpProcedureViewEndCallback(s); }" />
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent2" runat="server">
                                                        <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGrid" Style="height:300px">
                                                            <asp:GridView ID="grdProcedureView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                <Columns>
                                                                    <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                    <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn" >
                                                                        <ItemTemplate>
                                                                            <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                                            <input type="hidden" value="<%#:Eval("ProcedureDateInDatePickerFormat") %>" bindingfield="ProcedureDate" />
                                                                            <input type="hidden" value="<%#:Eval("ProcedureTime") %>" bindingfield="ProcedureTime" />
                                                                            <input type="hidden" value="<%#:Eval("ParamedicID") %>" bindingfield="ParamedicID" />
                                                                            <input type="hidden" value="<%#:Eval("ProcedureID") %>" bindingfield="ProcedureID" />
                                                                            <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
                                                                            <input type="hidden" value="<%#:Eval("IsCreatedBySystem") %>" bindingfield="IsCreatedBySystem" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                                                        <ItemTemplate>
                                                                            <table cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <img class="imgEditProcedure imgLink"
                                                                                            title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                                            alt="" />
                                                                                    </td>
                                                                                    <td style="width: 1px">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                    <td>
                                                                                        <img class="imgDeleteProcedure imgLink"
                                                                                            title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                                            alt="" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>    
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            <div style="text-align:right">
                                                                                <span class="lblLink" id="lblAddProcedure"><%= GetLabel("+ Tambah Procedure")%></span>
                                                                            </div>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <div style="float:left;width:120px;"><%#: Eval("ProcedureDateInString")%>, <%#: Eval("ProcedureTime")%></div>
                                                                            <div style="float:left;width:300px;"><%#: Eval("ParamedicName")%></div>
                                                                            <div style="margin-left:300px;margin-top:-3px;" class="createSystem-<%#: DataBinder.Eval(Container.DataItem, "IsCreatedBySystem")%>"><input type="checkbox" disabled="disabled" checked="checked" /><div style="margin-top:-16px;margin-left:140px;"><%=GetLabel("Created By System ")%> </div></div>
                                                                            <div style="margin-left:120px; clear:both;"><%#: Eval("ProcedureName")%> (<%#: Eval("ProcedureID")%>)</div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <EmptyDataTemplate>
                                                                    <%=GetLabel("Tidak ada data tindakan") %>
                                                                </EmptyDataTemplate>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dxcp:ASPxCallbackPanel>    
                                            <div class="containerPaging">
                                                <div class="wrapperPaging">
                                                    <div id="procedurePaging"></div>
                                                </div>
                                            </div> 
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </fieldset>
        <div style="display: none">
            <dxcp:ASPxCallbackPanel ID="cbpDeleteVitalSign" runat="server" Width="100%" ClientInstanceName="cbpDeleteVitalSign"
                ShowLoadingPanel="false" OnCallback="cbpDeleteVitalSign_Callback">
                <ClientSideEvents EndCallback="function(s,e){ onCbpVitalSignDeleteEndCallback(s); }" />
            </dxcp:ASPxCallbackPanel>
        </div>
    </div>
</asp:Content>