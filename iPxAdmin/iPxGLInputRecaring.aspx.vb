Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration
Partial Class iPxAdmin_iPxGLInputRecaring
    Inherits System.Web.UI.Page
    Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
    Dim oSQLCmd As SqlCommand
    Dim oSQLReader As SqlDataReader
    Dim sSQL, status, Close, active, RecID, MonthCekClosing, YearCekClosing As String
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
                lbAddDetail.Enabled = True
                lbAbortAdd.Enabled = True
                'lbAddGroup.Enabled = True
            Else
                lbAddDetail.Enabled = False
                lbAbortAdd.Enabled = False
                'lbAddGroup.Enabled = False
            End If
        Else
            lbAddDetail.Enabled = False
            lbAbortAdd.Enabled = False
            'lbAddGroup.Enabled = False
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
        sSQL = "SELECT Sum(Credit) as Credit, sum(Debit) as Debit FROM iPxAcctGL_JVdtlRec where businessid='" & Session("sBusinessID") & "' and TransID = '" & Replace(tbTransID.Text, "'", "''") & "'"
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
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT MAX(RecID) as RecID FROM iPxAcctGL_JVdtlRec where TransID = '" & Replace(tbTransID.Text, "'", "''") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            RecID = Val(oSQLReader.Item("RecID").ToString) + 1

        Else
            RecID = "1"
        End If
        oCnct.Close()
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
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid=a.businessid and x.usercode='" & Session("sUserCode") & "' and x.funtionid='24' and x.active='Y') as editGL "
        sSQL += "from iPxAcctGL_JVdtlRec as a INNER JOIN iPxAcctGL_JVhdrRec as b ON b.TransID=a.TransID "
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
        sSQL = "SELECT TransID FROM iPxAcctGL_JVhdrRec WHERE TransID = '" & tbTransID.Text & "'"
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
        sSQL = "SELECT TransID FROM iPxAcctGL_JVhdrRec WHERE TransID = '" & tbTransID.Text & "'"
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

        sSQL = "INSERT INTO iPxAcctGL_JVhdrRec(businessid, TransID, TransDate, Status, JVgroup, ReffNo, Description) "
        sSQL += "VALUES ('" & Session("sBusinessID") & "','" & Replace(tbTransID.Text, "'", "''") & "','" & GLDate & "', "
        sSQL += "'O','" & dlGroup.SelectedValue & "','" & Replace(tbReff.Text, "'", "''") & "','" & Replace(tbDesc.Text, "'", "''") & "') "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        Dim TransDateLog As String = Format(Now, "yyy-MM-dd hh:mm:ss")
        cIpx.saveLog(Session("sBusinessID"), Replace(tbTransID.Text, "'", "''"), TransDateLog, "GL RC", "Save", "Create New recaring journal " & Replace(tbTransID.Text, "'", "''"), "", Session("sUserCode"))

    End Sub
    Sub saveGLDtl()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "INSERT INTO iPxAcctGL_JVdtlRec(RecID, businessid, TransID, Coa, Description, Reff, Debit, Credit, isActive) "
        sSQL += "VALUES ('" & RecID & "','" & Session("sBusinessID") & "','" & Replace(tbTransID.Text, "'", "''") & "','" & Replace(tbCoaDtl.Text, "'", "''") & "', "
        sSQL += "'" & Replace(tbDescDtl.Text, "'", "''") & "','" & Replace(tbReffDtl.Text, "'", "''") & "','" & Replace(tbDebitDtl.Text, "'", "''") & "','" & Replace(tbCreditDtl.Text, "'", "''") & "', "
        sSQL += "'Y') "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        Dim TransDateLog As String = Format(Now, "yyy-MM-dd hh:mm:ss")
        cIpx.saveLog(Session("sBusinessID"), Replace(tbTransID.Text, "'", "''"), TransDateLog, "GL RC", "Save", "Create New recaring journal detail " & Replace(tbTransID.Text, "'", "''"), "", Session("sUserCode"))
    End Sub
    Sub editGLHeader()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT a.* FROM iPxAcctGL_JVhdrRec as a "
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
            'Close = oSQLReader.Item("CloseStatus").ToString
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
        sSQL = "SELECT a.*, (b.Description) as DescCoa FROM iPxAcctGL_JVdtlRec as a "
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
    Sub ListCOA()
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

        sSQL = "UPDATE iPxAcctGL_JVhdrRec SET  TransDate='" & GLDate & "', JVgroup='" & dlGroup.SelectedValue & "', ReffNo='" & Replace(tbReff.Text, "'", "''") & "', Description='" & Replace(tbDesc.Text, "'", "''") & "'"
        sSQL = sSQL & "WHERE businessid ='" & Session("sBusinessID") & "' and TransID ='" & tbTransID.Text & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        Dim TransDateLog As String = Format(Now, "yyy-MM-dd hh:mm:ss")
        cIpx.saveLog(Session("sBusinessID"), Replace(tbTransID.Text, "'", "''"), TransDateLog, "GL RC", "Update", "Update recaring journal " & Replace(tbTransID.Text, "'", "''"), "", Session("sUserCode"))
    End Sub
    Sub updateGLDtl()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)

        sSQL = "UPDATE iPxAcctGL_JVdtlRec SET  Coa='" & Replace(tbCoaDtl.Text, "'", "''") & "', Reff='" & Replace(tbReffDtl.Text, "'", "''") & "', Description='" & Replace(tbDescDtl.Text, "'", "''") & "'"
        sSQL += ",Debit='" & Replace(tbDebitDtl.Text, "'", "''") & "',Credit='" & Replace(tbCreditDtl.Text, "'", "''") & "'"
        sSQL += "WHERE businessid ='" & Session("sBusinessID") & "' and TransID ='" & tbTransID.Text & "' and RecID='" & RecID & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        Dim TransDateLog As String = Format(Now, "yyy-MM-dd hh:mm:ss")
        cIpx.saveLog(Session("sBusinessID"), Replace(tbTransID.Text, "'", "''"), TransDateLog, "GL RC", "Update", "Update recaring journal detail " & Replace(tbTransID.Text, "'", "''"), "", Session("sUserCode"))
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

        sSQL = "UPDATE iPxAcctGL_JVdtlRec SET isActive='N' "
        sSQL += "WHERE businessid ='" & Session("sBusinessID") & "' and TransID ='" & tbTransID.Text & "' and RecID='" & Session("RecID") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        Dim TransDateLog As String = Format(Now, "yyy-MM-dd hh:mm:ss")
        cIpx.saveLog(Session("sBusinessID"), Replace(tbTransID.Text, "'", "''"), TransDateLog, "GL RC", "Delete", "Delete recaring journal detail " & Session("RecID") & " of " & Replace(tbTransID.Text, "'", "''"), "", Session("sUserCode"))
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
        sSQL4 = "SELECT * FROM iPxAcctGL_JVhdrRec where businessid='" & Session("sBusinessID") & "' "
        sSQL4 += "and ReffNo like 'PL " & MonthCekClosing & "-" & YearCekClosing & "' and status<>'D'"
        oSQLCmd4.CommandText = sSQL4
        oSQLReader4 = oSQLCmd4.ExecuteReader

        oSQLReader4.Read()
        If oSQLReader4.HasRows Then
            lbAddDetail.Enabled = False
            lbAbortAdd.Enabled = False
        Else
            lbAddDetail.Enabled = True
            lbAbortAdd.Enabled = True
        End If
        oCnct4.Close()
    End Sub
    Protected Sub cari(ByVal sender As Object, ByVal e As EventArgs)
        MonthCekClosing = Strings.Mid(tbDate.Text, 4, 2)
        YearCekClosing = Strings.Right(tbDate.Text, 4)
        cekClose()
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            If Session("sEditGL") = "" Then
                tbDate.Text = Format(Now, "dd/MM/yyyy")
                tbTransID.Text = cIpx.GetCounterMBR("RJ", "RJ")
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
                If status = "O" Then
                    If Close = "Close" Then
                        lbAddDetail.Enabled = False
                        'lbAddGroup.Enabled = False
                        lbAbortAdd.Enabled = False
                    Else
                        lbAddDetail.Enabled = True
                        'lbAddGroup.Enabled = True
                        lbAbortAdd.Enabled = True
                    End If
                Else
                    lbAddDetail.Enabled = False
                    lbAbortAdd.Enabled = False
                End If
                lbAbortAdd.Text = "<i class='fa fa-save'></i> Update"
                UserAcces()
            End If
            lbAddDetail.Text = "<i class='fa fa-plus'></i> Add"
        End If
        totalDebcre()

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "tanggal", "$(document).ready(function() {tanggal()});", True)
    End Sub


    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvGLDetail.PageIndex = e.NewPageIndex
        Me.ListGLDetail()
        gvCoa.PageIndex = e.NewPageIndex
        Me.ListCOA()
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

    Protected Sub OnPageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvGLDetail.PageIndex = e.NewPageIndex
        Me.ListGLDetail()
        gvCoa.PageIndex = e.NewPageIndex
        Me.ListCOA()
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
            Response.Redirect("iPxGLRecaring.aspx")
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
        ElseIf tbDate.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter Date !!');", True)
        ElseIf dlGroup.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please select Group !!');", True)
        ElseIf tbReffDtl.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter Reff No Detail !!');", True)
        ElseIf tbCoaDtl.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter COA !!');", True)
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
                sSQL = "SELECT RecID FROM iPxAcctGL_JVdtlRec WHERE businessid ='" & Session("sBusinessID") & "' and TransID ='" & tbTransID.Text & "' and RecID = '" & RecID & "'"
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
        tbFindCoaList.Text = tbCoaDtl.Text
        Session("sFindCOA") = Replace(tbCoaDtl.Text, "'", "''")
        ListCOA()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalListCOA", "showModalListCOA()", True)
    End Sub

    Protected Sub lbFindListCoa_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbFindListCoa.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalListCOA", "hideModalListCOA()", True)
        'tbCoaDtl.Text = tbFindCoaList.Text
        Session("sFindCOA") = Replace(tbFindCoaList.Text, "'", "''")
        ListCOA()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalListCOA", "showModalListCOA()", True)
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
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalListCOA", "hideModalListCOA()", True)
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
            Response.Redirect("iPxGLRecaring.aspx")
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('total credit and debit are not balanced !!');", True)
        End If
    End Sub
End Class
