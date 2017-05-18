path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Bf.Account.Web"
appcmd delete site "Wodeyun.Bf.Account.Web"

appcmd add apppool /name:"Wodeyun.Bf.Account.Web"
appcmd set apppool "Wodeyun.Bf.Account.Web"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Bf.Account.Web"^
                /bindings:http/*:13003:^
                /physicalPath:"%cd%\Wodeyun.Bf.Account.Web"
appcmd set site "Wodeyun.Bf.Account.Web"^
                /[path='/'].applicationPool:"Wodeyun.Bf.Account.Web"
