Imports System.IO
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports System.Configuration
Partial Class iPxAdmin_iPxARReceipt
    Inherits System.Web.UI.Page
    Dim sCnct As String = ConfigurationManager.ConnectionStrings("iPxCNCT").ToString
    Dim oCnct As SqlConnection = New SqlConnection(sCnct)
    Dim oSQLCmd As SqlCommand
    Dim oSQLReader As SqlDataReader
    Dim sSQL, a, statusinvoice, month, year, RecID, MonthImport, YearImport, coaDebit, coaCredit As String
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
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.funtionid='10'and x.active='Y' and x.businessid=a.businessid and x.usercode=a.usercode) as AddRec "
        sSQL += "from iPxAcct_profile_client_useraccess as a "
        sSQL += "INNER JOIN iPxAcct_profile_client_useraccess_dtl as b ON b.businessid=a.businessid and b.usercode=a.usercode "
        sSQL += "where a.businessid='" & Session("sBusinessID") & "' and a.usercode='" & Session("sUserCode") & "' and a.active='Y' group by a.businessid, a.usercode "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            If oSQLReader.Item("AddRec").ToString = "Y" Then
                lbAddRec.Enabled = True
            Else
                lbAddRec.Enabled = False
            End If
        Else
            lbAddRec.Enabled = False
        End If
        oCnct.Close()
    End Sub
    Sub showdata_dropdownCustStatus()
        dlQStatus.Items.Insert(0, "")
        dlQStatus.Items.Insert(1, "New")
        dlQStatus.Items.Insert(2, "Delete")
    End Sub
    Sub CoyGroup()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcctAR_Cfg_CoyGroup where businessid = '" & Session("sBusinessID") & "'"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                dlQCoyGroup.DataSource = dt
                dlQCoyGroup.DataTextField = "Description"
                dlQCoyGroup.DataValueField = "CoyGroup"
                dlQCoyGroup.DataBind()
                dlQCoyGroup.Items.Insert(0, "")
            End Using
        End Using
    End Sub
    Sub country()
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPx_profile_geog_country order by country asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                dlQCountry.DataSource = dt
                dlQCountry.DataTextField = "country"
                dlQCountry.DataValueField = "countryid"
                dlQCountry.DataBind()
                dlQCountry.Items.Insert(0, "")
            End Using
        End Using
        'dlQCountry.SelectedValue = cIpx.getDefaultParameter(Session("sBusinessID"), "01")
    End Sub
    Sub province()
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPx_profile_geog_province where countryid ='" & dlQCountry.SelectedValue.Trim & "' order by description asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                dlQProvince.DataSource = dt
                dlQProvince.DataTextField = "description"
                dlQProvince.DataValueField = "provid"
                dlQProvince.DataBind()
                dlQProvince.Items.Insert(0, "")
            End Using
        End Using
        'dlQCity.SelectedValue = cIpx.getDefaultParameter(Session("sBusinessID"), "03")
    End Sub
    Sub city()
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPx_profile_geog_city where countryid ='" & dlQCountry.SelectedValue.Trim & "' and provid = '" & dlQProvince.SelectedValue.Trim & "' order by city asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                dlQCity.DataSource = dt
                dlQCity.DataTextField = "city"
                dlQCity.DataValueField = "cityid"
                dlQCity.DataBind()
                dlQCity.Items.Insert(0, "")
            End Using
        End Using
        'dlQCity.SelectedValue = cIpx.getDefaultParameter(Session("sBusinessID"), "04")
    End Sub
    Sub Paidby()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select * from iPxAcctAR_Cfg_Paidby WHERE businessid='" & Session("sBusinessID") & "'"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                dlPaid.DataSource = dt
                dlPaid.DataTextField = "Description"
                dlPaid.DataValueField = "PaidBy"
                dlPaid.DataBind()
                dlPaid.Items.Insert(0, "")
            End Using
        End Using
    End Sub
    Sub ListReceipt()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        month = Left(tbDate.Text, 2)
        year = Right(tbDate.Text, 4)
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.*,(SELECT sum(amountcr) as total FROM iPxAcctAR_Transaction where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' AND iPxAcctAR_Transaction.TransID = a.ReceiptID) as Amount, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid='" & Session("sBusinessID") & "' and x.usercode='" & Session("sUserCode") & "' and x.funtionid='11' and x.active='Y') as editRec, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid='" & Session("sBusinessID") & "' and x.usercode='" & Session("sUserCode") & "' and x.funtionid='12' and x.active='Y') as deleteRec, "
        sSQL += "(select 'Y' from iPxAcct_profile_client_useraccess_dtl as x where x.businessid='" & Session("sBusinessID") & "' and x.usercode='" & Session("sUserCode") & "' and x.funtionid='13' and x.active='Y') as postRec "
        sSQL += "from (SELECT iPxAcctAR_Receipt.ReceiptID, iPxAcctAR_Receipt.ReceiptDate, iPxAcctAR_Cfg_Customer.CoyName, iPxAcctAR_Receipt.ReffNo, "
        sSQL += "iPxAcctAR_Cfg_Paidby.Description, iPxAcctAR_Receipt.Notes, iPx_profile_user.fullname,iPxAcctAR_Receipt.Status "
        sSQL += "FROM iPxAcctAR_Receipt "
        sSQL += "INNER JOIN iPxAcctAR_Cfg_Paidby ON iPxAcctAR_Receipt.PaidBy = iPxAcctAR_Cfg_Paidby.PaidBy AND iPxAcctAR_Receipt.businessid = dbo.iPxAcctAR_Cfg_Paidby.businessid "
        sSQL += "INNER JOIN iPxAcctAR_Cfg_Customer ON iPxAcctAR_Receipt.CustomerID COLLATE Latin1_General_CI_AS = iPxAcctAR_Cfg_Customer.CustomerID AND "
        sSQL += "iPxAcctAR_Receipt.businessid COLLATE Latin1_General_CI_AS = iPxAcctAR_Cfg_Customer.businessid "
        sSQL += "INNER JOIN iPx_profile_user ON iPxAcctAR_Receipt.RegBy = iPx_profile_user.id "
        sSQL += "where iPxAcctAR_Receipt.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Cfg_Customer.ArGroup<>'CC' "
        If Session("sQueryTicket") = "" Then
            Session("sQueryTicket") = Session("sCondition")
            If Session("sQueryTicket") <> "" Or Session("sCondition") <> "" Then
                sSQL = sSQL & Session("sQueryTicket")
                Session("sCondition") = ""
            Else
                sSQL += " AND month(iPxAcctAR_Receipt.ReceiptDate)='" & month & "' AND year(iPxAcctAR_Receipt.ReceiptDate)='" & year & "' AND iPxAcctAR_Receipt.Status<>'X'"
            End If
        Else
            sSQL = sSQL & Session("sQueryTicket")
            Session("sCondition") = ""
        End If
        sSQL += " ) a order by a.ReceiptID desc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvARRec.DataSource = dt
                    gvARRec.DataBind()
                    Dim totalAmount As Decimal
                    If dt.Compute("Sum(Amount)", "").ToString() <> "" Then
                        totalAmount = dt.Compute("Sum(Amount)", "").ToString()
                    Else
                        totalAmount = 0
                    End If
                    gvARRec.FooterRow.Cells(4).Text = "Total"
                    gvARRec.FooterRow.Cells(4).HorizontalAlign = HorizontalAlign.Right
                    gvARRec.FooterRow.Cells(5).Text = totalAmount.ToString("N2")
                    gvARRec.FooterRow.Cells(5).HorizontalAlign = HorizontalAlign.Right
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvARRec.DataSource = dt
                    gvARRec.DataBind()
                    gvARRec.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub listCustomer()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT iPxAcctAR_Cfg_Customer.businessid, iPxAcctAR_Cfg_Customer.CustomerID, iPxAcctAR_Cfg_CustomerGrp.Description AS ARGroup, iPxAcctAR_Cfg_CoyGroup.Description AS CoyGroup, "
        sSQL += "iPxAcctAR_Cfg_Customer.CoyName, iPxAcctAR_Cfg_Customer.Address, iPxAcctAR_Cfg_Customer.BilllingAddress, "
        sSQL += "iPx_profile_geog_country.country, iPx_profile_geog_province.description AS Province, iPx_profile_geog_city.city, iPxAcctAR_Cfg_Customer.Phone, "
        sSQL += "iPxAcctAR_Cfg_Customer.Email, (select (sum(amountdr)-sum(amountcr)) from iPxAcctAR_Transaction as c where c.CustomerID=iPxAcctAR_Cfg_Customer.CustomerID and c.invoiceno<>'' and c.isActive='Y') as amountTrans FROM iPxAcctAR_Cfg_Customer "
        sSQL += "INNER JOIN iPxAcctAR_Cfg_CoyGroup ON iPxAcctAR_Cfg_Customer.businessid = iPxAcctAR_Cfg_CoyGroup.businessid AND iPxAcctAR_Cfg_Customer.CoyGroup = iPxAcctAR_Cfg_CoyGroup.CoyGroup "
        sSQL += "INNER JOIN iPxAcctAR_Cfg_CustomerGrp ON iPxAcctAR_Cfg_Customer.businessid = iPxAcctAR_Cfg_CustomerGrp.businessid AND iPxAcctAR_Cfg_Customer.arGroup = iPxAcctAR_Cfg_CustomerGrp.arGroup "
        sSQL += "INNER JOIN iPx_profile_geog_country ON iPxAcctAR_Cfg_Customer.CountryId = iPx_profile_geog_country.countryid "
        sSQL += "INNER JOIN iPx_profile_geog_province ON iPx_profile_geog_country.countryid = iPx_profile_geog_province.countryid AND "
        sSQL += "iPxAcctAR_Cfg_Customer.provid COLLATE SQL_Latin1_General_CP1_CI_AS = iPx_profile_geog_province.provid "
        sSQL += "INNER JOIN iPx_profile_geog_city ON iPx_profile_geog_province.countryid = iPx_profile_geog_city.countryid AND "
        sSQL += "iPx_profile_geog_province.provid = iPx_profile_geog_city.provid AND iPxAcctAR_Cfg_Customer.CityID = iPx_profile_geog_city.cityid "
        sSQL += "where iPxAcctAR_Cfg_Customer.businessid ='" & Session("sBusinessID") & "' AND iPxAcctAR_Cfg_Customer.CustomerID = (select CustomerID from iPxAcctAR_Transaction "
        sSQL += "where iPxAcctAR_Transaction.CustomerID = iPxAcctAR_Cfg_Customer.CustomerID and iPxAcctAR_Cfg_Customer.ArGroup<>'CC' and invoiceno like 'IV%' and iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' group by CustomerID) "
        sSQL += "and (select (sum(amountdr)-sum(amountcr)) from iPxAcctAR_Transaction as c where c.CustomerID=iPxAcctAR_Cfg_Customer.CustomerID and c.invoiceno<>'' and c.isActive='Y')>0 "
        sSQL += "AND iPxAcctAR_Cfg_Customer.IsActive='" & "Y" & "' "
        If Session("sQueryCus") = "" Then
            Session("sQueryCus") = Session("sConditionCus")
            If Session("sQueryCus") <> "" Or Session("sConditionCus") <> "" Then
                sSQL = sSQL & Session("sQueryCus")
                Session("sConditionCus") = ""
            Else
                sSQL = sSQL & ""
            End If
        Else
            sSQL = sSQL & Session("sQueryCus")
            Session("sConditionCus") = ""
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
                    Dim totalAmount As Decimal
                    If dt.Compute("Sum(amountTrans)", "").ToString() <> "" Then
                        totalAmount = dt.Compute("Sum(amountTrans)", "").ToString()
                    Else
                        totalAmount = 0
                    End If
                    gvCustAR.FooterRow.Cells(7).Text = "Total"
                    gvCustAR.FooterRow.Cells(7).HorizontalAlign = HorizontalAlign.Right
                    gvCustAR.FooterRow.Cells(8).Text = totalAmount.ToString("N2")
                    gvCustAR.FooterRow.Cells(8).HorizontalAlign = HorizontalAlign.Right
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
    Sub ListInvoiceEdit()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT iPxAcctAR_Invoice.*, iPxAcctAR_Cfg_Customer.CoyName, iPx_profile_user.fullname, "
        sSQL += "(select sum(amountdr) from iPxAcctAR_Transaction "
        sSQL += "where CustomerID='" & Session("sReceiptInv") & "' AND transactiontype <>'RC' and iPxAcctAR_Transaction.invoiceno = iPxAcctAR_Invoice.InvoiceNo) as Debit "
        sSQL += ",(select sum(amountcr) from iPxAcctAR_Transaction "
        sSQL += "where CustomerID='" & Session("sReceiptInv") & "' AND transactiontype ='RC' and iPxAcctAR_Transaction.invoiceno = iPxAcctAR_Invoice.InvoiceNo) as Credit "
        sSQL += ",(select (sum(amountdr)-sum(amountcr)) from iPxAcctAR_Transaction where invoiceno = iPxAcctAR_Invoice.InvoiceNo) as totalInvoice FROM iPxAcctAR_Invoice "
        sSQL += "INNER JOIN iPxAcctAR_Cfg_Customer ON iPxAcctAR_Invoice.businessid = iPxAcctAR_Cfg_Customer.businessid COLLATE SQL_Latin1_General_CP1_CI_AS AND "
        sSQL += "iPxAcctAR_Invoice.CustomerID COLLATE Latin1_General_CI_AS = iPxAcctAR_Cfg_Customer.CustomerID "
        sSQL += "INNER JOIN iPx_profile_user ON iPxAcctAR_Invoice.RegBy = iPx_profile_user.id "
        sSQL += "where iPxAcctAR_Invoice.businessid ='" & Session("sBusinessID") & "' and iPxAcctAR_Invoice.CustomerID ='" & Session("sReceiptInv") & "' "
        If Session("sQueryTicket") = "" Then
            Session("sQueryTicket") = Session("sCondition")
            If Session("sQueryTicket") <> "" Or Session("sCondition") <> "" Then
                sSQL = sSQL & Session("sQueryTicket")
                Session("sCondition") = ""
            Else
                sSQL += " AND iPxAcctAR_Invoice.Status<>'X'"
            End If
        Else
            sSQL = sSQL & Session("sQueryTicket")
            Session("sCondition") = ""
        End If
        sSQL += " order by iPxAcctAR_Invoice.DueDate asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvARInv.DataSource = dt
                    gvARInv.DataBind()
                    Dim totalCredit, total As Decimal
                    Dim totalDebit As Decimal = dt.Compute("Sum(Debit)", "").ToString()
                    If dt.Compute("Sum(Credit)", "").ToString() = "" Then
                        totalCredit = 0
                    Else
                        totalCredit = dt.Compute("Sum(Credit)", "").ToString()
                    End If
                    If dt.Compute("Sum(totalInvoice)", "").ToString() = "" Then
                        total = 0
                    Else
                        total = dt.Compute("Sum(totalInvoice)", "").ToString()
                    End If
                    gvARInv.FooterRow.Cells(6).Text = "Total"
                    gvARInv.FooterRow.Cells(6).HorizontalAlign = HorizontalAlign.Left
                    gvARInv.FooterRow.Cells(7).Text = totalDebit.ToString("N2")
                    gvARInv.FooterRow.Cells(7).HorizontalAlign = HorizontalAlign.Right
                    gvARInv.FooterRow.Cells(8).Text = totalCredit.ToString("N2")
                    gvARInv.FooterRow.Cells(8).HorizontalAlign = HorizontalAlign.Right
                    gvARInv.FooterRow.Cells(9).Text = total.ToString("N2")
                    gvARInv.FooterRow.Cells(9).HorizontalAlign = HorizontalAlign.Right
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvARInv.DataSource = dt
                    gvARInv.DataBind()
                    gvARInv.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub listDetail()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select iPxAcctAR_Transaction.TransID, iPxAcctAR_Transaction.TransDate,iPxAcctAR_Cfg_Customer.CoyName,iPxAcctAR_Transaction.invoiceno,iPxAcctAR_Transaction.amountcr from iPxAcctAR_Transaction "
        sSQL += "INNER JOIN iPxAcctAR_Cfg_Customer on iPxAcctAR_Cfg_Customer.CustomerID=iPxAcctAR_Transaction.CustomerID "
        sSQL += "where iPxAcctAR_Transaction.businessid ='" & Session("sBusinessID") & "' AND iPxAcctAR_Transaction.transactiontype='RC' and iPxAcctAR_Transaction.TransID ='" & Session("sEditRece") & "' "
        sSQL += " order by iPxAcctAR_Transaction.invoiceno asc"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvDetail.DataSource = dt
                    gvDetail.DataBind()
                    Dim total As Decimal
                    If dt.Compute("Sum(amountcr)", "").ToString() = "" Then
                        total = 0
                    Else
                        total = dt.Compute("Sum(amountcr)", "").ToString()
                    End If
                    gvDetail.FooterRow.Cells(3).Text = "Total"
                    gvDetail.FooterRow.Cells(3).HorizontalAlign = HorizontalAlign.Left
                    gvDetail.FooterRow.Cells(4).Text = total.ToString("N2")
                    gvDetail.FooterRow.Cells(4).HorizontalAlign = HorizontalAlign.Right
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvDetail.DataSource = dt
                    gvDetail.DataBind()
                    gvDetail.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Sub editReceipt()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcctAR_Receipt "
        sSQL += "where iPxAcctAR_Receipt.businessid ='" & Session("sBusinessID") & "' "
        sSQL += "AND iPxAcctAR_Receipt.ReceiptID = '" & Session("sEditRece") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            Session("sReceiptInv") = oSQLReader.Item("CustomerID").ToString
            oCnct.Close()
        Else
            oCnct.Close()
        End If
    End Sub
    Sub DeleteReceipt()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "UPDATE iPxAcctAR_Receipt SET status='X' "
        sSQL = sSQL & "WHERE ReceiptID ='" & Session("sSelectCustBathD") & "' and businessid='" & Session("sBusinessID") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()

        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "UPDATE iPxAcctAR_Transaction SET isActive='N'"
        sSQL = sSQL & "WHERE TransID ='" & Session("sSelectCustBathD") & "' and businessid='" & Session("sBusinessID") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Sub
    Sub InvoiceNo()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.invoiceno from iPxAcctAR_Transaction as a "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' "
        sSQL += "AND a.TransID = '" & Session("sSelectCustBathD") & "'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            Dim invDel As String = oSQLReader.Item("invoiceno").ToString
            oCnct.Close()
            findInvPaid(invDel)
        Else
            oCnct.Close()
        End If
    End Sub
    Public Function findInvPaid(ByVal invNo As String) As Boolean
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select a.invoiceno from iPxAcctAR_Transaction as a "
        sSQL += "where a.businessid ='" & Session("sBusinessID") & "' "
        sSQL += "AND a.invoiceno = '" & invNo & "' and TransID like 'RC%' and isActive='Y'"
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader

        oSQLReader.Read()
        If oSQLReader.HasRows Then
            oCnct.Close()
            statusinvoice = "P"
            updateInvoice(invNo)
        Else
            oCnct.Close()
            statusinvoice = "N"
            updateInvoice(invNo)
        End If
    End Function
    Public Function updateInvoice(ByVal invNo As String) As Boolean
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "UPDATE iPxAcctAR_Invoice SET Status='" & statusinvoice & "'"
        sSQL = sSQL & " WHERE InvoiceNo ='" & invNo & "' and businessid='" & Session("sBusinessID") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
    End Function
    Sub saveReason()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        Dim regDate As Date = Date.Now()
        sSQL = "INSERT INTO iPxAcct_ReasonDelete(businessid,idAcct,dateReason,RegBy,reason,isActive)"
        sSQL += "VALUES ('" & Session("sBusinessID") & "','" & Session("sSelectCustBathD") & "','" & regDate & "'"
        sSQL += ",'" & Session("iUserID") & "','" & Replace(tbReason.Text, "'", "''") & "','Y') "
        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        
    End Sub
    Public Function Posting(ByVal businessid As String, ByVal TransID As String) As Boolean

        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "SELECT * FROM iPxAcctAR_Receipt where businessid ='" & businessid & "' and ReceiptID='" & TransID & "' "
        oSQLCmd.CommandText = sSQL
        oSQLReader = oSQLCmd.ExecuteReader
        While oSQLReader.Read
            'oSQLReader.Close()
            Dim GLid As String = cIpx.GetCounterMBR("GL", "GL")
            saveGLHeader(GLid, businessid, TransID, oSQLReader.Item("ReceiptDate"), oSQLReader.Item("Notes"))
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
        sSQL1 += "'O','G2','" & TransID & "','" & GLDesc & "') "
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
        sSQL2 = "SELECT a.RecID, b.CustomerID, b.CoyName, b.CoyName, (b.CoaLink) as coaCredit, d.Description, (d.Coa) as coaDebit, a.GuestName, a.amountcr "
        sSQL2 += "FROM iPxAcctAR_Transaction as a "
        sSQL2 += "INNER JOIN iPxAcctAR_Cfg_Customer as b ON a.businessid = b.businessid AND a.CustomerID = b.CustomerID "
        sSQL2 += "INNER JOIN iPxAcctAR_Receipt as c ON c.businessid=a.businessid and c.ReceiptID=a.TransID "
        sSQL2 += "INNER JOIN iPxAcctAR_Cfg_Paidby as d ON d.businessid=c.businessid and d.PaidBy=c.PaidBy "
        sSQL2 += "where a.businessid ='" & businessid & "' and a.TransID='" & TransID & "' and a.isActive = 'Y' order by a.RecID asc "
        oSQLCmd2.CommandText = sSQL2
        oSQLReader2 = oSQLCmd2.ExecuteReader
        While oSQLReader2.Read
            saveGLDetailDebit(businessid, GLid, oSQLReader2.Item("coaDebit"), oSQLReader2.Item("CoyName"), TransID, oSQLReader2.Item("amountcr"))
            saveGLDetailCredit(businessid, GLid, oSQLReader2.Item("coaCredit"), oSQLReader2.Item("CoyName"), TransID, oSQLReader2.Item("amountcr"))
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
        sSQL3 += "'" & Desc & "','" & Reff & "','" & Amount & "','0', "
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
        sSQL3 += "'" & Desc & "','" & Reff & "','0','" & Amount & "', "
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
        sSQL = "UPDATE iPxAcctAR_Receipt SET Status='P'"
        sSQL = sSQL & "WHERE businessid ='" & Session("sBusinessID") & "' and ReceiptID='" & Session("sReceiptID") & "'"

        oSQLCmd.CommandText = sSQL
        oSQLCmd.ExecuteNonQuery()

        oCnct.Close()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('the data was successfully posted !!');", True)
        ListReceipt()
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
            Posting(Session("sBusinessID"), Session("sReceiptID"))
            EditPosting()
            Dim TransDateLog As String = Format(Now, "yyy-MM-dd hh:mm:ss")
            cIpx.saveLog(Session("sBusinessID"), Session("sReceiptID"), TransDateLog, "AR REC", "Posting", "Post AR Receipt " & Session("sReceiptID"), "", Session("sUserCode"))
        End If
        oCnct4.Close()
    End Sub
    Sub Docket()
        If oCnct.State = ConnectionState.Closed Then
            oCnct.Open()
        End If
        oSQLCmd = New SqlCommand(sSQL, oCnct)
        sSQL = "select * from iPxAcctDocket_Report WHERE businessid in ('','" & Session("sBusinessID") & "') and Grp='RC'"
        Using sda As New SqlDataAdapter()
            oSQLCmd.CommandText = sSQL
            sda.SelectCommand = oSQLCmd
            Using dt As New DataTable()
                sda.Fill(dt)
                If dt.Rows.Count <> 0 Then
                    gvDocket.DataSource = dt
                    gvDocket.DataBind()
                Else
                    dt.Rows.Add(dt.NewRow())
                    gvDocket.DataSource = dt
                    gvDocket.DataBind()
                    gvDocket.Enabled = False
                    gvDocket.Rows(0).Visible = False
                End If
            End Using
        End Using
        oCnct.Close()
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("sBusinessID") = "" Then
            Response.Redirect("signin.aspx")
        End If
        If Not Me.IsPostBack Then
            If cIpx.getAccessUser(Session("sBusinessID"), Session("sUserCode"), "AR Receipt") <> True Then

                Session("sMessage") = "Sorry, you dont have access in this module |"
                Session("sMemberid") = ""
                Session("sWarningID") = "0"
                Session("sUrlOKONLY") = "home.aspx"
                Session("sUrlYES") = "http://www.thepyxis.net"
                Session("sUrlNO") = "http://www.thepyxis.net"
                Response.Redirect("warningmsg.aspx")
            End If
            Session("sQueryTicket") = ""
            Session("sQueryCus") = ""
            If Session("sDateWorkARRec") = "" Then
                tbDate.Text = Format(Now, "MM-yyyy")
            Else
                tbDate.Text = Session("sDateWorkARRec")
            End If
            ListReceipt()
        Else
            ListReceipt()
        End If
        UserAcces()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "MonthGL", "$(document).ready(function() {MonthGL()});", True)
    End Sub
    Protected Sub TransdateWork(ByVal sender As Object, ByVal e As EventArgs)
        Session("sDateWorkARRec") = tbDate.Text
        Session("sQueryTicket") = ""
    End Sub
    Protected Sub OnPaging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvARRec.PageIndex = e.NewPageIndex
        Me.ListReceipt()
        gvCustAR.PageIndex = e.NewPageIndex
        Me.listCustomer()
    End Sub

    Protected Sub gvARRec_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvARRec.PageIndexChanging
        gvARRec.PageIndex = e.NewPageIndex
        ListReceipt()
        pnCustGroup.Visible = False
        pnARRec.Visible = True
    End Sub

    Protected Sub OnPageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        gvARRec.PageIndex = e.NewPageIndex
        Me.ListReceipt()
        gvCustAR.PageIndex = e.NewPageIndex
        Me.listCustomer()
    End Sub
    Protected Sub lbAddRec_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAddRec.Click
        pnCustGroup.Visible = True
        pnARRec.Visible = False
        listCustomer()
        Me.lbAddRec.Enabled = False
        lbView.Visible = True
    End Sub

    Protected Sub gvCustAR_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvCustAR.PageIndexChanging
        gvCustAR.PageIndex = e.NewPageIndex
        listCustomer()
        pnCustGroup.Visible = True
        pnARRec.Visible = False
    End Sub

    Protected Sub gvCustAR_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvCustAR.RowCommand
        If e.CommandName = "getSelect" Then
            Session("sReceiptInv") = e.CommandArgument
            Session("sEditRece") = ""
            Response.Redirect("iPxInputARReceipt.aspx")
        End If
    End Sub

    Protected Sub gvARRec_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvARRec.RowCommand
        If e.CommandName = "getEdit" Then
            Session("sEditRece") = e.CommandArgument
            Response.Redirect("iPxInputARReceipt.aspx")
        ElseIf e.CommandName = "getDetail" Then
            Session("sEditRece") = e.CommandArgument
            pnListDetail.Visible = True
            pnListDep.Visible = False
            listDetail()
            lbTitle.Text = "Detail Receipt"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
        ElseIf e.CommandName = "getDeposito" Then
            Session("sEditRece") = e.CommandArgument
            pnListDep.Visible = True
            pnListDetail.Visible = False
            editReceipt()
            ListInvoiceEdit()
            lbTitle.Text = "Saldo Balance"
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalAdd", "showModalAdd()", True)
        ElseIf e.CommandName = "getPrint" Then
            'Dim cIpx As New iPxClass
            'Session("sReport") = "ARRecept"
            Session("sTransID") = e.CommandArgument
            'Session("sMapPath") = "~/iPxReportFile/dckAR_Receipt.rpt"
            'Response.Redirect("rptviewer.aspx")
            Docket()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDocket", "showModalDocket()", True)
        ElseIf e.CommandName = "getDelete" Then
            Session("sSelectCustBathD") = e.CommandArgument
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDeleteTrans", "showModalDeleteTrans()", True)
        ElseIf e.CommandName = "getPosting" Then
            Session("sReceiptID") = e.CommandArgument
            If oCnct.State = ConnectionState.Closed Then
                oCnct.Open()
            End If
            oSQLCmd = New SqlCommand(sSQL, oCnct)
            sSQL = "SELECT a.TransID, b.ReceiptDate, "
            sSQL += "(SELECT COUNT(b.CoaLink) FROM iPxAcctAR_Transaction as a "
            sSQL += "INNER JOIN iPxAcctAR_Cfg_Customer as b ON a.businessid = b.businessid AND a.CustomerID = b.CustomerID "
            sSQL += "where a.businessid ='" & Session("sBusinessID") & "' and a.TransID='" & Session("sReceiptID") & "' and a.isActive = 'Y' and b.CoaLink='') as coaCredit, "
            sSQL += "(SELECT COUNT(c.Coa) FROM iPxAcctAR_Receipt as a INNER JOIN iPxAcctAR_Cfg_Paidby as c "
            sSQL += " ON a.businessid COLLATE SQL_Latin1_General_CP1_CI_AS = c.businessid "
            sSQL += "AND a.PaidBy = c.PaidBy  where a.businessid ='" & Session("sBusinessID") & "' and a.ReceiptID='" & Session("sReceiptID") & "' and a.isActive = 'Y' and c.Coa='') as coaDebit "
            sSQL += "FROM iPxAcctAR_Transaction as a "
            sSQL += "INNER JOIN iPxAcctAR_Receipt as b ON b.businessid=a.businessid and b.ReceiptID=a.TransID "
            sSQL += "WHERE a.TransID = '" & Session("sReceiptID") & "' and a.businessid='" & Session("sBusinessID") & "' group by a.TransID, b.ReceiptDate"
            oSQLCmd.CommandText = sSQL
            oSQLReader = oSQLCmd.ExecuteReader

            While oSQLReader.Read
                coaCredit = oSQLReader.Item("coaCredit").ToString
                coaDebit = oSQLReader.Item("coaDebit").ToString
                MonthImport = Strings.Mid(Format(oSQLReader.Item("ReceiptDate"), "dd-MM-yyy").ToString, 4, 2)
                YearImport = Strings.Right(Format(oSQLReader.Item("ReceiptDate"), "dd-MM-yyy").ToString, 4)
            End While
            oSQLReader.Close()
            If MonthImport <> "" And coaCredit = "0" And coaDebit = "0" Then
                cekCloseGrid()
            ElseIf MonthImport = "" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('posting process failed !');document.getElementById('Buttonx').click()", True)
            ElseIf coaCredit <> "0" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Posting process failed, COA Credit is empty !');document.getElementById('Buttonx').click()", True)
            ElseIf coaDebit <> "0" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alertMessage", "alert('Posting process failed, COA Debit is empty !');document.getElementById('Buttonx').click()", True)
            End If
        End If
    End Sub

    Protected Sub lbQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbQuery.Click
        If lbView.Visible = False Then
            pnRecept.Visible = True
            pnCust.Visible = False
            Paidby()
            tbQCus.Text = ""
            tbQFrom.Text = ""
            tbQRecDate.Text = ""
            tbQRecID.Text = ""
            tbQReff.Text = ""
            tbQUntil.Text = ""
            dlQStatus.Items.Clear()
            showdata_dropdownCustStatus()
            Session("sQueryTicket") = ""
        Else
            Session("sQueryCus") = ""
            pnRecept.Visible = False
            pnCust.Visible = True
            CoyGroup()
            country()
            province()
            city()
            tbQCoyName.Text = ""
            tbQEmail.Text = ""
        End If
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "datetimepicker", "datetimepicker()", True)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalQuery", "showModalQuery()", True)
    End Sub

    Protected Sub lbAbortQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortQuery.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQuery", "hideModalQuery()", True)
    End Sub

    Protected Sub lblQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblQuery.Click
        If lbView.Visible = False Then
            If dlQStatus.SelectedIndex = 0 Then
                If tbQRecDate.Text = "" And tbQFrom.Text = "" And tbQUntil.Text = "" Then
                    month = Left(tbDate.Text, 2)
                    year = Right(tbDate.Text, 4)
                    Session("sCondition") = Session("sCondition") & " AND month(iPxAcctAR_Receipt.ReceiptDate)='" & month & "' AND year(iPxAcctAR_Receipt.ReceiptDate)='" & year & "' AND iPxAcctAR_Receipt.Status<>'X' "
                ElseIf tbQRecDate.Text <> "" Or tbQFrom.Text <> "" Or tbQUntil.Text <> "" Then
                    Session("sCondition") = Session("sCondition") & " AND iPxAcctAR_Receipt.Status<>'X' "
                    tbDate.Text = Format(Now, "MM-yyyy")
                    Session("sDateWorkARRec") = tbDate.Text
                End If
            ElseIf dlQStatus.SelectedIndex = 1 Then
                Session("sCondition") = Session("sCondition") & " and iPxAcctAR_Receipt.Status = 'N' "
            ElseIf dlQStatus.SelectedIndex = 2 Then
                Session("sCondition") = Session("sCondition") & " and iPxAcctAR_Receipt.Status = 'X'"
            End If
            If tbQRecID.Text.Trim <> "" Then
                Session("sCondition") = Session("sCondition") & " and (iPxAcctAR_Receipt.ReceiptID like '%" & Replace(tbQRecID.Text.Trim, "'", "''") & "%') "
            End If
            If tbQCus.Text.Trim <> "" Then
                Session("sCondition") = Session("sCondition") & " and (iPxAcctAR_Cfg_Customer.CoyName like '%" & Replace(tbQCus.Text.Trim, "'", "''") & "%') "
            End If
            If tbQReff.Text.Trim <> "" Then
                Session("sCondition") = Session("sCondition") & " and (iPxAcctAR_Receipt.ReffNo like '%" & Replace(tbQReff.Text.Trim, "'", "''") & "%') "
            End If
            If dlPaid.Text.Trim <> "" Then
                Session("sCondition") = Session("sCondition") & " and (iPxAcctAR_Receipt.PaidBy = '" & dlPaid.SelectedValue & "') "
            End If
            If tbQRecDate.Text <> "" Then
                Dim RecDate As Date
                RecDate = Date.ParseExact(tbQRecDate.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                Session("sCondition") = Session("sCondition") & " and (iPxAcctAR_Receipt.ReceiptDate = '" & RecDate & "') "
            End If
            If tbQFrom.Text.Trim <> "" And tbQUntil.Text.Trim <> "" Then
                Dim PerFrom As Date = Date.ParseExact(tbQFrom.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                Dim PerUntl As Date = Date.ParseExact(tbQUntil.Text, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)
                Session("sCondition") = Session("sCondition") & " AND (iPxAcctAR_Receipt.ReceiptDate >= '" & PerFrom & "') AND (iPxAcctAR_Receipt.ReceiptDate <= '" & PerUntl & "') "
            End If
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQuery", "hideModalQuery()", True)
            ListReceipt()
        Else
            If dlQCoyGroup.Text.Trim <> "" Then
                Session("sConditionCus") = Session("sConditionCus") & " and (iPxAcctAR_Cfg_Customer.CoyGroup = '" & dlQCoyGroup.SelectedValue & "') "
            End If
            If tbQCoyName.Text.Trim <> "" Then
                Session("sConditionCus") = Session("sConditionCus") & " and (iPxAcctAR_Cfg_Customer.CoyName like '%" & Replace(tbQCoyName.Text, "'", "''") & "%') "
            End If
            If dlQCountry.Text.Trim <> "" Then
                Session("sConditionCus") = Session("sConditionCus") & " and (iPxAcctAR_Cfg_Customer.CountryId = '" & dlQCountry.SelectedValue & "') "
            End If
            If dlQProvince.Text.Trim <> "" Then
                Session("sConditionCus") = Session("sConditionCus") & " and (iPxAcctAR_Cfg_Customer.provid = '" & dlQProvince.SelectedValue & "') "
            End If
            If dlQCity.Text.Trim <> "" Then
                Session("sConditionCus") = Session("sConditionCus") & " and (iPxAcctAR_Cfg_Customer.CityID= '" & dlQCity.SelectedValue & "') "
            End If
            If tbQEmail.Text.Trim <> "" Then
                Session("sConditionCus") = Session("sConditionCus") & " and (iPxAcctAR_Cfg_Customer.Email like '%" & Replace(tbQEmail.Text, "'", "''") & "%') "
            End If
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQuery", "hideModalQuery()", True)
            listCustomer()
        End If
    End Sub

    Protected Sub lbView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbView.Click
        pnCustGroup.Visible = False
        pnARRec.Visible = True
        ListReceipt()
        Me.lbAddRec.Enabled = True
        Me.lbView.Visible = False
    End Sub

    Protected Sub dlQCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dlQCountry.SelectedIndexChanged
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQuery", "hideModalQuery()", True)
        province()
        city()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalQuery", "showModalQuery()", True)
    End Sub

    Protected Sub dlQProvince_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dlQProvince.SelectedIndexChanged
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalQuery", "hideModalQuery()", True)
        city()
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalQuery", "showModalQuery()", True)
    End Sub

    Protected Sub lbAbortDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortDelete.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalDeleteTrans", "hideModalDeleteTrans()", True)
    End Sub

    Protected Sub lbDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbDelete.Click
        If tbReason.Text = "" Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('please enter your reason first !!');", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalDeleteTrans", "hideModalDeleteTrans()", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "showModalDeleteTrans", "showModalDeleteTrans()", True)
        Else
            'saveReason()
            Dim TransDateLog As String = Format(Now, "yyy-MM-dd hh:mm:ss")
            cIpx.saveLog(Session("sBusinessID"), Session("sSelectCustBathD"), TransDateLog, "AR REC", "Delete", Replace(tbReason.Text, "'", "''"), "", Session("sUserCode"))

            DeleteReceipt()
            InvoiceNo()
            ListReceipt()
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalDeleteTrans", "hideModalDeleteTrans()", True)
        End If
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Protected Sub lbAbortDocket_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAbortDocket.Click
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "hideModalDocket", "hideModalDocket()", True)
    End Sub

    Protected Sub gvDocket_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDocket.RowCommand
        If e.CommandName = "getDocket" Then
            Dim cIpx As New iPxClass
            Session("sReport") = "ARRecept"
            Session("sMapPath") = "~/iPxReportFile/" + e.CommandArgument
            Response.Redirect("rptviewer.aspx")
        End If
    End Sub
End Class
