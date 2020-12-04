Public Class MainForm
    Dim Spielfeld(9, 9) As Integer
    Dim DraggedShip As String
    Dim rxShipSize As New Text.RegularExpressions.Regex("^.+\((?<Size>\d+) .+\)$")
    Public Modus As Modi
    Dim currentPosition As Point
    Dim AnzahlRoterPanels As Integer
    Dim CurrentShip As Integer
    Dim FarbenListe As Color() = {Color.Green, Color.Aqua, Color.Blue, Color.Magenta, Color.GreenYellow}
    Dim connection As Connection
    Dim PlayerId As Integer
    Dim ShipReady As Boolean = True
    Dim WinVariable As Integer
    Dim canShoot As Boolean
    Dim SpielerReady As Integer
    Dim impossible As Boolean
    Dim Leaver As Integer
    Public Enum Modi As Integer
        Platzieren = 0
        Richtungsbestimmung = 1
        Spielen = 9
        getroffen = 10
        Verfehlt = 11
        selfMark = 12
        selfMarkHit = 13
    End Enum
    Private Enum Directions As Integer
        Right = 1
        Left = 2
        Down = 3
        Up = 4
    End Enum

    Public Sub New(spielerZahl As Integer)
        InitializeComponent()
        Show()
        connection = New Connection(spielerZahl, Me)
        Me.Text = spielerZahl.ToString
        PlayerId = spielerZahl
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PictureBox1.Size = PictureBox1.Image.Size
        PictureBox2.Size = PictureBox2.Image.Size
        ToolStripButton1_Click(Nothing, Nothing)
        Size = New Size(868, 711)
        Me.PictureBox1.AllowDrop = True
        For x As Integer = 0 To 9
            For y As Integer = 0 To 9
                Spielfeld(x, y) = 0
            Next
        Next

#If DEBUG Then
        PictureBox2.Visible = True
        Size = New Size(1551, 711)
#End If

    End Sub
    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        Me.ListBox1.Visible = True
        Me.Modus = Modi.Platzieren
    End Sub
    Private Sub ListBox1_MouseMove(sender As Object, e As MouseEventArgs) Handles ListBox1.MouseMove
        If e.Button = MouseButtons.Left Then
            If Me.ListBox1.SelectedItem <> Nothing Then
                Me.DraggedShip = Me.ListBox1.SelectedItem
                Me.ListBox1.DoDragDrop(Me.ListBox1.SelectedItem, DragDropEffects.All)
            End If
        End If
    End Sub
    Private Sub PictureBox1_DragEnter(sender As Object, e As DragEventArgs) Handles PictureBox1.DragEnter, PictureBox1.DragOver

        Dim p As Point = Me.PointToClient(System.Windows.Forms.Control.MousePosition)
        If p.X < 52 Or p.X > 540 Then
            e.Effect = DragDropEffects.None
        ElseIf p.Y < (54 + ToolStrip1.Height) Or p.Y > (542 + ToolStrip1.Height) Then
            e.Effect = DragDropEffects.None
        Else
            e.Effect = DragDropEffects.Move
        End If

    End Sub
    Private Sub PictureBox1_DragDrop(sender As Object, e As DragEventArgs) Handles PictureBox1.DragDrop
        If ShipReady Then
            Dim mClient As Point = PointToClient(Control.MousePosition)
            Dim x As Integer = Math.Floor((mClient.X) / (SystemInformation.Border3DSize.Width + 49))
            Dim y As Integer = Math.Floor((mClient.Y - ToolStrip1.Height) / (SystemInformation.Border3DSize.Height + 49))
            CheckPlacement(x, y, ShipSize(DraggedShip))
            If impossible = False Then
                ShipReady = False
                impossible = True
            End If
        End If
    End Sub
