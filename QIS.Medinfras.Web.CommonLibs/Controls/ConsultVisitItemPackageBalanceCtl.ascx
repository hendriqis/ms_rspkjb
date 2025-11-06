<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConsultVisitItemPackageBalanceCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ConsultVisitItemPackageBalanceCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dxpc" %>
<script type="text/javascript" id="dxss_ConsultVisitItemPackageBalanceCtl">

    $(function () {
        $('#leftPanel ul li').click(function () {
            $('#leftPanel ul li.selected').removeClass('selected');
            $(this).addClass('selected');
            var contentID = $(this).attr('contentID');
            showContent(contentID);
        });

        registerCollapseExpandHandler();

        $('#leftPanel ul li').first().click();
    });

    function showContent(contentID) {
        var i, x, tablinks;
        x = document.getElementsByClassName("divContent");
        for (i = 0; i < x.length; i++) {
            x[i].style.display = "none";
        }
        document.getElementById(contentID).style.display = "block";
    }

    //#region AIO Qty

    $('#ulTabGrdDetailAIOQty li').live('click', function () {
        $('#ulTabGrdDetailAIOQty li.selected').removeAttr('class');
        $('.containerGrdDtAIOQty').filter(':visible').hide();
        $contentID = $(this).attr('contentid');
        $('#' + $contentID).show();
        $(this).addClass('selected');
    });

    $('.grdBalanceAIOQty td.tdExpandBalanceAIOQty').die('click');
    $('.grdBalanceAIOQty td.tdExpandBalanceAIOQty').live('click', function () {
        $tr = $(this).parent();
        $trDetailAIOQty = $(this).parent().next();
        if ($trDetailAIOQty.attr('class') != 'trDetailAIOQty') {
            $('#ulTabGrdDetailAIOQty li:eq(0)').click();

            $trCollapse = $('.trDetailAIOQty');

            $(this).find('.imgExpandBalanceAIOQty').attr('src', '<%= ResolveUrl("~/Libs/Images/down-arrow.png")%>');
            $newTr = $("<tr><td></td><td colspan='15'></td></tr>").attr('class', 'trDetailAIOQty');
            $newTr.insertAfter($tr);
            $newTr.find('td').last().append($('#containerMovementDtAIOQty'));

            if ($trCollapse != null) {
                $trCollapse.prev().find('.imgExpandBalanceAIOQty').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');
                $trCollapse.remove();
            }

            $('.grdMovementAIOQty tr:gt(0)').remove();

            $('#<%=hdnExpandIDAIOQty.ClientID %>').val($tr.find('.keyField').html().trim());

            cbpViewAIOQtyMovement.PerformCallback('refresh');
        }
        else {
            $(this).find('.imgExpandBalanceAIOQty').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');
            $('#containerMovementHdAIOQty').append($('#containerMovementDtAIOQty'));

            $('.grdMovementAIOQty tr:gt(0)').remove();

            $trDetailAIOQty.remove();
        }
    });

    //#endregion

    //#region AIO Amount

    $('#ulTabGrdDetailAIOAmount li').live('click', function () {
        $('#ulTabGrdDetailAIOAmount li.selected').removeAttr('class');
        $('.containerGrdDtAIOAmount').filter(':visible').hide();
        $contentID = $(this).attr('contentid');
        $('#' + $contentID).show();
        $(this).addClass('selected');
    });

    $('.grdBalanceAIOAmount td.tdExpandBalanceAIOAmount').die('click');
    $('.grdBalanceAIOAmount td.tdExpandBalanceAIOAmount').live('click', function () {
        $tr = $(this).parent();
        $trDetailAIOAmount = $(this).parent().next();
        if ($trDetailAIOAmount.attr('class') != 'trDetailAIOAmount') {
            $('#ulTabGrdDetailAIOAmount li:eq(0)').click();

            $trCollapse = $('.trDetailAIOAmount');

            $(this).find('.imgExpandBalanceAIOAmount').attr('src', '<%= ResolveUrl("~/Libs/Images/down-arrow.png")%>');
            $newTr = $("<tr><td></td><td colspan='15'></td></tr>").attr('class', 'trDetailAIOAmount');
            $newTr.insertAfter($tr);
            $newTr.find('td').last().append($('#containerMovementDtAIOAmount'));

            if ($trCollapse != null) {
                $trCollapse.prev().find('.imgExpandBalanceAIOAmount').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');
                $trCollapse.remove();
            }

            $('.grdMovementAIOAmount tr:gt(0)').remove();

            $('#<%=hdnExpandIDAIOAmount.ClientID %>').val($tr.find('.keyField').html().trim());

            cbpViewAIOAmountMovement.PerformCallback('refresh');
        }
        else {
            $(this).find('.imgExpandBalanceAIOAmount').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');
            $('#containerMovementHdAIOAmount').append($('#containerMovementDtAIOAmount'));

            $('.grdMovementAIOAmount tr:gt(0)').remove();

            $trDetailAIOAmount.remove();
        }
    });

    //#endregion

