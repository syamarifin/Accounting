Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration
Imports System.Data.OleDb
Partial Class iPxAdmin_iPxGLInputTransaction
    Inherits System.Web.UI.Page
    Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
    Dim oSQLCmd As SqlCommand
    Dim oSQLReader As SqlDataReader
    Dim sSQL, status, Close, active, RecID, MonthCekClosing, YearCekClosing, coaCount As String
    Dim cIpx As New iPxClass
    Sub UserAcces()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.businessid, a.usercode, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.funtionid='24'and x.active='Y' and x.businessid=a.businessid and x.usercode=a.usercode) as editGL "
        sSQL += "from iPxAcct_profile_client_useraccess as a "
        sSQL += "INNER JOIN iPxAcct_profile_client_useraccess_dtl as b ON b.businessid=a.businessid and b.usercode=a.usercode "
        sSQL += "where a.businessid='" & Session("sBusinessID") & "' and a.usercode='" & Session("sUserCode") & "' and a.active='Y' group by a.businessid, a.usercode "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            If oSQLReader.Item("editGL").ToString = "Y" Then
                'lbAddDetail.Enabled = True
                'lbAbortAdd.Enabled = True
                'lbAddGroup.Enabled = True
                If status <> "D" Then
                    If Close = "Close" Then
                        lbAddDetail.Enabled = False
                        'lbAddGroup.Enabled = False
                        lbAbortAdd.Enabled = False
                        lbRecaring.Enabled = False
                        lbImport.Enabled = False
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DateIsActive", "$(document).ready(function() {DateIsActive()});", True)
                    Else
                        If tbReff.Enabled = False Then
                            lbAddDetail.Enabled = False
                            'lbAddGroup.Enabled = False
                            lbAbortAdd.Enabled = True
                            lbRecaring.Enabled = False
                            lbImport.Enabled = False
                        Else
                            lbAddDetail.Enabled = True
                            'lbAddGroup.Enabled = True
                            lbAbortAdd.Enabled = True
                            lbRecaring.Enabled = True
                            lbImport.Enabled = True
                        End If
                    End If
                Else
                    lbAddDetail.Enabled = False
                    lbAbortAdd.Enabled = False
                    lbImport.Enabled = False
                End If
            Else
                lbAddDetail.Enabled = False
                lbAbortAdd.Enabled = False
                lbImport.Enabled = False
                'lbAddGroup.Enabled = False
            End If
        Else
            lbAddDetail.Enabled = False
            lbAbortAdd.Enabled = False
            lbImport.Enabled = False
            'lbAddGroup.Enabled = False
        End If
        oCnct.Close()
    End Sub
    Sub cekGLAutoPost()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select 'Y' as autoPost from iPxAcctGL_JVhdr as a "
        sSQL += "where a.businessid='" & Session("sBusinessID") & "' and a.Status<>'D' and "
        sSQL += "(a.ReffNo in (select transID from iPxAcct_Log where businessid=a.businessid and funct='Posting' and Grp like 'AR%' and transID='" & tbReff.Text & "') "
        sSQL += "or TransID in (select transID from iPxAcct_Log where businessid=a.businessid and funct='Posting' and Grp='SL' and transID='" & tbTransID.Text & "'))"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            tbReff.Enabled = False
        Else
            tbReff.Enabled = True
        End If
        oCnct.Close()
    End Sub
    Sub kosong()
        tbCoaDtl.Text = ""
        tbCreditDtl.Text = ""
        tbDebitDtl.Text = ""
        tbDescDtl.Text = ""
        tbReffDtl.Text = ""
        tbCoaDescDtl.Text = ""
    End Sub
    Sub totalDebcre()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT Sum(Credit) as Credit, sum(Debit) as Debit FROM iPxAcctGL_JVdtl where businessid='" & Session("sBusinessID") & "' and TransID = '" & Replace(tbTransID.Text, "'", "''") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            lblTotCredit.Text = oSQLReader.Item("Credit").ToString
            lblTotDebit.Text = oSQLReader.Item("Debit").ToString
        Else
            lblTotCredit.Text = "0"
            lblTotDebit.Text = "0"
        End If
        oCnct.Close()
    End Sub
    Sub idGLDtl()
        Dim sCnct1 As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
        Dim oCnct1 As SqlConnection = New SqlConnection(sCnct1)
        Dim oSQLCmd1 As SqlCommand
        Dim oSQLReader1 As SqlDataReader
        If oCnct1.State = ConnectionState.Closed Then
            oCnct1.Open()
        End If
        oSQLCmd1 = New SqlCommand(sSQL, oCnct1)
        sSQL = "SELECT MAX(RecID) as RecID FROM iPxAcctGL_JVdtl where TransID = '" & Replace(tbTransID.Text, "'", "''") & "'"
        oSQLCmd1.CommandText = sSQL
        oSQLReader1 = oSQLCmd1.ExecuteReader

        oSQLReader1.Read()
        If oSQLReader1.HasRows Then
            RecID = Val(oSQLReader1.Item("RecID").ToString) + 1

        Else
            RecID = "1"
        End If
        oCnct1.Close()
    End Sub
    Sub Glgroup()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcctGL_JVGrp where (businessid = '" & Session("sBusinessID") & "' or businessid = 'DF')"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                dlGroup.DataSource = dt
                dlGroup.DataTextField = "Description"
                dlGroup.DataValueField = "id"
                dlGroup.DataBind()
                dlGroup.Items.Insert(0, "")
            End Using
        End Using
    End Sub
    Sub ListGLDetail()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT a.*,b.*,(c.description)as CoaDesc, "
        sSQL += "(select 'Close' from iPxAcctGL_JVhdr as x where x.businessid ='" & Session("sBusinessID") & "' AND x.Status <> 'D' AND x.ReffNo like'PL%' and YEAR(x.TransDate)=YEAR(b.TransDate) AND MONTH(x.TransDate)=MONTH(b.TransDate)) as CloseStatus, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid=a.businessid and x.usercode='" & Session("sUserCode") & "' and x.funtionid='24' and x.active='Y') as editGL, "
        sSQL += "(select COUNT(id) from iPxAcct_Log where businessid=a.businessid and funct='Posting' and (transID='" & tbTransID.Text & "' or transID='" & tbReff.Text & "')) as autoPost "
        sSQL += "from iPxAcctGL_JVdtl as a INNER JOIN iPxAcctGL_JVhdr as b ON b.TransID=a.TransID "
        sSQL += "LEFT JOIN iPxAcct_Coa as c ON c.Coa=a.Coa "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and a.TransID='" & tbTransID.Text & "' "
        sSQL += " and a.isactive = 'Y' order by a.RecID asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvGLDetail.DataSource = dt
                    gvGLDetail.DataBind()
                    'Calculate Sum and display in Footer Row
                    Dim totalDeb As Decimal = dt.Compute("Sum(Debit)", "").ToString()
                    Dim totalCre As Decimal = dt.Compute("Sum(Credit)", "").ToString()
                    gvGLDetail.FooterRow.Cells(3).Text = "Total"
                    gvGLDetail.FooterRow.Cells(3).HorizontalAlign = HorizontalAlign.Left
                    gvGLDetail.FooterRow.Cells(4).Text = totalDeb.ToString("N2")
                    gvGLDetail.FooterRow.Cells(4).HorizontalAlign = HorizontalAlign.Right
                    gvGLDetail.FooterRow.Cells(5).Text = totalCre.ToString("N2")
                    gvGLDetail.FooterRow.Cells(5).HorizontalAlign = HorizontalAlign.Right
                    gvGLDetail.Enabled = True
                    If totalCre = totalDeb Then
                        Session("sBalanceTot") = True
                    Else
                        Session("sBalanceTot") = False
                    End If
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvGLDetail.DataSource = dt
                    gvGLDetail.DataBind()
                    gvGLDetail.Enabled = False
                    gvGLDetail.Rows(0).Visible = False
                    Session("sBalanceTot") = True
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub cekGLHeader()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT TransID FROM iPxAcctGL_JVhdr WHERE TransID = '" & tbTransID.Text & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        If oSQLReader.Read Then
            oSQLReader.Close()
            UpdateGLHeader()
            saveGLDtl()
        Else
            oSQLReader.Close()
            saveGLHeader()
            saveGLDtl()
        End If
    End Sub
    Sub cekGLHeaderAbort()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT TransID FROM iPxAcctGL_JVhdr WHERE TransID = '" & tbTransID.Text & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        If oSQLReader.Read Then
            oSQLReader.Close()
            UpdateGLHeader()
            'saveGLDtl()
        Else
            oSQLReader.Close()
            saveGLHeader()
            'saveGLDtl()
        End If
    End Sub
    Sub saveGLHeader()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        Dim GLDate As Date
        GLDate = Date.ParseExact(tbDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)

        sSQL = "INSERT INTO iPxAcctGL_JVhdr(businessid, TransID, TransDate, Status, JVgroup, ReffNo, Description) "
        sSQL += "VALUES ('" & Session("sBusinessID") & "','" & Replace(tbTransID.Text, "'", "''") & "','" & GLDate & "', "
        sSQL += "'O','" & dlGroup.SelectedValue & "','" & Replace(tbReff.Text, "'", "''") & "','" & Replace(tbDesc.Text, "'", "''") & "') "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        Dim TransDateLog As String = Format(Now, "yyy-MM-dd hh:mm:ss")
        cIpx.saveLog(Session("sBusinessID"), Replace(tbTransID.Text, "'", "''"), TransDateLog, "GL TR", "Save", "Create New general ledger " & Replace(tbTransID.Text, "'", "''"), "", Session("sUserCode"))

    End Sub
    Sub saveGLDtl()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "INSERT INTO iPxAcctGL_JVdtl(RecID, businessid, TransID, Coa, Description, Reff, Debit, Credit, isActive) "
        sSQL += "VALUES ('" & RecID & "','" & Session("sBusinessID") & "','" & Replace(tbTransID.Text, "'", "''") & "','" & Replace(tbCoaDtl.Text, "'", "''") & "', "
        sSQL += "'" & Replace(tbDescDtl.Text, "'", "''") & "','" & Replace(tbReffDtl.Text, "'", "''") & "','" & Replace(tbDebitDtl.Text, "'", "''") & "','" & Replace(tbCreditDtl.Text, "'", "''") & "', "
        sSQL += "'Y') "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        Dim TransDateLog As String = Format(Now, "yyy-MM-dd hh:mm:ss")
        cIpx.saveLog(Session("sBusinessID"), Replace(tbTransID.Text, "'", "''"), TransDateLog, "GL TR", "Save", "Create New general ledger detail " & Replace(tbTransID.Text, "'", "''"), "", Session("sUserCode"))
    End Sub
    Sub editGLHeader()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT a.*,(select 'Close' from iPxAcctGL_JVhdr as x where x.businessid ='" & Session("sBusinessID") & "' AND x.Status <> 'D' AND x.ReffNo like'PL%' and YEAR(x.TransDate)=YEAR(a.TransDate) AND MONTH(x.TransDate)=MONTH(a.TransDate)) as CloseStatus FROM iPxAcctGL_JVhdr as a "
        sSQL += "WHERE a.businessid ='" & Session("sBusinessID") & "' and a.TransID ='" & Session("sEditGL") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            Dim InvDate As Date = oSQLReader.Item("TransDate").ToString
            tbDate.Text = InvDate.ToString("dd/MM/yyyy")
            tbTransID.Text = oSQLReader.Item("TransID").ToString
            tbReff.Text = oSQLReader.Item("ReffNo").ToString
            status = oSQLReader.Item("Status").ToString
            tbDesc.Text = oSQLReader.Item("Description").ToString
            Dim grp As String = oSQLReader.Item("JVgroup").ToString
            Close = oSQLReader.Item("CloseStatus").ToString
            oCnct.Close()
            Glgroup()
            dlGroup.SelectedValue = grp
        Else
            oCnct.Close()
        End If
    End Sub
    Sub EditGLDtl()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT a.*, (b.Description) as DescCoa FROM iPxAcctGL_JVdtl as a "
        sSQL += "INNER JOIN iPxAcct_Coa as b ON b.businessid=a.businessid and b.Coa = a.Coa "
        sSQL += "WHERE a.businessid ='" & Session("sBusinessID") & "' and a.TransID ='" & Replace(tbTransID.Text, "'", "''") & "' and a.RecID='" & Session("RecID") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            tbCoaDtl.Text = oSQLReader.Item("Coa").ToString
            tbReffDtl.Text = oSQLReader.Item("Reff").ToString
            tbCoaDescDtl.Text = oSQLReader.Item("DescCoa").ToString
            tbDebitDtl.Text = String.Format("{0:N2}", (oSQLReader.Item("Debit"))).ToString
            tbCreditDtl.Text = String.Format("{0:N2}", (oSQLReader.Item("Credit"))).ToString
            tbDescDtl.Text = oSQLReader.Item("Description").ToString
            oCnct.Close()
        Else
            oCnct.Close()
        End If
    End Sub
    Sub cekCoaCount()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select COUNT(coa) as Count from iPxAcct_Coa as a "
        sSQL += "WHERE a.businessid ='" & Session("sBusinessID") & "' and (a.Coa like '" & tbCoaDtl.Text & "%')"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            coaCount = oSQLReader.Item("Count").ToString
            oCnct.Close()
            If coaCount <> "1" Then
                tbFindCoaList.Text = tbCoaDtl.Text
                Session("sFindCOA") = Replace(tbCoaDtl.Text, "'", "''")
                ListCOA()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalListCOA", "showModalListCOA();", True)
            Else
                selectCoa()
                tbDescDtl.Focus()
            End If
        Else
            oCnct.Close()
        End If
    End Sub
    Public Sub ListCOA()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT a.businessid, a.Coa, a.description, a.type, a.d_c, a.grpLevel, a.levelid, (b.Description) AS Devision, (c.Description) AS Departement, "
        sSQL += "(d.Description) AS SubDepartement, a.Status, a.notes, a.isactive "
        sSQL += "FROM iPxAcct_Coa AS a "
        sSQL += "INNER JOIN iPxAcct_CoaDivision AS b ON a.businessid = b.businessid COLLATE Latin1_General_CI_AS AND a.Devision = b.Division "
        sSQL += "INNER JOIN iPxAcct_CoaDepartement AS c ON a.businessid = c.businessid COLLATE Latin1_General_CI_AS AND b.Division = c.Division "
        sSQL += "AND a.Departement = c.Departement "
        sSQL += "INNER JOIN iPxAcct_CoaSubDepartement AS d ON a.businessid = d.businessid COLLATE Latin1_General_CI_AS "
        sSQL += "AND a.SubDepartement = d.SubDept AND c.Division = d.Division AND c.Departement = d.Departement "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and (a.Coa like '" & Session("sFindCOA") & "%' or a.description like '" & Session("sFindCOA") & "%' or a.type like '" & Session("sFindCOA") & "%')"
        sSQL += " and a.isactive = 'Y'"
        sSQL += " order by a.Coa asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvCoa.DataSource = dt
                    gvCoa.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvCoa.DataSource = dt
                    gvCoa.DataBind()
                    gvCoa.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub selectCoa()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcct_Coa "
        sSQL += "WHERE Coa ='" & tbCoaDtl.Text & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            tbCoaDtl.Text = oSQLReader.Item("Coa").ToString
            tbCoaDescDtl.Text = oSQLReader.Item("Description").ToString
        Else
            oCnct.Close()
        End If
    End Sub
    Sub UpdateGLHeader()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        Dim GLDate As Date
        GLDate = Date.ParseExact(tbDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)

        sSQL = "UPDATE iPxAcctGL_JVhdr SET  TransDate='" & GLDate & "', JVgroup='" & dlGroup.SelectedValue & "', ReffNo='" & Replace(tbReff.Text, "'", "''") & "', Description='" & Replace(tbDesc.Text, "'", "''") & "'"
        sSQL = sSQL & "WHERE businessid ='" & Session("sBusinessID") & "' and TransID ='" & tbTransID.Text & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        Dim TransDateLog As String = Format(Now, "yyy-MM-dd hh:mm:ss")
        cIpx.saveLog(Session("sBusinessID"), Replace(tbTransID.Text, "'", "''"), TransDateLog, "GL TR", "Update", "Update general ledger " & Replace(tbTransID.Text, "'", "''"), "", Session("sUserCode"))
    End Sub
    Sub updateGLDtl()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        
        sSQL = "UPDATE iPxAcctGL_JVdtl SET  Coa='" & Replace(tbCoaDtl.Text, "'", "''") & "', Reff='" & Replace(tbReffDtl.Text, "'", "''") & "', Description='" & Replace(tbDescDtl.Text, "'", "''") & "'"
        sSQL += ",Debit='" & Replace(tbDebitDtl.Text, "'", "''") & "',Credit='" & Replace(tbCreditDtl.Text, "'", "''") & "'"
        sSQL += "WHERE businessid ='" & Session("sBusinessID") & "' and TransID ='" & tbTransID.Text & "' and RecID='" & RecID & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        Dim TransDateLog As String = Format(Now, "yyy-MM-dd hh:mm:ss")
        cIpx.saveLog(Session("sBusinessID"), Replace(tbTransID.Text, "'", "''"), TransDateLog, "GL TR", "Update", "Update general ledger detail " & Replace(tbTransID.Text, "'", "''"), "", Session("sUserCode"))
        tbCoaDtl.Text = ""
        tbReffDtl.Text = ""
        tbDebitDtl.Text = ""
        tbCreditDtl.Text = ""
        tbDescDtl.Text = ""
        tbCoaDescDtl.Text = ""
    End Sub
    Sub DeleteGLDtl()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        Dim GLDate As Date
        GLDate = Date.ParseExact(tbDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)

        sSQL = "UPDATE iPxAcctGL_JVdtl SET isActive='N' "
        sSQL += "WHERE businessid ='" & Session("sBusinessID") & "' and TransID ='" & tbTransID.Text & "' and RecID='" & Session("RecID") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        Dim TransDateLog As String = Format(Now, "yyy-MM-dd hh:mm:ss")
        cIpx.saveLog(Session("sBusinessID"), Replace(tbTransID.Text, "'", "''"), TransDateLog, "GL TR", "Delete", "Delete general ledger detail " & Session("RecID") & " of " & Replace(tbTransID.Text, "'", "''"), "", Session("sUserCode"))
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
        sSQL4 = "SELECT * FROM iPxAcctGL_JVhdr where businessid='" & Session("sBusinessID") & "' "
        sSQL4 += "and ReffNo like 'PL " & MonthCekClosing & "-" & YearCekClosing & "' and status<>'D'"
        oSQLCmd4.CommandText = sSQL4
        oSQLReader4 = oSQLCmd4.ExecuteReader

        oSQLReader4.Read()
        If oSQLReader4.HasRows Then
            lbAddDetail.Enabled = False
            lbAbortAdd.Enabled = False
            lbRecaring.Enabled = False
            lbImport.Enabled = False
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('This Month Period Has been Close !!');", True)
        Else
            lbAddDetail.Enabled = True
            lbAbortAdd.Enabled = True
            lbRecaring.Enabled = True
            lbImport.Enabled = True
        End If
        oCnct4.Close()
    End Sub
    Protected Sub cari(ByVal sender As Object, ByVal e As EventArgs)
        MonthCekClosing = Strings.Mid(tbDate.Text, 4, 2)
        YearCekClosing = Strings.Right(tbDate.Text, 4)
        cekClose()
    End Sub

    Sub ListGLRecaring()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT a.*, (b.Description) AS GlGrp,(select sum(Credit) from iPxAcctGL_JVdtlRec where businessid ='" & Session("sBusinessID") & "' and TransID = a.TransID and IsActive='Y') as totCredit, "
        sSQL += "(select sum(Debit) from iPxAcctGL_JVdtlRec where businessid ='" & Session("sBusinessID") & "' and TransID = a.TransID and IsActive='Y') as totDebit, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid=a.businessid and x.usercode='" & Session("sUserCode") & "' and x.funtionid='25' and x.active='Y') as deleteGL "
        sSQL += "from iPxAcctGL_JVhdrRec AS a INNER JOIN iPxAcctGL_JVGrp AS b ON b.id=a.JVgroup "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' "
        sSQL += " AND a.Status <> 'D' "
        If Session("sQueryTicket") = "" Then
            Session("sQueryTicket") = Session("sCondition")
            If Session("sQueryTicket") <> "" Or Session("sCondition") <> "" Then
                sSQL = sSQL & Session("sQueryTicket")
                Session("sCondition") = ""
            Else
                sSQL = sSQL & " "
            End If
        Else
            sSQL = sSQL & Session("sQueryTicket")
            Session("sCondition") = ""
        End If
        sSQL += " order by a.TransDate asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvRecaring.DataSource = dt
                    gvRecaring.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvRecaring.DataSource = dt
                    gvRecaring.DataBind()
                    gvRecaring.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub

    Sub ListGLRecaringDetail()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT a.*,b.*,(c.description)as CoaDesc "
        sSQL += "from iPxAcctGL_JVdtlRec as a INNER JOIN iPxAcctGL_JVhdrRec as b ON b.TransID=a.TransID "
        sSQL += "LEFT JOIN iPxAcct_Coa as c ON c.Coa=a.Coa "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and a.TransID='" & Session("sRecuring") & "' "
        sSQL += " and a.isactive = 'Y' order by a.RecID asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvRecaringDtl.DataSource = dt
                    gvRecaringDtl.DataBind()
                    'Calculate Sum and display in Footer Row
                    Dim totalDeb As Decimal = dt.Compute("Sum(Debit)", "").ToString()
                    Dim totalCre As Decimal = dt.Compute("Sum(Credit)", "").ToString()
                    gvRecaringDtl.FooterRow.Cells(3).Text = "Total"
                    gvRecaringDtl.FooterRow.Cells(3).HorizontalAlign = HorizontalAlign.Left
                    gvRecaringDtl.FooterRow.Cells(4).Text = totalDeb.ToString("N2")
                    gvRecaringDtl.FooterRow.Cells(4).HorizontalAlign = HorizontalAlign.Right
                    gvRecaringDtl.FooterRow.Cells(5).Text = totalCre.ToString("N2")
                    gvRecaringDtl.FooterRow.Cells(5).HorizontalAlign = HorizontalAlign.Right
                    gvRecaringDtl.Enabled = True
                    If totalCre = totalDeb Then
                        Session("sBalanceTot") = True
                    Else
                        Session("sBalanceTot") = False
                    End If
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvRecaringDtl.DataSource = dt
                    gvRecaringDtl.DataBind()
                    gvRecaringDtl.Enabled = False
                    gvRecaringDtl.Rows(0).Visible = False
                    Session("sBalanceTot") = True
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub

    Sub saveByRecuringHdr()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        Dim GLDate As Date
        GLDate = Date.ParseExact(tbDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)

        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "INSERT INTO iPxAcctGL_JVhdr(businessid, TransID, TransDate, Status, JVgroup, ReffNo, Description) "
        sSQL += "SELECT '" & Session("sBusinessID") & "','" & Replace(tbTransID.Text, "'", "''") & "','" & GLDate & "',Status,JVgroup,ReffNo,Description FROM iPxAcctGL_JVhdrRec where TransID='" & Session("sRecuring") & "' "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub

    Sub saveByRecuringDtl()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If

        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "INSERT INTO iPxAcctGL_JVdtl(RecID, businessid, TransID, Coa, Description, Reff, Debit, Credit, isActive) "
        sSQL += "SELECT RecID,'" & Session("sBusinessID") & "','" & Replace(tbTransID.Text, "'", "''") & "',Coa,Description,Reff,Debit,Credit,isActive "
        sSQL += "FROM iPxAcctGL_JVdtlRec where TransID='" & Session("sRecuring") & "' and RecID not in (select RecID from iPxAcctGL_JVdtl where TransID='" & Replace(tbTransID.Text, "'", "''") & "' and isActive ='Y')"
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub

    Sub cekGLDtlRecuring()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcctGL_JVdtlRec where TransID='" & Session("sRecuring") & "' "
        sSQL += "and Coa not in (select Coa from iPxAcctGL_JVdtl where TransID='" & Replace(tbTransID.Text, "'", "''") & "' and isActive ='Y')"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader
        While oSQLReader.Read
            updateGLDtlRecuring(oSQLReader.Item("Coa"), oSQLReader.Item("Description"), oSQLReader.Item("Reff"), oSQLReader.Item("Debit"), oSQLReader.Item("Credit"))
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('posting successful !');document.getElementById('Buttonx').click()", True)
        End While
        oCnct.Close()
    End Sub

    Public Function updateGLDtlRecuring(ByVal Coa As String, ByVal Desc As String, ByVal Reff As String, ByVal Debit As String, ByVal Credit As String) As Boolean
        Dim oCnct2 As SqlConnection = New SqlConnection(sCnct)
        Dim oSQLCmd2 As SqlCommand
        Dim sSQL2 As String
        idGLDtl()
        If oCnct2.State = ConnectionState.Closed Then
            oCnct2.Open()
        End If

        oSQLCmd2 = New SqlCommand(sSQL2, oCnct2)
        sSQL2 = "INSERT INTO iPxAcctGL_JVdtl(RecID, businessid, TransID, Coa, Description, Reff, Debit, Credit, isActive) "
        sSQL2 += "VALUES ('" & RecID & "','" & Session("sBusinessID") & "','" & Replace(tbTransID.Text, "'", "''") & "','" & Coa & "', "
        sSQL2 += "'" & Desc & "','" & Reff & "','" & Debit & "','" & Credit & "', 'Y') "
        oSQLCmd2.CommandText = sSQL2
        oSQLCmd2.ExecuteNonQuery()

        oCnct2.Close()
    End Function
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
            connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & excelPath & ";Extended Properties='Excel 12.0;HDR=Yes;IMEX=2'"
            'connString = String.Format(connString, excelPath)
            Dim oledbConn As OleDbConnection = New OleDbConnection(connString)
            Try
                oledbConn.Open()
                Dim cmd As OleDbCommand = New OleDbCommand("SELECT * FROM [Sheet1$]", oledbConn)
                Dim oleda As OleDbDataAdapter = New OleDbDataAdapter()
                oleda.SelectCommand = cmd
                Dim ds As DataSet = New DataSet()
                oleda.Fill(ds, "GLTransaction")
                'ds.Tables(0).Rows

                For Each Drr As DataRow In ds.Tables(0).Rows
                    idGLDtl()
                    If oCnct.State = ConnectionState.Closed Then
                        oCnct.Open()
                    End If
                    oSQLCmd = New SqlCommand(sSQL, oCnct)
                    sSQL = "INSERT INTO iPxAcctGL_JVdtl(RecID, businessid, TransID, Coa, Description, Reff, Debit, Credit, isActive) "
                    sSQL += "VALUES ('" & RecID & "','" & Session("sBusinessID") & "','" & Replace(tbTransID.Text, "'", "''") & "','" & Drr(0) & "', "
                    sSQL += "'" & Drr(2) & "','" & Drr(3) & "','" & Drr(4) & "','" & Drr(5) & "', "
                    sSQL += "'Y') "
                    oSQLCmd.CommandText = sSQL
                    oSQLCmd.ExecuteNonQuery()

                    oCnct.Close()
                Next
            Catch
            Finally
                oledbConn.Close()
                System.IO.File.Delete(excelPath)
            End Try
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('import successfull !!');", True)
            ListGLDetail()
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('import failed, file extension must be .xslx !!');", True)
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            If Session("sEditGL") = "" Then
                tbDate.Text = Format(Now, "dd/MM/yyyy")
                tbTransID.Text = cIpx.GetCounterMBR("GL", "GL")
                Glgroup()
                ListGLDetail()
                tbReff.Enabled = True
                dlGroup.Enabled = True
                tbDesc.Enabled = True
                'lbAddGroup.Enabled = True
                lbAbortAdd.Text = "<i class='fa fa-save'></i> Save"
            ElseIf Session("sEditGL") <> "" Then
                editGLHeader()
                ListGLDetail()
                'If status = "O" Then
                '    If Close = "Close" Then
                '        lbAddDetail.Enabled = False
                '        lbAddGroup.Enabled = False
                '        lbAbortAdd.Enabled = False
                '        lbRecaring.Enabled = False
                '    Else
                '        lbAddDetail.Enabled = True
                '        lbAddGroup.Enabled = True
                '        lbAbortAdd.Enabled = True
                '        lbRecaring.Enabled = True
                '    End If
                'Else
                '    lbAddDetail.Enabled = False
                '    lbAbortAdd.Enabled = False
                'End If
                cekGLAutoPost()
                lbAbortAdd.Text = "<i class='fa fa-save'></i> Update"
            End If
            UserAcces()
            lbAddDetail.Text = "<i class='fa fa-plus'></i> Add"
            'Else
            '    tbCoaDtl.Attributes.Add("onkeypress", "return controlEnter('" + lbFindCoa.ClientID + "', event)")
        End If
        totalDebcre()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "tanggal", "$(document).ready(function() {tanggal()});", True)
    End Sub


    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvGLDetail.PageIndex = e.NewPageIndex
        Me.ListGLDetail()
        gvCoa.PageIndex = e.NewPageIndex
        Me.ListCOA()
        gvRecaring.PageIndex = e.NewPageIndex
        Me.ListGLRecaring()
        gvRecaringDtl.PageIndex = e.NewPageIndex
        Me.ListGLRecaringDetail()
    End Sub

    Protected Sub gvGLDetail_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvGLDetail.PageIndexChanging
        gvGLDetail.PageIndex = e.NewPageIndex
        Me.ListGLDetail()
    End Sub

    Protected Sub gvCoa_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvCoa.PageIndexChanging
        gvCoa.PageIndex = e.NewPageIndex
        Me.ListCOA()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalListCOA", "hideModalListCOA()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalListCOA", "showModalListCOA()", True)
    End Sub
    Protected Sub gvRecaring_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvRecaring.PageIndexChanging
        gvRecaring.PageIndex = e.NewPageIndex
        Me.ListGLRecaring()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalListRecaring", "hideModalListRecaring()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalListRecaring", "showModalListRecaring()", True)
    End Sub
    Protected Sub gvRecaringDtl_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvRecaringDtl.PageIndexChanging
        gvRecaringDtl.PageIndex = e.NewPageIndex
        Me.ListGLRecaringDetail()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalListRecaring", "hideModalListRecaring()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalListRecaring", "showModalListRecaring()", True)
    End Sub

    Protected Sub OnPageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvGLDetail.PageIndex = e.NewPageIndex
        Me.ListGLDetail()
        gvCoa.PageIndex = e.NewPageIndex
        Me.ListCOA()
        gvRecaring.PageIndex = e.NewPageIndex
        Me.ListGLRecaring()
        gvRecaringDtl.PageIndex = e.NewPageIndex
        Me.ListGLRecaringDetail()
    End Sub
    Protected Sub lbAbortAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortAdd.Click
        If Session("sBalanceTot") = True Then
            If lbAddDetail.Text = "<i class='fa fa-plus'></i> Add" Then
                idGLDtl()
            Else
                RecID = Session("RecID")
            End If
            cekGLHeaderAbort()
            'If oCnct.State = ConnectionState.Closed Then
            '    oCnct.Open()
            'End If
            'oSQLCmd = New SqlCommand(sSQL, oCnct)
            'sSQL = "SELECT Coa FROM iPxAcct_Coa WHERE (businessid ='" & Session("sBusinessID") & "' or businessid='DF') and Coa ='" & tbCoaDtl.Text & "'"
            'oSQLCmd.CommandText = sSQL
            'oSQLReader = oSQLCmd.ExecuteReader

            'If oSQLReader.Read Then
            '    oSQLReader.Close()
            '    If oCnct.State = ConnectionState.Closed Then
            '        oCnct.Open()
            '    End If
            '    oSQLCmd = New SqlCommand(sSQL, oCnct)
            '    sSQL = "SELECT RecID FROM iPxAcctGL_JVdtl WHERE businessid ='" & Session("sBusinessID") & "' and TransID ='" & tbTransID.Text & "' and RecID = '" & RecID & "'"
            '    oSQLCmd.CommandText = sSQL
            '    oSQLReader = oSQLCmd.ExecuteReader

            '    If oSQLReader.Read Then
            '        oSQLReader.Close()
            '        updateGLDtl()
            '        lbCancelAdd.Visible = False
            '        lbAddDetail.Text = "<i class='fa fa-plus'></i> Add"
            '    Else
            '        oSQLReader.Close()
            '        cekGLHeader()
            '    End If
            'Else
            '    oSQLReader.Close()
            '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('COA is not registered !!');", True)
            '    tbCoaDtl.Focus()
            'End If
            Response.Redirect("iPxGLTransaction.aspx")
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('total credit and debit are not balanced !!');", True)
        End If
    End Sub

    'Protected Sub lbAddGroup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAddGroup.Click
    '    'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "date", "$(document).ready(function() {date()});", True)
    '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddGrp", "showModalAddGrp()", True)
    'End Sub

    Protected Sub lbAbortGrp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortGrp.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "tanggal", "$(document).ready(function() {tanggal()});", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddGrp", "hideModalAddGrp()", True)
    End Sub

    Protected Sub lbAddDetail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAddDetail.Click
        If tbReff.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter Reff No !!');", True)
            tbReff.Focus()
        ElseIf tbDate.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter Date !!');", True)
            tbDate.Focus()
        ElseIf dlGroup.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please select Group !!');", True)
            dlGroup.Focus()
        ElseIf tbCoaDtl.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter COA !!');", True)
            tbCoaDtl.Focus()
        ElseIf tbReffDtl.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter Reff No Detail !!');", True)
            tbReffDtl.Focus()
        Else
            If lbAddDetail.Text = "<i class='fa fa-plus'></i> Add" Then
                idGLDtl()
            Else
                RecID = Session("RecID")
            End If
            If oCnct.State = ConnectionState.Closed Then
                oCnct.Open()
            End If
            oSQLCmd = New SqlCommand(sSQL, oCnct)
            sSQL = "SELECT Coa FROM iPxAcct_Coa WHERE (businessid ='" & Session("sBusinessID") & "' or businessid='DF') and Coa ='" & tbCoaDtl.Text & "'"
            oSQLCmd.CommandText = sSQL
            oSQLReader = oSQLCmd.ExecuteReader

            If oSQLReader.Read Then
                oSQLReader.Close()
                If oCnct.State = ConnectionState.Closed Then
                    oCnct.Open()
                End If
                oSQLCmd = New SqlCommand(sSQL, oCnct)
                sSQL = "SELECT RecID FROM iPxAcctGL_JVdtl WHERE businessid ='" & Session("sBusinessID") & "' and TransID ='" & tbTransID.Text & "' and RecID = '" & RecID & "'"
                oSQLCmd.CommandText = sSQL
                oSQLReader = oSQLCmd.ExecuteReader

                If oSQLReader.Read Then
                    oSQLReader.Close()
                    updateGLDtl()
                    lbCancelAdd.Visible = False
                    lbAddDetail.Text = "<i class='fa fa-plus'></i> Add"
                Else
                    oSQLReader.Close()
                    cekGLHeader()
                End If
            Else
                oSQLReader.Close()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('COA is not registered !!');", True)
                tbCoaDtl.Focus()
            End If
            ListGLDetail()
            kosong()
        End If
        If tbDate.Enabled = False Then
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "tanggal", "$(document).ready(function() {tanggal()});", True)
        End If
    End Sub

    Protected Sub lbFindCoa_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbFindCoa.Click
        cekCoaCount()
        'tbFindCoaList.Text = tbCoaDtl.Text
        'Session("sFindCOA") = Replace(tbCoaDtl.Text, "'", "''")
        'ListCOA()
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalListCOA", "showModalListCOA();", True)
    End Sub

    Protected Sub lbFindListCoa_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbFindListCoa.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalListCOA", "hideModalListCOA();", True)
        'tbCoaDtl.Text = tbFindCoaList.Text
        Session("sFindCOA") = Replace(tbFindCoaList.Text, "'", "''")
        ListCOA()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalListCOA", "showModalListCOA();", True)
    End Sub

    Protected Sub lbAbortListCOA_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortListCOA.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalListCOA", "hideModalListCOA()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "tanggal", "$(document).ready(function() {tanggal()});", True)
    End Sub

    Protected Sub gvCoa_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvCoa.RowCommand
        If e.CommandName = "getSelect" Then
            tbCoaDtl.Text = e.CommandArgument
            selectCoa()
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalListCOA", "hideModalListCOA();", True)
    End Sub

    Protected Sub gvGLDetail_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvGLDetail.RowCommand
        If e.CommandName = "getEdit" Then
            Session("RecID") = e.CommandArgument
            EditGLDtl()
            lbAddDetail.Text = "<i class='fa fa-edit'></i> Update"
            lbCancelAdd.Visible = True
        ElseIf e.CommandName = "getDelete" Then
            Session("RecID") = e.CommandArgument
            DeleteGLDtl()
            ListGLDetail()
        End If
    End Sub

    Protected Sub lbCancelAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCancelAdd.Click
        tbCoaDtl.Text = ""
        tbReffDtl.Text = ""
        tbDebitDtl.Text = ""
        tbCreditDtl.Text = ""
        tbDescDtl.Text = ""
        lbCancelAdd.Visible = False
        lbAddDetail.Text = "<i class='fa fa-plus'></i> Add"
    End Sub

    Protected Sub lbSaveGrp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSaveGrp.Click

    End Sub

    Protected Sub lbAbort_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbort.Click
        If Session("sBalanceTot") = True Then
            Response.Redirect("iPxGLTransaction.aspx")
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('total credit and debit are not balanced !!');", True)
        End If
    End Sub

    Protected Sub lbRecaring_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbRecaring.Click
        tbSearchRecaring.Text = ""
        Session("sCondition") = ""
        Session("sQueryTicket") = ""
        ListGLRecaring()
        Session("sRecuring") = ""
        ListGLRecaringDetail()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalListRecaring", "showModalListRecaring();", True)
    End Sub

    Protected Sub lbAbortRecaring_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortRecaring.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalListRecaring", "hideModalListRecaring();", True)
    End Sub

    Protected Sub gvRecaring_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvRecaring.RowCommand
        If e.CommandName = "getSelect" Then
            Session("sRecuring") = e.CommandArgument
            If oCnct.State = ConnectionState.Closed Then
                oCnct.Open()
            End If
            oSQLCmd = New SqlCommand(sSQL, oCnct)
            sSQL = "SELECT TransID FROM iPxAcctGL_JVhdr WHERE TransID = '" & tbTransID.Text & "'"
            oSQLCmd.CommandText = sSQL
            oSQLReader = oSQLCmd.ExecuteReader

            If oSQLReader.Read Then
                oSQLReader.Close()
                cekGLDtlRecuring()
            Else
                oSQLReader.Close()
                saveByRecuringHdr()
                idGLDtl()
                saveByRecuringDtl()
            End If
            oCnct.Close()
            Session("sEditGL") = tbTransID.Text
            Call editGLHeader()
            Call ListGLDetail()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalListRecaring", "hideModalListRecaring();", True)
        ElseIf e.CommandName = "getDetail" Then
            Session("sRecuring") = e.CommandArgument
            ListGLRecaringDetail()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalListRecaring", "hideModalListRecaring();", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalListRecaring", "showModalListRecaring();", True)
        End If
    End Sub

    Protected Sub lbSearchRecaring_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSearchRecaring.Click
        Session("sCondition") = Session("sCondition") & " and (a.Description like '%" & Replace(tbSearchRecaring.Text, "'", "''") & "%') "
        ListGLRecaring()
        Session("sRecuring") = ""
        ListGLRecaringDetail()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalListRecaring", "hideModalListRecaring();", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalListRecaring", "showModalListRecaring();", True)
    End Sub

    Protected Sub lbImport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbImport.Click
        If tbReff.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter Reff No !!');", True)
            tbReff.Focus()
        ElseIf dlGroup.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please select Group !!');", True)
            dlGroup.Focus()
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalImport", "showModalImport();", True)
        End If
    End Sub

    Protected Sub lbAbortImport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortImport.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalImport", "hideModalImport();", True)
    End Sub

    Protected Sub lbStartImport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbStartImport.Click
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT TransID FROM iPxAcctGL_JVhdr WHERE TransID = '" & tbTransID.Text & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        If oSQLReader.Read Then
            oSQLReader.Close()
            UpdateGLHeader()
            SaveMassUpluad()
        Else
            oSQLReader.Close()
            saveGLHeader()
            SaveMassUpluad()
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('import successfull !!');", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalImport", "hideModalImport();", True)
    End Sub
End Class
