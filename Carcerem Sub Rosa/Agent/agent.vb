Public Class agent
    Implements problemReporter
    Friend Shared Function buildRandomAgent(aPlayer As player) As agent
        Dim agent As New agent
        With agent
            ._player = aPlayer
            ._disposition = disposition.buildRandomDisposition
        End With
        Return agent
    End Function

    Private Property _player As player

    Friend Property name As String Implements problemReporter.name
    Private Property _health As Integer = 10
    Private Property _sanity As Integer = 10
    Private Property _morale As Integer = 10

    Private Property _disposition As New disposition
    Private Property _relationships As New Dictionary(Of agent, relationship)
    Friend Sub changeRelationship(agent As agent, choiceComponent As choiceComponent)
        If _relationships.ContainsKey(agent) = False Then _relationships.Add(agent, New relationship)

        Dim reaction As String = _disposition.getReaction(choiceComponent)
        With _relationships(agent)
            Select Case reaction
                Case "Fear" : .fearRespect -= 1
                Case "Respect" : .fearRespect += 1
                Case "Admire" : .loathingAdmiration += 1
                Case "Loathe" : .loathingAdmiration -= 1
            End Select
        End With
    End Sub
End Class
