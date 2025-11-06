<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UpdateUDDFlagCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Pharmacy.Program.UpdateUDDFlagCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<style type="text/css">
    .highlight
    {
        background-color: #FE5D15;
        color: White;
    }
</style>
<script type="text/javascript" id="dxss_UpdateUDDFlagCtl">
    $(function () {
        $('#<%=lvwView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
            if ($(this).attr('class') != 'selected') {
                $('#<%=lvwView.ClientID %> tr.selected').removeClass('selected');
                $('#<%=hdnSelectedID.ClientID %>').val($(this).closest('tr').find('.keyField').html());
                $(this).addClass('selected');
            }
        });
        $('#<%=lvwView.ClientID %> tr:eq(1)').click();
    });

    $('#chkSelectAll').die('change');
    $('#chkSelectAll').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkIsUsingUDD').each(function () {
            $(this).find('input').prop('checked', isChecked);
            var $cell = $(this).closest("td");
            var $tr = $cell.closest('tr');
            var frequency = $tr.find('.frequency').html()
            if ($tr.find('.txtMedicationTime1').val() == "") {
                SetMedicationDefaultTime($tr, frequency)
            }
            if (isChecked) {
                $cell.addClass('highlight');
            }
            else {
                $cell.removeClass('highlight');
            }
        });
    });

    function onBeforeProcess(param) {
        if (!getSelectedItem()) {
            return false;
        }
        else {
            return true;
        }
    }

    function onGetEntryPopupReturnValue() {
        var result = $('#<%=hdnSelectedID.ClientID %>').val();
        return result;
    }

    $('.chkIsUsingUDD input').live('change', function () {
        var $cell = $(this).closest("td");
        var $tr = $cell.closest('tr');
        var isChecked = $(this).is(":checked");
        var frequency = $tr.find('.frequency').html()
        if ($(this).is(':checked')) {
            $cell.addClass('highlight');
        }
        else {
            $cell.removeClass('highlight');
        }

        if ($tr.find('.txtMedicationTime1').val() == "") {
            SetMedicationDefaultTime($tr, frequency)
        }
    });

    function getSelectedItem() {
        var tempSelectedID = "";
        var tempIsUsingUDD = "";
        var tempSelectedTime1 = "";
        var tempSelectedTime2 = "";
        var tempSelectedTime3 = "";
        var tempSelectedTime4 = "";
        var tempSelectedTime5 = "";
        var tempSelectedTime6 = "";
        $('.grdPrescriptionOrderDt .chkIsUsingUDD').each(function () {
            var $tr = $(this).closest('tr');
            var id = $(this).closest('tr').find('.keyField').html();
            var isChecked = $(this).is(":checked");

            var isUsingUDD = '0';
            if ($tr.find('.chkIsUsingUDD input').is(':checked')) {
                isUsingUDD = '1';
            }

            var sequence1Time = $tr.find('.txtMedicationTime1').val();
            var sequence2Time = $tr.find('.txtMedicationTime2').val();
            var sequence3Time = $tr.find('.txtMedicationTime3').val();
            var sequence4Time = $tr.find('.txtMedicationTime4').val();
            var sequence5Time = $tr.find('.txtMedicationTime5').val();
            var sequence6Time = $tr.find('.txtMedicationTime6').val();

            if (sequence1Time == '') {
                sequence1Time = '-';
            }
            if (sequence2Time == '') {
                sequence2Time = '-';
            }
            if (sequence3Time == '') {
                sequence3Time = '-';
            }
            if (sequence4Time == '') {
                sequence4Time = '-';
            }
            if (sequence5Time == '') {
                sequence5Time = '-';
            }
            if (sequence6Time == '') {
                sequence6Time = '-';
            }

            if (tempSelectedID != "") {
                tempSelectedID += ",";
            }

            if (tempIsUsingUDD != "") {
                tempIsUsingUDD += ",";
            }

            if (tempSelectedTime1 != "") {
                tempSelectedTime1 += ",";
            }

            if (tempSelectedTime2 != "") {
                tempSelectedTime2 += ",";
            }

            if (tempSelectedTime3 != "") {
                tempSelectedTime3 += ",";
            }

            if (tempSelectedTime4 != "") {
                tempSelectedTime4 += ",";
            }

            if (tempSelectedTime5 != "") {
                tempSelectedTime5 += ",";
            }

            if (tempSelectedTime6 != "") {
                tempSelectedTime6 += ",";
            }

            tempSelectedID += id;
            tempIsUsingUDD += isUsingUDD;
            tempSelectedTime1 += sequence1Time;
            tempSelectedTime2 += sequence2Time;
            tempSelectedTime3 += sequence3Time;
            tempSelectedTime4 += sequence4Time;
            tempSelectedTime5 += sequence5Time;
            tempSelectedTime6 += sequence6Time;
        });

        $('#<%=hdnSelectedID.ClientID %>').val(tempSelectedID);
        $('#<%=hdnSelectedIsUsingUDD.ClientID %>').val(tempIsUsingUDD);
        $('#<%=hdnSequence1Time.ClientID %>').val(tempSelectedTime1);
        $('#<%=hdnSequence2Time.ClientID %>').val(tempSelectedTime2);
        $('#<%=hdnSequence3Time.ClientID %>').val(tempSelectedTime3);
        $('#<%=hdnSequence4Time.ClientID %>').val(tempSelectedTime4);
        $('#<%=hdnSequence5Time.ClientID %>').val(tempSelectedTime5);
        $('#<%=hdnSequence6Time.ClientID %>').val(tempSelectedTime6);

        return true;
    }

    function onCbpViewEndCallback(s) {
        hideLoadingPanel();
    }

    function SetMedicationDefaultTime(tr, frequency) {
        switch (frequency) {
            case "1":
                tr.find('.txtMedicationTime1').val('08:00');
                tr.find('.txtMedicationTime2').val('');
                tr.find('.txtMedicationTime3').val('');
                tr.find('.txtMedicationTime4').val('');
                tr.find('.txtMedicationTime5').val('');
                tr.find('.txtMedicationTime6').val('');
                break;
            case "2":
                tr.find('.txtMedicationTime1').val('08:00');
                tr.find('.txtMedicationTime2').val('20:00');
                tr.find('.txtMedicationTime3').val('');
                tr.find('.txtMedicationTime4').val('');
                tr.find('.txtMedicationTime5').val('');
                tr.find('.txtMedicationTime6').val('');
                break;
            case "3":
                tr.find('.txtMedicationTime1').val('08:00');
                tr.find('.txtMedicationTime2').val('16:00');
                tr.find('.txtMedicationTime3').val('24:00');
                tr.find('.txtMedicationTime4').val('');
                tr.find('.txtMedicationTime5').val('');
                tr.find('.txtMedicationTime6').val('');
                break;
            case "4":
                tr.find('.txtMedicationTime1').val('06:00');
                tr.find('.txtMedicationTime2').val('12:00');
                tr.find('.txtMedicationTime3').val('18:00');
                tr.find('.txtMedicationTime4').val('24:00');
                tr.find('.txtMedicationTime5').val('');
                tr.find('.txtMedicationTime6').val('');
                break;
            case "5":
                tr.find('.txtMedicationTime1').val('06:00');
                tr.find('.txtMedicationTime2').val('09:00');
                tr.find('.txtMedicationTime3').val('12:00');
                tr.find('.txtMedicationTime4').val('15:00');
                tr.find('.txtMedicationTime5').val('18:00');
                tr.find('.txtMedicationTime6').val('');
                break;
            case "6":
                tr.find('.txtMedicationTime1').val('02:00');
                tr.find('.txtMedicationTime2').val('06:00');
                tr.find('.txtMedicationTime3').val('10:00');
                tr.find('.txtMedicationTime4').val('14:00');
                tr.find('.txtMedicationTime5').val('18:00');
                tr.find('.txtMedicationTime6').val('22:00');
                break;
            default:
                tr.find('.txtMedicationTime1').val('08:00');
                tr.find('.txtMedicationTime2').val('');
                tr.find('.txtMedicationTime3').val('');
                tr.find('.txtMedicationTime4').val('');
                tr.find('.txtMedicationTime5').val('');
                tr.find('.txtMedicationTime6').val('');
                break;
        }
    }

    function onAfterProcessPopupEntry(param) {
        if (typeof onAfterUpdateUDDStatus == 'function')
            onAfterUpdateUDDStatus(param);
    }
