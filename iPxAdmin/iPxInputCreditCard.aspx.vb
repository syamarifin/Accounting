Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration
Partial Class iPxAdmin_iPxInputCreditCrad
    Inherits System.Web.UI.Page
    Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim sCnctPMS As String = ConfigurationManager.ConnectionStrings("iPxCNCTPMS").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
    Dim oCnctPMS As SqlConnection = New SqlConnection(sCnctPMS)
    Dim oSQLCmd As SqlCommand
    Dim oSQLReader As SqlDataReader
    Dim sSQL, sSQLPMS As String
    Dim cIpx As New iPxClass
    Sub kosong()
        tbNotes.Text = ""
        tbReff.Text = ""
    End Sub
    Sub listCustomer()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT iPxAcctAR_Cfg_Customer.businessid, iPxAcctAR_Cfg_Customer.CustomerID, iPxAcctAR_Cfg_CustomerGrp.Description AS ARGroup, iPxAcctAR_Cfg_CoyGroup.Description AS CoyGroup, "
        sSQL += "iPxAcctAR_Cfg_Customer.CoyName, iPxAcctAR_Cfg_Customer.Address, iPxAcctAR_Cfg_Customer.BilllingAddress, "
        sSQL += "iPx_profile_geog_country.country, iPx_profile_geog_province.description AS Province, iPx_profile_geog_city.city, iPxAcctAR_Cfg_Customer.Phone, "
        sSQL += "iPxAcctAR_Cfg_Customer.Email FROM iPxAcctAR_Cfg_Customer "
        sSQL += "INNER JOIN iPxAcctAR_Cfg_CoyGroup ON iPxAcctAR_Cfg_Customer.businessid = iPxAcctAR_Cfg_CoyGroup.businessid AND iPxAcctAR_Cfg_Customer.CoyGroup = iPxAcctAR_Cfg_CoyGroup.CoyGroup "
        sSQL += "INNER JOIN iPxAcctAR_Cfg_CustomerGrp ON iPxAcctAR_Cfg_Customer.businessid = iPxAcctAR_Cfg_CustomerGrp.businessid AND iPxAcctAR_Cfg_Customer.arGroup = iPxAcctAR_Cfg_CustomerGrp.arGroup "
        sSQL += "INNER JOIN iPx_profile_geog_country ON iPxAcctAR_Cfg_Customer.CountryId = iPx_profile_geog_country.countryid "
        sSQL += "INNER JOIN iPx_profile_geog_province ON iPx_profile_geog_country.countryid = iPx_profile_geog_province.countryid AND "
        sSQL += "iPxAcctAR_Cfg_Customer.provid COLLATE SQL_Latin1_General_CP1_CI_AS = iPx_profile_geog_province.provid "
        sSQL += "INNER JOIN iPx_profile_geog_city ON iPx_profile_geog_province.countryid = iPx_profile_geog_city.countryid AND "
        sSQL += "iPx_profile_geog_province.provid = iPx_profile_geog_city.provid AND iPxAcctAR_Cfg_Customer.CityID = iPx_profile_geog_city.cityid "
        sSQL += "where iPxAcctAR_Cfg_Customer.businessid ='" & Session("sBusinessID") & "' AND iPxAcctAR_Cfg_CustomerGrp.arGroup='CC' AND iPxAcctAR_Cfg_Customer.CustomerID = (select CustomerID from iPxAcctAR_Transaction where iPxAcctAR_Transaction.CustomerID = iPxAcctAR_Cfg_Customer.CustomerID and invoiceno='' and iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Transaction.IsActive ='Y' group by CustomerID) AND "
        If Session("sQueryTicket") = "" Then
            Session("sQueryTicket") = Session("sCondition")
            If Session("sQueryTicket") <> "" Or Session("sCondition") <> "" Then
                sSQL = sSQL & Session("sQueryTicket")
                Session("sCondition") = ""
            Else
                sSQL = sSQL & "iPxAcctAR_Cfg_Customer.IsActive='" & "Y" & "'"
            End If
        Else
            sSQL = sSQL & Session("sQueryTicket")
            Session("sCondition") = ""
        End If
        sSQL += " order by iPxAcctAR_Cfg_Customer.CustomerID asc"
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
        sSQL = "SELECT iPxAcctAR_Transaction.RecID, iPxAcctAR_Cfg_Customer.CoyName, iPxAcctAR_Cfg_Transactiontype.Description, iPxAcctAR_Transaction.invoiceno, iPxAcctAR_Transaction.voucherno, iPxAcctAR_Transaction.foliono, "
        sSQL += "iPxAcctAR_Transaction.GuestName, iPxAcctAR_Transaction.notes, iPxAcctAR_Transaction.RoomNo, iPxAcctAR_Transaction.arrival, iPxAcctAR_Transaction.departure, "
        sSQL += "iPxAcctAR_Transaction.amountdr FROM iPxAcctAR_Transaction "
        sSQL += "INNER JOIN iPxAcctAR_Cfg_Customer ON iPxAcctAR_Transaction.businessid = iPxAcctAR_Cfg_Customer.businessid AND "
        sSQL += "iPxAcctAR_Transaction.CustomerID = iPxAcctAR_Cfg_Customer.CustomerID "
        sSQL += "INNER JOIN iPxAcctAR_Cfg_Transactiontype ON iPxAcctAR_Transaction.businessid COLLATE SQL_Latin1_General_CP1_CI_AS = iPxAcctAR_Cfg_Transactiontype.businessid AND "
        sSQL += "iPxAcctAR_Transaction.transactiontype = iPxAcctAR_Cfg_Transactiontype.TransactionType "
        sSQL += "WHERE iPxAcctAR_Transaction.invoiceno='' and iPxAcctAR_Transaction.isActive='Y' and iPxAcctAR_Transaction.CustomerID='" & Session("sCustInv") & "'"
        sSQL += " order by iPxAcctAR_Cfg_Customer.CoyName asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvInvoice.DataSource = dt
                    gvInvoice.DataBind()
                    'Calculate Sum and display in Footer Row
                    Dim total As Decimal = dt.Compute("Sum(amountdr)", "").ToString()
                    gvInvoice.FooterRow.Cells(9).Text = "Total"
                    gvInvoice.FooterRow.Cells(9).HorizontalAlign = HorizontalAlign.Left
                    gvInvoice.FooterRow.Cells(10).Text = total.ToString("N2")
                    gvInvoice.FooterRow.Cells(10).HorizontalAlign = HorizontalAlign.Right
                    gvInvoice.FooterRow.Cells(11).Visible = False
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
        sSQL = "SELECT * FROM iPxAcctAR_Invoice "
        sSQL += "WHERE InvoiceNo ='" & Session("sEditInv") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            tbNotes.Text = oSQLReader.Item("Notes").ToString
            tbReff.Text = oSQLReader.Item("ReffNo").ToString
            If oSQLReader.Item("Status").ToString = "N" Then
                lbStatus.Text = "New"
            ElseIf oSQLReader.Item("Status").ToString = "X" Then
                lbStatus.Text = "Delete"
            ElseIf oSQLReader.Item("Status").ToString = "P" Then
                lbStatus.Text = "Paid"
            End If
            Session("sCustInv") = oSQLReader.Item("CustomerID").ToString
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
        sSQL = "SELECT iPxAcctAR_Transaction.RecID, iPxAcctAR_Cfg_Customer.CoyName, iPxAcctAR_Cfg_Transactiontype.Description, iPxAcctAR_Transaction.invoiceno, iPxAcctAR_Transaction.voucherno, iPxAcctAR_Transaction.foliono, "
        sSQL += "iPxAcctAR_Transaction.GuestName, iPxAcctAR_Transaction.notes, iPxAcctAR_Transaction.RoomNo, iPxAcctAR_Transaction.arrival, iPxAcctAR_Transaction.departure, "
        sSQL += "iPxAcctAR_Transaction.amountdr FROM iPxAcctAR_Transaction "
        sSQL += "INNER JOIN iPxAcctAR_Cfg_Customer ON iPxAcctAR_Transaction.businessid = iPxAcctAR_Cfg_Customer.businessid AND "
        sSQL += "iPxAcctAR_Transaction.CustomerID = iPxAcctAR_Cfg_Customer.CustomerID "
        sSQL += "INNER JOIN iPxAcctAR_Cfg_Transactiontype ON iPxAcctAR_Transaction.businessid COLLATE SQL_Latin1_General_CP1_CI_AS = iPxAcctAR_Cfg_Transactiontype.businessid AND "
        sSQL += "iPxAcctAR_Transaction.transactiontype = iPxAcctAR_Cfg_Transactiontype.TransactionType "
        sSQL += "WHERE (iPxAcctAR_Transaction.invoiceno='" & Session("sEditInv") & "') and iPxAcctAR_Transaction.CustomerID='" & Session("sCustInv") & "'"
        sSQL += " order by iPxAcctAR_Cfg_Customer.CoyName asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvInvoice.DataSource = dt
                    gvInvoice.DataBind()
                    'Calculate Sum and display in Footer Row
                    Dim total As Decimal = dt.Compute("Sum(amountdr)", "").ToString()
                    gvInvoice.FooterRow.Cells(9).Text = "Total"
                    gvInvoice.FooterRow.Cells(9).HorizontalAlign = HorizontalAlign.Left
                    gvInvoice.FooterRow.Cells(10).Text = total.ToString("N2")
                    gvInvoice.FooterRow.Cells(10).HorizontalAlign = HorizontalAlign.Right
                    gvInvoice.FooterRow.Cells(11).Visible = False
                    gvInvoice.Enabled = True
                    lbSaveInvoice.Enabled = True
                    If lbStatus.Text = "New" Then
                        gvInvoice.Enabled = True
                    Else
                        gvInvoice.Enabled = False
                    End If
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
    Sub updateInvoiceTrans()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "UPDATE iPxAcctAR_Transaction SET invoiceno='" & lbInvID.Text & "'"
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
        sSQL = "UPDATE iPxAcctAR_Transaction SET invoiceno=' '"
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
        
        sSQL = "INSERT INTO iPxAcctAR_Invoice(businessid,InvoiceNo,Status,InvDate,DueDate,CustomerID,ReffNo,Notes,RegDate,RegBy) "
        sSQL += "VALUES ('" & Session("sBusinessID") & "','" & lbInvID.Text & "','N','" & regDate & "','" & regDate & "'"
        sSQL += ",'" & Session("sCustInv") & "','" & Replace(tbReff.Text, "'", "''") & "','" & Replace(tbNotes.Text, "'", "''") & "'"
        sSQL += ",'" & regDate & "','" & Session("iUserID") & "') "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
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
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            Session("sQueryTicket") = ""
            If Session("sEditInv") = "" Then
                Session("sChkAll") = ""
                Session("sCustInv") = ""
                listCustomer()
                lbInvID.Text = cIpx.GetCounterMBR("BC", "BC")
                lbStatus.Text = "New"
            Else
                lbInvID.Text = Session("sEditInv")
                Session("sChkAll") = Session("sEditInv")
                lbSaveInvoice.Text = "<i class='fa fa-save'></i> Update Bath"
                editInvoice()
                editDetailInvoice()
                lbShowGrp.Visible = False
                pnDetailTrans.Visible = True
                lbaddInvEdit.Visible = True
                If lbStatus.Text = "New" Then
                    lbaddInvEdit.Enabled = True
                    lbSaveInvoice.Enabled = True
                Else
                    lbaddInvEdit.Enabled = False
                    lbSaveInvoice.Enabled = False
                End If
            End If
        End If
        'End If
        'enable select All
        If pnDetailTrans.Visible = True Then
            Dim arr As ArrayList
            If ViewState("SelectedRecords") IsNot Nothing Then
                arr = DirectCast(ViewState("SelectedRecords"), ArrayList)
            Else
                arr = New ArrayList()
            End If
            Dim enabled, pilih As Integer
            enabled = 0
            pilih = 0
            Dim chkAll As CheckBox = DirectCast(gvInvoice.HeaderRow _
                        .Cells(0).FindControl("chkAll"), CheckBox)
            For i As Integer = 0 To gvInvoice.Rows.Count - 1
                Dim chk As CheckBox = DirectCast(gvInvoice.Rows(i).Cells(0) _
                                            .FindControl("chk"), CheckBox)
                If chk.Enabled = False Then
                    enabled = enabled + 1
                Else
                    If chk.Checked = True Then
                        pilih = pilih + 1
                    Else

                    End If
                End If
            Next
            If enabled = gvInvoice.Rows.Count Then
                chkAll.Enabled = False
            End If
            If pilih = gvInvoice.Rows.Count Then
                chkAll.Checked = True
            End If
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "datetimepicker()", True)
    End Sub

    Protected Sub gvCustAR_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvCustAR.RowCommand
        If e.CommandName = "getSelect" Then
            Session("sCustInv") = e.CommandArgument
            listInvoice()
            pnCustGroup.Visible = False
            pnDetailTrans.Visible = True
        End If
    End Sub

    Protected Sub lbSaveInvoice_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSaveInvoice.Click
        If tbReff.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter reff no !!');", True)
            tbReff.Focus()
        ElseIf Session("sCustInv") = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please select customer first !!');", True)
        Else
            '
            If oCnct.State = ConnectionState.Closed Then
                oCnct.Open()
            End If
            oSQLCmd = New SqlCommand(sSQL, oCnct)
            sSQL = "SELECT InvoiceNo FROM iPxAcctAR_Invoice WHERE InvoiceNo ='" & Session("sEditInv") & "'"
            oSQLCmd.CommandText = sSQL
            oSQLReader = oSQLCmd.ExecuteReader

            oSQLReader.Read()
            If oSQLReader.HasRows Then
                'Update
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
                lbShowGrp.Visible = False
                pnDetailTrans.Visible = True
                lbaddInvEdit.Visible = True
                lbDetailInv.Visible = False
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('update batch successfully !');document.getElementById('Buttonx').click()", True)
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
                    pnCustGroup.Visible = True
                    pnDetailTrans.Visible = False
                    listCustomer()
                    kosong()
                    lbInvID.Text = cIpx.GetCounterMBR("BC", "BC")
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('create batch successfully !');document.getElementById('Buttonx').click()", True)
                Else
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('please select the transaction first !');document.getElementById('Buttonx').click()", True)
                    pnCustGroup.Visible = False
                    pnDetailTrans.Visible = True
                End If
            End If
        End If
    End Sub

    Protected Sub lbShowGrp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbShowGrp.Click
        pnDetailTrans.Visible = False
        pnCustGroup.Visible = True
    End Sub

    Protected Sub lbAbort_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbort.Click
        Response.Redirect("iPxARCreditCard.aspx")
    End Sub

    Protected Sub lbaddInvEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbaddInvEdit.Click
        Session("sChkAll") = ""
        listInvoice()
        lbaddInvEdit.Visible = False
        lbDetailInv.Visible = True
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
