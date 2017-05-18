path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.WoodPowerOfReadCard.Web"
appcmd delete site "Wodeyun.Project.WoodPowerOfReadCard.Web"

appcmd add apppool /name:"Wodeyun.Project.WoodPowerOfReadCard.Web"
appcmd set apppool "Wodeyun.Project.WoodPowerOfReadCard.Web"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.WoodPowerOfReadCard.Web"^
                /bindings:http/*:17014:^
                /physicalPath:"%cd%\Wodeyun.Project.WoodPowerOfReadCard.Web"
appcmd set site "Wodeyun.Project.WoodPowerOfReadCard.Web"^
                /[path='/'].applicationPool:"Wodeyun.Project.WoodPowerOfReadCard.Web"
