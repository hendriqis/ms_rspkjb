<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master" AutoEventWireup="true" 
    CodeBehind="ServiceOrderEntry.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.ServiceOrderEntry" %>

<%@ Register Src="~/Libs/Controls/PatientBannerCtl.ascx" TagName="PatientBannerCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnServiceOrderBack" runat="server" CRUDMode="R"><img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div><%=GetLabel("Back")%></div></li>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">   
    <input type="hidden" value="" id="hdnGCRegistrationStatus" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />   
    <input type="hidden" value="" id="hdnClassID" runat="server" /> 
    <input type="hidden" value="" id="hdnDefaultParamedicID" runat="server" /> 
    <input type="hidden" value="" id="hdnDefaultParamedicCode" runat="server" /> 
    <input type="hidden" value="" id="hdnDefaultParamedicName" runat="server" /> 
    <input type="hidden" value="" id="hdnCode" runat="server" />
    <uc1:PatientBannerCtl ID="ctlPatientBanner" runat="server" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">   
    <script type="text/javascript">
        function onLoad() {
            setDatePicker('<%=txtServiceOrderDate.ClientID %>');

            $('#<%=btnServiceOrderBack.ClientID %>').click(function () {
                showLoadingPanel();
                document.location = ResolveUrl('~/Program/PatientList/VisitList.aspx?id=mo');
            });

            //#region Transaction No
            $('#lblServiceOrderNo.lblLink').click(function () {
                var filterExpression = 'VisitID = ' + $('#<%=hdnVisitID.ClientID %>').val();
                openSearchDialog('serviceorderhd', filterExpression, function (value) {
                    $('#<%=txtServiceOrderNo.ClientID %>').val(value);
                    onTxtServiceOrderNoChanged(value);
                });
            });

            $('#<%=txtServiceOrderNo.ClientID %>').change(function () {
                onTxtServiceOrderNoChanged($(this).val());
            });

            function onTxtServiceOrderNoChanged(value) {
                onLoadObject(value);
            }
            //#endregion

            //#region Physician
            $('#<%=lblPhysician.ClientID %>.lblLink').click(function () {
                var filterExpression = 'IsDeleted = 0';
                openSearchDialog('paramedic', filterExpression, function (value) {
                    $('#<%=txtPhysicianCode.ClientID %>').val(value);
                    onTxtPhysicianCodeChanged(value);
                });
            });

            $('#<%=txtPhysicianCode.ClientID %>').change(function () {
                onTxtPhysicianCodeChanged($(this).val());
            });

            function onTxtPhysicianCodeChanged(value) {
                var filterExpression = "ParamedicCode = '" + value + "' AND IsDeleted = 0";
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

            //#region Service Unit
            function getServiceUnitFilterFilterExpression() {
                var filterExpression = "HealthcareID = '" + AppSession.healthcareID + "' AND DepartmentID = 'OUTPATIENT'";
                return filterExpression;
            }
            $('#<%=lblServiceUnit.ClientID %>.lblLink').live('click', function () {
                openSearchDialog('serviceunitperhealthcare', getServiceUnitFilterFilterExpression(), function (value) {
                    $('#<%=txtServiceUnitCode.ClientID %>').val(value);
                    onTxtServiceUnitCodeChanged(value);
                });
            });

            $('#<%=txtServiceUnitCode.ClientID %>').live('change', function () {
                onTxtServiceUnitCodeChanged($(this).val());
            });

            function onTxtServiceUnitCodeChanged(value) {
                var filterExpression = getServiceUnitFilterFilterExpression() + " AND ServiceUnitCode = '" + value + "'";
                Methods.getObject('GetvHealthcareServiceUnitList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                        $('#<%=hdnServiceUnitID.ClientID %>').val(result.ServiceUnitID);
                        $('#<%=txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);
                    }
                    else {
                        $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val('');
                        $('#<%=hdnServiceUnitID.ClientID %>').val('');
                        $('#<%=txtServiceUnitCode.ClientID %>').val('');
                        $('#<%=txtServiceUnitName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

        }
    </script> 
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnServiceOrderID" runat="server" />  
    <div style="height:435px;overflow-y:auto;overflow-x:hidden;">
        <div class="pageTitle">
            <div style="font-size: 1.1em"><%=GetPageTitle()%></div>
        </div>
        <table class="tblContentArea">
            <colgroup>
                <col style="width:50%"/>
                <col style="width:50%"/>
            </colgroup>
            <tr>
                <td style="padding:5px;vertical-align:top">
                    <table class="tblEntryContent" style="width:100%">
                        <colgroup>
                            <col style="width:150px"/>
                            <col/>
                        </colgroup>
                        <tr>
                            <td class="tdLabel"><label class="lblLink" id="lblServiceOrderNo"><%=GetLabel("No Order")%></label></td>
                            <td><asp:TextBox ID="txtServiceOrderNo" Width="150px" ReadOnly="true" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("No Referensi")%></label></td>
                            <td><asp:TextBox ID="txtReferenceNo" Width="150px" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><%=GetLabel("Tanggal") %> - <%=GetLabel("Jam") %></td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="padding-right: 1px;width:145px"><asp:TextBox ID="txtServiceOrderDate" Width="120px" CssClass="datepicker" runat="server" /></td>
                                        <td style="width:5px">&nbsp;</td>
                                        <td><asp:TextBox ID="txtServiceOrderTime" Width="80px" CssClass="time" runat="server" Style="text-align:center" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="padding:5px;vertical-align:top">
                    <table class="tblEntryContent" style="width:100%">
                        <colgroup>
                            <col style="width:150px"/>
                            <col/>
                        </colgroup>
                        <tr>
                            <td class="tdLabel"><label class="lblLink lblMandatory" runat="server" id="lblPhysician"><%=GetLabel("Dokter / Paramedis")%></label></td>
                            <td>
                                <input type="hidden" id="hdnPhysicianID" value="" runat="server" />
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width:30%"/>
                                        <col style="width:3px"/>
                                        <col/>
                                    </colgroup>
                                    <tr>
                                        <td><asp:TextBox ID="txtPhysicianCode" Width="100%" runat="server" /></td>
                                        <td>&nbsp;</td>
                                        <td><asp:TextBox ID="txtPhysicianName" Width="100%" runat="server" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr runat="server" id="trServiceUnit">
                            <td class="tdLabel"><label class="lblLink lblMandatory" runat="server" id="lblServiceUnit"><%=GetLabel("Unit Pelayanan")%></label></td>
                            <td>
                               <input type="hidden" id="hdnHealthcareServiceUnitID" value="" runat="server" />
                               <input type="hidden" id="hdnServiceUnitID" value="" runat="server" />
                               <table style="width:100%" cellpadding="0" cellspacing="0">
                                   <colgroup>
                                   <col style="width:30%"/>
                                   <col style="width:3px"/>
                                   <col/>
                                   </colgroup>
                                   <tr>
                                       <td><asp:TextBox ID="txtServiceUnitCode" Width="100%" runat="server" /></td>
                                       <td>&nbsp;</td>
                                       <td><asp:TextBox ID="txtServiceUnitName" Width="100%" runat="server" /></td>
                                  </tr>
                              </table>
                            </td>                        
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="padding-left:5px;">
                    <table class="tblEntryContent" style="width:100%">
                        <colgroup>
                            <col style="width:150px"/>
                            <col/>
                        </colgroup>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top:5px;"><label class="lblMandatory"><%=GetLabel("Catatan")%></label></td>
                            <td><asp:TextBox ID="txtNotes" Width="600px" TextMode="MultiLine" Height="150px" runat="server" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>  
    </div>
</asp:Content>