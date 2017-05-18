path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.FullPound.Web"
appcmd delete site "Wodeyun.Project.FullPound.Web"

appcmd add apppool /name:"Wodeyun.Project.FullPound.Web"
appcmd set apppool "Wodeyun.Project.FullPound.Web"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.FullPound.Web"^
                /bindings:http/*:17005:^
                /physicalPath:"%cd%\Wodeyun.Project.FullPound.Web"
appcmd set site "Wodeyun.Project.FullPound.Web"^
                /[path='/'].applicationPool:"Wodeyun.Project.FullPound.Web"
