Public Class Startform
    Private Sub btStart_Click(sender As Object, e As EventArgs) Handles btStart.Click
        Dim spieler1 As New MainForm(1)
        Dim spieler2 As New MainForm(2)
        AddHandler spieler1.FormClosed, AddressOf Close
        AddHandler spieler2.FormClosed, AddressOf Close
        Hide()
    End Sub
End Class