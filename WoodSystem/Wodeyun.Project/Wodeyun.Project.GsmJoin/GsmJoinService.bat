path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.GsmJoin.Services"
appcmd delete site "Wodeyun.Project.GsmJoin.Services"

appcmd add apppool /name:"Wodeyun.Project.GsmJoin.Services"
appcmd set apppool "Wodeyun.Project.GsmJoin.Services"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.GsmJoin.Services"^
                /bindings:http/*:16024:^
                /physicalPath:"%cd%\Wodeyun.Project.GsmJoin.Services"
appcmd set site "Wodeyun.Project.GsmJoin.Services"^
                /[path='/'].applicationPool:"Wodeyun.Project.GsmJoin.Services"