#Region "..."
    Private Function ShipSize(Shipname As String) As Integer
        If rxShipSize.IsMatch(Shipname) Then
            Return rxShipSize.Match(Shipname).Groups("Size").Value
        Else
            Return 0
        End If
        Return 5
    End Function
    Private Sub CheckPlacement(x As Integer, y As Integer, ShipSize As Integer)
        If CheckSpace(x, y) And MarkPossibilities(x, y, ShipSize) Then
            Me.Modus = Modi.Platzieren
            MarkCurrentPosition(x, y)
            currentPosition = New Point(x, y)
        End If
    End Sub
    Private Function MarkPossibilities(x As Integer, y As Integer, shipsize As Integer) As Boolean
        Dim AdditionalPanels As Integer = shipsize - 1
        Dim i As Integer
        Me.Modus = Modi.Richtungsbestimmung
        If CheckSpace(x + AdditionalPanels, y) And CheckForShips(x, y, shipsize, Directions.Right) Then
            MarkCurrentPosition(x + AdditionalPanels, y)
            i += 1
            AnzahlRoterPanels += 1
        End If
        If CheckSpace(x - AdditionalPanels, y) And CheckForShips(x, y, shipsize, Directions.Left) Then
            MarkCurrentPosition(x - AdditionalPanels, y)
            i += 1
            AnzahlRoterPanels += 1
        End If
        If CheckSpace(x, y + AdditionalPanels) And CheckForShips(x, y, shipsize, Directions.Down) Then
            MarkCurrentPosition(x, y + AdditionalPanels)
            i += 1
            AnzahlRoterPanels += 1
        End If
        If CheckSpace(x, y - AdditionalPanels) And CheckForShips(x, y, shipsize, Directions.Up) Then
            MarkCurrentPosition(x, y - AdditionalPanels)
            i += 1
            AnzahlRoterPanels += 1
        End If
        If i > 0 Then
            Return True
        End If
        impossible = True
        Return False
    End Function
    Private Function CheckForShips(x As Integer, y As Integer, shipsize As Integer, Richtung As Directions) As Boolean
        Dim AdditionalPanels As Integer = shipsize - 1
        Dim keinSchiff As Boolean = True
        Select Case Richtung
            Case Directions.Right
                If CheckDirectionSpaceX(x + 1, x + AdditionalPanels) Then
                    Return False
                Else
                    For i As Integer = (x + 1) To (x + AdditionalPanels)
                        If Spielfeld(PosToArray(i), PosToArray(y)) <> 0 Then
                            keinSchiff = False
                        End If
                    Next
                End If
                Return keinSchiff
            Case Directions.Left
                If CheckDirectionSpaceX(x - AdditionalPanels, x - 1) Then
                    Return False
                Else
                    For i As Integer = (x - AdditionalPanels) To (x - 1)
                        If Spielfeld(PosToArray(i), PosToArray(y)) <> 0 Then
                            keinSchiff = False
                        End If
                    Next
                End If
                Return keinSchiff
            Case Directions.Down
                If CheckDirectionSpaceY(y + 1, y + AdditionalPanels) Then
                    Return False
                Else
                    For i As Integer = (y + 1) To (y + AdditionalPanels)
                        If Spielfeld(PosToArray(x), PosToArray(i)) <> 0 Then
                            keinSchiff = False
                        End If
                    Next
                End If
                Return keinSchiff
            Case Directions.Up
                If CheckDirectionSpaceY(y - AdditionalPanels, y - 1) Then
                    Return False
                Else
                    For i As Integer = (y - AdditionalPanels) To (y - 1)
                        If Spielfeld(PosToArray(x), PosToArray(i)) <> 0 Then
                            keinSchiff = False
                        End If
                    Next
                End If
                Return keinSchiff
        End Select
        Return keinSchiff
    End Function
    Private Function CheckDirectionSpaceX(x As Integer, x2 As Integer) As Boolean
        If x < 1 Or x2 < 1 OrElse Spielfeld.GetLength(0) < x Or Spielfeld.GetLength(0) < x2 Then
            Return True
        Else
            Return False
        End If
        Exit Function
    End Function
    Private Function CheckDirectionSpaceY(y As Integer, y2 As Integer) As Boolean
        If y < 1 Or y2 < 1 OrElse Spielfeld.GetLength(1) < y Or Spielfeld.GetLength(1) < y2 Then
            Return True
        Else
            Return False
        End If
        Exit Function
    End Function
    <DebuggerStepThrough> Private Function CheckSpace(x As Integer, y As Integer) As Boolean
        If x < 1 Or y < 1 OrElse Spielfeld.GetLength(0) < x Or Spielfeld.GetLength(1) < y Then
            Return False
        Else
            Return Spielfeld(PosToArray(x), PosToArray(y)) = 0
        End If
        Exit Function
    End Function
