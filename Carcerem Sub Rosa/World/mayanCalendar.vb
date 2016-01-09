Public Class mayanCalendar
    Public Sub New(dateTime As DateTime)
        _dateTime = dateTime
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

        _lordNight = ((daysFromBeginning - 1) Mod 9) + 1
        _aztecTrecena = getAztecTrecenaInitial(_tzolkinDay, _tzolkinGod)

        'Debug.Print(ToString)
        'Debug.Print(tzolkinDate)
        'Debug.Print(haabDate)
    End Sub
    Friend Sub consoleReport(indent As Integer)
        Dim ind As String = vbSpace(indent)
        Const len As Integer = 13
        Console.WriteLine(ind & fakeTab("Long Count: ", len) & ToString())
        Console.WriteLine(ind & fakeTab("Mayan Date: ", len) & tzolkinDate & ", " & haabDate)
        Console.WriteLine(ind & fakeTab("Weeksign: ", len) & aztecTrecena)
        Console.WriteLine(ind & fakeTab("Daysign: ", len) & aztecDaysign & " (" & aztecDirection & ")")
        Console.WriteLine(ind & fakeTab("Nightsign: ", len) & lordNightName)
        Console.WriteLine(ind & fakeTab("Moon Phase: ", len) & getMoonPhase(_dateTime))
    End Sub
    Public Overrides Function ToString() As String
        Return _baktun & "." & _katun & "." & _tun & "." & _uinal & "." & _kin
    End Function

    Private Property _dateTime As DateTime

    Private Property _kin As Integer        'one day
    Private Property _uinal As Integer      '20 kin
    Private Property _tun As Integer        '18 uinal
    Private Property _katun As Integer      '20 uinal
    Private Property _baktun As Integer     '20 katun

    Private Property _tzolkinDay As Integer
    Private Property _tzolkinGod As Integer
    Friend ReadOnly Property tzolkinDate() As String
        Get
            Dim day As Integer = _tzolkinDay
            Dim god As Integer = _tzolkinGod
            Dim gods() As String = {"Imix", "Ik", "Akbal", "Kan", "Chicchan", "Cimi", "Manik", "Lamat", "Muluc", "Oc", "Chuen", "Eb", "Ben", "Ix", "Men", "Cib", "Caban", "Etznab", "Cauac", "Ahau"}
            Return day & " " & gods(god - 1)
        End Get
    End Property

    Private Property _haabDay As Integer
    Private Property _haabMonth As Integer
    Friend ReadOnly Property haabDate() As String
        Get
            Dim day As Integer = _haabDay
            Dim month As Integer = _haabMonth
            Dim months() As String = {"Pop", "Uo", "Zip", "Zotz", "Tzec", "Xul", "Yaxkin", "Mol", "Chen", "Yax", "Zac", "Ceh", "Mac", "Kankin", "Muan", "Pax", "Kayab", "Cumku", "Uayeb"}
            Return day & " " & months(month)
        End Get
    End Property

    Private Property _lordNight As Integer
    Private Function getLordNightName(lordNight As Integer) As String
        Dim lords As String() = {"Xiuhtecutli", "Itzli", "Pilzintecuhtli", "Cinteotl", "Mictantecutli", "Chalchiuhtlicue", "Tlazolteotl", "Tepeyollotl", "Tlaloc"}
        Return lords(lordNight - 1)
    End Function
    Friend ReadOnly Property lordNightName As String
        Get
            Return getLordNightName(_lordNight)
        End Get
    End Property

    Private Property _aztecTrecena As Integer
    Private Function getAztecTrecenaInitial(day As Integer, god As Integer) As Integer
        Dim weeksPassed As Integer = 1
        Dim x As Integer = 1
        Dim y As Integer = 1
        While x <> day OrElse y <> god
            x += 1
            y += 1
            If x > 13 Then
                x = 1
                weeksPassed += 1
            End If
            If y > 20 Then y = 1
        End While
        Return weeksPassed
    End Function
    Private Function getMayanMeaning(god As Integer) As String
        Dim daysigns As String() = {"Crocodile", "Wind", "Night-House", "Maize", "Serpent", "Death", "Deer", "Rabbit", "Water", "Dog", _
                                    "Monkey", "Grass", "Reed", "Jaguar", "Eagle", "Vulture", "Earthquake", "Knife", "Rain", "Lord"}
        Return daysigns(god - 1)
    End Function
    Friend ReadOnly Property aztecDaysign()
        Get
            Return getMayanMeaning(_tzolkinGod)
        End Get
    End Property
    Friend ReadOnly Property aztecDirection()
        Get
            Dim god As Integer = _tzolkinGod
            Dim directions As String() = {"East", "North", "West", "South"}
            Dim value As Integer = (god - 1) Mod 4
            Return directions(value)
        End Get
    End Property
    Friend ReadOnly Property aztecTrecena
        Get
            Dim gods As String() = {"Ometeotl", "Quetzalcoatl", "Tepeyollotl", "Huehuecoyotl", "Chalchiuhtlicue", "Tonatiuh", _
                                    "Tlaloc", "Mayahuel", "Xiuhtecuhtli", "Mictlantecuhtli", "Patecatl", "Itztlacoliuhqui", _
                                    "Tlazolteotl", "Xipe Totec", "Itzpapalotl", "Xolotl", "Chalchiuhtotolin", "Chantico", _
                                    "Xochiquetzal", "Xiuhtecuhtli"}

            Return gods(_aztecTrecena - 1)
        End Get
    End Property
    Friend ReadOnly Property aztecDate As String
        Get
            Return _tzolkinDay & " " & getMayanMeaning(_tzolkinGod)
        End Get
    End Property

    Private Function getMoonPhase(dateTime As DateTime) As String
        Dim year As Integer = dateTime.Year
        Dim month As Integer = dateTime.Month
        Dim day As Integer = dateTime.Day

        Dim b, c, e As Integer
        Dim jd As Double

        If month < 3 Then
            year -= 1
            month += 12
        End If
        month += 1
        c = 365.25 * year
        e = 30.6 * month
        jd = c + e + day - 694039.09            'julian date

        jd /= 29.53                             'divide by moon cycle (29.53 days)
        b = Math.Floor(jd)                      'take integer part of jd
        jd = jd - b                             'get fractional part of original jd
        b = jd * 8 + 0.5                        'scale fraction from 0 to 8, round by adding 0.5
        If b = 8 Then b = 0

        Select Case b
            Case 0 : Return "New"
            Case 1 : Return "Waxing Crescent"
            Case 2 : Return "First Quarter"
            Case 3 : Return "Waxing Gibbous"
            Case 4 : Return "Full"
            Case 5 : Return "Waning Gibbous"
            Case 6 : Return "Third Quarter"
            Case 7 : Return "Waning Crescent"
            Case Else : Return Nothing
        End Select
    End Function

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
        If _tzolkinDay > 13 Then
            _tzolkinDay = 1
            _aztecTrecena += 1
            If _aztecTrecena > 20 Then _aztecTrecena = 1
        End If
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

        _lordNight += 1
        If _lordNight > 9 Then _lordNight = 1

        'Debug.Print(ToString)
        'Debug.Print(tzolkinDate)
        'Debug.Print(haabDate)
    End Sub
    Private Sub resetLongCount()
        _kin = 0
        _uinal = 0
        _tun = 0
        _katun = 0
        _baktun = 0
    End Sub
End Class
