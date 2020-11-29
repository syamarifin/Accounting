<%@ Page Language="VB" MasterPageFile="~/iPxDashboard/iPxDashboard.master" CodeFile="iPxAccesUserAdd.aspx.vb" Inherits="iPxDashboard_iPxAccesUserAdd" title="Alcor Accounting"%>
<%---------------------------------------------------------------------------------------------------------------------%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
        <script type = "text/javascript">
            //<!--
            function Check_Click(objRef)
            {
                //Get the Row based on checkbox
                var row = objRef.parentNode.parentNode;
                
                //Get the reference of GridView
                var GridView = row.parentNode;
                
                //Get all input elements in Gridview
                var inputList = GridView.getElementsByTagName("input");
                var en=0;
                for (var i=0;i<inputList.length;i++)
                {
                    //The First element is the Header Checkbox
                    var headerCheckBox = inputList[0];
                    
                    //Based on all or none checkboxes
                    //are checked check/uncheck Header Checkbox
                    var checked = true;
                    if(inputList[i].type == "checkbox" && inputList[i] != headerCheckBox)
                    {
                        if(inputList[i].disabled!=false){
                        }else{
                            if(!inputList[i].checked)
                            {
                                checked = false;
                                break;
                            }
                        }
                    }
                }
                headerCheckBox.checked = checked;
            }
            function checkAll(objRef)
            {
                var GridView = objRef.parentNode.parentNode.parentNode;
                var inputList = GridView.getElementsByTagName("input");
                for (var i=0;i<inputList.length;i++)
                {
                    var row = inputList[i].parentNode.parentNode;
                    if(inputList[i].type == "checkbox"  && objRef != inputList[i])
                    {
                        if (objRef.checked)
                        {
                            if(inputList[i].disabled!=false){}
                            else{
                                inputList[i].checked=true;
                                }
                        }
                        else
                        {
                            inputList[i].checked=false;
                        }
                    }
                }
            }
            //-->
        </script>
        <script type = "text/javascript">
            function ConfirmModule()
            {
                var count = document.getElementById("<%=hfCount.ClientID %>").value;
                var gv = document.getElementById("<%=gvModuleAcc.ClientID%>");
                var chk = gv.getElementsByTagName("input");
                for(var i=0;i<chk.length;i++)
                {
                    if(chk[i].checked && chk[i].id.indexOf("chkAll") == -1)
                    {
                        count++;
                    }
                }
                if(count == 0)
                {
                    alert("No records to user module acces.");
                    return false;
                }
                else
                {
                    
                }
            }

            function ConfirmSave()
            {
                var count = document.getElementById("<%=hfCount.ClientID %>").value;
                var gv = document.getElementById("<%=gvModuleAcc.ClientID%>");
                var chk = gv.getElementsByTagName("input");
                // gridFunction
                var countF = document.getElementById("<%=hfCountFunct.ClientID %>").value;
                var gvF = document.getElementById("<%=gvFunctAcc.ClientID%>");
                var chkF = gvF.getElementsByTagName("input");
                for(var i=0;i<chk.length;i++)
                {
                    if(chk[i].checked && chk[i].id.indexOf("chkAll") == -1)
                    {
                        count++;
                    }
                }

                // cek select grid function
                for(var i=0;i<chkF.length;i++)
                {
                    if(chkF[i].checked && chkF[i].id.indexOf("chkAll") == -1)
                    {
                        countF++;
                    }
                }
                if(count == 0)
                {
                    alert("No records to user acces.");
                    return false;
                }
                else
                {
                       if(countF == 0)
                        {
                            return confirm("Do you want to acces view only?.");
                        }
                        else
                        {
                            
                        }
                }
            }
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div class="form-group">

    <label for="" class="col-sm-2 text-right">User Name <label style="color:red">*</label></label>
	<div class="col-sm-4">
		<asp:textbox class="form-control padding-horizontal-15" id="tbusercode" MaxLength="50" AutoPostBack="true" runat="server"></asp:textbox>
               
		<br/>
	</div>
	
	
	<label for="" class="col-sm-2 text-right">Group</label>
	<div class="col-sm-4">
        <asp:DropDownList ID="dlGroup" class="form-control padding-horizontal-15" runat="server">
        </asp:DropDownList>
		<br/>
	</div>
	
 	<label for="" class="col-sm-2 text-right">Full Name</label>
	<div class="col-sm-10">
		<asp:textbox class="form-control padding-horizontal-15" id="tbfullname" MaxLength="15" runat="server"></asp:textbox>
		<br/>
	</div>
    
	
	
	
	  <%--panel password--%> 
	
    <asp:Panel ID="PnlPassword" runat="server">
        	<label id="lblpassword" for="" runat="server" class="col-sm-2 text-right">Password <label style="color:red">*</label></label>
	<div class="col-sm-4">
		<asp:textbox class="form-control padding-horizontal-15" id="tbpass" MaxLength="25" runat="server" TextMode="Password"></asp:textbox>
        <br/>
	</div>

        	
    <label id="lblconfpass" for="" runat="server" class="col-sm-2 text-right">Confirm Password</label>
	<div class="col-sm-4">
		<asp:textbox class="form-control padding-horizontal-15" id="tbconf_pass" MaxLength="25" runat="server" TextMode="Password"></asp:textbox>
      
        <asp:CompareValidator ID="CompareValidator1" runat="server" 
            ErrorMessage="Passwords do not match!"
            ControlToValidate="tbconf_pass" ControlToCompare="tbpass"
            Operator="Equal" Type="String"></asp:CompareValidator>
         
        <br />  
	</div>
    </asp:Panel>