#End Region
    Public Sub MarkCurrentPosition(x As Integer, y As Integer)
        Dim pnl As New Panel With {
            .Size = New Size(50, 50),
            .Location = New Point(PictureBox1.Width / 12 * x, PictureBox1.Height / 12 * y),
            .Parent = PictureBox1
        }
        Select Case Me.Modus
            Case Modi.getroffen
                Dim hitPanel As Panel = CType(PictureBox1.Controls(x.ToString + ":" + y.ToString), Panel)
                hitPanel.BackColor = Color.Red
                Exit Sub
            Case Modi.Verfehlt
                pnl.BackColor = Color.Black
                PictureBox1.Controls.Add(pnl)
                Exit Sub
            Case Modi.selfMarkHit
                WinVariable += 1
                pnl.BackColor = Color.Red
                pnl.Parent = PictureBox2
                PictureBox2.Controls.Add(pnl)
                If WinVariable = 17 Then
                    connection.SendWon(PlayerId)
                    playerEndGame(PlayerId)
                End If
                Exit Sub
            Case Modi.selfMark
                pnl.BackColor = Color.Black
                pnl.Parent = PictureBox2
                PictureBox2.Controls.Add(pnl)
                Exit Sub
            Case Modi.Platzieren
                pnl.BackColor = FarbenListe(CurrentShip)
                pnl.Name = x.ToString + ":" + y.ToString
                pnl.Tag = CurrentShip
                Spielfeld(PosToArray(x), PosToArray(y)) = 1
            Case Modi.Richtungsbestimmung
                pnl.BackColor = Color.Red
                AddHandler pnl.Click, AddressOf DirectionPanel_Click
                pnl.Name = "RotesPanel:" + AnzahlRoterPanels.ToString
        End Select
        PictureBox1.Controls.Add(pnl)
    End Sub
    Private Sub DirectionPanel_Click(sender As Object, e As EventArgs)
        Dim pnl = DirectCast(sender, Panel)
        Dim x = Math.Round(pnl.Location.X / (PictureBox1.Width / 12))
        Dim y = Math.Round(pnl.Location.Y / (PictureBox1.Height / 12))
        If x > currentPosition.X Then                                    'rechts
            For i As Integer = currentPosition.X To x
                MarkCurrentPosition(i, y)
            Next
        End If
        If y > currentPosition.Y Then                                    'unten
            For i As Integer = currentPosition.Y To y
                MarkCurrentPosition(x, i)
            Next
        End If
        If y < currentPosition.Y Then                                     'oben
            For i As Integer = y To currentPosition.Y
                MarkCurrentPosition(x, i)
            Next
        End If
        If x < currentPosition.X Then                                     'links
            For i As Integer = x To currentPosition.X
                MarkCurrentPosition(i, y)
            Next
        End If
        sender.backcolor = FarbenListe(CurrentShip)
        deleteRedPanels()
        ListBox1.Items.Remove(ListBox1.SelectedItem)
        CurrentShip += 1
        If ListBox1.Items.Count = 0 Then
            PictureBox2.Visible = True
            Size = New Size(1551, 711)
            connection.SendReady(PlayerId)
            Ready()
        End If
        ShipReady = True
    End Sub

    Public Sub Ready()
        SpielerReady += 1
        If SpielerReady = 2 Then
            If PlayerId = 1 Then
                canShoot = True
            End If
            MsgBox("Ships have been placed.")
        End If

    End Sub

    Private Sub deleteRedPanels()
        For i As Integer = 0 To AnzahlRoterPanels - 1
            Dim redPanel As Object = Me.PictureBox1.Controls.Find("RotesPanel:" + i.ToString, False)(0)
            Me.PictureBox1.Controls.Remove(redPanel)
        Next
        AnzahlRoterPanels = 0
    End Sub
    <DebuggerStepThrough> Private Function PosToArray(pos As Integer) As Integer
        Return pos - 1
    End Function
    <DebuggerStepThrough> Private Function PosToField(pos As Integer) As Integer
        Return pos + 1
    End Function
    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        Select Case Me.Modus
            Case Modi.Richtungsbestimmung
            Case Modi.Spielen
        End Select
    End Sub

    Public Sub playerEndGame(playerId As Integer)
        MsgBox("Player " & playerId & " has won")
        canShoot = False
    End Sub

    '////////////////////////////////////////////////////////SCHIEßEN ZUM GEGNER////////////////////////////////////////////////////////
    Private Sub Picturebox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click

        If canShoot Then
            Dim MousePos As Point = PictureBox2.PointToClient(MousePosition)
            Dim x As Integer = Math.Floor(MousePos.X / 49)
            Dim y As Integer = Math.Floor(MousePos.Y / 49)
            If y = 0 Then
                y += 1
            Else
            End If
            If x = 0 Then
                x += 1
            End If
            If y = 11 Then
                y -= 1
            Else
            End If
            If x = 11 Then
                x -= 1
            End If
            Me.connection.ShootAt(PlayerId, x, y)
            canShoot = False
        End If
    End Sub
    Public Function CheckHit(x As Integer, y As Integer)
        If x < 11 And y < 11 And x > 0 And y > 0 Then
            If Not Spielfeld(x - 1, y - 1) = 0 Then
                Me.Modus = Modi.getroffen
                MarkCurrentPosition(x, y)
                canShoot = True
                Return True
            Else
                Me.Modus = Modi.Verfehlt
                MarkCurrentPosition(x, y)
                canShoot = True
                Return False
            End If
        Else Return False
        End If
    End Function

    Public Sub LeaverMessage()
        MsgBox("Opponent has left the game.")
    End Sub

    Private Sub MainForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If Leaver <> 1 Then
            connection.Leaver(PlayerId)
        End If
    End Sub

    Public Sub GameClose()
        Leaver = 1
    End Sub
End Class
