<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master" AutoEventWireup="true" 
    CodeBehind="MedicationOrderDetail.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PrescriptionOrderDetail" %>

<%@ Register Src="~/Libs/Controls/PatientBannerCtl.ascx" TagName="PatientBannerCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnPrescriptionOrderBack" runat="server" CRUDMode="R"><img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div><%=GetLabel("Back")%></div></li>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">   
    <input type="hidden" value="" id="hdnGCRegistrationStatus" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnClassID" runat="server" />        
    <uc1:PatientBannerCtl ID="ctlPatientBanner" runat="server" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">   
    <script type="text/javascript">
        function onLoad() {
            setDatePicker('<%=txtPrescriptionDate.ClientID %>');

            $('#<%=btnPrescriptionOrderBack.ClientID %>').click(function () {
                showLoadingPanel();
                document.location = ResolveUrl('~/Program/PatientList/VisitList.aspx?id=mo');
            });

            //#region Transaction No
            $('#lblPrescriptionOrderNo.lblLink').click(function () {
                var filterExpression = 'VisitID = ' + $('#<%=hdnVisitID.ClientID %>').val();
                openSearchDialog('prescriptionorderhd', filterExpression, function (value) {
                    $('#<%=txtPrescriptionOrderNo.ClientID %>').val(value);
                    onTxtPrescriptionOrderNoChanged(value);
                });
            });

            $('#<%=txtPrescriptionOrderNo.ClientID %>').change(function () {
                onTxtPrescriptionOrderNoChanged($(this).val());
            });

            function onTxtPrescriptionOrderNoChanged(value) {
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

        }
    </script> 
    <input type="hidden" value="" id="hdnPrescriptionOrderID" runat="server" />  
    <div style="height:435px;overflow-y:auto;overflow-x:hidden;">
        <div class="pageTitle">
            <div style="font-size: 1.1em"><%=GetLabel("Order Farmasi")%></div>
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
                            <td class="tdLabel"><label class="lblLink" id="lblPrescriptionOrderNo"><%=GetLabel("No Order")%></label></td>
                            <td><asp:TextBox ID="txtPrescriptionOrderNo" Width="150px" ReadOnly="true" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><%=GetLabel("Tanggal") %> - <%=GetLabel("Jam") %></td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="padding-right: 1px;width:145px"><asp:TextBox ID="txtPrescriptionDate" Width="120px" CssClass="datepicker" runat="server" /></td>
                                        <td style="width:5px">&nbsp;</td>
                                        <td><asp:TextBox ID="txtPrescriptionTime" Width="80px" CssClass="time" runat="server" Style="text-align:center" /></td>
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
                        <tr>
                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Refill Instruction") %></label></td>
                            <td><dxe:ASPxComboBox ID="cboRefillInstruction" ClientInstanceName="cboRefillInstruction" Width="200px" runat="server" /></td>
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
                            <td class="tdLabel" valign="top" style="padding-top:5px;"><label><%=GetLabel("Catatan Resep")%></label></td>
                            <td><asp:TextBox ID="txtNotes" Width="600px" TextMode="MultiLine" Height="150px" runat="server" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>  
    </div>
</asp:Content>