﻿Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration
Imports System.Net.Mail
Imports System.Net
Imports System.Security.Cryptography.X509Certificates
Imports System.Net.Security
Partial Class iPxAdmin_iPxAPInputPV
    Inherits System.Web.UI.Page
    Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    'Dim sCnctPMS As String = ConfigurationManager.ConnectionStrings("iPxCNCTPMS").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
    'Dim oCnctPMS As SqlConnection = New SqlConnection(sCnctPMS)
    Dim oSQLCmd As SqlCommand
    Dim oSQLReader As SqlDataReader
    Dim sSQL, sSQLPMS As String
    Dim cIpx As New iPxClass

    Sub kosong()
        tbDueDate.Text = ""
        tbInvDate.Text = ""
        tbNotes.Text = ""
        tbReff.Text = ""
    End Sub
    Sub listCustomer()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT iPxAcctAP_Cfg_Vendor.businessid, iPxAcctAP_Cfg_Vendor.VendorID, iPxAcctAP_Cfg_Vendor.CoyName, "
        sSQL += "(SELECT sum(iPxAcctAP_Transaction.amountcr) FROM iPxAcctAP_Transaction "
        sSQL += "WHERE iPxAcctAP_Transaction.PVno='' and iPxAcctAP_Transaction.isActive='Y' and iPxAcctAP_Transaction.VendorID=iPxAcctAP_Cfg_Vendor.VendorID) as amount FROM iPxAcctAP_Cfg_Vendor "
        sSQL += "where iPxAcctAP_Cfg_Vendor.businessid ='" & Session("sBusinessID") & "' AND "
        sSQL += "iPxAcctAP_Cfg_Vendor.VendorID = (select VendorID from iPxAcctAP_Transaction where iPxAcctAP_Transaction.VendorID = iPxAcctAP_Cfg_Vendor.VendorID and PVno='' and iPxAcctAP_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAP_Transaction.IsActive ='Y' group by VendorID) AND "
        If Session("sQueryTicket") = "" Then
            Session("sQueryTicket") = Session("sCondition")
            If Session("sQueryTicket") <> "" Or Session("sCondition") <> "" Then
                sSQL = sSQL & Session("sQueryTicket")
                Session("sCondition") = ""
            Else
                sSQL = sSQL & "iPxAcctAP_Cfg_Vendor.IsActive='" & "Y" & "'"
            End If
        Else
            sSQL = sSQL & Session("sQueryTicket")
            Session("sCondition") = ""
        End If
        sSQL += " order by iPxAcctAP_Cfg_Vendor.VendorID asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvCustAR.DataSource = dt
                    gvCustAR.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvCustAR.DataSource = dt
                    gvCustAR.DataBind()
                    gvCustAR.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub listInvoice()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT iPxAcctAP_Transaction.RecID, iPxAcctAP_Cfg_Vendor.CoyName, iPxAcctAP_Cfg_Transactiontype.Description, iPxAcctAP_Transaction.TransDate, iPxAcctAP_Transaction.PVno, iPxAcctAP_Transaction.POno, "
        sSQL += "iPxAcctAP_Transaction.RRno, iPxAcctAP_Transaction.notes, iPxAcctAP_Transaction.amountcr FROM iPxAcctAP_Transaction "
        sSQL += "INNER JOIN iPxAcctAP_Cfg_Vendor ON iPxAcctAP_Transaction.businessid = iPxAcctAP_Cfg_Vendor.businessid AND "
        sSQL += "iPxAcctAP_Transaction.VendorID = iPxAcctAP_Cfg_Vendor.VendorID "
        sSQL += "INNER JOIN iPxAcctAP_Cfg_Transactiontype ON iPxAcctAP_Transaction.businessid COLLATE SQL_Latin1_General_CP1_CI_AS = iPxAcctAP_Cfg_Transactiontype.businessid AND "
        sSQL += "iPxAcctAP_Transaction.transactiontype = iPxAcctAP_Cfg_Transactiontype.TransactionType "
        sSQL += "WHERE iPxAcctAP_Transaction.PVno='' and iPxAcctAP_Transaction.isActive='Y' and iPxAcctAP_Transaction.VendorID='" & Session("sCustInv") & "'"
        sSQL += " order by iPxAcctAP_Cfg_Vendor.CoyName asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvInvoice.DataSource = dt
                    gvInvoice.DataBind()
                    'Calculate Sum and display in Footer Row
                    Dim total As Decimal = dt.Compute("Sum(amountcr)", "").ToString()
                    gvInvoice.FooterRow.Cells(4).Text = "Total"
                    gvInvoice.FooterRow.Cells(4).HorizontalAlign = HorizontalAlign.Left
                    gvInvoice.FooterRow.Cells(5).Text = total.ToString("N2")
                    gvInvoice.FooterRow.Cells(5).HorizontalAlign = HorizontalAlign.Right
                    gvInvoice.FooterRow.Cells(6).Visible = False
                    gvInvoice.Enabled = True
                    lbSaveInvoice.Enabled = True
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvInvoice.DataSource = dt
                    gvInvoice.DataBind()
                    gvInvoice.Enabled = False
                    lbSaveInvoice.Enabled = False
                    gvInvoice.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub editInvoice()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcctAP_PV "
        sSQL += "WHERE businessid ='" & Session("sBusinessID") & "' and PVNo ='" & Session("sEditInv") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            Dim InvDate As Date = oSQLReader.Item("PVDate").ToString
            tbInvDate.Text = InvDate.ToString("dd/MM/yyyy")
            Dim DueDate As Date = oSQLReader.Item("DueDate").ToString
            tbDueDate.Text = DueDate.ToString("dd/MM/yyyy")
            tbNotes.Text = oSQLReader.Item("Notes").ToString
            tbReff.Text = oSQLReader.Item("ReffNo").ToString
            'If oSQLReader.Item("Status").ToString = "N" Then
            '    lbStatus.Text = "New"
            'ElseIf oSQLReader.Item("Status").ToString = "X" Then
            '    lbStatus.Text = "Delete"
            'ElseIf oSQLReader.Item("Status").ToString = "P" Then
            '    lbStatus.Text = "Paid"
            'End If
            Session("sCustInv") = oSQLReader.Item("VendorID").ToString
            oCnct.Close()
        Else
            oCnct.Close()
        End If
    End Sub
    Sub editDetailInvoice()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT iPxAcctAP_Transaction.RecID, iPxAcctAP_Cfg_Vendor.CoyName, iPxAcctAP_Cfg_Transactiontype.Description, iPxAcctAP_Transaction.PVno, iPxAcctAP_Transaction.POno, "
        sSQL += "iPxAcctAP_Transaction.RRno, iPxAcctAP_Transaction.notes, iPxAcctAP_Transaction.amountcr FROM iPxAcctAP_Transaction "
        sSQL += "INNER JOIN iPxAcctAP_Cfg_Vendor ON iPxAcctAP_Transaction.businessid = iPxAcctAP_Cfg_Vendor.businessid AND "
        sSQL += "iPxAcctAP_Transaction.VendorID = iPxAcctAP_Cfg_Vendor.VendorID "
        sSQL += "INNER JOIN iPxAcctAP_Cfg_Transactiontype ON iPxAcctAP_Transaction.businessid COLLATE SQL_Latin1_General_CP1_CI_AS = iPxAcctAP_Cfg_Transactiontype.businessid AND "
        sSQL += "iPxAcctAP_Transaction.transactiontype = iPxAcctAP_Cfg_Transactiontype.TransactionType "
        sSQL += "WHERE (iPxAcctAp_Transaction.PVno='" & Session("sEditInv") & "') and iPxAcctAP_Transaction.VendorID='" & Session("sCustInv") & "'"
        sSQL += " order by iPxAcctAP_Cfg_Vendor.CoyName asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvInvoice.DataSource = dt
                    gvInvoice.DataBind()
                    'Calculate Sum and display in Footer Row
                    Dim total As Decimal = dt.Compute("Sum(amountcr)", "").ToString()
                    gvInvoice.FooterRow.Cells(4).Text = "Total"
                    gvInvoice.FooterRow.Cells(4).HorizontalAlign = HorizontalAlign.Left
                    gvInvoice.FooterRow.Cells(5).Text = total.ToString("N2")
                    gvInvoice.FooterRow.Cells(5).HorizontalAlign = HorizontalAlign.Right
                    gvInvoice.FooterRow.Cells(6).Visible = False
                    gvInvoice.Enabled = True
                    lbSaveInvoice.Enabled = True
                    'If lbStatus.Text = "New" Then
                    '    gvInvoice.Enabled = True
                    'Else
                    '    gvInvoice.Enabled = False
                    'End If
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvInvoice.DataSource = dt
                    gvInvoice.DataBind()
                    gvInvoice.Enabled = False
                    lbSaveInvoice.Enabled = False
                    gvInvoice.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Private Sub StatusCek(ByVal checkState As Boolean)
        For Each row As GridViewRow In gvInvoice.Rows
            Dim cb As CheckBox = row.FindControl("cbSelect")
            If cb IsNot Nothing Then
                cb.Checked = checkState
            End If
        Next
    End Sub
    Sub updateInvoiceHeader()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        Dim regDate As Date = Date.Now()
        Dim InvDate, Duedate As Date
        InvDate = Date.ParseExact(tbInvDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
        Duedate = Date.ParseExact(tbDueDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
        sSQL = "UPDATE iPxAcctAP_PV SET PVDate='" & InvDate & "',DueDate='" & Duedate & "',ReffNo='" & tbReff.Text & "',Notes='" & tbNotes.Text & "'"
        sSQL = sSQL & "WHERE PVno='" & lbInvID.Text & "' and businessid='" & Session("sBusinessID") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
    Sub updateInvoiceTrans()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "UPDATE iPxAcctAP_Transaction SET PVno='" & lbInvID.Text & "'"
        sSQL = sSQL & "WHERE RecID ='" & Session("sRecIDIn") & "' and businessid='" & Session("sBusinessID") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
    Sub hapusInvoiceTrans()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "UPDATE iPxAcctAP_Transaction SET PVno=' '"
        sSQL = sSQL & "WHERE RecID ='" & Session("sRecIDIn") & "' and businessid='" & Session("sBusinessID") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
    Sub saveInvoice()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        Dim regDate As Date = Date.Now()
        Dim InvDate, Duedate As Date
        'If tbTransDate.Text <> "" Then
        InvDate = Date.ParseExact(tbInvDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
        Duedate = Date.ParseExact(tbDueDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
        'Else
        'dateBirthday = Date.ParseExact("01/01/1900", "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
        'End If
        sSQL = "INSERT INTO iPxAcctAP_PV(businessid,PVNo,Status,PVDate,DueDate,VendorID,ReffNo,Notes,RegDate,RegBy) "
        sSQL += "VALUES ('" & Session("sBusinessID") & "','" & lbInvID.Text & "','N','" & InvDate & "','" & Duedate & "'"
        sSQL += ",'" & Session("sCustInv") & "','" & Replace(tbReff.Text, "'", "''") & "','" & Replace(tbNotes.Text, "'", "''") & "'"
        sSQL += ",'" & regDate & "','" & Session("iUserID") & "') "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        Dim TransDateLog As String = Format(Now, "yyy-MM-dd hh:mm:ss")
        cIpx.saveLog(Session("sBusinessID"), lbInvID.Text, TransDateLog, "AP PV", "Save", "Create AP Payment Voucher from " & Session("sCoyName"), "", Session("sUserCode"))
    End Sub
    Private Sub SetData()
        Dim currentCount As Integer = 0
        Dim chkAll As CheckBox = DirectCast(gvInvoice.HeaderRow _
                        .Cells(0).FindControl("chkAll"), CheckBox)
        chkAll.Checked = True
        Dim arr As ArrayList = DirectCast(ViewState("SelectedRecords"), ArrayList)
        For i As Integer = 0 To gvInvoice.Rows.Count - 1
            Dim chk As CheckBox = DirectCast(gvInvoice.Rows(i).Cells(0) _
                                            .FindControl("chk"), CheckBox)
            If chk IsNot Nothing Then
                chk.Checked = arr.Contains(gvInvoice.DataKeys(i).Value)
                If Not chk.Checked Then
                    chkAll.Checked = False
                Else
                    currentCount += 1
                End If
            End If
        Next
        hfCount.Value = (arr.Count - currentCount).ToString()
    End Sub
    Sub coyName()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT iPxAcctAP_Cfg_Vendor.CoyName FROM iPxAcctAP_Transaction "
        sSQL += "INNER JOIN iPxAcctAP_Cfg_Vendor ON iPxAcctAP_Transaction.businessid = iPxAcctAP_Cfg_Vendor.businessid AND "
        sSQL += "iPxAcctAP_Transaction.VendorID = iPxAcctAP_Cfg_Vendor.VendorID "
        sSQL += "WHERE iPxAcctAP_Transaction.PVno='' and iPxAcctAP_Transaction.isActive='Y' and iPxAcctAP_Transaction.VendorID='" & Session("sCustInv") & "'"
        sSQL += " order by iPxAcctAP_Cfg_Vendor.CoyName asc"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            Session("sCoyName") = oSQLReader.Item("CoyName").ToString
            oCnct.Close()
        Else
            oCnct.Close()
        End If
    End Sub
    Sub coyNameEdit()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT iPxAcctAP_Cfg_Vendor.CoyName FROM iPxAcctAP_Transaction "
        sSQL += "INNER JOIN iPxAcctAP_Cfg_Vendor ON iPxAcctAP_Transaction.businessid = iPxAcctAP_Cfg_Vendor.businessid AND "
        sSQL += "iPxAcctAP_Transaction.VendorID = iPxAcctAP_Cfg_Vendor.VendorID "
        sSQL += "WHERE iPxAcctAP_Transaction.PVno='" & Session("sEditInv") & "' and iPxAcctAP_Transaction.isActive='Y' and iPxAcctAP_Transaction.VendorID='" & Session("sCustInv") & "'"
        sSQL += " order by iPxAcctAP_Cfg_Vendor.CoyName asc"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            Session("sCoyName") = oSQLReader.Item("CoyName").ToString
            oCnct.Close()
        Else
            oCnct.Close()
        End If
    End Sub

    Sub listCustomerEdit()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT iPxAcctAP_Cfg_Vendor.businessid, iPxAcctAP_Cfg_Vendor.vendorID, iPxAcctAP_Cfg_Vendor.CoyName, (SELECT sum(iPxAcctAP_Transaction.amountcr) FROM iPxAcctAP_Transaction "
        sSQL += "WHERE iPxAcctAP_Transaction.PVno='" & Session("sEditInv") & "' and iPxAcctAP_Transaction.isActive='Y' and iPxAcctAP_Transaction.VendorID=iPxAcctAP_Cfg_Vendor.VendorID) as amount FROM iPxAcctAP_Cfg_Vendor "
        sSQL += "where iPxAcctAP_Cfg_Vendor.businessid ='" & Session("sBusinessID") & "' AND iPxAcctAP_Cfg_Vendor.VendorID = (select VendorID from iPxAcctAP_Transaction where iPxAcctAP_Transaction.VendorID = iPxAcctAP_Cfg_Vendor.VendorID and PVno='" & Session("sEditInv") & "' and iPxAcctAP_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAP_Transaction.IsActive ='Y' group by VendorID) AND "
        If Session("sQueryTicket") = "" Then
            Session("sQueryTicket") = Session("sCondition")
            If Session("sQueryTicket") <> "" Or Session("sCondition") <> "" Then
                sSQL = sSQL & Session("sQueryTicket")
                Session("sCondition") = ""
            Else
                sSQL = sSQL & "iPxAcctAP_Cfg_Vendor.IsActive='" & "Y" & "'"
            End If
        Else
            sSQL = sSQL & Session("sQueryTicket")
            Session("sCondition") = ""
        End If
        sSQL += " order by iPxAcctAP_Cfg_Vendor.VendorID asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvCustAR.DataSource = dt
                    gvCustAR.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvCustAR.DataSource = dt
                    gvCustAR.DataBind()
                    gvCustAR.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            Session("sQueryTicket") = ""
            If Session("sEditInv") = "" Then
                Session("sChkAll") = ""
                Session("sCustInv") = ""
                listCustomer()
                'lbStatus.Text = "New"
                tbInvDate.Text = Format(Now, "dd/MM/yyyy")
                tbDueDate.Text = Format(DateAdd(DateInterval.Day, 30, Today), "dd/MM/yyyy")
            Else
                lbInvID.Text = Session("sEditInv")
                Session("sChkAll") = Session("sEditInv")
                listCustomerEdit()
                editDetailInvoice()
                Dim cb2 As CheckBox = gvInvoice.HeaderRow.FindControl("chkAll")
                cb2.Checked = True
                lbaddInvEdit.Enabled = True
                lbaddInvEdit.Text = "<i class='fa fa-edit'></i> Edit Invoice"
                'lbShowGrp.Visible = False
                'pnDetailTrans.Visible = True
                'If lbStatus.Text = "New" Then
                '    lbaddInvEdit.Enabled = True
                '    lbSaveInvoice.Enabled = True
                'Else
                '    lbaddInvEdit.Enabled = False
                '    lbSaveInvoice.Enabled = False
                'End If
            End If
        End If
        'End If
        'enable select All
        'If pnDetailTrans.Visible = True Then
        '    Dim arr As ArrayList
        '    If ViewState("SelectedRecords") IsNot Nothing Then
        '        arr = DirectCast(ViewState("SelectedRecords"), ArrayList)
        '    Else
        '        arr = New ArrayList()
        '    End If
        '    Dim enabled, pilih As Integer
        '    enabled = 0
        '    pilih = 0
        '    Dim chkAll As CheckBox = DirectCast(gvInvoice.HeaderRow _
        '                .Cells(0).FindControl("chkAll"), CheckBox)
        '    For i As Integer = 0 To gvInvoice.Rows.Count - 1
        '        Dim chk As CheckBox = DirectCast(gvInvoice.Rows(i).Cells(0) _
        '                                    .FindControl("chk"), CheckBox)
        '        If chk.Enabled = False Then
        '            enabled = enabled + 1
        '        Else
        '            If chk.Checked = True Then
        '                pilih = pilih + 1
        '            Else

        '            End If
        '        End If
        '    Next
        '    If enabled = gvInvoice.Rows.Count Then
        '        chkAll.Enabled = False
        '    End If
        '    If pilih = gvInvoice.Rows.Count Then
        '        chkAll.Checked = True
        '    End If
        'End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "datetimepicker()", True)
    End Sub

    Protected Sub gvCustAR_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvCustAR.RowCommand
        If e.CommandName = "getSelect" Then
            Session("sCustInv") = e.CommandArgument
            coyName()
            listInvoice()
            lblTitleOpen.Text = Session("sCoyName")
            lbaddInvEdit.Enabled = True
            'pnCustGroup.Visible = False
            'pnDetailTrans.Visible = True
        End If
    End Sub

    Protected Sub lbSaveInvoice_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSaveInvoice.Click
        If tbInvDate.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please select PV date !!');", True)
            tbInvDate.Focus()
        ElseIf tbDueDate.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please select due date !!');", True)
            tbDueDate.Focus()
        ElseIf tbReff.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter reff no !!');", True)
            tbReff.Focus()
        ElseIf Session("sCustInv") = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please select Vendor first !!');", True)
        Else
            '
            If oCnct.State = ConnectionState.Closed Then
                oCnct.Open()
            End If
            oSQLCmd = New SqlCommand(sSQL, oCnct)
            sSQL = "SELECT PVNo FROM iPxAcctAP_PV WHERE PVNo ='" & Session("sEditInv") & "'"
            oSQLCmd.CommandText = sSQL
            oSQLReader = oSQLCmd.ExecuteReader

            oSQLReader.Read()
            If oSQLReader.HasRows Then
                'Update
                oCnct.Close()
                updateInvoiceHeader()
                Dim cb2 As CheckBox = gvInvoice.HeaderRow.FindControl("chkAll")
                Dim cbterpilih As Boolean = False
                Dim cb As CheckBox = Nothing
                Dim n As Integer = 0
                Dim m As Integer = 0
                Do Until n = gvInvoice.Rows.Count
                    cb = gvInvoice.Rows.Item(n).FindControl("chk")
                    If cb IsNot Nothing AndAlso cb.Checked Then
                        cbterpilih = True
                        'insert & update
                        Session("sRecIDIn") = (gvInvoice.Rows(n).Cells(1).Text)
                        updateInvoiceTrans()
                    Else
                        Session("sRecIDIn") = (gvInvoice.Rows(n).Cells(1).Text)
                        hapusInvoiceTrans()
                    End If
                    n += 1
                Loop
                Session("sChkAll") = Session("sEditInv")
                editInvoice()
                editDetailInvoice()
                coyNameEdit()
                'lbShowGrp.Visible = False
                'pnDetailTrans.Visible = True
                lbaddInvEdit.Visible = True
                lbDetailInv.Visible = False
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('update invoice successfully !');document.getElementById('Buttonx').click()", True)
                Dim TransDateLog As String = Format(Now, "yyy-MM-dd hh:mm:ss")
                cIpx.saveLog(Session("sBusinessID"), lbInvID.Text, TransDateLog, "AP PV", "Update", "Update AP Payment Voucher from " & Session("sCoyName"), "", Session("sUserCode"))
                Response.Redirect("iPxAPVoucher.aspx")
            Else
                'Save
                oCnct.Close()
                Dim cb2 As CheckBox = gvInvoice.HeaderRow.FindControl("chkAll")
                Dim cbterpilih As Boolean = False
                Dim cb As CheckBox = Nothing
                Dim n As Integer = 0
                Dim m As Integer = 0
                Do Until n = gvInvoice.Rows.Count
                    cb = gvInvoice.Rows.Item(n).FindControl("chk")
                    If cb IsNot Nothing AndAlso cb.Checked Then
                        cbterpilih = True
                        'insert & update
                        m = m + 1
                    End If
                    n += 1
                Loop
                If m > 0 Then
                    n = 0
                    saveInvoice()
                    Do Until n = gvInvoice.Rows.Count
                        cb = gvInvoice.Rows.Item(n).FindControl("chk")
                        If cb IsNot Nothing AndAlso cb.Checked Then
                            cbterpilih = True
                            'insert & update
                            Session("sRecIDIn") = (gvInvoice.Rows(n).Cells(1).Text)
                            updateInvoiceTrans()
                        End If
                        n += 1
                    Loop
                    'pnCustGroup.Visible = True
                    'pnDetailTrans.Visible = False
                    listCustomer()
                    kosong()
                    tbInvDate.Text = Format(Now, "dd/MM/yyyy")
                    tbDueDate.Text = Format(DateAdd(DateInterval.Day, 30, Today), "dd/MM/yyyy")
                    lbInvID.Text = cIpx.GetCounterMBR("IV", "IV")
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('create invoice successfully !');document.getElementById('Buttonx').click()", True)
                    Response.Redirect("iPxAPVoucher.aspx")
                Else
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('please select the transaction first !');document.getElementById('Buttonx').click()", True)
                    'pnCustGroup.Visible = False
                    'pnDetailTrans.Visible = True
                End If
            End If
            '

        End If
    End Sub

    'Protected Sub lbShowGrp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbShowGrp.Click
    '    pnDetailTrans.Visible = False
    '    pnCustGroup.Visible = True
    'End Sub

    Protected Sub lbAbort_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbort.Click
        Response.Redirect("iPxAPVoucher.aspx")
    End Sub

    Protected Sub lbaddInvEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbaddInvEdit.Click
        If Session("sEditInv") = "" Then
            lbInvID.Text = cIpx.GetCounterMBR("PV", "PV")
            tbReff.Text = ""
            tbNotes.Text = ""
            lbSaveInvoice.Text = "<i class='fa fa-save'></i> Save Payment Voucher"
        Else
            lbInvID.Text = Session("sEditInv")
            editInvoice()
            lbSaveInvoice.Text = "<i class='fa fa-save'></i> Update Payment Voucher"
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalCreatInv", "showModalCreatInv()", True)
        'Session("sChkAll") = ""
        'listInvoice()
        'lbaddInvEdit.Visible = False
        'lbDetailInv.Visible = True
    End Sub

    Protected Sub lbDetailInv_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbDetailInv.Click
        Session("sChkAll") = Session("sEditInv")
        editDetailInvoice()
        lbaddInvEdit.Visible = True
        lbDetailInv.Visible = False
        lbSaveInvoice.Enabled = True
        Dim cb2 As CheckBox = gvInvoice.HeaderRow.FindControl("chkAll")
        cb2.Checked = True
    End Sub
End Class
