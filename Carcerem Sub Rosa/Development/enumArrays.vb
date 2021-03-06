﻿Public Class enumArrays
    Friend Shared Property continentArray As Array = System.Enum.GetValues(GetType(continent))
    Friend Shared Property departmentArray As Array = System.Enum.GetValues(GetType(department))
    Friend Shared Property choiceComponentArray As Array = System.Enum.GetValues(GetType(choiceComponent))
    Friend Shared Property godArray As Array = System.Enum.GetValues(GetType(god))

    Friend Shared Function getEnumFromString(str As String, enumArray As Array) As [Enum]
        For Each item In enumArray
            Dim itemStr As String = item.ToString.ToLower

            If itemStr = str.ToLower Then Return item
            If stripS(itemStr) = str.ToLower Then Return item
        Next
        Return Nothing
    End Function
End Class
