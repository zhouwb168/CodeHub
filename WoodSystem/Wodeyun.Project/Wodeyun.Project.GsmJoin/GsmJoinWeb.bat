path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.GsmJoin.Web"
appcmd delete site "Wodeyun.Project.GsmJoin.Web"

appcmd add apppool /name:"Wodeyun.Project.GsmJoin.Web"
appcmd set apppool "Wodeyun.Project.GsmJoin.Web"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.GsmJoin.Web"^
                /bindings:http/*:17024:^
                /physicalPath:"%cd%\Wodeyun.Project.GsmJoin.Web"
appcmd set site "Wodeyun.Project.GsmJoin.Web"^
                /[path='/'].applicationPool:"Wodeyun.Project.GsmJoin.Web"
