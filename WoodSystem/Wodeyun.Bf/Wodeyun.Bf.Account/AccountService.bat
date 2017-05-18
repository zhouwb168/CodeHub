path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Bf.Account.Services"
appcmd delete site "Wodeyun.Bf.Account.Services"

appcmd add apppool /name:"Wodeyun.Bf.Account.Services"
appcmd set apppool "Wodeyun.Bf.Account.Services"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Bf.Account.Services"^
                /bindings:http/*:12003:^
                /physicalPath:"%cd%\Wodeyun.Bf.Account.Services"
appcmd set site "Wodeyun.Bf.Account.Services"^
                /[path='/'].applicationPool:"Wodeyun.Bf.Account.Services"
