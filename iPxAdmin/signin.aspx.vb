Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration
Imports System.Net.Mail
Imports System.Net
Imports System.Security.Cryptography.X509Certificates
Imports System.Net.Security
Partial Class iPXADMIN_signin
    Inherits System.Web.UI.Page
    Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
    Dim oSQLCmd As SqlCommand
    Dim oSQLReader As SqlDataReader
    Dim sSQL As String
    Dim sActivationCode As String

    Protected Sub Logon_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles submit.Click
        If chkRemanagerMe.Checked Then
            writecookie()
        End If

        oCnct.Open()

        sSQL = "Select * From iPx_profile_user as a INNER JOIN iPx_profile_client AS b ON b.userid=a.id Where businesstype='26' and usercode = '" & txtUsername.Text & "' "
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        oSQLReader = oSQLCmd.ExecuteReader

        If oSQLReader.Read Then
            Dim chk As String
            If Trim(oSQLReader.Item("password")) = Trim(txtPassword.Text) Then
                Session("sUserCode") = oSQLReader.Item("usercode").ToString.Trim
                Session("iUserID") = oSQLReader.Item("id")
                Session("sUserName") = oSQLReader.Item("fullname")
                Session("sStatus") = oSQLReader.Item("status")
                Session("sEmailBussiness") = oSQLReader.Item("usercode")
                Session("sBussinessName") = oSQLReader.Item("businessname")
                Session("sBussinessAddress") = oSQLReader.Item("address")
                Session("sBussinessPhone") = oSQLReader.Item("phone")
                chk = Session("sUserName")
                ' Baru
                Session("iOwnerID") = Session("iUserID")
                Session("sUserGroup") = "0"
                Session("lFirstLogon") = True
                oSQLReader.Close()
                Response.Redirect("logon.aspx")
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Sorry Your Password Is Wrong !');document.getElementById('Buttonx').click()", True)
            End If


        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Sorry User Is Not Registered Yet !');document.getElementById('Buttonx').click()", True)

        End If
        oCnct.Close()


    End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load

        If txtUsername.Text = "" Then
            readcookie()
        End If

        Session("lPass") = False

    End Sub

    Protected Sub btnRequestPassword_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRequestPassword.Click
        oCnct.Open()

        sSQL = "Select * From iPx_profile_user as a INNER JOIN iPx_profile_client AS b ON b.userid=a.id Where businesstype='26' and usercode = '" & txtUsername.Text & "' "
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        oSQLReader = oSQLCmd.ExecuteReader

        If oSQLReader.Read Then
            If SendEmail(Trim(oSQLReader.Item("fullname")), Trim(oSQLReader.Item("usercode")), Trim(oSQLReader.Item("password"))) Then
                MsgBox("Password has been sent to your email !", MsgBoxStyle.OkOnly)

            End If
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Sorry Your Account Has Not Registered Yet !');document.getElementById('Buttonx').click()", True)
        End If
        oCnct.Close()
    End Sub

    Protected Function SendEmail(ByVal cUserName As String, ByVal cUsercode As String, ByVal cPassword As String) As Boolean

        Using mm As New MailMessage(ConfigurationManager.AppSettings("UserName"), cUsercode)
            mm.Subject = "Password Request"
            Dim body As String = "Hello " + cUserName + ","
            body += "<br /><br />Please find below detail of your account login"
            body += "<br /><br />   Username : " & cUsercode & vbCrLf
            body += "<br /><br />   Username : " & cPassword & vbCrLf

            body += "<br /><a href = '" + Request.Url.AbsoluteUri.Replace("signup.aspx", Convert.ToString("signin.aspx")) + "'>Click here to login to your account.</a>"
            body += "<br /><br />Thanks"
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
        SendEmail = True
    End Function

    Sub writecookie()
        'Create a Cookie with a suitable Key.
        Dim userCookie As New HttpCookie("user")
        Dim passCookie As New HttpCookie("pass")
        'Set the Cookie value.
        userCookie.Values("user") = txtUsername.Text
        passCookie.Values("pass") = txtPassword.Text
        'Set the Expiry date.
        userCookie.Expires = DateTime.Now.AddDays(30)
        passCookie.Expires = DateTime.Now.AddDays(30)
        'Add the Cookie to Browser.
        Response.Cookies.Add(userCookie)
        Response.Cookies.Add(passCookie)
    End Sub

    Sub readcookie()
        'Fetch the Cookie using its Key.
        Dim userCookie As HttpCookie = Request.Cookies("user")
        Dim passCookie As HttpCookie = Request.Cookies("pass")
        'If Cookie exists fetch its value.
        Dim user As String = If(userCookie IsNot Nothing, userCookie.Value.Split("="c)(1), "undefined")
        Dim pass As String = If(passCookie IsNot Nothing, passCookie.Value.Split("="c)(1), "undefined")
        If user <> "undefined" Then



            If oCnct.State = ConnectionState.Closed Then
                oCnct.Open()
            End If
            sSQL = "Select * From iPx_profile_user  as a INNER JOIN iPx_profile_client AS b ON b.userid=a.id Where businesstype='26' and usercode = '" & user & "' "
            oSQLCmd = New SqlCommand(sSQL, oCnct)
            oSQLReader = oSQLCmd.ExecuteReader

            If oSQLReader.Read Then
                If Trim(oSQLReader.Item("password")) = Trim(pass) Then
                    Session("sUserCode") = oSQLReader.Item("usercode").ToString.Trim
                    Session("iUserID") = oSQLReader.Item("id")
                    Session("sUserName") = oSQLReader.Item("fullname")
                    Session("sStatus") = oSQLReader.Item("status")
                    Session("sEmailBussiness") = oSQLReader.Item("usercode")
                    ' Baru
                    Session("iOwnerID") = Session("iUserID")
                    Session("sUserGroup") = "0"
                    Session("lFirstLogon") = True
                    oSQLReader.Close()
                    Response.Redirect("logon.aspx")
                Else
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Sorry Your Password Is Wrong !');document.getElementById('Buttonx').click()", True)
                End If


            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Sorry User Is Not Registered Yet !');document.getElementById('Buttonx').click()", True)

            End If
        End If
        oCnct.Close()
LBL_X:


    End Sub

End Class
