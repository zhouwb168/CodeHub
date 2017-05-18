path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Bf.Role.Services"
appcmd delete site "Wodeyun.Bf.Role.Services"

appcmd add apppool /name:"Wodeyun.Bf.Role.Services"
appcmd set apppool "Wodeyun.Bf.Role.Services"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Bf.Role.Services"^
                /bindings:http/*:12002:^
                /physicalPath:"%cd%\Wodeyun.Bf.Role.Services"
appcmd set site "Wodeyun.Bf.Role.Services"^
                /[path='/'].applicationPool:"Wodeyun.Bf.Role.Services"
