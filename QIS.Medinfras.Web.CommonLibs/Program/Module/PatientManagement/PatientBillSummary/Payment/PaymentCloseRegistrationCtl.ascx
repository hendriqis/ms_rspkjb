<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PaymentCloseRegistrationCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PaymentCloseRegistrationCtl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
    
<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    $(function () {
        $('#btnPopupYes').click(function () {
            cbpProcessPopup.PerformCallback();
        });
    });

    function onCbpProcessPopupEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'fail')
            showToast('Process Failed', 'Error Message : ' + param[1]);
        else {
            var count = parseInt(s.cpCount);
            for (var i = 0; i < count; ++i)
                onAfterAddRecordAddRowCount();
            exitPatientPage();
            pcRightPanelContent.Hide();
        }

        hideLoadingPanel();
    }
     
</script>
<input type="hidden" id="hdnRegistrationID" value="" runat="server" />

<div style="margin: 5px;">
    <%=GetLabel("Tagihan Pasien Sudah Lunas. Tutup Pendaftaran?") %> 
    <input type="button" id="btnPopupYes" value='<%=GetLabel("Ya") %>' />
    <input type="button" value='<%=GetLabel("Tidak") %>' onclick="pcRightPanelContent.Hide();" />
</div>

<dxcp:ASPxCallbackPanel runat="server" ID="cbpProcessPopup" ClientInstanceName="cbpProcessPopup" ShowLoadingPanel="false"
    OnCallback="cbpProcessPopup_Callback">
    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { onCbpProcessPopupEndCallback(s); }" />
</dxcp:ASPxCallbackPanel>

