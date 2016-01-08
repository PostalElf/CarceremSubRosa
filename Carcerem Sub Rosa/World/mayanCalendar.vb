Public Class mayanCalendar
    Public Sub New(dateTime As DateTime)
        Dim baktunStart As New DateTime(2012, 12, 21)
        Dim daysSince As Integer = (dateTime - baktunStart).TotalDays

        Dim baktunRemain, katunRemain, tunRemain As Integer
        _baktun = 13
        baktunRemain = daysSince
        _katun = Math.DivRem(baktunRemain, 7200, katunRemain)
        _tun = Math.DivRem(katunRemain, 360, tunRemain)
        _uinal = Math.DivRem(tunRemain, 20, _kin)

        Dim daysFromBeginning As Integer = 0
        daysFromBeginning += _baktun * 144000
        daysFromBeginning += _katun * 7200
        daysFromBeginning += _tun * 360
        daysFromBeginning += _uinal * 20
        daysFromBeginning += _kin

        Dim roundCycles As Integer = Math.Floor(daysFromBeginning / 18980)
        Dim r As Integer = daysFromBeginning Mod 18980
        Dim a As Integer = r Mod 13
        Dim b As Integer = r Mod 20

        _tzolkinDay = (a + 4) Mod 13
        _tzolkinGod = b Mod 20

        Dim x As Integer = daysFromBeginning + 348
        Dim y As Integer = x Mod 365
        _haabDay = y Mod 20
        _haabMonth = Math.Floor(y / 20)

        Debug.Print(ToString)
        Debug.Print(tzolkinDate)
        Debug.Print(haabDate)
    End Sub
    Public Overrides Function ToString() As String
        Return _baktun & "." & _katun & "." & _tun & "." & _uinal & "." & _kin & " (" & tzolkinDate & ", " & haabDate & ")"
    End Function

    Private Property _kin As Integer        'one day
    Private Property _uinal As Integer      '20 kin
    Private Property _tun As Integer        '18 uinal
    Private Property _katun As Integer      '20 uinal
    Private Property _baktun As Integer     '20 katun

    Private Property _tzolkinDay As Integer
    Private Property _tzolkinGod As Integer
    Private Function getTzolkinDate(day As Integer, god As Integer) As String
        Dim gods() As String = {"Imix", "Ik", "Akbal", "Kan", "Chicchan", "Cimi", "Manik", "Lamat", "Muluc", "Oc", "Chuen", "Eb", "Ben", "Ix", "Men", "Cib", "Caban", "Etznab", "Cauac", "Ahau"}
        Return day & " " & gods(god - 1)
    End Function
    Friend ReadOnly Property tzolkinDate As String
        Get
            Return getTzolkinDate(_tzolkinDay, _tzolkinGod)
        End Get
    End Property

    Private Property _haabDay As Integer
    Private Property _haabMonth As Integer
    Private Function getHaabDate(day As Integer, month As Integer) As String
        Dim months() As String = {"Pop", "Uo", "Zip", "Zotz", "Tzec", "Xul", "Yaxkin", "Mol", "Chen", "Yax", "Zac", "Ceh", "Mac", "Kankin", "Muan", "Pax", "Kayab", "Cumku", "Uayeb"}
        Return day & " " & months(month)
    End Function
    Friend ReadOnly Property haabDate As String
        Get
            Return getHaabDate(_haabDay, _haabMonth)
        End Get
    End Property


    Friend Sub timeTick()
        _kin += 1
        If _kin = 20 Then
            _kin = 0
            _uinal += 1
            If _uinal = 18 Then
                _uinal = 0
                _tun += 1
                If _tun = 20 Then
                    _tun = 0
                    _katun += 1
                    If _katun = 20 Then
                        _katun = 0
                        _baktun += 1
                        If _baktun = 20 Then
                            resetLongCount()
                        End If
                    End If
                End If
            End If
        End If

        _tzolkinDay += 1
        If _tzolkinDay > 13 Then _tzolkinDay = 1
        _tzolkinGod += 1
        If _tzolkinGod > 20 Then _tzolkinGod = 1

        _haabDay += 1
        If _haabDay = 20 Or (_haabMonth = 18 AndAlso _haabDay = 5) Then
            _haabDay = 0
            _haabMonth += 1
            If _haabMonth = 19 Then
                _haabMonth = 0
            End If
        End If

        Debug.Print(ToString)
        Debug.Print(tzolkinDate)
        Debug.Print(haabDate)
    End Sub
    Private Sub resetLongCount()
        _kin = 0
        _uinal = 0
        _tun = 0
        _katun = 0
        _baktun = 0
    End Sub
End Class
