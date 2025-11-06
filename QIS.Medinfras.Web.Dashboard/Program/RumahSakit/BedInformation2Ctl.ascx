<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BedInformation2Ctl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Dashboard.Program.BedInformation2Ctl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<div class="main-wrapper">
    <main class="main users chart-page" id="skip-target">
        <div class="containerdb">
            <div class="d-sm-flex align-items-center justify-content-between m-2"></div>
            <div class="bedCircleProgress">
                <div class="circleProgress">
                    <span class="circleValue">
                        0
                    </span>
                </div>
            </div>
        </div>
    </main>
</div>

<script type="text/javascript" id="dxss_bedinformationctl">
    circleProgress = document.querySelector(".circleProgress");
    circleValue = document.querySelector(".circleValue");

    progressStart = 0;
    var progressEnd = 100;
    speed = 150;

    progress = setInterval(() => {
        progressStart++;
        circleValue.textContent = `${progressStart}`;
        circleProgress.style.background = `conic-gradient(#0061f7 ${progressStart * 3.6}deg, #ededed 0deg)`;
        if(progressStart == progressEnd) {
            clearInterval(progress);
        }
    }, speed);
</script>
