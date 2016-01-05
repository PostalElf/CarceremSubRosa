Public Class agent
    Implements problemReporter
    Friend Shared Function buildRandomAgent(aPlayer As player) As agent
        Dim agent As New agent
        With agent
            .player = aPlayer
            ._isMale = coinFlip()
            ._age = 21 + rng.Next(13)
            ._firstName = getRandomFirstName(._isMale)
            ._lastName = getRandomLastName()
            ._disposition = disposition.buildRandomDisposition
        End With
        Return agent
    End Function
    Public Overrides Function ToString() As String
        Dim total As String = "Agent " & name
        If _isMale = True Then total &= " (M)" Else total &= " (F)"
        total &= ", " & _age
        Return total
    End Function

    Private Property _firstName As String
    Private Property _lastName As String
    Friend Property name As String Implements problemReporter.name
        Get
            Return _firstName(0) & ". " & _lastName
        End Get
        Set(value As String)
            If value.Contains(" ") Then
                Dim split() As String = value.Split(" ")
                _firstName = split(0)
                _lastName = split(1)
            Else
                _firstName = value
            End If
        End Set
    End Property
    Private _isMale As Boolean
    Private _age As Integer
    Private Shared Property _boyFirstNames As New List(Of String)
    Private Shared Property _girlFirstNames As New List(Of String)
    Private Shared Property _lastNames As New List(Of String)
    Private Shared Function getRandomFirstName(isMale As Boolean) As String
        If _boyFirstNames.Count = 0 Then _boyFirstNames = fileget("data/boyFirstNames.txt")
        If _girlFirstNames.Count = 0 Then _girlFirstNames = fileget("data/girlFirstNames.txt")

        Dim firstNames As List(Of String)
        If isMale = True Then firstNames = _boyFirstNames Else firstNames = _girlFirstNames
        Dim roll As Integer = rng.Next(firstNames.Count)
        getRandomFirstName = firstNames(roll)
        firstNames.RemoveAt(roll)
    End Function
    Private Shared Function getRandomLastName() As String
        If _lastNames.Count = 0 Then _lastNames = fileget("data/lastNames.txt")

        Dim roll As Integer = rng.Next(_lastNames.Count)
        getRandomLastName = _lastNames(roll)
        _lastNames.RemoveAt(roll)
    End Function

    Friend Property player As player
    Friend Property squad As squad

    Private Property _health As Integer = 10
    Private Property _sanity As Integer = 10
    Private Property _morale As Integer = 10
    Friend Sub addPenalty(valueStr As String, value As Integer)
        Select Case valueStr.ToLower
            Case "health" : _health -= value
            Case "sanity" : _sanity -= value
            Case "morale" : _morale -= value
        End Select

        If _health <= 0 OrElse _sanity <= 0 OrElse _morale <= 0 Then dead()
    End Sub
    Friend Sub addPenalty(valueStr As String)
        Dim rawstr As String() = valueStr.Split(" ")
        addPenalty(rawstr(0), CInt(rawstr(1)))
    End Sub
    Private Sub dead()
        _squad.removeAgent(Me)
    End Sub

    Friend ReadOnly Property upkeep As Integer
        Get
            Return 50
        End Get
    End Property

    Private Property _disposition As New disposition
    Private Property _relationships As New Dictionary(Of agent, relationship)
    Friend Sub changeRelationship(agent As agent, choiceComponent As choiceComponent)
        If _relationships.ContainsKey(agent) = False Then _relationships.Add(agent, New relationship)

        With _relationships(agent)
            Select Case _disposition.getReaction(choiceComponent)
                Case "Fear" : .fearRespect -= 1
                Case "Respect" : .fearRespect += 1
                Case "Admire" : .loathingAdmiration += 1
                Case "Loathe" : .loathingAdmiration -= 1
            End Select
        End With
    End Sub
    Private Property _training As New Dictionary(Of skill, Integer)
    Friend ReadOnly Property training As Dictionary(Of skill, Integer)
        Get
            Return _training
        End Get
    End Property
    Friend Sub setTraining(skill As skill, value As Integer)
        If _training.ContainsKey(skill) = False Then _training.Add(skill, value) Else _training(skill) = value
    End Sub
    Private Property _equipment As New List(Of equipment)
    Friend Function bonus(choiceComponent As choiceComponent) As Integer
        Dim total As Integer = 0
        For Each equipment In _equipment
            If equipment.bonuses.ContainsKey(choiceComponent) = True Then total += equipment.bonuses(choiceComponent)
        Next
        Return total
    End Function
    Friend Function bonus(skill As skill) As Integer
        Dim total As Integer = 0
        If _training.ContainsKey(skill) Then total += _training(skill)
        total += bonus(skill.action)
        total += bonus(skill.approach)
        Return total
    End Function
End Class
