Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration
Partial Class iPxAdmin_iPxAPVendor
    Inherits System.Web.UI.Page
    Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
    Dim oSQLCmd As SqlCommand
    Dim oSQLReader As SqlDataReader
    Dim sSQL As String
    Dim cIpx As New iPxClass
    Sub showdata_dropdownCustStatus()
        dlQStatus.Items.Insert(0, "")
        dlQStatus.Items.Insert(1, "All Vendor")
        dlQStatus.Items.Insert(2, "Non Vendor")
        dlQStatus.Items.Insert(3, "Vendor")
    End Sub
    Sub ARGroup()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcctAP_Cfg_VendorGrp where businessid = '" & Session("sBusinessID") & "'"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                dlArgroup.DataSource = dt
                dlArgroup.DataTextField = "Description"
                dlArgroup.DataValueField = "apGroup"
                dlArgroup.DataBind()
                dlArgroup.Items.Insert(0, "")
            End Using
        End Using
    End Sub
    Sub country()
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPx_profile_geog_country order by country asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                dlCountry.DataSource = dt
                dlCountry.DataTextField = "country"
                dlCountry.DataValueField = "countryid"
                dlCountry.DataBind()
                dlCountry.Items.Insert(0, "")
            End Using
        End Using
    End Sub
    Sub province()
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPx_profile_geog_province where countryid ='" & dlCountry.SelectedValue & "' order by description asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                dlProvince.DataSource = dt
                dlProvince.DataTextField = "description"
                dlProvince.DataValueField = "provid"
                dlProvince.DataBind()
                dlProvince.Items.Insert(0, "")
            End Using
        End Using
    End Sub
    Sub city()
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPx_profile_geog_city where countryid ='" & dlCountry.SelectedValue & "' and provid = '" & dlProvince.SelectedValue & "' order by city asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                dlCity.DataSource = dt
                dlCity.DataTextField = "city"
                dlCity.DataValueField = "cityid"
                dlCity.DataBind()
                dlCity.Items.Insert(0, "")
            End Using
        End Using
    End Sub
    Sub listCustomerAR()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT iPxAcctAP_Cfg_Vendor.businessid, iPxAcctAP_Cfg_Vendor.VendorID, iPxAcctAP_Cfg_VendorGrp.Description AS APGroup, "
        sSQL += "iPxAcctAP_Cfg_Vendor.CoyName, iPxAcct_Coa.COA as coaLink, "
        sSQL += "iPxAcctAP_Cfg_Vendor.CreditLimit, iPx_profile_geog_city.city, iPxAcctAP_Cfg_Vendor.Phone, "
        sSQL += "iPxAcctAP_Cfg_Vendor.Email FROM iPxAcctAP_Cfg_Vendor "
        sSQL += "INNER JOIN iPxAcctAP_Cfg_VendorGrp ON iPxAcctAP_Cfg_Vendor.businessid = iPxAcctAP_Cfg_VendorGrp.businessid AND iPxAcctAP_Cfg_Vendor.apGroup = iPxAcctAP_Cfg_VendorGrp.apGroup "
        sSQL += "INNER JOIN iPxAcct_Coa ON iPxAcct_Coa.businessid=iPxAcctAP_Cfg_Vendor.businessid AND iPxAcct_Coa.Coa=iPxAcctAP_Cfg_Vendor.CoaLink "
        sSQL += "LEFT JOIN iPxAcctAR_Cfg_FOmapping ON iPxAcctAR_Cfg_FOmapping.businessid=iPxAcctAP_Cfg_Vendor.businessid AND iPxAcctAR_Cfg_FOmapping.CustomerID=iPxAcctAP_Cfg_Vendor.VendorID "
        sSQL += "LEFT JOIN iPxAcct_FOlink ON iPxAcct_FOlink.businessid=iPxAcctAP_Cfg_Vendor.businessid AND iPxAcct_FOlink.FoLink=iPxAcctAR_Cfg_FOmapping.Folink "
        sSQL += "LEFT JOIN iPx_profile_geog_city ON iPxAcctAP_Cfg_Vendor.countryid = iPx_profile_geog_city.countryid AND "
        sSQL += "iPxAcctAP_Cfg_Vendor.provid = iPx_profile_geog_city.provid AND iPxAcctAP_Cfg_Vendor.CityID = iPx_profile_geog_city.cityid "
        sSQL += "where iPxAcctAP_Cfg_Vendor.businessid = '" & Session("sBusinessID") & "' and "
        If Session("sQueryTicket") = "" Then
            Session("sQueryTicket") = Session("sCondition")
            If Session("sQueryTicket") <> "" Or Session("sCondition") <> "" Then
                sSQL = sSQL & Session("sQueryTicket")
                Session("sCondition") = ""
            Else
                sSQL = sSQL & "iPxAcctAP_Cfg_Vendor.IsActive='" & "Y" & "'"
            End If
        Else
            sSQL = sSQL & Session("sQueryTicket")
            Session("sCondition") = ""
        End If
        sSQL += " order by iPxAcctAP_Cfg_Vendor.VendorID asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvCustAR.DataSource = dt
                    gvCustAR.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvCustAR.DataSource = dt
                    gvCustAR.DataBind()
                    gvCustAR.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("sBusinessID") = "" Then
            Response.Redirect("signin.aspx")
        End If
        If Not Me.IsPostBack Then
            If cIpx.getAccessUser(Session("sBusinessID"), Session("sUserCode"), "AR Configuration") <> True Then

                Session("sMessage") = "Sorry, you dont have access in this module |"
                Session("sMemberid") = ""
                Session("sWarningID") = "0"
                Session("sUrlOKONLY") = "home.aspx"
                Session("sUrlYES") = "http://www.thepyxis.net"
                Session("sUrlNO") = "http://www.thepyxis.net"
                Response.Redirect("warningmsg.aspx")
            End If
            Session("sQueryTicket") = ""
            listCustomerAR()
        End If
    End Sub

    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvCustAR.PageIndex = e.NewPageIndex
        Me.listCustomerAR()
    End Sub

    Protected Sub gvCustAR_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvCustAR.PageIndexChanging
        gvCustAR.PageIndex = e.NewPageIndex
        listCustomerAR()
    End Sub
    Protected Sub OnPageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvCustAR.PageIndex = e.NewPageIndex
        Me.listCustomerAR()
    End Sub
    Protected Sub lbAddCust_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAddCust.Click
        Session("sBiEdit") = ""
        Response.Redirect("iPxAPInputVendor.aspx")
    End Sub

    Protected Sub gvCustAR_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvCustAR.RowCommand
        If e.CommandName = "getEdit" Then
            Session("sBiEdit") = e.CommandArgument.ToString
            If oCnct.State = ConnectionState.Closed Then
                oCnct.Open()
            End If
            oSQLCmd = New SqlCommand(sSQL, oCnct)
            sSQL = "SELECT apGroup FROM iPxAcctAp_Cfg_Vendor WHERE VendorID = '" & Session("sBiEdit") & "'"
            oSQLCmd.CommandText = sSQL
            oSQLReader = oSQLCmd.ExecuteReader

            oSQLReader.Read()
            If oSQLReader.HasRows Then
                Dim AR As String = oSQLReader.Item("APGroup").ToString
                oCnct.Close()
                Response.Redirect("iPxAPInputVendor.aspx")
            Else
                oCnct.Close()
            End If
        End If
    End Sub

    Protected Sub lbQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbQuery.Click
        ARGroup()
        country()
        city()
        province()
        tbCoyName.Text = ""
        tbCustID.Text = ""
        Session("sQueryTicket") = ""
        showdata_dropdownCustStatus()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalQuery", "showModalQuery()", True)
    End Sub

    Protected Sub dlCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dlCountry.SelectedIndexChanged
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQuery", "hideModalQuery()", True)
        city()
        province()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalQuery", "showModalQuery()", True)
    End Sub

    Protected Sub dlProvince_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dlProvince.SelectedIndexChanged
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQuery", "hideModalQuery()", True)
        city()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalQuery", "showModalQuery()", True)
    End Sub

    Protected Sub lblQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblQuery.Click
        If dlQStatus.SelectedIndex = 0 Then
            Session("sCondition") = Session("sCondition") & " iPxAcctAP_Cfg_Vendor.IsActive='Y' "
        ElseIf dlQStatus.SelectedIndex = 1 Then
            Session("sCondition") = Session("sCondition") & " iPxAcctAP_Cfg_Vendor.IsActive = 'Y' or IsActive = 'N' "
        ElseIf dlQStatus.SelectedIndex = 2 Then
            Session("sCondition") = Session("sCondition") & " iPxAcctAP_Cfg_Vendor.IsActive = 'N'"
        ElseIf dlQStatus.SelectedIndex = 3 Then
            Session("sCondition") = Session("sCondition") & " iPxAcctAP_Cfg_Vendor.IsActive = 'Y'"
        End If
        If dlArgroup.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " and (iPxAcctAP_Cfg_Vendor.apGroup = '" & dlArgroup.SelectedValue & "') "
        End If
        If tbCustID.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " and (iPxAcctAP_Cfg_Vendor.VendorID like '%" & Replace(tbCustID.Text.Trim, "'", "''") & "%') "
        End If
        If tbCoyName.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " and (iPxAcctAP_Cfg_Vendor.CoyName like '%" & Replace(tbCoyName.Text.Trim, "'", "''") & "%') "
        End If
        If dlCountry.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " and (iPxAcctAP_Cfg_Vendor.CountryId = '" & dlCountry.SelectedValue & "') "
        End If
        If dlProvince.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " and (iPxAcctAP_Cfg_Vendor.provid = '" & dlProvince.SelectedValue & "') "
        End If
        If dlCity.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " and (iPxAcctAP_Cfg_Vendor.CityID = '" & dlCity.SelectedValue & "') "
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQuery", "hideModalQuery()", True)
        listCustomerAR()
    End Sub

    Protected Sub lbAbortQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortQuery.Click
        Session("sCondition") = ""
        Session("sQueryTicket") = ""
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQuery", "hideModalQuery()", True)
        listCustomerAR()
    End Sub
End Class
