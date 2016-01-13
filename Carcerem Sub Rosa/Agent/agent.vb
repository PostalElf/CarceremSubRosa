Public Class agent
    Implements problemReporter
    Friend Shared Function buildRandomAgent(aPlayer As player) As agent
        Dim agent As New agent
        With agent
            .player = aPlayer
            ._isMale = coinFlip()
            ._age = 21 + rng.Next(13)
            ._maslowKerning = rng.Next(14)
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
    Private _maslowKerning As Integer
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
    Friend Sub addConsequence(consequence As String)
        Dim rawstr As String() = consequence.Split(" ")
        If rawstr(0) <> "agent" Then Exit Sub

        Dim value As Integer = CInt(rawstr(2))
        Select Case rawstr(1)
            Case "health" : _health = constrain(_health + value, 0, 10)
            Case "sanity" : _sanity = constrain(_sanity + value, 0, 10)
            Case "morale" : _morale = constrain(_morale + value, 0, 10)
        End Select
        Debug.Print("Agent " & name & " " & withSign(value) & " " & rawstr(1) & ".")

        If _health <= 0 OrElse _sanity <= 0 OrElse _morale <= 0 Then dead()
    End Sub
    Private Sub dead()
        Dim city As city = squad.city
        If city.squads.Contains(squad) AndAlso city.missions.Count > 0 Then
            For Each mission In city.missions
                If mission.squad.Equals(squad) AndAlso mission.agent.equals(Me) Then mission.setAgent(Nothing)
            Next
        End If

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
    Private Property _training As New Dictionary(Of choiceComponent, Dictionary(Of choiceComponent, Integer))
    Friend Function getTraining(approach As choiceComponent, action As choiceComponent) As Integer
        If _training.ContainsKey(approach) = False Then Return 0
        If _training(approach).ContainsKey(action) = False Then Return 0
        Return _training(approach)(action)
    End Function
    Private Function getHighestTraining() As choiceComponent()
        Dim highest(1) As choiceComponent
        Dim highestValue As Integer = -1

        For Each kvp1 In _training
            For Each kvp2 In kvp1.Value
                Dim approach As choiceComponent = kvp1.Key
                Dim action As choiceComponent = kvp2.Key
                Dim currentBonus As Integer = bonus(approach, action)
                If currentBonus > highestValue Then
                    highest(0) = approach
                    highest(1) = action
                End If
            Next
        Next
        Return highest
    End Function
    Friend Sub setTraining(approach As choiceComponent, action As choiceComponent, value As Integer)
        If _training.ContainsKey(approach) = False Then _training.Add(approach, New Dictionary(Of choiceComponent, Integer))
        If _training(approach).ContainsKey(action) = False Then _training(approach).Add(action, value) Else _training(approach)(action) = value
    End Sub
    Private Property _equipment As New List(Of equipment)
    Friend Function bonus(choiceComponent As choiceComponent) As Integer
        Dim total As Integer = 0
        For Each equipment In _equipment
            If equipment.bonuses.ContainsKey(choiceComponent) = True Then total += equipment.bonuses(choiceComponent)
        Next
        Return total
    End Function
    Friend Function bonus(approach As choiceComponent, action As choiceComponent) As Integer
        Dim total As Integer = 0
        total += getTraining(approach, action)
        total += bonus(approach)
        total += bonus(action)
        Return total
    End Function

    Friend ReadOnly Property decisionMatrix() As List(Of Dictionary(Of choiceComponent, Integer))
        Get
            Dim approaches As New Dictionary(Of choiceComponent, Integer)
            Dim actions As New Dictionary(Of choiceComponent, Integer)
            For Each choiceComponent In enumArrays.choiceComponentArray
                If choiceComponent < 10 Then actions.Add(choiceComponent, 5) Else approaches.Add(choiceComponent, 5)
            Next


            'liked stuff
            approaches(_disposition.getApproach(True)) += 2
            actions(_disposition.getAction(True)) += 2
            If _maslowKerning >= 7 Then
                Dim respect As choiceComponent = _disposition.respect
                If approaches.ContainsKey(respect) Then approaches(respect) += 1 Else actions(respect) += 1
            Else
                Dim admire As choiceComponent = _disposition.admire
                If approaches.ContainsKey(admire) Then approaches(admire) += 1 Else actions(admire) += 1
            End If


            'highest training
            Dim highestTraining As choiceComponent() = getHighestTraining()
            If highestTraining(0) <> Nothing Then
                approaches(highestTraining(0)) += 2
                actions(highestTraining(1)) += 2
            End If


            'compile and send off
            Dim total As New List(Of Dictionary(Of choiceComponent, Integer))
            total.Add(approaches)
            total.Add(actions)
            Return total
        End Get
    End Property
End Class
