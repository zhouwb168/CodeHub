path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.GsmOrigin.Web"
appcmd delete site "Wodeyun.Project.GsmOrigin.Web"

appcmd add apppool /name:"Wodeyun.Project.GsmOrigin.Web"
appcmd set apppool "Wodeyun.Project.GsmOrigin.Web"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.GsmOrigin.Web"^
                /bindings:http/*:15002:^
                /physicalPath:"%cd%\Wodeyun.Project.GsmOrigin.Web"
appcmd set site "Wodeyun.Project.GsmOrigin.Web"^
                /[path='/'].applicationPool:"Wodeyun.Project.GsmOrigin.Web"
