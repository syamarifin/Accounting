Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration
Imports System.Net.Mail
Imports System.Net
Imports System.Security.Cryptography.X509Certificates
Imports System.Net.Security
Partial Class iPxDashboard_signupreq
    Inherits System.Web.UI.Page
    Public sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Public oCnct As SqlConnection = New SqlConnection(sCnct)
    Public oSQLCmd As New SqlCommand
    Public oSQLReader As SqlDataReader
    Public sSQL As String
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

    Sub showdata_dropdowncountry()
        'Dim cmd As New SqlCommand("SELECT * FROM iPx_profile_geog_country order by countryid ASC")
        'drpCountry.DataSource = ExecuteQuery(cmd, "SELECT")
        'drpCountry.DataTextField = "country"
        'drpCountry.DataValueField = "countryid"
        'drpCountry.DataBind()
        'drpCountry.Items.Insert(0, "-Select-")

    End Sub

    Sub showdata_dropdowncity()
        'Dim cmd As New SqlCommand("SELECT  RTRIM(cityid) AS cityid,city FROM iPx_profile_geog_city where countryid='" & drpCountry.SelectedValue.Trim & "' order by city ASC")
        'drpCity.DataSource = ExecuteQuery(cmd, "SELECT")
        'drpCity.DataTextField = "city"
        'drpCity.DataValueField = "cityid"
        'drpCity.DataBind()
        'drpCity.Items.Insert(0, "-Select-")
    End Sub

    Sub insert()

        If isAccountRegistered() Then
        Else
            If txtContactPerson.Text = "" Or txtEmail.Text = "" Or txtHotelname.Text = "" Or txtMobile.Text = "" Or txtaddress.Text = "" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Please Complete Data First !');document.getElementById('Buttonx').click()", True)

            Else
                txtHotelname.Text = txtHotelname.Text.Replace("'", "`")


                'Try


                If oCnct.State = ConnectionState.Closed Then
                    oCnct.Open()
                End If

                oSQLCmd = New SqlCommand(sSQL, oCnct)
                'hotelname, noofroom, contactperson, mobile, email, password, address,country, city, website, registerfor, registerdate, status, clerknotes, guestnotes,   approvalnotes, approvaldate
                sSQL = "INSERT INTO iPx_profile_user_signup( hotelname, noofroom, contactperson, mobile, email, password, address, website, registerfor, registerdate, status, clerknotes, guestnotes, approvalnotes,approvaldate) values"
                sSQL = sSQL & " (  '" & txtHotelname.Text & "', '', '" & txtContactPerson.Text & "', '" & txtMobile.Text & "', '" & txtEmail.Text & "', '','" & txtaddress.Text & "', "
                sSQL = sSQL & " '" & txtWebsite.Text & "','26','" & Now & "','7','','Request Trial 7 Days','','1900/01/01')"
                oSQLCmd.CommandText = sSQL
                oSQLCmd.ExecuteNonQuery()

                SendEmail()
                oCnct.Close()
                clear()
                ' ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Thank you " & txtHotelname.Text & vbCrLf & vbCrLf & "Your Request has been submitted !" & vbCrLf & "Please check your email for detail !" & "');document.getElementById('Buttonx').click()", True)
                'On Error Resume Next
                'Response.Redirect(Session("sURI"))
                Session("sMessage") = "Thank you " & txtHotelname.Text & vbCrLf & vbCrLf & "Your Request has been submitted !" & vbCrLf & "Please check your email for detail !" & " |"

                Session("sWarningID") = "0"
                Session("sUrlOKONLY") = "https://www.alcor-sys.com/"
                Session("sUrlYES") = "http://www.thepyxis.net"
                Session("sUrlNO") = "http://www.thepyxis.net"
                Response.Redirect("warningmsg.aspx")
                'Catch ex As Exception
                '    Session("sMessage") = "Registered Successfull ! |"

                '    Session("sWarningID") = "0"
                '    Session("sUrlOKONLY") = "signupreq.aspx"
                '    Session("sUrlYES") = "http://www.thepyxis.net"
                '    Session("sUrlNO") = "http://www.thepyxis.net"
                '    Response.Redirect("warningmsg.aspx")
                'End Try

            End If
        End If



    End Sub
    Protected Sub btnsubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsubmit.Click
        If chkAgree.Checked = True Then
            insert()
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Please tick I agree if you approve it!');document.getElementById('Buttonx').click()", True)

        End If

    End Sub
    Sub clear()
        txtContactPerson.Text = ""
        txtEmail.Text = ""
        txtHotelname.Text = ""
        txtMobile.Text = ""
        txtWebsite.Text = ""
        txtaddress.Text = ""


    End Sub
    Protected Function SendEmail() As Boolean

        Using mm As New MailMessage(ConfigurationManager.AppSettings("UserName"), txtEmail.Text)
            Dim emailbcc As New MailAddress(ConfigurationManager.AppSettings("from"))
            mm.Bcc.Add(emailbcc)
            mm.Subject = "Trial 7 Days Free Request"
            Dim body As String


            'body = "<div class=""center""> <img alt="" src=""../images/logo/alcor-logo.png"" />"
            body = "<br /><br />"
            body += "Hello " + txtHotelname.Text.Trim() + ","

            body += "<br /><br />Thank you for Registering ALCOR"
            body += "<br /><br />Please wait for approval, our account staff will approach you soon in 2 x 24 hour"

            body += "<br /><br /><br /><br />Thanks"
            mm.Body = body
            mm.IsBodyHtml = True

            '=====================================================
            Dim smtp As New SmtpClient()
            smtp.Host = ConfigurationManager.AppSettings("Host")
            smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings("EnableSsl"))
            Dim NetworkCred As New NetworkCredential(ConfigurationManager.AppSettings("UserName"), ConfigurationManager.AppSettings("Password"))
            smtp.UseDefaultCredentials = True
            smtp.Credentials = NetworkCred
            smtp.Port = Integer.Parse(ConfigurationManager.AppSettings("Port"))
            ServicePointManager.ServerCertificateValidationCallback = Function(s As Object, certificate As X509Certificate, chain As X509Chain, sslPolicyErrors As SslPolicyErrors) True

            smtp.Send(mm)
        End Using
    End Function
    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        pnlReg.Visible = True
        ' On Error Resume Next
        ' Response.Redirect("https://www.alcor-sys.com/")
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'On Error Resume Next
        'Session("sURI") = Request.UrlReferrer.AbsoluteUri

        If Not Page.IsPostBack Then
            'showdata_dropdowncountry()

        End If
    End Sub
    Protected Function isAccountRegistered() As Boolean
        Dim lRet As Boolean
        oCnct.Open()
        lRet = False
        sSQL = "Select status From iPx_profile_user Where rtrim(usercode) = '" & txtEmail.Text & "' "
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        oSQLReader = oSQLCmd.ExecuteReader

        If oSQLReader.Read Then
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Sorry Account Has Been Registered !');document.getElementById('Buttonx').click()", True)
            Session("sMessage") = "Sorry Account Has Been Registered ! |"

            Session("sWarningID") = "0"
            Session("sUrlOKONLY") = "signupreq.aspx"
            Session("sUrlYES") = "http://www.thepyxis.net"
            Session("sUrlNO") = "http://www.thepyxis.net"
            Response.Redirect("warningmsg.aspx")
            lRet = True

        End If
        oCnct.Close()
        Return lRet
    End Function
End Class
