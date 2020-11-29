Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration
Partial Class iPxAdmin_iPxGLClosing
    Inherits System.Web.UI.Page
    Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
    Dim oSQLCmd As SqlCommand
    Dim oSQLReader As SqlDataReader
    Dim sSQL, forMonth, forYear, lastMonth, lastYear, i, a, x, y, z, RecID As String
    Dim cIpx As New iPxClass
    Sub UserAcces()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.businessid, a.usercode, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.funtionid='26'and x.active='Y' and x.businessid=a.businessid and x.usercode=a.usercode) as ClosingGL "
        sSQL += "from iPxAcct_profile_client_useraccess as a "
        sSQL += "INNER JOIN iPxAcct_profile_client_useraccess_dtl as b ON b.businessid=a.businessid and b.usercode=a.usercode "
        sSQL += "where a.businessid='" & Session("sBusinessID") & "' and a.usercode='" & Session("sUserCode") & "' and a.active='Y' group by a.businessid, a.usercode "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            If oSQLReader.Item("ClosingGL").ToString = "Y" Then
                lbClosing.Enabled = True
            Else
                lbClosing.Enabled = False
            End If
        Else
            lbClosing.Enabled = False
        End If
        oCnct.Close()
    End Sub
    Sub cekAccessRe()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.businessid, a.usercode, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.funtionid='10039'and x.active='Y' and x.businessid=a.businessid and x.usercode=a.usercode) as OpenClosing "
        sSQL += "from iPxAcct_profile_client_useraccess as a "
        sSQL += "INNER JOIN iPxAcct_profile_client_useraccess_dtl as b ON b.businessid=a.businessid and b.usercode=a.usercode "
        sSQL += "where a.businessid='" & Session("sBusinessID") & "' and a.usercode='" & Session("sUserCode") & "' and a.active='Y' group by a.businessid, a.usercode "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            If oSQLReader.Item("OpenClosing").ToString = "Y" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalReOpenLogin", "hideModalReOpenLogin()", True)
                cbAgree.Checked = False
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Sorry username has no access !');document.getElementById('Buttonx').click()", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalReOpenLogin", "hideModalReOpenLogin()", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalReOpenLogin", "showModalReOpenLogin()", True)
            End If
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Sorry username has no access !');document.getElementById('Buttonx').click()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalReOpenLogin", "hideModalReOpenLogin()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalReOpenLogin", "showModalReOpenLogin()", True)
        End If
        oCnct.Close()
    End Sub
    Sub totBalanceSheet()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select sum(a.Debit-a.Credit)as amount from iPxAcctGL_JVdtl as a "
        sSQL += "inner join iPxAcct_Coa as b ON a.businessid = b.businessid COLLATE SQL_Latin1_General_CP1_CI_AS AND a.Coa = b.Coa "
        sSQL += "INNER JOIN dbo.iPxAcctGL_JVhdr as c ON a.businessid = c.businessid AND a.TransID = c.TransID "
        sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' and MONTH(c.TransDate)='" & forMonth & "' "
        sSQL += "and year(c.TransDate)='" & forYear & "' and b.type ='Asset' and (a.IsActive='Y' AND c.Status<>'D') "
        sSQL += "group by b.type "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            lblAsset.Text = String.Format("{0:N2}", (oSQLReader.Item("amount"))).ToString
            x = oSQLReader.Item("amount").ToString
        Else
            lblAsset.Text = "0"
            x = "0"
        End If
        oSQLReader.Close()

        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select sum(a.Credit-a.Debit)as amount from iPxAcctGL_JVdtl as a "
        sSQL += "inner join iPxAcct_Coa as b ON a.businessid = b.businessid COLLATE SQL_Latin1_General_CP1_CI_AS AND a.Coa = b.Coa "
        sSQL += "INNER JOIN dbo.iPxAcctGL_JVhdr as c ON a.businessid = c.businessid AND a.TransID = c.TransID "
        sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' and MONTH(c.TransDate)='" & forMonth & "' "
        sSQL += "and year(c.TransDate)='" & forYear & "' and b.type ='Equity' and (a.IsActive='Y' AND c.Status<>'D') "
        sSQL += "group by b.type "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            lblEquity.Text = String.Format("{0:N2}", (oSQLReader.Item("amount"))).ToString
            y = oSQLReader.Item("amount").ToString
        Else
            lblEquity.Text = "0"
            y = "0"
        End If
        oSQLReader.Close()

        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select sum(a.Credit-a.Debit)as amount from iPxAcctGL_JVdtl as a "
        sSQL += "inner join iPxAcct_Coa as b ON a.businessid = b.businessid COLLATE SQL_Latin1_General_CP1_CI_AS AND a.Coa = b.Coa "
        sSQL += "INNER JOIN dbo.iPxAcctGL_JVhdr as c ON a.businessid = c.businessid AND a.TransID = c.TransID "
        sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' and MONTH(c.TransDate)='" & forMonth & "' "
        sSQL += "and year(c.TransDate)='" & forYear & "' and b.type ='Liability' and (a.IsActive='Y' AND c.Status<>'D') "
        sSQL += "group by b.type "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            lblLeability.Text = String.Format("{0:N2}", (oSQLReader.Item("amount"))).ToString
            z = oSQLReader.Item("amount").ToString
        Else
            lblLeability.Text = "0"
            z = "0"
        End If
        oCnct.Close()
        lblBalance.Text = String.Format("{0:N2}", Val(x) - (Val(y) + Val(z))).ToString
    End Sub
    Sub totProfit()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select sum(a.Credit-a.Debit)as amount from iPxAcctGL_JVdtl as a "
        sSQL += "inner join iPxAcct_Coa as b ON a.businessid = b.businessid COLLATE SQL_Latin1_General_CP1_CI_AS AND a.Coa = b.Coa "
        sSQL += "INNER JOIN dbo.iPxAcctGL_JVhdr as c ON a.businessid = c.businessid AND a.TransID = c.TransID "
        sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' and MONTH(c.TransDate)='" & forMonth & "' "
        sSQL += "and year(c.TransDate)='" & forYear & "' and b.type ='Revenue' and (a.IsActive='Y' AND c.Status<>'D') "
        sSQL += "group by b.type "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            lblRevenue.Text = String.Format("{0:N2}", (oSQLReader.Item("amount"))).ToString
            x = oSQLReader.Item("amount").ToString
        Else
            lblRevenue.Text = "0"
            x = "0"
        End If
        oSQLReader.Close()

        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select sum(a.Debit-a.Credit)as amount from iPxAcctGL_JVdtl as a "
        sSQL += "inner join iPxAcct_Coa as b ON a.businessid = b.businessid COLLATE SQL_Latin1_General_CP1_CI_AS AND a.Coa = b.Coa "
        sSQL += "INNER JOIN dbo.iPxAcctGL_JVhdr as c ON a.businessid = c.businessid AND a.TransID = c.TransID "
        sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' and MONTH(c.TransDate)='" & forMonth & "' "
        sSQL += "and year(c.TransDate)='" & forYear & "' and b.type ='Cost' and (a.IsActive='Y' AND c.Status<>'D') "
        sSQL += "group by b.type "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            lblCost.Text = String.Format("{0:N2}", (oSQLReader.Item("amount"))).ToString
            y = oSQLReader.Item("amount").ToString
        Else
            lblCost.Text = "0"
            y = "0"
        End If
        oSQLReader.Close()

        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select sum(a.Debit-a.Credit)as amount from iPxAcctGL_JVdtl as a "
        sSQL += "inner join iPxAcct_Coa as b ON a.businessid = b.businessid COLLATE SQL_Latin1_General_CP1_CI_AS AND a.Coa = b.Coa "
        sSQL += "INNER JOIN dbo.iPxAcctGL_JVhdr as c ON a.businessid = c.businessid AND a.TransID = c.TransID "
        sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' and MONTH(c.TransDate)='" & forMonth & "' "
        sSQL += "and year(c.TransDate)='" & forYear & "' and b.type ='Expenses' and (a.IsActive='Y' AND c.Status<>'D') "
        sSQL += "group by b.type "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            lblExpenses.Text = String.Format("{0:N2}", (oSQLReader.Item("amount"))).ToString
            z = oSQLReader.Item("amount").ToString
        Else
            lblExpenses.Text = "0"
            z = "0"
        End If
        oCnct.Close()
        lblProfit.Text = String.Format("{0:N2}", Val(x) - (Val(y) + Val(z))).ToString
    End Sub
    Sub totStatistic()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select sum(a.Debit-a.Credit)as amount from iPxAcctGL_JVdtl as a "
        sSQL += "inner join iPxAcct_Coa as b ON a.businessid = b.businessid COLLATE SQL_Latin1_General_CP1_CI_AS AND a.Coa = b.Coa "
        sSQL += "INNER JOIN dbo.iPxAcctGL_JVhdr as c ON a.businessid = c.businessid AND a.TransID = c.TransID "
        sSQL += "WHERE a.businessid='" & Session("sBusinessID") & "' and MONTH(c.TransDate)='" & forMonth & "' "
        sSQL += "and year(c.TransDate)='" & forYear & "' and b.type ='Statistic' and (a.IsActive='Y' AND c.Status<>'D') "
        sSQL += "group by b.type "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            lblStatistic.Text = String.Format("{0:N2}", (oSQLReader.Item("amount"))).ToString
            x = oSQLReader.Item("amount").ToString
        Else
            lblStatistic.Text = "0"
            x = "0"
        End If
        oCnct.Close()
    End Sub
    Sub cekClose()
        Dim oCnct4 As SqlConnection = New SqlConnection(sCnct)
        Dim oSQLReader4 As SqlDataReader
        Dim oSQLCmd4 As SqlCommand
        Dim sSQL4 As String
        If oCnct4.State = ConnectionState.Closed Then
            oCnct4.Open()
        End If
        oSQLCmd4 = New SqlCommand(sSQL4, oCnct4)
        sSQL4 = "SELECT * FROM iPxAcctGL_JVhdr where businessid='" & Session("sBusinessID") & "' and ReffNo = 'PL " & tbDate.Text & "' and status<>'D'"
        oSQLCmd4.CommandText = sSQL4
        oSQLReader4 = oSQLCmd4.ExecuteReader

        oSQLReader4.Read()
        If oSQLReader4.HasRows Then
            oCnct4.Close()
            UserAcces()
            lbClosing.Enabled = False
            lbOpen.Visible = True
        Else
            oCnct4.Close()
            UserAcces()
            lbClosing.Enabled = True
            lbOpen.Visible = False
        End If
    End Sub
    Sub idGLDtl()
        Dim oCnct4 As SqlConnection = New SqlConnection(sCnct)
        Dim oSQLReader4 As SqlDataReader
        Dim oSQLCmd4 As SqlCommand
        Dim sSQL4 As String
        If oCnct4.State = ConnectionState.Closed Then
            oCnct4.Open()
        End If
        oSQLCmd4 = New SqlCommand(sSQL4, oCnct4)
        sSQL4 = "SELECT MAX(RecID) as RecID FROM iPxAcctGL_JVdtl where TransID = '" & Session("sTransIDGL") & "'"
        oSQLCmd4.CommandText = sSQL4
        oSQLReader4 = oSQLCmd4.ExecuteReader

        oSQLReader4.Read()
        If oSQLReader4.HasRows Then
            RecID = Val(oSQLReader4.Item("RecID").ToString) + 1

        Else
            RecID = "1"
        End If
        oCnct4.Close()
    End Sub
    Public Function saveGLHeader(ByVal GLid As String, ByVal businessid As String, ByVal GLDate As String, ByVal Reff As String) As Boolean
        Dim oCnct1 As SqlConnection = New SqlConnection(sCnct)
        Dim oSQLCmd1 As SqlCommand
        Dim oSQLReader1 As SqlDataReader
        Dim sSQL1 As String
        If oCnct1.State = ConnectionState.Closed Then
            oCnct1.Open()
        End If
        oSQLCmd1 = New SqlCommand(sSQL1, oCnct1)
        Dim GLTransDate As String = Today
        sSQL1 = "INSERT INTO iPxAcctGL_JVhdr(businessid, TransID, TransDate, Status, JVgroup, ReffNo, Description) "
        sSQL1 += "VALUES ('" & businessid & "','" & GLid & "','" & GLDate & "', "
        sSQL1 += "'O','G3','PL " & Reff & "','Auto PL Journal') "
        oSQLCmd1.CommandText = sSQL1
        oSQLCmd1.ExecuteNonQuery()

        oCnct1.Close()
    End Function
    Public Function saveGLDetailDebit(ByVal businessid As String, ByVal TransID As String, ByVal Coa As String, ByVal Reff As String, ByVal Amount As String) As Boolean
        Dim oCnct3 As SqlConnection = New SqlConnection(sCnct)
        Dim oSQLCmd3 As SqlCommand
        Dim sSQL3 As String
        Session("sTransIDGL") = TransID
        idGLDtl()
        If oCnct3.State = ConnectionState.Closed Then
            oCnct3.Open()
        End If
        oSQLCmd3 = New SqlCommand(sSQL3, oCnct3)
        sSQL3 = "INSERT INTO iPxAcctGL_JVdtl(RecID, businessid, TransID, Coa, Description, Reff, Debit, Credit, isActive) "
        sSQL3 += "VALUES ('" & RecID & "','" & businessid & "','" & TransID & "','" & Coa & "', "
        sSQL3 += "'Auto PL Journal','PL " & Reff & "','" & Amount & "','0', "
        sSQL3 += "'Y') "
        oSQLCmd3.CommandText = sSQL3
        oSQLCmd3.ExecuteNonQuery()

        oCnct3.Close()
    End Function
    Public Function saveGLDetailCredit(ByVal businessid As String, ByVal TransID As String, ByVal Coa As String, ByVal Reff As String, ByVal Amount As String) As Boolean
        Dim oCnct3 As SqlConnection = New SqlConnection(sCnct)
        Dim oSQLCmd3 As SqlCommand
        Dim sSQL3 As String
        Session("sTransIDGL") = TransID
        idGLDtl()
        If oCnct3.State = ConnectionState.Closed Then
            oCnct3.Open()
        End If
        oSQLCmd3 = New SqlCommand(sSQL3, oCnct3)
        sSQL3 = "INSERT INTO iPxAcctGL_JVdtl(RecID, businessid, TransID, Coa, Description, Reff, Debit, Credit, isActive) "
        sSQL3 += "VALUES ('" & RecID & "','" & businessid & "','" & TransID & "','" & Coa & "', "
        sSQL3 += "'Auto PL Journal','PL " & Reff & "','0','" & Amount & "', "
        sSQL3 += "'Y') "
        oSQLCmd3.CommandText = sSQL3
        oSQLCmd3.ExecuteNonQuery()

        oCnct3.Close()
    End Function
    Protected Sub cari(ByVal sender As Object, ByVal e As EventArgs)
        Dim dateBirthday As Date = tbDate.Text
        forMonth = dateBirthday.ToString("MM")
        forYear = dateBirthday.ToString("yyy")
        If forMonth = "01" Then
            lastMonth = "12"
            lastYear = forYear - 1
        Else
            lastMonth = forMonth - 1
            lastYear = forYear
        End If
        totBalanceSheet()
        totProfit()
        totStatistic()
    End Sub
    Sub CekIdGLClose()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select TransID from iPxAcctGL_JVhdr "
        sSQL = sSQL & "WHERE businessid ='" & Session("sBusinessID") & "' and ReffNo ='PL " & tbDate.Text & "' and Status<>'D'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            i = oSQLReader.Item("TransID").ToString
        Else
        End If
        oCnct.Close()
    End Sub
    Sub Delete()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)

        sSQL = "UPDATE iPxAcctGL_JVhdr SET  Status='D' "
        sSQL = sSQL & "WHERE businessid ='" & Session("sBusinessID") & "' and ReffNo ='PL " & tbDate.Text & "' and Status<>'D'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("sBusinessID") = "" Then
            Response.Redirect("signin.aspx")
        End If
        tbDate.Enabled = False
        If Not Me.IsPostBack Then

            If cIpx.getAccessUser(Session("sBusinessID"), Session("sUserCode"), "GL Closing") <> True Then

                Session("sMessage") = "Sorry, you dont have access in this module |"
                Session("sMemberid") = ""
                Session("sWarningID") = "0"
                Session("sUrlOKONLY") = "home.aspx"
                Session("sUrlYES") = "http://www.thepyxis.net"
                Session("sUrlNO") = "http://www.thepyxis.net"
                Response.Redirect("warningmsg.aspx")
            End If
            tbDate.Text = Format(Now, "MM-yyyy")
            Dim dateBirthday As Date = tbDate.Text
            forMonth = dateBirthday.ToString("MM")
            forYear = dateBirthday.ToString("yyy")
            If forMonth = "01" Then
                lastMonth = "12"
                lastYear = forYear - 1
            Else
                lastMonth = forMonth - 1
                lastYear = forYear
            End If
            totBalanceSheet()
            totProfit()
            totStatistic()
        End If
        cekClose()
        'UserAcces()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MonthGL", "$(document).ready(function() {MonthGL()});", True)
    End Sub

    Protected Sub lbClosing_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbClosing.Click
        If lblAsset.Text = "0" And lblEquity.Text = "0" And lblLeability.Text = "0" And lblRevenue.Text = "0" And lblCost.Text = "0" And lblExpenses.Text = "0" And lblStatistic.Text = "0" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('closing failed, Data is not found !!');", True)
        ElseIf lblBalance.Text <> lblProfit.Text Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('closing failed, Data is not balanced !!');", True)
        Else
            Dim GLCoaCredit As String = cIpx.getDefaultParameter(Session("sBusinessID"), "11")
            Dim GLCoaDebit As String = cIpx.getDefaultParameter(Session("sBusinessID"), "12")
            If GLCoaCredit = "" Or GLCoaDebit = "" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('closing failed, please setting Coa PL reversal account !!');", True)
            Else
                Dim GLid As String = cIpx.GetCounterMBR("GL", "GL")
                Dim Transdate As Date
                'Transdate = Date.ParseExact(tbDate.Text, "MM-yyyy", System.Globalization.CultureInfo.InvariantCulture)
                Transdate = DateSerial(Year(tbDate.Text), Month(tbDate.Text) + 1, 0)
                saveGLHeader(GLid, Session("sBusinessID"), Transdate, tbDate.Text)
                saveGLDetailDebit(Session("sBusinessID"), GLid, GLCoaDebit, tbDate.Text, lblProfit.Text)
                saveGLDetailCredit(Session("sBusinessID"), GLid, GLCoaCredit, tbDate.Text, lblProfit.Text)
                Dim dateBirthday As Date = tbDate.Text
                forMonth = dateBirthday.ToString("MM")
                Dim forMonthh As String = dateBirthday.ToString("MMMM")
                forYear = dateBirthday.ToString("yyy")
                If forMonth = "01" Then
                    lastMonth = "12"
                    lastYear = forYear - 1
                Else
                    lastMonth = forMonth - 1
                    lastYear = forYear
                End If
                totBalanceSheet()
                totProfit()
                totStatistic()
                cekClose()
                Dim TransDateLog As String = Format(Now, "yyy-MM-dd hh:mm:ss")
                cIpx.saveLog(Session("sBusinessID"), GLid, TransDateLog, "GL", "Closing", "Closing general ledger of periode " & forMonthh & " " & forYear, "", Session("sUserCode"))
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('Closing success !!');", True)
            End If
        End If
    End Sub

    Protected Sub lbOpen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbOpen.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalReOpenLogin", "showModalReOpenLogin()", True)
    End Sub

    Protected Sub lbAbort_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbort.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
    End Sub

    Protected Sub lbUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbUpdate.Click
        If cbAgree.Checked = False Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('Re-Open failed, please choose Agree to continue !!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
        ElseIf cbAgree.Checked = True Then
            CekIdGLClose()
            Delete()
            Dim dateBirthday As Date = tbDate.Text
            Dim forMonthh As String = dateBirthday.ToString("MMMM")
            forYear = dateBirthday.ToString("yyy")
            Dim TransDateLog As String = Format(Now, "yyy-MM-dd hh:mm:ss")
            cIpx.saveLog(Session("sBusinessID"), i, TransDateLog, "GL", "Closing", "Re-Open general ledger of periode " & forMonthh & " " & forYear, "", Session("sUserCode"))
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('Re-Open success, this periode is Open !!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
            cekClose()
            'tbDate.Text = Format(Now, "MM-yyyy")
            dateBirthday = tbDate.Text
            forMonth = dateBirthday.ToString("MM")
            forYear = dateBirthday.ToString("yyy")
            If forMonth = "01" Then
                lastMonth = "12"
                lastYear = forYear - 1
            Else
                lastMonth = forMonth - 1
                lastYear = forYear
            End If
            Call totBalanceSheet()
            Call totProfit()
            Call totStatistic()
        End If
    End Sub

    Protected Sub lbAbortLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortLogin.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalReOpenLogin", "hideModalReOpenLogin()", True)
    End Sub

    Protected Sub lbLogAcces_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbLogAcces.Click
        oCnct.Open()
        ' businessid, usergroup, userid, username, password, name, neverexpired, registereddate, expiredafter, islocked, userimage


        sSQL = "Select * From iPx_profile_client_userid Where businessid = '" & Session("sBusinessID") & "' AND usercode='" & tbUsernameRe.Text & "' "
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        oSQLReader = oSQLCmd.ExecuteReader

        If oSQLReader.Read Then
            If (Trim(oSQLReader.Item("usercode")) = Trim(tbUsernameRe.Text)) Then 'usercode
                If (Trim(oSQLReader.Item("password")) = Trim(tbPasswordRe.Text)) Then 'password

                    If oSQLReader.Item("islocked") = "Y" Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Sorry Username Is Locked !');document.getElementById('Buttonx').click()", True)
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalReOpenLogin", "hideModalReOpenLogin()", True)
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalReOpenLogin", "showModalReOpenLogin()", True)
                    Else
                        oCnct.Close()
                        cekAccessRe()
                    End If
                Else
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Sorry Your Password Is Wrong !');document.getElementById('Buttonx').click()", True)
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalReOpenLogin", "hideModalReOpenLogin()", True)
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalReOpenLogin", "showModalReOpenLogin()", True)
                End If
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Sorry Username Not Registered !');document.getElementById('Buttonx').click()", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalReOpenLogin", "hideModalReOpenLogin()", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalReOpenLogin", "showModalReOpenLogin()", True)
            End If
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Sorry Your Property Is Wrong !');document.getElementById('Buttonx').click()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalReOpenLogin", "hideModalReOpenLogin()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalReOpenLogin", "showModalReOpenLogin()", True)
        End If
        oCnct.Close()
    End Sub
End Class
