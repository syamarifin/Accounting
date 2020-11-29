
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing

Imports System.Configuration
Imports System.Net.Mail
Imports System.Net
Imports System.Security.Cryptography.X509Certificates
Imports System.Net.Security

Imports iTextSharp.text
Imports iTextSharp.text.html.simpleparser
Imports iTextSharp.text.pdf

Imports System.Text
Imports System.Web
Imports System.Net.Mime
Imports System.Globalization
Imports System.IO

Partial Class iPxDashboard_DetailUserAccess
    Inherits System.Web.UI.Page
    Public sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Public oCnct As SqlConnection = New SqlConnection(sCnct)
    Public oSQLCmd As New SqlCommand
    Public oSQLReader As SqlDataReader
    Public sSQL As String
    Dim cIpx As New iPxClass

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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim cph As ContentPlaceHolder = DirectCast(Me.Master.FindControl("ContentPlaceHolder4"), ContentPlaceHolder)
        Dim label As Label = DirectCast(cph.FindControl("lblMasterTitle"), Label)
        label.Text = "User Client Access"

        If Session("sEmailClient") = "" Then
            Response.Redirect("signin.aspx")
        End If
        If Not Page.IsPostBack Then


            showdata_dropdowncountry()

            txtusercode.Text = Session("sEmailClient")
            getData()
            getDataClient()
            guestphoto.ImageUrl = "Handler.ashx?ID=0|" & Session("sBusinessID") & ""

        End If

        If Session("sRequest") = True Then
            getRequest()
        End If


    End Sub

    Sub getRequest()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If

        Dim active As String
        If chkStatus.Checked Then
            active = "Y"
        Else
            active = "N"
        End If

        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM   iPx_profile_user_signup where email='" & Session("sEmailClient").ToString.Trim & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        txtusercode.ReadOnly = True
        While oSQLReader.Read
            'SELECT        id, , noofroom, , , , password, , , registerfor, registerdate, status, clerknotes, guestnotes, approvalnotes, approvaldate()
            txtusercode.Text = oSQLReader.Item("email").ToString.Trim
            txtmobile.Text = oSQLReader.Item("mobile").ToString.Trim
            txtbsname.Text = oSQLReader.Item("hotelname").ToString.Trim
            txtaddrs.Text = oSQLReader.Item("address").ToString.Trim
            txttaxno.Text = ""
            txtphone.Text = oSQLReader.Item("mobile").ToString.Trim
            txtfax.Text = ""
            txtconpers.Text = oSQLReader.Item("contactperson").ToString.Trim
            txtweb.Text = oSQLReader.Item("website").ToString.Trim
            chkStatus.Checked = oSQLReader.Item(active)

            showdata_dropdowncountry()
            drpCountry.SelectedValue = oSQLReader.Item("country").ToString.Trim
            showdata_dropdowncity()
            drpCity.SelectedValue = oSQLReader.Item("city").ToString.Trim
            


        End While
    End Sub

    Sub showdata_dropdowncountry()
        Dim cmd As New SqlCommand("SELECT * FROM iPx_profile_geog_country order by countryid ASC")
        drpCountry.DataSource = ExecuteQuery(cmd, "SELECT")
        drpCountry.DataTextField = "country"
        drpCountry.DataValueField = "countryid"
        drpCountry.DataBind()
        drpCountry.Items.Insert(0, "-Select-")

    End Sub

    Sub showdata_dropdowncity()
        Dim cmd As New SqlCommand("SELECT  RTRIM(cityid) AS cityid,city FROM iPx_profile_geog_city where countryid='" & drpCountry.SelectedValue.Trim & "' order by city ASC")
        drpCity.DataSource = ExecuteQuery(cmd, "SELECT")
        drpCity.DataTextField = "city"
        drpCity.DataValueField = "cityid"
        drpCity.DataBind()
        drpCity.Items.Insert(0, "-Select-")
    End Sub

    Sub getData()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If

        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPx_profile_user where usercode='" & Session("sEmailClient").ToString.Trim & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        txtusercode.ReadOnly = True
        While oSQLReader.Read
            'SELECT        TOP (200) id, usercode, mobileno, password, signupdate, fullname, status, guid FROM            iPx_profile_user
            txtusercode.Text = oSQLReader.Item("usercode").ToString.Trim
            txtmobile.Text = oSQLReader.Item("mobileno").ToString.Trim
            txtbsname.Text = oSQLReader.Item("fullname").ToString.Trim

        End While

        oCnct.Close()
    End Sub

    Sub getDataClient()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If

        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPx_profile_client where email='" & Session("sEmailClient").ToString.Trim & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader
        ', fax, contactperson, mobileno, email, , city, country, userid, registereddate, 
        txtusercode.ReadOnly = True
        While oSQLReader.Read

            txtaddrs.Text = oSQLReader.Item("address").ToString.Trim
            txttaxno.Text = oSQLReader.Item("taxno").ToString.Trim
            txtphone.Text = oSQLReader.Item("phone").ToString.Trim
            txtfax.Text = oSQLReader.Item("fax").ToString.Trim
            txtconpers.Text = oSQLReader.Item("contactperson").ToString.Trim
            txtcurrancy.Text = oSQLReader.Item("currency").ToString.Trim
            txtpromo.Text = oSQLReader.Item("promourl").ToString.Trim
            txtmobile.Text = oSQLReader.Item("mobileno").ToString.Trim
            txtweb.Text = oSQLReader.Item("weburl").ToString.Trim

            showdata_dropdowncountry()
            drpCountry.SelectedValue = oSQLReader.Item("country").ToString.Trim
            showdata_dropdowncity()
            drpCity.SelectedValue = oSQLReader.Item("city").ToString.Trim
            Dim act As String
            act = oSQLReader.Item("status").ToString.Trim

            If act = "Y" Then
                chkStatus.Checked = True
            Else
                chkStatus.Checked = False
            End If
        End While

        oCnct.Close()
    End Sub


    Protected Sub btnCxld_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCxld.Click
        Response.Redirect("viewUserAcc.aspx")
    End Sub

End Class