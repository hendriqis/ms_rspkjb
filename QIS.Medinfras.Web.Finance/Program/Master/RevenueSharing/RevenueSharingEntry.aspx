<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true"
    CodeBehind="RevenueSharingEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.RevenueSharingEntry" %>

<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#btnCount').click(function () {
                var revenueSharing = '';

                var rblValue = $("#<%=rblFormulaType.ClientID%> input:checked").val();
                var count = 0;
                var chkComp = [];
                var total = 0;
                chkComp.push($('#<%=chkComp1.ClientID %>').is(':checked'));
                chkComp.push($('#<%=chkComp2.ClientID %>').is(':checked'));
                chkComp.push($('#<%=chkComp3.ClientID %>').is(':checked'));

                //base tarif
                if (rblValue == '<%=OnGetFormulaTypeBaseTarifCode() %>') {
                    $('.txtBaseComp').each(function () {
                        if (chkComp[count]) {
                            total += parseFloat($(this).attr('hiddenVal'));
                        }
                        count++;
                    });
                }
                else {
                    $('.txtTariffComp').each(function () {
                        if (chkComp[count]) {
                            total += parseFloat($(this).attr('hiddenVal'));
                        }
                        count++;
                    });
                }

                if ($('#<%=chkIsSharingInPercentage.ClientID %>').is(':checked')) {
                    revenueSharing = parseFloat($('#<%=txtSharingAmount.ClientID %>').attr('hiddenVal'));
                    total = total * revenueSharing / 100;
                }
                else {
                    revenueSharing = parseFloat($('#<%=txtSharingAmount.ClientID %>').attr('hiddenVal'));
                    if (total > revenueSharing)
                        total = revenueSharing;
                }

                $('#<%=txtPrevBaseRevenue.ClientID %>').val(total).trigger('changeValue');

                var tempTotal = total;
                $('.trSharingComponent').each(function () {
                    var sharingComponent = $(this).find('.hdnGCSharingComponent').val().split('^')[1];
                    var amount = parseFloat($(this).find('.txtSharingComponentAmount').attr('hiddenVal'));

                    var chk = $(this).find('.chkIsInPercentage').find('input').is(':checked');

                    if (chk && amount > 0) {
                        compVal = tempTotal * amount / 100;
                        $('.txtPreviewDt[formulatype=' + sharingComponent + ']').val(compVal).trigger('changeValue');
                        total -= compVal;
                    }
                    else if (amount > 0) {
                        compVal = amount;
                        $('.txtPreviewDt[formulatype=' + sharingComponent + ']').val(compVal).trigger('changeValue');
                        total -= compVal;
                    } else {
                        $('.txtPreviewDt[formulatype=' + sharingComponent + ']').val(0).trigger('changeValue');
                    }
                });

                $('#<%=txtDoctor.ClientID %>').val(total).trigger('changeValue');
            });

            $('.txtBaseComp').change(function () {
                $(this).blur();
                var total = 0;
                $('.txtBaseComp').each(function () {
                    total += parseFloat($(this).attr('hiddenVal'));
                });

                $('#<%=txtPrevBaseTariffTotal.ClientID %>').val(total).trigger('changeValue');
            });

            $('.txtTariffComp').change(function () {
                $(this).blur();
                var total = 0;
                $('.txtTariffComp').each(function () {
                    total += parseFloat($(this).attr('hiddenVal'));
                });

                $('#<%=txtPrevTariffTotal.ClientID %>').val(total).trigger('changeValue');
            });

            $('#<%=txtPrevBaseTariffTotal.ClientID %>').change(function () {
                $('.txtBaseComp:eq(0)').val($(this).val()).trigger('changeValue');
                $('.txtBaseComp:eq(1)').val('0').trigger('changeValue');
                $('.txtBaseComp:eq(2)').val('0').trigger('changeValue');
            });

            $('#<%=txtPrevTariffTotal.ClientID %>').change(function () {
                $('.txtTariffComp:eq(0)').val($(this).val()).trigger('changeValue');
                $('.txtTariffComp:eq(1)').val('0').trigger('changeValue');
                $('.txtTariffComp:eq(2)').val('0').trigger('changeValue');
            });
        });


    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnComp1Text" runat="server" value="" />
    <input type="hidden" id="hdnComp2Text" runat="server" value="" />
    <input type="hidden" id="hdnComp3Text" runat="server" value="" />
    <table class="tblContentArea" style="width: 100%">
        <colgroup>
            <col style="width: 55%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 80%">
                    <colgroup>
                        <col style="width: 40%" />
                        <col style="width: 40px" />
                        <col/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Kode Jasa Medis")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtRevenueSharingCode" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Nama Jasa Medis")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtRevenueSharingName" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Formula Pembagian Berdasarkan")%></label></td>
                        <td colspan="2"><asp:RadioButtonList ID="rblFormulaType" runat="server" RepeatDirection="Horizontal" Width="200px" /></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td colspan="2">
                            <asp:CheckBox runat="server" ID="chkComp1" />
                            <asp:CheckBox runat="server" ID="chkComp2" />
                            <asp:CheckBox runat="server" ID="chkComp3" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Jumlah Pembagian")%></label></td>
                        <td><asp:CheckBox ID="chkIsSharingInPercentage" runat="server" /> %</td>
                        <td><asp:TextBox ID="txtSharingAmount" CssClass="txtCurrency" Width="100px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Kontrol Kartu Kredit")%></label></td>
                        <td><asp:CheckBox ID="chkIsControlCreditCard" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Kartu Kredit")%></label></td>
                        <td><asp:CheckBox ID="chkIsCreditCardFeeInPercentage" runat="server" /> %</td>
                        <td><asp:TextBox ID="txtCreditCardFeeAmount" CssClass="txtCurrency" Width="100px" runat="server" /></td>
                    </tr>
                    <asp:Repeater ID="rptFormulaType" runat="server">
                        <ItemTemplate>
                            <tr class="trSharingComponent">
                                <td class="tdLabel">
                                    <input type="hidden" id="hdnGCSharingComponent" class="hdnGCSharingComponent" value='<%#:Eval("StandardCodeID")%>' runat="server" />
                                    <label class="lblMandatory"><%#:Eval("StandardCodeName")%></label>
                                </td>
                                <td><asp:CheckBox ID="chkIsInPercentage" CssClass="chkIsInPercentage" runat="server" /> %</td>
                                <td><asp:TextBox ID="txtAmount" CssClass="txtCurrency txtSharingComponentAmount" Width="100px" runat="server" /></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Kontrol Jasa Minimal")%></label></td>
                        <td><asp:CheckBox ID="chkIsControlGuaranteePayment" runat="server" /></td>
                    </tr>
                </table>
            </td>
            <td valign="top">
                <table width = "100%">
                    <tr><td><h4><%=GetLabel("Simulasi Perhitungan Jasa Medis :")%></h4></td></tr>
                    <tr>
                        <td>
                            <table>
                                <colgroup>
                                    <col style="width: 150px" />
                                    <col style="width: 100px" />
                                    <col style="width: 100px" />
                                </colgroup>
                                <tr>
                                    <td style="width: 100px"></td>
                                    <td align="center"><label class="lblNormal"><%=GetLabel("Tarif Dasar") %></label></td>
                                    <td align="center"><label class="lblNormal"><%=GetLabel("Tarif") %></label></td>
                                </tr>
                                <tr>
                                    <td><label class="lblNormal"><%=GetTariffComponent1Text()%></label></td>
                                    <td><asp:TextBox runat="server" CssClass="txtCurrency txtBaseComp" ID="txtPrevBaseComp1" Width="100%"/></td>
                                    <td><asp:TextBox runat="server" CssClass="txtCurrency txtTariffComp" ID="txtPrevTariffComp1" Width="100%" /></td>
                                </tr>
                                <tr>
                                    <td><label class="lblNormal"><%=GetTariffComponent2Text()%></label></td>
                                    <td><asp:TextBox runat="server" CssClass="txtCurrency txtBaseComp" Width="100%"/></td>
                                    <td><asp:TextBox runat="server" CssClass="txtCurrency txtTariffComp" ID="prevTariffComp2" Width="100%"/></td>
                                </tr>
                                <tr>
                                    <td><label class="lblNormal"><%=GetTariffComponent3Text()%></label></td>
                                    <td><asp:TextBox runat="server" CssClass="txtCurrency txtBaseComp" Width="100%"/></td>
                                    <td><asp:TextBox runat="server" CssClass="txtCurrency txtTariffComp" ID="prevTariffComp3" Width="100%"/></td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td><label class="lblNormal"><%=GetLabel("Total") %></label></td>
                                    <td><asp:TextBox runat="server" CssClass="txtCurrency" ID="txtPrevBaseTariffTotal" Width="100%"/></td>
                                    <td><asp:TextBox runat="server" CssClass="txtCurrency" ID="txtPrevTariffTotal" Width="100%"/></td>
                                </tr>
                                <tr>
                                    <td colspan="3"><input type="button" value="Hitung" id="btnCount" /></td>
                                </tr>
                                <tr>
                                    <td><label class="lblNormal"><%=GetLabel("Jumlah Pembagian") %></label></td>
                                    <td><asp:TextBox runat="server" CssClass="txtCurrency" ID="txtPrevBaseRevenue" Width="100px" ReadOnly="true" /></td>
                                </tr>

                                <asp:Repeater ID="rptFormulaTypePreview" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td class="tdLabel">
                                                <input type="hidden" id="hdnGCSharingComponent" value='<%#:Eval("StandardCodeID")%>' runat="server" />
                                                <label class="lblNormal"><%#:Eval("StandardCodeName")%></label>
                                            </td>
                                            <td><asp:TextBox ID="txtAmountBase" CssClass="txtCurrency txtPreviewDt" formulatype='<%#:Eval("cfStandardCodeID")%>' ReadOnly="true" Width="100px" runat="server" /></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <tr>
                                    <td colspan="2">
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td><label class="lblNormal"><%=GetLabel("Dokter") %></label></td>
                                    <td><asp:TextBox ID="txtDoctor" CssClass="txtCurrency" ReadOnly="true" Width="100px" runat="server"/></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
