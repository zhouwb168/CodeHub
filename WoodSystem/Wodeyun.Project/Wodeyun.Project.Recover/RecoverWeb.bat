path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.Recover.Web"
appcmd delete site "Wodeyun.Project.Recover.Web"

appcmd add apppool /name:"Wodeyun.Project.Recover.Web"
appcmd set apppool "Wodeyun.Project.Recover.Web"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.Recover.Web"^
                /bindings:http/*:17004:^
                /physicalPath:"%cd%\Wodeyun.Project.Recover.Web"
appcmd set site "Wodeyun.Project.Recover.Web"^
                /[path='/'].applicationPool:"Wodeyun.Project.Recover.Web"
