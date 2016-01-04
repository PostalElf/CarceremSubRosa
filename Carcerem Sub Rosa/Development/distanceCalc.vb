Imports System.IO
Imports System.Net


Public Class distanceCalc
    Friend Shared Sub saveDistances(world As world)
        Dim allCities As New List(Of city)
        For Each kvp In world.cities
            For Each city In kvp.Value
                allCities.Add(city)
            Next
        Next

        Dim travelDistances As New Dictionary(Of String, Dictionary(Of String, Integer))
        For Each city In allCities
            Dim destinations As New Dictionary(Of String, Integer)
            For Each subcity In allCities
                destinations.Add(subcity.name, getDistance(city.name, subcity.name))
            Next
            travelDistances.Add(city.name, destinations)
        Next

        Using writer As New System.IO.StreamWriter("data/distances.csv")
            For Each kvp In travelDistances
                Dim origin As String = kvp.Key.ToString
                Dim destinations As Dictionary(Of String, Integer) = kvp.Value
                For Each kvp2 In destinations
                    Dim destination As String = kvp2.Key
                    Dim distance As Integer = kvp2.Value

                    writer.WriteLine(origin & "," & destination & "," & distance)
                Next
            Next
        End Using
    End Sub
    Private Shared Function getDistance(origin As String, destination As String)
        If origin = destination Then Return 0

        Dim pathname As String = "http://www.distance24.org/route.json?stops=" & origin & "|" & destination
        Dim request As WebRequest = WebRequest.Create(pathname)
        Dim response As WebResponse = request.GetResponse
        Dim dataStream As Stream = response.GetResponseStream()
        Dim reader As New StreamReader(dataStream)
        Dim responseFromServer As String = reader.ReadToEnd()
        reader.Close()
        response.Close()

        Dim index As Integer = responseFromServer.LastIndexOf("""distances"":")
        Dim rawstr As String = responseFromServer.Substring(index)
        Dim i As Integer = rawstr.IndexOf("[")
        Dim f As String = rawstr.Substring(i + 1, rawstr.IndexOf("]", i + 1) - i - 1)
        Return CInt(f)
    End Function
End Class
