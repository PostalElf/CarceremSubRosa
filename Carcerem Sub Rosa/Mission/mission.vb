Public Class mission
    Public Sub New()
    End Sub
    Public Sub New(aName As String, aCity As city, aMissionStages As Stack(Of missionStage))
        name = aName
        aCity.addMission(Me)
        missionStages = aMissionStages

        If aMissionStages Is Nothing = False Then
            For Each stage In missionStages
                stage.mission = Me
            Next
        End If
    End Sub
    Friend Function briefReport() As String
        Return """" & name & """ in " & city.briefReport
    End Function
    Public Overrides Function ToString() As String
        Return "Mission " & name
    End Function

    Friend Property name As String
    Friend Property city As city
    Friend Property squad As squad
    Friend Property missionStages As New Stack(Of missionStage)

    Friend Sub tick()
        If squad.city.Equals(city) Then
            Dim currentMissionStage As missionStage = missionStages.Pop
            Dim agent As agent = squad.agents(rng.Next(squad.agents.Count))
            Dim skill As New skill(skill.getRandomAction, skill.getRandomApproach)

            Dim result As String = currentMissionStage.tick(agent, skill)
            If result = "" Then
                'timer still ticking
                missionStages.Push(currentMissionStage)
            ElseIf result = "Success" Then
                'mission success
                'MsgBox(currentMissionStage.name & " success.")
            Else
                'complicated or failure; apply penalties to agent
                Dim rawstr As String() = result.Split(" ")
                agent.addPenalty(rawstr(1), CInt(rawstr(2)))

                'push back onto stack if failure
                If rawstr(0) = "Failure" Then
                    currentMissionStage.timeProgress = 0
                    missionStages.Push(currentMissionStage)
                End If
                'MsgBox(result)
            End If

            If missionStages.Count = 0 Then
                missionSuccess()
            End If
        End If
    End Sub
    Private Sub addPenalty(agent As agent, penalty As String, value As Integer)
        Select Case penalty.ToLower
            Case "health" Or "sanity" Or "morale" : agent.addPenalty(penalty, value)
            Case Else : agent.squad.city.addPenalty(penalty, value)
        End Select
    End Sub
    Private Sub missionSuccess()
        MsgBox("Mission Success")
    End Sub
End Class
