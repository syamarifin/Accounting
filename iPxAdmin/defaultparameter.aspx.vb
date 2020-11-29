Imports System.Data
Imports System.Data.SqlClient
Partial Class iPxAdmin_defaultparameter
    Inherits System.Web.UI.Page
    Public sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    'Dim sCnctPMS As String = ConfigurationManager.ConnectionStrings("iPxCNCTPMS").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
    'Dim oCnctPMS As SqlConnection = New SqlConnection(sCnctPMS)
    Public oSQLCmd As New SqlCommand
    Public oSQLReader As SqlDataReader
    Public sSQL, sSQLPMS As String
    Dim cIpx As New iPxClass
    Dim jobid As String
    Public Function ExecuteQuery(ByVal cmd As SqlCommand, ByVal action As String) As DataTable
        Dim conString As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ConnectionString
        Using con As New SqlConnection(conString)
            cmd.Connection = con
            Select Case action
                Case "SELECT"
                    Using sda As New SqlDataAdapter()
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dt)
                            Return dt
                        End Using
                    End Using
                Case "UPDATE"
                    con.Open()
                    cmd.ExecuteNonQuery()
                    con.Close()
                    Exit Select
            End Select
            Return Nothing
        End Using
    End Function
    Sub UserAcces()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.businessid, a.usercode, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.funtionid='34'and x.active='Y' and x.businessid=a.businessid and x.usercode=a.usercode) as AddOp "
        sSQL += "from iPxAcct_profile_client_useraccess as a "
        sSQL += "INNER JOIN iPxAcct_profile_client_useraccess_dtl as b ON b.businessid=a.businessid and b.usercode=a.usercode "
        sSQL += "where a.businessid='" & Session("sBusinessID") & "' and a.usercode='" & Session("sUserCode") & "' and a.active='Y' group by a.businessid, a.usercode "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            If oSQLReader.Item("AddOp").ToString = "Y" Then
                Linkbutton1.Enabled = True
            Else
                Linkbutton1.Enabled = False
            End If
        Else
            Linkbutton1.Enabled = False
        End If
        oCnct.Close()
    End Sub
    Sub showdata_dropdownPMSLink()
        sSQL = "SELECT a.businessid, a.Description, a.FoLink, b.businessname "
        sSQL += "from iPxAcct_FOlink as a "
        sSQL += "INNER JOIN iPx_profile_client AS b ON a.FoLink = b.businessid "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "'"
        sSQL += " order by a.Description asc"
        Dim cmd As New SqlCommand(sSQL)
        ddlPMSlink.DataSource = ExecuteQuery(cmd, "SELECT")
        ddlPMSlink.DataTextField = "businessname"
        ddlPMSlink.DataValueField = "FoLink"
        ddlPMSlink.DataBind()
        ddlPMSlink.Items.Insert(0, "-Select-")

    End Sub
    Sub showdata_dropdowntranstype()
        Dim cmd As New SqlCommand("SELECT * FROM iPxAcctAR_Cfg_Transactiontype where businessid ='" & Session("sBusinessID") & "' order by TransactionType ASC")
        dlTransType.DataSource = ExecuteQuery(cmd, "SELECT")
        dlTransType.DataTextField = "Description"
        dlTransType.DataValueField = "TransactionType"
        dlTransType.DataBind()
        dlTransType.Items.Insert(0, "-Select-")

    End Sub
    Sub showdata_dropdowntranstypeAP()
        Dim cmd As New SqlCommand("SELECT * FROM iPxAcctAP_Cfg_Transactiontype where businessid ='" & Session("sBusinessID") & "' order by TransactionType ASC")
        dlTransTypeAP.DataSource = ExecuteQuery(cmd, "SELECT")
        dlTransTypeAP.DataTextField = "Description"
        dlTransTypeAP.DataValueField = "TransactionType"
        dlTransTypeAP.DataBind()
        dlTransTypeAP.Items.Insert(0, "-Select-")

    End Sub

    Sub showdata_dropdowncountry()
        Dim cmd As New SqlCommand("SELECT * FROM iPx_profile_geog_country order by countryid ASC")
        ddlCountry.DataSource = ExecuteQuery(cmd, "SELECT")
        ddlCountry.DataTextField = "country"
        ddlCountry.DataValueField = "countryid"
        ddlCountry.DataBind()
        ddlCountry.Items.Insert(0, "-Select-")

    End Sub

    Sub showdata_dropdowncity()
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPx_profile_geog_city where countryid ='" & ddlCountry.SelectedValue.Trim & "' and provid = '" & ddlProvince.SelectedValue.Trim & "' order by city asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                ddlCity.DataSource = dt
                ddlCity.DataTextField = "city"
                ddlCity.DataValueField = "cityid"
                ddlCity.DataBind()
                ddlCity.Items.Insert(0, "-Select-")
            End Using
        End Using
    End Sub
    Sub showdata_dropdownNationality()
        Dim cmd As New SqlCommand("SELECT nationalityid, description FROM iPx_profile_geog_nationality order by description ASC")
        ddlNationality.DataSource = ExecuteQuery(cmd, "SELECT")
        ddlNationality.DataTextField = "description"
        ddlNationality.DataValueField = "nationalityid"
        ddlNationality.DataBind()
        ddlNationality.Items.Insert(0, "-Select-")
    End Sub
    Sub showdata_dropdownProvince()
        Dim cmd As New SqlCommand("SELECT countryid, provid, description FROM iPx_profile_geog_province where countryid='" & ddlCountry.SelectedValue & "' order by description ASC")
        ddlProvince.DataSource = ExecuteQuery(cmd, "SELECT")
        ddlProvince.DataTextField = "description"
        ddlProvince.DataValueField = "provid"
        ddlProvince.DataBind()
        ddlProvince.Items.Insert(0, "-Select-")
    End Sub
    Sub transdefault()
        showdata_dropdowntranstype()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcctAR_Cfg_Transactiontype "
        sSQL += "WHERE businessid='" & Session("sBusinessID") & "' and isDefault ='Y'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            dlTransType.SelectedValue = oSQLReader.Item("TransactionType").ToString
            oCnct.Close()
        Else
            oCnct.Close()
        End If
    End Sub
    Sub transdefaultAP()
        showdata_dropdowntranstypeAP()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcctAP_Cfg_Transactiontype "
        sSQL += "WHERE businessid='" & Session("sBusinessID") & "' and isDefault ='Y'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            dlTransTypeAP.SelectedValue = oSQLReader.Item("TransactionType").ToString
            oCnct.Close()
        Else
            oCnct.Close()
        End If
    End Sub
    Sub updatetransdefault()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "UPDATE iPxAcctAR_Cfg_Transactiontype SET  isDefault='Y' "
        sSQL = sSQL & "WHERE TransactionType ='" & dlTransType.SelectedValue & "' and businessid='" & Session("sBusinessID") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
    Sub updatetransdefaultAP()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "UPDATE iPxAcctAP_Cfg_Transactiontype SET  isDefault='Y' "
        sSQL = sSQL & "WHERE TransactionType ='" & dlTransTypeAP.SelectedValue & "' and businessid='" & Session("sBusinessID") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
    Sub updatetransnondefault()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "UPDATE iPxAcctAR_Cfg_Transactiontype SET  isDefault='N' "
        sSQL = sSQL & "WHERE TransactionType <> '" & dlTransType.SelectedValue & "' and businessid='" & Session("sBusinessID") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
    Sub updatetransnondefaultAP()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "UPDATE iPxAcctAP_Cfg_Transactiontype SET  isDefault='N' "
        sSQL = sSQL & "WHERE TransactionType <> '" & dlTransTypeAP.SelectedValue & "' and businessid='" & Session("sBusinessID") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Dim cph As ContentPlaceHolder = DirectCast(Me.Master.FindControl("ContentPlaceHolder2"), ContentPlaceHolder)
        'Dim label As Label = DirectCast(cph.FindControl("lblMasterTitle"), Label)
        'label.Text = "Default Parameter"

        If Session("sBusinessID") = "" Then
            Response.Redirect("signin.aspx")
        End If
        If Not Page.IsPostBack Then
            If cIpx.getAccessUser(Session("sBusinessID"), Session("sUserCode"), "Option") <> True Then

                Session("sMessage") = "Sorry, you dont have access in this module |"
                Session("sMemberid") = ""
                Session("sWarningID") = "0"
                Session("sUrlOKONLY") = "home.aspx"
                Session("sUrlYES") = "http://www.thepyxis.net"
                Session("sUrlNO") = "http://www.thepyxis.net"
                Response.Redirect("warningmsg.aspx")
            End If
            showData()
        End If
        UserAcces()
    End Sub
    Sub showData()
        showdata_dropdowncountry()
        ddlCountry.SelectedValue = cIpx.getDefaultParameter(Session("sBusinessID"), "01")

        showdata_dropdownNationality()
        ddlNationality.SelectedValue = cIpx.getDefaultParameter(Session("sBusinessID"), "02")

        showdata_dropdownProvince()
        ddlProvince.SelectedValue = cIpx.getDefaultParameter(Session("sBusinessID"), "03")

        showdata_dropdowncity()
        ddlCity.SelectedValue = cIpx.getDefaultParameter(Session("sBusinessID"), "04")

        showdata_dropdownPMSLink()
        ddlPMSlink.SelectedValue = cIpx.getDefaultParameter(Session("sBusinessID"), "10")

        transdefault()
        transdefaultAP()
    End Sub


    Protected Sub Linkbutton1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Linkbutton1.Click
        cIpx.setDefaultParam(Session("sBusinessID"), "01", "Country", ddlCountry.SelectedValue)
        cIpx.setDefaultParam(Session("sBusinessID"), "02", "Nationality", ddlNationality.SelectedValue)
        cIpx.setDefaultParam(Session("sBusinessID"), "03", "Province", ddlProvince.SelectedValue)
        cIpx.setDefaultParam(Session("sBusinessID"), "04", "City", ddlCity.SelectedValue)
        cIpx.setDefaultParam(Session("sBusinessID"), "10", "Fo Link", ddlPMSlink.SelectedValue)
        updatetransdefault()
        updatetransdefaultAP()
        updatetransnondefault()
        updatetransnondefaultAP()

        Session("sMessage") = "Data has been save !| ||"
        Session("sWarningID") = "0"
        Session("sUrlOKONLY") = "defaultparameter.aspx"
        Session("sUrlYES") = "http://www.thepyxis.net"
        Session("sUrlNO") = "http://www.thepyxis.net"
        Response.Redirect("warningmsg.aspx")
    End Sub




    Protected Sub drpCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCountry.SelectedIndexChanged
        showdata_dropdownProvince()
        showdata_dropdowncity()
    End Sub

    Protected Sub ddlProvince_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlProvince.SelectedIndexChanged
        showdata_dropdowncity()
    End Sub


    Protected Sub btnCxld_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCxld.Click
        Response.Redirect("home.aspx")
    End Sub
End Class