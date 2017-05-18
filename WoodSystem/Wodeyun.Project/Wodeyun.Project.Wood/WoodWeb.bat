path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.Wood.Web"
appcmd delete site "Wodeyun.Project.Wood.Web"

appcmd add apppool /name:"Wodeyun.Project.Wood.Web"
appcmd set apppool "Wodeyun.Project.Wood.Web"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.Wood.Web"^
                /bindings:http/*:17002:^
                /physicalPath:"%cd%\Wodeyun.Project.Wood.Web"
appcmd set site "Wodeyun.Project.Wood.Web"^
                /[path='/'].applicationPool:"Wodeyun.Project.Wood.Web"
