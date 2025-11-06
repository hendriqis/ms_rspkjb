var txtForm = document.getElementsByClassName("txtForm");
            if (txtForm.length > 0) {
                for (var i = 0; i < txtForm.length; i++) {
                    var num = txtForm[i].getAttribute("number");
                    if (num) {
                        txtForm[i].addEventListener('keyup', function (e) {
                            this.value = this.value.replace(/[^0-9.]/g, '').replace(/(\..*?)\..*/g, '$1').replace(/^0[^.]/, '0');
                        }, false);
                    }
                }
            }
         
			function openDefault(){
				var txtForm = document.getElementsByClassName("txtForm");
                for (var i = 0; i < txtForm.length; i++) {
                    var id = txtForm[i].getAttribute("id");
					var sortID = txtForm[i].getAttribute("sortid"); 
                    switch (sortID) {
                        case "2":
                            document.getElementById(id).disabled = true; 
                            break;
                        case "3":
                            document.getElementById(id).disabled = true; 
                            break;
                        case "4":
                            document.getElementById(id).disabled = true;
                            break;
                        case "7":
                            document.getElementById(id).disabled = true;
                            break;

					    case "10": 
						    document.getElementById(id).disabled = true; 
						    break; 
					    case "12":
						    document.getElementById(id).disabled = true; 
						    break;
					    case "14":
						    document.getElementById(id).disabled = true; 
						 
						    break;
					    case "16":
						    document.getElementById(id).disabled = true; 
						 
						    break;
					    case "18":
						    document.getElementById(id).disabled = true; 
						 
						    break;
					    case "20":
						    document.getElementById(id).disabled = true; 
						 
						    break;
					    case "22":
						    document.getElementById(id).disabled = true; 
					 
						    break;
					    case "24":
						    document.getElementById(id).disabled = true; 
					 
						    break;
					    case "26":
						    document.getElementById(id).disabled = true; 
					 
						    break;
					    case "28":
						    document.getElementById(id).disabled = true; 
						    break;
					    case "30":
						    document.getElementById(id).disabled = true; 
						    break; 
					    case "32":
						    document.getElementById(id).disabled = true; 
						    break;
					 
					    case "34":
						    document.getElementById(id).disabled = true; 
						    break;
					 
					    case "35":
						    document.getElementById(id).disabled = true; 
						    break;
					    case "42":
						    document.getElementById(id).disabled = true; 
						    break;
					    case "44":
						    document.getElementById(id).disabled = true; 
						    break;
					    case "105":
						    document.getElementById(id).disabled = true; 
						    break;
					    case "133":
						    document.getElementById(id).disabled = true; 
						    break;
					    case "135":
						    document.getElementById(id).disabled = true; 
						    break;
					    case "137": 
						    document.getElementById(id).disabled = true; 
                            break; 
                        case "144":
                            document.getElementById(id).disabled = true;
                            break;
                        case "148":
                            document.getElementById(id).disabled = true;
                            break;
                        case "153":
                            document.getElementById(id).disabled = true;
                            break;
                        case "209":
                            document.getElementById(id).disabled = true;
                            break;  
					    case "210":
						    document.getElementById(id).disabled = true; 
                            break; 
                        
                        case "156":
                            document.getElementById(id).disabled = true;
                            break; 

					}
                }
				var ddlForm = document.getElementsByClassName("ddlForm");
				for(var i=0; i < ddlForm.length; i++){
					var id = ddlForm[i].getAttribute("id");
					var sortID = ddlForm[i].getAttribute("sortid"); 
                    switch (sortID) {
                        case "5":
                            document.getElementById(id).disabled = true;
                            break; 
					    case "9": 
						    document.getElementById(id).disabled = true; 
                            break; 
                       
					}
				}
			}
			//untuk edit baca sini : 
            function changeCbo(sortID, value) {
              
                var IsDisabled = true;
                if (value == "Ya" || value == "Abnormal" || value == "Luar Kerja" || value == "Dalam Kerja" 
                    || value == "Abnormal" || value == "Diduga" || value == "Ada") {
						IsDisabled = false; 
                } 
               
                switch (sortID) {
                    case "1":
                        document.getElementById("txtParam1").disabled = IsDisabled;
                        if (IsDisabled) {
                            document.getElementById("txtParam1").value = "";
                        }
                        break;
                    case "3":
                        document.getElementById("txtParam2").disabled = IsDisabled;
                        document.getElementById("ddlParameter3").disabled = IsDisabled;
                        if (IsDisabled) {
                            document.getElementById("txtParam2").value = "";
                            document.getElementById("ddlParameter3").value = "-";
                        }
                        break;
                   
                    case "6":
                        document.getElementById("txtParam3").disabled = IsDisabled;
                        if (IsDisabled) {
                            document.getElementById("txtParam3").value = "";
                        }
                        break;
                    case "8":
								document.getElementById("ddlParameter6").disabled = IsDisabled; 
								if(IsDisabled){
									document.getElementById("ddlParameter6").value = "Tidak Ada";
								}
                        break;
                    case "9":
                        document.getElementById("txtParam4").disabled = IsDisabled;
                        if (IsDisabled) {
                            document.getElementById("txtParam4").value = "";
                        }
                        break; 
							case "11":
								document.getElementById("txtParam5").disabled = IsDisabled; 
								if(IsDisabled){
									document.getElementById("txtParam5").value = "0";
								}
								break;
							case "13":
								document.getElementById("txtParam6").disabled = IsDisabled; 
								if(IsDisabled){
									document.getElementById("txtParam6").value = "0";
								}
								break;
							case "15":
								document.getElementById("txtParam7").disabled = IsDisabled; 
								if(IsDisabled){
									document.getElementById("txtParam7").value = "0";
								}
								break;
							case "17":
								document.getElementById("txtParam8").disabled = IsDisabled; 
								if(IsDisabled){
									document.getElementById("txtParam8").value = "0";
								}
								break;
							case "19":
								document.getElementById("txtParam9").disabled = IsDisabled; 
								if(IsDisabled){
									document.getElementById("txtParam9").value = "";
								}
								break;
							case "21":
								document.getElementById("txtParam10").disabled = IsDisabled; 
								if(IsDisabled){
									document.getElementById("txtParam10").value = "";
								}
								break;
							case "23":
								document.getElementById("txtParam11").disabled = IsDisabled; 
								if(IsDisabled){
									document.getElementById("txtParam11").value = "";
								}
								break;
							case "25":
								document.getElementById("txtParam12").disabled = IsDisabled; 
								if(IsDisabled){
									document.getElementById("txtParam12").value = "";
								}
								break;
							case "27":
								document.getElementById("txtParam13").disabled = IsDisabled; 
								if(IsDisabled){
									document.getElementById("txtParam13").value = "";
								}
								break;
							case "29":
								document.getElementById("txtParam14").disabled = IsDisabled; 
								if(IsDisabled){
									document.getElementById("txtParam14").value = "";
								}
								break;
							case "31":
								document.getElementById("txtParam21").disabled = IsDisabled; 
								if(IsDisabled){
									document.getElementById("txtParam21").value = "";
								}
								break; 
							case "33":
								document.getElementById("txtParam22").disabled = IsDisabled; 
								if(IsDisabled){
									document.getElementById("txtParam22").value = "";
								}
								document.getElementById("txtParam23").disabled = IsDisabled; 
								if(IsDisabled){
									document.getElementById("txtParam23").value = "";
								}
								break;
							case "41":
								document.getElementById("txtParam28").disabled = IsDisabled; 
								if(IsDisabled){
									document.getElementById("txtParam28").value = "";
								}
								break; 
							case "43":
								document.getElementById("txtParam29").disabled = IsDisabled; 
								if(IsDisabled){
									document.getElementById("txtParam29").value = "";
								}
								break; 
							case "104":
                                document.getElementById("txtParam34").disabled = IsDisabled; 
								if(IsDisabled){
                                    document.getElementById("txtParam34").value = "";
								}
                            break;
                    
							case "131":
								document.getElementById("txtParam51").disabled = IsDisabled; 
								if(IsDisabled){
									document.getElementById("txtParam51").value = "";
								}
								break;
							case "132":
								document.getElementById("txtParam52").disabled = IsDisabled; 
								if(IsDisabled){
									document.getElementById("txtParam52").value = "";
								}
								break;
							case "134":
								document.getElementById("txtParam53").disabled = IsDisabled; 
								if(IsDisabled){
									document.getElementById("txtParam53").value = "";
								}
								break;
							case "136":
								document.getElementById("txtParam54").disabled = IsDisabled; 
								if(IsDisabled){
									document.getElementById("txtParam54").value = "";
								}
                        break;
                    case "143":
                        if (value == "Normal") {
                            IsDisabled = false;
                        }
                        document.getElementById("txtParam63").disabled = IsDisabled;
                        if (IsDisabled) {
                            document.getElementById("txtParam63").value = "";
                        }
                        break;
                    case "147":
                        if (value == "Normal") {
                            IsDisabled = false;
                        }
                        document.getElementById("txtParam64").disabled = IsDisabled;
                        if (IsDisabled) {
                            document.getElementById("txtParam64").value = "";
                        }
                        break;
                    case "155":
                        document.getElementById("txtParam70").disabled = IsDisabled;
                        if (IsDisabled) {
                            document.getElementById("txtParam70").value = "";
                        }
                        break;
                   
                    case "154":
                        document.getElementById("txtParam610").disabled = IsDisabled;
                        if (IsDisabled) {
                            document.getElementById("txtParam610").value = "";
                        }
                        break;
                   
						}
            }
           
			function editValue(){
				 var ddlForm = document.getElementsByClassName("ddlForm");
				for (var i = 0; i < ddlForm.length; i++) {
					var id = ddlForm[i].getAttribute("id");
					let select = document.getElementById(id);
					
					var value = select.value;		
					var sortID =select.getAttribute("sortid"); 
					var IsDisabled = true;
					 if(value == "Ya" || value == "Abnormal"){
						IsDisabled = false; 
					 } 
                    switch (sortID) {
                        case "1":
                            document.getElementById("txtParam1").disabled = IsDisabled;
                            if (IsDisabled) {
                                document.getElementById("txtParam1").value = "";
                            }
                            break; 
                        case "2":
                            document.getElementById("txtParam1").disabled = IsDisabled;
                            if (IsDisabled) {
                                document.getElementById("txtParam1").value = "";
                            }
                            break;
                        case "3":
                            document.getElementById("txtParam2").disabled = IsDisabled;
                            if (IsDisabled) {
                                document.getElementById("txtParam2").value = "";
                            }
                           
                            document.getElementById("ddlParameter3").disabled = IsDisabled;
                            if (IsDisabled) {
                                document.getElementById("ddlParameter3").value = "-";
                            }
                            break;
                        case "6":
                            document.getElementById("txtParam3").disabled = IsDisabled;
                            if (IsDisabled) {
                                document.getElementById("txtParam3").value = "";
                            }
                            break;
                        case "8":
                            document.getElementById("txtParam4").disabled = IsDisabled;
                            if (IsDisabled) {
                                document.getElementById("txtParam4").value = "";
                            }

                            document.getElementById("ddlParameter6").disabled = IsDisabled;
                            if (IsDisabled) {
                                document.getElementById("ddlParameter6").value = "Tidak Ada";
                            }

                            break;
							case "11":
								document.getElementById("txtParam5").disabled = IsDisabled; 
								if(IsDisabled){
									document.getElementById("txtParam5").value = "0";
								}
								break;
							case "13":
								document.getElementById("txtParam6").disabled = IsDisabled; 
								if(IsDisabled){
									document.getElementById("txtParam6").value = "0";
								}
								break;
							case "15":
								document.getElementById("txtParam7").disabled = IsDisabled; 
								if(IsDisabled){
									document.getElementById("txtParam7").value = "0";
								}
								break;
							case "17":
								document.getElementById("txtParam8").disabled = IsDisabled; 
								if(IsDisabled){
									document.getElementById("txtParam8").value = "0";
								}
								break;
							case "19":
								document.getElementById("txtParam9").disabled = IsDisabled; 
								if(IsDisabled){
									document.getElementById("txtParam9").value = "";
								}
								break;
							case "21":
								document.getElementById("txtParam10").disabled = IsDisabled; 
								if(IsDisabled){
									document.getElementById("txtParam10").value = "";
								}
								break;
							case "23":
								document.getElementById("txtParam11").disabled = IsDisabled; 
								if(IsDisabled){
									document.getElementById("txtParam11").value = "";
								}
								break;
							case "25":
								document.getElementById("txtParam12").disabled = IsDisabled; 
								if(IsDisabled){
									document.getElementById("txtParam12").value = "";
								}
								break;
							case "27":
								document.getElementById("txtParam13").disabled = IsDisabled; 
								if(IsDisabled){
									document.getElementById("txtParam13").value = "";
								}
								break;
							case "29":
								document.getElementById("txtParam14").disabled = IsDisabled; 
								if(IsDisabled){
									document.getElementById("txtParam14").value = "";
								}
								break;
							case "31":
								document.getElementById("txtParam21").disabled = IsDisabled; 
								if(IsDisabled){
									document.getElementById("txtParam21").value = "";
								}
								break; 
							case "33":
								document.getElementById("txtParam22").disabled = IsDisabled; 
								if(IsDisabled){
									document.getElementById("txtParam22").value = "";
								}
								document.getElementById("txtParam23").disabled = IsDisabled; 
								if(IsDisabled){
									document.getElementById("txtParam23").value = "";
								}
								break;
							case "41":
								document.getElementById("txtParam28").disabled = IsDisabled; 
								if(IsDisabled){
									document.getElementById("txtParam28").value = "";
								}
								break; 
							case "43":
								document.getElementById("txtParam29").disabled = IsDisabled; 
								if(IsDisabled){
									document.getElementById("txtParam29").value = "";
								}
                            break; 


						}
						
				  
				} 
			}
			//baca setiap cbo ubah value
			var ddlForm = document.getElementsByClassName("ddlForm");
			  for (var i = 0; i < ddlForm.length; i++) {
					var id = ddlForm[i].getAttribute("id");
				  let select = document.getElementById(id);
				 
				  select.addEventListener('change', function() {
					 var value = select.value; 
					 var sortID =select.getAttribute("sortid"); 
					  changeCbo(sortID, value); 
						
				  });
			}
		  
            function getFormDesktopResultValue() {
                var controlValues = "";
                var ddlForm = document.getElementsByClassName("ddlForm");
                for (var i = 0; i < ddlForm.length; i++) {
                    ddlForm[i];
                    var id = ddlForm[i].getAttribute("id");
                    if (controlValues != "") {
                        controlValues += "|";
                    }
                    controlValues += ddlForm[i].getAttribute("sortid") + "^" + ddlForm[i].getAttribute("labelname") + "=" + document.getElementById(id).value;

                }
                var txtForm = document.getElementsByClassName("txtForm");
                for (var i = 0; i < txtForm.length; i++) {
                    var id = txtForm[i].getAttribute("id");
                    if (controlValues != "") {
                        controlValues += "|";
                    }
                    controlValues += txtForm[i].getAttribute("sortid") + "^" + txtForm[i].getAttribute("labelname") + "=" + document.getElementById(id).value;
                    console.log(txtForm[i].getAttribute("sortid") + "^" + txtForm[i].getAttribute("labelname") + "=" + document.getElementById(id).value);
                }
                var chkForm = document.getElementsByClassName("chkForm");
                for (var i = 0; i < chkForm.length; i++) {
                    var id = chkForm[i].getAttribute("id");
                    var confirm = document.getElementById(id); ///document.getElementById(id).value;
                    var value = "0";
                    var tag_value = "";
                    if (confirm != null) {
                        value = confirm.checked;
                        if (value == true) {
                            value = confirm.Value;
                            tag_value = chkForm[i].getAttribute("tagvalue");
                        } else {
                            value = 0;
                            tag_value = "Tidak";
                        }
                    }

                    if (controlValues != "") {
                        controlValues += "|";
                    }
                    controlValues += chkForm[i].getAttribute("sortid") + "^" + chkForm[i].getAttribute("labelname") + "=" + tag_value;

                }
                var optForm = document.getElementsByClassName("optForm");
                for (var i = 0; i < optForm.length; i++) {
                    var id = optForm[i].getAttribute("id");
                    var confirm = document.getElementById(id); ///document.getElementById(id).value;
                    var value = "0";
                    var tag_value = "";
                    if (confirm != null) {
                        value = confirm.checked;
                        if (value == true) {
                            tag_value = optForm[i].getAttribute("tagvalue");
							if (controlValues != "") {
								controlValues += "|";
							}
							controlValues += optForm[i].getAttribute("sortid") + "^" + optForm[i].getAttribute("labelname") + "=" + tag_value;
                        }
                    }


                }

                document.getElementById("hdnFormDesktopResultValueCtl").value = controlValues;

            }
            function getFormDesktopValue() {
                var controlValues = "";
                var ddlForm = document.getElementsByClassName("ddlForm");
                for (var i = 0; i < ddlForm.length; i++) {
                    var id = ddlForm[i].getAttribute("id");
                    if (controlValues != "") {
                        controlValues += ";";
                    }
                    controlValues += ddlForm[i].getAttribute("controlID") + "=" + document.getElementById(id).value;
                }
                var txtForm = document.getElementsByClassName("txtForm");
                for (var i = 0; i < txtForm.length; i++) {
                    var id = txtForm[i].getAttribute("id");
                    if (controlValues != "") {
                        controlValues += ";";
                    }
                    controlValues += txtForm[i].getAttribute("controlID") + "=" + document.getElementById(id).value;
                }
                var chkForm = document.getElementsByClassName("chkForm");
                for (var i = 0; i < chkForm.length; i++) {
                    var id = chkForm[i].getAttribute("id");
                    var confirm = document.getElementById(id); ///document.getElementById(id).value;
                    var value = "0";
                    if (confirm != null) {
                        value = confirm.checked;
                        if (value == true) {
                            value = 1;
                        } else {
                            value = 0;
                        }
                    }

                    if (controlValues != "") {
                        controlValues += ";";
                    }

                    controlValues += chkForm[i].getAttribute("controlID") + "=" + value;
                }

                var optForm = document.getElementsByClassName("optForm");
                for (var i = 0; i < optForm.length; i++) {
                    var id = optForm[i].getAttribute("id");
                    var confirm = document.getElementById(id); ///document.getElementById(id).value;
                    var value = "0";
                    if (confirm != null) {
                        value = confirm.checked;
                        if (value == true) {
                            value = 1;
                        } else {
                            value = 0;
                        }
                    }
                    if (controlValues != "") {
                        controlValues += ";";
                    }
                    controlValues += optForm[i].getAttribute("controlID") + "=" + value;
                }
                console.log(controlValues);
                document.getElementById("hdnFormDesktopValueCtl").value = controlValues;
            }
            function onValidation(a) {

                var x = (a.value || a.options[a.selectedIndex].value);

                var readOnly = true;
                if (x == "Tidak") {
                    readOnly = true;
                } else {
                    if (x == "Abnormal") {
                        readOnly = false;
                    } else if (x == "Ya") {
                        readOnly = false;
                    }
                }
                var controlID = a.getAttribute("controlID");

                if (controlID == "ddlParameter1") {
                    document.getElementById("txtParam1").disabled = readOnly;
                    document.getElementById("txtParam1").value = "";
                } else if (controlID == "ddlParameter2") {
                    document.getElementById("txtParam2").disabled = readOnly;
                    document.getElementById("txtParam2").value = "";
                    document.getElementById("ddlParameter3").disabled = readOnly;
                    document.getElementById("ddlParameter3").value = "-";
                } else if (controlID == "ddlParameter4") {
                    document.getElementById("txtParam3").disabled = readOnly;
                    document.getElementById("txtParam3").value = "";
                } else if (controlID == "ddlParameter5") {
                    document.getElementById("txtParam4").disabled = readOnly;
                    document.getElementById("txtParam4").value = "";
                    document.getElementById("ddlParameter6").disabled = readOnly;
                    document.getElementById("ddlParameter6").value = "Tidak Ada";
                } else if (controlID == "ddlParameter7") {
                    document.getElementById("txtParam5").disabled = readOnly;
                    document.getElementById("txtParam5").value = "0";
                } else if (controlID == "ddlParameter8") {
                    document.getElementById("txtParam6").disabled = readOnly;
                    document.getElementById("txtParam6").value = "0";
                } else if (controlID == "ddlParameter9") {
                    document.getElementById("txtParam7").disabled = readOnly;
                    document.getElementById("txtParam7").value = "0";
                } else if (controlID == "ddlParameter10") {
                    document.getElementById("txtParam8").disabled = readOnly;
                    document.getElementById("txtParam8").value = "0";
                } else if (controlID == "ddlParameter11") {
                    document.getElementById("txtParam9").disabled = readOnly;
                    document.getElementById("txtParam9").value = "";
                } else if (controlID == "ddlParameter12") {
                    document.getElementById("txtParam10").disabled = readOnly;
                    document.getElementById("txtParam10").value = "";
                } else if (controlID == "ddlParameter13") {
                    document.getElementById("txtParam11").disabled = readOnly;
                    document.getElementById("txtParam11").value = "";
                } else if (controlID == "ddlParameter14") {
                    document.getElementById("txtParam12").disabled = readOnly;
                    document.getElementById("txtParam12").value = "";
                } else if (controlID == "ddlParameter15") {
                    document.getElementById("txtParam13").disabled = readOnly;
                    document.getElementById("txtParam13").value = "";
                } else if (controlID == "ddlParameter16") {
                    document.getElementById("txtParam14").disabled = readOnly;
                    document.getElementById("txtParam14").value = "";
                }

                else if (controlID == "ddlParameter21") {
                    document.getElementById("txtParam21").disabled = readOnly;
                    document.getElementById("txtParam21").value = "";
                } else if (controlID == "ddlParameter22") {
                    document.getElementById("txtParam22").disabled = readOnly;
                    document.getElementById("txtParam22").value = "";
                    document.getElementById("txtParam23").disabled = readOnly;
                    document.getElementById("txtParam23").value = "";
                } else if (controlID == "ddlParameter23") {
                    document.getElementById("txtParam23").disabled = readOnly;
                    document.getElementById("txtParam23").value = "";
                } else if (controlID == "ddlParameter23") {
                    document.getElementById("txtParam23").disabled = readOnly;
                    document.getElementById("txtParam23").value = "";
                } else if (controlID == "ddlParameter24") {
                    document.getElementById("txtParam28").disabled = readOnly;
                    document.getElementById("txtParam28").value = "";
                } else if (controlID == "ddlParameter25") {
                    document.getElementById("txtParam29").disabled = readOnly;
                    document.getElementById("txtParam29").value = "";
                } else if (controlID == "ddlParameter26") {
                    document.getElementById("txtParam26").disabled = readOnly;
                    document.getElementById("txtParam26").value = "";
                } else if (controlID == "ddlParameter27") {
                    document.getElementById("txtParam27").disabled = readOnly;
                    document.getElementById("txtParam27").value = "";
                } else if (controlID == "ddlParameter28") {
                    document.getElementById("txtParam21").disabled = readOnly;
                    document.getElementById("txtParam21").value = "";
                } else if (controlID == "ddlParameter29") {
                    document.getElementById("txtParam29").disabled = readOnly;
                    document.getElementById("txtParam29").value = "";
                } else if (controlID == "ddlParameter210") {
                    document.getElementById("txtParam210").disabled = readOnly;
                    document.getElementById("txtParam210").value = "";
                } else if (controlID == "ddlParameter211") {
                    document.getElementById("txtParam211").disabled = readOnly;
                    document.getElementById("txtParam211").value = "";
                } else if (controlID == "ddlParameter212") {
                    document.getElementById("txtParam212").disabled = readOnly;
                    document.getElementById("txtParam212").value = "";
                }

            }
            
           