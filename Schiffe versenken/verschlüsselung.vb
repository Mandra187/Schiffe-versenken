Public Class Verschlüsselung

    Public Shared g As Long                                                       'random Zahl kleiner als die Primzahl
    Public Shared PrimeNumber As Long = GetRandomPrimeNumber()                    'random Primzahl
    Dim conn As Connection

#Region "Cäsar"
    Public Shared Function Encode_Caesar(input As String, shift As Integer) As String            ' Verschlüsseln
        Dim ret As String = ""
        Dim length As Integer = input.Count - 1
        Dim myChar(length) As Char
        For i As Integer = 0 To length
            myChar(i) = input.Chars(i)
            myChar(i) = ShiftAsciiEncode(myChar(i), shift)
        Next
        ret = New String(myChar)
        Return ret
    End Function

    Public Shared Function Decode_Caesar(input As String, shift As Integer) As String        ' Entschlüsseln
        Dim ret As String = ""
        Dim length As Integer = input.Count - 1
        Dim myChar(length) As Char
        For i As Integer = 0 To length
            myChar(i) = input.Chars(i)
            myChar(i) = ShiftAsciiDecode(myChar(i), shift)
        Next
        ret = New String(myChar)
        MessageBox.Show(ret)
        Return ret
    End Function

    Private Shared Function ShiftAsciiEncode(letter As Char, shift As Integer)      ' Ascii verschieben
        Dim Ascii As Integer = Asc(letter)
        If Ascii > 64 And Ascii < 91 Then
            Ascii += shift
            Dim Modulo As Integer = Ascii
            If Modulo > 90 Then
                Ascii = 65
                Ascii += (Modulo Mod 90)
                Ascii -= 1
            End If
            Dim AsciiReverse As Char = Chr(Ascii)
            Return AsciiReverse
        Else
            Return Chr(Ascii)
        End If
    End Function

    Private Shared Function ShiftAsciiDecode(letter As Char, shift As Integer)     ' Ascii verschieben
        Dim Ascii As Integer = Asc(letter)
        If Ascii > 64 And Ascii < 91 Then
            Ascii -= shift '60
            Dim Modulo As Integer = Ascii '60     
            If Modulo < 65 Then
                Ascii = 90
                Ascii -= (65 Mod Modulo) '90 - 5 
                Ascii += 1
            End If
            Dim AsciiReverse As Char = Chr(Ascii)
            Return AsciiReverse
        Else
            Return Chr(Ascii)
        End If
    End Function
#End Region

#Region "Diffie Hellman"
    Public Shared Function HostDiffieNum() As String
        Do
            g = GetRandomNumber()
        Loop While g > PrimeNumber
        g -= 4
        Dim SecretNum As Long = GetLowerRandomNumber()                             'privater Schlüssel  der hosters
        Dim A As Long = CalculationA(g, PrimeNumber, SecretNum)                     'öffentlicher Schlüssel   

        'Diffie.Label2.Text = "g: " & g.ToString & vbCrLf & "p: " & PrimeNumber.ToString & vbCrLf & "a: " & SecretNum.ToString & vbCrLf & "A: " & A.ToString

        ConnectDiffieNum(g, PrimeNumber, A, SecretNum)

    End Function

    Public Shared Function ConnectDiffieNum(g As Long, primeNumber As Long, A As Long, secretNum As Long) As String
        Dim kleinB As Long = GetLowerRandomNumber()              'privater Schlüssel des Connectors
        Dim B As Long = calculationB(g, primeNumber, kleinB)       'öffentlicher Schlüssel
        Dim Schlüssel As Long = CalculationKey(A, kleinB, primeNumber)
        Dim SchlüsselProof As Long = KeyHost(B, secretNum, primeNumber)

        'Diffie.Label1.Text = "b: " & kleinB.ToString & vbCrLf & "B: " & B.ToString & vbCrLf & "K1: " & Schlüssel.ToString & vbCrLf & "K2: " & SchlüsselProof.ToString

    End Function

#Region "RandomNumbers"
    Private Shared Function GetRandomNumber() As Integer
        Dim secretNumber As Integer
        secretNumber = CInt(Int((25 * Rnd()) + 5))
        Return secretNumber
    End Function

    Private Shared Function GetLowerRandomNumber() As Integer
        Dim number As Integer
        number = CInt(Int(5 * Rnd() + 1))
        Return number
    End Function

    Private Shared Function GetRandomPrimeNumber() As Long
        Dim checkIfPrime As Boolean
        Dim rndNumber As Long
        Do While checkIfPrime = False
            rndNumber = GetRnd()
            checkIfPrime = CheckPrime(rndNumber)
        Loop
        Return rndNumber
    End Function

    Private Shared Function GetRnd() As Long
        Dim rnd As Random = New Random
        Dim rndm As Integer = rnd.Next(250)
        Return rndm
    End Function

    Private Shared Function CheckPrime(rndNumber As Long) As Boolean
        Dim c As Long
        Dim b As Long
        c = rndNumber
        If rndNumber = 1 Then
            Return False
        Else
            For b = 2 To c Step 1
                If rndNumber Mod b = 0 And c <> b Then
                    Return False
                Else
                    If b = c Then
                        Return True
                    End If
                End If
            Next
        End If
        Return False
    End Function
#End Region

    Private Shared Function CalculationKey(A As Long, secretNum As Long, primenumber As Long) As Long
        Dim Key As Long = A ^ secretNum Mod primenumber
        Return Key
    End Function

    Private Shared Function CalculationA(g As Long, primeNumber As Long, secretNum As Long) As Long
        Dim A As Long
        A = g ^ secretNum Mod primeNumber                    'Berechnung des öffentlichen Schlüssels

        Return A
    End Function

    Private Shared Function calculationB(g As Integer, primeNumber As Integer, kleinB As Integer)
        Dim B As Integer
        B = g ^ kleinB Mod primeNumber

        Return B
    End Function
    Private Shared Function KeyHost(B As Long, secretNum As Long, primeNumber As Long) As Long
        Dim Key = B ^ secretNum Mod primeNumber

        Return Key
    End Function

#End Region

End Class
