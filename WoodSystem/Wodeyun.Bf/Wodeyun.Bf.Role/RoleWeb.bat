path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Bf.Role.Web"
appcmd delete site "Wodeyun.Bf.Role.Web"

appcmd add apppool /name:"Wodeyun.Bf.Role.Web"
appcmd set apppool "Wodeyun.Bf.Role.Web"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Bf.Role.Web"^
                /bindings:http/*:13002:^
                /physicalPath:"%cd%\Wodeyun.Bf.Role.Web"
appcmd set site "Wodeyun.Bf.Role.Web"^
                /[path='/'].applicationPool:"Wodeyun.Bf.Role.Web"
