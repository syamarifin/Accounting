Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration
Imports System.Net.Mail
Imports System.Net
Imports System.Security.Cryptography.X509Certificates
Imports System.Net.Security
Partial Class iPxAdmin_signup
    Inherits System.Web.UI.Page
    Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
    Dim oSQLCmd As SqlCommand
    Dim oSQLReader As SqlDataReader
    Dim sSQL As String
    Dim sActivationCode As String
    Dim cPckg As String

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Dim lOK As Boolean
        Dim sMessage As String
        Dim i As Int16
        Dim lNum As Boolean
        Dim lChar As Boolean

        If chkAgreeement.Checked Then
            lOK = False
            If Len(Trim(txtPassword.Text)) < 8 Then
                sMessage = "Sorry for security reason password should be 8 chars minimum !"
            Else

                lNum = False
                lChar = False
                For i = 1 To Len(Trim(txtPassword.Text))
                    If InStr("0123456789", Mid(Trim(txtPassword.Text), i, 1)) > 0 Then
                        lNum = True
                    Else
                        lChar = True

                    End If
                Next
                If lNum And lChar Then
                    lOK = True
                Else
                    sMessage = "Sorry for security reason password should be 8 chars minimum and combination alphabet and numerical !"


                End If
            End If

            If Not lOK Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('" & sMessage & "');document.getElementById('Buttonx').click()", True)


            Else
                If txtPassword.Text = txtPasswordCFRM.Text Then
                    If Not isAccountRegistered() Then
                        sActivationCode = Guid.NewGuid().ToString()
                        oCnct.Open()

                        sSQL = "insert into iPx_profile_user (usercode,mobileno, password, signupdate, fullname, status, guid) values('" & txtEmail.Text & "','NA','" & txtPassword.Text & "','" & Format(Now, "yyyy/MM/dd hh:mm") & "', '" & txtFullname.Text & "','N','" & sActivationCode & "')"
                        oSQLCmd = New SqlCommand(sSQL, oCnct)
                        oSQLCmd.ExecuteNonQuery()
                        SendEmail()

                        Session("sMessage") = "Dear " & txtFullname.Text.Trim & " thank you for signup with iPx-ALCOR !|Activation link has been sent to your email address !|For security reason, Please login to your email to activate it||"
                        Session("sWarningID") = "0"
                        Session("sUrlOKONLY") = "signin.aspx"
                        Session("sUrlYES") = "http://www.thepyxis.net"
                        Session("sUrlNO") = "http://www.thepyxis.net"
                        Response.Redirect("warningmsg.aspx")
                    End If

                Else
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Sorry password and Re-Type Password not match!');document.getElementById('Buttonx').click()", True)
                End If
            End If
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Please Check The Term Agreement First');document.getElementById('Buttonx').click()", True)
        End If

    End Sub

    Protected Function isAccountRegistered() As Boolean
        Dim lRet As Boolean
        oCnct.Open()
        lRet = False
        sSQL = "Select status From iPx_profile_user_signup Where rtrim(usercode) = '" & txtEmail.Text & "' "
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        oSQLReader = oSQLCmd.ExecuteReader

        If oSQLReader.Read Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Sorry Account Has Been Registered !');document.getElementById('Buttonx').click()", True)
            lRet = True

        End If
        oCnct.Close()
        Return lRet
    End Function

    Protected Function SendEmail() As Boolean

        Using mm As New MailMessage(ConfigurationManager.AppSettings("UserName"), txtEmail.Text)
            mm.Subject = "Account Activation"
            Dim body As String = "Hello " + txtFullname.Text.Trim() + ","

            body += "<br /><br />Thank you for signedup "
            body += "<br /><br />Please click the following link to activate your account"
            body += "<br /><h4><a href = '" + Request.Url.AbsoluteUri.Replace("signup.aspx", Convert.ToString("signin.aspx?ActivationCode=") & sActivationCode) + "?" & cPckg & "?00?T'>Click here to activate your account.</a></h4>"
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        cPckg = If(Not String.IsNullOrEmpty(Request.QueryString("cp")), Request.QueryString("cp"), "0")

        If Not Page.IsPostBack Then
            txtEmail.Text = Trim(Session("sUserCode"))
        End If

    End Sub

    Protected Sub txtEmail_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtEmail.TextChanged

    End Sub
End Class
