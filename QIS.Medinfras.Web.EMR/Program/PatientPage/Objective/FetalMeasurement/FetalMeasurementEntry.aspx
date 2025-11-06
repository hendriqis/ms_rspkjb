<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MPPatientPageList.master"
    AutoEventWireup="true" CodeBehind="FetalMeasurementEntry.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.FetalMeasurementEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnAddFetus" runat="server" crudmode="R" visible="false">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbnew.png")%>' alt="" /><div>
            <%=GetLabel("Add Fetus")%></div>
    </li>
    <li id="btnPatientStatusSave" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><div>
            <%=GetLabel("Save")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" id="dxss_erpatientstatus">
        $(function () {
            setDatePicker('<%=txtMeasurementDate.ClientID %>');
            $('#<%=txtMeasurementDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%=btnPatientStatusSave.ClientID %>').click(function (evt) {
                if (IsValid(evt, 'fsPatientStatus', 'mpPatientStatus'))
                    onCustomButtonClick('save');
            });

            $('#<%=txtPregnancyWeek.ClientID %>').focus();
        });


        function onAfterCustomClickSuccess(type, retval) {
            if ($('#<%=hdnID.ClientID %>').val() == '')
                $('#<%=hdnID.ClientID %>').val(retval);
        }

        $('#<%=btnPatientStatusSave.ClientID %>').click(function () {
            if (IsValid(null, 'fsPatientStatus', 'mpPatientStatus'))
                onCustomButtonClick('save');
        });

        $('#<%=btnAddFetus.ClientID %>').click(function () {
            var url = ResolveUrl("~/Program/PatientPage/Objective/FetalMeasurement/AddFetusCtl.ascx");
            openUserControlPopup(url, '', 'Fetal Measurement - Add Fetus', 900, 600);
        });
    </script>
    <div>
        <input type="hidden" runat="server" id="hdnID" value="" />
        <input type="hidden" runat="server" id="hdnPregnancyNo" value="" />
        <input type="hidden" runat="server" id="hdnTimeElapsed0hour" value="00" />
        <input type="hidden" runat="server" id="hdnTimeElapsed0minute" value="00" />
        <input type="hidden" runat="server" id="hdnTimeElapsed1hour" value="00" />
        <input type="hidden" runat="server" id="hdnTimeElapsed1minute" value="00" />
        <input type="hidden" runat="server" id="hdnDepartmentID" value="" />
        <fieldset id="mpPatientStatus">
            <table class="tblContentArea">
                <colgroup>
                    <col style="width: 40%" />
                </colgroup>
                <tr>
                    <td style="padding: 5px; vertical-align: top;">
                        <table class="tblEntryContent" style="width: 100%">
                            <colgroup>
                                <col style="width: 180px" />
                                <col style="width: 150px" />
                                <col style="width: 80px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel" valign="top">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Date - Time")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMeasurementDate" Width="120px" CssClass="datepicker" runat="server" />
                                </td>
                                <td style="padding-left:5px">
                                    <asp:TextBox ID="txtMeasurementTime" Width="100%" CssClass="time" runat="server"/>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" valign="top">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Gestational Age")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPregnancyWeek" Width="80px" CssClass="number" runat="server"/> Weeks
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" valign="top">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Fetus No.")%></label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlFetusNo" runat="server" Width="84px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" valign="top">
                                    <label class="lblNormal">
                                        <%=GetLabel("Biparietal Diameter (BPD)")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBPD" Width="120px" CssClass="number" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" valign="top">
                                    <label class="lblNormal">
                                        <%=GetLabel("Abdominal Circumference (AC)")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAC" Width="120px" CssClass="number" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" valign="top">
                                    <label class="lblNormal">
                                        <%=GetLabel("Estimated Fetal Weight (EFW)")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEFW" Width="120px" CssClass="number" runat="server" />
                                </td>
                            </tr>   
                            <tr>
                                <td class="tdLabel" valign="top">
                                    <label class="lblNormal">
                                        <%=GetLabel("Head Circumference (HC)")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtHC" Width="120px" CssClass="number" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" valign="top">
                                    <label class="lblNormal">
                                        <%=GetLabel("Femur Length (FL)")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFL" Width="120px" CssClass="number" runat="server" />
                                </td>
                            </tr>      
                            <tr>
                                <td class="tdLabel" valign="top">
                                    <label class="lblNormal">
                                        <%=GetLabel("Humerus Length (HL)")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtHL" Width="120px" CssClass="number" runat="server" />
                                </td>
                            </tr> 
                            <tr>
                                <td class="tdLabel" valign="top">
                                    <label class="lblNormal">
                                        <%=GetLabel("Crown Crump Length (CRL)")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCRL" Width="120px" CssClass="number" runat="server" />
                                </td>
                            </tr>       
                            <tr>
                                <td class="tdLabel" valign="top">
                                    <label class="lblNormal">
                                        <%=GetLabel("Gestational Sac (GS)")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtGS" Width="120px" CssClass="number" runat="server" />
                                </td>
                            </tr>   
                            <tr>
                                <td class="tdLabel" valign="top">
                                    <label class="lblNormal">
                                        <%=GetLabel("Fetal Heart Rate (FHR)")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFHR" Width="120px" CssClass="number" runat="server" />
                                </td>
                            </tr>     
                            <tr>
                                <td class="tdLabel" valign="top">
                                    <label class="lblNormal">
                                        <%=GetLabel("Occipitofrontal Diameter (OFD)")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtOFD" Width="120px" CssClass="number" runat="server" />
                                </td>
                            </tr>                                                                                                                                          
                        </table>
                    </td>
                    <td valign="top">
                        <table id="tblRemarks" style="width:100%" runat="server">
                            <colgroup>
                                <col width="150px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel" valign="top">
                                </td>
                                <td>
                                </td>
                            </tr>    
                        </table>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
</asp:Content>