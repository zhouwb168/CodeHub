path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Bf.Token.Services"
appcmd delete site "Wodeyun.Bf.Token.Services"

appcmd add apppool /name:"Wodeyun.Bf.Token.Services"
appcmd set apppool "Wodeyun.Bf.Token.Services"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Bf.Token.Services"^
                /bindings:http/*:11003:^
                /physicalPath:"%cd%\Wodeyun.Bf.Token.Services"
appcmd set site "Wodeyun.Bf.Token.Services"^
                /[path='/'].applicationPool:"Wodeyun.Bf.Token.Services"
