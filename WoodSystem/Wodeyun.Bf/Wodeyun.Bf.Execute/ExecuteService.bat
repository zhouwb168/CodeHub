path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Bf.Execute.Services"
appcmd delete site "Wodeyun.Bf.Execute.Services"

appcmd add apppool /name:"Wodeyun.Bf.Execute.Services"
appcmd set apppool "Wodeyun.Bf.Execute.Services"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Bf.Execute.Services"^
                /bindings:http/*:10000:^
                /physicalPath:"%cd%\Wodeyun.Bf.Execute.Services"
appcmd set site "Wodeyun.Bf.Execute.Services"^
                /[path='/'].applicationPool:"Wodeyun.Bf.Execute.Services"