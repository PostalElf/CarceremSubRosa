Public Class mysticCalendar
    Public Sub New(dateTime As DateTime)
        _dateTime = dateTime
        _lunarDay = getMoonPhaseInitial(_dateTime)
        _lunarMansion = 24
        _lunarMansionDay = 6

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
        Const len As Integer = 16
        Console.WriteLine(ind & fakeTab("Long Count: ", len) & ToString())
        Console.WriteLine(ind & fakeTab("Mayan Date: ", len) & tzolkinDate & ", " & haabDate)
        'Console.WriteLine(ind & fakeTab("Weeksign: ", len) & aztecTrecena & " (" & getGodDomain(aztecTrecena) & ")")
        'Console.WriteLine(ind & fakeTab("Daysign: ", len) & aztecDay & " (" & getGodDomain(aztecDay) & ")")
        'Console.WriteLine(ind & fakeTab("Nightsign: ", len) & lordNightName & " (" & getGodDomain(lordNightName) & ")")
        'Console.WriteLine(ind & fakeTab("Cardinality: ", len) & aztecDirection)
        Console.WriteLine(ind & fakeTab("Moon Phase: ", len) & moonPhase & " in " & lunarMansion)
    End Sub
    Public Overrides Function ToString() As String
        Return _baktun & "." & _katun & "." & _tun & "." & _uinal & "." & _kin
    End Function

    Private Property _dateTime As DateTime
    Private Property _lunarDay As Integer
    Private Function getJulianDate(dateTime As DateTime) As Integer
        Dim year As Integer = dateTime.Year
        Dim month As Integer = dateTime.Month
        Dim day As Integer = dateTime.Day

        Dim yy As Integer = year - Math.Floor((12 - month) / 10)
        Dim mm As Integer = month + 9
        If mm > 12 Then mm -= 12

        Dim K1 As Integer = Math.Floor(365.25 * (yy + 4712))
        Dim K2 As Integer = Math.Floor(30.6 * mm + 0.5)
        Dim K3 As Integer = Math.Floor(Math.Floor((yy / 100) + 49) * 0.75) - 38

        Dim JD As Integer = K1 + K2 + day + 59
        If (JD > 2299160) Then JD -= K3
        Return JD
    End Function
    Private Function getMoonPhaseInitial(dateTime As DateTime) As Integer
        Const lunarCycle As Double = 29.530588853
        Dim knownNewMoon As New DateTime(2000, 1, 6)
        Dim daysSince As Integer = (dateTime - knownNewMoon).TotalDays
        Dim phase As Integer = daysSince Mod lunarCycle

        Return phase
    End Function
    Friend ReadOnly Property moonPhase() As String
        Get
            Select Case _lunarDay
                Case 2 To 7 : Return "Waxing Crescent"
                Case 8 : Return "Half Moon"
                Case 9 To 13 : Return "Waxing Gibbous"
                Case 14 To 16 : Return "Full Moon"
                Case 17 To 21 : Return "Waning Gibbous"
                Case 22 : Return "Half Moon"
                Case 23 To 27 : Return "Waning Crescent"
                Case Else : Return "New Moon"
            End Select
        End Get
    End Property
    Private Property _lunarMansionDay As Integer
    Private Property _lunarMansion As Integer
    Friend ReadOnly Property lunarMansion As String
        Get
            'Dim mansions As String() = {"Sheratan", "Pleione", "Albatain", "Al Tuwaibe", "Heka", "Alhena", "Murzim", "An Nathra", "Alterf", _
            '                            "Dschuba", "Azzubra", "Assarfa", "Auva", "Simak", "Syrma", "Az Zubana", "Akleel", "Qalb al Akraab", _
            '                            "Shaula", "Al Naam", "Al Baldaah", "Saad Al Thabib", "Saad Balaa", "Saad Al Saud", "Saad Al Akhbia", _
            '                            "Almuqaddam", "Al Muakhar", "Alrescha"}
            Dim mansions As String() = {"Al-Sharatain", "Al-Butain", "Al-Thurayya", "Al-Dabaran", "Al-Haqa", "Al-Hana", "Al-Dhira", "Al-Nathrah", _
                                        "Al-Tarf", "Al-Jabhah", "Al-Zubrah", "Al-Sarfah", "Al-Awwa", "Al-Simak", "Al-Ghafr", "Al-Zubana", "Al-Iklil", _
                                        "Al-Qalb", "Al-Shaulah", "Al-Na'am", "Al-Baldah", "Sa'd al-Dhabih", "Sa'd Bula", "Sa'd al-Su'ud", "Sa'd al-Akhbiyah", _
                                        "Al Fargh al-Awwal", "Al Fargh al-Thani", "Batn al-Hut"}

            Return mansions(_lunarMansion - 1)
        End Get
    End Property

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
    Friend ReadOnly Property lordNightName As String
        Get
            Dim lords As String() = {"Xiuhtecuhtli", "Itzli", "Piltzintecuhtli", "Centeotl", "Mictlantecuhtli", "Chalchiuhtlicue", "Tlazolteotl", "Tepeyollotl", "Tlaloc"}
            Return lords(_lordNight - 1)
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
        Dim meanings As String() = {"Crocodile", "Wind", "Night-House", "Maize", "Serpent", "Death", "Deer", "Rabbit", "Water", "Dog", _
                                    "Monkey", "Grass", "Reed", "Jaguar", "Eagle", "Vulture", "Earthquake", "Knife", "Rain", "Lord"}
        Return meanings(god - 1)
    End Function
    Friend ReadOnly Property aztecDay() As String
        Get
            Dim gods As String() = {"Xiuhtecuhtli", "Tlaltecuhtli", "Chalchiuhtlicue", "Tonatiuh", "Tlazolteotl", "Mictlantecuhtli", "Centeotl", _
                                    "Tlaloc", "Quetzalcoatl", "Tezcatlipoca", "Mictlantecuhtli", "Tlahuizcalpantecuhtli", "Citlalicue"}
            Return gods(_tzolkinDay - 1)
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

    Private Function getGodDomain(god As String) As String
        Select Case god
            Case "Xiuhtecuhtli" : Return "Fire"
            Case "Tlaltecuhtli" : Return "Earth"
            Case "Chalchiuhtlicue" : Return "Water"
            Case "Tonatiuh" : Return "Sun"
            Case "Tlazolteotl" : Return "Love"
            Case "Mictlantecuhtli" : Return "Death"
            Case "Centeotl" : Return "Maize"
            Case "Tlaloc" : Return "Rain"
            Case "Quetzalcoatl" : Return "Wisdom"
            Case "Tezcatlipoca" : Return "Twilight"
            Case "Mictlantecuhtli" : Return "Death"
            Case "Tlahuizcalpantecuhtli" : Return "Dawn"
            Case "Citlalicue" : Return "Stars"
            Case "Itzli" : Return "Obsidian"
            Case "Piltzintecuhtli" : Return "Vision"
            Case "Ometeotl" : Return "Duality"
            Case "Tepeyollotl" : Return "Earthquakes"
            Case "Huehuecoyotl" : Return "Mischief"
            Case "Mayahuel" : Return "Alcohol"
            Case "Patecatl" : Return "Healing"
            Case "Itztlacoliuhqui" : Return "Frost"
            Case "Xipe Totec" : Return "Rebirth"
            Case "Itzpapalotl" : Return "War"
            Case "Xolotl" : Return "Lightning"
            Case "Chalchiuhtotolin" : Return "Plague"
            Case "Chantico" : Return "Hearthfire"
            Case "Xochiquetzal" : Return "Maiden"
            Case Else : Return Nothing
        End Select
    End Function

    Friend Sub timeTick()
        _lunarDay += 1
        If _lunarDay > 29 Then _lunarDay = 1
        _lunarMansionDay += 1
        If _lunarMansionDay > 13 Then
            _lunarMansionDay = 1
            _lunarMansion += 1
            If _lunarMansion > 28 Then
                _lunarMansion = 1
            End If
        End If

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
