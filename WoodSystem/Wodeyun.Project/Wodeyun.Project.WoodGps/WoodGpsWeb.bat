path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.WoodGps.Web"
appcmd delete site "Wodeyun.Project.WoodGps.Web"

appcmd add apppool /name:"Wodeyun.Project.WoodGps.Web"
appcmd set apppool "Wodeyun.Project.WoodGps.Web"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.WoodGps.Web"^
                /bindings:http/*:17015:^
                /physicalPath:"%cd%\Wodeyun.Project.WoodGps.Web"
appcmd set site "Wodeyun.Project.WoodGps.Web"^
                /[path='/'].applicationPool:"Wodeyun.Project.WoodGps.Web"
