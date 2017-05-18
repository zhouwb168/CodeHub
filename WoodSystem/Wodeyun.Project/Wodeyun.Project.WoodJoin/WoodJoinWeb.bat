path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.WoodJoin.Web"
appcmd delete site "Wodeyun.Project.WoodJoin.Web"

appcmd add apppool /name:"Wodeyun.Project.WoodJoin.Web"
appcmd set apppool "Wodeyun.Project.WoodJoin.Web"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.WoodJoin.Web"^
                /bindings:http/*:17012:^
                /physicalPath:"%cd%\Wodeyun.Project.WoodJoin.Web"
appcmd set site "Wodeyun.Project.WoodJoin.Web"^
                /[path='/'].applicationPool:"Wodeyun.Project.WoodJoin.Web"
