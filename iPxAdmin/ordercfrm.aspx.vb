Imports System.Data
Imports System.Data.SqlClient
Imports System.Net
Imports System.Net.Mail
Imports System.Security.Cryptography.X509Certificates
Imports System.Net.Security
Partial Class iPxAdmin_ordercfrm
    Inherits System.Web.UI.Page

    Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
    Dim oSQLCmd As SqlCommand
    Dim oSQLReader As SqlDataReader
    Dim sSQL As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim cRetVal As String

        cRetVal = Request.Form.ToString
        'Response.Write(cRetVal)
        'GoTo LX
        ' record payment log
        'sSQL = "insert into iPx_payment_log (paymenttxt) values('" & cRetVal & "') "
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        'oSQLCmd = New SqlCommand(sSQL, oCnct)
        'oSQLCmd.ExecuteNonQuery()


        Select Case Session("sPaymentCode")
            Case "N"
                SendEmail()
                Session("sMessage") = "Thank You !|Your Payment Has Been Accepted !|Please logon to your email to activate your account !|"
            Case "R"

                Dim validNow As Date
                validNow = CheckValidation("")
                Dim cValid As String
                Select Case Session("cPyMode")
                    Case "M"
                        validNow = validNow.AddMonths(1)
                        cValid = validNow.ToString("yyyy/MM/dd")

                        'cValid = Format(DateAdd(DateInterval.Month, 1, validNow), "yyyy/MM/dd")
                    Case "Y"
                        validNow = validNow.AddYears(1)
                        cValid = validNow.ToString("yyyy/MM/dd")
                        'cValid = Format(DateAdd(DateInterval.Year, 1, validNow), "yyyy/MM/dd")
                    Case Else
                        validNow = validNow.AddDays(7)
                        cValid = validNow.ToString("yyyy/MM/dd")
                        'cValid = Format(DateAdd(DateInterval.Day, 7, validNow), "yyyy/MM/dd")

                End Select
                If oCnct.State = ConnectionState.Closed Then
                    oCnct.Open()
                End If
                sSQL = "INSERT into iPx_profile_clientprogram (businessid, registeredpackage, program, noofterminal, validity, refferalid) values('" & Session("sBusinessID") & "','" & Session("sRegisteredPackage") & "','" & Session("sProgram") & "',1,'" & cValid & "',0) "
                '00 - Program iPxPOS
                oSQLCmd.CommandText = sSQL
                oSQLCmd.ExecuteNonQuery()
                sSQL = "update iPx_profile_client set  registeredpackage='" & Session("sRegisteredPackage") & "' where businessid='" & Session("sBusinessID") & "' "
                oSQLCmd.CommandText = sSQL
                oSQLCmd.ExecuteNonQuery()

                sSQL = "update iPx_general_voucher set  status='X',usedby='" & Session("sEmailBussiness") & "' where voucherno='" & Session("sVoucher") & "' "
                oSQLCmd.CommandText = sSQL
                oSQLCmd.ExecuteNonQuery()

                oCnct.Close()
                Session("sMessage") = "Thank You !|Your Payment Has Been Accepted !|You License(s) Activated !|"
                'Case "S"   ' SITE LICENSE PAYMENT
                '    Dim cIpx As New iPxClass
                '    cIpx.InsertNewBusiness(Session("sBusinessID"), Session("sRegisteredPackage"), Session("cPyMode"), Session("nPackageQTY"), Session("sNewBusinesstype"))
                '    Select Case Session("sNewBusinesstype")
                '        Case 14, 15, 16, 17
                '            cIpx.SendEmailPMS(Session("sUserName"), Session("sEmail"))
                '            Session("sMessage") = "Thank You !|Your Payment Has Been Accepted !|You New Business License(s) Activated, user and default password has been sent please check your email!|"
                '        Case 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 18, 19, 20, 21, 22, 23
                '            Session("sMessage") = "Thank You !|Your Payment Has Been Accepted !|You New Business License(s) Activated !|"
                'End Select



            Case "T"   ' TERMINAL LICENSE PAYMENT
                Addterminal()
                Session("sMessage") = "Thank You !|Your Payment Has Been Accepted !|You New Terminal License(s) Activated !|"

        End Select


        'cRetVal = "trax_date=20110715130756&processed_date=20110715130756&trax_type=Payment&merchant_id=0574&invoice=" & Trim(Session("RSVDNO")) & "&amount=" & Session("BKAMT") & "&currency_code=IDR&mer_signature=4ADBDBAD4B4DBD4E12FA3CBCB3C39C39D98D6C6572EB41D13AE1DE25D0C8448E&result_code=0&result_desc=&payment_method=8&log_no=1107150000007&hostref_no=476&payment_info=004715&add_info=&add_param=&card_type=Visa"
        ' sSql = "insert into web_payment (retval) values('" & cRetVal & "') "
        'cnct.CreateCommand()
        'sqlCmd = New SqlCommand(sSql, cnct)
        'sqlCmd.ExecuteNonQuery()

        'aRet = Split(cRetVal, "&")
        'aResult = Split(aRet(8), "=")

        'If Val(aResult(1)) = 0 Then 'ok / approved
        'aResult = Split(aRet(16), "=")
        'Session("CARDTYPE") = aResult(1)
        'aResult = Split(aRet(14), "=")
        'Session("CARDNO") = aResult(1)
        'aResult = Split(aRet(15), "=")
        'Session("EXPIRY") = aResult(1)
        'aResult = Split(aRet(4), "=")
        'If DisplayDetail(aResult(1)) Then
        ' SavePaymentData(cRetVal)
        ' SendMail(Session("EMAIL"), "003", "EN"')
        '          SendMail(Session("HOTELEMAIL"), "013", "EN")

        '         Response.Redirect("~/eBookingEngine/confirmationFORM.aspx")
        ' Else
        '        Session("MESSAGE_1") = "Dear Guest Your Current Booking Failed Due To Database Error"
        '       Session("MESSAGE_2") = "Please Contact Our Customer Service To Solve The Problem"
        '      Session("MESSAGE_3") = "For The Moment We Hold Your Booking, Thanks Using Our Services !"
        '     Session("MODE") = "OKONLY"
        '    Session("OKURL") = "~/eBookingEngine/bookingHotel.aspx"
        '   Session("YESURL") = ""
        '  Session("CANCELURL") = ""
        ' Response.Redirect("~/alert.aspx")
        'End If
        ' Else
        'error
        '    aResult = Split(aRet(4), "=")
        '   cRDR = "~/eBookingEngine/bookingHotel.aspx"
        '  If DisplayDetail(aResult(1)) Then
        'CancelBookingData()
        'cRDR = "~/eBookingEngine/bookingHotel.aspx"
        'End If

        '   aResult = Split(aRet(8), "=")

        '  cRetCode = aResult(1)
        ' aResult = Split(aRet(9), "=")

        'Session("MESSAGE_1") = "Dear Guest Your Current Payment Failed !"
        'Session("MESSAGE_2") = "Fail Code From Payment Processor > " & cRetCode & " " & aResult(1)
        'Session("MESSAGE_3") = "For The Moment We Cancelled Your Booking, Thanks Using Our Services !"
        'Session("MODE") = "OKONLY"
        'Session("OKURL") = cRDR
        'Session("YESURL") = ""
        'Session("CANCELURL") = ""
        'Response.Redirect("~/alert.aspx")

        '        End If

        ' End If

        '       sqlCmd = Nothing
        '      cnct = Nothing

        oCnct.Close()



        Session("sWarningID") = "0"
        Session("sUrlOKONLY") = "home.aspx"
        Session("sUrlYES") = "http://www.thepyxis.net"
        Session("sUrlNO") = "http://www.thepyxis.net"
        Response.Redirect("warningmsg.aspx")
        'LX:

    End Sub

    Private Sub Addterminal()
        Dim nLast As Int16

        sSQL = "select count(businessid) as nt from iPx_profile_clientterminal where businessid='" & Session("sBusinessID") & "' "
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()

        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        oSQLReader = oSQLCmd.ExecuteReader
        If oSQLReader.Read Then
            nLast = oSQLReader.Item("nt")
        End If
        oSQLReader.Close()
        For i = 1 To Val(Session("nPackageQTY"))
            sSQL = "INSERT INTO iPx_profile_clientterminal values('" & Session("sBusinessID") & "', '127.0.0.1', 'Terminal " & Trim(Str(nLast + i)) & "', 'Portrait 5.5 - Simple','N')"
            oSQLCmd.CommandText = sSQL

            oSQLCmd.ExecuteNonQuery()

        Next
        oCnct.Close()
    End Sub

    Protected Function SendEmail() As Boolean

        Using mm As New MailMessage(ConfigurationManager.AppSettings("UserName"), Session("sRegEmail"))

            mm.Subject = "Account Activation"
            mm.Body = Session("sRegBody")
            mm.IsBodyHtml = True

            '============================================
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
    Private Function CheckValidation(ByRef validity As String) As String
        Dim sSql As String
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()

        End If

        sSql = "Select MAX(validity) as validity FROM iPx_profile_clientprogram WHERE (businessid = '" & Session("sBusinessID") & "') "
        oSQLCmd = New SqlCommand(sSql, oCnct)
        oSQLReader = oSQLCmd.ExecuteReader
        If oSQLReader.Read Then

            'OK


            validity = oSQLReader.Item("validity")






            oSQLReader.Close()
            oCnct.Close()
        End If
        CheckValidation = validity
    End Function

End Class
