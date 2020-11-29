Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration
Partial Class iPxAdmin_iPxInputARTransaction
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
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.funtionid='2'and x.active='Y' and x.businessid=a.businessid and x.usercode=a.usercode) as edit "
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
        tbArrival.Text = ""
        tbCustID.Text = ""
        tbDeparture.Text = ""
        tbFolio.Text = ""
        tbGuest.Text = ""
        tbNotes.Text = ""
        tbVoucher.Text = ""
        tbRsvdNo.Text = ""
        tbRoom.Text = ""
    End Sub
    Sub TipeTrans()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select * from iPxAcctAR_Cfg_Transactiontype WHERE businessid='" & Session("sBusinessID") & "' and isActive='Y'"
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
        sSQL = "SELECT * FROM iPxAcctAR_Cfg_Transactiontype "
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
        sSQL = "SELECT iPxAcctAR_Cfg_Customer.businessid, iPxAcctAR_Cfg_Customer.CustomerID, iPxAcctAR_Cfg_CustomerGrp.Description AS ARGroup, iPxAcctAR_Cfg_CoyGroup.Description AS CoyGroup, "
        sSQL += "iPxAcctAR_Cfg_Customer.CoyName FROM iPxAcctAR_Cfg_Customer "
        sSQL += "INNER JOIN iPxAcctAR_Cfg_CoyGroup ON iPxAcctAR_Cfg_Customer.businessid = iPxAcctAR_Cfg_CoyGroup.businessid AND iPxAcctAR_Cfg_Customer.CoyGroup = iPxAcctAR_Cfg_CoyGroup.CoyGroup "
        sSQL += "INNER JOIN iPxAcctAR_Cfg_CustomerGrp ON iPxAcctAR_Cfg_Customer.businessid = iPxAcctAR_Cfg_CustomerGrp.businessid AND iPxAcctAR_Cfg_Customer.arGroup = iPxAcctAR_Cfg_CustomerGrp.arGroup "
        sSQL += "where iPxAcctAR_Cfg_Customer.businessid='" & Session("sBusinessID") & "' and iPxAcctAR_Cfg_Customer.IsActive='" & "Y" & "' and iPxAcctAR_Cfg_CustomerGrp.arGroup<>'CC' "
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
    Sub listCustomerARCC()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT iPxAcctAR_Cfg_Customer.businessid, iPxAcctAR_Cfg_Customer.CustomerID, iPxAcctAR_Cfg_CustomerGrp.Description AS ARGroup, iPxAcctAR_Cfg_CoyGroup.Description AS CoyGroup, "
        sSQL += "iPxAcctAR_Cfg_Customer.CoyName FROM iPxAcctAR_Cfg_Customer "
        sSQL += "INNER JOIN iPxAcctAR_Cfg_CoyGroup ON iPxAcctAR_Cfg_Customer.businessid = iPxAcctAR_Cfg_CoyGroup.businessid AND iPxAcctAR_Cfg_Customer.CoyGroup = iPxAcctAR_Cfg_CoyGroup.CoyGroup "
        sSQL += "INNER JOIN iPxAcctAR_Cfg_CustomerGrp ON iPxAcctAR_Cfg_Customer.businessid = iPxAcctAR_Cfg_CustomerGrp.businessid AND iPxAcctAR_Cfg_Customer.arGroup = iPxAcctAR_Cfg_CustomerGrp.arGroup "
        sSQL += "where iPxAcctAR_Cfg_Customer.businessid='" & Session("sBusinessID") & "' and iPxAcctAR_Cfg_Customer.IsActive='" & "Y" & "' and iPxAcctAR_Cfg_CustomerGrp.arGroup='CC' "
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
        sSQL += " order by iPxAcctAR_Cfg_Customer.CustomerID asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvCustARCC.DataSource = dt
                    gvCustARCC.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvCustARCC.DataSource = dt
                    gvCustARCC.DataBind()
                    gvCustARCC.Rows(0).Visible = False
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
        sSQL = "SELECT iPxAcctAR_Transaction.RecID, iPxAcctAR_Cfg_Customer.CoyName, iPxAcctAR_Cfg_Transactiontype.Description, iPxAcctAR_Transaction.invoiceno, iPxAcctAR_Transaction.voucherno, iPxAcctAR_Transaction.foliono, "
        sSQL += "iPxAcctAR_Transaction.GuestName, iPxAcctAR_Transaction.notes, iPxAcctAR_Transaction.RoomNo, iPxAcctAR_Transaction.invoiceno, iPxAcctAR_Transaction.arrival, iPxAcctAR_Transaction.departure, iPxAcctAR_Transaction.amountdr, "
        sSQL += "(select 'Close' from iPxAcctGL_JVhdr as x where x.businessid ='" & Session("sBusinessID") & "' AND x.Status <> 'D' AND x.ReffNo like'PL%' and YEAR(x.TransDate)=YEAR(iPxAcctAR_Transaction.TransDate) AND MONTH(x.TransDate)=MONTH(iPxAcctAR_Transaction.TransDate)) as CloseStatus, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid=iPxAcctAR_Transaction.businessid and x.usercode='" & Session("sUserCode") & "' and x.funtionid='2' and x.active='Y') as edit, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid=iPxAcctAR_Transaction.businessid and x.usercode='" & Session("sUserCode") & "' and x.funtionid='3' and x.active='Y') as deleteAR "
        sSQL += "FROM iPxAcctAR_Transaction "
        sSQL += "LEFT JOIN iPxAcctAR_Cfg_Customer ON iPxAcctAR_Transaction.businessid = iPxAcctAR_Cfg_Customer.businessid AND "
        sSQL += "iPxAcctAR_Transaction.CustomerID = iPxAcctAR_Cfg_Customer.CustomerID "
        sSQL += "LEFT JOIN iPxAcctAR_Cfg_Transactiontype ON iPxAcctAR_Transaction.businessid COLLATE SQL_Latin1_General_CP1_CI_AS = iPxAcctAR_Cfg_Transactiontype.businessid AND "
        sSQL += "iPxAcctAR_Transaction.transactiontype = iPxAcctAR_Cfg_Transactiontype.TransactionType "
        sSQL += "WHERE iPxAcctAR_Transaction.TransID='" & lbTransID.Text & "' and iPxAcctAR_Transaction.isActive = 'Y'"
        sSQL += " order by iPxAcctAR_Transaction.RecID asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvTransDetail.DataSource = dt
                    gvTransDetail.DataBind()
                    Dim totalAmountdr As Decimal
                    If dt.Compute("Sum(amountdr)", "").ToString() <> "" Then
                        totalAmountdr = dt.Compute("Sum(amountdr)", "").ToString()
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
    Sub listTransDetailDelete()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT iPxAcctAR_Transaction.RecID, iPxAcctAR_Cfg_Customer.CoyName, iPxAcctAR_Cfg_Transactiontype.Description, iPxAcctAR_Transaction.invoiceno, iPxAcctAR_Transaction.voucherno, iPxAcctAR_Transaction.foliono, "
        sSQL += "iPxAcctAR_Transaction.GuestName, iPxAcctAR_Transaction.notes, iPxAcctAR_Transaction.RoomNo, iPxAcctAR_Transaction.invoiceno, iPxAcctAR_Transaction.arrival, iPxAcctAR_Transaction.departure, iPxAcctAR_Transaction.amountdr "
        sSQL += "FROM iPxAcctAR_Transaction "
        sSQL += "LEFT JOIN iPxAcctAR_Cfg_Customer ON iPxAcctAR_Transaction.businessid = iPxAcctAR_Cfg_Customer.businessid AND "
        sSQL += "iPxAcctAR_Transaction.CustomerID = iPxAcctAR_Cfg_Customer.CustomerID "
        sSQL += "LEFT JOIN iPxAcctAR_Cfg_Transactiontype ON iPxAcctAR_Transaction.businessid COLLATE SQL_Latin1_General_CP1_CI_AS = iPxAcctAR_Cfg_Transactiontype.businessid AND "
        sSQL += "iPxAcctAR_Transaction.transactiontype = iPxAcctAR_Cfg_Transactiontype.TransactionType "
        sSQL += "WHERE iPxAcctAR_Transaction.TransID='" & lbTransID.Text & "' and iPxAcctAR_Transaction.isActive = 'N'"
        sSQL += " order by iPxAcctAR_Transaction.RecID asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvTransDetail.DataSource = dt
                    gvTransDetail.DataBind()
                    Dim totalAmountdr As Decimal
                    If dt.Compute("Sum(amountdr)", "").ToString() <> "" Then
                        totalAmountdr = dt.Compute("Sum(amountdr)", "").ToString()
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
        sSQL = "SELECT * FROM iPxAcctAR_TransHdr "
        sSQL += "WHERE TransID = '" & Session("sTransID") & "' AND businessid='" & Session("sBusinessID") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            dlFOLink.SelectedValue = oSQLReader.Item("FoLink").ToString
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
        sSQL = "SELECT * FROM iPxAcctAR_Transaction "
        sSQL += "WHERE TransID = '" & lbTransID.Text & "' AND businessid='" & Session("sBusinessID") & "' AND RecID='" & Session("sRecID") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            dlType.SelectedValue = oSQLReader.Item("transactiontype").ToString
            Dim arrival As Date = oSQLReader.Item("arrival").ToString
            tbArrival.Text = arrival.ToString("dd/MM/yyyy")
            Dim departure As Date = oSQLReader.Item("departure").ToString
            tbDeparture.Text = departure.ToString("dd/MM/yyyy")
            Session("sCustID") = oSQLReader.Item("CustomerID").ToString
            tbVoucher.Text = oSQLReader.Item("voucherno").ToString
            tbFolio.Text = oSQLReader.Item("foliono").ToString
            tbGuest.Text = oSQLReader.Item("GuestName").ToString
            tbRoom.Text = oSQLReader.Item("RoomNo").ToString
            tbNotes.Text = oSQLReader.Item("notes").ToString
            tbAmount.Text = String.Format("{0:N2}", (oSQLReader.Item("amountdr"))).ToString
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
        sSQL = "SELECT * FROM iPxAcctAR_Cfg_Customer "
        sSQL += "WHERE CustomerID = '" & Session("sCustID") & "' AND businessid='" & Session("sBusinessID") & "'"
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

        sSQL = "INSERT INTO iPxAcctAR_TransHdr (businessid,TransID,Status,TransDate,FoLink,Description,RegDate,RegBy,isActive) "
        sSQL += "VALUES ('" & Session("sBusinessID") & "','" & lbTransID.Text & "','N','" & transDate & "','" & dlFOLink.SelectedValue & "','" & Replace(tbDesc.Text, "'", "''") & "'"
        sSQL += ",'" & regDate & "','" & Session("iUserID") & "','Y') "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        Dim TransDateLog As String = Format(Now, "yyy-MM-dd hh:mm:ss")
        cIpx.saveLog(Session("sBusinessID"), lbTransID.Text, TransDateLog, "AR TR", "Save", "Create AR Transaction dari " & dlFOLink.SelectedItem.ToString, dlFOLink.SelectedValue, Session("sUserCode"))
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
        Arrival = Date.ParseExact(tbArrival.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
        Departure = Date.ParseExact(tbDeparture.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
        'Else
        'dateBirthday = Date.ParseExact("01/01/1900", "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
        'End If
        sSQL = "INSERT INTO iPxAcctAR_Transaction(businessid,TransID,TransDate,CustomerID,transactiontype,invoiceno,voucherno, "
        sSQL += "foliono,RsvdNo,GuestName,RoomNo,arrival,departure,notes,amountdr,amountcr,amountdr_frgn, amountcr_frgn, currency, FoLink, isActive) "
        sSQL += "VALUES ('" & Session("sBusinessID") & "','" & lbTransID.Text & "','" & Transdate & "','" & Session("sCustID") & "','" & dlType.SelectedValue & "'"
        sSQL += ",'','" & Replace(tbVoucher.Text, "'", "''") & "','" & Replace(tbFolio.Text, "'", "''") & "','" & Replace(tbRsvdNo.Text, "'", "''") & "','" & Replace(tbGuest.Text, "'", "''") & "','" & Replace(tbRoom.Text, "'", "''") & "'"
        sSQL += ",'" & Arrival & "','" & Departure & "','" & Replace(tbNotes.Text, "'", "''") & "','" & Replace(tbAmount.Text, "'", "''") & "','0','0','0','0','" & dlFOLink.SelectedValue & "','Y') "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        listTransDetail()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('data save successfully !');document.getElementById('Buttonx').click()", True)
        Dim TransDateLog As String = Format(Now, "yyy-MM-dd hh:mm:ss")
        cIpx.saveLog(Session("sBusinessID"), lbTransID.Text, TransDateLog, "AR DT", "Save", "Create AR Transaction detail dari " & tbCustID.Text, dlFOLink.SelectedValue, Session("sUserCode"))

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

        sSQL = "INSERT INTO iPxAcctAR_Cfg_Transactiontype (businessid, TransactionType, Description, Coa, isDefault, isActive) "
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
        sSQL = "UPDATE iPxAcctAR_Cfg_Transactiontype SET businessid='" & Session("sBusinessID") & "', Description='" & Replace(tbDescription.Text, "'", "''") & "'"
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
        sSQL = "SELECT * FROM iPxAcctAR_Cfg_Transactiontype "
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
            gvTransDetail.Enabled = False
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
                lbTransID.Text = cIpx.GetCounterMBR("AR", "AR")
                listTransDetail()
                lbStatus.Text = "New"
                lbSave.Text = "<i class='fa fa-save'></i> Save "
                PnReason.Visible = False
                MonthCekClosing = Strings.Mid(tbTransDate.Text, 4, 2)
                YearCekClosing = Strings.Right(tbTransDate.Text, 4)
                cekClose()
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
        listCustomerARCC()
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
        sSQL = "SELECT TransID FROM iPxAcctAR_TransHdr WHERE TransID = '" & lbTransID.Text & "'"
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
            sSQL = "UPDATE iPxAcctAR_TransHdr SET TransDate='" & TransDate & "',FoLink='" & dlFOLink.SelectedValue & "',Description='" & Replace(tbDesc.Text, "'", "''") & "'"
            sSQL = sSQL & "WHERE businessid='" & Session("sBusinessID") & "' and TransID ='" & lbTransID.Text & "'"

            oSQLCmd.CommandText = sSQL
            oSQLCmd.ExecuteNonQuery()

            oCnct.Close()
            If oCnct.State = ConnectionState.Closed Then
                oCnct.Open()
            End If
            oSQLCmd = New SqlCommand(sSQL, oCnct)
            sSQL = "UPDATE iPxAcctAR_Transaction SET TransDate='" & TransDate & "' "
            sSQL = sSQL & "WHERE businessid='" & Session("sBusinessID") & "' and TransID ='" & lbTransID.Text & "'"

            oSQLCmd.CommandText = sSQL
            oSQLCmd.ExecuteNonQuery()

            oCnct.Close()
            Dim TransDateLog As String = Format(Now, "yyy-MM-dd hh:mm:ss")
            cIpx.saveLog(Session("sBusinessID"), lbTransID.Text, TransDateLog, "AR TR", "Update", "Update AR Transaction dari " & dlFOLink.SelectedItem.ToString, dlFOLink.SelectedValue, Session("sUserCode"))
            Response.Redirect("iPxARTransaction.aspx")
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
            tbArrival.Text = Format(DateAdd(DateInterval.Day, -2, Today), "dd/MM/yyyy")
            tbDeparture.Text = Format(DateAdd(DateInterval.Day, -1, Today), "dd/MM/yyyy")
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
                sSQL = "SELECT TransID FROM iPxAcctAR_TransHdr WHERE TransID = '" & lbTransID.Text & "' and businessid='" & Session("sBusinessID") & "'"
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
                Dim Arrival, Departure As Date
                Arrival = Date.ParseExact(tbArrival.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                Departure = Date.ParseExact(tbDeparture.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                oSQLCmd = New SqlCommand(sSQL, oCnct)
                sSQL = "UPDATE iPxAcctAR_Transaction SET CustomerID='" & Session("sCustID") & "',transactiontype='" & dlType.SelectedValue & "',voucherno='" & Replace(tbVoucher.Text, "'", "''") & "', "
                sSQL += "foliono='" & Replace(tbFolio.Text, "'", "''") & "',RsvdNo='" & Replace(tbRsvdNo.Text, "'", "''") & "',GuestName='" & Replace(tbGuest.Text, "'", "''") & "',RoomNo='" & Replace(tbRoom.Text, "'", "''") & "',arrival='" & Arrival & "',departure='" & Departure & "',notes='" & Replace(tbNotes.Text, "'", "''") & "',amountdr='" & Replace(tbAmount.Text, "'", "''") & "'"
                sSQL = sSQL & "WHERE RecID ='" & Session("sRecID") & "'"

                oSQLCmd.CommandText = sSQL
                oSQLCmd.ExecuteNonQuery()

                oCnct.Close()
                listTransDetail()
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('data update successfully !');document.getElementById('Buttonx').click()", True)
                Dim TransDateLog As String = Format(Now, "yyy-MM-dd hh:mm:ss")
                cIpx.saveLog(Session("sBusinessID"), lbTransID.Text, TransDateLog, "AR DT", "Update", "Update AR Transaction detail dari " & tbCustID.Text, dlFOLink.SelectedValue, Session("sUserCode"))
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
        sSQL = "UPDATE iPxAcctAR_Transaction SET isActive='N' WHERE RecID ='" & Session("sRecID") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        listTransDetail()
        Dim TransDateLog As String = Format(Now, "yyy-MM-dd hh:mm:ss")
        cIpx.saveLog(Session("sBusinessID"), lbTransID.Text, TransDateLog, "AR DT", "Delete", "Delete AR Transaction detail dari " & tbCustID.Text, dlFOLink.SelectedValue, Session("sUserCode"))
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('delete successfully !');document.getElementById('Buttonx').click()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalDeleteTrans", "hideModalDeleteTrans()", True)
    End Sub

    Protected Sub gvCustARCC_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvCustARCC.RowCommand
        If e.CommandName = "getSelect" Then
            Dim a As String = e.CommandArgument
            tbCustID.Text = Strings.Mid(a, 11, 50)
            Session("sCustID") = Strings.Left(a, 10)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "datetimepicker()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalAdd", "hideModalAdd()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAddTrans", "showModalAddTrans()", True)
        End If
    End Sub

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
            sSQL = "SELECT TransactionType FROM iPxAcctAR_Cfg_Transactiontype WHERE businessid ='" & Session("sBusinessID") & "' and TransactionType = '" & tbTransType.Text & "' and isActive='Y'"
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
        listCustomerARCC()
    End Sub

    Protected Sub gvTransDetail_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTransDetail.RowCreated

    End Sub
End Class
