namespace WebSharper.Dojo.Sample

open WebSharper
open WebSharper.JavaScript

[<JavaScript>]
module Client =

    // XHtml type provider allows strongly typed access to our layout
    type HomePage = Dojo.XHtml<"index.html">
   
    // Creates a strongly typed call to require function
    type Requires = 
        Dojo.Require<"dijit/registry, dojo/parser, 
            dijit/layout/BorderContainer, dijit/layout/TabContainer, dijit/layout/ContentPane, 
            dojox/charting/Chart, dojox/charting/plot2d/Lines, dojox/charting/plot2d/Markers, dojox/charting/axis2d/Default, 
            dojo/domReady!">

    let Main =
        // we initialize the page on Dom load by having "dojo/domReady!" in the requires
        Requires.Run(fun req ->
            // Inilialize widgets
            req.``dojo/parser``.Parse() |> ignore
            
            Console.Log("Dojo parse ok.")

            let chartData = [| 10000; 9200; 11811; 12000; 7662; 13887; 14200; 12222; 12000; 10009; 11288; 12099 |]

            // XHtml provider has JQuery lookups for all the nodes with ids in our document
            let chart = req.``dojox/charting/Chart``(HomePage.JQuery.chartNode.Get(0))
            
            chart
                // New is a shorthand for creating custom JS objects, also possible to use Dojo.dojox.charting.plot2d.Default.Config
                .AddPlot("default", New [ "type" => "Lines"; "markers" => true ])
                .AddAxis("x")
                // New is a shorthand for creating custom JS objects, also possible to use Dojo.dojox.charting.axis2d.Default.Config
                .AddAxis("y", New [ "min" => 5000; "max" => 15000; "vertical" => true; "fixLower" => "major"; "fixUpper" => "major" ])
                .AddSeries("SalesThisDecade", chartData)
                .Render()
                |> ignore
            
            // Dijit widget lookup uses the byId function, this has to be passed to provided type constructor
            // All nodes with id and data-dojo-type set will appear as correctly typed properties 
            let widgets = HomePage(req.``dijit/registry``.ById)  
           
            widgets.contentTabs.AddChild(
                req.``dijit/layout/ContentPane``(
                    // Configuration objects has typed helpers
                    Dojo.dijit.layout.ContentPane.Config(
                        Title = "Tab added programmatically"
                    )
                )
            )
       )
