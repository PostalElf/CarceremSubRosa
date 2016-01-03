Public Class researchProject
    Public Sub New()
    End Sub
    Public Sub New(aName As String, aCost As Integer, aRequirements As List(Of requirement), aChildProjectNames As List(Of String), Optional aModifiers As List(Of modifier) = Nothing)
        name = aName
        _cost = aCost
        _requirements = aRequirements
        childProjectNames = aChildProjectNames
        modifiers = aModifiers
    End Sub
    Friend Shared Function fileget(name As String)
        Dim raw As List(Of String()) = csvFileget("data/research.csv")
        For Each line In raw
            If line(0) = name Then
                Dim researchProject As New researchProject
                With researchProject
                    Dim n As New rollingCounter(0)
                    .name = line(n.Tick)

                    ._cost = CInt(line(n.Tick))

                    Dim childrenRaw As String() = line(n.Tick).Split(";")
                    For Each subline In childrenRaw
                        subline = subline.Trim
                        .childProjectNames.Add(subline)
                    Next

                    While n.Tick < line.Length AndAlso n.Last <> ""
                        Dim modifier As New modifier(.name, .modifiers, line(n.Last))
                        .modifiers.Add(modifier)
                    End While
                End With
                Return researchProject
            End If
        Next
        Return Nothing
    End Function
    Friend Function briefReport() As String
        Return name & " (" & _progress & "/" & _cost & ")"
    End Function

    Friend Property player As player
    Friend Property name As String
    Private Property _progress As Integer
    Private Property _cost As Integer
    Friend Sub addProgress(value As Integer)
        _progress += value
    End Sub
    Friend ReadOnly Property spilloverProgress As Integer
        Get
            Return _progress - _cost
        End Get
    End Property

    Friend Property childProjectNames As New List(Of String)
    Friend Property modifiers As New List(Of modifier)
    Private Property _requirements As New List(Of requirement)
    Friend ReadOnly Property requirementsMet As Boolean
        Get
            If _requirements Is Nothing OrElse _requirements.Count = 0 Then Return True

            Dim researchRequirements As List(Of requirement) = player.getActiveResearchRequirements
            For Each requirement In _requirements
                If researchRequirements.Contains(requirement) = False Then Return False
            Next
            Return True
        End Get
    End Property
End Class
