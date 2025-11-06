<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GeriatricScreeningEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.GeriatricScreeningEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_sgaentryctl">
    setDatePicker('<%=txtAssessmentDate.ClientID %>');
    $('#<%=txtAssessmentDate.ClientID %>').datepicker('option', 'maxDate', '0');

    //#region Screening
    //#region Screening A
    function onCboScreeningAChanged(s) {
        if (cboScreeningA.GetValue() != null) {
            if (cboScreeningA.GetValue().indexOf('^001') > -1) {
                $('#<%=txtScreeningA.ClientID %>').val('0');
            }
            else if (cboScreeningA.GetValue().indexOf('^002') > -1) {
                $('#<%=txtScreeningA.ClientID %>').val('1');
            }
            else if (cboScreeningA.GetValue().indexOf('^003') > -1) {
                $('#<%=txtScreeningA.ClientID %>').val('2');
            }
            else {
                $('#<%=txtScreeningA.ClientID %>').val('');
            }
        }
        calculateScreeningTotal();
    }
    //#endregion

    //#region Screening B
    function onCboScreeningBChanged(s) {
        if (cboScreeningB.GetValue() != null) {
            if (cboScreeningB.GetValue().indexOf('^001') > -1) {
                $('#<%=txtScreeningB.ClientID %>').val('0');
            }
            else if (cboScreeningB.GetValue().indexOf('^002') > -1) {
                $('#<%=txtScreeningB.ClientID %>').val('1');
            }
            else if (cboScreeningB.GetValue().indexOf('^003') > -1) {
                $('#<%=txtScreeningB.ClientID %>').val('2');
            }
            else if (cboScreeningB.GetValue().indexOf('^004') > -1) {
                $('#<%=txtScreeningB.ClientID %>').val('3');
            }
            else {
                $('#<%=txtScreeningB.ClientID %>').val('');
            }
        }
        calculateScreeningTotal();
    }
    //#endregion

    //#region Screening C
    function onCboScreeningCChanged(s) {
        if (cboScreeningC.GetValue() != null) {
            if (cboScreeningC.GetValue().indexOf('^001') > -1) {
                $('#<%=txtScreeningC.ClientID %>').val('0');
            }
            else if (cboScreeningC.GetValue().indexOf('^002') > -1) {
                $('#<%=txtScreeningC.ClientID %>').val('1');
            }
            else if (cboScreeningC.GetValue().indexOf('^003') > -1) {
                $('#<%=txtScreeningC.ClientID %>').val('2');
            }
            else if (cboScreeningC.GetValue().indexOf('^004') > -1) {
                $('#<%=txtScreeningC.ClientID %>').val('3');
            }
            else {
                $('#<%=txtScreeningC.ClientID %>').val('');
            }
        }
        calculateScreeningTotal();
    }
    //#endregion

    //#region Screening D
    $('#<%=rblIsPsychologicallyStress.ClientID %> input').change(function () {
        $('#<%=txtIsPsychologicallyStress.ClientID %>').val($(this).val());
        calculateScreeningTotal();
    });
    //#endregion

    //#region Screening E
    function onCboScreeningEChanged(s) {
        if (cboScreeningE.GetValue() != null) {
            if (cboScreeningE.GetValue().indexOf('^001') > -1) {
                $('#<%=txtScreeningE.ClientID %>').val('0');
            }
            else if (cboScreeningE.GetValue().indexOf('^002') > -1) {
                $('#<%=txtScreeningE.ClientID %>').val('1');
            }
            else if (cboScreeningE.GetValue().indexOf('^003') > -1) {
                $('#<%=txtScreeningE.ClientID %>').val('2');
            }
            else {
                $('#<%=txtScreeningE.ClientID %>').val('');
            }
        }
        calculateScreeningTotal();
    }
    //#endregion

    //#region Screening F
    function onCboScreeningFChanged(s) {
        if (cboScreeningF.GetValue() != null) {
            if (cboScreeningF.GetValue().indexOf('^001') > -1) {
                $('#<%=txtScreeningF.ClientID %>').val('0');
            }
            else if (cboScreeningF.GetValue().indexOf('^002') > -1) {
                $('#<%=txtScreeningF.ClientID %>').val('1');
            }
            else if (cboScreeningF.GetValue().indexOf('^003') > -1) {
                $('#<%=txtScreeningF.ClientID %>').val('2');
            }
            else if (cboScreeningF.GetValue().indexOf('^004') > -1) {
                $('#<%=txtScreeningF.ClientID %>').val('3');
            }
            else {
                $('#<%=txtScreeningF.ClientID %>').val('');
            }
        }
        calculateScreeningTotal();
    }
    //#endregion

    function calculateScreeningTotal() {
        var sA = 0;
        var sB = 0;
        var sC = 0;
        var sD = 0;
        var sE = 0;
        var sF = 0;

        if ($('#<%=txtScreeningA.ClientID %>').val())
            sA = parseFloat($('#<%=txtScreeningA.ClientID %>').val());

        if ($('#<%=txtScreeningB.ClientID %>').val())
            sB = parseFloat($('#<%=txtScreeningB.ClientID %>').val());

        if ($('#<%=txtScreeningC.ClientID %>').val())
            sC = parseFloat($('#<%=txtScreeningC.ClientID %>').val());

        if ($('#<%=txtIsPsychologicallyStress.ClientID %>').val())
            sD = parseFloat($('#<%=txtIsPsychologicallyStress.ClientID %>').val());

        if ($('#<%=txtScreeningE.ClientID %>').val())
            sE = parseFloat($('#<%=txtScreeningE.ClientID %>').val());

        if ($('#<%=txtScreeningF.ClientID %>').val())
            sF = parseFloat($('#<%=txtScreeningF.ClientID %>').val());

        var total = sA + sB + sC + sD + sE + sF;
        $('#<%=txtScreeningTotal.ClientID %>').val(total);
        $('#<%=txtScreeningTotalBefore.ClientID %>').val(total);
        calculateOverallTotal();
    }
    //#endregion

    //#region Assessment

    //#region Assessment G
    $('#<%=rblIsIndependent.ClientID %> input').change(function () {
        $('#<%=txtIsIndependent.ClientID %>').val($(this).val());
        calculateAssessmentTotal();
    });
    
    //#endregion

    //#region Assessment H
    $('#<%=rblIsTakingMedicine.ClientID %> input').change(function () {
        $('#<%=txtIsTakingMedicine.ClientID %>').val($(this).val());
        calculateAssessmentTotal();
    });
    //#endregion

    //#region Assessment I
    $('#<%=rblUlkusDekubitus.ClientID %> input').change(function () {
        $('#<%=txtUlkusDekubitus.ClientID %>').val($(this).val());
        calculateAssessmentTotal();
    });
    //#endregion

    //#region Assessment J
    function onCboAssessmentJChanged(s) {
        if (cboAssessmentJ.GetValue() != null) {
            if (cboAssessmentJ.GetValue().indexOf('^001') > -1) {
                $('#<%=txtAssessmentJ.ClientID %>').val('0');
            }
            else if (cboAssessmentJ.GetValue().indexOf('^002') > -1) {
                $('#<%=txtAssessmentJ.ClientID %>').val('1');
            }
            else if (cboAssessmentJ.GetValue().indexOf('^003') > -1) {
                $('#<%=txtAssessmentJ.ClientID %>').val('2');
            }
            else {
                $('#<%=txtAssessmentJ.ClientID %>').val('');
            }
        }
        calculateAssessmentTotal();
    }
    //#endregion

    //#region Assessment K
    $('#<%=rblExchangeOnce.ClientID %> input').change(function () {
        $('#<%=txtExchangeOnce.ClientID %>').val($(this).val());
        calculateIntake();
    });
    $('#<%=rblExchangeTwice.ClientID %> input').change(function () {
        $('#<%=txtExchangeTwice.ClientID %>').val($(this).val());
        calculateIntake();
    });
    $('#<%=rblMeatProduct.ClientID %> input').change(function () {
        $('#<%=txtMeatProduct.ClientID %>').val($(this).val());
        calculateIntake();
    });

    function calculateIntake() {
        var sA = 0;
        var sB = 0;
        var sC = 0;

        if ($('#<%=txtExchangeOnce.ClientID %>').val())
            sA = parseFloat($('#<%=txtExchangeOnce.ClientID %>').val());

        if ($('#<%=txtExchangeTwice.ClientID %>').val())
            sB = parseFloat($('#<%=txtExchangeTwice.ClientID %>').val());

        if ($('#<%=txtMeatProduct.ClientID %>').val())
            sC = parseFloat($('#<%=txtMeatProduct.ClientID %>').val());

        var total = sA + sB + sC;

        if (total == "1" || total == "0") {
            $('#<%=txtTotalIntake.ClientID %>').val('0');
        } else if (total == "2") {
            $('#<%=txtTotalIntake.ClientID %>').val('0.5');
        } else if (total == "3") {
            $('#<%=txtTotalIntake.ClientID %>').val('1');
        }

        calculateAssessmentTotal();
    }

    //#endregion

    //#region Assessment L
    $('#<%=rblChangeVegetableProduct.ClientID %> input').change(function () {
        $('#<%=txtChangeVegetableProduct.ClientID %>').val($(this).val());
        calculateAssessmentTotal();
    });
    //#endregion

    //#region Assessment M
    function onCboAssessmentMChanged(s) {
        if (cboAssessmentM.GetValue() != null) {
            if (cboAssessmentM.GetValue().indexOf('^001') > -1) {
                $('#<%=txtAssessmentM.ClientID %>').val('0');
            }
            else if (cboAssessmentM.GetValue().indexOf('^002') > -1) {
                $('#<%=txtAssessmentM.ClientID %>').val('0.5');
            }
            else if (cboAssessmentM.GetValue().indexOf('^003') > -1) {
                $('#<%=txtAssessmentM.ClientID %>').val('1');
            }
            else {
                $('#<%=txtAssessmentM.ClientID %>').val('');
            }
        }
        calculateAssessmentTotal();
    }
    //#endregion

    //#region Assessment N
    function onCboAssessmentNChanged(s) {
        if (cboAssessmentN.GetValue() != null) {
            if (cboAssessmentN.GetValue().indexOf('^001') > -1) {
                $('#<%=txtAssessmentN.ClientID %>').val('0');
            }
            else if (cboAssessmentN.GetValue().indexOf('^002') > -1) {
                $('#<%=txtAssessmentN.ClientID %>').val('1');
            }
            else if (cboAssessmentN.GetValue().indexOf('^003') > -1) {
                $('#<%=txtAssessmentN.ClientID %>').val('2');
            }
            else {
                $('#<%=txtAssessmentN.ClientID %>').val('');
            }
        }
        calculateAssessmentTotal();
    }
    //#endregion

    //#region Assessment O
    function onCboAssessmentOChanged(s) {
        if (cboAssessmentO.GetValue() != null) {
            if (cboAssessmentO.GetValue().indexOf('^001') > -1) {
                $('#<%=txtAssessmentO.ClientID %>').val('0');
            }
            else if (cboAssessmentO.GetValue().indexOf('^002') > -1) {
                $('#<%=txtAssessmentO.ClientID %>').val('1');
            }
            else if (cboAssessmentO.GetValue().indexOf('^003') > -1) {
                $('#<%=txtAssessmentO.ClientID %>').val('2');
            }
            else {
                $('#<%=txtAssessmentO.ClientID %>').val('');
            }
        }
        calculateAssessmentTotal();
    }
    //#endregion

    //#region Assessment P
    function onCboAssessmentPChanged(s) {
        if (cboAssessmentP.GetValue() != null) {
            if (cboAssessmentP.GetValue().indexOf('^001') > -1) {
                $('#<%=txtAssessmentP.ClientID %>').val('0');
            }
            else if (cboAssessmentP.GetValue().indexOf('^002') > -1) {
                $('#<%=txtAssessmentP.ClientID %>').val('0.5');
            }
            else if (cboAssessmentP.GetValue().indexOf('^003') > -1) {
                $('#<%=txtAssessmentP.ClientID %>').val('1');
            }
            else if (cboAssessmentP.GetValue().indexOf('^004') > -1) {
                $('#<%=txtAssessmentP.ClientID %>').val('2');
            }
            else {
                $('#<%=txtAssessmentP.ClientID %>').val('');
            }
        }
        calculateAssessmentTotal();
    }
    //#endregion

    //#region Assessment Q
    function onCboAssessmentQChanged(s) {
        if (cboAssessmentQ.GetValue() != null) {
            if (cboAssessmentQ.GetValue().indexOf('^001') > -1) {
                $('#<%=txtAssessmentQ.ClientID %>').val('0');
            }
            else if (cboAssessmentQ.GetValue().indexOf('^002') > -1) {
                $('#<%=txtAssessmentQ.ClientID %>').val('0.5');
            }
            else if (cboAssessmentQ.GetValue().indexOf('^003') > -1) {
                $('#<%=txtAssessmentQ.ClientID %>').val('1');
            }
            else {
                $('#<%=txtAssessmentQ.ClientID %>').val('');
            }
        }
        calculateAssessmentTotal();
    }
    //#endregion

    //#region Assessment R
    $('#<%=rblCalfCircumference.ClientID %> input').change(function () {
        $('#<%=txtCalfCircumference.ClientID %>').val($(this).val());
        calculateAssessmentTotal();
    });
    //#endregion

    function calculateAssessmentTotal() {
        var aG = 0;
        var aH = 0;
        var aI = 0;
        var aJ = 0;
        var aK = 0;
        var aL = 0;
        var aM = 0;
        var aN = 0;
        var aO = 0;
        var aP = 0;
        var aQ = 0;
        var aR = 0;

        if ($('#<%=txtIsIndependent.ClientID %>').val())
            aG = parseFloat($('#<%=txtIsIndependent.ClientID %>').val());

        if ($('#<%=txtIsTakingMedicine.ClientID %>').val())
            aH = parseFloat($('#<%=txtIsTakingMedicine.ClientID %>').val());

        if ($('#<%=txtUlkusDekubitus.ClientID %>').val())
            aI = parseFloat($('#<%=txtUlkusDekubitus.ClientID %>').val());

        if ($('#<%=txtAssessmentJ.ClientID %>').val())
            aJ = parseFloat($('#<%=txtAssessmentJ.ClientID %>').val());

        if ($('#<%=txtTotalIntake.ClientID %>').val())
            aK = parseFloat($('#<%=txtTotalIntake.ClientID %>').val());

        if ($('#<%=txtChangeVegetableProduct.ClientID %>').val())
            aL = parseFloat($('#<%=txtChangeVegetableProduct.ClientID %>').val());

        if ($('#<%=txtAssessmentM.ClientID %>').val())
            aM = parseFloat($('#<%=txtAssessmentM.ClientID %>').val());

        if ($('#<%=txtAssessmentN.ClientID %>').val())
            aN = parseFloat($('#<%=txtAssessmentN.ClientID %>').val());

        if ($('#<%=txtAssessmentO.ClientID %>').val())
            aO = parseFloat($('#<%=txtAssessmentO.ClientID %>').val());

        if ($('#<%=txtAssessmentP.ClientID %>').val())
            aP = parseFloat($('#<%=txtAssessmentP.ClientID %>').val());

        if ($('#<%=txtAssessmentQ.ClientID %>').val())
            aQ = parseFloat($('#<%=txtAssessmentQ.ClientID %>').val());

        if ($('#<%=txtCalfCircumference.ClientID %>').val())
            aR = parseFloat($('#<%=txtCalfCircumference.ClientID %>').val());

        var total = aG + aH + aI + aJ + aK + aL + aM + aN + aO + aP + aQ + aR;
        $('#<%=txtAssessmentTotal.ClientID %>').val(total);
        calculateOverallTotal();
    }

    //#endregion

    function calculateOverallTotal() {
        var totalScreening = 0;
        var TotalAssessment = 0;

        if ($('#<%=txtScreeningTotal.ClientID %>').val())
            totalScreening = parseFloat($('#<%=txtScreeningTotal.ClientID %>').val());

        if ($('#<%=txtAssessmentTotal.ClientID %>').val())
            TotalAssessment = parseFloat($('#<%=txtAssessmentTotal.ClientID %>').val());

        var total = totalScreening + TotalAssessment;
        $('#<%=txtOverallTotal.ClientID %>').val(total);
    }

