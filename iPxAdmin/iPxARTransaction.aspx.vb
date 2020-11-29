Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration
Partial Class iPxAdmin_iPxARTransaction
    Inherits System.Web.UI.Page
    Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
    Dim oSQLCmd As SqlCommand
    Dim oSQLReader As SqlDataReader
    Dim sSQL, RecID, month, year, ARCodeImport, totCust, totTranstype, MonthImport, YearImport, getImport, coaDebit, coaCredit As String
    Dim oCnct1 As SqlConnection = New SqlConnection(sCnct)
    Dim oSQLCmd1 As SqlCommand
    Dim oSQLReader1 As SqlDataReader
    Dim sSQL1 As String
    Dim oCnct2 As SqlConnection = New SqlConnection(sCnct)
    Dim oSQLCmd2 As SqlCommand
    Dim oSQLReader2 As SqlDataReader
    Dim sSQL2 As String
    Dim cIpx As New iPxClass
    Sub UserAcces()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.businessid, a.usercode, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.funtionid='1'and x.active='Y' and x.businessid=a.businessid and x.usercode=a.usercode) as AddAR, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.funtionid='4'and x.active='Y' and x.businessid=a.businessid and x.usercode=a.usercode) as import "
        sSQL += "from iPxAcct_profile_client_useraccess as a "
        sSQL += "INNER JOIN iPxAcct_profile_client_useraccess_dtl as b ON b.businessid=a.businessid and b.usercode=a.usercode "
        sSQL += "where a.businessid='" & Session("sBusinessID") & "' and a.usercode='" & Session("sUserCode") & "' and a.active='Y' group by a.businessid, a.usercode "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            If oSQLReader.Item("AddAR").ToString = "Y" Then
                lbAddAR.Enabled = True
            Else
                lbAddAR.Enabled = False
            End If
            If oSQLReader.Item("import").ToString = "Y" Then
                lbImportCC.Enabled = True
                lbImportCL.Enabled = True
            Else
                lbImportCC.Enabled = False
                lbImportCL.Enabled = False
            End If
        Else
            lbAddAR.Enabled = False
            lbImportCC.Enabled = False
            lbImportCL.Enabled = False
        End If
            oCnct.Close()
    End Sub
    Sub showdata_dropdownCustStatus()
        dlQStatus.Items.Insert(0, "")
        dlQStatus.Items.Insert(1, "New")
        dlQStatus.Items.Insert(2, "Posted")
        dlQStatus.Items.Insert(3, "Delete")
    End Sub
    Sub FOLinkImport()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select b.businessid, b.businessname "
        sSQL += "from iPxAcct_FOlink as a "
        sSQL += "INNER JOIN iPx_profile_client as b ON a.FoLink = b.businessid WHERE a.businessid='" & Session("sBusinessID") & "'"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                dlFOLinkImport.DataSource = dt
                dlFOLinkImport.DataTextField = "businessname"
                dlFOLinkImport.DataValueField = "businessid"
                dlFOLinkImport.DataBind()
                dlFOLinkImport.Items.Insert(0, "")
            End Using
        End Using
    End Sub
    Sub ListARHeader()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        month = Left(tbDate.Text, 2)
        year = Right(tbDate.Text, 4)
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT b.businessid, b.TransID, b.TransDate, b.Status, a.businessname AS FOLINK, b.Description, b.RegDate, b.RegBy, b.isActive, "
        sSQL += "(select sum(c.amountdr) from iPxAcctAR_Transaction as c where c.TransID=b.TransID) as amountTrans, "
        sSQL += "(select COUNT(invoiceno) from iPxAcctAR_Transaction as d where d.TransID=b.TransID and invoiceno<>'') as totInv, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid=b.businessid and x.usercode='" & Session("sUserCode") & "' and x.funtionid='5' and x.active='Y') as posting, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid=b.businessid and x.usercode='" & Session("sUserCode") & "' and x.funtionid='3' and x.active='Y') as deleteAR "
        sSQL += "FROM iPx_profile_client AS a RIGHT OUTER JOIN iPxAcctAR_TransHdr AS b ON a.businessid = b.FoLink COLLATE Latin1_General_CI_AS "
        sSQL += "WHERE b.businessid='" & Session("sBusinessID") & "' "
        If Session("sQueryTicket") = "" Then
            Session("sQueryTicket") = Session("sCondition")
            If Session("sQueryTicket") <> "" Or Session("sCondition") <> "" Then
                sSQL = sSQL & Session("sQueryTicket")
                Session("sCondition") = ""
            Else
                sSQL = sSQL & " AND month(b.TransDate)='" & month & "' AND year(b.TransDate)='" & year & "' AND b.Status<>'" & "X" & "'"
            End If
        Else
            sSQL = sSQL & Session("sQueryTicket")
            Session("sCondition") = ""
        End If
        sSQL += " order by b.TransDate asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvARTrans.DataSource = dt
                    gvARTrans.DataBind()
                    Dim totalAmount As Decimal
                    If dt.Compute("Sum(amountTrans)", "").ToString() <> "" Then
                        totalAmount = dt.Compute("Sum(amountTrans)", "").ToString()
                    Else
                        totalAmount = 0
                    End If
                    gvARTrans.FooterRow.Cells(3).Text = "Total"
                    gvARTrans.FooterRow.Cells(3).HorizontalAlign = HorizontalAlign.Right
                    gvARTrans.FooterRow.Cells(4).Text = totalAmount.ToString("N2")
                    gvARTrans.FooterRow.Cells(4).HorizontalAlign = HorizontalAlign.Right
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvARTrans.DataSource = dt
                    gvARTrans.DataBind()
                    gvARTrans.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub

    Public Function Posting(ByVal businessid As String, ByVal TransID As String) As Boolean

        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcctAR_TransHdr where businessid ='" & businessid & "' and TransID='" & TransID & "' "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader
        While oSQLReader.Read
            'oSQLReader.Close()
            Dim GLid As String = cIpx.GetCounterMBR("GL", "GL")
            saveGLHeader(GLid, businessid, TransID, oSQLReader.Item("TransDate"), oSQLReader.Item("Description"))
            GLDetail(businessid, TransID, GLid)
        End While
        oCnct.Close()

    End Function

    Public Function saveGLHeader(ByVal GLid As String, ByVal businessid As String, ByVal TransID As String, ByVal GLDate As String, ByVal GLDesc As String) As Boolean
        If oCnct1.State = ConnectionState.Closed Then
            oCnct1.Open()
        End If
        oSQLCmd1 = New SqlCommand(sSQL1, oCnct1)

        sSQL1 = "INSERT INTO iPxAcctGL_JVhdr(businessid, TransID, TransDate, Status, JVgroup, ReffNo, Description) "
        sSQL1 += "VALUES ('" & businessid & "','" & GLid & "','" & GLDate & "', "
        sSQL1 += "'O','G2','" & TransID & "','" & Replace(GLDesc, "'", "''") & "') "
        oSQLCmd1.CommandText = sSQL1
        oSQLCmd1.ExecuteNonQuery()

        oCnct1.Close()
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Posting header success !');document.getElementById('Buttonx').click()", True)
    End Function
    Public Function GLDetail(ByVal businessid As String, ByVal TransID As String, ByVal GLid As String) As Boolean

        If oCnct2.State = ConnectionState.Closed Then
            oCnct2.Open()
        End If
        oSQLCmd2 = New SqlCommand(sSQL2, oCnct2)
        sSQL2 = "SELECT a.RecID, b.CustomerID, b.CoyName, b.CoyName, b.CoaLink, c.Description, c.Coa, a.GuestName, a.amountdr "
        sSQL2 += "FROM iPxAcctAR_Transaction as a "
        sSQL2 += "INNER JOIN iPxAcctAR_Cfg_Customer as b ON a.businessid = b.businessid AND a.CustomerID = b.CustomerID "
        sSQL2 += "INNER JOIN iPxAcctAR_Cfg_Transactiontype as c ON a.businessid COLLATE SQL_Latin1_General_CP1_CI_AS = c.businessid AND a.transactiontype = c.TransactionType "
        sSQL2 += "where a.businessid ='" & businessid & "' and a.TransID='" & TransID & "' and a.isActive = 'Y' order by a.RecID asc "
        oSQLCmd2.CommandText = sSQL2
        oSQLReader2 = oSQLCmd2.ExecuteReader
        While oSQLReader2.Read
            saveGLDetailDebit(businessid, GLid, oSQLReader2.Item("CoaLink"), oSQLReader2.Item("CoyName") + " - " + oSQLReader2.Item("GuestName"), TransID, oSQLReader2.Item("amountdr"))
            saveGLDetailCredit(businessid, GLid, oSQLReader2.Item("Coa"), oSQLReader2.Item("CoyName") + " - " + oSQLReader2.Item("GuestName"), TransID, oSQLReader2.Item("amountdr"))
        End While
        oCnct2.Close()

    End Function
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
    Public Function saveGLDetailDebit(ByVal businessid As String, ByVal TransID As String, ByVal Coa As String, ByVal Desc As String, ByVal Reff As String, ByVal Amount As String) As Boolean
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
        sSQL3 += "'" & Replace(Desc, "'", "''") & "','" & Reff & "','" & Amount & "','0', "
        sSQL3 += "'Y') "
        oSQLCmd3.CommandText = sSQL3
        oSQLCmd3.ExecuteNonQuery()

        oCnct3.Close()
    End Function
    Public Function saveGLDetailCredit(ByVal businessid As String, ByVal TransID As String, ByVal Coa As String, ByVal Desc As String, ByVal Reff As String, ByVal Amount As String) As Boolean
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
        sSQL3 += "'" & Replace(Desc, "'", "''") & "','" & Reff & "','0','" & Amount & "', "
        sSQL3 += "'Y') "
        oSQLCmd3.CommandText = sSQL3
        oSQLCmd3.ExecuteNonQuery()

        oCnct3.Close()
    End Function
    Sub EditPosting()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "UPDATE iPxAcctAR_TransHdr SET Status='P'"
        sSQL = sSQL & "WHERE businessid ='" & Session("sBusinessID") & "' and TransID='" & Session("sTransID") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('the data was successfully posted !!');", True)
        ListARHeader()
    End Sub
    Sub deleteStatus()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "UPDATE iPxAcctAR_TransHdr SET Status='X'"
        sSQL = sSQL & "WHERE businessid ='" & Session("sBusinessID") & "' and TransID='" & Session("sTransID") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "UPDATE iPxAcctAR_Transaction SET isActive='N'"
        sSQL = sSQL & "WHERE businessid ='" & Session("sBusinessID") & "' and TransID='" & Session("sTransID") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        cIpx.updateLog(Session("sBusinessID"), Session("sTransID"))
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('the data was successfully delete !!');", True)
        ListARHeader()
    End Sub
    Sub FOLink()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select b.businessid, b.businessname "
        sSQL += "from iPxAcct_FOlink as a "
        sSQL += "INNER JOIN iPx_profile_client as b ON a.FoLink = b.businessid WHERE a.businessid='" & Session("sBusinessID") & "'"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                dlFOLink.DataSource = dt
                dlFOLink.DataTextField = "businessname"
                dlFOLink.DataValueField = "businessid"
                dlFOLink.DataBind()
                dlFOLink.Items.Insert(0, "")
            End Using
        End Using
    End Sub
    Sub ListFOLink()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT a.businessid, a.Description, a.FoLink, b.businessname "
        sSQL += "from iPxAcct_FOlink as a "
        sSQL += "INNER JOIN iPx_profile_client AS b ON a.FoLink = b.businessid "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "'"
        If Session("sQueryTicket") = "" Then
            Session("sQueryTicket") = Session("sCondition")
            If Session("sQueryTicket") <> "" Or Session("sCondition") <> "" Then
                sSQL = sSQL & Session("sQueryTicket")
                Session("sCondition") = ""
            Else
                sSQL = sSQL & ""
            End If
        Else
            sSQL = sSQL & Session("sQueryTicket")
            Session("sCondition") = ""
        End If
        sSQL += " order by a.Description asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvFO.DataSource = dt
                    gvFO.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvFO.DataSource = dt
                    gvFO.DataBind()
                    gvFO.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub FOLinkDatePosting()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select b.businessid, b.businessname "
        sSQL += "from iPxAcct_FOlink as a "
        sSQL += "INNER JOIN iPx_profile_client as b ON a.FoLink = b.businessid WHERE a.businessid='" & Session("sBusinessID") & "'"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                dlFOLinkDatePosting.DataSource = dt
                dlFOLinkDatePosting.DataTextField = "businessname"
                dlFOLinkDatePosting.DataValueField = "businessid"
                dlFOLinkDatePosting.DataBind()
            End Using
        End Using
    End Sub
    Sub tampilFO()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcctAR_TransHdr "
        sSQL += "WHERE TransID = '" & Session("sTransID") & "' AND businessid='" & Session("sBusinessID") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            Session("sFoLink") = oSQLReader.Item("FoLink").ToString
            oCnct.Close()
        Else
            oCnct.Close()
        End If
    End Sub
    Sub cekARDetailCL()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        Dim Transdate As Date
        Transdate = Date.ParseExact(tbDateImport.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT a.auditdate, c.CustomerID, 'NW' AS Type, '' AS Invoice, b.reffno AS voucherNo, a.checkno, a.rsvpno, "
        sSQL += "(b.lastname + b.firstname) AS GuestName, b.roomno, b.arrival, b.departure, b.clerknotes, "
        sSQL += "(a.amount * - 1) AS AmountDR, 0 AS amountCr, (a.amount * - 1) AS AmountDRFr, 0 AS amountCrFr, 'IDR' AS Currency, 'FLINK' AS Expr1, 'Y' AS isActive "
        sSQL += "FROM dbo.iPxPMS_postingrecord as a "
        sSQL += "INNER JOIN dbo.iPxPMS_reservation as b ON a.businessid = b.businessid AND a.rsvpno = b.rsvpno  AND     a.rsvpid = b.id "
        sSQL += "LEFT JOIN dbo.iPxAcctAR_Cfg_FOmapping as c ON b.businessid = c.Folink AND b.companycode = c.profilecode "
        sSQL += "WHERE (a.businessid = '" & dlFOLinkImport.SelectedValue & "') AND (a.poscode = 'SYPY') AND (a.auditdate = '" & Transdate & "') AND (a.paymenttype = 'CL') "
        sSQL += " UNION ALL "
        sSQL += "SELECT a.auditdate, c.CustomerID, 'NW' AS Type, '' AS Invoice, b.reffno AS voucherNo, a.checkno, a.rsvpno, "
        sSQL += "(b.lastname + b.firstname) AS GuestName, b.roomno, b.arrival, b.departure, b.clerknotes, "
        sSQL += "(a.amount * - 1) AS AmountDR, 0 AS amountCr, (a.amount * - 1) AS AmountDRFr, 0 AS amountCrFr, 'IDR' AS Currency, 'FLINK' AS Expr1, 'Y' AS isActive "
        sSQL += "FROM dbo.iPxPMS_postingrecord as a "
        sSQL += "INNER JOIN dbo.iPxPMS_reservationhis as b ON a.businessid = b.businessid AND a.rsvpno = b.rsvpno  AND     a.rsvpid = b.regno "
        sSQL += "LEFT JOIN dbo.iPxAcctAR_Cfg_FOmapping as c ON b.businessid = c.Folink AND b.companycode = c.profilecode "
        sSQL += "WHERE (a.businessid = '" & dlFOLinkImport.SelectedValue & "') AND (a.poscode = 'SYPY') AND (a.auditdate = '" & Transdate & "') AND (a.paymenttype = 'CL') "
        'sSQL += "ORDER BY dbo.iPxPMS_postingrecord.takendate DESC "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader
        oSQLReader.Read()
        If oSQLReader.HasRows Then
            oCnct.Close()
            saveARTransHeader("Auto AR Transaction")
            saveGrpARCL()
            ListARHeader()
            cIpx.saveLog(Session("sBusinessID"), ARCodeImport, Transdate, "AR CL", "Import", "Import AR City Ledger dari " & dlFOLinkImport.SelectedItem.ToString, dlFOLinkImport.SelectedValue, Session("sUserCode"))
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('import successful !');document.getElementById('Buttonx').click()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('no data can be imported !');document.getElementById('Buttonx').click()", True)
            oCnct.Close()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "datetimepicker()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
        End If
    End Sub
    Sub cekARDetailCC()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        Dim Transdate As Date
        Transdate = Date.ParseExact(tbDateImport.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT a.auditdate, c.CustomerID, 'NW' AS Type, '' AS Invoice, b.reffno AS voucherNo, a.checkno, a.rsvpno, "
        sSQL += "b.lastname + b.firstname AS GuestName, b.roomno, b.arrival, b.departure, b.clerknotes, a.amount * - 1 AS AmountDR, "
        sSQL += "0 AS amountCr,  a.amount * - 1 AS AmountDRFr, 0 AS amountCrFr, 'IDR' AS Currency, 'FLINK' AS Expr1, 'Y' AS isActive "
        sSQL += "FROM dbo.iPxPMS_postingrecord as a "
        sSQL += "INNER JOIN dbo.iPxPMS_reservation as b ON a.businessid = b.businessid AND a.rsvpno = b.rsvpno AND a.rsvpid = b.id "
        sSQL += "LEFT OUTER JOIN dbo.iPxAcctAR_Cfg_FOmapping as c ON a.cardtype = c.profilecode "
        sSQL += "WHERE        (a.businessid = '" & dlFOLinkImport.SelectedValue & "') AND (a.auditdate = '" & Transdate & "') AND (a.paymenttype = 'CR') and c.businessid='" & Session("sBusinessID") & "'"
        sSQL += "UNION ALL "
        sSQL += "SELECT a.auditdate, c.CustomerID, 'NW' AS Type, '' AS Invoice, b.reffno AS voucherNo, a.checkno, a.rsvpno, "
        sSQL += "b.lastname + b.firstname AS GuestName, b.roomno, b.arrival, b.departure, b.clerknotes, a.amount * - 1 AS AmountDR, "
        sSQL += "0 AS amountCr,  a.amount * - 1 AS AmountDRFr, 0 AS amountCrFr, 'IDR' AS Currency, 'FLINK' AS Expr1, 'Y' AS isActive "
        sSQL += "FROM dbo.iPxPMS_postingrecord as a "
        sSQL += "INNER JOIN dbo.iPxPMS_reservationhis as b ON a.businessid = b.businessid AND a.rsvpno = b.rsvpno AND a.rsvpid = b.regno "
        sSQL += "LEFT OUTER JOIN dbo.iPxAcctAR_Cfg_FOmapping as c ON a.cardtype = c.profilecode "
        sSQL += "WHERE        (a.businessid = '" & dlFOLinkImport.SelectedValue & "') AND (a.auditdate = '" & Transdate & "') AND (a.paymenttype = 'CR') and c.businessid='" & Session("sBusinessID") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader
        oSQLReader.Read()
        If oSQLReader.HasRows Then
            oCnct.Close()
            saveARTransHeader("Auto Credit Card Transaction")
            saveGrpARCC()
            ListARHeader()
            cIpx.saveLog(Session("sBusinessID"), ARCodeImport, Transdate, "AR CC", "Import", "Import AR Credit Card dari " & dlFOLinkImport.SelectedItem.ToString, dlFOLinkImport.SelectedValue, Session("sUserCode"))
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('import successful !');document.getElementById('Buttonx').click()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('no data can be imported !');document.getElementById('Buttonx').click()", True)
            oCnct.Close()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "datetimepicker()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
        End If
    End Sub
    Public Function saveARTransHeader(ByVal Desc As String) As Boolean
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        Dim regDate As Date = Date.Now()
        Dim range As String = Format(Now, "dd") - 1
        Dim Transdate As Date
        Transdate = Date.ParseExact(tbDateImport.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)

        sSQL = "INSERT INTO iPxAcctAR_TransHdr (businessid,TransID,Status,TransDate,FoLink,Description,RegDate,RegBy,isActive) "
        sSQL += "VALUES ('" & Session("sBusinessID") & "','" & ARCodeImport & "','N','" & Transdate & "','" & dlFOLinkImport.SelectedValue & "','" & Desc & "'"
        sSQL += ",'" & regDate & "','" & Session("iUserID") & "','Y') "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()

    End Function
    Sub saveGrpARCL()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        Dim Transdate As Date
        Transdate = Date.ParseExact(tbDateImport.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT a.auditdate, c.CustomerID, 'NW' AS Type, '' AS Invoice, b.reffno AS voucherNo, a.checkno, a.rsvpno, "
        sSQL += "(b.lastname + b.firstname) AS GuestName, b.roomno, b.arrival, b.departure, b.clerknotes, "
        sSQL += "(a.amount * - 1) AS AmountDR, 0 AS amountCr, (a.amount * - 1) AS AmountDRFr, 0 AS amountCrFr, 'IDR' AS Currency, 'FLINK' AS Expr1, 'Y' AS isActive "
        sSQL += "FROM dbo.iPxPMS_postingrecord as a "
        sSQL += "INNER JOIN dbo.iPxPMS_reservation as b ON a.businessid = b.businessid AND a.rsvpno = b.rsvpno  AND     a.rsvpid = b.id "
        sSQL += "LEFT JOIN dbo.iPxAcctAR_Cfg_FOmapping as c ON b.businessid = c.Folink AND b.companycode = c.profilecode "
        sSQL += "WHERE (a.businessid = '" & dlFOLinkImport.SelectedValue & "') AND (a.poscode = 'SYPY') AND (a.auditdate = '" & Transdate & "') AND (a.paymenttype = 'CL') "
        sSQL += " UNION ALL "
        sSQL += "SELECT a.auditdate, c.CustomerID, 'NW' AS Type, '' AS Invoice, b.reffno AS voucherNo, a.checkno, a.rsvpno, "
        sSQL += "(b.lastname + b.firstname) AS GuestName, b.roomno, b.arrival, b.departure, b.clerknotes, "
        sSQL += "(a.amount * - 1) AS AmountDR, 0 AS amountCr, (a.amount * - 1) AS AmountDRFr, 0 AS amountCrFr, 'IDR' AS Currency, 'FLINK' AS Expr1, 'Y' AS isActive "
        sSQL += "FROM dbo.iPxPMS_postingrecord as a "
        sSQL += "INNER JOIN dbo.iPxPMS_reservationhis as b ON a.businessid = b.businessid AND a.rsvpno = b.rsvpno  AND     a.rsvpid = b.regno "
        sSQL += "LEFT JOIN dbo.iPxAcctAR_Cfg_FOmapping as c ON b.businessid = c.Folink AND b.companycode = c.profilecode "
        sSQL += "WHERE (a.businessid = '" & dlFOLinkImport.SelectedValue & "') AND (a.poscode = 'SYPY') AND (a.auditdate = '" & Transdate & "') AND (a.paymenttype = 'CL') "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader
        While oSQLReader.Read
            'oSQLReader.Close()
            saveARDetail(Session("sBusinessID"), ARCodeImport, oSQLReader.Item("auditdate"), oSQLReader.Item("CustomerID").ToString, oSQLReader.Item("Type"), oSQLReader.Item("Invoice"), oSQLReader.Item("voucherNo"), oSQLReader.Item("checkno"), oSQLReader.Item("rsvpno"), oSQLReader.Item("GuestName"), oSQLReader.Item("roomno"), oSQLReader.Item("arrival"), oSQLReader.Item("departure"), oSQLReader.Item("clerknotes"), oSQLReader.Item("AmountDR"), oSQLReader.Item("amountCr"), oSQLReader.Item("AmountDRFr"), oSQLReader.Item("amountCrFr"), oSQLReader.Item("Currency"), dlFOLinkImport.SelectedValue)
        End While
        oCnct.Close()
    End Sub
    Sub saveGrpARCC()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        Dim Transdate As Date
        Transdate = Date.ParseExact(tbDateImport.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT a.auditdate, c.CustomerID, 'NW' AS Type, '' AS Invoice, b.reffno AS voucherNo, a.checkno, a.rsvpno, "
        sSQL += "b.lastname + b.firstname AS GuestName, b.roomno, b.arrival, b.departure, b.clerknotes, a.amount * - 1 AS AmountDR, "
        sSQL += "0 AS amountCr,  a.amount * - 1 AS AmountDRFr, 0 AS amountCrFr, 'IDR' AS Currency, 'FLINK' AS Expr1, 'Y' AS isActive "
        sSQL += "FROM dbo.iPxPMS_postingrecord as a "
        sSQL += "INNER JOIN dbo.iPxPMS_reservation as b ON a.businessid = b.businessid AND a.rsvpno = b.rsvpno AND a.rsvpid = b.id "
        sSQL += "LEFT OUTER JOIN dbo.iPxAcctAR_Cfg_FOmapping as c ON a.cardtype = c.profilecode "
        sSQL += "WHERE        (a.businessid = '" & dlFOLinkImport.SelectedValue & "') AND (a.auditdate = '" & Transdate & "') AND (a.paymenttype = 'CR') and c.businessid='" & Session("sBusinessID") & "'"
        sSQL += "UNION ALL "
        sSQL = "SELECT a.auditdate, c.CustomerID, 'NW' AS Type, '' AS Invoice, b.reffno AS voucherNo, a.checkno, a.rsvpno, "
        sSQL += "b.lastname + b.firstname AS GuestName, b.roomno, b.arrival, b.departure, b.clerknotes, a.amount * - 1 AS AmountDR, "
        sSQL += "0 AS amountCr,  a.amount * - 1 AS AmountDRFr, 0 AS amountCrFr, 'IDR' AS Currency, 'FLINK' AS Expr1, 'Y' AS isActive "
        sSQL += "FROM dbo.iPxPMS_postingrecord as a "
        sSQL += "INNER JOIN dbo.iPxPMS_reservationhis as b ON a.businessid = b.businessid AND a.rsvpno = b.rsvpno AND a.rsvpid = b.regno "
        sSQL += "LEFT OUTER JOIN dbo.iPxAcctAR_Cfg_FOmapping as c ON a.cardtype = c.profilecode "
        sSQL += "WHERE        (a.businessid = '" & dlFOLinkImport.SelectedValue & "') AND (a.auditdate = '" & Transdate & "') AND (a.paymenttype = 'CR') and c.businessid='" & Session("sBusinessID") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader
        While oSQLReader.Read
            'oSQLReader.Close()
            saveARDetail(Session("sBusinessID"), ARCodeImport, oSQLReader.Item("auditdate"), oSQLReader.Item("CustomerID").ToString, oSQLReader.Item("Type"), oSQLReader.Item("Invoice"), oSQLReader.Item("voucherNo"), oSQLReader.Item("checkno"), oSQLReader.Item("rsvpno"), oSQLReader.Item("GuestName"), oSQLReader.Item("roomno"), oSQLReader.Item("arrival"), oSQLReader.Item("departure"), oSQLReader.Item("clerknotes"), oSQLReader.Item("AmountDR"), oSQLReader.Item("amountCr"), oSQLReader.Item("AmountDRFr"), oSQLReader.Item("amountCrFr"), oSQLReader.Item("Currency"), dlFOLinkImport.SelectedValue)
        End While
        oCnct.Close()
    End Sub
    Public Function saveARDetail(ByVal businessid As String, ByVal TransID As String, ByVal TransDate As String, ByVal CustomerID As String, ByVal Type As String, ByVal InvoiceNo As String, ByVal voucherNo As String, ByVal fulioNo As String, ByVal rsvpNo As String, ByVal GuestName As String, ByVal RoomNo As String, ByVal arrival As String, ByVal departure As String, ByVal notes As String, ByVal AmountDr As String, ByVal AmountCr As String, ByVal AmountDrFr As String, ByVal AmountCrFr As String, ByVal currency As String, ByVal businessidfo As String) As Boolean
        Dim oCnct1 As SqlConnection = New SqlConnection(sCnct)
        Dim oSQLCmd1 As SqlCommand
        If oCnct1.State = ConnectionState.Closed Then
            oCnct1.Open()
        End If
        oSQLCmd1 = New SqlCommand(sSQL, oCnct1)
        sSQL = "INSERT INTO iPxAcctAR_Transaction(businessid,TransID,TransDate,CustomerID,transactiontype,invoiceno,voucherno, "
        sSQL += "foliono,RsvdNo,GuestName,RoomNo,arrival,departure,notes,amountdr,amountcr,amountdr_frgn, amountcr_frgn, currency, FoLink, isActive) "
        sSQL += "VALUES ('" & businessid & "','" & TransID & "','" & TransDate & "','" & CustomerID & "','" & Type & "'"
        sSQL += ",'" & InvoiceNo & "','" & voucherNo & "','" & fulioNo & "','" & rsvpNo & "','" & GuestName & "','" & RoomNo & "'"
        sSQL += ",'" & arrival & "','" & departure & "','" & notes & "','" & AmountDr & "','" & AmountCr & "','" & AmountDrFr & "','" & AmountCrFr & "','" & currency & "','" & businessidfo & "','Y') "
        oSQLCmd1.CommandText = sSQL
        oSQLCmd1.ExecuteNonQuery()
        oCnct1.Close()
    End Function
    Sub cekCustDetailAR()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        Dim Transdate As Date
        Transdate = Date.ParseExact(tbDateImport.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT b.businessid, b.TransID, "
        sSQL += "(select COUNT(e.CustomerID) from iPxAcctAR_Transaction as e where e.TransID=b.TransID and e.CustomerID='') as TotCust, "
        sSQL += "(select COUNT(f.transactiontype) from iPxAcctAR_Transaction as f where f.TransID=b.TransID and f.transactiontype='') as totPay "
        sSQL += "FROM iPx_profile_client AS a RIGHT OUTER JOIN iPxAcctAR_TransHdr AS b ON a.businessid = b.FoLink COLLATE Latin1_General_CI_AS "
        sSQL += "WHERE b.businessid='" & Session("sBusinessID") & "'  AND b.TransID='08' "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader
        While oSQLReader.Read
            'oSQLReader.Close()
            saveARDetail(Session("sBusinessID"), ARCodeImport, oSQLReader.Item("auditdate"), oSQLReader.Item("CustomerID").ToString, oSQLReader.Item("Type"), oSQLReader.Item("Invoice"), oSQLReader.Item("voucherNo"), oSQLReader.Item("checkno"), oSQLReader.Item("rsvpno"), oSQLReader.Item("GuestName"), oSQLReader.Item("roomno"), oSQLReader.Item("arrival"), oSQLReader.Item("departure"), oSQLReader.Item("clerknotes"), oSQLReader.Item("AmountDR"), oSQLReader.Item("amountCr"), oSQLReader.Item("AmountDRFr"), oSQLReader.Item("amountCrFr"), oSQLReader.Item("Currency"), dlFOLinkImport.SelectedValue)
        End While
        oCnct.Close()
    End Sub
    Public Function cekImport(ByVal arGroup As String) As Boolean
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        Dim Transdate As Date
        Transdate = Date.ParseExact(tbDateImport.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select * from iPxAcctAR_Log_Import where businessid='" & Session("sBusinessID") & "' and "
        sSQL += "pmsID='" & dlFOLinkImport.SelectedValue & "' and importDate='" & Transdate & "' and arGroup='" & arGroup & "' and isActive='Y' "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader
        oSQLReader.Read()
        If oSQLReader.HasRows Then
            oCnct.Close()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('import failed, data is duplicated !');document.getElementById('Buttonx').click()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
        Else
            oCnct.Close()
            If arGroup = "CC" Then
                cekARDetailCC()
            Else
                cekARDetailCL()
            End If
        End If
    End Function
    Public Function saveLogImport(ByVal arGroup As String, ByVal dateImport As String, ByVal foImport As String) As Boolean
        Dim oCnct3 As SqlConnection = New SqlConnection(sCnct)
        Dim oSQLCmd3 As SqlCommand
        Dim sSQL3 As String
        If oCnct3.State = ConnectionState.Closed Then
            oCnct3.Open()
        End If
        oSQLCmd3 = New SqlCommand(sSQL3, oCnct3)
        sSQL3 = "INSERT INTO iPxAcctAR_Log_Import(businessid,id,importDate,arGroup,pmsID,userid,isActive) "
        sSQL3 += "VALUES ('" & Session("sBusinessID") & "','" & ARCodeImport & "','" & dateImport & "','" & arGroup & "','" & foImport & "', "
        sSQL3 += "'" & Session("iUserID") & "','Y') "
        oSQLCmd3.CommandText = sSQL3
        oSQLCmd3.ExecuteNonQuery()

        oCnct3.Close()
    End Function
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
        sSQL4 += "and ReffNo like 'PL " & MonthImport & "-" & YearImport & "' and status<>'D'"
        oSQLCmd4.CommandText = sSQL4
        oSQLReader4 = oSQLCmd4.ExecuteReader

        oSQLReader4.Read()
        If oSQLReader4.HasRows Then
            lbStartImportCC.Enabled = False
            lbStartImportCL.Enabled = False
        Else
            lbStartImportCC.Enabled = True
            lbStartImportCL.Enabled = True
        End If
        oCnct4.Close()
    End Sub
    Sub cekCloseGrid()
        Dim oCnct4 As SqlConnection = New SqlConnection(sCnct)
        Dim oSQLReader4 As SqlDataReader
        Dim oSQLCmd4 As SqlCommand
        Dim sSQL4 As String
        If oCnct4.State = ConnectionState.Closed Then
            oCnct4.Open()
        End If
        oSQLCmd4 = New SqlCommand(sSQL4, oCnct4)
        sSQL4 = "SELECT * FROM iPxAcctGL_JVhdr where businessid='" & Session("sBusinessID") & "' "
        sSQL4 += "and ReffNo like 'PL " & MonthImport & "-" & YearImport & "' and status<>'D'"
        oSQLCmd4.CommandText = sSQL4
        oSQLReader4 = oSQLCmd4.ExecuteReader

        oSQLReader4.Read()
        If oSQLReader4.HasRows Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Posting process failed, this periode is closed !');document.getElementById('Buttonx').click()", True)
        Else
            Posting(Session("sBusinessID"), Session("sTransID"))
            EditPosting()
            Dim TransDateLog As String = Format(Now, "yyy-MM-dd hh:mm:ss")
            cIpx.saveLog(Session("sBusinessID"), Session("sTransID"), TransDateLog, "AR TR", "Posting", "Posting AR Transaction", "", Session("sUserCode"))
        End If
        oCnct4.Close()
    End Sub
    Protected Sub cari(ByVal sender As Object, ByVal e As EventArgs)
        Dim Transdate As Date
        Transdate = Date.ParseExact(tbDateImport.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
        getImport = cIpx.getLogImport(Session("sBusinessID"), Transdate, "AR CC")
        If getImport = "Y" Then
            lbStartImportCC.Enabled = False
            lbStartImportCL.Enabled = False
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "datetimepicker()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
        Else
            MonthImport = Strings.Mid(tbDateImport.Text, 4, 2)
            YearImport = Strings.Right(tbDateImport.Text, 4)
            cekClose()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "datetimepicker()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
        End If
    End Sub
    Sub ListDatePost()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        Dim i As Integer = 2
        Dim dateBirthday As Date = "1 " + tbDatePost.Text
        Dim j As Integer = dateBirthday.ToString("MM")
        year = dateBirthday.ToString("yyy")
        Dim x As Integer = Date.DaysInMonth(year, j)
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select 1 as numb, "
        sSQL += "(SELECT isActive FROM iPxAcct_Log WHERE businessid =x.businessid and transDate = '" & year & "-" & j & "-1 00:00:00' and Grp='AR CC' and funct='import' and isActive='Y') as PostCC, "
        sSQL += "(select sum(z.cc) from (SELECT count(a.auditdate) as 'cc' FROM dbo.iPxPMS_postingrecord as a "
        sSQL += "LEFT OUTER JOIN dbo.iPxAcctAR_Cfg_FOmapping as c ON a.cardtype = c.profilecode "
        sSQL += "WHERE        (a.businessid = '" & dlFOLinkDatePosting.SelectedValue & "') AND (a.auditdate = '" & j & "/1/" & year & "') AND (a.paymenttype = 'CR') and c.businessid=x.businessid "
        sSQL += "UNION ALL "
        sSQL += "SELECT count(a.auditdate) as 'cc' FROM dbo.iPxPMS_postingrecord as a "
        sSQL += "LEFT OUTER JOIN dbo.iPxAcctAR_Cfg_FOmapping as c ON a.cardtype = c.profilecode "
        sSQL += "WHERE        (a.businessid = '" & dlFOLinkDatePosting.SelectedValue & "') AND (a.auditdate = '" & j & "/1/" & year & "') AND (a.paymenttype = 'CR') and c.businessid=x.businessid) as z) as dataCC, "
        sSQL += "(SELECT isActive FROM iPxAcct_Log WHERE businessid =x.businessid and transDate = '" & year & "-" & j & "-1 00:00:00' and Grp='AR CL' and funct='import' and isActive='Y') as PostCL, "
        sSQL += "(select sum(z.cc) from (SELECT count(a.auditdate) as 'cc' FROM dbo.iPxPMS_postingrecord as a "
        sSQL += "LEFT OUTER JOIN dbo.iPxAcctAR_Cfg_FOmapping as c ON a.cardtype = c.profilecode "
        sSQL += "WHERE        (a.businessid = '" & dlFOLinkDatePosting.SelectedValue & "') AND (a.auditdate = '" & j & "/1/" & year & "') AND (a.paymenttype = 'CL') and c.businessid=x.businessid "
        sSQL += "UNION ALL "
        sSQL += "SELECT count(a.auditdate) as 'cc' FROM dbo.iPxPMS_postingrecord as a "
        sSQL += "LEFT OUTER JOIN dbo.iPxAcctAR_Cfg_FOmapping as c ON a.cardtype = c.profilecode "
        sSQL += "WHERE        (a.businessid = '" & dlFOLinkDatePosting.SelectedValue & "') AND (a.auditdate = '" & j & "/1/" & year & "') AND (a.paymenttype = 'CL') and c.businessid=x.businessid) as z) as dataCL "
        sSQL += "from iPx_profile_client as x where businessid='" & Session("sBusinessID") & "' "
        Do Until (i > x)
            sSQL += "UNION ALL "
            sSQL += "select " & i & " as numb, "
            sSQL += "(SELECT isActive FROM iPxAcct_Log WHERE businessid =x.businessid and transDate = '" & year & "-" & j & "-" & i & " 00:00:00' and Grp='AR CC' and funct='import' and isActive='Y') as PostCC, "
            sSQL += "(select sum(z.cc) from (SELECT count(a.auditdate) as 'cc' FROM dbo.iPxPMS_postingrecord as a "
            sSQL += "LEFT OUTER JOIN dbo.iPxAcctAR_Cfg_FOmapping as c ON a.cardtype = c.profilecode "
            sSQL += "WHERE        (a.businessid = '" & dlFOLinkDatePosting.SelectedValue & "') AND (a.auditdate = '" & j & "/" & i & "/" & year & "') AND (a.paymenttype = 'CR') and c.businessid=x.businessid "
            sSQL += "UNION ALL "
            sSQL += "SELECT count(a.auditdate) as 'cc' FROM dbo.iPxPMS_postingrecord as a "
            sSQL += "LEFT OUTER JOIN dbo.iPxAcctAR_Cfg_FOmapping as c ON a.cardtype = c.profilecode "
            sSQL += "WHERE        (a.businessid = '" & dlFOLinkDatePosting.SelectedValue & "') AND (a.auditdate = '" & j & "/" & i & "/" & year & "') AND (a.paymenttype = 'CR') and c.businessid=x.businessid) as z) as dataCC, "
            sSQL += "(SELECT isActive FROM iPxAcct_Log WHERE businessid =x.businessid and transDate = '" & year & "-" & j & "-" & i & " 00:00:00' and Grp='AR CL' and funct='import' and isActive='Y') as PostCL, "
            sSQL += "(select sum(z.cc) from (SELECT count(a.auditdate) as 'cc' FROM dbo.iPxPMS_postingrecord as a "
            sSQL += "LEFT OUTER JOIN dbo.iPxAcctAR_Cfg_FOmapping as c ON a.cardtype = c.profilecode "
            sSQL += "WHERE        (a.businessid = '" & dlFOLinkDatePosting.SelectedValue & "') AND (a.auditdate = '" & j & "/" & i & "/" & year & "') AND (a.paymenttype = 'CL') and c.businessid=x.businessid "
            sSQL += "UNION ALL "
            sSQL += "SELECT count(a.auditdate) as 'cc' FROM dbo.iPxPMS_postingrecord as a "
            sSQL += "LEFT OUTER JOIN dbo.iPxAcctAR_Cfg_FOmapping as c ON a.cardtype = c.profilecode "
            sSQL += "WHERE        (a.businessid = '" & dlFOLinkDatePosting.SelectedValue & "') AND (a.auditdate = '" & j & "/" & i & "/" & year & "') AND (a.paymenttype = 'CL') and c.businessid=x.businessid) as z) as dataCL "
            sSQL += "from iPx_profile_client as x where businessid='" & Session("sBusinessID") & "' "
            i += 1
        Loop
        sSQL += "ORDER BY numb asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvDatePost.DataSource = dt
                    gvDatePost.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvDatePost.DataSource = dt
                    gvDatePost.DataBind()
                    gvDatePost.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("sBusinessID") = "" Then
            Session("sDateWorkAR") = ""
            Response.Redirect("signin.aspx")
        End If
        If Not Me.IsPostBack Then
            If cIpx.getAccessUser(Session("sBusinessID"), Session("sUserCode"), "AR Transaction") <> True Then

                Session("sMessage") = "Sorry, you dont have access in this module |"
                Session("sMemberid") = ""
                Session("sWarningID") = "0"
                Session("sUrlOKONLY") = "home.aspx"
                Session("sUrlYES") = "http://www.thepyxis.net"
                Session("sUrlNO") = "http://www.thepyxis.net"
                Response.Redirect("warningmsg.aspx")
            End If
            UserAcces()
            Session("sQueryTicket") = ""
            If Session("sDateWorkAR") = "" Then
                tbDate.Text = Format(Now, "MM-yyyy")
            Else
                tbDate.Text = Session("sDateWorkAR")
            End If
            ListARHeader()
        Else
            ListARHeader()
        End If
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MonthGL", "$(document).ready(function() {MonthGL()});", True)
    End Sub
    Protected Sub TransdateWork(ByVal sender As Object, ByVal e As EventArgs)
        Session("sDateWorkAR") = tbDate.Text
        Session("sQueryTicket") = ""
    End Sub
    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvARTrans.PageIndex = e.NewPageIndex
        Me.ListARHeader()
    End Sub

    Protected Sub gvARTrans_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvARTrans.PageIndexChanging
        gvARTrans.PageIndex = e.NewPageIndex
        Me.ListARHeader()
    End Sub

    Protected Sub OnPageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvARTrans.PageIndex = e.NewPageIndex
        Me.ListARHeader()
    End Sub
    Protected Sub gvDatePost_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDatePost.PageIndexChanging
        gvDatePost.PageIndex = e.NewPageIndex
        Me.ListDatePost()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalDatePost", "hideModalDatePost()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDatePost", "showModalDatePost()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MonthPost", "$(document).ready(function() {MonthPost()});", True)
    End Sub

    Protected Sub lbAddAR_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAddAR.Click
        Session("sTransID") = ""
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SetTarget", "SetTarget();", True)
        Response.Redirect("iPxInputARTransaction.aspx")
    End Sub
    Protected Sub StatusChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim ddlStatus As DropDownList = DirectCast(sender, DropDownList)
        ViewState("Filter") = ddlStatus.SelectedValue
        Me.ListARHeader()
    End Sub
    Protected Sub gvARTrans_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvARTrans.RowCommand
        If e.CommandName = "getEdit" Then
            Session("sTransID") = e.CommandArgument
            Response.Redirect("iPxInputARTransaction.aspx")
        ElseIf e.CommandName = "getDelete" Then
            Session("sTransID") = e.CommandArgument
            tbReason.Text = ""
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDeleteTrans", "showModalDeleteTrans()", True)
        ElseIf e.CommandName = "getPost" Then
            Session("sTransID") = e.CommandArgument
            If oCnct.State = ConnectionState.Closed Then
                oCnct.Open()
            End If
            oSQLCmd = New SqlCommand(sSQL, oCnct)
            sSQL = "SELECT b.businessid, b.TransID,transDate, "
            sSQL += "(select COUNT(e.CustomerID) from iPxAcctAR_Transaction as e where e.TransID=b.TransID and e.CustomerID='') as TotCust, "
            sSQL += "(select COUNT(f.transactiontype) from iPxAcctAR_Transaction as f where f.TransID=b.TransID and f.transactiontype='') as totPay, "
            sSQL += "(SELECT COUNT(b.CoaLink) FROM iPxAcctAR_Transaction as a "
            sSQL += "INNER JOIN iPxAcctAR_Cfg_Customer as b ON a.businessid = b.businessid AND a.CustomerID = b.CustomerID "
            sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and a.TransID='" & Session("sTransID") & "' and a.isActive = 'Y' and b.CoaLink='') as coaCredit, "
            sSQL += "(SELECT COUNT(c.Coa) FROM iPxAcctAR_Transaction as a "
            sSQL += "INNER JOIN iPxAcctAR_Cfg_Transactiontype as c ON a.businessid COLLATE SQL_Latin1_General_CP1_CI_AS = c.businessid "
            sSQL += "AND a.transactiontype = c.TransactionType where a.businessid ='" & Session("sBusinessID") & "' and a.TransID='" & Session("sTransID") & "' and a.isActive = 'Y' and c.Coa='') as coaDebit "
            sSQL += "FROM iPxAcctAR_TransHdr AS b "
            sSQL += "WHERE b.businessid='" & Session("sBusinessID") & "'  AND b.TransID='" & Session("sTransID") & "' "

            oSQLCmd.CommandText = sSQL
            oSQLReader = oSQLCmd.ExecuteReader
            While oSQLReader.Read
                totCust = oSQLReader.Item("TotCust").ToString
                totTranstype = oSQLReader.Item("totPay").ToString
                coaCredit = oSQLReader.Item("coaCredit").ToString
                coaDebit = oSQLReader.Item("coaDebit").ToString
                MonthImport = Strings.Mid(Format(oSQLReader.Item("transDate"), "dd-MM-yyy").ToString, 4, 2)
                YearImport = Strings.Right(Format(oSQLReader.Item("transDate"), "dd-MM-yyy").ToString, 4)
            End While
            oSQLReader.Close()
            If totCust = "0" And totTranstype = "0" And coaCredit = "0" And coaDebit = "0" Then
                cekCloseGrid()
            ElseIf totCust <> "0" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Posting process failed, customer id is empty !');document.getElementById('Buttonx').click()", True)
            ElseIf totTranstype <> "0" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Posting process failed, transaction type is empty !');document.getElementById('Buttonx').click()", True)
            ElseIf coaCredit <> "0" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Posting process failed, COA Credit is empty !');document.getElementById('Buttonx').click()", True)
            ElseIf coaDebit <> "0" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Posting process failed, COA Debit is empty !');document.getElementById('Buttonx').click()", True)
            End If
        ElseIf e.CommandName = "getPrint" Then
                Dim cIpx As New iPxClass
                Session("sReport") = "ARTrans"
                Session("sTransID") = e.CommandArgument
                tampilFO()
                'Session("filename") = "dckCashierSummary_Form.rpt"
                Session("sMapPath") = "~/iPxReportFile/dckAR_Transaction.rpt"
                'Session("sFOLink") = txtP2.Text
                Response.Redirect("rptviewer.aspx")
            End If
    End Sub

    Protected Sub lbQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbQuery.Click
        tbRegDate.Text = ""
        tbTransDate.Text = ""
        tbTransID.Text = ""
        tbQFrom.Text = ""
        tbQUntil.Text = ""
        Session("sQueryTicket") = ""
        FOLink()
        dlQStatus.Items.Clear()
        showdata_dropdownCustStatus()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "datetimepicker()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalQuery", "showModalQuery()", True)
    End Sub

    Protected Sub lbAbortQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortQuery.Click
        Session("sQueryTicket") = ""
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQuery", "hideModalQuery()", True)
    End Sub

    Protected Sub lblQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblQuery.Click
        If dlQStatus.SelectedIndex = 0 Then
            If tbTransDate.Text = "" And tbRegDate.Text = "" And tbQFrom.Text = "" And tbQUntil.Text = "" Then
                month = Left(tbDate.Text, 2)
                year = Right(tbDate.Text, 4)
                Session("sCondition") = Session("sCondition") & " AND month(b.TransDate)='" & month & "' AND year(b.TransDate)='" & year & "' AND b.Status<>'X' "
            ElseIf tbTransDate.Text <> "" Or tbRegDate.Text <> "" Or tbQFrom.Text <> "" Or tbQUntil.Text <> "" Then
                Session("sCondition") = Session("sCondition") & " AND b.Status<>'X' "
                tbDate.Text = Format(Now, "MM-yyyy")
                Session("sDateWorkAR") = tbDate.Text
            End If
        ElseIf dlQStatus.SelectedIndex = 1 Then
            Session("sCondition") = Session("sCondition") & " and b.Status = 'N' "
        ElseIf dlQStatus.SelectedIndex = 2 Then
            Session("sCondition") = Session("sCondition") & " and b.Status = 'P'"
        ElseIf dlQStatus.SelectedIndex = 3 Then
            Session("sCondition") = Session("sCondition") & " and b.Status = 'X'"
        End If
        If dlFOLink.Text.Trim <> "" Then
            Session("sCondition") = Session("sCondition") & " and (FOLINK = '" & dlFOLink.SelectedValue & "') "
        End If
        If tbTransID.Text <> "" Then
            Session("sCondition") = Session("sCondition") & " and (b.TransID like '%" & Replace(tbTransID.Text, "'", "''") & "%') "
        End If
        If tbTransDate.Text <> "" Then
            Dim Transdate As Date
            Transdate = Date.ParseExact(tbTransDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
            Session("sCondition") = Session("sCondition") & " and (b.TransDate = '" & Transdate & "') "
        End If
        If tbRegDate.Text <> "" Then
            Dim Regdate As Date
            Regdate = Date.ParseExact(tbRegDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
            Session("sCondition") = Session("sCondition") & " and (b.RegDate = '" & Regdate & "') "
        End If
        If tbQFrom.Text.Trim <> "" And tbQUntil.Text.Trim <> "" Then
            Dim PerFrom As Date = Date.ParseExact(tbQFrom.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
            Dim PerUntl As Date = Date.ParseExact(tbQUntil.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
            Session("sCondition") = Session("sCondition") & " AND (b.TransDate >= '" & PerFrom & "') AND (b.TransDate <= '" & PerUntl & "') "
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQuery", "hideModalQuery()", True)
        ListARHeader()
    End Sub

    Protected Sub lbDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbDelete.Click
        If tbReason.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter your reason first !!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalDeleteTrans", "hideModalDeleteTrans()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDeleteTrans", "showModalDeleteTrans()", True)
        Else
            'saveReason()
            Dim Transdate As String = Format(Now, "yyy-MM-dd hh:mm:ss")
            deleteStatus()
            cIpx.saveLog(Session("sBusinessID"), Session("sTransID"), Transdate, "AR TR", "Delete", tbReason.Text, "", Session("sUserCode"))
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalDeleteTrans", "hideModalDeleteTrans()", True)
        End If
    End Sub

    Protected Sub lbImportCL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbImportCL.Click
        'ListFOLink()
        FOLinkImport()
        dlFOLinkImport.SelectedValue = cIpx.getDefaultParameter(Session("sBusinessID"), "10")
        tbDateImport.Text = Format(Now, "dd/MM/yyy")
        MonthImport = Strings.Mid(tbDateImport.Text, 4, 2)
        YearImport = Strings.Right(tbDateImport.Text, 4)
        cekClose()
        lbStartImportCL.Visible = True
        lbStartImportCC.Visible = False
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "datetimepicker()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
    End Sub
    Protected Sub lbImportCC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbImportCC.Click
        'ListFOLink()
        FOLinkImport()
        dlFOLinkImport.SelectedValue = cIpx.getDefaultParameter(Session("sBusinessID"), "10")
        tbDateImport.Text = Format(Now, "dd/MM/yyy")
        MonthImport = Strings.Mid(tbDateImport.Text, 4, 2)
        YearImport = Strings.Right(tbDateImport.Text, 4)
        cekClose()
        lbStartImportCL.Visible = False
        lbStartImportCC.Visible = True
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "datetimepicker()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
    End Sub

    Protected Sub lbStartImportCC_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbStartImportCC.Click
        If dlFOLinkImport.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please select FO Link first !!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
        Else
            'import credit card
            Dim Transdate As Date
            Transdate = Date.ParseExact(tbDateImport.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
            getImport = cIpx.getLogImport(Session("sBusinessID"), Transdate, "AR CC")
            If getImport = "Y" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('import failed, data is duplicated !');document.getElementById('Buttonx').click()", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
            Else
                ARCodeImport = cIpx.GetCounterMBR("AR", "AR")
                cekARDetailCC()
            End If
            'ARCodeImport = cIpx.GetCounterMBR("AR", "AR")
            'cekImport("CC")
        End If
    End Sub

    Protected Sub lbStartImportCL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbStartImportCL.Click
        If dlFOLinkImport.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please select FO Link first !!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "datetimepicker()", True)
        Else
            'import city ledger
            'ARCodeImport = cIpx.GetCounterMBR("AR", "AR")
            'cekImport("CL")
            Dim Transdate As Date
            Transdate = Date.ParseExact(tbDateImport.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
            getImport = cIpx.getLogImport(Session("sBusinessID"), Transdate, "AR CL")
            If getImport = "Y" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('import failed, data is duplicated !');document.getElementById('Buttonx').click()", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "datetimepicker()", True)
            Else
                ARCodeImport = cIpx.GetCounterMBR("AR", "AR")
                cekARDetailCL()
            End If
        End If
    End Sub

    Protected Sub lbAbortDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortDelete.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalDeleteTrans", "hideModalDeleteTrans()", True)
    End Sub

    Protected Sub lbOption_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbOption.Click
        FOLinkDatePosting()
        tbDatePost.Text = Format(Now, "MMMM yyy")
        ListDatePost()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MonthPost", "$(document).ready(function() {MonthPost()});", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDatePost", "showModalDatePost()", True)
    End Sub
    Protected Sub cariPost(ByVal sender As Object, ByVal e As EventArgs)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalDatePost", "hideModalDatePost()", True)
        ListDatePost()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MonthPost", "$(document).ready(function() {MonthPost()});", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDatePost", "showModalDatePost()", True)
    End Sub

    Protected Sub lbAbortDatePost_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortDatePost.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalDatePost", "hideModalDatePost()", True)
    End Sub

    Protected Sub dlFOLinkDatePosting_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dlFOLinkDatePosting.SelectedIndexChanged
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalDatePost", "hideModalDatePost()", True)
        tbDatePost.Text = Format(Now, "MMMM yyy")
        ListDatePost()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MonthPost", "$(document).ready(function() {MonthPost()});", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDatePost", "showModalDatePost()", True)
    End Sub
End Class
