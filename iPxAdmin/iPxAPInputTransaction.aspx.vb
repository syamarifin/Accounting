Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration
Partial Class iPxAdmin_iPxAPInputTransaction
    Inherits System.Web.UI.Page
    Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    'Dim sCnctPMS As String = ConfigurationManager.ConnectionStrings("iPxCNCTPMS").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
    'Dim oCnctPMS As SqlConnection = New SqlConnection(sCnctPMS)
    Dim oSQLCmd As SqlCommand
    Dim oSQLReader As SqlDataReader
    Dim sSQL, sSQLPMS, MonthCekClosing, YearCekClosing, Close As String
    Dim cIpx As New iPxClass
    Sub UserAcces()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.businessid, a.usercode, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.funtionid='10041'and x.active='Y' and x.businessid=a.businessid and x.usercode=a.usercode) as edit "
        sSQL += "from iPxAcct_profile_client_useraccess as a "
        sSQL += "INNER JOIN iPxAcct_profile_client_useraccess_dtl as b ON b.businessid=a.businessid and b.usercode=a.usercode "
        sSQL += "where a.businessid='" & Session("sBusinessID") & "' and a.usercode='" & Session("sUserCode") & "' and a.active='Y' group by a.businessid, a.usercode "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If Session("sTransID") <> "" Then
            If oSQLReader.HasRows Then
                If Close = "Close" Then
                    lbSave.Enabled = False
                    lbAddDetail.Enabled = False
                Else
                    If oSQLReader.Item("edit").ToString = "Y" Then
                        If lbStatus.Text = "New" Then
                            lbAddDetail.Enabled = True
                            lbSave.Enabled = True
                        Else
                            lbSave.Enabled = False
                            lbAddDetail.Enabled = False
                        End If
                        'lbAddDetail.Enabled = True
                        'lbSave.Enabled = True
                        dlFOLink.Enabled = True
                        tbDesc.Enabled = True
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datepicker", "$(document).ready(function() {datetimepicker()});", True)
                    Else
                        lbAddDetail.Enabled = False
                        lbSave.Enabled = False
                        dlFOLink.Enabled = False
                        tbDesc.Enabled = False
                    End If
                End If
            Else
                If lbStatus.Text = "New" Then
                    lbAddDetail.Enabled = True
                    lbSave.Enabled = True
                Else
                    lbSave.Enabled = False
                    lbAddDetail.Enabled = False
                End If
                'lbAddDetail.Enabled = True
                'lbSave.Enabled = True
                dlFOLink.Enabled = True
                tbDesc.Enabled = True
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datepicker", "$(document).ready(function() {datetimepicker()});", True)
            End If
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datepicker", "$(document).ready(function() {datetimepicker()});", True)
        End If
        oCnct.Close()
    End Sub
    Sub kosongType()
        tbTransType.Text = ""
        tbCoa.Text = ""
        tbDescription.Text = ""
        cbActive.Checked = True
    End Sub
    Sub kosong()
        tbAmount.Text = ""
        tbCustID.Text = ""
        tbPO.Text = ""
        tbRR.Text = ""
        tbNotes.Text = ""
    End Sub
    Sub TipeTrans()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select * from iPxAcctAP_Cfg_Transactiontype WHERE businessid='" & Session("sBusinessID") & "' and isActive='Y'"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                dlType.DataSource = dt
                dlType.DataTextField = "Description"
                dlType.DataValueField = "TransactionType"
                dlType.DataBind()
                dlType.Items.Insert(0, " ")
            End Using
        End Using
    End Sub
    Sub transdefault()
        TipeTrans()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcctAP_Cfg_Transactiontype "
        sSQL += "WHERE businessid='" & Session("sBusinessID") & "' and isDefault ='Y'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            dlType.SelectedValue = oSQLReader.Item("TransactionType").ToString
            oCnct.Close()
        Else
            oCnct.Close()
        End If
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
    Sub listCustomerARCA()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT iPxAcctAP_Cfg_Vendor.businessid, iPxAcctAP_Cfg_Vendor.VendorID, iPxAcctAP_Cfg_VendorGrp.Description AS APGroup, "
        sSQL += "iPxAcctAP_Cfg_Vendor.CoyName FROM iPxAcctAP_Cfg_Vendor "
        sSQL += "INNER JOIN iPxAcctAP_Cfg_VendorGrp ON iPxAcctAP_Cfg_Vendor.businessid = iPxAcctAP_Cfg_VendorGrp.businessid AND iPxAcctAP_Cfg_Vendor.apGroup = iPxAcctAP_Cfg_VendorGrp.apGroup "
        sSQL += "where iPxAcctAP_Cfg_Vendor.businessid='" & Session("sBusinessID") & "' and iPxAcctAP_Cfg_Vendor.IsActive='" & "Y" & "' "
        If Session("sQueryCust") = "" Then
            Session("sQueryCust") = Session("sConditionCust")
            If Session("sQueryCust") <> "" Or Session("sConditionCust") <> "" Then
                sSQL = sSQL & Session("sQueryCust")
                Session("sConditionCust") = ""
            Else
                sSQL = sSQL & " "
            End If
        Else
            sSQL = sSQL & Session("sQueryCust")
            Session("sConditionCust") = ""
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
    Sub listTransDetail()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT iPxAcctAP_Transaction.RecID, iPxAcctAP_Cfg_Vendor.CoyName, iPxAcctAP_Cfg_Transactiontype.Description, iPxAcctAP_Transaction.PVno, "
        sSQL += "iPxAcctAP_Transaction.POno, iPxAcctAP_Transaction.RRno, iPxAcctAP_Transaction.notes, iPxAcctAP_Transaction.amountcr, "
        sSQL += "(select 'Close' from iPxAcctGL_JVhdr as x where x.businessid ='" & Session("sBusinessID") & "' AND x.Status <> 'D' AND x.ReffNo like'PL%' and YEAR(x.TransDate)=YEAR(iPxAcctAP_Transaction.TransDate) AND MONTH(x.TransDate)=MONTH(iPxAcctAP_Transaction.TransDate)) as CloseStatus, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid=iPxAcctAP_Transaction.businessid and x.usercode='" & Session("sUserCode") & "' and x.funtionid='10041' and x.active='Y') as edit, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid=iPxAcctAP_Transaction.businessid and x.usercode='" & Session("sUserCode") & "' and x.funtionid='10042' and x.active='Y') as deleteAR "
        sSQL += "FROM iPxAcctAP_Transaction "
        sSQL += "LEFT JOIN iPxAcctAP_Cfg_Vendor ON iPxAcctAP_Transaction.businessid = iPxAcctAP_Cfg_Vendor.businessid AND "
        sSQL += "iPxAcctAP_Transaction.VendorID = iPxAcctAP_Cfg_Vendor.VendorID "
        sSQL += "LEFT JOIN iPxAcctAP_Cfg_Transactiontype ON iPxAcctAP_Transaction.businessid COLLATE SQL_Latin1_General_CP1_CI_AS = iPxAcctAP_Cfg_Transactiontype.businessid AND "
        sSQL += "iPxAcctAP_Transaction.transactiontype = iPxAcctAP_Cfg_Transactiontype.TransactionType "
        sSQL += "WHERE iPxAcctAP_Transaction.TransID='" & lbTransID.Text & "' and iPxAcctAP_Transaction.isActive = 'Y'"
        sSQL += " order by iPxAcctAP_Transaction.RecID asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvTransDetail.DataSource = dt
                    gvTransDetail.DataBind()
                    Dim totalAmountdr As Decimal
                    If dt.Compute("Sum(amountcr)", "").ToString() <> "" Then
                        totalAmountdr = dt.Compute("Sum(amountcr)", "").ToString()
                    Else
                        totalAmountdr = 0
                    End If
                    gvTransDetail.FooterRow.Cells(5).Text = "Total"
                    gvTransDetail.FooterRow.Cells(5).HorizontalAlign = HorizontalAlign.Left
                    gvTransDetail.FooterRow.Cells(6).Text = totalAmountdr.ToString("N2")
                    gvTransDetail.FooterRow.Cells(6).HorizontalAlign = HorizontalAlign.Right
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvTransDetail.DataSource = dt
                    gvTransDetail.DataBind()
                    gvTransDetail.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub listTransDetailDelete()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT iPxAcctAP_Transaction.RecID, iPxAcctAP_Cfg_Vendor.CoyName, iPxAcctAP_Cfg_Transactiontype.Description, iPxAcctAP_Transaction.PVno, "
        sSQL += "iPxAcctAP_Transaction.POno, iPxAcctAP_Transaction.RRno, iPxAcctAP_Transaction.notes, iPxAcctAP_Transaction.amountcr, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid=iPxAcctAP_Transaction.businessid and x.usercode='" & Session("sUserCode") & "' and x.funtionid='2' and x.active='Y') as edit, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid=iPxAcctAP_Transaction.businessid and x.usercode='" & Session("sUserCode") & "' and x.funtionid='3' and x.active='Y') as deleteAR "
        sSQL += "FROM iPxAcctAP_Transaction "
        sSQL += "LEFT JOIN iPxAcctAP_Cfg_Vendor ON iPxAcctAP_Transaction.businessid = iPxAcctAP_Cfg_Vendor.businessid AND "
        sSQL += "iPxAcctAP_Transaction.VendorID = iPxAcctAP_Cfg_Vendor.VendorID "
        sSQL += "LEFT JOIN iPxAcctAP_Cfg_Transactiontype ON iPxAcctAP_Transaction.businessid COLLATE SQL_Latin1_General_CP1_CI_AS = iPxAcctAP_Cfg_Transactiontype.businessid AND "
        sSQL += "iPxAcctAP_Transaction.transactiontype = iPxAcctAP_Cfg_Transactiontype.TransactionType "
        sSQL += "WHERE iPxAcctAP_Transaction.TransID='" & lbTransID.Text & "' and iPxAcctAP_Transaction.isActive = 'N'"
        sSQL += " order by iPxAcctAP_Transaction.RecID asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvTransDetail.DataSource = dt
                    gvTransDetail.DataBind()
                    Dim totalAmountdr As Decimal
                    If dt.Compute("Sum(amountcr)", "").ToString() <> "" Then
                        totalAmountdr = dt.Compute("Sum(amountcr)", "").ToString()
                    Else
                        totalAmountdr = 0
                    End If
                    gvTransDetail.FooterRow.Cells(8).Text = "Total"
                    gvTransDetail.FooterRow.Cells(8).HorizontalAlign = HorizontalAlign.Left
                    gvTransDetail.FooterRow.Cells(9).Text = totalAmountdr.ToString("N2")
                    gvTransDetail.FooterRow.Cells(9).HorizontalAlign = HorizontalAlign.Right
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvTransDetail.DataSource = dt
                    gvTransDetail.DataBind()
                    gvTransDetail.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub tampilHeader()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcctAP_TransHdr "
        sSQL += "WHERE TransID = '" & Session("sTransID") & "' AND businessid='" & Session("sBusinessID") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            dlFOLink.SelectedValue = oSQLReader.Item("InvLink").ToString
            Dim transDate As Date = oSQLReader.Item("TransDate").ToString
            tbTransDate.Text = transDate.ToString("dd/MM/yyyy")
            tbDesc.Text = oSQLReader.Item("Description").ToString
            If oSQLReader.Item("Status").ToString = "N" Then
                Session("sStatusARTrans") = oSQLReader.Item("Status").ToString
                lbStatus.Text = "New"
            ElseIf oSQLReader.Item("Status").ToString = "P" Then
                Session("sStatusARTrans") = oSQLReader.Item("Status").ToString
                lbStatus.Text = "Posted"
            ElseIf oSQLReader.Item("Status").ToString = "X" Then
                Session("sStatusARTrans") = oSQLReader.Item("Status").ToString
                lbStatus.Text = "Delete"
            End If
            oCnct.Close()
        Else
            oCnct.Close()
        End If
    End Sub
    Sub tampilDetail()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcctAP_Transaction "
        sSQL += "WHERE TransID = '" & lbTransID.Text & "' AND businessid='" & Session("sBusinessID") & "' AND RecID='" & Session("sRecID") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            dlType.SelectedValue = oSQLReader.Item("transactiontype").ToString
            Session("sCustID") = oSQLReader.Item("VendorID").ToString
            tbPO.Text = oSQLReader.Item("POno").ToString
            tbRR.Text = oSQLReader.Item("RRno").ToString
            tbNotes.Text = oSQLReader.Item("notes").ToString
            tbAmount.Text = String.Format("{0:N2}", (oSQLReader.Item("amountcr"))).ToString
            oCnct.Close()
            tampilCustomer()
        Else
            oCnct.Close()
        End If
    End Sub
    Sub tampilCustomer()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcctAP_Cfg_Vendor "
        sSQL += "WHERE VendorID = '" & Session("sCustID") & "' AND businessid='" & Session("sBusinessID") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            tbCustID.Text = oSQLReader.Item("CoyName").ToString
            oCnct.Close()
        Else
            oCnct.Close()
        End If
    End Sub
    Sub saveARTransHeader()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        'Dim active As String
        'If cbActive.Checked = True Then
        '    active = "Y"
        'ElseIf cbActive.Checked = False Then
        '    active = "N"
        'End If
        Dim regDate As Date = Date.Now()
        Dim range As String = Format(Now, "dd") - 1
        Dim Transdate As Date
        Transdate = Date.ParseExact(tbTransDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)

        sSQL = "INSERT INTO iPxAcctAP_TransHdr (businessid,TransID,Status,TransDate,InvLink,Description,RegDate,RegBy,isActive) "
        sSQL += "VALUES ('" & Session("sBusinessID") & "','" & lbTransID.Text & "','N','" & Transdate & "','" & dlFOLink.SelectedValue & "','" & Replace(tbDesc.Text, "'", "''") & "'"
        sSQL += ",'" & regDate & "','" & Session("iUserID") & "','Y') "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        Dim TransDateLog As String = Format(Now, "yyy-MM-dd hh:mm:ss")
        cIpx.saveLog(Session("sBusinessID"), lbTransID.Text, TransDateLog, "AP TR", "Save", "Create AP Transaction dari inventory " & dlFOLink.SelectedItem.ToString, dlFOLink.SelectedValue, Session("sUserCode"))
    End Sub
    Sub saveARTrans()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        'Dim active As String
        'If cbActive.Checked = True Then
        '    active = "Y"
        'ElseIf cbActive.Checked = False Then
        '    active = "N"
        'End If
        Dim regDate As Date = Date.Now()
        Dim range As String = Format(Now, "dd") - 1
        Dim Transdate, Arrival, Departure As Date
        'If tbTransDate.Text <> "" Then
        Transdate = Date.ParseExact(tbTransDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
        'Else
        'dateBirthday = Date.ParseExact("01/01/1900", "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
        'End If
        sSQL = "INSERT INTO iPxAcctAP_Transaction(businessid,TransID,TransDate,VendorID,TransactionType,PVno,POno,RRno,notes, "
        sSQL += "amountdr,amountcr,amountdr_frgn,amountcr_frgn,currency,InvLink,isActive) "
        sSQL += "VALUES ('" & Session("sBusinessID") & "','" & lbTransID.Text & "','" & Transdate & "','" & Session("sCustID") & "','" & dlType.SelectedValue & "'"
        sSQL += ",'','" & Replace(tbPO.Text, "'", "''") & "','" & Replace(tbRR.Text, "'", "''") & "','" & Replace(tbNotes.Text, "'", "''") & "','0', "
        sSQL += "'" & Replace(tbAmount.Text, "'", "''") & "','0','0','IDR','" & dlFOLink.SelectedValue & "','Y') "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        listTransDetail()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('data save successfully !');document.getElementById('Buttonx').click()", True)
        Dim TransDateLog As String = Format(Now, "yyy-MM-dd hh:mm:ss")
        cIpx.saveLog(Session("sBusinessID"), lbTransID.Text, TransDateLog, "AP DT", "Save", "Create AP Transaction detail dari Vendor " & tbCustID.Text, dlFOLink.SelectedValue, Session("sUserCode"))

    End Sub
#Region "Transaction Type"
    Sub listCoa()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcct_Coa where businessid ='" & Session("sBusinessID") & "' and IsActive='" & "Y" & "' "
        sSQL += "order by Coa asc"
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
    Sub saveTransType()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        Dim active As String
        If cbActive.Checked = True Then
            active = "Y"
        ElseIf cbActive.Checked = False Then
            active = "N"
        End If

        sSQL = "INSERT INTO iPxAcctAP_Cfg_Transactiontype (businessid, TransactionType, Description, Coa, isDefault, isActive) "
        sSQL = sSQL & "VALUES ('" & Session("sBusinessID") & "','" & Replace(tbTransType.Text, "'", "''") & "','" & Replace(tbDescription.Text, "'", "''") & "'"
        sSQL = sSQL & ",'" & Replace(tbCoa.Text, "'", "''") & "','N','" & active & "') "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('saved successfully!');", True)
        transdefault()
    End Sub
    Sub updateTransType()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        Dim active As String
        If cbActive.Checked = True Then
            active = "Y"
        ElseIf cbActive.Checked = False Then
            active = "N"
        End If
        sSQL = "UPDATE iPxAcctAP_Cfg_Transactiontype SET businessid='" & Session("sBusinessID") & "', Description='" & Replace(tbDescription.Text, "'", "''") & "'"
        sSQL += ",Coa='" & Replace(tbCoa.Text, "'", "''") & "', isActive='" & active & "'"
        sSQL = sSQL & "WHERE TransactionType ='" & tbTransType.Text & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('Data has been update !');", True)
        transdefault()
    End Sub
    Sub editTransType()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcctAP_Cfg_Transactiontype "
        sSQL += "WHERE TransactionType ='" & dlType.SelectedValue & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            tbDescription.Text = oSQLReader.Item("Description").ToString
            tbCoa.Text = oSQLReader.Item("Coa").ToString
            Dim Active As String = oSQLReader.Item("isActive").ToString
            If Active = "Y" Then
                cbActive.Checked = True
            Else
                cbActive.Checked = False
            End If
            oCnct.Close()
        Else
            oCnct.Close()
        End If
    End Sub
#End Region
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
            lbSave.Enabled = False
            Close = "Close"
            gvTransDetail.Enabled = False
            'Dim dateBirthday As Date = Date.ParseExact(tbTransDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
            'Dim forDate As String = dateBirthday.ToString("MM")
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('This Month Period Has been Close !!');", True)
        Else
            lbAddDetail.Enabled = True
            lbSave.Enabled = True
            gvTransDetail.Enabled = True
        End If
        oCnct4.Close()
    End Sub
    Protected Sub cari(ByVal sender As Object, ByVal e As EventArgs)
        MonthCekClosing = Strings.Mid(tbTransDate.Text, 4, 2)
        YearCekClosing = Strings.Right(tbTransDate.Text, 4)
        If lbStatus.Text = "New" Then
            lbAddDetail.Enabled = True
            lbSave.Enabled = True
            cekClose()
            PnReason.Visible = False
        Else
            lbSave.Enabled = False
            lbAddDetail.Enabled = False
            PnReason.Visible = True
        End If
        If lbStatus.Text = "Delete" Then
            PnReason.Visible = False
            listTransDetailDelete()
        Else
            PnReason.Visible = False
            listTransDetail()
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            If Session("sTransID") = "" Then
                FOLink()
                dlFOLink.SelectedValue = cIpx.getDefaultParameter(Session("sBusinessID"), "10")
                Session("sStatusARTrans") = "N"
                tbTransDate.Text = Format(Now, "dd/MM/yyy")
                lbTransID.Text = cIpx.GetCounterMBR("AP", "AP")
                listTransDetail()
                lbStatus.Text = "New"
                lbSave.Text = "<i class='fa fa-save'></i> Save "
                PnReason.Visible = False
                MonthCekClosing = Strings.Mid(tbTransDate.Text, 4, 2)
                YearCekClosing = Strings.Right(tbTransDate.Text, 4)
                cekClose()
                'listTransDetail()
            Else
                lbTransID.Text = Session("sTransID")
                FOLink()
                tampilHeader()
                lbSave.Text = "<i class='fa fa-save'></i> Update "
                If lbStatus.Text = "New" Then
                    lbAddDetail.Enabled = True
                    lbSave.Enabled = True
                    PnReason.Visible = False
                Else
                    lbSave.Enabled = False
                    lbAddDetail.Enabled = False
                    PnReason.Visible = True
                End If
                If lbStatus.Text = "Delete" Then
                    PnReason.Visible = False
                    listTransDetailDelete()
                Else
                    PnReason.Visible = False
                    listTransDetail()
                End If
                MonthCekClosing = Strings.Mid(tbTransDate.Text, 4, 2)
                YearCekClosing = Strings.Right(tbTransDate.Text, 4)
                cekClose()
            End If
        End If
        UserAcces()
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datepicker", "$(document).ready(function() {datetimepicker()});", True)
    End Sub

    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvTransDetail.PageIndex = e.NewPageIndex
        Me.listTransDetail()
    End Sub

    Protected Sub gvTransDetail_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvTransDetail.PageIndexChanging
        gvTransDetail.PageIndex = e.NewPageIndex
        listTransDetail()
    End Sub

    Protected Sub OnPageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvTransDetail.PageIndex = e.NewPageIndex
        Me.listTransDetail()
    End Sub

    Protected Sub lbFindCustID_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbFindCustID.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddTrans", "hideModalAddTrans()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CATab", "CATab()", True)
        listCustomerARCA()
        'listCustomerARCC()
    End Sub

    Protected Sub lbAbortFind_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortFind.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "datetimepicker()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddTrans", "showModalAddTrans()", True)
    End Sub

    Protected Sub gvCustAR_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvCustAR.RowCommand
        If e.CommandName = "getSelect" Then
            Dim a As String = e.CommandArgument
            tbCustID.Text = Strings.Mid(a, 11, 50)
            Session("sCustID") = Strings.Left(a, 10)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "datetimepicker()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddTrans", "showModalAddTrans()", True)
        End If
    End Sub

    Protected Sub lbSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSave.Click
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT TransID FROM iPxAcctAP_TransHdr WHERE TransID = '" & lbTransID.Text & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        If oSQLReader.Read Then
            oSQLReader.Close()
            If oCnct.State = ConnectionState.Closed Then
                oCnct.Open()
            End If
            Dim TransDate As Date
            TransDate = Date.ParseExact(tbTransDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
            oSQLCmd = New SqlCommand(sSQL, oCnct)
            sSQL = "UPDATE iPxAcctAP_TransHdr SET TransDate='" & TransDate & "',InvLink='" & dlFOLink.SelectedValue & "',Description='" & Replace(tbDesc.Text, "'", "''") & "'"
            sSQL = sSQL & "WHERE businessid='" & Session("sBusinessID") & "' and TransID ='" & lbTransID.Text & "'"

            oSQLCmd.CommandText = sSQL
            oSQLCmd.ExecuteNonQuery()

            oCnct.Close()
            If oCnct.State = ConnectionState.Closed Then
                oCnct.Open()
            End If
            oSQLCmd = New SqlCommand(sSQL, oCnct)
            sSQL = "UPDATE iPxAcctAP_Transaction SET TransDate='" & TransDate & "' "
            sSQL = sSQL & "WHERE businessid='" & Session("sBusinessID") & "' and TransID ='" & lbTransID.Text & "'"

            oSQLCmd.CommandText = sSQL
            oSQLCmd.ExecuteNonQuery()

            oCnct.Close()
            Dim TransDateLog As String = Format(Now, "yyy-MM-dd hh:mm:ss")
            cIpx.saveLog(Session("sBusinessID"), lbTransID.Text, TransDateLog, "AP TR", "Update", "Update AP Transaction dari Inventori " & dlFOLink.SelectedItem.ToString, dlFOLink.SelectedValue, Session("sUserCode"))
            Response.Redirect("iPxAPTransaction.aspx")
        Else
            oSQLReader.Close()
            saveARTransHeader()
        End If
    End Sub

    Protected Sub lbAddDetail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAddDetail.Click
        If dlFOLink.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please select FO Link !!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "datetimepicker()", True)
            dlFOLink.Focus()
        ElseIf tbTransDate.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please select Transaction date !!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "datetimepicker()", True)
            tbTransDate.Focus()
        Else
            Session("sRecID") = ""
            transdefault()
            kosong()
            Session("sQueryCust") = ""
            If dlType.Text = "" Then
                lbAddType.Text = "<i class='fa fa-plus' style='font-size:20px;'></i>"
            Else
                lbAddType.Text = "<i class='fa fa-edit' style='font-size:20px;'></i>"
            End If
            lbSaveDetail.Text = "<i class='fa fa-plus'></i> Add"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "datetimepicker()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddTrans", "showModalAddTrans()", True)
        End If
    End Sub

    Protected Sub lbAbortDetail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortDetail.Click
        listTransDetail()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddTrans", "hideModalAddTrans()", True)
    End Sub

    Protected Sub lbSaveDetail_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSaveDetail.Click
        If Session("sRecID") = "" Then
            If tbCustID.Text = "" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please select customer first !!');", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddTrans", "hideModalAddTrans()", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "datetimepicker()", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddTrans", "showModalAddTrans()", True)
            Else
                If oCnct.State = ConnectionState.Closed Then
                    oCnct.Open()
                End If
                oSQLCmd = New SqlCommand(sSQL, oCnct)
                sSQL = "SELECT TransID FROM iPxAcctAP_TransHdr WHERE TransID = '" & lbTransID.Text & "' and businessid='" & Session("sBusinessID") & "'"
                oSQLCmd.CommandText = sSQL
                oSQLReader = oSQLCmd.ExecuteReader

                If oSQLReader.Read Then
                    oSQLReader.Close()
                    saveARTrans()
                Else
                    oSQLReader.Close()
                    saveARTransHeader()
                    saveARTrans()
                End If
            End If
        Else
            If tbCustID.Text = "" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please select customer first !!');", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddTrans", "hideModalAddTrans()", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "datetimepicker()", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddTrans", "showModalAddTrans()", True)
            Else
                If oCnct.State = ConnectionState.Closed Then
                    oCnct.Open()
                End If
                oSQLCmd = New SqlCommand(sSQL, oCnct)
                sSQL = "UPDATE iPxAcctAP_Transaction SET VendorID='" & Session("sCustID") & "',transactiontype='" & dlType.SelectedValue & "',POno='" & Replace(tbPO.Text, "'", "''") & "', "
                sSQL += "RRno='" & Replace(tbRR.Text, "'", "''") & "',notes='" & Replace(tbNotes.Text, "'", "''") & "',amountcr='" & Replace(tbAmount.Text, "'", "''") & "'"
                sSQL = sSQL & "WHERE RecID ='" & Session("sRecID") & "'"

                oSQLCmd.CommandText = sSQL
                oSQLCmd.ExecuteNonQuery()

                oCnct.Close()
                listTransDetail()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('data update successfully !');document.getElementById('Buttonx').click()", True)
                Dim TransDateLog As String = Format(Now, "yyy-MM-dd hh:mm:ss")
                cIpx.saveLog(Session("sBusinessID"), lbTransID.Text, TransDateLog, "AP DT", "Update", "Update AP Transaction detail dari Vendor " & tbCustID.Text, dlFOLink.SelectedValue, Session("sUserCode"))
            End If
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddTrans", "hideModalAddTrans()", True)
    End Sub

    Protected Sub gvTransDetail_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTransDetail.RowCommand
        If e.CommandName = "getEdit" Then
            Session("sRecID") = e.CommandArgument
            tbCustID.Text = ""
            TipeTrans()
            tampilDetail()
            lbSaveDetail.Text = "<i class='fa fa-save'></i> Update"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddTrans", "showModalAddTrans()", True)
        ElseIf e.CommandName = "getDelete" Then
            Session("sRecID") = e.CommandArgument
            tampilDetail()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDeleteTrans", "showModalDeleteTrans()", True)
        End If
    End Sub

    Protected Sub lbAbortDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortDelete.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalDeleteTrans", "hideModalDeleteTrans()", True)
    End Sub

    Protected Sub lbDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbDelete.Click
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "UPDATE iPxAcctAP_Transaction SET isActive='N' WHERE RecID ='" & Session("sRecID") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        listTransDetail()
        Dim TransDateLog As String = Format(Now, "yyy-MM-dd hh:mm:ss")
        cIpx.saveLog(Session("sBusinessID"), lbTransID.Text, TransDateLog, "AP DT", "Delete", "Delete AP Transaction detail dari Vendor " & tbCustID.Text, dlFOLink.SelectedValue, Session("sUserCode"))
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('delete successfully !');document.getElementById('Buttonx').click()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalDeleteTrans", "hideModalDeleteTrans()", True)
    End Sub

    'Protected Sub gvCustARCC_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvCustARCC.RowCommand
    '    If e.CommandName = "getSelect" Then
    '        Dim a As String = e.CommandArgument
    '        tbCustID.Text = Strings.Mid(a, 11, 50)
    '        Session("sCustID") = Strings.Left(a, 10)
    '        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "datetimepicker()", True)
    '        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
    '        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddTrans", "showModalAddTrans()", True)
    '    End If
    'End Sub

    Protected Sub dlType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dlType.SelectedIndexChanged
        If dlType.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddTrans", "hideModalAddTrans()", True)
            lbAddType.Text = "<i class='fa fa-plus' style='font-size:20px;'></i>"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "datetimepicker()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddTrans", "showModalAddTrans()", True)
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddTrans", "hideModalAddTrans()", True)
            lbAddType.Text = "<i class='fa fa-edit' style='font-size:20px;'></i>"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "datetimepicker()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddTrans", "showModalAddTrans()", True)
        End If
    End Sub

    Protected Sub lbAddType_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAddType.Click
        If dlType.Text = " " Then
            tbTransType.Enabled = True
            kosongType()
            lbSaveType.Text = "<i class='fa fa-save'></i> Save"
        Else
            tbTransType.Text = dlType.SelectedValue
            tbTransType.Enabled = False
            editTransType()
            lbSaveType.Text = "<i class='fa fa-save'></i> Update"
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAddTrans", "hideModalAddTrans()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalCreatType", "showModalCreatType()", True)
    End Sub

    Protected Sub lbCloseType_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCloseType.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "datetimepicker()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalCreatType", "hideModalCreatType()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddTrans", "showModalAddTrans()", True)
    End Sub

    Protected Sub lbSearchCoa_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSearchCoa.Click
        listCoa()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalCreatType", "hideModalCreatType()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalCOA", "showModalCOA()", True)
    End Sub

    Protected Sub gvCoa_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvCoa.RowCommand
        If e.CommandName = "getSelect" Then
            tbCoa.Text = e.CommandArgument
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalCOA", "hideModalCOA()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalCreatType", "showModalCreatType()", True)
        End If
    End Sub

    Protected Sub lbCloseCoa_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbCloseCoa.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalCOA", "hideModalCOA()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalCreatType", "showModalCreatType()", True)
    End Sub

    Protected Sub lbSaveType_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSaveType.Click
        If tbTransType.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter Transaction Type!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalCreatType", "hideModalCreatType()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalCreatType", "showModalCreatType()", True)
        ElseIf tbDescription.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter Description!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalCreatType", "hideModalCreatType()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalCreatType", "showModalCreatType()", True)
        ElseIf tbCoa.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter Coa!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalCreatType", "hideModalCreatType()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalCreatType", "showModalCreatType()", True)
        Else
            If oCnct.State = ConnectionState.Closed Then
                oCnct.Open()
            End If
            oSQLCmd = New SqlCommand(sSQL, oCnct)
            sSQL = "SELECT TransactionType FROM iPxAcctAP_Cfg_Transactiontype WHERE businessid ='" & Session("sBusinessID") & "' and TransactionType = '" & tbTransType.Text & "' and isActive='Y'"
            oSQLCmd.CommandText = sSQL
            oSQLReader = oSQLCmd.ExecuteReader

            If oSQLReader.Read Then
                oSQLReader.Close()
                If tbTransType.Enabled = True Then
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('Transaction type duplicate!');", True)
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalCreatType", "hideModalCreatType()", True)
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalCreatType", "showModalCreatType()", True)
                Else
                    updateTransType()
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "datetimepicker()", True)
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalCreatType", "hideModalCreatType()", True)
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddTrans", "showModalAddTrans()", True)
                End If
            Else
                oSQLReader.Close()
                saveTransType()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "datetimepicker()", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalCreatType", "hideModalCreatType()", True)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddTrans", "showModalAddTrans()", True)
            End If
        End If
    End Sub

    Protected Sub lbSearcCust_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbSearcCust.Click
        Session("sQueryCust") = ""
        If tbQCust.Text <> "" Then
            Session("sConditionCust") = Session("sConditionCust") & " and (CoyName like '%" & Replace(tbQCust.Text, "'", "''") & "%') "
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
        listCustomerARCA()
        'listCustomerARCC()
    End Sub

    Protected Sub gvTransDetail_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTransDetail.RowCreated

    End Sub
End Class
