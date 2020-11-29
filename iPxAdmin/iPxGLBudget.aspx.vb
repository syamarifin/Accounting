Imports System.IO
Imports System.Data
Imports System.Drawing
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Data.OleDb

Partial Class iPxAdmin_iPxGLBudget
    Inherits System.Web.UI.Page
    Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
    Dim oSQLCmd As SqlCommand
    Dim oSQLReader As SqlDataReader
    Dim sSQL, tipe, d_c, grpLvl, status, active As String
    Dim cIpx As New iPxClass
    Sub SaveMassUpluad()
        Dim excelPath As String = Server.MapPath("~/UploadFile/") + Path.GetFileName(flMassUpoad.PostedFile.FileName)
        flMassUpoad.SaveAs(excelPath)

        Dim connString As String = String.Empty
        Dim extension As String = Path.GetExtension(flMassUpoad.PostedFile.FileName)
        'Select Case extension
        '    Case ".xls"
        '        'Excel 97-03
        '        'connString = ConfigurationManager.ConnectionStrings("Excel03ConString").ConnectionString
        '        connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & excelPath & ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=2'"
        '        Exit Select
        '    Case ".xlsx"
        '        'Excel 07 or higher
        '        'connString = ConfigurationManager.ConnectionStrings("Excel07+ConString").ConnectionString
        '        connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & excelPath & ";Extended Properties='Excel 12.0;HDR=Yes;IMEX=2'"
        '        Exit Select
        'End Select
        If extension = ".xlsx" Then
            'connString = String.Format(connString, excelPath)
            connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & excelPath & ";Extended Properties='Excel 12.0;HDR=Yes;IMEX=2'"
            Dim oledbConn As OleDbConnection = New OleDbConnection(connString)
            Try
                oledbConn.Open()
                Dim cmd As OleDbCommand = New OleDbCommand("SELECT * FROM [Sheet1$]", oledbConn)
                Dim oleda As OleDbDataAdapter = New OleDbDataAdapter()
                oleda.SelectCommand = cmd
                Dim ds As DataSet = New DataSet()
                oleda.Fill(ds, "inventory")
                For Each Drr As DataRow In ds.Tables(0).Rows
                    If oCnct.State = ConnectionState.Closed Then
                        oCnct.Open()
                    End If
                    oSQLCmd = New SqlCommand(sSQL, oCnct)
                    If Drr(1).ToString().Trim() <> "" Then
                        Dim a As String = Drr(3).ToString()

                        sSQL = "SELECT Coa FROM iPxAcctGL_COABudget WHERE businessid='" & Session("sBusinessID") & "' and Coa='" & Drr(0).ToString() & "' and periode ='" & Drr(2).ToString() & "'"
                        oSQLCmd.CommandText = sSQL
                        oSQLReader = oSQLCmd.ExecuteReader

                        If oSQLReader.Read Then
                            oSQLReader.Close()
                            UpdateBudgetImport(Session("sBusinessID"), Drr(0).ToString(), Drr(2).ToString(), Drr(3).ToString(), Drr(4).ToString(), Drr(5).ToString(), Drr(6).ToString(), Drr(7).ToString(), Drr(8).ToString(), Drr(9).ToString(), Drr(10).ToString(), Drr(11).ToString(), Drr(12).ToString(), Drr(13).ToString(), Drr(14).ToString())
                        Else
                            oSQLReader.Close()
                            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('GL Budget masih kosong !!');", True)
                            saveBudgetImport(Session("sBusinessID"), Drr(0).ToString(), Drr(2).ToString(), Drr(3).ToString(), Drr(4).ToString(), Drr(5).ToString(), Drr(6).ToString(), Drr(7).ToString(), Drr(8).ToString(), Drr(9).ToString(), Drr(10).ToString(), Drr(11).ToString(), Drr(12).ToString(), Drr(13).ToString(), Drr(14).ToString())
                        End If
                        oCnct.Close()
                    End If
                Next
            Catch
            Finally
                oledbConn.Close()
                System.IO.File.Delete(excelPath)
            End Try
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('import successfull !!');", True)
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('import failed, file extension must be .xslx !!');", True)
        End If
        ListCOA()
        ListCOALia()
        ListCOACost()
        ListCOAEquity()
        ListCOARev()
        ListCOAStatistic()
    End Sub
    Public Function saveBudgetImport(ByVal businessid As String, ByVal COA As String, ByVal periode As String, ByVal bln1 As String, ByVal bln2 As String, ByVal bln3 As String, ByVal bln4 As String, ByVal bln5 As String, ByVal bln6 As String, ByVal bln7 As String, ByVal bln8 As String, ByVal bln9 As String, ByVal bln10 As String, ByVal bln11 As String, ByVal bln12 As String) As Boolean
        Dim sCnct1 As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
        Dim oCnct1 As SqlConnection = New SqlConnection(sCnct1)
        Dim oSQLCmd1 As SqlCommand
        'Dim oSQLReader1 As SqlDataReader
        If oCnct1.State = ConnectionState.Closed Then
            oCnct1.Open()
        End If
        oSQLCmd1 = New SqlCommand(sSQL, oCnct1)
        sSQL = "INSERT INTO iPxAcctGL_COABudget(businessid,Coa,periode,bln1,bln2,bln3,bln4,bln5,bln6,bln7,bln8,bln9,bln10,bln11,bln12) "
        sSQL = sSQL & "VALUES ('" & businessid & "','" & COA & "','" & periode & "','" & bln1 & "','" & bln2 & "','" & bln3 & "','" & bln4 & "','" & bln5 & "','" & bln6 & "'"
        sSQL = sSQL & ",'" & bln7 & "','" & bln8 & "','" & bln9 & "','" & bln10 & "','" & bln11 & "','" & bln12 & "') "
        oSQLCmd1.CommandText = sSQL
        oSQLCmd1.ExecuteNonQuery()
        oCnct1.Close()
    End Function
    Public Function UpdateBudgetImport(ByVal businessid As String, ByVal COA As String, ByVal periode As String, ByVal bln1 As String, ByVal bln2 As String, ByVal bln3 As String, ByVal bln4 As String, ByVal bln5 As String, ByVal bln6 As String, ByVal bln7 As String, ByVal bln8 As String, ByVal bln9 As String, ByVal bln10 As String, ByVal bln11 As String, ByVal bln12 As String) As Boolean
        Dim sCnct1 As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
        Dim oCnct1 As SqlConnection = New SqlConnection(sCnct1)
        Dim oSQLCmd1 As SqlCommand
        'Dim oSQLReader1 As SqlDataReader
        If oCnct1.State = ConnectionState.Closed Then
            oCnct1.Open()
        End If
        oSQLCmd1 = New SqlCommand(sSQL, oCnct1)
        sSQL = "UPDATE iPxAcctGL_COABudget SET bln1='" & bln1 & "',bln2='" & bln2 & "', "
        sSQL += "bln3='" & bln3 & "',bln4='" & bln4 & "',bln5='" & bln5 & "', "
        sSQL += "bln6='" & bln6 & "',bln7='" & bln7 & "',bln8='" & bln8 & "', "
        sSQL += "bln9='" & bln9 & "',bln10='" & bln10 & "',bln11='" & bln11 & "', bln12='" & bln12 & "' "
        sSQL += "WHERE businessid ='" & businessid & "' and Coa='" & COA & "' and periode='" & periode & "' "
        oSQLCmd1.CommandText = sSQL
        oSQLCmd1.ExecuteNonQuery()
        oCnct1.Close()
    End Function
    Sub UserAcces()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.businessid, a.usercode, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.funtionid='27'and x.active='Y' and x.businessid=a.businessid and x.usercode=a.usercode) as AddGLConf "
        sSQL += "from iPxAcct_profile_client_useraccess as a "
        sSQL += "INNER JOIN iPxAcct_profile_client_useraccess_dtl as b ON b.businessid=a.businessid and b.usercode=a.usercode "
        sSQL += "where a.businessid='" & Session("sBusinessID") & "' and a.usercode='" & Session("sUserCode") & "' and a.active='Y' group by a.businessid, a.usercode "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            If oSQLReader.Item("AddGLConf").ToString = "Y" Then
                lbAddCoa.Enabled = True
            Else
                lbAddCoa.Enabled = False
            End If
        Else
            lbAddCoa.Enabled = False
        End If
        oCnct.Close()
    End Sub
    Sub ListCOA()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.*, b.description, b.grpLevel from iPxAcctGL_COABudget as a "
        sSQL += "INNER JOIN iPxAcct_Coa as b ON b.businessid = a.businessid and b.Coa = a.Coa "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and b.type='Asset' and a.periode='" & tbDate.Text & "' and b.isactive = 'Y'"
        sSQL += " order by a.Coa asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvCoaAsset.DataSource = dt
                    gvCoaAsset.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvCoaAsset.DataSource = dt
                    gvCoaAsset.DataBind()
                    gvCoaAsset.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub ListCOALia()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.*, b.description, b.grpLevel from iPxAcctGL_COABudget as a "
        sSQL += "INNER JOIN iPxAcct_Coa as b ON b.businessid = a.businessid and b.Coa = a.Coa "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and b.type='Liability' and a.periode='" & tbDate.Text & "' and b.isactive = 'Y'"
        sSQL += " order by a.Coa asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvLia.DataSource = dt
                    gvLia.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvLia.DataSource = dt
                    gvLia.DataBind()
                    gvLia.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub ListCOAEquity()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.*, b.description, b.grpLevel from iPxAcctGL_COABudget as a "
        sSQL += "INNER JOIN iPxAcct_Coa as b ON b.businessid = a.businessid and b.Coa = a.Coa "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and b.type='Equity' and a.periode='" & tbDate.Text & "' "
        sSQL += " and b.isactive = 'Y' order by a.Coa asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvEquity.DataSource = dt
                    gvEquity.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvEquity.DataSource = dt
                    gvEquity.DataBind()
                    gvEquity.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub ListCOARev()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.*, b.description, b.grpLevel from iPxAcctGL_COABudget as a "
        sSQL += "INNER JOIN iPxAcct_Coa as b ON b.businessid = a.businessid and b.Coa = a.Coa "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and b.type='Revenue' and a.periode='" & tbDate.Text & "' "
        sSQL += " and b.isactive = 'Y' order by a.Coa asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvRev.DataSource = dt
                    gvRev.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvRev.DataSource = dt
                    gvRev.DataBind()
                    gvRev.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub ListCOACost()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.*, b.description, b.grpLevel from iPxAcctGL_COABudget as a "
        sSQL += "INNER JOIN iPxAcct_Coa as b ON b.businessid = a.businessid and b.Coa = a.Coa "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and (b.type='Cost' or b.type='Expenses') and a.periode='" & tbDate.Text & "' "
        sSQL += " and b.isactive = 'Y' order by a.Coa asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvCost.DataSource = dt
                    gvCost.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvCost.DataSource = dt
                    gvCost.DataBind()
                    gvCost.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub ListCOAStatistic()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.*, b.description, b.grpLevel from iPxAcctGL_COABudget as a "
        sSQL += "INNER JOIN iPxAcct_Coa as b ON b.businessid = a.businessid and b.Coa = a.Coa "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and b.type='Statistic' and a.periode='" & tbDate.Text & "' "
        sSQL += " and b.isactive = 'Y' order by a.Coa asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvStatistic.DataSource = dt
                    gvStatistic.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvStatistic.DataSource = dt
                    gvStatistic.DataBind()
                    gvStatistic.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub cekBudget()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT Coa FROM iPxAcctGL_COABudget WHERE businessid='" & Session("sBusinessID") & "' and periode ='" & tbDate.Text & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        If oSQLReader.Read Then
            oSQLReader.Close()
            ListCOA()
            ListCOALia()
            ListCOACost()
            ListCOAEquity()
            ListCOARev()
            ListCOAStatistic()
        Else
            oSQLReader.Close()
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('GL Budget masih kosong !!');", True)
            saveGrpBudget(Session("sBusinessID"), tbDate.Text)
            ListCOA()
            ListCOALia()
            ListCOACost()
            ListCOAEquity()
            ListCOARev()
            ListCOAStatistic()
        End If
    End Sub
    Public Function saveGrpBudget(ByVal businessid As String, ByVal periode As String) As Boolean

        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT Coa FROM iPxAcct_Coa where businessid ='" & businessid & "' "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader
        While oSQLReader.Read
            'oSQLReader.Close()
            saveBudget(businessid, oSQLReader.Item("Coa"), periode)
        End While
        oCnct.Close()

    End Function
    Public Function saveBudget(ByVal businessid As String, ByVal COA As String, ByVal periode As String) As Boolean
        Dim sCnct1 As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
        Dim oCnct1 As SqlConnection = New SqlConnection(sCnct1)
        Dim oSQLCmd1 As SqlCommand
        'Dim oSQLReader1 As SqlDataReader
        If oCnct1.State = ConnectionState.Closed Then
            oCnct1.Open()
        End If
        oSQLCmd1 = New SqlCommand(sSQL, oCnct1)
        sSQL = "INSERT INTO iPxAcctGL_COABudget(businessid,Coa,periode,bln1,bln2,bln3,bln4,bln5,bln6,bln7,bln8,bln9,bln10,bln11,bln12) "
        sSQL = sSQL & "VALUES ('" & businessid & "','" & COA & "','" & periode & "','0','0','0','0','0','0'"
        sSQL = sSQL & ",'0','0','0','0','0','0') "
        oSQLCmd1.CommandText = sSQL
        oSQLCmd1.ExecuteNonQuery()
        oCnct1.Close()
    End Function
    Public Function saveGridRev(ByVal businessid As String, ByVal periode As String) As Boolean
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        For Each row As GridViewRow In gvRev.Rows
            Dim hdCoa As HiddenField = row.FindControl("hdCoa")
            Dim tbbln1 As TextBox = row.FindControl("txtBln1")
            Dim tbbln2 As TextBox = row.FindControl("txtBln2")
            Dim tbbln3 As TextBox = row.FindControl("txtBln3")
            Dim tbbln4 As TextBox = row.FindControl("txtBln4")
            Dim tbbln5 As TextBox = row.FindControl("txtBln5")
            Dim tbbln6 As TextBox = row.FindControl("txtBln6")
            Dim tbbln7 As TextBox = row.FindControl("txtBln7")
            Dim tbbln8 As TextBox = row.FindControl("txtBln8")
            Dim tbbln9 As TextBox = row.FindControl("txtBln9")
            Dim tbbln10 As TextBox = row.FindControl("txtBln10")
            Dim tbbln11 As TextBox = row.FindControl("txtBln11")
            Dim tbbln12 As TextBox = row.FindControl("txtBln12")

            oSQLCmd = New SqlCommand(sSQL, oCnct)
            sSQL = "UPDATE iPxAcctGL_COABudget  SET bln1 ='" & tbbln1.Text & "', bln2 ='" & tbbln2.Text & "', bln3 ='" & tbbln3.Text & "', bln4 ='" & tbbln4.Text & "', bln5 ='" & tbbln5.Text & "', bln6 ='" & tbbln6.Text & "', "
            sSQL += "bln7 ='" & tbbln7.Text & "', bln8 ='" & tbbln8.Text & "', bln9 ='" & tbbln9.Text & "', bln10 ='" & tbbln10.Text & "', bln11 ='" & tbbln11.Text & "', bln12 ='" & tbbln12.Text & "' "
            sSQL += "where businessid ='" & businessid & "' and Coa ='" & hdCoa.Value.Trim & "' and periode ='" & periode & "' "
            oSQLCmd.CommandText = sSQL
            oSQLCmd.ExecuteNonQuery()
        Next
        oCnct.Close()
        ListCOARev()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "RevenueActive", "RevenueActive();", True)
    End Function
    Public Function saveGridCost(ByVal businessid As String, ByVal periode As String) As Boolean
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        For Each row As GridViewRow In gvCost.Rows
            Dim hdCoa As HiddenField = row.FindControl("hdCoa")
            Dim tbbln1 As TextBox = row.FindControl("txtBln1")
            Dim tbbln2 As TextBox = row.FindControl("txtBln2")
            Dim tbbln3 As TextBox = row.FindControl("txtBln3")
            Dim tbbln4 As TextBox = row.FindControl("txtBln4")
            Dim tbbln5 As TextBox = row.FindControl("txtBln5")
            Dim tbbln6 As TextBox = row.FindControl("txtBln6")
            Dim tbbln7 As TextBox = row.FindControl("txtBln7")
            Dim tbbln8 As TextBox = row.FindControl("txtBln8")
            Dim tbbln9 As TextBox = row.FindControl("txtBln9")
            Dim tbbln10 As TextBox = row.FindControl("txtBln10")
            Dim tbbln11 As TextBox = row.FindControl("txtBln11")
            Dim tbbln12 As TextBox = row.FindControl("txtBln12")

            oSQLCmd = New SqlCommand(sSQL, oCnct)
            sSQL = "UPDATE iPxAcctGL_COABudget  SET bln1 ='" & tbbln1.Text & "', bln2 ='" & tbbln2.Text & "', bln3 ='" & tbbln3.Text & "', bln4 ='" & tbbln4.Text & "', bln5 ='" & tbbln5.Text & "', bln6 ='" & tbbln6.Text & "', "
            sSQL += "bln7 ='" & tbbln7.Text & "', bln8 ='" & tbbln8.Text & "', bln9 ='" & tbbln9.Text & "', bln10 ='" & tbbln10.Text & "', bln11 ='" & tbbln11.Text & "', bln12 ='" & tbbln12.Text & "' "
            sSQL += "where businessid ='" & businessid & "' and Coa ='" & hdCoa.Value.Trim & "' and periode ='" & periode & "' "
            oSQLCmd.CommandText = sSQL
            oSQLCmd.ExecuteNonQuery()
        Next
        oCnct.Close()
        ListCOACost()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CostActive", "CostActive();", True)
    End Function
    Public Function saveGridStatis(ByVal businessid As String, ByVal periode As String) As Boolean
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        For Each row As GridViewRow In gvStatistic.Rows
            Dim hdCoa As HiddenField = row.FindControl("hdCoa")
            Dim tbbln1 As TextBox = row.FindControl("txtBln1")
            Dim tbbln2 As TextBox = row.FindControl("txtBln2")
            Dim tbbln3 As TextBox = row.FindControl("txtBln3")
            Dim tbbln4 As TextBox = row.FindControl("txtBln4")
            Dim tbbln5 As TextBox = row.FindControl("txtBln5")
            Dim tbbln6 As TextBox = row.FindControl("txtBln6")
            Dim tbbln7 As TextBox = row.FindControl("txtBln7")
            Dim tbbln8 As TextBox = row.FindControl("txtBln8")
            Dim tbbln9 As TextBox = row.FindControl("txtBln9")
            Dim tbbln10 As TextBox = row.FindControl("txtBln10")
            Dim tbbln11 As TextBox = row.FindControl("txtBln11")
            Dim tbbln12 As TextBox = row.FindControl("txtBln12")

            oSQLCmd = New SqlCommand(sSQL, oCnct)
            sSQL = "UPDATE iPxAcctGL_COABudget  SET bln1 ='" & tbbln1.Text & "', bln2 ='" & tbbln2.Text & "', bln3 ='" & tbbln3.Text & "', bln4 ='" & tbbln4.Text & "', bln5 ='" & tbbln5.Text & "', bln6 ='" & tbbln6.Text & "', "
            sSQL += "bln7 ='" & tbbln7.Text & "', bln8 ='" & tbbln8.Text & "', bln9 ='" & tbbln9.Text & "', bln10 ='" & tbbln10.Text & "', bln11 ='" & tbbln11.Text & "', bln12 ='" & tbbln12.Text & "' "
            sSQL += "where businessid ='" & businessid & "' and Coa ='" & hdCoa.Value.Trim & "' and periode ='" & periode & "' "
            oSQLCmd.CommandText = sSQL
            oSQLCmd.ExecuteNonQuery()
        Next
        oCnct.Close()
        ListCOAStatistic()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatisticActive", "StatisticActive();", True)
    End Function
    Public Function saveGridAsset(ByVal businessid As String, ByVal periode As String) As Boolean
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        For Each row As GridViewRow In gvCoaAsset.Rows
            Dim hdCoa As HiddenField = row.FindControl("hdCoa")
            Dim tbbln1 As TextBox = row.FindControl("txtBln1")
            Dim tbbln2 As TextBox = row.FindControl("txtBln2")
            Dim tbbln3 As TextBox = row.FindControl("txtBln3")
            Dim tbbln4 As TextBox = row.FindControl("txtBln4")
            Dim tbbln5 As TextBox = row.FindControl("txtBln5")
            Dim tbbln6 As TextBox = row.FindControl("txtBln6")
            Dim tbbln7 As TextBox = row.FindControl("txtBln7")
            Dim tbbln8 As TextBox = row.FindControl("txtBln8")
            Dim tbbln9 As TextBox = row.FindControl("txtBln9")
            Dim tbbln10 As TextBox = row.FindControl("txtBln10")
            Dim tbbln11 As TextBox = row.FindControl("txtBln11")
            Dim tbbln12 As TextBox = row.FindControl("txtBln12")

            oSQLCmd = New SqlCommand(sSQL, oCnct)
            sSQL = "UPDATE iPxAcctGL_COABudget  SET bln1 ='" & tbbln1.Text & "', bln2 ='" & tbbln2.Text & "', bln3 ='" & tbbln3.Text & "', bln4 ='" & tbbln4.Text & "', bln5 ='" & tbbln5.Text & "', bln6 ='" & tbbln6.Text & "', "
            sSQL += "bln7 ='" & tbbln7.Text & "', bln8 ='" & tbbln8.Text & "', bln9 ='" & tbbln9.Text & "', bln10 ='" & tbbln10.Text & "', bln11 ='" & tbbln11.Text & "', bln12 ='" & tbbln12.Text & "' "
            sSQL += "where businessid ='" & businessid & "' and Coa ='" & hdCoa.Value.Trim & "' and periode ='" & periode & "' "
            oSQLCmd.CommandText = sSQL
            oSQLCmd.ExecuteNonQuery()
        Next
        oCnct.Close()
        ListCOA()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AssetActive", "AssetActive();", True)
    End Function
    Public Function saveGridLia(ByVal businessid As String, ByVal periode As String) As Boolean
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        For Each row As GridViewRow In gvLia.Rows
            Dim hdCoa As HiddenField = row.FindControl("hdCoa")
            Dim tbbln1 As TextBox = row.FindControl("txtBln1")
            Dim tbbln2 As TextBox = row.FindControl("txtBln2")
            Dim tbbln3 As TextBox = row.FindControl("txtBln3")
            Dim tbbln4 As TextBox = row.FindControl("txtBln4")
            Dim tbbln5 As TextBox = row.FindControl("txtBln5")
            Dim tbbln6 As TextBox = row.FindControl("txtBln6")
            Dim tbbln7 As TextBox = row.FindControl("txtBln7")
            Dim tbbln8 As TextBox = row.FindControl("txtBln8")
            Dim tbbln9 As TextBox = row.FindControl("txtBln9")
            Dim tbbln10 As TextBox = row.FindControl("txtBln10")
            Dim tbbln11 As TextBox = row.FindControl("txtBln11")
            Dim tbbln12 As TextBox = row.FindControl("txtBln12")

            oSQLCmd = New SqlCommand(sSQL, oCnct)
            sSQL = "UPDATE iPxAcctGL_COABudget  SET bln1 ='" & tbbln1.Text & "', bln2 ='" & tbbln2.Text & "', bln3 ='" & tbbln3.Text & "', bln4 ='" & tbbln4.Text & "', bln5 ='" & tbbln5.Text & "', bln6 ='" & tbbln6.Text & "', "
            sSQL += "bln7 ='" & tbbln7.Text & "', bln8 ='" & tbbln8.Text & "', bln9 ='" & tbbln9.Text & "', bln10 ='" & tbbln10.Text & "', bln11 ='" & tbbln11.Text & "', bln12 ='" & tbbln12.Text & "' "
            sSQL += "where businessid ='" & businessid & "' and Coa ='" & hdCoa.Value.Trim & "' and periode ='" & periode & "' "
            oSQLCmd.CommandText = sSQL
            oSQLCmd.ExecuteNonQuery()
        Next
        oCnct.Close()
        ListCOALia()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "LiabilityActive", "LiabilityActive();", True)
    End Function
    Public Function saveGridEqui(ByVal businessid As String, ByVal periode As String) As Boolean
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        For Each row As GridViewRow In gvEquity.Rows
            Dim hdCoa As HiddenField = row.FindControl("hdCoa")
            Dim tbbln1 As TextBox = row.FindControl("txtBln1")
            Dim tbbln2 As TextBox = row.FindControl("txtBln2")
            Dim tbbln3 As TextBox = row.FindControl("txtBln3")
            Dim tbbln4 As TextBox = row.FindControl("txtBln4")
            Dim tbbln5 As TextBox = row.FindControl("txtBln5")
            Dim tbbln6 As TextBox = row.FindControl("txtBln6")
            Dim tbbln7 As TextBox = row.FindControl("txtBln7")
            Dim tbbln8 As TextBox = row.FindControl("txtBln8")
            Dim tbbln9 As TextBox = row.FindControl("txtBln9")
            Dim tbbln10 As TextBox = row.FindControl("txtBln10")
            Dim tbbln11 As TextBox = row.FindControl("txtBln11")
            Dim tbbln12 As TextBox = row.FindControl("txtBln12")

            oSQLCmd = New SqlCommand(sSQL, oCnct)
            sSQL = "UPDATE iPxAcctGL_COABudget  SET bln1 ='" & tbbln1.Text & "', bln2 ='" & tbbln2.Text & "', bln3 ='" & tbbln3.Text & "', bln4 ='" & tbbln4.Text & "', bln5 ='" & tbbln5.Text & "', bln6 ='" & tbbln6.Text & "', "
            sSQL += "bln7 ='" & tbbln7.Text & "', bln8 ='" & tbbln8.Text & "', bln9 ='" & tbbln9.Text & "', bln10 ='" & tbbln10.Text & "', bln11 ='" & tbbln11.Text & "', bln12 ='" & tbbln12.Text & "' "
            sSQL += "where businessid ='" & businessid & "' and Coa ='" & hdCoa.Value.Trim & "' and periode ='" & periode & "' "
            oSQLCmd.CommandText = sSQL
            oSQLCmd.ExecuteNonQuery()
        Next
        oCnct.Close()
        ListCOAEquity()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "EquityActive", "EquityActive();", True)
    End Function

    Sub editBudget()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.*, b.description, b.grpLevel from iPxAcctGL_COABudget as a "
        sSQL += "INNER JOIN iPxAcct_Coa as b ON b.businessid = a.businessid and b.Coa = a.Coa "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and a.Coa='" & Session("sCoaBudgetEdit") & "' and a.periode='" & tbDate.Text & "' "
        sSQL += " and b.isactive = 'Y' order by a.Coa asc"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            tbCOA.Text = oSQLReader.Item("Coa").ToString
            tbDesc.Text = oSQLReader.Item("description").ToString
            tbJan.Text = String.Format("{0:N2}", (oSQLReader.Item("bln1"))).ToString
            tbFeb.Text = String.Format("{0:N2}", (oSQLReader.Item("bln2"))).ToString
            tbMar.Text = String.Format("{0:N2}", (oSQLReader.Item("bln3"))).ToString
            tbApr.Text = String.Format("{0:N2}", (oSQLReader.Item("bln4"))).ToString
            tbMei.Text = String.Format("{0:N2}", (oSQLReader.Item("bln5"))).ToString
            tbJun.Text = String.Format("{0:N2}", (oSQLReader.Item("bln6"))).ToString
            tbJul.Text = String.Format("{0:N2}", (oSQLReader.Item("bln7"))).ToString
            tbAgu.Text = String.Format("{0:N2}", (oSQLReader.Item("bln8"))).ToString
            tbSep.Text = String.Format("{0:N2}", (oSQLReader.Item("bln9"))).ToString
            tbOkt.Text = String.Format("{0:N2}", (oSQLReader.Item("bln10"))).ToString
            tbNov.Text = String.Format("{0:N2}", (oSQLReader.Item("bln11"))).ToString
            tbDes.Text = String.Format("{0:N2}", (oSQLReader.Item("bln12"))).ToString
            oCnct.Close()
        Else
            oCnct.Close()
        End If
    End Sub

    Sub UpdateBudget()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)

        sSQL = "UPDATE iPxAcctGL_COABudget SET bln1='" & Replace(tbJan.Text, "'", "''") & "',bln2='" & Replace(tbFeb.Text, "'", "''") & "', "
        sSQL += "bln3='" & Replace(tbMar.Text, "'", "''") & "',bln4='" & Replace(tbApr.Text, "'", "''") & "',bln5='" & Replace(tbMei.Text, "'", "''") & "', "
        sSQL += "bln6='" & Replace(tbJun.Text, "'", "''") & "',bln7='" & Replace(tbJul.Text, "'", "''") & "',bln8='" & Replace(tbAgu.Text, "'", "''") & "', "
        sSQL += "bln9='" & Replace(tbSep.Text, "'", "''") & "',bln10='" & Replace(tbOkt.Text, "'", "''") & "',bln11='" & Replace(tbNov.Text, "'", "''") & "', "
        sSQL += "bln12='" & Replace(tbDes.Text, "'", "''") & "' "
        sSQL += "WHERE businessid ='" & Session("sBusinessID") & "' and Coa='" & Replace(tbCOA.Text, "'", "''") & "' and periode='" & tbDate.Text & "' "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub

    Sub CopyBudget()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT Coa FROM iPxAcctGL_COABudget WHERE businessid='" & Session("sBusinessID") & "' and periode ='" & tbToCopy.Text & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        If oSQLReader.Read Then
            oSQLReader.Close()
            ClearCopy()
            CopyInsert()
            ListCOA()
            ListCOALia()
            ListCOACost()
            ListCOAEquity()
            ListCOARev()
            ListCOAStatistic()
        Else
            oSQLReader.Close()
            CopyInsert()
            ListCOA()
            ListCOALia()
            ListCOACost()
            ListCOAEquity()
            ListCOARev()
            ListCOAStatistic()
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('GL Budget copy succes !!');", True)
    End Sub

    Sub CopyInsert()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "INSERT INTO iPxAcctGL_COABudget(businessid,Coa,periode,bln1,bln2,bln3,bln4,bln5,bln6,bln7,bln8,bln9,bln10,bln11,bln12) "
        sSQL += "SELECT businessid,Coa,'" & tbToCopy.Text & "',bln1,bln2,bln3,bln4,bln5,bln6,bln7,bln8,bln9,bln10,bln11,bln12 FROM iPxAcctGL_COABudget WHERE businessid='" & Session("sBusinessID") & "' and periode ='" & tbFromCopy.Text & "'"
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub

    Sub ClearCopy()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "DELETE FROM iPxAcctGL_COABudget "
        sSQL += "WHERE businessid='" & Session("sBusinessID") & "' and periode ='" & tbToCopy.Text & "'"
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("sBusinessID") = "" Then
            Response.Redirect("signin.aspx")
        End If
        If Not Page.IsPostBack Then
            If cIpx.getAccessUser(Session("sBusinessID"), Session("sUserCode"), "GL Configuration") <> True Then

                Session("sMessage") = "Sorry, you dont have access in this module |"
                Session("sMemberid") = ""
                Session("sWarningID") = "0"
                Session("sUrlOKONLY") = "home.aspx"
                Session("sUrlYES") = "http://www.thepyxis.net"
                Session("sUrlNO") = "http://www.thepyxis.net"
                Response.Redirect("warningmsg.aspx")
            End If
            Session("sTab") = ""
            Session("sQueryTicket") = ""
            tbDate.Text = Format(Now, "yyyy")
            cekBudget()
        End If
        UserAcces()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "YearGL", "$(document).ready(function() {YearGL()});", True)
    End Sub

    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvCoaAsset.PageIndex = e.NewPageIndex
        Me.ListCOA()
        gvLia.PageIndex = e.NewPageIndex
        Me.ListCOALia()
        gvCost.PageIndex = e.NewPageIndex
        Me.ListCOACost()
        gvEquity.PageIndex = e.NewPageIndex
        Me.ListCOAEquity()
        gvRev.PageIndex = e.NewPageIndex
        Me.ListCOARev()
        gvStatistic.PageIndex = e.NewPageIndex
        Me.ListCOAStatistic()
    End Sub

    Protected Sub gvCoaAsset_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvCoaAsset.PageIndexChanging
        gvCoaAsset.PageIndex = e.NewPageIndex
        Me.ListCOA()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AssetActive", "AssetActive();", True)
    End Sub

    Protected Sub gvLia_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvLia.PageIndexChanging
        gvLia.PageIndex = e.NewPageIndex
        Me.ListCOALia()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "LiabilityActive", "LiabilityActive();", True)
    End Sub

    Protected Sub gvCost_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvCost.PageIndexChanging
        gvCost.PageIndex = e.NewPageIndex
        Me.ListCOACost()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CostActive", "CostActive();", True)
    End Sub

    Protected Sub gvEquity_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvEquity.PageIndexChanging
        gvEquity.PageIndex = e.NewPageIndex
        Me.ListCOAEquity()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "EquityActive", "EquityActive();", True)
    End Sub

    Protected Sub gvRev_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvRev.PageIndexChanging
        gvRev.PageIndex = e.NewPageIndex
        Me.ListCOARev()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "RevenueActive", "RevenueActive();", True)
    End Sub

    Protected Sub gvStatistic_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvStatistic.PageIndexChanging
        gvStatistic.PageIndex = e.NewPageIndex
        Me.ListCOAStatistic()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatisticActive", "StatisticActive();", True)
    End Sub

    Protected Sub OnPageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvCoaAsset.PageIndex = e.NewPageIndex
        Me.ListCOA()
        gvLia.PageIndex = e.NewPageIndex
        Me.ListCOALia()
        gvCost.PageIndex = e.NewPageIndex
        Me.ListCOACost()
        gvEquity.PageIndex = e.NewPageIndex
        Me.ListCOAEquity()
        gvRev.PageIndex = e.NewPageIndex
        Me.ListCOARev()
        gvStatistic.PageIndex = e.NewPageIndex
        Me.ListCOAStatistic()
    End Sub
    Protected Sub cari(ByVal sender As Object, ByVal e As EventArgs)
        
    End Sub
    Protected Sub lbAddCoa_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAddCoa.Click
        Dim a As String = hfCount.Value
        If a = "Revenue" Then
            saveGridRev(Session("sBusinessID"), tbDate.Text)
        ElseIf a = "Cost" Then
            saveGridCost(Session("sBusinessID"), tbDate.Text)
        ElseIf a = "Statistic" Then
            saveGridStatis(Session("sBusinessID"), tbDate.Text)
        ElseIf a = "Asset" Then
            saveGridAsset(Session("sBusinessID"), tbDate.Text)
        ElseIf a = "Liability" Then
            saveGridLia(Session("sBusinessID"), tbDate.Text)
        ElseIf a = "Equity" Then
            saveGridEqui(Session("sBusinessID"), tbDate.Text)
        End If
    End Sub

    'Protected Sub lbAbortEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortEdit.Click
    '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
    'End Sub

    Protected Sub gvRev_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvRev.RowCommand
        If e.CommandName = "getDetail" Then
            Session("sCoaBudgetEdit") = e.CommandArgument
            editBudget()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd();", True)
        End If
    End Sub

    Protected Sub gvCost_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvCost.RowCommand
        If e.CommandName = "getDetail" Then
            Session("sCoaBudgetEdit") = e.CommandArgument
            editBudget()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CostActive", "CostActive();", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd();", True)
        End If
    End Sub

    Protected Sub gvStatistic_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvStatistic.RowCommand
        If e.CommandName = "getDetail" Then
            Session("sCoaBudgetEdit") = e.CommandArgument
            editBudget()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatisticActive", "StatisticActive();", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd();", True)
        End If
    End Sub

    Protected Sub gvCoaAsset_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvCoaAsset.RowCommand
        If e.CommandName = "getDetail" Then
            Session("sCoaBudgetEdit") = e.CommandArgument
            editBudget()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AssetActive", "AssetActive();", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd();", True)
        End If
    End Sub

    Protected Sub gvEquity_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvEquity.RowCommand
        If e.CommandName = "getDetail" Then
            Session("sCoaBudgetEdit") = e.CommandArgument
            editBudget()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "EquityActive", "EquityActive();", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd();", True)
        End If
    End Sub

    Protected Sub gvLia_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvLia.RowCommand
        If e.CommandName = "getDetail" Then
            Session("sCoaBudgetEdit") = e.CommandArgument
            editBudget()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "LiabilityActive", "LiabilityActive();", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd();", True)
        End If
    End Sub

    Protected Sub lbUpdateBudget_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbUpdateBudget.Click
        Dim a As String = hfCount.Value
        UpdateBudget()
        If a = "Revenue" Then
            ListCOARev()
        ElseIf a = "Cost" Then
            ListCOACost()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CostActive", "CostActive();", True)
        ElseIf a = "Statistic" Then
            ListCOAStatistic()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatisticActive", "StatisticActive();", True)
        ElseIf a = "Asset" Then
            ListCOA()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AssetActive", "AssetActive();", True)
        ElseIf a = "Liability" Then
            ListCOALia()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "LiabilityActive", "LiabilityActive();", True)
        ElseIf a = "Equity" Then
            ListCOAEquity()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "EquityActive", "EquityActive();", True)
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd();", True)
    End Sub
    Public Function GetData() As DataTable
        Dim com1 As SqlCommand = Nothing
        Dim con1 As SqlConnection = Nothing
        con1 = New SqlConnection(ConfigurationManager.ConnectionStrings("iPxCNCT").ConnectionString)
        con1.Open()
        sSQL = "select a.Coa, b.description, a.periode, a.bln1, a.bln2, a.bln3, a.bln4, a.bln5, a.bln6, a.bln7, a.bln8, a.bln9, a.bln10, a.bln11, a.bln12 "
        sSQL += "from iPxAcctGL_COABudget as a "
        sSQL += "INNER JOIN iPxAcct_Coa as b ON b.businessid = a.businessid and b.Coa = a.Coa "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and a.periode='" & tbDate.Text & "' and b.isactive = 'Y'"
        sSQL += " order by a.Coa asc"
        com1 = New SqlCommand(sSQL, con1)
        Dim dt As DataTable = New DataTable()
        Dim ada As SqlDataAdapter = New SqlDataAdapter(com1)
        ada.Fill(dt)
        Return dt
    End Function

    Protected Sub lbFormatExc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbFormatExc.Click
        Dim dt As DataTable = GetData()
        Dim attachment As String = "attachment; filename=COA_Budget.xls"
        Response.ClearContent()
        Response.AddHeader("content-disposition", attachment)
        Response.ContentType = "application/vnd.ms-excel"
        Dim tab As String = ""
        'Response.Write(vbLf)
        'Response.Write(vbLf)
        For Each dc As DataColumn In dt.Columns
            If dc.ColumnName = "bln1" Then
                Response.Write(tab + "Januari")
                tab = vbTab
            ElseIf dc.ColumnName = "bln2" Then
                Response.Write(tab + "Februari")
                tab = vbTab
            ElseIf dc.ColumnName = "bln3" Then
                Response.Write(tab + "Maret")
                tab = vbTab
            ElseIf dc.ColumnName = "bln4" Then
                Response.Write(tab + "April")
                tab = vbTab
            ElseIf dc.ColumnName = "bln5" Then
                Response.Write(tab + "Mei")
                tab = vbTab
            ElseIf dc.ColumnName = "bln6" Then
                Response.Write(tab + "Juni")
                tab = vbTab
            ElseIf dc.ColumnName = "bln7" Then
                Response.Write(tab + "Juli")
                tab = vbTab
            ElseIf dc.ColumnName = "bln8" Then
                Response.Write(tab + "Agustus")
                tab = vbTab
            ElseIf dc.ColumnName = "bln9" Then
                Response.Write(tab + "September")
                tab = vbTab
            ElseIf dc.ColumnName = "bln10" Then
                Response.Write(tab + "Oktober")
                tab = vbTab
            ElseIf dc.ColumnName = "bln11" Then
                Response.Write(tab + "November")
                tab = vbTab
            ElseIf dc.ColumnName = "bln12" Then
                Response.Write(tab + "Desember")
                tab = vbTab
            Else
                Response.Write(tab + dc.ColumnName)
                tab = vbTab
            End If
        Next
        Response.Write(vbLf)

        Dim i As Integer
        For Each dr As DataRow In dt.Rows
            tab = ""
            For i = 0 To dt.Columns.Count - 1
                Response.Write(tab & dr(i).ToString())
                tab = vbTab
            Next
            Response.Write(vbLf)
        Next
        Response.End()

        'Dim emList As List(Of EmployeeMaster) = dc.EmployeeMasters.ToList()
        'Dim sb As StringBuilder = New StringBuilder()
        ''If emList.count > 0 Then
        'Dim fileName As String = Path.Combine(Server.MapPath("~/UploadFile"), "uploadGLBudget.xlsx")
        'Dim conString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & fileName & ";Extended Properties='Excel 12.0 Xml;HDR=Yes'"

        'Dim content As Byte() = File.ReadAllBytes(fileName)
        'Dim context As HttpContext = HttpContext.Current
        'context.Response.BinaryWrite(content)
        'context.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        'context.Response.AppendHeader("Content-Disposition", "attachment; filename=EmployeeData.xlsx")
        'context.Response.[End]()
        'End If
        
    End Sub

    Protected Sub lbImport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbImport.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalImport", "showModalImport();", True)
    End Sub

    Protected Sub lbStartImport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbStartImport.Click
        SaveMassUpluad()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalImport", "hideModalImport();", True)
    End Sub

    Protected Sub lbPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbPrint.Click
        Session("sReport") = "GLBudget1"
        Session("sMapPath") = "~/iPxReportFile/dckGL_Budget_P1.rpt"
        Session("sPeriod") = tbDate.Text
        Response.Redirect("rptviewer.aspx")
    End Sub

    Protected Sub lbPrint2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbPrint2.Click
        Session("sReport") = "GLBudget2"
        Session("sMapPath") = "~/iPxReportFile/dckGL_Budget_P2.rpt"
        Session("sPeriod") = tbDate.Text
        Response.Redirect("rptviewer.aspx")
    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        Dim a As String = hfCount.Value
        cekBudget()
        If a = "Revenue" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "RevenueActive", "RevenueActive();", True)
        ElseIf a = "Cost" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CostActive", "CostActive();", True)
        ElseIf a = "Statistic" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "StatisticActive", "StatisticActive();", True)
        ElseIf a = "Asset" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AssetActive", "AssetActive();", True)
        ElseIf a = "Liability" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "LiabilityActive", "LiabilityActive();", True)
        ElseIf a = "Equity" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "EquityActive", "EquityActive();", True)
        End If
    End Sub

    Protected Sub lbCopy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCopy.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalCopy", "showModalCopy();", True)
    End Sub

    Protected Sub lbAbortCopy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortCopy.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalCopy", "hideModalCopy();", True)
    End Sub

    Protected Sub lbStartCopy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbStartCopy.Click
        CopyBudget()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalCopy", "hideModalCopy();", True)
    End Sub
End Class
