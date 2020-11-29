<%@ WebHandler Language="VB" Class="Handler" %>

Imports System
Imports System.Web
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Drawing

Public Class Handler : Implements IHttpHandler
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim sqlCon As New SqlConnection(ConfigurationManager.ConnectionStrings("iPxCNCT").ToString())
        Dim sSql As New SqlCommand
        Dim aCmd() As String
        On Error Resume Next
        
        aCmd = Split(context.Request.QueryString("ID"), "|")

        Select Case aCmd(0)
            Case "0"   ' guestgeneral
                sSql.CommandText = "select imagelogo as imgFile from  iPx_profile_client WHERE businessid='" & aCmd(1) & "' "
            Case "1"   ' Settlement
                sSql.CommandText = "select imagelogo as imgFile from iPx_profile_settlement WHERE id=" & Val(aCmd(1)) & " "
          
        End Select
        sSql.CommandType = Data.CommandType.Text
        sSql.Connection = sqlCon
        sqlCon.Open()
        Dim sSqlR As SqlDataReader = sSql.ExecuteReader
        sSqlR.Read()
        context.Response.BinaryWrite(CType(sSqlR("imgFile"), Byte()))
        sSqlR.Close()



    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class