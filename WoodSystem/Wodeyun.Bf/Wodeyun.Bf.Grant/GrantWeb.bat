path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Bf.Grant.Web"
appcmd delete site "Wodeyun.Bf.Grant.Web"

appcmd add apppool /name:"Wodeyun.Bf.Grant.Web"
appcmd set apppool "Wodeyun.Bf.Grant.Web"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Bf.Grant.Web"^
                /bindings:http/*:13006:^
                /physicalPath:"%cd%\Wodeyun.Bf.Grant.Web"
appcmd set site "Wodeyun.Bf.Grant.Web"^
                /[path='/'].applicationPool:"Wodeyun.Bf.Grant.Web"
