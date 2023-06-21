# Blazor API client

- the microsoft's framework for the single page web application (spa)
-  In this project i'm gon build a app that will call no.of ApIs 
-  the app will be calling tREST api and the GraphQL api.
-  there are bunch of things we gon perform (build on  top of the blazor api)
- 1. fetch the space x data , 2. base 64 converter 
- forr the data service implementation we will be using both the rest and the GraphQL api.
- the "kestrel" is the web server (dotnet ) to serve our application.
- the blazor requires no plugins and supported all browsers
- The Blazor comes in 2 flavors 1. blazor client(wasm) and the 2. blazor server 
- 1. Blazor server - apps hosted on the AsP.net server, client doesn't run the code ("thin" ), client and server interacts using the SignalR(esssentially WebSocket), only the dom is updated, client is presentation only, works on all browsers
- 2. Blazor Client - App code download in full, runs on top of the wasm. the server can be anything that doesn't need to be the ASP .net, can work offline. Run on all the wasm browsers. 
- The same app can be deployed in either (server or client) configuration.
  
# SignalR

- open source lib from microsoft.com
- used for the real time async apps
- uses best available transport (aims for websocket, falls back to other method)
- if using web socket the connection is bidirectional, persistent or "nailed-up"
  
# WASM

- open standard, portable binary code format
- enabling high performance apps on web pages
- wwwc recommendations along with the html, js, css
- theoratically support any language
- aims to execute at native speed.


# why Blazor ? 

- when we wana use c# to dev the web app
- quickly prototype apps, "proof of concept"

# App

- ```dotnet new blazorwasm -n projectName```
- to run the dev server ```dotnet run``` starts the kestrel server/ listening on port 5000
- the first time the app loaded in the browser, we can see in the console it will be like 9mb in size, and all the resources loaded, and cached so the subsequent cache will be lot quicker.
- blazor webassembly.js --> downloads the .net runtime and download the compiled app (.dll)
- program.cs - main() executes - build wasm host, specify root component (app.razor), http client registered in DI container, App runs in <app> tag in index.html

# Base 64 converter (component)

- the first comp is the base 64 converter(Pages/Base64converter.razor) encoding and decoding fns
-the icons we can grab from "useicons.com/open"
- the converter convertTo fn is Encoding.UTF8.().getBytes() and use the plain text and convert to the base64 encoded byte [] -- Convert.ToBase64String()
- and the convertFrom() is vice versa.(the opposite of the converterTo()) .. get the string of base64 encoded bytes.

# APIs

- for the rest api im gon use the space x rest api - https://api/spacex.land/rest/ the swagger ui
- and for the graphql the api is https://api/spacex.land/graphql/ .. unlike the rest when using the graphql the http method would always be post, whether we get,post,put or delete it doesn't matter.. its always the post req in graphql
- and in graphql there is only one endpt https://api/spacex.land/qraphql/ unlike rest where for diff req the endpt changes ex. for get the details about rockets its /rest/rockets/ and for the launches /rest/launches/ 
- once we select the schema option in the insomnia -- refresh schema.. this will pull down the entire specification of the endpoint
- and we can write our query in the body -- like query{launches{}} will brings the array of launches obj. and from that we can access the fields.
- we can use "quick type " site to use our json obj and it creates the c# code for us.


# Http client Factory

- introduced to help manage the creation of http client and avoid the socket exhaustion problem.
- in short it manages the underlying http message handler for us.
- ```dotnet add package Microsoft.Extensions.Http```
- and then register the http client factory in the program.cs as the same way as the http client. --> .services.AddHttpClient<> and the impl we can mention the Interface and the actual class for the interface impl

# Graphql api

- "strawberry shake" api(https://chillicream.com), the team that developed the hotchocolate graphql api framework, it takes away the lota heavy lifting for the qraphql.
- The fetching of the data from the api is slightly complicated in the graphql in comparision with the REST API(which is pretty strigh forward just send a get req from our http client factory)
- where as in the graphql first need to make the query obj, and then launch the query object(serializing), then send(the launch query) a post Async req from the http client.o
- then finally once we get the res back then deserializing the data(GQLdataDTO). and read the content(ReadAsStreamAsync()).