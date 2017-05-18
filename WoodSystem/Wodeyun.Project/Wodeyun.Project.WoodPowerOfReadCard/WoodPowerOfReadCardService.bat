path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.WoodPowerOfReadCard.Services"
appcmd delete site "Wodeyun.Project.WoodPowerOfReadCard.Services"

appcmd add apppool /name:"Wodeyun.Project.WoodPowerOfReadCard.Services"
appcmd set apppool "Wodeyun.Project.WoodPowerOfReadCard.Services"^
                   /managedRuntimeVersion:"v4.0"
appcmd add site /name:"Wodeyun.Project.WoodPowerOfReadCard.Services"^
                /bindings:http/*:16014:^
                /physicalPath:"%cd%\Wodeyun.Project.WoodPowerOfReadCard.Services"
appcmd set site "Wodeyun.Project.WoodPowerOfReadCard.Services"^
                /[path='/'].applicationPool:"Wodeyun.Project.WoodPowerOfReadCard.Services"
