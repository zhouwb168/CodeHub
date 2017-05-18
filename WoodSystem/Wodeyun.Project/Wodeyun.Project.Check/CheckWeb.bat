path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.Check.Web"
appcmd delete site "Wodeyun.Project.Check.Web"

appcmd add apppool /name:"Wodeyun.Project.Check.Web"
appcmd set apppool "Wodeyun.Project.Check.Web"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.Check.Web"^
                /bindings:http/*:17003:^
                /physicalPath:"%cd%\Wodeyun.Project.Check.Web"
appcmd set site "Wodeyun.Project.Check.Web"^
                /[path='/'].applicationPool:"Wodeyun.Project.Check.Web"
