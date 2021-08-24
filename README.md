# buyscout

An example how to use Windows Terminal to run a bunch of processes for a full dev environment

## The domain...
Buyscout is a collaboration platform to manage stuff that needs to be acquired to make life worth living

Key features are:
 - Share lists with other
 - Management for adding items
 - In-store optiomized list for ez swipe actions
 - Suggestions based on previous purchase patterns
 - 


## The tech
 - dotnet 5 API for BFF including SignalR for near real-time messaging
 - dotnet 5 apps for micro-service and message driven PoC ProtoBuf + rabbitmq
 - MS SignalR for 
 - Node JS monitoring app utilizing the SignalR
 - Mailhog fake mail for testing invites / OTP-logins
 - Svelte (TBD) poc front-end app




 ## Runtime



```
# Set environment vars and start the node monitor app
> $env:API_URL = 'http://localhost:5000' ; node .\index.js
```


 ## Windows Terminal imperative setup

```
> setx MY_CODE_ROOT "C:\path\to\source\code"
```


```
> # For PowerShell
> $env:MY_CODE_ROOT\node-monitor
> $env:BUYSCOUT__API_URL='http://localhost:6000' ; nodemon .\index.js
```
 
```
>  wt split-pane `; move-focus left `; split-pane -d ~/
```

wt --title MyFreshTabs `; split-pane -d "C:\Users\JonasJerndin\source\buyscout\src\node-monitor" -p "Command Promt" -H cmd /k $env:BUYSCOUT__API_URL='http://localhost:6000' ; nodemon .\index.js  `; split-pane -d "~/source/buyscout/src/dotnet" -p "Command Promt" -H cmd /k echo testing


wt -d "C:\Users\JonasJerndin\source\buyscout\src\dotnet\BuyScout.API" cmd /k dotnet watch run `; split-pane -H -d "C:\Users\JonasJerndin\source\buyscout\src\dotnet\BuyScout.API" cmd /k dotnet watch run `; split-pane -H -d "C:\Users\JonasJerndin\source\buyscout\src\dotnet\BuyScout.Processors" cmd /k dotnet watch run `; split-pane -H -d "C:\Users\JonasJerndin\source\buyscout\src\infra" -p "Windows PowerShell" cmd /k docker-compose -f infra-services.yml up `; split-pane -H -d "C:\Users\JonasJerndin\source\buyscout\src\svelte-frontend" cmd /k npm run dev


wt -d "C:\Users\JonasJerndin\source\buyscout\src\node-monitor" -p "Windows PowerShell" `; split-pane -H -d "C:\Users\JonasJerndin\source\buyscout\src\dotnet\BuyScout.API" cmd /k dotnet watch run `; split-pane -d "C:\Users\JonasJerndin\source\buyscout\src\dotnet\BuyScout.Processors" cmd /k dotnet watch run `; split-pane -V -d "C:\Users\JonasJerndin\source\buyscout\src\svelte-frontend" cmd /k npm run dev `; move-focus up `; split-pane -V -d "C:\Users\JonasJerndin\source\buyscout\src\infra" -p "Windows PowerShell" cmd /k docker-compose -f infra-services.yml up


wt -d "C:\Users\JonasJerndin\source\buyscout\src\node-monitor" -p "Windows PowerShell" -c $env:BUYSCOUT__API_URL='http://localhost:6000' ; nodemon .\index.js


wt -d "C:\Users\JonasJerndin\source\buyscout\src\node-monitor" -p "Windows PowerShell" cmd /k nodemon .\index.js `; split-pane -H -d "C:\Users\JonasJerndin\source\buyscout\src\dotnet\BuyScout.API" cmd /k dotnet watch run `; split-pane -d "C:\Users\JonasJerndin\source\buyscout\src\dotnet\BuyScout.Processors" cmd /k dotnet watch run `; split-pane -V -d "C:\Users\JonasJerndin\source\buyscout\src\svelte-frontend" cmd /k npm run dev `; move-focus up `; split-pane -V -d "C:\Users\JonasJerndin\source\buyscout\src\infra" -p "Windows PowerShell" cmd /k docker-compose -f infra-services.yml up


wt -p "Windows PowerShell" cmd /k echo win1 `; split-pane -H -p "Windows PowerShell" cmd /k echo win2 `; split-pane -V  -p "Windows PowerShell" cmd /k echo win3 `; split-pane -V  -p "Windows PowerShell" cmd /k echo win4 `; move-focus up `; split-pane -V  -p "Windows PowerShell" cmd /k echo win5