</script>
<input type="hidden" id="hdnPrescriptionOrderID" runat="server" />
<input type="hidden" id="hdnSelectedID" runat="server" />
<input type="hidden" id="hdnSelectedIsUsingUDD" runat="server" />
<input type="hidden" id="hdnSequence1Time" runat="server" />
<input type="hidden" id="hdnSequence2Time" runat="server" />
<input type="hidden" id="hdnSequence3Time" runat="server" />
<input type="hidden" id="hdnSequence4Time" runat="server" />
<input type="hidden" id="hdnSequence5Time" runat="server" />
<input type="hidden" id="hdnSequence6Time" runat="server" />
<div style="height: 440px; overflow-y: auto">
    <table class="tblContentArea" style="width: 100%">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table>
                    <colgroup>
                        <col width="150px" />
                        <col width="150px" />
                        <col width="150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. Order")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTransactionNoCtl" runat="server" Width="99%" ReadOnly="true" />
                        </td>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Jenis Resep") %></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboPrescriptionTypeCtl" ClientInstanceName="cboPrescriptionTypeCtl"
                                runat="server" Width="235px" Enabled="false">
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tanggal/Jam Order")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtOrderDateTimeCtl" runat="server" Width="99%" ReadOnly="true" />
                        </td>
                        <td class="tdLabel">
                            <label class="lblMandatory" title="Mempengaruhi apakah dokter bisa melakukan reopen order atau tidak.">
                                <%=GetLabel("Status Transaksi Order") %></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboTransactionStatusCtl" ClientInstanceName="cboTransactionStatusCtl"
                                runat="server" Width="235px">
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                </table>
                <div style="height: 300px; overflow-y: auto;">
                    <dxcp:ASPxCallbackPanel ID="cbpProcessView" runat="server" Width="100%" ClientInstanceName="cbpProcessView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative; font-size: 0.95em;">
                                    <asp:ListView runat="server" ID="lvwView">
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdPrescriptionOrderDt grdSelected" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th>
                                                        <%=GetLabel("UDD")%>
                                                    </th>
                                                    <th class="keyField" rowspan="2">
                                                        &nbsp;
                                                    </th>
                                                    <th class="hiddenColumn" rowspan="2">
                                                        &nbsp;
                                                    </th>
                                                    <th class="hiddenColumn" rowspan="2">
                                                        &nbsp;
                                                    </th>
                                                    <th rowspan="2" align="left">
                                                        <%=GetLabel("Drug Name")%>
                                                    </th>
                                                    <th rowspan="2" align="left">
                                                        <%=GetLabel("Signa")%>
                                                    </th>
                                                    <th colspan="6" align="center">
                                                        <div>
                                                            <%=GetLabel("Jadwal Pemberian Obat") %></div>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th align="center" style="width: 30px">
                                                        <input id="chkSelectAll" type="checkbox" />
                                                    </th>
                                                    <th style="width: 30px;" align="center">
                                                        <div>
                                                            <%=GetLabel("1") %></div>
                                                    </th>
                                                    <th style="width: 30px;" align="center">
                                                        <div>
                                                            <%=GetLabel("2") %></div>
                                                    </th>
                                                    <th style="width: 30px;" align="center">
                                                        <div>
                                                            <%=GetLabel("3") %></div>
                                                    </th>
                                                    <th style="width: 30px;" align="center">
                                                        <div>
                                                            <%=GetLabel("4") %></div>
                                                    </th>
                                                    <th style="width: 30px;" align="center">
                                                        <div>
                                                            <%=GetLabel("5") %></div>
                                                    </th>
                                                    <th style="width: 30px;" align="center">
                                                        <div>
                                                            <%=GetLabel("6") %></div>
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td align="center" style="width: 30px">
                                                    <asp:CheckBox ID="chkIsUsingUDD" runat="server" CssClass="chkIsUsingUDD" Checked='<%# Eval("IsUsingUDD")%>' />
                                                </td>
                                                <td class="keyField">
                                                    <%#: Eval("PrescriptionOrderDetailID")%>
                                                </td>
                                                <td class="hiddenColumn frequency">
                                                    <%#: Eval("Frequency")%>
                                                </td>
                                                <td class="tdItemName">
                                                    <div>
                                                        <label class="lblItemName">
                                                            <%#: Eval("DrugName")%></label></div>
                                                    <div>
                                                        <label class="lblRoute">
                                                            <%#: Eval("Route")%></label></div>
                                                </td>
                                                <td class="tdConsumeMethod" style="width: 120px">
                                                    <div>
                                                        <label class="lblConsumeMethod">
                                                            <%#: Eval("cfConsumeMethod")%></label></div>
                                                </td>
                                                <td style="width: 70px;">
                                                    <input type="text" class="txtMedicationTime1" value="<%#:Eval("Sequence1Time") %>"
                                                        style="width: 60px; text-align: center" />
                                                </td>
                                                <td style="width: 70px;">
                                                    <input type="text" class="txtMedicationTime2" value="<%#:Eval("Sequence2Time") %>"
                                                        style="width: 60px; text-align: center" />
                                                </td>
                                                <td style="width: 70px;">
                                                    <input type="text" class="txtMedicationTime3" value="<%#:Eval("Sequence3Time") %>"
                                                        style="width: 60px; text-align: center" />
                                                </td>
                                                <td style="width: 70px;">
                                                    <input type="text" class="txtMedicationTime4" value="<%#:Eval("Sequence4Time") %>"
                                                        style="width: 60px; text-align: center" />
                                                </td>
                                                <td style="width: 70px;">
                                                    <input type="text" class="txtMedicationTime5" value="<%#:Eval("Sequence5Time") %>"
                                                        style="width: 60px; text-align: center" />
                                                </td>
                                                <td style="width: 70px;">
                                                    <input type="text" class="txtMedicationTime6" value="<%#:Eval("Sequence6Time") %>"
                                                        style="width: 60px; text-align: center" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </div>
            </td>
        </tr>
    </table>
