﻿Public Class modifier
    Public Sub New(modifierName As String, aParent As List(Of modifier), rawstring As String)
        Dim lines As String() = rawstring.Split

        name = modifierName
        parent = aParent
        typeName = lines(0)

        Select Case typeName
            Case "UnlockBlueprint"
                unlockName = lines(1)
        End Select
    End Sub

    Friend Property name As String
    Friend Property typeName As String
    Friend Property parent As List(Of modifier)

    Friend Property unlockName As String
End Class