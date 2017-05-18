path "C:\Windows\System32\inetsrv"

appcmd delete apppool "Wodeyun.Project.WoodPowerOfGps.Services"
appcmd delete site "Wodeyun.Project.WoodPowerOfGps.Services"

appcmd delete apppool "Wodeyun.Project.WoodPowerOfGps.Web"
appcmd delete site "Wodeyun.Project.WoodPowerOfGps.Web"