</script>
<div style="height: 550px; overflow-y: scroll;">
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnVisitID" value="" />
    <input type="hidden" runat="server" id="hdnParamedicID" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top;">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 300px" />
                        <col style="width: 150px" />
                        <col style="width: 80px" />
                        <col style="width: 100px" />
                        <col style="width: 50px" />
                        <col style="width: 50px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Tanggal - Jam Pengkajian")%>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAssessmentDate" Width="120px" CssClass="datepicker" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtAssessmentTime" Width="100%" CssClass="time" runat="server" Style="text-align: center" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Ahli Gizi") %></label>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox ID="cboParamedicID" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal" id="Label5" style="font-weight: bold">
                                <%=GetLabel("PENAPISAN (SCREENING) :")%></label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top: 5px; padding-left: 15px;" colspan="2">
                            <%=GetLabel("Apakah ada penurunan asupan makan dalam jangka waktu 3 bulan oleh karena kehilangan nafsu makan, masalah pencernaan, kesulitan menelan atau menguyah?") %>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left: 15px;">
                            <dxe:ASPxComboBox runat="server" ID="cboScreeningA" ClientInstanceName="cboScreeningA"
                                Width="210px">
                                <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboScreeningAChanged(s); }"
                                    Init="function(s,e){ onCboScreeningAChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="padding-left: 5px;">
                                        <%=GetLabel("Skor") %>
                                    </td>
                                    <td style="padding-left: 5px; width: 100px">
                                        <asp:TextBox ID="txtScreeningA" runat="server" Width="100%" ReadOnly="true" CssClass="number" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top: 5px; padding-left: 15px;" colspan="2">
                            <%=GetLabel("Penurunan berat badan dalam 3 bulan terakhir") %>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left: 15px;">
                            <dxe:ASPxComboBox runat="server" ID="cboScreeningB" ClientInstanceName="cboScreeningB"
                                Width="210px">
                                <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboScreeningBChanged(s); }"
                                    Init="function(s,e){ onCboScreeningBChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="padding-left: 5px;">
                                        <%=GetLabel("Skor") %>
                                    </td>
                                    <td style="padding-left: 5px; width: 100px">
                                        <asp:TextBox ID="txtScreeningB" runat="server" Width="100%" ReadOnly="true" CssClass="number" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top: 5px; padding-left: 15px;" colspan="2">
                            <%=GetLabel("Mobilitas") %>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left: 15px;">
                            <dxe:ASPxComboBox runat="server" ID="cboScreeningC" ClientInstanceName="cboScreeningC"
                                Width="210px">
                                <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboScreeningCChanged(s); }"
                                    Init="function(s,e){ onCboScreeningCChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="padding-left: 5px;">
                                        <%=GetLabel("Skor") %>
                                    </td>
                                    <td style="padding-left: 5px; width: 100px">
                                        <asp:TextBox ID="txtScreeningC" runat="server" Width="100%" ReadOnly="true" CssClass="number" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="padding-left: 15px">
                            <label class="lblNormal" id="Label4">
                                <%=GetLabel("Menderita stress psikologis atau penyakit akut dalam 3 bulan terakhir")%></label>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left: 15px">
                            <asp:RadioButtonList ID="rblIsPsychologicallyStress" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Ya" Value="0" />
                                <asp:ListItem Text="Tidak" Value="2" />
                            </asp:RadioButtonList>
                        </td>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="padding-left: 5px;">
                                        <%=GetLabel("Skor") %>
                                    </td>
                                    <td style="padding-left: 5px; width: 100px">
                                        <asp:TextBox ID="txtIsPsychologicallyStress" runat="server" Width="100%" ReadOnly="true"
                                            CssClass="number" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top: 5px; padding-left: 15px;" colspan="2">
                            <%=GetLabel("Masalah neuropsikologis") %>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left: 15px;">
                            <dxe:ASPxComboBox runat="server" ID="cboScreeningE" ClientInstanceName="cboScreeningE"
                                Width="210px">
                                <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboScreeningEChanged(s); }"
                                    Init="function(s,e){ onCboScreeningEChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="padding-left: 5px;">
                                        <%=GetLabel("Skor") %>
                                    </td>
                                    <td style="padding-left: 5px; width: 100px">
                                        <asp:TextBox ID="txtScreeningE" runat="server" Width="100%" ReadOnly="true" CssClass="number" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top: 5px; padding-left: 15px;" colspan="2">
                            <%=GetLabel("Indeks massa tubuh (IMT) (berat badan dalam kg/tinggi badan dalam m2)") %>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left: 15px;">
                            <dxe:ASPxComboBox runat="server" ID="cboScreeningF" ClientInstanceName="cboScreeningF"
                                Width="210px">
                                <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboScreeningFChanged(s); }"
                                    Init="function(s,e){ onCboScreeningFChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="padding-left: 5px;">
                                        <%=GetLabel("Skor") %>
                                    </td>
                                    <td style="padding-left: 5px; width: 100px">
                                        <asp:TextBox ID="txtScreeningF" runat="server" Width="100%" ReadOnly="true" CssClass="number" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" colspan="2">
                            <label class="lblNormal" id="Label2" style="font-weight: bold">
                                <%=GetLabel("Skor PENAPISAN (SCREENING) :")%></label>
                        </td>
                        <td style="width: 60px">
                        </td>
                        <td style="padding-left: 5px; width: 60px">
                            <asp:TextBox ID="txtScreeningTotal" runat="server" Width="100%" ReadOnly="true" CssClass="number" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="padding-left: 15px">
                            <label class="lblNormal" id="Label1">
                                <%=GetLabel("Hidup mandiri, tidak tergantung orang lain (bukan di rumah sakit atau panti werdha")%></label>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left: 15px">
                            <asp:RadioButtonList ID="rblIsIndependent" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Ya" Value="0" />
                                <asp:ListItem Text="Tidak" Value="1" />
                            </asp:RadioButtonList>
                        </td>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="padding-left: 5px;">
                                        <%=GetLabel("Skor") %>
                                    </td>
                                    <td style="padding-left: 5px; width: 100px">
                                        <asp:TextBox ID="txtIsIndependent" runat="server" Width="100%" ReadOnly="true" CssClass="number" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="padding-left: 15px">
                            <label class="lblNormal" id="Label3">
                                <%=GetLabel("Minum obat lebih dari 3 macam dalam 1 hari")%></label>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left: 15px">
                            <asp:RadioButtonList ID="rblIsTakingMedicine" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Ya" Value="0" />
                                <asp:ListItem Text="Tidak" Value="1" />
                            </asp:RadioButtonList>
                        </td>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="padding-left: 5px;">
                                        <%=GetLabel("Skor") %>
                                    </td>
                                    <td style="padding-left: 5px; width: 100px">
                                        <asp:TextBox ID="txtIsTakingMedicine" runat="server" Width="100%" ReadOnly="true"
                                            CssClass="number" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="padding-left: 15px">
                            <label class="lblNormal" id="Label6">
                                <%=GetLabel("Terdapat ulkus dekubitus / luka tekan atau luka kulit")%></label>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left: 15px">
                            <asp:RadioButtonList ID="rblUlkusDekubitus" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Ya" Value="0" />
                                <asp:ListItem Text="Tidak" Value="1" />
                            </asp:RadioButtonList>
                        </td>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="padding-left: 5px;">
                                        <%=GetLabel("Skor") %>
                                    </td>
                                    <td style="padding-left: 5px; width: 100px">
                                        <asp:TextBox ID="txtUlkusDekubitus" runat="server" Width="100%" ReadOnly="true" CssClass="number" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top: 5px; padding-left: 15px;" colspan="2">
                            <%=GetLabel("Berapa kali pasien makan lengkap dalam 1 hari?") %>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left: 15px;">
                            <dxe:ASPxComboBox runat="server" ID="cboAssessmentJ" ClientInstanceName="cboAssessmentJ"
                                Width="210px">
                                <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboAssessmentJChanged(s); }"
                                    Init="function(s,e){ onCboAssessmentJChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="padding-left: 5px;">
                                        <%=GetLabel("Skor") %>
                                    </td>
                                    <td style="padding-left: 5px; width: 100px">
                                        <asp:TextBox ID="txtAssessmentJ" runat="server" Width="100%" ReadOnly="true" CssClass="number" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="padding-left: 15px">
                            <label class="lblNormal" id="Label7">
                                <%=GetLabel("Konsumsi BM tertentu yang diketahui sebagai BM sumber protein (asupan protein)")%></label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="padding-left: 20px">
                            <label class="lblNormal" id="Label8">
                                <%=GetLabel("Sedikitnya 1 penukar dari produk susu (susu, keju, yoghurt) per hari")%></label>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left: 20px">
                            <asp:RadioButtonList ID="rblExchangeOnce" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Ya" Value="1" />
                                <asp:ListItem Text="Tidak" Value="0" />
                            </asp:RadioButtonList>
                        </td>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="padding-left: 5px; width: 100px">
                                        <asp:TextBox ID="txtExchangeOnce" runat="server" Width="100%" ReadOnly="true" CssClass="number" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="padding-left: 20px">
                            <label class="lblNormal" id="Label9">
                                <%=GetLabel("Dua penukar atau lebih dari kacang-kacangan atau telur perminggu")%></label>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left: 20px">
                            <asp:RadioButtonList ID="rblExchangeTwice" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Ya" Value="1" />
                                <asp:ListItem Text="Tidak" Value="0" />
                            </asp:RadioButtonList>
                        </td>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="padding-left: 5px; width: 100px">
                                        <asp:TextBox ID="txtExchangeTwice" runat="server" Width="100%" ReadOnly="true" CssClass="number" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="padding-left: 20px">
                            <label class="lblNormal" id="Label10">
                                <%=GetLabel("Daging, ikan, atau unggas tiap hari")%></label>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left: 20px">
                            <asp:RadioButtonList ID="rblMeatProduct" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Ya" Value="1" />
                                <asp:ListItem Text="Tidak" Value="0" />
                            </asp:RadioButtonList>
                        </td>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="padding-left: 5px; width: 100px">
                                        <asp:TextBox ID="txtMeatProduct" runat="server" Width="100%" ReadOnly="true" CssClass="number" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" colspan="2">
                            <label class="lblNormal" id="Label11" style="font-weight: bold; padding-left: 50px;">
                                <%=GetLabel("Skor total :")%></label>
                        </td>
                        <td style="width: 60px">
                        </td>
                        <td style="padding-left: 5px; width: 60px">
                            <asp:TextBox ID="txtTotalIntake" runat="server" Width="100%" ReadOnly="true" CssClass="number" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="padding-left: 15px">
                            <label class="lblNormal" id="Label12">
                                <%=GetLabel("Adakah mengkonsumsi 2 penukar atau lebih buah atau sayuran per hari?")%></label>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left: 15px">
                            <asp:RadioButtonList ID="rblChangeVegetableProduct" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Ya" Value="1" />
                                <asp:ListItem Text="Tidak" Value="0" />
                            </asp:RadioButtonList>
                        </td>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="padding-left: 5px;">
                                        <%=GetLabel("Skor") %>
                                    </td>
                                    <td style="padding-left: 5px; width: 100px">
                                        <asp:TextBox ID="txtChangeVegetableProduct" runat="server" Width="100%" ReadOnly="true"
                                            CssClass="number" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top: 5px; padding-left: 15px;" colspan="2">
                            <%=GetLabel("Berapa banyak carian (air, just, kopi, teh, susu ...) yang diminum setiap hari") %>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left: 15px;">
                            <dxe:ASPxComboBox runat="server" ID="cboAssessmentM" ClientInstanceName="cboAssessmentM"
                                Width="210px">
                                <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboAssessmentMChanged(s); }"
                                    Init="function(s,e){ onCboAssessmentMChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="padding-left: 5px;">
                                        <%=GetLabel("Skor") %>
                                    </td>
                                    <td style="padding-left: 5px; width: 100px">
                                        <asp:TextBox ID="txtAssessmentM" runat="server" Width="100%" ReadOnly="true" CssClass="number" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top: 5px; padding-left: 15px;" colspan="2">
                            <%=GetLabel("Cara Makan") %>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left: 15px;">
                            <dxe:ASPxComboBox runat="server" ID="cboAssessmentN" ClientInstanceName="cboAssessmentN"
                                Width="210px">
                                <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboAssessmentNChanged(s); }"
                                    Init="function(s,e){ onCboAssessmentNChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="padding-left: 5px;">
                                        <%=GetLabel("Skor") %>
                                    </td>
                                    <td style="padding-left: 5px; width: 100px">
                                        <asp:TextBox ID="txtAssessmentN" runat="server" Width="100%" ReadOnly="true" CssClass="number" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top: 5px; padding-left: 15px;" colspan="2">
                            <%=GetLabel("Pandangan pasien terhadap status gizinya") %>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left: 15px;">
                            <dxe:ASPxComboBox runat="server" ID="cboAssessmentO" ClientInstanceName="cboAssessmentO"
                                Width="210px">
                                <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboAssessmentOChanged(s); }"
                                    Init="function(s,e){ onCboAssessmentOChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="padding-left: 5px;">
                                        <%=GetLabel("Skor") %>
                                    </td>
                                    <td style="padding-left: 5px; width: 100px">
                                        <asp:TextBox ID="txtAssessmentO" runat="server" Width="100%" ReadOnly="true" CssClass="number" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top: 5px; padding-left: 15px;" colspan="2">
                            <%=GetLabel("Dibandingkan dengan orang lain yang seumur, bagaimana pasien melihat status kesehatannya") %>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left: 15px;">
                            <dxe:ASPxComboBox runat="server" ID="cboAssessmentP" ClientInstanceName="cboAssessmentP"
                                Width="210px">
                                <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboAssessmentPChanged(s); }"
                                    Init="function(s,e){ onCboAssessmentPChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="padding-left: 5px;">
                                        <%=GetLabel("Skor") %>
                                    </td>
                                    <td style="padding-left: 5px; width: 100px">
                                        <asp:TextBox ID="txtAssessmentP" runat="server" Width="100%" ReadOnly="true" CssClass="number" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top: 5px; padding-left: 15px;" colspan="2">
                            <%=GetLabel("Lingkar lengan atas (LLA) dalam cm") %>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left: 15px;">
                            <dxe:ASPxComboBox runat="server" ID="cboAssessmentQ" ClientInstanceName="cboAssessmentQ"
                                Width="210px">
                                <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboAssessmentQChanged(s); }"
                                    Init="function(s,e){ onCboAssessmentQChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="padding-left: 5px;">
                                        <%=GetLabel("Skor") %>
                                    </td>
                                    <td style="padding-left: 5px; width: 100px">
                                        <asp:TextBox ID="txtAssessmentQ" runat="server" Width="100%" ReadOnly="true" CssClass="number" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="padding-left: 20px">
                            <label class="lblNormal" id="Label13">
                                <%=GetLabel("Lingkar betis (LB) dalam cm")%></label>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left: 20px">
                            <asp:RadioButtonList ID="rblCalfCircumference" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="LB >= 31" Value="1" />
                                <asp:ListItem Text="LB < 31" Value="0" />
                            </asp:RadioButtonList>
                        </td>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="padding-left: 5px;">
                                        <%=GetLabel("Skor") %>
                                    </td>
                                    <td style="padding-left: 5px; width: 100px">
                                        <asp:TextBox ID="txtCalfCircumference" runat="server" Width="100%" ReadOnly="true"
                                            CssClass="number" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" colspan="2">
                            <label class="lblNormal" id="Label14" style="font-weight: bold">
                                <%=GetLabel("Skor PENAPISAN (maksimum 16 poin) :")%></label>
                        </td>
                        <td style="width: 60px">
                        </td>
                        <td style="padding-left: 5px; width: 60px">
                            <asp:TextBox ID="txtScreeningTotalBefore" runat="server" Width="100%" ReadOnly="true" CssClass="number" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" colspan="2">
                            <label class="lblNormal" id="Label15" style="font-weight: bold">
                                <%=GetLabel("Skor PENGKAJIAN (maksimum 14 poin) :")%></label>
                        </td>
                        <td style="width: 60px">
                        </td>
                        <td style="padding-left: 5px; width: 60px">
                            <asp:TextBox ID="txtAssessmentTotal" runat="server" Width="100%" ReadOnly="true" CssClass="number" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" colspan="2">
                            <label class="lblNormal" id="Label16" style="font-weight: bold">
                                <%=GetLabel("PENILAIAN TOTAL (maksimum 30 poin) :")%></label>
                        </td>
                        <td style="width: 60px">
                        </td>
                        <td style="padding-left: 5px; width: 60px">
                            <asp:TextBox ID="txtOverallTotal" runat="server" Width="100%" ReadOnly="true" CssClass="number" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
