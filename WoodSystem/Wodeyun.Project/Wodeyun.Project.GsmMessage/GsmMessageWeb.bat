path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.GsmMessage.Web"
appcmd delete site "Wodeyun.Project.GsmMessage.Web"

appcmd add apppool /name:"Wodeyun.Project.GsmMessage.Web"
appcmd set apppool "Wodeyun.Project.GsmMessage.Web"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.GsmMessage.Web"^
                /bindings:http/*:15006:^
                /physicalPath:"%cd%\Wodeyun.Project.GsmMessage.Web"
appcmd set site "Wodeyun.Project.GsmMessage.Web"^
                /[path='/'].applicationPool:"Wodeyun.Project.GsmMessage.Web"