<%--<label for="" class="col-sm-2 text-right">Locked</label>
	<div class="col-sm-4">
        <asp:CheckBox ID="chkStatus"  runat="server" Visible="false"/>
		<br/>
	</div>--%>
	
	  	 <asp:linkbutton class="col-sm-6 text-right" data- id="btnchange" Visible="false" runat="server">Reset Password</asp:linkbutton>
	

	<div class="col-sm-12 left">
	<br />
	<h4>For Access</h4>
	<hr />
	</div>
    
    <div class="col-sm-4">
        <asp:HiddenField ID="hfCount" runat="server" Value = "0" />
        <label for="" class="text-left">Module Access</label>
        <asp:GridView EmptyDataText="No records has been added." ID="gvModuleAcc" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None">
            <Columns>
                <asp:TemplateField ItemStyle-Width="10px">
                    <HeaderTemplate>
                        <asp:CheckBox ID="chkAll" runat="server" onclick="checkAll(this);" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chk" runat="server" Checked='<%# if(Trim(Eval("access").toString())= "", "false","true") %>' onclick="Check_Click(this)" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="id" ItemStyle-CssClass="hidden" HeaderText="ID" HeaderStyle-CssClass="hidden" />
                <asp:BoundField DataField="description" HeaderText="Module" />
            </Columns>
        </asp:GridView>
    </div>
    <div class="col-sm-8">
        <asp:HiddenField ID="hfCountFunct" runat="server" Value = "0" />
        <label for="" class="text-left">Function Acces</label>
        <asp:GridView EmptyDataText="No records has been added." ID="gvFunctAcc" runat="server" AutoGenerateColumns="false" CssClass="table" HeaderStyle-BackColor="#0a818e" HeaderStyle-ForeColor="White" HeaderStyle-Font-Bold="true" Font-Size="Smaller" GridLines="None">
            <Columns>
                <asp:TemplateField ItemStyle-Width="10px">
                    <HeaderTemplate>
                        <asp:CheckBox ID="chkAll" runat="server" onclick="checkAll(this);" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chk" runat="server" Checked='<%# if(Trim(Eval("access").toString())= "", "false","true") %>' onclick="Check_Click(this)" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="id" ItemStyle-CssClass="hidden" HeaderText="ID" HeaderStyle-CssClass="hidden" />
                <asp:BoundField DataField="description" HeaderText="Function" />
            </Columns>
        </asp:GridView>
    </div>
</div>

  
    
<br />
<br />

        
</asp:Content>
<asp:content id="Content3" contentplaceholderid="ContentPlaceHolder2" runat="Server">
    <div id="footer" class="container" >
        <asp:linkbutton width="150px" cssclass="btn btn-default " data- id="lbFunction" runat="server" Enabled="true" OnClientClick = "return ConfirmModule();"><span class="glyphicon glyphicon-list-alt"></span> Function</asp:linkbutton>
        <asp:linkbutton width="150px" cssclass="btn btn-default " data- id="btnSave" runat="server" Enabled="true" OnClientClick = "return ConfirmSave();"><span class="fa fa-save"></span> Save</asp:linkbutton>
        <asp:LinkButton Width="210" CssClass ="btn btn-default" data- id="btnview" runat="server" Visible="false"><span class ="glyphicon glyphicon-list-alt"></span> View User Access</asp:LinkButton>
        <asp:linkbutton width="150px" cssclass="btn btn-default " data- id="btnCxld" runat="server"><span class="fa fa-close"></span> Abort</asp:linkbutton>
    </div>
</asp:content>

