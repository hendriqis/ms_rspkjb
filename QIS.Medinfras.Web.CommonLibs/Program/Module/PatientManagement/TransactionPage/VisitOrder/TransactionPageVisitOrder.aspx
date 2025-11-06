<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx2.master" AutoEventWireup="true" 
    CodeBehind="TransactionPageVisitOrder.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.TransactionPageVisitOrder" %>

<%@ Register Src="~/Libs/Controls/PatientBannerCtl.ascx" TagName="PatientBannerCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx" TagName="ToolbarCtl" TagPrefix="uc1" %>

<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane" runat="server">   
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">   
    <script type="text/javascript" id="dxss_patientvisitctl">
        $(function () {
            $('#lblPatientVisitAddData').click(function () {
                $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val('');
                $('#<%=txtClinicCode.ClientID %>').val('');
                $('#<%=txtClinicName.ClientID %>').val('');
                $('#<%=hdnPhysicianID.ClientID %>').val('');
                $('#<%=txtPhysicianCode.ClientID %>').val('');
                $('#<%=txtPhysicianName.ClientID %>').val('');
                $('#<%=hdnVisitTypeID.ClientID %>').val('');
                $('#<%=txtVisitTypeCode.ClientID %>').val('');
                $('#<%=txtVisitTypeName.ClientID %>').val('');
                $('#<%=txtVisitReason.ClientID %>').val('');
                cboRegistrationEditSpecialty.SetValue('');

                $('#containerPatientVisitEntryData').show();
            });

            $('#btnPatientVisitCancel').click(function () {
                $('#containerPatientVisitEntryData').hide();
            });

            $('#btnPatientVisitSave').click(function (evt) {
                if (IsValid(evt, 'fsPatientVisit', 'mpPatientVisit'))
                    cbpPatientVisitTransHd.PerformCallback('save');
                return false;
            });
        });

        //#region Physician
        function onGetPatientVisitParamedicFilterExpression() {
            var polyclinicID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
            var filterExpression = 'IsDeleted = 0';
            if (polyclinicID != '')
                filterExpression = "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = " + polyclinicID + ")";
            return filterExpression;
        }

        $('#lblPatientVisitPhysician.lblLink').live('click', function () {
            openSearchDialog('paramedic', onGetPatientVisitParamedicFilterExpression(), function (value) {
                $('#<%=txtPhysicianCode.ClientID %>').val(value);
                onTxtPatientVisitPhysicianCodeChanged(value);
            });
        });

        $('#<%=txtPhysicianCode.ClientID %>').live('change', function () {
            onTxtPatientVisitPhysicianCodeChanged($(this).val());
        });

        function onTxtPatientVisitPhysicianCodeChanged(value) {
            var filterExpression = onGetPatientVisitParamedicFilterExpression() + " AND ParamedicCode = '" + value + "'";
            Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                if (result != null) {
                    cboRegistrationEditSpecialty.SetValue(result.SpecialtyID);
                    $('#<%=hdnPhysicianID.ClientID %>').val(result.ParamedicID);
                    $('#<%=txtPhysicianName.ClientID %>').val(result.ParamedicName);
                }
                else {
                    cboRegistrationEditSpecialty.SetValue('');
                    $('#<%=hdnPhysicianID.ClientID %>').val('');
                    $('#<%=txtPhysicianCode.ClientID %>').val('');
                    $('#<%=txtPhysicianName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Clinic
        function onGetPatientVisitClinicFilterExpression() {
            var filterExpression = "HealthcareID = '" + AppSession.healthcareID + "' AND DepartmentID = '" + Constant.Facility.OUTPATIENT + "' AND IsDeleted = 0";
            var paramedicID = $('#<%=hdnPhysicianID.ClientID %>').val();
            if (paramedicID != '')
                filterExpression += ' AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM ServiceUnitParamedic WHERE ParamedicID = ' + paramedicID + ')';
            return filterExpression;
        }

        $('#lblPatientVisitClinic.lblLink').live('click', function () {
            openSearchDialog('serviceunitparamedicvisittypeperhealthcare', onGetPatientVisitClinicFilterExpression(), function (value) {
                $('#<%=txtClinicCode.ClientID %>').val(value);
                onTxtPatientVisitClinicCodeChanged(value);
            });
        });

        $('#<%=txtClinicCode.ClientID %>').live('change', function () {
            onTxtPatientVisitClinicCodeChanged($(this).val());
        });

        function onTxtPatientVisitClinicCodeChanged(value) {
            var filterExpression = onGetPatientVisitClinicFilterExpression() + " AND ServiceUnitCode = '" + value + "'";
            Methods.getObject('GetvHealthcareServiceUnitList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                    $('#<%=txtClinicName.ClientID %>').val(result.ServiceUnitName);
                }
                else {
                    $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val('');
                    $('#<%=txtClinicCode.ClientID %>').val('');
                    $('#<%=txtClinicName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Visit Type
        function onGetPatientVisitVisitTypeFilterExpression() {
            var serviceUnitID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
            if (serviceUnitID == '')
                serviceUnitID = '0';
            var paramedicID = $('#<%=hdnPhysicianID.ClientID %>').val();
            if (paramedicID == '')
                paramedicID = '0';
            var filterExpression = serviceUnitID + ';' + paramedicID + ';';
            return filterExpression;
        }

        $('#<%=lblVisitType.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('paramedicvisittype', onGetPatientVisitVisitTypeFilterExpression(), function (value) {
                $('#<%=txtVisitTypeCode.ClientID %>').val(value);
                onTxtPatientVisitVisitTypeCodeChanged(value);
            });
        });

        $('#<%=txtVisitTypeCode.ClientID %>').live('change', function () {
            onTxtPatientVisitVisitTypeCodeChanged($(this).val());
        });

        function onTxtPatientVisitVisitTypeCodeChanged(value) {
            var filterExpression = onGetPatientVisitVisitTypeFilterExpression() + "VisitTypeCode = '" + value + "'";
            Methods.getObject('GetParamedicVisitTypeAccessList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnVisitTypeID.ClientID %>').val(result.VisitTypeID);
                    $('#<%=txtVisitTypeName.ClientID %>').val(result.VisitTypeName);
                }
                else {
                    $('#<%=hdnVisitTypeID.ClientID %>').val('');
                    $('#<%=txtVisitTypeCode.ClientID %>').val('');
                    $('#<%=txtVisitTypeName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        $('.imgPatientVisitDelete.imgLink').die('click');
        $('.imgPatientVisitDelete.imgLink').live('click', function () {
            if (confirm("Are You Sure Want To Delete This Data?")) {
                $row = $(this).parent().parent();
                var id = $row.find('.hdnVisitID').val();
                var serviceUnitID = $row.find('.hdnServiceUnitID').val();
                $('#<%=hdnVisitID.ClientID %>').val(id);
                $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val(serviceUnitID);
                cbpPatientVisitTransHd.PerformCallback('delete');
            }
        });

        $('.imgPatientVisitEdit.imgLink').die('click');
        $('.imgPatientVisitEdit.imgLink').live('click', function () {
            $row = $(this).parent().parent();

            var visitID = $row.find('.hdnVisitID').val();
            var serviceUnitID = $row.find('.hdnServiceUnitID').val();
            var paramedicID = $row.find('.hdnParamedicID').val();
            var serviceUnitCode = $row.find('.hdnServiceUnitCode').val();
            var paramedicCode = $row.find('.hdnParamedicCode').val();
            var serviceUnitName = $row.find('.divServiceUnitName').html();
            var paramedicName = $row.find('.divParamedicName').html();

            var visitTypeID = $row.find('.hdnVisitTypeID').val();
            var visitTypeCode = $row.find('.hdnVisitTypeCode').val();
            var visitTypeName = $row.find('.divVisitTypeName').html();
            var specialtyID = $row.find('.hdnSpecialtyID').val();
            var visitReason = $row.find('.hdnVisitReason').val();

            $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val(serviceUnitID);
            $('#<%=txtClinicCode.ClientID %>').val(serviceUnitCode);
            $('#<%=txtClinicName.ClientID %>').val(serviceUnitName);
            $('#<%=hdnPhysicianID.ClientID %>').val(paramedicID);
            $('#<%=txtPhysicianCode.ClientID %>').val(paramedicCode);
            $('#<%=txtPhysicianName.ClientID %>').val(paramedicName);
            $('#<%=hdnVisitTypeID.ClientID %>').val(visitTypeID);
            $('#<%=txtVisitTypeCode.ClientID %>').val(visitTypeCode);
            $('#<%=txtVisitTypeName.ClientID %>').val(visitTypeName);
            $('#<%=txtVisitReason.ClientID %>').val(visitReason);
            $('#<%=hdnVisitID.ClientID %>').val(visitID);
            cboRegistrationEditSpecialty.SetValue(specialtyID);

            $('#containerPatientVisitEntryData').show();
        });

        function onCbpPatientVisitTransHdEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'save') {
                if (param[1] == 'fail')
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                else
                    $('#containerPatientVisitEntryData').hide();
            }
            else if (param[0] == 'delete') {
                if (param[1] == 'fail')
                    showToast('Delete Failed', 'Error Message : ' + param[2]);
            }
            hideLoadingPanel();
        }
    </script>

    <div style="height:440px; overflow-y:auto">
        <input type="hidden" value="" runat="server" id="hdnRegistrationID" />
        <input type="hidden" value="" runat="server" id="hdnClassID" />
        <input type="hidden" value="" runat="server" id="hdnBusinessPartnerID" />
        <input type="hidden" value="" runat="server" id="hdnItemCardFee" />
        <div class="pageTitle"><%=GetLabel("Order Poli Lain")%></div>
        <table class="tblContentArea">
            <colgroup>
                <col style="width:100%"/>
            </colgroup>
            <tr>            
                <td style="padding:5px;vertical-align:top">
                    <div id="containerPatientVisitEntryData" style="margin-top:10px;display:none;">
                        <div class="pageTitle"><%=GetLabel("Entry Kunjungan")%></div>
                        <input type="hidden" id="hdnVisitID" runat="server" value="" />
                        <fieldset id="fsPatientVisit" style="margin:0"> 
                            <table class="tblEntryDetail" style="width:100%">
                                <colgroup>
                                    <col style="width:150px"/>
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel"><label class="lblLink lblMandatory" id="lblPatientVisitClinic"><%=GetLabel("Klinik")%></label></td>
                                    <td>
                                        <input type="hidden" id="hdnHealthcareServiceUnitID" runat="server" value="" />
                                        <table style="width:100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:30%"/>
                                                <col style="width:3px"/>
                                                <col/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox ID="txtClinicCode" CssClass="required" Width="100%" runat="server" /></td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox ID="txtClinicName" ReadOnly="true" Width="100%" runat="server" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblLink lblMandatory" id="lblPatientVisitPhysician"><%=GetLabel("Dokter / Paramedis")%></label></td>
                                    <td>
                                        <input type="hidden" id="hdnPhysicianID" value="" runat="server" />
                                        <table style="width:100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:30%"/>
                                                <col style="width:3px"/>
                                                <col/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox ID="txtPhysicianCode" CssClass="required" Width="100%" runat="server" /></td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox ID="txtPhysicianName" ReadOnly="true" Width="100%" runat="server" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Spesialisasi")%></label></td>
                                    <td><dxe:ASPxComboBox ID="cboRegistrationEditSpecialty" ClientInstanceName="cboRegistrationEditSpecialty" Width="100%" runat="server" /></td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblLink lblMandatory" runat="server" id="lblVisitType"><%=GetLabel("Jenis Kunjungan")%></label></td>
                                    <td>
                                        <input type="hidden" id="hdnVisitTypeID" value="" runat="server" />
                                        <table style="width:100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width:30%"/>
                                                <col style="width:3px"/>
                                                <col/>
                                            </colgroup>
                                            <tr>
                                                <td><asp:TextBox ID="txtVisitTypeCode" Width="100%" runat="server" /></td>
                                                <td>&nbsp;</td>
                                                <td><asp:TextBox ID="txtVisitTypeName" ReadOnly="true" Width="100%" runat="server" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Alasan Kunjungan")%></label></td>
                                    <td><asp:TextBox ID="txtVisitReason" Width="100%" runat="server" /></td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <table>
                                            <tr>
                                                <td>
                                                    <input type="button" id="btnPatientVisitSave" value='<%= GetLabel("Save")%>' />
                                                </td>
                                                <td>
                                                    <input type="button" id="btnPatientVisitCancel" value='<%= GetLabel("Cancel")%>' />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>

                    <dxcp:ASPxCallbackPanel ID="cbpPatientVisitTransHd" runat="server" Width="100%" ClientInstanceName="cbpPatientVisitTransHd"
                        ShowLoadingPanel="false" OnCallback="cbpPatientVisitTransHd_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                            EndCallback="function(s,e){ onCbpPatientVisitTransHdEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlPatientVisitTransHdGrdView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                                    <asp:GridView ID="grdPatientVisitTransHd" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="50px">
                                                <ItemTemplate>
                                                    <%--<img class='imgPatientVisitEdit <%#: IsAllowEditPatientVisit.ToString() == "False" || Eval("IsMainVisit").ToString() == "True" ? "imgDisabled" : "imgLink"%>' title='<%=GetLabel("Edit")%>' 
                                                        src='<%# IsAllowEditPatientVisit.ToString() == "False" || Eval("IsMainVisit").ToString() == "True" ? ResolveUrl("~/Libs/Images/Button/edit_disabled.png") : ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt="" style="float:left;margin-right: 2px;" />--%>
                                                    <img class='imgPatientVisitDelete <%#: IsAllowEditPatientVisit.ToString() == "False" || Eval("IsMainVisit").ToString() == "True" ? "imgDisabled" : "imgLink"%>' title='<%=GetLabel("Delete")%>' 
                                                        src='<%# IsAllowEditPatientVisit.ToString() == "False" || Eval("IsMainVisit").ToString() == "True" ? ResolveUrl("~/Libs/Images/Button/delete_disabled.png") : ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="200px" ItemStyle-VerticalAlign="Top">
                                                <HeaderTemplate>
                                                    <div style="text-align:left;padding-left:3px">
                                                        <%=GetLabel("No Registrasi")%>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div class="divVisitNo"><%#: Eval("RegistrationNo") %></div> 
                                                    <input type="hidden" class="hdnVisitID" value="<%#: Eval("VisitID")%>" />  
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="300px">
                                                <HeaderTemplate>
                                                    <div style="text-align:left;padding-left:3px">
                                                        <%=GetLabel("Informasi Kunjungan")%>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <input type="hidden" class="hdnServiceUnitID" value="<%#: Eval("HealthcareServiceUnitID")%>" />
                                                    <input type="hidden" class="hdnParamedicID" value="<%#: Eval("ParamedicID")%>" />
                                                    <input type="hidden" class="hdnServiceUnitCode" value="<%#: Eval("ServiceUnitCode")%>" />
                                                    <input type="hidden" class="hdnParamedicCode" value="<%#: Eval("ParamedicCode")%>" />
                                                    <input type="hidden" class="hdnSpecialtyID" value="<%#: Eval("SpecialtyID")%>" />
                                                    <input type="hidden" class="hdnVisitTypeID" value="<%#: Eval("VisitTypeID")%>" />
                                                    <input type="hidden" class="hdnVisitTypeCode" value="<%#: Eval("VisitTypeCode")%>" />
                                                    <input type="hidden" class="hdnVisitReason" value="<%#: Eval("VisitReason")%>" />
                                                    <div style="float:left;width:100px;" class="divServiceUnitName"><%#: Eval("ServiceUnitName")%></div> 
                                                    <div class="divVisitTypeName"><%#: Eval("VisitTypeName")%></div>  
                                                    <div class="divParamedicName"><%#: Eval("ParamedicName")%></div>   
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="100px" ItemStyle-VerticalAlign="Top">
                                                <HeaderTemplate>
                                                    <div style="text-align:right;padding-left:3px">
                                                        <%=GetLabel("No Antrian")%>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="text-align:right;"><%#: Eval("QueueNo") %></div> 
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <div class="imgLoadingGrdView" id="containerImgLoadingView">
                                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                    </div>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div style="width:100%;text-align:center" id="divContainerAddData" runat="server">
                        <span class="lblLink" id="lblPatientVisitAddData"><%= GetLabel("Add Data")%></span>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>