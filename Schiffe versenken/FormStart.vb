Imports System.Net.Sockets

Public Class FormStart

    Public Sub New()
        InitializeComponent()
#If DEBUG Then
        TextBox1.Text = "10.132.100.76:60005"
#End If
    End Sub

    Private Sub btHost_Click(sender As Object, e As EventArgs) Handles btHost.Click
        Dim loadForm As LoadingForm = LoadingForm.getForm()
        loadForm.Show()
        Dim spieler1 As New MainForm(1)
        AddHandler spieler1.FormClosed, AddressOf Close
        spieler1.Hide()
        AddHandler loadForm.FormClosed, AddressOf Close
        Hide()
    End Sub

    Private Sub btConnect_Click(sender As Object, e As EventArgs) Handles btConnect.Click
        IP = TextBox1.Text
        If testConnection() Then
            Dim spieler2 As New MainForm(2)
            AddHandler spieler2.FormClosed, AddressOf Close
            Hide()
        Else
            MsgBox("Es gibt keinen Server")
        End If

    End Sub

    Public Function testConnection() As Boolean
        Dim stringarray As String() = FormStart.IP.Split(":")
        Dim IP As String
        IP = stringarray(0)
        Dim Port As String
        Port = stringarray(1)

        Try
            Dim UserClient As New TcpClient(IP, Port)
            UserClient.Close()
            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function
    Public Shared IP As String
End Class