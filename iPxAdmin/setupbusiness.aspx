<%@ Page Language="VB" AutoEventWireup="false"  MasterPageFile="~/iPxAdmin/MasterPage.master" CodeFile="setupbusiness.aspx.vb" Inherits="iPxAdmin_setupbusiness" title="Alcor Accounting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- Site Setting Panel -->

<!-- Site Setting Panel End -->

<!-- Page Header Start -->


  <div class="thumbnail" style="overflow:auto;">
   <div class="row"> 
         <br />
      
      <h4> <strong>
      <asp:Label ID="Label1" class="nm pad-30" runat="server" 
              Text="Detail Business Profile" meta:resourcekey="Label1Resource1"></asp:Label> </strong>
      </h4> 

 
      <!--    
      <h4 class="nm pad-15"> Detail <strong> Business Profile </strong> </h4>
      -->
      <div >
          <div class="form-group">
               <div class="col-sm-12">
    
            <asp:Label ID="lblBusinessType" for="" class="col-sm-2" runat="server" 
                  Text="Business Type" meta:resourcekey="lblBusinessTypeResource1"></asp:Label>
            <div class="col-sm-10">
               <asp:DropDownList ID="lstBussType" class="form-control" 
                  runat="server" AutoPostBack="True" DataSourceID="iPxCNCT0" 
                    DataTextField="description" DataValueField="id" required="required"
                    meta:resourcekey="lstBussTypeResource1">
                     
              </asp:DropDownList>
              
                <asp:SqlDataSource ID="iPxCNCT0" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:iPxCNCT %>" 
                    SelectCommand="SELECT [description], [id] FROM [iPx_general_businesstype] WHERE id in ('26') ">
                </asp:SqlDataSource>
            </div>
                   </div>
              </div>

          <div class="divider"></div>
          
        <div class="form-group">
               <div class="col-sm-12">
            <asp:Label ID="lblBusinessGroup" for="" class="col-sm-2" runat="server" 
                  Text="Group/Under Management Of" 
                  meta:resourcekey="lblBusinessGroupResource1"></asp:Label>            
                  
            <div class="col-sm-10">
               <asp:DropDownList ID="lstBussGroup" class="form-control"
                  runat="server" AutoPostBack="True" DataSourceID="iPxCNCT1" 
                    DataTextField="description" DataValueField="id" required="required"
                    meta:resourcekey="lstInvGroupResource1">
              </asp:DropDownList>
                <asp:SqlDataSource ID="iPxCNCT1" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:iPxCNCT %>" 
                    
                    
                    SelectCommand="SELECT [description], [id] FROM [iPx_general_businessgroup] WHERE (([userid] = @userid) OR ([id] = @id))">
                    <SelectParameters>
                        <asp:SessionParameter Name="userid" SessionField="iUserID" Type="Int64" />
                        <asp:Parameter DefaultValue="1" Name="id" Type="Int64" />
                    </SelectParameters>
                </asp:SqlDataSource>
              <div class="row">
                <div class="col-lg-6">
                  <div class="input-group">
                    <asp:TextBox ID="txtBusinessGroup" placeholder="Enter a business group or Management here" 
                          class="form-control padding-horizontal-15"  runat="server" 
                          meta:resourcekey="txtInvGroupResource1" MaxLength="50"></asp:TextBox>
                    <span class="input-group-btn">
                    <asp:Button ID="btnAddBussGroup" class="btn btn-default" runat="server" Text="Add" OnClientClick="beep()" meta:resourcekey="btnAddInvGroupResource1" />
                    <asp:Button ID="btnDelBussGroup" class="btn btn-default" runat="server" Text="Del" meta:resourcekey="btnDelInvGroupResource1" OnClientClick="beep()"/>
                    </span> 
                  </div>
                </div>
              </div>
            </div>
          </div>
         </div>
          
          <div class="divider"></div>

          <div class="form-group">
               <div class="col-sm-12">
            <asp:Label ID="Label2" for="" class="col-sm-2" runat="server" 
                  Text="Business Name" meta:resourcekey="Label2Resource1"></asp:Label>            

            <div class="col-sm-10">
               <asp:TextBox ID="txtBusinessName" 
                    placeholder="Enter your business name here" required="required" 
                    class="form-control padding-horizontal-15"  runat="server" 
                    meta:resourcekey="txtInvDescriptionResource1" MaxLength="60"></asp:TextBox>
            </div>
          </div>   
          <div class="divider"></div>
            <div class="form-group">
               <div class="col-sm-12">
                <asp:Label ID="Label3" for="" class="col-sm-2" runat="server" 
                  Text="Business / Tax No" meta:resourcekey="Label3Resource1"></asp:Label>            

                   <div class="col-sm-10">
                      <asp:TextBox ID="txtBusinessNo" placeholder="Enter your business tax no here" 
                           class="form-control padding-horizontal-15"  runat="server" 
                           meta:resourcekey="TextBox6Resource1" MaxLength="25"></asp:TextBox>
                   </div>
                </div>   
                </div>
          <div class="divider"></div>
          <div class="form-group">
               <div class="col-sm-12">
            <asp:Label ID="Label4" for="" class="col-sm-2" runat="server" 
                  Text="Address" meta:resourcekey="Label4Resource1"></asp:Label>            
            
            <div class="col-sm-10">
              <asp:TextBox ID="txtBusinessAddress" rows="4" class="form-control" required="required"
                  runat="server" TextMode="MultiLine" meta:resourcekey="TextBox1Resource1"></asp:TextBox>
            </div>
          </div>
              </div>
              </div>

          <div class="divider"></div>

          <div class="form-group">
               <div class="col-sm-12">
            <asp:Label ID="Label5" for="" class="col-sm-2" runat="server" Text="Phone" meta:resourcekey="Label5Resource1"></asp:Label>            
            <div class="col-sm-4">
               <asp:TextBox ID="txtBusinessPhone" placeholder="Enter your business phone no here" class="form-control padding-horizontal-15"  runat="server" meta:resourcekey="TextBox2Resource1" MaxLength="25"></asp:TextBox>
            </div>
            
            <asp:Label ID="Label6" for="" class="col-sm-2" runat="server" Text="Fax" meta:resourcekey="Label6Resource1"></asp:Label>            
                   <div class="col-sm-4">
                      <asp:TextBox ID="txtBusinessFax" placeholder="Enter your business fax no here" 
                           class="form-control padding-horizontal-15"  runat="server" 
                           meta:resourcekey="TextBox6Resource1" MaxLength="25"></asp:TextBox>
                   </div>
            
            
          </div>   
 </div>
          <div class="divider"></div>

            <div class="form-group">
               <div class="col-sm-12">
                            <asp:Label ID="Label7" for="" class="col-sm-2" runat="server" 
                  Text="Contact Person" meta:resourcekey="Label7Resource1"></asp:Label>            

                   <div class="col-sm-4">
                      <asp:TextBox ID="txtBusinessContact" 
                           placeholder="Enter your contact person here" required="required"
                           class="form-control padding-horizontal-15"  runat="server" 
                           meta:resourcekey="TextBox6Resource1" MaxLength="50"></asp:TextBox>
                   </div>
            <asp:Label ID="Label8" for="" class="col-sm-2" runat="server" 
                  Text="Mobile No" meta:resourcekey="Label8Resource1"></asp:Label>            
                   <div class="col-sm-4">
                      <asp:TextBox ID="txtBusinessMobileNo" placeholder="Enter your mobile no here" 
                           class="form-control padding-horizontal-15"  runat="server" 
                           meta:resourcekey="TextBox6Resource1" MaxLength="25"></asp:TextBox>
                   </div>
                   
                   
                </div>   
                </div>

          <div class="divider"></div>

            <div class="form-group">
               <div class="col-sm-12">
                            <asp:Label ID="Label9" for="" class="col-sm-2" runat="server" 
                  Text="Email" meta:resourcekey="Label9Resource1"></asp:Label>            

                   <div class="col-sm-10">
                      <asp:TextBox ID="txtBusinessEmail" placeholder="Enter your business email here" required="required"
                           class="form-control padding-horizontal-15"  runat="server" 
                           meta:resourcekey="TextBox6Resource1" MaxLength="60"></asp:TextBox>
                   </div>
                </div>     
                                              
          <div class="divider"></div>

            <div class="form-group">
               <div class="col-sm-12">
            <asp:Label ID="Label10" for="" class="col-sm-2" runat="server" 
                  Text="WEB Address" meta:resourcekey="Label10Resource1"></asp:Label>            
                   <div class="col-sm-10">
                      <asp:TextBox ID="txtBusinessWEB" placeholder="Enter your business WEB address here" 
                           class="form-control padding-horizontal-15"  runat="server" 
                           meta:resourcekey="TextBox6Resource1"></asp:TextBox>
                   </div>
                </div>   
                </div>
                </div>
          <div class="divider"></div>

            <div class="form-group">
               <div class="col-sm-12">
            <asp:Label ID="Label11" for="" class="col-sm-2" runat="server" 
                  Text="Country" meta:resourcekey="Label11Resource1"></asp:Label>            
                   <div class="col-sm-10">
               <asp:DropDownList ID="lstCountry" class="form-control" 
                  runat="server" AutoPostBack="True" DataSourceID="iPxCNCT2" 
                    DataTextField="country" DataValueField="countryid" required="required"
                    meta:resourcekey="lstInvTypeResource1">
                    
              </asp:DropDownList>
              
                       <asp:SqlDataSource ID="iPxCNCT2" runat="server" 
                           ConnectionString="<%$ ConnectionStrings:iPxCNCT %>" 
                           SelectCommand="SELECT [country], [countryid] FROM [iPx_profile_geog_country]">
                       </asp:SqlDataSource>
              
                   </div>
                </div>   
 </div>
 <div class="form-group ">
               <div class="col-sm-12">
            <asp:Label ID="Label18" for="" class="col-sm-2" runat="server" 
                  Text="City" meta:resourcekey="Label11Resource1"></asp:Label>            
                   <div class="col-sm-10">
               <asp:DropDownList ID="ddCity" class="form-control" AutoPostBack="true"
                  runat="server"  DataSourceID="iPXCNCTCTY" 
                    DataTextField="city" DataValueField="cityid" 
                    meta:resourcekey="lstInvTypeResource1">
                    
              </asp:DropDownList>
              
                       <asp:SqlDataSource ID="iPXCNCTCTY" runat="server" 
                           ConnectionString="<%$ ConnectionStrings:iPxCNCT %>" 
                           SelectCommand="SELECT rtrim([city]) as city, rtrim([cityid]) as cityid FROM [iPx_profile_geog_city]">
                       </asp:SqlDataSource>
              
                   </div>
                </div>   
 </div>
         <div >
         
          <h4> <strong>
             <asp:Label ID="Label21" class="nm pad-30" runat="server" 
              Text="Images" meta:resourcekey="Label21Resource1"></asp:Label> </strong>
          </h4> 
          
        
               <div class="form-group">
               <div class="col-sm-12">
            <asp:Label ID="Label12" for="" class="col-sm-2" runat="server" 
                  Text="Business Logo" meta:resourcekey="Label12Resource1"></asp:Label>            
                   <div class="col-sm-10">
                   <asp:Image ID="Image3" class="form-control" runat="server" Width="155px" ImageUrl="~/images/icon/profile.png"
                           Height="155px" meta:resourcekey="Image3Resource1" />

                    <div class="col-sm-10">       
                   
                      <asp:FileUpload
                   ID="FileUpload4" class=" padding-horizontal-15" runat="server" 
                           meta:resourcekey="FileUpload4Resource1" />
                          
                   </div> 
                       <br />
                    <asp:Button ID="btnRefresh" class="btn btn-default" runat="server" Text="Update Logo"   meta:resourcekey="btnRefreshResource1" OnClientClick="beep()"/>
                   
                   </div> 
                </div>    
                   </div>                 
             </div>
      
                    
          
    
             <h4> <strong>
             <asp:Label ID="Label19" class="nm pad-30" runat="server" 
              Text="Business Tax" meta:resourcekey="Label19Resource1"></asp:Label> </strong>
          </h4> 
            <div class="form-group">
                     <div class="col-sm-12">
            <asp:Label ID="Label13" for="" class="col-sm-3" runat="server" 
                  Text="Tax Description" meta:resourcekey="Label13Resource1"></asp:Label>            
                   <div class="col-sm-3">
                      <asp:TextBox ID="txtTaxDescription" 
                           class="form-control padding-horizontal-15"  runat="server"
                      meta:resourcekey="TextBox6Resource1" MaxLength="25"></asp:TextBox>
                   </div> 
            <asp:Label ID="Label14" for="" class="col-sm-3" runat="server" 
                  Text="Tax %" meta:resourcekey="Label14Resource1"></asp:Label>            
                   <div class="col-sm-3">
                      <asp:TextBox ID="txtTaxPCT" 
                           class="form-control padding-horizontal-15"  runat="server" style="text-align: right"
                      meta:resourcekey="TextBox6Resource1"></asp:TextBox>
                   </div> 
                         </div>
                </div>   
                <div class="form-group">
                     <div class="col-sm-12">
            <asp:Label ID="Label15" for="" class="col-sm-3" runat="server" 
                  Text="Service Charge Description" meta:resourcekey="Label15Resource1"></asp:Label>            
                   <div class="col-sm-3">
                      <asp:TextBox ID="txtSchgDescription" 
                           class="form-control padding-horizontal-15"  
                           runat="server"                       meta:resourcekey="TextBox6Resource1" 
                           MaxLength="25"></asp:TextBox>
                   </div>
            <asp:Label ID="Label16" for="" class="col-sm-3" runat="server" 
                  Text="Service Charge %" meta:resourcekey="Label16Resource1"></asp:Label>            
                   
                   <div class="col-sm-3">
                      <asp:TextBox ID="txtServiceChargePCT" 
                           class="form-control padding-horizontal-15"  runat="server" style="text-align: right"
                      meta:resourcekey="TextBox6Resource1"></asp:TextBox>
                   </div>
                         </div>
                </div>   
                
                <div class="form-group">
                     <div class="col-sm-12">
            <asp:Label ID="Label17" for="" class="col-sm-3" runat="server" 
                  Text="Tax Formula" meta:resourcekey="Label17Resource1"></asp:Label>            
                   <div class="col-sm-3">
                      <asp:DropDownList ID="lstTaxFormula" class="form-control" required="required"
                      runat="server" meta:resourcekey="DropDownList3Resource1" 
                           DataSourceID="iPxCNCT3" DataTextField="description" DataValueField="id"> </asp:DropDownList>
                       <asp:SqlDataSource ID="iPxCNCT3" runat="server" 
                           ConnectionString="<%$ ConnectionStrings:iPxCNCT %>" 
                           SelectCommand="SELECT [description], [id] FROM [iPx_profile_taxformula]">
                       </asp:SqlDataSource>
                  </div>
                    <br />
                    <br />
                </div>                                        
  
          <div class="form-group">
      
             <h4> <strong>
             <asp:Label ID="Label26" class="nm pad-30" runat="server" Visible="false" 
              Text="Generic ID" meta:resourcekey="Label19Resource1"></asp:Label> </strong>
          </h4> 
          <br />
              <div class="form-group">
                     <div class="col-sm-12">
           <asp:Label ID="Label27" for="" class="col-sm-3" runat="server"  Visible="false"
                  Text="Generic Logon ID" meta:resourcekey="Label14Resource1"></asp:Label>   
          <div class="col-sm-3">
              <asp:TextBox class="form-control padding-horizontal-15" ID="txtGenericID"  Visible="false"
                  runat="server"  ></asp:TextBox>
          </div>
                    <asp:Label ID="Label28" for="" class="col-sm-3" runat="server"  Visible="false"
                  Text="Password" meta:resourcekey="Label14Resource1"></asp:Label>   
          <div class="col-sm-3">
          
              <asp:TextBox class="form-control padding-horizontal-15" ID="txtGenericPassword" Visible="false" runat="server" MaxLength="25" TextMode="Password"></asp:TextBox>
          </div> 
                </div>
          </div> 
                </div>

     
      </div>

    <!-- end Input --> 
    
 

      </div>
 
    
    </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder3" Runat="Server">

<asp:Panel ID="footer"  >
<div >
<h5></h5>
</div>

    <div class="form-group">  
    <label for="" class="col-sm-2"></label>
              <asp:LinkButton width=170px CssClass="btn btn-default dropdown-toggle" data- ID="btnSave" runat="server"><span class="fa fa-save "></span> Save</asp:LinkButton>    
      
      
         <asp:LinkButton width=170px CssClass="btn btn-default dropdown-toggle" data- ID="btnBack" runat="server"><span class="fa fa-close"></span> Abort</asp:LinkButton> 
    </div>   
       
 
 
  

 </asp:Panel>
 
 </asp:Content>