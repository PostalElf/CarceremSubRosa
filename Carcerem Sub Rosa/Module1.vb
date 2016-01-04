Imports System.IO
Imports System.Net

Module Module1

    Sub Main()
        Console.SetWindowSize(80, 70)

        Dim world As world = world.buildWorld
        distanceCalc(world)
        'Dim player As New player
        'Dim researchProject As New researchProject("Top-Secret Project", 1000, Nothing, Nothing)
        'player.devAddResearchProjectOpen(researchProject)
        'player.changeResearchProject(researchProject)

        'Dim city1 As city = world.cities(continent.AmericaN)(1)
        'Dim city2 As city = world.cities(continent.China)(0)
        'Dim citysite1 As New citysite(player, city1)
        'Dim citysite2 As New citysite(player, city1)
        'Dim citysite3 As New citysite(player, city1)
        'Dim citysite4 As New citysite(player, city1)
        'Dim shell1 As New shellcompany("", player, city1)
        'Dim shell2 As New shellcompany("", player, city2)
        'Dim research As New researchlab("", citysite1, shell2)
        'Dim factory As New factory("", citysite2, shell1)
        'Dim safehouse As New safehouse("", citysite3, shell1)
        'Dim tacsupport As New tacsupport("", citysite4, shell1)
        'Dim laundry As New laundry("", shell1)

        'Dim blueprint As New product("Sheepskin")
        'player.addBlueprint(blueprint)
        'city2.addDemand(1)
        'player.createTradeRoute(blueprint, factory, city2)

        'Dim agent1 As agent = agent.buildRandomAgent(player)
        'Dim agent2 As agent = agent.buildRandomAgent(player)
        'Dim squad As squad = squad.buildRandomSquad(player, city1)
        'squad.addAgent(agent1)
        'squad.addAgent(agent2)
        'agent1.changeRelationship(agent2, choiceComponent.Practical)
        'squad.moveTo(city2)

        'While True
        '    Console.Clear()
        '    player.consoleReport(1)
        '    Console.WriteLine(vbCrLf)

        '    shell1.consoleReport(1)
        '    For Each holding In shell1.holdings
        '        Console.WriteLine()
        '        holding.consoleReport(2)
        '    Next
        '    Console.WriteLine(vbCrLf)

        '    shell2.consoleReport(1)
        '    For Each holding In shell2.holdings
        '        Console.WriteLine()
        '        holding.consoleReport(2)
        '    Next
        '    Console.WriteLine(vbCrLf)

        '    squad.consoleReport(1)
        '    Console.ReadKey()

        '    player.tick()
        'End While
    End Sub

    Private Sub distanceCalc(world As world)
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
    End Sub
    Private Function getDistance(origin As String, destination As String)
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
End Module
