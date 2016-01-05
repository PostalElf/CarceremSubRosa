Public Class missionStage
    Public Sub New()
    End Sub
    Public Sub New(aName As String, aDifficulty As Integer, aTimeCost As Integer, aBonuses As Dictionary(Of choiceComponent, Integer), aPenalty As Dictionary(Of String, Integer))
        name = aName
        difficulty = aDifficulty
        timeCost = aTimeCost
        If aBonuses Is Nothing = False Then bonuses = aBonuses
        penalties = aPenalty
    End Sub
    Public Sub New(aName As String, aDifficulty As Integer, aTimeCost As Integer, aBonuses As Dictionary(Of choiceComponent, Integer), aPenalty As String, aSeverity As Integer)
        name = aName
        difficulty = aDifficulty
        timeCost = aTimeCost
        If aBonuses Is Nothing = False Then bonuses = aBonuses
        penalties.Add(aPenalty, aSeverity)
    End Sub
    Public Overrides Function ToString() As String
        Return name & " (TN " & difficulty & ")"
    End Function

    Friend Property name As String
    Friend Property mission As mission
    Friend ReadOnly Property city As city
        Get
            If mission Is Nothing = False Then Return mission.city Else Return Nothing
        End Get
    End Property
    Private Property _difficulty As Integer
    Friend Property difficulty As Integer
        Get
            Return _difficulty + city.missionDifficultyModifier
        End Get
        Set(value As Integer)
            _difficulty = value
        End Set
    End Property
    Friend Property timeCost As Integer
    Friend Property timeProgress As Integer
    Private ReadOnly Property _timeProgressPerTick As Integer
        Get
            Return 10 + city.missionProgressModifier
        End Get
    End Property
    Friend Property bonuses As New Dictionary(Of choiceComponent, Integer)

    Friend Property penalties As New Dictionary(Of String, Integer)
    Private Shared Function getRandomMinorPenalty(skill As skill, Optional additionalPenalties As List(Of String) = Nothing) As String
        Dim possibilities As New List(Of String)
        If additionalPenalties Is Nothing = False Then possibilities.AddRange(additionalPenalties)

        If skill.approach = choiceComponent.Supernatural Then possibilities.Add("sanity")
        If skill.action = choiceComponent.Violence Then possibilities.Add("health")
        If skill.action = choiceComponent.Guile OrElse skill.action = choiceComponent.Diplomacy Then possibilities.Add("morale")

        Dim severity As Integer = rng.Next(1, 4)

        Return possibilities(rng.Next(possibilities.Count)) & " " & severity
    End Function

    Friend Function tick(agent As agent, skill As skill) As missionStageResult
        timeProgress += _timeProgressPerTick
        If timeProgress >= timeCost Then Return roll(agent, skill) Else Return Nothing
    End Function
    Private Function roll(agent As agent, skill As skill) As missionStageResult
        Dim city As city = agent.squad.city
        Dim rawRoll As Integer = rollDice("3d6")
        Dim bonus As Integer = agent.bonus(skill)
        If bonuses.ContainsKey(skill.action) Then bonus += bonuses(skill.action)
        If bonuses.ContainsKey(skill.approach) Then bonus += bonuses(skill.approach)

        Dim total As Integer = rawRoll + bonus
        Dim result As missionStageResult = getResult(total)
        Return result
    End Function
    Private Function getResult(rollTotal As Integer) As missionStageResult
        If rollTotal >= difficulty + 2 Then
            Return missionStageResult.Success
        ElseIf rollTotal < difficulty + 2 AndAlso rollTotal >= difficulty - 2 Then
            Return missionStageResult.Complicated
        Else
            Return missionStageResult.Failure
        End If
    End Function
    Friend Function getPenalties(result As missionStageResult, skill As skill) As List(Of String)
        Dim total As New List(Of String)
        Select Case result
            Case missionStageResult.Success
                Return Nothing

            Case missionStageResult.Complicated
                total.Add(getRandomMinorPenalty(skill))

            Case missionStageResult.Failure
                For Each kvp In penalties
                    total.Add(kvp.Key.ToString & " " & kvp.Value)
                Next
        End Select
        Return total
    End Function
End Class

Public Enum missionStageResult
    Failure = 1
    Complicated = 2
    Success = 3
End Enum