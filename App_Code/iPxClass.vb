Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data

Imports System.Drawing
Imports System.Configuration
Imports System.Net.Mail
Imports System.Net
Imports System.Security.Cryptography.X509Certificates
Imports System.Net.Security
Imports System.IO

Public Class iPxClass
    Public Shared Function Date2IsoDate(ByVal cDt As String) As String
        Dim cIsoDate As String
        If Trim(cDt) = "" Then
            cIsoDate = "1900/01/01"
        Else
            cIsoDate = Right(cDt, 4) & "/" & Mid(cDt, 4, 2) & "/" & Left(cDt, 2)

        End If
        Date2IsoDate = cIsoDate
    End Function

    Public Shared Function GetNewBusinessID() As String
        Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
        Dim oCnct As SqlConnection = New SqlConnection(sCnct)
        Dim oSQLCmd As SqlCommand
        Dim oSQLReader As SqlDataReader
        Dim sSQL As String

        Dim nInt As Int16
        Dim cKey As String
        Dim lNext As Boolean

        Dim aRet() As String = {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"}

        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If

LBL_Loop:
        lNext = True
        cKey = ""
        While lNext

            nInt = (Math.Ceiling(Rnd() * 35))
            If nInt >= 0 And nInt <= 35 Then
                cKey = cKey & aRet(nInt)
            End If
            If Len(cKey) = 6 Then
                lNext = False
            End If
        End While


        sSQL = "Select businessid FROM iPx_profile_client WHERE (businessid = '" & cKey & "')"
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        oSQLReader = oSQLCmd.ExecuteReader

        If oSQLReader.Read Then
            ' loop
            oSQLReader.Close()
            GoTo LBL_Loop
        End If
        oCnct.Close()

        GetNewBusinessID = cKey
    End Function
    Public Shared Function GetNewToken() As String
        Dim alphabets As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"

        Dim numbers As String = "1234567890"

        Dim characters As String = numbers

        characters += Convert.ToString(alphabets) & numbers

        Dim length As Integer = 6
        Dim otp As String = String.Empty
        For i As Integer = 0 To length - 1
            Dim character As String = String.Empty
            Do
                Dim index As Integer = New Random().Next(0, characters.Length)
                character = characters.ToCharArray()(index).ToString()
            Loop While otp.IndexOf(character) <> -1
            otp += character
        Next
        Dim voucherCode As String
        voucherCode = otp
        GetNewToken = otp


    End Function
    Public Shared Function GetCounterARG(ByVal sBusinessID As String, ByVal cModule As String) As String
        Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
        Dim oCnct As SqlConnection = New SqlConnection(sCnct)
        Dim oSQLCmd As SqlCommand
        Dim oSQLReader As SqlDataReader
        Dim sSQL As String
        Dim oTransaction As SqlTransaction
        Dim cCounter As String
        Dim iCounter As Integer

        Dim iPrefix1 As Integer
        Dim iPrefix2 As Integer
        Dim iPrefix3 As Integer
        Dim cPrefix As String

        Dim aRet() As String = {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"}

        ' Starting tahun 2017
        ' Hasil Bagi adalah 57 ( DIV ) - Setiap 35 Tahun Naik 1
        iPrefix1 = (Year(Now) \ 35) - 56
        iPrefix2 = Year(Now) Mod 35
        iPrefix3 = Month(Now)

        cPrefix = Trim(Str(iPrefix1)) & aRet(iPrefix2) & aRet(iPrefix3)

        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        sSQL = "select lastcounter from iPx_profile_counter WHERE businessid='" & sBusinessID & "' and module='" & cModule & "' and prefix='" & cPrefix & "' "
        oTransaction = oCnct.BeginTransaction("GetCounter")

        oSQLCmd = New SqlCommand(sSQL, oCnct)
        oSQLCmd.Transaction = oTransaction

        oSQLReader = oSQLCmd.ExecuteReader

        If oSQLReader.Read Then
            iCounter = Val(oSQLReader.Item("lastcounter")) + 1
            oSQLReader.Close()
            oSQLCmd.CommandText = "update iPx_profile_counter set lastcounter=lastcounter+1 WHERE businessid='" & sBusinessID & "' and module='" & cModule & "' and prefix='" & cPrefix & "' "
            oSQLCmd.ExecuteNonQuery()
        Else
            iCounter = 1
            oSQLReader.Close()
            oSQLCmd.CommandText = "insert into iPx_profile_counter (businessid, module, prefix, lastcounter) values('" & sBusinessID & "','" & cModule & "','" & cPrefix & "',1) "
            oSQLCmd.ExecuteNonQuery()
        End If
        oTransaction.Commit()
        cCounter = cModule & cPrefix & Right("00" & Trim(Str(iCounter)), 2)
        GetCounterARG = cCounter

    End Function
    Public Shared Function GetCounterMBR(ByVal sBusinessID As String, ByVal cModule As String) As String
        Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
        Dim oCnct As SqlConnection = New SqlConnection(sCnct)
        Dim oSQLCmd As SqlCommand
        Dim oSQLReader As SqlDataReader
        Dim sSQL As String
        Dim oTransaction As SqlTransaction
        Dim cCounter As String
        Dim iCounter As Integer

        Dim iPrefix1 As Integer
        Dim iPrefix2 As Integer
        Dim iPrefix3 As Integer
        Dim cPrefix As String

        Dim aRet() As String = {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"}

        ' Starting tahun 2017
        ' Hasil Bagi adalah 57 ( DIV ) - Setiap 35 Tahun Naik 1
        iPrefix1 = (Year(Now) \ 35) - 56
        iPrefix2 = Year(Now) Mod 35
        iPrefix3 = Month(Now)

        cPrefix = Trim(Str(iPrefix1)) & aRet(iPrefix2) & aRet(iPrefix3)

        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        sSQL = "select lastcounter from iPx_profile_counter WHERE businessid='" & sBusinessID & "' and module='" & cModule & "' and prefix='" & cPrefix & "' "
        oTransaction = oCnct.BeginTransaction("GetCounter")

        oSQLCmd = New SqlCommand(sSQL, oCnct)
        oSQLCmd.Transaction = oTransaction

        oSQLReader = oSQLCmd.ExecuteReader

        If oSQLReader.Read Then
            iCounter = Val(oSQLReader.Item("lastcounter")) + 1
            oSQLReader.Close()
            oSQLCmd.CommandText = "update iPx_profile_counter set lastcounter=lastcounter+1 WHERE businessid='" & sBusinessID & "' and module='" & cModule & "' and prefix='" & cPrefix & "' "
            oSQLCmd.ExecuteNonQuery()
        Else
            iCounter = 1
            oSQLReader.Close()
            oSQLCmd.CommandText = "insert into iPx_profile_counter (businessid, module, prefix, lastcounter) values('" & sBusinessID & "','" & cModule & "','" & cPrefix & "',1) "
            oSQLCmd.ExecuteNonQuery()
        End If
        oTransaction.Commit()
        cCounter = cModule & cPrefix & Right("00000" & Trim(Str(iCounter)), 5)
        GetCounterMBR = cCounter

    End Function

    Public Shared Function GetCounterCOA(ByVal sBusinessID As String, ByVal cModule As String) As String
        Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
        Dim oCnct As SqlConnection = New SqlConnection(sCnct)
        Dim oSQLCmd As SqlCommand
        Dim oSQLReader As SqlDataReader
        Dim sSQL As String
        Dim oTransaction As SqlTransaction
        Dim cCounter As String
        Dim iCounter As Integer

        Dim iPrefix1 As Integer
        Dim iPrefix2 As Integer
        Dim iPrefix3 As Integer
        Dim cPrefix As String

        Dim aRet() As String = {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"}

        ' Starting tahun 2017
        ' Hasil Bagi adalah 57 ( DIV ) - Setiap 35 Tahun Naik 1
        iPrefix1 = (Year(Now) \ 35) - 56
        iPrefix2 = Year(Now) Mod 35
        iPrefix3 = Month(Now)

        cPrefix = Trim(Str(iPrefix1)) & aRet(iPrefix2) & aRet(iPrefix3)

        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        sSQL = "select lastcounter from iPx_profile_counter WHERE businessid='" & sBusinessID & "' and module='" & cModule & "' and prefix='" & cPrefix & "' "
        oTransaction = oCnct.BeginTransaction("GetCounter")

        oSQLCmd = New SqlCommand(sSQL, oCnct)
        oSQLCmd.Transaction = oTransaction

        oSQLReader = oSQLCmd.ExecuteReader

        If oSQLReader.Read Then
            iCounter = Val(oSQLReader.Item("lastcounter")) + 1
            oSQLReader.Close()
            oSQLCmd.CommandText = "update iPx_profile_counter set lastcounter=lastcounter+1 WHERE businessid='" & sBusinessID & "' and module='" & cModule & "' and prefix='" & cPrefix & "' "
            oSQLCmd.ExecuteNonQuery()
        Else
            iCounter = 1
            oSQLReader.Close()
            oSQLCmd.CommandText = "insert into iPx_profile_counter (businessid, module, prefix, lastcounter) values('" & sBusinessID & "','" & cModule & "','" & cPrefix & "',1) "
            oSQLCmd.ExecuteNonQuery()
        End If
        oTransaction.Commit()
        cCounter = cModule & cPrefix & Right("000" & Trim(Str(iCounter)), 3)
        GetCounterCOA = cCounter

    End Function

    Public Shared Function GetCounterMGR(ByVal sBusinessID As String, ByVal cModule As String) As String
        Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
        Dim oCnct As SqlConnection = New SqlConnection(sCnct)
        Dim oSQLCmd As SqlCommand
        Dim oSQLReader As SqlDataReader
        Dim sSQL As String
        Dim oTransaction As SqlTransaction
        Dim cCounter As String
        Dim iCounter As Integer

        Dim iPrefix1 As Integer
        Dim iPrefix2 As Integer
        Dim iPrefix3 As Integer
        Dim cPrefix As String

        Dim aRet() As String = {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"}

        ' Starting tahun 2017
        ' Hasil Bagi adalah 57 ( DIV ) - Setiap 35 Tahun Naik 1
        iPrefix1 = (Year(Now) \ 35) - 56
        iPrefix2 = Year(Now) Mod 35
        iPrefix3 = Month(Now)

        cPrefix = Trim(Str(iPrefix1)) & aRet(iPrefix2) & aRet(iPrefix3)

        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        sSQL = "select lastcounter from iPxMGR_profile_counter WHERE businessid='" & sBusinessID & "' and module='" & cModule & "' and prefix='" & cPrefix & "' "
        oTransaction = oCnct.BeginTransaction("GetCounter")

        oSQLCmd = New SqlCommand(sSQL, oCnct)
        oSQLCmd.Transaction = oTransaction

        oSQLReader = oSQLCmd.ExecuteReader

        If oSQLReader.Read Then
            iCounter = Val(oSQLReader.Item("lastcounter")) + 1
            oSQLReader.Close()
            oSQLCmd.CommandText = "update iPxMGR_profile_counter set lastcounter=lastcounter+1 WHERE businessid='" & sBusinessID & "' and module='" & cModule & "' and prefix='" & cPrefix & "' "
            oSQLCmd.ExecuteNonQuery()
        Else
            iCounter = 1
            oSQLReader.Close()
            oSQLCmd.CommandText = "insert into iPxMGR_profile_counter (businessid, module, prefix, lastcounter) values('" & sBusinessID & "','" & cModule & "','" & cPrefix & "',1) "
            oSQLCmd.ExecuteNonQuery()
        End If
        oTransaction.Commit()
        cCounter = cModule & cPrefix & Right("00000" & Trim(Str(iCounter)), 5)
        GetCounterMGR = cCounter

    End Function

    Public Shared Function getCodeUnx() As String

        Dim alphabets As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"

        Dim numbers As String = "1234567890"

        Dim characters As String = numbers

        characters += Convert.ToString(alphabets) & numbers

        Dim length As Integer = 6
        Dim otp As String = String.Empty
        For i As Integer = 0 To length - 1
            Dim character As String = String.Empty
            Do
                Dim index As Integer = New Random().Next(0, characters.Length)
                character = characters.ToCharArray()(index).ToString()
            Loop While otp.IndexOf(character) <> -1
            otp += character
        Next
        Dim voucherCode As String
        voucherCode = otp
        getCodeUnx = otp


    End Function

    Public Shared Function GetUniqueNo() As String
        Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
        Dim oCnct As SqlConnection = New SqlConnection(sCnct)
        Dim oSQLCmd As SqlCommand
        Dim oSQLReader As SqlDataReader
        Dim sSQL As String

        Dim nInt As Int16
        Dim cKey As String
        Dim lNext As Boolean

        Dim aRet() As String = {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"}

        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If

LBL_Loop:
        lNext = True
        cKey = ""
        While lNext

            nInt = (Math.Ceiling(Rnd() * 35))
            If nInt >= 0 And nInt <= 35 Then
                cKey = cKey & aRet(nInt)
            End If
            If Len(cKey) = 6 Then
                lNext = False
            End If
        End While


        sSQL = "Select businessid FROM iPx_profile_client WHERE (businessid = '" & cKey & "')"
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        oSQLReader = oSQLCmd.ExecuteReader

        If oSQLReader.Read Then
            ' loop
            oSQLReader.Close()
            GoTo LBL_Loop
        End If
        oCnct.Close()

        GetUniqueNo = cKey
    End Function

    Sub setEmailList()
        Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
        Dim oCnct As SqlConnection = New SqlConnection(sCnct)

        Dim sSQL As String

        oCnct.Open()

        sSQL = "SELECT  * FROM  iPx_autoemail where isactive='Y' "

        Dim da As SqlDataAdapter = New SqlDataAdapter(sSQL, oCnct)
        Dim ds As DataSet = New DataSet()
        da.Fill(ds)



        For Each table As DataTable In ds.Tables

            For Each dr As DataRow In table.Rows

                Dim autoemailID As String = dr("id").ToString.Trim
                Dim businessid As String = dr("businessid").ToString.Trim
                Dim scheduleid As String = dr("scheduleid").ToString.Trim
                Dim isbirthday As String = dr("isbirthday").ToString.Trim
                Dim startdate As Date = dr("startdate").ToString.Trim
                Dim time As String = dr("time").ToString.Trim
                Dim emailtype As String = dr("emailtype").ToString.Trim


                If isbirthday = "Y" Then
                    'birthday real time
                    insertlistBdayEmail(businessid, Date.Today.Day, Date.Today.Month, autoemailID)
                    sendBroadcast()
                Else
                    Dim src = DateTime.Now
                    Dim hm = New DateTime(src.Year, src.Month, src.Day, src.Hour, src.Minute, 0)

                    Select Case scheduleid
                        Case "01" 'real time

                            'insertlistEmail(businessid, autoemailID)
                            insertlistEmailCheckout(businessid, autoemailID)
                            sendBroadcast()
                        Case "02" 'daily



                            If Date.Today & " " & time = hm Then
                                insertlistEmail(businessid, autoemailID)
                                sendBroadcast()
                            End If

                        Case "03" 'weekly

                            If dayname(startdate) = dayname(Date.Today) Then
                                If startdate & " " & time = hm Then
                                    insertlistEmail(businessid, autoemailID)
                                    sendBroadcast()
                                End If
                            End If


                        Case "04" 'monthly

                            If startdate.Day = Date.Today.Day Then
                                If startdate & " " & time = hm Then
                                    insertlistEmail(businessid, autoemailID)
                                    sendBroadcast()
                                End If
                            End If

                        Case "05" 'yearly
                            If startdate.Day & "-" & startdate.Month = Date.Today.Day & "-" & Date.Today.Month Then
                                If startdate & " " & time = hm Then
                                    insertlistEmail(businessid, autoemailID)
                                    sendBroadcast()
                                End If
                            End If
                    End Select

                End If



            Next
        Next
        oCnct.Close()
    End Sub

    Public Function dayname(ByVal daytoname As String) As String
        Dim myCulture As System.Globalization.CultureInfo = Globalization.CultureInfo.CurrentCulture
        Dim dayOfWeek As DayOfWeek = myCulture.Calendar.GetDayOfWeek(daytoname)
        dayname = myCulture.DateTimeFormat.GetDayName(dayOfWeek)
    End Function

    Public Shared Function insertlistBdayEmail(ByVal businessid As String, ByVal day As String, ByVal month As String, ByVal autoemailID As String) As String
        Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
        Dim oCnct As SqlConnection = New SqlConnection(sCnct)
        Dim oSQLCmd As SqlCommand

        Dim sSQL As String

        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If

        sSQL = "DELETE FROM iPx_autoemail_list where date < '" & Date.Today & "'"
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        oSQLCmd.ExecuteNonQuery()

        sSQL = "INSERT  INTO   iPx_autoemail_list(businessid,autoemailID, email,title,firstname,lastname,date, isactive)  ( select businessid, '" & autoemailID & "',email, title, firstname, lastname,'" & Date.Today & "','Y' from iPx_profile_guestgeneral where month(specialdate)='" & month & "' and day(specialdate)='" & day & "'  and businessid='" & businessid & "' and  email not in (select email from iPx_autoemail_list where date='" & Date.Today & "') and email <> '')"
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        insertlistBdayEmail = ""
    End Function

    Public Shared Function insertlistEmail(ByVal businessid As String, ByVal autoemailID As String) As String
        Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
        Dim oCnct As SqlConnection = New SqlConnection(sCnct)
        Dim oSQLCmd As SqlCommand

        Dim sSQL As String

        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If

        sSQL = "INSERT  INTO   iPx_autoemail_list(businessid,autoemailID, email,title,firstname,lastname,date, isactive)  (select businessid, '" & autoemailID & "',email, title, firstname, lastname,'" & Date.Today & "','Y' from iPx_profile_guestgeneral where  email <> '' and  email not in (select email from iPx_autoemail_list where date='" & Date.Today & "') )"
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        insertlistEmail = ""
    End Function

    Public Shared Function insertlistEmailCheckout(ByVal businessid As String, ByVal autoemailID As String) As String
        Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
        Dim oCnct As SqlConnection = New SqlConnection(sCnct)
        Dim oSQLCmd As SqlCommand

        Dim sSQL As String

        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If

        sSQL = "INSERT  INTO   iPx_autoemail_list(businessid,autoemailID, email,title,firstname,lastname,date, isactive)  (select businessid, '" & autoemailID & "',email, title, firstname, lastname,'" & Date.Today & "','Y' from iPx_profile_guestgeneral where  email <> '' and email not in (select email from iPx_autoemail_list where date='" & Date.Today & "')  and profileid=(select iPx_profile_guestgeneral.profileid from iPx_member_trans inner join iPx_Member on iPx_member_trans.MemberID=iPx_Member.MemberID inner join iPx_profile_guestgeneral on iPx_Member.profileid=iPx_profile_guestgeneral.profileid where iPx_member_trans.TransDate='" & Date.Today & "' and iPx_member_trans.DeptCode='ROOM')  )"
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        insertlistEmailCheckout = ""
    End Function

    Sub sendBroadcast()

        Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
        Dim oCnct As SqlConnection = New SqlConnection(sCnct)

        Dim sSQL As String

        oCnct.Open()

        sSQL = "SELECT   autoemailID, iPx_autoemail_list.businessid, email, title, firstname, lastname, date,iPx_emailtemplate.description as text,iPx_emailtemplate.type, isactive FROM  iPx_autoemail_list inner join iPx_emailtemplate on iPx_autoemail_list.autoemailID=iPx_emailtemplate.id where iPx_autoemail_list.isactive='Y'"


        Dim da As SqlDataAdapter = New SqlDataAdapter(sSQL, oCnct)
        Dim ds As DataSet = New DataSet()
        da.Fill(ds)



        For Each table As DataTable In ds.Tables

            For Each dr As DataRow In table.Rows

                Dim autoemailID As String = dr("autoemailID").ToString.Trim
                Dim businessid As String = dr("businessid").ToString.Trim
                Dim email As String = dr("email").ToString.Trim
                Dim title As String = dr("title").ToString.Trim
                Dim firstname As String = dr("firstname").ToString.Trim
                Dim lastname As String = dr("lastname").ToString.Trim
                Dim text As String = dr("text").ToString.Trim
                Dim type As String = dr("type").ToString.Trim
                SendEmail(businessid, email, firstname, lastname, text, autoemailID, title, type)
                updatelistEmail(businessid, email)
            Next
        Next
        oCnct.Close()
    End Sub

    Public Shared Function SendEmail(ByVal businessid As String, ByVal email As String, ByVal firstname As String, ByVal lastname As String, ByVal emailtext As String, ByVal templateid As String, ByVal title As String, ByVal type As String) As String

        Try


            Dim opening As String = "Dear " & title & "." & firstname & " " & lastname & "," & "<br/><br/>"
            Dim mm As New MailMessage(ConfigurationManager.AppSettings("UserName"), email)

            Dim body As String


            body = "<style>"

            body = body & "</style>"
            body = body & "<table width='100%' border='0' cellspacing='0' cellpadding='0' style='min-width: 320px;'>"
            body = body & "<tr><td align='center' bgcolor='#eff3f8'>"

            body = body & "<br />"
            body = body & "<table border='0' cellspacing='0' cellpadding='0' class='table_width_100' width='100%' style='max-width: 680px; min-width: 300px;    border-radius: 15px;"
            body = body & "background: #ffffff;"
            body = body & "padding: 10px;'>"


            body = body & "<tr><td align='center' bgcolor='#ffffff'>"


            body = body & "{imageLogo}"
            body = body & "<br /><br /><br />"
            body = body & "</td>"
            body = body & "</tr>"
            body = body & "<tr><td align='' bgcolor='#ffffff'>"
            body = body & "<div class='container'>"

            body = body & "{ipx_emailbody}"

            body = body & "</div>"
            body = body & "</td>"
            body = body & "</tr>"

            body = body & "<tr><td align='center' bgcolor='#ffffff'>"
            body = body & "<br /><br /><br />"


            body = body & "{imageFooter}"
            body = body & "</td>"
            body = body & "</tr>"
            body = body & "</table>"
            body = body & "<br />"

            body = body & "</td>"
            body = body & "</tr>"

            body = body & "</table>"



            '   body = System.Web.HttpContext.Current.Server.MapPath("~/iPxEmailThemplate/emailbroadcast.html")



            Dim urlheader, urlfooter, urlcontent, imgHDR, imgfooter, imgcontent As String
            'http://membership.alcorsys.com/iPxAdmin/Handler.ashx?ID=1|HHARIS|7
            'imgHDR = "http://membership.alcorsys.com/iPxAdmin/Handler.ashx?ID=1|HHARIS|7"

            imgHDR = "http://membership.alcorsys.com/iPxAdmin/Handler.ashx?ID=1|" & businessid & "|" & templateid & ""
            imgfooter = "http://membership.alcorsys.com/iPxAdmin/Handler.ashx?ID=3|" & businessid & "|" & templateid & ""
            imgcontent = "http://membership.alcorsys.com/iPxAdmin/Handler.ashx?ID=2|" & businessid & "|" & templateid & ""



            urlheader = "<img src=""" & imgHDR & """  width=""100%"" />"
            urlfooter = "<img src=""" & imgfooter & """ width=""100%"" />"
            urlcontent = "<img src=""" & imgcontent & """ width=""100%"" />"


            body = body.Replace("{imageLogo}", urlheader)
            body = body.Replace("{imageFooter}", urlfooter)

            Dim htmlstr As String
            htmlstr = emailtext.Replace(vbCrLf, "<br/>")

            If type = "T" Then
                body = body.Replace("{ipx_emailbody}", opening & htmlstr)

            Else
                body = body.Replace("{ipx_emailbody}", urlcontent)
            End If


            Dim fromEmail As String = ConfigurationManager.AppSettings("UserName")
            mm.[To].Add(email)
            mm.From = New MailAddress(fromEmail)
            mm.Subject = "ALCOR "

            mm.Body = body
            mm.IsBodyHtml = True
            Dim smtp As SmtpClient = New SmtpClient()
            smtp.Host = ConfigurationManager.AppSettings("Host")
            smtp.EnableSsl = False
            Dim NetworkCred As NetworkCredential = New NetworkCredential()
            NetworkCred.UserName = ConfigurationManager.AppSettings("UserName")
            NetworkCred.Password = ConfigurationManager.AppSettings("Password")
            smtp.UseDefaultCredentials = True
            smtp.Credentials = NetworkCred
            smtp.Port = Integer.Parse(ConfigurationManager.AppSettings("Port"))
            smtp.Send(mm)






            '=====================================================

        Catch ex As Exception

        End Try
        SendEmail = ""
    End Function

    Public Shared Function updatelistEmail(ByVal businessid As String, ByVal email As String) As String
        Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
        Dim oCnct As SqlConnection = New SqlConnection(sCnct)
        Dim oSQLCmd As SqlCommand

        Dim sSQL As String

        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If

        sSQL = "UPDATE   iPx_autoemail_list SET  datesent='" & DateTime.Now & "',    isactive ='N' where businessid='" & businessid & "' and email='" & email & "'"
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        updatelistEmail = ""
    End Function

    Public Shared Function getAccessUser(ByVal businessid As String, ByVal usercode As String, ByVal moduleid As String) As Boolean
        Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
        Dim oCnct As SqlConnection = New SqlConnection(sCnct)
        Dim oSQLCmd As SqlCommand
        Dim oSQLReader As SqlDataReader
        Dim sSQL As String
        oCnct.Open()

        sSQL = "SELECT  a.* FROM    iPxAcct_profile_client_useraccess as a "
        sSQL += "INNER JOIN iPxAcct_profile_client_useraccess_mdl as b ON b.id=a.moduleid "
        sSQL += "where businessid='" & businessid & "' and usercode='" & usercode & "' and b.description='" & moduleid & "' and a.active='Y' "
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        oSQLReader = oSQLCmd.ExecuteReader

        If oSQLReader.Read Then

            If oSQLReader.Item("active") = "Y" Then
                getAccessUser = True
            Else
                getAccessUser = False
            End If

        End If


        oCnct.Close()
    End Function

    Public Shared Function GetSMTP(ByVal businessid As String) As String
        Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
        Dim oCnct As SqlConnection = New SqlConnection(sCnct)
        Dim oSQLCmd As SqlCommand
        Dim oSQLReader As SqlDataReader
        Dim sSQL As String
        oCnct.Open()
        'Host, EnableSsl, UserName, Password, Port, active
        sSQL = "SELECT  * FROM    iPxAcct_SMTP where businessid='" & businessid & "'  "
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        oSQLReader = oSQLCmd.ExecuteReader

        If oSQLReader.Read Then
            GetSMTP = oSQLReader.Item("active") & "|" & oSQLReader.Item("Host") & "|" & oSQLReader.Item("EnableSsl") & "|" & oSQLReader.Item("UserName") & "|" & oSQLReader.Item("Password") & "|" & oSQLReader.Item("Port") & "|"
        Else
            GetSMTP = "N"

        End If

    End Function
    Public Function updateDefaultParam(ByVal businessid As String, ByVal paramid As String, ByVal paramvalue As String) As Boolean
        Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
        Dim oCnct As SqlConnection = New SqlConnection(sCnct)
        Dim oSQLCmd As SqlCommand

        Dim sSQL As String

        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If

        sSQL = "UPDATE iPx_general_default_parameterACCT SET   paramvalue ='" & paramvalue & "' where  parameterid ='" & paramid & "' and  businessid ='" & businessid & "' "
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()

    End Function

    Public Function savedataDefaultParam(ByVal businessid As String, ByVal paramid As String, ByVal description As String, ByVal paramvalue As String) As Boolean
        Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
        Dim oCnct As SqlConnection = New SqlConnection(sCnct)
        Dim oSQLCmd As SqlCommand

        Dim sSQL As String

        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If


        sSQL = "INSERT INTO iPx_general_default_parameterACCT(businessid, parameterid, description, paramvalue) VALUES ('" & businessid & "','" & paramid & "','" & description & "','" & paramvalue & "')"
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()

    End Function

    Public Function setDefaultParam(ByVal businessid As String, ByVal paramid As String, ByVal description As String, ByVal paramvalue As String) As Boolean
        Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
        Dim oCnct As SqlConnection = New SqlConnection(sCnct)
        Dim oSQLCmd As SqlCommand
        Dim oSQLReader As SqlDataReader

        Dim sSQL As String
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If


        sSQL = "SELECT * FROM iPx_general_default_parameterACCT WHERE businessid='" & businessid & "' and parameterid = '" & paramid & "'  "
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        If oSQLReader.Read Then
            oSQLReader.Close()
            updateDefaultParam(businessid, paramid, paramvalue)


        Else
            oSQLReader.Close()
            savedataDefaultParam(businessid, paramid, description, paramvalue)

        End If
        oCnct.Close()
    End Function

    Public Function getDefaultParameter(ByVal businessid As String, ByVal paramid As String) As String
        Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
        Dim oCnct As SqlConnection = New SqlConnection(sCnct)
        Dim oSQLCmd As SqlCommand
        Dim oSQLReader As SqlDataReader
        Dim sSQL As String

        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        getDefaultParameter = ""

        sSQL = "SELECT businessid, parameterid, description, paramvalue FROM iPx_general_default_parameterACCT WHERE parameterid = '" & paramid & "' and businessid='" & businessid & "'"
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        If oSQLReader.Read Then
            getDefaultParameter = oSQLReader.Item("paramvalue")
        End If
        oCnct.Close()
    End Function
    Public Function saveLog(ByVal businessid As String, ByVal transID As String, ByVal transDate As String, ByVal arGroup As String, ByVal funct As String, ByVal notesLog As String, ByVal foImport As String, ByVal userID As String) As Boolean
        Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
        Dim oCnct As SqlConnection = New SqlConnection(sCnct)
        Dim oSQLCmd As SqlCommand
        Dim sSQL As String
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "INSERT INTO iPxAcct_Log(businessid,transID,transDate,Grp,funct,notes,pmsID,userid,isActive)"
        sSQL += "VALUES ('" & businessid & "','" & transID & "','" & transDate & "','" & arGroup & "','" & funct & "', "
        sSQL += "'" & notesLog & "','" & foImport & "','" & userID & "','Y') "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Function
    Public Function getLogImport(ByVal businessid As String, ByVal transDate As String, ByVal Grp As String) As String
        Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
        Dim oCnct As SqlConnection = New SqlConnection(sCnct)
        Dim oSQLCmd As SqlCommand
        Dim oSQLReader As SqlDataReader
        Dim sSQL As String

        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        getLogImport = ""

        sSQL = "SELECT isActive FROM iPxAcct_Log WHERE businessid ='" & businessid & "' and transDate = '" & transDate & " 00:00:00' and Grp='" & Grp & "' order by id desc "
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        If oSQLReader.Read Then
            getLogImport = oSQLReader.Item("isActive")
        End If
        oCnct.Close()
    End Function
    Public Function updateLog(ByVal businessid As String, ByVal transID As String) As Boolean
        Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
        Dim oCnct As SqlConnection = New SqlConnection(sCnct)
        Dim oSQLCmd As SqlCommand
        Dim sSQL As String
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "UPDATE iPxAcct_Log SET isActive='N'"
        sSQL = sSQL & "WHERE businessid ='" & businessid & "' and transID='" & transID & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Function
End Class
