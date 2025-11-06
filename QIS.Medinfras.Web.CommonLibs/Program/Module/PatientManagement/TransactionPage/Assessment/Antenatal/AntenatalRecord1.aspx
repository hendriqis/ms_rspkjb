<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="AntenatalRecord1.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.AntenatalRecord1" %>

<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
 <asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnPatientStatusSave" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><div>
            <%=GetLabel("Save")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" id="dxss_erpatientstatus">
        $(function () {
            setDatePicker('<%=txtLMP.ClientID %>');
            setDatePicker('<%=txtEDB.ClientID %>');
            $('#<%=txtLMP.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
            $('#<%=txtEDB.ClientID %>').datepicker('option', 'minDate', '0');

            $('#<%=btnPatientStatusSave.ClientID %>').click(function (evt) {
                if (IsValid(evt, 'fsPatientStatus', 'mpPatientStatus'))
                    onCustomButtonClick('save');
            });

            $('#<%=txtLMP.ClientID %>').change(function () {
                onLMPDateChange();
            });

            $('#<%=txtPregnancyNo.ClientID %>').focus();
        });


        function onLMPDateChange() {
            var _lmpDate = new Date(Methods.getDatePickerDate($('#<%=txtLMP.ClientID %>').val()));
            var _edbDate = new Date(_lmpDate.setDate(_lmpDate.getDate() + 280));
            $('#<%=txtEDB.ClientID %>').val(Methods.dateToDatePickerFormat(_edbDate));
        }

        function onAfterCustomClickSuccess(type, retval) {
            if ($('#<%=hdnID.ClientID %>').val() == '')
                $('#<%=hdnID.ClientID %>').val(retval);
        }

        $('#<%=btnPatientStatusSave.ClientID %>').click(function () {
            if (IsValid(null, 'fsPatientStatus', 'mpPatientStatus'))
                onCustomButtonClick('save');
        });
    </script>
    <div>
        <input type="hidden" value="" id="hdnPageTitle" runat="server" />
        <input type="hidden" runat="server" id="hdnID" value="" />
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
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel" valign="top">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Multipregnancy No.")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPregnancyNo" Width="120px" CssClass="number" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" valign="top">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Menstrual History (LMP)")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLMP" Width="120px" CssClass="datepicker" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" valign="top">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Estimated Delivery Date")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEDB" Width="120px" CssClass="datepicker" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" valign="top">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Gravida")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtGravida" Width="120px" CssClass="number" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" valign="top">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Para")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPara" Width="120px" CssClass="number" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" valign="top">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Abortion")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAbortion" Width="120px" CssClass="number" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" valign="top">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Life")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLife" Width="120px" CssClass="number" runat="server" />
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
                                    <label class="lblNormal">
                                        <%=GetLabel("Menstrual History Remarks")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMenstrualHistory" runat="server" TextMode="MultiLine" Rows="3"
                                        Width="100%" />
                                </td>
                            </tr>    
                            <tr>
                                <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                    <%=GetLabel("Previous Medical History Remarks") %>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMedicalHistory" runat="server" Width="100%" TextMode="Multiline"
                                        Rows="5" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </fieldset>
    </div>
</asp:Content>