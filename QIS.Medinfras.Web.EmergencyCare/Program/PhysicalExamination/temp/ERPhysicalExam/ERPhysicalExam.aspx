<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="ERPhysicalExam.aspx.cs" Inherits="QIS.Medinfras.Web.EmergencyCare.Program.ERPhysicalExam" %>

<%@ Register Src="~/Program/PhysicalExamination/PhysicalExaminationToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnPhysicalExamSave" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><div>
            <%=GetLabel("Save")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" id="dxss_chiefcomplaintentryctl">
        $(function () {
            setDatePicker('<%=txtObservationDate.ClientID %>');
            $('#<%=txtObservationDate.ClientID %>').datepicker('option', 'maxDate', '0');
        });

        $('#<%=btnPhysicalExamSave.ClientID %>').click(function () {
            if (IsValid(null, 'fsPhysicalExam', 'mpPhysicalExam'))
                onCustomButtonClick('save');
        });

        $(function () {
            $(".rblReviewOfSystem input").change(function () {
                $txt = $(this).closest('tr').parent().closest('tr').find('.txtFreeText');
                if ($(this).val() == '3')
                    $txt.show();
                else
                    $txt.hide();
            });
            $(".rblReviewOfSystem").each(function () {
                $(this).find('input[checked=checked]').change();
            });

            $("#<%=rblCheckAll.ClientID %> input").change(function () {
                var value = $(this).val();
                $(".rblReviewOfSystem").each(function () {
                    $(this).find('input[value=' + value + ']').prop('checked', true).change();
                });
            });

        });
    </script>
    <div>
        <input type="hidden" runat="server" id="hdnID" value="" />
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 450px" />
                <col />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top;">
                    <fieldset id="fsPhisycalExam">
                        <table class="tblEntryContent" style="width: 100%">
                            <colgroup>
                                <col style="width: 150px" />
                                <col style="width: 123px" />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Waktu Pemeriksaan")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtObservationDate" Width="100px" CssClass="datepicker" runat="server" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtObservationTime" Width="80px" CssClass="time" runat="server"
                                        Style="text-align: center" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
                <td>
                    <asp:RadioButtonList ID="rblCheckAll" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Semua Tidak Diperiksa" Value="1" />
                        <asp:ListItem Text="Semua Normal" Value="2" />
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div style="width: 99%; height: 400px; border: 1px solid #AAA; overflow-y: scroll;
                        overflow-x: hidden; padding-left: 10px;">
                        <asp:Repeater ID="rptReviewOfSystem" runat="server">
                            <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" style="width: 100%">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td class="labelColumn" style="width: 320px; vertical-align: top; padding-top: 5px;">
                                        <input type="hidden" id="hdnGCROSystem" runat="server" value='<%#Eval("StandardCodeID") %>' />
                                        <%# Eval("StandardCodeName") %>
                                    </td>
                                    <td>
                                        <div style="padding-left: 1px">
                                            <asp:RadioButtonList ID="rblReviewOfSystem" CssClass="rblReviewOfSystem" runat="server"
                                                RepeatDirection="Horizontal">
                                                <asp:ListItem Text="Tidak Diperiksa" Value="1" />
                                                <asp:ListItem Text="Normal" Value="2" />
                                                <asp:ListItem Text="Hasil Observasi" Value="3" />
                                            </asp:RadioButtonList>
                                        </div>
                                        <div style="padding-left: 5px">
                                            <asp:TextBox ID="txtFreeText" Style="display: none" CssClass="txtFreeText" runat="server"
                                                Width="500px" />
                                        </div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
