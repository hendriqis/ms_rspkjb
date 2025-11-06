<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DrugAlertInformationCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Information.DrugAlertInformationCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<style type="text/css">
    #rightPanel
    {
        border: 1px solid #6E6E6E;
        width: 100%;
        height: 100%;
        position: relative;
    }
    #rightPanel > ul
    {
        margin: 0;
        padding: 2px;
        border-bottom: 1px groove black;
    }
    #rightPanel > ul > li
    {
        list-style-type: none;
        font-size: 12px;
        display: inline-block;
        border: 1px solid #848484;
        padding: 5px 8px;
        cursor: pointer;
    }
    #rightPanel > ul > li.selected
    {
        background-color: blue;
        color: White;
    }
    
    .viewClass
    {
        width: 700px;
        overflow: auto;
        height: 400px;
        overflow-y: auto;
    }
</style>
<script type="text/javascript" id="dxss_DrugAlertInformationctl">
    $(function () {
        $('#ulTabClinicTransaction li').live('click', function () {
            var name = $(this).attr('contentid');
            $(this).addClass('selected');
            $('#' + name).removeAttr('style');
            $('#ulTabClinicTransaction li').each(function () {
                var tempNameContainer = $(this).attr('contentid');
                if (tempNameContainer != name) {
                    $(this).removeClass('selected');
                    $('#' + tempNameContainer).attr('style', 'display:none');
                }
            });
        });
    });

    $('#btnYa').die('click');
    $('#btnYa').live('click', function () {
        displayConfirmationMessageBox('SEND ORDER : FARMASI', 'Kirim order resep ke farmasi ?', function (result) {
            if (result) {
                SaveFromDrugAlertPopUp();
                pcRightPanelContent.Hide();
            }
        });
    });

    $('#btnTidak').die('click');
    $('#btnTidak').live('click', function () {
        pcRightPanelContent.Hide();
    });
</script>
<input type="hidden" id="hdnPrescriptionOrderIDCtlDrugAlert" value="" runat="server" />
<table style="width: 100%" cellpadding="0" cellspacing="0">
    <colgroup>
        <col />
        <col style="width: 10px" />
        <col style="width: 36%" />
    </colgroup>
    <tr style="height: 300px" runat="server">
        <td style="height: 450px; width: 100%; vertical-align: top">
            <div id="rightPanel">
                <ul id="ulTabClinicTransaction">
                    <li class="selected" contentid="containerDrugInteraction" id="containerDrugInteraction"
                        runat="server" style="display:none">
                        <%=GetLabel("Drug Alert")%></li>
                    <li contentid="containerDrugDuplicate" id="containerDrugDuplicate" runat="server" style="display:none">
                        <%=GetLabel("Drug Duplicate")%></li>
                </ul>
                <div id="containerDrugInteraction" class="containerDrugInteraction">                    
                    <iframe style="width: 770px; height: 400px;border:0" src="<%= ResolveUrl("~/Libs/Scripts/MIMS/Mims.html")%>" title="">
                    </iframe>
                </div>
                <div id="containerLeftPanelContent">
                </div>
                <div id="divLeftPanelContentLoading" style="position: absolute; top: 30%; left: 48%;
                    display: none">
                    <div style="margin: 0 auto">
                        <img src="<%= ResolveUrl("~/Libs/Images/Loading.gif")%>" alt="" />
                    </div>
                </div>
            </div>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <table>
                <tr id="trButton" runat="server">
                    <td align="right" style="vertical-align: middle;" class="blink-alert">
                        <img height="40px" src='<%= ResolveUrl("~/Libs/Images/warning.png")%>' alt='' />
                    </td>
                    <td align="left" style="vertical-align: middle;">
                        <label class="lblWarning">
                            <%=GetLabel("Lanjutkan Proses ?")%></label>
                        <input type="button" id="btnYa" title='<%:GetLabel("Ya") %>' value="Ya" class="btnYa w3-button w3-green w3-border w3-border-blue w3-round-large" />
                        <input type="button" id="btnTidak" title='<%:GetLabel("Tidak") %>' value="Tidak"
                            class="btnTidak w3-button w3-red w3-border w3-border-blue w3-round-large" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
