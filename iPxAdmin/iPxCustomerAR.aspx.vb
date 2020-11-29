Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration
Partial Class iPxAdmin_iPxCustomerAR
    Inherits System.Web.UI.Page
    Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
    Dim oSQLCmd As SqlCommand
    Dim oSQLReader As SqlDataReader
    Dim sSQL As String
    Dim cIpx As New iPxClass
    Sub showdata_dropdownCustStatus()
        dlQStatus.Items.Insert(0, "")
        dlQStatus.Items.Insert(1, "All Customer")
        dlQStatus.Items.Insert(2, "Non Customer")
        dlQStatus.Items.Insert(3, "Customer")
    End Sub
    Sub ARGroup()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcctAR_Cfg_CustomerGrp where businessid = '" & Session("sBusinessID") & "'"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                dlArgroup.DataSource = dt
                dlArgroup.DataTextField = "Description"
                dlArgroup.DataValueField = "arGroup"
                dlArgroup.DataBind()
                dlArgroup.Items.Insert(0, "")
            End Using
        End Using
    End Sub
    Sub CoyGroup()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcctAR_Cfg_CoyGroup where businessid = '" & Session("sBusinessID") & "'"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                dlCoyGroup.DataSource = dt
                dlCoyGroup.DataTextField = "Description"
                dlCoyGroup.DataValueField = "CoyGroup"
                dlCoyGroup.DataBind()
                dlCoyGroup.Items.Insert(0, "")
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
        sSQL = "SELECT iPxAcctAR_Cfg_Customer.businessid, iPxAcctAR_Cfg_Customer.CustomerID, iPxAcctAR_Cfg_CustomerGrp.Description AS ARGroup, iPxAcctAR_Cfg_CoyGroup.Description AS CoyGroup, "
        sSQL += "iPxAcctAR_Cfg_Customer.CoyName, iPxAcct_Coa.COA as coaLink, iPxAcct_FOlink.Description as FOLink, "
        sSQL += "iPxAcctAR_Cfg_Customer.CreditLimit, iPx_profile_geog_city.city, iPxAcctAR_Cfg_Customer.Phone, "
        sSQL += "iPxAcctAR_Cfg_Customer.Email FROM iPxAcctAR_Cfg_Customer "
        sSQL += "INNER JOIN iPxAcctAR_Cfg_CoyGroup ON iPxAcctAR_Cfg_Customer.businessid = iPxAcctAR_Cfg_CoyGroup.businessid AND iPxAcctAR_Cfg_Customer.CoyGroup = iPxAcctAR_Cfg_CoyGroup.CoyGroup "
        sSQL += "INNER JOIN iPxAcctAR_Cfg_CustomerGrp ON iPxAcctAR_Cfg_Customer.businessid = iPxAcctAR_Cfg_CustomerGrp.businessid AND iPxAcctAR_Cfg_Customer.arGroup = iPxAcctAR_Cfg_CustomerGrp.arGroup "
        sSQL += "INNER JOIN iPxAcct_Coa ON iPxAcct_Coa.businessid=iPxAcctAR_Cfg_Customer.businessid AND iPxAcct_Coa.Coa=iPxAcctAR_Cfg_Customer.CoaLink "
        sSQL += "INNER JOIN iPxAcctAR_Cfg_FOmapping ON iPxAcctAR_Cfg_FOmapping.businessid=iPxAcctAR_Cfg_Customer.businessid AND iPxAcctAR_Cfg_FOmapping.CustomerID=iPxAcctAR_Cfg_Customer.CustomerID "
        sSQL += "INNER JOIN iPxAcct_FOlink ON iPxAcct_FOlink.businessid=iPxAcctAR_Cfg_Customer.businessid AND iPxAcct_FOlink.FoLink=iPxAcctAR_Cfg_FOmapping.Folink "
        sSQL += "LEFT JOIN iPx_profile_geog_city ON iPxAcctAR_Cfg_Customer.countryid = iPx_profile_geog_city.countryid AND "
        sSQL += "iPxAcctAR_Cfg_Customer.provid = iPx_profile_geog_city.provid AND iPxAcctAR_Cfg_Customer.CityID = iPx_profile_geog_city.cityid "
        sSQL += "where iPxAcctAR_Cfg_Customer.businessid = '" & Session("sBusinessID") & "' and  iPxAcctAR_Cfg_Customer.arGroup<>'CC' and "
        If Session("sQueryTicket") = "" Then
            Session("sQueryTicket") = Session("sCondition")
            If Session("sQueryTicket") <> "" Or Session("sCondition") <> "" Then
                sSQL = sSQL & Session("sQueryTicket")
                Session("sCondition") = ""
            Else
                sSQL = sSQL & "iPxAcctAR_Cfg_Customer.IsActive='" & "Y" & "'"
            End If
        Else
            sSQL = sSQL & Session("sQueryTicket")
            Session("sCondition") = ""
        End If
        sSQL += " order by iPxAcctAR_Cfg_Customer.CustomerID asc"
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

    Sub listCustomerCC()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT iPxAcctAR_Cfg_Customer.businessid, iPxAcctAR_Cfg_Customer.CustomerID, iPxAcctAR_Cfg_CustomerGrp.Description AS ARGroup, iPxAcctAR_Cfg_CoyGroup.Description AS CoyGroup, "
        sSQL += "iPxAcctAR_Cfg_Customer.CoyName, iPx_profile_cardtype.Description as CardType, iPxAcct_Coa.COA as COALink, "
        sSQL += "CoaComisi.description as CoaComision, iPxAcctAR_Commission.CommissionPct, iPxAcctAR_Cfg_Customer.Phone, "
        sSQL += "iPxAcctAR_Cfg_Customer.Email FROM iPxAcctAR_Cfg_Customer "
        sSQL += "INNER JOIN iPxAcctAR_Cfg_CoyGroup ON iPxAcctAR_Cfg_Customer.businessid = iPxAcctAR_Cfg_CoyGroup.businessid AND iPxAcctAR_Cfg_Customer.CoyGroup = iPxAcctAR_Cfg_CoyGroup.CoyGroup "
        sSQL += "INNER JOIN iPxAcctAR_Cfg_CustomerGrp ON iPxAcctAR_Cfg_Customer.businessid = iPxAcctAR_Cfg_CustomerGrp.businessid AND iPxAcctAR_Cfg_Customer.arGroup = iPxAcctAR_Cfg_CustomerGrp.arGroup "
        sSQL += "INNER JOIN iPxAcctAR_Cfg_FOmapping ON iPxAcctAR_Cfg_FOmapping.businessid=iPxAcctAR_Cfg_Customer.businessid AND "
        sSQL += "iPxAcctAR_Cfg_FOmapping.CustomerID = iPxAcctAR_Cfg_Customer.CustomerID "
        sSQL += "INNER JOIN iPx_profile_cardtype ON iPx_profile_cardtype.cardtype=iPxAcctAR_Cfg_FOmapping.profilecode "
        sSQL += "LEFT JOIN iPxAcct_Coa ON iPxAcct_Coa.businessid=iPxAcctAR_Cfg_Customer.businessid AND iPxAcct_Coa.Coa=iPxAcctAR_Cfg_Customer.CoaLink "
        sSQL += "LEFT JOIN iPxAcctAR_Commission ON iPxAcctAR_Commission.businessid=iPxAcctAR_Cfg_Customer.businessid AND iPxAcctAR_Commission.CustomerID=iPxAcctAR_Cfg_Customer.CustomerID "
        sSQL += "LEFT JOIN iPxAcct_Coa AS CoaComisi ON CoaComisi.businessid=iPxAcctAR_Commission.businessid AND CoaComisi.Coa=iPxAcctAR_Commission.CommissionCoa "
        sSQL += "where iPxAcctAR_Cfg_Customer.businessid = '" & Session("sBusinessID") & "' and iPxAcctAR_Cfg_Customer.arGroup='CC' and "
        If Session("sQueryTicket") = "" Then
            Session("sQueryTicket") = Session("sCondition")
            If Session("sQueryTicket") <> "" Or Session("sCondition") <> "" Then
                sSQL = sSQL & Session("sQueryTicket")
                Session("sCondition") = ""
            Else
                sSQL = sSQL & "iPxAcctAR_Cfg_Customer.IsActive='" & "Y" & "'"
            End If
        Else
            sSQL = sSQL & Session("sQueryTicket")
            Session("sCondition") = ""
        End If
        sSQL += " order by iPxAcctAR_Cfg_Customer.CustomerID asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvCustCrad.DataSource = dt
                    gvCustCrad.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvCustCrad.DataSource = dt
                    gvCustCrad.DataBind()
                    gvCustCrad.Rows(0).Visible = False
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
            listCustomerCC()
        End If
    End Sub

    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvCustAR.PageIndex = e.NewPageIndex
        Me.listCustomerAR()
        gvCustCrad.PageIndex = e.NewPageIndex
        Me.listCustomerCC()
    End Sub

    Protected Sub gvCustAR_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvCustAR.PageIndexChanging
        gvCustAR.PageIndex = e.NewPageIndex
        listCustomerAR()
    End Sub

    Protected Sub gvCustCrad_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvCustCrad.PageIndexChanging
        gvCustCrad.PageIndex = e.NewPageIndex
        listCustomerCC()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CCActive", "CCActive()", True)
    End Sub

    Protected Sub OnPageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvCustAR.PageIndex = e.NewPageIndex
        Me.listCustomerAR()
        gvCustCrad.PageIndex = e.NewPageIndex
        Me.listCustomerCC()
    End Sub
    Protected Sub lbAddCust_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAddCust.Click
        Session("sBiEdit") = ""
        Response.Redirect("iPxInputCustomerAR.aspx")
    End Sub

    Protected Sub gvCustAR_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvCustAR.RowCommand
        If e.CommandName = "getEdit" Then
            Session("sBiEdit") = e.CommandArgument.ToString
            If oCnct.State = ConnectionState.Closed Then
                oCnct.Open()
            End If
            oSQLCmd = New SqlCommand(sSQL, oCnct)
            sSQL = "SELECT arGroup FROM iPxAcctAR_Cfg_Customer WHERE CustomerID = '" & Session("sBiEdit") & "'"
            oSQLCmd.CommandText = sSQL
            oSQLReader = oSQLCmd.ExecuteReader

            oSQLReader.Read()
            If oSQLReader.HasRows Then
                Dim AR As String = oSQLReader.Item("ARGroup").ToString
                oCnct.Close()
                    Response.Redirect("iPxInputCustomerAR.aspx")
            Else
                oCnct.Close()
            End If
        End If
    End Sub

    Protected Sub lbQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbQuery.Click
        ARGroup()
        CoyGroup()
        country()
        city()
        province()
        tbCoyName.Text = ""
        tbCredit.Text = ""
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
            Session("sCondition") = Session("sCondition") & " iPxAcctAR_Cfg_Customer.IsActive='Y' "
        ElseIf dlQStatus.SelectedIndex = 1 Then
            Session("sCondition") = Session("sCondition") & " iPxAcctAR_Cfg_Customer.IsActive = 'Y' or IsActive = 'N' "
        ElseIf dlQStatus.SelectedIndex = 2 Then
            Session("sCondition") = Session("sCondition") & " iPxAcctAR_Cfg_Customer.IsActive = 'N'"
        ElseIf dlQStatus.SelectedIndex = 3 Then
            Session("sCondition") = Session("sCondition") & " iPxAcctAR_Cfg_Customer.IsActive = 'Y'"
        End If
        If dlArgroup.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " and (iPxAcctAR_Cfg_Customer.arGroup = '" & dlArgroup.SelectedValue & "') "
        End If
        If dlCoyGroup.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " and (iPxAcctAR_Cfg_Customer.CoyGroup = '" & dlCoyGroup.SelectedValue & "') "
        End If
        If tbCustID.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " and (iPxAcctAR_Cfg_Customer.CustomerID like '%" & Replace(tbCustID.Text.Trim, "'", "''") & "%') "
        End If
        If tbCoyName.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " and (iPxAcctAR_Cfg_Customer.CoyName like '%" & Replace(tbCoyName.Text.Trim, "'", "''") & "%') "
        End If
        If tbCredit.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " and (iPxAcctAR_Cfg_Customer.CreditLimit like '%" & Replace(tbCredit.Text.Trim, "'", "''") & "%') "
        End If
        If dlCountry.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " and (iPxAcctAR_Cfg_Customer.CountryId = '" & dlCountry.SelectedValue & "') "
        End If
        If dlProvince.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " and (iPxAcctAR_Cfg_Customer.provid = '" & dlProvince.SelectedValue & "') "
        End If
        If dlCity.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " and (iPxAcctAR_Cfg_Customer.CityID = '" & dlCity.SelectedValue & "') "
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQuery", "hideModalQuery()", True)
        listCustomerAR()
        listCustomerCC()
    End Sub

    Protected Sub lbAbortQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortQuery.Click
        Session("sCondition") = ""
        Session("sQueryTicket") = ""
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQuery", "hideModalQuery()", True)
        listCustomerAR()
    End Sub

    Protected Sub lbAddCC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAddCC.Click
        Session("sBiEdit") = ""
        Response.Redirect("iPxInputCustomerCC.aspx")
    End Sub

    Protected Sub gvCustCrad_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvCustCrad.RowCommand
        If e.CommandName = "getEdit" Then
            Session("sBiEdit") = e.CommandArgument.ToString
            If oCnct.State = ConnectionState.Closed Then
                oCnct.Open()
            End If
            oSQLCmd = New SqlCommand(sSQL, oCnct)
            sSQL = "SELECT arGroup FROM iPxAcctAR_Cfg_Customer WHERE CustomerID = '" & Session("sBiEdit") & "'"
            oSQLCmd.CommandText = sSQL
            oSQLReader = oSQLCmd.ExecuteReader

            oSQLReader.Read()
            If oSQLReader.HasRows Then
                Dim AR As String = oSQLReader.Item("ARGroup").ToString
                oCnct.Close()
                Response.Redirect("iPxInputCustomerCC.aspx")
            Else
                oCnct.Close()
            End If
        End If
    End Sub
End Class
