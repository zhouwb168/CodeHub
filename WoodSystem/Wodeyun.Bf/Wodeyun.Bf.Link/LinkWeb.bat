path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Bf.Link.Web"
appcmd delete site "Wodeyun.Bf.Link.Web"

appcmd add apppool /name:"Wodeyun.Bf.Link.Web"
appcmd set apppool "Wodeyun.Bf.Link.Web"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Bf.Link.Web"^
                /bindings:http/*:13001:^
                /physicalPath:"%cd%\Wodeyun.Bf.Link.Web"
appcmd set site "Wodeyun.Bf.Link.Web"^
                /[path='/'].applicationPool:"Wodeyun.Bf.Link.Web"
