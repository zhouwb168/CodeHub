path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.GsmLine.Web"
appcmd delete site "Wodeyun.Project.GsmLine.Web"

appcmd add apppool /name:"Wodeyun.Project.GsmLine.Web"
appcmd set apppool "Wodeyun.Project.GsmLine.Web"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.GsmLine.Web"^
                /bindings:http/*:15003:^
                /physicalPath:"%cd%\Wodeyun.Project.GsmLine.Web"
appcmd set site "Wodeyun.Project.GsmLine.Web"^
                /[path='/'].applicationPool:"Wodeyun.Project.GsmLine.Web"
