﻿Public Class missionStage
    Public Sub New()
    End Sub
    Public Sub New(aName As String, aDifficulty As Integer, aTimeCost As Integer, aBonuses As Dictionary(Of choiceComponent, Integer), aPenalty As String, aSeverity As Integer)
        name = aName
        difficulty = aDifficulty
        timeCost = aTimeCost
        If aBonuses Is Nothing = False Then bonuses = aBonuses
        penalty = aPenalty
        severity = aSeverity
    End Sub
    Public Overrides Function ToString() As String
        Return name & " (TN " & difficulty & ")"
    End Function

    Friend Property name As String
    Friend Property mission As mission
    Friend Property difficulty As Integer
    Friend Property timeCost As Integer
    Friend Property timeProgress As Integer
    Friend Property bonuses As New Dictionary(Of choiceComponent, Integer)

    Friend Property penalty As String
    Friend Property severity As Integer
    Private Shared Function getRandomMinorPenalty(skill As skill, Optional additionalPenalties As List(Of String) = Nothing) As String
        Dim possibilities As New List(Of String)
        If additionalPenalties Is Nothing = False Then possibilities.AddRange(additionalPenalties)

        If skill.approach = choiceComponent.Supernatural Then possibilities.Add("sanity")
        If skill.action = choiceComponent.Violence Then possibilities.Add("health")
        If skill.action = choiceComponent.Guile OrElse skill.action = choiceComponent.Diplomacy Then possibilities.Add("morale")

        Return possibilities(rng.Next(possibilities.Count))
    End Function

    Friend Function tick(agent As agent, skill As skill) As String
        timeProgress += 1
        If timeProgress >= timeCost Then Return roll(agent, skill) Else Return ""
    End Function
    Private Function roll(agent As agent, skill As skill) As String
        Dim rawRoll As Integer = rollDice("3d6")
        Dim bonus As Integer = agent.bonus(skill)
        If bonuses.ContainsKey(skill.action) Then bonus += bonuses(skill.action)
        If bonuses.ContainsKey(skill.approach) Then bonus += bonuses(skill.approach)

        Dim total As Integer = rawRoll + bonus
        Dim result As missionStageResult = getResult(total)
        If result = missionStageResult.Complicated Then
            Dim minorPenalty As String = missionStage.getRandomMinorPenalty(skill)
            Return "Complicated " & minorPenalty & " " & severity
        ElseIf result = missionStageResult.Failure Then
            Return "Failure " & penalty & " " & severity
        Else
            Return "Success"
        End If
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
End Class

Public Enum missionStageResult
    Failure = 1
    Complicated = 2
    Success = 3
End Enum