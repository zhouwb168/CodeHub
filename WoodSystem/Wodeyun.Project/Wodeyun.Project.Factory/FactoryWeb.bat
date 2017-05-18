path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.Factory.Web"
appcmd delete site "Wodeyun.Project.Factory.Web"

appcmd add apppool /name:"Wodeyun.Project.Factory.Web"
appcmd set apppool "Wodeyun.Project.Factory.Web"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.Factory.Web"^
                /bindings:http/*:17007:^
                /physicalPath:"%cd%\Wodeyun.Project.Factory.Web"
appcmd set site "Wodeyun.Project.Factory.Web"^
                /[path='/'].applicationPool:"Wodeyun.Project.Factory.Web"
