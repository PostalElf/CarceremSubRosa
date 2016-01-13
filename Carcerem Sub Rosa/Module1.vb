Module Module1

    Sub Main()
        Console.SetWindowSize(80, 70)

        Dim world As world = world.buildWorld
        Dim player As New player(world)
        player.addConsequence("player departmentLevelMax HR 2")
        player.addConsequence("player govtFunding 1000")
        player.departmentBudgets(department.HR) = 2000

        Dim researchProject As New researchProject("Top-Secret Project", 1000, Nothing, Nothing)
        player.devAddResearchProjectOpen(researchProject)
        player.changeResearchProject(researchProject)

        Dim city1 As city = world.cities(continent.China)(2)
        Dim city2 As city = world.cities(continent.AmericaN)(0)
        Dim citysite1 As citysite = citysite.buildCitysite(player, city2)
        Dim citysite2 As citysite = citysite.buildCitysite(player, city1)
        Dim citysite3 As citysite = citysite.buildCitysite(player, city1)
        Dim citysite4 As citysite = citysite.buildCitysite(player, city1)
        Dim citysite5 As citysite = citysite.buildCitysite(player, city2)
        Dim shell1 As shellcompany = shellcompany.buildShellcompany(player, city1)
        Dim shell2 As shellcompany = shellcompany.buildShellcompany(player, city2)
        Dim research As researchlab = researchlab.buildResearchlab(citysite1, shell2)
        Dim factory As factory = factory.buildFactory(citysite2, shell1)
        Dim safehouse As safehouse = safehouse.buildSafehouse(citysite3, shell1)
        Dim tacsupport As tacsupport = tacsupport.buildTacsupport(citysite4, shell1)
        Dim laundry As laundry = laundry.buildLaundry(shell1)

        Dim blueprint As New product("Sheepskin")
        player.devAddBlueprint(blueprint)
        player.createTradeRoute(blueprint, factory, city2)

        Dim agent1 As agent = agent.buildRandomAgent(player)
        Dim agent2 As agent = agent.buildRandomAgent(player)
        Dim squad As squad = squad.buildRandomSquad(player, city2, New List(Of agent) From {agent1, agent2})
        agent1.changeRelationship(agent2, choiceComponent.Practical)
        squad.moveTo(city1)
        squad.addConsequence("squad travelspeed +10000")

        Dim stage1 As New missionStage("Dress Up", 5, 10, New Dictionary(Of choiceComponent, Integer) From {{choiceComponent.Diplomacy, 1}}, "mission progress 0")
        Dim stage2 As New missionStage("Travel to the Party", 7, 10, Nothing, "mission progress 0")
        Dim stage3 As New missionStage("Eat!", 15, 10, Nothing, "mission progress -20")
        Dim stage4 As New missionStage("Assassinate VIP", 18, 10, Nothing, "agent health -5")
        Dim stageStack As New Stack(Of missionStage)
        stageStack.Push(stage4)
        stageStack.Push(stage3)
        stageStack.Push(stage2)
        stageStack.Push(stage1)
        Dim mission As New mission("Sample Cheese & Wines", city1, stageStack)
        mission.squad = squad
        mission.setAgent(squad.agents(0))

        While True
            While interrupt.Count > 0
                Console.WriteLine()
                interrupt.Pop.handle()
            End While

            Console.Clear()
            world.consoleReportTime(0)
            Console.WriteLine()
            player.fullConsoleReport(1)
            Console.ReadKey()

            world.timeTick()
            player.tick()
        End While
    End Sub
End Module
