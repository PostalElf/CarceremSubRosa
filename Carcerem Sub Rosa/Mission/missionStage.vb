Public Class missionStage
    Implements IDisposable
    Public Sub New()
    End Sub
    Public Sub New(aName As String, aDifficulty As Integer, aTimeCost As Integer, aBonuses As Dictionary(Of choiceComponent, Integer), aPenalty As List(Of String))
        name = aName
        difficulty = aDifficulty
        timeCost = aTimeCost
        If aBonuses Is Nothing = False Then bonuses = aBonuses
        penalties = aPenalty
    End Sub
    Public Sub New(aName As String, aDifficulty As Integer, aTimeCost As Integer, aBonuses As Dictionary(Of choiceComponent, Integer), aPenalty As String)
        name = aName
        difficulty = aDifficulty
        timeCost = aTimeCost
        If aBonuses Is Nothing = False Then bonuses = aBonuses
        penalties.Add(aPenalty)
    End Sub
    Friend Function briefReport() As String
        Return name & " (" & timeProgress & "/" & timeCost & ")"
    End Function
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

    Friend Property penalties As New List(Of String)
    Private Shared Function getRandomMinorPenalty(approach As choiceComponent, action As choiceComponent, Optional additionalPenalties As List(Of String) = Nothing) As String
        Dim possibilities As New List(Of String)
        If additionalPenalties Is Nothing = False Then possibilities.AddRange(additionalPenalties)

        Dim severity As Integer = rng.Next(1, 4)
        If approach = choiceComponent.Supernatural Then possibilities.Add("agent sanity -" & severity)
        If approach = choiceComponent.Practical Then possibilities.Add("city policeGoodwill -" & severity)
        If approach = choiceComponent.Digital Then possibilities.Add("city mediaGoodwill -" & severity)
        If action = choiceComponent.Violence Then possibilities.Add("agent health -" & severity)
        If action = choiceComponent.Guile OrElse action = choiceComponent.Diplomacy Then possibilities.Add("agent morale -" & severity)

        Return possibilities(rng.Next(possibilities.Count))
    End Function

    Friend Function tick(agent As agent, decisionMatrix As List(Of Dictionary(Of choiceComponent, Integer))) As missionStageResult
        timeProgress += constrain(_timeProgressPerTick, 1, 20)
        If timeProgress >= timeCost Then Return roll(agent, decisionMatrix) Else Return Nothing
    End Function
    Private Function roll(agent As agent, decisionMatrix As List(Of Dictionary(Of choiceComponent, Integer))) As missionStageResult
        Dim approach As choiceComponent = getRandomChoice(decisionMatrix(0))
        Dim action As choiceComponent = getRandomChoice(decisionMatrix(1))

        Dim city As city = agent.squad.city
        Dim rawRoll As Integer = rollDice("3d6")
        Dim bonus As Integer = agent.bonus(approach, action)
        If bonuses.ContainsKey(approach) Then bonus += bonuses(approach)
        If bonuses.ContainsKey(action) Then bonus += bonuses(action)

        Dim total As Integer = rawRoll + bonus
        Dim result As missionStageResult = getResult(total)
        For Each penalty In getPenalties(result, approach, action)
            mission.addConsequence(penalty, Me)
        Next

        Return result
    End Function
    Private Function getRandomChoice(decisionMatrix As Dictionary(Of choiceComponent, Integer))
        Dim total As New List(Of choiceComponent)
        For Each kvp In decisionMatrix
            For n = 1 To kvp.Value
                total.Add(kvp.Key)
            Next
        Next
        Return total(rng.Next(total.Count))
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
    Private Function getPenalties(result As missionStageResult, approach As choiceComponent, action As choiceComponent) As List(Of String)
        Dim total As New List(Of String)
        Select Case result
            Case missionStageResult.Success
                Return total

            Case missionStageResult.Complicated
                total.Add(getRandomMinorPenalty(approach, action))

            Case missionStageResult.Failure
                total.AddRange(penalties)
        End Select
        Return total
    End Function

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
            mission.setAgent(Nothing)
            mission = Nothing
            bonuses = Nothing
            penalties = Nothing
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region
End Class

Public Enum missionStageResult
    Failure = 1
    Complicated = 2
    Success = 3
End Enum