</div>
<div>
    <table border="0" cellpadding="1" cellspacing="0">
        <colgroup>
            <col style="width: 30px" />
            <col style="width: 40px" />
            <col style="width: 30px" />
            <col style="width: 40px" />
            <col style="width: 30px" />
            <col style="width: 40px" />
            <col style="width: 30px" />
            <col style="width: 40px" />
            <col style="width: 30px" />
            <col style="width: 40px" />
            <col style="width: 30px" />
            <col style="width: 40px" />
            <col style="width: 30px" />
            <col style="width: 40px" />
            <col style="width: 30px" />
            <col style="width: 40px" />
            <col style="width: 30px" />
            <col style="width: 40px" />
            <col style="width: 30px" />
            <col style="width: 40px" />
            <col style="width: 30px" />
            <col style="width: 40px" />
            <col />
        </colgroup>
        <tr>
            <td>
                <asp:CheckBox ID="chkIsCorrectPatientCtl" runat="server" ToolTip="Benar Pasien" />
            </td>
            <td style="font-weight: bold;">
                <asp:Label ID="Label1" runat="server" ToolTip="Benar Pasien" Text="Px">Px</asp:Label>
            </td>
            <td>
                <asp:CheckBox ID="chkIsCorrectMedicationCtl" runat="server" ToolTip="Benar Obat" />
            </td>
            <td style="font-weight: bold;">
                <asp:Label ID="Label2" runat="server" ToolTip="Benar Obat" Text="OB">OB</asp:Label>
            </td>
            <td>
                <asp:CheckBox ID="chkIsCorrectStrengthCtl" runat="server" ToolTip="Benar Kekuatan Obat" />
            </td>
            <td style="font-weight: bold;">
                <asp:Label ID="Label3" runat="server" ToolTip="Benar Kekuatan Obat" Text="KE">KE</asp:Label>
            </td>
            <td>
                <asp:CheckBox ID="chkIsCorrectFrequencyCtl" runat="server" ToolTip="Benar Frekuensi Pemberian" />
            </td>
            <td style="font-weight: bold;">
                <asp:Label ID="Label4" runat="server" ToolTip="Benar Frekuensi Pemberian" Text="FRE">FRE</asp:Label>
            </td>
            <td>
                <asp:CheckBox ID="chkIsCorrectDosageCtl" runat="server" ToolTip="Benar Dosis" />
            </td>
            <td style="font-weight: bold;">
                <asp:Label ID="Label5" runat="server" ToolTip="Benar Dosis" Text="DO">DO</asp:Label>
            </td>
            <td>
                <asp:CheckBox ID="chkIsCorrectRouteCtl" runat="server" ToolTip="Benar Rute Pemberian" />
            </td>
            <td style="font-weight: bold;">
                <asp:Label ID="Label6" runat="server" ToolTip="Benar Rute Pemberian" Text="RP">RP</asp:Label>
            </td>
            <td>
                <asp:CheckBox ID="chkIsHasDrugInteractionCtl" runat="server" ToolTip="ada tidaknya interaksi obat" />
            </td>
            <td style="font-weight: bold;">
                <asp:Label ID="Label7" runat="server" ToolTip="ada tidaknya interaksi obat" Text="IO">IO</asp:Label>
            </td>
            <td>
                <asp:CheckBox ID="chkIsHasDuplicationCtl" runat="server" ToolTip="ada tidaknya duplikasi obat" />
            </td>
            <td style="font-weight: bold;">
                <asp:Label ID="Label8" runat="server" ToolTip="ada tidaknya duplikasi obat" Text="IO">DUP</asp:Label>
            </td>
            <td>
                <asp:CheckBox ID="chkIsADCheckedCtl" runat="server" ToolTip="(AD)" />
            </td>
            <td style="font-weight: bold;">
                <asp:Label ID="Label9" runat="server" ToolTip="(AD)" Text="AD">AD</asp:Label>
            </td>
            <td>
                <asp:CheckBox ID="chkIsFARCheckedCtl" runat="server" ToolTip="(FAR)" />
            </td>
            <td style="font-weight: bold;">
                <asp:Label ID="Label10" runat="server" ToolTip="(FAR)" Text="FAR">FAR</asp:Label>
            </td>
            <td>
                <asp:CheckBox ID="chkIsKLNCheckedCtl" runat="server" ToolTip="(KLN)" />
            </td>
            <td style="font-weight: bold;">
                <asp:Label ID="Label11" runat="server" ToolTip="(KLN)" Text="FAR">KLN</asp:Label>
            </td>
            <td />
        </tr>
    </table>
</div>
