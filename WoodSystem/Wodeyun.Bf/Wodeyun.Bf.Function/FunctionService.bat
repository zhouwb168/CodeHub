path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Bf.Function.Services"
appcmd delete site "Wodeyun.Bf.Function.Services"

appcmd add apppool /name:"Wodeyun.Bf.Function.Services"
appcmd set apppool "Wodeyun.Bf.Function.Services"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Bf.Function.Services"^
                /bindings:http/*:12005:^
                /physicalPath:"%cd%\Wodeyun.Bf.Function.Services"
appcmd set site "Wodeyun.Bf.Function.Services"^
                /[path='/'].applicationPool:"Wodeyun.Bf.Function.Services"