</script>
<style type="text/css">
    #leftPanel
    {
        border: 1px solid #6E6E6E;
        width: 100%;
        height: 100%;
        position: relative;
    }
    #leftPanel > ul
    {
        margin: 0;
        padding: 2px;
        border-bottom: 1px groove black;
    }
    #leftPanel > ul > li
    {
        list-style-type: none;
        font-size: 15px;
        display: list-item;
        border: 1px solid #fdf5e6 !important;
        padding: 5px 8px;
        cursor: pointer;
        background-color: #87CEEB !important;
    }
    #leftPanel > ul > li.selected
    {
        background-color: #ff5722 !important;
        color: White;
    }
    .divContent
    {
        padding-left: 3px;
        min-height: 500px;
    }
</style>
<div style="overflow-y: hidden; overflow-x: hidden">
    <input type="hidden" value="" runat="server" id="hdnVisitIDCtlPU" />
    <input type="hidden" value="" runat="server" id="hdnRegistrationIDCtlPU" />
    <input type="hidden" value="" runat="server" id="hdnDtID" />
    <input type="hidden" value="" runat="server" id="hdnExpandIDAIOQty" />
    <input type="hidden" value="" runat="server" id="hdnExpandIDAIOAmount" />
    <table style="width: 50%">
        <colgroup>
            <col style="width: 120px" />
            <col style="width: 150px" />
            <col style="width: 300px" />
            <col />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("No. Registrasi")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtRegistrationNoCVIPBalanceCtl" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Paket AIO")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtItemCode" ReadOnly="true" Width="100%" runat="server" />
            </td>
            <td>
                <asp:TextBox ID="txtItemName1" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
    </table>
    <table style="width: 100%">
        <colgroup>
            <col style="width: 150px" />
            <col />
        </colgroup>
        <tr>
            <td style="vertical-align: top">
                <div id="leftPanel" class="w3-border">
                    <ul>
                        <li contentid="divPageBalanceQty" title="Balance Qty" class="w3-hover-red">
                            <%=GetLabel("Balance Qty") %></li>
                        <li contentid="divPageBalanceAmount" title="Balance Amount" class="w3-hover-red">
                            <%=GetLabel("Balance Amount") %></li>
                    </ul>
                </div>
            </td>
            <td>
                <div id="divPageBalanceQty" class="w3-border divContent w3-animate-left" style="display: none">
                    <div id="containerBalanceAIOQty">
                        <dxcp:ASPxCallbackPanel ID="cbpViewAIOQty" runat="server" Width="100%" ClientInstanceName="cbpViewAIOQty"
                            ShowLoadingPanel="false">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1AIOQTY" runat="server">
                                    <asp:Panel runat="server" ID="pnlViewAIOQTY" CssClass="pnlContainerGrid">
                                        <table class="grdBalanceAIOQty grdNormal" cellspacing="0" width="100%" rules="all">
                                            <tr>
                                                <th class="keyField" style="width: 1px">
                                                </th>
                                                <th style="width: 15px">
                                                </th>
                                                <th style="width: 150px">
                                                    <div>
                                                        <%=GetLabel("Detail Unit")
                                                        %></div>
                                                </th>
                                                <th>
                                                    <div>
                                                        <%=GetLabel("Detail Item") %></div>
                                                </th>
                                                <th style="width: 100px; text-align: right">
                                                    <div>
                                                        <%=GetLabel("BEGIN") %></div>
                                                </th>
                                                <th style="width: 100px; text-align: right">
                                                    <div>
                                                        <%=GetLabel("IN") %></div>
                                                </th>
                                                <th style="width: 100px; text-align: right">
                                                    <div>
                                                        <%=GetLabel("OUT") %></div>
                                                </th>
                                                <th style="width: 100px; text-align: right">
                                                    <div>
                                                        <%=GetLabel("END") %></div>
                                                </th>
                                                <th style="width: 100px; text-align: center">
                                                    <div>
                                                        <%=GetLabel("Dibuat Oleh") %></div>
                                                </th>
                                                <th style="width: 100px; text-align: center">
                                                    <div>
                                                        <%=GetLabel("Diubah Oleh") %></div>
                                                </th>
                                            </tr>
                                            <asp:ListView ID="lvwViewAIOQty" runat="server">
                                                <EmptyDataTemplate>
                                                    <tr class="trEmpty">
                                                        <td colspan="15">
                                                            <%=GetLabel("---tidak ada data---") %>
                                                        </td>
                                                    </tr>
                                                </EmptyDataTemplate>
                                                <ItemTemplate>
                                                    <tr style="<%#: Eval("cfBalanceENDInString").ToString() == "0.00" ? "background-color:#9ffffb" : ""%>">
                                                        <td class="keyField">
                                                            <%#: Eval("DtID")%>
                                                        </td>
                                                        <td align="center" class="tdExpandBalanceAIOQty">
                                                            <img class="imgExpandBalanceAIOQty imgLink" src='<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>'
                                                                alt='' />
                                                        </td>
                                                        <td>
                                                            <div style="font-size: x-small">
                                                                <i>
                                                                    <%#:Eval("DtDepartmentID")%></i></div>
                                                            <div style="font-size: small">
                                                                <b>
                                                                    <%#:Eval("DtServiceUnitName") %></b></div>
                                                            <div style="font-size: x-small">
                                                                <%#:Eval("DtServiceUnitCode") %></div>
                                                        </td>
                                                        <td>
                                                            <div style="font-size: xx-small">
                                                                <%#:Eval("DtItemType") %></div>
                                                            <div>
                                                                <b>
                                                                    <%#:Eval("DtItemName1")%></b>
                                                                <img class="lblIsControlAmount" title="<%=GetLabel("Kontrol Batasan Nilai Obat Alkes") %>"
                                                                    src='<%# ResolveUrl("~/Libs/Images/Status/coverage_ok.png")%>' alt="" style='<%# Eval("IsControlAmount").ToString() == "True" ? "": "display:none"%>'
                                                                    width="20px" />
                                                            </div>
                                                            <div style="font-size: x-small">
                                                                <i>
                                                                    <%#:Eval("DtItemCode")%></i></div>
                                                        </td>
                                                        <td align="right">
                                                            <div>
                                                                <b>
                                                                    <%#:Eval("cfBalanceBEGINInString")%></b></div>
                                                        </td>
                                                        <td align="right">
                                                            <div style="color: Blue">
                                                                <b>
                                                                    <%#:Eval("cfBalanceINInString") %></b></div>
                                                        </td>
                                                        <td align="right">
                                                            <div style="color: Maroon">
                                                                <b>
                                                                    <%#:Eval("cfBalanceOUTInString") %></b></div>
                                                        </td>
                                                        <td align="right" style="<%#: Eval("cfBalanceENDInString").ToString() == "0.00" ? "background-color: #ffa592" : "background-color: #adff84"%>">
                                                            <div>
                                                                <b>
                                                                    <%#:Eval("cfBalanceENDInString") %></b></div>
                                                        </td>
                                                        <td align="center">
                                                            <div style="font-size: x-small">
                                                                <%#:Eval("CreatedByName") %></div>
                                                            <div style="font-size: xx-small">
                                                                <%#:Eval("cfCreatedDateInStringFull") %></div>
                                                        </td>
                                                        <td align="center">
                                                            <div style="font-size: x-small">
                                                                <%#:Eval("LastUpdatedByName")%></div>
                                                            <div style="font-size: xx-small">
                                                                <%#:Eval("cfLastUpdatedDateInStringFull")%></div>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </table>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                    </div>
                    <div id="containerMovementHdAIOQty" style="position: relative; display: none">
                        <div id="containerMovementDtAIOQty" class="borderBox" style="width: 100%; padding: 10px 5px;">
                            <div class="containerUlTabPage">
                                <ul class="ulTabPage" id="ulTabGrdDetailAIOQty">
                                    <li class="selected" contentid="containerMovementAIOQty">
                                        <%=GetLabel("AIO Movement") %></li>
                                </ul>
                            </div>
                            <div style="position: relative;">
                                <div id="containerMovementAIOQty" class="containerAIOQty" style="display: none">
                                    <dxcp:ASPxCallbackPanel ID="cbpViewAIOQtyMovement" runat="server" Width="100%" ClientInstanceName="cbpViewAIOQtyMovement"
                                        ShowLoadingPanel="false" OnCallback="cbpViewAIOQtyMovement_Callback">
                                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent5AIOQTY" runat="server">
                                                <asp:Panel runat="server" ID="Panel4AIOQTY" CssClass="pnlContainerGrid" Style="height: auto">
                                                    <table class="grdMovementAIOQty grdSelected" cellspacing="0" width="100%" rules="all">
                                                        <tr>
                                                            <th style="width: 150px; text-align: left">
                                                                <div>
                                                                    <%=GetLabel("No/Tanggal Transaksi") %></div>
                                                            </th>
                                                            <th style="text-align: left">
                                                                <div>
                                                                    <%=GetLabel("Unit Transaksi") %></div>
                                                            </th>
                                                            <th style="width: 100px; text-align: right">
                                                                <div>
                                                                    <%=GetLabel("BEGIN") %></div>
                                                            </th>
                                                            <th style="width: 100px; text-align: right">
                                                                <div>
                                                                    <%=GetLabel("IN") %></div>
                                                            </th>
                                                            <th style="width: 100px; text-align: right">
                                                                <div>
                                                                    <%=GetLabel("OUT")%></div>
                                                            </th>
                                                            <th style="width: 100px; text-align: right">
                                                                <div>
                                                                    <%=GetLabel("END") %></div>
                                                            </th>
                                                            <th style="width: 200px; text-align: center">
                                                                <div>
                                                                    <%=GetLabel("Informasi Mutasi") %></div>
                                                            </th>
                                                        </tr>
                                                        <asp:ListView runat="server" ID="lvwMovementAIOQty">
                                                            <EmptyDataTemplate>
                                                                <tr class="trEmpty">
                                                                    <td colspan="15">
                                                                        <%=GetLabel("---tidak ada data---") %>
                                                                    </td>
                                                                </tr>
                                                            </EmptyDataTemplate>
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td style="<%#: Eval("PatientChargesHdGCTransactionStatus").ToString() == "X121^999" ? "background-color:#fc919b" : ""%>">
                                                                        <div>
                                                                            <b>
                                                                                <%#:Eval("PatientChargesHdTransactionNo")%></b>
                                                                        </div>
                                                                        <div style="font-size: x-small">
                                                                            <%#:Eval("cfPatientChargesHdTransactionDateTimeInFullString")%>
                                                                        </div>
                                                                        <div style="<%#: Eval("PatientChargesHdTransactionStatus").ToString() == "" ? "display:none" : "font-size: xx-small"%>">
                                                                            <%=GetLabel("Status Transaksi = ") %><%#:Eval("PatientChargesHdTransactionStatus")%>
                                                                        </div>
                                                                        <div style="<%#: Eval("cfIsDeletedInformation").ToString() == "" ? "display:none" : "font-size: xx-small"%>">
                                                                            <%#:Eval("cfIsDeletedInformation")%>
                                                                        </div>
                                                                    </td>
                                                                    <td>
                                                                        <div>
                                                                            <%#:Eval("PatientChargesHdServiceUnitName") %></div>
                                                                    </td>
                                                                    <td align="right">
                                                                        <div>
                                                                            <%#:Eval("cfBalanceBEGINInString") %></div>
                                                                    </td>
                                                                    <td align="right">
                                                                        <div style="color: Blue">
                                                                            <%#:Eval("cfBalanceINInString") %></div>
                                                                    </td>
                                                                    <td align="right">
                                                                        <div style="color: Maroon">
                                                                            <%#:Eval("cfBalanceOUTInString") %></div>
                                                                    </td>
                                                                    <td align="right">
                                                                        <div>
                                                                            <%#:Eval("cfBalanceENDInString") %></div>
                                                                    </td>
                                                                    <td align="center">
                                                                        <div style="font-size: x-small">
                                                                            <%#:Eval("CreatedByName") %></div>
                                                                        <div style="font-size: xx-small">
                                                                            <%#:Eval("cfCreatedDateInStringFull") %></div>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:ListView>
                                                    </table>
                                                </asp:Panel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dxcp:ASPxCallbackPanel>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="divPageBalanceAmount" class="w3-border divContent w3-animate-left" style="display: none">
                    <div id="containerBalanceAIOAmount">
                        <dxcp:ASPxCallbackPanel ID="cbpViewAIOAmount" runat="server" Width="100%" ClientInstanceName="cbpViewAIOAmount"
                            ShowLoadingPanel="false">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1AIOAmount" runat="server">
                                    <asp:Panel runat="server" ID="pnlViewAIOAmount" CssClass="pnlContainerGrid">
                                        <table class="grdBalanceAIOAmount grdNormal" cellspacing="0" width="100%" rules="all">
                                            <tr>
                                                <th class="keyField" style="width: 1px">
                                                </th>
                                                <th style="width: 15px">
                                                </th>
                                                <th style="width: 120px">
                                                    <div>
                                                        <%=GetLabel("Detail Unit")
                                                        %></div>
                                                </th>
                                                <th>
                                                    <div>
                                                        <%=GetLabel("Detail Item") %></div>
                                                </th>
                                                <th style="width: 120px; text-align: right">
                                                    <div>
                                                        <%=GetLabel("BEGIN") %></div>
                                                </th>
                                                <th style="width: 120px; text-align: right">
                                                    <div>
                                                        <%=GetLabel("IN") %></div>
                                                </th>
                                                <th style="width: 120px; text-align: right">
                                                    <div>
                                                        <%=GetLabel("OUT") %></div>
                                                </th>
                                                <th style="width: 120px; text-align: right">
                                                    <div>
                                                        <%=GetLabel("END") %></div>
                                                </th>
                                                <th style="width: 100px; text-align: center">
                                                    <div>
                                                        <%=GetLabel("Dibuat Oleh") %></div>
                                                </th>
                                                <th style="width: 100px; text-align: center">
                                                    <div>
                                                        <%=GetLabel("Diubah Oleh") %></div>
                                                </th>
                                            </tr>
                                            <asp:ListView ID="lvwViewAIOAmount" runat="server">
                                                <EmptyDataTemplate>
                                                    <tr class="trEmpty">
                                                        <td colspan="15">
                                                            <%=GetLabel("---tidak ada data---") %>
                                                        </td>
                                                    </tr>
                                                </EmptyDataTemplate>
                                                <ItemTemplate>
                                                    <tr style="<%#: Eval("cfBalanceENDInString").ToString() == "0.00" ? "background-color:#9ffffb" : ""%>">
                                                        <td class="keyField">
                                                            <%#: Eval("DtID")%>
                                                        </td>
                                                        <td align="center" class="tdExpandBalanceAIOAmount">
                                                            <img class="imgExpandBalanceAIOAmount imgLink" src='<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>'
                                                                alt='' />
                                                        </td>
                                                        <td>
                                                            <div style="font-size: x-small">
                                                                <i>
                                                                    <%#:Eval("DtDepartmentID")%></i></div>
                                                            <div style="font-size: small">
                                                                <b>
                                                                    <%#:Eval("DtServiceUnitName") %></b></div>
                                                            <div style="font-size: x-small">
                                                                <%#:Eval("DtServiceUnitCode") %></div>
                                                        </td>
                                                        <td>
                                                            <div style="font-size: xx-small">
                                                                <%#:Eval("DtItemType") %></div>
                                                            <div>
                                                                <b>
                                                                    <%#:Eval("DtItemName1")%></b>
                                                                <img class="lblIsControlAmount" title="<%=GetLabel("Kontrol Batasan Nilai Obat Alkes") %>"
                                                                    src='<%# ResolveUrl("~/Libs/Images/Status/coverage_ok.png")%>' alt="" style='<%# Eval("IsControlAmount").ToString() == "True" ? "": "display:none"%>'
                                                                    width="20px" />
                                                            </div>
                                                            <div style="font-size: x-small">
                                                                <i>
                                                                    <%#:Eval("DtItemCode")%></i></div>
                                                        </td>
                                                        <td align="right">
                                                            <div>
                                                                <b>
                                                                    <%#:Eval("cfBalanceBEGINInString")%></b></div>
                                                        </td>
                                                        <td align="right">
                                                            <div style="color: Blue">
                                                                <b>
                                                                    <%#:Eval("cfBalanceINInString") %></b></div>
                                                        </td>
                                                        <td align="right">
                                                            <div style="color: Maroon">
                                                                <b>
                                                                    <%#:Eval("cfBalanceOUTInString") %></b></div>
                                                        </td>
                                                        <td align="right" style="<%#: Eval("cfBalanceENDInString").ToString() == "0.00" ? "background-color: #ffa592" : "background-color: #adff84"%>">
                                                            <div>
                                                                <b>
                                                                    <%#:Eval("cfBalanceENDInString") %></b></div>
                                                        </td>
                                                        <td align="center">
                                                            <div style="font-size: x-small">
                                                                <%#:Eval("CreatedByName") %></div>
                                                            <div style="font-size: xx-small">
                                                                <%#:Eval("cfCreatedDateInStringFull") %></div>
                                                        </td>
                                                        <td align="center">
                                                            <div style="font-size: x-small">
                                                                <%#:Eval("LastUpdatedByName")%></div>
                                                            <div style="font-size: xx-small">
                                                                <%#:Eval("cfLastUpdatedDateInStringFull")%></div>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </table>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                    </div>
                    <div id="containerMovementHdAIOAmount" style="position: relative; display: none">
                        <div id="containerMovementDtAIOAmount" class="borderBox" style="width: 100%; padding: 10px 5px;">
                            <div class="containerUlTabPage">
                                <ul class="ulTabPage" id="ulTabGrdDetailAIOAmount">
                                    <li class="selected" contentid="containerMovementAIOAmount">
                                        <%=GetLabel("AIO Movement") %></li>
                                </ul>
                            </div>
                            <div style="position: relative;">
                                <div id="containerMovementAIOAmount" class="containerAIOAmount" style="display: none">
                                    <dxcp:ASPxCallbackPanel ID="cbpViewAIOAmountMovement" runat="server" Width="100%"
                                        ClientInstanceName="cbpViewAIOAmountMovement" ShowLoadingPanel="false" OnCallback="cbpViewAIOAmountMovement_Callback">
                                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent5AIOAmount" runat="server">
                                                <asp:Panel runat="server" ID="Panel4AIOAmount" CssClass="pnlContainerGrid" Style="height: auto">
                                                    <table class="grdMovementAIOAmount grdSelected" cellspacing="0" width="100%" rules="all">
                                                        <tr>
                                                            <th style="width: 120px; text-align: left">
                                                                <div>
                                                                    <%=GetLabel("No/Tanggal Transaksi") %></div>
                                                            </th>
                                                            <th style="text-align: left">
                                                                <div>
                                                                    <%=GetLabel("Unit Transaksi") %></div>
                                                            </th>
                                                            <th style="width: 120px; text-align: right">
                                                                <div>
                                                                    <%=GetLabel("BEGIN") %></div>
                                                            </th>
                                                            <th style="width: 120px; text-align: right">
                                                                <div>
                                                                    <%=GetLabel("IN") %></div>
                                                            </th>
                                                            <th style="width: 120px; text-align: right">
                                                                <div>
                                                                    <%=GetLabel("OUT")%></div>
                                                            </th>
                                                            <th style="width: 120px; text-align: right">
                                                                <div>
                                                                    <%=GetLabel("END") %></div>
                                                            </th>
                                                            <th style="width: 200px; text-align: center">
                                                                <div>
                                                                    <%=GetLabel("Informasi Mutasi") %></div>
                                                            </th>
                                                        </tr>
                                                        <asp:ListView runat="server" ID="lvwMovementAIOAmount">
                                                            <EmptyDataTemplate>
                                                                <tr class="trEmpty">
                                                                    <td colspan="15">
                                                                        <%=GetLabel("---tidak ada data---") %>
                                                                    </td>
                                                                </tr>
                                                            </EmptyDataTemplate>
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td style="<%#: Eval("PatientChargesHdGCTransactionStatus").ToString() == "X121^999" || Eval("cfIsUnlinkChargesTariffInformation").ToString() != "" ? "background-color:#fc919b" : ""%>">
                                                                        <div>
                                                                            <b>
                                                                                <%#:Eval("PatientChargesHdTransactionNo")%></b>
                                                                        </div>
                                                                        <div style="font-size: x-small">
                                                                            <%#:Eval("cfPatientChargesHdTransactionDateTimeInFullString")%>
                                                                        </div>
                                                                        <div style="<%#: Eval("PatientChargesHdTransactionStatus").ToString() == "" ? "display:none" : "font-size: xx-small"%>">
                                                                            <%=GetLabel("Status Transaksi = ") %><%#:Eval("PatientChargesHdTransactionStatus")%>
                                                                        </div>
                                                                        <div style="<%#: Eval("cfIsDeletedInformation").ToString() == "" ? "display:none" : "font-size: xx-small"%>">
                                                                            <%#:Eval("cfIsDeletedInformation")%>
                                                                        </div>
                                                                        <div style="<%#: Eval("cfIsUnlinkChargesTariffInformation").ToString() == "" ? "display:none" : "font-size: xx-small"%>">
                                                                            <%#:Eval("cfIsUnlinkChargesTariffInformation")%>
                                                                        </div>
                                                                    </td>
                                                                    <td>
                                                                        <div>
                                                                            <%#:Eval("PatientChargesHdServiceUnitName") %></div>
                                                                    </td>
                                                                    <td align="right">
                                                                        <div>
                                                                            <%#:Eval("cfBalanceBEGINInString") %></div>
                                                                    </td>
                                                                    <td align="right">
                                                                        <div style="color: Blue">
                                                                            <%#:Eval("cfBalanceINInString") %></div>
                                                                    </td>
                                                                    <td align="right">
                                                                        <div style="color: Maroon">
                                                                            <%#:Eval("cfBalanceOUTInString") %></div>
                                                                    </td>
                                                                    <td align="right">
                                                                        <div>
                                                                            <%#:Eval("cfBalanceENDInString") %></div>
                                                                    </td>
                                                                    <td align="center">
                                                                        <div style="font-size: x-small">
                                                                            <%#:Eval("CreatedByName") %></div>
                                                                        <div style="font-size: xx-small">
                                                                            <%#:Eval("cfCreatedDateInStringFull") %></div>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:ListView>
                                                    </table>
                                                </asp:Panel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dxcp:ASPxCallbackPanel>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
