path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.GsmArea.Web"
appcmd delete site "Wodeyun.Project.GsmArea.Web"

appcmd add apppool /name:"Wodeyun.Project.GsmArea.Web"
appcmd set apppool "Wodeyun.Project.GsmArea.Web"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.GsmArea.Web"^
                /bindings:http/*:15001:^
                /physicalPath:"%cd%\Wodeyun.Project.GsmArea.Web"
appcmd set site "Wodeyun.Project.GsmArea.Web"^
                /[path='/'].applicationPool:"Wodeyun.Project.GsmArea.Web"
